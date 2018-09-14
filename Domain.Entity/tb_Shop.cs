using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 商铺管理
    /// </summary>
    public partial class tb_Shop
    {
        /// <summary>
        /// 商铺ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 商铺名
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 商铺图片
        /// </summary>
        public string ShopImage { get; set; }
        /// <summary>
        /// 商铺地址-省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 商铺地址-市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 商铺地址-区
        /// </summary>
        public string County { get; set; }
        /// <summary>
        /// 商铺地址-详细
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 商铺联系电话-手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 商铺联系电话-座机
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 服务范围ID列表
        /// </summary>
        public string ServerType { get; set; }
        /// <summary>
        /// 服务范围列表
        /// </summary>
        public string ServerTypeDetail { get; set; }
        /// <summary>
        /// 商铺坐标-经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 商铺坐标-纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }

        public virtual ICollection<tb_Goods> tb_Goods { get; set; }
    }
}
