using AdminManage.Models;
using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Administrator = Database.Admin;

namespace AdminManage.Controllers
{
    public class IndexController : Controller
    {
        /// <summary>
        /// 单例:数据管理器
        /// </summary>
        protected static BetterlifeNetEntities db = DatabaseCenter.Instance();

        //首页
        // GET: /Index/
        public ActionResult Index()
        {
            return View();
        }

        //显示登录页面
        // GET: /Login/
        public ActionResult Login()
        {
            return View();
        }

        //登录
        // POST: /Index/Login
        [HttpPost]
        public ActionResult Login(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string ValidateNumber = (string)Session["ValidateNumber"];

                if (model.ValidateCode.Equals(ValidateNumber))
                {
                    Administrator admin = db.Admin.SingleOrDefault(e => e.Username.Equals(model.UserName) &&
                                         e.Password.Equals(model.Password));
                    if (admin == null)
                    {
                        ModelState.AddModelError("", "用户名或密码不正确，请重试！");
                    }
                    else
                    {
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Index");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "提供的图形验证码不正确。");
                }
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);

        }

        //登出
        // GET: /Index/Logout
        public ActionResult Logout()
        {
            return RedirectToAction("Login", "Index");
        }

    }
}
