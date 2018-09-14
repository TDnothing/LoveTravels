using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FytMsys.Common;
using FytMsys.Helper;

namespace FytMsys.Logic.Admin
{
    /// <summary>
    /// 多图片上传
    /// </summary>
    public class WebUploadController:BaseController
    {
        public ActionResult FileUpload()
        {
            var jsonM = new JsonHelper.JsonAjaxModel() { Status = "y", Msg = "success" };
            var hfc = Request.Files;
            if (hfc.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    string smallPic;
                    HttpPostedFileBase hpf = Request.Files[i];
                    var jsFile = new UpLoadHelper().FileSaveAs(hpf,true,false,0);
                    jsonM.Status = jsFile.Status;
                    jsonM.Msg = jsFile.Msg;
                    jsonM.Data = jsFile.ImgUrl;
                }
            }
            return Json(jsonM);
        }
    }
}
