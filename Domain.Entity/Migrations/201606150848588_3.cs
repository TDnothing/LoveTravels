namespace Domain.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.lv_ProJect", "UsdPrice", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.lv_ProJect", "UsdPrice");
        }
    }
}
