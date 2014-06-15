namespace Tools
{
    partial class ToolAutoCodeForm
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
            this.btnOneKey = new System.Windows.Forms.Button();
            this.tSaveDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOneKey
            // 
            this.btnOneKey.Location = new System.Drawing.Point(234, 346);
            this.btnOneKey.Name = "btnOneKey";
            this.btnOneKey.Size = new System.Drawing.Size(239, 57);
            this.btnOneKey.TabIndex = 0;
            this.btnOneKey.Text = "一键生成所有代码";
            this.btnOneKey.UseVisualStyleBackColor = true;
            this.btnOneKey.Click += new System.EventHandler(this.btnOneKey_Click);
            // 
            // tSaveDir
            // 
            this.tSaveDir.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tSaveDir.Location = new System.Drawing.Point(126, 39);
            this.tSaveDir.Name = "tSaveDir";
            this.tSaveDir.Size = new System.Drawing.Size(457, 29);
            this.tSaveDir.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "保存路径：";
            // 
            // ToolAutoCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 537);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tSaveDir);
            this.Controls.Add(this.btnOneKey);
            this.Name = "ToolAutoCodeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AutoCodeForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ToolAutoCodeForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOneKey;
        private System.Windows.Forms.TextBox tSaveDir;
        private System.Windows.Forms.Label label1;
    }
}