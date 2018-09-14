using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace FytMsys.DALMSSQL
{
    public class DBContextFactory
    {
        #region 创建 EF上下文 对象，在线程中共享 一个 上下文对象 
        /// <summary>
        /// 创建 EF上下文 对象，在线程中共享 一个 上下文对象
        /// </summary>
        /// <returns></returns>
        public static DbContext GetDbContext()
        {
            //从当前线程中 获取 EF上下文对象
            var dbContext = CallContext.GetData(typeof(DBContextFactory).Name) as DbContext;
            if (dbContext != null) return dbContext;
            dbContext = new FytDbContext();
            dbContext.Configuration.ValidateOnSaveEnabled = false;
            CallContext.SetData(typeof(DBContextFactory).Name, dbContext);
            return dbContext;
        }
        #endregion
    }
}
