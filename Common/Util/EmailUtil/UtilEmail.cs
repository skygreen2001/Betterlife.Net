using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;

namespace Util.EmailUtil
{
    /// <summary>
    /// 工具类:发送邮件
    /// </summary>
    /// <see cref="http://blog.csdn.net/esinzhong/article/details/7337678" title="Asp.net(C#) 发送Email的用法"/>
    /// <see cref="http://www.csharptutorial.in/17/c-sharp-how-to-send-email-in-asp-net-using-c-sharp" title="C#.Net How To: Send email in asp.net using c#"/>
    public static class UtilEmail
    {
        private static MailMessage mail;
        private static SmtpClient smtp;

        /// <summary>
        /// 连接到邮件服务器
        /// </summary>
        private static void ConnectServer()
        {
            if (mail == null)
            {
                try
                {
                    SmtpSection cfg = ConfigurationManager.GetSection(@"system.net/mailSettings/smtp") as SmtpSection;
                    SmtpNetworkElement SmtpConfig = cfg.Network;
                    mail = new MailMessage
                    {
                        From=new MailAddress(cfg.From)
                    };
                    smtp = new SmtpClient
                    {
                        Host=SmtpConfig.Host,
                        Port=Convert.ToInt32(SmtpConfig.Port),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(SmtpConfig.UserName, SmtpConfig.Password)
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// 添加邮件头设置
        /// </summary>
        private static void AddCustomHeaders()
        {
            ///以下四个自定义Header是将邮件伪装成OutLook发送的
            ///目的是为了防止一些网站的反垃圾邮件功能
            ///将本系统发送的邮件当做垃圾邮件过滤掉。
            mail.Headers.Add("X-Priority", "3");
            mail.Headers.Add("X-MSMail-Priority", "Normal");
            mail.Headers.Add("X-Mailer", "Microsoft Outlook Express 6.00.2900.2869");
            mail.Headers.Add("X-MimeOLE", "Produced By Microsoft MimeOLE V6.00.2900.2869");
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="ToEmail">发送至邮件地址</param>
        /// <param name="Subject">邮件标题</param>
        /// <param name="Body">邮件内容</param>
        /// <param name="IsBodyHtml">邮件是否Html格式</param>
        /// <returns></returns>
        public static bool SendMail(string ToEmail, string Subject, string Body, bool IsBodyHtml=true)
        {
            try
            {
                ConnectServer();
                mail.To.Add(new MailAddress(ToEmail));
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = IsBodyHtml;
                AddCustomHeaders();
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="ToEmail">发送至邮件地址</param>
        /// <param name="Subject">邮件标题</param>
        /// <param name="Body">邮件内容</param>
        /// <param name="IsBodyHtml">邮件是否Html格式</param>
        /// <returns></returns>
        public static bool SendMailWithAttachment(string Attachment_FilePath,string ToEmail, string Subject, string Body, bool IsBodyHtml = true)
        {
            try
            {
                ConnectServer();
                mail.To.Add(new MailAddress(ToEmail));
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = IsBodyHtml;
                if (File.Exists(Attachment_FilePath))
                {
                    Attachment attach = new Attachment(Attachment_FilePath);
                    mail.Attachments.Add(attach);
                }

                AddCustomHeaders();
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
