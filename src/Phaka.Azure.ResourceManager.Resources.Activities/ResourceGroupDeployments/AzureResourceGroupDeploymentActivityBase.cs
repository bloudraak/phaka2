using System;
using System.Activities;
using System.Threading.Tasks;
using Phaka.Activities;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroupDeployments
{
    public abstract class AzureResourceGroupDeploymentActivityBase<T> : AsyncTaskCodeActivity<T>
    {
        [RequiredArgument]
        public InArgument<Guid> SubscriptionId { get; set; }

        [RequiredArgument]
        public InArgument<string> AccessToken { get; set; }

        protected override async Task<T> ExecuteAsync(AsyncCodeActivityContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var accessToken = context.GetValue(AccessToken);
            var subscriptionId = context.GetValue(SubscriptionId);
            using (var service = new ResourceGroupDeploymentService(accessToken, subscriptionId))
            {
                return await Execute(context, service);
            }
        }

        protected abstract Task<T> Execute(AsyncCodeActivityContext context, ResourceGroupDeploymentService service);
    }

    public abstract class AzureResourceGroupDeploymentActivityBase : AsyncTaskCodeActivity
    {
        [RequiredArgument]
        public InArgument<Guid> SubscriptionId { get; set; }

        [RequiredArgument]
        public InArgument<string> AccessToken { get; set; }

        protected override async Task ExecuteAsync(AsyncCodeActivityContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var accessToken = context.GetValue(AccessToken);
            var subscriptionId = context.GetValue(SubscriptionId);
            var service = new ResourceGroupDeploymentService(accessToken, subscriptionId);

            await Execute(context, service);
        }

        protected abstract Task Execute(AsyncCodeActivityContext context, ResourceGroupDeploymentService service);
    }
}