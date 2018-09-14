using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 收货地址管理
    /// </summary>
    public partial class tb_GoodsAddress
    {
        /// <summary>
        /// 收货地址ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 所属用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 收件人名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 收件人手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 收件人座机
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 收件地址-省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 收件地址-市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 收件地址-区
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 收件地址-详细
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }

        public virtual ICollection<tb_GoodsOrder> tb_GoodsOrder { get; set; }
    }
}
