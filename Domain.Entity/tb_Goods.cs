using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 商城商品管理
    /// </summary>
    public partial class tb_Goods
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string GoodsNum { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 商品副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 商品展示图
        /// </summary>
        public string GoodsImage { get; set; }
        /// <summary>
        /// 商品所属类别ID
        /// </summary>
        public int ClassId { get; set; }
        /// <summary>
        /// 商品所属商店ID
        /// </summary>
        public int ShopId { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 品牌ID
        /// </summary>
        public int BrandsId { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 库存数
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// 已销售数
        /// </summary>
        public int SoldNum { get; set; }
        /// <summary>
        /// 限购数
        /// </summary>
        public int LimitNum { get; set; }
        /// <summary>
        /// 访问量
        /// </summary>
        public int Hits { get; set; }
        /// <summary>
        /// 详细介绍
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 服务保障
        /// </summary>
        public string Notice { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        public bool IsList { get; set; }
        /// <summary>
        /// 是否左侧推荐
        /// </summary>
        public bool IsSlide { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime EditDate { get; set; }
        [ForeignKey("ShopId")]
        public virtual tb_Shop tb_Shop { get; set; }
        [ForeignKey("BrandsId")]
        public virtual tb_GoodsBrand tb_GoodsBrand { get; set; }
        public virtual ICollection<tb_GoodsComment> tb_GoodsComment { get; set; }
        //public virtual ICollection<tb_ImageLibrary> tb_ImageLibrary { get; set; }
        public virtual ICollection<tb_GoodsOrderDetail> tb_GoodsOrderDetail { get; set; }

        [ForeignKey("ClassId")]
        public virtual tb_Column tb_Column { get; set; }
    }
}
