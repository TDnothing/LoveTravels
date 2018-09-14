using System;
using System.Collections.Generic;
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
    /// 文件管理器
    /// </summary>
    public class FileMiamController:BaseController
    {
        /// <summary>
        /// 加载视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获得文件列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFileData()
        {
            string path = FytRequest.GetFormString("path");
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                jsonM.Data = ConvertHelper<FileModel>.ConvertToList(FileHelper.GetFileTable(Server.MapPath(path)));
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "上传过程中发生错误，消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FileMiamController), ex);
            }
            return Json(jsonM);
        }

        /// <summary>
        /// 单个文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SignUpFile()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() {Status = "y",Msg = "success"};
            try
            {
                HttpPostedFileBase upfile = Request.Files["fileUp"]; //取得上传文件
                //上传类型 0=图片 1=文件 2=视频
                var isImg = FytRequest.GetQueryInt("isImg");
                //是否缩略图  0=不压缩  1=压缩
                var isThum = FytRequest.GetQueryInt("isThum",0);
                //是否添加水印
                var isWater = FytRequest.GetQueryInt("isWater",0);
                if (upfile==null)
                {
                    jsonM.Status = "err";
                    jsonM.Msg = "请选择要上传文件！";
                    return Json(jsonM);
                }
                var jsFile = new UpLoadHelper().FileSaveAs(upfile, Convert.ToBoolean(isThum), Convert.ToBoolean(isWater),isImg);
                jsonM.Status = jsFile.Status;
                jsonM.Msg = jsFile.Msg;
                jsonM.Data = jsFile.ImgUrl;
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "上传过程中发生错误，消息："+ex.Message;
                LogHelper.WriteLog(typeof(FileMiamController),ex);
            }
            return Json(jsonM);
        }

        /// <summary>
        /// 删除文件或文件夹
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteBy()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                var path = FytRequest.GetFormString("path");
                var isFile = FytRequest.GetFormInt("isfile");
                if (isFile == 0)
                {
                    //删除文件夹
                    FileHelper.ClearDirectory(Server.MapPath(path));
                }
                else
                {
                    //删除文件
                    FileHelper.DeleteFile(Server.MapPath(path));
                }
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除过程中发生错误，消息：" + ex.Message;
                LogHelper.WriteLog(typeof(FileMiamController), ex);
            }
            return Json(jsonM);
        }

        /// <summary>
        /// 加载文件管理主页
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexDoc()
        {
            return View();
        }

        /// <summary>
        /// 获得项目根目录的文件夹名称，tree模式显示
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDocTree()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            try
            {
                jsonM.Data = FileHelper.GetDirs(Server.MapPath("/"));
            }
            catch (Exception ex)
            {
                jsonM.Status = "err";
                jsonM.Msg = "删除数据发生错误！ 消息：" + ex.Message;
            }
            return Json(jsonM);
        }

        /// <summary>
        /// 根据指定目朗，加载列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DocList()
        {
            var path = DESEncrypt.Decrypt(FytRequest.GetQueryString("path"));
            ViewBag.list=ConvertHelper<FileModel>.ConvertToList(FileHelper.GetFileTable(path));
            return View();
        }

        /// <summary>
        /// 根据文件加载对应的内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CodeIndex()
        {
            var path = DESEncrypt.Decrypt(FytRequest.GetQueryString("path"));
            var filename = FytRequest.GetQueryString("filename");
            var ext = FytRequest.GetQueryString("ext").ToLower();
            var type = "";
            if (ext == ".txt" || ext == ".html" || ext == ".cshtml" || ext == ".js" || ext == ".css" || ext == ".aspx" || ext == ".php" || ext == ".cs")
            {
                var sr = new StreamReader(path+"/"+filename, Encoding.GetEncoding("utf-8"));
                ViewBag.content = sr.ReadToEnd();
                sr.Close();
                type = "html";
            }
            if (ext == ".jpg" || ext == ".gif" || ext == ".png" || ext == ".bmp" || ext == ".jpeg")
            {
                type = "图片";
                ViewBag.content = "/"+path.Replace(Server.MapPath("/"),"") + "/" + filename;
            }
            if (ext == ".swf" || ext == ".pdf" || ext == ".rar" || ext == ".zip" || ext == ".mp3" || ext == ".mp4" || ext == ".flv" || ext == ".doc"
                || ext == ".docx" || ext == ".xls" || ext == ".xlsx" || ext == ".ppt" || ext == ".pptx")
            {
                type = "下载";
                ViewBag.content = "/" + path.Replace(Server.MapPath("/"), "") + "/" + filename;
            }
            ViewBag.type = type;
            return View();
        }

        /// <summary>
        /// 保存文件信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CodeIndex(string code)
        {
            var path = FytRequest.GetFormString("path");
            var npath = DESEncrypt.Decrypt(path);
            var filename = FytRequest.GetFormString("filename");
            var ext = FytRequest.GetFormString("ext").ToLower();
            var content = FytRequest.GetFormString("code");
            try
            {
                if (!Directory.Exists(npath))
                {
                    Directory.CreateDirectory(npath);
                }
                string fname = npath + "/" + filename;
                if (!System.IO.File.Exists(fname))
                {
                    FileStream fs = System.IO.File.Create(fname);
                    fs.Close();
                }
                var sw = new StreamWriter(fname, false, System.Text.Encoding.GetEncoding("utf-8"));
                sw.WriteLine(content);
                sw.Close();
                sw.Dispose();
            }
            catch
            {
            }
            return RedirectToAction("CodeIndex", new { path = path, filename = filename,ext="."+ext});
        }

    }
}
