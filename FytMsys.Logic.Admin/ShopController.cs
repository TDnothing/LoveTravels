using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Logic.Admin
{
    public class ShopController:BaseController
    {
        #region 1、商品管理的左右视图+Home
        /// <summary>
        /// 商品管理的左右视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            return View();
        } 
        #endregion

        #region 2、加载左侧tree +GetTree()
        /// <summary>
        /// 加载左侧tree
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTree()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var coluList = OperateContext<tb_Column>.SetServer.GetList(m=>m.TypeID==2, m => m.ID, true).ToList()
                .Select(m => new
                {
                    id = m.ID,
                    pId = m.ParentId,
                    name = m.Title,
                    open = true,
                    target = "DeployBase",
                    url = m.TempUrl + m.ID.ToString(CultureInfo.InvariantCulture)
                });
                jsonM.Data = coluList;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ContentController), ex);
            }
            return Json(jsonM);
        } 
        #endregion
    }
}
