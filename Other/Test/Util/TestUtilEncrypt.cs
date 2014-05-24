using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Util.Common;

namespace Test.Util
{
    /// <summary>
    /// 测试加密算法
    /// </summary>
    [TestClass]
    public class TestUtilEncrypt
    {
        [TestMethod]
        public void TestMD5()
        {
            // 测试1:MD5加密-测试值:admin
            string md5Str = UtilEncrypt.MD5Encoding("admin");
            Assert.AreEqual("21232f297a57a5a743894a0e4a801fc3", md5Str);

            // 测试2:MD5加密-测试值:iloveu
            md5Str = UtilEncrypt.MD5Encoding("iloveu");
            Assert.AreEqual("edbd0effac3fcc98e725920a512881e0", md5Str);

            // 测试3:MD5加密加盐
            md5Str = UtilEncrypt.MD5Encoding("admin", "admin");
            Assert.AreEqual("ceb4f32325eda6142bd65215f4c0f371", md5Str);

            // 测试4:MD5加密16位
            md5Str = UtilEncrypt.MD5Encoding16Bit("iloveu");
            Assert.AreEqual("ac3fcc98e725920a", md5Str);

        }
    }
}
