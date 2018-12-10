using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.ViewModels.QuoteForm
{
    [Serializable]
    public class QuoteItemViewModel
    {
        public QuoteItemViewModel()
        {
            this.EWPartOptions = new List<SelectListItem>() { new SelectListItem()
            {
                Text = "Without EW",
                Value = "0"
            } };
        }

        public string QuoteId { get; set; }
        public int LineNo { get; set; }
        public string PartNo { get; set; }

        public string Description { get; set; }
        public decimal ListPrice { get; set; }
        public decimal UnitPrice { get; set; }
        //[DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal QuotingPrice { get; set; }
        public decimal Itp { get; set; }
        public decimal NewItp { get; set; }
        //public decimal Tax { get; set; }
        public int CurrentQty { get; set; }
        public int Qty { get; set; }
        //public string CurrencySign { get; set; }
        public string SprNo { get; set; }
        public int HigherLevel { get; set; }
        public string DMF_Flag { get; set; }
        public int ItemType { get; set; }
        public string DeliveryPlant { get; set; }
        public bool NCNR { get; set; }
        public decimal RecyclingFee { get; set; }
        public string Category { get; set; }
        public string ClassABC { get; set; }
        public int Rohs { get; set; }
        public int EWFlag { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime DueDate { get; set; }
        public int SatisfyFlag { get; set; }
        public int CanBeConfirmed { get; set; }
        public string CusMaterial { get; set; }
        public int Inventory { get; set; }
        public string ModelNo { get; set; }

        public string VirtualPartNo { get; set; }

        public string Recfigid { get; set; }
        public int SequenceNo { get; set; }

        public bool IsEWPart { get; set; }

        public bool EWDrpIsEditable { get; set; }

        public List<SelectListItem> EWPartOptions { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal PostTaxListPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal PostTaxUnitPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal PostTaxQuotingPrice { get; set; }

        public decimal PostTaxBtoParentListPrice { get; set; }
        public decimal PostTaxBtoParentUnitPrice { get; set; }
        public decimal PostTaxBtoParentQuotingPrice { get; set; }
        public decimal PostTaxBtoParentSubTotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}")]
        public Decimal DefaultDiscountRate
        {
            get
            {
                return (this.ListPrice == 0) ? 0 : Decimal.Round((this.ListPrice - this.UnitPrice) / this.ListPrice, 4);
            }

        }

        [DisplayFormat(DataFormatString = "{0:N}")]
        public Decimal SalesDiscountRate
        {
            get
            {
                return (this.UnitPrice == 0) ? 0 : Decimal.Round((this.UnitPrice - this.QuotingPrice) / this.UnitPrice, 4);
            }

        }

        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal PostTaxSubTotal
        {
            get
            {
                return this.PostTaxQuotingPrice * this.Qty;
            }
        }

        public decimal PreTaxSubTotal
        {
            get
            {
                return this.QuotingPrice * this.Qty;
            }
        }
    }
}