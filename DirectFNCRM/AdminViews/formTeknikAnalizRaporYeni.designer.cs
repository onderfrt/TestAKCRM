namespace DirectFNCRM.AdminViews
{
    partial class formTeknikAnalizRaporYeni
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
            this.richboxicerik = new System.Windows.Forms.RichTextBox();
            this.txtBaslik = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboPiyasa = new System.Windows.Forms.ComboBox();
            this.txtLink = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSembol = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnEkle = new System.Windows.Forms.Button();
            this.btnCikar = new System.Windows.Forms.Button();
            this.lboxSembol = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkMailGonder = new System.Windows.Forms.CheckBox();
            this.lblBaslangic = new System.Windows.Forms.Label();
            this.dtpBaslangic = new System.Windows.Forms.DateTimePicker();
            this.lblBitis = new System.Windows.Forms.Label();
            this.dtpBitis = new System.Windows.Forms.DateTimePicker();
            this.lblViopUyari = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richboxicerik
            // 
            this.richboxicerik.Location = new System.Drawing.Point(58, 123);
            this.richboxicerik.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.richboxicerik.Name = "richboxicerik";
            this.richboxicerik.Size = new System.Drawing.Size(708, 331);
            this.richboxicerik.TabIndex = 6;
            this.richboxicerik.Text = "";
            // 
            // txtBaslik
            // 
            this.txtBaslik.Location = new System.Drawing.Point(58, 26);
            this.txtBaslik.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBaslik.Name = "txtBaslik";
            this.txtBaslik.Size = new System.Drawing.Size(708, 20);
            this.txtBaslik.TabIndex = 5;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(806, 10);
            this.btnSend.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(92, 37);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Gönder";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 119);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "İçerik";
            // 
            // comboPiyasa
            // 
            this.comboPiyasa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPiyasa.FormattingEnabled = true;
            this.comboPiyasa.Items.AddRange(new object[] {
            "Piyasa Seçiniz",
            "Hisse",
            "Viop"});
            this.comboPiyasa.Location = new System.Drawing.Point(58, 54);
            this.comboPiyasa.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboPiyasa.Name = "comboPiyasa";
            this.comboPiyasa.Size = new System.Drawing.Size(158, 21);
            this.comboPiyasa.TabIndex = 8;
            this.comboPiyasa.SelectedIndexChanged += new System.EventHandler(this.comboPiyasa_SelectedIndexChanged);
            // 
            // txtLink
            // 
            this.txtLink.Location = new System.Drawing.Point(58, 89);
            this.txtLink.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtLink.Name = "txtLink";
            this.txtLink.Size = new System.Drawing.Size(708, 20);
            this.txtLink.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 89);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Link";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 30);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Başlık";
            // 
            // txtSembol
            // 
            this.txtSembol.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSembol.Location = new System.Drawing.Point(769, 89);
            this.txtSembol.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSembol.Name = "txtSembol";
            this.txtSembol.Size = new System.Drawing.Size(146, 20);
            this.txtSembol.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(769, 72);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "İlişkili Semboller";
            // 
            // btnEkle
            // 
            this.btnEkle.BackColor = System.Drawing.Color.Lime;
            this.btnEkle.Location = new System.Drawing.Point(769, 110);
            this.btnEkle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(73, 23);
            this.btnEkle.TabIndex = 10;
            this.btnEkle.Text = "Ekle";
            this.btnEkle.UseVisualStyleBackColor = false;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // btnCikar
            // 
            this.btnCikar.BackColor = System.Drawing.Color.Salmon;
            this.btnCikar.Location = new System.Drawing.Point(842, 110);
            this.btnCikar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCikar.Name = "btnCikar";
            this.btnCikar.Size = new System.Drawing.Size(71, 23);
            this.btnCikar.TabIndex = 10;
            this.btnCikar.Text = "Çıkar";
            this.btnCikar.UseVisualStyleBackColor = false;
            this.btnCikar.Click += new System.EventHandler(this.btnCikar_Click);
            // 
            // lboxSembol
            // 
            this.lboxSembol.FormattingEnabled = true;
            this.lboxSembol.Location = new System.Drawing.Point(771, 137);
            this.lboxSembol.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lboxSembol.Name = "lboxSembol";
            this.lboxSembol.Size = new System.Drawing.Size(144, 316);
            this.lboxSembol.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 56);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Piyasa";
            // 
            // chkMailGonder  (Gönder butonunun altına taşındı)
            // 
            this.chkMailGonder.AutoSize = true;
            this.chkMailGonder.Checked = true;
            this.chkMailGonder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMailGonder.Location = new System.Drawing.Point(810, 51);
            this.chkMailGonder.Name = "chkMailGonder";
            this.chkMailGonder.Size = new System.Drawing.Size(83, 17);
            this.chkMailGonder.TabIndex = 12;
            this.chkMailGonder.Text = "Mail Gönder";
            this.chkMailGonder.UseVisualStyleBackColor = true;
            // 
            // lblBaslangic
            // 
            this.lblBaslangic.AutoSize = true;
            this.lblBaslangic.Location = new System.Drawing.Point(228, 57);
            this.lblBaslangic.Name = "lblBaslangic";
            this.lblBaslangic.Size = new System.Drawing.Size(53, 13);
            this.lblBaslangic.TabIndex = 13;
            this.lblBaslangic.Text = "Başlangıç";
            // 
            // dtpBaslangic
            // 
            this.dtpBaslangic.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpBaslangic.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBaslangic.Location = new System.Drawing.Point(285, 54);
            this.dtpBaslangic.Name = "dtpBaslangic";
            this.dtpBaslangic.ShowCheckBox = true;
            this.dtpBaslangic.Size = new System.Drawing.Size(135, 20);
            this.dtpBaslangic.TabIndex = 14;
            // 
            // lblBitis
            // 
            this.lblBitis.AutoSize = true;
            this.lblBitis.Location = new System.Drawing.Point(432, 57);
            this.lblBitis.Name = "lblBitis";
            this.lblBitis.Size = new System.Drawing.Size(28, 13);
            this.lblBitis.TabIndex = 15;
            this.lblBitis.Text = "Bitiş";
            // 
            // dtpBitis
            // 
            this.dtpBitis.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpBitis.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBitis.Location = new System.Drawing.Point(464, 54);
            this.dtpBitis.Name = "dtpBitis";
            this.dtpBitis.ShowCheckBox = true;
            this.dtpBitis.Size = new System.Drawing.Size(135, 20);
            this.dtpBitis.TabIndex = 16;
            // 
            // lblViopUyari
            // 
            this.lblViopUyari.AutoSize = true;
            this.lblViopUyari.ForeColor = System.Drawing.Color.Red;
            this.lblViopUyari.Location = new System.Drawing.Point(612, 57);
            this.lblViopUyari.Name = "lblViopUyari";
            this.lblViopUyari.Size = new System.Drawing.Size(140, 13);
            this.lblViopUyari.TabIndex = 17;
            this.lblViopUyari.Text = "VİOP mobilde gösterilmez";
            this.lblViopUyari.Visible = false;
            // 
            // formTeknikAnalizRaporYeni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 463);
            this.Controls.Add(this.lblViopUyari);
            this.Controls.Add(this.dtpBitis);
            this.Controls.Add(this.lblBitis);
            this.Controls.Add(this.dtpBaslangic);
            this.Controls.Add(this.lblBaslangic);
            this.Controls.Add(this.chkMailGonder);
            this.Controls.Add(this.lboxSembol);
            this.Controls.Add(this.btnCikar);
            this.Controls.Add(this.btnEkle);
            this.Controls.Add(this.txtSembol);
            this.Controls.Add(this.comboPiyasa);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.richboxicerik);
            this.Controls.Add(this.txtLink);
            this.Controls.Add(this.txtBaslik);
            this.Controls.Add(this.btnSend);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "formTeknikAnalizRaporYeni";
            this.ShowIcon = false;
            this.Text = "Teknik Analiz Raporları Düzenle";
            this.Load += new System.EventHandler(this.formTeknikAnalizRaporYeni_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox richboxicerik;
        private System.Windows.Forms.TextBox txtBaslik;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboPiyasa;
        private System.Windows.Forms.TextBox txtLink;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSembol;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.Button btnCikar;
        private System.Windows.Forms.ListBox lboxSembol;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkMailGonder;
        private System.Windows.Forms.Label lblBaslangic;
        private System.Windows.Forms.DateTimePicker dtpBaslangic;
        private System.Windows.Forms.Label lblBitis;
        private System.Windows.Forms.DateTimePicker dtpBitis;
        private System.Windows.Forms.Label lblViopUyari;
    }
}