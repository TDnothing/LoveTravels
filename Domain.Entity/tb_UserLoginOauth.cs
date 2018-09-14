using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 用户授权管理
    /// </summary>
    public partial class tb_UserLoginOauth
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
        /// 授权ID
        /// </summary>
        public int OauthId { get; set; }
        /// <summary>
        /// 授权密钥
        /// </summary>
        public string OauthKey { get; set; }
        /// <summary>
        /// 公开ID
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 拓展
        /// </summary>
        public string Expand { get; set; }
        [ForeignKey("UserId")]
        public virtual tb_User tb_User { get; set; }
        [ForeignKey("OauthId")]
        public virtual tb_UserLoginApi tb_UserLoginApi { get; set; }
    }
}
