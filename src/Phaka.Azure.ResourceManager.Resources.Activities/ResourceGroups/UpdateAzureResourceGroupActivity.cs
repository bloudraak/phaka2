using System.Activities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups
{
    public sealed class UpdateAzureResourceGroupActivity : AzureResourceGroupActivity<ResourceGroup>
    {
        public InArgument<string> Location { get; set; }

        public InArgument<Dictionary<string, string>> Tags { get; set; }

        [RequiredArgument]
        public InArgument<string> ExistingName { get; set; }

        public InArgument<string> NewName { get; set; }

        protected override async Task<ResourceGroup> ExecuteAsync(AsyncCodeActivityContext context, ResourceGroupService service)
        {
            var existingName = context.GetValue(this.ExistingName);
            var newName = context.GetValue(this.NewName);
            var location = context.GetValue(this.Location);
            var tags = context.GetValue(this.Tags) ?? new Dictionary<string, string>();
            return await service.UpdateResourceGroup(existingName, tags, location, newName);
        }
    }
}