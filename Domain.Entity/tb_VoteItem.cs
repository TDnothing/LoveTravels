using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 投票项关联表
    /// </summary>
    public partial class tb_VoteItem
    {
        /// <summary>
        /// 自增长ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 投票ID
        /// </summary>
        public int VoteId { get; set; }
        /// <summary>
        /// 投票项标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 投票项总数
        /// </summary>
        public int VoteSum { get; set; }
        /// <summary>
        /// 投票得分
        /// </summary>
        public string Scale { get; set; }
        [ForeignKey("VoteId")]
        public virtual tb_Vote tb_Vote { get; set; }
    }
}
