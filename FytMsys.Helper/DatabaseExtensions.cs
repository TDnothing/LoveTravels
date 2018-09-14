using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace FytMsys.Helper
{
    /// <summary>
    /// 查询动态类
    /// add huafg by 2015-05-10
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// 自定义Connection对象
        /// </summary>
        private static IDbConnection DefaultConnection
        {
            get
            {
                IDbConnection defaultConn = null;
                //数据库类型
                string action = ConfigurationManager.AppSettings["ConnectionString"];
                defaultConn = new System.Data.SqlClient.SqlConnection();
                return defaultConn;
            }
        }
        /// <summary>
        /// 自定义数据库连接字符串，与EF连接模式一致
        /// </summary>
        public static string DefaultConnectionString
        {
            get
            {
                string connstr = ConfigurationManager.AppSettings["ConnectionString"];
                return connstr;
            }
        }
        /// <summary>
        /// 动态查询主方法
        /// </summary>
        /// <returns></returns>
        public static IEnumerable SqlQueryForDynamic(this Database db,
                string sql,
                params object[] parameters)
        {
            IDbConnection defaultConn = DefaultConnection;

            //ADO.NET数据库连接字符串
            db.Connection.ConnectionString = DefaultConnectionString;

            return SqlQueryForDynamicOtherDB(db, sql, defaultConn, parameters);
        }
        private static IEnumerable SqlQueryForDynamicOtherDB(this Database db,  string sql, IDbConnection conn, params object[] parameters)
        {
            conn.ConnectionString = db.Connection.ConnectionString;

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }
            }

            IDataReader dataReader = cmd.ExecuteReader();

            if (!dataReader.Read())
            {
                return null; //无结果返回Null
            }

            #region 构建动态字段

            TypeBuilder builder = DatabaseExtensions.CreateTypeBuilder(
                          "EF_DynamicModelAssembly",
                          "DynamicModule",
                          "DynamicType");

            int fieldCount = dataReader.FieldCount;
            for (int i = 0; i < fieldCount; i++)
            {
                DatabaseExtensions.CreateAutoImplementedProperty(
                  builder,
                  dataReader.GetName(i),
                  dataReader.GetFieldType(i));
            }

            #endregion

            dataReader.Close();
            dataReader.Dispose();
            cmd.Dispose();
            conn.Close();
            conn.Dispose();

            Type returnType = builder.CreateType();

            if (parameters != null)
            {
                return db.SqlQuery(returnType, sql, parameters);
            }
            else
            {
                return db.SqlQuery(returnType, sql);
            }
        }

        private static TypeBuilder CreateTypeBuilder(string assemblyName, string moduleName, string typeName)
        {
            TypeBuilder typeBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
              new AssemblyName(assemblyName),
              AssemblyBuilderAccess.Run).DefineDynamicModule(moduleName).DefineType(typeName,
              TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }

        private static void CreateAutoImplementedProperty(TypeBuilder builder, string propertyName, Type propertyType)
        {
            const string PrivateFieldPrefix = "m_";
            const string GetterPrefix = "get_";
            const string SetterPrefix = "set_";

            // Generate the field.
            FieldBuilder fieldBuilder = builder.DefineField(
              string.Concat(
                PrivateFieldPrefix, propertyName),
              propertyType,
              FieldAttributes.Private);

            // Generate the property
            PropertyBuilder propertyBuilder = builder.DefineProperty(
              propertyName,
              System.Reflection.PropertyAttributes.HasDefault,
              propertyType, null);

            // Property getter and setter attributes.
            MethodAttributes propertyMethodAttributes = MethodAttributes.Public
              | MethodAttributes.SpecialName
              | MethodAttributes.HideBySig;

            // Define the getter method.
            MethodBuilder getterMethod = builder.DefineMethod(
                string.Concat(
                  GetterPrefix, propertyName),
                propertyMethodAttributes,
                propertyType,
                Type.EmptyTypes);

            // Emit the IL code.
            // ldarg.0
            // ldfld,_field
            // ret
            ILGenerator getterILCode = getterMethod.GetILGenerator();
            getterILCode.Emit(OpCodes.Ldarg_0);
            getterILCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterILCode.Emit(OpCodes.Ret);

            // Define the setter method.
            MethodBuilder setterMethod = builder.DefineMethod(
              string.Concat(SetterPrefix, propertyName),
              propertyMethodAttributes,
              null,
              new Type[] { propertyType });

            // Emit the IL code.
            // ldarg.0
            // ldarg.1
            // stfld,_field
            // ret
            ILGenerator setterILCode = setterMethod.GetILGenerator();
            setterILCode.Emit(OpCodes.Ldarg_0);
            setterILCode.Emit(OpCodes.Ldarg_1);
            setterILCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterILCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
        }

        public static dynamic SqlFunctionForDynamic(this Database db,
                string sql,
                params object[] parameters)
        {
            IDbConnection conn = DefaultConnection;

            //ADO.NET数据库连接字符串
            conn.ConnectionString = DefaultConnectionString;

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }
            }
            //1、DataReader查询数据
            IDataReader dataReader = cmd.ExecuteReader();
            if (!dataReader.Read())
            {
                return null;
            }
            //2、DataReader转换Json
            string jsonstr = Common.JsonConverter.ToJson(dataReader);
            //3、Json转换动态类
            dynamic dyna =  Common.JsonConverter.ConvertJson(jsonstr);

            #region 构建动态字段 --废弃

            //TypeBuilder builder = DatabaseExtensions.CreateTypeBuilder(
            //              "EF_DynamicModelAssembly",
            //              "DynamicModule",
            //              "DynamicType");

            //int fieldCount = dataReader.FieldCount;
            //for (int i = 0; i < fieldCount; i++)
            //{
            //    DatabaseExtensions.CreateAutoImplementedProperty(
            //      builder,
            //      dataReader.GetName(i),
            //      dataReader.GetFieldType(i));
            //}

            //#endregion
            //Type returnType = builder.CreateType();
            
            //List<object> list = new List<object>();
            //while (dataReader.Read())
            //{
            //    for (int i = 0; i < dataReader.FieldCount; i++)
            //    {
            //        if (!IsNullOrDBNull(dataReader[i]))
            //        {
            //            PropertyInfo pi = returnType.GetProperty(dataReader.GetName(i), BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            //            if (pi != null)
            //            {
            //                pi.SetValue(builder, CheckType(dataReader[i], pi.PropertyType), null);
            //            }
            //        }
            //        list.Add(builder);
            //    }
            //}
            
            #endregion

            dataReader.Close();
            dataReader.Dispose();
            cmd.Dispose();
            conn.Close();
            conn.Dispose();

            return dyna;
        }
        /// <summary>
        /// 对可空类型进行判断转换(*要不然会报错)
        /// </summary>
        /// <param name="value">DataReader字段的值</param>
        /// <param name="conversionType">该字段的类型</param>
        /// <returns></returns>
        private static object CheckType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }

        /// <summary>
        /// 判断指定对象是否是有效值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool IsNullOrDBNull(object obj)
        {
            return (obj == null || (obj is DBNull)) ? true : false;
        }
    }


    public class AdoHelper
    {
        /// <summary>
        /// EF SQL 语句返回 dataTable
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable SqlQueryForDataTatable(string sql,
        SqlParameter[] parameters)
        {
            var conn = new SqlConnection { ConnectionString = DatabaseExtensions.DefaultConnectionString };
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            var cmd = new SqlCommand { Connection = conn, CommandText = sql };
            if (parameters!=null && parameters.Length > 0)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }
            }
            var adapter = new SqlDataAdapter(cmd);
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        /// <summary>
        /// 查询返回数据表的重载方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public static DataSet GetTableDataSet(string sql, CommandType commandType, params SqlParameter[] sqlParams)
        {
            DataSet dataSet = null;
            using (var conn = new SqlConnection(DatabaseExtensions.DefaultConnectionString))
            {

                var cmd = new SqlCommand(sql, conn) {CommandType = commandType, CommandTimeout = 180};
                AdoHelper.SetParams(cmd, sqlParams);
                var adp = new SqlDataAdapter {SelectCommand = cmd};
                try
                {
                    dataSet = new DataSet();
                    adp.Fill(dataSet, "table");
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源
                    cmd.Dispose();
                    adp.Dispose();
                    conn.Close();
                }
            }
            return dataSet.Tables[0] != null ? dataSet : null;
        }
        /// <summary>
        /// 设置命令中参数的方法
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sqlParams"></param>
        private static void SetParams(SqlCommand cmd, params SqlParameter[] sqlParams)
        {
            if (sqlParams != null && sqlParams.Length > 0)
            {
                cmd.Parameters.AddRange(sqlParams);
            }
        }
    }
}
