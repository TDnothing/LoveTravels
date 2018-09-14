using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 会员邮箱发送内容设置表
    /// </summary>
    public partial class tb_UserEmail
    {
        /// <summary>
        /// 自动增长
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 调用标识
        /// </summary>
        public string CallIndex { get; set; }
        /// <summary>
        /// 邮件发送类型
        /// </summary>
        public string MessType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BusType { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 是否启用的状态
        /// </summary>
        public bool Status { get; set; }
    }
}
