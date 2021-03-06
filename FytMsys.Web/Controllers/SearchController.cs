﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.ViewModel;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Web.Controllers
{
    /// <summary>
    /// 顶部搜索
    /// </summary>
    public class SearchController : Controller
    {
        /// <summary>
        /// 搜索结果
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var key = FytRequest.GetQueryString("key"); //关键字
            var city = FytRequest.GetQueryString("city"); //想要去的地方
            if (city == "all")
            {
                city = "";
            }
            var page = FytRequest.GetQueryInt("page", 1); //分页
            const int pageSize = 20;
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            #region  业务处理
            var lq = from p in OperateSession.SetContext.lv_ProJect
                     orderby p.UpdateTime descending
                     select new
                     {
                         p.ID,
                         p.UserId,
                         p.tb_User.NickName,
                         p.tb_User.TrueName,
                         p.tb_User.Types,
                         p.tb_User.HeadPic,
                         p.Title,
                         p.ShowImg,
                         p.CoverImg,
                         p.Region,
                         p.Hits,
                         StarNum = (int?)OperateSession.SetContext.tb_Comment.Where(m => m.ClassId == p.ID && m.Option == 1).Sum(m => m.Star) % OperateSession.SetContext.tb_Comment.Count(m => m.ClassId == p.ID) ?? 0,
                         p.AddTime,
                         p.UpdateTime
                     };
            if (key != "")
            {
                lq = lq.Where(m => m.Title.Contains(key) || m.NickName.Contains(key) || m.TrueName.Contains(key) || m.Region.Contains(key));
            }
            if (city!="")
            {
                lq = lq.Where(m => m.Region.Contains(city));
            }
            ViewBag.rows = lq.Count();
            ViewBag.list = JsonConverter.JsonClass(lq.Skip(pageSize * (page - 1)).Take(pageSize).ToList());
            //分页控件
            ViewBag.pageModel = new PageHelper()
            {
                PageSize = pageSize,
                PageIndex = page,
                Rows = lq.Count(),
                Counts = Convert.ToInt32(Math.Ceiling((lq.Count() * 1.0) / pageSize)),
                Urls = "/search/?key="+key+"&city="+city
            };
            #endregion
            stopwatch.Stop();
            ViewBag.seconds = stopwatch.Elapsed.TotalSeconds;
            return View();
        }
	}
}