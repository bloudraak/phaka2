using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Phaka.Azure.Authentication.Activities;
using Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups;
using Phaka.Azure.Testing.Framework;

namespace Phaka.Azure.ResourceManager.Resources.Activities.Tests
{
    public abstract class AzureResourceManagerTestsBase
    {
        protected string Token { get; set; }
        protected string ResourceGroupName { get; set; }

        [SetUp]
        public virtual async Task SetUp()
        {
            var service = new AuthenticationService();
            Token = await service.AcquireToken(AzureTestContext.ClientId, AzureTestContext.ClientSecret, AzureTestContext.TenantId);
            ResourceGroupName = "test" + Guid.NewGuid().ToString("N");
        }

        [TearDown]
        public virtual async Task TearDown()
        {
            var service = new ResourceGroupService(Token, AzureTestContext.SubscriptionId);
            await service.DeleteResourceGroup(ResourceGroupName);
        }
    }
}