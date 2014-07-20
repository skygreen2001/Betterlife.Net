using Database;
using Ext.Direct;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using Util.Common;
using Util.Reflection;

namespace AdminManage.Services
{
    /// <summary>
    /// BUILD ASP.NET WEB APPS FASTE:http://www.ext.net/
    /// </summary>
    /// <see cref="https://github.com/evantrimboli"/>
    public class ExtServiceBasic : DirectHandler, IRequiresSessionState
    {
        /// <summary>
        /// 当前的数据库数据资源
        /// </summary>
        protected static BetterlifeNetEntities db = DatabaseCenter.Instance();
        /// <summary>
        /// 当前的Session会话
        /// </summary>
        protected static HttpSessionState Session = HttpContext.Current.Session;

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
        /// 初始化工作
        /// 1.初始化获取数据库数据单例
        /// 一般是在服务器变更了数据库，重置DatabaseCenter的db，重新获取一次即可
        /// </summary>
        public static void Init_Db()
        {
            db = DatabaseCenter.Instance();
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
            List<string> conditionL = new List<string>();
            foreach (KeyValuePair<String, object> entry in condition)
            {
                string value = entry.Value.ToString();
                if (entry.Key.Equals("ID") || (entry.Key.ToUpper().Contains("_ID")))
                {
                    conditionL.Add(entry.Key + " = '" + value + "' ");
                }
                else if ((value != null) && (!string.IsNullOrEmpty(value)))
                {
                    conditionL.Add(entry.Key + " LIKE '%" + value + "%' ");
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
        protected object ClearInclude(object entityObject,bool IsIncludeCommitTime=true,bool IsIncludeUpdateTime=true)
        {
            object destObject;
            if (entityObject.GetType().BaseType.FullName.Equals("System.Object"))
            {
                destObject = Activator.CreateInstance(entityObject.GetType());
            }
            else
            {
                destObject = Activator.CreateInstance(entityObject.GetType().BaseType);
            }
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
                        if (!IsIncludeCommitTime)
                        {
                            if (key.ToUpper().Equals("COMMITTIME"))
                            {
                                p_n.SetValue(destObject, null);
                                continue;
                            }
                        }
                        if (!IsIncludeUpdateTime)
                        {
                            if (key.ToUpper().Equals("UPDATETIME"))
                            {
                                p_n.SetValue(destObject, null);
                                continue;
                            }
                        }
                        object origin_pro = p.GetValue(entityObject);
                        if (origin_pro != null) UtilReflection.SetValue(destObject, key, origin_pro.ToString());
                    }
                }
            }
            return destObject;
        }

        /// <summary>
        /// 上传图片文件
        /// </summary>
        /// <param name="files">上传的文件对象</param>
        /// <param name="uploadFlag">上传标识,上传文件的input组件的名称</param>
        /// <param name="upload_dir">上传文件存储的所在目录[最后一级目录，一般对应图片列名称]</param>
        /// <param name="categoryId">上传文件所在的目录标识，一般为类实例名称</param>
        /// <returns></returns>
        public Dictionary<string, object> UploadImage(HttpFileCollection files, string uploadFlag, string upload_dir, string categoryId = "default")
        {
            string diffpart = DateTime.Now.Ticks.ToString();//UtilDateTime.NowS();
            Dictionary<string, object> result = null;
            if ((files != null) && (files.Count > 0))
            {
                string filename, uploadPath, tmptail = "";
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile hpf = files[i];
                    filename = hpf.FileName;
                    if (!String.IsNullOrEmpty(filename))
                    {
                        tmptail = Path.GetExtension(filename);
                        uploadPath = Path.Combine(Business.Gc.UploadPath, "images", categoryId, upload_dir, diffpart + tmptail); //保存路径名称,统一文件命名
                        UtilFile.CreateDir(uploadPath);
                        hpf.SaveAs(uploadPath); //保存文件
                        result = new Dictionary<string, object>();
                        result["success"] = true;
                        result["file_showname"] = hpf.FileName;
                        result["file_name"] = categoryId + "/" + upload_dir + "/" + diffpart + tmptail;
                    }
                }
            }
            return result;
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