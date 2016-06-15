namespace com.lover.astd.game.ui
{
    partial class NewTranslator
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
            this.txt_data = new System.Windows.Forms.TextBox();
            this.web_view = new System.Windows.Forms.WebBrowser();
            this.txt_url = new System.Windows.Forms.TextBox();
            this.btn_openUrl = new System.Windows.Forms.Button();
            this.txt_result = new System.Windows.Forms.TextBox();
            this.btn_open = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_data
            // 
            this.txt_data.Location = new System.Drawing.Point(314, 13);
            this.txt_data.Name = "txt_data";
            this.txt_data.Size = new System.Drawing.Size(35, 21);
            this.txt_data.TabIndex = 11;
            // 
            // web_view
            // 
            this.web_view.Location = new System.Drawing.Point(561, 13);
            this.web_view.MinimumSize = new System.Drawing.Size(20, 20);
            this.web_view.Name = "web_view";
            this.web_view.ScriptErrorsSuppressed = true;
            this.web_view.ScrollBarsEnabled = false;
            this.web_view.Size = new System.Drawing.Size(20, 20);
            this.web_view.TabIndex = 10;
            this.web_view.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            // 
            // txt_url
            // 
            this.txt_url.Location = new System.Drawing.Point(9, 13);
            this.txt_url.Name = "txt_url";
            this.txt_url.Size = new System.Drawing.Size(299, 21);
            this.txt_url.TabIndex = 9;
            // 
            // btn_openUrl
            // 
            this.btn_openUrl.Location = new System.Drawing.Point(355, 11);
            this.btn_openUrl.Name = "btn_openUrl";
            this.btn_openUrl.Size = new System.Drawing.Size(61, 23);
            this.btn_openUrl.TabIndex = 8;
            this.btn_openUrl.Text = "打开网址";
            this.btn_openUrl.UseVisualStyleBackColor = true;
            this.btn_openUrl.Click += new System.EventHandler(this.btn_openUrl_Click);
            // 
            // txt_result
            // 
            this.txt_result.Location = new System.Drawing.Point(9, 42);
            this.txt_result.Multiline = true;
            this.txt_result.Name = "txt_result";
            this.txt_result.Size = new System.Drawing.Size(572, 208);
            this.txt_result.TabIndex = 7;
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(421, 11);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(64, 23);
            this.btn_open.TabIndex = 6;
            this.btn_open.Text = "打开文件";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(490, 11);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(64, 23);
            this.btn_save.TabIndex = 12;
            this.btn_save.Text = "保存文件";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // NewTranslator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 262);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.txt_data);
            this.Controls.Add(this.web_view);
            this.Controls.Add(this.txt_url);
            this.Controls.Add(this.btn_openUrl);
            this.Controls.Add(this.txt_result);
            this.Controls.Add(this.btn_open);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "NewTranslator";
            this.Text = "协议解析器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_data;
        private System.Windows.Forms.WebBrowser web_view;
        private System.Windows.Forms.TextBox txt_url;
        private System.Windows.Forms.Button btn_openUrl;
        private System.Windows.Forms.TextBox txt_result;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.Button btn_save;
    }
}