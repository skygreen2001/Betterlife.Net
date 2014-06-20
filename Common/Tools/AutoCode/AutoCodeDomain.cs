using System;
using System.Collections.Generic;
using System.IO;
using Util.Common;

namespace Tools.AutoCode
{
    /// <summary>
    /// 工具类:自动生成代码-实体类
    /// </summary>
    public class AutoCodeDomain:AutoCode
    {
        /// <summary>
        /// 运行主程序
        /// 1.生成实体类
        /// 2.生成上下文环境类
        /// 3.生成枚举类
        /// 4.实体类有外键的实体类需要生成HttpData文件
        /// </summary>
        public void Run()
        {
            base.Init();

            // 1.生成实体类
            Save_Dir = App_Dir + "Core" + Path.DirectorySeparatorChar + "Database" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
            CreateDomain();

            // 2.生成上下文环境类
            Save_Dir = App_Dir + "Core" + Path.DirectorySeparatorChar + "Database" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
            CreateContext();

            // 3.生成枚举类
            Save_Dir = App_Dir + "Core" + Path.DirectorySeparatorChar + "Database" + Path.DirectorySeparatorChar + "Domain" + Path.DirectorySeparatorChar + "Enums" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
            CreateEnum();

            // 4.实体类有外键的实体类需要生成HttpData 文件
            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "HttpData" + Path.DirectorySeparatorChar + "Core"+ Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
            CreateHttpData();
        }

        /// <summary>
        /// 1.生成实体类【多个文件】
        /// [模板文件]:domain/domain.txt            
        /// [生成文件名]:ClassName
        /// [生成文件后缀名]:.cs
        /// </summary>
        private void CreateDomain()
        {
            string ClassName = "Admin";
            string Table_Comment = "系统管理员";
            string Template_Name,Unit_Template, Content, Content_New;

            string UnitColumnDefine;
            string Column_Name, Column_Type,Column_Comment;
            foreach (string Table_Name in TableList)
            {
                //读取原文件内容到内存
                Template_Name = @"AutoCode/Model/domain/domain.txt";
                Content = UtilFile.ReadFile2String(Template_Name);
                ClassName = Table_Name;
                Table_Comment = TableInfoList[Table_Name]["Comment"];
                string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (t_c.Length > 1) {
                    Table_Comment="";
                    foreach (string Comment_Single_Line in t_c)
                    {
                        Table_Comment += "    /// "+Comment_Single_Line+"\r\n";
                    }
                }else{
                    Table_Comment= "    /// "+Table_Comment;
                }
                Content_New = Content.Replace("{$ClassName}", ClassName);
                Content_New = Content_New.Replace("{$Table_Comment}", Table_Comment);

                Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
                UnitColumnDefine = "";;
                foreach (KeyValuePair<String, Dictionary<string, string>> entry in FieldInfo)
                {
                    Column_Name = entry.Key;
                    Column_Comment = entry.Value["Comment"];
                    Column_Type = entry.Value["Type"];
                    string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length > 1)
                    {
                        Column_Comment = "";
                        foreach (string Comment_Single_Line in c_c)
                        {
                            Column_Comment += "        /// " + Comment_Single_Line + "\r\n";
                        }
                        Column_Comment = Column_Comment.Substring(0,Column_Comment.Length-2);
                    }
                    else
                    {
                        Column_Comment = "        /// " + Column_Comment;
                    }
                    Unit_Template = 
@"        /// <summary>
{$Column_Comment}
        /// </summary>
        public {$Column_Type} {$Column_Name} { get; set; }
";
                    Column_Type = ColumnTypeByDbDefine(Column_Type, Column_Name);
                    Unit_Template = Unit_Template.Replace("{$Column_Comment}", Column_Comment);
                    Unit_Template = Unit_Template.Replace("{$Column_Type}", Column_Type);
                    Unit_Template = Unit_Template.Replace("{$Column_Name}", Column_Name);

                    UnitColumnDefine += Unit_Template;
                }
                Content_New = Content_New.Replace("{$ColumnDefines}", UnitColumnDefine);

                //存入目标文件内容
                UtilFile.WriteString2File(Save_Dir + ClassName + ".cs", Content_New);
            }
        }

        /// <summary>
        /// 返回Domain属性数据类型
        /// </summary>
        /// <param name="Column_Type">列类型</param>
        /// <param name="Column_Name">列名称</param>
        private string ColumnTypeByDbDefine(string Column_Type, string Column_Name)
        {
            switch (Column_Type)
            {
                case "nvarchar":
                case "char":
                    return "string";
                case "int":
                    if (Column_Name.Contains("ID"))
                    {
                        return "int";
                    }
                    else
                    {
                        return "Nullable<int>";
                    }
                case "datetime":
                    return "Nullable<System.DateTime>";
            }
            return "string";

        }

        /// <summary>
        /// 2.生成上下文环境类【1个文件】
        /// [模板文件]:domain/context.txt
        /// 生成文件名称:BetterlifeNetEntities.Context.cs[EntitiesName+".Context.cs"]
        /// </summary>
        private void CreateContext()
        {
            string EntitiesName = "BetterlifeNetEntities";
            string ClassName = "Admin";
            string Table_Comment = "系统管理员";
            string Template_Name, Unit_Template, Content, MainContent;

            //读取原文件内容到内存
            Template_Name = @"AutoCode/Model/domain/context.txt";
            Content = UtilFile.ReadFile2String(Template_Name);
            MainContent = "";
            foreach (string Table_Name in TableList)
            {
                ClassName = Table_Name;
                Table_Comment = TableInfoList[Table_Name]["Comment"];
                string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (t_c.Length > 1) Table_Comment = t_c[0];

                Unit_Template = @"
        /// <summary>
        /// {$Table_Comment}
        /// </summary>
        public DbSet<{$ClassName}> {$ClassName} { get; set; }
                ";
                
                Unit_Template = Unit_Template.Replace("{$ClassName}", ClassName);
                MainContent += Unit_Template.Replace("{$Table_Comment}", Table_Comment);
            }

            Content = Content.Replace("{$EntitiesName}", EntitiesName);
            Content = Content.Replace("{$MainContent}", MainContent);
            //存入目标文件内容
            UtilFile.WriteString2File(Save_Dir + EntitiesName+".Context.cs", Content);


        }

        /// <summary>
        /// 3.生成枚举类【一个文件】
        /// [模板文件]:domain/enum.txt
        /// 生成文件名称:Enum.cs
        /// </summary>
        private void CreateEnum()
        {
            string ClassName = "Admin";
            string Table_Comment = "系统管理员";
            string Template_Name, Content;
            
            string Unit_Template;
            string EnumDefineBlock, EnumD2VBlock, EnumV2DBlock;
            string Column_Name, Column_Type, Column_Comment, MainContent;
            List<Dictionary<string, string>> Enum_ColumnDefine;
            MainContent = "";
            foreach (string Table_Name in TableList)
            {
                //读取原文件内容到内存
                ClassName = Table_Name;
                Table_Comment = TableInfoList[Table_Name]["Comment"];

                Template_Name = @"AutoCode/Model/domain/enum.txt";
                Content = UtilFile.ReadFile2String(Template_Name);

                Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
                foreach (KeyValuePair<String, Dictionary<string, string>> entry in FieldInfo)
                {
                    Column_Name = entry.Key;
                    Column_Comment = entry.Value["Comment"];
                    Column_Type = entry.Value["Type"];

                    if (Column_Type.Equals("char"))
                    {
                        string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        if (c_c.Length > 1)
                        {
                            Enum_ColumnDefine = EnumDefines(Column_Comment);
                            if ((Enum_ColumnDefine != null) && (Enum_ColumnDefine.Count>0))
                            {
                                EnumDefineBlock = ""; EnumD2VBlock = ""; EnumV2DBlock = "";
                                foreach (Dictionary<string, string> ColumnDefine in Enum_ColumnDefine)
                                {
                                    ColumnDefine["Name"] = UtilString.UcFirst(ColumnDefine["Name"]);
                                    EnumDefineBlock += "        /// <summary>\r\n" +
                                                      "        /// " + ColumnDefine["Comment"] + "\r\n" +
                                                      "        /// </summary>\r\n" +
                                                      "        public const char " + ColumnDefine["Name"] + " = '" + ColumnDefine["Value"] + "';\r\n";
                                    EnumD2VBlock += "                case " + ColumnDefine["Name"] + ":\r\n" +
                                                    "                    return \"" + ColumnDefine["Comment"] + "\";\r\n";
                                    EnumV2DBlock += "                case \"" + ColumnDefine["Comment"] + "\":\r\n" +
                                                    "                    result= " + ColumnDefine["Name"] + ";\r\n" +
                                                    "                    break;\r\n";

                                }

                                Unit_Template = @"
    /// <summary>
    /// {$Column_Comment}
    /// </summary>
    public class Enum{$Column_Name}
    {
{$EnumDefineBlock}

        /// <summary>
        /// 显示{$Column_Comment}
        /// </summary>
        public static String {$Column_Name}Show(char Type)
        {
            switch (Type)
            {
{$EnumD2VBlock}
            }
            return ""未知"";
        }

        /// <summary>
        /// 根据{$Column_Comment}显示文字获取{$Column_Comment}
        /// </summary>
        public static string {$Column_Name}ByShow(string Content)
        {
            char result=char.MinValue;
            switch (Content)
            {
{$EnumV2DBlock}
            }
            return result.ToString();
        }
    }
";
                                Unit_Template = Unit_Template.Replace("{$EnumDefineBlock}", EnumDefineBlock);
                                Unit_Template = Unit_Template.Replace("{$EnumD2VBlock}", EnumD2VBlock);
                                Unit_Template = Unit_Template.Replace("{$EnumV2DBlock}", EnumV2DBlock);

                                Unit_Template = Unit_Template.Replace("{$Column_Name}", Column_Name);
                                Column_Comment = c_c[0];
                                Unit_Template = Unit_Template.Replace("{$Column_Comment}", Column_Comment);
                                MainContent += Unit_Template;
                            }
                        }
                    }
                }
                Content = Content.Replace("{$MainContent}", MainContent);
                
                //存入目标文件内容
                UtilFile.WriteString2File(Save_Dir + "Enum.cs", Content);
            }
        }

        /// <summary>
        /// 4.实体类有外键的实体类需要生成HttpData 文件
        ///   例如Admin有外键Department_ID,会生成Department的HttpData类【多个文件】
        /// [模板文件]:domain/httpdata.txt|domain/httpdatadefine.txt
        /// [生成文件名称]:ClassName|ClassName
        /// [生成文件后缀名]:.ashx.cs|.ashx"
        /// </summary>
        private void CreateHttpData()
        {
            string ClassName = "Admin";
            string InstanceName = "admin";
            string Table_Comment = "系统管理员";
            string Relation_Table_Name, Relation_Column_Name="";
            string Template_Name, Content, Content_New;

            string Column_Name, Column_Comment;
            foreach (string Table_Name in TableList)
            {
                Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
                foreach (KeyValuePair<String, Dictionary<string, string>> entry in FieldInfo)
                {
                    Column_Name = entry.Key;
                    Column_Comment = entry.Value["Comment"];
                    if (Column_Name.Contains("_ID"))
                    {
                        ClassName = Column_Name.Replace("_ID", "");
                        if (TableList.Contains(ClassName))
                        {
                            //读取原文件内容到内存
                            Template_Name = @"AutoCode/Model/domain/httpdata.txt";
                            Content = UtilFile.ReadFile2String(Template_Name);

                            Table_Comment = TableInfoList[ClassName]["Comment"];
                            string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (t_c.Length > 1) Table_Comment = t_c[0];
                            InstanceName = UtilString.LcFirst(ClassName);

                            Relation_Table_Name=ClassName;

                            Dictionary<string, Dictionary<string, string>> Relation_FieldInfo=FieldInfos[Relation_Table_Name];
                            foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                            {
                                Relation_Column_Name = relation_entry.Key;
                                if (UtilString.Contains(relation_entry.Key.ToUpper(),"NAME","TITLE","URL"))break;
                            }

                            Content_New = Content.Replace("{$ClassName}", ClassName);
                            Content_New = Content_New.Replace("{$Table_Comment}", Table_Comment);
                            Content_New = Content_New.Replace("{$InstanceName}", InstanceName);
                            Content_New = Content_New.Replace("{$Column_Name}", Column_Name);
                            Content_New = Content_New.Replace("{$Relation_Column_Name}", Relation_Column_Name);

                            //存入目标文件内容
                            UtilFile.WriteString2File(Save_Dir + ClassName + ".ashx.cs", Content_New);


                            //读取原文件内容到内存
                            Template_Name = @"AutoCode/Model/domain/httpdatadefine.txt";
                            Content = UtilFile.ReadFile2String(Template_Name);
                            Content_New = Content.Replace("{$ClassName}", ClassName);

                            //存入目标文件内容
                            UtilFile.WriteString2File(Save_Dir + ClassName + ".ashx", Content_New);
                        }
                    }

                }
            }
        }
    }
}
