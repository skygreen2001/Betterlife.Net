using System.Text.RegularExpressions;

namespace Util.Common
{
    /// <summary>
    /// 字符串工具类
    /// </summary>
	public static class UtilString
	{
        /// <summary>
        /// 判读提供的字符串是否是URL
        /// </summary>
        /// <param name="urlString"></param>
        /// <returns></returns>
        public static bool IsUrl(string urlString)
        {
            const string websitereg = @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            
            Match match = Regex.Match(urlString, websitereg);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 查看是否电话号码
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        public static bool IsCallPhone(string phoneNum)
        {
            if (!UtilNumber.IsDigit(phoneNum))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 查看是否手机号
        /// </summary>
        /// 其中：RegexPatterns引自uoLib.common
        /// <see cref="http://udnz.com/Works/uolib/Default.aspx"/>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(string phoneNum)
        {
            if (!UtilNumber.IsDigit(phoneNum))
            {
                return false;
            }

            Regex re = new Regex(UtilRegex.MobileNum,RegexOptions.IgnoreCase);
            bool result = re.IsMatch(phoneNum);
            return result;
        }

        /// <summary>
        /// 取得MD5加密串
        /// </summary>
        /// HashPasswordForStoringInConfigFile中的Md5算法并非常用的Md5算法
        /// <see cref="http://blog.csdn.net/chaoyang0502/archive/2008/05/15/2448156.aspx"/>
        /// <param name="input">源明文字符串</param>
        /// <returns>密文字符串</returns>
        public static string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = md5.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToUpper());
            }
            string password = s.ToString();
            return password;
        }

        /// <summary>
        /// 字符串头字母大写
        /// </summary>
        /// <param name="content">字符串内容</param>
        public static string UcFirst(string content)
        {
            content = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(content);
            return content;
        }

        /// <summary>
        /// 是否包含指定若干字符串中的一个
        /// </summary>
        /// <returns></returns>
        public static bool Contains(string subject,params string[] args)
        {
            string pattern;
            pattern = string.Join("|",args);
            Regex r = new Regex(pattern);
            return r.IsMatch(subject);

        }
	}
}
