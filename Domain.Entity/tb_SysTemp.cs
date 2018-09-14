using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 系统模型管理
    /// </summary>
    public partial class tb_SysTemp
    {
        /// <summary>
        /// 模型ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 模型标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 模型链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 模型所属站点名
        /// </summary>
        public int SiteId { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLock { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 添加日期
        /// </summary>
        public DateTime AddDate { get; set; }

        public virtual ICollection<tb_Column> tb_Column { get; set; }
    }
}
