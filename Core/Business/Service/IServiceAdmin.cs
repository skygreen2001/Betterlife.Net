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
    public interface IServiceAdmin
    {        
        /// <summary>
        /// 添加系统管理员
        /// </summary>
        /// <param name="username">用户名称</param>
        /// <param name="password">密码</param>
        /// <param name="roletype">角色</param>
        /// <param name="department_id">部门标识</param>
        /// <returns></returns>
        bool addAdmin(string username, string password, int roletype, int department_id);
    }
}
