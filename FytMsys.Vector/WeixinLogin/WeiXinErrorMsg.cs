using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FytMsys.Vector.WeixinLogin
{
    /// <summary>
    /// 微信错误访问的情况 
    /// </summary>
    public class WeiXinErrorMsg
    {
        /// <summary>
        /// 错误编号
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 错误提示消息
        /// </summary>
        public string errmsg { get; set; }
    }
}
