using Dexcom.Api.Uwp.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace Dexcom.Api.Uwp
{
    public sealed class DexcomAuthProviderForWindows : IDexcomAuthProvider
    {
        public async Task<string> PromptForCredentialsAsync(string authenticationUrl, string callbackUrl, CancellationToken ct)
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

    //public sealed class DexcomWebAuthenticationBroker
    //{
    //    private static string CLIENT_ID;
    //    private static string CLIENT_SECRET;
    //    private static string CALLBACK_URL;
    //    private static bool IS_DEVELOPMENT = false;

    //    public DexcomWebAuthenticationBroker(string clientID, string clientSecret, string callbackUrl, bool isDevelopment = false)
    //    {
    //        CLIENT_ID = clientID;
    //        CLIENT_SECRET = clientSecret;
    //        CALLBACK_URL = callbackUrl;
    //        IS_DEVELOPMENT = isDevelopment;
    //    }

    //    public async Task<AuthenticationResult> PromptForCredentialsAsync(CancellationToken ct)
    //    {
    //        using (var client = new DexcomClient(CLIENT_ID, CLIENT_SECRET, IS_DEVELOPMENT))
    //        {
    //            var authenticationUrl = client.GetUserAuthorizationUrl(CALLBACK_URL);
    //            var result = await WebAuthenticationBroker.AuthenticateAsync(
    //                WebAuthenticationOptions.None,
    //                new Uri(authenticationUrl),
    //                new Uri(CALLBACK_URL));

    //            if (result.ResponseStatus == WebAuthenticationStatus.Success)
    //            {
    //                // Get AUTHORIZATION_CODE parameter from callback URL
    //                var authorizationCode = new Uri(result.ResponseData.ToString()).GetQueryParameter("code");

    //                // Use AUTHORIZATION_CODE to retrieve TOKEN
    //                return await client.GetAccessTokenAsync(authorizationCode, CALLBACK_URL, ct);
    //            }
    //            else if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
    //            {
    //                Console.WriteLine("HTTP Error returned by AuthenticateAsync() : " + result.ResponseErrorDetail.ToString());
    //            }
    //        }

    //        return null;
    //    }
    //}
}
