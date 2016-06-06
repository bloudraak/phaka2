using System;
using System.Activities;
using System.Threading.Tasks;
using Phaka.Activities;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups
{
    public abstract class AzureResourceGroupActivity<T> : AsyncTaskCodeActivity<T>
    {
        [RequiredArgument]
        public InArgument<string> AccessToken { get; set; }

        [RequiredArgument]
        public InArgument<Guid> SubscriptionId { get; set; }

        protected override async Task<T> ExecuteAsync(AsyncCodeActivityContext context)
        {
            var accessToken = context.GetValue(this.AccessToken);
            var subscriptionId = context.GetValue(this.SubscriptionId);
            var service = new ResourceGroupService(accessToken, subscriptionId);

            return await ExecuteAsync(context, service);
        }

        protected abstract Task<T> ExecuteAsync(AsyncCodeActivityContext context, ResourceGroupService service);
    }
}