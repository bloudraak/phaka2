using System;
using System.Activities;
using System.Collections.Generic;
using NUnit.Framework;
using Phaka.Azure.ResourceManager.Resources.Activities.Tests.Workflows;
using Phaka.Azure.Testing.Framework;

namespace Phaka.Azure.ResourceManager.Resources.Activities.Tests.ResourceGroupDeployments
{
    [TestFixture]
    public class NewAzureResourceGroupDeploymentTests
    {
        private readonly string _resourceGroupName = "test" + Guid.NewGuid().ToString("N");

        [Test]
        public void Scenario1()
        {
            // Arrange
            var workflow = new BlankDeploymentWorkflow();
            var invoker = new WorkflowInvoker(workflow);
            var inputs = new Dictionary<string, object>
            {
                {"ClientId", AzureTestContext.ClientId},
                {"ClientSecret", AzureTestContext.ClientSecret},
                {"SubscriptionId", AzureTestContext.SubscriptionId},
                {"TenantId", AzureTestContext.TenantId},
                {"ResourceGroupName", _resourceGroupName}
            };

            // Act 
            var actual = invoker.Invoke(inputs);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public void Scenario2()
        {
            // Arrange
            var workflow = new StorageAccountWorkflow();
            var invoker = new WorkflowInvoker(workflow);
            var inputs = new Dictionary<string, object>
            {
                {"ClientId", AzureTestContext.ClientId},
                {"ClientSecret", AzureTestContext.ClientSecret},
                {"SubscriptionId", AzureTestContext.SubscriptionId},
                {"TenantId", AzureTestContext.TenantId},
                {"ResourceGroupName", _resourceGroupName}
            };

            // Act 
            var actual = invoker.Invoke(inputs);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public void Scenario3()
        {
            // Arrange
            var workflow = new Scenario3();
            var invoker = new WorkflowInvoker(workflow);
            var inputs = new Dictionary<string, object>
            {
                {"ClientId", AzureTestContext.ClientId},
                {"ClientSecret", AzureTestContext.ClientSecret},
                {"SubscriptionId", AzureTestContext.SubscriptionId},
                {"TenantId", AzureTestContext.TenantId},
                {"ResourceGroupName", _resourceGroupName}
            };

            // Act 
            var actual = invoker.Invoke(inputs);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public void Scenario4()
        {
            // Arrange
            var workflow = new Scenario4();
            var invoker = new WorkflowInvoker(workflow);
            var inputs = new Dictionary<string, object>
            {
                {"ClientId", AzureTestContext.ClientId},
                {"ClientSecret", AzureTestContext.ClientSecret},
                {"SubscriptionId", AzureTestContext.SubscriptionId},
                {"TenantId", AzureTestContext.TenantId},
                {"ResourceGroupName", _resourceGroupName}
            };

            // Act 
            var actual = invoker.Invoke(inputs);

            // Assert
            Assert.IsNotNull(actual);
        }
    }
}