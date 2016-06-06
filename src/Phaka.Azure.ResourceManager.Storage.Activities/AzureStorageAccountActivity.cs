using System;
using System.Activities;
using System.Threading.Tasks;
using Phaka.Activities;

namespace Phaka.Azure.ResourceManager.Storage.Activities
{
    public abstract class AzureStorageAccountActivity<T> : AsyncTaskCodeActivity<T>
    {
        [RequiredArgument]
        public InArgument<string> AccessToken { get; set; }

        [RequiredArgument]
        public InArgument<Guid> SubscriptionId { get; set; }

        protected override async Task<T> ExecuteAsync(AsyncCodeActivityContext context)
        {
            var subscriptionId = context.GetValue(SubscriptionId);
            var accessToken = context.GetValue(AccessToken);
            using (var service = new StorageAccountService(accessToken, subscriptionId))
            {
                return await Execute(context, service);
            }
        }

        protected abstract Task<T> Execute(ActivityContext context, StorageAccountService service);
    }

    public abstract class AzureStorageAccountActivity : AsyncTaskCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> AccessToken { get; set; }

        [RequiredArgument]
        public InArgument<Guid> SubscriptionId { get; set; }

        protected override async Task ExecuteAsync(AsyncCodeActivityContext context)
        {
            var subscriptionId = context.GetValue(SubscriptionId);
            var accessToken = context.GetValue(AccessToken);

            var service = new StorageAccountService(accessToken, subscriptionId);

            await Execute(context, service);
        }

        protected abstract Task Execute(ActivityContext context, StorageAccountService service);
    }
}