
namespace DirectFNCRM.AdminViews
{
    partial class formHisseSinyalUsers
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblToplamUser = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblAcikKullanici = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblKRMD1 = new System.Windows.Forms.Label();
            this.txtFiltre3 = new System.Windows.Forms.TextBox();
            this.txtFiltre2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFiltre1 = new System.Windows.Forms.TextBox();
            this.btnAra = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.rbTum = new System.Windows.Forms.RadioButton();
            this.rbAcik = new System.Windows.Forms.RadioButton();
            this.rbKapali = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.dataGridView1.Location = new System.Drawing.Point(11, 128);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 30;
            this.dataGridView1.RowTemplate.Height = 20;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1002, 594);
            this.dataGridView1.TabIndex = 47;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(824, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "Toplam Kullanıcı Sayısı";
            // 
            // lblToplamUser
            // 
            this.lblToplamUser.AutoSize = true;
            this.lblToplamUser.Location = new System.Drawing.Point(944, 17);
            this.lblToplamUser.Name = "lblToplamUser";
            this.lblToplamUser.Size = new System.Drawing.Size(19, 13);
            this.lblToplamUser.TabIndex = 48;
            this.lblToplamUser.Text = "----";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(828, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 48;
            this.label2.Text = "Açık Kullanıcı Sayısı";
            // 
            // lblAcikKullanici
            // 
            this.lblAcikKullanici.AutoSize = true;
            this.lblAcikKullanici.Location = new System.Drawing.Point(948, 39);
            this.lblAcikKullanici.Name = "lblAcikKullanici";
            this.lblAcikKullanici.Size = new System.Drawing.Size(19, 13);
            this.lblAcikKullanici.TabIndex = 48;
            this.lblAcikKullanici.Text = "----";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(828, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 48;
            this.label3.Text = "KarmaD1 Sayısı";
            // 
            // lblKRMD1
            // 
            this.lblKRMD1.AutoSize = true;
            this.lblKRMD1.Location = new System.Drawing.Point(948, 64);
            this.lblKRMD1.Name = "lblKRMD1";
            this.lblKRMD1.Size = new System.Drawing.Size(19, 13);
            this.lblKRMD1.TabIndex = 48;
            this.lblKRMD1.Text = "----";
            // 
            // txtFiltre3
            // 
            this.txtFiltre3.BackColor = System.Drawing.Color.PowderBlue;
            this.txtFiltre3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtFiltre3.Location = new System.Drawing.Point(224, 11);
            this.txtFiltre3.Margin = new System.Windows.Forms.Padding(2);
            this.txtFiltre3.Name = "txtFiltre3";
            this.txtFiltre3.Size = new System.Drawing.Size(78, 19);
            this.txtFiltre3.TabIndex = 51;
            this.txtFiltre3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFiltre1_KeyPress);
            // 
            // txtFiltre2
            // 
            this.txtFiltre2.BackColor = System.Drawing.Color.Yellow;
            this.txtFiltre2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtFiltre2.Location = new System.Drawing.Point(143, 11);
            this.txtFiltre2.Margin = new System.Windows.Forms.Padding(2);
            this.txtFiltre2.Name = "txtFiltre2";
            this.txtFiltre2.Size = new System.Drawing.Size(78, 19);
            this.txtFiltre2.TabIndex = 50;
            this.txtFiltre2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFiltre1_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(15, 13);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 52;
            this.label4.Text = "Filtreler";
            // 
            // txtFiltre1
            // 
            this.txtFiltre1.BackColor = System.Drawing.Color.Lime;
            this.txtFiltre1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtFiltre1.Location = new System.Drawing.Point(61, 11);
            this.txtFiltre1.Margin = new System.Windows.Forms.Padding(2);
            this.txtFiltre1.Name = "txtFiltre1";
            this.txtFiltre1.Size = new System.Drawing.Size(79, 19);
            this.txtFiltre1.TabIndex = 49;
            this.txtFiltre1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFiltre1_KeyPress);
            // 
            // btnAra
            // 
            this.btnAra.Location = new System.Drawing.Point(362, 9);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(75, 23);
            this.btnAra.TabIndex = 53;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(898, 100);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(114, 23);
            this.btnExcel.TabIndex = 54;
            this.btnExcel.Text = "Excele Aktar";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // rbTum
            // 
            this.rbTum.AutoSize = true;
            this.rbTum.Checked = true;
            this.rbTum.Location = new System.Drawing.Point(12, 100);
            this.rbTum.Name = "rbTum";
            this.rbTum.Size = new System.Drawing.Size(46, 17);
            this.rbTum.TabIndex = 55;
            this.rbTum.TabStop = true;
            this.rbTum.Text = "Tüm";
            this.rbTum.UseVisualStyleBackColor = true;
            this.rbTum.CheckedChanged += new System.EventHandler(this.rbTum_CheckedChanged);
            // 
            // rbAcik
            // 
            this.rbAcik.AutoSize = true;
            this.rbAcik.Location = new System.Drawing.Point(76, 100);
            this.rbAcik.Name = "rbAcik";
            this.rbAcik.Size = new System.Drawing.Size(97, 17);
            this.rbAcik.TabIndex = 55;
            this.rbAcik.Text = "Sadece Açıklar";
            this.rbAcik.UseVisualStyleBackColor = true;
            this.rbAcik.CheckedChanged += new System.EventHandler(this.rbTum_CheckedChanged);
            // 
            // rbKapali
            // 
            this.rbKapali.AutoSize = true;
            this.rbKapali.Location = new System.Drawing.Point(179, 100);
            this.rbKapali.Name = "rbKapali";
            this.rbKapali.Size = new System.Drawing.Size(105, 17);
            this.rbKapali.TabIndex = 55;
            this.rbKapali.Text = "Sadece Kapalılar";
            this.rbKapali.UseVisualStyleBackColor = true;
            this.rbKapali.CheckedChanged += new System.EventHandler(this.rbTum_CheckedChanged);
            // 
            // formHisseSinyalUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 733);
            this.Controls.Add(this.rbKapali);
            this.Controls.Add(this.rbAcik);
            this.Controls.Add(this.rbTum);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnAra);
            this.Controls.Add(this.txtFiltre3);
            this.Controls.Add(this.txtFiltre2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFiltre1);
            this.Controls.Add(this.lblKRMD1);
            this.Controls.Add(this.lblAcikKullanici);
            this.Controls.Add(this.lblToplamUser);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "formHisseSinyalUsers";
            this.ShowIcon = false;
            this.Text = "Hisse Sinyal Kullanıcarı";
            this.Load += new System.EventHandler(this.formHisseSinyalUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblToplamUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblAcikKullanici;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblKRMD1;
        private System.Windows.Forms.TextBox txtFiltre3;
        private System.Windows.Forms.TextBox txtFiltre2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFiltre1;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.RadioButton rbTum;
        private System.Windows.Forms.RadioButton rbAcik;
        private System.Windows.Forms.RadioButton rbKapali;
    }
}