using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 消息管理
    /// </summary>
    public partial class tb_Message
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 所属类别ID
        /// </summary>
        public int ClassId { get; set; }
        /// <summary>
        /// 回复ID
        /// </summary>
        public int MessId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// QQ号
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 所属父ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 回复信息ID
        /// </summary>
        public int RepId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime? RepDate { get; set; }
        [ForeignKey("ClassId")]
        public virtual tb_Column tb_Column { get; set; }
    }
}
