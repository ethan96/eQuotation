using eQuotation.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Utility
{
    public class AppControllerBase : Controller
    {
        public RequestInfo RequestInfo { get; set; }

        public AppControllerBase() : base() { }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //configure Culture Info
            if (HttpContext.Request.Cookies["CLIENTLN"] != null)
            {
                var lan = HttpContext.Request.Cookies["CLIENTLN"].Value;
                if (!string.IsNullOrEmpty(lan))
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lan);
                    Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
                }
            }

            base.OnActionExecuting(filterContext);

            if (User.Identity.IsAuthenticated)
            {
                var principal = (ClaimsPrincipal)Thread.CurrentPrincipal;

                //set fullname of authenticated user
                ViewBag.FullName = AppContext.FullName;

                //get plant
                ViewBag.AppName = AppContext.AppName;
                
            }

        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            if (filterContext.Controller.ViewData["ErrorModel"] != null)
            {
                var model = (AppError)filterContext.Controller.ViewData["ErrorModel"];

                //return error page
                filterContext.Result = PartialView("_error", model);
            }

            //confirm that error is handled
            filterContext.ExceptionHandled = true;
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }

        protected override void Dispose(bool disposing)
        {
            if (AppContext.UnitOfWork != null)
            {
                AppContext.UnitOfWork.Dispose();
                base.Dispose(disposing);
            }

        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }

        public string GetErrorsFromModelState()
        {
            string messages = string.Join("; ", ModelState.Values
                                                    .SelectMany(x => x.Errors)
                                                    .Select(x => x.ErrorMessage));


            return messages;
        }
    }
}