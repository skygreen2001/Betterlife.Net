using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Util.Common
{
    /// <summary>
    /// 工具类：Dictionary
    /// </summary>
    public class UtilDictionary
    {
        /// <summary>
        /// 移除多个存在指定键的键值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="keys"></param>
        public static void Removes(Dictionary<String, object> data, params string[] keys)
        {
            foreach (var key in keys)
            {
                if (data.ContainsKey(key)) data.Remove(key);
            }
        }
    }
}
