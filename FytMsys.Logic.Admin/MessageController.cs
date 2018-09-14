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
    public class MessageController:BaseController
    {
        #region 1、[留言管理]  初始化+Index()
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

        #region 2、[留言管理] 获取数据+IndexData()
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
                string beginTime = FytRequest.GetFormString("beginTime"),  //日志开始时间
                    endTime = FytRequest.GetFormString("endTime"); //日志结束时间
                var classId = FytRequest.GetFormInt("ClassId");
                var where = PredicateBuilder.True<tb_Message>();
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                    where = where.Or(m => m.Content.Contains(key));
                }
                where = where.And(m => m.ClassId==classId);
                where = where.And(m => m.ParentId == 0);
                if (beginTime != "" && endTime != "")
                {
                    var bt = Convert.ToDateTime(beginTime);
                    var et = Convert.ToDateTime(endTime);
                    where = where.And(m => m.AddDate >= bt && m.AddDate <= et);
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_Message>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.AddDate)
                    .Select(m => new { Cname = m.tb_Column.Title, m.Title,m.ParentId,m.Content,m.ID,m.Mobile,m.Email,m.QQ,m.AddDate});
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MessageController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[留言管理] 修改模板加载视图+Modfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modfiy(int id)
        {
            var model = OperateContext<tb_Message>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_Message() {};
            }
            model.ClassId = FytRequest.GetQueryInt("ClassId", 0);
            return View(model);
        }
        #endregion

        #region 4、[留言管理] 修改模板加载视图+Modfiy(tb_Message model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Modfiy(tb_Message model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "/FytAdmin/Message/Index" };
            try
            {
                if (ModelState.IsValid)
                {
                    model.AddDate = DateTime.Now;
                    //添加或修改
                    OperateContext<tb_Message>.SetServer.SaveOrUpdate(model, model.ID);
                    jsonM.BackUrl = "/FytAdmin/Message/Index?ClassId=" + model.ClassId;
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MessageController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[留言管理] 删除记录+DeleteBy()
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
                    OperateContext<tb_Message>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_Message>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MessageController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        /// <summary>
        /// 获得详细的留言信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail()
        {
            int id = FytRequest.GetFormInt("id");
            var data = OperateContext<tb_MessageRep>.SetServer.GetList(m => m.MessId == id, m => m.AddDate,true)
                .Select(m=>new{m.Content,m.AddDate,m.UserId,m.UserName,m.ParentId,m.ID,m.MessId});
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Data = data };
            return MyJson(jsonM, "yyyy-MM-dd");
        }

        /// <summary>
        /// 增加回复留言信息
        /// </summary>
        /// <returns></returns>
        public ActionResult RepMess()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！" };
            try
            {
                int taskId = FytRequest.GetFormInt("taskId", 0),
                userId = FytRequest.GetFormInt("userId", 0),
                sortId = FytRequest.GetFormInt("sortId", 0);
                string userName = FytRequest.GetFormStringEncode("userName"),
                    contents = FytRequest.GetFormStringEncode("contents");
                var adminModel = GetAdminInfo();
                var model = new tb_MessageRep()
                {
                    MessId = taskId,
                    UserId = adminModel.ID,
                    UserName = adminModel.RealName,
                    Content = contents,
                    ParentId = sortId,
                    AddDate = DateTime.Now
                };
                jsonM.Data = OperateContext<tb_MessageRep>.SetServer.Add(model);
                jsonM.DataA = model.AddDate;
            }
            catch (Exception ex)
            {
                jsonM.Status = "n";
                jsonM.Msg = "保存失败！消息：" + ex.Message;
            }
            return MyJson(jsonM, "yyyy-MM-dd");
        }

    }
}
