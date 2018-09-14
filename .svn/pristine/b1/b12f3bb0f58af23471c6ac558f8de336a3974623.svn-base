using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;
using FytMsys.Vector.WeixinLogin;

namespace FytMsys.Web.Controllers
{
    public class WeiXinOAuthController : Controller
    {
        /// <summary>
        /// 微信授权登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //获取appId,appSecret的配置信息
            string appId = "wx23cd91df711b7795";
            string appSecret = "6eb29b228e14744857b3af2407c5ea3d";

            var weixinOAuth = new WeiXinOAuth();
            //微信第一次握手后得到的code 和state
            string code = FytRequest.GetQueryString("code");
            string state = FytRequest.GetQueryString("state");
            if (code == "" || code == "authdeny")
            {
                if (code == "")
                {
                    //发起授权(第一次微信握手)
                    string authUrl = weixinOAuth.GetWeiXinCode(appId, appSecret, Server.UrlEncode(Request.Url.ToString()));
                    Response.Redirect(authUrl, true);
                }
                else
                {
                    // 用户取消授权
                    Response.Redirect("~/Error.html", true);
                }
            }
            else
            {
                //获取微信的Access_Token（第二次微信握手）
                var modelResult = weixinOAuth.GetWeiXinAccessToken(appId, appSecret, code);
                //获取微信的用户信息(第三次微信握手)
                var userInfo = weixinOAuth.GetWeiXinUserInfo(modelResult.SuccessResult.access_token, modelResult.SuccessResult.openid);
                //用户信息（判断是否已经获取到用户的微信用户信息）
                if (userInfo.Result && userInfo.UserInfo.openid != "")
                {
                    //根据OpenId判断数据库是否存在，如果存在，直接登录即可
                    var oauthUser =
                        OperateContext<tb_UserLoginOauth>.SetServer.GetModel(m => m.OpenId == userInfo.UserInfo.openid);
                    if (oauthUser != null)
                    {
                        //直接登录即可  根据授权ID，查询用户信息
                        if (HttpContext.Session != null) HttpContext.Session["FytUser"] = oauthUser.tb_User;
                        UtilsHelper.WriteCookie("FytUserId",
                            DESEncrypt.Encrypt(oauthUser.tb_User.ID.ToString(CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        //注册操作
                        OauthReg(userInfo);
                    }
                    //授权成功后，直接返回到首页
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    return Content("授权失败，请返回重新操作！");
                    //GameTradingByPublic.ExceptionLog.writeFile(2, "获取用户OpenId失败");
                }
            }
            return View();
        }

        /// <summary>
        /// 注册机制
        /// </summary>
        /// <param name="userInfo">微信授权成功返回的用户信息</param>
        private void OauthReg(WeiXinUserInfoResult userInfo)
        {
            var model=new tb_User
            {
                GroupId = 1,
                LoginName = UtilsHelper.Number(10, false),
                NickName = userInfo.UserInfo.nickname,
                LoginPwd = UtilsHelper.MD5("123456", true),
                HeadPic = userInfo.UserInfo.headimgurl,
                TrueName = userInfo.UserInfo.nickname,
                Birthday = "",
                Sex = userInfo.UserInfo.sex=="1" ? "男" : "女",
                IsMarried = "",
                Email = "",
                Mobile = "",
                Status = true,
                Types = 0,
                Amount = 0,
                Point = 0,
                Exp = 0,
                Province = userInfo.UserInfo.province,
                City = userInfo.UserInfo.city,
                RegIp = UtilsHelper.GetIP(),
                RegDate = DateTime.Now
            };
            OperateContext<tb_User>.SetServer.Add(model);
            var appModel = new tb_UserApp() { UserId = model.ID, IsPush = true };
            OperateContext<tb_UserApp>.SetServer.Add(appModel);
            //增加第三方登录平台
            var oauthModel = new tb_UserLoginOauth() { UserId = model.ID, OauthId = 1, OpenId = userInfo.UserInfo.openid, OauthKey = userInfo.UserInfo.openid };
            OperateContext<tb_UserLoginOauth>.SetServer.Add(oauthModel);

            if (HttpContext.Session != null) HttpContext.Session["FytUser"] = model;
            UtilsHelper.WriteCookie("FytUserId", DESEncrypt.Encrypt(model.ID.ToString(CultureInfo.InvariantCulture)));
        }

    }
}