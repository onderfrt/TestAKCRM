namespace DirectFNCRM.ServerViews
{
    partial class Server
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server));
            this.IpDeamon = new nsoftware.IPWorks.Ipdaemon(this.components);
            this.listBoxClientList = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listBoxTransactions = new System.Windows.Forms.ListBox();
            this.IpDeamonForClilents = new nsoftware.IPWorks.Ipdaemon(this.components);
            this.menuAna = new System.Windows.Forms.MenuStrip();
            this.localİşlemelerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kullanıcıListesiOkuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expiryDateKontrolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acilacaklarKontrolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onluPaketKapamaKontrolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ayarlarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bağlantıAyarlarıToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kotrolSaatiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sSLVePortBilgileriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dBBaglantıBilgileriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pushNotificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllSendPushNotMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.httpSSO = new nsoftware.IPWorks.Http(this.components);
            this.timerOnOffCheck = new System.Windows.Forms.Timer(this.components);
            this.lblSayac = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.listBoxCepServerList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.IpdaemonforCepServers = new nsoftware.IPWorks.Ipdaemon(this.components);
            this.lblDbip = new System.Windows.Forms.Label();
            this.IpportSso = new nsoftware.IPWorks.Ipport(this.components);
            this.lblLoginHistory = new System.Windows.Forms.Label();
            this.timerKontrol = new System.Windows.Forms.Timer(this.components);
            this.lblSayac3 = new System.Windows.Forms.Label();
            this.lblSayac2 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timerParseAutocreate = new System.Windows.Forms.Timer(this.components);
            this.txtGunSonuInterval = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.HttpsService = new nsoftware.IPWorks.Ipdaemon(this.components);
            this.chkKarmaAcilsin = new System.Windows.Forms.CheckBox();
            this.lblAutoCreateStats = new System.Windows.Forms.Label();
            this.timerLog = new System.Windows.Forms.Timer(this.components);
            this.menuAna.SuspendLayout();
            this.SuspendLayout();
            // 
            // IpDeamon
            // 
            this.IpDeamon.About = "IP*Works! 2016 [Build 7126]";
            this.IpDeamon.OnConnected += new nsoftware.IPWorks.Ipdaemon.OnConnectedHandler(this.IpDeamon_OnConnected);
            this.IpDeamon.OnDataIn += new nsoftware.IPWorks.Ipdaemon.OnDataInHandler(this.IpDeamon_OnDataIn);
            this.IpDeamon.OnDisconnected += new nsoftware.IPWorks.Ipdaemon.OnDisconnectedHandler(this.IpDeamon_OnDisconnected);
            this.IpDeamon.OnReadyToSend += new nsoftware.IPWorks.Ipdaemon.OnReadyToSendHandler(this.IpDeamon_OnReadyToSend);
            // 
            // listBoxClientList
            // 
            this.listBoxClientList.FormattingEnabled = true;
            this.listBoxClientList.ItemHeight = 16;
            this.listBoxClientList.Location = new System.Drawing.Point(1004, 139);
            this.listBoxClientList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBoxClientList.Name = "listBoxClientList";
            this.listBoxClientList.Size = new System.Drawing.Size(147, 260);
            this.listBoxClientList.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1001, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Bağlı Kullanıcılar";
            // 
            // listBoxTransactions
            // 
            this.listBoxTransactions.FormattingEnabled = true;
            this.listBoxTransactions.ItemHeight = 16;
            this.listBoxTransactions.Location = new System.Drawing.Point(12, 121);
            this.listBoxTransactions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBoxTransactions.Name = "listBoxTransactions";
            this.listBoxTransactions.Size = new System.Drawing.Size(985, 468);
            this.listBoxTransactions.TabIndex = 3;
            // 
            // IpDeamonForClilents
            // 
            this.IpDeamonForClilents.About = "IP*Works! 2016 [Build 7126]";
            this.IpDeamonForClilents.OnDataIn += new nsoftware.IPWorks.Ipdaemon.OnDataInHandler(this.IpDeamonForClilents_OnDataIn);
            this.IpDeamonForClilents.OnDisconnected += new nsoftware.IPWorks.Ipdaemon.OnDisconnectedHandler(this.IpDeamonForClilents_OnDisconnected);
            this.IpDeamonForClilents.OnReadyToSend += new nsoftware.IPWorks.Ipdaemon.OnReadyToSendHandler(this.IpDeamonForClilents_OnReadyToSend);
            // 
            // menuAna
            // 
            this.menuAna.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuAna.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.localİşlemelerToolStripMenuItem,
            this.ayarlarToolStripMenuItem,
            this.pushNotificationToolStripMenuItem});
            this.menuAna.Location = new System.Drawing.Point(0, 0);
            this.menuAna.Name = "menuAna";
            this.menuAna.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuAna.Size = new System.Drawing.Size(1163, 28);
            this.menuAna.TabIndex = 5;
            this.menuAna.Text = "menuStrip1";
            // 
            // localİşlemelerToolStripMenuItem
            // 
            this.localİşlemelerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kullanıcıListesiOkuToolStripMenuItem,
            this.expiryDateKontrolToolStripMenuItem,
            this.acilacaklarKontrolToolStripMenuItem,
            this.onluPaketKapamaKontrolToolStripMenuItem});
            this.localİşlemelerToolStripMenuItem.Name = "localİşlemelerToolStripMenuItem";
            this.localİşlemelerToolStripMenuItem.Size = new System.Drawing.Size(122, 24);
            this.localİşlemelerToolStripMenuItem.Text = "Local İşlemeler";
            // 
            // kullanıcıListesiOkuToolStripMenuItem
            // 
            this.kullanıcıListesiOkuToolStripMenuItem.Name = "kullanıcıListesiOkuToolStripMenuItem";
            this.kullanıcıListesiOkuToolStripMenuItem.Size = new System.Drawing.Size(274, 26);
            this.kullanıcıListesiOkuToolStripMenuItem.Text = "Kullanıcı listesi Oku";
            this.kullanıcıListesiOkuToolStripMenuItem.Click += new System.EventHandler(this.kullaniciListesiOkuToolStripMenuItem_Click);
            // 
            // expiryDateKontrolToolStripMenuItem
            // 
            this.expiryDateKontrolToolStripMenuItem.Name = "expiryDateKontrolToolStripMenuItem";
            this.expiryDateKontrolToolStripMenuItem.Size = new System.Drawing.Size(274, 26);
            this.expiryDateKontrolToolStripMenuItem.Text = "Expiry Date Kontrol";
            this.expiryDateKontrolToolStripMenuItem.Click += new System.EventHandler(this.expiryDateKontrolToolStripMenuItem_Click);
            // 
            // acilacaklarKontrolToolStripMenuItem
            // 
            this.acilacaklarKontrolToolStripMenuItem.Name = "acilacaklarKontrolToolStripMenuItem";
            this.acilacaklarKontrolToolStripMenuItem.Size = new System.Drawing.Size(274, 26);
            this.acilacaklarKontrolToolStripMenuItem.Text = "Acilacaklar Kontrol Et";
            this.acilacaklarKontrolToolStripMenuItem.Click += new System.EventHandler(this.acilacaklarKontrolToolStripMenuItem_Click);
            // 
            // onluPaketKapamaKontrolToolStripMenuItem
            // 
            this.onluPaketKapamaKontrolToolStripMenuItem.Name = "onluPaketKapamaKontrolToolStripMenuItem";
            this.onluPaketKapamaKontrolToolStripMenuItem.Size = new System.Drawing.Size(274, 26);
            this.onluPaketKapamaKontrolToolStripMenuItem.Text = "Onlu Paket Kapama Kontrol";
            this.onluPaketKapamaKontrolToolStripMenuItem.Click += new System.EventHandler(this.onluPaketKapamaKontrolToolStripMenuItem_Click);
            // 
            // ayarlarToolStripMenuItem
            // 
            this.ayarlarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bağlantıAyarlarıToolStripMenuItem,
            this.kotrolSaatiToolStripMenuItem,
            this.sSLVePortBilgileriToolStripMenuItem,
            this.dBBaglantıBilgileriToolStripMenuItem});
            this.ayarlarToolStripMenuItem.Name = "ayarlarToolStripMenuItem";
            this.ayarlarToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.ayarlarToolStripMenuItem.Text = "Ayarlar";
            // 
            // bağlantıAyarlarıToolStripMenuItem
            // 
            this.bağlantıAyarlarıToolStripMenuItem.Name = "bağlantıAyarlarıToolStripMenuItem";
            this.bağlantıAyarlarıToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.bağlantıAyarlarıToolStripMenuItem.Text = "Bağlantı Ayarları";
            this.bağlantıAyarlarıToolStripMenuItem.Visible = false;
            this.bağlantıAyarlarıToolStripMenuItem.Click += new System.EventHandler(this.baglantiAyarlariToolStripMenuItem_Click);
            // 
            // kotrolSaatiToolStripMenuItem
            // 
            this.kotrolSaatiToolStripMenuItem.Name = "kotrolSaatiToolStripMenuItem";
            this.kotrolSaatiToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.kotrolSaatiToolStripMenuItem.Text = "Kotrol Saati";
            this.kotrolSaatiToolStripMenuItem.Click += new System.EventHandler(this.kotrolSaatiToolStripMenuItem_Click);
            // 
            // sSLVePortBilgileriToolStripMenuItem
            // 
            this.sSLVePortBilgileriToolStripMenuItem.Name = "sSLVePortBilgileriToolStripMenuItem";
            this.sSLVePortBilgileriToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.sSLVePortBilgileriToolStripMenuItem.Text = "SSL ve Port Bilgileri";
            this.sSLVePortBilgileriToolStripMenuItem.Click += new System.EventHandler(this.sSLVePortBilgileriToolStripMenuItem_Click);
            // 
            // dBBaglantıBilgileriToolStripMenuItem
            // 
            this.dBBaglantıBilgileriToolStripMenuItem.Name = "dBBaglantıBilgileriToolStripMenuItem";
            this.dBBaglantıBilgileriToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.dBBaglantıBilgileriToolStripMenuItem.Text = "DB Baglantı Bilgileri";
            this.dBBaglantıBilgileriToolStripMenuItem.Click += new System.EventHandler(this.dBBaglantıBilgileriToolStripMenuItem_Click);
            // 
            // pushNotificationToolStripMenuItem
            // 
            this.pushNotificationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AllSendPushNotMenuItem});
            this.pushNotificationToolStripMenuItem.Name = "pushNotificationToolStripMenuItem";
            this.pushNotificationToolStripMenuItem.Size = new System.Drawing.Size(132, 24);
            this.pushNotificationToolStripMenuItem.Text = "PushNotification";
            // 
            // AllSendPushNotMenuItem
            // 
            this.AllSendPushNotMenuItem.Name = "AllSendPushNotMenuItem";
            this.AllSendPushNotMenuItem.Size = new System.Drawing.Size(252, 26);
            this.AllSendPushNotMenuItem.Text = "Tüm Müşterilere Gönder";
            this.AllSendPushNotMenuItem.Click += new System.EventHandler(this.AllSendPushNotMenuItem_Click);
            // 
            // httpSSO
            // 
            this.httpSSO.About = "IP*Works! 2016 [Build 7126]";
            // 
            // timerOnOffCheck
            // 
            this.timerOnOffCheck.Interval = 1000;
            this.timerOnOffCheck.Tick += new System.EventHandler(this.timerOnOffCheck_Tick);
            // 
            // lblSayac
            // 
            this.lblSayac.AutoSize = true;
            this.lblSayac.Location = new System.Drawing.Point(556, 42);
            this.lblSayac.Name = "lblSayac";
            this.lblSayac.Size = new System.Drawing.Size(0, 16);
            this.lblSayac.TabIndex = 53;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(791, 38);
            this.btnTest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(117, 23);
            this.btnTest.TabIndex = 54;
            this.btnTest.Text = "testtoplamyaz";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // listBoxCepServerList
            // 
            this.listBoxCepServerList.FormattingEnabled = true;
            this.listBoxCepServerList.ItemHeight = 16;
            this.listBoxCepServerList.Location = new System.Drawing.Point(1004, 428);
            this.listBoxCepServerList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBoxCepServerList.Name = "listBoxCepServerList";
            this.listBoxCepServerList.Size = new System.Drawing.Size(147, 148);
            this.listBoxCepServerList.TabIndex = 56;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1004, 404);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 16);
            this.label1.TabIndex = 55;
            this.label1.Text = "Bağlı CepServer";
            // 
            // IpdaemonforCepServers
            // 
            this.IpdaemonforCepServers.About = "IP*Works! 2016 [Build 7126]";
            this.IpdaemonforCepServers.OnConnected += new nsoftware.IPWorks.Ipdaemon.OnConnectedHandler(this.IpdaemonforCepServers_OnConnected);
            this.IpdaemonforCepServers.OnDataIn += new nsoftware.IPWorks.Ipdaemon.OnDataInHandler(this.IpdaemonforCepServers_OnDataIn);
            this.IpdaemonforCepServers.OnDisconnected += new nsoftware.IPWorks.Ipdaemon.OnDisconnectedHandler(this.IpdaemonforCepServers_OnDisconnected);
            // 
            // lblDbip
            // 
            this.lblDbip.AutoSize = true;
            this.lblDbip.Location = new System.Drawing.Point(928, 42);
            this.lblDbip.Name = "lblDbip";
            this.lblDbip.Size = new System.Drawing.Size(42, 16);
            this.lblDbip.TabIndex = 57;
            this.lblDbip.Text = "db_IP";
            this.lblDbip.Click += new System.EventHandler(this.lblDbip_Click);
            // 
            // IpportSso
            // 
            this.IpportSso.About = "IP*Works! 2016 [Build 7126]";
            this.IpportSso.OnDataIn += new nsoftware.IPWorks.Ipport.OnDataInHandler(this.IpportSso_OnDataIn);
            // 
            // lblLoginHistory
            // 
            this.lblLoginHistory.AutoSize = true;
            this.lblLoginHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblLoginHistory.ForeColor = System.Drawing.Color.Firebrick;
            this.lblLoginHistory.Location = new System.Drawing.Point(443, 98);
            this.lblLoginHistory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoginHistory.Name = "lblLoginHistory";
            this.lblLoginHistory.Size = new System.Drawing.Size(109, 20);
            this.lblLoginHistory.TabIndex = 59;
            this.lblLoginHistory.Text = "Login History";
            // 
            // timerKontrol
            // 
            this.timerKontrol.Enabled = true;
            this.timerKontrol.Interval = 5000;
            this.timerKontrol.Tick += new System.EventHandler(this.timerKontrol_Tick);
            // 
            // lblSayac3
            // 
            this.lblSayac3.AutoSize = true;
            this.lblSayac3.Location = new System.Drawing.Point(604, 68);
            this.lblSayac3.Name = "lblSayac3";
            this.lblSayac3.Size = new System.Drawing.Size(51, 16);
            this.lblSayac3.TabIndex = 62;
            this.lblSayac3.Text = "sayac3";
            // 
            // lblSayac2
            // 
            this.lblSayac2.AutoSize = true;
            this.lblSayac2.Location = new System.Drawing.Point(327, 63);
            this.lblSayac2.Name = "lblSayac2";
            this.lblSayac2.Size = new System.Drawing.Size(51, 16);
            this.lblSayac2.TabIndex = 60;
            this.lblSayac2.Text = "sayac2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 61;
            this.label2.Text = "sayac";
            // 
            // timerParseAutocreate
            // 
            this.timerParseAutocreate.Enabled = true;
            this.timerParseAutocreate.Tick += new System.EventHandler(this.timerParseAutocreate_Tick);
            // 
            // txtGunSonuInterval
            // 
            this.txtGunSonuInterval.Location = new System.Drawing.Point(1027, 81);
            this.txtGunSonuInterval.Margin = new System.Windows.Forms.Padding(4);
            this.txtGunSonuInterval.Name = "txtGunSonuInterval";
            this.txtGunSonuInterval.Size = new System.Drawing.Size(87, 22);
            this.txtGunSonuInterval.TabIndex = 63;
            this.txtGunSonuInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGunSonuInterval_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(828, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 16);
            this.label4.TabIndex = 60;
            this.label4.Text = "Gün Sonu Gönderim Sıklığı";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1121, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 16);
            this.label5.TabIndex = 62;
            this.label5.Text = "ms";
            // 
            // HttpsService
            // 
            this.HttpsService.About = "IP*Works! 2016 [Build 7126]";
            this.HttpsService.OnDataIn += new nsoftware.IPWorks.Ipdaemon.OnDataInHandler(this.HttpsService_OnDataIn);
            // 
            // chkKarmaAcilsin
            // 
            this.chkKarmaAcilsin.AutoSize = true;
            this.chkKarmaAcilsin.Location = new System.Drawing.Point(632, 94);
            this.chkKarmaAcilsin.Margin = new System.Windows.Forms.Padding(4);
            this.chkKarmaAcilsin.Name = "chkKarmaAcilsin";
            this.chkKarmaAcilsin.Size = new System.Drawing.Size(155, 20);
            this.chkKarmaAcilsin.TabIndex = 64;
            this.chkKarmaAcilsin.Text = "Karma Lisansı Açılsın";
            this.chkKarmaAcilsin.UseVisualStyleBackColor = true;
            this.chkKarmaAcilsin.CheckedChanged += new System.EventHandler(this.chkKarmaAcilsin_CheckedChanged);
            // 
            // lblAutoCreateStats
            // 
            this.lblAutoCreateStats.AutoSize = true;
            this.lblAutoCreateStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAutoCreateStats.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblAutoCreateStats.Location = new System.Drawing.Point(20, 98);
            this.lblAutoCreateStats.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAutoCreateStats.Name = "lblAutoCreateStats";
            this.lblAutoCreateStats.Size = new System.Drawing.Size(363, 17);
            this.lblAutoCreateStats.TabIndex = 65;
            this.lblAutoCreateStats.Text = "AutoCreate | Yeni: 0  Güncelleme: 0  Başarısız: 0";
            // 
            // timerLog
            // 
            this.timerLog.Enabled = true;
            this.timerLog.Interval = 2000;
            this.timerLog.Tick += new System.EventHandler(this.timerLog_Tick);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 603);
            this.Controls.Add(this.lblAutoCreateStats);
            this.Controls.Add(this.chkKarmaAcilsin);
            this.Controls.Add(this.txtGunSonuInterval);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblSayac3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblSayac2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblLoginHistory);
            this.Controls.Add(this.lblDbip);
            this.Controls.Add(this.listBoxCepServerList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lblSayac);
            this.Controls.Add(this.listBoxTransactions);
            this.Controls.Add(this.listBoxClientList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.menuAna);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuAna;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Server";
            this.Text = "CrmDirectFN";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Server_FormClosed);
            this.Load += new System.EventHandler(this.Server_Load);
            this.menuAna.ResumeLayout(false);
            this.menuAna.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox listBoxClientList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBoxTransactions;
        private System.Windows.Forms.MenuStrip menuAna;
        private System.Windows.Forms.ToolStripMenuItem localİşlemelerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ayarlarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bağlantıAyarlarıToolStripMenuItem;
        public nsoftware.IPWorks.Ipdaemon IpDeamon;
        public nsoftware.IPWorks.Ipdaemon IpDeamonForClilents;
        private System.Windows.Forms.ToolStripMenuItem kullanıcıListesiOkuToolStripMenuItem;
        private nsoftware.IPWorks.Http httpSSO;
        private System.Windows.Forms.Timer timerOnOffCheck;
        private System.Windows.Forms.ToolStripMenuItem kotrolSaatiToolStripMenuItem;
        private System.Windows.Forms.Label lblSayac;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.ToolStripMenuItem pushNotificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AllSendPushNotMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expiryDateKontrolToolStripMenuItem;
        private System.Windows.Forms.ListBox listBoxCepServerList;
        private System.Windows.Forms.Label label1;
        private nsoftware.IPWorks.Ipdaemon IpdaemonforCepServers;
        private System.Windows.Forms.Label lblDbip;
        public nsoftware.IPWorks.Ipport IpportSso;
        private System.Windows.Forms.Label lblLoginHistory;
        private System.Windows.Forms.Timer timerKontrol;
        private System.Windows.Forms.Label lblSayac3;
        private System.Windows.Forms.Label lblSayac2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem acilacaklarKontrolToolStripMenuItem;
        private System.Windows.Forms.Timer timerParseAutocreate;
        private System.Windows.Forms.TextBox txtGunSonuInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem sSLVePortBilgileriToolStripMenuItem;
        public nsoftware.IPWorks.Ipdaemon HttpsService;
        private System.Windows.Forms.CheckBox chkKarmaAcilsin;
        private System.Windows.Forms.ToolStripMenuItem onluPaketKapamaKontrolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dBBaglantıBilgileriToolStripMenuItem;
        private System.Windows.Forms.Timer timerLog;
        private System.Windows.Forms.Label lblAutoCreateStats;
    }
}