using System;
using System.Collections.Generic;

using Util.Log.Writer;

namespace Util.Log
{
    /// <summary>
    /// 日志抽象基类
    /// </summary>
    public class Logger : IDisposable
    {

        private LogLevel _logLevel;
        public LogLevel LogLevel
        {
            get
            {
                return _logLevel;
            }
            set
            {
                _logLevel = value;
            }
        }
        /// <summary>
        /// 日志写入器
        /// </summary>
        private List<IWriter> _writers;
        public List<IWriter> Writers
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


        public Logger()
        {

        }

        /// <summary>
        /// 检查记录器是否为null,如果为null添加默认记录器
        /// </summary>
        private void CheckWriter()
        {
            if (_writers == null || _writers.Count == 0)
            {
                TextWriter textWriter = new TextWriter();
                textWriter.FileName = "runlog.txt";
                textWriter.LogPath = @"C:\xiben\temp";
                _writers.Add(textWriter);
            }
        }

        /// <summary>
        /// 记录系统的运行信息
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            if (LogLevel == LogLevel.All || LogLevel == LogLevel.Info)
            {
                foreach (IWriter writer in Writers)
                {
                    writer.Write(message);
                }
            }
        }

        /// <summary>
        /// 告警信息，但系统可以继续的运行
        /// </summary>
        /// <param name="message"></param>
        public void Warn(string message)
        {
            if (LogLevel == LogLevel.All ||
                LogLevel == LogLevel.Warn ||
                LogLevel == LogLevel.Info)
            {
                foreach (IWriter writer in Writers)
                {
                    writer.Write(message);
                }
            }
        }

        /// <summary>
        /// 错误信息，系统有可能不能继续的运行
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            if (LogLevel == LogLevel.All ||
                LogLevel == LogLevel.Info ||
                LogLevel == LogLevel.Warn ||
                LogLevel == LogLevel.Error
            )
            {
                foreach (IWriter writer in Writers)
                {
                    writer.Write(message);
                }
            }
        }

        public void Dispose()
        {
            foreach (IWriter writer in Writers)
            {
                writer.Dispose();
            }
        }
    }
}
