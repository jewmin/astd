namespace com.lover.astd.game.ui.ui
{
    partial class NewNpcSelector
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
            this.btn_ok = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.combo_formation = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lst_npc = new System.Windows.Forms.ListBox();
            this.lst_reason = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(557, 285);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 38;
            this.btn_ok.Text = "确定";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label3.Location = new System.Drawing.Point(275, 290);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 15);
            this.label3.TabIndex = 37;
            this.label3.Text = "选择阵型：";
            // 
            // combo_formation
            // 
            this.combo_formation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_formation.FormattingEnabled = true;
            this.combo_formation.Items.AddRange(new object[] {
            "不变阵",
            "格挡阵",
            "长蛇阵",
            "锋矢阵",
            "偃月阵",
            "锥形阵",
            "八卦阵",
            "七星阵",
            "雁行阵"});
            this.combo_formation.Location = new System.Drawing.Point(379, 287);
            this.combo_formation.Name = "combo_formation";
            this.combo_formation.Size = new System.Drawing.Size(150, 20);
            this.combo_formation.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label2.Location = new System.Drawing.Point(275, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 35;
            this.label2.Text = "选择NPC：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 34;
            this.label1.Text = "选择目的：";
            // 
            // lst_npc
            // 
            this.lst_npc.DisplayMember = "ItemDesc";
            this.lst_npc.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lst_npc.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lst_npc.FormattingEnabled = true;
            this.lst_npc.ItemHeight = 15;
            this.lst_npc.Location = new System.Drawing.Point(275, 43);
            this.lst_npc.Name = "lst_npc";
            this.lst_npc.Size = new System.Drawing.Size(357, 214);
            this.lst_npc.TabIndex = 33;
            // 
            // lst_reason
            // 
            this.lst_reason.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lst_reason.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lst_reason.FormattingEnabled = true;
            this.lst_reason.ItemHeight = 20;
            this.lst_reason.Location = new System.Drawing.Point(12, 43);
            this.lst_reason.Name = "lst_reason";
            this.lst_reason.Size = new System.Drawing.Size(213, 264);
            this.lst_reason.TabIndex = 32;
            this.lst_reason.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lst_reason_DrawItem);
            this.lst_reason.SelectedIndexChanged += new System.EventHandler(this.lst_reason_SelectedIndexChanged);
            // 
            // NewNpcSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 322);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.combo_formation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lst_npc);
            this.Controls.Add(this.lst_reason);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "NewNpcSelector";
            this.Text = "NPC选择器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox combo_formation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lst_npc;
        private System.Windows.Forms.ListBox lst_reason;
    }
}