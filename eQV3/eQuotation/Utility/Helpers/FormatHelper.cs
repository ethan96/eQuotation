using eQuotation.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;

namespace eQuotation.Utility
{
    public static class FormatHelper
    {
        public static string CorrectPartnerAddressFormat(string address)
        {
            if (address != null && address.Length > 50)
                    return address.Substring(0, 50);
            return address;
        }

    }



}