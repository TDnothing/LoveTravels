﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Domain.Entity;

namespace FytMsys.DALMSSQL
{
    /// <summary>
    /// 实现接口基类
    /// <remarks>创建：2015.05.04 FUYU
    /// </remarks>
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class BaseRepository<T> where T : class ,new()
    {
        /// <summary>
        /// 获得数据上下文
        /// </summary>
        protected FytDbContext FytContext = new FytDbContext();


        #region 1、添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="isSave">是否保存</param>
        /// <returns>添加后的数据实体</returns>
        public T Add(T entity, bool isSave = true)
        {
            FytContext = new FytDbContext();
            FytContext.Set<T>().Add(entity);
            if (isSave) FytContext.SaveChanges();
            return entity;
        }

        /// <summary>
        /// 更新模型记录，如不存在进行添加操作
        /// </summary>
        public virtual bool SaveOrUpdate(T entity)
        {
            try
            {
                FytContext = new FytDbContext();
                var entry = FytContext.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    entry.State = EntityState.Modified;
                }
                else
                {
                    entry.State = EntityState.Added;
                }
                return FytContext.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 同时增加多条数据到一张表（事务处理）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool AddEntity(List<T> entitys)
        {
            FytContext = new FytDbContext();
            foreach (var entity in entitys)
            {
                FytContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Added;
            }
            return FytContext.SaveChanges() > 0;
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
            FytContext = new FytDbContext();
            FytContext.Set<T>().Attach(entity);
            FytContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
            return !isSave || FytContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 同时更新多条数据（事务处理）
        /// </summary>
        /// <param name="entitys">数据实体</param>
        /// <returns>是否成功</returns>
        public bool Update(List<T> entitys)
        {
            FytContext = new FytDbContext();
            entitys.ForEach(entity =>
            {
                FytContext.Set<T>().Attach(entity);
                FytContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;//将所有属性标记为修改状态
            });
            FytContext.Configuration.ValidateOnSaveEnabled = false;
            return FytContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 修改一条数据,会修改指定列的值
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Update(T entity, params string[] proNames)
        {
            FytContext = new FytDbContext();
            DbEntityEntry<T> dbee = FytContext.Entry<T>(entity);
            if (dbee.State == System.Data.Entity.EntityState.Detached)
            {
                FytContext.Set<T>().Attach(entity);
            }
            dbee.State = System.Data.Entity.EntityState.Unchanged;//先将所有属性状态标记为未修改
            proNames.ToList().ForEach(c => dbee.Property(c).IsModified = true);//将要修改的属性状态标记为修改
            FytContext.Configuration.ValidateOnSaveEnabled = false;
            return FytContext.SaveChanges() > 0;
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
            FytContext = new FytDbContext();
            FytContext.Set<T>().Attach(entity);
            FytContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
            return !isSave || FytContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="delWhere">删除表达式</param>
        /// <param name="isSave">是否保存</param>
        /// <returns>是否成功</returns>
        public bool DeleteBy(Expression<Func<T, bool>> delWhere, bool isSave = true)
        {
            FytContext = new FytDbContext();
            //3.1查询要删除的数据
            List<T> listDeleting = FytContext.Set<T>().Where(delWhere).ToList();
            //3.2将要删除的数据 用删除方法添加到 EF 容器中
            listDeleting.ForEach(u =>
            {
                FytContext.Set<T>().Attach(u);//先附加到 EF容器
                FytContext.Set<T>().Remove(u);//标识为 删除 状态
            });
            return !isSave || FytContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 批量物理删除数据,也可以用作单个物理删除--此方法适用于id为int类型的表--性能会比先查询后删除快
        /// </summary>
        /// <param name="ids">ID集合1,2,3</param>
        /// <returns>是否成功</returns>
        public bool DeletePhysics(string ids)
        {
            FytContext = new FytDbContext();
            var tableName = typeof(T).Name;//获取表名   
            var sql = string.Format("delete from {0} where id in({1})", tableName, ids);
            return FytContext.Database.ExecuteSqlCommand(sql) > 0;
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
            using (FytContext=new FytDbContext())
            {
                var context = ((IObjectContextAdapter)FytContext).ObjectContext;
                return context.ExecuteStoreQuery<T>(strSql);
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查询表达式</param>
        /// <returns>布尔值</returns>
        public bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            FytContext = new FytDbContext();
            return FytContext.Set<T>().Any(anyLambda);
        }

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>记录数</returns>
        public int Count(Expression<Func<T, bool>> predicate)
        {
            FytContext = new FytDbContext();
            return FytContext.Set<T>().Count(predicate);
        }

        /// <summary>
        /// 查询数据根据ID主键(优先)
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>实体</returns>
        public T GetModel(int id)
        {
            FytContext = new FytDbContext();
            return FytContext.Set<T>().Find(id);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体</returns>
        public T GetModel(Expression<Func<T, bool>> whereLambda)
        {
            FytContext = new FytDbContext();
            var entity =FytContext.Set<T>().AsNoTracking().FirstOrDefault<T>(whereLambda);
            return entity;
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
            FytContext = new FytDbContext();
            return isAsc ? FytContext.Set<T>().AsNoTracking().Where(whereLamdba).OrderBy(orderLambda) :
                FytContext.Set<T>().AsNoTracking().Where(whereLamdba).OrderByDescending(orderLambda);
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
            FytContext = new FytDbContext();
            var qy = FytContext.Set<T>().AsNoTracking().Where(whereLamdba);
            IOrderedQueryable<T> output = null;
            foreach (var orderByExpression in orderByExpressions)
            {
                if (output == null)
                    output = orderByExpression.ApplyOrderBy(qy);
                else
                    output = orderByExpression.ApplyThenBy(output);
            }
            return output ?? qy;
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
            FytContext = new FytDbContext();
            var temp = FytContext.Set<T>().Where<T>(whereLambda);
            rows = temp.Count();
            
            totalPage = rows % pageSize == 0 ? rows / pageSize : rows / pageSize + 1;
            temp = isAsc ? temp.OrderBy<T, TKey>(orderBy) : temp.OrderByDescending<T, TKey>(orderBy);
            return  temp.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);

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
            FytContext = new FytDbContext();
            var temp = FytContext.Set<T>().Where<T>(whereLambda);
            rows = temp.Count();

            IOrderedQueryable<T> output = null;
            foreach (var orderByExpression in orderByExpressions)
            {
                if (output == null)
                    output = orderByExpression.ApplyOrderBy(temp);
                else
                    output = orderByExpression.ApplyThenBy(output);
            }
            totalPage = rows % pageSize == 0 ? rows / pageSize : rows / pageSize + 1;
            return output == null ? temp.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize) : output.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);



        }
        #endregion
    }
}
