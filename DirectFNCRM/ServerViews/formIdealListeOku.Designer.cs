namespace DirectFNCRM.ServerViews
{
    partial class formIdealListeOku
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuGird = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.seciliOlanKaydiEkleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtAra = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblSayac = new System.Windows.Forms.Label();
            this.btnExceldenOku = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnXmlOku = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSqlYaz = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuGird.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuGird;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 98);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 30;
            this.dataGridView1.RowTemplate.Height = 20;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1453, 479);
            this.dataGridView1.TabIndex = 47;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // contextMenuGird
            // 
            this.contextMenuGird.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuGird.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.seciliOlanKaydiEkleToolStripMenuItem});
            this.contextMenuGird.Name = "contextMenuGird";
            this.contextMenuGird.Size = new System.Drawing.Size(227, 30);
            // 
            // seciliOlanKaydiEkleToolStripMenuItem
            // 
            this.seciliOlanKaydiEkleToolStripMenuItem.Name = "seciliOlanKaydiEkleToolStripMenuItem";
            this.seciliOlanKaydiEkleToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.seciliOlanKaydiEkleToolStripMenuItem.Text = "Secili Olan Kaydi Ekle";
            this.seciliOlanKaydiEkleToolStripMenuItem.Click += new System.EventHandler(this.seciliOlanKaydiEkleToolStripMenuItem_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(1050, 28);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(107, 32);
            this.btnBrowse.TabIndex = 49;
            this.btnBrowse.Text = "Dosya Seç";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Visible = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtAra
            // 
            this.txtAra.Location = new System.Drawing.Point(325, 28);
            this.txtAra.Name = "txtAra";
            this.txtAra.Size = new System.Drawing.Size(141, 22);
            this.txtAra.TabIndex = 50;
            this.txtAra.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAra_KeyPress);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Yayın Durumu Seçiniz",
            "Açık",
            "Kapalı"});
            this.comboBox1.Location = new System.Drawing.Point(486, 28);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(202, 24);
            this.comboBox1.TabIndex = 51;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lblSayac
            // 
            this.lblSayac.AutoSize = true;
            this.lblSayac.Location = new System.Drawing.Point(32, 67);
            this.lblSayac.Name = "lblSayac";
            this.lblSayac.Size = new System.Drawing.Size(0, 17);
            this.lblSayac.TabIndex = 52;
            // 
            // btnExceldenOku
            // 
            this.btnExceldenOku.Location = new System.Drawing.Point(22, 21);
            this.btnExceldenOku.Name = "btnExceldenOku";
            this.btnExceldenOku.Size = new System.Drawing.Size(105, 33);
            this.btnExceldenOku.TabIndex = 53;
            this.btnExceldenOku.Text = "ExceldenOku";
            this.btnExceldenOku.UseVisualStyleBackColor = true;
            this.btnExceldenOku.Click += new System.EventHandler(this.btnExceldenOku_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnXmlOku
            // 
            this.btnXmlOku.Location = new System.Drawing.Point(133, 21);
            this.btnXmlOku.Name = "btnXmlOku";
            this.btnXmlOku.Size = new System.Drawing.Size(103, 33);
            this.btnXmlOku.TabIndex = 54;
            this.btnXmlOku.Text = "XMLdenOku";
            this.btnXmlOku.UseVisualStyleBackColor = true;
            this.btnXmlOku.Click += new System.EventHandler(this.btnXmlOku_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(265, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 17);
            this.label1.TabIndex = 55;
            this.label1.Text = "ARA";
            // 
            // btnSqlYaz
            // 
            this.btnSqlYaz.Location = new System.Drawing.Point(768, 20);
            this.btnSqlYaz.Name = "btnSqlYaz";
            this.btnSqlYaz.Size = new System.Drawing.Size(144, 39);
            this.btnSqlYaz.TabIndex = 56;
            this.btnSqlYaz.Text = "Sql Yaz  Gonder";
            this.btnSqlYaz.UseVisualStyleBackColor = true;
            this.btnSqlYaz.Click += new System.EventHandler(this.btnSqlYaz_Click);
            // 
            // formIdealListeOku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1453, 577);
            this.Controls.Add(this.btnSqlYaz);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnXmlOku);
            this.Controls.Add(this.btnExceldenOku);
            this.Controls.Add(this.lblSayac);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.txtAra);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.dataGridView1);
            this.Name = "formIdealListeOku";
            this.Text = "IDeal Müşteri Listesi İşlemleri";
            this.Load += new System.EventHandler(this.formIdealListeOku_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuGird.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ContextMenuStrip contextMenuGird;
        private System.Windows.Forms.ToolStripMenuItem seciliOlanKaydiEkleToolStripMenuItem;
        private System.Windows.Forms.TextBox txtAra;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblSayac;
        private System.Windows.Forms.Button btnExceldenOku;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnXmlOku;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSqlYaz;
    }
}