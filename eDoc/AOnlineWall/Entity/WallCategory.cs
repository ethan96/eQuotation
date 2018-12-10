using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOnlineWall.Entity
{
    public partial class WallCategory
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public int ParentCategoryId { get; set; }

        public DateTime UpdateDate { get; set; }

        public string Owner { get; set; }
    }
}
