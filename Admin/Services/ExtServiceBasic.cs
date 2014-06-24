﻿using Database;
using Ext.Direct;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using Util.Reflection;

namespace AdminManage.Services
{
    /// <summary>
    /// BUILD ASP.NET WEB APPS FASTE:http://www.ext.net/
    /// </summary>
    /// <see cref="https://github.com/evantrimboli"/>
    public class ExtServiceBasic : DirectHandler, IRequiresSessionState
    {
        protected static BetterlifeNetEntities db = DatabaseCenter.Instance();

        /// <summary>
        /// 返回Ext Direct Api的ProviderName
        /// </summary>
        public override string ProviderName
        {
            get
            {
                return "Ext.app.REMOTING_API";
            }
        }

        /// <summary>
        /// 返回命名空间
        /// </summary>
        public override string Namespace
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// 基本的复制同名的属性从数组到对象
        /// </summary>
        /// <param name="enti"></param>
        protected void CopyProperties(object entityObject, HttpRequest condition)
        {
            NameValueCollection conForm = condition.Form;
            String[] keys = conForm.AllKeys;
            LinkedList<string> keysList = new LinkedList<string>(keys);
            this.ClearValuelessData(keysList);
            String value;
            PropertyInfo propertyInfo;
            foreach (string key in keysList)
            {
                value = condition[key];
                propertyInfo = entityObject.GetType().GetProperty(key);
                UtilReflection.SetValue(entityObject,key,value);
            }
            propertyInfo = entityObject.GetType().GetProperty("UpdateTime");
            if (propertyInfo!=null)
            {
                propertyInfo.SetValue(entityObject, DateTime.Now, null);
            }

        }

        /// <summary>
        /// 将过滤条件转换成需查询的模糊条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        /// <returns></returns>
        protected string FiltertoCondition(Dictionary<String, object> condition)
        {
            string result="";
            List<string> conditionL=new List<string>();
            foreach (KeyValuePair<String,object> entry in condition)
            {
                Console.WriteLine(entry.Key + entry.Value);
                string value = entry.Value.ToString();
                if ((value != null) && (!string.IsNullOrEmpty(value)))
                {
                    conditionL.Add(entry.Key+" LIKE '%"+value+"%' "); 
                }
            }
            if (condition.Count>0)result=string.Join(" AND ",conditionL.ToArray());
            return result;
        }

        /// <summary>
        /// 去除Ext通用的变量
        /// 
        /// </summary>
        protected void ClearValuelessData(LinkedList<string> keysList)
        {
            keysList.Remove("extAction");
            keysList.Remove("extMethod");
            keysList.Remove("extTID");
            keysList.Remove("extType");
            keysList.Remove("extUpload");
        }

        /// <summary>
        /// 清除数据对象关联的数据对象,一般在获取到所需数据之后最后执行
        /// </summary>
        protected object ClearInclude(object entityObject)
        {
            object destObject = Activator.CreateInstance(entityObject.GetType().BaseType);
            List<string> keysList = UtilReflection.GetPropertNames(entityObject);
            PropertyInfo p, p_n;
            foreach (string key in keysList)
            {
                p = entityObject.GetType().GetProperty(key);
                p_n = destObject.GetType().GetProperty(key);
                if (p_n != null)
                {
                    if (p_n.PropertyType.FullName.Contains("Database."))
                    {
                        p_n.SetValue(destObject, null);
                    }
                    else
                    {
                        object origin_pro = p.GetValue(entityObject);
                        if (origin_pro != null) UtilReflection.SetValue(destObject, key, origin_pro.ToString());
                    }
                }
            }
            return destObject;
        }

        [JsonProperty(PropertyName = "totalCount")]
        public int TotalCount
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "success")]
        public bool Success
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "data")]
        public List<Object> Stores
        {
            get;
            set;
        }

    }
}