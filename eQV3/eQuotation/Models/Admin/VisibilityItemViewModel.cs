using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eQuotation.Models.Admin
{

    public class VisibilityItemViewModel
    {
        public string CategoryID { get; set; }

        public string Category { get; set; }

        public int ProcIDCat { get; set; }

        public bool IsNewCategory { get; set; }

        public string GroupID { get; set; }

        public string Group { get; set; }

        public int ProcIDGroup { get; set; }

        public bool IsNewGroup { get; set; }

        public string ElementID { get; set; }

        public int ProcIDElem { get; set; }

        public string ElementName { get; set; }

        public bool Enabled { get; set; }

        public bool Default { get; set; }

        public string URL { get; set; }


    }
}