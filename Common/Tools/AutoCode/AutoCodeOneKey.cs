using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.AutoCode.Prepare;

namespace Tools.AutoCode
{
    /// <summary>
    /// 一键生成所有主程序
    /// </summary>
    class AutoCodeOneKey:AutoCodeBase
    {
        /// <summary>
        /// 生成实体类
        /// </summary>
        public AutoCodeDomain domain = new AutoCodeDomain();
        /// <summary>
        /// 生成服务类
        /// </summary>
        public AutoCodeService service = new AutoCodeService();
        /// <summary>
        /// 生成控制器类
        /// </summary>
        public AutoCodeAction action = new AutoCodeAction();
        /// <summary>
        /// 生成表示层类
        /// </summary>
        public AutoCodeView view = new AutoCodeView();
        /// <summary>
        /// 生成表示层类:后台Extjs类
        /// </summary>
        public AutoCodeViewExt viewExtjs = new AutoCodeViewExt();
        /// <summary>
        /// 生成生成代码配置文件
        /// </summary>
        public AutoCodeConfig autoconfig = new AutoCodeConfig();


        /// <summary>
        /// 代码生成主程序
        /// </summary>
        public void Run()
        {
            autoconfig.Run();

            //读取配置文件里查询条件和关系列显示的配置
            AutoCodeViewExt.Filter_Fieldnames=autoconfig.ToDictionary();

            base.Init();

            domain.Run();
            
            service.ServiceType = 1;
            service.Run();

            service.ServiceType = 2;
            service.Run();

            action.Run();

            view.Run();
            viewExtjs.Run();
        }
    }
}
