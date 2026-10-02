using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using static DirectFNCRM.ServerViews.Server;

namespace DirectFNCRM.AdminViews
{

    public partial class formDosyadanUserIslemleri : Form
    {
        public formDosyadanUserIslemleri()
        {
            InitializeComponent();
        }

        public string yeniOkunanKullaniciSayisi = "0";
        public string iptalOkunanKullaniciSayisi = "0";
        public string Paket10OkunanKullaniciSayisi = "0";
        public string Krmd1OkunanKullaniciSayisi = "0";
        public string UserinfoOkunanKullaniciSayisi = "0";
        public string UserPasswordOkunanKullaniciSayisi = "0";
        public int count = 0;
        public List<DosyadanYeniUser> DosyadanYeniUserListe = new List<DosyadanYeniUser>();
        public List<DosyadanYeniUser> DosyadanIptalUserListe = new List<DosyadanYeniUser>();
        public List<DosyadanYeniUser> DosyadanPaket10AcKapaListe = new List<DosyadanYeniUser>();
        public List<DosyadanYeniUser> DosyadanKRMD1AcKapaListe = new List<DosyadanYeniUser>();
        public List<DosyadanYeniUser> DosyadanUserinfoListe = new List<DosyadanYeniUser>();
        public List<DosyadanYeniUser> DosyadanUserPasswordListe = new List<DosyadanYeniUser>();
        private static readonly object _logLock = new object();

        public string LisansTarihOkunanKullaniciSayisi = "0";
        public List<DosyadanYeniUser> DosyadanLisansTarihListe = new List<DosyadanYeniUser>();
        int batchSize = 100;
        private void formDosyadanUserIslemleri_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                durumYaz(ex.Message);
            }
        }

        public void durumYaz(string textx)
        {
            try
            {
                if (lboxDurum.InvokeRequired)
                { this.Invoke((MethodInvoker)delegate { durumYaz(textx); }); }
                else
                {

                    lboxDurum.Items.Insert(0, textx);
                    if (lboxDurum.Items.Count > 1000)
                        lboxDurum.Items.RemoveAt(lboxDurum.Items.Count - 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private crmDFNDataContext YeniContext()
        {
            var c = new crmDFNDataContext();
            var dlo = new DataLoadOptions();
            dlo.LoadWith<User>(u => u.LisansDurum);
            dlo.LoadWith<User>(u => u.Iletisim);
            c.LoadOptions = dlo;
            return c;
        }

        /// <summary>
        /// (USERNAME ; SON KULLANIM TARİHİ)  -> DosyadanIptalUserListe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            var satirno = "";
            int okunan = 0, atlanan = 0;
            try
            {
                DosyadanIptalUserListe.Clear();

                OpenFileDialog op = new OpenFileDialog();
                var filename = "";
                op.InitialDirectory = Application.StartupPath;
                op.Filter = "(*.csv)|*.csv";
                if (op.ShowDialog() == DialogResult.OK) filename = op.FileName; else return;

                char[] delimiterChars = { ';' };
                var filestr = File.ReadAllLines(filename, Encoding.GetEncoding("iso-8859-9"));
                if (filestr.Length < 2) { MessageBox.Show("csv dosyası boş"); return; }

                for (int i = 1; i < filestr.Length; i++)
                {
                    satirno = i.ToString();
                    if (string.IsNullOrWhiteSpace(filestr[i])) continue;

                    try
                    {
                        //var satirstr = filestr[i].Split(delimiterChars);
                        var satirstr = SplitCsv(filestr[i]);
                        if (satirstr.Length < 2)
                        {
                            durumYaz($"{satirno}. satır ATLANDI: temel alanlar eksik (kolon {satirstr.Length}).");
                            atlanan++; continue;
                        }

                        string mno = satirstr[0].Trim();
                        string expriydate = satirstr[1].Trim();

                        if (mno == "")
                        {
                            durumYaz($"{satirno}. satır ATLANDI: Username boş olamaz.");
                            atlanan++; continue;
                        }

                        DateTime parsed;
                        //if (!DateTime.TryParseExact(expriydate, "yyyyMMdd",
                        //        CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                        //{
                        //    durumYaz($"{satirno}. satır ATLANDI: tarih formatı hatalı ('{expriydate}'). Format yyyyMMdd olmalı.");
                        //    atlanan++; continue;
                        //}
                        if (!DateTime.TryParseExact(expriydate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                        {
                            durumYaz($"{satirno}. satır ATLANDI: tarih formatı hatalı ('{expriydate}'). Format yyyyMMdd olmalı.");
                            atlanan++; continue;
                        }

                        var dosuser = new DosyadanYeniUser();
                        dosuser.expirydate = parsed;
                        dosuser.Username = mno;

                        DosyadanIptalUserListe.Add(dosuser);
                        okunan++;
                        iptalOkunanKullaniciSayisi = DosyadanIptalUserListe.Count.ToString();
                    }
                    catch (Exception exRow)
                    {
                        durumYaz($"{satirno}. satır ATLANDI (hata): {exRow.Message}");
                        atlanan++;
                    }
                }

                durumYaz($"Okuma bitti. Eklenen: {okunan}, Atlanan: {atlanan}, Toplam veri satırı: {filestr.Length - 1}.");
            }
            catch (Exception ex)
            {
                durumYaz("Dosya okuma hatası: " + ex.Message);
            }
        }

        private void btnYeniKullanıcıAc_Click(object sender, EventArgs e)
        {
            DosyadanYeniUserListe.Clear();
            var satirno = "";

            //Şablondaki sabit kolon sayısı: TCKN..GKKUL = 26 kolon (index 0-25)
            const int beklenenKolonSayisi = 26;

            //Özet sayaçları
            int okunan = 0;
            int atlanan = 0;

            try
            {
                OpenFileDialog op = new OpenFileDialog();
                var filename = "";
                op.InitialDirectory = Application.StartupPath;
                op.Filter = "(*.csv)|*.csv";

                if (op.ShowDialog() == DialogResult.OK)
                    filename = op.FileName;
                else
                    return;

                char[] delimiterChars = { ';' };
                var filestr = File.ReadAllLines(filename, Encoding.GetEncoding("iso-8859-9"));
                if (filestr.Length < 2) { MessageBox.Show("csv dosyası boş"); return; }

                for (int i = 1; i < filestr.Length; i++)
                {
                    satirno = i.ToString();

                    //Boş / boşluk satırlarını atla (Excel'in sona eklediği boş satır dahil)
                    if (string.IsNullOrWhiteSpace(filestr[i]))
                        continue;

                    //Satır bazlı hata yalıtımı: bir satırın hatası tüm yüklemeyi durdurmasın
                    try
                    {
                       // var satirstr = filestr[i].Split(delimiterChars);
                        var satirstr = SplitCsv(filestr[i]);

                        //Kolon sayısı doğrulaması — serbest metindeki ';' kaymasına karşı koruma
                        if (satirstr.Length > beklenenKolonSayisi)
                        {
                            durumYaz($"{satirno}. satır ATLANDI: kolon sayısı fazla ({satirstr.Length}/{beklenenKolonSayisi}). " +
                                     $"Muhtemelen ad/soyad/adres içinde ';' var ve kolonlar kaymış.");
                            atlanan++;
                            continue;
                        }
                        if (satirstr.Length < 8)
                        {
                            durumYaz($"{satirno}. satır ATLANDI: temel alanlar eksik (kolon sayısı {satirstr.Length}).");
                            atlanan++;
                            continue;
                        }

                        string adi = "";
                        string soyad = "";
                        string mail = "";
                        string telefon = "";
                        string mno = "";
                        string expriydate = "";
                        string sehir = "";
                        string adres = "";

                        string PRO = "";
                        string CEP = "";
                        string PD1P = "";
                        string PD2 = "";
                        string Pd2P = "";
                        string END = "";
                        string PIT = "";
                        string PITE = "";
                        string MKK = "";
                        string GKKUL = "";
                        string VD1P = "";
                        string VD2 = "";
                        string Vd2P = "";
                        string VIT = "";
                        string BD1P = "";
                        string BD2 = "";
                        string KRMD1 = "";
                        string CME = "";

                        #region parse
                        for (int j = 0; j < satirstr.Length; j++)
                        {
                            switch (j)
                            {
                                case 0: mno = satirstr[j].Trim(); break;
                                case 1: adi = satirstr[j].Trim(); break;
                                case 2: soyad = satirstr[j].Trim(); break;
                                case 3: expriydate = satirstr[j].Trim(); break;
                                case 4: telefon = satirstr[j].Trim(); break;
                                case 5: mail = satirstr[j].Trim(); break;
                                case 6: sehir = satirstr[j].Trim(); break;
                                case 7: adres = satirstr[j].Trim(); break;
                                case 8: PRO = satirstr[j].Trim(); break;
                                case 9: CEP = satirstr[j].Trim(); break;
                                case 10: PD1P = satirstr[j].Trim(); break;
                                case 11: PD2 = satirstr[j].Trim(); break;
                                case 12: Pd2P = satirstr[j].Trim(); break;
                                case 13: END = satirstr[j].Trim(); break;
                                case 14: PIT = satirstr[j].Trim(); break;
                                case 15: PITE = satirstr[j].Trim(); break;
                                case 16: VD1P = satirstr[j].Trim(); break;
                                case 17: VD2 = satirstr[j].Trim(); break;
                                case 18: Vd2P = satirstr[j].Trim(); break;
                                case 19: VIT = satirstr[j].Trim(); break;
                                case 20: BD1P = satirstr[j].Trim(); break;
                                case 21: BD2 = satirstr[j].Trim(); break;
                                case 22: KRMD1 = satirstr[j].Trim(); break;
                                case 23: CME = satirstr[j].Trim(); break;   // ALG (DB'de CME)
                                case 24: MKK = satirstr[j].Trim(); break;
                                case 25: GKKUL = satirstr[j].Trim(); break;
                            }
                        }
                        #endregion

                        // [DEĞİŞTİ-2] Username boşsa: return DEĞİL, sadece bu satırı atla
                        if (mno == "")
                        {
                            durumYaz($"{satirno}. satır ATLANDI: TCKN/Username boş olamaz.");
                            atlanan++;
                            continue;
                        }

                        var dosuser = new DosyadanYeniUser();

                        // [DEĞİŞTİ-2] ParseExact yerine TryParseExact — bozuk tarih satırı atlar, yüklemeyi çökertmez
                        DateTime parsedDate;
                        if (!DateTime.TryParseExact(expriydate, "yyyyMMdd",
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None, out parsedDate))
                        {
                            durumYaz($"{satirno}. satır ATLANDI: son kullanım tarihi formatı hatalı ('{expriydate}'). " +
                                     $"Format yyyyMMdd olmalı (örn. 20251231).");
                            atlanan++;
                            continue;
                        }
                        dosuser.expirydate = parsedDate;

                        dosuser.Ad = adi;
                        dosuser.Soyad = soyad;
                        dosuser.Tckn = mno;
                        dosuser.Username = mno;
                        dosuser.mail = mail;
                        dosuser.Tel = telefon;
                        dosuser.Sehir = sehir;
                        dosuser.Adres = adres;

                        dosuser.DESKTOP = PRO._ToBool();
                        dosuser.MOBIL = CEP._ToBool();
                        dosuser.PD1P = PD1P._ToBool();
                        dosuser.PD2 = PD2._ToBool();
                        dosuser.PD2P = Pd2P._ToBool();
                        dosuser.END = END._ToBool();
                        dosuser.PIT = PIT._ToBool();
                        dosuser.PITE = PITE._ToBool();
                        dosuser.VD1P = VD1P._ToBool();
                        dosuser.VD2 = VD2._ToBool();
                        dosuser.VD2P = Vd2P._ToBool();
                        dosuser.VIT = VIT._ToBool();
                        dosuser.BD1P = BD1P._ToBool();
                        dosuser.BD2 = BD2._ToBool();
                        dosuser.KRMD1 = KRMD1._ToBool();
                        dosuser.CME = CME._ToBool();
                        dosuser.MKK = MKK._ToBool();
                        dosuser.GKKUL = GKKUL._ToBool();

                        // Lisans hiyerarşisi (mevcut mantık aynen korundu)
                        if (dosuser.PD1P) { dosuser.PD1 = true; }
                        if (dosuser.PD2) { dosuser.PD1 = true; dosuser.PD1P = true; }
                        if (dosuser.PD2P) { dosuser.PD1 = true; dosuser.PD1P = true; dosuser.PD2 = true; }
                        if (dosuser.VD1P) { dosuser.VD1 = true; }
                        if (dosuser.VD2) { dosuser.VD1 = true; dosuser.VD1P = true; }
                        if (dosuser.VD2P) { dosuser.VD1 = true; dosuser.VD1P = true; dosuser.VD2 = true; }
                        if (dosuser.BD1P) { dosuser.BD1 = true; }
                        if (dosuser.BD2) { dosuser.BD1 = true; dosuser.BD1P = true; }

                        DosyadanYeniUserListe.Add(dosuser);
                        okunan++;
                        yeniOkunanKullaniciSayisi = DosyadanYeniUserListe.Count.ToString();
                    }
                    catch (Exception exRow)
                    {
                        // [YENİ-2] Satır bazlı hata — diğer satırlar etkilenmeden devam eder
                        durumYaz($"{satirno}. satır ATLANDI (hata): {exRow.Message}");
                        atlanan++;
                    }
                }

                // [YENİ-2] Okuma özeti
                durumYaz($"Okuma bitti. Eklenen: {okunan}, Atlanan: {atlanan}, Toplam veri satırı: {filestr.Length - 1}.");
            }
            catch (Exception ex)
            {
                durumYaz("Dosya okuma hatası: " + ex.Message);
            }
        }

        private void btnYeniUserOrnekOlustur_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("TCKN");
                sb.Append(";");
                sb.Append("AD");
                sb.Append(";");
                sb.Append("SOYAD");
                sb.Append(";");
                sb.Append("SON KULLANIM TARİHİ");
                sb.Append(";");
                sb.Append("TELEFON");
                sb.Append(";");
                sb.Append("MAİL");
                sb.Append(";");
                sb.Append("SEHİR");
                sb.Append(";");
                sb.Append("ADRES");
                sb.Append(";");

                sb.Append("DESKTOP");
                sb.Append(";");
                sb.Append("MOBIL");
                sb.Append(";");

                sb.Append("PD1P");
                sb.Append(";");
                sb.Append("PD2");
                sb.Append(";");
                sb.Append("PD2P");
                sb.Append(";");
                sb.Append("END");
                sb.Append(";");
                sb.Append("PIT");
                sb.Append(";");
                sb.Append("PITE");
                sb.Append(";");

                sb.Append("VD1P");
                sb.Append(";");
                sb.Append("VD2");
                sb.Append(";");
                sb.Append("VD2P");
                sb.Append(";");
                sb.Append("VIT");
                sb.Append(";");

                sb.Append("BD1P");
                sb.Append(";");
                sb.Append("BD2");
                sb.Append(";");
                sb.Append("KRMD1");
                sb.Append(";");
                sb.Append("ALG");
                sb.Append(";");
                sb.Append("MKK");
                sb.Append(";");
                sb.Append("GKKUL");
                sb.Append(";");
                //sb.Append("ALG");
                //sb.Append(";");

                sb.Append(Environment.NewLine);

                sb.Append("12345678901");
                sb.Append(";");
                sb.Append("Kulllanıcı Adı");
                sb.Append(";");
                sb.Append("Kullanıcı Soyadı");
                sb.Append(";");
                sb.Append("20261231");
                sb.Append(";");
                sb.Append("05551234567");
                sb.Append(";");
                sb.Append("test@ideal.data.com.tr");
                sb.Append(";");
                sb.Append("İstanbul");
                sb.Append(";");
                sb.Append("Barbaros Mah. Ihlamur Blv. No:3");
                sb.Append(";");

                sb.Append("1;0;1;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0");

                string filename = Application.StartupPath + "\\DosyadanYeniKullanciAc.csv";

                if (File.Exists(filename))
                    File.Delete(filename);

                File.WriteAllText(filename, sb.ToString(), Encoding.GetEncoding("iso-8859-9"));

                Process.Start(filename);

            }
            catch (Exception ex)
            {
                durumYaz(ex.Message);
            }
        }

        private void btnIptalUserOrnekOlustur_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();



                sb.Append("USERNAME");
                sb.Append(";");
                sb.Append("SON KULLANIM TARİHİ");
                sb.Append(";");


                sb.Append(Environment.NewLine);

                sb.Append("12345678901");
                sb.Append(";");
                sb.Append("20251231");
                sb.Append(";");


                string filename = Application.StartupPath + "\\DosyadanKullaniciTarih.csv";

                if (File.Exists(filename))
                    File.Delete(filename);


                File.WriteAllText(filename, sb.ToString(), Encoding.GetEncoding("iso-8859-9"));

                Process.Start(filename);
            }
            catch (Exception ex)
            {
                durumYaz(ex.Message);
            }

        }

        private void timerKontrol_Tick(object sender, EventArgs e)
        {
            try
            {
                lblyeniOkunanKlullaniciSayisi.Text = yeniOkunanKullaniciSayisi;
                lblIptalOkunanKlullaniciSayisi.Text = iptalOkunanKullaniciSayisi;
                lblUserinfoOkunanKlullaniciSayisi.Text = UserinfoOkunanKullaniciSayisi;
                lblUserPasswordOkunanKlullaniciSayisi.Text = UserPasswordOkunanKullaniciSayisi;
                lblKRMD1AcKapaOkunanKlullaniciSayisi.Text = Krmd1OkunanKullaniciSayisi;
                lblPaket10AcKapaOkunanKlullaniciSayisi.Text = Paket10OkunanKullaniciSayisi;
                lblLisansTarihOkunanKlullaniciSayisi.Text = LisansTarihOkunanKullaniciSayisi;
            }
            catch { }
        }
        public delegate void lblsayacguncelle(Label label, string text);
        public void LabelYaz(Label label, string text)
        {
            try
            {
                if (label.InvokeRequired)
                {
                    lblsayacguncelle lbl = new lblsayacguncelle(LabelYaz);

                    this.Invoke(lbl, new object[] { label, text });

                }
                else
                {
                    label.Text = text;
                }
            }
            catch (Exception ex)
            {

                durumYaz(ex.Message);
            }
        }
        //private void btnYeniSendServer_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (MessageBox.Show("Okutulan Tüm Kullanıcılar Server'a gönderilecektir ", "Kullanıcı Açma İşlemi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
        //        {
        //            return;
        //        }

        //        Task thred = Task.Factory.StartNew(() =>
        //        {
        //            if (DosyadanYeniUserListe.Count < 1)
        //            {
        //                durumYaz("Okutulan Liste Boş"); return;
        //            }
        //            LabelYaz(lblYeniGonderimDurum, "0");
        //            var say = 1;
        //            var count = DosyadanYeniUserListe.Count;

        //            var crm = new crmDFNDataContext();
        //            for (int i = 0; i < DosyadanYeniUserListe.Count; i++)
        //            {
        //                if (i > 0 && i % batchSize == 0)
        //                {
        //                    crm.Dispose();
        //                    var dlo = new DataLoadOptions();
        //                    dlo.LoadWith<User>(u => u.LisansDurum);
        //                    dlo.LoadWith<User>(u => u.Iletisim);
        //                    crm.LoadOptions = dlo;
        //                }
        //                Thread.Sleep(300);
        //                UserEvent ue = new UserEvent();
        //                ue.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
        //                ue.EventTarih = DateTime.Now;
        //                ue.IP = formAdminAra.referance.IpAdress;
        //                ue.HostName = formAdminAra.referance.HostName;

        //                var dosyaUser = DosyadanYeniUserListe[i];

        //                //if (crm.Users.Where(x => x.UserName == dosyaUser.HesapNo).Any())
        //                //  if (crm.Users.Where(x => x.tckno == dosyaUser.Tckn).Any())
        //                if (crm.Users.Where(x => x.UserName == dosyaUser.Username).Any())
        //                {
        //                    //UPDATE USER

        //                    var userx = crm.Users.FirstOrDefault(x => x.UserName == dosyaUser.Username);
        //                    try
        //                    {

        //                        #region Guncelle
        //                        var kaptianlar = new StringBuilder();
        //                        var acilanlar = new StringBuilder();
        //                        ue.UserId = userx.UserID;
        //                        userx.Name = dosyaUser.Ad;
        //                        userx.Surname = dosyaUser.Soyad;
        //                        userx.Aciklama = dosyaUser.Ad + " " + dosyaUser.Soyad;
        //                        userx.ExpiryDate = dosyaUser.expirydate;

        //                        LisansDurum lisanslar = new LisansDurum();

        //                        var yayind = true;
        //                        lisanslar.CepYetki = dosyaUser.MOBIL;
        //                        lisanslar.ProYetki = dosyaUser.DESKTOP;
        //                        lisanslar.Futgck = true;
        //                        lisanslar.WINX = true;
        //                        // int lastLisansId = (int)activeuser.LisansDurumId;
        //                        if (userx.LisansDurum.YayinDurumu != yayind)
        //                        {
        //                            if (yayind == true)
        //                                ue.EventTypeId = 1;
        //                            else
        //                                ue.EventTypeId = 2;

        //                            lisanslar.YayinDurumu = yayind;

        //                           // crm.UserEvents.InsertOnSubmit(ue);
        //                           // crm.SubmitChanges();
        //                        }
        //                        else
        //                            lisanslar.YayinDurumu = yayind;

        //                        bool yayinAcildi = (userx.LisansDurum.YayinDurumu != yayind && yayind);
        //                        lisanslar.YayinDurumu = yayind;
        //                        #region iletisim


        //                        if (userx.iletisimId == null)
        //                        {
        //                            var iletisim = new Iletisim();
        //                            if (dosyaUser.Adres != "")
        //                                iletisim.acikadres = dosyaUser.Adres;
        //                            if (dosyaUser.Tel != "")
        //                                iletisim.Tel1 = dosyaUser.Tel;
        //                            if (dosyaUser.Sehir != "")
        //                            {
        //                                var il = MyTools.SehirIdBul(dosyaUser.Sehir);
        //                                if (il != null && il.Id > 0)
        //                                {
        //                                    iletisim.IlId = il.Id;
        //                                    iletisim.UlkeId = il.UlkeId;
        //                                }
        //                            }
        //                            if (dosyaUser.mail != "")
        //                                iletisim.email = dosyaUser.mail;

        //                            crm.Iletisims.InsertOnSubmit(iletisim);
        //                            crm.SubmitChanges();

        //                            userx.iletisimId = iletisim.IletisimId;
        //                        }
        //                        else
        //                        {
        //                            if (dosyaUser.Adres != "")
        //                                userx.Iletisim.acikadres = dosyaUser.Adres;
        //                            if (dosyaUser.Tel != "")
        //                                userx.Iletisim.Tel1 = dosyaUser.Tel;
        //                            if (dosyaUser.Sehir != "")
        //                            {
        //                                var il = MyTools.SehirIdBul(dosyaUser.Sehir);
        //                                if (il != null && il.Id > 0)
        //                                {
        //                                    userx.Iletisim.IlId = il.Id;
        //                                    userx.Iletisim.UlkeId = il.UlkeId;
        //                                }
        //                            }
        //                            if (dosyaUser.mail != "")
        //                                userx.Iletisim.email = dosyaUser.mail;
        //                        }


        //                        #endregion
        //                        #region Lisanslar

        //                        string tstr = DateTime.Now.ToString("yyyy-MM-dd");

        //                        DateTime now = DateTime.Now;
        //                        var aySonuDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        //                        var aySonu = aySonuDate.ToString("yyyy-MM-dd");
        //                        #region Desktop
        //                        if (dosyaUser.DESKTOP)
        //                        {
        //                            acilanlar.Append("ProYetki;");
        //                            lisanslar.ProYetki = true;
        //                            lisanslar.ProYetkiStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            // lisanslar.ProYetkiEnd = DateTime.ParseExact(aySonu, "yyyy-MM-dd", CultureInfo.CurrentCulture);
        //                            lisanslar.ProYetkiEnd = dosyaUser.expirydate;
        //                        }
        //                        else
        //                        {
        //                            lisanslar.ProYetki = userx.LisansDurum.ProYetki;
        //                            lisanslar.ProYetkiStart = userx.LisansDurum.ProYetkiStart;
        //                            lisanslar.ProYetkiEnd = userx.LisansDurum.ProYetkiEnd;
        //                        }

        //                        #endregion
        //                        #region Mobil
        //                        if (dosyaUser.MOBIL)
        //                        {
        //                            acilanlar.Append("CepYetki;");
        //                            lisanslar.CepYetki = true;
        //                            lisanslar.CepYetkiStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            // lisanslar.CepYetkiEnd = DateTime.ParseExact(aySonu, "yyyy-MM-dd", CultureInfo.CurrentCulture);
        //                            lisanslar.CepYetkiEnd = dosyaUser.expirydate;

        //                        }
        //                        else
        //                        {
        //                            lisanslar.CepYetki = userx.LisansDurum.CepYetki;
        //                            lisanslar.CepYetkiStart = userx.LisansDurum.CepYetkiStart;
        //                            lisanslar.CepYetkiEnd = userx.LisansDurum.CepYetkiEnd;
        //                        }

        //                        #endregion
        //                        #region PD1P
        //                        if (dosyaUser.PD1P)
        //                        {
        //                            acilanlar.Append("PayLP;");
        //                            lisanslar.PayLP = true;
        //                            lisanslar.PayL1 = true;
        //                            lisanslar.PayLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            //lisanslar.PayLPEnd= DateTime.ParseExact(aySonu, "yyyy-MM-dd", CultureInfo.CurrentCulture);
        //                            lisanslar.PayLPEnd = dosyaUser.expirydate;
        //                            lisanslar.PayL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayL1End = dosyaUser.expirydate;
        //                        }
        //                        else
        //                        {
        //                            lisanslar.PayLP = userx.LisansDurum.PayLP;
        //                            lisanslar.PayLPStart = userx.LisansDurum.PayLPStart;
        //                            lisanslar.PayLPEnd = userx.LisansDurum.PayLPEnd;

        //                        }

        //                        #endregion
        //                        #region PD2
        //                        if (dosyaUser.PD2)
        //                        {
        //                            acilanlar.Append("PayLP;PayL2;");
        //                            lisanslar.PayL1 = true;
        //                            lisanslar.PayLP = true;
        //                            lisanslar.PayL2 = true;

        //                            lisanslar.PayL2Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayLPEnd = dosyaUser.expirydate;
        //                            lisanslar.PayL2End = dosyaUser.expirydate;
        //                            lisanslar.PayL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayL1End = dosyaUser.expirydate;
        //                        }
        //                        else
        //                        {
        //                            lisanslar.PayL2 = userx.LisansDurum.PayL2;
        //                            lisanslar.PayL2Start = userx.LisansDurum.PayL2Start;
        //                            lisanslar.PayL2End = userx.LisansDurum.PayL2End;

        //                        }

        //                        #endregion
        //                        #region PD2P
        //                        if (dosyaUser.PD2P)
        //                        {
        //                            acilanlar.Append("PayLP;PayL2;PD2P");
        //                            lisanslar.PayL1 = true;
        //                            lisanslar.PayLP = true;
        //                            lisanslar.PayL2 = true;
        //                            lisanslar.Pd2P = true;
        //                            lisanslar.PayLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayL2Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.Pd2PStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayLPEnd = dosyaUser.expirydate;
        //                            lisanslar.PayL2End = dosyaUser.expirydate;
        //                            lisanslar.Pd2PEnd = dosyaUser.expirydate;
        //                            lisanslar.PayL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayL1End = dosyaUser.expirydate;
        //                        }
        //                        else
        //                        {
        //                            lisanslar.Pd2P = userx.LisansDurum.Pd2P;
        //                            lisanslar.Pd2PStart = userx.LisansDurum.Pd2PStart;
        //                            lisanslar.Pd2PEnd = userx.LisansDurum.Pd2PEnd;

        //                        }

        //                        #endregion
        //                        #region END
        //                        if (dosyaUser.END)
        //                        {
        //                            acilanlar.Append("PayX;");
        //                            lisanslar.PayX = true;
        //                            lisanslar.PayXStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayXEnd = dosyaUser.expirydate;

        //                        }
        //                        else
        //                        {
        //                            lisanslar.PayX = userx.LisansDurum.PayX;
        //                            lisanslar.PayXStart = userx.LisansDurum.PayXStart;
        //                            lisanslar.PayXEnd = userx.LisansDurum.PayXEnd;
        //                        }

        //                        #endregion
        //                        #region PIT
        //                        if (dosyaUser.PIT)
        //                        {
        //                            acilanlar.Append("PayGS;");
        //                            lisanslar.PayGS = true;
        //                            lisanslar.PayGSStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayGSEnd = dosyaUser.expirydate;

        //                        }
        //                        else
        //                        {
        //                            lisanslar.PayGS = userx.LisansDurum.PayGS;
        //                            lisanslar.PayGSStart = userx.LisansDurum.PayGSStart;
        //                            lisanslar.PayGSEnd = userx.LisansDurum.PayGSEnd;
        //                        }

        //                        #endregion
        //                        #region PITE
        //                        if (dosyaUser.PITE)
        //                        {
        //                            acilanlar.Append("PayGS;PITE;");
        //                            lisanslar.PayGS = true;
        //                            lisanslar.PITE = true;
        //                            lisanslar.PayGSStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayPiteStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayGSEnd = dosyaUser.expirydate;
        //                            lisanslar.PayPiteEnd = dosyaUser.expirydate;
        //                        }
        //                        else
        //                        {
        //                            lisanslar.PITE = userx.LisansDurum.PITE;
        //                            lisanslar.PayPiteStart = userx.LisansDurum.PayPiteStart;
        //                            lisanslar.PayPiteEnd = userx.LisansDurum.PayPiteEnd;

        //                        }

        //                        #endregion
        //                        #region MKK
        //                        if (dosyaUser.MKK)
        //                        {
        //                            acilanlar.Append("MKK;");
        //                            lisanslar.MKK = true;
        //                            lisanslar.MKKStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.MKKEnd = dosyaUser.expirydate;

        //                        }
        //                        else
        //                        {
        //                            lisanslar.MKK = userx.LisansDurum.MKK;
        //                            lisanslar.MKKStart = userx.LisansDurum.MKKStart;
        //                            lisanslar.MKKEnd = userx.LisansDurum.MKKEnd;
        //                        }

        //                        #endregion

        //                        #region GKKUL
        //                        if (dosyaUser.GKKUL)
        //                        {
        //                            acilanlar.Append("GKKUL;");
        //                            lisanslar.GKKUL = true;
        //                            lisanslar.GKKULStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.GKKULEnd = dosyaUser.expirydate;

        //                        }
        //                        else
        //                        {
        //                            lisanslar.GKKUL = userx.LisansDurum.GKKUL;
        //                            lisanslar.GKKULStart = userx.LisansDurum.GKKULStart;
        //                            lisanslar.GKKULEnd = userx.LisansDurum.GKKULEnd;
        //                        }
        //                        #endregion

        //                        #region VD1P
        //                        if (dosyaUser.VD1P)
        //                        {
        //                            acilanlar.Append("ViopLP;");
        //                            lisanslar.ViopL1 = true;
        //                            lisanslar.ViopLP = true;
        //                            lisanslar.ViopLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopLPEnd = dosyaUser.expirydate;
        //                            lisanslar.ViopL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopL1End = dosyaUser.expirydate;

        //                        }
        //                        else
        //                        {
        //                            lisanslar.ViopLP = userx.LisansDurum.ViopLP;
        //                            lisanslar.ViopLPStart = userx.LisansDurum.ViopLPStart;
        //                            lisanslar.ViopLPEnd = userx.LisansDurum.ViopLPEnd;


        //                        }
        //                        #endregion
        //                        #region VD2
        //                        if (dosyaUser.VD2)
        //                        {
        //                            acilanlar.Append("ViopLP;ViopL2;");
        //                            lisanslar.ViopL1 = true;
        //                            lisanslar.ViopLP = true;
        //                            lisanslar.ViopL2 = true;
        //                            lisanslar.ViopL2Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopL2End = dosyaUser.expirydate;
        //                            lisanslar.ViopL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopL1End = dosyaUser.expirydate;
        //                        }
        //                        else
        //                        {
        //                            lisanslar.ViopL2 = userx.LisansDurum.ViopL2;
        //                            lisanslar.ViopL2Start = userx.LisansDurum.ViopL2Start;
        //                            lisanslar.ViopL2End = userx.LisansDurum.ViopL2End;

        //                        }

        //                        #endregion
        //                        #region VD2P
        //                        if (dosyaUser.VD2P)
        //                        {
        //                            acilanlar.Append("ViopLP;ViopL2;VD2P;");
        //                            lisanslar.ViopL1 = true;
        //                            lisanslar.ViopLP = true;
        //                            lisanslar.ViopL2 = true;
        //                            lisanslar.Vd2P = true;

        //                            lisanslar.ViopLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopL2Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.Vd2PStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopLPEnd = dosyaUser.expirydate;
        //                            lisanslar.ViopL2End = dosyaUser.expirydate;
        //                            lisanslar.Vd2PEnd = dosyaUser.expirydate;
        //                            lisanslar.ViopL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopL1End = dosyaUser.expirydate;
        //                        }
        //                        else
        //                        {
        //                            lisanslar.Vd2P = userx.LisansDurum.Vd2P;
        //                            lisanslar.Vd2PStart = userx.LisansDurum.Vd2PStart;
        //                            lisanslar.Vd2PEnd = userx.LisansDurum.Vd2PEnd;
        //                        }

        //                        #endregion
        //                        #region VIT
        //                        if (dosyaUser.VIT)
        //                        {
        //                            acilanlar.Append("ViopGS;");
        //                            lisanslar.ViopGS = true;
        //                            lisanslar.ViopGSStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopGSEnd = dosyaUser.expirydate;

        //                        }
        //                        else
        //                        {
        //                            lisanslar.ViopGS = userx.LisansDurum.ViopGS;
        //                            lisanslar.ViopGSStart = userx.LisansDurum.ViopGSStart;
        //                            lisanslar.ViopGSEnd = userx.LisansDurum.ViopGSEnd;
        //                        }

        //                        #endregion
        //                        #region BD1P
        //                        if (dosyaUser.BD1P)
        //                        {
        //                            acilanlar.Append("TahvilLP;");
        //                            lisanslar.TahvilL1 = true;
        //                            lisanslar.TahvilLP = true;
        //                            lisanslar.TahvilLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.TahvilL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.TahvilL1End = dosyaUser.expirydate;
        //                            lisanslar.TahvilLPEnd = dosyaUser.expirydate;

        //                        }
        //                        else
        //                        {
        //                            lisanslar.TahvilLP = userx.LisansDurum.TahvilLP;
        //                            lisanslar.TahvilLPStart = userx.LisansDurum.TahvilLPStart;
        //                            lisanslar.TahvilLPEnd = userx.LisansDurum.TahvilLPEnd;

        //                        }
        //                        #endregion
        //                        #region BD2
        //                        if (dosyaUser.BD2)
        //                        {
        //                            acilanlar.Append("TahvilLP;TahvilL2;");
        //                            lisanslar.TahvilL1 = true;
        //                            lisanslar.TahvilLP = true;
        //                            lisanslar.TahvilL2 = true;
        //                            lisanslar.TahvilLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.TahvilL2Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.TahvilLPEnd = dosyaUser.expirydate;
        //                            lisanslar.TahvilL2End = dosyaUser.expirydate;
        //                            lisanslar.TahvilL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.TahvilL1End = dosyaUser.expirydate;

        //                        }
        //                        else
        //                        {
        //                            lisanslar.TahvilL2 = userx.LisansDurum.TahvilL2;
        //                            lisanslar.TahvilL2Start = userx.LisansDurum.TahvilL2Start;
        //                            lisanslar.TahvilL2End = userx.LisansDurum.TahvilL2End;

        //                        }
        //                        #endregion
        //                        #region KRMD1
        //                        if (dosyaUser.KRMD1)
        //                        {
        //                            acilanlar.Append("KRMD1;");
        //                            lisanslar.COMEX = true;
        //                            lisanslar.KRMD1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.KRMD1End = dosyaUser.expirydate;

        //                        }
        //                        else
        //                        {
        //                            lisanslar.COMEX = userx.LisansDurum.COMEX;
        //                            lisanslar.KRMD1Start = userx.LisansDurum.KRMD1Start;
        //                            lisanslar.KRMD1End = userx.LisansDurum.KRMD1End;
        //                        }

        //                        #endregion
        //                        #region CME
        //                        if (dosyaUser.CME)
        //                        {
        //                            acilanlar.Append("CME;");
        //                            lisanslar.CME = true;
        //                            lisanslar.CMEStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.CMEEnd = dosyaUser.expirydate;
        //                        }
        //                        else
        //                        {
        //                            lisanslar.CME = userx.LisansDurum.CME;
        //                            lisanslar.CMEStart = userx.LisansDurum.CMEStart;
        //                            lisanslar.CMEEnd = userx.LisansDurum.CMEEnd;
        //                        }



        //                        #endregion

        //                        //lisanslar.ROBOT = userx.LisansDurum.ROBOT;
        //                        // lisanslar.SPI = userx.LisansDurum.SPI;
        //                        #endregion
        //                        //  ue.AcilanLisans = acilanlar.ToString();

        //                        if (acilanlar.Length > 0 || kaptianlar.Length > 0)
        //                        {
        //                            if (acilanlar.Length > 0 && kaptianlar.Length > 0) ue.EventTypeId = 6;
        //                            else if (acilanlar.Length > 0) ue.EventTypeId = 4;
        //                            else ue.EventTypeId = 3;
        //                            ue.AcilanLisans = acilanlar.ToString();
        //                            ue.KapatilanLisans = kaptianlar.ToString();
        //                        }
        //                        else if (yayinAcildi)
        //                        {
        //                            ue.EventTypeId = 1;   // sadece yayın açıldı, lisans değişmedi
        //                        }
        //                        else
        //                        {
        //                            ue.EventTypeId = 6;   // sadece bilgi/tarih güncellemesi
        //                        }
        //                        crm.LisansDurums.InsertOnSubmit(lisanslar);
        //                        crm.SubmitChanges();
        //                        userx.LisansDurum = lisanslar;
        //                        crm.SubmitChanges();
        //                        ue.SonLisandurumID = lisanslar.LisansDurumId;
        //                        userx.LisansDurumId = lisanslar.LisansDurumId;
        //                        if (ue.EventId < 1)
        //                            crm.UserEvents.InsertOnSubmit(ue);
        //                        crm.SubmitChanges();
        //                        formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + userx.UserID.ToString() + (char)3;
        //                        durumYaz(userx.UserName + " nolu müşteri servera gönderildi.(Değiştirme)");
        //                        //durumYaz(userx.tckno + " TCKN nolu müşteri servera gönderildi.(Değiştirme)");
        //                        LogDosyadanIslem(
        //                                    operasyon: "KullanicıGuncelle",
        //                                    tckn: userx.UserName,
        //                                    calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
        //                                               formAdminAra.referance.ActiveCalisan.Soyad,
        //                                    ip: formAdminAra.referance.IpAdress,
        //                                    hostname: formAdminAra.referance.HostName,
        //                                    degisiklikler: $"Açılan: {acilanlar} | Kapatılan: {kaptianlar} | ExpiryDate: {dosyaUser.expirydate:yyyyMMdd}"
        //                                );
        //                        #endregion
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MyTools.logyaz($"Dosyadan kullanıcı işlemleri Güncelleme hatası: {ex.Message}");
        //                        durumYaz(Text = $"[HATA]  {userx.UserName}: {ex.Message}");
        //                        LogDosyadanIslem(
        //                                    operasyon: "KullanicıGuncelleHata",
        //                                    tckn: userx.UserName,
        //                                    calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
        //                                               formAdminAra.referance.ActiveCalisan.Soyad,
        //                                    ip: formAdminAra.referance.IpAdress,
        //                                    hostname: formAdminAra.referance.HostName,
        //                                    degisiklikler: $"Hata: {ex.Message}"
        //                                );

        //                    }
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        #region CreateUser
        //                        var sifre = "Akyatirim1";
        //                        var newcustomer = new User();
        //                        var iletisim = new Iletisim();
        //                        var kurumbilgi = new KurumsalBilgiler();
        //                        var acilanlar = new StringBuilder();
        //                        //newcustomer.UserName = dosyaUser.HesapNo;
        //                        newcustomer.UserName = dosyaUser.Username;
        //                        newcustomer.tckno = dosyaUser.Tckn;


        //                        //newcustomer.PmtsNo = dosyaUser.HesapNo;
        //                        // newcustomer.PmtsNo = "10011";
        //                        newcustomer.Aciklama = dosyaUser.Ad + " " + dosyaUser.Soyad;
        //                        newcustomer.Name = dosyaUser.Ad;
        //                        newcustomer.Surname = dosyaUser.Soyad;
        //                        newcustomer.Password = MyTools.Sifreleme.Encryp(sifre);
        //                        newcustomer.ExpiryDate = dosyaUser.expirydate;
        //                        newcustomer.MusteriMenseiID = 1;
        //                        newcustomer.ProductType = "IDEAL";
        //                        newcustomer.BaslangicTarihi = DateTime.Now;
        //                        newcustomer.StatusId = 1;

        //                        #region Lisanslar


        //                        LisansDurum lisanslar = new LisansDurum();


        //                        lisanslar.CepYetki = dosyaUser.MOBIL;
        //                        lisanslar.ProYetki = dosyaUser.DESKTOP;
        //                        //lisanslar.CepYetki = true;
        //                        lisanslar.Futgck = true;
        //                        lisanslar.WINX = true;
        //                        lisanslar.YayinDurumu = true;
        //                        ue.EventTypeId = 5;



        //                        #region iletisim

        //                        if (dosyaUser.Adres != "")
        //                            iletisim.acikadres = dosyaUser.Adres;
        //                        if (dosyaUser.Tel != "")
        //                            iletisim.Tel1 = dosyaUser.Tel;
        //                        if (dosyaUser.Sehir != "")
        //                        {
        //                            var ilx = MyTools.SehirIdBul(dosyaUser.Sehir);
        //                            if (ilx != null && ilx.Id > 0)
        //                            {
        //                                iletisim.IlId = ilx.Id;
        //                                iletisim.UlkeId = ilx.UlkeId;
        //                            }
        //                        }
        //                        if (dosyaUser.mail != "")
        //                            iletisim.email = dosyaUser.mail;


        //                        #endregion


        //                        #region Lisanslar

        //                        string tstr = DateTime.Now.ToString("yyyy-MM-dd");

        //                        DateTime now = DateTime.Now;
        //                        var aySonuDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        //                        var aySonu = aySonuDate.ToString("yyyy-MM-dd");
        //                        #region Desktop
        //                        if (dosyaUser.DESKTOP)
        //                        {
        //                            acilanlar.Append("ProYetki;");
        //                            lisanslar.ProYetki = true;
        //                            lisanslar.ProYetkiStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ProYetkiEnd = dosyaUser.expirydate;
        //                        }

        //                        #endregion
        //                        #region Mobil
        //                        if (dosyaUser.MOBIL)
        //                        {
        //                            acilanlar.Append("CepYetki;");
        //                            lisanslar.CepYetki = true;
        //                            lisanslar.CepYetkiStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.CepYetkiEnd = dosyaUser.expirydate;

        //                        }

        //                        #endregion
        //                        #region PD1P
        //                        if (dosyaUser.PD1P)
        //                        {
        //                            acilanlar.Append("PayLP;");
        //                            lisanslar.PayLP = true;
        //                            lisanslar.PayL1 = true;
        //                            lisanslar.PayLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayLPEnd = dosyaUser.expirydate;
        //                            lisanslar.PayL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayL1End = dosyaUser.expirydate;

        //                        }

        //                        #endregion
        //                        #region PD2
        //                        if (dosyaUser.PD2)
        //                        {
        //                            acilanlar.Append("PayLP;PayL2;");
        //                            lisanslar.PayL1 = true;
        //                            lisanslar.PayLP = true;
        //                            lisanslar.PayL2 = true;
        //                            lisanslar.PayL2Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayL2End = dosyaUser.expirydate;
        //                            lisanslar.PayLPEnd = dosyaUser.expirydate;
        //                            lisanslar.PayL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayL1End = dosyaUser.expirydate;
        //                        }

        //                        #endregion
        //                        #region PD2P
        //                        if (dosyaUser.PD2P)
        //                        {
        //                            acilanlar.Append("PayLP;PayL2;PD2P");
        //                            lisanslar.PayL1 = true;
        //                            lisanslar.PayLP = true;
        //                            lisanslar.PayL2 = true;
        //                            lisanslar.Pd2P = true;
        //                            lisanslar.Pd2PStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayL2Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.Pd2PEnd = dosyaUser.expirydate;
        //                            lisanslar.PayLPEnd = dosyaUser.expirydate;
        //                            lisanslar.PayL2End = dosyaUser.expirydate;
        //                            lisanslar.PayL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayL1End = dosyaUser.expirydate;
        //                        }

        //                        #endregion
        //                        #region END
        //                        if (dosyaUser.END)
        //                        {
        //                            acilanlar.Append("PayX;");
        //                            lisanslar.PayX = true;
        //                            lisanslar.PayXStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayXEnd = dosyaUser.expirydate;

        //                        }

        //                        #endregion
        //                        #region PIT
        //                        if (dosyaUser.PIT)
        //                        {
        //                            acilanlar.Append("PayGS;");
        //                            lisanslar.PayGS = true;
        //                            lisanslar.PayGSStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayGSEnd = dosyaUser.expirydate;

        //                        }

        //                        #endregion
        //                        #region PITE
        //                        if (dosyaUser.PITE)
        //                        {
        //                            acilanlar.Append("PayGs;PITE;");
        //                            lisanslar.PayGS = true;
        //                            lisanslar.PITE = true;
        //                            lisanslar.PayGSStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayPiteStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.PayGSEnd = dosyaUser.expirydate;
        //                            lisanslar.PayPiteEnd = dosyaUser.expirydate;

        //                        }

        //                        #endregion
        //                        #region MKK
        //                        if (dosyaUser.MKK)
        //                        {
        //                            acilanlar.Append("MKK;");
        //                            lisanslar.MKK = true;
        //                            lisanslar.MKKStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.MKKEnd = dosyaUser.expirydate;

        //                        }
        //                        #endregion
        //                        #region GKKUL
        //                        if (dosyaUser.GKKUL)
        //                        {
        //                            acilanlar.Append("GKKUL;");
        //                            lisanslar.GKKUL = true;
        //                            lisanslar.GKKULStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.GKKULEnd = dosyaUser.expirydate;

        //                        }
        //                        #endregion

        //                        #region VD1P
        //                        if (dosyaUser.VD1P)
        //                        {
        //                            acilanlar.Append("ViopLP;");
        //                            lisanslar.ViopL1 = true;
        //                            lisanslar.ViopLP = true;
        //                            lisanslar.ViopLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopLPEnd = dosyaUser.expirydate;
        //                            lisanslar.ViopL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopL1End = dosyaUser.expirydate;

        //                        }
        //                        #endregion
        //                        #region VD2
        //                        if (dosyaUser.VD2)
        //                        {
        //                            acilanlar.Append("ViopLP;ViopL2;");
        //                            lisanslar.ViopL1 = true;
        //                            lisanslar.ViopLP = true;
        //                            lisanslar.ViopL2 = true;
        //                            lisanslar.ViopL2Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopL2End = dosyaUser.expirydate;
        //                            lisanslar.ViopLPEnd = dosyaUser.expirydate;
        //                            lisanslar.ViopL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopL1End = dosyaUser.expirydate;

        //                        }

        //                        #endregion
        //                        #region VD2P
        //                        if (dosyaUser.VD2P)
        //                        {
        //                            acilanlar.Append("ViopLP;ViopL2;VD2P;");
        //                            lisanslar.ViopL1 = true;
        //                            lisanslar.ViopLP = true;
        //                            lisanslar.ViopL2 = true;
        //                            lisanslar.Vd2P = true;
        //                            lisanslar.Vd2PStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopL2Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopL1End = dosyaUser.expirydate;
        //                            lisanslar.Vd2PEnd = dosyaUser.expirydate;
        //                            lisanslar.ViopL2End = dosyaUser.expirydate;
        //                            lisanslar.ViopLPEnd = dosyaUser.expirydate;

        //                        }

        //                        #endregion
        //                        #region VIT
        //                        if (dosyaUser.VIT)
        //                        {
        //                            acilanlar.Append("ViopGS;");
        //                            lisanslar.ViopGS = true;
        //                            lisanslar.ViopGSStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.ViopGSEnd = dosyaUser.expirydate;

        //                        }

        //                        #endregion
        //                        #region BD1P
        //                        if (dosyaUser.BD1P)
        //                        {
        //                            acilanlar.Append("TahvilLP;");
        //                            lisanslar.TahvilL1 = true;
        //                            lisanslar.TahvilLP = true;
        //                            lisanslar.TahvilLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.TahvilLPEnd = dosyaUser.expirydate;
        //                            lisanslar.TahvilL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.TahvilL1End = dosyaUser.expirydate;

        //                        }
        //                        #endregion
        //                        #region BD2
        //                        if (dosyaUser.BD2)
        //                        {
        //                            acilanlar.Append("TahvilLP;TahvilL2;");
        //                            lisanslar.TahvilL1 = true;
        //                            lisanslar.TahvilLP = true;
        //                            lisanslar.TahvilL2 = true;
        //                            lisanslar.TahvilL2Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.TahvilLPStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.TahvilL2End = dosyaUser.expirydate;
        //                            lisanslar.TahvilLPEnd = dosyaUser.expirydate;
        //                            lisanslar.TahvilL1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.TahvilL1End = dosyaUser.expirydate;

        //                        }
        //                        #endregion
        //                        #region KRMD1
        //                        if (dosyaUser.KRMD1)
        //                        {
        //                            acilanlar.Append("KRMD1;");
        //                            lisanslar.COMEX = true;
        //                            lisanslar.KRMD1Start = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.KRMD1End = dosyaUser.expirydate;

        //                        }

        //                        #endregion
        //                        #region CME
        //                        if (dosyaUser.CME)
        //                        {
        //                            acilanlar.Append("CME;");
        //                            lisanslar.CME = true;
        //                            lisanslar.CMEStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                            lisanslar.CMEEnd = dosyaUser.expirydate;

        //                        }

        //                        #endregion
        //                        #endregion
        //                        ue.AcilanLisans = acilanlar.ToString();

        //                        #endregion
        //                        crm.KurumsalBilgilers.InsertOnSubmit(kurumbilgi);
        //                        crm.SubmitChanges();
        //                        newcustomer.KurumsalBilgilerId = kurumbilgi.Id;

        //                        crm.LisansDurums.InsertOnSubmit(lisanslar);
        //                        crm.SubmitChanges();

        //                        newcustomer.LisansDurumId = lisanslar.LisansDurumId;
        //                        ue.SonLisandurumID = lisanslar.LisansDurumId;

        //                        crm.Iletisims.InsertOnSubmit(iletisim);
        //                        crm.SubmitChanges();
        //                        newcustomer.iletisimId = iletisim.IletisimId;

        //                        crm.Users.InsertOnSubmit(newcustomer);
        //                        crm.SubmitChanges();

        //                        ue.UserId = newcustomer.UserID;

        //                        crm.UserEvents.InsertOnSubmit(ue);
        //                        crm.SubmitChanges();

        //                        formAdminAra.referance.IPport.DataToSend = "CreateUser|" + newcustomer.UserID.ToString() + (char)3;

        //                        // durumYaz(newcustomer.UserName + " nolu müşteri servera gönderildi.(Yeni Kayıt)");
        //                        // durumYaz(newcustomer.tckno + " TCKN nolu müşteri servera gönderildi.(Yeni Kayıt)");
        //                        durumYaz(newcustomer.UserName + "  nolu müşteri servera gönderildi.(Yeni Kayıt)");
        //                        LogDosyadanIslem(
        //                            operasyon: "YeniKullanici",
        //                            tckn: newcustomer.UserName,
        //                            calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
        //                                       formAdminAra.referance.ActiveCalisan.Soyad,
        //                            ip: formAdminAra.referance.IpAdress,
        //                            hostname: formAdminAra.referance.HostName,
        //                            degisiklikler: $"Açılan: {acilanlar} | ExpiryDate: {dosyaUser.expirydate:yyyyMMdd}"
        //                        );
        //                        #endregion
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MyTools.logyaz($"Dosyadan kullanıcı işlemleri YeniKullanıcı hatası: {ex.Message}");
        //                        durumYaz(dosyaUser.Username + " nolu müşteride hata oluştu, işlem atlandı");
        //                        LogDosyadanIslem(
        //                            operasyon: "Hata",
        //                            tckn: dosyaUser.Username,
        //                            calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
        //                                       formAdminAra.referance.ActiveCalisan.Soyad,
        //                            ip: formAdminAra.referance.IpAdress,
        //                            hostname: formAdminAra.referance.HostName,
        //                            degisiklikler: $"Hata Detayı: {ex.Message}"
        //                        );
        //                    }
        //                }
        //                LabelYaz(lblYeniGonderimDurum, say + " / " + count);
        //                say++;
        //            }

        //            LabelYaz(lblYeniGonderimDurum, "Bitti");
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        durumYaz(ex.Message);
        //    }
        //}

        // =====================================================================================
        // btnYeniSendServer_Click — DÜZELTİLMİŞ
        // Uygulanan maddeler:
        //   (2) Event artık erken commit edilmiyor; lisans başarıyla yazıldıktan SONRA tek sefer.
        //   (3) KopyalaLisans ile TÜM alanlar kopyalanıp sadece dosyadaki lisanslar override ediliyor.
        //        -> PayL1/ViopL1/TahvilL1 düşmesi ve SPI/ROBOT silinmesi kökten bitti. else blokları kalktı.
        //   (6) Tüm kullanıcı işlemi tek transaction; parça patlarsa hepsi rollback (hayalet event imkansız).
        //   (7) EventTypeId en sonda tek yerde belirleniyor (yayın açma vs lisans ekleme karışmıyor).
        //   (1) catch'ler durumYaz + LogDosyadanIslem(basarili:false) ile loglanıyor (kendi sürümünle birleştir).
        //
        // NOT: System.Data (ConnectionState) zaten using'lerde mevcut.
        // =====================================================================================

        #region KopyalaLisans helper (sınıfa ekle)
        /// <summary>
        /// Kaynak LisansDurum'un TÜM alanlarını hedefe kopyalar.
        /// Yeni snapshot oluşturmadan önce çağrılır; sonra sadece dosyadaki lisanslar override edilir.
        /// Bu sayede dosyada 0 (işaretsiz) olan lisanslar eksiksiz korunur.
        /// </summary>
        private void KopyalaLisans(LisansDurum k, LisansDurum h)
        {
            h.YayinDurumu = k.YayinDurumu; h.Futgck = k.Futgck; h.WINX = k.WINX;

            h.ProYetki = k.ProYetki; h.ProYetkiStart = k.ProYetkiStart; h.ProYetkiEnd = k.ProYetkiEnd;
            h.CepYetki = k.CepYetki; h.CepYetkiStart = k.CepYetkiStart; h.CepYetkiEnd = k.CepYetkiEnd;

            h.PayL1 = k.PayL1; h.PayL1Start = k.PayL1Start; h.PayL1End = k.PayL1End;
            h.PayLP = k.PayLP; h.PayLPStart = k.PayLPStart; h.PayLPEnd = k.PayLPEnd;
            h.PayL2 = k.PayL2; h.PayL2Start = k.PayL2Start; h.PayL2End = k.PayL2End;
            h.Pd2P = k.Pd2P; h.Pd2PStart = k.Pd2PStart; h.Pd2PEnd = k.Pd2PEnd;
            h.PayX = k.PayX; h.PayXStart = k.PayXStart; h.PayXEnd = k.PayXEnd;
            h.PayGS = k.PayGS; h.PayGSStart = k.PayGSStart; h.PayGSEnd = k.PayGSEnd;
            h.PITE = k.PITE; h.PayPiteStart = k.PayPiteStart; h.PayPiteEnd = k.PayPiteEnd;

            h.MKK = k.MKK; h.MKKStart = k.MKKStart; h.MKKEnd = k.MKKEnd;
            h.GKKUL = k.GKKUL; h.GKKULStart = k.GKKULStart; h.GKKULEnd = k.GKKULEnd;

            h.ViopL1 = k.ViopL1; h.ViopL1Start = k.ViopL1Start; h.ViopL1End = k.ViopL1End;
            h.ViopLP = k.ViopLP; h.ViopLPStart = k.ViopLPStart; h.ViopLPEnd = k.ViopLPEnd;
            h.ViopL2 = k.ViopL2; h.ViopL2Start = k.ViopL2Start; h.ViopL2End = k.ViopL2End;
            h.Vd2P = k.Vd2P; h.Vd2PStart = k.Vd2PStart; h.Vd2PEnd = k.Vd2PEnd;
            h.ViopGS = k.ViopGS; h.ViopGSStart = k.ViopGSStart; h.ViopGSEnd = k.ViopGSEnd;

            h.TahvilL1 = k.TahvilL1; h.TahvilL1Start = k.TahvilL1Start; h.TahvilL1End = k.TahvilL1End;
            h.TahvilLP = k.TahvilLP; h.TahvilLPStart = k.TahvilLPStart; h.TahvilLPEnd = k.TahvilLPEnd;
            h.TahvilL2 = k.TahvilL2; h.TahvilL2Start = k.TahvilL2Start; h.TahvilL2End = k.TahvilL2End;

            h.COMEX = k.COMEX; h.KRMD1Start = k.KRMD1Start; h.KRMD1End = k.KRMD1End;
            h.CME = k.CME; h.CMEStart = k.CMEStart; h.CMEEnd = k.CMEEnd;

            h.SPI = k.SPI;     // PAKET10 — artık silinmiyor
            h.ROBOT = k.ROBOT; // artık silinmiyor
        }
        #endregion


        private void btnYeniSendServer_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Okutulan Tüm Kullanıcılar Server'a gönderilecektir ", "Kullanıcı Açma İşlemi",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    return;
                }

                Task thred = Task.Factory.StartNew(() =>
                {
                    if (DosyadanYeniUserListe.Count < 1)
                    {
                        durumYaz("Okutulan Liste Boş"); return;
                    }
                    LabelYaz(lblYeniGonderimDurum, "0");
                    var say = 1;
                    var count = DosyadanYeniUserListe.Count;

                   // var crm = new crmDFNDataContext();
                    for (int i = 0; i < DosyadanYeniUserListe.Count; i++)
                    {
                        //if (i > 0 && i % batchSize == 0)
                        //{
                        //    crm.Dispose();
                        //    crm = new crmDFNDataContext();
                        //    var dlo = new DataLoadOptions();
                        //    dlo.LoadWith<User>(u => u.LisansDurum);
                        //    dlo.LoadWith<User>(u => u.Iletisim);
                        //    crm.LoadOptions = dlo;
                        //}
                        Thread.Sleep(300);
                        using (var crm = new crmDFNDataContext())
                        {
                            var dlo = new DataLoadOptions();
                            dlo.LoadWith<User>(u => u.LisansDurum);
                            dlo.LoadWith<User>(u => u.Iletisim);
                            crm.LoadOptions = dlo;

                            UserEvent ue = new UserEvent();
                            ue.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
                            ue.EventTarih = DateTime.Now;
                            ue.IP = formAdminAra.referance.IpAdress;
                            ue.HostName = formAdminAra.referance.HostName;

                            var dosyaUser = DosyadanYeniUserListe[i];

                            var userx = crm.Users.FirstOrDefault(x => x.UserName == dosyaUser.Username);

                            if (userx != null)
                            {
                                //UPDATE USER
                                try
                                {
                                    var kaptianlar = new StringBuilder();
                                    var acilanlar = new StringBuilder();

                                    ue.UserId = userx.UserID;
                                    userx.Name = dosyaUser.Ad;
                                    userx.Surname = dosyaUser.Soyad;
                                    userx.Aciklama = dosyaUser.Ad + " " + dosyaUser.Soyad;
                                    userx.ExpiryDate = dosyaUser.expirydate;

                                    LisansDurum lisanslar = new LisansDurum();
                                    KopyalaLisans(userx.LisansDurum, lisanslar);

                                    bool yayinAcildi = (userx.LisansDurum.YayinDurumu != true);
                                    lisanslar.YayinDurumu = true;
                                    lisanslar.Futgck = true;
                                    lisanslar.WINX = true;

                                    DateTime bugun = DateTime.Now.Date;

                                    #region Lisanslar (sadece dosyada 1 olanlar override edilir)
                                    if (dosyaUser.DESKTOP)
                                    {
                                        acilanlar.Append("ProYetki;");
                                        lisanslar.ProYetki = true;
                                        lisanslar.ProYetkiStart = bugun;
                                        lisanslar.ProYetkiEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.MOBIL)
                                    {
                                        acilanlar.Append("CepYetki;");
                                        lisanslar.CepYetki = true;
                                        lisanslar.CepYetkiStart = bugun;
                                        lisanslar.CepYetkiEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.PD1P)
                                    {
                                        acilanlar.Append("PayLP;");
                                        lisanslar.PayLP = true; lisanslar.PayL1 = true;
                                        lisanslar.PayLPStart = bugun; lisanslar.PayLPEnd = dosyaUser.expirydate;
                                        lisanslar.PayL1Start = bugun; lisanslar.PayL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.PD2)
                                    {
                                        acilanlar.Append("PayLP;PayL2;");
                                        lisanslar.PayL1 = true; lisanslar.PayLP = true; lisanslar.PayL2 = true;
                                        lisanslar.PayLPStart = bugun; lisanslar.PayL2Start = bugun; lisanslar.PayL1Start = bugun;
                                        lisanslar.PayLPEnd = dosyaUser.expirydate; lisanslar.PayL2End = dosyaUser.expirydate; lisanslar.PayL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.PD2P)
                                    {
                                        acilanlar.Append("PayLP;PayL2;PD2P;");
                                        lisanslar.PayL1 = true; lisanslar.PayLP = true; lisanslar.PayL2 = true; lisanslar.Pd2P = true;
                                        lisanslar.PayLPStart = bugun; lisanslar.PayL2Start = bugun; lisanslar.Pd2PStart = bugun; lisanslar.PayL1Start = bugun;
                                        lisanslar.PayLPEnd = dosyaUser.expirydate; lisanslar.PayL2End = dosyaUser.expirydate; lisanslar.Pd2PEnd = dosyaUser.expirydate; lisanslar.PayL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.END)
                                    {
                                        acilanlar.Append("PayX;");
                                        lisanslar.PayX = true; lisanslar.PayXStart = bugun; lisanslar.PayXEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.PIT)
                                    {
                                        acilanlar.Append("PayGS;");
                                        lisanslar.PayGS = true; lisanslar.PayGSStart = bugun; lisanslar.PayGSEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.PITE)
                                    {
                                        acilanlar.Append("PayGS;PITE;");
                                        lisanslar.PayGS = true; lisanslar.PITE = true;
                                        lisanslar.PayGSStart = bugun; lisanslar.PayPiteStart = bugun;
                                        lisanslar.PayGSEnd = dosyaUser.expirydate; lisanslar.PayPiteEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.MKK)
                                    {
                                        acilanlar.Append("MKK;");
                                        lisanslar.MKK = true; lisanslar.MKKStart = bugun; lisanslar.MKKEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.GKKUL)
                                    {
                                        acilanlar.Append("GKKUL;");
                                        lisanslar.GKKUL = true; lisanslar.GKKULStart = bugun; lisanslar.GKKULEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.VD1P)
                                    {
                                        acilanlar.Append("ViopLP;");
                                        lisanslar.ViopL1 = true; lisanslar.ViopLP = true;
                                        lisanslar.ViopLPStart = bugun; lisanslar.ViopLPEnd = dosyaUser.expirydate;
                                        lisanslar.ViopL1Start = bugun; lisanslar.ViopL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.VD2)
                                    {
                                        acilanlar.Append("ViopLP;ViopL2;");
                                        lisanslar.ViopL1 = true; lisanslar.ViopLP = true; lisanslar.ViopL2 = true;
                                        lisanslar.ViopL2Start = bugun; lisanslar.ViopL1Start = bugun;
                                        lisanslar.ViopL2End = dosyaUser.expirydate; lisanslar.ViopL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.VD2P)
                                    {
                                        acilanlar.Append("ViopLP;ViopL2;VD2P;");
                                        lisanslar.ViopL1 = true; lisanslar.ViopLP = true; lisanslar.ViopL2 = true; lisanslar.Vd2P = true;
                                        lisanslar.ViopLPStart = bugun; lisanslar.ViopL2Start = bugun; lisanslar.Vd2PStart = bugun; lisanslar.ViopL1Start = bugun;
                                        lisanslar.ViopLPEnd = dosyaUser.expirydate; lisanslar.ViopL2End = dosyaUser.expirydate; lisanslar.Vd2PEnd = dosyaUser.expirydate; lisanslar.ViopL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.VIT)
                                    {
                                        acilanlar.Append("ViopGS;");
                                        lisanslar.ViopGS = true; lisanslar.ViopGSStart = bugun; lisanslar.ViopGSEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.BD1P)
                                    {
                                        acilanlar.Append("TahvilLP;");
                                        lisanslar.TahvilL1 = true; lisanslar.TahvilLP = true;
                                        lisanslar.TahvilLPStart = bugun; lisanslar.TahvilL1Start = bugun;
                                        lisanslar.TahvilLPEnd = dosyaUser.expirydate; lisanslar.TahvilL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.BD2)
                                    {
                                        acilanlar.Append("TahvilLP;TahvilL2;");
                                        lisanslar.TahvilL1 = true; lisanslar.TahvilLP = true; lisanslar.TahvilL2 = true;
                                        lisanslar.TahvilLPStart = bugun; lisanslar.TahvilL2Start = bugun; lisanslar.TahvilL1Start = bugun;
                                        lisanslar.TahvilLPEnd = dosyaUser.expirydate; lisanslar.TahvilL2End = dosyaUser.expirydate; lisanslar.TahvilL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.KRMD1)
                                    {
                                        acilanlar.Append("KRMD1;");
                                        lisanslar.COMEX = true; lisanslar.KRMD1Start = bugun; lisanslar.KRMD1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.CME)
                                    {
                                        acilanlar.Append("CME;");
                                        lisanslar.CME = true; lisanslar.CMEStart = bugun; lisanslar.CMEEnd = dosyaUser.expirydate;
                                    }
                                    #endregion

                                    if (acilanlar.Length > 0 || kaptianlar.Length > 0)
                                    {
                                        if (acilanlar.Length > 0 && kaptianlar.Length > 0)
                                        {
                                            ue.EventTypeId = 6;
                                            ue.AcilanLisans = acilanlar.ToString();
                                            ue.KapatilanLisans = kaptianlar.ToString();
                                        }
                                        else if (acilanlar.Length > 0)
                                        {
                                            ue.EventTypeId = 4;
                                            ue.AcilanLisans = acilanlar.ToString();
                                        }
                                        else
                                        {
                                            ue.EventTypeId = 3;
                                            ue.KapatilanLisans = kaptianlar.ToString();
                                        }
                                    }
                                    else if (yayinAcildi)
                                    {
                                        ue.EventTypeId = 1; // sadece yayın açıldı, lisans değişmedi
                                    }
                                    else
                                    {
                                        ue.EventTypeId = 6; // sadece bilgi/tarih güncellemesi
                                    }

                                    if (crm.Connection.State != ConnectionState.Open) crm.Connection.Open();
                                    var tx = crm.Connection.BeginTransaction();
                                    crm.Transaction = tx;
                                    try
                                    {
                                        #region iletisim (transaction içinde)
                                        if (userx.iletisimId == null)
                                        {
                                            var iletisim = new Iletisim();
                                            if (!string.IsNullOrWhiteSpace(dosyaUser.Adres)) iletisim.acikadres = dosyaUser.Adres;
                                            if (!string.IsNullOrWhiteSpace(dosyaUser.Tel)) iletisim.Tel1 = dosyaUser.Tel;
                                            if (!string.IsNullOrWhiteSpace(dosyaUser.Sehir))
                                            {
                                                var il = MyTools.SehirIdBul(dosyaUser.Sehir);
                                                if (il != null && il.Id > 0) { iletisim.IlId = il.Id; iletisim.UlkeId = il.UlkeId; }
                                            }
                                            if (!string.IsNullOrWhiteSpace(dosyaUser.mail)) iletisim.email = dosyaUser.mail;

                                            crm.Iletisims.InsertOnSubmit(iletisim);
                                            crm.SubmitChanges();

                                            userx.Iletisim = iletisim;   // userx.iletisimId = ... SATIRINI SİLİN
                                        }
                                        else
                                        {
                                            if (dosyaUser.Adres != "") userx.Iletisim.acikadres = dosyaUser.Adres;
                                            if (dosyaUser.Tel != "") userx.Iletisim.Tel1 = dosyaUser.Tel;
                                            if (dosyaUser.Sehir != "")
                                            {
                                                var il = MyTools.SehirIdBul(dosyaUser.Sehir);
                                                if (il != null && il.Id > 0) { userx.Iletisim.IlId = il.Id; userx.Iletisim.UlkeId = il.UlkeId; }
                                            }
                                            if (dosyaUser.mail != "") userx.Iletisim.email = dosyaUser.mail;
                                        }
                                        #endregion

                                        crm.LisansDurums.InsertOnSubmit(lisanslar);
                                        crm.SubmitChanges();

                                        userx.LisansDurum = lisanslar;
                                        userx.LisansDurumId = lisanslar.LisansDurumId;
                                        crm.SubmitChanges();

                                        ue.SonLisandurumID = lisanslar.LisansDurumId;
                                        crm.UserEvents.InsertOnSubmit(ue);
                                        crm.SubmitChanges();

                                        tx.Commit();
                                    }
                                    catch
                                    {
                                        tx.Rollback();
                                        throw;
                                    }
                                    finally
                                    {
                                        crm.Transaction = null;
                                    }

                                    formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + userx.UserID.ToString() + (char)3;
                                    durumYaz(userx.UserName + " nolu müşteri servera gönderildi.(Değiştirme)");
                                    LogDosyadanIslem(
                                        operasyon: "KullanicıGuncelle",
                                        tckn: userx.UserName,
                                        calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
                                                   formAdminAra.referance.ActiveCalisan.Soyad,
                                        ip: formAdminAra.referance.IpAdress,
                                        hostname: formAdminAra.referance.HostName,
                                        degisiklikler: $"YayinAcildi:{yayinAcildi} | Açılan: {acilanlar} | Kapatılan: {kaptianlar} | ExpiryDate: {dosyaUser.expirydate:yyyyMMdd}"
                                    );
                                }
                                catch (Exception ex)
                                {

                                 durumYaz($"[HATA] {dosyaUser.Username} - {ex.Message}\n{ex.StackTrace}");
                                    MyTools.logyaz($"Dosyadan kullanıcı işlemleri Güncelleme hatası - {dosyaUser.Username}: {ex.Message}");
                                    LogDosyadanIslem(
                                        operasyon: "KullanicıGuncelle",
                                        tckn: dosyaUser.Username,
                                        calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
                                                   formAdminAra.referance.ActiveCalisan.Soyad,
                                        ip: formAdminAra.referance.IpAdress,
                                        hostname: formAdminAra.referance.HostName,
                                        degisiklikler: null,
                                        basarili: false,
                                        hata: ex.Message
                                    );
                                }
                            }
                            else
                            {
                                // CREATE USER
                                try
                                {
                                    var sifre = "Akyatirim1";
                                    var newcustomer = new User();
                                    var iletisim = new Iletisim();
                                    var kurumbilgi = new KurumsalBilgiler();
                                    var acilanlar = new StringBuilder();

                                    newcustomer.UserName = dosyaUser.Username;
                                    newcustomer.tckno = dosyaUser.Tckn;
                                    newcustomer.Aciklama = dosyaUser.Ad + " " + dosyaUser.Soyad;
                                    newcustomer.Name = dosyaUser.Ad;
                                    newcustomer.Surname = dosyaUser.Soyad;
                                    newcustomer.Password = MyTools.Sifreleme.Encryp(sifre);
                                    newcustomer.ExpiryDate = dosyaUser.expirydate;
                                    newcustomer.MusteriMenseiID = 1;
                                    newcustomer.ProductType = "IDEAL";
                                    newcustomer.BaslangicTarihi = DateTime.Now;
                                    newcustomer.StatusId = 1;

                                    LisansDurum lisanslar = new LisansDurum();
                                    lisanslar.CepYetki = dosyaUser.MOBIL;
                                    lisanslar.ProYetki = dosyaUser.DESKTOP;
                                    lisanslar.Futgck = true;
                                    lisanslar.WINX = true;
                                    lisanslar.YayinDurumu = true;
                                    ue.EventTypeId = 5;

                                    #region iletisim
                                    if (dosyaUser.Adres != "") iletisim.acikadres = dosyaUser.Adres;
                                    if (dosyaUser.Tel != "") iletisim.Tel1 = dosyaUser.Tel;
                                    if (dosyaUser.Sehir != "")
                                    {
                                        var ilx = MyTools.SehirIdBul(dosyaUser.Sehir);
                                        if (ilx != null && ilx.Id > 0) { iletisim.IlId = ilx.Id; iletisim.UlkeId = ilx.UlkeId; }
                                    }
                                    if (dosyaUser.mail != "") iletisim.email = dosyaUser.mail;
                                    #endregion

                                    DateTime bugun = DateTime.Now.Date;

                                    #region Lisanslar
                                    if (dosyaUser.DESKTOP)
                                    {
                                        acilanlar.Append("ProYetki;");
                                        lisanslar.ProYetki = true; lisanslar.ProYetkiStart = bugun; lisanslar.ProYetkiEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.MOBIL)
                                    {
                                        acilanlar.Append("CepYetki;");
                                        lisanslar.CepYetki = true; lisanslar.CepYetkiStart = bugun; lisanslar.CepYetkiEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.PD1P)
                                    {
                                        acilanlar.Append("PayLP;");
                                        lisanslar.PayLP = true; lisanslar.PayL1 = true;
                                        lisanslar.PayLPStart = bugun; lisanslar.PayLPEnd = dosyaUser.expirydate;
                                        lisanslar.PayL1Start = bugun; lisanslar.PayL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.PD2)
                                    {
                                        acilanlar.Append("PayLP;PayL2;");
                                        lisanslar.PayL1 = true; lisanslar.PayLP = true; lisanslar.PayL2 = true;
                                        lisanslar.PayL2Start = bugun; lisanslar.PayLPStart = bugun; lisanslar.PayL1Start = bugun;
                                        lisanslar.PayL2End = dosyaUser.expirydate; lisanslar.PayLPEnd = dosyaUser.expirydate; lisanslar.PayL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.PD2P)
                                    {
                                        acilanlar.Append("PayLP;PayL2;PD2P;");
                                        lisanslar.PayL1 = true; lisanslar.PayLP = true; lisanslar.PayL2 = true; lisanslar.Pd2P = true;
                                        lisanslar.Pd2PStart = bugun; lisanslar.PayLPStart = bugun; lisanslar.PayL2Start = bugun; lisanslar.PayL1Start = bugun;
                                        lisanslar.Pd2PEnd = dosyaUser.expirydate; lisanslar.PayLPEnd = dosyaUser.expirydate; lisanslar.PayL2End = dosyaUser.expirydate; lisanslar.PayL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.END)
                                    {
                                        acilanlar.Append("PayX;");
                                        lisanslar.PayX = true; lisanslar.PayXStart = bugun; lisanslar.PayXEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.PIT)
                                    {
                                        acilanlar.Append("PayGS;");
                                        lisanslar.PayGS = true; lisanslar.PayGSStart = bugun; lisanslar.PayGSEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.PITE)
                                    {
                                        acilanlar.Append("PayGs;PITE;");
                                        lisanslar.PayGS = true; lisanslar.PITE = true;
                                        lisanslar.PayGSStart = bugun; lisanslar.PayPiteStart = bugun;
                                        lisanslar.PayGSEnd = dosyaUser.expirydate; lisanslar.PayPiteEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.MKK)
                                    {
                                        acilanlar.Append("MKK;");
                                        lisanslar.MKK = true; lisanslar.MKKStart = bugun; lisanslar.MKKEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.GKKUL)
                                    {
                                        acilanlar.Append("GKKUL;");
                                        lisanslar.GKKUL = true; lisanslar.GKKULStart = bugun; lisanslar.GKKULEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.VD1P)
                                    {
                                        acilanlar.Append("ViopLP;");
                                        lisanslar.ViopL1 = true; lisanslar.ViopLP = true;
                                        lisanslar.ViopLPStart = bugun; lisanslar.ViopLPEnd = dosyaUser.expirydate;
                                        lisanslar.ViopL1Start = bugun; lisanslar.ViopL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.VD2)
                                    {
                                        acilanlar.Append("ViopLP;ViopL2;");
                                        lisanslar.ViopL1 = true; lisanslar.ViopLP = true; lisanslar.ViopL2 = true;
                                        lisanslar.ViopL2Start = bugun; lisanslar.ViopLPStart = bugun; lisanslar.ViopL1Start = bugun;
                                        lisanslar.ViopL2End = dosyaUser.expirydate; lisanslar.ViopLPEnd = dosyaUser.expirydate; lisanslar.ViopL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.VD2P)
                                    {
                                        acilanlar.Append("ViopLP;ViopL2;VD2P;");
                                        lisanslar.ViopL1 = true; lisanslar.ViopLP = true; lisanslar.ViopL2 = true; lisanslar.Vd2P = true;
                                        lisanslar.Vd2PStart = bugun; lisanslar.ViopL2Start = bugun; lisanslar.ViopLPStart = bugun; lisanslar.ViopL1Start = bugun;
                                        lisanslar.ViopL1End = dosyaUser.expirydate; lisanslar.Vd2PEnd = dosyaUser.expirydate; lisanslar.ViopL2End = dosyaUser.expirydate; lisanslar.ViopLPEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.VIT)
                                    {
                                        acilanlar.Append("ViopGS;");
                                        lisanslar.ViopGS = true; lisanslar.ViopGSStart = bugun; lisanslar.ViopGSEnd = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.BD1P)
                                    {
                                        acilanlar.Append("TahvilLP;");
                                        lisanslar.TahvilL1 = true; lisanslar.TahvilLP = true;
                                        lisanslar.TahvilLPStart = bugun; lisanslar.TahvilLPEnd = dosyaUser.expirydate;
                                        lisanslar.TahvilL1Start = bugun; lisanslar.TahvilL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.BD2)
                                    {
                                        acilanlar.Append("TahvilLP;TahvilL2;");
                                        lisanslar.TahvilL1 = true; lisanslar.TahvilLP = true; lisanslar.TahvilL2 = true;
                                        lisanslar.TahvilL2Start = bugun; lisanslar.TahvilLPStart = bugun; lisanslar.TahvilL1Start = bugun;
                                        lisanslar.TahvilL2End = dosyaUser.expirydate; lisanslar.TahvilLPEnd = dosyaUser.expirydate; lisanslar.TahvilL1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.KRMD1)
                                    {
                                        acilanlar.Append("KRMD1;");
                                        lisanslar.COMEX = true; lisanslar.KRMD1Start = bugun; lisanslar.KRMD1End = dosyaUser.expirydate;
                                    }
                                    if (dosyaUser.CME)
                                    {
                                        acilanlar.Append("CME;");
                                        lisanslar.CME = true; lisanslar.CMEStart = bugun; lisanslar.CMEEnd = dosyaUser.expirydate;
                                    }
                                    #endregion

                                    ue.AcilanLisans = acilanlar.ToString();

                                    if (crm.Connection.State != ConnectionState.Open) crm.Connection.Open();
                                    var tx = crm.Connection.BeginTransaction();
                                    crm.Transaction = tx;
                                    try
                                    {
                                        crm.KurumsalBilgilers.InsertOnSubmit(kurumbilgi);
                                        crm.SubmitChanges();
                                        newcustomer.KurumsalBilgilerId = kurumbilgi.Id;

                                        crm.LisansDurums.InsertOnSubmit(lisanslar);
                                        crm.SubmitChanges();
                                        newcustomer.LisansDurumId = lisanslar.LisansDurumId;
                                        ue.SonLisandurumID = lisanslar.LisansDurumId;

                                        crm.Iletisims.InsertOnSubmit(iletisim);
                                        crm.SubmitChanges();
                                        newcustomer.iletisimId = iletisim.IletisimId;

                                        crm.Users.InsertOnSubmit(newcustomer);
                                        crm.SubmitChanges();
                                        ue.UserId = newcustomer.UserID;

                                        crm.UserEvents.InsertOnSubmit(ue);
                                        crm.SubmitChanges();

                                        tx.Commit();
                                    }
                                    catch
                                    {
                                        tx.Rollback();
                                        throw;
                                    }
                                    finally
                                    {
                                        crm.Transaction = null;
                                    }

                                    formAdminAra.referance.IPport.DataToSend = "CreateUser|" + newcustomer.UserID.ToString() + (char)3;
                                    durumYaz(newcustomer.UserName + "  nolu müşteri servera gönderildi.(Yeni Kayıt)");
                                    LogDosyadanIslem(
                                        operasyon: "YeniKullanici",
                                        tckn: newcustomer.UserName,
                                        calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
                                                   formAdminAra.referance.ActiveCalisan.Soyad,
                                        ip: formAdminAra.referance.IpAdress,
                                        hostname: formAdminAra.referance.HostName,
                                        degisiklikler: $"Açılan: {acilanlar} | ExpiryDate: {dosyaUser.expirydate:yyyyMMdd}"
                                    );
                                }
                                catch (Exception ex)
                                {
                                    durumYaz($"[HATA] {dosyaUser.Username} - {ex.Message}");
                                    MyTools.logyaz($"Dosyadan kullanıcı işlemleri YeniKullanıcı hatası - {dosyaUser.Username}: {ex.Message}");
                                    LogDosyadanIslem(
                                        operasyon: "YeniKullanici",
                                        tckn: dosyaUser.Username,
                                        calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
                                                   formAdminAra.referance.ActiveCalisan.Soyad,
                                        ip: formAdminAra.referance.IpAdress,
                                        hostname: formAdminAra.referance.HostName,
                                        degisiklikler: null,
                                        basarili: false,
                                        hata: ex.Message
                                    );
                                }
                            } 
                        }

                        LabelYaz(lblYeniGonderimDurum, say + " / " + count);
                        say++;
                    }

                    LabelYaz(lblYeniGonderimDurum, "Bitti");
                });
            }
            catch (Exception ex)
            {
                durumYaz(ex.Message);
            }
        }

        private void texteAktarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var filename = Application.StartupPath + "\\DosyadanUserIslemDurum.txt";



                var sb = new StringBuilder();

                if (lboxDurum.Items.Count < 1) return;

                for (int i = 0; i < lboxDurum.Items.Count; i++)
                {
                    sb.AppendLine(lboxDurum.Items[i].ToString());
                }

                File.WriteAllText(filename, sb.ToString(), Encoding.GetEncoding("iso-8859-9"));

                Process.Start(filename);

            }
            catch (Exception ex)
            {
                durumYaz(ex.Message);
            }
        }

        private void btnIptalSendServer_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Okutulan Tüm Kullanıcılar Server'a gönderilecektir ", "Kullanıcı Son Tarih Değiştirme İşlemi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    return;
                }

                Task thred = Task.Factory.StartNew(() =>
                {

                    if (DosyadanIptalUserListe.Count < 1)
                    {
                        durumYaz("Okutulan Liste Boş"); return;

                    }
                    LabelYaz(lblIptalGonderimDurum, "0");
                    var say = 1;
                    var count = DosyadanIptalUserListe.Count;


                    //var crm = new crmDFNDataContext();
                    var crm = YeniContext();
                    for (int i = 0; i < DosyadanIptalUserListe.Count; i++)
                    {
                        if (i > 0 && i % batchSize == 0)
                        {
                            crm.Dispose();
                            crm = YeniContext();
                        }
                        Thread.Sleep(100);

                        var dosyaUser = DosyadanIptalUserListe[i];

                        var userx = crm.Users.FirstOrDefault(x => x.UserName == dosyaUser.Username);
                        if (userx != null)
                        {
                            #region Guncelle
                            var kaptianlar = new StringBuilder();

                            userx.ExpiryDate = dosyaUser.expirydate;
                            crm.SubmitChanges();

                            UserEvent ue = new UserEvent();
                            ue.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
                            ue.EventTarih = DateTime.Now;
                            ue.IP = formAdminAra.referance.IpAdress;
                            ue.HostName = formAdminAra.referance.HostName;
                            ue.UserId = userx.UserID;
                            ue.SonLisandurumID = userx.LisansDurumId;
                            ue.EventTypeId = 4; // Lisans güncelleme
                            ue.AcilanLisans = $"ExpiryDate:{dosyaUser.expirydate:yyyyMMdd}";
                            crm.UserEvents.InsertOnSubmit(ue);
                            crm.SubmitChanges();

                            var calisanAd = formAdminAra.referance.ActiveCalisan.Ad + " " +
                             formAdminAra.referance.ActiveCalisan.Soyad;

                            LogDosyadanIslem(
                                operasyon: "TarihDegistirme",
                                tckn: userx.UserName,
                                calisanAd: calisanAd,
                                ip: formAdminAra.referance.IpAdress,
                                hostname: formAdminAra.referance.HostName,
                                degisiklikler: $"ExpiryDate: {dosyaUser.expirydate:yyyyMMdd}"
                            );
                            formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + userx.UserID.ToString() + (char)3;
                            durumYaz(userx.UserName + " nolu müşteri servera gönderildi.(Değiştirme)");
                            // durumYaz(userx.tckno + " TCKN nolu müşteri servera gönderildi.(Değiştirme)");
                            #endregion
                        }
                        else
                        {
                            durumYaz(dosyaUser.Username + " nolu müşteri bulunamadı");
                        }

                        LabelYaz(lblIptalGonderimDurum, say + " / " + count);
                        say++;
                    }


                    LabelYaz(lblIptalOkunanKlullaniciSayisi, "Bitti");

                });


            }
            catch (Exception ex)
            {
                durumYaz(ex.Message);
            }
        }

        /// <summary>
        /// Tırnak-duyarlı CSV satır ayrıştırıcı.
        /// Çift tırnak ("...") içindeki ';' ayraç sayılmaz. "" -> tek tırnak (Excel kaçışı).
        /// </summary>
        private static string[] SplitCsv(string line, char delimiter = ';')
        {
            var alanlar = new List<string>();
            if (line == null) return alanlar.ToArray();

            var sb = new StringBuilder();
            bool tirnakIcinde = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (tirnakIcinde)
                {
                    if (c == '"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"') { sb.Append('"'); i++; } // "" -> "
                        else tirnakIcinde = false;
                    }
                    else sb.Append(c);
                }
                else
                {
                    if (c == '"') tirnakIcinde = true;
                    else if (c == delimiter) { alanlar.Add(sb.ToString()); sb.Clear(); }
                    else sb.Append(c);
                }
            }
            alanlar.Add(sb.ToString());
            return alanlar.ToArray();
        }
        private void btnKRMD1AcKapaOku_Click(object sender, EventArgs e)
        {
            var satirno = "";
            int okunan = 0, atlanan = 0;
            try
            {
                DosyadanKRMD1AcKapaListe.Clear();

                OpenFileDialog op = new OpenFileDialog();
                var filename = "";
                op.InitialDirectory = Application.StartupPath;
                op.Filter = "(*.csv)|*.csv";
                if (op.ShowDialog() == DialogResult.OK) filename = op.FileName; else return;

                char[] delimiterChars = { ';' };
                var filestr = File.ReadAllLines(filename, Encoding.GetEncoding("iso-8859-9"));
                if (filestr.Length < 2) { MessageBox.Show("csv dosyası boş"); return; }

                for (int i = 1; i < filestr.Length; i++)
                {
                    satirno = i.ToString();
                    if (string.IsNullOrWhiteSpace(filestr[i])) continue;

                    try
                    {
                        var satirstr = SplitCsv(filestr[i]);
                        if (satirstr.Length < 3)
                        {
                            durumYaz($"{satirno}. satır ATLANDI: temel alanlar eksik (kolon {satirstr.Length}, en az 3 gerekli).");
                            atlanan++; continue;
                        }

                        string mno = satirstr[0].Trim();
                        bool KRDM1 = satirstr[1].Trim()._ToBool();
                        string expriydate = satirstr[2].Trim();

                        if (mno == "")
                        {
                            durumYaz($"{satirno}. satır ATLANDI: TCKN boş olamaz.");
                            atlanan++; continue;
                        }

                        DateTime parsed;
                        if (!DateTime.TryParseExact(expriydate, "yyyyMMdd",
                                CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                        {
                            durumYaz($"{satirno}. satır ATLANDI: tarih formatı hatalı ('{expriydate}'). Format yyyyMMdd olmalı.");
                            atlanan++; continue;
                        }

                        var dosuser = new DosyadanYeniUser();
                        dosuser.Username = mno;
                        dosuser.KRMD1 = KRDM1;
                        dosuser.expirydate = parsed;

                        DosyadanKRMD1AcKapaListe.Add(dosuser);
                        okunan++;
                        Krmd1OkunanKullaniciSayisi = DosyadanKRMD1AcKapaListe.Count.ToString();
                    }
                    catch (Exception exRow)
                    {
                        durumYaz($"{satirno}. satır ATLANDI (hata): {exRow.Message}");
                        atlanan++;
                    }
                }

                durumYaz($"Okuma bitti. Eklenen: {okunan}, Atlanan: {atlanan}, Toplam veri satırı: {filestr.Length - 1}.");
            }
            catch (Exception ex)
            {
                durumYaz("Dosya okuma hatası: " + ex.Message);
            }
        }

        private void btnKRMD1AcKapaGonder_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Okutulan Tüm Kullanıcılar Server'a gönderilecektir ", "Kullanıcı Açma İşlemi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    return;
                }

                Task thred = Task.Factory.StartNew(() =>
                {

                    if (DosyadanKRMD1AcKapaListe.Count < 1)
                    {
                        durumYaz("Okutulan Liste Boş"); return;

                    }
                    LabelYaz(lblKRMD1AcKapaGonderimDurum, "0");
                    var say = 1;
                    var count = DosyadanKRMD1AcKapaListe.Count;


                    var crm = YeniContext();
                    for (int i = 0; i < DosyadanKRMD1AcKapaListe.Count; i++)
                    {
                        if (i > 0 && i % batchSize == 0)
                        {
                            crm.Dispose();
                            crm = YeniContext();
                        }
                        Thread.Sleep(100);

                        UserEvent ue = new UserEvent();
                        ue.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
                        ue.EventTarih = DateTime.Now;
                        ue.IP = formAdminAra.referance.IpAdress;
                        ue.HostName = formAdminAra.referance.HostName;

                        var dosyaUser = DosyadanKRMD1AcKapaListe[i];


                        //if (crm.Users.Where(x => x.UserName == dosyaUser.HesapNo).Any())
                        // if (crm.Users.Where(x => x.tckno == dosyaUser.Tckn).Any())
                        var userx = crm.Users.FirstOrDefault(x => x.UserName == dosyaUser.Username);
                        if (userx != null)
                        {
                            #region Guncelle
                            var kaptianlar = new StringBuilder();
                            var acilanlar = new StringBuilder();
                            userx.ExpiryDate = dosyaUser.expirydate;
                            ue.UserId = userx.UserID;

                            LisansDurum lisanslar = new LisansDurum();

                            var yayind = true;
                            lisanslar.CepYetki = dosyaUser.MOBIL;
                            lisanslar.ProYetki = dosyaUser.DESKTOP;
                            lisanslar.Futgck = true;
                            lisanslar.WINX = true;


                            // int lastLisansId = (int)activeuser.LisansDurumId;

                            if (userx.LisansDurum.YayinDurumu != yayind)
                            {

                                if (yayind == true)
                                    ue.EventTypeId = 1;
                                else
                                    ue.EventTypeId = 2;

                                lisanslar.YayinDurumu = yayind;

                                crm.UserEvents.InsertOnSubmit(ue);
                                crm.SubmitChanges();
                            }
                            else
                                lisanslar.YayinDurumu = yayind;



                            #region iletisim


                            if (userx.iletisimId == null)
                            {
                                var iletisim = new Iletisim();
                                if (dosyaUser.Adres != "")
                                    iletisim.acikadres = dosyaUser.Adres;
                                if (dosyaUser.Tel != "")
                                    iletisim.Tel1 = dosyaUser.Tel;
                                if (dosyaUser.Sehir != "")
                                {
                                    var il = MyTools.SehirIdBul(dosyaUser.Sehir);
                                    if (il != null && il.Id > 0)
                                    {
                                        iletisim.IlId = il.Id;
                                        iletisim.UlkeId = il.UlkeId;
                                    }
                                }
                                if (dosyaUser.mail != "")
                                    iletisim.email = dosyaUser.mail;

                                crm.Iletisims.InsertOnSubmit(iletisim);
                                crm.SubmitChanges();

                                userx.iletisimId = iletisim.IletisimId;
                            }
                            else
                            {
                                if (dosyaUser.Adres != "")
                                    userx.Iletisim.acikadres = dosyaUser.Adres;
                                if (dosyaUser.Tel != "")
                                    userx.Iletisim.Tel1 = dosyaUser.Tel;
                                if (dosyaUser.Sehir != "")
                                {
                                    var il = MyTools.SehirIdBul(dosyaUser.Sehir);
                                    if (il != null && il.Id > 0)
                                    {
                                        userx.Iletisim.IlId = il.Id;
                                        userx.Iletisim.UlkeId = il.UlkeId;
                                    }
                                }
                                if (dosyaUser.mail != "")
                                    userx.Iletisim.email = dosyaUser.mail;
                            }


                            #endregion
                            #region Lisanslar


                            #region Desktop
                            if (userx.LisansDurum.ProYetki != true && dosyaUser.DESKTOP)
                            {
                                acilanlar.Append("ProYetki;");
                                lisanslar.ProYetki = true;
                                lisanslar.ProYetkiStart = DateTime.Now.Date;
                                lisanslar.ProYetkiEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.ProYetki = userx.LisansDurum.ProYetki;
                                lisanslar.ProYetkiStart = userx.LisansDurum.ProYetkiStart;
                                lisanslar.ProYetkiEnd = userx.LisansDurum.ProYetkiEnd;
                            }


                            #endregion
                            #region Mobil
                            if (userx.LisansDurum.CepYetki != true && dosyaUser.MOBIL)
                            {
                                acilanlar.Append("CepYetki;");
                                lisanslar.CepYetki = true;
                                lisanslar.CepYetkiStart = DateTime.Now.Date;
                                lisanslar.CepYetkiEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.CepYetki = userx.LisansDurum.CepYetki;
                                lisanslar.CepYetkiStart = userx.LisansDurum.CepYetkiStart;
                                lisanslar.CepYetkiEnd = userx.LisansDurum.CepYetkiEnd;
                            }

                            #endregion

                            #region PD1
                            if (userx.LisansDurum.PayL1 != true && dosyaUser.PD1)
                            {
                                acilanlar.Append("PayL1;");
                                lisanslar.PayL1 = true;
                                lisanslar.PayL1Start = DateTime.Now.Date;
                                lisanslar.PayL1End = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.PayL1 = userx.LisansDurum.PayL1;
                                lisanslar.PayL1Start = userx.LisansDurum.PayL1Start;
                                lisanslar.PayL1End = userx.LisansDurum.PayL1End;
                            }

                            #endregion
                            #region PD1P
                            if (userx.LisansDurum.PayLP != true && dosyaUser.PD1P)
                            {
                                acilanlar.Append("PayLP;");
                                lisanslar.PayLP = true;
                                lisanslar.PayLPStart = DateTime.Now.Date;
                                lisanslar.PayLPEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.PayLP = userx.LisansDurum.PayLP;
                                lisanslar.PayLPStart = userx.LisansDurum.PayLPStart;
                                lisanslar.PayLPEnd = userx.LisansDurum.PayLPEnd;
                            }

                            #endregion
                            #region PD2
                            if (userx.LisansDurum.PayL2 != true && dosyaUser.PD2)
                            {
                                acilanlar.Append("PayL2;");
                                lisanslar.PayL2 = true;
                                lisanslar.PayL2Start = DateTime.Now.Date;
                                lisanslar.PayL2End = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.PayL2 = userx.LisansDurum.PayL2;
                                lisanslar.PayL2Start = userx.LisansDurum.PayL2Start;
                                lisanslar.PayL2End = userx.LisansDurum.PayL2End;
                            }

                            #endregion
                            #region PD2P
                            if (userx.LisansDurum.Pd2P != true && dosyaUser.PD2P)
                            {
                                acilanlar.Append("PD2P;");
                                lisanslar.Pd2P = true;
                                lisanslar.Pd2PStart = DateTime.Now.Date;
                                lisanslar.Pd2PEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.Pd2P = userx.LisansDurum.Pd2P;
                                lisanslar.Pd2PStart = userx.LisansDurum.Pd2PStart;
                                lisanslar.Pd2PEnd = userx.LisansDurum.Pd2PEnd;
                            }

                            #endregion
                            #region END
                            if (userx.LisansDurum.PayX != true && dosyaUser.END)
                            {
                                acilanlar.Append("PayX;");
                                lisanslar.PayX = true;
                                lisanslar.PayXStart = DateTime.Now.Date;
                                lisanslar.PayXEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.PayX = userx.LisansDurum.PayX;
                                lisanslar.PayXStart = userx.LisansDurum.PayXStart;
                                lisanslar.PayXEnd = userx.LisansDurum.PayXEnd;
                            }

                            #endregion
                            #region PIT
                            if (userx.LisansDurum.PayGS != true && dosyaUser.PIT)
                            {
                                acilanlar.Append("PayGS;");
                                lisanslar.PayGS = true;
                                lisanslar.PayGSStart = DateTime.Now.Date;
                                lisanslar.PayGSEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.PayGS = userx.LisansDurum.PayGS;
                                lisanslar.PayGSStart = userx.LisansDurum.PayGSStart;
                                lisanslar.PayGSEnd = userx.LisansDurum.PayGSEnd;
                            }

                            #endregion
                            #region PITE
                            if (userx.LisansDurum.PITE != true && dosyaUser.PITE)
                            {
                                acilanlar.Append("PITE;");
                                lisanslar.PITE = true;
                                lisanslar.PayPiteStart = DateTime.Now.Date;
                                lisanslar.PayPiteEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.PITE = userx.LisansDurum.PITE;
                                lisanslar.PayPiteStart = userx.LisansDurum.PayPiteStart;
                                lisanslar.PayPiteEnd = userx.LisansDurum.PayPiteEnd;
                            }

                            #endregion
                            #region MKK
                            if (userx.LisansDurum.MKK != true && dosyaUser.MKK)
                            {
                                acilanlar.Append("MKK;");
                                lisanslar.MKK = true;
                                lisanslar.MKKStart = DateTime.Now.Date;
                                lisanslar.MKKEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.MKK = userx.LisansDurum.MKK;
                                lisanslar.MKKStart = userx.LisansDurum.MKKStart;
                                lisanslar.MKKEnd = userx.LisansDurum.MKKEnd;
                            }

                            #endregion

                            #region GKKUL
                            if (userx.LisansDurum.GKKUL != true && dosyaUser.GKKUL)
                            {
                                acilanlar.Append("GKKUL;");
                                lisanslar.GKKUL = true;
                                lisanslar.GKKULStart = DateTime.Now.Date;
                                lisanslar.GKKULEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.GKKUL = userx.LisansDurum.GKKUL;
                                lisanslar.GKKULStart = userx.LisansDurum.GKKULStart;
                                lisanslar.GKKULEnd = userx.LisansDurum.GKKULEnd;
                            }

                            #endregion

                            #region VD1
                            if (userx.LisansDurum.ViopL1 != true && dosyaUser.VD1)
                            {
                                acilanlar.Append("ViopL1;");
                                lisanslar.ViopL1 = true;
                                lisanslar.ViopL1Start = DateTime.Now.Date;
                                lisanslar.ViopL1End = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.ViopL1 = userx.LisansDurum.ViopL1;
                                lisanslar.ViopL1Start = userx.LisansDurum.ViopL1Start;
                                lisanslar.ViopL1End = userx.LisansDurum.ViopL1End;
                            }

                            #endregion
                            #region VD1P
                            if (userx.LisansDurum.ViopLP != true && dosyaUser.VD1P)
                            {
                                acilanlar.Append("ViopLP;");
                                lisanslar.ViopLP = true;
                                lisanslar.ViopLPStart = DateTime.Now.Date;
                                lisanslar.ViopLPEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.ViopLP = userx.LisansDurum.ViopLP;
                                lisanslar.ViopLPStart = userx.LisansDurum.ViopLPStart;
                                lisanslar.ViopLPEnd = userx.LisansDurum.ViopLPEnd;
                            }

                            #endregion
                            #region VD2
                            if (userx.LisansDurum.ViopL2 != true && dosyaUser.VD2)
                            {
                                acilanlar.Append("ViopL2;");
                                lisanslar.ViopL2 = true;
                                lisanslar.ViopL2Start = DateTime.Now.Date;
                                lisanslar.ViopL2End = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.ViopL2 = userx.LisansDurum.ViopL2;
                                lisanslar.ViopL2Start = userx.LisansDurum.ViopL2Start;
                                lisanslar.ViopL2End = userx.LisansDurum.ViopL2End;
                            }

                            #endregion
                            #region VD2P
                            if (userx.LisansDurum.Vd2P != true && dosyaUser.VD2P)
                            {
                                acilanlar.Append("VD2P;");
                                lisanslar.Vd2P = true;
                                lisanslar.Vd2PStart = DateTime.Now.Date;
                                lisanslar.Vd2PEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.Vd2P = userx.LisansDurum.Vd2P;
                                lisanslar.Vd2PStart = userx.LisansDurum.Vd2PStart;
                                lisanslar.Vd2PEnd = userx.LisansDurum.Vd2PEnd;
                            }

                            #endregion
                            #region VIT
                            if (userx.LisansDurum.ViopGS != true && dosyaUser.VIT)
                            {
                                acilanlar.Append("ViopGS;");
                                lisanslar.ViopGS = true;
                                lisanslar.ViopGSStart = DateTime.Now.Date;
                                lisanslar.ViopGSEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.ViopGS = userx.LisansDurum.ViopGS;
                                lisanslar.ViopGSStart = userx.LisansDurum.ViopGSStart;
                                lisanslar.ViopGSEnd = userx.LisansDurum.ViopGSEnd;
                            }

                            #endregion
                            #region BD1
                            if (userx.LisansDurum.TahvilL1 != true && dosyaUser.BD1)
                            {
                                acilanlar.Append("TahvilL1;");
                                lisanslar.TahvilL1 = true;
                                lisanslar.TahvilL1Start = DateTime.Now.Date;
                                lisanslar.TahvilL1End = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.TahvilL1 = userx.LisansDurum.TahvilL1;
                                lisanslar.TahvilL1Start = userx.LisansDurum.TahvilL1Start;
                                lisanslar.TahvilL1End = userx.LisansDurum.TahvilL1End;
                            }

                            #endregion
                            #region BD1P
                            if (userx.LisansDurum.TahvilLP != true && dosyaUser.BD1P)
                            {
                                acilanlar.Append("TahvilLP;");
                                lisanslar.TahvilLP = true;
                                lisanslar.TahvilLPStart = DateTime.Now.Date;
                                lisanslar.TahvilLPEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.TahvilLP = userx.LisansDurum.TahvilLP;
                                lisanslar.TahvilLPStart = userx.LisansDurum.TahvilLPStart;
                                lisanslar.TahvilLPEnd = userx.LisansDurum.TahvilLPEnd;
                            }

                            #endregion
                            #region BD2
                            if (userx.LisansDurum.TahvilL2 != true && dosyaUser.BD2)
                            {
                                acilanlar.Append("TahvilL2;");
                                lisanslar.TahvilL2 = true;
                                lisanslar.TahvilL2Start = DateTime.Now.Date;
                                lisanslar.TahvilL2End = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.TahvilL2 = userx.LisansDurum.TahvilL2;
                                lisanslar.TahvilL2Start = userx.LisansDurum.TahvilL2Start;
                                lisanslar.TahvilL2End = userx.LisansDurum.TahvilL2End;
                            }

                            #endregion
                            #region KRMD1
                            if (dosyaUser.KRMD1)
                            {
                                if (userx.LisansDurum.COMEX == true)
                                {
                                    lisanslar.COMEX = true;
                                    lisanslar.KRMD1Start = userx.LisansDurum.KRMD1Start;
                                    lisanslar.KRMD1End = userx.LisansDurum.KRMD1End;
                                }
                                else
                                {
                                    acilanlar.Append("KRMD1;");
                                    lisanslar.COMEX = true;
                                    lisanslar.KRMD1Start = DateTime.Now.Date;
                                    lisanslar.KRMD1End = dosyaUser.expirydate;

                                    if (userx.LisansDurum.SPI == true)
                                    {
                                        lisanslar.SPI = false;
                                        kaptianlar.Append("PAKET10;");
                                    }

                                    if (lisanslar.PayL1 == true && lisanslar.PayLP == false && lisanslar.PayL2 == false && lisanslar.ViopL1 == true && lisanslar.ViopLP == false && lisanslar.ViopL2 == false)
                                    {
                                        lisanslar.PayL1 = false;
                                        kaptianlar.Append("PayL1;");
                                        lisanslar.ViopL1 = false;
                                        kaptianlar.Append("ViopL1;");
                                    }
                                    else if (lisanslar.PayL1 == true && lisanslar.PayLP == false && lisanslar.PayL2 == false && lisanslar.ViopL1 == false && lisanslar.ViopLP == false && lisanslar.ViopL2 == false)
                                    {
                                        lisanslar.PayL1 = false;
                                        kaptianlar.Append("PayL1;");
                                    }
                                    else if (lisanslar.PayL1 == false && lisanslar.PayLP == false && lisanslar.PayL2 == false && lisanslar.ViopL1 == true && lisanslar.ViopLP == false && lisanslar.ViopL2 == false)
                                    {
                                        lisanslar.ViopL1 = false;
                                        kaptianlar.Append("ViopL1;");
                                    }
                                }
                            }
                            else
                            {
                                if (userx.LisansDurum.COMEX == true)
                                {
                                    lisanslar.COMEX = false;
                                    lisanslar.KRMD1Start = userx.LisansDurum.KRMD1Start;
                                    lisanslar.KRMD1End = DateTime.Now.Date;
                                    kaptianlar.Append("KRMD1;");
                                }
                                else
                                {
                                    lisanslar.COMEX = userx.LisansDurum.COMEX;
                                    lisanslar.KRMD1Start = userx.LisansDurum.KRMD1Start;
                                    lisanslar.KRMD1End = userx.LisansDurum.KRMD1End;
                                }
                            }
                            #endregion
                            #region PAKET10
                            if (userx.LisansDurum.SPI != true && dosyaUser.PAKET10)
                            {

                                acilanlar.Append("PAKET10;");
                                lisanslar.SPI = true;

                                if (lisanslar.PayL1 == true)
                                {
                                    lisanslar.PayL1 = false;
                                    kaptianlar.Append("PayL1;");
                                }
                                if (lisanslar.PayLP == true)
                                {
                                    lisanslar.PayLP = false;
                                    kaptianlar.Append("PayLP;");
                                }
                                if (lisanslar.PayL2 == true)
                                {
                                    lisanslar.PayL2 = false;
                                    kaptianlar.Append("PayL2;");
                                }

                                if (lisanslar.COMEX == true)
                                {
                                    lisanslar.COMEX = false;
                                    kaptianlar.Append("KRMD1;");
                                }
                            }
                            else
                                lisanslar.SPI = userx.LisansDurum.SPI;
                            #endregion
                            #region CME
                            if (userx.LisansDurum.CME != true && dosyaUser.CME)
                            {
                                acilanlar.Append("ALG;");
                                lisanslar.CME = true;
                                lisanslar.CMEStart = DateTime.Now.Date;
                                lisanslar.CMEEnd = dosyaUser.expirydate;
                            }
                            else
                            {
                                lisanslar.CME = userx.LisansDurum.CME;
                                lisanslar.CMEStart = userx.LisansDurum.CMEStart;
                                lisanslar.CMEEnd = userx.LisansDurum.CMEEnd;
                            }
                            #endregion
                            #endregion

                            if (acilanlar.ToString() != "" || kaptianlar.ToString() != "")
                            {
                                if (acilanlar.ToString() != "" && kaptianlar.ToString() != "")
                                {
                                    ue.EventTypeId = 6;
                                    ue.KapatilanLisans = kaptianlar.ToString();
                                    ue.AcilanLisans = acilanlar.ToString();

                                }
                                else
                                {
                                    if (acilanlar.ToString() != "")
                                    {
                                        ue.EventTypeId = 4;
                                        ue.AcilanLisans = acilanlar.ToString();
                                    }

                                    if (kaptianlar.ToString() != "")
                                    {
                                        ue.EventTypeId = 3;
                                        ue.KapatilanLisans = kaptianlar.ToString();
                                    }
                                }

                            }

                            crm.LisansDurums.InsertOnSubmit(lisanslar);
                            crm.SubmitChanges();
                            userx.LisansDurum = lisanslar;
                            crm.SubmitChanges();
                            ue.SonLisandurumID = lisanslar.LisansDurumId;
                            userx.LisansDurumId = lisanslar.LisansDurumId;
                            if (ue.EventId < 1)
                                crm.UserEvents.InsertOnSubmit(ue);
                            crm.SubmitChanges();
                            formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + userx.UserID.ToString() + (char)3;
                            // durumYaz(userx.UserName + " nolu müşteri servera gönderildi.(Değiştirme)");
                            if (acilanlar.ToString() != "" || kaptianlar.ToString() != "")
                            {
                                durumYaz(userx.UserName + "  nolu müşteri servera gönderildi.(Değiştirme)");
                                LogDosyadanIslem(
                                operasyon: "KRMD1AcKapat",
                                tckn: userx.tckno ?? userx.UserName,
                                calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
                                           formAdminAra.referance.ActiveCalisan.Soyad,
                                ip: formAdminAra.referance.IpAdress,
                                hostname: formAdminAra.referance.HostName,
                                degisiklikler: $"Açılan: {acilanlar} | Kapatılan: {kaptianlar}"
                                );
                            }
                            else
                                durumYaz(userx.tckno + " TCKN nolu müşteri - değişiklik yok, mevcut durum korundu.");

                            #endregion
                        }
                        else
                        {
                            // durumYaz(dosyaUser.HesapNo + " numaralı müşteri bulunamadı");
                            durumYaz(dosyaUser.Username + "  numaralı müşteri bulunamadı");

                        }

                        LabelYaz(lblKRMD1AcKapaGonderimDurum, say + " / " + count);
                        say++;
                    }


                    LabelYaz(lblKRMD1AcKapaGonderimDurum, "Bitti");

                });


            }
            catch (Exception ex)
            {
                durumYaz(ex.Message);
            }
        }
        private void btnKRMD1OrnekDosya_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();



                sb.Append("TCKN");
                sb.Append(";");
                sb.Append("KRMD1");
                sb.Append(";");
                sb.Append("SON KULLANMA TARİHİ");
                sb.Append(";");

                sb.Append(Environment.NewLine);

                sb.Append("12345678901");
                sb.Append(";");
                sb.Append("1");
                sb.Append(";");
                sb.Append("20401231");
                sb.Append(";");

                string filename = Application.StartupPath + "\\DosyadanKRMD1AcKapat.csv";

                if (File.Exists(filename))
                    File.Delete(filename);

                File.WriteAllText(filename, sb.ToString(), Encoding.GetEncoding("iso-8859-9"));

                Process.Start(filename);
            }
            catch (Exception ex)
            {
                durumYaz(ex.Message);
            }

        }
        private void brnUserinfo_Click(object sender, EventArgs e)
        {

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("TCKN");
                sb.Append(";");
                sb.Append("Ad");
                sb.Append(";");
                sb.Append("Soyad");
                sb.Append(";");
                sb.Append("PmtsNo");
                sb.Append(";");
                sb.Append("Telefon");
                sb.Append(";");
                sb.Append("Ülke");
                sb.Append(";");
                sb.Append("Şehir");
                sb.Append(";");
                sb.Append("Adres");
                sb.Append(";");


                sb.Append(Environment.NewLine);

                sb.Append("12345678901");
                sb.Append(";");
                sb.Append("Kullanıcı Adı");
                sb.Append(";");
                sb.Append("Kullanıcı Soyadı");
                sb.Append(";");
                sb.Append("KullanıcıHesapNo");
                sb.Append(";");
                sb.Append("0555XXXXXXX");
                sb.Append(";");
                sb.Append("Türkiye");
                sb.Append(";");
                sb.Append("İstanbul");
                sb.Append(";");
                sb.Append("Nazım Hikmet Caddesi Orhan Veli Sok Cemal Sürreyya Apt. No:1 Kat:2 ");
                sb.Append(";");
                string filename = Application.StartupPath + "\\DosyadanUserInfo.csv";

                if (File.Exists(filename))
                    File.Delete(filename);


                File.WriteAllText(filename, sb.ToString(), Encoding.GetEncoding("iso-8859-9"));

                Process.Start(filename);
            }
            catch (Exception ex)
            {
                durumYaz(ex.Message);
            }
        }
        private void btnUserinfoOku_Click(object sender, EventArgs e)
        {
            var satirno = "";
            int okunan = 0, atlanan = 0;
            try
            {
                DosyadanUserinfoListe.Clear();

                OpenFileDialog op = new OpenFileDialog();
                var filename = "";
                op.InitialDirectory = Application.StartupPath;
                op.Filter = "(*.csv)|*.csv";
                if (op.ShowDialog() == DialogResult.OK) filename = op.FileName; else return;

                char[] delimiterChars = { ';' };
                var filestr = File.ReadAllLines(filename, Encoding.GetEncoding("iso-8859-9"));
                if (filestr.Length < 2) { MessageBox.Show("csv dosyası boş"); return; }

                for (int i = 1; i < filestr.Length; i++)
                {
                    satirno = i.ToString();
                    if (string.IsNullOrWhiteSpace(filestr[i])) continue;

                    try
                    {
                        var satirstr = SplitCsv(filestr[i]);
                        if (satirstr.Length < 1)
                        {
                            durumYaz($"{satirno}. satır ATLANDI: boş satır.");
                            atlanan++; continue;
                        }

                        string mno = satirstr[0].Trim();
                        if (mno == "")
                        {
                            durumYaz($"{satirno}. satır ATLANDI: TCKN boş olamaz.");
                            atlanan++; continue;
                        }

                        var dosuser = new DosyadanYeniUser();
                        dosuser.Username = mno;
                        dosuser.Ad = satirstr.Length > 1 ? satirstr[1].Trim() : "";
                        dosuser.Soyad = satirstr.Length > 2 ? satirstr[2].Trim() : "";
                        dosuser.PmtsNo = satirstr.Length > 3 ? satirstr[3].Trim() : "";
                        dosuser.Tel = satirstr.Length > 4 ? satirstr[4].Trim() : "";
                        dosuser.Ulke = satirstr.Length > 5 ? satirstr[5].Trim() : "";
                        dosuser.Sehir = satirstr.Length > 6 ? satirstr[6].Trim() : "";
                        dosuser.Adres = satirstr.Length > 7 ? satirstr[7].Trim() : "";

                        DosyadanUserinfoListe.Add(dosuser);
                        okunan++;
                        UserinfoOkunanKullaniciSayisi = DosyadanUserinfoListe.Count.ToString();
                    }
                    catch (Exception exRow)
                    {
                        durumYaz($"{satirno}. satır ATLANDI (hata): {exRow.Message}");
                        atlanan++;
                    }
                }

                durumYaz($"Okuma bitti. Eklenen: {okunan}, Atlanan: {atlanan}, Toplam veri satırı: {filestr.Length - 1}.");
            }
            catch (Exception ex)
            {
                durumYaz("Dosya okuma hatası: " + ex.Message);
            }
        }




        private void btnUserinfoGonder_Click(object sender, EventArgs e)
        {
            try
            {
                string message;
                string dialogMessage = "Okutulan Tüm Kullanıcılar Server'a gönderilecektir";
                string dialogMessage2 = "Okutulan Tüm Kullanıcılar sadece DB'ye gönderilecektir";
                if (chbOnlySendDb.Checked == true)
                {
                    message = dialogMessage2;
                }
                else
                {
                    message = dialogMessage;
                }
                if (MessageBox.Show(message, "Kullanıcı Güncelleme İşlemi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    return;
                }

                Task thred = Task.Factory.StartNew(() =>
                {

                    if (DosyadanUserinfoListe.Count < 1)
                    {
                        durumYaz("Okutulan Liste Boş"); return;
                    }

                    LabelYaz(lblUserinfoGonderimDurum, "0");
                    var say = 1;
                    var count = DosyadanUserinfoListe.Count;

                    // Sayaçlar
                    int basariliSayisi = 0;
                    int hataliSayisi = 0;
                    int bulunamayanSayisi = 0;

                    var crm = YeniContext();
                    for (int i = 0; i < DosyadanUserinfoListe.Count; i++)
                    {
                        if (i > 0 && i % batchSize == 0)
                        {
                            crm.Dispose();
                            crm = YeniContext();
                        }
                        Thread.Sleep(100);

                        var dosyaUser = DosyadanUserinfoListe[i];

                        try
                        {
                            var userx = crm.Users.FirstOrDefault(x => x.UserName == dosyaUser.Username);
                            if (userx != null)
                            {
                               // var userx = crm.Users.FirstOrDefault(x => x.UserName == dosyaUser.Username);

                                // Güncellenen alanları takip et
                                var guncellemeler = new StringBuilder();

                                if (dosyaUser.Ad != "" && dosyaUser.Ad != userx.Name)
                                {
                                    userx.Name = dosyaUser.Ad;
                                    guncellemeler.Append("Ad, ");
                                }
                                if (dosyaUser.Soyad != "" && dosyaUser.Soyad != userx.Surname)
                                {
                                    userx.Surname = dosyaUser.Soyad;
                                    guncellemeler.Append("Soyad, ");
                                }
                                if (dosyaUser.PmtsNo != "" && dosyaUser.PmtsNo != userx.PmtsNo)
                                {
                                    userx.PmtsNo = dosyaUser.PmtsNo.Replace("|", ";");
                                    guncellemeler.Append("PmtsNo, ");
                                }

                                if (userx.Iletisim == null)
                                {
                                    var iletisim = new Iletisim();

                                    if (dosyaUser.Tel != "")
                                    {
                                        iletisim.Tel1 = dosyaUser.Tel;
                                        guncellemeler.Append("Telefon, ");
                                    }
                                    if (dosyaUser.Adres != "")
                                    {
                                        iletisim.acikadres = dosyaUser.Adres;
                                        guncellemeler.Append("Adres, ");
                                    }

                                    if (dosyaUser.Ulke != "")
                                    {
                                        var il = MyTools.SehirIdBul(dosyaUser.Sehir);

                                        if (il != null && il.Id > 0)
                                        {
                                            iletisim.IlId = il.Id;
                                            iletisim.UlkeId = il.UlkeId;
                                            guncellemeler.Append("Şehir/Ülke, ");
                                        }
                                    }
                                    crm.Iletisims.InsertOnSubmit(iletisim);
                                    userx.Iletisim = iletisim;
                                }
                                else
                                {
                                    if (dosyaUser.Tel != "" && dosyaUser.Tel != userx.Iletisim.Tel1)
                                    {
                                        userx.Iletisim.Tel1 = dosyaUser.Tel;
                                        guncellemeler.Append("Telefon, ");
                                    }
                                    if (dosyaUser.Adres != "" && dosyaUser.Adres != userx.Iletisim.acikadres)
                                    {
                                        userx.Iletisim.acikadres = dosyaUser.Adres;
                                        guncellemeler.Append("Adres, ");
                                    }
                                    if (dosyaUser.Ulke != "")
                                    {
                                        var il = MyTools.SehirIdBul(dosyaUser.Sehir);
                                        if (il.Id > 0)
                                        {
                                            userx.Iletisim.IlId = il.Id;
                                            userx.Iletisim.UlkeId = il.UlkeId;
                                            guncellemeler.Append("Şehir/Ülke, ");
                                            if (il.Ulke.UlkeAdi == "Türkiye")
                                            {
                                                userx.MusteriMenseiID = 1;
                                            }
                                            else
                                            {
                                                userx.MusteriMenseiID = 2;
                                            }
                                        }
                                        else
                                        {
                                            var ulke = MyTools.UlkeIdBul(dosyaUser.Ulke);
                                            if (ulke != null)
                                            {
                                                userx.Iletisim.UlkeId = ulke.Id;
                                                userx.Iletisim.IlId = null;
                                                guncellemeler.Append("Ülke, ");
                                                if (ulke.UlkeAdi == "Türkiye")
                                                {
                                                    userx.MusteriMenseiID = 1;
                                                }
                                                else
                                                {
                                                    userx.MusteriMenseiID = 2;
                                                }
                                            }
                                            else
                                            {
                                                durumYaz($"[UYARI] {dosyaUser.Username} - {dosyaUser.Ulke} adında ülke bulunamadı");
                                            }
                                        }
                                    }
                                }

                                crm.SubmitChanges();

                                if (chbOnlySendDb.Checked == false)
                                {
                                    formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + userx.UserID.ToString() + (char)3;
                                }

                                // Başarılı güncelleme logu
                                string guncellenenAlanlar = guncellemeler.ToString().TrimEnd(' ', ',');
                                if (!string.IsNullOrEmpty(guncellenenAlanlar))
                                {
                                    durumYaz($"[OK] {dosyaUser.Username} - Güncellenen: {guncellenenAlanlar}");
                                    LogDosyadanIslem(
                                                operasyon: "UserInfoGuncelle",
                                                tckn: dosyaUser.Username,
                                                calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
                                                           formAdminAra.referance.ActiveCalisan.Soyad,
                                                ip: formAdminAra.referance.IpAdress,
                                                hostname: formAdminAra.referance.HostName,
                                                degisiklikler: guncellenenAlanlar
                                            );
                                }
                                else
                                {
                                    durumYaz($"[OK] {dosyaUser.Username} - Değişiklik yok");
                                }

                                basariliSayisi++;
                            }
                            else
                            {
                                durumYaz($"[BULUNAMADI] {dosyaUser.Username} numaralı müşteri bulunamadı");
                                bulunamayanSayisi++;
                            }
                        }
                        catch (Exception ex)
                        {
                            hataliSayisi++;
                            durumYaz($"[HATA] {dosyaUser.Username} - {ex.Message}");
                            MyTools.logyaz($"UserInfo güncelleme hatası - TCKN: {dosyaUser.Username}, Hata: {ex.Message}");
                        }

                        LabelYaz(lblUserinfoGonderimDurum, say + " / " + count);
                        say++;
                    }

                    // İşlem özeti

                    durumYaz($"[ÖZET] Toplam: {count}, Başarılı: {basariliSayisi}, Bulunamayan: {bulunamayanSayisi}, Hatalı: {hataliSayisi}");
                    LabelYaz(lblUserinfoGonderimDurum, "Bitti");

                });
            }
            catch (Exception ex)
            {
                durumYaz($"[KRİTİK HATA] {ex.Message}");
                MyTools.logyaz($"UserInfo toplu güncelleme kritik hatası: {ex.Message}");
            }
        }
        private void btnTarihErtele_Click(object sender, EventArgs e)
        {
            //burası artık kullanılmıyor.
        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void lblKRMD1AcKapaOkunanKlullaniciSayisi_Click(object sender, EventArgs e)
        {

        }

        private void btnKrmdYap_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (MessageBox.Show("Okutulan Tüm Kullanıcılar Server'a gönderilecektir ", "Kullanıcı Açma İşlemi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
            //    {
            //        return;
            //    }
            //    Task thred = Task.Factory.StartNew(() =>
            //    {

            //        crmDFNDataContext crm = new crmDFNDataContext();

            //        var sorgu = crm.Users.Where(x => (x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false && x.LisansDurum.Pd2P == false) ||
            //             (x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false && x.LisansDurum.Vd2P == false) || (x.LisansDurum.TahvilL1 == true
            //             && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false) || x.LisansDurum.SPI
            //             && x.LisansDurum.YayinDurumu == true).ToList();



            //        var count = sorgu.Count;

            //        for (int i = 0; i < sorgu.Count; i++)
            //        {
            //            UserEvent ue = new UserEvent();
            //            ue.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
            //            ue.EventTarih = DateTime.Now;
            //            ue.IP = formAdminAra.referance.IpAdress;
            //            ue.HostName = formAdminAra.referance.HostName;
            //            var yayind = true;
            //            var kaptianlar = new StringBuilder();
            //            var acilanlar = new StringBuilder();
            //            ue.UserId = sorgu[i].UserID;
            //            ue.SonLisandurumID = sorgu[i].LisansDurumId;

            //            if (sorgu[i].LisansDurum.YayinDurumu != yayind)
            //            {

            //                if (yayind == true)
            //                    ue.EventTypeId = 1;
            //                else
            //                    ue.EventTypeId = 2;

            //                sorgu[i].LisansDurum.YayinDurumu = yayind;

            //                crm.UserEvents.InsertOnSubmit(ue);
            //                crm.SubmitChanges();
            //            }
            //            else
            //                sorgu[i].LisansDurum.YayinDurumu = yayind;

            //            if (sorgu[i].LisansDurum.SPI == true)
            //            {
            //                sorgu[i].LisansDurum.SPI = false;
            //                kaptianlar.Append("PAKET10;");
            //            }
            //            if (sorgu[i].LisansDurum.TahvilL1 == true && sorgu[i].LisansDurum.TahvilL2 == false && sorgu[i].LisansDurum.TahvilLP == false)
            //            {
            //                sorgu[i].LisansDurum.TahvilL1 = false;
            //                kaptianlar.Append("TahvilL1;");
            //            }
            //            if (sorgu[i].LisansDurum.PayL1 == true && sorgu[i].LisansDurum.PayL2 == false && sorgu[i].LisansDurum.PayLP == false && sorgu[i].LisansDurum.Pd2P == false)
            //            {
            //                sorgu[i].LisansDurum.PayL1 = false;
            //                kaptianlar.Append("PayL1;");
            //            }
            //            if (sorgu[i].LisansDurum.ViopL1 == true && sorgu[i].LisansDurum.ViopL2 == false && sorgu[i].LisansDurum.ViopLP == false && sorgu[i].LisansDurum.Vd2P == false)
            //            {
            //                sorgu[i].LisansDurum.ViopL1 = false;
            //                kaptianlar.Append("ViopL1;");
            //            }
            //            if (sorgu[i].LisansDurum.COMEX == false)
            //            {
            //                sorgu[i].LisansDurum.COMEX = true;
            //                acilanlar.Append("KRMD1;");
            //            }
            //            if (sorgu[i].LisansDurum.PayX == false)
            //            {
            //                sorgu[i].LisansDurum.PayX = true;
            //                acilanlar.Append("PayX;");
            //            }

            //            if (acilanlar.ToString() != "" || kaptianlar.ToString() != "")
            //            {
            //                if (acilanlar.ToString() != "" && kaptianlar.ToString() != "")
            //                {
            //                    ue.EventTypeId = 6;
            //                    ue.KapatilanLisans = kaptianlar.ToString();
            //                    ue.AcilanLisans = acilanlar.ToString();

            //                }
            //                else
            //                {
            //                    if (acilanlar.ToString() != "")
            //                    {
            //                        ue.EventTypeId = 4;
            //                        ue.AcilanLisans = acilanlar.ToString();
            //                    }
            //                    if (kaptianlar.ToString() != "")
            //                    {
            //                        ue.EventTypeId = 3;
            //                        ue.KapatilanLisans = kaptianlar.ToString();
            //                    }
            //                }

            //            }
            //            if (ue.EventId < 1)
            //                crm.UserEvents.InsertOnSubmit(ue);
            //            crm.SubmitChanges();
            //            formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + sorgu[i].UserID.ToString() + (char)3;

            //            //lblYuzeyselKarmaCevir.Text = (i + 1).ToString() + "/" + count.ToString();
            //        }
            //    });
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        private void btnPaket10Cevir_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (MessageBox.Show("Okutulan Tüm Kullanıcılar Server'a gönderilecektir ", "Kullanıcı Açma İşlemi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
            //    {
            //        return;
            //    }
            //    Task thred = Task.Factory.StartNew(() =>
            //    {

            //        crmDFNDataContext crm = new crmDFNDataContext();

            //        var sorgu = crm.Users.Where(x => x.LisansDurum.SPI == true && x.LisansDurum.YayinDurumu == true).ToList();

            //        var count = sorgu.Count;

            //        for (int i = 0; i < sorgu.Count; i++)
            //        {
            //            UserEvent ue = new UserEvent();
            //            ue.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
            //            ue.EventTarih = DateTime.Now;
            //            ue.IP = formAdminAra.referance.IpAdress;
            //            ue.HostName = formAdminAra.referance.HostName;
            //            var yayind = true;
            //            var kaptianlar = new StringBuilder();
            //            var acilanlar = new StringBuilder();
            //            ue.UserId = sorgu[i].UserID;
            //            ue.SonLisandurumID = sorgu[i].LisansDurumId;

            //            if (sorgu[i].LisansDurum.YayinDurumu != yayind)
            //            {

            //                if (yayind == true)
            //                    ue.EventTypeId = 1;
            //                else
            //                    ue.EventTypeId = 2;

            //                sorgu[i].LisansDurum.YayinDurumu = yayind;

            //                crm.UserEvents.InsertOnSubmit(ue);
            //                crm.SubmitChanges();
            //            }
            //            else
            //                sorgu[i].LisansDurum.YayinDurumu = yayind;

            //            if (sorgu[i].LisansDurum.SPI == true)
            //            {
            //                sorgu[i].LisansDurum.SPI = false;
            //                kaptianlar.Append("PAKET10;");
            //            }
            //            if (sorgu[i].LisansDurum.COMEX == false)
            //            {
            //                sorgu[i].LisansDurum.COMEX = true;
            //                acilanlar.Append("KRMD1;");
            //            }

            //            if (acilanlar.ToString() != "" || kaptianlar.ToString() != "")
            //            {
            //                if (acilanlar.ToString() != "" && kaptianlar.ToString() != "")
            //                {
            //                    ue.EventTypeId = 6;
            //                    ue.KapatilanLisans = kaptianlar.ToString();
            //                    ue.AcilanLisans = acilanlar.ToString();

            //                }
            //                else
            //                {
            //                    if (acilanlar.ToString() != "")
            //                    {
            //                        ue.EventTypeId = 4;
            //                        ue.AcilanLisans = acilanlar.ToString();
            //                    }
            //                    if (kaptianlar.ToString() != "")
            //                    {
            //                        ue.EventTypeId = 3;
            //                        ue.KapatilanLisans = kaptianlar.ToString();
            //                    }
            //                }

            //            }
            //            if (ue.EventId < 1)
            //                crm.UserEvents.InsertOnSubmit(ue);
            //            crm.SubmitChanges();
            //            formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + sorgu[i].UserID.ToString() + (char)3;

            //        }
            //        //lblPaket10KarmaCevir.Text =  count.ToString();
            //    });
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
        private void btnUserPassword_Click(object sender, EventArgs e)
        {
            try
            {

                StringBuilder sb = new StringBuilder();

                sb.Append("TCKN");
                sb.Append(";");
                sb.Append("SIFRE");
                sb.Append(";");

                sb.Append(Environment.NewLine);

                sb.Append("12345678901");
                sb.Append(";");
                sb.Append("Akyatirim1");
                sb.Append(";");

                string filename = Application.StartupPath + "\\DosyadanPasswordDegistir.csv";

                if (File.Exists(filename))
                    File.Delete(filename);

                File.WriteAllText(filename, sb.ToString(), Encoding.GetEncoding("iso-8859-9"));
                Process.Start(filename);
            }
            catch (Exception ex)
            {
                durumYaz(ex.Message);
            }
        }
        private void btnUserPasswordOku_Click(object sender, EventArgs e)
        {
            var satirno = "";
            int okunan = 0, atlanan = 0;
            try
            {
                DosyadanUserPasswordListe.Clear();

                OpenFileDialog op = new OpenFileDialog();
                var filename = "";
                op.InitialDirectory = Application.StartupPath;
                op.Filter = "(*.csv)|*.csv";
                if (op.ShowDialog() == DialogResult.OK) filename = op.FileName; else return;

                var filestr = File.ReadAllLines(filename, Encoding.GetEncoding("iso-8859-9"));
                if (filestr.Length < 2) { MessageBox.Show("csv dosyası boş"); return; }

                for (int i = 1; i < filestr.Length; i++)
                {
                    satirno = i.ToString();
                    if (string.IsNullOrWhiteSpace(filestr[i])) continue;

                    try
                    {
                        var satirstr = SplitCsv(filestr[i]);
                        if (satirstr.Length < 2)
                        {
                            durumYaz($"{satirno}. satır ATLANDI: temel alanlar eksik (kolon {satirstr.Length}, en az 2 gerekli).");
                            atlanan++; continue;
                        }

                        string tckn = satirstr[0].Trim();
                        string password = satirstr[1].Trim();

                        if (string.IsNullOrEmpty(tckn))
                        {
                            durumYaz($"{satirno}. satır ATLANDI: TCKN boş olamaz.");
                            atlanan++; continue;
                        }
                        if (string.IsNullOrEmpty(password))
                        {
                            durumYaz($"{satirno}. satır ATLANDI: Şifre boş olamaz.");
                            atlanan++; continue;
                        }

                        var dosuser = new DosyadanYeniUser();
                        dosuser.Tckn = tckn;
                        dosuser.Username = tckn;
                        dosuser.Sifre = password;

                        DosyadanUserPasswordListe.Add(dosuser);
                        okunan++;
                        UserPasswordOkunanKullaniciSayisi = DosyadanUserPasswordListe.Count.ToString();
                    }
                    catch (Exception exRow)
                    {
                        durumYaz($"{satirno}. satır ATLANDI (hata): {exRow.Message}");
                        atlanan++;
                    }
                }

                durumYaz($"Okuma bitti. Eklenen: {okunan}, Atlanan: {atlanan}, Toplam veri satırı: {filestr.Length - 1}.");
            }
            catch (Exception ex)
            {
                durumYaz("Dosya okuma hatası: " + ex.Message);
            }
        }

        private void btnUserPasswordGonder_Click(object sender, EventArgs e)
        {
            try
            {
                string message;
                string dialogMessage = "Okutulan Tüm Kullanıcılar Server'a gönderilecektir";
                string dialogMessage2 = "Okutulan Tüm Kullanıcılar sadece DB'ye gönderilecektir";
                if (chbOnlyPasswordSendDb.Checked == true)
                {
                    message = dialogMessage2;
                }
                else
                {
                    message = dialogMessage;
                }
                if (MessageBox.Show(message, "Kullanıcı Güncelleme İşlemi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    return;
                }

                Task thred = Task.Factory.StartNew(() =>
                {
                    if (DosyadanUserPasswordListe.Count < 1) { durumYaz("Okutulan Liste Boş"); return; }

                    LabelYaz(lblUserPasswordGonderimDurum, "0");
                    var say = 1;
                    var count = DosyadanUserPasswordListe.Count;

                    int basariliSayisi = 0, hataliSayisi = 0, bulunamayanSayisi = 0, atlananSayisi = 0;

                    for (int i = 0; i < DosyadanUserPasswordListe.Count; i++)
                    {
                        Thread.Sleep(100);
                        var dosyaUser = DosyadanUserPasswordListe[i];

                        try
                        {
                            using (var crm = YeniContext())
                            {
                                var userx = crm.Users.FirstOrDefault(x => x.UserName == dosyaUser.Username);
                                if (userx == null)
                                {
                                    durumYaz($"[BULUNAMADI] {dosyaUser.Username} TCKN numaralı müşteri bulunamadı");
                                    bulunamayanSayisi++;
                                }
                                else if (dosyaUser.Sifre.Length < 1)
                                {
                                    durumYaz($"[ATLANDI] {userx.UserName} - Şifre en az 1 karakter olmalıdır.");
                                    atlananSayisi++;
                                }
                                else
                                {
                                    string encryptedPassword = MyTools.Sifreleme.Encryp(dosyaUser.Sifre);

                                    // Kolon sınırı koruması — SQL truncation hatasını sessiz çökme yerine anlamlı mesaja çevirir.
                                    // 50'yi Password kolonunun gerçek boyutuna göre ayarla (şifreli değerin uzunluğu esas).
                                    if (encryptedPassword.Length > 50)
                                    {
                                        durumYaz($"[ATLANDI] {userx.UserName} - Şifre çok uzun (şifreli uzunluk {encryptedPassword.Length} > 50).");
                                        atlananSayisi++;
                                    }
                                    else if (userx.Password == encryptedPassword)
                                    {
                                        durumYaz($"[UYARI] {userx.UserName} - Yeni şifre mevcut şifre ile aynı, atlandı.");
                                        atlananSayisi++;
                                    }
                                    else
                                    {
                                        userx.Password = encryptedPassword;
                                        crm.SubmitChanges();

                                        UserEvent ue = new UserEvent();
                                        ue.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
                                        ue.EventTarih = DateTime.Now;
                                        ue.IP = formAdminAra.referance.IpAdress;
                                        ue.HostName = formAdminAra.referance.HostName;
                                        ue.UserId = userx.UserID;
                                        ue.SonLisandurumID = userx.LisansDurumId;
                                        ue.EventTypeId = 4;
                                        ue.AcilanLisans = "SifreDegistirme";
                                        crm.UserEvents.InsertOnSubmit(ue);
                                        crm.SubmitChanges();

                                        LogDosyadanIslem(
                                            operasyon: "SifreDegistirme",
                                            tckn: userx.UserName,
                                            calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
                                                       formAdminAra.referance.ActiveCalisan.Soyad,
                                            ip: formAdminAra.referance.IpAdress,
                                            hostname: formAdminAra.referance.HostName,
                                            degisiklikler: "Şifre güncellendi"
                                        );

                                        durumYaz($"[OK] {userx.UserName} kullanıcısının şifresi güncellendi.");

                                        if (chbOnlyPasswordSendDb.Checked == false)
                                            formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + userx.UserID.ToString() + (char)3;

                                        basariliSayisi++;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            hataliSayisi++;
                            durumYaz($"[HATA] {dosyaUser.Username} - {ex.Message}");
                            MyTools.logyaz($"Şifre güncelleme hatası - {dosyaUser.Username}: {ex.Message}");
                            LogDosyadanIslem(
                                operasyon: "SifreDegistirme",
                                tckn: dosyaUser.Username,
                                calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
                                           formAdminAra.referance.ActiveCalisan.Soyad,
                                ip: formAdminAra.referance.IpAdress,
                                hostname: formAdminAra.referance.HostName,
                                degisiklikler: null,
                                basarili: false,
                                hata: ex.Message
                            );
                        }

                        LabelYaz(lblUserPasswordGonderimDurum, say + " / " + count);
                        say++;
                    }

                    durumYaz($"[ÖZET] Toplam: {count}, Başarılı: {basariliSayisi}, Bulunamayan: {bulunamayanSayisi}, Atlanan: {atlananSayisi}, Hatalı: {hataliSayisi}");
                    LabelYaz(lblUserPasswordGonderimDurum, "Bitti");
                });
            }
            catch (Exception ex)
            {
                // hataliSayisi++;
                //  durumYaz($"[HATA] {dosyaUser.Username} - {ex.Message}");
                LogDosyadanIslem(
                    operasyon: "SifreDegistirme",
                    tckn: "",
                    calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
                               formAdminAra.referance.ActiveCalisan.Soyad,
                    ip: formAdminAra.referance.IpAdress,
                    hostname: formAdminAra.referance.HostName,
                    degisiklikler: null,
                    basarili: false,
                    hata: ex.Message
                );
                durumYaz(ex.Message);
            }
        }
        private void btnLisansTarihOrnekDosya_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // Header satırı
                sb.Append("TCKN;AD;SOYAD;TELEFON;MAIL;SEHIR;ADRES;EXPIRYDATE");
                sb.Append(";PRO;PROSD;PROED");
                sb.Append(";MOBIL;MOBILSD;MOBILED");
                sb.Append(";PD1P;PD1PSD;PD1PED");
                sb.Append(";PD2;PD2SD;PD2ED");
                sb.Append(";PD2P;PD2PSD;PD2PED");
                sb.Append(";END;ENDSD;ENDED");
                sb.Append(";PIT;PITSD;PITED");
                sb.Append(";PITE;PITESD;PITEED");
                sb.Append(";MKK;MKKSD;MKKED");
                sb.Append(";GKKUL;GKKULSD;GKKULED");
                sb.Append(";VD1P;VD1PSD;VD1PED");
                sb.Append(";VD2;VD2SD;VD2ED");
                sb.Append(";VD2P;VD2PSD;VD2PED");
                sb.Append(";VIT;VITSD;VITED");
                sb.Append(";BD1P;BD1PSD;BD1PED");
                sb.Append(";BD2;BD2SD;BD2ED");
                sb.Append(";KRMD1;KRMD1SD;KRMD1ED");
                sb.Append(";ALG;ALGSD;ALGED");
                sb.Append(Environment.NewLine);

                // Örnek veri satırı
                sb.Append("12345678901;Kullanıcı Adı;Kullanıcı Soyadı;05551234567;test@ideal.data.com.tr;İstanbul;Barbaros Mah. Ihlamur Blv. No:3;20261231");
                sb.Append(";1;20260101;20261231");  // DESKTOP
                sb.Append(";1;20260101;20261231");  // MOBIL 
                sb.Append(";1;20260101;20261231");  // PD1P
                sb.Append(";0;;");                   // PD2
                sb.Append(";0;;");                   // PD2P
                sb.Append(";0;;");                   // END
                sb.Append(";0;;");                   // PIT
                sb.Append(";0;;");                   // PITE
                sb.Append(";0;;");                   // MKK
                sb.Append(";0;;");                   // GKKUL
                sb.Append(";0;;");                   // VD1P
                sb.Append(";0;;");                   // VD2
                sb.Append(";0;;");                   // VD2P
                sb.Append(";0;;");                   // VIT
                sb.Append(";0;;");                   // BD1P
                sb.Append(";0;;");                   // BD2
                sb.Append(";0;;");                   // KRMD1
                sb.Append(";0;;");                   // CME

                string filename = Application.StartupPath + "\\DosyadanLisansTarihGuncelle.csv";

                if (File.Exists(filename))
                    File.Delete(filename);

                File.WriteAllText(filename, sb.ToString(), Encoding.GetEncoding("iso-8859-9"));
                Process.Start(filename);
            }
            catch (Exception ex)
            {
                durumYaz(ex.Message);
            }
        }
        private DateTime? ParseTarihNullable(string tarihStr)
        {
            if (string.IsNullOrWhiteSpace(tarihStr))
                return null;
            return DateTime.ParseExact(tarihStr.Trim(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
        }

        private void btnLisansTarihOku_Click(object sender, EventArgs e)
        {
            var satirno = "";
            int okunan = 0, atlanan = 0;
            try
            {
                DosyadanLisansTarihListe.Clear();

                OpenFileDialog op = new OpenFileDialog();
                var filename = "";
                op.InitialDirectory = Application.StartupPath;
                op.Filter = "(*.csv)|*.csv";
                if (op.ShowDialog() == DialogResult.OK) filename = op.FileName; else return;

                char[] delimiterChars = { ';' };
                var filestr = File.ReadAllLines(filename, Encoding.GetEncoding("iso-8859-9"));
                if (filestr.Length < 2) { MessageBox.Show("csv dosyası boş"); return; }

                for (int i = 1; i < filestr.Length; i++)
                {
                    satirno = i.ToString();
                    if (string.IsNullOrWhiteSpace(filestr[i])) continue;

                    try
                    {
                        var satirstr = SplitCsv(filestr[i]);

                        if (satirstr.Length < 8)
                        {
                            durumYaz($"{satirno}. satır ATLANDI: yeterli sütun yok (kolon {satirstr.Length}, en az 8 gerekli).");
                            atlanan++; continue;
                        }

                        string tckn = satirstr[0].Trim();
                        if (string.IsNullOrEmpty(tckn))
                        {
                            durumYaz($"{satirno}. satır ATLANDI: TCKN boş olamaz.");
                            atlanan++; continue;
                        }

                        var dosuser = new DosyadanYeniUser();
                        dosuser.Tckn = tckn;
                        dosuser.Username = tckn;
                        dosuser.Ad = satirstr.Length > 1 ? satirstr[1].Trim() : "";
                        dosuser.Soyad = satirstr.Length > 2 ? satirstr[2].Trim() : "";
                        dosuser.Tel = satirstr.Length > 3 ? satirstr[3].Trim() : "";
                        dosuser.mail = satirstr.Length > 4 ? satirstr[4].Trim() : "";
                        dosuser.Sehir = satirstr.Length > 5 ? satirstr[5].Trim() : "";
                        dosuser.Adres = satirstr.Length > 6 ? satirstr[6].Trim() : "";

                        string expiryStr = satirstr.Length > 7 ? satirstr[7].Trim() : "";
                        if (!string.IsNullOrEmpty(expiryStr))
                        {
                            DateTime expParsed;
                            if (!DateTime.TryParseExact(expiryStr, "yyyyMMdd",
                                    CultureInfo.InvariantCulture, DateTimeStyles.None, out expParsed))
                            {
                                durumYaz($"{satirno}. satır ATLANDI: EXPIRYDATE formatı hatalı ('{expiryStr}'). Format yyyyMMdd olmalı.");
                                atlanan++; continue;
                            }
                            dosuser.expirydate = expParsed;
                        }

                        // Lisans blokları: her biri 3 sütun (DURUM;START;END)
                        // DESKTOP(8-10), MOBIL(11-13), PD1P(14-16), PD2(17-19), PD2P(20-22),
                        // END(23-25), PIT(26-28), PITE(29-31), MKK(32-34), GKKUL(35-37),
                        // VD1P(38-40), VD2(41-43), VD2P(44-46), VIT(47-49),
                        // BD1P(50-52), BD2(53-55), KRMD1(56-58), CME(59-61)
                        int idx = 8;

                        if (satirstr.Length > idx) { dosuser.DESKTOP = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.DESKTOP_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.DESKTOP_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.MOBIL = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.MOBIL_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.MOBIL_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.PD1P = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.PD1P_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.PD1P_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.PD2 = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.PD2_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.PD2_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.PD2P = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.PD2P_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.PD2P_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.END = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.END_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.END_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.PIT = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.PIT_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.PIT_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.PITE = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.PITE_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.PITE_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.MKK = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.MKK_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.MKK_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.GKKUL = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.GKKUL_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.GKKUL_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.VD1P = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.VD1P_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.VD1P_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.VD2 = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.VD2_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.VD2_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.VD2P = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.VD2P_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.VD2P_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.VIT = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.VIT_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.VIT_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.BD1P = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.BD1P_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.BD1P_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.BD2 = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.BD2_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.BD2_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.KRMD1 = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.KRMD1_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.KRMD1_End = ParseTarihNullable(satirstr[idx + 2]); }
                        idx += 3;

                        if (satirstr.Length > idx) { dosuser.CME = satirstr[idx].Trim()._ToBool(); }
                        if (satirstr.Length > idx + 1) { dosuser.CME_Start = ParseTarihNullable(satirstr[idx + 1]); }
                        if (satirstr.Length > idx + 2) { dosuser.CME_End = ParseTarihNullable(satirstr[idx + 2]); }

                        // Lisans hiyerarşisi (mevcut mantık)
                        if (dosuser.PD1P) { dosuser.PD1 = true; }
                        if (dosuser.PD2) { dosuser.PD1 = true; dosuser.PD1P = true; }
                        if (dosuser.PD2P) { dosuser.PD1 = true; dosuser.PD1P = true; }
                        if (dosuser.VD1P) { dosuser.VD1 = true; }
                        if (dosuser.VD2) { dosuser.VD1 = true; dosuser.VD1P = true; }
                        if (dosuser.VD2P) { dosuser.VD1 = true; dosuser.VD1P = true; }
                        if (dosuser.BD1P) { dosuser.BD1 = true; }
                        if (dosuser.BD2) { dosuser.BD1 = true; dosuser.BD1P = true; }

                        DosyadanLisansTarihListe.Add(dosuser);
                        okunan++;
                        LisansTarihOkunanKullaniciSayisi = DosyadanLisansTarihListe.Count.ToString();
                    }
                    catch (Exception exRow)
                    {
                        durumYaz($"{satirno}. satır ATLANDI (hata): {exRow.Message}");
                        atlanan++;
                    }
                }

                durumYaz($"Okuma bitti. Eklenen: {okunan}, Atlanan: {atlanan}, Toplam veri satırı: {filestr.Length - 1}.");
            }
            catch (Exception ex)
            {
                durumYaz("Dosya okuma hatası: " + ex.Message);
            }
        }

        /// <summary>
        /// Mevcut ayın son günü.
        /// </summary>
        private DateTime AySonu()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        }

        /// <summary>
        /// Bir lisansın aktif olup olmadığını kontrol eder.
        /// Aktif = flag true VE end tarihi bugün veya sonrası.
        /// </summary>
        private bool LisansAktif(bool flag, DateTime? endTarih)
        {
            if (!flag) return false;
            if (!endTarih.HasValue) return false;
            return endTarih.Value.Date >= DateTime.Now.Date;
        }
        private void btnLisansTarihGonder_Click(object sender, EventArgs e)
        {
            try
            {
                string message;
                if (chbLisansTarihOnlySendDb.Checked)
                    message = "Okutulan Tüm Kullanıcılar sadece DB'ye gönderilecektir";
                else
                    message = "Okutulan Tüm Kullanıcılar Server'a gönderilecektir";

                if (MessageBox.Show(message, "Kullanıcı Lisans Tarihleri Güncelleme", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    return;
                }

                Task thred = Task.Factory.StartNew(() =>
                {
                    if (DosyadanLisansTarihListe.Count < 1)
                    {
                        durumYaz("Okutulan Liste Boş"); return;
                    }

                    LabelYaz(lblLisansTarihGonderimDurum, "0");
                    var say = 1;
                    var count = DosyadanLisansTarihListe.Count;

                    int basariliSayisi = 0;
                    int hataliSayisi = 0;
                    int bulunamayanSayisi = 0;

                    var crm = YeniContext();

                    for (int i = 0; i < DosyadanLisansTarihListe.Count; i++)
                    {
                        if (i > 0 && i % batchSize == 0)
                        {
                            crm.Dispose();
                            crm = YeniContext();
                        }
                        Thread.Sleep(100);
                        var dosyaUser = DosyadanLisansTarihListe[i];

                        try
                        {
                            if (!crm.Users.Where(x => x.UserName == dosyaUser.Username).Any())
                            {
                                durumYaz($"[BULUNAMADI] {dosyaUser.Username} TCKN numaralı müşteri bulunamadı");
                                bulunamayanSayisi++;

                                LogDosyadanIslem(
                                    operasyon: "LisansTarihGuncelle",
                                    tckn: dosyaUser.Username,
                                    calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
                                               formAdminAra.referance.ActiveCalisan.Soyad,
                                    ip: formAdminAra.referance.IpAdress,
                                    hostname: formAdminAra.referance.HostName,
                                    degisiklikler: null,
                                    basarili: false,
                                    hata: "Kullanıcı bulunamadı"
                                );

                                LabelYaz(lblLisansTarihGonderimDurum, say + " / " + count);
                                say++;
                                continue;
                            }

                            var userx = crm.Users.FirstOrDefault(x => x.UserName == dosyaUser.Username);

                            UserEvent ue = new UserEvent();
                            ue.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
                            ue.EventTarih = DateTime.Now;
                            ue.IP = formAdminAra.referance.IpAdress;
                            ue.HostName = formAdminAra.referance.HostName;
                            ue.UserId = userx.UserID;

                            var acilanlar = new StringBuilder();
                            var guncellenenler = new StringBuilder();

                            // --- Kullanıcı Bilgileri Güncelleme ---
                            if (dosyaUser.Ad != "" && dosyaUser.Ad != userx.Name) { userx.Name = dosyaUser.Ad; guncellenenler.Append("Ad, "); }
                            if (dosyaUser.Soyad != "" && dosyaUser.Soyad != userx.Surname) { userx.Surname = dosyaUser.Soyad; guncellenenler.Append("Soyad, "); }
                            if (dosyaUser.Ad != "" || dosyaUser.Soyad != "") { userx.Aciklama = (dosyaUser.Ad + " " + dosyaUser.Soyad).Trim(); }

                            if (dosyaUser.expirydate != DateTime.MinValue)
                                userx.ExpiryDate = dosyaUser.expirydate;

                            // --- İletişim Güncelleme ---
                            if (userx.iletisimId == null)
                            {
                                var iletisim = new Iletisim();
                                bool iletisimVar = false;

                                if (dosyaUser.Adres != "") { iletisim.acikadres = dosyaUser.Adres; iletisimVar = true; }
                                if (dosyaUser.Tel != "") { iletisim.Tel1 = dosyaUser.Tel; iletisimVar = true; }
                                if (dosyaUser.Sehir != "")
                                {
                                    var il = MyTools.SehirIdBul(dosyaUser.Sehir);
                                    if (il != null && il.Id > 0) { iletisim.IlId = il.Id; iletisim.UlkeId = il.UlkeId; iletisimVar = true; }
                                }
                                if (dosyaUser.mail != "") { iletisim.email = dosyaUser.mail; iletisimVar = true; }

                                if (iletisimVar)
                                {
                                    crm.Iletisims.InsertOnSubmit(iletisim);
                                    crm.SubmitChanges();
                                    userx.iletisimId = iletisim.IletisimId;
                                    guncellenenler.Append("İletişim(yeni), ");
                                }
                            }
                            else
                            {
                                if (dosyaUser.Adres != "" && dosyaUser.Adres != userx.Iletisim.acikadres) { userx.Iletisim.acikadres = dosyaUser.Adres; guncellenenler.Append("Adres, "); }
                                if (dosyaUser.Tel != "" && dosyaUser.Tel != userx.Iletisim.Tel1) { userx.Iletisim.Tel1 = dosyaUser.Tel; guncellenenler.Append("Telefon, "); }
                                if (dosyaUser.Sehir != "")
                                {
                                    var il = MyTools.SehirIdBul(dosyaUser.Sehir);
                                    if (il != null && il.Id > 0) { userx.Iletisim.IlId = il.Id; userx.Iletisim.UlkeId = il.UlkeId; guncellenenler.Append("Şehir, "); }
                                }
                                if (dosyaUser.mail != "" && dosyaUser.mail != userx.Iletisim.email) { userx.Iletisim.email = dosyaUser.mail; guncellenenler.Append("Email, "); }
                            }

                            // --- Yeni Lisans Durumu Oluşturma ---
                            LisansDurum lisanslar = new LisansDurum();
                            lisanslar.YayinDurumu = true;
                            lisanslar.Futgck = true;
                            lisanslar.WINX = true;

                            string tstr = DateTime.Now.ToString("yyyy-MM-dd");
                            DateTime defaultStart = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                            DateTime defaultEnd = AySonu();
                            DateTime aySonu = AySonu();

                            // ============================================================
                            // DESKTOP / ProYetki
                            // ============================================================
                            if (dosyaUser.DESKTOP)
                            {
                                lisanslar.ProYetki = true;
                                lisanslar.ProYetkiStart = dosyaUser.DESKTOP_Start ?? defaultStart;
                                lisanslar.ProYetkiEnd = dosyaUser.DESKTOP_End ?? defaultEnd;
                                if (userx.LisansDurum?.ProYetki != true) acilanlar.Append("ProYetki;");
                                guncellenenler.Append($"DESKTOP({lisanslar.ProYetkiStart:yyyyMMdd}-{lisanslar.ProYetkiEnd:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.ProYetki = userx.LisansDurum?.ProYetki ?? false;
                                lisanslar.ProYetkiStart = userx.LisansDurum?.ProYetkiStart;
                                lisanslar.ProYetkiEnd = userx.LisansDurum?.ProYetkiEnd;

                                // X=0 + aktif → ED ay sonuna çek (kapatma event'i scheduler tarafından yazılacak)
                                if (LisansAktif(lisanslar.ProYetki, lisanslar.ProYetkiEnd) && lisanslar.ProYetkiEnd.Value.Date > aySonu)
                                {
                                    lisanslar.ProYetkiEnd = aySonu;
                                    guncellenenler.Append("DESKTOP(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // MOBIL / CepYetki
                            // ============================================================
                            if (dosyaUser.MOBIL)
                            {
                                lisanslar.CepYetki = true;
                                lisanslar.CepYetkiStart = dosyaUser.MOBIL_Start ?? defaultStart;
                                lisanslar.CepYetkiEnd = dosyaUser.MOBIL_End ?? defaultEnd;
                                if (userx.LisansDurum?.CepYetki != true) acilanlar.Append("CepYetki;");
                                guncellenenler.Append($"MOBIL({lisanslar.CepYetkiStart:yyyyMMdd}-{lisanslar.CepYetkiEnd:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.CepYetki = userx.LisansDurum?.CepYetki ?? false;
                                lisanslar.CepYetkiStart = userx.LisansDurum?.CepYetkiStart;
                                lisanslar.CepYetkiEnd = userx.LisansDurum?.CepYetkiEnd;

                                if (LisansAktif(lisanslar.CepYetki, lisanslar.CepYetkiEnd) && lisanslar.CepYetkiEnd.Value.Date > aySonu)
                                {
                                    lisanslar.CepYetkiEnd = aySonu;
                                    guncellenenler.Append("MOBIL(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // PD1P / PayLP (zincir başında PayL1'i de set eder)
                            // ============================================================
                            if (dosyaUser.PD1P)
                            {
                                lisanslar.PayLP = true;
                                lisanslar.PayL1 = true;
                                lisanslar.PayLPStart = dosyaUser.PD1P_Start ?? defaultStart;
                                lisanslar.PayLPEnd = dosyaUser.PD1P_End ?? defaultEnd;
                                lisanslar.PayL1Start = dosyaUser.PD1P_Start ?? defaultStart;
                                lisanslar.PayL1End = dosyaUser.PD1P_End ?? defaultEnd;
                                if (userx.LisansDurum?.PayLP != true) acilanlar.Append("PayLP;");
                                guncellenenler.Append($"PD1P({lisanslar.PayLPStart:yyyyMMdd}-{lisanslar.PayLPEnd:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.PayLP = userx.LisansDurum?.PayLP ?? false;
                                lisanslar.PayLPStart = userx.LisansDurum?.PayLPStart;
                                lisanslar.PayLPEnd = userx.LisansDurum?.PayLPEnd;

                                lisanslar.PayL1 = userx.LisansDurum?.PayL1 ?? false;
                                lisanslar.PayL1Start = userx.LisansDurum?.PayL1Start;
                                lisanslar.PayL1End = userx.LisansDurum?.PayL1End;

                                if (LisansAktif(lisanslar.PayLP, lisanslar.PayLPEnd) && lisanslar.PayLPEnd.Value.Date > aySonu)
                                {
                                    lisanslar.PayLPEnd = aySonu;
                                    guncellenenler.Append("PD1P(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // PD2 / PayL2
                            // ============================================================
                            if (dosyaUser.PD2)
                            {
                                lisanslar.PayL2 = true;
                                lisanslar.PayL2Start = dosyaUser.PD2_Start ?? defaultStart;
                                lisanslar.PayL2End = dosyaUser.PD2_End ?? defaultEnd;
                                if (userx.LisansDurum?.PayL2 != true) acilanlar.Append("PayL2;");
                                guncellenenler.Append($"PD2({lisanslar.PayL2Start:yyyyMMdd}-{lisanslar.PayL2End:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.PayL2 = userx.LisansDurum?.PayL2 ?? false;
                                lisanslar.PayL2Start = userx.LisansDurum?.PayL2Start;
                                lisanslar.PayL2End = userx.LisansDurum?.PayL2End;

                                if (LisansAktif(lisanslar.PayL2, lisanslar.PayL2End) && lisanslar.PayL2End.Value.Date > aySonu)
                                {
                                    lisanslar.PayL2End = aySonu;
                                    guncellenenler.Append("PD2(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // PD2P / Pd2P
                            // ============================================================
                            if (dosyaUser.PD2P)
                            {
                                lisanslar.Pd2P = true;
                                lisanslar.Pd2PStart = dosyaUser.PD2P_Start ?? defaultStart;
                                lisanslar.Pd2PEnd = dosyaUser.PD2P_End ?? defaultEnd;
                                if (userx.LisansDurum?.Pd2P != true) acilanlar.Append("Pd2P;");
                                guncellenenler.Append($"PD2P({lisanslar.Pd2PStart:yyyyMMdd}-{lisanslar.Pd2PEnd:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.Pd2P = userx.LisansDurum?.Pd2P ?? false;
                                lisanslar.Pd2PStart = userx.LisansDurum?.Pd2PStart;
                                lisanslar.Pd2PEnd = userx.LisansDurum?.Pd2PEnd;

                                if (LisansAktif(lisanslar.Pd2P, lisanslar.Pd2PEnd) && lisanslar.Pd2PEnd.Value.Date > aySonu)
                                {
                                    lisanslar.Pd2PEnd = aySonu;
                                    guncellenenler.Append("PD2P(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // END / PayX
                            // ============================================================
                            if (dosyaUser.END)
                            {
                                lisanslar.PayX = true;
                                lisanslar.PayXStart = dosyaUser.END_Start ?? defaultStart;
                                lisanslar.PayXEnd = dosyaUser.END_End ?? defaultEnd;
                                if (userx.LisansDurum?.PayX != true) acilanlar.Append("PayX;");
                                guncellenenler.Append($"END({lisanslar.PayXStart:yyyyMMdd}-{lisanslar.PayXEnd:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.PayX = userx.LisansDurum?.PayX ?? false;
                                lisanslar.PayXStart = userx.LisansDurum?.PayXStart;
                                lisanslar.PayXEnd = userx.LisansDurum?.PayXEnd;

                                if (LisansAktif(lisanslar.PayX, lisanslar.PayXEnd) && lisanslar.PayXEnd.Value.Date > aySonu)
                                {
                                    lisanslar.PayXEnd = aySonu;
                                    guncellenenler.Append("END(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // PIT / PayGS
                            // ============================================================
                            if (dosyaUser.PIT)
                            {
                                lisanslar.PayGS = true;
                                lisanslar.PayGSStart = dosyaUser.PIT_Start ?? defaultStart;
                                lisanslar.PayGSEnd = dosyaUser.PIT_End ?? defaultEnd;
                                if (userx.LisansDurum?.PayGS != true) acilanlar.Append("PayGS;");
                                guncellenenler.Append($"PIT({lisanslar.PayGSStart:yyyyMMdd}-{lisanslar.PayGSEnd:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.PayGS = userx.LisansDurum?.PayGS ?? false;
                                lisanslar.PayGSStart = userx.LisansDurum?.PayGSStart;
                                lisanslar.PayGSEnd = userx.LisansDurum?.PayGSEnd;

                                if (LisansAktif(lisanslar.PayGS, lisanslar.PayGSEnd) && lisanslar.PayGSEnd.Value.Date > aySonu)
                                {
                                    lisanslar.PayGSEnd = aySonu;
                                    guncellenenler.Append("PIT(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // PITE
                            // ============================================================
                            if (dosyaUser.PITE)
                            {
                                lisanslar.PITE = true;
                                lisanslar.PayPiteStart = dosyaUser.PITE_Start ?? defaultStart;
                                lisanslar.PayPiteEnd = dosyaUser.PITE_End ?? defaultEnd;
                                if (userx.LisansDurum?.PITE != true) acilanlar.Append("PITE;");
                                guncellenenler.Append($"PITE({lisanslar.PayPiteStart:yyyyMMdd}-{lisanslar.PayPiteEnd:yyyyMMdd}), ");

                                // PITE açıldığında PayGS de aynı süreye ayarlanır (mevcut iş kuralı)
                                if (!lisanslar.PayGS || !lisanslar.PayGSEnd.HasValue || lisanslar.PayGSEnd.Value < lisanslar.PayPiteEnd.Value)
                                {
                                    lisanslar.PayGS = true;
                                    lisanslar.PayGSStart = lisanslar.PayGSStart ?? lisanslar.PayPiteStart;
                                    lisanslar.PayGSEnd = lisanslar.PayPiteEnd;
                                    if (userx.LisansDurum?.PayGS != true && !acilanlar.ToString().Contains("PayGS"))
                                        acilanlar.Append("PayGS;");
                                }
                            }
                            else
                            {
                                lisanslar.PITE = userx.LisansDurum?.PITE ?? false;
                                lisanslar.PayPiteStart = userx.LisansDurum?.PayPiteStart;
                                lisanslar.PayPiteEnd = userx.LisansDurum?.PayPiteEnd;

                                if (LisansAktif(lisanslar.PITE, lisanslar.PayPiteEnd) && lisanslar.PayPiteEnd.Value.Date > aySonu)
                                {
                                    lisanslar.PayPiteEnd = aySonu;
                                    guncellenenler.Append("PITE(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // MKK
                            // ============================================================
                            if (dosyaUser.MKK)
                            {
                                lisanslar.MKK = true;
                                lisanslar.MKKStart = dosyaUser.MKK_Start ?? defaultStart;
                                lisanslar.MKKEnd = dosyaUser.MKK_End ?? defaultEnd;
                                if (userx.LisansDurum?.MKK != true) acilanlar.Append("MKK;");
                                guncellenenler.Append($"MKK({lisanslar.MKKStart:yyyyMMdd}-{lisanslar.MKKEnd:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.MKK = userx.LisansDurum?.MKK ?? false;
                                lisanslar.MKKStart = userx.LisansDurum?.MKKStart;
                                lisanslar.MKKEnd = userx.LisansDurum?.MKKEnd;

                                if (LisansAktif(lisanslar.MKK, lisanslar.MKKEnd) && lisanslar.MKKEnd.Value.Date > aySonu)
                                {
                                    lisanslar.MKKEnd = aySonu;
                                    guncellenenler.Append("MKK(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // GKKUL
                            // ============================================================
                            if (dosyaUser.GKKUL)
                            {
                                lisanslar.GKKUL = true;
                                lisanslar.GKKULStart = dosyaUser.GKKUL_Start ?? defaultStart;
                                lisanslar.GKKULEnd = dosyaUser.GKKUL_End ?? defaultEnd;
                                if (userx.LisansDurum?.GKKUL != true) acilanlar.Append("GKKUL;");
                                guncellenenler.Append($"GKKUL({lisanslar.GKKULStart:yyyyMMdd}-{lisanslar.GKKULEnd:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.GKKUL = userx.LisansDurum?.GKKUL ?? false;
                                lisanslar.GKKULStart = userx.LisansDurum?.GKKULStart;
                                lisanslar.GKKULEnd = userx.LisansDurum?.GKKULEnd;

                                if (LisansAktif(lisanslar.GKKUL, lisanslar.GKKULEnd) && lisanslar.GKKULEnd.Value.Date > aySonu)
                                {
                                    lisanslar.GKKULEnd = aySonu;
                                    guncellenenler.Append("GKKUL(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // VD1P / ViopLP (zincir başında ViopL1'i de set eder)
                            // ============================================================
                            if (dosyaUser.VD1P)
                            {
                                lisanslar.ViopLP = true;
                                lisanslar.ViopL1 = true;
                                lisanslar.ViopLPStart = dosyaUser.VD1P_Start ?? defaultStart;
                                lisanslar.ViopLPEnd = dosyaUser.VD1P_End ?? defaultEnd;
                                lisanslar.ViopL1Start = dosyaUser.VD1P_Start ?? defaultStart;
                                lisanslar.ViopL1End = dosyaUser.VD1P_End ?? defaultEnd;
                                if (userx.LisansDurum?.ViopLP != true) acilanlar.Append("ViopLP;");
                                guncellenenler.Append($"VD1P({lisanslar.ViopLPStart:yyyyMMdd}-{lisanslar.ViopLPEnd:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.ViopLP = userx.LisansDurum?.ViopLP ?? false;
                                lisanslar.ViopLPStart = userx.LisansDurum?.ViopLPStart;
                                lisanslar.ViopLPEnd = userx.LisansDurum?.ViopLPEnd;

                                lisanslar.ViopL1 = userx.LisansDurum?.ViopL1 ?? false;
                                lisanslar.ViopL1Start = userx.LisansDurum?.ViopL1Start;
                                lisanslar.ViopL1End = userx.LisansDurum?.ViopL1End;

                                if (LisansAktif(lisanslar.ViopLP, lisanslar.ViopLPEnd) && lisanslar.ViopLPEnd.Value.Date > aySonu)
                                {
                                    lisanslar.ViopLPEnd = aySonu;
                                    guncellenenler.Append("VD1P(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // VD2 / ViopL2
                            // ============================================================
                            if (dosyaUser.VD2)
                            {
                                lisanslar.ViopL2 = true;
                                lisanslar.ViopL2Start = dosyaUser.VD2_Start ?? defaultStart;
                                lisanslar.ViopL2End = dosyaUser.VD2_End ?? defaultEnd;
                                if (userx.LisansDurum?.ViopL2 != true) acilanlar.Append("ViopL2;");
                                guncellenenler.Append($"VD2({lisanslar.ViopL2Start:yyyyMMdd}-{lisanslar.ViopL2End:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.ViopL2 = userx.LisansDurum?.ViopL2 ?? false;
                                lisanslar.ViopL2Start = userx.LisansDurum?.ViopL2Start;
                                lisanslar.ViopL2End = userx.LisansDurum?.ViopL2End;

                                if (LisansAktif(lisanslar.ViopL2, lisanslar.ViopL2End) && lisanslar.ViopL2End.Value.Date > aySonu)
                                {
                                    lisanslar.ViopL2End = aySonu;
                                    guncellenenler.Append("VD2(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // VD2P / Vd2P
                            // ============================================================
                            if (dosyaUser.VD2P)
                            {
                                lisanslar.Vd2P = true;
                                lisanslar.Vd2PStart = dosyaUser.VD2P_Start ?? defaultStart;
                                lisanslar.Vd2PEnd = dosyaUser.VD2P_End ?? defaultEnd;
                                if (userx.LisansDurum?.Vd2P != true) acilanlar.Append("Vd2P;");
                                guncellenenler.Append($"VD2P({lisanslar.Vd2PStart:yyyyMMdd}-{lisanslar.Vd2PEnd:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.Vd2P = userx.LisansDurum?.Vd2P ?? false;
                                lisanslar.Vd2PStart = userx.LisansDurum?.Vd2PStart;
                                lisanslar.Vd2PEnd = userx.LisansDurum?.Vd2PEnd;

                                if (LisansAktif(lisanslar.Vd2P, lisanslar.Vd2PEnd) && lisanslar.Vd2PEnd.Value.Date > aySonu)
                                {
                                    lisanslar.Vd2PEnd = aySonu;
                                    guncellenenler.Append("VD2P(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // VIT / ViopGS
                            // ============================================================
                            if (dosyaUser.VIT)
                            {
                                lisanslar.ViopGS = true;
                                lisanslar.ViopGSStart = dosyaUser.VIT_Start ?? defaultStart;
                                lisanslar.ViopGSEnd = dosyaUser.VIT_End ?? defaultEnd;
                                if (userx.LisansDurum?.ViopGS != true) acilanlar.Append("ViopGS;");
                                guncellenenler.Append($"VIT({lisanslar.ViopGSStart:yyyyMMdd}-{lisanslar.ViopGSEnd:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.ViopGS = userx.LisansDurum?.ViopGS ?? false;
                                lisanslar.ViopGSStart = userx.LisansDurum?.ViopGSStart;
                                lisanslar.ViopGSEnd = userx.LisansDurum?.ViopGSEnd;

                                if (LisansAktif(lisanslar.ViopGS, lisanslar.ViopGSEnd) && lisanslar.ViopGSEnd.Value.Date > aySonu)
                                {
                                    lisanslar.ViopGSEnd = aySonu;
                                    guncellenenler.Append("VIT(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // BD1P / TahvilLP (zincir başında TahvilL1'i de set eder)
                            // ============================================================
                            if (dosyaUser.BD1P)
                            {
                                lisanslar.TahvilLP = true;
                                lisanslar.TahvilL1 = true;
                                lisanslar.TahvilLPStart = dosyaUser.BD1P_Start ?? defaultStart;
                                lisanslar.TahvilLPEnd = dosyaUser.BD1P_End ?? defaultEnd;
                                lisanslar.TahvilL1Start = dosyaUser.BD1P_Start ?? defaultStart;
                                lisanslar.TahvilL1End = dosyaUser.BD1P_End ?? defaultEnd;
                                if (userx.LisansDurum?.TahvilLP != true) acilanlar.Append("TahvilLP;");
                                guncellenenler.Append($"BD1P({lisanslar.TahvilLPStart:yyyyMMdd}-{lisanslar.TahvilLPEnd:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.TahvilLP = userx.LisansDurum?.TahvilLP ?? false;
                                lisanslar.TahvilLPStart = userx.LisansDurum?.TahvilLPStart;
                                lisanslar.TahvilLPEnd = userx.LisansDurum?.TahvilLPEnd;

                                lisanslar.TahvilL1 = userx.LisansDurum?.TahvilL1 ?? false;
                                lisanslar.TahvilL1Start = userx.LisansDurum?.TahvilL1Start;
                                lisanslar.TahvilL1End = userx.LisansDurum?.TahvilL1End;

                                if (LisansAktif(lisanslar.TahvilLP, lisanslar.TahvilLPEnd) && lisanslar.TahvilLPEnd.Value.Date > aySonu)
                                {
                                    lisanslar.TahvilLPEnd = aySonu;
                                    guncellenenler.Append("BD1P(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // BD2 / TahvilL2
                            // ============================================================
                            if (dosyaUser.BD2)
                            {
                                lisanslar.TahvilL2 = true;
                                lisanslar.TahvilL2Start = dosyaUser.BD2_Start ?? defaultStart;
                                lisanslar.TahvilL2End = dosyaUser.BD2_End ?? defaultEnd;
                                if (userx.LisansDurum?.TahvilL2 != true) acilanlar.Append("TahvilL2;");
                                guncellenenler.Append($"BD2({lisanslar.TahvilL2Start:yyyyMMdd}-{lisanslar.TahvilL2End:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.TahvilL2 = userx.LisansDurum?.TahvilL2 ?? false;
                                lisanslar.TahvilL2Start = userx.LisansDurum?.TahvilL2Start;
                                lisanslar.TahvilL2End = userx.LisansDurum?.TahvilL2End;

                                if (LisansAktif(lisanslar.TahvilL2, lisanslar.TahvilL2End) && lisanslar.TahvilL2End.Value.Date > aySonu)
                                {
                                    lisanslar.TahvilL2End = aySonu;
                                    guncellenenler.Append("BD2(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // KRMD1 / COMEX
                            // ============================================================
                            if (dosyaUser.KRMD1)
                            {
                                lisanslar.COMEX = true;
                                lisanslar.KRMD1Start = dosyaUser.KRMD1_Start ?? defaultStart;
                                lisanslar.KRMD1End = dosyaUser.KRMD1_End ?? defaultEnd;
                                if (userx.LisansDurum?.COMEX != true) acilanlar.Append("KRMD1;");
                                guncellenenler.Append($"KRMD1({lisanslar.KRMD1Start:yyyyMMdd}-{lisanslar.KRMD1End:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.COMEX = userx.LisansDurum?.COMEX ?? false;
                                lisanslar.KRMD1Start = userx.LisansDurum?.KRMD1Start;
                                lisanslar.KRMD1End = userx.LisansDurum?.KRMD1End;

                                if (LisansAktif(lisanslar.COMEX, lisanslar.KRMD1End) && lisanslar.KRMD1End.Value.Date > aySonu)
                                {
                                    lisanslar.KRMD1End = aySonu;
                                    guncellenenler.Append("KRMD1(ED ay sonuna çekildi), ");
                                }
                            }

                            // ============================================================
                            // CME / ALG
                            // ============================================================
                            if (dosyaUser.CME)
                            {
                                lisanslar.CME = true;
                                lisanslar.CMEStart = dosyaUser.CME_Start ?? defaultStart;
                                lisanslar.CMEEnd = dosyaUser.CME_End ?? defaultEnd;
                                if (userx.LisansDurum?.CME != true) acilanlar.Append("ALG;");
                                guncellenenler.Append($"ALG({lisanslar.CMEStart:yyyyMMdd}-{lisanslar.CMEEnd:yyyyMMdd}), ");
                            }
                            else
                            {
                                lisanslar.CME = userx.LisansDurum?.CME ?? false;
                                lisanslar.CMEStart = userx.LisansDurum?.CMEStart;
                                lisanslar.CMEEnd = userx.LisansDurum?.CMEEnd;

                                if (LisansAktif(lisanslar.CME, lisanslar.CMEEnd) && lisanslar.CMEEnd.Value.Date > aySonu)
                                {
                                    lisanslar.CMEEnd = aySonu;
                                    guncellenenler.Append("ALG(ED ay sonuna çekildi), ");
                                }
                            }

                            // SPI ve ROBOT alanları korunur
                            lisanslar.SPI = userx.LisansDurum?.SPI ?? false;
                            lisanslar.ROBOT = userx.LisansDurum?.ROBOT ?? false;

                            // ============================================================
                            // ZİNCİR TUTARLILIK KONTROLÜ
                            // Kural: Üst kademe açıksa altındaki tüm kademeler de en az
                            //        üst kademe kadar açık olmalı.
                            // Pay zinciri:    Pd2P → PayL2 → PayLP → PayL1
                            // Viop zinciri:   Vd2P → ViopL2 → ViopLP → ViopL1
                            // Tahvil zinciri: TahvilL2 → TahvilLP → TahvilL1
                            // ============================================================

                            // --- Pay zinciri: Pd2P → PayL2 ---
                            if (lisanslar.Pd2P && lisanslar.Pd2PEnd.HasValue)
                            {
                                if (!lisanslar.PayL2 || !lisanslar.PayL2End.HasValue || lisanslar.PayL2End.Value < lisanslar.Pd2PEnd.Value)
                                {
                                    bool oncedenAcik = lisanslar.PayL2;
                                    lisanslar.PayL2 = true;
                                    lisanslar.PayL2Start = lisanslar.PayL2Start ?? defaultStart;
                                    lisanslar.PayL2End = lisanslar.Pd2PEnd;
                                    if (!oncedenAcik && userx.LisansDurum?.PayL2 != true && !acilanlar.ToString().Contains("PayL2;"))
                                        acilanlar.Append("PayL2;");
                                }
                            }

                            // --- Pay zinciri: PayL2 → PayLP ---
                            if (lisanslar.PayL2 && lisanslar.PayL2End.HasValue)
                            {
                                if (!lisanslar.PayLP || !lisanslar.PayLPEnd.HasValue || lisanslar.PayLPEnd.Value < lisanslar.PayL2End.Value)
                                {
                                    bool oncedenAcik = lisanslar.PayLP;
                                    lisanslar.PayLP = true;
                                    lisanslar.PayLPStart = lisanslar.PayLPStart ?? defaultStart;
                                    lisanslar.PayLPEnd = lisanslar.PayL2End;
                                    if (!oncedenAcik && userx.LisansDurum?.PayLP != true && !acilanlar.ToString().Contains("PayLP;"))
                                        acilanlar.Append("PayLP;");
                                }
                            }

                            // --- Pay zinciri: PayLP → PayL1 ---
                            if (lisanslar.PayLP && lisanslar.PayLPEnd.HasValue)
                            {
                                if (!lisanslar.PayL1 || !lisanslar.PayL1End.HasValue || lisanslar.PayL1End.Value < lisanslar.PayLPEnd.Value)
                                {
                                    lisanslar.PayL1 = true;
                                    lisanslar.PayL1Start = lisanslar.PayL1Start ?? defaultStart;
                                    lisanslar.PayL1End = lisanslar.PayLPEnd;
                                }
                            }

                            // --- Viop zinciri: Vd2P → ViopL2 ---
                            if (lisanslar.Vd2P && lisanslar.Vd2PEnd.HasValue)
                            {
                                if (!lisanslar.ViopL2 || !lisanslar.ViopL2End.HasValue || lisanslar.ViopL2End.Value < lisanslar.Vd2PEnd.Value)
                                {
                                    bool oncedenAcik = lisanslar.ViopL2;
                                    lisanslar.ViopL2 = true;
                                    lisanslar.ViopL2Start = lisanslar.ViopL2Start ?? defaultStart;
                                    lisanslar.ViopL2End = lisanslar.Vd2PEnd;
                                    if (!oncedenAcik && userx.LisansDurum?.ViopL2 != true && !acilanlar.ToString().Contains("ViopL2;"))
                                        acilanlar.Append("ViopL2;");
                                }
                            }

                            // --- Viop zinciri: ViopL2 → ViopLP ---
                            if (lisanslar.ViopL2 && lisanslar.ViopL2End.HasValue)
                            {
                                if (!lisanslar.ViopLP || !lisanslar.ViopLPEnd.HasValue || lisanslar.ViopLPEnd.Value < lisanslar.ViopL2End.Value)
                                {
                                    bool oncedenAcik = lisanslar.ViopLP;
                                    lisanslar.ViopLP = true;
                                    lisanslar.ViopLPStart = lisanslar.ViopLPStart ?? defaultStart;
                                    lisanslar.ViopLPEnd = lisanslar.ViopL2End;
                                    if (!oncedenAcik && userx.LisansDurum?.ViopLP != true && !acilanlar.ToString().Contains("ViopLP;"))
                                        acilanlar.Append("ViopLP;");
                                }
                            }

                            // --- Viop zinciri: ViopLP → ViopL1 ---
                            if (lisanslar.ViopLP && lisanslar.ViopLPEnd.HasValue)
                            {
                                if (!lisanslar.ViopL1 || !lisanslar.ViopL1End.HasValue || lisanslar.ViopL1End.Value < lisanslar.ViopLPEnd.Value)
                                {
                                    lisanslar.ViopL1 = true;
                                    lisanslar.ViopL1Start = lisanslar.ViopL1Start ?? defaultStart;
                                    lisanslar.ViopL1End = lisanslar.ViopLPEnd;
                                }
                            }

                            // --- Tahvil zinciri: TahvilL2 → TahvilLP ---
                            if (lisanslar.TahvilL2 && lisanslar.TahvilL2End.HasValue)
                            {
                                if (!lisanslar.TahvilLP || !lisanslar.TahvilLPEnd.HasValue || lisanslar.TahvilLPEnd.Value < lisanslar.TahvilL2End.Value)
                                {
                                    bool oncedenAcik = lisanslar.TahvilLP;
                                    lisanslar.TahvilLP = true;
                                    lisanslar.TahvilLPStart = lisanslar.TahvilLPStart ?? defaultStart;
                                    lisanslar.TahvilLPEnd = lisanslar.TahvilL2End;
                                    if (!oncedenAcik && userx.LisansDurum?.TahvilLP != true && !acilanlar.ToString().Contains("TahvilLP;"))
                                        acilanlar.Append("TahvilLP;");
                                }
                            }

                            // --- Tahvil zinciri: TahvilLP → TahvilL1 ---
                            if (lisanslar.TahvilLP && lisanslar.TahvilLPEnd.HasValue)
                            {
                                if (!lisanslar.TahvilL1 || !lisanslar.TahvilL1End.HasValue || lisanslar.TahvilL1End.Value < lisanslar.TahvilLPEnd.Value)
                                {
                                    lisanslar.TahvilL1 = true;
                                    lisanslar.TahvilL1Start = lisanslar.TahvilL1Start ?? defaultStart;
                                    lisanslar.TahvilL1End = lisanslar.TahvilLPEnd;
                                }
                            }

                            // ============================================================
                            // Event Type Belirleme
                            // ============================================================
                            if (acilanlar.ToString() != "")
                            {
                                ue.EventTypeId = 4; // Lisans Ekleme
                                ue.AcilanLisans = acilanlar.ToString();
                            }
                            else
                            {
                                ue.EventTypeId = 6; // Lisans Değiştirme (sadece tarih güncellemesi veya ED ay sonuna çekme)
                                ue.AcilanLisans = "LisansTarihGuncelleme";
                            }

                            // ============================================================
                            // DB Kayıt
                            // ============================================================
                            crm.LisansDurums.InsertOnSubmit(lisanslar);
                            crm.SubmitChanges();
                            userx.LisansDurum = lisanslar;
                            crm.SubmitChanges();

                            ue.SonLisandurumID = lisanslar.LisansDurumId;
                            if (ue.EventId < 1)
                                crm.UserEvents.InsertOnSubmit(ue);
                            crm.SubmitChanges();

                            // ============================================================
                            // Server'a Bildir
                            // ============================================================
                            if (!chbLisansTarihOnlySendDb.Checked)
                            {
                                formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + userx.UserID.ToString() + (char)3;
                            }

                            string guncellenenAlanlar = guncellenenler.ToString().TrimEnd(' ', ',');
                            if (!string.IsNullOrEmpty(guncellenenAlanlar))
                            {
                                durumYaz($"[OK] {dosyaUser.Username} - Güncellenen: {guncellenenAlanlar}");
                                LogDosyadanIslem(
                                    operasyon: "LisansTarihGuncelle",
                                    tckn: dosyaUser.Username,
                                    calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
                                               formAdminAra.referance.ActiveCalisan.Soyad,
                                    ip: formAdminAra.referance.IpAdress,
                                    hostname: formAdminAra.referance.HostName,
                                    degisiklikler: guncellenenAlanlar
                                );
                            }
                            else
                            {
                                durumYaz($"[OK] {dosyaUser.Username} - Lisans tarihleri korundu");
                            }

                            basariliSayisi++;
                        }
                        catch (Exception ex)
                        {
                            hataliSayisi++;
                            durumYaz($"[HATA] {dosyaUser.Username} - {ex.Message}");
                            MyTools.logyaz($"LisansTarih güncelleme hatası - TCKN: {dosyaUser.Username}, Hata: {ex.Message}");

                            LogDosyadanIslem(
                                operasyon: "LisansTarihGuncelle",
                                tckn: dosyaUser.Username,
                                calisanAd: formAdminAra.referance.ActiveCalisan.Ad + " " +
                                           formAdminAra.referance.ActiveCalisan.Soyad,
                                ip: formAdminAra.referance.IpAdress,
                                hostname: formAdminAra.referance.HostName,
                                degisiklikler: null,
                                basarili: false,
                                hata: ex.Message
                            );
                        }

                        LabelYaz(lblLisansTarihGonderimDurum, say + " / " + count);
                        say++;
                    }

                    durumYaz($"[ÖZET] Toplam: {count}, Başarılı: {basariliSayisi}, Bulunamayan: {bulunamayanSayisi}, Hatalı: {hataliSayisi}");

                    LabelYaz(lblLisansTarihGonderimDurum, "Bitti");
                });
            }
            catch (Exception ex)
            {
                durumYaz($"[KRİTİK HATA] {ex.Message}");
                MyTools.logyaz($"LisansTarih toplu güncelleme kritik hatası: {ex.Message}");
            }
        }

        public static void LogDosyadanIslem(
     string operasyon, string tckn, string calisanAd,
     string ip, string hostname, string degisiklikler,
     bool basarili = true, string hata = null)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"[DOSYADAN İŞLEM] [{operasyon}]");
                sb.AppendLine($"  Kullanıcı  : {tckn}");
                sb.AppendLine($"  Yapan      : {calisanAd}");
                sb.AppendLine($"  IP/Host    : {ip} / {hostname}");
                sb.AppendLine($"  Sonuç      : {(basarili ? "BAŞARILI" : "HATA")}");
                if (!string.IsNullOrEmpty(degisiklikler))
                    sb.AppendLine($"  Değişiklik : {degisiklikler}");
                if (!string.IsNullOrEmpty(hata))
                    sb.AppendLine($"  Hata       : {hata}");

                var satirSb = new StringBuilder();
                satirSb.AppendLine("\n---------------------------------------------" +
                    DateTime.Now.ToLongTimeString() +
                    "----------------------------------------------- ");
                satirSb.AppendLine(sb.ToString());


                lock (_logLock)
                {
                    var yol = System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(
                            System.Reflection.Assembly.GetExecutingAssembly().Location),
                        "LOG");

                    if (!System.IO.Directory.Exists(yol))
                        System.IO.Directory.CreateDirectory(yol);

                    var dosya = System.IO.Path.Combine(yol,
                        DateTime.Today.ToString("yyyyMMdd") + ".txt");

                    System.IO.File.AppendAllText(dosya, satirSb.ToString(), Encoding.UTF8);
                }
            }
            catch { }
        }
    }

    public class DosyadanYeniUser
    {
        public string Username = "";
        public string Tckn = "";
        public string Sifre = "";
        public string Ad = "";
        public string Soyad = "";
        public string Tel = "";
        public string Adres = "";
        public string Sehir = "";
        public string mail = "";
        public string Ulke = "";
        public string PmtsNo = "";

        public DateTime expirydate = new DateTime();

        public bool DESKTOP = false;
        public bool MOBIL = false;
        public bool PD1 = false;
        public bool PD1P = false;
        public bool PD2 = false;
        public bool PD2P = false;
        public bool END = false;
        public bool PIT = false;
        public bool PITE = false;
        public bool MKK = false;
        public bool GKKUL = false;
        public bool VD1 = false;
        public bool VD1P = false;
        public bool VD2 = false;
        public bool VD2P = false;
        public bool VIT = false;
        public bool BD1 = false;
        public bool BD1P = false;
        public bool BD2 = false;
        public bool KRMD1 = false;
        public bool CME = false;
        public bool PAKET10 = false;


        public DateTime? DESKTOP_Start = null;
        public DateTime? DESKTOP_End = null;
        public DateTime? MOBIL_Start = null;
        public DateTime? MOBIL_End = null;
        public DateTime? PD1P_Start = null;
        public DateTime? PD1P_End = null;
        public DateTime? PD2_Start = null;
        public DateTime? PD2_End = null;
        public DateTime? PD2P_Start = null;
        public DateTime? PD2P_End = null;
        public DateTime? END_Start = null;
        public DateTime? END_End = null;
        public DateTime? PIT_Start = null;
        public DateTime? PIT_End = null;
        public DateTime? PITE_Start = null;
        public DateTime? PITE_End = null;
        public DateTime? MKK_Start = null;
        public DateTime? MKK_End = null;
        public DateTime? GKKUL_Start = null;
        public DateTime? GKKUL_End = null;
        public DateTime? VD1P_Start = null;
        public DateTime? VD1P_End = null;
        public DateTime? VD2_Start = null;
        public DateTime? VD2_End = null;
        public DateTime? VD2P_Start = null;
        public DateTime? VD2P_End = null;
        public DateTime? VIT_Start = null;
        public DateTime? VIT_End = null;
        public DateTime? BD1P_Start = null;
        public DateTime? BD1P_End = null;
        public DateTime? BD2_Start = null;
        public DateTime? BD2_End = null;
        public DateTime? KRMD1_Start = null;
        public DateTime? KRMD1_End = null;
        public DateTime? CME_Start = null;
        public DateTime? CME_End = null;

    }

}
