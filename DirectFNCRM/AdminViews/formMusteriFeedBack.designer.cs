namespace DirectFNCRM.AdminViews
{
    partial class formMusteriFeedBack
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
            this.txtkriter3 = new System.Windows.Forms.TextBox();
            this.txtkriter2 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtkriter1 = new System.Windows.Forms.TextBox();
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
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(9, 76);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 25;
            this.dataGridView1.RowTemplate.Height = 20;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(800, 529);
            this.dataGridView1.TabIndex = 18;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // txtkriter3
            // 
            this.txtkriter3.BackColor = System.Drawing.Color.PowderBlue;
            this.txtkriter3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtkriter3.Location = new System.Drawing.Point(300, 25);
            this.txtkriter3.Name = "txtkriter3";
            this.txtkriter3.Size = new System.Drawing.Size(102, 22);
            this.txtkriter3.TabIndex = 124;
            this.txtkriter3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtkriter3_KeyPress);
            // 
            // txtkriter2
            // 
            this.txtkriter2.BackColor = System.Drawing.Color.Yellow;
            this.txtkriter2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtkriter2.Location = new System.Drawing.Point(192, 25);
            this.txtkriter2.Name = "txtkriter2";
            this.txtkriter2.Size = new System.Drawing.Size(102, 22);
            this.txtkriter2.TabIndex = 123;
            this.txtkriter2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtkriter2_KeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label15.Location = new System.Drawing.Point(21, 28);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(55, 17);
            this.label15.TabIndex = 125;
            this.label15.Text = "Filtreler";
            // 
            // txtkriter1
            // 
            this.txtkriter1.BackColor = System.Drawing.Color.Lime;
            this.txtkriter1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtkriter1.Location = new System.Drawing.Point(82, 25);
            this.txtkriter1.Name = "txtkriter1";
            this.txtkriter1.Size = new System.Drawing.Size(104, 22);
            this.txtkriter1.TabIndex = 122;
            this.txtkriter1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtkriter1_KeyPress);
            // 
            // formMusteriFeedBack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 614);
            this.Controls.Add(this.txtkriter3);
            this.Controls.Add(this.txtkriter2);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtkriter1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "formMusteriFeedBack";
            this.ShowIcon = false;
            this.Text = "Müşteri Talep ve Şikayetleri";
            this.Load += new System.EventHandler(this.formMusteriFeedBack_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtkriter3;
        private System.Windows.Forms.TextBox txtkriter2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtkriter1;
    }
}