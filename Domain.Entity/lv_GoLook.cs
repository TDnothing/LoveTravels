﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    /// <summary>
    /// 旅游-去看看
    /// </summary>
    public class lv_GoLook
    {
        /// <summary>
        /// 自动增长
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "编号")]
        public string Number { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        [Display(Name = "发布人")]
        public int UserId { get; set; }

        /// <summary>
        /// 去看看标题
        /// </summary>
        [MaxLength(500)]
        [Display(Name = "去看看标题")]
        public string Title { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        [Display(Name = "访问量")]
        public int Hits { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        [Display(Name = "封面图")]
        [MaxLength(500)]
        public string CoverImg { get; set; }

        /// <summary>
        /// 展示图
        /// </summary>
        [Display(Name = "展示图")]
        [MaxLength(500)]
        public string ShowImg { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        [MaxLength(500)]
        [Display(Name = "去看看目的地")]
        public string GoAddress { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        [MaxLength(10)]
        [Display(Name = "去看看人数")]
        public string Rsum { get; set; }

        /// <summary>
        /// 行程时间
        /// </summary>
        [MaxLength(10)]
        [Display(Name = "行程时间")]
        public string XcTime { get; set; }

        /// <summary>
        /// 类型，0=求带，1=组团
        /// </summary>
        [Display(Name = "类型，0=求带，1=组团")]
        public int Flags { get; set; }

        /// <summary>
        /// 是否为特色旅程
        /// </summary>
        [Display(Name = "是否为特色旅程")]
        public Boolean IsSpecial { get; set; }

        /// <summary>
        /// 审核状态，0=审核中，1=显示，2=禁止
        /// </summary>
        [Display(Name = "审核状态，0=审核中，1=显示，2=禁止")]
        public int Audit { get; set; }

        /// <summary>
        /// 预计抵达时间
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "预计抵达时间")]
        public string ArriveTime { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [MaxLength(10)]
        [Display(Name = "价格")]
        public string Price { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [MaxLength(10)]
        [Display(Name = "美元价格")]
        public string UsdPrice { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        public string Centents { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime UpdateTime { get; set; }

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