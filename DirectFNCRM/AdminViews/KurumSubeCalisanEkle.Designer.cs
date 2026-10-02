
namespace DirectFNCRM.AdminViews
{
    partial class KurumSubeCalisanEkle
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
            this.CalisanGuncelle = new System.Windows.Forms.Button();
            this.CalisanOrnek = new System.Windows.Forms.Button();
            this.SubeKurumGuncelle = new System.Windows.Forms.Button();
            this.SubeOrnekDosya = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SubeDosyaSec = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDosyaYol = new System.Windows.Forms.Label();
            this.lblDosyaAd = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblKullaniciSayisi = new System.Windows.Forms.Label();
            this.lblCalisansayisi = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnTemizle = new System.Windows.Forms.Button();
            this.lbleklemeYapilamayankullanici = new System.Windows.Forms.Label();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CalisanGuncelle
            // 
            this.CalisanGuncelle.Location = new System.Drawing.Point(217, 495);
            this.CalisanGuncelle.Name = "CalisanGuncelle";
            this.CalisanGuncelle.Size = new System.Drawing.Size(169, 40);
            this.CalisanGuncelle.TabIndex = 1;
            this.CalisanGuncelle.Text = "Dosyadan Calışanlar Ekle";
            this.CalisanGuncelle.UseVisualStyleBackColor = true;
            this.CalisanGuncelle.Click += new System.EventHandler(this.CalisanGuncelle_Click);
            // 
            // CalisanOrnek
            // 
            this.CalisanOrnek.Location = new System.Drawing.Point(369, 12);
            this.CalisanOrnek.Name = "CalisanOrnek";
            this.CalisanOrnek.Size = new System.Drawing.Size(265, 40);
            this.CalisanOrnek.TabIndex = 2;
            this.CalisanOrnek.Text = "Calisanlar Örnek Dosya Oluştur";
            this.CalisanOrnek.UseVisualStyleBackColor = true;
            this.CalisanOrnek.Click += new System.EventHandler(this.CalisanOrnek_Click);
            // 
            // SubeKurumGuncelle
            // 
            this.SubeKurumGuncelle.Location = new System.Drawing.Point(12, 495);
            this.SubeKurumGuncelle.Name = "SubeKurumGuncelle";
            this.SubeKurumGuncelle.Size = new System.Drawing.Size(165, 40);
            this.SubeKurumGuncelle.TabIndex = 3;
            this.SubeKurumGuncelle.Text = "Dosyadan Şube ve Temsilci Güncelleme";
            this.SubeKurumGuncelle.UseVisualStyleBackColor = true;
            this.SubeKurumGuncelle.Click += new System.EventHandler(this.SubeKurumGuncelle_Click);
            // 
            // SubeOrnekDosya
            // 
            this.SubeOrnekDosya.Location = new System.Drawing.Point(34, 12);
            this.SubeOrnekDosya.Name = "SubeOrnekDosya";
            this.SubeOrnekDosya.Size = new System.Drawing.Size(265, 40);
            this.SubeOrnekDosya.TabIndex = 4;
            this.SubeOrnekDosya.Text = "Dosyadan Sube İçin Örnek Dosya Oluştur";
            this.SubeOrnekDosya.UseVisualStyleBackColor = true;
            this.SubeOrnekDosya.Click += new System.EventHandler(this.SubeOrnekDosya_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(34, 240);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(134, 186);
            this.listBox1.TabIndex = 5;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(446, 240);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(179, 186);
            this.listBox2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Seçilen Dosya Yolu :";
            // 
            // SubeDosyaSec
            // 
            this.SubeDosyaSec.Location = new System.Drawing.Point(34, 69);
            this.SubeDosyaSec.Name = "SubeDosyaSec";
            this.SubeDosyaSec.Size = new System.Drawing.Size(134, 40);
            this.SubeDosyaSec.TabIndex = 4;
            this.SubeDosyaSec.Text = "Dosya Seç";
            this.SubeDosyaSec.UseVisualStyleBackColor = true;
            this.SubeDosyaSec.Click += new System.EventHandler(this.SubeDosyaSec_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Seçilen Dosya Adı :";
            // 
            // lblDosyaYol
            // 
            this.lblDosyaYol.AutoSize = true;
            this.lblDosyaYol.Location = new System.Drawing.Point(136, 155);
            this.lblDosyaYol.Name = "lblDosyaYol";
            this.lblDosyaYol.Size = new System.Drawing.Size(0, 13);
            this.lblDosyaYol.TabIndex = 6;
            // 
            // lblDosyaAd
            // 
            this.lblDosyaAd.AutoSize = true;
            this.lblDosyaAd.Location = new System.Drawing.Point(136, 132);
            this.lblDosyaAd.Name = "lblDosyaAd";
            this.lblDosyaAd.Size = new System.Drawing.Size(0, 13);
            this.lblDosyaAd.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Seçilen Kullanıcı Sayısı :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(366, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Seçilen Çalıişan Sayısı :";
            // 
            // lblKullaniciSayisi
            // 
            this.lblKullaniciSayisi.AutoSize = true;
            this.lblKullaniciSayisi.Location = new System.Drawing.Point(147, 181);
            this.lblKullaniciSayisi.Name = "lblKullaniciSayisi";
            this.lblKullaniciSayisi.Size = new System.Drawing.Size(0, 13);
            this.lblKullaniciSayisi.TabIndex = 6;
            // 
            // lblCalisansayisi
            // 
            this.lblCalisansayisi.AutoSize = true;
            this.lblCalisansayisi.Location = new System.Drawing.Point(489, 181);
            this.lblCalisansayisi.Name = "lblCalisansayisi";
            this.lblCalisansayisi.Size = new System.Drawing.Size(0, 13);
            this.lblCalisansayisi.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 211);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(182, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "GÜNCELLENECEK KULLANICILAR!";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(443, 211);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "EKLENECEK ÇALIŞANLAR";
            // 
            // btnTemizle
            // 
            this.btnTemizle.Location = new System.Drawing.Point(195, 69);
            this.btnTemizle.Name = "btnTemizle";
            this.btnTemizle.Size = new System.Drawing.Size(134, 40);
            this.btnTemizle.TabIndex = 4;
            this.btnTemizle.Text = "Temizle";
            this.btnTemizle.UseVisualStyleBackColor = true;
            this.btnTemizle.Click += new System.EventHandler(this.btnTemizle_Click);
            // 
            // lbleklemeYapilamayankullanici
            // 
            this.lbleklemeYapilamayankullanici.AutoSize = true;
            this.lbleklemeYapilamayankullanici.Location = new System.Drawing.Point(256, 274);
            this.lbleklemeYapilamayankullanici.Name = "lbleklemeYapilamayankullanici";
            this.lbleklemeYapilamayankullanici.Size = new System.Drawing.Size(158, 13);
            this.lbleklemeYapilamayankullanici.TabIndex = 30;
            this.lbleklemeYapilamayankullanici.Text = "Ekleme yapılamayan kullanıcılar.";
            this.lbleklemeYapilamayankullanici.Visible = false;
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point(259, 290);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(93, 108);
            this.listBox3.TabIndex = 29;
            this.listBox3.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 542);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 13);
            this.label7.TabIndex = 31;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 558);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(622, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 32;
            this.progressBar1.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(360, 69);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(265, 40);
            this.button1.TabIndex = 33;
            this.button1.Text = "Adres Güncelle Örnek Dosya Oluştur";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(408, 495);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(217, 40);
            this.button2.TabIndex = 1;
            this.button2.Text = "Dosyadan Adres Güncelle";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // KurumSubeCalisanEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 587);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbleklemeYapilamayankullanici);
            this.Controls.Add(this.listBox3);
            this.Controls.Add(this.lblDosyaAd);
            this.Controls.Add(this.lblDosyaYol);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblCalisansayisi);
            this.Controls.Add(this.lblKullaniciSayisi);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.CalisanGuncelle);
            this.Controls.Add(this.CalisanOrnek);
            this.Controls.Add(this.btnTemizle);
            this.Controls.Add(this.SubeDosyaSec);
            this.Controls.Add(this.SubeKurumGuncelle);
            this.Controls.Add(this.SubeOrnekDosya);
            this.Name = "KurumSubeCalisanEkle";
            this.Text = "KurumSubeCalisanEkle";
            this.Load += new System.EventHandler(this.KurumSubeCalisanEkle_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CalisanGuncelle;
        private System.Windows.Forms.Button CalisanOrnek;
        private System.Windows.Forms.Button SubeKurumGuncelle;
        private System.Windows.Forms.Button SubeOrnekDosya;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SubeDosyaSec;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDosyaYol;
        private System.Windows.Forms.Label lblDosyaAd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblKullaniciSayisi;
        private System.Windows.Forms.Label lblCalisansayisi;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnTemizle;
        private System.Windows.Forms.Label lbleklemeYapilamayankullanici;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}