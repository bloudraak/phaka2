using System;
using System.Activities;
using System.Threading.Tasks;

namespace Phaka.Azure.ResourceManager.Storage.Activities
{
    public sealed class NewAzureStorageContainerSasTokenActivity : AzureStorageAccountActivity<string>
    {
        [RequiredArgument]
        public InArgument<string> ContainerName { get; set; }

        [RequiredArgument]
        public InArgument<string> AccountName { get; set; }

        [RequiredArgument]
        public InArgument<string> ResourceGroupName { get; set; }

        protected override async Task<string> Execute(ActivityContext context, StorageAccountService service)
        {
            var resourceGroupName = context.GetValue(ResourceGroupName);
            var accountName = context.GetValue(AccountName);
            var containerName = context.GetValue(ContainerName);
            var expiryTime = context.GetValue(this.ExpiryTime);
            return await service.CreateSasToken(resourceGroupName, 
                accountName, 
                containerName, StorageContainerPermission.Read, 
                expiryTime);
        }

        public InArgument<DateTime?> ExpiryTime { get; set; }
    }
}