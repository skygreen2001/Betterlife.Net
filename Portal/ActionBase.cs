using Portal.Controllers;
using Business;
using Database.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Util.Common;
using Database;
using Business.Core.Service;

namespace Portal
{
    /// <summary>
    /// 所有Controller的拦截器:在Controller的action方法执行前和执行后执行的方法
    /// </summary>
    /// <see cref="http://www.cnblogs.com/fly_dragon/archive/2011/06/15/2081063.html"/>
    public class ActionBase : ActionFilterAttribute
    {
        public string Message { get; set; }

        /// <summary>
        /// 在所有的Controller执行Action方法后执行【after action】
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //var properties = new Dictionary<string, string>();
            //if (filterContext.Result is ViewResult)
            //{
            //    var action = filterContext.Result as ViewResult;
            //    properties.Add("Page.Title", action.ViewBag.Title);
            //}

            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// 在所有的Controller执行Action方法前执行【before action】
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 在执行操作结果后由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        { 
            base.OnResultExecuted(filterContext);
        }

        /// <summary>
        /// 在执行操作结果之前由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
}
