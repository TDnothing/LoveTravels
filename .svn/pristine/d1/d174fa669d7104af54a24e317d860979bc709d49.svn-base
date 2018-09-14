using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;


namespace FytMsys.Logic.Admin
{
    public class BaseController:Controller
    {
        protected int SiteID = 0;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //当action里面不含有login并且登录不为空时的操作
            string v = filterContext.RouteData.Values["controller"].ToString();
            if (!v.Contains("login"))
            {
                if (!IsAdminLogin())
                {
                    filterContext.Result = new RedirectResult("/fytadmin/login/");
                    return;
                }
            }
            //赋值站点ID 
            try
            {
                var s = UtilsHelper.GetCookie("FYTSYS", KeyHelper.CACHE_SITE_LANGUAGE);
                if (s != "")
                {
                    SiteID = Convert.ToInt32(s);
                }
            }
            catch
            {
            }
        }

        #region 1、判断管理员是否已经登录(解决Session超时问题)  +IsAdminLogin()
        /// <summary>
        /// 判断管理员是否已经登录(解决Session超时问题)
        /// </summary>
        public bool IsAdminLogin()
        {
            //如果Session为Null
            if (Session[KeyHelper.SESSION_ADMIN_INFO] != null)
            {
                return true;
            }
            else
            {
                //检查Cookies
                string userId = UtilsHelper.GetCookie("FYTSYS", "FYTGUID"); //解密id
                if (userId == "") return false;
                userId = new AESEncrypt().Decrypt(userId); //解密用户名
                int uId = int.Parse(userId);
                var uc = OperateContext<tb_Admin>.SetServer.GetModel(u=>u.ID==uId);
                if (uc == null) return false;
                Session[KeyHelper.SESSION_ADMIN_INFO] = uc;
                return true;
            }
        } 
        #endregion

        #region 2、获取管理员信息 +GetAdminInfo
        /// <summary>
        /// 获取管理员信息 
        /// </summary>
        /// <returns>tb_Admin</returns>
        public tb_Admin GetAdminInfo()
        {
            if (!IsAdminLogin()) return null;
            var uc = Session[KeyHelper.SESSION_ADMIN_INFO] as tb_Admin;
            return uc ?? null;
        }
        
        #endregion

        #region 3、保存系统操作日志 +SaveLogs(string title, int logtype)
        /// <summary>
        /// 保存系统操作日志
        /// </summary>
        /// <param name="title">操作标题</param>
        /// <param name="logtype">操作类型 1=登录  0=操作</param>
        public void SaveLogs(string title, int logtype)
        {
            try
            {
                var uc = GetAdminInfo();
                if (uc == null) return;
                var model = new tb_SystemLog()
                {
                    loginName = uc.LoginName,
                    title = title,
                    IP = UtilsHelper.GetIP(),
                    logType = logtype,
                    addDate = DateTime.Now
                };
                OperateContext<tb_SystemLog>.SetServer.Add(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(BaseController),ex);
            }
        } 
        #endregion


        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new ToJsonResult
            {
                Data = data,
                ContentEncoding = contentEncoding,
                ContentType = contentType,
                JsonRequestBehavior = behavior,
                FormateStr = "yyyy-MM-dd HH:mm:ss"
            };
        }
        /// <summary>
        /// 返回JsonResult.24         /// </summary>
        /// <param name="data">数据</param>
        /// <param name="behavior">行为</param>
        /// <param name="format">json中dateTime类型的格式</param>
        /// <returns>Json</returns>
        protected JsonResult MyJson(object data, JsonRequestBehavior behavior, string format)
        {
            return new ToJsonResult
            {
                Data = data,
                JsonRequestBehavior = behavior,
                FormateStr = format
            };
        }
        /// <summary>
        /// 返回JsonResult42         /// </summary>
        /// <param name="data">数据</param>
        /// <param name="format">数据格式</param>
        /// <returns>Json</returns>
        protected JsonResult MyJson(object data, string format)
        {
            return new ToJsonResult
            {
                Data = data,
                FormateStr = format
            };
        }

    }
}
