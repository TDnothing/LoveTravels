using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// App安装统计管理
    /// </summary>
    public partial class tb_MobileLog
    {
        /// <summary>
        /// 统计ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 手机系统类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 手机编号
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
