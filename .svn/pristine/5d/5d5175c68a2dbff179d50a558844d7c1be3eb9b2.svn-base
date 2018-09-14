using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 商品留言管理
    /// </summary>
    public partial class tb_GoodsComment
    {
        /// <summary>
        /// 留言ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 所属商品ID
        /// </summary>
        public int GoodsId { get; set; }
        /// <summary>
        /// 留言用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 评价星级
        /// </summary>
        public int Star { get; set; }
        /// <summary>
        /// 是否回复
        /// </summary>
        public bool IsRep { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string RepContent { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime? RepDate { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        [ForeignKey("GoodsId")]
        public virtual tb_Goods tb_Goods { get; set; }

        [ForeignKey("UserId")]
        public virtual tb_User tb_User { get; set; }
    }
}
