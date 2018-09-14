using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;
using System.Globalization;

namespace FytMsys.Logic.Admin
{
    /// <summary>
    /// 商城自定义属性表
    /// </summary>
    public class ShopAttrController : BaseController
    {
        #region 1、[商城自定义属性管理]  初始化+Index()
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

        #region 2、[商城自定义属性管理] 获取数据+IndexData()
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
                var typeId = FytRequest.GetFormInt("typeId"); //归属栏目
                var where = PredicateBuilder.True<tb_GoodsAttr>();
                where = where.And(m => m.Flag == typeId);
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = GetTreeList().Select(m => new { m.ID, m.Icon, m.Title, m.EnTitle, m.Sort, m.Flag, m.ParentId }).ToList();
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ShopAttrController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[商城自定义属性管理] 修改模板加载视图+Modfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modfiy(int id)
        {
            var model = OperateContext<tb_GoodsAttr>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                //获得最大的ID
                var maxModel = OperateContext<tb_GoodsAttr>.SetServer.GetList(m => true, m => m.Sort, false).FirstOrDefault();
                var maxId = maxModel != null ? maxModel.Sort : 0;
                model = new tb_GoodsAttr()
                {
                    Flag = 1,
                    Sort = maxId + 1
                };
            }
            //添加子栏目，接受值
            var parentId = FytRequest.GetQueryInt("ParentId", 0);
            if (parentId != 0)
                model.ParentId = parentId;
            //父级下拉
            var sList = GetTreeList();
            var pardrop = sList.Select(p => new SelectListItem
            {
                Text =
                    UtilsHelper.StringOfChar(Convert.ToInt32(p.Flag) - 1, "　") + (p.Flag == 1 ? "" : "├ ") +
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
        public List<tb_GoodsAttr> GetTreeList()
        {
            var where = PredicateBuilder.True<tb_GoodsAttr>();
            var oldList = OperateContext<tb_GoodsAttr>.SetServer.GetList(where, p => p.Sort, true).ToList();
            var sList = new List<tb_GoodsAttr>();
            Recursion(oldList, sList, 0);
            return sList;
        }
        /// <summary>  
        /// 递归菜单列表
        /// </summary>  
        private void Recursion(List<tb_GoodsAttr> oldList, List<tb_GoodsAttr> newList, int pid)
        {
            foreach (var item in from c in oldList where c.ParentId == pid select c)
            {
                var model = item;
                newList.Add(model);
                Recursion(oldList, newList, item.ID);
            }
        }
        #endregion

        #region 4、[商城自定义属性管理] 修改模板加载视图+Modfiy(tb_GoodsAttr model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modfiy(tb_GoodsAttr model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                if (ModelState.IsValid)
                {
                    //根据模板ID，查询模板的地址
                    if (model.ID == 0)
                    {
                        var tempM = OperateContext<tb_GoodsAttr>.SetServer.Add(model);
                        if (model.ParentId > 0)
                        {
                            //说明有父级  根据父级，查询对应的模型
                            var parModel =
                                OperateContext<tb_GoodsAttr>.SetServer.GetModel(m => m.ID == model.ParentId);
                            if (parModel != null)
                            {
                                model.Flag = parModel.Flag + 1;
                            }
                        }
                        OperateContext<tb_GoodsAttr>.SetServer.Update(model);
                    }
                    else
                    {
                        OperateContext<tb_GoodsAttr>.SetServer.Update(model);
                    }
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ShopAttrController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[商城自定义属性管理] 删除记录+DeleteBy()
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
                    OperateContext<tb_GoodsAttr>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_GoodsAttr>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(ShopAttrController), ex);
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
                    OperateContext<tb_GoodsAttr>.SetServer.GetList(
                        PredicateBuilder.True<tb_GoodsAttr>().And(m => m.ParentId == p)
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
                                    OperateContext<tb_GoodsAttr>.SetServer.Update(item);
                                    var nitem = list[index];
                                    nitem.Sort = b;
                                    OperateContext<tb_GoodsAttr>.SetServer.Update(nitem);
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
                                    OperateContext<tb_GoodsAttr>.SetServer.Update(item);
                                    var nitem = list[index - 2];
                                    nitem.Sort = b;
                                    OperateContext<tb_GoodsAttr>.SetServer.Update(nitem);
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
                                    OperateContext<tb_GoodsAttr>.SetServer.Update(item);
                                    var nitem = list[index];
                                    nitem.Sort = b;
                                    OperateContext<tb_GoodsAttr>.SetServer.Update(nitem);
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
                                    OperateContext<tb_GoodsAttr>.SetServer.Update(item);
                                    var nitem = list[index - 2];
                                    nitem.Sort = b;
                                    OperateContext<tb_GoodsAttr>.SetServer.Update(nitem);
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
