using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Resources;
using Phaka.Azure.ResourceManager.Activities;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroupDeployments
{
    public static class ResourceManagementClientExtensions
    {
        private static readonly ProvisioningState[] CompleteStatus;

        static ResourceManagementClientExtensions()
        {
            CompleteStatus = new[]
            {
                ProvisioningState.Canceled,
                ProvisioningState.Succeeded,
                ProvisioningState.Failed
            };
        }

        public static async Task<bool> IsComplete(this ResourceManagementClient client, string resourceGroupName, string deploymentName)
        {
            var deploymentGetResult = await client.Deployments.GetAsync(resourceGroupName, deploymentName);
            var provisioningState = deploymentGetResult?.Deployment?.Properties?.ProvisioningState;
            return CompleteStatus.Any(s => s.ToString().Equals(provisioningState, StringComparison.OrdinalIgnoreCase));
        }
    }
}