using FytMsys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FytMsys.Helper
{
    public class AuthenFilterAttribute : ActionFilterAttribute
    {
        public string Message { get; set; }
        //FilterContextInfo _fcinfo;
        // OnActionExecuted 在执行操作方法后由 ASP.NET MVC 框架调用。
        // OnActionExecuting 在执行操作方法之前由 ASP.NET MVC 框架调用。
        // OnResultExecuted 在执行操作结果后由 ASP.NET MVC 框架调用。
        // OnResultExecuting 在执行操作结果之前由 ASP.NET MVC 框架调用。
        /// <summary>
        /// 在执行操作方法之前由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var nd = DateTime.Now.ToString("yyyyMMddHHmm");
            var ndp = (Int64.Parse(nd) - 1).ToString();
            var ndn = (Int64.Parse(nd) + 1).ToString();

            var ecode = "feiyit";

            var bases = (HttpRequestBase)filterContext.HttpContext.Request;
            var uid = bases.Params.Get("uid") == null ? "0" : bases.Params.Get("uid");
            var k = bases.Params.Get("key") == null ? "" : bases.Params.Get("key");

            var code = UtilsHelper.MD5(nd + uid + ecode, false);
            var codep = UtilsHelper.MD5(ndp + uid + ecode, false);
            var coden = UtilsHelper.MD5(ndn + uid + ecode, false);

            var rcode = UtilsHelper.MD5(code.Substring(0, 16), false);
            var rcodep = UtilsHelper.MD5(codep.Substring(0, 16), false);
            var rcoden = UtilsHelper.MD5(coden.Substring(0, 16), false);

            if (k == "" || k.Equals(rcode) || !k.Equals(rcodep) || !k.Equals(rcoden))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { Controller = "Error", action = "Index" }));
            }
        }

        /// <summary>
        /// 在执行操作方法后由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        ///  OnResultExecuted 在执行操作结果后由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }
        /// <summary>
        /// OnResultExecuting 在执行操作结果之前由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
        public class FilterContextInfo
        {
            public FilterContextInfo(ActionExecutingContext filterContext)
            {
                #region 获取链接中的字符
                // 获取域名
                if (filterContext.HttpContext.Request.Url != null)
                    DomainName = filterContext.HttpContext.Request.Url.Authority;

                //获取模块名称
                //  module = filterContext.HttpContext.Request.Url.Segments[1].Replace('/', ' ').Trim();

                //获取 controllerName 名称
                ControllerName = filterContext.RouteData.Values["controller"].ToString();

                //获取ACTION 名称
                ActionName = filterContext.RouteData.Values["action"].ToString();

                #endregion
            }

            /// <summary>
            /// 获取域名
            /// </summary>
            public string DomainName { get; set; }

            /// <summary>
            /// 获取模块名称
            /// </summary>
            public string Module { get; set; }
            /// <summary>
            /// 获取 controllerName 名称
            /// </summary>
            public string ControllerName { get; set; }
            /// <summary>
            /// 获取ACTION 名称
            /// </summary>
            public string ActionName { get; set; }

        }
    }
}
