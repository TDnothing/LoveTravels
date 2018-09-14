using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 网站总栏目管理
    /// </summary>
    public partial class tb_Column
    {
        /// <summary>
        /// 总栏目ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        ///总栏目编号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 总栏目名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 总栏目英文名
        /// </summary>
        public string EnTitle { get; set; }
        /// <summary>
        /// 总栏目副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 总栏目父ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 总栏目类别列表
        /// </summary>
        public string ClassList { get; set; }
        /// <summary>
        /// 总栏目类别层级
        /// </summary>
        public int ClassLayer { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 类型ID
        /// </summary>
        public int TypeID { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public string Attr { get; set; }
        /// <summary>
        /// 所属站点ID
        /// </summary>
        public int SiteID { get; set; }
        /// <summary>
        /// 所属模板ID
        /// </summary>
        public int TempId { get; set; }
        /// <summary>
        /// 所属模板链接
        /// </summary>
        public string TempUrl { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string LinkUrl { get; set; }
        /// <summary>
        /// 手机端链接地址
        /// </summary>
        public string WapLinkUrl { get; set; }
        /// <summary>
        /// 是否头部显示
        /// </summary>
        public bool IsTopShow { get; set; }
        /// <summary>
        /// 是否手机端显示
        /// </summary>
        public bool IsWapShow { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 概述
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 详细介绍
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }

        public virtual ICollection<tb_Article> tb_Article { get; set; }
        [ForeignKey("TempId")]
        public virtual tb_SysTemp tb_SysTemp { get; set; }
        public virtual ICollection<tb_DownLoad> tb_DownLoad { get; set; }
        public virtual ICollection<tb_Goods> tb_Goods { get; set; }
        public virtual ICollection<tb_Message> tb_Message { get; set; }
    }
}
