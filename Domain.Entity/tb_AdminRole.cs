using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 管理员角色表
    /// </summary>
    public partial class tb_AdminRole
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>
        public string RoleNumber { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLock { get; set; }
        /// <summary>
        /// 是否拥有编辑权限
        /// </summary>
        public bool IsEdit { get; set; }
        /// <summary>
        /// 是否拥有删除权限
        /// </summary>
        public bool IsDel { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        public System.DateTime AddDate { get; set; }

        public virtual ICollection<tb_Admin> tb_Admin { get; set; }
    }
}
