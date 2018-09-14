using FytCms.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FytMsys.Common
{
    public class ApiUploadHelper
    {

        /// <summary>
        /// 上传图片的方法
        /// </summary>
        /// <param name="file">HttpPostedFile</param>
        /// <param name="small">输出参数，小图</param>
        /// <param name="type">HW=指定高宽缩放（可能变形）,W=指定宽，高按比例,H=指定高，宽按比例,Cut=指定高宽裁减（不变形） </param>
        /// <returns></returns>
        public static string SavePicture(HttpPostedFileBase file, int width, int height, string type)
        {
            string fileExt = UtilsHelper.GetFileExt(file.FileName); //文件扩展名，不含“.”
            string fileName = file.FileName.Substring(file.FileName.LastIndexOf(@"\", System.StringComparison.Ordinal) + 1); //取得原文件名
            string newFileName = UtilsHelper.GetRamCode() + "." + fileExt; //随机生成新的文件名
            string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名
            string upLoadPath = "/upload/headpic/"; //上传目录相对路径
            string fullUpLoadPath = UtilsHelper.GetMapPath(upLoadPath); //上传目录的物理路径
            string newFilePath = upLoadPath + newFileName; //上传后的路径
            string newThumbnailPath = upLoadPath + newThumbnailFileName; //上传后的缩略图路径
            string newSmallFileName = "small_" + newFileName; //随机生成缩略图文件名
            string newSmallPath = upLoadPath + newSmallFileName;
            if (!Directory.Exists(fullUpLoadPath))
            {
                Directory.CreateDirectory(fullUpLoadPath);
            }
            file.SaveAs(fullUpLoadPath + newFileName);
            //生成新的缩略图保存到服务端
            try
            {
                //生成小图 高度自定
                Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newSmallFileName, width, height, "W");
                //删除已经上传的大图
                File.Delete(fullUpLoadPath + newFileName);
            }
            catch
            {

            }
            return newSmallPath;
        }

        /// <summary>
        /// [社区-发现]上传图片的方法
        /// </summary>
        /// <param name="file">HttpPostedFile</param>
        /// <param name="small">输出参数，小图</param>
        /// <returns></returns>
        public static string SavePicSmall(HttpPostedFileBase file, out string small)
        {
            string fileExt = UtilsHelper.GetFileExt(file.FileName); //文件扩展名，不含“.”
            string fileName = file.FileName.Substring(file.FileName.LastIndexOf(@"\", System.StringComparison.Ordinal) + 1); //取得原文件名
            string newFileName = UtilsHelper.GetRamCode() + "." + fileExt; //随机生成新的文件名
            string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名
            string upLoadPath = "/upload/image/say/"; //上传目录相对路径
            string fullUpLoadPath = UtilsHelper.GetMapPath(upLoadPath); //上传目录的物理路径
            string newFilePath = upLoadPath + newFileName; //上传后的路径
            string newThumbnailPath = upLoadPath + newThumbnailFileName; //上传后的缩略图路径
            string newSmallFileName = "small_" + newFileName; //随机生成缩略图文件名
            string newSmallPath = upLoadPath + newSmallFileName;
            if (!Directory.Exists(fullUpLoadPath))
            {
                Directory.CreateDirectory(fullUpLoadPath);
            }
            file.SaveAs(fullUpLoadPath + newFileName);
            //生成新的缩略图保存到服务端
            try
            {
                //生成大图 宽度1000
                Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newThumbnailFileName, 1000, 0, "W");
                //生成小图 高度100
                Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newSmallFileName, 100, 100, "Cut");
                //删除已经上传的大图
                File.Delete(fullUpLoadPath + newFileName);
            }
            catch
            {

            }
            small = newSmallPath;
            return newThumbnailPath;
        }

        /// <summary>
        /// 上传作品图片的方法
        /// </summary>
        /// <param name="file">HttpPostedFile</param>
        /// <param name="small">输出参数，小图</param>
        /// <returns></returns>
        public static string SaveCasePicSmall(HttpPostedFileBase file, out string small)
        {
            string fileExt = UtilsHelper.GetFileExt(file.FileName); //文件扩展名，不含“.”
            string fileName = file.FileName.Substring(file.FileName.LastIndexOf(@"\", System.StringComparison.Ordinal) + 1); //取得原文件名
            string newFileName = UtilsHelper.GetRamCode() + "." + fileExt; //随机生成新的文件名
            string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名
            string upLoadPath = "/upload/case/"; //上传目录相对路径
            string fullUpLoadPath = UtilsHelper.GetMapPath(upLoadPath); //上传目录的物理路径
            string newFilePath = upLoadPath + newFileName; //上传后的路径
            string newThumbnailPath = upLoadPath + newThumbnailFileName; //上传后的缩略图路径
            string newSmallFileName = "small_" + newFileName; //随机生成缩略图文件名
            string newSmallPath = upLoadPath + newSmallFileName;
            if (!Directory.Exists(fullUpLoadPath))
            {
                Directory.CreateDirectory(fullUpLoadPath);
            }
            file.SaveAs(fullUpLoadPath + newFileName);
            //生成新的缩略图保存到服务端
            try
            {
                //生成大图 宽度1000
                Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newThumbnailFileName, 500, 0, "W");
                //生成小图 高度100
                Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newSmallFileName, 260, 260, "Cut");
                //删除已经上传的大图
                File.Delete(fullUpLoadPath + newFileName);
            }
            catch
            {

            }
            small = newSmallPath;
            return newThumbnailPath;
        }

    }
}
