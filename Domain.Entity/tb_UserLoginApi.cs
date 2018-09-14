using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 用户第三方登录管理
    /// </summary>
    public partial class tb_UserLoginApi
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
        /// 第三方类型
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 第三方名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 第三方编号
        /// </summary>
        public string ApiID { get; set; }
        /// <summary>
        /// 第三方密钥
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// 返回链接
        /// </summary>
        public string BackUrl { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        public virtual ICollection<tb_UserLoginOauth> tb_UserLoginOauth { get; set; }
    }
}
