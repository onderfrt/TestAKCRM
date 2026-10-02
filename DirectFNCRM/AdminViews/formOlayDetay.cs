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
    public partial class formOlayDetay : Form
    {
        public formOlayDetay()
        {
            InitializeComponent();
           // this.Load += new System.EventHandler(this.formOlayDetay_Load);
        }

        public UserEvent activeitem = null;

        private void formOlayDetay_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Tag != null)
                {
                    activeitem = (UserEvent)this.Tag;

                    lblOlayTarih.Text = activeitem.EventTarih.ToString();
                    lblUsername.Text = activeitem.User.UserName;
                    lblAdSoyad.Text = activeitem.User.Name + " " + activeitem.User.Surname;
                    lblAciklama.Text = activeitem.User.Aciklama;
                    lblCalisan.Text = activeitem.Calisan.Ad + " " + activeitem.Calisan.Soyad;
                    txtAcilanlar.Text = activeitem.AcilanLisans;
                    txtkapananlar.Text = activeitem.KapatilanLisans;
                    txtDegisenler.Text = activeitem.LisansTarihDegisen;
                    lblOlayTip.Text = activeitem.EventType.EventName;
                    lblExpiryDate.Text = activeitem.User.ExpiryDate.ToString();

                    #region Lisanslar

                    chkyayinDurum.Checked = activeitem.LisansDurum.YayinDurumu;
                    chkPro.Checked = activeitem.LisansDurum.ProYetki;
                    chkCep.Checked = activeitem.LisansDurum.CepYetki;
                   // chkRobot.Checked = activeitem.LisansDurum.ROBOT;

                    chkPayL1.Checked = activeitem.LisansDurum.PayL1;
                    chkPayLP.Checked = activeitem.LisansDurum.PayLP;
                    chkPayL2.Checked = activeitem.LisansDurum.PayL2;
                    chkPayX.Checked = activeitem.LisansDurum.PayX;
                    chkPd2P.Checked = activeitem.LisansDurum.Pd2P;
                    chkPayGS.Checked = activeitem.LisansDurum.PayGS;
                    chkPITE.Checked = activeitem.LisansDurum.PITE;

                  //  chkVL1.Checked = activeitem.LisansDurum.ViopL1;
                    chkVLP.Checked = activeitem.LisansDurum.ViopLP;
                    chkVL2.Checked = activeitem.LisansDurum.ViopL2;
                    chkVD2P.Checked = activeitem.LisansDurum.Vd2P;
                    chkVGS.Checked = activeitem.LisansDurum.ViopGS;

                   // chkTL1.Checked = activeitem.LisansDurum.TahvilL1;
                    chkTLP.Checked = activeitem.LisansDurum.TahvilLP;
                    chkTL2.Checked = activeitem.LisansDurum.TahvilL2;

                    chkCME.Checked = activeitem.LisansDurum.CME;//idealgo
                  //  chkCMEM.Checked = activeitem.LisansDurum.CMEM; //UserDll

                  //  chkTemelAnaliz.Checked = activeitem.LisansDurum.TemelAnaliz;
                  //  chkBMK.Checked = activeitem.LisansDurum.BMK;
                  //  chkBMC.Checked = activeitem.LisansDurum.BMC;
                   // chkBarSistem.Checked = activeitem.LisansDurum.BarSistem;
                   // chkFSystem.Checked = activeitem.LisansDurum.FSystem;
                    chkKRMD1.Checked = activeitem.LisansDurum.COMEX;
                  //  chkPara.Checked = activeitem.LisansDurum.PARA;
                  //  chkHISSEA.Checked = activeitem.LisansDurum.HISSEA;
                  //  chkSentilL1.Checked = activeitem.LisansDurum.SentiL1;
                  //  chkSentiL2.Checked = activeitem.LisansDurum.SentiL2;
                  //  chkMagnus.Checked = activeitem.LisansDurum.MAGNUS;
                  //  chkOneriVer.Checked = activeitem.LisansDurum.ONERIVER;
                 //   chkDropCopy.Checked = activeitem.LisansDurum.DROPCOPY;
                 //   chkTurib.Checked = activeitem.LisansDurum.TURIB;
                 //   chkTarama.Checked = activeitem.LisansDurum.TARAMA;
                 //   chkPortalgo.Checked = activeitem.LisansDurum.PORTALGO;
                    chkMkk.Checked = activeitem.LisansDurum.MKK;
                    chkGkkul.Checked = activeitem.LisansDurum.GKKUL;
                  //  chkAlgCrypt.Checked = activeitem.LisansDurum.ALGOCRYPT;

                    #region StartDate

                    if (activeitem.LisansDurum.ProYetkiStart == null) dtpProYetkiStart.Visible = false; else dtpProYetkiStart.Value = activeitem.LisansDurum.ProYetkiStart.Value;
                    if (activeitem.LisansDurum.CepYetkiStart == null) dtpCepYetkiStart.Visible = false; else dtpCepYetkiStart.Value = activeitem.LisansDurum.CepYetkiStart.Value;
                    if (activeitem.LisansDurum.PayL1Start == null) dtpPayL1Start.Visible = false; else dtpPayL1Start.Value = activeitem.LisansDurum.PayL1Start.Value;
                    if (activeitem.LisansDurum.PayLPStart == null) dtpPayLPStart.Visible = false; else dtpPayLPStart.Value = activeitem.LisansDurum.PayLPStart.Value;
                    if (activeitem.LisansDurum.PayL2Start == null) dtpPayL2Start.Visible = false; else dtpPayL2Start.Value = activeitem.LisansDurum.PayL2Start.Value;
                    if (activeitem.LisansDurum.PayXStart == null) dtpPayXStart.Visible = false; else dtpPayXStart.Value = activeitem.LisansDurum.PayXStart.Value;
                    if (activeitem.LisansDurum.PayGSStart == null) dtpPayGSStart.Visible = false; else dtpPayGSStart.Value = activeitem.LisansDurum.PayGSStart.Value;
                    if (activeitem.LisansDurum.PayPiteStart == null) dtpPITEStart.Visible = false; else dtpPITEStart.Value = activeitem.LisansDurum.PayPiteStart.Value;
                    if (activeitem.LisansDurum.Pd2PStart == null) dtpPd2PStart.Visible = false; else dtpPd2PStart.Value = activeitem.LisansDurum.Pd2PStart.Value;

                   // if (activeitem.LisansDurum.ViopL1Start == null) dtpViopL1Start.Visible = false; else dtpViopL1Start.Value = activeitem.LisansDurum.ViopL1Start.Value;
                    if (activeitem.LisansDurum.ViopLPStart == null) dtpViopLPStart.Visible = false; else dtpViopLPStart.Value = activeitem.LisansDurum.ViopLPStart.Value;
                    if (activeitem.LisansDurum.ViopL2Start == null) dtpViopL2Start.Visible = false; else dtpViopL2Start.Value = activeitem.LisansDurum.ViopL2Start.Value;
                    if (activeitem.LisansDurum.Vd2PStart == null) dtpVd2PStart.Visible = false; else dtpVd2PStart.Value = activeitem.LisansDurum.Vd2PStart.Value;
                    if (activeitem.LisansDurum.ViopGSStart == null) dtpViopGSStart.Visible = false; else dtpViopGSStart.Value = activeitem.LisansDurum.ViopGSStart.Value;

                  //  if (activeitem.LisansDurum.TahvilL1Start == null) dtpTahvilL1Start.Visible = false; else dtpTahvilL1Start.Value = activeitem.LisansDurum.TahvilL1Start.Value;
                    if (activeitem.LisansDurum.TahvilLPStart == null) dtpTahvilLPStart.Visible = false; else dtpTahvilLPStart.Value = activeitem.LisansDurum.TahvilLPStart.Value;
                    if (activeitem.LisansDurum.TahvilL2Start == null) dtpTahvilL2Start.Visible = false; else dtpTahvilL2Start.Value = activeitem.LisansDurum.TahvilL2Start.Value;

                    if (activeitem.LisansDurum.CMEStart == null) dtpCMEStart.Visible = false; else dtpCMEStart.Value = activeitem.LisansDurum.CMEStart.Value;
                 //   if (activeitem.LisansDurum.CMEMStart == null) dtpCMEMStart.Visible = false; else dtpCMEMStart.Value = activeitem.LisansDurum.CMEMStart.Value;

                  //  if (activeitem.LisansDurum.TemelAnalizStart == null) dtpTemelAnalizStart.Visible = false; else dtpTemelAnalizStart.Value = activeitem.LisansDurum.TemelAnalizStart.Value;
                  //  if (activeitem.LisansDurum.BMKStart == null) dtpBMKStart.Visible = false; else dtpBMKStart.Value = activeitem.LisansDurum.BMKStart.Value;
                  //  if (activeitem.LisansDurum.RobotStart == null) dtpROBOTStart.Visible = false; else dtpROBOTStart.Value = activeitem.LisansDurum.RobotStart.Value;

                 //   if (activeitem.LisansDurum.BMCStart == null) dtpBMCStart.Visible = false; else dtpBMCStart.Value = activeitem.LisansDurum.BMCStart.Value;
                 //   if (activeitem.LisansDurum.BarSistemStart == null) dtpBarSistemStart.Visible = false; else dtpBarSistemStart.Value = activeitem.LisansDurum.BarSistemStart.Value;
                  //  if (activeitem.LisansDurum.FSystemStart == null) dtpFSystemStart.Visible = false; else dtpFSystemStart.Value = activeitem.LisansDurum.FSystemStart.Value;
                    if (activeitem.LisansDurum.KRMD1Start == null) dtpKRMD1Start.Visible = false; else dtpKRMD1Start.Value = activeitem.LisansDurum.KRMD1Start.Value;
                  //  if (activeitem.LisansDurum.PARAStart == null) dtpParaStart.Visible = false; else dtpParaStart.Value = activeitem.LisansDurum.PARAStart.Value;
                 //   if (activeitem.LisansDurum.HISSEAStart == null) dtpHISSEAStart.Visible = false; else dtpHISSEAStart.Value = activeitem.LisansDurum.HISSEAStart.Value;
                    //if (activeitem.LisansDurum.SentiL1Start == null) dtpSentiL1Start.Visible = false; else dtpSentiL1Start.Value = activeitem.LisansDurum.SentiL1Start.Value;
                    //if (activeitem.LisansDurum.SentiL2Start == null) dtpSentiL2Start.Visible = false; else dtpSentiL2Start.Value = activeitem.LisansDurum.SentiL2Start.Value;
                    //if (activeitem.LisansDurum.MgnsStart == null) dtpMagnusStart.Visible = false; else dtpMagnusStart.Value = activeitem.LisansDurum.MgnsStart.Value;
                    //if (activeitem.LisansDurum.OnrVrStart == null) dtpOneriVerStart.Visible = false; else dtpOneriVerStart.Value = activeitem.LisansDurum.OnrVrStart.Value;
                    //if (activeitem.LisansDurum.DrpCpyStart == null) dtpDropCopyStart.Visible = false; else dtpDropCopyStart.Value = activeitem.LisansDurum.DrpCpyStart.Value;
                    //if (activeitem.LisansDurum.TuribStart == null) dtpTuribStart.Visible = false; else dtpTuribStart.Value = activeitem.LisansDurum.TuribStart.Value;
                    //if (activeitem.LisansDurum.TaramaStart == null) dtpTaramaStart.Visible = false; else dtpTaramaStart.Value = activeitem.LisansDurum.TaramaStart.Value;
                    //if (activeitem.LisansDurum.PortalgoStart == null) dtpPortalgoStart.Visible = false; else dtpPortalgoStart.Value = activeitem.LisansDurum.PortalgoStart.Value;
                    if (activeitem.LisansDurum.MKKStart == null) dtpMkkStart.Visible = false; else dtpMkkStart.Value = activeitem.LisansDurum.MKKStart.Value;
                    if (activeitem.LisansDurum.GKKULStart == null) dtpGkkulStart.Visible = false; else dtpGkkulStart.Value = activeitem.LisansDurum.GKKULStart.Value;
                    //if (activeitem.LisansDurum.AlgoCryptStart == null) dtpAlgoCryptStart.Visible = false; else dtpAlgoCryptStart.Value = activeitem.LisansDurum.AlgoCryptStart.Value;

                    #endregion

                    #region Enddate
                    if (activeitem.LisansDurum.ProYetkiEnd == null) dtpProYetkiEnd.Visible = false; else dtpProYetkiEnd.Value = activeitem.LisansDurum.ProYetkiEnd.Value;
                    if (activeitem.LisansDurum.CepYetkiEnd == null) dtpCepYetkiEnd.Visible = false; else dtpCepYetkiEnd.Value = activeitem.LisansDurum.CepYetkiEnd.Value;
                    if (activeitem.LisansDurum.PayL1End == null) dtpPayL1End.Visible = false; else dtpPayL1End.Value = activeitem.LisansDurum.PayL1End.Value;
                    if (activeitem.LisansDurum.PayLPEnd == null) dtpPayLPEnd.Visible = false; else dtpPayLPEnd.Value = activeitem.LisansDurum.PayLPEnd.Value;
                    if (activeitem.LisansDurum.PayL2End == null) dtpPayL2End.Visible = false; else dtpPayL2End.Value = activeitem.LisansDurum.PayL2End.Value;
                    if (activeitem.LisansDurum.PayXEnd == null) dtpPayXEnd.Visible = false; else dtpPayXEnd.Value = activeitem.LisansDurum.PayXEnd.Value;
                    if (activeitem.LisansDurum.PayGSEnd == null) dtpPayGSEnd.Visible = false; else dtpPayGSEnd.Value = activeitem.LisansDurum.PayGSEnd.Value;
                    if (activeitem.LisansDurum.PayPiteEnd == null) dtpPITEEnd.Visible = false; else dtpPITEEnd.Value = activeitem.LisansDurum.PayPiteEnd.Value;
                    if (activeitem.LisansDurum.Pd2PEnd == null) dtpPd2PEnd.Visible = false; else dtpPd2PEnd.Value = activeitem.LisansDurum.Pd2PEnd.Value;

                  //  if (activeitem.LisansDurum.ViopL1End == null) dtpViopL1End.Visible = false; else dtpViopL1End.Value = activeitem.LisansDurum.ViopL1End.Value;
                    if (activeitem.LisansDurum.ViopLPEnd == null) dtpViopLPEnd.Visible = false; else dtpViopLPEnd.Value = activeitem.LisansDurum.ViopLPEnd.Value;
                    if (activeitem.LisansDurum.ViopL2End == null) dtpViopL2End.Visible = false; else dtpViopL2End.Value = activeitem.LisansDurum.ViopL2End.Value;
                    if (activeitem.LisansDurum.Vd2PEnd == null) dtpVd2PEnd.Visible = false; else dtpVd2PEnd.Value = activeitem.LisansDurum.Vd2PEnd.Value;
                    if (activeitem.LisansDurum.ViopGSEnd == null) dtpViopGSEnd.Visible = false; else dtpViopGSEnd.Value = activeitem.LisansDurum.ViopGSEnd.Value;

                  //  if (activeitem.LisansDurum.TahvilL1End == null) dtpTahvilL1End.Visible = false; else dtpTahvilL1End.Value = activeitem.LisansDurum.TahvilL1End.Value;
                    if (activeitem.LisansDurum.TahvilLPEnd == null) dtpTahvilLPEnd.Visible = false; else dtpTahvilLPEnd.Value = activeitem.LisansDurum.TahvilLPEnd.Value;
                    if (activeitem.LisansDurum.TahvilL2End == null) dtpTahvilL2End.Visible = false; else dtpTahvilL2End.Value = activeitem.LisansDurum.TahvilL2End.Value;

                    if (activeitem.LisansDurum.CMEEnd == null) dtpCMEEnd.Visible = false; else dtpCMEEnd.Value = activeitem.LisansDurum.CMEEnd.Value;
                  //  if (activeitem.LisansDurum.CMEMEnd == null) dtpCMEMEnd.Visible = false; else dtpCMEMEnd.Value = activeitem.LisansDurum.CMEMEnd.Value;

                    //if (activeitem.LisansDurum.TemelAnalizEnd == null) dtpTemelAnalizEnd.Visible = false; else dtpTemelAnalizEnd.Value = activeitem.LisansDurum.TemelAnalizEnd.Value;
                    //if (activeitem.LisansDurum.BMKEnd == null) dtpBMKEnd.Visible = false; else dtpBMKEnd.Value = activeitem.LisansDurum.BMKEnd.Value;
                    //if (activeitem.LisansDurum.RobotEnd == null) dtpROBOTEnd.Visible = false; else dtpROBOTEnd.Value = activeitem.LisansDurum.RobotEnd.Value;

                    //if (activeitem.LisansDurum.BMCEnd == null) dtpBMCEnd.Visible = false; else dtpBMCEnd.Value = activeitem.LisansDurum.BMCEnd.Value;
                    //if (activeitem.LisansDurum.BarSistemEnd == null) dtpBarSistemEnd.Visible = false; else dtpBarSistemEnd.Value = activeitem.LisansDurum.BarSistemEnd.Value;
                    //if (activeitem.LisansDurum.FSystemEnd == null) dtpFSystemEnd.Visible = false; else dtpFSystemEnd.Value = activeitem.LisansDurum.FSystemEnd.Value;

                    if (activeitem.LisansDurum.KRMD1End == null) dtpKRMD1End.Visible = false; else dtpKRMD1End.Value = activeitem.LisansDurum.KRMD1End.Value;
                    //if (activeitem.LisansDurum.PARAEnd == null) dtpParaEnd.Visible = false; else dtpParaEnd.Value = activeitem.LisansDurum.PARAEnd.Value;
                    //if (activeitem.LisansDurum.HISSEAEnd == null) dtpHISSEAEnd.Visible = false; else dtpHISSEAEnd.Value = activeitem.LisansDurum.HISSEAEnd.Value;
                    //if (activeitem.LisansDurum.SentiL1End == null) dtpSentiL1End.Visible = false; else dtpSentiL1End.Value = activeitem.LisansDurum.SentiL1End.Value;
                    //if (activeitem.LisansDurum.SentiL2End == null) dtpSentiL2End.Visible = false; else dtpSentiL2End.Value = activeitem.LisansDurum.SentiL2End.Value;
                    //if (activeitem.LisansDurum.MgnsEnd == null) dtpMagnusEnd.Visible = false; else dtpMagnusEnd.Value = activeitem.LisansDurum.MgnsEnd.Value;
                    //if (activeitem.LisansDurum.OnrVrEnd == null) dtpOneriVerEnd.Visible = false; else dtpOneriVerEnd.Value = activeitem.LisansDurum.OnrVrEnd.Value;
                    //if (activeitem.LisansDurum.DrpCpyEnd == null) dtpDropCopyEnd.Visible = false; else dtpDropCopyEnd.Value = activeitem.LisansDurum.DrpCpyEnd.Value;
                    //if (activeitem.LisansDurum.TuribEnd == null) dtpTuribEnd.Visible = false; else dtpTuribEnd.Value = activeitem.LisansDurum.TuribEnd.Value;
                    //if (activeitem.LisansDurum.TaramaEnd == null) dtpTaramaEnd.Visible = false; else dtpTaramaEnd.Value = activeitem.LisansDurum.TaramaEnd.Value;
                    //if (activeitem.LisansDurum.PortalgoEnd == null) dtpPortalgoEnd.Visible = false; else dtpPortalgoEnd.Value = activeitem.LisansDurum.PortalgoEnd.Value;
                    if (activeitem.LisansDurum.MKKEnd == null) dtpMkkEnd.Visible = false; else dtpMkkEnd.Value = activeitem.LisansDurum.MKKEnd.Value;
                    if (activeitem.LisansDurum.GKKULEnd == null) dtpGkkulEnd.Visible = false; else dtpGkkulEnd.Value = activeitem.LisansDurum.GKKULEnd.Value;
                    //if (activeitem.LisansDurum.AlgoCryptEnd == null) dtpAlgoCryptEnd.Visible = false; else dtpAlgoCryptEnd.Value = activeitem.LisansDurum.AlgoCryptEnd.Value;
                    #endregion

                    #endregion
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
