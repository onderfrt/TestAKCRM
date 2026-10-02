namespace DirectFNCRM.AdminViews
{
    partial class formOtomatikBorsaRapor
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbNone = new System.Windows.Forms.CheckBox();
            this.cbAysonu = new System.Windows.Forms.CheckBox();
            this.cbGunluk = new System.Windows.Forms.CheckBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnGonderimTest = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbNone);
            this.groupBox1.Controls.Add(this.cbAysonu);
            this.groupBox1.Controls.Add(this.cbGunluk);
            this.groupBox1.Location = new System.Drawing.Point(32, 32);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(267, 123);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rapor Periodu";
            // 
            // cbNone
            // 
            this.cbNone.AutoSize = true;
            this.cbNone.Location = new System.Drawing.Point(23, 80);
            this.cbNone.Margin = new System.Windows.Forms.Padding(4);
            this.cbNone.Name = "cbNone";
            this.cbNone.Size = new System.Drawing.Size(116, 20);
            this.cbNone.TabIndex = 2;
            this.cbNone.Text = "Oluşturulmasın";
            this.cbNone.UseVisualStyleBackColor = true;
            // 
            // cbAysonu
            // 
            this.cbAysonu.AutoSize = true;
            this.cbAysonu.Location = new System.Drawing.Point(23, 52);
            this.cbAysonu.Margin = new System.Windows.Forms.Padding(4);
            this.cbAysonu.Name = "cbAysonu";
            this.cbAysonu.Size = new System.Drawing.Size(79, 20);
            this.cbAysonu.TabIndex = 1;
            this.cbAysonu.Text = "Ay Sonu";
            this.cbAysonu.UseVisualStyleBackColor = true;
            // 
            // cbGunluk
            // 
            this.cbGunluk.AutoSize = true;
            this.cbGunluk.Location = new System.Drawing.Point(23, 25);
            this.cbGunluk.Margin = new System.Windows.Forms.Padding(4);
            this.cbGunluk.Name = "cbGunluk";
            this.cbGunluk.Size = new System.Drawing.Size(70, 20);
            this.cbGunluk.TabIndex = 0;
            this.cbGunluk.Text = "Günlük";
            this.cbGunluk.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(32, 207);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(265, 117);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 187);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Bilgilendirme Mail Listesi :";
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(155, 346);
            this.btnKaydet.Margin = new System.Windows.Forms.Padding(4);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(144, 28);
            this.btnKaydet.TabIndex = 3;
            this.btnKaydet.Text = "Ayarları Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(32, 346);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(115, 28);
            this.btnTest.TabIndex = 4;
            this.btnTest.Text = "Test Çalıştır";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnGonderimTest
            // 
            this.btnGonderimTest.Location = new System.Drawing.Point(32, 394);
            this.btnGonderimTest.Name = "btnGonderimTest";
            this.btnGonderimTest.Size = new System.Drawing.Size(139, 28);
            this.btnGonderimTest.TabIndex = 5;
            this.btnGonderimTest.Text = "Gonderimi Test Et";
            this.btnGonderimTest.UseVisualStyleBackColor = true;
            this.btnGonderimTest.Click += new System.EventHandler(this.btnGonderimTest_Click);
            // 
            // formOtomatikBorsaRapor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 441);
            this.Controls.Add(this.btnGonderimTest);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "formOtomatikBorsaRapor";
            this.ShowIcon = false;
            this.Text = "Otomatik Borsa Rapor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbNone;
        private System.Windows.Forms.CheckBox cbAysonu;
        private System.Windows.Forms.CheckBox cbGunluk;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnGonderimTest;
    }
}