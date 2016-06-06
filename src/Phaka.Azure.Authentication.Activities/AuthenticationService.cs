using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Phaka.Azure.Authentication.Activities
{
    public class AuthenticationService
    {
        public async Task<string> AcquireToken(string clientId, string clientSecret, Guid tenantId)
        {
            var credential = new ClientCredential(clientId, clientSecret);
            var authenticationContext = new AuthenticationContext($"https://login.windows.net/{tenantId}");
            var result = await authenticationContext.AcquireTokenAsync("https://management.azure.com/", credential);

            // TODO: Handle Authentication errors gracefully
            // TODO: Handle DNS errors gracefully

            return result.AccessToken;
        }
    }
}