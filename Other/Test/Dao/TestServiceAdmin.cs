using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;
using Business.Core.Service;
using Util.Reflection;
using System.Collections.Generic;
using Database.Domain.Enums;

namespace Test.Dao
{
    /// <summary>
    /// 【测试】服务:系统管理员
    ///    Project:Core->Service
    ///    路径:Service->ServiceAdmin
    /// </summary>
    [TestClass]
    public class TestServiceAdmin
    {
        private const string AdminOriginal = "skygreen";
        private const string AdminNameUpdated = "pupu";
        private const string Password = "4008001666";
        private const int AdminID = 1;
        private const int DeletedID = 8;

        /// <summary>
        /// 系统管理员服务
        /// </summary>
        private static IServiceAdmin adminService;


        /// <summary>
        /// 初始化
        /// </summary>
        [TestInitialize]
        public void init()
        {
            if (adminService == null) adminService = new ServiceAdmin();
        }

        /// <summary>
        /// 保存系统管理员
        /// </summary>
        /// <param name="admin">需要保存的系统管理员信息</param>
        /// <returns></returns>
        [TestMethod]
        public void Save()
        {
            Admin admin = new Admin();
            admin.Username = AdminOriginal;
            admin.Realname = AdminOriginal;
            admin.Department_ID = 1;
            admin.Password = Password;
            admin.Seescope = EnumSeescope.All.ToString();
            admin.Roletype = EnumRoleType.Superadmin.ToString();
            admin.CommitTime = DateTime.Now;
            admin.UpdateTime = DateTime.Now;
            int ID=adminService.Save(admin);
            Console.WriteLine(admin.ID+","+ID);

            //Test 1
            Assert.IsNotNull(admin.ID);

            //Test 2
            Assert.AreEqual(admin.ID,ID);
        }

        /// <summary>
        /// 更新系统管理员
        /// </summary>
        /// <param name="admin">需要更新的系统管理员信息</param>
        /// <returns>是否保存成功</returns>
        [TestMethod]
        public void Update()
        {
            var adminToUpdate = adminService.GetByID(AdminID);
            if (adminToUpdate != null) adminToUpdate.Username = AdminNameUpdated;
            bool IsUpdated=adminService.Update(adminToUpdate);
            Assert.IsTrue(IsUpdated);
        }

        /// <summary>
        /// 保存或更新系统管理员
        /// </summary>
        /// <param name="admin">需保存或更新的系统管理员信息</param>
        /// <returns>是否保存或更新成功</returns>
        [TestMethod]
        public void SaveOrUpdate()
        {
            Admin admin = new Admin();
            admin.Username = AdminOriginal;
            admin.Realname = AdminOriginal;
            admin.Department_ID = 1;
            admin.Password = Password;
            bool IsSaved = adminService.SaveOrUpdate(admin);
            Assert.IsTrue(IsSaved);

            if (admin != null) admin.Username = AdminNameUpdated;
            bool IsUpdated = adminService.SaveOrUpdate(admin);
            Assert.IsTrue(IsUpdated);
        }

        /// <summary>
        /// 删除系统管理员
        /// </summary>
        /// <param name="admin">需要删除的系统管理员信息</param>
        /// <returns>是否删除成功</returns>
        [TestMethod]
        public void Delete()
        {
            //先默认插入10条Admin记录
            int ID=4;
            Admin admin;
            for (int i = 0; i < 1000; i++)
            {
                admin = new Admin();
                admin.Username = AdminOriginal + i;
                admin.Realname = AdminOriginal + i;
                admin.Department_ID = 1;
                admin.Password = Password;
                admin.LoginTimes = i;
                admin.Seescope = EnumSeescope.All.ToString();
                admin.Roletype = EnumRoleType.Superadmin.ToString();
                admin.CommitTime = DateTime.Now;
                admin.UpdateTime = DateTime.Now;
                ID = adminService.Save(admin);
            }
            admin=adminService.GetByID(ID - 2);
            bool IsDeleted = adminService.Delete(admin);
            Assert.IsTrue(IsDeleted);
            admin = adminService.GetByID(ID - 2);
            Assert.IsNull(admin);
        }

        /// <summary>
        /// 由标识删除指定ID数据对象
        /// </summary>
        /// <param name="ID">系统管理员标识</param>
        /// <returns>是否删除成功</returns>
        [TestMethod]
        public void DeleteByID()
        {
            Admin admin = adminService.GetByID(DeletedID);
            Assert.IsNotNull(admin);
            bool IsDeleted=adminService.DeleteByID(DeletedID);
            Assert.IsTrue(IsDeleted);
            admin = adminService.GetByID(DeletedID);
            Assert.IsNull(admin);
        }

        /// <summary>
        /// 根据主键删除多条记录
        /// </summary>
        /// <param name="ids">多个系统管理员标识的字符串:1,2,3,4</param>
        /// <returns>是否删除成功</returns>
        [TestMethod]
        public void DeleteByIDs()
        {
            string ids = "11,12,13,14";
            bool IsDeleted = adminService.DeleteByIDs(ids);
            Assert.IsTrue(IsDeleted);
        }

        /// <summary>
        /// 由标识判断指定ID系统管理员是否存在
        /// </summary>
        /// <param name="ID">系统管理员标识<></param>
        /// <returns></returns>
        [TestMethod]
        public void ExistByID()
        {
            bool IsExist = adminService.ExistByID(DeletedID);
            Assert.IsTrue(IsExist);
        }

        /// <summary>
        /// 判断符合条件的数据对象是否存在
        /// </summary>
        /// <param name="WhereClause">查询条件<></param>
        /// <returns></returns>
        [TestMethod]
        public void ExistBy()
        {
            bool IsExist = adminService.ExistBy("ID>1000");
            Assert.IsTrue(IsExist);
        }

        /// <summary>
        /// 根据表ID主键获取指定的系统管理员[ID对应的表列]
        /// </summary>
        /// <param name="ID">标识</param>
        /// <returns></returns>
        [TestMethod]
        public void GetByID()
        {
            Admin admin = adminService.GetByID(AdminID);
            UtilReflection.print_r(admin);
            Assert.IsNotNull(admin);
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
            List<Admin> admins = adminService.Get();
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
            Admin admin = adminService.GetOne();
            UtilReflection.print_r(admin);
            Assert.IsNotNull(admin);
        }

        /// <summary>
        /// 总计数
        /// </summary>
        /// <param name="WhereClause">查询条件<></param>
        /// <returns></returns>
        [TestMethod]
        public void Count()
        {
            int count=adminService.Count();
            Console.WriteLine(count);
            Assert.IsTrue(count>1);
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
            List<Admin> admins = adminService.QueryPage(1,10);
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
            List<Admin> admins = adminService.QueryPage(1, 10);
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
            List<Admin> admins = adminService.SqlExecute("select * from Admin where LoginTimes>900");
            Console.WriteLine("总计数:"+admins.Count);
            foreach (Admin admin in admins)
            {
                UtilReflection.print_r(admin);
            }

            Assert.IsTrue(admins.Count > 10);
        }
    }
}
