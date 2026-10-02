using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using System.Web;
using DirectFNCRM.AdminViews;

namespace DirectFNCRM
{
    public partial class formLogin : Form
    {
        public formLogin()
        {
            InitializeComponent();
        }
        public string ayarlarpath = Application.StartupPath + @"\ayarlar.ini";

        public string versiyon = MyTools.KurumVersiyon;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static bool HasConnection()
        {
            try
            {
                System.Net.IPHostEntry i = System.Net.Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch
            {
                return false;
            }
        }


        private void formLogin_Load(object sender, EventArgs e)
        {




            MyTools.KurumKontrolList.Add("10011"); //Ak Yatırım
            MyTools.KurumKontrolList.Add("10155"); // Osmanlı Menkul
            MyTools.KurumKontrolList.Add("10148");  // HSBC          
            MyTools.KurumKontrolList.Add("10196"); // Meksa Yatırım
            MyTools.KurumKontrolList.Add("10860"); // Halk Yatırım
            MyTools.KurumKontrolList.Add("14999"); // A1 CapitalCrmA1CapitalDBConnectionString
            MyTools.KurumKontrolList.Add("10155"); // A1 CapitalCrmA1CapitalDBConnectionString
            MyTools.KurumKontrolList.Add("10158"); // İNFO
            MyTools.KurumKontrolList.Add("14931"); // ALB
            MyTools.KurumKontrolList.Add("10196"); // Meksa
            MyTools.KurumKontrolList.Add("10191"); // Marbaş
            MyTools.KurumKontrolList.Add("18845"); // Bizim Menkul Değerler
            MyTools.KurumKontrolList.Add("10282"); // ICBC
            MyTools.KurumKontrolList.Add("10304"); // Vakıf
            MyTools.KurumKontrolList.Add("10005"); // Acar Menkul
            MyTools.KurumKontrolList.Add("16398"); // Trive
            MyTools.KurumKontrolList.Add("18843"); // Colendi
            MyTools.KurumKontrolList.Add("14594"); // Rasyonet
            MyTools.KurumKontrolList.Add("11175"); // Bulls Yatırım
            MyTools.KurumKontrolList.Add("18859"); // Allbatross
            MyTools.KurumKontrolList.Add("14684"); // NCM(NoorCapital)
            MyTools.KurumKontrolList.Add("18874"); // Destek Menkul
            MyTools.KurumKontrolList.Add("14999"); // ACP Menkul
            MyTools.KurumKontrolList.Add("10269"); // STJ Menkul

            MyTools.versiyon = versiyon;

            lblVersiyon.Text = versiyon;

            var kurumTxt = MyTools.AyarOku("Kurum", "KurumURL", 100, ayarlarpath);
            var kurumURL = MyTools.AyarOku("Kurum", "KurumURL", 100, Application.StartupPath + @"\ayarlar.ini") + "/versiyon.txt";
            var hisseSinyal = MyTools.AyarOku("Kurum", "HisseSinyal", 100,  ayarlarpath) ; // hisse sinyal

            compnetGizleGoster(false);
            btnChange1.Visible = true;


            // http status = 200 =>ok
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(kurumURL);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (HasConnection())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {


                            using (WebClient wc = new WebClient())
                            {
                                wc.DownloadFileCompleted += new AsyncCompletedEventHandler(completed);
                                wc.DownloadFileAsync(new Uri(kurumURL), Application.StartupPath);
                            }

                        }

                    }


                }
            }
            catch (Exception)
            {

            }
            try
            {
                StringBuilder okunan = new StringBuilder(100);
                GetPrivateProfileString("Authentication", "UserName", "", okunan, 100, Application.StartupPath + @"\ayarlar.ini");
                txtUsername.Text = okunan.ToString().Trim();
                GetPrivateProfileString("Authentication", "Password", "", okunan, 100, Application.StartupPath + @"\ayarlar.ini");
                txtPassword.Text = MyTools.Sifreleme.Decryp(okunan.ToString().Trim());

                var kurumkodstr = MyTools.AyarOku("Kurum", "KurumKod", 20, ayarlarpath);
               

                //MyTools.KurumAd = MyTools.AyarOku("Kurum", "KurumAdi", 100, ayarlarpath);
                MyTools.KurumAd = MyTools.AyarOku("Kurum", "KurumAdı", 100, ayarlarpath);
                MyTools.HisseSinyal = MyTools.AyarOku("Kurum", "HisseSinyal", 100, ayarlarpath);

                if (kurumkodstr == "")
                {
                    MessageBox.Show("Kurum Kodu Bilgisini girilmedi !!");
                    this.Close();
                }
                else
                {
                    if (MyTools.KurumKontrolList.Contains(kurumkodstr))
                    {
                        MyTools.KurumKod = kurumkodstr;
                    }
                    else
                    {
                        MessageBox.Show("Kurum Kod Tanımı Yanlış !!");
                        this.Close();
                    }
                }

                if (txtPassword.Text != "")
                {
                    chxHatirla.CheckState = CheckState.Checked;
                    btnLogin.Select();
                }
                else
                {
                    txtPassword.Select();
                }

    


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void completed(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                var ver = File.ReadAllText(Application.StartupPath + "\\versiyon.txt");

                double ver1 = Convert.ToDouble(MyTools.KurumVersiyon); // kod
                double ver2 = Convert.ToDouble(ver.Trim()); //txt
                                                            // if (ver.Trim() != MyTools.KurumVersiyon)
                if (ver1 < ver2)
                {
                    if (MessageBox.Show("Yeni versiyon bulundu, version yenilensin mi ?", "Versiyon Yenileme", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var filename = Application.StartupPath + "\\VersiyonYenile.exe";
                        if (File.Exists(filename))
                        {


                            Process.Start(filename);
                            Application.Exit();
                        }
                        else MessageBox.Show("VersiyonYenile.exe Yok");


                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }





        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {

                #region hissesinyal

                //hisse sinyal kontrolü
                string DBConnectionString = DirectFNCRM.Properties.Settings.Default.DBConnectionString;
                formAdminAra fara = new formAdminAra();
                if (DBConnectionString == "" || DBConnectionString == null)
                {
                    MessageBox.Show("DBConnectionString string yazmadınız!");
                    //formAdminAra fara = new formAdminAra();
                    //fara.hisseSinyaluserMenuItem.Visible = true;
             

                }
                if (MyTools.HisseSinyal == "1")
                {
                    string HisseSinyalConnectionString = DirectFNCRM.Properties.Settings.Default.DBConnectionString;
               
                    if (HisseSinyalConnectionString == "" || HisseSinyalConnectionString ==null)
                    {
                        MessageBox.Show("HisseSinyalConnectionString yazmadınız!");
                       MyTools.HisseSinyal = "0";
                     

                    }
                    else
                    {
                        MyTools.HisseSinyal = "1";

                    }
                }
                


                #endregion





                WritePrivateProfileString("Authentication", "UserName", txtUsername.Text, Application.StartupPath + "\\ayarlar.ini");

                if (chxHatirla.CheckState == CheckState.Checked)
                {

                    WritePrivateProfileString("Authentication", "Password", MyTools.Sifreleme.Encryp(txtPassword.Text), Application.StartupPath + "\\ayarlar.ini");
                }
                else
                {
                    //  WritePrivateProfileString("Authentication", "UserName", "", Application.StartupPath + "\\ayarlar.ini");
                    WritePrivateProfileString("Authentication", "Password", "", Application.StartupPath + "\\ayarlar.ini");

                }

                //AdminViews.formAdmin frmadmin2 = new AdminViews.formAdmin();
                //frmadmin2.Show();
                //return;


                var sifrelipass = MyTools.Sifreleme.Encryp(txtPassword.Text);


                using (crmDFNDataContext crm = new crmDFNDataContext())
                {

                    if (crm.Calisans.Where(x => x.UserName == txtUsername.Text).Any())
                    {
                        var calisan = crm.Calisans.FirstOrDefault(x => x.UserName == txtUsername.Text);
                        if (calisan.Password == sifrelipass)
                        {
                            var sonuc = crm.Calisans.FirstOrDefault(x => x.UserName == txtUsername.Text);
                            var sonuc2 = from c in crm.Calisans join y in crm.Yetkis on c.YetiID equals y.yetkiID select new { c.Ad, y.YetkiAdi };
                            var yetki = crm.Yetkis.FirstOrDefault(x => x.yetkiID == sonuc.YetiID);

                            MyTools.ActiveCalisan = sonuc;

                            if (yetki.YetkiAdi == null)
                                return;
                            switch (yetki.YetkiAdi)
                            {
                                case "Admin":
                                case "TeknikServis":
                                case "Muhasebe":
                                    //  AdminViews.formAdmin frmadmin = new AdminViews.formAdmin();
                                    AdminViews.formAdminAra frmara = new AdminViews.formAdminAra();
                                    frmara.Tag = sonuc;//.calisanID;
                                    frmara.Show();
                                    this.Visible = false;
                                    break;
                                case "Server":
                                    ServerViews.Server frmsrv = new ServerViews.Server();
                                    frmsrv.Tag = sonuc;
                                    frmsrv.Show();
                                    this.Visible = false;
                                    break;
                                case "Pazarlama":
                                    AdminViews.formAdminAra frm = new AdminViews.formAdminAra();
                                    frm.Tag = sonuc;
                                    frm.Show();
                                    this.Visible = false;
                                    break;

                                default:
                                    break;
                            }


                        }
                        else
                        {
                            MessageBox.Show("Şifreyi Yanlış Girdiniz");
                        }

                    }
                    else
                    {
                        MessageBox.Show(txtUsername.Text + " Tanımlı Değil ! ");
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEscape_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnUnuttum_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                MessageBox.Show("Bir kullanıcı adı griniz");
            }
            else
            {
                using (crmDFNDataContext crm = new crmDFNDataContext())
                {
                    if (crm.Calisans.Where(x => x.UserName == txtUsername.Text).Any())
                    {

                        var calisan = crm.Calisans.FirstOrDefault(x => x.UserName == txtUsername.Text);
                        if (calisan != null)
                        {
                            var text = "Kullanıcı adınız : " + calisan.UserName + "\n"
                                      + "Şifreniz        : " + MyTools.Sifreleme.Decryp(calisan.Password) + "\n";




                            if (calisan.Iletisim.email == null)
                                MessageBox.Show("Sistemde email bilgisi bulunmuyor");
                            var sonuc = MyTools.MailGonder(calisan.Iletisim.email, text);

                            if (sonuc == "ok")
                                MessageBox.Show("Kullanıcı bilgileriniz Sistemde Kayıtlı Mail adresinize gönderilmiştir.");

                        }


                    }
                    else
                        MessageBox.Show("Girmiş olduğunuz kullanıcı Sistemde Bulunmuyor\nLütfen geçerli bir kullanıcı adı giriniz.");
                }

            }


        }

        private void btnChange1_Click(object sender, EventArgs e)
        {
            btnChange1.Visible = false;
            var count = formLogin.ActiveForm.Size.Height + 85;
            for (int i = formLogin.ActiveForm.Size.Height; i < count; i++)
            {
                //  var hei = formLogin.ActiveForm.Size.Height + i;
                formLogin.ActiveForm.Size = new System.Drawing.Size(formLogin.ActiveForm.Size.Width, i);
            }

            compnetGizleGoster(true);
            txtOldSifre.Select();

        }

        public void compnetGizleGoster(bool state)
        {
            lblNewSifre.Visible = state;
            lblOldSifre.Visible = state;
            txtNewSifre.Visible = state;
            txtOldSifre.Visible = state;
            btnChangeESC.Visible = state;
            btnChange2.Visible = state;

        }


        public void kapat()
        {
            var count = formLogin.ActiveForm.Size.Height - 85;
            for (int i = formLogin.ActiveForm.Size.Height; i > count; i--)
            {
                //  var hei = formLogin.ActiveForm.Size.Height + i;
                formLogin.ActiveForm.Size = new System.Drawing.Size(formLogin.ActiveForm.Size.Width, i);
            }
            btnChange1.Visible = true;
            compnetGizleGoster(false);
            if (txtPassword.Text == "")
                txtPassword.Select();
            else
                btnLogin.Select();
        }
        private void btnChangeESC_Click(object sender, EventArgs e)
        {
            kapat();
        }

        private void btnChange2_Click(object sender, EventArgs e)
        {

            crmDFNDataContext crm = new crmDFNDataContext();

            if (crm.Calisans.Where(x => x.UserName == txtUsername.Text).Any())
            {
                var cal = crm.Calisans.FirstOrDefault(x => x.UserName == txtUsername.Text);

                if (MyTools.Sifreleme.Decryp(cal.Password) == txtOldSifre.Text)
                {
                    cal.Password = MyTools.Sifreleme.Encryp(txtNewSifre.Text);
                    crm.SubmitChanges();
                    MessageBox.Show("Şifreniz Değiştirilmiştir");

                    AdminViews.formAdminAra frm = new AdminViews.formAdminAra();
                    frm.Tag = cal;
                    frm.Show();
                    this.Visible = false;
                }
                else
                {
                    MessageBox.Show("Eski şifrenizi Kotrol edin");
                }



            }
            else
            {
                MessageBox.Show("Bu kullanıcı adı Sistemde bulunmuyor \nKullanıcı adınızı kotrolediniz");
            }


        }

        private void formLogin_FormClosed(object sender, FormClosedEventArgs e)
        {

            var ayarlarpath = Application.StartupPath + @"\ayarlar.ini";

            MyTools.AyarYaz("Kurum", "KurumKod", MyTools.KurumKod, ayarlarpath);
            MyTools.AyarYaz("Kurum", "KurumAdı", MyTools.KurumAd, ayarlarpath);
          // MyTools.AyarYaz("Kurum", "HisseSinyal", MyTools.HisseSinyal, ayarlarpath); //hisse sinyal

            MyTools.ConnectionStingAyarlariYaz();

        }

        private void btnSQL_Click(object sender, EventArgs e)
        {
            var frm = new formSQLAyar();
            frm.Show();
        }
    }
}
