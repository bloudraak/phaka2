using System.Activities;
using System.Threading.Tasks;

namespace Phaka.Azure.ResourceManager.Storage.Activities
{
    public sealed class RemoveAzureStorageContainerActivity : AzureStorageAccountActivity<StorageContainer>
    {
        protected override async Task<StorageContainer> Execute(ActivityContext context, StorageAccountService service)
        {
            string resourceGroupName = context.GetValue(this.ResourceGroupName);
            string accountName = context.GetValue(this.AccountName);
            string containerName = context.GetValue(this.ContainerName);
            return await service.DeleteStorageContainer(resourceGroupName, accountName, containerName);
        }

        [RequiredArgument]
        public InArgument<string> ContainerName { get; set; }

        [RequiredArgument]
        public InArgument<string> AccountName { get; set; }

        [RequiredArgument]
        public InArgument<string> ResourceGroupName { get; set; }
    }
}