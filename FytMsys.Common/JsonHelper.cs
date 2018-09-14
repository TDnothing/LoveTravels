using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace FytMsys.Common
{
    /// <summary>
    /// 操作Json帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 返回页面Json 提供客户端判断
        /// </summary>
        public class JsonAjaxModel
        {
            /// <summary>
            /// 数据 object类型
            /// </summary>
            public object DataC { get; set; }
            /// <summary>
            /// 数据 object类型
            /// </summary>
            public object DataB { get; set; }
            /// <summary>
            /// 数据 object类型
            /// </summary>
            public object DataA { get; set; }
            /// <summary>
            /// 数据 object类型
            /// </summary>
            public object Data { get; set; }
            /// <summary>
            /// 消息说明
            /// </summary>
            public string Msg { get; set; }
            /// <summary>
            /// 状态，可自定义
            /// </summary>
            public string Status { get; set; }
            /// <summary>
            /// 返回的url地址
            /// </summary>
            public string BackUrl { get; set; }
            /// <summary>
            /// 分页使用的总页数
            /// </summary>
            public int PageTotal { get; set; }
            /// <summary>
            /// 分页使用的总行数
            /// </summary>
            public int PageRows { get; set; }
            /// <summary>
            /// 是否收藏
            /// </summary>
            public int IsColl { get; set; }
        }

        public class jsonFile
        {
            /// <summary>
            /// 消息说明
            /// </summary>
            public string Msg { get; set; }
            /// <summary>
            /// 状态，可自定义
            /// </summary>
            public string Status { get; set; }
            /// <summary>
            /// 原图
            /// </summary>
            public string ImgUrl { get; set; }
            /// <summary>
            /// 缩略图
            /// </summary>
            public string ThumImg { get; set; }
        }

        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
    }
}
