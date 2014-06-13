using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools.AutoCode;

namespace Tools
{
    public partial class ToolAutoCodeForm : Form
    {
        private AutoCodeOneKey autoCodeOneKey;
        public ToolAutoCodeForm()
        {
            InitializeComponent();
            tSaveDir.Text = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Model" + Path.DirectorySeparatorChar;
        }

        private void btnOneKey_Click(object sender, EventArgs e)
        {
            autoCodeOneKey = new AutoCodeOneKey();
            autoCodeOneKey.App_Dir = tSaveDir.Text;
            autoCodeOneKey.Run();
            MessageBox.Show("生成代码完成！");
        }

        private void ToolAutoCodeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Default.mainWindow.Show();
        }
    }
}
