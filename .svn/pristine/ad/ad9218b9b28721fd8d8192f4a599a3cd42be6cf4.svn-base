using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Logic.Admin
{
    public class GoodsBrandController : BaseController
    {
        #region 1、[商品品牌管理] 初始化+Index()
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

        #region  2、[商品品牌管理] 获取数据+IndexData()
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
                var where = PredicateBuilder.True<tb_GoodsBrand>();
                if (key != "")
                {
                    where = where.And(m => m.BrandName.Contains(key));
                    where = where.Or(m => m.BrandAlias.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_GoodsBrand>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.AddDate)
                    .Select(m => new
                    {
                        m.ID,
                        m.BrandName,
                        m.BrandAlias,
                        m.BrandWebSite,
                        m.Description,
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
                LogHelper.WriteLog(typeof(GoodsBrandController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[商品品牌管理] 修改模板加载视图+Modify(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modify(int id)
        {
            var model = OperateContext<tb_GoodsBrand>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_GoodsBrand()
                {
                    BrandName = "",
                    BrandAlias = "",
                    BrandWebSite = "Http://"
                };
            }
            return View(model);
        }
        #endregion

        #region 4、[商品品牌管理] 修改模板加载视图+Modify(tb_GoodsBrand model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modify(tb_GoodsBrand model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "" };
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Description == null)
                    {
                        model.Description = "";
                    }
                    if (model.Summary == null)
                    {
                        model.Summary = "";
                    }
                    if (model.BrandLogo == null)
                    {
                        model.BrandLogo = "";
                    }
                    if (model.BrandWebSite == null)
                    {
                        model.BrandWebSite = "";
                    }
                    //添加或修改
                    if (model.ID == 0)
                    {
                        model.AddDate = DateTime.Now;
                        OperateContext<tb_GoodsBrand>.SetServer.Add(model);
                    }
                    else
                    {
                        //执行修改操作
                        OperateContext<tb_GoodsBrand>.SetServer.Update(model);
                    }
                    jsonM.BackUrl = "/FytAdmin/GoodsBrand/Index";
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(GoodsController), ex);
            }
            return Json(jsonM);
        }
        #endregion
    }
}
