using Domain.Entity;
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
    /// 旅程故事管理
    /// </summary>
    public class FytStoryController : BaseController
    {
        #region 1、[旅程故事管理] 初始化 + Index()
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 2、[旅程故事管理] 加载数据 + IndexData()
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
                var lq = from gl in OperateSession.SetContext.lv_Story
                         orderby gl.UpdateTime descending
                         select new
                         {
                             gl.ID,
                             gl.tb_User.NickName,
                             gl.tb_User.TrueName,
                             gl.Title,
                             gl.IsTop,
                             gl.SubTitle,
                             gl.Tag,
                             gl.Hits,
                             gl.UpdateTime
                         };
                if (key != "")
                {
                    lq = lq.Where(m => m.Title.Contains(key) || m.SubTitle.Contains(key) || m.Tag.Contains(key));
                }
                jsonM.Data = lq.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                jsonM.PageRows = lq.Count();
                jsonM.PageTotal = Convert.ToInt32(Math.Ceiling((lq.Count() * 1.0) / pageSize));
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytStoryController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[旅程故事管理] 删除记录 + DeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    OperateContext<lv_Story>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<lv_Story>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytStoryController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 4、[旅程故事管理] 查看详细 + Detail()
        /// <summary>
        /// 查看详细
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            var model = OperateContext<lv_Story>.SetServer.GetModel(m => m.ID == id);
            return View(model);
        }
        #endregion

        #region 5、[旅行故事管理] 一键推荐 + TopSet()
        /// <summary>
        /// 一键推荐
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TopSet()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                var types = FytRequest.GetFormInt("types");//操作类型（0=取消推荐/1=推荐）
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    var model = OperateContext<lv_Story>.SetServer.GetModel(m => m.ID == id);
                    model.IsTop = types == 1 ? "1" : "0";
                    OperateContext<lv_Story>.SetServer.Update(model);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    var list = OperateContext<lv_Story>.SetServer.GetList(m => result.Contains(m.ID), m => m.ID, true).ToList();
                    foreach (var item in list)
                    {
                        item.IsTop = types == 1 ? "1" : "0";
                    }
                    OperateContext<lv_Story>.SetServer.Update(list);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "修改数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytStoryController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion
    }
}
