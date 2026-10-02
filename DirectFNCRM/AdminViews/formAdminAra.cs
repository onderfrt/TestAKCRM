using DirectFNCRM.ServerViews;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectFNCRM.AdminViews
{
    public partial class formAdminAra : Form
    {
        private string _otmIniPath => System.Windows.Forms.Application.StartupPath + @"\ClientAyarlar.ini";
        private const string OtmIniSection = "OtomatikBorsaRapor";
        private const string OtmIniKeyPeriyot = "Periyot";
        private const string OtmIniKeyMailListesi = "MailListesi";
        private const string OtmIniKeyLastRun = "LastRun"; // yyyy-MM-dd for GUNLUK; yyyy-MM for AYSONU
        
        public formAdminAra()
        {
            InitializeComponent();
            dataGridView1._DoubleBuffer(true);
            gridLisanDurumOzet.Rows.Add(9);
            gridLisanDurumOzet._DoubleBuffer(true);
            referance = this;
            //gridLisanDurumOzet.Rows.Add(8);
            //referance = this;

        }
        delegate void UpdateGridThreadHandler(IQueryable<User> Sorgu);
        public bool acilis = false;
        public Calisan ActiveCalisan = new Calisan();
        public static formAdminAra referance;
        public static List<User> DataList = new List<User>();
        public static IQueryable<User> ActiveSorgu;
        public bool toplamguncelle = false;
        public string HostName = "";
        public string IpAdress = "";
        public delegate void ListBoxTransactionGuncelle(string Text);
        public delegate void gridviewOzetGuncelle(string Text);

        private void formAdminAra_Load(object sender, EventArgs e)
        {

            try
            {

                this.dataGridView1.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView1_RowPrePaint);
                ActiveCalisan = (Calisan)this.Tag;
                this.Text += this.Text += " - " + ActiveCalisan.Ad + " " + ActiveCalisan.Soyad + " - Ver : " + MyTools.KurumVersiyon + "  " + MyTools.KurumAd + "  CRM";
                #region LocalNetworkInfo

                HostName = Dns.GetHostName();
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    var addr = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();
                    if (addr != null)
                    {
                        if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {
                            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    IpAdress = ip.Address.ToString();
                                }
                            }
                        }
                    }
                }
                #endregion
                IPport.Config("InBufferSize=10000000");
                IPport.Config("OutBufferSize=10000000");
                IPport.Config("MaxLineLength=50000");

                if (MyTools.KurumKod == "14999" || MyTools.KurumKod == "10155")
                {
                    musteriTaleplerMenuItem.Visible = true;
                }
                else
                {
                    musteriTaleplerMenuItem.Visible = false;
                }


                if (MyTools.HisseSinyal == "1")
                {
                    hisseSinyaluserMenuItem.Visible = true;
                }
                else
                {
                    hisseSinyaluserMenuItem.Visible = false;
                }
                //hisse sinyal diğer

                //    string DBConnectionString = DirectFNCRM.Properties.Settings.Default.DBConnectionString;

                //    if (DBConnectionString == null)
                //    {
                //        MessageBox.Show("DBConnectionString string yazmadınız!");
                //        this.Close();
                //        hisseSinyaluserMenuItem.Visible = true;

                //    }
                //if (MyTools.HisseSinyal == "1")
                //{
                //    string HisseSinyalConnectionString = DirectFNCRM.Properties.Settings.Default.HisseSinyalConnectionString;

                //    if (HisseSinyalConnectionString == null)
                //    {
                //        MessageBox.Show("HisseSinyalConnectionString yazmadınız!");
                //    }

                //}


                StringBuilder okunan = new StringBuilder(100);
                MyTools.GetPrivateProfileString("Connection", "ServerIP", "", okunan, 100, System.Windows.Forms.Application.StartupPath + @"\ClientAyarlar.ini");
                if (okunan.ToString() == "")
                {
                    IPport.RemoteHost = "127.0.0.1";

                }
                else
                    IPport.RemoteHost = okunan.ToString().Trim();

                MyTools.GetPrivateProfileString("Connection", "ServerPort", "", okunan, 100, System.Windows.Forms.Application.StartupPath + @"\ClientAyarlar.ini");

                if (okunan.ToString() == "")
                {
                    IPport.RemotePort = 4450;

                }
                else
                    IPport.RemotePort = Int32.Parse(okunan.ToString().Trim());



                IPport.Connected = true;



                crmDFNDataContext crm = new crmDFNDataContext();


                #region LisansFiyatlariniSqldenOku


                var lisanlar = crm.LisansFiyatlaris.ToList();

                foreach (var lfiyat in lisanlar)
                {
                    switch (lfiyat.LisansKod)
                    {
                        case "PD1":
                            MyTools.lisansfiyatlari.Fpd1 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypd1 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "PD1P":
                            MyTools.lisansfiyatlari.Fpd1p = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypd1p = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "PD2":
                            MyTools.lisansfiyatlari.Fpd2 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypd2 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "PD2P":
                            MyTools.lisansfiyatlari.Fpd2p = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypd2p = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "END":
                            MyTools.lisansfiyatlari.Fend = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Yend = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "PIT":
                            MyTools.lisansfiyatlari.Fpit = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypit = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "PITE":
                            MyTools.lisansfiyatlari.Fpite = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypite = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "PVA":
                            MyTools.lisansfiyatlari.Fpva = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypva = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "VD1":
                            MyTools.lisansfiyatlari.Fvl1 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Yvl1 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "VD1P":
                            MyTools.lisansfiyatlari.Fvl1p = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Yvl1p = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "VD2":
                            MyTools.lisansfiyatlari.Fvl2 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Yvl2 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "VD2P":
                            MyTools.lisansfiyatlari.Fvl2p = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Yvl2p = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "VIT":
                            MyTools.lisansfiyatlari.Fvit = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Yvit = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "KRMD1":
                            MyTools.lisansfiyatlari.Fkrmd1 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ykrmd1 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "BD1":
                            MyTools.lisansfiyatlari.Fbd1 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ybd1 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "BD1P":
                            MyTools.lisansfiyatlari.Fbd1p = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ybd1p = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "BD2":
                            MyTools.lisansfiyatlari.Fbd2 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ybd2 = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "Analiz Pro":
                            MyTools.lisansfiyatlari.FanPro = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.FanPro = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "SENTIL1":
                            MyTools.lisansfiyatlari.FSentiL1 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YSentiL1 = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "SENTIL2":
                            MyTools.lisansfiyatlari.FSentiL2 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YSentiL2 = (decimal)lfiyat.YurtDisiFiyat;
                            break;


                        case "karma1k":
                            MyTools.lisansfiyatlari.karma1k = (decimal)lfiyat.Fiyat;
                            break;
                        case "karma5k":
                            MyTools.lisansfiyatlari.karma5k = (decimal)lfiyat.Fiyat;
                            break;
                        case "karma10k":
                            MyTools.lisansfiyatlari.karma10k = (decimal)lfiyat.Fiyat;
                            break;
                        case "karma20k":
                            MyTools.lisansfiyatlari.karma20k = (decimal)lfiyat.Fiyat;
                            break;
                        case "karma50k":
                            MyTools.lisansfiyatlari.karma50k = (decimal)lfiyat.Fiyat;
                            break;
                        case "karma100k":
                            MyTools.lisansfiyatlari.karma100k = (decimal)lfiyat.Fiyat;
                            break;
                        case "karmaSINIRSIZ":
                            MyTools.lisansfiyatlari.karmaSINIRSIZ = (decimal)lfiyat.Fiyat;
                            break;

                        case "GKULKYD":
                            MyTools.lisansfiyatlari.GKULKYD = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YGKULKYD = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "GKULEND":
                            MyTools.lisansfiyatlari.GKULEND = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YGKULEND = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "GKULD1P":
                            MyTools.lisansfiyatlari.GKULD1P = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YGKULD1P = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "GKULD2":
                            MyTools.lisansfiyatlari.GKULD2 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YGKULD2 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "GKULPITE":
                            MyTools.lisansfiyatlari.GKULPITE = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YGKULPITE = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "GKULPVA":
                            MyTools.lisansfiyatlari.GKULPVA = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YGKULPVA = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "GUYEKYD":
                            MyTools.lisansfiyatlari.GUYEKYD = (decimal)lfiyat.Fiyat;
                            break;

                        case "GUYEEND":
                            MyTools.lisansfiyatlari.GUYEEND = (decimal)lfiyat.Fiyat;
                            break;

                        case "GUYED1P":
                            MyTools.lisansfiyatlari.GUYED1P = (decimal)lfiyat.Fiyat;
                            break;

                        case "GUYED2":
                            MyTools.lisansfiyatlari.GUYED2 = (decimal)lfiyat.Fiyat;
                            break;

                        case "GUYEPVA":
                            MyTools.lisansfiyatlari.GUYEPVA = (decimal)lfiyat.Fiyat;
                            break;

                        case "GUYEPITE":
                            MyTools.lisansfiyatlari.GUYEPITE = (decimal)lfiyat.Fiyat;
                            break;

                        case "Spot Paket":
                            MyTools.lisansfiyatlari.SpotNonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.SpotPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "DJI":
                            MyTools.lisansfiyatlari.DJINonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.DJIPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "SPX":
                            MyTools.lisansfiyatlari.SPINonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.SPIPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "XETRA":
                            MyTools.lisansfiyatlari.XETRANonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.XETRAPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "CMEM":
                            MyTools.lisansfiyatlari.CMEMNonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.CMEMPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "CBOTM":
                            MyTools.lisansfiyatlari.CBOTMNonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.CBOTMPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "CBOT":
                            MyTools.lisansfiyatlari.CBOTNonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.CBOTPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "CME":
                            MyTools.lisansfiyatlari.CMENonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.CMEPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "EUREX":
                            MyTools.lisansfiyatlari.EUREXNonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.EUREXPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;

                        case "TARAMA":
                            MyTools.lisansfiyatlari.Ftarama = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ytarama = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "MKK":
                            MyTools.lisansfiyatlari.Fmkk = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ymkk = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "GKKUL":
                            MyTools.lisansfiyatlari.Fgkkul = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ygkkul = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        default:
                            break;

                    }

                }

                #endregion

                toplamguncelle = false;

                ara();

                HbControl.Start();




                comboStatus.DataSource = crm.Status;
                comboStatus.DisplayMember = "StatusAdi";
                comboStatus.ValueMember = "StatusId";
                comboStatus.SelectedIndex = -1;


                comboLisans.SelectedIndex = -1;

                if (ActiveCalisan.Yetki.YetkiAdi.ToLower() != "admin")
                {
                    btnNew.Visible = false;
                    ContexMenuAnaSecileniGonder.Visible = false;
                    ContexMenuAnaTumunuGonder.Visible = false;
                    calisanlarMenuItem.Visible = false;
                    MenuYetki.Visible = false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private static bool _otmJobRunning = false;

        private async void TimerOtomatikBorsaRaporu_Tick(object sender, EventArgs e)
        {
            try
            {
                var now = DateTime.Now;
                // Saat 14:30'da çalışacak şekilde ayarlandı
                if (now.Hour != 23 || now.Minute != 30) return;

                var periyot = MyTools.AyarOku(OtmIniSection, OtmIniKeyPeriyot, 64, _otmIniPath);
                if (string.IsNullOrWhiteSpace(periyot)) periyot = "NONE";

                // Prevent multiple runs in the same day/month
                var lastRun = MyTools.AyarOku(OtmIniSection, OtmIniKeyLastRun, 32, _otmIniPath);

                if (periyot.Equals("GUNLUK", StringComparison.OrdinalIgnoreCase))
                {
                    var todayKey = now.ToString("yyyy-MM-dd");
                    // Test için lastRun kontrolünü geçici olarak devre dışı bırak
                    // if (lastRun == todayKey) return;
                    if (_otmJobRunning) return; // already running
                    _otmJobRunning = true;
                    await TriggerOtomatikBorsaRapor();
                    MyTools.AyarYaz(OtmIniSection, OtmIniKeyLastRun, todayKey, _otmIniPath);
                }
                else if (periyot.Equals("AYSONU", StringComparison.OrdinalIgnoreCase))
                {
                    if (now.Date != now._LastDayOfMonth()) return; // only on month-end
                    var monthKey = now.ToString("yyyy-MM");
                    if (lastRun == monthKey) return;
                    await TriggerOtomatikBorsaRapor();
                    MyTools.AyarYaz(OtmIniSection, OtmIniKeyLastRun, monthKey, _otmIniPath);
                }
                else
                {
                    // NONE: do nothing
                }
            }
            catch (Exception ex)
            {
                try { MyTools.logyaz("Hata;TimerOtomatikBorsaRaporu_Tick;" + ex.Message); } catch { }
            }
            finally
            {
                _otmJobRunning = false;
            }
        }
       
        private async Task TriggerOtomatikBorsaRapor()
        {
            try
            {
                var now = DateTime.Now;
                var tarihText = now.ToString("dd.MM.yyyy");
                var saatText = now.ToString("HHmm");
                var description = "";
                var isSuccess = false;
                string raporKlasoru = string.Empty;
                // Bu çalıştırmada üretilen hedef dosya yollarını sakla
                string hedefCsvPath = string.Empty;
                string hedefXlsxPath = string.Empty;
                try
                {
                    // İstenen iki metot için birebir kopya isimlerle tetikleme (ExcelRutin içinde)
                    try
                    {
                        await MyTools.ExcelRutin.AutoBorsaListeYeni();
                        try { MyTools.logyaz("AutoBorsaListeYeni başarıyla tamamlandı"); } catch { }
                    }
                    catch (Exception ex1)
                    {
                        try { MyTools.logyaz("AutoBorsaListeYeni hatası: " + ex1.Message); } catch { }
                    }

                    try
                    {
                        await MyTools.ExcelRutin.AutoBorsaTamListe();
                        try { MyTools.logyaz("AutoBorsaTamListe başarıyla tamamlandı"); } catch { }
                    }
                    catch (Exception ex2)
                    {
                        try { MyTools.logyaz("AutoBorsaTamListe hatası: " + ex2.Message); } catch { }
                    }

                    // Rapor çıktılarını tarih klasörüne taşı

                    raporKlasoru = System.Windows.Forms.Application.StartupPath + @"\OtomatikRaporlar\" + tarihText;
                    try { if (!Directory.Exists(raporKlasoru)) Directory.CreateDirectory(raporKlasoru); } catch { }

                    // Kaynak dosya adlarını orijinal metotların oluşturduğu kalıba göre belirle
                    var kaynakCsv = System.Windows.Forms.Application.StartupPath + @"\" + "BorsaListeYeni" + now.Month.ToString() + now.Year.ToString() + ".CSV";
                    var hedefCsv = Path.Combine(raporKlasoru, $"BorsaListeYeni_{now:ddMMyyyy}_{saatText}.csv");
                    hedefCsvPath = hedefCsv;
                    // CSV üretimi bazen FS senkronizasyonu nedeniyle gecikebilir: kısa bir retry/poll uygula
                    try
                    {
                        int tryCount = 0;
                        while (!File.Exists(kaynakCsv) && tryCount < 10)
                        {
                            System.Threading.Thread.Sleep(300);
                            tryCount++;
                        }
                    }
                    catch { }
                    try { MyTools.logyaz("CSV dosyası aranıyor: " + kaynakCsv + " - Var mı: " + File.Exists(kaynakCsv)); } catch { }
                    if (File.Exists(kaynakCsv))
                    {
                        try { if (File.Exists(hedefCsv)) File.Delete(hedefCsv); } catch { }
                        try { File.Copy(kaynakCsv, hedefCsv, true); } catch { }
                        try { File.Delete(kaynakCsv); } catch { }
                        try { MyTools.logyaz("CSV dosyası kopyalandı: " + hedefCsv); } catch { }
                    }
                    else
                    {
                        // Fallback: isim beklediğimiz gibi oluşmadıysa son oluşturulan 'BorsaListeYeni*.CSV' dosyasını al
                        try
                        {
                            var adaylar = Directory.GetFiles(System.Windows.Forms.Application.StartupPath, "BorsaListeYeni*.CSV")
                                                  .OrderByDescending(f => File.GetLastWriteTime(f))
                                                  .ToList();
                            var aday = adaylar.FirstOrDefault();
                            if (!string.IsNullOrEmpty(aday))
                            {
                                try { MyTools.logyaz("CSV fallback bulundu: " + aday); } catch { }
                                try { if (File.Exists(hedefCsv)) File.Delete(hedefCsv); } catch { }
                                try { File.Move(aday, hedefCsv); } catch { }
                                try { MyTools.logyaz("CSV fallback taşındı: " + hedefCsv); } catch { }
                            }
                            else
                            {
                                try { MyTools.logyaz("CSV fallback da bulunamadı."); } catch { }
                            }
                        }
                        catch { }
                    }

                    var kaynakXlsx = System.Windows.Forms.Application.StartupPath + @"\" + "BorsaTamListe" + now.Month.ToString() + now.Year.ToString() + ".XLSX";
                    var hedefXlsx = Path.Combine(raporKlasoru, $"BorsaTamListe_{now:ddMMyyyy}_{saatText}.xlsx");
                    hedefXlsxPath = hedefXlsx;
                    try { MyTools.logyaz("XLSX dosyası aranıyor: " + kaynakXlsx + " - Var mı: " + File.Exists(kaynakXlsx)); } catch { }
                    if (File.Exists(kaynakXlsx))
                    {
                        try { if (File.Exists(hedefXlsx)) File.Delete(hedefXlsx); } catch { }
                        try { File.Copy(kaynakXlsx, hedefXlsx, true); } catch { }
                        try { File.Delete(kaynakXlsx); } catch { }
                        try { MyTools.logyaz("XLSX dosyası kopyalandı: " + hedefXlsx); } catch { }
                    }
                    else
                    {
                        try { MyTools.logyaz("XLSX dosyası bulunamadı: " + kaynakXlsx); } catch { }
                    }

                    // Klasör içeriğini logla (teşhis için)
                    try
                    {
                        var csvList = string.Join(", ", Directory.GetFiles(raporKlasoru, "BorsaListeYeni_*.csv"));
                        var xlsxList = string.Join(", ", Directory.GetFiles(raporKlasoru, "BorsaTamListe_*.xlsx"));
                        MyTools.logyaz("Klasörde CSV'ler: " + (string.IsNullOrEmpty(csvList) ? "(yok)" : csvList));
                        MyTools.logyaz("Klasörde XLSX'ler: " + (string.IsNullOrEmpty(xlsxList) ? "(yok)" : xlsxList));
                    }
                    catch { }

                    // Dosya taşıma işleminin tamamlanması için kısa bekleme
                    System.Threading.Thread.Sleep(1000);

                    // Gerçekleşen çıktılara göre mesajı dinamik oluştur
                    var csvOlustu = File.Exists(hedefCsv);
                    var xlsxOlustu = File.Exists(hedefXlsx);

                    if (csvOlustu && xlsxOlustu)
                    {
                        isSuccess = true;
                        description = "Borsa Liste Yeni ve Borsa Tam Liste kayıt edildi.";
                    }
                    else if (csvOlustu)
                    {
                        isSuccess = true;
                        description = "Borsa Liste Yeni kayıt edildi.";
                    }
                    else if (xlsxOlustu)
                    {
                        isSuccess = true;
                        description = "Borsa Tam Liste kayıt edildi.";
                    }
                    else
                    {
                        isSuccess = false;
                        description = "Rapor dosyaları oluşturulamadı.";
                    }
                }
                catch (Exception exgen)
                {
                    isSuccess = false;
                    description = exgen.Message;
                }

                // 2) Read mail list from settings and send the report via email
                var mailListesi = MyTools.AyarOku(OtmIniSection, OtmIniKeyMailListesi, 4096, _otmIniPath);
                var mails = (mailListesi ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(x => x.Trim())
                                              .Where(x => x.Length > 0)
                                              .Distinct()
                                              .ToList();

                if (mails.Count > 0)
                {
                    try
                    {
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                        var message = new System.Net.Mail.MailMessage();
                        
                        var smtp = new System.Net.Mail.SmtpClient("smtp.office365.com");
                        smtp.Credentials = new System.Net.NetworkCredential("idealdestek@idealdata.com.tr", "Mor43719");
                        smtp.EnableSsl = true;
                        smtp.Port = 587;
                        message.From = new System.Net.Mail.MailAddress("idealdestek@idealdata.com.tr", "İdeal Data", Encoding.UTF8);
                        message.Subject = "Otomatik raporlar - " + DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                        foreach (var email in mails)
                        {
                            message.To.Add(email);
                        }
                        var dizinBilgisi = string.IsNullOrWhiteSpace(raporKlasoru) ? "" : ("\nRapor dizini: " + raporKlasoru);
                        if (isSuccess)
                            message.Body = "Otomatik raporlar başarıyla oluşturuldu : " + description + dizinBilgisi;
                        else
                            message.Body = "Otomatik rapor oluşturma başarısız oldu : " + description + dizinBilgisi;

                        // Raporları mail ekine ekle (sadece bu çalıştırmada üretilenler)
                        #region zipsiz raporEkleri
                        //if (isSuccess && (!string.IsNullOrWhiteSpace(hedefCsvPath) || !string.IsNullOrWhiteSpace(hedefXlsxPath)))
                        //{
                        //    try
                        //    {
                        //        // Dosya boyutlarını kontrol et
                        //        long csvSize = 0, xlsxSize = 0;
                        //        if (!string.IsNullOrWhiteSpace(hedefCsvPath) && File.Exists(hedefCsvPath))
                        //            csvSize = new FileInfo(hedefCsvPath).Length;
                        //        if (!string.IsNullOrWhiteSpace(hedefXlsxPath) && File.Exists(hedefXlsxPath))
                        //            xlsxSize = new FileInfo(hedefXlsxPath).Length;

                        //        var totalSize = csvSize + xlsxSize;
                        //        const long maxSize = 20 * 1024 * 1024; // 20MB

                        //        if (totalSize > maxSize)
                        //        {
                        //            // Dosya boyutu büyük, ek ekleme
                        //            message.Body += "\n\nNot: Dosya boyutu büyük olduğu için (" + (totalSize / (1024 * 1024)).ToString("F1") + " MB) ekler gönderilmedi.";
                        //            try { MyTools.logyaz("Uyarı;Mail Ek Boyut;" + "Toplam boyut: " + (totalSize / (1024 * 1024)).ToString("F1") + " MB - Ekler gönderilmedi"); } catch { }
                        //        }
                        //        else
                        //        {
                        //            // Normal ek ekleme
                        //            if (!string.IsNullOrWhiteSpace(hedefCsvPath) && File.Exists(hedefCsvPath))
                        //            {
                        //                var csvAttachment = new System.Net.Mail.Attachment(hedefCsvPath);
                        //                csvAttachment.Name = "BorsaListeYeni.csv";
                        //                message.Attachments.Add(csvAttachment);
                        //            }

                        //            if (!string.IsNullOrWhiteSpace(hedefXlsxPath) && File.Exists(hedefXlsxPath))
                        //            {
                        //                var xlsxAttachment = new System.Net.Mail.Attachment(hedefXlsxPath);
                        //                xlsxAttachment.Name = "BorsaTamListe.xlsx";
                        //                message.Attachments.Add(xlsxAttachment);
                        //            }
                        //        }
                        //    }
                        //    catch (Exception exAttach)
                        //    {
                        //        // Ek ekleme hatası durumunda mail gönderimini durdurma, sadece log yaz
                        //        try { MyTools.logyaz("Hata;Mail Ek Ekleme;" + exAttach.Message); } catch { }
                        //    }
                        //}
                        #endregion
                        // Raporları mail ekine ekle (sadece bu çalıştırmada üretilenler)
                        // Mevcut tek mail bloğunu şununla değiştir:
                        if (isSuccess && (!string.IsNullOrWhiteSpace(hedefCsvPath) || !string.IsNullOrWhiteSpace(hedefXlsxPath)))
                        {
                            try
                            {
                                var zipCsvPath = Path.Combine(raporKlasoru, $"BorsaListeYeni_{now:ddMMyyyy}_{saatText}.zip");
                                var zipXlsxPath = Path.Combine(raporKlasoru, $"BorsaTamListe_{now:ddMMyyyy}_{saatText}.zip");

                                if (!string.IsNullOrWhiteSpace(hedefCsvPath) && File.Exists(hedefCsvPath))
                                {
                                    try { if (File.Exists(zipCsvPath)) File.Delete(zipCsvPath); } catch { }
                                    using (var archive = ZipFile.Open(zipCsvPath, ZipArchiveMode.Create))
                                        archive.CreateEntryFromFile(hedefCsvPath, Path.GetFileName(hedefCsvPath), CompressionLevel.Optimal);
                                }

                                if (!string.IsNullOrWhiteSpace(hedefXlsxPath) && File.Exists(hedefXlsxPath))
                                {
                                    try { if (File.Exists(zipXlsxPath)) File.Delete(zipXlsxPath); } catch { }
                                    using (var archive = ZipFile.Open(zipXlsxPath, ZipArchiveMode.Create))
                                        archive.CreateEntryFromFile(hedefXlsxPath, Path.GetFileName(hedefXlsxPath), CompressionLevel.Optimal);
                                }

                                const long maxSize = 25 * 1024 * 1024; // 25MB (Office 365 limiti)

                                // CSV ZIP'i ayrı mail olarak gönder
                                if (File.Exists(zipCsvPath) && new FileInfo(zipCsvPath).Length < maxSize)
                                {
                                    try
                                    {
                                        var msgCsv = new System.Net.Mail.MailMessage();
                                        msgCsv.From = new System.Net.Mail.MailAddress("idealdestek@idealdata.com.tr", "İdeal Data", Encoding.UTF8);
                                        msgCsv.Subject = "Otomatik rapor (1/2) BorsaListeYeni - " + DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                                        foreach (var email in mails) msgCsv.To.Add(email);
                                        msgCsv.Body = "Otomatik raporlar başarıyla oluşturuldu.\nEk: BorsaListeYeni" + dizinBilgisi;

                                        var att = new System.Net.Mail.Attachment(zipCsvPath);
                                        att.Name = "BorsaListeYeni.zip";
                                        msgCsv.Attachments.Add(att);

                                        smtp.Send(msgCsv);
                                        att.Dispose();
                                        msgCsv.Dispose();
                                        try { MyTools.logyaz("CSV ZIP mail gönderildi: " + (new FileInfo(zipCsvPath).Length / (1024.0 * 1024.0)).ToString("F1") + " MB"); } catch { }
                                    }
                                    catch (Exception exCsv)
                                    {
                                        try { MyTools.logyaz("Hata;CSV Mail Gönderim;" + exCsv.Message); } catch { }
                                    }
                                }

                                // XLSX ZIP'i ayrı mail olarak gönder
                                if (File.Exists(zipXlsxPath) && new FileInfo(zipXlsxPath).Length < maxSize)
                                {
                                    try
                                    {
                                        var msgXlsx = new System.Net.Mail.MailMessage();
                                        msgXlsx.From = new System.Net.Mail.MailAddress("idealdestek@idealdata.com.tr", "İdeal Data", Encoding.UTF8);
                                        msgXlsx.Subject = "Otomatik rapor (2/2) BorsaTamListe - " + DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                                        foreach (var email in mails) msgXlsx.To.Add(email);
                                        msgXlsx.Body = "Otomatik raporlar başarıyla oluşturuldu.\nEk: BorsaTamListe" + dizinBilgisi;

                                        var att = new System.Net.Mail.Attachment(zipXlsxPath);
                                        att.Name = "BorsaTamListe.zip";
                                        msgXlsx.Attachments.Add(att);

                                        smtp.Send(msgXlsx);
                                        att.Dispose();
                                        msgXlsx.Dispose();
                                        try { MyTools.logyaz("XLSX ZIP mail gönderildi: " + (new FileInfo(zipXlsxPath).Length / (1024.0 * 1024.0)).ToString("F1") + " MB"); } catch { }
                                    }
                                    catch (Exception exXlsx)
                                    {
                                        try { MyTools.logyaz("Hata;XLSX Mail Gönderim;" + exXlsx.Message); } catch { }
                                    }
                                }
                            }
                            catch (Exception exAttach)
                            {
                                try { MyTools.logyaz("Hata;Mail Ek Ekleme;" + exAttach.Message); } catch { }
                            }
                        }

                        // Bilgilendirme mailini eksiz gönder (mevcut message objesi)
                        //smtp.Send(message);
                        //var smtp = new System.Net.Mail.SmtpClient("smtp.office365.com");
                        //smtp.Credentials = new System.Net.NetworkCredential("idealdestek@idealdata.com.tr", "Mor43719");
                        //smtp.EnableSsl = true;
                        //smtp.Port = 587;
                        //smtp.Send(message);

                        // Attachment'ları temizle
                        foreach (System.Net.Mail.Attachment attachment in message.Attachments)
                        {
                            attachment.Dispose();
                        }
                        message.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("Otomatik raporlar için bilgilendirme maili gönderilemedi");
                    }
                }
                else
                {
                    MyTools.logyaz("OtomatikBorsaRapor: mail listesi boş olduğu için gönderim yapılmadı.");
                }
            }
            catch (Exception ex)
            {
                try { MyTools.logyaz("Hata;TriggerOtomatikBorsaRapor;" + ex.Message); } catch { }
            }
        }

        public async void TriggerOtomatikBorsaRaporNowForTest()
        {
            await TriggerOtomatikBorsaRapor();
        }

        public void TestMailGonder()
        {
            try
            {
                var mailListesi = MyTools.AyarOku(OtmIniSection, OtmIniKeyMailListesi, 4096, _otmIniPath);
                var mails = (mailListesi ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(x => x.Trim())
                                              .Where(x => x.Length > 0)
                                              .Distinct()
                                              .ToList();

                if (mails.Count == 0)
                {
                    MessageBox.Show("Mail listesi boş.");
                    return;
                }

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                var message = new System.Net.Mail.MailMessage();
                message.From = new System.Net.Mail.MailAddress("idealdestek@idealdata.com.tr", "İdeal Data", Encoding.UTF8);
                message.Subject = "TEST - Otomatik raporlar - " + DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                foreach (var email in mails)
                    message.To.Add(email);

                message.Body = "Bu bir test mailidir. Rapor ekleri kasıtlı olarak gönderilmemiştir.";

                var smtp = new System.Net.Mail.SmtpClient("smtp.office365.com");
                smtp.Credentials = new System.Net.NetworkCredential("idealdestek@idealdata.com.tr", "Mor43719");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.Send(message);
                message.Dispose();

                MessageBox.Show("Test maili gönderildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Test maili gönderilemedi: " + ex.Message);
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var secili = dataGridView1.Rows[e.RowIndex].Cells[7].Value;
            //dataGridView1.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex+1).ToString();
            if (secili != null)
            {
                if ((bool)secili)
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                else
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
            }
        }

        private void formAdminAra_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        public void gridGuncelle()
        {

            ara();



        }


        private void ara()
        {
            try
            {
                if (acilis)
                {
                    crmDFNDataContext crm = new crmDFNDataContext();
                    var sorgu = crm.Users.Where(x => x.UserName != null);

                    if (txtUserName.Text != "")
                    {
                        sorgu = sorgu.Where(x => x.UserName.Contains(txtUserName.Text)
                        || x.Name.Contains(txtUserName.Text)
                        || x.Surname.Contains(txtUserName.Text)
                        || x.Aciklama.Contains(txtUserName.Text)
                          || x.Iletisim.Il.IlAdi.Contains(txtUserName.Text)
                        || x.PmtsNo.Contains(txtUserName.Text));
                    }
                    if (txtAdi.Text != "")
                    {

                        sorgu = sorgu.Where(x => x.UserName.Contains(txtAdi.Text)
                        || x.Name.Contains(txtAdi.Text)
                        || x.Surname.Contains(txtAdi.Text)
                        || x.Aciklama.Contains(txtAdi.Text)
                          || x.Iletisim.Il.IlAdi.Contains(txtAdi.Text)
                        || x.PmtsNo.Contains(txtAdi.Text));
                    }
                    if (txtSurname.Text != "")
                    {

                        sorgu = sorgu.Where(x => x.UserName.Contains(txtSurname.Text)
                        || x.Name.Contains(txtSurname.Text)
                        || x.Surname.Contains(txtSurname.Text)
                        || x.Aciklama.Contains(txtSurname.Text)
                          || x.Iletisim.Il.IlAdi.Contains(txtSurname.Text)
                        || x.PmtsNo.Contains(txtSurname.Text));
                    }


                    if (comboYayinDurum.SelectedIndex != -1)
                    {
                        if (comboYayinDurum.SelectedIndex == 0)
                            sorgu = sorgu.Where(x => x.LisansDurum.YayinDurumu == true);
                        else if (comboYayinDurum.SelectedIndex == 1)
                            sorgu = sorgu.Where(x => x.LisansDurum.YayinDurumu == false);
                    }


                    if (comboEkranType.SelectedIndex == 0)
                        sorgu = sorgu.Where(x => x.LisansDurum.ProYetki == true && x.LisansDurum.CepYetki == false || x.LisansDurum.SCMUsable == true);
                    else if (comboEkranType.SelectedIndex == 1)
                        sorgu = sorgu.Where(x => x.LisansDurum.CepYetki == true && x.LisansDurum.ProYetki == false);
                    else if (comboEkranType.SelectedIndex == 2)
                        sorgu = sorgu.Where(x => x.LisansDurum.CepYetki == true && x.LisansDurum.ProYetki == true);
                    else if (comboEkranType.SelectedIndex == 3)
                        sorgu = sorgu.Where(x => x.LisansDurum.ROBOT == true);
                    else if (comboEkranType.SelectedIndex == 4)
                        sorgu = sorgu.Where(x => x.ProductType == "SCM" && (x.LisansDurum.SCMDownload == true || x.LisansDurum.SCMRealTıme == true || x.LisansDurum.SCMUsable == true));
                    else if (comboEkranType.SelectedIndex == 5)
                        sorgu = sorgu.Where(x => x.ProductType == "SCM" && x.LisansDurum.SCMUsable == true);


                    #region LisanlarAra


                


                    if (comboLisans.SelectedIndex == 0)
                        sorgu = sorgu.Where(x => x.LisansDurum.PayLP == true && x.LisansDurum.PayL2 == false && x.LisansDurum.Pd2P == false);
                    else if (comboLisans.SelectedIndex == 1)
                        sorgu = sorgu.Where(x => x.LisansDurum.PayLP == true && x.LisansDurum.PayL2 == true && x.LisansDurum.Pd2P == false);
                    else if (comboLisans.SelectedIndex == 2)
                        sorgu = sorgu.Where(x => x.LisansDurum.PayLP == true && x.LisansDurum.PayL2 == true && x.LisansDurum.Pd2P == true);
                    else if (comboLisans.SelectedIndex == 3)
                        sorgu = sorgu.Where(x => x.LisansDurum.PayX == true);
                    else if (comboLisans.SelectedIndex == 4)
                        sorgu = sorgu.Where(x => x.LisansDurum.PayGS == true);
                    else if (comboLisans.SelectedIndex == 5)
                        sorgu = sorgu.Where(x => x.LisansDurum.PITE == true);
                    else if (comboLisans.SelectedIndex == 6)
                        sorgu = sorgu.Where(x => x.LisansDurum.ViopLP == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.Vd2P == false);
                    else if (comboLisans.SelectedIndex == 7)
                        sorgu = sorgu.Where(x => x.LisansDurum.ViopLP == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.Vd2P == false);
                    else if (comboLisans.SelectedIndex == 8)
                        sorgu = sorgu.Where(x => x.LisansDurum.ViopLP == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.Vd2P == true);
                    else if (comboLisans.SelectedIndex == 9)
                        sorgu = sorgu.Where(x => x.LisansDurum.ViopGS == true);
                    else if (comboLisans.SelectedIndex == 10)
                        sorgu = sorgu.Where(x => x.LisansDurum.TahvilLP == true && x.LisansDurum.TahvilL2 == false);
                    else if (comboLisans.SelectedIndex == 11)
                        sorgu = sorgu.Where(x => x.LisansDurum.TahvilLP == true && x.LisansDurum.TahvilL2 == true);
                    else if (comboLisans.SelectedIndex == 12)
                        sorgu = sorgu.Where(x => x.LisansDurum.COMEX == true);
                    else if (comboLisans.SelectedIndex == 13)
                        sorgu = sorgu.Where(x => x.LisansDurum.MKK == true);
                    else if (comboLisans.SelectedIndex == 14)
                        sorgu = sorgu.Where(x => x.LisansDurum.CME == true);
                    else if (comboLisans.SelectedIndex == 15)
                        sorgu = sorgu.Where(x => x.LisansDurum.ROBOT == true);
                    else if (comboLisans.SelectedIndex == 16)
                        sorgu = sorgu.Where(x => x.LisansDurum.GKKUL == true);
                    else if (comboLisans.SelectedIndex == 17)
                        sorgu = sorgu.Where(x => x.LisansDurum.ProYetki == true);
                    #endregion


                    if (comboStatus.SelectedIndex != -1)
                    {
                        var a = comboStatus.SelectedIndex;

                        if (a == 8)
                        {

                        }
                        else
                        {
                            sorgu = sorgu.Where(x => x.StatusId == a + 1);
                        }

                    }

                    //if (comboYurtDisiType.SelectedIndex != 0)
                    //{
                    //    if (comboYurtDisiType.SelectedIndex == 1)
                    //        sorgu = sorgu.Where(x => x.ProNonPro.Value == true);
                    //    else if (comboYurtDisiType.SelectedIndex == 2)
                    //        sorgu = sorgu.Where(x => x.ProNonPro.Value == false);
                    //}
                    //var sorguUserLimit = crm.Users.Where(x => x.tckno != null).Take(chkUserlimit.Checked ? int.MaxValue : 100);
                    var sorguUserLimit = crm.Users.Where(x => x.UserName != null).Take(chkUserlimit.Checked ? int.MaxValue : 100);
                    //if (chkUserlimit.Checked == false)
                    //    sorgu = sorgu.Take(100);


                    // if (rb500.Checked) sorgu = sorgu.OrderByDescending(x => x.UserID).Take(500);
                    if (rb500.Checked) sorgu = sorgu.OrderByDescending(x => x.UserID).Take(500);
                    else sorgu = sorgu.OrderByDescending(x => x.UserID);


                    DataList = sorgu.ToList();

                    lblSayac.Text = DataList.Count.ToString();
                    ActiveSorgu = sorgu;

                    dataGridView1.DataSource = sorgu.OrderByDescending(x => x.UserID).Select(x => new
                    {
                        x.UserName,
                        TCKN = x.tckno,
                        MusteriNo = x.PmtsNo,
                        Adı_Soyadı = x.Name + " " + x.Surname,
                        x.Aciklama,
                        Expiry_Date = x.ExpiryDate.Value.Date,
                        x.Iletisim.Tel1,
                        x.LisansDurum.YayinDurumu,
                        x.UserID
                    });
                }
                else acilis = true;



            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ToplamShow()
        {

            try
            {

                #region Toplamlar


                var Cep = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true).Count();
                var pro = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ProYetki == true).Count();
                var robot = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ROBOT == true).Count();
                var ProCep = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true && x.LisansDurum.ProYetki == true).Count();
                var PayYuzeysel = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false && x.LisansDurum.Pd2P == false).Count();
                var PayPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                var PayDerinlik = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                var PayDerinlikPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == true).Count();
                var PayEndeks = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayX == true).Count();
                var PayGs = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayGS == true).Count();

                var Pite = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PITE == true).Count();

                var viopYuzeysel = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false && x.LisansDurum.Vd2P == false).Count();
                var viopPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                var viopDerinlik = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                var viopDerinlikPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == true).Count();
                var viopGS = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();
                var krmd1 = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.COMEX == true).Count();
                var mkk = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.MKK == true).Count();
                var tarama = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TARAMA == true).Count();
                var gkkul = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.GKKUL == true).Count();

                var TahvilYuzeysel = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false).Count();
                var TahvilPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == true).Count();
                var TahvilDerinlik = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == true && x.LisansDurum.TahvilLP == true).Count();

                var AnalizPro = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.AnPro == true && x.LisansDurum.AnPro == true && x.LisansDurum.AnPro == true).Count();

                var On = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true).Count();
                var Off = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == false).Count();




                gridLisanDurumOzet.Rows[0].SetValues("Pro", pro.ToString(), "KRMD1", krmd1.ToString(), "", "");
                gridLisanDurumOzet.Rows[1].SetValues("Mobil", Cep.ToString(), "PD1P", PayPlus.ToString(), "VD1P", viopPlus.ToString());
                gridLisanDurumOzet.Rows[2].SetValues("Pro+Mobil", ProCep.ToString(), "PD2", PayDerinlik.ToString(), "VD2", viopDerinlik.ToString());
                gridLisanDurumOzet.Rows[3].SetValues("", "", "Pd2P", PayDerinlikPlus.ToString(), "Vd2P", viopDerinlikPlus.ToString());
                gridLisanDurumOzet.Rows[4].SetValues("Robot", robot.ToString(), "END", PayEndeks.ToString(), "VIT", viopGS.ToString());
                gridLisanDurumOzet.Rows[5].SetValues("", "", "PIT", PayGs.ToString(), "BD1P", TahvilPlus.ToString());
                gridLisanDurumOzet.Rows[6].SetValues("Açık", On.ToString(), "PITE", Pite, "BD2", TahvilDerinlik.ToString());
                gridLisanDurumOzet.Rows[7].SetValues("Kapalı", Off.ToString(), "GKKUL", gkkul.ToString(), "MKK", mkk.ToString());
                gridLisanDurumOzet.Rows[8].SetValues("", "", "", "", "", "");


                gridLisanDurumOzet.Rows[5].Cells[1].Selected = true;

                var renk = Color.Firebrick;
                var renkback = Color.WhiteSmoke;
                for (int i = 0; i < 9; i++)
                {
                    gridLisanDurumOzet.Rows[i].Cells[0].Style.ForeColor = gridLisanDurumOzet.Rows[i].Cells[2].Style.ForeColor = gridLisanDurumOzet.Rows[i].Cells[4].Style.ForeColor = renk;
                    gridLisanDurumOzet.Rows[i].Cells[0].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[2].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[4].Style.BackColor = renkback;
                }

                for (int i = 0; i < 9; i++)
                {
                    gridLisanDurumOzet.Rows[i].Cells[0].Style.ForeColor = gridLisanDurumOzet.Rows[i].Cells[2].Style.ForeColor = gridLisanDurumOzet.Rows[i].Cells[4].Style.ForeColor = renk;
                }


                toplamguncelle = false;


                #endregion


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }




        public void GridUpdateHesaplama(IQueryable<User> sorgu)
        {

            if (gridLisanDurumOzet.InvokeRequired)
            {

                UpdateGridThreadHandler upgrid = new UpdateGridThreadHandler(GridUpdateHesaplama);
                this.Invoke(upgrid, new object[] { sorgu });

            }
            else
            {

                if (toplamguncelle)
                {

                    var Cep = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true).Count();
                    var pro = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ProYetki == true).Count();
                    var robot = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ROBOT == true).Count();
                    var ProCep = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true && x.LisansDurum.ProYetki == true).Count();
                    var PayYuzeysel = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false && x.LisansDurum.Pd2P == false).Count();
                    var PayPlus = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                    var PayDerinlik = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                    var PayDerinlikPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == true).Count();
                    var PayEndeks = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayX == true).Count();
                    var PayGs = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayGS == true).Count();


                    var analitik = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.VeriAnalitik == true).Count();

                    var viopYuzeysel = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false && x.LisansDurum.Vd2P == false).Count();
                    var viopPlus = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                    var viopDerinlik = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                    var viopDerinlikPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == true).Count();
                    var viopGS = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();


                    var TahvilYuzeysel = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false).Count();
                    var TahvilPlus = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == true).Count();
                    var TahvilDerinlik = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == true && x.LisansDurum.TahvilLP == true).Count();

                    var AnalizPro = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.AnPro == true && x.LisansDurum.AnPro == true && x.LisansDurum.AnPro == true).Count();

                    var On = sorgu.Where(x => x.LisansDurum.YayinDurumu == true).Count();
                    var Off = sorgu.Where(x => x.LisansDurum.YayinDurumu == false).Count();



                    gridLisanDurumOzet.Rows[0].SetValues("Pro", pro.ToString(), "", "", "", "");
                    gridLisanDurumOzet.Rows[1].SetValues("Mobil", Cep.ToString(), "PayL1+", PayPlus.ToString(), "ViopL1+", viopPlus.ToString());
                    gridLisanDurumOzet.Rows[2].SetValues("Pro+Mobil", ProCep.ToString(), "PayL2", PayDerinlik.ToString(), "ViopL2", viopDerinlik.ToString());
                    gridLisanDurumOzet.Rows[3].SetValues("Robot", robot.ToString(), "Pd2P", PayDerinlikPlus.ToString(), "Vd2P", viopDerinlikPlus.ToString());
                    gridLisanDurumOzet.Rows[4].SetValues("", "", "", "PayX", PayEndeks.ToString(), "ViopGS", viopGS.ToString());
                    gridLisanDurumOzet.Rows[5].SetValues("", "", "PayGS", PayGs.ToString(), "", "");
                    gridLisanDurumOzet.Rows[6].SetValues("Açık", On.ToString(), "", "", "THVL1+", TahvilPlus.ToString());
                    gridLisanDurumOzet.Rows[7].SetValues("Kapalı", Off.ToString(), "", "", "THVL2", TahvilDerinlik.ToString());

                    gridLisanDurumOzet.Rows[5].Cells[1].Selected = true;



                    var renk = Color.Firebrick;
                    var renkback = Color.WhiteSmoke;
                    for (int i = 0; i < 8; i++)
                    {
                        gridLisanDurumOzet.Rows[i].Cells[0].Style.ForeColor = gridLisanDurumOzet.Rows[i].Cells[2].Style.ForeColor = gridLisanDurumOzet.Rows[i].Cells[4].Style.ForeColor = renk;
                        gridLisanDurumOzet.Rows[i].Cells[0].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[2].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[4].Style.BackColor = renkback;
                    }


                    toplamguncelle = false;

                }

            }

        }
        public void GridAnaUpdate(IQueryable<User> sorgu)
        {

            if (dataGridView1.InvokeRequired)
            {

                UpdateGridThreadHandler upgrid = new UpdateGridThreadHandler(GridAnaUpdate);
                this.Invoke(upgrid, new object[] { sorgu });

            }
            else
            {

                DataList = sorgu.ToList();


                GridUpdateHesaplama(sorgu);


                dataGridView1.DataSource = sorgu.OrderByDescending(x => x.UserID).Select(x => new
                {
                    x.UserName,
                    // TCKN = x.tckno,
                    x.tckno,
                    x.PmtsNo,
                    Adı_Soyadı = x.Name + " " + x.Surname,
                    x.Aciklama,
                    Start_Date = x.BaslangicTarihi.Value.Date,
                    x.ExpiryDate,
                    x.Iletisim.Tel1,
                    x.LisansDurum.YayinDurumu,
                    x.UserID
                });

            }
        }

        private void ServerGuncelemetoForm()
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            var sorgu = crm.Users.Where(x => x.UserName != null);
            GridAnaUpdate(sorgu);
        }



        public void ListBoxTransactionEkle(string text)
        {
            try
            {

                if (listBoxTransaction.InvokeRequired)
                {
                    ListBoxTransactionGuncelle list = new ListBoxTransactionGuncelle(ListBoxTransactionEkle);
                    this.Invoke(list, new object[] { text });
                }
                else
                {
                    if (listBoxTransaction.Items.Count > 100)
                        listBoxTransaction.Items.RemoveAt(listBoxTransaction.Items.Count - 1);
                    listBoxTransaction.Items.Insert(0, DateTime.Now.ToString() + " : " + text);
                    MyTools.logyaz(text);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void comboKurum_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(comboKurum.SelectedIndex!=-1)
            //ara();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {

                txtUserName.Text = string.Empty;
                txtAdi.Text = string.Empty;
                txtSurname.Text = string.Empty;
                txtSurname.Text = string.Empty;
                comboYayinDurum.SelectedIndex = -1;
                comboEkranType.SelectedIndex = -1;
                comboLisans.SelectedIndex = -1;

                ara();
                ToplamShow();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
                return;

            var id = dataGridView1.CurrentRow.Cells[8].Value;

            crmDFNDataContext crm = new crmDFNDataContext();

            var user = crm.Users.FirstOrDefault(x => x.UserID == (int)id);

            if (user == null)
                return;

            formUserDetay frm = new formUserDetay();
            frm.Tag = user.UserID;
            frm.Show();



        }

        private void btnYaniKayit_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboYayinDurum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboYayinDurum.SelectedIndex != -1)
            {
                ara();
                ToplamShow();
            }
        }

        private void comboEkranType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboEkranType.SelectedIndex != -1)
            {
                ara();
                ToplamShow();
            }
        }



        #region MenuClickEvents
        private void baglantiAyarlariToolStripMenuItem_Click(object sender, EventArgs e)
        {

            formAdminAyarlar frm = new formAdminAyarlar();
            frm.txtServerIP.Text = IPport.RemoteHost;
            frm.txtServerPort.Text = IPport.RemotePort.ToString();
            frm.ShowInTaskbar = false;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();

        }
        private void calisanlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formCalisancs cal = new formCalisancs();
            cal.StartPosition = FormStartPosition.CenterScreen;
            cal.Show();
        }

        private void MenuUserOlaylar_Click(object sender, EventArgs e)
        {
            formOlayGoruntule frm = new formOlayGoruntule();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }

        private void exceleAktarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyTools.ExcelRutin.ToExcel("AnaFrom", DataList);
        }

        private void kurumlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formMusteriler frm = new formMusteriler();
            frm.Show();
        }

        private void muhasebeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Muhasebe.formMuhasebe frm = new Muhasebe.formMuhasebe();
            frm.Show();

        }

        private void pazarlamaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pazarlama.formPazarlamaAna frm = new Pazarlama.formPazarlamaAna();
            frm.Tag = ActiveCalisan;
            frm.Show();
        }

        private void lisansFiyatlariToolStripMenuItem_Click(object sender, EventArgs e)
        {

            formLisansFiyatlari frm = new formLisansFiyatlari();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();

        }

        private void RaporBorsaOzetListe_Click(object sender, EventArgs e)
        {
            MyTools.ExcelRutin.BorsaOzetlisteToExcel();
        }

        private void RaporBorsaDetayListe_Click(object sender, EventArgs e)
        {
            MyTools.ExcelRutin.BorsaDetaylisteToExcel();
        }

        private void MenuDovizKurlari_Click(object sender, EventArgs e)
        {
            formKurlar frm = new formKurlar();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();

        }

        private void MenuYetki_Click(object sender, EventArgs e)
        {
            formYetki frm = new formYetki();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }
        private void MenuProjeler_Click(object sender, EventArgs e)
        {
            formProjeler frm = new formProjeler();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();

        }

        private void menuSozlesme_Click(object sender, EventArgs e)
        {


        }

        private void MenuProjeUrunler_Click(object sender, EventArgs e)
        {

        }


        private void MenuNotAra_Click(object sender, EventArgs e)
        {
            formNotAra frm = new formNotAra();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }

        private void MenuOyakListe_Click(object sender, EventArgs e)
        {
            // MyTools.ExcelRutin.OyakListeToExcel();
        }


        private void MenuHesapMakinesi_Click(object sender, EventArgs e)
        {

            formHesapMakinesi frm = new formHesapMakinesi();
            frm.Show();


        }

        private void MenuToplanti_Click(object sender, EventArgs e)
        {

        }

        private void MenuKurumTalepleri_Click(object sender, EventArgs e)
        {
            var frm = new formKurumTalepleri();
            frm.Show();
        }

        #endregion




        private void IPport_OnConnected(object sender, nsoftware.IPWorks.IpportConnectedEventArgs e)
        {
            if (MyTools.serverbag)
            {
                if (e.StatusCode == 0)
                {

                    panelBaglanti.BackColor = Color.LimeGreen;
                    IPport.DataToSend = "Connect|" + ActiveCalisan.Ad + " " + ActiveCalisan.Soyad + (char)3;
                }
                else
                {
                    panelBaglanti.BackColor = Color.Red;

                    formBaglantiKotrol frm = new formBaglantiKotrol();
                    frm.lblMesaj.Text = "Server Makinaya Bağlantınız Kurulamadı\nlütfen Sistem Yöneticinize başvurun";
                    frm.ShowIcon = false;
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.TopMost = true;
                    frm.ShowDialog();


                }
            }
        }

        public void baglantiyok(object s, EventArgs e)
        {
            HbControl.Dispose();
            formBaglantiKotrol frm = new formBaglantiKotrol();
            frm.lblMesaj.Text = "Server Makina ile Bağlantınız Koptu\n Lütfen sistem yöneticinize başvurunuz";
            frm.ShowDialog();

        }

        private void IPport_OnDisconnected(object sender, nsoftware.IPWorks.IpportDisconnectedEventArgs e)
        {
            try
            {
                this.panelBaglanti.BackColor = Color.Red;
                this.Invoke(new EventHandler(baglantiyok));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void IPport_OnDataIn(object sender, nsoftware.IPWorks.IpportDataInEventArgs e)
        {

            try
            {
                var data = e.Text.Split((char)3);

                foreach (var d in data)
                {

                    if (d.StartsWith("CreateUser"))
                    {
                        var satir = d.Split('|');
                        ListBoxTransactionEkle(satir[1]);
                        toplamguncelle = true;
                        //  ServerGuncelemetoForm();
                    }
                    else if (d.Contains("_ChangeUser"))
                    {
                        var satir = d.Split('|');
                        ListBoxTransactionEkle(satir[1]);
                        toplamguncelle = true;
                        // ServerGuncelemetoForm();
                    }
                    else if (d.StartsWith("AutoCreateWebCustomer"))
                    {
                        ListBoxTransactionEkle(d);
                        toplamguncelle = true;
                        // ServerGuncelemetoForm();
                    }
                    else if (d.Contains("_CreateUser"))
                    {
                        var satir = d.Split('|');
                        ListBoxTransactionEkle(satir[1]);
                        toplamguncelle = true;
                        // ServerGuncelemetoForm();
                    }
                    else if (d.StartsWith("OYAK_AUTO_CREATE"))
                    {
                        var satir = d.Split('|');
                        ListBoxTransactionEkle(satir[1]);
                        toplamguncelle = true;
                        //  ServerGuncelemetoForm();
                    }
                    else if (d.StartsWith("WEBKURUMTALEP"))
                    {
                        var satir = d.Split('|');
                        ListBoxTransactionEkle(satir[1]);
                    }
                    else if (d.StartsWith("TekMesaj") || d.StartsWith("PUSH") || d.StartsWith("TopluMesaj"))
                    {
                        var satir = d.Split('|');
                        ListBoxTransactionEkle(satir[1]);
                    }
                    else if (d.StartsWith("HisseSinyal_CreateUser") || d.StartsWith("HisseSinyal_UpdateUser") || d.StartsWith("HisseSinyal_UpdateLisance"))
                    {
                        var satir = d.Split('|');
                        ListBoxTransactionEkle(satir[1]);
                    }
                }

            }
            catch (Exception ex)
            {

                ListBoxTransactionEkle("Hata;IPport_OnDataIn;" + ex.Message);
            }

        }

        private void timerServerKontrol_Tick(object sender, EventArgs e)
        {


        }

        private void comboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ara();
            ToplamShow();

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                if (txtUserName.Text != null)
                    ara();
                //ToplamShow();

            }
        }

        private void txtAdi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtAdi.Text != null)
                    ara();
                // ToplamShow();

            }
        }

        private void txtSurname_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtSurname.Text != null)
                    ara();
                // ToplamShow();
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            formUserDetay frm = new formUserDetay();
            frm.Tag = 0;
            frm.Show();
        }

        private void ContexMenuAnaSecileniGonder_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count < 0)
                    return;
                var idlist = new List<string>();

                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    idlist.Add(row.Cells[8].Value.ToString());

                }

                var newthread = new Thread(new ThreadStart(() =>
                {
                    var say = 0;

                    foreach (var id in idlist)
                    {
                        say++;
                        IPport.DataToSend = "SendUserInfo|" + id + (char)3;
                        // ListBoxTransactionEkle(id + " id li Kullanıcı Bilgileri Servera gönderildi ");
                        Thread.Sleep(100);

                        LabelGuncelle(lblSendingCount, idlist.Count.ToString() + " / " + say);
                    }

                }));

                newthread.Start();
            }
            catch (Exception)
            {

                throw;
            }

        }



        void LabelGuncelle(System.Windows.Forms.Label lbl, string text)
        {

            try
            {
                if (lbl.InvokeRequired)
                {

                    lbl.Invoke((MethodInvoker)delegate { LabelGuncelle(lbl, text); });

                }
                else
                {
                    lbl.Text = text;
                }
            }
            catch { }

        }

        private void ContexMenuAnaTumunuGonder_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 0)
                return;
            var idlist = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                idlist.Add(row.Cells[8].Value.ToString());

            }



            var newthread = new Thread(new ThreadStart(() =>
            {


                foreach (var id in idlist)
                {
                    IPport.DataToSend = "SendUserInfo|" + id + (char)3;
                    // ListBoxTransactionEkle(id + " id li Kullanıcı Bilgileri Servera gönderildi ");
                    Thread.Sleep(1000);
                }

            }));

            newthread.Start();
        }

        private void menuAna_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void comboLisans_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboLisans.SelectedIndex != -1)
            {
                ara();
                ToplamShow();
            }
        }

        private void listBoxTransaction_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                if (listBoxTransaction.SelectedItems.Count < 1)
                    return;
                if (listBoxTransaction.SelectedItems[0].ToString().Contains("Talep") || listBoxTransaction.SelectedItems[0].ToString().Contains("TALEP"))
                {
                    var talepid = listBoxTransaction.SelectedItems[0].ToString().Split(';')[1].Split('=')[1].Trim();

                    var frrm = new formKurumTalepDetay();
                    frrm.Tag = talepid._ToInt();
                    frrm.Show();
                }
                else
                {
                    var username = listBoxTransaction.SelectedItems[0].ToString().Split(';')[1].Split('=')[1].Trim();
                    var userid = crm.Users.FirstOrDefault(x => x.UserName == username).UserID;

                    if (userid == 0)
                    {
                        MessageBox.Show("user bulunamadı");
                        return;
                    }
                    formUserDetay frm = new formUserDetay();
                    frm.Tag = userid;
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MenuControlItem1_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MenuControlItem2_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuControlItem3_Click(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void MenuControlItem4_Click(object sender, EventArgs e)
        {
            try
            {

                formEkstraAra frm = new formEkstraAra();
                frm.Tag = 4;
                frm.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void btnSedatKontrol_Click(object sender, EventArgs e)
        {
            //  formSedatKontrol frm = new formSedatKontrol();

            formHesapMakinesi frm = new formHesapMakinesi();
            frm.Show();


        }

        private void userToplamAnalizToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new formToplamAnaliz();
            frm.Show();

        }

        private void pushNotificationMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    var frm = new formClientPushNotification();
            //    frm.Show();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            try
            {
                var frm = new formPushNotification();
                frm.Tag = ActiveCalisan.calisanID;
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void duzeltmelerMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                var frm = new formDuzeltmeler();
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void testMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MyTools.logyaz("test için yazılmıştır");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void musteriTaleplerMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new formMusteriFeedBack();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();

        }

        private void textListedenMusteriKapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                var filename = "";
                op.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                op.Filter = "(*.txt)|*.txt";

                if (op.ShowDialog() == DialogResult.OK)
                {
                    filename = op.FileName;
                }
                else
                    return;
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var crm = new crmDFNDataContext();

                        var array = File.ReadAllLines(filename);


                        for (int i = 0; i < array.Length; i++)
                        {
                            var username = array[i].Trim();

                            if (crm.Users.Where(x => x.UserName == username).Any())
                            {
                                var user = crm.Users.FirstOrDefault(x => x.UserName == username);


                                if (user.LisansDurum.YayinDurumu == false)
                                {
                                    formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + user.UserID.ToString() + (char)3;
                                    continue;
                                }
                                user.ExpiryDate = DateTime.Now;
                                UserEvent userevent = new UserEvent();

                                var lid = MyTools.lisansAcKapa(user.LisansDurum, false, 1);

                                Thread.Sleep(30);
                                user.LisansDurum = lid;
                                crm.SubmitChanges();
                                userevent.CalisanId = 2;
                                userevent.EventTypeId = 2;
                                userevent.HostName = HostName;
                                userevent.IP = IpAdress;
                                userevent.LisansDurum = lid;
                                userevent.UserId = user.UserID;
                                userevent.EventTarih = DateTime.Now;

                                crm.UserEvents.InsertOnSubmit(userevent);
                                crm.SubmitChanges();

                                formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + user.UserID.ToString() + (char)3;

                                ListBoxTransactionEkle(username);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BuAySonGunMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count < 0)
                    return;
                var id = Int32.Parse(dataGridView1.SelectedRows[0].Cells[8].Value.ToString());
                var crm = new crmDFNDataContext();
                if (crm.Users.Where(x => x.UserID == id).Any())
                {
                    var user = crm.Users.FirstOrDefault(x => x.UserID == id);
                    user.ExpiryDate = DateTime.Now._LastDayOfMonth();
                    UserEvent userevent = new UserEvent();
                    crm.SubmitChanges();
                    userevent.CalisanId = ActiveCalisan.calisanID;
                    userevent.EventTypeId = 2;
                    userevent.HostName = HostName;
                    userevent.IP = IpAdress;
                    userevent.LisansDurum = user.LisansDurum;
                    userevent.UserId = user.UserID;
                    userevent.EventTarih = DateTime.Now;
                    crm.UserEvents.InsertOnSubmit(userevent);
                    crm.SubmitChanges();
                    formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + user.UserID.ToString() + (char)3;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GecenAySonGunMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count < 0)
                    return;
                var id = Int32.Parse(dataGridView1.SelectedRows[0].Cells[8].Value.ToString());
                var crm = new crmDFNDataContext();
                if (crm.Users.Where(x => x.UserID == id).Any())
                {
                    var user = crm.Users.FirstOrDefault(x => x.UserID == id);
                    user.ExpiryDate = DateTime.Now._PrevDayOfMonth();
                    UserEvent userevent = new UserEvent();
                    crm.SubmitChanges();
                    userevent.CalisanId = ActiveCalisan.calisanID;
                    userevent.EventTypeId = 2;
                    userevent.HostName = HostName;
                    userevent.IP = IpAdress;
                    userevent.LisansDurum = user.LisansDurum;
                    userevent.UserId = user.UserID;
                    userevent.EventTarih = DateTime.Now;
                    crm.UserEvents.InsertOnSubmit(userevent);
                    crm.SubmitChanges();
                    formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + user.UserID.ToString() + (char)3;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void teknikAnalizOneriToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (formTeknikAnalizRapor.reference == null)
            {
                formTeknikAnalizRapor.reference = new formTeknikAnalizRapor();
                formTeknikAnalizRapor.reference.Show();
            }
            else
            {
                formTeknikAnalizRapor.reference.BringToFront();
            }

        }

        private void modelPortfoyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formModelPortfoy.reference == null)
            {
                formModelPortfoy.reference = new formModelPortfoy();
                formModelPortfoy.reference.Show();
            }
            else
            {
                formModelPortfoy.reference.BringToFront();
            }
        }

        private void borsaTamListeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { MyTools.ExcelRutin.BorsaTamListe(); } catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void yuzeyselKarmaDonusumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tüm Yüzeysel Kullanıcılar Karma Düzey 1 Lisansa çevrilecektir. \nEmin misiniz?", "Karma Düzey 1 Dönüşüm", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            MyTools.ExcelDurumFormAc();
            Thread T = new Thread((obj) =>
            {
                try
                {

                    var crm = new crmDFNDataContext();

                    var users = crm.Users.Where(x => x.LisansDurum.YayinDurumu == true && ((x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false && x.LisansDurum.ViopL1 == true)
                    || (x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false && x.LisansDurum.PayL1 == true))).ToList();
                    for (int i = 0; i < users.Count; i++)
                    {
                        var user = users[i];

                        Thread.Sleep(200);
                        var kapanan = new StringBuilder();

                        #region LisansCopy
                        LisansDurum sondurum = new LisansDurum();
                        sondurum.YayinDurumu = user.LisansDurum.YayinDurumu;
                        sondurum.PayGS = user.LisansDurum.PayGS;
                        sondurum.PITE = user.LisansDurum.PITE;
                        sondurum.COMEX = true;


                        sondurum.ViopGS = user.LisansDurum.ViopGS;

                        if (user.LisansDurum.PayL1 == true) kapanan.Append(";PayL1");
                        if (user.LisansDurum.ViopL1 == true) kapanan.Append(";ViopL1");
                        if (user.LisansDurum.PayX == true) kapanan.Append(";PayX");

                        sondurum.PayLP = user.LisansDurum.PayLP;
                        sondurum.PayL2 = user.LisansDurum.PayL2;
                        sondurum.Pd2P = user.LisansDurum.Pd2P;
                        sondurum.ViopLP = user.LisansDurum.ViopLP;
                        sondurum.ViopL2 = user.LisansDurum.ViopL2;
                        sondurum.Vd2P = user.LisansDurum.Vd2P;

                        sondurum.TahvilL1 = user.LisansDurum.TahvilL1;
                        sondurum.TahvilLP = user.LisansDurum.TahvilLP;
                        sondurum.TahvilL2 = user.LisansDurum.TahvilL2;

                        sondurum.AnPro = user.LisansDurum.AnPro;

                        sondurum.DJI = user.LisansDurum.DJI;
                        sondurum.SPI = user.LisansDurum.SPI;
                        sondurum.XETRA = user.LisansDurum.XETRA;
                        sondurum.CBOT = user.LisansDurum.CBOT;
                        sondurum.CBOTM = user.LisansDurum.CBOTM;
                        sondurum.CME = user.LisansDurum.CME;
                        sondurum.CMEM = user.LisansDurum.CMEM;
                        sondurum.EUREX = user.LisansDurum.EUREX;

                        sondurum.CepYetki = user.LisansDurum.CepYetki;
                        sondurum.ProYetki = user.LisansDurum.ProYetki;
                        sondurum.ROBOT = user.LisansDurum.ROBOT;

                        sondurum.SCMDownload = user.LisansDurum.SCMDownload;
                        sondurum.SCMRealTıme = user.LisansDurum.SCMRealTıme;
                        sondurum.SCMUsable = user.LisansDurum.SCMUsable;

                        sondurum.Futgck = user.LisansDurum.Futgck;
                        sondurum.WINX = user.LisansDurum.WINX;
                        sondurum.MKK = user.LisansDurum.MKK;
                        sondurum.TARAMA = user.LisansDurum.TARAMA;
                        sondurum.GKKUL = user.LisansDurum.GKKUL;

                        #endregion

                        crm.LisansDurums.InsertOnSubmit(sondurum);
                        crm.SubmitChanges();

                        UserEvent ue = new UserEvent();
                        ue.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
                        ue.EventTarih = DateTime.Now;
                        ue.IP = formAdminAra.referance.IpAdress;
                        ue.AcilanLisans = "KRMD1";
                        ue.HostName = formAdminAra.referance.HostName;
                        ue.EventTypeId = 6;
                        ue.SonLisandurumID = sondurum.LisansDurumId;
                        user.LisansDurum = sondurum;
                        ue.UserId = user.UserID;
                        ue.KapatilanLisans = kapanan.ToString();
                        crm.UserEvents.InsertOnSubmit(ue);
                        crm.SubmitChanges();

                        formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + user.UserID.ToString() + (char)3;
                        MyTools.ExcelDurumYaz(users.Count + " /" + (i + 1));
                    }
                }
                catch (Exception ex)
                {
                    MyTools.ExcelDurumYaz("Hata : " + ex.Message);
                }

            });
            T.Start();
        }
        private void MenuItemKulListTip1_Click(object sender, EventArgs e)
        {
            MyTools.ExcelRutin.OrtakListe();
        }


        private void MenuItemKulListTip2_Click(object sender, EventArgs e)
        {
        }
        private void sentimentAlgoMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (formSentimentAlgo.reference == null)
                {
                    formSentimentAlgo.reference = new formSentimentAlgo();
                    formSentimentAlgo.reference.Show();
                }
                else
                {
                    formSentimentAlgo.reference.BringToFront();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void yDSRaporToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MyTools.ExcelRutin.YdsRapor();
        }

        private void yDSRaporAktifKullanıcılarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyTools.ExcelRutin.YdsRaporAciklar();
        }

        private void loginOlanKullanıcılarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                formEkstraAra frm = new formEkstraAra();
                frm.Tag = 1;
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void hisseSinyaluserMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new formHisseSinyalUsers();
                frm.Show();
            }
            catch { }
        }

        private void rb500_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ara();
            }
            catch { }
        }

        private void dosyadanEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new KurumSubeCalisanEkle();
            frm.Show();
        }

        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        private void aktifKullanıcıRaporListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyTools.ExcelRutin.AcikKullaniciListesi();
        }

        private void HbControl_Tick(object sender, EventArgs e)
        {
            try
            {
                IPport.DataToSend = "H" + (char)3;

            }
            catch (Exception ex)
            {
                HbControl.Dispose();
                formBaglantiKotrol frm = new formBaglantiKotrol();
                frm.lblMesaj.Text = "Server Makina ile Bağlantınız Koptu\n Lütfen sistem yöneticinize başvurunuz";
                frm.ShowDialog();
            }
        }

        private void subeEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formSubeEkle frm = new formSubeEkle();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }

        private void cepVeProEnşanlıKapalıKontrolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formCepProEsanliKontrol formCepProEsanliKontrol = new formCepProEsanliKontrol();
            formCepProEsanliKontrol.Show();
        }

        private void borsaListeYeniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MyTools.ExcelRutin.BorsaListeYeni();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void paket10RaporToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MyTools.ExcelRutin.OnluPaketToExcel();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void kRMD1RaporToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MyTools.ExcelRutin.KRMD1ToExcel();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void ProLisansliKullanicilarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MyTools.ProlisansiOlanKullaniciExcelRaporu();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void DosyadanKullaniciislemleriMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new formDosyadanUserIslemleri();

                frm.Show();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void otomatikBorsaRaporToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new formOtomatikBorsaRapor();

                frm.Show();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void abonelerTxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("_Aboneler.Txt dosyası oluşturulacak. Devam etmek istiyor musunuz?",
                    "Dosya Oluştur", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;


                var customers = GetCustomersFromDatabase();
                if (customers == null)
                {
                    MessageBox.Show("Müşteri listesi alınamadı. Logları kontrol et.");
                    return;
                }

                string outputPath = System.Windows.Forms.Application.StartupPath + @"\_Aboneler.Txt";


                CreateAbonelerFile(customers, outputPath);

                MessageBox.Show($"Başarılı!\n\nToplam {customers.Count} müşteri kaydı oluşturuldu.\n\nDosya: {outputPath}",
                    "İşlem Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void CreateAbonelerFile(List<cxCustomer> customers, string outputPath)
        {
            try
            {
               
                var CHR2 = (char)2;
                var list = new List<string>();

                foreach (var item in customers)
                {
                    string SafeString(string value) => value ?? "";
                    string SafeBool(bool value) => value ? "1" : "0";

                    string str =
                        SafeString(item.Username) + CHR2 +           // 1
                        SafeString(MyTools.Sifreleme.Decryp(item.Password)) + CHR2 +           // 2
                        SafeString(item.TerminalID) + CHR2 +         // 3
                        SafeBool(item.OnOff) + CHR2 +                // 4
                        SafeString(item.Expiry) + CHR2 +             // 5
                        SafeString(item.Version) + CHR2 +            // 6
                        SafeString(item.MachineName) + CHR2 +        // 7
                        SafeString(item.ProductType) + CHR2 +        // 8
                        SafeString(item.IP) + CHR2 +                 // 9
                        SafeBool(item.SCM) + CHR2 +                  // 10
                        SafeBool(item.SCMusable) + CHR2 +            // 11
                        SafeBool(item.RealtimeHostEnabled) + CHR2 +  // 12
                        SafeBool(item.DownloadHostEnabled) + CHR2 +  // 13
                        SafeString(item.Explanation) + CHR2 +        // 14
                        SafeBool(item.DDE) + CHR2 +                  // 15
                        SafeBool(item.IMKBL1) + CHR2 +               // 16
                        SafeBool(item.IMKBL1P) + CHR2 +              // 17
                        SafeBool(item.IMKBL2) + CHR2 +               // 18
                        SafeBool(item.IMKBISL) + CHR2 +              // 19
                        SafeBool(item.IMKBX) + CHR2 +                // 20
                        SafeBool(item.PITE) + CHR2 +                 // 21
                        SafeBool(item.SASEL1) + CHR2 +               // 22
                        SafeBool(item.SASEL2) + CHR2 +               // 23
                        SafeBool(item.THVL1) + CHR2 +                // 24
                        SafeBool(item.THVL1P) + CHR2 +               // 25
                        SafeBool(item.THVL2) + CHR2 +                // 26
                        SafeBool(item.VIPL1) + CHR2 +                // 27
                        SafeBool(item.VIPL1P) + CHR2 +               // 28
                        SafeBool(item.VIPL2) + CHR2 +                // 29
                        SafeBool(item.VIPNET) + CHR2 +               // 30
                        SafeBool(item.FUTGCK) + CHR2 +               // 31
                        SafeBool(item.WINX) + CHR2 +                 // 32
                        SafeBool(item.AMEX) + CHR2 +                 // 33
                        SafeBool(item.CBOT) + CHR2 +                 // 34
                        SafeBool(item.CBOTM) + CHR2 +                // 35
                        SafeBool(item.CME) + CHR2 +                  // 36
                        SafeBool(item.CMEM) + CHR2 +                 // 37
                        SafeBool(item.COMEX) + CHR2 +                // 38
                        SafeBool(item.DJI) + CHR2 +                  // 39
                        SafeBool(item.EUREX) + CHR2 +                // 40
                        SafeBool(item.LSE) + CHR2 +                  // 41
                        SafeBool(item.NYMEX) + CHR2 +                // 42
                        SafeBool(item.NYMEXM) + CHR2 +               // 43
                        SafeBool(item.NYSE) + CHR2 +                 // 44
                        SafeBool(item.NASDAQ) + CHR2 +               // 45
                        SafeBool(item.SPI) + CHR2 +                  // 46
                        SafeBool(item.XETRA) + CHR2 +                // 47
                        SafeBool(item.CHIX) + CHR2 +                 // 48
                        SafeBool(item.GTIS) + CHR2 +                 // 49
                        SafeBool(item.DJBN) + CHR2 +                 // 50
                        SafeBool(item.DJA) + CHR2 +                  // 51
                        SafeBool(item.DJCS) + CHR2 +                 // 52
                        SafeBool(item.DJES) + CHR2 +                 // 53
                        SafeBool(item.DJF) + CHR2 +                  // 54
                        SafeBool(item.DJN) + CHR2 +                  // 55
                        SafeBool(item.MEKSA) + CHR2 +                // 56
                        SafeBool(item.Messenger) + CHR2 +            // 57
                        SafeBool(item.DovizQuote) + CHR2 +           // 58
                        SafeString(item.KRM) + CHR2 +                // 59
                        SafeString(item.DownloadUrl) + CHR2 +        // 60
                        SafeString(item.PmtsNo) + CHR2 +             // 61
                        SafeString(item.Telefon) + CHR2 +            // 62
                        SafeString(item.Mail) + CHR2 +               // 63
                        SafeString(item.Adres) + CHR2 +              // 64
                        SafeString(item.ServerIP1) + CHR2 +          // 65
                        SafeString(item.ServerPort1) + CHR2 +        // 66 - STRING!
                        SafeString(item.ServerIP2) + CHR2 +          // 67
                        SafeString(item.ServerPort2) + CHR2 +        // 68 - STRING!
                        SafeString(item.ServerIP3) + CHR2 +          // 69
                        SafeString(item.ServerPort3) + CHR2 +        // 70 - STRING!
                        SafeString(item.ServerIP4) + CHR2 +          // 71
                        SafeString(item.ServerPort4) + CHR2 +        // 72 - STRING!
                        SafeString(item.ChartIP) + CHR2 +            // 73
                        SafeString(item.ChartPort) + CHR2 +          // 74 - STRING!
                        item.IdbPort.ToString() + CHR2 +             // 75 - INT
                        SafeBool(item.Robot) + CHR2 +                // 76
                        SafeString(item.GhostKurum) + CHR2 +         // 77
                        SafeString(item.WebMesajKurum) + CHR2 +      // 78
                        SafeString(item.WebMesajGrup) + CHR2 +       // 79
                        item.WebMesajEnabled.ToString() + CHR2 +     // 80
                        item.PRO.ToString() + CHR2 +                 // 81
                        item.CEP.ToString() + CHR2 +                 // 82
                        item.Status.ToString() + CHR2 +              // 83
                        SafeString(item.BaslangicTarih) + CHR2 +     // 84
                        SafeString(item.KimlikNo) + CHR2 +           // 85
                        SafeString(item.Fiyat) + CHR2 +              // 86
                        SafeString(item.SozlesmeKabulDateStr) + CHR2 +     // 87
                        SafeString(item.DisclaimerKabulDateStr) + CHR2 +   // 88
                        SafeString(item.RaporWebUrun) + CHR2 +       // 89
                        SafeString(item.RaporWebCihaz) + CHR2 +      // 90
                        SafeString(item.RaporWebSonLoginTarihi) + CHR2 +   // 91
                        item.RaporWebBuAyLoginSayisi.ToString() + CHR2 +   // 92
                        item.RaporWebOncekiAyLoginSayisi.ToString() + CHR2 + // 93
                        SafeString(item.Sube) + CHR2 +               // 94
                        SafeString(item.KullaniciTip) + CHR2 +       // 95
                        SafeString(item.Isim) + CHR2 +               // 96
                        SafeString(item.Sehir) + CHR2 +              // 97
                        SafeString(item.KurumMusteriNo) + CHR2 +     // 98
                        SafeString(item.Not1) + CHR2 +               // 99
                        SafeString(item.Not2) + CHR2 +               // 100
                        SafeString(item.Not3) + CHR2 +               // 101
                        item.DataAktarimPort.ToString() + CHR2 +     // 102
                        SafeString(item.Senetler) + CHR2 +           // 103
                        SafeString(item.MultiLisans);                // 104 - SON ALAN, CHR2 YOK!

                    list.Add(str);
                }

                File.WriteAllLines(outputPath, list);
            }
            catch (Exception ex)
            {
                MyTools.logyaz("Dosya oluşturma hatası: " + ex.Message);
               MessageBox.Show("Dosya oluşturma hatası: " + ex.Message);
            }
        }
        public List<cxCustomer> GetCustomersFromDatabase()
        {
            try
            {
                var customers = new List<cxCustomer>();

                using (var crm = new crmDFNDataContext())
                {
                    var users = crm.Users
                        .Where(x => x.UserName != null && x.UserName != "")
                        .ToList();

                    foreach (var user in users)
                    {

                        var multiLisans = "";
                        if (user.LisansDurum?.MKK ?? false) multiLisans += "MKK;";
                        if (user.LisansDurum?.GKKUL ?? false) multiLisans += "GKKUL;";

                        var customer = new cxCustomer
                        {

                            Username = user.UserName ?? "",
                            Password = user.Password ?? "",
                            TerminalID = user.TerminalID ?? "",
                            OnOff = user.LisansDurum?.YayinDurumu ?? false,
                            Expiry = user.ExpiryDate?.ToString("yyyyMMdd") ?? "",
                            Version = "0",
                            MachineName = user.UserExtraInfo?.ComputerName ?? "",
                            ProductType = user.ProductType ?? "",
                            IP = user.UserExtraInfo?.IP ?? "",
                            Explanation = user.Aciklama ?? "",

                            // SCM ayarları
                            SCM = user.ProductType == "SCM",
                            SCMusable = user.LisansDurum?.SCMUsable ?? false,
                            RealtimeHostEnabled = user.LisansDurum?.SCMRealTıme ?? true,
                            DownloadHostEnabled = user.LisansDurum?.SCMDownload ?? true,
                            DDE = false,

                            IMKBL1 = user.LisansDurum?.PayL1 ?? false,
                            IMKBL1P = user.LisansDurum?.PayLP ?? false,
                            IMKBL2 = user.LisansDurum?.PayL2 ?? false,
                            IMKBISL = user.LisansDurum?.PayGS ?? false,
                            IMKBX = user.LisansDurum?.PayX ?? false,
                            PITE = user.LisansDurum?.PITE ?? false,
                            SASEL1 = false,
                            SASEL2 = false,
                            THVL1 = user.LisansDurum?.TahvilL1 ?? false,
                            THVL1P = user.LisansDurum?.TahvilLP ?? false,
                            THVL2 = user.LisansDurum?.TahvilL2 ?? false,
                            VIPL1 = user.LisansDurum?.ViopL1 ?? false,
                            VIPL1P = user.LisansDurum?.ViopLP ?? false,
                            VIPL2 = user.LisansDurum?.ViopL2 ?? false,
                            VIPNET = user.LisansDurum?.ViopGS ?? false,
                            FUTGCK = user.LisansDurum?.Futgck ?? false,
                            WINX = user.LisansDurum?.WINX ?? false,
                            AMEX = false, //
                            CBOT = user.LisansDurum?.CBOT ?? false,//
                            CBOTM = user.LisansDurum?.CBOTM ?? false,//
                            CME = user.LisansDurum?.CME ?? false,
                            CMEM = user.LisansDurum?.CMEM ?? false,
                            COMEX = user.LisansDurum?.COMEX ?? false,


                            DJI = user.LisansDurum?.DJI ?? false,
                            EUREX = user.LisansDurum?.Pd2P ?? false,
                            LSE = false,
                            NYMEX = false,
                            NYMEXM = false,
                            NYSE = false,
                            NASDAQ = false,
                            SPI = user.LisansDurum?.SPI ?? false,
                            XETRA = user.LisansDurum?.XETRA ?? false,
                            CHIX = user.LisansDurum?.Vd2P ?? false,
                            GTIS = false,
                            DJBN = false,
                            DJA = false,
                            DJCS = false,
                            DJES = false,
                            DJF = false,
                            DJN = false,
                            MEKSA = false,
                            Messenger = false,
                            DovizQuote = false,

                            KRM = "",
                            DownloadUrl = "",
                            PmtsNo = user.PmtsNo ?? "",
                            Telefon = user.Iletisim?.Tel1 ?? "",
                            Mail = user.Iletisim?.email ?? "",
                            Adres = user.Iletisim?.acikadres ?? "",

                            // Server bilgileri - ServerList tablosundan
                            //ServerIP1 = user.ServerList?.ServerIP1 ?? "",
                            //ServerPort1 = user.ServerList?.ServerPort1 ?? 0,
                            //ServerIP2 = user.ServerList?.ServerIP2 ?? "",
                            //ServerPort2 = user.ServerList?.ServerPort2 ?? 0,
                            //ServerIP3 = user.ServerList?.ServerIP3 ?? "",
                            //ServerPort3 = user.ServerList?.ServerPort3 ?? 0,
                            //ServerIP4 = user.ServerList?.ServerIP4 ?? "",
                            //ServerPort4 = user.ServerList?.ServerPort4 ?? 0,
                            //ChartIP = user.ServerList?.ChartIP ?? "",
                            //ChartPort = user.ServerList?.ChartPort ?? 0,
                            //IdbPort = user.ServerList?.IdbPort ?? 0,
                            ServerIP1 = user.ServerList?.Server1 ?? "",
                            ServerPort1 = user.ServerList?.Port1 ?? "",
                            ServerIP2 = user.ServerList?.Server2 ?? "",
                            ServerPort2 = user.ServerList?.Port2 ?? "",
                            ServerIP3 = user.ServerList?.Server3 ?? "",
                            ServerPort3 = user.ServerList?.Port3 ?? "",
                            ServerIP4 = user.ServerList?.Server4 ?? "",
                            ServerPort4 = user.ServerList?.Port4 ?? "",
                            ChartIP = user.ServerList?.ChartServer ?? "",
                            ChartPort = user.ServerList?.ChartPort ?? "",
                            IdbPort = 0,
                            ////// 

                            // Kullanıcı ayarları
                            Robot = user.LisansDurum?.ROBOT ?? false,
                            GhostKurum = user.FXkurum ?? "",
                            WebMesajKurum = user.UserExtraInfo?.WebMesajKurum ?? "",
                            WebMesajGrup = user.UserExtraInfo?.WebMesajGrup ?? "",
                            WebMesajEnabled = 0,
                            PRO = (user.ProNonPro ?? false) ? 1 : 0,
                            CEP = (user.LisansDurum?.CepYetki ?? false) ? 1 : 0,
                            Status = user.StatusId ?? 0,

                            // Tarih ve kimlik
                            BaslangicTarih = user.BaslangicTarihi?.ToString("yyyyMMdd") ?? "",
                            KimlikNo = user.tckno ?? "",
                            Fiyat = "",
                            SozlesmeKabulDateStr = user.UserExtraInfo?.SozlesmeOnayDate ?? "",
                            DisclaimerKabulDateStr = user.UserExtraInfo?.MesajOnayDate ?? "",

                            // Rapor bilgileri
                            RaporWebUrun = user.UserExtraInfo?.RaporWebUrun ?? "",
                            RaporWebCihaz = user.UserExtraInfo?.RaporWebCihaz ?? "",
                            RaporWebSonLoginTarihi = user.UserExtraInfo?.RaporWebSonLoginTarihi ?? "",
                            RaporWebBuAyLoginSayisi = user.UserExtraInfo?.RaporWebBuAyLoginSayisi ?? 0,
                            RaporWebOncekiAyLoginSayisi = user.UserExtraInfo?.RaporWebOncekiAyLoginSayisi ?? 0,

                            // Kurumsal bilgiler
                            Sube = user.KurumsalBilgiler?.KurumSube ?? "",
                            KullaniciTip = user.KurumsalBilgiler?.KurumKullaniciTip ?? "",
                            // Isim = user.KurumsalBilgiler?. ?? "",
                            Isim = "",
                            Sehir = user.Iletisim?.Il?.IlAdi ?? "",
                            // KurumMusteriNo = user.KurumsalBilgiler?.KurumMusteriNo ?? "",
                            KurumMusteriNo = "",

                            // Notlar
                            Not1 = user.UserExtraInfo?.Not1 ?? "",
                            Not2 = user.UserExtraInfo?.Not2 ?? "",
                            Not3 = user.UserExtraInfo?.Not3 ?? "",
                            // DataAktarimPort = user.ServerList?.ChartPort ?? 0,
                            DataAktarimPort = 0,
                            // Senetler = user.SozHaricLisans ?? "",
                            Senetler = "",
                            MultiLisans = multiLisans
                        };

                        customers.Add(customer);
                    }
                }

                return customers;
            }
            catch (Exception ex)
            {
                MyTools.logyaz(ex.ToString());
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        private void dBBağlantıBilgileriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmDbSettings())
            {
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
            }
        }

        public class cxCustomer
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string TerminalID { get; set; }
            public bool OnOff { get; set; }
            public string Expiry { get; set; }
            public string Version { get; set; }
            public string MachineName { get; set; }
            public string ProductType { get; set; }
            public string IP { get; set; }
            public bool SCM { get; set; }
            public bool SCMusable { get; set; }
            public bool RealtimeHostEnabled { get; set; }
            public bool DownloadHostEnabled { get; set; }
            public string Explanation { get; set; }
            public bool DDE { get; set; }

            public bool IMKBL1 { get; set; }
            public bool IMKBL1P { get; set; }
            public bool IMKBL2 { get; set; }
            public bool IMKBISL { get; set; }//17
            public bool IMKBX { get; set; }
            public bool PITE { get; set; }
            public bool SASEL1 { get; set; }
            public bool SASEL2 { get; set; }
            public bool THVL1 { get; set; }
            public bool THVL1P { get; set; }
            public bool THVL2 { get; set; }
            public bool VIPL1 { get; set; }
            public bool VIPL1P { get; set; }
            public bool VIPL2 { get; set; }
            public bool VIPNET { get; set; }
            public bool FUTGCK { get; set; }
            public bool WINX { get; set; }
            public bool AMEX { get; set; }
            public bool CBOT { get; set; }
            public bool CBOTM { get; set; }
            public bool CME { get; set; }
            public bool CMEM { get; set; }
            public bool COMEX { get; set; }
            public bool DJI { get; set; }
            public bool EUREX { get; set; }
            public bool LSE { get; set; }
            public bool NYMEX { get; set; }
            public bool NYMEXM { get; set; }
            public bool NYSE { get; set; }
            public bool NASDAQ { get; set; }
            public bool SPI { get; set; }
            public bool XETRA { get; set; }
            public bool CHIX { get; set; }
            public bool GTIS { get; set; }
            public bool DJBN { get; set; }
            public bool DJA { get; set; }
            public bool DJCS { get; set; }
            public bool DJES { get; set; }
            public bool DJF { get; set; }
            public bool DJN { get; set; }
            public bool MEKSA { get; set; }
            public bool Messenger { get; set; }
            public bool DovizQuote { get; set; }


            public string KRM { get; set; }
            public string DownloadUrl { get; set; }
            public string PmtsNo { get; set; }
            public string Telefon { get; set; }
            public string Mail { get; set; }
            public string Adres { get; set; }
            public string ServerIP1 { get; set; }
            public string ServerPort1 { get; set; }
            public string ServerIP2 { get; set; }
            public string ServerPort2 { get; set; }
            public string ServerIP3 { get; set; }
            public string ServerPort3 { get; set; }
            public string ServerIP4 { get; set; }
            public string ServerPort4 { get; set; }
            public string ChartIP { get; set; }
            public string ChartPort { get; set; }
            public int IdbPort { get; set; }
            public bool Robot { get; set; }
            public string GhostKurum { get; set; }
            public string WebMesajKurum { get; set; }
            public string WebMesajGrup { get; set; }
            public int WebMesajEnabled { get; set; }
            public int PRO { get; set; }
            public int CEP { get; set; }
            public int Status { get; set; }
            public string BaslangicTarih { get; set; }
            public string KimlikNo { get; set; }
            public string Fiyat { get; set; }
            public string SozlesmeKabulDateStr { get; set; }
            public string DisclaimerKabulDateStr { get; set; }
            public string RaporWebUrun { get; set; }
            public string RaporWebCihaz { get; set; }
            public string RaporWebSonLoginTarihi { get; set; }
            public int RaporWebBuAyLoginSayisi { get; set; }
            public int RaporWebOncekiAyLoginSayisi { get; set; }
            public string Sube { get; set; }
            public string KullaniciTip { get; set; }
            public string Isim { get; set; }
            public string Sehir { get; set; }
            public string KurumMusteriNo { get; set; }
            public string Not1 { get; set; }
            public string Not2 { get; set; }
            public string Not3 { get; set; }
            public int DataAktarimPort { get; set; }
            public string Senetler { get; set; }
            public string MultiLisans { get; set; }
        }

        private void listedenServeraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                var filename = "";
                op.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                op.Filter = "(*.txt)|*.txt";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    filename = op.FileName;
                }
                else
                    return;

                var crm = new crmDFNDataContext();
                var array = File.ReadAllLines(filename);

                
                var idlist = new List<string>();
                foreach (var line in array)
                {
                    var username = line.Trim();
                    if (string.IsNullOrEmpty(username)) continue;

                    var user = crm.Users.FirstOrDefault(x => x.UserName == username);
                    if (user == null) continue;

                    idlist.Add(user.UserID.ToString());
                }

                var newthread = new Thread(new ThreadStart(() =>
                {
                    var say = 0;
                    foreach (var id in idlist)
                    {
                        say++;
                        IPport.DataToSend = "SendUserInfo|" + id + (char)3;
                        Thread.Sleep(100);
                        LabelGuncelle(lblSendingCount, idlist.Count.ToString() + " / " + say);
                      //  ListBoxTransactionEkle(id + " id li kullanıcı bilgileri servera gönderildi");
                    }
                }));
                newthread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void kullanıcıBilgiRaporuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MyTools.ExcelRutin.BorsaEkranRaporu();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

