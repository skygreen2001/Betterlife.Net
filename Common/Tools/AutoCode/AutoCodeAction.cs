using System;
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
            //1.生成核心业务控制器
            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
            CreateHomeController();

            //2.生成上传文件控制器
            Save_Dir = App_Dir + "Admin" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);
            CreateUploadController();
        }

        /// <summary>
        /// 1.生成核心业务控制器
        /// [如果是在线编辑器需生成：this.ViewBag.OnlineEditorHtml],默认不生成[1个文件]
        /// [模板文件]:action/homecontroller.txt
        /// 生成文件名称:HomeController.cs
        /// </summary>
        private void CreateHomeController()
        {
            string ClassName = "Admin";
            string Table_Comment = "系统管理员";
            string Template_Name, Unit_Template, Content, MainContent, Textarea_Text;

            //读取原文件内容到内存
            Template_Name = @"AutoCode/Model/action/homecontroller.txt";
            Content = UtilFile.ReadFile2String(Template_Name);
            MainContent = "";
            foreach (string Table_Name in TableList)
            {
                ClassName = Table_Name;
                Table_Comment = TableInfoList[Table_Name]["Comment"];
                string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (t_c.Length > 1) Table_Comment = t_c[0];

                Unit_Template = @"
        // 控制器:{$Table_Comment}
        // GET: /Home/{$ClassName}
        public ActionResult {$ClassName}()
        {
            return View();
        }
                ";
                Textarea_Text = "            this.ViewBag.OnlineEditorHtml = this.Load_Onlineditor(\"Blog_Content\", \"Content\");";
                Unit_Template = Unit_Template.Replace("{$ClassName}", ClassName);
                MainContent += Unit_Template.Replace("{$Table_Comment}", Table_Comment);
            }
            Content = Content.Replace("{$MainContent}", MainContent);
            //存入目标文件内容
            UtilFile.WriteString2File(Save_Dir + "HomeController.cs", Content);
        }

        /// <summary>
        /// 2.生成上传文件控制器
        /// [模板文件]:action/uploadcontroller.txt
        /// 生成文件名称:UploadController.cs
        /// </summary>
        private void CreateUploadController()
        {
            string ClassName = "Admin";
            string InstanceName = "admin";
            string Table_Comment = "系统管理员";
            string Template_Name, Unit_Template, Content, MainContent;

            //读取原文件内容到内存
            Template_Name = @"AutoCode/Model/action/uploadcontroller.txt";
            Content = UtilFile.ReadFile2String(Template_Name);
            MainContent = "";
            foreach (string Table_Name in TableList)
            {
                ClassName = Table_Name;
                Table_Comment = TableInfoList[Table_Name]["Comment"];
                string[] t_c = Table_Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (t_c.Length > 1) Table_Comment = t_c[0];
                InstanceName = UtilString.LcFirst(ClassName);
                Unit_Template = @"
        /// <summary>
        /// 上传Excel文件:{$Table_Comment}
        /// </summary>
        /// <returns></returns>
        // POST: /Upload/Upload{$ClassName}/
        [HttpPost]
        public ActionResult Upload{$ClassName}(FormCollection form)
        {{
            if (Request.Files.Count > 0){{
                HttpPostedFileBase file = Request.Files[0];
                string fileName = Path.Combine(Gc.UploadPath, ""attachment"", ""{$InstanceName}"", ""{$InstanceName}"" + UtilDateTime.NowS() + "".xls"");
                file.SaveAs(fileName);

                JObject resultJ = ExtService{$ClassName}.import{$ClassName}(fileName);
                string result = JsonConvert.SerializeObject(resultJ);
                Response.Write(result);
            }}else{{
                Response.Write(""{'success':false,'data':'上传文件不能为空'}"");
            }}
            return null;
        }}
                ";
                Unit_Template = Unit_Template.Replace("{$ClassName}", ClassName);
                Unit_Template = Unit_Template.Replace("{$InstanceName}", InstanceName);
                MainContent += Unit_Template.Replace("{$Table_Comment}", Table_Comment);
            }
            Content = Content.Replace("{$MainContent}", MainContent);
            //存入目标文件内容
            UtilFile.WriteString2File(Save_Dir + "UploadController.cs", Content);
        }
    }
}
