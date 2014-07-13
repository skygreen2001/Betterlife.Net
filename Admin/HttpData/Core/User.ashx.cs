using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace AdminManage.HttpData.Core
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class User : BasicHandler, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            Init_Db();

            int pageSize = 15;
            string query = context.Request["query"];

            int start = 0;
            if (context.Request["start"] != null) start = int.Parse(context.Request["start"]);
            Dictionary<string, object> userDic = new Dictionary<string, object>();
            int totalCount = db.User.Where(e => e.Username.Contains(query)).Count();
            var users = db.User.Where(e => e.Username.Contains(query)).
                 OrderByDescending(p => p.ID).Skip(start).Take(pageSize);
            string result = "";
            if (users != null)
            {
                foreach (var user in users)
                {
                    user.User_ID = user.ID;
                }

                try
                {
                    userDic.Add("totalCount", totalCount);
                    userDic.Add("users", users);

                    //执行序列化
                    result = JsonConvert.SerializeObject(userDic, Formatting.Indented, new JsonSerializerSettings
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