using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Tools.Util.Db;
using System.Data;
using Util.Common;
using Tools.DbScripts.Migrant;

namespace Tools.DbScripts
{
    /// <summary>
    /// 通过代码生成常用的SQL脚本工具
    /// </summary>
    public static class ToolDbScripts
    {
        #region Mysql
        /// <summary>
        /// 移植数据库脚本:从Mysql到Sqlserver脚本
        /// </summary>
        public static string DbCreateMigrantFromMysql()
        {
            return MigrantFromMysql.run();
        }
        #endregion

        #region Sqlserver
        /// <summary>
        /// 移植数据库脚本:从Sqlserver到Mysql脚本
        /// </summary>
        public static string DbCreateMigrantFromSqlserver()
        {
            return MigrantFromSqlserver.run();
        }

        /// <summary>
        /// 生成删除数据库所有表的脚本
        /// </summary>
        /// <returns></returns>
        public static string DropAllTables()
        {

            return MigrantFromSqlserver.DropAllTables();
        }
        #endregion
    }
}
