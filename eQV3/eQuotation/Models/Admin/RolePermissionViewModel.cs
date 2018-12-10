using eQuotation.Entities;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eQuotation.Models.Admin
{
    public class RolePermissionViewModel : ViewModelBase<Object>
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleDesc { get; set; }

        public string RoleRegion { get; set; }

        public string  RoleSector { get; set; }

        public Dictionary<AppAction, bool> Actions { get; set; }

        public RolePermissionViewModel()
        {
            this.Actions = new Dictionary<AppAction, bool>();
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
            throw new NotImplementedException();
        }

        public void GetValue(string roleId, bool isAll = false)
        {
            var role = this.UnitWork.AppRole.GetByID(roleId);

            if (role != null)
            {
                this.RoleId = role.Id;
                this.RoleName = role.Name;
                this.RoleDesc = role.Description;
                this.RoleRegion = role.Region;
                this.RoleSector = role.Sector;

                //get the existing permission
                var actionIds = this.UnitWork.AppRoleAction.Get(x => x.RoleId == role.Id).Select(ra => ra.ActionId).ToArray();
                foreach (var action in this.UnitWork.AppAction.Get())
                {
                    if (actionIds.Contains(action.Id))
                        this.Actions.Add(action, true);
                    else
                    {
                        if (isAll) this.Actions.Add(action, false);
                    }
                }

            }
        }

        public void SynchronizePermission(string roleId)
        {
            var identityManager = new IdentityManager();

            //get new actions/task
            var newActions = identityManager.GetNewActions();

            //insert new actions into database
            foreach (var action in newActions)
                this.UnitWork.AppAction.Insert(action);

            //save record
            this.UnitWork.Save();

            //get permission for the selected role
            GetValue(roleId, true);
        }

        public void SavePermission(string roleId, string actionIds)
        {
            var identityMgr = new IdentityManager();

            //get role-name
            var role = this.UnitWork.AppRole.GetByID(roleId);

            if (role != null)
            {
                //remove all action from the selected role
                foreach (var action in role.Actions.ToList())
                    role.Actions.Remove(action);

                this.UnitWork.Save();

                //add the selected actions to role
                var selectedActions = actionIds.Split((",").ToCharArray());
                if (selectedActions.Count() > 0)
                {
                    foreach (var item in selectedActions)
                        identityMgr.AddActionToRole(role.Name, item);
                }
            }

            //get permission for the selected role
            GetValue(roleId, false);
        }
    }
}