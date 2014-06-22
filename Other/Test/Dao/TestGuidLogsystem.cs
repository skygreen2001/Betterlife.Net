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
    /// 【测试】服务:采用Linq获取系统日志数据
    /// 设置：主键ID类型为GUID
    /// </summary>
    [TestClass]
    public class TestGuidLogsystem
    {
        private static BetterlifeNetEntities db;
        private const string originalMessage = "error for test";
        private const string updateMessage = "111";
        private const string originalLogID = "1ED0F702-DBE3-4FBB-B762-0A3418D89C67";

        private readonly string[] originalLogIDs = new string[]{
            "E63D5EE7-108B-4974-BF91-01013DCAC380",
            "C6840802-9C35-4A84-A5F9-0413942E39F4",
            "57FAD0C1-0D34-4150-9D0C-0436E8B5CF16",
            "2219427A-103B-42EC-945B-03D6E7074AD5",
            "5682C4EA-9239-42E6-91CF-0715C36F0AF9",
            "1ED0F702-DBE3-4FBB-B762-0A3418D89C67",
            "BCD96A0E-84BB-4DD0-9A4C-0F82D1261AEC",
            "A848480C-65DB-4E3B-90BB-0FB7422DAD5A",
            "70151E19-4C71-432E-9646-1000AC209FC4",
            "011C4262-08CD-4A1D-AF70-10DFE1A262BA"};

        /// <summary>
        /// 初始化
        /// </summary>
        [TestInitialize]
        public void init()
        {
            db = DatabaseCenter.Instance();
        }


        #region 基本函数测试
        /// <summary>
        /// 保存系统日志
        /// </summary>
        [TestMethod]
        public void SaveLogsystem()
        {
            Logsystem logsystem = new Logsystem();
            logsystem.ID = new Guid(originalLogID);
            logsystem.Message = originalMessage;
            logsystem.Ident = "4";
            logsystem.Priority = "4";
            logsystem.Logtime = DateTime.Now;
            db.Logsystem.Add(logsystem);
            db.SaveChanges();

            // Test 1
            Assert.IsNotNull(logsystem.ID);

            //Test 2
            var newFound = db.Logsystem.Find(logsystem.ID);
            Assert.IsNotNull(newFound);
        }

        /// <summary>
        /// 修改系统日志
        /// </summary>
        [TestMethod]
        public void UpdateLogsystem()
        {
            Guid logID = new Guid(originalLogID);
            //获取需修改的系统管理员
            var logToUpdate = db.Logsystem.Find(logID);
            if (logToUpdate != null) logToUpdate.Message = updateMessage;
            db.SaveChanges();

            // Test 1
            var updatedLog = db.Logsystem.Find(logID);
            Assert.IsTrue(updatedLog.Message == updateMessage);

            // T还原数据
            var logToRevert = db.Logsystem.Find(logID);

            if (logToRevert != null) logToRevert.Message = originalMessage;
            db.SaveChanges();

        }

        /// <summary>
        /// 插入测试数据
        /// </summary>
        [TestMethod]
        public void InsertTestLogSystem()
        {
            //先默认插入10条Logsystem记录
            Logsystem logsystem = new Logsystem();
            int Count = db.Logsystem.Count();
            for (int i = 0; i < 10; i++)
            {
                logsystem = new Logsystem();
                logsystem.ID = new Guid(originalLogIDs[i]);
                logsystem.Message = originalMessage;
                logsystem.Ident = "4";
                logsystem.Priority = "4";
                logsystem.Logtime = DateTime.Now;
                db.Logsystem.Add(logsystem);
            }
            for (int i = 0; i < 90; i++)
            {
                logsystem = new Logsystem();
                logsystem.ID = Guid.NewGuid();
                logsystem.Message = originalMessage;
                logsystem.Ident = "4";
                logsystem.Priority = "4";
                logsystem.Logtime = DateTime.Now;
                db.Logsystem.Add(logsystem);
            }
            db.SaveChanges();
            Assert.IsTrue(Count == db.Logsystem.Count() - 100);
        }

        /// <summary>
        /// 由标识删除指定ID数据对象
        /// </summary>
        /// <param name="ID">系统日志标示</param>
        /// <returns>是否删除成功</returns>
        [TestMethod]
        public void DeleteByID()
        {
            Guid logID = new Guid(originalLogIDs[0]);

            Logsystem log = db.Logsystem.Find(logID);
            Assert.IsNotNull(log);
            db.Logsystem.Remove(log);
            db.SaveChanges();
            int Count = db.Logsystem.Count(e => e.ID.Equals(logID));
            Assert.IsTrue(Count == 0);
        }

        /// <summary>
        /// 根据主键删除多条记录
        /// </summary>
        /// <param name="ids">多个系统日志标识的字符串:从数据库中获取第100，101,102,103条数据</param>
        /// <returns>是否删除成功</returns>
        [TestMethod]
        public void DeleteByIDs()
        {
            //获取第1，2,3,4条数据的GUID
            string IDs = originalLogIDs[1] + "," + originalLogIDs[2] + ","
                + originalLogIDs[3] + "," + originalLogIDs[4];
            string[] ID_Arr = IDs.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string Condition = "";
            //测试 1
            if ((ID_Arr != null) && (ID_Arr.Length > 0))
            {
                Condition = " ID='" + ID_Arr[0] + "' ";
                for (int i = 1; i < ID_Arr.Length; i++)
                {
                    Condition += " OR ID='" + ID_Arr[i] + "' ";
                }
            }
            int Count = db.Database.ExecuteSqlCommand("DELETE FROM Logsystem WHERE " + Condition);
            Assert.IsTrue(Count > 0);
        }

        
        /// <summary>
        /// 删除系统日志
        /// </summary>
        [TestMethod]
        public void DeleteLogSystem()
        {
            //测试 2 获取第7，8条数据的GUID
            string IDs = originalLogIDs[7] + "," + originalLogIDs[8];
            int Count = db.Logsystem.Count();
            string[] ID_Arr = IDs.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var ID in ID_Arr)
            {
                Logsystem toDelete = db.Logsystem.Find(ID);
                Console.WriteLine(toDelete.ID + toDelete.Message + ":" + toDelete.Logtime);
                db.Logsystem.Remove(toDelete);
            }
            db.SaveChanges();
            Assert.IsTrue(Count == db.Logsystem.Count() + 2);
        }

        /// <summary>
        /// 由标识判断指定ID系统日志是否存在
        /// </summary>
        /// <param name="ID">系统日志标识<></param>
        /// <returns></returns>
        [TestMethod]
        public void ExistByID()
        {
            Guid logID = new Guid(originalLogIDs[9]);
            int Count = db.Logsystem.Count(e => e.ID.Equals(logID));
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
            int Count = db.Logsystem.Where(e => e.Logtime < DateTime.Now).Count();
            Assert.IsTrue(Count > 0);
        }

        /// <summary>
        /// 根据编号获取系统日志
        /// </summary>
        [TestMethod]
        public void GetByIDLogSystem()
        {
            Guid logID = new Guid(originalLogID);
            Logsystem log = db.Logsystem.Find(logID);
            if (log != null)
            {
                UtilReflection.print_r(log);
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        /// <summary>
        /// 计数系统日志
        /// </summary>
        [TestMethod]
        public void CountLogSystem()
        {
            var count = (db.Logsystem.Select(p => p)).Count();
            Console.WriteLine(count);
            Assert.IsTrue(count > 0);
        }

        /// <summary>
        /// 获取系统日志列表
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Get()
        {
            List<Logsystem> logs = db.Logsystem.Take(10).ToList<Logsystem>();
            foreach (Logsystem log in logs)
            {
                UtilReflection.print_r(log);
            }
            Assert.AreEqual(10, logs.Count);
        }

        /// <summary>
        /// 查询得到单个系统日志
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void GetOne()
        {
            Logsystem log = db.Logsystem.Select(e => e).FirstOrDefault();
            UtilReflection.print_r(log);
            Assert.IsNotNull(log);
        }

        /// <summary>
        /// 分页方法:查询系统日志分页
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
            int EndPoint = 10;


            int From = StartPoint - 1;
            int PageSize = EndPoint - StartPoint + 1;
            List<Logsystem> logs = db.Logsystem
            .OrderByDescending(e => e.Logtime)
            .Skip(From)
            .Take(PageSize).ToList<Logsystem>();
            foreach (Logsystem log in logs)
            {
                UtilReflection.print_r(log);
            }

            Assert.AreEqual(10, logs.Count);
        }

        /// <summary>
        /// 分页方法:根据当前页数和每页显示记录数查询系统日志分页
        /// </summary>
        /// <param name="PageNo">当前页数</param>
        /// <param name="EndPoint">每页显示记录数</param>
        /// <param name="WhereClause">查询条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns></returns>
        [TestMethod]
        public void QueryPageByPageNo()
        {

            int PageNo = 3;
            int PageSize = 10;

            int StartPoint = (PageNo - 1) * PageSize;

            List<Logsystem> logs = db.Logsystem
            .OrderByDescending(e => e.Logtime)
            .Skip(StartPoint)
            .Take(PageSize).ToList<Logsystem>();
            foreach (Logsystem log in logs)
            {
                UtilReflection.print_r(log);
            }
            Assert.AreEqual(10, logs.Count);
        }

        /// <summary>
        /// 执行SQL语句,返回系统日志列表
        /// </summary>
        /// <param name="Sqlstr">sql语句</param>
        /// <returns></returns>
        [TestMethod]
        public void SqlExecute()
        {
            List<Logsystem> logs = db.Logsystem.SqlQuery("select * from Logsystem where Logtime<getdate()").ToList<Logsystem>();
            Console.WriteLine("总计数:" + logs.Count);
            foreach (Logsystem log in logs)
            {
                UtilReflection.print_r(log);
            }

            Assert.IsTrue(logs.Count > 10);
        }

        #endregion
    }
}
