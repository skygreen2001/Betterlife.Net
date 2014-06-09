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
        public int Online_Editor = EnumOnlineEditor.CKEDITOR;
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
                    result=UtilUEditor.Init(Gc.IsDebug);
                    foreach (string Textarea_ID in Textarea_IDs)
                    {
                        result+=UtilUEditor.Init_Function(Textarea_ID);
                    }
                    break;
                case EnumOnlineEditor.CKEDITOR:
                    result = UtilCKEeditor.Init();
                    foreach (string Textarea_ID in Textarea_IDs)
                    {
                        result += UtilCKEeditor.LoadReplace(Textarea_ID);
                    }
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
