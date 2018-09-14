using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 商品订单管理
    /// </summary>
    public partial class tb_GoodsOrder
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 订单用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 订单收货地址ID
        /// </summary>
        public int ShopAddressId { get; set; }
        /// <summary>
        /// 收件人名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 收件人联系电话
        /// </summary>
        public string UserMobile { get; set; }
        /// <summary>
        /// 收件地址
        /// </summary>
        public string UserAddress { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 订单描述
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public bool PayStatus { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalMoney { get; set; }
        /// <summary>
        /// 服务状态
        /// </summary>
        public int ServerStauts { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime AddDate { get; set; }

        [ForeignKey("ShopAddressId")]
        public virtual tb_GoodsAddress tb_GoodsAddress { get; set; }
        [ForeignKey("UserId")]
        public virtual tb_User tb_User { get; set; }
        public virtual ICollection<tb_GoodsOrderDetail> tb_GoodsOrderDetail { get; set; }
    }
}
