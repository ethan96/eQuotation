using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eQuotation.Entities
{
    public class AppAction
    {
        public AppAction() { }

        public AppAction(string controller, string method)
            : this()
        {
            this.Controller = controller;
            this.Action = method;
            ////this.UriAction = AppsScheme.GetUriAction(controller, method);
            this.Id = string.Format("/{0}/{1}", controller, method);
        }

        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public string UriAction { get; set; }

        [Required]
        public string Controller { get; set; }

        [Required]
        public string Action { get; set; }

        [Required]
        public string Category { get; set; }

        public string Parent { get; set; }

        public string Description { get; set; }

    }
}