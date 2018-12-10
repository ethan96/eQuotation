using eQuotation.Entities;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eQuotation.Models.Admin
{
    public class AppRoleViewModel : ViewModelBase<AppRole>
    {
        public AppRole Role { get; set; }

        public AppRoleViewModel()
        {
            this.Role = new AppRole("","",AppContext.AppRegion,"");
        }

        public override void Init()
        {
            throw new NotImplementedException();
        }

        public override void SetValue() { }

        public void SetValue(AppRole model)
        {
            var succeed = true;

            if (string.IsNullOrEmpty(model.Name))
                throw new HttpException(608, "Role Name is required.");

            var mngr = new IdentityManager();

            //check if role exists
            if (mngr.RoleExists(model.Name))
                throw new HttpException(608, string.Format("Name {0} is already taken.", model.Name));

            succeed = mngr.CreateRole(model.Name,model.Region, model.Sector, model.Description);

            if (!succeed)
                throw new HttpException(608, "Fail to create new Role");
        }

        public override void GetValue(AppRole data)
        {
            this.Role = data;
        }

        public void EditRole(string id)
        {
            //get role
            var role = this.UnitWork.AppRole.GetByID(id);
            role.Sector = this.Role.Sector;
            role.Description = this.Role.Description;
            this.UnitWork.AppRole.Update(role);
        }

        public void DeleteRole(string id)
        {
            var role = this.UnitWork.AppRole.GetByID(id);
            this.UnitWork.AppRole.Delete(role);
        }
    }
}