using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Util.Reflection;
using Database.Domain.System;
using Database.Domain.Enums.System;
namespace Business
{
    /// <summary>
    /// 应用全局设置
    /// </summary>
    public partial class Gc
    {
        #region 主体部分
        #region 定义部分
        #region 应用设置
        /// <summary>
        /// 网站应用名称
        /// </summary>
        public static string SiteName { set; get; }
        /// <summary>
        /// 网站应用版本
        /// </summary>
        public static string Version { set; get; }
        /// <summary>
        /// 是否进行调试
        /// </summary>
        public static bool IsDebug { set; get; }
        /// <summary>
        /// 是否进行测试走捷径
        /// </summary>
        public static bool IsShortcut { set; get; }
        /// <summary>
        /// 日志路径信息
        /// </summary>
        public static string LogPathInfo { set; get; }
        /// <summary>
        /// 欢迎页面
        /// </summary>
        public static string UrlWelcome { set; get; }
        /// <summary>
        /// 请求超时配置，单位是milliseconds:毫秒
        /// </summary>
        public static string RequestTimeout { set; get; }
        #endregion 应用设置

        #region 当前网站路径
        /// <summary>
        /// 当前网站路径
        /// </summary>
        public static string UrlWebsite { set; get; }
        #endregion 当前网站路径

        #region 网站前台根路径
        /// <summary>
        /// 网站前台根路径
        /// </summary>
        public static string UrlPortal { set; get; }
        #endregion 网站前台根路径

        #region 上传文件物理路径
        /// <summary>
        /// 上传文件物理路径
        /// </summary>
        public static string UploadPath { set; get; }
        #endregion 上传文件物理路径

        #region 上传文件网络路径
        /// <summary>
        /// 上传文件网络路径：可通过设置修改
        /// </summary>
        public static string UploadUrl { set; get; }
        #endregion 上传文件网络路径
        #endregion 定义部分

        #region 初始化
        /// <summary>
        /// 将配置文件App.config里的所有系统配置信息注入到全局变量中供应用使用
        /// </summary>
        public static void init()
        {
            initAppSettings();
        }

        /// <summary>
        /// 将配置文件App.config里的AppSetting配置信息注入到全局变量中供应用使用
        /// </summary>
        private static void initAppSettings()
        {
            Configuration config = AppConfig.Instance().getCurrentConfig();
            
            // Get the KeyValueConfigurationCollection from the configuration.
            KeyValueConfigurationCollection settings = config.AppSettings.Settings;
            Hashtable data = new Hashtable();
            foreach (KeyValueConfigurationElement keyValueElement in settings)
            {
                data.Add(keyValueElement.Key, keyValueElement.Value);
            }
            UtilReflection.SetPublicStaticProperties(typeof(Gc), data);
            UtilReflection.IsDebug = Gc.IsDebug;
        }
        #endregion 初始化
        #endregion 主体部分
    }
}
