namespace DirectFNCRM.AdminViews
{
    partial class formSozlesmeDetay
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tbSozlesmeDetay = new System.Windows.Forms.TabControl();
            this.ekran = new System.Windows.Forms.TabPage();
            this.dgEkran = new System.Windows.Forms.DataGridView();
            this.projeler = new System.Windows.Forms.TabPage();
            this.dgProje = new System.Windows.Forms.DataGridView();
            this.lblunvan = new System.Windows.Forms.Label();
            this.lblMusterino = new System.Windows.Forms.Label();
            this.lblSozlesmeSayisi = new System.Windows.Forms.Label();
            this.gridDurumOzet = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Alan2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deger2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Alan3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Deger3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbSozlesmeDetay.SuspendLayout();
            this.ekran.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEkran)).BeginInit();
            this.projeler.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgProje)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDurumOzet)).BeginInit();
            this.SuspendLayout();
            // 
            // tbSozlesmeDetay
            // 
            this.tbSozlesmeDetay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSozlesmeDetay.Controls.Add(this.ekran);
            this.tbSozlesmeDetay.Controls.Add(this.projeler);
            this.tbSozlesmeDetay.Location = new System.Drawing.Point(8, 248);
            this.tbSozlesmeDetay.Name = "tbSozlesmeDetay";
            this.tbSozlesmeDetay.SelectedIndex = 0;
            this.tbSozlesmeDetay.Size = new System.Drawing.Size(864, 566);
            this.tbSozlesmeDetay.TabIndex = 1;
            this.tbSozlesmeDetay.SelectedIndexChanged += new System.EventHandler(this.tbSozlesmeDetay_SelectedIndexChanged);
            // 
            // ekran
            // 
            this.ekran.Controls.Add(this.dgEkran);
            this.ekran.Location = new System.Drawing.Point(4, 25);
            this.ekran.Name = "ekran";
            this.ekran.Padding = new System.Windows.Forms.Padding(3);
            this.ekran.Size = new System.Drawing.Size(856, 537);
            this.ekran.TabIndex = 0;
            this.ekran.Text = "Pro ve Cep Ekranlar";
            this.ekran.UseVisualStyleBackColor = true;
            // 
            // dgEkran
            // 
            this.dgEkran.AllowUserToAddRows = false;
            this.dgEkran.AllowUserToDeleteRows = false;
            this.dgEkran.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgEkran.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEkran.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgEkran.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgEkran.Location = new System.Drawing.Point(3, 3);
            this.dgEkran.Margin = new System.Windows.Forms.Padding(0);
            this.dgEkran.Name = "dgEkran";
            this.dgEkran.ReadOnly = true;
            this.dgEkran.RowHeadersWidth = 25;
            this.dgEkran.RowTemplate.Height = 20;
            this.dgEkran.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgEkran.Size = new System.Drawing.Size(850, 531);
            this.dgEkran.TabIndex = 18;
            this.dgEkran.DoubleClick += new System.EventHandler(this.dgEkran_DoubleClick);
            // 
            // projeler
            // 
            this.projeler.Controls.Add(this.dgProje);
            this.projeler.Location = new System.Drawing.Point(4, 25);
            this.projeler.Name = "projeler";
            this.projeler.Padding = new System.Windows.Forms.Padding(3);
            this.projeler.Size = new System.Drawing.Size(856, 537);
            this.projeler.TabIndex = 1;
            this.projeler.Text = "Projeler";
            this.projeler.UseVisualStyleBackColor = true;
            // 
            // dgProje
            // 
            this.dgProje.AllowUserToAddRows = false;
            this.dgProje.AllowUserToDeleteRows = false;
            this.dgProje.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgProje.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgProje.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProje.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgProje.Location = new System.Drawing.Point(3, 3);
            this.dgProje.Margin = new System.Windows.Forms.Padding(0);
            this.dgProje.Name = "dgProje";
            this.dgProje.ReadOnly = true;
            this.dgProje.RowHeadersWidth = 25;
            this.dgProje.RowTemplate.Height = 20;
            this.dgProje.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgProje.Size = new System.Drawing.Size(701, 545);
            this.dgProje.TabIndex = 19;
            this.dgProje.DoubleClick += new System.EventHandler(this.dgProje_DoubleClick);
            // 
            // lblunvan
            // 
            this.lblunvan.AutoSize = true;
            this.lblunvan.BackColor = System.Drawing.SystemColors.Control;
            this.lblunvan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblunvan.ForeColor = System.Drawing.Color.Firebrick;
            this.lblunvan.Location = new System.Drawing.Point(310, 248);
            this.lblunvan.Name = "lblunvan";
            this.lblunvan.Size = new System.Drawing.Size(0, 20);
            this.lblunvan.TabIndex = 115;
            this.lblunvan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMusterino
            // 
            this.lblMusterino.AutoSize = true;
            this.lblMusterino.BackColor = System.Drawing.SystemColors.Control;
            this.lblMusterino.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblMusterino.ForeColor = System.Drawing.Color.Firebrick;
            this.lblMusterino.Location = new System.Drawing.Point(17, 50);
            this.lblMusterino.Name = "lblMusterino";
            this.lblMusterino.Size = new System.Drawing.Size(0, 20);
            this.lblMusterino.TabIndex = 115;
            this.lblMusterino.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSozlesmeSayisi
            // 
            this.lblSozlesmeSayisi.AutoSize = true;
            this.lblSozlesmeSayisi.BackColor = System.Drawing.SystemColors.Control;
            this.lblSozlesmeSayisi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSozlesmeSayisi.ForeColor = System.Drawing.Color.Firebrick;
            this.lblSozlesmeSayisi.Location = new System.Drawing.Point(219, 252);
            this.lblSozlesmeSayisi.Name = "lblSozlesmeSayisi";
            this.lblSozlesmeSayisi.Size = new System.Drawing.Size(17, 18);
            this.lblSozlesmeSayisi.TabIndex = 115;
            this.lblSozlesmeSayisi.Text = "0";
            this.lblSozlesmeSayisi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gridDurumOzet
            // 
            this.gridDurumOzet.AllowUserToAddRows = false;
            this.gridDurumOzet.AllowUserToDeleteRows = false;
            this.gridDurumOzet.AllowUserToResizeColumns = false;
            this.gridDurumOzet.AllowUserToResizeRows = false;
            this.gridDurumOzet.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.gridDurumOzet.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridDurumOzet.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gridDurumOzet.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.LightPink;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDurumOzet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.gridDurumOzet.ColumnHeadersHeight = 22;
            this.gridDurumOzet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridDurumOzet.ColumnHeadersVisible = false;
            this.gridDurumOzet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.dataGridViewTextBoxColumn3,
            this.Alan2,
            this.deger2,
            this.Alan3,
            this.Deger3});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridDurumOzet.DefaultCellStyle = dataGridViewCellStyle9;
            this.gridDurumOzet.GridColor = System.Drawing.Color.Gainsboro;
            this.gridDurumOzet.Location = new System.Drawing.Point(8, 2);
            this.gridDurumOzet.Margin = new System.Windows.Forms.Padding(0);
            this.gridDurumOzet.MultiSelect = false;
            this.gridDurumOzet.Name = "gridDurumOzet";
            this.gridDurumOzet.ReadOnly = true;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.Format = "N0";
            dataGridViewCellStyle10.NullValue = null;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDurumOzet.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.gridDurumOzet.RowHeadersVisible = false;
            this.gridDurumOzet.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridDurumOzet.RowTemplate.Height = 18;
            this.gridDurumOzet.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gridDurumOzet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.ColumnHeaderSelect;
            this.gridDurumOzet.Size = new System.Drawing.Size(860, 208);
            this.gridDurumOzet.TabIndex = 116;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle7.NullValue = null;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn2.HeaderText = "Alan";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 50;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Column1.Width = 35;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Column2.Width = 50;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Column3.Width = 35;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Column4";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Column4.Width = 50;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Column5";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Column5.Width = 35;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Column6";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Column6.Width = 75;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle8.NullValue = null;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn3.HeaderText = "Değer";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dataGridViewTextBoxColumn3.Width = 35;
            // 
            // Alan2
            // 
            this.Alan2.HeaderText = "Alan2";
            this.Alan2.Name = "Alan2";
            this.Alan2.ReadOnly = true;
            this.Alan2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Alan2.Width = 55;
            // 
            // deger2
            // 
            this.deger2.HeaderText = "deger2";
            this.deger2.Name = "deger2";
            this.deger2.ReadOnly = true;
            this.deger2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.deger2.Width = 35;
            // 
            // Alan3
            // 
            this.Alan3.HeaderText = "Alan3";
            this.Alan3.Name = "Alan3";
            this.Alan3.ReadOnly = true;
            this.Alan3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Alan3.Width = 90;
            // 
            // Deger3
            // 
            this.Deger3.HeaderText = "Deger3";
            this.Deger3.Name = "Deger3";
            this.Deger3.ReadOnly = true;
            this.Deger3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Deger3.Width = 85;
            // 
            // formSozlesmeDetay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 798);
            this.Controls.Add(this.gridDurumOzet);
            this.Controls.Add(this.lblSozlesmeSayisi);
            this.Controls.Add(this.lblMusterino);
            this.Controls.Add(this.lblunvan);
            this.Controls.Add(this.tbSozlesmeDetay);
            this.Name = "formSozlesmeDetay";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sözleşme Detayları";
            this.Load += new System.EventHandler(this.formSozlesmeDetay_Load);
            this.tbSozlesmeDetay.ResumeLayout(false);
            this.ekran.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgEkran)).EndInit();
            this.projeler.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgProje)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDurumOzet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tbSozlesmeDetay;
        private System.Windows.Forms.TabPage ekran;
        private System.Windows.Forms.DataGridView dgEkran;
        private System.Windows.Forms.TabPage projeler;
        private System.Windows.Forms.DataGridView dgProje;
        private System.Windows.Forms.Label lblunvan;
        private System.Windows.Forms.Label lblMusterino;
        private System.Windows.Forms.Label lblSozlesmeSayisi;
        private System.Windows.Forms.DataGridView gridDurumOzet;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alan2;
        private System.Windows.Forms.DataGridViewTextBoxColumn deger2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alan3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Deger3;
    }
}