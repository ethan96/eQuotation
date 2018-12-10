using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOnlineWall.Entity
{
    public partial class WallFile
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public double FileSize { get; set; }

        public string Type { get; set; }

        public DateTime UpdateDate { get; set; }

        public string Owner { get; set; }
    }
}
