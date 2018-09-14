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
    /// 申请博客管理
    /// </summary>
    public class BlogManController:BaseController
    {
        #region 1、[申请博客管理]  初始化+Index()
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

        #region 2、[申请博客管理] 获取数据+IndexData()
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
                var where = PredicateBuilder.True<tb_Blog>();
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_Blog>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.ID)
                    .Select(m=>new
                    {
                        m.ID,m.Title,m.tb_User.LoginName,m.Level,m.Hits
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(BlogManController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[申请博客管理] 修改模板加载视图+Modfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modfiy(int id)
        {
            var model = OperateContext<tb_Blog>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_Blog() { };
            }
            return View(model);
        }
        #endregion

        #region 4、[申请博客管理] 修改模板加载视图+Modfiy(tb_Blog model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modfiy(tb_Blog model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                if (ModelState.IsValid)
                {
                    //添加或修改
                    OperateContext<tb_Blog>.SetServer.SaveOrUpdate(model, model.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(BlogManController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[申请博客管理] 删除记录+DeleteBy()
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
                    OperateContext<tb_Blog>.SetServer.DeleteBy(m => m.ID == id);
                    OperateContext<tb_BlogActicle>.SetServer.DeleteBy(m => m.BlogID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_Blog>.SetServer.DeleteBy(m => result.Contains(m.ID));
                    OperateContext<tb_BlogActicle>.SetServer.DeleteBy(m => result.Contains(m.BlogID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(BlogManController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion



        #region 1、[申请博客管理]  初始化+ArticleIndex()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ArticleIndex()
        {
            return View();
        }
        #endregion

        #region 2、[博文管理] 获取数据+ArticleData()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询
                var where = PredicateBuilder.True<tb_BlogActicle>();
                var isRe = FytRequest.GetFormInt("isRe");
                where = isRe == 1 ? where.And(m => m.IsRecyc == true) : where.And(m => m.IsRecyc==false);
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_BlogActicle>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.ID)
                    .Select(m => new
                    {
                        m.ID,
                        m.Title,
                        blogTitle=m.tb_Blog.Title,
                        m.Author,
                        m.Audit,
                        m.Hits,
                        m.EditDate
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(BlogManController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[博文管理] 修改模板加载视图+ArticleModfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ArticleModfiy(int id)
        {
            var model = OperateContext<tb_BlogActicle>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_BlogActicle() {EditDate = DateTime.Now, AddDate = DateTime.Now};
            }
            else
            {
                model.EditDate = DateTime.Now;
            }
            return View(model);
        }
        #endregion

        #region 4、[申请博客管理] 修改模板加载视图+ArticleModfiy(tb_BlogActicle model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ArticleModfiy(tb_BlogActicle model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "/FytAdmin/BlogMan/ArticleIndex" };
            try
            {
                if (string.IsNullOrEmpty(model.Tag))
                {
                    model.Tag = WordSpliter.GetKeyword(model.Title, 6); //根据标题获得Tags
                }
                if (string.IsNullOrEmpty(model.Summary))
                {
                    model.Summary = UtilsHelper.DropHTML(model.Content, 200); //在内容中截取200个字符放入到摘要内
                }
                if (ModelState.IsValid)
                {
                    //添加或修改
                    OperateContext<tb_BlogActicle>.SetServer.SaveOrUpdate(model, model.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(BlogManController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[申请博客管理] 删除记录到回收站+ArticleGoRecyc()
        /// <summary>
        /// 删除记录到回收站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleGoRecyc()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    var model = OperateContext<tb_BlogActicle>.SetServer.GetModel(id);
                    model.IsRecyc = true;
                    model.DelDate = DateTime.Now;
                    OperateContext<tb_BlogActicle>.SetServer.Update(model, new string[] { "IsRecyc" });
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    var mList = OperateContext<tb_BlogActicle>.SetServer.GetList(m => result.Contains(m.ID), m => m.ID, true).ToList();
                    foreach (var art in mList)
                    {
                        art.IsRecyc = true;
                        art.DelDate = DateTime.Now;
                    }
                    OperateContext<tb_BlogActicle>.SetServer.Update(mList);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(BlogManController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 6、[申请博客管理] 删除记录+ArticleDeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleDeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    OperateContext<tb_BlogActicle>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_BlogActicle>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(BlogManController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 7、[申请博客管理-清空回收站]  初始化+EmptyRecyc()
        /// <summary>
        /// 文章管理-清空回收站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EmptyRecyc()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                OperateContext<tb_BlogActicle>.SetServer.DeleteBy(m => m.IsRecyc == true);
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "清空数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(BlogManController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 8、[申请博客管理-还原回收站] 还原回收站+ReductionRecyc()
        /// <summary>
        /// 还原回收站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReductionRecyc()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    var model = OperateContext<tb_BlogActicle>.SetServer.GetModel(id);
                    model.IsRecyc = false;
                    OperateContext<tb_BlogActicle>.SetServer.Update(model, new string[] { "IsRecyc" });
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    var mList = OperateContext<tb_BlogActicle>.SetServer.GetList(m => result.Contains(m.ID), m => m.ID, true).ToList();
                    foreach (var art in mList)
                    {
                        art.IsRecyc = false;
                    }
                    OperateContext<tb_BlogActicle>.SetServer.Update(mList);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(BlogManController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion
    }
}
