using eQuotation.DataAccess;
using eQuotation.Entities;
using eQuotation.Models.Admin;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Controllers
{
    public class AdminController : AppControllerBase
    {
        ////private IdentityContext dbContext = new IdentityContext();
        private AppDbContext appContext = new AppDbContext();

        public AdminController()
            : base()
        {
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult UserProfile()
        {
            var model = new UserProfileViewModel();
            model.GetPermission();
            return PartialView(model);
        }

        [Authorize]
        [AuthorizeInfo("MA02001", "It allows user to review the list of users registered in the application.", "Module Admin", "MA00000")]
        public ActionResult UserManager()
        {
            var model = new AppUserViewModel();
            model.GetUserList();
            return PartialView((model.UsersList));
        }

        [Authorize]
        [AuthorizeInfo("MA02002", "It allows user to review the detail information of users.", "Module Admin", "MA00000")]
        public ActionResult ReviewUser(string id)
        {
            var model = new AppUserViewModel();
            model.GetValueByID(id);
            return PartialView(model.User);
        }

        [Authorize]
        public ActionResult EditProfile(string id)
        {
            var model = new AppUserViewModel();
            model.GetValueByID(id);
            return PartialView(model);
        }

        [HttpPost]
        [Authorize]
        [AuthorizeInfo("MA02003", "It allows user to review and edit the basic information of users.", "Module Admin", "MA00000")]
        public ActionResult EditProfile(AppUserViewModel model)
        {
            model.SetValue();
            return RedirectToAction("usermanager", "admin");
        }

        [Authorize]
        [AuthorizeInfo("MA02004", "It allows user to delete users.", "Module Admin", "MA00000")]
        public ActionResult DeleteProfile(string id)
        {
            var model = new AppUserViewModel();
            model.DeleteProfile(id);
            return RedirectToAction("usermanager", "admin");
            //return Json(new { succeed = true }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult NewUsers()
        {
            var model = new AppUserViewModel();
            return PartialView("NewUsers", model);
        }

        [HttpPost]
        [Authorize]
        [AuthorizeInfo("MA02007", "It allows user to create new users.", "Module Admin", "MA00000")]
        public ActionResult NewUsers(AppUserViewModel model)
        {
            var test = new AppUser();
            var test2 = test.Roles;

            string[] array = model.AccountList.Split(',');
            model.BatchImportAccount(array);


            return RedirectToAction("usermanager", "admin");
        }

        [Authorize]
        public ActionResult NewUser()
        {
            var model = new AppUserViewModel();
            return PartialView("NewUser", model);
        }

        [HttpPost]
        [Authorize]
        [AuthorizeInfo("MA02005", "It allows user to create new users.", "Module Admin", "MA00000")]
        public ActionResult NewUser(AppUserViewModel model)
        {
            if (string.IsNullOrEmpty(model.AccountList))
                model.SetValue();
            else
            {
                string[] array = model.AccountList.Split(',');
                model.BatchImportAccount(array);
            }

            return RedirectToAction("usermanager", "admin");
        }

        [Authorize]
        public ActionResult EditUserRole(string id)
        {
            var model = new UserRoleViewModel();
            model.GetRolesByUser(id);
            return PartialView(model);
        }

        [HttpPost]
        [Authorize]
        [AuthorizeInfo("MA02006", "It allows user to modify the user role.", "Module Admin", "MA00000")]
        public ActionResult EditUserRole(string id, string roleIds)
        {
            var model = new UserRoleViewModel();
            var succeed = model.EditUserRole(id, roleIds);
            return Json(new { succeed = succeed }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AuthorizeInfo("MA03001", "It allows user to review the list of existing user roles and their permission.", "Module Admin", "MA00000")]
        public ActionResult RoleManager()
        {
            var model = new RoleManagerViewModel();
            model.GetValue(model.SelectedRoleId);
            return PartialView(model);
        }

        [Authorize]
        public ActionResult RolePermission(string id)
        {
            var model = new RolePermissionViewModel();
            model.GetValue(id, false);
            return PartialView("Permission", model);
        }

        [HttpPost]
        [Authorize]
        [AuthorizeInfo("MA03002", "It allows user to review and modify new permission (task) for specified role.", "Module Admin", "MA00000")]
        public ActionResult RolePermission(RoleManagerViewModel model)
        {
            return null;
        }

        [Authorize]
        public ActionResult NewRole()
        {
            var model = new AppRoleViewModel();
            return PartialView(model.Role);
        }

        [HttpPost]
        [Authorize]
        [AuthorizeInfo("MA03003", "It allows user to create new roles.", "Module Admin", "MA00000")]
        public ActionResult NewRole(AppRole data)
        {
            var model = new AppRoleViewModel();
            model.SetValue(data);
            return Json(new { succeed = true }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult EditRole(string id)
        {
            var model = new AppRoleViewModel();
            model.GetValueByID(id);
            return PartialView(model);
        }

        [HttpPost]
        [Authorize]
        [AuthorizeInfo("MA03004", "It allows user to edit user roles.", "Module Admin", "MA00000")]
        public ActionResult EditRole(AppRoleViewModel model)
        {
            model.EditRole(model.ID);
            return Json(new { succeed = true }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult DeleteRole(string id)
        {
            var model = new AppRoleViewModel();
            model.GetValueByID(id);
            return PartialView(model);
        }

        [Authorize]
        [HttpPost]
        [AuthorizeInfo("MA03005", "It allows user to delete user roles.", "Module Admin", "MA00000")]
        public ActionResult DeleteRole(AppRoleViewModel model)
        {
            //get role
            model.DeleteRole(model.ID);
            return RedirectToAction("RoleManager", "admin");
            //return Json(new { succeed = true }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AuthorizeInfo("MA03006", "It allows user to synchronize and update new created tasks (actions).", "Module Admin", "MA00000")]
        public ActionResult SynchronizePermission(string id)
        {
            var model = new RolePermissionViewModel();
            model.SynchronizePermission(id);
            return PartialView("Permission", model);
        }

        [Authorize]
        [AuthorizeInfo("MA03007", "It allows user to modify permission for specified roles.", "Module Admin", "MA00000")]
        public ActionResult SavePermission(string id, string actionIds)
        {
            var model = new RolePermissionViewModel();
            model.SavePermission(id, actionIds);

            return PartialView("Permission", model);
        }

        [Authorize]
        [AuthorizeInfo("MA04001", "It allows to review specific feature", "Module Admin", "MA00000")]
        public ActionResult Visibility()
        {
            var model = new VisibilityViewModel("");
            model.GetValue(null);
            return PartialView("Visibility", model);

        }

        [Authorize]
        [AuthorizeInfo("MA04002", "It allows to enable or disable specific function", "Module Admin", "MA00000")]
        public ActionResult SaveControl(string ctrls)
        {
            var model = new VisibilityViewModel("");
            model.UpdateControl(ctrls);

            return RedirectToAction("visibility", "admin");
        }
	}
}