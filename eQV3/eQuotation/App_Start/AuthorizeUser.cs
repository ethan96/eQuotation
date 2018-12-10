using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace eQuotation
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        private string controllerName;
        private string actionName;
        private bool accepted = true;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var request = httpContext.Request.RequestContext;

            //get controller name
            controllerName = request.RouteData.Values["controller"].ToString();

            //get action name
            actionName = request.RouteData.Values["action"].ToString();

            //get UriAction
            var uriAction = AppReflection.GetUriAction(controllerName, actionName);

            var isAuth = base.AuthorizeCore(httpContext);

            //not authorize
            if (!isAuth) return false;

            if (AppReflection.HasAuthorizeInfo(string.Format("{0}Controller", controllerName), actionName))
            {
                var principal = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var userRoles = principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                //get permitted role-Id
                var permittedRoles = AppContext.UnitOfWork.AppRoleAction.Get(r => r.Action.UriAction == uriAction);

                //check if action is already stored
                if (permittedRoles.Count() == 0) 
                    throw new HttpException(608, string.Format("Action {0} was not registered. Please contact administrator", uriAction.ToUpper()));

                //check if the given role has access to this action
                var roleIdList = permittedRoles.Select(id => id.RoleId).ToArray();
                var testRoles = AppContext.UnitOfWork.AppRole.Get(r => userRoles.Contains(r.Name) && roleIdList.Contains(r.Id));

                if (testRoles.Count() == 0)
                {
                    accepted = false;
                    return false;
                }
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!accepted)
            {
                throw new HttpException((int)HttpStatusCode.Unauthorized, "Unauthorized");
            }
            else
                base.HandleUnauthorizedRequest(filterContext);
        }
    }
}