using System;


namespace Util.Log
{
    /// <summary>
    /// 对日志的静态封装类
    /// </summary>
    public static class LogHelper
    {

        private static Logger defaultLogger = null;
        static LogHelper()
        {
            defaultLogger = LogManager.GetLogger("");
        }

        /// <summary>
        /// 记录系统信息
        /// </summary>
        /// <param name="message">记录的信息</param>
        public static void Info(string message)
        {
            if (defaultLogger != null)
            {
                defaultLogger.Info(Helper.FormatMessage(message, LogLevel.Info));
            }
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="message">记录的信息</param>
        public static void Warn(string message)
        {
            if (defaultLogger != null)
            {
                defaultLogger.Warn(Helper.FormatMessage(message, LogLevel.Warn));
            }
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="exp">警告的异常信息</param>
        public static void Warn(Exception exp)
        {
            if (defaultLogger != null)
            {
                defaultLogger.Warn(Helper.FormatMessage(Helper.BuiltExpString(exp), LogLevel.Warn));
            }
        }


        /// <summary>
        /// 记录系统运行过程中的错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        public static void Error(string message)
        {
            if (defaultLogger != null)
            {
                defaultLogger.Error(Helper.FormatMessage(message,LogLevel.Error));
            }
        }

        /// <summary>
        /// 记录系统运行过程中的错误信息
        /// </summary>
        /// <param name="exp">错误的异常</param>
        public static void Error(Exception exp)
        {
            if (defaultLogger != null)
            {
                defaultLogger.Error(Helper.FormatMessage(Helper.BuiltExpString(exp),
                    LogLevel.Error));
            }
        }

        /// <summary>
        /// 记录系统运行过程中的错误信息
        /// </summary>
        /// <param name="exp">错误的异常</param>
        public static void Error(string message ,Exception exp)
        {
            if (defaultLogger != null)
            {
                defaultLogger.Error(Helper.FormatMessage(message + Helper.BuiltExpString(exp),
                    LogLevel.Error));
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public static void Dispose()
        {
            defaultLogger.Dispose();
        }
    }
}
