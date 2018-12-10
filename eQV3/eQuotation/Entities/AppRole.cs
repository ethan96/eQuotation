using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eQuotation.Entities
{
    public class AppRole : IdentityRole<string, AppUserRole>
    {
        public AppRole() : base() { }

        //public AppRole(string name) : base(name) { }

        public AppRole(string name, string description, string region, string sector)
            //: this(name)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Name = name;
            this.Description = description;
            this.Region = region;
            this.Sector = sector;
            this.Actions = new HashSet<AppRoleAction>();
        }

        public string Description { get; set; }

        public string Region { get; set; }

        public string Sector { get; set; }

        public virtual ICollection<AppRoleAction> Actions { get; set; }
    }
}