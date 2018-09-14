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
    /// <summary>
    /// 银行账号后台管理
    /// </summary>
    public class FytBankController : BaseController
    {
        #region 1、[银行账户管理] 初始化 + Index()
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 2、[银行账户管理] 加载数据 + IndexData()
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IndexData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询         
                int pageSize = FytRequest.GetFormInt("pageSize", 1),
                    pageIndex = FytRequest.GetFormInt("pageIndex", 10);
                var lq = from b in OperateSession.SetContext.lv_Bank
                         orderby b.AddTime descending
                         select new
                         {
                             b.ID,
                             b.tb_User.NickName,
                             b.tb_User.TrueName,
                             b.UserName,
                             b.BankName,
                             b.BankAccont,
                             b.BankAddress,
                             b.AddTime
                         };
                if (key != "")
                {
                    lq = lq.Where(m => m.UserName.Contains(key));
                }
                jsonM.Data = lq.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                jsonM.PageRows = lq.Count();
                jsonM.PageTotal = Convert.ToInt32(Math.Ceiling((lq.Count() * 1.0) / pageSize));
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytBankController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion
    }
}
