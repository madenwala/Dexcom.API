using System;
using System.Web;

namespace Dexcom.Api.Uwp.Extensions
{
    public static class UriExtensions
    {
        public static string GetQueryParameter(this Uri uri, string parameterName)
        {
            var queryDictionary = HttpUtility.ParseQueryString(uri.Query);
            return Uri.UnescapeDataString(queryDictionary[parameterName]);
        }
    }
}