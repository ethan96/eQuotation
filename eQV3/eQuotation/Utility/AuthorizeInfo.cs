using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace eQuotation.Utility
{
    public class AuthorizeInfoAttribute : DescriptionAttribute
    {
        public AuthorizeInfoAttribute() : base() { }

        public AuthorizeInfoAttribute(string description) : base(description) { }

        public AuthorizeInfoAttribute(string id, string name, string cat, string parent) : base()
        {
            this.Id = id;
            this.Name = name;
            this.Category = cat;
            this.Parent = parent;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Parent { get; set; }
    }
}