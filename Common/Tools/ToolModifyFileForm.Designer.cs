namespace Tools
{
    partial class ToolModifyFileForm
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
            this.btnReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnModifyFile
            // 
            this.btnModifyFile.AutoEllipsis = true;
            this.btnModifyFile.Location = new System.Drawing.Point(24, 12);
            this.btnModifyFile.Name = "btnModifyFile";
            this.btnModifyFile.Size = new System.Drawing.Size(109, 23);
            this.btnModifyFile.TabIndex = 0;
            this.btnModifyFile.Text = "修改数据库配置";
            this.btnModifyFile.UseVisualStyleBackColor = true;
            this.btnModifyFile.Click += new System.EventHandler(this.btnModifyFile_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(24, 41);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(109, 23);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "还原";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 314);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnModifyFile);
            this.Name = "MainForm";
            this.Text = "工具箱";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnModifyFile;
        private System.Windows.Forms.Button btnReset;
    }
}

