using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 收藏管理
    /// </summary>
    public partial class tb_Collect
    {
        /// <summary>
        /// 收藏ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 收藏项ID
        /// </summary>
        public int CollectId { get; set; }
        /// <summary>
        /// 收藏项唯一标示
        /// </summary>
        public string CollectGuid { get; set; }
        /// <summary>
        /// 收藏描述
        /// </summary>
        public string CollectInfo { get; set; }
        /// <summary>
        /// 收藏类型
        /// </summary>
        public int? CollectType { get; set; }
        /// <summary>
        /// 收藏时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
