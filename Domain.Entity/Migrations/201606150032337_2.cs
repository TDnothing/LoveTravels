namespace Domain.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.lv_GetMoney", "AreaNum", c => c.String());
            AddColumn("dbo.lv_GoLook", "Audit", c => c.Int(nullable: false));
            AddColumn("dbo.lv_ProJect", "Audit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.lv_ProJect", "Audit");
            DropColumn("dbo.lv_GoLook", "Audit");
            DropColumn("dbo.lv_GetMoney", "AreaNum");
        }
    }
}
