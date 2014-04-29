using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.HttpData.Core
{
    /// <summary>
    /// Summary description for Department
    /// </summary>
    public class Department : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            context.Response.Write("Hello World");
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