using eQuotation.DataAccess;
using eQuotation.Entities;
using eQuotation.Models.Admin;
using eQuotation.Models.Home;
using eQuotation.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Controllers
{
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    [Authorize]
    public class HomeController : AppControllerBase
    {
        //public UserManager<AppUser> UserManager { get; private set; }

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public HomeController()
            : base()
        {
            //this.UserManager = new UserManager<AppUser>(new UserStore<AppUser>(new AppDbContext()));
            //this.UserManager.UserValidator = new UserValidator<AppUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };

        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
            : base()
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                var result = _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); ;
                return result;
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        // GET: /Home/
        [AllowAnonymous]
        public ActionResult Index(string ReturnUrl, string module)
        {
            if (ReturnUrl != null && ReturnUrl.Contains("Quotes/QuoteMobileApprove"))
                return Redirect(ReturnUrl);

            if (User.Identity.IsAuthenticated)
            {
                if (ReturnUrl != null)
                    return RedirectToLocal(ReturnUrl);

                var userId = User.Identity.GetUserId();
                var model = new LayoutViewModel();


                return View("_index", model);
            }
            else
                return RedirectToAction("authUser", "home", new { ReturnUrl = ReturnUrl });


        }

        [Authorize]
        public ActionResult DefaultTab()
        {
            var mTab = new LayoutViewModel();
            mTab.GetMenuGroup("Admin");
            var model = new NavigationViewModel(mTab.MenuGroup.CategoryID);
            model.GetValue(null);

            return PartialView("_navigation", model);
        }

        [Authorize]
        public ActionResult QuotesTab()
        {
            var mTab = new LayoutViewModel();
            mTab.GetMenuGroup("Quotes");
            var model = new NavigationViewModel(mTab.MenuGroup.CategoryID);
            model.GetValue(null);

            return PartialView("_navigation", model);
        }

        [Authorize]
        public ActionResult FrontPage()
        {
            return PartialView("_default");
        }

        [AllowAnonymous]
        public ActionResult AuthUser(string ReturnUrl)
        {
            return View("_authUser", new LoginViewModel());
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AuthUser(LoginViewModel model, string returnUrl, string tab)
        {
            if (ModelState.IsValid)
            {
                    if (SSOAuthHelper.IsEmployee(model.Email))
                    { 
                        var loginUser = await UserManager.FindByEmailAsync(model.Email);
                        if (loginUser == null)
                        {
                            ModelState.AddModelError("", "You don't have permission to access eQuotation, please contact: eQ.Helpdesk@advantech.com");
                        }
                        else
                        {
                            String loginTicket = SSOAuthHelper.GetSSOloginTicket(model.Email, model.Password);
                            if (!String.IsNullOrEmpty(loginTicket))
                            {
                                await SignInManager.SignInAsync(loginUser, true, true);
                                try
                                {
                                    DBUtil.dbExecuteScalar("EQ", String.Format("insert into [loginLog] values ('{0}','{1}','{2}','{3}','{4}')", loginTicket, model.Email, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), model.Password, Util.GetClientIP()));
                                }
                                catch { }

                                if (string.IsNullOrEmpty(tab))
                                    return RedirectToLocal(returnUrl);
                                else
                                    return RedirectToLocal(returnUrl + "#" + tab);
                            }
                            else
                                ModelState.AddModelError("", "Password is incorrect.");                       
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sorry, your account is not allowed to login eQuotation.");
                    }                
            }
            // If we got this far, something failed, redisplay form
            //return RedirectToAction("authuser", "home", new { ReturnUrl = returnUrl, tab = tab });
            return View("_authUser", model);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("authuser", "home");
        }

        // POST: /Account/Manage
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();

            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    //make sure that newpassword match
                    if (model.NewPassword == model.ConfirmPassword)
                    {
                        IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            AuthenticationManager.SignOut();
                            return Json(new { succeed = true }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            throw new HttpException(608, "Fail to change password.");
                        }
                    }
                }
                else
                {
                    throw new HttpException(608, "Password is invalid.");
                }
            }

            throw new HttpException(608, "Password is not found.");

        }

        [Authorize]
        public ActionResult ResetPwd(string id)
        {
            string defaultPsw = "advantech";
            return RedirectToAction("ChgPwd", new { usr = id, pwd = defaultPsw });
        }

        [Authorize]
        public async Task<ActionResult> ChgPwd(string usr, string pwd)
        {
            if (HasPassword())
            {
                var user = UserManager.FindById(usr);
                IdentityResult result = await UserManager.RemovePasswordAsync(user.Id);
                if (result.Succeeded)
                {
                    await UserManager.AddPasswordAsync(user.Id, pwd);
                    return Json(new { succeed = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new HttpException(608, "Fail to reset password.");
                }
            }

            throw new HttpException(608, "Failed");
        }

        #region Helpers
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(AppUser user, bool isPersistent)
        {
            var account = new IdentityManager();

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            //user info
            //var extuser = this. UnitOfWork.App.GetByID(user.UserName);
            identity.AddClaim(new Claim(ClaimTypes.UserData, AppContext.AppName));
            identity.AddClaim(new Claim("FullName", AppContext.FullName));

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/#" + returnUrl.Remove(0, 1);

                return Redirect(returnUrl);
            }
            return RedirectToAction("index", "home");

        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }
        #endregion


    }
}