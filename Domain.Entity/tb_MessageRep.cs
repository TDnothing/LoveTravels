using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 回复消息管理
    /// </summary>
    public partial class tb_MessageRep
    {
        /// <summary>
        /// 回复消息ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 消息ID
        /// </summary>
        public int MessId { get; set; }
        /// <summary>
        /// 所属父ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
