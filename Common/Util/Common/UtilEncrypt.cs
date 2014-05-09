using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Util.Common
{
    /// <summary>
    /// 工具类:各种合适的加密算法
    /// </summary>
    /// <see cref="http://www.cnblogs.com/zhanqi/archive/2011/02/18/1958117.html">C# .NET 实现 MD5 加密字符串（支持盐值）</see>
    public class UtilEncrypt
    {
        #region -.MD5 加密字符串，支持盐值加密，不可逆
        /// <summary>
       /// MD5 加密字符串
       /// </summary>
       /// <param name="rawPass">源字符串</param>
       /// <returns>加密后字符串</returns>
       public static string MD5Encoding(string rawPass)
       {
           // 创建MD5类的默认实例：MD5CryptoServiceProvider
           MD5 md5 = MD5.Create();
           byte[] bs = Encoding.UTF8.GetBytes(rawPass);
           byte[] hs = md5.ComputeHash(bs);
           StringBuilder sb = new StringBuilder();
           foreach(byte b in hs)
           {
              // 以十六进制格式格式化
              sb.Append(b.ToString("x2"));
           }
           return sb.ToString();
       }
 
       /// <summary>
       /// MD5盐值加密
       /// </summary>
       /// <param name="rawPass">源字符串</param>
       /// <param name="salt">盐值</param>
       /// <returns>加密后字符串</returns>
       public static string MD5Encoding(string rawPass, string salt)
       {
           if(salt == null) return rawPass;
           return MD5Encoding(rawPass + "{" + salt + "}");
       }

        /// <summary>
       /// MD5 加密字符串16位
        /// </summary>
        /// <param name="rawPass"></param>
        /// <returns></returns>
        public static string MD5Encoding16Bit(string rawPass)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(rawPass)), 4, 8);
            t2 = t2.Replace("-", "");
            t2 = t2.ToLower();
            return t2;
        }
        #endregion



    }
}
