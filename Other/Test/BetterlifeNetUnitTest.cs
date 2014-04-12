using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;
using System.Linq;
using Util.Util.Common;
using System.Reflection;
using Util.Reflection;

namespace Test
{
    [TestClass]
    public class BetterlifeNetUnitTest
    {
        private const string PersonOriginal = "John Doe";
        private const string PersonNameUpdated = "Updated Name";
        private const int PersonID = 1;

        private static BetterlifeNetEntities db = new BetterlifeNetEntities();

        /// <summary>
        /// 初始化
        /// </summary>
        [TestInitialize]
        public void init()
        {
        }

        /// <summary>
        /// 保存管理员
        /// </summary>
        [TestMethod]
        public void saveAdmin()
        {
            Admin admin = new Admin();
            admin.Username = PersonOriginal;
            admin.Department_ID = 1;
            admin.Password = "4008001666";
            //admin.ID = Guid.NewGuid();
            db.Admin.Add(admin);
            db.SaveChanges();

            // Test 1
            var personCount = (db.Admin.Select(p => p)).Count();
            Assert.AreEqual(4, personCount);

            //Test 2
            var newPersonFound = db.Admin.FirstOrDefault(
                person => person.Username == PersonOriginal);
            Assert.IsNotNull(newPersonFound);
        }

        /// <summary>
        /// 修改管理员
        /// </summary>
        [TestMethod]
        public void updateAdmin()
        {
            // Setup test
            var personToUpdate = db.Admin.FirstOrDefault(
                person => person.Username == PersonOriginal);

            if (personToUpdate != null) personToUpdate.Username = PersonNameUpdated;
            db.SaveChanges();

            // Test
            var updatedPerson = db.Admin.FirstOrDefault(
                person => person.Username == PersonNameUpdated);
            Assert.IsNotNull(updatedPerson);

            // Tear down test
            var personToRevert = db.Admin.FirstOrDefault(
                person => person.Username == PersonNameUpdated);

            if (personToRevert != null) personToRevert.Username = PersonOriginal;
            db.SaveChanges();
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        [TestMethod]
        public void deleteAdmin()
        {
            var toDeletes = db.Admin.Where(admin => admin.Username == PersonOriginal);
            foreach (var toDelete in toDeletes)
            {
                Console.WriteLine(toDelete.ID + toDelete.Username + ":" + toDelete.Updatetime);
                db.Admin.Remove(toDelete);
            }
            db.SaveChanges();
        }

        /// <summary>
        /// 根据编号获取管理员
        /// </summary>
        [TestMethod]
        public void get_by_id()
        {
            Admin admin = db.Admin.Single(e => e.ID.Equals(PersonID));//new Guid(PersonID)
            UtilReflection.print_r(admin);
            Assert.AreEqual("admin",admin.Username);
        }

        /// <summary>
        /// 计数管理员
        /// </summary>
        [TestMethod]
        public void countAdmin()
        {
            var count = (db.Admin.Select(p => p)).Count();
            Console.WriteLine(count);
            Assert.IsTrue(count > 0);
        }




    }
}
