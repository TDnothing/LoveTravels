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
    /// 站点设置
    /// </summary>
    public class SiteSetController:BaseController
    {
        /// <summary>
        /// 站点默认管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            //查询所有站点
            ViewBag.list = OperateContext<tb_Site>.SetServer.GetList(s=>true, s=>s.ID, false).ToList();
            ViewBag.SiteId = SiteID;
            return View();
        }

        /// <summary>
        /// 站点编辑
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modfiy(int id)
        {
            //根据ID查询站点
            var model = OperateContext<tb_Site>.SetServer.GetModel(u=>u.ID==id);
            return View(model);
        }

        /// <summary>
        /// 修改或添加站点信息
        /// </summary>
        /// <param name="model">站点模型</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modfiy(tb_Site model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "" };
            try
            {
                //添加或修改
                OperateContext<tb_Site>.SetServer.SaveOrUpdate(model,model.ID);
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息："+ex.Message;
                LogHelper.WriteLog(typeof(SiteSetController),ex);
            }
            return Json(jsonM);
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "删除成功！", BackUrl = "" };
            try
            {
                var id = FytRequest.GetFormInt("id");
                if (id == SiteID)
                {
                    jsonM.Status = "err";
                    jsonM.Msg = "当前站点不能删除！";
                }
                else
                {
                    OperateContext<tb_Site>.SetServer.DeleteBy(u => u.ID == id);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SiteSetController), ex);
            }
            return Json(jsonM);
        }
    }
}
