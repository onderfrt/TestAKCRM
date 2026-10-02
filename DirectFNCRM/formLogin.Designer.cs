namespace DirectFNCRM
{
    partial class formLogin
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
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnEscape = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUnuttum = new System.Windows.Forms.Button();
            this.chxHatirla = new System.Windows.Forms.CheckBox();
            this.lblVersiyon = new System.Windows.Forms.Label();
            this.btnChange1 = new System.Windows.Forms.Button();
            this.txtOldSifre = new System.Windows.Forms.TextBox();
            this.txtNewSifre = new System.Windows.Forms.TextBox();
            this.lblOldSifre = new System.Windows.Forms.Label();
            this.lblNewSifre = new System.Windows.Forms.Label();
            this.btnChange2 = new System.Windows.Forms.Button();
            this.btnChangeESC = new System.Windows.Forms.Button();
            this.btnSQL = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(24, 124);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(96, 30);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Giriş";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnEscape
            // 
            this.btnEscape.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnEscape.Location = new System.Drawing.Point(132, 124);
            this.btnEscape.Name = "btnEscape";
            this.btnEscape.Size = new System.Drawing.Size(97, 30);
            this.btnEscape.TabIndex = 3;
            this.btnEscape.Text = "Vazgeç";
            this.btnEscape.UseVisualStyleBackColor = true;
            this.btnEscape.Click += new System.EventHandler(this.btnEscape_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(111, 23);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(222, 22);
            this.txtUsername.TabIndex = 0;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(111, 60);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(222, 22);
            this.txtPassword.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Kullanıcı Adı";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Şifre";
            // 
            // btnUnuttum
            // 
            this.btnUnuttum.Location = new System.Drawing.Point(235, 88);
            this.btnUnuttum.Name = "btnUnuttum";
            this.btnUnuttum.Size = new System.Drawing.Size(136, 30);
            this.btnUnuttum.TabIndex = 6;
            this.btnUnuttum.Text = "Şifremi Unuttum";
            this.btnUnuttum.UseVisualStyleBackColor = true;
            this.btnUnuttum.Click += new System.EventHandler(this.btnUnuttum_Click);
            // 
            // chxHatirla
            // 
            this.chxHatirla.AutoSize = true;
            this.chxHatirla.Location = new System.Drawing.Point(111, 90);
            this.chxHatirla.Name = "chxHatirla";
            this.chxHatirla.Size = new System.Drawing.Size(118, 21);
            this.chxHatirla.TabIndex = 7;
            this.chxHatirla.Text = "Şifremi Hatırla";
            this.chxHatirla.UseVisualStyleBackColor = true;
            // 
            // lblVersiyon
            // 
            this.lblVersiyon.AutoSize = true;
            this.lblVersiyon.Location = new System.Drawing.Point(143, 3);
            this.lblVersiyon.Name = "lblVersiyon";
            this.lblVersiyon.Size = new System.Drawing.Size(123, 17);
            this.lblVersiyon.TabIndex = 4;
            this.lblVersiyon.Text = "versiyon numarası";
            // 
            // btnChange1
            // 
            this.btnChange1.Location = new System.Drawing.Point(235, 124);
            this.btnChange1.Name = "btnChange1";
            this.btnChange1.Size = new System.Drawing.Size(136, 30);
            this.btnChange1.TabIndex = 8;
            this.btnChange1.Text = "Şifre Değiştir";
            this.btnChange1.UseVisualStyleBackColor = true;
            this.btnChange1.Click += new System.EventHandler(this.btnChange1_Click);
            // 
            // txtOldSifre
            // 
            this.txtOldSifre.Location = new System.Drawing.Point(109, 194);
            this.txtOldSifre.Name = "txtOldSifre";
            this.txtOldSifre.Size = new System.Drawing.Size(141, 22);
            this.txtOldSifre.TabIndex = 0;
            // 
            // txtNewSifre
            // 
            this.txtNewSifre.Location = new System.Drawing.Point(109, 231);
            this.txtNewSifre.Name = "txtNewSifre";
            this.txtNewSifre.PasswordChar = '*';
            this.txtNewSifre.Size = new System.Drawing.Size(141, 22);
            this.txtNewSifre.TabIndex = 1;
            // 
            // lblOldSifre
            // 
            this.lblOldSifre.AutoSize = true;
            this.lblOldSifre.Location = new System.Drawing.Point(9, 194);
            this.lblOldSifre.Name = "lblOldSifre";
            this.lblOldSifre.Size = new System.Drawing.Size(67, 17);
            this.lblOldSifre.TabIndex = 4;
            this.lblOldSifre.Text = "Eski Şifre";
            // 
            // lblNewSifre
            // 
            this.lblNewSifre.AutoSize = true;
            this.lblNewSifre.Location = new System.Drawing.Point(9, 234);
            this.lblNewSifre.Name = "lblNewSifre";
            this.lblNewSifre.Size = new System.Drawing.Size(69, 17);
            this.lblNewSifre.TabIndex = 5;
            this.lblNewSifre.Text = "Yeni Şifre";
            // 
            // btnChange2
            // 
            this.btnChange2.Location = new System.Drawing.Point(256, 190);
            this.btnChange2.Name = "btnChange2";
            this.btnChange2.Size = new System.Drawing.Size(124, 30);
            this.btnChange2.TabIndex = 8;
            this.btnChange2.Text = "Şifre Değiştir";
            this.btnChange2.UseVisualStyleBackColor = true;
            this.btnChange2.Click += new System.EventHandler(this.btnChange2_Click);
            // 
            // btnChangeESC
            // 
            this.btnChangeESC.Location = new System.Drawing.Point(256, 226);
            this.btnChangeESC.Name = "btnChangeESC";
            this.btnChangeESC.Size = new System.Drawing.Size(124, 30);
            this.btnChangeESC.TabIndex = 8;
            this.btnChangeESC.Text = "Vazgeç";
            this.btnChangeESC.UseVisualStyleBackColor = true;
            this.btnChangeESC.Click += new System.EventHandler(this.btnChangeESC_Click);
            // 
            // btnSQL
            // 
            this.btnSQL.Location = new System.Drawing.Point(462, 318);
            this.btnSQL.Name = "btnSQL";
            this.btnSQL.Size = new System.Drawing.Size(122, 30);
            this.btnSQL.TabIndex = 9;
            this.btnSQL.Text = "SQL AYAR";
            this.btnSQL.UseVisualStyleBackColor = true;
            this.btnSQL.Visible = false;
            this.btnSQL.Click += new System.EventHandler(this.btnSQL_Click);
            // 
            // formLogin
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnEscape;
            this.ClientSize = new System.Drawing.Size(388, 166);
            this.Controls.Add(this.btnSQL);
            this.Controls.Add(this.btnChangeESC);
            this.Controls.Add(this.btnChange2);
            this.Controls.Add(this.btnChange1);
            this.Controls.Add(this.chxHatirla);
            this.Controls.Add(this.btnUnuttum);
            this.Controls.Add(this.lblNewSifre);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblVersiyon);
            this.Controls.Add(this.lblOldSifre);
            this.Controls.Add(this.txtNewSifre);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOldSifre);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.btnEscape);
            this.Controls.Add(this.btnLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "formLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CRM  DirectFN Giriş";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.formLogin_FormClosed);
            this.Load += new System.EventHandler(this.formLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnEscape;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUnuttum;
        private System.Windows.Forms.CheckBox chxHatirla;
        private System.Windows.Forms.Label lblVersiyon;
        private System.Windows.Forms.Button btnChange1;
        private System.Windows.Forms.TextBox txtOldSifre;
        private System.Windows.Forms.TextBox txtNewSifre;
        private System.Windows.Forms.Label lblOldSifre;
        private System.Windows.Forms.Label lblNewSifre;
        private System.Windows.Forms.Button btnChange2;
        private System.Windows.Forms.Button btnChangeESC;
        private System.Windows.Forms.Button btnSQL;
    }
}

