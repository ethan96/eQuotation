using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eQuotation.Entities
{
    public class Country
    {
        [Key]
        public string ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Position { get; set; }

        public bool IsDefault { get; set; }

        //[ForeignKey("Plant")]
        //public string PlantID { get; set; }

        //public virtual Factory Plant { get; set; }

    }
}