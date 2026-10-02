namespace DirectFNCRM.AdminViews
{
    partial class formPushNotification
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
            this.btnMultiMsj = new System.Windows.Forms.Button();
            this.rbMultiMesaj = new System.Windows.Forms.RichTextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.txtSembol = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtHesapno = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbtnTumKullanici = new System.Windows.Forms.RadioButton();
            this.rbtnTekKullanici = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMultiMsj
            // 
            this.btnMultiMsj.Location = new System.Drawing.Point(551, 12);
            this.btnMultiMsj.Name = "btnMultiMsj";
            this.btnMultiMsj.Size = new System.Drawing.Size(92, 40);
            this.btnMultiMsj.TabIndex = 12;
            this.btnMultiMsj.Text = "Gönder";
            this.btnMultiMsj.UseVisualStyleBackColor = true;
            this.btnMultiMsj.Click += new System.EventHandler(this.btnMultiMsj_Click);
            // 
            // rbMultiMesaj
            // 
            this.rbMultiMesaj.Location = new System.Drawing.Point(8, 76);
            this.rbMultiMesaj.Name = "rbMultiMesaj";
            this.rbMultiMesaj.Size = new System.Drawing.Size(660, 204);
            this.rbMultiMesaj.TabIndex = 11;
            this.rbMultiMesaj.Text = "";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(0, 318);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 25;
            this.dataGridView1.RowTemplate.Height = 20;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(679, 353);
            this.dataGridView1.TabIndex = 127;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(442, 11);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(92, 40);
            this.btnEdit.TabIndex = 128;
            this.btnEdit.Text = "Düzelt";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(330, 11);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(92, 40);
            this.btnNew.TabIndex = 129;
            this.btnNew.Text = "Yeni";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // txtSembol
            // 
            this.txtSembol.Location = new System.Drawing.Point(824, 48);
            this.txtSembol.Name = "txtSembol";
            this.txtSembol.Size = new System.Drawing.Size(112, 22);
            this.txtSembol.TabIndex = 130;
            this.txtSembol.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(736, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 17);
            this.label1.TabIndex = 131;
            this.label1.Text = "ilgili Sembol";
            this.label1.Visible = false;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(8, 288);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(271, 22);
            this.txtSearch.TabIndex = 132;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(299, 287);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 133;
            this.btnSearch.Text = "ARA";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtHesapno
            // 
            this.txtHesapno.Location = new System.Drawing.Point(100, 45);
            this.txtHesapno.Name = "txtHesapno";
            this.txtHesapno.Size = new System.Drawing.Size(131, 22);
            this.txtHesapno.TabIndex = 137;
            this.txtHesapno.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 17);
            this.label2.TabIndex = 136;
            this.label2.Text = "HesapNo";
            this.label2.Visible = false;
            // 
            // rbtnTumKullanici
            // 
            this.rbtnTumKullanici.AutoSize = true;
            this.rbtnTumKullanici.Location = new System.Drawing.Point(16, 11);
            this.rbtnTumKullanici.Name = "rbtnTumKullanici";
            this.rbtnTumKullanici.Size = new System.Drawing.Size(137, 21);
            this.rbtnTumKullanici.TabIndex = 135;
            this.rbtnTumKullanici.TabStop = true;
            this.rbtnTumKullanici.Text = "Tüm Kullanıcılara";
            this.rbtnTumKullanici.UseVisualStyleBackColor = true;
            this.rbtnTumKullanici.Click += new System.EventHandler(this.rbtnTumKullanici_Click);
            // 
            // rbtnTekKullanici
            // 
            this.rbtnTekKullanici.AutoSize = true;
            this.rbtnTekKullanici.Location = new System.Drawing.Point(170, 11);
            this.rbtnTekKullanici.Name = "rbtnTekKullanici";
            this.rbtnTekKullanici.Size = new System.Drawing.Size(109, 21);
            this.rbtnTekKullanici.TabIndex = 134;
            this.rbtnTekKullanici.TabStop = true;
            this.rbtnTekKullanici.Text = "Tek Kullanıcı";
            this.rbtnTekKullanici.UseVisualStyleBackColor = true;
            this.rbtnTekKullanici.Click += new System.EventHandler(this.rbtnTumKullanici_Click);
            // 
            // formPushNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 671);
            this.Controls.Add(this.txtHesapno);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbtnTumKullanici);
            this.Controls.Add(this.rbtnTekKullanici);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSembol);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnMultiMsj);
            this.Controls.Add(this.rbMultiMesaj);
            this.Name = "formPushNotification";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Push Notification Gönderim";
            this.Load += new System.EventHandler(this.formPushNotification_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMultiMsj;
        private System.Windows.Forms.RichTextBox rbMultiMesaj;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.TextBox txtSembol;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtHesapno;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbtnTumKullanici;
        private System.Windows.Forms.RadioButton rbtnTekKullanici;
    }
}