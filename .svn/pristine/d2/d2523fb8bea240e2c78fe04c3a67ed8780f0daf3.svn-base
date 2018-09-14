using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.Server;
using Domain.Entity;

namespace FytMsys.Helper
{
    /// <summary>
    /// 公用类实例化业务对象
    /// </summary>
    public class OperateContext<T> where T : class,new()
    {
        public static BaseServer<T> SetServer = new BaseServer<T>();
    }

    public class OperateSession
    {
        public static FytDbContext SetContext = new FytDbContext();
    }

    public class ConfigHelpers
    {

    }

    public class ColumnHelper
    {
        ///// <summary>
        ///// 获得角色列表，根据级别进行分类展示
        ///// </summary>
        ///// <returns></returns>
        public static List<tb_Column> GetTreeList(int typeId)
        {
            var where = PredicateBuilder.True<tb_Column>();
            where = where.And(m => m.TypeID == typeId);
            var oldList = OperateContext<tb_Column>.SetServer.GetList(where, p => p.ID, true).ToList();
            var sList = new List<tb_Column>();
            Recursion(oldList, sList, 0);
            return sList;
        }
        ///// <summary>  
        ///// 递归菜单列表
        ///// </summary>  
        private static void Recursion(List<tb_Column> oldList, List<tb_Column> newList, int pid)
        {
            foreach (var item in from c in oldList where c.ParentId == pid select c)
            {
                var model = item;
                newList.Add(model);
                Recursion(oldList, newList, item.ID);
            }
        }
    }

    public class AttrHelper
    {
        ///// <summary>
        ///// 文章拓展属性
        ///// </summary>
        ///// <returns></returns>
        public static List<tb_GoodsAttr> GetTreeList(int typeId)
        {
            var where = PredicateBuilder.True<tb_GoodsAttr>();
            where = where.And(m => m.ParentId == typeId);
            var oldList = OperateContext<tb_GoodsAttr>.SetServer.GetList(where, p => p.ID, true).ToList();
            var sList = new List<tb_GoodsAttr>();
            Recursion(oldList, sList, 0);
            return sList;
        }
        ///// <summary>  
        ///// 递归菜单列表
        ///// </summary>  
        private static void Recursion(List<tb_GoodsAttr> oldList, List<tb_GoodsAttr> newList, int pid)
        {
            foreach (var item in from c in oldList where c.ParentId == pid select c)
            {
                var model = item;
                newList.Add(model);
                Recursion(oldList, newList, item.ID);
            }
        }
    }

}
