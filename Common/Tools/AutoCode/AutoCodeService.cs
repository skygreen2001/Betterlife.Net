using System.IO;
using Util.Common;

namespace Tools.AutoCode
{
    /// <summary>
    /// 工具类:自动生成代码-服务类
    ///      本框架中包括两种:
    ///      1.Core/Service服务层所有的服务业务类
    ///      2.Business/Admin后台所有ExtService服务类
    /// </summary>
    public class AutoCodeService:AutoCode
    {
        /// <summary>
        /// 服务类生成定义的方式
        /// 1.Core/Service服务层所有的服务业务类|接口
        /// 2.Business/Admin后台所有ExtService服务类
        /// </summary>
        private int ServiceType;

        /// <summary>
        /// 运行主程序
        /// </summary>
        public void Run()
        {
            base.Init();
            string ClassName = "Admin";
            string InstanceName = "admin";
            string TableNameComment = "系统管理员";
            // TODO:1.Core/Service服务层所有的服务业务类|接口【多个文件】
            //[模板文件]:service/service.txt|service/iservice.txt
            //[生成文件名称]:"Service"+ClassName|"IService"+ClassName
            //[生成文件后缀名]:.cs
            Save_Dir = App_Dir + "Core" + Path.DirectorySeparatorChar + "Business" + Path.DirectorySeparatorChar + "Service" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);

            // TODO:2.Business/Admin后台所有ExtService服务类【多个文件】
            //[模板文件]:service/extservice.txt|service/extservicedefine.txt
            //[生成文件名称]:"ExtService"+ClassName|"ExtService"+ClassName
            //[生成文件后缀名]:.ashx.cs|.ashx
            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "Services" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
        }


    }
}
