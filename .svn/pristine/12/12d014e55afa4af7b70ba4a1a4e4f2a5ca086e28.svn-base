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
    /// 旅游消息
    /// </summary>
    public class lv_Message
    {
        /// <summary>
        /// 自动增长
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        [Display(Name = "收件人")]
        public int GoUserId { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        [Display(Name = "发件人")]
        public int SendUserId { get; set; }

        /// <summary>
        /// 发送的消息内容
        /// </summary>
        [MaxLength(500)]
        [Display(Name = "发送的消息内容")]
        public string Centents { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        [Display(Name = "是否已读")]
        public Boolean IsRead { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Display(Name = "添加时间")]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 关联到用户表
        /// </summary>
        [ForeignKey("SendUserId")]
        public virtual tb_User tb_User { get; set; }
    }

}
