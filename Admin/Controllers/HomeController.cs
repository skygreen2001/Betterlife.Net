using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class HomeController : BasicController
    {
        // 控制器:系统管理人员
        // GET: /Home/Admin
        public ActionResult Admin()
        {
            return View();
        }

        // 控制器:博客
        // GET: /Home/Blog
        public ActionResult Blog()
        {
            this.ViewBag.OnlineEditorHtml = this.Load_Onlineditor("Blog_Content", "Comment");
            return View();
        }


    }
}
