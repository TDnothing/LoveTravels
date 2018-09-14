using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 系统菜单管理
    /// </summary>
    public partial class tb_AdminMenu
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 系统菜单ID
        /// </summary>
        public int NodeID { get; set; }
        /// <summary>
        /// 菜单父ID
        /// </summary>
        public int ParentID { get; set; }
        /// <summary>
        /// 菜单等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 菜单域名
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 菜单控制器名
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// 菜单方法名
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 菜单传递方式
        /// </summary>
        public string FormMethod { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Ico { get; set; }
        /// <summary>
        /// 菜单排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否有外链
        /// </summary>
        public bool IsLink { get; set; }
        /// <summary>
        /// 外链地址
        /// </summary>
        public string LinkUrl { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Summary { get; set; }
    }
}
