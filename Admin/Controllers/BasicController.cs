using Business;
using Database.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Util.View.OnlineEditor;

namespace Admin.Controllers
{
    public class BasicController : Controller
    {
        public int Online_Editor = EnumOnlineEditor.UEDITOR;
        /// <summary>
        /// 加载在线编辑器初始化语句
        /// </summary>
        /// <returns></returns>
        protected string Load_Onlineditor(params string[] Textarea_IDs)
        {
            string result = "";
            switch (Online_Editor)
            {
                case EnumOnlineEditor.UEDITOR:
                    if (Gc.IsDebug)
                    {
                        result = @"
    <script type=""text/javascript"" src=""/Content/common/js/onlineditor/ueditor/ueditor.config.js""></script>
    <script type=""text/javascript"" src=""/Content/common/js/onlineditor/ueditor/ueditor.all.js""></script>
    <script type=""text/javascript"" src=""/Content/common/js/onlineditor/ueditor/ueditor.parse.js""></script>
    <script type=""text/javascript"" src=""/Content/common/js/onlineditor/ueditor/lang/zh-cn/zh-cn.js""></script>
                        ";
                    }
                    else
                    {
                        result = @"
    <script type=""text/javascript"" src=""/Content/common/js/onlineditor/ueditor/ueditor.config.js""></script>
    <script type=""text/javascript"" src=""/Content/common/js/onlineditor/ueditor/ueditor.all.min.js""></script>
    <script type=""text/javascript"" src=""/Content/common/js/onlineditor/ueditor/ueditor.parse.min.js""></script>
    <script type=""text/javascript"" src=""/Content/common/js/onlineditor/ueditor/lang/zh-cn/zh-cn.js""></script>
                        ";

                    }
                    foreach (string Textarea_ID in Textarea_IDs)
                    {
                        result+=UtilUEditor.Init_Function(Textarea_ID);
                    }
                    break;

                case EnumOnlineEditor.CKEDITOR:
                    result = @"

    <script type=""text/javascript"">
        function ckeditor_replace_Feature()
        {
            CKFinder.setupCKEditor(null, ""../Content/common/js/onlineditor/ckfinder/"");
        }
    </script>
                    ";
                    break;

                case EnumOnlineEditor.KINDEDITOR:
                    break;

                case EnumOnlineEditor.XHEDITOR:
                    break;
            }
            return result;
        }
    }
}
