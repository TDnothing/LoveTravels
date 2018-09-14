using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.ViewModel;
using FytMsys.Common;

namespace FytMsys.Helper
{
    /// <summary>
    /// 网站专用帮助类
    /// </summary>
    public class WebSiteHelper
    {
        #region 1、获得网站基本信息 +GetSite(int siteId)

        /// <summary>
        /// 获得网站基本信息
        /// </summary>
        /// <param name="siteId">站点ID</param>
        /// <returns></returns>
        public static tb_Site GetSite(int siteId)
        {
            tb_Site model;
            if (CacheHelper.Get<tb_Site>("SiteManage") != null)
            {
                model = CacheHelper.Get<tb_Site>("SiteManage") as tb_Site;
            }
            else
            {
                model = OperateContext<tb_Site>.SetServer.GetModel(m => m.ID == siteId) ?? new tb_Site();
                CacheHelper.Insert("SiteManage", model);
            }
            return model;
        }
        #endregion

        #region 2、商品分页方法  +GetPage(int pageSize, int page, int count, string url)
        /// <summary>
        /// 商品分页方法
        /// </summary>
        /// <param name="pageSize">当前页显示总条数</param>
        /// <param name="page">当前页码</param>
        /// <param name="count">总数</param>
        /// <param name="url">访问的地址</param>
        /// <returns></returns>
        public static string GetPage(int pageSize, int page, int count, string url)
        {
            int maxi = 0;
            var strPage = new StringBuilder();
            if (count % pageSize == 0)
            {
                maxi = count / pageSize;
            }
            else
            {
                maxi = (count / pageSize) + 1;
            }
            if (maxi < 2)
            {
                return strPage.ToString();
            }
            if (page > 1)
            {
                if (url.Contains("?"))
                {
                    var ulist = url.Split('?');
                    var flist = ulist[0].Substring(0, ulist[0].Length - 1) + 1;
                    var list = ulist[0].Substring(0, ulist[0].Length - 1) + (page - 1);
                    var furl = flist + "?" + ulist[1];
                    url = list + "?" + ulist[1];
                    strPage.Append(" <a href=\"" + url + "\" ><<上一页</a>");
                }
                else
                {
                    strPage.Append(" <a href=\"" + url.Split('-')[0] + "-" + (page - 1) + "\" ><<上一页</a>");
                }
            }
            else
            {
                if (url.Contains("?"))
                {
                    var ulist = url.Split('?');
                    var list = ulist[0].Substring(0, ulist[0].Length - 1) + 1;
                    url = list + "?" + ulist[1];
                }
                else
                {
                    strPage.Append(" <a href=\"javascript:void(0)\"><<上一页</a>");
                }
            }
            if (maxi < 8)
            {
                for (int i = 1; i < maxi + 1; i++)
                {
                    if (i == page)
                    {
                        strPage.Append("<a>" + i + "</a>");
                    }
                    else
                    {
                        if (url.Contains("?"))
                        {
                            var ulist = url.Split('?');
                            var list = ulist[0].Substring(0, ulist[0].Length - 1) + (i);
                            url = list + "?" + ulist[1];
                            strPage.Append("<a href=\"" + url + "\">" + i + "</a>");
                        }
                        else
                        {
                            strPage.Append("<a href=\"" + url.Split('-')[0] + "-" + i + "\">" + i + "</a>");
                        }
                    }
                }
            }
            else
            {
                int maxfeye = page + 5;
                int minfeye = page - 4;
                if (page < 6)
                {
                    minfeye = 1;
                    maxfeye = 11;
                }
                if (maxfeye > maxi)
                {
                    maxfeye = maxi;
                }
                for (int f = minfeye; f < maxfeye + 1; f++)//每页显示9个分页数字
                {
                    if (f == page)
                    {
                        strPage.Append("<a>" + f + "</a></li>");
                    }
                    else
                    {
                        if (url.Contains("?"))
                        {
                            var ulist = url.Split('?');
                            var list = ulist[0].Substring(0, ulist[0].Length - 1) + (f);
                            url = list + "?" + ulist[1];
                            strPage.Append("<a href=\"" + url + "\">" + f + "</a>");
                        }
                        else
                        {
                            strPage.Append("<a href=\"" + url.Split('-')[0] + "-" + f + "\">" + f + "</a>");
                        }
                    }
                }

            }
            if (page < maxi)
            {
                if (url.Contains("?"))
                {
                    var ulist = url.Split('?');
                    var list = ulist[0].Substring(0, ulist[0].Length - 1) + (page + 1);
                    url = list + "?" + ulist[1];
                    strPage.Append("<a href=\"" + url + "\" >下一页>></a>");
                }
                else
                {
                    strPage.Append(" <a href=\"" + url.Split('-')[0] + "-" + (page + 1) + "\" >下一页>></a>");
                }
            }
            else
            {
                strPage.Append(" <a href=\"javascript:void(0)\">下一页>></a>");
            }
            if (url.Contains("?"))
            {
                var ulist = url.Split('?');
                var llist = ulist[0].Substring(0, ulist[0].Length - 1) + maxi;
                var lurl = llist + "?" + ulist[1];
            }
            else
            {
            }
            return strPage.ToString();
        }
        #endregion

        #region 3、上下页方法 +GetPageTurning(int pageSize, int page, int count, string url)
        /// <summary>
        /// 上下页方法
        /// </summary>
        /// <param name="pageSize">当前页显示总条数</param>
        /// <param name="page">当前页码</param>
        /// <param name="count">总数</param>
        /// <param name="url">访问的地址</param>
        /// <returns></returns>
        public static string GetPageTurning(int pageSize, int page, int count, string url)
        {
            int maxi = 0;
            var strPage = new StringBuilder();
            if (count % pageSize == 0)
            {
                maxi = count / pageSize;
            }
            else
            {
                maxi = (count / pageSize) + 1;
            }
            if (page > 1)
            {
                if (url.Contains("?"))
                {
                    var ulist = url.Split('?');
                    var list = ulist[0].Substring(0, ulist[0].Length - 1) + (page - 1);
                    url = list + "?" + ulist[1];
                    strPage.Append("<a  href=\"" + url + "\" class='wpage'><i class='icon_triangle triangle_prev'></i>上一页</a>");
                }
                else
                {
                    strPage.Append("<a  href=\"" + url.Substring(0, url.Length - 1) + "" + (page - 1) + "\" class='wpage'><i class='icon_triangle triangle_prev'></i>上一页</a>");
                }
            }
            else
            {
                strPage.Append("<a href='javascript:void(0)' class='wpage disabled'><i class='icon_triangle triangle_prev'></i>上一页</a>");
            }
            if (page < maxi)
            {
                if (url.Contains("?"))
                {
                    var ulist = url.Split('?');
                    var list = ulist[0].Substring(0, ulist[0].Length - 1) + (page + 1);
                    url = list + "?" + ulist[1];
                    strPage.Append("<a  href=\"" + url + "\" class='wpage'><i class='icon_triangle triangle_next'></i>下一页</a>");
                }
                else
                {
                    strPage.Append("<a  href=\"" + url.Substring(0, url.Length - 1) + "" + (page + 1) + "\" class='wpage'><i class='icon_triangle triangle_next'></i>下一页</a>");
                }
            }
            else
            {
                strPage.Append(
                    "<a href='javascript:void(0)' class='wpage disabled'><i class='icon_triangle triangle_next'></i>下一页</a>");
            }
            return strPage.ToString();
        }
        #endregion

        #region 4、个人中心分页方法 +GetPaging(int pageSize, int page, int count, string url)
        /// <summary>
        /// 个人中心分页方法
        /// </summary>
        /// <param name="pageSize">当前页显示总条数</param>
        /// <param name="page">当前页码</param>
        /// <param name="count">总数</param>
        /// <param name="url">访问的地址</param>
        /// <returns></returns>
        public static string GetPaging(int pageSize, int page, int count, string url)
        {
            int maxi = 0;
            var strPage = new StringBuilder();
            //取出所有新闻列表
            if (count % pageSize == 0)
            {
                maxi = count / pageSize;
            }
            else
            {
                maxi = (count / pageSize) + 1;
            }
            if (maxi < 2)
            {
                return strPage.ToString();
            }
            if (url.Contains("?"))
            {
                url += "&page=";
            }
            else
            {
                url += "?page=";
            }
            if (page > 1)
                strPage.Append("<a href=\"" + url + "1\">首页</a>");
            else
                strPage.Append("<a href=\"" + url + "1\">首页</a>");
            if (maxi < 8)
            {
                for (int i = 1; i < maxi + 1; i++)
                {
                    if (i == page)
                    {
                        strPage.Append("<a class=\"pager-current\">" + i + "</a>");
                    }
                    else
                    {
                        strPage.Append("<a href=\"" + url + "" + i + "\">" + i + "</a>");
                    }
                }
            }
            else
            {
                int maxfeye = page + 5;
                int minfeye = page - 4;
                if (page < 6)
                {
                    minfeye = 1;
                    maxfeye = 11;
                }
                if (maxfeye > maxi)
                {
                    maxfeye = maxi;
                }
                for (int f = minfeye; f < maxfeye + 1; f++)//每页显示9个分页数字
                {
                    if (f == page)
                    {
                        strPage.Append("<a class=\"pager-current\">" + f + "</a>");
                    }
                    else
                    {
                        strPage.Append("<a href=\"" + url + "" + f + "\">" + f + "</a>");

                    }
                }

            }
            //if (Page < maxi)
            //    strPage.Append("<a title=\"下一页\" href=\"" + url + "" + (Page + 1) + "\" class=\"dpage\">下一页</a>");
            //else
            //    strPage.Append("<a title=\"下一页\" href=\"javascript:void(0)\" class=\"dpage disabled\">下一页</a>");
            strPage.Append("<a class=\"dpage\" title=\"末页\" href=\"" + url + "" + maxi + "\">尾页</a>");
            return strPage.ToString();
        }
        #endregion

        #region 5、又一扩展分页方法，以后企业站调用这个方法比较，UI很好
        /// <summary>
        /// 又一扩展分页方法，以后企业站调用这个方法比较，UI很好
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="count">总共多少页</param>
        /// <param name="url">当前Url地址</param>
        /// <returns></returns>
        public static string WebSitePage(int page, int count, string url)
        {
            //判断，如果分页总数等于1，则不显示分页
            if (count == 1)
            {
                return "";
            }
            var strPage = new StringBuilder();
            //判断路径里面是否包含?号
            if (url.Contains("?"))
            {
                url += "&page=";
            }
            else
            {
                url += "?page=";
            }
            //如果总数小于指定数值，则循环，不显示...标记
            if (count < 9)
            {
                for (int i = 1; i < count + 1; i++)
                {
                    if (i == page)
                    {
                        strPage.Append("<div class=\"page_item cur\">" + i + "</div>");
                    }
                    else
                    {
                        strPage.Append("<div class=\"page_item\"><a href=\"" + url + "" + i + "\">" + i + "</a></div>");
                    }
                }
            }
            else
            {
                //如果总数大于指定数值，进行分解
                int maxfeye = page + 4;
                int minfeye = page - 3;
                if (page < 8)
                {
                    minfeye = 1;
                    maxfeye = 8;
                }
                if (maxfeye > count)
                {
                    minfeye = count - 4;
                    maxfeye = count;
                }
                //如果当前页大于指定数值，显示首页
                if (page > 8)
                {
                    strPage.Append("<div class=\"next\"><a href=\"" + url + "\">　首页　</a></div>");
                }
                for (int f = minfeye; f < maxfeye + 1; f++)//每页显示9个分页数字
                {
                    if (f == page)
                    {
                        strPage.Append("<div class=\"page_item cur\">" + f + "</div>");
                    }
                    else
                    {
                        strPage.Append("<div class=\"page_item\"><a href=\"" + url + "" + f + "\">" + f + "</a></div>");

                    }
                }
                //如果总数大于6，则后面增加...标记，以及总页数
                if (count - page > 6)
                {
                    strPage.Append("<div class=\"dot\"></div><div class=\"page_item\"><a href=\"" + url + "" + count + "\">" + count + "</a></div>");
                }
                //判断是否存在下一页
                if (page == count)
                {
                    strPage.Append("<div class=\"next\">下一页</div>");
                }
                else
                {
                    strPage.Append("<div class=\"next\"><a href=\"" + url + "" + (page + 1) + "\">下一页</a></div>");
                }
            }
            return strPage.ToString();
        }

        #endregion

    }

    #region 会员注册配置读取帮助类 +MemberHelper
    /// <summary>
    /// 会员注册配置读取帮助类
    /// </summary>
    public class MemberHelper
    {
        /// <summary>
        /// 通过缓存方式读取会员基本配置信息
        /// </summary>
        /// <returns></returns>
        public static MemberConfig GetCacheMember()
        {
            var model = CacheHelper.Get<MemberConfig>(KeyHelper.CACHE_USER_CONFIG);
            if (model != null) return model;
            CacheHelper.Insert(KeyHelper.CACHE_USER_CONFIG, LoadConfig(UtilsHelper.GetXmlMapPath(KeyHelper.FILE_USER_XML_CONFING)),
                UtilsHelper.GetXmlMapPath(KeyHelper.FILE_USER_XML_CONFING));
            model = CacheHelper.Get<MemberConfig>(KeyHelper.CACHE_USER_CONFIG);
            return model;
        }
        /// <summary>
        ///  读取站点配置文件
        /// </summary>
        public static MemberConfig LoadConfig(string configFilePath)
        {
            return (MemberConfig)SerializationHelper.Load(typeof(MemberConfig), configFilePath);
        }
    }
    #endregion

    #region 网站基本配置帮助类 +SysBasicHelper
    /// <summary>
    /// 网站基本配置帮助类
    /// </summary>
    public class SysBasicHelper
    {
        /// <summary>
        /// 通过缓存方式读取网站基本配置信息
        /// </summary>
        /// <returns></returns>
        public static SysBasicConfig GetCacheSysBasic()
        {
            var model = CacheHelper.Get<SysBasicConfig>(KeyHelper.CACHE_SITE_CONFIG);
            if (model != null) return model;
            CacheHelper.Insert(KeyHelper.CACHE_SITE_CONFIG, LoadConfig(UtilsHelper.GetXmlMapPath(KeyHelper.FILE_SITE_XML_CONFING)),
                UtilsHelper.GetXmlMapPath(KeyHelper.FILE_SITE_XML_CONFING));
            model = CacheHelper.Get<SysBasicConfig>(KeyHelper.CACHE_SITE_CONFIG);
            return model;
        }

        /// <summary>
        /// 根据配置文件读取xml配置信息
        /// </summary>
        /// <param name="configFilePath"></param>
        /// <returns></returns>
        public static SysBasicConfig LoadConfig(string configFilePath)
        {
            return (SysBasicConfig)SerializationHelper.Load(typeof(SysBasicConfig), configFilePath);
        }
    }
    #endregion
}
