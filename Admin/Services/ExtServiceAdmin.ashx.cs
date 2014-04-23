using Ext.Direct;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Data;
using Administrator=Database.Admin;
using Util.Common;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core.Objects;
using Newtonsoft.Json;

namespace Admin.Services
{
    [DirectAction("ExtServiceAdmin")]
    public class ExtServiceAdminHandler : ExtServiceBasic
    {
        /// <summary>
        /// 返回Ext Direct Api的ProviderName
        /// </summary>
        public override string ProviderName
        {
            get
            {
                return "Ext.app.remote_admin";
            }
        }
        [DirectMethodForm]
        public JObject save(HttpRequest condition)
        {
            bool result = false;
            string msg  = "";
            if (condition != null)
            {
                string Username = condition["Username"];
                string admin_id = null;
                bool Flag = this.usernameHandler(Username, admin_id);
                if (Flag)
                {
                    msg = "该用户名已存在,请重新输入!";
                }
                else
                {

                    Administrator admin = new Administrator();
                    byte Roletype = Convert.ToByte(condition["Roletype"]);
                    byte Seescope = Convert.ToByte(condition["Seescope"]);
                    base.copyProperties(admin, condition);
                    try
                    {
                        admin.Department_ID = 1;
                        db.Admin.Add(admin); 
                        db.SaveChanges();
                        msg = "保存成功!";
                        result = true;
                    }
                    catch (Exception error)
                    {
                        msg = "操作失败:"+error.Message+",请重试!";
                    }
                }
            }
            return new JObject(
                new JProperty("success", result),
                new JProperty("msg", msg)
            );
        }

        [DirectMethodForm]
        public JObject update(HttpRequest condition)
        {
            bool result = false;
            string msg  = "";
            if (condition != null)
            {
                string id_str = condition["Admin_ID"];
                string Username = condition["Username"];
                bool Flag = this.usernameHandler(Username, id_str);
                if (Flag)
                {
                    msg = "该用户名已存在,请重新输入!";
                }
                else
                {
                    
                    try
                    {
                        int id = UtilNumber.Parse(id_str);
                        Administrator admin = db.Admin.Single(e => e.ID.Equals(id));
                        //admin.Realname = "ccc";
                        base.copyProperties(admin, condition);
                        db.SaveChanges();
                        msg = "保存成功!";
                        result = true;
                    }
                    catch (Exception error)
                    {
                        msg = "操作失败:" + error.Message + ",请重试!";
                    }
                }
            }
            return new JObject(
                new JProperty("success", result),
                new JProperty("msg", msg)
            );
        }

        /// <summary>
        /// 分页方法:系统管理员
        /// </summary>
        /// <see cref="http://diaosbook.com/Post/2012/9/21/linq-paging-in-entity-framework"/>
        /// <param name="condition"></param>
        /// <returns></returns>
        [DirectMethod]
        public ExtServiceAdminHandler queryPageAdmin(Dictionary<String, Object> condition)
        {
            int CurrentPage = 0;
            int limit = 10;
            if (condition.ContainsKey("limit"))limit=Convert.ToInt16(condition["limit"]);
            PageCount = limit;
            int start = 0;
            if (condition.ContainsKey("start"))start=Convert.ToInt16(condition["start"]);

            CurrentPage = start / PageCount;
            this.Stores = new List<Object>();
            condition.Remove("start");
            condition.Remove("limit");

            this.Stores.Clear();

            int RowCount = 0;//总行记录数
            RowCount = db.Admin.Count();

            var admins = db.Admin.OrderByDescending(p => p.ID).Skip(start).Take(PageCount);
            List<Administrator> listAdmins=admins.ToList<Administrator>();
            foreach (Administrator row in listAdmins)
            {
                //row.Department = null;
                this.Stores.Add(row);
                
            }
            this.TotalCount = RowCount;
            this.Success = true;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
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
                    var toDeletes = db.Admin.Where(admin => admin.ID.Equals(ID));
                    foreach (var toDelete in toDeletes)
                    {
                        db.Admin.Remove(toDelete);
                    }
                    db.SaveChanges();
                }
            }

            return new JObject(
                new JProperty("success", true)
            );
        }

        /// <summary>
        /// 用户名称是否使用
        /// </summary>
        /// <param name="Username">用户名</param>
        /// <param name="admin_id">用户ID</param>
        /// <returns>true:已使用 ;false:未使用</returns>
        private bool usernameHandler(string Username, string admin_id)
        {
            bool Used = true;
            var adminToUpdate=db.Admin.FirstOrDefault(person => person.Username == Username);
            if (adminToUpdate != null) 
            {
                Used = false;
            }
            else
            {
                if (!String.IsNullOrEmpty(admin_id))
                {
                    Administrator admin = db.Admin.Single(e => e.ID.Equals(admin_id));
                    if (admin != null && admin.Username == Username)
                    {
                        Used = false;
                    }
                }
                else
                {
                    Used = false;
                }
            }
            return Used;
        }
    }
}