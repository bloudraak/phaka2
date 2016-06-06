using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Phaka.Activities;

namespace Phaka.Azure.ResourceManager.Storage.Activities
{
    public sealed class NewAzureStorageAccountActivity : AzureStorageAccountActivity<StorageAccount>
    {
        [DefaultValue(AccountType.StandardLRS)]
        public InArgument<AccountType> Sku { get; set; }

        [RequiredArgument]
        public InArgument<string> ResourceGroupName { get; set; }

        public InArgument<Dictionary<string, string>> Tags { get; set; }

        [RequiredArgument]
        public InArgument<string> Location { get; set; }

        [RequiredArgument]
        public InArgument<string> Name { get; set; }

        protected override async Task<StorageAccount> Execute(ActivityContext context, StorageAccountService service)
        {
            var name = context.GetValue(Name);
            var resourceGroupName = context.GetValue(ResourceGroupName);
            var location = context.GetValue(Location);
            var sku = context.GetValue(Sku);
            var tags = context.GetValue(Tags) ?? new Dictionary<string, string>();

            return await service.Create(resourceGroupName, name, location, sku, tags);
        }
    }
}