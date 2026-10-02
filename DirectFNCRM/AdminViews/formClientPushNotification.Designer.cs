namespace DirectFNCRM.AdminViews
{
    partial class formClientPushNotification
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
            this.rbtnTekKullanici = new System.Windows.Forms.RadioButton();
            this.rbtnTumKullanici = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHesapno = new System.Windows.Forms.TextBox();
            this.rtboxMesaj = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbtnTekKullanici
            // 
            this.rbtnTekKullanici.AutoSize = true;
            this.rbtnTekKullanici.Location = new System.Drawing.Point(14, 12);
            this.rbtnTekKullanici.Name = "rbtnTekKullanici";
            this.rbtnTekKullanici.Size = new System.Drawing.Size(109, 21);
            this.rbtnTekKullanici.TabIndex = 0;
            this.rbtnTekKullanici.TabStop = true;
            this.rbtnTekKullanici.Text = "Tek Kullanıcı";
            this.rbtnTekKullanici.UseVisualStyleBackColor = true;
            this.rbtnTekKullanici.Click += new System.EventHandler(this.rbtnTumKullanici_Click);
            // 
            // rbtnTumKullanici
            // 
            this.rbtnTumKullanici.AutoSize = true;
            this.rbtnTumKullanici.Location = new System.Drawing.Point(141, 12);
            this.rbtnTumKullanici.Name = "rbtnTumKullanici";
            this.rbtnTumKullanici.Size = new System.Drawing.Size(137, 21);
            this.rbtnTumKullanici.TabIndex = 1;
            this.rbtnTumKullanici.TabStop = true;
            this.rbtnTumKullanici.Text = "Tüm Kullanıcılara";
            this.rbtnTumKullanici.UseVisualStyleBackColor = true;
            this.rbtnTumKullanici.Click += new System.EventHandler(this.rbtnTumKullanici_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "HesapNo";
            // 
            // txtHesapno
            // 
            this.txtHesapno.Location = new System.Drawing.Point(102, 46);
            this.txtHesapno.Name = "txtHesapno";
            this.txtHesapno.Size = new System.Drawing.Size(131, 22);
            this.txtHesapno.TabIndex = 3;
            // 
            // rtboxMesaj
            // 
            this.rtboxMesaj.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtboxMesaj.Location = new System.Drawing.Point(12, 83);
            this.rtboxMesaj.Name = "rtboxMesaj";
            this.rtboxMesaj.Size = new System.Drawing.Size(524, 216);
            this.rtboxMesaj.TabIndex = 4;
            this.rtboxMesaj.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(415, 14);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(121, 54);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Gönder";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // formClientPushNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 317);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.rtboxMesaj);
            this.Controls.Add(this.txtHesapno);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbtnTumKullanici);
            this.Controls.Add(this.rbtnTekKullanici);
            this.Name = "formClientPushNotification";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Müşteriye Mesaj Gönder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtnTekKullanici;
        private System.Windows.Forms.RadioButton rbtnTumKullanici;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHesapno;
        private System.Windows.Forms.RichTextBox rtboxMesaj;
        private System.Windows.Forms.Button btnSend;
    }
}