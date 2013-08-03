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

namespace Tools
{
    public partial class ToolDbScriptsForm : Form
    {
        public ToolDbScriptsForm()
        {
            InitializeComponent();
            cbDbType.SelectedIndex = 0;
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
            if (cbTables.Items.Count <= 0)
            {
                Dictionary<string, string> tables;
                tables = (cbDbType.SelectedIndex == 0) ? UtilSqlserver.TableList() : UtilMysql.TableList();
                foreach (string cur_tablename in tables.Values)
                {
                    this.cbTables.Items.Add(cur_tablename);
                }
                this.cbTables.SelectedIndex = 0;
            }
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

    }
}
