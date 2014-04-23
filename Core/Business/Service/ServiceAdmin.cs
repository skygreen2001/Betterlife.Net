using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Core.Service
{

    /// <summary>
    /// 服务:系统管理员
    /// </summary>
    public class ServiceAdmin:ServiceBasic,IServiceAdmin
    {
        /// <summary>
        /// 添加系统管理员
        /// </summary>
        /// <param name="username">用户名称</param>
        /// <param name="password">密码</param>
        /// <param name="roletype">角色</param>
        /// <param name="department_id">部门标识</param>
        /// <returns></returns>
        public bool addAdmin(string username, string password, int roletype, int department_id)
        {
            //1.确认用户名和密码是否为空，如果为空，返回false
            //2.查看部门标识的部门是否存在，如果不存在，标识为普通部门
            //3.查看角色类型是否定义过，如果没有，标识为普通用户
            //4.密码需要进行加密，采用md5不可逆编码
            //5.确认用户名称是否已经使用过，如果已经使用过，返回false
            return true;
        }
    }
}
