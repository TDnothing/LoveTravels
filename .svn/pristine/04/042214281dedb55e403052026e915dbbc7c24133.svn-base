using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FytMsys.Logic.Admin
{
    /// <summary>
    /// 我等你后台管理
    /// </summary>
    public class FytProjectController : BaseController
    {
        #region 1、[我等你管理] 初始化 + Index()
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 2、[我等你管理] 加载数据 + IndexData()
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IndexData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询         
                int pageSize = FytRequest.GetFormInt("pageSize", 1),
                    pageIndex = FytRequest.GetFormInt("pageIndex", 10);
                var lq = from fp in OperateSession.SetContext.lv_ProJect
                         orderby fp.UpdateTime descending
                         select new
                         {
                             fp.ID,
                             fp.Number,
                             fp.tb_User.NickName,
                             fp.tb_User.TrueName,
                             fp.Title,
                             fp.Centents,
                             fp.Rsum,
                             fp.Price,
                             fp.IsTcjs,
                             fp.IsJcjs,
                             fp.IsApzs,
                             fp.Flags,
                             fp.IsRecommend,
                             fp.IsSpecial,
                             fp.Audit,
                             fp.UpdateTime
                         };
                if (key != "")
                {
                    lq = lq.Where(m => m.Title.Contains(key) || m.Centents.Contains(key));
                }
                jsonM.Data = lq.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                jsonM.PageRows = lq.Count();
                jsonM.PageTotal = Convert.ToInt32(Math.Ceiling((lq.Count() * 1.0) / pageSize));
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytProjectController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[我等你管理] 修改模板加载视图 + Modify(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Modify(int id)
        {
            var model = OperateContext<lv_ProJect>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new lv_ProJect() { AddTime = DateTime.Now, UpdateTime = DateTime.Now };
            }
            return View(model);
        }
        #endregion

        #region 4、[我等你管理] 修改模板加载视图 + Modify(lv_ProJect)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Modify(lv_ProJect model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                if (model.ID == 0)
                {
                    model.AddTime = DateTime.Now;
                }
                if (ModelState.IsValid)
                {
                    //添加或修改
                    model.UpdateTime = DateTime.Now;
                    OperateContext<lv_ProJect>.SetServer.SaveOrUpdate(model, model.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytProjectController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[我等你管理] 删除记录 + DeleteBy()
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
                    OperateContext<lv_ProjectOrder>.SetServer.DeleteBy(m => m.ProJectId == id);
                    OperateContext<lv_ProJect>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<lv_ProjectOrder>.SetServer.DeleteBy(m => result.Contains(m.ProJectId));
                    OperateContext<lv_ProJect>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytProjectController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 6、[我等你管理] 参与用户初始化 + UserIndex()
        /// <summary>
        /// 参与用户初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult UserIndex()
        {
            return View();
        }
        #endregion

        #region 7、[我等你管理] 参与用户加载数据 + UserIndexData()
        /// <summary>
        /// 参与用户加载数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserIndexData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询         
                int projectId = FytRequest.GetFormInt("projectId"),//我等你ID
                    pageSize = FytRequest.GetFormInt("pageSize", 1),
                    pageIndex = FytRequest.GetFormInt("pageIndex", 10);
                var lq = from po in OperateSession.SetContext.lv_ProjectOrder
                         where po.ProJectId == projectId
                         orderby po.AddTime descending
                         select new
                         {
                             po.ID,
                             po.tb_User.NickName,
                             po.tb_User.TrueName,
                             po.PayStatus,
                             po.PayName,
                             po.PayPrice,
                             po.AddTime
                         };
                if (key != "")
                {
                    lq = lq.Where(m => m.TrueName.Contains(key));
                }
                jsonM.Data = lq.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                jsonM.PageRows = lq.Count();
                jsonM.PageTotal = Convert.ToInt32(Math.Ceiling((lq.Count() * 1.0) / pageSize));
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytProjectController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 8、[我等你管理] 用户评论初始化 + UserCommentIndex()
        /// <summary>
        /// 用户评论初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult UserCommentIndex()
        {
            return View();
        }
        #endregion

        #region 9、[我等你管理] 用户评论加载数据 + UserCommentData()
        /// <summary>
        /// 用户评论加载数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserCommentData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询         
                int projectId = FytRequest.GetFormInt("projectId"),//我等你ID
                    pageSize = FytRequest.GetFormInt("pageSize", 1),
                    pageIndex = FytRequest.GetFormInt("pageIndex", 10);
                var lq = from c in OperateSession.SetContext.tb_Comment
                         where c.ClassId == projectId
                         orderby c.AddDate descending
                         select new
                         {
                             c.ID,
                             c.tb_User.NickName,
                             c.tb_User.TrueName,
                             c.Star,
                             c.Content,
                             c.AddDate
                         };
                if (key != "")
                {
                    lq = lq.Where(m => m.TrueName.Contains(key));
                }
                jsonM.Data = lq.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                jsonM.PageRows = lq.Count();
                jsonM.PageTotal = Convert.ToInt32(Math.Ceiling((lq.Count() * 1.0) / pageSize));
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytProjectController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 10、[我等你管理] 我等你修改特色旅程 + ChangeIsSpecial()
        /// <summary>
        /// 我等你修改特色旅程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeIsSpecial()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    var model = OperateContext<lv_ProJect>.SetServer.GetModel(m => m.ID == id);
                    if (model != null)
                    {
                        model.IsSpecial = !model.IsSpecial;
                        OperateContext<lv_ProJect>.SetServer.Update(model);
                    }
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    var list = OperateContext<lv_ProJect>.SetServer.GetList(m => result.Contains(m.ID), m => m.UpdateTime, false);
                    foreach (var item in list)
                    {
                        item.IsSpecial = !item.IsSpecial;
                        OperateContext<lv_ProJect>.SetServer.Update(item);
                    }
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "修改数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytProjectController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 11、[我等你管理] 我等你修改推荐 + ChangeIsRecommend()
        /// <summary>
        /// 我等你修改推荐
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeIsRecommend()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    var model = OperateContext<lv_ProJect>.SetServer.GetModel(m => m.ID == id);
                    if (model != null)
                    {
                        model.IsRecommend = !model.IsRecommend;
                        OperateContext<lv_ProJect>.SetServer.Update(model);
                    }
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    var list = OperateContext<lv_ProJect>.SetServer.GetList(m => result.Contains(m.ID), m => m.UpdateTime, false);
                    foreach (var item in list)
                    {
                        item.IsRecommend = !item.IsRecommend;
                        OperateContext<lv_ProJect>.SetServer.Update(item);
                    }
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "修改数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytProjectController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 12、[我等你管理] 我等你修改审核 + ChangeAudit()
        /// <summary>
        /// 我等你修改审核
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeAudit()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                var status = FytRequest.GetFormInt("status");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    var model = OperateContext<lv_ProJect>.SetServer.GetModel(m => m.ID == id);
                    if (model != null)
                    {
                        model.Audit = status == 1 ? model.Audit = 1 : model.Audit = 2;
                        OperateContext<lv_ProJect>.SetServer.Update(model);
                    }
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    var list = OperateContext<lv_ProJect>.SetServer.GetList(m => result.Contains(m.ID), m => m.UpdateTime, false);
                    foreach (var item in list)
                    {
                        item.Audit = status == 1 ? item.Audit = 1 : item.Audit = 2;
                        OperateContext<lv_ProJect>.SetServer.Update(item);
                    }
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "修改数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytProjectController), ex);
            }
            return Json(jsonM);
        }
        #endregion


    }
}
