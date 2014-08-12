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
    ///      3.Portal 网站主体的ManageService工具类
    /// </summary>
    public class AutoCodeService:AutoCodeBase
    {
        /// <summary>
        /// 特殊字段:CommitTime
        /// </summary>
        private static string CommitTime_Str = "CommitTime";
        /// <summary>
        /// 特殊字段:Updatetime
        /// </summary>
        private static string UpdateTime_Str = "UpdateTime";
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
            //3.Portal 网站主体的ManageService工具类
            Save_Dir = App_Dir + "Core" + Path.DirectorySeparatorChar + "Business" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
            if (ServiceType == 2) CreateManageService();
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
                if (TableInfoList.ContainsKey(Table_Name))
                {
                    Table_Comment = TableInfoList[Table_Name]["Comment"];
                    string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (t_c.Length > 1) Table_Comment = t_c[0];
                    InstanceName = UtilString.LcFirst(ClassName);

                    Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
                    Dictionary<string, string> entry = FieldInfo["ID"];
                    if (entry["Type"].Equals("uniqueidentifier"))
                    {
                        ID_Type = "Guid";
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
            string Service_NameSpace = "AdminManage";
            string Template_Name, UnitTemplate, Content, Content_New;
            string ColumnNameComment, ColumnCommentName, EnumColumnName;
            string Column_Name,Column_Table_Name,Column_Comment, Column_Type, Column_Length;
            string ImportConvertDataToShow, ExportConvertShowToData;
            string SpecialResult = "";
            string Relation_ClassName, Relation_InstanceName;
            string Relation_Table_Name, Relation_Column_Name,TreeInstanceDefine, Relation_Column_Level,RelationFieldTreeRecursive;
            string ImgUploadSrc = "", Relation_Parent_Init = "";
            bool IsImage;

            foreach (string Table_Name in TableList)
            {
                //读取原文件内容到内存
                Template_Name = @"AutoCode/Model/service/extservice.txt";
                Content = UtilFile.ReadFile2String(Template_Name);
                ClassName = Table_Name;
                RelationFieldTreeRecursive="";
                TreeInstanceDefine="";
                if (TableInfoList.ContainsKey(Table_Name))
                {
                    Table_Comment = TableInfoList[Table_Name]["Comment"];
                    string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (t_c.Length > 1) Table_Comment = t_c[0];
                    InstanceName = UtilString.LcFirst(ClassName);

                    Content_New = Content.Replace("{$ClassName}", ClassName);
                    Content_New = Content_New.Replace("{$Table_Comment}", Table_Comment);
                    Content_New = Content_New.Replace("{$InstanceName}", InstanceName);

                    Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
                    ColumnNameComment = ""; ColumnCommentName = ""; EnumColumnName = ""; SpecialResult = "";
                    ImportConvertDataToShow = ""; ExportConvertShowToData = ""; ImgUploadSrc = ""; Relation_Parent_Init = "";
                    foreach (KeyValuePair<String, Dictionary<string, string>> entry in FieldInfo)
                    {
                        IsImage = false;
                        Column_Name = entry.Key;
                        Column_Comment = entry.Value["Comment"];
                        Column_Type = entry.Value["Type"];
                        Column_Length = entry.Value["Length"];
                        string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        if (c_c.Length >= 1) Column_Comment = c_c[0];
                        if (!((Column_Name.ToUpper().Equals(CommitTime_Str.ToUpper())) || (Column_Name.ToUpper().Equals(UpdateTime_Str.ToUpper()))))
                        {
                            if (!Column_Type.Equals("tinyint"))
                            {
                                ColumnNameComment += "                    {\"" + Column_Name + "\",\"" + Column_Comment + "\"},\r\n";
                                ColumnCommentName += "                    {\"" + Column_Comment + "\",\"" + Column_Name + "\"},\r\n";
                            }
                        }
                        int iLength = UtilNumber.Parse(Column_Length);
                        IsImage = ColumnIsImage(Column_Name, Column_Comment);
                        if (IsImage)
                        {
                            UnitTemplate = @"
                    Dictionary<string,object> uploadResult=this.UploadImage({$InstanceName}Form.Files,""{$Column_Name}Upload"",""{$Column_Name}"",""{$InstanceName}"");
                    if ((uploadResult!=null)&&((bool)uploadResult[""success""]==true)&&(uploadResult.Keys.Contains(""file_name""))){
                        {$InstanceName}.{$Column_Name}=(string)uploadResult[""file_name""];
                    }";
                            UnitTemplate = UnitTemplate.Replace("{$InstanceName}", InstanceName);
                            UnitTemplate = UnitTemplate.Replace("{$Column_Name}", Column_Name);
                            ImgUploadSrc += UnitTemplate;
                        }else if (Column_Type.Equals("tinyint"))
                        {
                            Column_Comment = entry.Value["Comment"];
                            c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (c_c.Length > 1)
                            {
                                UnitTemplate = @"
                    {$InstanceName}.{$Column_Name}Show = Enum{$Column_Table_Name}.{$Column_Name}Show(Convert.ToChar({$InstanceName}.{$Column_Name}));";
                                UnitTemplate = UnitTemplate.Replace("{$InstanceName}", InstanceName);
                                UnitTemplate = UnitTemplate.Replace("{$Column_Name}", Column_Name);
                                Column_Table_Name = Column_Name;
                                if (Array.IndexOf(Same_Column_Names, Column_Name) > -1)
                                {
                                    Column_Table_Name = ClassName + "_" + Column_Name;
                                }
                                UnitTemplate = UnitTemplate.Replace("{$Column_Table_Name}", Column_Table_Name);
                                SpecialResult += UnitTemplate;
                                Column_Comment = c_c[0].Trim();
                                EnumColumnName += "\""+Column_Name+"\",";
                                ColumnNameComment += "                    {\"" + Column_Name + "Show\",\"" + Column_Comment + "\"},\r\n";
                                ColumnCommentName += "                    {\"" + Column_Comment + "\",\"" + Column_Name + "Show\"},\r\n";
                                ExportConvertShowToData += "                    " + InstanceName + "." + Column_Name + "Show = Enum" + Column_Name + "." + Column_Name + "Show(" + InstanceName + "." + Column_Name + ");\r\n";
                                ImportConvertDataToShow += "                    " + InstanceName + "." + Column_Name + " = Enum" + Column_Name + "." + Column_Name + "ByShow(" + InstanceName + "." + Column_Name + "Show);\r\n";
                            }
                        }
                        else if (Column_Name.Contains("_ID"))
                        {
                            Relation_Table_Name = Column_Name.Replace("_ID", "");
                            Relation_ClassName = Relation_Table_Name;

                            if (TableList.Contains(Relation_ClassName) || (Relation_ClassName.ToUpper().Equals("PARENT")))
                            {
                                if (Relation_Table_Name.ToUpper().Equals("PARENT"))
                                {
                                    Relation_Table_Name = Table_Name;
                                    Relation_ClassName = ClassName;
                                    Relation_Column_Name = "";
                                    Relation_Column_Level = "";
                                    Relation_InstanceName = UtilString.LcFirst(Relation_ClassName);
                                    Dictionary<string, Dictionary<string, string>> Relation_FieldInfo = FieldInfos[Table_Name];
                                    foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                                    {
                                        if (UtilString.Contains(relation_entry.Key.ToUpper(), "NAME", "TITLE", "URL"))
                                        {
                                            Relation_Column_Name = relation_entry.Key;
                                            break;
                                        }
                                    }
                                    bool IsPermitNull = true;
                                    foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                                    {
                                        if (UtilString.Contains(relation_entry.Key.ToUpper(), "LEVEL"))
                                        {
                                            Relation_Column_Level = relation_entry.Key;
                                            if (relation_entry.Value["Null"].Equals("否")) IsPermitNull = false;
                                            break;
                                        }
                                    }
                                    TreeInstanceDefine = @"
                {$Relation_ClassName} {$Relation_InstanceName}_instance;";
                                    if (string.IsNullOrEmpty(Relation_Column_Level))
                                    {
                                        foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                                        {
                                            if (UtilString.Contains(relation_entry.Key.ToUpper(), "TYPE"))
                                            {
                                                Relation_Column_Level = relation_entry.Key;
                                                break;
                                            }
                                        }
                                        UnitTemplate = @"
                    {$Relation_InstanceName}_instance=null;
                    if ({$Relation_InstanceName}.Parent_ID!=null){
                        {$Relation_InstanceName}_instance=db.{$Relation_ClassName}.Find({$Relation_InstanceName}.Parent_ID);
                        {$Relation_InstanceName}.{$Relation_Column_Name}_Parent={$Relation_InstanceName}_instance.{$Relation_Column_Name};
                    }
                    if ({$Relation_InstanceName}_instance!=null){
                        int level = (int){$Relation_InstanceName}_instance.{$Relation_Column_Level};
                        {$Relation_InstanceName}.{$Relation_ClassName}ShowAll=this.{$Relation_ClassName}ShowAll({$Relation_InstanceName}.Parent_ID,level);
                    }";
                                        RelationFieldTreeRecursive = @"
        /// <summary>
        /// 显示{$Column_Comment}[全]
        ///  注:采用了递归写法
        /// </summary>
        /// <param name=""Parent_ID"">{$Column_Comment}标识</param>
        /// <param name=""level"">目录层级</param>
        /// <returns></returns>
        private string {$Relation_ClassName}ShowAll(int? Parent_ID,int level)
        {
            string {$Relation_ClassName}ShowAll="""";
            {$Relation_ClassName} {$Relation_InstanceName}_p=db.{$Relation_ClassName}.Find(Parent_ID);
            if (level<=0){
                {$Relation_ClassName}ShowAll={$Relation_InstanceName}_p.{$Relation_Column_Name};
            }else{
                Parent_ID={$Relation_InstanceName}_p.Parent_ID;
                {$Relation_ClassName}ShowAll=this.{$Relation_ClassName}ShowAll(Parent_ID,level-1)+""->""+{$Relation_InstanceName}_p.{$Relation_Column_Name};
            }
            return {$Relation_ClassName}ShowAll;
        }
";
                                    }
                                    else
                                    {
                                        string ColumnTypeNull = "int";
                                        if (IsPermitNull) ColumnTypeNull += "?";
                                        UnitTemplate = @"
                    {$Relation_InstanceName}_instance=null;
                    if ({$Relation_InstanceName}.Parent_ID!=null){
                        {$Relation_InstanceName}_instance=db.{$Relation_ClassName}.Find({$Relation_InstanceName}.Parent_ID);
                        {$Relation_InstanceName}.{$Relation_Column_Name}_Parent={$Relation_InstanceName}_instance.{$Relation_Column_Name};
                    }
                    if ({$Relation_InstanceName}_instance!=null){
                        {$ColumnTypeNull} level = {$Relation_InstanceName}_instance.{$Relation_Column_Level};
                        {$Relation_InstanceName}.{$Relation_ClassName}ShowAll=this.{$Relation_ClassName}ShowAll({$Relation_InstanceName}.Parent_ID,level);
                    }";
                                        UnitTemplate = UnitTemplate.Replace("{$ColumnTypeNull}", ColumnTypeNull);
                                        RelationFieldTreeRecursive = @"
        /// <summary>
        /// 显示{$Column_Comment}[全]
        ///  注:采用了递归写法
        /// </summary>
        /// <param name=""Parent_ID"">{$Column_Comment}标识</param>
        /// <param name=""level"">目录层级</param>
        /// <returns></returns>
        private string {$Relation_ClassName}ShowAll(int? Parent_ID,int level)
        {
            string {$Relation_ClassName}ShowAll="""";
            {$Relation_ClassName} {$Relation_InstanceName}_p=db.{$Relation_ClassName}.Find(Parent_ID);
            if (level<=1){
                {$Relation_ClassName}ShowAll={$Relation_InstanceName}_p.{$Relation_Column_Name};
            }else{
                Parent_ID={$Relation_InstanceName}_p.Parent_ID;
                {$Relation_ClassName}ShowAll=this.{$Relation_ClassName}ShowAll(Parent_ID,level-1)+""->""+{$Relation_InstanceName}_p.{$Relation_Column_Name};
            }
            return {$Relation_ClassName}ShowAll;
        }
";
                                    }
                                    UnitTemplate = UnitTemplate.Replace("{$Relation_InstanceName}", Relation_InstanceName);
                                    UnitTemplate = UnitTemplate.Replace("{$Relation_ClassName}", Relation_ClassName);
                                    UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Name}", Relation_Column_Name);
                                    UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Level}", Relation_Column_Level);
                                    TreeInstanceDefine = TreeInstanceDefine.Replace("{$Relation_InstanceName}", Relation_InstanceName);
                                    TreeInstanceDefine = TreeInstanceDefine.Replace("{$Relation_ClassName}", Relation_ClassName);
                                    SpecialResult += UnitTemplate;
                                    Column_Comment = entry.Value["Comment"];
                                    c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (c_c.Length >= 1) Column_Comment = c_c[0];
                                    Column_Comment = Column_Comment.Replace("标识", "");
                                    RelationFieldTreeRecursive = RelationFieldTreeRecursive.Replace("{$Column_Comment}", Column_Comment);
                                    RelationFieldTreeRecursive = RelationFieldTreeRecursive.Replace("{$Relation_InstanceName}", Relation_InstanceName);
                                    RelationFieldTreeRecursive = RelationFieldTreeRecursive.Replace("{$Relation_ClassName}", Relation_ClassName);
                                    RelationFieldTreeRecursive = RelationFieldTreeRecursive.Replace("{$Relation_Column_Name}", Relation_Column_Name);
                                    UnitTemplate = @"
                    if ({$InstanceName}.{$Column_Name} == 0) {$InstanceName}.{$Column_Name} = null;";
                                    UnitTemplate = UnitTemplate.Replace("{$InstanceName}", InstanceName);
                                    UnitTemplate = UnitTemplate.Replace("{$Column_Name}", Column_Name);
                                    Relation_Parent_Init += UnitTemplate;
                                }
                                else if (TableInfoList.ContainsKey(Relation_Table_Name))
                                {
                                    //读取原文件内容到内存
                                    Template_Name = @"AutoCode/Model/domain/httpdata.txt";
                                    Content = UtilFile.ReadFile2String(Template_Name);
                                    Relation_InstanceName = UtilString.LcFirst(Relation_ClassName);
                                    Relation_Table_Name = Relation_ClassName;

                                    Dictionary<string, Dictionary<string, string>> Relation_FieldInfo = FieldInfos[Relation_Table_Name];
                                    Relation_Column_Name = Column_Name;
                                    foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                                    {
                                        Relation_Column_Name = relation_entry.Key;
                                        if (UtilString.Contains(relation_entry.Key.ToUpper(), "NAME", "TITLE", "URL")) break;
                                    }
                                    UnitTemplate = @"
                    {$InstanceName}.{$Relation_Column_Name} = {$InstanceName}.{$Relation_ClassName}.{$Relation_Column_Name};";
                                    UnitTemplate = UnitTemplate.Replace("{$InstanceName}", InstanceName);
                                    UnitTemplate = UnitTemplate.Replace("{$Relation_ClassName}", Relation_ClassName);
                                    UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Name}", Relation_Column_Name);
                                    SpecialResult += UnitTemplate;
                                    UnitTemplate = @"
                    {$Relation_ClassName} {$Relation_InstanceName} = db.{$Relation_ClassName}.Where(e => e.{$Relation_Column_Name}.Equals({$InstanceName}.{$Relation_Column_Name})).SingleOrDefault();
                    {$InstanceName}.{$Column_Name} = {$Relation_InstanceName}.ID;
                                ";
                                    UnitTemplate = UnitTemplate.Replace("{$InstanceName}", InstanceName);
                                    UnitTemplate = UnitTemplate.Replace("{$Relation_InstanceName}", Relation_InstanceName);
                                    UnitTemplate = UnitTemplate.Replace("{$Column_Name}", Column_Name);
                                    UnitTemplate = UnitTemplate.Replace("{$Relation_ClassName}", Relation_ClassName);
                                    UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Name}", Relation_Column_Name);
                                    ImportConvertDataToShow += UnitTemplate;
                                }
                            }
                        }
                        else if (ColumnIsTextArea(Column_Name, Column_Type, iLength))
                        {
                            UnitTemplate = @"
                    if (!string.IsNullOrEmpty({$InstanceName}.{$Column_Name}))
                    {
                        {$InstanceName}.{$Column_Name}Show = Regex.Replace({$InstanceName}.{$Column_Name}, ""<\\s*img\\s+[^>]*?src\\s*=\\s*(\'|\"")(.*?)\\1[^>]*?\\/?\\s*>"", ""<a href='${2}' target='_blank'>${0}</a>"");
                        {$InstanceName}.{$Column_Name}Show = {$InstanceName}.{$Column_Name}Show.Replace(""\\\"""", """");
                    }";
                            UnitTemplate = UnitTemplate.Replace("{$InstanceName}", InstanceName);
                            UnitTemplate = UnitTemplate.Replace("{$Column_Name}", Column_Name);
                            SpecialResult += UnitTemplate;
                        }
                    }
                    ColumnNameComment = ColumnNameComment.Substring(0, ColumnNameComment.Length - 3);
                    ColumnCommentName = ColumnCommentName.Substring(0, ColumnCommentName.Length - 3);

                    Content_New = Content_New.Replace("{$ColumnNameComment}", ColumnNameComment);
                    Content_New = Content_New.Replace("{$ColumnCommentName}", ColumnCommentName);

                    SpecialResult += @"
                    this.Stores.Add((" + ClassName + ")ClearInclude(" + InstanceName + "));";
                    SpecialResult = SpecialResult.Substring(1, SpecialResult.Length - 1);
                    Content_New = Content_New.Replace("{$SpecialResult}", SpecialResult);
                    Content_New = Content_New.Replace("{$Service_NameSpace}", Service_NameSpace);
                    Content_New = Content_New.Replace("{$CommitTime_Str}", CommitTime_Str);
                    Content_New = Content_New.Replace("{$UpdateTime_Str}", UpdateTime_Str);

                    Content_New = Content_New.Replace("{$EnumColumnName}", EnumColumnName);
                    Content_New = Content_New.Replace("{$ImportConvertDataToShow}", ImportConvertDataToShow);
                    Content_New = Content_New.Replace("{$ExportConvertShowToData}", ExportConvertShowToData);
                    Content_New = Content_New.Replace("{$RelationFieldTreeRecursive}", RelationFieldTreeRecursive);
                    Content_New = Content_New.Replace("{$TreeInstanceDefine}",TreeInstanceDefine);
                    
                    Content_New = Content_New.Replace("{$ImgUploadSrc}", ImgUploadSrc);
                    Content_New = Content_New.Replace("{$Relation_Parent_Init}", Relation_Parent_Init);
                    
                    //存入目标文件内容
                    UtilFile.WriteString2File(Save_Dir + "ExtService" + ClassName + ".ashx.cs", Content_New);

                    //读取原文件内容到内存
                    Template_Name = @"AutoCode/Model/service/extservicedefine.txt";
                    Content = UtilFile.ReadFile2String(Template_Name);
                    Content_New = Content.Replace("{$ClassName}", ClassName);
                    Content_New = Content_New.Replace("{$Service_NameSpace}", Service_NameSpace);

                    //存入目标文件内容
                    UtilFile.WriteString2File(Save_Dir + "ExtService" + ClassName + ".ashx", Content_New);
                }
            }
        }

        /// <summary>
        /// 3.Portal 网站主体的ManageService工具类
        /// </summary>
        /// [模板文件]:service/manageservice.txt
        /// 生成文件名称:ManageService.cs
        private void CreateManageService()
        {
            string Content="";
            string ClassName,InstanceName,Table_Comment,UnitTemplate;
            string ServiceDefine = "", ServiceMethod = "";
            //读取原文件内容到内存
            string Template_Name = @"AutoCode/Model/service/manageservice.txt";
            Content = UtilFile.ReadFile2String(Template_Name);

            foreach (string Table_Name in TableList)
            {
                ClassName = Table_Name;
                Table_Comment = "";
                if (TableInfoList.ContainsKey(Table_Name))
                {
                    Table_Comment = TableInfoList[Table_Name]["Comment"];
                    string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (t_c.Length > 1) Table_Comment = t_c[0];
                }
                InstanceName = UtilString.LcFirst(ClassName);
                UnitTemplate = @"
        /// <summary>
        /// {$Table_Comment}服务
        /// </summary>
        private static IService{$ClassName} {$InstanceName}Service;"; 
                UnitTemplate = UnitTemplate.Replace("{$InstanceName}", InstanceName);
                UnitTemplate = UnitTemplate.Replace("{$ClassName}", ClassName);
                UnitTemplate = UnitTemplate.Replace("{$Table_Comment}", Table_Comment);
                ServiceDefine += UnitTemplate;
                UnitTemplate = @"
        /// <summary>
        /// 服务:{$Table_Comment}
        /// </summary>
        public static IService{$ClassName} {$ClassName}Service()
        {
            Init();
            if ({$InstanceName}Service == null) {$InstanceName}Service = new Service{$ClassName}();
            return {$InstanceName}Service;
        }";
                UnitTemplate = UnitTemplate.Replace("{$InstanceName}", InstanceName);
                UnitTemplate = UnitTemplate.Replace("{$ClassName}", ClassName);
                UnitTemplate = UnitTemplate.Replace("{$Table_Comment}", Table_Comment);
                ServiceMethod += UnitTemplate;
            }
            Content = Content.Replace("{$ServiceDefine}", ServiceDefine);
            Content = Content.Replace("{$ServiceMethod}", ServiceMethod);
            
            //存入目标文件内容
            UtilFile.WriteString2File(Save_Dir + "ManageService.cs", Content);
        }
    }
}
