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
    /// 投票功能模块
    /// </summary>
    public class VoteController:BaseController
    {
        #region 1、[投票管理]  初始化+Index()
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

        #region 2、[投票管理] 获取数据+IndexData()
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
                var where = PredicateBuilder.True<tb_Vote>();
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                }
                if (classId != 0)
                {
                    where = where.And(m => m.ClassId == classId);
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_Vote>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.ID).ToList()
                    .Select(m=>new
                    {
                        m.ID,
                        m.Title,
                        m.VoteSum,
                        m.BeginTime,
                        m.EndTime,
                        m.ItemSum
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(VoteController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[投票管理] 修改模板加载视图+Modfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modfiy(int id)
        {
            var model = OperateContext<tb_Vote>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                var classId = FytRequest.GetQueryInt("ClassId", 0);
                model = new tb_Vote() { ClassId = classId, AddDate = DateTime.Now,Option = true,VoteSum = 0,ItemSum = 3,IsTime = true};
            }
            return View(model);
        }
        #endregion

        #region 4、[投票管理] 修改模板加载视图+Modfiy(tb_Vote model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modfiy(tb_Vote model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "/FytAdmin/Vote/Index" };
            try
            {
                if (ModelState.IsValid)
                {
                    model.ItemSum = FytRequest.GetFormInt("ItemSum");
                    if (model.ID == 0)
                    {
                        var addModel=OperateContext<tb_Vote>.SetServer.Add(model, true);
                        //添加投票项
                        var hsum = FytRequest.GetFormString("hsum")
                            .Split(new char[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
                        var listModel=new List<tb_VoteItem>();
                        foreach (string s in hsum)
                        {
                            var t = FytRequest.GetFormString("itemTitle_" + s);
                            var n = FytRequest.GetFormInt("itemNum_" + s);
                            listModel.Add(new tb_VoteItem()
                            {
                                VoteId = addModel.ID,Title = t,VoteSum = n,Scale = "0"
                            });
                        }
                        OperateContext<tb_VoteItem>.SetServer.AddEntity(listModel);
                    }
                    else
                    {
                        OperateContext<tb_Vote>.SetServer.Update(model);
                        //添加投票项
                        var hsum = FytRequest.GetFormString("hsum")
                            .Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        //删除所有投票数据
                        OperateContext<tb_VoteItem>.SetServer.DeleteBy(m=>m.VoteId==model.ID);
                        //增加新的投票项
                        var listModel = new List<tb_VoteItem>();
                        foreach (string s in hsum)
                        {
                            var t = FytRequest.GetFormString("itemTitle_" + s);
                            var n = FytRequest.GetFormInt("itemNum_" + s);
                            var st = FytRequest.GetFormString("itemScale_" + s);
                            listModel.Add(new tb_VoteItem()
                            {
                                VoteId = model.ID,
                                Title = t,
                                VoteSum = n,
                                Scale = (string.IsNullOrEmpty(st) ? "0" : st)
                            });
                        }
                        OperateContext<tb_VoteItem>.SetServer.AddEntity(listModel);
                    } 
                }
                jsonM.BackUrl = "/FytAdmin/Vote/Index?ClassId=" + model.ClassId;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(VoteController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[投票管理] 删除记录+DeleteBy()
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
                    OperateContext<tb_VoteItem>.SetServer.DeleteBy(m => m.VoteId == id);
                    OperateContext<tb_Vote>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_VoteItem>.SetServer.DeleteBy(m => result.Contains(m.VoteId));
                    OperateContext<tb_Vote>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(VoteController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 6、[投票管理] 投票查看详细+Detail(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var model = OperateContext<tb_Vote>.SetServer.GetModel(m => m.ID == id);
            return View(model);
        }
        #endregion
    }
}
