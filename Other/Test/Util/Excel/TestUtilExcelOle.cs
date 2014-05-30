using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;
using System.Data;
using Util.DataType.Datatable;
using System.Collections.Generic;
using System.Linq;
using Administrator=Database.Admin;
using Database.Domain.Enums;
using Util.Common;
using Business;
using System.IO;

namespace Test.Util.Excel
{
    /// <summary>
    /// 测试:采用OLE方式读写Excel
    /// </summary>
    [TestClass]
    public class TestUtilExcelOle
    {
        private static BetterlifeNetEntities db;

        /// <summary>
        /// 初始化
        /// </summary>
        [TestInitialize]
        public void init()
        {
            Gc.init(EnumAppType.AppExe);
            db = new BetterlifeNetEntities();
        }

        /// <summary>
        /// 导出系统管理员
        /// </summary>
        [TestMethod]
        public void exportAdmin()
        {
            var admins = db.Admin.OrderByDescending(e => e.ID).Take(50);
            if (admins != null)
            {
                var query = admins.AsEnumerable();

                foreach (Administrator row in query)
                {
                    if (row.Roletype != null) row.RoletypeShow = EnumRoleType.RoletypeShow(Convert.ToChar(row.Roletype));
                    if (row.Seescope != null) row.SeescopeShow = EnumSeescope.SeescopeShow(Convert.ToChar(row.Seescope));
                    row.Department_Name = row.Department.Department_Name;
                }

                DataTable dt = UtilDataTable.ToDataTable(query);
                dt.TableName = "Admin";
                UtilDataTable.DeleteColumns(dt, "Seescope", "Roletype", "Department_ID");
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("ID", "编号");
                dic.Add("Department_Name", "部门名称");
                dic.Add("Username", "用户名称");
                dic.Add("Password", "密码");
                dic.Add("Realname", "真实姓名");
                dic.Add("RoletypeShow", "扮演角色");
                dic.Add("SeescopeShow", "视野");
                dic.Add("LoginTimes", "登录次数");
                dic.Add("CommitTime", "创建时间");
                dic.Add("UpdateTime", "更新时间");
                UtilDataTable.ReplaceColumnName(dt, dic);

                string fileName = UtilDateTime.NowS() + ".xls";
                fileName = Path.Combine(Gc.UploadPath, "attachment", "admin", fileName);
                UtilExcelOle.DataTable2Sheet(fileName, dt);
                Console.WriteLine("导出文件:"+fileName);
            }
        }

        /// <summary>
        /// 导入系统管理员
        /// </summary>
        public void importAdmin()
        {

        }
    }
}
