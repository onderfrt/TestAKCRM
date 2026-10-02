namespace DirectFNCRM.AdminViews
{
    partial class formFaturaGiris
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
            this.btnSave = new System.Windows.Forms.Button();
            this.txtVerDairesi = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimeEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimeStart = new System.Windows.Forms.DateTimePicker();
            this.comboDonem = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblSozlesme = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboILCE = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboUlke = new System.Windows.Forms.ComboBox();
            this.comboIL = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.lblil = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.txtCeptel = new System.Windows.Forms.TextBox();
            this.txtTel2 = new System.Windows.Forms.TextBox();
            this.txtTel1 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtAdres = new System.Windows.Forms.TextBox();
            this.chkAdresOnay = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtVerNo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtUnvan = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(600, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(119, 45);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // txtVerDairesi
            // 
            this.txtVerDairesi.Location = new System.Drawing.Point(167, 169);
            this.txtVerDairesi.Name = "txtVerDairesi";
            this.txtVerDairesi.Size = new System.Drawing.Size(117, 22);
            this.txtVerDairesi.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "UserName";
            // 
            // dateTimeEnd
            // 
            this.dateTimeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeEnd.Location = new System.Drawing.Point(167, 144);
            this.dateTimeEnd.Name = "dateTimeEnd";
            this.dateTimeEnd.Size = new System.Drawing.Size(117, 22);
            this.dateTimeEnd.TabIndex = 55;
            // 
            // dateTimeStart
            // 
            this.dateTimeStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeStart.Location = new System.Drawing.Point(167, 91);
            this.dateTimeStart.Name = "dateTimeStart";
            this.dateTimeStart.Size = new System.Drawing.Size(117, 22);
            this.dateTimeStart.TabIndex = 56;
            // 
            // comboDonem
            // 
            this.comboDonem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDonem.FormattingEnabled = true;
            this.comboDonem.Location = new System.Drawing.Point(167, 116);
            this.comboDonem.Name = "comboDonem";
            this.comboDonem.Size = new System.Drawing.Size(117, 24);
            this.comboDonem.TabIndex = 54;
            this.comboDonem.SelectedIndexChanged += new System.EventHandler(this.comboDonem_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 17);
            this.label5.TabIndex = 53;
            this.label5.Text = "Fatura Dönemi";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 17);
            this.label2.TabIndex = 51;
            this.label2.Text = "Fatura Sonlanma Tarihi";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 17);
            this.label3.TabIndex = 52;
            this.label3.Text = "Fatura Başlangıç Tarihi";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(112, 19);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(75, 17);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "UserName";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Sözleşme No";
            // 
            // lblSozlesme
            // 
            this.lblSozlesme.AutoSize = true;
            this.lblSozlesme.Location = new System.Drawing.Point(112, 40);
            this.lblSozlesme.Name = "lblSozlesme";
            this.lblSozlesme.Size = new System.Drawing.Size(69, 17);
            this.lblSozlesme.TabIndex = 2;
            this.lblSozlesme.Text = "Sözleşme";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAdresOnay);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtAdres);
            this.groupBox1.Controls.Add(this.comboILCE);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.comboUlke);
            this.groupBox1.Controls.Add(this.comboIL);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.lblil);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtMail);
            this.groupBox1.Controls.Add(this.txtCeptel);
            this.groupBox1.Controls.Add(this.txtTel2);
            this.groupBox1.Controls.Add(this.txtTel1);
            this.groupBox1.Location = new System.Drawing.Point(290, 94);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(463, 242);
            this.groupBox1.TabIndex = 75;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fatura Adresi";
            // 
            // comboILCE
            // 
            this.comboILCE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboILCE.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboILCE.FormattingEnabled = true;
            this.comboILCE.Location = new System.Drawing.Point(74, 203);
            this.comboILCE.Name = "comboILCE";
            this.comboILCE.Size = new System.Drawing.Size(131, 24);
            this.comboILCE.TabIndex = 84;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label12.Location = new System.Drawing.Point(14, 208);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 17);
            this.label12.TabIndex = 75;
            this.label12.Text = "İLÇE";
            // 
            // comboUlke
            // 
            this.comboUlke.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUlke.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboUlke.FormattingEnabled = true;
            this.comboUlke.Location = new System.Drawing.Point(74, 149);
            this.comboUlke.Name = "comboUlke";
            this.comboUlke.Size = new System.Drawing.Size(131, 24);
            this.comboUlke.TabIndex = 82;
            this.comboUlke.SelectedIndexChanged += new System.EventHandler(this.comboUlke_SelectedIndexChanged);
            // 
            // comboIL
            // 
            this.comboIL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboIL.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboIL.FormattingEnabled = true;
            this.comboIL.Location = new System.Drawing.Point(74, 176);
            this.comboIL.Name = "comboIL";
            this.comboIL.Size = new System.Drawing.Size(131, 24);
            this.comboIL.TabIndex = 83;
            this.comboIL.SelectedIndexChanged += new System.EventHandler(this.comboIL_SelectedIndexChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label21.Location = new System.Drawing.Point(16, 155);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(36, 17);
            this.label21.TabIndex = 76;
            this.label21.Text = "Ülke";
            // 
            // lblil
            // 
            this.lblil.AutoSize = true;
            this.lblil.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblil.Location = new System.Drawing.Point(16, 184);
            this.lblil.Name = "lblil";
            this.lblil.Size = new System.Drawing.Size(19, 17);
            this.lblil.TabIndex = 77;
            this.lblil.Text = "İL";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label22.Location = new System.Drawing.Point(15, 129);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(46, 17);
            this.label22.TabIndex = 88;
            this.label22.Text = "e-mail";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label11.Location = new System.Drawing.Point(14, 105);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 17);
            this.label11.TabIndex = 87;
            this.label11.Text = "CepTel";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label10.Location = new System.Drawing.Point(15, 81);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 17);
            this.label10.TabIndex = 86;
            this.label10.Text = "Tel2";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label9.Location = new System.Drawing.Point(15, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 17);
            this.label9.TabIndex = 85;
            this.label9.Text = "Tel1";
            // 
            // txtMail
            // 
            this.txtMail.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtMail.Location = new System.Drawing.Point(74, 124);
            this.txtMail.Name = "txtMail";
            this.txtMail.Size = new System.Drawing.Size(131, 22);
            this.txtMail.TabIndex = 81;
            // 
            // txtCeptel
            // 
            this.txtCeptel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtCeptel.Location = new System.Drawing.Point(74, 100);
            this.txtCeptel.Name = "txtCeptel";
            this.txtCeptel.Size = new System.Drawing.Size(131, 22);
            this.txtCeptel.TabIndex = 80;
            // 
            // txtTel2
            // 
            this.txtTel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtTel2.Location = new System.Drawing.Point(74, 76);
            this.txtTel2.Name = "txtTel2";
            this.txtTel2.Size = new System.Drawing.Size(131, 22);
            this.txtTel2.TabIndex = 79;
            // 
            // txtTel1
            // 
            this.txtTel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtTel1.Location = new System.Drawing.Point(74, 52);
            this.txtTel1.Name = "txtTel1";
            this.txtTel1.Size = new System.Drawing.Size(131, 22);
            this.txtTel1.TabIndex = 78;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label13.Location = new System.Drawing.Point(211, 52);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(45, 17);
            this.label13.TabIndex = 89;
            this.label13.Text = "Adres";
            // 
            // txtAdres
            // 
            this.txtAdres.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtAdres.Location = new System.Drawing.Point(214, 72);
            this.txtAdres.Multiline = true;
            this.txtAdres.Name = "txtAdres";
            this.txtAdres.Size = new System.Drawing.Size(202, 155);
            this.txtAdres.TabIndex = 90;
            // 
            // chkAdresOnay
            // 
            this.chkAdresOnay.AutoSize = true;
            this.chkAdresOnay.Location = new System.Drawing.Point(19, 27);
            this.chkAdresOnay.Name = "chkAdresOnay";
            this.chkAdresOnay.Size = new System.Drawing.Size(206, 21);
            this.chkAdresOnay.TabIndex = 76;
            this.chkAdresOnay.Text = "Montaj Adresi İle Aynı Adres";
            this.chkAdresOnay.UseVisualStyleBackColor = true;
            this.chkAdresOnay.CheckedChanged += new System.EventHandler(this.chkAdresOnay_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "Vergi Dairesi";
            // 
            // txtVerNo
            // 
            this.txtVerNo.Location = new System.Drawing.Point(167, 193);
            this.txtVerNo.Name = "txtVerNo";
            this.txtVerNo.Size = new System.Drawing.Size(117, 22);
            this.txtVerNo.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 196);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "Vergi Numarası";
            // 
            // txtUnvan
            // 
            this.txtUnvan.Location = new System.Drawing.Point(167, 66);
            this.txtUnvan.Name = "txtUnvan";
            this.txtUnvan.Size = new System.Drawing.Size(365, 22);
            this.txtUnvan.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 17);
            this.label8.TabIndex = 2;
            this.label8.Text = "Adı / Ünvanı";
            // 
            // formFaturaGiris
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 356);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dateTimeEnd);
            this.Controls.Add(this.dateTimeStart);
            this.Controls.Add(this.comboDonem);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.lblSozlesme);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtUnvan);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtVerNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtVerDairesi);
            this.Controls.Add(this.btnSave);
            this.Name = "formFaturaGiris";
            this.Text = "formFaturaGiris";
            this.Load += new System.EventHandler(this.formFaturaGiris_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtVerDairesi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimeEnd;
        private System.Windows.Forms.DateTimePicker dateTimeStart;
        private System.Windows.Forms.ComboBox comboDonem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblSozlesme;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAdresOnay;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtAdres;
        private System.Windows.Forms.ComboBox comboILCE;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboUlke;
        private System.Windows.Forms.ComboBox comboIL;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label lblil;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.TextBox txtCeptel;
        private System.Windows.Forms.TextBox txtTel2;
        private System.Windows.Forms.TextBox txtTel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtVerNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtUnvan;
        private System.Windows.Forms.Label label8;
    }
}