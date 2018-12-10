using Advantech.Myadvantech.Business;
using Advantech.Myadvantech.DataAccess.DataCore.eQuotation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Utility
{
    public class SalesRepresentativeModelBinder : IModelBinder
    {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var values = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (values != null)
            {
                // We have specified asterisk (*) as a token delimiter. So
                // the ids will be separated by *. For example "2*3*5"
                var ids = values.AttemptedValue.Split('*');

                // Now that we have the selected ids we could fetch the corresponding
                // salesRepresentative from our datasource
                
                var sales = QuoteBusinessLogic.GetSalesRepresentatives("", AppContext.AppRegion).Where(x => ids.Contains(x.SalesCode)).Select(x => new SalesRepresentative
                {
                    SalesCode = x.SalesCode,
                    Email = x.Email
                }).ToList();
                return sales;
            }
            return Enumerable.Empty<SalesRepresentative>();


            //var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            //if (!string.IsNullOrEmpty(valueProviderResult.AttemptedValue))
            //{
            //    var items = valueProviderResult.AttemptedValue.Split(',');
            //    var result = new List<SalesRepresentative>();
            //    for (var counter = 0; counter < items.Length; counter++)
            //    {
            //        result[counter] = items[counter];
            //    }
            //    return result;
            //}
            //return base.BindModel(controllerContext, bindingContext);
        }

    }
}