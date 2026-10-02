namespace DirectFNCRM.AdminViews
{
    partial class formProjeler
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
            this.btnSave = new System.Windows.Forms.Button();
            this.txtPmtsno = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtKullaniciAd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAciklama = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFiyat = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.gboxBorsaLisans = new System.Windows.Forms.GroupBox();
            this.chkGUYEPVA = new System.Windows.Forms.CheckBox();
            this.chkGKULPVA = new System.Windows.Forms.CheckBox();
            this.chkGUYEEND = new System.Windows.Forms.CheckBox();
            this.chkGUYED1P = new System.Windows.Forms.CheckBox();
            this.chkGKULEND = new System.Windows.Forms.CheckBox();
            this.chkGUYED2 = new System.Windows.Forms.CheckBox();
            this.chkGKULD1P = new System.Windows.Forms.CheckBox();
            this.chkGKULD2 = new System.Windows.Forms.CheckBox();
            this.comboYayinDurumu = new System.Windows.Forms.ComboBox();
            this.txtkriter3 = new System.Windows.Forms.TextBox();
            this.txtkriter2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtkriter1 = new System.Windows.Forms.TextBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.comboSozlesme = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboProjeUrun = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.gboxBorsaLisans.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(3, 267);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 20;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(849, 382);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(690, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(138, 33);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtPmtsno
            // 
            this.txtPmtsno.Location = new System.Drawing.Point(132, 54);
            this.txtPmtsno.Name = "txtPmtsno";
            this.txtPmtsno.Size = new System.Drawing.Size(121, 22);
            this.txtPmtsno.TabIndex = 0;
            this.txtPmtsno.Leave += new System.EventHandler(this.txtPmtsno_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "PmtsNo";
            // 
            // txtKullaniciAd
            // 
            this.txtKullaniciAd.Location = new System.Drawing.Point(132, 108);
            this.txtKullaniciAd.Name = "txtKullaniciAd";
            this.txtKullaniciAd.Size = new System.Drawing.Size(365, 22);
            this.txtKullaniciAd.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Kullanıcı Adı";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Proje Adı";
            // 
            // txtAciklama
            // 
            this.txtAciklama.Location = new System.Drawing.Point(132, 167);
            this.txtAciklama.Name = "txtAciklama";
            this.txtAciklama.Size = new System.Drawing.Size(365, 22);
            this.txtAciklama.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Açıklama";
            // 
            // txtFiyat
            // 
            this.txtFiyat.Location = new System.Drawing.Point(132, 81);
            this.txtFiyat.Name = "txtFiyat";
            this.txtFiyat.Size = new System.Drawing.Size(121, 22);
            this.txtFiyat.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Fiyat";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(259, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 17);
            this.label6.TabIndex = 6;
            this.label6.Text = "Başlangıç Tarihi";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(259, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 17);
            this.label7.TabIndex = 6;
            this.label7.Text = "Bitiş Tarihi";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(376, 79);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(121, 24);
            this.dtpEndDate.TabIndex = 18;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(376, 50);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(121, 24);
            this.dtpStartDate.TabIndex = 18;
            // 
            // gboxBorsaLisans
            // 
            this.gboxBorsaLisans.Controls.Add(this.chkGUYEPVA);
            this.gboxBorsaLisans.Controls.Add(this.chkGKULPVA);
            this.gboxBorsaLisans.Controls.Add(this.chkGUYEEND);
            this.gboxBorsaLisans.Controls.Add(this.chkGUYED1P);
            this.gboxBorsaLisans.Controls.Add(this.chkGKULEND);
            this.gboxBorsaLisans.Controls.Add(this.chkGUYED2);
            this.gboxBorsaLisans.Controls.Add(this.chkGKULD1P);
            this.gboxBorsaLisans.Controls.Add(this.chkGKULD2);
            this.gboxBorsaLisans.Location = new System.Drawing.Point(561, 54);
            this.gboxBorsaLisans.Name = "gboxBorsaLisans";
            this.gboxBorsaLisans.Size = new System.Drawing.Size(251, 125);
            this.gboxBorsaLisans.TabIndex = 19;
            this.gboxBorsaLisans.TabStop = false;
            this.gboxBorsaLisans.Text = "Borsa Lisansları";
            // 
            // chkGUYEPVA
            // 
            this.chkGUYEPVA.AutoSize = true;
            this.chkGUYEPVA.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkGUYEPVA.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkGUYEPVA.Location = new System.Drawing.Point(129, 89);
            this.chkGUYEPVA.Name = "chkGUYEPVA";
            this.chkGUYEPVA.Size = new System.Drawing.Size(94, 21);
            this.chkGUYEPVA.TabIndex = 34;
            this.chkGUYEPVA.Text = "GUYEPVA";
            this.chkGUYEPVA.UseVisualStyleBackColor = true;
            // 
            // chkGKULPVA
            // 
            this.chkGKULPVA.AutoSize = true;
            this.chkGKULPVA.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkGKULPVA.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkGKULPVA.Location = new System.Drawing.Point(13, 89);
            this.chkGKULPVA.Name = "chkGKULPVA";
            this.chkGKULPVA.Size = new System.Drawing.Size(93, 21);
            this.chkGKULPVA.TabIndex = 34;
            this.chkGKULPVA.Text = "GKULPVA";
            this.chkGKULPVA.UseVisualStyleBackColor = true;
            // 
            // chkGUYEEND
            // 
            this.chkGUYEEND.AutoSize = true;
            this.chkGUYEEND.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkGUYEEND.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkGUYEEND.Location = new System.Drawing.Point(129, 68);
            this.chkGUYEEND.Name = "chkGUYEEND";
            this.chkGUYEEND.Size = new System.Drawing.Size(98, 21);
            this.chkGUYEEND.TabIndex = 34;
            this.chkGUYEEND.Text = "GUYEEND";
            this.chkGUYEEND.UseVisualStyleBackColor = true;
            // 
            // chkGUYED1P
            // 
            this.chkGUYED1P.AutoSize = true;
            this.chkGUYED1P.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkGUYED1P.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkGUYED1P.Location = new System.Drawing.Point(129, 21);
            this.chkGUYED1P.Name = "chkGUYED1P";
            this.chkGUYED1P.Size = new System.Drawing.Size(96, 21);
            this.chkGUYED1P.TabIndex = 34;
            this.chkGUYED1P.Text = "GUYED1P";
            this.chkGUYED1P.UseVisualStyleBackColor = true;
            // 
            // chkGKULEND
            // 
            this.chkGKULEND.AutoSize = true;
            this.chkGKULEND.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkGKULEND.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkGKULEND.Location = new System.Drawing.Point(13, 68);
            this.chkGKULEND.Name = "chkGKULEND";
            this.chkGKULEND.Size = new System.Drawing.Size(97, 21);
            this.chkGKULEND.TabIndex = 34;
            this.chkGKULEND.Text = "GKULEND";
            this.chkGKULEND.UseVisualStyleBackColor = true;
            // 
            // chkGUYED2
            // 
            this.chkGUYED2.AutoSize = true;
            this.chkGUYED2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkGUYED2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkGUYED2.Location = new System.Drawing.Point(129, 45);
            this.chkGUYED2.Name = "chkGUYED2";
            this.chkGUYED2.Size = new System.Drawing.Size(86, 21);
            this.chkGUYED2.TabIndex = 34;
            this.chkGUYED2.Text = "GUYED2";
            this.chkGUYED2.UseVisualStyleBackColor = true;
            // 
            // chkGKULD1P
            // 
            this.chkGKULD1P.AutoSize = true;
            this.chkGKULD1P.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkGKULD1P.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkGKULD1P.Location = new System.Drawing.Point(13, 21);
            this.chkGKULD1P.Name = "chkGKULD1P";
            this.chkGKULD1P.Size = new System.Drawing.Size(95, 21);
            this.chkGKULD1P.TabIndex = 34;
            this.chkGKULD1P.Text = "GKULD1P";
            this.chkGKULD1P.UseVisualStyleBackColor = true;
            // 
            // chkGKULD2
            // 
            this.chkGKULD2.AutoSize = true;
            this.chkGKULD2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkGKULD2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkGKULD2.Location = new System.Drawing.Point(13, 45);
            this.chkGKULD2.Name = "chkGKULD2";
            this.chkGKULD2.Size = new System.Drawing.Size(85, 21);
            this.chkGKULD2.TabIndex = 34;
            this.chkGKULD2.Text = "GKULD2";
            this.chkGKULD2.UseVisualStyleBackColor = true;
            // 
            // comboYayinDurumu
            // 
            this.comboYayinDurumu.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboYayinDurumu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboYayinDurumu.Items.AddRange(new object[] {
            "AÇIK",
            "KAPALI"});
            this.comboYayinDurumu.Location = new System.Drawing.Point(544, 3);
            this.comboYayinDurumu.Name = "comboYayinDurumu";
            this.comboYayinDurumu.Size = new System.Drawing.Size(127, 26);
            this.comboYayinDurumu.TabIndex = 25;
            this.comboYayinDurumu.SelectedIndexChanged += new System.EventHandler(this.comboYayinDurumu_SelectedIndexChanged);
            // 
            // txtkriter3
            // 
            this.txtkriter3.BackColor = System.Drawing.Color.PowderBlue;
            this.txtkriter3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtkriter3.Location = new System.Drawing.Point(302, 12);
            this.txtkriter3.Name = "txtkriter3";
            this.txtkriter3.Size = new System.Drawing.Size(102, 22);
            this.txtkriter3.TabIndex = 124;
            this.txtkriter3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtkriter3_KeyPress);
            // 
            // txtkriter2
            // 
            this.txtkriter2.BackColor = System.Drawing.Color.Yellow;
            this.txtkriter2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtkriter2.Location = new System.Drawing.Point(194, 12);
            this.txtkriter2.Name = "txtkriter2";
            this.txtkriter2.Size = new System.Drawing.Size(102, 22);
            this.txtkriter2.TabIndex = 123;
            this.txtkriter2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtkriter2_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label8.Location = new System.Drawing.Point(23, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 17);
            this.label8.TabIndex = 125;
            this.label8.Text = "Filtreler";
            // 
            // txtkriter1
            // 
            this.txtkriter1.BackColor = System.Drawing.Color.Lime;
            this.txtkriter1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtkriter1.Location = new System.Drawing.Point(84, 12);
            this.txtkriter1.Name = "txtkriter1";
            this.txtkriter1.Size = new System.Drawing.Size(104, 22);
            this.txtkriter1.TabIndex = 122;
            this.txtkriter1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtkriter1_KeyPress);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(437, 3);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(94, 33);
            this.btnNew.TabIndex = 126;
            this.btnNew.Text = "Yeni";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // comboSozlesme
            // 
            this.comboSozlesme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSozlesme.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboSozlesme.FormattingEnabled = true;
            this.comboSozlesme.Location = new System.Drawing.Point(132, 195);
            this.comboSozlesme.Name = "comboSozlesme";
            this.comboSozlesme.Size = new System.Drawing.Size(118, 24);
            this.comboSozlesme.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 198);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 17);
            this.label9.TabIndex = 6;
            this.label9.Text = "Sözleşme";
            // 
            // comboProjeUrun
            // 
            this.comboProjeUrun.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboProjeUrun.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboProjeUrun.FormattingEnabled = true;
            this.comboProjeUrun.Location = new System.Drawing.Point(132, 137);
            this.comboProjeUrun.Name = "comboProjeUrun";
            this.comboProjeUrun.Size = new System.Drawing.Size(365, 24);
            this.comboProjeUrun.TabIndex = 5;
            // 
            // formProjeler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 661);
            this.Controls.Add(this.comboProjeUrun);
            this.Controls.Add(this.comboSozlesme);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.txtkriter3);
            this.Controls.Add(this.txtkriter2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtkriter1);
            this.Controls.Add(this.comboYayinDurumu);
            this.Controls.Add(this.gboxBorsaLisans);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFiyat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAciklama);
            this.Controls.Add(this.txtKullaniciAd);
            this.Controls.Add(this.txtPmtsno);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dataGridView1);
            this.Name = "formProjeler";
            this.ShowIcon = false;
            this.Text = "Projeler";
            this.Load += new System.EventHandler(this.formProjeler_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.gboxBorsaLisans.ResumeLayout(false);
            this.gboxBorsaLisans.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtPmtsno;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtKullaniciAd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAciklama;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFiyat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.GroupBox gboxBorsaLisans;
        private System.Windows.Forms.CheckBox chkGUYEPVA;
        private System.Windows.Forms.CheckBox chkGKULPVA;
        private System.Windows.Forms.CheckBox chkGUYEEND;
        private System.Windows.Forms.CheckBox chkGUYED1P;
        private System.Windows.Forms.CheckBox chkGKULEND;
        private System.Windows.Forms.CheckBox chkGUYED2;
        private System.Windows.Forms.CheckBox chkGKULD1P;
        private System.Windows.Forms.CheckBox chkGKULD2;
        private System.Windows.Forms.ComboBox comboYayinDurumu;
        private System.Windows.Forms.TextBox txtkriter3;
        private System.Windows.Forms.TextBox txtkriter2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtkriter1;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.ComboBox comboSozlesme;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboProjeUrun;
    }
}