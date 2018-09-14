using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Entity;
using Domain.ViewModel;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Logic.Admin
{
    /// <summary>
    /// 系统基本设置控制器
    /// </summary>
    public class SysBasicController : BaseController
    {
        #region 1、系统基本设置，主页 +Index()
        /// <summary>
        /// 系统基本设置，主页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = CacheHelper.Get<SysBasicConfig>(KeyHelper.CACHE_SITE_CONFIG);
            if (model != null) return View(model);
            CacheHelper.Insert(KeyHelper.CACHE_SITE_CONFIG, LoadConfig(UtilsHelper.GetXmlMapPath(KeyHelper.FILE_SITE_XML_CONFING)),
                UtilsHelper.GetXmlMapPath(KeyHelper.FILE_SITE_XML_CONFING));
            model = CacheHelper.Get<SysBasicConfig>(KeyHelper.CACHE_SITE_CONFIG);
            return View(model);
        }
        /// <summary>
        ///  读取站点配置文件
        /// </summary>
        public static SysBasicConfig LoadConfig(string configFilePath)
        {
            return (SysBasicConfig)SerializationHelper.Load(typeof(SysBasicConfig), configFilePath);
        }
        #endregion

        #region 2、保存系统基本设置 +Index(SysBasicConfig model)
        /// <summary>
        /// 保存系统基本设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(SysBasicConfig model)
        {
            SerializationHelper.Save(model, UtilsHelper.GetXmlMapPath(KeyHelper.FILE_SITE_XML_CONFING));

            //判断IOS是否更新
            var iosmodel = OperateContext<tb_VersionUpdate>.SetServer.GetList(m => m.Types == "IOS", m => m.ID, false).FirstOrDefault();
            if (iosmodel == null || model.iosverison != iosmodel.Version)
            {
                var newinfo = new tb_VersionUpdate()
                {
                    Types = "IOS",
                    Version = model.iosverison,
                    Address = "",
                    Summary = model.iossummary,
                    AddDate = DateTime.Now
                };
                OperateContext<tb_VersionUpdate>.SetServer.SaveOrUpdate(newinfo, 0);
            }
            else if (model.iosverison == iosmodel.Version)
            {
                iosmodel.Summary = model.iossummary;
                OperateContext<tb_VersionUpdate>.SetServer.SaveOrUpdate(iosmodel, iosmodel.ID);
            }

            //判断Android是否更新
            var androidmodel = OperateContext<tb_VersionUpdate>.SetServer.GetList(m => m.Types == "Android", m => m.ID, false).FirstOrDefault();
            if (androidmodel == null || model.iosverison != androidmodel.Version)
            {
                var newinfo = new tb_VersionUpdate()
                {
                    Types = "Android",
                    Version = model.androidversion,
                    Address = model.androidfile,
                    Summary = model.androidsummary,
                    AddDate = DateTime.Now
                };
                OperateContext<tb_VersionUpdate>.SetServer.SaveOrUpdate(newinfo, 0);
            }
            else if (model.androidversion == androidmodel.Version)
            {
                androidmodel.Summary = model.androidsummary;
                OperateContext<tb_VersionUpdate>.SetServer.SaveOrUpdate(androidmodel, androidmodel.ID);
            }

            return Json(new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "修改成功！", BackUrl = "" });
        }
        #endregion

        #region 3、应用安装统计+InstallCount()
        public ActionResult InstallCount()
        {
            var year = FytRequest.GetQueryInt("yearSelect");
            var month = FytRequest.GetQueryInt("monthSelect");
            //各系统安装比例
            GYearCount(year, month);
            //按年份统计
            TYearCount();
            //按月份统计
            YearCount(year);
            //按日统计
            MonthCount(year, month);

            return View();
        }

        //各系统安装比例
        void GYearCount(int year, int month)
        {
            var iwhere = PredicateBuilder.True<tb_MobileLog>().And(m => m.Type == "IOS");
            var awhere = PredicateBuilder.True<tb_MobileLog>().And(m => m.Type == "Android");
            if (year > 0)
            {
                iwhere = iwhere.And(m => m.AddDate.Year == year);
                awhere = awhere.And(m => m.AddDate.Year == year);
            }
            if (month > 0)
            {
                iwhere = iwhere.And(m => m.AddDate.Month == month);
                awhere = awhere.And(m => m.AddDate.Month == month);
            }
            int MaxNum = 0;
            var IOSNum = OperateContext<tb_MobileLog>.SetServer.Count(iwhere);
            var AndoridNum = OperateContext<tb_MobileLog>.SetServer.Count(awhere);
            MaxNum = IOSNum >= AndoridNum ? IOSNum : AndoridNum;

            ViewBag.MaxNum = MaxNum;
            ViewBag.MaleNum = IOSNum;
            ViewBag.FemaleNum = AndoridNum;
        }

        //按年份统计
        void TYearCount()
        {
            StringBuilder strcategory = new StringBuilder();
            //IOS变量
            StringBuilder strIOSdata = new StringBuilder();
            //Andorid变量
            StringBuilder strAndoriddata = new StringBuilder();

            //查询IOS数据并保存
            dynamic dList = OperateSession.SetContext.Database.SqlQueryForDynamic("select count(1) as p,YEAR(AddDate) as y from tb_MobileLog where Type='IOS' group by YEAR(AddDate)");
            if (dList != null)
            {
                foreach (var item in dList)
                {
                    strcategory.Append("'" + item.y + "年',");
                    strIOSdata.Append("" + item.p + ",");
                }
                strcategory.Remove(strcategory.Length - 1, 1);
                strIOSdata.Remove(strIOSdata.Length - 1, 1);
            }
            else
            {
                strcategory.Append("'" + DateTime.Now.Year + "年',");
                strIOSdata.Append("0,");

                strcategory.Remove(strcategory.Length - 1, 1);
                strIOSdata.Remove(strIOSdata.Length - 1, 1);
            }
            ViewBag.category_tyear = strcategory;
            ViewBag.mdata_tyear = strIOSdata;

            //查询Android数据并保存
            dList = OperateSession.SetContext.Database.SqlQueryForDynamic("select count(1) as p,YEAR(AddDate) as y from tb_MobileLog where Type='Android' group by YEAR(AddDate)");
            if (dList != null)
            {
                foreach (var item in dList)
                {
                    strAndoriddata.Append("" + item.p + ",");
                }
                strAndoriddata.Remove(strAndoriddata.Length - 1, 1);
            }
            else
            {
                strAndoriddata.Append("0,");

                strAndoriddata.Remove(strAndoriddata.Length - 1, 1);
            }
            ViewBag.fdata_tyear = strAndoriddata;
        }

        //按月份统计
        void YearCount(int year)
        {
            StringBuilder strcategory = new StringBuilder();
            //IOS变量
            StringBuilder strIOSdata = new StringBuilder();
            //Android变量
            StringBuilder strAndoriddata = new StringBuilder();

            if (year == 0)
            {
                year = DateTime.Now.Year;
            }

            //查询IOS数据并保存
            dynamic dList = OperateSession.SetContext.Database.SqlQueryForDynamic("select count(1) as p,MONTH(AddDate) as m from tb_MobileLog where Type='IOS' and YEAR(AddDate)=" + year + " group by month(AddDate)");
            if (dList != null)
            {
                for (int i = 1; i < 13; i++)
                {
                    int j = 0;
                    foreach (var item in dList)
                    {
                        if (i == Convert.ToInt32(item.m))
                        {
                            j = Convert.ToInt32(item.p);
                            break;
                        }
                    }
                    if (j > 0)
                    {
                        strcategory.Append("'" + i + "月',");
                        strIOSdata.Append("" + j + ",");
                    }
                    else
                    {
                        strcategory.Append("'" + i + "月',");
                        strIOSdata.Append("0,");
                    }
                }
                strcategory.Remove(strcategory.Length - 1, 1);
                strIOSdata.Remove(strIOSdata.Length - 1, 1);
            }
            else
            {
                for (int i = 1; i < 13; i++)
                {
                    strcategory.Append("'" + i + "月',");
                    strIOSdata.Append("0,");
                }
                strcategory.Remove(strcategory.Length - 1, 1);
                strIOSdata.Remove(strIOSdata.Length - 1, 1);
            }
            ViewBag.category_year = strcategory;
            ViewBag.mdata_year = strIOSdata;

            //查询Android数据并保存
            dList = OperateSession.SetContext.Database.SqlQueryForDynamic("select count(1) as p,MONTH(AddDate) as m from tb_MobileLog where Type='Android' and YEAR(AddDate)=" + year + " group by month(AddDate)");
            if (dList != null)
            {
                for (int i = 1; i < 13; i++)
                {
                    int j = 0;
                    foreach (var item in dList)
                    {
                        if (i == Convert.ToInt32(item.m))
                        {
                            j = Convert.ToInt32(item.p);
                            break;
                        }
                    }
                    if (j > 0)
                    {
                        strAndoriddata.Append("" + j + ",");
                    }
                    else
                    {
                        strAndoriddata.Append("0,");
                    }
                }
                strAndoriddata.Remove(strAndoriddata.Length - 1, 1);
            }
            else
            {
                for (int i = 1; i < 13; i++)
                {
                    strAndoriddata.Append("0,");
                }
                strAndoriddata.Remove(strAndoriddata.Length - 1, 1);
            }
            ViewBag.fdata_year = strAndoriddata;
        }

        //按日统计
        void MonthCount(int year, int month)
        {
            StringBuilder strcategory = new StringBuilder();
            //IOS变量
            StringBuilder strIOSdata = new StringBuilder();
            //Android变量
            StringBuilder strAndoriddata = new StringBuilder();

            if (year == 0)
            {
                year = DateTime.Now.Year;
            }
            if (month == 0)
            {
                month = DateTime.Now.Month;
            }
            int days = DateTime.DaysInMonth(year, month);

            //查询IOS数据并保存
            dynamic dList = OperateSession.SetContext.Database.SqlQueryForDynamic("select count(1) as p,DAY(AddDate) as m from tb_MobileLog where Type='IOS' and MONTH(AddDate)=" + month + " and YEAR(AddDate)=" + year + " group by day(AddDate)");
            if (dList != null)
            {
                for (int i = 1; i < days + 1; i++)
                {
                    int j = 0;
                    foreach (var item in dList)
                    {
                        if (i == Convert.ToInt32(item.m))
                        {
                            j = Convert.ToInt32(item.p);
                            break;
                        }
                    }
                    if (j > 0)
                    {
                        strcategory.Append("'" + i + "日',");
                        strIOSdata.Append("" + j + ",");
                    }
                    else
                    {
                        strcategory.Append("'" + i + "日',");
                        strIOSdata.Append("0,");
                    }
                }
                strcategory.Remove(strcategory.Length - 1, 1);
                strIOSdata.Remove(strIOSdata.Length - 1, 1);
            }
            else
            {
                for (int i = 1; i < days + 1; i++)
                {
                    strcategory.Append("'" + i + "日',");
                    strIOSdata.Append("0,");
                }
                strcategory.Remove(strcategory.Length - 1, 1);
                strIOSdata.Remove(strIOSdata.Length - 1, 1);
            }
            ViewBag.category_month = strcategory;
            ViewBag.mdata_month = strIOSdata;

            //查询IOS数据并保存
            dList = OperateSession.SetContext.Database.SqlQueryForDynamic("select count(1) as p,DAY(AddDate) as m from tb_MobileLog where Type='Android' and MONTH(AddDate)=" + month + " and YEAR(AddDate)=" + year + " group by day(AddDate)");
            if (dList != null)
            {
                for (int i = 1; i < days + 1; i++)
                {
                    int j = 0;
                    foreach (var item in dList)
                    {
                        if (i == Convert.ToInt32(item.m))
                        {
                            j = Convert.ToInt32(item.p);
                            break;
                        }
                    }
                    if (j > 0)
                    {
                        strAndoriddata.Append("" + j + ",");
                    }
                    else
                    {
                        strAndoriddata.Append("0,");
                    }
                }
                strAndoriddata.Remove(strAndoriddata.Length - 1, 1);
            }
            else
            {
                for (int i = 1; i < days + 1; i++)
                {
                    strAndoriddata.Append("0,");
                }
                strAndoriddata.Remove(strAndoriddata.Length - 1, 1);
            }
            ViewBag.fdata_month = strAndoriddata;
        }

        #endregion


        #region 1、[系统模板管理] 系统模型管理 初始化+TempIndex()
        /// <summary>
        /// 系统模型管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TempIndex()
        {
            return View();
        }
        #endregion

        #region 2、[系统模板管理] 系统模型管理获取数据+TempData()
        /// <summary>
        /// 系统模型管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TempData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key");
                var where = PredicateBuilder.True<tb_SysTemp>();
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                    where = where.Or(m => m.Url.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_SysTemp>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.Sort)
                    .Select(m => new { m.ID, m.Title, m.Url, m.IsLock, m.Sort, m.AddDate });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysBasicController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[系统模板管理] 修改模板加载视图+TempModfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TempModfiy(int id)
        {
            var model = OperateContext<tb_SysTemp>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_SysTemp() { SiteId = SiteID, IsLock = true, AddDate = DateTime.Now };
            }
            return View(model);
        }
        #endregion

        #region 4、[系统模板管理] 修改模板加载视图+TempModfiy(tb_SysTemp model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TempModfiy(tb_SysTemp model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                if (ModelState.IsValid)
                {
                    //添加或修改
                    OperateContext<tb_SysTemp>.SetServer.SaveOrUpdate(model, model.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SiteSetController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[系统模板管理] 系统模型管理-删除记录+DeleteBy()
        /// <summary>
        /// 系统模型管理-删除记录
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
                    OperateContext<tb_SysTemp>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_SysTemp>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysBasicController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion


        #region 1、[系统日志管理] 系统日志管理 初始化+SysLogIndex()
        /// <summary>
        /// 系统日志管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SysLogIndex()
        {
            return View();
        }
        #endregion

        #region 2、[系统日志管理] 系统日志管理获取数据+TempData()
        /// <summary>
        /// 系统日志管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SysLogData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询
                var logType = FytRequest.GetFormInt("logType"); //日志类型查询
                string beginTime = FytRequest.GetFormString("beginTime"),  //日志开始时间
                    endTime = FytRequest.GetFormString("endTime"); //日志结束时间
                var where = PredicateBuilder.True<Domain.Entity.tb_SystemLog>();
                if (key != "")
                {
                    where = where.And(m => m.loginName.Contains(key));
                    where = where.Or(m => m.title.Contains(key));
                }
                if (logType != -1)
                {
                    where = where.And(m => m.logType == logType);
                }
                if (beginTime != "" && endTime != "")
                {
                    var bt = Convert.ToDateTime(beginTime);
                    var et = Convert.ToDateTime(endTime);
                    where = where.And(m => m.addDate >= bt && m.addDate <= et);
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<Domain.Entity.tb_SystemLog>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.addDate).ToList();
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysBasicController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[系统日志管理] 系统日志管理-删除记录+SysLogDeleteBy()
        /// <summary>
        /// 系统日志管理-删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SysLogDeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    OperateContext<Domain.Entity.tb_SystemLog>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<Domain.Entity.tb_SystemLog>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysBasicController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion


        #region 1、[APP版本管理] APP版本管理 初始化 +VersionIndex()
        /// <summary>
        /// APP版本管理 初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult VersionIndex()
        {
            return View();
        }
        #endregion

        #region 2、[APP版本管理] APP版本管理 获取数据 +VersionData()
        /// <summary>
        /// APP版本管理 获取数据
        /// </summary>
        /// <returns></returns>
        public ActionResult VersionData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var versionType = FytRequest.GetFormString("versionType"); //日志类型查询
                string beginTime = FytRequest.GetFormString("beginTime"),  //日志开始时间
                    endTime = FytRequest.GetFormString("endTime"); //日志结束时间
                var where = PredicateBuilder.True<Domain.Entity.tb_VersionUpdate>();
                if (!string.IsNullOrEmpty(versionType))
                {
                    where = where.And(m => m.Types == versionType);
                }
                if (beginTime != "" && endTime != "")
                {
                    var bt = Convert.ToDateTime(beginTime);
                    var et = Convert.ToDateTime(endTime);
                    where = where.And(m => m.AddDate >= bt && m.AddDate <= et);
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<Domain.Entity.tb_VersionUpdate>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.AddDate).ToList();
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysBasicController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[APP版本管理] APP版本管理 修改模板加载视图 +VersionModify(int id)
        /// <summary>
        /// APP版本管理 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        public ActionResult VersionModify(int id)
        {
            var model = OperateContext<tb_VersionUpdate>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_VersionUpdate() { Version = "1", Types = "IOS" };
            }
            return View(model);
        }
        #endregion

        #region 4、[APP版本管理] APP版本管理 修改信息 + VersionModify(tb_VersionUpdate model)
        /// <summary>
        /// APP版本管理 修改信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VersionModify(tb_VersionUpdate model)
        {

            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                model.AddDate = DateTime.Now;
                if (string.IsNullOrEmpty(model.Address))
                {
                    model.Address = "";
                }
                if (ModelState.IsValid)
                {
                    //添加或修改
                    OperateContext<tb_VersionUpdate>.SetServer.SaveOrUpdate(model, model.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysBasicController), ex);
            }
            return Json(jsonM);
        }
        #endregion

    }
}
