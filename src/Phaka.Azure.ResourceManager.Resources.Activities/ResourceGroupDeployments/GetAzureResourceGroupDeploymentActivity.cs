using System.Activities;
using System.Threading.Tasks;
using Phaka.Activities;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroupDeployments
{
    public sealed class GetAzureResourceGroupDeploymentActivity : AzureResourceGroupDeploymentActivityBase<ResourceGroupDeployment>
    {
        protected override async Task<ResourceGroupDeployment> Execute(AsyncCodeActivityContext context, ResourceGroupDeploymentService service)
        {
            var resourceGroupName = context.GetValue(ResourceGroupName);
            var deploymentName = context.GetValue(DeploymentName);

            return await service.GetDeployment(resourceGroupName, deploymentName);
        }

        [RequiredArgument]
        public InArgument<string> DeploymentName { get; set; }

        [RequiredArgument]
        public InArgument<string> ResourceGroupName { get; set; }
    }
}