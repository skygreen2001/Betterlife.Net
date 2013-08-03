using System;
using System.Windows.Forms;

namespace Tools
{
    static class Default
    {
        public static ToolsList mainWindow;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainWindow=new ToolsList();
            //Application.Run(mainWindow);
            //Application.Run(new ToolModifyFileForm());
            Application.Run(new ToolDbScriptsForm());
        }
    }
}
