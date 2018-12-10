using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eQuotation.Entities
{
    public class MenuGroup
    {
        [Key]
        public string ID { get; set; }

        public int ProcID { get; set; }

        public string Name { get; set; }

        public string NameDe { get; set; }

        public string NameTw { get; set; }

        public string ExtDesc { get; set; }

        public DateTime? Timestamp { get; set; }

        public bool Active { get; set; }

        [ForeignKey("Category")]
        public string CategoryID { get; set; }

        public virtual MenuCategory Category { get; set; }

        public virtual ICollection<MenuElement> Elements { get; set; }
    }
}