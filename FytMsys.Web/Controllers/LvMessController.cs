using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Web.Controllers
{
    public class LvMessController : Controller
    {
        /// <summary>
        /// 消息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int oid,int nid)
        {
            ViewBag.oid = oid;
            ViewBag.nid = nid;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(lv_Message model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "发送成功" };
            try
            {
                model.AddTime = DateTime.Now;
                model.IsRead = false;
                OperateContext<lv_Message>.SetServer.Add(model);

                int touserid = model.GoUserId; //go is touser
                int fromuserid = model.SendUserId;
                var ToUser = OperateContext<tb_User>.SetServer.GetModel(m => m.ID == touserid);
                var FromUser = OperateContext<tb_User>.SetServer.GetModel(m => m.ID == fromuserid);
               
                //查询站点基本信息
                var siteModel = WebSiteHelper.GetSite(1);
                //查询系统配置
                var sysModel = SysBasicHelper.GetCacheSysBasic();
                var emodel = OperateContext<tb_UserEmail>.SetServer.GetModel(m => m.ID == 2);
                emodel.Content = emodel.Content.Replace("{logo}", siteModel.SiteUrl + siteModel.SiteLogo).Replace("{username}", ToUser.NickName)
                    .Replace("{fromuser}", FromUser.NickName).Replace("{link}", "http://www.51voy.com/account/message?types=receive")
                    .Replace("{message}", model.Centents);

                //开始发送
                EmailHelper.SendMail(sysModel.emailsmtp, sysModel.emailusername, sysModel.emailpassword, sysModel.emailnickname,
                    sysModel.emailusername, ToUser.Email, emodel.Title, emodel.Content);
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：";
                LogHelper.WriteLog(typeof(lv_Message), ex);
            }
            return Json(jsonM);
        }
    }
}