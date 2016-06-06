using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyak.Common;
using Microsoft.Azure;
using Microsoft.Azure.Management.Resources;
using Microsoft.Azure.Management.Resources.Models;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups
{
    public class ResourceGroupService
    {
        public ResourceGroupService(string accessToken, Guid subscriptionId)
        {
            if (accessToken == null) throw new ArgumentNullException(nameof(accessToken));
            AccessToken = accessToken;
            SubscriptionId = subscriptionId;
        }

        public string AccessToken { get; set; }

        public Guid SubscriptionId { get; set; }

        public Task<ResourceGroup> CreateResourceGroup(string name, string location)
        {
            return CreateResourceGroup(name, location, new Dictionary<string, string>());
        }

        public async Task<ResourceGroup> CreateResourceGroup(string name, string location,
            IDictionary<string, string> tags)
        {
            var subscriptionId = SubscriptionId.ToString("D");
            SubscriptionCloudCredentials credentials = new TokenCloudCredentials(subscriptionId, AccessToken);
            using (var client = new ResourceManagementClient(credentials))
            {
                var response = await client.ResourceGroups.CreateOrUpdateAsync(name, new ResourceGroupExtended
                {
                    Location = location,
                    Tags = tags
                });

                return Map(response.ResourceGroup);
            }
        }

        public async Task DeleteResourceGroup(string name)
        {
            try
            {
                var subscriptionId = SubscriptionId.ToString("D");
                SubscriptionCloudCredentials credentials = new TokenCloudCredentials(subscriptionId, AccessToken);
                using (var client = new ResourceManagementClient(credentials))
                {
                    await client.ResourceGroups.DeleteAsync(name);
                }
            }
            catch (CloudException e)
            {
                if (e.Error != null && e.Error.Code != "ResourceGroupNotFound")
                {
                    throw;
                }
            }
        }

        private static ResourceGroup Map(ResourceGroupExtended resourceGroup)
        {
            if (resourceGroup == null) throw new ArgumentNullException(nameof(resourceGroup));
            return new ResourceGroup
            {
                Id = resourceGroup.Id,
                Name = resourceGroup.Name,
                Location = resourceGroup.Location,
                Tags = resourceGroup.Tags,
                ProvisioningState = resourceGroup.ProvisioningState
            };
        }

        public async Task<List<ResourceGroup>> FindResourceGroups(IResourceGroupFilter filter)
        {
            if (null == filter)
            {
                filter = new DefaultResourceGroupFilter();
            }
            var subscriptionId = SubscriptionId.ToString("D");
            SubscriptionCloudCredentials credentials = new TokenCloudCredentials(subscriptionId, AccessToken);
            using (var client = new ResourceManagementClient(credentials))
            {
                var parameters = new ResourceGroupListParameters();
                var response = await client.ResourceGroups.ListAsync(parameters);

                return (from resourceGroup in response.ResourceGroups
                    where filter.Match(resourceGroup)
                    select Map(resourceGroup)).ToList();
            }
        }

        public async Task<ResourceGroup> GetResourceGroup(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var subscriptionId = SubscriptionId.ToString("D");
            SubscriptionCloudCredentials credentials = new TokenCloudCredentials(subscriptionId, AccessToken);
            using (var client = new ResourceManagementClient(credentials))
            {
                var response = await client.ResourceGroups.GetAsync(name);
                return Map(response.ResourceGroup);
            }
        }

        public async Task<ResourceGroup> UpdateResourceGroup(string name, IDictionary<string, string> tags, string location, string newName)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var subscriptionId = SubscriptionId.ToString("D");
            SubscriptionCloudCredentials credentials = new TokenCloudCredentials(subscriptionId, AccessToken);
            using (var client = new ResourceManagementClient(credentials))
            {
                var parameters = new ResourceGroupExtended()
                {
                    Tags = tags,
                    Location = location,
                    Name = newName
                };
                var response = await client.ResourceGroups.PatchAsync(name, parameters);
                return Map(response.ResourceGroup);
            }
        }
    }
}