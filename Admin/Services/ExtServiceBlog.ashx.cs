using Business;
using Database;
using Ext.Direct;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Util.Common;
using Util.DataType.Datatable;

namespace Admin.Services
{
    /// <summary>
    /// 服务类:博客
    /// </summary>
    [DirectAction("ExtServiceBlog")]
    public class ExtServiceBlogHandler : ExtServiceBasic
    {
        /// <summary>
        /// 返回Ext Direct Api的ProviderName
        /// </summary>
        public override string ProviderName
        {
            get
            {
                return "Ext.app.remote_blog";
            }
        }

        /// <summary>
        /// 保存博客
        /// </summary>
        /// <param name="blogForm">系统管理员输入表单</param>
        /// <returns></returns>
        [DirectMethodForm]
        public JObject save(HttpRequest blogForm)
        {
            bool result = false;
            string msg = "";
            if (blogForm != null)
            {
                Blog blog = new Blog();
                base.copyProperties(blog, blogForm);
                try
                {
                    blog.CommitTime = DateTime.Now;
                    blog.UpdateTime = DateTime.Now;
                    db.Blog.Add(blog);
                    db.SaveChanges();
                    msg = "保存成功!";
                    result = true;
                }
                catch (Exception error)
                {
                    msg = "操作失败:" + error.Message + ",请重试!";
                }
            }
            return new JObject(
                new JProperty("success", result),
                new JProperty("msg", msg)
            );
        }

        /// <summary>
        /// 更新博客
        /// </summary>
        /// <param name="blogForm">系统管理员输入表单</param>
        /// <returns></returns>
        [DirectMethodForm]
        public JObject update(HttpRequest blogForm)
        {
            bool result = false;
            string msg = "";
            if (blogForm != null)
            {
                string id_str = blogForm["ID"];
                string Username = blogForm["Username"];

                try
                {
                    int id = UtilNumber.Parse(id_str);
                    Blog blog = db.Blog.Single(e => e.ID.Equals(id));
                    base.copyProperties(blog, blogForm);
                    blog.UpdateTime = DateTime.Now;
                    db.SaveChanges();
                    msg = "保存成功!";
                    result = true;
                }
                catch (Exception error)
                {
                    msg = "操作失败:" + error.Message + ",请重试!";
                }
            }
            return new JObject(
                new JProperty("success", result),
                new JProperty("msg", msg)
            );
        }

        /// <summary>
        /// 分页方法:博客
        /// </summary>
        /// <see cref="http://diaosbook.com/Post/2012/9/21/linq-paging-in-entity-framework"/>
        /// <param name="condition">
        /// 查询条件对象:
        ///     必须传递分页参数：start:分页开始数，默认从0开始
        ///     limit:分页查询数，默认10个。
        /// </param>
        /// <returns></returns>
        [DirectMethod]
        public ExtServiceBlogHandler queryPageBlog(Dictionary<String, object> condition)
        {
            int currentPage = 0;
            int start = 0, limit = 10;

            if (condition.ContainsKey("limit")) limit = Convert.ToInt16(condition["limit"]);
            if (condition.ContainsKey("start")) start = Convert.ToInt16(condition["start"]);
            UtilDictionary.Removes(condition, "start", "limit");

            pageCount = limit;
            currentPage = start / pageCount;
            this.Stores = new List<Object>();
            //this.Stores.Clear();

            int User_ID = 0; 
            string Blog_Name = "";
            if (condition.ContainsKey("User_ID")) User_ID = Convert.ToInt16(condition["User_ID"]);
            if (condition.ContainsKey("Blog_Name")) Blog_Name = Convert.ToString(condition["Blog_Name"]);
            int rowCount = 0;//总行记录数
            rowCount = db.Blog.Where(e=>e.Blog_Name.Contains(Blog_Name)).Count();

            var blogs = db.Blog.Where(e=>e.Blog_Name.Contains(Blog_Name)).
                OrderByDescending(p => p.ID).Skip(start).Take(pageCount);

            List<Blog> listBlogs = blogs.ToList<Blog>();
            int i = 1;
            foreach (Blog blog in listBlogs)
            {
                User user = db.User.Where(e => e.ID==blog.User_ID).SingleOrDefault();
                blog.Username = user.Username;
                this.Stores.Add(blog);
                i++;
            }
            this.TotalCount = rowCount;
            this.Success = true;
            return this;
        }

        /// <summary>
        /// 根据主键删除数据对象:博客的多条数据记录
        /// </summary>
        /// <param name="condition">数据对象编号。形式如下:1,2,3,4</param>
        /// <returns></returns>
        [DirectMethod]
        public JObject deleteByIds(string condition)
        {
            string[] pids = condition.Split(',');
            for (int i = 0; i < pids.Length; i++)
            {
                if (!String.IsNullOrEmpty(pids[i]))
                {
                    string id_str = pids[i];
                    int ID = UtilNumber.Parse(id_str);
                    var toDeletes = db.Blog.Where(blog => blog.ID.Equals(ID));
                    foreach (var toDelete in toDeletes)
                    {
                        db.Blog.Remove(toDelete);
                    }
                }
            }
            db.SaveChanges();
            return new JObject(
                new JProperty("success", true)
            );
        }

        /// <summary>
        /// 导入:博客
        /// </summary>
        public static JObject importBlog(string fileName)
        {
            //Excel导出入到DataTable
            DataTable dt = UtilExcelOle.ExcelToDataTableBySheet(fileName, "Admin");
            if (dt != null)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"标识","ID"},
                    {"用户名称","Username"},
                    {"博客标题","Blog_Name"},
                    {"博客内容","Blog_Content"},
                    {"创建时间","CommitTime"},
                    {"更新时间","UpdateTime"}
                };

                UtilDataTable.ReplaceColumnName(dt, dic);

                //循环插入数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Blog blog = new Blog();
                    UtilDataTable.ToObject(blog, dt.Rows[i], dt.Columns);
                    User user = db.User.Where(e => e.Username.Equals(blog.Username)).SingleOrDefault();
                    blog.User_ID = user.ID;
                    db.Blog.Add(blog);
                }
                db.SaveChanges();
            }
            return new JObject(
                new JProperty("success", true),
                new JProperty("data", true)
            );
        }

        /// <summary>
        /// 导出:博客
        /// </summary>
        /// <param name="condition">查询条件对象</param>
        /// <returns></returns>
        [DirectMethod]
        public JObject exportBlog(Dictionary<String, object> condition)
        {
            var blogs = db.Blog.OrderByDescending(e => e.ID);
            string attachment_url = "";
            if (blogs != null)
            {
                var query = blogs.AsEnumerable();

                foreach (Blog blog in query)
                {
                    blog.Username = blog.User.Username;
                }

                DataTable dt = UtilDataTable.ToDataTable(query);
                dt.TableName = "Admin";
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"ID", "标识"},
                    {"Username", "用户名称"},
                    {"Blog_Name", "博客标题"},
                    {"Blog_Content", "博客内容"},
                    {"CommitTime","创建时间"},
                    {"UpdateTime","更新时间"}
                };
                UtilDataTable.ReplaceColumnName(dt, dic);

                string fileName = "admin" + UtilDateTime.NowS() + ".xls";
                attachment_url = Gc.UploadUrl + "/attachment/admin/" + fileName;
                fileName = Path.Combine(Gc.UploadPath, "attachment", "admin", fileName);
                UtilExcelOle.DataTableToExcel(fileName, dt);
            }

            return new JObject(
                new JProperty("success", true),
                new JProperty("data", attachment_url)
            );
        }

    }
}