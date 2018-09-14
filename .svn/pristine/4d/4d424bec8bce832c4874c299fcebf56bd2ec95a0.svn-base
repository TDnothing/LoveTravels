using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace FytMsys.Common
{
    public sealed class ConfigHelper
    {
        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            var cacheKey = "AppSettings-" + key;
            var objModel = CacheHelper.Get(cacheKey);
            if (objModel != null) return objModel.ToString();
            try
            {
                objModel = ConfigurationManager.AppSettings[key];
                if (objModel != null)
                {
                    CacheHelper.SetCache(cacheKey, objModel, DateTime.Now.AddMinutes(180), TimeSpan.Zero);
                }
            }
            catch (FormatException)
            { }
            return objModel.ToString();
        }

        /// <summary>
        /// 获得配置文件的 是否启用缓存机制
        /// </summary>
        /// <returns></returns>
        public static bool IsCache()
        {
            const string cacheKey = "AppSettings-iscache";
            const string key = "iscache";
            var objModel = CacheHelper.Get(cacheKey);
            if (objModel != null) return Convert.ToInt32(objModel) == 1;
            try
            {
                objModel = ConfigurationManager.AppSettings[key];
                if (objModel != null)
                {
                    CacheHelper.SetCache(cacheKey, objModel, DateTime.Now.AddMinutes(180), TimeSpan.Zero);
                }
            }
            catch (FormatException)
            { }
            return Convert.ToInt32(objModel) == 1;
        }

        /// <summary>
        /// 得到AppSettings中的配置Bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(string key)
        {
            var result = false;
            var cfgVal = GetConfigString(key);
            if (string.IsNullOrEmpty(cfgVal)) return false;
            try
            {
                result = bool.Parse(cfgVal);
            }
            catch (FormatException)
            {
                // Ignore format exceptions.
            }
            return result;
        }
        /// <summary>
        /// 得到AppSettings中的配置Decimal信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetConfigDecimal(string key)
        {
            decimal result = 0;
            var cfgVal = GetConfigString(key);
            if (string.IsNullOrEmpty(cfgVal)) return result;
            try
            {
                result = decimal.Parse(cfgVal);
            }
            catch (FormatException)
            {
                // Ignore format exceptions.
            }

            return result;
        }
        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(string key)
        {
            var result = 0;
            var cfgVal = GetConfigString(key);
            if (string.IsNullOrEmpty(cfgVal)) return result;
            try
            {
                result = int.Parse(cfgVal);
            }
            catch (FormatException)
            {
                // Ignore format exceptions.
            }

            return result;
        }
    }
}
