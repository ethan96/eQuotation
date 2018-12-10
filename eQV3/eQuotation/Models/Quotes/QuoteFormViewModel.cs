using Advantech.Myadvantech.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Models.Quotes
{
    public class QuoteFormViewModel
    {
        public QuoteFormViewModel()
        {
            this.QuotationMaster = new QuotationMaster();
            this.QuotationExtensionNew = new QuotationExtensionNew();
            //this.QuotationDetails = new List<QuotationDetail>();
            this.QuotationPartners = new List<EQPARTNER>();
            this.SiebelContact = new SIEBEL_CONTACT();
            this.SiebelAccount = new SIEBEL_ACCOUNT();
            this.SiebelActive = new SiebelActive();
            this.optyQuote = new optyQuote();
            this.ContactOptions = new List<SelectListItem>(); 
            this.OrgOptions = new List<SelectListItem>(); 
            this.ParentItemOptions = new List<SelectListItem>();
        }

        public QuotationMaster QuotationMaster { get; set; }
        public QuotationExtensionNew QuotationExtensionNew { get; set; }
        //public List<QuotationDetail> QuotationDetails { get; set; }
        public List<EQPARTNER> QuotationPartners { get; set; }

        public SIEBEL_CONTACT SiebelContact { get; set; }       

        public SIEBEL_ACCOUNT SiebelAccount { get; set; }

        public SiebelActive  SiebelActive { get; set; }

        public optyQuote optyQuote { get; set; }

        public List<SelectListItem> ContactOptions { get; set; }
        public List<SelectListItem> OrgOptions { get; set; }
        //public List<SelectListItem> InsideSalesList { get; set; }
        public List<SelectListItem> ParentItemOptions { get; set; }

        public string AddParts { get; set; }

        public int AddPartQty { get; set; }
        public string SelectedParentPart { get; set; }
    }
}