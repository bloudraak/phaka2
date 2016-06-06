using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups;
using Phaka.Azure.Testing.Framework;

namespace Phaka.Azure.ResourceManager.Resources.Activities.Tests
{
    [TestFixture]
    public class NewAzureResourceGroupActivityTests : AzureResourceManagerTestsBase
    {
        [Test]
        public void Execute()
        {
            // Arrange
            var target = new NewAzureResourceGroupActivity();
            var invoker = new WorkflowInvoker(target);
            var tags = new Dictionary<string, string>
            {
                { "Tag1", "Value1" },
                { "Tag2", "Value2" }
            };

            var arguments = new Dictionary<string, object>
            {
                {"AccessToken", Token},
                {"SubscriptionId", AzureTestContext.SubscriptionId},
                {"Name", ResourceGroupName},
                {"Location", "West US"},
                {"Tags", tags}
            };

            // Act 
            var actual = invoker.Invoke(arguments);

            // Assert
            Assert.IsNotNull(actual);
            var resourceGroup = actual["Result"] as ResourceGroup;
            Assert.IsNotNull(resourceGroup != null, "resourceGroup != null");
            Assert.AreEqual("/subscriptions/"+AzureTestContext.SubscriptionId+ "/resourceGroups/" + ResourceGroupName, resourceGroup.Id);
            Assert.AreEqual(ResourceGroupName, resourceGroup.Name);
            Assert.AreEqual(tags, resourceGroup.Tags);
            Assert.AreEqual("Succeeded", resourceGroup.ProvisioningState);
            Assert.AreEqual("westus", resourceGroup.Location);
        }
    }
}