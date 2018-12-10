using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Advantech.Myadvantech.DataAccess
{
    public class MyAdminContext     : System.Web.SessionState.IRequiresSessionState
    {
        public static MyAdminEntity Current
        {
             
            get
            {
                if (HttpContext.Current == null) return new MyAdminEntity();
                if (HttpContext.Current.Items["MyAdminContext"] == null)
                {
                    MyAdminEntity _MyAdminEntity = new MyAdminEntity();
                    HttpContext.Current.Items.Add("MyAdminContext", _MyAdminEntity);
                    return _MyAdminEntity;
                }
                return (MyAdminEntity)HttpContext.Current.Items["MyAdminContext"];
            }
        }
    }
}
