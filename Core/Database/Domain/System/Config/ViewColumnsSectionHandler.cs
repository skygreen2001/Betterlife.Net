using System;
using System.Collections.Generic;
using System.Configuration;
//using System.Windows;
using System.Xml;
using Util.Common;

namespace Database.Domain.System.Config
{
    class ViewColumnsSectionHandler : IConfigurationSectionHandler
    {
        private readonly static string defaultNodeName = "column";
        private static Dictionary<string, ViewColumnsSection> viewColumns;

        public object Create(object parent, object context, XmlNode section)
        {
            //获取配置文件中ViewUIColumns节点的defaultNodeName所有子节点
            XmlNodeList nodes = section.SelectNodes(defaultNodeName);
            viewColumns = new Dictionary<string, ViewColumnsSection>();
            ViewColumnsSection column = null;
            foreach (XmlNode n in nodes)
            {
                column = new ViewColumnsSection();
                column.isMust = false;
                foreach (XmlAttribute att in n.Attributes)
                {
                    if (att.Name == "name")
                    {
                        column.name = att.Value;
                        continue;
                    }
                    if (att.Name == "header")
                    {
                        column.header = att.Value;
                        continue;
                    }

                    if (att.Name == "visibility")
                    {
                        column.visibility = (Visibility)UtilNumber.Parse(att.Value);
                        continue;
                    }
                    if (att.Name == "defaultHeader")
                    {
                        column.defaultHeader = att.Value;
                        continue;
                    }
                    if (att.Name == "isMust")
                    {
                        bool isMust=false;
                        bool.TryParse(att.Value,out isMust);
                        column.isMust = isMust;
                        continue;
                    }
                }
                if (column.name != String.Empty && column.header != String.Empty)
                {
                    viewColumns.Add(column.name, column);
                }
                else
                {
                    Console.WriteLine(String.Format("配置文件{0}节点出错。\r\n{1}:{2}", section.Name, column.name, column.header));
                }
            }
            if (viewColumns.Count == 0)
            {
                return null;
            }
            return viewColumns;
        }
    }
}
