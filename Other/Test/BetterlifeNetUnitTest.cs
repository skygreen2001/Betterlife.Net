using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;
using System.Linq;

namespace Test
{
    [TestClass]
    public class BetterlifeNetUnitTest
    {
        private const string PersonNew = "New Person";
        private const string PersonOriginal = "John Doe";
        private const string PersonNameUpdated = "Updated Name";

        private const string PersonNameUpdated = "Updated Name";

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
            admin.Username = PersonNew;
            admin.Password = "4008001666";
            admin.ID = Guid.NewGuid();
            db.Admins.Add(admin);
            db.SaveChanges();

            // Test 1
            var personCount = (db.Admins.Select(p => p)).Count();
            Assert.AreEqual(22, personCount);

            //Test 2
            var newPersonFound = db.Admins.FirstOrDefault(
                person => person.Username == PersonNew);
            Assert.IsNotNull(newPersonFound);
        }

        /// <summary>
        /// 修改管理员
        /// </summary>
        [TestMethod]
        public void updateAdmin()
        {
            // Setup test
            var personToUpdate = db.Admins.FirstOrDefault(
                person => person.Username == PersonOriginal);

            if (personToUpdate != null) personToUpdate.Username = PersonNameUpdated;
            db.SaveChanges();

            // Test
            var updatedPerson = db.Admins.FirstOrDefault(
                person => person.Username == PersonNameUpdated);
            Assert.IsNotNull(updatedPerson);

            // Tear down test
            var personToRevert = db.Admins.FirstOrDefault(
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
            var toDeletes = db.Admins.Where(admin => admin.Username == PersonOriginal);
            foreach (var toDelete in toDeletes)
            {
                Console.WriteLine(toDelete.ID + toDelete.Username + ":" + toDelete.Updatetime);
                db.Admins.Remove(toDelete);
            }
            db.SaveChanges();
        }

        /// <summary>
        /// 根据编号获取管理员
        /// </summary>
        public void get_by_id()
        {

        }

        /// <summary>
        /// 计数管理员
        /// </summary>
        [TestMethod]
        public void countAdmin()
        {
            var count = (db.Admins.Select(p => p)).Count();
            Assert.IsTrue(count > 0);
        }




    }
}
