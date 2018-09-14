using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 用户推送管理
    /// </summary>
    public partial class tb_UserApp
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// IOS标示符
        /// </summary>
        public string IosToke { get; set; }
        /// <summary>
        /// 安卓标识符
        /// </summary>
        public string AndroidToke { get; set; }
        /// <summary>
        /// 安全码
        /// </summary>
        public string SafeCode { get; set; }
        /// <summary>
        /// 是否推送
        /// </summary>
        public bool IsPush { get; set; }
        [ForeignKey("UserId")]
        public virtual tb_User tb_User { get; set; }
    }
}
