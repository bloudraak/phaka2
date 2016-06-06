using System.Activities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Phaka.Azure.ResourceManager.Activities;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroupDeployments
{
    public sealed class FindAzureResourceGroupDeploymentActivity :
        AzureResourceGroupDeploymentActivityBase<List<ResourceGroupDeployment>>
    {
        [RequiredArgument]
        public InArgument<string> ResourceGroupName { get; set; }

        public InArgument<string> NamePattern { get; set; }

        public InArgument<ProvisioningState[]> ProvisioningStates { get; set; }

        protected override async Task<List<ResourceGroupDeployment>> Execute(AsyncCodeActivityContext context,
            ResourceGroupDeploymentService service)
        {
            var provisioningStates = context.GetValue(ProvisioningStates);
            var namePattern = context.GetValue(NamePattern);
            var resourceGroupName = context.GetValue(ResourceGroupName);
            var filter = new ResourceGroupDeploymentFilter(namePattern, provisioningStates);
            return await service.FindDeployments(resourceGroupName, filter);
        }
    }
}