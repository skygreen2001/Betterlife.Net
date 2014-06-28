using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    /// <summary>
    /// 系统管理人员
    /// </summary>
    public partial class Admin
    {

        /// <summary>
        /// 部门名称
        /// </summary>
        public String Department_Name
        {
            get;
            set;
        }
        /// <summary>
        /// 扮演角色
        /// 系统管理员扮演角色。
        /// 0:超级管理员-superadmin
        /// 1:管理人员-manager
        /// 2:运维人员-normal
        /// 3:合作伙伴-partner
        /// </summary>
        public String RoletypeShow
        {
            get;
            set;
        }
        /// <summary>
        /// 视野
        /// 0:只能查看自己的信息-self
        /// 1:查看所有的信息-all
        /// </summary>
        public String SeescopeShow
        {
            get;
            set;
        }
    }
}
