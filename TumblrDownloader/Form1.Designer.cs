namespace TumblrDownloader
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.proxyAddr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.proxyPort = new System.Windows.Forms.TextBox();
            this.blogName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.LBL_PostCNT = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cb_ThreadCnt = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(839, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 27);
            this.button1.TabIndex = 1;
            this.button1.Text = "开始获取";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // proxyAddr
            // 
            this.proxyAddr.Location = new System.Drawing.Point(104, 16);
            this.proxyAddr.Name = "proxyAddr";
            this.proxyAddr.Size = new System.Drawing.Size(114, 25);
            this.proxyAddr.TabIndex = 2;
            this.proxyAddr.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "代理地址";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(225, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "代理端口";
            // 
            // proxyPort
            // 
            this.proxyPort.Location = new System.Drawing.Point(304, 16);
            this.proxyPort.Name = "proxyPort";
            this.proxyPort.Size = new System.Drawing.Size(41, 25);
            this.proxyPort.TabIndex = 5;
            this.proxyPort.Text = "1690";
            // 
            // blogName
            // 
            this.blogName.Location = new System.Drawing.Point(515, 16);
            this.blogName.Name = "blogName";
            this.blogName.Size = new System.Drawing.Size(247, 25);
            this.blogName.TabIndex = 6;
            this.blogName.Text = "http://shemale-pix.tumblr.com/archive/page/1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(392, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Tumblr博客名";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1108, 36);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(142, 66);
            this.button2.TabIndex = 9;
            this.button2.Text = "多线程传值测试";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 401);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(1292, 235);
            this.dataGridView1.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(349, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "Post数量：";
            // 
            // LBL_PostCNT
            // 
            this.LBL_PostCNT.AutoSize = true;
            this.LBL_PostCNT.Location = new System.Drawing.Point(439, 87);
            this.LBL_PostCNT.Name = "LBL_PostCNT";
            this.LBL_PostCNT.Size = new System.Drawing.Size(0, 15);
            this.LBL_PostCNT.TabIndex = 13;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(28, 329);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(86, 27);
            this.button3.TabIndex = 14;
            this.button3.Text = "全选";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(132, 329);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(86, 27);
            this.button4.TabIndex = 15;
            this.button4.Text = "全不选";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(644, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 15);
            this.label5.TabIndex = 16;
            this.label5.Text = "label5";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.Location = new System.Drawing.Point(352, 122);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(898, 169);
            this.listView1.TabIndex = 17;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(28, 122);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(252, 169);
            this.listBox1.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 15);
            this.label6.TabIndex = 19;
            this.label6.Text = "关注的Tumblr账号";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1108, 309);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(142, 66);
            this.button5.TabIndex = 20;
            this.button5.Text = "开始下载";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(810, 347);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
            this.label7.TabIndex = 22;
            this.label7.Text = "下载线程数";
            // 
            // cb_ThreadCnt
            // 
            this.cb_ThreadCnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_ThreadCnt.FormattingEnabled = true;
            this.cb_ThreadCnt.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cb_ThreadCnt.Location = new System.Drawing.Point(913, 344);
            this.cb_ThreadCnt.Name = "cb_ThreadCnt";
            this.cb_ThreadCnt.Size = new System.Drawing.Size(80, 23);
            this.cb_ThreadCnt.TabIndex = 23;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(395, 314);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(123, 57);
            this.button6.TabIndex = 24;
            this.button6.Text = "装载测试数据";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1292, 636);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.cb_ThreadCnt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.LBL_PostCNT);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.blogName);
            this.Controls.Add(this.proxyPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.proxyAddr);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox proxyAddr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox proxyPort;
        private System.Windows.Forms.TextBox blogName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LBL_PostCNT;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cb_ThreadCnt;
        private System.Windows.Forms.Button button6;
    }
}

