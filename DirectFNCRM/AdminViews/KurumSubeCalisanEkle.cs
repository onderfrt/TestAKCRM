using System;

using System.Collections.Generic;

using System.ComponentModel;

using System.Data;

using System.Diagnostics;

using System.Drawing;

using System.IO;

using System.Linq;

using System.Text;

using System.Threading;

using System.Threading.Tasks;

using System.Windows.Forms;



namespace DirectFNCRM.AdminViews

{

    public partial class KurumSubeCalisanEkle : Form

    {

        public KurumSubeCalisanEkle()

        {

            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;



        }

        string soneklenencalisan = "";

        string soneklenenkurumsube = "";

        List<string> EklenemyenKullanicilar = new List<string>();

        private void SubeOrnekDosya_Click(object sender, EventArgs e)

        {



            string fileName = "SubeTemsilciGuncelleme.txt";

            string writeText = "Username;Sube;Temsilci;KurumHesapNo";





            DosyaVarmi(fileName, writeText);





        }

        static void txtyaz(string fileName, string writeText)

        {

            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

            fs.Close();

            File.AppendAllText(fileName, Environment.NewLine + writeText);

            Process.Start(fileName);

        }



        private void CalisanOrnek_Click(object sender, EventArgs e)

        {

            string fileName = "CalisanEkleme.txt";

            string writeText = "ad;soyad;username;password;departman(Yaz?l?m-Pazarlama);yetki(Admin-Kullan?c?-Server-KurumRapor-TeknikServis);acikadres;Ulke;Il;Ilce;Tel1;Tel2;Ceptel;Email;CalisanTip(personel-KurumPersone-KurumY?netici-KurumSubePersonel-KurumSubeY?neticisi);calisandurum(true-false);KurumMusteriNo(10158);KurumSube(Merkez);TemsilciKod(209);";


            DosyaVarmi(fileName, writeText);



        }



        private void SubeKurumGuncelle_Click(object sender, EventArgs e)

        {
            DialogResult result2 = MessageBox.Show("Müşterileri güncellemek istiyor musun?", "Müşterileri Güncelle", MessageBoxButtons.YesNo);
            if (result2 == DialogResult.Yes)
            {
                try

                {
                    MüsterileriGüncelle();

                }

                catch (Exception)

                {

                    MyTools.dosyaeklelogyaz("Bu username de:" + soneklenenkurumsube + " bir hata oluştu.");

                }
            }





        }


        private void MüsterileriGüncelle()
        {
            var subethread = new Thread(new ThreadStart(() =>

            {



                if (lblDosyaYol.Text == "")

                {

                    MessageBox.Show("Dosya Yolu seçilemedi ! ");

                }

                else

                {

                    string DosyaYolu = lblDosyaYol.Text;

                    string DosyaAdi = lblDosyaAd.Text;







                    var yeni1 = File.ReadLines(DosyaYolu);

                    List<string> usernamesubetemsilci = new List<string>();

                    foreach (var item in yeni1)

                    {

                        usernamesubetemsilci.Add(item);

                    }





                    List<string> username = new List<string>();

                    List<string> sube = new List<string>();

                    List<string> temsilci = new List<string>();

                    List<string> KurumHesapNo = new List<string>();



                    foreach (var word in usernamesubetemsilci)

                    {

                        var a = word.Split(';');

                        username.Add(a[0].ToString());

                        sube.Add(a[1].ToString());

                        temsilci.Add(a[2].ToString());

                        KurumHesapNo.Add(a[3].ToString());



                    }





                    crmDFNDataContext crm = new crmDFNDataContext();




                    progressBar1.Value = 0;
                    for (int i = 1; i < usernamesubetemsilci.Count; i++)

                    {

                        label7.Text = "Yapılan işlem sayısı :" + i;
                        progressBar1.Visible = true;

                        progressBar1.Maximum = usernamesubetemsilci.Count;
                        progressBar1.Value += 1;


                        if (crm.Users.Where(x => x.UserName == username[i].ToString()).Any())

                        {


                            var sonuc = crm.Users.FirstOrDefault(x => x.UserName == username[i].ToString());
                            var sonuc2 = crm.KurumsalBilgilers.FirstOrDefault(x => x.Id == sonuc.KurumsalBilgilerId);
                            if (sonuc.FXkurum != temsilci[i])
                            {
                                sonuc.FXkurum = temsilci[i];
                                var bekleyenGuncellemeler = crm.GetChangeSet().Updates;
                                crm.SubmitChanges();

                            }
                            if (sonuc.KurumsalBilgilerId == null)

                            {

                                KurumsalBilgiler yeni = new KurumsalBilgiler();
                                yeni.KurumSube = sube[i].ToString();
                                yeni.kurumhesapno = KurumHesapNo[i];
                                crm.KurumsalBilgilers.InsertOnSubmit(yeni);
                                crm.SubmitChanges();
                                sonuc.KurumsalBilgilerId = yeni.Id;
                                sonuc.FXkurum = temsilci[i].ToString();

                                crm.SubmitChanges();
                                MyTools.dosyaeklelogyaz("Username: " + sonuc.UserName + "; " + "Userid :" + sonuc.UserID + "  " + yeni.Id.ToString() + "   ss  " + " KurumBilgileri olu?turuldu girildi");
                                //  MessageBox.Show("Username " + sonuc.UserName + ";" + "Userid" + sonuc.UserID + yeni.Id.ToString() + "   ss  " + " KurumBilgileri olu?turuldu girildi");
                            }

                            else if (crm.KurumsalBilgilers.Where(x => x.Id == sonuc.KurumsalBilgilerId && x.KurumSube != sube[i]).Any())
                            {

                                var girilecekBilgi = crm.KurumsalBilgilers.FirstOrDefault(x => x.Id == sonuc.KurumsalBilgilerId);

                                girilecekBilgi.KurumSube = sube[i].ToString();

                                girilecekBilgi.kurumhesapno = KurumHesapNo[i];

                                sonuc.FXkurum = temsilci[i].ToString();

                                var bekleyenGuncellemeler = crm.GetChangeSet().Updates;

                                crm.SubmitChanges();

                                // MessageBox.Show(girilecekBilgi.Id.ToString() + "KurumSube Dolduruldu");

                                MyTools.dosyaeklelogyaz(girilecekBilgi.Id.ToString() + "KurumSube Dolduruldu");



                            }

                            else

                            {



                                MyTools.dosyaeklelogyaz("Şube güncellenirken hata oluş?tu : " + "\n" + "Bu Username ' " + username[i] + " ' mevcut değildir");

                                EklenemyenKullanicilar.Add(username[i]);

                                if (EklenemyenKullanicilar.Count > 1)

                                {

                                    lbleklemeYapilamayankullanici.Visible = true;



                                }



                                // MessageBox.Show("Mevcut de?ildir.");

                            }



                        }



                    }

                    Finalize("Username");
                }
            }

            ));
            subethread.Start();
        }


        private void MüsterilerinAdresleriniGüncelle()
        {
            var subethread = new Thread(new ThreadStart(() =>

            {



                if (lblDosyaYol.Text == "")

                {

                    MessageBox.Show("Dosya Yolu seçilemedi ! ");

                }

                else

                {

                    string DosyaYolu = lblDosyaYol.Text;

                    string DosyaAdi = lblDosyaAd.Text;







                    var yeni1 = File.ReadLines(DosyaYolu);

                    List<string> Adress = new List<string>();

                    foreach (var item in yeni1)

                    {

                        Adress.Add(item);

                    }



                    List<string> username = new List<string>();
                    List<string> adres = new List<string>();
                    List<string> Il = new List<string>();
                    List<string> Ulke = new List<string>();



                    foreach (var word in Adress)

                    {

                        var a = word.Split(';');

                        username.Add(a[0].ToString());

                        adres.Add(a[1].ToString());

                        Il.Add(a[2].ToString());

                        Ulke.Add(a[3].ToString());



                    }





                    crmDFNDataContext crm = new crmDFNDataContext();




                    progressBar1.Value = 0;
                    for (int i = 1; i < Adress.Count; i++)

                    {

                        label7.Text = "Yapılan işlem sayısı :" + i;
                        progressBar1.Visible = true;

                        progressBar1.Maximum = Adress.Count;
                        progressBar1.Value += 1;


                        if (crm.Users.Where(x => x.UserName == username[i].ToString()).Any())
                        {
                            var sonuc = crm.Users.FirstOrDefault(x => x.UserName == username[i].ToString());
                            var sonuc2 = crm.Iletisims.FirstOrDefault(x => x.IletisimId == sonuc.iletisimId);
                            // var sonuc2 = crm.KurumsalBilgilers.FirstOrDefault(x => x.Id == sonuc.KurumsalBilgilerId);
                            var ulkeId = crm.Ulkes.FirstOrDefault(x => x.tr_TR == Ulke[i]);
                            var ilId = crm.Ils.FirstOrDefault(x => x.IlAdi == Il[i]);

                          
                            sonuc2.acikadres = adres[i];
                            if (ulkeId != null)
                            {
                                sonuc2.UlkeId = ulkeId.Id;

                            }
                            else
                            {
                                MyTools.dosyaeklelogyaz("Ülke  bulunamadı :" + Ulke[i]);

                            }
                            if (ilId != null)
                            {
                                sonuc2.IlId = ilId.Id;

                            }
                            else
                            {
                                MyTools.dosyaeklelogyaz("Il bulunamadı :" + Il[i]);

                            }

                            crm.SubmitChanges();
                            MyTools.dosyaeklelogyaz("Username: " + sonuc.UserName + "; " + "Userid :" + sonuc.UserID + "  " + " " + " Yeni il olarak :" + Il[i].ToString() + " Yeni Ülke olarak :" + Ulke[i].ToString());
                        }

                    }
                    Finalize("Username");

                }
            }));
            subethread.Start();
        }
        private void Finalize(string items)
        {
            listBox3.Visible = true;
            listBox3.Items.Add(items);
            listBox3.Items.Add(EklenemyenKullanicilar);
            label7.Text = "işlemi bitmiştir.";
            MessageBox.Show("Tamamlandı.");
        }

        private void CalisanGuncelle_Click(object sender, EventArgs e)

        {

            try

            {
                var calisanthread = new Thread(new ThreadStart(() =>

                {
                    if (lblDosyaYol.Text == "")

                    {

                        MessageBox.Show("Dosya Yolu seçilemedi!");

                    }

                    else

                    {


                        if (lblDosyaYol.Text == "")

                        {

                            MessageBox.Show("Dosya Yolu seçilemedi ! ");

                        }

                        else

                        {

                            string DosyaYolu = lblDosyaYol.Text;

                            string DosyaAdi = lblDosyaAd.Text;

                            var yeni1 = File.ReadLines(DosyaYolu);

                            List<string> usernamesubetemsilci = new List<string>();

                            foreach (var item in yeni1)

                            {

                                usernamesubetemsilci.Add(item);

                            }

                            List<string> ad = new List<string>();

                            List<string> soyad = new List<string>();

                            List<string> username = new List<string>();

                            List<string> password = new List<string>();

                            List<string> departmanId = new List<string>();

                            List<string> yetkiId = new List<string>();

                            //acikadres;Ulke;Il;Ilce;Tel1;Tel2;Ceptel;Email

                            List<string> acikadres = new List<string>();

                            List<string> Ulke = new List<string>();

                            List<string> Il = new List<string>();

                            List<string> Ilce = new List<string>();

                            List<string> Tel1 = new List<string>();

                            List<string> Tel2 = new List<string>();

                            List<string> Ceptel = new List<string>();

                            List<string> Email = new List<string>();

                            List<string> calisanTip = new List<string>();

                            List<string> calisanDurum = new List<string>();

                            List<string> kurumMusteriNo = new List<string>();

                            List<int> sifrehata = new List<int>();

                            List<string> kurumSube = new List<string>();

                            List<string> temsilciKodu = new List<string>();



                            foreach (var word in usernamesubetemsilci)

                            {

                                var a = word.Split(';');

                                ad.Add(a[0].ToString());

                                soyad.Add(a[1].ToString());

                                username.Add(a[2].ToString());

                                password.Add(a[3].ToString());

                                departmanId.Add(a[4].ToString());

                                yetkiId.Add(a[5].ToString());



                                acikadres.Add(a[6].ToString());

                                Ulke.Add(a[7].ToString());

                                Il.Add(a[8].ToString());

                                Ilce.Add(a[9].ToString());

                                Tel1.Add(a[10].ToString());

                                Tel2.Add(a[11].ToString());

                                Ceptel.Add(a[12].ToString());

                                Email.Add(a[13].ToString());



                                calisanTip.Add(a[14].ToString());

                                calisanDurum.Add(a[15].ToString());

                                kurumMusteriNo.Add(a[16].ToString());



                                kurumSube.Add(a[17].ToString());

                                temsilciKodu.Add(a[18].ToString());

                            }



                            // ?letisimId

                            // Acikadres

                            // UlkeId

                            // IlId

                            // IlceId

                            // Tel1

                            // Tel2

                            // Ceptel

                            // email



                            //CalisanTip

                            // 1 Personel

                            // 2 KurumOtoCreate

                            // 3 Server

                            // 4 KurumOtoCreate

                            // 5 KurumPersonel

                            // 6 KurumYonetici

                            // 7 KurumSubePersonel

                            // 8 KurumSubeYonetici



                            crmDFNDataContext crm = new crmDFNDataContext();


                            for (int i = 0; i < usernamesubetemsilci.Count; i++)

                            {


                                if (crm.Calisans.Where(x => x.UserName == username[i].ToString()).Any())

                                {

                                    MyTools.dosyaeklelogyaz("Bu kullanıcı adı mevcut. " + " " + username[i] + " Ekleme yapılamadı.");

                                    EklenemyenKullanicilar.Add(username[i]);

                                }

                                else

                                {

                                    soneklenencalisan = username[i];

                                    var yenicalisan = new Calisan();

                                    yenicalisan.Ad = ad[i];

                                    yenicalisan.Soyad = soyad[i];

                                    yenicalisan.UserName = username[i];

                                    yenicalisan.Password = yenicalisan.Password = MyTools.Sifreleme.Encryp(password[i].ToString()); ;

                                    #region departman

                                    //DepartmanId

                                    //     // 1 Yaz?l?m

                                    //     // 2 Teknik Servis

                                    //     // 3 Pazarlama 

                                    //     // 4 Muhasebe

                                    var departmanal = crm.Departmans.FirstOrDefault(x => x.DepartmanAdi == departmanId[i]);

                                    yenicalisan.Departman = departmanal;

                                    //yenicalisan.DepartmanId = departmanal.DepartmanId;

                                    #endregion

                                    #region Yetki

                                    // YetliId

                                    // 1 Admin 

                                    // 2 Kullan?c? 

                                    // 3 Server

                                    // 4 Kurum Rapor 

                                    // 5 Teknik Servis

                                    var yetkial = crm.Yetkis.FirstOrDefault(x => x.YetkiAdi == yetkiId[i]);

                                    yenicalisan.Yetki = yetkial;

                                    // yenicalisan.Yetki= yetkial.YetkiAdi;

                                    #endregion

                                    //acikadres; Ulke; Il; Ilce; Tel1; Tel2; Ceptel; Email

                                    #region iletisim

                                    var ilet = new Iletisim();

                                    ilet.acikadres = acikadres[i];



                                    var ulke1 = crm.Ulkes.FirstOrDefault(x => x.UlkeAdi == Ulke[i]);

                                    var Il1 = crm.Ils.FirstOrDefault(x => x.IlAdi == Il[i]);

                                    var Ilce2 = crm.Ilces.FirstOrDefault(x => x.IlceAdi == Ilce[i]);



                                    ilet.Ulke = ulke1;

                                    ilet.Il = Il1;

                                    ilet.Ilce = Ilce2;



                                    ilet.Tel1 = Tel1[i];

                                    ilet.Tel2 = Tel2[i];

                                    ilet.Ceptel = Ceptel[i];

                                    ilet.email = Email[i];



                                    crm.Iletisims.InsertOnSubmit(ilet);

                                    crm.SubmitChanges();



                                    yenicalisan.iletsimId = ilet.IletisimId;

                                    #endregion

                                    #region calisantip

                                    var calisantipal = new CalisanTip();

                                    calisantipal = crm.CalisanTips.FirstOrDefault(x => x.CalisanTipAdi == calisanTip[i]);

                                    yenicalisan.CalisanTip = calisantipal;



                                    #endregion


                                    yenicalisan.kurumMutserino = kurumMusteriNo[i];

                                    yenicalisan.tel = Ceptel[i];

                                    yenicalisan.sifrehata = 0;

                                    yenicalisan.TemsilciKodu = temsilciKodu[i];

                                    yenicalisan.CalisanDurum = true;

                                    if (crm.KurumSubes.Where(x => x.SubeAdi == kurumSube[i]).Any())

                                    {

                                        var kurumsubeal = crm.KurumSubes.FirstOrDefault(x => x.SubeAdi == kurumSube[i]);

                                        yenicalisan.KurumSube = kurumsubeal.SubeAdi;

                                    }

                                    //crm.Calisan.InsertOnSubmit(yenicalisan);

                                    crm.SubmitChanges();

                                    MyTools.dosyaeklelogyaz("Bu kullanıcı kayıt edildi." + username[i]);

                                }



                            }


                            Finalize("Çalışan");

                        }



                    }
                }

                ));
                calisanthread.Start();
            }

            catch (Exception)

            {

                MyTools.dosyaeklelogyaz("Username i :" + soneklenencalisan + " olan kullanıcıda hata oluştu.");

            }



            MessageBox.Show("Tamamlandı.");

        }

        public void DosyaVarmi(string dosyaad, string ornekyazi)

        {

            if (!File.Exists(dosyaad))

            {

                using (StreamWriter sw = File.CreateText(dosyaad))

                {

                    sw.Write(ornekyazi);

                    Process.Start(dosyaad);

                }

            }

            else

            {

                File.Delete(dosyaad);

                using (StreamWriter sw = File.CreateText(dosyaad))

                {

                    sw.Write(ornekyazi);

                    Process.Start(dosyaad);

                }

            }

        }


        private void SubeDosyaSec_Click(object sender, EventArgs e)

        {

            try

            {

                //99

                OpenFileDialog file = new OpenFileDialog();

                file.FilterIndex = 2;

                file.RestoreDirectory = true;

                file.CheckFileExists = false;

                file.Title = "TXT DOSYASINI SEÇİNİZ..";

                file.ShowDialog();

                listBox1.Items.Clear();

                listBox2.Items.Clear();

                if (FileisSelected(file))

                {
                    List<string> hangidosya = new List<string>();

                    var yeni12 = File.ReadLines(file.FileName);

                    foreach (var item in yeni12)

                    {

                        hangidosya.Add(item);

                    }

                    var a = hangidosya[0].Split(';');


                    if (a[1] == "Adres")
                    {

                        listBox2.Items.Clear();

                        lblDosyaYol.Text = file.FileName;

                        lblDosyaAd.Text = file.SafeFileName;

                        var yeni1 = File.ReadLines(file.FileName);

                        List<string> Adress = new List<string>();

                        List<string> username = new List<string>();




                        foreach (var item in yeni1)
                        {
                            Adress.Add(item);
                        }
                        foreach (var word in Adress)
                        {
                            var aa = word.Split(';');
                            username.Add(aa[0].ToString());
                        }
                        foreach (var item in username)
                        {
                            listBox1.Items.Add(item);
                        }


                        lblKullaniciSayisi.Text = Adress.Count.ToString();
                    }

                    if (a[1] == "Sube")

                    {
                        try

                        {

                            listBox2.Items.Clear();

                            lblDosyaYol.Text = file.FileName;

                            lblDosyaAd.Text = file.SafeFileName;

                            var yeni1 = File.ReadLines(file.FileName);

                            List<string> usernamesubetemsilci = new List<string>();
                            List<string> username = new List<string>();



                            foreach (var item in yeni1)
                            {
                                usernamesubetemsilci.Add(item);
                            }
                            foreach (var word in usernamesubetemsilci)
                            {
                                var aa = word.Split(';');
                                username.Add(aa[0].ToString());
                            }
                            foreach (var item in username)
                            {
                                listBox1.Items.Add(item);
                            }


                            lblKullaniciSayisi.Text = usernamesubetemsilci.Count.ToString();

                        }

                        catch (Exception)

                        {
                            MessageBox.Show("Dosya Seçilmedi.");

                        }

                    }

                    if (a[1] == "soyad")

                    {

                        try

                        {

                            listBox2.Items.Clear();


                            lblDosyaYol.Text = file.FileName;

                            lblDosyaAd.Text = file.SafeFileName;

                            var yeni1 = File.ReadLines(lblDosyaYol.Text);

                            List<string> usernamesubetemsilci = new List<string>();

                            foreach (var item in yeni1)

                            {

                                usernamesubetemsilci.Add(item);

                            }

                            List<string> ad = new List<string>();

                            List<string> soyad = new List<string>();

                            List<string> username = new List<string>();



                            foreach (var word in usernamesubetemsilci)

                            {

                                var a2 = word.Split(';');

                                ad.Add(a2[0].ToString());

                                soyad.Add(a2[1].ToString());

                                username.Add(a2[2].ToString());

                            }


                            for (int i = 0; i < usernamesubetemsilci.Count; i++)

                            {

                                listBox2.Items.Add(ad[i] + " " + soyad[i] + " " + username[i]);

                            }

                            lblCalisansayisi.Text = usernamesubetemsilci.Count.ToString();


                        }

                        catch (Exception)

                        {

                            MessageBox.Show("Dosya Seçilmedi.");

                        }

                    }



                }


            }

            catch (Exception)

            {

                throw;

            }



        }

        private void KurumSubeCalisanEkle_Load(object sender, EventArgs e)
        {
            Temizle();

        }
        private void btnTemizle_Click(object sender, EventArgs e)

        {
            Temizle();

        }

        private void Temizle()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            lblDosyaYol.Text = "";
            lblDosyaAd.Text = "";
            lblCalisansayisi.Text = "";
            lblKullaniciSayisi.Text = "";
            lbleklemeYapilamayankullanici.Visible = false;
            listBox3.Visible = false;
            progressBar1.Value = 0;
            progressBar1.Visible = false;
        }

        private bool FileisSelected(OpenFileDialog file)

        {

            if (file.FileName == "")

            {

                return false;

            }

            return true;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string fileName = "AdresGuncelle.txt";

            string writeText = "username;Adres;Il;Ulke";

            DosyaVarmi(fileName, writeText);


        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result2 = MessageBox.Show("Müşterileri güncellemek istiyor musun?", "Müşterileri Güncelle", MessageBoxButtons.YesNo);
            if (result2 == DialogResult.Yes)
            {
                try

                {
                    MüsterilerinAdresleriniGüncelle();

                }

                catch (Exception)

                {

                    MyTools.dosyaeklelogyaz("Bu username de:" + soneklenenkurumsube + " bir hata oluştu.");

                }
            }
        }
    }



}

