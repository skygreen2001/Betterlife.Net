using Database;

namespace Business.Core.Service
{
    public class ServiceBasic
    {
        protected static BetterlifeNetEntities db = DatabaseCenter.Instance();
        /// <summary>
        /// 初始化工作
        /// 1.初始化获取数据库数据单例
        /// 一般是在服务器变更了数据库，重置DatabaseCenter的db，重新获取一次即可
        /// </summary>
        public static void Init_Db()
        {
            db = DatabaseCenter.Instance();
        }
    }
}
