using Business;
using Business.Core.Service;
using Database;
using Database.Domain.Enums;
using Ext.Direct;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Util.Common;
using Util.DataType.Datatable;
using Administrator = Database.Admin;

namespace Admin.Services
{
    /// <summary>
    /// 服务类:系统管理人员
    /// </summary>
    [DirectAction("ExtServiceAdmin")]
    public class ExtServiceAdmin : ExtServiceBasic
    {
        /// <summary>
        /// 系统管理员服务
        /// </summary>
        private static IServiceAdmin adminService;

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
                bool Flag = IsUsernameExist(Username, admin_id);
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
                        admin.CommitTime = DateTime.Now;
                        admin.UpdateTime = DateTime.Now;
                        admin.LoginTimes = 1;
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
                string admin_id = adminForm["ID"];
                string Username = adminForm["Username"];
                bool Flag = this.IsUsernameExist(Username, admin_id);
                if (Flag)
                {
                    msg = "该用户名已存在,请重新输入!";
                }
                else
                {
                    try
                    {
                        int id = UtilNumber.Parse(admin_id);
                        Administrator admin = db.Admin.Single(e => e.ID.Equals(id));
                        base.copyProperties(admin, adminForm);
                        admin.UpdateTime = DateTime.Now;
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
        public ExtServiceAdmin queryPageAdmin(Dictionary<String, object> condition)
        {
            int currentPage = 0;
            int start = 0, limit = 10;

            if (condition.ContainsKey("limit")) limit = Convert.ToInt16(condition["limit"]);
            if (condition.ContainsKey("start")) start = Convert.ToInt16(condition["start"]);
            UtilDictionary.Removes(condition, "start", "limit");

            pageCount = limit;
            currentPage = start / pageCount;
            this.Stores = new List<Object>();

            string Username = "", Realname = "";
            if (condition.ContainsKey("Username")) Username = Convert.ToString(condition["Username"]);
            if (condition.ContainsKey("Realname")) Realname = Convert.ToString(condition["Realname"]);
            int rowCount = 0;//总行记录数
            if (adminService == null) adminService = new ServiceAdmin();
            adminService.Count()
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
        /// 导入:系统管理员
        /// </summary>
        public static JObject importAdmin(string fileName)
        {
            //Excel导出入到DataTable
            DataTable dt = UtilExcelOle.ExcelToDataTableBySheet(fileName,"Admin");
            if (dt != null)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"编号","ID"},
                    {"部门名称","Department_Name"},
                    {"用户名称","Username"},
                    {"密码","Password"},
                    {"真实姓名","Realname"},
                    {"扮演角色","RoletypeShow"},
                    {"视野","SeescopeShow"},
                    {"登录次数","LoginTimes"},
                    {"创建时间","CommitTime"},
                    {"更新时间","UpdateTime"}
                };

                UtilDataTable.ReplaceColumnName(dt, dic);

                //检查Excel中是否有用户名重复数据
                if (UtilDataTable.HasRepeat(dt, "Username"))
                {
                    Console.WriteLine("Excel中有重复用户名");
                    return new JObject(
                        new JProperty("success", true),
                        new JProperty("data", false)
                    );
                }
                //循环插入数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Administrator admin = new Administrator();
                    UtilDataTable.ToObject(admin, dt.Rows[i], dt.Columns);
                    Department dep = db.Department.Where(e => e.Department_Name.Equals(admin.Department_Name)).SingleOrDefault();
                    admin.Department_ID = dep.ID;
                    admin.Seescope = EnumSeescope.SeescopeByShow(admin.SeescopeShow);
                    admin.Roletype = EnumRoleType.RoletypeByShow(admin.RoletypeShow);
                    db.Admin.Add(admin);
                }
                db.SaveChanges();
            }
            return new JObject(
                new JProperty("success", true),
                new JProperty("data",true)
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
                    if (row.Roletype != null) row.RoletypeShow = EnumRoleType.RoletypeShow(Convert.ToChar(row.Roletype));
                    if (row.Seescope != null) row.SeescopeShow = EnumSeescope.SeescopeShow(Convert.ToChar(row.Seescope));
                    row.Department_Name = row.Department.Department_Name;
                }

                DataTable dt = UtilDataTable.ToDataTable(query); 
                dt.TableName = "Admin";
                UtilDataTable.DeleteColumns(dt, "Seescope", "Roletype", "Department_ID");
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"ID","编号"},
                    {"Department_Name","部门名称"},
                    {"Username","用户名称"},
                    {"Password","密码"},
                    {"Realname","真实姓名"},
                    {"RoletypeShow","扮演角色"},
                    {"SeescopeShow","视野"},
                    {"LoginTimes","登录次数"},
                    {"CommitTime","创建时间"},
                    {"UpdateTime","更新时间"}
                };
                UtilDataTable.ReplaceColumnName(dt, dic);

                string fileName = "admin"+UtilDateTime.NowS()+".xls";
                attachment_url=Gc.UploadUrl+"/attachment/admin/"+fileName;
                fileName=Path.Combine(Gc.UploadPath,"attachment", "admin", fileName);
                UtilExcelOle.DataTableToExcel(fileName, dt);
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
        private bool IsUsernameExist(string Username, string admin_id)
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