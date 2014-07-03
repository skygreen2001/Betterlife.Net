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

        public static IServiceUser UserService()
        {
            if (userService == null) userService = new ServiceUser();
            return userService;
        }
    }
}