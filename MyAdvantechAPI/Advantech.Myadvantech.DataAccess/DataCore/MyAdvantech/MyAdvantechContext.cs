using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Advantech.Myadvantech.DataAccess
{
    public class MyAdvantechContext : System.Web.SessionState.IRequiresSessionState
    {

        public static MyAdvantechGlobalEntities Current
        {
            get
            {
                if (HttpContext.Current == null)
                    return new MyAdvantechGlobalEntities();
                if (HttpContext.Current.Items["MyAdvantechGlobalEntities"] == null)
                {
                    //MyAdvantechEntities
                    MyAdvantechGlobalEntities _Entity = new MyAdvantechGlobalEntities();
                    HttpContext.Current.Items.Add("MyAdvantechGlobalEntities", _Entity);
                    return _Entity;
                }
                return (MyAdvantechGlobalEntities)HttpContext.Current.Items["MyAdvantechGlobalEntities"];
            }
        }


    }
}
