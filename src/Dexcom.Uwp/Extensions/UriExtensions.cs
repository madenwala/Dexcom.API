using System;
using System.Web;

namespace Dexcom.Uwp
{
    public static class UriExtensions
    {
        public static string GetQueryParameter(this Uri uri, string parameterName)
        {
            var queryDictionary = HttpUtility.ParseQueryString(uri.Query);
            var value = queryDictionary[parameterName];
            if (!string.IsNullOrWhiteSpace(value))
                return Uri.UnescapeDataString(value);
            else
                return null;
        }
    }
}