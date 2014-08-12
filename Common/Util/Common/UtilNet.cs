using System.Net;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Diagnostics;

namespace Util.Common
{
    /// <summary>
    /// 网络工具类
    /// </summary>
    public static class UtilNet
    {
        /// <summary>
        /// 检测URL是否存在
        /// </summary>
        /// <see cref="http://www.qimao.cn/article.asp?id=224"/>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool UrlExistsUsingHttpWebRequest(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Timeout = 50000;
            //req.Method = "HEAD";
            //req.AllowAutoRedirect = true;  //设置请求是否应跟随重定向响应
            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                {
                    return (resp.StatusCode == System.Net.HttpStatusCode.OK);
                }
            }
            catch (WebException we)//异常
            {
                Debug.WriteLine(we.Message);
                return false;
            }
            finally
            {
                req.Abort();//取消请求
            }
        }

        /// <summary>
        /// 将URL地址中的域名替换成IP地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDomaintoIp(string url)
        {
            Uri u = new Uri(url);
            String domain = u.Host;
            //String domain=System.Text.RegularExpressions.Regex.Match(url, @"(?<=://)[a-zA-Z\.0-9]+(?=\/)").Value.ToString(); 
            string ip = IPParse(domain);
            url = url.Replace(domain, ip);
            return url;
        }

        /// <summary>
        /// MD5编码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string MD5(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            else
            {
                MD5 m = new MD5CryptoServiceProvider();
                byte[] s = m.ComputeHash(UnicodeEncoding.UTF8.GetBytes(content));
                string result = BitConverter.ToString(s);
                result = result.Replace("-", "");
                result = result.ToLower();
                return result;
            }
        }

        /// <summary>
        /// 将域名替换成IP地址<br/>
        /// 由于BetterlifeNet的IP地址虚拟映射的域名方能正常访问应用<br/>
        /// BetterlifeNet虚拟的域名并不是真正注册的域名，一般以.betterlife.net结尾，它需要在系统的host文件下注册与IP的映射。<br/>
        /// 因此在本应用中对BetterlifeNet虚拟的域名不需要做ip地址转换。
        /// </summary>
        /// <param name="domainName"></param>
        /// <returns></returns>
        public static string IPParse(string domainName)
        {
            string result = domainName;
            if (!result.Contains(".betterlife.net"))
            {
                try
                {
                    IPAddress[] dnstoip = new IPAddress[2];

                    dnstoip = Dns.GetHostAddresses(domainName);
                    for (int i = 0; i < dnstoip.Length; i++)
                    {
                        result = dnstoip[i].ToString();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取客户端Ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }

    }
}
