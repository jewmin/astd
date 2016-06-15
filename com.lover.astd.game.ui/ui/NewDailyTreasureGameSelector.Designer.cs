namespace com.lover.astd.game.ui.ui
{
    partial class NewDailyTreasureGameSelector
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
            this.combo_games = new System.Windows.Forms.ComboBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_init = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // combo_games
            // 
            this.combo_games.FormattingEnabled = true;
            this.combo_games.Location = new System.Drawing.Point(15, 17);
            this.combo_games.Name = "combo_games";
            this.combo_games.Size = new System.Drawing.Size(419, 20);
            this.combo_games.TabIndex = 5;
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(544, 15);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 4;
            this.btn_ok.Text = "确定";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_init
            // 
            this.btn_init.Location = new System.Drawing.Point(450, 15);
            this.btn_init.Name = "btn_init";
            this.btn_init.Size = new System.Drawing.Size(75, 23);
            this.btn_init.TabIndex = 3;
            this.btn_init.Text = "读取";
            this.btn_init.UseVisualStyleBackColor = true;
            this.btn_init.Click += new System.EventHandler(this.btn_init_Click);
            // 
            // NewDailyTreasureGameSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 52);
            this.Controls.Add(this.combo_games);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.btn_init);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "NewDailyTreasureGameSelector";
            this.Text = "日常探宝副本选择器";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox combo_games;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_init;
    }
}