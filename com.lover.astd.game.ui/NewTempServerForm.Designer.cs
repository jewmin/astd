namespace com.lover.astd.game.ui
{
    partial class NewTempServerForm
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
            this.components = new System.ComponentModel.Container();
            this.btn_ticketWeapon = new System.Windows.Forms.Button();
            this.num_ticketWeapons = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.combo_ticketWeapons = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_del_wash = new System.Windows.Forms.Button();
            this.lst_wash = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_init_campaign = new System.Windows.Forms.Button();
            this.combo_campains = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_campaign = new System.Windows.Forms.Button();
            this.num_campaign_count = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_add_wash = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.num_rawstone_amount = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.chk_rawstone_sellsys = new System.Windows.Forms.CheckBox();
            this.btn_rawstone = new System.Windows.Forms.Button();
            this.btn_doStore = new System.Windows.Forms.Button();
            this.tip_temp = new System.Windows.Forms.ToolTip(this.components);
            this.chk_wash_zhi = new System.Windows.Forms.CheckBox();
            this.chk_wash_yong = new System.Windows.Forms.CheckBox();
            this.chk_wash_tong = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_init_melt = new System.Windows.Forms.Button();
            this.combo_store_equips = new System.Windows.Forms.ComboBox();
            this.btn_melt = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_init_wash = new System.Windows.Forms.Button();
            this.btn_doWeave = new System.Windows.Forms.Button();
            this.combo_wash_heroes = new System.Windows.Forms.ComboBox();
            this.btn_start_wash = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btn_hongbao = new System.Windows.Forms.Button();
            this.btn_juedou = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.num_ticketWeapons)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_campaign_count)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_rawstone_amount)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_ticketWeapon
            // 
            this.btn_ticketWeapon.Location = new System.Drawing.Point(201, 60);
            this.btn_ticketWeapon.Name = "btn_ticketWeapon";
            this.btn_ticketWeapon.Size = new System.Drawing.Size(52, 23);
            this.btn_ticketWeapon.TabIndex = 12;
            this.btn_ticketWeapon.Text = "开始";
            this.btn_ticketWeapon.UseVisualStyleBackColor = true;
            this.btn_ticketWeapon.Click += new System.EventHandler(this.btn_ticketWeapon_Click);
            // 
            // num_ticketWeapons
            // 
            this.num_ticketWeapons.Location = new System.Drawing.Point(81, 60);
            this.num_ticketWeapons.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.num_ticketWeapons.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_ticketWeapons.Name = "num_ticketWeapons";
            this.num_ticketWeapons.Size = new System.Drawing.Size(114, 21);
            this.num_ticketWeapons.TabIndex = 11;
            this.num_ticketWeapons.Value = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 10;
            this.label8.Text = "兑换数量：";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_ticketWeapon);
            this.groupBox4.Controls.Add(this.num_ticketWeapons);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.combo_ticketWeapons);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Location = new System.Drawing.Point(340, 38);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(268, 90);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "点券兑换兵器并升级";
            // 
            // combo_ticketWeapons
            // 
            this.combo_ticketWeapons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_ticketWeapons.FormattingEnabled = true;
            this.combo_ticketWeapons.Items.AddRange(new object[] {
            "无敌将军炮",
            "五毒问心钉",
            "玄霆角",
            "七戮锋",
            "落魂冥灯",
            "蟠龙华盖",
            "轩辕指南车"});
            this.combo_ticketWeapons.Location = new System.Drawing.Point(81, 34);
            this.combo_ticketWeapons.Name = "combo_ticketWeapons";
            this.combo_ticketWeapons.Size = new System.Drawing.Size(114, 20);
            this.combo_ticketWeapons.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "兑换兵器：";
            // 
            // btn_del_wash
            // 
            this.btn_del_wash.Location = new System.Drawing.Point(171, 45);
            this.btn_del_wash.Name = "btn_del_wash";
            this.btn_del_wash.Size = new System.Drawing.Size(53, 23);
            this.btn_del_wash.TabIndex = 13;
            this.btn_del_wash.Text = "删除";
            this.btn_del_wash.UseVisualStyleBackColor = true;
            this.btn_del_wash.Click += new System.EventHandler(this.btn_del_wash_Click);
            // 
            // lst_wash
            // 
            this.lst_wash.FormattingEnabled = true;
            this.lst_wash.ItemHeight = 12;
            this.lst_wash.Location = new System.Drawing.Point(12, 75);
            this.lst_wash.Name = "lst_wash";
            this.lst_wash.Size = new System.Drawing.Size(212, 64);
            this.lst_wash.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btn_init_campaign);
            this.groupBox1.Controls.Add(this.combo_campains);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btn_campaign);
            this.groupBox1.Controls.Add(this.num_campaign_count);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(2, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(335, 90);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "单人战役(从最低级开始)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.DarkRed;
            this.label6.Location = new System.Drawing.Point(10, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(317, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "必须保证你的一半兵力就能够打完整个战役, 否则会有问题";
            // 
            // btn_init_campaign
            // 
            this.btn_init_campaign.Location = new System.Drawing.Point(271, 34);
            this.btn_init_campaign.Name = "btn_init_campaign";
            this.btn_init_campaign.Size = new System.Drawing.Size(53, 23);
            this.btn_init_campaign.TabIndex = 8;
            this.btn_init_campaign.Text = "读取";
            this.btn_init_campaign.UseVisualStyleBackColor = true;
            this.btn_init_campaign.Click += new System.EventHandler(this.btn_init_campaign_Click);
            // 
            // combo_campains
            // 
            this.combo_campains.FormattingEnabled = true;
            this.combo_campains.Location = new System.Drawing.Point(80, 36);
            this.combo_campains.Name = "combo_campains";
            this.combo_campains.Size = new System.Drawing.Size(185, 20);
            this.combo_campains.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "从哪开始：";
            // 
            // btn_campaign
            // 
            this.btn_campaign.Enabled = false;
            this.btn_campaign.Location = new System.Drawing.Point(272, 60);
            this.btn_campaign.Name = "btn_campaign";
            this.btn_campaign.Size = new System.Drawing.Size(52, 23);
            this.btn_campaign.TabIndex = 2;
            this.btn_campaign.Text = "开始";
            this.btn_campaign.UseVisualStyleBackColor = true;
            this.btn_campaign.Click += new System.EventHandler(this.btn_campaign_Click);
            // 
            // num_campaign_count
            // 
            this.num_campaign_count.Location = new System.Drawing.Point(80, 61);
            this.num_campaign_count.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_campaign_count.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_campaign_count.Name = "num_campaign_count";
            this.num_campaign_count.Size = new System.Drawing.Size(185, 21);
            this.num_campaign_count.TabIndex = 1;
            this.num_campaign_count.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "打多少次：";
            // 
            // btn_add_wash
            // 
            this.btn_add_wash.Location = new System.Drawing.Point(109, 45);
            this.btn_add_wash.Name = "btn_add_wash";
            this.btn_add_wash.Size = new System.Drawing.Size(53, 23);
            this.btn_add_wash.TabIndex = 11;
            this.btn_add_wash.Text = "添加";
            this.btn_add_wash.UseVisualStyleBackColor = true;
            this.btn_add_wash.Click += new System.EventHandler(this.btn_add_wash_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.num_rawstone_amount);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.chk_rawstone_sellsys);
            this.groupBox5.Controls.Add(this.btn_rawstone);
            this.groupBox5.Location = new System.Drawing.Point(340, 134);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(268, 129);
            this.groupBox5.TabIndex = 20;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "切割/挂牌/抛售璞玉";
            // 
            // label10
            // 
            this.label10.ForeColor = System.Drawing.Color.DarkRed;
            this.label10.Location = new System.Drawing.Point(13, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(244, 28);
            this.label10.TabIndex = 16;
            this.label10.Text = "因为挂牌的石头不是及时到账, 因此下方输入的玉石数量仅是蓝色和白色切割的玉石量";
            // 
            // num_rawstone_amount
            // 
            this.num_rawstone_amount.Location = new System.Drawing.Point(107, 58);
            this.num_rawstone_amount.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.num_rawstone_amount.Name = "num_rawstone_amount";
            this.num_rawstone_amount.Size = new System.Drawing.Size(92, 21);
            this.num_rawstone_amount.TabIndex = 15;
            this.num_rawstone_amount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 62);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 14;
            this.label9.Text = "需要玉石数量：";
            // 
            // chk_rawstone_sellsys
            // 
            this.chk_rawstone_sellsys.AutoSize = true;
            this.chk_rawstone_sellsys.Location = new System.Drawing.Point(15, 85);
            this.chk_rawstone_sellsys.Name = "chk_rawstone_sellsys";
            this.chk_rawstone_sellsys.Size = new System.Drawing.Size(162, 16);
            this.chk_rawstone_sellsys.TabIndex = 13;
            this.chk_rawstone_sellsys.Text = "绿色以上抛售/不选则挂牌";
            this.chk_rawstone_sellsys.UseVisualStyleBackColor = true;
            // 
            // btn_rawstone
            // 
            this.btn_rawstone.Location = new System.Drawing.Point(205, 80);
            this.btn_rawstone.Name = "btn_rawstone";
            this.btn_rawstone.Size = new System.Drawing.Size(52, 23);
            this.btn_rawstone.TabIndex = 12;
            this.btn_rawstone.Text = "开始";
            this.btn_rawstone.UseVisualStyleBackColor = true;
            this.btn_rawstone.Click += new System.EventHandler(this.btn_rawstone_Click);
            // 
            // btn_doStore
            // 
            this.btn_doStore.Location = new System.Drawing.Point(195, 37);
            this.btn_doStore.Name = "btn_doStore";
            this.btn_doStore.Size = new System.Drawing.Size(61, 23);
            this.btn_doStore.TabIndex = 21;
            this.btn_doStore.Text = "整理仓库";
            this.btn_doStore.UseVisualStyleBackColor = true;
            this.btn_doStore.Click += new System.EventHandler(this.btn_doStore_Click);
            // 
            // tip_temp
            // 
            this.tip_temp.AutomaticDelay = 100;
            // 
            // chk_wash_zhi
            // 
            this.chk_wash_zhi.AutoSize = true;
            this.chk_wash_zhi.Location = new System.Drawing.Point(276, 21);
            this.chk_wash_zhi.Name = "chk_wash_zhi";
            this.chk_wash_zhi.Size = new System.Drawing.Size(36, 16);
            this.chk_wash_zhi.TabIndex = 10;
            this.chk_wash_zhi.Text = "智";
            this.chk_wash_zhi.UseVisualStyleBackColor = true;
            // 
            // chk_wash_yong
            // 
            this.chk_wash_yong.AutoSize = true;
            this.chk_wash_yong.Location = new System.Drawing.Point(229, 21);
            this.chk_wash_yong.Name = "chk_wash_yong";
            this.chk_wash_yong.Size = new System.Drawing.Size(36, 16);
            this.chk_wash_yong.TabIndex = 9;
            this.chk_wash_yong.Text = "勇";
            this.chk_wash_yong.UseVisualStyleBackColor = true;
            // 
            // chk_wash_tong
            // 
            this.chk_wash_tong.AutoSize = true;
            this.chk_wash_tong.Location = new System.Drawing.Point(178, 21);
            this.chk_wash_tong.Name = "chk_wash_tong";
            this.chk_wash_tong.Size = new System.Drawing.Size(36, 16);
            this.chk_wash_tong.TabIndex = 8;
            this.chk_wash_tong.Text = "统";
            this.chk_wash_tong.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_init_melt);
            this.groupBox2.Controls.Add(this.combo_store_equips);
            this.groupBox2.Controls.Add(this.btn_melt);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(2, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(335, 51);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "一键降级并融化装备";
            // 
            // btn_init_melt
            // 
            this.btn_init_melt.Location = new System.Drawing.Point(212, 18);
            this.btn_init_melt.Name = "btn_init_melt";
            this.btn_init_melt.Size = new System.Drawing.Size(53, 23);
            this.btn_init_melt.TabIndex = 7;
            this.btn_init_melt.Text = "读取";
            this.btn_init_melt.UseVisualStyleBackColor = true;
            this.btn_init_melt.Click += new System.EventHandler(this.btn_init_melt_Click);
            // 
            // combo_store_equips
            // 
            this.combo_store_equips.FormattingEnabled = true;
            this.combo_store_equips.Location = new System.Drawing.Point(46, 19);
            this.combo_store_equips.Name = "combo_store_equips";
            this.combo_store_equips.Size = new System.Drawing.Size(160, 20);
            this.combo_store_equips.TabIndex = 6;
            // 
            // btn_melt
            // 
            this.btn_melt.Enabled = false;
            this.btn_melt.Location = new System.Drawing.Point(271, 18);
            this.btn_melt.Name = "btn_melt";
            this.btn_melt.Size = new System.Drawing.Size(53, 23);
            this.btn_melt.TabIndex = 5;
            this.btn_melt.Text = "开始";
            this.btn_melt.UseVisualStyleBackColor = true;
            this.btn_melt.Click += new System.EventHandler(this.btn_melt_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "哪件：";
            // 
            // btn_init_wash
            // 
            this.btn_init_wash.Location = new System.Drawing.Point(46, 45);
            this.btn_init_wash.Name = "btn_init_wash";
            this.btn_init_wash.Size = new System.Drawing.Size(53, 23);
            this.btn_init_wash.TabIndex = 7;
            this.btn_init_wash.Text = "读取";
            this.btn_init_wash.UseVisualStyleBackColor = true;
            this.btn_init_wash.Click += new System.EventHandler(this.btn_init_wash_Click);
            // 
            // btn_doWeave
            // 
            this.btn_doWeave.Location = new System.Drawing.Point(6, 37);
            this.btn_doWeave.Name = "btn_doWeave";
            this.btn_doWeave.Size = new System.Drawing.Size(61, 23);
            this.btn_doWeave.TabIndex = 22;
            this.btn_doWeave.Text = "纺织";
            this.btn_doWeave.UseVisualStyleBackColor = true;
            this.btn_doWeave.Click += new System.EventHandler(this.btn_doWeave_Click);
            // 
            // combo_wash_heroes
            // 
            this.combo_wash_heroes.FormattingEnabled = true;
            this.combo_wash_heroes.Location = new System.Drawing.Point(46, 19);
            this.combo_wash_heroes.Name = "combo_wash_heroes";
            this.combo_wash_heroes.Size = new System.Drawing.Size(116, 20);
            this.combo_wash_heroes.TabIndex = 6;
            // 
            // btn_start_wash
            // 
            this.btn_start_wash.Enabled = false;
            this.btn_start_wash.Location = new System.Drawing.Point(271, 114);
            this.btn_start_wash.Name = "btn_start_wash";
            this.btn_start_wash.Size = new System.Drawing.Size(53, 23);
            this.btn_start_wash.TabIndex = 5;
            this.btn_start_wash.Text = "开始";
            this.btn_start_wash.UseVisualStyleBackColor = true;
            this.btn_start_wash.Click += new System.EventHandler(this.btn_start_wash_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "哪位：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_del_wash);
            this.groupBox3.Controls.Add(this.lst_wash);
            this.groupBox3.Controls.Add(this.btn_add_wash);
            this.groupBox3.Controls.Add(this.chk_wash_zhi);
            this.groupBox3.Controls.Add(this.chk_wash_yong);
            this.groupBox3.Controls.Add(this.chk_wash_tong);
            this.groupBox3.Controls.Add(this.btn_init_wash);
            this.groupBox3.Controls.Add(this.combo_wash_heroes);
            this.groupBox3.Controls.Add(this.btn_start_wash);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(2, 192);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(335, 143);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "一键洗将";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F);
            this.label4.ForeColor = System.Drawing.Color.IndianRed;
            this.label4.Location = new System.Drawing.Point(11, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(264, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "每个功能有读取按钮的请先点下读取";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btn_hongbao);
            this.groupBox6.Controls.Add(this.btn_juedou);
            this.groupBox6.Controls.Add(this.btn_doStore);
            this.groupBox6.Controls.Add(this.btn_doWeave);
            this.groupBox6.Location = new System.Drawing.Point(340, 269);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(268, 66);
            this.groupBox6.TabIndex = 23;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "其他";
            // 
            // btn_hongbao
            // 
            this.btn_hongbao.Location = new System.Drawing.Point(69, 37);
            this.btn_hongbao.Name = "btn_hongbao";
            this.btn_hongbao.Size = new System.Drawing.Size(61, 23);
            this.btn_hongbao.TabIndex = 24;
            this.btn_hongbao.Text = "送红包";
            this.btn_hongbao.UseVisualStyleBackColor = true;
            this.btn_hongbao.Click += new System.EventHandler(this.btn_hongbao_Click);
            // 
            // btn_juedou
            // 
            this.btn_juedou.Location = new System.Drawing.Point(132, 37);
            this.btn_juedou.Name = "btn_juedou";
            this.btn_juedou.Size = new System.Drawing.Size(61, 23);
            this.btn_juedou.TabIndex = 23;
            this.btn_juedou.Text = "决斗";
            this.btn_juedou.UseVisualStyleBackColor = true;
            this.btn_juedou.Click += new System.EventHandler(this.btn_juedou_Click);
            // 
            // NewTempServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 342);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "NewTempServerForm";
            this.Text = "临时任务管理";
            ((System.ComponentModel.ISupportInitialize)(this.num_ticketWeapons)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_campaign_count)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_rawstone_amount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_ticketWeapon;
        private System.Windows.Forms.NumericUpDown num_ticketWeapons;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox combo_ticketWeapons;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_del_wash;
        private System.Windows.Forms.ListBox lst_wash;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_init_campaign;
        private System.Windows.Forms.ComboBox combo_campains;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_campaign;
        private System.Windows.Forms.NumericUpDown num_campaign_count;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_add_wash;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown num_rawstone_amount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chk_rawstone_sellsys;
        private System.Windows.Forms.Button btn_rawstone;
        private System.Windows.Forms.Button btn_doStore;
        private System.Windows.Forms.ToolTip tip_temp;
        private System.Windows.Forms.CheckBox chk_wash_zhi;
        private System.Windows.Forms.CheckBox chk_wash_yong;
        private System.Windows.Forms.CheckBox chk_wash_tong;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_init_melt;
        private System.Windows.Forms.ComboBox combo_store_equips;
        private System.Windows.Forms.Button btn_melt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_init_wash;
        private System.Windows.Forms.Button btn_doWeave;
        private System.Windows.Forms.ComboBox combo_wash_heroes;
        private System.Windows.Forms.Button btn_start_wash;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btn_juedou;
        private System.Windows.Forms.Button btn_hongbao;
    }
}