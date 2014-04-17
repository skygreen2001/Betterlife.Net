using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        // 控制器:系统管理人员
        // GET: /Home/Admin
        public ActionResult Admin()
        {
            return View();
        }

    }
}
