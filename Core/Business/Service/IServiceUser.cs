using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Core.Service
{
    /// <summary>
    /// 服务:用户
    /// </summary>
    public interface IServiceUser
    {        
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="username">用户名称</param>
        /// <param name="password">密码</param>
        /// <param name="email">邮箱地址</param>
        /// <param name="cellphone">手机号码</param>
        /// <returns>返回状态参考MembershipCreateStatus[System.Web.ApplicationServices.dll, v4.0.0.0]</returns>
        int CreatUser(string UserName, string Password, string Email, string Cellphone);
        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="username">用户名称</param>
        /// <returns></returns>
        User GetUserByUsername(string UserName);
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="OldPassword">旧密码</param>
        /// <param name="NewPassword">新密码</param>
        /// <returns></returns>
        bool ChangePassword(User user,string OldPassword,string NewPassword);
        /// <summary>
        /// 校验是否合法用户
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        bool IsValidateUser(string UserName, string Password);
        /// <summary>
        /// 用户名称是否使用
        /// </summary>
        /// <param name="Username">用户名</param>
        /// <param name="user_id">用户ID</param>
        /// <returns>true:已使用 ;false:未使用</returns>
        bool IsUsernameExist(string Username, string User_ID);
    }
}
