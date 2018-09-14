using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Logic.Admin
{
    public class DownLoadController:BaseController
    {
        #region 1、[下载管理]  初始化+Index()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ClassId = FytRequest.GetQueryInt("ClassId", 0);
            return View();
        }
        #endregion

        #region 2、[下载管理] 获取数据+IndexData()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IndexData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询
                var audit = FytRequest.GetFormInt("audit"); //日志类型查询
                string beginTime = FytRequest.GetFormString("beginTime"),  //日志开始时间
                    endTime = FytRequest.GetFormString("endTime"); //日志结束时间
                var classId = FytRequest.GetFormInt("ClassId");
                var where = PredicateBuilder.True<tb_DownLoad>();
                where = where.And(m => m.ClassId==classId);
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                    where = where.Or(m => m.Summary.Contains(key));
                }
                if (audit != -1)
                {
                    where = where.And(m => m.Audit == audit);
                }
                if (beginTime != "" && endTime != "")
                {
                    var bt = Convert.ToDateTime(beginTime);
                    var et = Convert.ToDateTime(endTime);
                    where = where.And(m => m.EditDate >= bt && m.EditDate <= et);
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_DownLoad>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.EditDate)
                    .Select(m => new { Cname = m.tb_Column.Title, m.Title, m.EditDate, m.Hits, m.Audit, m.ImgUrl, m.ID, m.IsHot, m.IsTop, m.IsScroll, m.IsSlide, m.IsComment, m.FileSize,m.FileType });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(DownLoadController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[下载管理] 修改模板加载视图+DownLoadModfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DownLoadModfiy(int id)
        {
            var model = OperateContext<tb_DownLoad>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_DownLoad() { SiteID = SiteID, EditDate = DateTime.Now, Audit = 1, IsSystem = "Win8/Win7/Vista/Win2003/WinXP" };
            }
            model.ClassId = FytRequest.GetQueryInt("ClassId", 0);
            //父级下拉
            var sList = ColumnHelper.GetTreeList(1);
            var pardrop = sList.Select(p => new SelectListItem
            {
                Text =
                    UtilsHelper.StringOfChar(Convert.ToInt32(p.ClassLayer) - 1, "　") + (p.ClassLayer == 1 ? "" : "├ ") +
                    p.Title,
                Value = p.ID.ToString(CultureInfo.InvariantCulture)
            }).ToList();
            ViewBag.pardrop = pardrop.AsEnumerable();
            return View(model);
        }
        #endregion

        #region 4、[下载管理] 修改模板加载视图+DownLoadModfiy(tb_DownLoad model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DownLoadModfiy(tb_DownLoad model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "/FytAdmin/DownLoad/Index" };
            try
            {
                if (ModelState.IsValid)
                {
                    model.AddDate = DateTime.Now;
                    //添加或修改
                    OperateContext<tb_DownLoad>.SetServer.SaveOrUpdate(model, model.ID);
                    jsonM.BackUrl = "/FytAdmin/DownLoad/Index?ClassId=" + model.ClassId;
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(DownLoadController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[下载管理] 删除记录+DownLoadDeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DownLoadDeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    OperateContext<tb_DownLoad>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_DownLoad>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(DownLoadController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion
    }
}
