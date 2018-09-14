using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Logic.Admin
{
    /// <summary>
    /// 内容管理控制器
    /// </summary>
    public class ContentController : BaseController
    {
        /// <summary>
        /// 加载文档管理器
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            return View();
        }

        /// <summary>
        /// 加载左侧tree
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTree()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var where = PredicateBuilder.True<tb_Column>().And(m => m.SiteID == SiteID).And(m => m.TypeID == 1);
                var coluList = OperateContext<tb_Column>.SetServer.GetList(where, m => m.ID, true).ToList()
                .Select(m => new
                {
                    id = m.ID,
                    pId = m.ParentId,
                    name = m.Title,
                    open = true,
                    target = "DeployBase",
                    url = m.TempUrl + m.ID.ToString(CultureInfo.InvariantCulture)
                });
                jsonM.Data = coluList;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ContentController), ex);
            }
            return Json(jsonM);
        }


        #region 1、[文章管理]  初始化+Index()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ClassId = FytRequest.GetQueryInt("ClassId", 0);
            ViewBag.Recyc = 0;
            return View();
        }
        #endregion

        #region 2、[文章管理] 获取数据+IndexData()
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
                var classId = FytRequest.GetFormInt("ClassId"); //栏目ID
                var recyc = FytRequest.GetFormInt("Recyc", 0); //是否回收站1=是 0=否
                var where = PredicateBuilder.True<tb_Article>();
                where = where.And(m => m.SiteID == SiteID);
                if (classId != 0)
                    where = where.And(m => m.ClassID == classId);
                @where = recyc == 0 ? @where.And(m => m.IsRecyc == false) : @where.And(m => m.IsRecyc == true);
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
                var list = OperateContext<tb_Article>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.EditDate)
                    .Select(m => new
                    {
                        Cname = m.tb_Column.Title,
                        m.Title,
                        m.EditDate,
                        m.Hits,
                        m.Audit,
                        m.ImgUrl,
                        m.ID,
                        m.IsHot,
                        m.IsTop,
                        m.IsScroll,
                        m.IsSlide,
                        m.IsComment,
                        m.Author
                    ,
                        m.LastHitDate,
                        m.DayHits,
                        m.WeedHits,
                        m.MonthHits
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ContentController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[文章管理] 修改模板加载视图+ArticleModfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ArticleModfiy(int id)
        {
            var model = OperateContext<tb_Article>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_Article() { SiteID = SiteID, EditDate = DateTime.Now, Audit = 1, Author = GetAdminInfo().RealName };
            }
            model.ClassID = FytRequest.GetQueryInt("ClassId", 0);
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

        #region 4、[文章管理] 修改模板加载视图+ArticleModfiy(tb_Article model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ArticleModfiy(tb_Article model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "/FytAdmin/Content/Index" };
            try
            {
                if (ModelState.IsValid)
                {
                    model.AddDate = DateTime.Now;
                    //添加或修改
                    OperateContext<tb_Article>.SetServer.SaveOrUpdate(model, model.ID);
                    jsonM.BackUrl = "/FytAdmin/Content/Index?ClassId=" + model.ClassID;
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ContentController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[文章管理] 删除记录+ArticleDeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleDeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    OperateContext<tb_Article>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_Article>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ContentController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 6、[文章管理-删除到回收站] 删除记录+DeleteRecyc()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteRecyc()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    var model = OperateContext<tb_Article>.SetServer.GetModel(id);
                    model.IsRecyc = true;
                    OperateContext<tb_Article>.SetServer.Update(model, new string[] { "IsRecyc" });
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    var mList = OperateContext<tb_Article>.SetServer.GetList(m => result.Contains(m.ID), m => m.ID, true).ToList();
                    foreach (var art in mList)
                    {
                        art.IsRecyc = true;
                    }
                    OperateContext<tb_Article>.SetServer.Update(mList);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ContentController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 7、[文章管理-回收站列表]  初始化+RecycIndex()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RecycIndex()
        {
            ViewBag.ClassId = FytRequest.GetQueryInt("ClassId", 0);
            ViewBag.Recyc = 1;
            return View();
        }
        #endregion

        #region 8、[文章管理-清空回收站]  初始化+EmptyRecyc()
        /// <summary>
        /// 文章管理-清空回收站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EmptyRecyc()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                OperateContext<tb_Article>.SetServer.DeleteBy(m => m.IsRecyc == true);
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "清空数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ContentController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 9、[文章管理-还原回收站] 还原回收站+ReductionRecyc()
        /// <summary>
        /// 还原回收站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReductionRecyc()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    var model = OperateContext<tb_Article>.SetServer.GetModel(id);
                    model.IsRecyc = false;
                    OperateContext<tb_Article>.SetServer.Update(model, new string[] { "IsRecyc" });
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    var mList = OperateContext<tb_Article>.SetServer.GetList(m => result.Contains(m.ID), m => m.ID, true).ToList();
                    foreach (var art in mList)
                    {
                        art.IsRecyc = false;
                    }
                    OperateContext<tb_Article>.SetServer.Update(mList);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ContentController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion


        #region 10、[文章点击排行管理]  初始化+HitsIndex()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HitsIndex()
        {
            return View();
        }
        #endregion

        #region 11、[文章点击排行管理] 获取数据+IndexData()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult HitsIndexData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询
                var audit = FytRequest.GetFormInt("audit"); //日志类型查询
                string beginTime = FytRequest.GetFormString("beginTime"),  //日志开始时间
                    endTime = FytRequest.GetFormString("endTime"); //日志结束时间
                var classId = FytRequest.GetFormInt("ClassId"); //栏目ID
                var recyc = FytRequest.GetFormInt("Recyc", 0); //是否回收站1=是 0=否
                var where = PredicateBuilder.True<tb_Article>();
                if (classId != 0)
                    where = where.And(m => m.ClassID == classId);
                @where = recyc == 0 ? @where.And(m => m.IsRecyc == false) : @where.And(m => m.IsRecyc == true);
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
                var list = OperateContext<tb_Article>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.Hits)
                    .Select(m => new
                    {
                        Cname = m.tb_Column.Title,
                        m.Title,
                        m.EditDate,
                        m.Hits,
                        m.Audit,
                        m.ImgUrl,
                        m.ID,
                        m.IsHot,
                        m.IsTop,
                        m.IsScroll,
                        m.IsSlide,
                        m.IsComment,
                        m.Author,
                        m.LastHitDate,
                        m.DayHits,
                        m.WeedHits,
                        m.MonthHits
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ContentController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion



        #region 12、[复制转移加载下拉选择] +BatchIndex()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        public ActionResult BatchIndex()
        {
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
            return View();
        }
        #endregion

        #region 12、[复制转移加载下拉选择] +BatchIndex()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        public ActionResult BatchSave()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "操作成功", BackUrl = "close" };
            try
            {
                var type = FytRequest.GetFormInt("type"); //判断0=转移  1=复制
                var item = FytRequest.GetFormString("item"); //需要转移或复制ID的集合
                var classID = FytRequest.GetFormInt("ClassID"); //将要转移或复制指定栏目的ID
                List<int> result = new List<string>(item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                if (type == 0)
                {
                    //执行转移操作
                    var list = OperateContext<tb_Article>.SetServer.GetList(m => result.Contains(m.ID), m => m.ID, true).ToList();
                    list.ForEach(m => { m.ClassID = classID; });
                    OperateContext<tb_Article>.SetServer.Update(list);
                }
                else
                {
                    //执行复制操作
                    var list = OperateContext<tb_Article>.SetServer.GetList(m => result.Contains(m.ID), m => m.ID, true).ToList();
                    list.ForEach(m =>
                    {
                        m.ClassID = classID;
                        m.AddDate = DateTime.Now;
                        m.EditDate = DateTime.Now;
                    });
                    OperateContext<tb_Article>.SetServer.AddEntity(list);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "保存数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ContentController), ex);
            }
            return Json(jsonM);
        }
        #endregion


    }
}
