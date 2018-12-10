using Advantech.Myadvantech.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Advantech.Myadvantech.DataAccess
{
    public class BBeStoreContext : System.Web.SessionState.IRequiresSessionState
    {
        public static BBeStoreEntities Current
        {
            get
            {
                if (HttpContext.Current == null)
                    return new BBeStoreEntities();
                if (HttpContext.Current.Items["BBeStoreEntities"] == null)
                {
                    BBeStoreEntities _Entity = new BBeStoreEntities();
                    HttpContext.Current.Items.Add("BBeStoreEntities", _Entity);
                    return _Entity;
                }
                return (BBeStoreEntities)HttpContext.Current.Items["BBeStoreEntities"];
            }
        }
    }
}
