using System.Activities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups
{
    public sealed class NewAzureResourceGroupActivity : AzureResourceGroupActivity<ResourceGroup>
    {
        [RequiredArgument]
        public InArgument<string> Location { get; set; }

        public InArgument<Dictionary<string, string>> Tags { get; set; }

        [RequiredArgument]
        public InArgument<string> Name { get; set; }

        protected override async Task<ResourceGroup> ExecuteAsync(AsyncCodeActivityContext context, ResourceGroupService service)
        {
            var name1 = context.GetValue(this.Name);
            var location = context.GetValue(this.Location);
            var tags = context.GetValue(this.Tags) ?? new Dictionary<string, string>();
            return await service.CreateResourceGroup(name1, location, tags);
        }
    }
}