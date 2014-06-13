using AdminManage.Services;
using Business;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Util.Common;

namespace AdminManage.Controllers
{
    /// <summary>
    /// 上传控制器:后台所有业务上传文件功能
    /// </summary>
    public class UploadController : Controller
    {
        // GET: /Upload/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 上传Excel文件:系统管理员
        /// </summary>
        /// <returns></returns>
        // POST: /Upload/UploadAdmin/
        [HttpPost]
        public ActionResult UploadAdmin(FormCollection form)
        {
            if (Request.Files.Count > 0){
                HttpPostedFileBase file = Request.Files[0];
                string fileName = Path.Combine(Gc.UploadPath, "attachment", "admin", "admin" + UtilDateTime.NowS() + ".xls");
                file.SaveAs(fileName);

                JObject resultJ = ExtServiceAdmin.importAdmin(fileName);
                string result = JsonConvert.SerializeObject(resultJ);
                Response.Write(result);
            }else{
                Response.Write("{'success':false,'data':'上传文件不能为空'}");
            }

            return null;
        }

    }
}
