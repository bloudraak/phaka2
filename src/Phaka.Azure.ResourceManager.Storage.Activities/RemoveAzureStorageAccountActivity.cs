using System.Activities;
using System.Threading.Tasks;

namespace Phaka.Azure.ResourceManager.Storage.Activities
{
    public class RemoveAzureStorageAccountActivity : AzureStorageAccountActivity
    {
        protected override async Task Execute(ActivityContext context, StorageAccountService service)
        {
            string resourceGroupName = context.GetValue(this.ResourceGroupName);
            string accountName = context.GetValue(this.AccountName);
            await service.Delete(resourceGroupName, accountName);
        }

        [RequiredArgument]
        public InArgument<string> ResourceGroupName { get; set; }

        [RequiredArgument]
        public InArgument<string> AccountName { get; set; }
    }
}