using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 文章管理
    /// </summary>
    public partial class tb_Article
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 文章所属栏目ID
        /// </summary>
        public int ClassID { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 文章标题栏目
        /// </summary>
        public string TitleColor { get; set; }
        /// <summary>
        /// 文章副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 是否有链接
        /// </summary>
        public bool IsLink { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string LinkUrl { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 点击量
        /// </summary>
        public int Hits { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 缩略图地址
        /// </summary>
        public string ThumImg { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        public string VideoUrl { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }
        /// <summary>
        /// 是否热门
        /// </summary>
        public bool IsHot { get; set; }
        /// <summary>
        /// 是否滚动
        /// </summary>
        public bool IsScroll { get; set; }
        /// <summary>
        /// 是否轮播
        /// </summary>
        public bool IsSlide { get; set; }
        /// <summary>
        /// 是否评论
        /// </summary>
        public bool IsComment { get; set; }
        /// <summary>
        /// 是否在手机端显示
        /// </summary>
        public bool IsWap { get; set; }
        /// <summary>
        /// 是否回收
        /// </summary>
        public bool IsRecyc { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int Audit { get; set; }
        /// <summary>
        /// 所属站点ID
        /// </summary>
        public int SiteID { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 详细描述
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 日点击量
        /// </summary>
        public int DayHits { get; set; }
        /// <summary>
        /// 周点击量
        /// </summary>
        public int WeedHits { get; set; }
        /// <summary>
        /// 月点击量
        /// </summary>
        public int MonthHits { get; set; }
        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime EditDate { get; set; }
        /// <summary>
        /// 最后点击时间
        /// </summary>
        public DateTime? LastHitDate { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DelDate { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddDate { get; set; }

        [ForeignKey("ClassID")]
        public virtual tb_Column tb_Column { get; set; }
    }
}
