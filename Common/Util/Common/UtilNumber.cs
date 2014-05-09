
using System;
namespace Util.Common
{
    /// <summary>
    /// 数字处理类
    /// </summary>
	public static class UtilNumber
	{
        /// <summary>
        /// 据说效率最高
        /// </summary>
        /// <param name="_string"></param>
        /// <returns></returns>
        /// <see cref="http://www.cnblogs.com/sohighthesky/archive/2010/01/31/how-to-test-isnumberic.html"/>
        public static bool IsDigit(this string _string)
        {
            if (string.IsNullOrEmpty(_string))
                return false;
            foreach (char c in _string)
            {
                if (!char.IsDigit(c))//if(c<'0' || c>'9')//最好的方法,在下面测试数据中再加一个0，然后这种方法效率会搞10毫秒左右
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 转换数字字符串到数字
        /// </summary>
        /// <param name="numberString"></param>
        /// <param name="defaultValue">无解析结果，结果为指定默认值</param>
        /// <returns></returns>
        public static int Parse(string numberString,int defaultValue=100)
        {
            int result = Parse(numberString);
            if (result == 0)
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// 转换数字字符串到数字
        /// </summary>
        /// <param name="numberString"></param>
        /// <returns></returns>
        public static int Parse(string numberString)
        {
            int result=0;
            int.TryParse(numberString,out result);
            return result;
        }

        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <param name="int_NumberLength">随机数长度</param>
        /// <returns></returns>
        public static string RandomNumber(int int_NumberLength)
        {
            System.Text.StringBuilder str_Number = new System.Text.StringBuilder();//字符串存储器
            Random rand = new Random(Guid.NewGuid().GetHashCode());//生成随机数

            for (int i = 1; i <= int_NumberLength; i++)
            {
                str_Number.Append(rand.Next(0, 10).ToString());//产生0~9的随机数
            }

            return str_Number.ToString();
        }
	}
}
