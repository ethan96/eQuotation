using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

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
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListForEnum<TModel, TProperty>(this HtmlHelper<TModel> html,
                        Expression<Func<TModel, TProperty>> expression,
                        string optionLabel = null,
                        IDictionary<string, object> htmlAttributes = null) where TProperty : struct, IConvertible
        {
            if (!typeof(TProperty).IsEnum)
                throw new ArgumentException("TProperty must be an enumerated type");

            var values = Enum
                .GetValues(typeof(TProperty))
                .Cast<Enum>()
                .Where(e => e.ToExclude() == false)
                .Select(c => new SelectListItem
                {
                    Text = c.ToDescription(), // GetEnumDisplay.ToString(c),
                    Value = c.ToString()
                });

            return html.DropDownListFor(expression, values, optionLabel, htmlAttributes);
        }

    }
}