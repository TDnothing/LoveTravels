namespace Domain.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.lv_GetMoney", "AreaNum");
            DropColumn("dbo.lv_GoLook", "Audit");
            DropColumn("dbo.lv_ProJect", "Audit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.lv_ProJect", "Audit", c => c.Int(nullable: false));
            AddColumn("dbo.lv_GoLook", "Audit", c => c.Int(nullable: false));
            AddColumn("dbo.lv_GetMoney", "AreaNum", c => c.String());
        }
    }
}
