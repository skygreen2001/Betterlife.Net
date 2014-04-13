using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Util.Log
{
    public static class Helper
    {
        /// <summary>
        /// 格式化消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public static string FormatMessage(string message, LogLevel logLevel)
        {
            string strTmp = "";
            switch (logLevel)
            {
                case LogLevel.Warn:
                    strTmp += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + "Warn" + "\t" + message;
                    break;
                case LogLevel.Info:
                    strTmp += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + "Info" + "\t" + message;
                    break;
                case LogLevel.Error:
                    strTmp += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + "Error" + "\t" + message;
                    break;
                default:
                    strTmp += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + "All" + "\t" + message;
                    break;
            }
            return strTmp;
        }

        /// <summary>
        /// 构建异常字符串
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string BuiltExpString(Exception exp)
        {
            return exp.Message + BuiltStackTraceString(new StackTrace(exp, true));
        }

        /// <summary>
        /// 构建堆栈字符串
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public static string BuiltStackTraceString(StackTrace st)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append("---- Stack Trace ----");
            sb.Append(Environment.NewLine);
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                MemberInfo mi = sf.GetMethod();
                sb.Append(BuiltStackFrameString(sf));
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        /// <summary>
        /// 构建堆栈字符串
        /// </summary>
        /// <param name="sf"></param>
        /// <returns></returns>
        public static string BuiltStackFrameString(StackFrame sf)
        {
            StringBuilder sb = new StringBuilder();
            int intParam;
            MemberInfo mi = sf.GetMethod();
            sb.Append("   ");
            sb.Append(mi.DeclaringType.Namespace);
            sb.Append(".");
            sb.Append(mi.DeclaringType.Name);
            sb.Append(".");
            sb.Append(mi.Name);
            // -- build method params     
            sb.Append("(");
            intParam = 0;
            foreach (ParameterInfo param in sf.GetMethod().GetParameters())
            {
                intParam += 1;
                sb.Append(param.Name);
                sb.Append(" As ");
                sb.Append(param.ParameterType.Name);
            }
            sb.Append(")");
            sb.Append(Environment.NewLine);
            // -- if source code is available, append location info       
            sb.Append("       ");
            if (string.IsNullOrEmpty(sf.GetFileName()))
            {
                sb.Append("(unknown file)");
                //-- native code offset is always available         
                sb.Append(": N ");
                sb.Append(String.Format("{0:#00000}", sf.GetNativeOffset()));
            }
            else
            {
                sb.Append(System.IO.Path.GetFileName(sf.GetFileName()));
                sb.Append(": line ");
                sb.Append(String.Format("{0:#0000}", sf.GetFileLineNumber()));
                sb.Append(", col ");
                sb.Append(String.Format("{0:#00}", sf.GetFileColumnNumber()));
                if (sf.GetILOffset() != StackFrame.OFFSET_UNKNOWN)
                {
                    sb.Append(", IL ");
                    sb.Append(String.Format("{0:#0000}", sf.GetILOffset()));
                }
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
    }
}
