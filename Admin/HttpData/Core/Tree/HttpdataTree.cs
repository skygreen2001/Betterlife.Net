using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Domain
{
    /// <summary>
    /// 目录树通用结构
    /// </summary>
    class HttpdataTree
    {
        /// <summary>
        /// 标识
        /// </summary>
        public String id
        {
            get;
            set;
        }

        /// <summary>
        /// 显示内容
        /// </summary>
        public String text
        {
            get;
            set;
        }

        /// <summary>
        /// 当前层级
        /// </summary>
        public String level
        {
            get;
            set;
        }

        /// <summary>
        /// 当前图标的样式
        /// file|folder
        /// </summary>
        public String cls
        {
            get;
            set;
        }
        /// <summary>
        /// 是否叶子
        /// </summary>
        public bool leaf
        {
            get;
            set;
        }
    }
}
