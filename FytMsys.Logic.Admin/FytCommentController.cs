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
    /// 评论管理控制器
    /// </summary>
    public class FytCommentController:BaseController
    {
        #region 1、[评论管理]  初始化+Index()
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

        #region 2、[评论管理] 获取数据+IndexData()
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
                var status = FytRequest.GetFormInt("status"); //状态查询
                var option = FytRequest.GetFormInt("option"); //类型
                var where = PredicateBuilder.True<tb_Comment>();
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                    where = where.Or(m => m.Content.Contains(key));
                }
                if (status != 0)
                {
                    where = where.And(m => m.Status==status);
                }
                if (option != 0)
                {
                    where = where.And(m => m.Option == option);
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_Comment>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.ID)
                    .Select(p=>new
                    {
                        p.ID,
                        p.Content,
                        p.tb_User.LoginName,
                        p.UserIP,
                        p.AddDate,
                        p.Status,
                        p.IsUser,
                        p.UserNumber,
                        p.RepContent
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytCommentController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[评论管理] 修改模板加载视图+Modfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modfiy(int id)
        {
            var model = OperateContext<tb_Comment>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_Comment() {  };
            }
            return View(model);
        }
        #endregion

        #region 4、[评论管理] 修改模板加载视图+Modfiy(tb_DownLoad model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modfiy(tb_Comment model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                if (ModelState.IsValid)
                {
                    var tmodel = OperateContext<tb_Comment>.SetServer.GetModel(m => m.ID == model.ID);
                    tmodel.Status = model.Status;
                    tmodel.RepContent = model.RepContent;
                    tmodel.RepDate = DateTime.Now;
                    tmodel.RepId = GetAdminInfo().ID;
                    //添加或修改
                    OperateContext<tb_Comment>.SetServer.SaveOrUpdate(tmodel, tmodel.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytCommentController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[评论管理] 删除记录+DeleteBy()
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
                    OperateContext<tb_Comment>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_Comment>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytCommentController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 6、[评论管理] 更改审核状态+ModfiyStatus()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModfiyStatus()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                var type = FytRequest.GetFormInt("type"); //状态更改 2=通过  3=未通过
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    var model = OperateContext<tb_Comment>.SetServer.GetModel(m => m.ID == id);
                    model.Status = type;
                    OperateContext<tb_Comment>.SetServer.Update(model);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    var listModel=OperateContext<tb_Comment>.SetServer.GetList(m => result.Contains(m.ID),m=>m.ID,true).ToList();
                    listModel.ForEach(m => { m.Status = type; });
                    OperateContext<tb_Comment>.SetServer.Update(listModel);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FytCommentController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion
    }
}
