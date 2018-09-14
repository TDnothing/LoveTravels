using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 下载管理
    /// </summary>
    public partial class tb_DownLoad
    {
        /// <summary>
        /// 下载ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 下载所属类型
        /// </summary>
        public int ClassId { get; set; }
        /// <summary>
        /// 下载标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 下载副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
        /// <summary>
        /// 下载数量
        /// </summary>
        public int DownSum { get; set; }
        /// <summary>
        /// 访问数
        /// </summary>
        public int Hits { get; set; }
        /// <summary>
        /// 适用系统
        /// </summary>
        public string IsSystem { get; set; }
        /// <summary>
        /// 软件类型
        /// </summary>
        public string IsCharge { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 缩略图地址
        /// </summary>
        public string ThumImg { get; set; }
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
        /// 是否有外链
        /// </summary>
        public bool IsLink { get; set; }
        /// <summary>
        /// 外链地址
        /// </summary>
        public string LinkUrl { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Audit { get; set; }
        /// <summary>
        /// 所属站点ID
        /// </summary>
        public int SiteID { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
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
        /// 修改时间
        /// </summary>
        public DateTime EditDate { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
        [ForeignKey("ClassId")]
        public virtual tb_Column tb_Column { get; set; }
    }
}
