using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace DirectFNCRM.AdminViews
{
    public partial class formUserDetay : Form
    {
        public formUserDetay()
        {
            InitializeComponent();
            gridLisanHesaplaOzet.Rows.Add(25);
        }

        public bool comboildoldurma = false;
        public bool lisansdegistirme = false;
        public int acticeitem = 0;
        public bool KaydetUpdate = false;
        public string yayinKontrol = "";

        void formUserDetay_Load(object sender, EventArgs e)
        {
            KurumLisansGorunum();

            crmDFNDataContext crm = new crmDFNDataContext();
            //comboKurumSube.Items.Clear();
            var subes = crm.KurumSubes.Where(x => x.MusteriNo == "10011").OrderBy(x => x.SubeAdi).Select(x => x.SubeAdi).ToArray();
            //comboKurumSube.Items.AddRange(subes.ToArray());


            acticeitem = (int)this.Tag;

            comboMensei.DataSource = crm.MusteriMenseis;
            comboMensei.DisplayMember = "MenseiAdi";
            comboMensei.ValueMember = "id";

            comboUlke.DataSource = crm.Ulkes;
            comboUlke.DisplayMember = "UlkeAdi";
            comboUlke.ValueMember = "Id";

            comboStatus.DataSource = crm.Status;
            comboStatus.DisplayMember = "StatusAdi";
            comboStatus.ValueMember = "StatusId";

            comboUlke.SelectedItem = comboUlke.Items[212];

            comboProNonPro.SelectedIndex = 0;
            comboildoldurma = true;
            //txtPMTSno.Text ="10011";
            //txtPMTSno.Text = crm.Musterilers.);
            txtPMTSno.Text = MyTools.KurumKod;
            // txtPMTSno.ReadOnly = true;
            dateTimeExpiry.Text = dateTimeExpiry.Value._LastDayOfMonth().ToString(); //değişti
            dtpCepEnd.Text = dtpCepEnd.Value._LastDayOfMonth().ToString();// değişti

            ///

            if (acticeitem == 0)
                txtUserName.Select();


            #region Faturabilgileri

            #endregion

            GridToForm(acticeitem);

            if (formAdminAra.referance.ActiveCalisan.YetiID > 3)
            {
                lisanDegistirToolStripMenuItem.Visible = false;

            }
            if (formAdminAra.referance.ActiveCalisan.YetiID == 6)
                lisanDegistirToolStripMenuItem.Visible = true;


            var notsayisi = crm.UserNots.Where(x => x.Userid == acticeitem).ToList().Count;

            btnNots.Text = "Notlar ( " + notsayisi + " )";
            yayinKontrol = comboYayinDurumu.SelectedItem.ToString();

        }

        void KurumLisansGorunum()
        {
            if (MyTools.KurumKod == "10011")
            {
                chkPro.Visible = true;
                chkCep.Visible = true;
                chkCep.CheckState = CheckState.Checked;
                chkROBOT.Visible = true;
                chkPayGS.Visible = true;
                chkPayX.Visible = true;
                chkPayLP.Visible = true;
                chkPayL2.Visible = true;
                chkPd2P.Visible = true;
                chkPITE.Visible = true;
                //chkPayX.Location = chkPayL2.Location;
                chkVL2.Visible = true;
                chkVd2P.Visible = true;
                chkVLP.Visible = true;
                chkVGS.Visible = true;
                // chkTL1.Visible = true;
                chkTLP.Visible = true;
                chkTL2.Visible = true;
                //chkAnPro.Visible = true;
                //panel16.Visible = true;
                chkMKK.Visible = true;
                chkCME.Visible = true;
                chkComex.Visible = true;
                chkGKKUL.Visible = true;

            }


            else
            {
                chkPro.Visible = false;
                chkCep.Visible = false;
                chkROBOT.Visible = false;
                chkAnPro.Visible = false;
                // panel16.Visible = false;
            }
        }

        void GridToForm(int userID)
        {
            try
            {
                if (userID == 0)
                {
                    LisansEtkin(true);
                    lisansdegistirme = true;
                    dtpProStart.Visible = false;
                    dtpCepStart.Visible = true;
                    //  dtpROBOTStart.Visible = false;
                    dtpPayL1Start.Visible = false;
                    dtpPayLPStart.Visible = false;
                    dtpPayL2Start.Visible = false;
                    dtpPayL2Start.Visible = false;
                    dtpPd2PStart.Visible = false;
                    //  dtpPd2PStart.Visible = false;
                    dtpPayXStart.Visible = false;
                    dtpPayGSStart.Visible = false;
                    dtpPITEStart.Visible = false;
                    dtpViopL1Start.Visible = false;
                    dtpViopLPStart.Visible = false;
                    dtpViopL2Start.Visible = false;
                    dtpVd2PStart.Visible = false;
                    dtpViopGSStart.Visible = false;
                    dtpTahvilL1Start.Visible = false;
                    dtpTahvilLPStart.Visible = false;
                    dtpTahvilL2Start.Visible = false;
                    dtpAnProStart.Visible = false;
                    //dtpCMEStart.Visible = false;
                    //dtpCMEMStart.Visible = false;
                    //dtpTemelAnalizStart.Visible = false;
                    //dtpBMKStart.Visible = false;
                    //dtpBMCStart.Visible = false;
                    //dtpBarSistemStart.Visible = false;
                    //dtpFSystemStart.Visible = false;
                    dtpKRMD1Start.Visible = false;
                    //dtpHISSEAStart.Visible = false;
                    //dtpParaStart.Visible = false;

                    dtpSentiL1Start.Visible = false;
                    dtpSentiL2Start.Visible = false;
                    dtpMkkStart.Visible = false;
                    dtpCMEStart.Visible = false;
                    dtpGkkulStart.Visible = false;

                    dtpProEnd.Visible = false;
                    dtpCepEnd.Visible = true;
                    //     dtpROBOTEnd.Visible = false;
                    dtpPayL1End.Visible = false;
                    dtpPayLPEnd.Visible = false;
                    dtpPayL2End.Visible = false;
                    dtpPd2PEnd.Visible = false;
                    //  dtpPd2PEnd.Visible = false;
                    dtpPayXEnd.Visible = false;
                    dtpPayGSEnd.Visible = false;
                    dtpPITEEnd.Visible = false;
                    dtpViopL1End.Visible = false;
                    dtpViopLPEnd.Visible = false;
                    dtpViopL2End.Visible = false;
                    dtpVd2PEnd.Visible = false;
                    dtpViopGSEnd.Visible = false;
                    dtpTahvilL1End.Visible = false;
                    dtpTahvilLPEnd.Visible = false;
                    dtpTahvilL2End.Visible = false;
                    dtpAnProEnd.Visible = false;
                    //dtpCMEEnd.Visible = false;
                    //dtpCMEMEnd.Visible = false;
                    //dtpTemelAnalizEnd.Visible = false;
                    //dtpBMKEnd.Visible = false;
                    //dtpBMCEnd.Visible = false;
                    //dtpBarSistemEnd.Visible = false;
                    //dtpFSystemEnd.Visible = false;
                    dtpKRMD1End.Visible = false;
                    //dtpHISSEAEnd.Visible = false;
                    //dtpParaEnd.Visible = false;
                    dtpMkkEnd.Visible = false;
                    dtpCMEEnd.Visible = false;
                    dtpGkkulEnd.Visible = false;

                    dtpSentiL1End.Visible = false;
                    dtpSentiL2End.Visible = false;

                    comboYayinDurumu.SelectedIndex = 0;
                    return;
                }
                crmDFNDataContext crm = new crmDFNDataContext();
                var user = crm.Users.FirstOrDefault(x => x.UserID == userID);

                lisansdegistirme = false;
                LisansEtkin(false);
                txtUserName.Text = (user.UserName == null) ? "" : user.UserName;
                txtPassword.Text = MyTools.Sifreleme.Decryp(user.Password);
                txtAdi.Text = (user.Name == null) ? "" : user.Name;
                txtAciklama.Text = (user.Aciklama == null) ? "" : user.Aciklama;
                txtSurname.Text = (user.Surname == null) ? "" : user.Surname;
                chkEmirTeyid.Checked = user.EmirTeyid;
                txtFxKurum.Text = (user.FXkurum == null) ? "" : user.FXkurum;
                //if (user.ProNonPro != null)
                //{
                //    if (user.ProNonPro == true)                    
                //        comboProNonPro.SelectedIndex = 1;                    
                //    else                    
                //        comboProNonPro.SelectedIndex = 0;
                //}
                //else
                //{
                //    comboProNonPro.SelectedIndex = -1;
                //}
                //if (user.SozlesmeID != null)
                //{
                //    var sorgu = crm.Sozlesmelers.Where(x => x.MusteriNo == user.PmtsNo);
                //    comboSozlesme.DataSource = sorgu;
                //    comboSozlesme.DisplayMember = "SozlesmeNo";
                //    comboSozlesme.ValueMember = "SozlesmeilID";
                //    comboSozlesme.SelectedValue = user.SozlesmeID;
                //}
                //else
                //{
                //    var sorgu = crm.Sozlesmelers.Where(x => x.MusteriNo == user.PmtsNo);
                //    comboSozlesme.DataSource = sorgu;
                //    comboSozlesme.DisplayMember = "SozlesmeNo";
                //    comboSozlesme.ValueMember = "SozlesmeilID";                
                //    comboSozlesme.SelectedIndex = -1;
                //}

                txtPMTSno.Text = (user.PmtsNo == null) ? "" : user.PmtsNo;
                txtTckn.Text = (user.tckno == null) ? "" : user.tckno;
                // txtTckn.Text = (user.UserName == null) ? "" : user.UserName;
                dateTimeStartDate.Value = user.BaslangicTarihi.Value;
                dateTimeExpiry.Value = user.ExpiryDate.Value;

                //status
                if (user.StatusId != null)
                    comboStatus.SelectedValue = user.StatusId;
                else
                {
                    comboStatus.SelectedIndex = -1;
                }
                //mensei
                if (user.MusteriMenseiID != null)
                {
                    comboMensei.SelectedValue = user.MusteriMenseiID;
                }
                else
                    comboMensei.SelectedIndex = -1;
                ////unvan yaz
                var musteri = MusteriBul(user.PmtsNo);
                var subes = crm.KurumSubes.Where(x => x.MusteriNo == txtPMTSno.Text).OrderBy(x => x.SubeAdi).Select(x => x.SubeAdi).ToList();
                //comboKurumSube.Items.Clear();
                //comboKurumSube.Items.AddRange(subes.ToArray());
                //if (musteri != null)
                //{
                //    lblunvan.Text = musteri.MusteriAdi;
                //}
                //// kurumsal bilgiler
                if (user.KurumsalBilgilerId != null)
                {
                    if (user.KurumsalBilgiler.kurumhesapno != null)
                        txtKurumHesapNo.Text = user.KurumsalBilgiler.kurumhesapno;

                    if (user.KurumsalBilgiler.KurumKullaniciTip != null)
                        txtMusteriTip.Text = user.KurumsalBilgiler.KurumKullaniciTip;

                    if (user.KurumsalBilgiler.Not1 != null)
                        txtNot1.Text = user.KurumsalBilgiler.Not1;

                    if (user.KurumsalBilgiler.KurumSube != null)
                        txtKurumSube.Text = user.KurumsalBilgiler.KurumSube;

                }

                if (user.iletisimId != null)
                {
                    txtTel1.Text = (user.Iletisim.Tel1 == null) ? "" : user.Iletisim.Tel1;
                    txtTel2.Text = (user.Iletisim.Tel2 == null) ? "" : user.Iletisim.Tel2;
                    txtCeptel.Text = (user.Iletisim.Ceptel == null) ? "" : user.Iletisim.Ceptel;
                    txtMail.Text = (user.Iletisim.email == null) ? "" : user.Iletisim.email;
                    txtAdres.Text = (user.Iletisim.acikadres == null) ? "" : user.Iletisim.acikadres;
                    if (user.Iletisim.UlkeId != null)
                        comboUlke.SelectedValue = user.Iletisim.UlkeId;
                    else
                        comboUlke.SelectedIndex = -1;
                    if (user.Iletisim.IlId != null)
                    { comboIL.SelectedValue = user.Iletisim.IlId; }
                    else comboIL.SelectedIndex = -1;
                    if (user.Iletisim.Ilce != null)
                    { comboILCE.SelectedValue = user.Iletisim.IlceId; }
                    else comboILCE.SelectedIndex = -1;
                }
                dateTimeStartDate.Value = user.BaslangicTarihi.Value;

                #region Lisanlaroku

                var lisansdurum = user.LisansDurum;
                if (lisansdurum != null)
                {
                    if (lisansdurum.YayinDurumu)
                    {
                        comboYayinDurumu.SelectedIndex = 0;
                        comboYayinDurumu.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        comboYayinDurumu.SelectedIndex = 1;
                        comboYayinDurumu.BackColor = Color.Red;
                    }


                    chkCep.Checked = (bool)lisansdurum.CepYetki;
                    chkPro.Checked = (bool)lisansdurum.ProYetki;
                    chkPayL1.Checked = (bool)lisansdurum.PayL1;
                    chkPayLP.Checked = (bool)lisansdurum.PayLP;
                    chkPayL2.Checked = (bool)lisansdurum.PayL2;
                    chkPd2P.Checked = (bool)lisansdurum.Pd2P;
                    chkPayGS.Checked = (bool)lisansdurum.PayGS;
                    chkPayX.Checked = (bool)lisansdurum.PayX;
                    chkPITE.Checked = (bool)lisansdurum.PITE;
                    chkVeriAnalitik.Checked = (bool)lisansdurum.VeriAnalitik;
                    chkVL1.Checked = (bool)lisansdurum.ViopL1;
                    chkVLP.Checked = (bool)lisansdurum.ViopLP;
                    chkVL2.Checked = (bool)lisansdurum.ViopL2;
                    chkVd2P.Checked = (bool)lisansdurum.Vd2P;
                    chkVGS.Checked = (bool)lisansdurum.ViopGS;
                    chkPITE.Checked = (bool)lisansdurum.PITE;
                    chkTL1.Checked = (bool)lisansdurum.TahvilL1;
                    chkTL2.Checked = (bool)lisansdurum.TahvilL2;
                    chkTLP.Checked = (bool)lisansdurum.TahvilLP;
                    chkAnPro.Checked = (bool)lisansdurum.AnPro;
                    chkSentiL1.Checked = (bool)lisansdurum.SentiL1;
                    chkSentiL2.Checked = (bool)lisansdurum.SentiL2;
                    chkMKK.Checked = (bool)lisansdurum.MKK;
                    chkGKKUL.Checked = (bool)lisansdurum.GKKUL;
                    chkCME.Checked = (bool)lisansdurum.CME;
                    chksaseL1.Checked = (bool)lisansdurum.SaseL1;
                    chkSaseL2.Checked = (bool)lisansdurum.SaseL2;
                    chkFutGck.Checked = (bool)lisansdurum.Futgck;
                    chkWINX.Checked = (bool)lisansdurum.WINX;
                    chkDJI.Checked = (bool)lisansdurum.DJI;
                    chkXetra.Checked = (bool)lisansdurum.XETRA;
                    chkSpI.Checked = (bool)lisansdurum.SPI;
                    chkCBOT.Checked = (bool)lisansdurum.CBOT;
                    chkCBOTM.Checked = (bool)lisansdurum.CBOTM;
                    chkCME2.Checked = (bool)lisansdurum.CME;
                    chkCMEM.Checked = (bool)lisansdurum.CMEM;
                    chkEUREX.Checked = (bool)lisansdurum.EUREX;
                    chkComex.Checked = (bool)lisansdurum.COMEX;
                    chkSentiL1.Checked = (bool)lisansdurum.SentiL1;
                    chkSentiL2.Checked = (bool)lisansdurum.SentiL2;
                    //chkMKK.Checked = (bool)lisansdurum.MKK;
                    //chkTARAMA.Checked = (bool)lisansdurum.TARAMA;
                    chkNymex.Checked = (bool)lisansdurum.NYMEX;
                    chkNymexM.Checked = (bool)lisansdurum.NYMEXM;
                    chkNYSE.Checked = (bool)lisansdurum.NYSE;
                    chkNASDAQ.Checked = (bool)lisansdurum.NASDAQ;
                    chkAMEX.Checked = (bool)lisansdurum.Amex;
                    chkCHIX.Checked = (bool)lisansdurum.CHIX;
                    chkLSE.Checked = (bool)lisansdurum.LSE;
                    chkROBOT.Checked = (bool)lisansdurum.ROBOT;
                    chkSCMDownload.Checked = (bool)lisansdurum.SCMDownload;
                    chkSCMRealTime.Checked = (bool)lisansdurum.SCMRealTıme;
                    chkSCMUsable.Checked = (bool)lisansdurum.SCMUsable;

                }


                #endregion

                #region SonKullanımTarihleri
                //TarihYazAyarla(lisansdurum.ROBOT, dtpROBOTStart, lisansdurum.RobotStart, dtpROBOTEnd, lisansdurum.RobotEnd, chkRobotTarih);
                TarihYazAyarla(lisansdurum.ProYetki, dtpProStart, lisansdurum.ProYetkiStart, dtpProEnd, lisansdurum.ProYetkiEnd, chkProTarih);
                TarihYazAyarla(lisansdurum.CepYetki, dtpCepStart, lisansdurum.CepYetkiStart, dtpCepEnd, lisansdurum.CepYetkiEnd, chkCepTarih);
                TarihYazAyarla(lisansdurum.PayL1, dtpPayL1Start, lisansdurum.PayL1Start, dtpPayL1End, lisansdurum.PayL1End, chkPayL1Tarih);
                TarihYazAyarla(lisansdurum.PayLP, dtpPayLPStart, lisansdurum.PayLPStart, dtpPayLPEnd, lisansdurum.PayLPEnd, chkPayLPTarih);
                TarihYazAyarla(lisansdurum.PayL2, dtpPayL2Start, lisansdurum.PayL2Start, dtpPayL2End, lisansdurum.PayL2End, chkPayL2Tarih);
                TarihYazAyarla(lisansdurum.Pd2P, dtpPd2PStart, lisansdurum.Pd2PStart, dtpPd2PEnd, lisansdurum.Pd2PEnd, chkPd2PTarih);
                // TarihYazAyarla(lisansdurum.Pd2P, dtpPd2PStart, lisansdurum.Pd2PStart, dtpPd2PEnd, lisansdurum.Pd2Pend, chkPd2PTarih);
                TarihYazAyarla(lisansdurum.PayX, dtpPayXStart, lisansdurum.PayXStart, dtpPayXEnd, lisansdurum.PayXEnd, chkPayXTarih);
                TarihYazAyarla(lisansdurum.PayGS, dtpPayGSStart, lisansdurum.PayGSStart, dtpPayGSEnd, lisansdurum.PayGSEnd, chkPayGSTarih);
                TarihYazAyarla(lisansdurum.PITE, dtpPITEStart, lisansdurum.PayPiteStart, dtpPITEEnd, lisansdurum.PayPiteEnd, chkPITETarih);

                TarihYazAyarla(lisansdurum.ViopL1, dtpViopL1Start, lisansdurum.ViopL1Start, dtpViopL1End, lisansdurum.ViopL1End, chkVL1Tarih);
                TarihYazAyarla(lisansdurum.ViopLP, dtpViopLPStart, lisansdurum.ViopLPStart, dtpViopLPEnd, lisansdurum.ViopLPEnd, chkVLPTarih);
                TarihYazAyarla(lisansdurum.ViopL2, dtpViopL2Start, lisansdurum.ViopL2Start, dtpViopL2End, lisansdurum.ViopL2End, chkVL2Tarih);
                TarihYazAyarla(lisansdurum.Vd2P, dtpVd2PStart, lisansdurum.Vd2PStart, dtpVd2PEnd, lisansdurum.Vd2PEnd, chkVd2PTarih);
                TarihYazAyarla(lisansdurum.ViopGS, dtpViopGSStart, lisansdurum.ViopGSStart, dtpViopGSEnd, lisansdurum.ViopGSEnd, chkVGSTarih);

                TarihYazAyarla(lisansdurum.COMEX, dtpKRMD1Start, lisansdurum.KRMD1Start, dtpKRMD1End, lisansdurum.KRMD1End, chkKRMD1Tarih);

                TarihYazAyarla(lisansdurum.TahvilL1, dtpTahvilL1Start, lisansdurum.TahvilL1Start, dtpTahvilL1End, lisansdurum.TahvilL1End, chkTL1Tarih);
                TarihYazAyarla(lisansdurum.TahvilLP, dtpTahvilLPStart, lisansdurum.TahvilLPStart, dtpTahvilLPEnd, lisansdurum.TahvilLPEnd, chkTLPTarih);
                TarihYazAyarla(lisansdurum.TahvilL2, dtpTahvilL2Start, lisansdurum.TahvilL2Start, dtpTahvilL2End, lisansdurum.TahvilL2End, chkTL2Tarih);

                //TarihYazAyarla(lisansdurum.AnPro, dtpAnProStart, lisansdurum.AnProStart, dtpAnProEnd, lisansdurum.AnProEnd, chkAnProTarih);
                TarihYazAyarla(lisansdurum.MKK, dtpMkkStart, lisansdurum.MKKStart, dtpMkkEnd, lisansdurum.MKKEnd, chkMkkTarih);
                TarihYazAyarla(lisansdurum.GKKUL, dtpGkkulStart, lisansdurum.GKKULStart, dtpGkkulEnd, lisansdurum.GKKULEnd, chkGkkulTarih);
                TarihYazAyarla(lisansdurum.CME, dtpCMEStart, lisansdurum.CMEStart, dtpCMEEnd, lisansdurum.CMEEnd, chkCMETarih);


                //TarihYazAyarla(lisansdurum.TemelAnaliz, dtpTemelAnalizStart, lisansdurum.TemelAnalizStart, dtpTemelAnalizEnd, lisansdurum.TemelAnalizEnd, chkTemelAnalizTarih);
                //TarihYazAyarla(lisansdurum.BMK, dtpBMKStart, lisansdurum.BMKStart, dtpBMKEnd, lisansdurum.BMKEnd, chkBMKTarih);
                //TarihYazAyarla(lisansdurum.BMC, dtpBMCStart, lisansdurum.BMCStart, dtpBMCEnd, lisansdurum.BMCEnd, chkBMCTarih);
                //TarihYazAyarla(lisansdurum.BarSistem, dtpBarSistemStart, lisansdurum.BarSistemStart, dtpBarSistemEnd, lisansdurum.BarSistemEnd, chkBarSistemTarih);
                //TarihYazAyarla(lisansdurum.FSystem, dtpFSystemStart, lisansdurum.FSystemStart, dtpFSystemEnd, lisansdurum.FSystemEnd, chkFSystemTarih);
                //TarihYazAyarla(lisansdurum.PARA, dtpParaStart, lisansdurum.PARAStart, dtpParaEnd, lisansdurum.PARAEnd, chkParaTarih);
                //TarihYazAyarla(lisansdurum.HISSEA, dtpHISSEAStart, lisansdurum.HISSEAStart, dtpHISSEAEnd, lisansdurum.HISSEAEnd, chkHISSEATarih);
                //TarihYazAyarla(lisansdurum.SentiL1, dtpSentiL1Start, lisansdurum.SentiL1Start, dtpSentiL1End, lisansdurum.SentiL1End, chkSentiL1Tarih);
                //TarihYazAyarla(lisansdurum.SentiL2, dtpSentiL2Start, lisansdurum.SentiL2Start, dtpSentiL2End, lisansdurum.SentiL2End, chkSentiL2Tarih);
                //TarihYazAyarla(lisansdurum.CME, dtpCMEStart, lisansdurum.CMEStart, dtpCMEEnd, lisansdurum.CMEEnd, chkCMETarih);
                //TarihYazAyarla(lisansdurum.CMEM, dtpCMEMStart, lisansdurum.CMEMStart, dtpCMEMEnd, lisansdurum.CMEMEnd, chkCMEMTarih);
                #endregion

                if (lisansdurum.YayinDurumu == true)
                {
                    #region TutarHesapla

                    decimal toplam = 0;


                    //decimal profiyat = 0;
                    //decimal cepfiyat = 0;



                    //if (user.Sozlesmeler != null)
                    //{
                    //    if (user.Sozlesmeler.ParaBirimi.id == 1)
                    //    {
                    //        if (lisansdurum.ProYetki == true)
                    //            profiyat = (decimal)user.Sozlesmeler.ProFiyat;
                    //        if (lisansdurum.CepYetki == true)
                    //            cepfiyat = (decimal)user.Sozlesmeler.CepFiyat;
                    //    }

                    //    else
                    //    {
                    //         if(lisansdurum.ProYetki==true)
                    //    profiyat =(decimal)(user.Sozlesmeler.ProFiyat*user.Sozlesmeler.ParaBirimi.Kur);
                    //    if (lisansdurum.CepYetki == true)
                    //        cepfiyat = (decimal)(user.Sozlesmeler.CepFiyat * user.Sozlesmeler.ParaBirimi.Kur);
                    //    }



                    //    toplam += profiyat + cepfiyat;
                    //}

                    #region Pay

                    var paytanimi = "Pay Lisansı Yok";
                    decimal paytutar = 0;
                    decimal payendeks = 0;
                    decimal verianalitik = 0;
                    decimal pit = 0;
                    decimal pite = 0;


                    if (lisansdurum.PayL1 == true && lisansdurum.PayLP == false && lisansdurum.PayL2 == false)
                    {
                        paytanimi = "Hisse Yüzeysel";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            paytutar = MyTools.lisansfiyatlari.Fpd1;
                        else
                            paytutar = MyTools.lisansfiyatlari.Ypd1;
                    }
                    else if (lisansdurum.PayL1 == true && lisansdurum.PayLP == true && lisansdurum.PayL2 == false)
                    {
                        paytanimi = "Hisse Yüzeysel+";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            paytutar = MyTools.lisansfiyatlari.Fpd1p;
                        else
                            paytutar = MyTools.lisansfiyatlari.Ypd1p;


                    }
                    else if (lisansdurum.PayL1 == true && lisansdurum.PayLP == true && lisansdurum.PayL2 == true && lisansdurum.Pd2P == false)
                    {
                        paytanimi = "Hisse Derinlik";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            paytutar = MyTools.lisansfiyatlari.Fpd2;
                        else
                            paytutar = MyTools.lisansfiyatlari.Ypd2;


                    }
                    else if (lisansdurum.PayL1 == true && lisansdurum.PayLP == true && lisansdurum.PayL2 == true && lisansdurum.Pd2P == true)
                    {
                        paytanimi = "Hisse Derinlik+";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            paytutar = MyTools.lisansfiyatlari.Fpd2p;
                        else
                            paytutar = MyTools.lisansfiyatlari.Ypd2p;


                    }

                    toplam += paytutar;


                    if (lisansdurum.PayX == true)
                    {
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            payendeks = MyTools.lisansfiyatlari.Fend;
                        else
                            payendeks = MyTools.lisansfiyatlari.Yend;

                        toplam += payendeks;
                    }



                    if (lisansdurum.PayGS == true)
                    {
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            pit = MyTools.lisansfiyatlari.Fpit;
                        else
                            pit = MyTools.lisansfiyatlari.Ypit;
                        toplam += pit;
                    }

                    if (lisansdurum.PITE == true)
                    {
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            pite = MyTools.lisansfiyatlari.Fpite;
                        else
                            pite = MyTools.lisansfiyatlari.Ypite;
                        toplam += pite;
                    }

                    if (lisansdurum.VeriAnalitik == true)
                    {
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            verianalitik = MyTools.lisansfiyatlari.Fpva;
                        else
                            verianalitik = MyTools.lisansfiyatlari.Ypva;

                        toplam += verianalitik;
                    }


                    #endregion
                    #region Viop

                    var Vioptanimi = "Viop Lisansı Yok";
                    decimal vioptutar = 0;
                    decimal vit = 0;


                    if (lisansdurum.ViopL1 == true && lisansdurum.ViopLP == false && lisansdurum.ViopL2 == false)
                    {
                        Vioptanimi = "Viop Yüzeysel";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            vioptutar = MyTools.lisansfiyatlari.Fvl1;
                        else
                            vioptutar = MyTools.lisansfiyatlari.Yvl1;
                    }
                    else if (lisansdurum.ViopL1 == true && lisansdurum.ViopLP == true && lisansdurum.ViopL2 == false)
                    {
                        Vioptanimi = "Viop Yüzeysel+";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            vioptutar = MyTools.lisansfiyatlari.Fvl1p;
                        else
                            vioptutar = MyTools.lisansfiyatlari.Yvl1p;

                    }
                    else if (lisansdurum.ViopL1 == true && lisansdurum.ViopLP == true && lisansdurum.ViopL2 == true && lisansdurum.Vd2P == false)
                    {
                        Vioptanimi = "Viop Derinlik";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            vioptutar = MyTools.lisansfiyatlari.Fvl2;
                        else
                            vioptutar = MyTools.lisansfiyatlari.Yvl2;

                    }
                    else if (lisansdurum.ViopL1 == true && lisansdurum.ViopLP == true && lisansdurum.ViopL2 == true && lisansdurum.Vd2P == true)
                    {
                        Vioptanimi = "Viop Derinlik+";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            vioptutar = MyTools.lisansfiyatlari.Fvl2p;
                        else
                            vioptutar = MyTools.lisansfiyatlari.Yvl2p;

                    }

                    toplam += vioptutar;

                    if (lisansdurum.ViopGS == true)
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            vit = MyTools.lisansfiyatlari.Fvit;
                        else
                            vit = MyTools.lisansfiyatlari.Yvit;

                    toplam += vit;
                    #endregion
                    #region Karmad1


                    decimal karmatutar = 0;

                    if (lisansdurum.COMEX)
                    {

                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            karmatutar = MyTools.lisansfiyatlari.Fkrmd1;
                        else
                            karmatutar = MyTools.lisansfiyatlari.Ykrmd1;
                    }
                    toplam += karmatutar;

                    #endregion
                    #region Mkk Tarama
                    decimal mkktutar = 0;
                    if (lisansdurum.MKK)
                    {

                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            mkktutar = MyTools.lisansfiyatlari.Fmkk;
                        else
                            mkktutar = MyTools.lisansfiyatlari.Ymkk;
                    }
                    toplam += mkktutar;

                    decimal taramatutar = 0;

                    if (lisansdurum.TARAMA)
                    {

                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            taramatutar = MyTools.lisansfiyatlari.Ftarama;
                        else
                            taramatutar = MyTools.lisansfiyatlari.Ytarama;
                    }

                    toplam += taramatutar;

                    #endregion
                    #region Gkkul
                    decimal gkkultutar = 0;
                    if (lisansdurum.GKKUL)
                    {

                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            gkkultutar = MyTools.lisansfiyatlari.Fgkkul;
                        else
                            gkkultutar = MyTools.lisansfiyatlari.Ygkkul;
                    }
                    toplam += gkkultutar;

                    #endregion
                    #region Tahvil
                    var Tahviltanimi = "Tahvil Lisansı Yok";
                    decimal Tahviltutar = 0;
                    if (lisansdurum.TahvilL1 == true && lisansdurum.TahvilLP == false && lisansdurum.TahvilL2 == false)
                    {
                        Tahviltanimi = "Tahvil Yüzeysel";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            Tahviltutar = MyTools.lisansfiyatlari.Fbd1;
                        else
                            Tahviltutar = MyTools.lisansfiyatlari.Ybd1;
                    }
                    else if (lisansdurum.TahvilL1 == true && lisansdurum.TahvilLP == true && lisansdurum.TahvilL2 == false)
                    {
                        Tahviltanimi = "Tahvil Yüzeysel+";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            Tahviltutar = MyTools.lisansfiyatlari.Fbd1p;
                        else
                            Tahviltutar = MyTools.lisansfiyatlari.Ybd1p;
                    }
                    else if (lisansdurum.TahvilL1 == true && lisansdurum.TahvilLP == true && lisansdurum.TahvilL2 == true)
                    {
                        Tahviltanimi = "Tahvil Derinlik";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            Tahviltutar = MyTools.lisansfiyatlari.Fbd2;
                        else
                            Tahviltutar = MyTools.lisansfiyatlari.Ybd2;
                    }
                    toplam += Tahviltutar;
                    #endregion
                    #region AnalizPro
                    var AnProTanimi = "Analiz Pro Lisansı Yok";
                    decimal AnProTutar = 0;
                    if (lisansdurum.AnPro == true)
                    {
                        AnProTanimi = "Analiz Pro";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            AnProTutar = MyTools.lisansfiyatlari.FanPro;
                        else
                            AnProTutar = MyTools.lisansfiyatlari.YanPro;
                    }
                    toplam += AnProTutar;
                    #endregion
                    #region Senti
                    var SentiTanimi = "Senti Lisansı Yok";
                    decimal Sentitutar = 0;
                    if (lisansdurum.SentiL1 == true && lisansdurum.SentiL2 == false)
                    {
                        SentiTanimi = "SentliL1";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            Sentitutar = MyTools.lisansfiyatlari.FSentiL1;
                        else
                            Sentitutar = MyTools.lisansfiyatlari.YSentiL1;
                    }
                    else if (lisansdurum.SentiL1 == true && lisansdurum.SentiL2 == true)
                    {
                        SentiTanimi = "SentiL2";
                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            Sentitutar = MyTools.lisansfiyatlari.FSentiL2;
                        else
                            Sentitutar = MyTools.lisansfiyatlari.YSentiL2;
                    }
                    toplam += Sentitutar;
                    #endregion
                    decimal algtutar = 0;
                    if (lisansdurum.CME)
                    {

                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            algtutar = MyTools.lisansfiyatlari.Fcme;
                        else
                            algtutar = MyTools.lisansfiyatlari.Ycme;
                    }
                    toplam += algtutar;

                    decimal robottutar = 0;
                    if (lisansdurum.ROBOT)
                    {

                        if (user.MusteriMenseiID == null || user.MusteriMenseiID == 1)
                            algtutar = MyTools.lisansfiyatlari.Frobot;
                        else
                            robottutar = MyTools.lisansfiyatlari.Yrobot;
                    }
                    toplam += robottutar;

                    #region YurtDisiFiyatlariHesapla
                    //yurtdışı fiyatları hesapla

                    decimal yurtdisitutar = 0;

                    decimal spotpaket = 0;
                    decimal DJI = 0;
                    decimal SPX = 0;
                    decimal XETRA = 0;

                    decimal CBOT = 0;
                    decimal CME = 0;

                    decimal CMEM = 0;
                    decimal CBOTM = 0;
                    decimal EUREX = 0;
                    // var yurtdisitanim = "Yurt Dışı Toplam";
                    if (user.ProNonPro != null)
                    {
                        #region spotpaketanaliz

                        if (lisansdurum.DJI == true && lisansdurum.SPI == true && lisansdurum.XETRA == true)
                        {
                            if (user.ProNonPro == true)
                                spotpaket = MyTools.lisansfiyatlari.SpotPro;
                            else
                                spotpaket = MyTools.lisansfiyatlari.SpotNonPro;
                        }
                        else
                        {
                            if (lisansdurum.DJI == true)
                            {
                                if (user.ProNonPro == true)
                                    DJI = MyTools.lisansfiyatlari.DJIPro;
                                else
                                    DJI = MyTools.lisansfiyatlari.DJINonPro;
                            }

                            if (lisansdurum.SPI == true)
                            {
                                if (user.ProNonPro == true)
                                    SPX = MyTools.lisansfiyatlari.SPIPro;
                                else
                                    SPX = MyTools.lisansfiyatlari.SPINonPro;
                            }

                            if (lisansdurum.XETRA == true)
                            {
                                if (user.ProNonPro == true)
                                    XETRA = MyTools.lisansfiyatlari.XETRAPro;
                                else
                                    XETRA = MyTools.lisansfiyatlari.XETRANonPro;
                            }

                        }



                        #endregion

                        if (lisansdurum.CBOT == true)
                        {
                            if (user.ProNonPro == true)
                                CBOT = MyTools.lisansfiyatlari.CBOTPro;
                            else
                                CBOT = MyTools.lisansfiyatlari.CBOTNonPro;
                        }

                        if (lisansdurum.CME == true)
                        {
                            if (user.ProNonPro == true)
                                CME = MyTools.lisansfiyatlari.CMEPro;
                            else
                                CME = MyTools.lisansfiyatlari.CMENonPro;
                        }

                        if (lisansdurum.CMEM == true)
                        {
                            if (user.ProNonPro == true)
                                CMEM = MyTools.lisansfiyatlari.CMEMPro;
                            else
                                CMEM = MyTools.lisansfiyatlari.CMEMNonPro;
                        }

                        if (lisansdurum.CBOTM == true)
                        {
                            if (user.ProNonPro == true)
                                CBOTM = MyTools.lisansfiyatlari.CBOTMPro;
                            else
                                CBOTM = MyTools.lisansfiyatlari.CBOTMNonPro;
                        }

                        if (lisansdurum.EUREX == true)
                        {
                            if (user.ProNonPro == true)
                                EUREX = MyTools.lisansfiyatlari.EUREXPro;
                            else
                                EUREX = MyTools.lisansfiyatlari.EUREXNonPro;
                        }
                        yurtdisitutar = spotpaket + DJI + SPX + XETRA + CBOT + CME + CBOTM + CMEM + EUREX;
                    }
                    // else yurtdisitanim = "ProNonPro Seçin";
                    #endregion

                    //var renk = Color.LightSalmon;

                    //gridLisanHesaplaOzet.Rows[0].SetValues("iDeal", profiyat.ToString("0.00"));
                    //gridLisanHesaplaOzet.Rows[1].SetValues("Mobil", cepfiyat.ToString("0.00"));
                    gridLisanHesaplaOzet.Rows[0].SetValues(paytanimi, paytutar.ToString("0.00"));
                    gridLisanHesaplaOzet.Rows[1].SetValues("Borsa Endeks", payendeks.ToString("0.00"));
                    gridLisanHesaplaOzet.Rows[2].SetValues("Pay Gün Sonu", pit.ToString("0.00"));
                    gridLisanHesaplaOzet.Rows[3].SetValues("PİTE", pite.ToString("0.00"));
                    gridLisanHesaplaOzet.Rows[4].SetValues(Vioptanimi, vioptutar.ToString("0.00"));
                    gridLisanHesaplaOzet.Rows[5].SetValues("Viop Gün Sonu", vit.ToString("0.00"));
                    gridLisanHesaplaOzet.Rows[6].SetValues(Tahviltanimi, Tahviltutar.ToString("0.00"));
                    gridLisanHesaplaOzet.Rows[7].SetValues("Karma Düzey 1", karmatutar.ToString("0.00"));
                    gridLisanHesaplaOzet.Rows[8].SetValues("MKK", mkktutar.ToString("0.00"));
                    gridLisanHesaplaOzet.Rows[9].SetValues("ALG", algtutar.ToString("0.00"));
                    gridLisanHesaplaOzet.Rows[10].SetValues("ROBOT", robottutar.ToString("0.00"));
                    gridLisanHesaplaOzet.Rows[11].SetValues("GKKUL", gkkultutar.ToString("0.00"));

                    // gridLisanHesaplaOzet.Rows[8].SetValues(yurtdisitanim, String.Format("{0:C}", yurtdisitutar));

                    gridLisanHesaplaOzet.Rows[12].SetValues("", "");
                    toplam = toplam + yurtdisitutar;
                    var kdvoran = 0.20m;
                    var top = String.Format("{0:C}", toplam);
                    var aratop = String.Format("{0:C}", toplam * kdvoran);
                    var geneltop = String.Format("{0:C}", toplam + (toplam * kdvoran));
                    gridLisanHesaplaOzet.Rows[14].SetValues("Toplam", top);

                    gridLisanHesaplaOzet.Rows[15].SetValues("KDV", String.Format("{0:C}", aratop));
                    gridLisanHesaplaOzet.Rows[16].SetValues("Genel Toplam", String.Format("{0:C}", geneltop));



                    foreach (DataGridViewRow row in gridLisanHesaplaOzet.Rows)
                    {

                        row.Cells[0].Style.ForeColor = Color.Firebrick;
                        row.Cells[0].Style.BackColor = Color.WhiteSmoke;

                    }


                    gridLisanHesaplaOzet.Rows[11].Cells[1].Selected = true;

                    #endregion
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        public void LisansEtkin(bool durum)
        {
            chkEmirTeyid.Enabled = durum;
            lisansdegistirme = durum;
            chkPayL1.Enabled = durum;
            chkPayLP.Enabled = durum;
            chkPayL2.Enabled = durum;
            chkPd2P.Enabled = durum;
            chkPayLP.Enabled = durum;
            chkPayGS.Enabled = durum;
            chkPayX.Enabled = durum;
            chkVeriAnalitik.Enabled = durum;
            chkPITE.Enabled = durum;
            chkVL1.Enabled = durum;
            chkVLP.Enabled = durum;
            chkVL2.Enabled = durum;
            chkVd2P.Enabled = durum;
            chkVGS.Enabled = durum;
            chkTL1.Enabled = durum;
            chkTL2.Enabled = durum;
            chkTLP.Enabled = durum;
            chkAnPro.Enabled = durum;
            chkMKK.Enabled = durum;
            chkCME.Enabled = durum;
            chkGKKUL.Enabled = durum;

            chkSentiL1.Enabled = durum;
            chkSentiL2.Enabled = durum;

            chkCep.Enabled = durum;
            chkPro.Enabled = durum;
            chkROBOT.Enabled = durum;

            chkProTarih.Enabled = durum;
            chkCepTarih.Enabled = durum;

            chkPayL1Tarih.Enabled = durum;
            chkPayLPTarih.Enabled = durum;
            chkPayL2Tarih.Enabled = durum;
            chkPd2PTarih.Enabled = durum;
            // chkPd2PTarih.Enabled = durum;
            chkPayLPTarih.Enabled = durum;
            chkPayGSTarih.Enabled = durum;
            chkPayXTarih.Enabled = durum;
            chkPITETarih.Enabled = durum;

            chkVL1Tarih.Enabled = durum;
            chkVLPTarih.Enabled = durum;
            chkVL2Tarih.Enabled = durum;
            chkVd2PTarih.Enabled = durum;
            chkVGSTarih.Enabled = durum;

            chkKRMD1Tarih.Enabled = durum;

            chkTL1Tarih.Enabled = durum;
            chkTL2Tarih.Enabled = durum;
            chkTLPTarih.Enabled = durum;
            chkAnProTarih.Enabled = durum;
            chkMkkTarih.Enabled = durum;
            chkGkkulTarih.Enabled = durum;
            chkCMETarih.Enabled = durum;
            chkROBOT.Enabled = durum;

            //chkRobotTarih.Enabled = durum;
            //chkTemelAnalizTarih.Enabled = durum;
            //chkBMKTarih.Enabled = durum;
            //chkBMCTarih.Enabled = durum;
            //chkCMEMTarih.Enabled = durum;
            //chkCMETarih.Enabled = durum;
            //chkBarSistemTarih.Enabled = durum;
            //chkFSystemTarih.Enabled = durum;
            //chkHISSEATarih.Enabled = durum;
            //chkParaTarih.Enabled = durum;
            chkSentiL1Tarih.Enabled = durum;
            chkSentiL2Tarih.Enabled = durum;

            chkCepTarih.Enabled = durum;
            chkProTarih.Enabled = durum;

            chkSCMDownload.Enabled = durum;
            chkSCMRealTime.Enabled = durum;
            chkSCMUsable.Enabled = durum;

            chkDJI.Enabled = durum;
            chkXetra.Enabled = durum;
            chkSpI.Enabled = durum;
            chkCBOTM.Enabled = durum;
            chkEUREX.Enabled = durum;
            chkCMEM.Enabled = durum;
            chkCBOT.Enabled = durum;
            chkCME2.Enabled = durum;
            chkComex.Enabled = durum;

        }
        public string lisanstoString(LisansDurum lisanlar)
        {
            try
            {
                var sb = new StringBuilder();
                if (lisanlar == null)
                    return "";


                if (lisanlar.YayinDurumu)
                    sb.Append("AÇIK;");
                else
                    sb.Append("KAPALI;");


                if (lisanlar.PayL1)
                    sb.Append("PayL1;");
                if (lisanlar.PayL2)
                    sb.Append("PayL2;");
                if (lisanlar.PayLP)
                    sb.Append("PayLP;");
                if (lisanlar.Pd2P)
                    sb.Append("Pd2P;");
                if (lisanlar.PayX)
                    sb.Append("PayX;");
                if (lisanlar.PayGS)
                    sb.Append("PayGS;");
                if (lisanlar.PITE)
                    sb.Append("PITE;");
                if (lisanlar.VeriAnalitik)
                    sb.Append("VeriAnalitik;");

                //viop
                if (lisanlar.ViopL1)
                    sb.Append("ViopL1;");
                if (lisanlar.ViopL2)
                    sb.Append("ViopL2;");
                if (lisanlar.Vd2P)
                    sb.Append("Vd2P;");
                if (lisanlar.ViopLP)
                    sb.Append("ViopLP;");
                if (lisanlar.ViopGS)
                    sb.Append("ViopGS;");
                //tahvil
                if (lisanlar.TahvilL1)
                    sb.Append("TahvilL1;");
                if (lisanlar.TahvilL2)
                    sb.Append("TahvilL2;");
                if (lisanlar.TahvilLP)
                    sb.Append("TahvilLP;");

                //Analiz Pro
                if (lisanlar.AnPro)
                    sb.Append("AnalizPro;");

                if (lisanlar.MKK)
                    sb.Append("MKK;");
                if (lisanlar.TARAMA)
                    sb.Append("TARAMA;");
                if (lisanlar.GKKUL)
                    sb.Append("GKKUL;");

                // local
                if (lisanlar.CepYetki)
                    sb.Append("CepYetki;");
                if (lisanlar.ProYetki)
                    sb.Append("ProYetki;");
                if (lisanlar.ROBOT)
                    sb.Append("ROBOT;");
                if (lisanlar.DJI)
                    sb.Append("DJI;");
                if (lisanlar.XETRA)
                    sb.Append("XETRA;");
                if (lisanlar.SPI)
                    sb.Append("SPI;");
                if (lisanlar.CBOTM)
                    sb.Append("CBOTM;");
                if (lisanlar.EUREX)
                    sb.Append("EUREX;");
                if (lisanlar.CMEM)
                    sb.Append("CMEM;");
                if (lisanlar.CBOT)
                    sb.Append("CBOT;");
                if (lisanlar.CME)
                    sb.Append("CME;");
                if (lisanlar.COMEX)
                    sb.Append("COMEX;");
                if (lisanlar.NYMEX)
                    sb.Append("NYMEX;");
                if (lisanlar.NYMEXM)
                    sb.Append("NYMEXM;");
                if (lisanlar.NYSE)
                    sb.Append("NYSE;");
                if (lisanlar.NASDAQ)
                    sb.Append("NASDAQ;");
                if (lisanlar.Amex)
                    sb.Append("Amex;");
                if (lisanlar.CHIX)
                    sb.Append("CHIX;");
                if (lisanlar.LSE)
                    sb.Append("LSE;");

                return sb.ToString();


            }
            catch
            {

                return "";
            }


        }

        public Musteriler MusteriBul(string Musterino)
        {
            var musteri = new Musteriler();
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                musteri = crm.Musterilers.FirstOrDefault(x => x.MusteriNo == Musterino);

                return musteri;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return musteri;
            }

        }

        void comboUlke_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboUlke.SelectedIndex == -1)
                    return;


                if (comboUlke.SelectedItem != comboUlke.Items[212])
                    comboILCE.SelectedIndex = -1;


                var ulkeid = 0;
                crmDFNDataContext crm = new crmDFNDataContext();
                if (comboUlke.SelectedValue is Ulke)
                {
                    Ulke deger = (Ulke)comboUlke.SelectedValue;
                    ulkeid = deger.Id;
                }
                else
                {
                    ulkeid = (int)comboUlke.SelectedValue;
                }

                if (crm.Ils.Where(x => x.UlkeId == ulkeid).Any())
                {
                    comboIL.DataSource = crm.Ils.OrderBy(x => x.IlAdi).Where(x => x.UlkeId == ulkeid);
                    comboIL.DisplayMember = "IlAdi";
                    comboIL.ValueMember = "Id";
                }


                if (!comboildoldurma)
                {
                    comboIL.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void comboIL_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboIL.SelectedIndex == -1)
                    return;
                var ilid = 0;
                var ulkeid = 0;
                crmDFNDataContext crm = new crmDFNDataContext();
                if (comboIL.SelectedValue is Il)
                {
                    Il deger = (Il)comboIL.SelectedValue;
                    ilid = deger.Id;
                    ulkeid = (int)deger.UlkeId;
                }
                else
                {
                    ilid = (int)comboIL.SelectedValue;
                    var il = crm.Ils.Where(x => x.Id == ilid).FirstOrDefault();
                    ulkeid = il.UlkeId.Value;

                }

                if (ulkeid == 213)
                {

                    if (crm.Ilces.Where(x => x.IlId == ilid).Any())
                    {
                        comboILCE.DataSource = crm.Ilces.OrderBy(x => x.IlceAdi).Where(x => x.IlId == ilid);
                        comboILCE.DisplayMember = "IlceAdi";
                        comboILCE.ValueMember = "Id";
                    }

                }

                if (!comboildoldurma)
                {
                    comboILCE.SelectedIndex = -1;
                }

                if (comboUlke.SelectedIndex != 212)
                {
                    comboILCE.SelectedIndex = -1;
                }

                comboILCE.SelectedIndex = -1;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void lisanDegistirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formAdminAra.referance.ActiveCalisan.YetiID > 2 && formAdminAra.referance.ActiveCalisan.YetiID != 6)
            {
                return;
            }

            //    if (comboYayinDurumu.SelectedIndex != 1)
            //{ }
            //else
            //{ MessageBox.Show("Bu kullanıcının yayını kapalı lisans eklemeden önce yayınını açmalısınız"); return; }
            LisansEtkin(true); lisansdegistirme = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            KaydetUpdate = true;

            crmDFNDataContext crm = new crmDFNDataContext();

            UserEvent ue = new UserEvent();
            ue.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
            ue.EventTarih = DateTime.Now;
            ue.IP = formAdminAra.referance.IpAdress;
            ue.HostName = formAdminAra.referance.HostName;
            bool gonderildimi = false;

            if (acticeitem == 0)
            {
                //var eType = crm.EventTypes.FirstOrDefault(x => x.EventTypeId == 4);
                //ue.EventType = eType;
                ue.EventTypeId = 5;

                #region Kontrol
                //if (txtUserName.Text.Trim() == "")
                //{
                //    MessageBox.Show("UserName Kısmı Boş Bırakılamaz");
                //    return;
                //}
                //if (txtTckn.Text.Trim() == "")
                //{
                //    MessageBox.Show("TCKN Kısmı Boş Bırakılamaz");
                //    return;
                //}
                if (txtUserName.Text.Trim() == "")
                {
                    MessageBox.Show("UserName Kısmı Boş Bırakılamaz");
                    return;
                }
                //if (crm.Users.Where(x => x.UserName == txtUserName.Text.Trim()).Any())
                //{
                //    MessageBox.Show("Bu Username Başka Kullanıcı Tarafından Kullanılmaktadır.\n Lütfen Başka Bir Kullanıcı ismi giriniz.");
                //    return;
                //}
                // if (crm.Users.Where(x => x.tckno == txtTckn.Text.Trim()).Any())
                if (crm.Users.Where(x => x.UserName == txtUserName.Text.Trim()).Any())
                {
                    // MessageBox.Show("Bu TCKN Başka Kullanıcı Tarafından Kullanılmaktadır.\n Lütfen Başka Bir TCKN numarası giriniz.");
                    MessageBox.Show("Bu UserName Başka Kullanıcı Tarafından Kullanılmaktadır.\n Lütfen Başka Bir Username değeri giriniz.");
                    return;
                }
                #endregion
                #region yenikayit
                try
                {
                    User user = new User();

                    if (txtPassword.Text == "")
                    { MessageBox.Show("Password Kısmı Boş Bırakılamaz"); return; }

                    //if (txtPassword.Text == "")
                    //{ MessageBox.Show("Password Kısmı Boş Bırakılamaz"); return; }

                    //if (string.IsNullOrWhiteSpace(txtTckn.Text))
                    //{ MessageBox.Show("TCKN Kısmı Boş Bırakılamaz"); return; }
                    if (string.IsNullOrWhiteSpace(txtUserName.Text))
                    { MessageBox.Show("UserName Kısmı Boş Bırakılamaz"); return; }
                    //if (txtTckn.Text.Length != 11 || !txtTckn.Text.All(char.IsDigit))
                    //{
                    //    MessageBox.Show("TCKN 11 haneli olmalı ve sadece rakamlardan oluşmalıdır.");
                    //    return;
                    //}
                    //if (txtUserName.Text.Length != 11 || !txtUserName.Text.All(char.IsDigit))
                    //{
                    //    MessageBox.Show("UserName 11 haneli olmalı ve sadece rakamlardan oluşmalıdır.");
                    //    return;
                    //}
                    if (MyTools.KurumKod == "10860" || MyTools.KurumKod == "10158" || MyTools.KurumKod == "10196")
                    {
                        chkCep.Checked = true;
                    }

                    //if (chkCep.Checked == false && chkCepTarih.Checked == false /* &&chkPro.Checked == false && chkProTarih.Checked == false*/)
                    //{ MessageBox.Show("Mobil Yetkisini seçmelisiniz.İleri de açılacaksa başlangıç tarihini ileriye atamalısınız."); return; }
                    //else if (chkCep.Checked == false && chkCepTarih.Checked == true && dtpCepStart.Value.Date <= DateTime.Now.Date && dtpCepEnd.Value.Date >= DateTime.Now.Date /*&& chkPro.Checked == false && chkProTarih.Checked == true && dtpProStart.Value.Date < DateTime.Now.Date && dtpProEnd.Value.Date >= DateTime.Now.Date*/)
                    //{
                    //    MessageBox.Show("Mobil Yetkisini seçmelisiniz"); return;
                    //}
                    //else if (chkPro.Checked == false && chkProTarih.Checked == true && dtpProStart.Value.Date <= DateTime.Now.Date && dtpProEnd.Value.Date > DateTime.Now.Date)
                    //{
                    //    MessageBox.Show("Pro Yetkisini Seçmelisiniz"); return;
                    //}
                    //else if (chkCep.Checked == false && chkCepTarih.Checked == true && dtpCepStart.Value.Date <= DateTime.Now.Date && dtpCepEnd.Value.Date > DateTime.Now.Date)
                    //{
                    //    MessageBox.Show("Mobil Yetkisini Seçmelisiniz"); return;
                    //}
                    //else
                    //{
                    //    if (chkPro.Checked == false && chkCep.Checked == false)
                    //    { MessageBox.Show("Desktop veya Mobile Yetkilerinden en az birini seçlemelisiniz"); return; }
                    //}

                    user.UserName = txtUserName.Text.Trim();
                    user.Password = MyTools.Sifreleme.Encryp(txtPassword.Text.Trim());
                    user.Name = txtAdi.Text;
                    user.Surname = txtSurname.Text;
                    user.EmirTeyid = chkEmirTeyid.Checked;
                    user.FXkurum = txtFxKurum.Text;
                    user.Aciklama = txtAciklama.Text.Trim();

                    if (txtPMTSno.Text == "")
                    {
                        // MessageBox.Show("Müşteri no bilgisiniz doldurunuz !"); return;
                    }

                    //if (crm.Users.Where(x => x.PmtsNo == txtPMTSno.Text.Trim()).Any())
                    //{
                    //    MessageBox.Show("Bu müşteri no bilgisi başka müşteri tarafından kullanılmakta"); return;
                    //}

                    user.PmtsNo = txtPMTSno.Text.Trim();

                    user.ProNonPro = comboProNonPro.SelectedIndex._ToBool();
                    if (comboSozlesme.SelectedIndex != -1)
                        user.SozlesmeID = (int)comboSozlesme.SelectedValue;

                    user.ProductType = "IDEAL";
                     user.tckno = txtTckn.Text;
                    user.UserName = txtUserName.Text;

                    user.StatusId = (int)comboStatus.SelectedValue;

                    #region Mensei
                    if (comboMensei.SelectedIndex == -1)
                        user.MusteriMenseiID = 1;
                    else
                        user.MusteriMenseiID = (int)comboMensei.SelectedValue;
                    if (user.MusteriMenseiID == 2 && (chkComex.Checked || chkKRMD1Tarih.Checked))
                    {
                        MessageBox.Show("karma Düzey 1 Yurdışı satşı yoktur"); return;
                    }
                    #endregion
                    #region iletisim
                    Iletisim ileti = new Iletisim();
                    ileti.Tel1 = txtTel1.Text;
                    ileti.Tel2 = txtTel2.Text;
                    ileti.Ceptel = txtCeptel.Text;
                    ileti.UlkeId = (int)comboUlke.SelectedValue;
                    if (comboIL.SelectedValue != null)
                        ileti.IlId = (int)comboIL.SelectedValue;
                    if (comboILCE.SelectedValue != null)
                        ileti.IlceId = (int)comboILCE.SelectedValue;
                    ileti.acikadres = txtAdres.Text;
                    ileti.email = txtMail.Text;

                    crm.Iletisims.InsertOnSubmit(ileti);
                    crm.SubmitChanges();

                    user.iletisimId = ileti.IletisimId;
                    user.Aciklama = txtAciklama.Text;

                    user.BaslangicTarihi = dateTimeStartDate.Value;
                    user.ExpiryDate = dateTimeExpiry.Value;


                    #endregion
                    #region Lisanslar
                    LisansDurum lisans = new LisansDurum();
                    lisans.YayinDurumu = true;
                    if (comboYayinDurumu.SelectedIndex == 0)
                        lisans.YayinDurumu = true;
                    else if (comboYayinDurumu.SelectedIndex == 1)
                        lisans.YayinDurumu = false;
                    else
                    {
                        MessageBox.Show("Yayın Durumunu Belirtiniz !");
                        return;
                    }
                    if (chkCep.Checked == false /*&& chkPro.Checked == false*/ && (/*dtpProStart.Value.Date > DateTime.Now.Date ||*/ dtpCepStart.Value.Date > DateTime.Now.Date))
                        lisans.YayinDurumu = false;

                    lisans.CepYetki = chkCep.Checked;
                    // Bist Pay
                    lisans.PayL1 = chkPayL1.Checked;
                    lisans.PayLP = chkPayLP.Checked;
                    lisans.PayL2 = chkPayL2.Checked;
                    lisans.Pd2P = chkPd2P.Checked;
                    lisans.PayGS = chkPayGS.Checked;
                    lisans.PayX = chkPayX.Checked;
                    lisans.PITE = chkPITE.Checked;
                    //  lisans.VeriAnalitik = chkVeriAnalitik.Checked;

                    //viop
                    lisans.ViopL1 = chkVL1.Checked;
                    lisans.ViopLP = chkVLP.Checked;
                    lisans.ViopL2 = chkVL2.Checked;
                    lisans.Vd2P = chkVd2P.Checked;
                    lisans.ViopGS = chkVGS.Checked;

                    // tahvil
                    lisans.TahvilL1 = chkTL1.Checked;
                    lisans.TahvilLP = chkTLP.Checked;
                    lisans.TahvilL2 = chkTL2.Checked;

                    //Analiz Pro
                    // lisans.AnPro = chkAnPro.Checked;

                    lisans.MKK = chkMKK.Checked;
                    lisans.CME = chkCME.Checked;
                    lisans.ROBOT = chkROBOT.Checked;
                    lisans.GKKUL = chkGKKUL.Checked;

                    // local
                    lisans.CepYetki = chkCep.Checked;
                    lisans.ProYetki = chkPro.Checked;
                    lisans.ROBOT = chkROBOT.Checked;
                    lisans.Futgck = chkFutGck.Checked;
                    lisans.WINX = chkWINX.Checked;
                    lisans.SCMDownload = chkSCMDownload.Checked;
                    lisans.SCMRealTıme = chkSCMRealTime.Checked;
                    lisans.SCMUsable = chkSCMUsable.Checked;

                    // sase
                    lisans.SaseL1 = chksaseL1.Checked;
                    lisans.SaseL2 = chkSaseL2.Checked;

                    // yurtdışı
                    lisans.DJI = chkDJI.Checked;
                    lisans.XETRA = chkXetra.Checked;
                    //lisans.SPI = chkSpI.Checked;
                    //lisans.SPI = true;
                    lisans.CBOT = chkCBOT.Checked;
                    lisans.CBOTM = chkCBOTM.Checked;
                    //  lisans.CME = chkCME2.Checked;
                    lisans.CMEM = chkCMEM.Checked;
                    lisans.EUREX = chkEUREX.Checked;
                    lisans.COMEX = chkComex.Checked;
                    lisans.NYMEX = chkNymex.Checked;
                    lisans.NYMEXM = chkNymexM.Checked;
                    lisans.NYSE = chkNYSE.Checked;
                    lisans.NASDAQ = chkNASDAQ.Checked;
                    lisans.Amex = chkAMEX.Checked;
                    lisans.CHIX = chkCHIX.Checked;
                    lisans.LSE = chkLSE.Checked;

                    lisans.SentiL1 = chkSentiL1.Checked;
                    lisans.SentiL2 = chkSentiL2.Checked;
                   // lisans.SPI = true;
                    #region LisansTarihleri

                    var illegaltarih = new StringBuilder();

                    if (chkProTarih.Checked || chkPro.Checked) { if (user.ExpiryDate.Value.Date < dtpProEnd.Value.Date) { illegaltarih.AppendLine("PRO"); dtpProEnd.Value = user.ExpiryDate.Value; } lisans.ProYetkiStart = dtpProStart.Value.Date; lisans.ProYetkiEnd = dtpProEnd.Value.Date; }
                    if (chkCepTarih.Checked || chkCep.Checked) { if (user.ExpiryDate.Value.Date < dtpCepEnd.Value.Date) { illegaltarih.AppendLine("CEP"); dtpCepEnd.Value = user.ExpiryDate.Value; } lisans.CepYetkiStart = dtpCepStart.Value.Date; lisans.CepYetkiEnd = dtpCepEnd.Value.Date; }
                    if (chkPayL1.Checked || chkPayL1Tarih.Checked) { if (user.ExpiryDate.Value.Date < dtpPayL1End.Value.Date) { illegaltarih.AppendLine("PD1"); dtpPayL1End.Value = user.ExpiryDate.Value; } lisans.PayL1Start = dtpPayL1Start.Value.Date; lisans.PayL1End = dtpPayL1End.Value.Date; }
                    if (chkPayLP.Checked || chkPayLPTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpPayLPEnd.Value.Date) { illegaltarih.AppendLine("PD1P"); dtpPayLPEnd.Value = user.ExpiryDate.Value; } lisans.PayLPStart = dtpPayLPStart.Value.Date; lisans.PayLPEnd = dtpPayLPEnd.Value.Date; }
                    if (chkPayL2.Checked || chkPayL2Tarih.Checked) { if (user.ExpiryDate.Value.Date < dtpPayL2End.Value.Date) { illegaltarih.AppendLine("PD2"); dtpPayL2End.Value = user.ExpiryDate.Value; } lisans.PayL2Start = dtpPayL2Start.Value.Date; lisans.PayL2End = dtpPayL2End.Value.Date; }
                    if (chkPd2P.Checked || chkPd2PTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpPd2PEnd.Value.Date) { illegaltarih.AppendLine("PD2P"); dtpPd2PEnd.Value = user.ExpiryDate.Value; } lisans.Pd2PStart = dtpPd2PStart.Value.Date; lisans.Pd2PEnd = dtpPd2PEnd.Value.Date; }
                    //if (chkPd2P.Checked || chkPd2PTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpPd2PEnd.Value.Date) { illegaltarih.AppendLine("PD2P"); dtpPd2PEnd.Value = user.ExpiryDate.Value; } lisans.Pd2PStart = dtpPd2PStart.Value.Date; lisans.Pd2Pend = dtpPd2PEnd.Value.Date; }
                    if (chkPayX.Checked || chkPayXTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpPayXEnd.Value.Date) { illegaltarih.AppendLine("END"); dtpPayXEnd.Value = user.ExpiryDate.Value; } lisans.PayXStart = dtpPayXStart.Value.Date; lisans.PayXEnd = dtpPayXEnd.Value.Date; }
                    if (chkPayGS.Checked || chkPayGSTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpPayGSEnd.Value.Date) { illegaltarih.AppendLine("PIT"); dtpPayGSEnd.Value = user.ExpiryDate.Value; } lisans.PayGSStart = dtpPayGSStart.Value.Date; lisans.PayGSEnd = dtpPayGSEnd.Value.Date; }
                    if (chkPITE.Checked || chkPITETarih.Checked) { if (user.ExpiryDate.Value.Date < dtpPITEEnd.Value.Date) { illegaltarih.AppendLine("PITE"); dtpPITEEnd.Value = user.ExpiryDate.Value; } lisans.PayPiteStart = dtpPITEStart.Value.Date; lisans.PayPiteEnd = dtpPITEEnd.Value.Date; }
                    if (chkVL1.Checked || chkVL1Tarih.Checked) { if (user.ExpiryDate.Value.Date < dtpViopL1End.Value.Date) { illegaltarih.AppendLine("VL1"); dtpViopL1End.Value = user.ExpiryDate.Value; } lisans.ViopL1Start = dtpViopL1Start.Value.Date; lisans.ViopL1End = dtpViopL1End.Value.Date; }
                    if (chkVLP.Checked || chkVLPTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpViopLPEnd.Value.Date) { illegaltarih.AppendLine("VL1p"); dtpViopLPEnd.Value = user.ExpiryDate.Value; } lisans.ViopLPStart = dtpViopLPStart.Value.Date; lisans.ViopLPEnd = dtpViopLPEnd.Value.Date; }
                    if (chkVL2.Checked || chkVL2Tarih.Checked) { if (user.ExpiryDate.Value.Date < dtpViopL2End.Value.Date) { illegaltarih.AppendLine("VL2"); dtpViopL2End.Value = user.ExpiryDate.Value; } lisans.ViopL2Start = dtpViopL2Start.Value.Date; lisans.ViopL2End = dtpViopL2End.Value.Date; }
                    if (chkVd2P.Checked || chkVd2PTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpVd2PEnd.Value.Date) { illegaltarih.AppendLine("VD2P"); dtpVd2PEnd.Value = user.ExpiryDate.Value; } lisans.Vd2PStart = dtpVd2PStart.Value.Date; lisans.Vd2PEnd = dtpVd2PEnd.Value.Date; }
                    if (chkVGS.Checked || chkVGSTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpViopGSEnd.Value.Date) { illegaltarih.AppendLine("VIT"); dtpViopGSEnd.Value = user.ExpiryDate.Value; } lisans.ViopGSStart = dtpViopGSStart.Value.Date; lisans.ViopGSEnd = dtpViopGSEnd.Value.Date; }
                    if (chkComex.Checked || chkKRMD1Tarih.Checked) { if (user.ExpiryDate.Value.Date < dtpKRMD1End.Value.Date) { illegaltarih.AppendLine("KRMD1"); dtpKRMD1End.Value = user.ExpiryDate.Value; } lisans.KRMD1Start = dtpKRMD1Start.Value.Date; lisans.KRMD1End = dtpKRMD1End.Value.Date; }
                    if (chkTL1.Checked || chkTL1Tarih.Checked) { if (user.ExpiryDate.Value.Date < dtpTahvilL1End.Value.Date) { illegaltarih.AppendLine("BD1"); dtpTahvilL1End.Value = user.ExpiryDate.Value; } lisans.TahvilL1Start = dtpTahvilL1Start.Value.Date; lisans.TahvilL1End = dtpTahvilL1End.Value.Date; }
                    if (chkTLP.Checked || chkTLPTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpTahvilLPEnd.Value.Date) { illegaltarih.AppendLine("BD1P"); dtpTahvilLPEnd.Value = user.ExpiryDate.Value; } lisans.TahvilLPStart = dtpTahvilLPStart.Value.Date; lisans.TahvilLPEnd = dtpTahvilLPEnd.Value.Date; }
                    if (chkTL2.Checked || chkTL2Tarih.Checked) { if (user.ExpiryDate.Value.Date < dtpTahvilL2End.Value.Date) { illegaltarih.AppendLine("BD2"); dtpTahvilL2End.Value = user.ExpiryDate.Value; } lisans.TahvilL2Start = dtpTahvilL2Start.Value.Date; lisans.TahvilL2End = dtpTahvilL2End.Value.Date; }
                    if (chkAnPro.Checked || chkAnProTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpAnProEnd.Value.Date) { illegaltarih.AppendLine("AnalizPro"); dtpAnProEnd.Value = user.ExpiryDate.Value; } lisans.AnProStart = dtpAnProStart.Value.Date; lisans.AnProEnd = dtpAnProEnd.Value.Date; }
                    //if (chkCME.Checked || chkCMETarih.Checked) { if (user.ExpiryDate.Value.Date < dtpCMEEnd.Value.Date) { illegaltarih.AppendLine("İdealGO"); dtpCMEEnd.Value = user.ExpiryDate.Value; } lisans.CMEStart = dtpCMEStart.Value.Date; lisans.CMEEnd = dtpCMEEnd.Value.Date; }
                    //if (chkCMEM.Checked || chkCMEMTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpCMEMEnd.Value.Date) { illegaltarih.AppendLine("UserDll"); dtpCMEMEnd.Value = user.ExpiryDate.Value; } lisans.CMEMStart = dtpCMEMStart.Value.Date; lisans.CMEMEnd = dtpCMEMEnd.Value.Date; }
                    //if (chkTemelAnaliz.Checked || chkTemelAnalizTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpTemelAnalizEnd.Value.Date) { illegaltarih.AppendLine("Temel Analiz"); dtpTemelAnalizEnd.Value = user.ExpiryDate.Value; } lisans.TemelAnalizStart = dtpTemelAnalizStart.Value.Date; lisans.TemelAnalizEnd = dtpTemelAnalizEnd.Value.Date; }
                    //if (chkBMK.Checked || chkBMKTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpBMKEnd.Value.Date) { illegaltarih.AppendLine("BMK"); dtpBMKEnd.Value = user.ExpiryDate.Value; } lisans.BMKStart = dtpBMKStart.Value.Date; lisans.BMKEnd = dtpBMKEnd.Value.Date; }
                    //if (chkBMC.Checked || chkBMCTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpBMCEnd.Value.Date) { illegaltarih.AppendLine("BMCAPITAL"); dtpBMCEnd.Value = user.ExpiryDate.Value; } lisans.BMCStart = dtpBMCStart.Value.Date; lisans.BMCEnd = dtpBMCEnd.Value.Date; }
                    //if (chkBarSistem.Checked || chkBarSistemTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpBarSistemEnd.Value.Date) { illegaltarih.AppendLine("Bar Sistem"); dtpBarSistemEnd.Value = user.ExpiryDate.Value; } lisans.BarSistemStart = dtpBarSistemStart.Value.Date; lisans.BarSistemEnd = dtpBarSistemEnd.Value.Date; }
                    //if (chkFSystem.Checked || chkFSystemTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpFSystemEnd.Value.Date) { illegaltarih.AppendLine("F System"); dtpFSystemEnd.Value = user.ExpiryDate.Value; } lisans.FSystemStart = dtpFSystemStart.Value.Date; lisans.FSystemEnd = dtpFSystemEnd.Value.Date; }
                    //if (chkPara.Checked || chkParaTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpParaEnd.Value.Date) { illegaltarih.AppendLine("PARA"); dtpParaEnd.Value = user.ExpiryDate.Value; } lisans.PARAStart = dtpParaStart.Value.Date; lisans.PARAEnd = dtpParaEnd.Value.Date; }
                    //if (chkHISSEA.Checked || chkHISSEATarih.Checked) { if (user.ExpiryDate.Value.Date < dtpHISSEAEnd.Value.Date) { illegaltarih.AppendLine("HISSEANALIZ"); dtpHISSEAEnd.Value = user.ExpiryDate.Value; } lisans.HISSEAStart = dtpHISSEAStart.Value.Date; lisans.HISSEAEnd = dtpHISSEAEnd.Value.Date; }
                    if (chkSentiL1.Checked || chkSentiL1Tarih.Checked) { if (user.ExpiryDate.Value.Date < dtpSentiL1End.Value.Date) { illegaltarih.AppendLine("SentiL1"); dtpSentiL1End.Value = user.ExpiryDate.Value; } lisans.SentiL1Start = dtpSentiL1Start.Value.Date; lisans.SentiL1End = dtpSentiL1End.Value.Date; }
                    if (chkSentiL2.Checked || chkSentiL2Tarih.Checked) { if (user.ExpiryDate.Value.Date < dtpSentiL2End.Value.Date) { illegaltarih.AppendLine("SentiL2"); dtpSentiL2End.Value = user.ExpiryDate.Value; } lisans.SentiL2Start = dtpSentiL2Start.Value.Date; lisans.SentiL2End = dtpSentiL2End.Value.Date; }
                    if (chkMKK.Checked || chkMkkTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpMkkEnd.Value.Date) { illegaltarih.AppendLine("MKK"); dtpMkkEnd.Value = user.ExpiryDate.Value; } lisans.MKKStart = dtpMkkStart.Value.Date; lisans.MKKEnd = dtpMkkEnd.Value.Date; }
                    if (chkGKKUL.Checked || chkGkkulTarih.Checked) { if (user.ExpiryDate.Value.Date < dtpGkkulEnd.Value.Date) { illegaltarih.AppendLine("GKKUL"); dtpGkkulEnd.Value = user.ExpiryDate.Value; } lisans.GKKULStart = dtpGkkulStart.Value.Date; lisans.GKKULEnd = dtpGkkulEnd.Value.Date; }
                    //if (chkCME.Checked || chkCMETarih.Checked) { if (user.ExpiryDate.Value.Date < dtpCMEEnd.Value.Date) { illegaltarih.AppendLine("TARAMA"); dtpCMEEnd.Value = user.ExpiryDate.Value; } lisans.TaramaStart = dtpCMEStart.Value.Date; lisans.TaramaEnd = dtpCMEEnd.Value.Date; }
                    if (chkCME.Checked || chkCMETarih.Checked) { if (user.ExpiryDate.Value.Date < dtpCMEEnd.Value.Date) { illegaltarih.AppendLine("CME"); dtpCMEEnd.Value = user.ExpiryDate.Value; } lisans.CMEStart = dtpCMEStart.Value.Date; lisans.CMEEnd = dtpCMEEnd.Value.Date; }




                    if (illegaltarih.ToString() != "")
                        if (MessageBox.Show(illegaltarih + "\nYukarıdaki Lisansların son tarihleri , ürün son tarihi ile aynı yapılmıştır.\nDevam edilsin mi?", "Lisans ürün tarih uyuşmazlığı", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                            return;
                    #endregion

                    #region Duzey1Kontrol
                    if (yayinKontrol == "KAPALI" && lisans.YayinDurumu == true)
                    {
                        if (lisans.PayL1 == true && lisans.PayLP == false)
                        {
                            lisans.PayL1 = false;
                        }

                        if (lisans.ViopL1 == true && lisans.ViopLP == false)
                        {
                            lisans.ViopL1 = false;
                        }

                        if (lisans.TahvilL1 == true && lisans.TahvilLP == false)
                        {
                            lisans.TahvilL1 = false;
                        }
                    }
                    #endregion
                    
                    var acilanlar = new StringBuilder();
                    if (lisans.ProYetki) acilanlar.Append("PRO;");
                    if (lisans.CepYetki) acilanlar.Append("MOBIL;");
                    if (lisans.PayL1) acilanlar.Append("PD1;");
                    if (lisans.PayLP) acilanlar.Append("PD1P;");
                    if (lisans.PayL2) acilanlar.Append("PD2;");
                    if (lisans.Pd2P) acilanlar.Append("PD2P;");
                    if (lisans.PayGS) acilanlar.Append("PIT;");
                    if (lisans.PayX) acilanlar.Append("END;");
                    if (lisans.PITE) acilanlar.Append("PITE;");
                    if (lisans.ViopL1) acilanlar.Append("VD1;");
                    if (lisans.ViopLP) acilanlar.Append("VD1P;");
                    if (lisans.ViopL2) acilanlar.Append("VD2;");
                    if (lisans.Vd2P) acilanlar.Append("VD2P;");
                    if (lisans.ViopGS) acilanlar.Append("VIT;");
                    if (lisans.TahvilL1) acilanlar.Append("BD1;");
                    if (lisans.TahvilLP) acilanlar.Append("BD1P;");
                    if (lisans.TahvilL2) acilanlar.Append("BD2;");
                    if (lisans.COMEX) acilanlar.Append("KRMD1;");
                    if (lisans.CME) acilanlar.Append("ALG;");
                    if (lisans.MKK) acilanlar.Append("MKK;");
                    if (lisans.GKKUL) acilanlar.Append("GKKUL;");
                    if (lisans.ROBOT) acilanlar.Append("ROBOT;");

                    ue.AcilanLisans = acilanlar.ToString();

                    crm.LisansDurums.InsertOnSubmit(lisans);
                    crm.SubmitChanges();

                    KurumsalBilgiler kbilgi = new KurumsalBilgiler();
                    kbilgi.kurumhesapno = txtKurumHesapNo.Text.Trim();
                    kbilgi.KurumSube = txtKurumSube.Text;
                    kbilgi.Not1 = txtNot1.Text.Trim();
                    kbilgi.KurumKullaniciTip = txtMusteriTip.Text.Trim();

                    // kbilgi.KurumSube = txtKurumSube.Text;
                    //if (comboKurumSube.Text != "")
                    //{
                    //    kbilgi.KurumSube = comboKurumSube.Text;
                    //}
                    crm.KurumsalBilgilers.InsertOnSubmit(kbilgi);
                    crm.SubmitChanges();

                    user.KurumsalBilgilerId = kbilgi.Id;

                    ue.SonLisandurumID = lisans.LisansDurumId;

                    user.LisansDurumId = lisans.LisansDurumId;

                    crm.Users.InsertOnSubmit(user);
                    crm.SubmitChanges();
                   
                    MyTools.logyaz($"[YeniKullanici] UserName:{user.UserName} | Yapan:{formAdminAra.referance.ActiveCalisan.Ad} | IP:{formAdminAra.referance.IpAdress}");
                   
                    if (user.UserID > 0)
                    {
                        ue.UserId = user.UserID;
                        crm.UserEvents.InsertOnSubmit(ue);
                        crm.SubmitChanges();
                        formAdminAra.referance.IPport.DataToSend = "CreateUser|" + user.UserID.ToString() + (char)3;
                        gonderildimi = true;
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Yeni Kayıt İşlemi Sırasında HATA\n" + ex.Message);
                }
                #endregion
            }
            else
            {
                #region Guncelle


                var kaptianlar = new StringBuilder();
                var acilanlar = new StringBuilder();
                var tarihdegisen = new StringBuilder();

                //if (chkPro.Checked == false && chkCep.Checked == false)
                //{ MessageBox.Show("Mobile Yetkisini Seçlemelisiniz"); return; }
                //else if (chkCep.Checked == false && chkCepTarih.Checked == true && dtpCepStart.Value.Date <= DateTime.Now.Date && dtpCepEnd.Value.Date >= DateTime.Now.Date && chkPro.Checked == false && chkProTarih.Checked == true && dtpProStart.Value.Date < DateTime.Now.Date && dtpProEnd.Value.Date >= DateTime.Now.Date)
                //{
                //    MessageBox.Show("Cep Yetkisini Seçmelisiniz"); return;
                //}
                //else if (chkPro.Checked == false && chkProTarih.Checked == true && dtpProStart.Value.Date <= DateTime.Now.Date && dtpProEnd.Value.Date >= DateTime.Now.Date)
                //{
                //    MessageBox.Show("Pro Yetkisini Seçmelisiniz"); return;
                //}
                //else if (chkCep.Checked == false && chkCepTarih.Checked == true && dtpCepStart.Value.Date <= DateTime.Now.Date && dtpCepEnd.Value.Date >= DateTime.Now.Date)
                //{
                //    MessageBox.Show("Mobil Yetkisini Seçmelisiniz"); return;
                //}

                if (acticeitem == 0)
                {
                    MessageBox.Show("Böyle Bir Kayıt Bulunmuyor");
                    return;
                }

                ue.UserId = acticeitem;

                var activeuser = crm.Users.FirstOrDefault(x => x.UserID == acticeitem);


                activeuser.ProductType = "IDEAL";

                if (activeuser.UserName != txtUserName.Text.Trim())
                {
                    txtUserName.Text = activeuser.UserName;
                    MessageBox.Show("Var olan bir kullanıcın ismini değiştiremezsiniz.");
                    return;
                }
                activeuser.UserName = txtUserName.Text.Trim();
                activeuser.Password = MyTools.Sifreleme.Encryp(txtPassword.Text.Trim());
                activeuser.Name = txtAdi.Text;
                activeuser.Surname = txtSurname.Text;
                activeuser.EmirTeyid = chkEmirTeyid.Checked;
                activeuser.FXkurum = txtFxKurum.Text;
                activeuser.Aciklama = txtAciklama.Text.Trim();


                activeuser.BaslangicTarihi = dateTimeStartDate.Value;
                activeuser.ExpiryDate = dateTimeExpiry.Value;
                activeuser.ProNonPro = comboProNonPro.SelectedIndex._ToBool();
                if (comboSozlesme.SelectedIndex != -1)
                    activeuser.SozlesmeID = (int)comboSozlesme.SelectedValue;

                activeuser.PmtsNo = txtPMTSno.Text.Trim();
                // if (!string.IsNullOrWhiteSpace(txtTckn.Text) && txtTckn.Text.Length == 11 && txtTckn.Text.All(char.IsDigit))
                //  {
                // activeuser.UserName = txtTckn.Text.Trim();
                activeuser.tckno = txtTckn.Text.Trim();
                // }
                //else
                //{
                //    MessageBox.Show("Geçerli bir TCKN giriniz. (11 haneli ve sadece rakamlardan oluşmalı)");
                //    return;
                //}

                activeuser.StatusId = (int)comboStatus.SelectedValue;

                if (activeuser.KurumsalBilgilerId == null)
                {
                    KurumsalBilgiler kbilgi = new KurumsalBilgiler();
                    kbilgi.kurumhesapno = txtKurumHesapNo.Text.Trim();
                    kbilgi.Not1 = txtNot1.Text;
                    kbilgi.KurumSube = txtKurumSube.Text.Trim();
                    //if (comboKurumSube.Text.Trim() != "")
                    //{
                    //    //  kbilgi.KurumSube = txtKurumSube.Text.Trim();
                    //    kbilgi.KurumSube = comboKurumSube.Text.Trim();
                    //}
                    kbilgi.KurumKullaniciTip = txtMusteriTip.Text.Trim();
                    crm.KurumsalBilgilers.InsertOnSubmit(kbilgi);
                    crm.SubmitChanges();
                    activeuser.KurumsalBilgilerId = kbilgi.Id;


                }
                else
                {
                    activeuser.KurumsalBilgiler.KurumSube = txtKurumSube.Text.Trim();
                    activeuser.KurumsalBilgiler.kurumhesapno = txtKurumHesapNo.Text.Trim();
                    activeuser.KurumsalBilgiler.Not1 = txtNot1.Text.Trim();
                    //activeuser.KurumsalBilgiler.KurumSube = txtKurumSube.Text.Trim();
                    activeuser.KurumsalBilgiler.KurumKullaniciTip = txtMusteriTip.Text.Trim();

                }

                #region Mensei
                if (comboMensei.SelectedIndex == -1)
                    activeuser.MusteriMenseiID = 1;
                else
                    activeuser.MusteriMenseiID = (int)comboMensei.SelectedValue;
                #endregion

                #region Lisanslar

                LisansDurum lisanslar = new LisansDurum();

                var yayind = false;

                if (comboYayinDurumu.SelectedIndex == 0)
                    yayind = true;
                else yayind = false;

                var lisansdegistimi = false;
                int lastLisansId = (int)activeuser.LisansDurumId;

                if (activeuser.LisansDurum.YayinDurumu != yayind)
                {
                    if (formAdminAra.referance.ActiveCalisan.YetiID > 1)
                    {
                        MessageBox.Show("Bu işlemi Yapmaya Yetkiniz Bulunmamaktatır");
                        return;
                    }

                    if (yayind == true)
                        ue.EventTypeId = 1;
                    else
                        ue.EventTypeId = 2;

                    lisanslar.YayinDurumu = yayind;
                    crm.UserEvents.InsertOnSubmit(ue);
                    crm.SubmitChanges();
                    lisansdegistimi = true;
                }
                else
                    lisanslar.YayinDurumu = yayind;
                if (chkCep.Checked == false /*&& chkPro.Checked == false*/ && (/*dtpProStart.Value.Date > DateTime.Now.Date ||*/ dtpCepStart.Value.Date > DateTime.Now.Date))
                    lisanslar.YayinDurumu = false;
                #region iletisim
                if (activeuser.iletisimId != null)
                {
                    if (activeuser.Iletisim.Tel1 != txtTel1.Text)
                    {
                        activeuser.Iletisim.Tel1 = txtTel1.Text;
                    }
                    if (activeuser.Iletisim.Tel2 != txtTel2.Text)
                    {
                        activeuser.Iletisim.Tel2 = txtTel2.Text;
                    }
                    if (activeuser.Iletisim.Ceptel != txtCeptel.Text)
                    {
                        activeuser.Iletisim.Ceptel = txtCeptel.Text;
                    }
                    if (comboUlke.SelectedValue != null)
                        if (activeuser.Iletisim.UlkeId != (int)comboUlke.SelectedValue)
                        {
                            activeuser.Iletisim.UlkeId = (int)comboUlke.SelectedValue;
                        }
                    if (comboIL.SelectedValue != null)
                        if (activeuser.Iletisim.IlId != (int)comboIL.SelectedValue)
                        {
                            activeuser.Iletisim.IlId = (int)comboIL.SelectedValue;
                        }
                    if (comboILCE.SelectedValue != null)
                        if (activeuser.Iletisim.IlceId != (int)comboILCE.SelectedValue)
                        {
                            activeuser.Iletisim.IlceId = (int)comboILCE.SelectedValue;
                        }
                    if (activeuser.Iletisim.acikadres != txtAdres.Text)
                    {
                        activeuser.Iletisim.acikadres = txtAdres.Text;
                    }
                    if (activeuser.Iletisim.email != txtMail.Text)
                    {
                        activeuser.Iletisim.email = txtMail.Text;
                    }
                }
                else
                {
                    Iletisim ileti = new Iletisim();
                    ileti.Tel1 = txtTel1.Text;
                    ileti.Tel2 = txtTel2.Text;
                    ileti.Ceptel = txtCeptel.Text;
                    ileti.UlkeId = (int)comboUlke.SelectedValue;
                    if (comboIL.SelectedValue != null)
                        ileti.IlId = (int)comboIL.SelectedValue;
                    if (comboILCE.SelectedValue != null)
                        ileti.IlceId = (int)comboILCE.SelectedValue;
                    ileti.acikadres = txtAdres.Text;
                    ileti.email = txtMail.Text;

                    crm.Iletisims.InsertOnSubmit(ileti);
                    crm.SubmitChanges();

                    activeuser.iletisimId = ileti.IletisimId;
                }
                #endregion
                #region BistPay
                // Bist Pay
                if (activeuser.LisansDurum.PayL1 != chkPayL1.Checked)
                {
                    if (chkPayL1.Checked == true)
                    {
                        acilanlar.Append("PayL1;");
                    }
                    else
                    {
                        kaptianlar.Append("PayL1;");
                    }
                    lisanslar.PayL1 = chkPayL1.Checked;
                }
                else
                    lisanslar.PayL1 = activeuser.LisansDurum.PayL1;

                if (activeuser.LisansDurum.PayLP != chkPayLP.Checked)
                {
                    if (chkPayLP.Checked == true)
                    {

                        acilanlar.Append("PayLP;");
                    }
                    else
                    {
                        kaptianlar.Append("PayLP;");
                    }
                    lisanslar.PayLP = chkPayLP.Checked;
                }
                else
                    lisanslar.PayLP = activeuser.LisansDurum.PayLP;

                if (activeuser.LisansDurum.PayL2 != chkPayL2.Checked)
                {
                    if (chkPayL2.Checked == true) acilanlar.Append("PayL2;"); else kaptianlar.Append("PayL2;");
                    lisanslar.PayL2 = chkPayL2.Checked;
                }
                else
                    lisanslar.PayL2 = activeuser.LisansDurum.PayL2;

                if (activeuser.LisansDurum.Pd2P != chkPd2P.Checked)
                {
                    if (chkPd2P.Checked == true) acilanlar.Append("Pd2P;"); else kaptianlar.Append("Pd2P;");
                    lisanslar.Pd2P = chkPd2P.Checked;
                }
                else
                    lisanslar.Pd2P = activeuser.LisansDurum.Pd2P;

                if (activeuser.LisansDurum.PayGS != chkPayGS.Checked)
                {
                    if (chkPayGS.Checked == true) acilanlar.Append("PayGS;"); else kaptianlar.Append("PayGS;");
                    lisanslar.PayGS = chkPayGS.Checked;
                }
                else
                    lisanslar.PayGS = activeuser.LisansDurum.PayGS;

                if (activeuser.LisansDurum.PayX != chkPayX.Checked)
                {
                    if (chkPayX.Checked == true) acilanlar.Append("PayX;"); else kaptianlar.Append("PayX;");
                    lisanslar.PayX = chkPayX.Checked;
                }
                else
                    lisanslar.PayX = activeuser.LisansDurum.PayX;


                if (activeuser.LisansDurum.VeriAnalitik != chkVeriAnalitik.Checked)
                {
                    if (chkVeriAnalitik.Checked == true) acilanlar.Append("PayAnalitik;"); else kaptianlar.Append("PayAnalitik;");
                    lisanslar.VeriAnalitik = chkVeriAnalitik.Checked;
                }
                else
                    lisanslar.VeriAnalitik = activeuser.LisansDurum.VeriAnalitik;

                if (activeuser.LisansDurum.PITE != chkPITE.Checked)
                {
                    if (chkPITE.Checked == true) acilanlar.Append("PITE;"); else kaptianlar.Append("PITE;");
                    lisanslar.PITE = chkPITE.Checked;
                }
                else
                    lisanslar.PITE = activeuser.LisansDurum.PITE;

                if (activeuser.LisansDurum.MKK != chkMKK.Checked)
                {
                    if (chkMKK.Checked == true) acilanlar.Append("MKK;"); else kaptianlar.Append("MKK;");
                    lisanslar.MKK = chkMKK.Checked;
                }
                else
                    lisanslar.MKK = activeuser.LisansDurum.MKK;

                if (activeuser.LisansDurum.GKKUL != chkGKKUL.Checked)
                {
                    if (chkGKKUL.Checked == true) acilanlar.Append("GKKUL;"); else kaptianlar.Append("GKKUL;");
                    lisanslar.GKKUL = chkGKKUL.Checked;
                }
                else
                    lisanslar.GKKUL = activeuser.LisansDurum.GKKUL;

                if (activeuser.LisansDurum.CME != chkCME.Checked)
                {
                    if (chkCME.Checked == true) acilanlar.Append("CME;"); else kaptianlar.Append("CME;");
                    lisanslar.CME = chkCME.Checked;
                }
                else
                    lisanslar.CME = activeuser.LisansDurum.CME;
                if (activeuser.LisansDurum.ROBOT != chkROBOT.Checked)
                {
                    if (chkROBOT.Checked == true) acilanlar.Append("ROBOT;"); else kaptianlar.Append("ROBOT;");
                    lisanslar.ROBOT = chkROBOT.Checked;
                }
                else
                    lisanslar.ROBOT = activeuser.LisansDurum.ROBOT;
                #endregion
                #region Viop
                //viop

                if (activeuser.LisansDurum.ViopL1 != chkVL1.Checked)
                {
                    if (chkVL1.Checked == true) acilanlar.Append("ViopL1;"); else kaptianlar.Append("ViopL1;");
                    lisanslar.ViopL1 = chkVL1.Checked;
                }
                else
                    lisanslar.ViopL1 = activeuser.LisansDurum.ViopL1;


                if (activeuser.LisansDurum.ViopLP != chkVLP.Checked)
                {
                    if (chkVLP.Checked == true) acilanlar.Append("ViopLP;"); else kaptianlar.Append("ViopLP;");
                    lisanslar.ViopLP = chkVLP.Checked;
                }
                else
                    lisanslar.ViopLP = activeuser.LisansDurum.ViopLP;

                if (activeuser.LisansDurum.ViopL2 != chkVL2.Checked)
                {
                    if (chkVL2.Checked == true) acilanlar.Append("ViopL2;"); else kaptianlar.Append("ViopL2;");
                    lisanslar.ViopL2 = chkVL2.Checked;
                }
                else
                    lisanslar.ViopL2 = activeuser.LisansDurum.ViopL2;

                if (activeuser.LisansDurum.Vd2P != chkVd2P.Checked)
                {
                    if (chkVd2P.Checked == true) acilanlar.Append("Vd2P"); else kaptianlar.Append("Vd2P;");
                    lisanslar.Vd2P = chkVd2P.Checked;
                }
                else
                    lisanslar.Vd2P = activeuser.LisansDurum.Vd2P;

                if (activeuser.LisansDurum.ViopGS != chkVGS.Checked)
                {
                    if (chkVGS.Checked == true) acilanlar.Append("ViopGS;"); else kaptianlar.Append("ViopGS;");
                    lisanslar.ViopGS = chkVGS.Checked;
                }
                else
                    lisanslar.ViopGS = activeuser.LisansDurum.ViopGS;

                #endregion
                #region tahvil

                // tahvil

                if (activeuser.LisansDurum.TahvilL1 != chkTL1.Checked)
                {
                    if (chkTL1.Checked == true) acilanlar.Append("TahvilL1;"); else kaptianlar.Append("TahvilL1;");
                    lisanslar.TahvilL1 = chkTL1.Checked;
                }
                else
                    lisanslar.TahvilL1 = activeuser.LisansDurum.TahvilL1;

                if (activeuser.LisansDurum.TahvilLP != chkTLP.Checked)
                {
                    if (chkTLP.Checked == true) acilanlar.Append("TahvilLP;"); else kaptianlar.Append("TahvilLP;");
                    lisanslar.TahvilLP = chkTLP.Checked;
                }
                else
                    lisanslar.TahvilLP = activeuser.LisansDurum.TahvilLP;

                if (activeuser.LisansDurum.TahvilL2 != chkTL2.Checked)
                {
                    if (chkTL2.Checked == true) acilanlar.Append("TahvilL2;"); else kaptianlar.Append("TahvilL2;");
                    lisanslar.TahvilL2 = chkTL2.Checked;
                }
                else
                    lisanslar.TahvilL2 = activeuser.LisansDurum.TahvilL2;


                #endregion
                #region AnalizPro
                if (activeuser.LisansDurum.AnPro != chkAnPro.Checked)
                {
                    if (chkAnPro.Checked == true) acilanlar.Append("AnalizPro;"); else kaptianlar.Append("AnalizPro;");
                    lisanslar.AnPro = chkAnPro.Checked;
                }
                else
                    lisanslar.AnPro = activeuser.LisansDurum.AnPro;
                #endregion
                #region senti

                // senti

                if (activeuser.LisansDurum.SentiL1 != chkSentiL1.Checked)
                {
                    if (chkSentiL1.Checked == true) acilanlar.Append("SentiL1;"); else kaptianlar.Append("SentiL1;");
                    lisanslar.SentiL1 = chkSentiL1.Checked;
                }
                else
                    lisanslar.SentiL1 = activeuser.LisansDurum.SentiL1;

                if (activeuser.LisansDurum.SentiL2 != chkSentiL2.Checked)
                {
                    if (chkSentiL2.Checked == true) acilanlar.Append("SentiL2;"); else kaptianlar.Append("SentiL2;");
                    lisanslar.SentiL2 = chkSentiL2.Checked;
                }
                else
                    lisanslar.SentiL2 = activeuser.LisansDurum.SentiL2;

                #endregion
                #region local
                // local
                if (activeuser.LisansDurum.CepYetki != chkCep.Checked)
                {
                    if (chkCep.Checked == true) acilanlar.Append("CepYetki;"); else kaptianlar.Append("CepYetki;");
                    lisanslar.CepYetki = chkCep.Checked;
                }
                else
                    lisanslar.CepYetki = activeuser.LisansDurum.CepYetki;

                if (activeuser.LisansDurum.ProYetki != chkPro.Checked)
                {
                    if (chkPro.Checked == true) acilanlar.Append("ProYetki;"); else kaptianlar.Append("ProYetki;");
                    lisanslar.ProYetki = chkPro.Checked;
                }
                else
                    lisanslar.ProYetki = activeuser.LisansDurum.ProYetki;

                if (activeuser.LisansDurum.ROBOT != chkROBOT.Checked)
                {
                    if (chkROBOT.Checked == true) acilanlar.Append("ROBOT;"); else kaptianlar.Append("ROBOT;");
                    lisanslar.ROBOT = chkROBOT.Checked;
                }
                else
                    lisanslar.ROBOT = activeuser.LisansDurum.ROBOT;



                if (activeuser.LisansDurum.SCMDownload != chkSCMDownload.Checked)
                {
                    if (chkSCMDownload.Checked == true) acilanlar.Append("SCMDownload;"); else kaptianlar.Append("SCMDownload;");
                    lisanslar.SCMDownload = chkSCMDownload.Checked;
                }
                else
                    lisanslar.SCMDownload = activeuser.LisansDurum.SCMDownload;


                if (activeuser.LisansDurum.SCMRealTıme != chkSCMRealTime.Checked)
                {
                    if (chkSCMRealTime.Checked == true) acilanlar.Append("SCMRealTıme;"); else kaptianlar.Append("SCMRealTıme;");
                    lisanslar.SCMRealTıme = chkSCMRealTime.Checked;
                }
                else
                    lisanslar.SCMRealTıme = activeuser.LisansDurum.SCMRealTıme;


                if (activeuser.LisansDurum.SCMUsable != chkSCMUsable.Checked)
                {
                    if (chkSCMUsable.Checked == true) acilanlar.Append("SCMUsable;"); else kaptianlar.Append("SCMUsable;");
                    lisanslar.SCMUsable = chkSCMUsable.Checked;
                }
                else
                    lisanslar.SCMUsable = activeuser.LisansDurum.SCMUsable;




                lisanslar.Futgck = chkFutGck.Checked;
                lisanslar.WINX = chkWINX.Checked;
                #endregion
                #region sase

                // sase

                lisanslar.SaseL1 = chksaseL1.Checked;
                lisanslar.SaseL2 = chkSaseL2.Checked;

                #endregion

                //if (acilanlar.Length > 0 || kaptianlar.Length > 0)
                //    lisansdegistirme = true;
                #region YurtDisi

                // yurtdışı



                if (activeuser.LisansDurum.DJI != chkDJI.Checked)
                {
                    if (chkDJI.Checked == true) acilanlar.Append("DJI;"); else kaptianlar.Append("DJI;");
                    lisanslar.DJI = chkDJI.Checked;
                }
                else
                    lisanslar.DJI = activeuser.LisansDurum.DJI;

                if (activeuser.LisansDurum.XETRA != chkXetra.Checked)
                {
                    if (chkXetra.Checked == true) acilanlar.Append("XETRA;"); else kaptianlar.Append("XETRA;");
                    lisanslar.XETRA = chkXetra.Checked;
                }
                else
                    lisanslar.XETRA = activeuser.LisansDurum.XETRA;

                if (activeuser.LisansDurum.SPI != true)
                    acilanlar.Append("SPI;");
                lisanslar.SPI = true;
                //else
                //    lisanslar.SPI = activeuser.LisansDurum.SPI;

                if (activeuser.LisansDurum.CBOT != chkCBOT.Checked)
                {
                    if (chkCBOT.Checked == true) acilanlar.Append("CBOT;"); else kaptianlar.Append("CBOT;");
                    lisanslar.CBOT = chkCBOT.Checked;
                }
                else
                    lisanslar.CBOT = activeuser.LisansDurum.CBOT;


                if (activeuser.LisansDurum.CBOTM != chkCBOTM.Checked)
                {
                    if (chkCBOTM.Checked == true) acilanlar.Append("CBOTM;"); else kaptianlar.Append("CBOTM;");
                    lisanslar.CBOTM = chkCBOTM.Checked;
                }
                else
                    lisanslar.CBOTM = activeuser.LisansDurum.CBOTM;


        

                if (activeuser.LisansDurum.CMEM != chkCMEM.Checked)
                {
                    if (chkCMEM.Checked == true) acilanlar.Append("CMEM;"); else kaptianlar.Append("CMEM;");
                    lisanslar.CMEM = chkCMEM.Checked;
                }
                else
                    lisanslar.CMEM = activeuser.LisansDurum.CMEM;


                if (activeuser.LisansDurum.EUREX != chkEUREX.Checked)
                {
                    if (chkEUREX.Checked == true) acilanlar.Append("EUREX;"); else kaptianlar.Append("EUREX;");
                    lisanslar.EUREX = chkEUREX.Checked;
                }
                else
                    lisanslar.EUREX = activeuser.LisansDurum.EUREX;


                if (activeuser.LisansDurum.COMEX != chkComex.Checked)
                {
                    if (chkComex.Checked == true) acilanlar.Append("KRMD1;"); else kaptianlar.Append("KRMD1;");
                    lisanslar.COMEX = chkComex.Checked;
                }
                else
                    lisanslar.COMEX = activeuser.LisansDurum.COMEX;

          
                //if (activeuser.LisansDurum.TARAMA != chkCME.Checked)
                //{
                //    if (chkCME.Checked == true) acilanlar.Append("TARAMA;"); else kaptianlar.Append("TARAMA;");
                //    lisanslar.TARAMA = chkCME.Checked;
                //}
                //else
                //    lisanslar.TARAMA = activeuser.LisansDurum.TARAMA;
                if (activeuser.LisansDurum.ROBOT != chkROBOT.Checked)
                {
                    if (chkROBOT.Checked == true) acilanlar.Append("ROBOT;"); else kaptianlar.Append("ROBOT;");
                    lisanslar.ROBOT = chkROBOT.Checked;
                }
                else
                    lisanslar.ROBOT = activeuser.LisansDurum.ROBOT;
                if (activeuser.LisansDurum.CME != chkCME.Checked)
                {
                    if (chkCME.Checked == true) acilanlar.Append("CME;"); else kaptianlar.Append("CME;");
                    lisanslar.CME = chkCME.Checked;
                }
                else
                    lisanslar.CME = activeuser.LisansDurum.CME;


                #endregion

                if (acilanlar.Length > 0 || kaptianlar.Length > 0)
                    lisansdegistirme = true;
                #region LisansTarihleri
                var illegaltarih = new StringBuilder();

                //if (chkRobot.Checked || chkRobotTarih.Checked)
                //{
                //    if (activeuser.ExpiryDate.Value.Date < dtpROBOTEnd.Value.Date)
                //    {
                //        illegaltarih.AppendLine("Robot");
                //        dtpROBOTEnd.Value = activeuser.ExpiryDate.Value;
                //    }
                //    lisanslar.RobotStart = dtpROBOTStart.Value.Date;
                //    lisanslar.RobotEnd = dtpROBOTEnd.Value.Date;
                //}
                if (chkPro.Checked || chkProTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpProEnd.Value.Date) { illegaltarih.AppendLine("PRO"); dtpProEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.ProYetkiStart = dtpProStart.Value.Date; lisanslar.ProYetkiEnd = dtpProEnd.Value.Date; }
                if (chkCep.Checked || chkCepTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpCepEnd.Value.Date) { illegaltarih.AppendLine("CEP"); dtpCepEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.CepYetkiStart = dtpCepStart.Value.Date; lisanslar.CepYetkiEnd = dtpCepEnd.Value.Date; }
                if (chkPayL1.Checked || chkPayL1Tarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpPayL1End.Value.Date) { illegaltarih.AppendLine("PD1"); dtpPayL1End.Value = activeuser.ExpiryDate.Value; } lisanslar.PayL1Start = dtpPayL1Start.Value.Date; lisanslar.PayL1End = dtpPayL1End.Value.Date; }
                if (chkPayLP.Checked || chkPayLPTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpPayLPEnd.Value.Date) { illegaltarih.AppendLine("PD1P"); dtpPayLPEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.PayLPStart = dtpPayLPStart.Value.Date; lisanslar.PayLPEnd = dtpPayLPEnd.Value.Date; }
                if (chkPayL2.Checked || chkPayL2Tarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpPayL2End.Value.Date) { illegaltarih.AppendLine("PD2"); dtpPayL2End.Value = activeuser.ExpiryDate.Value; } lisanslar.PayL2Start = dtpPayL2Start.Value.Date; lisanslar.PayL2End = dtpPayL2End.Value.Date; }
                if (chkPd2P.Checked || chkPd2PTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpPd2PEnd.Value.Date) { illegaltarih.AppendLine("PD2P"); dtpPd2PEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.Pd2PStart = dtpPd2PStart.Value.Date; lisanslar.Pd2PEnd = dtpPd2PEnd.Value.Date; }
                if (chkPayX.Checked || chkPayXTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpPayXEnd.Value.Date) { illegaltarih.AppendLine("END"); dtpPayXEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.PayXStart = dtpPayXStart.Value.Date; lisanslar.PayXEnd = dtpPayXEnd.Value.Date; }
                if (chkPayGS.Checked || chkPayGSTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpPayGSEnd.Value.Date) { illegaltarih.AppendLine("PIT"); dtpPayGSEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.PayGSStart = dtpPayGSStart.Value.Date; lisanslar.PayGSEnd = dtpPayGSEnd.Value.Date; }
                if (chkPITE.Checked || chkPITETarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpPITEEnd.Value.Date) { illegaltarih.AppendLine("PITE"); dtpPITEEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.PayPiteStart = dtpPITEStart.Value.Date; lisanslar.PayPiteEnd = dtpPITEEnd.Value.Date; }
                if (chkVL1.Checked || chkVL1Tarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpViopL1End.Value.Date) { illegaltarih.AppendLine("VD1"); dtpViopL1End.Value = activeuser.ExpiryDate.Value; } lisanslar.ViopL1Start = dtpViopL1Start.Value.Date; lisanslar.ViopL1End = dtpViopL1End.Value.Date; }
                if (chkVLP.Checked || chkVLPTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpViopLPEnd.Value.Date) { illegaltarih.AppendLine("VD1P"); dtpViopLPEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.ViopLPStart = dtpViopLPStart.Value.Date; lisanslar.ViopLPEnd = dtpViopLPEnd.Value.Date; }
                if (chkVL2.Checked || chkVL2Tarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpViopL2End.Value.Date) { illegaltarih.AppendLine("VD2"); dtpViopL2End.Value = activeuser.ExpiryDate.Value; } lisanslar.ViopL2Start = dtpViopL2Start.Value.Date; lisanslar.ViopL2End = dtpViopL2End.Value.Date; }
                if (chkVd2P.Checked || chkVd2PTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpVd2PEnd.Value.Date) { illegaltarih.AppendLine("VD2P"); dtpVd2PEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.Vd2PStart = dtpVd2PStart.Value.Date; lisanslar.Vd2PEnd = dtpVd2PEnd.Value.Date; }
                if (chkVGS.Checked || chkVGSTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpViopGSEnd.Value.Date) { illegaltarih.AppendLine("VIT"); dtpViopGSEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.ViopGSStart = dtpViopGSStart.Value.Date; lisanslar.ViopGSEnd = dtpViopGSEnd.Value.Date; }
                if (chkComex.Checked || chkKRMD1Tarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpKRMD1End.Value.Date) { illegaltarih.AppendLine("KRMD1"); dtpKRMD1End.Value = activeuser.ExpiryDate.Value; } lisanslar.KRMD1Start = dtpKRMD1Start.Value.Date; lisanslar.KRMD1End = dtpKRMD1End.Value.Date; }
                if (chkTL1.Checked || chkTL1Tarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpTahvilL1End.Value.Date) { illegaltarih.AppendLine("BD1"); dtpTahvilL1End.Value = activeuser.ExpiryDate.Value; } lisanslar.TahvilL1Start = dtpTahvilL1Start.Value.Date; lisanslar.TahvilL1End = dtpTahvilL1End.Value.Date; }
                if (chkTLP.Checked || chkTLPTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpTahvilLPEnd.Value.Date) { illegaltarih.AppendLine("BD1P"); dtpTahvilLPEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.TahvilLPStart = dtpTahvilLPStart.Value.Date; lisanslar.TahvilLPEnd = dtpTahvilLPEnd.Value.Date; }
                if (chkTL2.Checked || chkTL2Tarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpTahvilL2End.Value.Date) { illegaltarih.AppendLine("BD2"); dtpTahvilL2End.Value = activeuser.ExpiryDate.Value; } lisanslar.TahvilL2Start = dtpTahvilL2Start.Value.Date; lisanslar.TahvilL2End = dtpTahvilL2End.Value.Date; }
                if (chkAnPro.Checked || chkAnProTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpAnProEnd.Value.Date) { illegaltarih.AppendLine("AnalizPro"); dtpAnProEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.AnProStart = dtpAnProStart.Value.Date; lisanslar.AnProEnd = dtpAnProEnd.Value.Date; }
                //if (chkCME.Checked || chkCMETarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpCMEEnd.Value.Date) { illegaltarih.AppendLine("İdealGO"); dtpCMEEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.CMEStart = dtpCMEStart.Value.Date; lisanslar.CMEEnd = dtpCMEEnd.Value.Date; }
                //if (chkCMEM.Checked || chkCMEMTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpCMEMEnd.Value.Date) { illegaltarih.AppendLine("UserDLL"); dtpCMEMEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.CMEMStart = dtpCMEMStart.Value.Date; lisanslar.CMEMEnd = dtpCMEMEnd.Value.Date; }
                //if (chkTemelAnaliz.Checked || chkTemelAnalizTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpTemelAnalizEnd.Value.Date) { illegaltarih.AppendLine("Temel Analiz"); dtpTemelAnalizEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.TemelAnalizStart = dtpTemelAnalizStart.Value.Date; lisanslar.TemelAnalizEnd = dtpTemelAnalizEnd.Value.Date; }
                //if (chkBMK.Checked || chkBMKTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpBMKEnd.Value.Date) { illegaltarih.AppendLine("BMK"); dtpBMKEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.BMKStart = dtpBMKStart.Value.Date; lisanslar.BMKEnd = dtpBMKEnd.Value.Date; }
                //if (chkBMC.Checked || chkBMCTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpBMCEnd.Value.Date) { illegaltarih.AppendLine("BMCAPITAL"); dtpBMCEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.BMCStart = dtpBMCStart.Value.Date; lisanslar.BMCEnd = dtpBMCEnd.Value.Date; }
                //if (chkBarSistem.Checked || chkBarSistemTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpBarSistemEnd.Value.Date) { illegaltarih.AppendLine("Bar Sistem"); dtpBarSistemEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.BarSistemStart = dtpBarSistemStart.Value.Date; lisanslar.BarSistemEnd = dtpBarSistemEnd.Value.Date; }
                //if (chkFSystem.Checked || chkFSystemTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpFSystemEnd.Value.Date) { illegaltarih.AppendLine("F System"); dtpFSystemEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.FSystemStart = dtpFSystemStart.Value.Date; lisanslar.FSystemEnd = dtpFSystemEnd.Value.Date; }
                //if (chkPara.Checked || chkParaTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpParaEnd.Value.Date) { illegaltarih.AppendLine("PARA"); dtpParaEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.PARAStart = dtpParaStart.Value.Date; lisanslar.PARAEnd = dtpParaEnd.Value.Date; }
                //if (chkHISSEA.Checked || chkHISSEATarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpHISSEAEnd.Value.Date) { illegaltarih.AppendLine("HISSEANALIZ"); dtpHISSEAEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.HISSEAStart = dtpHISSEAStart.Value.Date; lisanslar.HISSEAEnd = dtpHISSEAEnd.Value.Date; }
                if (chkSentiL1.Checked || chkSentiL1Tarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpSentiL1End.Value.Date) { illegaltarih.AppendLine("SentiL1"); dtpSentiL1End.Value = activeuser.ExpiryDate.Value; } lisanslar.SentiL1Start = dtpSentiL1Start.Value.Date; lisanslar.SentiL1End = dtpSentiL1End.Value.Date; }
                if (chkSentiL2.Checked || chkSentiL2Tarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpSentiL2End.Value.Date) { illegaltarih.AppendLine("SentiL2"); dtpSentiL2End.Value = activeuser.ExpiryDate.Value; } lisanslar.SentiL2Start = dtpSentiL2Start.Value.Date; lisanslar.SentiL2End = dtpSentiL2End.Value.Date; }
                if (chkMKK.Checked || chkMkkTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpMkkEnd.Value.Date) { illegaltarih.AppendLine("MKK"); dtpMkkEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.MKKStart = dtpMkkStart.Value.Date; lisanslar.MKKEnd = dtpMkkEnd.Value.Date; }
                if (chkGKKUL.Checked || chkGkkulTarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpGkkulEnd.Value.Date) { illegaltarih.AppendLine("GKKUL"); dtpGkkulEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.GKKULStart = dtpGkkulStart.Value.Date; lisanslar.GKKULEnd = dtpGkkulEnd.Value.Date; }
                if (chkCME.Checked || chkCMETarih.Checked) { if (activeuser.ExpiryDate.Value.Date < dtpCMEEnd.Value.Date) { illegaltarih.AppendLine("CME"); dtpCMEEnd.Value = activeuser.ExpiryDate.Value; } lisanslar.CMEStart = dtpCMEStart.Value.Date; lisanslar.CMEEnd = dtpCMEEnd.Value.Date; }

                if (illegaltarih.ToString() != "")
                    if (MessageBox.Show(illegaltarih + "\nYukarıdaki Lisansların son tarihleri , ürün son tarihi ile aynı yapılmıştır.\nDevam edilsin mi?", "Lisans ürün tarih uyuşmazlığı", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        return;

                if (activeuser.LisansDurum.ProYetkiEnd != dtpProEnd.Value.Date && chkProTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PROEnd;"); }
                if (activeuser.LisansDurum.CepYetkiEnd != dtpCepEnd.Value.Date && chkCepTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("CEPEnd;"); }
                if (activeuser.LisansDurum.KRMD1End != dtpKRMD1End.Value.Date && chkKRMD1Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("KRMD1End;"); }
                //    if (activeuser.LisansDurum.RobotEnd != dtpROBOTEnd.Value.Date && chkRobotTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("RobotEnd;"); }
                if (activeuser.LisansDurum.PayL1End != dtpPayL1End.Value.Date && chkPayL1Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PayL1End;"); }
                if (activeuser.LisansDurum.PayLPEnd != dtpPayLPEnd.Value.Date && chkPayLPTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PayLPEnd;"); }
                if (activeuser.LisansDurum.PayL2End != dtpPayL2End.Value.Date && chkPayL2Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PayL2End;"); }
                if (activeuser.LisansDurum.Pd2PEnd != dtpPd2PEnd.Value.Date && chkPd2PTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("Pd2PEnd;"); }
                if (activeuser.LisansDurum.PayXEnd != dtpPayXEnd.Value.Date && chkPayXTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PayXEnd;"); }
                if (activeuser.LisansDurum.PayGSEnd != dtpPayGSEnd.Value.Date && chkPayGSTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PayGSEnd;"); }
                if (activeuser.LisansDurum.PayPiteEnd != dtpPITEEnd.Value.Date && chkPITETarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PayPiteEnd;"); }
                if (activeuser.LisansDurum.ViopL1End != dtpViopL1End.Value.Date && chkVL1Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("ViopL1End;"); }
                if (activeuser.LisansDurum.ViopLPEnd != dtpViopLPEnd.Value.Date && chkVLPTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("ViopLPEnd;"); }
                if (activeuser.LisansDurum.ViopL2End != dtpViopL2End.Value.Date && chkVL2Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("ViopL2End;"); }
                if (activeuser.LisansDurum.Vd2PEnd != dtpVd2PEnd.Value.Date && chkVd2PTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("Vd2PEnd;"); }
                if (activeuser.LisansDurum.ViopGSEnd != dtpViopGSEnd.Value.Date && chkVGSTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("ViopGSEnd;"); }
                if (activeuser.LisansDurum.TahvilL1End != dtpTahvilL1End.Value.Date && chkTL1Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("TahvilL1End;"); }
                if (activeuser.LisansDurum.TahvilLPEnd != dtpTahvilLPEnd.Value.Date && chkTLPTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("TahvilLPEnd;"); }
                if (activeuser.LisansDurum.TahvilL2End != dtpTahvilL2End.Value.Date && chkTL2Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("TahvilL2End;"); }
                if (activeuser.LisansDurum.AnProEnd != dtpAnProEnd.Value.Date && chkAnProTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("AnalizProEnd;"); }
                //if (activeuser.LisansDurum.CMEEnd != dtpCMEEnd.Value.Date && chkCMETarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("CMEEnd;"); }
                //if (activeuser.LisansDurum.CMEMEnd != dtpCMEMEnd.Value.Date && chkCMEMTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("CMEMEnd;"); }
                //if (activeuser.LisansDurum.TemelAnalizEnd != dtpTemelAnalizEnd.Value.Date && chkTemelAnalizTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("TemelAnalizEnd;"); }
                //if (activeuser.LisansDurum.BMKEnd != dtpBMKEnd.Value.Date && chkBMKTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("BMKEnd;"); }
                //if (activeuser.LisansDurum.BMCEnd != dtpBMCEnd.Value.Date && chkBMCTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("BMCEnd;"); }
                //if (activeuser.LisansDurum.BarSistemEnd != dtpBarSistemEnd.Value.Date && chkBarSistemTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("BarSistemEnd;"); }
                //if (activeuser.LisansDurum.FSystemEnd != dtpFSystemEnd.Value.Date && chkFSystemTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("FSystemEnd;"); }
                //if (activeuser.LisansDurum.PARAEnd != dtpParaEnd.Value.Date && chkParaTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("ParaEnd;"); }
                //if (activeuser.LisansDurum.HISSEAEnd != dtpHISSEAEnd.Value.Date && chkHISSEATarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("HISSEAEnd;"); }
                if (activeuser.LisansDurum.SentiL1End != dtpSentiL1End.Value.Date && chkSentiL1Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("SentiL1End;"); }
                if (activeuser.LisansDurum.SentiL2End != dtpSentiL2End.Value.Date && chkSentiL2Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("SentiL2End;"); }
                if (activeuser.LisansDurum.MKKEnd != dtpMkkEnd.Value.Date && chkMkkTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("MKKEnd;"); }
                if (activeuser.LisansDurum.GKKULEnd != dtpGkkulEnd.Value.Date && chkGkkulTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("GKKULEnd;"); }
                // if (activeuser.LisansDurum.TaramaEnd != dtpCMEEnd.Value.Date && chkCMETarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("TaramaEnd;"); }
                if (activeuser.LisansDurum.CMEEnd != dtpCMEEnd.Value.Date && chkCMETarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("CMEEnd;"); }

                if (activeuser.LisansDurum.ProYetkiStart != dtpProStart.Value.Date && chkProTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PROStart;"); }
                if (activeuser.LisansDurum.CepYetkiStart != dtpCepStart.Value.Date && chkCepTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("CEPStart;"); }
                if (activeuser.LisansDurum.KRMD1Start != dtpKRMD1Start.Value.Date && chkKRMD1Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("KRMD1Start;"); }
                //    if (activeuser.LisansDurum.RobotStart != dtpROBOTStart.Value.Date && chkRobotTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("RobotStart;"); }
                if (activeuser.LisansDurum.PayL1Start != dtpPayL1Start.Value.Date && chkPayL1Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PayL1Start;"); }
                if (activeuser.LisansDurum.PayLPStart != dtpPayLPStart.Value.Date && chkPayLPTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PayLPStart;"); }
                if (activeuser.LisansDurum.PayL2Start != dtpPayL2Start.Value.Date && chkPayL2Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PayL2Start;"); }
                if (activeuser.LisansDurum.Pd2PStart != dtpPd2PStart.Value.Date && chkPd2PTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("Pd2PStart;"); }
                if (activeuser.LisansDurum.PayXStart != dtpPayXStart.Value.Date && chkPayXTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PayXStart;"); }
                if (activeuser.LisansDurum.PayGSStart != dtpPayGSStart.Value.Date && chkPayGSTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PayGSStart;"); }
                if (activeuser.LisansDurum.PayPiteStart != dtpPITEStart.Value.Date && chkPITETarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("PayPiteStart;"); }
                if (activeuser.LisansDurum.ViopL1Start != dtpViopL1Start.Value.Date && chkVL1Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("ViopL1Start;"); }
                if (activeuser.LisansDurum.ViopLPStart != dtpViopLPStart.Value.Date && chkVLPTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("ViopLPStart;"); }
                if (activeuser.LisansDurum.ViopL2Start != dtpViopL2Start.Value.Date && chkVL2Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("ViopL2Start;"); }
                if (activeuser.LisansDurum.Vd2PStart != dtpVd2PStart.Value.Date && chkVd2PTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("Vd2PStart;"); }
                if (activeuser.LisansDurum.ViopGSStart != dtpViopGSStart.Value.Date && chkVGSTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("ViopGSStart;"); }
                if (activeuser.LisansDurum.TahvilL1Start != dtpTahvilL1Start.Value.Date && chkTL1Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("TahvilL1Start;"); }
                if (activeuser.LisansDurum.TahvilLPStart != dtpTahvilLPStart.Value.Date && chkTLPTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("TahvilLPStart;"); }
                if (activeuser.LisansDurum.TahvilL2Start != dtpTahvilL2Start.Value.Date && chkTL2Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("TahvilL2Start;"); }
                if (activeuser.LisansDurum.AnProStart != dtpAnProStart.Value.Date && chkAnProTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("AnalizProStart;"); }
                //if (activeuser.LisansDurum.CMEStart != dtpCMEStart.Value.Date && chkCMETarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("CMEStart;"); }
                //if (activeuser.LisansDurum.CMEMStart != dtpCMEMStart.Value.Date && chkCMEMTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("CMEMStart;"); }
                //if (activeuser.LisansDurum.TemelAnalizStart != dtpTemelAnalizStart.Value.Date && chkTemelAnalizTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("TemelAnalizStart;"); }
                //if (activeuser.LisansDurum.BMKStart != dtpBMKStart.Value.Date && chkBMKTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("BMKStart;"); }
                //if (activeuser.LisansDurum.BMCStart != dtpBMCStart.Value.Date && chkBMCTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("BMCStart;"); }
                //if (activeuser.LisansDurum.BarSistemStart != dtpBarSistemStart.Value.Date && chkBarSistemTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("BarSistemStart;"); }
                //if (activeuser.LisansDurum.FSystemStart != dtpFSystemStart.Value.Date && chkFSystemTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("FSystemStart;"); }
                //if (activeuser.LisansDurum.PARAStart != dtpParaStart.Value.Date && chkParaTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("ParaStart;"); }
                //if (activeuser.LisansDurum.HISSEAStart != dtpHISSEAStart.Value.Date && chkHISSEATarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("HISSEAStart;"); }
                if (activeuser.LisansDurum.SentiL1Start != dtpSentiL1Start.Value.Date && chkSentiL1Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("SentiL1Start;"); }
                if (activeuser.LisansDurum.SentiL2Start != dtpSentiL2Start.Value.Date && chkSentiL2Tarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("SentiL2Start;"); }
                if (activeuser.LisansDurum.MKKStart != dtpMkkStart.Value.Date && chkMkkTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("MKKStart;"); }
                if (activeuser.LisansDurum.GKKULStart != dtpGkkulStart.Value.Date && chkGkkulTarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("GKKULStart;"); }
                if (activeuser.LisansDurum.CMEStart != dtpCMEStart.Value.Date && chkCMETarih.Checked) { lisansdegistirme = true; tarihdegisen.Append("CMEStart;"); }


                #endregion
                #region Duzey1Kontrol
                if (yayinKontrol == "KAPALI" && lisanslar.YayinDurumu == true)
                {
                    if (lisanslar.PayL1 == true && lisanslar.PayLP == false)
                    {
                        lisanslar.PayL1 = false;
                        kaptianlar.Append("PayL1");
                        lisansdegistirme = true;
                    }
                    if (lisanslar.ViopL1 == true && lisanslar.ViopLP == false)
                    {
                        lisanslar.ViopL1 = false;
                        kaptianlar.Append("ViopL1");
                        lisansdegistirme = true;
                    }
                    if (lisanslar.TahvilL1 == true && lisanslar.TahvilLP == false)
                    {
                        lisanslar.TahvilL1 = false;
                        kaptianlar.Append("TahvilL1");
                        lisansdegistirme = true;
                    }
                }
                #endregion
                //if (lisansdegistirme)
                //{
                //    if (acilanlar.ToString() != "" || kaptianlar.ToString() != "" || tarihdegisen.ToString().Trim() != "")
                //    {
                //        if ((acilanlar.ToString() != "" && kaptianlar.ToString() != "") || (tarihdegisen.ToString().Trim() != ""))
                //        {
                //            ue.EventTypeId = 6;
                //            ue.KapatilanLisans = kaptianlar.ToString();
                //            ue.AcilanLisans = acilanlar.ToString();
                //            ue.LisansTarihDegisen = tarihdegisen.ToString();
                //        }
                //        else
                //        {
                //            if (acilanlar.ToString() != "")
                //            {
                //                ue.EventTypeId = 4;
                //                ue.AcilanLisans = acilanlar.ToString();
                //                ue.LisansTarihDegisen = tarihdegisen.ToString();
                //            }
                //            if (kaptianlar.ToString() != "")
                //            {
                //                ue.EventTypeId = 3;
                //                ue.KapatilanLisans = kaptianlar.ToString();
                //                ue.LisansTarihDegisen = tarihdegisen.ToString();
                //            }
                //        }
                //    }
                //}
                if (lisansdegistirme)
                {
                    if (acilanlar.Length > 0 || kaptianlar.Length > 0)
                    {
                        ue.EventTypeId = (acilanlar.Length > 0 && kaptianlar.Length > 0) ? 6
                                       : (acilanlar.Length > 0 ? 4 : 3);
                        ue.AcilanLisans = acilanlar.ToString();
                        ue.KapatilanLisans = kaptianlar.ToString();
                    }


                    if (tarihdegisen.Length > 0)
                    {
                        if (ue.EventId < 1) 
                            ue.EventTypeId = 6;
                        ue.LisansTarihDegisen = tarihdegisen.ToString();
                    }
                }
                #endregion

                crm.LisansDurums.InsertOnSubmit(lisanslar);
                crm.SubmitChanges();
                if (lisansdegistimi == true)
                {
                    activeuser.LisansDurum = lisanslar;
                    crm.SubmitChanges();
                    ue.SonLisandurumID = lisanslar.LisansDurumId;
                    if (ue.EventId < 1)
                        crm.UserEvents.InsertOnSubmit(ue);
                    crm.SubmitChanges();
                    formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + activeuser.UserID.ToString() + (char)3;
                    gonderildimi = true;
                }
                if (lisansdegistirme && ue.EventTypeId != null)
                {
                    activeuser.LisansDurum = lisanslar;
                    crm.SubmitChanges();
                    ue.SonLisandurumID = lisanslar.LisansDurumId;
                    if (ue.EventId < 1)
                        crm.UserEvents.InsertOnSubmit(ue);
                    crm.SubmitChanges();
                    formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + activeuser.UserID.ToString() + (char)3;
                    gonderildimi = true;
                }

                if (chkPro.Checked == false && chkProTarih.Checked == false) { if (activeuser.LisansDurum.ProYetkiStart != null) activeuser.LisansDurum.ProYetkiStart = null; if (activeuser.LisansDurum.ProYetkiEnd != null) activeuser.LisansDurum.ProYetkiEnd = null; }
                if (chkCep.Checked == false && chkCepTarih.Checked == false)
                {
                    if (activeuser.LisansDurum.CepYetkiStart != null)
                        activeuser.LisansDurum.CepYetkiStart = null;
                    if (activeuser.LisansDurum.CepYetkiEnd != null)
                        activeuser.LisansDurum.CepYetkiEnd = null;
                }
                //if (chkRobot.Checked == false && chkRobotTarih.Checked == false) { if (activeuser.LisansDurum.RobotStart != null) activeuser.LisansDurum.RobotStart = null; if (activeuser.LisansDurum.RobotEnd != null) activeuser.LisansDurum.RobotEnd = null; }
                if (chkPayL1.Checked == false && chkPayL1Tarih.Checked == false) { if (activeuser.LisansDurum.PayL1Start != null) activeuser.LisansDurum.PayL1Start = null; if (activeuser.LisansDurum.PayL1End != null) activeuser.LisansDurum.PayL1End = null; }
                if (chkPayLP.Checked == false && chkPayLPTarih.Checked == false) { if (activeuser.LisansDurum.PayLPStart != null) activeuser.LisansDurum.PayLPStart = null; if (activeuser.LisansDurum.PayLPEnd != null) activeuser.LisansDurum.PayLPEnd = null; }
                if (chkPayL2.Checked == false && chkPayL2Tarih.Checked == false) { if (activeuser.LisansDurum.PayL2Start != null) activeuser.LisansDurum.PayL2Start = null; if (activeuser.LisansDurum.PayL2End != null) activeuser.LisansDurum.PayL2End = null; }
                if (chkPd2P.Checked == false && chkPd2PTarih.Checked == false) { if (activeuser.LisansDurum.Pd2PStart != null) activeuser.LisansDurum.Pd2PStart = null; if (activeuser.LisansDurum.Pd2PEnd != null) activeuser.LisansDurum.Pd2PEnd = null; }
                if (chkPayX.Checked == false && chkPayXTarih.Checked == false)
                {
                    if (activeuser.LisansDurum.PayXStart != null)
                        activeuser.LisansDurum.PayXStart = null;
                    if (activeuser.LisansDurum.PayXEnd != null)
                        activeuser.LisansDurum.PayXEnd = null;
                }
                if (chkPayGS.Checked == false && chkPayGSTarih.Checked == false) { if (activeuser.LisansDurum.PayGSStart != null) activeuser.LisansDurum.PayGSStart = null; if (activeuser.LisansDurum.PayGSEnd != null) activeuser.LisansDurum.PayGSEnd = null; }
                if (chkPITE.Checked == false && chkPITETarih.Checked == false) { if (activeuser.LisansDurum.PayPiteStart != null) activeuser.LisansDurum.PayPiteStart = null; if (activeuser.LisansDurum.PayPiteEnd != null) activeuser.LisansDurum.PayPiteEnd = null; }
                if (chkVL1.Checked == false && chkVL1Tarih.Checked == false) { if (activeuser.LisansDurum.ViopL1Start != null) activeuser.LisansDurum.ViopL1Start = null; if (activeuser.LisansDurum.ViopL1End != null) activeuser.LisansDurum.ViopL1End = null; }
                if (chkVLP.Checked == false && chkVLPTarih.Checked == false) { if (activeuser.LisansDurum.ViopLPStart != null) activeuser.LisansDurum.ViopLPStart = null; if (activeuser.LisansDurum.ViopLPEnd != null) activeuser.LisansDurum.ViopLPEnd = null; }
                if (chkVL2.Checked == false && chkVL2Tarih.Checked == false) { if (activeuser.LisansDurum.ViopL2Start != null) activeuser.LisansDurum.ViopL2Start = null; if (activeuser.LisansDurum.ViopL2End != null) activeuser.LisansDurum.ViopL2End = null; }
                if (chkVd2P.Checked == false && chkVd2PTarih.Checked == false) { if (activeuser.LisansDurum.Vd2PStart != null) activeuser.LisansDurum.Vd2PStart = null; if (activeuser.LisansDurum.Vd2PEnd != null) activeuser.LisansDurum.Vd2PEnd = null; }
                if (chkVGS.Checked == false && chkVGSTarih.Checked == false) { if (activeuser.LisansDurum.ViopGSStart != null) activeuser.LisansDurum.ViopGSStart = null; if (activeuser.LisansDurum.ViopGSEnd != null) activeuser.LisansDurum.ViopGSEnd = null; }
                if (chkComex.Checked == false && chkKRMD1Tarih.Checked == false) { if (activeuser.LisansDurum.KRMD1Start != null) activeuser.LisansDurum.KRMD1Start = null; if (activeuser.LisansDurum.KRMD1End != null) activeuser.LisansDurum.KRMD1End = null; }
                if (chkTL1.Checked == false && chkTL1Tarih.Checked == false) { if (activeuser.LisansDurum.TahvilL1Start != null) activeuser.LisansDurum.TahvilL1Start = null; if (activeuser.LisansDurum.TahvilL1End != null) activeuser.LisansDurum.TahvilL1End = null; }
                if (chkTLP.Checked == false && chkTLPTarih.Checked == false) { if (activeuser.LisansDurum.TahvilLPStart != null) activeuser.LisansDurum.TahvilLPStart = null; if (activeuser.LisansDurum.TahvilLPEnd != null) activeuser.LisansDurum.TahvilLPEnd = null; }
                if (chkTL2.Checked == false && chkTL2Tarih.Checked == false) { if (activeuser.LisansDurum.TahvilL2Start != null) activeuser.LisansDurum.TahvilL2Start = null; if (activeuser.LisansDurum.TahvilL2End != null) activeuser.LisansDurum.TahvilL2End = null; }
                if (chkAnPro.Checked == false && chkAnProTarih.Checked == false) { if (activeuser.LisansDurum.AnProStart != null) activeuser.LisansDurum.AnProStart = null; if (activeuser.LisansDurum.AnProEnd != null) activeuser.LisansDurum.AnProEnd = null; }
                //if (chkCME.Checked == false && chkCMETarih.Checked == false) { if (activeuser.LisansDurum.CMEStart != null) activeuser.LisansDurum.CMEStart = null; if (activeuser.LisansDurum.CMEEnd != null) activeuser.LisansDurum.CMEEnd = null; }
                //if (chkCMEM.Checked == false && chkCMEMTarih.Checked == false) { if (activeuser.LisansDurum.CMEMStart != null) activeuser.LisansDurum.CMEMStart = null; if (activeuser.LisansDurum.CMEMEnd != null) activeuser.LisansDurum.CMEMEnd = null; }
                //if (chkTemelAnaliz.Checked == false && chkTemelAnalizTarih.Checked == false) { if (activeuser.LisansDurum.TemelAnalizStart != null) activeuser.LisansDurum.TemelAnalizStart = null; if (activeuser.LisansDurum.TemelAnalizEnd != null) activeuser.LisansDurum.TemelAnalizEnd = null; }
                //if (chkBMK.Checked == false && chkBMKTarih.Checked == false) { if (activeuser.LisansDurum.BMKStart != null) activeuser.LisansDurum.BMKStart = null; if (activeuser.LisansDurum.BMKEnd != null) activeuser.LisansDurum.BMKEnd = null; }
                //if (chkBMC.Checked == false && chkBMCTarih.Checked == false) { if (activeuser.LisansDurum.BMCStart != null) activeuser.LisansDurum.BMCStart = null; if (activeuser.LisansDurum.BMCEnd != null) activeuser.LisansDurum.BMCEnd = null; }
                //if (chkBarSistem.Checked == false && chkBarSistemTarih.Checked == false) { if (activeuser.LisansDurum.BarSistemStart != null) activeuser.LisansDurum.BarSistemStart = null; if (activeuser.LisansDurum.BarSistemEnd != null) activeuser.LisansDurum.BarSistemEnd = null; }
                //if (chkFSystem.Checked == false && chkFSystemTarih.Checked == false) { if (activeuser.LisansDurum.FSystemStart != null) activeuser.LisansDurum.FSystemStart = null; if (activeuser.LisansDurum.FSystemEnd != null) activeuser.LisansDurum.FSystemEnd = null; }
                //if (chkPara.Checked == false && chkParaTarih.Checked == false) { if (activeuser.LisansDurum.PARAStart != null) activeuser.LisansDurum.PARAStart = null; if (activeuser.LisansDurum.PARAEnd != null) activeuser.LisansDurum.PARAEnd = null; }
                //if (chkHISSEA.Checked == false && chkHISSEATarih.Checked == false) { if (activeuser.LisansDurum.HISSEAStart != null) activeuser.LisansDurum.HISSEAStart = null; if (activeuser.LisansDurum.HISSEAEnd != null) activeuser.LisansDurum.HISSEAEnd = null; }
                if (chkSentiL1.Checked == false && chkSentiL1Tarih.Checked == false) { if (activeuser.LisansDurum.SentiL1Start != null) activeuser.LisansDurum.SentiL1Start = null; if (activeuser.LisansDurum.SentiL1End != null) activeuser.LisansDurum.SentiL1End = null; }
                if (chkSentiL2.Checked == false && chkSentiL2Tarih.Checked == false) { if (activeuser.LisansDurum.SentiL2Start != null) activeuser.LisansDurum.SentiL2Start = null; if (activeuser.LisansDurum.SentiL2End != null) activeuser.LisansDurum.SentiL2End = null; }
                if (chkMKK.Checked == false && chkMkkTarih.Checked == false) { if (activeuser.LisansDurum.MKKStart != null) activeuser.LisansDurum.MKKStart = null; if (activeuser.LisansDurum.MKKEnd != null) activeuser.LisansDurum.MKKEnd = null; }
                if (chkGKKUL.Checked == false && chkGkkulTarih.Checked == false) { if (activeuser.LisansDurum.GKKULStart != null) activeuser.LisansDurum.GKKULStart = null; if (activeuser.LisansDurum.GKKULEnd != null) activeuser.LisansDurum.GKKULEnd = null; }
                if (chkCME.Checked == false && chkCMETarih.Checked == false) { if (activeuser.LisansDurum.CMEStart != null) activeuser.LisansDurum.CMEStart = null; if (activeuser.LisansDurum.CMEEnd != null) activeuser.LisansDurum.CMEEnd = null; }


                crm.SubmitChanges();

                MyTools.logyaz($"[KullaniciGuncelle] UserName:{activeuser.UserName} | Yapan:{formAdminAra.referance.ActiveCalisan.Ad} | Açılan:{acilanlar} | Kapatılan:{kaptianlar}");
                MessageBox.Show(activeuser.UserName + " Kullanıcısının bilgileri güncellendi");
                //if (adresdegistimi)
                //{
                //    MessageBox.Show(activeuser.UserName + " Adres bilgileride değişti");
                //}

                if (gonderildimi == false)
                {
                    ue.EventTypeId = 7; 
                    ue.AcilanLisans = "KullaniciBilgiGuncelleme";
                    ue.SonLisandurumID = activeuser.LisansDurumId;
                    crm.UserEvents.InsertOnSubmit(ue);
                    crm.SubmitChanges();
                    formAdminAra.referance.IPport.DataToSend = "SendUserInfo|" + activeuser.UserID.ToString() + (char)3;
                }
                #endregion
            }
            this.Close();
        }
        private bool BistPayi()
        {
            var bistpayi = false;

            #region Bist
            if (chkPayL1.Checked)
                bistpayi = true;
            if (chkPayL2.Checked)
                bistpayi = true;
            if (chkPd2P.Checked)
                bistpayi = true;
            if (chkPayLP.Checked)
                bistpayi = true;
            if (chkPayGS.Checked)
                bistpayi = true;
            if (chkPayX.Checked)
                bistpayi = true;
            if (chkPITE.Checked)
                bistpayi = true;
            if (chkVeriAnalitik.Checked)
                bistpayi = true;
            #endregion
            #region Viop

            if (chkVL1.Checked)
                bistpayi = true;
            if (chkVL2.Checked)
                bistpayi = true;
            if (chkVd2P.Checked)
                bistpayi = true;
            if (chkVLP.Checked)
                bistpayi = true;
            if (chkVGS.Checked)
                bistpayi = true;
            #endregion
            #region Tahvil
            if (chkTL1.Checked)
                bistpayi = true;
            if (chkTL2.Checked)
                bistpayi = true;
            if (chkTLP.Checked)
                bistpayi = true;


            #endregion

            return bistpayi;
        }

        private void formUserDetay_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (KaydetUpdate)
            {
                formAdminAra.referance.toplamguncelle = true;

                formAdminAra.referance.gridGuncelle();
            }
        }

        private void comboYayinDurumu_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboYayinDurumu.SelectedIndex == 0)
                comboYayinDurumu.BackColor = Color.LimeGreen;
            else if (comboYayinDurumu.SelectedIndex == 1)
            {
                comboYayinDurumu.BackColor = Color.Red;
            }
            if (yayinKontrol == "AÇIK" && yayinKontrol != comboYayinDurumu.SelectedItem.ToString())
            {
                tarihleriDegistir();
            }

            btnSave.Select();
        }

        private void chkPayL2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPayL2.Checked == true)
            {
                chkPayL1.Checked = true;
                chkPayLP.Checked = true;

            }
            if (chkPayL2.Checked == false)
            {

                chkPd2P.Checked = false;
            }
            tarihleriGizleGoster(chkPayL2, dtpPayL2Start, dtpPayL2End, chkPayL2Tarih);
        }

        private void chkPd2P_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPd2P.Checked == true)
            {

                chkPayL1.Checked = true;
                chkPayLP.Checked = true;
                chkPayL2.Checked = true;
            }

            tarihleriGizleGoster(chkPd2P, dtpPd2PStart, dtpPd2PEnd, chkPd2PTarih);
        }
        private void chkPayLP_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPayLP.Checked == true)
            {

                chkPayL1.Checked = true;

            }
            if (chkPayLP.Checked == false)
            {
                chkPayL1.Checked = false;
                chkPd2P.Checked = false;
                chkPayL2.Checked = false;

            }
            tarihleriGizleGoster(chkPayLP, dtpPayLPStart, dtpPayLPEnd, chkPayLPTarih);
        }

        private void chkVL2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVL2.Checked == true)
            {
                chkVL1.Checked = true;
                chkVLP.Checked = true;

            }
            if (chkVL2.Checked == false)
            {
                chkVd2P.Checked = false;
            }
            tarihleriGizleGoster(chkVL2, dtpViopL2Start, dtpViopL2End, chkVL2Tarih);
        }

        private void chkVd2P_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVd2P.Checked == true)
            {
                chkVL1.Checked = true;
                chkVLP.Checked = true;
                chkVL2.Checked = true;
            }

            tarihleriGizleGoster(chkVd2P, dtpVd2PStart, dtpVd2PEnd, chkVd2PTarih);
        }
        private void chkVLP_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVLP.Checked == true)
                chkVL1.Checked = true;


            if (chkVLP.Checked == false)
            {
                chkVL1.Checked = false;
                chkVL2.Checked = false;
                chkVd2P.Checked = false;
            }
            tarihleriGizleGoster(chkVLP, dtpViopLPStart, dtpViopLPEnd, chkVLPTarih);
        }

        private void chkTL2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTL2.Checked == true)
            {
                chkTL1.Checked = true;
                chkTLP.Checked = true;

            }
            if (chkTL2.Checked == false)
            {
                chkTL1.Checked = false;
                chkTLP.Checked = false;
            }
            tarihleriGizleGoster(chkTL2, dtpTahvilL2Start, dtpTahvilL2End, chkTL2Tarih);
        }

        private void chkTLP_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTLP.Checked == true)
                chkTL1.Checked = true;

            if (chkTLP.Checked == false)
                chkTL1.Checked = false;

            tarihleriGizleGoster(chkTLP, dtpTahvilLPStart, dtpTahvilLPEnd, chkTLPTarih);
        }

        private void txtPMTSno_Leave(object sender, EventArgs e)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                //comboKurumSube.Items.Clear();
                var subes = crm.KurumSubes.Where(x => x.MusteriNo == txtPMTSno.Text).OrderBy(x => x.SubeAdi).Select(x => x.SubeAdi).ToList();
                // comboKurumSube.Items.AddRange(subes.ToArray());
                //var mus = MusteriBul(txtPMTSno.Text);

                //if (mus != null)
                //{
                //    lblunvan.Text = mus.MusteriAdi;
                //}
                //else
                //{
                //    lblunvan.Text = "";
                //}

                //crmDFNDataContext crm = new crmDFNDataContext();
                //var sorgu = crm.Sozlesmelers.Where(x => x.MusteriNo == txtPMTSno.Text);
                //comboSozlesme.DataSource = sorgu;
                //comboSozlesme.DisplayMember = "SozlesmeNo";
                //comboSozlesme.ValueMember = "SozlesmeilID";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btnNots_Click(object sender, EventArgs e)
        {
            formUserNotscs frm = new formUserNotscs();
            frm.Tag = acticeitem;
            frm.ShowDialog();
        }

        private void chkVL1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVL1.Checked == false)
            {
                chkVL1.Checked = false;
                chkVL2.Checked = false;
                chkVd2P.Checked = false;
                chkVLP.Checked = false;
            }
            tarihleriGizleGoster(chkVL1, dtpViopL1Start, dtpViopL1End, chkVL1Tarih);
        }

        private void chkTL1_CheckedChanged(object sender, EventArgs e)
        {

            if (chkTL1.Checked == false)
            {
                chkTL1.Checked = false;
                chkTLP.Checked = false;
                chkTL2.Checked = false;
            }
        }

        private void chkPayL1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPayL1.Checked == false)
            {
                chkPayL1.Checked = false;
                chkPayLP.Checked = false;
                chkPayL2.Checked = false;
            }
            tarihleriGizleGoster(chkPayL1, dtpPayL1Start, dtpPayL1End, chkPayL1Tarih);
        }



        private void chkProTarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpProEnd.Visible = chkProTarih.Checked;
            dtpProStart.Visible = chkProTarih.Checked;
        }

        private void chkPro_CheckedChanged(object sender, EventArgs e)
        {
            tarihleriGizleGoster(chkPro, dtpProStart, dtpProEnd, chkProTarih);
        }
        private void TarihYazAyarla(bool lisans, DateTimePicker datepickerStart, DateTime? StartDateValue, DateTimePicker datepickerEnd, DateTime? EndDateValue, CheckBox chkTarih)
        {
            try
            {
                if ((EndDateValue != null && EndDateValue.Value > DateTime.Now) || lisans == true)
                {
                    chkTarih.Checked = true;
                    datepickerStart.Visible = true;
                    datepickerEnd.Visible = true;
                    datepickerStart.Value = StartDateValue.Value;
                    datepickerEnd.Value = EndDateValue.Value;
                }
                else
                {
                    chkTarih.Checked = false;
                    datepickerStart.Visible = false;
                    datepickerEnd.Visible = false;
                }
                //if (lisans)
                //{
                //    if (StartDateValue != null)
                //        datepickerStart.Value = StartDateValue.Value;
                //    if (EndDateValue != null)
                //        datepickerEnd.Value = EndDateValue.Value;

                //    //datepickerStart.Visible = true;
                //    //datepickerEnd.Visible = true;
                //}
                //else
                //{
                //    //datepickerStart.Visible = false;
                //    //datepickerEnd.Visible = false;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tarihleriDegistir()
        {
            dateTimeExpiry.Value = DateTime.Now;

            if (chkPro.Checked == true)
            {
                dtpProEnd.Value = DateTime.Now;
            }
            if (chkCep.Checked == true)
            {
                dtpCepEnd.Value = DateTime.Now;
            }
            if (chkComex.Checked == true)
            {
                dtpKRMD1End.Value = DateTime.Now;
            }
            if (chkPayL1.Checked == true)
            {
                dtpPayL1End.Value = DateTime.Now;
            }
            if (chkPayLP.Checked == true)
            {
                dtpPayLPEnd.Value = DateTime.Now;
            }
            if (chkPayL2.Checked == true)
            {
                dtpPayL2End.Value = DateTime.Now;
            }
            if (chkPd2P.Checked == true)
            {
                dtpPd2PEnd.Value = DateTime.Now;
            }
            if (chkVL1.Checked == true)
            {
                dtpViopL1End.Value = DateTime.Now;
            }
            if (chkVLP.Checked == true)
            {
                dtpViopLPEnd.Value = DateTime.Now;
            }
            if (chkVL2.Checked == true)
            {
                dtpViopL2End.Value = DateTime.Now;
            }
            if (chkVd2P.Checked == true)
            {
                dtpVd2PEnd.Value = DateTime.Now;
            }
            if (chkPayX.Checked == true)
            {
                dtpPayXEnd.Value = DateTime.Now;
            }
            if (chkPayGS.Checked == true)
            {
                dtpPayGSEnd.Value = DateTime.Now;
            }
            if (chkPITE.Checked == true)
            {
                dtpPITEEnd.Value = DateTime.Now;
            }
            if (chkVGS.Checked == true)
            {
                dtpViopGSEnd.Value = DateTime.Now;
            }
            if (chkTL1.Checked == true)
            {
                dtpTahvilL1End.Value = DateTime.Now;
            }
            if (chkTLP.Checked == true)
            {
                dtpTahvilLPEnd.Value = DateTime.Now;
            }
            if (chkTL2.Checked == true)
            {
                dtpTahvilL2End.Value = DateTime.Now;
            }
            if (chkAnPro.Checked == true)
            {
                dtpAnProEnd.Value = DateTime.Now;
            }
            if (chkMKK.Checked == true)
            {
                dtpMkkEnd.Value = DateTime.Now;
            }
            if (chkGKKUL.Checked == true)
            {
                dtpGkkulEnd.Value = DateTime.Now;
            }
            if (chkCME.Checked == true)
            {
                dtpCMEEnd.Value = DateTime.Now;
            }

            //if (chkCME.Checked == true)
            //{
            //    dtpCMEEnd.Value = DateTime.Now;
            //}
            //if (chkCMEM.Checked == true)
            //{
            //    dtpCMEMEnd.Value = DateTime.Now;
            //}
            //if (chkPara.Checked == true)
            //{
            //    dtpParaEnd.Value = DateTime.Now;
            //}
            //if (chkTemelAnaliz.Checked == true)
            //{
            //    dtpTemelAnalizEnd.Value = DateTime.Now;
            //}
            //if (chkBMK.Checked == true)
            //{
            //    dtpBMKEnd.Value = DateTime.Now;
            //}
            //if (chkBMC.Checked == true)
            //{
            //    dtpBMCEnd.Value = DateTime.Now;
            //}
            //if (chkHISSEA.Checked == true)
            //{
            //    dtpHISSEAEnd.Value = DateTime.Now;
            //}
            //if (chkBarSistem.Checked == true)
            //{
            //    dtpBarSistemEnd.Value = DateTime.Now;
            //}
            //if (chkFSystem.Checked == true)
            //{
            //    dtpFSystemEnd.Value = DateTime.Now;
            //}
            //if (chkSentiL1.Checked == true)
            //{
            //    dtpSentiL1End.Value = DateTime.Now;
            //}
            //if (chkSentiL2.Checked == true)
            //{
            //    dtpSentiL2End.Value = DateTime.Now;
            //}
        }
        private void tarihleriGizleGoster(CheckBox chkLisasn, DateTimePicker dtpStart, DateTimePicker dtpEnd, CheckBox chkTarih)
        {
            try
            {
                if (chkLisasn.Checked)
                {
                    // dtpEnd.Value = DateTime.Now._LastDayOfMonth().Date;
                    dtpStart.Visible = true;
                    dtpEnd.Visible = true;
                    chkTarih.Checked = true;
                    dtpEnd.Value = dateTimeExpiry.Value.Date;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void chkCep_CheckedChanged(object sender, EventArgs e)
        {
            tarihleriGizleGoster(chkCep, dtpCepStart, dtpCepEnd, chkCepTarih);
        }

        private void chkCepTarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpCepEnd.Visible = chkCepTarih.Checked;
            dtpCepStart.Visible = chkCepTarih.Checked;
        }

        private void chkComex_CheckedChanged(object sender, EventArgs e)
        {
            tarihleriGizleGoster(chkComex, dtpKRMD1Start, dtpKRMD1End, chkKRMD1Tarih);
        }

        private void chkKRMD1Tarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpKRMD1End.Visible = chkKRMD1Tarih.Checked;
            dtpKRMD1Start.Visible = chkKRMD1Tarih.Checked;
        }

        private void chkTL1Tarih_CheckedChanged(object sender, EventArgs e)
        {
            //dtpTahvilL1End.Visible = chkTL1Tarih.Checked;
            //dtpTahvilL1Start.Visible = chkTL1Tarih.Checked;
        }

        private void chkTL1PTarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpTahvilLPEnd.Visible = chkTLPTarih.Checked;
            dtpTahvilLPStart.Visible = chkTLPTarih.Checked;
        }

        private void chkTL2Tarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpTahvilL2End.Visible = chkTL2Tarih.Checked;
            dtpTahvilL2Start.Visible = chkTL2Tarih.Checked;
        }

        private void chkPayL1Tarih_CheckedChanged(object sender, EventArgs e)
        {
            //dtpPayL1Start.Visible = chkPayL1Tarih.Checked;
            //dtpPayL1End.Visible = chkPayL1Tarih.Checked;
        }

        private void chkPayL2Tarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpPayL2Start.Visible = chkPayL2Tarih.Checked;
            dtpPayL2End.Visible = chkPayL2Tarih.Checked;
        }
        private void chkPd2PTarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpPd2PStart.Visible = chkPd2PTarih.Checked;
            dtpPd2PEnd.Visible = chkPd2PTarih.Checked;


        }


        private void chkPayLPTarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpPayLPStart.Visible = chkPayLPTarih.Checked;
            dtpPayLPEnd.Visible = chkPayLPTarih.Checked;

            if (chkPayLPTarih.Checked == true)
            {
                chkPayL1Tarih.Checked = true;
            }
            if (chkPayLPTarih.Checked == false)
            {
                chkPayL1Tarih.Checked = false;
            }




        }

        private void chkPayX_CheckedChanged(object sender, EventArgs e)
        {
            tarihleriGizleGoster(chkPayX, dtpPayXStart, dtpPayXEnd, chkPayXTarih);
        }

        private void chkPayXTarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpPayXStart.Visible = chkPayXTarih.Checked;
            dtpPayXEnd.Visible = chkPayXTarih.Checked;
        }

        private void chkPayGS_CheckedChanged(object sender, EventArgs e)
        {
            tarihleriGizleGoster(chkPayGS, dtpPayGSStart, dtpPayGSEnd, chkPayGSTarih);
        }

        private void chkPayGSTarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpPayGSStart.Visible = chkPayGSTarih.Checked;
            dtpPayGSEnd.Visible = chkPayGSTarih.Checked;
        }

        private void chkPITE_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPITE.Checked == true)
            {
               // chkPayGS.Checked = true;
            }
            tarihleriGizleGoster(chkPITE, dtpPITEStart, dtpPITEEnd, chkPITETarih);
        }

        private void chkPITETarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpPITEStart.Visible = chkPITETarih.Checked;
            dtpPITEEnd.Visible = chkPITETarih.Checked;
        }

        private void chkMkkTarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpMkkStart.Visible = chkMkkTarih.Checked;
            dtpMkkEnd.Visible = chkMkkTarih.Checked;
        }

        private void chkGkkulTarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpGkkulStart.Visible = chkGkkulTarih.Checked;
            dtpGkkulEnd.Visible = chkGkkulTarih.Checked;
        }

        private void chkMkk_CheckedChanged(object sender, EventArgs e)
        {
            tarihleriGizleGoster(chkMKK, dtpMkkStart, dtpMkkEnd, chkMkkTarih);
        }

        private void chkGkkul_CheckedChanged(object sender, EventArgs e)
        {
            tarihleriGizleGoster(chkGKKUL, dtpGkkulStart, dtpGkkulEnd, chkGkkulTarih);

        }

        private void chkCMETarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpCMEStart.Visible = chkCMETarih.Checked;
            dtpCMEEnd.Visible = chkCMETarih.Checked;
        }

        private void chkCME_CheckedChanged(object sender, EventArgs e)
        {
            tarihleriGizleGoster(chkCME, dtpCMEStart, dtpCMEEnd, chkCMETarih);
        }

        private void chkVL1Tarih_CheckedChanged(object sender, EventArgs e)
        {
            //dtpViopL1Start.Visible = chkVL1Tarih.Checked;
            //dtpViopL1End.Visible = chkVL1Tarih.Checked;
        }

        private void chkVLPTarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpViopLPStart.Visible = chkVLPTarih.Checked;
            dtpViopLPEnd.Visible = chkVLPTarih.Checked;
            if (chkVLPTarih.Checked == true)
            {
                chkVL1Tarih.Checked = true;
            }
            if (chkVLPTarih.Checked == false)
            {
                chkVL1Tarih.Checked = false;
            }
        }

        private void chkVL2Tarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpViopL2Start.Visible = chkVL2Tarih.Checked;
            dtpViopL2End.Visible = chkVL2Tarih.Checked;
        }

        private void chkVd2PTarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpVd2PStart.Visible = chkVd2PTarih.Checked;
            dtpVd2PEnd.Visible = chkVd2PTarih.Checked;
        }
        private void chkVGS_CheckedChanged(object sender, EventArgs e)
        {
            tarihleriGizleGoster(chkVGS, dtpViopGSStart, dtpViopGSEnd, chkVGSTarih);
        }

        private void chkVGSTarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpViopGSStart.Visible = chkVGSTarih.Checked;
            dtpViopGSEnd.Visible = chkVGSTarih.Checked;
        }

        private void chkSentiL1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSentiL1.Checked == false)
            {
                chkSentiL2.Checked = false;
            }
            tarihleriGizleGoster(chkSentiL1, dtpSentiL1Start, dtpSentiL1End, chkSentiL1Tarih);
        }

        private void chkSentiL1Tarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpSentiL1Start.Visible = chkSentiL1Tarih.Checked;
            dtpSentiL1End.Visible = chkSentiL1Tarih.Checked;
        }

        private void chkSentiL2Tarih_CheckedChanged(object sender, EventArgs e)
        {
            dtpSentiL2Start.Visible = chkSentiL2Tarih.Checked;
            dtpSentiL2End.Visible = chkSentiL2Tarih.Checked;
        }

        private void chkSentiL2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSentiL2.Checked == true)
            {
                chkSentiL1.Checked = true;
            }
            tarihleriGizleGoster(chkSentiL2, dtpSentiL2Start, dtpSentiL2End, chkSentiL2Tarih);
        }

        private void chkAnPro_CheckedChanged(object sender, EventArgs e)
        {

            tarihleriGizleGoster(chkAnPro, dtpAnProStart, dtpAnProEnd, chkAnProTarih);
        }

        private void chkAnProTarih_CheckedChanged(object sender, EventArgs e)
        {

            dtpAnProStart.Visible = chkAnProTarih.Checked;
            dtpAnProEnd.Visible = chkAnProTarih.Checked;
        }

        private void btnTarihUygula_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkPro.Checked == true)
                    dtpProEnd.Value = dateTimeExpiry.Value;
                if (chkCep.Checked == true)
                    dtpCepEnd.Value = dateTimeExpiry.Value;
                if (chkPayL1.Checked == true)
                    dtpPayL1End.Value = dateTimeExpiry.Value;
                if (chkPayLP.Checked == true)
                    dtpPayLPEnd.Value = dateTimeExpiry.Value;
                if (chkPayL2.Checked == true)
                    dtpPayL2End.Value = dateTimeExpiry.Value;
                if (chkPd2P.Checked == true)
                    dtpPd2PEnd.Value = dateTimeExpiry.Value;
                if (chkPayGS.Checked == true)
                    dtpPayGSEnd.Value = dateTimeExpiry.Value;
                if (chkPITE.Checked == true)
                    dtpPITEEnd.Value = dateTimeExpiry.Value;
                if (chkPayX.Checked == true)
                    dtpPayXEnd.Value = dateTimeExpiry.Value;
                if (chkComex.Checked == true)
                    dtpKRMD1End.Value = dateTimeExpiry.Value;
                if (chkVL1.Checked == true)
                    dtpViopL1End.Value = dateTimeExpiry.Value;
                if (chkVLP.Checked == true)
                    dtpViopLPEnd.Value = dateTimeExpiry.Value;
                if (chkVL2.Checked == true)
                    dtpViopL2End.Value = dateTimeExpiry.Value;
                if (chkVd2P.Checked == true)
                    dtpVd2PEnd.Value = dateTimeExpiry.Value;
                if (chkVGS.Checked == true)
                    dtpViopGSEnd.Value = dateTimeExpiry.Value;
                if (chkTL1.Checked == true)
                    dtpTahvilL1End.Value = dateTimeExpiry.Value;
                if (chkTLP.Checked == true)
                    dtpTahvilLPEnd.Value = dateTimeExpiry.Value;
                if (chkTL2.Checked == true)
                    dtpTahvilL2End.Value = dateTimeExpiry.Value;
                if (chkSentiL1.Checked == true)
                    dtpSentiL1End.Value = dateTimeExpiry.Value;
                if (chkSentiL2.Checked == true)
                    dtpSentiL2End.Value = dateTimeExpiry.Value;
                if (chkAnPro.Checked == true)
                    dtpAnProEnd.Value = dateTimeExpiry.Value;
                if (chkMKK.Checked == true)
                    dtpMkkEnd.Value = dateTimeExpiry.Value;
                if (chkGKKUL.Checked == true)
                    dtpGkkulEnd.Value = dateTimeExpiry.Value;
                if (chkCME.Checked == true)
                    dtpCMEEnd.Value = dateTimeExpiry.Value;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void LisansDateChange(DateTimePicker dtc)
        {
            try
            {
                switch (dtc.Name)
                {
                    case "dtpPayLPEnd":
                        dtpPayL1End.Value = dtpPayLPEnd.Value;
                        break;
                    case "dtpPayL2End":
                        dtpPayL1End.Value = dtpPayL2End.Value;
                        dtpPayLPEnd.Value = dtpPayL2End.Value;
                        break;
                    case "dtpPd2PEnd":
                        dtpPayL1End.Value = dtpPd2PEnd.Value;
                        dtpPayLPEnd.Value = dtpPd2PEnd.Value;
                        dtpPayL2End.Value = dtpPd2PEnd.Value;
                        break;
                    case "dtpPayLPStart":
                        if (chkPayLP.Checked)
                            dtpPayL1Start.Value = dtpPayLPStart.Value;
                        break;
                    case "dtpPayL2Start":
                        if (chkPayL2.Checked)
                        {
                            dtpPayL1Start.Value = dtpPayL2Start.Value;
                            dtpPayLPStart.Value = dtpPayL2Start.Value;
                        }
                        break;
                    case "dtpPd2PStart":
                        if (chkPd2P.Checked)
                        {
                            dtpPayL1Start.Value = dtpPd2PStart.Value;
                            dtpPayLPStart.Value = dtpPd2PStart.Value;
                            dtpPayL2Start.Value = dtpPd2PStart.Value;
                        }
                        break;
                    case "dtpViopLPEnd":
                        dtpViopL1End.Value = dtpViopLPEnd.Value;

                        break;
                    case "dtpViopL2End":

                        dtpViopL1End.Value = dtpViopL2End.Value;
                        dtpViopLPEnd.Value = dtpViopL2End.Value;

                        break;
                    case "dtpVd2PEnd":

                        dtpViopL1End.Value = dtpVd2PEnd.Value;
                        dtpViopLPEnd.Value = dtpVd2PEnd.Value;
                        dtpViopL2End.Value = dtpVd2PEnd.Value;

                        break;
                    case "dtpViopLPStart":
                        if (chkVLP.Checked)
                        {
                            dtpViopL1Start.Value = dtpViopLPStart.Value;
                        }
                        break;
                    case "dtpViopL2Start":
                        if (chkVL2.Checked)
                        {
                            dtpViopL1Start.Value = dtpViopL2Start.Value;
                            dtpViopLPStart.Value = dtpViopL2Start.Value;
                        }
                        break;
                    case "dtpVd2PStart":
                        if (chkVd2P.Checked)
                        {
                            dtpViopL1Start.Value = dtpVd2PStart.Value;
                            dtpViopLPStart.Value = dtpVd2PStart.Value;
                            dtpViopL2Start.Value = dtpVd2PStart.Value;
                        }
                        break;
                    case "dtpTahvilLPEnd":
                        dtpTahvilL1End.Value = dtpTahvilLPEnd.Value;
                        break;
                    case "dtpTahvilL2End":
                        dtpTahvilL1End.Value = dtpTahvilL2End.Value;
                        dtpTahvilLPEnd.Value = dtpTahvilL2End.Value;
                        break;
                    case "dtpTahvilLPStart":
                        if (chkTLP.Checked)
                        {
                            dtpTahvilL1Start.Value = dtpTahvilLPStart.Value;
                        }
                        break;
                    case "dtpTahvilL2Start":
                        if (chkTL2.Checked)
                        {
                            dtpTahvilL1Start.Value = dtpTahvilL2Start.Value;
                            dtpTahvilLPStart.Value = dtpTahvilL2Start.Value;
                        }
                        break;
                    case "dtpPITEStart":
                        dtpPayGSStart.Value = dtpPITEStart.Value;
                        break;
                    case "dtpPITEEnd":
                        dtpPayGSEnd.Value = dtpPITEEnd.Value;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void dtpPayLPEnd_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }

        private void dtpPayL2End_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }

        private void dtpPd2PEnd_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }

        private void dtpPayLPStart_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }

        private void dtpPayL2Start_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }

        private void dtpPd2PStart_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }

        private void dtpViopLPEnd_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }

        private void dtpViopL2End_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }

        private void dtpViopLPStart_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }

        private void dtpViopL2Start_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }

        private void dtpVd2PEnd_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }

        private void dtpVd2PStart_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }

        private void dtpPITEStart_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }
        private void dtpPITEEnd_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }
        private void dtpTahvilLPStart_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }

        private void dtpTahvilL2Start_ValueChanged(object sender, EventArgs e)
        {
            LisansDateChange((DateTimePicker)sender);
        }


    }
}
