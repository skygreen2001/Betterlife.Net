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
        /// 单例:数据管理器
        /// </summary>
        private static BetterlifeNetEntities db;
        
        /// <summary>
        /// 单例模式:在所有的应用服务中只使用一个单例进行数据库数据管理
        /// 解决问题:当存在多个实例的时候，其中一个实例数据发生改变SaveChanges后，其他实例查询到的数据仍然看不到改变。需要重新实例化才能看到改变。
        /// </summary>
        /// <returns></returns>
        public static BetterlifeNetEntities Instance()
        {
            //if (db == null) db = new BetterlifeNetEntities();
            //实时更新数据模型的数据
            db = new BetterlifeNetEntities();
            return db;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            db = null;
        }
    }
}
