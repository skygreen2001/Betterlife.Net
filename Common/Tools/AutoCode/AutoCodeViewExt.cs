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
            string JsNamespace = "BetterlifeNet";
            string JsNamespace_Alias = "Bn";


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
                    Content_New = Content_New.Replace("$fieldLabels", Model_FieldLables(ClassName, JsNamespace_Alias, ""));
                    //Ext "Tabs" 中"onAddItems"包含的viewdoblock
                    Content_New = Content_New.Replace("$viewdoblock", Model_Viewblock(ClassName));
                    //Ext "Grid" 中包含的columns
                    Content_New = Content_New.Replace("$columns", Model_Columns(ClassName, ""));

                    //存入目标文件内容
                    UtilFile.WriteString2File(Save_Dir + InstanceName + ".js", Content_New);
                }
            }
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
            string Column_Name, Column_Type, Column_Comment, ColumnFormat;
            string Relation_Table_Name, Relation_Table_Comment, Relation_Class_Name, Relation_InstanceName, Relation_Column_Name;
            string RelationStore="",RelationStoreTemplate = "";
            Result = new Dictionary<string,string>();
            Fields = "";
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
                    if (TableList.Contains(Relation_Table_Name))
                    {
                        Relation_Class_Name = Relation_Table_Name;
                        if (TableInfoList.ContainsKey(Relation_Table_Name))
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
    })
                        ";
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
                Column_Comment = entry.Value["Comment"];
                if (Column_Type.Equals("tinyint"))
                {
                    c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length > 1)
                    {
                        Unit_Template = @"
                { name: '{$Column_Name}Show', type: 'string' },";
                        Unit_Template = Unit_Template.Replace("{$Column_Name}", Column_Name);
                        Fields += Unit_Template;

                    }
                }
            }
            Fields = Fields.Substring(2,Fields.Length-3);
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
        public string Model_FieldLables(string Table_Name, string Ns_Alias, string Blank_Pre = "")
        {
            string Unit_Template, Result;
            string Relation_Table_Name, Relation_Table_Comment, Relation_Class_Name, Relation_InstanceName, Relation_Column_Name;
            string Column_Name, Column_Type, Column_Comment, ColumnFormat = "";

            Result = "";

            Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
            Result = "                              { xtype: 'hidden', name: 'ID', ref: '../ID' },";
            Unit_Template = "";
            foreach (KeyValuePair<String, Dictionary<string, string>> entry in FieldInfo)
            {
                Column_Name = entry.Key;
                ColumnFormat = "";
                if (Column_Name.ToUpper().Equals("ID")) continue;
                if ((Column_Name.ToUpper().Equals("COMMITTIME")) || (Column_Name.ToUpper().Equals("UPDATETIME"))) continue;
                Column_Comment = entry.Value["Comment"];
                Column_Type = entry.Value["Type"];

                if (Column_Name.ToUpper().Contains("_ID"))
                {
                    Relation_Table_Name = Column_Name.Replace("_ID", "");
                    if (TableList.Contains(Relation_Table_Name))
                    {
                        Relation_Class_Name = Relation_Table_Name;
                        if (TableInfoList.ContainsKey(Relation_Table_Name))
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
                            Unit_Template = @"
                            {
                                fieldLabel: '{$Relation_Table_Comment}', xtype: 'combo', name: '{$Relation_Column_Name}', ref: '../{$Relation_Column_Name}',
                                store: $ns_alias.$classname.Store.{$Relation_InstanceName}StoreForCombo, emptyText: '请选择{$Relation_Table_Comment}', itemSelector: 'div.search-item',
                                loadingText: '查询中...', width: 570, pageSize: $ns_alias.$classname.Config.PageSize,
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
                            Unit_Template = Unit_Template.Replace("{$Relation_InstanceName}", Relation_InstanceName);
                        }
                    }
                }
                else if (Column_Type.Equals("tinyint"))
                {
                    string Enum_Data="";
                    List<Dictionary<string, string>> Enum_ColumnDefine=new List<Dictionary<string,string>>();
                    string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length > 1)
                    {
                        Enum_ColumnDefine = EnumDefines(Column_Comment);
                        Column_Comment = c_c[0].Trim();
                    }
                    foreach (Dictionary<string, string> entry_enum in Enum_ColumnDefine)
                    {
                        Enum_Data +="['" + entry_enum["Value"] + "','" + entry_enum["Comment"] + "'],";
                    }
                    if (Enum_Data.Length > 1) Enum_Data = Enum_Data.Substring(0,Enum_Data.Length-1);
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
                }else
                {
                    string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (c_c.Length > 0) Column_Comment = c_c[0].Trim(); else Column_Comment = Column_Name;
                    Column_Type = ColumnTypeByDbDefine(Column_Type, Column_Name);
                    if (Column_Type.Equals("date")) ColumnFormat = ",xtype : 'datefield',format : 'Y-m-d'";
                    Unit_Template = @"
                            { fieldLabel: '{$Column_Comment}', name: '{$Column_Name}' {$ColumnFormat}},";
                }
                Unit_Template = Unit_Template.Replace("{$Column_Name}", Column_Name);
                Unit_Template = Unit_Template.Replace("{$ColumnFormat}", ColumnFormat);
                Result += Unit_Template.Replace("{$Column_Comment}", Column_Comment);
            }
            Result = Result.Substring(2, Result.Length - 3);
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
            string Column_Name, Column_Type,Column_Comment;
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
                             '    <tr class=""entry""><td class=""head"">{$Relation_Column_Comment}</td><td class=""content"">{{$Relation_Column_Name}}</td></tr>',";
                        UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Name}", Relation_Column_Name);
                        UnitTemplate = UnitTemplate.Replace("{$Relation_Column_Comment}", Relation_Column_Comment);
                        Result += UnitTemplate;
                        continue;
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
