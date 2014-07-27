using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class HomeController : BasicController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "欢迎使用 Betterlife.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
