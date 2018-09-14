using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 广告栏目分类管理
    /// </summary>
    public partial class tb_AdvClass
    {
        /// <summary>
        /// 广告栏目ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 广告栏目父ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 广告栏目标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 广告栏目类别
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// 广告栏目类型
        /// </summary>
        public string Types { get; set; }
        /// <summary>
        /// 广告栏目宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 广告栏目高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 广告栏目介绍
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 广告栏目状态
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 广告栏目所属站点ID
        /// </summary>
        public int SiteID { get; set; }

        public virtual ICollection<tb_AdvList> tb_AdvList { get; set; }
    }
}
