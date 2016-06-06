using System.Activities;
using System.Threading.Tasks;
using Phaka.Activities;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroupDeployments
{
    public sealed class WaitAzureResourceGroupDeploymentActivity : AzureResourceGroupDeploymentActivityBase
    {
        [RequiredArgument]
        public InArgument<string> DeploymentName { get; set; }

        [RequiredArgument]
        public InArgument<string> ResourceGroupName { get; set; }

        protected override async Task Execute(AsyncCodeActivityContext context, ResourceGroupDeploymentService service)
        {
            string resourceGroupName = context.GetValue(ResourceGroupName);
            string deploymentName = context.GetValue(DeploymentName);
            await service.WaitForDeployment(resourceGroupName, deploymentName);
        }
    }
}