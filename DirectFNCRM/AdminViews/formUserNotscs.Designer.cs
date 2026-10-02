namespace DirectFNCRM.AdminViews
{
    partial class formUserNotscs
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
            this.gridUserNots = new System.Windows.Forms.DataGridView();
            this.rboxUserNot = new System.Windows.Forms.RichTextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridUserNots)).BeginInit();
            this.SuspendLayout();
            // 
            // gridUserNots
            // 
            this.gridUserNots.AllowUserToAddRows = false;
            this.gridUserNots.AllowUserToDeleteRows = false;
            this.gridUserNots.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.gridUserNots.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridUserNots.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridUserNots.Location = new System.Drawing.Point(6, 304);
            this.gridUserNots.Margin = new System.Windows.Forms.Padding(0);
            this.gridUserNots.Name = "gridUserNots";
            this.gridUserNots.ReadOnly = true;
            this.gridUserNots.RowHeadersWidth = 25;
            this.gridUserNots.RowTemplate.Height = 20;
            this.gridUserNots.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridUserNots.Size = new System.Drawing.Size(690, 387);
            this.gridUserNots.TabIndex = 18;
            this.gridUserNots.SelectionChanged += new System.EventHandler(this.gridUserNots_SelectionChanged);
            // 
            // rboxUserNot
            // 
            this.rboxUserNot.Location = new System.Drawing.Point(6, 41);
            this.rboxUserNot.Name = "rboxUserNot";
            this.rboxUserNot.Size = new System.Drawing.Size(690, 260);
            this.rboxUserNot.TabIndex = 19;
            this.rboxUserNot.Text = "";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(600, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 27);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(499, 12);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(95, 27);
            this.btnChange.TabIndex = 20;
            this.btnChange.Text = "Değiştir";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(398, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(95, 27);
            this.btnNew.TabIndex = 20;
            this.btnNew.Text = "Yeni";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // formUserNotscs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 700);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.rboxUserNot);
            this.Controls.Add(this.gridUserNots);
            this.Name = "formUserNotscs";
            this.Text = "Users Notları";
            this.Load += new System.EventHandler(this.formUserNotscs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridUserNots)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridUserNots;
        private System.Windows.Forms.RichTextBox rboxUserNot;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Button btnNew;
    }
}