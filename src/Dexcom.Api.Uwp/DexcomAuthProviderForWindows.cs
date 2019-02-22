using Dexcom.Api.Uwp.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace Dexcom.Api.Uwp
{
    public sealed class DexcomAuthProviderForWindows : IDexcomAuthProvider
    {
        public async Task<string> PromptUserForCredentialsAsync(string authenticationUrl, string callbackUrl, CancellationToken ct)
        {
            var result = await WebAuthenticationBroker.AuthenticateAsync(
                       WebAuthenticationOptions.None,
                       new Uri(authenticationUrl),
                       new Uri(callbackUrl));

            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                // Get AUTHORIZATION_CODE parameter from callback URL
                return new Uri(result.ResponseData.ToString()).GetQueryParameter("code");
            }
            else if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {
                Console.WriteLine("HTTP Error returned by AuthenticateAsync() : " + result.ResponseErrorDetail.ToString());
            }

            return null;
        }
    }
}