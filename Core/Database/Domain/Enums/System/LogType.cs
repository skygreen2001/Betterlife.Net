
using Util.Reflection;
namespace Database.Domain.Enums.System
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public struct LogType
    {
        public static int TYPE_UNCONFIRMTRADE = 1;//待交易确认
        public static int TYPE_CALLCOME = 10;//普通来电
        public static int TYPE_MEMBERCALL = 11;//会员来电
        public static int TYPE_CUSTOMERCALL = 12;//客户来电
        public static int TYPE_EXIT = -1;//退出

        public static string DESC_UNCONFIRMTRADE = "待交易确认";
        public static string DESC_CALLCOME = "普通来电";
        public static string DESC_MEMBERCALL = "会员来电";
        public static string DESC_CUSTOMERCALL = "客户来电";
        public static string DESC_EXIT = "您确认要退出吗？";

        /// <summary>
        /// 根据Log Type显示描述
        /// </summary>
        /// <param name="ivrStatueType"></param>
        /// <returns></returns>
        public static string ShowLogType(string logType)
        {
            string result = UtilReflection.GetPublicStaticFieldValueByValue(typeof(LogType), logType, "TYPE", "DESC");
            return result;
        }
    }
}
