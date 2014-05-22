namespace Tools
{
    partial class ToolDbScriptsForm
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
            this.btnViewDbInfo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDbType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnTableComment = new System.Windows.Forms.Button();
            this.btnColumns = new System.Windows.Forms.Button();
            this.cbTables = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnMigrantScript = new System.Windows.Forms.Button();
            this.listResult = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbDatabases = new System.Windows.Forms.ComboBox();
            this.btnDropAllTables = new System.Windows.Forms.Button();
            this.btnExportexcel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnViewDbInfo
            // 
            this.btnViewDbInfo.Location = new System.Drawing.Point(37, 97);
            this.btnViewDbInfo.Name = "btnViewDbInfo";
            this.btnViewDbInfo.Size = new System.Drawing.Size(116, 42);
            this.btnViewDbInfo.TabIndex = 0;
            this.btnViewDbInfo.Text = "所有表名";
            this.btnViewDbInfo.UseVisualStyleBackColor = true;
            this.btnViewDbInfo.Click += new System.EventHandler(this.btnViewDbInfo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(36, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "结果显示";
            // 
            // cbDbType
            // 
            this.cbDbType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cbDbType.FormattingEnabled = true;
            this.cbDbType.Items.AddRange(new object[] {
            "SQL Server",
            "Mysql"});
            this.cbDbType.Location = new System.Drawing.Point(107, 20);
            this.cbDbType.Name = "cbDbType";
            this.cbDbType.Size = new System.Drawing.Size(108, 20);
            this.cbDbType.TabIndex = 3;
            this.cbDbType.SelectedIndexChanged += new System.EventHandler(this.cbDbType_SelectedIndexChanged);
            this.cbDbType.SelectionChangeCommitted += new System.EventHandler(this.cbDbType_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "数据库类型";
            // 
            // btnTableComment
            // 
            this.btnTableComment.Location = new System.Drawing.Point(159, 97);
            this.btnTableComment.Name = "btnTableComment";
            this.btnTableComment.Size = new System.Drawing.Size(103, 42);
            this.btnTableComment.TabIndex = 5;
            this.btnTableComment.Text = "所有表评论";
            this.btnTableComment.UseVisualStyleBackColor = true;
            this.btnTableComment.Click += new System.EventHandler(this.btnTableComment_Click);
            // 
            // btnColumns
            // 
            this.btnColumns.Location = new System.Drawing.Point(268, 97);
            this.btnColumns.Name = "btnColumns";
            this.btnColumns.Size = new System.Drawing.Size(117, 42);
            this.btnColumns.TabIndex = 6;
            this.btnColumns.Text = "所有列名";
            this.btnColumns.UseVisualStyleBackColor = true;
            this.btnColumns.Click += new System.EventHandler(this.btnColumns_Click);
            // 
            // cbTables
            // 
            this.cbTables.FormattingEnabled = true;
            this.cbTables.Location = new System.Drawing.Point(266, 58);
            this.cbTables.Name = "cbTables";
            this.cbTables.Size = new System.Drawing.Size(117, 20);
            this.cbTables.TabIndex = 7;
            this.cbTables.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cbTables_MouseClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(221, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "表名称";
            // 
            // btnMigrantScript
            // 
            this.btnMigrantScript.Location = new System.Drawing.Point(394, 21);
            this.btnMigrantScript.Name = "btnMigrantScript";
            this.btnMigrantScript.Size = new System.Drawing.Size(125, 56);
            this.btnMigrantScript.TabIndex = 9;
            this.btnMigrantScript.Text = "移植数据库脚本[SQLServer->Mysql]";
            this.btnMigrantScript.UseVisualStyleBackColor = true;
            this.btnMigrantScript.Click += new System.EventHandler(this.btnMigrantScript_Click);
            // 
            // listResult
            // 
            this.listResult.Location = new System.Drawing.Point(38, 170);
            this.listResult.Name = "listResult";
            this.listResult.Size = new System.Drawing.Size(958, 466);
            this.listResult.TabIndex = 10;
            this.listResult.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "数据库名称";
            // 
            // cbDatabases
            // 
            this.cbDatabases.FormattingEnabled = true;
            this.cbDatabases.Location = new System.Drawing.Point(107, 58);
            this.cbDatabases.Name = "cbDatabases";
            this.cbDatabases.Size = new System.Drawing.Size(108, 20);
            this.cbDatabases.TabIndex = 12;
            this.cbDatabases.SelectionChangeCommitted += new System.EventHandler(this.cbDatabases_SelectionChangeCommitted);
            this.cbDatabases.TextChanged += new System.EventHandler(this.cbDatabases_TextChanged);
            this.cbDatabases.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cbDatabases_MouseClick);
            // 
            // btnDropAllTables
            // 
            this.btnDropAllTables.Location = new System.Drawing.Point(525, 20);
            this.btnDropAllTables.Name = "btnDropAllTables";
            this.btnDropAllTables.Size = new System.Drawing.Size(112, 56);
            this.btnDropAllTables.TabIndex = 13;
            this.btnDropAllTables.Text = "删除所有表";
            this.btnDropAllTables.UseVisualStyleBackColor = true;
            this.btnDropAllTables.Click += new System.EventHandler(this.btnDropAllTables_Click);
            // 
            // btnExportexcel
            // 
            this.btnExportexcel.Location = new System.Drawing.Point(394, 97);
            this.btnExportexcel.Name = "btnExportexcel";
            this.btnExportexcel.Size = new System.Drawing.Size(129, 42);
            this.btnExportexcel.TabIndex = 15;
            this.btnExportexcel.Text = "导出Excel(Beta)";
            this.btnExportexcel.UseVisualStyleBackColor = true;
            this.btnExportexcel.Click += new System.EventHandler(this.btnExportexcel_Click);
            // 
            // ToolDbScriptsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 649);
            this.Controls.Add(this.btnExportexcel);
            this.Controls.Add(this.btnDropAllTables);
            this.Controls.Add(this.cbDatabases);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listResult);
            this.Controls.Add(this.btnMigrantScript);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbTables);
            this.Controls.Add(this.btnColumns);
            this.Controls.Add(this.btnTableComment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbDbType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnViewDbInfo);
            this.Name = "ToolDbScriptsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "显示数据库信息";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ToolDbScriptsForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnViewDbInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDbType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnTableComment;
        private System.Windows.Forms.Button btnColumns;
        private System.Windows.Forms.ComboBox cbTables;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnMigrantScript;
        private System.Windows.Forms.RichTextBox listResult;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbDatabases;
        private System.Windows.Forms.Button btnDropAllTables;
        private System.Windows.Forms.Button btnExportexcel;
    }
}