namespace Domain.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.lv_GoLook", "UsdPrice", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.lv_GoLook", "UsdPrice");
        }
    }
}
