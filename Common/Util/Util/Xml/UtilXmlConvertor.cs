#region Refrences
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Util.Common;
#endregion

namespace Util.xml
{
    /// <summary>
    /// 将XML格式数据直接转换成应用系统中的对象工具类
    /// this class define fuction for xml serialization and deserialization.
    /// It can be used in the client object conversion.
    /// </summary>
    public static class UtilXmlConvertor
    {
        private const string CONVERT_EXCEPTION_MSG = "Can not convert XML to Object. ";

        /// <summary>
        /// serialize an object to string.
        /// </summary>
        /// <param name="obj">the object.</param>
        /// <returns>the serialized string.</returns>
        public static string ObjectToXml(object obj)
        {
            return UtilXmlConvertor.ObjectToXml(obj, true);
        }

        /// <summary>
        /// serialize an object to string.
        /// </summary>
        /// <param name="obj">the object.</param>
        /// <param name="toBeIndented">whether to be indented.</param>
        /// <returns>the serialized string.</returns>
        public static string ObjectToXml(object obj, bool toBeIndented)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            UTF8Encoding encoding1 = new UTF8Encoding(false);
            XmlSerializer serializer1 = new XmlSerializer(obj.GetType());
            using (MemoryStream stream1 = new MemoryStream())
            {
                XmlTextWriter writer1 = new XmlTextWriter(stream1, encoding1) {
                    Formatting = toBeIndented ? Formatting.Indented : Formatting.None 
                };
                serializer1.Serialize(writer1, obj);
                string text1 = encoding1.GetString(stream1.ToArray());
                writer1.Close();
                return text1;
            }
        }

        /// <summary>
        /// serialize an object to string using binary format.
        /// </summary>
        /// <param name="obj">the object.</param>
        /// <returns>the serialized string.</returns>
        public static string ObjectToXmlBin(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            BinaryFormatter formatter1 = new BinaryFormatter();
            byte[] bytes = null;
            using (MemoryStream stream1 = new MemoryStream())
            {
                formatter1.Serialize(stream1, obj);
                bytes = stream1.ToArray();
            }
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// deserialize string.to an object.
        /// </summary>
        /// <param name="xml">the string need to be deserialized.</param>
        /// <returns>the deserialized object.</returns>
        public static object XmlToObjectBin(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException("xml");
            }
            byte[] bytes = Convert.FromBase64String(xml);

            object obj = null;
            BinaryFormatter formatter1 = new BinaryFormatter();
            using (MemoryStream stream1 = new MemoryStream())
            {
                stream1.Write(bytes, 0, bytes.Length);
                stream1.Position = 0;
                obj = formatter1.Deserialize(stream1);
            }
            return obj;
        }

        /// <summary>
        /// deserialize string.to an object.
        /// </summary>
        /// <param name="xml">the string need to be deserialized.</param>
        /// <param name="type">the type of the object.</param>
        /// <returns>the deserialized object.</returns>
        public static object XmlToObject(string xml, Type type)
        {
            if (xml == null)
            {
                //throw new ArgumentNullException("xml");
                Console.WriteLine(String.Format("无xml信息：{0}\r\n{1}", xml, type.FullName));
                return null;
            }
            if (type == null)
            {
                //throw new ArgumentNullException("type");
                Console.WriteLine(String.Format("无type信息：{0}\r\n{1}", xml, type.FullName));
            }
            object obj1 = null;
            XmlSerializer serializer = new XmlSerializer(type);
            using (StringReader reader = new StringReader(xml))
            {
                XmlReader reader2 = new NamespaceIgnorantXmlTextReader(reader);
                //XmlReader reader2 = new XmlTextReader(reader);
                try
                {
                    obj1 = serializer.Deserialize(reader2);
                }
                catch (InvalidOperationException exception1)
                {
                    Console.WriteLine(CONVERT_EXCEPTION_MSG + exception1.Message);
                }
                finally
                {
                    reader2.Close();
                }
            }
            return obj1;
        }


    }
}
