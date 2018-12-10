
using eQuotation.DataAccess;
using eQuotation.Entities;
using eQuotation.Models.Enum;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eQuotation.Utility
{


    public class IdentityManager
    {
        // Swap AppsRole for IdentityRole:
        //RoleManager<AppRole> roleManager = new RoleManager<AppRole>(new RoleStore<AppRole>(new AppDbContext()));
        RoleManager<AppRole> roleManager = new RoleManager<AppRole>(new RoleStore<AppRole, string, AppUserRole>(new AppDbContext()));

        //UserManager<AppUser> userManager = new UserManager<AppUser>(new UserStore<AppUser>(new AppDbContext()));
        UserManager<AppUser, string> userManager = new UserManager<AppUser, string>(new UserStore<AppUser, AppRole, string, AppUserLogin, AppUserRole, AppUserClaim>(new AppDbContext()));


        AppDbContext dbContext = new AppDbContext();

        public IdentityManager()
        {
            userManager.UserValidator = new UserValidator<AppUser>(userManager) { AllowOnlyAlphanumericUserNames = false };
        }

        public AppUser CurrentUser
        {

            get
            {
                var user = userManager.FindById(HttpContext.Current.User.Identity.GetUserId());
                return user;
            }
            
        }

        public bool RoleExists(string name)
        {
            return roleManager.RoleExists(name);
        }

        public bool CreateRole(string name, string org, string sector, string description = "")
        {
            // Swap ApplicationRole for IdentityRole:
            var idResult = roleManager.Create(new AppRole(name, description, org, sector));
            return idResult.Succeeded;
        }

        public bool CreateUser(AppUser user, string password)
        {
            //userManager.UserValidator = new UserValidator<AppsUser>(userManager) { AllowOnlyAlphanumericUserNames = false };
            IdentityResult idResult = new IdentityResult();
            var newUser = new AppUser();
            newUser.UserName = user.UserName;
            newUser.FirstName = user.FirstName;
            newUser.LastName = user.LastName;
            newUser.Email = user.Email;
            newUser.Company = user.Company;
            newUser.Department = user.Department;
            newUser.Location = user.Location;
            newUser.Position = user.Position;
            newUser.Email = user.Email;
            newUser.Disabled = false;

            if(password != null)
                idResult = userManager.Create(newUser, password);
            else
                idResult = userManager.Create(newUser);
            return idResult.Succeeded;
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var idResult = userManager.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }

        public bool ClearUserRoles(string userId, string region)
        {
            var succeed = true;

            var user = userManager.FindById(userId);
            //var currentRoles = new List<IdentityUserRole>();
            var currentRoles = new List<AppUserRole>();

            currentRoles.AddRange(user.Roles);
            foreach (var role in currentRoles)
            {
                var role1 = dbContext.Roles.SingleOrDefault(r=>r.Id == role.RoleId && r.Region == region);
                if(role1 !=null)
                    succeed = RemoveUserRole(userId, role1.Name);
                if (!succeed) return succeed;
            }

            return succeed;
        }

        public bool RemoveUserRole(string userId, string roleName)
        {
            var idt = userManager.RemoveFromRole(userId, roleName);
            return idt.Succeeded;
        }

        public void DeleteRole(string roleId)
        {
            var roleUsers = dbContext.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId));
            //var role = dbContext.AppRoles.Find(roleId);
            var role = dbContext.Roles.Find(roleId);

            //remove user role
            foreach (var user in roleUsers)
            {
                this.RemoveUserRole(user.Id, role.Name);
            }

            //remove role actions
            //var roleActions = dbContext.AppRoles.Where(r => r.Actions.Any(a => a.RoleId == roleId));
            var roleActions = dbContext.Roles.Where(r => r.Actions.Any(a => a.RoleId == roleId));
            foreach (var act in roleActions)
            {
                this.RemoveRoleAction(roleId, act.Id);
            }

            //dbContext.AppRoles.Remove(role);
            dbContext.Roles.Remove(role);
            dbContext.SaveChanges();
        }

        public bool ActionExists(string actionId)
        {
            var act = dbContext.AppActions.Where(a => a.Id == actionId);
            if (act.Count() > 0)
                return true;
            else
                return false;
        }

        public void CreateAction(AppAction action)
        {
            if (this.ActionExists(action.Id))
                throw new System.Exception("Action by that idenfier already exists in the database.");

            dbContext.AppActions.Add(action);
            dbContext.SaveChanges();
        }

        //delete role along with its actions
        public void ClearRoleActions(string actionId)
        { 
            //var roleActions = dbContext.AppRoles.Where(r => r.Actions.Any(a => a.ActionId == actionId)).ToList();
            var roleActions = dbContext.Roles.Where(r => r.Actions.Any(a => a.ActionId == actionId)).ToList();
            //var action = dbContext.Actions.Find(actionId);

            foreach (var role in roleActions)
                this.RemoveRoleAction(role.Id, actionId);
        }

        public void RemoveRoleAction(string roleId, string actionId)
        {
            //get the corresponding role
            //var role = dbContext.AppRoles.Find(roleId);
            var role = dbContext.Roles.Find(roleId);

            if (role.Actions != null)
            {
                //get relation between role - actions
                role.Actions.Where(ra => ra.ActionId == actionId).ToList().ForEach(
                    item => { role.Actions.Remove(item); });

                dbContext.SaveChanges();
            }

        }

        public void AddActionToRole(string roleName, string actionId)
        {
            //var role = dbContext.AppRoles.First(r => r.Name == roleName);
            var role = dbContext.Roles.First(r => r.Name == roleName);

            //check if the permission has been added or not
            if (role.Actions.Where(ra => ra.ActionId == actionId).Count() == 0)
            {
                var action = dbContext.AppActions.First(a => a.Id == actionId);
                var newRoleAction = new AppRoleAction()
                {
                    RoleId = role.Id,
                    ActionId = actionId,
                    Role = role,
                    Action = action
                };

                role.Actions.Add(newRoleAction);
                dbContext.SaveChanges();
            }

        }

        public void DeleteAction(string actionId)
        {
            ClearRoleActions(actionId);
            var action = dbContext.AppActions.First(a => a.Id == actionId);

            dbContext.AppActions.Remove(action);

            dbContext.SaveChanges();
        }

        public List<string> GetUriActions(string userName)
        {
            var uris = new List<string>();

            //get role for the corresponding username
            var roles = this.GetRoles(userName);

            foreach(var roleName in roles)
            {
                //var role = dbContext.AppRoles.First(r => r.Name == roleName);
                var role = dbContext.Roles.First(r => r.Name == roleName);
                foreach (var actId in role.Actions)
                {
                    var action = dbContext.AppActions.First(a => a.Id == actId.ActionId);
                    uris.Add(action.UriAction);
                }
            }

            return uris;
        }

        public List<string> GetActionsByUserId(string userId)
        {
            var actionList = new List<string>();

            //get role for the corresponding username
            var roleNames = this.GetRoleNamesByUserId(userId);

            foreach (var roleName in roleNames)
            {
                //var role = dbContext.AppRoles.First(r => r.Name == roleName);
                var role = dbContext.Roles.First(r => r.Name == roleName);
                foreach (var act in role.Actions)
                {
                    var action = dbContext.AppActions.First(a => a.Id == act.ActionId);
                    actionList.Add(action.Action);
                }
            }

            return actionList;
        }

        public List<string> GetRoles(string userName)
        {
            var user = dbContext.Users.First(u => u.UserName == userName);

            return userManager.GetRoles(user.Id).ToList();

        }

        public List<string> GetRoleNamesByUserId(string userId)
        {

            return userManager.GetRoles(userId).ToList();

        }

        public List<AppRole> GetRolesByRegion(string region)
        {
            return dbContext.Roles.Where(r => r.Region == region ).ToList();


        }

        public List<AppRole> GetRolesByUserIdAndRegion(string userId, string region)
        {
            var finalRoles = new List<AppRole>();
            var regionRoles = GetRolesByRegion(region);
            var userroleNames = GetRoleNamesByUserId(userId);
            foreach (var role in regionRoles)
            {
                if (userroleNames.Contains(role.Name))
                    finalRoles.Add(role);
            }

            return finalRoles;
        }

        public AppRole GetRoleByRoleId(string roleId)
        {

            return  dbContext.Roles.Where(r => r.Id == roleId).FirstOrDefault();
        }

        public AppRole GetRole(string roleNameKeyWord, string region, string sector)
        {
            var role = dbContext.Roles.Where(r => r.Name.Contains(roleNameKeyWord) && r.Region == region && r.Sector == sector).FirstOrDefault();

            return role;
        }

        public AppRole GetRole(string roleNameKeyWord, string region)
        {
            var role = dbContext.Roles.Where(r => r.Name.Contains(roleNameKeyWord) && r.Region == region).FirstOrDefault();

            return role;
        }

        public List<AppRole> GetRolesBynameKeyWord(string roleNameKeyWord)
        {
            var roles = dbContext.Roles.Where(r => r.Name.Contains(roleNameKeyWord)).ToList();

            return roles;
        }



        public List<AppUser> GetUsersByRole(string roleId)
        {

            var users = dbContext.Users
                .Where(x => x.Roles.Select(r => r.RoleId).Contains(roleId))
                .ToList();


            return users;

        }

        public List<string> GetUserRegions(string userId)
        {
            var user = dbContext.Users.First(u => u.Id == userId);
            List<string> regionList = new List<string>();
            if (user.Roles != null)
            {
                foreach (var userRole in user.Roles)
                {
                    var role = dbContext.Roles.First(r => r.Id == userRole.RoleId);
                    if(role != null && role.Region != null && !regionList.Contains(role.Region))
                        regionList.Add(role.Region);
                }
                return regionList;
            }
            return null;
        }

        public List<string> GetUserSectorsByRegion(string userId, string region)
        {
            var user = dbContext.Users.First(u => u.Id == userId);
            List<string> sectorList = new List<string>();
            if (user.Roles != null)
            {
                foreach (var userRole in user.Roles)
                {
                    var role = dbContext.Roles.Where(r => r.Id == userRole.RoleId && r.Region == region).FirstOrDefault();
                    if(role!=null && role.Sector!=null && !sectorList.Contains(role.Sector))
                        sectorList.Add(role.Sector);
                }
                return sectorList;
            }
            return null;
        }

        public bool UserIsInAction(string userId, string actionName)
        {
            var identity = new IdentityManager();
            var actions = identity.GetActionsByUserId(userId);
            return actions.Contains(actionName);            
        }


        public AppUser GetUserByName(string name)
        {

            return dbContext.Users.Single(r => r.UserName == name);
        }

        public AppUser GetUserByEmail(string email)
        {
            return userManager.FindByEmail(email);
        }

        public IEnumerable<AppAction> GetNewActions()
        {
            //search for all actions in application assembly
            var actions = AppReflection.GetActions();

            //get actions registered in database
            var regActions = dbContext.AppActions.Select(a => a.Id).ToList();

            //return only new Actions
            //takes only methods which are not in database
            return (IEnumerable<AppAction>) actions.Where(a => !regActions.Contains(a.Id));
        }
    }
}