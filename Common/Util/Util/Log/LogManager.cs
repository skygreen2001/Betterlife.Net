using System.Collections.Generic;

namespace Util.Log
{
    /// <summary>
    /// Log管理器
    /// </summary>
    public static class LogManager
    {
        /// <summary>
        /// 系统日志的Logger名
        /// </summary>
        public static readonly string SystemLoggerName = "systemLog";
        /// <summary>
        /// 座席日志的Logger名
        /// </summary>
        public static readonly string AgentLoggerName = "agentLog";
        /// <summary>
        /// 来电日志的Logger名
        /// </summary>
        public static readonly string CallLoggerName = "callLog";

        /// <summary>
        /// 日志字典
        /// </summary>
        private readonly static Dictionary<string, Logger> logDic = new Dictionary<string, Logger>();

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static LogManager()
        {
            LogConfigSetting[] logConfigSettings = ConfigurationSectionHandler.GetSection();
            foreach (LogConfigSetting logConfigSetting in logConfigSettings)
            {
                if (logConfigSetting != null)
                {
                    Logger logger = new Logger { LogLevel = logConfigSetting.logLevel, Writers = logConfigSetting.writers };
                    logDic.Add(logConfigSetting.loggerName.ToLower(), logger);
                }
                if (logConfigSetting.loggers != null)
                {
                    foreach (LogConfigSetting logSetting in logConfigSetting.loggers)
                    {
                        Logger logger = new Logger { LogLevel = logSetting.logLevel, Writers = logSetting.writers };
                        logDic.Add(logSetting.loggerName.ToLower(), logger);
                    }
                }
            }
        }

        /// <summary>
        /// 获取一个Logger
        /// </summary>
        /// <param name="logName"></param>
        /// <returns></returns>
        public static Logger GetLogger(string logName)
        {
            try
            {
                if (logDic.ContainsKey(logName.ToLower()))
                {
                    return logDic[logName.ToLower()];
                }
                else
                {
                    return logDic[SystemLoggerName];
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
