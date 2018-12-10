using Advantech.Myadvantech.Business;
using Advantech.Myadvantech.DataAccess;
using eQuotation.Entities;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Models.Shared
{
    public class ConfiguratorViewModel
    {
        public string UserEmail { get; set; }
        public string BTOId { get; set; }
        public string BTOName { get; set; }
        public string Currency { get; set; }
        public string CurrencySign { get; set; }
        public string ORG { get; set; }
        public string QTY { get; set; }
        public string Language { get; set; }
        public List<SelectListItem> CatalogList;
        public ConfiguratorViewModel()
        {
            this.UserEmail = AppContext.UserEmail;
        }
    }
}