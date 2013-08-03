using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Tools.Util.Db;
using System.Data;
using Util.Common;

namespace Tools.DbScripts
{
    /// <summary>
    /// 通过代码生成常用的SQL脚本工具
    /// </summary>
    public static class ToolDbScripts
    {
        private static Dictionary<string, Dictionary<string, string>> tableInfos;
        /// <summary>
        /// 移植数据库脚本
        /// </summary>
        public static string MigrantFromMysql()
        {
            string result = "";

            tableInfos = UtilMysql.TableinfoList();

            //生成数据库主体部分
            result = CreateDbDefine();

            //生成数据库注释部分
            result += CreateDbComment();

            //修改数据库主键部分
            result += ModfiyDbKeyDefine();

            return result;
        }

        /// <summary>
        /// 修改数据库主键部分
        /// </summary>
        private static string ModfiyDbKeyDefine()
        {
            return "";
        }

        /// <summary>
        /// 生成数据库注释部分
        /// </summary>
        private static string CreateDbComment()
        {
            string result = "/****** 创建数据库所有表的注释说明备注    Script Date:" + DateTime.Now + " ******/\r\n";

            string tablename, tableComment;
            string column_name,column_comment;
            foreach (Dictionary<string, string> tableInfo in tableInfos.Values)
            {
                //获取表名
                tablename = tableInfo["Name"];
                tablename = UtilString.UcFirst(tablename);
                tableComment = tableInfo["Comment"];
                result+=string.Format("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{0}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{1}'\r\nGO\r\n",tableComment,tablename);

                Dictionary<string, Dictionary<string, string>> columnInfos;
                columnInfos = UtilMysql.FieldInfoList(tablename);
                //获取主键名称
                foreach (Dictionary<string, string> columnInfo in columnInfos.Values)
                {
                    column_name = columnInfo["Field"];
                    column_name = UtilString.UcFirst(column_name);
                    column_comment = columnInfo["Comment"];
                    result+=string.Format("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{0}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{1}', @level2type=N'COLUMN',@level2name=N'{2}'\r\nGO\r\n",column_comment,tablename,column_name);
                }
            }
            return result;
        }

        /// <summary>
        /// 生成数据库主体部分
        /// </summary>
        private static string CreateDbDefine()
        {
            string result = "/****** 创建数据库所有表    Script Date:" + DateTime.Now + " ******/\r\n";
            result += "USE " + UtilMysql.Database_Name + "\r\n";
            string tablename, tableComment, columnDefine;
            string sqlTemplate, column_name, column_type, column_null,column_default;
            string defaultValue;
            foreach (Dictionary<string, string> tableInfo in tableInfos.Values)
            {
                //获取表名
                tablename = tableInfo["Name"];
                tablename = UtilString.UcFirst(tablename);
                tableComment = tableInfo["Comment"];
                Dictionary<string, Dictionary<string, string>> columnInfos;
                columnInfos = UtilMysql.FieldInfoList(tablename);
                columnDefine = "";
                defaultValue = "";
                //获取主键名称
                foreach (Dictionary<string, string> columnInfo in columnInfos.Values)
                {
                    column_name = columnInfo["Field"];
                    column_name = UtilString.UcFirst(column_name);
                    if (column_name.ToUpper().Equals("ID"))
                    {
                        columnDefine += "[ID] [uniqueidentifier] NOT NULL,";
                        defaultValue += string.Format("ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [DF_{0}_ID]  DEFAULT (newid()) FOR [ID]\r\nGO\r\n",tablename);
                    }
                    else
                    {
                        //获取列名|列类型|是否Null
                        column_type = columnInfo["Type"];
                        column_type = ConvertType(column_type);
                        column_null = (columnInfo["Null"].Equals("YES")) ? "NULL" : "NOT NULL";

                        column_default = columnInfo["Default"];
                        if (column_name.ToUpper().Contains("_ID"))
                        {
                            column_type = "[uniqueidentifier]";
                            column_null = "NOT NULL";
                        }
                        if (UtilString.Contains(column_name.ToUpper(),"TIME","DATE"))
                        {
                            column_type = "[datetime]";
                        }

                        if (!string.IsNullOrEmpty(column_default) && (!column_name.ToUpper().Equals("UPDATETIME")))
                        {
                            if (!column_name.ToUpper().Contains("_ID"))
                            {
                                defaultValue += string.Format("ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [DF_{0}_{1}]  DEFAULT ({2}) FOR [{1}]\r\nGO\r\n", tablename, column_name, column_default);
                            }
                        }

                        if ((column_name.ToUpper().Equals("COMMITTIME"))||(column_name.ToUpper().Equals("UPDATETIME")))
                        {
                            defaultValue += string.Format("ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [DF_{0}_{1}]  DEFAULT (getdate()) FOR [{1}]\r\nGO\r\n",tablename,column_name);
                        }

                        columnDefine += string.Format("     [{0}]  {1}  {2},", column_name, column_type, column_null);
                    }
                    columnDefine += "\r\n";
                }
                columnDefine = columnDefine.Substring(0, columnDefine.Length - 2);
                //生成表脚本
                sqlTemplate = @"
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[{0}] (
     {1}
 CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED ([ID] ASC)
 WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)ON [PRIMARY]
) ON [PRIMARY]
GO

";
                result += string.Format(sqlTemplate, tablename, columnDefine);

                //生成列默认值
                result += defaultValue;
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
            string newType=oldType;
            string[] otArr=null;
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
                        newType = "[smallint]";
                    }else if (oldType.Contains("int")){
                        newType = "[int]";
                    }else
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
