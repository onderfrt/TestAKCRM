
namespace DirectFNCRM.AdminViews
{
    partial class formKurumTalepleri
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
            this.btnSecimleriTemizle = new System.Windows.Forms.Button();
            this.comboTalebiGiren = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboDurum = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAra = new System.Windows.Forms.Button();
            this.txtKriter = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSecimleriTemizle
            // 
            this.btnSecimleriTemizle.Location = new System.Drawing.Point(721, 19);
            this.btnSecimleriTemizle.Name = "btnSecimleriTemizle";
            this.btnSecimleriTemizle.Size = new System.Drawing.Size(75, 39);
            this.btnSecimleriTemizle.TabIndex = 72;
            this.btnSecimleriTemizle.Text = "Filtreleri Temizle";
            this.btnSecimleriTemizle.UseVisualStyleBackColor = true;
            this.btnSecimleriTemizle.Click += new System.EventHandler(this.btnSecimleriTemizle_Click);
            // 
            // comboTalebiGiren
            // 
            this.comboTalebiGiren.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTalebiGiren.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboTalebiGiren.FormattingEnabled = true;
            this.comboTalebiGiren.Location = new System.Drawing.Point(562, 22);
            this.comboTalebiGiren.Margin = new System.Windows.Forms.Padding(2);
            this.comboTalebiGiren.Name = "comboTalebiGiren";
            this.comboTalebiGiren.Size = new System.Drawing.Size(135, 20);
            this.comboTalebiGiren.TabIndex = 70;
            this.comboTalebiGiren.SelectionChangeCommitted += new System.EventHandler(this.comboTalebiGiren_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(494, 24);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 71;
            this.label2.Text = "Talebi Giren";
            // 
            // comboDurum
            // 
            this.comboDurum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDurum.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboDurum.FormattingEnabled = true;
            this.comboDurum.Location = new System.Drawing.Point(361, 22);
            this.comboDurum.Margin = new System.Windows.Forms.Padding(2);
            this.comboDurum.Name = "comboDurum";
            this.comboDurum.Size = new System.Drawing.Size(118, 20);
            this.comboDurum.TabIndex = 68;
            this.comboDurum.SelectionChangeCommitted += new System.EventHandler(this.comboDurum_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(289, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 69;
            this.label1.Text = "Talep Durum";
            // 
            // btnAra
            // 
            this.btnAra.Location = new System.Drawing.Point(206, 22);
            this.btnAra.Margin = new System.Windows.Forms.Padding(2);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(56, 20);
            this.btnAra.TabIndex = 67;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            this.btnAra.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btnAra_KeyPress);
            // 
            // txtKriter
            // 
            this.txtKriter.Location = new System.Drawing.Point(23, 22);
            this.txtKriter.Margin = new System.Windows.Forms.Padding(2);
            this.txtKriter.Name = "txtKriter";
            this.txtKriter.Size = new System.Drawing.Size(168, 20);
            this.txtKriter.TabIndex = 66;
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
            this.dataGridView1.Location = new System.Drawing.Point(3, 75);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 30;
            this.dataGridView1.RowTemplate.Height = 20;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1057, 493);
            this.dataGridView1.TabIndex = 65;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // formKurumTalepleri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 569);
            this.Controls.Add(this.btnSecimleriTemizle);
            this.Controls.Add(this.comboTalebiGiren);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboDurum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAra);
            this.Controls.Add(this.txtKriter);
            this.Controls.Add(this.dataGridView1);
            this.Name = "formKurumTalepleri";
            this.Text = "formKurumTalepleri";
            this.Load += new System.EventHandler(this.formKurumTalepleri_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSecimleriTemizle;
        private System.Windows.Forms.ComboBox comboTalebiGiren;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboDurum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.TextBox txtKriter;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}