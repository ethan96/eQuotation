
using eQuotation.DataAccess;
using eQuotation.Entities;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace eQuotation
{
    public class ManageErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            //skip custom error handler if the exception has been handled
            if (filterContext.ExceptionHandled) return;

            //get controller
            var controller = filterContext.RouteData.Values["controller"].ToString();

            //get action
            var action = filterContext.RouteData.Values["action"].ToString();

            //get UriAction
            var uriAction = AppReflection.GetUriAction(controller, action);

            //create error instance
            var errModel = new AppError();

            //set other attributes
            errModel.Client = filterContext.HttpContext.Request.UserHostAddress;
            errModel.UserName = filterContext.HttpContext.User.Identity.Name;
            errModel.Timestamp = DateTime.Now;
            errModel.ControllerName = controller;
            errModel.ActionName = action;

            //get user error message
            //var errMsg = filterContext.Exception.Message.Split(("$$$").ToCharArray());

            if (filterContext.Exception.GetType().Equals(typeof(HttpException)))
            {
                //set error title
                errModel.Code = ((HttpException)filterContext.Exception).GetHttpCode();
                errModel.Title = string.Format("Error: {0} {1}",
                    (((HttpStatusCode)Enum.ToObject(typeof(HttpStatusCode), errModel.Code))).ToString(),
                    errModel.Code.ToString());

                if (errModel.Code == 401)
                {
                    //get the required permission code to access this resource
                    var permission = AppContext.UnitOfWork.AppAction.Get(a => a.UriAction == uriAction);

                    if (permission.Count() > 0)
                    {
                        //handle unauthorized Exception
                        errModel.Message = string.Format("Permission code {0} is required for this action", permission.First().Id);
                        errModel.StackTrace = permission.First().Description;
                    }
                }
                else if (errModel.Code == 608)
                {
                    errModel.Title = string.Format("Error: User Defined Validation {0}", errModel.Code);
                    errModel.Message = filterContext.Exception.Message;
                    errModel.StackTrace = string.Empty;
                }
                else
                {
                    errModel.Message = filterContext.Exception.Message;
                    errModel.StackTrace = filterContext.Exception.StackTrace.ToString();
                }

            }
            else
            {
                errModel.Code = filterContext.Exception.HResult;
                errModel.Title = errModel.Title = string.Format("Error: {0}", filterContext.Exception.HResult.ToString());
                errModel.Message = filterContext.Exception.Message;

                //get InnerException Error Message
                if (filterContext.Exception.InnerException != null)
                    errModel.Message = string.Format("{0} $$$ {1}", errModel.Message, filterContext.Exception.InnerException.Message);

                errModel.StackTrace = filterContext.Exception.StackTrace.ToString();
                errModel.ParamInfo = filterContext.HttpContext.Request.QueryString.ToString();
            }

            //log error into database
            try
            {
                //UnitOfWork.AppsError.Insert(errModel);
                //UnitOfWork.Save();

                var dbContext = new AppDbContext();
                dbContext.AppErrors.Add(errModel);
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errmsg = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                errModel.Message = string.Format("{0} -%- {1}", errModel.Message, string.Concat(errmsg));
            }

            filterContext.Controller.ViewData["ErrorModel"] = errModel;

            //confirm that error is handled
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
        }
    }
}