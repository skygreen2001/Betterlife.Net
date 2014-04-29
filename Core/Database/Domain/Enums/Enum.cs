using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Database.Domain.Enums
{
    /// <summary>
    /// 系统管理员扮演角色
    /// </summary>
    public class EnumRoleType
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        public const char Superadmin = '0';
        /// <summary>
        /// 普通管理员
        /// </summary>
        public const char Manager = '1';

        /// <summary>
        /// 显示同步行为
        /// </summary>
        public static String RoletypeShow(char Type)
        {
            switch (Type)
            {
                case Superadmin:
                    return "超级管理员";
                case Manager:
                    return "普通管理员";
            }
            return "未知";
        }
    }

    /// <summary>
    /// 视野
    /// </summary>
    public class EnumSeescope
    {
        /// <summary>
        /// 只能查看自己的信息
        /// </summary>
        public const char Self = '0';
        /// <summary>
        /// 查看所有的信息
        /// </summary>
        public const char All = '1';

        /// <summary>
        /// 显示同步行为
        /// </summary>
        public static String SeescopeShow(char Type)
        {
            switch (Type)
            {
                case Self:
                    return "只能查看自己的信息";
                case All:
                    return "查看所有的信息";
            }
            return "未知";
        }
    }
}