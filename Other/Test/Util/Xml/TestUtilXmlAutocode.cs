using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using Util.Common;
using Tools.AutoCode.Prepare;

namespace Test.Util.Xml
{
    /// <summary>
    /// Summary description for TestUtilXmlAutocode
    /// </summary>
    [TestClass]
    public class TestUtilXmlAutocode
    {
        public TestUtilXmlAutocode()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// 生成表示层类
        /// </summary>
        public AutoCodeConfig autoconfig = new AutoCodeConfig();

        [TestMethod]
        public void TestAutoCodeConfigXml()
        {
            autoconfig.Run();
        }

        /// <summary>
        /// 自动生成代码配置的生成代码模板
        /// </summary>
        [TestMethod]
        public void TestXmlAutoCodeModel()
        {
            string App_Dir = Directory.GetCurrentDirectory();
            App_Dir = App_Dir + Path.DirectorySeparatorChar + "AutoCode" + Path.DirectorySeparatorChar + "Model" + Path.DirectorySeparatorChar;
            string Save_Dir = App_Dir + "autocode.config.xml"; 
            if (!Directory.Exists(Save_Dir)) UtilFile.CreateDir(Save_Dir);

            XElement xElement = new XElement(
                new XElement("classes",
                    new XElement("class",
                        new XElement("conditions",
                            new XElement("condition", "User_ID", new XAttribute("relation_class", "User"),new XAttribute("show_name", "Username")),
                            new XElement("condition", "Blog_Name"),
                            new XElement("condition", "Content")
                        ),
                        new XElement("relationShows",
                            new XElement("show", "Username", new XAttribute("local_key", "User"), new XAttribute("relation_class", "User"))
                        )
                    )
                )
            );
          
            xElement.Add(new XElement("class",
                        new XElement("conditions",
                            new XElement("condition", "User_ID1", new XAttribute("relation_class", "User"), new XAttribute("show_name", "Username")),
                            new XElement("condition", "Blog_Name1"),
                            new XElement("condition", "Content1")
                        ),
                        new XElement("relationShows1",
                            new XElement("show1", "Username", new XAttribute("local_key", "User"), new XAttribute("relation_class", "User"))
                        )
                    ));

            //需要指定编码格式，否则在读取时会抛：根级别上的数据无效。 第 1 行 位置 1异常
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.Indent = true;
            XmlWriter xw = XmlWriter.Create(Save_Dir, settings);
            xElement.Save(xw);
            //写入文件
            xw.Flush();
            xw.Close();
            Debug.WriteLine(xElement.ToString());
        }
    }
}
