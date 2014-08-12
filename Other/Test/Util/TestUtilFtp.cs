using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Util.Common;
using System.Net;
using System.IO;

namespace Test.Util
{
    [TestClass]
    public class TestUtilFtp
    {
 
        [TestMethod]
        public void TestUploadToRemoteServer()
        {
            //UtilFtp.UploadToRemoteServer("C:\\Users\\k\\Desktop\\d\\paruru.jpg");
            //UtilFtp.UpMakeDirectory("ftp://210.51.51.164:213/img/");   
            //UtilFtp.CheckDirExist("/img/cc");

            //UtilFtp ftp = new UtilFtp("ftp://210.51.51.164", 213, "administrator", "5ikhABCD");
            //ftp.checkDir("/test/sdf/ddd/we/a.jpg");
        }

        [TestMethod]
        public void TestWebDav()
        {
            string lstrWebUrl = "http://192.168.1.101/paruru.jpg"; //服务器上将要存的文件名
            string lstrLocalFile = "C:\\Users\\k\\Desktop\\d\\paruru.jpg"; //原文件
            string lstrUserName = "pupu";
            string lstrPassword = "123.com";

            int i = UploadWebDavFile(lstrWebUrl, lstrLocalFile, lstrUserName, lstrPassword);
            Assert.AreEqual(i, 1);
        }

        public int UploadWebDavFile(string _WebFileUrl, string _LocalFile, string _UserName, string _Password)
        {
            try
            {
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)WebRequest.Create(_WebFileUrl);  //Http和服务器交互
                req.Credentials = new NetworkCredential(_UserName, _Password); //验证密码
                req.PreAuthenticate = true;
                req.Method = "PUT";//采用PUT方式
                req.AllowWriteStreamBuffering = true;

                Stream reqStream = req.GetRequestStream();
                FileStream rdm = new FileStream(_LocalFile, FileMode.Open); //打开本地文件流

                byte[] inData = new byte[4096];
                int byteRead = rdm.Read(inData, 0, inData.Length);  //二进制读文件
                while (byteRead > 0)
                {
                    reqStream.Write(inData, 0, byteRead); //响应流写入
                    byteRead = rdm.Read(inData, 0, inData.Length);
                }
                rdm.Close();
                reqStream.Close();

                req.GetResponse(); //提交
            }
            catch
            {
                return 0;
            }
            return 1; //正确返回
        }

    }
}
