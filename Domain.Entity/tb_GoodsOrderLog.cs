using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 商品订单日志管理
    /// </summary>
    public partial class tb_GoodsOrderLog
    {
        /// <summary>
        /// 订单日志ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 下单用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 订单总价
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public int Types { get; set; }
        /// <summary>
        /// 日志描述
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
