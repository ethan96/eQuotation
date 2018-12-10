
using eQuotation.Entities;
using eQuotation.Models.Enum;
using eQuotation.Utility;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Models.Home
{
    public class LayoutViewModel : ViewModelBase<Object>
    {
        public string AppTitle { get; set; }

        public string UserName { get; set; }


        public string CurrentRegion { get; set; }

        public string CurrentSector { get; set; }

        public List<AppUser> PowerUserList {
            get
            {
                var powerUserlist = new List<AppUser>();
                IdentityManager mngr = new IdentityManager();
                var roles = mngr.GetRolesBynameKeyWord("Power User");
                if (roles != null)
                {
                    foreach (var role in roles)
                    {
                        var users = mngr.GetUsersByRole(role.Id).ToList();
                        powerUserlist.AddRange(users);
                    }
                }
                return powerUserlist;
            }
        }

        public string DefaultRegion { get { return AppContext.defaultRegion.ToString(); } }

        public List<SelectListItem> BelongOrgList { get; set; }
        public List<SelectListItem> BelongSectorList { get; set; }

        public List<SelectListItem> BelongOrgSectorList { get; set; }

        public string Keyword { get; set; }
        public MenuGroup MenuGroup { get; set; }
        public LayoutViewModel()
        {
            this.CurrentRegion = AppContext.AppRegion;
            this.CurrentSector= AppContext.AppSector;

            this.BelongOrgList = AppContext.UserBelongRegion.Select(c => new SelectListItem
            {
                Text = c,
                Value = c,
                Selected = (c == this.CurrentRegion)
            }).ToList();
            

            this.BelongSectorList = AppContext.UserBelongSectors.Select(c => new SelectListItem
            {
                Text = c,
                Value = c,
                Selected = (c == this.CurrentSector)
            }).ToList();


            this.UserName = AppContext.FullName;
            this.Init();         
        }

        public void GetMenuGroup(string tab)
        {
            this.MenuGroup = this.UnitWork.MenuGroup.GetFirst(x => x.Name == tab);
        }

        public override void Init()
        {

        }

        public override void SetValue()
        {
            throw new NotImplementedException();
        }

        public override void GetValue(object data)
        {
            throw new NotImplementedException();
        }
    }

}