using System.IO;
using Util.Common;

namespace Tools.AutoCode
{
    /// <summary>
    /// 工具类:自动生成代码-控制器类
    /// </summary>
    public class AutoCodeAction: AutoCode
    {
        /// <summary>
        /// 运行主程序
        /// 1.生成核心业务控制器
        /// 2.生成上传文件控制器
        /// </summary>
        public void Run()
        {
            base.Init();
            string ClassName = "Admin";
            string InstanceName = "admin";
            string TableNameComment = "系统管理员";

            //TODO:1.生成核心业务控制器
            //       [如果是在线编辑器需生成：this.ViewBag.OnlineEditorHtml],默认不生成[1个文件]
            //[模板文件]:action/homecontroller.txt
            //生成文件名称:HomeController.cs
            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);

            //TODO:2.生成上传文件控制器[1个文件]
            //[模板文件]:action/uploadcontroller.txt
            //生成文件名称:UploadController.cs
            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);

        }
    }
}
