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
    /// 会员财务日志
    /// </summary>
    public class MoneyLogController:BaseController
    {
        #region 1、[会员财务日志管理]  初始化+Index()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 2、[会员财务日志管理] 获取数据+IndexData()
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
                var status = FytRequest.GetFormInt("selStatus"); //归属栏目
                var userId = FytRequest.GetFormInt("userId", 0);
                var where = PredicateBuilder.True<tb_UserMoneyLog>();
                if (key != "")
                {
                    where = where.And(m => m.tb_User.LoginName.Contains(key));
                }
                if (status != 0)
                {
                    where = where.And(m => m.Status == status);
                }
                if (userId != 0)
                {
                    where = where.And(m => m.UserId == userId);
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_UserMoneyLog>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.ID).ToList()
                    .Select(m => new
                    {
                        m.ID,
                        m.Number,
                        m.Option,
                        m.Price,
                        m.NowPrice,
                        m.RealPrice,
                        m.Status,
                        m.PayType,
                        m.AddDate,
                        m.Summary,
                        m.tb_User.LoginName
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MoneyLogController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[会员财务日志管理] 修改模板加载视图+Modfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modfiy(int id)
        {
            var model = OperateContext<tb_UserMoneyLog>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                var classId = FytRequest.GetQueryInt("ClassId", 0);
                model = new tb_UserMoneyLog() { AddDate = DateTime.Now };
            }
            return View(model);
        }
        #endregion

        #region 4、[会员财务日志管理] 修改模板加载视图+Modfiy(tb_MoneyLog model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modfiy(tb_UserMoneyLog model)
        {
            var userId = FytRequest.GetFormInt("userId", 0);
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "/FytAdmin/MoneyLog/Index" };
            try
            {
                if (ModelState.IsValid)
                {
                    //添加或修改
                    OperateContext<tb_UserMoneyLog>.SetServer.SaveOrUpdate(model, model.ID);
                }
                jsonM.BackUrl = "/FytAdmin/MoneyLog/Index?userId=" + userId;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MoneyLogController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[会员财务日志管理] 删除记录+DeleteBy()
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
                    OperateContext<tb_UserMoneyLog>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_UserMoneyLog>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MoneyLogController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion
    }
}
