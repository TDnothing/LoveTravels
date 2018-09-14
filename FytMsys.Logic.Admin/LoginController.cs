using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Logic.Admin
{
    public class LoginController:BaseController
    {
        #region 1.0 管理员登陆页面 +ActionResult Login()
        /// <summary>
        /// 管理员登陆页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 2.0 管理员登录 +Index(FormCollection form)
        /// <summary>
        /// 管理员登录 +Index(FormCollection form)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            //1、获取用户输入的数据，并进行过滤
            string loginName = FytRequest.GetFormString("loginName", true),
                loginPwd = FytRequest.GetFormString("loginPwd"),
                code = FytRequest.GetFormString("loginCode");
            //2、组装json返回客户端
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "err", Msg = "登录失败，请检查登录账号或密码！" };
            //3、验证验证码输入的是否正确
            if (Session[KeyHelper.SESSION_CODE] != null && Session[KeyHelper.SESSION_CODE].ToString().ToUpper() != code.Trim().ToUpper())
            {
                jsonM.Status = "err-code";
                jsonM.Msg = "验证码输入错误！";
                return Json(jsonM);
            }
            try
            {
                //4、通过业务接口，获得登录对象
                var user = OperateContext<tb_Admin>.SetServer.GetModel(u => u.LoginName == loginName);
                if (user!=null)
                {
                    //判断登录用户角色是否有效
                    if (!user.tb_AdminRole.IsLock)
                    {
                        jsonM.Status = "n";
                        jsonM.Msg = "登录失败，该用户角色授权失效，请联系客服！";
                        return Json(jsonM);
                    }
                    if (user.LoginPwd.Equals(UtilsHelper.MD5(loginPwd,true)))
                    {
                        jsonM.Status = "y";
                        jsonM.Msg = "登录成功，即将进入管理主界面。";
                        jsonM.BackUrl = "/fytadmin/index/";
                        //5、将用户登录账号保存到Cookie中
                        UtilsHelper.WriteCookie("FYTSYS", "FYTGUID", new AESEncrypt().Encrypt(user.ID.ToString(CultureInfo.InvariantCulture)));
                        UtilsHelper.WriteCookie("FYTSYS", KeyHelper.CACHE_SITE_LANGUAGE, "1");
                        //7、保存登录日志
                        SaveLogs("【登录操作】登录后台管理系统", 1);
                        //8、修改登录时间
                        user.LastLoginTime = DateTime.Now;
                        user.LastLoginIP = UtilsHelper.GetIP();
                        OperateContext<tb_Admin>.SetServer.Update(user);
                    }
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "登录失败，请检查数据库链接！";
                LogHelper.WriteLog(typeof(LoginController), ex);
            }
            return Json(jsonM);
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
