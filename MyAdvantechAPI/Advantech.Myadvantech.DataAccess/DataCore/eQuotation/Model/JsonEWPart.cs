using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess.DataCore.eQuotation.Model
{
    public class JsonEWPart
    {
        private string _partno;

        [Newtonsoft.Json.JsonProperty("partno")]
        public string Partno
        {
            get { return _partno; }
            set { _partno = value; }

        }

        private int _qty;

        [Newtonsoft.Json.JsonProperty("qty")]
        public int Qty
        {
            get { return _qty; }
            set { _qty = value; }

        }

        private int _ewId;
        [Newtonsoft.Json.JsonProperty("ewId")]
        public int EWId
        {
            get { return _ewId; }
            set { _ewId = value; }

        }
    }
}
