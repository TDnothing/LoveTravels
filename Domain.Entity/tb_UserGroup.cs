using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 会员组管理
    /// </summary>
    public partial class tb_UserGroup
    {
        /// <summary>
        /// 会员组ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 会员组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 会员组等级
        /// </summary>
        public int Grade { get; set; }
        /// <summary>
        /// 升级所需经验
        /// </summary>
        public int UpgradeExp { get; set; }
        /// <summary>
        /// 初始金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 初始积分
        /// </summary>
        public int Point { get; set; }
        /// <summary>
        /// 购物折扣
        /// </summary>
        public int Discount { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }

        public virtual ICollection<tb_User> tb_User { get; set; }
    }
}
