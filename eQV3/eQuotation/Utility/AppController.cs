
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using Microsoft.Practices.Unity;
using System.Globalization;
using eQuotation.DataAccess;

namespace eQuotation.Utility
{
    public class AppController : AppControllerBase
    {
        public IUnitOfWork UnitOfWork { get; set; }

        //protected Plant Plant { get; set; }

        public LogEventManager LogManager { get; set; }

        public AppController()
        {
            //initiate unit of work per request
            if (AppContext.Container != null)
            {
                this.UnitOfWork = AppContext.UnitOfWork;
                this.LogManager = new LogEventManager();
            }
            
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);

            ////initiate plant
            //if (User.Identity.IsAuthenticated)
            //{
            //    this.Plant = (Plant)Enum.Parse(typeof(Plant), AppContext.Plant, true);
            //}
        }

	}
}
