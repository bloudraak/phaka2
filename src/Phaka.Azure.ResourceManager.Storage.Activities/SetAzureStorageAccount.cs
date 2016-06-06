using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Phaka.Azure.ResourceManager.Storage.Activities
{
    [DisplayName("Update Azure Storage Account")]
    [Description("Updates an Azure Resource Manager Storage Account")]
    public class SetAzureStorageAccount : AzureStorageAccountActivity<StorageAccount>
    {
        [RequiredArgument]
        public InArgument<string> ResourceGroupName { get; set; }

        [RequiredArgument]
        public InArgument<string> AccountName { get; set; }

        public InArgument<IDictionary<string, string>> Tags { get; set; }

        public InArgument<AccountType?> Sku { get; set; }

        protected override async Task<StorageAccount> Execute(ActivityContext context, StorageAccountService service)
        {
            var accountType = context.GetValue(Sku);
            var tags = context.GetValue(Tags);
            var accountName = context.GetValue(AccountName);
            var resourceGroupName = context.GetValue(ResourceGroupName);
            return await service.Update(resourceGroupName, accountName, accountType, tags);
        }
    }
}