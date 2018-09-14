using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 企业商品管理（不支持购买）
    /// </summary>
    public partial class tb_Product
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 所属类别ID
        /// </summary>
        public int ClassId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 商品类型
        /// </summary>
        public string ShopType { get; set; }
        /// <summary>
        /// 商品品牌
        /// </summary>
        public string ShopBrands { get; set; }
        /// <summary>
        /// 市场价
        /// </summary>
        public string MarkPrice { get; set; }
        /// <summary>
        /// 销售价
        /// </summary>
        public string SellPrice { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string ShopImg { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 详细介绍
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
