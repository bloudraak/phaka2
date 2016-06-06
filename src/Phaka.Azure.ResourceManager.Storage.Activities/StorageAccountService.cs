using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.Management.Storage;
using Microsoft.Azure.Management.Storage.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Phaka.Azure.ResourceManager.Activities;
using ProvisioningState = Phaka.Azure.ResourceManager.Activities.ProvisioningState;

namespace Phaka.Azure.ResourceManager.Storage.Activities
{
    public sealed class StorageAccountService : IDisposable
    {
        public StorageAccountService(string accessToken, Guid subscriptionId)
        {
            var credential = new TokenCloudCredentials(subscriptionId.ToString("D"), accessToken);
            Client = new StorageManagementClient(credential);
        }

        private StorageManagementClient Client { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Client.Dispose();
            }
        }

        public async Task<StorageAccount> Create(string resourceGroupName,
            string name,
            string location,
            AccountType sku = AccountType.StandardLRS,
            Dictionary<string, string> tags = null)
        {
            var parameters = new StorageAccountCreateParameters
            {
                AccountType = Map(sku),
                Location = location,
                Tags = tags
            };

            var response = await Client.StorageAccounts.CreateAsync(resourceGroupName, name, parameters);

            return Map(response.StorageAccount);
        }

        public async Task<StorageContainer> CreateStorageContainer(string resourceGroupName, string accountName,
            string containerName)
        {
            var response = await Client.StorageAccounts.ListKeysAsync(resourceGroupName, accountName);
            var keyValue = response.StorageAccountKeys.Key1;
            var credentials = new StorageCredentials(accountName, keyValue);
            var storageAccount = new CloudStorageAccount(credentials, accountName, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            return Map(container);
        }

        public async Task<StorageContainer> DeleteStorageContainer(string resourceGroupName, string accountName,
            string containerName)
        {
            var response = await Client.StorageAccounts.ListKeysAsync(resourceGroupName, accountName);
            var keyValue = response.StorageAccountKeys.Key1;
            var credentials = new StorageCredentials(accountName, keyValue);
            var storageAccount = new CloudStorageAccount(credentials, accountName, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            await container.DeleteIfExistsAsync();
            return Map(container);
        }

        private StorageContainer Map(CloudBlobContainer container)
        {
            return new StorageContainer
            {
                Name = container.Name,
                Uri = container.Uri
            };
        }

        private Microsoft.Azure.Management.Storage.Models.AccountType Map(AccountType value)
        {
            return Map<Microsoft.Azure.Management.Storage.Models.AccountType, AccountType>(value);
        }

        private StorageAccount Map(Microsoft.Azure.Management.Storage.Models.StorageAccount storageAccount)
        {
            return new StorageAccount
            {
                ProvisioningState = Map(storageAccount.ProvisioningState),
                Sku = Map(storageAccount.AccountType),
                Id = storageAccount.Id,
                Location = storageAccount.Location,
                Name = storageAccount.Name,
                Tags = storageAccount.Tags
            };
        }

        private AccountType? Map(Microsoft.Azure.Management.Storage.Models.AccountType? value)
        {
            return Map<AccountType, Microsoft.Azure.Management.Storage.Models.AccountType>(value);
        }

        private ProvisioningState? Map(Microsoft.Azure.Management.Storage.Models.ProvisioningState? value)
        {
            return Map<ProvisioningState, Microsoft.Azure.Management.Storage.Models.ProvisioningState>(value);
        }

        private static TResult? Map<TResult, TValue>(TValue? from)
            where TResult : struct
            where TValue : struct
        {
            if (!from.HasValue)
            {
                return null;
            }

            var value = from.Value.ToString();
            return EnumParser.Parse<TResult>(value);
        }

        private static TResult Map<TResult, TValue>(TValue from)
            where TResult : struct
            where TValue : struct
        {
            var value = from.ToString();
            return EnumParser.Parse<TResult>(value);
        }

        public async Task Delete(string resourceGroupName, string accountName)
        {
            await Client.StorageAccounts.DeleteAsync(resourceGroupName, accountName);
        }

        public async Task<bool> CheckNameAvailability(string accountName)
        {
            var result = await Client.StorageAccounts.CheckNameAvailabilityAsync(accountName);
            return result.NameAvailable;
        }

        public async Task<StorageAccount> Update(string resourceGroupName, string accountName, AccountType? sku = null,
            IDictionary<string, string> tags = null)
        {
            var parameters = new StorageAccountUpdateParameters
            {
                AccountType = Map(sku),
                Tags = tags
            };

            var response = await Client.StorageAccounts.UpdateAsync(resourceGroupName, accountName, parameters);
            return Map(response.StorageAccount);
        }

        private Microsoft.Azure.Management.Storage.Models.CustomDomain Map(CustomDomain customDomain)
        {
            if (customDomain == null) return null;
            return new Microsoft.Azure.Management.Storage.Models.CustomDomain
            {
                UseSubDomain = customDomain.UseSubDomain,
                Name = customDomain.Name
            };
        }

        private Microsoft.Azure.Management.Storage.Models.AccountType? Map(AccountType? from)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreateSasToken(string resourceGroupName, string accountName, string containerName,
            StorageContainerPermission permission = StorageContainerPermission.Read, DateTime? expiryTime = null)
        {
            var response = await Client.StorageAccounts.ListKeysAsync(resourceGroupName, accountName);
            var keyValue = response.StorageAccountKeys.Key1;
            var credentials = new StorageCredentials(accountName, keyValue);
            var storageAccount = new CloudStorageAccount(credentials, accountName, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);

            var sasConstraints = new SharedAccessBlobPolicy
            {
                SharedAccessExpiryTime = expiryTime ?? DateTime.UtcNow.AddHours(24),
                Permissions = Map(permission)
            };
            return container.GetSharedAccessSignature(sasConstraints);
        }

        private static SharedAccessBlobPermissions Map(StorageContainerPermission permission)
        {
            switch (permission)
            {
                case StorageContainerPermission.Read:
                    return SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List;

                case StorageContainerPermission.Write:
                    return SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create |
                           SharedAccessBlobPermissions.Add | SharedAccessBlobPermissions.Delete |
                           SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List;

                default:
                    return SharedAccessBlobPermissions.None;
            }
        }

        public async Task Import(string resourceGroupName, string accountName, string containerName, string path,
            string searchPattern, bool recursive = true)
        {
            var response = await Client.StorageAccounts.ListKeysAsync(resourceGroupName, accountName);
            var keyValue = response.StorageAccountKeys.Key1;
            var credentials = new StorageCredentials(accountName, keyValue);
            var storageAccount = new CloudStorageAccount(credentials, accountName, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);

            path = Path.GetFullPath(path);
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            // TODO: Implement this in parallel, so we don't do one file at a time
            var files = Directory.EnumerateFiles(path, searchPattern, searchOption);
            foreach (var file in files)
            {
                var blobName = file.Substring(path.Length+1);
                var blob = container.GetBlockBlobReference(blobName);
                using (var fileStream = File.OpenRead(file))
                {
                    await blob.UploadFromStreamAsync(fileStream);
                }
            }
        }
    }

    public enum StorageContainerPermission
    {
        Read,
        Write
    }
}