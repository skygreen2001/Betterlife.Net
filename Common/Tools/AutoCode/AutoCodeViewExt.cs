using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Common;

namespace Tools.AutoCode
{
    /// <summary>
    /// 工具类:自动生成代码-使用后台生成的ExtJS表示层
    /// </summary>
    public class AutoCodeViewExt:AutoCodeBase
    {
        /// <summary>
        /// JS命名空间
        /// </summary>
        private static string JsNamespace = "BetterlifeNet";
        /// <summary>
        /// JS命名空间别名
        /// </summary>
        private static string JsNamespace_Alias = "Bn";
        /// <summary>
        /// 运行主程序
        /// 1.后台extjs文件生成
        /// </summary>
        public void Run()
        {
            base.Init();

            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "Scripts" + Path.DirectorySeparatorChar + "core" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
            CreateExtjsView();
        }

        /// <summary>
        /// 1.后台extjs文件生成【多个文件】
        /// [模板文件]:view/extjs.txt   
        /// [生成文件名称]:InstanceName
        /// [生成文件后缀名]:.js
        /// </summary>
        private void CreateExtjsView()
        {
            string ClassName = "Admin";
            string InstanceName = "admin";
            string Table_Comment = "系统管理员";


            string Template_Name, Content, Content_New;
            foreach (string Table_Name in TableList)
            {
                //读取原文件内容到内存
                Template_Name = @"AutoCode/Model/view/extjs.txt";
                Content = UtilFile.ReadFile2String(Template_Name);
                ClassName = Table_Name;
                if (TableInfoList.ContainsKey(Table_Name))
                {
                    Table_Comment = TableInfoList[Table_Name]["Comment"];
                    string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (t_c.Length > 1) Table_Comment = t_c[0];
                    InstanceName = UtilString.LcFirst(ClassName);

                    Content_New = Content.Replace("$classname", ClassName);
                    Content_New = Content_New.Replace("{$table_comment}", Table_Comment);
                    Content_New = Content_New.Replace("{$instancename}", InstanceName);
                    Content_New = Content_New.Replace("$appName", JsNamespace);
                    Content_New = Content_New.Replace("$ns_alias", JsNamespace_Alias);
                    //Ext "store" 中包含的fields
                    Dictionary<string, string> StoreInfo = Model_Fields(Table_Name, InstanceName);
                    Content_New = Content_New.Replace("$fields", StoreInfo["fields"]);
                    //Ext "$relationStore="中关系库Store的定义
                    Content_New = Content_New.Replace("$relationStore", StoreInfo["relationStore"]);

                    //Ext "EditWindow"里items的fieldLabels
                    Dictionary<string,string> EditWindowVars=Model_FieldLables(ClassName, JsNamespace_Alias, "");
                    Content_New = Content_New.Replace("$fieldLabels", EditWindowVars["FieldLabels"]);
                    Content_New = Content_New.Replace("{$TreeLevelVisible_Add}", EditWindowVars["TreeLevelVisible_Add"]);
                    Content_New = Content_New.Replace("{$TreeLevelVisible_Update}", EditWindowVars["TreeLevelVisible_Update"]);
                    Content_New = Content_New.Replace("{$Password_Add}", EditWindowVars["Password_Add"]);
                    Content_New = Content_New.Replace("{$Password_Update}", EditWindowVars["Password_Update"]);
                    Content_New = Content_New.Replace("{$IsFileUpload}", EditWindowVars["IsFileUpload"]);
                    
                    //Ext "Tabs" 中"onAddItems"包含的viewdoblock
                    Content_New = Content_New.Replace("$viewdoblock", Model_Viewblock(Table_Name));

                    //Ext "Grid" 中包含的columns
                    Content_New = Content_New.Replace("$columns", Model_Columns(ClassName, ""));

                    //获取Ext "Textarea" 转换成在线编辑器
                    Dictionary<string, string> Textarea_Vars = Model_TextareaOnlineEditor(ClassName);
                    Content_New = Content_New.Replace("{$TextareaOnlineditor_Add}", Textarea_Vars["TextareaOnlineditor_Add"]);
                    Content_New = Content_New.Replace("{$TextareaOnlineditor_Replace}", Textarea_Vars["TextareaOnlineditor_Replace"]);
                    Content_New = Content_New.Replace("{$TextareaOnlineditor_Update}", Textarea_Vars["TextareaOnlineditor_Update"]);
                    Content_New = Content_New.Replace("{$TextareaOnlineditor_Save}", Textarea_Vars["TextareaOnlineditor_Save"]);
                    Content_New = Content_New.Replace("{$TextareaOnlineditor_Reset}", Textarea_Vars["TextareaOnlineditor_Reset"]);
                    Content_New = Content_New.Replace("{$TextareaOnlineditor_Init}", Textarea_Vars["TextareaOnlineditor_Init"]);
                    Content_New = Content_New.Replace("{$TextareaOnlineditor_Init_func}", Textarea_Vars["TextareaOnlineditor_Init_func"]);

                    //存入目标文件内容
                    UtilFile.WriteString2File(Save_Dir + InstanceName + ".js", Content_New);
                }
            }
        }

        /// <summary>
        /// 获取Ext "Textarea" 转换成在线编辑器
        /// </summary>
        /// <param name="Table_Name">表名</param>
        /// <param name="Blank_Pre">空格字符串</param>
        /// <returns></returns>
        public Dictionary<string, string> Model_TextareaOnlineEditor(string Table_Name,string Blank_Pre="")
        {
            Dictionary<string, string> Result;
            bool IsImage, Has_Textarea=false;
            string TextareaOnlineditor_Replace="",TextareaOnlineditor_Add="",TextareaOnlineditor_Update="",TextareaOnlineditor_Save="";
            string TextareaOnlineditor_Reset="",TextareaOnlineditor_Init="",TextareaOnlineditor_Init_func="";
            string ClassName,ColumnType, Column_Name, Column_Comment,Column_Length;
            string Reset_Img="",Add_Img="",Update_Img="";

            ClassName = Table_Name;
            Dictionary<string, string> TextareaOnlineditor_Replace_array=new Dictionary<string, string>(){{"UEditor",""},{"ckEditor",""}};
            Dictionary<string, string> TextareaOnlineditor_Add_array=new Dictionary<string, string>(){{"UEditor",""},{"ckEditor",""}};
            Dictionary<string, string> TextareaOnlineditor_Update_array=new Dictionary<string, string>(){{"UEditor",""},{"ckEditor",""}};
            Dictionary<string, string> TextareaOnlineditor_Save_array=new Dictionary<string, string>(){{"UEditor",""},{"ckEditor",""}};
            Dictionary<string, string> TextareaOnlineditor_Reset_array = new Dictionary<string, string>() { { "UEditor", "" }, { "ckEditor", "" } };

            Result = new Dictionary<string, string>();
            Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
            foreach (KeyValuePair<String, Dictionary<string, string>> entry in FieldInfo)
            {
                Column_Name = entry.Key;
                ColumnType = entry.Value["Type"];
                Column_Comment = entry.Value["Comment"];
                Column_Length = entry.Value["Length"];
                int iLength = UtilNumber.Parse(Column_Length);
                IsImage = ColumnIsImage(Column_Name,Column_Comment);
                if (IsImage)
                {
                    Reset_Img += "                        this."+Column_Name+"Upload.setValue(this."+Column_Name+".getValue());\r\n";
                    Add_Img += "            "+JsNamespace_Alias+"."+ClassName+".View.Running.edit_window."+Column_Name+"Upload.setValue(\"\");\r\n";
                    Update_Img += "            "+JsNamespace_Alias+"."+ClassName+".View.Running.edit_window."+Column_Name+"Upload.setValue("+JsNamespace_Alias+"."+ClassName+".View.Running.edit_window."+Column_Name+".getValue());\r\n";
                }
                else
                {
                    if (ColumnIsTextArea(Column_Name, ColumnType, iLength))
                    {
                        Has_Textarea = true;
						TextareaOnlineditor_Replace_array["UEditor"]+="                                this.editForm."+Column_Name+".setWidth(\"98%\");\r\n";
						TextareaOnlineditor_Replace_array["UEditor"]+=Blank_Pre+"                                pageInit_ue_"+Column_Name+"();\r\n";
						TextareaOnlineditor_Replace_array["ckEditor"]+="                                ckeditor_replace_"+Column_Name+"();\r\n";
						
						TextareaOnlineditor_Add_array["UEditor"]+="                    if (ue_"+Column_Name+")ue_"+Column_Name+".setContent(\"\");\r\n";
						TextareaOnlineditor_Add_array["ckEditor"]+="                    if (CKEDITOR.instances."+Column_Name+") CKEDITOR.instances."+Column_Name+".setData(\"\");\r\n";
						
						TextareaOnlineditor_Update_array["UEditor"]+="                    ue_"+Column_Name+".ready(function(){ue_"+Column_Name+".setContent(data."+Column_Name+");});\r\n";
						TextareaOnlineditor_Update_array["ckEditor"]+="                    if (CKEDITOR.instances."+Column_Name+") CKEDITOR.instances."+Column_Name+".setData(data."+Column_Name+");\r\n";
						
						TextareaOnlineditor_Save_array["UEditor"]+="                                if (ue_"+Column_Name+")this.editForm."+Column_Name+".setValue(ue_"+Column_Name+".getContent());\r\n";
						TextareaOnlineditor_Save_array["ckEditor"]+="                                if (CKEDITOR.instances."+Column_Name+") this.editForm."+Column_Name+".setValue(CKEDITOR.instances."+Column_Name+".getData());\r\n";
						
						TextareaOnlineditor_Reset_array["UEditor"]+="                                if (ue_"+Column_Name+") ue_"+Column_Name+".setContent("+JsNamespace_Alias+"."+ClassName+".View.Running.{$instancename}Grid.getSelectionModel().getSelected().data."+Column_Name+");\r\n";
						TextareaOnlineditor_Reset_array["ckEditor"]+="                                if (CKEDITOR.instances."+Column_Name+") CKEDITOR.instances."+Column_Name+".setData("+JsNamespace_Alias+"."+ClassName+".View.Running.{$instancename}Grid.getSelectionModel().getSelected().data."+Column_Name+");\r\n";
					}
                }
            }

            if (Has_Textarea)
            {   
			    TextareaOnlineditor_Init=",\r\n"+
									        "        /**\r\n"+
									        "         * 在线编辑器类型。\r\n";
			    TextareaOnlineditor_Init+="         * 1:CkEditor,4:UEditor[默认]\r\n";
			    TextareaOnlineditor_Init+="         * 配合Action的变量配置$online_editor\r\n"+
									        "         */\r\n"+
									        "        OnlineEditor:4";
			    TextareaOnlineditor_Init_func="\r\n"+
									        "        if (Ext.util.Cookies.get('OnlineEditor')!=null){\r\n"+
									        "            "+JsNamespace_Alias+"."+ClassName+".Config.OnlineEditor=parseInt(Ext.util.Cookies.get('OnlineEditor'));\r\n"+
									        "        }\r\n";
			    TextareaOnlineditor_Replace=",\r\n"+
									        Blank_Pre+"                    afterrender:function(){\r\n"+
									        Blank_Pre+"                        switch ("+JsNamespace_Alias+"."+ClassName+".Config.OnlineEditor)\r\n"+
									        Blank_Pre+"                        {\r\n"+
									        Blank_Pre+"                            case 1:\r\n"+
									        Blank_Pre+TextareaOnlineditor_Replace_array["ckEditor"]+
									        Blank_Pre+"                                break\r\n";
									  
			    TextareaOnlineditor_Replace+=
									        Blank_Pre+"                            default:\r\n"+
									        Blank_Pre+TextareaOnlineditor_Replace_array["UEditor"]+
									        Blank_Pre+"                        }\r\n"+
									        Blank_Pre+"                    }";
			    TextareaOnlineditor_Add=Add_Img+
									        Blank_Pre+"            switch ("+JsNamespace_Alias+"."+ClassName+".Config.OnlineEditor)\r\n"+
									        Blank_Pre+"            {\r\n"+
									        Blank_Pre+"                case 1:\r\n"+
									        Blank_Pre+TextareaOnlineditor_Add_array["ckEditor"]+
									        Blank_Pre+"                    break\r\n";
			    TextareaOnlineditor_Add+=
									        Blank_Pre+"                default:\r\n"+
									        Blank_Pre+TextareaOnlineditor_Add_array["UEditor"]+
									        Blank_Pre+"            }\r\n";
			    TextareaOnlineditor_Update=Update_Img+
									        Blank_Pre+"            var data = this.getSelectionModel().getSelected().data;\r\n"+
									        Blank_Pre+"            switch ("+JsNamespace_Alias+"."+ClassName+".Config.OnlineEditor)\r\n"+
									        Blank_Pre+"            {\r\n"+
									        Blank_Pre+"                case 1:\r\n"+
									        Blank_Pre+TextareaOnlineditor_Update_array["ckEditor"]+
									        Blank_Pre+"                    break\r\n";
			    TextareaOnlineditor_Update+=
									        Blank_Pre+"                default:\r\n"+
									        Blank_Pre+TextareaOnlineditor_Update_array["UEditor"]+
									        Blank_Pre+"            }\r\n";
			    TextareaOnlineditor_Save=Blank_Pre+"                        switch ("+JsNamespace_Alias+"."+ClassName+".Config.OnlineEditor)\r\n"+
									        Blank_Pre+"                        {\r\n"+
									        Blank_Pre+"                            case 1:\r\n"+
									        Blank_Pre+TextareaOnlineditor_Save_array["ckEditor"]+
									        Blank_Pre+"                                break\r\n";
									  
			    TextareaOnlineditor_Save+=
									        Blank_Pre+"                            default:\r\n"+
									        Blank_Pre+TextareaOnlineditor_Save_array["UEditor"]+
									        Blank_Pre+"                        }\r\n";
			    TextareaOnlineditor_Reset=Reset_Img+
									        Blank_Pre+"                        switch ("+JsNamespace_Alias+"."+ClassName+".Config.OnlineEditor)\r\n"+
									        Blank_Pre+"                        {\r\n"+
									        Blank_Pre+"                            case 1:\r\n"+
									        Blank_Pre+TextareaOnlineditor_Reset_array["ckEditor"]+
									        Blank_Pre+"                                break\r\n";
									  
			    TextareaOnlineditor_Reset+=
									        Blank_Pre+"                            default:\r\n"+
									        Blank_Pre+TextareaOnlineditor_Reset_array["UEditor"]+
									        Blank_Pre+"                        }\r\n";
		    }else{
			    TextareaOnlineditor_Add=Add_Img;
			    TextareaOnlineditor_Update=Update_Img;
			    TextareaOnlineditor_Reset=Reset_Img;
		    }
            
		    Result["TextareaOnlineditor_Replace"]=TextareaOnlineditor_Replace;
		    Result["TextareaOnlineditor_Add"]=TextareaOnlineditor_Add;
		    Result["TextareaOnlineditor_Update"]=TextareaOnlineditor_Update;
		    Result["TextareaOnlineditor_Save"]=TextareaOnlineditor_Save;
		    Result["TextareaOnlineditor_Reset"]=TextareaOnlineditor_Reset;
		    Result["TextareaOnlineditor_Init"]=TextareaOnlineditor_Init;
		    Result["TextareaOnlineditor_Init_func"]=TextareaOnlineditor_Init_func;
            return Result;
        }

        /// <summary>
        /// 获取Ext "Store"里的fields 
        /// </summary>
        /// <param name="Table_Name">表名称</param>
        /// <param name="InstanceName">实体变量</param>
        /// <returns></returns>
        public Dictionary<string,string> Model_Fields(string Table_Name, string InstanceName)
        {
            Dictionary<string,string> Result;
            string Unit_Template,Fields;
            string ClassName, Column_Name, Column_Type, Column_Comment, Column_Length, ColumnFormat;
            string Relation_Table_Name, Relation_Table_Comment, Relation_Class_Name, Relation_InstanceName, Relation_Column_Name;
            string RelationStore="",RelationStoreTemplate = "";
            Result = new Dictionary<string,string>();
            Fields = "";
            ClassName = Table_Name;
            Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
            foreach (KeyValuePair<String, Dictionary<string, string>> entry in FieldInfo)
            {
                Column_Name = entry.Key;
                ColumnFormat = "";
                if ((Column_Name.ToUpper().Equals("COMMITTIME")) || (Column_Name.ToUpper().Equals("UPDATETIME"))) continue;
                Column_Type = entry.Value["Type"];
                Column_Type = ColumnTypeByDbDefine(Column_Type, Column_Name);
                Column_Comment = entry.Value["Comment"];
                string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (c_c.Length > 0) Column_Comment = c_c[0].Trim(); else Column_Comment = Column_Name;

                if (Column_Type.Equals("date"))
                {
                    ColumnFormat = ",dateFormat:'Y-m-d H:i:s'";
                }

                Unit_Template = @"
                { name: '{$Column_Name}', type: '{$Column_Type}' {$ColumnFormat}},";
                string Relation_Unit_Template = "";
                if (Column_Name.ToUpper().Contains("_ID"))
                {
                    Relation_Table_Name = Column_Name.Replace("_ID", "");
                    Relation_Class_Name = Relation_Table_Name;
                    if (TableInfoList.ContainsKey(Relation_Table_Name) || (Relation_Table_Name.ToUpper().Equals("PARENT")))
                    {
                        if (Relation_Table_Name.ToUpper().Equals("PARENT"))
                        {
                            Relation_Column_Name = "";
                            Dictionary<string, Dictionary<string, string>> Relation_FieldInfo = FieldInfos[Table_Name];
                            foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                            {
                                Relation_Column_Name = relation_entry.Key;
                                if (UtilString.Contains(relation_entry.Key.ToUpper(), "NAME", "TITLE", "URL"))
                                {
                                    break;
                                }
                            }
                            Relation_Unit_Template = "\r\n                { name: '" + Relation_Column_Name + "_Parent',type: 'string' },\r\n";
                            Relation_Unit_Template += "                { name: '" + ClassName + "ShowAll',type: 'string' },";
                        }
                        else if (TableInfoList.ContainsKey(Relation_Table_Name))
                        {
                            Relation_Table_Comment = TableInfoList[Relation_Table_Name]["Comment"];
                            string[] t_c = Relation_Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (t_c.Length > 1) Relation_Table_Comment = t_c[0];
                            Relation_InstanceName = UtilString.LcFirst(Relation_Class_Name);
                            Relation_Column_Name = "";
                            Dictionary<string, Dictionary<string, string>> Relation_FieldInfo = FieldInfos[Relation_Table_Name];
                            foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                            {
                                Relation_Column_Name = relation_entry.Key;
                                if (UtilString.Contains(relation_entry.Key.ToUpper(), "NAME", "TITLE", "URL")) break;
                            }
                            RelationStoreTemplate = @"
    /**
     * {$Column_Comment}所属{$Relation_Table_Comment}
     */
    {$Relation_InstanceName}StoreForCombo:new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '../HttpData/Core/{$Relation_Class_Name}.ashx'
        }),
        reader: new Ext.data.JsonReader({
            root: '{$Relation_InstanceName}s',
            autoLoad: true,
            totalProperty: 'totalCount',
            idProperty: '{$Column_Name}'
        }, [
            {name: '{$Column_Name}', mapping: '{$Column_Name}'},
            {name: '{$Relation_Column_Name}', mapping: '{$Relation_Column_Name}'}
        ])
    }),";
                            RelationStoreTemplate = RelationStoreTemplate.Replace("{$Relation_Column_Name}", Relation_Column_Name);
                            RelationStoreTemplate = RelationStoreTemplate.Replace("{$Relation_Table_Comment}", Relation_Table_Comment);
                            RelationStoreTemplate = RelationStoreTemplate.Replace("{$Relation_Class_Name}", Relation_Class_Name);
                            RelationStoreTemplate = RelationStoreTemplate.Replace("{$Relation_InstanceName}", Relation_InstanceName);
                            RelationStoreTemplate = RelationStoreTemplate.Replace("{$Column_Name}", Column_Name);
                            RelationStoreTemplate = RelationStoreTemplate.Replace("{$Column_Comment}", Column_Comment);
                            RelationStore += RelationStoreTemplate;
                            Relation_Unit_Template = @"
                { name: '{$Relation_Column_Name}', type: 'string' },";
                            Relation_Unit_Template = Relation_Unit_Template.Replace("{$Relation_Column_Name}", Relation_Column_Name);
                        }
                    }
                }
                Unit_Template = Unit_Template.Replace("{$Column_Name}", Column_Name);
                Unit_Template = Unit_Template.Replace("{$ColumnFormat}", ColumnFormat);
                Fields += Unit_Template.Replace("{$Column_Type}", Column_Type);
                Fields += Relation_Unit_Template;
                Column_Type = entry.Value["Type"];
                if (Column_Type.Equals("tinyint"))
                {
                    Column_Comment = entry.Value["Comment"];
                    c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length > 1)
                    {
                        Unit_Template = @"
                { name: '{$Column_Name}Show', type: 'string' },";
                        Unit_Template = Unit_Template.Replace("{$Column_Name}", Column_Name);
                        Fields += Unit_Template;

                    }
                }
                Column_Type = entry.Value["Type"];
                Column_Length = entry.Value["Length"];
                int iLength = UtilNumber.Parse(Column_Length);
                if (ColumnIsTextArea(Column_Name, Column_Type, iLength))
                {
                    Unit_Template = @"
                { name: '{$Column_Name}Show', type: 'string' },";
                    Unit_Template = Unit_Template.Replace("{$Column_Name}", Column_Name);
                    Fields += Unit_Template;
                }
            }
            Fields = Fields.Substring(2,Fields.Length-3);
            if(!string.IsNullOrEmpty(RelationStore)&&RelationStore.Length>3)RelationStore = RelationStore.Substring(0, RelationStore.Length - 1);
            Result["fields"]=Fields;
            Result["relationStore"] = RelationStore;
            return Result;
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
                    return "int";
                case "datetime":
                    return "date";
            }
            return "string";
        }

        /// <summary>
        /// 获取Ext "EditWindow"里items的fieldLabels
        /// </summary>
        /// <param name="Table_Name">表名称</param>
        /// <param name="Ns_Alias">应用别名</param>
        /// <param name="Blank_Pre">空格字符串</param>
        /// <returns></returns>
        public Dictionary<string, string> Model_FieldLables(string Table_Name, string Ns_Alias, string Blank_Pre = "")
        {
            Dictionary<string, string> Result;
            string FieldLabels;//Ext "EditWindow"里items的fieldLabels
            string Unit_Template, ClassName;
            string Relation_Table_Name, Relation_Table_Comment, Relation_Class_Name, Relation_InstanceName, Relation_Column_Name, Relation_Column_Comment;
            string Column_Name, Column_Type,Column_Length, Column_Comment, ColumnFormat = "";
            bool IsImage;
            Result = new Dictionary<string,string>();
            FieldLabels = "";
            ClassName = Table_Name;
            Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
            FieldLabels = "                              { xtype: 'hidden', name: 'ID', ref: '../ID' },";
            Unit_Template = "";
            string TreeLevelVisible_Add   ="",TreeLevelVisible_Update="";
            string Password_Add = "", Password_Update = "";
            Result["IsFileUpload"]="";
            foreach (KeyValuePair<String, Dictionary<string, string>> entry in FieldInfo)
            {
                Column_Name = entry.Key;
                ColumnFormat = "";
                if (Column_Name.ToUpper().Equals("ID")) continue;
                if ((Column_Name.ToUpper().Equals("COMMITTIME")) || (Column_Name.ToUpper().Equals("UPDATETIME"))) continue;
                Column_Comment = entry.Value["Comment"];
                Column_Type = entry.Value["Type"];

                Column_Length = entry.Value["Length"];
                int iLength = UtilNumber.Parse(Column_Length);

                IsImage = ColumnIsImage(Column_Name,Column_Comment);
                if (IsImage)
                {
                    string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length > 0) Column_Comment = c_c[0].Trim(); else Column_Comment = Column_Name;
                    Result["IsFileUpload"]="fileUpload: true,";
                    Unit_Template = Blank_Pre + "                            {xtype: 'hidden',  name : '{$Column_Name}',ref:'../{$Column_Name}'},\r\n";
                    Unit_Template += Blank_Pre + "                            {fieldLabel : '{$Column_Comment}',name : '{$Column_Name}Upload',ref:'../{$Column_Name}Upload',xtype:'fileuploadfield',\r\n" +
                                   Blank_Pre + "                              emptyText: '请上传{$Column_Comment}文件',buttonText: '',accept:'image/*',buttonCfg: {iconCls: 'upload-icon'}";

                    Unit_Template = Unit_Template.Replace("{$Column_Name}", Column_Name);
                    Unit_Template = Unit_Template.Replace("{$Column_Comment}", Column_Comment);
                }else if (Column_Name.ToUpper().Contains("_ID"))
                {
                    Relation_Table_Name = Column_Name.Replace("_ID", "");
                    if (TableList.Contains(Relation_Table_Name) || (Relation_Table_Name.ToUpper().Equals("PARENT")))
                    {
                        Relation_Class_Name = Relation_Table_Name;
                        if (Relation_Table_Name.ToUpper().Equals("PARENT"))
                        {
                            ClassName = Table_Name;
                            Relation_Table_Name = Table_Name;
                            Relation_Class_Name = ClassName;

                            Relation_Table_Comment = TableInfoList[Relation_Table_Name]["Comment"];
                            string[] t_c = Relation_Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (t_c.Length > 1) Relation_Table_Comment = t_c[0];
                            Relation_InstanceName = Relation_Class_Name;
                            Relation_Column_Name = ""; Relation_Column_Comment = "";
                            Dictionary<string, Dictionary<string, string>> Relation_FieldInfo = FieldInfos[Relation_Table_Name];
                            foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                            {
                                Relation_Column_Name = relation_entry.Key;
                                if (UtilString.Contains(relation_entry.Key.ToUpper(), "NAME", "TITLE", "URL"))
                                {
                                    break;
                                }
                            } 
                            string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (c_c.Length >= 1) Column_Comment = c_c[0];
                            Column_Comment = Column_Comment.Replace("标识", "");
                            TreeLevelVisible_Add="\r\n"+
													Blank_Pre+"            $ns_alias.$classname.View.Running.edit_window.{$Relation_InstanceName}comp.btnModify.setVisible(false);\r\n"+
													Blank_Pre+"            $ns_alias.$classname.View.Running.edit_window.{$Relation_InstanceName}comp.{$Relation_InstanceName}_Name.setVisible(true);\r\n"+
													Blank_Pre+"            $ns_alias.$classname.View.Running.edit_window.{$Relation_InstanceName}comp.{$Relation_InstanceName}ShowLabel.setVisible(false);\r\n"+
													Blank_Pre+"            $ns_alias.$classname.View.Running.edit_window.{$Relation_InstanceName}comp.{$Relation_InstanceName}ShowValue.setVisible(false);\r\n";
							TreeLevelVisible_Update="\r\n"+
														Blank_Pre+"            if (this.getSelectionModel().getSelected().data.{$Relation_InstanceName}ShowAll){\r\n"+
														Blank_Pre+"                $ns_alias.$classname.View.Running.edit_window.{$Relation_InstanceName}comp.btnModify.setVisible(true);\r\n"+
														Blank_Pre+"                $ns_alias.$classname.View.Running.edit_window.{$Relation_InstanceName}comp.{$Relation_InstanceName}_Name.setVisible(false);\r\n"+
														Blank_Pre+"                $ns_alias.$classname.View.Running.edit_window.{$Relation_InstanceName}comp.{$Relation_InstanceName}ShowLabel.setVisible(true);\r\n"+
														Blank_Pre+"                $ns_alias.$classname.View.Running.edit_window.{$Relation_InstanceName}comp.{$Relation_InstanceName}ShowValue.setVisible(true);\r\n"+
														Blank_Pre+"            }else{\r\n"+
														Blank_Pre+"                $ns_alias.$classname.View.Running.edit_window.{$Relation_InstanceName}comp.btnModify.setVisible(false);\r\n"+
														Blank_Pre+"                $ns_alias.$classname.View.Running.edit_window.{$Relation_InstanceName}comp.{$Relation_InstanceName}_Name.setVisible(true);\r\n"+
														Blank_Pre+"                $ns_alias.$classname.View.Running.edit_window.{$Relation_InstanceName}comp.{$Relation_InstanceName}ShowLabel.setVisible(false);\r\n"+
														Blank_Pre+"                $ns_alias.$classname.View.Running.edit_window.{$Relation_InstanceName}comp.{$Relation_InstanceName}ShowValue.setVisible(false);\r\n"+
														Blank_Pre+"            }\r\n";


                            TreeLevelVisible_Add = TreeLevelVisible_Add.Replace("$ns_alias", JsNamespace_Alias);
                            TreeLevelVisible_Add = TreeLevelVisible_Add.Replace("$classname", ClassName);
                            TreeLevelVisible_Add = TreeLevelVisible_Add.Replace("{$Relation_InstanceName}", Relation_InstanceName);

                            TreeLevelVisible_Update = TreeLevelVisible_Update.Replace("$ns_alias", JsNamespace_Alias);
                            TreeLevelVisible_Update = TreeLevelVisible_Update.Replace("$classname", ClassName);
                            TreeLevelVisible_Update = TreeLevelVisible_Update.Replace("{$Relation_InstanceName}", Relation_InstanceName);

                            Unit_Template = @"
                            {xtype: 'hidden',name : 'Parent_ID',ref:'../Parent_ID'},
                            {
                                  xtype: 'compositefield',ref: '../{$Relation_InstanceName}comp',
                                  items: [
                                      {
                                          xtype:'combotree', fieldLabel:'{$Column_Comment}',ref:'{$Relation_InstanceName}_Name',name: '{$Relation_InstanceName}_Name',grid:this,
                                          emptyText: '请选择{$Column_Comment}',canFolderSelect:true,flex:1,editable:false,
                                          tree: new Ext.tree.TreePanel({
                                              dataUrl: '../HttpData/Core/Tree/{$ClassName}Tree.ashx',
                                              root: {nodeType: 'async'},border: false,rootVisible: false,
                                              listeners: {
                                                  beforeload: function(n) {if (n) {this.getLoader().baseParams.id = n.attributes.id;}}
                                              }
                                          }),
                                          onSelect: function(cmb, node) {
                                              this.grid.Parent_ID.setValue(node.attributes.id);
                                              this.setValue(node.attributes.text);
                                          }
                                      },
                                      {xtype:'button',text : '修改{$Column_Comment}',ref: 'btnModify',iconCls : 'icon-edit',
                                       handler:function(){
                                           this.setVisible(false);
                                           this.ownerCt.ownerCt.{$Relation_InstanceName}_Name.setVisible(true);
                                           this.ownerCt.ownerCt.{$Relation_InstanceName}ShowLabel.setVisible(true);
                                           this.ownerCt.ownerCt.{$Relation_InstanceName}ShowValue.setVisible(true);
                                           this.ownerCt.ownerCt.doLayout();
                                      }},
                                      {xtype:'displayfield',value:'所选{$Column_Comment}:',ref: '{$Relation_InstanceName}ShowLabel'},{xtype:'displayfield',name:'{$Relation_InstanceName}ShowAll',flex:1,ref: '{$Relation_InstanceName}ShowValue'}]
                            },";
                            Unit_Template = Unit_Template.Replace("{$Relation_Column_Name}", Relation_Column_Name);
                            Unit_Template = Unit_Template.Replace("{$Relation_Table_Comment}", Relation_Table_Comment);
                            Unit_Template = Unit_Template.Replace("{$Column_Comment}", Column_Comment);
                            Unit_Template = Unit_Template.Replace("{$Relation_InstanceName}", Relation_InstanceName);
                            Unit_Template = Unit_Template.Replace("{$ClassName}", ClassName);
                            Unit_Template = Unit_Template.Replace("{$Column_Name}", Column_Name);
                        }else if (TableInfoList.ContainsKey(Relation_Table_Name))
                        {
                            Relation_Table_Comment = TableInfoList[Relation_Table_Name]["Comment"];
                            string[] t_c = Relation_Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (t_c.Length > 1) Relation_Table_Comment = t_c[0];
                            Relation_InstanceName = UtilString.LcFirst(Relation_Class_Name);
                            Relation_Column_Name = ""; Relation_Column_Comment = "";
                            Dictionary<string, Dictionary<string, string>> Relation_FieldInfo = FieldInfos[Relation_Table_Name];
                            foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                            {
                                Relation_Column_Name = relation_entry.Key;
                                if (UtilString.Contains(relation_entry.Key.ToUpper(), "NAME", "TITLE", "URL"))
                                {
                                    Relation_Column_Comment = relation_entry.Value["Comment"];
                                    string[] c_c = Relation_Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (c_c.Length >= 1) Relation_Column_Comment = c_c[0];
                                    Relation_Column_Comment = Relation_Column_Comment.Replace("标识","");
                                    break;
                                }
                            }
                            Unit_Template = @"
                            {
                                fieldLabel: '{$Relation_Column_Comment}', xtype: 'combo', name: '{$Relation_Column_Name}', ref: '../{$Relation_Column_Name}',
                                store: $ns_alias.{$ClassName}.Store.{$Relation_InstanceName}StoreForCombo, emptyText: '请选择{$Relation_Column_Comment}', itemSelector: 'div.search-item',
                                loadingText: '查询中...', width: 570, pageSize: $ns_alias.{$ClassName}.Config.PageSize,
                                displayField: '{$Relation_Column_Name}', grid: this,
                                mode: 'remote', editable: true, minChars: 1, autoSelect: true, typeAhead: false,
                                forceSelection: true, triggerAction: 'all', resizable: false, selectOnFocus: true,
                                tpl: new Ext.XTemplate(
                                    '<tpl for="".""><div class=""search-item"">',
                                        '<h3>{{$Relation_Column_Name}}</h3>',
                                    '</div></tpl>'
                                ),
                                listeners: {
                                    'beforequery': function (event) { delete event.combo.lastQuery; }
                                },
                                onSelect: function (record, index) {
                                    if (this.fireEvent('beforeselect', this, record, index) !== false) {
                                        this.grid.{$Column_Name}.setValue(record.data.{$Column_Name});
                                        this.grid.{$Relation_Column_Name}.setValue(record.data.{$Relation_Column_Name});
                                        this.collapse();
                                    }
                                }
                            },";
                            Unit_Template = Unit_Template.Replace("{$Relation_Column_Name}", Relation_Column_Name);
                            Unit_Template = Unit_Template.Replace("{$Relation_Table_Comment}", Relation_Table_Comment);
                            Unit_Template = Unit_Template.Replace("{$Relation_Column_Comment}", Relation_Column_Comment);
                            Unit_Template = Unit_Template.Replace("{$Relation_InstanceName}", Relation_InstanceName);
                            Unit_Template = Unit_Template.Replace("{$ClassName}", ClassName);
                            Unit_Template = Unit_Template.Replace("{$Column_Name}", Column_Name);
                            Unit_Template = Unit_Template.Replace("$ns_alias", JsNamespace_Alias);
                        }
                    }
                }else if(ColumnIsPassword(Table_Name,Column_Name)){
                    string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length > 0) Column_Comment = c_c[0].Trim(); else Column_Comment = Column_Name;

                    Unit_Template = Blank_Pre + "\r\n                            { fieldLabel : '{$Column_Comment}(<font color=red>*</font>)',name : '{$Column_Name}',inputType:'{$Column_Name}',ref:'../{$Column_Name}' },\r\n";
                    Unit_Template += Blank_Pre + "                            { xtype: 'hidden',name : '{$Column_Name}_old',ref:'../{$Column_Name}_old' },";
                    Password_Add += Blank_Pre + "            var {$Column_Name}Obj=$ns_alias.$classname.View.Running.edit_window.{$Column_Name};\r\n" +
								   Blank_Pre+"            {$Column_Name}Obj.allowBlank=false;\r\n"+
								   Blank_Pre+"            if ({$Column_Name}Obj.getEl()) {$Column_Name}Obj.getEl().dom.parentNode.previousSibling.innerHTML =\"{$Column_Comment}(<font color=red>*</font>)\";\r\n";
					Password_Update+="\r\n"+
									  Blank_Pre+"            var {$Column_Name}Obj=$ns_alias.$classname.View.Running.edit_window.{$Column_Name};\r\n"+
									  Blank_Pre+"            {$Column_Name}Obj.allowBlank=true;\r\n"+
									  Blank_Pre+"            if ({$Column_Name}Obj.getEl()){$Column_Name}Obj.getEl().dom.parentNode.previousSibling.innerHTML =\"{$Column_Comment}\";\r\n"+
									  Blank_Pre+"            $ns_alias.$classname.View.Running.edit_window.{$Column_Name}_old.setValue(this.getSelectionModel().getSelected().data.{$Column_Name}.getValue());\r\n"+
									  Blank_Pre+"            $ns_alias.$classname.View.Running.edit_window.{$Column_Name}.setValue(\"\");\r\n";


                    Unit_Template = Unit_Template.Replace("{$Column_Name}", Column_Name);
                    Unit_Template = Unit_Template.Replace("{$Column_Comment}", Column_Comment);
                    Unit_Template = Unit_Template.Replace("$ns_alias", JsNamespace_Alias);
                    Unit_Template = Unit_Template.Replace("{$classname}", ClassName);

                    Password_Add = Password_Add.Replace("{$Column_Name}", Column_Name);
                    Password_Add = Password_Add.Replace("{$Column_Comment}", Column_Comment);
                    Password_Add = Password_Add.Replace("$ns_alias", JsNamespace_Alias);
                    Password_Add = Password_Add.Replace("{$classname}", ClassName);

                    Password_Update = Password_Update.Replace("{$Column_Name}", Column_Name);
                    Password_Update = Password_Update.Replace("{$Column_Comment}", Column_Comment);
                    Password_Update = Password_Update.Replace("$ns_alias", JsNamespace_Alias);
                    Password_Update = Password_Update.Replace("{$classname}", ClassName);

                }
                else if (Column_Type.Equals("bit"))
                {
                    string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length > 0) Column_Comment = c_c[0].Trim(); else Column_Comment = Column_Name;
                    Unit_Template ="                            \r\n" +
                                  Blank_Pre + "                            {\r\n" +
                                  Blank_Pre + "                                 fieldLabel: '{$Column_Comment}',xtype:'combo',ref:'../{$Column_Name}',mode : 'local',triggerAction : 'all',\r\n" +
								  Blank_Pre+"                                 lazyRender : true,editable: false,allowBlank : false,valueNotFoundText:'否',\r\n"+
								  Blank_Pre+"                                 store : new Ext.data.SimpleStore({\r\n"+
								  Blank_Pre+"                                     fields : ['value', 'text'],\r\n"+
								  Blank_Pre+"                                     data : [['false', '否'], ['true', '是']]\r\n"+
                                  Blank_Pre + "                                 }),emptyText: '请选择{$Column_Comment}',\r\n" +
								  Blank_Pre+"                                 valueField : 'value',displayField : 'text'\r\n"+
                                  Blank_Pre + "                            },";
                    Unit_Template = Unit_Template.Replace("{$Column_Name}", Column_Name);
                    Unit_Template = Unit_Template.Replace("{$Column_Comment}", Column_Comment);
                }
                else if (Column_Type.Equals("tinyint"))
                {
                    string Enum_Data = "";
                    List<Dictionary<string, string>> Enum_ColumnDefine = new List<Dictionary<string, string>>();
                    string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length > 1)
                    {
                        Enum_ColumnDefine = EnumDefines(Column_Comment);
                        Column_Comment = c_c[0].Trim();
                    }
                    foreach (Dictionary<string, string> entry_enum in Enum_ColumnDefine)
                    {
                        Enum_Data += "['" + entry_enum["Value"] + "','" + entry_enum["Comment"] + "'],";
                    }
                    if (Enum_Data.Length > 1) Enum_Data = Enum_Data.Substring(0, Enum_Data.Length - 1);
                    Unit_Template = @"
                            {
                                fieldLabel: '{$Column_Comment}', hiddenName: '{$Column_Name}', xtype: 'combo', ref: '../{$Column_Name}',
                                mode: 'local', triggerAction: 'all', lazyRender: true, editable: false, allowBlank: false,
                                store: new Ext.data.SimpleStore({
                                    fields: ['value', 'text'],
                                    data: [{$Enum_Data}]
                                }), emptyText: '请选择{$Column_Comment}',
                                valueField: 'value', displayField: 'text'
                            },";
                    Unit_Template = Unit_Template.Replace("{$Enum_Data}", Enum_Data);
                }
                else if (ColumnIsTextArea(Column_Name, Column_Type, iLength))
                {
                    Unit_Template = @"
                            { fieldLabel: '{$Column_Comment}', name: '{$Column_Name}',  xtype: 'textarea', id: '{$Column_Name}', ref: '{$Column_Name}' },";
                }
                else
                {
                    string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length > 0) Column_Comment = c_c[0].Trim(); else Column_Comment = Column_Name;
                    Column_Type = ColumnTypeByDbDefine(Column_Type, Column_Name);
                    if (Column_Type.Equals("date")) ColumnFormat = ",xtype : 'datefield',format : 'Y-m-d'";
                    string AllowBlankStr = "";
                    if (Column_Name.ToUpper().Contains("NAME")) AllowBlankStr = ", allowBlank:false";
                    bool IsPermitNull = true;
                    if (entry.Value["Null"].Equals("否")) IsPermitNull = false;
                    if (!IsPermitNull) AllowBlankStr = ", allowBlank:false";

                    Unit_Template = @"
                            { fieldLabel: '{$Column_Comment}', name: '{$Column_Name}'" + AllowBlankStr + " {$ColumnFormat}},";
                }
                Unit_Template = Unit_Template.Replace("{$Column_Name}", Column_Name);
                Unit_Template = Unit_Template.Replace("{$ColumnFormat}", ColumnFormat);
                FieldLabels += Unit_Template.Replace("{$Column_Comment}", Column_Comment);
            }
            FieldLabels = FieldLabels.Substring(2, FieldLabels.Length - 3);
            Result["FieldLabels"]=FieldLabels;
            Result["TreeLevelVisible_Add"] = TreeLevelVisible_Add;
            Result["TreeLevelVisible_Update"] = TreeLevelVisible_Update;
            Result["Password_Add"] = Password_Add;
            Result["Password_Update"] = Password_Update;
            return Result;
        }

        /// <summary>
        /// Ext "Tabs" 中"onAddItems"包含的viewdoblock
        /// </summary>
        /// <param name="ClassName">数据对象类名</param>
        /// <returns></returns>
        public string Model_Viewblock(string ClassName)
        {
            string Result = "";
            string UnitTemplate = "";
            string Column_Name, Column_Type, Column_Length, Column_Comment;
            string Relation_Table_Name, Relation_Column_Name, Relation_Column_Comment;
            string Table_Name = ClassName;
            Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];

            foreach (KeyValuePair<String, Dictionary<string, string>> entry in FieldInfo)
            {
                Column_Name = entry.Key;
                if (Column_Name.ToUpper().Equals("ID") || Column_Name.ToUpper().Equals("PASSWORD")) continue;
                if (Column_Name.ToUpper().Equals("COMMITTIME") || Column_Name.ToUpper().Equals("UPDATETIME")) continue;
                Column_Type = entry.Value["Type"];
                Column_Comment = entry.Value["Comment"];

                Column_Length = entry.Value["Length"];
                int iLength = UtilNumber.Parse(Column_Length);

                string[] c_c;
                if (Column_Type.Equals("tinyint"))
                {
                    c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length > 1)
                    {
                        Column_Comment = c_c[0].Trim();
                        UnitTemplate = @"
                             '    <tr class=""entry""><td class=""head"">{$Column_Comment}</td><td class=""content"">{{$Column_Name}Show}</td></tr>',";
                        UnitTemplate = UnitTemplate.Replace("{$Column_Name}", Column_Name);
                        UnitTemplate = UnitTemplate.Replace("{$Column_Comment}", Column_Comment); 
                        Result += UnitTemplate;
                        continue;
                    }
                }else if (ColumnIsTextArea(Column_Name, Column_Type, iLength))
                {
                    c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length >= 1) Column_Comment = c_c[0].Trim();
                    UnitTemplate = @"
                             '    <tr class=""entry""><td class=""head"">{$Column_Comment}</td><td class=""content"">{{$Column_Name}Show}</td></tr>',";
                    UnitTemplate = UnitTemplate.Replace("{$Column_Name}", Column_Name);
                    UnitTemplate = UnitTemplate.Replace("{$Column_Comment}", Column_Comment);
                    Result += UnitTemplate;
                    continue;
                }
                else if (Column_Name.ToUpper().Contains("_ID"))
                {
                    Relation_Table_Name = Column_Name.Replace("_ID", "");
                    if (TableList.Contains(Relation_Table_Name) || (Relation_Table_Name.ToUpper().Equals("PARENT")))
                    {

                        if (Relation_Table_Name.ToUpper().Equals("PARENT"))
                        {
                            Relation_Column_Name = "";
                            Dictionary<string, Dictionary<string, string>> Relation_FieldInfo = FieldInfos[Table_Name];
                            foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                            {
                                Relation_Column_Name = relation_entry.Key;
                                if (UtilString.Contains(relation_entry.Key.ToUpper(), "NAME", "TITLE", "URL"))
                                {
                                    break;
                                }
                            }

                            c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (c_c.Length >= 1) Column_Comment = c_c[0];
                            Column_Comment = Column_Comment.Replace("标识", "");
                            UnitTemplate = @"
                             '    <tr class=""entry""><td class=""head"">{$Relation_Column_Comment}</td><td class=""content"">{{$Relation_Column_Name}_Parent}<tpl if=""{$Relation_Column_Name}_Parent"">({" + ClassName + "ShowAll})</tpl></td></tr>',";
                            UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Comment}", Column_Comment);
                            UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Name}", Relation_Column_Name);
                            Result += UnitTemplate;
                            continue;
                        }
                        else if (TableInfoList.ContainsKey(Relation_Table_Name))
                        {
                            Relation_Column_Name = ""; Relation_Column_Comment = "";
                            Dictionary<string, Dictionary<string, string>> Relation_FieldInfo = FieldInfos[Relation_Table_Name];
                            foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                            {
                                Relation_Column_Name = relation_entry.Key;
                                Relation_Column_Comment = relation_entry.Value["Comment"];
                                if (UtilString.Contains(relation_entry.Key.ToUpper(), "NAME", "TITLE", "URL")) break;
                            }
                            UnitTemplate = @"
                             '    <tr class=""entry""><td class=""head"">{$Relation_Column_Comment}</td><td class=""content"">{{$Relation_Column_Name}}</td></tr>',";
                            UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Name}", Relation_Column_Name);
                            UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Comment}", Relation_Column_Comment);
                            Result += UnitTemplate;
                            continue;
                        }
                    }

                }
                c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (c_c.Length >= 1) Column_Comment = c_c[0].Trim();
                UnitTemplate = @"
                             '    <tr class=""entry""><td class=""head"">{$Column_Comment}</td><td class=""content"">{{$Column_Name}}</td></tr>',";
                UnitTemplate = UnitTemplate.Replace("{$Column_Name}", Column_Name);
                UnitTemplate = UnitTemplate.Replace("{$Column_Comment}", Column_Comment);
                Result += UnitTemplate;
            }
            Result = Result.Substring(2, Result.Length -2);
            return Result;
        }

        /// <summary>
        /// Ext "Grid" 中包含的columns
        /// </summary>
        /// <param name="ClassName">数据对象类名</param>
        /// <param name="Blank_Pre">空格字符串</param>
        /// <returns></returns>
        public string Model_Columns(string ClassName, string Blank_Pre = "")
        {
            string Result = "";
            string UnitTemplate = "";
            string Column_Name, Column_Type, Column_Comment;
            string Relation_Table_Name, Relation_Column_Name, Relation_Column_Comment;
            string Table_Name = ClassName;
            Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];

            foreach (KeyValuePair<String, Dictionary<string, string>> entry in FieldInfo)
            {
                Column_Name = entry.Key;
                if (Column_Name.ToUpper().Equals("PASSWORD")) continue;
                if (Column_Name.ToUpper().Equals("COMMITTIME") || Column_Name.ToUpper().Equals("UPDATETIME")) continue;
                Column_Type = entry.Value["Type"];
                Column_Comment = entry.Value["Comment"];
                string[] c_c;

                if (Column_Name.ToUpper().Equals("ID")){
                    UnitTemplate = @"
                        { header: '{$Column_Comment}', dataIndex: 'ID', hidden: true },";
                    UnitTemplate = UnitTemplate.Replace("{$Column_Comment}", Column_Comment);
                    Result += UnitTemplate;
                    continue;
                }
                if (Column_Type.Equals("tinyint"))
                {
                    c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length > 1)
                    {
                        Column_Comment = c_c[0].Trim();
                        UnitTemplate = @"
                        { header: '{$Column_Comment}', dataIndex: '{$Column_Name}Show' },";
                        UnitTemplate = UnitTemplate.Replace("{$Column_Name}", Column_Name);
                        UnitTemplate = UnitTemplate.Replace("{$Column_Comment}", Column_Comment);
                        Result += UnitTemplate;
                        continue;
                    }
                }
                else if (Column_Name.ToUpper().Contains("_ID"))
                {
                    Relation_Table_Name = Column_Name.Replace("_ID", "");
                    if (TableList.Contains(Relation_Table_Name))
                    {
                        Relation_Column_Name = ""; Relation_Column_Comment = "";
                        Dictionary<string, Dictionary<string, string>> Relation_FieldInfo = FieldInfos[Relation_Table_Name];
                        foreach (KeyValuePair<String, Dictionary<string, string>> relation_entry in Relation_FieldInfo)
                        {
                            Relation_Column_Name = relation_entry.Key;
                            Relation_Column_Comment = relation_entry.Value["Comment"];
                            if (UtilString.Contains(relation_entry.Key.ToUpper(), "NAME", "TITLE", "URL")) break;
                        }
                        UnitTemplate = @"
                        { header: '{$Relation_Column_Comment}', dataIndex: '{$Relation_Column_Name}' },";
                        UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Name}", Relation_Column_Name);
                        UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Comment}", Relation_Column_Comment);
                        Result += UnitTemplate;
                        continue;
                    }
                }
                c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (c_c.Length >= 1) Column_Comment = c_c[0].Trim();
                UnitTemplate = @"
                        { header: '{$Column_Comment}', dataIndex: '{$Column_Name}' },";
                UnitTemplate = UnitTemplate.Replace("{$Column_Name}", Column_Name);
                UnitTemplate = UnitTemplate.Replace("{$Column_Comment}", Column_Comment);
                Result += UnitTemplate;
            }
            Result = Result.Substring(2, Result.Length - 3);
            return Result;
        }
    }
}
