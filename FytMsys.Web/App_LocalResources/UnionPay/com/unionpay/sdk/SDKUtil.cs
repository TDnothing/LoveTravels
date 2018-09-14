using System.Collections.Generic;
using System.Text;
using System;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace upacp_sdk_net.com.unionpay.sdk
{
    public class SDKUtil
    {

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="dataStr"></param>
        /// <param name="encoder"></param>
        /// <returns></returns>
        public static bool Sign(Dictionary<string, string> data, Encoding encoding)
        {
            //设置签名证书序列号 ？

            data["certId"] = CertUtil.GetSignCertId();

            //将Dictionary信息转换成key1=value1&key2=value2的形式
            string stringData = CreateLinkString(data, true, false);

            string stringSign = null;

            byte[] signDigest = SecurityUtil.Sha1X16(stringData, encoding);

            string stringSignDigest = BitConverter.ToString(signDigest).Replace("-", "").ToLower();

            byte[] byteSign = SecurityUtil.SignBySoft(CertUtil.GetSignProviderFromPfx(), encoding.GetBytes(stringSignDigest));

            stringSign = Convert.ToBase64String(byteSign);

            //设置签名域值
            data["signature"] = stringSign;

            return true;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoder"></param>
        /// <returns></returns>
        public static bool Validate(Dictionary<string, string> data, Encoding encoding)
        {
            //获取签名
            string signValue = data["signature"];
            byte[] signByte = Convert.FromBase64String(signValue);
            data.Remove("signature");
            string stringData = CreateLinkString(data, true, false);
            byte[] signDigest = SecurityUtil.Sha1X16(stringData, encoding);
            string stringSignDigest = BitConverter.ToString(signDigest).Replace("-", "").ToLower();
            RSACryptoServiceProvider provider = CertUtil.GetValidateProviderFromPath(data["certId"]);
            if (null == provider)
            {
                return false;
            }
            return SecurityUtil.ValidateBySoft(provider, signByte, encoding.GetBytes(stringSignDigest));
        }

        /// <summary>
        /// 将字符串key1=value1&key2=value2转换为Dictionary数据结构
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Dictionary<string, string> CoverStringToDictionary(string data)
        {
            if (null == data || 0 == data.Length)
            {
                return null;
            }
            string[] arrray = data.Split(new char[] { '&' });
            Dictionary<string, string> res = new Dictionary<string, string>();
            foreach (string s in arrray)
            {
                int n = s.IndexOf("=");
                string key = s.Substring(0, n);
                string value = s.Substring(n + 1);
                Console.WriteLine(key + "=" + value);
                res.Add(key, value);
            }
            return res;
        }
        

        public static string CreateAutoSubmitForm(string url, Dictionary<string, string> data, Encoding encoding)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendFormat("<meta http-equiv=\"Content-Type\" content=\"text/html; charset={0}\" />", encoding);
            html.AppendLine("</head>");
            html.AppendLine("<body onload=\"OnLoadSubmit();\">");
            html.AppendFormat("<form id=\"pay_form\" action=\"{0}\" method=\"post\">", url);
            foreach (KeyValuePair<string, string> kvp in data)
            {
                html.AppendFormat("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", kvp.Key, kvp.Value);
            }
            html.AppendLine("</form>");
            html.AppendLine("<script type=\"text/javascript\">");
            html.AppendLine("<!--");
            html.AppendLine("function OnLoadSubmit()");
            html.AppendLine("{");
            html.AppendLine("document.getElementById(\"pay_form\").submit();");
            html.AppendLine("}");
            html.AppendLine("//-->");
            html.AppendLine("</script>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            return html.ToString();
        }
        

        /**
        /// <summary>
        /// 将Dictionary内容排序后输出为键值对字符串,供打印报文使用
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string PrintDictionaryToString(Dictionary<string, string> data)
        {
            //如果不加stringComparer.Ordinal，排序方式和java treemap有差异
            SortedDictionary<string, string> treeMap = new SortedDictionary<string, string>(StringComparer.Ordinal); 

            foreach (KeyValuePair<string, string> kvp in data)
            {
                treeMap.Add(kvp.Key, kvp.Value);
            }

            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> element in treeMap)
            {
                builder.Append(element.Key + "=" + element.Value + "&");
            }

            return builder.ToString().Substring(0, builder.Length - 1);
        }
        */

        /// <summary>
        /// 把请求要素按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="para">请求要素</param>
        /// <param name="sort">是否需要根据key值作升序排列</param>
        /// <param name="encode">是否需要URL编码</param>
        /// <returns>拼接成的字符串</returns>
        public static String CreateLinkString(Dictionary<String, String> para, bool sort, bool encode)
        {
            List<String> list = new List<String>(para.Keys);

            if (sort)
                list.Sort(StringComparer.Ordinal);

            StringBuilder sb = new StringBuilder();
            foreach (String key in list)
            {
                String value = para[key];
                if (encode && value != null)
                {
                    try
                    {
                        value = HttpUtility.UrlEncode(value);
                    }
                    catch (Exception ex)
                    {
                        //LogError(ex);
                        return "#ERROR: HttpUtility.UrlEncode Error!" + ex.Message;
                    }
                }

                sb.Append(key).Append("=").Append(value).Append("&");

            }

            return sb.Remove(sb.Length - 1, 1).ToString();
        }

 

        /// <summary>
        /// pinblock 16进制计算
        /// </summary>
        /// <param name="encoder"></param>
        /// <returns></returns>

        public static string PrintHexString(byte[] b)
        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < b.Length; i++)
            {
                string hex = Convert.ToString(b[i] & 0xFF, 16);
                if (hex.Length == 1)
                {
                    hex = '0' + hex;
                }
                sb.Append("0x");
                sb.Append(hex + " ");
            }
            sb.Append("");
            return sb.ToString();
        }



        /// <summary>
        /// 密码pinblock加密
        /// </summary>
        /// <param name="card"></param>
        /// <param name="pwd"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string EncryptPin(string card, string pwd)
        {

            /** 生成PIN Block **/
            byte[] pinBlock = SecurityUtil.pin2PinBlockWithCardNO(pwd, card);
            PrintHexString(pinBlock);


            X509Certificate2 pc = new X509Certificate2(SDKConfig.EncryptCert);


            RSACryptoServiceProvider p = new RSACryptoServiceProvider();

            p = (RSACryptoServiceProvider)pc.PublicKey.Key;

            byte[] enBytes = p.Encrypt(pinBlock, false);

            return Convert.ToBase64String(enBytes);


           // return SecurityUtil.EncryptPin(pwd, card, encoding);
        }


        /// <summary>
        /// 数据加密
        /// </summary>
        /// <param name="encoder"></param>
        /// <returns></returns>
        public static string EncryptData(string data, Encoding encoding)
        {

            X509Certificate2 pc = new X509Certificate2(SDKConfig.EncryptCert);


            RSACryptoServiceProvider p = new RSACryptoServiceProvider();

            p = (RSACryptoServiceProvider)pc.PublicKey.Key;

            byte[] enBytes = p.Encrypt(encoding.GetBytes(data), false);

            return Convert.ToBase64String(enBytes);
        }
    }
}