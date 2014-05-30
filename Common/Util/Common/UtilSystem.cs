//using System.Drawing;
using System.Configuration;
using System.Web.Configuration;

/// <summary>
///应用程序类型
/// </summary>
public class EnumAppType
{
    /// <summary>
    /// 应用程序Exe：如Windows Form
    /// </summary>
    public const char AppExe = '0';
    /// <summary>
    /// Web网站
    /// </summary>
    public const char Web = '1';
}
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
        public static Configuration getCurrentConfig(char AppType = EnumAppType.Web)
        {
            Configuration result;
            if (AppType == EnumAppType.Web)
            {
                result = WebConfigurationManager.OpenWebConfiguration("/");
            }
            else
            {
                result = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            return result;
        }


    }
}
