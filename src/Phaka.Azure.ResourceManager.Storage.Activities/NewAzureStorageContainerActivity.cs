using System.Activities;
using System.Threading.Tasks;

namespace Phaka.Azure.ResourceManager.Storage.Activities
{
    public sealed class NewAzureStorageContainerActivity : AzureStorageAccountActivity<StorageContainer>
    {
        protected override async Task<StorageContainer> Execute(ActivityContext context, StorageAccountService service)
        {
            string resourceGroupName = context.GetValue(this.ResourceGroupName);
            string accountName = context.GetValue(this.AccountName);
            string containerName = context.GetValue(this.ContainerName);
            return await service.CreateStorageContainer(resourceGroupName, accountName, containerName);
        }

        [RequiredArgument]
        public InArgument<string> ContainerName { get; set; }

        [RequiredArgument]
        public InArgument<string> AccountName { get; set; }

        [RequiredArgument]
        public InArgument<string> ResourceGroupName { get; set; }
    }

    public sealed class ImportBlob : AzureStorageAccountActivity
    {
        protected override async Task Execute(ActivityContext context, StorageAccountService service)
        {
            string resourceGroupName = context.GetValue(this.ResourceGroupName);
            string accountName = context.GetValue(this.AccountName);
            string containerName = context.GetValue(this.ContainerName);
            await service.Import(resourceGroupName, accountName, containerName);
        }

        [RequiredArgument]
        public InArgument<string> ContainerName { get; set; }

        [RequiredArgument]
        public InArgument<string> AccountName { get; set; }

        [RequiredArgument]
        public InArgument<string> ResourceGroupName { get; set; }
    }
}