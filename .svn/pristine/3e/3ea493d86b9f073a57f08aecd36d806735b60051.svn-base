using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FytMsys.Logic.Admin
{
    /// <summary>
    /// 退换货管理
    /// </summary>
    public class ReturnGoodsOrderController : BaseController
    {
        #region 1、[退换货管理] 初始化+Index()
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

        #region  2、[退换货管理] 获取数据+IndexData()
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
                var where = PredicateBuilder.True<tb_GoodsReturn>();
                if (key != "")
                {
                    where = where.And(m => m.OrderNum.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_GoodsReturn>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.ID)
                    .Select(m => new
                    {
                        m.ID,
                        m.OrderNum,
                        m.UserId,
                        m.UserName,
                        m.UserMobile,
                        m.UserAddress,
                        m.Summary,
                        m.Status,
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

        #region 3、[退换货管理] 修改模板加载视图+Modify(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modify(int id)
        {
            //获取退货信息
            var model = OperateContext<tb_GoodsReturn>.SetServer.GetModel(m => m.ID == id);
            //获取商品信息
            var orderinfo = OperateContext<tb_GoodsOrder>.SetServer.GetModel(m => m.Number == model.OrderNum);
            var goodsInfo = OperateContext<tb_GoodsOrderDetail>.SetServer.GetList(PredicateBuilder.True<tb_GoodsOrderDetail>().And(m => m.OrderID == orderinfo.ID).And(m => m.ID == model.GoodsId), m => m.ID, true).ToList();
            ViewBag.goodsInfo = goodsInfo;
            //获取会员信息
            var member = OperateContext<tb_User>.SetServer.GetModel(m => m.ID == model.UserId);
            ViewBag.member = member;
            return View(model);
        }
        #endregion

        #region 4、[退换货管理] 修改模板加载视图+Modify(tb_ReserveOrder model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modify(tb_GoodsReturn model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                if (ModelState.IsValid)
                {
                    var umodel = OperateContext<tb_GoodsReturn>.SetServer.GetModel(m => m.ID == model.ID);
                    umodel.Status = model.Status;
                    if (!string.IsNullOrEmpty(model.AuditSummary))
                    {
                        umodel.AuditSummary = model.AuditSummary;
                        umodel.AuditDate = DateTime.Now;
                    }
                    //执行修改操作
                    OperateContext<tb_GoodsReturn>.SetServer.Update(umodel);
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
