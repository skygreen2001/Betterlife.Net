using System.IO;
using Util.Common;

namespace Tools.Config
{
    public static class ToolModifyFile
    {
        private static string configFilePathName = "";//原配置文件路径名
        private static string bakFilePathName = "";//备份文件名.bak
        private static string content = "";//内存中的内容

        /// <summary>
        /// 初始化路径
        /// </summary>
        private static void initPath()
        {
            //原配置文件名
            configFilePathName = @"..\..\..\..\DataBase\BetterlifeEntities.Designer.cs";
            //备份文件名.bak
            bakFilePathName = configFilePathName + ".bak";
        }

        /// <summary>
        /// 恢复源文件
        /// </summary>
        public static void reset()
        {
            initPath();
            File.Copy(bakFilePathName, configFilePathName, true);
        }

        /// <summary>
        /// 修改文件并替换原文件内容
        /// </summary>
        public static void run()
        {
            initPath();
            //备份原文件为新文件名
            if (!File.Exists(bakFilePathName)) File.Copy(configFilePathName, bakFilePathName, true);
            //读取原文件内容到内存
            content = UtilFile.ReadFile2String(configFilePathName);
            //修改原文件内容
            ModifyContent();
            //存入原文件内容
            UtilFile.WriteString2File(configFilePathName, content);
        }

        /// <summary>
        /// 修改原文件内容
        /// </summary>
        private static void ModifyContent()
        {
            content = content.Replace("using System.ComponentModel;", "using System.ComponentModel;\r\nusing Newtonsoft.Json;");
            content = content.Replace("[XmlIgnoreAttribute()]\r\n", "[XmlIgnoreAttribute()]\r\n        [JsonIgnore]\r\n");
            content = content.Replace("[BrowsableAttribute(false)]\r\n", "[BrowsableAttribute(false)]\r\n        [JsonIgnore]\r\n");
        }
    }
}
