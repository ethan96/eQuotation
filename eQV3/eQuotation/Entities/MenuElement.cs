using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eQuotation.Entities
{
    public class MenuElement
    {
        [Key]
        public string ID { get; set; }

        public int ProcID { get; set; }

        public string Name { get; set; }

        public string ExtDesc { get; set; }

        public DateTime? Timestamp { get; set; }

        public bool Active { get; set; }

        public bool Default { get; set; }

        public string ClientURL { get; set; }

        [ForeignKey("Group")]
        public string GroupID { get; set; }

        public virtual MenuGroup Group { get; set; }

        [ForeignKey("Category")]
        public string CategoryID { get; set; }

        public virtual MenuCategory Category { get; set; }

        public virtual ICollection<MenuControl> Controls { get; set; }
    }
}