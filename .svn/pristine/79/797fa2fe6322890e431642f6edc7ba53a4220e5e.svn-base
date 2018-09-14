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
    public class PictureController:BaseController
    {
        #region 1、[图文模板管理]  初始化+Index()
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

        #region 2、[图文模板管理] 获取数据+IndexData()
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
                var classId = FytRequest.GetFormInt("ClassId"); //栏目ID
                var where = PredicateBuilder.True<tb_Article>();
                where = where.And(m => m.IsRecyc == false);
                if (classId != 0)
                    where = where.And(m => m.ClassID == classId);
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                    where = where.Or(m => m.Summary.Contains(key));
                }
                if (audit != -1)
                {
                    where = where.And(m => m.Audit == audit);
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
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(PictureController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[图文模板管理] 修改模板加载视图+Modfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modfiy(int id)
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

        #region 4、[图文模板管理] 修改模板加载视图+Modfiy(tb_SysTemp model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modfiy(tb_Article model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "/FytAdmin/Picture/Index" };
            try
            {
                if (ModelState.IsValid)
                {
                    var reslist = FytRequest.GetFormString("imlist").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    model.AddDate = DateTime.Now;
                    if (model.ID == 0)
                    {
                        var tModel=OperateContext<tb_Article>.SetServer.Add(model, true);
                        var listModel = new List<tb_Picture>();
                        foreach (string s in reslist)
                        {
                            var u = FytRequest.GetFormString("file_name_" + s);
                            var su = FytRequest.GetFormString("file_summary_" + s);
                            var so = FytRequest.GetFormInt("file_sort_" + s);
                            listModel.Add(new tb_Picture()
                            {
                                ClassId = tModel.ID,
                                BigImg=u,
                                Title = su,
                                Flag=1,
                                IsCover=false,
                                Sort = so
                            });
                        }
                        OperateContext<tb_Picture>.SetServer.AddEntity(listModel);
                    }
                    else
                    {
                        OperateContext<tb_Article>.SetServer.Update(model);
                        //删除所有投票数据
                        OperateContext<tb_Picture>.SetServer.DeleteBy(m => m.ClassId == model.ID);
                        var listModel = new List<tb_Picture>();
                        foreach (string s in reslist)
                        {
                            var u = FytRequest.GetFormString("file_name_" + s);
                            var su = FytRequest.GetFormString("file_summary_" + s);
                            var so = FytRequest.GetFormInt("file_sort_" + s);
                            listModel.Add(new tb_Picture()
                            {
                                ClassId = model.ID,
                                BigImg = u,
                                Title = su,
                                Flag = 1,
                                IsCover = false,
                                Sort = so
                            });
                        }
                        OperateContext<tb_Picture>.SetServer.AddEntity(listModel);
                    }
                    jsonM.BackUrl = "/FytAdmin/Picture/Index?ClassId=" + model.ClassID;
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(PictureController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[图文模板管理] 删除记录+DeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteBy()
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
                LogHelper.WriteLog(typeof(PictureController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 6、[图文模板-删除到回收站] 删除记录+DeleteRecyc()
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
                LogHelper.WriteLog(typeof(PictureController), ex);
            }
            return Json(jsonM);
        }
        #endregion
        
    }
}
