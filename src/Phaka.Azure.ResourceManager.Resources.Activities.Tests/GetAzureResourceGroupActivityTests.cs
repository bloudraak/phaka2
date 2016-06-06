using System.Activities;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups;
using Phaka.Azure.Testing.Framework;

namespace Phaka.Azure.ResourceManager.Resources.Activities.Tests
{
    [TestFixture]
    public class GetAzureResourceGroupActivityTests : AzureResourceManagerTestsBase
    {
        public override async Task SetUp()
        {
            await base.SetUp();

            var service = new ResourceGroupService(Token, AzureTestContext.SubscriptionId);
            await service.CreateResourceGroup(ResourceGroupName, "West US");
        }

        [Test]
        public void Execute()
        {
            // Arrange
            var target = new GetAzureResourceGroupActivity();
            var invoker = new WorkflowInvoker(target);

            var arguments = new Dictionary<string, object>
            {
                {"AccessToken", Token},
                {"SubscriptionId", AzureTestContext.SubscriptionId},
                {"Name", ResourceGroupName},
            };

            // Act 
            var actual = invoker.Invoke(arguments);

            // Assert
            Assert.IsNotNull(actual);
            var resourceGroup = actual["Result"];
            Assert.IsNotNull(resourceGroup);
        }
    }
}