using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Util.Db;
using Util.Common;
using System.Text.RegularExpressions;

namespace Tools.AutoCode
{
    public class AutoCodeBase
    {
        #region 解决方案个性化设置
        /// <summary>
        /// 数据库名称
        /// </summary>
        public static string Database_Name = "BetterlifeNet";
        /// <summary>
        /// 生成的Entities配置文件的名称:在项目Database下*.edmx文件的名称
        /// </summary>
        public static string EntitiesName = "BetterlifeNetEntities";
        /// <summary>
        /// JS命名空间
        /// </summary>
        public static string JsNamespace = "BetterlifeNet";
        /// <summary>
        /// JS命名空间别名
        /// </summary>
        public static string JsNamespace_Alias = "Bn";
        /// <summary>
        /// 特殊字段:CommitTime
        /// </summary>
        public static string CommitTime_Str = "CommitTime";
        /// <summary>
        /// 特殊字段:Updatetime
        /// </summary>
        public static string UpdateTime_Str = "UpdateTime";
        #endregion

        /// <summary>
        /// 生成文件保存的路径
        /// </summary>
        public string Save_Dir;
        /// <summary>
        /// 应用所在的路径
        /// </summary>
        public string App_Dir;
        /// <summary>
        /// 生成源码[services|domain]所在目录名称
        /// </summary>
        public string Dir_Src="src";
        /// <summary>
        /// 实体数据对象类文件所在的路径
        /// </summary>
	    public string Domain_Dir="domain";
	    /// <summary>
	    /// 表列表
	    /// </summary>
        public static List<string> TableList;
        /// <summary>
        /// 所有表信息
        /// </summary>
        public static Dictionary<string, Dictionary<string, string>> TableInfoList;
        /// <summary>
        /// 所有表列信息
        /// </summary>
        public static Dictionary<string,Dictionary<string, Dictionary<string, string>>> FieldInfos;
        /// <summary>
        /// 数据对象关系显示字段
        /// </summary>
        public static string relation_viewfield;
        /// <summary>
        /// 所有的数据对象关系
        /// 一对一，一对多，多对多
        /// 包括*.has_one,belong_has_one,has_many,many_many,belongs_many_many.
        /// 参考说明:EnumTableRelation
        /// </summary>
        public static string relation_all;
        /// <summary>
        /// 数据对象引用另一个数据对象同样值的冗余字段
        /// </summary>
        public static string redundancy_table_fields;
        /// <summary>
        /// 获取类和注释的说明
        /// </summary>
        public static string Class_Comments;
        /// <summary>
        /// 一对多表关系定义数据
        /// </summary>
        public static Dictionary<string, List<string>> OneHasManyDefine;
        /// <summary>
        /// 获取不同表里列名相同的列名 
        /// 不包括列名为ID,CommitTime,UpdateTime
        /// 不包含列名为_ID
        /// </summary>
        public string[] Same_Column_Names;

        /// <summary>
        /// 初始化工作
        /// </summary>
        protected void Init()
        {
            UtilSqlserver.Database_Name = Database_Name;
            if (string.IsNullOrEmpty(App_Dir))
            {
                App_Dir = Directory.GetCurrentDirectory();
                App_Dir = App_Dir + Path.DirectorySeparatorChar + "Model" + Path.DirectorySeparatorChar;
            }
            if (TableInfoList == null)
            {
                TableInfoList = UtilSqlserver.TableinfoList();
                TableList = UtilSqlserver.TableList().Keys.ToList();
            }

            if (FieldInfos == null)
            {
                FieldInfos = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
                foreach (string Table_Name in TableList)
                {
                    FieldInfos.Add(Table_Name, UtilSqlserver.FieldInfoList(Table_Name));
                }
            }
            if (OneHasManyDefine == null)
            {
                OneHasManyDefine = new Dictionary<string, List<string>>();
                string Column_Name,Relation_ClassName;
                List<string> lRelation_TableName=null;
                foreach (string Table_Name in TableList)
                {
                    Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[Table_Name];
                    
                    foreach (KeyValuePair<String, Dictionary<string, string>> entry in FieldInfo)
                    {
                        Column_Name = entry.Key;
                         
                        if (Column_Name.Contains("_ID"))
                        {
                            Relation_ClassName = Column_Name.Replace("_ID", "");
                            if (TableList.Contains(Relation_ClassName))
                            {
                                if (lRelation_TableName == null) lRelation_TableName = new List<string>();
                                if (OneHasManyDefine.Keys.Contains(Relation_ClassName))
                                {
                                    lRelation_TableName = OneHasManyDefine[Relation_ClassName];
                                }
                                else
                                {
                                    lRelation_TableName=new List<string>();
                                    OneHasManyDefine[Relation_ClassName] = lRelation_TableName;
                                }
                                if (!lRelation_TableName.Contains(Table_Name)) lRelation_TableName.Add(Table_Name);
                            }
                        }
                    }
                }
            }
            Same_Column_Names = UtilSqlserver.Same_Column_Name_In_TableName();
            //UtilObjectDump.WriteLine(Same_Column_Names);
        }

        /// <summary>
        /// 获取列注释第一行关键词说明
        /// </summary>
        /// <param name="field_comment">列注释</param>
        /// <param name="Default">默认返回值</param>
        /// <returns></returns>
        protected string ColumnCommentKey(string Column_Comment, string Default = "")
        {
            if (string.IsNullOrEmpty(Column_Comment))
            {
                Column_Comment = Default;
            }
            else
            {
                string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (c_c.Length > 1)
                {
                    Column_Comment = c_c[0];
                }
                if(UtilString.Contains(Column_Comment,"标识","编号","主键"))
                {
                    Column_Comment = Column_Comment.Replace("标识","");
                    Column_Comment = Column_Comment.Replace("编号", "");
                    Column_Comment = Column_Comment.Replace("主键", "");
                }
            }
            return Column_Comment;
        }

        /// <summary>
        /// 表枚举类型列注释转换成可以处理的数组数据
        /// 注释风格如下：
        ///     用户性别
        ///     0：女-female
        ///     1：男-mail
        ///     -1：待确认-unknown
        ///     默认男
        /// </summary>
        /// <param name="Column_Comment">表枚举类型列注释</param>
        /// <returns></returns>
        protected List<Dictionary<string, string>> EnumDefines(string Column_Comment)
        {
            string C_Comment, Enum_Comment;
            string[] Part_Arr, Cn_En_Arr;
            List<Dictionary<string, string>> Result=null;
            string[] c_c = Column_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (c_c.Length > 1)
            {
                Result = new List<Dictionary<string, string>>();
                for (int i = 1; i < c_c.Length; i++)
                {
                    C_Comment = c_c[i].Trim();
                    C_Comment = C_Comment.Replace("：", ":");
                    Part_Arr = C_Comment.Split(':');
                    if ((Part_Arr != null) && (Part_Arr.Length == 2))
                    {
                        Cn_En_Arr = new string[2];
                        Enum_Comment = Part_Arr[1];
                        if (UtilNumber.IsDigit(Part_Arr[0]))
                        {
                            if (Enum_Comment.Contains("-"))
                            {
                                Cn_En_Arr[0] = Enum_Comment.Substring(0, Enum_Comment.IndexOf("-"));
                                Cn_En_Arr[1] = Enum_Comment.Substring(Enum_Comment.IndexOf("-") + 1);
                                Result.Add(new Dictionary<string, string>()
                                            {
                                                {"Name",Cn_En_Arr[1].ToLower()},
                                                {"Value",Part_Arr[0]},
                                                {"Comment",Cn_En_Arr[0]}
                                            });
                            }
                        }
                        else
                        {
                            Result.Add(new Dictionary<string, string>()
                                        {
                                            {"Name",Part_Arr[0].ToLower()},
                                            {"Value",Part_Arr[0].ToLower()},
                                            {"Comment",Part_Arr[1]}
                                        });
                        }
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// 列是否大量文本输入应该TextArea输入
        /// </summary>
        /// <param name="Column_Name">列名称</param>
        /// <param name="Column_Type">列类型</param>
        protected bool ColumnIsTextArea(string Column_Name, string Column_Type, int Column_Length)
        {
            Column_Name = Column_Name.ToUpper();
            if (Column_Name.Contains("ID")) return false;
            if ((Column_Length>=500) && (!UtilString.Contains(Column_Name, "URL","PROFILE", "IMAGES", "LINK", "ICO", "PASSWORD", "EMAIL", "PHONE", "ADDRESS")) ||
            (UtilString.Contains("INTRO", "MEMO", "CONTENT")) ||
            (Column_Type.ToUpper().Contains("TEXT")))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 列是否是图片路径
        /// </summary>
        /// <param name="Column_Name">列名称</param>
        /// <param name="Column_Comment">列注释</param>
        protected bool ColumnIsImage(string Column_Name, string Column_Comment)
        {
            Column_Name = Column_Name.ToUpper();
            if (Column_Name.Contains("ID")) return false;
            if (UtilString.Contains(Column_Name, "PROFILE", "IMAGE", "IMG", "ICO", "LOGO", "PIC")) return true;
            return false;
        }

        /// <summary>
        /// 列是否是密码
        /// </summary>
        /// <param name="Table_Name">表名称</param>
        /// <param name="Column_Name">列名称</param>
        protected bool ColumnIsPassword(string Table_Name, string Column_Name)
        {
            Table_Name = Table_Name.ToUpper();
            if (UtilString.Contains(Table_Name, "MEMBER", "ADMIN", "USER"))
            {
                Column_Name = Column_Name.ToUpper();
                if (Column_Name.Contains("PASSWORD")) return true;
            }
            return false;
        }

        /// <summary>
        /// 列是否是Email
        /// </summary>
        /// <param name="Column_Name">列名称</param>
        /// <param name="Column_Comment">列注释</param>
        protected bool ColumnIsEmail(string Column_Name, string Column_Comment)
        {
            Column_Name=Column_Name.ToUpper();
            if (Column_Name.Contains("EMAIL")||(UtilString.Contains(Column_Comment,"邮件","邮箱")&&(!Column_Name.Contains("IS"))))return true;
		    return false;
        }

        /// <summary>
        /// 根据类名获取表代表列显示名称
        /// </summary>
        /// <param name="ClassName">数据对象类名</param>
        /// <param name="IsReturnNull">是否没有就返回Null</param>
        /// <returns></returns>
        protected string GetShowFieldNameByClassname(string ClassName, bool IsReturnNull=true)
        {
            string TableName = ClassName;
            Dictionary<string, Dictionary<string, string>> FieldInfo = FieldInfos[TableName];
            List<string> FieldNames = FieldInfo.Keys.ToList();
            foreach (string FieldName in FieldNames)
            {
                string FieldName_Filter = FieldName.ToLower();
                if (!FieldName.ToLower().Contains("id"))
                {
                    if (UtilString.Contains(FieldName.ToLower(), "name", "title"))
                    {
                        return FieldName;
                    }
                    string ClassName_Filter = ClassName.ToLower();
                    if (FieldName.ToLower().Contains(ClassName_Filter)) return FieldName;
                }
            }
            if (IsReturnNull) return ""; else return "Name";
        }

        /// <summary>
        /// 根据类名判断是不是多对多关系，存在中间表表名
        /// </summary>
        /// <param name="Classname">数据对象类名</param>
        /// <returns></returns>
	    protected bool IsMany2ManyByClassname(string Classname)
	    {
		    string Tablename=Classname;
            if (TableInfoList.Keys.Contains(Tablename))
            {
                string Comment = TableInfoList[Tablename]["Comment"];
                Dictionary<string, Dictionary<string, string>> Fields = FieldInfos[Tablename];
                if (Tablename.Length >= 8)
                {
                    if (Comment.Contains("关系表"))
                    {
                        int Count = 0;
                        foreach (KeyValuePair<string, Dictionary<string, string>> Entry in Fields)
                        {
                            string Field_Name = Entry.Key;
                            if (Field_Name.Contains("_ID")) Count += 1;

                        }
                        if (Count >= 2) return true;
                    }
                    else
                    {
                        string[] MultiTablesMaybe = Regex.Split(Tablename, "Re", RegexOptions.IgnoreCase);
                        if (MultiTablesMaybe.Count() > 1)
                        {
                            int countTables = 0;
                            foreach (string TableNameMaybe in MultiTablesMaybe)
                            {
                                if (TableList.Contains(TableNameMaybe))
                                {
                                    countTables += 1;
                                }
                            }
                            if (countTables >= 2) return true;
                        }

                    }
                }
            }
		    return false;
	    }

        /// <summary>
        /// 根据类名判断是不是多对多关系，如果存在其他显示字段则需要在显示Tab中显示
        /// </summary>
        /// <param name="Classname">数据对象类名</param>
        /// <returns></returns>
        protected bool IsMany2ManyShowHasMany(string Classname)
        {
            bool isMany2ManyByClassname = IsMany2ManyByClassname(Classname);

            string Tablename = Classname;
            if (isMany2ManyByClassname)
            {
                Dictionary<string, Dictionary<string, string>> Fields = FieldInfos[Tablename];
                if (Fields.Count() == 5) return false;
            }
            return true;
        }

    }
}
