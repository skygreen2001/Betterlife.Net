using System;
using System.IO;
using System.Text;
using Util.Common;

namespace Util.Log.Writer
{
    /// <summary>
    /// 记录文本日志
    /// </summary>
    public class TextWriter : IWriter
    {
        //文件流
        private StreamWriter writer = null;
        private string oldFileName = "";
        /// <summary>
        /// 日志文件完整的路径文件名
        /// </summary>
        public string strLogFullPathName;
        /// <summary>
        /// 锁，防止多线程破坏文件
        /// </summary>
        private static object syncRoot = new object();

        private string _logPath;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string LogPath
        {
            get
            {
                return _logPath;
            }
            set
            {
                _logPath = value;
            }
        }

        private string _fileName;
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }

        private DateTime lastFlushTime = DateTime.Now;
        /// <summary>
        /// 记录日志到文本中，每天记录一个日志
        /// </summary>
        /// <param name="message"></param>
        public void Write(string message)
        {
            try
            {
                string newFileName = ChangeFileName();
                if (newFileName != oldFileName)
                {
                    lock (syncRoot)
                    {
                        if (newFileName != oldFileName)
                        {
                            oldFileName = newFileName;
                            OpenFileStream(newFileName);
                        }
                    }
                }
                if (writer == null)
                {
                    OpenFileStream(newFileName);
                }
                writer.WriteLine(message);

                TimeSpan s = DateTime.Now.Subtract(lastFlushTime);
                if (s.TotalSeconds >= 3)
                {
                    lastFlushTime = DateTime.Now;
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                string str = ex.ToString();
            }
        }

        private void OpenFileStream(string newFileName)
        {
            if (writer != null)
            {
                writer.Flush();
            }
            string strFielFullPath = Path.Combine(LogPath, newFileName);
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            writer = new StreamWriter(strFielFullPath, true, Encoding.UTF8);
            this.strLogFullPathName = strFielFullPath;
        }

        /// <summary>
        /// 修改文件名，前面附带日期信息
        /// </summary>
        /// <returns></returns>
        private string ChangeFileName()
        {

            string curDate = DateTime.Now.ToString(UtilDateTime.FORMAT_YYYYMMDD);
            return curDate + "-" + FileName;
        }

        public void Dispose()
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Dispose();
            }
        }

    }
}
