using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Logic.Admin
{
    public class IndexController : BaseController
    {
        #region 1、初始化管理主页 +Index()
        /// <summary>
        /// 初始化管理主页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            //获得当前站点
            ViewBag.siteModel = OperateContext<tb_Site>.SetServer.GetModel(SiteID);
            //获得站点列表
            ViewBag.siteList = OperateContext<tb_Site>.SetServer.GetList(m => true, m => m.ID, true).ToList();
            var userModel = GetAdminInfo();
            //查询未读留言总数
            ViewBag.meCount = OperateContext<tb_Message>.SetServer.Count(m => m.RepId == 0);
            //查询一级导航栏目
            var roid = userModel.RoleID;
            
            //查询首页右上角消息数量
            var msgcount = OperateContext<tb_Message>.SetServer.Count(PredicateBuilder.True<tb_Message>());
            ViewBag.msgcount = msgcount;
            var lastmsg = OperateContext<tb_Message>.SetServer.GetList(PredicateBuilder.True<tb_Message>(), m => m.AddDate, false).FirstOrDefault();
            if (lastmsg != null)
            {
                ViewBag.lastmsg = CommentHelper.DateStringFromNow(lastmsg.AddDate);
            }
            else
            {
                ViewBag.lastmsg = "";
            }

            return View(userModel);
        }
        #endregion

        #region 2、管理中心主页，左侧默认页 +Default()
        /// <summary>
        /// 管理中心主页，左侧默认页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Default()
        {
            //获得当前站点
            ViewBag.siteModel = OperateContext<tb_Site>.SetServer.GetModel(SiteID);
            var userModel = GetAdminInfo();
            //查询留言前5条列表
            ViewBag.messCount = OperateContext<tb_Message>.SetServer.Count(m => m.RepId == 0);
            int row = 0, totaPage = 0;
            ViewBag.messList = OperateContext<tb_Message>.SetServer.GetPageList(1, 5, out row, out totaPage,
                m => true, false, m => m.ID).ToList();

            #region 文章点击统计
            string strSql = "select SUM(Hits) p,MONTH(AddDate) as m from tb_Article  where YEAR(AddDate)=" + DateTime.Now.Year + " group by month(AddDate)";
            //tb_Article sss=new tb_Article();
            //dynamic tjLists = OperateContext<tb_Article>.SetServer.QueryEnumerable(sss, strSql);
            //foreach (dynamic b in tjLists)
            //{
            //    string a = b.p;
            //}

            dynamic tjList = OperateSession.SetContext.Database.SqlQueryForDynamic(strSql, null);
            DateTime dtNow = DateTime.Now;
            string sums = "";
            if (tjList != null)
            {
                for (int i = 1; i < 13; i++)
                {
                    int j = 0;
                    foreach (var item in tjList)
                    {
                        if (i == item.m)
                        {
                            j = 1;
                            sums += item.p + ",";
                        }
                    }
                    if (j == 0)
                        sums += "0,";
                }
            }
            else
            {
                for (int i = 1; i < 13; i++)
                {
                    sums += "0,";
                }
            }
            sums = sums.Substring(0, sums.Length - 1);
            ViewBag.tjSum = sums;
            #endregion

            return View(userModel);
        }
        #endregion

        #region 3、更改站点 +NewSite()
        /// <summary>
        /// 管理中心主页，左侧默认页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NewSite()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "站点更换成功！" };
            try
            {
                int sid = FytRequest.GetFormInt("sid");
                if (sid == SiteID)
                {
                    jsonM.Status = "n";
                    jsonM.Msg = "无需操作，目前就是您选中的站点！";
                    return Json(jsonM);
                }
                else
                {
                    UtilsHelper.WriteCookie("FYTSYS", KeyHelper.CACHE_SITE_LANGUAGE, sid.ToString(CultureInfo.InvariantCulture));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(IndexController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 4、退出登录 +OutLogin()
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OutLogin()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "退出成功！" };
            try
            {
                Session[KeyHelper.SESSION_ADMIN_INFO] = null;
                UtilsHelper.WriteCookie("FYTSYS", "FYTGUID", "");
                UtilsHelper.WriteCookie("FYTSYS", KeyHelper.CACHE_SITE_LANGUAGE, "");
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(IndexController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、清除系统缓存
        /// <summary>
        /// 清楚系统缓存
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearCache()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "站点更换成功！" };
            var cacheE = HttpContext.Cache.GetEnumerator();
            while (cacheE.MoveNext())
            {
                HttpContext.Cache.Remove(cacheE.Key.ToString());
            }
            return Json(jsonM);
        }

        #endregion
    }
}
