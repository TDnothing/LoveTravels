using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 广告管理
    /// </summary>
    public partial class tb_AdvList
    {
        /// <summary>
        /// 广告ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 广告所属栏目ID
        /// </summary>
        public int ClassId { get; set; }
        /// <summary>
        /// 广告标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 广告类别ID
        /// </summary>
        public int Types { get; set; }
        /// <summary>
        /// 广告状态
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 广告图片
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 广告链接
        /// </summary>
        public string LinkUrl { get; set; }
        /// <summary>
        /// 广告链接介绍
        /// </summary>
        public string LinkSummary { get; set; }
        /// <summary>
        /// 广告跳转方式
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// 广告轮播效果代码
        /// </summary>
        public string AdvCode { get; set; }
        /// <summary>
        /// 是否有时间限制
        /// </summary>
        public bool IsTimeLimit { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 点击量
        /// </summary>
        public int Hits { get; set; }

        [ForeignKey("ClassId")]
        public virtual tb_AdvClass tb_AdvClass { get; set; }
    }
}
