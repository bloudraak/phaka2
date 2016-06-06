using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phaka.Azure.ResourceManager.Activities;

namespace Phaka.Azure.ResourceManager.Storage.Activities
{
    public class StorageAccount
    {
        public ProvisioningState? ProvisioningState { get; set; }
        public AccountType? Sku { get; set; }
        public string Id { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public IDictionary<string, string> Tags { get; set; }

        public override string ToString()
        {
            var tagString = string.Join("; ", Tags.Select(kv => $"{kv.Key}={kv.Value}"));
            return $"ProvisioningState: {ProvisioningState}, Sku: {Sku}, Id: {Id}, Location: {Location}, Name: {Name}, Tags: {{{tagString}}}";
        }
    }
}