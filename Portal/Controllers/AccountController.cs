using Business.Core;
using Database;
using Portal.Models;
using Portal.Properties;
using System;
using System.Web.Mvc;
using System.Web.Security;
using Util.Common;
using Util.EmailUtil;

namespace Portal.Controllers
{
    public class AccountController : BasicController
    {
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ManageService.UserService().IsValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "提供的用户名或密码不正确。");
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // 尝试注册用户
                int createStatus = ManageService.UserService().CreatUser(model.UserName, model.Password, model.Email, null);
                if (createStatus==0)
                {
                    UtilEmail.SendMail(
                        model.Email,
                        Resources.Subject,
                        "您收到此邮件是因为您是Betterlife.Net网站的会员。我们很高兴地通知您，您现在已经可以观看新的管理博客了。<br/><br/>",
                        true);
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus tt = new MembershipCreateStatus();


                // 在某些出错情况下，ChangePassword 将引发异常，
                // 而不是返回 false。
                bool changePasswordSucceeded= false;
                try
                {
                    User currentUser = ManageService.UserService().GetUserByUsername(User.Identity.Name);
                    if (model.OldPassword.Equals(model.NewPassword))
                    {
                        ModelState.AddModelError("", "新密码不能设置和旧密码一样。");

                    }
                    else if (!currentUser.Password.Equals(UtilEncrypt.MD5Encoding(model.OldPassword)))
                    {
                        ModelState.AddModelError("", "您输入的旧密码不正确！");

                    }else
                    {
                        changePasswordSucceeded = ManageService.UserService().ChangePassword(currentUser, model.OldPassword, model.NewPassword);
                        changePasswordSucceeded = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message); ;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "当前密码不正确或新密码无效。");
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(int createStatus)
        {
            // 请参见 http://go.microsoft.com/fwlink/?LinkID=177550 以查看
            // 状态代码的完整列表。
            switch (createStatus)
            {
                case 6:
                    return "用户名已存在。请输入不同的用户名。";

                case 7:
                    return "该电子邮件地址的用户名已存在。请输入不同的电子邮件地址。";

                case 2:
                    return "提供的密码无效。请输入有效的密码值。";

                case 5:
                    return "提供的电子邮件地址无效。请检查该值并重试。";

                case 1:
                    return "提供的用户名无效。请检查该值并重试。";

                case 11:
                    return "身份验证提供程序返回了错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                case 8:
                    return "已取消用户创建请求。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                case 4:
                    return "提供的密码取回答案无效。请检查该值并重试。";

                case 3:
                    return "提供的密码取回问题无效。请检查该值并重试。";

                case -1:
                    return "用户名或密码为空，请检查该值并重试。";

                default:
                    return "发生未知错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";
            }
        }
        #endregion
    }
}
