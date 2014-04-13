using System.Collections.Generic;
using Util.Log;
using Util.Log.Writer;
using Util.Reflection;

namespace Database.Domain.System.Log
{
    class Log
    {
        /// <summary>
        /// 最近一次日志的文件路径
        /// </summary>
        /// <returns></returns>
        protected static string RecentLogFileName(string loggerName)
        {
            if (LogManager.GetLogger(loggerName) != null)
            {
                List<IWriter> writter = LogManager.GetLogger(loggerName).Writers;
                if ((writter != null) && (writter.Count > 0))
                {
                    if (writter[0].GetType() == typeof(TextWriter))
                    {
                        TextWriter textWriter = (TextWriter)writter[0];
                        return textWriter.strLogFullPathName;
                        //return System.IO.Path.Combine(textWriter.LogPath, textWriter.FileName);
                    }
                }

            }
            return null;
        }
    }
}
