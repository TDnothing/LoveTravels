using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using FytMsys.Common;
using FytMsys.Vector.WeixinLogin;

namespace FytMsys.Web.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        public ActionResult Index()
        {
            //获取appId,appSecret的配置信息
            string appId = "wx23cd91df711b7795";
            string appSecret = "6eb29b228e14744857b3af2407c5ea3d";

            var weixinOAuth = new WeiXinOAuth();
            //微信第一次握手后得到的code 和state
            string _code = FytRequest.GetQueryString("code");
            string _state = FytRequest.GetQueryString("state");
            if (_code == "" || _code == "authdeny")
            {
                if (_code == "")
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
                var modelResult = weixinOAuth.GetWeiXinAccessToken(appId, appSecret, _code);
                //获取微信的用户信息(第三次微信握手)
                var userInfo = weixinOAuth.GetWeiXinUserInfo(modelResult.SuccessResult.access_token, modelResult.SuccessResult.openid);
                //用户信息（判断是否已经获取到用户的微信用户信息）
                if (userInfo.Result && userInfo.UserInfo.openid != "")
                {
                    //保存获取到的用户微信用户信息，并保存到数据库中
                    return Content(JsonHelper.JsonSerializer(userInfo));
                }
                else
                {
                    return Content("出问题了");
                    //GameTradingByPublic.ExceptionLog.writeFile(2, "获取用户OpenId失败");
                }
            }


            return View();
        }


        public ActionResult MoneyTest()
        {
            var ulr = "http://api.k780.com:88/?app=finance.rate&scur=USD&tcur=CNY&appkey=10003&sign=b59bc3ef6191eb9f747dd4e83c99f2a4";
            var s = UtilsHelper.GetHttp(ulr);
            var ss = JsonConverter.ConvertJson(s);
            return Content(ss.result.rate);
        }
	}
}