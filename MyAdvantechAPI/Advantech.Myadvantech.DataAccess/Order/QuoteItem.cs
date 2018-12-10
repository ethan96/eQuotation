using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    [Serializable]
    public class QuoteItem
    {
        public QuoteItem() { }
        
        public QuoteItem(QuotationDetail detail)
        {
            this.id = detail.id;
            this.quoteId = detail.quoteId;
            this.line_No = detail.line_No;
            this.partNo = detail.partNo;
            this.description = detail.description;
            this.qty = detail.qty;
            this.listPrice = detail.listPrice;
            this.unitPrice = detail.unitPrice;
            this.newUnitPrice = detail.newUnitPrice;
            this.category = detail.category;
            this.classABC = detail.classABC;
            this.reqDate = detail.reqDate;
            this.dueDate = detail.dueDate;
            this.inventory = detail.inventory;
            this.HigherLevel = detail.HigherLevel;
            this.ItemType = detail.ItemType;
            this.DisplayUnitPrice = detail.DisplayUnitPrice;
            this.DisplayQty = detail.DisplayQty;
            this.DisplayLineNo = detail.DisplayLineNo;
        }
        
        public int id { get; set; }
        
        public string quoteId { get; set; }
        
        public Nullable<int> line_No { get; set; }
        
        public string partNo { get; set; }
        
        public string description { get; set; }
        
        public Nullable<int> qty { get; set; }
        
        public Nullable<decimal> listPrice { get; set; }
        
        public Nullable<decimal> unitPrice { get; set; }
        
        public Nullable<decimal> newUnitPrice { get; set; }
        //public Nullable<decimal> itp { get; set; }
        //public Nullable<decimal> newItp { get; set; }
        //public string deliveryPlant { get; set; }
        public string category { get; set; }
        
        public string classABC { get; set; }
        //public Nullable<int> rohs { get; set; }
        //public Nullable<int> ewFlag { get; set; }
        public Nullable<System.DateTime> reqDate { get; set; }
        
        public Nullable<System.DateTime> dueDate { get; set; }
        //public Nullable<int> satisfyFlag { get; set; }
        //public Nullable<int> canBeConfirmed { get; set; }
        //public string custMaterial { get; set; }
        
        public Nullable<int> inventory { get; set; }
        //public Nullable<int> oType { get; set; }
        //public string modelNo { get; set; }
        //public string sprNo { get; set; }
        
        public Nullable<int> HigherLevel { get; set; }
        //public string DMF_Flag { get; set; }
        
        public Nullable<int> ItemType { get; set; }
        //public string VirtualPartNo { get; set; }
        //public string DELIVERYGROUP { get; set; }
        //public string ShipPoint { get; set; }
        //public string StorageLoc { get; set; }
        //public string RECFIGID { get; set; }
        public string DisplayUnitPrice { get; set; }
        public string DisplayQty { get; set; }
        public string DisplayLineNo { get; set; }
        //public Nullable<int> SequenceNo { get; set; }
    }
}
