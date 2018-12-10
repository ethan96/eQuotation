using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace eQuotation.Utility
{
    public static partial class HtmlExtention
    {
        public static MvcHtmlString DropDownListEnum<TEnum>(this HtmlHelper html, string name, TEnum selectedValue, IDictionary<string, object> htmlAttributes = null)
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an enumerated type");

            var values = Enum
                .GetValues(typeof(TEnum))
                .Cast<Enum>()
                .Where(e => e.ToExclude() == false)
                .Select(c => new SelectListItem
                {
                    Text = c.ToDescription(), // GetEnumDisplay.ToString(c),
                    Value = c.ToString(),
                    Selected = c.Equals(selectedValue)
                });

            //IEnumerable<SelectListItem> items =
            //    from itm in values
            //    select new SelectListItem
            //    {
            //        Text = itm.ToString(),
            //        Value = itm.ToString(),
            //        Selected = (itm.Equals(selectedValue))
            //    };

            return html.DropDownList(name, values, htmlAttributes);
        }

    }
}