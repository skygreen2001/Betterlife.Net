using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Xml;
using Database.Domain.System.Config;
using Util.Common;

namespace Database.Domain.System
{
    /// <summary>
    /// 应用配置
    /// 主要用于获取系统配置文件的信息
    /// </summary>
    public class AppConfig 
    {
        //private static string AppConfigFileName = "App.config";
        private static AppConfig appConfig;

        private XmlDocument xmlDoc;
        private XmlNode xnode;

        /// <summary>
        /// 当前应用配置对象
        /// </summary>
        private Configuration currentConfig;

        public static AppConfig Instance()
        {
            if (appConfig == null)
            {
                appConfig = new AppConfig();
            }
            return appConfig;
        }
        
        //private static string AppConfigPath()
        //{
        //    return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConfigFileName);
        //}

        /// <summary>
        /// 获取当前应用配置
        /// </summary>
        /// <returns></returns>
        public Configuration getCurrentConfig()
        {
            if (currentConfig==null){
                currentConfig = UtilSystem.getCurrentConfig();
            }
            return currentConfig;
        }

        /// <summary>
        /// 更新系统配置
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void updateAppSettings(string key, string value)
        {
            Configuration config = AppConfig.Instance().getCurrentConfig();
            if (config.AppSettings.Settings[key] != null)
            {
                config.AppSettings.Settings[key].Value = value;
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 更新配置
        /// 前提条件是：
        ///     NameValueCollection，并且以add开头，在整个配置文档里key的命名是唯一的。
        /// 说明：  
        ///     由于Section得到的是ReadOnlyNameValueCollection,无法进行修改，
        ///     因此需要使用XML读取文档的方式进行修改
        /// </summary>
        /// Manipulate XML data with XPath and XmlDocument (C#)
        /// <see cref="http://www.codeproject.com/KB/cpp/myXPath.aspx"/>
        /// <param name="key">SectionName</param>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void updateSettingsNameValueCollectionUniqueKey(string key, string value)
        {            
            xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            xnode = xmlDoc.SelectSingleNode("//add[@key='" + key + "']");
            if (xnode != null)
            {
                xnode.Attributes.GetNamedItem("value").Value = value;
            }
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            //ConfigurationManager.RefreshSection(SectionName);
        }

        /// <summary>
        /// 更新设置了SectionPation下指定name的值
        /// </summary>
        /// <param name="sectionPath"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void updateSettingsViewUI(string sectionPath, string name, string propertyName,string propertyValue)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            xnode = xmlDoc.SelectSingleNode("/configuration/" + sectionPath + "/column[@name='" + name + "']");
            if (xnode != null)
            {
                xnode.Attributes.GetNamedItem(propertyName).Value = propertyValue;
            }
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

        }

        /// <summary>
        /// 获取本应用路径
        /// </summary>
        /// <returns></returns>
        public static string appPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.SetupInformation.ApplicationName);
        }

        /// <summary>
        /// 获取系统环境路径
        /// </summary>
        public static string windowSystemFolder(string systemVarible)
        {
            return Environment.GetEnvironmentVariable(systemVarible);
        }

        /// <summary>
        /// 获取系统配置指定SectionName的Section内容
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static NameValueCollection GetSection(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName) as NameValueCollection;
        }

        /// <summary>
        /// 获取系统配置指定SectionName的Section内容
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static Dictionary<string, ViewColumnsSection> GetSectionViewColumns(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName) as Dictionary<string, ViewColumnsSection>;
        }
        
    }
}
