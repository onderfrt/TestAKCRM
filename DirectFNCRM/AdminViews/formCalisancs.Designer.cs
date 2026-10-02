namespace DirectFNCRM.AdminViews
{
    partial class formCalisancs
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtAdi = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSoyad = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.comboDepartman = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboYetki = new System.Windows.Forms.ComboBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboDurum = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtTemsilciKodu = new System.Windows.Forms.TextBox();
            this.txtKurumNo = new System.Windows.Forms.TextBox();
            this.txtTelefon = new System.Windows.Forms.TextBox();
            this.cmbCalisanSube = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtAra = new System.Windows.Forms.TextBox();
            this.chkBloke = new System.Windows.Forms.CheckBox();
            this.chbTumCalisanlar = new System.Windows.Forms.CheckBox();
            this.chbAktif = new System.Windows.Forms.CheckBox();
            this.gbFiltrele = new System.Windows.Forms.GroupBox();
            this.chbPasif = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.gbFiltrele.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(0, 201);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(770, 272);
            this.dataGridView1.TabIndex = 12;
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            // 
            // txtAdi
            // 
            this.txtAdi.Location = new System.Drawing.Point(73, 20);
            this.txtAdi.Margin = new System.Windows.Forms.Padding(2);
            this.txtAdi.Name = "txtAdi";
            this.txtAdi.Size = new System.Drawing.Size(161, 20);
            this.txtAdi.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Adı";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(673, 49);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(83, 29);
            this.btnSave.TabIndex = 100;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Soyadı";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 67);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Username";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 91);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Şifre";
            // 
            // txtSoyad
            // 
            this.txtSoyad.Location = new System.Drawing.Point(73, 43);
            this.txtSoyad.Margin = new System.Windows.Forms.Padding(2);
            this.txtSoyad.Name = "txtSoyad";
            this.txtSoyad.Size = new System.Drawing.Size(161, 20);
            this.txtSoyad.TabIndex = 1;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(73, 66);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(2);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(161, 20);
            this.txtUsername.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(73, 89);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(161, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // comboDepartman
            // 
            this.comboDepartman.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDepartman.FormattingEnabled = true;
            this.comboDepartman.Location = new System.Drawing.Point(547, 55);
            this.comboDepartman.Margin = new System.Windows.Forms.Padding(2);
            this.comboDepartman.Name = "comboDepartman";
            this.comboDepartman.Size = new System.Drawing.Size(102, 21);
            this.comboDepartman.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(462, 57);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Departman";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(462, 82);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Yetki";
            // 
            // comboYetki
            // 
            this.comboYetki.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboYetki.FormattingEnabled = true;
            this.comboYetki.Location = new System.Drawing.Point(547, 79);
            this.comboYetki.Margin = new System.Windows.Forms.Padding(2);
            this.comboYetki.Name = "comboYetki";
            this.comboYetki.Size = new System.Drawing.Size(102, 21);
            this.comboYetki.TabIndex = 6;
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(673, 15);
            this.btnNew.Margin = new System.Windows.Forms.Padding(2);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(82, 29);
            this.btnNew.TabIndex = 9;
            this.btnNew.Text = "Yeni Oluştur";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // txtMail
            // 
            this.txtMail.Location = new System.Drawing.Point(73, 111);
            this.txtMail.Margin = new System.Windows.Forms.Padding(2);
            this.txtMail.Name = "txtMail";
            this.txtMail.Size = new System.Drawing.Size(161, 20);
            this.txtMail.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 114);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "e-mail";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(462, 104);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Çalışan Tipi";
            // 
            // comboType
            // 
            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboType.FormattingEnabled = true;
            this.comboType.Location = new System.Drawing.Point(547, 102);
            this.comboType.Margin = new System.Windows.Forms.Padding(2);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(102, 21);
            this.comboType.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(462, 128);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Çalışan Durumu";
            // 
            // comboDurum
            // 
            this.comboDurum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDurum.FormattingEnabled = true;
            this.comboDurum.Items.AddRange(new object[] {
            "Aktif",
            "Pasif"});
            this.comboDurum.Location = new System.Drawing.Point(547, 125);
            this.comboDurum.Margin = new System.Windows.Forms.Padding(2);
            this.comboDurum.Name = "comboDurum";
            this.comboDurum.Size = new System.Drawing.Size(102, 21);
            this.comboDurum.TabIndex = 8;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(249, 90);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(67, 13);
            this.label14.TabIndex = 26;
            this.label14.Text = "Temsilci Kod";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(249, 68);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Kurum Şube";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(249, 45);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "Kurum No";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(249, 23);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "Telefon";
            // 
            // txtTemsilciKodu
            // 
            this.txtTemsilciKodu.Location = new System.Drawing.Point(317, 87);
            this.txtTemsilciKodu.Margin = new System.Windows.Forms.Padding(2);
            this.txtTemsilciKodu.Name = "txtTemsilciKodu";
            this.txtTemsilciKodu.Size = new System.Drawing.Size(127, 20);
            this.txtTemsilciKodu.TabIndex = 22;
            // 
            // txtKurumNo
            // 
            this.txtKurumNo.Location = new System.Drawing.Point(317, 42);
            this.txtKurumNo.Margin = new System.Windows.Forms.Padding(2);
            this.txtKurumNo.Name = "txtKurumNo";
            this.txtKurumNo.Size = new System.Drawing.Size(127, 20);
            this.txtKurumNo.TabIndex = 24;
            // 
            // txtTelefon
            // 
            this.txtTelefon.Location = new System.Drawing.Point(317, 20);
            this.txtTelefon.Margin = new System.Windows.Forms.Padding(2);
            this.txtTelefon.Name = "txtTelefon";
            this.txtTelefon.Size = new System.Drawing.Size(127, 20);
            this.txtTelefon.TabIndex = 25;
            // 
            // cmbCalisanSube
            // 
            this.cmbCalisanSube.FormattingEnabled = true;
            this.cmbCalisanSube.Location = new System.Drawing.Point(317, 64);
            this.cmbCalisanSube.Name = "cmbCalisanSube";
            this.cmbCalisanSube.Size = new System.Drawing.Size(127, 21);
            this.cmbCalisanSube.TabIndex = 30;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(18, 149);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(23, 13);
            this.label13.TabIndex = 31;
            this.label13.Text = "Ara";
            // 
            // txtAra
            // 
            this.txtAra.Location = new System.Drawing.Point(73, 146);
            this.txtAra.Name = "txtAra";
            this.txtAra.Size = new System.Drawing.Size(161, 20);
            this.txtAra.TabIndex = 101;
            this.txtAra.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAra_KeyPress);
            // 
            // chkBloke
            // 
            this.chkBloke.AutoSize = true;
            this.chkBloke.Location = new System.Drawing.Point(471, 20);
            this.chkBloke.Name = "chkBloke";
            this.chkBloke.Size = new System.Drawing.Size(127, 17);
            this.chkBloke.TabIndex = 102;
            this.chkBloke.Text = "Hesap Bloke Durumu";
            this.chkBloke.UseVisualStyleBackColor = true;
            // 
            // chbTumCalisanlar
            // 
            this.chbTumCalisanlar.AutoSize = true;
            this.chbTumCalisanlar.Checked = true;
            this.chbTumCalisanlar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbTumCalisanlar.Location = new System.Drawing.Point(30, 13);
            this.chbTumCalisanlar.Name = "chbTumCalisanlar";
            this.chbTumCalisanlar.Size = new System.Drawing.Size(95, 17);
            this.chbTumCalisanlar.TabIndex = 103;
            this.chbTumCalisanlar.Text = "Tüm Çalışanlar";
            this.chbTumCalisanlar.UseVisualStyleBackColor = true;
            this.chbTumCalisanlar.CheckedChanged += new System.EventHandler(this.chbTumCalisanlar_CheckedChanged);
            // 
            // chbAktif
            // 
            this.chbAktif.AutoSize = true;
            this.chbAktif.Location = new System.Drawing.Point(131, 13);
            this.chbAktif.Name = "chbAktif";
            this.chbAktif.Size = new System.Drawing.Size(47, 17);
            this.chbAktif.TabIndex = 104;
            this.chbAktif.Text = "Aktif";
            this.chbAktif.UseVisualStyleBackColor = true;
            this.chbAktif.CheckedChanged += new System.EventHandler(this.chbAktif_CheckedChanged);
            // 
            // gbFiltrele
            // 
            this.gbFiltrele.Controls.Add(this.chbPasif);
            this.gbFiltrele.Controls.Add(this.chbTumCalisanlar);
            this.gbFiltrele.Controls.Add(this.chbAktif);
            this.gbFiltrele.Location = new System.Drawing.Point(465, 162);
            this.gbFiltrele.Name = "gbFiltrele";
            this.gbFiltrele.Size = new System.Drawing.Size(259, 34);
            this.gbFiltrele.TabIndex = 106;
            this.gbFiltrele.TabStop = false;
            this.gbFiltrele.Text = "Filtrele";
            // 
            // chbPasif
            // 
            this.chbPasif.AutoSize = true;
            this.chbPasif.Location = new System.Drawing.Point(185, 13);
            this.chbPasif.Name = "chbPasif";
            this.chbPasif.Size = new System.Drawing.Size(49, 17);
            this.chbPasif.TabIndex = 105;
            this.chbPasif.Text = "Pasif";
            this.chbPasif.UseVisualStyleBackColor = true;
            this.chbPasif.CheckedChanged += new System.EventHandler(this.chbPasif_CheckedChanged);
            // 
            // formCalisancs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 473);
            this.Controls.Add(this.gbFiltrele);
            this.Controls.Add(this.chkBloke);
            this.Controls.Add(this.txtAra);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cmbCalisanSube);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtTemsilciKodu);
            this.Controls.Add(this.txtKurumNo);
            this.Controls.Add(this.txtTelefon);
            this.Controls.Add(this.comboDurum);
            this.Controls.Add(this.comboType);
            this.Controls.Add(this.comboYetki);
            this.Controls.Add(this.comboDepartman);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMail);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtSoyad);
            this.Controls.Add(this.txtAdi);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "formCalisancs";
            this.Text = "formCalisancs";
            this.Load += new System.EventHandler(this.formCalisancs_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.formCalisancs_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.gbFiltrele.ResumeLayout(false);
            this.gbFiltrele.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtAdi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSoyad;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.ComboBox comboDepartman;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboYetki;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboDurum;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtTemsilciKodu;
        private System.Windows.Forms.TextBox txtKurumNo;
        private System.Windows.Forms.TextBox txtTelefon;
        private System.Windows.Forms.ComboBox cmbCalisanSube;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtAra;
        private System.Windows.Forms.CheckBox chkBloke;
        private System.Windows.Forms.CheckBox chbTumCalisanlar;
        private System.Windows.Forms.CheckBox chbAktif;
        private System.Windows.Forms.GroupBox gbFiltrele;
        private System.Windows.Forms.CheckBox chbPasif;
    }
}