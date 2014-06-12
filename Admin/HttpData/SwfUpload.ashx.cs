using System;
using System.Web;
using System.IO;
using Business;

namespace AdminManage.HttpData
{
    /// <summary>
    /// 处理上传图片
    /// </summary>
    public class SwfUpload : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                HttpPostedFile upload = context.Request.Files["Filedata"];
                string name = DateTime.Now.Ticks.ToString() + Path.GetExtension(upload.FileName);
                string fupathh = Gc.UrlWebsite + "Upload/Images/TmpImage/" + name;
                string urlpath = context.Server.MapPath("~/Upload/Images/TmpImage");
                upload.SaveAs(Path.Combine(urlpath, name));
                context.Response.StatusCode = 200;
                context.Response.Write(fupathh);
            }
            catch { }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}