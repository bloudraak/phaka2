using System.Activities;
using System.Threading.Tasks;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroupDeployments
{
    public sealed class RemoveAzureResourceGroupDeploymentActivity : AzureResourceGroupDeploymentActivityBase
    {
        public InArgument<string> Name { get; set; }

        public InArgument<string> ResourceGroupName { get; set; }

        protected override async Task Execute(AsyncCodeActivityContext context, ResourceGroupDeploymentService service)
        {
            var name = context.GetValue(Name);
            var resourceGroupName = context.GetValue(ResourceGroupName);
            await service.DeleteDeployment(resourceGroupName, name);
        }
    }
}