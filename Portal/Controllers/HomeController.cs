using Portal.Properties;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class HomeController : BasicController
    {
        public ActionResult Index()
        {
            ViewBag.Message =Resources.Welcome;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
