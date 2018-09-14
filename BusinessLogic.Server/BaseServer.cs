using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Domain.Entity;
using FytMsys.DALMSSQL;

namespace BusinessLogic.Server
{
    /// <summary>
    /// BLL层抽象类
    /// <remarks>创建：2015.05.04 FUYU
    /// </remarks>
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class BaseServer<T> where T : class,new()
    {
        /// <summary>
        /// 实例化数据层方法
        /// </summary>
        protected BaseRepository<T> Repository=new BaseRepository<T>();

        #region 1、添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="isSave">是否保存</param>
        /// <returns>添加后的数据实体</returns>
        public T Add(T entity, bool isSave = true)
        {
            return Repository.Add(entity,isSave);
        }

        /// <summary>
        /// 同时增加多条数据到一张表（事务处理）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool AddEntity(List<T> entitys)
        {
            return Repository.AddEntity(entitys);
        }
        #endregion

        #region 2、修改
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="isSave">是否保存</param>
        /// <returns>是否成功</returns>
        public bool Update(T entity, bool isSave = true)
        {
            return Repository.Update(entity,isSave);
        }

        /// <summary>
        /// 同时更新多条数据（事务处理）
        /// </summary>
        /// <param name="entitys">数据实体</param>
        /// <returns>是否成功</returns>
        public bool Update(List<T> entitys)
        {
            return Repository.Update(entitys);
        }

        /// <summary>
        /// 修改一条数据,会修改指定列的值
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Update(T entity, params string[] proNames)
        {
            return Repository.Update(entity,proNames);
        }
        #endregion

        #region 3、删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="isSave">是否保存</param>
        /// <returns>是否成功</returns>
        public bool Delete(T entity, bool isSave = true)
        {
            return Repository.Delete(entity,isSave);
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="delWhere">删除表达式</param>
        /// <param name="isSave">是否保存</param>
        /// <returns>是否成功</returns>
        public bool DeleteBy(Expression<Func<T, bool>> delWhere, bool isSave = true)
        {
            return Repository.DeleteBy(delWhere,isSave);
        }

        /// <summary>
        /// 批量物理删除数据,也可以用作单个物理删除--此方法适用于id为int类型的表--性能会比先查询后删除快
        /// </summary>
        /// <param name="ids">ID集合1,2,3</param>
        /// <returns>是否成功</returns>
        public bool DeletePhysics(string ids)
        {
            return Repository.DeletePhysics(ids);
        }
        #endregion

        #region 4、查询

        /// <summary>
        /// 根据sql语句返回动态类
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryEnumerable<T>(T sample, string strSql)
        {
            return Repository.QueryEnumerable<T>(sample, strSql);
        }

        /// <summary>
        /// 更新模型记录，如不存在进行添加操作
        /// </summary>
        public virtual void SaveOrUpdate(T entity,int id)
        {
            if (id == 0) Repository.Add(entity);
            else Repository.Update(entity);
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查询表达式</param>
        /// <returns>布尔值</returns>
        public bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            return Repository.Exist(anyLambda);
        }

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>记录数</returns>
        public int Count(Expression<Func<T, bool>> predicate)
        {
            return Repository.Count(predicate);
        }

        /// <summary>
        /// 查询数据根据ID主键(优先)
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>实体</returns>
        public T GetModel(int id)
        {
            return Repository.GetModel(id);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体</returns>
        public T GetModel(Expression<Func<T, bool>> whereLambda)
        {
            return Repository.GetModel(whereLambda);
        }


        /// <summary>
        /// 查找数据列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="orderLambda">排序表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> whereLamdba, Expression<Func<T, TKey>> orderLambda, bool isAsc)
        {
            return Repository.GetList(whereLamdba,orderLambda,isAsc);
        }

        /// <summary>
        /// 查找数据列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="orderByExpressions"></param>
        /// <returns></returns>
        public IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> whereLamdba, params IOrderByExpression<T>[] orderByExpressions)
        {
            return Repository.GetList<T>(whereLamdba, orderByExpressions);
        }
        #endregion

        #region 5、分页

        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <typeparam name="TKey">排序</typeparam>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="rows">总行数</param>
        /// <param name="totalPage">分页总数</param>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        public IQueryable<T> GetPageList<TKey>(int pageIndex, int pageSize, out int rows, out int totalPage,
            Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, TKey>> orderBy)
        {
            return Repository.GetPageList(pageIndex,pageSize,out rows,out totalPage,whereLambda,isAsc,orderBy);

        }

        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <typeparam name="TKey">排序</typeparam>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="rows">总行数</param>
        /// <param name="totalPage">分页总数</param>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="orderByExpressions">排序表达式</param>
        /// <returns></returns>
        public IQueryable<T> GetPageList(int pageIndex, int pageSize, out int rows, out int totalPage,
            Expression<Func<T, bool>> whereLambda, params IOrderByExpression<T>[] orderByExpressions)
        {
            return Repository.GetPageList(pageIndex, pageSize, out rows, out totalPage, whereLambda, orderByExpressions);
        }


        #endregion

    }
}
