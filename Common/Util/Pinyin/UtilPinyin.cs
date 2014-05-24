using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PinginObject=Util.Pinyin.NPinyin.Pinyin;

namespace Util.Pinyin
{
    /// <summary>
    /// 中文转拼音工具类
    /// </summary>
    public static class UtilPinyin
    {
        /// <summary>
        /// 中文转拼音
        /// </summary>
        /// <param name="content">原中文内容</param>
        /// <returns></returns>
        public static string Translate(string content)
        {
            return PinginObject.GetPinyin(content);
        }

        /// <summary>
        /// 中文拼音头字母
        /// </summary>
        /// <param name="content">原中文内容</param>
        /// <returns>返回中文拼音头字母</returns>
        public static string LetterFirst(string content)
        {
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            content = PinginObject.ConvertEncoding(content, Encoding.UTF8, gb2312);
            return PinginObject.GetInitials(content, gb2312);
        }
    }
}
