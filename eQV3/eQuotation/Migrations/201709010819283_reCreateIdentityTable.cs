namespace eQuotation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reCreateIdentityTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppActions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UriAction = c.String(nullable: false),
                        Controller = c.String(nullable: false),
                        Action = c.String(nullable: false),
                        Category = c.String(nullable: false),
                        Parent = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppErrors",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        Title = c.String(),
                        Message = c.String(),
                        ControllerName = c.String(),
                        ActionName = c.String(),
                        UserName = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Client = c.String(),
                        ParamInfo = c.String(),
                        StackTrace = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AppLogEvents",
                c => new
                    {
                        Uid = c.Guid(nullable: false, identity: true),
                        LogDate = c.DateTime(nullable: false),
                        ProviderName = c.String(),
                        Source = c.String(),
                        Machine = c.String(),
                        Code = c.Int(nullable: false),
                        Title = c.String(),
                        Level = c.Int(nullable: false),
                        Message = c.String(),
                        StackTrace = c.String(),
                        AllXml = c.String(),
                        User = c.String(),
                        RequestUrl = c.String(),
                        Duration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Uid);
            
            CreateTable(
                "dbo.MenuCategories",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        ProcID = c.Int(nullable: false),
                        Name = c.String(),
                        ExtDesc = c.String(),
                        Timestamp = c.DateTime(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MenuElements",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        ProcID = c.Int(nullable: false),
                        Name = c.String(),
                        ExtDesc = c.String(),
                        Timestamp = c.DateTime(),
                        Active = c.Boolean(nullable: false),
                        Default = c.Boolean(nullable: false),
                        ClientURL = c.String(),
                        GroupID = c.String(maxLength: 128),
                        CategoryID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MenuCategories", t => t.CategoryID)
                .ForeignKey("dbo.MenuGroups", t => t.GroupID)
                .Index(t => t.GroupID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.MenuControls",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        ElementID = c.String(maxLength: 128),
                        AppName = c.String(),
                        Timestamp = c.DateTime(),
                        CreatedBy = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MenuElements", t => t.ElementID)
                .Index(t => t.ElementID);
            
            CreateTable(
                "dbo.MenuGroups",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        ProcID = c.Int(nullable: false),
                        Name = c.String(),
                        NameDe = c.String(),
                        NameTw = c.String(),
                        ExtDesc = c.String(),
                        Timestamp = c.DateTime(),
                        Active = c.Boolean(nullable: false),
                        CategoryID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MenuCategories", t => t.CategoryID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.AppRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AppRoleActions",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        ActionId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.ActionId })
                .ForeignKey("dbo.AppActions", t => t.ActionId, cascadeDelete: true)
                .ForeignKey("dbo.AppRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.ActionId);
            
            CreateTable(
                "dbo.AppUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AppRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AppUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Company = c.String(),
                        Department = c.String(),
                        Location = c.String(),
                        Position = c.String(),
                        Disabled = c.Boolean(nullable: false),
                        Groupkey = c.String(maxLength: 10),
                        PurchaseGroup = c.String(),
                        MRPController = c.String(),
                        Gender = c.String(),
                        CellPhoneNum = c.String(),
                        JobTittle = c.String(),
                        CompanyPhoneNum = c.String(),
                        CompanyFax = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AppUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AppUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AppUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUserRoles", "UserId", "dbo.AppUsers");
            DropForeignKey("dbo.AppUserLogins", "UserId", "dbo.AppUsers");
            DropForeignKey("dbo.AppUserClaims", "UserId", "dbo.AppUsers");
            DropForeignKey("dbo.AppUserRoles", "RoleId", "dbo.AppRoles");
            DropForeignKey("dbo.AppRoleActions", "RoleId", "dbo.AppRoles");
            DropForeignKey("dbo.AppRoleActions", "ActionId", "dbo.AppActions");
            DropForeignKey("dbo.MenuElements", "GroupID", "dbo.MenuGroups");
            DropForeignKey("dbo.MenuGroups", "CategoryID", "dbo.MenuCategories");
            DropForeignKey("dbo.MenuControls", "ElementID", "dbo.MenuElements");
            DropForeignKey("dbo.MenuElements", "CategoryID", "dbo.MenuCategories");
            DropIndex("dbo.AppUserLogins", new[] { "UserId" });
            DropIndex("dbo.AppUserClaims", new[] { "UserId" });
            DropIndex("dbo.AppUsers", "UserNameIndex");
            DropIndex("dbo.AppUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AppUserRoles", new[] { "UserId" });
            DropIndex("dbo.AppRoleActions", new[] { "ActionId" });
            DropIndex("dbo.AppRoleActions", new[] { "RoleId" });
            DropIndex("dbo.AppRoles", "RoleNameIndex");
            DropIndex("dbo.MenuGroups", new[] { "CategoryID" });
            DropIndex("dbo.MenuControls", new[] { "ElementID" });
            DropIndex("dbo.MenuElements", new[] { "CategoryID" });
            DropIndex("dbo.MenuElements", new[] { "GroupID" });
            DropTable("dbo.AppUserLogins");
            DropTable("dbo.AppUserClaims");
            DropTable("dbo.AppUsers");
            DropTable("dbo.AppUserRoles");
            DropTable("dbo.AppRoleActions");
            DropTable("dbo.AppRoles");
            DropTable("dbo.MenuGroups");
            DropTable("dbo.MenuControls");
            DropTable("dbo.MenuElements");
            DropTable("dbo.MenuCategories");
            DropTable("dbo.AppLogEvents");
            DropTable("dbo.AppErrors");
            DropTable("dbo.AppActions");
        }
    }
}
