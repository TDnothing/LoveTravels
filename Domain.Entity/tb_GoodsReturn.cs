using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 商品退货申请
    /// </summary>
    public class tb_GoodsReturn
    {
        /// <summary>
        /// 自动增长
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [MaxLength(50)]
        public string OrderNum { get; set; }

        /// <summary>
        /// 商品的ID
        /// </summary>
        public int GoodsId { get; set; }

        /// <summary>
        /// 申请用户的ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 会员昵称
        /// </summary>
        [MaxLength(50)]
        public string UserName { get; set; }

        /// <summary>
        /// 会员手机号码
        /// </summary>
        [MaxLength(50)]
        public string UserMobile { get; set; }

        /// <summary>
        /// 申请人地址
        /// </summary>
        [MaxLength(200)]
        public string UserAddress { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(500)]
        public string Summary { get; set; }

        /// <summary>
        /// 管理员回复退单信息
        /// </summary>
        [MaxLength(3000)]
        public string AuditSummary { get; set; }

        /// <summary>
        /// 回复状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime? AuditDate { get; set; }

    }
}
