using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 关键字管理
    /// </summary>
    public partial class tb_keyTags
    {
        /// <summary>
        /// 关键字管理
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 首字母
        /// </summary>
        public string FirstLetter { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Links { get; set; }
        /// <summary>
        /// 点击数
        /// </summary>
        public int TagsHits { get; set; }
        /// <summary>
        /// 日点击数
        /// </summary>
        public int DayHits { get; set; }
        /// <summary>
        /// 周点击数
        /// </summary>
        public int WeekHits { get; set; }
        /// <summary>
        /// 月点击数
        /// </summary>
        public int MonthHits { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? LastTime { get; set; }
    }
}
