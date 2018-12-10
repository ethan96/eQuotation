using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOnlineWall.Entity
{
    public class WallMenu
    {
        public int Id { get; set; }

        public string MenuName { get; set; }

        public int ParentMenuId { get; set; }

        public bool PublishStatus { get; set; }

        public string Url { get; set; }

        public DateTime UpdateDate { get; set; }

        public string Owner { get; set; }
    }
}
