using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FytMsys.Common;

namespace FytCms.uploadify
{
    /// <summary>
    /// UploadHandler 的摘要说明
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpPostedFile file = context.Request.Files["Filedata"];
            string uploadPath =
                HttpContext.Current.Server.MapPath("/upload/file/") + "\\";

            if (file != null)
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string fileExt = UtilsHelper.GetFileExt(file.FileName); //文件扩展名，不含“.”
                string fileName = file.FileName.Substring(file.FileName.LastIndexOf(@"\") + 1); //取得原文件名
                string newFileName = UtilsHelper.GetRamCode() + "." + fileExt; //随机生成新的文件名
                file.SaveAs(uploadPath + DateTime.Now.ToString("yyyyMMdd") +"/"+ newFileName);
                //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                context.Response.Write("/upload/file/" + DateTime.Now.ToString("yyyyMMdd") +"/"+ newFileName);
            }
            else
            {
                context.Response.Write("0");
            }  
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}