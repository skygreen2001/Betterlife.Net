
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
        public static bool isDigit(this string _string)
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
        public static int parse(string numberString,int defaultValue=100)
        {
            int result = parse(numberString);
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
        public static int parse(string numberString)
        {
            int result=0;
            int.TryParse(numberString,out result);
            return result;
        }
	}
}
