using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Domain.Entity;
using FytMsys.Helper;
using FytMsys.Common;
using Domain.ViewModel;

namespace FytMsys.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()

        {
            //查询我等你前10条
            var plq = from p in OperateSession.SetContext.lv_ProJect
                      where p.IsRecommend && p.Audit == 1
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
                          p.IsRecommend,
                          p.Region,
                          p.Hits,
                          StarNum = (int?)OperateSession.SetContext.tb_Comment.Where(m => m.ClassId == p.ID && m.Option == 1).Sum(m => m.Star) % OperateSession.SetContext.tb_Comment.Count(m => m.ClassId == p.ID) ?? 0,
                          p.AddTime
                      };
            ViewBag.pjlist = JsonConverter.JsonClass(plq.Take(8).ToList());
            //查询去看看前10条
            var glq = from p in OperateSession.SetContext.lv_GoLook
                      where p.Audit == 1
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
                          p.CoverImg,
                          p.ShowImg,
                          p.Rsum,
                          IsEffect = (int?)OperateSession.SetContext.lv_GoLookOrder.Where(m => m.LookId == p.ID).Count() ?? 0,
                          p.GoAddress,
                          p.ArriveTime,
                          p.Flags,
                          p.Hits,
                          p.Centents,
                          p.AddTime
                      };
            ViewBag.gllist = JsonConverter.JsonClass(glq.Take(10).ToList());

            #region 查询广告位

            ViewBag.advList =
                OperateContext<tb_AdvList>.SetServer.GetList(m => m.ClassId == 10 && m.Status, m => m.Sort, true)
                    .ToList();
            #endregion

            #region 查询幻灯片右侧2张广告位

            ViewBag.advRightList =
                OperateContext<tb_AdvList>.SetServer.GetList(m => m.ClassId == 12 && m.Status, m => m.Sort, true).Take(2)
                    .ToList();
            #endregion

            #region 查询优选精华的5个旅程故事

            ViewBag.iStory =
                OperateContext<lv_Story>.SetServer.GetList(m => m.IsTop == "1", m => m.UpdateTime, true).Take(5)
                    .ToList();
            #endregion

            #region 查询前10条特色旅程（我等你 + 去看看）
            var lq = (from a in OperateSession.SetContext.lv_GoLook
                      where a.IsSpecial && a.Audit == 1
                      select new { a.ID, a.Title, a.CoverImg, Types = 0, AddDate = a.UpdateTime }).Union(
                    from b in OperateSession.SetContext.lv_ProJect
                    where b.IsSpecial && b.Audit == 1
                    select new { b.ID, b.Title, b.CoverImg, Types = 1, AddDate = b.UpdateTime });
            lq = lq.OrderByDescending(m => m.AddDate);
            ViewBag.splist = JsonConverter.JsonClass(lq.Take(8).ToList());
            #endregion
       
            //ViewBag.isEnglish = false;
            //WebClient client = new WebClient();
            //string jsonURL = "http://pv.sohu.com/cityjson";
            //string sjson = client.DownloadString(jsonURL);
            //var json = new JavaScriptSerializer().Serialize(sjson);
            //if (sjson.IndexOf("UNITED STATES") != -1)
            //    ViewBag.isEnglish = true;

            return View();
        }



        public ActionResult Footers()
        {
            //根据父标题查询二级栏目列表
            ViewBag.aboutList = OperateContext<tb_Column>.SetServer.GetList(m => m.ParentId == 27, m => m.Sort, true).ToList();
            //根据父标题查询二级栏目列表
            ViewBag.helpList = OperateContext<tb_Column>.SetServer.GetList(m => m.ParentId == 26, m => m.Sort, true).ToList();
            return View();
        }

        /// <summary>
        /// 发布按钮初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult Release()
        {
            return View();
        }
        public ActionResult ReleaseEng()
        {
            return View();
        }

        /// <summary>
        /// 发布的条约
        /// </summary>
        /// <returns></returns>
        public ActionResult Treaty()
        {
            var model = OperateContext<tb_Column>.SetServer.GetModel(m => m.ID == 1025);
            return View(model);
        }
        public ActionResult TreatyEng()
        {
            var model = OperateContext<tb_Column>.SetServer.GetModel(m => m.ID == 1027);
            return View("Treaty", model);
        }

        /// <summary>
        /// 特色旅程
        /// </summary>
        /// <returns></returns>
        public ActionResult SpecialVacation(int page)
        {
            int pageSize = 20;
            var lq = (from a in OperateSession.SetContext.lv_GoLook
                      where a.IsSpecial && a.Audit == 1
                      select new { a.ID, a.Title, a.CoverImg, Types = 0, AddDate = a.UpdateTime }).Union(
                    from b in OperateSession.SetContext.lv_ProJect
                    where b.IsSpecial && b.Audit == 1
                    select new { b.ID, b.Title, b.CoverImg, Types = 1, AddDate = b.UpdateTime });
            lq = lq.OrderByDescending(m => m.AddDate);
            ViewBag.list = JsonConverter.JsonClass(lq.Skip(pageSize * (page - 1)).Take(pageSize).ToList());
            //分页控件
            ViewBag.pageModel = new PageHelper()
            {
                PageSize = pageSize,
                PageIndex = page,
                Rows = lq.Count(),
                Counts = Convert.ToInt32(Math.Ceiling((lq.Count() * 1.0) / pageSize)),
                Urls = "/home/specialvacation/" + page
            };
            //网站信息
            var siteModel = WebSiteHelper.GetSite(1);
            return View(siteModel);
        }
    }
}