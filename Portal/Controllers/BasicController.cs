using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Util.Reflection;
using Portal.Code.Util;
using Portal.Properties;

namespace Portal.Controllers
{
    /// <summary>
    /// 前台所有控制器的父类
    /// </summary>
    public class BasicController : Controller
    {
        /// <summary>
        /// 在所有子类执行方法之前执行
        /// 执行顺序为:
        /// 1.BasicController ExecuteCore
        /// 2.ActionBase OnActionExecuting
        /// 3.请求Controller的请求方法
        /// </summary>
        protected override void ExecuteCore()
        {
            ViewBag.Title = Resources.SiteName;
            ViewBag.SiteName = Resources.SiteName;

            UseMultiLanguage();
            base.ExecuteCore();
        }

        /// <summary>
        /// 多语言执行选择
        /// </summary>
        private void UseMultiLanguage()
        {
            string cultureName = null;
            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages

            // Validate culture name
            cultureName = UtilCultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        /// <summary>
        /// 清除数据对象关联的数据对象,一般在获取到所需数据之后最后执行
        /// </summary>
        protected object ClearInclude(object entityObject)
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
                        object origin_pro = p.GetValue(entityObject);
                        if (origin_pro != null) UtilReflection.SetValue(destObject, key, origin_pro.ToString());
                    }
                }
            }
            return destObject;
        }


    }
}
