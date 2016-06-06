using System;
using System.Activities;
using System.Collections.Generic;
using NUnit.Framework;
using Phaka.Azure.ResourceManager.Storage.Activities.Tests.Workflows;
using Phaka.Azure.Testing.Framework;

namespace Phaka.Azure.ResourceManager.Storage.Activities.Tests
{
    [TestFixture]
    public class ScenarioTesting
    {
        private readonly string _resourceGroupName = "test" + Guid.NewGuid().ToString("N");

        [Test]
        public void Scenario1()
        {
            RunScenario<Scenario1>();
        }

        [Test]
        public void Scenario2()
        {
            RunScenario<Scenario2>();
        }

        [Test]
        public void Scenario3()
        {
            RunScenario<Scenario3>();
        }

        [Test]
        public void Scenario4()
        {
            RunScenario<Scenario4>();
        }

        [Test]
        [Ignore("Azure Storage SDK doesn't implement updates yet")]
        public void Scenario5()
        {
            RunScenario<Scenario5>();
        }

        private void RunScenario<TActivity>() where TActivity: Activity, new() 
        {
            // Arrange
            var workflow = new TActivity();
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
