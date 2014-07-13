using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminManage.HttpData.Core
{
    /// <summary>
    /// Summary description for Department
    /// </summary>
    public class DepartmentHandler : BasicHandler,IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain"; 
            Init_Db();
            
            int pageSize=15;
            string query = context.Request["query"];

            int start = 0;
            if (context.Request["start"]!=null) start = int.Parse(context.Request["start"]);
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

                try
                {
                    departmentDic.Add("totalCount", totalCount);
                    departmentDic.Add("departments", departments);
                    result = JsonConvert.SerializeObject(departmentDic, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            context.Response.Write(result);
        }
    }
}