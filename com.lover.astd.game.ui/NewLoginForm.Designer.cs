namespace com.lover.astd.game.ui
{
    partial class NewLoginForm
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
            this.label14 = new System.Windows.Forms.Label();
            this.num_proxy_port = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.txt_proxy_host = new System.Windows.Forms.TextBox();
            this.login_worker = new System.ComponentModel.BackgroundWorker();
            this.label12 = new System.Windows.Forms.Label();
            this.btn_delAccount = new System.Windows.Forms.Button();
            this.img_verify_code = new System.Windows.Forms.PictureBox();
            this.btn_refresh_verify_code = new System.Windows.Forms.Button();
            this.txt_verify_code = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lbl_login_status = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_roleName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_customGameUrl = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_customLoginUrl = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.combo_username = new System.Windows.Forms.ComboBox();
            this.txt_serverId = new System.Windows.Forms.NumericUpDown();
            this.btn_login = new System.Windows.Forms.Button();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboServerList = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.num_proxy_port)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_verify_code)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_serverId)).BeginInit();
            this.SuspendLayout();
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 329);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(203, 12);
            this.label14.TabIndex = 59;
            this.label14.Text = "代理设置(目前仅支持匿名http代理):";
            // 
            // num_proxy_port
            // 
            this.num_proxy_port.Location = new System.Drawing.Point(361, 349);
            this.num_proxy_port.Name = "num_proxy_port";
            this.num_proxy_port.Size = new System.Drawing.Size(100, 21);
            this.num_proxy_port.TabIndex = 58;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(259, 354);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(95, 12);
            this.label13.TabIndex = 57;
            this.label13.Text = "代理服务器端口:";
            // 
            // txt_proxy_host
            // 
            this.txt_proxy_host.Location = new System.Drawing.Point(130, 350);
            this.txt_proxy_host.Name = "txt_proxy_host";
            this.txt_proxy_host.Size = new System.Drawing.Size(100, 21);
            this.txt_proxy_host.TabIndex = 56;
            // 
            // login_worker
            // 
            this.login_worker.WorkerReportsProgress = true;
            this.login_worker.WorkerSupportsCancellation = true;
            this.login_worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.login_worker_DoWork);
            this.login_worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.login_worker_ProgressChanged);
            this.login_worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.login_worker_RunWorkerCompleted);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(53, 354);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 12);
            this.label12.TabIndex = 55;
            this.label12.Text = "代理服务器:";
            // 
            // btn_delAccount
            // 
            this.btn_delAccount.Location = new System.Drawing.Point(468, 67);
            this.btn_delAccount.Name = "btn_delAccount";
            this.btn_delAccount.Size = new System.Drawing.Size(67, 22);
            this.btn_delAccount.TabIndex = 54;
            this.btn_delAccount.Text = "删除账号";
            this.btn_delAccount.UseVisualStyleBackColor = true;
            this.btn_delAccount.Click += new System.EventHandler(this.btn_delAccount_Click);
            // 
            // img_verify_code
            // 
            this.img_verify_code.Location = new System.Drawing.Point(345, 140);
            this.img_verify_code.Name = "img_verify_code";
            this.img_verify_code.Size = new System.Drawing.Size(190, 92);
            this.img_verify_code.TabIndex = 53;
            this.img_verify_code.TabStop = false;
            // 
            // btn_refresh_verify_code
            // 
            this.btn_refresh_verify_code.Enabled = false;
            this.btn_refresh_verify_code.Location = new System.Drawing.Point(252, 144);
            this.btn_refresh_verify_code.Name = "btn_refresh_verify_code";
            this.btn_refresh_verify_code.Size = new System.Drawing.Size(75, 23);
            this.btn_refresh_verify_code.TabIndex = 52;
            this.btn_refresh_verify_code.Text = "刷新验证码";
            this.btn_refresh_verify_code.UseVisualStyleBackColor = true;
            this.btn_refresh_verify_code.Click += new System.EventHandler(this.btn_refresh_verify_code_Click);
            // 
            // txt_verify_code
            // 
            this.txt_verify_code.Enabled = false;
            this.txt_verify_code.Location = new System.Drawing.Point(126, 146);
            this.txt_verify_code.Name = "txt_verify_code";
            this.txt_verify_code.Size = new System.Drawing.Size(120, 21);
            this.txt_verify_code.TabIndex = 51;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(67, 151);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 50;
            this.label11.Text = "验证码：";
            // 
            // lbl_login_status
            // 
            this.lbl_login_status.ForeColor = System.Drawing.Color.DarkRed;
            this.lbl_login_status.Location = new System.Drawing.Point(9, 291);
            this.lbl_login_status.Name = "lbl_login_status";
            this.lbl_login_status.Size = new System.Drawing.Size(524, 30);
            this.lbl_login_status.TabIndex = 49;
            this.lbl_login_status.Text = "登录状态";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.DarkRed;
            this.label10.Location = new System.Drawing.Point(9, 238);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(491, 12);
            this.label10.TabIndex = 48;
            this.label10.Text = "如自定义服已到游戏,助手还未取数据,请点\"读取数据\";如需选角色, 请选完角色再读取数据";
            // 
            // txt_roleName
            // 
            this.txt_roleName.Location = new System.Drawing.Point(126, 121);
            this.txt_roleName.Name = "txt_roleName";
            this.txt_roleName.Size = new System.Drawing.Size(121, 21);
            this.txt_roleName.TabIndex = 38;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Chocolate;
            this.label9.Location = new System.Drawing.Point(251, 125);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(185, 12);
            this.label9.TabIndex = 47;
            this.label9.Text = "(在进入游戏前需要选择号时使用)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(55, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 46;
            this.label8.Text = "号的名字：";
            // 
            // txt_customGameUrl
            // 
            this.txt_customGameUrl.Location = new System.Drawing.Point(176, 203);
            this.txt_customGameUrl.Name = "txt_customGameUrl";
            this.txt_customGameUrl.Size = new System.Drawing.Size(161, 21);
            this.txt_customGameUrl.TabIndex = 43;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 207);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(161, 12);
            this.label7.TabIndex = 45;
            this.label7.Text = "自定义游戏地址(战报地址)：";
            // 
            // txt_customLoginUrl
            // 
            this.txt_customLoginUrl.Location = new System.Drawing.Point(176, 176);
            this.txt_customLoginUrl.Name = "txt_customLoginUrl";
            this.txt_customLoginUrl.Size = new System.Drawing.Size(161, 21);
            this.txt_customLoginUrl.TabIndex = 42;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(57, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 44;
            this.label6.Text = "自定义服登录地址：";
            // 
            // combo_username
            // 
            this.combo_username.FormattingEnabled = true;
            this.combo_username.Location = new System.Drawing.Point(126, 95);
            this.combo_username.Name = "combo_username";
            this.combo_username.Size = new System.Drawing.Size(121, 20);
            this.combo_username.TabIndex = 35;
            this.combo_username.SelectedIndexChanged += new System.EventHandler(this.combo_username_SelectedIndexChanged);
            // 
            // txt_serverId
            // 
            this.txt_serverId.Location = new System.Drawing.Point(362, 68);
            this.txt_serverId.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txt_serverId.Name = "txt_serverId";
            this.txt_serverId.Size = new System.Drawing.Size(100, 21);
            this.txt_serverId.TabIndex = 33;
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(201, 256);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(143, 28);
            this.btn_login.TabIndex = 40;
            this.btn_login.Text = "登录";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(362, 95);
            this.txt_password.Name = "txt_password";
            this.txt_password.PasswordChar = '*';
            this.txt_password.Size = new System.Drawing.Size(100, 21);
            this.txt_password.TabIndex = 36;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(286, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 41;
            this.label5.Text = "密码拿来：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 39;
            this.label4.Text = "报上大名：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(286, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 37;
            this.label3.Text = "哪个小区：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(193, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 33);
            this.label2.TabIndex = 34;
            this.label2.Text = "我要登录!";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 32;
            this.label1.Text = "哪里来的：";
            // 
            // comboServerList
            // 
            this.comboServerList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboServerList.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboServerList.FormattingEnabled = true;
            this.comboServerList.Location = new System.Drawing.Point(126, 68);
            this.comboServerList.Name = "comboServerList";
            this.comboServerList.Size = new System.Drawing.Size(121, 20);
            this.comboServerList.TabIndex = 31;
            this.comboServerList.SelectedIndexChanged += new System.EventHandler(this.comboServerList_SelectedIndexChanged);
            // 
            // NewLoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 382);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.num_proxy_port);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txt_proxy_host);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btn_delAccount);
            this.Controls.Add(this.img_verify_code);
            this.Controls.Add(this.btn_refresh_verify_code);
            this.Controls.Add(this.txt_verify_code);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lbl_login_status);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txt_roleName);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txt_customGameUrl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_customLoginUrl);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.combo_username);
            this.Controls.Add(this.txt_serverId);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.txt_password);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboServerList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "NewLoginForm";
            this.Text = "登录啦";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NewLoginForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.num_proxy_port)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_verify_code)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_serverId)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown num_proxy_port;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txt_proxy_host;
        private System.ComponentModel.BackgroundWorker login_worker;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btn_delAccount;
        private System.Windows.Forms.PictureBox img_verify_code;
        private System.Windows.Forms.Button btn_refresh_verify_code;
        private System.Windows.Forms.TextBox txt_verify_code;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbl_login_status;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_roleName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_customGameUrl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_customLoginUrl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox combo_username;
        private System.Windows.Forms.NumericUpDown txt_serverId;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboServerList;
    }
}