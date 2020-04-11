namespace PlanMyRun.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedPace : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationUser", "Pace", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationUser", "Pace", c => c.Double());
        }
    }
}
