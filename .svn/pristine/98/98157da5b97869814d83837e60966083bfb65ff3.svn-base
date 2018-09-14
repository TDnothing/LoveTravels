using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 博客文章类
    /// </summary>
    public class tb_BlogActicle
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 文章所属栏目ID
        /// </summary>
        public int BlogID { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        [MaxLength(300)]
        public string Title { get; set; }
        /// <summary>
        /// 文章副标题
        /// </summary>
        [MaxLength(500)]
        public string SubTitle { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [MaxLength(50)]
        public string Author { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        [MaxLength(300)]
        public string Source { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [MaxLength(300)]
        public string Tag { get; set; }
        /// <summary>
        /// 点击量
        /// </summary>
        public int Hits { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        [MaxLength(300)]
        public string ImgUrl { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }
        /// <summary>
        /// 是否评论
        /// </summary>
        public bool IsComment { get; set; }
        /// <summary>
        /// 是否回收
        /// </summary>
        public bool IsRecyc { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int Audit { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        [MaxLength(300)]
        public string KeyWord { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [MaxLength(2000)]
        public string Summary { get; set; }
        /// <summary>
        /// 详细描述
        /// </summary>
        public string Content { get; set; }
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
        public DateTime AddDate { get; set; }

        [ForeignKey("BlogID")]
        public virtual tb_Blog tb_Blog { get; set; }
    }
}
