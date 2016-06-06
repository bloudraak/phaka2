using System;
using System.Activities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Phaka.Activities;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroupDeployments
{
    public sealed class NewAzureResourceGroupDeploymentActivity : AzureResourceGroupDeploymentActivityBase<ResourceGroupDeployment>
    {
        public InArgument<Dictionary<string, object>> Parameters { get; set; }

        public InArgument<string> Template { get; set; }

        public InArgument<string> DeploymentName { get; set; }

        [RequiredArgument]
        public InArgument<string> ResourceGroupName { get; set; }

        protected override async Task<ResourceGroupDeployment> Execute(AsyncCodeActivityContext context, ResourceGroupDeploymentService service)
        {
            var resourceGroupName = context.GetValue(ResourceGroupName);
            var deploymentName = context.GetValue(DeploymentName);
            var template = context.GetValue(Template);
            var parameters = context.GetValue(Parameters);

            return await service.CreateAsync(resourceGroupName, deploymentName, template, parameters);
        }
    }
}