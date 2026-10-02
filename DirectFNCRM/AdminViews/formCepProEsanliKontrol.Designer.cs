namespace DirectFNCRM.AdminViews
{
    partial class formCepProEsanliKontrol
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
            this.lblBaslik = new System.Windows.Forms.Label();
            this.cbxHatalar = new System.Windows.Forms.ComboBox();
            this.lblAciklama = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chkAySonuGoster = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBaslik
            // 
            this.lblBaslik.AutoSize = true;
            this.lblBaslik.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.lblBaslik.Location = new System.Drawing.Point(551, 15);
            this.lblBaslik.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(504, 39);
            this.lblBaslik.TabIndex = 1;
            this.lblBaslik.Text = "Hatalı Kullanıcı Listeleme Ekranı";
            // 
            // cbxHatalar
            // 
            this.cbxHatalar.FormattingEnabled = true;
            this.cbxHatalar.Items.AddRange(new object[] {
            "TÜM HATALI KULLANICILAR",
            "HEM CEP HEM PRO YETKİSİ OLMAYANLAR",
            "LİSANSIN BAĞLI ALT LİSANS ATAMALARINDA PROBLEM OLANLAR",
            "YAYINDURUMU AÇIK AMA EXPIRYDATE GEÇMİŞ OLANLAR",
            "EXPIRYDATE AY SONUNA SET EDİLMEMİŞ OLANLAR"});
            this.cbxHatalar.Location = new System.Drawing.Point(559, 64);
            this.cbxHatalar.Margin = new System.Windows.Forms.Padding(4);
            this.cbxHatalar.Name = "cbxHatalar";
            this.cbxHatalar.Size = new System.Drawing.Size(523, 24);
            this.cbxHatalar.TabIndex = 4;
            this.cbxHatalar.SelectedIndexChanged += new System.EventHandler(this.cbxHatalar_SelectedIndexChanged);
            // 
            // lblAciklama
            // 
            this.lblAciklama.AutoSize = true;
            this.lblAciklama.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAciklama.ForeColor = System.Drawing.Color.Red;
            this.lblAciklama.Location = new System.Drawing.Point(680, 94);
            this.lblAciklama.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAciklama.Name = "lblAciklama";
            this.lblAciklama.Size = new System.Drawing.Size(255, 20);
            this.lblAciklama.TabIndex = 5;
            this.lblAciklama.Text = "label Combobox Hata Açıklaması";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(16, 135);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1612, 587);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // chkAySonuGoster
            // 
            this.chkAySonuGoster.AutoSize = true;
            this.chkAySonuGoster.Location = new System.Drawing.Point(101, 52);
            this.chkAySonuGoster.Name = "chkAySonuGoster";
            this.chkAySonuGoster.Size = new System.Drawing.Size(318, 20);
            this.chkAySonuGoster.TabIndex = 7;
            this.chkAySonuGoster.Text = "ExpiryDate Ay Sonu Olmayan Kullanıcıları Göster";
            this.chkAySonuGoster.UseVisualStyleBackColor = true;
            // 
            // formCepProEsanliKontrol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1644, 778);
            this.Controls.Add(this.chkAySonuGoster);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblAciklama);
            this.Controls.Add(this.cbxHatalar);
            this.Controls.Add(this.lblBaslik);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "formCepProEsanliKontrol";
            this.Text = "formCepProEsanliKontrol";
            this.Load += new System.EventHandler(this.formCepProEsanliKontrol_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.ComboBox cbxHatalar;
        private System.Windows.Forms.Label lblAciklama;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox chkAySonuGoster;
    }
}