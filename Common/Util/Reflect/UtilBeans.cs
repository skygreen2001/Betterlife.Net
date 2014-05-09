using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Util.Common;

namespace Util.Reflection
{
    //// <summary>
    /// UtilBeans提供用于拷贝对象中的具体字段的值的方法
    /// </summary>
    public static class UtilBeans
    {
        /// <summary>
        /// 把源对象里的各个字段的内容直接赋值给目标对象（只是字段复制，两个对象的字段名和类型都必须一致）
        /// </summary>
        /// <param name="dest">目标对象</param>
        /// <param name="src">源对象</param>
        public static void CopyObject(object dest, object src)
        {
            bool isRunWell = true;
            if (null == src)
            {
                isRunWell = false;
                Console.WriteLine("源对象不能为空！");
            }
            if (null == dest) {
                isRunWell = false;
                Console.WriteLine("目的对象不能为空！");
            }
            if (isRunWell)
            {

                Type srcType = src.GetType();
                Type destType = dest.GetType();

                FieldInfo[] srcInfo = srcType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                FieldInfo[] destInfo = destType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                for (int i = 0; i < srcInfo.Length; i++)
                {
                    string name = srcInfo[i].Name;
                    object val = srcInfo[i].GetValue(src);

                    for (int j = 0; j < destInfo.Length; j++)
                    {
                        string targetName = destInfo[j].Name;

                        if (name.Equals(targetName))
                        {
                            //类型相同的才能进行设置

                            if (destInfo[j].FieldType == srcInfo[i].FieldType)
                            {
                                destInfo[j].SetValue(dest, val);
                            }
                            else
                            {
                                if (destInfo[j].FieldType == typeof(int) && srcInfo[i].FieldType == typeof(string))
                                {
                                    destInfo[j].SetValue(dest, int.Parse((string)val));
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 从一个对象里复制属性到另一个对象的同名属性
        /// </summary>
        /// <param name="dest">目标对象</param>
        /// <param name="src">源对象</param>
        /// <param name="fieldName">要复制的属性名字</param>
        public static void CopyProperty(object dest, object src, string fieldName)
        {
            if (null == src || null == dest || null == fieldName)
            {
                Console.WriteLine("操作的对象参数不能为空！");
            }
            else
            {
                Type srcType = src.GetType();
                Type destType = dest.GetType();
                FieldInfo srcInfo = srcType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                FieldInfo destInfo = destType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                object val = srcInfo.GetValue(src);
                destInfo.SetValue(dest, val);
            }
        }

        /// <summary>     
        /// Perform a deep Copy of the object.     
        /// </summary>     
        /// <typeparam name="T">The type of object being copied.</typeparam>     
        /// <param name="source">The object instance to copy.</param>     
        /// <returns>The copied object.</returns>     
        public static T Clone<T>(T source)     {         
            if (!typeof(T).IsSerializable)         {
                Console.WriteLine("Type必须serializable." + source);
                return default(T);
            }          
            // Don't serialize a null object, simply return the default for that object 
            if (Object.ReferenceEquals(source, null))         {            
                return default(T);         
            }          
            IFormatter formatter = new BinaryFormatter();         
            Stream stream = new MemoryStream();         
            using (stream)         {            
                formatter.Serialize(stream, source);            
                stream.Seek(0, SeekOrigin.Begin);             
                return (T)formatter.Deserialize(stream);        
            }     
        } 
        
    }
    

}
