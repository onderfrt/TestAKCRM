namespace DirectFNCRM.ServerViews
{
    partial class FrmDbSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDbSettings));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblCurrentDbTitle = new System.Windows.Forms.Label();
            this.lblCurrentDbValue = new System.Windows.Forms.Label();
            this.gbConn = new System.Windows.Forms.GroupBox();
            this.tlpFields = new System.Windows.Forms.TableLayoutPanel();
            this.labelServer = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.labelDb = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.labelAuth = new System.Windows.Forms.Label();
            this.pnlAuth = new System.Windows.Forms.Panel();
            this.rbSqlAuth = new System.Windows.Forms.RadioButton();
            this.rbWindowsAuth = new System.Windows.Forms.RadioButton();
            this.labelUser = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblHint = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tlpMain.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.gbConn.SuspendLayout();
            this.tlpFields.SuspendLayout();
            this.pnlAuth.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.pnlTop, 0, 0);
            this.tlpMain.Controls.Add(this.gbConn, 0, 1);
            this.tlpMain.Controls.Add(this.lblHint, 0, 2);
            this.tlpMain.Controls.Add(this.pnlBottom, 0, 3);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.Padding = new System.Windows.Forms.Padding(10);
            this.tlpMain.RowCount = 4;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpMain.Size = new System.Drawing.Size(700, 442);
            this.tlpMain.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblCurrentDbTitle);
            this.pnlTop.Controls.Add(this.lblCurrentDbValue);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(13, 13);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(674, 36);
            this.pnlTop.TabIndex = 0;
            // 
            // lblCurrentDbTitle
            // 
            this.lblCurrentDbTitle.AutoSize = true;
            this.lblCurrentDbTitle.Location = new System.Drawing.Point(6, 10);
            this.lblCurrentDbTitle.Name = "lblCurrentDbTitle";
            this.lblCurrentDbTitle.Size = new System.Drawing.Size(104, 16);
            this.lblCurrentDbTitle.TabIndex = 0;
            this.lblCurrentDbTitle.Text = "Mevcut bağlantı:";
            // 
            // lblCurrentDbValue
            // 
            this.lblCurrentDbValue.AutoSize = true;
            this.lblCurrentDbValue.ForeColor = System.Drawing.Color.DimGray;
            this.lblCurrentDbValue.Location = new System.Drawing.Point(110, 10);
            this.lblCurrentDbValue.Name = "lblCurrentDbValue";
            this.lblCurrentDbValue.Size = new System.Drawing.Size(11, 16);
            this.lblCurrentDbValue.TabIndex = 1;
            this.lblCurrentDbValue.Text = "-";
            // 
            // gbConn
            // 
            this.gbConn.Controls.Add(this.tlpFields);
            this.gbConn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbConn.Location = new System.Drawing.Point(13, 55);
            this.gbConn.Name = "gbConn";
            this.gbConn.Padding = new System.Windows.Forms.Padding(10);
            this.gbConn.Size = new System.Drawing.Size(674, 194);
            this.gbConn.TabIndex = 1;
            this.gbConn.TabStop = false;
            this.gbConn.Text = "Yeni bağlantı bilgileri";
            // 
            // tlpFields
            // 
            this.tlpFields.ColumnCount = 2;
            this.tlpFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFields.Controls.Add(this.labelServer, 0, 0);
            this.tlpFields.Controls.Add(this.txtServer, 1, 0);
            this.tlpFields.Controls.Add(this.labelDb, 0, 1);
            this.tlpFields.Controls.Add(this.txtDatabase, 1, 1);
            this.tlpFields.Controls.Add(this.labelAuth, 0, 2);
            this.tlpFields.Controls.Add(this.pnlAuth, 1, 2);
            this.tlpFields.Controls.Add(this.labelUser, 0, 3);
            this.tlpFields.Controls.Add(this.txtUser, 1, 3);
            this.tlpFields.Controls.Add(this.labelPassword, 0, 4);
            this.tlpFields.Controls.Add(this.txtPassword, 1, 4);
            this.tlpFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFields.Location = new System.Drawing.Point(10, 25);
            this.tlpFields.Name = "tlpFields";
            this.tlpFields.RowCount = 5;
            this.tlpFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpFields.Size = new System.Drawing.Size(654, 159);
            this.tlpFields.TabIndex = 0;
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelServer.Location = new System.Drawing.Point(3, 0);
            this.labelServer.Name = "labelServer";
            this.labelServer.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.labelServer.Size = new System.Drawing.Size(144, 30);
            this.labelServer.TabIndex = 0;
            this.labelServer.Text = "Server";
            // 
            // txtServer
            // 
            this.txtServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtServer.Location = new System.Drawing.Point(153, 3);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(498, 22);
            this.txtServer.TabIndex = 1;
            // 
            // labelDb
            // 
            this.labelDb.AutoSize = true;
            this.labelDb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDb.Location = new System.Drawing.Point(3, 30);
            this.labelDb.Name = "labelDb";
            this.labelDb.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.labelDb.Size = new System.Drawing.Size(144, 30);
            this.labelDb.TabIndex = 2;
            this.labelDb.Text = "Database (Initial Catalog)";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDatabase.Location = new System.Drawing.Point(153, 33);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(498, 22);
            this.txtDatabase.TabIndex = 3;
            // 
            // labelAuth
            // 
            this.labelAuth.AutoSize = true;
            this.labelAuth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAuth.Location = new System.Drawing.Point(3, 60);
            this.labelAuth.Name = "labelAuth";
            this.labelAuth.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.labelAuth.Size = new System.Drawing.Size(144, 34);
            this.labelAuth.TabIndex = 4;
            this.labelAuth.Text = "Kimlik Doğrulama";
            // 
            // pnlAuth
            // 
            this.pnlAuth.Controls.Add(this.rbSqlAuth);
            this.pnlAuth.Controls.Add(this.rbWindowsAuth);
            this.pnlAuth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAuth.Location = new System.Drawing.Point(153, 63);
            this.pnlAuth.Name = "pnlAuth";
            this.pnlAuth.Size = new System.Drawing.Size(498, 28);
            this.pnlAuth.TabIndex = 5;
            // 
            // rbSqlAuth
            // 
            this.rbSqlAuth.AutoSize = true;
            this.rbSqlAuth.Checked = true;
            this.rbSqlAuth.Location = new System.Drawing.Point(0, 5);
            this.rbSqlAuth.Name = "rbSqlAuth";
            this.rbSqlAuth.Size = new System.Drawing.Size(140, 20);
            this.rbSqlAuth.TabIndex = 0;
            this.rbSqlAuth.TabStop = true;
            this.rbSqlAuth.Text = "SQL Authentication";
            this.rbSqlAuth.UseVisualStyleBackColor = true;
            // 
            // rbWindowsAuth
            // 
            this.rbWindowsAuth.AutoSize = true;
            this.rbWindowsAuth.Location = new System.Drawing.Point(259, 6);
            this.rbWindowsAuth.Name = "rbWindowsAuth";
            this.rbWindowsAuth.Size = new System.Drawing.Size(169, 20);
            this.rbWindowsAuth.TabIndex = 1;
            this.rbWindowsAuth.Text = "Windows Authentication";
            this.rbWindowsAuth.UseVisualStyleBackColor = true;
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUser.Location = new System.Drawing.Point(3, 94);
            this.labelUser.Name = "labelUser";
            this.labelUser.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.labelUser.Size = new System.Drawing.Size(144, 30);
            this.labelUser.TabIndex = 6;
            this.labelUser.Text = "User";
            // 
            // txtUser
            // 
            this.txtUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUser.Location = new System.Drawing.Point(153, 97);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(498, 22);
            this.txtUser.TabIndex = 7;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPassword.Location = new System.Drawing.Point(3, 124);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.labelPassword.Size = new System.Drawing.Size(144, 35);
            this.labelPassword.TabIndex = 8;
            this.labelPassword.Text = "Şifre";
            // 
            // txtPassword
            // 
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Location = new System.Drawing.Point(153, 127);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(498, 22);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblHint
            // 
            this.lblHint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHint.ForeColor = System.Drawing.SystemColors.Desktop;
            this.lblHint.Location = new System.Drawing.Point(13, 252);
            this.lblHint.Name = "lblHint";
            this.lblHint.Padding = new System.Windows.Forms.Padding(8);
            this.lblHint.Size = new System.Drawing.Size(674, 110);
            this.lblHint.TabIndex = 2;
            this.lblHint.Text = resources.GetString("lblHint.Text");
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lblStatus);
            this.pnlBottom.Controls.Add(this.btnTest);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(13, 365);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(674, 64);
            this.pnlBottom.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Location = new System.Drawing.Point(6, 8);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(408, 48);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Bağlantı durumu: Henüz test edilmedi.";
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(420, 18);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 28);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(505, 18);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(590, 18);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmDbSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 442);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDbSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Veritabanı Bağlantı Ayarları";
            this.tlpMain.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.gbConn.ResumeLayout(false);
            this.tlpFields.ResumeLayout(false);
            this.tlpFields.PerformLayout();
            this.pnlAuth.ResumeLayout(false);
            this.pnlAuth.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblCurrentDbTitle;
        private System.Windows.Forms.Label lblCurrentDbValue;

        private System.Windows.Forms.GroupBox gbConn;
        private System.Windows.Forms.TableLayoutPanel tlpFields;

        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.TextBox txtServer;

        private System.Windows.Forms.Label labelDb;
        private System.Windows.Forms.TextBox txtDatabase;

        private System.Windows.Forms.Label labelAuth;
        private System.Windows.Forms.Panel pnlAuth;
        private System.Windows.Forms.RadioButton rbSqlAuth;
        private System.Windows.Forms.RadioButton rbWindowsAuth;

        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.TextBox txtUser;

        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox txtPassword;

        private System.Windows.Forms.Label lblHint;

        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

    }
}