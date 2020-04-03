namespace PlanMyRun.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Run", "ScheduledDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RacePlan", "RaceDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RacePlan", "RaceDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Run", "ScheduledDateTime", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
