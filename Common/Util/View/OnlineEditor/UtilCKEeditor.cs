using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.View.OnlineEditor
{
    /// <summary>
    /// 工具类：CKEditor
    /// </summary>
    public static class UtilCKEeditor
    {
        /// <summary>
        /// 初始化加载JS文件
        /// </summary>
        /// <returns></returns>
        public static string Init()
        {
            string result ="";
            result = @"    <script type=""text/javascript"" src=""/Content/common/js/onlineditor/ckeditor/ckeditor.js""></script>";
            result += @"    <script type=""text/javascript"" src=""/Content/common/js/onlineditor/ckfinder/ckfinder.js""></script>";
//            result += @"
//            <script type=""text/javascript"">
//            <![CDATA[
//                window.CKEDITOR_BASEPATH = '{0}/Content/Common/js/onlineditor/ckeditor/';
//            ]]>  
//            </script>
//            ";
            return result;
        }



        /// <summary>
        /// 预加载CKEditor的JS函数
        /// </summary>
        /// <param name=""Textarea_ID"">在线编辑器所在的内容编辑区域TextArea的ID</param>
        /// <param name=""ConfigString"">配置字符串</param>
        /// <returns></returns>
        public static string LoadReplace(string Textarea_ID,string ConfigString="")
        {
            string result ="";
            string jsTemplate = @"
    <script type=""text/javascript"">
        function ckeditor_replace_{0}()
        {{
            var editor_{0} = CKEDITOR.replace('{0}', {{ ""toolbar"": [[""Font"", ""FontSize"", ""TextColor"", ""BGColor""], [""-"", ""Bold"", ""Italic"", ""Underline"", ""Strike""], [""JustifyLeft"", ""JustifyCenter"", ""JustifyRight""], [""Link"", ""Unlink"", ""Image"", ""Source"", ""Maximize""]], ""toolbarStartupExpanded"": true, ""startupOutlineBlocks"": true, ""removeDialogTabs"": ""image:Link;image:advanced"" }});
            CKFinder.setupCKEditor(null,""/Content/common/js/onlineditor/ckfinder/"");
        }}
    </script>
                    ";
            result = string.Format(jsTemplate, Textarea_ID);
            return result;
        }
    }
}
