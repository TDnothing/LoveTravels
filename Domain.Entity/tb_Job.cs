using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 招聘管理
    /// </summary>
    public partial class tb_Job
    {
        /// <summary>
        /// 招聘ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 所属类型ID
        /// </summary>
        public int ClassId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 招聘数
        /// </summary>
        public int JobSum { get; set; }
        /// <summary>
        /// 薪资
        /// </summary>
        public string Salary { get; set; }
        /// <summary>
        /// 工作经验
        /// </summary>
        public string WorkJy { get; set; }
        /// <summary>
        /// 工作类型
        /// </summary>
        public string WorkType { get; set; }
        /// <summary>
        /// 最低学历
        /// </summary>
        public string Degrees { get; set; }
        /// <summary>
        /// 工作地址
        /// </summary>
        public string WorkAddress { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
