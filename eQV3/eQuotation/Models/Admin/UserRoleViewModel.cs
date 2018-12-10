using eQuotation.Entities;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace eQuotation.Models.Admin
{
    public class UserRoleViewModel : ViewModelBase<Object>
    {
        public string UserName { get; set; }

        public string UserId { get; set; }

        public string FullName { get; set; }

        private readonly string Region;

        public Dictionary<AppRole, bool> RoleList { get; set; }

        public UserRoleViewModel()
        {
            this.RoleList = new Dictionary<AppRole, bool>();
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

        public override void GetValue(Object data)
        {
            throw new NotImplementedException();
        }

        public void GetRolesByUser(string id)
        {
            var user = this.UnitWork.AppUser.GetByID(id);

            if (user != null)
            {
                this.UserId = user.Id;
                this.UserName = user.UserName;
                this.FullName = string.Format("{0} {1}", user.FirstName, user.LastName);

                //get roleIds for corrresponding user
                var identity = new IdentityManager();
                var roleNames = identity.GetRoles(user.UserName);
                
                foreach (var role in this.UnitWork.AppRole.Get().Where(r=>r.Region == this.Region))
                {
                    
                    if (roleNames.Contains(role.Name))
                        this.RoleList.Add(role, true);
                    else
                        this.RoleList.Add(role, false);
                }
            }
        }

        public bool EditUserRole(string id, string roleIds)
        {
            var succeed = true;

            //get user instance
            var user = this.UnitWork.AppUser.GetByID(id);

            //create instance of identity manager
            var mngr = new IdentityManager();

            //clear all existing roles of corresponding user by region
            if (!mngr.ClearUserRoles(id, this.Region))
                throw new HttpException(608, "Fail to clear existing roles.");

            //get selected role-Ids
            if (!string.IsNullOrEmpty(roleIds))
            {
                var newRoles = roleIds.Split((",").ToCharArray());
                var failRoles = new List<string>();

                if (newRoles.Count() > 0)
                {
                    foreach (var newRole in newRoles)
                    {
                        succeed = mngr.AddUserToRole(id, newRole);
                        if (!succeed) failRoles.Add(newRole);
                    }
                }

                if (failRoles.Count() > 0)
                    throw new HttpException(608, string.Format("Role [{0}] could not be assigned.", string.Join(",", failRoles)));
            }

            return succeed;
        }
    }
}