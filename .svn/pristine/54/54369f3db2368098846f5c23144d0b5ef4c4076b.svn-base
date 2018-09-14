using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 商品品牌
    /// </summary>
    public partial class tb_GoodsBrand
    {
        /// <summary>
        /// 品牌ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 品牌名
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 品牌别名
        /// </summary>
        public string BrandAlias { get; set; }
        /// <summary>
        /// 品牌LOGO
        /// </summary>
        public string BrandLogo { get; set; }
        /// <summary>
        /// 品牌网址
        /// </summary>
        public string BrandWebSite { get; set; }
        /// <summary>
        /// 品牌描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 品牌备注
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }

        public virtual ICollection<tb_Goods> tb_Goods { get; set; }
    }
}
