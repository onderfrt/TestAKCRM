using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace DirectFNCRM
{
    public class MyTools
    {
        public static string DBName = "";
        public static string versiyon = "";
        public static string DataSource = "";
        public static string UserName = "";
        public string tckno = "";
        public static string Password = "";
        public static string ServerKey = "";
        public static string SenderId = "";
        public static string KurumKod = ""; // 10860 Halk
        public static string KurumAd = ""; // Halk Yatırım
        public static List<string> KurumKontrolList = new List<string>();
        public static string HisseSinyal = "";
        public static string KurumVersiyon = "1.31";
        public static bool serverbag = true;
        static ConcurrentQueue<string> Ekranlog = new ConcurrentQueue<string>();
        static ConcurrentQueue<string> DataLog = new ConcurrentQueue<string>();
        public static string GetConnectionString()
        {
            if (UserName != "")
                return "Data Source=" + DataSource + ";Initial Catalog=" + DBName + ";User ID=" + UserName + ";Password=" + Password + ";";
            else
            {
                return "Data Source=.;Initial Catalog=" + DBName + ";Integrated Security=True";
            }
        }
        public static void ConnectionStingAyarlariOku()
        {
            try
            {
                var ayarlarpath = Application.StartupPath + @"\ayarlar.ini";
                DBName = AyarOku("SQL", "DBNAME", 20, ayarlarpath);
                DataSource = AyarOku("SQL", "DataSource", 20, ayarlarpath);
                UserName = AyarOku("SQL", "UserName", 20, ayarlarpath);
                Password = MyTools.Sifreleme.Decryp(AyarOku("SQL", "Password", 100, ayarlarpath));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void ConnectionStingAyarlariYaz()
        {
            try
            {
                var ayarlarpath = Application.StartupPath + @"\ayarlar.ini";
                AyarYaz("SQL", "DBNAME", DBName, ayarlarpath);
                AyarYaz("SQL", "DataSource", DataSource, ayarlarpath);
                AyarYaz("SQL", "UserName", UserName, ayarlarpath);
                AyarYaz("SQL", "Password", MyTools.Sifreleme.Encryp(Password), ayarlarpath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static Calisan ActiveCalisan = new Calisan();

        public static class lisansfiyatlari
        {

            public static decimal Fpd1 = 0;
            public static decimal Fpd1p = 0;
            public static decimal Fpd2 = 0;
            public static decimal Fpd2p = 0;
            public static decimal Fend = 0;
            public static decimal Fpit = 0;
            public static decimal Fpva = 0;
            public static decimal Fpite = 0;


            public static decimal Ypd1 = 0;
            public static decimal Ypd1p = 0;
            public static decimal Ypd2 = 0;
            public static decimal Ypd2p = 0;
            public static decimal Yend = 0;
            public static decimal Ypit = 0;
            public static decimal Ypite = 0;
            public static decimal Ypva = 0;

            public static decimal Fvl1 = 0;
            public static decimal Fvl1p = 0;
            public static decimal Fvl2 = 0;
            public static decimal Fvl2p = 0;
            public static decimal Fvd2p = 0;
            public static decimal Fvit = 0;

            public static decimal Yvl1 = 0;
            public static decimal Yvl1p = 0;
            public static decimal Yvl2 = 0;
            public static decimal Yvl2p = 0;
            public static decimal Yvd2p = 0;
            public static decimal Yvit = 0;

            public static decimal Fbd1 = 0;
            public static decimal Fbd1p = 0;
            public static decimal Fbd2 = 0;

            public static decimal Fkrmd1 = 0;
            public static decimal Ykrmd1 = 0;

            public static decimal Ftarama = 0;
            public static decimal Fmkk = 0;
            public static decimal Fgkkul = 0;

            public static decimal Fcme = 0;
            public static decimal Frobot = 0;

            public static decimal Ytarama = 0;
            public static decimal Ymkk = 0;
            public static decimal Ygkkul = 0;
            public static decimal Ycme = 0;
            public static decimal Yrobot = 0;

            public static decimal Ybd1 = 0;
            public static decimal Ybd1p = 0;
            public static decimal Ybd2 = 0;

            public static decimal FanPro = 0; // Analiz pro lisans için
            public static decimal YanPro = 0;

            public static decimal FSPI = 0; // YDS lisans için
            public static decimal YSPI = 0;

            public static decimal FSentiL1 = 0;
            public static decimal FSentiL2 = 0;

            public static decimal YSentiL1 = 0;
            public static decimal YSentiL2 = 0;


            public static decimal DJIPro = 0;
            public static decimal DJINonPro = 0;

            public static decimal SpotPro = 0;
            public static decimal SpotNonPro = 0;
            public static decimal XETRAPro = 0;
            public static decimal XETRANonPro = 0;
            public static decimal SPIPro = 0;
            public static decimal SPINonPro = 0;
            public static decimal CBOTPro = 0;
            public static decimal CBOTNonPro = 0;
            public static decimal CBOTMPro = 0;
            public static decimal CBOTMNonPro = 0;
            public static decimal CMEPro = 0;
            public static decimal CMENonPro = 0;
            public static decimal CMEMPro = 0;
            public static decimal CMEMNonPro = 0;
            public static decimal EUREXPro = 0;
            public static decimal EUREXNonPro = 0;
            public static decimal COMEXPro = 0;
            public static decimal COMEXNonPro = 0;
            public static decimal NYMEXPro = 0;
            public static decimal NYMEXNonPro = 0;
            public static decimal NYMEXMPro = 0;
            public static decimal NYMEXMNonPro = 0;
            public static decimal NYSEPro = 0;
            public static decimal NYSENonPro = 0;
            public static decimal NASDAQPro = 0;
            public static decimal NASDAQNonPro = 0;
            public static decimal AmexPro = 0;
            public static decimal AmexNonPro = 0;
            public static decimal CHIXPro = 0;
            public static decimal CHIXNonPro = 0;
            public static decimal LSEPro = 0;
            public static decimal LSENonPro = 0;

            public static decimal GKULD2 = 0;
            public static decimal GKULD1P = 0;
            public static decimal GKULPVA = 0;
            public static decimal GKULEND = 0;
            public static decimal GUYED2 = 0;
            public static decimal GUYED2P = 0;
            public static decimal GUYED1P = 0;
            public static decimal GUYEPVA = 0;
            public static decimal GUYEEND = 0;

            public static decimal GKULPITE = 0;
            public static decimal GUYEPITE = 0;
            public static decimal GKULKYD = 0;
            public static decimal GUYEKYD = 0;

            public static decimal YGKULKYD = 0;
            public static decimal YGKULEND = 0;
            public static decimal YGKULD1P = 0;
            public static decimal YGKULD2 = 0;
            public static decimal YGKULPITE = 0;
            public static decimal YGKULPVA = 0;
            public static decimal YGUYEKYD = 0;

            public static decimal karma1k = 0;
            public static decimal karma5k = 0;
            public static decimal karma10k = 0;
            public static decimal karma20k = 0;
            public static decimal karma50k = 0;
            public static decimal karma100k = 0;
            public static decimal karmaSINIRSIZ = 0;

        }
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public static class Sifreleme
        {
            private static string key = "bsyrmursbsyrmursbsyrmurs";
            private static string iv = "bsntkrpktsnehtdn";
            public static string Encryp(string text)
            {
                try
                {
                    byte[] plaintextbytes = ASCIIEncoding.UTF8.GetBytes(text);
                    AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                    aes.BlockSize = 128;
                    aes.KeySize = 256;
                    aes.Key = ASCIIEncoding.ASCII.GetBytes(key);
                    aes.IV = ASCIIEncoding.ASCII.GetBytes(iv);
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;
                    ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
                    byte[] encrypted = crypto.TransformFinalBlock(plaintextbytes, 0, plaintextbytes.Length);

                    return Convert.ToBase64String(encrypted);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
            public static string Decryp(string decryped)
            {
                try
                {
                    byte[] encrypedbytes = Convert.FromBase64String(decryped);
                    AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                    aes.BlockSize = 128;
                    aes.KeySize = 256;
                    aes.Key = ASCIIEncoding.ASCII.GetBytes(key);
                    aes.IV = ASCIIEncoding.ASCII.GetBytes(iv);
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;
                    ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
                    byte[] secret = crypto.TransformFinalBlock(encrypedbytes, 0, encrypedbytes.Length);
                    return ASCIIEncoding.UTF8.GetString(secret);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
        }

        public class ExcelRutin
        {
            public static Microsoft.Office.Interop.Excel.Application ExcellApp;
            private static Task RunStaAsync(Action action)
            {
                var tcs = new TaskCompletionSource<bool>();
                var thread = new Thread(() =>
                {
                    try { action(); tcs.SetResult(true); }
                    catch (Exception ex) { tcs.SetException(ex); }
                });
                thread.IsBackground = true;
                try { thread.SetApartmentState(ApartmentState.STA); } catch { }
                thread.Start();
                return tcs.Task;
            }
            private static void SafeFreezePanes(Microsoft.Office.Interop.Excel.Worksheet sheet, int splitRow)
            {
                try
                {
                    if (sheet == null) return;
                    // Önce görünür ActiveWindow'u dene
                    try
                    {
                        if (sheet.Application != null && sheet.Application.ActiveWindow != null)
                        {
                            sheet.Application.ActiveWindow.SplitRow = splitRow;
                            sheet.Application.ActiveWindow.FreezePanes = true;
                            return;
                        }
                    }
                    catch { }

                    // Workbook penceresi üzerinden dene (headless durumda ActiveWindow null olabilir)
                    try
                    {
                        var wb = sheet.Parent as Microsoft.Office.Interop.Excel._Workbook;
                        if (wb != null && wb.Windows != null && wb.Windows.Count > 0)
                        {
                            var win = wb.Windows[1];
                            win.SplitRow = splitRow;
                            win.FreezePanes = true;
                        }
                    }
                    catch { }
                }
                catch { }
            }
            private static void SafeDeleteFile(string path)
            {
                if (string.IsNullOrWhiteSpace(path)) return;
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        if (File.Exists(path))
                        {
                            File.SetAttributes(path, FileAttributes.Normal);
                            File.Delete(path);
                        }
                        return;
                    }
                    catch (IOException)
                    {
                        System.Threading.Thread.Sleep(300);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        System.Threading.Thread.Sleep(300);
                    }
                }
            }
            public static void ToExcel(string formAdi, List<User> DataList)
            {
                try
                {
                    string filename = System.Windows.Forms.Application.StartupPath + "\\" + "CrmDFN_Rapor.XLSX";
                    if (File.Exists(filename)) File.Delete(filename);
                    var excelworkbook = GetExcelWorkbook(filename);
                    if (excelworkbook != null)
                    {
                        excelworkbook.Save();
                        excelworkbook.Close(false);
                    }
                    excelworkbook = OpenExcelWorkbook(filename);
                    if (excelworkbook == null) return;

                    var excelsheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.get_Item(1);

                    // var sorgu = (IQueryable<User>) dataGridView1.DataSource;
                    //var customerlist = sorgu.ToList();
                    var customerlist = DataList;

                    switch (formAdi)
                    {
                        case "AnaFrom":

                            var cellarray = new object[customerlist.Count + 1, 56];
                            for (int i = 0; i < cellarray.GetLength(0); i++)
                            {
                                for (int j = 0; j < cellarray.GetLength(1); j++)
                                    cellarray[i, j] = "";
                            }
                            
                            
                            cellarray[0, 0] = "USERNAME";
                            cellarray[0, 1] = "TCKNO";
                            cellarray[0, 2] = "MÜŞTERİ NO";
                            cellarray[0, 3] = "AD SOYAD";
                            cellarray[0, 4] = "Açıklama";
                            cellarray[0, 5] = "PRODUCT";
                            cellarray[0, 6] = "STATUS";
                            cellarray[0, 7] = "YayınDurumu";
                            cellarray[0, 8] = "Pro";
                            cellarray[0, 9] = "Cep";
                            cellarray[0, 10] = "Robot";
                            cellarray[0, 11] = "PayL1";
                            cellarray[0, 12] = "PayL1+";
                            cellarray[0, 13] = "PayL2";
                            cellarray[0, 14] = "PayL2+";
                            cellarray[0, 15] = "PayX";
                            cellarray[0, 16] = "PayGS";
                            cellarray[0, 17] = "PITE";
                            cellarray[0, 18] = "viopL1";
                            cellarray[0, 19] = "viopL1+";
                            cellarray[0, 20] = "viopL2";
                            cellarray[0, 21] = "viopL2+";
                            cellarray[0, 22] = "viopGS";
                            cellarray[0, 23] = "THVL1";
                            cellarray[0, 24] = "THVL1+";
                            cellarray[0, 25] = "THVL2";
                            cellarray[0, 25] = "SentiL1";
                            cellarray[0, 27] = "SentiL2";
                            cellarray[0, 28] = "AnalizPro";
                            cellarray[0, 29] = "Gkkul";
                            cellarray[0, 30] = "Tarama";
                            cellarray[0, 31] = "Gkkul";
                            cellarray[0, 32] = "DJI";
                            cellarray[0, 33] = "SPI";
                            cellarray[0, 34] = "XETRA";
                            cellarray[0, 35] = "CBOTM";
                            cellarray[0, 36] = "CMEM";
                            cellarray[0, 37] = "EUREX";
                            cellarray[0, 38] = "CBOT";
                            cellarray[0, 39] = "CME";
                            cellarray[0, 40] = "KurumMüşteriNo";
                            cellarray[0, 41] = "Not1";
                            cellarray[0, 42] = "KurumŞube";
                            cellarray[0, 43] = "BistPayı";
                            cellarray[0, 44] = "SozleşmeNo";
                            cellarray[0, 45] = "Şehir";


                            // data
                            for (int i = 0; i < customerlist.Count; i++)
                            {
                                var customeritem = customerlist[i];
                                cellarray[i + 1, 0] = cellarray[i + 1, 0] = customeritem.UserName;
                                cellarray[i + 1, 1] = cellarray[i + 1, 1] = customeritem.tckno;
                                cellarray[i + 1, 2] = cellarray[i + 1, 2] = customeritem.PmtsNo;
                                var adsoyad = "";
                                if (customeritem.Name != null)
                                    adsoyad += customeritem.Name;
                                if (customeritem.Surname != null)
                                    adsoyad += " " + customeritem.Surname;


                                cellarray[i + 1, 3] = adsoyad;
                                cellarray[i + 1, 4] = customeritem.Aciklama;
                                cellarray[i + 1, 4] = customeritem.ProductType;
                                cellarray[i + 1, 6] = (customeritem.StatusId != null) ? customeritem.Status.StatusAdi : "";
                                cellarray[i + 1, 7] = (customeritem.LisansDurum.YayinDurumu == true) ? "1" : "0";
                                cellarray[i + 1, 8] = (customeritem.LisansDurum.ProYetki == true) ? "1" : "0";
                                cellarray[i + 1, 9] = (customeritem.LisansDurum.CepYetki == true) ? "1" : "0";
                                cellarray[i + 1, 10] = (customeritem.LisansDurum.ROBOT == true) ? "1" : "0";
                                cellarray[i + 1, 11] = (customeritem.LisansDurum.PayL1 == true) ? "1" : "0";
                                cellarray[i + 1, 12] = (customeritem.LisansDurum.PayLP == true) ? "1" : "0";
                                cellarray[i + 1, 13] = (customeritem.LisansDurum.PayL2 == true) ? "1" : "0";
                                cellarray[i + 1, 14] = (customeritem.LisansDurum.Pd2P == true) ? "1" : "0";
                                cellarray[i + 1, 15] = (customeritem.LisansDurum.PayX == true) ? "1" : "0";
                                cellarray[i + 1, 16] = (customeritem.LisansDurum.PayGS == true) ? "1" : "0";
                                cellarray[i + 1, 17] = (customeritem.LisansDurum.PITE == true) ? "1" : "0";
                                cellarray[i + 1, 18] = (customeritem.LisansDurum.ViopL1 == true) ? "1" : "0";
                                cellarray[i + 1, 19] = (customeritem.LisansDurum.ViopLP == true) ? "1" : "0";
                                cellarray[i + 1, 20] = (customeritem.LisansDurum.ViopL2 == true) ? "1" : "0";
                                cellarray[i + 1, 21] = (customeritem.LisansDurum.Vd2P == true) ? "1" : "0";
                                cellarray[i + 1, 22] = (customeritem.LisansDurum.ViopGS == true) ? "1" : "0";
                                cellarray[i + 1, 23] = (customeritem.LisansDurum.TahvilL1 == true) ? "1" : "0";
                                cellarray[i + 1, 24] = (customeritem.LisansDurum.TahvilLP == true) ? "1" : "0";
                                cellarray[i + 1, 25] = (customeritem.LisansDurum.TahvilL2 == true) ? "1" : "0";
                                cellarray[i + 1, 26] = (customeritem.LisansDurum.SentiL1 == true) ? "1" : "0";
                                cellarray[i + 1, 27] = (customeritem.LisansDurum.SentiL2 == true) ? "1" : "0";
                                cellarray[i + 1, 28] = (customeritem.LisansDurum.AnPro == true) ? "1" : "0";
                                cellarray[i + 1, 29] = (customeritem.LisansDurum.MKK == true) ? "1" : "0";
                                cellarray[i + 1, 30] = (customeritem.LisansDurum.TARAMA == true) ? "1" : "0";
                                cellarray[i + 1, 31] = (customeritem.LisansDurum.GKKUL == true) ? "1" : "0";
                                cellarray[i + 1, 32] = (customeritem.LisansDurum.DJI == true) ? "1" : "0";
                                cellarray[i + 1, 33] = (customeritem.LisansDurum.SPI == true) ? "1" : "0";
                                cellarray[i + 1, 34] = (customeritem.LisansDurum.XETRA == true) ? "1" : "0";
                                cellarray[i + 1, 35] = (customeritem.LisansDurum.CBOTM == true) ? "1" : "0";
                                cellarray[i + 1, 36] = (customeritem.LisansDurum.CMEM == true) ? "1" : "0";
                                cellarray[i + 1, 37] = (customeritem.LisansDurum.EUREX == true) ? "1" : "0";
                                cellarray[i + 1, 38] = (customeritem.LisansDurum.CBOT == true) ? "1" : "0";
                                cellarray[i + 1, 39] = (customeritem.LisansDurum.CME == true) ? "1" : "0";
                                if (customeritem.KurumsalBilgilerId != null)
                                {
                                    cellarray[i + 1, 40] = (customeritem.KurumsalBilgiler.kurumhesapno != null) ? customeritem.KurumsalBilgiler.kurumhesapno : " ";
                                    cellarray[i + 1, 41] = (customeritem.KurumsalBilgiler.Not1 != null) ? customeritem.KurumsalBilgiler.Not1 : " ";
                                    cellarray[i + 1, 42] = (customeritem.KurumsalBilgiler.KurumSube != null) ? customeritem.KurumsalBilgiler.KurumSube : " ";

                                }

                                #region BistPayiHesapla


                                decimal toplam = 0;

                                #region Pay



                                decimal paytutar = 0;
                                decimal payendeks = 0;
                                decimal pite = 0;
                                decimal pit = 0;


                                if (customeritem.LisansDurum.PayL1 == true && customeritem.LisansDurum.PayLP == false && customeritem.LisansDurum.PayL2 == false && customeritem.LisansDurum.Pd2P == false)
                                {

                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        paytutar = MyTools.lisansfiyatlari.Fpd1;
                                    else
                                        paytutar = MyTools.lisansfiyatlari.Ypd1;
                                }
                                else if (customeritem.LisansDurum.PayL1 == true && customeritem.LisansDurum.PayLP == true && customeritem.LisansDurum.PayL2 == false && customeritem.LisansDurum.Pd2P == false)
                                {

                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        paytutar = MyTools.lisansfiyatlari.Fpd1p;
                                    else
                                        paytutar = MyTools.lisansfiyatlari.Ypd1p;


                                }
                                else if (customeritem.LisansDurum.PayL1 == true && customeritem.LisansDurum.PayLP == true && customeritem.LisansDurum.PayL2 == true && customeritem.LisansDurum.Pd2P == false)
                                {

                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        paytutar = MyTools.lisansfiyatlari.Fpd2;
                                    else
                                        paytutar = MyTools.lisansfiyatlari.Ypd2;


                                }
                                else if (customeritem.LisansDurum.PayL1 == true && customeritem.LisansDurum.PayLP == true && customeritem.LisansDurum.PayL2 == true && customeritem.LisansDurum.Pd2P == true)
                                {

                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        paytutar = MyTools.lisansfiyatlari.Fpd2p;
                                    else
                                        paytutar = MyTools.lisansfiyatlari.Ypd2p;


                                }
                                toplam += paytutar;
                                if (customeritem.LisansDurum.PayX == true)
                                {
                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        payendeks = MyTools.lisansfiyatlari.Fend;
                                    else
                                        payendeks = MyTools.lisansfiyatlari.Yend;

                                    toplam += payendeks;
                                }
                                if (customeritem.LisansDurum.PayGS == true)
                                {
                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        pit = MyTools.lisansfiyatlari.Fpit;
                                    else
                                        pit = MyTools.lisansfiyatlari.Ypit;
                                    toplam += pit;
                                }

                                if (customeritem.LisansDurum.PITE == true)
                                {
                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        pite = MyTools.lisansfiyatlari.Fpite;
                                    else
                                        pite = MyTools.lisansfiyatlari.Ypite;

                                    toplam += pite;
                                }

                                #endregion
                                #region Viop


                                decimal vioptutar = 0;
                                decimal vit = 0;


                                if (customeritem.LisansDurum.ViopL1 == true && customeritem.LisansDurum.ViopLP == false && customeritem.LisansDurum.ViopL2 == false && customeritem.LisansDurum.Vd2P == false)
                                {

                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        vioptutar = MyTools.lisansfiyatlari.Fvl1;
                                    else
                                        vioptutar = MyTools.lisansfiyatlari.Yvl1;
                                }
                                else if (customeritem.LisansDurum.ViopL1 == true && customeritem.LisansDurum.ViopLP == true && customeritem.LisansDurum.ViopL2 == false && customeritem.LisansDurum.Vd2P == false)
                                {

                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        vioptutar = MyTools.lisansfiyatlari.Fvl1p;
                                    else
                                        vioptutar = MyTools.lisansfiyatlari.Yvl1p;

                                }
                                else if (customeritem.LisansDurum.ViopL1 == true && customeritem.LisansDurum.ViopLP == true && customeritem.LisansDurum.ViopL2 == true && customeritem.LisansDurum.Vd2P == false)
                                {

                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        vioptutar = MyTools.lisansfiyatlari.Fvl2;
                                    else
                                        vioptutar = MyTools.lisansfiyatlari.Yvl2;

                                }
                                else if (customeritem.LisansDurum.ViopL1 == true && customeritem.LisansDurum.ViopLP == true && customeritem.LisansDurum.ViopL2 == true && customeritem.LisansDurum.Vd2P == true)
                                {

                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        vioptutar = MyTools.lisansfiyatlari.Fvl2p;
                                    else
                                        vioptutar = MyTools.lisansfiyatlari.Yvl2p;

                                }
                                toplam += vioptutar;

                                if (customeritem.LisansDurum.ViopGS == true)
                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        vit = MyTools.lisansfiyatlari.Fvit;
                                    else
                                        vit = MyTools.lisansfiyatlari.Yvit;

                                toplam += vit;
                                #endregion
                                #region Tahvil
                                decimal Tahviltutar = 0;

                                if (customeritem.LisansDurum.TahvilL1 == true && customeritem.LisansDurum.TahvilLP == false && customeritem.LisansDurum.TahvilL2 == false)
                                {
                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        Tahviltutar = MyTools.lisansfiyatlari.Fbd1;
                                    else
                                        Tahviltutar = MyTools.lisansfiyatlari.Ybd1;
                                }
                                else if (customeritem.LisansDurum.TahvilL1 == true && customeritem.LisansDurum.TahvilLP == true && customeritem.LisansDurum.TahvilL2 == false)
                                {
                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        Tahviltutar = MyTools.lisansfiyatlari.Fbd1p;
                                    else
                                        Tahviltutar = MyTools.lisansfiyatlari.Ybd1p;
                                }
                                else if (customeritem.LisansDurum.TahvilL1 == true && customeritem.LisansDurum.TahvilLP == true && customeritem.LisansDurum.TahvilL2 == true)
                                {
                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        Tahviltutar = MyTools.lisansfiyatlari.Fbd2;
                                    else
                                        Tahviltutar = MyTools.lisansfiyatlari.Ybd2;
                                }

                                toplam += Tahviltutar;
                                #endregion
                                #region senti
                                decimal Sentitutar = 0;

                                if (customeritem.LisansDurum.SentiL1 == true && customeritem.LisansDurum.SentiL2 == false)
                                {
                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        Sentitutar = MyTools.lisansfiyatlari.FSentiL1;
                                    else
                                        Sentitutar = MyTools.lisansfiyatlari.YSentiL1;
                                }
                                else if (customeritem.LisansDurum.SentiL1 == true && customeritem.LisansDurum.SentiL2 == true)
                                {
                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        Sentitutar = MyTools.lisansfiyatlari.FSentiL2;
                                    else
                                        Sentitutar = MyTools.lisansfiyatlari.YSentiL2;
                                }

                                toplam += Sentitutar;
                                #endregion
                                #region analizPro
                                decimal anProTutar = 0;

                                if (customeritem.LisansDurum.AnPro == true)
                                {
                                    if (customeritem.MusteriMenseiID == null || customeritem.MusteriMenseiID == 1)
                                        anProTutar = MyTools.lisansfiyatlari.FanPro;
                                    else
                                        anProTutar = MyTools.lisansfiyatlari.YanPro;
                                }


                                toplam += anProTutar;
                                #endregion



                                #endregion
                                cellarray[i + 1, 43] = toplam.ToString("0.00");
                                if (customeritem.SozlesmeID != null)
                                    cellarray[i + 1, 44] = customeritem.Sozlesmeler.SozlesmeNo;

                                if (customeritem.iletisimId != null)
                                {
                                    if (customeritem.Iletisim.IlId != null)
                                    {
                                        cellarray[i + 1, 45] = customeritem.Iletisim.Il.IlAdi;
                                    }
                                }
                                else
                                    cellarray[i + 1, 44] = "";
                            }

                            ExcellApp.ScreenUpdating = false;
                            if (ExcellApp.Visible)
                            {
                                Microsoft.Office.Interop.Excel.Range range = excelsheet.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                                range.Value = cellarray;
                                range.Columns.AutoFit();
                            }
                            ExcellApp.ScreenUpdating = true;
                            break;
                        default:
                            break;
                    }
                }
                catch
                {
                    ExcellApp.ScreenUpdating = true;
                }
            }
            public static void OlaylarTarihRapor(DateTime tarihX)
            {
                ExcelDurumFormAc();
                var thread = new Thread(() =>
                {

                    try
                    {
                        crmDFNDataContext crm = new crmDFNDataContext();
                        var date = tarihX;
                        var guncelay = date.ToString("MMMMMMMMM").ToUpper().Trim();
                        var dosyaadi = "OLAYRAPOR_" + guncelay + "" + date.Year.ToString() + ".XLSX";
                        var yol = Application.StartupPath + "\\RAPOR";
                        if (!Directory.Exists(yol))
                            Directory.CreateDirectory(yol);



                        string filename = yol + "\\" + dosyaadi;
                        if (File.Exists(filename)) File.Delete(filename);
                        ExcelDurumYaz(dosyaadi + " Oluşturuluyor....");
                        var otarihdekiler = crm.UserEvents.Where(x => x.EventTarih.Value.Date <= tarihX.Date && x.LisansDurum != null).ToList();

                        // var sorgu1 = (from n in otarihdekiler group n by n.User.UserName into g select new { tarih = g.OrderByDescending(x => x.EventTarih).FirstOrDefault() }).Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.EventTypeId.Value != 2).ToList();
                        var sorgu1 = (from n in otarihdekiler group n by n.User.tckno into g select new { tarih = g.OrderByDescending(x => x.EventTarih).FirstOrDefault() }).Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.EventTypeId.Value != 2).ToList();
                        var sorgu = sorgu1.Where(x => x.tarih.EventTarih.Value.Date <= tarihX.Date).Select(x =>
                        new
                        {
                            x.tarih.EventId,
                            x.tarih.EventTarih.Value.Date,
                            x.tarih.User.PmtsNo,
                            // x.tarih.User.UserName,
                            x.tarih.User.tckno,
                            islemiYapan = x.tarih.Calisan.Ad + "" + x.tarih.Calisan.Soyad,
                            x.tarih.HostName,
                            x.tarih.EventType.EventName,
                            x.tarih.AcilanLisans,
                            x.tarih.KapatilanLisans,
                            x.tarih.LisansDurum.YayinDurumu,
                            x.tarih.User.Status.StatusAdi,
                            x.tarih.LisansDurum.ROBOT,
                            x.tarih.LisansDurum.ProYetki,
                            x.tarih.LisansDurum.CepYetki,
                            x.tarih.LisansDurum.COMEX,
                            x.tarih.LisansDurum.PayL1,
                            x.tarih.LisansDurum.PayLP,
                            x.tarih.LisansDurum.PayL2,
                            x.tarih.LisansDurum.Pd2P,
                            x.tarih.LisansDurum.PayX,
                            x.tarih.LisansDurum.PayGS,
                            x.tarih.LisansDurum.PITE,
                            x.tarih.LisansDurum.ViopL1,
                            x.tarih.LisansDurum.ViopLP,
                            x.tarih.LisansDurum.ViopL2,
                            x.tarih.LisansDurum.Vd2P,
                            x.tarih.LisansDurum.ViopGS,
                            x.tarih.LisansDurum.TahvilL1,
                            x.tarih.LisansDurum.TahvilLP,
                            x.tarih.LisansDurum.TahvilL2,
                            x.tarih.LisansDurum.MKK,
                            x.tarih.LisansDurum.TARAMA,
                            x.tarih.LisansDurum.GKKUL
                        }).ToList();



                        var KayitSayisi = sorgu.Count;

                        var Cep = sorgu.Where(x => x.YayinDurumu == true && x.CepYetki == true).Count();
                        var pro = sorgu.Where(x => x.YayinDurumu == true && x.ProYetki == true).Count();
                        var robot = sorgu.Where(x => x.YayinDurumu == true && x.ROBOT == true).Count();
                        var ProCep = sorgu.Where(x => x.YayinDurumu == true && x.CepYetki == true && x.ProYetki == true).Count();
                        var karma = sorgu.Where(x => x.YayinDurumu == true && x.COMEX == true).Count();
                        var mkk = sorgu.Where(x => x.YayinDurumu == true && x.MKK == true).Count();
                        var tarama = sorgu.Where(x => x.YayinDurumu == true && x.TARAMA == true).Count();
                        var gkkul = sorgu.Where(x => x.YayinDurumu == true && x.GKKUL == true).Count();
                        var PayYuzeysel = sorgu.Where(x => x.YayinDurumu == true && x.PayL1 == true && x.PayL2 == false && x.PayLP == false).Count();
                        var PayPlus = sorgu.Where(x => x.YayinDurumu == true && x.PayL1 == true && x.PayL2 == false && x.PayLP == true).Count();
                        var PayDerinlik = sorgu.Where(x => x.YayinDurumu == true && x.PayL1 == true && x.PayL2 == true && x.PayLP == true && x.Pd2P == false).Count();
                        var PayDerinlikPlus = sorgu.Where(x => x.YayinDurumu == true && x.PayL1 == true && x.PayL2 == true && x.PayLP == true && x.Pd2P == true).Count();
                        var PayEndeks = sorgu.Where(x => x.YayinDurumu == true && x.PayX == true).Count();
                        var PayGs = sorgu.Where(x => x.YayinDurumu == true && x.PayGS == true).Count();
                        var Pite = sorgu.Where(x => x.YayinDurumu == true && x.PITE == true).Count();
                        var viopYuzeysel = sorgu.Where(x => x.YayinDurumu == true && x.ViopL1 == true && x.ViopL2 == false && x.ViopLP == false).Count();
                        var viopPlus = sorgu.Where(x => x.YayinDurumu == true && x.ViopL1 == true && x.ViopL2 == false && x.ViopLP == true).Count();
                        var viopDerinlik = sorgu.Where(x => x.YayinDurumu == true && x.ViopL1 == true && x.ViopL2 == true && x.ViopLP == true && x.Vd2P == false).Count();
                        var viopDerinlikPlus = sorgu.Where(x => x.YayinDurumu == true && x.ViopL1 == true && x.ViopL2 == true && x.ViopLP == true && x.Vd2P == true).Count();
                        var viopGS = sorgu.Where(x => x.YayinDurumu == true && x.ViopGS == true).Count();

                        var TahvilYuzeysel = sorgu.Where(x => x.YayinDurumu == true && x.TahvilL1 == true && x.TahvilL2 == false && x.TahvilLP == false).Count();
                        var TahvilPlus = sorgu.Where(x => x.YayinDurumu == true && x.TahvilL1 == true && x.TahvilL2 == false && x.TahvilLP == true).Count();
                        var TahvilDerinlik = sorgu.Where(x => x.YayinDurumu == true && x.TahvilL1 == true && x.TahvilL2 == true && x.TahvilLP == true).Count();

                        var On = sorgu.Where(x => x.YayinDurumu == true).Count();
                        var Off = sorgu.Where(x => x.YayinDurumu == false).Count();


                        var sorgu2 = sorgu.Where(x => x.StatusAdi == "ISE" || x.StatusAdi == "MUAF").ToList();

                        var Cep2 = sorgu2.Where(x => x.YayinDurumu == true && x.CepYetki == true).Count();
                        var pro2 = sorgu2.Where(x => x.YayinDurumu == true && x.ProYetki == true).Count();
                        var robot2 = sorgu2.Where(x => x.YayinDurumu == true && x.ROBOT == true).Count();
                        var ProCep2 = sorgu2.Where(x => x.YayinDurumu == true && x.CepYetki == true && x.ProYetki == true).Count();
                        var karma2 = sorgu2.Where(x => x.YayinDurumu == true && x.COMEX == true).Count();
                        var mkk2 = sorgu2.Where(x => x.YayinDurumu == true && x.MKK == true).Count();
                        var gkkul2 = sorgu2.Where(x => x.YayinDurumu == true && x.GKKUL == true).Count();
                        var tarama2 = sorgu2.Where(x => x.YayinDurumu == true && x.TARAMA == true).Count();
                        var PayYuzeysel2 = sorgu2.Where(x => x.YayinDurumu == true && x.PayL1 == true && x.PayL2 == false && x.PayLP == false).Count();
                        var PayPlus2 = sorgu2.Where(x => x.YayinDurumu == true && x.PayL1 == true && x.PayL2 == false && x.PayLP == true).Count();
                        var PayDerinlik2 = sorgu2.Where(x => x.YayinDurumu == true && x.PayL1 == true && x.PayL2 == true && x.PayLP == true && x.Pd2P == false).Count();
                        var PayDerinlikPlus2 = sorgu2.Where(x => x.YayinDurumu == true && x.PayL1 == true && x.PayL2 == true && x.PayLP == true && x.Pd2P == true).Count();
                        var PayEndeks2 = sorgu2.Where(x => x.YayinDurumu == true && x.PayX == true).Count();
                        var PayGs2 = sorgu2.Where(x => x.YayinDurumu == true && x.PayGS == true).Count();
                        var Pite2 = sorgu2.Where(x => x.YayinDurumu == true && x.PITE == true).Count();
                        var viopYuzeysel2 = sorgu2.Where(x => x.YayinDurumu == true && x.ViopL1 == true && x.ViopL2 == false && x.ViopLP == false).Count();
                        var viopPlus2 = sorgu2.Where(x => x.YayinDurumu == true && x.ViopL1 == true && x.ViopL2 == false && x.ViopLP == true).Count();
                        var viopDerinlik2 = sorgu2.Where(x => x.YayinDurumu == true && x.ViopL1 == true && x.ViopL2 == true && x.ViopLP == true && x.Vd2P == false).Count();
                        var viopDerinlikPlus2 = sorgu2.Where(x => x.YayinDurumu == true && x.ViopL1 == true && x.ViopL2 == true && x.ViopLP == true && x.Vd2P == true).Count();
                        var viopGS2 = sorgu2.Where(x => x.YayinDurumu == true && x.ViopGS == true).Count();

                        var TahvilYuzeysel2 = sorgu2.Where(x => x.YayinDurumu == true && x.TahvilL1 == true && x.TahvilL2 == false && x.TahvilLP == false).Count();
                        var TahvilPlus2 = sorgu2.Where(x => x.YayinDurumu == true && x.TahvilL1 == true && x.TahvilL2 == false && x.TahvilLP == true).Count();
                        var TahvilDerinlik2 = sorgu2.Where(x => x.YayinDurumu == true && x.TahvilL1 == true && x.TahvilL2 == true && x.TahvilLP == true).Count();

                        var On2 = sorgu2.Where(x => x.YayinDurumu == true).Count();
                        var Off2 = sorgu2.Where(x => x.YayinDurumu == false).Count();




                        var excelworkbook = GetExcelWorkbook(filename);
                        if (excelworkbook != null)
                        {
                            //excelworkbook.Save();
                            excelworkbook.Close(false);
                        }
                        excelworkbook = OpenExcelWorkbook(filename);







                        var excelsheetilk = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.Add();
                        excelsheetilk.Name = "Detay " + guncelay + " " + date.Year;

                        var cellarray = new object[KayitSayisi + 2, 30];
                        var currrow = 0;
                        var currcol = 0;

                        cellarray[currrow, currcol] = "Olay Id"; currcol++;
                        cellarray[currrow, currcol] = "Tarih"; currcol++;
                        cellarray[currrow, currcol] = "PmtsNo"; currcol++;
                        //cellarray[currrow, currcol] = "Username"; currcol++;
                        cellarray[currrow, currcol] = "TCKN"; currcol++;
                        cellarray[currrow, currcol] = "Status"; currcol++;
                        cellarray[currrow, currcol] = "İşlemi Yapan"; currcol++;
                        cellarray[currrow, currcol] = "Host"; currcol++;
                        cellarray[currrow, currcol] = "OlayTanım"; currcol++;
                        cellarray[currrow, currcol] = "Açılan Lisans"; currcol++;
                        cellarray[currrow, currcol] = "Kapanan Lisans"; currcol++;
                        cellarray[currrow, currcol] = "KRMD1"; currcol++;
                        cellarray[currrow, currcol] = "MKK"; currcol++;
                        cellarray[currrow, currcol] = "TARAMA"; currcol++;
                        cellarray[currrow, currcol] = "GKKUL"; currcol++;
                        cellarray[currrow, currcol] = "PD1"; currcol++;
                        cellarray[currrow, currcol] = "PD1P"; currcol++;
                        cellarray[currrow, currcol] = "PD2"; currcol++;
                        cellarray[currrow, currcol] = "PD2P"; currcol++;
                        cellarray[currrow, currcol] = "PIT"; currcol++;
                        cellarray[currrow, currcol] = "PITE"; currcol++;
                        cellarray[currrow, currcol] = "END"; currcol++;
                        cellarray[currrow, currcol] = "VD1"; currcol++;
                        cellarray[currrow, currcol] = "VD1P"; currcol++;
                        cellarray[currrow, currcol] = "VD2"; currcol++;
                        cellarray[currrow, currcol] = "VD2P"; currcol++;
                        cellarray[currrow, currcol] = "VIT"; currcol++;
                        cellarray[currrow, currcol] = "BD1"; currcol++;
                        cellarray[currrow, currcol] = "BD1P"; currcol++;
                        cellarray[currrow, currcol] = "BD2";
                        currrow++;

                        Microsoft.Office.Interop.Excel.Range baslikrange2 = excelsheetilk.Application.get_Range("A1", "X1");
                        baslikrange2.Font.Bold = true;
                        baslikrange2.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(220, 230, 241));
                        baslikrange2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        baslikrange2.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                        baslikrange2.RowHeight = 40;
                        baslikrange2.Application.ActiveWindow.SplitRow = 1;
                        baslikrange2.Application.ActiveWindow.FreezePanes = true;
                        baslikrange2.Borders.Value = true;

                        for (int i = 0; i < KayitSayisi; i++)
                        {
                            currcol = 0;
                            var olay = sorgu[i];
                            ExcelDurumYaz("Detay Oluşturuluyor...." + currrow + "/" + KayitSayisi);
                            cellarray[currrow, currcol] = olay.EventId; currcol++;
                            cellarray[currrow, currcol] = olay.Date; currcol++;
                            cellarray[currrow, currcol] = olay.PmtsNo; currcol++;
                            //cellarray[currrow, currcol] = olay.UserName; currcol++;
                            cellarray[currrow, currcol] = olay.tckno; currcol++;
                            cellarray[currrow, currcol] = olay.StatusAdi; currcol++;
                            cellarray[currrow, currcol] = olay.islemiYapan; currcol++;
                            cellarray[currrow, currcol] = olay.HostName; currcol++;
                            cellarray[currrow, currcol] = olay.EventName; currcol++;
                            cellarray[currrow, currcol] = olay.AcilanLisans; currcol++;
                            cellarray[currrow, currcol] = olay.KapatilanLisans; currcol++;
                            cellarray[currrow, currcol] = olay.COMEX._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.MKK._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.PayL1._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.PayLP._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.PayL2._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.Pd2P._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.PayGS._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.PITE._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.PayX._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.ViopL1._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.ViopLP._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.ViopL2._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.Vd2P._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.ViopGS._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.TahvilL1._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.TahvilLP._ToIntStr(); currcol++;
                            cellarray[currrow, currcol] = olay.TahvilL2._ToIntStr();
                            cellarray[currrow, currcol] = olay.TARAMA._ToIntStr();
                            cellarray[currrow, currcol] = olay.GKKUL._ToIntStr();
                            currrow++;

                        }

                        Microsoft.Office.Interop.Excel.Range range2 = excelsheetilk.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                        range2.Value = cellarray;
                        range2.Columns.AutoFit();

                        ExcelDurumYaz("Detay Sayfa Aktarımı Bitti");

                        var excelsheetson = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.Add();
                        excelsheetson.Name = "Özet";


                        cellarray = new object[25, 5];
                        currrow = 0;
                        currcol = 0;

                        cellarray[currrow, currcol] = "Linsans Kodu"; cellarray[currrow, currcol + 1] = "Lisans Adet"; cellarray[currrow, currcol + 2] = "Bildirilen\nLisans Adet"; currrow++;
                        cellarray[currrow, currcol] = "PRO"; cellarray[currrow, currcol + 1] = pro.ToString(); cellarray[currrow, currcol + 2] = pro2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "CEP"; cellarray[currrow, currcol + 1] = Cep.ToString(); cellarray[currrow, currcol + 2] = Cep2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "PROCEP"; cellarray[currrow, currcol + 1] = ProCep.ToString(); cellarray[currrow, currcol + 2] = ProCep2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "KRMD1"; cellarray[currrow, currcol + 1] = karma.ToString(); cellarray[currrow, currcol + 2] = karma2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "MKK"; cellarray[currrow, currcol + 1] = mkk.ToString(); cellarray[currrow, currcol + 2] = mkk2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "TARAMA"; cellarray[currrow, currcol + 1] = tarama.ToString(); cellarray[currrow, currcol + 2] = tarama2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "GKKUL"; cellarray[currrow, currcol + 1] = gkkul.ToString(); cellarray[currrow, currcol + 2] = gkkul2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "PD1"; cellarray[currrow, currcol + 1] = PayYuzeysel.ToString(); cellarray[currrow, currcol + 2] = PayYuzeysel2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "PD1P"; cellarray[currrow, currcol + 1] = PayPlus.ToString(); cellarray[currrow, currcol + 2] = PayPlus2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "PD2"; cellarray[currrow, currcol + 1] = PayDerinlik.ToString(); cellarray[currrow, currcol + 2] = PayDerinlik2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "PD2P"; cellarray[currrow, currcol + 1] = PayDerinlikPlus.ToString(); cellarray[currrow, currcol + 2] = PayDerinlikPlus2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "PIT"; cellarray[currrow, currcol + 1] = PayGs.ToString(); cellarray[currrow, currcol + 2] = PayGs2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "PITE"; cellarray[currrow, currcol + 1] = Pite.ToString(); cellarray[currrow, currcol + 2] = Pite2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "END"; cellarray[currrow, currcol + 1] = PayEndeks.ToString(); cellarray[currrow, currcol + 2] = PayEndeks2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "VD1"; cellarray[currrow, currcol + 1] = viopYuzeysel.ToString(); cellarray[currrow, currcol + 2] = viopYuzeysel2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "VD1P"; cellarray[currrow, currcol + 1] = viopPlus.ToString(); cellarray[currrow, currcol + 2] = viopPlus2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "VD2"; cellarray[currrow, currcol + 1] = viopDerinlik.ToString(); cellarray[currrow, currcol + 2] = viopDerinlik2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "VD2P"; cellarray[currrow, currcol + 1] = viopDerinlikPlus.ToString(); cellarray[currrow, currcol + 2] = viopDerinlikPlus2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "VIT"; cellarray[currrow, currcol + 1] = viopGS.ToString(); cellarray[currrow, currcol + 2] = viopGS2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "BD1"; cellarray[currrow, currcol + 1] = TahvilYuzeysel.ToString(); cellarray[currrow, currcol + 2] = TahvilYuzeysel2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "BD1P"; cellarray[currrow, currcol + 1] = TahvilPlus.ToString(); cellarray[currrow, currcol + 2] = TahvilPlus2.ToString(); currrow++;
                        cellarray[currrow, currcol] = "BD2"; cellarray[currrow, currcol + 1] = TahvilDerinlik.ToString(); cellarray[currrow, currcol + 2] = TahvilDerinlik2.ToString(); currrow++;



                        baslikrange2 = excelsheetson.Application.get_Range("A1", "C1");
                        baslikrange2.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(220, 230, 241));
                        baslikrange2 = excelsheetson.Application.get_Range("A1", "C" + currrow);
                        baslikrange2.Font.Bold = true;
                        baslikrange2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        baslikrange2.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                        baslikrange2.Borders.Value = true;

                        range2 = excelsheetson.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                        range2.Value = cellarray;
                        range2.Columns.AutoFit();

                        ExcelDurumYaz("Aktarımı Bitti");

                        excelworkbook.Save();

                    }
                    catch (Exception ex)
                    {
                        ExcelDurumYaz("Hata Oluştu\n" + ex.Message);
                    }

                });
                thread.Start();
            }
         
            public static void BorsaOzetlisteToExcel()
            {
                try
                {
                    crmDFNDataContext crm = new crmDFNDataContext();
                    var Sorgu = crm.Users.Where(x => x.StatusId == 1);

                    var filename = System.Windows.Forms.Application.StartupPath + "\\" + "Ozet" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".XLSX";
                    if (File.Exists(filename)) File.Delete(filename);
                    var excelworkbook = GetExcelWorkbook(filename);
                    if (excelworkbook != null)
                    {
                        excelworkbook.Save();
                        excelworkbook.Close(false);
                    }
                    excelworkbook = OpenExcelWorkbook(filename);
                    if (excelworkbook == null) return;

                    var excelsheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.get_Item(1);

                    List<alanlar> alanlar = new List<alanlar>();


                    var PayYuzeysel = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false && x.LisansDurum.Pd2P == false).Count();
                    alanlar.Add(new alanlar { bilgikodu = "PD1", mensei = "Yurtiçi", cihaztoplam = PayYuzeysel.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpd1.ToString("0.00"), toplambedel = (PayYuzeysel * MyTools.lisansfiyatlari.Fpd1).ToString("0.00") });
                    var PayYuzeysel_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false && x.LisansDurum.Pd2P == false).Count();
                    if (PayYuzeysel_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "PD1", mensei = "Yurtdışı", cihaztoplam = PayYuzeysel_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypd1.ToString("0.00"), toplambedel = (PayYuzeysel_Y * MyTools.lisansfiyatlari.Ypd1).ToString("0.00") });


                    var PayPlus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                    alanlar.Add(new alanlar { bilgikodu = "PD1P", mensei = "Yurtiçi", cihaztoplam = PayPlus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpd1p.ToString("0.00"), toplambedel = (PayPlus * MyTools.lisansfiyatlari.Fpd1p).ToString("0.00") });
                    var PayPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                    if (PayPlus_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "PD1P", mensei = "Yurtdışı", cihaztoplam = PayPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypd1p.ToString("0.00"), toplambedel = (PayPlus_Y * MyTools.lisansfiyatlari.Ypd1p).ToString("0.00") });


                    var PayDerinlik = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                    alanlar.Add(new alanlar { bilgikodu = "PD2", mensei = "Yurtiçi", cihaztoplam = PayDerinlik.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpd2.ToString("0.00"), toplambedel = (PayDerinlik * MyTools.lisansfiyatlari.Fpd2).ToString("0.00") });
                    var PayDerinlik_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                    if (PayDerinlik_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "PD2", mensei = "Yurtdışı", cihaztoplam = PayDerinlik_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypd2.ToString("0.00"), toplambedel = (PayDerinlik_Y * MyTools.lisansfiyatlari.Ypd2).ToString("0.00") });

                    var PayDerinlikPlus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "PD2P", mensei = "Yurtiçi", cihaztoplam = PayDerinlikPlus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpd2p.ToString("0.00"), toplambedel = (PayDerinlikPlus * MyTools.lisansfiyatlari.Fpd2p).ToString("0.00") });
                    var PayDerinlikPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == true).Count();
                    if (PayDerinlikPlus_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "PD2P", mensei = "Yurtdışı", cihaztoplam = PayDerinlikPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypd2p.ToString("0.00"), toplambedel = (PayDerinlikPlus_Y * MyTools.lisansfiyatlari.Ypd2p).ToString("0.00") });

                    var PayEndeks = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayX == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "END", mensei = "Yurtiçi", cihaztoplam = PayEndeks.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fend.ToString("0.00"), toplambedel = (PayEndeks * MyTools.lisansfiyatlari.Fend).ToString("0.00") });
                    var PayEndeks_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayX == true).Count();
                    if (PayEndeks_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "END", mensei = "Yurtdışı", cihaztoplam = PayEndeks_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yend.ToString("0.00"), toplambedel = (PayEndeks_Y * MyTools.lisansfiyatlari.Yend).ToString("0.00") });


                    var PayGs = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayGS == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "PIT", mensei = "Yurtiçi", cihaztoplam = PayGs.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpit.ToString("0.00"), toplambedel = (PayGs * MyTools.lisansfiyatlari.Fpit).ToString("0.00") });
                    var PayGs_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayGS == true).Count();
                    if (PayGs_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "PIT", mensei = "Yurtdışı", cihaztoplam = PayGs_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypit.ToString("0.00"), toplambedel = (PayGs_Y * MyTools.lisansfiyatlari.Ypit).ToString("0.00") });


                    var pite = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PITE == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "PITE", mensei = "Yurtiçi", cihaztoplam = pite.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpite.ToString("0.00"), toplambedel = (pite * MyTools.lisansfiyatlari.Fpite).ToString("0.00") });
                    var pite_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PITE == true).Count();
                    if (pite_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "PITE", mensei = "Yurtdışı", cihaztoplam = pite_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypva.ToString("0.00"), toplambedel = (pite_Y * MyTools.lisansfiyatlari.Ypite).ToString("0.00") });


                    var viopYuzeysel = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false && x.LisansDurum.Vd2P == false).Count();
                    alanlar.Add(new alanlar { bilgikodu = "VD1", mensei = "Yurtiçi", cihaztoplam = viopYuzeysel.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvl1.ToString("0.00"), toplambedel = (viopYuzeysel * MyTools.lisansfiyatlari.Fvl1).ToString("0.00") });
                    var viopYuzeysel_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false).Count();
                    if (viopYuzeysel_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "VD1", mensei = "Yurtdışı", cihaztoplam = viopYuzeysel_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvl1.ToString("0.00"), toplambedel = (viopYuzeysel_Y * MyTools.lisansfiyatlari.Yvl1).ToString("0.00") });




                    var viopPlus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                    alanlar.Add(new alanlar { bilgikodu = "VD1P", mensei = "Yurtiçi", cihaztoplam = viopPlus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvl1p.ToString("0.00"), toplambedel = (viopPlus * MyTools.lisansfiyatlari.Fvl1p).ToString("0.00") });
                    var viopPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                    if (viopPlus_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "VD1P", mensei = "Yurtdışı", cihaztoplam = viopPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvl1p.ToString("0.00"), toplambedel = (viopPlus_Y * MyTools.lisansfiyatlari.Yvl1p).ToString("0.00") });



                    var viopDerinlik = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                    alanlar.Add(new alanlar { bilgikodu = "VD2", mensei = "Yurtiçi", cihaztoplam = viopDerinlik.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvl2.ToString("0.00"), toplambedel = (viopDerinlik * MyTools.lisansfiyatlari.Fvl2).ToString("0.00") });
                    var viopDerinlik_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                    if (viopDerinlik_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "VD2", mensei = "Yurtdışı", cihaztoplam = viopDerinlik_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvl2.ToString("0.00"), toplambedel = (viopDerinlik_Y * MyTools.lisansfiyatlari.Yvl2).ToString("0.00") });


                    var viopDerinlikPlus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "VD2P", mensei = "Yurtiçi", cihaztoplam = viopDerinlikPlus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvl2p.ToString("0.00"), toplambedel = (viopDerinlikPlus * MyTools.lisansfiyatlari.Fvl2p).ToString("0.00") });

                    var viopDerinlikPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == true).Count();
                    if (viopDerinlikPlus_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "VD2P", mensei = "Yurtdışı", cihaztoplam = viopDerinlikPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvl2.ToString("0.00"), toplambedel = (viopDerinlikPlus_Y * MyTools.lisansfiyatlari.Yvl2p).ToString("0.00") });



                    var viopGS = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "VIT", mensei = "Yurtiçi", cihaztoplam = viopGS.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvit.ToString("0.00"), toplambedel = (viopGS * MyTools.lisansfiyatlari.Fvit).ToString("0.00") });
                    var viopGS_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();
                    if (viopGS_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "VIT", mensei = "Yurtdışı", cihaztoplam = viopGS_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvit.ToString("0.00"), toplambedel = (viopGS_Y * MyTools.lisansfiyatlari.Yvit).ToString("0.00") });

                    var karmaDuzey1 = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.COMEX == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "KRMD1", mensei = "Yurtiçi", cihaztoplam = karmaDuzey1.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fkrmd1.ToString("0.00"), toplambedel = (karmaDuzey1 * MyTools.lisansfiyatlari.Fkrmd1).ToString("0.00") });
                    var karmaDuzey1_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.COMEX == true).Count();
                    if (karmaDuzey1_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "KRMD1", mensei = "Yurtdışı", cihaztoplam = karmaDuzey1_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ykrmd1.ToString("0.00"), toplambedel = (karmaDuzey1_Y * MyTools.lisansfiyatlari.Ykrmd1).ToString("0.00") });

                    var mkk = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.MKK == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "MKK", mensei = "Yurtiçi", cihaztoplam = mkk.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fmkk.ToString("0.00"), toplambedel = (mkk * MyTools.lisansfiyatlari.Fmkk).ToString("0.00") });
                    var mkk_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.MKK == true).Count();
                    if (mkk_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "MKK", mensei = "Yurtdışı", cihaztoplam = mkk_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ymkk.ToString("0.00"), toplambedel = (mkk_Y * MyTools.lisansfiyatlari.Ymkk).ToString("0.00") });

                    var gkkul = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.GKKUL == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "GKKUL", mensei = "Yurtiçi", cihaztoplam = gkkul.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fgkkul.ToString("0.00"), toplambedel = (gkkul * MyTools.lisansfiyatlari.Fgkkul).ToString("0.00") });
                    var gkkul_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.GKKUL == true).Count();
                    if (gkkul_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "GKKUL", mensei = "Yurtdışı", cihaztoplam = gkkul_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ygkkul.ToString("0.00"), toplambedel = (gkkul_Y * MyTools.lisansfiyatlari.Ygkkul).ToString("0.00") });

                    var tarama = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TARAMA == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "TARAMA", mensei = "Yurtiçi", cihaztoplam = tarama.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ftarama.ToString("0.00"), toplambedel = (tarama * MyTools.lisansfiyatlari.Ftarama).ToString("0.00") });
                    var tarama_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TARAMA == true).Count();
                    if (tarama_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "TARAMA", mensei = "Yurtdışı", cihaztoplam = tarama_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ytarama.ToString("0.00"), toplambedel = (tarama_Y * MyTools.lisansfiyatlari.Ytarama).ToString("0.00") });




                    var TahvilYuzeysel = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false).Count();
                    alanlar.Add(new alanlar { bilgikodu = "BD1", mensei = "Yurtiçi", cihaztoplam = TahvilYuzeysel.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fbd1.ToString("0.00"), toplambedel = (TahvilYuzeysel * MyTools.lisansfiyatlari.Fbd1).ToString("0.00") });
                    var TahvilYuzeysel_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false).Count();
                    if (TahvilYuzeysel_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "BD1", mensei = "Yurtdışı", cihaztoplam = TahvilYuzeysel_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ybd1.ToString("0.00"), toplambedel = (TahvilYuzeysel_Y * MyTools.lisansfiyatlari.Ybd1).ToString("0.00") });



                    var TahvilPlus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "BD1P", mensei = "Yurtiçi", cihaztoplam = TahvilPlus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fbd1p.ToString("0.00"), toplambedel = (TahvilPlus * MyTools.lisansfiyatlari.Fbd1p).ToString("0.00") });
                    var TahvilPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == true).Count();
                    if (TahvilPlus_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "BD1P", mensei = "Yurtdışı", cihaztoplam = TahvilPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ybd1p.ToString("0.00"), toplambedel = (TahvilPlus_Y * MyTools.lisansfiyatlari.Ybd1p).ToString("0.00") });



                    var TahvilDerinlik = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == true && x.LisansDurum.TahvilLP == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "BD2", mensei = "Yurtiçi", cihaztoplam = TahvilDerinlik.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fbd2.ToString("0.00"), toplambedel = (TahvilDerinlik * MyTools.lisansfiyatlari.Fbd2).ToString("0.00") });
                    var TahvilDerinlik_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == true && x.LisansDurum.TahvilLP == true).Count();
                    if (TahvilDerinlik_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "BD2", mensei = "Yurtdışı", cihaztoplam = TahvilDerinlik_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ybd2.ToString("0.00"), toplambedel = (TahvilDerinlik_Y * MyTools.lisansfiyatlari.Ybd2).ToString("0.00") });

                    var AnPro = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.AnPro == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "AnalizPro", mensei = "Yurtiçi", cihaztoplam = AnPro.ToString(), bilgibedel = MyTools.lisansfiyatlari.FanPro.ToString("0.00"), toplambedel = (AnPro * MyTools.lisansfiyatlari.FanPro).ToString("0.00") });
                    var AnPro_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.AnPro == true).Count();
                    if (AnPro_Y > 0)
                        alanlar.Add(new alanlar { bilgikodu = "AnalizPro", mensei = "Yurtdışı", cihaztoplam = AnPro_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.YanPro.ToString("0.00"), toplambedel = (AnPro_Y * MyTools.lisansfiyatlari.YanPro).ToString("0.00") });


                    var cellarray = new object[alanlar.Count + 20, 5];
                    for (int i = 0; i < cellarray.GetLength(0); i++)
                    {
                        for (int j = 0; j < cellarray.GetLength(1); j++)
                            cellarray[i, j] = "";
                    }


                    var yurticialanlar = alanlar.Where(x => x.mensei == "Yurtiçi").ToList();
                    var yurtDisialanlar = alanlar.Where(x => x.mensei == "Yurtdışı").ToList();


                    cellarray[0, 0] = "Bilgi Raporlama Kodu";
                    cellarray[0, 1] = "Yuriçi/Yurtdışı";
                    cellarray[0, 2] = "Toplam Kullanıcı / Cihaz Adedi";
                    cellarray[0, 3] = "Bilgi Bedeli(TL)";
                    cellarray[0, 4] = "Toplam Bedel(TL)";

                    decimal geneltoplambedel = 0;
                    decimal geneltoplambedelY = 0;

                    var currrow = 1;
                    for (int i = 0; i < yurticialanlar.Count; i++)
                    {
                        cellarray[i + 1, 0] = yurticialanlar[i].bilgikodu;
                        cellarray[i + 1, 1] = yurticialanlar[i].mensei;
                        cellarray[i + 1, 2] = yurticialanlar[i].cihaztoplam;
                        cellarray[i + 1, 3] = yurticialanlar[i].bilgibedel;
                        cellarray[i + 1, 4] = yurticialanlar[i].toplambedel;
                        geneltoplambedel += decimal.Parse(yurticialanlar[i].toplambedel);
                        currrow++;
                    }


                    currrow++;



                    cellarray[currrow, 0] = "TOPLAM";
                    cellarray[currrow, 1] = "";
                    cellarray[currrow, 2] = ""; ;
                    cellarray[currrow, 3] = "";
                    cellarray[currrow, 4] = geneltoplambedel.ToString("0.00");

                    currrow += 5;
                    if (yurtDisialanlar.Count > 0)
                    {
                        cellarray[currrow, 0] = "Bilgi Raporlama Kodu";
                        cellarray[currrow, 1] = "Yuriçi/Yurtdışı";
                        cellarray[currrow, 2] = "Toplam Kullanıcı / Cihaz Adedi";
                        cellarray[currrow, 3] = "Bilgi Bedeli($)";
                        cellarray[currrow, 4] = "Toplam Bedel($)";
                        currrow++;

                        for (int i = 0; i < yurtDisialanlar.Count; i++)
                        {
                            cellarray[currrow, 0] = yurtDisialanlar[i].bilgikodu;
                            cellarray[currrow, 1] = yurtDisialanlar[i].mensei;
                            cellarray[currrow, 2] = yurtDisialanlar[i].cihaztoplam;
                            cellarray[currrow, 3] = yurtDisialanlar[i].bilgibedel;
                            cellarray[currrow, 4] = yurtDisialanlar[i].toplambedel;
                            geneltoplambedelY += decimal.Parse(yurtDisialanlar[i].toplambedel);
                            currrow++;
                        }

                        currrow++;

                        cellarray[currrow, 0] = "YURTDIŞI TOPLAM";
                        cellarray[currrow, 1] = "";
                        cellarray[currrow, 2] = ""; ;
                        cellarray[currrow, 3] = "";
                        cellarray[currrow, 4] = geneltoplambedelY.ToString("0.00");

                    }


                    ExcellApp.ScreenUpdating = false;
                    if (ExcellApp.Visible)
                    {
                        Microsoft.Office.Interop.Excel.Range range = excelsheet.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                        range.Value = cellarray;
                        range.Columns.AutoFit();
                    }

                    ExcellApp.ScreenUpdating = true;
                }
                catch
                {


                    ExcellApp.ScreenUpdating = true;
                }

            }
            public static void OrtakListe()
            {
                ExcelDurumFormAc();

                Thread T = new Thread((obj) =>
                {

                    try
                    {
                        crmDFNDataContext crm = new crmDFNDataContext();

                        ExcelDurumYaz("Aktarım Başladı");


                        var date = DateTime.Now.AddMonths(1);
                        var guncelay = date.ToString("MMMMMMMMM").ToUpper().Trim();
                        var guncelyil = date.Year.ToString();
                        var dosyaadi = "SubeBazliKullanıcıListe_" + guncelay + "_" + guncelyil + ".xlsx";
                        var yol = Application.StartupPath + "\\RAPOR";
                        if (!Directory.Exists(yol))
                            Directory.CreateDirectory(yol);
                        string filename = yol + "\\" + dosyaadi;
                        if (File.Exists(filename)) File.Delete(filename);

                        var excelworkbook = GetExcelWorkbook(filename);
                        if (excelworkbook != null)
                            excelworkbook.Close(false);

                        excelworkbook = OpenExcelWorkbook(filename);

                        if (excelworkbook == null) return;

                        var sheetname = guncelay + " " + date.Year;


                        var sorgu = crm.Users.Where(x => x.LisansDurum.YayinDurumu && x.StatusId == 1).ToList();

                        var sorguTum = new List<UserRecordKurum>();
                        for (int i = 0; i < sorgu.Count; i++)
                        {
                            var user = CalculateUser2(sorgu[i]);
                            user.Toplam();
                            // user.UserName = sorgu[i].UserName;
                            user.tckno = sorgu[i].UserName;
                            sorguTum.Add(user);

                            ExcelDurumYaz("Listeler Hazırlanıyor " + i + " / " + sorgu.Count);
                        }


                        var subeler = sorguTum.Select(x => x.sube).Distinct().ToList();


                        var cellmax = sorgu.Count + subeler.Count;
                        var cellarray = new object[cellmax + 250, 250];
                        var excelsheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.Add();



                        excelsheet.Name = sheetname;

                        var currentRow = 0;
                        var collindex = 0;


                        excelsheet.Application.ActiveWindow.SplitRow = 1;
                        excelsheet.Application.ActiveWindow.FreezePanes = true;


                        Microsoft.Office.Interop.Excel.Range baslikrange2 = excelsheet.Application.get_Range("A1", "AO1");
                        baslikrange2.Font.Bold = true;
                        baslikrange2.Borders.Value = true;
                        baslikrange2.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightYellow);
                        baslikrange2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        excelsheet.Application.get_Range("B1", "AO1").Orientation = 90;
                        excelsheet.Application.get_Range("A1").RangeOrtala(3);

                        //excelsheet.Application.get_Range("AH2", "AK3").Font.Bold = true;
                        //excelsheet.Application.get_Range("AH2", "AK3").RangeOrtala(3);
                        //excelsheet.Application.get_Range("AH2", "AK3").Borders.Value = true;
                        //excelsheet.Application.get_Range("AH2", "AK2").RangeFontColor(Color.Red);
                        currentRow = 0;
                        #region Basiklar                  
                        cellarray[currentRow, collindex] = "ŞUBELER"; collindex++;
                        cellarray[currentRow, collindex] = "Hesap No"; collindex++;
                        cellarray[currentRow, collindex] = "Başlangıç Tarihi"; collindex++;
                        cellarray[currentRow, collindex] = "Ekran"; collindex++;
                        cellarray[currentRow, collindex] = "Karma Düzey 1"; collindex++;
                        cellarray[currentRow, collindex] = "Bist Endeks"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Yüzeysel"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Düzey 1 Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Derinlik"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Derinlik Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Gün Sonu"; collindex++;
                        cellarray[currentRow, collindex] = "Pay Eşanlı İşlemler"; collindex++;
                        cellarray[currentRow, collindex] = "Tahvil Yüzeysel"; collindex++;
                        cellarray[currentRow, collindex] = "Tahvil Düzey 1 Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Tahvil Derinlik"; collindex++;
                        cellarray[currentRow, collindex] = "Analiz Pro"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Yüzeysel"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Düzey 1 Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Derinlik"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Derinlik Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Gün Sonu"; collindex++;
                        cellarray[currentRow, collindex] = "Karma Düzey 1"; collindex++;
                        cellarray[currentRow, collindex] = "Bist Endeks"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Yüzeysel"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Düzey 1 Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Derinlik"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Derinlik Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Gün Sonu"; collindex++;
                        cellarray[currentRow, collindex] = "Pay Eşanlı İşlemler"; collindex++;
                        cellarray[currentRow, collindex] = "Tahvil Yüzeysel"; collindex++;
                        cellarray[currentRow, collindex] = "Tahvil Düzey 1 Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Tahvil Derinlik"; collindex++;
                        cellarray[currentRow, collindex] = "Analiz Pro"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Yüzeysel"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Düzey 1 Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Derinlik"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Derinlik Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Gün Sonu"; collindex++;
                        cellarray[currentRow, collindex] = "Toplam"; collindex++;
                        cellarray[currentRow, collindex] = "KDV"; collindex++;
                        cellarray[currentRow, collindex] = "KDV Dahil Toplam";

                        #endregion
                        var row = 4;
                        var H1 = "A" + row;
                        var H2 = "A" + row;
                        var colorCyan = Color.Cyan;
                        var colorViolet = Color.Violet;
                        var MediumSpringGreen = Color.MediumSpringGreen;

                        subeler.Remove("MERKEZ");
                        subeler.Insert(0, "MERKEZ");
                        var geneltoplamlist = new List<UserRecordToplam2>();



                        foreach (var sube in subeler)
                        {
                            currentRow++;
                            ExcelDurumYaz(sube + " Aktarılılıyor");

                            var merkezsubeM = sorguTum.Where(x => x.sube == sube).ToList();

                            cellarray[currentRow, 0] = sube;

                            H1 = HucreAdresBul(currentRow, 0);
                            H2 = HucreAdresBul(currentRow, 0);

                            excelsheet.Application.get_Range(H1).Font.Bold = true;
                            excelsheet.Application.get_Range(H1).BorderAround2();
                            excelsheet.Application.get_Range(H1).BorderWeight(3);
                            excelsheet.Application.get_Range(H1).RangeBackColor(colorCyan);


                            if ((merkezsubeM.Count) > 0)
                            {
                                var toplamlarM = new UserRecordToplam2();
                                if (merkezsubeM.Count > 0)
                                {
                                    currentRow++;
                                    cellarray[currentRow, 0] = "MÜŞTERİLER";
                                    H1 = HucreAdresBul(currentRow, 0);
                                    excelsheet.Application.get_Range(H1).Font.Bold = true;
                                    excelsheet.Application.get_Range(H1).BorderAround2();
                                    excelsheet.Application.get_Range(H1).BorderWeight(3);
                                    excelsheet.Application.get_Range(H1).RangeBackColor(colorCyan);
                                    var startRow = currentRow + 1;

                                    for (int j = 0; j < merkezsubeM.Count; j++)
                                    {

                                        currentRow++;
                                        collindex = 0;
                                        var rec = merkezsubeM[j];
                                        cellarray[currentRow, collindex] = rec.AdSoyad; collindex++;
                                        // cellarray[currentRow, collindex] = rec.UserName; collindex++;
                                        cellarray[currentRow, collindex] = rec.tckno; collindex++;
                                        cellarray[currentRow, collindex] = rec.BasliangicTarihi; collindex++;
                                        #region Lisanslar                                  
                                        if (rec.Cep) { cellarray[currentRow, collindex] = "1"; }//1
                                        collindex++;
                                        if (rec.sKRMD1 > 0) { cellarray[currentRow, collindex] = rec.sKRMD1; }
                                        collindex++;//2
                                        if (rec.sEND > 0) { cellarray[currentRow, collindex] = rec.sEND; }
                                        collindex++;//3
                                        if (rec.sPD1 > 0) { cellarray[currentRow, collindex] = rec.sPD1; }
                                        collindex++;//4
                                        if (rec.sPD1P > 0) { cellarray[currentRow, collindex] = rec.sPD1P; }
                                        collindex++;//5
                                        if (rec.sPD2 > 0) { cellarray[currentRow, collindex] = rec.sPD2; }
                                        collindex++;//
                                        if (rec.sPD2P > 0) { cellarray[currentRow, collindex] = rec.sPD2P; }
                                        collindex++;

                                        if (rec.sPIT > 0) { cellarray[currentRow, collindex] = rec.sPIT; }
                                        collindex++;
                                        if (rec.sPITE > 0) { cellarray[currentRow, collindex] = rec.sPITE; }
                                        collindex++;
                                        if (rec.sBD1 > 0) { cellarray[currentRow, collindex] = rec.sBD1; }
                                        collindex++;
                                        if (rec.sBD1P > 0) { cellarray[currentRow, collindex] = rec.sBD1P; }
                                        collindex++;
                                        if (rec.sBD2 > 0) { cellarray[currentRow, collindex] = rec.sBD2; }
                                        collindex++;
                                        if (rec.sANPRO > 0) { cellarray[currentRow, collindex] = rec.sANPRO; }
                                        collindex++;
                                        if (rec.sVD1 > 0) { cellarray[currentRow, collindex] = rec.sVD1; ; }
                                        collindex++;
                                        if (rec.sVD1P > 0) { cellarray[currentRow, collindex] = rec.sVD1P; }
                                        collindex++;
                                        if (rec.sVD2 > 0) { cellarray[currentRow, collindex] = rec.sVD2; }
                                        collindex++;
                                        if (rec.sVD2P > 0) { cellarray[currentRow, collindex] = rec.sVD2P; }
                                        collindex++;
                                        if (rec.sVIT > 0) { cellarray[currentRow, collindex] = rec.sVIT; }
                                        collindex++;
                                        if (rec.KRMD1 > 0) { cellarray[currentRow, collindex] = rec.KRMD1; }
                                        collindex++;
                                        if (rec.MKK > 0) { cellarray[currentRow, collindex] = rec.MKK; }
                                        collindex++;
                                        if (rec.GKKUL > 0) { cellarray[currentRow, collindex] = rec.GKKUL; }
                                        collindex++;
                                        if (rec.TARAMA > 0) { cellarray[currentRow, collindex] = rec.TARAMA; }
                                        collindex++;
                                        if (rec.END > 0) { cellarray[currentRow, collindex] = rec.END; }
                                        collindex++;
                                        if (rec.PD1 > 0) { cellarray[currentRow, collindex] = rec.PD1; }
                                        collindex++;
                                        if (rec.PD1P > 0) { cellarray[currentRow, collindex] = rec.PD1P; }
                                        collindex++;
                                        if (rec.PD2 > 0) { cellarray[currentRow, collindex] = rec.PD2; }
                                        collindex++;
                                        if (rec.PD2P > 0) { cellarray[currentRow, collindex] = rec.PD2P; }
                                        collindex++;
                                        if (rec.PIT > 0) { cellarray[currentRow, collindex] = rec.PIT; }
                                        collindex++;
                                        if (rec.PITE > 0) { cellarray[currentRow, collindex] = rec.PITE; }
                                        collindex++;
                                        if (rec.BD1 > 0) { cellarray[currentRow, collindex] = rec.BD1; }
                                        collindex++;
                                        if (rec.BD1P > 0) { cellarray[currentRow, collindex] = rec.BD1P; }
                                        collindex++;
                                        if (rec.BD2 > 0) { cellarray[currentRow, collindex] = rec.BD2; }
                                        collindex++;
                                        if (rec.ANPRO > 0) { cellarray[currentRow, collindex] = rec.ANPRO; }
                                        collindex++;
                                        if (rec.VD1 > 0) { cellarray[currentRow, collindex] = rec.VD1; ; }
                                        collindex++;
                                        if (rec.VD1P > 0) { cellarray[currentRow, collindex] = rec.VD1P; }
                                        collindex++;
                                        if (rec.VD2 > 0) { cellarray[currentRow, collindex] = rec.VD2; }
                                        collindex++;
                                        if (rec.VD2P > 0) { cellarray[currentRow, collindex] = rec.VD2P; }
                                        collindex++;
                                        if (rec.VIT > 0) { cellarray[currentRow, collindex] = rec.VIT; }

                                        collindex++;
                                        if (rec.ToplamFiyat > 0) { cellarray[currentRow, collindex] = rec.ToplamFiyat; }
                                        collindex++;
                                        if (rec.KDV > 0) { cellarray[currentRow, collindex] = rec.KDV; }
                                        collindex++;
                                        if (rec.KDVDahilToplam > 0) { cellarray[currentRow, collindex] = rec.KDVDahilToplam; }







                                        ExcelDurumYaz(j + " / " + merkezsubeM.Count);

                                        #endregion

                                    }
                                    excelsheet.Application.get_Range(HucreAdresBul(startRow, 0), HucreAdresBul(currentRow, collindex)).Borders.Value = true;
                                    excelsheet.Application.get_Range(HucreAdresBul(startRow, 1), HucreAdresBul(currentRow, collindex + 1)).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                                    toplamlarM = UserRecordToplam2.UserToplamlar(merkezsubeM);
                                }

                                currentRow++;

                                geneltoplamlist.Add(toplamlarM);

                                #region ToplamSubeM

                                var toplamlar = toplamlarM;


                                collindex = 0;
                                cellarray[currentRow, collindex] = "TOPLAMLAR"; collindex++; collindex++; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sMobile; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sKRMD1; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sMKK; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sEND; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sPD1; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sPD1P; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sPD2; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sPD2P; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sPIT; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sPITE; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sBD1; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sBD1P; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sBD2; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sANPRO; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sVD1; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sVD1P; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sVD2; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sVD2P; collindex++;
                                cellarray[currentRow, collindex] = toplamlar.sVIT; collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.KRMD1); ; collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.MKK); ; collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.END); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.PD1); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.PD1P); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.PD2); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.PD2P); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.PIT); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.PITE); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.BD1); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.BD1P); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.BD2); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.ANPRO); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.VD1); ; collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.VD1P); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.VD2); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.VD2P); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.VIT); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.TARAMA); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.GKKUL); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.ToplamFiyat); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.ToplamKDV); collindex++;
                                cellarray[currentRow, collindex] = String.Format("{0:N}", toplamlar.KDVDahilToplam);

                                baslikrange2 = excelsheet.Application.get_Range(HucreAdresBul(currentRow, 0), HucreAdresBul(currentRow, collindex));
                                baslikrange2.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.MediumSpringGreen);
                                baslikrange2.Borders.Value = true;
                                baslikrange2.Font.Bold = true;
                                baslikrange2 = excelsheet.Application.get_Range(HucreAdresBul(currentRow, 0), HucreAdresBul(currentRow, collindex));
                                baslikrange2.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignBottom;
                                baslikrange2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                currentRow++;
                                #endregion

                            }

                        }





                        var geneltoplam = UserRecordToplam2.GenelToplam(geneltoplamlist);

                        #region ToplamGenel
                        currentRow++;
                        currentRow++;


                        collindex = 0;
                        cellarray[currentRow, collindex] = "GENEL TOPLAM"; collindex++; collindex++; collindex++;
                        cellarray[currentRow, collindex] = geneltoplam.sMobile; collindex++;//1
                        cellarray[currentRow, collindex] = geneltoplam.sKRMD1; collindex++;//2
                        cellarray[currentRow, collindex] = geneltoplam.sMKK; collindex++;//2
                        cellarray[currentRow, collindex] = geneltoplam.sEND; collindex++;//3
                        cellarray[currentRow, collindex] = geneltoplam.sPD1; collindex++;//4
                        cellarray[currentRow, collindex] = geneltoplam.sPD1P; collindex++;//5
                        cellarray[currentRow, collindex] = geneltoplam.sPD2; collindex++;//6
                        cellarray[currentRow, collindex] = geneltoplam.sPD2P; collindex++;//7
                        cellarray[currentRow, collindex] = geneltoplam.sPIT; collindex++;//8
                        cellarray[currentRow, collindex] = geneltoplam.sPITE; collindex++;//9
                        cellarray[currentRow, collindex] = geneltoplam.sBD1; collindex++;//10
                        cellarray[currentRow, collindex] = geneltoplam.sBD1P; collindex++;//11
                        cellarray[currentRow, collindex] = geneltoplam.sBD2; collindex++;//12
                        cellarray[currentRow, collindex] = geneltoplam.sANPRO; collindex++;//12
                        cellarray[currentRow, collindex] = geneltoplam.sVD1; collindex++;//13
                        cellarray[currentRow, collindex] = geneltoplam.sVD1P; collindex++;//14
                        cellarray[currentRow, collindex] = geneltoplam.sVD2; collindex++;//15
                        cellarray[currentRow, collindex] = geneltoplam.sVD2P; collindex++;//16
                        cellarray[currentRow, collindex] = geneltoplam.sVIT; collindex++;//17
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.KRMD1); ; collindex++;//18
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.MKK); ; collindex++;//18
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.END); collindex++;//19
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.PD1); collindex++;//20
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.PD1P); collindex++;//21
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.PD2); collindex++;//22
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.PD2P); collindex++;//23
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.PIT); collindex++;//24
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.PITE); collindex++;//25
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.BD1); collindex++;//26
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.BD1P); collindex++;//27
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.BD2); collindex++;//28
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.ANPRO); collindex++;//28
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.VD1); ; collindex++;//29
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.VD1P); collindex++;//30
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.VD2); collindex++;//31
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.VD2P); collindex++;//32
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.VIT); collindex++;//33
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.TARAMA); collindex++;//34
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.GKKUL); collindex++;//35
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.ToplamFiyat); collindex++;//34
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.ToplamKDV); collindex++;//35
                        cellarray[currentRow, collindex] = String.Format("{0:N}", geneltoplam.KDVDahilToplam);//39


                        baslikrange2 = excelsheet.Application.get_Range(HucreAdresBul(currentRow, 0), HucreAdresBul(currentRow, collindex));
                        baslikrange2.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.MediumSpringGreen);
                        baslikrange2.Borders.Value = true;
                        baslikrange2.Font.Bold = true;
                        baslikrange2 = excelsheet.Application.get_Range(HucreAdresBul(currentRow, 0), HucreAdresBul(currentRow, collindex));
                        baslikrange2.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignBottom;
                        baslikrange2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        currentRow++;
                        #endregion



                        ExcellApp.ScreenUpdating = false;
                        if (ExcellApp.Visible)
                        {

                            Microsoft.Office.Interop.Excel.Range range = excelsheet.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                            range.Value = cellarray;
                            range.Columns.AutoFit();

                            ExcelDurumYaz("Aktarım Tamamlandı");

                        }


                        ExcellApp.ScreenUpdating = true;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
                );
                T.Start();



            }

            public static void BorsaEkranRaporu()
            {
                ExcelDurumFormAc();

                Thread T = new Thread((obj) =>
                {
                    try
                    {
                        crmDFNDataContext crm = new crmDFNDataContext();
                        ExcelDurumYaz("Aktarım Başladı");

                        var now = DateTime.Now;
                        var guncelay = now.AddMonths(1).ToString("MMMMMMMMM").ToUpper().Trim();
                        var guncelyil = now.AddMonths(1).Year.ToString();
                        var dosyaadi = "BorsaEkranRaporu_" + guncelay + "_" + guncelyil + ".xlsx";
                        var yol = Application.StartupPath + "\\RAPOR";
                        if (!Directory.Exists(yol))
                            Directory.CreateDirectory(yol);
                        string filename = yol + "\\" + dosyaadi;
                        if (File.Exists(filename)) File.Delete(filename);

                        var excelworkbook = GetExcelWorkbook(filename);
                        if (excelworkbook != null)
                            excelworkbook.Close(false);

                        excelworkbook = OpenExcelWorkbook(filename);
                        if (excelworkbook == null) return;

                        var sheetname = guncelay + " " + now.AddMonths(1).Year;

                        // Veri çek
                        var sorgu = crm.Users.Where(x => x.LisansDurum.YayinDurumu && x.StatusId == 1).ToList();

                        var sorguTum = new List<UserRecordKurum>();
                        for (int i = 0; i < sorgu.Count; i++)
                        {
                            var user = CalculateUser2(sorgu[i]);
                            user.Toplam();
                            user.tckno = sorgu[i].UserName;
                            sorguTum.Add(user);
                            ExcelDurumYaz("Listeler Hazırlanıyor " + i + " / " + sorgu.Count);
                        }

                        // Excel sheet oluştur
                        var colCount = 9; // A-I (PMTS, TCKN, AdSoyad, Ekran, Maliyet, Mensei, LisansTarih, KRMD1, END)
                        var cellarray = new object[sorgu.Count + 5, colCount];
                        var excelsheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.Add();
                        excelsheet.Name = sheetname;

                        // Freeze panes
                        excelsheet.Application.ActiveWindow.SplitRow = 1;
                        excelsheet.Application.ActiveWindow.FreezePanes = true;

                        // Başlık satırı formatla
                        Microsoft.Office.Interop.Excel.Range baslikRange = excelsheet.Application.get_Range("A1", "I1");
                        baslikRange.Font.Bold = true;
                        baslikRange.Borders.Value = true;
                        baslikRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightYellow);
                        baslikRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        baslikRange.WrapText = true;

                        // Başlıklar
                        int currentRow = 0;
                        cellarray[currentRow, 0] = "PMTS Kodu";
                        cellarray[currentRow, 1] = "TCKN";
                        cellarray[currentRow, 2] = "Müşteri Ad Soyad";
                        cellarray[currentRow, 3] = "Ekran";
                        cellarray[currentRow, 4] = "Ekran Maliyeti (TL)";
                        cellarray[currentRow, 5] = "Yurtiçi / YurtDışı";
                        cellarray[currentRow, 6] = "Lisans Sonlanma Tarihi";
                        cellarray[currentRow, 7] = "KRMD1";
                        cellarray[currentRow, 8] = "Endeks";

                        // Düz liste olarak tüm kullanıcıları yaz
                        for (int i = 0; i < sorguTum.Count; i++)
                        {
                            currentRow++;
                            var rec = sorguTum[i];
                            var srcUser = sorgu[i]; // Aynı index — sorgu ve sorguTum paralel

                            // Ekran tipi
                            string ekranTipi = "";
                            if (rec.Pro && rec.Cep)
                                ekranTipi = "TradeAll TR Masaüstü + Mobil";
                            else if (rec.Pro)
                                ekranTipi = "TradeAll TR Masaüstü";
                            else if (rec.Cep)
                                ekranTipi = "TradeAll TR Mobil";

                            // Mensei
                            string mensei = "";
                            if (srcUser.MusteriMenseiID == 1)
                                mensei = "Yurtiçi";
                            else if (srcUser.MusteriMenseiID == 2)
                                mensei = "YurtDışı";

                            cellarray[currentRow, 0] = srcUser.PmtsNo;
                            cellarray[currentRow, 1] = rec.tckno;
                            cellarray[currentRow, 2] = rec.AdSoyad;
                            cellarray[currentRow, 3] = ekranTipi;
                            cellarray[currentRow, 4] = rec.ToplamFiyat;
                            cellarray[currentRow, 5] = mensei;
                            cellarray[currentRow, 6] = srcUser.ExpiryDate.HasValue
                                ? srcUser.ExpiryDate.Value.ToShortDateString()
                                : "";
                            cellarray[currentRow, 7] = srcUser.LisansDurum.COMEX ? "Evet" : "";
                            cellarray[currentRow, 8] = srcUser.LisansDurum.PayX ? "Evet" : "";

                            ExcelDurumYaz(i + " / " + sorguTum.Count);
                        }

                        // AutoFilter ekle (Excel'de filtreleme yapılabilsin)
                        var dataRange = excelsheet.Application.get_Range("A1", HucreAdresBul(currentRow, colCount - 1));
                        dataRange.Borders.Value = true;
                        dataRange.AutoFilter(1);

                        // Array'i Excel'e yaz
                        ExcellApp.ScreenUpdating = false;
                        if (ExcellApp.Visible)
                        {
                            var actualRows = currentRow + 1;
                            var trimmed = new object[actualRows, colCount];
                            for (int r = 0; r < actualRows; r++)
                                for (int c = 0; c < colCount; c++)
                                    trimmed[r, c] = cellarray[r, c];

                            Microsoft.Office.Interop.Excel.Range range = excelsheet.Cells.get_Resize(actualRows, colCount);
                            range.Value = trimmed;
                            range.Columns.AutoFit();
                            ExcelDurumYaz("Aktarım Tamamlandı");
                        }
                        ExcellApp.ScreenUpdating = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
                T.Start();
            }
            public static void AcikKullaniciListesi()
            {
                ExcelDurumFormAc();

                Thread T = new Thread((obj) =>
                {

                    try
                    {
                        crmDFNDataContext crm = new crmDFNDataContext();

                        ExcelDurumYaz("Aktarım Başladı");
                        var date = DateTime.Now.AddMonths(1);
                        var guncelay = date.ToString("MMMMMMMMM").ToUpper().Trim();
                        var guncelyil = date.Year.ToString();
                        var dosyaadi = "AktifKullanıcıListesi" + guncelay + "_" + guncelyil + ".xlsx";
                        var yol = Application.StartupPath + "\\RAPOR";
                        if (!Directory.Exists(yol))
                            Directory.CreateDirectory(yol);
                        string filename = yol + "\\" + dosyaadi;
                        if (File.Exists(filename)) File.Delete(filename);
                        var excelworkbook = GetExcelWorkbook(filename);
                        if (excelworkbook != null)
                            excelworkbook.Close(false);
                        excelworkbook = OpenExcelWorkbook(filename);
                        if (excelworkbook == null) return;
                        var sheetname = guncelay + " " + date.Year;
                        var sorgu = crm.Users.Where(x => x.LisansDurum.YayinDurumu && x.StatusId == 1).ToList();
                        var sorguTum = new List<UserRecordKurum>();
                        for (int i = 0; i < sorgu.Count; i++)
                        {
                            var user = CalculateUser2(sorgu[i]);
                            user.Toplam();
                            // user.UserName = sorgu[i].UserName;
                            user.tckno = sorgu[i].tckno;
                            sorguTum.Add(user);
                            ExcelDurumYaz("Listeler Hazırlanıyor " + i + " / " + sorgu.Count);
                        }
                        var subeler = sorguTum.Select(x => x.sube).Distinct().ToList();
                        var cellmax = sorgu.Count + subeler.Count;
                        var cellarray = new object[cellmax + 250, 250];
                        var excelsheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.Add();
                        excelsheet.Name = sheetname;
                        var currentRow = 0;
                        var collindex = 0;
                        excelsheet.Application.ActiveWindow.SplitRow = 1;
                        excelsheet.Application.ActiveWindow.FreezePanes = true;
                        Microsoft.Office.Interop.Excel.Range baslikrange2 = excelsheet.Application.get_Range("A1", "AV1");
                        baslikrange2.Font.Bold = true;
                        baslikrange2.Borders.Value = true;
                        baslikrange2.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightYellow);
                        baslikrange2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        excelsheet.Application.get_Range("B1", "AV1").Orientation = 90;
                        excelsheet.Application.get_Range("A1").RangeOrtala(3);
                        //excelsheet.Application.get_Range("AH2", "AK3").Font.Bold = true;
                        //excelsheet.Application.get_Range("AH2", "AK3").RangeOrtala(3);
                        //excelsheet.Application.get_Range("AH2", "AK3").Borders.Value = true;
                        //excelsheet.Application.get_Range("AH2", "AK2").RangeFontColor(Color.Red);
                        currentRow = 0;
                        #region Basiklar 
                        cellarray[currentRow, collindex] = "Adı Soyadı"; collindex++;
                        cellarray[currentRow, collindex] = "Hesap No"; collindex++;
                        cellarray[currentRow, collindex] = "Sube"; collindex++;
                        cellarray[currentRow, collindex] = "Temsilci"; collindex++;
                        cellarray[currentRow, collindex] = "Başlangıç Tarihi"; collindex++;
                        cellarray[currentRow, collindex] = "Ekran"; collindex++;
                        cellarray[currentRow, collindex] = "YDS"; collindex++;
                        cellarray[currentRow, collindex] = "Karma Düzey 1"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Yüzeysel"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Düzey 1 Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Derinlik"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Derinlik Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Gün Sonu"; collindex++;
                        cellarray[currentRow, collindex] = "Pay Eşanlı İşlemler"; collindex++;
                        cellarray[currentRow, collindex] = "Bist Endeks"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Yüzeysel"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Düzey 1 Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Derinlik"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Derinlik Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Gün Sonu"; collindex++;
                        cellarray[currentRow, collindex] = "Tahvil Yüzeysel"; collindex++;
                        cellarray[currentRow, collindex] = "Tahvil Düzey 1 Plus"; collindex++;
                        cellarray[currentRow, collindex] = "Tahvil Derinlik"; collindex++;
                        cellarray[currentRow, collindex] = "Analiz Pro"; collindex++;
                        cellarray[currentRow, collindex] = "SentiL1"; collindex++;
                        cellarray[currentRow, collindex] = "SentiL2"; collindex++;
                        cellarray[currentRow, collindex] = "Karma Düzey 1 Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Yüzeysel Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Düzey 1 Plus Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Derinlik Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Derinlik Plus Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Hisse Gün Sonu Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Pay Eşanlı İşlemler Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Bist Endeks Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Yüzeysel Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Düzey 1 Plus Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Derinlik Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Derinlik Plus Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Viop Gün Sonu Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Tahvil Yüzeysel Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Tahvil Düzey 1 Plus Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Tahvil Derinlik Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Analiz Pro Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "SentiL1 Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "SentiL2 Fiyat"; collindex++;
                        cellarray[currentRow, collindex] = "Toplam"; collindex++;
                        cellarray[currentRow, collindex] = "KDV"; collindex++;
                        cellarray[currentRow, collindex] = "KDV Dahil Toplam";

                        #endregion
                        var row = 4;
                        var H1 = "A" + row;
                        var H2 = "A" + row;
                        var colorCyan = Color.Cyan;
                        var colorViolet = Color.Violet;
                        var MediumSpringGreen = Color.MediumSpringGreen;
                        foreach (var sube in subeler)
                        {
                            ExcelDurumYaz(sube + " Aktarılılıyor");
                            var merkezsubeM = sorguTum.Where(x => x.sube == sube).ToList();
                            if ((merkezsubeM.Count) > 0)
                            {
                                var toplamlarM = new UserRecordToplam2();
                                if (merkezsubeM.Count > 0)
                                {
                                    var startRow = currentRow + 1;
                                    for (int j = 0; j < merkezsubeM.Count; j++)
                                    {
                                        currentRow++;
                                        collindex = 0;
                                        var rec = merkezsubeM[j];
                                        cellarray[currentRow, collindex] = rec.AdSoyad; collindex++;
                                        // cellarray[currentRow, collindex] = rec.UserName; collindex++;
                                        cellarray[currentRow, collindex] = rec.tckno; collindex++;
                                        cellarray[currentRow, collindex] = rec.sube; collindex++;
                                        //cellarray[currentRow, collindex] = rec.FXkurum; collindex++;
                                        //Burada FXkurum yerine MTID eklendi. Excelde Temsilci alanına denk geliyor.
                                        cellarray[currentRow, collindex] = rec.MTID; collindex++;
                                        cellarray[currentRow, collindex] = rec.BasliangicTarihi; collindex++;
                                        #region Lisanslar                                  
                                        if (rec.Cep) { cellarray[currentRow, collindex] = "1"; }//1
                                        collindex++;
                                        if (rec.sSPI > 0) { cellarray[currentRow, collindex] = rec.sSPI; }
                                        collindex++;//2
                                        if (rec.sKRMD1 > 0) { cellarray[currentRow, collindex] = rec.sKRMD1; }
                                        collindex++;//2
                                        if (rec.sMKK > 0) { cellarray[currentRow, collindex] = rec.sMKK; }
                                        collindex++;//2
                                        if (rec.sPD1 > 0) { cellarray[currentRow, collindex] = rec.sPD1; }
                                        collindex++;//4
                                        if (rec.sPD1P > 0) { cellarray[currentRow, collindex] = rec.sPD1P; }
                                        collindex++;//5
                                        if (rec.sPD2 > 0) { cellarray[currentRow, collindex] = rec.sPD2; }
                                        collindex++;//
                                        if (rec.sPD2P > 0) { cellarray[currentRow, collindex] = rec.sPD2P; }
                                        collindex++;
                                        if (rec.sPIT > 0) { cellarray[currentRow, collindex] = rec.sPIT; }
                                        collindex++;
                                        if (rec.sPITE > 0) { cellarray[currentRow, collindex] = rec.sPITE; }
                                        collindex++;
                                        if (rec.sEND > 0) { cellarray[currentRow, collindex] = rec.sEND; }
                                        collindex++;//3
                                        if (rec.sVD1 > 0) { cellarray[currentRow, collindex] = rec.sVD1; ; }
                                        collindex++;
                                        if (rec.sVD1P > 0) { cellarray[currentRow, collindex] = rec.sVD1P; }
                                        collindex++;
                                        if (rec.sVD2 > 0) { cellarray[currentRow, collindex] = rec.sVD2; }
                                        collindex++;
                                        if (rec.sVD2P > 0) { cellarray[currentRow, collindex] = rec.sVD2P; }
                                        collindex++;
                                        if (rec.sVIT > 0) { cellarray[currentRow, collindex] = rec.sVIT; }
                                        collindex++;
                                        if (rec.sTARAMA > 0) { cellarray[currentRow, collindex] = rec.sTARAMA; }
                                        collindex++;
                                        if (rec.sGKKUL > 0) { cellarray[currentRow, collindex] = rec.sGKKUL; }
                                        collindex++;
                                        if (rec.sBD1 > 0) { cellarray[currentRow, collindex] = rec.sBD1; }
                                        collindex++;
                                        if (rec.sBD1P > 0) { cellarray[currentRow, collindex] = rec.sBD1P; }
                                        collindex++;
                                        if (rec.sBD2 > 0) { cellarray[currentRow, collindex] = rec.sBD2; }
                                        collindex++;
                                        if (rec.sANPRO > 0) { cellarray[currentRow, collindex] = rec.sANPRO; }
                                        collindex++;
                                        if (rec.sSentiL1 > 0) { cellarray[currentRow, collindex] = rec.sSentiL1; }
                                        collindex++;
                                        if (rec.sSentiL2 > 0) { cellarray[currentRow, collindex] = rec.sSentiL2; }
                                        collindex++;

                                        if (rec.KRMD1 > 0) { cellarray[currentRow, collindex] = rec.KRMD1; }
                                        collindex++;//2
                                        if (rec.MKK > 0) { cellarray[currentRow, collindex] = rec.MKK; }
                                        collindex++;//2
                                        if (rec.TARAMA > 0) { cellarray[currentRow, collindex] = rec.TARAMA; }
                                        collindex++;//2
                                        if (rec.GKKUL > 0) { cellarray[currentRow, collindex] = rec.GKKUL; }
                                        collindex++;//2
                                        if (rec.PD1 > 0) { cellarray[currentRow, collindex] = rec.PD1; }
                                        collindex++;//4
                                        if (rec.PD1P > 0) { cellarray[currentRow, collindex] = rec.PD1P; }
                                        collindex++;//5
                                        if (rec.PD2 > 0) { cellarray[currentRow, collindex] = rec.PD2; }
                                        collindex++;//
                                        if (rec.PD2P > 0) { cellarray[currentRow, collindex] = rec.PD2P; }
                                        collindex++;
                                        if (rec.PIT > 0) { cellarray[currentRow, collindex] = rec.PIT; }
                                        collindex++;
                                        if (rec.PITE > 0) { cellarray[currentRow, collindex] = rec.PITE; }
                                        collindex++;
                                        if (rec.END > 0) { cellarray[currentRow, collindex] = rec.END; }
                                        collindex++;//3
                                        if (rec.VD1 > 0) { cellarray[currentRow, collindex] = rec.VD1; ; }
                                        collindex++;
                                        if (rec.VD1P > 0) { cellarray[currentRow, collindex] = rec.VD1P; }
                                        collindex++;
                                        if (rec.VD2 > 0) { cellarray[currentRow, collindex] = rec.VD2; }
                                        collindex++;
                                        if (rec.VD2P > 0) { cellarray[currentRow, collindex] = rec.VD2P; }
                                        collindex++;
                                        if (rec.VIT > 0) { cellarray[currentRow, collindex] = rec.VIT; }
                                        collindex++;
                                        if (rec.BD1 > 0) { cellarray[currentRow, collindex] = rec.BD1; }
                                        collindex++;
                                        if (rec.BD1P > 0) { cellarray[currentRow, collindex] = rec.BD1P; }
                                        collindex++;
                                        if (rec.BD2 > 0) { cellarray[currentRow, collindex] = rec.BD2; }
                                        collindex++;
                                        if (rec.ANPRO > 0) { cellarray[currentRow, collindex] = rec.ANPRO; }
                                        collindex++;
                                        if (rec.SentiL1 > 0) { cellarray[currentRow, collindex] = rec.SentiL1; }
                                        collindex++;
                                        if (rec.SentiL2 > 0) { cellarray[currentRow, collindex] = rec.SentiL2; }
                                        collindex++;

                                        if (rec.ToplamFiyat > 0) { cellarray[currentRow, collindex] = rec.ToplamFiyat; }
                                        collindex++;
                                        if (rec.KDV > 0) { cellarray[currentRow, collindex] = rec.KDV; }
                                        collindex++;
                                        if (rec.KDVDahilToplam > 0) { cellarray[currentRow, collindex] = rec.KDVDahilToplam; }
                                        ExcelDurumYaz(j + " / " + merkezsubeM.Count);
                                        #endregion
                                    }
                                    excelsheet.Application.get_Range(HucreAdresBul(startRow, 0), HucreAdresBul(currentRow, collindex)).Borders.Value = true;
                                    excelsheet.Application.get_Range(HucreAdresBul(startRow, 1), HucreAdresBul(currentRow, collindex + 1)).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                                    toplamlarM = UserRecordToplam2.UserToplamlar(merkezsubeM);
                                }
                            }
                        }
                        ExcellApp.ScreenUpdating = false;
                        if (ExcellApp.Visible)
                        {

                            Microsoft.Office.Interop.Excel.Range range = excelsheet.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                            range.Value = cellarray;
                            range.Columns.AutoFit();

                            ExcelDurumYaz("Aktarım Tamamlandı");

                        }


                        ExcellApp.ScreenUpdating = true;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
                );
                T.Start();

            }
            public static string HucreAdresBul(int row, int collno)
            {

                return HucreHafiBul(collno + 1) + (row + 1);
            }
            public static string HucreHafiBul(int collno)
            {
                var sonuc = "";

                try
                {
                    int nCol = collno;
                    string sChars = "0ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string sCol = "";
                    while (nCol > 26)
                    {
                        int nChar = nCol % 26;
                        if (nChar == 0)
                            nChar = 26;
                        nCol = (nCol - nChar) / 26;
                        sCol = sChars[nChar] + sCol;
                    }
                    if (nCol != 0)
                        sCol = sChars[nCol] + sCol;

                    return sCol;
                }
                catch { return sonuc; }


            }
            public static void YdsRapor()
            {

                try
                {
                    crmDFNDataContext crm = new crmDFNDataContext();
                    var Sorgu = crm.Users.Where(x => x.LisansDurum.SPI == true && x.Status.StatusAdi == "ISE").ToList();


                    musteridurum.durumdictionary.Clear();
                    musteridurum.ydurumdictionary.Clear();


                    var filename = System.Windows.Forms.Application.StartupPath + "\\" + "YDSRapor" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".XLSX";
                    //string filename = System.Windows.Forms.Application.StartupPath + "\\" + "CrmDFN_Rapor.XLSX";
                    if (File.Exists(filename)) File.Delete(filename);
                    var excelworkbook = GetExcelWorkbook(filename);
                    if (excelworkbook != null)
                    {
                        excelworkbook.Save();
                        excelworkbook.Close(false);
                    }
                    excelworkbook = OpenExcelWorkbook(filename);
                    if (excelworkbook == null) return;
                    var sheetname = "YDS-RAPOR";

                    var cellmax = Sorgu.Count + 10;
                    var cellarray = new object[cellmax + 10, 41];
                    var colorCyan = Color.Cyan;
                    var colorViolet = Color.Violet;
                    var MediumSpringGreen = Color.MediumSpringGreen;
                    var excelsheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.ActiveSheet;
                    excelsheet.Name = sheetname;
                    Microsoft.Office.Interop.Excel.Range baslikrange2 = excelsheet.Application.get_Range("A1", "B1");

                    baslikrange2.Font.Bold = true;
                    baslikrange2.Interior.Color = ColorTranslator.ToOle(colorCyan);
                    baslikrange2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    cellarray[0, 0] = "Hesap No";

                    cellarray[0, 1] = "Ad Soyad";
                    int toplam = 0;
                    var row = 0;
                    //  var brow = 0;
                    row++;


                    if (Sorgu.Count > 0)
                    {

                        for (int i = 0; i < Sorgu.Count; i++)
                        {
                            toplam++;

                            //  var ucret = MyTools.lisansfiyatlari.Yvl2;
                            var rec = Sorgu[i];
                            // cellarray[row, 0] = rec.UserName;
                            cellarray[row, 0] = rec.tckno;
                            cellarray[row, 1] = rec.Name + " " + rec.Surname;

                            row++;
                        }
                    }


                    cellarray[row, 0] = "TOPLAM";
                    cellarray[row, 1] = toplam;
                    row++;
                    Microsoft.Office.Interop.Excel.Range toplamsatir = excelsheet.Application.get_Range("A" + row, "B" + row);
                    toplamsatir.Font.Bold = true;
                    toplamsatir.Interior.Color = ColorTranslator.ToOle(colorCyan);
                    toplamsatir.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;


                    ExcellApp.ScreenUpdating = false;
                    if (ExcellApp.Visible)
                    {
                        //  Microsoft.Office.Interop.Excel.Range range2 = excelsheet2.Cells.get_Resize(cellarray2.GetLength(0), cellarray2.GetLength(1));
                        //  range2.Value = cellarray2;
                        // range2.Columns.AutoFit();


                        Microsoft.Office.Interop.Excel.Range range = excelsheet.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                        range.Value = cellarray;
                        range.Columns.AutoFit();
                    }

                    ExcellApp.ScreenUpdating = true;

                }
                catch
                {


                    ExcellApp.ScreenUpdating = true;
                }
            }
            public static void YdsRaporAciklar()
            {
                try
                {
                    crmDFNDataContext crm = new crmDFNDataContext();
                    var Sorgu = crm.Users.Where(x => x.LisansDurum.SPI == true && x.Status.StatusAdi == "ISE" && x.LisansDurum.YayinDurumu == true).ToList();


                    musteridurum.durumdictionary.Clear();
                    musteridurum.ydurumdictionary.Clear();


                    var filename = System.Windows.Forms.Application.StartupPath + "\\" + "YDSRaporAcikKullanicilar" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".XLSX";
                    //string filename = System.Windows.Forms.Application.StartupPath + "\\" + "CrmDFN_Rapor.XLSX";
                    if (File.Exists(filename)) File.Delete(filename);
                    var excelworkbook = GetExcelWorkbook(filename);
                    if (excelworkbook != null)
                    {
                        excelworkbook.Save();
                        excelworkbook.Close(false);
                    }
                    excelworkbook = OpenExcelWorkbook(filename);
                    if (excelworkbook == null) return;
                    var sheetname = "YDS-RAPOR";

                    var cellmax = Sorgu.Count + 10;
                    var cellarray = new object[cellmax + 10, 41];
                    var colorCyan = Color.Cyan;
                    var colorViolet = Color.Violet;
                    var MediumSpringGreen = Color.MediumSpringGreen;
                    var excelsheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.ActiveSheet;
                    excelsheet.Name = sheetname;
                    Microsoft.Office.Interop.Excel.Range baslikrange2 = excelsheet.Application.get_Range("A1", "B1");

                    baslikrange2.Font.Bold = true;
                    baslikrange2.Interior.Color = ColorTranslator.ToOle(colorCyan);
                    baslikrange2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    cellarray[0, 0] = "Müşteri No";
                    cellarray[0, 1] = "Ad Soyad";
                    int toplam = 0;
                    var row = 0;
                    //  var brow = 0;
                    row++;


                    if (Sorgu.Count > 0)
                    {

                        for (int i = 0; i < Sorgu.Count; i++)
                        {
                            toplam++;

                            //  var ucret = MyTools.lisansfiyatlari.Yvl2;
                            var rec = Sorgu[i];
                            //cellarray[row, 0] = rec.UserName;
                            cellarray[row, 0] = rec.tckno;
                            cellarray[row, 1] = rec.Name + " " + rec.Surname;

                            row++;
                        }
                    }


                    cellarray[row, 0] = "TOPLAM";
                    cellarray[row, 1] = toplam;
                    row++;
                    Microsoft.Office.Interop.Excel.Range toplamsatir = excelsheet.Application.get_Range("A" + row, "B" + row);
                    toplamsatir.Font.Bold = true;
                    toplamsatir.Interior.Color = ColorTranslator.ToOle(colorCyan);
                    toplamsatir.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;


                    ExcellApp.ScreenUpdating = false;
                    if (ExcellApp.Visible)
                    {
                        //  Microsoft.Office.Interop.Excel.Range range2 = excelsheet2.Cells.get_Resize(cellarray2.GetLength(0), cellarray2.GetLength(1));
                        //  range2.Value = cellarray2;
                        // range2.Columns.AutoFit();


                        Microsoft.Office.Interop.Excel.Range range = excelsheet.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                        range.Value = cellarray;
                        range.Columns.AutoFit();
                    }

                    ExcellApp.ScreenUpdating = true;

                }
                catch
                {


                    ExcellApp.ScreenUpdating = true;
                }
            }
            public static void BorsaDetaylisteToExcel()
            {
                try
                {
                    crmDFNDataContext crm = new crmDFNDataContext();
                    var Sorgu = crm.Users.Where(x => x.StatusId == 1);


                    musteridurum.durumdictionary.Clear();
                    musteridurum.ydurumdictionary.Clear();


                    var filename = System.Windows.Forms.Application.StartupPath + "\\" + "Detay" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".XLSX";
                    //string filename = System.Windows.Forms.Application.StartupPath + "\\" + "CrmDFN_Rapor.XLSX";
                    SafeDeleteFile(filename);
                    var excelworkbook = GetExcelWorkbook(filename);
                    if (excelworkbook != null)
                    {
                        excelworkbook.Save();
                        excelworkbook.Close(false);
                    }
                    excelworkbook = OpenExcelWorkbook(filename);
                    if (excelworkbook == null) return;

                    var excelsheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.get_Item(1);


                    var sorguDetay = crm.Users.Where(x => (x.LisansDurum.YayinDurumu == true && x.StatusId == 1) || (x.LisansDurum.YayinDurumu == true && x.StatusId == 5));


                    foreach (var item in sorguDetay)
                    {
                        musteridurum m = new musteridurum();
                        #region LISANLARIHESAPLA
                        if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == false && item.LisansDurum.PayL2 == false && item.LisansDurum.Pd2P == false)
                        {

                            m.PD1 = 1;
                        }
                        else if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == false && item.LisansDurum.Pd2P == false)
                        {
                            m.PD1P = 1;

                        }
                        else if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == true && item.LisansDurum.Pd2P == false)
                        {

                            m.PD2 = 1;
                        }
                        else if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == true && item.LisansDurum.Pd2P == true)
                        {

                            m.PD2P = 1;
                        }

                        if (item.LisansDurum.PayX == true)
                            m.END = 1;

                        if (item.LisansDurum.PITE == true)
                            m.PITE = 1;

                        if (item.LisansDurum.PayGS == true)
                            m.PIT = 1;



                        if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == false && item.LisansDurum.ViopL2 == false && item.LisansDurum.Vd2P == false)
                        {

                            m.VL1 = 1;
                        }
                        else if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == false && item.LisansDurum.Vd2P == false)
                        {
                            m.VL1P = 1;

                        }
                        else if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == true && item.LisansDurum.Vd2P == false)
                        {

                            m.VL2 = 1;
                        }

                        else if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == true && item.LisansDurum.Vd2P == true)
                        {

                            m.VL2P = 1;
                        }

                        if (item.LisansDurum.ViopGS == true)
                            m.VIT = 1;
                        if (item.LisansDurum.COMEX == true)
                            m.KRMD1 = 1;
                        if (item.LisansDurum.MKK == true)
                            m.MKK = 1;
                        if (item.LisansDurum.GKKUL == true)
                            m.GKKUL = 1;
                        if (item.LisansDurum.TARAMA == true)
                            m.TARAMA = 1;

                        if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == false && item.LisansDurum.TahvilL2 == false)
                        {
                            m.BD1 = 1;
                        }
                        else if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == true && item.LisansDurum.TahvilL2 == false)
                        {
                            m.BD1P = 1;

                        }
                        else if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == true && item.LisansDurum.TahvilL2 == true)
                        {

                            m.BD2 = 1;
                        }
                        if (item.LisansDurum.AnPro == true)
                            m.ANPRO = 1;
                        #endregion

                        // var musteri = crm.Musterilers.FirstOrDefault(x => x.MusteriNo == item.PmtsNo);
                        var musteri = crm.Musterilers.FirstOrDefault(x => x.Tckno == item.PmtsNo);
                        if (musteri != null)
                        {
                            m.musteriadi = musteri.MusteriAdi;

                        }
                        else
                        {
                            m.musteriadi = item.Name + " " + item.Surname;
                        }

                        if (item.Iletisim != null)
                        {
                            m.adres = (item.Iletisim.acikadres == null) ? "" : item.Iletisim.acikadres;
                            m.sehir = (item.Iletisim.Il == null) ? "" : item.Iletisim.Il.IlAdi;
                            m.ulke = (item.Iletisim.Ulke == null) ? "" : item.Iletisim.Ulke.UlkeAdi;
                        }

                        if (item.BaslangicTarihi != null)
                        {

                            m.yetkilendirmetarihi = item.BaslangicTarihi.Value.ToShortDateString();
                        }

                        if (item.MusteriMenseiID == 2)
                            m.mensei = "YurtDışı";
                        else
                            m.mensei = "Yurtİçi";

                        if (item.StatusId == 1)
                            m.Bedelsizaciklama = "";
                        else
                        {
                            m.Bedelsizaciklama = "Bedelsiz";
                        }

                        if (item.MusteriMenseiID == 2)
                            musteridurum.ekleY(item.tckno, m);
                        else
                        {
                            musteridurum.ekle(item.tckno, m);

                        }
                    }

                    var alanlarlistesi = new List<alanlar>();
                    foreach (var mdurum in musteridurum.durumdictionary)
                    {

                        #region PD1
                        if (mdurum.Value.PD1 > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PD1";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.PD1.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fpd1.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fpd1 * mdurum.Value.PD1).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PD1P
                        if (mdurum.Value.PD1P > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PD1P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.PD1P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fpd1p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fpd1p * mdurum.Value.PD1P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PD2
                        if (mdurum.Value.PD2 > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PD2";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.PD2.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fpd2.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fpd2 * mdurum.Value.PD2).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PD2P
                        if (mdurum.Value.PD2P > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PD2P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.PD2P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fpd2p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fpd2p * mdurum.Value.PD2P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region END
                        if (mdurum.Value.END > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "END";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.END.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fend.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fend * mdurum.Value.END).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }

                        #endregion
                        #region PIT
                        if (mdurum.Value.PIT > 0)
                        {



                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PIT";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.PIT.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fpit.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fpit * mdurum.Value.PIT).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PITE
                        if (mdurum.Value.PITE > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PITE";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.PITE.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fpite.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fpite * mdurum.Value.PITE).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL1
                        if (mdurum.Value.VL1 > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VD1";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.VL1.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fvl1.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fvl1 * mdurum.Value.VL1).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL1P
                        if (mdurum.Value.VL1P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VD1P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.VL1P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fvl1p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fvl1p * mdurum.Value.VL1P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL2
                        if (mdurum.Value.VL2 > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VD2";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.VL2.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fvl2.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fvl2 * mdurum.Value.VL2).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL2P
                        if (mdurum.Value.VL2P > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VD2P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.VL2P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fvl2p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fvl2p * mdurum.Value.VL2P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VIT
                        if (mdurum.Value.VIT > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VIT";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.VIT.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fvit.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fvit * mdurum.Value.VIT).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region KRMD1
                        if (mdurum.Value.KRMD1 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "KRMD1";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.KRMD1.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fkrmd1.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fkrmd1 * mdurum.Value.KRMD1).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }

                        #endregion
                        #region BD1
                        if (mdurum.Value.BD1 > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "BD1";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.BD1.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fbd1.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fbd1 * mdurum.Value.BD1).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region BD1P
                        if (mdurum.Value.BD1P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "BD1P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.BD1P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fbd1p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fbd1p * mdurum.Value.BD1P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region ANPRO
                        if (mdurum.Value.ANPRO > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "ANALİZPRO";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.ANPRO.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.FanPro.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.FanPro * mdurum.Value.ANPRO).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region MKK
                        if (mdurum.Value.MKK > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "MKK";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.MKK.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fmkk.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fmkk * mdurum.Value.MKK).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }

                        #endregion
                        #region TARAMA
                        if (mdurum.Value.TARAMA > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "TARAMA";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.TARAMA.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ftarama.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ftarama * mdurum.Value.TARAMA).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }

                        #endregion
                        #region GKKUL
                        if (mdurum.Value.GKKUL > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "GKKUL";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.GKKUL.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fgkkul.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fgkkul * mdurum.Value.GKKUL).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }

                        #endregion
                    }


                    #region yurdisilar


                    foreach (var mdurum in musteridurum.ydurumdictionary)
                    {

                        #region PD1
                        if (mdurum.Value.PD1 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PD1";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.PD1.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ypd1.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ypd1 * mdurum.Value.PD1).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PD1P
                        if (mdurum.Value.PD1P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PD1P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.PD1P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ypd1p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ypd1p * mdurum.Value.PD1P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }

                        #endregion
                        #region PD2
                        if (mdurum.Value.PD2 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PD2";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.PD2.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ypd2.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ypd2 * mdurum.Value.PD2).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PD2P
                        if (mdurum.Value.PD2P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PD2P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.PD2P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ypd2p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ypd2p * mdurum.Value.PD2P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region END
                        if (mdurum.Value.END > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "END";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.END.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Yend.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Yend * mdurum.Value.END).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PIT
                        if (mdurum.Value.PIT > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PIT";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.PIT.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ypit.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ypit * mdurum.Value.PIT).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PITE
                        if (mdurum.Value.PITE > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PITE";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.PITE.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ypite.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ypite * mdurum.Value.PITE).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL1
                        if (mdurum.Value.VL1 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VD1";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.VL1.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Yvl1.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Yvl1 * mdurum.Value.VL1).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL1p
                        if (mdurum.Value.VL1P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VD1P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.VL1P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Yvl1p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Yvl1p * mdurum.Value.VL1P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL2
                        if (mdurum.Value.VL2 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VD2";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.VL2.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Yvl2.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Yvl2 * mdurum.Value.VL2).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL2P
                        if (mdurum.Value.VL2P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VD2P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.VL2P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Yvl2p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Yvl2p * mdurum.Value.VL2P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VIT
                        if (mdurum.Value.VIT > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VIT";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.VIT.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Yvit.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Yvit * mdurum.Value.VIT).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region KRMD1
                        if (mdurum.Value.KRMD1 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "KRMD1";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.KRMD1.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ykrmd1.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ykrmd1 * mdurum.Value.KRMD1).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region BD1

                        if (mdurum.Value.BD1 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "BD1";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.BD1.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ybd1.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ybd1 * mdurum.Value.BD1).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region BD1P
                        if (mdurum.Value.BD1P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "BD1P";
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.mensei = mdurum.Value.mensei;
                            alan.cihaztoplam = mdurum.Value.BD1P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ybd1p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ybd1p * mdurum.Value.BD1P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region BD2
                        if (mdurum.Value.BD2 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "BD2";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.BD2.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ybd2.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ybd2 * mdurum.Value.BD2).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region ANPRO
                        if (mdurum.Value.ANPRO > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "ANALİZPRO";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.ANPRO.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.YanPro.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.YanPro * mdurum.Value.ANPRO).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region MKK
                        if (mdurum.Value.MKK > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "MKK";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.MKK.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ymkk.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ymkk * mdurum.Value.MKK).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region TARAMA
                        if (mdurum.Value.TARAMA > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "TARAMA";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.TARAMA.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ytarama.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ytarama * mdurum.Value.TARAMA).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region GKKUL
                        if (mdurum.Value.GKKUL > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "GKKUL";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.cihaztoplam = mdurum.Value.GKKUL.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ygkkul.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ygkkul * mdurum.Value.GKKUL).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                    }
                    #endregion

                    var cellarray = new object[alanlarlistesi.Count + 1, 14];
                    for (int i = 0; i < cellarray.GetLength(0); i++)
                    {
                        for (int j = 0; j < cellarray.GetLength(1); j++)
                            cellarray[i, j] = "";
                    }

                    cellarray[0, 0] = "Abone Kodu";
                    cellarray[0, 1] = "Abone Adı Soyadı";
                    cellarray[0, 2] = "Adres";
                    cellarray[0, 3] = "Şehir";
                    cellarray[0, 4] = "postakodu";
                    cellarray[0, 5] = "Ülke";
                    cellarray[0, 6] = "Raporlama Bilgi Kodu";
                    cellarray[0, 7] = "Yurtiçi / YurtDışı";
                    cellarray[0, 8] = "Erişim Tipi";
                    cellarray[0, 9] = "Toplam Cihaz Adet";
                    cellarray[0, 10] = "Bilgi Bedeli";
                    cellarray[0, 11] = "Toplam Bedel";
                    cellarray[0, 12] = "yetkilendirme Tarihi";
                    cellarray[0, 13] = "Bedelsiz Açıklama";



                    for (int i = 0; i < alanlarlistesi.Count; i++)
                    {

                        cellarray[i + 1, 0] = alanlarlistesi[i].aboneno;
                        cellarray[i + 1, 1] = alanlarlistesi[i].aboneadi;
                        cellarray[i + 1, 2] = alanlarlistesi[i].adres;
                        cellarray[i + 1, 3] = alanlarlistesi[i].sehir;
                        cellarray[i + 1, 4] = alanlarlistesi[i].postakod;
                        cellarray[i + 1, 5] = alanlarlistesi[i].ulke;
                        cellarray[i + 1, 6] = alanlarlistesi[i].bilgikodu;
                        cellarray[i + 1, 7] = alanlarlistesi[i].mensei;
                        cellarray[i + 1, 8] = alanlarlistesi[i].erisimtipi;
                        cellarray[i + 1, 9] = alanlarlistesi[i].cihaztoplam;
                        cellarray[i + 1, 10] = alanlarlistesi[i].bilgibedel;
                        cellarray[i + 1, 11] = alanlarlistesi[i].toplambedel;
                        cellarray[i + 1, 12] = alanlarlistesi[i].yetkikendirmetar;
                        cellarray[i + 1, 13] = alanlarlistesi[i].bedelsizaciklama;

                    }




                    ExcellApp.ScreenUpdating = false;
                    if (ExcellApp.Visible)
                    {
                        Microsoft.Office.Interop.Excel.Range range = excelsheet.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                        range.Value = cellarray;
                        range.Columns.AutoFit();
                    }
                    ExcellApp.ScreenUpdating = true;



                }
                catch
                {


                    ExcellApp.ScreenUpdating = true;
                }

            }
            public static void lisansFiyatlariniOku(crmDFNDataContext crm)
            {
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
                        case "MKK":
                            MyTools.lisansfiyatlari.Fmkk = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ymkk = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "TARAMA":
                            MyTools.lisansfiyatlari.Ftarama = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ytarama = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "GKKUL":
                            MyTools.lisansfiyatlari.Fgkkul = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ygkkul = (decimal)lfiyat.YurtDisiFiyat;
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
                        default:
                            break;

                    }

                }

            }
            public static void BorsaTamListe()
            {
                ExcelDurumFormAc();
                Thread T = new Thread((obj) =>
                {


                    var filename = System.Windows.Forms.Application.StartupPath + "\\" + "BorsaTamListe" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".XLSX";

                    SafeDeleteFile(filename);
                    var excelworkbook = GetExcelWorkbook(filename);
                    if (excelworkbook != null)
                    {
                        // excelworkbook.Save();
                        excelworkbook.Close(false);
                    }
                    excelworkbook = OpenExcelWorkbook(filename);
                    if (excelworkbook == null) return;
                    crmDFNDataContext crm = new crmDFNDataContext();





                    lisansFiyatlariniOku(crm);

                    ExcelDurumYaz("BorsaTamListe Aktarım Başladı");
                    #region RaporlamaKodlari

                    var excelsheetRk = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.get_Item(1);
                    excelsheetRk.Name = "Raporlama Kodları";

                    var RPList = new List<RaporlamaKodu>();
                    RPList.Add(new RaporlamaKodu { EskiKod = "EPG", YeniKod = "Eşanlı Portföy Gösterimi" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "KRMD1", YeniKod = "Karma Düzey 1" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "MKK", YeniKod = "MKK" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "PD1P", YeniKod = "PAY Düzey 1P" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "PD2", YeniKod = "PAY Düzey 2" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "PD2P", YeniKod = "PAY Düzey 2P" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "VD1P", YeniKod = "VİOP Düzey 1P" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "VD2", YeniKod = "VİOP Düzey 2" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "VD2P", YeniKod = "VİOP Düzey 2P" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "BD1P", YeniKod = "BAP Düzey 1P" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "BD2", YeniKod = "BAP Düzey 2" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "KD1P", YeniKod = "KMTP Düzey 1P" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "KD2", YeniKod = "KMTP Düzey 2" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "END", YeniKod = "Borsa İstanbul Endeksleri" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "PITE", YeniKod = "Pay İşlem Tarafı Eşanlı" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "PIT", YeniKod = "Pay İşlem Tarafı Günsonu" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "PVA", YeniKod = "Pay Veri Analitikleri" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "VIT", YeniKod = "VİOP İşlem Tarafı Günsonu" });
                    // RPList.Add(new RaporlamaKodu { EskiKod = "TARAMA", YeniKod = "TARAMA" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "GKKUL", YeniKod = "Kullanıcı Gösterimsiz Kullanım Bedeli " });
                    RPList.Add(new RaporlamaKodu { EskiKod = "GKULKYD", YeniKod = "Gösterimsiz Kul. BIST-KYD Endeksleri" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "GKULEND", YeniKod = "Gösterimsiz Kul. Borsa İstanbul Endeksleri" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "GKULD1P", YeniKod = "Gösterimsiz Kul. Düzey 1P" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "GKULD2", YeniKod = "Gösterimsiz Kul. Düzey 2/2P" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "GKULPITE", YeniKod = "Gösterimsiz Kul. Pay İşlem Tarafı Eşanlı" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "GKULPVA", YeniKod = "Gösterimsiz Kul. Üye Pay Veri Analitikleri" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "GUYEKYD", YeniKod = "Gösterimsiz Kul.Üye BIST-KYD Endeksleri" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "GUYEEND", YeniKod = "Gösterimsiz Kul. Üye Borsa İstanbul Endeksleri" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "GUYED1P", YeniKod = "Gösterimsiz Kul.Üye Düzey 1P" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "GUYED2", YeniKod = "Gösterimsiz Kul. Üye Düzey 2/2P" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "GUYEPITE", YeniKod = "Gösterimsiz Kul. Üye Pay İşlem Tarafı Eşanlı" });
                    RPList.Add(new RaporlamaKodu { EskiKod = "GUYEPVA", YeniKod = "Gösterimsiz Kul. Üye Pay Veri Analitikleri" });

                    var cellarray3 = new object[57, 2];
                    cellarray3[0, 0] = "Kod";
                    cellarray3[0, 1] = "Raporlama Kodu";
                    //cellarray3[0, 2] = "Yurtiçi/Yurtdışı";


                    for (int i = 0; i < RPList.Count; i++)
                    {
                        cellarray3[i + 1, 0] = RPList[i].EskiKod;
                        cellarray3[i + 1, 1] = RPList[i].YeniKod;
                        //cellarray3[i, 2] = RPList[i - 1].Mensei;
                    }



                    Microsoft.Office.Interop.Excel.Range rangerp = excelsheetRk.Cells.get_Resize(cellarray3.GetLength(0), cellarray3.GetLength(1));
                    rangerp.Value = cellarray3;
                    rangerp.Columns.AutoFit();


                    //RPList.Add(new RaporlamaKodu { EskiKod = "EPG", YeniKod = "Eşanlı Portföy Gösterimi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "KRMD1", YeniKod = "Karma Düzey 1"});
                    //RPList.Add(new RaporlamaKodu { EskiKod = "BD1P", YeniKod = "BAP Düzey 1P" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "BD2", YeniKod = "BAP Düzey 2" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "END", YeniKod = "Borsa İstanbul Endeksleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GKULKYD", YeniKod = "Gösterimsiz Kul. BIST-KYD Endeksleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GKULEND", YeniKod = "Gösterimsiz Kul. Borsa İstanbul Endeksleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GKULD1P", YeniKod = "Gösterimsiz Kul. Düzey 1/1P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GKULD2", YeniKod = "Gösterimsiz Kul. Düzey 2/2P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GKULPITE", YeniKod = "Gösterimsiz Kul. Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GKULPVA", YeniKod = "Gösterimsiz Kul. Üye Pay Veri Analitikleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEKYD", YeniKod = "Gösterimsiz Kul. Üye BIST-KYD Endeksleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEEND", YeniKod = "Gösterimsiz Kul. Üye Borsa İstanbul Endeksleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GUYED1P", YeniKod = "Gösterimsiz Kul. Üye Düzey 1P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GUYED2", YeniKod = "Gösterimsiz Kul. Üye Düzey 2/2P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEPITE", YeniKod = "Gösterimsiz Kul. Üye Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEPVA", YeniKod = "Gösterimsiz Kul. Üye Pay Veri Analitikleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "KD1P", YeniKod = "KMTP Düzey 1P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "KD2", YeniKod = "KMTP Düzey 2 - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "PD1P", YeniKod = "PAY Düzey 1P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "PD2", YeniKod = "PAY Düzey 2 - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "PD2P", YeniKod = "PAY Düzey 2P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "PITE", YeniKod = "Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "PIT", YeniKod = "Pay İşlem Tarafı Günsonu - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "PVA", YeniKod = "Pay Veri Analitikleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "VD1P", YeniKod = "VİOP Düzey 1P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "VD2", YeniKod = "VİOP Düzey 2 - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "VD2P", YeniKod = "VİOP Düzey 2P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "VIT", YeniKod = "VİOP İşlem Tarafı Günsonu - Değişken - Yurtiçi", Mensei = "Yurtiçi" });

                    //RPList.Add(new RaporlamaKodu { EskiKod = "KRMD1", YeniKod = "Karma Düzey 1 - 1K Kullanıcı Paketi - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "BD1P", YeniKod = "BAP Düzey 1P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "BD2", YeniKod = "BAP Düzey 2 - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "ANALİZPRO", YeniKod = "Analiz Pro - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "END", YeniKod = "Borsa İstanbul Endeksleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GKULKYD", YeniKod = "Gösterimsiz Kul. BIST-KYD Endeksleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GKULEND", YeniKod = "Gösterimsiz Kul. Borsa İstanbul Endeksleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GKULD1P", YeniKod = "Gösterimsiz Kul. Düzey 1/1P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GKULD2", YeniKod = "Gösterimsiz Kul. Düzey 2/2P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GKULPITE", YeniKod = "Gösterimsiz Kul. Pay İşlem Tarafı Eşanlı - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GKULPVA", YeniKod = "Gösterimsiz Kul. Üye Pay Veri Analitikleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEKYD", YeniKod = "Gösterimsiz Kul. Üye BIST-KYD Endeksleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEEND", YeniKod = "Gösterimsiz Kul. Üye Borsa İstanbul Endeksleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GUYED1P", YeniKod = "Gösterimsiz Kul. Üye Düzey 1P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GUYED2", YeniKod = "Gösterimsiz Kul. Üye Düzey 2/2P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEPITE", YeniKod = "Gösterimsiz Kul. Üye Pay İşlem Tarafı Eşanlı - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEPVA", YeniKod = "Gösterimsiz Kul. Üye Pay Veri Analitikleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "KD1P", YeniKod = "KMTP Düzey 1P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "KD2", YeniKod = "KMTP Düzey 2 - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "PD1P", YeniKod = "PAY Düzey 1P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "PD2", YeniKod = "PAY Düzey 2 - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "PD2P", YeniKod = "PAY Düzey 2P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "PITE", YeniKod = "Pay İşlem Tarafı Eşanlı - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "PIT", YeniKod = "Pay İşlem Tarafı Günsonu - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "PVA", YeniKod = "Pay Veri Analitikleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "VD1P", YeniKod = "VİOP Düzey 1P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "VD2", YeniKod = "VİOP Düzey 2 - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "VD2P", YeniKod = "VİOP Düzey 2P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                    //RPList.Add(new RaporlamaKodu { EskiKod = "VIT", YeniKod = "VİOP İşlem Tarafı Günsonu - Değişken - Yurtdışı", Mensei = "Yurtdışı" });


                    //var cellarray3 = new object[60, 3];//57-60
                    //cellarray3[0, 0] = "Eski Kod";
                    //cellarray3[0, 1] = "Yeni Raporlama Kodu";
                    //cellarray3[0, 2] = "Yurtiçi/Yurtdışı";


                    //for (int i = 1; i < RPList.Count; i++)
                    //{
                    //    cellarray3[i, 0] = RPList[i].EskiKod;
                    //    cellarray3[i, 1] = RPList[i].YeniKod;
                    //    cellarray3[i, 2] = RPList[i].Mensei;
                    //}



                    //Microsoft.Office.Interop.Excel.Range rangerp = excelsheetRk.Cells.get_Resize(cellarray3.GetLength(0), cellarray3.GetLength(1));
                    //rangerp.Value = cellarray3;
                    //rangerp.Columns.AutoFit();

                    //ExcelDurumYaz("Raporlama Kodları Yazıldı");

                    #endregion

                    #region DETAY


                    ExcelDurumYaz("DETAY Sayfa Aktarımı Başladı");
                    var excelsheetDetay = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.Add();
                    excelsheetDetay.Name = "DETAY";

                    var Sorgu = crm.Users.Where(x => x.StatusId == 1);
                    excelsheetDetay.Activate();

                    musteridurum.durumdictionary.Clear();
                    musteridurum.ydurumdictionary.Clear();

                    var sorguDetay = crm.Users.Where(x => (x.LisansDurum.YayinDurumu == true && x.StatusId == 1) || (x.LisansDurum.YayinDurumu == true));
                    var count = sorguDetay.Count();
                    var say = 0;
                    foreach (var item in sorguDetay)
                    {


                        musteridurum m = new musteridurum();
                        #region LISANLARIHESAPLA
                        //if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == false && item.LisansDurum.PayL2 == false && item.LisansDurum.Pd2P == false)
                        //{

                        //    m.PD1 = 1;
                        //}
                        try
                        {
                            if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == false && item.LisansDurum.Pd2P == false)
                            {
                                m.PD1P = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.PayLPStart.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.PayLPEnd.Value.ToShortDateString();

                            }
                            else if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == true && item.LisansDurum.Pd2P == false)
                            {
                                m.PD2 = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.PayL2Start.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.PayL2End.Value.ToShortDateString();
                            }
                            else if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == true && item.LisansDurum.Pd2P == true)
                            {
                                m.PD2P = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.Pd2PStart.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.Pd2PEnd.Value.ToShortDateString();
                            }



                            if (item.LisansDurum.PayX == true)
                            {
                                m.END = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.PayXStart.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.PayXEnd.Value.ToShortDateString();
                            }


                            if (item.LisansDurum.PITE == true)
                            {
                                m.PITE = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.PayPiteStart.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.PayPiteEnd.Value.ToShortDateString();
                            }


                            if (item.LisansDurum.PayGS == true && item.LisansDurum.PITE == false)
                            {
                                m.PIT = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.PayGSStart.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.PayGSEnd.Value.ToShortDateString();
                            }




                            //if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == false && item.LisansDurum.ViopL2 == false && item.LisansDurum.Vd2P == false)
                            //{

                            //    m.VL1 = 1;
                            //}
                            if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == false && item.LisansDurum.Vd2P == false)
                            {
                                m.VL1P = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.ViopLPStart.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.ViopLPEnd.Value.ToShortDateString();
                            }
                            else if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == true && item.LisansDurum.Vd2P == false)
                            {
                                m.VL2 = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.ViopL2Start.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.ViopL2End.Value.ToShortDateString();
                            }
                            else if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == true && item.LisansDurum.Vd2P == true)
                            {
                                m.VL2P = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.Vd2PStart.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.Vd2PEnd.Value.ToShortDateString();
                            }

                            if (item.LisansDurum.ViopGS == true)
                            {
                                m.VIT = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.ViopGSStart.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.ViopGSEnd.Value.ToShortDateString();
                            }

                            if (item.LisansDurum.COMEX == true)
                            {
                                m.KRMD1 = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.KRMD1Start.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.KRMD1End.Value.ToShortDateString();
                            }
                            if (item.LisansDurum.MKK == true)
                            {
                                m.MKK = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.MKKStart.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.MKKEnd.Value.ToShortDateString();
                            }
                            if (item.LisansDurum.TARAMA == true)
                            {
                                m.TARAMA = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.TaramaStart.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.TaramaEnd.Value.ToShortDateString();
                            }

                            if (item.LisansDurum.GKKUL == true)
                            {
                                m.GKKUL = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.GKKULStart.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.GKKULEnd.Value.ToShortDateString();
                            }

                            if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == false && item.LisansDurum.TahvilL2 == false)
                            {
                                m.BD1 = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.TahvilL1Start.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.TahvilL1End.Value.ToShortDateString();
                            }
                            else if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == true && item.LisansDurum.TahvilL2 == false)
                            {
                                m.BD1P = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.TahvilLPStart.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.TahvilLPEnd.Value.ToShortDateString();
                            }
                            else if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == true && item.LisansDurum.TahvilL2 == true)
                            {

                                m.BD2 = 1;
                                m.lisansYenilemeTarihi = item.LisansDurum.TahvilL2Start.Value.ToShortDateString();
                                m.lisansSonlanmaTarihi = item.LisansDurum.TahvilL2End.Value.ToShortDateString();
                            }
                        }
                        catch (Exception EX)
                        {
                            MessageBox.Show($"lisans hatası: {EX.Message}");
                        }

                        #endregion

                        //var musteri = crm.Musterilers.FirstOrDefault(x => x.MusteriNo == item.PmtsNo);
                        //m.musteriadi = (musteri != null) ? musteri.MusteriAdi : m.musteriadi = item.Name + " " + item.Surname;

                        m.musteriadi = item.Name + " " + item.Surname;

                        if (item.Iletisim != null)
                        {
                            m.adres = (item.Iletisim.acikadres == null) ? "" : item.Iletisim.acikadres.Replace("\n", " ");
                            m.sehir = (item.Iletisim.Il == null) ? "" : item.Iletisim.Il.IlAdi;
                            m.ulke = (item.Iletisim.Ulke == null) ? "" : item.Iletisim.Ulke.UlkeAdi;
                            if (m.adres == "" & m.sehir == "")
                            {
                                m.adres = "İstanbul";
                                m.sehir = "İstanbul";
                            }
                            else if (m.adres == "" && m.sehir != "")
                            {
                                m.adres = m.sehir;
                            }
                        }

                        if (item.BaslangicTarihi != null)
                            m.yetkilendirmetarihi = item.BaslangicTarihi.Value.ToShortDateString();
                        m.mensei = (item.MusteriMenseiID == 2) ? "YurtDışı" : m.mensei = "Yurtİçi";
                        m.Bedelsizaciklama = (item.StatusId == 1) ? "" : m.Bedelsizaciklama = "Bedelsiz";


                        if (item.MusteriMenseiID == 2)
                             musteridurum.ekleY(item.UserName, m);
                          //  musteridurum.ekleY(item.tckno, m);
                        else
                              musteridurum.ekle(item.UserName, m);
                           // musteridurum.ekle(item.tckno, m);
                        say++;
                        ExcelDurumYaz("Detay Sayfa Aktarılıyor -->  " + (say).ToString() + " / " + count.ToString());
                    }


                    var alanlarlistesi = new List<alanlar>();
                    foreach (var mdurum in musteridurum.durumdictionary)
                    {

                        #region PD1
                        //if (mdurum.Value.PD1 > 0)
                        //{

                        //    var alan = new alanlar();
                        //    alan.aboneno = mdurum.Key;
                        //    alan.aboneadi = mdurum.Value.musteriadi;
                        //    alan.adres = mdurum.Value.adres;
                        //    alan.sehir = mdurum.Value.sehir;
                        //    alan.ulke = mdurum.Value.ulke;
                        //    alan.bilgikodu = "PD1";
                        //    //alan.bilgikodu = "PAY Düzey 1 - Değişken - Yurtiçi";
                        //    alan.mensei = mdurum.Value.mensei;
                        //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                        //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                        //    alan.cihaztoplam = mdurum.Value.PD1.ToString();
                        //    if (mdurum.Value.Bedelsizaciklama == "")
                        //    {
                        //        alan.bilgibedel = MyTools.lisansfiyatlari.Fpd1.ToString("0.00");
                        //        alan.toplambedel = (MyTools.lisansfiyatlari.Fpd1 * mdurum.Value.PD1).ToString("0.00");
                        //    }
                        //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                        //    alanlarlistesi.Add(alan);
                        //}
                        #endregion
                        #region PD1P
                        if (mdurum.Value.PD1P > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PAY Düzey 1P - Değişken - Yurtiçi";
                           // alan.bilgikodu = "PD1P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.PD1P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fpd1p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fpd1p * mdurum.Value.PD1P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PD2
                        if (mdurum.Value.PD2 > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PAY Düzey 2 - Değişken - Yurtiçi";
                            //alan.bilgikodu = "PD2";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.PD2.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fpd2.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fpd2 * mdurum.Value.PD2).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PD2P
                        if (mdurum.Value.PD2P > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PAY Düzey 2P - Değişken - Yurtiçi";
                            //alan.bilgikodu = "PD2P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.PD2P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fpd2p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fpd2p * mdurum.Value.PD2P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region END
                        if (mdurum.Value.END > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "Borsa İstanbul Endeksleri - Değişken - Yurtiçi";
                           //alan.bilgikodu = "END";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.END.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fend.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fend * mdurum.Value.END).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }

                        #endregion
                        #region PIT
                        if (mdurum.Value.PIT > 0)
                        {



                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "Pay İşlem Tarafı Günsonu - Değişken - Yurtiçi";
                           // alan.bilgikodu = "PIT";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.PIT.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fpit.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fpit * mdurum.Value.PIT).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PITE
                        if (mdurum.Value.PITE > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi";
                          //  alan.bilgikodu = "PITE";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.PITE.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fpite.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fpite * mdurum.Value.PITE).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL1
                        //if (mdurum.Value.VL1 > 0)
                        //{

                        //    var alan = new alanlar();
                        //    alan.aboneno = mdurum.Key;
                        //    alan.aboneadi = mdurum.Value.musteriadi;
                        //    alan.adres = mdurum.Value.adres;
                        //    alan.sehir = mdurum.Value.sehir;
                        //    alan.ulke = mdurum.Value.ulke;
                        //    alan.bilgikodu = "VİOP Düzey 1 - Değişken - Yurtiçi";
                        //    alan.bilgikodu = "VD1";
                        //    alan.mensei = mdurum.Value.mensei;
                        //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                        //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                        //    alan.cihaztoplam = mdurum.Value.VL1.ToString();
                        //    if (mdurum.Value.Bedelsizaciklama == "")
                        //    {
                        //        alan.bilgibedel = MyTools.lisansfiyatlari.Fvl1.ToString("0.00");
                        //        alan.toplambedel = (MyTools.lisansfiyatlari.Fvl1 * mdurum.Value.VL1).ToString("0.00");
                        //    }
                        //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                        //    alanlarlistesi.Add(alan);
                        //}
                        #endregion
                        #region VL1P
                        if (mdurum.Value.VL1P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VİOP Düzey 1P - Değişken - Yurtiçi";
                           // alan.bilgikodu = "VD1P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.VL1P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fvl1p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fvl1p * mdurum.Value.VL1P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL2
                        if (mdurum.Value.VL2 > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VİOP Düzey 2 - Değişken - Yurtiçi";
                           // alan.bilgikodu = "VD2";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.VL2.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fvl2.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fvl2 * mdurum.Value.VL2).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL2P
                        if (mdurum.Value.VL2P > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VİOP Düzey 2P- Değişken - Yurtiçi";
                          //  alan.bilgikodu = "VD2P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.VL2P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fvl2p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fvl2p * mdurum.Value.VL2P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VIT
                        if (mdurum.Value.VIT > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VİOP İşlem Tarafı Günsonu - Değişken - Yurtiçi";
                          //  alan.bilgikodu = "VIT";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.VIT.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fvit.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fvit * mdurum.Value.VIT).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region KRMD1
                        if (mdurum.Value.KRMD1 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "Karma Düzey 1 - Yurtiçi";
                           // alan.bilgikodu = "KRMD1";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.KRMD1.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = "0";
                                alan.toplambedel = "0";
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }

                        #endregion
                        #region BD1
                        if (mdurum.Value.BD1 > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "BAP Düzey 1 - Değişken - Yurtiçi";
                          //  alan.bilgikodu = "BD1";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.BD1.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fbd1.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fbd1 * mdurum.Value.BD1).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region BD1P
                        if (mdurum.Value.BD1P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "BAP Düzey 1P - Değişken - Yurtiçi";
                          //  alan.bilgikodu = "BD1P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.BD1P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fbd1p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fbd1p * mdurum.Value.BD1P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region BD2
                        if (mdurum.Value.BD2 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "BAP Düzey 2 - Değişken - Yurtiçi";
                          //  alan.bilgikodu = "BD2";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.BD2.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Fbd2.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Fbd2 * mdurum.Value.BD2).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region MKK
                        if (mdurum.Value.MKK > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "MKK - Yurtiçi";
                          //  alan.bilgikodu = "MKK";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.MKK.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = "0";
                                alan.toplambedel = "0";
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }

                        #endregion
                        #region TARAMA
                        if (mdurum.Value.TARAMA > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "TARAMA - Yurtiçi";
                          //  alan.bilgikodu = "TARAMA";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.TARAMA.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = "0";
                                alan.toplambedel = "0";
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }

                        #endregion
                        #region GKKUL
                        if (mdurum.Value.GKKUL > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "GKKUL - Yurtiçi";
                           // alan.bilgikodu = "GKKUL";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.GKKUL.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = "0";
                                alan.toplambedel = "0";
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }

                        #endregion
                    }


                    #region yurdisilar


                    foreach (var mdurum in musteridurum.ydurumdictionary)
                    {

                        #region PD1
                        //if (mdurum.Value.PD1 > 0)
                        //{
                        //    var alan = new alanlar();
                        //    alan.aboneno = mdurum.Key;
                        //    alan.aboneadi = mdurum.Value.musteriadi;
                        //    alan.adres = mdurum.Value.adres;
                        //    alan.sehir = mdurum.Value.sehir;
                        //    alan.ulke = mdurum.Value.ulke;
                        //    alan.bilgikodu = "PAY Düzey 1 - Değişken - Yurtdışı";
                        //    alan.bilgikodu = "PD1";
                        //    alan.mensei = mdurum.Value.mensei;
                        //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                        //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                        //    alan.cihaztoplam = mdurum.Value.PD1.ToString();
                        //    if (mdurum.Value.Bedelsizaciklama == "")
                        //    {
                        //        alan.bilgibedel = MyTools.lisansfiyatlari.Ypd1.ToString("0.00");
                        //        alan.toplambedel = (MyTools.lisansfiyatlari.Ypd1 * mdurum.Value.PD1).ToString("0.00");
                        //    }
                        //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                        //    alanlarlistesi.Add(alan);
                        //}
                        #endregion
                        #region PD1P
                        if (mdurum.Value.PD1P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PAY Düzey 1P - Değişken - Yurtdışı";
                          //  alan.bilgikodu = "PD1P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.PD1P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ypd1p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ypd1p * mdurum.Value.PD1P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }

                        #endregion
                        #region PD2
                        if (mdurum.Value.PD2 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PAY Düzey 2 - Değişken - Yurtdışı";
                          //  alan.bilgikodu = "PD2";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.PD2.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ypd2.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ypd2 * mdurum.Value.PD2).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PD2P
                        if (mdurum.Value.PD2P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "PAY Düzey 2P- Değişken - Yurtdışı";
                          //  alan.bilgikodu = "PD2P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.PD2P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ypd2p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ypd2p * mdurum.Value.PD2P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region END
                        if (mdurum.Value.END > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "Borsa İstanbul Endeksleri - Değişken - Yurtdışı";
                          //  alan.bilgikodu = "END";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.END.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Yend.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Yend * mdurum.Value.END).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PIT
                        if (mdurum.Value.PIT > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "Pay İşlem Tarafı Günsonu - Değişken - Yurtdışı";
                          //  alan.bilgikodu = "PIT";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.PIT.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ypit.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ypit * mdurum.Value.PIT).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region PITE
                        if (mdurum.Value.PITE > 0)
                        {

                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "Pay İşlem Tarafı Eşanlı - Değişken - Yurtdışı";
                          //  alan.bilgikodu = "PITE";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.PITE.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ypite.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ypite * mdurum.Value.PITE).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL1
                        //if (mdurum.Value.VL1 > 0)
                        //{
                        //    var alan = new alanlar();
                        //    alan.aboneno = mdurum.Key;
                        //    alan.aboneadi = mdurum.Value.musteriadi;
                        //    alan.adres = mdurum.Value.adres;
                        //    alan.sehir = mdurum.Value.sehir;
                        //    alan.ulke = mdurum.Value.ulke;
                        //    alan.bilgikodu = "VİOP Düzey 1 - Değişken - Yurtdışı";
                        //    alan.bilgikodu = "VD1";
                        //    alan.mensei = mdurum.Value.mensei;
                        //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                        //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                        //    alan.cihaztoplam = mdurum.Value.VL1.ToString();
                        //    if (mdurum.Value.Bedelsizaciklama == "")
                        //    {
                        //        alan.bilgibedel = MyTools.lisansfiyatlari.Yvl1.ToString("0.00");
                        //        alan.toplambedel = (MyTools.lisansfiyatlari.Yvl1 * mdurum.Value.VL1).ToString("0.00");
                        //    }
                        //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                        //    alanlarlistesi.Add(alan);
                        //}
                        #endregion
                        #region VL1p
                        if (mdurum.Value.VL1P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VİOP Düzey 1P - Değişken - Yurtdışı";
                         //   alan.bilgikodu = "VL1P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.VL1P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Yvl1p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Yvl1p * mdurum.Value.VL1P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL2
                        if (mdurum.Value.VL2 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VİOP Düzey 2 - Değişken - Yurtdışı";
                          //  alan.bilgikodu = "VD2";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.VL2.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Yvl2.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Yvl2 * mdurum.Value.VL2).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VL2P
                        if (mdurum.Value.VL2P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VİOP Düzey 2P - Değişken - Yurtdışı";
                         //   alan.bilgikodu = "VD2P";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.VL2P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Yvl2p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Yvl2p * mdurum.Value.VL2P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region VIT
                        if (mdurum.Value.VIT > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "VİOP İşlem Tarafı Günsonu - Değişken - Yurtdışı";
                         //   alan.bilgikodu = "VIT";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.VIT.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Yvit.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Yvit * mdurum.Value.VIT).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region KRMD1
                        if (mdurum.Value.KRMD1 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "Karma Düzey 1 - Yurtdışı";
                           // alan.bilgikodu = "KRMD1";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.KRMD1.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ykrmd1.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ykrmd1 * mdurum.Value.KRMD1).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region BD1

                        if (mdurum.Value.BD1 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "BAP Düzey 1 - Değişken - Yurtdışı";
                          //  alan.bilgikodu = "BD1";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.BD1.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ybd1.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ybd1 * mdurum.Value.BD1).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region BD1P
                        if (mdurum.Value.BD1P > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "BAP Düzey 1P - Değişken - Yurtdışı";
                          //  alan.bilgikodu = "BD1P";
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.mensei = mdurum.Value.mensei;
                            alan.cihaztoplam = mdurum.Value.BD1P.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ybd1p.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ybd1p * mdurum.Value.BD1P).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion
                        #region BD2
                        if (mdurum.Value.BD2 > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "BAP Düzey 2 - Değişken - Yurtdışı";
                          //  alan.bilgikodu = "BD2";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.BD2.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ybd2.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ybd2 * mdurum.Value.BD2).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion

                        #region MKK
                        if (mdurum.Value.MKK > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "MKK - Yurtdışı";
                          //  alan.bilgikodu = "MKK";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.MKK.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ymkk.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ymkk * mdurum.Value.MKK).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion

                        //#region TARAMA
                        //if (mdurum.Value.TARAMA > 0)
                        //{
                        //    var alan = new alanlar();
                        //    alan.aboneno = mdurum.Key;
                        //    alan.aboneadi = mdurum.Value.musteriadi;
                        //    alan.adres = mdurum.Value.adres;
                        //    alan.sehir = mdurum.Value.sehir;
                        //    alan.ulke = mdurum.Value.ulke;
                        //    alan.bilgikodu = "TARAMA - Yurtdışı";
                        //    alan.bilgikodu = "TARAMA";
                        //    alan.mensei = mdurum.Value.mensei;
                        //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                        //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                        //    alan.cihaztoplam = mdurum.Value.TARAMA.ToString();
                        //    if (mdurum.Value.Bedelsizaciklama == "")
                        //    {
                        //        alan.bilgibedel = MyTools.lisansfiyatlari.Ytarama.ToString("0.00");
                        //        alan.toplambedel = (MyTools.lisansfiyatlari.Ytarama * mdurum.Value.TARAMA).ToString("0.00");
                        //    }
                        //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                        //    alanlarlistesi.Add(alan);
                        //}
                        //#endregion

                        #region GKKUL
                        if (mdurum.Value.GKKUL > 0)
                        {
                            var alan = new alanlar();
                            alan.aboneno = mdurum.Key;
                            alan.aboneadi = mdurum.Value.musteriadi;
                            alan.adres = mdurum.Value.adres;
                            alan.sehir = mdurum.Value.sehir;
                            alan.ulke = mdurum.Value.ulke;
                            alan.bilgikodu = "GKKUL - Yurtdışı";
                          //  alan.bilgikodu = "GKKUL";
                            alan.mensei = mdurum.Value.mensei;
                            alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            alan.lisansSonlanmaTarihi = mdurum.Value.lisansSonlanmaTarihi;
                            alan.cihaztoplam = mdurum.Value.GKKUL.ToString();
                            if (mdurum.Value.Bedelsizaciklama == "")
                            {
                                alan.bilgibedel = MyTools.lisansfiyatlari.Ygkkul.ToString("0.00");
                                alan.toplambedel = (MyTools.lisansfiyatlari.Ygkkul * mdurum.Value.GKKUL).ToString("0.00");
                            }
                            alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            alanlarlistesi.Add(alan);
                        }
                        #endregion

                    }
                    #endregion




                    var cellarray = new object[alanlarlistesi.Count + 10, 16];
                    for (int i = 0; i < cellarray.GetLength(0); i++)
                    {
                        for (int j = 0; j < cellarray.GetLength(1); j++)
                            cellarray[i, j] = "";
                    }


                    var currrow = 0;

                    var date = DateTime.Now;

                    cellarray[currrow, 0] = "Dağıtıcı Adı";
                    cellarray[currrow, 1] = "";
                    currrow++;

                    excelsheetDetay.Application.get_Range("A" + currrow, "B" + currrow).Font.Size = 16;

                    cellarray[currrow, 0] = "Borsa";
                    cellarray[currrow, 1] = "Borsa İstanbul A.Ş.";
                    currrow++;
                    cellarray[currrow, 0] = "Raporlanan Ay";
                    cellarray[currrow, 1] = date.ToString("MMMMMMMMM").ToUpper().Trim() + " - " + date.Year;
                    currrow++;
                    cellarray[currrow, 0] = "Rapor Tipi";
                    cellarray[currrow, 1] = "Detay";

                    currrow++;


                    excelsheetDetay.Application.get_Range("A" + 1, "E" + currrow).Font.Bold = true;
                    excelsheetDetay.Application.get_Range("A" + 1, "E" + currrow).Interior.Color = ColorTranslator.ToOle(Color.FromArgb(192, 192, 192));

                    cellarray[currrow, 0] = "";

                    currrow++;
                    cellarray[currrow, 0] = "Abone Kodu";
                    cellarray[currrow, 1] = "Abone Adı Soyadı";
                    cellarray[currrow, 2] = "Adres";
                    cellarray[currrow, 3] = "Şehir";
                    cellarray[currrow, 4] = "postakodu";
                    cellarray[currrow, 5] = "Ülke";
                    cellarray[currrow, 6] = "Raporlama Bilgi Kodu";
                    cellarray[currrow, 7] = "Yurtiçi / YurtDışı";
                    cellarray[currrow, 8] = "Erişim Tipi";
                    cellarray[currrow, 9] = "Toplam Cihaz Adet";
                    cellarray[currrow, 10] = "Bilgi Bedeli";
                    cellarray[currrow, 11] = "Toplam Bedel";
                    cellarray[currrow, 12] = "Yetkilendirme Tarihi";
                    cellarray[currrow, 13] = "Lisans Yenileme Tarihi";
                    cellarray[currrow, 14] = "Lisans Sonlanma Tarihi";
                    cellarray[currrow, 15] = "Bedelsiz Açıklama";

                    currrow++;

                    excelsheetDetay.Application.get_Range("A" + currrow, "P" + currrow).Interior.Color = ColorTranslator.ToOle(Color.FromArgb(192, 192, 192));
                    excelsheetDetay.Application.get_Range("A" + currrow, "P" + currrow).Font.Bold = true;
                    excelsheetDetay.Application.get_Range("A" + currrow, "P" + currrow).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    excelsheetDetay.Application.get_Range("A" + currrow, "P" + currrow).RowHeight = 60;

                    SafeFreezePanes(excelsheetDetay, 6);


                    for (int i = 0; i < alanlarlistesi.Count; i++)
                    {

                        cellarray[currrow, 0] = alanlarlistesi[i].aboneno;
                        cellarray[currrow, 1] = alanlarlistesi[i].aboneadi;
                        cellarray[currrow, 2] = alanlarlistesi[i].adres;
                        cellarray[currrow, 3] = alanlarlistesi[i].sehir;
                        cellarray[currrow, 4] = alanlarlistesi[i].postakod;
                        cellarray[currrow, 5] = alanlarlistesi[i].ulke;
                        cellarray[currrow, 6] = alanlarlistesi[i].bilgikodu;
                        cellarray[currrow, 7] = alanlarlistesi[i].mensei;
                        cellarray[currrow, 8] = alanlarlistesi[i].erisimtipi;
                        cellarray[currrow, 9] = alanlarlistesi[i].cihaztoplam;
                        cellarray[currrow, 10] = alanlarlistesi[i].bilgibedel;
                        cellarray[currrow, 11] = alanlarlistesi[i].toplambedel;
                        cellarray[currrow, 12] = alanlarlistesi[i].yetkikendirmetar;
                        cellarray[currrow, 13] = alanlarlistesi[i].lisansYenilemeTarihi;
                        cellarray[currrow, 14] = alanlarlistesi[i].lisansSonlanmaTarihi;
                        cellarray[currrow, 15] = alanlarlistesi[i].bedelsizaciklama;
                        currrow++;
                    }




                    Microsoft.Office.Interop.Excel.Range range2 = excelsheetDetay.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                    range2.Value = cellarray;
                    range2.Columns.AutoFit();

                    ExcelDurumYaz("DETAY Sayfa Aktarımı Bitti");

                    #endregion
                    #region OZET

                    Sorgu = crm.Users.Where(x => x.StatusId == 1);

                    var excelsheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.Add();
                    excelsheet.Name = "ÖZET";
                    List<alanlar> alanlar = new List<alanlar>();
                    ExcelDurumYaz("ÖZET Sayfa Aktarılıyor");
                    string RaporKurum = "";
                    #region OzetLisansHesapla



                    //#region BD1
                    //var TahvilYuzeysel = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false).Count();
                    //alanlar.Add(new alanlar { bilgikodu = "BAP Düzey 1 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = TahvilYuzeysel.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fbd1.ToString("0.00"), toplambedel = (TahvilYuzeysel * MyTools.lisansfiyatlari.Fbd1).ToString("0.00") });
                    //var TahvilYuzeysel_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false).Count();
                    //alanlar.Add(new alanlar { bilgikodu = "BAP Düzey 1 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = TahvilYuzeysel_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ybd1.ToString("0.00"), toplambedel = (TahvilYuzeysel_Y * MyTools.lisansfiyatlari.Ybd1).ToString("0.00") });

                    //#endregion
                    #region BD1P
                    var TahvilPlus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "BAP Düzey 1P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = TahvilPlus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fbd1p.ToString("0.00"), toplambedel = (TahvilPlus * MyTools.lisansfiyatlari.Fbd1p).ToString("0.00") });
                    var TahvilPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == true).Count();

                    alanlar.Add(new alanlar { bilgikodu = "BAP Düzey 1P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = TahvilPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ybd1p.ToString("0.00"), toplambedel = (TahvilPlus_Y * MyTools.lisansfiyatlari.Ybd1p).ToString("0.00") });

                    #endregion
                    #region BD2
                    var TahvilDerinlik = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == true && x.LisansDurum.TahvilLP == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "BAP Düzey 2 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = TahvilDerinlik.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fbd2.ToString("0.00"), toplambedel = (TahvilDerinlik * MyTools.lisansfiyatlari.Fbd2).ToString("0.00") });
                    var TahvilDerinlik_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == true && x.LisansDurum.TahvilLP == true).Count();

                    alanlar.Add(new alanlar { bilgikodu = "BAP Düzey 2 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = TahvilDerinlik_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ybd2.ToString("0.00"), toplambedel = (TahvilDerinlik_Y * MyTools.lisansfiyatlari.Ybd2).ToString("0.00") });

                    #endregion

                    #region END
                    var PayEndeks = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayX == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "Borsa İstanbul Endeksleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = PayEndeks.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fend.ToString("0.00"), toplambedel = (PayEndeks * MyTools.lisansfiyatlari.Fend).ToString("0.00") });
                    var PayEndeks_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayX == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "Borsa İstanbul Endeksleri - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = PayEndeks_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yend.ToString("0.00"), toplambedel = (PayEndeks_Y * MyTools.lisansfiyatlari.Yend).ToString("0.00") });

                    #endregion
                    #region GosterimsizKullanim
                    var GKULKYD = 0;
                    var GKULEND = 0;
                    var GKULD1P = 0;
                    var GKULD2 = 0;
                    var GKULPITE = 0;
                    var GKULPVA = 0;

                    var GUYEKYD = 0;
                    var GUYEEND = 0;
                    var GUYED1P = 0;
                    var GUYED2 = 0;
                    var GUYEPITE = 0;
                    var GUYEPVA = 0;

                    GKULEND = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GKULEND == true).Count();
                    GKULD1P = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GKULD1P == true).Count();
                    GKULD2 = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GKULD2 == true).Count();
                    GKULPITE = 0;
                    GKULPVA = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GKULPVA == true).Count();

                    GUYEEND = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GUYEEND == true).Count();
                    GUYED1P = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GUYED1P == true).Count();
                    GUYED2 = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GUYED2 == true).Count();
                    GUYEPITE = 0;
                    GUYEPVA = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GUYEPVA == true).Count();



                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. BIST-KYD Endeksleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GKULKYD.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULKYD.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Borsa İstanbul Endeksleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GKULEND.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULEND.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Düzey 1P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GKULD1P.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULD1P.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Düzey 2/2P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GKULD2.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULD2.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GKULPITE.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULPITE.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Pay Veri Analitikleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GKULPVA.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULPVA.ToString("0.00"), toplambedel = "" });

                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Üye BIST-KYD Endeksleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GUYEKYD.ToString(), bilgibedel = MyTools.lisansfiyatlari.GUYEKYD.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Üye Borsa İstanbul Endeksleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GUYEEND.ToString(), bilgibedel = MyTools.lisansfiyatlari.GUYEEND.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Üye Düzey 1P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GUYED1P.ToString(), bilgibedel = MyTools.lisansfiyatlari.GUYED1P.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Üye Düzey 2/2P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GUYED2.ToString(), bilgibedel = MyTools.lisansfiyatlari.GUYED2.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Üye Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GUYEPITE.ToString(), bilgibedel = MyTools.lisansfiyatlari.GUYEPITE.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Üye Pay Veri Analitikleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GUYEPVA.ToString(), bilgibedel = MyTools.lisansfiyatlari.GUYEPVA.ToString("0.00"), toplambedel = "" });

                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. BIST-KYD Endeksleri - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = GKULKYD.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULKYD.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Borsa İstanbul Endeksleri - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = GKULEND.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULEND.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Düzey 1P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = GKULD1P.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULD1P.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Düzey 2/2P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = GKULD2.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULD2.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Pay İşlem Tarafı Eşanlı - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = GKULPITE.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULPITE.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Pay Veri Analitikleri - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = GKULPVA.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULPVA.ToString("0.00"), toplambedel = "" });

                    #endregion
                    #region KMTP1P
                    alanlar.Add(new alanlar { bilgikodu = "KMTP Düzey 1P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = "0", bilgibedel = "11", toplambedel = "0" });
                    alanlar.Add(new alanlar { bilgikodu = "KMTP Düzey 1P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = "0", bilgibedel = "5", toplambedel = "0" });
                    #endregion
                    #region KMTP2
                    alanlar.Add(new alanlar { bilgikodu = "KMTP Düzey 2 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = "0", bilgibedel = "21", toplambedel = "0" });
                    alanlar.Add(new alanlar { bilgikodu = "KMTP Düzey 2 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = "0", bilgibedel = "9", toplambedel = "0" });
                    #endregion
                    //#region PD1
                    //var PayYuzeysel = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false).Count();
                    //alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 1 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = PayYuzeysel.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpd1.ToString("0.00"), toplambedel = (PayYuzeysel * MyTools.lisansfiyatlari.Fpd1).ToString("0.00") });
                    //var PayYuzeysel_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false).Count();
                    //alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 1 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = PayYuzeysel_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypd1.ToString("0.00"), toplambedel = (PayYuzeysel_Y * MyTools.lisansfiyatlari.Ypd1).ToString("0.00") });

                    //#endregion
                    #region PD1P
                    var PayPlus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 1P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = PayPlus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpd1p.ToString("0.00"), toplambedel = (PayPlus * MyTools.lisansfiyatlari.Fpd1p).ToString("0.00") });
                    var PayPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 1P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = PayPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypd1p.ToString("0.00"), toplambedel = (PayPlus_Y * MyTools.lisansfiyatlari.Ypd1p).ToString("0.00") });

                    #endregion
                    #region PD2
                    var PayDerinlik = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                    alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 2 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = PayDerinlik.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpd2.ToString("0.00"), toplambedel = (PayDerinlik * MyTools.lisansfiyatlari.Fpd2).ToString("0.00") });
                    var PayDerinlik_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                    alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 2 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = PayDerinlik_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypd2.ToString("0.00"), toplambedel = (PayDerinlik_Y * MyTools.lisansfiyatlari.Ypd2).ToString("0.00") });

                    #endregion
                    #region PD2P

                    var PayDerinlikplus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 2P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = PayDerinlikplus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpd2p.ToString("0.00"), toplambedel = (PayDerinlikplus * MyTools.lisansfiyatlari.Fpd2p).ToString("0.00") });
                    var PayDerinlikPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 2P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = PayDerinlikPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypd2p.ToString("0.00"), toplambedel = (PayDerinlikPlus_Y * MyTools.lisansfiyatlari.Ypd2p).ToString("0.00") });
                    //alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 2P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = "0", bilgibedel = "75", toplambedel = "0" });
                    //alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 2P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = "0", bilgibedel = "32", toplambedel = "0" });

                    #endregion

                    #region PIT
                    var PayGs = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayGS == true && x.LisansDurum.PITE == false).Count();
                    alanlar.Add(new alanlar { bilgikodu = "Pay İşlem Tarafı Günsonu - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = PayGs.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpit.ToString("0.00"), toplambedel = (PayGs * MyTools.lisansfiyatlari.Fpit).ToString("0.00") });
                    var PayGs_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayGS == true && x.LisansDurum.PITE == false).Count();
                    alanlar.Add(new alanlar { bilgikodu = "Pay İşlem Tarafı Günsonu - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = PayGs_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypit.ToString("0.00"), toplambedel = (PayGs_Y * MyTools.lisansfiyatlari.Ypit).ToString("0.00") });

                    #endregion
                    #region AND
                    alanlar.Add(new alanlar { bilgikodu = "Pay Veri Analitikleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = "0", bilgibedel = "44", toplambedel = "0" });
                    alanlar.Add(new alanlar { bilgikodu = "Pay Veri Analitikleri - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = "0", bilgibedel = "19", toplambedel = "0" });
                    #endregion
                    #region PITE
                    var pite = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PITE == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = pite.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpite.ToString("0.00"), toplambedel = (pite * MyTools.lisansfiyatlari.Fpite).ToString("0.00") });
                    var pite_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PITE == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "Pay İşlem Tarafı Eşanlı - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = pite_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypva.ToString("0.00"), toplambedel = (pite_Y * MyTools.lisansfiyatlari.Ypite).ToString("0.00") });

                    #endregion

                    //#region VD1
                    //var viopYuzeysel = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false).Count();
                    //alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 1 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = viopYuzeysel.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvl1.ToString("0.00"), toplambedel = (viopYuzeysel * MyTools.lisansfiyatlari.Fvl1).ToString("0.00") });
                    //var viopYuzeysel_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false).Count();
                    //alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 1 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = viopYuzeysel_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvl1.ToString("0.00"), toplambedel = (viopYuzeysel_Y * MyTools.lisansfiyatlari.Yvl1).ToString("0.00") });


                    //#endregion
                    #region VD1P
                    var viopPlus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 1P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = viopPlus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvl1p.ToString("0.00"), toplambedel = (viopPlus * MyTools.lisansfiyatlari.Fvl1p).ToString("0.00") });
                    var viopPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 1P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = viopPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvl1p.ToString("0.00"), toplambedel = (viopPlus_Y * MyTools.lisansfiyatlari.Yvl1p).ToString("0.00") });

                    #endregion
                    #region VD2
                    var viopDerinlik = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                    alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 2 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = viopDerinlik.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvl2.ToString("0.00"), toplambedel = (viopDerinlik * MyTools.lisansfiyatlari.Fvl2).ToString("0.00") });
                    var viopDerinlik_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                    alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 2 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = viopDerinlik_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvl2.ToString("0.00"), toplambedel = (viopDerinlik_Y * MyTools.lisansfiyatlari.Yvl2).ToString("0.00") });

                    #endregion
                    #region VD2P
                    var viopDerinlikPlus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 2P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = viopDerinlikPlus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvl2p.ToString("0.00"), toplambedel = (viopDerinlikPlus * MyTools.lisansfiyatlari.Fvl2p).ToString("0.00") });
                    var viopDerinlikPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 2P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = viopDerinlikPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvl2p.ToString("0.00"), toplambedel = (viopDerinlikPlus_Y * MyTools.lisansfiyatlari.Yvl2p).ToString("0.00") });

                    #endregion
                    #region VIT

                    var viopGS = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "VİOP İşlem Tarafı Günsonu - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = viopGS.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvit.ToString("0.00"), toplambedel = (viopGS * MyTools.lisansfiyatlari.Fvit).ToString("0.00") });
                    var viopGS_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "VİOP İşlem Tarafı Günsonu - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = viopGS_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvit.ToString("0.00"), toplambedel = (viopGS_Y * MyTools.lisansfiyatlari.Yvit).ToString("0.00") });

                    #endregion
                    #region MKK

                    var mkk = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.MKK == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "MKK - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = mkk.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fmkk.ToString("0.00"), toplambedel = (mkk * MyTools.lisansfiyatlari.Fmkk).ToString("0.00") });
                    var mkk_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.MKK == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "MKK - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = mkk_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ymkk.ToString("0.00"), toplambedel = (mkk_Y * MyTools.lisansfiyatlari.Ymkk).ToString("0.00") });

                    #endregion
                    //#region TARAMA

                    //var tarama = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TARAMA == true).Count();
                    //alanlar.Add(new alanlar { bilgikodu = "TARAMA - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = tarama.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ftarama.ToString("0.00"), toplambedel = (tarama * MyTools.lisansfiyatlari.Ftarama).ToString("0.00") });
                    //var tarama_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();
                    //alanlar.Add(new alanlar { bilgikodu = "TARAMA - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = tarama_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ytarama.ToString("0.00"), toplambedel = (tarama_Y * MyTools.lisansfiyatlari.Ytarama).ToString("0.00") });

                    //#endregion
                    #region GKKUL

                    var gkkul = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.GKKUL == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "GKKUL - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = gkkul.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fgkkul.ToString("0.00"), toplambedel = (gkkul * MyTools.lisansfiyatlari.Fgkkul).ToString("0.00") });
                    var gkkul_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.GKKUL == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "GKKUL - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = gkkul_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ygkkul.ToString("0.00"), toplambedel = (gkkul_Y * MyTools.lisansfiyatlari.Ygkkul).ToString("0.00") });

                    #endregion
                    #region KARMA

                    var karma1K = 0;
                    var karma5K = 0;
                    var karma10K = 0;
                    var karma20K = 0;
                    var karma50K = 0;
                    var karma100K = 0;
                    var karmaSINIRSIZ = 0;
                    var krmd1 = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.COMEX == true).Count();

                    if (krmd1 > 0 && krmd1 <= 1000) karma1K = krmd1;
                    if (krmd1 > 1000 && krmd1 <= 5000) karma5K = krmd1;
                    if (krmd1 > 5000 && krmd1 <= 10000) karma10K = krmd1;
                    if (krmd1 > 10000 && krmd1 <= 20000) karma20K = krmd1;
                    if (krmd1 > 20000 && krmd1 <= 50000) karma50K = krmd1;
                    if (krmd1 > 50000 && krmd1 <= 100000) karma100K = krmd1;
                    if (krmd1 > 100000) karmaSINIRSIZ = krmd1;

                    alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - 1K Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karma1K.ToString(), bilgibedel = MyTools.lisansfiyatlari.karma1k.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - 5K Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karma5K.ToString(), bilgibedel = MyTools.lisansfiyatlari.karma5k.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - 10K Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karma10K.ToString(), bilgibedel = MyTools.lisansfiyatlari.karma10k.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - 20K Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karma20K.ToString(), bilgibedel = MyTools.lisansfiyatlari.karma20k.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - 50K Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karma50K.ToString(), bilgibedel = MyTools.lisansfiyatlari.karma50k.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - 100K Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karma100K.ToString(), bilgibedel = MyTools.lisansfiyatlari.karma100k.ToString("0.00"), toplambedel = "" });
                    alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - Sınırsız Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karmaSINIRSIZ.ToString(), bilgibedel = MyTools.lisansfiyatlari.karmaSINIRSIZ.ToString("0.00"), toplambedel = "" });


                    var krmd1_y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.COMEX == true).Count();
                    alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = krmd1_y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ykrmd1.ToString("0.00"), toplambedel = (krmd1_y * MyTools.lisansfiyatlari.Ykrmd1).ToString("0.00") });

                    #endregion




                    #endregion
                    cellarray = new object[alanlar.Count + 20, 6];
                    for (int i = 0; i < cellarray.GetLength(0); i++)
                    {
                        for (int j = 0; j < cellarray.GetLength(1); j++)
                            cellarray[i, j] = "";
                    }


                    var yurticialanlar = alanlar.Where(x => x.mensei == "Yurtiçi").ToList();
                    var yurtDisialanlar = alanlar.Where(x => x.mensei == "Yurtdışı").ToList();


                    Microsoft.Office.Interop.Excel.Range baslikrange2 = excelsheet.Application.get_Range("A1", "G1");
                    baslikrange2.Font.Bold = true;
                    baslikrange2.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(220, 230, 241));
                    baslikrange2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    baslikrange2.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    baslikrange2.RowHeight = 40;
                    baslikrange2.Application.ActiveWindow.SplitRow = 1;
                    baslikrange2.Application.ActiveWindow.FreezePanes = true;
                    baslikrange2.Borders.Value = true;
                    cellarray[0, 0] = "Dağıtıcı Adı";
                    cellarray[0, 1] = "Ürün";
                    cellarray[0, 2] = "Kullanıcı Sayısı";
                    cellarray[0, 3] = "Birim Fiyat";
                    cellarray[0, 4] = "Para Birimi";
                    cellarray[0, 5] = "İndirim Tutarı";
                    // cellarray[0, 6] = "Toplam";

                    Microsoft.Office.Interop.Excel.Range baslikToplam = excelsheet.Application.get_Range("G1");
                    baslikToplam.Value = "Toplam";
                    currrow = 1;
                    var firstrow = 0;
                    for (int i = 0; i < yurticialanlar.Count; i++)
                    {

                        cellarray[currrow, 0] = RaporKurum;
                        cellarray[currrow, 1] = yurticialanlar[i].bilgikodu;
                        cellarray[currrow, 2] = yurticialanlar[i].cihaztoplam;
                        cellarray[currrow, 3] = yurticialanlar[i].bilgibedel;
                        cellarray[currrow, 4] = "TL";
                        cellarray[currrow, 5] = 0.00;

                        var krmd1Say = 0;
                        double krmd1Fiyat = 0;

                        if (yurticialanlar[i].bilgikodu.StartsWith("Karma"))
                        {
                            if (yurticialanlar[i].bilgikodu.Contains("1K")) { krmd1Say = 1000; krmd1Fiyat = 2.8; }
                            else if (yurticialanlar[i].bilgikodu.Contains("5K")) { krmd1Say = 5000; krmd1Fiyat = 2.5; }
                            else if (yurticialanlar[i].bilgikodu.Contains("10K")) { krmd1Say = 10000; krmd1Fiyat = 2.2; }
                            else if (yurticialanlar[i].bilgikodu.Contains("20K")) { krmd1Say = 20000; krmd1Fiyat = 1.9; }
                            else if (yurticialanlar[i].bilgikodu.Contains("50K")) { krmd1Say = 50000; krmd1Fiyat = 1.65; }
                            else if (yurticialanlar[i].bilgikodu.Contains("100K")) { krmd1Say = 100000; krmd1Fiyat = 1.45; }

                        }

                        Microsoft.Office.Interop.Excel.Range fomula = excelsheet.Application.get_Range("G" + (currrow + 1));


                        var formulx = "";
                        if (yurticialanlar[i].bilgikodu.StartsWith("Karma") && yurticialanlar[i].bilgikodu.Contains("Sınırsız") == false)
                            formulx = "=EĞER(C" + (currrow + 1) + "<1;0;EĞER(C" + (currrow + 1) + "<" + krmd1Say + ";D" + (currrow + 1) + ";C" + (currrow + 1) + "*" + krmd1Fiyat + "))";
                        else if (yurticialanlar[i].bilgikodu.StartsWith("Karma") && yurticialanlar[i].bilgikodu.Contains("Sınırsız"))
                            formulx = "=EĞER(C" + (currrow + 1) + "<1;0;D" + (currrow + 1) + ")";
                        else
                            formulx = "=(C" + (currrow + 1) + "*D" + (currrow + 1) + ")-(C" + (currrow + 1) + "*F" + (currrow + 1) + ")";

                        fomula.FormulaLocal = formulx;


                        currrow++;
                        if (i == 0) firstrow = currrow;
                        ExcelDurumYaz("ÖZET Sayfa Aktarılıyor (Yurt İçi) -->  " + (i).ToString() + " / " + yurticialanlar.Count.ToString());
                    }
                    //cellarray[currrow - 1, 7] = "TL Toplam";
                    //cellarray[currrow - 1, 8] = "=TOPLA(G" + (firstrow) + ":G" + (currrow) + ")";
                    excelsheet.Application.get_Range("H" + firstrow, "I" + currrow).Font.Bold = true;

                    excelsheet.Application.get_Range("H" + currrow).Value = "TL Toplam";
                    excelsheet.Application.get_Range("I" + currrow).FormulaLocal = "=TOPLA(G2:G" + currrow + ")";
                    excelsheet.Application.get_Range("I" + currrow).NumberFormat = "#,###,###.00 TL";

                    var ortaRow = currrow + 1;

                    if (yurtDisialanlar.Count > 0)
                    {
                        for (int i = 0; i < yurtDisialanlar.Count; i++)
                        {
                            cellarray[currrow, 0] = RaporKurum;
                            cellarray[currrow, 1] = yurtDisialanlar[i].bilgikodu;
                            cellarray[currrow, 2] = yurtDisialanlar[i].cihaztoplam;
                            cellarray[currrow, 3] = yurtDisialanlar[i].bilgibedel;
                            cellarray[currrow, 4] = "$";
                            cellarray[currrow, 5] = 0.00;

                            Microsoft.Office.Interop.Excel.Range fomula = excelsheet.Application.get_Range("G" + (currrow + 1));

                            fomula.FormulaLocal = "=(C" + (currrow + 1) + "*D" + (currrow + 1) + ")-(C" + (currrow + 1) + "*F" + (currrow + 1) + ")";

                            currrow++;
                            if (i == 0) firstrow = currrow;


                            ExcelDurumYaz("ÖZET Sayfa Aktarılıyor (Yurt Dışı) -->  " + (i).ToString() + " / " + yurtDisialanlar.Count.ToString());
                        }

                        //cellarray[currrow - 1, 7] = "USD Toplam";
                        //cellarray[currrow - 1, 8] = "=TOPLA(G" + (firstrow) + ":G" + (currrow) + ")";
                        excelsheet.Application.get_Range("H" + firstrow, "I" + currrow).Font.Bold = true;

                        excelsheet.Application.get_Range("H" + currrow).Value = "USD Toplam";
                        excelsheet.Application.get_Range("I" + currrow).FormulaLocal = "=TOPLA(G" + ortaRow + ":G" + currrow + ")";


                    }
                    excelsheet.Application.get_Range("A" + firstrow, "G" + currrow).Font.Bold = true;
                    Microsoft.Office.Interop.Excel.Range bodyrange = excelsheet.Application.get_Range("A2", "G" + currrow);
                    bodyrange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(228, 238, 243));
                    bodyrange.Font.Name = "Calibri";
                    bodyrange.Font.Size = 10;
                    bodyrange.Borders.Value = true;

                    ExcellApp.ScreenUpdating = false;
                    if (ExcellApp.Visible)
                    {
                        Microsoft.Office.Interop.Excel.Range range = excelsheet.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                        range.Value = cellarray;
                        range.Columns.AutoFit();
                    }
                    ExcellApp.ScreenUpdating = true;


                    ExcelDurumYaz("ÖZET Sayfa Aktarımı Bitti");
                    #endregion


                });
                T.Start();
            }

            public static void BorsaListeYeni()

            {
                try
                {
                    ExcelDurumFormAc();
                    Thread T = new Thread((obj) =>
                    {

                        List<string> duzeltilenler = new List<string>();
                        var filename = System.Windows.Forms.Application.StartupPath + "\\" + "BorsaListeYeni" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".csv";
                        SafeDeleteFile(filename);
                        var excelworkbook = GetCSVWorkbook(filename);

                        if (excelworkbook != null)

                        {
                            excelworkbook.Save();
                        }

                        excelworkbook = OpenExcelWorkbook(filename);
                        if (excelworkbook == null) return;
                        crmDFNDataContext crm = new crmDFNDataContext();

                        ExcelDurumYaz("BorsaListeYeni Aktarım Başladı");

                        #region DETAY

                        ExcelDurumYaz("BIST_Rapor Sayfa Aktarımı Başladı");

                        var excelsheetDetay = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.Add();

                        excelsheetDetay.Name = "BIST Rapor";

                        var Sorgu = crm.Users
                            .Where(x => x.LisansDurum != null &&
                                        x.LisansDurum.YayinDurumu == true &&
                                        (x.StatusId == 1 || x.StatusId == 5))
                            .ToList();

                        if (Sorgu == null || Sorgu.Count == 0)
                        {
                            ExcelDurumYaz("Uyarı: Hiç veri bulunamadı!");
                            return;
                        }

                        excelsheetDetay.Activate();

                        musteridurum.durumdictionary.Clear();

                        musteridurum.ydurumdictionary.Clear();

                        var count = Sorgu.Count();

                        var say = 0;

                        foreach (var item in Sorgu)

                        {

                            try
                            {
                                if (item.LisansDurum == null) continue;
                                musteridurum m = new musteridurum();
                                #region LISANLARIHESAPLA

                               
                                if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == false)
                                {
                                    m.PD1P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayLPStart.Value.ToShortDateString();
                                }

                                else if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == true && item.LisansDurum.Pd2P == false)
                                {
                                    m.PD2 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayL2Start.Value.ToShortDateString();

                                }

                                else if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == true && item.LisansDurum.Pd2P == true)

                                {
                                    m.PD2P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.Pd2PStart.Value.ToShortDateString();

                                }

                                if (item.LisansDurum.PayX == true)
                                {
                                    m.END = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayXStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.PITE == true)
                                {
                                    m.PITE = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayPiteStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.PayGS == true && item.LisansDurum.PITE == false)
                                {
                                    m.PIT = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayGSStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == false && item.LisansDurum.Vd2P == false)

                                {
                                    m.VL1P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.ViopLPStart.Value.ToShortDateString();
                                }

                                else if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == true && item.LisansDurum.Vd2P == false)

                                {
                                    m.VL2 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.ViopL2Start.Value.ToShortDateString();
                                }

                                else if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == true && item.LisansDurum.Vd2P == true)

                                {
                                    m.VL2P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.Vd2PStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.ViopGS == true)
                                {
                                    m.VIT = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.ViopGSStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.COMEX == true)
                                {
                                    m.KRMD1 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.KRMD1Start.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.MKK == true)
                                {
                                    m.MKK = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.MKKStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.GKKUL == true)
                                {
                                    m.GKKUL = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.GKKULStart.Value.ToShortDateString();
                                }

                            
                                if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == false && item.LisansDurum.TahvilL2 == false)

                                {
                                    m.BD1 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.TahvilL1Start.Value.ToShortDateString();
                                }

                                else if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == true && item.LisansDurum.TahvilL2 == false)
                                {
                                    m.BD1P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.TahvilLPStart.Value.ToShortDateString();
                                }

                                else if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == true && item.LisansDurum.TahvilL2 == true)

                                {
                                    m.BD2 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.TahvilL2Start.Value.ToShortDateString();
                                }
                                #endregion

                               // var musteri = crm.Musterilers.FirstOrDefault(x => x.Tckno == item.PmtsNo);
                                var musteri = crm.Musterilers.FirstOrDefault(x => x.MusteriNo == item.PmtsNo);
                                m.musteriadi = (musteri != null) ? musteri.MusteriAdi : m.musteriadi = item.Name + " " + item.Surname;
                                if (item.Iletisim != null)
                                {
                                    m.adres = (item.Iletisim.acikadres == null) ? "" : item.Iletisim.acikadres.Replace("\n", " ");
                                    m.sehir = (item.Iletisim.Il == null) ? "" : item.Iletisim.Il.IlAdi;
                                    if (item.Iletisim.Ulke == null) m.ulke = "";
                                    else if (item.Iletisim.Ulke.UlkeAdi == "Türkiye") m.ulke = "TR";
                                    else m.ulke = item.Iletisim.Ulke.UlkeAdi;

                                    // 10158 info adres ve şehri müşteriler formundaki adres bilgisini yaz o da yoksa default istanbul olsun
                                    if (musteri != null && musteri.Iletisim != null)
                                    {
                                        if (musteri.Iletisim.Il == null && m.sehir == "")

                                        {
                                            m.sehir = "İstanbul";
                                           // duzeltilenler.Add("ülke değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                            duzeltilenler.Add("ülke değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                        }
                                        if (musteri.Iletisim.Il != null && m.sehir == "")
                                        {
                                            m.sehir = musteri.Iletisim.Il.IlAdi;
                                            duzeltilenler.Add("şehir değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                           // duzeltilenler.Add("şehir değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                        }
                                        if (musteri.Iletisim.Ulke == null && m.ulke == "")
                                        {
                                            m.ulke = "TR";
                                             duzeltilenler.Add("ülke değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                           // duzeltilenler.Add("ülke değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                        }
                                        if (musteri.Iletisim.Ulke != null && m.ulke == "")
                                        {
                                            m.ulke = musteri.Iletisim.Ulke.UlkeAdi;
                                            duzeltilenler.Add("ülke değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                           // duzeltilenler.Add("ülke değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                        }
                                        if (musteri.Iletisim.acikadres == null && m.adres == "")
                                        {
                                            m.adres = "İstanbul";
                                             duzeltilenler.Add("adres değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                           // duzeltilenler.Add("adres değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                        }
                                        if (musteri.Iletisim.acikadres != null && m.adres == "")
                                        {
                                            m.adres = musteri.Iletisim.acikadres.ToString();
                                              duzeltilenler.Add("adres değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                           // duzeltilenler.Add("adres değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                        }
                                        if (m.adres == "" && m.sehir == "")
                                        {
                                            m.adres = "İstanbul";
                                            m.sehir = "İstanbul";
                                             duzeltilenler.Add("adres ve sehir değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                          //  duzeltilenler.Add("adres ve sehir değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                        }
                                        else if (m.adres == "" && m.sehir != "")
                                        {
                                            m.adres = m.sehir;
                                             duzeltilenler.Add("adres değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                           // duzeltilenler.Add("adres değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                        }
                                    }
                                }
                                if (item.BaslangicTarihi != null)

                                    m.yetkilendirmetarihi = item.BaslangicTarihi.Value.ToShortDateString();

                                m.mensei = (item.MusteriMenseiID == 2) ? "YurtDışı" : m.mensei = "Yurtİçi";

                                m.Bedelsizaciklama = (item.StatusId == 1) ? "" : m.Bedelsizaciklama = "Bedelsiz";

                                if (item.MusteriMenseiID == 2)
                                     musteridurum.ekleY(item.UserName, m);
                                 //   musteridurum.ekleY(item.tckno, m);
                                else
                                     musteridurum.ekle(item.UserName, m);
                                   // musteridurum.ekle(item.tckno, m);
                                say++;

                                ExcelDurumYaz("BIST Rapor Sayfa Aktarılıyor -->  " + (say).ToString() + " / " + count.ToString());
                            }
                            catch (Exception ex)
                            {
                                MyTools.logyaz("BorsaListeYeni thread hata: " + ex);

                                ExcelDurumYaz("HATA: " + ex.Message);
                            }

                        }
                        var alanlarlistesi = new List<alanlar>();
                        foreach (var mdurum in musteridurum.durumdictionary)
                        {
                            if (mdurum.Key == "10993")
                            {

                            }

                            #region PD1

                            if (mdurum.Value.PD1 > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpd1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpd1 * mdurum.Value.PD1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;
                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PD1P

                            if (mdurum.Value.PD1P > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD1P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD1P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpd1p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpd1p * mdurum.Value.PD1P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PD2

                            if (mdurum.Value.PD2 > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD2";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD2.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpd2.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpd2 * mdurum.Value.PD2).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PD2P

                            if (mdurum.Value.PD2P > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD2P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD2P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpd2p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpd2p * mdurum.Value.PD2P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region END

                            if (mdurum.Value.END > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "END";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.END.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fend.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fend * mdurum.Value.END).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PIT

                            if (mdurum.Value.PIT > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PIT";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PIT.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpit.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpit * mdurum.Value.PIT).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PITE

                            if (mdurum.Value.PITE > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PITE";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PITE.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpite.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpite * mdurum.Value.PITE).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL1

                            if (mdurum.Value.VL1 > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvl1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvl1 * mdurum.Value.VL1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL1P

                            if (mdurum.Value.VL1P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD1P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL1P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvl1p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvl1p * mdurum.Value.VL1P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL2

                            if (mdurum.Value.VL2 > 0)

                            {



                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD2";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL2.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvl2.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvl2 * mdurum.Value.VL2).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL2P

                            if (mdurum.Value.VL2P > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD2P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL2P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvl2p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvl2p * mdurum.Value.VL2P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VIT

                            if (mdurum.Value.VIT > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VIT";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VIT.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvit.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvit * mdurum.Value.VIT).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region KRMD1

                            if (mdurum.Value.KRMD1 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "KRMD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.KRMD1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = "0";

                                    alan.toplambedel = "0";

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }



                            #endregion

                            #region BD1

                            if (mdurum.Value.BD1 > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "BD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.BD1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fbd1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fbd1 * mdurum.Value.BD1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region BD1P

                            if (mdurum.Value.BD1P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "BD1P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.BD1P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fbd1p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fbd1p * mdurum.Value.BD1P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region BD2

                            if (mdurum.Value.BD2 > 0)

                            {

                                if (mdurum.Key == "10993")

                                {

                                }

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "BD2";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.BD2.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fbd2.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fbd2 * mdurum.Value.BD2).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region MKK

                            if (mdurum.Value.MKK > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "MKK";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.MKK.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = "0";

                                    alan.toplambedel = "0";

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }



                            #endregion

                            //#region TARAMA

                            //if (mdurum.Value.TARAMA > 0)

                            //{

                            //    var alan = new alanlar();

                            //    alan.aboneno = mdurum.Key;

                            //    alan.aboneadi = mdurum.Value.musteriadi;

                            //    alan.adres = mdurum.Value.adres;

                            //    alan.sehir = mdurum.Value.sehir;

                            //    alan.ulke = mdurum.Value.ulke;

                            //    alan.bilgikodu = "TARAMA";

                            //    alan.mensei = mdurum.Value.mensei;

                            //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                            //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                            //    alan.cihaztoplam = mdurum.Value.TARAMA.ToString();

                            //    if (mdurum.Value.Bedelsizaciklama == "")

                            //    {

                            //        alan.bilgibedel = "0";

                            //        alan.toplambedel = "0";

                            //    }

                            //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                            //    alanlarlistesi.Add(alan);

                            //}



                            //#endregion

                            #region GKKUL

                            if (mdurum.Value.GKKUL > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "GKKUL";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.GKKUL.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = "0";

                                    alan.toplambedel = "0";

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);

                            }

                            #endregion


                        }


                        #region yurdisilar

                        foreach (var mdurum in musteridurum.ydurumdictionary)
                        {
                            #region PD1

                            if (mdurum.Value.PD1 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypd1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypd1 * mdurum.Value.PD1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;
                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PD1P

                            if (mdurum.Value.PD1P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD1P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD1P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypd1p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypd1p * mdurum.Value.PD1P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;
                                alanlarlistesi.Add(alan);

                            }
                            #endregion

                            #region PD2

                            if (mdurum.Value.PD2 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD2";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD2.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypd2.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypd2 * mdurum.Value.PD2).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PD2P

                            if (mdurum.Value.PD2P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD2P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD2P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypd2p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypd2p * mdurum.Value.PD2P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region END

                            if (mdurum.Value.END > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "END";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.END.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yend.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yend * mdurum.Value.END).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PIT

                            if (mdurum.Value.PIT > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PIT";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PIT.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypit.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypit * mdurum.Value.PIT).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;
                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PITE

                            if (mdurum.Value.PITE > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PITE";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PITE.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypite.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypite * mdurum.Value.PITE).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;
                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL1

                            if (mdurum.Value.VL1 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvl1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvl1 * mdurum.Value.VL1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL1p

                            if (mdurum.Value.VL1P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD1P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL1P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvl1p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvl1p * mdurum.Value.VL1P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL2

                            if (mdurum.Value.VL2 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD2";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL2.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvl2.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvl2 * mdurum.Value.VL2).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL2P

                            if (mdurum.Value.VL2P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD2P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL2P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvl2p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvl2p * mdurum.Value.VL2P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VIT

                            if (mdurum.Value.VIT > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VIT";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VIT.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvit.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvit * mdurum.Value.VIT).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region KRMD1

                            if (mdurum.Value.KRMD1 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "KRMD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.KRMD1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ykrmd1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ykrmd1 * mdurum.Value.KRMD1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region BD1



                            if (mdurum.Value.BD1 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "BD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.BD1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ybd1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ybd1 * mdurum.Value.BD1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region BD1P

                            if (mdurum.Value.BD1P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "BD1P";

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.mensei = mdurum.Value.mensei;

                                alan.cihaztoplam = mdurum.Value.BD1P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ybd1p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ybd1p * mdurum.Value.BD1P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region BD2

                            if (mdurum.Value.BD2 > 0)

                            {

                                if (mdurum.Key == "10993")

                                {



                                }

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "BD2";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.BD2.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ybd2.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ybd2 * mdurum.Value.BD2).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;
                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region MKK

                            if (mdurum.Value.MKK > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "MKK";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.MKK.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ymkk.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ymkk * mdurum.Value.MKK).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            //#region TARAMA

                            //if (mdurum.Value.TARAMA > 0)

                            //{

                            //    var alan = new alanlar();

                            //    alan.aboneno = mdurum.Key;

                            //    alan.aboneadi = mdurum.Value.musteriadi;

                            //    alan.adres = mdurum.Value.adres;

                            //    alan.sehir = mdurum.Value.sehir;

                            //    alan.ulke = mdurum.Value.ulke;

                            //    alan.bilgikodu = "TARAMA";

                            //    alan.mensei = mdurum.Value.mensei;

                            //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                            //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                            //    alan.cihaztoplam = mdurum.Value.TARAMA.ToString();

                            //    if (mdurum.Value.Bedelsizaciklama == "")

                            //    {

                            //        alan.bilgibedel = MyTools.lisansfiyatlari.Ytarama.ToString("0.00");

                            //        alan.toplambedel = (MyTools.lisansfiyatlari.Ytarama * mdurum.Value.TARAMA).ToString("0.00");

                            //    }

                            //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                            //    alanlarlistesi.Add(alan);

                            //}

                            //#endregion

                            #region GKKUL

                            if (mdurum.Value.GKKUL > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "GKKUL";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.GKKUL.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ygkkul.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ygkkul * mdurum.Value.GKKUL).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion
                        }

                        #endregion

                        var cellarray = new object[alanlarlistesi.Count + 10, 14];

                        for (int i = 0; i < cellarray.GetLength(0); i++)

                        {

                            for (int j = 0; j < cellarray.GetLength(1); j++)

                                cellarray[i, j] = "";

                        }
                        var currrow = 0;
                        var date = DateTime.Now;
                        cellarray[currrow, 0] = "Dağıtıcı Adı:";

                        cellarray[currrow, 1] = "İdeal Data Finansal Teknolojiler A.Ş.";

                        currrow++;

                        cellarray[currrow, 0] = "Borsa";

                        cellarray[currrow, 1] = "Borsa İstanbul A.Ş.";

                        currrow++;

                        cellarray[currrow, 0] = "Raporlanan Ay";

                        cellarray[currrow, 1] = date.ToString("MMM.").Trim() + date.ToString("yy");

                        currrow++;

                        cellarray[currrow, 0] = "Rapor Tipi";

                        cellarray[currrow, 1] = "Detay";

                        currrow++;

                        cellarray[currrow, 0] = "";

                        currrow++;

                        cellarray[currrow, 0] = "Abone Kodu";

                        cellarray[currrow, 1] = "Abone Adı Soyadı";

                        cellarray[currrow, 2] = "Adres";

                        cellarray[currrow, 3] = "Şehir";

                        cellarray[currrow, 4] = "Ülke";

                        cellarray[currrow, 5] = "Bilgi Raporlama Kodu";

                        cellarray[currrow, 6] = "Erişim Tipi";

                        cellarray[currrow, 7] = "Toplam Kullanıcı/Cihaz Adedi";

                        cellarray[currrow, 8] = "Yetkilendirme Tarihi";

                        cellarray[currrow, 9] = "Lisans Yenileme Tarihi";

                        cellarray[currrow, 10] = "Bedelsiz Açıklama";

                        currrow++;

                        SafeFreezePanes(excelsheetDetay, 6);

                        for (int i = 0; i < alanlarlistesi.Count; i++)

                        {

                            if (alanlarlistesi[i].aboneno == "10993")

                            {

                            }

                            cellarray[currrow, 0] = alanlarlistesi[i].aboneno;

                            cellarray[currrow, 1] = alanlarlistesi[i].aboneadi;

                            cellarray[currrow, 2] = alanlarlistesi[i].adres;

                            cellarray[currrow, 3] = alanlarlistesi[i].sehir;

                            cellarray[currrow, 4] = alanlarlistesi[i].ulke;

                            cellarray[currrow, 5] = alanlarlistesi[i].bilgikodu; ;

                            cellarray[currrow, 6] = alanlarlistesi[i].erisimtipi;

                            cellarray[currrow, 7] = alanlarlistesi[i].cihaztoplam;

                            cellarray[currrow, 8] = alanlarlistesi[i].yetkikendirmetar;

                            cellarray[currrow, 9] = alanlarlistesi[i].lisansYenilemeTarihi;

                            cellarray[currrow, 10] = alanlarlistesi[i].bedelsizaciklama;

                            currrow++;

                        }

                        Microsoft.Office.Interop.Excel.Range range2 = excelsheetDetay.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));

                        range2.Value = cellarray;

                        range2.Columns.AutoFit();



                        ExcelDurumYaz("DETAY Sayfa Aktarımı Bitti");

                        string strDuzeltilenler = string.Join("-", duzeltilenler);

                        var dosya_yolu = System.Windows.Forms.Application.StartupPath + "\\" + "BorsaListeYeniDüzeltilenler" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".txt";

                        if (File.Exists(dosya_yolu)) File.Delete(dosya_yolu);

                        FileStream fs = new FileStream(dosya_yolu, FileMode.OpenOrCreate, FileAccess.Write);

                        StreamWriter sw = new StreamWriter(fs);

                        foreach (var duz in duzeltilenler)

                        {

                            sw.WriteLine(duz);

                        }

                        sw.Flush();

                        sw.Close();

                        fs.Close();

                        #endregion

                        ExcelDurumYaz("CSV dosyası kaydediliyor...");
                        //try
                        //{
                        //    excelworkbook.SaveAs(filename,
                        //        Microsoft.Office.Interop.Excel.XlFileFormat.xlCSV,
                        //        Type.Missing, Type.Missing, false, false,
                        //        Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                        //        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                        //    ExcelDurumYaz("CSV dosyası kaydedildi: " + filename);

                        //    excelworkbook.Close(false);
                        //}
                        //catch (Exception excelEx)
                        //{
                        //    MyTools.logyaz($"CSV kaydetme hatası: {excelEx.Message}");
                        //    ExcelDurumYaz($"HATA: CSV kaydedilemedi - {excelEx.Message}");
                        //}

                        //try
                        //{
                        //    if (ExcelRutin.ExcellApp != null)
                        //    {
                        //        ExcelRutin.ExcellApp.Quit();
                        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelRutin.ExcellApp);
                        //        ExcelRutin.ExcellApp = null;
                        //    }
                        //    GC.Collect();
                        //    GC.WaitForPendingFinalizers();
                        //}
                        //catch (Exception cleanupEx)
                        //{
                        //    MyTools.logyaz($"Excel cleanup hatası: {cleanupEx.Message}");
                        //}

                        //// Durum formunu kapat
                        //try
                        //{
                        //    System.Windows.Forms.Application.OpenForms
                        //        .Cast<System.Windows.Forms.Form>()
                        //        .Where(f => f.Name == "formExcelDurum" || f.Text.Contains("AKTARIM DURUM"))
                        //        .ToList()
                        //        .ForEach(f =>
                        //        {
                        //            if (f.InvokeRequired)
                        //                f.BeginInvoke(new Action(() => f.Close()));
                        //            else
                        //                f.Close();
                        //        });
                        //}
                        //catch (Exception formEx)
                        //{
                        //    MyTools.logyaz($"Form kapatma hatası: {formEx.Message}");
                        //}
                        WriteToCSV(filename, cellarray);
                        ExcelDurumYaz("İşlem tamamlandı!");

                    });

                    T.SetApartmentState(ApartmentState.STA);
                    T.IsBackground = true;
                    T.Start();
                }
                catch (Exception ex)
                {
                    MyTools.logyaz($"BorsaListeYeni de hata: {ex.Message.ToString()}");
                    throw;
                }

            }
            private static void WriteToCSV(string filename, object[,] cellarray)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
                    {
                        for (int i = 0; i < cellarray.GetLength(0); i++)
                        {
                            List<string> row = new List<string>();
                            for (int j = 0; j < cellarray.GetLength(1); j++)
                            {
                                string value = cellarray[i, j]?.ToString() ?? "";
                                // CSV için özel karakterleri escape et
                                if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
                                {
                                    value = "\"" + value.Replace("\"", "\"\"") + "\"";
                                }
                                row.Add(value);
                            }
                            sw.WriteLine(string.Join(",", row));
                        }
                    }
                    ExcelDurumYaz("CSV dosyası kaydedildi: " + filename);
                }
                catch (Exception ex)
                {
                    MyTools.logyaz($"CSV yazma hatası: {ex.Message}");
                    ExcelDurumYaz($"HATA: CSV yazılamadı - {ex.Message}");
                }
            }
            public static async Task AutoBorsaTamListe()
            {
                try
                {
                    ExcelDurumFormAc();
                    await RunStaAsync(() =>
                    {
                        var filename = System.Windows.Forms.Application.StartupPath + "\\" + "BorsaTamListe" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".XLSX";

                        SafeDeleteFile(filename);
                        var excelworkbook = GetExcelWorkbook(filename);
                        if (excelworkbook != null)
                        {
                            // excelworkbook.Save();
                            excelworkbook.Close(false);
                        }
                        excelworkbook = OpenExcelWorkbook(filename);
                        if (excelworkbook == null) return;
                        crmDFNDataContext crm = new crmDFNDataContext();

                        lisansFiyatlariniOku(crm);

                        ExcelDurumYaz("BorsaTamListe Aktarım Başladı");
                        #region RaporlamaKodlari

                        var excelsheetRk = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.get_Item(1);
                        excelsheetRk.Name = "Raporlama Kodları";

                        var RPList = new List<RaporlamaKodu>();
                        RPList.Add(new RaporlamaKodu { EskiKod = "EPG", YeniKod = "Eşanlı Portföy Gösterimi" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "KRMD1", YeniKod = "Karma Düzey 1" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "MKK", YeniKod = "MKK" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "PD1P", YeniKod = "PAY Düzey 1P" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "PD2", YeniKod = "PAY Düzey 2" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "PD2P", YeniKod = "PAY Düzey 2P" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "VD1P", YeniKod = "VİOP Düzey 1P" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "VD2", YeniKod = "VİOP Düzey 2" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "VD2P", YeniKod = "VİOP Düzey 2P" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "BD1P", YeniKod = "BAP Düzey 1P" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "BD2", YeniKod = "BAP Düzey 2" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "KD1P", YeniKod = "KMTP Düzey 1P" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "KD2", YeniKod = "KMTP Düzey 2" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "END", YeniKod = "Borsa İstanbul Endeksleri" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "PITE", YeniKod = "Pay İşlem Tarafı Eşanlı" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "PIT", YeniKod = "Pay İşlem Tarafı Günsonu" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "PVA", YeniKod = "Pay Veri Analitikleri" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "VIT", YeniKod = "VİOP İşlem Tarafı Günsonu" });
                        // RPList.Add(new RaporlamaKodu { EskiKod = "TARAMA", YeniKod = "TARAMA" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "GKKUL", YeniKod = "Kullanıcı Gösterimsiz Kullanım Bedeli " });
                        RPList.Add(new RaporlamaKodu { EskiKod = "GKULKYD", YeniKod = "Gösterimsiz Kul. BIST-KYD Endeksleri" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "GKULEND", YeniKod = "Gösterimsiz Kul. Borsa İstanbul Endeksleri" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "GKULD1P", YeniKod = "Gösterimsiz Kul. Düzey 1P" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "GKULD2", YeniKod = "Gösterimsiz Kul. Düzey 2/2P" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "GKULPITE", YeniKod = "Gösterimsiz Kul. Pay İşlem Tarafı Eşanlı" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "GKULPVA", YeniKod = "Gösterimsiz Kul. Üye Pay Veri Analitikleri" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "GUYEKYD", YeniKod = "Gösterimsiz Kul.Üye BIST-KYD Endeksleri" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "GUYEEND", YeniKod = "Gösterimsiz Kul. Üye Borsa İstanbul Endeksleri" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "GUYED1P", YeniKod = "Gösterimsiz Kul.Üye Düzey 1P" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "GUYED2", YeniKod = "Gösterimsiz Kul. Üye Düzey 2/2P" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "GUYEPITE", YeniKod = "Gösterimsiz Kul. Üye Pay İşlem Tarafı Eşanlı" });
                        RPList.Add(new RaporlamaKodu { EskiKod = "GUYEPVA", YeniKod = "Gösterimsiz Kul. Üye Pay Veri Analitikleri" });

                        // var cellarray3 = new object[57, 2];
                        var cellarray3 = new object[RPList.Count + 1, 2];
                        cellarray3[0, 0] = "Kod";
                        cellarray3[0, 1] = "Raporlama Kodu";
                        //cellarray3[0, 2] = "Yurtiçi/Yurtdışı";


                        for (int i = 0; i < RPList.Count; i++)
                        {
                            cellarray3[i + 1, 0] = RPList[i].EskiKod;
                            cellarray3[i + 1, 1] = RPList[i].YeniKod;
                            //cellarray3[i, 2] = RPList[i - 1].Mensei;
                        }



                        Microsoft.Office.Interop.Excel.Range rangerp = excelsheetRk.Cells.get_Resize(cellarray3.GetLength(0), cellarray3.GetLength(1));
                        rangerp.Value = cellarray3;
                        rangerp.Columns.AutoFit();


                        //RPList.Add(new RaporlamaKodu { EskiKod = "EPG", YeniKod = "Eşanlı Portföy Gösterimi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "KRMD1", YeniKod = "Karma Düzey 1"});
                        //RPList.Add(new RaporlamaKodu { EskiKod = "BD1P", YeniKod = "BAP Düzey 1P" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "BD2", YeniKod = "BAP Düzey 2" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "END", YeniKod = "Borsa İstanbul Endeksleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GKULKYD", YeniKod = "Gösterimsiz Kul. BIST-KYD Endeksleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GKULEND", YeniKod = "Gösterimsiz Kul. Borsa İstanbul Endeksleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GKULD1P", YeniKod = "Gösterimsiz Kul. Düzey 1/1P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GKULD2", YeniKod = "Gösterimsiz Kul. Düzey 2/2P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GKULPITE", YeniKod = "Gösterimsiz Kul. Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GKULPVA", YeniKod = "Gösterimsiz Kul. Üye Pay Veri Analitikleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEKYD", YeniKod = "Gösterimsiz Kul. Üye BIST-KYD Endeksleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEEND", YeniKod = "Gösterimsiz Kul. Üye Borsa İstanbul Endeksleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GUYED1P", YeniKod = "Gösterimsiz Kul. Üye Düzey 1P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GUYED2", YeniKod = "Gösterimsiz Kul. Üye Düzey 2/2P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEPITE", YeniKod = "Gösterimsiz Kul. Üye Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEPVA", YeniKod = "Gösterimsiz Kul. Üye Pay Veri Analitikleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "KD1P", YeniKod = "KMTP Düzey 1P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "KD2", YeniKod = "KMTP Düzey 2 - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "PD1P", YeniKod = "PAY Düzey 1P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "PD2", YeniKod = "PAY Düzey 2 - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "PD2P", YeniKod = "PAY Düzey 2P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "PITE", YeniKod = "Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "PIT", YeniKod = "Pay İşlem Tarafı Günsonu - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "PVA", YeniKod = "Pay Veri Analitikleri - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "VD1P", YeniKod = "VİOP Düzey 1P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "VD2", YeniKod = "VİOP Düzey 2 - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "VD2P", YeniKod = "VİOP Düzey 2P - Değişken - Yurtiçi", Mensei = "Yurtiçi" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "VIT", YeniKod = "VİOP İşlem Tarafı Günsonu - Değişken - Yurtiçi", Mensei = "Yurtiçi" });

                        //RPList.Add(new RaporlamaKodu { EskiKod = "KRMD1", YeniKod = "Karma Düzey 1 - 1K Kullanıcı Paketi - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "BD1P", YeniKod = "BAP Düzey 1P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "BD2", YeniKod = "BAP Düzey 2 - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "ANALİZPRO", YeniKod = "Analiz Pro - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "END", YeniKod = "Borsa İstanbul Endeksleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GKULKYD", YeniKod = "Gösterimsiz Kul. BIST-KYD Endeksleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GKULEND", YeniKod = "Gösterimsiz Kul. Borsa İstanbul Endeksleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GKULD1P", YeniKod = "Gösterimsiz Kul. Düzey 1/1P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GKULD2", YeniKod = "Gösterimsiz Kul. Düzey 2/2P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GKULPITE", YeniKod = "Gösterimsiz Kul. Pay İşlem Tarafı Eşanlı - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GKULPVA", YeniKod = "Gösterimsiz Kul. Üye Pay Veri Analitikleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEKYD", YeniKod = "Gösterimsiz Kul. Üye BIST-KYD Endeksleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEEND", YeniKod = "Gösterimsiz Kul. Üye Borsa İstanbul Endeksleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GUYED1P", YeniKod = "Gösterimsiz Kul. Üye Düzey 1P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GUYED2", YeniKod = "Gösterimsiz Kul. Üye Düzey 2/2P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEPITE", YeniKod = "Gösterimsiz Kul. Üye Pay İşlem Tarafı Eşanlı - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "GUYEPVA", YeniKod = "Gösterimsiz Kul. Üye Pay Veri Analitikleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "KD1P", YeniKod = "KMTP Düzey 1P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "KD2", YeniKod = "KMTP Düzey 2 - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "PD1P", YeniKod = "PAY Düzey 1P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "PD2", YeniKod = "PAY Düzey 2 - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "PD2P", YeniKod = "PAY Düzey 2P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "PITE", YeniKod = "Pay İşlem Tarafı Eşanlı - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "PIT", YeniKod = "Pay İşlem Tarafı Günsonu - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "PVA", YeniKod = "Pay Veri Analitikleri - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "VD1P", YeniKod = "VİOP Düzey 1P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "VD2", YeniKod = "VİOP Düzey 2 - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "VD2P", YeniKod = "VİOP Düzey 2P - Değişken - Yurtdışı", Mensei = "Yurtdışı" });
                        //RPList.Add(new RaporlamaKodu { EskiKod = "VIT", YeniKod = "VİOP İşlem Tarafı Günsonu - Değişken - Yurtdışı", Mensei = "Yurtdışı" });


                        //var cellarray3 = new object[60, 3];//57-60
                        //cellarray3[0, 0] = "Eski Kod";
                        //cellarray3[0, 1] = "Yeni Raporlama Kodu";
                        //cellarray3[0, 2] = "Yurtiçi/Yurtdışı";


                        //for (int i = 1; i < RPList.Count; i++)
                        //{
                        //    cellarray3[i, 0] = RPList[i].EskiKod;
                        //    cellarray3[i, 1] = RPList[i].YeniKod;
                        //    cellarray3[i, 2] = RPList[i].Mensei;
                        //}



                        //Microsoft.Office.Interop.Excel.Range rangerp = excelsheetRk.Cells.get_Resize(cellarray3.GetLength(0), cellarray3.GetLength(1));
                        //rangerp.Value = cellarray3;
                        //rangerp.Columns.AutoFit();

                        //ExcelDurumYaz("Raporlama Kodları Yazıldı");

                        #endregion

                        #region DETAY


                        ExcelDurumYaz("DETAY Sayfa Aktarımı Başladı");
                        var excelsheetDetay = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.Add();
                        excelsheetDetay.Name = "DETAY";

                        var Sorgu = crm.Users.Where(x => x.StatusId == 1);
                        excelsheetDetay.Activate();

                        musteridurum.durumdictionary.Clear();
                        musteridurum.ydurumdictionary.Clear();

                        var sorguDetay = crm.Users.Where(x => (x.LisansDurum.YayinDurumu == true && x.StatusId == 1) || (x.LisansDurum.YayinDurumu == true));
                        var count = sorguDetay.Count();
                        var say = 0;
                        foreach (var item in sorguDetay)
                        {


                            musteridurum m = new musteridurum();
                            #region LISANLARIHESAPLA
                            //if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == false && item.LisansDurum.PayL2 == false && item.LisansDurum.Pd2P == false)
                            //{

                            //    m.PD1 = 1;
                            //}
                            try
                            {
                                if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == false && item.LisansDurum.Pd2P == false)
                                {
                                    m.PD1P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayLPStart.Value.ToShortDateString();

                                }
                                else if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == true && item.LisansDurum.Pd2P == false)
                                {
                                    m.PD2 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayL2Start.Value.ToShortDateString();
                                }
                                else if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == true && item.LisansDurum.Pd2P == true)
                                {
                                    m.PD2P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.Pd2PStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.PayX == true)
                                {
                                    m.END = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayXStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.PITE == true)
                                {
                                    m.PITE = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayPiteStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.PayGS == true && item.LisansDurum.PITE == false)
                                {
                                    m.PIT = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayGSStart.Value.ToShortDateString();
                                }

                                //if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == false && item.LisansDurum.ViopL2 == false && item.LisansDurum.Vd2P == false)
                                //{

                                //    m.VL1 = 1;
                                //}
                                if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == false && item.LisansDurum.Vd2P == false)
                                {
                                    m.VL1P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.ViopLPStart.Value.ToShortDateString();
                                }
                                else if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == true && item.LisansDurum.Vd2P == false)
                                {
                                    m.VL2 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.ViopL2Start.Value.ToShortDateString();
                                }
                                else if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == true && item.LisansDurum.Vd2P == true)
                                {
                                    m.VL2P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.Vd2PStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.ViopGS == true)
                                {
                                    m.VIT = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.ViopGSStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.COMEX == true)
                                {
                                    m.KRMD1 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.KRMD1Start.Value.ToShortDateString();
                                }
                                if (item.LisansDurum.MKK == true)
                                {
                                    m.MKK = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.MKKStart.Value.ToShortDateString();
                                }
                                if (item.LisansDurum.GKKUL == true)
                                {
                                    m.GKKUL = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.GKKULStart.Value.ToShortDateString();
                                }
                                if (item.LisansDurum.TARAMA == true)
                                {
                                    m.TARAMA = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.TaramaStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == false && item.LisansDurum.TahvilL2 == false)
                                {
                                    m.BD1 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.TahvilL1Start.Value.ToShortDateString();
                                }
                                else if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == true && item.LisansDurum.TahvilL2 == false)
                                {
                                    m.BD1P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.TahvilLPStart.Value.ToShortDateString();
                                }
                                else if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == true && item.LisansDurum.TahvilL2 == true)
                                {

                                    m.BD2 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.TahvilL2Start.Value.ToShortDateString();
                                }
                            }
                            catch (Exception EX)
                            {
                                MyTools.logyaz($"Lisans hatası - User ID: {item.UserID}, TCKNO: {item.tckno}, Hata: {EX.Message}");
                                MessageBox.Show($"lisans hatası: {EX.Message}");
                                continue;
                            }

                            #endregion

                            //var musteri = crm.Musterilers.FirstOrDefault(x => x.MusteriNo == item.PmtsNo);
                            //m.musteriadi = (musteri != null) ? musteri.MusteriAdi : m.musteriadi = item.Name + " " + item.Surname;

                            // m.musteriadi = item.Name + " " + item.Surname;
                            m.musteriadi = !string.IsNullOrWhiteSpace(item.Name) || !string.IsNullOrWhiteSpace(item.Surname)
                               ? $"{item.Name ?? ""} {item.Surname ?? ""}".Trim()
                               : "İsimsiz Kullanıcı";

                            if (item.Iletisim != null)
                            {
                                m.adres = (item.Iletisim.acikadres == null) ? "" : item.Iletisim.acikadres.Replace("\n", " ");
                                m.sehir = (item.Iletisim.Il == null) ? "" : item.Iletisim.Il.IlAdi;
                                m.ulke = (item.Iletisim.Ulke == null) ? "" : item.Iletisim.Ulke.UlkeAdi;
                                if (string.IsNullOrEmpty(m.adres) && string.IsNullOrEmpty(m.sehir))
                                {
                                    m.adres = "İstanbul";
                                    m.sehir = "İstanbul";
                                }
                                else if (string.IsNullOrEmpty(m.adres) && !string.IsNullOrEmpty(m.sehir))
                                {
                                    m.adres = m.sehir;
                                }
                            }
                            else //ÖFK
                            {
                                m.adres = "İstanbul";
                                m.sehir = "İstanbul";
                                m.ulke = "Türkiye";
                            }

                            if (item.BaslangicTarihi != null)
                                m.yetkilendirmetarihi = item.BaslangicTarihi.Value.ToShortDateString();
                            m.mensei = (item.MusteriMenseiID == 2) ? "YurtDışı" : m.mensei = "Yurtİçi";
                            m.Bedelsizaciklama = (item.StatusId == 1) ? "" : m.Bedelsizaciklama = "Bedelsiz";


                            if (item.MusteriMenseiID == 2)
                                // musteridurum.ekleY(item.UserName, m);
                                musteridurum.ekleY(item.tckno, m);
                            else
                                //  musteridurum.ekle(item.UserName, m);
                                musteridurum.ekle(item.tckno, m);
                            say++;
                            ExcelDurumYaz("Detay Sayfa Aktarılıyor -->  " + (say).ToString() + " / " + count.ToString());
                        }



                        var alanlarlistesi = new List<alanlar>();
                        foreach (var mdurum in musteridurum.durumdictionary)
                        {

                            #region PD1
                            //if (mdurum.Value.PD1 > 0)
                            //{

                            //    var alan = new alanlar();
                            //    alan.aboneno = mdurum.Key;
                            //    alan.aboneadi = mdurum.Value.musteriadi;
                            //    alan.adres = mdurum.Value.adres;
                            //    alan.sehir = mdurum.Value.sehir;
                            //    alan.ulke = mdurum.Value.ulke;
                            //    alan.bilgikodu = "PD1";
                            //    //alan.bilgikodu = "PAY Düzey 1 - Değişken - Yurtiçi";
                            //    alan.mensei = mdurum.Value.mensei;
                            //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            //    alan.cihaztoplam = mdurum.Value.PD1.ToString();
                            //    if (mdurum.Value.Bedelsizaciklama == "")
                            //    {
                            //        alan.bilgibedel = MyTools.lisansfiyatlari.Fpd1.ToString("0.00");
                            //        alan.toplambedel = (MyTools.lisansfiyatlari.Fpd1 * mdurum.Value.PD1).ToString("0.00");
                            //    }
                            //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            //    alanlarlistesi.Add(alan);
                            //}
                            #endregion
                            #region PD1P
                            if (mdurum.Value.PD1P > 0)
                            {

                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                //alan.bilgikodu = "PAY Düzey 1P - Değişken - Yurtiçi";
                                alan.bilgikodu = "PD1P";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.PD1P.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpd1p.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpd1p * mdurum.Value.PD1P).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region PD2
                            if (mdurum.Value.PD2 > 0)
                            {

                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                //alan.bilgikodu = "PAY Düzey 2 - Değişken - Yurtiçi";
                                alan.bilgikodu = "PD2";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.PD2.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpd2.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpd2 * mdurum.Value.PD2).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region PD2P
                            if (mdurum.Value.PD2P > 0)
                            {

                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                //alan.bilgikodu = "PAY Düzey 2 - Değişken - Yurtiçi";
                                alan.bilgikodu = "PD2P";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.PD2P.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpd2p.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpd2p * mdurum.Value.PD2P).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region END
                            if (mdurum.Value.END > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                // alan.bilgikodu = "Borsa İstanbul Endeksleri - Değişken - Yurtiçi";
                                alan.bilgikodu = "END";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.END.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fend.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fend * mdurum.Value.END).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }

                            #endregion
                            #region PIT
                            if (mdurum.Value.PIT > 0)
                            {



                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "Pay İşlem Tarafı Günsonu - Değişken - Yurtiçi";
                                alan.bilgikodu = "PIT";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.PIT.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpit.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpit * mdurum.Value.PIT).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region PITE
                            if (mdurum.Value.PITE > 0)
                            {

                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi";
                                alan.bilgikodu = "PITE";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.PITE.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpite.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpite * mdurum.Value.PITE).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region VL1
                            //if (mdurum.Value.VL1 > 0)
                            //{

                            //    var alan = new alanlar();
                            //    alan.aboneno = mdurum.Key;
                            //    alan.aboneadi = mdurum.Value.musteriadi;
                            //    alan.adres = mdurum.Value.adres;
                            //    alan.sehir = mdurum.Value.sehir;
                            //    alan.ulke = mdurum.Value.ulke;
                            //    alan.bilgikodu = "VİOP Düzey 1 - Değişken - Yurtiçi";
                            //    alan.bilgikodu = "VD1";
                            //    alan.mensei = mdurum.Value.mensei;
                            //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            //    alan.cihaztoplam = mdurum.Value.VL1.ToString();
                            //    if (mdurum.Value.Bedelsizaciklama == "")
                            //    {
                            //        alan.bilgibedel = MyTools.lisansfiyatlari.Fvl1.ToString("0.00");
                            //        alan.toplambedel = (MyTools.lisansfiyatlari.Fvl1 * mdurum.Value.VL1).ToString("0.00");
                            //    }
                            //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            //    alanlarlistesi.Add(alan);
                            //}
                            #endregion
                            #region VL1P
                            if (mdurum.Value.VL1P > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "VİOP Düzey 1P - Değişken - Yurtiçi";
                                alan.bilgikodu = "VD1P";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.VL1P.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvl1p.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvl1p * mdurum.Value.VL1P).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region VL2
                            if (mdurum.Value.VL2 > 0)
                            {

                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "VİOP Düzey 2 - Değişken - Yurtiçi";
                                alan.bilgikodu = "VD2";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.VL2.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvl2.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvl2 * mdurum.Value.VL2).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region VL2P
                            if (mdurum.Value.VL2P > 0)
                            {

                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "VİOP Düzey 2P- Değişken - Yurtiçi";
                                alan.bilgikodu = "VD2P";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.VL2P.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvl2p.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvl2p * mdurum.Value.VL2P).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region VIT
                            if (mdurum.Value.VIT > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "VİOP İşlem Tarafı Günsonu - Değişken - Yurtiçi";
                                alan.bilgikodu = "VIT";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.VIT.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvit.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvit * mdurum.Value.VIT).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region KRMD1
                            if (mdurum.Value.KRMD1 > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "Karma Düzey 1 - Yurtiçi";
                                alan.bilgikodu = "KRMD1";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.KRMD1.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = "0";
                                    alan.toplambedel = "0";
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }

                            #endregion
                            #region BD1
                            if (mdurum.Value.BD1 > 0)
                            {

                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "BAP Düzey 1 - Değişken - Yurtiçi";
                                alan.bilgikodu = "BD1";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.BD1.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fbd1.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fbd1 * mdurum.Value.BD1).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region BD1P
                            if (mdurum.Value.BD1P > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "BAP Düzey 1P - Değişken - Yurtiçi";
                                alan.bilgikodu = "BD1P";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.BD1P.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fbd1p.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fbd1p * mdurum.Value.BD1P).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region BD2
                            if (mdurum.Value.BD2 > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "BAP Düzey 2 - Değişken - Yurtiçi";
                                alan.bilgikodu = "BD2";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.BD2.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fbd2.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fbd2 * mdurum.Value.BD2).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region MKK
                            if (mdurum.Value.MKK > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "MKK - Yurtiçi";
                                alan.bilgikodu = "MKK";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.MKK.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = "0";
                                    alan.toplambedel = "0";
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }

                            #endregion
                            #region TARAMA
                            if (mdurum.Value.TARAMA > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "TARAMA - Yurtiçi";
                                alan.bilgikodu = "TARAMA";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.TARAMA.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = "0";
                                    alan.toplambedel = "0";
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }

                            #endregion
                            #region GKKUL
                            if (mdurum.Value.GKKUL > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "GKKUL - Yurtiçi";
                                alan.bilgikodu = "GKKUL";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.GKKUL.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = "0";
                                    alan.toplambedel = "0";
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }

                            #endregion

                        }


                        #region yurdisilar


                        foreach (var mdurum in musteridurum.ydurumdictionary)
                        {

                            #region PD1
                            //if (mdurum.Value.PD1 > 0)
                            //{
                            //    var alan = new alanlar();
                            //    alan.aboneno = mdurum.Key;
                            //    alan.aboneadi = mdurum.Value.musteriadi;
                            //    alan.adres = mdurum.Value.adres;
                            //    alan.sehir = mdurum.Value.sehir;
                            //    alan.ulke = mdurum.Value.ulke;
                            //    alan.bilgikodu = "PAY Düzey 1 - Değişken - Yurtdışı";
                            //    alan.bilgikodu = "PD1";
                            //    alan.mensei = mdurum.Value.mensei;
                            //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            //    alan.cihaztoplam = mdurum.Value.PD1.ToString();
                            //    if (mdurum.Value.Bedelsizaciklama == "")
                            //    {
                            //        alan.bilgibedel = MyTools.lisansfiyatlari.Ypd1.ToString("0.00");
                            //        alan.toplambedel = (MyTools.lisansfiyatlari.Ypd1 * mdurum.Value.PD1).ToString("0.00");
                            //    }
                            //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            //    alanlarlistesi.Add(alan);
                            //}
                            #endregion
                            #region PD1P
                            if (mdurum.Value.PD1P > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "PAY Düzey 1P - Değişken - Yurtdışı";
                                alan.bilgikodu = "PD1P";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.PD1P.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypd1p.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypd1p * mdurum.Value.PD1P).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }

                            #endregion
                            #region PD2
                            if (mdurum.Value.PD2 > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "PAY Düzey 2 - Değişken - Yurtdışı";
                                alan.bilgikodu = "PD2";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.PD2.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypd2.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypd2 * mdurum.Value.PD2).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region PD2P
                            if (mdurum.Value.PD2P > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "PAY Düzey 2P- Değişken - Yurtdışı";
                                alan.bilgikodu = "PD2P";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.PD2P.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypd2p.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypd2p * mdurum.Value.PD2P).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region END
                            if (mdurum.Value.END > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "Borsa İstanbul Endeksleri - Değişken - Yurtdışı";
                                alan.bilgikodu = "END";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.END.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yend.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yend * mdurum.Value.END).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region PIT
                            if (mdurum.Value.PIT > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "Pay İşlem Tarafı Günsonu - Değişken - Yurtdışı";
                                alan.bilgikodu = "PIT";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.PIT.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypit.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypit * mdurum.Value.PIT).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region PITE
                            if (mdurum.Value.PITE > 0)
                            {

                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "Pay İşlem Tarafı Eşanlı - Değişken - Yurtdışı";
                                alan.bilgikodu = "PITE";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.PITE.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypite.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypite * mdurum.Value.PITE).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region VL1
                            //if (mdurum.Value.VL1 > 0)
                            //{
                            //    var alan = new alanlar();
                            //    alan.aboneno = mdurum.Key;
                            //    alan.aboneadi = mdurum.Value.musteriadi;
                            //    alan.adres = mdurum.Value.adres;
                            //    alan.sehir = mdurum.Value.sehir;
                            //    alan.ulke = mdurum.Value.ulke;
                            //    alan.bilgikodu = "VİOP Düzey 1 - Değişken - Yurtdışı";
                            //    alan.bilgikodu = "VD1";
                            //    alan.mensei = mdurum.Value.mensei;
                            //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            //    alan.cihaztoplam = mdurum.Value.VL1.ToString();
                            //    if (mdurum.Value.Bedelsizaciklama == "")
                            //    {
                            //        alan.bilgibedel = MyTools.lisansfiyatlari.Yvl1.ToString("0.00");
                            //        alan.toplambedel = (MyTools.lisansfiyatlari.Yvl1 * mdurum.Value.VL1).ToString("0.00");
                            //    }
                            //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            //    alanlarlistesi.Add(alan);
                            //}
                            #endregion
                            #region VL1p
                            if (mdurum.Value.VL1P > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "VİOP Düzey 1P - Değişken - Yurtdışı";
                                alan.bilgikodu = "VD1P";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.VL1P.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvl1p.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvl1p * mdurum.Value.VL1P).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region VL2
                            if (mdurum.Value.VL2 > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "VİOP Düzey 2 - Değişken - Yurtdışı";
                                alan.bilgikodu = "VD2";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.VL2.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvl2.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvl2 * mdurum.Value.VL2).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region VL2P
                            if (mdurum.Value.VL2P > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "VİOP Düzey 2P - Değişken - Yurtdışı";
                                alan.bilgikodu = "VD2P";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.VL2P.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvl2p.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvl2p * mdurum.Value.VL2P).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region VIT
                            if (mdurum.Value.VIT > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "VİOP İşlem Tarafı Günsonu - Değişken - Yurtdışı";
                                alan.bilgikodu = "VIT";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.VIT.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvit.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvit * mdurum.Value.VIT).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region KRMD1
                            if (mdurum.Value.KRMD1 > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "Karma Düzey 1 - Yurtdışı";
                                alan.bilgikodu = "KRMD1";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.KRMD1.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ykrmd1.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ykrmd1 * mdurum.Value.KRMD1).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region BD1

                            if (mdurum.Value.BD1 > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "BAP Düzey 1 - Değişken - Yurtdışı";
                                alan.bilgikodu = "BD1";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.BD1.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ybd1.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ybd1 * mdurum.Value.BD1).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region BD1P
                            if (mdurum.Value.BD1P > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "BAP Düzey 1P - Değişken - Yurtdışı";
                                alan.bilgikodu = "BD1P";
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.mensei = mdurum.Value.mensei;
                                alan.cihaztoplam = mdurum.Value.BD1P.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ybd1p.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ybd1p * mdurum.Value.BD1P).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                            #region BD2
                            if (mdurum.Value.BD2 > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "BAP Düzey 2 - Değişken - Yurtdışı";
                                alan.bilgikodu = "BD2";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.BD2.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ybd2.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ybd2 * mdurum.Value.BD2).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion

                            #region MKK
                            if (mdurum.Value.MKK > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "MKK - Yurtdışı";
                                alan.bilgikodu = "MKK";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.MKK.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ymkk.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ymkk * mdurum.Value.MKK).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion

                            //#region TARAMA
                            //if (mdurum.Value.TARAMA > 0)
                            //{
                            //    var alan = new alanlar();
                            //    alan.aboneno = mdurum.Key;
                            //    alan.aboneadi = mdurum.Value.musteriadi;
                            //    alan.adres = mdurum.Value.adres;
                            //    alan.sehir = mdurum.Value.sehir;
                            //    alan.ulke = mdurum.Value.ulke;
                            //    alan.bilgikodu = "TARAMA - Yurtdışı";
                            //    alan.bilgikodu = "TARAMA";
                            //    alan.mensei = mdurum.Value.mensei;
                            //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                            //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                            //    alan.cihaztoplam = mdurum.Value.TARAMA.ToString();
                            //    if (mdurum.Value.Bedelsizaciklama == "")
                            //    {
                            //        alan.bilgibedel = MyTools.lisansfiyatlari.Ytarama.ToString("0.00");
                            //        alan.toplambedel = (MyTools.lisansfiyatlari.Ytarama * mdurum.Value.TARAMA).ToString("0.00");
                            //    }
                            //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                            //    alanlarlistesi.Add(alan);
                            //}
                            //#endregion
                            #region GKKUL
                            if (mdurum.Value.GKKUL > 0)
                            {
                                var alan = new alanlar();
                                alan.aboneno = mdurum.Key;
                                alan.aboneadi = mdurum.Value.musteriadi;
                                alan.adres = mdurum.Value.adres;
                                alan.sehir = mdurum.Value.sehir;
                                alan.ulke = mdurum.Value.ulke;
                                alan.bilgikodu = "GKKUL - Yurtdışı";
                                alan.bilgikodu = "GKKUL";
                                alan.mensei = mdurum.Value.mensei;
                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;
                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;
                                alan.cihaztoplam = mdurum.Value.GKKUL.ToString();
                                if (mdurum.Value.Bedelsizaciklama == "")
                                {
                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ygkkul.ToString("0.00");
                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ygkkul * mdurum.Value.GKKUL).ToString("0.00");
                                }
                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;

                                alanlarlistesi.Add(alan);
                            }
                            #endregion
                        }
                        #endregion




                        var cellarray = new object[alanlarlistesi.Count + 7, 15];
                        //for (int i = 0; i < cellarray.GetLength(0); i++)
                        //{
                        //    //for (int j = 0; j < cellarray.GetLength(1); j++)
                        //    //    cellarray[i, j] = "";
                        //}


                        var currrow = 0;

                        var date = DateTime.Now;

                        cellarray[currrow, 0] = "Dağıtıcı Adı";
                        cellarray[currrow, 1] = "";
                        currrow++;

                        excelsheetDetay.Application.get_Range("A" + currrow, "B" + currrow).Font.Size = 16;

                        cellarray[currrow, 0] = "Borsa";
                        cellarray[currrow, 1] = "Borsa İstanbul A.Ş.";
                        currrow++;
                        cellarray[currrow, 0] = "Raporlanan Ay";
                        cellarray[currrow, 1] = date.ToString("MMMMMMMMM").ToUpper().Trim() + " - " + date.Year;
                        currrow++;
                        cellarray[currrow, 0] = "Rapor Tipi";
                        cellarray[currrow, 1] = "Detay";

                        currrow++;


                        excelsheetDetay.Application.get_Range("A" + 1, "E" + currrow).Font.Bold = true;
                        excelsheetDetay.Application.get_Range("A" + 1, "E" + currrow).Interior.Color = ColorTranslator.ToOle(Color.FromArgb(192, 192, 192));

                        cellarray[currrow, 0] = "";

                        currrow++;
                        cellarray[currrow, 0] = "Abone Kodu";
                        cellarray[currrow, 1] = "Abone Adı Soyadı";
                        cellarray[currrow, 2] = "Adres";
                        cellarray[currrow, 3] = "Şehir";
                        cellarray[currrow, 4] = "postakodu";
                        cellarray[currrow, 5] = "Ülke";
                        cellarray[currrow, 6] = "Raporlama Bilgi Kodu";
                        cellarray[currrow, 7] = "Yurtiçi / YurtDışı";
                        cellarray[currrow, 8] = "Erişim Tipi";
                        cellarray[currrow, 9] = "Toplam Cihaz Adet";
                        cellarray[currrow, 10] = "Bilgi Bedeli";
                        cellarray[currrow, 11] = "Toplam Bedel";
                        cellarray[currrow, 12] = "Yetkilendirme Tarihi";
                        cellarray[currrow, 13] = "Lisans Yenileme Tarihi";
                        cellarray[currrow, 14] = "Bedelsiz Açıklama";

                        currrow++;

                        excelsheetDetay.Application.get_Range("A" + currrow, "O" + currrow).Interior.Color = ColorTranslator.ToOle(Color.FromArgb(192, 192, 192));
                        excelsheetDetay.Application.get_Range("A" + currrow, "O" + currrow).Font.Bold = true;
                        excelsheetDetay.Application.get_Range("A" + currrow, "O" + currrow).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        excelsheetDetay.Application.get_Range("A" + currrow, "O" + currrow).RowHeight = 60;

                        SafeFreezePanes(excelsheetDetay, 6);


                        for (int i = 0; i < alanlarlistesi.Count; i++)
                        {

                            cellarray[currrow, 0] = alanlarlistesi[i].aboneno;
                            cellarray[currrow, 1] = alanlarlistesi[i].aboneadi;
                            cellarray[currrow, 2] = alanlarlistesi[i].adres;
                            cellarray[currrow, 3] = alanlarlistesi[i].sehir;
                            cellarray[currrow, 4] = alanlarlistesi[i].postakod;
                            cellarray[currrow, 5] = alanlarlistesi[i].ulke;
                            cellarray[currrow, 6] = alanlarlistesi[i].bilgikodu;
                            cellarray[currrow, 7] = alanlarlistesi[i].mensei;
                            cellarray[currrow, 8] = alanlarlistesi[i].erisimtipi;
                            cellarray[currrow, 9] = alanlarlistesi[i].cihaztoplam;
                            cellarray[currrow, 10] = alanlarlistesi[i].bilgibedel;
                            cellarray[currrow, 11] = alanlarlistesi[i].toplambedel;
                            cellarray[currrow, 12] = alanlarlistesi[i].yetkikendirmetar;
                            cellarray[currrow, 13] = alanlarlistesi[i].lisansYenilemeTarihi;
                            cellarray[currrow, 14] = alanlarlistesi[i].bedelsizaciklama;
                            currrow++;
                        }




                        //Microsoft.Office.Interop.Excel.Range range2 = excelsheetDetay.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                        //range2.Value = cellarray;
                        // YENİ: Sadece dolu satırları yaz
                        Microsoft.Office.Interop.Excel.Range range2 = excelsheetDetay.Cells.get_Resize(currrow, cellarray.GetLength(1));
                        // Dolu kısmı kopyala
                        var trimmedArray = new object[currrow, cellarray.GetLength(1)];
                        Array.Copy(cellarray, trimmedArray, currrow * cellarray.GetLength(1));
                        range2.Value = trimmedArray;
                        range2.Columns.AutoFit();

                        ExcelDurumYaz("DETAY Sayfa Aktarımı Bitti");

                        #endregion
                        #region OZET

                        Sorgu = crm.Users.Where(x => x.StatusId == 1);

                        var excelsheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.Add();
                        excelsheet.Name = "ÖZET";
                        List<alanlar> alanlar = new List<alanlar>();
                        ExcelDurumYaz("ÖZET Sayfa Aktarılıyor");
                        string RaporKurum = "";
                        #region OzetLisansHesapla



                        //#region BD1
                        //var TahvilYuzeysel = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false).Count();
                        //alanlar.Add(new alanlar { bilgikodu = "BAP Düzey 1 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = TahvilYuzeysel.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fbd1.ToString("0.00"), toplambedel = (TahvilYuzeysel * MyTools.lisansfiyatlari.Fbd1).ToString("0.00") });
                        //var TahvilYuzeysel_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false).Count();
                        //alanlar.Add(new alanlar { bilgikodu = "BAP Düzey 1 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = TahvilYuzeysel_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ybd1.ToString("0.00"), toplambedel = (TahvilYuzeysel_Y * MyTools.lisansfiyatlari.Ybd1).ToString("0.00") });

                        //#endregion
                        #region BD1P
                        var TahvilPlus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "BAP Düzey 1P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = TahvilPlus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fbd1p.ToString("0.00"), toplambedel = (TahvilPlus * MyTools.lisansfiyatlari.Fbd1p).ToString("0.00") });
                        var TahvilPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == true).Count();

                        alanlar.Add(new alanlar { bilgikodu = "BAP Düzey 1P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = TahvilPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ybd1p.ToString("0.00"), toplambedel = (TahvilPlus_Y * MyTools.lisansfiyatlari.Ybd1p).ToString("0.00") });

                        #endregion
                        #region BD2
                        var TahvilDerinlik = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == true && x.LisansDurum.TahvilLP == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "BAP Düzey 2 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = TahvilDerinlik.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fbd2.ToString("0.00"), toplambedel = (TahvilDerinlik * MyTools.lisansfiyatlari.Fbd2).ToString("0.00") });
                        var TahvilDerinlik_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == true && x.LisansDurum.TahvilLP == true).Count();

                        alanlar.Add(new alanlar { bilgikodu = "BAP Düzey 2 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = TahvilDerinlik_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ybd2.ToString("0.00"), toplambedel = (TahvilDerinlik_Y * MyTools.lisansfiyatlari.Ybd2).ToString("0.00") });

                        #endregion

                        #region END
                        var PayEndeks = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayX == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "Borsa İstanbul Endeksleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = PayEndeks.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fend.ToString("0.00"), toplambedel = (PayEndeks * MyTools.lisansfiyatlari.Fend).ToString("0.00") });
                        var PayEndeks_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayX == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "Borsa İstanbul Endeksleri - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = PayEndeks_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yend.ToString("0.00"), toplambedel = (PayEndeks_Y * MyTools.lisansfiyatlari.Yend).ToString("0.00") });

                        #endregion
                        #region GosterimsizKullanim
                        var GKULKYD = 0;
                        var GKULEND = 0;
                        var GKULD1P = 0;
                        var GKULD2 = 0;
                        var GKULPITE = 0;
                        var GKULPVA = 0;

                        var GUYEKYD = 0;
                        var GUYEEND = 0;
                        var GUYED1P = 0;
                        var GUYED2 = 0;
                        var GUYEPITE = 0;
                        var GUYEPVA = 0;

                        GKULEND = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GKULEND == true).Count();
                        GKULD1P = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GKULD1P == true).Count();
                        GKULD2 = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GKULD2 == true).Count();
                        GKULPITE = 0;
                        GKULPVA = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GKULPVA == true).Count();

                        GUYEEND = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GUYEEND == true).Count();
                        GUYED1P = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GUYED1P == true).Count();
                        GUYED2 = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GUYED2 == true).Count();
                        GUYEPITE = 0;
                        GUYEPVA = crm.Projelers.Where(x => x.yayinDurum == true && x.BorsaAltLisansar.GUYEPVA == true).Count();



                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. BIST-KYD Endeksleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GKULKYD.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULKYD.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Borsa İstanbul Endeksleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GKULEND.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULEND.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Düzey 1P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GKULD1P.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULD1P.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Düzey 2/2P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GKULD2.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULD2.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GKULPITE.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULPITE.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Pay Veri Analitikleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GKULPVA.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULPVA.ToString("0.00"), toplambedel = "" });

                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Üye BIST-KYD Endeksleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GUYEKYD.ToString(), bilgibedel = MyTools.lisansfiyatlari.GUYEKYD.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Üye Borsa İstanbul Endeksleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GUYEEND.ToString(), bilgibedel = MyTools.lisansfiyatlari.GUYEEND.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Üye Düzey 1P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GUYED1P.ToString(), bilgibedel = MyTools.lisansfiyatlari.GUYED1P.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Üye Düzey 2/2P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GUYED2.ToString(), bilgibedel = MyTools.lisansfiyatlari.GUYED2.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Üye Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GUYEPITE.ToString(), bilgibedel = MyTools.lisansfiyatlari.GUYEPITE.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Üye Pay Veri Analitikleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = GUYEPVA.ToString(), bilgibedel = MyTools.lisansfiyatlari.GUYEPVA.ToString("0.00"), toplambedel = "" });

                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. BIST-KYD Endeksleri - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = GKULKYD.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULKYD.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Borsa İstanbul Endeksleri - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = GKULEND.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULEND.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Düzey 1P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = GKULD1P.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULD1P.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Düzey 2/2P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = GKULD2.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULD2.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Pay İşlem Tarafı Eşanlı - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = GKULPITE.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULPITE.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Gösterimsiz Kul. Pay Veri Analitikleri - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = GKULPVA.ToString(), bilgibedel = MyTools.lisansfiyatlari.GKULPVA.ToString("0.00"), toplambedel = "" });

                        #endregion
                        #region KMTP1P
                        alanlar.Add(new alanlar { bilgikodu = "KMTP Düzey 1P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = "0", bilgibedel = "11", toplambedel = "0" });
                        alanlar.Add(new alanlar { bilgikodu = "KMTP Düzey 1P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = "0", bilgibedel = "5", toplambedel = "0" });
                        #endregion
                        #region KMTP2
                        alanlar.Add(new alanlar { bilgikodu = "KMTP Düzey 2 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = "0", bilgibedel = "21", toplambedel = "0" });
                        alanlar.Add(new alanlar { bilgikodu = "KMTP Düzey 2 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = "0", bilgibedel = "9", toplambedel = "0" });
                        #endregion
                        //#region PD1
                        //var PayYuzeysel = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false).Count();
                        //alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 1 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = PayYuzeysel.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpd1.ToString("0.00"), toplambedel = (PayYuzeysel * MyTools.lisansfiyatlari.Fpd1).ToString("0.00") });
                        //var PayYuzeysel_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false).Count();
                        //alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 1 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = PayYuzeysel_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypd1.ToString("0.00"), toplambedel = (PayYuzeysel_Y * MyTools.lisansfiyatlari.Ypd1).ToString("0.00") });

                        //#endregion
                        #region PD1P
                        var PayPlus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 1P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = PayPlus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpd1p.ToString("0.00"), toplambedel = (PayPlus * MyTools.lisansfiyatlari.Fpd1p).ToString("0.00") });
                        var PayPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 1P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = PayPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypd1p.ToString("0.00"), toplambedel = (PayPlus_Y * MyTools.lisansfiyatlari.Ypd1p).ToString("0.00") });

                        #endregion
                        #region PD2
                        var PayDerinlik = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                        alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 2 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = PayDerinlik.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpd2.ToString("0.00"), toplambedel = (PayDerinlik * MyTools.lisansfiyatlari.Fpd2).ToString("0.00") });
                        var PayDerinlik_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                        alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 2 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = PayDerinlik_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypd2.ToString("0.00"), toplambedel = (PayDerinlik_Y * MyTools.lisansfiyatlari.Ypd2).ToString("0.00") });

                        #endregion
                        #region PD2P

                        var PayDerinlikplus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 2P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = PayDerinlikplus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpd2p.ToString("0.00"), toplambedel = (PayDerinlikplus * MyTools.lisansfiyatlari.Fpd2p).ToString("0.00") });
                        var PayDerinlikPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 2P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = PayDerinlikPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypd2p.ToString("0.00"), toplambedel = (PayDerinlikPlus_Y * MyTools.lisansfiyatlari.Ypd2p).ToString("0.00") });
                        //alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 2P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = "0", bilgibedel = "75", toplambedel = "0" });
                        //alanlar.Add(new alanlar { bilgikodu = "PAY Düzey 2P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = "0", bilgibedel = "32", toplambedel = "0" });

                        #endregion

                        #region PIT
                        var PayGs = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayGS == true && x.LisansDurum.PITE == false).Count();
                        alanlar.Add(new alanlar { bilgikodu = "Pay İşlem Tarafı Günsonu - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = PayGs.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpit.ToString("0.00"), toplambedel = (PayGs * MyTools.lisansfiyatlari.Fpit).ToString("0.00") });
                        var PayGs_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayGS == true && x.LisansDurum.PITE == false).Count();
                        alanlar.Add(new alanlar { bilgikodu = "Pay İşlem Tarafı Günsonu - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = PayGs_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypit.ToString("0.00"), toplambedel = (PayGs_Y * MyTools.lisansfiyatlari.Ypit).ToString("0.00") });

                        #endregion
                        #region AND
                        alanlar.Add(new alanlar { bilgikodu = "Pay Veri Analitikleri - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = "0", bilgibedel = "44", toplambedel = "0" });
                        alanlar.Add(new alanlar { bilgikodu = "Pay Veri Analitikleri - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = "0", bilgibedel = "19", toplambedel = "0" });
                        #endregion
                        #region PITE
                        var pite = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PITE == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "Pay İşlem Tarafı Eşanlı - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = pite.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fpite.ToString("0.00"), toplambedel = (pite * MyTools.lisansfiyatlari.Fpite).ToString("0.00") });
                        var pite_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.PITE == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "Pay İşlem Tarafı Eşanlı - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = pite_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ypva.ToString("0.00"), toplambedel = (pite_Y * MyTools.lisansfiyatlari.Ypite).ToString("0.00") });

                        #endregion

                        //#region VD1
                        //var viopYuzeysel = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false).Count();
                        //alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 1 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = viopYuzeysel.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvl1.ToString("0.00"), toplambedel = (viopYuzeysel * MyTools.lisansfiyatlari.Fvl1).ToString("0.00") });
                        //var viopYuzeysel_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false).Count();
                        //alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 1 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = viopYuzeysel_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvl1.ToString("0.00"), toplambedel = (viopYuzeysel_Y * MyTools.lisansfiyatlari.Yvl1).ToString("0.00") });


                        //#endregion
                        #region VD1P
                        var viopPlus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 1P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = viopPlus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvl1p.ToString("0.00"), toplambedel = (viopPlus * MyTools.lisansfiyatlari.Fvl1p).ToString("0.00") });
                        var viopPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 1P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = viopPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvl1p.ToString("0.00"), toplambedel = (viopPlus_Y * MyTools.lisansfiyatlari.Yvl1p).ToString("0.00") });

                        #endregion
                        #region VD2
                        var viopDerinlik = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                        alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 2 - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = viopDerinlik.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvl2.ToString("0.00"), toplambedel = (viopDerinlik * MyTools.lisansfiyatlari.Fvl2).ToString("0.00") });
                        var viopDerinlik_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                        alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 2 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = viopDerinlik_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvl2.ToString("0.00"), toplambedel = (viopDerinlik_Y * MyTools.lisansfiyatlari.Yvl2).ToString("0.00") });

                        #endregion
                        #region VD2P
                        var viopDerinlikPlus = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 2P - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = viopDerinlikPlus.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvl2p.ToString("0.00"), toplambedel = (viopDerinlikPlus * MyTools.lisansfiyatlari.Fvl2p).ToString("0.00") });
                        var viopDerinlikPlus_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "VİOP Düzey 2P - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = viopDerinlikPlus_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvl2p.ToString("0.00"), toplambedel = (viopDerinlikPlus_Y * MyTools.lisansfiyatlari.Yvl2p).ToString("0.00") });

                        #endregion
                        #region VIT

                        var viopGS = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "VİOP İşlem Tarafı Günsonu - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = viopGS.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fvit.ToString("0.00"), toplambedel = (viopGS * MyTools.lisansfiyatlari.Fvit).ToString("0.00") });
                        var viopGS_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "VİOP İşlem Tarafı Günsonu - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = viopGS_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Yvit.ToString("0.00"), toplambedel = (viopGS_Y * MyTools.lisansfiyatlari.Yvit).ToString("0.00") });

                        #endregion
                        #region MKK

                        var mkk = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.MKK == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "MKK - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = mkk.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fmkk.ToString("0.00"), toplambedel = (mkk * MyTools.lisansfiyatlari.Fmkk).ToString("0.00") });
                        var mkk_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.MKK == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "MKK - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = mkk_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ymkk.ToString("0.00"), toplambedel = (mkk_Y * MyTools.lisansfiyatlari.Ymkk).ToString("0.00") });

                        #endregion
                        //#region TARAMA

                        //var tarama = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.TARAMA == true).Count();
                        //alanlar.Add(new alanlar { bilgikodu = "TARAMA - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = tarama.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ftarama.ToString("0.00"), toplambedel = (tarama * MyTools.lisansfiyatlari.Ftarama).ToString("0.00") });
                        //var tarama_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();
                        //alanlar.Add(new alanlar { bilgikodu = "TARAMA - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = tarama_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ytarama.ToString("0.00"), toplambedel = (tarama_Y * MyTools.lisansfiyatlari.Ytarama).ToString("0.00") });

                        //#endregion
                        #region GKKUL

                        var gkkul = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.GKKUL == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "GKKUL - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = gkkul.ToString(), bilgibedel = MyTools.lisansfiyatlari.Fgkkul.ToString("0.00"), toplambedel = (gkkul * MyTools.lisansfiyatlari.Fgkkul).ToString("0.00") });
                        var gkkul_Y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.GKKUL == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "GKKUL - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = gkkul_Y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ygkkul.ToString("0.00"), toplambedel = (gkkul_Y * MyTools.lisansfiyatlari.Ygkkul).ToString("0.00") });

                        #endregion
                        #region KARMA

                        var karma1K = 0;
                        var karma5K = 0;
                        var karma10K = 0;
                        var karma20K = 0;
                        var karma50K = 0;
                        var karma100K = 0;
                        var karmaSINIRSIZ = 0;
                        var krmd1 = Sorgu.Where(x => x.MusteriMenseiID == 1 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.COMEX == true).Count();

                        if (krmd1 > 0 && krmd1 <= 1000) karma1K = krmd1;
                        if (krmd1 > 1000 && krmd1 <= 5000) karma5K = krmd1;
                        if (krmd1 > 5000 && krmd1 <= 10000) karma10K = krmd1;
                        if (krmd1 > 10000 && krmd1 <= 20000) karma20K = krmd1;
                        if (krmd1 > 20000 && krmd1 <= 50000) karma50K = krmd1;
                        if (krmd1 > 50000 && krmd1 <= 100000) karma100K = krmd1;
                        if (krmd1 > 100000) karmaSINIRSIZ = krmd1;

                        alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - 1K Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karma1K.ToString(), bilgibedel = MyTools.lisansfiyatlari.karma1k.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - 5K Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karma5K.ToString(), bilgibedel = MyTools.lisansfiyatlari.karma5k.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - 10K Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karma10K.ToString(), bilgibedel = MyTools.lisansfiyatlari.karma10k.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - 20K Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karma20K.ToString(), bilgibedel = MyTools.lisansfiyatlari.karma20k.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - 50K Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karma50K.ToString(), bilgibedel = MyTools.lisansfiyatlari.karma50k.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - 100K Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karma100K.ToString(), bilgibedel = MyTools.lisansfiyatlari.karma100k.ToString("0.00"), toplambedel = "" });
                        alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - Sınırsız Kullanıcı Paketi - Değişken - Yurtiçi", mensei = "Yurtiçi", cihaztoplam = karmaSINIRSIZ.ToString(), bilgibedel = MyTools.lisansfiyatlari.karmaSINIRSIZ.ToString("0.00"), toplambedel = "" });


                        var krmd1_y = Sorgu.Where(x => x.MusteriMenseiID == 2 && x.LisansDurum.YayinDurumu == true && x.LisansDurum.COMEX == true).Count();
                        alanlar.Add(new alanlar { bilgikodu = "Karma Düzey 1 - Değişken - Yurtdışı", mensei = "Yurtdışı", cihaztoplam = krmd1_y.ToString(), bilgibedel = MyTools.lisansfiyatlari.Ykrmd1.ToString("0.00"), toplambedel = (krmd1_y * MyTools.lisansfiyatlari.Ykrmd1).ToString("0.00") });

                        #endregion




                        #endregion
                        cellarray = new object[alanlar.Count + 5, 6];
                        //for (int i = 0; i < cellarray.GetLength(0); i++)
                        //{
                        //    //for (int j = 0; j < cellarray.GetLength(1); j++)
                        //    //    cellarray[i, j] = "";
                        //}


                        var yurticialanlar = alanlar.Where(x => x.mensei == "Yurtiçi").ToList();
                        var yurtDisialanlar = alanlar.Where(x => x.mensei == "Yurtdışı").ToList();


                        Microsoft.Office.Interop.Excel.Range baslikrange2 = excelsheet.Application.get_Range("A1", "G1");
                        baslikrange2.Font.Bold = true;
                        baslikrange2.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(220, 230, 241));
                        baslikrange2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        baslikrange2.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                        baslikrange2.RowHeight = 40;
                        baslikrange2.Application.ActiveWindow.SplitRow = 1;
                        baslikrange2.Application.ActiveWindow.FreezePanes = true;
                        baslikrange2.Borders.Value = true;
                        cellarray[0, 0] = "Dağıtıcı Adı";
                        cellarray[0, 1] = "Ürün";
                        cellarray[0, 2] = "Kullanıcı Sayısı";
                        cellarray[0, 3] = "Birim Fiyat";
                        cellarray[0, 4] = "Para Birimi";
                        cellarray[0, 5] = "İndirim Tutarı";
                        // cellarray[0, 6] = "Toplam";

                        Microsoft.Office.Interop.Excel.Range baslikToplam = excelsheet.Application.get_Range("G1");
                        baslikToplam.Value = "Toplam";
                        currrow = 1;
                        var firstrow = 0;
                        for (int i = 0; i < yurticialanlar.Count; i++)
                        {

                            cellarray[currrow, 0] = RaporKurum;
                            cellarray[currrow, 1] = yurticialanlar[i].bilgikodu;
                            cellarray[currrow, 2] = yurticialanlar[i].cihaztoplam;
                            cellarray[currrow, 3] = yurticialanlar[i].bilgibedel;
                            cellarray[currrow, 4] = "TL";
                            cellarray[currrow, 5] = 0.00;

                            var krmd1Say = 0;
                            double krmd1Fiyat = 0;

                            if (yurticialanlar[i].bilgikodu.StartsWith("Karma"))
                            {
                                if (yurticialanlar[i].bilgikodu.Contains("1K")) { krmd1Say = 1000; krmd1Fiyat = 2.8; }
                                else if (yurticialanlar[i].bilgikodu.Contains("5K")) { krmd1Say = 5000; krmd1Fiyat = 2.5; }
                                else if (yurticialanlar[i].bilgikodu.Contains("10K")) { krmd1Say = 10000; krmd1Fiyat = 2.2; }
                                else if (yurticialanlar[i].bilgikodu.Contains("20K")) { krmd1Say = 20000; krmd1Fiyat = 1.9; }
                                else if (yurticialanlar[i].bilgikodu.Contains("50K")) { krmd1Say = 50000; krmd1Fiyat = 1.65; }
                                else if (yurticialanlar[i].bilgikodu.Contains("100K")) { krmd1Say = 100000; krmd1Fiyat = 1.45; }

                            }

                            Microsoft.Office.Interop.Excel.Range fomula = excelsheet.Application.get_Range("G" + (currrow + 1));


                            var formulx = "";
                            if (yurticialanlar[i].bilgikodu.StartsWith("Karma") && yurticialanlar[i].bilgikodu.Contains("Sınırsız") == false)
                                formulx = "=EĞER(C" + (currrow + 1) + "<1;0;EĞER(C" + (currrow + 1) + "<" + krmd1Say + ";D" + (currrow + 1) + ";C" + (currrow + 1) + "*" + krmd1Fiyat + "))";
                            else if (yurticialanlar[i].bilgikodu.StartsWith("Karma") && yurticialanlar[i].bilgikodu.Contains("Sınırsız"))
                                formulx = "=EĞER(C" + (currrow + 1) + "<1;0;D" + (currrow + 1) + ")";
                            else
                                formulx = "=(C" + (currrow + 1) + "*D" + (currrow + 1) + ")-(C" + (currrow + 1) + "*F" + (currrow + 1) + ")";

                            fomula.FormulaLocal = formulx;


                            currrow++;
                            if (i == 0) firstrow = currrow;
                            ExcelDurumYaz("ÖZET Sayfa Aktarılıyor (Yurt İçi) -->  " + (i).ToString() + " / " + yurticialanlar.Count.ToString());
                        }
                        //cellarray[currrow - 1, 7] = "TL Toplam";
                        //cellarray[currrow - 1, 8] = "=TOPLA(G" + (firstrow) + ":G" + (currrow) + ")";
                        excelsheet.Application.get_Range("H" + firstrow, "I" + currrow).Font.Bold = true;

                        excelsheet.Application.get_Range("H" + currrow).Value = "TL Toplam";
                        excelsheet.Application.get_Range("I" + currrow).FormulaLocal = "=TOPLA(G2:G" + currrow + ")";
                        excelsheet.Application.get_Range("I" + currrow).NumberFormat = "#,###,###.00 TL";

                        var ortaRow = currrow + 1;

                        if (yurtDisialanlar.Count > 0)
                        {
                            for (int i = 0; i < yurtDisialanlar.Count; i++)
                            {
                                cellarray[currrow, 0] = RaporKurum;
                                cellarray[currrow, 1] = yurtDisialanlar[i].bilgikodu;
                                cellarray[currrow, 2] = yurtDisialanlar[i].cihaztoplam;
                                cellarray[currrow, 3] = yurtDisialanlar[i].bilgibedel;
                                cellarray[currrow, 4] = "$";
                                cellarray[currrow, 5] = 0.00;

                                Microsoft.Office.Interop.Excel.Range fomula = excelsheet.Application.get_Range("G" + (currrow + 1));

                                fomula.FormulaLocal = "=(C" + (currrow + 1) + "*D" + (currrow + 1) + ")-(C" + (currrow + 1) + "*F" + (currrow + 1) + ")";

                                currrow++;
                                if (i == 0) firstrow = currrow;


                                ExcelDurumYaz("ÖZET Sayfa Aktarılıyor (Yurt Dışı) -->  " + (i).ToString() + " / " + yurtDisialanlar.Count.ToString());
                            }

                            //cellarray[currrow - 1, 7] = "USD Toplam";
                            //cellarray[currrow - 1, 8] = "=TOPLA(G" + (firstrow) + ":G" + (currrow) + ")";
                            excelsheet.Application.get_Range("H" + firstrow, "I" + currrow).Font.Bold = true;

                            excelsheet.Application.get_Range("H" + currrow).Value = "USD Toplam";
                            excelsheet.Application.get_Range("I" + currrow).FormulaLocal = "=TOPLA(G" + ortaRow + ":G" + currrow + ")";


                        }
                        excelsheet.Application.get_Range("A" + firstrow, "G" + currrow).Font.Bold = true;
                        Microsoft.Office.Interop.Excel.Range bodyrange = excelsheet.Application.get_Range("A2", "G" + currrow);
                        bodyrange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(228, 238, 243));
                        bodyrange.Font.Name = "Calibri";
                        bodyrange.Font.Size = 10;
                        bodyrange.Borders.Value = true;

                        ExcellApp.ScreenUpdating = false;
                        if (ExcellApp.Visible)
                        {
                            //Microsoft.Office.Interop.Excel.Range range = excelsheet.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                            //range.Value = cellarray;
                            Microsoft.Office.Interop.Excel.Range range = excelsheet.Cells.get_Resize(currrow, cellarray.GetLength(1));
                            var trimmedOzet = new object[currrow, cellarray.GetLength(1)];
                            Array.Copy(cellarray, trimmedOzet, currrow * cellarray.GetLength(1));
                            range.Value = trimmedOzet;
                            range.Columns.AutoFit();
                        }
                        ExcellApp.ScreenUpdating = true;


                        ExcelDurumYaz("ÖZET Sayfa Aktarımı Bitti");
                        #endregion

                        excelworkbook.Save();
                        excelworkbook.Close(false);

                        // Excel uygulamasını tamamen kapat
                        if (ExcelRutin.ExcellApp != null)
                        {
                            ExcelRutin.ExcellApp.Quit();
                            ExcelRutin.ExcellApp = null;
                        }

                        // Durum formunu kapat
                        System.Windows.Forms.Application.OpenForms.Cast<System.Windows.Forms.Form>()
                            .Where(f => f.Name == "formExcelDurum" || f.Text.Contains("AKTARIM DURUM"))
                            .ToList()
                            .ForEach(f => f.Invoke(new Action(() => f.Close())));

                    });
                }
                catch (Exception ex)
                {
                    MyTools.logyaz($"AutoBorsaTamListe metodunda hata: {ex.Message}");
                    MyTools.logyaz($"Stack Trace: {ex.StackTrace}"); // Hatanın yerini görmek için

                    MessageBox.Show($"Rapor oluşturulurken hata: {ex.Message}",
                                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            public static async Task AutoBorsaListeYeni()

            {
                try
                {
                    ExcelDurumFormAc();
                    await RunStaAsync(() =>

                    {
                        List<string> duzeltilenler = new List<string>();
                        var filename = System.Windows.Forms.Application.StartupPath + "\\" + "BorsaListeYeni" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".CSV";
                        SafeDeleteFile(filename);
                        var excelworkbook = GetExcelWorkbook(filename);

                        if (excelworkbook != null)

                        {
                            //  excelworkbook.Save();
                            excelworkbook.Close(false);
                        }

                        excelworkbook = OpenExcelWorkbook(filename);
                        if (excelworkbook == null) return;
                        crmDFNDataContext crm = new crmDFNDataContext();

                        ExcelDurumYaz("BorsaListeYeni Aktarım Başladı");

                        #region DETAY

                        ExcelDurumYaz("BIST_Rapor Sayfa Aktarımı Başladı");

                        var excelsheetDetay = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.Add();

                        excelsheetDetay.Name = "BIST Rapor";

                        var Sorgu = crm.Users.Where(x => x.LisansDurum.YayinDurumu == true && (x.StatusId == 1 || x.StatusId == 5));

                        excelsheetDetay.Activate();

                        musteridurum.durumdictionary.Clear();

                        musteridurum.ydurumdictionary.Clear();

                        var count = Sorgu.Count();

                        var say = 0;

                        foreach (var item in Sorgu)

                        {

                            musteridurum m = new musteridurum();
                            #region LISANLARIHESAPLA

                            try
                            {
                                //Pay
                                //if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == false && item.LisansDurum.PayL2 == false)
                                //{
                                //    //if (item.MusteriMenseiID == 2)
                                //    //{
                                //    //m.PD1 = 1;
                                //    //}
                                //    //else
                                //    //{
                                //    //    m.KRMD1 = 1;
                                //    //}
                                //    MessageBox.Show("PayL1 (PD1) lisansı artık kullanılmıyor. Lütfen kontrol ediniz" + "Username: " + item.UserName.ToString());
                                //}
                                /*  else */
                                if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == false)
                                {
                                    m.PD1P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayLPStart.Value.ToShortDateString();
                                }

                                else if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == true && item.LisansDurum.Pd2P == false)
                                {
                                    m.PD2 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayL2Start.Value.ToShortDateString();

                                }

                                else if (item.LisansDurum.PayL1 == true && item.LisansDurum.PayLP == true && item.LisansDurum.PayL2 == true && item.LisansDurum.Pd2P == true)

                                {
                                    m.PD2P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.Pd2PStart.Value.ToShortDateString();

                                }

                                if (item.LisansDurum.PayX == true)
                                {
                                    m.END = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayXStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.PITE == true)
                                {
                                    m.PITE = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayPiteStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.PayGS == true && item.LisansDurum.PITE == false)
                                {
                                    m.PIT = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.PayGSStart.Value.ToShortDateString();
                                }

                                //Viop

                                //if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == false && item.LisansDurum.ViopL2 == false && item.LisansDurum.Vd2P == false)

                                //{
                                //    //if (item.MusteriMenseiID == 2)
                                //    //{
                                //        //m.VL1 = 1;
                                //        //m.lisansYenilemeTarihi = item.LisansDurum.ViopL1Start.Value.ToShortDateString();
                                //    //}
                                //    //else
                                //    //{
                                //    //    m.KRMD1 = 1;
                                //    //}
                                //    MessageBox.Show("ViopL1(VL1) lisansı artık kullanılmıyor. lütfen kontrol ediniz" + "Username: " + item.UserName.ToString());

                                //}

                                /*else */
                                if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == false && item.LisansDurum.Vd2P == false)

                                {
                                    m.VL1P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.ViopLPStart.Value.ToShortDateString();
                                }

                                else if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == true && item.LisansDurum.Vd2P == false)

                                {
                                    m.VL2 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.ViopL2Start.Value.ToShortDateString();
                                }

                                else if (item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == true && item.LisansDurum.Vd2P == true)

                                {
                                    m.VL2P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.Vd2PStart.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.ViopGS == true)
                                {
                                    m.VIT = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.ViopGSStart.Value.ToShortDateString();
                                }




                                if (item.LisansDurum.COMEX == true)
                                {
                                    m.KRMD1 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.KRMD1Start.Value.ToShortDateString();
                                }

                                if (item.LisansDurum.MKK == true)
                                {
                                    m.MKK = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.MKKStart.Value.ToShortDateString();
                                }

                                //if (item.LisansDurum.TARAMA == true)
                                //{
                                //    m.TARAMA = 1;
                                //    m.lisansYenilemeTarihi = item.LisansDurum.TaramaStart.Value.ToShortDateString();
                                //}
                                if (item.LisansDurum.GKKUL == true)
                                {
                                    m.GKKUL = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.GKKULStart.Value.ToShortDateString();
                                }
                                //Tahvil
                                if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == false && item.LisansDurum.TahvilL2 == false)

                                {
                                    m.BD1 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.TahvilL1Start.Value.ToShortDateString();
                                }

                                else if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == true && item.LisansDurum.TahvilL2 == false)
                                {
                                    m.BD1P = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.TahvilLPStart.Value.ToShortDateString();
                                }

                                else if (item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == true && item.LisansDurum.TahvilL2 == true)

                                {
                                    m.BD2 = 1;
                                    m.lisansYenilemeTarihi = item.LisansDurum.TahvilL2Start.Value.ToShortDateString();
                                }
                            }
                            catch (Exception EX)
                            {
                                MyTools.logyaz($"AutoBorsaYeni Lisans hatası - User ID: {item.UserID}, TCKNO: {item.tckno}, Hata: {EX.Message}");
                                MessageBox.Show($"AutoBorsaYeni Lisans hatası: {EX.Message}");
                                continue;
                            }
                            #endregion

                            // var musteri = crm.Musterilers.FirstOrDefault(x => x.MusteriNo == item.PmtsNo);
                            var musteri = crm.Musterilers.FirstOrDefault(x => x.Tckno == item.PmtsNo);
                            m.musteriadi = (musteri != null) ? musteri.MusteriAdi : m.musteriadi = item.Name + " " + item.Surname;
                            if (item.Iletisim != null)
                            {
                                m.adres = (item.Iletisim.acikadres == null) ? "" : item.Iletisim.acikadres.Replace("\n", " ");
                                m.sehir = (item.Iletisim.Il == null) ? "" : item.Iletisim.Il.IlAdi;
                                if (item.Iletisim.Ulke == null) m.ulke = "";
                                else if (item.Iletisim.Ulke.UlkeAdi == "Türkiye") m.ulke = "TR";
                                else m.ulke = item.Iletisim.Ulke.UlkeAdi;

                                // 10158 info adres ve şehri müşteriler formundaki adres bilgisini yaz o da yoksa default istanbul olsun
                                if (musteri != null && musteri.Iletisim != null)
                                {
                                    if (musteri.Iletisim.Il == null && m.sehir == "")

                                    {
                                        m.sehir = "İstanbul";
                                        // duzeltilenler.Add("ülke değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                        duzeltilenler.Add("ülke değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                    }
                                    if (musteri.Iletisim.Il != null && m.sehir == "")
                                    {
                                        m.sehir = musteri.Iletisim.Il.IlAdi;
                                        //duzeltilenler.Add("şehir değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                        duzeltilenler.Add("şehir değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                    }
                                    if (musteri.Iletisim.Ulke == null && m.ulke == "")
                                    {
                                        m.ulke = "TR";
                                        // duzeltilenler.Add("ülke değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                        duzeltilenler.Add("ülke değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                    }
                                    if (musteri.Iletisim.Ulke != null && m.ulke == "")
                                    {
                                        m.ulke = musteri.Iletisim.Ulke.UlkeAdi;
                                        //duzeltilenler.Add("ülke değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                        duzeltilenler.Add("ülke değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                    }
                                    if (musteri.Iletisim.acikadres == null && m.adres == "")
                                    {
                                        m.adres = "İstanbul";
                                        // duzeltilenler.Add("adres değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                        duzeltilenler.Add("adres değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                    }
                                    if (musteri.Iletisim.acikadres != null && m.adres == "")
                                    {
                                        m.adres = musteri.Iletisim.acikadres.ToString();
                                        //  duzeltilenler.Add("adres değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                        duzeltilenler.Add("adres değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                    }
                                    if (m.adres == "" && m.sehir == "")
                                    {
                                        m.adres = "İstanbul";
                                        m.sehir = "İstanbul";
                                        // duzeltilenler.Add("adres ve sehir değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                        duzeltilenler.Add("adres ve sehir değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                    }
                                    else if (m.adres == "" && m.sehir != "")
                                    {
                                        m.adres = m.sehir;
                                        // duzeltilenler.Add("adres değişti" + " " + m.musteriadi + "" + musteri.MusteriNo);
                                        duzeltilenler.Add("adres değişti" + " " + m.musteriadi + "" + musteri.Tckno);
                                    }
                                }
                            }
                            if (item.BaslangicTarihi != null)

                                m.yetkilendirmetarihi = item.BaslangicTarihi.Value.ToShortDateString();

                            m.mensei = (item.MusteriMenseiID == 2) ? "YurtDışı" : m.mensei = "Yurtİçi";

                            m.Bedelsizaciklama = (item.StatusId == 1) ? "" : m.Bedelsizaciklama = "Bedelsiz";

                            if (item.MusteriMenseiID == 2)
                                // musteridurum.ekleY(item.UserName, m);
                                musteridurum.ekleY(item.tckno, m);
                            else
                                // musteridurum.ekle(item.UserName, m);
                                musteridurum.ekle(item.tckno, m);
                            say++;

                            ExcelDurumYaz("BIST Rapor Sayfa Aktarılıyor -->  " + (say).ToString() + " / " + count.ToString());

                        }
                        var alanlarlistesi = new List<alanlar>();
                        foreach (var mdurum in musteridurum.durumdictionary)
                        {
                            if (mdurum.Key == "10993")
                            {

                            }

                            #region PD1

                            if (mdurum.Value.PD1 > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpd1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpd1 * mdurum.Value.PD1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;
                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PD1P

                            if (mdurum.Value.PD1P > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD1P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD1P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpd1p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpd1p * mdurum.Value.PD1P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PD2

                            if (mdurum.Value.PD2 > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD2";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD2.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpd2.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpd2 * mdurum.Value.PD2).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PD2P

                            if (mdurum.Value.PD2P > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD2P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD2P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpd2p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpd2p * mdurum.Value.PD2P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region END

                            if (mdurum.Value.END > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "END";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.END.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fend.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fend * mdurum.Value.END).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PIT

                            if (mdurum.Value.PIT > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PIT";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PIT.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpit.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpit * mdurum.Value.PIT).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PITE

                            if (mdurum.Value.PITE > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PITE";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PITE.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fpite.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fpite * mdurum.Value.PITE).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL1

                            if (mdurum.Value.VL1 > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvl1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvl1 * mdurum.Value.VL1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL1P

                            if (mdurum.Value.VL1P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD1P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL1P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvl1p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvl1p * mdurum.Value.VL1P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL2

                            if (mdurum.Value.VL2 > 0)

                            {



                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD2";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL2.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvl2.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvl2 * mdurum.Value.VL2).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL2P

                            if (mdurum.Value.VL2P > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD2P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL2P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvl2p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvl2p * mdurum.Value.VL2P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VIT

                            if (mdurum.Value.VIT > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VIT";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VIT.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fvit.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fvit * mdurum.Value.VIT).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region KRMD1

                            if (mdurum.Value.KRMD1 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "KRMD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.KRMD1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = "0";

                                    alan.toplambedel = "0";

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }



                            #endregion

                            #region BD1

                            if (mdurum.Value.BD1 > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "BD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.BD1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fbd1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fbd1 * mdurum.Value.BD1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region BD1P

                            if (mdurum.Value.BD1P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "BD1P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.BD1P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fbd1p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fbd1p * mdurum.Value.BD1P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region BD2

                            if (mdurum.Value.BD2 > 0)

                            {

                                if (mdurum.Key == "10993")

                                {

                                }

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "BD2";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.BD2.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Fbd2.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Fbd2 * mdurum.Value.BD2).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region MKK

                            if (mdurum.Value.MKK > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "MKK";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.MKK.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = "0";

                                    alan.toplambedel = "0";

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }



                            #endregion

                            //#region TARAMA

                            //if (mdurum.Value.TARAMA > 0)

                            //{

                            //    var alan = new alanlar();

                            //    alan.aboneno = mdurum.Key;

                            //    alan.aboneadi = mdurum.Value.musteriadi;

                            //    alan.adres = mdurum.Value.adres;

                            //    alan.sehir = mdurum.Value.sehir;

                            //    alan.ulke = mdurum.Value.ulke;

                            //    alan.bilgikodu = "TARAMA";

                            //    alan.mensei = mdurum.Value.mensei;

                            //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                            //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                            //    alan.cihaztoplam = mdurum.Value.TARAMA.ToString();

                            //    if (mdurum.Value.Bedelsizaciklama == "")

                            //    {

                            //        alan.bilgibedel = "0";

                            //        alan.toplambedel = "0";

                            //    }

                            //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                            //    alanlarlistesi.Add(alan);

                            //}



                            //#endregion

                            #region GKKUL

                            if (mdurum.Value.GKKUL > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "GKKUL";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.GKKUL.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = "0";

                                    alan.toplambedel = "0";

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;
                                alanlarlistesi.Add(alan);

                            }
                            #endregion


                        }





                        #region yurdisilar

                        foreach (var mdurum in musteridurum.ydurumdictionary)
                        {
                            #region PD1

                            if (mdurum.Value.PD1 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypd1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypd1 * mdurum.Value.PD1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;
                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PD1P

                            if (mdurum.Value.PD1P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD1P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD1P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypd1p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypd1p * mdurum.Value.PD1P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;
                                alanlarlistesi.Add(alan);

                            }
                            #endregion

                            #region PD2

                            if (mdurum.Value.PD2 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD2";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD2.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypd2.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypd2 * mdurum.Value.PD2).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PD2P

                            if (mdurum.Value.PD2P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PD2P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PD2P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypd2p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypd2p * mdurum.Value.PD2P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region END

                            if (mdurum.Value.END > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "END";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.END.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yend.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yend * mdurum.Value.END).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PIT

                            if (mdurum.Value.PIT > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PIT";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PIT.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypit.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypit * mdurum.Value.PIT).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;
                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region PITE

                            if (mdurum.Value.PITE > 0)

                            {
                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "PITE";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.PITE.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ypite.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ypite * mdurum.Value.PITE).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;
                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL1

                            if (mdurum.Value.VL1 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvl1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvl1 * mdurum.Value.VL1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL1p

                            if (mdurum.Value.VL1P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD1P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL1P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvl1p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvl1p * mdurum.Value.VL1P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL2

                            if (mdurum.Value.VL2 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD2";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL2.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvl2.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvl2 * mdurum.Value.VL2).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VL2P

                            if (mdurum.Value.VL2P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VD2P";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VL2P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvl2p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvl2p * mdurum.Value.VL2P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region VIT

                            if (mdurum.Value.VIT > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "VIT";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.VIT.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Yvit.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Yvit * mdurum.Value.VIT).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region KRMD1

                            if (mdurum.Value.KRMD1 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "KRMD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.KRMD1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ykrmd1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ykrmd1 * mdurum.Value.KRMD1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region BD1



                            if (mdurum.Value.BD1 > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "BD1";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.BD1.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ybd1.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ybd1 * mdurum.Value.BD1).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region BD1P

                            if (mdurum.Value.BD1P > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "BD1P";

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.mensei = mdurum.Value.mensei;

                                alan.cihaztoplam = mdurum.Value.BD1P.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ybd1p.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ybd1p * mdurum.Value.BD1P).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region BD2

                            if (mdurum.Value.BD2 > 0)

                            {

                                if (mdurum.Key == "10993")

                                {



                                }

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "BD2";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.BD2.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ybd2.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ybd2 * mdurum.Value.BD2).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;
                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            #region MKK

                            if (mdurum.Value.MKK > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "MKK";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.MKK.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ymkk.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ymkk * mdurum.Value.MKK).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion

                            //#region TARAMA

                            //if (mdurum.Value.TARAMA > 0)

                            //{

                            //    var alan = new alanlar();

                            //    alan.aboneno = mdurum.Key;

                            //    alan.aboneadi = mdurum.Value.musteriadi;

                            //    alan.adres = mdurum.Value.adres;

                            //    alan.sehir = mdurum.Value.sehir;

                            //    alan.ulke = mdurum.Value.ulke;

                            //    alan.bilgikodu = "TARAMA";

                            //    alan.mensei = mdurum.Value.mensei;

                            //    alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                            //    alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                            //    alan.cihaztoplam = mdurum.Value.TARAMA.ToString();

                            //    if (mdurum.Value.Bedelsizaciklama == "")

                            //    {

                            //        alan.bilgibedel = MyTools.lisansfiyatlari.Ytarama.ToString("0.00");

                            //        alan.toplambedel = (MyTools.lisansfiyatlari.Ytarama * mdurum.Value.TARAMA).ToString("0.00");

                            //    }

                            //    alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                            //    alanlarlistesi.Add(alan);

                            //}

                            //#endregion

                            #region GKKUL

                            if (mdurum.Value.GKKUL > 0)

                            {

                                var alan = new alanlar();

                                alan.aboneno = mdurum.Key;

                                alan.aboneadi = mdurum.Value.musteriadi;

                                alan.adres = mdurum.Value.adres;

                                alan.sehir = mdurum.Value.sehir;

                                alan.ulke = mdurum.Value.ulke;

                                alan.bilgikodu = "GKKUL";

                                alan.mensei = mdurum.Value.mensei;

                                alan.yetkikendirmetar = mdurum.Value.yetkilendirmetarihi;

                                alan.lisansYenilemeTarihi = mdurum.Value.lisansYenilemeTarihi;

                                alan.cihaztoplam = mdurum.Value.GKKUL.ToString();

                                if (mdurum.Value.Bedelsizaciklama == "")

                                {

                                    alan.bilgibedel = MyTools.lisansfiyatlari.Ygkkul.ToString("0.00");

                                    alan.toplambedel = (MyTools.lisansfiyatlari.Ygkkul * mdurum.Value.GKKUL).ToString("0.00");

                                }

                                alan.bedelsizaciklama = mdurum.Value.Bedelsizaciklama;



                                alanlarlistesi.Add(alan);

                            }

                            #endregion
                        }

                        #endregion

                        var cellarray = new object[alanlarlistesi.Count + 10, 11];

                        //for (int i = 0; i < cellarray.GetLength(0); i++)

                        //{

                        //    for (int j = 0; j < cellarray.GetLength(1); j++)

                        //        cellarray[i, j] = "";

                        //}
                        var currrow = 0;
                        var date = DateTime.Now;
                        cellarray[currrow, 0] = "Dağıtıcı Adı:";

                        cellarray[currrow, 1] = "İdeal Data Finansal Teknolojiler A.Ş.";

                        currrow++;

                        cellarray[currrow, 0] = "Borsa";

                        cellarray[currrow, 1] = "Borsa İstanbul A.Ş.";

                        currrow++;

                        cellarray[currrow, 0] = "Raporlanan Ay";

                        cellarray[currrow, 1] = date.ToString("MMM.").Trim() + date.ToString("yy");

                        currrow++;

                        cellarray[currrow, 0] = "Rapor Tipi";

                        cellarray[currrow, 1] = "Detay";

                        currrow++;

                        cellarray[currrow, 0] = "";

                        currrow++;

                        cellarray[currrow, 0] = "Abone Kodu";

                        cellarray[currrow, 1] = "Abone Adı Soyadı";

                        cellarray[currrow, 2] = "Adres";

                        cellarray[currrow, 3] = "Şehir";

                        cellarray[currrow, 4] = "Ülke";

                        cellarray[currrow, 5] = "Bilgi Raporlama Kodu";

                        cellarray[currrow, 6] = "Erişim Tipi";

                        cellarray[currrow, 7] = "Toplam Kullanıcı/Cihaz Adedi";

                        cellarray[currrow, 8] = "Yetkilendirme Tarihi";

                        cellarray[currrow, 9] = "Lisans Yenileme Tarihi";

                        cellarray[currrow, 10] = "Bedelsiz Açıklama";

                        currrow++;

                        SafeFreezePanes(excelsheetDetay, 6);

                        for (int i = 0; i < alanlarlistesi.Count; i++)

                        {

                            if (alanlarlistesi[i].aboneno == "10993")

                            {

                            }

                            cellarray[currrow, 0] = alanlarlistesi[i].aboneno;

                            cellarray[currrow, 1] = alanlarlistesi[i].aboneadi;

                            cellarray[currrow, 2] = alanlarlistesi[i].adres;

                            cellarray[currrow, 3] = alanlarlistesi[i].sehir;

                            cellarray[currrow, 4] = alanlarlistesi[i].ulke;

                            cellarray[currrow, 5] = alanlarlistesi[i].bilgikodu; ;

                            cellarray[currrow, 6] = alanlarlistesi[i].erisimtipi;

                            cellarray[currrow, 7] = alanlarlistesi[i].cihaztoplam;

                            cellarray[currrow, 8] = alanlarlistesi[i].yetkikendirmetar;

                            cellarray[currrow, 9] = alanlarlistesi[i].lisansYenilemeTarihi;

                            cellarray[currrow, 10] = alanlarlistesi[i].bedelsizaciklama;

                            currrow++;

                        }
                        //Microsoft.Office.Interop.Excel.Range range2 = excelsheetDetay.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));

                        //range2.Value = cellarray;
                        Microsoft.Office.Interop.Excel.Range range2 = excelsheetDetay.Cells.get_Resize(currrow, cellarray.GetLength(1));
                        var trimmedCsv = new object[currrow, cellarray.GetLength(1)];
                        Array.Copy(cellarray, trimmedCsv, currrow * cellarray.GetLength(1));
                        range2.Value = trimmedCsv;
                        range2.Columns.AutoFit();

                        ExcelDurumYaz("DETAY Sayfa Aktarımı Bitti");

                        string strDuzeltilenler = string.Join("-", duzeltilenler);

                        var dosya_yolu = System.Windows.Forms.Application.StartupPath + "\\" + "BorsaListeYeniDüzeltilenler" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".txt";

                        if (File.Exists(dosya_yolu)) File.Delete(dosya_yolu);

                        FileStream fs = new FileStream(dosya_yolu, FileMode.OpenOrCreate, FileAccess.Write);

                        StreamWriter sw = new StreamWriter(fs);

                        foreach (var duz in duzeltilenler)

                        {

                            sw.WriteLine(duz);

                        }

                        sw.Flush();

                        sw.Close();

                        fs.Close();

                        #endregion

                        excelworkbook.Save();

                    });
                }
                catch (Exception ex)
                {
                    MyTools.logyaz($"AutoBorsaListeYeni metodunda hata: {ex.Message}");
                    MyTools.logyaz($"Stack Trace: {ex.StackTrace}"); // Hatanın yerini görmek için

                    MessageBox.Show($"Rapor oluşturulurken hata: {ex.Message}",
                                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                //T.Start();

            }
            private class musteridurum
            {
                public int PD1 = 0;
                public int PD1P = 0;
                public int PD2 = 0;
                public int PD2P = 0;
                public int END = 0;
                public int PIT = 0;
                public int PITE = 0;
                public int VL1 = 0;
                public int VL1P = 0;
                public int VL2 = 0;
                public int VL2P = 0;
                public int VIT = 0;
                public int BD1 = 0;
                public int BD1P = 0;
                public int BD2 = 0;
                public int ANPRO = 0;
                public int KRMD1 = 0;
                public int SPI = 0;
                public int MKK = 0;
                public int TARAMA = 0;
                public int GKKUL = 0;
                public int cihazadet = 1;
                public string mensei = "";
                public string ulke = "";
                public string sehir = "";
                public string adres = "";
                public string Bedelsizaciklama = "";
                public string musteriadi = "";
                public string yetkilendirmetarihi = "";
                public string lisansYenilemeTarihi = "";
                public string lisansSonlanmaTarihi = "";

                public static Dictionary<string, musteridurum> durumdictionary = new Dictionary<string, musteridurum>();
                public static Dictionary<string, musteridurum> ydurumdictionary = new Dictionary<string, musteridurum>();
                public static void ekle(string tckno, musteridurum durum)
                {
                    tckno = tckno?.Trim();
                    if (string.IsNullOrEmpty(tckno))
                    {
                        MyTools.logyaz("musteridurum.ekle: null/boş tckno ile kayıt atlandı. Musteri: " + durum?.musteriadi);
                        return;
                    }

                    if (durumdictionary.ContainsKey(tckno))
                    {

                        durumdictionary[tckno].PD1 += durum.PD1;
                        durumdictionary[tckno].PD1P += durum.PD1P;
                        durumdictionary[tckno].PD2 += durum.PD2;
                        durumdictionary[tckno].PD2P += durum.PD2P;
                        durumdictionary[tckno].END += durum.END;
                        durumdictionary[tckno].PIT += durum.PIT;
                        durumdictionary[tckno].PITE += durum.PITE;
                        durumdictionary[tckno].VL1 += durum.VL1;
                        durumdictionary[tckno].VL1P += durum.VL1P;
                        durumdictionary[tckno].VL2 += durum.VL2;
                        durumdictionary[tckno].VL2P += durum.VL2P;
                        durumdictionary[tckno].VIT += durum.VIT;
                        durumdictionary[tckno].BD1 += durum.BD1;
                        durumdictionary[tckno].BD1P += durum.BD1P;
                        durumdictionary[tckno].BD2 += durum.BD2;
                        durumdictionary[tckno].ANPRO += durum.ANPRO;
                        durumdictionary[tckno].KRMD1 += durum.KRMD1;
                        durumdictionary[tckno].MKK += durum.MKK;
                        durumdictionary[tckno].TARAMA += durum.TARAMA;
                        durumdictionary[tckno].GKKUL += durum.GKKUL;
                        durumdictionary[tckno].cihazadet += 1;
                        durumdictionary[tckno].Bedelsizaciklama = durum.Bedelsizaciklama;
                        durumdictionary[tckno].musteriadi = durum.musteriadi;

                    }
                    else
                    {

                        durumdictionary.Add(tckno, durum);


                    }

                }
                public static void ekleY(string pmtsno, musteridurum durum)
                {
                    pmtsno = pmtsno?.Trim();
                    if (string.IsNullOrEmpty(pmtsno))
                    {
                        MyTools.logyaz("musteridurum.ekleY: null/boş pmtsno ile kayıt atlandı. Musteri: " + durum?.musteriadi);
                        return;
                    }

                    if (ydurumdictionary.ContainsKey(pmtsno))
                    {
                        ydurumdictionary[pmtsno].PD1 += durum.PD1;
                        ydurumdictionary[pmtsno].PD1P += durum.PD1P;
                        ydurumdictionary[pmtsno].PD2 += durum.PD2;
                        ydurumdictionary[pmtsno].PD2P += durum.PD2P;
                        ydurumdictionary[pmtsno].END += durum.END;
                        ydurumdictionary[pmtsno].PIT += durum.PIT;
                        ydurumdictionary[pmtsno].PITE += durum.PITE;
                        ydurumdictionary[pmtsno].VL1 += durum.VL1;
                        ydurumdictionary[pmtsno].VL1P += durum.VL1P;
                        ydurumdictionary[pmtsno].VL2 += durum.VL2;
                        ydurumdictionary[pmtsno].VL2P += durum.VL2P;
                        // ydurumdictionary[pmtsno].VIT += durum.PIT;
                        ydurumdictionary[pmtsno].VIT += durum.VIT;
                        ydurumdictionary[pmtsno].BD1 += durum.BD1;
                        ydurumdictionary[pmtsno].BD1P += durum.BD1P;
                        ydurumdictionary[pmtsno].BD2 += durum.BD2;
                        ydurumdictionary[pmtsno].ANPRO += durum.ANPRO;
                        ydurumdictionary[pmtsno].KRMD1 += durum.KRMD1;
                        ydurumdictionary[pmtsno].MKK += durum.MKK;
                        ydurumdictionary[pmtsno].TARAMA += durum.TARAMA;
                        ydurumdictionary[pmtsno].GKKUL += durum.GKKUL;
                        ydurumdictionary[pmtsno].cihazadet += 1;
                        ydurumdictionary[pmtsno].Bedelsizaciklama = durum.Bedelsizaciklama;
                        ydurumdictionary[pmtsno].musteriadi = durum.musteriadi;

                    }
                    else
                    {
                        ydurumdictionary.Add(pmtsno, durum);
                    }
                }
            }

            public class RaporlamaKodu
            {
                public string EskiKod = "";
                public string YeniKod = "";
                public string Mensei = "";
            }
            public class UserRecordKurum
            {
                public int id = 0;
                public string UserName = "";
                public string tckno = "";
                public string AdSoyad = "";
                public string Ad = "";
                public string Soyad = "";
                public string sube = "";
                public string KurumMusteriNo = "";
                public string FXkurum = "";
                public bool Pro = false;
                public bool Cep = false;
                public decimal Mobile = 0;
                public decimal Desktop = 0;
                public string YayinDurumu = "";
                public string BasliangicTarihi = "";
                public string BitisTarihi = "";
                public string MTID = "";
                public string Aciklama = "";
                public string KullaniciTip = "";
                public bool Proje = false;
                public string pmts = "";
                #region sayilar
                public decimal sSentiL1 = 0;
                public decimal sSentiL2 = 0;
                public decimal sPD1 = 0;
                public decimal sPD1P = 0;
                public decimal sPD2 = 0;
                public decimal sPD2P = 0;
                public decimal sEND = 0;
                public decimal sPIT = 0;
                public decimal sPITE = 0;
                public decimal sPVA = 0;
                public decimal sVD1 = 0;
                public decimal sVD1P = 0;
                public decimal sVD2 = 0;
                public decimal sVD2P = 0;
                public decimal sVIT = 0;
                public decimal sBD1 = 0;
                public decimal sBD1P = 0;
                public decimal sBD2 = 0;
                public decimal sANPRO = 0;
                public decimal sDJI = 0;
                public decimal sSPI = 0;
                public decimal sXETRA = 0;
                public decimal sSPOT = 0;
                public decimal sEUREX = 0;
                public decimal sCBOTM = 0;
                public decimal sCBOT = 0;
                public decimal sCME = 0;
                public decimal sCMEM = 0;
                public decimal sTemelAnaliz = 0;
                public decimal sidealGO = 0;
                public decimal sUserDll = 0;
                public decimal sBMK = 0;
                public decimal sBarSistem = 0;
                public decimal sFSystem = 0;
                public decimal sKRMD1 = 0;
                public int sDesktop = 0;
                public int sMobile = 0;
                public int sMKK = 0;
                public int sTARAMA = 0;
                public int sGKKUL = 0;
                #endregion

                #region Ucret
                public decimal SentiL1 = 0;
                public decimal SentiL2 = 0;
                public decimal PD1 = 0;
                public decimal PD1P = 0;
                public decimal PD2 = 0;
                public decimal PD2P = 0;
                public decimal END = 0;
                public decimal PIT = 0;
                public decimal PITE = 0;
                public decimal PVA = 0;
                public decimal KRMD1 = 0;
                public decimal YDS = 0;
                public decimal VD1 = 0;
                public decimal VD1P = 0;
                public decimal VD2 = 0;
                public decimal VD2P = 0;
                public decimal VIT = 0;
                public decimal BD1 = 0;
                public decimal BD1P = 0;
                public decimal BD2 = 0;
                public decimal ANPRO = 0;
                public decimal DJI = 0;
                public decimal SPI = 0;
                public decimal XETRA = 0;
                public decimal SPOT = 0;
                public decimal EUREX = 0;
                public decimal CBOTM = 0;
                public decimal CBOT = 0;
                public decimal CME = 0;
                public decimal CMEM = 0;
                public decimal TemelAnaliz = 0;
                public decimal idealGO = 0;
                public decimal UserDll = 0;
                public decimal BMK = 0;
                public decimal BarSistem = 0;
                public decimal FSystem = 0;
                public decimal EkranUcreti = 0;
                public decimal MKK = 0;
                public decimal TARAMA = 0;
                public decimal GKKUL = 0;

                #endregion
                public decimal BistPay = 0;
                public decimal ToplamFiyat = 0;
                public decimal KDV = 0;
                public decimal KDVDahilToplam = 0;

                public void Toplam()
                {
                    this.BistPay = KRMD1 + PD1 + PD1P + PD2 + PD2P + END + PIT + PITE + VD1 + VD1P + VD2 + VD2P + VIT + BD1 + BD1P + BD2 + MKK + GKKUL;
                    this.ToplamFiyat = EkranUcreti + KRMD1 + PD1 + PD1P + PD2 + PD2P + END + PIT + PITE + VD1 + VD1P + VD2 + VD2P + VIT + BD1 + BD1P + BD2 + ANPRO + idealGO + TemelAnaliz + UserDll + BMK + BarSistem + FSystem + MKK + TARAMA;
                    this.KDV = this.ToplamFiyat * 0.20M;
                    this.KDVDahilToplam = this.ToplamFiyat + this.KDV;
                }

                public void ToplamProYoksaLisnansHesapla()
                {
                    this.BistPay = (Pro == false && Cep == true) ? KRMD1 + PD1 + PD1P + PD2 + PD2P + END + PIT + PITE + VD1 + VD1P + VD2 + VD2P + VIT + BD1 + BD1P + BD2 + MKK + TARAMA + GKKUL : 0;
                    this.ToplamFiyat = EkranUcreti + this.BistPay + idealGO + ANPRO + TemelAnaliz + UserDll + BMK + BarSistem + FSystem;
                    this.KDV = this.ToplamFiyat * 0.18M;
                    this.KDVDahilToplam = this.ToplamFiyat + this.KDV;
                }

                public void ToplamMobiPro(int tip)
                {

                    this.BistPay = KRMD1 + PD1 + PD1P + PD2 + PD2P + END + PIT + PITE + VD1 + VD1P + VD2 + VD2P + VIT + BD1 + BD1P + BD2 + MKK + GKKUL;
                    if (tip == 1)
                        this.ToplamFiyat = Desktop + KRMD1 + PD1 + PD1P + PD2 + PD2P + END + PIT + PITE + VD1 + VD1P + VD2 + VD2P + VIT + BD1 + BD1P + BD2 + ANPRO + idealGO + TemelAnaliz + UserDll + BMK + BarSistem + FSystem + MKK + TARAMA + GKKUL;
                    else
                        this.ToplamFiyat = Mobile + KRMD1 + PD1 + PD1P + PD2 + PD2P + END + PIT + PITE + VD1 + VD1P + VD2 + VD2P + VIT + BD1 + BD1P + BD2 + ANPRO + idealGO + TemelAnaliz + UserDll + BMK + BarSistem + FSystem + MKK + TARAMA + GKKUL;

                    this.KDV = this.ToplamFiyat * 0.20M;
                    this.KDVDahilToplam = this.ToplamFiyat + this.KDV;
                }

            }
            public class UserRecordToplam
            {
                public decimal DeskTop = 0;
                public decimal Mobile = 0;
                public decimal KRMD1 = 0;
                public decimal PD1 = 0;
                public decimal PD1P = 0;
                public decimal PD2 = 0;
                public decimal PD2P = 0;
                public decimal END = 0;
                public decimal PIT = 0;
                public decimal PITE = 0;
                public decimal PVA = 0;
                public decimal VD1 = 0;
                public decimal VD1P = 0;
                public decimal VD2 = 0;
                public decimal VD2P = 0;
                public decimal VIT = 0;
                public decimal BD1 = 0;
                public decimal BD1P = 0;
                public decimal BD2 = 0;
                public decimal ANPRO = 0;
                public decimal DJI = 0;
                public decimal SPI = 0;
                public decimal XETRA = 0;
                public decimal SPOT = 0;
                public decimal EUREX = 0;
                public decimal CBOTM = 0;
                public decimal CBOT = 0;
                public decimal CME = 0;
                public decimal CMEM = 0;
                public decimal BistPay = 0;
                public decimal MKK = 0;
                public decimal TARAMA = 0;
                public decimal GKKUL = 0;
                public decimal ToplamFiyat = 0;
                public decimal KDVDahilToplam = 0;
                public decimal BistToplam = 0;
                public decimal YurtdisiToplam = 0;
                public static UserRecordToplam UserToplamlar(List<UserRecordKurum> list)
                {
                    UserRecordToplam ust = new UserRecordToplam();

                    ust.KRMD1 = list.Sum(x => x.KRMD1);
                    ust.MKK = list.Sum(x => x.MKK);
                    ust.PD1 = list.Sum(x => x.PD1);
                    ust.PD1P = list.Sum(x => x.PD1P);
                    ust.PD2 = list.Sum(x => x.PD2);
                    ust.PD2P = list.Sum(x => x.PD2P);
                    ust.PIT = list.Sum(x => x.PIT);
                    ust.PITE = list.Sum(x => x.PITE);
                    ust.END = list.Sum(x => x.END);
                    ust.VD1 = list.Sum(x => x.VD1);
                    ust.VD1P = list.Sum(x => x.VD1P);
                    ust.VD2 = list.Sum(x => x.VD2);
                    ust.VD2P = list.Sum(x => x.VD2P);
                    ust.VIT = list.Sum(x => x.VIT);
                    ust.TARAMA = list.Sum(x => x.TARAMA);
                    ust.GKKUL = list.Sum(x => x.GKKUL);
                    ust.BD1 = list.Sum(x => x.BD1);
                    ust.BD1P = list.Sum(x => x.BD1P);
                    ust.BD2 = list.Sum(x => x.BD2);
                    ust.ANPRO = list.Sum(x => x.ANPRO);
                    ust.DJI = list.Sum(x => x.DJI);
                    ust.SPI = list.Sum(x => x.SPI);
                    ust.XETRA = list.Sum(x => x.XETRA);
                    ust.SPOT = list.Sum(x => x.SPOT);
                    ust.CBOT = list.Sum(x => x.CBOT);
                    ust.CBOTM = list.Sum(x => x.CBOTM);
                    ust.CME = list.Sum(x => x.CME);
                    ust.CMEM = list.Sum(x => x.CMEM);
                    ust.EUREX = list.Sum(x => x.EUREX);
                    ust.DeskTop = list.Sum(x => x.Desktop);
                    ust.Mobile = list.Sum(x => x.Mobile);

                    ust.BistToplam = ust.KRMD1 + ust.PD1 + ust.PD1P + ust.PD2 + ust.PD2P + ust.PIT + ust.PITE + ust.END + ust.VD1 + ust.VD1P + ust.VD2 + ust.VD2P + ust.VIT + ust.BD1 + ust.BD1P + ust.BD2 + ust.MKK + ust.TARAMA + ust.GKKUL;
                    ust.YurtdisiToplam = ust.DJI + ust.SPI + ust.XETRA + ust.SPOT + ust.CBOT + ust.CBOTM + ust.EUREX + ust.CME + ust.CMEM;
                    ust.ToplamFiyat = ust.BistToplam + ust.YurtdisiToplam;
                    return ust;


                }

            }
            public class UserRecordToplam2
            {
                #region Ucretler
                public decimal KRMD1 = 0;
                public decimal PD1 = 0;
                public decimal PD1P = 0;
                public decimal PD2 = 0;
                public decimal PD2P = 0;
                public decimal END = 0;
                public decimal PIT = 0;
                public decimal PITE = 0;
                public decimal PVA = 0;
                public decimal VD1 = 0;
                public decimal VD1P = 0;
                public decimal VD2 = 0;
                public decimal VD2P = 0;
                public decimal VIT = 0;
                public decimal BD1 = 0;
                public decimal BD1P = 0;
                public decimal BD2 = 0;
                public decimal ANPRO = 0;
                public decimal DJI = 0;
                public decimal SPI = 0;
                public decimal XETRA = 0;
                public decimal SPOT = 0;
                public decimal EUREX = 0;
                public decimal CBOTM = 0;
                public decimal CBOT = 0;
                public decimal CME = 0;
                public decimal CMEM = 0;
                public decimal TemelAnaliz = 0;
                public decimal idealGO = 0;
                public decimal UserDll = 0;
                public decimal BMK = 0;
                public decimal BarSistem = 0;
                public decimal FSystem = 0;
                public decimal EkranUcreti = 0;
                public decimal Desktop = 0;
                public decimal Mobile = 0;
                public decimal SENTIL1 = 0;
                public decimal SENTIL2 = 0;
                public decimal MKK = 0;
                public decimal TARAMA = 0;
                public decimal GKKUL = 0;
                #endregion

                #region sayilar
                public decimal sKRMD1 = 0;
                public decimal sPD1 = 0;
                public decimal sPD1P = 0;
                public decimal sPD2 = 0;
                public decimal sPD2P = 0;
                public decimal sEND = 0;
                public decimal sPIT = 0;
                public decimal sPITE = 0;
                public decimal sPVA = 0;
                public decimal sVD1 = 0;
                public decimal sVD1P = 0;
                public decimal sVD2 = 0;
                public decimal sVD2P = 0;
                public decimal sVIT = 0;
                public decimal sBD1 = 0;
                public decimal sBD1P = 0;
                public decimal sBD2 = 0;
                public decimal sANPRO = 0;
                public decimal sDJI = 0;
                public decimal sSPI = 0;
                public decimal sXETRA = 0;
                public decimal sSPOT = 0;
                public decimal sEUREX = 0;
                public decimal sCBOTM = 0;
                public decimal sCBOT = 0;
                public decimal sCME = 0;
                public decimal sCMEM = 0;
                public decimal sTemelAnaliz = 0;
                public decimal sidealGO = 0;
                public decimal sUserDll = 0;
                public decimal sBMK = 0;
                public decimal sBarSistem = 0;
                public decimal sFSystem = 0;
                public decimal sSENTIL1 = 0;
                public decimal sSENTIL2 = 0;
                public int EkranSayisi = 0;
                public int sDesktop = 0;
                public int sMobile = 0;
                public int sMKK = 0;
                public int sTARAMA = 0;
                public int sGKKUL = 0;



                #endregion

                public decimal ToplamFiyat = 0;
                public decimal ToplamKDV = 0;
                public decimal KDVDahilToplam = 0;
                public decimal BistToplam = 0;
                public decimal YurtdisiToplam = 0;

                public static UserRecordToplam2 operator +(UserRecordToplam2 a, UserRecordToplam2 b) => new UserRecordToplam2()
                {



                    sKRMD1 = a.sKRMD1 + b.sKRMD1,
                    sMKK = a.sMKK + b.sMKK,
                    sPD1 = a.sPD1 + b.sPD1,
                    sPD1P = a.sPD1 + b.sPD1P,
                    sPD2 = a.sPD2 + b.sPD2,
                    sPD2P = a.sPD2P + b.sPD2P,
                    sEND = a.sEND + b.sEND,
                    sPIT = a.sPIT + b.sPIT,
                    sPITE = a.sPITE + b.sPITE,
                    sVD1 = a.sVD1 + b.sVD1,
                    sVD1P = a.sVD1P + b.sVD1P,
                    sVD2 = a.sVD2 + b.sVD2,
                    sVD2P = a.sVD2P + b.sVD2P,
                    sVIT = a.sVIT + b.sVIT,
                    sTARAMA = a.sTARAMA + b.sTARAMA,
                    sGKKUL = a.sGKKUL + b.sGKKUL,
                    sBD1 = a.sBD1 + b.sBD1,
                    sBD1P = a.sBD1P + b.sBD1P,
                    sBD2 = a.sBD2 + b.sBD2,
                    sANPRO = a.sANPRO + b.sANPRO,
                    sTemelAnaliz = a.sTemelAnaliz + b.sTemelAnaliz,
                    sidealGO = a.sidealGO + b.sidealGO,
                    sUserDll = a.sUserDll + b.sUserDll,
                    sBMK = a.sBMK + b.sBMK,
                    sBarSistem = a.sBarSistem + b.sBarSistem,
                    sFSystem = a.sFSystem + b.sFSystem,
                    EkranSayisi = a.EkranSayisi + b.EkranSayisi,
                    sDesktop = a.sDesktop + b.sDesktop,
                    sMobile = a.sMobile + b.sMobile,
                    KRMD1 = a.KRMD1 + b.KRMD1,
                    MKK = a.MKK + b.MKK,
                    TARAMA = a.TARAMA + b.TARAMA,
                    GKKUL = a.GKKUL + b.GKKUL,
                    PD1 = a.PD1 + b.PD1,
                    PD1P = a.PD1 + b.PD1P,
                    PD2 = a.PD2 + b.PD2,
                    PD2P = a.PD2P + b.PD2P,
                    END = a.END + b.END,
                    PIT = a.PIT + b.PIT,
                    PITE = a.PITE + b.PITE,
                    VD1 = a.VD1 + b.VD1,
                    VD1P = a.VD1P + b.VD1P,
                    VD2 = a.VD2 + b.VD2,
                    VD2P = a.VD2P + b.VD2P,
                    VIT = a.VIT + b.VIT,
                    BD1 = a.BD1 + b.BD1,
                    BD1P = a.BD1P + b.BD1P,
                    BD2 = a.BD2 + b.BD2,
                    ANPRO = a.ANPRO + b.ANPRO,
                    TemelAnaliz = a.TemelAnaliz + b.TemelAnaliz,
                    idealGO = a.idealGO + b.idealGO,
                    UserDll = a.UserDll + b.UserDll,
                    BMK = a.BMK + b.BMK,
                    BarSistem = a.BarSistem + b.BarSistem,
                    FSystem = a.FSystem + b.FSystem,
                    Desktop = a.Desktop + b.Desktop,
                    Mobile = a.Mobile + b.Mobile,
                    BistToplam = a.BistToplam + b.BistToplam,
                    ToplamFiyat = a.ToplamFiyat + b.ToplamFiyat,
                    ToplamKDV = a.ToplamKDV + b.ToplamKDV,
                    KDVDahilToplam = a.KDVDahilToplam + b.KDVDahilToplam,
                    EkranUcreti = a.EkranUcreti + b.EkranUcreti


                };

                public static UserRecordToplam2 UserToplamlar(List<UserRecordKurum> list)
                {
                    UserRecordToplam2 ust = new UserRecordToplam2();

                    #region Sayilar
                    ust.EkranSayisi = list.Where(x => x.Proje == false).ToList().Count;
                    ust.sKRMD1 = list.Sum(x => x.sKRMD1);
                    ust.sMKK = list.Sum(x => x.sMKK);
                    ust.sPD1 = list.Sum(x => x.sPD1);
                    ust.sPD1P = list.Sum(x => x.sPD1P);
                    ust.sPD2 = list.Sum(x => x.sPD2);
                    ust.sPD2P = list.Sum(x => x.sPD2P);
                    ust.sSENTIL1 = list.Sum(x => x.sSentiL1);
                    ust.sSENTIL2 = list.Sum(x => x.sSentiL2);
                    ust.sPIT = list.Sum(x => x.sPIT);
                    ust.sPITE = list.Sum(x => x.sPITE);
                    ust.sEND = list.Sum(x => x.sEND);
                    ust.sVD1 = list.Sum(x => x.sVD1);
                    ust.sVD1P = list.Sum(x => x.sVD1P);
                    ust.sVD2 = list.Sum(x => x.sVD2);
                    ust.sVD2P = list.Sum(x => x.sVD2P);
                    ust.sVIT = list.Sum(x => x.sVIT);
                    ust.sTARAMA = list.Sum(x => x.sTARAMA);
                    ust.sGKKUL = list.Sum(x => x.sGKKUL);
                    ust.sBD1 = list.Sum(x => x.sBD1);
                    ust.sBD1P = list.Sum(x => x.sBD1P);
                    ust.sBD2 = list.Sum(x => x.sBD2);
                    ust.sANPRO = list.Sum(x => x.sANPRO);
                    ust.sTemelAnaliz = list.Sum(x => x.sTemelAnaliz);
                    ust.sidealGO = list.Sum(x => x.sidealGO);
                    ust.sUserDll = list.Sum(x => x.sUserDll);
                    ust.sBMK = list.Sum(x => x.sBMK);
                    ust.sBarSistem = list.Sum(x => x.sBarSistem);
                    ust.sFSystem = list.Sum(x => x.sFSystem);
                    ust.sDesktop = list.Sum(x => x.sDesktop);
                    ust.sMobile = list.Sum(x => x.sMobile);
                    #endregion
                    #region ucretler
                    ust.KRMD1 = list.Sum(x => x.KRMD1);
                    ust.MKK = list.Sum(x => x.MKK);
                    ust.SENTIL1 = list.Sum(x => x.SentiL1);
                    ust.SENTIL2 = list.Sum(x => x.SentiL2);
                    ust.PD1 = list.Sum(x => x.PD1);
                    ust.PD1P = list.Sum(x => x.PD1P);
                    ust.PD2 = list.Sum(x => x.PD2);
                    ust.PD2P = list.Sum(x => x.PD2P);
                    ust.PIT = list.Sum(x => x.PIT);
                    ust.PITE = list.Sum(x => x.PITE);
                    ust.END = list.Sum(x => x.END);
                    ust.VD1 = list.Sum(x => x.VD1);
                    ust.VD1P = list.Sum(x => x.VD1P);
                    ust.VD2 = list.Sum(x => x.VD2);
                    ust.VD2P = list.Sum(x => x.VD2P);
                    ust.VIT = list.Sum(x => x.VIT);
                    ust.TARAMA = list.Sum(x => x.TARAMA);
                    ust.GKKUL = list.Sum(x => x.GKKUL);
                    ust.BD1 = list.Sum(x => x.BD1);
                    ust.BD1P = list.Sum(x => x.BD1P);
                    ust.BD2 = list.Sum(x => x.BD2);
                    ust.ANPRO = list.Sum(x => x.ANPRO);
                    ust.TemelAnaliz = list.Sum(x => x.TemelAnaliz);
                    ust.idealGO = list.Sum(x => x.idealGO);
                    ust.UserDll = list.Sum(x => x.UserDll);
                    ust.BMK = list.Sum(x => x.BMK);
                    ust.BarSistem = list.Sum(x => x.BarSistem);
                    ust.FSystem = list.Sum(x => x.FSystem);
                    ust.EkranUcreti = list.Sum(x => x.EkranUcreti);
                    ust.Desktop = list.Sum(x => x.Desktop);
                    ust.Mobile = list.Sum(x => x.Mobile);

                    #endregion

                    ust.ToplamFiyat = list.Sum(x => x.ToplamFiyat);
                    ust.KDVDahilToplam = list.Sum(x => x.KDVDahilToplam);
                    ust.ToplamKDV = list.Sum(x => x.KDV);
                    return ust;


                }
                public static UserRecordToplam2 GenelToplam(List<UserRecordToplam2> ToplamListe)
                {
                    var geneltoplam = new UserRecordToplam2();
                    try
                    {

                        #region Sayilar
                        geneltoplam.EkranSayisi = ToplamListe.Sum(x => x.EkranSayisi);
                        geneltoplam.sMobile = ToplamListe.Sum(x => x.sMobile);
                        geneltoplam.sKRMD1 = ToplamListe.Sum(x => x.sKRMD1);
                        geneltoplam.sMKK = ToplamListe.Sum(x => x.sMKK);
                        geneltoplam.sPD1 = ToplamListe.Sum(x => x.sPD1);
                        geneltoplam.sPD1P = ToplamListe.Sum(x => x.sPD1P);
                        geneltoplam.sPD2 = ToplamListe.Sum(x => x.sPD2);
                        geneltoplam.sPD2P = ToplamListe.Sum(x => x.sPD2P);
                        geneltoplam.sPIT = ToplamListe.Sum(x => x.sPIT);
                        geneltoplam.sPITE = ToplamListe.Sum(x => x.sPITE);
                        geneltoplam.sEND = ToplamListe.Sum(x => x.sEND);
                        geneltoplam.sVD1 = ToplamListe.Sum(x => x.sVD1);
                        geneltoplam.sVD1P = ToplamListe.Sum(x => x.sVD1P);
                        geneltoplam.sVD2 = ToplamListe.Sum(x => x.sVD2);
                        geneltoplam.sVD2P = ToplamListe.Sum(x => x.sVD2P);
                        geneltoplam.sVIT = ToplamListe.Sum(x => x.sVIT);
                        geneltoplam.sTARAMA = ToplamListe.Sum(x => x.sTARAMA);
                        geneltoplam.sGKKUL = ToplamListe.Sum(x => x.sGKKUL);
                        geneltoplam.sBD1 = ToplamListe.Sum(x => x.sBD1);
                        geneltoplam.sBD1P = ToplamListe.Sum(x => x.sBD1P);
                        geneltoplam.sBD2 = ToplamListe.Sum(x => x.sBD2);
                        geneltoplam.sANPRO = ToplamListe.Sum(x => x.sANPRO);
                        geneltoplam.sTemelAnaliz = ToplamListe.Sum(x => x.sTemelAnaliz);
                        geneltoplam.sidealGO = ToplamListe.Sum(x => x.sidealGO);
                        geneltoplam.sUserDll = ToplamListe.Sum(x => x.sUserDll);
                        geneltoplam.sBMK = ToplamListe.Sum(x => x.sBMK);
                        geneltoplam.sBarSistem = ToplamListe.Sum(x => x.sBarSistem);
                        geneltoplam.sFSystem = ToplamListe.Sum(x => x.sFSystem);

                        #endregion
                        #region ucretler
                        geneltoplam.KRMD1 = ToplamListe.Sum(x => x.KRMD1);
                        geneltoplam.PD1 = ToplamListe.Sum(x => x.PD1);
                        geneltoplam.PD1P = ToplamListe.Sum(x => x.PD1P);
                        geneltoplam.PD2 = ToplamListe.Sum(x => x.PD2);
                        geneltoplam.PD2P = ToplamListe.Sum(x => x.PD2P);
                        geneltoplam.PIT = ToplamListe.Sum(x => x.PIT);
                        geneltoplam.PITE = ToplamListe.Sum(x => x.PITE);
                        geneltoplam.END = ToplamListe.Sum(x => x.END);
                        geneltoplam.VD1 = ToplamListe.Sum(x => x.VD1);
                        geneltoplam.VD1P = ToplamListe.Sum(x => x.VD1P);
                        geneltoplam.VD2 = ToplamListe.Sum(x => x.VD2);
                        geneltoplam.VD2P = ToplamListe.Sum(x => x.VD2P);
                        geneltoplam.VIT = ToplamListe.Sum(x => x.VIT);
                        geneltoplam.BD1 = ToplamListe.Sum(x => x.BD1);
                        geneltoplam.BD1P = ToplamListe.Sum(x => x.BD1P);
                        geneltoplam.BD2 = ToplamListe.Sum(x => x.BD2);
                        geneltoplam.ANPRO = ToplamListe.Sum(x => x.ANPRO);
                        geneltoplam.TemelAnaliz = ToplamListe.Sum(x => x.TemelAnaliz);
                        geneltoplam.idealGO = ToplamListe.Sum(x => x.idealGO);
                        geneltoplam.UserDll = ToplamListe.Sum(x => x.UserDll);
                        geneltoplam.BMK = ToplamListe.Sum(x => x.BMK);
                        geneltoplam.BarSistem = ToplamListe.Sum(x => x.BarSistem);
                        geneltoplam.FSystem = ToplamListe.Sum(x => x.FSystem);
                        geneltoplam.EkranUcreti = ToplamListe.Sum(x => x.EkranUcreti);
                        geneltoplam.Desktop = ToplamListe.Sum(x => x.Desktop);
                        geneltoplam.Mobile = ToplamListe.Sum(x => x.Mobile);
                        geneltoplam.MKK = ToplamListe.Sum(x => x.MKK);
                        geneltoplam.TARAMA = ToplamListe.Sum(x => x.TARAMA);
                        geneltoplam.GKKUL = ToplamListe.Sum(x => x.GKKUL);
                        #endregion

                        geneltoplam.ToplamFiyat = ToplamListe.Sum(x => x.ToplamFiyat);
                        geneltoplam.KDVDahilToplam = ToplamListe.Sum(x => x.KDVDahilToplam);
                        geneltoplam.ToplamKDV = ToplamListe.Sum(x => x.ToplamKDV);


                        return geneltoplam;
                    }
                    catch { return geneltoplam; }

                }

            }
            public static UserRecordKurum CalculateUser(User user)
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                var sozlesme1 = crm.Sozlesmelers.FirstOrDefault(x => x.SozlesmeNo == user.Sozlesmeler.SozlesmeNo);
                UserRecordKurum userrecord = new UserRecordKurum();

                userrecord.Desktop = (user.LisansDurum.ProYetki) ? ((decimal)sozlesme1.ProFiyat) : 0;
                userrecord.Mobile = (user.LisansDurum.CepYetki) ? ((decimal)sozlesme1.CepFiyat) : 0;
                userrecord.KRMD1 = (user.LisansDurum.COMEX) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fkrmd1 : MyTools.lisansfiyatlari.Ykrmd1) : 0;
                userrecord.PD1 = (user.LisansDurum.PayL1 && user.LisansDurum.PayLP == false && user.LisansDurum.PayL2 == false && user.LisansDurum.Pd2P == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fpd1 : MyTools.lisansfiyatlari.Ypd1) : 0;
                userrecord.PD1P = (user.LisansDurum.PayLP && user.LisansDurum.PayL2 == false && user.LisansDurum.Pd2P == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fpd1p : MyTools.lisansfiyatlari.Ypd1p) : 0;
                userrecord.PD2 = (user.LisansDurum.PayL2 && user.LisansDurum.Pd2P == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fpd2 : MyTools.lisansfiyatlari.Ypd2) : 0;
                userrecord.PD2P = (user.LisansDurum.Pd2P) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fpd2p : MyTools.lisansfiyatlari.Ypd2p) : 0;
                userrecord.END = (user.LisansDurum.PayX) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fend : MyTools.lisansfiyatlari.Yend) : 0;
                userrecord.PIT = (user.LisansDurum.PayGS) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fpit : MyTools.lisansfiyatlari.Ypit) : 0;
                userrecord.PITE = (user.LisansDurum.PITE) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fpite : MyTools.lisansfiyatlari.Ypite) : 0;
                userrecord.BD1 = (user.LisansDurum.TahvilL1 && user.LisansDurum.TahvilLP == false && user.LisansDurum.TahvilL2 == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fbd1 : MyTools.lisansfiyatlari.Ybd1) : 0;
                userrecord.BD1P = (user.LisansDurum.TahvilLP && user.LisansDurum.TahvilL2 == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fbd1p : MyTools.lisansfiyatlari.Ybd1p) : 0;
                userrecord.BD2 = (user.LisansDurum.TahvilL2) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fbd2 : MyTools.lisansfiyatlari.Ybd2) : 0;
                userrecord.ANPRO = (user.LisansDurum.AnPro) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.FanPro : MyTools.lisansfiyatlari.YanPro) : 0;
                userrecord.VD1 = (user.LisansDurum.ViopL1 && user.LisansDurum.ViopLP == false && user.LisansDurum.ViopL2 == false && user.LisansDurum.Vd2P == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fvl1 : MyTools.lisansfiyatlari.Yvl1) : 0;
                userrecord.VD1P = (user.LisansDurum.ViopLP && user.LisansDurum.ViopL2 == false && user.LisansDurum.Vd2P == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fvl1p : MyTools.lisansfiyatlari.Yvl1p) : 0;
                userrecord.VD2 = (user.LisansDurum.ViopL2 && user.LisansDurum.Vd2P == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fvl2 : MyTools.lisansfiyatlari.Yvl2) : 0;
                userrecord.VD2P = (user.LisansDurum.Vd2P) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fvl2p : MyTools.lisansfiyatlari.Yvl2p) : 0;
                userrecord.VIT = (user.LisansDurum.ViopGS) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fvit : MyTools.lisansfiyatlari.Yvit) : 0;
                userrecord.CBOTM = (user.LisansDurum.CBOTM) ? ((!user.ProNonPro.Value) ? MyTools.lisansfiyatlari.CBOTMNonPro : MyTools.lisansfiyatlari.CBOTMPro) : 0;
                userrecord.EUREX = (user.LisansDurum.EUREX) ? ((!user.ProNonPro.Value) ? MyTools.lisansfiyatlari.EUREXNonPro : MyTools.lisansfiyatlari.EUREXPro) : 0;
                userrecord.MKK = (user.LisansDurum.MKK) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fmkk : MyTools.lisansfiyatlari.Ymkk) : 0;
                userrecord.TARAMA = (user.LisansDurum.TARAMA) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Ftarama : MyTools.lisansfiyatlari.Ytarama) : 0;
                userrecord.GKKUL = (user.LisansDurum.GKKUL) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fgkkul : MyTools.lisansfiyatlari.Ygkkul) : 0;

                return userrecord;

            }
            public static UserRecordKurum CalculateUser2(User user)
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                UserRecordKurum userrecord = new UserRecordKurum();

                if (user.KurumsalBilgiler != null && user.KurumsalBilgiler.KurumSube != null)
                {
                    userrecord.KurumMusteriNo = user.KurumsalBilgiler.kurumhesapno;

                    userrecord.sube = (user.KurumsalBilgiler.KurumSube.Trim() == "") ? "MERKEZ" : user.KurumsalBilgiler.KurumSube;
                    userrecord.KullaniciTip = user.KurumsalBilgiler.KurumKullaniciTip;
                }
                else
                {
                    var kurumsalbilgiler = new KurumsalBilgiler();

                    kurumsalbilgiler.kurumhesapno = "10158";
                    kurumsalbilgiler.KurumSube = "Merkez";
                    crm.KurumsalBilgilers.InsertOnSubmit(kurumsalbilgiler);
                    crm.SubmitChanges();

                    user.KurumsalBilgiler = kurumsalbilgiler;

                }

                userrecord.pmts = user.PmtsNo;
                userrecord.Aciklama = user.Aciklama;
                userrecord.MTID = user.FXkurum;
                userrecord.BasliangicTarihi = user.BaslangicTarihi.Value.ToShortDateString();
                userrecord.BitisTarihi = user.ExpiryDate.Value.ToShortDateString();
                userrecord.AdSoyad = user.Name + " " + user.Surname;
                userrecord.Pro = user.LisansDurum.ProYetki;
                userrecord.Cep = user.LisansDurum.CepYetki;
                if (userrecord.Pro) userrecord.sDesktop = 1;
                if (userrecord.Cep) userrecord.sMobile = 1;



                userrecord.KRMD1 = (user.LisansDurum.COMEX) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fkrmd1 : MyTools.lisansfiyatlari.Ykrmd1) : 0;
                userrecord.MKK = (user.LisansDurum.MKK) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fmkk : MyTools.lisansfiyatlari.Ymkk) : 0;
                userrecord.TARAMA = (user.LisansDurum.TARAMA) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Ftarama : MyTools.lisansfiyatlari.Ytarama) : 0;
                userrecord.GKKUL = (user.LisansDurum.GKKUL) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fgkkul : MyTools.lisansfiyatlari.Ygkkul) : 0;
                userrecord.PD1 = (user.LisansDurum.PayL1 && user.LisansDurum.PayLP == false && user.LisansDurum.PayL2 == false && user.LisansDurum.Pd2P == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fpd1 : MyTools.lisansfiyatlari.Ypd1) : 0;
                userrecord.PD1P = (user.LisansDurum.PayLP && user.LisansDurum.PayL2 == false && user.LisansDurum.Pd2P == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fpd1p : MyTools.lisansfiyatlari.Ypd1p) : 0;
                userrecord.PD2 = (user.LisansDurum.PayL2 && user.LisansDurum.Pd2P == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fpd2 : MyTools.lisansfiyatlari.Ypd2) : 0;
                userrecord.PD2P = (user.LisansDurum.Pd2P) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fpd2p : MyTools.lisansfiyatlari.Ypd2p) : 0;
                userrecord.END = (user.LisansDurum.PayX) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fend : MyTools.lisansfiyatlari.Yend) : 0;
                userrecord.PIT = (user.LisansDurum.PayGS) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fpit : MyTools.lisansfiyatlari.Ypit) : 0;
                userrecord.PITE = (user.LisansDurum.PITE) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fpite : MyTools.lisansfiyatlari.Ypite) : 0;
                userrecord.BD1 = (user.LisansDurum.TahvilL1 && user.LisansDurum.TahvilLP == false && user.LisansDurum.TahvilL2 == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fbd1 : MyTools.lisansfiyatlari.Ybd1) : 0;
                userrecord.BD1P = (user.LisansDurum.TahvilLP && user.LisansDurum.TahvilL2 == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fbd1p : MyTools.lisansfiyatlari.Ybd1p) : 0;
                userrecord.BD2 = (user.LisansDurum.TahvilL2) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fbd2 : MyTools.lisansfiyatlari.Ybd2) : 0;
                userrecord.ANPRO = (user.LisansDurum.AnPro) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.FanPro : MyTools.lisansfiyatlari.YanPro) : 0;
                userrecord.SPI = (user.LisansDurum.SPI) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.FSPI : MyTools.lisansfiyatlari.YSPI) : 0;
                userrecord.VD1 = (user.LisansDurum.ViopL1 && user.LisansDurum.ViopLP == false && user.LisansDurum.ViopL2 == false && user.LisansDurum.Vd2P == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fvl1 : MyTools.lisansfiyatlari.Yvl1) : 0;
                userrecord.VD1P = (user.LisansDurum.ViopLP && user.LisansDurum.ViopL2 == false && user.LisansDurum.Vd2P == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fvl1p : MyTools.lisansfiyatlari.Yvl1p) : 0;
                userrecord.VD2 = (user.LisansDurum.ViopL2 && user.LisansDurum.Vd2P == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fvl2 : MyTools.lisansfiyatlari.Yvl2) : 0;
                userrecord.VD2P = (user.LisansDurum.Vd2P) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fvl2p : MyTools.lisansfiyatlari.Yvl2p) : 0;
                userrecord.VIT = (user.LisansDurum.ViopGS) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.Fvit : MyTools.lisansfiyatlari.Yvit) : 0;



                userrecord.SentiL1 = (user.LisansDurum.SentiL1 && user.LisansDurum.SentiL1 == false) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.FSentiL1 : MyTools.lisansfiyatlari.YSentiL1) : 0;
                userrecord.SentiL2 = (user.LisansDurum.SentiL2) ? ((user.MusteriMenseiID == 1) ? MyTools.lisansfiyatlari.FSentiL2 : MyTools.lisansfiyatlari.YSentiL2) : 0;


                userrecord.sKRMD1 = (user.LisansDurum.COMEX) ? 1 : 0;
                userrecord.sMKK = (user.LisansDurum.MKK) ? 1 : 0;
                userrecord.sPD1 = (user.LisansDurum.PayL1 && user.LisansDurum.PayLP == false && user.LisansDurum.PayL2 == false && user.LisansDurum.Pd2P == false) ? 1 : 0;
                userrecord.sPD1P = (user.LisansDurum.PayLP && user.LisansDurum.PayL2 == false && user.LisansDurum.Pd2P == false) ? 1 : 0;
                userrecord.sPD2P = (user.LisansDurum.Pd2P) ? 1 : 0;
                userrecord.sPD2 = (user.LisansDurum.PayL2 && user.LisansDurum.Pd2P == false) ? 1 : 0;
                userrecord.sEND = (user.LisansDurum.PayX) ? 1 : 0;
                userrecord.sPIT = (user.LisansDurum.PayGS) ? 1 : 0;
                userrecord.sPITE = (user.LisansDurum.PITE) ? 1 : 0;
                userrecord.sBD1 = (user.LisansDurum.TahvilL1 && user.LisansDurum.TahvilLP == false && user.LisansDurum.TahvilL2 == false) ? 1 : 0;
                userrecord.sBD1P = (user.LisansDurum.TahvilLP && user.LisansDurum.TahvilL2 == false) ? 1 : 0;
                userrecord.sBD2 = (user.LisansDurum.TahvilL2) ? 1 : 0;
                userrecord.sANPRO = (user.LisansDurum.AnPro) ? 1 : 0;
                userrecord.sVD1 = (user.LisansDurum.ViopL1 && user.LisansDurum.ViopLP == false && user.LisansDurum.ViopL2 == false && user.LisansDurum.Vd2P == false) ? 1 : 0;
                userrecord.sVD1P = (user.LisansDurum.ViopLP && user.LisansDurum.ViopL2 == false && user.LisansDurum.Vd2P == false) ? 1 : 0;
                userrecord.sVD2 = (user.LisansDurum.ViopL2 && user.LisansDurum.Vd2P == false) ? 1 : 0;
                userrecord.sVD2P = (user.LisansDurum.Vd2P) ? 1 : 0;
                userrecord.sVIT = (user.LisansDurum.ViopGS) ? 1 : 0;
                userrecord.sTARAMA = (user.LisansDurum.TARAMA) ? 1 : 0;
                userrecord.sGKKUL = (user.LisansDurum.GKKUL) ? 1 : 0;
                userrecord.sidealGO = (user.LisansDurum.CME) ? 1 : 0;
                userrecord.sUserDll = (user.LisansDurum.CMEM) ? 1 : 0;


                userrecord.sSentiL1 = (user.LisansDurum.SentiL1 && user.LisansDurum.SentiL2 == false) ? 1 : 0;
                userrecord.sSentiL2 = (user.LisansDurum.SentiL2) ? 1 : 0;
                userrecord.sSPI = (user.LisansDurum.SPI) ? 1 : 0;
                if (userrecord.Pro)
                    userrecord.EkranUcreti = userrecord.Desktop;
                if (userrecord.Cep)
                    userrecord.EkranUcreti += userrecord.Mobile;
                return userrecord;
            }
            public static UserRecordKurum CalculateProje(Projeler proje)
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                UserRecordKurum userrecord = new UserRecordKurum();


                var ekranucret = (proje.Sozlesmeler.ParaBirimi.ParaBirimiKod == "TRY") ? proje.Fiyat.Value : proje.Fiyat.Value * proje.Sozlesmeler.ParaBirimi.Kur;

                userrecord.Aciklama = proje.Aciklama;
                userrecord.AdSoyad = proje.ProjeAdi;
                userrecord.EkranUcreti = ekranucret.Value;
                userrecord.Proje = true;

                if (proje.BorsaAltLisansar != null)
                {
                    if (proje.BorsaAltLisansar.GUYEEND.Value)
                        userrecord.END = MyTools.lisansfiyatlari.GUYEEND;
                    if (proje.BorsaAltLisansar.GUYED1P.Value)
                        userrecord.PD1P = MyTools.lisansfiyatlari.GUYED1P;
                    if (proje.BorsaAltLisansar.GUYED2.Value)
                        userrecord.PD2 = MyTools.lisansfiyatlari.GUYED2;
                    //if (proje.BorsaAltLisansar.GUYED2P.Value)
                    //    userrecord.PD2P = MyTools.lisansfiyatlari.GUYED2P;
                }

                userrecord.Toplam();

                return userrecord;

            }
            class alanlar
            {
                public string aboneno = "";
                public string aboneadi = "";
                public string adres = "";
                public string sehir = "";
                public string ulke = "";
                public string postakod = "";
                public string bilgikodu = "";
                public string mensei = "";
                public string erisimtipi = "Kullanıcı";
                public string cihaztoplam = "";
                public string bilgibedel = "";
                public string toplambedel = "";
                public string yonlendirmetarihi = "";
                public string bedelsizaciklama = "";
                public string yetkikendirmetar = "";
                public string lisansYenilemeTarihi = "";
                public string lisansSonlanmaTarihi = "";
            }
            public static Microsoft.Office.Interop.Excel.Workbook GetExcelWorkbook(string filenameX)
            {
                try
                {
                    ExcellApp = (Microsoft.Office.Interop.Excel.Application)Marshal.GetActiveObject("Excel.Application");
                    foreach (Microsoft.Office.Interop.Excel.Workbook excelworkbook in ExcellApp.Workbooks)
                    {
                        if (filenameX == excelworkbook.FullName)
                            return excelworkbook;
                    }
                    return null;
                }
                catch { return null; }
            }

            public static Microsoft.Office.Interop.Excel.Workbook GetCSVWorkbook(string filename)
            {
                try
                {
                    if (ExcelRutin.ExcellApp == null)
                    {
                        ExcelRutin.ExcellApp = new Microsoft.Office.Interop.Excel.Application();
                        ExcelRutin.ExcellApp.DisplayAlerts = false;
                        ExcelRutin.ExcellApp.Visible = false;
                    }

                    
                    var workbook = ExcelRutin.ExcellApp.Workbooks.Add();

                    if (workbook == null)
                    {
                        MyTools.logyaz("Workbook oluşturulamadı!");
                    }
                    return workbook;
                }
                catch (Exception ex)
                {
                    MyTools.logyaz($"GetCSVWorkbook hatası: {ex.Message}");
                    return null;
                }
            }
            public static void RunExcel()
            {
                try
                {
                    ExcellApp = (Microsoft.Office.Interop.Excel.Application)Marshal.GetActiveObject("Excel.Application");
                    ExcellApp.Visible = true;
                }
                catch
                {
                    ExcellApp = new Microsoft.Office.Interop.Excel.Application();
                    ExcellApp.Visible = true;
                }
            }
            public static Microsoft.Office.Interop.Excel.Workbook OpenExcelWorkbook(string filenameX)
            {
                try
                {
                    RunExcel();

                    foreach (Microsoft.Office.Interop.Excel.Workbook excelworkbook in ExcellApp.Workbooks)
                    {
                        if (filenameX == excelworkbook.FullName)
                            return excelworkbook;
                    }

                    if (File.Exists(filenameX) == false)
                    {
                        var excelworkbook = ExcellApp.Workbooks.Add(Type.Missing);
                        excelworkbook.SaveAs(filenameX, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, false, false, null, null, null);
                        excelworkbook.Close(false);
                    }
                    return ExcellApp.Workbooks.Open(filenameX, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", false, false, 0, true, false, false);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); return null; }
            }

            public static Queue<string> excelKuyruk = new Queue<string>();

            public static void OnluPaketToExcel()
            {
                try
                {
                    crmDFNDataContext crm = new crmDFNDataContext();
                    // var Sorgu = crm.Users.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.SPI == true).Select(x => new { x.UserName, AdSoyad = x.Name + " " + x.Surname, x.Iletisim.Il.IlAdi, x.Iletisim.acikadres, x.Iletisim.email, x.Iletisim.Tel1 }).ToList();
                    var Sorgu = crm.Users.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.SPI == true).Select(x => new { x.tckno, AdSoyad = x.Name + " " + x.Surname, x.Iletisim.Il.IlAdi, x.Iletisim.acikadres, x.Iletisim.email, x.Iletisim.Tel1 }).ToList();


                    var filename = System.Windows.Forms.Application.StartupPath + "\\" + "OnluPaketListe" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".XLSX";
                    //string filename = System.Windows.Forms.Application.StartupPath + "\\" + "CrmDFN_Rapor.XLSX";
                    SafeDeleteFile(filename);
                    var excelworkbook = GetExcelWorkbook(filename);
                    if (excelworkbook != null)
                    {
                        excelworkbook.Save();
                        excelworkbook.Close(false);
                    }
                    excelworkbook = OpenExcelWorkbook(filename);
                    if (excelworkbook == null) return;

                    var excelsheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.get_Item(1);

                    var cellarray = new object[Sorgu.Count + 1, 14];


                    //cellarray[0, 0] = "Hesap No";
                    cellarray[0, 0] = "TCKN";
                    cellarray[0, 1] = "Adı Soyadı";
                    cellarray[0, 2] = "Şehir";
                    cellarray[0, 3] = "Adres";
                    cellarray[0, 4] = "Eposta";
                    cellarray[0, 5] = "Telefon";




                    for (int i = 0; i < Sorgu.Count; i++)
                    {

                        //cellarray[i + 1, 0] = Sorgu[i].UserName;
                        cellarray[i + 1, 0] = Sorgu[i].tckno;
                        cellarray[i + 1, 1] = Sorgu[i].AdSoyad;
                        cellarray[i + 1, 2] = Sorgu[i].IlAdi;
                        cellarray[i + 1, 3] = Sorgu[i].acikadres;
                        cellarray[i + 1, 4] = Sorgu[i].email;
                        cellarray[i + 1, 5] = Sorgu[i].Tel1;
                    }
                    ExcellApp.ScreenUpdating = false;
                    if (ExcellApp.Visible)
                    {
                        Microsoft.Office.Interop.Excel.Range range = excelsheet.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                        range.Value = cellarray;
                        range.Columns.AutoFit();
                    }
                    ExcellApp.ScreenUpdating = true;



                }
                catch
                {


                    ExcellApp.ScreenUpdating = true;
                }

            }
            public static void KRMD1ToExcel()
            {
                try
                {
                    crmDFNDataContext crm = new crmDFNDataContext();
                    var Sorgu = crm.Users.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.COMEX == true).Select(x => new { x.UserName, AdSoyad = x.Name + " " + x.Surname, x.Iletisim.Il.IlAdi, x.Iletisim.acikadres, x.Iletisim.email, x.Iletisim.Tel1 }).ToList();
                    // var Sorgu = crm.Users.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.COMEX == true).Select(x => new { x.tckno, AdSoyad = x.Name + " " + x.Surname, x.Iletisim.Il.IlAdi, x.Iletisim.acikadres, x.Iletisim.email, x.Iletisim.Tel1 }).ToList();


                    var filename = System.Windows.Forms.Application.StartupPath + "\\" + "KRMD1Liste" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".XLSX";
                    //string filename = System.Windows.Forms.Application.StartupPath + "\\" + "CrmDFN_Rapor.XLSX";
                    if (File.Exists(filename)) File.Delete(filename);
                    var excelworkbook = GetExcelWorkbook(filename);
                    if (excelworkbook != null)
                    {
                        excelworkbook.Save();
                        excelworkbook.Close(false);
                    }
                    excelworkbook = OpenExcelWorkbook(filename);
                    if (excelworkbook == null) return;

                    var excelsheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.get_Item(1);

                    var cellarray = new object[Sorgu.Count + 1, 14];

                    cellarray[0, 0] = "UserName";
                    cellarray[0, 1] = "Adı Soyadı";
                    cellarray[0, 2] = "Şehir";
                    cellarray[0, 3] = "Adres";
                    cellarray[0, 4] = "Eposta";
                    cellarray[0, 5] = "Telefon";

                    for (int i = 0; i < Sorgu.Count; i++)
                    {

                        cellarray[i + 1, 0] = Sorgu[i].UserName;
                        // cellarray[i + 1, 0] = Sorgu[i].tckno;
                        cellarray[i + 1, 1] = Sorgu[i].AdSoyad;
                        cellarray[i + 1, 2] = Sorgu[i].IlAdi;
                        cellarray[i + 1, 3] = Sorgu[i].acikadres;
                        cellarray[i + 1, 4] = Sorgu[i].email;
                        cellarray[i + 1, 5] = Sorgu[i].Tel1;
                    }
                    ExcellApp.ScreenUpdating = false;
                    if (ExcellApp.Visible)
                    {
                        Microsoft.Office.Interop.Excel.Range range = excelsheet.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                        range.Value = cellarray;
                        range.Columns.AutoFit();
                    }
                    ExcellApp.ScreenUpdating = true;

                }
                catch
                {
                    ExcellApp.ScreenUpdating = true;
                }

            }
        }
        public static string MailGonder(string mailadres, string icerik)
        {

            try
            {

                MailMessage msj = new MailMessage();
                msj.From = new MailAddress("tarantulaspider@gmail.com", "CRM DirectFN", Encoding.UTF8);
                msj.Subject = "CRM DirectFN Giriş Bilgileriniz";
                msj.To.Add(mailadres);
                msj.Body = icerik;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Credentials = new System.Net.NetworkCredential("tarantulaspider", "Sg50465046");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.Send(msj);
                msj.Dispose();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string Split(ref string strX, char charX)
        {
            if (strX == null)
                return "";
            int intPos = -1;
            string strOut = "";
            try
            {
                intPos = strX.IndexOf(charX);
                if (strX.IndexOf(charX) >= 0)
                {
                    strOut = strX.Substring(0, intPos);
                    strX = strX.Substring(intPos + 1);
                }
                return strOut;
            }
            catch { return strOut; }
        }
        public static void logyaz(string text)
        {
            try
            {
                DataLog.Enqueue(text);
            }
            catch { }

        }
        public static void writeDataLog()
        {
            try
            {
                if (DataLog.Count > 0)
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        if (DataLog.Count == 0) break;
                        if (DataLog.TryDequeue(out string message))
                        {
                            try
                            {
                               
                                var logklasor = DateTime.Today.ToString("yyyyMMdd");
                                logklasor = logklasor.Replace("/", ".");
                                var yol = Application.StartupPath + "\\LOG";
                                if (!Directory.Exists(yol))
                                    Directory.CreateDirectory(yol);
                                var dosyaismi = logklasor + ".txt";
                                var yazilacak = yol + "\\" + dosyaismi;
                                var sb = new StringBuilder();
                                sb.AppendLine("\n---------------------------------------------" + DateTime.Now.ToLongTimeString() + "----------------------------------------------- ");
                                sb.AppendLine(message);
                                File.AppendAllText(yazilacak, sb.ToString());
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }
        }
        public static void dosyaeklelogyaz(string text)
        {
            try
            {
                var logklasor = DateTime.Today.ToString("yyyyMMdd");
                logklasor = logklasor.Replace("/", ".");
                var yol = Application.StartupPath + "\\LOG\\DosyaEkleLog";
                if (!Directory.Exists(yol))
                    Directory.CreateDirectory(yol);

                var dosyaismi = logklasor + ".txt";
                var yazilacak = yol + "\\" + dosyaismi;
                var sb = new StringBuilder();

                //using (var sw = new StreamWriter(yazilacak, true))
                //{
                //    sw.WriteLine("\n---------------------------------------------" + DateTime.Now.ToLongTimeString() + "----------------------------------------------- ");
                //    sw.WriteLine(text);
                //}

                sb.AppendLine("\n---------------------------------------------" + DateTime.Now.ToLongTimeString() + "----------------------------------------------- ");
                sb.AppendLine(text);
                File.AppendAllText(yazilacak, sb.ToString());



            }
            catch { }

        }
        public static void ServerEkranlogyaz(string text)
        {
            try
            {
                Ekranlog.Enqueue(text);   
            }
            catch { }

        }

        public static void writeServerEkranlog()
        {
            try
            {
                if(Ekranlog.Count > 0)
                {

                    for (int i = 0; i < 1000; i++)
                    {

                        if (Ekranlog.Count == 0) break;
                        if (Ekranlog.TryDequeue(out string message))
                        {

                            var logklasor = DateTime.Today.ToString("yyyyMMdd");
                            var yol = Application.StartupPath + "\\EKRANLOG";
                            if (!Directory.Exists(yol))
                                Directory.CreateDirectory(yol);
                            var dosyaismi = logklasor + ".txt";
                            var yazilacak = yol + "\\" + dosyaismi;
                            using (var sw = new StreamWriter(yazilacak, true))
                            {
                                sw.WriteLine(message);
                            }

                        }
                    }
                }

            }
            catch { }
        }
        public static Il SehirIdBul(string sehir)
        {
            Il il = new Il();
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                var sehirlistesi = crm.Ils.Select(x => new { x.Id, x.IlAdi }).ToList(); ;



                sehir = sehir.ToLower();

                if (sehirlistesi.Where(x => x.IlAdi._ToEngUp() == sehir._ToEngUp()).Any())
                {
                    var id = sehirlistesi.FirstOrDefault(x => x.IlAdi._ToEngUp() == sehir._ToEngUp()).Id;

                    il = crm.Ils.FirstOrDefault(x => x.Id == id);
                }


                return il;
            }
            catch
            {

                return il;
            }



        }
        public static Ilce IlceIdBul(string ilce)
        {
            Ilce ilcee = new Ilce();
            try
            {
                ilce = ilce.ToLower();
                crmDFNDataContext crm = new crmDFNDataContext();
                if (crm.Ilces.Where(x => x.IlceAdi == ilce).Any())
                {
                    ilcee = crm.Ilces.FirstOrDefault(x => x.IlceAdi == ilce);
                }
                return ilcee;
            }
            catch
            {

                return ilcee;
            }
        }
        public static int KurumBul(string pmtsno)

        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                var id = 0;
                if (crm.Kurumlars.Where(x => x.PmtsNo == pmtsno).Any())
                {
                    var kurum = crm.Kurumlars.FirstOrDefault(x => x.PmtsNo == pmtsno);
                    return kurum.KurumID;
                }
                return id;

            }
            catch
            {

                return 0;
            }


        }
        public static void AyarYaz(string Baslik, string Anahtar, string Deger, string path)
        {
            WritePrivateProfileString(Baslik, Anahtar, Deger, path);
        }
        public static string AyarOku(string Baslik, string Anahtar, int lenth, string path)
        {
            StringBuilder okunan = new StringBuilder(lenth);
            GetPrivateProfileString(Baslik, Anahtar, "", okunan, lenth, path);

            return okunan.ToString().Trim();

        }
        public static string CalisanAdSoyadGetir(int id)
        {
            string adsoyad = "";
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                if (crm.Calisans.Where(x => x.calisanID == id).Any())
                {
                    var cal = crm.Calisans.FirstOrDefault(x => x.calisanID == id);
                    adsoyad = cal.Ad + " " + cal.Soyad;
                }

                return adsoyad;
            }
            catch (Exception)
            {

                return adsoyad;
            }
        }
        public static string MailGonder2(string mailadres, string icerik)
        {
            try
            {
                MailMessage msj = new MailMessage();
                msj.From = new MailAddress("idealdestek@idealdata.com.tr", "İdeal Data", Encoding.UTF8);
                msj.Subject = "İDEAL ABONELİĞİNİZ HAKKINDA";
                msj.To.Add(mailadres);
                msj.CC.Add("pazarlama@idealdata.com.tr,teknik@idealdata.com.tr");
                msj.Body = icerik;
                SmtpClient smtp = new SmtpClient("smtp.office365.com");
                smtp.Credentials = new System.Net.NetworkCredential("idealdestek@idealdata.com.tr", "Mor43719");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.Send(msj);
                msj.Dispose();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string SendAlarmMulti(string messageX)
        {
            var result = "-1";
            try
            {
                var webAddr = "https://fcm.googleapis.com/fcm/send";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", ServerKey));
                httpWebRequest.Headers.Add(string.Format("Sender: id={0}", SenderId));
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json = "{\"to\": \"/topics/all\",\"content_available\":true,\"data\": {\"message\": \"" + messageX + "\",},\"notification\": {\"body\": \"" + messageX + "\",\"sound\": \"default\"}}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public static string SendAlarmToAndroidAndIos(string tokenX, string messageX)
        {

            var result = "-1";

            try
            {

                var webAddr = "https://fcm.googleapis.com/fcm/send";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", ServerKey));
                httpWebRequest.Headers.Add(string.Format("Sender: id={0}", SenderId));
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //string json = "{\"to\": \""+ tokenX + "\",\"content_available\":true,\"data\": {\"message\": \"" + messageX + "\",}}";
                    string json = "{\"to\": \"" + tokenX + "\",\"content_available\":true,\"data\": {\"message\": \"" + messageX + "\",},\"notification\": {\"body\": \"" + messageX + "\",\"sound\": \"default\"}}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public static Ulke UlkeIdBul(string ulkeadi)
        {

            if (ulkeadi == "TR") ulkeadi = "Türkiye";
            Ulke ulke = new Ulke();
            try
            {
                ulkeadi = ulkeadi.ToLower();
                crmDFNDataContext crm = new crmDFNDataContext();
                if (crm.Ulkes.Where(x => x.UlkeAdi == ulkeadi).Any())
                {
                    ulke = crm.Ulkes.FirstOrDefault(x => x.UlkeAdi == ulkeadi);
                }
                return ulke;
            }
            catch
            {
                return ulke;
            }
        }
        public static LisansDurum lisansAcKapa(LisansDurum LisansDurum, bool status)
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            LisansDurum sondurum = new LisansDurum();


            sondurum.YayinDurumu = status;
            sondurum.PayL1 = LisansDurum.PayL1;
            sondurum.PayLP = LisansDurum.PayLP;
            sondurum.PayL2 = LisansDurum.PayL2;
            sondurum.Pd2P = LisansDurum.Pd2P;
            sondurum.PayX = LisansDurum.PayX;
            sondurum.PayGS = LisansDurum.PayGS;
            sondurum.VeriAnalitik = LisansDurum.VeriAnalitik;

            sondurum.ViopL1 = LisansDurum.ViopL1;
            sondurum.ViopLP = LisansDurum.ViopLP;
            sondurum.ViopL2 = LisansDurum.ViopL2;
            sondurum.Vd2P = LisansDurum.Vd2P;
            sondurum.ViopGS = LisansDurum.ViopGS;


            sondurum.TahvilL1 = LisansDurum.TahvilL1;
            sondurum.TahvilLP = LisansDurum.TahvilLP;
            sondurum.TahvilL2 = LisansDurum.TahvilL2;

            sondurum.AnPro = LisansDurum.AnPro;

            sondurum.DJI = LisansDurum.DJI;
            sondurum.SPI = LisansDurum.SPI;
            sondurum.XETRA = LisansDurum.XETRA;
            sondurum.CBOT = LisansDurum.CBOT;
            sondurum.CBOTM = LisansDurum.CBOTM;
            sondurum.CME = LisansDurum.CME;
            sondurum.CMEM = LisansDurum.CMEM;
            sondurum.EUREX = LisansDurum.EUREX;

            sondurum.CepYetki = LisansDurum.CepYetki;
            sondurum.ProYetki = LisansDurum.ProYetki;
            sondurum.ROBOT = LisansDurum.ROBOT;

            sondurum.SCMDownload = LisansDurum.SCMDownload;
            sondurum.SCMRealTıme = LisansDurum.SCMRealTıme;
            sondurum.SCMUsable = LisansDurum.SCMUsable;

            sondurum.Futgck = LisansDurum.Futgck;
            sondurum.WINX = LisansDurum.WINX;


            crm.LisansDurums.InsertOnSubmit(sondurum);
            crm.SubmitChanges();

            return sondurum;


        }
        public static void ExcelDurumFormAc()
        {
            if (formExcelDurum.reference == null)
            {
                formExcelDurum.reference = new formExcelDurum();
                formExcelDurum.reference.Show();
            }
            else
            {
                formExcelDurum.reference.BringToFront();
            }
        }
        public static void ExcelDurumKapat()
        {
            try
            {
                if (formExcelDurum.reference != null)
                {
                    formExcelDurum.reference.Close();
                    formExcelDurum.reference = null;
                }
            }
            catch { }
        }
        public static void ExcelDurumYaz(string mesaj)
        {
            try
            {

                clsEvent.onExcelMesajGeldi(mesaj);
                //excelKuyruk.Enqueue(mesaj);
            }
            catch { }
        }
        public static LisansDurum lisansAcKapa(LisansDurum LisansDurum, bool status, int tip)
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            #region LisansCopy
            LisansDurum sondurum = new LisansDurum();

            DateTime lisansEndDate = DateTime.Now.AddDays(-1);

            if (tip == 1)
                sondurum.YayinDurumu = status;
            if (tip == 2)
                sondurum.YayinDurumu = LisansDurum.YayinDurumu;
            sondurum.ProYetki = LisansDurum.ProYetki;
            sondurum.CepYetki = LisansDurum.CepYetki;
            sondurum.COMEX = LisansDurum.COMEX;
             sondurum.PayL1 = LisansDurum.PayL1;
            sondurum.PayLP = LisansDurum.PayLP;
            sondurum.PayL2 = LisansDurum.PayL2;
            sondurum.Pd2P = LisansDurum.Pd2P;
            sondurum.PayX = LisansDurum.PayX;
            sondurum.PayGS = LisansDurum.PayGS;
            sondurum.PITE = LisansDurum.PITE;

            sondurum.ViopL1 = LisansDurum.ViopL1;
            sondurum.ViopLP = LisansDurum.ViopLP;
            sondurum.ViopL2 = LisansDurum.ViopL2;
            sondurum.Vd2P = LisansDurum.Vd2P;
            sondurum.ViopGS = LisansDurum.ViopGS;


            sondurum.TahvilL1 = LisansDurum.TahvilL1;
            sondurum.TahvilLP = LisansDurum.TahvilLP;
            sondurum.TahvilL2 = LisansDurum.TahvilL2;

            // sondurum.AnPro = LisansDurum.AnPro;
            sondurum.MKK = LisansDurum.MKK;
            sondurum.GKKUL = LisansDurum.GKKUL;
            //  sondurum.TARAMA = LisansDurum.TARAMA;

            //  sondurum.DJI = LisansDurum.DJI;
            //sondurum.SPI = LisansDurum.SPI;
            //sondurum.XETRA = LisansDurum.XETRA;
            // sondurum.CBOT = LisansDurum.CBOT;
            //sondurum.CBOTM = LisansDurum.CBOTM;
            sondurum.CME = LisansDurum.CME;
            //sondurum.CMEM = LisansDurum.CMEM;
            //sondurum.EUREX = LisansDurum.EUREX;

            sondurum.CepYetki = LisansDurum.CepYetki;
            sondurum.ProYetki = LisansDurum.ProYetki;
            sondurum.ROBOT = LisansDurum.ROBOT;

            //sondurum.SCMDownload = LisansDurum.SCMDownload;
            //sondurum.SCMRealTıme = LisansDurum.SCMRealTıme;
            //sondurum.SCMUsable = LisansDurum.SCMUsable;

            //sondurum.Futgck = LisansDurum.Futgck;
            //sondurum.WINX = LisansDurum.WINX;

            //sondurum.BMK = LisansDurum.BMK;
            //sondurum.BMC = LisansDurum.BMC;
            //sondurum.TemelAnaliz = LisansDurum.TemelAnaliz;
            //sondurum.BarSistem = LisansDurum.BarSistem;
            //sondurum.FSystem = LisansDurum.FSystem;
            //sondurum.PARA = LisansDurum.PARA;
            //sondurum.HISSEA = LisansDurum.HISSEA;
            //sondurum.SentiL1 = LisansDurum.SentiL1;
            //sondurum.SentiL2 = LisansDurum.SentiL2;

            //START DATE
             sondurum.PayL1Start = LisansDurum.PayL1Start;
            sondurum.PayLPStart = LisansDurum.PayLPStart;
            sondurum.PayL2Start = LisansDurum.PayL2Start;
            sondurum.Pd2PStart = LisansDurum.Pd2PStart;
            sondurum.PayXStart = LisansDurum.PayXStart;
            sondurum.PayGSStart = LisansDurum.PayGSStart;
            sondurum.PayPiteStart = LisansDurum.PayPiteStart;

            sondurum.ViopL1Start = LisansDurum.ViopL1Start;
            sondurum.ViopLPStart = LisansDurum.ViopLPStart;
            sondurum.ViopL2Start = LisansDurum.ViopL2Start;
            sondurum.Vd2PStart = LisansDurum.Vd2PStart;
            sondurum.ViopGSStart = LisansDurum.ViopGSStart;

            sondurum.TahvilL1Start = LisansDurum.TahvilL1Start;
            sondurum.TahvilLPStart = LisansDurum.TahvilLPStart;
            sondurum.TahvilL2Start = LisansDurum.TahvilL2Start;

           // sondurum.AnProStart = LisansDurum.AnProStart;
            sondurum.MKKStart = LisansDurum.MKKStart;
            sondurum.GKKULStart = LisansDurum.GKKULStart;
            //  sondurum.TaramaStart = LisansDurum.TaramaStart;
            //sondurum.CMEStart = LisansDurum.CMEStart;
            //sondurum.CMEMStart = LisansDurum.CMEMStart;

            //sondurum.RobotStart = LisansDurum.RobotStart;
            //sondurum.BMKStart = LisansDurum.BMKStart;
            //sondurum.TemelAnalizStart = LisansDurum.TemelAnalizStart;
            //sondurum.BarSistemStart = LisansDurum.BarSistemStart;
            //sondurum.FSystemStart = LisansDurum.FSystemStart;
            sondurum.KRMD1Start = LisansDurum.KRMD1Start;
            //sondurum.PARAStart = LisansDurum.PARAStart;
            //sondurum.HISSEAStart = LisansDurum.HISSEAStart;
            //sondurum.SentiL1Start = LisansDurum.SentiL1Start;
            //sondurum.SentiL2Start = LisansDurum.SentiL2Start;
            sondurum.ProYetkiStart = LisansDurum.ProYetkiStart;
            sondurum.CepYetkiStart = LisansDurum.CepYetkiStart;
            // sondurum.BMCStart = LisansDurum.BMCStart;

            //END DATE
            if (status == false)
            {
                if (LisansDurum.PayL1End != null)
                {
                    sondurum.PayL1End = lisansEndDate;
                }
                if (LisansDurum.PayLPEnd != null)
                {
                    sondurum.PayLPEnd = lisansEndDate;
                }
                if (LisansDurum.PayL2End != null)
                {
                    sondurum.PayL2End = lisansEndDate;
                }
                if (LisansDurum.Pd2PEnd != null)
                {
                    sondurum.Pd2PEnd = lisansEndDate;
                }
                if (LisansDurum.PayXEnd != null)
                {
                    sondurum.PayXEnd = lisansEndDate;
                }
                if (LisansDurum.PayGSEnd != null)
                {
                    sondurum.PayGSEnd = lisansEndDate;
                }
                if (LisansDurum.PayPiteEnd != null)
                {
                    sondurum.PayPiteEnd = lisansEndDate;
                }
                if (LisansDurum.ViopL1End != null)
                {
                    sondurum.ViopL1End = lisansEndDate;
                }
                if (LisansDurum.ViopLPEnd != null)
                {
                    sondurum.ViopLPEnd = lisansEndDate;
                }
                if (LisansDurum.ViopL2End != null)
                {
                    sondurum.ViopL2End = lisansEndDate;
                }
                if (LisansDurum.Vd2PEnd != null)
                {
                    sondurum.Vd2PEnd = lisansEndDate;
                }
                if (LisansDurum.ViopGSEnd != null)
                {
                    sondurum.ViopGSEnd = lisansEndDate;
                }
                if (LisansDurum.TahvilL1End != null)
                {
                    sondurum.TahvilL1End = lisansEndDate;
                }
                if (LisansDurum.TahvilLPEnd != null)
                {
                    sondurum.TahvilLPEnd = lisansEndDate;
                }
                if (LisansDurum.TahvilL2End != null)
                {
                    sondurum.TahvilL2End = lisansEndDate;
                }
                //if (LisansDurum.AnProEnd != null)
                //{
                //    sondurum.AnProEnd = lisansEndDate;
                //}
                if (LisansDurum.CMEEnd != null)
                {
                    sondurum.CMEEnd = lisansEndDate;
                }
                //if (LisansDurum.CMEMEnd != null)
                //{
                //    sondurum.CMEMEnd = lisansEndDate;
                //}
                //if (LisansDurum.RobotEnd != null)
                //{
                //    sondurum.RobotEnd = lisansEndDate;
                //}
                //if (LisansDurum.BMKEnd != null)
                //{
                //    sondurum.BMKEnd = lisansEndDate;
                //}
                //if (LisansDurum.BMCEnd != null)
                //{
                //    sondurum.BMCEnd = lisansEndDate;
                //}
                //if (LisansDurum.TemelAnalizEnd != null)
                //{
                //    sondurum.TemelAnalizEnd = lisansEndDate;
                //}
                //if (LisansDurum.BarSistemEnd != null)
                //{
                //    sondurum.BarSistemEnd = lisansEndDate;
                //}
                //if (LisansDurum.FSystemEnd != null)
                //{
                //    sondurum.FSystemEnd = lisansEndDate;
                //}
                if (LisansDurum.KRMD1End != null)
                {
                    sondurum.KRMD1End = lisansEndDate;
                }
                if (LisansDurum.MKKEnd != null)
                {
                    sondurum.MKKEnd = lisansEndDate;
                }
                if (LisansDurum.GKKULEnd != null)
                {
                    sondurum.GKKULEnd = lisansEndDate;
                }
                //if (LisansDurum.TaramaEnd != null)
                //{
                //    sondurum.TaramaEnd = lisansEndDate;
                //}
                //if (LisansDurum.PARAEnd != null)
                //{
                //    sondurum.PARAEnd = lisansEndDate;
                //}
                //if (LisansDurum.HISSEAEnd != null)
                //{
                //    sondurum.HISSEAEnd = lisansEndDate;
                //}
                //if (LisansDurum.SentiL1End != null)
                //{
                //    sondurum.SentiL1End = lisansEndDate;
                //}
                //if (LisansDurum.SentiL2End != null)
                //{
                //    sondurum.SentiL2End = lisansEndDate;
                //}
                if (LisansDurum.ProYetkiEnd != null)
                {
                    sondurum.ProYetkiEnd = lisansEndDate;
                }
                if (LisansDurum.CepYetkiEnd != null)
                {
                    sondurum.CepYetkiEnd = lisansEndDate;
                }
            }
            else
            {
                sondurum.PayL1End = LisansDurum.PayL1End;
                sondurum.PayLPEnd = LisansDurum.PayLPEnd;
                sondurum.PayL2End = LisansDurum.PayL2End;
                sondurum.Pd2PEnd = LisansDurum.Pd2PEnd;
                sondurum.PayXEnd = LisansDurum.PayXEnd;
                sondurum.PayGSEnd = LisansDurum.PayGSEnd;
                sondurum.PayPiteEnd = LisansDurum.PayPiteEnd;

                 sondurum.ViopL1End = LisansDurum.ViopL1End;
                sondurum.ViopLPEnd = LisansDurum.ViopLPEnd;
                sondurum.ViopL2End = LisansDurum.ViopL2End;
                sondurum.Vd2PEnd = LisansDurum.Vd2PEnd;
                sondurum.ViopGSEnd = LisansDurum.ViopGSEnd;

                 sondurum.TahvilL1End = LisansDurum.TahvilL1End;
                sondurum.TahvilLPEnd = LisansDurum.TahvilLPEnd;
                sondurum.TahvilL2End = LisansDurum.TahvilL2End;
                // sondurum.AnProEnd = LisansDurum.AnProEnd;
                sondurum.CMEEnd = LisansDurum.CMEEnd;
                //sondurum.CMEMEnd = LisansDurum.CMEMEnd;

                //sondurum.RobotEnd = LisansDurum.RobotEnd;
                //sondurum.BMKEnd = LisansDurum.BMKEnd;
                //sondurum.BMCEnd = LisansDurum.BMCEnd;
                //sondurum.TemelAnalizEnd = LisansDurum.TemelAnalizEnd;
                //sondurum.BarSistemEnd = LisansDurum.BarSistemEnd;
                //sondurum.FSystemEnd = LisansDurum.FSystemEnd;
                sondurum.KRMD1End = LisansDurum.KRMD1End;
                sondurum.MKKEnd = LisansDurum.MKKEnd;
                sondurum.GKKULEnd = LisansDurum.GKKULEnd;
                // sondurum.TaramaEnd = LisansDurum.TaramaEnd;

                //sondurum.PARAEnd = LisansDurum.PARAEnd;
                //sondurum.HISSEAEnd = LisansDurum.HISSEAEnd;
                //sondurum.SentiL1End = LisansDurum.SentiL1End;
                //sondurum.SentiL2End = LisansDurum.SentiL2End;

                sondurum.ProYetkiEnd = LisansDurum.ProYetkiEnd;
                sondurum.CepYetkiEnd = LisansDurum.CepYetkiEnd;

            }
            crm.LisansDurums.InsertOnSubmit(sondurum);
            crm.SubmitChanges();
            #endregion
            return sondurum;
        }

        public static void ProlisansiOlanKullaniciExcelRaporu(bool openworkBook = true)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                var date = DateTime.Now;
                var guncelay = date.ToString("MMMMMMMMM").ToUpper().Trim();
                var dosyaadi = "Pro_lisansi_Olan_Kullanici_Excel_Raporu--" + guncelay + " " + date.Year + ".xlsx";
                var yol = System.Windows.Forms.Application.StartupPath + "\\RAPOR\\Pro_lisansi_Olan_Kullanici_Excel_Raporu";
                if (!Directory.Exists(yol))
                    Directory.CreateDirectory(yol);

                string filename = yol + "\\" + dosyaadi;

                var excelworkbook = ExcelRutin.GetExcelWorkbook(filename);
                if (excelworkbook != null)
                {
                    //excelworkbook.Save();
                    excelworkbook.Close(false);
                }
                if (openworkBook)
                {
                    excelworkbook = ExcelRutin.OpenExcelWorkbook(filename);
                }

                if (excelworkbook == null) return;
                var sorgu = crm.Users.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ProYetki == true).Select(x => new
                {
                    x.UserName,
                    // x.tckno,
                    x.Name,
                    x.Surname,
                    x.Iletisim.Ceptel,
                    x.Iletisim.email,
                    x.Iletisim.Tel1,
                    x.Iletisim.Tel2,
                    x.Sozlesmeler.MusteriNo,
                    x.KurumsalBilgiler.KurumSube
                }).ToList();

                var cellarray = new object[sorgu.Count + 1, 53];
                var excelsheetson = (Microsoft.Office.Interop.Excel.Worksheet)excelworkbook.Sheets.Add();
                List<Microsoft.Office.Interop.Excel.Worksheet> sheets = new List<Microsoft.Office.Interop.Excel.Worksheet>();
                foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in excelworkbook.Worksheets)
                    sheets.Add(sheet);

                var excelsheetilk = sheets[0];
                /// excelsheetilk.Name = guncelay + " " + date.Year;
                excelsheetilk.Name = "ProLisansKullanicilari";
                excelsheetilk.Activate();

                #region Basliklar

                excelsheetilk.Application.ActiveWindow.SplitRow = 1;
                excelsheetilk.Application.ActiveWindow.FreezePanes = true;

                var currentRow = 0;
                var collindex = 0;
                var brow = 0;

                cellarray[currentRow, collindex] = "AD SOYAD"; collindex++;
                cellarray[currentRow, collindex] = "Kullanıcı_Adı"; collindex++;
                //cellarray[currentRow, collindex] = "TCKN"; collindex++;
                cellarray[currentRow, collindex] = "E_Mail"; collindex++;
                cellarray[currentRow, collindex] = "Cep_Tel"; collindex++;
                cellarray[currentRow, collindex] = "Tel1"; collindex++;
                cellarray[currentRow, collindex] = "Tel2"; collindex++;
                cellarray[currentRow, collindex] = "Musteri_no"; collindex++;
                cellarray[currentRow, collindex] = "Sube"; collindex++;

                // x.KurumsalBilgiler.KurumSube
                #endregion

                Microsoft.Office.Interop.Excel.Range baslikrange2 = excelsheetilk.Application.get_Range(ExcelRutin.HucreAdresBul(0, 0), ExcelRutin.HucreAdresBul(0, collindex));
                baslikrange2.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                baslikrange2.Font.Bold = true;
                baslikrange2.Borders.Value = true;
                baslikrange2.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange);
                baslikrange2 = excelsheetilk.Application.get_Range(ExcelRutin.HucreAdresBul(0, 1), ExcelRutin.HucreAdresBul(0, collindex));
                baslikrange2.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignBottom;
                baslikrange2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                baslikrange2.Orientation = 90;

                currentRow++;
                collindex = 0;

                while (currentRow <= sorgu.Count)
                {
                    cellarray[currentRow, collindex] = sorgu[currentRow - 1].Name + " " + sorgu[currentRow - 1].Surname; collindex++;
                    cellarray[currentRow, collindex] = sorgu[currentRow - 1].UserName; collindex++;
                    //cellarray[currentRow, collindex] = sorgu[currentRow - 1].tckno; collindex++;
                    cellarray[currentRow, collindex] = sorgu[currentRow - 1].email; collindex++;
                    cellarray[currentRow, collindex] = sorgu[currentRow - 1].Ceptel; collindex++;
                    cellarray[currentRow, collindex] = sorgu[currentRow - 1].Tel1; collindex++;
                    cellarray[currentRow, collindex] = sorgu[currentRow - 1].Tel2; collindex++;                        //
                    cellarray[currentRow, collindex] = sorgu[currentRow - 1].MusteriNo; collindex++;
                    cellarray[currentRow, collindex] = sorgu[currentRow - 1].KurumSube; collindex++;


                    collindex = 0;
                    currentRow++;

                }

                ExcelRutin.ExcellApp.ScreenUpdating = false;
                if (ExcelRutin.ExcellApp.Visible)
                {
                    Microsoft.Office.Interop.Excel.Range range = excelsheetilk.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                    range.Value = cellarray;
                    range.Columns.AutoFit();
                }
                ExcelRutin.ExcellApp.ScreenUpdating = true;
                excelworkbook.Save();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
