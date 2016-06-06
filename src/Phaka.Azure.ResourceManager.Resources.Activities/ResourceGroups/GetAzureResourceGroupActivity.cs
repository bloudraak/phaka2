using System.Activities;
using System.Threading.Tasks;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups
{
    public sealed class GetAzureResourceGroupActivity : AzureResourceGroupActivity<ResourceGroup>
    {
        protected override async Task<ResourceGroup> ExecuteAsync(AsyncCodeActivityContext context, ResourceGroupService service)
        {
            var name = context.GetValue(this.Name);
            return await service.GetResourceGroup(name);
        }

        [RequiredArgument]
        public InArgument<string> Name { get; set; }
    }
}