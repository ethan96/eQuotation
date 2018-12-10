
using eQuotation.DataAccess;
using eQuotation.Entities;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Models.Admin
{
    public class AppUserViewModel : ViewModelBase<AppUser>
    {
        public AppUser User { get; set; }

        public List<AppUser> UsersList { get; set; }

        public List<Tuple<AppUser, AppRole>> Users { get; set; }

        public List<SelectListItem> RegionRoleNames { get; set; }

        public string AccountList { get; set; }

        public string SelectedRoleId { get; set; }

        private readonly string Region;

        public string LoginTicket { get; set; }

        public bool BatchImportUsersIsAvaliable { get; private set; }

        public AppUserViewModel()
        {
            this.UsersList = new List<AppUser>();
            this.Region = AppContext.AppRegion;

            this.RegionRoleNames = new List<SelectListItem>();
            this.Users = new List<Tuple<AppUser, AppRole>>();

            //get the list of existing roles
            foreach (var role in this.UnitWork.AppRole.Get().Where(r => r.Region == this.Region).OrderBy(r => r.Name))
                this.RegionRoleNames.Add(new SelectListItem() { Text = role.Name, Value = role.Id });

            if (this.RegionRoleNames.Count() > 0)
            {
                this.RegionRoleNames.First().Selected = true;
                this.SelectedRoleId = this.RegionRoleNames.First().Value;
            }

            this.BatchImportUsersIsAvaliable = false;
            foreach (var rolename in AppContext.UserBelongRoles)
            {
                if (rolename.Contains("Administrator"))
                {
                    this.BatchImportUsersIsAvaliable = true;
                    break;
                }

            }
        }

        public override void Init()
        {
            throw new NotImplementedException();
        }

        public void BatchImportAccount(string[] accountList)
        {
            foreach (var mail in accountList)
            {
                // check membership data existed or not
                try
                {
                    var profile = SSOAuthHelper.GetAdvantechMemberProfile(mail);
                    if (profile != null)
                    {
                        var appUser = SSOAuthHelper.MappingSSOProfileToAppuser(profile, mail);
                        if (SSOAuthHelper.IsEmployee(appUser.Email))
                        {
                            var mngr = new IdentityManager();
                            var existedUser = mngr.GetUserByEmail(mail);
                            if (existedUser == null)
                            {
                                //create new user
                                //var password = "advantech";
                                //if (LoginTicket != null) // if SSO, set password = null
                                //    password = null;
                                var succeed = mngr.CreateUser(appUser, null);

                                //add one role to this user
                                if (succeed)
                                {
                                    //get selected role-Ids
                                    if (!string.IsNullOrEmpty(this.SelectedRoleId))
                                    {
                                        var role = mngr.GetRoleByRoleId(this.SelectedRoleId);
                                        appUser = mngr.GetUserByName(appUser.UserName);
                                        succeed = mngr.AddUserToRole(appUser.Id, role.Name);
                                        if (!succeed)
                                            throw new HttpException(608, string.Format("Role [{0}] could not be assigned.", role.Name));
                                    }
                                }
                                else
                                    throw new HttpException(608, string.Format("Creat user {0} fail.", this.User.Email));


                            }
                            else
                                throw new HttpException(608, string.Format("User {0} is existed in eQuotation.", this.User.Email));

                        }
                        else
                            throw new HttpException(608, string.Format("User {0} is not the employee of Advantech.", this.User.Email));

                    }
                    else
                        throw new HttpException(608, string.Format("User {0} is not the member of Advantech.", this.User.Email));
                }
                catch { };
            }
        }

        public override void SetValue()
        {
            if (!HasEntity(this.User.Id))
            {
                if (this.UnitWork.AppUser.Exists(x => x.UserName == this.User.UserName))
                    throw new HttpException(608, "UserName has been used.");

                // check membership data existed or not
                var profile = SSOAuthHelper.GetAdvantechMemberProfile(this.User.Email);
                if (profile != null)
                {
                    var appUser = SSOAuthHelper.MappingSSOProfileToAppuser(profile, this.User.Email);
                    if (SSOAuthHelper.IsEmployee(appUser.Email))
                    {
                        var mngr = new IdentityManager();
                        var existedUser = mngr.GetUserByEmail(this.User.Email);
                        if (existedUser == null)
                        {
                            //create new user
                            var succeed = mngr.CreateUser(appUser, null);

                            //add one role to this user
                            if (succeed)
                            {
                                //get selected role-Ids
                                if (!string.IsNullOrEmpty(this.SelectedRoleId))
                                {
                                    var role = mngr.GetRoleByRoleId(this.SelectedRoleId);
                                    appUser = mngr.GetUserByName(appUser.UserName);
                                    succeed = mngr.AddUserToRole(appUser.Id, role.Name);
                                    if (!succeed)
                                        throw new HttpException(608, string.Format("Role [{0}] could not be assigned.", role.Name));
                                }
                            }
                            else
                                throw new HttpException(608, string.Format("Creat user {0} fail.", this.User.Email));


                        }
                        else
                            throw new HttpException(608, string.Format("User {0} is existed in eQuotation.", this.User.Email));

                    }
                    else
                        throw new HttpException(608, string.Format("User {0} is not the employee of Advantech.", this.User.Email));

                }
                else
                    throw new HttpException(608, string.Format("User {0} is not the member of Advantech.", this.User.Email));

            }
            else
            {
                //update existing user attributes
                Entity.Id = this.User.Id;
                Entity.FirstName = this.User.FirstName;
                Entity.LastName = this.User.LastName;
                Entity.Position = this.User.Position;
                Entity.Department = this.User.Department;
                Entity.Company = this.User.Company;
                Entity.Location = this.User.Location;
                Entity.Email = this.User.Email;

                this.UnitWork.AppUser.Update(Entity);
            }
        }


        public override void GetValue(AppUser data)
        {
            this.User = data;
        }

        public void GetUserList()
        {
            this.UsersList = this.UnitWork.AppUser.Get(x => !x.Disabled).ToList();
        }

        public void GetUserByUserID(string userid)
        {

        }

        public void DeleteProfile(string id)
        {
            var user = this.UnitWork.AppUser.GetByID(id);
            user.Disabled = true;

            this.UnitWork.AppUser.Update(user);
        }
    }
}