using System;
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

            btnMigrantScript.Enabled = false;
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
            string sql=ToolDbScripts.MigrantFromMysql();
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

        private void cbDbType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (isDbTypeChanged)
            {
                cbDatabases.Text = (cbDbType.SelectedIndex==(int)DbType.SqlServer)?UtilSqlserver.Database_Name:UtilMysql.Database_Name;
                if (cbDbType.SelectedIndex == (int)DbType.SqlServer)
                {
                    UtilSqlserver.SetDatabase(UtilSqlserver.Database_Name);
                    btnMigrantScript.Enabled = false;
                    btnDropAllTables.Enabled = true;
                }
                else
                {
                    UtilMysql.SetDatabase(UtilMysql.Database_Name);
                    btnMigrantScript.Enabled = true;
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

        private void btnExportexcel_Click(object sender, EventArgs e)
        {
            btnExportexcel.Enabled = false;

            UtilExcel excel = new UtilExcel();
            ExcelBE be = null;
            Dictionary<string, Dictionary<string, string>> tableInfos;
            tableInfos = (cbDbType.SelectedIndex == 0) ? UtilSqlserver.TableinfoList() : UtilMysql.TableinfoList();

            be = new ExcelBE(1, 1, "ID", "A1", "A1", "GRAY", false, 10, 13.50, 26.50, 2, null, "Century Gothic", 10, true, null);
            excel.InsertData(be);
            be = new ExcelBE(1, 2, "Table Name", "B1", "B1", "GRAY", false, 10, 32.63, 26.50, 2, null, "Century Gothic", 10, true, null);
            excel.InsertData(be);
            be = new ExcelBE(1, 3, "Description", "C1", "C1", "GRAY", false, 10, 24.50, 26.50, 2, null, "Century Gothic", 10, true, null);
            excel.InsertData(be);
            int rowno = 2;
            int index = 1;
            foreach (Dictionary<string, string> tablename in tableInfos.Values)
            {
                be = new ExcelBE(rowno, 1, "A" + index, "A" + rowno, "A" + rowno, null, false, 10, 13.50, 26.50, 2, null, "Century Gothic", 10, false, null);
                excel.InsertData(be);
                be = new ExcelBE(rowno, 2, tablename["Name"], "B" + rowno, "B" + rowno, null, false, 10, 32.63, 26.50, 1, null, "Century Gothic", 10, false, null);
                excel.InsertData(be);
                be = new ExcelBE(rowno, 3, tablename["Comment"], "C" + rowno, "C" + rowno, null, false, 10, 24.50, 26.50, 1, null, "Century Gothic", 10, false, null);
                excel.InsertData(be);
                rowno++;
                index++;
            }
            //this.listResult.AppendText(tablenames);
            excel.doExport();
            btnExportexcel.Enabled = true;

        }
    }
}
