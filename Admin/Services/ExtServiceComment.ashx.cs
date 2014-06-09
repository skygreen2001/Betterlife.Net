﻿using Business;
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
    /// 服务类:评论
    /// </summary>
   [DirectAction("ExtServiceComment")]
    public class ExtServiceCommentHandler : ExtServiceBasic
    {
        /// <summary>
        /// 返回Ext Direct Api的ProviderName
        /// </summary>
        public override string ProviderName
        {
            get
            {
                return "Ext.app.remote_comment";
            }
        }

        /// <summary>
        /// 保存评论
        /// </summary>
        /// <param name="commentForm">评论输入表单</param>
        /// <returns></returns>
        [DirectMethodForm]
        public JObject save(HttpRequest commentForm)
        {
            bool result = false;
            string msg = "";
            if (commentForm != null)
            {
                Comment comment = new Comment();
                base.copyProperties(comment, commentForm);
                try
                {
                    comment.CommitTime = DateTime.Now;
                    comment.UpdateTime = DateTime.Now;
                    db.Comment.Add(comment);
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
        /// 更新评论
        /// </summary>
        /// <param name="commentForm">评论输入表单</param>
        /// <returns></returns>
        [DirectMethodForm]
        public JObject update(HttpRequest commentForm)
        {
            bool result = false;
            string msg = "";
            if (commentForm != null)
            {
                string id_str = commentForm["ID"];
                string Username = commentForm["Username"];

                try
                {
                    int id = UtilNumber.Parse(id_str);
                    Comment comment = db.Comment.Single(e => e.ID.Equals(id));
                    base.copyProperties(comment, commentForm);
                    comment.UpdateTime = DateTime.Now;
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
        /// 分页方法:评论
        /// </summary>
        /// <see cref="http://diaosbook.com/Post/2012/9/21/linq-paging-in-entity-framework"/>
        /// <param name="condition">
        /// 查询条件对象:
        ///     必须传递分页参数：start:分页开始数，默认从0开始
        ///     limit:分页查询数，默认10个。
        /// </param>
        /// <returns></returns>
        [DirectMethod]
        public ExtServiceCommentHandler queryPageComment(Dictionary<String, object> condition)
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

            int Blog_ID = 0;
            if (condition.ContainsKey("Blog_ID")) Blog_ID = Convert.ToInt16(condition["Blog_ID"]);
            int rowCount = 0;//总行记录数
            rowCount = db.Comment.Where(e => e.Blog_ID == Blog_ID).Count();

            var comments = db.Comment.Where(e => e.Blog_ID == Blog_ID).
                OrderByDescending(p => p.ID).Skip(start).Take(pageCount);

            List<Comment> listComments = comments.ToList<Comment>();
            int i = 1;
            foreach (Comment comment in listComments)
            {
                comment.Username = comment.User.Username;
                comment.Blog_Name = comment.Blog.Blog_Name;
                comment.Content = comment.Comment1;
                this.Stores.Add(comment);
                i++;
            }
            this.TotalCount = rowCount;
            this.Success = true;
            return this;
        }

        /// <summary>
        /// 根据主键删除数据对象:评论的多条数据记录
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
                    var toDeletes = db.Comment.Where(comment => comment.ID.Equals(ID));
                    foreach (var toDelete in toDeletes)
                    {
                        db.Comment.Remove(toDelete);
                    }
                }
            }
            db.SaveChanges();
            return new JObject(
                new JProperty("success", true)
            );
        }

        /// <summary>
        /// 导入:评论
        /// </summary>
        public static JObject importComment(string fileName)
        {
            //Excel导出入到DataTable
            DataTable dt = UtilExcelOle.ExcelToDataTableBySheet(fileName, "Admin");
            if (dt != null)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"标识","ID"},
                    {"评论者","Username"},
                    {"评论","Content"},
                    {"博客标题","Blog_Name"},
                    {"创建时间","CommitTime"},
                    {"更新时间","UpdateTime"}
                };
                UtilDataTable.ReplaceColumnName(dt, dic);

                //循环插入数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Comment comment = new Comment();
                    UtilDataTable.ToObject(comment, dt.Rows[i], dt.Columns);

                    User user = db.User.Where(e => e.Username.Equals(comment.Username)).SingleOrDefault();
                    comment.User_ID = user.ID;
                    comment.Comment1 = comment.Content;
                    Blog blog = db.Blog.Where(e => e.Blog_Name.Equals(comment.Blog_Name)).SingleOrDefault();
                    comment.Blog_ID = blog.ID;
                    db.Comment.Add(comment);
                }
                db.SaveChanges();
            }
            return new JObject(
                new JProperty("success", true),
                new JProperty("data", true)
            );
        }

        /// <summary>
        /// 导出:评论
        /// </summary>
        /// <param name="condition">查询条件对象</param>
        /// <returns></returns>
        [DirectMethod]
        public JObject exportComment(Dictionary<String, object> condition)
        {
            var comments = db.Comment.OrderByDescending(e => e.ID);
            string attachment_url = "";
            if (comments != null)
            {
                var query = comments.AsEnumerable();

                foreach (Comment comment in query)
                {
                    comment.Username = comment.User.Username;
                    comment.Blog_Name = comment.Blog.Blog_Name;
                    comment.Content = comment.Comment1;
                }

                DataTable dt = UtilDataTable.ToDataTable(query);
                dt.TableName = "Admin";
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"ID", "标识"},
                    {"Username", "评论者"},
                    {"Content", "评论"},
                    {"Blog_Name", "博客标题"},
                    {"CommitTime","创建时间"},
                    {"UpdateTime","更新时间"}
                };
                UtilDataTable.ReplaceColumnName(dt, dic);

                string fileName = "comment" + UtilDateTime.NowS() + ".xls";
                attachment_url = Gc.UploadUrl + "/attachment/comment/" + fileName;
                fileName = Path.Combine(Gc.UploadPath, "attachment", "comment", fileName);
                UtilExcelOle.DataTableToExcel(fileName, dt);
            }

            return new JObject(
                new JProperty("success", true),
                new JProperty("data", attachment_url)
            );
        }
    }
}