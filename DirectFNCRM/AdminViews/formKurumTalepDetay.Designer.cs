
namespace DirectFNCRM.AdminViews
{
    partial class formKurumTalepDetay
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
            this.button1 = new System.Windows.Forms.Button();
            this.gBoxLisans = new System.Windows.Forms.GroupBox();
            this.lViewLisans = new System.Windows.Forms.ListView();
            this.Lisans = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LisansOlay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnTalebiYap = new System.Windows.Forms.Button();
            this.gBoxMusteri = new System.Windows.Forms.GroupBox();
            this.txtMusAdres = new System.Windows.Forms.TextBox();
            this.txtMusIlce = new System.Windows.Forms.TextBox();
            this.txtMusSehir = new System.Windows.Forms.TextBox();
            this.txtMusUlke = new System.Windows.Forms.TextBox();
            this.txtMusEposta = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtMusCeptelefonu = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtMusTelefon = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtMustemsilciKodu = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtMusteriAdSoyad = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtMusHesapNo = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblsss = new System.Windows.Forms.Label();
            this.txtTalepAciklama = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.gBoxtalep = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTalepYapilmaTarihi = new System.Windows.Forms.Label();
            this.lblOnayTarihi = new System.Windows.Forms.Label();
            this.lblTalepYapan = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblSube = new System.Windows.Forms.Label();
            this.lblTalebiOnaylayan = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblTalepNo = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTalepDurum = new System.Windows.Forms.Label();
            this.lblTalepTarihi = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblTalepEden = new System.Windows.Forms.Label();
            this.lblTalepTipi = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnYapildi = new System.Windows.Forms.Button();
            this.lblKurumAd = new System.Windows.Forms.Label();
            this.gBoxLisans.SuspendLayout();
            this.gBoxMusteri.SuspendLayout();
            this.gBoxtalep.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(584, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 28);
            this.button1.TabIndex = 15;
            this.button1.Text = "Yeni Kullanıcı Maili At";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // gBoxLisans
            // 
            this.gBoxLisans.Controls.Add(this.lViewLisans);
            this.gBoxLisans.Location = new System.Drawing.Point(649, 77);
            this.gBoxLisans.Margin = new System.Windows.Forms.Padding(2);
            this.gBoxLisans.Name = "gBoxLisans";
            this.gBoxLisans.Padding = new System.Windows.Forms.Padding(2);
            this.gBoxLisans.Size = new System.Drawing.Size(314, 328);
            this.gBoxLisans.TabIndex = 13;
            this.gBoxLisans.TabStop = false;
            this.gBoxLisans.Text = "Lisanlar";
            // 
            // lViewLisans
            // 
            this.lViewLisans.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Lisans,
            this.LisansOlay});
            this.lViewLisans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lViewLisans.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lViewLisans.GridLines = true;
            this.lViewLisans.HideSelection = false;
            this.lViewLisans.Location = new System.Drawing.Point(2, 15);
            this.lViewLisans.Margin = new System.Windows.Forms.Padding(2);
            this.lViewLisans.Name = "lViewLisans";
            this.lViewLisans.Size = new System.Drawing.Size(310, 311);
            this.lViewLisans.TabIndex = 5;
            this.lViewLisans.UseCompatibleStateImageBehavior = false;
            this.lViewLisans.View = System.Windows.Forms.View.Details;
            this.lViewLisans.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lViewLisans_DrawColumnHeader);
            this.lViewLisans.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lViewLisans_DrawItem);
            // 
            // Lisans
            // 
            this.Lisans.Text = "Lisans";
            this.Lisans.Width = 150;
            // 
            // LisansOlay
            // 
            this.LisansOlay.Text = "Lisans Talep Olay";
            this.LisansOlay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.LisansOlay.Width = 150;
            // 
            // btnTalebiYap
            // 
            this.btnTalebiYap.Location = new System.Drawing.Point(879, 15);
            this.btnTalebiYap.Name = "btnTalebiYap";
            this.btnTalebiYap.Size = new System.Drawing.Size(75, 28);
            this.btnTalebiYap.TabIndex = 14;
            this.btnTalebiYap.Text = "Talebi Yap";
            this.btnTalebiYap.UseVisualStyleBackColor = true;
            this.btnTalebiYap.Click += new System.EventHandler(this.btnTalebiYap_Click);
            // 
            // gBoxMusteri
            // 
            this.gBoxMusteri.Controls.Add(this.txtMusAdres);
            this.gBoxMusteri.Controls.Add(this.txtMusIlce);
            this.gBoxMusteri.Controls.Add(this.txtMusSehir);
            this.gBoxMusteri.Controls.Add(this.txtMusUlke);
            this.gBoxMusteri.Controls.Add(this.txtMusEposta);
            this.gBoxMusteri.Controls.Add(this.label21);
            this.gBoxMusteri.Controls.Add(this.txtMusCeptelefonu);
            this.gBoxMusteri.Controls.Add(this.label20);
            this.gBoxMusteri.Controls.Add(this.txtMusTelefon);
            this.gBoxMusteri.Controls.Add(this.label19);
            this.gBoxMusteri.Controls.Add(this.label18);
            this.gBoxMusteri.Controls.Add(this.txtMustemsilciKodu);
            this.gBoxMusteri.Controls.Add(this.label17);
            this.gBoxMusteri.Controls.Add(this.txtMusteriAdSoyad);
            this.gBoxMusteri.Controls.Add(this.label16);
            this.gBoxMusteri.Controls.Add(this.txtUsername);
            this.gBoxMusteri.Controls.Add(this.txtMusHesapNo);
            this.gBoxMusteri.Controls.Add(this.label15);
            this.gBoxMusteri.Controls.Add(this.label14);
            this.gBoxMusteri.Controls.Add(this.lblUsername);
            this.gBoxMusteri.Controls.Add(this.label13);
            this.gBoxMusteri.Controls.Add(this.lblsss);
            this.gBoxMusteri.Location = new System.Drawing.Point(349, 77);
            this.gBoxMusteri.Margin = new System.Windows.Forms.Padding(2);
            this.gBoxMusteri.Name = "gBoxMusteri";
            this.gBoxMusteri.Padding = new System.Windows.Forms.Padding(2);
            this.gBoxMusteri.Size = new System.Drawing.Size(295, 322);
            this.gBoxMusteri.TabIndex = 12;
            this.gBoxMusteri.TabStop = false;
            this.gBoxMusteri.Text = "MUSTERİ";
            // 
            // txtMusAdres
            // 
            this.txtMusAdres.Location = new System.Drawing.Point(95, 230);
            this.txtMusAdres.Margin = new System.Windows.Forms.Padding(2);
            this.txtMusAdres.Multiline = true;
            this.txtMusAdres.Name = "txtMusAdres";
            this.txtMusAdres.Size = new System.Drawing.Size(192, 88);
            this.txtMusAdres.TabIndex = 3;
            // 
            // txtMusIlce
            // 
            this.txtMusIlce.Location = new System.Drawing.Point(95, 208);
            this.txtMusIlce.Margin = new System.Windows.Forms.Padding(2);
            this.txtMusIlce.Name = "txtMusIlce";
            this.txtMusIlce.Size = new System.Drawing.Size(192, 20);
            this.txtMusIlce.TabIndex = 3;
            // 
            // txtMusSehir
            // 
            this.txtMusSehir.Location = new System.Drawing.Point(95, 186);
            this.txtMusSehir.Margin = new System.Windows.Forms.Padding(2);
            this.txtMusSehir.Name = "txtMusSehir";
            this.txtMusSehir.Size = new System.Drawing.Size(192, 20);
            this.txtMusSehir.TabIndex = 3;
            // 
            // txtMusUlke
            // 
            this.txtMusUlke.Location = new System.Drawing.Point(95, 165);
            this.txtMusUlke.Margin = new System.Windows.Forms.Padding(2);
            this.txtMusUlke.Name = "txtMusUlke";
            this.txtMusUlke.Size = new System.Drawing.Size(192, 20);
            this.txtMusUlke.TabIndex = 3;
            // 
            // txtMusEposta
            // 
            this.txtMusEposta.Location = new System.Drawing.Point(95, 145);
            this.txtMusEposta.Margin = new System.Windows.Forms.Padding(2);
            this.txtMusEposta.Name = "txtMusEposta";
            this.txtMusEposta.Size = new System.Drawing.Size(192, 20);
            this.txtMusEposta.TabIndex = 3;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(10, 233);
            this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(34, 13);
            this.label21.TabIndex = 2;
            this.label21.Text = "Adres";
            // 
            // txtMusCeptelefonu
            // 
            this.txtMusCeptelefonu.Location = new System.Drawing.Point(94, 125);
            this.txtMusCeptelefonu.Margin = new System.Windows.Forms.Padding(2);
            this.txtMusCeptelefonu.Name = "txtMusCeptelefonu";
            this.txtMusCeptelefonu.Size = new System.Drawing.Size(192, 20);
            this.txtMusCeptelefonu.TabIndex = 3;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(10, 210);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(24, 13);
            this.label20.TabIndex = 2;
            this.label20.Text = "İlçe";
            // 
            // txtMusTelefon
            // 
            this.txtMusTelefon.Location = new System.Drawing.Point(94, 104);
            this.txtMusTelefon.Margin = new System.Windows.Forms.Padding(2);
            this.txtMusTelefon.Name = "txtMusTelefon";
            this.txtMusTelefon.Size = new System.Drawing.Size(192, 20);
            this.txtMusTelefon.TabIndex = 3;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(10, 190);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(31, 13);
            this.label19.TabIndex = 2;
            this.label19.Text = "Şehir";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(10, 169);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(29, 13);
            this.label18.TabIndex = 2;
            this.label18.Text = "Ülke";
            // 
            // txtMustemsilciKodu
            // 
            this.txtMustemsilciKodu.Location = new System.Drawing.Point(94, 83);
            this.txtMustemsilciKodu.Margin = new System.Windows.Forms.Padding(2);
            this.txtMustemsilciKodu.Name = "txtMustemsilciKodu";
            this.txtMustemsilciKodu.Size = new System.Drawing.Size(192, 20);
            this.txtMustemsilciKodu.TabIndex = 3;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(10, 148);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(44, 13);
            this.label17.TabIndex = 2;
            this.label17.Text = "E Posta";
            // 
            // txtMusteriAdSoyad
            // 
            this.txtMusteriAdSoyad.Location = new System.Drawing.Point(94, 62);
            this.txtMusteriAdSoyad.Margin = new System.Windows.Forms.Padding(2);
            this.txtMusteriAdSoyad.Name = "txtMusteriAdSoyad";
            this.txtMusteriAdSoyad.Size = new System.Drawing.Size(192, 20);
            this.txtMusteriAdSoyad.TabIndex = 3;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(10, 128);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(71, 13);
            this.label16.TabIndex = 2;
            this.label16.Text = "Cep Telefonu";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(94, 39);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(2);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(192, 20);
            this.txtUsername.TabIndex = 3;
            // 
            // txtMusHesapNo
            // 
            this.txtMusHesapNo.Location = new System.Drawing.Point(94, 18);
            this.txtMusHesapNo.Margin = new System.Windows.Forms.Padding(2);
            this.txtMusHesapNo.Name = "txtMusHesapNo";
            this.txtMusHesapNo.Size = new System.Drawing.Size(192, 20);
            this.txtMusHesapNo.TabIndex = 3;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 108);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(43, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Telefon";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 88);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Temsilci Kodu";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(10, 44);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Username";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 65);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Ad Soyad";
            // 
            // lblsss
            // 
            this.lblsss.AutoSize = true;
            this.lblsss.Location = new System.Drawing.Point(10, 23);
            this.lblsss.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblsss.Name = "lblsss";
            this.lblsss.Size = new System.Drawing.Size(55, 13);
            this.lblsss.TabIndex = 2;
            this.lblsss.Text = "Hesap No";
            // 
            // txtTalepAciklama
            // 
            this.txtTalepAciklama.Location = new System.Drawing.Point(7, 214);
            this.txtTalepAciklama.Margin = new System.Windows.Forms.Padding(2);
            this.txtTalepAciklama.Multiline = true;
            this.txtTalepAciklama.Name = "txtTalepAciklama";
            this.txtTalepAciklama.Size = new System.Drawing.Size(309, 104);
            this.txtTalepAciklama.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 198);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Talep açıklama";
            // 
            // gBoxtalep
            // 
            this.gBoxtalep.Controls.Add(this.txtTalepAciklama);
            this.gBoxtalep.Controls.Add(this.label3);
            this.gBoxtalep.Controls.Add(this.lblTalepYapilmaTarihi);
            this.gBoxtalep.Controls.Add(this.lblOnayTarihi);
            this.gBoxtalep.Controls.Add(this.lblTalepYapan);
            this.gBoxtalep.Controls.Add(this.label12);
            this.gBoxtalep.Controls.Add(this.label2);
            this.gBoxtalep.Controls.Add(this.label11);
            this.gBoxtalep.Controls.Add(this.lblSube);
            this.gBoxtalep.Controls.Add(this.lblTalebiOnaylayan);
            this.gBoxtalep.Controls.Add(this.label6);
            this.gBoxtalep.Controls.Add(this.label9);
            this.gBoxtalep.Controls.Add(this.lblTalepNo);
            this.gBoxtalep.Controls.Add(this.label10);
            this.gBoxtalep.Controls.Add(this.label7);
            this.gBoxtalep.Controls.Add(this.lblTalepDurum);
            this.gBoxtalep.Controls.Add(this.lblTalepTarihi);
            this.gBoxtalep.Controls.Add(this.label8);
            this.gBoxtalep.Controls.Add(this.lblTalepEden);
            this.gBoxtalep.Controls.Add(this.lblTalepTipi);
            this.gBoxtalep.Controls.Add(this.label4);
            this.gBoxtalep.Controls.Add(this.label5);
            this.gBoxtalep.Location = new System.Drawing.Point(18, 77);
            this.gBoxtalep.Margin = new System.Windows.Forms.Padding(2);
            this.gBoxtalep.Name = "gBoxtalep";
            this.gBoxtalep.Padding = new System.Windows.Forms.Padding(2);
            this.gBoxtalep.Size = new System.Drawing.Size(327, 326);
            this.gBoxtalep.TabIndex = 11;
            this.gBoxtalep.TabStop = false;
            this.gBoxtalep.Text = "TALEP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 74);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Talep Eden";
            // 
            // lblTalepYapilmaTarihi
            // 
            this.lblTalepYapilmaTarihi.AutoSize = true;
            this.lblTalepYapilmaTarihi.Location = new System.Drawing.Point(162, 180);
            this.lblTalepYapilmaTarihi.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTalepYapilmaTarihi.Name = "lblTalepYapilmaTarihi";
            this.lblTalepYapilmaTarihi.Size = new System.Drawing.Size(35, 13);
            this.lblTalepYapilmaTarihi.TabIndex = 2;
            this.lblTalepYapilmaTarihi.Text = "label3";
            // 
            // lblOnayTarihi
            // 
            this.lblOnayTarihi.AutoSize = true;
            this.lblOnayTarihi.Location = new System.Drawing.Point(162, 145);
            this.lblOnayTarihi.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOnayTarihi.Name = "lblOnayTarihi";
            this.lblOnayTarihi.Size = new System.Drawing.Size(35, 13);
            this.lblOnayTarihi.TabIndex = 2;
            this.lblOnayTarihi.Text = "label3";
            // 
            // lblTalepYapan
            // 
            this.lblTalepYapan.AutoSize = true;
            this.lblTalepYapan.Location = new System.Drawing.Point(162, 163);
            this.lblTalepYapan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTalepYapan.Name = "lblTalepYapan";
            this.lblTalepYapan.Size = new System.Drawing.Size(35, 13);
            this.lblTalepYapan.TabIndex = 2;
            this.lblTalepYapan.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Kurum Şube";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 180);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Talebi Yapılma Tarihi";
            // 
            // lblSube
            // 
            this.lblSube.AutoSize = true;
            this.lblSube.Location = new System.Drawing.Point(162, 20);
            this.lblSube.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSube.Name = "lblSube";
            this.lblSube.Size = new System.Drawing.Size(42, 13);
            this.lblSube.TabIndex = 2;
            this.lblSube.Text = "lblSube";
            // 
            // lblTalebiOnaylayan
            // 
            this.lblTalebiOnaylayan.AutoSize = true;
            this.lblTalebiOnaylayan.Location = new System.Drawing.Point(162, 128);
            this.lblTalebiOnaylayan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTalebiOnaylayan.Name = "lblTalebiOnaylayan";
            this.lblTalebiOnaylayan.Size = new System.Drawing.Size(35, 13);
            this.lblTalebiOnaylayan.TabIndex = 2;
            this.lblTalebiOnaylayan.Text = "label3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 38);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Talep No";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 145);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Talebi Onay /Red Tarihi";
            // 
            // lblTalepNo
            // 
            this.lblTalepNo.AutoSize = true;
            this.lblTalepNo.Location = new System.Drawing.Point(162, 36);
            this.lblTalepNo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTalepNo.Name = "lblTalepNo";
            this.lblTalepNo.Size = new System.Drawing.Size(10, 13);
            this.lblTalepNo.TabIndex = 2;
            this.lblTalepNo.Text = ".";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 163);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Talebi Yapan";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 56);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Talep Tarihi";
            // 
            // lblTalepDurum
            // 
            this.lblTalepDurum.AutoSize = true;
            this.lblTalepDurum.Location = new System.Drawing.Point(162, 110);
            this.lblTalepDurum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTalepDurum.Name = "lblTalepDurum";
            this.lblTalepDurum.Size = new System.Drawing.Size(35, 13);
            this.lblTalepDurum.TabIndex = 2;
            this.lblTalepDurum.Text = "label3";
            // 
            // lblTalepTarihi
            // 
            this.lblTalepTarihi.AutoSize = true;
            this.lblTalepTarihi.Location = new System.Drawing.Point(162, 54);
            this.lblTalepTarihi.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTalepTarihi.Name = "lblTalepTarihi";
            this.lblTalepTarihi.Size = new System.Drawing.Size(10, 13);
            this.lblTalepTarihi.TabIndex = 2;
            this.lblTalepTarihi.Text = ".";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 128);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(147, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Talebi Onaylayan /Reddeden";
            // 
            // lblTalepEden
            // 
            this.lblTalepEden.AutoSize = true;
            this.lblTalepEden.Location = new System.Drawing.Point(162, 74);
            this.lblTalepEden.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTalepEden.Name = "lblTalepEden";
            this.lblTalepEden.Size = new System.Drawing.Size(35, 13);
            this.lblTalepEden.TabIndex = 2;
            this.lblTalepEden.Text = "label3";
            // 
            // lblTalepTipi
            // 
            this.lblTalepTipi.AutoSize = true;
            this.lblTalepTipi.Location = new System.Drawing.Point(162, 93);
            this.lblTalepTipi.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTalepTipi.Name = "lblTalepTipi";
            this.lblTalepTipi.Size = new System.Drawing.Size(35, 13);
            this.lblTalepTipi.TabIndex = 2;
            this.lblTalepTipi.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 93);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Talep Tipi";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 110);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Talep Durumu";
            // 
            // btnYapildi
            // 
            this.btnYapildi.Location = new System.Drawing.Point(421, 11);
            this.btnYapildi.Margin = new System.Windows.Forms.Padding(2);
            this.btnYapildi.Name = "btnYapildi";
            this.btnYapildi.Size = new System.Drawing.Size(124, 28);
            this.btnYapildi.TabIndex = 9;
            this.btnYapildi.Text = "Yapıldı Olarak İşaretle";
            this.btnYapildi.UseVisualStyleBackColor = true;
            this.btnYapildi.Visible = false;
            this.btnYapildi.Click += new System.EventHandler(this.btnYapildi_Click);
            // 
            // lblKurumAd
            // 
            this.lblKurumAd.AutoSize = true;
            this.lblKurumAd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblKurumAd.ForeColor = System.Drawing.Color.Firebrick;
            this.lblKurumAd.Location = new System.Drawing.Point(22, 26);
            this.lblKurumAd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblKurumAd.Name = "lblKurumAd";
            this.lblKurumAd.Size = new System.Drawing.Size(46, 17);
            this.lblKurumAd.TabIndex = 10;
            this.lblKurumAd.Text = "label1";
            // 
            // formKurumTalepDetay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 448);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gBoxLisans);
            this.Controls.Add(this.btnTalebiYap);
            this.Controls.Add(this.gBoxMusteri);
            this.Controls.Add(this.gBoxtalep);
            this.Controls.Add(this.btnYapildi);
            this.Controls.Add(this.lblKurumAd);
            this.Name = "formKurumTalepDetay";
            this.Text = "formKurumTalepDetay";
            this.Load += new System.EventHandler(this.formKurumTalepDetay_Load);
            this.gBoxLisans.ResumeLayout(false);
            this.gBoxMusteri.ResumeLayout(false);
            this.gBoxMusteri.PerformLayout();
            this.gBoxtalep.ResumeLayout(false);
            this.gBoxtalep.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox gBoxLisans;
        private System.Windows.Forms.ListView lViewLisans;
        private System.Windows.Forms.ColumnHeader Lisans;
        private System.Windows.Forms.ColumnHeader LisansOlay;
        private System.Windows.Forms.Button btnTalebiYap;
        private System.Windows.Forms.GroupBox gBoxMusteri;
        private System.Windows.Forms.TextBox txtMusAdres;
        private System.Windows.Forms.TextBox txtMusIlce;
        private System.Windows.Forms.TextBox txtMusSehir;
        private System.Windows.Forms.TextBox txtMusUlke;
        private System.Windows.Forms.TextBox txtMusEposta;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtMusCeptelefonu;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtMusTelefon;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtMustemsilciKodu;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtMusteriAdSoyad;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtMusHesapNo;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblsss;
        private System.Windows.Forms.TextBox txtTalepAciklama;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox gBoxtalep;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTalepYapilmaTarihi;
        private System.Windows.Forms.Label lblOnayTarihi;
        private System.Windows.Forms.Label lblTalepYapan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblSube;
        private System.Windows.Forms.Label lblTalebiOnaylayan;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblTalepNo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTalepDurum;
        private System.Windows.Forms.Label lblTalepTarihi;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblTalepEden;
        private System.Windows.Forms.Label lblTalepTipi;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnYapildi;
        private System.Windows.Forms.Label lblKurumAd;
    }
}