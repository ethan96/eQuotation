using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class Freight
    {

        public SAPFreightType Type
        {
            get;
            set;
        }

        public decimal Price
        {
            get;
            set;
        }

        public SAPCurrency Currency
        {
            get;
            set;
        }


    }
}
