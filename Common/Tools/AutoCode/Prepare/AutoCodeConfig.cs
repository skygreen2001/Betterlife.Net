using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Util.Common;

namespace Tools.AutoCode.Prepare
{
    public class AutoCodeConfig : AutoCodeBase
    {
        /// <summary>
        /// 生成条件的个数
        /// </summary>
        private static int count_condition = 4;

        /// <summary>
        /// 运行主程序
        /// 1.生成配置文件:autocode.config.xml
        /// </summary>
        public void Run()
        {
            base.Init();
            //1.生成配置文件:autocode.config.xml
            App_Dir = App_Dir + Path.DirectorySeparatorChar + "AutoCode" + Path.DirectorySeparatorChar + "Model" + Path.DirectorySeparatorChar;
            string Save_Dir = App_Dir + "autocode.config.xml";
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);

            XElement xElement = new XElement(new XElement("classes"));
            foreach (string Table_Name in TableList)
            {
                if (TableInfoList.ContainsKey(Table_Name))
                {
                    //添加查询条件配置
                    XElement ConditionsElement=new XElement("conditions");
                    ConditionsToConfig(Table_Name, ConditionsElement);

                    //表关系主键显示配置
                    XElement RelationShowsElement = new XElement("relationShows");
                    RelationShowsToConfig(Table_Name, RelationShowsElement);
                    if (ConditionsElement.Elements().Count()<= 0)ConditionsElement = null;
                    if (RelationShowsElement.Elements().Count() <= 0)RelationShowsElement = null;  
                    xElement.Add(
                        new XElement("class", 
                            ConditionsElement, 
                            RelationShowsElement, 
                            new XAttribute("name", Table_Name)
                        )
                    );

                }
            }

            //需要指定编码格式，否则在读取时会抛：根级别上的数据无效。 第 1 行 位置 1异常
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.Indent = true;
            XmlWriter xw = XmlWriter.Create(Save_Dir, settings);
            xElement.Save(xw);
            //写入文件
            xw.Flush();
            xw.Close();

            //表五种关系映射配置
            XDocument xmlDoc = XDocument.Load(Save_Dir);
            foreach (string Table_Name in TableList)
            {
                RelationFives(Table_Name, xmlDoc);
            }
            xmlDoc.Save(Save_Dir);
            Debug.WriteLine(xElement.ToString());
        }

        /// <summary>
        /// 添加查询条件配置
        /// </summary>
        /// <param name="Table_Name">表名称</param>
        /// <param name="conditionsElement">条件生成元素</param>
        private void ConditionsToConfig(string Table_Name, XElement conditionsElement)
        {
            string ClassName = Table_Name;

            List<string> Exists_Condition = new List<string>();
            string ShowFieldName = "";
            if (!IsMany2ManyByClassname(ClassName))
            {
                ShowFieldName = GetShowFieldNameByClassname(ClassName);
                if (!string.IsNullOrEmpty(ShowFieldName))
                {

                    bool IsValid = true;
                    string[] MultiTablesMaybe = Regex.Split(Table_Name, "Re", RegexOptions.IgnoreCase);
                    if (MultiTablesMaybe.Count() > 1)
                    {
                        IsValid = false;
                        foreach (string TableName in MultiTablesMaybe)
                        {
                            if (!TableList.Contains(TableName))
                            {
                                IsValid = true;
                                break;
                            }
                        }
                    }
                    if (IsValid)
                    {
                        if (!string.IsNullOrEmpty(ShowFieldName))
                        {
                            conditionsElement.Add(new XElement("condition", ShowFieldName));
                            Exists_Condition.Add(ShowFieldName);
                        }
                    }
                }
            }
            Dictionary<string, Dictionary<string, string>> Fields = FieldInfos[Table_Name];
            foreach (KeyValuePair<string, Dictionary<string, string>> Entry in Fields)
            {
                string Fieldname = Entry.Key;
                if (Fieldname.ToUpper().Equals("ID")) continue;
                if (UtilString.Contains(Fieldname.ToUpper(), "COMMITTIME", "UPDATETIME")) continue;

                if (!string.IsNullOrEmpty(ShowFieldName))
                {
                    if (!Exists_Condition.Contains(Fieldname) && UtilString.Contains(Fieldname, "Name", "Title") && (!Fieldname.Equals(ShowFieldName)) && (!Fieldname.ToUpper().Contains("_ID")))
                    {
                        conditionsElement.Add(new XElement("condition", Fieldname));
                        Exists_Condition.Add(Fieldname);
                    }
                    if (conditionsElement.Elements().Count() < count_condition)
                    {
                        if (!Exists_Condition.Contains(Fieldname) && UtilString.Contains(Fieldname.ToUpper(), "CODE", "_NO", "STATUS", "TYPE") && (!Fieldname.ToUpper().Contains("_ID")))
                        {
                            conditionsElement.Add(new XElement("condition", Fieldname));
                            Exists_Condition.Add(Fieldname);
                        }
                        if (Fieldname.ToUpper().Contains("_ID") && (!Fieldname.ToUpper().Contains("PARENT_ID")))
                        {
                            string relation_classname = Fieldname.Replace("_ID", "");
                            relation_classname = UtilString.UcFirst(relation_classname);

                            string ShowFieldName_Relation = "";
                            if (TableList.Contains(relation_classname))
                            {
                                ShowFieldName_Relation = GetShowFieldNameByClassname(relation_classname);
                                if (!Exists_Condition.Contains(ShowFieldName_Relation))
                                {

                                    if (!string.IsNullOrEmpty(ShowFieldName_Relation))
                                    {
                                        conditionsElement.Add(new XElement("condition", Fieldname, new XAttribute("relation_class", relation_classname), new XAttribute("show_name", ShowFieldName_Relation)));
                                    }
                                    Exists_Condition.Add(Fieldname);
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 表关系主键显示配置
        /// </summary>
        /// <param name="Table_Name">表名称</param>
        /// <param name="relationShowsElement">表关系主键显示配置生成元素</param>
        private void RelationShowsToConfig(string Table_Name, XElement relationShowsElement)
        {
            string ClassName = Table_Name;
            Dictionary<string, Dictionary<string, string>> Fields = FieldInfos[Table_Name];
            foreach (KeyValuePair<string, Dictionary<string, string>> Entry in Fields)
            {
                string FieldName = Entry.Key;
                if (FieldName.ToUpper().Equals("ID")) continue;
                if (UtilString.Contains(FieldName.ToUpper(), "COMMITTIME", "UPDATETIME")) continue;
                if (FieldName.ToUpper().Contains("_ID"))
                {
                    string Relation_Classname=FieldName.Replace("_ID", "");
                    Relation_Classname = UtilString.UcFirst(Relation_Classname);
                    string ShowFieldName = "";
                    if(Relation_Classname.ToUpper().Equals("PARENT")){
                        ShowFieldName=GetShowFieldNameByClassname(ClassName);
                        if (!string.IsNullOrEmpty(ShowFieldName))
                        {
                            relationShowsElement.Add(new XElement("show", ShowFieldName, new XAttribute("local_key", FieldName), new XAttribute("relation_class", ClassName)));
                        }
                    }else{
                        if(TableList.Contains(Relation_Classname))
                        {
                            ShowFieldName=GetShowFieldNameByClassname(Relation_Classname);
                            if (!string.IsNullOrEmpty(ShowFieldName))
                            {
                                relationShowsElement.Add(new XElement("show", ShowFieldName, new XAttribute("local_key", FieldName), new XAttribute("relation_class", Relation_Classname)));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 表五种关系映射配置
        /// </summary>
        /// <see cref="http://www.xmlplease.com/update-xml-linq" title="Update XML with LINQ to XML in ASP.NET"/>
        /// <param name="Table_Name">表名称</param>
        /// <param name="ClassElement">表五种关系映射配置</param>
        private void RelationFives(string Table_Name, XDocument XmlDoc)
        {
            XElement ClassElement = XmlDoc.XPathSelectElement("classes/class[@name='" + Table_Name + "']");
            XElement Has_OneElement = new XElement("has_one");
            XElement Belong_Has_OneElement = new XElement("belong_has_one");
            XElement Has_ManyElement = new XElement("has_many");
            XElement Many_ManyElement = new XElement("many_many");
            XElement Belongs_Many_ManyElement = new XElement("belongs_many_many");
            string ClassName = Table_Name;
            Dictionary<string, Dictionary<string, string>> Fields = FieldInfos[Table_Name];

            XElement HasManyClassElement = null, HasOneClassElement=null;
            foreach (KeyValuePair<string, Dictionary<string, string>> Entry in Fields)
            {
                string FieldName = Entry.Key;
                if (FieldName.ToUpper().Equals("ID")) continue;
                if (UtilString.Contains(FieldName.ToUpper(), "COMMITTIME", "UPDATETIME")) continue;
                
                if (FieldName.ToUpper().Contains("_ID"))
                {
                    string Relation_Classname=FieldName.Replace("_ID", "");
                    Relation_Classname = UtilString.UcFirst(Relation_Classname);
                    if(Relation_Classname.ToUpper().Equals("PARENT")){
                        string instance_name=UtilString.LcFirst(ClassName);
                        Belong_Has_OneElement.Add("relationclass",instance_name+"_p",new XAttribute("name",ClassName));
                        ClassElement.Add(Belong_Has_OneElement);
                    }else{
                        if(TableList.Contains(Relation_Classname)){
                            //belong_has_one:[当前表有归属表的标识，归属表没有当前表的类名+"_ID"]
                            Dictionary<string, Dictionary<string, string>> Fields_Relation = FieldInfos[Relation_Classname];
                            if(!Fields_Relation.Keys.Contains(ClassName+"_ID"))
                            {
                                string instance_name=UtilString.LcFirst(Relation_Classname);
                                Belong_Has_OneElement.Add(new XElement("relationclass",instance_name,new XAttribute("name",Relation_Classname)));
                            }
                            
    					    //has_many[归属表没有当前表的标识，当前表有归属表的标识]
    					    //has_one:[归属表没有当前表的标识，当前表有归属表的标识，并且当前表里归属表的标识为Unique]
                            if(!Fields_Relation.Keys.Contains(ClassName+"_ID"))
                            {
                                string instance_name=UtilString.LcFirst(ClassName)+"s";
                                bool Is_Unique=false;
                                if(Is_Unique){
                                    HasOneClassElement = XmlDoc.XPathSelectElement("classes/class[@name='" + Relation_Classname + "']");
                                    if (HasManyClassElement.Element("has_one") == null)
                                    {
                                        Has_OneElement.Add(new XElement("relationclass",instance_name,new XAttribute("name",ClassName)));
                                        HasOneClassElement.Add(Has_OneElement);
                                    }else{
                                        HasOneClassElement.Element("has_one").Add(new XElement("relationclass",instance_name,new XAttribute("name",ClassName)));
                                    }
                                }else{
                                    bool is_create_hasmany = true;
                                    if (IsMany2ManyByClassname(ClassName)) is_create_hasmany = false;
                                    if (is_create_hasmany)
                                    {
                                        HasManyClassElement = XmlDoc.XPathSelectElement("classes/class[@name='" + Relation_Classname + "']");
                                        if (HasManyClassElement.Element("has_many") == null)
                                        {
                                            Has_ManyElement.Add(new XElement("relationclass", instance_name, new XAttribute("name", ClassName)));
                                            HasManyClassElement.Add(Has_ManyElement);
                                        }
                                        else
                                        {
                                            HasManyClassElement.Element("has_many").Add(new XElement("relationclass", instance_name, new XAttribute("name", ClassName)));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (Belong_Has_OneElement.Elements().Count() > 0) ClassElement.Add(Belong_Has_OneElement);

            if (IsMany2ManyByClassname(ClassName))
            {
                Dictionary<string, Dictionary<string, string>> FieldInfo_m2m = FieldInfos[Table_Name];
                if (FieldInfo_m2m.Keys.Count() == 5)
                {
                    //many_many[在关系表中有两个关系主键，并且表名的前半部分是其中一个主键]
                    //belongs_many_many[在关系表中有两个关系主键，并且表名的后半部分是其中一个主键]
                    string[] class_onetwo = new string[2];
                    int index = 0;
                    foreach (KeyValuePair<string, Dictionary<string, string>> Entry in Fields)
                    {
                        string FieldName = Entry.Key;
                        if (FieldName.ToUpper().Contains("_ID"))
                        {
                            string class_onetwo_element = FieldName.Replace("_ID", "");
                            class_onetwo.SetValue(FieldName, index);
                            index += 1;
                        }
                    }
                    string ownerClassname = "", belongClassname = "", ownerInstancename = "", belongInstancename = "";
                    if ((class_onetwo[0] + "Re" + class_onetwo[1]).ToLower().Equals(ClassName.ToLower()))
                    {
                        ownerClassname = class_onetwo[0];
                        belongClassname = class_onetwo[1];
                        ownerInstancename = class_onetwo[0] + "s";
                        belongInstancename = class_onetwo[1] + "s";
                    }
                    else if ((class_onetwo[1] + "Re" + class_onetwo[0]).ToLower().Equals(ClassName.ToLower()))
                    {
                        ownerClassname = class_onetwo[1];
                        belongClassname = class_onetwo[0];
                        ownerInstancename = class_onetwo[1] + "s";
                        belongInstancename = class_onetwo[0] + "s";
                    }
                    ownerClassname = UtilString.UcFirst(ownerClassname);
                    belongClassname = UtilString.UcFirst(belongClassname);
                    XElement ClassOwnerElement = XmlDoc.XPathSelectElement("classes/class[@name='" + ownerClassname + "']");
                    if (ClassOwnerElement.Element("many_many") == null)
                    {
                        Many_ManyElement.Add(new XElement("relationclass", belongInstancename, new XAttribute("name", belongClassname)));
                        ClassOwnerElement.Add(Many_ManyElement);
                    }
                    else
                    {
                        ClassOwnerElement.Element("many_many").Add(new XElement("relationclass", belongInstancename, new XAttribute("name", belongClassname)));
                    }

                    XElement ClassBelongElement = XmlDoc.XPathSelectElement("classes/class[@name='" + belongClassname + "']");
                    if (ClassOwnerElement.Element("belongs_many_many") == null)
                    {
                        Belongs_Many_ManyElement.Add(new XElement("relationclass", belongInstancename, new XAttribute("name", belongClassname)));
                        ClassBelongElement.Add(Belongs_Many_ManyElement);
                    }
                    else
                    {
                        ClassBelongElement.Element("belongs_many_many").Add(new XElement("relationclass", ownerInstancename, new XAttribute("name", ownerClassname)));
                    }

                }
            }
        }
    }
}
