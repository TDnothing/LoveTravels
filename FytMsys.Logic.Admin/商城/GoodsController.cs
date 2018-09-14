using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FytMsys.Logic.Admin
{
    public class GoodsController : BaseController
    {

        #region 1、[商品管理]  初始化+Index()
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ClassId = FytRequest.GetQueryInt("ClassId");
            return View();
        }
        #endregion

        #region  2、[商品管理] 获取数据+IndexData()
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IndexData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询
                var where = PredicateBuilder.True<tb_Goods>();
                var classId = Convert.ToInt32(FytRequest.GetFormInt("ClassId"));
                if (classId != 0)
                {
                    where = where.And(m => m.ClassId == classId);
                }
                if (key != "")
                {
                    where = where.And(m => m.GoodsName.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_Goods>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.ID)
                    .Select(m => new
                    {
                        m.ID,
                        m.GoodsNum,
                        m.GoodsName,
                        m.Price,
                        m.Stock,
                        m.IsTop,
                        m.IsList,
                        m.IsDel,
                        m.AddDate
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(GoodsController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[商品管理] 修改模板加载视图+Modify(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modify(int id)
        {
            var model = OperateContext<tb_Goods>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_Goods()
                {
                    ClassId = FytRequest.GetQueryInt("ClassId"),
                    GoodsNum = UtilsHelper.Number(10, true),
                    Price = 0.00M,
                    OriginalPrice = 0.00M,
                    Stock = 1000,
                    SoldNum = 0,
                    LimitNum = 0,
                    IsTop = false,
                    IsList = true,
                    IsDel = false,
                    ShopId = 1,
                    AddDate = DateTime.Now,
                    EditDate = DateTime.Now
                };
            }
            //查询商品品牌
            var shopSelect = OperateContext<tb_GoodsBrand>.SetServer.GetList(m => true, m => m.AddDate, false).ToList();
            var selBrank = shopSelect.Select(s => new SelectListItem
            {
                Text = s.BrandName,
                Value = s.ID.ToString(CultureInfo.InvariantCulture)
            }).ToList();
            ViewBag.selBrank = selBrank.AsEnumerable();
            //查询商品图片列表
            var where = PredicateBuilder.True<tb_ImageLibrary>();
            where = where.And(m => m.ImgID == id);
            where = where.And(m => m.ImgType == 4);
            ViewBag.imgList =
                OperateContext<tb_ImageLibrary>.SetServer.GetList(where, m => m.ID, true).ToList();
            return View(model);
        }
        #endregion

        #region 4、[商品管理] 修改模板加载视图+Modify(tb_Goods model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modify(tb_Goods model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "" };
            try
            {
                if (ModelState.IsValid)
                {
                    var reslist = FytRequest.GetFormString("imlist").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (model.OriginalPrice == 0)
                    {
                        model.OriginalPrice = model.Price;
                    }
                    if (model.GoodsImage == null)
                    {
                        model.GoodsImage = "";
                    }
                    if (model.Content == null)
                    {
                        model.Content = "";
                    }
                    model.EditDate = DateTime.Now;
                    model.BrandsId = FytRequest.GetFormInt("Brands"); 
                    //添加或修改
                    if (model.ID == 0)
                    {
                        model.AddDate = DateTime.Now;
                        var aM = OperateContext<tb_Goods>.SetServer.Add(model);
                        var listModel = new List<tb_ImageLibrary>();
                        foreach (string s in reslist)
                        {
                            var u = FytRequest.GetFormString("file_name_" + s);
                            var thumb = "";
                            if (u != "")
                            {
                                //获得缩略图地址
                                var preU = u.Substring(0, u.LastIndexOf('/') + 1);
                                var nextU = u.Substring(u.LastIndexOf('/') + 1, u.Length - u.LastIndexOf('/') - 1);
                                thumb = preU + "thumb_" + nextU;
                            }
                            var su = FytRequest.GetFormString("file_summary");
                            bool isc = su == s;
                            var so = FytRequest.GetFormInt("file_sort_" + s);
                            listModel.Add(new tb_ImageLibrary()
                            {
                                ImgID = aM.ID,
                                ImgType = 4,
                                ImgUrl = u,
                                ImgSmall = thumb,
                                IsCover = isc
                            });
                        }
                        OperateContext<tb_ImageLibrary>.SetServer.AddEntity(listModel);
                    }
                    else
                    {
                        //执行修改操作
                        OperateContext<tb_Goods>.SetServer.Update(model);
                        //删除所有图片数据
                        OperateContext<tb_ImageLibrary>.SetServer.DeleteBy(m => m.ImgID == model.ID);
                        var listModel = new List<tb_ImageLibrary>();
                        foreach (string s in reslist)
                        {
                            var u = FytRequest.GetFormString("file_name_" + s);
                            var thumb = "";
                            if (u != "")
                            {
                                //获得缩略图地址
                                var preU = u.Substring(0, u.LastIndexOf('/') + 1);
                                var nextU = u.Substring(u.LastIndexOf('/') + 1, u.Length - u.LastIndexOf('/') - 1);
                                thumb = preU + "thumb_" + nextU;
                            }
                            var su = FytRequest.GetFormString("file_summary");
                            bool isc = su == s;
                            var so = FytRequest.GetFormInt("file_sort_" + s);
                            listModel.Add(new tb_ImageLibrary()
                            {
                                ImgID = model.ID,
                                ImgType = 4,
                                ImgUrl = u,
                                ImgSmall = thumb,
                                IsCover = isc
                            });
                        }
                        OperateContext<tb_ImageLibrary>.SetServer.AddEntity(listModel);
                    }
                    jsonM.BackUrl = "/FytAdmin/Goods/Index?ClassId=" + model.ClassId;
                }
            }
            catch (DbEntityValidationException ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(GoodsController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[商品管理] 删除记录+DeleteBy()
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
                    var model = OperateContext<tb_Goods>.SetServer.GetList(m => m.ID == id, m => m.ID, true).FirstOrDefault();
                    if (model != null)
                    {
                        model.IsDel = true;
                        OperateContext<tb_Goods>.SetServer.SaveOrUpdate(model, model.ID);
                    }
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    foreach (var item in result)
                    {
                        var model = OperateContext<tb_Goods>.SetServer.GetList(m => m.ID == item, m => m.ID, true).FirstOrDefault();
                        if (model != null)
                        {
                            model.IsDel = true;
                            OperateContext<tb_Goods>.SetServer.SaveOrUpdate(model, model.ID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(GoodsController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 6、[商品管理拓展]  商品详细+ModelDetail(int id)
        /// <summary>
        /// 商品展示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ModelDetail(int id)
        {
            var model = OperateContext<tb_Goods>.SetServer.GetModel(m => m.ID == id);
            return View(model);
        }
        #endregion

    }
}
