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
        /// </summary>
        public void Run()
        {
            base.Init();
            string ClassName = "Admin";
            string InstanceName = "admin";
            string TableNameComment = "系统管理员";
            //TODO:1.普通的显示cshtml文件【多个文件】
            //       如果是大文本列，需生成@Html.Raw(ViewBag.OnlineEditorHtml),默认不生成
            //[模板文件]:view/view.txt
            //[生成文件名称]:ClassName
            //[生成文件后缀名]:.cshtml
            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "Views" + Path.DirectorySeparatorChar + "Home" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);

            //TODO:2.首页功能列表显示【1个文件】
            //[模板文件]:view/index.txt     
            //生成文件名称:Index.cshtml
            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "Views" + Path.DirectorySeparatorChar + "Index" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);

            //TODO:3.后台extjs文件生成【多个文件】
            //[模板文件]:view/extjs.txt   
            //[生成文件名称]:InstanceName
            //[生成文件后缀名]:.js
            string jsNamespace = "BetterlifeNet";
            string jsNamespace_alias = "Bn";
            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "Scripts" + Path.DirectorySeparatorChar + "core" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);

        }

    }
}
