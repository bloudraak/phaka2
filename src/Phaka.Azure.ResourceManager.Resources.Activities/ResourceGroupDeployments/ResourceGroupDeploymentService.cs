using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.Management.Resources;
using Microsoft.Azure.Management.Resources.Models;
using Newtonsoft.Json;
using Phaka.Azure.ResourceManager.Activities;
using ProvisioningState = Phaka.Azure.ResourceManager.Activities.ProvisioningState;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroupDeployments
{
    public class ResourceGroupDeploymentService : IDisposable
    {
        public ResourceGroupDeploymentService(string accessToken, Guid subscriptionId)
        {
            if (accessToken == null) throw new ArgumentNullException(nameof(accessToken));
            if (subscriptionId == Guid.Empty)
                throw new ArgumentException("The subscription id isn't valid", nameof(subscriptionId));

            var credentials = new TokenCloudCredentials(subscriptionId.ToString("D"), accessToken);
            Client = new ResourceManagementClient(credentials);
        }

        public ResourceManagementClient Client { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<ResourceGroupDeployment> CreateAsync(string resourceGroupName, string deploymentName = null,
            string template = null, Dictionary<string, object> parameters = null)
        {
            if (resourceGroupName == null) throw new ArgumentNullException(nameof(resourceGroupName));

            if (deploymentName == null)
            {
                // We have two options when dealing with deployment names; we could be very strict and just fail
                // or we can assume a reasonable default.
                deploymentName = DateTime.Now.ToString("s").Replace(":", "-");
            }

            if (template == null)
            {
                // So we may be performing a deployment, but we don't have anything to deploy yet. We could fail
                // or assume that a null template is a "blank template".  We're assuming the latter
                template = Properties.Resources.DefaultResourceGroupTemplate;
            }

            var deployment = new Deployment
            {
                Properties = new DeploymentProperties
                {
                    Mode = DeploymentMode.Incremental,
                    Parameters = SerializeParameters(parameters),
                    Template = template
                }
            };

            try
            {
                var result = await Client.Deployments.CreateOrUpdateAsync(resourceGroupName, deploymentName, deployment);
                return Map(result.Deployment);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task WaitForDeployment(string resourceGroupName, string deploymentName)
        {
            if (resourceGroupName == null) throw new ArgumentNullException(nameof(resourceGroupName));
            if (deploymentName == null) throw new ArgumentNullException(nameof(deploymentName));

            var delay = TimeSpan.FromSeconds(1);
            while (!await Client.IsComplete(resourceGroupName, deploymentName))
            {
                await Task.Delay(delay);
            }
        }

        public async Task<ResourceGroupDeployment> GetDeployment(string resourceGroupName, string deploymentName)
        {
            var deploymentGetResult = await Client.Deployments.GetAsync(resourceGroupName, deploymentName);
            return Map(deploymentGetResult.Deployment);
        }


        private static string SerializeParameters(IDictionary<string, object> parameters)
        {
            // save some compute cycles an memory if there are no parameters
            if (null == parameters || parameters.Count == 0)
            {
                return Properties.Resources.DefaultResourceGroupParameters;
            }

            var transformedParameters = parameters.ToDictionary(parameter => parameter.Key,
                parameter => new TemplateParameter {Value = parameter.Value});
            var serializeObject = JsonConvert.SerializeObject(transformedParameters, Formatting.Indented);
            return serializeObject;
        }

        private ResourceGroupDeployment Map(DeploymentExtended deployment)
        {
            if (deployment == null) return null;
            return new ResourceGroupDeployment
            {
                Id = deployment.Id,
                Name = deployment.Name,
                Duration = deployment.Properties.Duration,
                ProvisioningState = EnumParser.Parse<ProvisioningState>(deployment.Properties.ProvisioningState),
                Timestamp = deployment.Properties.Timestamp,
                CorrelationId = deployment.Properties.CorrelationId,
                Outputs = DeserializeOutput(deployment)
            };
        }

        private Dictionary<string, DeploymentVariable> DeserializeOutput(DeploymentExtended deployment)
        {
            var value = deployment?.Properties?.Outputs;
            if (value == null) return null;
            return JsonConvert.DeserializeObject<Dictionary<string, DeploymentVariable>>(value);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Client.Dispose();
            }
        }

        public async Task<List<ResourceGroupDeployment>> FindDeployments(string resourceGroupName, IResourceGroupDeploymentFilter filter)
        {
            if(null == filter) filter = new DefaultResourceGroupDeploymentFilter();

            var result = await Client.Deployments.ListAsync(resourceGroupName, new DeploymentListParameters());
            return result.Deployments.Select(Map).Where(filter.Match).ToList();
        }

        public async Task DeleteDeployment(string resourceGroupName, string name)
        {
            if (resourceGroupName == null) throw new ArgumentNullException(nameof(resourceGroupName));
            if (name == null) throw new ArgumentNullException(nameof(name));
            await Client.Deployments.DeleteAsync(resourceGroupName, name);
        }

       
    }
}