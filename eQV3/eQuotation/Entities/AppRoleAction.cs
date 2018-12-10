using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eQuotation.Entities
{
    public class AppRoleAction
    {
        [Required]
        public virtual string RoleId { get; set; }

        [Required]
        public virtual string ActionId { get; set; }

        public virtual AppRole Role { get; set; }

        public virtual AppAction Action { get; set; }
    }
}