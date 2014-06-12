using AdminManage.Controllers;
using Business;
using Database.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Util.Common;

namespace AdminManage
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
            //在Action执行之后执行 输出到输出流中文字：After Action execute xxx
            //filterContext.HttpContext.Response.Write(@"<br />After Action execute" + "\t " + Message);
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// 在所有的Controller执行Action方法前执行【before action】
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Title = Gc.SiteName;
            filterContext.Controller.ViewBag.SiteName = Gc.SiteName;

            //设置在线编辑器的类型:UEditor|CKEditor
            HttpCookie OnlineEditorCookie=filterContext.RequestContext.HttpContext.Request.Cookies["OnlineEditor"];
            if (OnlineEditorCookie!=null)
                BasicController.Online_Editor = UtilNumber.Parse(OnlineEditorCookie.Value, EnumOnlineEditor.UEDITOR);

            //在Action执行前执行
            //filterContext.HttpContext.Response.Write(@"<br />Before Action execute" + "\t " + Message);
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        { 
            //在Result执行之后 
            //filterContext.HttpContext.Response.Write(@"<br />After ViewResult execute" + "\t " + Message);
            base.OnResultExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //在Result执行之前
            //filterContext.HttpContext.Response.Write(@"<br />Before ViewResult execute" + "\t " + Message);
            base.OnResultExecuting(filterContext);
        }


    }
}
