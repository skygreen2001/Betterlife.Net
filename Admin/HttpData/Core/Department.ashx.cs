using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace Admin.HttpData.Core
{
    /// <summary>
    /// Summary description for Department
    /// </summary>
    public class DepartmentHandler : BasicHandler,IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pageSize=15;
            string query = context.Request["query"];

            int start = 0;
            if (context.Request["start"]!=null) start = int.Parse(context.Request["start"]);
            //int limit = pageSize;
            //if (context.Request["limit"] != null)
            //{
            //    limit = int.Parse(context.Request["limit"]);
            //    limit = start + limit - 1;
            //}
            Dictionary<string, object> departmentDic = new Dictionary<string, object>();
            int totalCount = db.Department.Where(e => e.Department_Name.Contains(query)).Count();
            var departments = db.Department.Where(e => e.Department_Name.Contains(query)).
                 OrderByDescending(p => p.ID).Skip(start).Take(pageSize);
            string result = "";
            if (departments != null)
            {
                foreach (var department in departments)
                {
                    department.Department_ID = department.ID;
                }

                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                departmentDic.Add("totalCount", totalCount);
                departmentDic.Add("departments", departments);
                try
                {
                    var serializerSettings = new JsonSerializerSettings { 
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                        ReferenceLoopHandling=ReferenceLoopHandling.Ignore
                    };

                    
                    //执行序列化
                    result = JsonConvert.SerializeObject(departmentDic, Formatting.Indented, serializerSettings);
                }catch(Exception ex){
                    Console.WriteLine(ex.Message);
                }
            }

            context.Response.Write(result);
        }
    }
}