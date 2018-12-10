using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Web;

namespace eQuotation.Entities
{
    public class ProductItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string QuoteNo { get; set; }
        public string LineNo { get; set; }
        public string PartNo { get; set; }
        public string Description { get; set; }
        public string List_Price { get; set; }
        public string Unit_Price { get; set; }
        public decimal Quoting_Price { get; set; }
        public int Quantity { get; set; }
        public decimal Sub_total { get; set; }
    }
}