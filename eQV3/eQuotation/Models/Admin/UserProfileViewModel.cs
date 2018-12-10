
using eQuotation.Entities;
using eQuotation.Models.Home;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace eQuotation.Models.Admin
{
    public class UserProfileViewModel : ViewModelBase<Object>
    {
        public string FullName { get; set; }

        public string Department { get; set; }

        public string Company { get; set; }

        public string Location { get; set; }

        public List<string> RoleNames { get; set; }

        public List<AppAction> Permission { get; set; }

        public ManageUserViewModel ManageUser { get; set; }

        private readonly string Region;
        //public string Plant { get; set; }

        public UserProfileViewModel()
        {
            this.Permission = new List<AppAction>();
            this.ManageUser = new ManageUserViewModel();
            this.Region = AppContext.AppRegion;
        }

        public override void Init()
        {
            throw new NotImplementedException();
        }

        public override void SetValue()
        {
            throw new NotImplementedException();
        }

        public override void GetValue(object data)
        {
            var mngr = new IdentityManager();
            var user = UnitWork.AppUser.GetFirst(us => us.UserName == HttpContext.Current.User.Identity.Name);

            this.FullName = string.Format("{0} {1}", user.FirstName, user.LastName);
            this.Department = user.Department;
            this.Company = user.Company;
            this.Location = user.Location;

            //get roles
            this.RoleNames = mngr.GetRoles(HttpContext.Current.User.Identity.Name);

            //get Role-Actions
            var roles = this.UnitWork.AppRole.Get(rl => this.RoleNames.Contains(rl.Name)).ToList();

            //get actions (permission)
            var actions = new List<AppAction>();
            foreach (var role in roles)
            {
                foreach (var act in role.Actions)
                {
                    var action = this.UnitWork.AppAction.GetByID(act.ActionId);
                    actions.Add(action);
                }
            }

            this.Permission = actions.Distinct().ToList();
        }

        public void GetPermission()
        {
            var mngr = new IdentityManager();
            var user = UnitWork.AppUser.GetFirst(us => us.UserName == HttpContext.Current.User.Identity.Name);

            this.FullName = string.Format("{0} {1}", user.FirstName, user.LastName);
            this.Department = user.Department;
            this.Company = user.Company;
            this.Location = user.Location;

            var roles = mngr.GetRolesByUserIdAndRegion(user.Id, this.Region);
            this.RoleNames = roles.Select(r => r.Name).ToList();

            //get actions (permission)
            var actions = new List<AppAction>();
            foreach (var role in roles)
            {
                foreach (var act in role.Actions)
                {
                    var action = this.UnitWork.AppAction.GetByID(act.ActionId);
                    actions.Add(action);
                }
            }

            this.Permission = actions.Distinct().ToList();
        }

        public AppUser GetUserById(string id)
        {
            return this.UnitWork.AppUser.GetByID(id);
        }
    }

}