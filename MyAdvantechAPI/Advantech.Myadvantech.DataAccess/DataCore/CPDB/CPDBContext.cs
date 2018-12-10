using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Advantech.Myadvantech.DataAccess
{
    public class CPDBContext : System.Web.SessionState.IRequiresSessionState
    {
        public static CheckPointDBEntities1 Current
        {

            get
            {
                if (HttpContext.Current == null) return new CheckPointDBEntities1();
                if (HttpContext.Current.Items["CPDBContext"] == null)
                {
                    CheckPointDBEntities1 _CheckPointDBEntities1 = new CheckPointDBEntities1();
                    HttpContext.Current.Items.Add("CPDBContext", _CheckPointDBEntities1);
                    return _CheckPointDBEntities1;
                }
                return (CheckPointDBEntities1)HttpContext.Current.Items["CPDBContext"];
            }
        }
    }
}
