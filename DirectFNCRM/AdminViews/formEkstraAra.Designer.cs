namespace DirectFNCRM.AdminViews
{
    partial class formEkstraAra
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formEkstraAra));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnClear = new System.Windows.Forms.Button();
            this.comboCalisan = new System.Windows.Forms.ComboBox();
            this.comboDepartman = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblcalisanadi = new System.Windows.Forms.Label();
            this.txtkriter3 = new System.Windows.Forms.TextBox();
            this.txtkriter2 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtkriter1 = new System.Windows.Forms.TextBox();
            this.lblSayac = new System.Windows.Forms.Label();
            this.lblAciklama = new System.Windows.Forms.Label();
            this.dateTimeEndDate = new System.Windows.Forms.DateTimePicker();
            this.btnTekTarih = new System.Windows.Forms.Button();
            this.panelSonKullanim = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.chxKurumsalWeb = new System.Windows.Forms.CheckBox();
            this.comboEkranType = new System.Windows.Forms.ComboBox();
            this.panelTopluExpiryDate = new System.Windows.Forms.Panel();
            this.lblSayc2 = new System.Windows.Forms.Label();
            this.brnExpriydateAta = new System.Windows.Forms.Button();
            this.dtExpriyDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.chkStatusClose = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTimeEndDate1 = new System.Windows.Forms.DateTimePicker();
            this.txtAra = new System.Windows.Forms.Button();
            this.dateTimeStartDate = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelSonKullanim.SuspendLayout();
            this.panelTopluExpiryDate.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(9, 117);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 30;
            this.dataGridView1.RowTemplate.Height = 20;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(806, 388);
            this.dataGridView1.TabIndex = 47;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(652, 5);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(77, 37);
            this.btnClear.TabIndex = 138;
            this.btnClear.Text = "Seçimleri Temizle";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Visible = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // comboCalisan
            // 
            this.comboCalisan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCalisan.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboCalisan.FormattingEnabled = true;
            this.comboCalisan.Location = new System.Drawing.Point(434, 5);
            this.comboCalisan.Margin = new System.Windows.Forms.Padding(2);
            this.comboCalisan.Name = "comboCalisan";
            this.comboCalisan.Size = new System.Drawing.Size(120, 20);
            this.comboCalisan.TabIndex = 32;
            this.comboCalisan.Visible = false;
            this.comboCalisan.SelectedIndexChanged += new System.EventHandler(this.comboCalisan_SelectedIndexChanged);
            // 
            // comboDepartman
            // 
            this.comboDepartman.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDepartman.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboDepartman.FormattingEnabled = true;
            this.comboDepartman.Location = new System.Drawing.Point(434, 30);
            this.comboDepartman.Margin = new System.Windows.Forms.Padding(2);
            this.comboDepartman.Name = "comboDepartman";
            this.comboDepartman.Size = new System.Drawing.Size(120, 20);
            this.comboDepartman.TabIndex = 32;
            this.comboDepartman.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(362, 33);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 50;
            this.label4.Text = "Departman";
            this.label4.Visible = false;
            // 
            // lblcalisanadi
            // 
            this.lblcalisanadi.AutoSize = true;
            this.lblcalisanadi.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblcalisanadi.Location = new System.Drawing.Point(362, 11);
            this.lblcalisanadi.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblcalisanadi.Name = "lblcalisanadi";
            this.lblcalisanadi.Size = new System.Drawing.Size(59, 13);
            this.lblcalisanadi.TabIndex = 50;
            this.lblcalisanadi.Text = "Çalışan Adı";
            this.lblcalisanadi.Visible = false;
            // 
            // txtkriter3
            // 
            this.txtkriter3.BackColor = System.Drawing.Color.PowderBlue;
            this.txtkriter3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtkriter3.Location = new System.Drawing.Point(225, 5);
            this.txtkriter3.Margin = new System.Windows.Forms.Padding(2);
            this.txtkriter3.Name = "txtkriter3";
            this.txtkriter3.Size = new System.Drawing.Size(78, 19);
            this.txtkriter3.TabIndex = 135;
            this.txtkriter3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtkriter3_KeyPress);
            // 
            // txtkriter2
            // 
            this.txtkriter2.BackColor = System.Drawing.Color.Yellow;
            this.txtkriter2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtkriter2.Location = new System.Drawing.Point(144, 5);
            this.txtkriter2.Margin = new System.Windows.Forms.Padding(2);
            this.txtkriter2.Name = "txtkriter2";
            this.txtkriter2.Size = new System.Drawing.Size(78, 19);
            this.txtkriter2.TabIndex = 134;
            this.txtkriter2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtkriter2_KeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label15.Location = new System.Drawing.Point(16, 7);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(40, 13);
            this.label15.TabIndex = 136;
            this.label15.Text = "Filtreler";
            // 
            // txtkriter1
            // 
            this.txtkriter1.BackColor = System.Drawing.Color.Lime;
            this.txtkriter1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtkriter1.Location = new System.Drawing.Point(62, 5);
            this.txtkriter1.Margin = new System.Windows.Forms.Padding(2);
            this.txtkriter1.Name = "txtkriter1";
            this.txtkriter1.Size = new System.Drawing.Size(79, 19);
            this.txtkriter1.TabIndex = 133;
            this.txtkriter1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtkriter1_KeyPress);
            // 
            // lblSayac
            // 
            this.lblSayac.AutoSize = true;
            this.lblSayac.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblSayac.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSayac.ForeColor = System.Drawing.Color.Firebrick;
            this.lblSayac.Location = new System.Drawing.Point(9, 93);
            this.lblSayac.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSayac.Name = "lblSayac";
            this.lblSayac.Size = new System.Drawing.Size(14, 15);
            this.lblSayac.TabIndex = 139;
            this.lblSayac.Text = "0";
            // 
            // lblAciklama
            // 
            this.lblAciklama.AutoSize = true;
            this.lblAciklama.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblAciklama.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAciklama.ForeColor = System.Drawing.Color.Firebrick;
            this.lblAciklama.Location = new System.Drawing.Point(9, 36);
            this.lblAciklama.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAciklama.Name = "lblAciklama";
            this.lblAciklama.Size = new System.Drawing.Size(0, 15);
            this.lblAciklama.TabIndex = 139;
            // 
            // dateTimeEndDate
            // 
            this.dateTimeEndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dateTimeEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeEndDate.Location = new System.Drawing.Point(2, 2);
            this.dateTimeEndDate.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimeEndDate.Name = "dateTimeEndDate";
            this.dateTimeEndDate.Size = new System.Drawing.Size(100, 21);
            this.dateTimeEndDate.TabIndex = 140;
            this.dateTimeEndDate.ValueChanged += new System.EventHandler(this.dateTimeEndDate_ValueChanged);
            // 
            // btnTekTarih
            // 
            this.btnTekTarih.Location = new System.Drawing.Point(106, 2);
            this.btnTekTarih.Margin = new System.Windows.Forms.Padding(2);
            this.btnTekTarih.Name = "btnTekTarih";
            this.btnTekTarih.Size = new System.Drawing.Size(193, 20);
            this.btnTekTarih.TabIndex = 141;
            this.btnTekTarih.Text = "Tarihinde Sona Erecekler";
            this.btnTekTarih.UseVisualStyleBackColor = true;
            this.btnTekTarih.Click += new System.EventHandler(this.btnTekTarih_Click);
            // 
            // panelSonKullanim
            // 
            this.panelSonKullanim.Controls.Add(this.label7);
            this.panelSonKullanim.Controls.Add(this.chxKurumsalWeb);
            this.panelSonKullanim.Controls.Add(this.dateTimeEndDate);
            this.panelSonKullanim.Controls.Add(this.comboEkranType);
            this.panelSonKullanim.Controls.Add(this.btnTekTarih);
            this.panelSonKullanim.Location = new System.Drawing.Point(88, 30);
            this.panelSonKullanim.Margin = new System.Windows.Forms.Padding(2);
            this.panelSonKullanim.Name = "panelSonKullanim";
            this.panelSonKullanim.Size = new System.Drawing.Size(310, 69);
            this.panelSonKullanim.TabIndex = 142;
            this.panelSonKullanim.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.Location = new System.Drawing.Point(157, 32);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 144;
            this.label7.Text = "Ekran Tip";
            this.label7.Visible = false;
            // 
            // chxKurumsalWeb
            // 
            this.chxKurumsalWeb.AutoSize = true;
            this.chxKurumsalWeb.Location = new System.Drawing.Point(2, 32);
            this.chxKurumsalWeb.Margin = new System.Windows.Forms.Padding(2);
            this.chxKurumsalWeb.Name = "chxKurumsalWeb";
            this.chxKurumsalWeb.Size = new System.Drawing.Size(134, 17);
            this.chxKurumsalWeb.TabIndex = 142;
            this.chxKurumsalWeb.Text = "Kurumsal Webler Hariç";
            this.chxKurumsalWeb.UseVisualStyleBackColor = true;
            this.chxKurumsalWeb.Visible = false;
            this.chxKurumsalWeb.CheckedChanged += new System.EventHandler(this.chxKurumsalWeb_CheckedChanged);
            // 
            // comboEkranType
            // 
            this.comboEkranType.BackColor = System.Drawing.SystemColors.Control;
            this.comboEkranType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboEkranType.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboEkranType.FormattingEnabled = true;
            this.comboEkranType.Items.AddRange(new object[] {
            "IDEAL",
            "CEP",
            "IDEAL+CEP",
            "ROBOT",
            "SCM",
            "USABLE SCM"});
            this.comboEkranType.Location = new System.Drawing.Point(211, 30);
            this.comboEkranType.Margin = new System.Windows.Forms.Padding(2);
            this.comboEkranType.Name = "comboEkranType";
            this.comboEkranType.Size = new System.Drawing.Size(89, 21);
            this.comboEkranType.TabIndex = 143;
            this.comboEkranType.Visible = false;
            this.comboEkranType.SelectedIndexChanged += new System.EventHandler(this.comboEkranType_SelectedIndexChanged);
            // 
            // panelTopluExpiryDate
            // 
            this.panelTopluExpiryDate.Controls.Add(this.lblSayc2);
            this.panelTopluExpiryDate.Controls.Add(this.brnExpriydateAta);
            this.panelTopluExpiryDate.Controls.Add(this.dtExpriyDate);
            this.panelTopluExpiryDate.Controls.Add(this.label1);
            this.panelTopluExpiryDate.Location = new System.Drawing.Point(56, 55);
            this.panelTopluExpiryDate.Margin = new System.Windows.Forms.Padding(2);
            this.panelTopluExpiryDate.Name = "panelTopluExpiryDate";
            this.panelTopluExpiryDate.Size = new System.Drawing.Size(242, 56);
            this.panelTopluExpiryDate.TabIndex = 143;
            this.panelTopluExpiryDate.Visible = false;
            // 
            // lblSayc2
            // 
            this.lblSayc2.AutoSize = true;
            this.lblSayc2.ForeColor = System.Drawing.Color.Blue;
            this.lblSayc2.Location = new System.Drawing.Point(185, 12);
            this.lblSayc2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSayc2.Name = "lblSayc2";
            this.lblSayc2.Size = new System.Drawing.Size(0, 13);
            this.lblSayc2.TabIndex = 145;
            // 
            // brnExpriydateAta
            // 
            this.brnExpriydateAta.Location = new System.Drawing.Point(109, 28);
            this.brnExpriydateAta.Margin = new System.Windows.Forms.Padding(2);
            this.brnExpriydateAta.Name = "brnExpriydateAta";
            this.brnExpriydateAta.Size = new System.Drawing.Size(99, 24);
            this.brnExpriydateAta.TabIndex = 144;
            this.brnExpriydateAta.Text = "Değiştir Gönder";
            this.brnExpriydateAta.UseVisualStyleBackColor = true;
            this.brnExpriydateAta.Click += new System.EventHandler(this.brnExpriydateAta_Click);
            // 
            // dtExpriyDate
            // 
            this.dtExpriyDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dtExpriyDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtExpriyDate.Location = new System.Drawing.Point(10, 29);
            this.dtExpriyDate.Margin = new System.Windows.Forms.Padding(2);
            this.dtExpriyDate.Name = "dtExpriyDate";
            this.dtExpriyDate.Size = new System.Drawing.Size(92, 21);
            this.dtExpriyDate.TabIndex = 141;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "Toplu Expiry Date Değiştirme İşlemi";
            this.label1.Visible = false;
            // 
            // chkStatusClose
            // 
            this.chkStatusClose.AutoSize = true;
            this.chkStatusClose.Location = new System.Drawing.Point(414, 74);
            this.chkStatusClose.Name = "chkStatusClose";
            this.chkStatusClose.Size = new System.Drawing.Size(114, 17);
            this.chkStatusClose.TabIndex = 144;
            this.chkStatusClose.Text = "Kapalı olanlar dahil";
            this.chkStatusClose.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateTimeEndDate1);
            this.groupBox1.Controls.Add(this.txtAra);
            this.groupBox1.Controls.Add(this.dateTimeStartDate);
            this.groupBox1.Location = new System.Drawing.Point(501, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(314, 47);
            this.groupBox1.TabIndex = 150;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tarih Aralığı Seçiniz..";
            this.groupBox1.Visible = false;
            // 
            // dateTimeEndDate1
            // 
            this.dateTimeEndDate1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dateTimeEndDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeEndDate1.Location = new System.Drawing.Point(100, 18);
            this.dateTimeEndDate1.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimeEndDate1.Name = "dateTimeEndDate1";
            this.dateTimeEndDate1.Size = new System.Drawing.Size(86, 21);
            this.dateTimeEndDate1.TabIndex = 147;
            this.dateTimeEndDate1.Visible = false;
            // 
            // txtAra
            // 
            this.txtAra.Location = new System.Drawing.Point(191, 16);
            this.txtAra.Name = "txtAra";
            this.txtAra.Size = new System.Drawing.Size(75, 23);
            this.txtAra.TabIndex = 148;
            this.txtAra.Text = "ARA";
            this.txtAra.UseVisualStyleBackColor = true;
            this.txtAra.Visible = false;
            this.txtAra.Click += new System.EventHandler(this.txtAra_Click);
            // 
            // dateTimeStartDate
            // 
            this.dateTimeStartDate.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dateTimeStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeStartDate.Location = new System.Drawing.Point(9, 18);
            this.dateTimeStartDate.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimeStartDate.Name = "dateTimeStartDate";
            this.dateTimeStartDate.Size = new System.Drawing.Size(88, 21);
            this.dateTimeStartDate.TabIndex = 146;
            this.dateTimeStartDate.Visible = false;
            // 
            // formEkstraAra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 514);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkStatusClose);
            this.Controls.Add(this.panelTopluExpiryDate);
            this.Controls.Add(this.panelSonKullanim);
            this.Controls.Add(this.lblAciklama);
            this.Controls.Add(this.lblSayac);
            this.Controls.Add(this.comboCalisan);
            this.Controls.Add(this.comboDepartman);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblcalisanadi);
            this.Controls.Add(this.txtkriter3);
            this.Controls.Add(this.txtkriter2);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtkriter1);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "formEkstraAra";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "formEkstraAra";
            this.Load += new System.EventHandler(this.formEkstraAra_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelSonKullanim.ResumeLayout(false);
            this.panelSonKullanim.PerformLayout();
            this.panelTopluExpiryDate.ResumeLayout(false);
            this.panelTopluExpiryDate.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ComboBox comboCalisan;
        private System.Windows.Forms.ComboBox comboDepartman;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblcalisanadi;
        private System.Windows.Forms.TextBox txtkriter3;
        private System.Windows.Forms.TextBox txtkriter2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtkriter1;
        private System.Windows.Forms.Label lblSayac;
        private System.Windows.Forms.Label lblAciklama;
        private System.Windows.Forms.DateTimePicker dateTimeEndDate;
        private System.Windows.Forms.Button btnTekTarih;
        private System.Windows.Forms.Panel panelSonKullanim;
        private System.Windows.Forms.CheckBox chxKurumsalWeb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboEkranType;
        private System.Windows.Forms.Panel panelTopluExpiryDate;
        private System.Windows.Forms.Button brnExpriydateAta;
        private System.Windows.Forms.DateTimePicker dtExpriyDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSayc2;
        private System.Windows.Forms.CheckBox chkStatusClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dateTimeEndDate1;
        private System.Windows.Forms.Button txtAra;
        private System.Windows.Forms.DateTimePicker dateTimeStartDate;
    }
}