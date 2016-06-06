using System;
using System.Activities;
using System.Threading.Tasks;
using Phaka.Activities;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups
{
    public sealed class RemoveAzureResourceGroupActivity : AsyncTaskCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> AccessToken { get; set; }

        [RequiredArgument]
        public InArgument<Guid> SubscriptionId { get; set; }

        [RequiredArgument]
        public InArgument<string> Name { get; set; }

        protected override async Task ExecuteAsync(AsyncCodeActivityContext context)
        {
            var accessToken = context.GetValue(this.AccessToken);
            var subscriptionId = context.GetValue(this.SubscriptionId);
            var service = new ResourceGroupService(accessToken, subscriptionId);

            var name = context.GetValue(this.Name);
            await service.DeleteResourceGroup(name);
        }
    }
}