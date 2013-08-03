using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools.Config;

namespace Tools
{
    public partial class ToolModifyFileForm : Form
    {
        public ToolModifyFileForm()
        {
            InitializeComponent();
        }

        private void btnModifyFile_Click(object sender, EventArgs e)
        {
            this.btnModifyFile.Enabled = false;
            ToolModifyFile.run();
            this.btnModifyFile.Enabled = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.btnReset.Enabled = false;
            ToolModifyFile.reset();
            this.btnReset.Enabled = true;
        }

        private void ToolModifyFileForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Default.mainWindow.Show();
        }
    }
}
