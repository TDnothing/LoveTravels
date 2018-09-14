using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Logic.Admin
{
    /// <summary>
    /// 广告视图
    /// </summary>
    public class AdvManageController : BaseController
    {
        /// <summary>
        /// 加载Iframe
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.flag = FytRequest.GetQueryInt("Flag", 0);
            return View();
        }

        #region 2、[广告栏目] 获取数据+IndexData()
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
                var flag = FytRequest.GetFormInt("flag");
                var where = PredicateBuilder.True<tb_AdvClass>();
                where = where.And(m => m.Flag == flag);
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize", 100),
                    pageIndex = FytRequest.GetFormInt("pageIndex", 1),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_AdvClass>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.ID)
                    .Select(m => new
                    {
                        m.ID,
                        m.Title,
                        m.Width,
                        m.Height
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(AdvManageController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[广告栏目] 修改模板加载视图+ClassModfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ClassModfiy(int id)
        {
            var flag = FytRequest.GetQueryInt("Flag", 0);
            var model = OperateContext<tb_AdvClass>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_AdvClass() { Status = true, Width = 0, Height = 0, ParentId = 0, Flag = flag };
            }
            return View(model);
        }
        #endregion

        #region 4、[广告栏目管理] 修改模板加载视图+ClassModfiy(tb_keyTags model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ClassModfiy(tb_AdvClass model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                model.SiteID = SiteID;
                if (ModelState.IsValid)
                {
                    //添加或修改
                    OperateContext<tb_AdvClass>.SetServer.SaveOrUpdate(model, model.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(AdvManageController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[广告栏目管理] 删除记录+DeleteBy()
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
                    OperateContext<tb_AdvClass>.SetServer.DeleteBy(m => m.ID == id);
                    OperateContext<tb_AdvList>.SetServer.DeleteBy(m => m.ClassId == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_AdvClass>.SetServer.DeleteBy(m => result.Contains(m.ID));
                    OperateContext<tb_AdvList>.SetServer.DeleteBy(m => result.Contains(m.ClassId));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(AdvManageController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        /// <summary>
        /// 广告位列表，根据栏目查询
        /// </summary>
        /// <returns></returns>
        public ActionResult ImgIndex()
        {
            ViewBag.classId = FytRequest.GetQueryInt("classId");
            return View();
        }

        #region 2、[广告详细管理] 获取数据+ImgData()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImgData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询
                var classId = FytRequest.GetFormInt("classId");
                var where = PredicateBuilder.True<tb_AdvList>();
                where = where.And(m => m.ClassId == classId);
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize", 100),
                    pageIndex = FytRequest.GetFormInt("pageIndex", 1),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_AdvList>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, true, m => m.Sort)
                    .Select(m => new
                    {
                        m.ID,
                        m.ClassId,
                        m.Title,
                        m.Sort,
                        m.Hits,
                        m.Status,
                        m.BeginTime,
                        m.EndTime,
                        m.Types
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(AdvManageController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[广告详细管理] 修改模板加载视图+ImageModfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImageModfiy(int id)
        {
            var classId = FytRequest.GetQueryInt("classId", 0);
            var classModel = OperateContext<tb_AdvClass>.SetServer.GetModel(m => m.ID == classId);
            ViewBag.classModel = classModel;
            var model = OperateContext<tb_AdvList>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                //获得最大的ID
                var maxModel = OperateContext<tb_AdvList>.SetServer.GetList(m => true, m => m.Sort, false).FirstOrDefault();
                var maxId = maxModel != null ? maxModel.Sort : 0;
                model = new tb_AdvList()
                {
                    Status = true,
                    Sort = maxId + 1,
                    Hits = 0,
                    ClassId = classModel.ID
                };
            }

            return View(model);
        }
        #endregion

        #region 4、[广告详细管理] 修改模板加载视图+ImageModfiy(tb_AdvList model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ImageModfiy(tb_AdvList model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                if (ModelState.IsValid)
                {
                    //添加或修改
                    OperateContext<tb_AdvList>.SetServer.SaveOrUpdate(model, model.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(AdvManageController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[广告详细管理] 删除记录+ImageDeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImageDeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    OperateContext<tb_AdvList>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_AdvList>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(AdvManageController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 6、[广告详细管理] 排序 +ColSort()
        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public ActionResult ColSort()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！" };
            try
            {
                int i = FytRequest.GetFormInt("i"),
                    o = FytRequest.GetFormInt("o"),
                    p = FytRequest.GetFormInt("p"),
                    a = 0, b = 0, c = 0;
                var list =
                    OperateContext<tb_AdvList>.SetServer.GetList(
                        PredicateBuilder.True<tb_AdvList>().And(m => m.ClassId == p)
                        , m => m.Sort, true).ToList();
                if (list.Count > 0)
                {
                    var index = 0;
                    foreach (var item in list)
                    {
                        index++;
                        if (index == 1)
                        {
                            if (item.ID == i) //判断是否是头如果上升则不做处理
                            {
                                if (o == 1) //下降一位
                                {
                                    a = Convert.ToInt32(item.Sort);
                                    b = Convert.ToInt32(list[index].Sort);
                                    c = a;
                                    a = b;
                                    b = c;
                                    item.Sort = a;
                                    OperateContext<tb_AdvList>.SetServer.Update(item);
                                    var nitem = list[index];
                                    nitem.Sort = b;
                                    OperateContext<tb_AdvList>.SetServer.Update(nitem);
                                    break;
                                }
                            }
                        }
                        else if (index == list.Count)
                        {
                            if (item.ID == i) //最后一条如果下降则不做处理
                            {
                                if (o == 0) //上升一位
                                {
                                    a = Convert.ToInt32(item.Sort);
                                    b = Convert.ToInt32(list[index - 2].Sort);
                                    c = a;
                                    a = b;
                                    b = c;
                                    item.Sort = a;
                                    OperateContext<tb_AdvList>.SetServer.Update(item);
                                    var nitem = list[index - 2];
                                    nitem.Sort = b;
                                    OperateContext<tb_AdvList>.SetServer.Update(nitem);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (item.ID == i) //判断是否是头如果上升则不做处理
                            {
                                if (o == 1) //下降一位
                                {
                                    a = Convert.ToInt32(item.Sort);
                                    b = Convert.ToInt32(list[index].Sort);
                                    c = a;
                                    a = b;
                                    b = c;
                                    item.Sort = a;
                                    OperateContext<tb_AdvList>.SetServer.Update(item);
                                    var nitem = list[index];
                                    nitem.Sort = b;
                                    OperateContext<tb_AdvList>.SetServer.Update(nitem);
                                    break;
                                }
                                else
                                {
                                    a = Convert.ToInt32(item.Sort);
                                    b = Convert.ToInt32(list[index - 2].Sort);
                                    c = a;
                                    a = b;
                                    b = c;
                                    item.Sort = a;
                                    OperateContext<tb_AdvList>.SetServer.Update(item);
                                    var nitem = list[index - 2];
                                    nitem.Sort = b;
                                    OperateContext<tb_AdvList>.SetServer.Update(nitem);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ColumnController), ex);
            }
            return Json(jsonM);
        }
        #endregion
    }
}
