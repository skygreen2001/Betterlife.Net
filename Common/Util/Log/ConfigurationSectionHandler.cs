using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Util.Common;
using Util.Log.Writer;

namespace Util.Log
{
    /// <summary>
    /// 读取日志的配置信息类
    /// </summary>
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {
        #region 定义部分
        private static string defaultSectionName = "logSetting";
        /// <summary>
        /// 日志路径信息
        /// </summary>
        public static string LogPathInfo;
        /// <summary>
        /// 实际定义的Writer
        /// </summary>
        private Dictionary<String, IWriter> defineWriterDic = new Dictionary<String, IWriter>();

        private LogConfigSetting defaultLogger = new LogConfigSetting();

        private List<LogConfigSetting> defChildLoggers = new List<LogConfigSetting>();
        #endregion 定义部分

        public static LogConfigSetting[] GetSection(string sectionName)
        {
            return (LogConfigSetting[])ConfigurationManager.GetSection(sectionName);
        }

        public static LogConfigSetting[] GetSection()
        {
            Configuration config = UtilSystem.getCurrentConfig();
            ConfigurationSectionGroup oSectionGroup = config.SectionGroups[defaultSectionName];
            
            if ((oSectionGroup != null) && (oSectionGroup.Sections.Count > 0))
            {
                LogConfigSetting[] logConfigSettings = new LogConfigSetting[oSectionGroup.Sections.Count];
                LogConfigSetting logConfigSetting = null;
                for (int i = 0; i < oSectionGroup.Sections.Count; i++)
                {
                    logConfigSetting = (LogConfigSetting)config.GetSection(defaultSectionName + "/" + oSectionGroup.Sections.Keys.Get(i));
                    try
                    {
                        List<IWriter> writers = new List<IWriter>();
                        TextWriter textWriter = new TextWriter();
                        textWriter.FileName = logConfigSetting.fileName;
                        textWriter.LogPath = ConfigurationSectionHandler.LogPathInfo + logConfigSetting.logPath;                        
                        writers.Add(textWriter);
                        logConfigSetting.writers = writers;
                    }
                    catch
                    {
                        //出现异常，填充默认的日志
                        logConfigSetting.loggerName = "";
                        logConfigSetting.logLevel = LogLevel.All;
                        List<IWriter> writers = new List<IWriter>();
                        TextWriter textWriter = new TextWriter();
                        textWriter.FileName = "xiben.log";
                        textWriter.LogPath = @"C:\xiben\Temp";
                        writers.Add(textWriter);

                        logConfigSetting.writers = writers;
                    }
                    logConfigSettings[i] = logConfigSetting;
                }
                return logConfigSettings;
            }
            return null;
        }

        public object Create(object parent, object configContext, XmlNode section)
        {
            //解析配置文件
            foreach (XmlNode oneLevelNode in section.ChildNodes)
            {
                if (oneLevelNode.NodeType == XmlNodeType.Element)
                {
                    string oneLevelNodeName = oneLevelNode.Name.ToLower();
                    switch (oneLevelNodeName)
                    {
                        case "root":
                            ParseRoot((XmlElement)oneLevelNode);
                            break;
                        case "logger":
                            ParseLogger((XmlElement)oneLevelNode);
                            break;
                        case "writer":
                            ParseWriter((XmlElement)oneLevelNode);
                            break;
                    }
                }
            }
            defaultLogger.loggers = defChildLoggers;
            MatchLoggerWriter();
            return defaultLogger;
        }

        private void MatchLoggerWriter()
        {
            //首先匹配默认配置
            foreach (string writerName in defaultLogger.writerNames)
            {
                IWriter writer = defineWriterDic[writerName];
                defaultLogger.writers.Add(writer);
            }
            if (defaultLogger.loggers != null)
            {
                //在匹配子Logger
                foreach (LogConfigSetting logger in defaultLogger.loggers)
                {
                    foreach (string writerName in logger.writerNames)
                    {
                        IWriter writer = defineWriterDic[writerName];
                        logger.writers.Add(writer);
                    }
                    //检查root中默认的配置是否在子项中，如果不存在则添加
                    foreach (string defaultWriterName in defaultLogger.writerNames)
                    {
                        if (!logger.writerNames.Contains(defaultWriterName))
                        {
                            IWriter writer = defineWriterDic[defaultWriterName];
                            logger.writers.Add(writer);
                        }
                    }
                }
            }
        }

        int i = 0;
        /// <summary>
        /// 解析日志
        /// </summary>
        /// <param name="curElement"></param>
        private void ParseLogger(XmlElement curElement)
        {
            i++;
            LogConfigSetting logConfigSetting = new LogConfigSetting();
            string loggerName = curElement.Attributes["name"].Value.ToLower();
            if (String.IsNullOrEmpty(loggerName))
            {
                loggerName = "defaultLogger" + i;
            }
            logConfigSetting.loggerName = loggerName;

            foreach (XmlNode twoLevelNode in curElement.ChildNodes)
            {
                if (twoLevelNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement element = twoLevelNode as XmlElement;
                    switch (element.Name.ToLower())
                    {
                        case "level":
                            string level = element.GetAttribute("value").ToLower();
                            switch (level)
                            {
                                case "all":
                                    logConfigSetting.logLevel = LogLevel.All;
                                    break;
                                case "info":
                                    logConfigSetting.logLevel = LogLevel.Info;
                                    break;
                                case "warn":
                                    logConfigSetting.logLevel = LogLevel.Warn;
                                    break;
                                case "error":
                                    logConfigSetting.logLevel = LogLevel.Error;
                                    break;
                                default:
                                    logConfigSetting.logLevel = LogLevel.All;
                                    break;
                            }
                            break;
                        case "writer":
                            logConfigSetting.writerNames.Add(element.GetAttribute("ref").ToLower());
                            break;
                    }
                }
            }
            defChildLoggers.Add(logConfigSetting);
        }

        /// <summary>
        /// 解析配置根
        /// </summary>
        /// <param name="curElement"></param>
        private void ParseRoot(XmlElement curElement)
        {
            defaultLogger.loggerName = LogManager.SystemLoggerName;

            foreach (XmlNode twoLevelNode in curElement.ChildNodes)
            {
                if (twoLevelNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement element = twoLevelNode as XmlElement;
                    switch (element.Name.ToLower())
                    {
                        case "level":
                            string level = element.GetAttribute("value").ToLower();
                            switch (level)
                            {
                                case "all":
                                    defaultLogger.logLevel = LogLevel.All;
                                    break;
                                case "info":
                                    defaultLogger.logLevel = LogLevel.Info;
                                    break;
                                case "warn":
                                    defaultLogger.logLevel = LogLevel.Warn;
                                    break;
                                case "error":
                                    defaultLogger.logLevel = LogLevel.Error;
                                    break;
                                default:
                                    defaultLogger.logLevel = LogLevel.All;
                                    break;
                            }
                            break;
                        case "writer":
                            defaultLogger.writerNames.Add(element.GetAttribute("ref").ToLower());
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 解析具体的Writer
        /// </summary>
        /// <param name="writerElement"></param>
        private void ParseWriter(XmlElement writerElement)
        {
            string writerName = writerElement.Attributes["name"].Value.ToLower();
            //对于没有在root中定义的writer丢弃
            if (!CheckExistWriterName(writerName))
            {
                return;
            }
            string writerType = writerElement.Attributes["type"].Value;

            IWriter writer = (IWriter)Activator.CreateInstance(GetTypeFromString(writerType, true, true));

            #region "设置Writer"
            foreach (XmlNode currentNode in writerElement.ChildNodes)
            {
                if (currentNode.NodeType == XmlNodeType.Element)
                {
                    string propName = currentNode.Name;
                    Type targetType = writer.GetType();
                    Type propertyType = null;
                    PropertyInfo propInfo = null;
                    MethodInfo methInfo = null;

                    propInfo = targetType.GetProperty(propName, BindingFlags.Instance |
                                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

                    if (propInfo != null && propInfo.CanWrite)
                    {
                        // found a property
                        propertyType = propInfo.PropertyType;
                    }
                    else
                    {
                        propInfo = null;
                        // look for a method with the signature Add<property>(type)
                        methInfo = FindMethodInfo(targetType, propName);

                        if (methInfo != null)
                        {
                            propertyType = methInfo.GetParameters()[0].ParameterType;
                        }
                    }


                    if (propertyType != null)
                    {
                        string propertyValue = String.Empty;
                        propertyValue = currentNode.Attributes["value"].Value;
                        object convertedValue = null;

                        convertedValue = ConvertStringTo(propertyType, propertyValue);

                        if (propertyValue != null)
                        {
                            propInfo.SetValue(writer, convertedValue, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
            #endregion

            if (writer != null)
            {
                defineWriterDic.Add(writerName, writer);
            }
        }

        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="writerName"></param>
        /// <returns></returns>
        private bool CheckExistWriterName(string writerName)
        {
            if (defaultLogger.writerNames.Contains(writerName))
            {
                return true;
            }
            foreach (LogConfigSetting logger in defChildLoggers)
            {
                if (logger.writerNames.Contains(writerName))
                {
                    return true;
                }
            }
            return false;
        }

        #region "Parse"

        private static Type GetTypeFromString(string typeName, bool throwOnError, bool ignoreCase)
        {
            return GetTypeFromString(Assembly.GetCallingAssembly(), typeName, throwOnError, ignoreCase);
        }

        private static Type GetTypeFromString(Assembly relativeAssembly, string typeName, bool throwOnError, bool ignoreCase)
        {
            // Check if the type name specifies the assembly name
            if (typeName.IndexOf(',') == -1)
            {
                // Attempt to lookup the type from the relativeAssembly
                Type type = relativeAssembly.GetType(typeName, false, ignoreCase);
                if (type != null)
                {
                    // Found type in relative assembly
                    //LogLog.Debug("SystemInfo: Loaded type ["+typeName+"] from assembly ["+relativeAssembly.FullName+"]");
                    return type;
                }

                Assembly[] loadedAssemblies = null;
                try
                {
                    loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                }
                catch (System.Security.SecurityException)
                {
                    // Insufficient permissions to get the list of loaded assemblies
                }

                if (loadedAssemblies != null)
                {
                    // Search the loaded assemblies for the type
                    foreach (Assembly assembly in loadedAssemblies)
                    {
                        type = assembly.GetType(typeName, false, ignoreCase);
                        if (type != null)
                        {
                            return type;
                        }
                    }
                }
                // Didn't find the type
                if (throwOnError)
                {
                    throw new TypeLoadException("Could not load type [" + typeName + "]. Tried assembly [" + relativeAssembly.FullName + "] and all loaded assemblies");
                }
                return null;
            }
            else
            {
                return Type.GetType(typeName, throwOnError, ignoreCase);

            }
        }

        private MethodInfo FindMethodInfo(Type targetType, string name)
        {
            string requiredMethodNameA = name;
            string requiredMethodNameB = "Add" + name;

            MethodInfo[] methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (MethodInfo methInfo in methods)
            {
                if (!methInfo.IsStatic)
                {
                    if (string.Compare(methInfo.Name, requiredMethodNameA, true, System.Globalization.CultureInfo.InvariantCulture) == 0 ||
                        string.Compare(methInfo.Name, requiredMethodNameB, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
                    {
                        // Found matching method name

                        // Look for version with one arg only
                        System.Reflection.ParameterInfo[] methParams = methInfo.GetParameters();
                        if (methParams.Length == 1)
                        {
                            return methInfo;
                        }
                    }
                }
            }
            return null;
        }

        private static object ConvertStringTo(Type target, string value)
        {
            if (typeof(string) == target || typeof(object) == target)
            {
                return value;
            }

            MethodInfo meth = target.GetMethod("Parse", new Type[] { typeof(string) });
            if (meth != null)
            {
                // Call the Parse method
                return meth.Invoke(null, BindingFlags.InvokeMethod, null,
                        new object[] { value }, CultureInfo.InvariantCulture);
            }
            return value;
        }

        #endregion
    }
}
