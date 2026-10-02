namespace DirectFNCRM.Muhasebe
{
    partial class formMuhasebe
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
            this.btnLisansFiyat = new System.Windows.Forms.Button();
            this.btnOzetListe = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLisansFiyat
            // 
            this.btnLisansFiyat.Location = new System.Drawing.Point(38, 12);
            this.btnLisansFiyat.Name = "btnLisansFiyat";
            this.btnLisansFiyat.Size = new System.Drawing.Size(112, 44);
            this.btnLisansFiyat.TabIndex = 0;
            this.btnLisansFiyat.Text = "Lisans Fiyatları";
            this.btnLisansFiyat.UseVisualStyleBackColor = true;
            this.btnLisansFiyat.Click += new System.EventHandler(this.btnLisansFiyat_Click);
            // 
            // btnOzetListe
            // 
            this.btnOzetListe.Location = new System.Drawing.Point(218, 12);
            this.btnOzetListe.Name = "btnOzetListe";
            this.btnOzetListe.Size = new System.Drawing.Size(125, 44);
            this.btnOzetListe.TabIndex = 1;
            this.btnOzetListe.Text = "Borsa Özet Liste";
            this.btnOzetListe.UseVisualStyleBackColor = true;
            this.btnOzetListe.Click += new System.EventHandler(this.btnOzetListe_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(369, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 44);
            this.button1.TabIndex = 2;
            this.button1.Text = "BorsaDetay";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // formMuhasebe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 722);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOzetListe);
            this.Controls.Add(this.btnLisansFiyat);
            this.Name = "formMuhasebe";
            this.Text = "Muhasebe Paneli";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLisansFiyat;
        private System.Windows.Forms.Button btnOzetListe;
        private System.Windows.Forms.Button button1;
    }
}