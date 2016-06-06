using System;
using System.Activities;
using System.Threading.Tasks;
using Phaka.Activities;

namespace Phaka.Azure.Authentication.Activities
{
    public sealed class AcquireTokenActivity : AsyncTaskCodeActivity<string>
    {
        [RequiredArgument]
        public InArgument<string> ClientId { get; set; }

        [RequiredArgument]
        public InArgument<string> ClientSecret { get; set; }

        [RequiredArgument]
        public InArgument<Guid> TenantId { get; set; }

        protected override async Task<string> ExecuteAsync(AsyncCodeActivityContext context)
        {
            var clientId = context.GetValue(ClientId);
            var clientSecret = context.GetValue(ClientSecret);
            var tenantId = context.GetValue(TenantId);

            var service = new AuthenticationService();
            return await service.AcquireToken(clientId, clientSecret, tenantId);
        }
    }
}