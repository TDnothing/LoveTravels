using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Domain.ViewModel;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Logic.Admin
{
    /// <summary>
    /// 数据库管理
    /// </summary>
    public class DataBaseController:BaseController
    {
        #region 1、加载数据库备份视图 +DataBackup()
        /// <summary>
        /// 加载数据库备份视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DataBackup()
        {
            ViewBag.dataName=OperateSession.SetContext.Database.Connection.Database;
            ViewBag.dataUrl=Server.MapPath("/backup/") + DateTime.Now.ToString("yyyyMMddHHmmss");
            dynamic t = OperateSession.SetContext.Database.SqlQueryForDynamic("select name as text,name as value from sysobjects where xtype='U' order by text", null);
            var selectList = new List<SelectListItem>();
            foreach (dynamic o in t)
            {
                selectList.Add(new SelectListItem(){Text = o.text,Value = o.value});
            }
            selectList.Insert(0, new SelectListItem() { Text = "--请选择-", Value = "0" });
            ViewBag.selectList = selectList.AsEnumerable();
            return View();
        } 
        #endregion

        #region 2、备份数据库Bak格式 +BAKDataBackup()
        /// <summary>
        /// 加载数据库备份视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BAKDataBackup()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "备份数据成功！",BackUrl = ""};
            var con = new SqlConnection(ConfigHelper.GetConfigString("ConnectionString"));
            try
            {
                var url = FytRequest.GetFormString("BakUrl");
                var sqlStr = "backup database " + FytRequest.GetFormString("BakName") + " to disk='" + url + ".bak'";
                //object[] para = {};
                //OperateSession.SetContext.Database.ExecuteSqlCommandAsync(sqlStr, para);
                con.Open();
                if (System.IO.File.Exists(url))
                {
                    jsonM.Msg = "此文件已存在，请从新输入！";
                    jsonM.Status = "err";
                }
                var com = new SqlCommand(sqlStr, con);
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                jsonM.Msg = "备份数据失败！ 消息："+ex.Message;
                jsonM.Status = "err";
            }
            finally
            {
                con.Close();
            }
            return Json(jsonM);
        }
        #endregion

        #region 3、备份数据Sql格式 +SqlDataBackup()
        /// <summary>
        /// 加载数据库备份视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SqlDataBackup()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "备份数据成功！", BackUrl = "" };
            try
            {
                if (FytRequest.GetFormInt("bfsql") == 0)
                {
                    var thread = new System.Threading.Thread(new System.Threading.ThreadStart(DataBackHelper.BackUpAll));
                    thread.Start();

                    if (thread.ThreadState != System.Threading.ThreadState.Running)
                    {
                        thread.Abort();
                        DataBackHelper.BackUpAll();
                    }
                    else
                    {
                        jsonM.Msg = "备份任务正在后台处理，请稍后到数据库恢复菜单中查看";
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                else
                {
                    string tablename = UtilsHelper.GetString(Request["tablename"], String.Empty);
                    if (tablename.Length > 0)
                    {
                        var thread =
                            new System.Threading.Thread(
                                new System.Threading.ParameterizedThreadStart(DataBackHelper.BackUpSingle))
                            {
                                Priority = System.Threading.ThreadPriority.Highest
                            };
                        thread.Start(tablename);



                        if (thread.ThreadState != System.Threading.ThreadState.Running)
                        {
                            thread.Abort();
                            DataBackHelper.BackUpSingle(tablename);
                        }
                        else
                        {
                            jsonM.Msg = "备份任务正在后台处理，请稍后到数据库恢复菜单中查看";
                            System.Threading.Thread.Sleep(1000);
                        }

                    }
                    else
                    {
                        jsonM.Msg = "数据表选择错误";
                        jsonM.Status = "err";
                    }
                }
            }
            catch (Exception ex)
            {
                jsonM.Msg = "备份数据失败！ 消息：" + ex.Message;
                jsonM.Status = "err";
            }
            return Json(jsonM);
        }
        #endregion

        #region 4、列出所有备份的文件 +ListFile()
        /// <summary>
        /// 加载数据库备份视图
        /// </summary>
        /// <returns></returns>
        public ActionResult ListFile()
        {
            return View();
        }
        #endregion

        #region 5、列出所有备份的文件 +ListFile()
        /// <summary>
        /// 加载数据库备份视图
        /// </summary>
        /// <returns></returns>
        public ActionResult GetListFile()
        {
            
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                string path = "/backup";
                var list = ConvertHelper<FileModel>.ConvertToList(FileHelper.GetFileTable(Server.MapPath(path)));
                jsonM.Data = list;
            }
            catch (Exception ex)
            {
                jsonM.Msg = "备份数据失败！ 消息：" + ex.Message;
                jsonM.Status = "err";
            }
            return Json(jsonM);
        }
        #endregion

        #region 6、删除选中文件 +DeleteFile()
        /// <summary>
        /// 加载数据库备份视图
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteFile()
        {

            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                string idlist = FytRequest.GetFormString("id");
                string[] str = idlist.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string t in str)
                {
                    FileHelper.DeleteFile(Server.MapPath("/backup/") + t);
                }
            }
            catch (Exception ex)
            {
                jsonM.Msg = "删除数据失败！ 消息：" + ex.Message;
                jsonM.Status = "err";
            }
            return Json(jsonM);
        }
        #endregion

        #region 7、下载选中文件 +DeleteFile()
        /// <summary>
        /// 加载数据库备份视图
        /// </summary>
        /// <returns></returns>
        public void FileDown()
        {

            string spath = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["p"]))
            {
                spath = Request.QueryString["p"];
            }
            var file = new FileInfo(Server.MapPath("/backup/") + spath);//路径
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
            Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(file.Name)); //解决中文文件名乱码    
            Response.AddHeader("Content-length", file.Length.ToString(CultureInfo.InvariantCulture));
            Response.ContentType = "application/pdf";
            Response.WriteFile(file.FullName);
            Response.End();
        }
        #endregion
    }
}
