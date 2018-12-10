using eQuotation.DataAccess;
using eQuotation.Entities;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Models.Admin
{
    public class RoleManagerViewModel : ViewModelBase<Object>
    {
        public List<SelectListItem> Roles { get; set; }

        public string SelectedRoleId { get; set; }

        public RolePermissionViewModel Permission { get; set; }

        public RoleManagerViewModel()
        {
            this.Roles = new List<SelectListItem>();

            //get the list of existing roles
            foreach(var role in this.UnitWork.AppRole.Get().Where(r=> r.Region == AppContext.AppRegion).OrderBy(r => r.Name))
                this.Roles.Add(new SelectListItem() { Text = role.Name, Value = role.Id });

            if (this.Roles.Count() > 0)
            {
                this.Roles.First().Selected = true;
                this.SelectedRoleId = this.Roles.First().Value;
            }

            this.Permission = new RolePermissionViewModel();
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
            this.Permission = GetPermission(data.ToString());
        }

        public RolePermissionViewModel GetPermission(string roleId, bool isAll = false)
        {
            var permission = new RolePermissionViewModel();
            permission.GetValue(roleId, isAll);

            return permission;
        }
    }




}