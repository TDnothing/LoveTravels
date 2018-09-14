using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using FytMsys.Common;

namespace FytMsys.Web.Controllers
{
    public class MoneyHelper
    {
        /// <summary>
        /// 获得国际化汇率
        /// </summary>
        /// <returns></returns>
        public static decimal GetHl()
        {
            try
            {
                var ulr = "http://api.k780.com:88/?app=finance.rate&scur=USD&tcur=CNY&appkey=10003&sign=b59bc3ef6191eb9f747dd4e83c99f2a4";
                WebClient client = new WebClient();
               // string jsonURL = "http://localhost/jev";
               var source = client.DownloadString(ulr);

                var s = UtilsHelper.GetHttp(ulr);
                var ss = JsonConverter.ConvertJson(s);
                var gg = 1;
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                var routes_list =
          json_serializer.DeserializeObject("{ \"rate\":\"some data\" }");
                return Convert.ToDecimal(ss.ss.result.rate);
            }
            catch (Exception)
            {
                return Convert.ToDecimal("7");
            }
        }
    }
}