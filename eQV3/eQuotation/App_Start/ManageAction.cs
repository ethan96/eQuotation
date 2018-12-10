using eQuotation.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation
{
    public class ManageActionAttribute : ActionFilterAttribute
    {
        private DateTime _startAction;
        private RequestInfo _requestInfo = new RequestInfo();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //get controller
            var controller = (AppControllerBase)filterContext.Controller;

            //set request info
            _requestInfo = RequestInfo.ReadFromHttpContext(filterContext.HttpContext);
            controller.RequestInfo = _requestInfo;

            //log start action
            _startAction = DateTime.Now;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (filterContext.Exception == null)
            {
                //save and destroy instance
                if (AppContext.UnitOfWork != null)
                    AppContext.UnitOfWork.Save();
            }

            //log user request
            var logmgr = new LogEventManager(_requestInfo);
            logmgr.Info("User Request", null, DateTime.Now.Subtract(_startAction).TotalSeconds);

        }
    }


}