using System.Activities;
using System.Collections.Generic;
using NUnit.Framework;
using Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups;
using Phaka.Azure.Testing.Framework;

namespace Phaka.Azure.ResourceManager.Resources.Activities.Tests
{
    [TestFixture]
    public class FindAzureResourceGroupActivityTests : AzureResourceManagerTestsBase
    {
        [Test]
        public void Execute()
        {
            // Arrange
            var target = new FindAzureResourceGroupActivity();
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
            var resourceGroups = actual["Result"];
            Assert.IsInstanceOf<List<ResourceGroup>>(resourceGroups);
        }
    }
}