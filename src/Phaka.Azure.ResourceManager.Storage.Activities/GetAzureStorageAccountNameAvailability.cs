using System.Activities;
using System.Threading.Tasks;

namespace Phaka.Azure.ResourceManager.Storage.Activities
{
    public class GetAzureStorageAccountNameAvailability : AzureStorageAccountActivity<bool>
    {
        protected override async Task<bool> Execute(ActivityContext context, StorageAccountService service)
        {
            var accountName = context.GetValue(AccountName);
            return await service.CheckNameAvailability(accountName);
        }

        public InArgument<string> AccountName { get; set; }
    }
}