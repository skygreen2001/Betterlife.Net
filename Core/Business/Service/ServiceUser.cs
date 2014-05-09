using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Common;

namespace Business.Core.Service
{
    /// <summary>
    /// 服务:用户
    /// </summary>
    public class ServiceUser:ServiceBasic,IServiceUser
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="username">用户名称</param>
        /// <param name="password">密码</param>
        /// <param name="Email">邮箱地址</param>
        /// <param name="Cellphone">联系电话</param>
        /// <returns></returns>
        public int CreatUser(string UserName, string Password, string Email, string Cellphone)
        {
            //1.确认用户名和密码是否为空，如果为空，返回 -1
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password)) return 0;

            //UNDONE:2.用户名是否无效【如含有特殊符号】

            //UNDONE:3.电子邮件地址是否已存在，如果存在，返回 7

            //4.确认用户名称是否已经使用过，如果已经使用过，返回 6
            if (IsUsernameExist(UserName,null))
            {
                return 6;
            }else{
                User user = new User();
                user.Username = UserName;
                //5.密码需要进行加密，采用md5不可逆编码
                Password = UtilEncrypt.MD5Encoding(Password);
                user.Password = Password;
                user.Email = Email;
                user.Cellphone = Cellphone;
                user.Logintimes = 1;
                user.Committime = DateTime.Now;
                user.Updatetime = DateTime.Now;
                db.User.Add(user);
                db.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="username">用户名称</param>
        /// <returns></returns>
        public User GetUserByUsername(string UserName)
        {
            return db.User.Where(e => e.Username.Equals(UserName)).Single();
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="OldPassword">旧密码</param>
        /// <param name="NewPassword">新密码</param>
        /// <returns></returns>
        public bool ChangePassword(User user, string OldPassword, string NewPassword)
        {
            try
            {
                NewPassword = UtilEncrypt.MD5Encoding(NewPassword);
                if (user.Password.Equals(NewPassword))
                {
                    return false;
                }else{
                    user.Password = NewPassword;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 校验是否合法用户
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        public bool IsValidateUser(string UserName, string Password)
        {
            bool result;
            Password=UtilEncrypt.MD5Encoding(Password);
            int count=db.User.Where(e => e.Username.Equals(UserName) &&
                e.Password.Equals(Password)
            ).Count();
            if (count == 1) result = true; else result = false; 
            return result;
        }

        /// <summary>
        /// 用户名称是否使用
        /// </summary>
        /// <param name="Username">用户名</param>
        /// <param name="User_ID">用户ID</param>
        /// <returns>true:已使用 ;false:未使用</returns>
        public bool IsUsernameExist(string Username, string User_ID)
        {
            bool Used = true;
            var userToUpdate = db.User.FirstOrDefault(person => person.Username == Username);
            if (userToUpdate != null)
            {
                Used = false;
            }
            else
            {
                if (!String.IsNullOrEmpty(User_ID))
                {
                    int id = UtilNumber.Parse(User_ID);
                    User user = db.User.Single(e => e.ID.Equals(id));
                    if (user != null && user.Username == Username)
                    {
                        Used = false;
                    }
                }
                else
                {
                    Used = false;
                }
            }
            return Used;
        }
    }
}
