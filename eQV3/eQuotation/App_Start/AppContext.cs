using eQuotation.DataAccess;
using eQuotation.Models.Enum;
using eQuotation.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace eQuotation
{
    /// <summary>
    /// Get Application Relative Configure
    /// </summary>
    public class AppContext
    {
        public static readonly string defaultRegion = Region.None.ToString();
        public static readonly string defaultRole = "Guest";

        #region UNITY.MVC
        public static IUnitOfWork UnitOfWork
        {
            get
            {
                if (AppContext.Container != null)
                {
                    if (!AppContext.Container.IsRegistered<IUnitOfWork>())
                    {
                        AppContext.Container.RegisterInstance<IUnitOfWork>(new UnitOfWork());
                        return AppContext.Container.Resolve<IUnitOfWork>();
                    }
                    else
                        return AppContext.Container.Resolve<IUnitOfWork>();
                }
                else
                    return null;
            }
        }

        public static IUnityContainer Container
        {
            get
            {
                var container = (IUnityContainer)HttpContext.Current.Items["perRequestContainer"];

                return (container != null) ? container : null;
            }
        }

        #endregion

        #region USER-INFO
        public static string AppName
        {
            get
            {
                return "eQuotation";

            }
        }

        public static string AppRegion
        {
            get
            {
                if(HttpContext.Current.Session["AppRegion"] == null)
                {

                    var mngr = new IdentityManager();
                    var user = mngr.CurrentUser;
                    if (user!= null)
                    {
                        var region = mngr.GetUserRegions(user.Id).FirstOrDefault();
                        if (region != null)
                            return region;
                        else
                            return defaultRegion;
                    }
                    else
                    {
                        return defaultRegion;
                    }

                }

                return HttpContext.Current.Session["AppRegion"].ToString();
            }

            set
            {
                HttpContext.Current.Session["AppRegion"] = value;
            }
        }

        public static string AppSector
        {
            get
            {
                if (HttpContext.Current.Session["AppSector"] == null)
                {

                    var mngr = new IdentityManager();
                    var user = mngr.CurrentUser;
                    if (user != null)
                    {
                        var sector = mngr.GetUserSectorsByRegion(user.Id, AppContext.AppRegion).FirstOrDefault();
                        if (sector != null)
                            return sector;
                        else
                            return "";
                    }
                    else
                    {
                        return "";
                    }

                }

                return HttpContext.Current.Session["AppSector"].ToString();
            }

            set
            {
                HttpContext.Current.Session["AppSector"] = value;
            }
        }

        public static List<string> UserBelongRoles
        {
            get
            {
                var mngr = new IdentityManager();
                var user = mngr.CurrentUser;
                var roleList = mngr.GetRoleNamesByUserId(user.Id);
                if (roleList == null || roleList.Count == 0)
                    roleList = new List<string> { defaultRole };
                return roleList;
            }

        }

        public static List<string> UserBelongRegion
        {
            get
            {
                var mngr = new IdentityManager();
                var user = mngr.CurrentUser;
                var regionList = mngr.GetUserRegions(user.Id);
                if (regionList == null || regionList.Count == 0)
                    regionList = new List<string> { defaultRegion };
                return regionList;
            }

        }

        public static List<string> UserBelongSectors
        {
            get
            {
                var mngr = new IdentityManager();
                var user = mngr.CurrentUser;
                var sectorList = mngr.GetUserSectorsByRegion(user.Id, AppContext.AppRegion);
                if (sectorList == null || sectorList.Count == 0)
                    sectorList = new List<string> { "All" };
                return sectorList;
            }

        }

        public static List<string> UserBelongRegionSectors
        {
            get
            {
                var regionSectorList = new List<string>(); 
                var mngr = new IdentityManager();
                var user = mngr.CurrentUser;
                var regionList = mngr.GetUserRegions(user.Id);

 
                if (regionList == null || regionList.Count == 0)
                    regionList = new List<string> { defaultRegion };


                foreach (var org in regionList)
                {
                    var sectorList = mngr.GetUserSectorsByRegion(user.Id, org);
                    if (sectorList != null || sectorList.Count > 0)
                    {
                        foreach (var sector in sectorList)
                        {
                            regionSectorList.Add(org + " " + sector);
                        }
                    }
                    else
                        regionSectorList.Add(org + " " + "All");
                }

                return regionSectorList;

            }

        }


        public static string FullName
        {
            get
            {
                if (AppContext.UnitOfWork != null)
                {
                    var user = UnitOfWork.AppUser.GetFirst(x => x.UserName == HttpContext.Current.User.Identity.Name);
                    return (user != null) ? string.Format("{0} {1}", user.FirstName.ToUpper(), user.LastName.ToUpper()) : string.Empty;
                }

                return null;
            }
        }


        public static string UserEmail
        {
            get
            {
                if (AppContext.UnitOfWork != null)
                {
                    var user = UnitOfWork.AppUser.GetFirst(x => x.UserName == HttpContext.Current.User.Identity.Name);
                    return (user != null) ? user.Email : string.Empty;
                }

                return null;
            }
        }

        public static bool IsNewVersion
        {
            get
            {
                switch (AppContext.AppRegion)
                {
                    case "ACN":
                    case "ASG":
                    case "AAU":
                        return true;
                    default:
                        return false;
                }
            }


        }

        #endregion
    }


}