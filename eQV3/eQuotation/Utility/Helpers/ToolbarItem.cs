using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Utility
{
    public static partial class HtmlExtention
    {
        public static MvcHtmlString ToolbarItem(this HtmlHelper html, string imgUrl, string title, string cls, string strAlt)
        {

            //create div-container
            var container = new TagBuilder("div");
            container.MergeAttribute("class", cls);
            container.MergeAttribute("data-attr", strAlt);

            //create toobarItem image
            var imgCon = new TagBuilder("div");
            var img = new TagBuilder("img");
            img.MergeAttribute("src", imgUrl);
            //img.MergeAttribute("alt", strAlt);
            imgCon.InnerHtml = img.ToString(TagRenderMode.SelfClosing);
            string imgConHtml = imgCon.ToString(TagRenderMode.Normal);

            //create toolbarItem Title
            var txtCon = new TagBuilder("div");
            txtCon.SetInnerText(title);
            string txtConHtml = txtCon.ToString(TagRenderMode.Normal);

            //build toolbar
            container.InnerHtml = imgConHtml + txtConHtml;
            string containerHtml = string.Format("<a data-attr='{0}' href='javascript:void(0)'>{1}</a>", strAlt, container.ToString(TagRenderMode.Normal));

            return MvcHtmlString.Create(containerHtml);
        }
    }
}