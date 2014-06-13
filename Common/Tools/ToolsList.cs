using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tools.AutoCode;

namespace Tools
{
    public partial class ToolsList : Form
    {
        /// <summary>
        /// 数据库工具窗口
        /// </summary>
        private ToolDbScriptsForm dbScriptForm;
        /// <summary>
        /// 自动生成代码窗口
        /// </summary>
        private ToolAutoCodeForm autoCodeForm;
        public ToolsList()
        {
            InitializeComponent();
        }

        private void showDbInfo_Click(object sender, EventArgs e)
        {
            dbScriptForm = new ToolDbScriptsForm();
            dbScriptForm.Show();
            this.Hide();
        }

        private void autoCodeOnekey_Click(object sender, EventArgs e)
        {
            autoCodeForm = new ToolAutoCodeForm();
            autoCodeForm.Show();
            this.Hide();
        }
    }
}
