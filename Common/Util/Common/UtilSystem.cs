//using System.Drawing;
using System.Configuration;
using System.Web.Configuration;
namespace Util.Common
{
    /// <summary>
    /// 操作系统工具类
    /// </summary>
    public static class UtilSystem
    {
        /// <summary>
        /// 获取当前应用配置
        /// </summary>
        /// <returns></returns>
        public static Configuration getCurrentConfig()
        {
            return WebConfigurationManager.OpenWebConfiguration("/");

            ///如果是Windows Form
            //return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

    }
}
