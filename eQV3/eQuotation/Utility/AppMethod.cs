using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eQuotation.Utility
{
    public class AppMethod : IAppsMethod
    {
        public AppMethod() { }

        public AppMethod(string controller, string method) : this()
        {
            this.Controller = controller;
            this.Action = method;
            this.UriAction = string.Format("/{0}/{1}", controller.Replace("Controller", "").Trim(), method.Trim());
            this.Id = string.Format("/{0}/{1}", controller, method);
        }

        public virtual string Id { get; set; }

        public string UriAction { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }
    }

    public interface IAppsMethod
    {
        string Id { get; set; }
        string UriAction { get; set; }
        string Controller { get; set; }
        string Action { get; set; }
        string Category { get; set; }
        string Description { get; set; }
    }
}