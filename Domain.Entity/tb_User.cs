using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public partial class tb_User
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 所属会员组ID
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string LoginPwd { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPic { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string IsMarried { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 会员状态ID
        /// </summary>
        public int Types { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 安全问题
        /// </summary>
        public string SafeQuestion { get; set; }
        /// <summary>
        /// 安全答案
        /// </summary>
        public string SafeAnswer { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Point { get; set; }
        /// <summary>
        /// 经验
        /// </summary>
        public int Exp { get; set; }
        /// <summary>
        /// QQ号
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 用户地址-省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 用户地址-市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 用户地址-区
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 注册IP
        /// </summary>
        public string RegIp { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegDate { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime? LoginDate { get; set; }

        public virtual ICollection<tb_Comment> tb_Comment { get; set; }
        public virtual ICollection<tb_GoodsComment> tb_GoodsComment { get; set; }
        public virtual ICollection<tb_GoodsOrder> tb_GoodsOrder { get; set; }
        [ForeignKey("GroupId")]
        public virtual tb_UserGroup tb_UserGroup { get; set; }
        public virtual ICollection<tb_UserApp> tb_UserApp { get; set; }
        public virtual ICollection<tb_UserLog> tb_UserLog { get; set; }
        public virtual ICollection<tb_UserLoginOauth> tb_UserLoginOauth { get; set; }
        public virtual ICollection<tb_UserMoneyLog> tb_UserMoneyLog { get; set; }
        public virtual ICollection<tb_UserPointLog> tb_UserPointLog { get; set; }

        public virtual ICollection<tb_Blog> tb_Blog { get; set; }
    }
}
