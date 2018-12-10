using eQuotation.DataAccess;
using eQuotation.Entities;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace eQuotation.Migrations
{
    public class InitData
    {
        public static void Configure()
        {
            var unitWork = new UnitOfWork();
            var identityMgr = new IdentityManager();

            //create user admin
            var admin = new AppUser();
            admin.UserName = "admin.system";
            admin.FirstName = "Admin";
            admin.LastName = "System";
            admin.Email = "prie@advantech.de";

            //insert user admin
            identityMgr.CreateUser(admin, "passw0rd");
            var newUser = unitWork.AppUser.GetFirst(x => x.UserName == admin.UserName);

            //create new roles
            identityMgr.CreateRole("Administrator", "","","It grants full permission on system");

            //link user to ádmin-role
            var roleName = "Administrator";
            identityMgr.AddUserToRole(newUser.Id, roleName);
            var newRole = unitWork.AppRole.GetFirst(x => x.Name == roleName);

            //insert actions
            unitWork.AppAction.Insert(new AppAction() { Id = "MA02001", UriAction = "/Admin/UserManager", Controller = "AdminController", Action = "UserManager", Category = "Module Admin", Parent = "MA00000", Description = "It allows user to review the list of users registered in the application." });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA02002", UriAction = "/Admin/ReviewUser", Controller = "AdminController", Action = "ReviewUser", Category = "Module Admin", Parent = "MA00000", Description = "It allows user to review the detail information of users." });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA02003", UriAction = "/Admin/EditProfile", Controller = "AdminController", Action = "EditProfile", Category = "Module Admin", Parent = "MA00000", Description = "It allows user to review and edit the basic information of users." });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA02004", UriAction = "/Admin/DeleteProfile", Controller = "AdminController", Action = "DeleteProfile", Category = "Module Admin", Parent = "MA00000", Description = "It allows user to delete users." });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA02005", UriAction = "/Admin/NewUser", Controller = "AdminController", Action = "NewUser", Category = "Module Admin", Parent = "MA00000", Description = "It allows user to create new users." });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA02006", UriAction = "/Admin/EditUserRole", Controller = "AdminController", Action = "EditUserRole", Category = "Module Admin", Parent = "MA00000", Description = "It allows user to modify the user role." });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA03001", UriAction = "/Admin/RoleManager", Controller = "AdminController", Action = "RoleManager", Category = "Module Admin", Parent = "MA00000", Description = "It allows user to review the list of existing user roles and their permission." });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA03002", UriAction = "/Admin/RolePermission", Controller = "AdminController", Action = "RolePermission", Category = "Module Admin", Parent = "MA00000", Description = "It allows user to review and modify new permission for specified role." });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA03003", UriAction = "/Admin/NewRole", Controller = "AdminController", Action = "NewRole", Category = "Module Admin", Parent = "MA00000", Description = "It allows user to create new roles." });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA03004", UriAction = "/Admin/EditRole", Controller = "AdminController", Action = "EditRole", Category = "Module Admin", Parent = "MA00000", Description = "It allows user to edit user roles." });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA03005", UriAction = "/Admin/DeleteRole", Controller = "AdminController", Action = "DeleteRole", Category = "Module Admin", Parent = "MA00000", Description = "It allows user to delete user roles." });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA03006", UriAction = "/Admin/SynchronizePermission", Controller = "AdminController", Action = "SynchronizePermission", Category = "Module Admin", Parent = "MA00000", Description = "It allows user to synchronize and update new created tasks (actions)." });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA03007", UriAction = "/Admin/SavePermission", Controller = "AdminController", Action = "SavePermission", Category = "Module Admin", Parent = "MA00000", Description = "It allows user to modify permission for specified roles." });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA04001", UriAction = "/Admin/Visibility", Controller = "AdminController", Action = "SaveControl", Category = "Module Admin", Parent = "MA00000", Description = "It allows to review specific feature" });
            unitWork.AppAction.Insert(new AppAction() { Id = "MA04002", UriAction = "/Admin/SaveControl", Controller = "AdminController", Action = "SaveControl", Category = "Module Admin", Parent = "MA00000", Description = "It allows to enable or disable specific function" });

            unitWork.Save();

            //associate action to admin-role
            identityMgr.AddActionToRole(roleName, "MA02001");
            identityMgr.AddActionToRole(roleName, "MA02002");
            identityMgr.AddActionToRole(roleName, "MA02003");
            identityMgr.AddActionToRole(roleName, "MA02005");
            identityMgr.AddActionToRole(roleName, "MA02004");
            identityMgr.AddActionToRole(roleName, "MA02006");
            identityMgr.AddActionToRole(roleName, "MA03001");
            identityMgr.AddActionToRole(roleName, "MA03002");
            identityMgr.AddActionToRole(roleName, "MA03003");
            identityMgr.AddActionToRole(roleName, "MA03004");
            identityMgr.AddActionToRole(roleName, "MA03005");
            identityMgr.AddActionToRole(roleName, "MA03006");
            identityMgr.AddActionToRole(roleName, "MA03007");
            identityMgr.AddActionToRole(roleName, "MA04001");
            identityMgr.AddActionToRole(roleName, "MA04002");

            //insert menu category
            unitWork.MenuCategory.Insert(new MenuCategory() { ID = "001", ProcID = 100, Name = "System Administration", Timestamp = DateTime.Now, Active = true });
            unitWork.MenuCategory.Insert(new MenuCategory() { ID = "002", ProcID = 200, Name = "Quotation", Timestamp = DateTime.Now, Active = true });

            //insert menu element
            unitWork.MenuElement.Insert(new MenuElement() { ID = "0001", ProcID = 10, Name = "User Profile", Timestamp = DateTime.Now, Active = true, Default = true, ClientURL = "admin/userprofile", CategoryID = "001" });
            unitWork.MenuElement.Insert(new MenuElement() { ID = "0002", ProcID = 20, Name = "User Manager", Timestamp = DateTime.Now, Active = true, Default = true, ClientURL = "admin/usermanager", CategoryID = "001" });
            unitWork.MenuElement.Insert(new MenuElement() { ID = "0003", ProcID = 30, Name = "Role Manager", Timestamp = DateTime.Now, Active = true, Default = true, ClientURL = "admin/rolemanager", CategoryID = "001" });
            unitWork.MenuElement.Insert(new MenuElement() { ID = "0004", ProcID = 40, Name = "Visibility Control", Timestamp = DateTime.Now, Active = true, Default = true, ClientURL = "admin/visibility", CategoryID = "001" });
            unitWork.MenuElement.Insert(new MenuElement() { ID = "0006", ProcID = 50, Name = "MyQuotation", Timestamp = DateTime.Now, Active = true, Default = true, ClientURL = "Quotes/MyQuotation", CategoryID = "002" });
            unitWork.MenuElement.Insert(new MenuElement() { ID = "0007", ProcID = 60, Name = "MyTeamQuotation", Timestamp = DateTime.Now, Active = true, Default = true, ClientURL = "Quotes/MyTeamQuotation", CategoryID = "002" });
            unitWork.MenuElement.Insert(new MenuElement() { ID = "0008", ProcID = 70, Name = "CreateNewQuotation", Timestamp = DateTime.Now, Active = true, Default = true, ClientURL = "Quotes/CreateNewQuotation", CategoryID = "002" });

            //insert menu control
            unitWork.MenuControl.Insert(new MenuControl() { ID = "0001", AppName = AppContext.AppName, ElementID = "0001", Active = true } );
            unitWork.MenuControl.Insert(new MenuControl() { ID = "0002", AppName = AppContext.AppName, ElementID = "0002", Active = true });
            unitWork.MenuControl.Insert(new MenuControl() { ID = "0003", AppName = AppContext.AppName, ElementID = "0003", Active = true });
            unitWork.MenuControl.Insert(new MenuControl() { ID = "0004", AppName = AppContext.AppName, ElementID = "0004", Active = true });

            unitWork.Save();
        }




    }
}