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
        /// 1a.生成实体类分部类(显示属性)
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

            // 1a.生成实体类分部类(显示属性)
            Save_Dir = App_Dir + "Core" + Path.DirectorySeparatorChar + "Database" + Path.DirectorySeparatorChar + "Domain" + Path.DirectorySeparatorChar + "Core" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
            CreateDomainPartial();

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
            string Relation_ClassName, Relation_InstanceName, Relation_Table_Name;
            string Relation_Unit_Template = "", Relation_Table_Comment = "", Relation_UnitColumnDefine = "";//相关列数据对象定义
            string OneHasMany_Table_Comment="",OneHasMany_Unit_Template="", OneHasMany_UnitColumnDefine = "";
            foreach (string Table_Name in TableList)
            {
                //读取原文件内容到内存
                Template_Name = @"AutoCode/Model/domain/domain.txt";
                Content = UtilFile.ReadFile2String(Template_Name);
                ClassName = Table_Name;
                if (TableInfoList.ContainsKey(Table_Name))
                {
                    Table_Comment = TableInfoList[Table_Name]["Comment"];
                    string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (t_c.Length > 1)
                    {
                        Table_Comment = "";
                        foreach (string Comment_Single_Line in t_c)
                        {
                            Table_Comment += "    /// " + Comment_Single_Line + "\r\n";
                        }
                    }
                    else
                    {
                        Table_Comment = "    /// " + Table_Comment;
                    }
                    Content_New = Content.Replace("{$ClassName}", ClassName);
                    Content_New = Content_New.Replace("{$Table_Comment}", Table_Comment);

                    Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
                    UnitColumnDefine = "";
                    Relation_UnitColumnDefine = ""; OneHasMany_UnitColumnDefine = "";
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
                            Column_Comment = Column_Comment.Substring(0, Column_Comment.Length - 2);
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
                        if (Column_Name.Contains("_ID"))
                        {
                            Relation_ClassName = Column_Name.Replace("_ID", "");
                            if (TableList.Contains(Relation_ClassName))
                            {
                                //读取原文件内容到内存
                                Template_Name = @"AutoCode/Model/domain/httpdata.txt";
                                Content = UtilFile.ReadFile2String(Template_Name);
                                Relation_InstanceName = UtilString.LcFirst(Relation_ClassName);
                                Relation_Table_Name = Relation_ClassName;
                                if (TableInfoList.ContainsKey(Relation_Table_Name))
                                {
                                    Relation_Table_Comment = TableInfoList[Relation_Table_Name]["Comment"];
                                    t_c = Relation_Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (c_c.Length > 1)
                                    {
                                        Relation_Table_Comment = "";
                                        foreach (string Comment_Single_Line in c_c)
                                        {
                                            Relation_Table_Comment += "        /// " + Comment_Single_Line + "\r\n";
                                        }
                                        Relation_Table_Comment = Relation_Table_Comment.Substring(0, Column_Comment.Length - 2);
                                    }
                                    else
                                    {
                                        Relation_Table_Comment = "        /// " + Relation_Table_Comment;
                                    }
                                    //多对一关系需定义
                                    Relation_Unit_Template = @"
        /// <summary>
{$Relation_Table_Comment}
        /// </summary>
        public virtual {$Relation_ClassName} {$Relation_ClassName} { get; set; }";
                                    Relation_Unit_Template = Relation_Unit_Template.Replace("{$Relation_ClassName}", Relation_ClassName);
                                    Relation_Unit_Template = Relation_Unit_Template.Replace("{$Relation_Table_Comment}", Relation_Table_Comment);
                                    Relation_UnitColumnDefine += Relation_Unit_Template;
                                }
                            }
                        }
                    }
                    if (OneHasManyDefine.ContainsKey(Table_Name))
                    {
                        List<string> lOneHasMany = OneHasManyDefine[Table_Name];
                        foreach (string OneHasMany_TableName in lOneHasMany)
                        {
                            OneHasMany_Table_Comment = TableInfoList[OneHasMany_TableName]["Comment"];
                            t_c = OneHasMany_Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (t_c.Length > 1)
                            {
                                OneHasMany_Table_Comment = "";
                                foreach (string Comment_Single_Line in t_c)
                                {
                                    OneHasMany_Table_Comment += "        /// " + Comment_Single_Line + "\r\n";
                                }
                                OneHasMany_Table_Comment = OneHasMany_Table_Comment.Substring(0, OneHasMany_Table_Comment.Length - 2);
                            }
                            else
                            {
                                OneHasMany_Table_Comment = "        /// " + OneHasMany_Table_Comment;
                            }
                            OneHasMany_Unit_Template = @"
        /// <summary>
{$OneHasMany_Table_Comment}
        /// </summary>
        public virtual ICollection<{$OneHasMany_TableName}> {$OneHasMany_TableName} { get; set; }
                        ";
                            OneHasMany_Unit_Template = OneHasMany_Unit_Template.Replace("{$OneHasMany_Table_Comment}", OneHasMany_Table_Comment);
                            OneHasMany_Unit_Template = OneHasMany_Unit_Template.Replace("{$OneHasMany_TableName}", OneHasMany_TableName);
                            OneHasMany_UnitColumnDefine += OneHasMany_Unit_Template;
                        }
                    }
                    UnitColumnDefine += Relation_UnitColumnDefine;
                    UnitColumnDefine += OneHasMany_UnitColumnDefine;

                    Content_New = Content_New.Replace("{$ColumnDefines}", UnitColumnDefine);

                    //存入目标文件内容
                    UtilFile.WriteString2File(Save_Dir + ClassName + ".cs", Content_New);
                }
            }
        }

        /// <summary>
        /// 1a.生成实体类分部类(显示属性)
        /// </summary>
        private void CreateDomainPartial()
        {
            string ClassName = "Admin";
            string InstanceName = "admin";
            string Table_Comment = "系统管理员";
            string Template_Name, UnitTemplate, Content, Content_New;
            string Column_Name, Column_Comment, Column_Type, Column_Length;
            string Relation_ClassName, Relation_Column_Comment, Relation_Table_Name, Relation_Column_Name;
            string UnitColumnDefine;

            foreach (string Table_Name in TableList)
            {
                //读取原文件内容到内存
                Template_Name = @"AutoCode/Model/domain/domain.txt";
                Content = UtilFile.ReadFile2String(Template_Name);
                ClassName = Table_Name;
                if (TableInfoList.ContainsKey(Table_Name))
                {
                    Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
                    UnitColumnDefine = "";
                    foreach (KeyValuePair<String, Dictionary<string, string>> entry in FieldInfo)
                    {
                        Column_Name = entry.Key;
                        Column_Comment = entry.Value["Comment"];
                        Column_Type = entry.Value["Type"];
                        Column_Length = entry.Value["Length"];
                        string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        if (c_c.Length >= 1) Column_Comment = c_c[0];
                        
                        int iLength = UtilNumber.Parse(Column_Length);

                        if (Column_Type.Equals("char"))
                        {
                            Column_Comment = entry.Value["Comment"];
                            c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (c_c.Length > 1)
                            {
                                Column_Comment = "";
                                foreach (string Comment_Single_Line in c_c)
                                {
                                    Column_Comment += "        /// " + Comment_Single_Line + "\r\n";
                                }
                                Column_Comment = Column_Comment.Substring(0, Column_Comment.Length - 2);
                                UnitTemplate = @"
        /// <summary>
{$Column_Comment}
        /// </summary>
        public String {$Column_Name}Show
        {
            get;
            set;
        }";
                                UnitTemplate = UnitTemplate.Replace("{$Column_Comment}", Column_Comment);
                                UnitTemplate = UnitTemplate.Replace("{$Column_Name}", Column_Name);
                                UnitColumnDefine += UnitTemplate;
                            }
                        }
                        else if (Column_Name.Contains("_ID"))
                        {
                            Relation_ClassName = Column_Name.Replace("_ID", "");
                            if (TableList.Contains(Relation_ClassName))
                            {
                                Relation_Table_Name = Relation_ClassName;
                                Dictionary<string, Dictionary<string, string>> Relation_FieldInfo = FieldInfos[Relation_Table_Name];
                                Relation_Column_Name = Column_Name;
                                Relation_Column_Comment = Relation_Column_Name;
                                foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                                {
                                    Relation_Column_Name = relation_entry.Key;

                                    if (UtilString.Contains(relation_entry.Key.ToUpper(), "NAME", "TITLE", "URL"))
                                    {
                                        Relation_Column_Comment = relation_entry.Value["Comment"];
                                        break;
                                    }
                                }
                                UnitTemplate = @"
        /// <summary>
        /// {$Relation_Column_Comment}
        /// </summary>
        public String {$Relation_Column_Name}
        {
            get;
            set;
        }";
                                UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Comment}", Relation_Column_Comment);
                                UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Name}", Relation_Column_Name);
                                UnitColumnDefine += UnitTemplate;
                            }
                        }
                        else if (ColumnIsTextArea(Column_Name, Column_Type, iLength))
                        {
                            UnitTemplate = @"
        /// <summary>
        /// {$Column_Comment}
        /// </summary>
        public String {$Column_Name}Show
        {
            get;
            set;
        }";
                    
                            Column_Comment = entry.Value["Comment"];
                            c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (c_c.Length > 1)
                            {
                                Column_Comment = "";
                                foreach (string Comment_Single_Line in c_c)
                                {
                                    Column_Comment += "        /// " + Comment_Single_Line + "\r\n";
                                }
                                Column_Comment = Column_Comment.Substring(0, Column_Comment.Length - 2);
                            }
                            UnitTemplate = UnitTemplate.Replace("{$Column_Comment}", Column_Comment);
                            UnitTemplate = UnitTemplate.Replace("{$Column_Name}", Column_Name);
                            UnitColumnDefine += UnitTemplate;
                        }
                    }
                    if (!string.IsNullOrEmpty(UnitColumnDefine))
                    {

                        Table_Comment = TableInfoList[Table_Name]["Comment"];
                        string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        if (t_c.Length > 1)
                        {
                            Table_Comment = "";
                            foreach (string Comment_Single_Line in t_c)
                            {
                                Table_Comment += "    /// " + Comment_Single_Line + "\r\n";
                            }
                        }
                        else
                        {
                            Table_Comment = "    /// " + Table_Comment;
                        }
                        Content_New = Content.Replace("{$ClassName}", ClassName);
                        Content_New = Content_New.Replace("{$Table_Comment}", Table_Comment);
                        Content_New = Content_New.Replace("{$InstanceName}", InstanceName);
                        Content_New = Content_New.Replace("{$ColumnDefines}", UnitColumnDefine);
                        //存入目标文件内容
                        UtilFile.WriteString2File(Save_Dir + ClassName + ".cs", Content_New);
                    }
                }
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
                case "uniqueidentifier":
                    return "System.Guid";
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
                if (TableInfoList.ContainsKey(Table_Name))
                {
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
                if (TableInfoList.ContainsKey(Table_Name))
                {
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
                                if ((Enum_ColumnDefine != null) && (Enum_ColumnDefine.Count > 0))
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
            string Relation_ClassName = "Admin";
            string Relation_InstanceName = "admin";
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
                        Relation_ClassName = Column_Name.Replace("_ID", "");
                        if (TableList.Contains(Relation_ClassName))
                        {
                            //读取原文件内容到内存
                            Template_Name = @"AutoCode/Model/domain/httpdata.txt";
                            Content = UtilFile.ReadFile2String(Template_Name);
                            if (TableInfoList.ContainsKey(Relation_ClassName))
                            {
                                Table_Comment = TableInfoList[Relation_ClassName]["Comment"];
                                string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                if (t_c.Length > 1) Table_Comment = t_c[0];
                                Relation_InstanceName = UtilString.LcFirst(Relation_ClassName);

                                Relation_Table_Name = Relation_ClassName;

                                Dictionary<string, Dictionary<string, string>> Relation_FieldInfo = FieldInfos[Relation_Table_Name];
                                foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                                {
                                    Relation_Column_Name = relation_entry.Key;
                                    if (UtilString.Contains(relation_entry.Key.ToUpper(), "NAME", "TITLE", "URL")) break;
                                }

                                Content_New = Content.Replace("{$ClassName}", Relation_ClassName);
                                Content_New = Content_New.Replace("{$Table_Comment}", Table_Comment);
                                Content_New = Content_New.Replace("{$InstanceName}", Relation_InstanceName);
                                Content_New = Content_New.Replace("{$Column_Name}", Column_Name);
                                Content_New = Content_New.Replace("{$Relation_Column_Name}", Relation_Column_Name);

                                //存入目标文件内容
                                UtilFile.WriteString2File(Save_Dir + Relation_ClassName + ".ashx.cs", Content_New);


                                //读取原文件内容到内存
                                Template_Name = @"AutoCode/Model/domain/httpdatadefine.txt";
                                Content = UtilFile.ReadFile2String(Template_Name);
                                Content_New = Content.Replace("{$ClassName}", Relation_ClassName);

                                //存入目标文件内容
                                UtilFile.WriteString2File(Save_Dir + Relation_ClassName + ".ashx", Content_New);
                            }
                        }
                    }

                }
            }
        }
    }
}
