using System.Web;
using System.Web.Mvc;

namespace eQuotation
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ManageActionAttribute());
            filters.Add(new AuthorizeUserAttribute());
            filters.Add(new ManageErrorAttribute());
        }
    }
}