using Database;
using Ext.Direct;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using Util.Reflection;

namespace Admin.Services
{
    /// <summary>
    /// BUILD ASP.NET WEB APPS FASTE:http://www.ext.net/
    /// </summary>
    /// <see cref="https://github.com/evantrimboli"/>
    public class ExtServiceBasic : DirectHandler, IRequiresSessionState
    {
        protected static BetterlifeNetEntities db = new BetterlifeNetEntities();

        public int PageCount=15;
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
        /// 基本的保存动作
        /// </summary>
        /// <param name="enti"></param>
        protected void copyProperties(object entityObject, HttpRequest condition)
        {
            NameValueCollection conForm = condition.Form;
            String[] keys = conForm.AllKeys;
            LinkedList<string> keysList = new LinkedList<string>(keys);
            this.clearValuelessData(keysList);
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
        /// 去除Ext通用的变量
        /// 
        /// </summary>
        protected void clearValuelessData(LinkedList<string> keysList)
        {
            keysList.Remove("extAction");
            keysList.Remove("extMethod");
            keysList.Remove("extTID");
            keysList.Remove("extType");
            keysList.Remove("extUpload");
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

        /// <summary>
        /// 门店ID
        /// </summary>
        protected Guid Store_ID
        {
            get
            {
                HttpContext context = HttpContext.Current;
                string store_ID = "";
                try
                {
                    store_ID = context.Session["Store_ID"].ToString();
                }
                catch 
                {

                    HttpCookie getCookie = context.Request.Cookies["JjShop_Store_ID"];
                    store_ID = getCookie.Value;
                }
                return new Guid(store_ID);
            }
        }

        /// <summary>
        /// 管理员权限
        /// </summary>
        protected int Roletype
        {
            get
            {
                HttpContext context = HttpContext.Current;
                int roletype = 1;
                try
                {
                    roletype = Convert.ToByte(context.Session["Roletype"]);
                }
                catch 
                {
                    HttpCookie getCookie = context.Request.Cookies["JjShop_Roletype"];
                    roletype = Convert.ToByte(getCookie.Value);
                }
                return roletype;
            }
        }
    }
}