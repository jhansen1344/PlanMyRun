namespace PlanMyRun.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedZipCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUser", "ZipCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUser", "ZipCode");
        }
    }
}
