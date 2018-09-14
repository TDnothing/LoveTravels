using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 预定商品管理
    /// </summary>
    public partial class tb_GoodsMess
    {
        /// <summary>
        /// 预定ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// 商品信息
        /// </summary>
        public string GoodsInfo { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public string GoodsNum { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string SendAddress { get; set; }
        /// <summary>
        /// 收货人名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 收货人手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 收货人座机
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 客户需求
        /// </summary>
        public string Requirement { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
