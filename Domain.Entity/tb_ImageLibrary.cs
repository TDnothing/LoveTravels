using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 图片管理
    /// </summary>
    public partial class tb_ImageLibrary
    {
        /// <summary>
        /// 图片ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 图片外联ID
        /// </summary>
        public int ImgID { get; set; }
        /// <summary>
        /// 图片所属类型
        /// </summary>
        public int ImgType { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 缩略图地址
        /// </summary>
        public string ImgSmall { get; set; }
        /// <summary>
        /// 是否为封面
        /// </summary>
        public bool IsCover { get; set; }

    }
}
