using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace FytMsys.Common
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public class EnumHelper
    {
        public class EnumExt { static public List<ListItem> ToListItem<T>() { List<ListItem> li = new List<ListItem>(); foreach (int s in Enum.GetValues(typeof(T))) { li.Add(new ListItem { Value = s.ToString(), Text = Enum.GetName(typeof(T), s) }); } return li; } }

        //月份枚举(短)
        public enum SMonthEnum
        {
            /// <summary>
            /// 一月
            /// </summary>
            Jan = 1,
            /// <summary>
            /// 二月
            /// </summary>
            Feb = 2,
            /// <summary>
            /// 三月
            /// </summary>
            Mar = 3,
            /// <summary>
            /// 四月
            /// </summary>
            Apr = 4,
            /// <summary>
            /// 五月
            /// </summary>
            May = 5,
            /// <summary>
            /// 六月
            /// </summary>
            Jun = 6,
            /// <summary>
            /// 七月
            /// </summary>
            Jul = 7,
            /// <summary>
            /// 八月
            /// </summary>
            Aug = 8,
            /// <summary>
            /// 九月
            /// </summary>
            Sep = 9,
            /// <summary>
            /// 十月
            /// </summary>
            Oct = 10,
            /// <summary>
            /// 十一月
            /// </summary>
            Nov = 11,
            /// <summary>
            /// 十二月
            /// </summary>
            Dec = 12
        }

        //月份枚举(长)
        public enum LMonthEnum
        {
            /// <summary>
            /// 一月
            /// </summary>
            January = 1,
            /// <summary>
            /// 二月
            /// </summary>
            February = 2,
            /// <summary>
            /// 三月
            /// </summary>
            March = 3,
            /// <summary>
            /// 四月
            /// </summary>
            April = 4,
            /// <summary>
            /// 五月
            /// </summary>
            May = 5,
            /// <summary>
            /// 六月
            /// </summary>
            June = 6,
            /// <summary>
            /// 七月
            /// </summary>
            July = 7,
            /// <summary>
            /// 八月
            /// </summary>
            August = 8,
            /// <summary>
            /// 九月
            /// </summary>
            September = 9,
            /// <summary>
            /// 十月
            /// </summary>
            October = 10,
            /// <summary>
            /// 十一月
            /// </summary>
            November = 11,
            /// <summary>
            /// 十二月
            /// </summary>
            December = 12
        }

        /// <summary>
        /// 订单类型枚举
        /// </summary>
        public enum OrderStatusEnum
        {
            无效 = -1,
            待付款 = 0,
            待发货 = 1,
            待收货 = 2,
            已完成 = 3
        }

        public enum ReturnOrderStatusEnum
        {
            待审核 = 0,
            审核通过 = 1,
            审核未通过 = -1
        }

        /// <summary>
        /// 图片类型枚举
        /// </summary>
        public enum ImageEnum
        {
            /// <summary>
            /// 问题图片
            /// </summary>
            Question = 1,
            /// <summary>
            /// 商品图片
            /// </summary>
            Goods = 4
        }

        /// <summary>
        /// 评论枚举
        /// </summary>
        public enum CommentEnum
        {
            /// <summary>
            /// 商品评论
            /// </summary>
            Goods = 1
        }

        /// <summary>
        /// 收藏枚举
        /// </summary>
        public enum CollectEnum
        {
            /// <summary>
            /// 文章收藏
            /// </summary>
            Goods = 1
        }

        /// <summary>
        /// 关注枚举
        /// </summary>
        public enum AttentionEnum
        {
            /// <summary>
            /// 文章关注
            /// </summary>
            Article = 0,
            /// <summary>
            /// 技师关注
            /// </summary>
            Master = 1,
            /// <summary>
            /// 商铺关注
            /// </summary>
            Shop = 2,
            /// <summary>
            /// 预约关注
            /// </summary>
            Order = 3
        }

        /// <summary>
        /// 会员积分策略枚举
        /// </summary>
        public enum UserPoint
        {
            Add = 1,
            Del = 2
        }
        /// <summary>
        /// 会员积分策略枚举值
        /// </summary>
        public enum UserPointVal
        {
            Registered = 20,
            Attendance = 5
        }

    }
}
