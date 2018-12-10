using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Utility
{
    public class RequestInfo
    {
        public string User { get; set; }

        public string Machine { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Area { get; set; }

        public string Url { get; set; }

        public string Params { get; set; }

        public string Method { get; set; }

        public RequestInfo() { }

        public static RequestInfo ReadFromException(ExceptionContext filterContext)
        {
            var info = new RequestInfo();

            info.User = filterContext.HttpContext.User.Identity.Name;
            info.Machine = filterContext.HttpContext.Request.UserHostAddress;
            info.Controller = filterContext.RouteData.Values["controller"].ToString();
            info.Action = filterContext.RouteData.Values["action"].ToString();
            info.Area = filterContext.RouteData.DataTokens["area"] != null ? filterContext.RouteData.DataTokens["area"].ToString() : "Home";
            info.Url = string.Format("/{0}/{1}/{2}", info.Area, info.Controller, info.Action);
            info.Params = filterContext.HttpContext.Request.QueryString.ToString();

            return info;
        }

        public static RequestInfo ReadFromHttpContext(HttpContextBase http)
        {
            var info = new RequestInfo();
            var request = http.Request;

            info.User = http.User.Identity.Name;
            info.Machine = request.UserHostAddress;
            info.Controller = request.RequestContext.RouteData.Values["controller"].ToString();
            info.Action = request.RequestContext.RouteData.Values["action"].ToString();
            info.Area = request.RequestContext.RouteData.DataTokens["area"] != null ?
                            request.RequestContext.RouteData.DataTokens["area"].ToString() : "Home";
            info.Url = string.Format("/{0}/{1}", info.Controller, info.Action);
            info.Params = http.Request.QueryString.ToString();
            info.Method = http.Request.HttpMethod;

            return info;
        }

        public static RequestInfo ReadFromHttpContext(HttpContext http)
        {
            var info = new RequestInfo();
            var request = http.Request;

            info.User = http.User.Identity.Name;
            info.Machine = request.UserHostAddress;
            info.Controller = request.RequestContext.RouteData.Values["controller"].ToString();
            info.Action = request.RequestContext.RouteData.Values["action"].ToString();
            info.Area = request.RequestContext.RouteData.DataTokens["area"] != null ?
                            request.RequestContext.RouteData.DataTokens["area"].ToString() : "Home";
            info.Url = string.Format("/{0}/{1}", info.Controller, info.Action);
            info.Params = request.QueryString.ToString();
            info.Method = request.HttpMethod;

            return info;
        }
    }

}