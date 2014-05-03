using Ext.Direct;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Data;
using Administrator=Database.Admin;
using Util.Common;
using Util.Util.Common;
using Database.Domain.Enums;
using Database;
using System.Reflection;
using System.IO;
using Business;
using Util.DataType.Datatable;

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

        /// <summary>
        /// 保存系统管理员
        /// </summary>
        /// <param name="adminForm">系统管理员输入表单</param>
        /// <returns></returns>
        [DirectMethodForm]
        public JObject save(HttpRequest adminForm)
        {
            bool result = false;
            string msg  = "";
            if (adminForm != null)
            {
                string Username = adminForm["Username"];
                string admin_id = null;
                bool Flag = this.usernameHandler(Username, admin_id);
                if (Flag)
                {
                    msg = "该用户名已存在,请重新输入!";
                }
                else
                {

                    Administrator admin = new Administrator();
                    byte Roletype = Convert.ToByte(adminForm["Roletype"]);
                    byte Seescope = Convert.ToByte(adminForm["Seescope"]);
                    base.copyProperties(admin, adminForm);
                    try
                    {
                        admin.Logintimes = 1;
                        admin.Committime = DateTime.Now;
                        admin.Updatetime = DateTime.Now;
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

        /// <summary>
        /// 更新系统管理员
        /// </summary>
        /// <param name="adminForm">系统管理员输入表单</param>
        /// <returns></returns>
        [DirectMethodForm]
        public JObject update(HttpRequest adminForm)
        {
            bool result = false;
            string msg  = "";
            if (adminForm != null)
            {
                string id_str = adminForm["ID"];
                string Username = adminForm["Username"];
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
                        base.copyProperties(admin, adminForm);
                        admin.Updatetime = DateTime.Now;
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
        /// <param name="condition">
        /// 查询条件对象:
        ///     必须传递分页参数：start:分页开始数，默认从0开始
        ///     limit:分页查询数，默认10个。
        /// </param>
        /// <returns></returns>
        [DirectMethod]
        public ExtServiceAdminHandler queryPageAdmin(Dictionary<String, object> condition)
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

            string Username = "", Realname = "";
            if (condition.ContainsKey("Username")) Username = Convert.ToString(condition["Username"]);
            if (condition.ContainsKey("Realname")) Realname = Convert.ToString(condition["Realname"]);
            int rowCount = 0;//总行记录数
            rowCount = db.Admin.Where(e => e.Username.Contains(Username) &&
                                             e.Realname.Contains(Realname)).Count();

            var admins = db.Admin.Where(e => e.Username.Contains(Username) &&
                                             e.Realname.Contains(Realname)).
                OrderByDescending(p => p.ID).Skip(start).Take(pageCount);

            List<Administrator> listAdmins=admins.ToList<Administrator>();
            int i=1;
            foreach (Administrator row in listAdmins)
            {
                row.RoletypeShow = EnumRoleType.RoletypeShow(Convert.ToChar(row.Roletype));
                row.SeescopeShow = EnumSeescope.SeescopeShow(Convert.ToChar(row.Seescope));
                row.Department_Name = row.Department.Department_Name;
                this.Stores.Add(row);
                i++;
            }
            this.TotalCount = rowCount;
            this.Success = true;
            return this;
        }

        /// <summary>
        /// 根据主键删除数据对象:系统管理人员的多条数据记录
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
                    var toDeletes = db.Admin.Where(admin => admin.ID.Equals(ID));
                    foreach (var toDelete in toDeletes)
                    {
                        db.Admin.Remove(toDelete);
                    }
                }
            }
            db.SaveChanges();
            return new JObject(
                new JProperty("success", true)
            );
        }

        /// <summary>
        /// 导入:系统管理员
        /// </summary>
        public static JObject importAdmin()
        {

            return new JObject(
                new JProperty("success", true)
            );
        }

        /// <summary>
        /// 导出:系统管理员
        /// </summary>
        /// <param name="condition">查询条件对象</param>
        /// <returns></returns>
        [DirectMethod]
        public JObject exportAdmin(Dictionary<String, object> condition)
        {
            var admins = db.Admin.OrderByDescending(e => e.ID);
            string attachment_url ="";
            if (admins != null)
            {
                var query=admins.AsEnumerable();

                foreach (Administrator row in query)
                {
                    row.RoletypeShow = EnumRoleType.RoletypeShow(Convert.ToChar(row.Roletype));
                    row.SeescopeShow = EnumSeescope.SeescopeShow(Convert.ToChar(row.Seescope));
                    row.Department_Name = row.Department.Department_Name;
                }

                DataTable dt = UtilDataTable.ToDataTable(query); 
                dt.TableName = "Admin";
                UtilDataTable.DeleteColumns(dt, "Seescope", "Roletype", "Department_ID");
                Dictionary<string,string> dic=new Dictionary<string,string>();
                dic.Add("ID", "编号");
                dic.Add("Department_Name", "部门名称");
                dic.Add("Username", "用户名称");
                dic.Add("Password", "密码");
                dic.Add("Realname", "真实姓名");
                dic.Add("RoletypeShow", "扮演角色");
                dic.Add("SeescopeShow", "视野");
                dic.Add("LoginTimes", "登录次数");
                dic.Add("Committime", "创建时间");
                dic.Add("Updatetime", "更新时间");
                UtilDataTable.ReplaceColumnName(dt, dic);

                string fileName = UtilDateTime.NowS()+".xls";
                attachment_url=Gc.UploadUrl+"/attachment/admin/"+fileName;
                fileName=Path.Combine(Gc.UploadPath,"attachment", "admin", fileName);
                UtilExcelOle.DataTable2Sheet(fileName, dt);
            }
           
            return new JObject(
                new JProperty("success", true),
                new JProperty("data",attachment_url)
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
                    int id = UtilNumber.Parse(admin_id);
                    Administrator admin = db.Admin.Single(e => e.ID.Equals(id));
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