using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Web.Controllers
{
    /// <summary>
    /// 用户登录和注册，以及找回密码操作
    /// </summary>
    public class UsersController : Controller
    {
        /// <summary>
        /// 详细
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            var model = OperateContext<tb_User>.SetServer.GetModel(m => m.ID == id);
            return View(model);
        }


        #region 登录 Get  Post
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            if (UtilsHelper.GetCookie("FytUserId") != "")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.adv = GetAdvList();
            return View();
        }
        //log in English
        public ActionResult LoginEng()
        {
            if (UtilsHelper.GetCookie("FytUserId") != "")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.adv = GetAdvList();
            return View();
        }
        /// <summary>
        /// 登录实现
        /// </summary>
        /// <param name="models">参数</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tb_User models)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "注册成功！", BackUrl = "/" };
            try
            {
                var model = OperateContext<tb_User>.SetServer.GetModel(m => m.Email.Equals(models.LoginName));
                if (model != null)
                {
                    if (model.LoginPwd.Equals(UtilsHelper.MD5(models.LoginPwd, true)))
                    {
                        if (!model.Status)
                        {
                            jsonM.Status = "err";
                            jsonM.Msg = "您的账号被冻结，请联系客服人员！";
                        }
                        else
                        {
                            if (HttpContext.Session != null) HttpContext.Session["FytUser"] = model;
                            UtilsHelper.WriteCookie("FytUserId",
                                DESEncrypt.Encrypt(model.ID.ToString(CultureInfo.InvariantCulture)));
                        }
                    }
                    else
                    {
                        jsonM.Status = "err";
                        jsonM.Msg = "密码输入错误！";
                    }
                }
                else
                {
                    jsonM.Status = "err";
                    jsonM.Msg = "邮箱输入错误！";
                }

            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(UsersController), ex);
            }
            return Json(jsonM);
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult IsCode()
        {
            var p = Request["param"];
            if (Session[KeyHelper.SESSION_CODE] != null && Session[KeyHelper.SESSION_CODE].ToString().ToUpper() != p.Trim().ToUpper())
            {
                return Json(new { info = "验证码输入错误！", status = "n" });
            }
            return Json(new { info = "", status = "y" });
        }

        #endregion

        #region 注册 Get  Post
        /// <summary>
        /// 注册页面初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Reg()
        {
            if (UtilsHelper.GetCookie("FytUserId") != "")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.adv = GetAdvList();
            return View();
        }
        [HttpGet]
        public ActionResult RegEng()
        {
            if (UtilsHelper.GetCookie("FytUserId") != "")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.adv = GetAdvList();
            return View();
        }
        /// <summary>
        /// 注册提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reg(tb_User model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "注册成功！", BackUrl = "/" };
            try
            {
                model.GroupId = 1;
                model.LoginName = UtilsHelper.Number(10, false);
                model.NickName = "匿名" + UtilsHelper.Number(6, false);
                model.LoginPwd = UtilsHelper.MD5(model.LoginPwd, true);
                model.HeadPic = "/lib/img/no-face.jpg";
                model.Status = true;
                model.Types = 0;
                model.Amount = 0;
                model.Point = 0;
                model.Exp = 0;
                model.RegIp = UtilsHelper.GetIP();
                model.RegDate = DateTime.Now;
                OperateContext<tb_User>.SetServer.Add(model);
                var appModel = new tb_UserApp() { UserId = model.ID, IsPush = true };
                OperateContext<tb_UserApp>.SetServer.Add(appModel);
                //增加第三方登录平台
                var oauthModel = new tb_UserLoginOauth() { UserId = model.ID, OauthId = 1 };
                OperateContext<tb_UserLoginOauth>.SetServer.Add(oauthModel);

                if (HttpContext.Session != null) HttpContext.Session["FytUser"] = model;
                UtilsHelper.WriteCookie("FytUserId", DESEncrypt.Encrypt(model.ID.ToString(CultureInfo.InvariantCulture)));

            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(UsersController), ex);
            }
            return Json(jsonM);
        }

        /// <summary>
        /// 邮箱验证
        /// </summary>
        /// <returns></returns>
        public ActionResult IsEmExist()
        {
            var p = Request["param"];
            var model = OperateContext<tb_User>.SetServer.GetModel(m => m.Email.Equals(p));
            return model == null ? Json(new { info = "", status = "y" }) : Json(new { info = "您输入的邮箱以存在，请更换！", status = "n" });
        }
        #endregion

        #region 找回密码/重置密码 Get  Post
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FindPwd()
        {
            return View();
        }

        /// <summary>
        /// 判断邮箱是否存在，如果存在发送邮件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FindPwd(tb_User model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "注册成功！", BackUrl = "/" };
            try
            {
                var isModel = OperateContext<tb_User>.SetServer.GetModel(m => m.Email.Equals(model.Email));
                if (isModel != null)
                {
                    //查询站点基本信息
                    var siteModel = WebSiteHelper.GetSite(1);
                    //查询系统配置
                    var sysModel = SysBasicHelper.GetCacheSysBasic();
                    //构建发送邮件地址
                    var linkUrl = siteModel.SiteUrl + "/users/resetpwd?session=" + DESEncrypt.Encrypt(isModel.ID.ToString(CultureInfo.InvariantCulture)) +
                        "&et=" + DESEncrypt.Encrypt(DateTime.Now.AddDays(3).ToString(CultureInfo.InvariantCulture));
                    //发送邮件  查询邮件模板
                    var emodel = OperateContext<tb_UserEmail>.SetServer.GetModel(m => m.ID == 1);
                    emodel.Content = emodel.Content.Replace("{logo}", siteModel.SiteUrl + siteModel.SiteLogo).Replace("{username}", model.Email)
                        .Replace("{webname}", siteModel.SiteName).Replace("{Links}", linkUrl).Replace("{重设密码}", "<a href=\"" + linkUrl + "\" target=\"_blank\">重设密码</a>");

                    //开始发送
                    EmailHelper.SendMail(sysModel.emailsmtp, sysModel.emailusername, sysModel.emailpassword, sysModel.emailnickname,
                        sysModel.emailusername, model.Email, emodel.Title, emodel.Content);
                }
                else
                {
                    jsonM.Status = "err";
                    jsonM.Msg = "您输入的邮箱，没有注册成为会员！";
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(UsersController), ex);
            }
            return Json(jsonM);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResetPwd()
        {
            try
            {
                string session = FytRequest.GetQueryString("session"),
                endTime = FytRequest.GetQueryString("et");
                if (session != "" && endTime != "")
                {
                    var et = DESEncrypt.Decrypt(endTime);
                    if (Convert.ToDateTime(et) < DateTime.Now)
                    {
                        return Content("<script>alert('链接已过期，请重新发起请求！');window.location.href='/';</script>");
                    }
                }
                else
                {
                    return Content("<script>alert('参数验证失败，请重新发起请求！');window.location.href='/';</script>");
                }
            }
            catch (Exception ex)
            {
                return Content("<script>alert('内部发生错误，请联系客服人员！');window.location.href='/';</script>");
            }
            return View();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPwd(string pwd)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "密码修改成功！", BackUrl = "/users/login" };
            try
            {
                var uid = FytRequest.GetFormString("session");
                if (uid != "")
                {
                    var userId = Convert.ToInt32(DESEncrypt.Decrypt(uid));
                    var model = OperateContext<tb_User>.SetServer.GetModel(m => m.ID == userId);
                    if (model != null)
                    {
                        model.LoginPwd = UtilsHelper.MD5(FytRequest.GetFormString("LoginPwd"), true);
                        OperateContext<tb_User>.SetServer.Update(model);
                    }
                    else
                    {
                        jsonM.Status = "err";
                        jsonM.Msg = "数据模型出错！";
                    }
                }
                else
                {
                    jsonM.Status = "err";
                    jsonM.Msg = "参数为空！";
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(UsersController), ex);
            }
            return Json(jsonM);
        }

        #endregion

        #region 分布视图，底部信息
        public ActionResult Footer()
        {
            return View();
        }
        #endregion

        #region 登录/注册广告
        /// <summary>
        /// 获得广告
        /// </summary>
        /// <returns></returns>
        private tb_AdvList GetAdvList()
        {
            var model =
                OperateContext<tb_AdvList>.SetServer.GetList(m => m.ClassId == 4 && m.Status, m => m.Sort, false)
                    .FirstOrDefault();
            return model ?? new tb_AdvList();
        }

        #endregion

        #region 3.0 登录页面使用验证码+GetVaildateCode()
        /// <summary>
        /// 使用验证吗
        /// </summary>
        /// <returns></returns>
        public ActionResult GetVaildateCode()
        {
            string code;
            byte[] bytes = ValidateHelper.CreateValidateCode(4, out code);
            Session[KeyHelper.SESSION_CODE] = code;
            return File(bytes, @"image/Png");
        }
        #endregion
    }
}