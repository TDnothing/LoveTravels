using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Security.Cryptography;

/**
 * 仅供UPOP联机对账文件下载使用
 * */
namespace upacp_sdk_net.com.unionpay.sdk
{
    public class DemoUtil
    {

        /// <summary>
        /// 将customerInfo转化为string，按旧规范组成
        /// </summary>
        /// <param name="customerInfo">Dictionary的customerInfo</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        /// <returns>string类型结果</returns>
        public static string GetCustomerInfoStr(Dictionary<string, string> customerInfo, Encoding encoding)
        {
            if (customerInfo == null || customerInfo.Count == 0)
            {
                return "";
            }
            string customerInfoStr = "{" + SDKUtil.CreateLinkString(customerInfo, false, false) + "}";
            return SecurityUtil.EncodeBase64(customerInfoStr, encoding);
        }

        /// <summary>
        /// 将customerInfo转化为string，按新规范组成
        /// </summary>
        /// <param name="customerInfo">Dictionary的customerInfo</param>
        /// <param name="encoding">编码</param>
        /// <returns>string类型结果</returns>
        public static string GetCustomerInfoStrNew(Dictionary<string, string> customerInfo, Encoding encoding)
        {
            if (customerInfo == null || customerInfo.Count == 0) 
            {
                return "";
            }
            Dictionary<string, string> encryptedInfo = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in customerInfo)
            {
                if (pair.Key == "phoneNo" || pair.Key == "cvn2" || pair.Key == "expired")
                {
                    encryptedInfo[pair.Key] = pair.Value;
                }
            }
            customerInfo.Remove("phoneNo");
            customerInfo.Remove("cvn2");
            customerInfo.Remove("expired");
            if (encryptedInfo.Count != 0)
            {
                string encryptedInfoStr = SDKUtil.CreateLinkString(encryptedInfo, false, false);
                encryptedInfoStr = SDKUtil.EncryptData(encryptedInfoStr, encoding);
                customerInfo["encryptedInfo"] = encryptedInfoStr;
            }
            string customerInfoStr = "{" + SDKUtil.CreateLinkString(customerInfo, false, false) + "}";
            return SecurityUtil.EncodeBase64(customerInfoStr, encoding);
        }

        /// <summary>
        /// 得到用于打印页面的html
        /// </summary>
        /// <param name="url"></param>
        /// <param name="req"></param>
        /// <param name="resp"></param>
        public static string GetPrintResult(string url, Dictionary<string, string> req, string resp)
        {
            string result = "=============<br>\n";
            result = result + "地址：" + url + "<br>\n";
            result = result + "请求：" + System.Web.HttpContext.Current.Server.HtmlEncode(SDKUtil.CreateLinkString(req, false, true)).Replace("\n", "<br>\n") + "<br>\n";
            result = result + "应答：" + System.Web.HttpContext.Current.Server.HtmlEncode(resp).Replace("\n", "<br>\n") + "<br>\n";
            result = result + "=============<br>\n";
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encoding"></param>
        public static string GetFileContent(string filePath, Encoding encoding)
        {

            string fileContent;
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                fileContent = sr.ReadToEnd();
                byte[] fileContentByte = SecurityUtil.deflater(encoding.GetBytes(fileContent));
                fileContent = Convert.ToBase64String(fileContentByte);
                sr.Close();
                fs.Close();
                return fileContent;
            }
            else
            {
                //    Response.Write("文件不存在");
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="fileName"></param>
        /// <param name="savePath"></param>
        public static bool DealWithFileContent(string fileContent, string fileName, string savePath)
        {
            if (!string.IsNullOrEmpty(fileContent))
            {
                //Base64解码
                byte[] dBase64Byte = Convert.FromBase64String(fileContent);
                //解压缩
                byte[] fileByte = SecurityUtil.inflater(dBase64Byte);

                //保存
                string path = System.IO.Path.Combine(savePath, fileName);
                System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create);
                fs.Write(fileByte, 0, fileByte.Length);
                fs.Close();
                fs.Dispose();

                return true;
            }
            else
            {
                return false;
            }
        }

    }
}