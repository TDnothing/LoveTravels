using System.Data.Entity.Validation;
using System.Globalization;
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
    public class RegionController : BaseController
    {
        #region 1、[地域管理] 初始化+Index()
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region 2、[地域管理] 加载地域信息树+GetRegionTree()
        /// <summary>
        /// 加载左侧tree
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRegionTree()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var regionList = OperateContext<tb_City>.SetServer.GetList(PredicateBuilder.True<tb_City>(), m => m.ID, true).ToList()
                .Select(m => new
                {
                    id = m.ID,
                    pId = m.ParentId,
                    name = m.ID == 0 ? "地域列表" : m.RegionName,
                    open = m.ID == 0 ? true : false,
                    target = "DeployBase",
                    url = m.ID == 0 ? "" : "Modify/" + m.ID.ToString(CultureInfo.InvariantCulture)
                });
                jsonM.Data = regionList;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "加载地区树发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ContentController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 3、[地域管理] 修改模板加载视图+Modify(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modify(int id)
        {
            //获取父级名称列表
            var regionSelect =
                OperateContext<tb_City>.SetServer.GetList(PredicateBuilder.True<tb_City>(),
                    m => m.ID, true).Select(m => new
                    {
                        m.ID,
                        m.RegionName
                    }).ToList();
            var regionList = regionSelect.Select(p => new SelectListItem
            {
                Text = p.RegionName,
                Value = p.ID.ToString(CultureInfo.InvariantCulture)
            }).ToList();
            ViewBag.regionList = regionList;
            if (id == 0)
            {
                id = -1;
            }
            var model = OperateContext<tb_City>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_City()
                {
                    ID = -1,
                    RegionName = "",
                    ParentId = 1,
                    FirstLetter = ""
                };
            }
            return View(model);
        }
        #endregion

        #region 4、[地域管理] 修改模板加载视图+Modify(tb_Region model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modify(tb_City model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "" };
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.FirstLetter == null)
                    {
                        model.FirstLetter = "";
                    }
                    //添加或修改
                    if (model.ID == -1)
                    {
                        OperateContext<tb_City>.SetServer.Add(model);
                    }
                    else
                    {
                        //执行修改操作
                        OperateContext<tb_City>.SetServer.Update(model);
                    }
                    jsonM.BackUrl = "/FytAdmin/Region/Modify?id=-1";
                }
            }
            catch (DbEntityValidationException ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(GoodsController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[地域管理] 删除地域记录+DeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var id = FytRequest.GetFormInt("id");
                if (id != 0)
                {
                    //--查询该地域是否有二级地域
                    var list = OperateContext<tb_City>.SetServer.GetList(m => m.ParentId == id, m => m.ID, true);
                    if (list != null)
                    {
                        var temp = "";
                        foreach (var item in list)
                        {
                            temp += item.ID + ",";
                        }
                        List<int> regionlist = new List<string>(temp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                        //----查询是否有三级地域
                        var slist = OperateContext<tb_City>.SetServer.GetList(m => regionlist.Contains(m.ID), m => m.ID, true);
                        if (slist != null)
                        {
                            var stemp = "";
                            foreach (var sitem in slist)
                            {
                                stemp += sitem.ID + ",";
                            }
                            //----删除三级地域
                            List<int> sregionlist = new List<string>(stemp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                            OperateContext<tb_City>.SetServer.DeleteBy(m => sregionlist.Contains(m.ID));
                        }
                        //----删除二级地域
                        OperateContext<tb_City>.SetServer.DeleteBy(m => regionlist.Contains(m.ID));
                    }
                    //----删除一级地域
                    OperateContext<tb_City>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    jsonM.Status = "n";
                    jsonM.Msg = "地域信息ID为0";
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(RegionController), ex);
            }
            return MyJson(jsonM, "");
        }

        #endregion

    }
}
