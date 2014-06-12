using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    /// <summary>
    /// 数据管理中心
    /// 所有调用数据库数据资源的调度中心
    /// </summary>
    public class DatabaseCenter
    {
        /// <summary>
        /// 
        /// </summary>
        public static BetterlifeNetEntities db;
        
        /// <summary>
        /// 单例模式:在所有的应用服务中只使用一个单例进行数据库数据管理
        /// </summary>
        /// <returns></returns>
        public static BetterlifeNetEntities Instance()
        {
            if (db == null) db = new BetterlifeNetEntities();
            return db;
        }
    }
}
