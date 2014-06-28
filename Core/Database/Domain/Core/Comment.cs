namespace Database
{
    using System;

    /// <summary>
    /// 评论
    /// </summary>
    public partial class Comment
    {

        /// <summary>
        /// 用户名
        /// </summary>
        public String Username
        {
            get;
            set;
        }
        /// <summary>
        /// 评论内容
        /// </summary>
        public String Content
        {
            get;
            set;
        }
        /// <summary>
        /// 博客标题
        /// </summary>
        public String Blog_Name
        {
            get;
            set;
        }
    }
}
