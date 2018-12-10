using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eQuotation.Entities
{
    public class Product
    {
        [Key]
        public string ID { get; set; }

        public string Name { get; set; }

        public string ExtDesc { get; set; }

    }
}