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
        public static MvcHtmlString ModalFilter(this HtmlHelper html, object model, string vwname, string title, string height, string width)
        {
            //modal container
            var container = new TagBuilder("div");
            container.MergeAttribute("id", vwname);
            container.MergeAttribute("class", "osx-init-container");

            //title container
            var divTitle = new TagBuilder("div");
            divTitle.MergeAttribute("class", "modal-title osx-modal-title");
            divTitle.Attributes["style"] = "height:24px";
            divTitle.InnerHtml = title;

            //close button container
            var divClose = new TagBuilder("div");
            divClose.MergeAttribute("class", "close");

            //close button
            var btnClose = new TagBuilder("a");
            btnClose.MergeAttribute("href", "javascript:void(0)");
            btnClose.MergeAttribute("class", "simplemodal-close");
            //btnClose.Attributes["style"] = "display:none";
            btnClose.InnerHtml = "x";

            //render btnclose inside close-button-container
            divClose.InnerHtml = btnClose.ToString(TagRenderMode.Normal);
            var dataContainer = new TagBuilder("div");
            dataContainer.MergeAttribute("class", "osx-modal-data");
            dataContainer.Attributes["style"] = string.Format("height:{0}; width:{1};", height, width);

            if (model != null) dataContainer.InnerHtml = html.Partial(vwname, model).ToHtmlString();

            container.InnerHtml =
                divTitle.ToString(TagRenderMode.Normal) +
                divClose.ToString(TagRenderMode.Normal) +
                dataContainer.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(container.ToString(TagRenderMode.Normal));
        }

    }
}