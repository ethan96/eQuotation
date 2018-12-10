using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using eQuotation.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using eQuotation.DataAccess;
using eQuotation.Controllers;

namespace eQuotation
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            //container.RegisterType<IUserStore<AppUser>, UserStore<AppUser>>();

            container.RegisterType<DbContext, AppDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<AppUser>>(new HierarchicalLifetimeManager());
            //container.RegisterType<IUserStore<AppUser>, UserStore<AppUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<AppUser,string>, UserStore<AppUser, AppRole, string, AppUserLogin, AppUserRole, AppUserClaim>> (new HierarchicalLifetimeManager());
            container.RegisterType<HomeController>(new InjectionConstructor());


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}