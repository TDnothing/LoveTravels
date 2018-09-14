using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Web.Controllers
{
    /// <summary>
    /// 单篇文章
    /// </summary>
    public class CategoryController:Controller
    {
        /// <summary>
        /// 根据栏目的英文标题查询内容
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public ActionResult Index(string flag,string en)
        {
            var pid = flag == "about" ? 27 : 26;
            ViewBag.cmodel = OperateContext<tb_Column>.SetServer.GetModel(m => m.ID == pid);
            //根据英文标题查询内容查询
            var model = OperateContext<tb_Column>.SetServer.GetModel(m => m.EnTitle == en);
            //根据父标题查询二级栏目列表
            ViewBag.navList = OperateContext<tb_Column>.SetServer.GetList(m => m.ParentId == pid, m => m.Sort, true).ToList();
            return View(model);
        }

        /// <summary>
        /// 帮助搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            string key = FytRequest.GetQueryString("key");
            var idList = new List<int> {26, 27};

            var q = from s in OperateSession.SetContext.tb_Column
                where s.Title.Contains(key) || s.Content.Contains(key) && idList.Contains(s.ParentId)
                select new
                {
                    s.ID,
                    s.Title,
                    s.Content,
                    s.ParentId,
                    s.EnTitle
                };
            ViewBag.list = JsonConverter.JsonClass(q.ToList());
            return View();
        }
    }
}