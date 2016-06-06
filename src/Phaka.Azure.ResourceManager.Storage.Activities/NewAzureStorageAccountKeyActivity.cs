using System;
using System.Activities;
using System.Threading.Tasks;

namespace Phaka.Azure.ResourceManager.Storage.Activities
{
    public class NewAzureStorageAccountKeyActivity<T> : AzureStorageAccountActivity<T>
    {
        protected override Task<T> Execute(ActivityContext context, StorageAccountService service)
        {
            throw new NotImplementedException();
        }
    }
}