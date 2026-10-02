
namespace DirectFNCRM.AdminViews
{
    partial class formSubeEkle
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
            this.SubeGrid = new System.Windows.Forms.DataGridView();
            this.YeniSubeTxt = new System.Windows.Forms.TextBox();
            this.btnYeniSube = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSubeGuncelle = new System.Windows.Forms.Button();
            this.btnSubeDelete = new System.Windows.Forms.Button();
            this.SubeGuncelletxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.silinecekSubeAd = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SubeGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // SubeGrid
            // 
            this.SubeGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.SubeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SubeGrid.Location = new System.Drawing.Point(12, 26);
            this.SubeGrid.Name = "SubeGrid";
            this.SubeGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.SubeGrid.Size = new System.Drawing.Size(271, 412);
            this.SubeGrid.TabIndex = 0;
            this.SubeGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SubeGrid_CellClick);
            this.SubeGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SubeGrid_CellDoubleClick);
            // 
            // YeniSubeTxt
            // 
            this.YeniSubeTxt.Location = new System.Drawing.Point(113, 31);
            this.YeniSubeTxt.Name = "YeniSubeTxt";
            this.YeniSubeTxt.Size = new System.Drawing.Size(100, 20);
            this.YeniSubeTxt.TabIndex = 1;
            // 
            // btnYeniSube
            // 
            this.btnYeniSube.Location = new System.Drawing.Point(113, 72);
            this.btnYeniSube.Name = "btnYeniSube";
            this.btnYeniSube.Size = new System.Drawing.Size(100, 23);
            this.btnYeniSube.TabIndex = 3;
            this.btnYeniSube.Text = "Yeni Şube Ekle";
            this.btnYeniSube.UseVisualStyleBackColor = true;
            this.btnYeniSube.Click += new System.EventHandler(this.btnYeniSube_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.YeniSubeTxt);
            this.groupBox1.Controls.Add(this.btnYeniSube);
            this.groupBox1.Location = new System.Drawing.Point(308, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(254, 111);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Yeni Şube";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Yeni Şube Adı:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Şube Listesi";
            // 
            // btnSubeGuncelle
            // 
            this.btnSubeGuncelle.Location = new System.Drawing.Point(45, 65);
            this.btnSubeGuncelle.Name = "btnSubeGuncelle";
            this.btnSubeGuncelle.Size = new System.Drawing.Size(168, 23);
            this.btnSubeGuncelle.TabIndex = 6;
            this.btnSubeGuncelle.Text = "Seçilen Şubeyi Güncelle";
            this.btnSubeGuncelle.UseVisualStyleBackColor = true;
            this.btnSubeGuncelle.Click += new System.EventHandler(this.btnSubeGuncelle_Click);
            // 
            // btnSubeDelete
            // 
            this.btnSubeDelete.Location = new System.Drawing.Point(45, 51);
            this.btnSubeDelete.Name = "btnSubeDelete";
            this.btnSubeDelete.Size = new System.Drawing.Size(168, 23);
            this.btnSubeDelete.TabIndex = 6;
            this.btnSubeDelete.Text = "Seçilen Şubeyi Sil";
            this.btnSubeDelete.UseVisualStyleBackColor = true;
            this.btnSubeDelete.Click += new System.EventHandler(this.btnSubeDelete_Click);
            // 
            // SubeGuncelletxt
            // 
            this.SubeGuncelletxt.Location = new System.Drawing.Point(113, 25);
            this.SubeGuncelletxt.Name = "SubeGuncelletxt";
            this.SubeGuncelletxt.Size = new System.Drawing.Size(100, 20);
            this.SubeGuncelletxt.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Şube Adı:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.SubeGuncelletxt);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnSubeGuncelle);
            this.groupBox2.Location = new System.Drawing.Point(308, 184);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(254, 106);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Şube Güncelleme";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.silinecekSubeAd);
            this.groupBox3.Controls.Add(this.btnSubeDelete);
            this.groupBox3.Location = new System.Drawing.Point(308, 321);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(254, 90);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Şube Silme";
            // 
            // silinecekSubeAd
            // 
            this.silinecekSubeAd.Dock = System.Windows.Forms.DockStyle.Top;
            this.silinecekSubeAd.ForeColor = System.Drawing.Color.Red;
            this.silinecekSubeAd.Location = new System.Drawing.Point(3, 16);
            this.silinecekSubeAd.Name = "silinecekSubeAd";
            this.silinecekSubeAd.Size = new System.Drawing.Size(248, 23);
            this.silinecekSubeAd.TabIndex = 7;
            this.silinecekSubeAd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // formSubeEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 455);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SubeGrid);
            this.Name = "formSubeEkle";
            this.Text = "Şube Ekle";
            this.Load += new System.EventHandler(this.formSubeEkle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SubeGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView SubeGrid;
        private System.Windows.Forms.TextBox YeniSubeTxt;
        private System.Windows.Forms.Button btnYeniSube;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSubeGuncelle;
        private System.Windows.Forms.Button btnSubeDelete;
        private System.Windows.Forms.TextBox SubeGuncelletxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label silinecekSubeAd;
    }
}