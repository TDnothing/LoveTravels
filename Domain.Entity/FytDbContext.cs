using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Domain.Entity
{
    public class FytDbContext : DbContext
    {
        public FytDbContext()
            : base("name=FytDbContext")
        {
        }
        public virtual DbSet<tb_Admin> tb_Admin { get; set; }
        public virtual DbSet<tb_AdminMenu> tb_AdminMenu { get; set; }
        public virtual DbSet<tb_AdminRole> tb_AdminRole { get; set; }
        public virtual DbSet<tb_AdvClass> tb_AdvClass { get; set; }
        public virtual DbSet<tb_AdvList> tb_AdvList { get; set; }
        public virtual DbSet<tb_Article> tb_Article { get; set; }
        public virtual DbSet<tb_Attention> tb_Attention { get; set; }
        public virtual DbSet<tb_Collect> tb_Collect { get; set; }
        public virtual DbSet<tb_Comment> tb_Comment { get; set; }
        public virtual DbSet<tb_DownLoad> tb_DownLoad { get; set; }
        public virtual DbSet<tb_Goods> tb_Goods { get; set; }
        public virtual DbSet<tb_GoodsAddress> tb_GoodsAddress { get; set; }
        public virtual DbSet<tb_GoodsAttr> tb_GoodsAttr { get; set; }
        public virtual DbSet<tb_GoodsBrand> tb_GoodsBrand { get; set; }
        public virtual DbSet<tb_GoodsComment> tb_GoodsComment { get; set; }
        public virtual DbSet<tb_GoodsOrder> tb_GoodsOrder { get; set; }
        public virtual DbSet<tb_GoodsOrderDetail> tb_GoodsOrderDetail { get; set; }
        public virtual DbSet<tb_GoodsOrderLog> tb_GoodsOrderLog { get; set; }
        public virtual DbSet<tb_ImageLibrary> tb_ImageLibrary { get; set; }
        public virtual DbSet<tb_Job> tb_Job { get; set; }
        public virtual DbSet<tb_keyTags> tb_keyTags { get; set; }
        public virtual DbSet<tb_Message> tb_Message { get; set; }
        public virtual DbSet<tb_MessageRep> tb_MessageRep { get; set; }
        public virtual DbSet<tb_MobileLog> tb_MobileLog { get; set; }
        public virtual DbSet<tb_Product> tb_Product { get; set; }
        public virtual DbSet<tb_City> tb_Region { get; set; }
        public virtual DbSet<tb_GoodsMess> tb_ReserveOrder { get; set; }
        public virtual DbSet<tb_Shop> tb_Shop { get; set; }
        public virtual DbSet<tb_Site> tb_Site { get; set; }
        public virtual DbSet<tb_SystemLog> tb_SystemLog { get; set; }
        public virtual DbSet<tb_SysTemp> tb_SysTemp { get; set; }
        public virtual DbSet<tb_User> tb_User { get; set; }
        public virtual DbSet<tb_UserApp> tb_UserApp { get; set; }
        public virtual DbSet<tb_UserEmail> tb_UserEmail { get; set; }
        public virtual DbSet<tb_UserGroup> tb_UserGroup { get; set; }
        public virtual DbSet<tb_UserLog> tb_UserLog { get; set; }
        public virtual DbSet<tb_UserLoginApi> tb_UserLoginApi { get; set; }
        public virtual DbSet<tb_UserLoginOauth> tb_UserLoginOauth { get; set; }
        public virtual DbSet<tb_UserMoneyLog> tb_UserMoneyLog { get; set; }
        public virtual DbSet<tb_UserPointLog> tb_UserPointLog { get; set; }
        public virtual DbSet<tb_VersionUpdate> tb_VersionUpdate { get; set; }
        public virtual DbSet<tb_Vote> tb_Vote { get; set; }
        public virtual DbSet<tb_VoteItem> tb_VoteItem { get; set; }
        public virtual DbSet<tb_Column> tb_Column { get; set; }
        public virtual DbSet<tb_Picture> tb_Picture { get; set; }
        public virtual DbSet<tb_AdminPermission> tb_AdminPermission { get; set; }
        public virtual DbSet<tb_Blog> tb_Blog { get; set; }
        public virtual DbSet<tb_BlogActicle> tb_BlogActicle { get; set; }
        public virtual DbSet<tb_GoodsReturn> tb_GoodsReturn { get; set; }

        public virtual DbSet<lv_Bank> lv_Bank { get; set; }
        public virtual DbSet<lv_GetMoney> lv_GetMoney { get; set; }
        public virtual DbSet<lv_GoLook> lv_GoLook { get; set; }
        public virtual DbSet<lv_GoLookOrder> lv_GoLookOrder { get; set; }
        public virtual DbSet<lv_Message> lv_Message { get; set; }
        public virtual DbSet<lv_ProJect> lv_ProJect { get; set; }
        public virtual DbSet<lv_ProjectOrder> lv_ProjectOrder { get; set; }
        public virtual DbSet<lv_Story> lv_Story { get; set; }
    }
}
