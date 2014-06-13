namespace Database
{
    using System;
    /// <summary>
    /// 系统管理员
    /// </summary>
    public partial class Admin
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 部门标识
        /// </summary>
        public int Department_ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Realname { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 扮演角色
        /// 系统管理员扮演角色。
        /// 0:超级管理员-superadmin
        /// 1:管理人员-manager
        /// 2:运维人员-normal
        /// 3:合作伙伴-partner
        /// </summary>
        public string Roletype { get; set; }
        /// <summary>
        /// 视野
        /// 0:只能查看自己的信息-self
        /// 1:查看所有的信息-all
        /// </summary>
        public string Seescope { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public Nullable<int> LoginTimes { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<System.DateTime> CommitTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public Nullable<System.DateTime> UpdateTime { get; set; }
    
        public virtual Department Department { get; set; }
    }
}
