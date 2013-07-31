using System;

namespace Util.Common
{
    /// <summary>
    /// 时间日期工具类
    /// </summary>
    public static class UtilDateTime
    {
        public const string FORMAT_NORMAL = "yyyy-MM-dd HH:mm:ss";
        public const string FORMAT_YYYYMMDD = "yyyy-MM-dd";
        public const string FORMAT_YYYYMMDDHHMMSS="yyyyMMddHHmmss";
        public const string FORMAT_YYYY_MM_DD_HH_MM_SS = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 指定时间格式显示
        /// </summary>
        /// <param name="value">时间</param>
        /// <returns></returns>
        public static String getDateTimeValue(DateTime value, string format)
        {
            return value.ToString(format);
        }

        /// <summary>
        /// 按时间格式：FORMAT_YYYYMMDDHHMMSS显示
        /// </summary>
        /// <returns></returns>
        public static string nowS()
        {
            return UtilDateTime.getDateTimeValue(DateTime.Now, UtilDateTime.FORMAT_YYYYMMDDHHMMSS);
        }

        /// <summary>
        /// 按时间格式：FORMAT_YYYY_MM_DD_HH_MM_SS显示
        /// </summary>
        /// <returns></returns>
        public static string now_underline()
        {
            return UtilDateTime.getDateTimeValue(DateTime.Now, UtilDateTime.FORMAT_YYYY_MM_DD_HH_MM_SS);
        }

        /// <summary>
        /// 按时间格式：FORMAT_NORMAL:yyyy-MM-dd HH:mm:ss显示
        /// </summary>
        /// <returns></returns>
        public static string now()
        {
            return UtilDateTime.getDateTimeValue(DateTime.Now, UtilDateTime.FORMAT_NORMAL);
        }
        
        /// <summary>
        /// 转换Timestamp字符串成DateTime类型
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime convertTimestampToDateTimeValue(string timestamp)
        {
            if (!string.IsNullOrEmpty(timestamp))
            {
                return (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(double.Parse(timestamp));                
            }
            return DateTime.MinValue;
        }
    }
}
