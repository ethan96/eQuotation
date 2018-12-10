using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class Inventory
    {
        public string Plant { get; set; }
        public string PartNumber { get; set; }
        public DateTime AvailableDate { get; set; }
        public int AvailableQty { get; set; }
    }
}
