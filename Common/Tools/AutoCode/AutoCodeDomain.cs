using System.IO;
using Util.Common;

namespace Tools.AutoCode
{
    /// <summary>
    /// 工具类:自动生成代码-实体类
    /// </summary>
    public class AutoCodeDomain:AutoCode
    {
        /// <summary>
        /// 运行主程序
        /// 1.生成实体类
        /// 2.生成上下文环境类
        /// 3.生成枚举类
        /// 4.实体类有外键的实体类需要生成HttpData文件
        /// </summary>
        public void Run()
        {
            base.Init();
            string EntitiesName = "BetterlifeNetEntities";
            string ClassName = "Admin";
            string InstanceName = "admin";
            string TableNameComment = "系统管理员";

            // TODO:1.生成实体类【多个文件】
            //[模板文件]:domain/domain.txt            
            //[生成文件名]:ClassName
            //[生成文件后缀名]:.cs
            Save_Dir = App_Dir + "Core" + Path.DirectorySeparatorChar + "Database" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);

            // TODO:2.生成上下文环境类【1个文件】
            //[模板文件]:domain/context.txt
            //生成文件名称:BetterlifeNetEntities.Context.cs[EntitiesName+".Context.cs"]
            Save_Dir = App_Dir + "Core" + Path.DirectorySeparatorChar + "Database" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);

            // TODO:3.生成枚举类【一个文件】
            //[模板文件]:domain/enum.txt
            //生成文件名称:Enum.cs
            Save_Dir = App_Dir + "Core" + Path.DirectorySeparatorChar + "Database" + Path.DirectorySeparatorChar + "Domain" + Path.DirectorySeparatorChar + "Enums" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
           
            // TODO:4.实体类有外键的实体类需要生成HttpData 文件
            //        例如Admin有外键Department_ID,会生成Department的HttpData类【多个文件】
            //[模板文件]:domain/httpdata.txt|domain/httpdatadefine.txt
            //[生成文件名称]:ClassName|ClassName
            //[生成文件后缀名]:.ashx.cs|.ashx"
            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "HttpData" + Path.DirectorySeparatorChar + "Core"+ Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
            
        }
    }
}
