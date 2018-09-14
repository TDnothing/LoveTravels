using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FytMsys.Helper
{
    public class DataBackHelper
    {
        public static void BackUpAll()
        {


            string folter = AppDomain.CurrentDomain.BaseDirectory + "backup/";
            if (!Directory.Exists(folter)) Directory.CreateDirectory(folter);
            string file = folter + "all_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".sql";
            DataTable tables = AdoHelper.SqlQueryForDataTatable("select * from sysobjects where xtype='U'",null);
            Back_Restore_Start();
            for (int i = 0; i < tables.Rows.Count; i++)
            {
                DataRow table = tables.Rows[i];
                BackUpSingleTable(table["name"].ToString(), file, true);
            }
            Back_Restore_End();

        }

        public static void BackUp(string sql, string file)
        {
            File.AppendAllText(file, sql, System.Text.Encoding.UTF8);
        }

        public static void Back_Restore_Start()
        {
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "backup.bk", "aaa");
        }

        public static void Back_Restore_End()
        {
            System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "backup.bk");
        }

        public static bool Back_Restore_Now()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "backup.bk"))
            {
                return true;
            }
            return false;
        }

        public static void BackUpSingleTable(string tablename, string file, bool isall)
        {
            if (!isall)
                Back_Restore_Start();
            try
            {
                DataTable table = AdoHelper.SqlQueryForDataTatable("select * from sysobjects where name='" + tablename + "' and type='U'",null);
                string sql = String.Empty;
                bool auto_insert = false;
                if (table.Rows.Count == 1)
                {
                    DataRow t = table.Rows[0];
                    sql = sql + ";\r\nif(EXISTS(select * from sysobjects where  xtype='U' and name='" + t["name"] + "'))drop table [" + t["name"] + "]";
                    sql = sql + ";\r\ncreate table [" + t["name"] + "] \r\n (\r\n";
                    //DataTable columns = DBclass.GetTableDataSet("select t3.*,t2.indid from(select t1.*,syscomments.text as syscomments_text from (select syscolumns.colid as syscolumns_colid, syscolumns.name as syscolumns_name,syscolumns.id as syscolumns_id,syscolumns.xtype as syscolumns_xtype,syscolumns.length as syscolumns_length,syscolumns.status as syscolumns_status,syscolumns.prec as syscolumns_prec,syscolumns.scale as syscolumns_scale, syscolumns.colstat as syscolumns_colstat,syscolumns.cdefault as syscolumns_cdefault,syscolumns.isnullable as syscolumns_isnullable,systypes.name as systypes_name from syscolumns  inner join systypes on(syscolumns.xtype=systypes.xtype) where syscolumns.id=" + t["id"] + ") t1 left join syscomments on(t1.syscolumns_cdefault=syscomments.id))t3 left join (select sysindexes.id,sysindexes.indid,colid from sysindexes inner join sysindexkeys on(sysindexes.id=sysindexkeys.id) where sysindexes.id=" + t["id"] + " and sysindexes.indid=1)t2 on(t3.syscolumns_colid=t2.colid) order by syscolumns_colid asc").Tables[0];
                    SqlParameter[] spfy = new SqlParameter[]
                  {
                        new SqlParameter("@syscolumnsid",Convert.ToInt32(t["id"])),
                        new SqlParameter("@sysindexesid",Convert.ToInt32(t["id"]))
                  };
                    DataTable columns = AdoHelper.GetTableDataSet("bfdata", CommandType.StoredProcedure, spfy).Tables[0];

                    for (int i = 0; i < columns.Rows.Count; i++)
                    {
                        DataRow column = columns.Rows[i];
                        sql = sql + "[" + column["syscolumns_name"] + "] " + column["systypes_name"];
                        string name = column["systypes_name"].ToString();
                        if (name == "numeric" || name == "decimal")
                        {
                            sql = sql + "(" + column["syscolumns_prec"] + "," + column["syscolumns_scale"] + ")";
                        }
                        else if (name == "char" || name == "nchar" || name == "nvarchar" || name == "varchar")
                        {
                            sql = sql + "(" + column["syscolumns_length"] + ")";
                        }
                        if (Convert.ToInt32(column["syscolumns_colstat"]) > 0)
                        {
                            sql = sql + " identity(1,1)";
                            auto_insert = true;
                        }
                        if (Convert.ToInt32(column["syscolumns_isnullable"]) == 0)
                        {
                            sql = sql + " not null";
                        }
                        else
                        {
                            sql = sql + " null";
                        }
                        if (column["indid"].ToString().Length > 0 && Convert.ToInt32(column["indid"]) == 1 && i == 0)
                        {
                            sql = sql + " primary key";
                        }
                        if (Convert.ToInt32(column["syscolumns_cdefault"]) > 0)
                        {
                            sql = sql + " default " + column["syscomments_text"];
                        }
                        sql = sql + ",\r\n";

                    }
                    sql = sql + ") ;\r\n";

                    if (auto_insert)
                    {
                        sql = sql + "\r\nSET IDENTITY_INSERT  [" + t["name"] + "] ON";
                    }
                    BackUp(sql, file); //写入文件
                    sql = String.Empty;//清空sql
                    DataTable data = AdoHelper.SqlQueryForDataTatable("select * from [" + t["name"] + "]",null);
                    for (int j = 0; j < data.Rows.Count; j++)
                    {
                        string insertsqlvalue = String.Empty;
                        string insertsql = String.Empty;
                        DataRow d = data.Rows[j];
                        for (int i = 0; i < columns.Rows.Count; i++)
                        {
                            DataRow column = columns.Rows[i];
                            insertsql = insertsql + ",[" + column["syscolumns_name"] + "]";
                            string value = d[column["syscolumns_name"].ToString()].ToString();
                            if (d[column["syscolumns_name"].ToString()] is DBNull)
                                insertsqlvalue = insertsqlvalue + ",null";
                            else if (value == String.Empty)
                                insertsqlvalue = insertsqlvalue + ",''";
                            else
                                insertsqlvalue = insertsqlvalue + ",'" + value.Replace("'", "''").Replace(";\r\n", "; \r\n") + "'";
                        }
                        if (insertsql.Length > 0)
                        {
                            insertsql = ";\r\ninsert into [" + t["name"] + "](" + insertsql.Substring(1) + ")values(" + insertsqlvalue.Substring(1) + ")";
                        }
                        sql = sql + insertsql + ";\r\n";
                        BackUp(sql, file); //写入文件
                        sql = String.Empty;//清空sql
                    }
                    if (auto_insert)
                    {
                        sql = sql + ";\r\nSET IDENTITY_INSERT  [" + t["name"] + "] OFF";
                    }
                    BackUp(sql, file);//写入文件
                }
            }
            catch { }
            if (!isall)
                Back_Restore_End();
        }

        public static void BackUpSingle(object tablename)
        {
            string folter = AppDomain.CurrentDomain.BaseDirectory + "backup/";
            if (!Directory.Exists(folter)) Directory.CreateDirectory(folter);
            string file = folter + tablename + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".sql";
            BackUpSingleTable(tablename.ToString(), file, false);
        }


        public static void Restore(object path)
        {
            Back_Restore_Start();
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseExtensions.DefaultConnectionString))
                {
                    string filename = path.ToString();
                    FileStream reader = File.OpenRead(filename);
                    try
                    {
                        conn.Open();

                        string sql = String.Empty;
                        string[] sqls = null;
                        long totalcount = reader.Length;
                        int curpos = 0;
                        while (totalcount > 0)
                        {

                            if (totalcount > 512000)
                            {
                                byte[] buffer = new byte[512000];
                                curpos = curpos + reader.Read(buffer, 0, buffer.Length);
                                sql = sql + System.Text.Encoding.UTF8.GetString(buffer);
                                string[] split = new string[] { ";\r\n" };
                                sqls = sql.Split(split, StringSplitOptions.RemoveEmptyEntries);
                                if (sqls.Length > 1)
                                {
                                    for (int i = 0; i < sqls.Length - 1; i++)
                                    {
                                        try
                                        {
                                            SqlCommand cmd = new SqlCommand(sqls[i], conn);
                                            cmd.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            //string s = String.Empty;
                                            //if (i > 1)
                                            //    s = s + sqls[i - 1] + "VVV";
                                            //s = s + sql[i] + "DDD";
                                            //if (i < sqls.Length)
                                            //    s = s + sqls[i+1] + "SSS";
                                            //File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "a.txt", s + "\r\n" + ex.Message);

                                        }
                                    }
                                }
                                if (sqls.Length >= 1)
                                    sql = sqls[sqls.Length - 1];
                                totalcount = totalcount - buffer.Length;
                            }
                            else
                            {
                                byte[] buffer = new byte[totalcount];
                                curpos = curpos + reader.Read(buffer, 0, (int)totalcount);
                                sql = sql + System.Text.Encoding.UTF8.GetString(buffer);
                                string[] split = new string[] { ";\r\n" };
                                sqls = sql.Split(split, StringSplitOptions.RemoveEmptyEntries);
                                if (sqls.Length > 1)
                                {
                                    for (int i = 0; i < sqls.Length - 1; i++)
                                    {
                                        try
                                        {
                                            SqlCommand cmd = new SqlCommand(sqls[i], conn);
                                            cmd.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            //string s = String.Empty;
                                            //if (i > 1)
                                            //    s = s + sqls[i - 1]+"VVV";
                                            //s = s + sql[i]+"DDD";
                                            //if (i < sqls.Length)
                                            //    s = s + sqls[i+1]+"SSS";
                                            //File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "a.txt",s + "\r\n" + ex.Message);

                                        }
                                    }
                                }
                                if (sqls.Length >= 1)
                                    sql = sqls[sqls.Length - 1];
                                totalcount = 0;
                            }
                        }

                        if (sql != String.Empty)
                        {
                            try
                            {
                                SqlCommand cmd = new SqlCommand(sql, conn);
                                cmd.ExecuteNonQuery();
                            }
                            catch
                            {
                                //File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "a.txt", "\r\n" + sql);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        reader.Close();
                        conn.Close();

                    }
                }
            }
            catch { }
            Back_Restore_End();

        }




        public static bool Restore(string sql)
        {
            bool ok = false;
            if (sql == String.Empty)
                return true;
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString()))
            {
                try
                {
                    conn.Open();
                    string[] split = new string[] { ";\r\n" };
                    string[] sqls = null;
                    sqls = sql.Split(split, StringSplitOptions.RemoveEmptyEntries);
                    ok = true;
                    if (sqls != null && sqls.Length > 0)
                    {

                        for (int i = 0; i < sqls.Length; i++)
                        {
                            try
                            {
                                SqlCommand cmd = new SqlCommand(sqls[i], conn);
                                cmd.ExecuteNonQuery();
                            }
                            catch { ok = false; }
                        }
                    }

                }
                catch (Exception ex)
                {
                    ok = false;
                }
                finally
                {
                    conn.Close();
                }
            }
            return ok;
        }

    }
}
