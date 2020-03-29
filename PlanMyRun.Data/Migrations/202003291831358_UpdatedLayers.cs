namespace PlanMyRun.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedLayers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Location", "UserId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Location", "UserId");
        }
    }
}
