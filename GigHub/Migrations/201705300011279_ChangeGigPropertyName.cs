namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGigPropertyName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "OriginalDateTime", c => c.DateTime());
            DropColumn("dbo.Notifications", "OriginaDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "OriginaDateTime", c => c.DateTime());
            DropColumn("dbo.Notifications", "OriginalDateTime");
        }
    }
}
