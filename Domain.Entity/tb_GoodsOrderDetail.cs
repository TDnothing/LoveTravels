using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 商品订单详情管理
    /// </summary>
    public partial class tb_GoodsOrderDetail
    {
        /// <summary>
        /// 订单详情ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public int GoodsId { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 购买数
        /// </summary>
        public int GSum { get; set; }
        /// <summary>
        /// 商品总价
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string GoodsImage { get; set; }
        /// <summary>
        /// 用户需求
        /// </summary>
        public string UMsg { get; set; }
        [ForeignKey("GoodsId")]
        public virtual tb_Goods tb_Goods { get; set; }
        [ForeignKey("OrderID")]
        public virtual tb_GoodsOrder tb_GoodsOrder { get; set; }
    }
}
