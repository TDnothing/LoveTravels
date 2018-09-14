using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 评论管理
    /// </summary>
    public partial class tb_Comment
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 所属类别ID
        /// </summary>
        public int ClassId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Go_LookId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 是否匿名
        /// </summary>
        public bool IsUser { get; set; }
        /// <summary>
        /// 用户匿名编号
        /// </summary>
        public string UserNumber { get; set; }
        /// <summary>
        /// 评论标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 评论详情
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime AddDate { get; set; }
        /// <summary>
        /// 用户IP
        /// </summary>
        public string UserIP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Option { get; set; }
        /// <summary>
        /// 评论星级
        /// </summary>
        public int Star { get; set; }
        /// <summary>
        /// 回复者ID
        /// </summary>
        public int RepId { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string RepContent { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime? RepDate { get; set; }
        /// <summary>
        /// 赞
        /// </summary>
        public int Good { get; set; }
        /// <summary>
        /// 踩
        /// </summary>
        public int Tread { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        [ForeignKey("UserId")]
        public virtual tb_User tb_User { get; set; }
    }
}
