namespace DirectFNCRM.AdminViews
{
    partial class formNotAra
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
            this.txtkriter3 = new System.Windows.Forms.TextBox();
            this.txtkriter2 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtkriter1 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboCalisan = new System.Windows.Forms.ComboBox();
            this.comboDepartman = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn2TarihAra = new System.Windows.Forms.Button();
            this.dateTimeStartDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimeEndDate = new System.Windows.Forms.DateTimePicker();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtkriter3
            // 
            this.txtkriter3.BackColor = System.Drawing.Color.PowderBlue;
            this.txtkriter3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtkriter3.Location = new System.Drawing.Point(308, 24);
            this.txtkriter3.Name = "txtkriter3";
            this.txtkriter3.Size = new System.Drawing.Size(102, 22);
            this.txtkriter3.TabIndex = 124;
            this.txtkriter3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtkriter3_KeyPress);
            // 
            // txtkriter2
            // 
            this.txtkriter2.BackColor = System.Drawing.Color.Yellow;
            this.txtkriter2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtkriter2.Location = new System.Drawing.Point(200, 24);
            this.txtkriter2.Name = "txtkriter2";
            this.txtkriter2.Size = new System.Drawing.Size(102, 22);
            this.txtkriter2.TabIndex = 123;
            this.txtkriter2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtkriter2_KeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label15.Location = new System.Drawing.Point(29, 27);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(55, 17);
            this.label15.TabIndex = 125;
            this.label15.Text = "Filtreler";
            // 
            // txtkriter1
            // 
            this.txtkriter1.BackColor = System.Drawing.Color.Lime;
            this.txtkriter1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtkriter1.Location = new System.Drawing.Point(90, 24);
            this.txtkriter1.Name = "txtkriter1";
            this.txtkriter1.Size = new System.Drawing.Size(104, 22);
            this.txtkriter1.TabIndex = 122;
            this.txtkriter1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtkriter1_KeyPress);
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
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(0, 177);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 25;
            this.dataGridView1.RowTemplate.Height = 20;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1014, 572);
            this.dataGridView1.TabIndex = 126;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboCalisan);
            this.groupBox2.Controls.Add(this.comboDepartman);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(32, 62);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(289, 93);
            this.groupBox2.TabIndex = 127;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Çalışan Bilgileri ile ara";
            // 
            // comboCalisan
            // 
            this.comboCalisan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCalisan.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboCalisan.FormattingEnabled = true;
            this.comboCalisan.Location = new System.Drawing.Point(104, 23);
            this.comboCalisan.Name = "comboCalisan";
            this.comboCalisan.Size = new System.Drawing.Size(159, 25);
            this.comboCalisan.TabIndex = 32;
            this.comboCalisan.SelectedIndexChanged += new System.EventHandler(this.comboCalisan_SelectedIndexChanged);
            // 
            // comboDepartman
            // 
            this.comboDepartman.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDepartman.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboDepartman.FormattingEnabled = true;
            this.comboDepartman.Location = new System.Drawing.Point(104, 54);
            this.comboDepartman.Name = "comboDepartman";
            this.comboDepartman.Size = new System.Drawing.Size(159, 25);
            this.comboDepartman.TabIndex = 32;
            this.comboDepartman.SelectedIndexChanged += new System.EventHandler(this.comboDepartman_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(8, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 17);
            this.label4.TabIndex = 50;
            this.label4.Text = "Departman";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(8, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 17);
            this.label1.TabIndex = 50;
            this.label1.Text = "Çalışan Adı";
            // 
            // btn2TarihAra
            // 
            this.btn2TarihAra.Location = new System.Drawing.Point(693, 20);
            this.btn2TarihAra.Name = "btn2TarihAra";
            this.btn2TarihAra.Size = new System.Drawing.Size(195, 30);
            this.btn2TarihAra.TabIndex = 131;
            this.btn2TarihAra.Text = "Tarihleri arasındaki Notlar";
            this.btn2TarihAra.UseVisualStyleBackColor = true;
            this.btn2TarihAra.Click += new System.EventHandler(this.btn2TarihAra_Click);
            // 
            // dateTimeStartDate
            // 
            this.dateTimeStartDate.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dateTimeStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeStartDate.Location = new System.Drawing.Point(451, 23);
            this.dateTimeStartDate.Name = "dateTimeStartDate";
            this.dateTimeStartDate.Size = new System.Drawing.Size(116, 25);
            this.dateTimeStartDate.TabIndex = 129;
            // 
            // dateTimeEndDate
            // 
            this.dateTimeEndDate.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dateTimeEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeEndDate.Location = new System.Drawing.Point(573, 23);
            this.dateTimeEndDate.Name = "dateTimeEndDate";
            this.dateTimeEndDate.Size = new System.Drawing.Size(114, 25);
            this.dateTimeEndDate.TabIndex = 130;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(385, 82);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(103, 45);
            this.btnClear.TabIndex = 132;
            this.btnClear.Text = "Seçimleri Temizle";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // formNotAra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 758);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btn2TarihAra);
            this.Controls.Add(this.dateTimeStartDate);
            this.Controls.Add(this.dateTimeEndDate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtkriter3);
            this.Controls.Add(this.txtkriter2);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtkriter1);
            this.Name = "formNotAra";
            this.Text = "formNotAra";
            this.Load += new System.EventHandler(this.formNotAra_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtkriter3;
        private System.Windows.Forms.TextBox txtkriter2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtkriter1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboCalisan;
        private System.Windows.Forms.ComboBox comboDepartman;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn2TarihAra;
        private System.Windows.Forms.DateTimePicker dateTimeStartDate;
        private System.Windows.Forms.DateTimePicker dateTimeEndDate;
        private System.Windows.Forms.Button btnClear;
    }
}