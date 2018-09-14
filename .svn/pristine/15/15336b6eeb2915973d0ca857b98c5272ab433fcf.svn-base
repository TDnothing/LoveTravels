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
    /// 提现银行
    /// </summary>
    public class lv_Bank
    {
        /// <summary>
        /// 自动增长
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 归属用户ID
        /// </summary>
        [Display(Name = "归属用户ID")]
        public int UserId { get; set; }

        /// <summary>
        /// 收款银行
        /// </summary>
        [MaxLength(100)]
        [Display(Name = "收款银行")]
        public string BankName { get; set; }

        /// <summary>
        /// 收款银行账号
        /// </summary>
        [MaxLength(100)]
        [Display(Name = "收款银行")]
        public string BankAccont { get; set; }

        /// <summary>
        /// 收款银行所在地区
        /// </summary>
        [MaxLength(100)]
        [Display(Name = "收款银行")]
        public string BankAddress { get; set; }

        /// <summary>
        /// 收款人姓名
        /// </summary>
        [MaxLength(100)]
        [Display(Name = "收款人姓名")]
        public string UserName { get; set; }


        /// <summary>
        /// 添加时间
        /// </summary>
        [Display(Name = "添加时间")]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 关联到用户表
        /// </summary>
        [ForeignKey("UserId")]
        public virtual tb_User tb_User { get; set; }
    }
}
