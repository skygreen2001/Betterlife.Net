using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Util.Db;
using Util.Common;

namespace Tools.AutoCode
{
    public class AutoCode
    {
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
        /// 初始化工作
        /// </summary>
        protected void Init()
        {
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
                foreach (string TableName in TableList)
                {
                    FieldInfos.Add(TableName,UtilSqlserver.FieldInfoList(TableName));
                }
            }

        }

    }
}
