using System.Configuration;
//using System.Windows;

namespace Database.Domain.System.Config
{
    /// <summary>
    /// 显示列的配置实体类
    /// </summary>
    public class ViewColumnsSection : ConfigurationSection
    {
        /// <summary>
        /// 列名
        /// </summary>
        [ConfigurationProperty("name")]
        public string name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// 列显示文字
        /// </summary>
        [ConfigurationProperty("header")]
        public string header
        {
            get { return (string)this["header"]; }
            set { this["header"] = value; }
        }

        /// <summary>
        /// 是否显示
        /// 0-Visible-显示
        /// 1-Hidden-隐藏
        /// 2-Collapsed-折叠
        /// </summary>
        [ConfigurationProperty("visibility")]
        public Visibility visibility
        {

            get { return (Visibility)this["visibility"]; }
            set { this["visibility"] = value; }
        }

        /// <summary>
        /// 列默认文字
        /// </summary>
        [ConfigurationProperty("defaultHeader")]
        public string defaultHeader
        {
            get { return (string)this["defaultHeader"]; }
            set { this["defaultHeader"] = value; }
        }

        /// <summary>
        /// 是否必须的列
        /// </summary>
        [ConfigurationProperty("isMust")]
        public bool isMust
        {
            get { return (bool)this["isMust"]; }
            set { this["isMust"] = value; }
        }
    }
}
