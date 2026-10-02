using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectFNCRM.AdminViews
{
    public partial class formKurumTalepDetay : Form
    {
        public formKurumTalepDetay()
        {
            InitializeComponent();
        }
        public int acriveTalepId = 0;
        public KurumTalepleri activetalep;
        private void formKurumTalepDetay_Load(object sender, EventArgs e)
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            txtMusHesapNo.Enabled = false;


            if (this.Tag != null)
            {

                acriveTalepId = (int)this.Tag;
            }

            if (crm.KurumTalepleris.Where(x => x.id == acriveTalepId).Any())
            {
                activetalep = crm.KurumTalepleris.FirstOrDefault(x => x.id == acriveTalepId);

                gridToform(activetalep);

            }
        }
        private void gridToform(KurumTalepleri activetalepx)
        {
            try
            {
                var crm = new crmDFNDataContext();

                var mus = crm.Musterilers.FirstOrDefault(x => x.MusteriNo == activetalepx.PmtsNo);

                lblKurumAd.Text = mus.MusteriAdi;
                lblTalepNo.Text = activetalepx.id.ToString();
                lblTalepTarihi.Text = activetalepx.TalepTarihi.Value.ToString();
                lblSube.Text = activetalepx.SubeAdi;
                lblTalepEden.Text = (activetalepx.TalebiGiren != null) ? MyTools.CalisanAdSoyadGetir(activetalepx.TalebiGiren.Value) : "";
                lblTalepTipi.Text = activetalepx.KurumTalepTip.TalepTipAd;
                lblTalepDurum.Text = activetalepx.KurumTalepDurum.DurumAd;
                lblTalebiOnaylayan.Text = (activetalepx.TalebiOnaylayan != null) ? MyTools.CalisanAdSoyadGetir(activetalepx.TalebiOnaylayan.Value) : "";
                lblOnayTarihi.Text = (activetalepx.TalepOnayTarihi != null) ? activetalepx.TalepOnayTarihi.Value.ToString() : "";
                lblTalepYapan.Text = (activetalepx.TalebiYapan != null) ? MyTools.CalisanAdSoyadGetir(activetalepx.TalebiYapan.Value) : "";
                lblTalepYapilmaTarihi.Text = (activetalepx.TalepYapilmaTarihi != null) ? activetalepx.TalepYapilmaTarihi.Value.ToString() : "";
                txtTalepAciklama.Text = activetalepx.Aciklama;
                txtMusHesapNo.Text = activetalepx.HesapNo;
                txtMusteriAdSoyad.Text = activetalepx.MusteriAd + " " + activetalepx.MusteriSoyad;
                txtMustemsilciKodu.Text = activetalepx.TemsilciKod;
                txtMusTelefon.Text = activetalepx.Iletisim.Tel1;
                if (activetalep.Iletisim.Ulke != null)
                {
                    txtMusUlke.Text = (activetalepx.Iletisim.Ulke.UlkeAdi != null) ? activetalep.Iletisim.Ulke.UlkeAdi : "";

                }
                if (activetalep.Iletisim.Il != null)
                {
                    txtMusSehir.Text = (activetalepx.Iletisim.Il != null) ? activetalepx.Iletisim.Il.IlAdi : "";

                }
                txtMusIlce.Text = (activetalepx.Iletisim.Ilce != null) ? activetalepx.Iletisim.Ilce.IlceAdi : "";
                txtMusAdres.Text = activetalepx.Iletisim.acikadres;
                txtMusEposta.Text = activetalepx.Iletisim.email;
                txtMusCeptelefonu.Text = activetalepx.Iletisim.Ceptel;
                txtUsername.Text = activetalepx.Usename;
                var lisanlar = crm.KurumTalepLisans.Where(x => x.TalepId == activetalepx.id).ToList();

                for (int i = 0; i < lisanlar.Count; i++)
                {
                    var lisansAd = lisanlar[i].LisansAdi;
                    var lisansOlay = lisanlar[i].KurumTalepLisansOlay.LisansOlayAdi;
                    lViewLisans.Items.Add(new ListViewItem(new string[] { lisansAd, lisansOlay }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnTalebiYap_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                 * 1	Yeni Kullanıcı
                   2	Lisans Düzenleme
                   3	İptal
                   4	Bilgi Düzenleme     
                 */
                var cal = formAdminAra.referance.ActiveCalisan;
                User mailuser = new User();
                var crm = new crmDFNDataContext();
                var talep = crm.KurumTalepleris.FirstOrDefault(x => x.id == acriveTalepId);
                if (talep.TalepDurumId == 3)
                {
                    MessageBox.Show("Bu Talep zaten yapılmış");
                    return;
                }
                if (talep.TalepDurumId != 5 && talep.TalepDurumId != 1)
                {
                    MessageBox.Show("Talep durumu uygun değil");
                    return;
                }
                LisansDurum lisansx = new LisansDurum();
                UserEvent ue = new UserEvent();
                ue.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
                ue.EventTarih = DateTime.Now;
                ue.IP = formAdminAra.referance.IpAdress;
                ue.HostName = formAdminAra.referance.HostName;
                var acilanlar = new StringBuilder();
                var talepdenAcilanlar = new StringBuilder();
                var expriydate = new DateTime(2030, 12, 01);

                #region YeniKullanıcı
                if (activetalep.TalepTipId == 1) //Yeni Kullanıcı
                {
                    if (crm.Users.Where(x => x.UserName == talep.Usename).Any())
                    {
                        MessageBox.Show("Girilen talebin username  bilgisi önceden kaydedilmiş, başka bir username ile manuel olarak kullanıcıyı açınız."); return;
                    }
                    var userx = new User();
                    ue.EventTypeId = 5;
                    var random = new Random();
                    var sayi = random.Next(10000, 99999);

                    userx.UserName = talep.Usename;
                    userx.Password = MyTools.Sifreleme.Encryp(sayi.ToString());
                    userx.Name = talep.MusteriAd;
                    userx.Surname = talep.MusteriSoyad;
                    userx.FXkurum = talep.TemsilciKod;
                    userx.Aciklama = userx.Name + " " + userx.Surname;
                    userx.PmtsNo = talep.PmtsNo;
                    userx.ProNonPro = false;
                    //var soz = crm.Sozlesmelers.FirstOrDefault(x => x.SozlesmeNo == (talep.PmtsNo + "-1"));
                    //userx.SozlesmeID = soz.SozlesmeilID;
                    userx.ProductType = "IDEAL";
                    userx.StatusId = 1;
                    userx.MusteriMenseiID = 1;
                    userx.iletisimId = talep.iletisimId;
                    //  lisansx.YayinDurumu = true;
                    /* Lisans Olay Id
                     * 
                     * 1	Hemen Açılsın
                       2	Ay Başında Açılsın
                       3	İptal
                     */
                    KurumsalBilgiler kbilgi = new KurumsalBilgiler();
                    kbilgi.kurumhesapno = talep.HesapNo;
                    kbilgi.KurumSube = talep.SubeAdi;
                    kbilgi.KurumKullaniciTip = "Müşteri";
                    crm.KurumsalBilgilers.InsertOnSubmit(kbilgi);
                    crm.SubmitChanges();
                    userx.KurumsalBilgilerId = kbilgi.Id;
                    userx.ExpiryDate = expriydate;
                    userx.BaslangicTarihi = DateTime.Now;
                    var lisanlar = crm.KurumTalepLisans.Where(x => x.TalepId == talep.id).ToList();

                    for (int i = 0; i < lisanlar.Count; i++)
                    {
                        var kod = lisanlar[i].LisansKod;

                        #region chkDesktop
                        if (kod == "chkDesktop")
                        {
                            lisansx.ProYetki = true;
                            lisansx.ProYetkiStart = DateTime.Now;
                            lisansx.ProYetkiEnd = expriydate;
                        }
                        if (kod.Contains("chkDesktop_A"))
                        {
                            lisansx.ProYetkiStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.ProYetkiEnd = expriydate;
                        }
                        #endregion
                        #region chkMobil
                        if (kod == "chkMobil")
                        {
                            lisansx.CepYetki = true;
                            lisansx.CepYetkiStart = DateTime.Now;
                            lisansx.CepYetkiEnd = expriydate;
                        }
                        if (kod.Contains("chkMobil_A"))
                        {
                            lisansx.CepYetkiStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.CepYetkiEnd = expriydate;
                        }
                        #endregion

                        #region chkYds
                        if (kod == "chkYds_H")
                        {
                            lisansx.SPI = true;
                           
                        }
                       
                        #endregion

                        #region KRMD1
                        if (kod.Contains("chkKRMD1_H"))
                        {
                            lisansx.COMEX = true;
                            lisansx.KRMD1Start = DateTime.Now;
                            lisansx.KRMD1End = expriydate;
                        }
                        if (kod.Contains("chkKRMD1_A"))
                        {
                            lisansx.KRMD1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.KRMD1End = expriydate;
                        }
                        #endregion
                        #region PD1
                        if (kod.Contains("chkPD1_H"))
                        {
                            lisansx.PayL1 = true;
                            lisansx.PayL1Start = DateTime.Now;
                            lisansx.PayL1End = expriydate;
                        }
                        if (kod.Contains("chkPD1_A"))
                        {
                            lisansx.PayL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayL1End = expriydate;
                        }
                        #endregion
                        #region PD1P
                        if (kod.Contains("chkPD1P_H"))
                        {
                            lisansx.PayL1 = true;
                            lisansx.PayL1Start = DateTime.Now;
                            lisansx.PayL1End = expriydate;
                            lisansx.PayLP = true;
                            lisansx.PayLPStart = DateTime.Now;
                            lisansx.PayLPEnd = expriydate;
                        }
                        if (kod.Contains("chkPD1P_A"))
                        {
                            lisansx.PayL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayL1End = expriydate;
                            lisansx.PayLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayLPEnd = expriydate;
                        }
                        #endregion
                        #region PD2
                        if (kod.Contains("chkPD2_H"))
                        {
                            lisansx.PayL1 = true;
                            lisansx.PayL1Start = DateTime.Now;
                            lisansx.PayL1End = expriydate;
                            lisansx.PayLP = true;
                            lisansx.PayLPStart = DateTime.Now;
                            lisansx.PayLPEnd = expriydate;
                            lisansx.PayL2 = true;
                            lisansx.PayL2Start = DateTime.Now;
                            lisansx.PayL2End = expriydate;
                        }
                        if (kod.Contains("chkPD2_A"))
                        {
                            lisansx.PayL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayL1End = expriydate;
                            lisansx.PayLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayLPEnd = expriydate;
                            lisansx.PayL2Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayL2End = expriydate;
                        }
                        #endregion
                        #region PD2P
                        if (kod.Contains("chkPD2P_H"))
                        {
                            lisansx.PayL1 = true;
                            lisansx.PayL1Start = DateTime.Now;
                            lisansx.PayL1End = expriydate;
                            lisansx.PayLP = true;
                            lisansx.PayLPStart = DateTime.Now;
                            lisansx.PayLPEnd = expriydate;
                            lisansx.PayL2 = true;
                            lisansx.PayL2Start = DateTime.Now;
                            lisansx.PayL2End = expriydate;
                            lisansx.Pd2P = true;
                            lisansx.Pd2PStart = DateTime.Now;
                            lisansx.Pd2PEnd = expriydate;
                        }
                        if (kod.Contains("chkPD2P_A"))
                        {
                            lisansx.PayL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayL1End = expriydate;
                            lisansx.PayLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayLPEnd = expriydate;
                            lisansx.PayL2Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayL2End = expriydate;
                            lisansx.Pd2PStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.Pd2PEnd = expriydate;
                        }
                        #endregion
                        #region PDX
                        if (kod.Contains("chkPDX_H"))
                        {

                            lisansx.PayX = true;
                            lisansx.PayXStart = DateTime.Now;
                            lisansx.PayXEnd = expriydate;
                        }
                        if (kod.Contains("chkPDX_A"))
                        {

                            lisansx.PayXStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayXEnd = expriydate;
                        }
                        #endregion
                        #region PIT
                        if (kod.Contains("chkPIT_H"))
                        {

                            lisansx.PayGS = true;
                            lisansx.PayGSStart = DateTime.Now;
                            lisansx.PayGSEnd = expriydate;
                        }
                        if (kod.Contains("chkPIT_A"))
                        {

                            lisansx.PayGSStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayGSEnd = expriydate;
                        }
                        #endregion
                        #region PITE
                        if (kod.Contains("chkPITE_H"))
                        {

                            lisansx.PITE = true;
                            lisansx.PayPiteStart = DateTime.Now;
                            lisansx.PayPiteEnd = expriydate;
                        }
                        if (kod.Contains("chkPITE_A"))
                        {

                            lisansx.PayPiteStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayPiteEnd = expriydate;
                        }
                        #endregion

                        #region VD1
                        if (kod.Contains("chkVD1_H"))
                        {
                            lisansx.ViopL1 = true;
                            lisansx.ViopL1Start = DateTime.Now;
                            lisansx.ViopL1End = expriydate;
                        }
                        if (kod.Contains("chkVD1_A"))
                        {
                            lisansx.ViopL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopL1End = expriydate;
                        }
                        #endregion
                        #region VD1P
                        if (kod.Contains("chkVD1P_H"))
                        {
                            lisansx.ViopL1 = true;
                            lisansx.ViopL1Start = DateTime.Now;
                            lisansx.ViopL1End = expriydate;
                            lisansx.ViopLP = true;
                            lisansx.ViopLPStart = DateTime.Now;
                            lisansx.ViopLPEnd = expriydate;
                        }
                        if (kod.Contains("chkVD1P_A"))
                        {
                            lisansx.ViopL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopL1End = expriydate;
                            lisansx.ViopLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopLPEnd = expriydate;
                        }
                        #endregion
                        #region VD2
                        if (kod.Contains("chkVD2_H"))
                        {
                            lisansx.ViopL1 = true;
                            lisansx.ViopL1Start = DateTime.Now;
                            lisansx.ViopL1End = expriydate;
                            lisansx.ViopLP = true;
                            lisansx.ViopLPStart = DateTime.Now;
                            lisansx.ViopLPEnd = expriydate;
                            lisansx.ViopL2 = true;
                            lisansx.ViopL2Start = DateTime.Now;
                            lisansx.ViopL2End = expriydate;
                        }
                        if (kod.Contains("chkVD2_A"))
                        {
                            lisansx.ViopL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopL1End = expriydate;
                            lisansx.ViopLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopLPEnd = expriydate;
                            lisansx.ViopL2Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopL2End = expriydate;
                        }
                        #endregion
                        #region VD2P
                        if (kod.Contains("chkVd2P_H"))
                        {
                            lisansx.ViopL1 = true;
                            lisansx.ViopL1Start = DateTime.Now;
                            lisansx.ViopL1End = expriydate;
                            lisansx.ViopLP = true;
                            lisansx.ViopLPStart = DateTime.Now;
                            lisansx.ViopLPEnd = expriydate;
                            lisansx.ViopL2 = true;
                            lisansx.ViopL2Start = DateTime.Now;
                            lisansx.ViopL2End = expriydate;
                            lisansx.Vd2P = true;
                            lisansx.Vd2PStart = DateTime.Now;
                            lisansx.Vd2PEnd = expriydate;
                        }
                        if (kod.Contains("chkVD2P_A"))
                        {
                            lisansx.ViopL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopL1End = expriydate;
                            lisansx.ViopLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopLPEnd = expriydate;
                            lisansx.ViopL2Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopL2End = expriydate;
                            lisansx.Vd2PStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.Vd2PEnd = expriydate;
                        }
                        #endregion
                        #region VIT
                        if (kod.Contains("chkVIT_H"))
                        {
                            lisansx.ViopGS = true;
                            lisansx.ViopGSStart = DateTime.Now;
                            lisansx.ViopGSEnd = expriydate;
                        }
                        if (kod.Contains("chkVIT_A"))
                        {
                            lisansx.ViopGSStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopGSEnd = expriydate;
                        }
                        #endregion

                        #region BD1
                        if (kod.Contains("chkBD1_H"))
                        {
                            lisansx.TahvilL1 = true;
                            lisansx.TahvilL1Start = DateTime.Now;
                            lisansx.TahvilL1End = expriydate;
                        }
                        if (kod.Contains("chkBD1_A"))
                        {
                            lisansx.TahvilL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.TahvilL1End = expriydate;
                        }
                        #endregion
                        #region BD1P
                        if (kod.Contains("chkBD1P_H"))
                        {

                            lisansx.TahvilL1 = true;
                            lisansx.TahvilL1Start = DateTime.Now;
                            lisansx.TahvilL1End = expriydate;
                            lisansx.TahvilLP = true;
                            lisansx.TahvilLPStart = DateTime.Now;
                            lisansx.TahvilLPEnd = expriydate;
                        }
                        if (kod.Contains("chkBD1P_A"))
                        {
                            lisansx.TahvilL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.TahvilL1End = expriydate;
                            lisansx.TahvilLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.TahvilLPEnd = expriydate;
                        }
                        #endregion
                        #region BD2
                        if (kod.Contains("chkBD2_H"))
                        {
                            lisansx.TahvilL1 = true;
                            lisansx.TahvilL1Start = DateTime.Now;
                            lisansx.TahvilL1End = expriydate;
                            lisansx.TahvilLP = true;
                            lisansx.TahvilLPStart = DateTime.Now;
                            lisansx.TahvilLPEnd = expriydate;
                            lisansx.TahvilL2 = true;
                            lisansx.TahvilL2Start = DateTime.Now;
                            lisansx.TahvilL2End = expriydate;
                        }
                        if (kod.Contains("chkBD2_A"))
                        {
                            lisansx.TahvilL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.TahvilL1End = expriydate;
                            lisansx.TahvilLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.TahvilLPEnd = expriydate;
                            lisansx.TahvilL2Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.TahvilL2End = expriydate;
                        }
                        #endregion

                        #region AnalizPro
                        if (kod.Contains("chkAnPro_H"))
                        {
                            lisansx.AnPro = true;
                            lisansx.AnProStart = DateTime.Now;
                            lisansx.AnProEnd = expriydate;
                        }
                        if (kod.Contains("chkAnPro_A"))
                        {
                            lisansx.AnProStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.AnProEnd = expriydate;
                        }
                        #endregion

                        #region SentiL1
                        if (kod.Contains("chkSENTIL1_H"))
                        {
                            lisansx.SentiL1 = true;
                            lisansx.SentiL1Start = DateTime.Now;
                            lisansx.SentiL1End = expriydate;
                        }
                        if (kod.Contains("chkSENTIL1_A"))
                        {
                            lisansx.SentiL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.SentiL1End = expriydate;
                        }
                        #endregion
                        #region SentiL2
                        if (kod.Contains("chkSENTIL2_H"))
                        {
                            lisansx.SentiL1 = true;
                            lisansx.SentiL1Start = DateTime.Now;
                            lisansx.SentiL1End = expriydate;
                            lisansx.SentiL2 = true;
                            lisansx.SentiL2Start = DateTime.Now;
                            lisansx.SentiL2End = expriydate;
                        }
                        if (kod.Contains("chkSENTIL2_A"))
                        {
                            lisansx.SentiL2Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.SentiL2End = expriydate;
                            lisansx.SentiL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.SentiL1End = expriydate;
                        }

                        //#region ROBOT
                        //if (kod.Contains("chkROBOT_H"))
                        //{
                        //    lisansx.ROBOT = true;
                        //    //lisansx.RobotStart = DateTime.Now;
                        //    //lisansx.RobotEnd = expriydate;
                        //}
                        //if (kod.Contains("chkROBOT_A"))
                        //{
                        //    //lisansx.RobotStart = DateTime.Now._FirstDayOfMonth();
                        //    //lisansx.RobotEnd = expriydate;
                        //}
                        //#endregion
                        //#region IDEALGO
                        //if (kod.Contains("chkIDEALGO_H"))
                        //{
                        //    lisansx.CME = true;
                        //    //lisansx.CMEStart = DateTime.Now;
                        //    //lisansx.CMEEnd = expriydate;
                        //}
                        //if (kod.Contains("chkIDEALGO_A"))
                        //{
                        //    //lisansx.CMEStart = DateTime.Now._FirstDayOfMonth();
                        //    //lisansx.CMEEnd = expriydate;
                        //}
                        //#endregion
                        //#region USERDLL
                        //if (kod.Contains("chkUSERDLL_H"))
                        //{
                        //    lisansx.CMEM = true;
                        //    //lisansx.CMEMStart = DateTime.Now;
                        //    //lisansx.CMEMEnd = expriydate;
                        //}
                        //if (kod.Contains("chkUSERDLL_A"))
                        //{
                        //    //lisansx.CMEMStart = DateTime.Now._FirstDayOfMonth();
                        //    //lisansx.CMEMEnd = expriydate;
                        //}
                        //#endregion
                        //#region TEMELANALIZ
                        //if (kod.Contains("chkTEMELANALIZ_H"))
                        //{
                        //    //lisansx.TemelAnaliz = true;
                        //    //lisansx.TemelAnalizStart = DateTime.Now;
                        //    //lisansx.TemelAnalizEnd = expriydate;
                        //}
                        //if (kod.Contains("chkTEMELANALIZ_A"))
                        //{
                        //    //lisansx.TemelAnalizStart = DateTime.Now._FirstDayOfMonth();
                        //    //lisansx.TemelAnalizEnd = expriydate;
                        //}
                        //#endregion
                        //#region BMK
                        //if (kod.Contains("chkBMK_H"))
                        //{
                        //    lisansx.BMK = true;
                        //    lisansx.BMKStart = DateTime.Now;
                        //    lisansx.BMKEnd = expriydate;
                        //}
                        //if (kod.Contains("chkBMK_A"))
                        //{
                        //    lisansx.BMKStart = DateTime.Now._FirstDayOfMonth();
                        //    lisansx.BMKEnd = expriydate;
                        //}
                        //#endregion
                        //#region BMC
                        //if (kod.Contains("chkBMC_H"))
                        //{
                        //    lisansx.BMC = true;
                        //    lisansx.BMCStart = DateTime.Now;
                        //    lisansx.BMCEnd = expriydate;
                        //}
                        //if (kod.Contains("chkBMC_A"))
                        //{
                        //    lisansx.BMCStart = DateTime.Now._FirstDayOfMonth();
                        //    lisansx.BMCEnd = expriydate;
                        //}
                        //#endregion
                        //#region BarSistem
                        //if (kod.Contains("chkBarSistem_H"))
                        //{
                        //    lisansx.BarSistem = true;
                        //    lisansx.BarSistemStart = DateTime.Now;
                        //    lisansx.BarSistemEnd = expriydate;
                        //}
                        //if (kod.Contains("chkBarSistem_A"))
                        //{
                        //    lisansx.BarSistemStart = DateTime.Now._FirstDayOfMonth();
                        //    lisansx.BarSistemEnd = expriydate;
                        //}
                        //#endregion
                        //#region FSYSTEM
                        //if (kod.Contains("chkFSYSTEM_H"))
                        //{
                        //    lisansx.FSystem = true;
                        //    lisansx.FSystemStart = DateTime.Now;
                        //    lisansx.FSystemEnd = expriydate;
                        //}
                        //if (kod.Contains("chkFSYSTEM_A"))
                        //{
                        //    lisansx.FSystemStart = DateTime.Now._FirstDayOfMonth();
                        //    lisansx.FSystemEnd = expriydate;
                        //}
                        //#endregion

                        //#region PARA
                        //if (kod.Contains("chkPARA_H"))
                        //{
                        //    lisansx.PARA = true;
                        //    lisansx.PARAStart = DateTime.Now;
                        //    lisansx.PARAEnd = expriydate;
                        //}
                        //if (kod.Contains("chkPARA_A"))
                        //{
                        //    lisansx.PARAStart = DateTime.Now._FirstDayOfMonth();
                        //    lisansx.PARAEnd = expriydate;
                        //}
                        //#endregion
                        //#region HISSEA
                        //if (kod.Contains("chkHISSEA_H"))
                        //{
                        //    lisansx.HISSEA = true;
                        //    lisansx.HISSEAStart = DateTime.Now;
                        //    lisansx.HISSEAEnd = expriydate;
                        //}
                        //if (kod.Contains("chkHISSEA_A"))
                        //{
                        //    lisansx.HISSEAStart = DateTime.Now._FirstDayOfMonth();
                        //    lisansx.HISSEAEnd = expriydate;
                        //}
                        //#endregion
                        #endregion
                        #region MKK
                        if (kod.Contains("chkMKK_H"))
                        {

                            lisansx.MKK = true;
                            lisansx.MKKStart = DateTime.Now;
                            lisansx.MKKEnd = expriydate;
                        }
                        if (kod.Contains("chkMKK_A"))
                        {

                            lisansx.MKKStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.MKKEnd = expriydate;
                        }
                        #endregion
                        #region TARAMA
                        if (kod.Contains("chkTARAMA_H"))
                        {

                            lisansx.TARAMA = true;
                            lisansx.TaramaStart = DateTime.Now;
                            lisansx.TaramaEnd = expriydate;
                        }
                        if (kod.Contains("chkTARAMA_A"))
                        {

                            lisansx.TaramaStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.TaramaEnd = expriydate;
                        }
                        #endregion
                        #region GKKUL
                        if (kod.Contains("chkGKKUL_H"))
                        {

                            lisansx.GKKUL = true;
                            lisansx.GKKULStart = DateTime.Now;
                            lisansx.GKKULEnd = expriydate;
                        }
                        if (kod.Contains("chkGKKUL_A"))
                        {

                            lisansx.GKKULStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.GKKULEnd = expriydate;
                        }
                        #endregion

                        #region CME
                        if (kod.Contains("chkCME_H"))
                        {

                            lisansx.CME = true;
                            lisansx.CMEStart = DateTime.Now;
                            lisansx.CMEEnd = expriydate;
                        }
                        if (kod.Contains("chkCME_A"))
                        {

                            lisansx.CMEStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.CMEEnd = expriydate;
                        }
                        #endregion
                    }

                    if (lisansx.CepYetki == false && lisansx.ProYetki == false)
                    {
                        lisansx.YayinDurumu = false;
                        MessageBox.Show("Kullanıcının Pro ve Cep Yetkisi Ay Başı Açılsın Olduğu İçin Yayın Durumu Kapalı Olarak Açılacaktır\nKullanıcı Ürünü Ay Başında Kullanabilecektir..!");
                    }
                    if (lisansx.CepYetki == true || lisansx.ProYetki == true)
                    {
                        lisansx.YayinDurumu = true;
                    }

                    crm.LisansDurums.InsertOnSubmit(lisansx);
                    crm.SubmitChanges();
                    userx.LisansDurumId = lisansx.LisansDurumId;
                    crm.Users.InsertOnSubmit(userx);
                    crm.SubmitChanges();
                    ue.SonLisandurumID = lisansx.LisansDurumId;
                    ue.UserId = userx.UserID;
                    crm.UserEvents.InsertOnSubmit(ue);
                    crm.SubmitChanges();
                    formAdminAra.referance.IPport.DataToSend = "CreateUser|" + userx.UserID.ToString() + (char)3;
                }
                #endregion
                #region LisansDüzenleme
                else if (activetalep.TalepTipId == 2) //Lisans Düzenleme
                {
                    var user = crm.Users.FirstOrDefault(x => x.UserName == talep.Usename);
                    //user.ExpiryDate = expriydate;
                    if (user.LisansDurum.YayinDurumu == true)
                    {
                        #region LisansCopy

                        lisansx.YayinDurumu = true;
                        lisansx.ProYetki = user.LisansDurum.ProYetki;

                        lisansx.CepYetki = user.LisansDurum.CepYetki;

                        lisansx.PayL1 = user.LisansDurum.PayL1;

                        lisansx.PayLP = user.LisansDurum.PayLP;

                        lisansx.PayL2 = user.LisansDurum.PayL2;

                        lisansx.Pd2P = user.LisansDurum.Pd2P;

                        lisansx.PayX = user.LisansDurum.PayX;

                        lisansx.PayGS = user.LisansDurum.PayGS;

                        lisansx.PITE = user.LisansDurum.PITE;


                        lisansx.ViopL1 = user.LisansDurum.ViopL1;

                        lisansx.ViopLP = user.LisansDurum.ViopLP;

                        lisansx.ViopL2 = user.LisansDurum.ViopL2;

                        lisansx.Vd2P = user.LisansDurum.Vd2P;

                        lisansx.ViopGS = user.LisansDurum.ViopGS;


                        lisansx.COMEX = user.LisansDurum.COMEX;


                        lisansx.TahvilL1 = user.LisansDurum.TahvilL1;
                        lisansx.TahvilLP = user.LisansDurum.TahvilLP;
                        lisansx.TahvilL2 = user.LisansDurum.TahvilL2;

                        lisansx.AnPro = user.LisansDurum.AnPro;

                        lisansx.DJI = user.LisansDurum.DJI;
                        lisansx.SPI = user.LisansDurum.SPI;
                        lisansx.XETRA = user.LisansDurum.XETRA;
                        lisansx.CBOT = user.LisansDurum.CBOT;
                        lisansx.CBOTM = user.LisansDurum.CBOTM;
                        lisansx.CME = user.LisansDurum.CME;
                        lisansx.CMEM = user.LisansDurum.CMEM;
                        lisansx.EUREX = user.LisansDurum.EUREX;

                        lisansx.ROBOT = user.LisansDurum.ROBOT;

                        lisansx.SCMDownload = user.LisansDurum.SCMDownload;
                        lisansx.SCMRealTıme = user.LisansDurum.SCMRealTıme;
                        lisansx.SCMUsable = user.LisansDurum.SCMUsable;

                        lisansx.Futgck = user.LisansDurum.Futgck;
                        lisansx.WINX = user.LisansDurum.WINX;

                        //lisansx.BMK = user.LisansDurum.BMK;
                        //lisansx.BMC = user.LisansDurum.BMC;
                        //lisansx.TemelAnaliz = user.LisansDurum.TemelAnaliz;
                        //lisansx.BarSistem = user.LisansDurum.BarSistem;
                        //lisansx.FSystem = user.LisansDurum.FSystem;

                        //lisansx.PARA = user.LisansDurum.PARA;
                        //lisansx.HISSEA = user.LisansDurum.HISSEA;
                        lisansx.SentiL1 = user.LisansDurum.SentiL1;
                        lisansx.SentiL2 = user.LisansDurum.SentiL2;
                        lisansx.MKK = user.LisansDurum.MKK;
                        lisansx.TARAMA = user.LisansDurum.TARAMA;
                        lisansx.GKKUL = user.LisansDurum.GKKUL;

                        ////START DATE
                        lisansx.ProYetkiStart = user.LisansDurum.ProYetkiStart;
                        lisansx.CepYetkiStart = user.LisansDurum.CepYetkiStart;
                        lisansx.PayL1Start = user.LisansDurum.PayL1Start;
                        lisansx.PayLPStart = user.LisansDurum.PayLPStart;
                        lisansx.PayL2Start = user.LisansDurum.PayL2Start;
                        lisansx.Pd2PStart = user.LisansDurum.Pd2PStart;
                        lisansx.PayXStart = user.LisansDurum.PayXStart;
                        lisansx.PayGSStart = user.LisansDurum.PayGSStart;
                        lisansx.PayPiteStart = user.LisansDurum.PayPiteStart;

                        lisansx.ViopL1Start = user.LisansDurum.ViopL1Start;
                        lisansx.ViopLPStart = user.LisansDurum.ViopLPStart;
                        lisansx.ViopL2Start = user.LisansDurum.ViopL2Start;
                        lisansx.Vd2PStart = user.LisansDurum.Vd2PStart;
                        lisansx.ViopGSStart = user.LisansDurum.ViopGSStart;

                        lisansx.KRMD1Start = user.LisansDurum.KRMD1Start;

                        lisansx.TahvilL1Start = user.LisansDurum.TahvilL1Start;
                        lisansx.TahvilLPStart = user.LisansDurum.TahvilLPStart;
                        lisansx.TahvilL2Start = user.LisansDurum.TahvilL2Start;

                        lisansx.AnProStart = user.LisansDurum.AnProStart;

                        //lisansx.CMEStart = user.LisansDurum.CMEStart;
                        //lisansx.CMEMStart = user.LisansDurum.CMEMStart;

                        //lisansx.RobotStart = user.LisansDurum.RobotStart;
                        //lisansx.BMKStart = user.LisansDurum.BMKStart;
                        //lisansx.BMCStart = user.LisansDurum.BMCStart;
                        //lisansx.TemelAnalizStart = user.LisansDurum.TemelAnalizStart;
                        //lisansx.BarSistemStart = user.LisansDurum.BarSistemStart;
                        //lisansx.FSystemStart = user.LisansDurum.FSystemStart;

                        //lisansx.PARAStart = user.LisansDurum.PARAStart;
                        //lisansx.HISSEAStart = user.LisansDurum.HISSEAStart;
                        lisansx.SentiL1Start = user.LisansDurum.SentiL1Start;
                        lisansx.SentiL2Start = user.LisansDurum.SentiL2Start;
                        lisansx.MKKStart = user.LisansDurum.MKKStart;
                        lisansx.TaramaStart = user.LisansDurum.TaramaStart;
                        lisansx.GKKULStart = user.LisansDurum.GKKULStart;

                        ////END DATE
                        lisansx.ProYetkiEnd = user.LisansDurum.ProYetkiEnd;
                        lisansx.CepYetkiEnd = user.LisansDurum.CepYetkiEnd;
                        lisansx.PayL1End = user.LisansDurum.PayL1End;
                        lisansx.PayLPEnd = user.LisansDurum.PayLPEnd;
                        lisansx.PayL2End = user.LisansDurum.PayL2End;
                        lisansx.Pd2PEnd = user.LisansDurum.Pd2PEnd;
                        lisansx.PayXEnd = user.LisansDurum.PayXEnd;
                        lisansx.PayGSEnd = user.LisansDurum.PayGSEnd;
                        lisansx.PayPiteEnd = user.LisansDurum.PayPiteEnd;

                        lisansx.ViopL1End = user.LisansDurum.ViopL1End;
                        lisansx.ViopLPEnd = user.LisansDurum.ViopLPEnd;
                        lisansx.ViopL2End = user.LisansDurum.ViopL2End;
                        lisansx.Vd2PEnd = user.LisansDurum.Vd2PEnd;
                        lisansx.ViopGSEnd = user.LisansDurum.ViopGSEnd;

                        lisansx.KRMD1End = user.LisansDurum.KRMD1End;

                        lisansx.TahvilL1End = user.LisansDurum.TahvilL1End;
                        lisansx.TahvilLPEnd = user.LisansDurum.TahvilLPEnd;
                        lisansx.TahvilL2End = user.LisansDurum.TahvilL2End;

                        lisansx.AnProEnd = user.LisansDurum.AnProEnd;

                        //lisansx.CMEEnd = user.LisansDurum.CMEEnd;
                        //lisansx.CMEMEnd = user.LisansDurum.CMEMEnd;

                        //lisansx.RobotEnd = user.LisansDurum.RobotEnd;
                        //lisansx.BMKEnd = user.LisansDurum.BMKEnd;
                        //lisansx.BMCEnd = user.LisansDurum.BMCEnd;
                        //lisansx.TemelAnalizEnd = user.LisansDurum.TemelAnalizEnd;
                        //lisansx.BarSistemEnd = user.LisansDurum.BarSistemEnd;
                        //lisansx.FSystemEnd = user.LisansDurum.FSystemEnd;

                        //lisansx.PARAEnd = user.LisansDurum.PARAEnd;
                        //lisansx.HISSEAEnd = user.LisansDurum.HISSEAEnd;
                        lisansx.SentiL1End = user.LisansDurum.SentiL1End;
                        lisansx.SentiL2End = user.LisansDurum.SentiL2End;
                        lisansx.MKKEnd = user.LisansDurum.MKKEnd;
                        lisansx.TaramaEnd = user.LisansDurum.TaramaEnd;
                        lisansx.GKKULEnd = user.LisansDurum.GKKULEnd;
                       
                        #endregion
                    }
                    lisansx.YayinDurumu = true;

                    

                   

                    if (lisansx.CepYetki == true) 
                    {
                        lisansx.CepYetkiEnd = expriydate;

                    }
                    if (lisansx.COMEX == true) // krmd1
                    {
                        lisansx.KRMD1End = expriydate;

                    }
                    if (lisansx.PayL1 == true && lisansx.PayL1End != LastDayOfMonth())
                    {
                        lisansx.PayL1End = expriydate;

                    }
                    if (lisansx.PayLP == true && lisansx.PayLPEnd != LastDayOfMonth())
                    {
                        lisansx.PayLPEnd = expriydate;

                    }
                    if (lisansx.PayL2 == true && lisansx.PayL2End != LastDayOfMonth())
                    {
                        lisansx.PayL2End = expriydate;

                    }
                    if (lisansx.Pd2P == true && lisansx.Pd2PEnd!= LastDayOfMonth())
                    {
                        lisansx.Pd2PEnd = expriydate;

                    }
                    if (lisansx.PayX == true) // pay endeks
                    {
                        lisansx.PayXEnd = expriydate;

                    }
                    if (lisansx.PayGS == true) // pit 
                    {
                        lisansx.PayGSEnd = expriydate;

                    }
                    if (lisansx.PITE == true)
                    {
                        lisansx.PayPiteEnd = expriydate;

                    }
                    if (lisansx.ViopL1 == true && lisansx.ViopL1End != LastDayOfMonth())
                    {
                        lisansx.ViopL1End = expriydate;

                    }
                    if (lisansx.ViopLP == true && lisansx.ViopLPEnd != LastDayOfMonth())
                    {
                        lisansx.ViopLPEnd = expriydate;

                    }
                    if (lisansx.ViopL2 == true && lisansx.ViopL2End != LastDayOfMonth())
                    {
                        lisansx.ViopL2End = expriydate;

                    }
                    if (lisansx.Vd2P == true && lisansx.Vd2PEnd != LastDayOfMonth())
                    {
                        lisansx.Vd2PEnd = expriydate;

                    }
                    if (lisansx.ViopGS == true) // viop akd
                    {
                        lisansx.ViopGSEnd = expriydate;

                    }

                    if (lisansx.SentiL1 == true)
                    {
                        lisansx.SentiL1End = expriydate;

                    }
                    if (lisansx.SentiL2 == true && lisansx.SentiL2End != LastDayOfMonth())
                    {
                        lisansx.SentiL2End = expriydate;

                    }
                    if (lisansx.AnPro == true) //Anpro
                    {
                        lisansx.AnProEnd = expriydate;

                    }
                    if (lisansx.MKK == true)
                    {
                        lisansx.MKKEnd = expriydate;
                    }
                    if (lisansx.TARAMA == true)
                    {
                        lisansx.TaramaEnd = expriydate;
                    }
                    if (lisansx.GKKUL == true)
                    {
                        lisansx.GKKULEnd = expriydate;
                    }

                    var lisanlar = crm.KurumTalepLisans.Where(x => x.TalepId == talep.id).ToList();
                    for (int i = 0; i < lisanlar.Count; i++)
                    {
                        var kod = lisanlar[i].LisansKod;

                        #region chkDesktop
                        if (kod == "chkDesktop")
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.ProYetki == false) acilanlar.Append("ProYetki");
                            lisansx.ProYetki = true;
                            lisansx.ProYetkiStart = DateTime.Now;
                            lisansx.ProYetkiEnd = expriydate;
                        }
                        if (kod.Contains("chkDesktop_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.ProYetkiStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.ProYetkiEnd = expriydate;
                        }
                        if (kod.Contains("chkDesktop_K"))
                        {

                            lisansx.ProYetkiEnd = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region chkMobil
                        if (kod == "chkMobil")
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.CepYetki == false) acilanlar.Append("CepYetki");
                            lisansx.CepYetki = true;
                            lisansx.CepYetkiStart = DateTime.Now;
                            lisansx.CepYetkiEnd = expriydate;
                        }
                        if (kod.Contains("chkMobil_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.CepYetkiStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.CepYetkiEnd = expriydate;
                        }
                        if (kod.Contains("chkMobil_K"))
                        {

                            lisansx.CepYetkiEnd = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region KRMD1
                        if (kod.Contains("chkKRMD1_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.COMEX == false) acilanlar.Append("KRMD1");
                            lisansx.COMEX = true;
                            lisansx.KRMD1Start = DateTime.Now;
                            lisansx.KRMD1End = expriydate;

                        }
                        if (kod.Contains("chkKRMD1_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.KRMD1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.KRMD1End = expriydate;
                        }
                        if (kod.Contains("chkKRMD1_K"))
                        {
                            lisansx.KRMD1End = DateTime.Now._LastDayOfMonth();

                        }
                        #endregion
                        #region PD1
                        if (kod.Contains("chkPD1_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.PayL1 == false) acilanlar.Append("PayL1");
                            lisansx.PayL1 = true;
                            lisansx.PayL1Start = DateTime.Now;
                            lisansx.PayL1End = expriydate;
                        }
                        if (kod.Contains("chkPD1_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.PayL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayL1End = expriydate;
                        }
                        if (kod.Contains("chkPD1_K"))
                        {

                            lisansx.PayL1End = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region PD1P
                        if (kod.Contains("chkPD1P_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.PayL1 == false) acilanlar.Append("PayL1");
                            lisansx.PayL1 = true;
                            lisansx.PayL1Start = DateTime.Now;
                            lisansx.PayL1End = expriydate;
                            if (user.LisansDurum.PayLP == false) acilanlar.Append("PayLP");
                            lisansx.PayLP = true;
                            lisansx.PayLPStart = DateTime.Now;
                            lisansx.PayLPEnd = expriydate;
                        }
                        if (kod.Contains("chkPD1P_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.PayL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayL1End = expriydate;
                            lisansx.PayLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayLPEnd = expriydate;
                        }
                        if (kod.Contains("chkPD1P_K"))
                        {

                            lisansx.PayL1End = DateTime.Now._LastDayOfMonth();
                            lisansx.PayLPEnd = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region PD2
                        if (kod.Contains("chkPD2_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.PayL1 == false) acilanlar.Append("PayL1");
                            lisansx.PayL1 = true;
                            lisansx.PayL1Start = DateTime.Now;
                            lisansx.PayL1End = expriydate;
                            if (user.LisansDurum.PayLP == false) acilanlar.Append("PayLP");
                            lisansx.PayLP = true;
                            lisansx.PayLPStart = DateTime.Now;
                            lisansx.PayLPEnd = expriydate;
                            if (user.LisansDurum.PayL2 == false) acilanlar.Append("PayL2");
                            lisansx.PayL2 = true;
                            lisansx.PayL2Start = DateTime.Now;
                            lisansx.PayL2End = expriydate;
                        }
                        if (kod.Contains("chkPD2_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.PayL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayL1End = expriydate;
                            lisansx.PayLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayLPEnd = expriydate;
                            lisansx.PayL2Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayL2End = expriydate;
                        }
                        if (kod.Contains("chkPD2_K"))
                        {

                            lisansx.PayL1End = DateTime.Now._LastDayOfMonth();
                            lisansx.PayLPEnd = DateTime.Now._LastDayOfMonth();
                            lisansx.PayL2End = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region PD2P
                        if (kod.Contains("chkPD2P_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.PayL1 == false) acilanlar.Append("PayL1");
                            lisansx.PayL1 = true;
                            lisansx.PayL1Start = DateTime.Now;
                            lisansx.PayL1End = expriydate;
                            if (user.LisansDurum.PayLP == false) acilanlar.Append("PayLP");
                            lisansx.PayLP = true;
                            lisansx.PayLPStart = DateTime.Now;
                            lisansx.PayLPEnd = expriydate;
                            if (user.LisansDurum.PayL2 == false) acilanlar.Append("PayL2");
                            lisansx.PayL2 = true;
                            lisansx.PayL2Start = DateTime.Now;
                            lisansx.PayL2End = expriydate;
                            if (user.LisansDurum.Pd2P == false) acilanlar.Append("Pd2P");
                            lisansx.Pd2P = true;
                            lisansx.Pd2PStart = DateTime.Now;
                            lisansx.Pd2PEnd = expriydate;
                        }
                        if (kod.Contains("chkPD2P_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.PayL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayL1End = expriydate;
                            lisansx.PayLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayLPEnd = expriydate;
                            lisansx.PayL2Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayL2End = expriydate;
                            lisansx.Pd2PStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.Pd2PEnd = expriydate;
                        }
                        if (kod.Contains("chkPD2P_K"))
                        {
                            lisansx.PayL1End = DateTime.Now._LastDayOfMonth();
                            lisansx.PayLPEnd = DateTime.Now._LastDayOfMonth();
                            lisansx.PayL2End = DateTime.Now._LastDayOfMonth();
                            lisansx.Pd2PEnd = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region PDX
                        if (kod.Contains("chkPDX_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.PayX == false) acilanlar.Append("PayX");
                            lisansx.PayX = true;
                            lisansx.PayXStart = DateTime.Now;
                            lisansx.PayXEnd = expriydate;
                        }
                        if (kod.Contains("chkPDX_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.PayXStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayXEnd = expriydate;
                        }
                        if (kod.Contains("chkPDX_K"))
                        {

                            lisansx.PayXEnd = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region chkYds
                        if (kod.Contains("chkYds_H"))
                        {
                            lisansx.SPI = true;

                        }
                        if (kod.Contains("chkYds_K"))
                        {
                            lisansx.SPI = false;

                        }
                        #endregion
                        #region PIT
                        if (kod.Contains("chkPIT_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.PayGS == false) acilanlar.Append("PayGS");
                            lisansx.PayGS = true;
                            lisansx.PayGSStart = DateTime.Now;
                            lisansx.PayGSEnd = expriydate;
                        }
                        if (kod.Contains("chkPIT_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.PayGSStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayGSEnd = expriydate;
                        }
                        if (kod.Contains("chkPIT_K"))
                        {
                            lisansx.PayGSEnd = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region PITE
                        if (kod.Contains("chkPITE_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.PITE == false) acilanlar.Append("PITE");
                            lisansx.PITE = true;
                            lisansx.PayPiteStart = DateTime.Now;
                            lisansx.PayPiteEnd = expriydate;
                        }
                        if (kod.Contains("chkPITE_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.PayPiteStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.PayPiteEnd = expriydate;
                        }
                        if (kod.Contains("chkPITE_K"))
                        {

                            lisansx.PayPiteEnd = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion

                        #region VD1
                        if (kod.Contains("chkVD1_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.ViopL1 == false) acilanlar.Append("ViopL1");
                            lisansx.ViopL1 = true;
                            lisansx.ViopL1Start = DateTime.Now;
                            lisansx.ViopL1End = expriydate;
                        }
                        if (kod.Contains("chkVD1_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.ViopL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopL1End = expriydate;
                        }
                        if (kod.Contains("chkVD1_K"))
                        {

                            lisansx.ViopL1End = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region VD1P
                        if (kod.Contains("chkVD1P_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.ViopL1 == false) acilanlar.Append("ViopL1");
                            lisansx.ViopL1 = true;
                            lisansx.ViopL1Start = DateTime.Now;
                            lisansx.ViopL1End = expriydate;
                            if (user.LisansDurum.ViopLP == false) acilanlar.Append("ViopLP");
                            lisansx.ViopLP = true;
                            lisansx.ViopLPStart = DateTime.Now;
                            lisansx.ViopLPEnd = expriydate;
                        }
                        if (kod.Contains("chkVD1P_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.ViopL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopL1End = expriydate;
                            lisansx.ViopLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopLPEnd = expriydate;
                        }
                        if (kod.Contains("chkVD1P_K"))
                        {

                            lisansx.ViopL1End = DateTime.Now._LastDayOfMonth();
                            lisansx.ViopLPEnd = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region VD2
                        if (kod.Contains("chkVD2_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.ViopL1 == false) acilanlar.Append("ViopL1");
                            lisansx.ViopL1 = true;
                            lisansx.ViopL1Start = DateTime.Now;
                            lisansx.ViopL1End = expriydate;
                            if (user.LisansDurum.ViopLP == false) acilanlar.Append("ViopLP");
                            lisansx.ViopLP = true;
                            lisansx.ViopLPStart = DateTime.Now;
                            lisansx.ViopLPEnd = expriydate;
                            if (user.LisansDurum.ViopL2 == false) acilanlar.Append("ViopL2");
                            lisansx.ViopL2 = true;
                            lisansx.ViopL2Start = DateTime.Now;
                            lisansx.ViopL2End = expriydate;
                        }
                        if (kod.Contains("chkVD2_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.ViopL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopL1End = expriydate;
                            lisansx.ViopLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopLPEnd = expriydate;
                            lisansx.ViopL2Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopL2End = expriydate;
                        }
                        if (kod.Contains("chkVD2_K"))
                        {

                            lisansx.ViopL1End = DateTime.Now._LastDayOfMonth();
                            lisansx.ViopLPEnd = DateTime.Now._LastDayOfMonth();
                            lisansx.ViopL2End = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region VD2P
                        if (kod.Contains("chkVD2P_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.ViopL1 == false) acilanlar.Append("ViopL1");
                            lisansx.ViopL1 = true;
                            lisansx.ViopL1Start = DateTime.Now;
                            lisansx.ViopL1End = expriydate;
                            if (user.LisansDurum.ViopLP == false) acilanlar.Append("ViopLP");
                            lisansx.ViopLP = true;
                            lisansx.ViopLPStart = DateTime.Now;
                            lisansx.ViopLPEnd = expriydate;
                            if (user.LisansDurum.ViopL2 == false) acilanlar.Append("ViopL2");
                            lisansx.ViopL2 = true;
                            lisansx.ViopL2Start = DateTime.Now;
                            lisansx.ViopL2End = expriydate;
                            if (user.LisansDurum.Vd2P == false) acilanlar.Append("Vd2P");
                            lisansx.Vd2P = true;
                            lisansx.Vd2PStart = DateTime.Now;
                            lisansx.Vd2PEnd = expriydate;
                        }
                        if (kod.Contains("chkVD2P_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.ViopL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopL1End = expriydate;
                            lisansx.ViopLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopLPEnd = expriydate;
                            lisansx.ViopL2Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopL2End = expriydate;
                            lisansx.Vd2PStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.Vd2PEnd = expriydate;
                        }
                        if (kod.Contains("chkVD2P_K"))
                        {

                            lisansx.ViopL1End = DateTime.Now._LastDayOfMonth();
                            lisansx.ViopLPEnd = DateTime.Now._LastDayOfMonth();
                            lisansx.ViopL2End = DateTime.Now._LastDayOfMonth();
                            lisansx.Vd2PEnd = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region VIT
                        if (kod.Contains("chkVIT_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.ViopGS == false) acilanlar.Append("ViopGS");
                            lisansx.ViopGS = true;
                            lisansx.ViopGSStart = DateTime.Now;
                            lisansx.ViopGSEnd = expriydate;
                        }
                        if (kod.Contains("chkVIT_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.ViopGSStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.ViopGSEnd = expriydate;
                        }
                        if (kod.Contains("chkVIT_K"))
                        {

                            lisansx.ViopGSEnd = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion

                        #region BD1
                        if (kod.Contains("chkBD1_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.TahvilL1 == false) acilanlar.Append("TahvilL1");
                            lisansx.TahvilL1 = true;
                            lisansx.TahvilL1Start = DateTime.Now;
                            lisansx.TahvilL1End = expriydate;
                        }
                        if (kod.Contains("chkBD1_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.TahvilL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.TahvilL1End = expriydate;
                        }
                        if (kod.Contains("chkBD1_K"))
                        {

                            lisansx.TahvilL1End = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region BD1P
                        if (kod.Contains("chkBD1P_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.TahvilL1 == false) acilanlar.Append("TahvilL1");
                            lisansx.TahvilL1 = true;
                            lisansx.TahvilL1Start = DateTime.Now;
                            lisansx.TahvilL1End = expriydate;
                            if (user.LisansDurum.TahvilLP == false) acilanlar.Append("TahvilLP");
                            lisansx.TahvilLP = true;
                            lisansx.TahvilLPStart = DateTime.Now;
                            lisansx.TahvilLPEnd = expriydate;
                        }
                        if (kod.Contains("chkBD1P_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.TahvilL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.TahvilL1End = expriydate;
                            lisansx.TahvilLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.TahvilLPEnd = expriydate;
                        }
                        if (kod.Contains("chkBD1P_K"))
                        {

                            lisansx.TahvilL1End = DateTime.Now._LastDayOfMonth();
                            lisansx.TahvilLPEnd = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region BD2
                        if (kod.Contains("chkBD2_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.TahvilL1 == false) acilanlar.Append("TahvilL1");
                            lisansx.TahvilL1 = true;
                            lisansx.TahvilL1Start = DateTime.Now;
                            lisansx.TahvilL1End = expriydate;
                            if (user.LisansDurum.TahvilLP == false) acilanlar.Append("TahvilLP");
                            lisansx.TahvilLP = true;
                            lisansx.TahvilLPStart = DateTime.Now;
                            lisansx.TahvilLPEnd = expriydate;
                            if (user.LisansDurum.TahvilL2 == false) acilanlar.Append("TahvilL2");
                            lisansx.TahvilL2 = true;
                            lisansx.TahvilL2Start = DateTime.Now;
                            lisansx.TahvilL2End = expriydate;
                        }
                        if (kod.Contains("chkBD2_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.TahvilL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.TahvilL1End = expriydate;
                            lisansx.TahvilLPStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.TahvilLPEnd = expriydate;
                            lisansx.TahvilL2Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.TahvilL2End = expriydate;
                        }
                        if (kod.Contains("chkBD2_K"))
                        {

                            lisansx.TahvilL1End = DateTime.Now._LastDayOfMonth();
                            lisansx.TahvilLPEnd = DateTime.Now._LastDayOfMonth();
                            lisansx.TahvilL2End = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region AnalizPro
                        if (kod.Contains("chkAnPro_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.AnPro == false) acilanlar.Append("AnalizPro");
                            lisansx.AnPro = true;
                            lisansx.AnProStart = DateTime.Now;
                            lisansx.AnProEnd = expriydate;
                        }
                        if (kod.Contains("chkAnPro_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.AnProStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.AnProEnd = expriydate;
                        }
                        if (kod.Contains("chkAnPro_K"))
                        {

                            lisansx.AnProEnd = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region SentiL1
                        if (kod.Contains("chkSENTIL1_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.SentiL1 == false) acilanlar.Append("SentiL1");
                            lisansx.SentiL1 = true;
                            lisansx.SentiL1Start = DateTime.Now;
                            lisansx.SentiL1End = expriydate;
                        }
                        if (kod.Contains("chkSENTIL1_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.SentiL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.SentiL1End = expriydate;
                        }
                        if (kod.Contains("chkSENTIL1_K"))
                        {


                            lisansx.SentiL1End = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion
                        #region SentiL2
                        if (kod.Contains("chkSENTIL2_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.SentiL1 == false) acilanlar.Append("SentiL1");
                            lisansx.SentiL1 = true;
                            lisansx.SentiL1Start = DateTime.Now;
                            lisansx.SentiL1End = expriydate;
                            if (user.LisansDurum.SentiL2 == false) acilanlar.Append("SentiL2");
                            lisansx.SentiL2 = true;
                            lisansx.SentiL2Start = DateTime.Now;
                            lisansx.SentiL2End = expriydate;
                        }
                        if (kod.Contains("chkSENTIL2_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.SentiL1Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.SentiL1End = expriydate;
                            lisansx.SentiL2Start = DateTime.Now._FirstDayOfMonth();
                            lisansx.SentiL2End = expriydate;
                        }
                        if (kod.Contains("chkSENTIL2_K"))
                        {

                            lisansx.SentiL1End = DateTime.Now._LastDayOfMonth();
                            lisansx.SentiL2End = DateTime.Now._LastDayOfMonth();
                        }
                        #endregion

                        #region MKK
                        if (kod.Contains("chkMKK_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.MKK == false) acilanlar.Append("MKK");
                            lisansx.MKK = true;
                            lisansx.MKKStart = DateTime.Now;
                            lisansx.MKKEnd = expriydate;

                        }
                        if (kod.Contains("chkMKK_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.MKKStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.MKKEnd = expriydate;
                        }
                        if (kod.Contains("chkMKK_K"))
                        {
                            lisansx.MKKEnd = DateTime.Now._LastDayOfMonth();

                        }
                        #endregion

                        #region TARAMA
                        if (kod.Contains("chkTARAMA_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.TARAMA == false) acilanlar.Append("TARAMA");
                            lisansx.TARAMA = true;
                            lisansx.TaramaStart = DateTime.Now;
                            lisansx.TaramaEnd = expriydate;

                        }
                        if (kod.Contains("chkTARAMA_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.TaramaStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.TaramaEnd = expriydate;
                        }
                        if (kod.Contains("chkTARAMA_K"))
                        {
                            lisansx.TaramaEnd = DateTime.Now._LastDayOfMonth();

                        }
                        #endregion

                        #region GKKUL
                        if (kod.Contains("chkGKKUL_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.GKKUL == false) acilanlar.Append("GKKUL");
                            lisansx.GKKUL = true;
                            lisansx.GKKULStart = DateTime.Now;
                            lisansx.GKKULEnd = expriydate;

                        }
                        if (kod.Contains("chkGKKUL_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.GKKULStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.GKKULEnd = expriydate;
                        }
                        if (kod.Contains("chkGKKUL_K"))
                        {
                            lisansx.GKKULEnd = DateTime.Now._LastDayOfMonth();

                        }
                        #endregion

                        #region CME
                        if (kod.Contains("chkCME_H"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            if (user.LisansDurum.CME == false) acilanlar.Append("CME");
                            lisansx.CME = true;
                            lisansx.CMEStart = DateTime.Now;
                            lisansx.CMEEnd = expriydate;

                        }
                        if (kod.Contains("chkCME_A"))
                        {
                            if (user.ExpiryDate < expriydate)
                            {
                                user.ExpiryDate = expriydate;
                            }
                            lisansx.CMEStart = DateTime.Now._FirstDayOfMonth();
                            lisansx.CMEEnd = expriydate;
                        }
                        if (kod.Contains("chkCME_K"))
                        {
                            lisansx.CMEEnd = DateTime.Now._LastDayOfMonth();

                        }
                        #endregion


                    }

                    if (lisansx.CepYetki == false && lisansx.ProYetki == false)
                    {
                        lisansx.YayinDurumu = false;
                    }
                    //if ((lisansx.ProYetkiEnd != null && lisansx.ProYetkiEnd == DateTime.Now._LastDayOfMonth()) && (lisansx.CepYetkiEnd != null && lisansx.CepYetkiEnd == DateTime.Now._LastDayOfMonth()))
                    //{
                    //    user.ExpiryDate = DateTime.Now._LastDayOfMonth();
                    //}
                    //if ((lisansx.ProYetkiEnd != null && lisansx.ProYetkiEnd == DateTime.Now._LastDayOfMonth()) && (lisansx.CepYetki == false && (lisansx.CepYetkiStart == null || lisansx.CepYetkiStart != DateTime.Now._FirstDayOfMonth())))
                    //{
                    //    user.ExpiryDate = DateTime.Now._LastDayOfMonth();
                    //}
                    //if ((lisansx.CepYetkiEnd != null && lisansx.CepYetkiEnd == DateTime.Now._LastDayOfMonth()) && (lisansx.ProYetki == false && (lisansx.ProYetkiStart == null || lisansx.ProYetkiStart != DateTime.Now._FirstDayOfMonth())))
                    //{
                    //    user.ExpiryDate = DateTime.Now._LastDayOfMonth();
                    //}





                    crm.LisansDurums.InsertOnSubmit(lisansx);
                    crm.SubmitChanges();



                    UserEvent uex = new UserEvent();
                    uex.EventTypeId = 6;
                    uex.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
                    uex.EventTarih = DateTime.Now;
                    uex.IP = formAdminAra.referance.IpAdress;
                    uex.HostName = formAdminAra.referance.HostName;
                    uex.AcilanLisans = acilanlar.ToString();

                    uex.LisansDurum = lisansx;
                    user.LisansDurum = lisansx;

                    uex.UserId = user.UserID;

                    crm.UserEvents.InsertOnSubmit(uex);
                    crm.SubmitChanges();

                    formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + user.UserID.ToString() + (char)3;
                }
                #endregion
                #region Iptal
                //else if (activetalep.TalepTipId == 3) //İptal
                //{
                //    var user = crm.Users.FirstOrDefault(x=>x.UserName==talep.Usename);
                //    user.ExpiryDate = DateTime.Now._LastDayOfMonth();
                //    crm.SubmitChanges();
                //    formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + user.UserID.ToString() + (char)3;
                //}
                #region Iptal
                else if (activetalep.TalepTipId == 3) //İptal
                {
                    var user = crm.Users.FirstOrDefault(x => x.UserName == talep.Usename);
                    //var dgr = "";
                    //if (dgr != "1" && dgr != "2")
                    //{
                    user.ExpiryDate = DateTime.Now._LastDayOfMonth();
                    crm.SubmitChanges();
                    //}
                    //else
                    //{
                    if (user.LisansDurum.YayinDurumu == true)
                    {
                        #region LisansCopy

                        lisansx.YayinDurumu = true;
                        lisansx.ProYetki = user.LisansDurum.ProYetki;  ///???
                        lisansx.CepYetki = user.LisansDurum.CepYetki;  ///???
                        lisansx.PayL1 = user.LisansDurum.PayL1;
                        lisansx.PayLP = user.LisansDurum.PayLP;
                        lisansx.PayL2 = user.LisansDurum.PayL2;
                        lisansx.Pd2P = user.LisansDurum.Pd2P;
                        lisansx.PayX = user.LisansDurum.PayX;
                        lisansx.PayGS = user.LisansDurum.PayGS;
                        lisansx.PITE = user.LisansDurum.PITE;

                        lisansx.ViopL1 = user.LisansDurum.ViopL1;
                        lisansx.ViopLP = user.LisansDurum.ViopLP;
                        lisansx.ViopL2 = user.LisansDurum.ViopL2;
                        lisansx.Vd2P = user.LisansDurum.Vd2P;
                        lisansx.ViopGS = user.LisansDurum.ViopGS;

                        lisansx.COMEX = user.LisansDurum.COMEX;


                        lisansx.TahvilL1 = user.LisansDurum.TahvilL1;
                        lisansx.TahvilLP = user.LisansDurum.TahvilLP;
                        lisansx.TahvilL2 = user.LisansDurum.TahvilL2;

                        lisansx.AnPro = user.LisansDurum.AnPro;

                        lisansx.CME = user.LisansDurum.CME;
                        lisansx.CMEM = user.LisansDurum.CMEM;
                        lisansx.EUREX = user.LisansDurum.EUREX;

                        lisansx.ROBOT = user.LisansDurum.ROBOT;
                        //lisansx.BMK = user.LisansDurum.BMK;
                        //lisansx.BMC = user.LisansDurum.BMC;
                        //lisansx.TemelAnaliz = user.LisansDurum.TemelAnaliz;
                        //lisansx.BarSistem = user.LisansDurum.BarSistem;
                        //lisansx.FSystem = user.LisansDurum.FSystem;

                        //lisansx.PARA = user.LisansDurum.PARA;
                        //lisansx.HISSEA = user.LisansDurum.HISSEA;
                        lisansx.SentiL1 = user.LisansDurum.SentiL1;
                        lisansx.SentiL2 = user.LisansDurum.SentiL2;
                        lisansx.MKK = user.LisansDurum.MKK;
                        lisansx.TARAMA = user.LisansDurum.TARAMA;
                        lisansx.GKKUL = user.LisansDurum.GKKUL;

                        ////START DATE
                        lisansx.ProYetkiStart = user.LisansDurum.ProYetkiStart;
                        lisansx.CepYetkiStart = user.LisansDurum.CepYetkiStart;
                        lisansx.PayL1Start = user.LisansDurum.PayL1Start;
                        lisansx.PayLPStart = user.LisansDurum.PayLPStart;
                        lisansx.PayL2Start = user.LisansDurum.PayL2Start;
                        lisansx.Pd2PStart = user.LisansDurum.Pd2PStart;
                        lisansx.PayXStart = user.LisansDurum.PayXStart;
                        lisansx.PayGSStart = user.LisansDurum.PayGSStart;
                        lisansx.PayPiteStart = user.LisansDurum.PayPiteStart;

                        lisansx.ViopL1Start = user.LisansDurum.ViopL1Start;
                        lisansx.ViopLPStart = user.LisansDurum.ViopLPStart;
                        lisansx.ViopL2Start = user.LisansDurum.ViopL2Start;
                        lisansx.Vd2PStart = user.LisansDurum.Vd2PStart;
                        lisansx.ViopGSStart = user.LisansDurum.ViopGSStart;

                        lisansx.KRMD1Start = user.LisansDurum.KRMD1Start;

                        lisansx.TahvilL1Start = user.LisansDurum.TahvilL1Start;
                        lisansx.TahvilLPStart = user.LisansDurum.TahvilLPStart;
                        lisansx.TahvilL2Start = user.LisansDurum.TahvilL2Start;

                        lisansx.AnProStart = user.LisansDurum.AnProStart;

                        //lisansx.CMEStart = user.LisansDurum.CMEStart;
                        //lisansx.CMEMStart = user.LisansDurum.CMEMStart;

                        //lisansx.RobotStart = user.LisansDurum.RobotStart;
                        //lisansx.BMKStart = user.LisansDurum.BMKStart;
                        //lisansx.BMCStart = user.LisansDurum.BMCStart;
                        //lisansx.TemelAnalizStart = user.LisansDurum.TemelAnalizStart;
                        //lisansx.BarSistemStart = user.LisansDurum.BarSistemStart;
                        //lisansx.FSystemStart = user.LisansDurum.FSystemStart;

                        //lisansx.PARAStart = user.LisansDurum.PARAStart;
                        //lisansx.HISSEAStart = user.LisansDurum.HISSEAStart;
                        lisansx.SentiL1Start = user.LisansDurum.SentiL1Start;
                        lisansx.SentiL2Start = user.LisansDurum.SentiL2Start;
                        lisansx.MKKStart = user.LisansDurum.MKKStart;
                        lisansx.TaramaStart = user.LisansDurum.TaramaStart;
                        lisansx.GKKULStart = user.LisansDurum.GKKULStart;
                        ////END DATE

                        if (lisansx.ProYetki) user.LisansDurum.ProYetkiEnd = user.ExpiryDate;
                        if (lisansx.CepYetki) user.LisansDurum.CepYetkiEnd = user.ExpiryDate;
                        if (lisansx.PayL1) user.LisansDurum.PayL1End = user.ExpiryDate;
                        if (lisansx.PayLP) user.LisansDurum.PayLPEnd = user.ExpiryDate;
                        if (lisansx.PayL2) user.LisansDurum.PayL2End = user.ExpiryDate;
                        if (lisansx.Pd2P) user.LisansDurum.Pd2PEnd = user.ExpiryDate;
                        if (lisansx.PayX) user.LisansDurum.PayXEnd = user.ExpiryDate;
                        if (lisansx.PayGS) user.LisansDurum.PayGSEnd = user.ExpiryDate;
                        if (lisansx.PITE) user.LisansDurum.PayPiteEnd = user.ExpiryDate;

                        if (lisansx.ViopL1) user.LisansDurum.ViopL1End = user.ExpiryDate;
                        if (lisansx.ViopLP) user.LisansDurum.ViopLPEnd = user.ExpiryDate;
                        if (lisansx.ViopL2) user.LisansDurum.ViopL2End = user.ExpiryDate;
                        if (lisansx.Vd2P) user.LisansDurum.Vd2PEnd = user.ExpiryDate;
                        if (lisansx.ViopGS) user.LisansDurum.ViopGSEnd = user.ExpiryDate;

                        if (lisansx.COMEX) user.LisansDurum.KRMD1End = user.ExpiryDate;

                        if (lisansx.TahvilL1) user.LisansDurum.TahvilL1End = user.ExpiryDate;
                        if (lisansx.TahvilLP) user.LisansDurum.TahvilLPEnd = user.ExpiryDate;
                        if (lisansx.TahvilL2) user.LisansDurum.TahvilL2End = user.ExpiryDate;

                        if (lisansx.AnPro) user.LisansDurum.AnProEnd = user.ExpiryDate;

                        if (lisansx.SentiL1) user.LisansDurum.SentiL1End = user.ExpiryDate;
                        if (lisansx.SentiL2) user.LisansDurum.SentiL2End = user.ExpiryDate;
                        if (lisansx.MKK) user.LisansDurum.MKKEnd = user.ExpiryDate;
                        if (lisansx.TARAMA) user.LisansDurum.TaramaEnd = user.ExpiryDate;
                        if (lisansx.GKKUL) user.LisansDurum.GKKULEnd = user.ExpiryDate;
                        #endregion
                    }
                    crm.LisansDurums.InsertOnSubmit(lisansx);
                    crm.SubmitChanges();
                    //}
                    //crm.SubmitChanges();
                    formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + user.UserID.ToString() + (char)3;
                }
                #endregion
                #endregion

                talep.Aciklama = txtTalepAciklama.Text;
                talep.TalepDurumId = 3;
                talep.TalebiYapan = cal.calisanID;
                talep.TalepYapilmaTarihi = DateTime.Now;


                crm.SubmitChanges();
                MessageBox.Show("Talep Yapılmıştır");
                this.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //void lisanskontrol(string lisansadi)
        //{
        //    if (lisansadi==1)
        //    {

        //    }
        //}
        private void button1_Click(object sender, EventArgs e)
        {
            var crm = new crmDFNDataContext();
            var talep = crm.KurumTalepleris.FirstOrDefault(x => x.id == acriveTalepId);
            if (activetalep.TalepTipId != 1)
            {
                MessageBox.Show("Sadece Yeni Kullanıcıya mail atabilirsiniz"); return;
            }
            var user = crm.Users.FirstOrDefault(x => x.UserName == talep.Usename);
            SendMail(user);
        }
        void SendMail(User mailuser)
        {

            try
            {
                if (mailuser.Iletisim == null || mailuser.Iletisim.email == "" || mailuser.Iletisim.email.Contains("@") == false)
                {
                    MessageBox.Show("Müşteri Mail bilgisinde hata var");
                }
                else
                {
                    try
                    {
                        var sb = new StringBuilder();
                        sb.Append("Merhabalar\nAdınıza tahsis edilen IDEAL Programı için login bilgileriniz aşağıdaki gibidir");
                        sb.AppendLine("\n");
                        sb.AppendLine("Kullanıcı Adınız: " + mailuser.UserName);
                        sb.AppendLine("Şifreniz        : " + MyTools.Sifreleme.Decryp(mailuser.Password));
                        sb.AppendLine("\n");
                        sb.AppendLine("\n");

                        if (mailuser.LisansDurum.ProYetki /*|| (mailuser.LisansDurum.ProYetkiStart != null && mailuser.LisansDurum.ProYetkiStart >= DateTime.Now)*/)
                            sb.AppendLine("IDEAL Masaüstü Programımızı aşağıdaki linkten indirip bilgisayarınıza kurabilirsiniz.");

                        // if ((mailuser.LisansDurum.ProYetki || (mailuser.LisansDurum.ProYetkiStart != null && mailuser.LisansDurum.ProYetkiStart >= DateTime.Now)) && (mailuser.LisansDurum.BMK == true || (mailuser.LisansDurum.BMKStart != null && mailuser.LisansDurum.BMKStart >= DateTime.Now.Date)) && (mailuser.LisansDurum.FSystem == true || (mailuser.LisansDurum.FSystemStart != null && mailuser.LisansDurum.FSystemStart >= DateTime.Now)))
                        if (mailuser.LisansDurum.ProYetki)
                        {

                            sb.AppendLine("https://idealdata.com.tr/downloads/idealBMK.exe");
                            sb.AppendLine("\n");

                            //sb.AppendLine("Fsystem – Güncelleme");
                            //sb.AppendLine(" https://idealdata.com.tr/downloads/fsystemmini.exe");
                            //sb.AppendLine("Eğitim Videoları");
                            //sb.AppendLine("BMK Sistemleri nedir? Nasıl alınır? Nasıl kullanılır?");
                            //sb.AppendLine("https://www.youtube.com/watch?v=KGF8g0rUCfM");


                            //sb.AppendLine("FSystem Lisansları nasıl alınır, nasıl yüklenir, nasıl kullanılır?");
                            //sb.AppendLine("https://www.youtube.com/watch?v=KGF8g0rUCfM");
                        }
                        //else if (/*(*/mailuser.LisansDurum.ProYetki== true /*||*/ /*(mailuser.LisansDurum.ProYetkiStart != null && mailuser.LisansDurum.ProYetkiStart >= DateTime.Now))*/ && /*(*/mailuser.LisansDurum.BMK == true /*|| (mailuser.LisansDurum.BMKStart != null && mailuser.LisansDurum.BMKStart >= DateTime.Now))*/)
                        //{
                        //    sb.AppendLine("https://idealdata.com.tr/downloads/idealBMK.exe");
                        //    sb.AppendLine("\n");
                        //    sb.AppendLine("Eğitim Videoları");
                        //    sb.AppendLine("BMK Sistemleri nedir? Nasıl alınır? Nasıl kullanılır?");
                        //    sb.AppendLine("https://www.youtube.com/watch?v=KGF8g0rUCfM");
                        //}
                        //else if ((mailuser.LisansDurum.ProYetki || (mailuser.LisansDurum.ProYetkiStart != null && mailuser.LisansDurum.ProYetkiStart >= DateTime.Now)) && (mailuser.LisansDurum.FSystem == true || (mailuser.LisansDurum.FSystemStart != null && mailuser.LisansDurum.FSystemStart >= DateTime.Now)))
                        //{
                        //    sb.AppendLine("https://idealdata.com.tr/downloads/fsystem.exe");
                        //    sb.AppendLine("\n");
                        //    sb.AppendLine("Eğitim Videoları");
                        //    sb.AppendLine("FSystem Lisansları nasıl alınır, nasıl yüklenir, nasıl kullanılır?");
                        //    sb.AppendLine("https://www.youtube.com/watch?v=KGF8g0rUCfM");
                        //}
                        //else if (mailuser.LisansDurum.ProYetki || (mailuser.LisansDurum.ProYetkiStart != null && mailuser.LisansDurum.ProYetkiStart >= DateTime.Now))
                        //{
                        //    sb.AppendLine("https://idealdata.com.tr/downloads/ideal_setup.exe");
                        //}
                        if (mailuser.LisansDurum.CepYetki)
                        {
                            sb.AppendLine("\n");
                            sb.AppendLine("iDeal Cep uygulamamızı kurmak için akıllı telefonunuzun uygulama marketinden (Google Play Store/Apple Store)  IDEAL Mobile veya IDEAL CEP yazarak aratıp ANDROID veya IOS cihazlarınıza indirip aynı kullanıcı adı ve şifreyle giriş yapabilirsiniz.");
                            sb.AppendLine("\n\n");
                            sb.AppendLine("iDeal Mobile – Android");
                            sb.AppendLine("https://play.google.com/store/apps/details?id=com.ideal.fintables&gl=TR");
                            sb.AppendLine("iDeal Mobile – iPhone");
                            sb.AppendLine("https://apps.apple.com/us/app/ideal-mobil/id1498124629");
                            sb.AppendLine("\n\n");
                            sb.AppendLine("\n\n");
                            sb.AppendLine("iDeal Cep – Android");
                            sb.AppendLine("https://play.google.com/store/apps/details?id=com.DirectFN.Turkey.idealcep");
                            sb.AppendLine("iDeal Cep – iPhone");
                            sb.AppendLine("https://itunes.apple.com/tr/app/directfn-ideal-cep/id843135427?mt=8");
                            sb.AppendLine("iDeal Cep - Eğitim Videoları");
                            sb.AppendLine("https://www.youtube.com/channel/UCE3GVa1pkCavO9MLPaXxZjA/search?query=cep");

                            sb.AppendLine("Aynı Kullanıcı adı ve şifrenizle İDEALWEB uygulamamızı da kullanabilirsiniz.");
                            sb.AppendLine("İDEALWEB kullanmak için bir internet tarayıcısından aşağıdaki adrese girmeniz yeterlidir:");
                            sb.AppendLine("\n");
                            sb.AppendLine("https://web.idealdata.com.tr");

                            sb.AppendLine("\n");
                            sb.AppendLine("Ekim 2020 de yayınlanacak olan Yeni (Native) iDeal Mobil uygulamamızı da aynı kullanıcı adı ve şifrelerle kullanabilirsiniz.");
                            sb.AppendLine("Yeni iDeal Mobil uygulamasını telefonunuza kurmak için cihazınıza uygun kurulum dosyalarını aşağıdaki linklerden indirebilirsiniz;");
                            sb.AppendLine("ANDROID: http://rebrand.ly/ideal-mobil");
                            sb.AppendLine("IPHONE: https://testflight.apple.com/join/lU2Krelg");
                        }
                        //if (mailuser.LisansDurum.CME || (mailuser.LisansDurum.CMEStart != null && mailuser.LisansDurum.CMEStart >= DateTime.Now))
                        //{
                        //    sb.AppendLine("\n");

                        //    sb.AppendLine("iDealGo - Eğitim Videoları");
                        //    sb.AppendLine("https://www.youtube.com/watch?v=eTqPPUF6_cA&list=PLGPR8mtpJxwxv-sMJxBRO8kRRTFHfPH_s");
                        //}
                        sb.AppendLine("\n");
                        sb.AppendLine("Her türlü soru ve sorununuz için bize pazarlama@idealdata.com.tr veya teknik@idealdata.com.tr mail adreslerimizden veya 0212 385 35 35 numaralı telefonumuzdan ulaşabilirsiniz.");
                        sb.AppendLine("\n");
                        sb.AppendLine("\n");
                        sb.AppendLine("Saygılarımızla");
                        sb.AppendLine("iDeal Data Finansal Teknolojiler A.Ş.");
                        sb.AppendLine("0212 385 35 35");
                        sb.AppendLine("https://idealdata.com.tr/");

                        //var sonucx = MyTools.MailGonder2(mailuser.Iletisim.email, sb.ToString());

                        Task.Factory.StartNew(() =>
                        {
                            return MyTools.MailGonder2(mailuser.Iletisim.email, sb.ToString());

                        }).ContinueWith<string>((i) =>
                        {
                            var sonucx = i.Result;

                            if (sonucx == "ok")
                            {
                                formAdminAra.referance.IPport.DataToSend = "SendUserMusMail|" + mailuser.UserID.ToString() + (char)3;
                                MessageBox.Show("Müşteriye mail Gönderildi");
                            }
                            else
                            {
                                MessageBox.Show("Müşteriye mail Gönderilirken hata oluştu \nHata:" + sonucx);
                            }
                            return sonucx;
                        });
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public static DateTime LastDayOfMonth()
        {
            DateTime dateX = DateTime.Now;
            try
            {
                return new DateTime(dateX.Year, dateX.Month, 1).AddMonths(1).AddDays(-1);
            }
            catch { return dateX; }
        }
        private void btnYapildi_Click(object sender, EventArgs e)
        {
            try
            {
                var cal = formAdminAra.referance.ActiveCalisan;
                var crm = new crmDFNDataContext();
                var talep = crm.KurumTalepleris.FirstOrDefault(x => x.id == acriveTalepId);
                //if (talep.TalepDurumId != 5)
                //{
                //    MessageBox.Show("Onaylanmamış Talep Yapılamaz");
                //    return;
                //}
                talep.Aciklama = txtTalepAciklama.Text;
                talep.TalepDurumId = 3;
                talep.TalebiYapan = cal.calisanID;
                talep.TalepYapilmaTarihi = DateTime.Now;
                crm.SubmitChanges();

                MessageBox.Show("Talep Yapıldı Olarak İşaretlendi");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void lViewLisans_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Salmon, e.Bounds);
            e.DrawText();
        }
        private void lViewLisans_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }
    }
}
