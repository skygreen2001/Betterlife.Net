using System.Web.Mvc;

namespace AdminManage.Controllers
{
    /// <summary>
    /// 主控制器:后台主要业务逻辑页面功能
    /// </summary>
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
            this.ViewBag.OnlineEditorHtml = this.Load_Onlineditor("Blog_Content", "Content");
            return View();
        }
    }
}
