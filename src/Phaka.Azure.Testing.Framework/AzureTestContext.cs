using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phaka.Azure.Testing.Framework
{
    public class AzureTestContext
    {
        // You need to replace these with your own
        public static Guid SubscriptionId = new Guid("99326b3d-1862-4ddd-923c-a36c4164a8c9");
        public const string ClientId = "a7b66e44-4890-4b09-a96a-fc99dba98d5a";
        public const string ClientSecret = "password";
        public static Guid TenantId = new Guid("6791a5df-e49b-46b1-ad00-44fd5b57143a");
    }
}
