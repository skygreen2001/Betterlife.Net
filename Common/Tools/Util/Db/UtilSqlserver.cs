using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Tools.Util.Db
{
    /// <summary>
    /// 工具类：SQLServer数据库信息
    /// Beginners guide to accessing SQL Server through C#:
    ///     http://www.codeproject.com/Articles/4416/Beginners-guide-to-accessing-SQL-Server-through-C
    /// </summary>
    public class UtilSqlserver:UtilDb
    {
        /// <summary>
        /// 连接数据库字符串
        /// </summary>
        private static string ConnStr = 
                                        "server=(local);" + 
                                        "user id=sa;" +
                                        "pwd=123.com;" +
                                        "database={0}; " +
                                        "Trusted_Connection=yes;" +
                                        "connection timeout=30";//默认超时时间是30秒
        public static string Database_Name = "BetterlifeNet";
        #region T-SQL定义
        /// <summary>
        /// 查看所有数据库
        /// </summary>
        private static string Sql_Databases = "Select Name FROM Master.dbo.SysDatabases";
        /// <summary>
        /// 查看数据库所有的数据表
        /// </summary>
        private static string Sql_Tables = "select name from sysobjects where xtype='u' order by name asc";
        /// <summary>
        /// 查看所有表注释
        /// </summary>
        private static string Sql_Table_Comment = "SELECT objname as name, cast(value as nvarchar(4000)) as comment FROM fn_listextendedproperty('MS_DESCRIPTION','schema', 'dbo', 'table', null, null, null) order by objname asc";
        /// <summary>
        /// 查看指定表所有的列
        /// </summary>
        private static string Sql_Table_Columns = "select * from information_schema.columns where TABLE_NAME='{0}'";
        /// <summary>
        /// 查看指定表所有的列的注释
        /// </summary>
        private static string Sql_Table_Columns_Comment = "SELECT objname as column_name, cast(value as nvarchar(4000)) as comment FROM fn_listextendedproperty('MS_DESCRIPTION','schema', 'dbo', 'table', '{0}', 'column', '{1}')";
        /// <summary>
        /// 查询数据库所有的外键
        /// 参考：http://chenjianjx.iteye.com/blog/222267
        /// 从左到右分别是： 外键约束名，子表名，外键列名，父表名 
        /// </summary>
        private static string Sql_Table_Foreign_Key = @"
                                    select fk.name fkname , ftable.name ftablename, cn.name fkcol, rtable.name rtable from sysforeignkeys 
                                      join sysobjects fk 
                                        on sysforeignkeys.constid = fk.id 
                                      join sysobjects ftable 
                                        on sysforeignkeys.fkeyid = ftable.id 
                                      join sysobjects rtable 
                                        on sysforeignkeys.rkeyid = rtable.id 
                                      join syscolumns cn 
                                        on sysforeignkeys.fkeyid = cn.id and sysforeignkeys.fkey = cn.colid";
        /// <summary>
        /// 获取表的主外键
        /// </summary>
        private static string Sql_PK_DK_FK = @"exec sp_helpconstraint '{0}'";
        /// <summary>
        /// 查看数据库表的个数
        /// </summary>
        //private static string Sql_Table_Count="select COUNT(id) from sysobjects where xtype='u'";
        /// <summary>
        /// 查看数据库所有的列数
        /// </summary>
        //private static string Sql_Table_Columns_Count = "select COUNT(TABLE_NAME) from information_schema.columns";
        /// <summary>
        /// 查看数据库所有的视图
        /// </summary>
        //private static string Sql_Db_View = "select * from information_schema.views order by TABLE_NAME asc";
        /// <summary>
        /// 查看数据库的视图个数
        /// </summary>
        //private static string Sql_Db_View_Count = "select COUNT(TABLE_CATALOG) from information_schema.views";
        /// <summary>
        /// --查看所有的表、视图
        /// </summary>
        //select * from information_schema.tables order by TABLE_NAME asc
        /// <summary>
        /// 查看所有的表、视图的个数
        /// </summary>
        //select COUNT(TABLE_CATALOG) from information_schema.tables
        /// <summary>
        /// 查看所有的自定义存储过程
        /// </summary>
        //SELECT * FROM sys.all_objects WHERE(type='P' OR type='X' OR type='PC') and object_id>0 ORDER BY name
        /// <summary>
        /// 查看所有的自定义存储过程个数
        /// </summary>
        //SELECT count(name) FROM sys.all_objects WHERE(type='P' OR type='X' OR type='PC') and object_id>0
        #endregion

        /// <summary>
        /// 数据库连接
        /// </summary>
        private static SqlConnection myConnection;

        #region 基本操作
        /// <summary>
        /// 连接数据库
        /// </summary>
        private static void Connect()
        {
            string connString = string.Format(ConnStr,Database_Name);
            myConnection = new SqlConnection(connString);
        }

        /// <summary>
        /// 执行sql查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable SqlExecute(string sql)
        {
            DataTable result = new DataTable();
            if (myConnection == null)Connect();
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(sql, myConnection);
                using (SqlDataReader reader = myCommand.ExecuteReader())
                {
                    result.Load(reader);
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (myConnection != null) Close();
            }
            return result;
        }

        /// <summary>
        /// 数据库关闭
        /// </summary>
        private static void Close()
        {
            try
            {
                myConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        #endregion

        #region 查询数据库表信息
        /// <summary>
        /// 设置指定数据库
        /// </summary>
        /// <param name="databaseName"></param>
        public static void SetDatabase(string databaseName)
        {
            Database_Name = databaseName;
            Connect();
        }

        /// <summary>
        /// 返回所有的数据库列表
        /// </summary>
        /// <returns></returns>
        public static List<string> AllDatabaseNames()
        {
            List<string> result = new List<string>();
            DataTable databases = UtilSqlserver.SqlExecute(Sql_Databases);
            string database_name;
            foreach (DataRow item in databases.Rows)
            {
                database_name = (string)item.ItemArray[0];
                result.Add(database_name);
            }
            return result;
        }

        /// <summary>
        /// 查询所有表名
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> TableList()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            DataTable tables = UtilSqlserver.SqlExecute(Sql_Tables);
            string tablename, key;
            foreach (DataRow item in tables.Rows)
            {
                tablename = (string)item.ItemArray[0];
                key = tablename.ToLower();
                result[key] = tablename;
            }
            return result;
        }

        /// <summary>
        /// 获取所有的表信息包括表注释信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string,string>> TableinfoList()
        {
            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
            DataTable tables = UtilSqlserver.SqlExecute(Sql_Table_Comment);
            string tablename,key;
            foreach (DataRow item in tables.Rows)
            {
                tablename = (string)item.ItemArray[0];
                Dictionary<string, string> tableInfo = new Dictionary<string, string>();
                tableInfo["Name"] = tablename;
                tableInfo["Comment"] = (string)item.ItemArray[1];
                key = tablename.ToLower();
                result[key] = tableInfo;
            }
            return result;
        }

        /// <summary>
        /// 获取指定表所有的列信息
        /// </summary>
        /// <param name="table_name"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> FieldInfoList(string table_name)
        {
            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();

            string sql = string.Format(Sql_Table_Columns,table_name);
            DataTable columns = UtilSqlserver.SqlExecute(sql);
            string column_name,column_type;
            foreach (DataRow item in columns.Rows)
            {
                Dictionary<string, string> columnInfo = new Dictionary<string, string>();
                column_name = (string)item.ItemArray[3];
                columnInfo["Field"] = column_name;
                column_type=(string)item.ItemArray[7];
                columnInfo["Type"] = column_type;
                var nullYN = (string)item.ItemArray[6];
                if (nullYN.Contains("YES")) nullYN = "是";else nullYN = "否";

                columnInfo["Null"] = nullYN;
                if (column_type.Contains("char"))
                {
                    var len = item.ItemArray[8];
                    columnInfo["Length"] = len.ToString();
                }
                else if (column_type.Contains("numeric") || (column_type.Contains("int")))
                {
                    var len = item.ItemArray[10];
                    columnInfo["Length"] = len.ToString();
                }
                else 
                {
                    columnInfo["Length"] = "";
                }
                var fkPk = "";
                if (column_name.Equals("ID")) fkPk = "PK";
                if (column_name.Contains("_ID")) fkPk = "FK";
                columnInfo["Fkpk"] = fkPk;
                sql = string.Format(Sql_Table_Columns_Comment,  table_name, column_name);
                DataTable column_comment = UtilSqlserver.SqlExecute(sql);
                foreach (DataRow item_c in column_comment.Rows)
                {
                    columnInfo["Comment"] = (string)item_c.ItemArray[1];
                }
                result[column_name] = columnInfo;
            }
            return result;
        }

        /// <summary>
        /// 查询数据库所有的外键
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> Table_Foreign_Key()
        {
            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
            DataTable tables = UtilSqlserver.SqlExecute(Sql_Table_Foreign_Key);
            string tablename;
            Dictionary<string, string> foreignKeyInfo;
            foreach (DataRow item in tables.Rows)
            {
                tablename = (string)item.ItemArray[1];
                foreignKeyInfo=new Dictionary<string,string>();
                ///外键约束名，子表名，外键列名，父表名
                foreignKeyInfo["fkname"] = (string)item.ItemArray[0];
                foreignKeyInfo["ftablename"] = (string)item.ItemArray[1];
                foreignKeyInfo["fkcol"] = (string)item.ItemArray[2];
                foreignKeyInfo["rtable"] = (string)item.ItemArray[3];

                result[tablename] = foreignKeyInfo;
            }
            return result;
        }

        /// <summary>
        /// 获取数据库所有的主键外键默认值
        /// </summary>
        /// <param name="table_name"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> Table_FK_PK_DK(string table_name)
        {
            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
            string sql_fkpkdk = string.Format(Sql_PK_DK_FK,table_name);
            DataTable tables = UtilSqlserver.SqlExecute(sql_fkpkdk);
            Dictionary<string, string> fkpkdkInfo;
            foreach (DataRow item in tables.Rows)
            {
                fkpkdkInfo = new Dictionary<string, string>();
                ///外键约束名，子表名，外键列名，父表名
                fkpkdkInfo["constrant_type"] = (string)item.ItemArray[0];
                fkpkdkInfo["constrant_name"] = (string)item.ItemArray[1];
                fkpkdkInfo["constrant_keys"] = (string)item.ItemArray[6];

                result[table_name] = fkpkdkInfo;
            }
            return result;

        }
        #endregion

    }
}
