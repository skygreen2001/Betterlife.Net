using System;
using System.Collections.Generic;
using System.IO;
using Util.Common;

namespace Tools.AutoCode
{
    /// <summary>
    /// 工具类:自动生成代码-服务类
    ///      本框架中包括两种:
    ///      1.Core/Service服务层所有的服务业务类
    ///      2.Business/Admin后台所有ExtService服务类
    /// </summary>
    public class AutoCodeService:AutoCode
    {
        /// <summary>
        /// 服务类生成定义的方式
        /// 1.Core/Service服务层所有的服务业务类|接口
        /// 2.Business/Admin后台所有ExtService服务类
        /// </summary>
        public int ServiceType;

        /// <summary>
        /// 运行主程序
        /// </summary>
        public void Run()
        {
            base.Init();
            //1.Core/Service服务层所有的服务业务类
            Save_Dir = App_Dir + "Core" + Path.DirectorySeparatorChar + "Business" + Path.DirectorySeparatorChar + "Service" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);            
            if (ServiceType == 1) CreateNormalService();
            //2.Business/Admin后台所有ExtService服务类
            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "Services" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
            if (ServiceType == 2) CreateExtService();
        }

        /// <summary>
        ///  1.Core/Service服务层所有的服务业务类|接口【多个文件】
        /// [模板文件]:service/service.txt|service/iservice.txt
        /// [生成文件名称]:"Service"+ClassName|"IService"+ClassName
        /// [生成文件后缀名]:.cs
        /// </summary>
        private void CreateNormalService()
        {
            string ClassName = "Admin";
            string InstanceName = "admin";
            string Table_Comment = "系统管理员";
            string Template_Name, Content, Content_New;
            string ID_Type, ID_Default_Value;
            foreach (string Table_Name in TableList)
            {
                //读取原文件内容到内存
                Template_Name = @"AutoCode/Model/service/iservice.txt";
                Content = UtilFile.ReadFile2String(Template_Name);
                ClassName = Table_Name;
                Table_Comment = TableInfoList[Table_Name]["Comment"];
                string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (t_c.Length > 1) Table_Comment = t_c[0];
                InstanceName = UtilString.LcFirst(ClassName);

                Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
                Dictionary<string, string> entry=FieldInfo["ID"];
                if (entry["Type"].Equals("uniqueidentifier"))
                {
                    ID_Type = "System.Guid";
                    ID_Default_Value = "null";
                }
                else
                {
                    ID_Type = "int";
                    ID_Default_Value = "0";
                }

                Content_New = Content.Replace("{$ClassName}", ClassName);
                Content_New = Content_New.Replace("{$Table_Comment}", Table_Comment);
                Content_New = Content_New.Replace("{$InstanceName}", InstanceName);
                Content_New = Content_New.Replace("{$ID_Type}", ID_Type);

                //存入目标文件内容
                UtilFile.WriteString2File(Save_Dir + "IService" + ClassName + ".cs", Content_New);

                //读取原文件内容到内存
                Template_Name = @"AutoCode/Model/service/service.txt";
                Content = UtilFile.ReadFile2String(Template_Name);
                Content_New = Content.Replace("{$ClassName}", ClassName);
                Content_New = Content_New.Replace("{$Table_Comment}", Table_Comment);
                InstanceName = UtilString.LcFirst(ClassName);
                Content_New = Content_New.Replace("{$InstanceName}", InstanceName);
                Content_New = Content_New.Replace("{$ID_Type}", ID_Type);
                Content_New = Content_New.Replace("{$ID_Default_Value}", ID_Default_Value);

                //存入目标文件内容
                UtilFile.WriteString2File(Save_Dir + "Service" + ClassName + ".cs", Content_New);
            }
        }

        /// <summary>
        ///  2.Business/Admin后台所有ExtService服务类【多个文件】
        /// [模板文件]:service/extservice.txt|service/extservicedefine.txt
        /// [生成文件名称]:"ExtService"+ClassName|"ExtService"+ClassName
        /// [生成文件后缀名]:.ashx.cs|.ashx
        /// </summary>
        private void CreateExtService()
        {
            string ClassName = "Admin";
            string InstanceName = "admin";
            string Table_Comment = "系统管理员";
            string Template_Name, Content, Content_New;

            string ColumnNameComment, ColumnCommentName;
            string Column_Name, Column_Comment;
            foreach (string Table_Name in TableList)
            {
                //读取原文件内容到内存
                Template_Name = @"AutoCode/Model/service/extservice.txt";
                Content = UtilFile.ReadFile2String(Template_Name);
                ClassName = Table_Name;
                Table_Comment = TableInfoList[Table_Name]["Comment"];
                string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (t_c.Length > 1) Table_Comment = t_c[0];
                InstanceName = UtilString.LcFirst(ClassName);

                Content_New = Content.Replace("{$ClassName}", ClassName);
                Content_New = Content_New.Replace("{$Table_Comment}", Table_Comment);
                Content_New = Content_New.Replace("{$InstanceName}", InstanceName);

                Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
                ColumnNameComment = ""; ColumnCommentName = "";
                foreach (KeyValuePair<String, Dictionary<string, string>> entry in FieldInfo)
                {
                    Column_Name = entry.Key;
                    Column_Comment = entry.Value["Comment"];
                    string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length > 1) Column_Comment = c_c[0];
                    ColumnNameComment += "                    {\"" + Column_Name + "\",\"" + Column_Comment + "\"},\r\n";
                    ColumnCommentName += "                    {\"" + Column_Comment + "\",\"" + Column_Name + "\"},\r\n";
                }
                ColumnNameComment = ColumnNameComment.Substring(0, ColumnNameComment.Length - 3);
                ColumnCommentName = ColumnCommentName.Substring(0, ColumnCommentName.Length - 3);

                Content_New = Content_New.Replace("{$ColumnNameComment}", ColumnNameComment);
                Content_New = Content_New.Replace("{$ColumnCommentName}", ColumnCommentName);

                //存入目标文件内容
                UtilFile.WriteString2File(Save_Dir + "ExtService" + ClassName + ".ashx.cs", Content_New);

                //读取原文件内容到内存
                Template_Name = @"AutoCode/Model/service/extservicedefine.txt";
                Content = UtilFile.ReadFile2String(Template_Name);
                Content_New = Content.Replace("{$ClassName}", ClassName);

                //存入目标文件内容
                UtilFile.WriteString2File(Save_Dir + "ExtService" + ClassName + ".ashx", Content_New);
            }
        }

    }
}
