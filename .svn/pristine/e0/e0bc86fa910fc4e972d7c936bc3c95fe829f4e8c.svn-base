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
using System.Data;

namespace FytMsys.Logic.Admin
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderManController : BaseController
    {
        #region 1、[订单管理]  初始化+Index()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            var orderStatus = FytRequest.GetQueryInt("orderStatus");
            TempData["orderStatus"] = orderStatus;
            return View();
        }
        #endregion

        #region 2、[订单管理] 获取数据+IndexData()
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
                var orderStatus = FytRequest.GetFormInt("orderStatus", -1);//1=未发货，2=已发货，3=已完成
                var where = PredicateBuilder.True<tb_GoodsOrder>();
                if (key != "")
                {
                    where = where.And(m => m.Number.Contains(key));
                    where = where.Or(m => m.Summary.Contains(key));
                    where = where.Or(m => m.UserName.Contains(key));
                }
                switch (orderStatus)
                {
                    case 0:
                        where = where.And(m => m.PayStatus == false);
                        where = where.Or(m => m.Status == 0);
                        break;
                    case 1:
                        where = where.And(m => m.PayStatus == true);
                        where = where.And(m => m.Status == 1);
                        break;
                    case 2:
                        where = where.And(m => m.PayStatus == true);
                        where = where.And(m => m.Status == 2);
                        break;
                    case 3:
                        where = where.And(m => m.PayStatus == true);
                        where = where.And(m => m.Status == 3);
                        break;
                    default:
                        break;
                }
                if (beginTime != "" && endTime != "")
                {
                    var bt = Convert.ToDateTime(beginTime);
                    var et = Convert.ToDateTime(endTime);
                    where = where.And(m => m.AddDate >= bt && m.AddDate <= et);
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_GoodsOrder>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.AddDate)
                    .Select(m => new
                    {
                        m.ID,
                        m.Status,
                        m.Number,
                        m.UserId,
                        m.UserName,
                        m.UserMobile,
                        m.TotalMoney,
                        m.PayType,
                        m.PayStatus,
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
                LogHelper.WriteLog(typeof(OrderManController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[会员商城订单管理] 修改商城订单 + Modify(int id)
        /// <summary>
        /// [会员商城订单管理] 修改预约订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modify(int id)
        {
            //获取用户列表
            var userSelect =
                OperateContext<tb_User>.SetServer.GetList(PredicateBuilder.True<tb_User>().And(m => m.GroupId == 1).And(m => m.Status == true),
                    m => m.RegDate, false).Select(m => new
                    {
                        m.ID,
                        m.TrueName
                    }).ToList();
            var userList = userSelect.Select(p => new SelectListItem
            {
                Text = p.TrueName,
                Value = p.ID.ToString(CultureInfo.InvariantCulture)
            }).ToList();
            ViewBag.userList = userList;
            var model = OperateContext<tb_GoodsOrder>.SetServer.GetModel(m => m.ID == id) ?? new tb_GoodsOrder()
            {
                AddDate = DateTime.Now
            };
            if (model.ID != 0)
            {
                if (model.OrderType == 0)
                {
                    var goodslist = OperateContext<tb_GoodsOrderDetail>.SetServer.GetList(m => m.OrderID == model.ID, m => m.ID, false).ToList();
                    ViewBag.goodslist = goodslist;
                }
            }
            return View(model);
        }

        #endregion

        #region 4、[会员商城订单管理] 修改商城订单 + Modify(tb_GoodsOrder model)
        /// <summary>
        /// [会员商城订单管理] 修改预约订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Modify(tb_GoodsOrder model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                if (ModelState.IsValid)
                {
                    var gmodel = OperateContext<tb_GoodsOrder>.SetServer.GetModel(m => m.ID == model.ID);
                    if (model.PayStatus == false && gmodel.PayStatus == true)
                    {
                        model.Status = 0;
                    }
                    else if (model.PayStatus == true && gmodel.PayStatus == false)
                    {
                        model.Status = 1;
                    }
                    gmodel.PayStatus = model.PayStatus;
                    gmodel.Status = model.Status;
                    //添加或修改
                    OperateContext<tb_GoodsOrder>.SetServer.SaveOrUpdate(gmodel, gmodel.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(OrderManController), ex);
            }
            return Json(jsonM);
        }

        #endregion

        #region 5、[订单管理] 删除记录+DownLoadDeleteBy()
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
                    OperateContext<tb_GoodsOrderDetail>.SetServer.DeleteBy(m => m.OrderID == id);
                    OperateContext<tb_GoodsOrder>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_GoodsOrderDetail>.SetServer.DeleteBy(m => result.Contains(m.OrderID));
                    OperateContext<tb_GoodsOrder>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(OrderManController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 5、[订单管理] 订单统计+OrderCount()
        /// <summary>
        /// 订单统计报表
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderCount()
        {
            var year = FytRequest.GetQueryInt("yearSelect");
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }
            //按年统计
            TYearCount();
            //按月统计
            YearCount(year);
            return View();
        }

        //按年份统计
        void TYearCount()
        {
            StringBuilder strcategory = new StringBuilder();
            StringBuilder strdata = new StringBuilder();
            var dt = AdoHelper.GetTableDataSet("select count(1) as p,YEAR(AddDate) as y from tb_GoodsOrder group by YEAR(AddDate)", CommandType.Text).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    strcategory.Append("'" + item["y"] + "年',");
                    strdata.Append("" + item["p"] + ",");
                }
                strcategory.Remove(strcategory.Length - 1, 1);
                strdata.Remove(strdata.Length - 1, 1);
            }
            else
            {

                strcategory.Append("" + DateTime.Now.Year + "年,");
                strdata.Append("0,");

                strcategory.Remove(strcategory.Length - 1, 1);
                strdata.Remove(strdata.Length - 1, 1);
            }
            ViewBag.category_tyear = strcategory;
            ViewBag.data_tyear = strdata;
        }

        //按月份统计
        void YearCount(int year)
        {
            StringBuilder strcategory = new StringBuilder();
            StringBuilder strdata = new StringBuilder();
            var dt = AdoHelper.GetTableDataSet("select count(1) as p,MONTH(AddDate) as m from tb_GoodsOrder where YEAR(AddDate)=" + year + " group by month(AddDate)", CommandType.Text).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i < 13; i++)
                {
                    int j = 0;
                    foreach (DataRow item in dt.Rows)
                    {
                        if (i == Convert.ToInt32(item["m"]))
                        {
                            j = Convert.ToInt32(item["p"]);
                            break;
                        }
                    }
                    if (j > 0)
                    {
                        strcategory.Append("'" + i + "月',");
                        strdata.Append("" + j + ",");
                    }
                    else
                    {
                        strcategory.Append("'" + i + "月',");
                        strdata.Append("0,");
                    }
                }
                strcategory.Remove(strcategory.Length - 1, 1);
                strdata.Remove(strdata.Length - 1, 1);
            }
            else
            {
                for (int i = 1; i < 13; i++)
                {
                    strcategory.Append("'" + i + "月',");
                    strdata.Append("0,");
                }
                strcategory.Remove(strcategory.Length - 1, 1);
                strdata.Remove(strdata.Length - 1, 1);
            }
            ViewBag.category_year = strcategory;
            ViewBag.data_year = strdata;
        }

        #endregion
    }
}
