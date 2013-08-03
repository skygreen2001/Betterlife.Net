namespace Tools
{
    partial class ToolsList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnModifyFile = new System.Windows.Forms.Button();
            this.showDbInfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnModifyFile
            // 
            this.btnModifyFile.Location = new System.Drawing.Point(12, 12);
            this.btnModifyFile.Name = "btnModifyFile";
            this.btnModifyFile.Size = new System.Drawing.Size(120, 48);
            this.btnModifyFile.TabIndex = 0;
            this.btnModifyFile.Text = "修改配置文件";
            this.btnModifyFile.UseVisualStyleBackColor = true;
            this.btnModifyFile.Click += new System.EventHandler(this.btnModifyFile_Click);
            // 
            // showDbInfo
            // 
            this.showDbInfo.Location = new System.Drawing.Point(12, 66);
            this.showDbInfo.Name = "showDbInfo";
            this.showDbInfo.Size = new System.Drawing.Size(120, 52);
            this.showDbInfo.TabIndex = 1;
            this.showDbInfo.Text = "显示数据库信息";
            this.showDbInfo.UseVisualStyleBackColor = true;
            this.showDbInfo.Click += new System.EventHandler(this.showDbInfo_Click);
            // 
            // ToolsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 522);
            this.Controls.Add(this.showDbInfo);
            this.Controls.Add(this.btnModifyFile);
            this.Name = "ToolsList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工具箱";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnModifyFile;
        private System.Windows.Forms.Button showDbInfo;
    }
}