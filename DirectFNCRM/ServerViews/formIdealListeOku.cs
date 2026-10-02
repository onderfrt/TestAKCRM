using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Data.OleDb;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Excel = Microsoft.Office.Interop.Excel;
using System.Xml;

namespace DirectFNCRM.ServerViews
{
    public partial class formIdealListeOku : Form
    {
        public formIdealListeOku()
        {
            InitializeComponent();
        }

        ArrayOfXmlCustomer users = new ArrayOfXmlCustomer();

        
   
        private void formIdealListeOku_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;         

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {


            try
            {
                OpenFileDialog op = new OpenFileDialog();
                var filename = "";
                op.InitialDirectory = Application.StartupPath;
                op.Filter = "(*.xml)|*.xml";

                if (op.ShowDialog() == DialogResult.OK)
                {
                    filename = op.FileName;

                }
                else
                    return;


                FileStream fs = new FileStream(filename, FileMode.Open);

                XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfXmlCustomer));

                users = (ArrayOfXmlCustomer)serializer.Deserialize(fs);

                fs.Close();


                dataGridView1.DataSource = users.XmlCustomer;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
        }

       

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {

            

        }

        public void writetosql(cxCustomer gelenuser)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                UserEvent userevent = new UserEvent();
                userevent.CalisanId = Server.referance.ActiveCalisan.calisanID;
                userevent.EventTarih = DateTime.Now;
                userevent.IP = Server.referance.IpAdress;
                userevent.HostName = Server.referance.HostName;
                userevent.EventTypeId = 8;
                var sozlesme = new Sozlesmeler();



                if (crm.Users.Where(x => x.UserName == gelenuser.Username).Any())
                {

                    var user = crm.Users.FirstOrDefault(x => x.UserName == gelenuser.Username);

                    Iletisim iletisim = new Iletisim();
                    UserExtraInfo extrainfo = new UserExtraInfo();
                    LisansDurum lisanslar = new LisansDurum();
                    KurumsalBilgiler kurumsalbilgi = new KurumsalBilgiler();

                    #region isimolusturma

                    var gelenisim = gelenuser.Isim.Trim().Split(' ');
                    var ad = "";
                    var soyad = "";
                    if (gelenisim.Length == 1)
                    {
                        ad = gelenisim[0];
                        soyad = "yok";

                    }
                    else if (gelenisim.Length == 2)
                    {
                        ad = gelenisim[0];
                        soyad = gelenisim[1];

                    }
                    else if (gelenisim.Length == 3)
                    {
                        ad = gelenisim[0] + " " + gelenisim[1];
                        soyad = gelenisim[2];
                    }

                    user.Name = ad;
                    user.Surname = soyad;



                    #endregion
                    user.Password = MyTools.Sifreleme.Encryp(gelenuser.Password);
                    user.Aciklama = gelenuser.Explanation;
                    user.FXkurum = gelenuser.GhostKurum;


                    

                   // user.SozlesmeNo = user.PmtsNo;
                    user.tckno = gelenuser.KimlikNo;
                    user.ProductType = gelenuser.ProductType;
                    #region KurumsalBilgiler

                    //kurumsal bilgiler
                    if (user.KurumsalBilgilerId == null)
                    {
                        kurumsalbilgi.Not1 = gelenuser.Not1;
                        kurumsalbilgi.KurumSube = gelenuser.Sube;
                        kurumsalbilgi.kurumhesapno = gelenuser.KurumMusteriNo;
                    }
                    else
                    {
                        user.KurumsalBilgiler.Not1 = gelenuser.Not1;
                        user.KurumsalBilgiler.KurumSube = gelenuser.Sube;
                        user.KurumsalBilgiler.kurumhesapno = gelenuser.KurumMusteriNo;
                    }
                    #endregion

                    #region iletisim

                    //iletişim

                    if (user.iletisimId == null)
                    {
                        iletisim.Tel1 = gelenuser.Telefon;
                        iletisim.acikadres = gelenuser.Adres;
                        iletisim.email = gelenuser.Mail;
                        var il = MyTools.SehirIdBul(gelenuser.Sehir);
                        if (il != null && il.Id > 0)
                        {
                            iletisim.IlId = il.Id;
                            iletisim.UlkeId = il.UlkeId;
                        }
                    }
                    else
                    {
                        user.Iletisim.Tel1 = gelenuser.Telefon;
                        user.Iletisim.acikadres = gelenuser.Adres;
                        user.Iletisim.email = gelenuser.Mail;
                        var il = MyTools.SehirIdBul(gelenuser.Sehir);
                        if (il != null && il.Id > 0)
                        {
                            user.Iletisim.IlId = il.Id;
                            user.Iletisim.UlkeId = il.UlkeId;
                        }
                    }
                    #endregion


                    #region Extrainfos

                    if (user.UserExtraInfoId == null)
                    {

                        //userextrainfo

                        extrainfo.IP = gelenuser.IP;
                        extrainfo.SozlesmeOnayDate = gelenuser.SozlesmeKabulDateStr;
                        extrainfo.MesajOnayDate = gelenuser.DisclaimerKabulDateStr;
                        extrainfo.Not1 = gelenuser.Not1;
                        extrainfo.Not2 = gelenuser.Not2;
                        extrainfo.Not3 = gelenuser.Not3;
                        extrainfo.RaporWebBuAyLoginSayisi = gelenuser.RaporWebBuAyLoginSayisi;
                        extrainfo.RaporWebOncekiAyLoginSayisi = gelenuser.RaporWebOncekiAyLoginSayisi;
                        extrainfo.RaporWebSonLoginTarihi = gelenuser.RaporWebSonLoginTarihi;
                        extrainfo.RaporWebCihaz = gelenuser.RaporWebCihaz;
                        extrainfo.RaporWebUrun = gelenuser.RaporWebUrun;
                        extrainfo.WebMesajGrup = gelenuser.WebMesajGrup;
                        extrainfo.WebMesajKurum = gelenuser.WebMesajKurum;

                        extrainfo.ComputerName = gelenuser.MachineName;

                    }
                    else
                    {


                        //userextrainfo

                        user.UserExtraInfo.IP = gelenuser.IP;
                        user.UserExtraInfo.SozlesmeOnayDate = gelenuser.SozlesmeKabulDateStr;
                        user.UserExtraInfo.MesajOnayDate = gelenuser.DisclaimerKabulDateStr;
                        user.UserExtraInfo.Not1 = gelenuser.Not1;
                        user.UserExtraInfo.Not2 = gelenuser.Not2;
                        user.UserExtraInfo.Not3 = gelenuser.Not3;
                        user.UserExtraInfo.RaporWebBuAyLoginSayisi = gelenuser.RaporWebBuAyLoginSayisi;
                        user.UserExtraInfo.RaporWebOncekiAyLoginSayisi = gelenuser.RaporWebOncekiAyLoginSayisi;
                        user.UserExtraInfo.RaporWebSonLoginTarihi = gelenuser.RaporWebSonLoginTarihi;
                        user.UserExtraInfo.RaporWebCihaz = gelenuser.RaporWebCihaz;
                        user.UserExtraInfo.RaporWebUrun = gelenuser.RaporWebUrun;
                        user.UserExtraInfo.WebMesajGrup = gelenuser.WebMesajGrup;
                        user.UserExtraInfo.WebMesajKurum = gelenuser.WebMesajKurum;

                        user.UserExtraInfo.ComputerName = gelenuser.MachineName;

                    }

                    #endregion






                        user.StatusId = 1;
               

                   
                    user.MusteriMenseiID = 1;


                    /// başlangıç tariihini çözme
                    /// 

                    var tarih = gelenuser.Expiry.ToString();
                    if (tarih.Length == 8)
                    {
                        var newDate = DateTime.ParseExact(tarih, "yyyyMMdd", CultureInfo.InvariantCulture);

                        user.ExpiryDate = newDate;

                    }
                    else user.ExpiryDate = DateTime.Now;

                    if (gelenuser.BaslangicTarih == "")
                    {
                        user.BaslangicTarihi = user.ExpiryDate;
                    }
                    else if (gelenuser.BaslangicTarih.Length == 8)
                    {
                        var newDate = DateTime.ParseExact(gelenuser.BaslangicTarih, "yyyyMMdd", CultureInfo.InvariantCulture);
                        user.BaslangicTarihi = newDate;
                    }
                    else if (gelenuser.BaslangicTarih.Length > 9)
                    {

                        var sub = gelenuser.BaslangicTarih.Substring(0, 10);
                        var newDate = DateTime.ParseExact(sub,
                                 "yyyy.MM.dd",
                                  CultureInfo.InvariantCulture);
                        user.BaslangicTarihi = newDate;

                    }                 
                 
                 


                    #region Lisanslar

                    if (user.LisansDurumId == null)
                    {

                        /// lisanslar 
                        /// 

                        lisanslar.YayinDurumu = gelenuser.OnOff;
                        lisanslar.PayL1 = gelenuser.IMKBL1;
                        lisanslar.PayLP = gelenuser.IMKBL1P;
                        lisanslar.PayL2 = gelenuser.IMKBL2;
                        lisanslar.PayX = gelenuser.IMKBX;
                        lisanslar.PayGS = gelenuser.IMKBISL;

                        lisanslar.ViopL1 = gelenuser.VIPL1;
                        lisanslar.ViopLP = gelenuser.VIPL1P;
                        lisanslar.ViopL2 = gelenuser.VIPL2;
                        lisanslar.Vd2P = gelenuser.Vd2P;
                        lisanslar.ViopGS = gelenuser.VIPNET;

                        lisanslar.TahvilL1 = gelenuser.THVL1;
                        lisanslar.TahvilLP = gelenuser.THVL1P;
                        lisanslar.TahvilL2 = gelenuser.THVL2;

                        lisanslar.AnPro = gelenuser.ANPRO;

                        lisanslar.CepYetki = gelenuser.CEP._ToBool();
                        lisanslar.ProYetki = gelenuser.PRO._ToBool();
                        lisanslar.ROBOT = gelenuser.Robot;

                        lisanslar.Futgck = true;
                        lisanslar.WINX = true;

                        lisanslar.Amex = gelenuser.AMEX;
                        lisanslar.CBOT = gelenuser.CBOT;
                        lisanslar.CBOTM = gelenuser.CBOTM;
                        lisanslar.CHIX = gelenuser.CHIX;
                        lisanslar.CME = gelenuser.CME;
                        lisanslar.CMEM = gelenuser.CMEM;
                        lisanslar.COMEX = gelenuser.COMEX;
                        lisanslar.DJI = gelenuser.DJI;
                        lisanslar.EUREX = gelenuser.EUREX;
                        lisanslar.XETRA = gelenuser.XETRA;
                        lisanslar.LSE = gelenuser.LSE;
                        lisanslar.NASDAQ = gelenuser.NASDAQ;
                        lisanslar.NYMEX = gelenuser.NYMEX;
                        lisanslar.NYMEXM = gelenuser.NYMEXM;
                        lisanslar.NYSE = gelenuser.NYSE;
                        lisanslar.SPI = gelenuser.SPI;

                    }
                    else
                    {


                        user.LisansDurum.YayinDurumu = gelenuser.OnOff;
                        user.LisansDurum.PayL1 = gelenuser.IMKBL1;
                        user.LisansDurum.PayLP = gelenuser.IMKBL1P;
                        user.LisansDurum.PayL2 = gelenuser.IMKBL2;
                        user.LisansDurum.Pd2P = gelenuser.IMKBL2P;
                        user.LisansDurum.PayX = gelenuser.IMKBX;
                        user.LisansDurum.PayGS = gelenuser.IMKBISL;

                        user.LisansDurum.ViopL1 = gelenuser.VIPL1;
                        user.LisansDurum.ViopLP = gelenuser.VIPL1P;
                        user.LisansDurum.ViopL2 = gelenuser.VIPL2;
                        user.LisansDurum.Vd2P = gelenuser.Vd2P;
                        user.LisansDurum.ViopGS = gelenuser.VIPNET;

                        user.LisansDurum.TahvilL1 = gelenuser.THVL1;
                        user.LisansDurum.TahvilLP = gelenuser.THVL1P;
                        user.LisansDurum.TahvilL2 = gelenuser.THVL2;

                        user.LisansDurum.AnPro = gelenuser.ANPRO;

                        user.LisansDurum.CepYetki = gelenuser.CEP._ToBool();
                        user.LisansDurum.ProYetki = gelenuser.PRO._ToBool();
                        user.LisansDurum.ROBOT = gelenuser.Robot;

                        user.LisansDurum.Futgck = true;
                        user.LisansDurum.WINX = true;

                        user.LisansDurum.Amex = gelenuser.AMEX;
                        user.LisansDurum.CBOT = gelenuser.CBOT;
                        user.LisansDurum.CBOTM = gelenuser.CBOTM;
                        user.LisansDurum.CHIX = gelenuser.CHIX;
                        user.LisansDurum.CME = gelenuser.CME;
                        user.LisansDurum.CMEM = gelenuser.CMEM;
                        user.LisansDurum.COMEX = gelenuser.COMEX;
                        user.LisansDurum.DJI = gelenuser.DJI;
                        user.LisansDurum.EUREX = gelenuser.EUREX;
                        user.LisansDurum.XETRA = gelenuser.XETRA;
                        user.LisansDurum.LSE = gelenuser.LSE;
                        user.LisansDurum.NASDAQ = gelenuser.NASDAQ;
                        user.LisansDurum.NYMEX = gelenuser.NYMEX;
                        user.LisansDurum.NYMEXM = gelenuser.NYMEXM;
                        user.LisansDurum.NYSE = gelenuser.NYSE;
                        user.LisansDurum.SPI = gelenuser.SPI;
                    }



                    if (iletisim.IletisimId > 0)
                    {
                        crm.Iletisims.InsertOnSubmit(iletisim);
                        crm.SubmitChanges();

                        user.iletisimId = iletisim.IletisimId;
                    }



                    if (extrainfo.id > 0)
                    {

                        crm.UserExtraInfos.InsertOnSubmit(extrainfo);
                        crm.SubmitChanges();
                        user.UserExtraInfoId = extrainfo.id;


                    }

                    if (kurumsalbilgi.Id > 0)
                    {
                        crm.KurumsalBilgilers.InsertOnSubmit(kurumsalbilgi);
                        crm.SubmitChanges();
                        user.KurumsalBilgilerId = kurumsalbilgi.Id;



                    }

                    if (lisanslar.LisansDurumId > 0)
                    {
                        crm.LisansDurums.InsertOnSubmit(lisanslar);
                        crm.SubmitChanges();
                        user.LisansDurumId = lisanslar.LisansDurumId;

                    }
                    

                    crm.SubmitChanges();

                    #endregion







                }
                else
                {
                   

                    User user = new User();
                    Iletisim iletisim = new Iletisim();
                    UserExtraInfo extrainfo = new UserExtraInfo();
                    LisansDurum lisanslar = new LisansDurum();
                    KurumsalBilgiler kurumsalbilgi = new KurumsalBilgiler();

                    user.UserName = gelenuser.Username;
                    #region isimolusturma

                    var gelenisim = gelenuser.Isim.Trim().Split(' ');
                    var ad = "";
                    var soyad = "";
                    if (gelenisim.Length == 1)
                    {
                        ad = gelenisim[0];
                        soyad = "yok";

                    }
                    else if (gelenisim.Length == 2)
                    {
                        ad = gelenisim[0];
                        soyad = gelenisim[1];

                    }
                    else if (gelenisim.Length == 3)
                    {
                        ad = gelenisim[0] + " " + gelenisim[1];
                        soyad = gelenisim[2];
                    }

                    user.Name = ad;
                    user.Surname = soyad;



                    #endregion
                    user.Password = MyTools.Sifreleme.Encryp(gelenuser.Password);
                    user.Aciklama = gelenuser.Explanation; 
                    user.FXkurum = gelenuser.GhostKurum;


                    if(MyTools.KurumKod=="10011")
                    user.PmtsNo = gelenuser.Username;
                    else
                        user.PmtsNo = gelenuser.PmtsNo.ToString();


                    user.tckno = gelenuser.KimlikNo;
                    user.ProductType = gelenuser.ProductType;


                    //kurumsal bilgiler

                    kurumsalbilgi.Not1 = gelenuser.Not1;
                    kurumsalbilgi.KurumSube = gelenuser.Sube;







                    //iletişim

                    iletisim.Tel1 = gelenuser.Telefon;
                    iletisim.acikadres = gelenuser.Adres;
                    iletisim.email = gelenuser.Mail;
                    var il = MyTools.SehirIdBul(gelenuser.Sehir);
                    if (il != null && il.Id > 0)
                    {
                        iletisim.IlId = il.Id;
                        iletisim.UlkeId = il.UlkeId;
                    }
                 
                      user.StatusId = 1;
                    
                 

                  
                    user.MusteriMenseiID = 1;


                    /// başlangıç tariihini çözme
                    /// 

                    if (gelenuser.BaslangicTarih == "")
                    {
                        user.BaslangicTarihi = DateTime.Now;
                    }
                    else if (gelenuser.BaslangicTarih.Length == 8)
                    {
                        var newDate = DateTime.ParseExact(gelenuser.BaslangicTarih, "yyyyMMdd", CultureInfo.InvariantCulture);
                        user.BaslangicTarihi = newDate;
                    }
                    else if (gelenuser.BaslangicTarih.Length > 9)
                    {

                        var sub = gelenuser.BaslangicTarih.Substring(0, 10);
                        var newDate = DateTime.ParseExact(sub,
                                 "yyyy.MM.dd",
                                  CultureInfo.InvariantCulture);
                        user.BaslangicTarihi = newDate;

                    }

                    var tarih = gelenuser.Expiry.ToString();
                    if (tarih.Length == 8)
                    {
                        var newDate = DateTime.ParseExact(tarih, "yyyyMMdd", CultureInfo.InvariantCulture);

                        user.ExpiryDate = newDate;

                    }
                    else user.ExpiryDate = DateTime.Now;

                    //userextrainfo

                    extrainfo.IP = gelenuser.IP;
                    extrainfo.SozlesmeOnayDate = gelenuser.SozlesmeKabulDateStr;
                    extrainfo.MesajOnayDate = gelenuser.DisclaimerKabulDateStr;
                    extrainfo.Not1 = gelenuser.Not1;
                    extrainfo.Not2 = gelenuser.Not2;
                    extrainfo.Not3 = gelenuser.Not3;
                    extrainfo.RaporWebBuAyLoginSayisi = gelenuser.RaporWebBuAyLoginSayisi;
                    extrainfo.RaporWebOncekiAyLoginSayisi = gelenuser.RaporWebOncekiAyLoginSayisi;
                    extrainfo.RaporWebSonLoginTarihi = gelenuser.RaporWebSonLoginTarihi;
                    extrainfo.RaporWebCihaz = gelenuser.RaporWebCihaz;
                    extrainfo.RaporWebUrun = gelenuser.RaporWebUrun;
                    extrainfo.WebMesajGrup = gelenuser.WebMesajGrup;
                    extrainfo.WebMesajKurum = gelenuser.WebMesajKurum;
                 
                    extrainfo.ComputerName = gelenuser.MachineName;


                    /// lisanslar 
                    /// 

                    lisanslar.YayinDurumu = gelenuser.OnOff;
                    lisanslar.PayL1 = gelenuser.IMKBL1;
                    lisanslar.PayLP = gelenuser.IMKBL1P;
                    lisanslar.PayL2 = gelenuser.IMKBL2;
                    lisanslar.Pd2P = gelenuser.IMKBL2P;
                    lisanslar.PayX = gelenuser.IMKBX;
                    lisanslar.PayGS = gelenuser.IMKBISL;

                    lisanslar.ViopL1 = gelenuser.VIPL1;
                    lisanslar.ViopLP = gelenuser.VIPL1P;
                    lisanslar.ViopL2 = gelenuser.VIPL2;
                    lisanslar.Vd2P = gelenuser.Vd2P;
                    lisanslar.ViopGS = gelenuser.VIPNET;

                    lisanslar.TahvilL1 = gelenuser.THVL1;
                    lisanslar.TahvilLP = gelenuser.THVL1P;
                    lisanslar.TahvilL2 = gelenuser.THVL2;

                    lisanslar.AnPro = gelenuser.ANPRO;

                    lisanslar.CepYetki = gelenuser.CEP._ToBool();
                    lisanslar.ProYetki = gelenuser.PRO._ToBool();
                    lisanslar.ROBOT = gelenuser.Robot;

                    lisanslar.Futgck = true;
                    lisanslar.WINX = true;

                    lisanslar.Amex = gelenuser.AMEX;
                    lisanslar.CBOT = gelenuser.CBOT;
                    lisanslar.CBOTM = gelenuser.CBOTM;
                    lisanslar.CHIX = gelenuser.CHIX;
                    lisanslar.CME = gelenuser.CME;
                    lisanslar.CMEM = gelenuser.CMEM;
                    lisanslar.COMEX = gelenuser.COMEX;
                    lisanslar.DJI = gelenuser.DJI;
                    lisanslar.EUREX = gelenuser.EUREX;
                    lisanslar.XETRA = gelenuser.XETRA;
                    lisanslar.LSE = gelenuser.LSE;
                    lisanslar.NASDAQ = gelenuser.NASDAQ;
                    lisanslar.NYMEX = gelenuser.NYMEX;
                    lisanslar.NYMEXM = gelenuser.NYMEXM;
                    lisanslar.NYSE = gelenuser.NYSE;
                    lisanslar.SPI = gelenuser.SPI;



                    crm.UserExtraInfos.InsertOnSubmit(extrainfo);
                    crm.SubmitChanges();
                    user.UserExtraInfoId = extrainfo.id;

                    crm.Iletisims.InsertOnSubmit(iletisim);
                    crm.SubmitChanges();
                    user.iletisimId = iletisim.IletisimId;


                    crm.KurumsalBilgilers.InsertOnSubmit(kurumsalbilgi);
                    crm.SubmitChanges();
                    user.KurumsalBilgilerId = kurumsalbilgi.Id;


                    crm.LisansDurums.InsertOnSubmit(lisanslar);
                    crm.SubmitChanges();
                    user.LisansDurumId = lisanslar.LisansDurumId;

                    crm.Users.InsertOnSubmit(user);
                    crm.SubmitChanges();
                    userevent.SonLisandurumID = user.LisansDurumId;
                    userevent.UserId = user.UserID;
                    crm.UserEvents.InsertOnSubmit(userevent);
                    crm.SubmitChanges();


                    Server.referance.formListeTransaction(user.UserName+"  Eklendi");




                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool BistPayi(ArrayOfXmlCustomerXmlCustomer gelenuser)
        {
            var bistpayi = false;

            #region Bist
            if (gelenuser.IMKBL1._ToBool())
                bistpayi = true;
            if (gelenuser.IMKBL2._ToBool())
                bistpayi = true;
            if (gelenuser.IMKBL2P._ToBool())
                bistpayi = true;
            if (gelenuser.IMKBL1P._ToBool())
                bistpayi = true;
            if (gelenuser.IMKBISL._ToBool())
                bistpayi = true;
            if (gelenuser.IMKBX._ToBool())
                bistpayi = true;
            //if (chkVeriAnalitik.Checked)
            //    bistpayi = true;
            #endregion
            #region Viop

            if (gelenuser.VIPL1._ToBool())
                bistpayi = true;
            if (gelenuser.VIPL2._ToBool())
                bistpayi = true;
            if (gelenuser.Vd2P._ToBool())
                bistpayi = true;
            if (gelenuser.VIPL1P._ToBool())
                bistpayi = true;
            if (gelenuser.VIPNET._ToBool())
                bistpayi = true;
            #endregion
            #region Tahvil
            if (gelenuser.THVL1._ToBool())
                bistpayi = true;
            if (gelenuser.THVL2._ToBool())
                bistpayi = true;
            if (gelenuser.THVL1P._ToBool())
                bistpayi = true;


            #endregion

            return bistpayi;
        }


        public delegate void lblsayacguncelle(string text);

        public void lblsayacyaz(string text)
        {
            try
            {
                if (lblSayac.InvokeRequired)
                {
                    lblsayacguncelle lbl = new lblsayacguncelle(lblsayacyaz);

                    this.Invoke(lbl, new object[] { text });

                }
                else
                {
                    lblSayac.Text = text;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void seciliOlanKaydiEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var newtrhread = new Thread(new ThreadStart(() => {
                
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    Thread.Sleep(200);
                    var username = dataGridView1.SelectedRows[i].Cells[0].Value.ToString();
                    var secili = Dictionary.Values.FirstOrDefault(x => x.Username == username);
                    writetosql(secili);
                    lblsayacyaz(dataGridView1.SelectedRows.Count.ToString()+" / " +(i + 1).ToString());
                }


            }));

            newtrhread.Start();


        }

        public void ara2()
        {
            try
            {
                if (users.XmlCustomer == null)
                    return;
                IEnumerable<ArrayOfXmlCustomerXmlCustomer> usersara = users.XmlCustomer.Where(x => x.Username.Contains(txtAra.Text) || x.Explanation.Contains(txtAra.Text));

              
              
                
                if (comboBox1.SelectedIndex!=0)
                {

                    if (comboBox1.SelectedIndex == 1)
                    {
                        usersara = usersara.Where(x => x.OnOff.ToString() == "1");
                  
                    }
                    else if (comboBox1.SelectedIndex == 2)
                          usersara = usersara.Where(x => x.OnOff.ToString() == "0");

                }

                dataGridView1.DataSource = usersara.ToList();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public void ara()
        {
            try
            {
                if (Dictionary.Count<1)
                    return;
                var liste = Dictionary.Values.Where(x => x.Username != "").ToList();

                if (txtAra.Text != "")
                {
                    liste = liste.Where(x => x.Username.StartsWith(txtAra.Text) || x.Explanation.StartsWith(txtAra.Text)).ToList();
                }

               
                if (comboBox1.SelectedIndex != 0)
                {

                    if (comboBox1.SelectedIndex == 1)
                    {
                        liste = liste.Where(x => x.Status==1).ToList();

                    }
                    else if (comboBox1.SelectedIndex == 2)
                        liste = liste.Where(x => x.Status == 0).ToList();

                }

                gridDoldur(liste);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ara();
        }

        private void txtAra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ara();
            }
        }

        void gridDoldur(List<cxCustomer> liste)
        {
            var count = liste.Count;
            lblsayacyaz(count.ToString());

            dataGridView1.DataSource = liste.Select(x => new {
                x.Username,
                x.Password,
                x.Isim,
                x.Explanation,
                MusteriNo = x.PmtsNo,
                x.KimlikNo,
                x.Telefon,
                x.Mail,
                x.Sube,
                x.KurumMusteriNo,
                x.Not1,
                x.BaslangicTarih,
                x.Expiry,
                x.Adres,
                x.OnOff,
                x.PRO,
                x.CEP,
                x.Robot,
                x.IMKBL1,
                x.IMKBL1P,
                x.IMKBL2,
                x.IMKBL2P,
                x.IMKBX,
                x.IMKBISL,
                x.VIPL1,
                x.VIPL1P,
                x.VIPL2,
                x.Vd2P,
                x.VIPNET,
                x.THVL1,
                x.THVL1P,
                x.THVL2,
                x.ANPRO, // analiz pro
                x.DJI,
                x.SPI,
                x.XETRA,
                x.CBOT,
                x.CME,
                x.CBOTM,
                x.CMEM,
                x.EUREX


            }).ToList();

        }

        private void btnExceldenOku_Click(object sender, EventArgs e)
        {
            try
            {

                Dictionary.Clear();
                string filePath = "";


                openFileDialog1.InitialDirectory = Application.StartupPath;
                openFileDialog1.Filter = "(*.xlsx)|*.xlsx|(*.*)|*.*";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog1.FileName;

                }
                else
                    return;



                excelMusteriListesiOku(filePath);
                var liste = Dictionary.Values.Where(x => x.Username != "").ToList();
                gridDoldur(liste);







            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        

        private void btnXmlOku_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary.Clear();

                string filePath = "";


                openFileDialog1.InitialDirectory = Application.StartupPath;
                openFileDialog1.Filter = "(*.xml)|*.xml|(*.*)|*.*";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog1.FileName;

                }
                else
                    return;



                xmlMusteriListesiOku(filePath);

                var liste = Dictionary.Values.Where(x => x.Username != "").ToList();

                gridDoldur(liste);



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        void excelMusteriListesiOku(string filename)
        {
            try
            {
              
                if (File.Exists(filename) == false)
                {
                    MessageBox.Show(filename, "Dosya Bulunamadı");
                    return;
                }

               try
                    {
                        Excel.Application excelapp = new Excel.Application();
                        Excel.Workbook workbook = excelapp.Workbooks.Open(filename, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "", false, false, 0, true, false, false);
                        Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets.get_Item(1);
                        Excel.Range range = sheet.UsedRange;
                        object[,] value = range.Value2;
                        workbook.Close();
                        excelapp.Quit();

                        int rowcount = value.GetLength(0);
                        int colcount = value.GetLength(1);

                        // headers
                        string[] headers = new string[colcount + 1];
                        for (int j = 1; j <= colcount; j++)
                        {
                            if (value[1, j] != null)
                            {
                                headers[j] = value[1, j].ToString().Trim();
                                headers[j] = headers[j]._ToEngUp();
                            }
                        }

                        // data
                        for (int i = 3; i <= rowcount; i++)
                        {
                            string username = "";
                           string password = "";
                        string terminalid = "";
                        bool onoff = false;
                            string expiry = "";
                            string producttype = "";
                            bool scmusable = false;
                            bool realtimesend = false;
                            bool requestsend = false;
                            string explanation = "";
                            bool imkbl1 = false;
                            bool imkbl1p = false;
                            bool imkbl2 = false;
                            bool imkbl2P = false;
                            bool imkbisl = false;
                            bool imkbx = false;
                            bool thvl1 = false;
                            bool thvl1p = false;
                            bool thvl2 = false;
                            bool anPro = false;
                            bool vipl1 = false;
                            bool vipl1p = false;
                            bool vipl2 = false;
                            bool Vd2P = false;
                            bool vipnet = false;
                            bool futgck = false;
                            bool winx = false;
                            bool amex = false;
                            bool cbot = false;
                            bool cbotm = false;
                            bool cme = false;
                            bool cmem = false;
                            bool comex = false;
                            bool dji = false;
                            bool eurex = false;
                            bool lse = false;
                            bool nymex = false;
                            bool nymexm = false;
                            bool nyse = false;
                            bool nasdaq = false;
                            bool spi = false;
                            bool xetra = false;
                            bool chix = false;
                            bool gtis = false;
                            bool djbn = false;
                            bool dja = false;
                            bool djcs = false;
                            bool djes = false;
                            bool djf = false;
                            bool djn = false;
                            bool messenger = false;
                            bool dovizquote = false;
                            string krm = "";
                            string downloadurl = "";
                            string pmtsno = "";
                            string telefon = "";
                            string mail = "";
                            string adres = "";
                            int pro = 0;
                            int cep = 0;
                            string baslangictarih = "";

                            string isim = "";
                            string sehir = "";
                            string kurummusterino = "";
                            string not1 = "";
                            string not2 = "";
                            string not3 = "";

                            for (int j = 1; j <= colcount; j++)
                            {
                                if (value[i, j] != null)
                                {
                                    string str = value[i, j].ToString().Trim();
                                    switch (headers[j])
                                    {
                                        case "USERNAME": username = str; break;
                                        case "PASSWORD": password = str; break;
                                        case "TERMINAL ID": terminalid = str; break;
                                        case "ON/OFF": onoff = str._ToBool(); break;
                                        case "EXPIRY": expiry = str; break;
                                        case "PRODUCT TYPE": producttype = str.ToUpper()._ToEngUp(); break;
                                        case "USABLE": scmusable = str._ToBool(); break;
                                        case "REALTIME SEND": realtimesend = str._ToBool(); break;
                                        case "REQUEST SEND": requestsend = str._ToBool(); break;
                                        case "EXPLANATION": explanation = str; break;
                                        case "IMKB_L1": imkbl1 = str._ToBool(); break;
                                        case "IMKB_L1+": imkbl1p = str._ToBool(); break;
                                        case "IMKB_L2": imkbl2 = str._ToBool(); break;
                                        case "IMKB_L2+": imkbl2P = str._ToBool(); break;
                                        case "IMKB_ISL": imkbisl = str._ToBool(); break;
                                        case "IMKB_X": imkbx = str._ToBool(); break;
                                        case "THV_L1": thvl1 = str._ToBool(); break;
                                        case "THV_L1+": thvl1p = str._ToBool(); break;
                                        case "THV_L2": thvl2 = str._ToBool(); break;
                                        case "ANPRO": anPro = str._ToBool(); break;  //analizpro
                                        case "VIP_L1": vipl1 = str._ToBool(); break;
                                        case "VIP_L1+": vipl1p = str._ToBool(); break;
                                        case "VIP_L2": vipl2 = str._ToBool(); break;
                                        case "VIP_L2+": Vd2P = str._ToBool(); break;
                                        case "VIPNET": vipnet = str._ToBool(); break;
                                        case "FUTGCK": futgck = str._ToBool(); break;
                                        case "WINX": winx = str._ToBool(); break;
                                        case "AMEX": amex = str._ToBool(); break;
                                        case "CBOT": cbot = str._ToBool(); break;
                                        case "CBOTM": cbotm = str._ToBool(); break;
                                        case "CME": cme = str._ToBool(); break;
                                        case "CMEM": cmem = str._ToBool(); break;
                                        case "COMEX": comex = str._ToBool(); break;
                                        case "DJI": dji = str._ToBool(); break;
                                        case "EUREX": eurex = str._ToBool(); break;
                                        case "LSE": lse = str._ToBool(); break;
                                        case "NYMEX": nymex = str._ToBool(); break;
                                        case "NYMEXM": nymexm = str._ToBool(); break;
                                        case "NYSE": nyse = str._ToBool(); break;
                                        case "NASDAQ": nasdaq = str._ToBool(); break;
                                        case "SPI": spi = str._ToBool(); break;
                                        case "XETRA": xetra = str._ToBool(); break;
                                        case "CHIX": chix = str._ToBool(); break;
                                        case "GTIS": gtis = str._ToBool(); break;
                                        case "DJBN": djbn = str._ToBool(); break;
                                        case "DJA": dja = str._ToBool(); break;
                                        case "DJCS": djcs = str._ToBool(); break;
                                        case "DJES": djes = str._ToBool(); break;
                                        case "DJF": djf = str._ToBool(); break;
                                        case "DJN": djn = str._ToBool(); break;
                                        case "PRO": pro = str._ToInt(); break;
                                        case "CEP": cep = str._ToInt(); break;
                                        case "BASLANGICTARIH": baslangictarih = str; break;

                                        case "MESSENGER": messenger = str._ToBool(); break;
                                        case "DOVIZQUOTE": dovizquote = str._ToBool(); break;
                                        case "FXKURUM": krm = str; break;
                                        case "DOWNLOADURL": downloadurl = str; break;
                                        case "PMTSNO": pmtsno = str; break;
                                        case "TELEFON": telefon = str; break;
                                        case "MAIL": mail = str; break;
                                        case "ADRES": adres = str; break;
                                        case "ISIM": isim = str; break;
                                        case "SEHIR": sehir = str; break;
                                        case "KURUMMUSTERINO": kurummusterino = str; break;
                                        case "NOT1": not1 = str; break;
                                        case "NOT2": not2 = str; break;
                                        case "NOT3": not3 = str; break;
                                    }
                                }
                            }

                            if (imkbl2) imkbl1p = true;
                            if (thvl2) thvl1p = true;
                            if (vipl2) vipl1p = true;

                            if (username != "")
                            {
                                if (Dictionary.ContainsKey(username) == false)
                                {
                                    var newitem = new cxCustomer();
                                    newitem.TerminalID = username;
                                    newitem.Password = "ideal";
                                    Dictionary[username] = newitem;
                                }
                                var customeritem = Dictionary[username];

                                customeritem.Username = username;
                                customeritem.Password = password;
                                customeritem.TerminalID = terminalid;
                                customeritem.OnOff = onoff;
                                customeritem.Expiry = expiry;
                                customeritem.ProductType = producttype;
                                customeritem.SCMusable = scmusable;
                                customeritem.RealtimeHostEnabled = realtimesend;
                                customeritem.DownloadHostEnabled = requestsend;
                                customeritem.Explanation = explanation;
                                customeritem.IMKBL1 = imkbl1;
                                customeritem.IMKBL1P = imkbl1p;
                                customeritem.IMKBL2 = imkbl2;
                                customeritem.IMKBL2P = imkbl2P;
                                customeritem.IMKBISL = imkbisl;
                                customeritem.IMKBX = imkbx;
                                customeritem.THVL1 = thvl1;
                                customeritem.THVL1P = thvl1p;
                                customeritem.THVL2 = thvl2;
                                customeritem.ANPRO = anPro;
                                customeritem.VIPL1 = vipl1;
                                customeritem.VIPL1P = vipl1p;
                                customeritem.VIPL2 = vipl2;
                                customeritem.Vd2P = Vd2P;
                                customeritem.VIPNET = vipnet;
                                customeritem.FUTGCK = futgck;
                                customeritem.WINX = winx;
                                customeritem.AMEX = amex;
                                customeritem.CBOT = cbot;
                                customeritem.CBOTM = cbotm;
                                customeritem.CME = cme;
                                customeritem.CMEM = cmem;
                                customeritem.COMEX = comex;
                                customeritem.DJI = dji;
                                customeritem.EUREX = eurex;
                                customeritem.LSE = lse;
                                customeritem.NYMEX = nymex;
                                customeritem.NYMEXM = nymexm;
                                customeritem.NYSE = nyse;
                                customeritem.NASDAQ = nasdaq;
                                customeritem.SPI = spi;
                                customeritem.XETRA = xetra;
                                customeritem.CHIX = chix;
                                customeritem.GTIS = gtis;
                                customeritem.DJBN = djbn;
                                customeritem.DJA = dja;
                                customeritem.DJCS = djcs;
                                customeritem.DJES = djes;
                                customeritem.DJF = djf;
                                customeritem.DJN = djn;
                                customeritem.Messenger = messenger;
                                customeritem.DovizQuote = dovizquote;
                                customeritem.KRM = krm;
                                customeritem.DownloadUrl = downloadurl;
                                customeritem.PmtsNo = pmtsno;
                                customeritem.Telefon = telefon;
                                customeritem.Mail = mail;
                                customeritem.Adres = adres;
                                customeritem.PRO = pro;
                                customeritem.CEP = cep;
                                customeritem.BaslangicTarih = baslangictarih;
                                customeritem.Isim = isim;
                                customeritem.Sehir = sehir;
                                customeritem.KurumMusteriNo = kurummusterino;
                                customeritem.Not1 = not1;
                                customeritem.Not2 = not2;
                                customeritem.Not3 = not3;
                            }
                        }
                        MessageBox.Show("Kayıtlar Aktarıldı", "Müşteri Listesi");
                    
                    }
                    catch (Exception error) { MessageBox.Show(error.Message); }
               
            }
            catch (Exception error) { MessageBox.Show(error.Message); }
        }

        void xmlMusteriListesiOku(string filename)
        {
            try
            {
               
                if (File.Exists(filename) == false)
                {
                    MessageBox.Show(filename, "Dosya Bulunamadı");
                    return;
                }

                var doc = new XmlDocument();
                doc.Load(filename);
                XmlNodeList nodes = doc.SelectNodes("//XmlCustomer");
                foreach (XmlNode node in nodes)
                {
                    string itemusername = node._GetText("Username");
                    var customer = Dictionary._GetOrInsert(itemusername);
                    customer.Username = itemusername;
                    foreach (XmlNode childnode in node)
                    {
                        string fieldname = childnode.Name;
                        string fieldvalue = childnode.InnerText;
                        switch (fieldname)
                        {
                            case "Username": customer.Username = fieldvalue; break;
                            case "Password": customer.Password = fieldvalue; break;
                            case "TerminalID": customer.TerminalID = fieldvalue; break;
                            case "OnOff": customer.OnOff = fieldvalue._ToBool(); break;
                            case "Expiry": customer.Expiry = fieldvalue; break;
                            case "Version": customer.Version = fieldvalue; break;
                            case "MachineName": customer.MachineName = fieldvalue; break;
                            case "ProductType": customer.ProductType = fieldvalue; break;
                            case "IP": customer.IP = fieldvalue; break;
                            case "Explanation": customer.Explanation = fieldvalue; break;
                            case "IMKBL1": customer.IMKBL1 = fieldvalue._ToBool(); break;
                            case "IMKBL1P": customer.IMKBL1P = fieldvalue._ToBool(); break;
                            case "IMKBL2": customer.IMKBL2 = fieldvalue._ToBool(); break;
                            case "IMKBL2P": customer.IMKBL2P = fieldvalue._ToBool(); break;
                            case "IMKBISL": customer.IMKBISL = fieldvalue._ToBool(); break;
                            case "IMKBX": customer.IMKBX = fieldvalue._ToBool(); break;
                            case "THVL1": customer.THVL1 = fieldvalue._ToBool(); break;
                            case "THVL1P": customer.THVL1P = fieldvalue._ToBool(); break;
                            case "THVL2": customer.THVL2 = fieldvalue._ToBool(); break;
                            case "ANPRO": customer.ANPRO = fieldvalue._ToBool(); break;
                            case "VIPL1": customer.VIPL1 = fieldvalue._ToBool(); break;
                            case "VIPL1P": customer.VIPL1P = fieldvalue._ToBool(); break;
                            case "VIPL2": customer.VIPL2 = fieldvalue._ToBool(); break;
                            case "Vd2P": customer.Vd2P = fieldvalue._ToBool(); break;
                            case "VIPNET": customer.VIPNET = fieldvalue._ToBool(); break;
                            case "SASEL1": customer.SASEL1 = fieldvalue._ToBool(); break;
                            case "SASEL2": customer.SASEL2 = fieldvalue._ToBool(); break;
                            case "FUTGCK": customer.FUTGCK = fieldvalue._ToBool(); break;
                            case "WINX": customer.WINX = fieldvalue._ToBool(); break;
                            case "AMEX": customer.AMEX = fieldvalue._ToBool(); break;
                            case "CBOT": customer.CBOT = fieldvalue._ToBool(); break;
                            case "CBOTM": customer.CBOTM = fieldvalue._ToBool(); break;
                            case "CME": customer.CME = fieldvalue._ToBool(); break;
                            case "CMEM": customer.CMEM = fieldvalue._ToBool(); break;
                            case "COMEX": customer.COMEX = fieldvalue._ToBool(); break;
                            case "DJI": customer.DJI = fieldvalue._ToBool(); break;
                            case "EUREX": customer.EUREX = fieldvalue._ToBool(); break;
                            case "LSE": customer.LSE = fieldvalue._ToBool(); break;
                            case "NYMEX": customer.NYMEX = fieldvalue._ToBool(); break;
                            case "NYMEXM": customer.NYMEXM = fieldvalue._ToBool(); break;
                            case "NYSE": customer.NYSE = fieldvalue._ToBool(); break;
                            case "NASDAQ": customer.NASDAQ = fieldvalue._ToBool(); break;
                            case "SPI": customer.SPI = fieldvalue._ToBool(); break;
                            case "XETRA": customer.XETRA = fieldvalue._ToBool(); break;
                            case "CHIX": customer.CHIX = fieldvalue._ToBool(); break;
                            case "MEKSA": customer.MEKSA = fieldvalue._ToBool(); break;
                            case "Messenger": customer.Messenger = fieldvalue._ToBool(); break;
                            case "DovizQuote": customer.DovizQuote = fieldvalue._ToBool(); break;
                            case "KRM": customer.KRM = fieldvalue; break;
                            case "PmtsNo": customer.PmtsNo = fieldvalue; break;
                            case "Telefon": customer.Telefon = fieldvalue; break;
                            case "Mail": customer.Mail = fieldvalue; break;
                            case "Adres": customer.Adres = fieldvalue; break;
                            case "Robot": customer.Robot = fieldvalue._ToBool(); break;
                            case "GhostKurum": customer.GhostKurum = fieldvalue; break;
                            case "WebMesajKurum": customer.WebMesajKurum = fieldvalue; break;
                            case "WebMesajGrup": customer.WebMesajGrup = fieldvalue; break;
                            case "WebMesajEnabled": customer.WebMesajEnabled = fieldvalue._ToInt(); break;
                            case "PRO": customer.PRO = fieldvalue._ToInt(); break;
                            case "CEP": customer.CEP = fieldvalue._ToInt(); break;
                            case "Status": customer.Status = fieldvalue._ToInt(); break;
                            case "BaslangicTarih": customer.BaslangicTarih = fieldvalue; break;
                            case "KimlikNo": customer.KimlikNo = fieldvalue; break;
                            case "Fiyat": customer.Fiyat = fieldvalue; break;

                            case "Isim": customer.Isim = fieldvalue; break;
                            case "Sehir": customer.Sehir = fieldvalue; break;
                            case "KurumMusteriNo": customer.KurumMusteriNo = fieldvalue; break;
                            case "Not1": customer.Not1 = fieldvalue; break;
                            case "Not2": customer.Not2 = fieldvalue; break;
                            case "Not3": customer.Not3 = fieldvalue; break;
                        }
                    }
                }
               
                MessageBox.Show("Kayıtlar Aktarıldı", "Müşteri Listesi");
            }
            catch { }
        }










        // static
        public static volatile Dictionary<string, cxCustomer> Dictionary = new Dictionary<string, cxCustomer>();
        public static void Deserialize(string filename)
        {
            try
            {

                if (File.Exists(filename))
                {
                    var itemlist = new List<cxCustomer>();
                    var bformatter = new BinaryFormatter();
                    using (var fstream = new FileStream(filename, FileMode.Open))
                        itemlist = (List<cxCustomer>)bformatter.Deserialize(fstream);

                    foreach (var item in itemlist)
                    {
                        if (item.Username.Trim() != "")
                        {
                            item.SCM = false;
                            if (item.ProductType == "SCM") item.SCM = true;
                            if (item.KRM == null) item.KRM = "";
                            if (item.PmtsNo == null) item.PmtsNo = "";
                            if (item.Telefon == null) item.Telefon = "";
                            if (item.Mail == null) item.Mail = "";
                            if (item.Adres == null) item.Adres = "";
                            if (item.DownloadUrl == null) item.DownloadUrl = "";

                            if (item.ServerIP1 == null) item.ServerIP1 = "";
                            if (item.ServerPort1 == 0) item.ServerPort1 = 0;
                            if (item.ServerIP2 == null) item.ServerIP2 = "";
                            if (item.ServerPort2 == 0) item.ServerPort2 = 0;
                            if (item.ServerIP3 == null) item.ServerIP3 = "";
                            if (item.ServerPort3 == 0) item.ServerPort3 = 0;
                            if (item.ServerIP4 == null) item.ServerIP4 = "";
                            if (item.ServerPort4 == 0) item.ServerPort4 = 0;
                            if (item.ChartIP == null) item.ChartIP = "";

                            if (item.ClassVersion < 2)
                            {
                                item.ClassVersion = 2;
                                // item.Robot = false;
                            }

                            if (item.GhostKurum == null) item.GhostKurum = "";

                            if (item.WebMesajKurum == null) item.WebMesajKurum = "";
                            if (item.WebMesajGrup == null) item.WebMesajGrup = "";
                            //if (cxSetting.OperationMode == 0) item.PRO = 1;
                            //item.Status = 1;
                            if (item.BaslangicTarih == null) item.BaslangicTarih = "";
                            if (item.KimlikNo == null) item.KimlikNo = "";
                            if (item.Fiyat == null) item.Fiyat = "";
                            if (item.SozlesmeKabulDateStr == null) item.SozlesmeKabulDateStr = "";
                            if (item.DisclaimerKabulDateStr == null) item.DisclaimerKabulDateStr = "";
                            if (item.RaporWebUrun == null) item.RaporWebUrun = "";
                            if (item.RaporWebCihaz == null) item.RaporWebCihaz = "";
                            if (item.RaporWebSonLoginTarihi == null) item.RaporWebSonLoginTarihi = "";
                            if (item.Sube == null) item.Sube = "";
                            if (item.KullaniciTip == null) item.KullaniciTip = "";
                            if (item.Isim == null) item.Isim = "";
                            if (item.Sehir == null) item.Sehir = "";
                            if (item.KurumMusteriNo == null) item.KurumMusteriNo = "";
                            if (item.Not1 == null) item.Not1 = "";
                            if (item.Not2 == null) item.Not2 = "";
                            if (item.Not3 == null) item.Not3 = "";

                            Dictionary[item.Username] = item;
                        }
                    }
                }

            }
            catch (Exception error) { MessageBox.Show(error.Message); }
        }
        public static void Serialize(string filename)
        {
            try
            {
                foreach (var item in Dictionary.Values)
                {
                    item.SCM = false;
                    if (item.ProductType == "SCM")
                        item.SCM = true;
                }


                var itemlist = Dictionary.Values.ToList();
                if (itemlist.Count > 0)
                {
                    var bformatter = new BinaryFormatter();
                    using (var fstream = new FileStream(filename, FileMode.Create))
                        bformatter.Serialize(fstream, itemlist);
                }

            }
            catch (Exception error) { MessageBox.Show(error.Message); }
        }

        private void btnSqlYaz_Click(object sender, EventArgs e)
        {
          

            var newtrhread = new Thread(new ThreadStart(() => {



                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {

                    Thread.Sleep(200);
                    var username = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    var secili = Dictionary.Values.FirstOrDefault(x => x.Username == username);
                    writetosql(secili);
                    lblsayacyaz(dataGridView1.Rows.Count.ToString() + " / " + (i + 1).ToString());
                }


            }));

            newtrhread.Start();
        }
    }




    public class cxCustomer
    {
        public int ClassVersion = 0;
        public string Username = "";
        public string Password = "";
        public string TerminalID = "";
        public bool OnOff = false;
        public string Expiry = "";
        public string Version = "";
        public string MachineName = "";
        public string ProductType = "";
        public string IP = "";
        public bool SCM = false;
        public bool SCMusable = false;
        public bool RealtimeHostEnabled = false;
        public bool DownloadHostEnabled = false;
        public string Explanation = "";
        public bool DDE = false;
        public bool IMKBL1 = false;
        public bool IMKBL1P = false;
        public bool IMKBL2 = false;
        public bool IMKBL2P = false;
        public bool IMKBISL = false;
        public bool IMKBX = false;
        public bool SASEL1 = false;
        public bool SASEL2 = false;
        public bool THVL1 = false;
        public bool THVL1P = false;
        public bool THVL2 = false;
        public bool ANPRO = false; // analiz pro
        public bool VIPL1 = false;
        public bool VIPL1P = false;
        public bool VIPL2 = false;
        public bool Vd2P = false;
        public bool VIPNET = false;
        public bool FUTGCK = false;
        public bool WINX = false;
        public bool AMEX = false;
        public bool CBOT = false;
        public bool CBOTM = false;
        public bool CME = false;
        public bool CMEM = false;
        public bool COMEX = false;
        public bool DJI = false;
        public bool EUREX = false;
        public bool LSE = false;
        public bool NYMEX = false;
        public bool NYMEXM = false;
        public bool NYSE = false;
        public bool NASDAQ = false;
        public bool SPI = false;
        public bool XETRA = false;
        public bool CHIX = false;
        public bool GTIS = false;
        public bool DJBN = false;
        public bool DJA = false;
        public bool DJCS = false;
        public bool DJES = false;
        public bool DJF = false;
        public bool DJN = false;
        public bool MEKSA = false;

        public bool Messenger = true;
        public bool DovizQuote = false;
        public string KRM = "";
        public string DownloadUrl = "";
        public string PmtsNo = "";
        public string Telefon = "";
        public string Mail = "";
        public string Adres = "";

        public string ServerIP1 = "";
        public int ServerPort1 = 0;
        public string ServerIP2 = "";
        public int ServerPort2 = 0;
        public string ServerIP3 = "";
        public int ServerPort3 = 0;
        public string ServerIP4 = "";
        public int ServerPort4 = 0;
        public string ChartIP = "";
        public int ChartPort = 0;
        public int IdbPort = 0;
        public bool Robot = false;
        public string GhostKurum = "";
        public string WebMesajKurum = "";
        public string WebMesajGrup = "";
        public int WebMesajEnabled = 0;
        public int PRO = 0;
        public int CEP = 0;
        public int Status = 0;
        public string BaslangicTarih = "";
        public string KimlikNo = "";
        public string Fiyat = "";
        public string SozlesmeKabulDateStr = "";
        public string DisclaimerKabulDateStr = "";

        public string RaporWebUrun = "";
        public string RaporWebCihaz = "";
        public string RaporWebSonLoginTarihi = "";
        public int RaporWebBuAyLoginSayisi = 0;
        public int RaporWebOncekiAyLoginSayisi = 0;

        public string Sube = "";
        public string KullaniciTip = "";
        public string Isim = "";
        public string Sehir = "";
        public string KurumMusteriNo = "";
        public string Not1 = "";
        public string Not2 = "";
        public string Not3 = "";
        public int DataAktarimPort = 0;
    }


    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ArrayOfXmlCustomer
    {

        private ArrayOfXmlCustomerXmlCustomer[] xmlCustomerField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("XmlCustomer")]
        public ArrayOfXmlCustomerXmlCustomer[] XmlCustomer
        {
            get
            {
                return this.xmlCustomerField;
            }
            set
            {
                this.xmlCustomerField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ArrayOfXmlCustomerXmlCustomer
    {

        private uint pmtsNoField;

        private string usernameField;

        private string passwordField;

        private byte onOffField;

        private byte pROField;

        private byte cEPField;

        private uint expiryField;

        private string explanationField;

        private string telefonField;

        private byte iMKBL1Field;

        private byte iMKBL1PField;

        private byte iMKBL2Field;

        private byte iMKBL2PField;

        private byte iMKBISLField;

        private byte iMKBXField;

        private byte tHVL1Field;

        private byte tHVL1PField;

        private byte tHVL2Field;

        private byte anProField;

        private byte vIPL1Field;

        private byte vIPL1PField;

        private byte vIPL2Field;

        private byte Vd2PField;

        private byte vIPNETField;

        private byte fUTGCKField;

        private byte wINXField;

        private byte aMEXField;

        private byte cBOTField;

        private byte cBOTMField;

        private byte cMEField;

        private byte cMEMField;

        private byte cOMEXField;

        private byte dJIField;

        private byte eUREXField;

        private byte lSEField;

        private byte nYMEXField;

        private byte nYMEXMField;

        private byte nYSEField;

        private byte nASDAQField;

        private byte sPIField;

        private byte xETRAField;

        private byte cHIXField;

        private byte messengerField;

        private byte dovizQuoteField;

        private byte robotField;

        private string versionField;

        private string machineNameField;

        private string productTypeField;

        private string ipField;

        private string terminalIDField;

        private string mailField;

        private string adresField;

        private string ghostKurumField;

        private string webMesajKurumField;

        private string webMesajGrupField;

        private byte webMesajEnabledField;

        private byte statusField;

        private string baslangicTarihField;

        private string kimlikNoField;

        private string fiyatField;

        private string sozlesmeKabulDateStrField;

        private string disclaimerKabulDateStrField;

        private string raporWebUrunField;

        private string raporWebCihazField;

        private string raporWebSonLoginTarihiField;

        private ushort raporWebBuAyLoginSayisiField;

        private ushort raporWebOncekiAyLoginSayisiField;

        private string subeField;

        private string kullaniciTipField;

        private string isimField;

        private string sehirField;

        private string kurumMusteriNoField;

        private string not1Field;

        private string not2Field;

        private string not3Field;

        private decimal yurticiLisansField;

        /// <remarks/>
        public uint PmtsNo
        {
            get
            {
                return this.pmtsNoField;
            }
            set
            {
                this.pmtsNoField = value;
            }
        }

        /// <remarks/>
        public string Username
        {
            get
            {
                return this.usernameField;
            }
            set
            {
                this.usernameField = value;
            }
        }

        /// <remarks/>
        public string Password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }

        /// <remarks/>
        public byte OnOff
        {
            get
            {
                return this.onOffField;
            }
            set
            {
                this.onOffField = value;
            }
        }

        /// <remarks/>
        public byte PRO
        {
            get
            {
                return this.pROField;
            }
            set
            {
                this.pROField = value;
            }
        }

        /// <remarks/>
        public byte CEP
        {
            get
            {
                return this.cEPField;
            }
            set
            {
                this.cEPField = value;
            }
        }

        /// <remarks/>
        public uint Expiry
        {
            get
            {
                return this.expiryField;
            }
            set
            {
                this.expiryField = value;
            }
        }

        /// <remarks/>
        public string Explanation
        {
            get
            {
                return this.explanationField;
            }
            set
            {
                this.explanationField = value;
            }
        }

        /// <remarks/>
        public string Telefon
        {
            get
            {
                return this.telefonField;
            }
            set
            {
                this.telefonField = value;
            }
        }

        /// <remarks/>
        public byte IMKBL1
        {
            get
            {
                return this.iMKBL1Field;
            }
            set
            {
                this.iMKBL1Field = value;
            }
        }

        /// <remarks/>
        public byte IMKBL1P
        {
            get
            {
                return this.iMKBL1PField;
            }
            set
            {
                this.iMKBL1PField = value;
            }
        }

        /// <remarks/>
        public byte IMKBL2
        {
            get
            {
                return this.iMKBL2Field;
            }
            set
            {
                this.iMKBL2Field = value;
            }
        }

        /// <remarks/>
        public byte IMKBL2P
        {
            get
            {
                return this.iMKBL2PField;
            }
            set
            {
                this.iMKBL2PField = value;
            }
        }

        /// <remarks/>
        public byte IMKBISL
        {
            get
            {
                return this.iMKBISLField;
            }
            set
            {
                this.iMKBISLField = value;
            }
        }

        /// <remarks/>
        public byte IMKBX
        {
            get
            {
                return this.iMKBXField;
            }
            set
            {
                this.iMKBXField = value;
            }
        }

        /// <remarks/>
        public byte THVL1
        {
            get
            {
                return this.tHVL1Field;
            }
            set
            {
                this.tHVL1Field = value;
            }
        }

        /// <remarks/>
        public byte THVL1P
        {
            get
            {
                return this.tHVL1PField;
            }
            set
            {
                this.tHVL1PField = value;
            }
        }

        /// <remarks/>
        public byte THVL2
        {
            get
            {
                return this.tHVL2Field;
            }
            set
            {
                this.tHVL2Field = value;
            }
        }


        /// <remarks/>
        public byte ANPRO
        {
            get
            {
                return this.anProField;
            }
            set
            {
                this.anProField = value;
            }
        }

        /// <remarks/>
        public byte VIPL1
        {
            get
            {
                return this.vIPL1Field;
            }
            set
            {
                this.vIPL1Field = value;
            }
        }

        /// <remarks/>
        public byte VIPL1P
        {
            get
            {
                return this.vIPL1PField;
            }
            set
            {
                this.vIPL1PField = value;
            }
        }

        /// <remarks/>
        public byte VIPL2
        {
            get
            {
                return this.vIPL2Field;
            }
            set
            {
                this.vIPL2Field = value;
            }
        }

        /// <remarks/>
        public byte Vd2P
        {
            get
            {
                return this.Vd2PField;
            }
            set
            {
                this.Vd2PField = value;
            }
        }
        /// <remarks/>
        public byte VIPNET
        {
            get
            {
                return this.vIPNETField;
            }
            set
            {
                this.vIPNETField = value;
            }
        }

        /// <remarks/>
        public byte FUTGCK
        {
            get
            {
                return this.fUTGCKField;
            }
            set
            {
                this.fUTGCKField = value;
            }
        }

        /// <remarks/>
        public byte WINX
        {
            get
            {
                return this.wINXField;
            }
            set
            {
                this.wINXField = value;
            }
        }

        /// <remarks/>
        public byte AMEX
        {
            get
            {
                return this.aMEXField;
            }
            set
            {
                this.aMEXField = value;
            }
        }

        /// <remarks/>
        public byte CBOT
        {
            get
            {
                return this.cBOTField;
            }
            set
            {
                this.cBOTField = value;
            }
        }

        /// <remarks/>
        public byte CBOTM
        {
            get
            {
                return this.cBOTMField;
            }
            set
            {
                this.cBOTMField = value;
            }
        }

        /// <remarks/>
        public byte CME
        {
            get
            {
                return this.cMEField;
            }
            set
            {
                this.cMEField = value;
            }
        }

        /// <remarks/>
        public byte CMEM
        {
            get
            {
                return this.cMEMField;
            }
            set
            {
                this.cMEMField = value;
            }
        }

        /// <remarks/>
        public byte COMEX
        {
            get
            {
                return this.cOMEXField;
            }
            set
            {
                this.cOMEXField = value;
            }
        }

        /// <remarks/>
        public byte DJI
        {
            get
            {
                return this.dJIField;
            }
            set
            {
                this.dJIField = value;
            }
        }

        /// <remarks/>
        public byte EUREX
        {
            get
            {
                return this.eUREXField;
            }
            set
            {
                this.eUREXField = value;
            }
        }

        /// <remarks/>
        public byte LSE
        {
            get
            {
                return this.lSEField;
            }
            set
            {
                this.lSEField = value;
            }
        }

        /// <remarks/>
        public byte NYMEX
        {
            get
            {
                return this.nYMEXField;
            }
            set
            {
                this.nYMEXField = value;
            }
        }

        /// <remarks/>
        public byte NYMEXM
        {
            get
            {
                return this.nYMEXMField;
            }
            set
            {
                this.nYMEXMField = value;
            }
        }

        /// <remarks/>
        public byte NYSE
        {
            get
            {
                return this.nYSEField;
            }
            set
            {
                this.nYSEField = value;
            }
        }

        /// <remarks/>
        public byte NASDAQ
        {
            get
            {
                return this.nASDAQField;
            }
            set
            {
                this.nASDAQField = value;
            }
        }

        /// <remarks/>
        public byte SPI
        {
            get
            {
                return this.sPIField;
            }
            set
            {
                this.sPIField = value;
            }
        }

        /// <remarks/>
        public byte XETRA
        {
            get
            {
                return this.xETRAField;
            }
            set
            {
                this.xETRAField = value;
            }
        }

        /// <remarks/>
        public byte CHIX
        {
            get
            {
                return this.cHIXField;
            }
            set
            {
                this.cHIXField = value;
            }
        }

        /// <remarks/>
        public byte Messenger
        {
            get
            {
                return this.messengerField;
            }
            set
            {
                this.messengerField = value;
            }
        }

        /// <remarks/>
        public byte DovizQuote
        {
            get
            {
                return this.dovizQuoteField;
            }
            set
            {
                this.dovizQuoteField = value;
            }
        }

        /// <remarks/>
        public byte Robot
        {
            get
            {
                return this.robotField;
            }
            set
            {
                this.robotField = value;
            }
        }

        /// <remarks/>
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        public string MachineName
        {
            get
            {
                return this.machineNameField;
            }
            set
            {
                this.machineNameField = value;
            }
        }

        /// <remarks/>
        public string ProductType
        {
            get
            {
                return this.productTypeField;
            }
            set
            {
                this.productTypeField = value;
            }
        }

        /// <remarks/>
        public string IP
        {
            get
            {
                return this.ipField;
            }
            set
            {
                this.ipField = value;
            }
        }

        /// <remarks/>
        public string TerminalID
        {
            get
            {
                return this.terminalIDField;
            }
            set
            {
                this.terminalIDField = value;
            }
        }

        /// <remarks/>
        public string Mail
        {
            get
            {
                return this.mailField;
            }
            set
            {
                this.mailField = value;
            }
        }

        /// <remarks/>
        public string Adres
        {
            get
            {
                return this.adresField;
            }
            set
            {
                this.adresField = value;
            }
        }

        /// <remarks/>
        public string GhostKurum
        {
            get
            {
                return this.ghostKurumField;
            }
            set
            {
                this.ghostKurumField = value;
            }
        }

        /// <remarks/>
        public string WebMesajKurum
        {
            get
            {
                return this.webMesajKurumField;
            }
            set
            {
                this.webMesajKurumField = value;
            }
        }

        /// <remarks/>
        public string WebMesajGrup
        {
            get
            {
                return this.webMesajGrupField;
            }
            set
            {
                this.webMesajGrupField = value;
            }
        }

        /// <remarks/>
        public byte WebMesajEnabled
        {
            get
            {
                return this.webMesajEnabledField;
            }
            set
            {
                this.webMesajEnabledField = value;
            }
        }

        /// <remarks/>
        public byte Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string BaslangicTarih
        {
            get
            {
                return this.baslangicTarihField;
            }
            set
            {
                this.baslangicTarihField = value;
            }
        }

        /// <remarks/>
        public string KimlikNo
        {
            get
            {
                return this.kimlikNoField;
            }
            set
            {
                this.kimlikNoField = value;
            }
        }

        /// <remarks/>
        public string Fiyat
        {
            get
            {
                return this.fiyatField;
            }
            set
            {
                this.fiyatField = value;
            }
        }

        /// <remarks/>
        public string SozlesmeKabulDateStr
        {
            get
            {
                return this.sozlesmeKabulDateStrField;
            }
            set
            {
                this.sozlesmeKabulDateStrField = value;
            }
        }

        /// <remarks/>
        public string DisclaimerKabulDateStr
        {
            get
            {
                return this.disclaimerKabulDateStrField;
            }
            set
            {
                this.disclaimerKabulDateStrField = value;
            }
        }

        /// <remarks/>
        public string RaporWebUrun
        {
            get
            {
                return this.raporWebUrunField;
            }
            set
            {
                this.raporWebUrunField = value;
            }
        }

        /// <remarks/>
        public string RaporWebCihaz
        {
            get
            {
                return this.raporWebCihazField;
            }
            set
            {
                this.raporWebCihazField = value;
            }
        }

        /// <remarks/>
        public string RaporWebSonLoginTarihi
        {
            get
            {
                return this.raporWebSonLoginTarihiField;
            }
            set
            {
                this.raporWebSonLoginTarihiField = value;
            }
        }

        /// <remarks/>
        public ushort RaporWebBuAyLoginSayisi
        {
            get
            {
                return this.raporWebBuAyLoginSayisiField;
            }
            set
            {
                this.raporWebBuAyLoginSayisiField = value;
            }
        }

        /// <remarks/>
        public ushort RaporWebOncekiAyLoginSayisi
        {
            get
            {
                return this.raporWebOncekiAyLoginSayisiField;
            }
            set
            {
                this.raporWebOncekiAyLoginSayisiField = value;
            }
        }

        /// <remarks/>
        public string Sube
        {
            get
            {
                return this.subeField;
            }
            set
            {
                this.subeField = value;
            }
        }

        /// <remarks/>
        public string KullaniciTip
        {
            get
            {
                return this.kullaniciTipField;
            }
            set
            {
                this.kullaniciTipField = value;
            }
        }

        /// <remarks/>
        public string Isim
        {
            get
            {
                return this.isimField;
            }
            set
            {
                this.isimField = value;
            }
        }

        /// <remarks/>
        public string Sehir
        {
            get
            {
                return this.sehirField;
            }
            set
            {
                this.sehirField = value;
            }
        }

        /// <remarks/>
        public string KurumMusteriNo
        {
            get
            {
                return this.kurumMusteriNoField;
            }
            set
            {
                this.kurumMusteriNoField = value;
            }
        }

        /// <remarks/>
        public string Not1
        {
            get
            {
                return this.not1Field;
            }
            set
            {
                this.not1Field = value;
            }
        }

        /// <remarks/>
        public string Not2
        {
            get
            {
                return this.not2Field;
            }
            set
            {
                this.not2Field = value;
            }
        }

        /// <remarks/>
        public string Not3
        {
            get
            {
                return this.not3Field;
            }
            set
            {
                this.not3Field = value;
            }
        }

        /// <remarks/>
        public decimal YurticiLisans
        {
            get
            {
                return this.yurticiLisansField;
            }
            set
            {
                this.yurticiLisansField = value;
            }
        }
    }




}
