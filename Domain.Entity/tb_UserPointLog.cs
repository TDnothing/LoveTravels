using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 用户积分日志管理
    /// </summary>
    public partial class tb_UserPointLog
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
        /// 订单ID
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public int Option { get; set; }
        /// <summary>
        /// 获得积分
        /// </summary>
        public int Point { get; set; }
        /// <summary>
        /// 当前积分
        /// </summary>
        public int? NowPoint { get; set; }
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
