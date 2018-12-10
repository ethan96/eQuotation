using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Utility.Helpers
{
    public static class SelectListItemsFromDt
    {
        public static List<SelectListItem> SAPCountrySelectListItem(this DataTable table)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "Select...",
                Value = ""
            });
            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row["COUNTRYNAME"].ToString() + " (" + row["COUNTRY"].ToString() + ")",
                    Value = row["COUNTRY"].ToString()
                });
            }

            return list;
        }
    }
}