namespace Touha.Utils.DBExportDoc
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.ckListBoxTables = new System.Windows.Forms.CheckedListBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnFileSel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "连接字符串：";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(144, 17);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(530, 21);
            this.txtConnectionString.TabIndex = 1;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(735, 15);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // ckListBoxTables
            // 
            this.ckListBoxTables.CheckOnClick = true;
            this.ckListBoxTables.FormattingEnabled = true;
            this.ckListBoxTables.Location = new System.Drawing.Point(15, 56);
            this.ckListBoxTables.Name = "ckListBoxTables";
            this.ckListBoxTables.Size = new System.Drawing.Size(243, 388);
            this.ckListBoxTables.TabIndex = 3;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(608, 79);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnFileSel
            // 
            this.btnFileSel.Location = new System.Drawing.Point(307, 79);
            this.btnFileSel.Name = "btnFileSel";
            this.btnFileSel.Size = new System.Drawing.Size(101, 23);
            this.btnFileSel.TabIndex = 5;
            this.btnFileSel.Text = "选择模板文件";
            this.btnFileSel.UseVisualStyleBackColor = true;
            this.btnFileSel.Click += new System.EventHandler(this.btnFileSel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 467);
            this.Controls.Add(this.btnFileSel);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.ckListBoxTables);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.CheckedListBox ckListBoxTables;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnFileSel;
    }
}

