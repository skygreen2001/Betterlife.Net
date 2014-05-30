using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;
using System.Linq;
using Util.Reflection;
using System.Collections.Generic;
using Util.Common;
using Database.Domain.Enums;

namespace Test.Dao
{
    /// <summary>
    /// 【测试】服务:采用Linq获取系统管理员数据
    /// 设置：主键ID类型为自增长数字类型
    /// </summary>
    [TestClass]
    public class TestLinqAdmin
    {
        private const string AdminOriginal = "skygreen";
        private const string AdminNameUpdated = "fly";
        private const string Password = "4008001666";
        private const int AdminID = 1;
        private const int DeletedID = 16;

        private static BetterlifeNetEntities db;

        /// <summary>
        /// 初始化
        /// </summary>
        [TestInitialize]
        public void init()
        {
            db = new BetterlifeNetEntities();
        }


        #region 基本函数测试
        /// <summary>
        /// 保存管理员
        /// </summary>
        [TestMethod]
        public void SaveAdmin()
        {
            Admin admin = new Admin();
            admin.Username = AdminOriginal;
            admin.Department_ID = 1;
            admin.Password = Password;
            admin.Seescope = EnumSeescope.All.ToString();
            admin.Roletype = EnumRoleType.Superadmin.ToString();
            admin.Committime = DateTime.Now;
            admin.Updatetime = DateTime.Now;
            //admin.ID = Guid.NewGuid();
            db.Admin.Add(admin);
            db.SaveChanges();

            // Test 1
            Assert.IsNotNull(admin.ID);

            //Test 2
            var newPersonFound = db.Admin.FirstOrDefault(
                person => person.Username == AdminOriginal);
            Assert.IsNotNull(newPersonFound);
        }

        /// <summary>
        /// 修改管理员
        /// </summary>
        [TestMethod]
        public void UpdateAdmin()
        {
            //获取需修改的系统管理员
            var adminToUpdate = db.Admin.FirstOrDefault(admin => admin.Username == AdminOriginal);
            if (adminToUpdate != null) adminToUpdate.Username = AdminNameUpdated;
            db.SaveChanges();

            // Test 1
            var updatedAdmin = db.Admin.FirstOrDefault(admin => admin.Username == AdminNameUpdated);
            Assert.IsNotNull(updatedAdmin);

            // T还原数据
            var adminToRevert = db.Admin.FirstOrDefault(
                person => person.Username == AdminNameUpdated);

            if (adminToRevert != null) adminToRevert.Username = AdminOriginal;
            db.SaveChanges();

        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        [TestMethod]
        public void DeleteAdmin()
        {            
            //先默认插入10条Admin记录
            Admin admin=new Admin();
            int Count = db.Admin.Count();
            for (int i = 0; i < 1000; i++)
            {
                admin = new Admin();
                admin.Username = AdminOriginal + i;
                admin.Department_ID = 1;
                admin.Password = Password;
                admin.Logintimes = i;
                admin.Seescope = EnumSeescope.All.ToString();
                admin.Roletype = EnumRoleType.Superadmin.ToString();
                admin.Committime = DateTime.Now;
                admin.Updatetime = DateTime.Now;
                db.Admin.Add(admin);
            }
            db.SaveChanges();
            Assert.IsTrue(Count == db.Admin.Count()-1000);
        }


        /// <summary>
        /// 由标识删除指定ID数据对象
        /// </summary>
        /// <param name="ID">系统管理员标识</param>
        /// <returns>是否删除成功</returns>
        [TestMethod]
        public void DeleteByID()
        {
            Admin admin = db.Admin.Single(e => e.ID.Equals(DeletedID));
            Assert.IsNotNull(admin);
            db.Admin.Remove(admin);
            db.SaveChanges();
            int Count = db.Admin.Count(e => e.ID.Equals(DeletedID));
            Assert.IsTrue(Count==0);
        }

        /// <summary>
        /// 根据主键删除多条记录
        /// </summary>
        /// <param name="ids">多个系统管理员标识的字符串:1,2,3,4</param>
        /// <returns>是否删除成功</returns>
        [TestMethod]
        public void DeleteByIDs()
        {
            string IDs = "131,132,133,134";

            string[] ID_Arr = IDs.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string Condition = "";
            //测试 1
            if ((ID_Arr != null) && (ID_Arr.Length > 0))
            {
                Condition = " ID=" + ID_Arr[0] + " ";
                for (int i = 1; i < ID_Arr.Length; i++)
                {
                    Condition += " OR ID=" + ID_Arr[i] + " ";
                }
            }
            int Count = db.Database.ExecuteSqlCommand("DELETE FROM Admin WHERE " + Condition);
            Assert.IsTrue(Count > 0);

            //测试 2
            IDs = "105,106,107,108";
            Count = db.Admin.Count();
            ID_Arr = IDs.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var ID in ID_Arr)
            {
                int iID = UtilNumber.Parse(ID);
                Admin toDelete = db.Admin.Single(e => e.ID.Equals(iID));
                Console.WriteLine(toDelete.ID + toDelete.Username + ":" + toDelete.Updatetime);
                db.Admin.Remove(toDelete);
            }
            db.SaveChanges();
            Assert.IsTrue(Count==db.Admin.Count()+4);
        }

        /// <summary>
        /// 由标识判断指定ID系统管理员是否存在
        /// </summary>
        /// <param name="ID">系统管理员标识<></param>
        /// <returns></returns>
        [TestMethod]
        public void ExistByID()
        {
            int Count=db.Admin.Count(e => e.ID.Equals(DeletedID));
            Assert.IsTrue(Count > 0);
        }

        /// <summary>
        /// 判断符合条件的数据对象是否存在
        /// </summary>
        /// <param name="WhereClause">查询条件<></param>
        /// <returns></returns>
        [TestMethod]
        public void ExistBy()
        {
            int Count=db.Admin.Where(e => e.ID > 1000).Count();
            Assert.IsTrue(Count>0);
        }

        /// <summary>
        /// 根据编号获取管理员
        /// </summary>
        [TestMethod]
        public void GetByIDAdmin()
        {
            Admin admin = db.Admin.SingleOrDefault(e => e.ID.Equals(AdminID));//new Guid(PersonID)
            if (admin != null)
            {
                UtilReflection.print_r(admin);
                Assert.AreEqual("admin", admin.Username);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        /// <summary>
        /// 计数管理员
        /// </summary>
        [TestMethod]
        public void CountAdmin()
        {
            var count = (db.Admin.Select(p => p)).Count();
            Console.WriteLine(count);
            Assert.IsTrue(count > 0);
        }

        /// <summary>
        /// 获取系统管理员列表
        /// </summary>
        /// <param name="WhereClause"></param>
        /// <param name="OrderBy"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        [TestMethod]
        public void Get()
        {
            List<Admin> admins = db.Admin.Take(10).ToList<Admin>();
            foreach (Admin admin in admins)
            {
                UtilReflection.print_r(admin);
            }
            Assert.AreEqual(10, admins.Count);
        }

        /// <summary>
        /// 查询得到单个系统管理员
        /// </summary>
        /// <param name="WhereClause"></param>
        /// <param name="OrderBy"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        [TestMethod]
        public void GetOne()
        {
            Admin admin = db.Admin.Select(e => e).FirstOrDefault();
            UtilReflection.print_r(admin);
            Assert.IsNotNull(admin);
        }

        /// <summary>
        /// 分页方法:查询系统管理员分页
        /// </summary>
        /// <param name="StartPoint">分页开始记录数</param>
        /// <param name="EndPoint">分页结束记录数</param>
        /// <param name="WhereClause">查询条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns></returns>
        [TestMethod]
        public void QueryPage()
        {
            //开始记录数
            int StartPoint = 1;
            //结束记录数
            int EndPoint=10;


            int From = StartPoint - 1;
            int PageSize = EndPoint - StartPoint + 1;
            List<Admin> admins = db.Admin
            .OrderByDescending(e=>e.ID)
            .Skip(From)
            .Take(PageSize).ToList<Admin>();
            foreach (Admin admin in admins)
            {
                UtilReflection.print_r(admin);
            }

            Assert.AreEqual(10, admins.Count);
        }

        /// <summary>
        /// 分页方法:根据当前页数和每页显示记录数查询系统管理员分页
        /// </summary>
        /// <param name="PageNo">当前页数</param>
        /// <param name="EndPoint">每页显示记录数</param>
        /// <param name="WhereClause">查询条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns></returns>
        [TestMethod]
        public void QueryPageByPageNo()
        {

            int PageNo = 1;
            int PageSize = 10;

            int StartPoint = (PageNo - 1) * PageSize;

            List<Admin> admins = db.Admin
            .OrderByDescending(e => e.ID)
            .Skip(StartPoint)
            .Take(PageSize).ToList<Admin>();
            foreach (Admin admin in admins)
            {
                UtilReflection.print_r(admin);
            }
            Assert.AreEqual(10, admins.Count);
        }

        /// <summary>
        /// 执行SQL语句,返回系统管理员列表
        /// </summary>
        /// <param name="Sqlstr">sql语句</param>
        /// <returns></returns>
        [TestMethod]
        public void SqlExecute()
        {
            List<Admin> admins = db.Admin.SqlQuery("select * from Admin where LoginTimes>900").ToList<Admin>();
            Console.WriteLine("总计数:" + admins.Count);
            foreach (Admin admin in admins)
            {
                UtilReflection.print_r(admin);
            }

            Assert.IsTrue(admins.Count > 10);
        }
        #endregion

        #region  统计
        /// <summary>
        /// 登录次数最小值
        /// </summary>
        [TestMethod]
        public void min()
        {
            int? min = db.Admin.Min(a => a.Logintimes);
            Console.WriteLine(min);
            Assert.IsTrue(min == 0);
        }

        /// <summary>
        /// 登录次数最大值
        /// </summary>
        [TestMethod]
        public void max()
        {
            int? max = db.Admin.Max(a => a.Logintimes);
            Console.WriteLine(max);
            Assert.IsTrue(max > 0);
        }

        /// <summary>
        /// 登录次数总和
        /// </summary>
        [TestMethod]
        public void sum()
        {
            int? sum = db.Admin.Where(e=>e.ID<100).Sum(a => a.Logintimes);
            Console.WriteLine(sum);
            Assert.IsTrue(sum > 0);
        }
        #endregion

    }
}
