using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eQuotation.Utility
{
    public class Util
    {
        public static Boolean isTesting()
        {
            if (HttpContext.Current.Request.Url.Port != 8600 && HttpContext.Current.Request.Url.Port != 80)
                return true;

            return false;
        }

        public static bool IsNumeric(object expression)
        {
            if (expression == null)
                return false;

            double number;
            return Double.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), System.Globalization.NumberStyles.Any, NumberFormatInfo.InvariantInfo, out number);
        }

        public static string GetClientIP()
        {
            string _ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (_ip == null || _ip == "" || _ip.ToLower() == "unknown")
            {
                _ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return _ip;
        }

        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            string newUrl = serverUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                "://" + originalUri.Authority + newUrl;
            return newUrl;
        }

        public static string CurrentUrl()
        {
            return System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority;
        }

    }
}