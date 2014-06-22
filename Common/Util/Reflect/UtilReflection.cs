using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Util.Common;

namespace Util.Reflection
{
    /// <summary>
    /// 类反射注入工具类
    /// </summary>
    public static class UtilReflection
    {
        /// <summary>
        /// 是否Debug
        /// </summary>
        public static bool IsDebug;

        public readonly static BindingFlags BindingFlags =
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.FlattenHierarchy |
            BindingFlags.CreateInstance;

        #region 静态变量
        /// <summary>
        /// 获取指定类所有的全局静态变量
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public static List<string> GetAllStaticFieldsByPrefix(Type classType, string prefix)
        {
            List<string> result = null;
            FieldInfo[] fields = classType.GetFields(BindingFlags.Public | BindingFlags.Static);
            if (fields != null && fields.Length > 0)
            {
                result = new List<string>();
                foreach (FieldInfo field in fields)
                {
                    if (field.Name.Contains(prefix))
                    {
                        result.Add(field.Name);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取指定类所有的全局静态变量
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public static List<string> GetAllStaticPropertiesByPrefix(Type classType, string prefix)
        {
            List<string> result = null;
            PropertyInfo[] properties = classType.GetProperties(BindingFlags.Public | BindingFlags.Static);
            if (properties != null && properties.Length > 0)
            {
                result = new List<string>();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.Contains(prefix))
                    {
                        result.Add(property.Name);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取指定类的静态Field的值
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public static object GetPublicStaticFieldValue(Type classType, string fieldname)
        {
            FieldInfo fieldInfo = classType.GetField(fieldname, BindingFlags.Public | BindingFlags.Static);
            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(null);
            }
            return null;
        }

        /// <summary>
        /// 获取指定类的静态Field替换了前缀的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <param name="oldPrefix"></param>
        /// <param name="newPrefix"></param>
        /// <returns></returns>
        public static object GetPublicStaticFieldValue(Type type, string fieldName, string oldPrefix, string newPrefix)
        {
                string viewfield_name = fieldName.Replace(oldPrefix, "");
                string result = (string) UtilReflection.GetPublicStaticFieldValue(type, newPrefix + viewfield_name);
                return result;
        }
        
        /// <summary>
        /// 获取指定类的静态属性的值
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public static object GetPublicStaticPropertyValue(Type classType, string propertyname)
        {
            PropertyInfo propertyInfo = classType.GetProperty(propertyname, BindingFlags.Public | BindingFlags.Static);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(classType,null);
            }
            return null;
        }

        /// <summary>
        /// 由指定的前缀在类里找到等于指定值的静态变量替换了前缀
        /// 并得到了指定了新的前缀的类的静态变量的值。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="oldPrefix"></param>
        /// <param name="newPrefix"></param>
        /// <returns></returns>
        public static string GetPublicStaticFieldValueByValue(Type type, string assignValue, string oldPrefix, string newPrefix)
        {  
            List<string> fields = UtilReflection.GetAllStaticFieldsByPrefix(type, oldPrefix);
            if (fields != null && fields.Count > 0)
            {
                string currentTypeNameValue = "";
                foreach (string fieldname in fields)
                {
                    currentTypeNameValue = UtilReflection.GetPublicStaticFieldValue(type, fieldname).ToString();
                    if (currentTypeNameValue == assignValue)
                    {
                        string result = (string)UtilReflection.GetPublicStaticFieldValue(type, fieldname, oldPrefix, newPrefix);
                        if (result == null) result = "";
                        return result;
                    }
                }
            }
            return "";
        }
        
        /// <summary>
        /// 将指定类的静态全局变量设置值
        /// </summary>
        /// <param name="classFullName">类的完整名称如：callcenter.GlobalConfig</param>
        /// <param name="data">指定Key-Value值</param>
        public static void SetPublicStaticProperties(Type classType, Hashtable data)
        {            
            PropertyInfo propertyInfo;

            foreach (DictionaryEntry property in data)
            {
                propertyInfo = classType.GetProperty(property.Key.ToString(), BindingFlags.Public | BindingFlags.Static);
                if (propertyInfo != null)
                {
                    if (propertyInfo.PropertyType.Equals(typeof(Boolean)))
                    {
                        bool value = property.Value.ToString().ToLower() == "true" ? true : false;
                        propertyInfo.SetValue(property.Key, value, null);
                    }
                    else if (propertyInfo.PropertyType.Equals(typeof(Int32)))
                    {
                        if (UtilNumber.IsDigit(property.Value.ToString()))
                        {
                            int value = int.Parse(property.Value.ToString());
                            propertyInfo.SetValue(property.Key, value, null);
                        }
                    }
                    else
                    {
                        propertyInfo.SetValue(property.Key, property.Value, null);
                    }
                }

            }
        }
        #endregion


        /// <summary>
        /// 根据类路径获取Type
        /// </summary>
        /// <param name="classFullName"></param>
        /// <returns></returns>
        public static Type GetTypeByClassFullName(string classFullName)
        {
            //获取当前运行的程序集
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.GetType(classFullName);
        }

        /// <summary>
        /// 用于设置对象的属性值
        /// </summary>
        /// <param name="dest">目标对象</param>
        /// <param name="fieldName">属性名字</param>
        /// <param name="value">属性里要设置的值</param>
        public static void SetField(ref object dest, string fieldName, object value)
        {
            if (null == dest || null == fieldName || null == value)
            {
                throw new ArgumentNullException("one argument is null!");
            }
            Type t = dest.GetType();
            FieldInfo f = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            f.SetValue(dest, value);
        }

        /// <summary>
        /// 设置相应属性的值
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="fieldName">属性名</param>
        /// <param name="fieldValue">属性值</param>
        public static void SetValue(object entity, string fieldName, string fieldValue)
        {
            Type entityType = entity.GetType();

            PropertyInfo propertyInfo = entityType.GetProperty(fieldName);
            if (propertyInfo == null)
            {
                return;
            }
            if (IsType(propertyInfo.PropertyType, "System.String"))
            {
                propertyInfo.SetValue(entity, fieldValue, null);
                return;

            }

            if (IsType(propertyInfo.PropertyType, "System.Boolean"))
            {
                propertyInfo.SetValue(entity, Boolean.Parse(fieldValue), null);
                return;
            }

            if (IsType(propertyInfo.PropertyType, "System.Int32"))
            {
                if (fieldValue != "")
                    propertyInfo.SetValue(entity, int.Parse(fieldValue), null);
                else
                    propertyInfo.SetValue(entity, 0, null);
                return;
            }

            if (IsType(propertyInfo.PropertyType, "System.Nullable`1[System.Decimal]"))
            {
                if (fieldValue != "")
                    propertyInfo.SetValue(entity, Decimal.Parse(fieldValue), null);
                else
                    propertyInfo.SetValue(entity, new Decimal(0), null);
                return;
            }

            if (IsType(propertyInfo.PropertyType, "System.Nullable`1[System.DateTime]"))
            {
                if (fieldValue != "")
                {
                    try
                    {
                        string fStyle = "yyyy-MM-dd HH:mm:ss";
                        if (UtilString.Substr_Count(fieldValue, "-") == 2)
                        {
                            string[] fv = fieldValue.Split('-');
                            if (UtilString.Substr_Count(fieldValue, ":") == 2)
                            {
                                if (fv[1].Length == 1) fStyle = "yyyy-M-dd HH:mm:ss";
                                propertyInfo.SetValue(entity, (DateTime?)DateTime.ParseExact(fieldValue, fStyle, null), null);
                            }
                            else
                            {
                                fStyle = "yyyy-MM-dd";
                                if (fv[1].Length == 1) fStyle = "yyyy-M-dd";
                                propertyInfo.SetValue(entity, (DateTime?)DateTime.ParseExact(fieldValue, fStyle, null), null);
                            }
                        }
                        else if (UtilString.Substr_Count(fieldValue, "/") == 2)
                        {
                            fStyle = "yyyy/MM/dd HH:mm:ss";
                            string[] fv = fieldValue.Split('/');
                            if (UtilString.Substr_Count(fieldValue, ":") == 2)
                            {
                                if (fv[1].Length == 1) fStyle = "yyyy/M/dd HH:mm:ss";
                                propertyInfo.SetValue(entity, (DateTime?)DateTime.ParseExact(fieldValue, fStyle, null), null);
                            }
                            else
                            {
                                fStyle = "yyyy/MM/dd";
                                if (fv[1].Length == 1) fStyle = "yyyy/M/dd";
                                propertyInfo.SetValue(entity, (DateTime?)DateTime.ParseExact(fieldValue, fStyle, null), null);
                            }


                        }else{
                            if (UtilString.Substr_Count(fieldValue, ":") == 2)
                            {
                                propertyInfo.SetValue(entity, (DateTime?)DateTime.ParseExact(fieldValue, "HH:mm:ss", null), null);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                    propertyInfo.SetValue(entity, null, null);
                return;
            }

        }

        /// <summary>
        /// 类型匹配
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool IsType(Type type, string typeName)
        {
            if (type == null) return false;
            if (type.ToString() == typeName)
                return true;
            if (type.ToString() == "System.Object")
                return false;

            return IsType(type.BaseType, typeName);
        }

        //// <summary>
        /// 反射打印出对象有的方法以及调用参数
        /// </summary>
        /// <param name="obj">传入的对象</param>
        public static void PrintMethod(object obj)
        {
            Type t = obj.GetType();
            MethodInfo[] mif = t.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < mif.Length; i++)
            {
                Console.WriteLine("方法名字：" + mif[i].Name);
                Console.WriteLine("方法名字：" + mif[i].Module.Name);
                ParameterInfo[] p = mif[i].GetParameters();
                for (int j = 0; j < p.Length; j++)
                {
                    Console.WriteLine("参数名:  " + p[j].Name);
                    Console.WriteLine("参数类型:  " + p[j].ParameterType.ToString());
                }
                Console.WriteLine("******************************************");
            }
        }

        /// <summary>
        /// 获取对象指定属性值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object ReflectGetProperty(this object target, string propertyName)
        {
            if (target == null)
            {
                if (IsDebug)
                {
                    throw new ArgumentNullException("target");
                }
                Console.WriteLine("目标对象不能为空");
            }
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                if (IsDebug)
                {
                    throw new ArgumentException("propertyName can not be null or whitespace", "propertyName");
                }
                Console.WriteLine("属性不能为空不能为空");
            }
            var propertyInfo = target.GetType().GetProperty(propertyName, BindingFlags);
            if (propertyInfo == null)
            {
                if (IsDebug)
                {
                    throw new ArgumentException(string.Format("Can not find property '{0}' on '{1}'", propertyName, target.GetType()));
                }
                Console.WriteLine(String.Format("在类：{0}里不能找到指定属性：{1}", target.GetType(), propertyName));                
            }
            return propertyInfo.GetValue(target, null);
        }

        /// <summary>
        /// 获取对象的所有属性名
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static List<string> GetPropertNames(this object target)
        {
            PropertyInfo[] pis=target.GetType().GetProperties();
            List<string> result = new List<string>();
            foreach (PropertyInfo pi in pis)
            {
                result.Add(pi.Name);
            }
            return result;
        }

        /// <summary>
        ///  触发运行对象指定方法值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        /// <param name="argTypes"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object ReflectInvokeMethod(this object target, string methodName, Type[] argTypes, object[] parameters)
        {
            if (target == null)
            {
                if (IsDebug)
                {
                    throw new ArgumentNullException("target");
                }
                Console.WriteLine("目标对象不能为空");
            }
            if (string.IsNullOrWhiteSpace(methodName))
            {
                if (IsDebug)
                {
                    throw new ArgumentException("methodName can not be null or whitespace", "methodName");
                }
                Console.WriteLine("方法名不能为空");
            }

            var methodInfo = target.GetType().GetMethod(methodName, BindingFlags, null, argTypes, null);
            if (methodInfo == null)
            {
                if (IsDebug)
                {
                    throw new ArgumentException(string.Format("Can not find method '{0}' on '{1}'", methodName, target.GetType()));
                }

                Console.WriteLine(String.Format("在类：{0}里不能找到指定方法：{1}", target.GetType(), methodName));    
            }
            return methodInfo.Invoke(target, parameters);
        }

        /// <summary>
        /// 打印对象信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String print_r(Object obj)
        {
            String result = "";
            if (obj != null)
            {
                result = obj.GetType().FullName + ":\r\n";
                PropertyInfo[] ps = obj.GetType().GetProperties();
                foreach (PropertyInfo property in ps)
                {
                    result += property.Name + ":" + property.GetValue(obj) + "\r\n";
                }
            }
            Console.WriteLine(result);
            return result;
        }
    }
}
