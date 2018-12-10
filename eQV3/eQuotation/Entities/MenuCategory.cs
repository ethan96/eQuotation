using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eQuotation.Entities
{
    public class MenuCategory
    {
        [Key]
        public string ID { get; set; }

        public int ProcID { get; set; }

        public string Name { get; set; }

        public string ExtDesc { get; set; }

        public DateTime? Timestamp { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<MenuGroup> Groups { get; set; }

        public virtual ICollection<MenuElement> Elements { get; set; }
    }
}