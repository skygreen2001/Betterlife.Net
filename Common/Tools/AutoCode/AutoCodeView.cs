using System;
using System.IO;
using Util.Common;

namespace Tools.AutoCode
{
    /// <summary>
    /// 工具类:自动生成代码-使用后台生成的表示层
    /// </summary>
    public class AutoCodeView:AutoCode
    {
        /// <summary>
        /// 运行主程序
        /// 1.后台普通的显示cshtml文件
        /// 2.后台首页功能列表显示
        /// </summary>
        public void Run()
        {
            base.Init();

            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "Views" + Path.DirectorySeparatorChar + "Home" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
            CreateNormalView();

            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "Views" + Path.DirectorySeparatorChar + "Index" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
            CreateIndexView();
        }

        /// <summary>
        /// 1.后台普通的显示cshtml文件【多个文件】
        ///       如果是大文本列，需生成@Html.Raw(ViewBag.OnlineEditorHtml),默认不生成
        /// [模板文件]:view/view.txt
        /// [生成文件名称]:ClassName
        /// [生成文件后缀名]:.cshtml
        /// </summary>
        private void CreateNormalView()
        {
            string ClassName = "Admin";
            string InstanceName = "admin";
            string Table_Comment = "系统管理员";
            string Template_Name, Content, Content_New, OnlineEditorHtml;

            foreach (string Table_Name in TableList)
            {
                //读取原文件内容到内存
                Template_Name = @"AutoCode/Model/view/view.txt";
                Content = UtilFile.ReadFile2String(Template_Name);
                ClassName = Table_Name;
                Table_Comment = TableInfoList[Table_Name]["Comment"];
                string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (t_c.Length > 1) Table_Comment = t_c[0];
                InstanceName = UtilString.LcFirst(ClassName);
                
                Content_New = Content.Replace("{$ClassName}", ClassName);
                Content_New = Content_New.Replace("{$Table_Comment}", Table_Comment);
                Content_New = Content_New.Replace("{$InstanceName}", InstanceName);

                OnlineEditorHtml = "	@Html.Raw(ViewBag.OnlineEditorHtml)";//TODO:
                Content_New = Content_New.Replace("{$OnlineEditorHtml}", OnlineEditorHtml);

                //存入目标文件内容
                UtilFile.WriteString2File(Save_Dir + ClassName + ".cshtml", Content_New);
            }
        }

        /// <summary>
        /// 2.后台首页功能列表显示【1个文件】
        /// [模板文件]:view/index.txt     
        /// 生成文件名称:Index.cshtml
        /// </summary>
        private void CreateIndexView()
        {
            string Site_SEO = "Betterlife.Net网站框架";
            string ClassName = "Admin";
            string InstanceName = "admin";
            string Table_Comment = "系统管理员";
            string Template_Name,Unit_Template, Content;

            //读取原文件内容到内存
            Template_Name = @"AutoCode/Model/view/index.txt";
            Content = UtilFile.ReadFile2String(Template_Name);
            string MainContent = "";
            foreach (string Table_Name in TableList)
            {
                ClassName = Table_Name;
                Table_Comment = TableInfoList[Table_Name]["Comment"];
                string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (t_c.Length > 1) Table_Comment = t_c[0];
                InstanceName = UtilString.LcFirst(ClassName);
                Unit_Template = @"
        <p>@Html.ActionLink(""{$Table_Comment}"", ""{$ClassName}"", ""Home"",null,new { id = ""{$InstanceName}"",title = ""{$Table_Comment}"",@class=""menuIcon""})</p>";
                Unit_Template = Unit_Template.Replace("{$ClassName}", ClassName);
                Unit_Template = Unit_Template.Replace("{$Table_Comment}", Table_Comment);
                Unit_Template = Unit_Template.Replace("{$InstanceName}", InstanceName);
                MainContent += Unit_Template;
            }
            MainContent = MainContent.Substring(2);
            Content = Content.Replace("{$MainContent}", MainContent);
            Content = Content.Replace("{$Site_SEO}", Site_SEO);


            //存入目标文件内容
            UtilFile.WriteString2File(Save_Dir + "Index.cshtml", Content);
            

        }

    }
}
