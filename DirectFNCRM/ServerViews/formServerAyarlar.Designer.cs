namespace DirectFNCRM.ServerViews
{
    partial class formServerAyarlar
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSSOPort = new System.Windows.Forms.TextBox();
            this.txtSSOIP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServisPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtClientPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "SSO IP";
            // 
            // txtSSOPort
            // 
            this.txtSSOPort.Location = new System.Drawing.Point(79, 35);
            this.txtSSOPort.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSSOPort.Name = "txtSSOPort";
            this.txtSSOPort.Size = new System.Drawing.Size(83, 20);
            this.txtSSOPort.TabIndex = 3;
            // 
            // txtSSOIP
            // 
            this.txtSSOIP.Location = new System.Drawing.Point(79, 12);
            this.txtSSOIP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSSOIP.Name = "txtSSOIP";
            this.txtSSOIP.Size = new System.Drawing.Size(83, 20);
            this.txtSSOIP.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 60);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Servis Port";
            // 
            // txtServisPort
            // 
            this.txtServisPort.Location = new System.Drawing.Point(79, 58);
            this.txtServisPort.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtServisPort.Name = "txtServisPort";
            this.txtServisPort.Size = new System.Drawing.Size(83, 20);
            this.txtServisPort.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "SSO Port";
            // 
            // txtClientPort
            // 
            this.txtClientPort.Location = new System.Drawing.Point(79, 80);
            this.txtClientPort.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtClientPort.Name = "txtClientPort";
            this.txtClientPort.Size = new System.Drawing.Size(83, 20);
            this.txtClientPort.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 83);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Client Port";
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(260, 16);
            this.btnKaydet.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(65, 27);
            this.btnKaydet.TabIndex = 8;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // formServerAyarlar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 118);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtClientPort);
            this.Controls.Add(this.txtServisPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSSOPort);
            this.Controls.Add(this.txtSSOIP);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "formServerAyarlar";
            this.Text = "Server Bağlanti Ayarları";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.formServerAyarlar_FormClosed);
            this.Load += new System.EventHandler(this.formServerAyarlar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnKaydet;
        public System.Windows.Forms.TextBox txtSSOPort;
        public System.Windows.Forms.TextBox txtSSOIP;
        public System.Windows.Forms.TextBox txtServisPort;
        public System.Windows.Forms.TextBox txtClientPort;
    }
}