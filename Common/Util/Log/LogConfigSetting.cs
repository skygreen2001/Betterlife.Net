using System;
using System.Collections.Generic;
using System.Configuration;

namespace Util.Log
{
    /// <summary>
    /// 日志的配置实体类
    /// </summary>
    public class LogConfigSetting : ConfigurationSection
    {
        /// <summary>
        /// 日志路径
        /// </summary>
        [ConfigurationProperty("logPath")]
        public string logPath
        {
            get { return (string)this["logPath"]; }
            set { this["logPath"] = value; }
        }


        /// <summary>
        /// 日志文件名
        /// </summary>
        [ConfigurationProperty("fileName")]
        public string fileName
        {
            get { return (string)this["fileName"]; }
            set { this["fileName"] = value; }
        }
        
        /// <summary>
        /// 日志名
        /// </summary>
        [ConfigurationProperty("loggerName")]
        public string loggerName
        {
            get { return (string)this["loggerName"]; }
            set { this["loggerName"] = value; }
        }
        
        /// <summary>
        /// 日志等级
        /// </summary>
        [ConfigurationProperty("logLevel")]
        public LogLevel logLevel
        {
            get { return (LogLevel)this["logLevel"]; }
            set { this["logLevel"] = value; }
        }

        private List<String> _writerNames = new List<string>();
        /// <summary>
        /// 记录器名
        /// </summary>
        public List<String> writerNames
        {
            get
            {
                return _writerNames;
            }
            set
            {
                _writerNames = value;
            }
        }

        private List<IWriter> _writers = new List<IWriter>();
        /// <summary>
        /// 记录器
        /// </summary>
        public List<IWriter> writers
        {
            get
            {
                return _writers;
            }
            set
            {
                _writers = value;
            }
        }

        private List<LogConfigSetting> _loggers;
        /// <summary>
        /// 日志器
        /// </summary>
        public List<LogConfigSetting> loggers
        {
            get
            {
                return _loggers;
            }
            set
            {
                _loggers = value;
            }
        }
    }
}
