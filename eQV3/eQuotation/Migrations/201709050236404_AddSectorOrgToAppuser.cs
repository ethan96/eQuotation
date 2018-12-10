namespace eQuotation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSectorOrgToAppuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppRoles", "Org", c => c.String());
            AddColumn("dbo.AppRoles", "Sector", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppRoles", "Sector");
            DropColumn("dbo.AppRoles", "Org");
        }
    }
}
