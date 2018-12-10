using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eQuotation.Entities
{
    public class MenuControl
    {
        [Key]
        public string ID { get; set; }

        [ForeignKey("Element")]
        public string ElementID { get; set; }

        public virtual MenuElement Element { get; set; }

        public string AppName { get; set; }

        public DateTime? Timestamp { get; set; }

        public string CreatedBy { get; set; }

        public bool Active { get; set; }

        
    }
}