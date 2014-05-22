using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tools.DbScripts;
using Tools.Util.Db;
using Util.Common;
using System.Configuration;
using System.IO;

namespace Tools
{
    public partial class ToolDbScriptsForm : Form
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        enum DbType { Mysql = 1, SqlServer = 0 }; 
        public ToolDbScriptsForm()
        {
            InitializeComponent();

            cbDbType.SelectedIndex = 0;
            cbDatabases.Items.Clear();
            cbDatabases.Items.Add(UtilSqlserver.Database_Name);
            cbDatabases.SelectedIndex = 0;
            btnColumns.Enabled = false;

            //btnMigrantScript.Enabled = false;
            btnDropAllTables.Enabled = true;

        }

        /// <summary>
        /// 查询所有表信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewDbInfo_Click(object sender, EventArgs e)
        {
            btnViewDbInfo.Enabled = false;
            Dictionary<string, string> tables;
            tables = (cbDbType.SelectedIndex==0)?UtilSqlserver.TableList():UtilMysql.TableList();
            string tablenames = "";
            this.listResult.Clear();
            foreach (string tablename in tables.Values)
            {
                tablenames += tablename + "\r\n";
            }
            this.listResult.AppendText(tablenames);
            Console.WriteLine(tablenames);
            btnViewDbInfo.Enabled = true;
        }

        /// <summary>
        /// 查看所有的表信息包括表注释
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTableComment_Click(object sender, EventArgs e)
        {
            btnTableComment.Enabled = false;
            Dictionary<string, Dictionary<string, string>> tableInfos;
            tableInfos =  (cbDbType.SelectedIndex == 0) ? UtilSqlserver.TableinfoList():UtilMysql.TableinfoList();
            string tableComment,tablenames="";
            this.listResult.Clear();
            foreach (Dictionary<string, string> tablename in tableInfos.Values)
            {
                tableComment = tablename["Comment"];
                string[] tableCommentArr = tableComment.Split(new char[2]{'\r', '\n'});
                tableComment = tableCommentArr[0];
                tableComment = tablename["Name"] + ":" + tableComment+"\r\n";
                tablenames += tableComment;
            }
            this.listResult.AppendText(tablenames);
            Console.WriteLine(tablenames);
            btnTableComment.Enabled = true;
        }

        /// <summary>
        /// 查看指定表列名，如果没有指定表，默认指定第一张表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnColumns_Click(object sender, EventArgs e)
        {
            btnColumns.Enabled = false;
            this.listResult.Clear();
            string tablename;
            if (cbTables.Items.Count <= 0)
            {
                Dictionary<string, string> tables;
                tables = (cbDbType.SelectedIndex == 0) ? UtilSqlserver.TableList() : UtilMysql.TableList();
                tablename = tables.Values.First();
                foreach (string cur_tablename in tables.Values)
                {
                    this.cbTables.Items.Add(cur_tablename);
                }
                this.cbTables.SelectedIndex = 0;
            }
            else
            {
                tablename = (string)cbTables.SelectedItem;
            }
            Dictionary<string, Dictionary<string, string>> columnInfos;
            columnInfos = (cbDbType.SelectedIndex == 0) ? UtilSqlserver.FieldInfoList(tablename) : UtilMysql.FieldInfoList(tablename);

            this.listResult.AppendText("显示指定表的信息："+tablename+"\r\n");
            string columnResult,comment;
            foreach (Dictionary<string, string> columnInfo in columnInfos.Values)
            {
                comment = "";
                if (columnInfo.ContainsKey("Comment")){
                    comment = columnInfo["Comment"];
                    string[] commentArr = comment.Split(new char[2] { '\r', '\n' });
                    comment = commentArr[0];
                    comment = "|备注：" + comment;
                }
                columnResult = "列名：" + columnInfo["Field"] + comment + "|类型：" + columnInfo["Type"] + "|是否允许为空：" + columnInfo["Null"]+"\r\n";
                this.listResult.AppendText(columnResult);
            }

            btnColumns.Enabled = true;

        }

        /// <summary>
        /// 点选表列表框选择默认从数据库读一次所有表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbTables_MouseClick(object sender, MouseEventArgs e)
        {
            //if (cbTables.Items.Count <= 0)
            //{
                cbTables.Items.Clear();
                Dictionary<string, string> tables;
                tables = (cbDbType.SelectedIndex == 0) ? UtilSqlserver.TableList() : UtilMysql.TableList();
                foreach (string cur_tablename in tables.Values)
                {
                    this.cbTables.Items.Add(cur_tablename);
                }
                //this.cbTables.SelectedIndex = 0;

                btnColumns.Enabled = true;
            //}
        }

        /// <summary>
        /// 点选数据库列表框默认从数据库读一次所有数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDatabases_MouseClick(object sender, MouseEventArgs e)
        {
            //if (cbDatabases.Items.Count <= 0)
            //{
                cbDatabases.Items.Clear();
                List<string> databases;
                databases = (cbDbType.SelectedIndex == 0) ? UtilSqlserver.AllDatabaseNames() : UtilMysql.AllDatabaseNames();
                foreach (string database in databases)
                {
                    this.cbDatabases.Items.Add(database);
                }
                //this.cbDatabases.SelectedIndex = 0;
            //}

        }

        /// <summary>
        /// 移植数据库脚本[Mysql->SQLServer]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMigrantScript_Click(object sender, EventArgs e)
        {
            this.btnMigrantScript.Enabled = false;
            string sql="";
            if (cbDbType.SelectedIndex == (int)DbType.SqlServer)
            {
                sql = ToolDbScripts.DbCreateMigrantFromSqlserver();
            }
            else
            {
                sql = ToolDbScripts.DbCreateMigrantFromMysql();
            }
            this.listResult.Clear();
            this.listResult.AppendText(sql);
            this.btnMigrantScript.Enabled = true;
            Console.WriteLine(this.listResult);
        }

        /// <summary>
        /// 关闭窗口时显示主窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolDbScriptsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Default.mainWindow.Show();
        }

        private bool isDbChanged = false;
        private void cbDatabases_TextChanged(object sender, EventArgs e)
        {
            isDbChanged = true;
        }

        /// <summary>
        /// 选择不同的数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDatabases_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (isDbChanged)
            {

                if (cbDbType.SelectedIndex == (int)DbType.SqlServer)
                {
                    UtilSqlserver.SetDatabase(cbDatabases.SelectedItem.ToString());
                }
                else
                {
                    UtilMysql.SetDatabase(cbDatabases.SelectedItem.ToString());
                }
                isDbChanged = false;
                listResult.Clear();
                cbTables.Items.Clear();
                cbTables.Text = "";
                btnColumns.Enabled = false;
            }

        }
        
        bool isDbTypeChanged = false;
        private void cbDbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            isDbTypeChanged = true;
        }

        /// <summary>
        /// 选择不同数据库类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDbType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (isDbTypeChanged)
            {
                cbDatabases.Text = (cbDbType.SelectedIndex==(int)DbType.SqlServer)?UtilSqlserver.Database_Name:UtilMysql.Database_Name;
                if (cbDbType.SelectedIndex == (int)DbType.SqlServer)
                {
                    UtilSqlserver.SetDatabase(UtilSqlserver.Database_Name);
                    //btnMigrantScript.Enabled = false;
                    btnMigrantScript.Text = "移植数据库脚本[SQLServer->Mysql]";
                    btnDropAllTables.Enabled = true;
                }
                else
                {
                    UtilMysql.SetDatabase(UtilMysql.Database_Name);
                   // btnMigrantScript.Enabled = true;
                    btnMigrantScript.Text = "移植数据库脚本[Mysql->SQLServer]";
                    btnDropAllTables.Enabled = false;
                }
                cbDatabases.Text = "";
                isDbTypeChanged = false;
                listResult.Clear();
            }
        }

        /// <summary>
        /// 点击生成删除数据库所有表的脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDropAllTables_Click(object sender, EventArgs e)
        {
            this.btnDropAllTables.Enabled = false;
            string sql = ToolDbScripts.DropAllTables();
            this.listResult.Clear();
            this.listResult.AppendText(sql);
            this.btnDropAllTables.Enabled = true;
            Console.WriteLine(this.listResult);

        }

        /// <summary>
        /// 点击生成数据库说明Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportexcel_Click(object sender, EventArgs e)
        {
            btnExportexcel.Enabled = false;
            UtilExcelCom.Release();
            ExcelBE be = null;
            Dictionary<string, Dictionary<string, string>> tableInfos;
            tableInfos = (cbDbType.SelectedIndex == 0) ? UtilSqlserver.TableinfoList() : UtilMysql.TableinfoList();

            int rowHeight = 25;
            //数据库表说明
            UtilExcelCom.Current().SetSheet("Summary");
            be = new ExcelBE(1, 1, "编号", "A1", "A1", "GRAYDARK", false, 10, 13.50, rowHeight, 2, null, "Century Gothic", 10, true, null);
            UtilExcelCom.Current().InsertData(be);
            be = new ExcelBE(1, 2, "表名称", "B1", "B1", "GRAYDARK", false, 10, 32.63, rowHeight, 2, null, "Century Gothic", 10, true, null);
            UtilExcelCom.Current().InsertData(be);
            be = new ExcelBE(1, 3, "说明", "C1", "C1", "GRAYDARK", false, 10, 24.50, rowHeight, 2, null, "Century Gothic", 10, true, null);
            UtilExcelCom.Current().InsertData(be);

            int rowno = 2;
            int index = 1;
            int line = 1;
            string idIndex = "";
            string tablet_name = "", table_comment = "";
            string[] commentArr;
            ArrayList commentList;
            foreach (Dictionary<string, string> tablename in tableInfos.Values)
            {
                table_comment = "";
                idIndex = index.ToString();
                if (idIndex.Length < 2) idIndex = "0" + idIndex;
                be = new ExcelBE(rowno, 1, "A" + idIndex, "A" + rowno, "A" + rowno, null, false, 10, 13.50, rowHeight, 2, null, "Century Gothic", 10, false, null);
                UtilExcelCom.Current().InsertData(be);
                tablet_name=tablename["Name"];
                be = new ExcelBE(rowno, 2, tablet_name, "B" + rowno, "B" + rowno, null, false, 10, 32.63, rowHeight, 1, null, "Century Gothic", 10, false, null);
                UtilExcelCom.Current().InsertData(be);
                //添加链接
                UtilExcelCom.Current().AddLink(be,tablet_name,"列名");
                table_comment = tablename["Comment"];
                table_comment = table_comment.Trim();
                line = 1;
                rowHeight = 25;
                if (UtilString.Contains(table_comment, "\r", "\n"))
                {
                    rowHeight = 16;
                    commentArr = table_comment.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    rowHeight = rowHeight * line;
                    commentList = new ArrayList(commentArr);
                    table_comment = "";
                    foreach (var commenti in commentList)
                    {
                        table_comment += commenti + "\r\n";
                        line += 1;
                    }
                    line -= 1;
                    table_comment = table_comment.Substring(0, table_comment.Length - 2);
                    rowHeight = rowHeight * line + 8;
                }
                be = new ExcelBE(rowno, 3, table_comment, "C" + rowno, "C" + rowno, null, false, 10, 24.50, rowHeight, 1, null, "Century Gothic", 10, false, null);
                UtilExcelCom.Current().InsertData(be);
                rowno++;
                index++;
            }

            //数据库各表详细说明
            Dictionary<string, Dictionary<string, string>> columnInfos;
            string comment="", dicComment="";
            foreach (Dictionary<string, string> tablename in tableInfos.Values)
            {
                rowHeight = 16;
                columnInfos = (cbDbType.SelectedIndex == 0) ? UtilSqlserver.FieldInfoList(tablename["Name"]) : UtilMysql.FieldInfoList(tablename["Name"]);
                UtilExcelCom.Current().AddSheet(tablename["Name"]);
                be = new ExcelBE(1, 1, "列名", "A1", "A1", "GRAYDARK", false, 10, 36.50, rowHeight, 2, null, "Century Gothic", 10, true, null);
                UtilExcelCom.Current().InsertData(be);
                be = new ExcelBE(1, 2, "数据类型", "B1", "B1", "GRAYDARK", false, 10, 24.50, rowHeight, 2, null, "Century Gothic", 10, true, null);
                UtilExcelCom.Current().InsertData(be);
                be = new ExcelBE(1, 3, "长度", "C1", "C1", "GRAYDARK", false, 10, 24.50, rowHeight, 2, null, "Century Gothic", 10, true, null);
                UtilExcelCom.Current().InsertData(be);
                be = new ExcelBE(1, 4, "允许NULL", "D1", "D1", "GRAYDARK", false, 10, 24.50, rowHeight, 2, null, "Century Gothic", 10, true, null);
                UtilExcelCom.Current().InsertData(be);
                be = new ExcelBE(1, 5, "键值", "E1", "E1", "GRAYDARK", false, 10, 24.50, rowHeight, 2, null, "Century Gothic", 10, true, null);
                UtilExcelCom.Current().InsertData(be);
                be = new ExcelBE(1, 6, "说明", "F1", "F1", "GRAYDARK", false, 10, 39, rowHeight, 2, null, "Century Gothic", 10, true, null);
                UtilExcelCom.Current().InsertData(be);

                bool isDicComment = false;
                foreach (Dictionary<string, string> columnInfo in columnInfos.Values)
                {
                    if (columnInfo.Keys.Contains("Comment"))
                    {
                        comment = columnInfo["Comment"];
                    }
                    else
                    {
                        comment = "";
                    }
                    if (UtilString.Contains(comment, "\r", "\n"))
                    {

                        if (columnInfo["Type"].Equals("char"))
                        {
                            isDicComment = true;
                            break;
                        }
                    }
                }
                if (isDicComment)
                {
                    be = new ExcelBE(1, 7, "数据字典定义", "G1", "G1", "GRAYDARK", false, 10, 48.50, rowHeight, 2, null, "Century Gothic", 10, true, null);
                    UtilExcelCom.Current().InsertData(be);
                }
                int tablerowno = 2;
                bool isColumnDicComment = false;
                foreach (Dictionary<string, string> columnInfo in columnInfos.Values)
                {
                    rowHeight = 16; 
                    if (columnInfo.Keys.Contains("Comment"))
                        comment = columnInfo["Comment"];
                    else
                        comment = "";

                    isColumnDicComment = false;
                    dicComment = ""; 
                    line = 1;
                    comment = comment.Trim();
                    if (isDicComment)
                    {
                        if (UtilString.Contains(comment, "\r", "\n"))
                        {
                            commentArr = comment.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            comment = commentArr[0];
                            commentList = new ArrayList(commentArr);
                            commentList.Remove(0);
                            foreach (var commenti in commentList)
                            {
                                dicComment += commenti + "\r\n";
                                line += 1;
                            }
                            line -= 1;
                            dicComment = dicComment.Substring(0, dicComment.Length - 2);
                            rowHeight = rowHeight * line + 8;
                            if (columnInfo["Type"].Equals("char"))
                                isColumnDicComment = true;
                            else
                                comment = dicComment;
                        }
                        else
                        {
                            comment = comment.Trim();
                            if (UtilString.Contains(comment, "\r", "\n"))
                            {
                                commentArr = comment.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                commentList = new ArrayList(commentArr);
                                comment = "";
                                foreach (var commenti in commentList)
                                {
                                    comment += commenti + "\r\n";
                                    line += 1;
                                }
                                line -= 1;
                                comment = comment.Substring(0, comment.Length - 2);
                                rowHeight = rowHeight * line + 8;
                            }
                        }
                    }
                    else
                    {
                        comment = comment.Trim();
                        if (UtilString.Contains(comment, "\r", "\n"))
                        {
                            commentArr = comment.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            commentList = new ArrayList(commentArr);
                            comment = "";
                            foreach (var commenti in commentList)
                            {
                                comment += commenti + "\r\n";
                                line += 1;
                            }
                            line -= 1;
                            comment = comment.Substring(0, comment.Length - 2);
                            rowHeight = rowHeight * line + 8;
                        }
                    }
                    be = new ExcelBE(tablerowno, 1, columnInfo["Field"], "A" + tablerowno, "A" + tablerowno, null, false, 10, 18, rowHeight, 2, null, "Century Gothic", 10, false, null);
                    UtilExcelCom.Current().InsertData(be);
                    be = new ExcelBE(tablerowno, 2, columnInfo["Type"], "B" + tablerowno, "B" + tablerowno, null, false, 10, 15, rowHeight, 1, null, "Century Gothic", 10, false, null);
                    UtilExcelCom.Current().InsertData(be);
                    be = new ExcelBE(tablerowno, 3, columnInfo["Length"], "C" + tablerowno, "C" + tablerowno, null, false, 10, 9, rowHeight, 1, null, "Century Gothic", 10, false, null);
                    UtilExcelCom.Current().InsertData(be);
                    be = new ExcelBE(tablerowno, 4, columnInfo["Null"], "D" + tablerowno, "D" + tablerowno, null, false, 10, 9, rowHeight, 1, null, "Century Gothic", 10, false, null);
                    UtilExcelCom.Current().InsertData(be);
                    be = new ExcelBE(tablerowno, 5, columnInfo["Fkpk"], "E" + tablerowno, "E" + tablerowno, null, false, 10, 9, rowHeight, 1, null, "Century Gothic", 10, false, null);
                    UtilExcelCom.Current().InsertData(be);

                    be = new ExcelBE(tablerowno, 6, comment, "F" + tablerowno, "F" + tablerowno, null, false, 10, 39, rowHeight, 1, null, "Century Gothic", 10, false, null);
                    UtilExcelCom.Current().InsertData(be);

                    if (isDicComment)
                    {
                        if (!isColumnDicComment)dicComment = "";
                        be = new ExcelBE(tablerowno, 7, dicComment, "G" + tablerowno, "G" + tablerowno, null, false, 10, 45, rowHeight, 1, null, "Century Gothic", 10, false, null);
                        UtilExcelCom.Current().InsertData(be);
                    }
                    tablerowno++;
                }
            }

            UtilExcelCom.Current().SetActivateSheet(1);
            //显示Excel
            UtilExcelCom.Current().DoExport();

            string sitename = ConfigurationManager.AppSettings["SiteName"];
            UtilExcelCom.Current().Save(Path.Combine(System.Environment.CurrentDirectory, sitename + "数据模型.xlsx"));
            btnExportexcel.Enabled = true;
        }
    }
}
