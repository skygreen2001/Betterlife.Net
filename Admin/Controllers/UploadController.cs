using Admin.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class UploadController : Controller
    {
        //
        // GET: /Upload/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 上传Excel文件:系统管理员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult uploadAdmin(HttpPostedFileBase file)
        {
            JObject resultImport = ExtServiceAdminHandler.importAdmin();
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
            }
            string result = "";
            try
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };


                //执行序列化
                result = JsonConvert.SerializeObject(resultImport, Formatting.Indented, serializerSettings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Response.Write(result);
            Response.End();
            return null;
            //return View();
        }

    }
}
