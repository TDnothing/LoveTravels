using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 投票栏目表
    /// </summary>
    public partial class tb_Vote
    {
        /// <summary>
        /// 自动增长
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 归属栏目
        /// </summary>
        public int ClassId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 投票项
        /// </summary>
        public int ItemSum { get; set; }
        /// <summary>
        /// 投票总数
        /// </summary>
        public int VoteSum { get; set; }
        /// <summary>
        /// 投票类型，文字图片
        /// </summary>
        public bool Option { get; set; }
        /// <summary>
        /// 投票类型，文字图片
        /// </summary>
        public int VoteType { get; set; }
        /// <summary>
        /// 投票图片
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 是否开启时间限制
        /// </summary>
        public bool IsTime { get; set; }
        /// <summary>
        /// 开始 时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 投票介绍
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 添加日期
        /// </summary>
        public DateTime? AddDate { get; set; }

        public virtual ICollection<tb_VoteItem> tb_VoteItem { get; set; }
    }
}
