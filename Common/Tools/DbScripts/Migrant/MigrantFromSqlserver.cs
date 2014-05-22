using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Util.Db;
using Util.Common;

namespace Tools.DbScripts.Migrant
{
    /// <summary>
    /// 参考框架Bettlife.Net->betterlife
    /// 用于betterlife生成Betterlife.Net后台所需的Extjs的JS代码
    /// 两种数据库之间的规格进行betterlife使用的Mysql数据库到Betterlife.Net使用的Sqlserver数据库的移植
    /// </summary>
    /// <see cref="https://github.com/skygreen2001/betterlife"/>
    public static class MigrantFromSqlserver
    {
        /// <summary>
        /// Sqserver的数据库表信息
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> tableInfos;
        /// <summary>
        /// 所有的表名列表
        /// </summary>
        private static List<String> tablenames = new List<String>();
        /// <summary>
        /// 默认数据库
        /// </summary>
        private const string sampleDb = "BetterlifeNet";
        /// <summary>
        /// 默认初始化SQL脚本
        /// </summary>
        private const string initDataFile = "DbScripts\\initdata.txt";
        /// <summary>
        /// 移植数据库脚本
        /// </summary>
        public static string run()
        {
            string result = "";

            tableInfos = UtilSqlserver.TableinfoList();

            //生成数据库主体部分
            result = CreateDbDefine();

            return result;
        }

        /// <summary>
        /// 生成删除数据库所有表的脚本
        /// </summary>
        /// <returns></returns>
        public static string DropAllTables()
        {

            tableInfos = UtilSqlserver.TableinfoList();
            string result = "/****** 删除数据库所有表的脚本    Script Date:" + DateTime.Now + " ******/\r\n";
            string tablename;
            string sql_template, sql_refer_tempalte, sql_df_template;
            Dictionary<string, Dictionary<string, string>> columnInfos;
            sql_template = @"
/****** Object:  Table [dbo].[{0}]    Script Date: 08/03/2013 21:46:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'U'))
DROP TABLE [dbo].[{0}]
GO
";
            sql_refer_tempalte = @"
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND parent_object_id = OBJECT_ID(N'[dbo].[{1}]'))
ALTER TABLE [dbo].[{1}] DROP CONSTRAINT [{0}]
GO
";
            sql_df_template = @"
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_{0}_{1}]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[{0}] DROP CONSTRAINT [DF_{0}_{1}]
END

GO
";

            Dictionary<string, Dictionary<string, string>> table_fk = UtilSqlserver.Table_Foreign_Key();
            string fkname, ftablename;
            foreach (Dictionary<string, string> table_fkInfo in table_fk.Values)
            {
                fkname = table_fkInfo["fkname"];
                ftablename = table_fkInfo["ftablename"];
                //fkcol = table_fkInfo["fkcol"];
                //rtable = table_fkInfo["rtable"];

                result += string.Format(sql_refer_tempalte, fkname, ftablename);
            }

            ArrayList allTbNames = new ArrayList(tableInfos.Keys.Count());
            foreach (Dictionary<string, string> tableInfo in tableInfos.Values)
            {
                //获取表名
                tablename = tableInfo["Name"];
                tablename = UtilString.UcFirst(tablename);
                columnInfos = UtilSqlserver.FieldInfoList(tablename);
                allTbNames.Add(tablename);
                string column_name;

                //获取主键名称
                foreach (Dictionary<string, string> columnInfo in columnInfos.Values)
                {
                    column_name = columnInfo["Field"];
                    column_name = UtilString.UcFirst(column_name);
                    if ((column_name.ToUpper().Equals("COMMITTIME")) || (column_name.ToUpper().Equals("UPDATETIME")))
                    {
                        result += string.Format(sql_df_template, tablename, column_name);
                    }
                }
                //result += string.Format(sql_template, tablename);
            }
            allTbNames.Sort();
            allTbNames.Reverse();
            foreach (var tbname in allTbNames)
            {
                result += string.Format(sql_template, tbname);
            }
            return result;
        }

        /// <summary>
        /// 生成数据库主体部分
        /// </summary>
        private static string CreateDbDefine()
        {
            string now=DateTime.Now.ToString();
            string result = @"
/*
Betterlife.Net Sqlserver Convert to Mysql
Source Server         : localhost_3306
Source Server Version : 50520
Source Host           : localhost:3306
Source Database       :  {0}

Target Server Type    : MYSQL
Target Server Version : 50520
File Encoding         : 65001

Date: {1}
*/  
SET FOREIGN_KEY_CHECKS=0;       
";
            result = string.Format(result, UtilSqlserver.Database_Name, now);
            string tablename, tableComment, columnDefine;
            string sqlTemplate, column_name,column_comment,column_type, column_default;
            string foreign_key;
            List<string> primary_keys;
            Dictionary<string, Dictionary<string, string>> columnInfos;
            foreach (Dictionary<string, string> tableInfo in tableInfos.Values)
            {
                //获取表名
                tablename = tableInfo["Name"];
                tablename = UtilString.UcFirst(tablename);
                tableComment = tableInfo["Comment"];
                tableComment = tableComment.Replace("\r", "\\r");
                tableComment = tableComment.Replace("\n", "\\n");

                columnInfos = UtilSqlserver.FieldInfoList(tablename);
                columnDefine = "";
                column_default = "";
                primary_keys = new List<string>();
                //获取主键名称
                foreach (Dictionary<string, string> columnInfo in columnInfos.Values)
                {
                    column_name = columnInfo["Field"];
                    column_name = UtilString.UcFirst(column_name);

                    column_comment = columnInfo["Comment"];
                    if (column_name.ToUpper().Equals("ID"))
                    {
                        columnDefine += "     'ID' int(11) NOT NULL AUTO_INCREMENT COMMENT '" + column_comment + "',";
                        primary_keys.Add("'ID'");
                    }
                    else
                    {
                        //获取列名|列类型|是否Null
                        column_type = columnInfo["Type"];
                        column_type = ConvertType(column_type);

                        //column_default = columnInfo["Default"];
                        if (column_name.ToUpper().Contains("_ID"))
                        {
                            column_type = "int(11) NOT NULL ";
                            primary_keys.Add("'"+column_name+"'");
                        }
                        else
                        {
                            if (columnInfo["Null"].Equals("YES")) column_default = "DEFAULT NULL";
                            if (UtilString.Contains(column_name.ToUpper(), "TIME", "DATE"))
                            {
                                if (UtilString.Contains(column_name.ToUpper(), "TIMES"))
                                {
                                    column_type = "int";
                                    column_default = "DEFAULT '0'";
                                }
                                else
                                {
                                    column_type = "datetime";
                                }
                            }
                        }

                        string[] type_length = column_type.Split(new char[4] { '[', ']', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                        if (type_length.Length == 2)
                        {
                            string length = type_length[1];
                            int i_len = UtilNumber.Parse(length);
                            if (i_len > 4000)
                            {
                                i_len = 4000;
                                column_type = "" + type_length[0] + "" + "(" + i_len + ")";
                            }
                        }
                        column_comment = column_comment.Replace("\r","\\r");
                        column_comment = column_comment.Replace("\n", "\\n");
                        column_comment = "COMMENT '" + column_comment + "'";
                        columnDefine += string.Format("     '{0}' {1} {2} {3},", column_name, column_type, column_default, column_comment);
                    }
                    columnDefine += "\r\n";
                }
                if(columnDefine.Length>2)columnDefine = columnDefine.Substring(0, columnDefine.Length - 2);
                //生成表脚本
                sqlTemplate = @"
-- ----------------------------
-- Table structure for `{0}`
-- ----------------------------
DROP TABLE IF EXISTS `{0}`;
CREATE TABLE `{0}` (
{1}
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='{2}';

";
                result += string.Format(sqlTemplate, tablename, columnDefine, tableComment);
            }
            return result;
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="oldType">原数据类型</param>
        /// <returns>新数据类型</returns>
        private static string ConvertType(string oldType)
        {
            string newType = oldType;
            string[] otArr = null;
            if (oldType.Contains("("))
            {
                otArr = oldType.Split(new char[2] { '(', ')' });
                oldType = otArr[0];

                switch (oldType)
                {
                    case "varchar":
                        oldType = "nvarchar";
                        break;
                    default:
                        break;
                }

                if ((otArr != null) && (otArr.Count() >= 2))
                {

                    if (oldType.Contains("enum"))
                    {
                        newType = "[char](1)";
                    }
                    else if (oldType.Contains("int"))
                    {
                        newType = "[int]";
                    }
                    else if (oldType.Contains("bit"))
                    {
                        newType = "[bit]";
                    }
                    else
                    {
                        newType = "[" + oldType + "](" + otArr[1] + ")";
                    }
                }
            }
            else
            {
                switch (oldType)
                {
                    case "longtext":
                        oldType = "text";
                        break;
                    default:
                        break;
                }
                newType = "[" + oldType + "]";
            }
            return newType;
        }

    }
}
