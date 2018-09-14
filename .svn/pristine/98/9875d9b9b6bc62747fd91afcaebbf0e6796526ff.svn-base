using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 报名人数
    /// </summary>
    public class lv_GoLookOrder
    {
        /// <summary>
        /// 自动增长
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 去看看主键
        /// </summary>
        [Display(Name = "去看看主键")]
        public int LookId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "编号")]
        public string Number { get; set; }

        /// <summary>
        /// 报名人ID
        /// </summary>
        [Display(Name = "报名人")]
        public int UserId { get; set; }

        /// <summary>
        /// 支付状态，0=未支付，1=已支付，2-支付失败
        /// </summary>
        [Display(Name = "支付状态，0=未支付，1=已支付，2-支付失败")]
        public int PayStatus { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "支付类型")]
        public string PayName { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        [Display(Name = "支付金额")]
        public decimal PayPrice { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Display(Name = "添加时间")]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 关联到用户表
        /// </summary>
        [ForeignKey("UserId")]
        public virtual tb_User tb_User { get; set; }

    }
}
