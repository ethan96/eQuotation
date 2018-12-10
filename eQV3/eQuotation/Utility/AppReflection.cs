using eQuotation.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Utility
{
    public static class AppReflection
    {
        public static List<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(
                type => type.IsSubclassOf(typeof(T))).ToList();
        }

        public static List<string> GetControllerNames()
        {
            List<string> controllerNames = new List<string>();
            GetSubClasses<AppControllerBase>().ForEach(
                type => controllerNames.Add(type.Name));

            return controllerNames;
        }

        public static IEnumerable<AppAction> GetActions()
        {
            var actions = new List<AppAction>();

            GetSubClasses<AppControllerBase>().ForEach(
                type =>
                {
                    var methods = type.GetMethods().Where(p => p.ReturnType == typeof(ActionResult) 
                                    || p.ReturnType == typeof(Task)
                                    || p.ReturnType == typeof(Task<ActionResult>));

                    foreach (var info in methods)
                    {
                        if (info.GetCustomAttribute<AuthorizeAttribute>(true) != null)
                        {
                            var attribute = info.GetCustomAttribute<AuthorizeInfoAttribute>(true);
                            if (attribute != null)
                            {
                                var method = new AppAction(type.Name, info.Name)
                                    {
                                        Category = attribute.Category,
                                        Description = attribute.Name,
                                        Id = attribute.Id,
                                        Parent = attribute.Parent,
                                        UriAction = GetUriAction(type.Name, info.Name)
                                    };

                                actions.Add(method);
                            }
                        }
                    }
                }
            );

            return (IEnumerable<AppAction>) actions;
        }

        public static bool HasAuthorizeInfo(string controller, string action)
        {
            int count = 0;

            GetSubClasses<AppControllerBase>().ForEach(
                type =>
                {
                    if (type.Name.ToUpper().IndexOf(controller.ToUpper()) >= 0)
                    {
                        var methods = type.GetMethods().Where(p => p.ReturnType == typeof(ActionResult)
                                        || p.ReturnType == typeof(Task)
                                        || p.ReturnType == typeof(Task<ActionResult>));

                        foreach (var info in methods)
                        {
                            if (info.Name.ToUpper() == action.ToUpper())
                            {
                                if (info.GetCustomAttribute<AuthorizeAttribute>(true) != null)
                                {
                                    if (info.GetCustomAttribute<AuthorizeInfoAttribute>(true) != null)
                                    {
                                        count++;
                                        break;
                                    }
                               }
                            }
                        }
                    }
                }
            );

            return (count > 0);
        }

        public static string GetUriAction(string controller, string action)
        {
            return string.Format("/{0}/{1}", controller.Replace("Controller", "").Trim(), action.Trim());
        }
    }

}