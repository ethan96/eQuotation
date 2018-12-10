using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Utility
{
    public static partial class HtmlExtention
    {
        public static MvcHtmlString Modal(this HtmlHelper html, string vwname, string title, string height, string width)
        {
            return ModalFilter(html, null, vwname, title, height, width);
        }

    }
}