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
    public class ColumnController : BaseController
    {
        #region 1、[栏目管理] 栏目列表-加载+Index()
        /// <summary>
        /// 栏目列表-加载
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //站点类型
            ViewBag.typeId = FytRequest.GetQueryInt("typeId");
            return View();
        }
        #endregion

        #region 2、[栏目管理] 获取数据+IndexData()
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
                var key = FytRequest.GetFormStringEncode("key");
                int typeId = FytRequest.GetFormInt("typeId");
                var where = PredicateBuilder.True<tb_Column>();
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = GetTreeList(typeId).Select(m => new { m.ID, m.Title, m.Sort, tName = m.tb_SysTemp.Title, m.ClassLayer, m.ParentId });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ColumnController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[栏目管理] 修改模板加载视图+ColumnModfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ColumnModfiy(int id)
        {
            var model = OperateContext<tb_Column>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                //获得最大的ID
                var maxModel = OperateContext<tb_Column>.SetServer.GetList(m => true, m => m.Sort, false).FirstOrDefault();
                var maxId = maxModel != null ? maxModel.Sort : 0;
                model = new tb_Column()
                {
                    AddDate = DateTime.Now,
                    Number = UtilsHelper.Number(10, false),
                    ClassLayer = 1,
                    Sort = maxId + 1,
                    TypeID = FytRequest.GetQueryInt("typeId", 1)
                };
            }
            //添加子栏目，接受值
            var partId = FytRequest.GetQueryInt("PartId", 0);
            if (partId != 0)
                model.ParentId = partId;
            //下拉模板
            var tempdrop = OperateContext<tb_SysTemp>.SetServer.GetList(m => m.IsLock == true, m => m.Sort, false).ToList();
            var selectList = tempdrop.Select(p => new SelectListItem
            {
                Text = p.Title,
                Value = p.ID.ToString(CultureInfo.InvariantCulture)
            }).ToList();
            ViewBag.selectList = selectList.AsEnumerable();
            //父级下拉
            var typeId = FytRequest.GetQueryInt("typeId");
            var sList = GetTreeList(typeId);
            var pardrop = sList.Select(p => new SelectListItem
            {
                Text =
                    UtilsHelper.StringOfChar(Convert.ToInt32(p.ClassLayer) - 1, "　") + (p.ClassLayer == 1 ? "" : "├ ") +
                    p.Title,
                Value = p.ID.ToString(CultureInfo.InvariantCulture)
            }).ToList();
            pardrop.Insert(0, new SelectListItem() { Text = "父级", Value = "0" });
            ViewBag.pardrop = pardrop.AsEnumerable();
            return View(model);
        }
        /// <summary>
        /// 获得角色列表，根据级别进行分类展示
        /// </summary>
        /// <returns></returns>
        public List<tb_Column> GetTreeList(int typeId)
        {
            var sList = new List<tb_Column>();
            var where = PredicateBuilder.True<tb_Column>();
            where = where.And(m => m.TypeID == typeId);
            where = where.And(m => m.SiteID == SiteID);
            var oldList = OperateContext<tb_Column>.SetServer.GetList(where, p => p.Sort, true).ToList();
            Recursion(oldList, sList, 0);
            return sList;
        }
        /// <summary>  
        /// 递归菜单列表
        /// </summary>  
        private void Recursion(List<tb_Column> oldList, List<tb_Column> newList, int pid)
        {
            foreach (var item in from c in oldList where c.ParentId == pid select c)
            {
                var model = item;
                newList.Add(model);
                Recursion(oldList, newList, item.ID);
            }
        }
        #endregion

        #region 4、[栏目管理] 修改模板加载视图+RoleModfiy(tb_SysTemp model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ColumnModfiy(tb_Column model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "/FytAdmin/Column/Index" };
            try
            {
                if (ModelState.IsValid)
                {
                    //根据模板ID，查询模板的地址
                    model.TempUrl = OperateContext<tb_SysTemp>.SetServer.GetModel(m => m.ID == model.TempId).Url;
                    int pid = FytRequest.GetFormInt("pid");
                    model.SiteID = SiteID;
                    if (model.ID == 0)
                    {
                        var tempM = OperateContext<tb_Column>.SetServer.Add(model);
                        if (model.ParentId > 0)
                        {
                            //说明有父级  根据父级，查询对应的模型
                            var parModel =
                                OperateContext<tb_Column>.SetServer.GetModel(m => m.ID == model.ParentId);
                            if (parModel != null)
                            {
                                model.ClassList = parModel.ClassList + tempM.ID + ",";
                                model.ClassLayer = parModel.ClassLayer + 1;
                            }
                        }
                        else
                        {
                            //没有父级
                            model.ClassList = "," + tempM.ID + ",";
                        }
                        OperateContext<tb_Column>.SetServer.Update(model);
                    }
                    else
                    {
                        OperateContext<tb_Column>.SetServer.Update(model);
                    }
                    jsonM.BackUrl = "/FytAdmin/Column/Index?typeId=" + model.TypeID;
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ColumnController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[栏目管理] 删除记录+ColumnDeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ColumnDeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    OperateContext<tb_Column>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_Column>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ColumnController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 6、排序 +ColSort
        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public ActionResult ColSort()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！" };
            try
            {
                int p = FytRequest.GetFormInt("p"),
                    i = FytRequest.GetFormInt("i"),
                    o = FytRequest.GetFormInt("o"),
                    a = 0, b = 0, c = 0;
                var list =
                    OperateContext<tb_Column>.SetServer.GetList(
                        PredicateBuilder.True<tb_Column>().And(m => m.ParentId == p)
                        , m => m.Sort, true).ToList();
                if (list.Count > 0)
                {
                    var index = 0;
                    foreach (var item in list)
                    {
                        index++;
                        if (index == 1)
                        {
                            if (item.ID == i) //判断是否是头如果上升则不做处理
                            {
                                if (o == 1) //下降一位
                                {
                                    a = Convert.ToInt32(item.Sort);
                                    b = Convert.ToInt32(list[index].Sort);
                                    c = a;
                                    a = b;
                                    b = c;
                                    item.Sort = a;
                                    OperateContext<tb_Column>.SetServer.Update(item);
                                    var nitem = list[index];
                                    nitem.Sort = b;
                                    OperateContext<tb_Column>.SetServer.Update(nitem);
                                    break;
                                }
                            }
                        }
                        else if (index == list.Count)
                        {
                            if (item.ID == i) //最后一条如果下降则不做处理
                            {
                                if (o == 0) //上升一位
                                {
                                    a = Convert.ToInt32(item.Sort);
                                    b = Convert.ToInt32(list[index - 2].Sort);
                                    c = a;
                                    a = b;
                                    b = c;
                                    item.Sort = a;
                                    OperateContext<tb_Column>.SetServer.Update(item);
                                    var nitem = list[index - 2];
                                    nitem.Sort = b;
                                    OperateContext<tb_Column>.SetServer.Update(nitem);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (item.ID == i) //判断是否是头如果上升则不做处理
                            {
                                if (o == 1) //下降一位
                                {
                                    a = Convert.ToInt32(item.Sort);
                                    b = Convert.ToInt32(list[index].Sort);
                                    c = a;
                                    a = b;
                                    b = c;
                                    item.Sort = a;
                                    OperateContext<tb_Column>.SetServer.Update(item);
                                    var nitem = list[index];
                                    nitem.Sort = b;
                                    OperateContext<tb_Column>.SetServer.Update(nitem);
                                    break;
                                }
                                else
                                {
                                    a = Convert.ToInt32(item.Sort);
                                    b = Convert.ToInt32(list[index - 2].Sort);
                                    c = a;
                                    a = b;
                                    b = c;
                                    item.Sort = a;
                                    OperateContext<tb_Column>.SetServer.Update(item);
                                    var nitem = list[index - 2];
                                    nitem.Sort = b;
                                    OperateContext<tb_Column>.SetServer.Update(nitem);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ColumnController), ex);
            }
            return Json(jsonM);
        }
        #endregion
    }
}
