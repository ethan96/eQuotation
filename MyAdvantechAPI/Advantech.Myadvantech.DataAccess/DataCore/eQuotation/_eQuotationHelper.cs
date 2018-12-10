using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class _eQuotationHelper
    {
        protected eQEntities context;

        public _eQuotationHelper()
        {
            //context = new eQEntities();
           context = eQuotationContext.Current;
        }
    }
}
