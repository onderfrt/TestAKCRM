using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectFNCRM.AdminViews
{
    public partial class formHesapMakinesi : Form
    {
        public formHesapMakinesi()
        {
            InitializeComponent();
            gridLisanHesaplaOzet.Rows.Add(25);
        }

        private void formHesapMakinesi_Load(object sender, EventArgs e)
        {
            crmDFNDataContext crm = new crmDFNDataContext();

            var usd = crm.ParaBirimis.FirstOrDefault(x => x.id == 2);
            var euro = crm.ParaBirimis.FirstOrDefault(x => x.id == 3);

           lblDolar.Text = "Dolar Kur :  "+ usd.Kur;
           lblEuro.Text = "Euro  Kur :  " + euro.Kur; 


            comboMensei.SelectedIndex = 0;
            comboProNonPro.SelectedIndex = 0;
            comboDonem.SelectedIndex = 0;

            #region Lisansfiyatlariyaz

            lisansFiyatlariToForm();

            #endregion

        }


        public void lisansFiyatlariToForm()
        {

            chkPayL1.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fpd1) : String.Format("{0:C}", MyTools.lisansfiyatlari.Ypd1);
            chkPayL1P.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fpd1p) : String.Format("{0:C}", MyTools.lisansfiyatlari.Ypd1p);
            chkPayL2.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fpd2) : String.Format("{0:C}", MyTools.lisansfiyatlari.Ypd2);
            chkPd2P.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fpd2p) : String.Format("{0:C}", MyTools.lisansfiyatlari.Ypd2p);

            chkPayX.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fend) : String.Format("{0:C}", MyTools.lisansfiyatlari.Yend);
            chkPayGS.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fpit) : String.Format("{0:C}", MyTools.lisansfiyatlari.Ypit);
            chkVA.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fpva) : String.Format("{0:C}", MyTools.lisansfiyatlari.Ypva);

            chkVL1.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fvl1) : String.Format("{0:C}", MyTools.lisansfiyatlari.Yvl1);
            chkVL1P.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fvl1p) : String.Format("{0:C}", MyTools.lisansfiyatlari.Yvl1p);
            chkVL2.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fvl2) : String.Format("{0:C}", MyTools.lisansfiyatlari.Yvl2);
            chkVd2P.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fvd2p) : String.Format("{0:C}", MyTools.lisansfiyatlari.Yvd2p);
            chkVGS.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fvit) : String.Format("{0:C}", MyTools.lisansfiyatlari.Yvit);

            chkTHVL1.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fbd1) : String.Format("{0:C}", MyTools.lisansfiyatlari.Ybd1);
            chkTHVL1P.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fbd1p) : String.Format("{0:C}", MyTools.lisansfiyatlari.Ybd1p);
            chkTHVL2.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.Fbd2) : String.Format("{0:C}", MyTools.lisansfiyatlari.Ybd2);
            
            chkAnPro.Text = (comboMensei.SelectedIndex == 0) ? String.Format("{0:C}", MyTools.lisansfiyatlari.FanPro) : String.Format("{0:C}", MyTools.lisansfiyatlari.YanPro);


            crmDFNDataContext crm = new crmDFNDataContext();

            var lisanslar = crm.LisansFiyatlaris.ToList();

            chkDJI.Text =((comboProNonPro.SelectedIndex == 0) ? String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "DJI").Fiyat) :
                String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "DJI").YurtDisiFiyat)) + "   " +
                lisanslar.FirstOrDefault(x => x.LisansKod == "DJI").ParaBirimi.ParaBirimiKod.ToString();

            chkXETRA.Text = ((comboProNonPro.SelectedIndex == 0) ? String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "XETRA").Fiyat) :
             String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "XETRA").YurtDisiFiyat)) + "   " +
             lisanslar.FirstOrDefault(x => x.LisansKod == "XETRA").ParaBirimi.ParaBirimiKod.ToString();

            chkSPI.Text = ((comboProNonPro.SelectedIndex == 0) ? String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "SPX").Fiyat) :
           String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "SPX").YurtDisiFiyat)) + "   " +
           lisanslar.FirstOrDefault(x => x.LisansKod == "SPX").ParaBirimi.ParaBirimiKod.ToString();

            chkSPOT.Text = ((comboProNonPro.SelectedIndex == 0) ? String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "Spot Paket").Fiyat) :
           String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "Spot Paket").YurtDisiFiyat)) + "   " +
           lisanslar.FirstOrDefault(x => x.LisansKod == "Spot Paket").ParaBirimi.ParaBirimiKod.ToString();


        chkCME.Text = ((comboProNonPro.SelectedIndex == 0) ? String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "CME").Fiyat) :
        String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "CME").YurtDisiFiyat)) + "   " +
        lisanslar.FirstOrDefault(x => x.LisansKod == "CME").ParaBirimi.ParaBirimiKod.ToString();

            chkCBOT.Text = ((comboProNonPro.SelectedIndex == 0) ? String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "CBOT").Fiyat) :
      String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "CBOT").YurtDisiFiyat)) + "   " +
      lisanslar.FirstOrDefault(x => x.LisansKod == "CBOT").ParaBirimi.ParaBirimiKod.ToString();

            chkCBOTM.Text = ((comboProNonPro.SelectedIndex == 0) ? String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "CBOTM").Fiyat) :
      String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "CBOTM").YurtDisiFiyat)) + "   " +
      lisanslar.FirstOrDefault(x => x.LisansKod == "CBOTM").ParaBirimi.ParaBirimiKod.ToString();

            chkCMEM.Text = ((comboProNonPro.SelectedIndex == 0) ? String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "CMEM").Fiyat) :
     String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "CMEM").YurtDisiFiyat)) + "   " +
     lisanslar.FirstOrDefault(x => x.LisansKod == "CMEM").ParaBirimi.ParaBirimiKod.ToString();

            chkEUREX.Text = ((comboProNonPro.SelectedIndex == 0) ? String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "EUREX").Fiyat) :
   String.Format("{0:N}", (decimal)lisanslar.FirstOrDefault(x => x.LisansKod == "EUREX").YurtDisiFiyat)) + "   " +
   lisanslar.FirstOrDefault(x => x.LisansKod == "EUREX").ParaBirimi.ParaBirimiKod.ToString();






        }


        public void hesapla()
        {
            decimal toplam = 0;
            decimal profiyat = 0;
            decimal cepfiyat = 0;
           
            if (txtCepFiyat.Text == "" && txtProFiyat.Text == "")
            {
                MessageBox.Show("Cep veya Pro Olarak en az bir ekran tipi için fiyat giriniz.");
                return;
            }

            if (txtCepFiyat.Text.Contains(","))
            {
                txtCepFiyat.Text = txtCepFiyat.Text.Replace(",",".");
            }


            if (txtProFiyat.Text.Contains(","))
            {
                txtProFiyat.Text = txtProFiyat.Text.Replace(",", ".");
            }

            try
            {
                if (txtProFiyat.Text == "")
                    profiyat = 0;
                else
                profiyat = decimal.Parse(txtProFiyat.Text);
            }
            catch 
            {

                MessageBox.Show("Pro Fiyat için geçerli bir fiyat giriniz");
                return;

            }

            try
            {
                if (txtCepFiyat.Text == "")
                    cepfiyat = 0;
                else
                    cepfiyat = decimal.Parse(txtCepFiyat.Text);
            }
            catch
            {

                MessageBox.Show("Cep Fiyat için geçerli bir fiyat giriniz");
                return;

            }

            toplam = profiyat + cepfiyat;

            #region Pay

            
            var paytanimi = "Pay Lisansı Yok";
            decimal paytutar = 0;
            decimal payendeks = 0;
            decimal verianalitik = 0;
            decimal pit = 0;
          


            if (chkPayL1.Checked == true && chkPayL1P.Checked == false && chkPayL2.Checked == false && chkPd2P.Checked == false)
            {
                paytanimi = "Hisse Yüzeysel";
                if (comboMensei.SelectedIndex==0)
                    paytutar = MyTools.lisansfiyatlari.Fpd1;
                else
                    paytutar = MyTools.lisansfiyatlari.Ypd1;
            }
            else if (chkPayL1.Checked == true && chkPayL1P.Checked == true && chkPayL2.Checked == false)
            {
                paytanimi = "Hisse Yüzeysel+";
                if (comboMensei.SelectedIndex == 0)
                    paytutar = MyTools.lisansfiyatlari.Fpd1p;
                else
                    paytutar = MyTools.lisansfiyatlari.Ypd1p;


            }
            else if (chkPayL1.Checked == true && chkPayL1P.Checked == true && chkPayL2.Checked == true)
            {
                paytanimi = "Hisse Derinlik";
                if (comboMensei.SelectedIndex == 0)
                    paytutar = MyTools.lisansfiyatlari.Fpd2;
                else
                    paytutar = MyTools.lisansfiyatlari.Ypd2;


            }
            else if (chkPayL1.Checked == true && chkPayL1P.Checked == true && chkPayL2.Checked == true && chkPd2P.Checked == true)
            {
                paytanimi = "Hisse Derinlik+";
                if (comboMensei.SelectedIndex == 0)
                    paytutar = MyTools.lisansfiyatlari.Fpd2p;
                else
                    paytutar = MyTools.lisansfiyatlari.Ypd2p;


            }

            toplam += paytutar;


            if (chkPayX.Checked == true)
            {
                if (comboMensei.SelectedIndex == 0)
                    payendeks = MyTools.lisansfiyatlari.Fend;
                else
                    payendeks = MyTools.lisansfiyatlari.Yend;

                toplam += payendeks;
            }



            if (chkPayGS.Checked == true)
            {
                if (comboMensei.SelectedIndex == 0)
                    pit = MyTools.lisansfiyatlari.Fpit;
                else
                    pit = MyTools.lisansfiyatlari.Ypit;
                toplam += pit;
            }

            if (chkVA.Checked== true)
            {
                if (comboMensei.SelectedIndex == 0)
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


            if (chkVL1.Checked == true && chkVL1P.Checked == false && chkVL2.Checked == false)
            {
                Vioptanimi = "Viop Yüzeysel";
                if (comboMensei.SelectedIndex == 0)
                    vioptutar = MyTools.lisansfiyatlari.Fvl1;
                else
                    vioptutar = MyTools.lisansfiyatlari.Yvl1;
            }
            else if (chkVL1.Checked == true && chkVL1P.Checked == true && chkVL2.Checked == false)
            {
                Vioptanimi = "Viop Yüzeysel+";
                if (comboMensei.SelectedIndex == 0)
                    vioptutar = MyTools.lisansfiyatlari.Fvl1p;
                else
                    vioptutar = MyTools.lisansfiyatlari.Yvl1p;

            }
            else if (chkVL1.Checked == true && chkVL1P.Checked == true && chkVL2.Checked == true)
            {
                Vioptanimi = "Viop Derinlik";
                if (comboMensei.SelectedIndex == 0)
                    vioptutar = MyTools.lisansfiyatlari.Fvl2;
                else
                    vioptutar = MyTools.lisansfiyatlari.Yvl2;

            }
            else if (chkVL1.Checked == true && chkVL1P.Checked == true && chkVL2.Checked == true && chkVd2P.Checked == true)
            {
                Vioptanimi = "Viop Derinlik+";
                if (comboMensei.SelectedIndex == 0)
                    vioptutar = MyTools.lisansfiyatlari.Fvl2p;
                else
                    vioptutar = MyTools.lisansfiyatlari.Yvl2p;

            }

            toplam += vioptutar;

            if (chkVGS.Checked== true)
                if (comboMensei.SelectedIndex == 0)
                    vit = MyTools.lisansfiyatlari.Fvit;
                else
                    vit = MyTools.lisansfiyatlari.Yvit;

            toplam += vit;
            #endregion
            #region Tahvil


            var Tahviltanimi = "Tahvil Lisansı Yok";
            decimal Tahviltutar = 0;


            if (chkTHVL1.Checked == true && chkTHVL1P.Checked == false && chkTHVL2.Checked == false)
            {
                Tahviltanimi = "Tahvil Yüzeysel";
                if (comboMensei.SelectedIndex == 0)
                    Tahviltutar = MyTools.lisansfiyatlari.Fbd1;
                else
                    Tahviltutar = MyTools.lisansfiyatlari.Ybd1;
            }
            else if (chkTHVL1.Checked == true && chkTHVL1P.Checked == true && chkTHVL2.Checked == false)
            {
                Tahviltanimi = "Tahvil Yüzeysel+";
                if (comboMensei.SelectedIndex == 0)
                    Tahviltutar = MyTools.lisansfiyatlari.Fbd1p;
                else
                    Tahviltutar = MyTools.lisansfiyatlari.Ybd1p;

            }
            else if (chkTHVL1.Checked == true && chkTHVL1P.Checked == true && chkTHVL2.Checked == true)
            {
                Tahviltanimi = "Tahvil Derinlik";
                if (comboMensei.SelectedIndex == 0)
                    Tahviltutar = MyTools.lisansfiyatlari.Fbd2;
                else
                    Tahviltutar = MyTools.lisansfiyatlari.Ybd2;

            }

            toplam += Tahviltutar;

            #endregion
            #region AnalizPro
            var anProTanimi = "Analiz Pro Lisansı Yok";
            decimal anProtutar= 0;


            if (chkAnPro.Checked == true)
            {
                anProTanimi = "Analiz Pro";
                if (comboMensei.SelectedIndex == 0)
                    anProtutar = MyTools.lisansfiyatlari.FanPro;
                else
                    anProtutar = MyTools.lisansfiyatlari.YanPro;
            }
            #endregion



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
            var yurtdisitanim = "Yurt Dışı Toplam";
            if (comboProNonPro.SelectedIndex!=-1)
            {
                #region spotpaketanaliz

                if (chkSPOT.Checked==true)
                {
                    if (comboProNonPro.SelectedIndex==1)
                        spotpaket = MyTools.lisansfiyatlari.SpotPro;
                    else
                        spotpaket = MyTools.lisansfiyatlari.SpotNonPro;
                }
                else
                {
                    if (chkDJI.Checked == true)
                    {
                        if (comboProNonPro.SelectedIndex == 1)
                            DJI = MyTools.lisansfiyatlari.DJIPro;
                        else
                            DJI = MyTools.lisansfiyatlari.DJINonPro;
                    }

                    if (chkSPI.Checked == true)
                    {
                        if (comboProNonPro.SelectedIndex == 1)
                            SPX = MyTools.lisansfiyatlari.SPIPro;
                        else
                            SPX = MyTools.lisansfiyatlari.SPINonPro;
                    }

                    if (chkXETRA.Checked == true)
                    {
                        if (comboProNonPro.SelectedIndex == 1)
                            XETRA = MyTools.lisansfiyatlari.XETRAPro;
                        else
                            XETRA = MyTools.lisansfiyatlari.XETRANonPro;
                    }

                }



                #endregion

                if (chkCBOT.Checked == true)
                {
                    if (comboProNonPro.SelectedIndex == 1)
                        CBOT = MyTools.lisansfiyatlari.CBOTPro;
                    else
                        CBOT = MyTools.lisansfiyatlari.CBOTNonPro;
                }

                if (chkCME.Checked == true)
                {
                    if (comboProNonPro.SelectedIndex == 1)
                        CME = MyTools.lisansfiyatlari.CMEPro;
                    else
                        CME = MyTools.lisansfiyatlari.CMENonPro;
                }

                if (chkCMEM.Checked == true)
                {
                    if (comboProNonPro.SelectedIndex == 1)
                        CMEM = MyTools.lisansfiyatlari.CMEMPro;
                    else
                        CMEM = MyTools.lisansfiyatlari.CMEMNonPro;
                }

                if (chkCBOTM.Checked == true)
                {
                    if (comboProNonPro.SelectedIndex == 1)
                        CBOTM = MyTools.lisansfiyatlari.CBOTMPro;
                    else
                        CBOTM = MyTools.lisansfiyatlari.CBOTMNonPro;
                }

                if (chkEUREX.Checked== true)
                {
                    if (comboProNonPro.SelectedIndex == 1)
                        EUREX = MyTools.lisansfiyatlari.EUREXPro;
                    else
                        EUREX = MyTools.lisansfiyatlari.EUREXNonPro;
                }



                yurtdisitutar = spotpaket + DJI + SPX + XETRA + CBOT + CME + CBOTM + CMEM + EUREX;

            }
            else yurtdisitanim = "ProNonPro Seçin";






            #endregion




            gridLisanHesaplaOzet.Rows[0].SetValues("iDeal", profiyat.ToString("0.00"));
            gridLisanHesaplaOzet.Rows[1].SetValues("Mobil", cepfiyat.ToString("0.00"));
            gridLisanHesaplaOzet.Rows[2].SetValues(paytanimi, paytutar.ToString("0.00"));
            gridLisanHesaplaOzet.Rows[3].SetValues("Borsa Endeks", payendeks.ToString("0.00"));
            gridLisanHesaplaOzet.Rows[4].SetValues("Pay Gün Sonu", pit.ToString("0.00"));
            gridLisanHesaplaOzet.Rows[5].SetValues("Veri Analitik", verianalitik.ToString("0.00"));
            gridLisanHesaplaOzet.Rows[6].SetValues(Vioptanimi, vioptutar.ToString("0.00"));
            gridLisanHesaplaOzet.Rows[7].SetValues("Viop Gün Sonu", vit.ToString("0.00"));
            gridLisanHesaplaOzet.Rows[8].SetValues(Tahviltanimi, Tahviltutar.ToString("0.00"));
            gridLisanHesaplaOzet.Rows[9].SetValues(anProTanimi, anProtutar.ToString("0.00"));
            gridLisanHesaplaOzet.Rows[10].SetValues(yurtdisitanim, String.Format("{0:C}", yurtdisitutar));
            gridLisanHesaplaOzet.Rows[11].SetValues("", "");
            toplam = toplam + yurtdisitutar;
            var kdvoran = 0.20m;

            if (comboDonem.SelectedIndex == 1)
                toplam = toplam * 3;
            else if (comboDonem.SelectedIndex == 2)
                toplam = toplam * 6;
            else if (comboDonem.SelectedIndex == 3)
                toplam = toplam * 9;
            else if (comboDonem.SelectedIndex == 4)
                toplam = toplam * 12;
            else if (comboDonem.SelectedIndex == 5)
                toplam = toplam * 24;


            var top = String.Format("{0:C}", toplam);
            var aratop = String.Format("{0:C}", toplam * kdvoran);
            var geneltop = String.Format("{0:C}", toplam + (toplam * kdvoran));
            gridLisanHesaplaOzet.Rows[12].SetValues("Toplam", top);

            gridLisanHesaplaOzet.Rows[13].SetValues("KDV", String.Format("{0:C}", aratop));
            gridLisanHesaplaOzet.Rows[14].SetValues("Genel Toplam", String.Format("{0:C}", geneltop));

            foreach (DataGridViewRow row in gridLisanHesaplaOzet.Rows)
            {

                row.Cells[0].Style.ForeColor = Color.Firebrick;
                row.Cells[0].Style.BackColor = Color.WhiteSmoke;

            }

            gridLisanHesaplaOzet.Rows[11].Cells[1].Selected = true;




        }

        private void chkPayL1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPayL1.Checked == false)
            {
                chkPayL1.Checked = false;
                chkPayL1P.Checked = false;
                chkPayL2.Checked = false;
            }

            hesapla();
        }

        private void chkPayL1P_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPayL1P.Checked == true)
                chkPayL1.Checked = true;

            if(chkPayL1P.Checked == false)
            {
                if (chkPayL2.Checked == true)
                {
                    chkPayL2.Checked = false;
                }
            }

            hesapla();
        }

        private void chkPayL2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPayL2.Checked == true)
            {
                chkPayL1.Checked = true;
                chkPayL1P.Checked = true;
            }

            hesapla();
        }
        private void chkPd2P_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPd2P.Checked == true)
            {
                chkPayL1.Checked = true;
                chkPayL1P.Checked = true;
                chkPayL2.Checked = true;
            }

            hesapla();
        }

        private void chkVL1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVL1.Checked == false)
            {
                chkVL1.Checked = false;
                chkVL2.Checked = false;
                chkVL1P.Checked = false;

            }

            hesapla();
        }

        private void chkVL1P_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVL1P.Checked == true)
                chkVL1.Checked = true;

            if (chkVL1P.Checked == false)
            {
                if (chkVL2.Checked == true)
                {
                    chkVL2.Checked = false;
                }
            }

            hesapla();

        }

        private void chkVL2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVL2.Checked == true)
            {
                chkVL1.Checked = true;
                chkVL1P.Checked = true;
            }

            hesapla();
        }

        private void chkVd2P_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVd2P.Checked == true)
            {
                chkVL1.Checked = true;
                chkVL1P.Checked = true;
                chkVL2.Checked = true;
            }

            hesapla();
        }
        private void chkTHVL1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTHVL1.Checked == false)
            {
                chkTHVL1.Checked = false;
                chkTHVL1P.Checked = false;
                chkTHVL2.Checked = false;

            }

            hesapla();
        }

        private void chkTHVL1P_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTHVL1P.Checked == true)
                chkTHVL1.Checked = true;

            if (chkTHVL1P.Checked == false)
            {
                if (chkTHVL2.Checked == true)
                {
                    chkTHVL2.Checked = false;
                }
            }

            hesapla();
        }

        private void chkTHVL2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTHVL2.Checked == true)
            {
                chkTHVL1.Checked = true;
                chkTHVL1P.Checked = true;
            }

            hesapla();
        }


        private void chkAnPro_CheckedChanged(object sender, EventArgs e)
        {
            chkAnPro.Checked = true;
        }
        private void comboMensei_SelectedIndexChanged(object sender, EventArgs e)
        {
            lisansFiyatlariToForm();
            hesapla();
        }

        private void comboProNonPro_SelectedIndexChanged(object sender, EventArgs e)
        {
            lisansFiyatlariToForm();
            hesapla();
        }

        private void txtProFiyat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                hesapla();

            }
        }

        private void txtCepFiyat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                hesapla();

            }
        }

        private void chkPayX_CheckedChanged(object sender, EventArgs e)
        {
            hesapla();
        }

        private void chkPayGS_CheckedChanged(object sender, EventArgs e)
        {
            hesapla();
        }

        private void chkVA_CheckedChanged(object sender, EventArgs e)
        {
            hesapla();
        }

        private void chkVGS_CheckedChanged(object sender, EventArgs e)
        {
            hesapla();
        }

        private void chkDJI_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDJI.Checked == true)
            {
                chkSPOT.Checked = false;
            }
            hesapla();
        }

        private void chkXETRA_CheckedChanged(object sender, EventArgs e)
        {
            if (chkXETRA.Checked == true)
            {
                chkSPOT.Checked = false;
            }
            hesapla();
        }

        private void chkSPI_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSPI.Checked == true)
            {
                chkSPOT.Checked = false;
            }
            hesapla();
        }

        private void chkSPOT_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSPOT.Checked == true)
            {
                chkSPI.Checked = false;
                chkDJI.Checked = false;
                chkXETRA.Checked = false;

            }

            hesapla();
        }

        private void chkCBOT_CheckedChanged(object sender, EventArgs e)
        {
            hesapla();
        }

        private void chkCME_CheckedChanged(object sender, EventArgs e)
        {
           

            hesapla();
        }

        private void chkCBOTM_CheckedChanged(object sender, EventArgs e)
        {
            hesapla();
        }

        private void chkEUREX_CheckedChanged(object sender, EventArgs e)
        {
            hesapla();
        }

        private void chkCMEM_CheckedChanged(object sender, EventArgs e)
        {
            hesapla();
        }

        private void comboDonem_SelectedIndexChanged(object sender, EventArgs e)
        {
            hesapla();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

            chkPayL1.Checked = false;
            chkPayL1P.Checked = false;
            chkPayL2.Checked = false;
            chkPd2P.Checked = false;
            chkPayGS.Checked = false;
            chkPayX.Checked = false;
            chkVA.Checked = false;
            chkVL1.Checked = false;
            chkVL1P.Checked = false;
            chkVL2.Checked = false;
            chkVd2P.Checked = false;
            chkVGS.Checked = false;
            chkTHVL1.Checked = false;
            chkTHVL1P.Checked = false;
            chkTHVL2.Checked = false;
            chkAnPro.Checked = false; 
            chkDJI.Checked = false;
            chkSPI.Checked = false;
            chkXETRA.Checked = false;
            chkSPOT.Checked = false;
            chkCBOT.Checked = false;
            chkCBOTM.Checked = false;
            chkCME.Checked = false;
            chkCMEM.Checked = false;
            chkEUREX.Checked = false;
            hesapla();




        }

    }
}
