namespace HMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class propertyDescriptionAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccomodationTypes", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccomodationTypes", "Description");
        }
    }
}
