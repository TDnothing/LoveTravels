using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace FytMsys.Web
{
    /// <summary>
    /// 远程Post请求
    /// </summary>
    public class HttpUrlPost
    {
        public static string SendPost(string url,string pars)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;
            string result = "";
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = 20 * 100000;

                //设置POST的数据类型和长度
                request.ContentType = "application/x-www-form-urlencoded";


                //往服务器写入数据
                string body = pars;
                byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bodyBytes, 0, bodyBytes.Length);

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                result = "发生错误！ 消息："+e.Message;
            }
            return result;
        }
    }
}