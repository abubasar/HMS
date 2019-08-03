namespace HMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialUpdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "AccomodationId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Bookings", "AccomodationId");
            AddForeignKey("dbo.Bookings", "AccomodationId", "dbo.Accomodations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "AccomodationId", "dbo.Accomodations");
            DropIndex("dbo.Bookings", new[] { "AccomodationId" });
            AlterColumn("dbo.Bookings", "AccomodationId", c => c.String());
        }
    }
}
