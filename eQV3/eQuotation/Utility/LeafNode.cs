using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eQuotation.Utility
{
    public class LeafNode
    {
        public string id { get; set; }

        public string label { get; set; }

        public string icon { get; set; }

        public bool inode { get; set; }

        public bool open { get; set; }

        public bool selected { get; set; }

        public List<LeafNode> branch { get; set; }

        public LeafNode()
        {
            this.branch = new List<LeafNode>();
        }
    }
}