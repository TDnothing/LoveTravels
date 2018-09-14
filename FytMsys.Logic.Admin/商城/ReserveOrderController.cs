using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
    /// <summary>
    /// 预定商品管理
    /// </summary>
    public class ReserveOrderController : BaseController
    {
        #region 1、[预订商品管理] 初始化+Index()
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region  2、[预订商品管理] 获取数据+IndexData()
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IndexData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询
                var where = PredicateBuilder.True<tb_GoodsMess>();
                if (key != "")
                {
                    where = where.And(m => m.GoodsInfo.Contains(key));
                    where = where.Or(m => m.Mobile.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_GoodsMess>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.ID)
                    .Select(m => new
                    {
                        m.ID,
                        m.GoodsInfo,
                        m.GoodsNum,
                        m.MemberId,
                        m.MemberName,
                        m.UserName,
                        m.Mobile,
                        m.Tel,
                        m.SendAddress,
                        m.Requirement,
                        m.AddDate
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ReserveOrderController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[预订商品管理] 修改模板加载视图+Modify(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modify(int id)
        {
            //获取用户列表
            var model = OperateContext<tb_GoodsMess>.SetServer.GetModel(m => m.ID == id);
            var member = OperateContext<tb_User>.SetServer.GetModel(m => m.ID == model.MemberId);
            ViewBag.member = member;
            return View(model);
        }
        #endregion

        #region 4、[预订商品管理] 修改模板加载视图+Modify(tb_ReserveOrder model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modify(tb_GoodsMess model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "" };
            try
            {
                if (ModelState.IsValid)
                {
                    var umodel = OperateContext<tb_User>.SetServer.GetModel(m => m.ID == model.MemberId);
                    model.MemberName = umodel.TrueName;
                    if (model.Requirement == null)
                    {
                        model.Requirement = "";
                    }
                    if (model.Tel == null)
                    {
                        model.Tel = "";
                    }
                    if (model.Summary == null)
                    {
                        model.Summary = "";
                    }
                    //添加或修改
                    if (model.ID == 0)
                    {
                        model.AddDate = DateTime.Now;
                        OperateContext<tb_GoodsMess>.SetServer.Add(model);
                    }
                    else
                    {
                        //执行修改操作
                        OperateContext<tb_GoodsMess>.SetServer.Update(model);
                    }
                    jsonM.BackUrl = "/FytAdmin/ReserveOrder/Index";
                }
            }
            catch (DbEntityValidationException ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ReserveOrderController), ex);
            }
            return Json(jsonM);
        }
        #endregion
    }
}
