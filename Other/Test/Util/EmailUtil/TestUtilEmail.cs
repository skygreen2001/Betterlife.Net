using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Util.EmailUtil;

namespace Test.Util.EmailUtil
{
    [TestClass]
    public class TestUtilEmail
    {
        /// <summary>
        /// 发送至邮箱
        /// </summary>
        private static string ToEmail = "zhouyuepu@xun-ao.com";
        /// <summary>
        /// 邮件标题
        /// </summary>
        private static string Subject = "乐活betterlife.net";
        /// <summary>
        /// 关键词
        /// </summary>
        private static string SEOName = "乐活";
        /// <summary>
        /// 用户名
        /// </summary>
        private static string UserName = "skygreen2001";
        /// <summary>
        /// 验证邮箱网址
        /// </summary>
        private static string LinkVerifyEmail = "http://blog.csdn.net/skygreen_2001";
        /// <summary>
        /// 测试:发送普通的邮件
        /// </summary>
        [TestMethod]
        public void TestSendEmail()
        {
            UtilEmail.SendMail(
                ToEmail,
                Subject, 
                "您收到此邮件是因为您是Betterlife.Net网站的会员。我们很高兴地通知您，您现在已经可以观看新的管理博客了。<br/><br/>", 
                true);
        }


        /// <summary>
        /// 测试:发送注册成功的邮件
        /// </summary>
        [TestMethod]
        public void TestRegisterSendEmail()
        {
            UtilEmail.SendMail(
                ToEmail,
                Subject,
                @"亲爱的" + UserName + "：<br/><br/>"+
                 "&nbsp;&nbsp;&nbsp;&nbsp;您好！欢迎您加入" + SEOName + "的大家庭。我们将全程为您提供贴心的服务，赶快来看看吧！<br/><br/>",
                true);
        }

        /// <summary>
        /// 测试:发送邮件验证的邮件
        /// </summary>
        [TestMethod]
        public void TestVerifyMailAddressSendEmail()
        {
            UtilEmail.SendMail(
                ToEmail,
                Subject,
                @"亲爱的" + UserName + "：<br/><br/>" +
                 "&nbsp;&nbsp;&nbsp;&nbsp;请点此链接完成邮件验证：<br/><br/> " +
                 "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + LinkVerifyEmail + "<br/><br/>",
                true);
        }


        /// <summary>
        /// 测试:发送附件邮件
        /// </summary>
        [TestMethod]
        public void TestSendMailWithAttachment()
        {
            UtilEmail.SendMailWithAttachment(
                Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Test.dll.config",
                ToEmail,
                Subject,
                @"亲爱的" + UserName + "：<br/><br/>" +
                 "附件仅是我测试使用的配置文件，如有雷同，纯属虚构！<br/><br/>",
                true);

        }
    }
}
