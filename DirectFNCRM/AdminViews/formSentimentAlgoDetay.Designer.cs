namespace DirectFNCRM
{
    partial class formSentimentAlgoDetay
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
            this.txtBaslik = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txticerik = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtHisse = new System.Windows.Forms.TextBox();
            this.lviewSembol = new System.Windows.Forms.ListView();
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sembol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sonFiyat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.txtSonFiyat = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSembolekle = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLink = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCikar = new System.Windows.Forms.Button();
            this.lblHisseId = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblOlusturan = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblOlusturmaTarihi = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Başlık";
            // 
            // txtBaslik
            // 
            this.txtBaslik.Location = new System.Drawing.Point(58, 85);
            this.txtBaslik.Name = "txtBaslik";
            this.txtBaslik.Size = new System.Drawing.Size(768, 20);
            this.txtBaslik.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "İçerik";
            // 
            // txticerik
            // 
            this.txticerik.Location = new System.Drawing.Point(58, 111);
            this.txticerik.Multiline = true;
            this.txticerik.Name = "txticerik";
            this.txticerik.Size = new System.Drawing.Size(768, 354);
            this.txticerik.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Hisse";
            // 
            // txtHisse
            // 
            this.txtHisse.Location = new System.Drawing.Point(9, 60);
            this.txtHisse.Name = "txtHisse";
            this.txtHisse.Size = new System.Drawing.Size(99, 20);
            this.txtHisse.TabIndex = 1;
            // 
            // lviewSembol
            // 
            this.lviewSembol.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.sembol,
            this.sonFiyat});
            this.lviewSembol.FullRowSelect = true;
            this.lviewSembol.Location = new System.Drawing.Point(9, 86);
            this.lviewSembol.Name = "lviewSembol";
            this.lviewSembol.Size = new System.Drawing.Size(304, 316);
            this.lviewSembol.TabIndex = 2;
            this.lviewSembol.UseCompatibleStateImageBehavior = false;
            this.lviewSembol.View = System.Windows.Forms.View.Details;
            this.lviewSembol.DoubleClick += new System.EventHandler(this.lviewSembol_DoubleClick);
            // 
            // id
            // 
            this.id.Text = "id";
            this.id.Width = 34;
            // 
            // sembol
            // 
            this.sembol.Text = "Sembol";
            this.sembol.Width = 80;
            // 
            // sonFiyat
            // 
            this.sonFiyat.Text = "Son Fiyat";
            this.sonFiyat.Width = 80;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(117, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Son Fiyat";
            // 
            // txtSonFiyat
            // 
            this.txtSonFiyat.Location = new System.Drawing.Point(120, 60);
            this.txtSonFiyat.Name = "txtSonFiyat";
            this.txtSonFiyat.Size = new System.Drawing.Size(99, 20);
            this.txtSonFiyat.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1076, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSembolekle
            // 
            this.btnSembolekle.Location = new System.Drawing.Point(225, 58);
            this.btnSembolekle.Name = "btnSembolekle";
            this.btnSembolekle.Size = new System.Drawing.Size(48, 23);
            this.btnSembolekle.TabIndex = 3;
            this.btnSembolekle.Text = "Ekle";
            this.btnSembolekle.UseVisualStyleBackColor = true;
            this.btnSembolekle.Click += new System.EventHandler(this.btnSembolekle_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 483);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Link";
            // 
            // txtLink
            // 
            this.txtLink.Location = new System.Drawing.Point(58, 480);
            this.txtLink.Name = "txtLink";
            this.txtLink.Size = new System.Drawing.Size(768, 20);
            this.txtLink.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCikar);
            this.groupBox1.Controls.Add(this.txtSonFiyat);
            this.groupBox1.Controls.Add(this.btnSembolekle);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lviewSembol);
            this.groupBox1.Controls.Add(this.txtHisse);
            this.groupBox1.Controls.Add(this.lblHisseId);
            this.groupBox1.Location = new System.Drawing.Point(838, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 414);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sentiment Algo Sembol Tablosu";
            // 
            // btnCikar
            // 
            this.btnCikar.Location = new System.Drawing.Point(276, 58);
            this.btnCikar.Name = "btnCikar";
            this.btnCikar.Size = new System.Drawing.Size(48, 23);
            this.btnCikar.TabIndex = 4;
            this.btnCikar.Text = "Çıkar";
            this.btnCikar.UseVisualStyleBackColor = true;
            this.btnCikar.Click += new System.EventHandler(this.btnCikar_Click);
            // 
            // lblHisseId
            // 
            this.lblHisseId.AutoSize = true;
            this.lblHisseId.Location = new System.Drawing.Point(208, 29);
            this.lblHisseId.Name = "lblHisseId";
            this.lblHisseId.Size = new System.Drawing.Size(52, 13);
            this.lblHisseId.TabIndex = 0;
            this.lblHisseId.Text = "Oluşturan";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Oluşturan";
            // 
            // lblOlusturan
            // 
            this.lblOlusturan.AutoSize = true;
            this.lblOlusturan.Location = new System.Drawing.Point(110, 22);
            this.lblOlusturan.Name = "lblOlusturan";
            this.lblOlusturan.Size = new System.Drawing.Size(52, 13);
            this.lblOlusturan.TabIndex = 0;
            this.lblOlusturan.Text = "Oluşturan";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Oluşturma Tarihi";
            // 
            // lblOlusturmaTarihi
            // 
            this.lblOlusturmaTarihi.AutoSize = true;
            this.lblOlusturmaTarihi.Location = new System.Drawing.Point(110, 48);
            this.lblOlusturmaTarihi.Name = "lblOlusturmaTarihi";
            this.lblOlusturmaTarihi.Size = new System.Drawing.Size(52, 13);
            this.lblOlusturmaTarihi.TabIndex = 0;
            this.lblOlusturmaTarihi.Text = "Oluşturan";
            // 
            // formSentimentAlgoDetay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 518);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txticerik);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLink);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBaslik);
            this.Controls.Add(this.lblOlusturmaTarihi);
            this.Controls.Add(this.lblOlusturan);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Name = "formSentimentAlgoDetay";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sentiment Algo Detay";
            this.Load += new System.EventHandler(this.formSentimentAlgoDetay_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBaslik;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txticerik;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtHisse;
        private System.Windows.Forms.ListView lviewSembol;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader sembol;
        private System.Windows.Forms.ColumnHeader sonFiyat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSonFiyat;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSembolekle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLink;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblOlusturan;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblOlusturmaTarihi;
        private System.Windows.Forms.Label lblHisseId;
        private System.Windows.Forms.Button btnCikar;
    }
}