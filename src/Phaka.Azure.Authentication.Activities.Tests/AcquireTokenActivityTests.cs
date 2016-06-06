using System;
using System.Activities;
using System.Collections.Generic;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using NUnit.Framework;
using Phaka.Azure.Testing.Framework;

namespace Phaka.Azure.Authentication.Activities.Tests
{
    [TestFixture]
    public class AcquireTokenActivityTests
    {
        [Test]
        public void Execute()
        {
            // Arrange
            var target = new AcquireTokenActivity();
            var invoker = new WorkflowInvoker(target);
            var arguments = new Dictionary<string, object>
            {
                {"ClientId", AzureTestContext.ClientId},
                {"ClientSecret", AzureTestContext.ClientSecret},
                {"TenantId", AzureTestContext.TenantId}
            };

            // Act 

            var actual = invoker.Invoke(arguments);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual["Result"]);
        }

        [Test]
        [Ignore("Inconsistences with Azure Authentication")]
        public void ExecuteWithInvalidClientSecret()
        {
            // Arrange
            var target = new AcquireTokenActivity();
            var invoker = new WorkflowInvoker(target);
            var arguments = new Dictionary<string, object>
            {
                {"ClientId", AzureTestContext.ClientId},
                {"ClientSecret", "password"},
                {"TenantId", AzureTestContext.TenantId}
            };
            Exception actual = null;

            // Act 
            try
            {
                invoker.Invoke(arguments);
            }
            catch (AdalServiceException e)
            {
                actual = e;
            }

            // Assert
            Assert.IsInstanceOf<AdalServiceException>(actual);
        }

        [Test]
        public void ExecuteWithInvalidTenantId()
        {
            // Arrange
            var target = new AcquireTokenActivity();
            var invoker = new WorkflowInvoker(target);
            var arguments = new Dictionary<string, object>
            {
                {"ClientId", AzureTestContext.ClientId},
                {"ClientSecret", AzureTestContext.ClientSecret},
                {"TenantId", Guid.Empty}
            };
            Exception actual = null;

            // Act 
            try
            {
                invoker.Invoke(arguments);
            }
            catch (AdalServiceException e)
            {
                actual = e;
            }

            // Assert
            Assert.IsInstanceOf<AdalServiceException>(actual);
        }
    }
}