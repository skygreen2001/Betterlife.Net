using Business.Core.Service;

namespace Portal
{
    /// <summary>
    /// 服务类:所有Service的管理类
    /// </summary>
    public class ManageService
    {
        /// <summary>
        /// 用户服务
        /// </summary>
        private static IServiceUser userService;

        /// <summary>
        /// 初始化工作
        /// </summary>
        private static void Init()
        {
            //处理数据库获取数据缓存的问题【当直接修改数据库数据,需重置获取数据】
            ServiceBasic.Init_Db();
        }

        public static IServiceUser UserService()
        {
            Init();
            if (userService == null) userService = new ServiceUser();
            return userService;
        }
    }
}