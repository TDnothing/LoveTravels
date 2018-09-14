using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 用户财务日志管理
    /// </summary>
    public partial class tb_UserMoneyLog
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 日志编号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public int Option { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? RealPrice { get; set; }
        /// <summary>
        /// 当前余额
        /// </summary>
        public decimal NowPrice { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
        [ForeignKey("UserId")]
        public virtual tb_User tb_User { get; set; }
    }
}
