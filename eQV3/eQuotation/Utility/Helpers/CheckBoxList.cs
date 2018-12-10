using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Utility
{
    public static partial class HtmlExtention
    {
        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items, IDictionary<string, object> htmlAttributes)
        {
            var output = new StringBuilder();

            foreach (var item in items)
            {
                output.Append("<div class=\"fields\"><label>");
                var checkboxList = new TagBuilder("input");
                checkboxList.MergeAttribute("type", "checkbox");
                checkboxList.MergeAttribute("name", name);
                checkboxList.MergeAttribute("value", item.Value);

                // Check to see if it’s checked
                if (item.Selected)
                    checkboxList.MergeAttribute("checked", "checked");

                // Add any attributes
                if (htmlAttributes != null)
                    checkboxList.MergeAttributes(htmlAttributes);

                checkboxList.SetInnerText(item.Text);

                output.Append(checkboxList.ToString(TagRenderMode.SelfClosing));

                output.Append("&nbsp; " + item.Text + "</label></div>");
            }

            return MvcHtmlString.Create(output.ToString());
        }

    }
}