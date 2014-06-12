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
        private ToolDbScriptsForm dbScriptForm;
        private AutoCodeOneKey autoCode;
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
            autoCode.Run();
        }
    }
}
