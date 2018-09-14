using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// 会员管理控制器
    /// </summary>
    public class MemberController : BaseController
    {
        #region 1、[会员管理]  初始化+Index()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            TempData["classId"] = 0;
            return View();
        }
        #endregion

        #region 2、[会员管理] 获取数据+IndexData()
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
                var where = PredicateBuilder.True<tb_User>();
                var classId = Convert.ToInt32(TempData["classId"]);
                if (classId != 0)
                {
                    where = where.And(m => m.GroupId == classId);
                }
                if (key != "")
                {
                    where = where.And(m => m.LoginName.Contains(key));
                    where = where.Or(m => m.NickName.Contains(key));
                    where = where.Or(m => m.TrueName.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_User>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.ID)
                    .Select(m => new
                    {
                        m.ID,
                        m.LoginName,
                        m.TrueName,
                        m.Email,
                        m.QQ,
                        m.Province,
                        m.Mobile,
                        m.Point,
                        m.Types,
                        m.RegDate
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[会员管理] 修改模板加载视图+Modfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modfiy(int id)
        {
            var model = OperateContext<tb_User>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_User()
                {
                    Status = true,
                    RegIp = UtilsHelper.GetIP(),
                    RegDate = DateTime.Now,
                    Amount = 0,
                    Point = 0,
                    Exp = 0
                };
            }
            //查询会员组别
            var groupSelect = OperateContext<tb_UserGroup>.SetServer.GetList(m => true, m => m.ID, true).ToList();
            var selectList = groupSelect.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ID.ToString(CultureInfo.InvariantCulture)
            }).ToList();
            ViewBag.selectList = selectList.AsEnumerable();
            return View(model);
        }
        #endregion

        #region 4、[会员管理] 修改模板加载视图+Modfiy(tb_User model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modfiy(tb_User model, tb_UserApp appModel)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "" };
            try
            {
                if (ModelState.IsValid)
                {
                    //添加或修改
                    if (model.ID != 0)
                    {
                        OperateContext<tb_User>.SetServer.Update(model);
                        OperateContext<tb_UserApp>.SetServer.Update(appModel);
                    }
                    else
                    {
                        var tmModel = OperateContext<tb_User>.SetServer.Add(model);
                        appModel.UserId = tmModel.ID;
                        OperateContext<tb_UserApp>.SetServer.Add(appModel);
                    }
                    jsonM.BackUrl = "/FytAdmin/Member/Index";
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[会员管理] 删除记录+DeleteBy()
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
                    OperateContext<tb_User>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_User>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 7、[会员管理] 会员注册统计+RegisterCount()
        public ActionResult RegisterCount()
        {
            var year = FytRequest.GetQueryInt("yearSelect");
            var month = FytRequest.GetQueryInt("monthSelect");
            //男女注册比例
            GYearCount(year, month);
            //按年份统计
            TYearCount();
            //按月份统计
            YearCount(year);
            //按日统计
            MonthCount(year, month);

            return View();
        }

        //男女注册比例
        void GYearCount(int year, int month)
        {
            var mwhere = PredicateBuilder.True<tb_User>().And(m => m.Sex == "男");
            var fwhere = PredicateBuilder.True<tb_User>().And(m => m.Sex == "女");
            if (year > 0)
            {
                mwhere = mwhere.And(m => m.RegDate.Year == year);
                fwhere = fwhere.And(m => m.RegDate.Year == year);
            }
            if (month > 0)
            {
                mwhere = mwhere.And(m => m.RegDate.Month == month);
                fwhere = fwhere.And(m => m.RegDate.Month == month);
            }
            int MaxNum = 0;
            var MaleNum = OperateContext<tb_User>.SetServer.Count(mwhere);
            var FemaleNum = OperateContext<tb_User>.SetServer.Count(fwhere);
            MaxNum = MaleNum >= FemaleNum ? MaleNum : FemaleNum;

            ViewBag.MaxNum = MaxNum;
            ViewBag.MaleNum = MaleNum;
            ViewBag.FemaleNum = FemaleNum;
        }

        //按年份统计
        void TYearCount()
        {
            StringBuilder strcategory = new StringBuilder();
            //男性变量
            StringBuilder strMaledata = new StringBuilder();
            //女性变量
            StringBuilder strFemaledata = new StringBuilder();

            //查询男性数据并保存
            dynamic dList = OperateSession.SetContext.Database.SqlQueryForDynamic("select count(1) as p,YEAR(RegDate) as y from tb_User where Sex='男' group by YEAR(RegDate)");
            if (dList != null)
            {
                foreach (var item in dList)
                {
                    strcategory.Append("'" + item.y + "年',");
                    strMaledata.Append("" + item.p + ",");
                }
                strcategory.Remove(strcategory.Length - 1, 1);
                strMaledata.Remove(strMaledata.Length - 1, 1);
            }
            else
            {
                strcategory.Append("'" + DateTime.Now.Year + "年',");
                strMaledata.Append("0,");

                strcategory.Remove(strcategory.Length - 1, 1);
                strMaledata.Remove(strMaledata.Length - 1, 1);
            }
            ViewBag.category_tyear = strcategory;
            ViewBag.mdata_tyear = strMaledata;

            //查询女性数据并保存
            dList = OperateSession.SetContext.Database.SqlQueryForDynamic("select count(1) as p,YEAR(RegDate) as y from tb_User where Sex='女' group by YEAR(RegDate)");
            if (dList != null)
            {
                foreach (var item in dList)
                {
                    strFemaledata.Append("" + item.p + ",");
                }
                strFemaledata.Remove(strFemaledata.Length - 1, 1);
            }
            else
            {
                strFemaledata.Append("0,");

                strFemaledata.Remove(strFemaledata.Length - 1, 1);
            }
            ViewBag.fdata_tyear = strFemaledata;
        }

        //按月份统计
        void YearCount(int year)
        {
            StringBuilder strcategory = new StringBuilder();
            //男性变量
            StringBuilder strMaledata = new StringBuilder();
            //女性变量
            StringBuilder strFemaledata = new StringBuilder();

            if (year == 0)
            {
                year = DateTime.Now.Year;
            }

            //查询男性数据并保存
            dynamic dList = OperateSession.SetContext.Database.SqlQueryForDynamic("select count(1) as p,MONTH(RegDate) as m from tb_User where Sex='男' and YEAR(RegDate)=" + year + " group by month(RegDate)");
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
                        strMaledata.Append("" + j + ",");
                    }
                    else
                    {
                        strcategory.Append("'" + i + "月',");
                        strMaledata.Append("0,");
                    }
                }
                strcategory.Remove(strcategory.Length - 1, 1);
                strMaledata.Remove(strMaledata.Length - 1, 1);
            }
            else
            {
                for (int i = 1; i < 13; i++)
                {
                    strcategory.Append("'" + i + "月',");
                    strMaledata.Append("0,");
                }
                strcategory.Remove(strcategory.Length - 1, 1);
                strMaledata.Remove(strMaledata.Length - 1, 1);
            }
            ViewBag.category_year = strcategory;
            ViewBag.mdata_year = strMaledata;

            //查询女性数据并保存
            dList = OperateSession.SetContext.Database.SqlQueryForDynamic("select count(1) as p,MONTH(RegDate) as m from tb_User where Sex='女' and YEAR(RegDate)=" + year + " group by month(RegDate)");
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
                        strFemaledata.Append("" + j + ",");
                    }
                    else
                    {
                        strFemaledata.Append("0,");
                    }
                }
                strFemaledata.Remove(strFemaledata.Length - 1, 1);
            }
            else
            {
                for (int i = 1; i < 13; i++)
                {
                    strFemaledata.Append("0,");
                }
                strFemaledata.Remove(strFemaledata.Length - 1, 1);
            }
            ViewBag.fdata_year = strFemaledata;
        }

        //按日统计
        void MonthCount(int year, int month)
        {
            StringBuilder strcategory = new StringBuilder();
            //男性变量
            StringBuilder strMaledata = new StringBuilder();
            //女性变量
            StringBuilder strFemaledata = new StringBuilder();

            if (year == 0)
            {
                year = DateTime.Now.Year;
            }
            if (month == 0)
            {
                month = DateTime.Now.Month;
            }
            int days = DateTime.DaysInMonth(year, month);

            //查询男性数据并保存
            dynamic dList = OperateSession.SetContext.Database.SqlQueryForDynamic("select count(1) as p,DAY(RegDate) as m from tb_User where Sex='男' and MONTH(RegDate)=" + month + " and YEAR(RegDate)=" + year + " group by day(RegDate)");
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
                        strMaledata.Append("" + j + ",");
                    }
                    else
                    {
                        strcategory.Append("'" + i + "日',");
                        strMaledata.Append("0,");
                    }
                }
                strcategory.Remove(strcategory.Length - 1, 1);
                strMaledata.Remove(strMaledata.Length - 1, 1);
            }
            else
            {
                for (int i = 1; i < days + 1; i++)
                {
                    strcategory.Append("'" + i + "日',");
                    strMaledata.Append("0,");
                }
                strcategory.Remove(strcategory.Length - 1, 1);
                strMaledata.Remove(strMaledata.Length - 1, 1);
            }
            ViewBag.category_month = strcategory;
            ViewBag.mdata_month = strMaledata;

            //查询男性数据并保存
            dList = OperateSession.SetContext.Database.SqlQueryForDynamic("select count(1) as p,DAY(RegDate) as m from tb_User where Sex='女' and MONTH(RegDate)=" + month + " and YEAR(RegDate)=" + year + " group by day(RegDate)");
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
                        strFemaledata.Append("" + j + ",");
                    }
                    else
                    {
                        strFemaledata.Append("0,");
                    }
                }
                strFemaledata.Remove(strFemaledata.Length - 1, 1);
            }
            else
            {
                for (int i = 1; i < days + 1; i++)
                {
                    strFemaledata.Append("0,");
                }
                strFemaledata.Remove(strFemaledata.Length - 1, 1);
            }
            ViewBag.fdata_month = strFemaledata;
        }

        #endregion


        #region 6、[会员组管理]  初始化+Index()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GroupHome()
        {
            return View();
        }
        #endregion

        #region 7、[会员组管理] 获取数据+GroupData()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GroupData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询
                var where = PredicateBuilder.True<tb_UserGroup>();
                var list = OperateContext<tb_UserGroup>.SetServer.GetList(m => true, m => m.ID, true)
                    .Select(m => new
                    {
                        m.ID,
                        m.Name
                    });
                jsonM.Data = list;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 6、[会员组管理主页]  初始化+GroupIndex()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GroupIndex()
        {
            var classId = FytRequest.GetQueryInt("classId", 0);
            TempData["classId"] = classId;
            var model = OperateContext<tb_UserGroup>.SetServer.GetModel(m => m.ID == classId) ??
                        new tb_UserGroup() { Name = "", Summary = "" };
            return View(model);
        }
        #endregion

        #region 8、[会员组管理主页] 删除记录+GroupDeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GroupDeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var id = FytRequest.GetFormInt("id");
                OperateContext<tb_UserGroup>.SetServer.DeleteBy(m => m.ID == id);
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 9、[会员组管理] 修改模板加载视图+GroupModfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GroupModfiy(int id)
        {
            var model = OperateContext<tb_UserGroup>.SetServer.GetModel(m => m.ID == id) ?? new tb_UserGroup()
            {
                Grade = 0,
                UpgradeExp = 0,
                Amount = 0,
                Point = 0,
                Discount = 0
            };
            return View(model);
        }
        #endregion

        #region 10、[会员组管理] +GroupModfiy(tb_UserGroup model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupModfiy(tb_UserGroup model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                if (ModelState.IsValid)
                {
                    //添加或修改
                    OperateContext<tb_UserGroup>.SetServer.SaveOrUpdate(model, model.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return Json(jsonM);
        }
        #endregion


        #region 1、[邮件模板管理]  初始化+EmailIndex()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EmailIndex()
        {
            return View();
        }
        #endregion

        #region 2、[邮件模板管理] 获取数据+EmailData()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EmailData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询
                var where = PredicateBuilder.True<tb_UserEmail>();
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list =
                    OperateContext<tb_UserEmail>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage,
                        m => true, false, m => m.ID)
                        .Select(m => new
                        {
                            m.ID,
                            m.Title,
                            m.CallIndex,
                            m.MessType,
                            m.BusType,
                            m.Status
                        });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[邮件模板管理] 修改模板加载视图+EmailModfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EmailModfiy(int id)
        {
            var model = OperateContext<tb_UserEmail>.SetServer.GetModel(m => m.ID == id) ?? new tb_UserEmail()
            {
                Status = true,
                Content = ""
            };
            return View(model);
        }
        #endregion

        #region 4、[邮件模板管理] 修改模板加载视图+EmailModfiy(tb_UserEmail model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EmailModfiy(tb_UserEmail model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                if (ModelState.IsValid)
                {
                    //添加或修改
                    OperateContext<tb_UserEmail>.SetServer.SaveOrUpdate(model, model.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[邮件模板管理] 删除记录+EmailDeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EmailDeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    OperateContext<tb_UserEmail>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_UserEmail>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion


        #region 1、[会员注册配置管理]  初始化+ConfigIndex()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConfigIndex()
        {
            var model = MemberHelper.GetCacheMember();
            return View(model);
        }
        #endregion

        #region 2、[会员注册配置编辑]  初始化+ConfigModfiy()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ConfigModfiy(MemberConfig model)
        {
            SerializationHelper.Save(model, UtilsHelper.GetXmlMapPath(KeyHelper.FILE_USER_XML_CONFING));
            return Json(new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "修改成功！", BackUrl = "" });
        }
        #endregion


        #region 1、[消息日志管理]  初始化+LogIndex()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LogIndex()
        {
            return View();
        }
        #endregion

        #region 2、[消息日志管理] 获取数据+LogData()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key"); //关键字查询
                var where = PredicateBuilder.True<tb_UserLog>();
                var logType = FytRequest.GetFormStringEncode("logType");
                var userId = FytRequest.GetFormInt("userId");
                if (key != "")
                {
                    where = where.And(m => m.Title.Contains(key));
                    where = where.Or(m => m.Content.Contains(key));
                    where = where.Or(m => m.ToUser.Contains(key));
                    where = where.Or(m => m.BusType.Contains(key));
                }
                if (logType != "")
                {
                    where = where.And(m => m.LogType == logType);
                }
                if (userId != 0)
                {
                    where = where.And(m => m.UserId == userId);
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_UserLog>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.ID)
                    .Select(m => new
                    {
                        m.ID,
                        m.LogType,
                        m.BusType,
                        m.Title,
                        m.ToUser,
                        m.Status,
                        m.AddDate,
                        UserName = m.tb_User.LoginName
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[消息日志管理] 修改模板加载视图+LogDetail(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LogDetail(int id)
        {
            var model = OperateContext<tb_UserLog>.SetServer.GetModel(m => m.ID == id) ?? new tb_UserLog()
            {
                Status = true,
                Content = ""
            };
            return View(model);
        }
        #endregion

        #region 4、[消息日志管理] 删除记录+LogDeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogDeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    OperateContext<tb_UserLog>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_UserLog>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion


        #region 1、[发送邮件管理]  初始化+SendMail()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SendMail()
        {
            TempData["userId"] = FytRequest.GetQueryInt("userId", 0);
            var model = new tb_UserLog() { AddDate = DateTime.Now, BusType = "", Title = "" };
            return View(model);
        }
        #endregion

        #region 2、[发送邮件管理]  初始化+SendMail()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendMails(tb_UserLog model)
        {
            //var adModel = GetAdminInfo();
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "发送成功", BackUrl = "close" };
            var userId = Convert.ToInt32(TempData["userId"]);//收件人
            var userModel = OperateContext<tb_User>.SetServer.GetModel(m => m.ID == userId);
            if (userModel == null)
            {
                jsonM.Status = "err";
                jsonM.Msg = "错误，收件人找不到了，请重新选择发送吧！";
                return Json(jsonM);
            }
            model.UserId = 7;
            model.LogType = "Email";
            model.ToUserId = userId;
            model.ToUser = userModel.LoginName;
            model.Status = true;

            try
            {
                if (ModelState.IsValid)
                {
                    //添加或修改
                    OperateContext<tb_UserLog>.SetServer.SaveOrUpdate(model, model.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return Json(jsonM);
        }
        #endregion


        #region 1、查看用户下的所有信息+UserDetail()
        /// <summary>
        /// 查看用户下的所有信息主页
        /// </summary>
        /// <returns></returns>
        public ActionResult UserIndex()
        {

            return View();
        }
        #endregion

        #region 2、查看用户下的所有信息+UserDetail(int id)
        /// <summary>
        /// 查看用户下的所有信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UserDetail(int id)
        {
            var model = OperateContext<tb_User>.SetServer.GetModel(m => m.ID == id);
            return View(model);
        }
        #endregion

        #region 3、第三方登录授权登录列表+UserOauth()
        /// <summary>
        /// 查看用户下的所有信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UserOauth()
        {
            return View();
        }
        #endregion

        #region 4、第三方登录授权登录列表+UserOauthData()
        /// <summary>
        /// 查看用户下的所有信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UserOauthData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var userId = FytRequest.GetFormInt("userId");
                var list = OperateContext<tb_UserLoginOauth>.SetServer.GetList(m => m.UserId == userId, m => m.ID, false)
                    .Select(m => new
                    {
                        m.ID,
                        m.OauthKey,
                        m.OpenId
                    });
                jsonM.Data = list;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 5、[授权管理] 删除记录+OauthDeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OauthDeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    OperateContext<tb_UserLoginOauth>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_UserLoginOauth>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion


        #region 1、人工干预()
        /// <summary>
        /// 查看用户下的所有信息主页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserCustom()
        {
            return View();
        }
        #endregion

        #region 1、[人工干预] 保存信息()
        /// <summary>
        /// 查看用户下的所有信息主页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveCustom()
        {

            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                var userId = FytRequest.GetFormInt("userId");
                var t = FytRequest.GetFormString("type");
                var v = FytRequest.GetFormDecimal("returnValues", 0);
                var model = OperateContext<tb_User>.SetServer.GetModel(m => m.ID == userId);
                if (model != null)
                {
                    switch (t)
                    {
                        case "exp":
                            model.Exp = Convert.ToInt32(v); break;
                        case "point":
                            model.Point = Convert.ToInt32(v); break;
                        case "amount":
                            model.Amount = v; break;
                    }
                    OperateContext<tb_User>.SetServer.Update(model);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(MemberController), ex);
            }
            return Json(jsonM);
        }
        #endregion

    }
}
