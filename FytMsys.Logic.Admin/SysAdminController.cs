using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Entity;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Logic.Admin
{
    public class SysAdminController:BaseController
    {
        #region 1、[系统角色管理]  初始化+TempIndex()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RoleIndex()
        {
            return View();
        }
        #endregion

        #region 2、[系统角色管理] 获取数据+RoleData()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoleData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key");
                var where = PredicateBuilder.True<tb_AdminRole>();
                if (key != "")
                {
                    where = where.And(m => m.RoleNumber.Contains(key));
                    where = where.Or(m => m.RoleName.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_AdminRole>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where,false, m => m.Sort)
                    .Select(role => new { role.ID, role.RoleNumber, role.RoleName, role.IsLock, role.IsEdit, role.IsDel, role.Summary, role.Sort, role.AddDate });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysAdminController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[系统角色管理] 修改模板加载视图+RoleModfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RoleModfiy(int id)
        {
            var model = OperateContext<tb_AdminRole>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_AdminRole() { IsLock = true, IsEdit = true, IsDel = true, AddDate = DateTime.Now, RoleNumber = UtilsHelper.Number(6, false) };
            }
            return View(model);
        }
        #endregion

        #region 4、[系统角色管理] 修改模板加载视图+RoleModfiy(tb_SysTemp model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoleModfiy(tb_AdminRole model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                if (ModelState.IsValid)
                {
                    //添加或修改
                    OperateContext<tb_AdminRole>.SetServer.SaveOrUpdate(model, model.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysAdminController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[系统角色管理] 删除记录+RoleDeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoleDeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    OperateContext<tb_AdminRole>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_AdminRole>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysAdminController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion


        #region 1、[系统管理员管理]  初始化+TempIndex()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AdminIndex()
        {
            return View();
        }
        #endregion

        #region 2、[系统管理员管理] 获取数据+RoleData()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AdminData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key");
                var where = PredicateBuilder.True<tb_Admin>();
                if (key != "")
                {
                    where = where.And(m => m.LoginName.Contains(key));
                    where = where.Or(m => m.RealName.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_Admin>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, false, m => m.AddDate)
                    .Select(admin => new
                    {
                        admin.ID,admin.LoginName,admin.RealName,admin.Sex,admin.Mobile,admin.Email,admin.tb_AdminRole.RoleName,admin.Status
                        ,admin.LastLoginTime
                    });
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysAdminController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[系统管理员管理] 修改模板加载视图+AdminModfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AdminModfiy(int id)
        {
            var model = OperateContext<tb_Admin>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_Admin() { AddDate = DateTime.Now,Sex = "男"};
            }
            var sList = OperateContext<tb_AdminRole>.SetServer.GetList(m => true, m => m.Sort, false).ToList();
            var selectList = sList.Select(p => new SelectListItem
            {
                Text =p.RoleName,
                Value = p.ID.ToString(CultureInfo.InvariantCulture)
            }).ToList();
            selectList.Insert(0, new SelectListItem() { Text = "--请选择-", Value = "0" });
            ViewBag.selectList = selectList.AsEnumerable();
            return View(model);
        }
        #endregion

        #region 4、[系统管理员管理] 修改模板加载视图+AdminModfiy(tb_Admin model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AdminModfiy(tb_Admin model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                if (ModelState.IsValid)
                {
                    //判断密码修改，是否加密
                    if (model.ID == 0 || model.LoginPwd != "111111")
                        model.LoginPwd = UtilsHelper.MD5(model.LoginPwd, true);
                    else
                        model.LoginPwd = FytRequest.GetFormString("oldpwd");
                    //添加或修改
                    OperateContext<tb_Admin>.SetServer.SaveOrUpdate(model, model.ID);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysAdminController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[系统管理员管理] 删除记录+AdminDeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AdminDeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    OperateContext<tb_Admin>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_Admin>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysAdminController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion


        #region 1、[系统菜单管理]  初始化+MenuIndex()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MenuIndex()
        {
            return View();
        }
        #endregion

        #region 2、[系统菜单管理] 获取数据+MenuData()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MenuData()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var key = FytRequest.GetFormStringEncode("key");
                var where = PredicateBuilder.True<tb_AdminMenu>();
                if (key != "")
                {
                    where = where.And(m => m.Name.Contains(key));
                    where = where.Or(m => m.ActionName.Contains(key));
                }
                int pageSize = FytRequest.GetFormInt("pageSize"),
                    pageIndex = FytRequest.GetFormInt("pageIndex"),
                    rows = 0, totalPage = 0;
                var list = OperateContext<tb_AdminMenu>.SetServer.GetPageList(pageIndex, pageSize, out rows, out totalPage, where, true, m => m.ID);
                jsonM.Data = list;
                jsonM.PageTotal = totalPage;
                jsonM.PageRows = rows;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "获取数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysAdminController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 3、[系统菜单管理] 修改模板加载视图+MenuModfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MenuModfiy(int id)
        {
            var model = OperateContext<tb_AdminMenu>.SetServer.GetModel(m => m.ID == id);
            if (model == null)
            {
                model = new tb_AdminMenu();
            }
            var sList = OperateContext<tb_AdminMenu>.SetServer.GetList(m => m.ParentID == 0, m => m.Sort, true).ToList();
            var selectList = sList.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.NodeID.ToString(CultureInfo.InvariantCulture)
            }).ToList();
            selectList.Insert(0, new SelectListItem() { Text = "父级", Value = "0" });
            ViewBag.selectList = selectList.AsEnumerable();
            return View(model);
        }
        #endregion

        #region 4、[系统菜单管理] 修改模板加载视图+MenuModfiy(tb_AdminMenu model)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MenuModfiy(tb_AdminMenu model)
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "close" };
            try
            {
                if (ModelState.IsValid)
                {
                    //添加或修改
                    OperateContext<tb_AdminMenu>.SetServer.SaveOrUpdate(model, model.ID);
                    SaveLogs("【系统菜单管理】修改或添加一条系统菜单", 0);
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysAdminController), ex);
            }
            return Json(jsonM);
        }
        #endregion

        #region 5、[系统菜单管理] 删除记录+MenuDeleteBy()
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MenuDeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var ids = FytRequest.GetFormString("id");
                if (!ids.Contains(","))
                {
                    var id = Convert.ToInt32(ids);
                    OperateContext<tb_AdminMenu>.SetServer.DeleteBy(m => m.ID == id);
                }
                else
                {
                    List<int> result = new List<string>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ConvertAll(int.Parse);
                    OperateContext<tb_AdminMenu>.SetServer.DeleteBy(m => result.Contains(m.ID));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysAdminController), ex);
            }
            return MyJson(jsonM, "");
        }
        #endregion

        #region 6、选择Icon   +MenuIcon()
        /// <summary>
        /// 选择Icon
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuIcon()
        {
            return View();
        } 
        #endregion


        #region 1、[系统权限管理]  初始化+PermissionIndex()
        /// <summary>
        /// 系统角色管理 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PermissionIndex()
        {
            ViewBag.list = OperateContext<tb_AdminRole>.SetServer.GetList(s => true, s => s.Sort, false).ToList();
            //查询系统菜单
            ViewBag.menuList = GetTreeList();
            return View();
        }
        #endregion

        #region 2、[系统权限管理] 获取数据+PermissionData()
        /// <summary>
        /// 系统角色管理获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PermissionData()
        {
            var roldId = FytRequest.GetFormInt("roldId", 0);
            //根据角色ID查询权限表中对应的菜单ID，返回Json
            var jsonList = OperateContext<tb_AdminPermission>.SetServer.GetList(m => m.RoleID == roldId,m=>m.ID,true);
            return Json(jsonList);
        }

        /// <summary>
        /// 获得角色列表，根据级别进行分类展示
        /// </summary>
        /// <returns></returns>
        public List<tb_AdminMenu> GetTreeList()
        {
            var oldList = OperateContext<tb_AdminMenu>.SetServer.GetList(p => true,p=>p.ID,true).ToList();
            var sList = new List<tb_AdminMenu>();
            Recursion(oldList, sList, 0);
            return sList;
        }
        /// <summary>  
        /// 递归菜单列表
        /// </summary>  
        private void Recursion(List<tb_AdminMenu> oldList, List<tb_AdminMenu> newList, int Pid)
        {
            foreach (var item in from c in oldList where c.ParentID == Pid select c)
            {
                var model = item;
                newList.Add(model);
                if(Pid==item.ParentID)
                    Recursion(oldList, newList, item.NodeID);
                else
                    Recursion(oldList, newList, item.ParentID);
            }
        }  
        #endregion

        #region 3、[系统权限管理] 修改模板加载视图+MenuModfiy(int id)
        /// <summary>
        /// 修改模板加载视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PermissionModfiy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "保存成功！", BackUrl = "" };
            try
            {
                var roleId = FytRequest.GetFormInt("roleId", 0);
                var menuList = FytRequest.GetFormString("cbk-menu");
                if (menuList == "")
                {
                    jsonM.Status = "err";
                    jsonM.Msg = "请选择对应的角色！";
                }
                else
                {
                    //根据角色删除权限
                    OperateContext<tb_AdminPermission>.SetServer.DeleteBy(p => p.RoleID == roleId);
                    //循环选中的权限，并保存
                    foreach (
                        var model in
                            menuList.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => new tb_AdminPermission()
                                {
                                    RoleID = roleId,
                                    NodeID = int.Parse(s),
                                    IsDel = false
                                }))
                    {
                        OperateContext<tb_AdminPermission>.SetServer.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "发生错误！消息：" + ex.Message;
                LogHelper.WriteLog(typeof(SysAdminController), ex);
            }
            return Json(jsonM);
        }
        #endregion
    }
}
