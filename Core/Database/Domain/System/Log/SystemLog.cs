using Util.Common;
using Util.Log;
using System;

namespace Database.Domain.System.Log
{
    /// <summary>
    /// 系统日志
    /// </summary>
    class SystemLog:Log
    {

        public static void log(string message)
        {
            if (LogManager.GetLogger(LogManager.SystemLoggerName) != null)
            {
                message = String.Format("【{0}】{1}", UtilDateTime.Now(), message);
                LogManager.GetLogger(LogManager.SystemLoggerName).Info(message);
            }
        }

        /// <summary>
        /// 最近一次日志的文件路径
        /// </summary>
        /// <returns></returns>
        public static string RecentLogFileName()
        {
            return RecentLogFileName(LogManager.SystemLoggerName);
        }
    }
}
