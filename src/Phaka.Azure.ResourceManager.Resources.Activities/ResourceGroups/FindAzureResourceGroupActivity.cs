using System.Activities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups
{
    public class FindAzureResourceGroupActivity : AzureResourceGroupActivity<List<ResourceGroup>>
    {
        protected override async Task<List<ResourceGroup>> ExecuteAsync(AsyncCodeActivityContext context, ResourceGroupService service)
        {
            var name = context.GetValue(NamePattern);
            var locations = context.GetValue(Locations);
            return await service.FindResourceGroups(new ResourceGroupFilter(name, locations));
        }

        public InArgument<string> NamePattern { get; set; }

        public InArgument<string[]> Locations { get; set; }
    }
}