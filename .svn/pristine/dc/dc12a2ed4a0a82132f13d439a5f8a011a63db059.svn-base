using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Logic.Admin
{
    /// <summary>
    /// 招聘模块
    /// </summary>
    public class FytJobController:BaseController
    {
        #region 1、[招聘管理]  初始化+Index()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 2、[招聘管理] 获取数据+IndexData()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IndexData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询
                var classId = FytRequest.GetFormInt("classId"); //归属栏目
                var where = PredicateBuilder.True<tb_Job>();
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                }
                if (classId!=0)
                {
                    where = where.And(m => m.ClassId==classId);
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_Job>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.ID).ToList();
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytJobController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[招聘管理] 修改模板加载视图+Modfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modfiy(int id)
        {
            var model = OperateContext<tb_Job>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                var classId = FytRequest.GetQueryInt("ClassId", 0);
                model = new tb_Job() {ClassId = classId,AddDate = DateTime.Now};
            }
            return View(model);
        }
        #endregion

        #region 4、[招聘管理] 修改模板加载视图+Modfiy(tb_FytJob model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modfiy(tb_Job model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "/FytAdmin/FytJob/Index" };
            try
            {
                if (ModelState.IsValid)
                {
                    //添加或修改
                    OperateContext<tb_Job>.SetServer.SaveOrUpdate(model, model.ID);
                }
                jsonM.BackUrl = "/FytAdmin/FytJob/Index?ClassId="+model.ClassId;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytJobController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[招聘管理] 删除记录+DeleteBy()
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
                    OperateContext<tb_Job>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_Job>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytJobController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion
    }
}
