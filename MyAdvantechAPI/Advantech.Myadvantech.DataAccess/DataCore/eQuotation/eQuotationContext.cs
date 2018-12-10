using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Advantech.Myadvantech.DataAccess
{
    public  class eQuotationContext : System.Web.SessionState.IRequiresSessionState
    {
        public static eQEntities Current
        {
            get
            {
                if (HttpContext.Current == null)
                    return new eQEntities();

                if (HttpContext.Current.Items["eQuotationContext"] == null)
                {
                    eQEntities _eQEntities = new eQEntities();
                    HttpContext.Current.Items.Add("eQuotationContext", _eQEntities);
                    return _eQEntities;
                }
                return (eQEntities)HttpContext.Current.Items["eQuotationContext"];
            }
        }
    }
}
