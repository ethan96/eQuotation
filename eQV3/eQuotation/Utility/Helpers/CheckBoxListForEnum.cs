﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Utility
{
    public static partial class HtmlExtention
    {

        /// <summary>
        /// https://bitbucket.org/kibiluzbad/xango/src/7acfdb8a1d1f/src/Xango.Mvc/Extensions/HtmlExtensions.cs
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxListForEnum<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            IDictionary<string, object> htmlAttributes = null) where TProperty : struct, IConvertible
        {
            if (!typeof(TProperty).IsEnum)
                throw new ArgumentException("TProperty must be an enumerated type");

            TProperty value = expression.Compile()((TModel)html.ViewContext.ViewData.Model);

            var enumValue = (Enum)Enum.Parse(typeof(TProperty), value.ToString());

            var items = Enum.GetValues(typeof(TProperty))
                            .Cast<Enum>()
                            .Select(c => new SelectListItem
                            {
                                Text = c.ToDescription(), // GetEnumDisplay.ToString(c),
                                Value = c.ToString(),
                                Selected = null != enumValue && enumValue.HasFlag(c)
                            });

            var name = ExpressionHelper.GetExpressionText(expression);

            var sb = new StringBuilder();
            var ul = new TagBuilder("ul");

            ul.MergeAttributes(htmlAttributes);

            foreach (var item in items)
            {
                var id = string.Format("{0}_{1}", name, item.Value);

                var li = new TagBuilder("li");

                var checkBox = new TagBuilder("input");
                checkBox.Attributes.Add("id", id);
                checkBox.Attributes.Add("value", item.Value);
                checkBox.Attributes.Add("name", name);
                checkBox.Attributes.Add("type", "checkbox");
                if (item.Selected)
                    checkBox.Attributes.Add("checked", "checked");

                var label = new TagBuilder("label");
                label.Attributes.Add("for", id);

                label.SetInnerText(item.Text);

                li.InnerHtml = checkBox.ToString(TagRenderMode.SelfClosing) + "\r\n" +
                               label.ToString(TagRenderMode.Normal);

                sb.AppendLine(li.ToString(TagRenderMode.Normal));
            }

            ul.InnerHtml = sb.ToString();

            return new MvcHtmlString(ul.ToString(TagRenderMode.Normal));
        }

    }
}