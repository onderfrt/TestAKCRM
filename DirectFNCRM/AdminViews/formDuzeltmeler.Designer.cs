namespace DirectFNCRM.AdminViews
{
    partial class formDuzeltmeler
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
            this.lblSayac = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOnizle = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();
            this.lblBilgi = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSayac
            // 
            this.lblSayac.AutoSize = true;
            this.lblSayac.Location = new System.Drawing.Point(29, 13);
            this.lblSayac.Name = "lblSayac";
            this.lblSayac.Size = new System.Drawing.Size(0, 16);
            this.lblSayac.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 62);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(264, 31);
            this.button1.TabIndex = 1;
            this.button1.Text = "Usernam deki Boşlukları  temzile";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOnizle
            // 
            this.btnOnizle.Location = new System.Drawing.Point(32, 99);
            this.btnOnizle.Name = "btnOnizle";
            this.btnOnizle.Size = new System.Drawing.Size(121, 23);
            this.btnOnizle.TabIndex = 2;
            this.btnOnizle.Text = "Önizleme";
            this.btnOnizle.UseVisualStyleBackColor = true;
            this.btnOnizle.Click += new System.EventHandler(this.btnOnizle_Click);
            // 
            // btnIptal
            // 
            this.btnIptal.Location = new System.Drawing.Point(175, 99);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(121, 23);
            this.btnIptal.TabIndex = 2;
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = true;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // lblBilgi
            // 
            this.lblBilgi.AutoSize = true;
            this.lblBilgi.Location = new System.Drawing.Point(29, 143);
            this.lblBilgi.Name = "lblBilgi";
            this.lblBilgi.Size = new System.Drawing.Size(317, 48);
            this.lblBilgi.TabIndex = 3;
            this.lblBilgi.Text = "⚠️  Bu işlem UserName ve PmtsNo alanlarındaki\n    baştaki/sondaki boşlukları kalı" +
    "cı olarak siler.\n    Yapılan tüm değişiklikler log dosyasına kaydedilir.";
            // 
            // formDuzeltmeler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 200);
            this.Controls.Add(this.lblBilgi);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.btnOnizle);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblSayac);
            this.Name = "formDuzeltmeler";
            this.ShowIcon = false;
            this.Text = "Düzeltme İşlemleri";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSayac;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnOnizle;
        private System.Windows.Forms.Button btnIptal;
        private System.Windows.Forms.Label lblBilgi;
    }
}