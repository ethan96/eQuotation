using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eQuotation.Utility
{
    public class EnumDisplay : Attribute
    {
        public string Text { get; private set; }

        public bool IsExclude { get; set; }

        public EnumDisplay(string text)
        {
            this.Text = text;
            this.IsExclude = false;
        }

        public EnumDisplay(string text, bool isExcl)
            : this(text)
        {
            this.IsExclude = isExcl;
        }
    }
}