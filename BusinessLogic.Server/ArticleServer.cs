using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Domain.Entity;
using FytMsys.DALMSSQL;

namespace BusinessLogic.Server
{
    public class ArticleServer
    {
        /// <summary>
        /// 实例化数据层方法
        /// </summary>
        protected BaseRepository<tb_Article> Repository = new BaseRepository<tb_Article>();

        /// <summary>
        /// 文章点击量增加
        /// </summary>
        public tb_Article ArticleHit(int articleId)
        {
            var model = Repository.GetModel(m => m.ID == articleId);
            //判断是否当前天
            if (Convert.ToDateTime(model.LastHitDate).Day == DateTime.Now.Day)
            {
                model.DayHits += 1;
            }
            //判断是否当前星期
            if (Convert.ToDateTime(model.LastHitDate).DayOfWeek == DateTime.Now.DayOfWeek)
            {
                model.WeedHits += 1;
            }
            //判断是否当前月份
            if (Convert.ToDateTime(model.LastHitDate).Month == DateTime.Now.Month)
            {
                model.MonthHits += 1;
            }
            //增加总的点击数
            model.Hits += 1;
            Repository.Update(model);
            return model;
        }
    }
}
