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
    /// 博客操作类
    /// </summary>
    public class tb_Blog
    {
        /// <summary>
        /// 博客ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 归属用户的ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 博客名称
        /// </summary>
        [MaxLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// 博客的描述
        /// </summary>
        [MaxLength(500)]
        public string Summary { get; set; }

        /// <summary>
        /// 博客等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 博客积分
        /// </summary>
        public int Integrate { get; set; }

        /// <summary>
        /// 博客访问量
        /// </summary>
        public int Hits { get; set; }

        public string Contents { get; set; }

         [ForeignKey("UserId")]
        public virtual tb_User tb_User { get; set; }
    }
}
