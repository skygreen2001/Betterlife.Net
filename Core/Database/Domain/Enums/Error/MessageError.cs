
namespace Database.Domain.Enums.Error
{

    /// <summary>
    /// 接收系统错误事件信息类型
    /// </summary>
    public enum MessageError : int
    {
        /// <summary>
        /// 座席加入队列失败，队列不存在。
        /// </summary>
        DATAJOINQUEUENOTEXIST = -1,
        /// <summary>
        /// 座席加入队列失败，座席已存在。
        /// </summary>
        DATAJOINAGENTHASEXIST = -2,
        /// <summary>
        /// 座席退出队列失败，座席不存在。
        /// </summary>
        DATAEXITAGENTHASNOTEXIST = -3,
        /// <summary>
        /// 座席退出队列失败，队列不存在。
        /// </summary>
        DATAEXITQUEUENOTEXIST = -4,
        /// <summary>
        /// 该帐号在其他地方登录。
        /// </summary>
        DATALOGINOTHERPLACE = -8,
        /// <summary>
        /// 分机下线。
        /// </summary>
        DATAEXTERNLOGOUT = -9
    }

}
