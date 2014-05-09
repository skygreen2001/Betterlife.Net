using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Util.Common;

namespace Portal.HttpData
{
    /// <summary>
    /// Summary description for survey
    /// </summary>
    public class Survey : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            List<string> showResult = new List<string>();
            showResult.Add("Javascript");
            showResult.Add("C#");
            showResult.Add("VB");
            showResult.Add("C++");
            showResult.Add("F");
            showResult.Add("JAVA");
            showResult.Add("PHP");
            showResult.Add("Object-C");
            showResult.Add("CSS");
            showResult.Add("HTML");
            int index=UtilNumber.Parse(UtilNumber.RandomNumber(1));
            string result=showResult.ElementAt(index);
            context.Response.Write(result);
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