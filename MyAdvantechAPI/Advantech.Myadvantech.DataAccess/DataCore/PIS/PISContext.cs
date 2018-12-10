using Advantech.Myadvantech.DataAccess.Entities;
using System.Web;

namespace Advantech.Myadvantech.DataAccess.Entities
{
    public class PISContext : System.Web.SessionState.IRequiresSessionState
    {
        public static PISEntities Current
        {
            get
            {
                if (HttpContext.Current == null)
                    return new PISEntities();

                if (HttpContext.Current.Items["PISContext"] == null)
                {
                    PISEntities _PISEntities = new PISEntities();
                    HttpContext.Current.Items.Add("PISContext", _PISEntities);
                    return _PISEntities;
                }
                return (PISEntities)HttpContext.Current.Items["PISContext"];
            }
        }
    }
}