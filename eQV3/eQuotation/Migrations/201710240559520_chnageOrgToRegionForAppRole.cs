namespace eQuotation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chnageOrgToRegionForAppRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppRoles", "Region", c => c.String());
            DropColumn("dbo.AppRoles", "Org");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppRoles", "Org", c => c.String());
            DropColumn("dbo.AppRoles", "Region");
        }
    }
}
