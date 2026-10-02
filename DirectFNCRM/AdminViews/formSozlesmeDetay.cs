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
    public partial class formSozlesmeDetay : Form
    {
        public formSozlesmeDetay()
        {
            InitializeComponent();
            gridDurumOzet.Rows.Add(11);
        }
        public int activeitem = 0;
        public static IQueryable<User> sorgu;
        private void formSozlesmeDetay_Load(object sender, EventArgs e)
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            activeitem = (int)this.Tag;

            var musterino = crm.Sozlesmelers.FirstOrDefault(x => x.SozlesmeilID == activeitem).MusteriNo;
            var museri = crm.Musterilers.FirstOrDefault(x => x.MusteriNo == musterino);

            var ekranlar = crm.Users.Where(x => x.SozlesmeID == activeitem && x.LisansDurum.YayinDurumu == true && x.Status.StatusAdi!="SRV");

            

            
            
            dgEkran.DataSource = ekranlar.Select(x => new { Ad_Soyad = x.Name + " " + x.Surname, x.Sozlesmeler.SozlesmeNo,x.ProductType,x.UserName, x.UserID});

            var ekransayisi = ekranlar.ToList().Count;
            lblSozlesmeSayisi.Text = ekransayisi.ToString();

            lblunvan.Text = museri.MusteriAdi;
            lblMusterino.Text = museri.MusteriNo;

            sorgu = ekranlar;

            Hesapla();

        }

        public void Hesapla()
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            var soz = crm.Sozlesmelers.FirstOrDefault(x => x.SozlesmeilID == activeitem);

            decimal projeBorsaTutar = 0;

            var projeler = crm.Projelers.Where(x => x.SozlesmeID == activeitem && x.yayinDurum == true );   
            var projesayisi = projeler.ToList().Count;
            decimal projetutar = 0;
            foreach (var p in projeler)
            {
                if (soz.ParaBirimiID != 1)
                {
                    projetutar += (decimal)(p.Fiyat * soz.ParaBirimi.Kur);
                }
                else
                {
                    projetutar +=(decimal) p.Fiyat;
                }


                if (p.BorsaLisansId != null)
                {
                    if (p.BorsaAltLisansar.GKULD1P == true)
                        projeBorsaTutar += (decimal)MyTools.lisansfiyatlari.GKULD1P;
                    if (p.BorsaAltLisansar.GKULD2 == true)
                        projeBorsaTutar += (decimal)MyTools.lisansfiyatlari.GKULD2;
                    if (p.BorsaAltLisansar.GKULEND == true)
                        projeBorsaTutar += (decimal)MyTools.lisansfiyatlari.GKULEND;
                    if (p.BorsaAltLisansar.GKULPVA == true)
                        projeBorsaTutar += (decimal)MyTools.lisansfiyatlari.GKULPVA;
                    if (p.BorsaAltLisansar.GUYED1P == true)
                        projeBorsaTutar += (decimal)MyTools.lisansfiyatlari.GUYED1P;
                    if (p.BorsaAltLisansar.GUYED2 == true)
                        projeBorsaTutar += (decimal)MyTools.lisansfiyatlari.GUYED2;
                    if (p.BorsaAltLisansar.GUYEEND == true)
                        projeBorsaTutar += (decimal)MyTools.lisansfiyatlari.GUYEEND;
                    if (p.BorsaAltLisansar.GUYEPVA == true)
                        projeBorsaTutar += (decimal)MyTools.lisansfiyatlari.GUYEPVA;

                }
            }


            decimal BorsaLisansToplam = 0;

            var Cep = 0;//sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true).Count();
            var pro = 0;//sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ProYetki == true).Count();
            var robot = 0;// sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ROBOT == true).Count();           
            var PayYuzeysel = 0;//sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false).Count();
            var PayPlus = 0;// sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == true).Count();
            var PayDerinlik = 0;// sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true).Count();
            var PayDerinlikPlus = 0;// sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true).Count();
            var PayEndeks = 0;//sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayX == true).Count();
            var PayGs = 0;//sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayGS == true).Count();

            var analitik = 0;//sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.VeriAnalitik == true).Count();

            var viopYuzeysel = 0;//sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false).Count();
            var viopPlus = 0;// sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == true).Count();
            var viopDerinlik = 0;// sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true).Count();
            var viopDerinlikPlus = 0;// sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true).Count();
            var viopGS = 0;//sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();

            var TahvilYuzeysel = 0;//sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false).Count();
            var TahvilPlus = 0;//sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == true).Count();
            var TahvilDerinlik = 0;//sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == true && x.LisansDurum.TahvilLP == true).Count();
            var AnalizPro = 0;

            decimal Cep_t = 0;
            decimal pro_t = 0;
            decimal PayYuzeysel_t = 0;
            decimal PayPlus_t = 0;
            decimal PayDerinlik_t = 0;
            decimal PayDerinlikPlus_t = 0;
            decimal PayEndeks_t = 0;
            decimal PayGs_t = 0;
            decimal analitik_t = 0;
            decimal viopYuzeysel_t = 0;
            decimal viopPlus_t = 0;
            decimal viopDerinlik_t = 0;
            decimal viopDerinlikPlus_t = 0;
            decimal viopGS_t = 0;
            decimal TahvilYuzeysel_t = 0;
            decimal TahvilPlus_t = 0;
            decimal TahvilDerinlik_t = 0;
            decimal AnalizPro_t = 0;
            



            decimal yutdiditoplam = 0;
            decimal spotpaket_tutar = 0;
            decimal DJI_tutar = 0;
            decimal SPI_tutar = 0;
            decimal XETRA_tutar = 0;

            decimal CBOT_tutar = 0;
            decimal CME_tutar = 0;

            decimal CMEM_tutar = 0;
            decimal CBOTM_tutar = 0;
            decimal EUREX_tutar = 0;

            decimal spotpaket = 0;
            decimal DJI = 0;
            decimal SPI = 0;
            decimal XETRA = 0;

            decimal CBOT = 0;
            decimal CME = 0;

            decimal CMEM = 0;
            decimal CBOTM = 0;
            decimal EUREX = 0;


            foreach (var item in sorgu)
            {

               

                #region YurtDisi
                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.DJI == true && item.LisansDurum.SPI == true && item.LisansDurum.XETRA == true)
                {
                    spotpaket++;
                    if (item.ProNonPro == false)
                        spotpaket_tutar = MyTools.lisansfiyatlari.SpotNonPro * spotpaket;
                    else
                        spotpaket_tutar = MyTools.lisansfiyatlari.SpotPro * spotpaket;
                }
                else
                {
                    if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.DJI == true)
                    {
                        DJI++;
                        if (item.ProNonPro == false)
                            DJI_tutar = MyTools.lisansfiyatlari.DJINonPro * DJI;
                        else
                            DJI_tutar = MyTools.lisansfiyatlari.DJIPro * DJI;
                    }
                    if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.SPI == true)
                    {
                        SPI++;
                        if (item.ProNonPro == false)
                            SPI_tutar = MyTools.lisansfiyatlari.SPINonPro * SPI;
                        else
                            SPI_tutar = MyTools.lisansfiyatlari.SPIPro * SPI;
                    }

                    if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.XETRA == true)
                    {
                        XETRA++;
                        if (item.ProNonPro == false)
                            XETRA_tutar = MyTools.lisansfiyatlari.XETRANonPro * XETRA;
                        else
                            XETRA_tutar = MyTools.lisansfiyatlari.XETRAPro * XETRA;
                    }
                }




                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.CBOT == true)
                {
                    CBOT++;
                    if (item.ProNonPro == false)
                        CBOT_tutar = MyTools.lisansfiyatlari.CBOTNonPro * CBOT;
                    else
                        CBOT_tutar = MyTools.lisansfiyatlari.CBOTPro * CBOT;
                }


                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.CME == true)
                {
                    CME++;
                    if (item.ProNonPro == false)
                        CME_tutar = MyTools.lisansfiyatlari.CMENonPro * CME;
                    else
                        CME_tutar = MyTools.lisansfiyatlari.CMEPro * CME;
                }



                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.CBOTM == true)
                {
                    CBOTM++;
                    if (item.ProNonPro == false)
                        CBOTM_tutar = MyTools.lisansfiyatlari.CBOTMNonPro * CBOTM;
                    else
                        CBOTM_tutar = MyTools.lisansfiyatlari.CBOTMPro * CBOTM;
                }


                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.CMEM == true)
                {
                    CMEM++;
                    if (item.ProNonPro == false)
                        CMEM_tutar = MyTools.lisansfiyatlari.CMEMNonPro * CMEM;
                    else
                        CMEM_tutar = MyTools.lisansfiyatlari.CMEMPro * CMEM;
                }


                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.EUREX == true)
                {
                    EUREX++;
                    if (item.ProNonPro == false)
                        EUREX_tutar = MyTools.lisansfiyatlari.EUREXNonPro * EUREX;
                    else
                        EUREX_tutar = MyTools.lisansfiyatlari.EUREXPro * EUREX;
                }

                #endregion

                #region ProCep

                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.ProYetki == true )
                {
                    pro++;
                    if(soz.ParaBirimiID==1)
                    pro_t = (decimal)soz.ProFiyat*pro;
                    else
                        pro_t = (decimal)soz.ProFiyat*(decimal)soz.ParaBirimi.Kur * pro;

                }


                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.CepYetki == true)
                {
                    Cep++;
                 
                    if (soz.ParaBirimiID == 1)
                        Cep_t = (decimal)soz.CepFiyat * Cep;
                    else
                        Cep_t = (decimal)soz.CepFiyat * (decimal)soz.ParaBirimi.Kur * Cep;

                }
                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.ROBOT == true)
                {
                    robot++;
               

                }

                #endregion


                #region BorsaPay
                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.PayL1== true && item.LisansDurum.PayL2 == false && item.LisansDurum.PayLP == false && item.LisansDurum.Pd2P == false)
                {
                    PayYuzeysel++;
                    if (item.MusteriMenseiID == 1)
                        PayYuzeysel_t = MyTools.lisansfiyatlari.Fpd1 * PayYuzeysel;
                    else
                        PayYuzeysel_t = MyTools.lisansfiyatlari.Ypd1 * PayYuzeysel;
                }

                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.PayL1 == true && item.LisansDurum.PayL2 == true && item.LisansDurum.PayLP == true && item.LisansDurum.Pd2P == false)
                {
                    PayDerinlik++;
                    if (item.MusteriMenseiID == 1)
                        PayDerinlik_t = MyTools.lisansfiyatlari.Fpd2 * PayDerinlik;
                    else
                        PayDerinlik_t = MyTools.lisansfiyatlari.Ypd2 * PayDerinlik;
                }


                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.PayL1 == true && item.LisansDurum.PayL2 == false && item.LisansDurum.PayLP == true && item.LisansDurum.Pd2P == false)
                {
                    PayPlus++;
                    if (item.MusteriMenseiID == 1)
                        PayPlus_t = MyTools.lisansfiyatlari.Fpd1p * PayPlus;
                    else
                        PayPlus_t = MyTools.lisansfiyatlari.Ypd1p * PayPlus;
                }

                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.PayL1 == true && item.LisansDurum.PayL2 == false && item.LisansDurum.PayLP == true && item.LisansDurum.Pd2P == true)
                {
                    PayPlus++;
                    if (item.MusteriMenseiID == 1)
                        PayPlus_t = MyTools.lisansfiyatlari.Fpd2p * PayDerinlikPlus;
                    else
                        PayPlus_t = MyTools.lisansfiyatlari.Ypd2p * PayDerinlikPlus;
                }


                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.PayX == true)
                {
                    PayEndeks++;
                    if (item.MusteriMenseiID == 1)
                        PayEndeks_t = MyTools.lisansfiyatlari.Fend * PayEndeks;
                    else
                        PayEndeks_t = MyTools.lisansfiyatlari.Yend * PayEndeks;
                }

                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.PayGS == true)
                {
                    PayGs++;
                    if (item.MusteriMenseiID == 1)
                        PayGs_t = MyTools.lisansfiyatlari.Fpit * PayGs;
                    else
                        PayGs_t = MyTools.lisansfiyatlari.Ypit * PayGs;
                }


                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.VeriAnalitik == true)
                {
                    analitik++;
                    if (item.MusteriMenseiID == 1)
                        analitik_t = MyTools.lisansfiyatlari.Fpva * analitik;
                    else
                        analitik_t = MyTools.lisansfiyatlari.Ypva * analitik;
                }
                #endregion

                #region viop

                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == false && item.LisansDurum.ViopL2 == false && item.LisansDurum.Vd2P == false)
                {
                    viopYuzeysel++;
                    if (item.MusteriMenseiID == 1)
                        viopYuzeysel_t = MyTools.lisansfiyatlari.Fvl1 * viopYuzeysel;
                    else
                        viopYuzeysel_t = MyTools.lisansfiyatlari.Yvl1 * viopYuzeysel;
                }


                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == false && item.LisansDurum.Vd2P == false)
                {
                    viopPlus++;
                    if (item.MusteriMenseiID == 1)
                        viopPlus_t = MyTools.lisansfiyatlari.Fvl1p * viopPlus;
                    else
                        viopPlus_t = MyTools.lisansfiyatlari.Yvl1p * viopPlus;
                }

                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == true && item.LisansDurum.Vd2P == false)
                {
                    viopDerinlik++;
                    if (item.MusteriMenseiID == 1)
                        viopDerinlik_t = MyTools.lisansfiyatlari.Fvl2 * viopDerinlik;
                    else
                        viopDerinlik_t = MyTools.lisansfiyatlari.Yvl2 * viopDerinlik;
                }

                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.ViopL1 == true && item.LisansDurum.ViopLP == true && item.LisansDurum.ViopL2 == true && item.LisansDurum.Vd2P== true)
                {
                    viopDerinlik++;
                    if (item.MusteriMenseiID == 1)
                        viopDerinlik_t = MyTools.lisansfiyatlari.Fvd2p * viopDerinlikPlus;
                    else
                        viopDerinlik_t = MyTools.lisansfiyatlari.Yvd2p* viopDerinlikPlus;
                }

                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.ViopGS == true)
                {
                    viopGS++;
                    if (item.MusteriMenseiID == 1)
                        viopGS_t = MyTools.lisansfiyatlari.Fvit * viopGS;
                    else
                        viopGS_t = MyTools.lisansfiyatlari.Yvit * viopGS;
                }

                #endregion

                #region Tahvil

                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == false && item.LisansDurum.TahvilL2 == false)
                {
                    TahvilYuzeysel++;
                    if (item.MusteriMenseiID == 1)
                        TahvilYuzeysel_t = MyTools.lisansfiyatlari.Fbd1 * TahvilYuzeysel;
                    else
                        TahvilYuzeysel_t = MyTools.lisansfiyatlari.Ybd1 * TahvilYuzeysel;
                }
                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == true && item.LisansDurum.TahvilL2 == false)
                {
                    TahvilPlus++;
                    if (item.MusteriMenseiID == 1)
                        TahvilPlus_t = MyTools.lisansfiyatlari.Fbd1p * TahvilPlus;
                    else
                        TahvilPlus_t = MyTools.lisansfiyatlari.Ybd1p * TahvilPlus;
                }

                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.TahvilL1 == true && item.LisansDurum.TahvilLP == true && item.LisansDurum.TahvilL2 == true)
                {
                    TahvilDerinlik++;
                    if (item.MusteriMenseiID == 1)
                        TahvilDerinlik_t = MyTools.lisansfiyatlari.Fbd2 * TahvilDerinlik;
                    else
                        TahvilDerinlik_t = MyTools.lisansfiyatlari.Ybd2 * TahvilDerinlik;
                }
                #endregion

                #region AnalizPro
                if (item.LisansDurum.YayinDurumu == true && item.LisansDurum.AnPro == true)
                {
                    AnalizPro++;
                    if (item.MusteriMenseiID == 1)
                        AnalizPro_t = MyTools.lisansfiyatlari.FanPro * AnalizPro;
                    else
                        AnalizPro_t = MyTools.lisansfiyatlari.YanPro * AnalizPro;
                }
                #endregion
            }

            yutdiditoplam = spotpaket_tutar + DJI_tutar+ XETRA_tutar+CBOT_tutar+CME_tutar+ CBOTM_tutar+CMEM_tutar+ EUREX_tutar;
            BorsaLisansToplam = PayYuzeysel_t+ PayDerinlik_t + PayDerinlikPlus_t + PayPlus_t + PayEndeks_t+ PayGs_t+ analitik_t+ viopYuzeysel_t+ viopPlus_t+ viopDerinlik_t + viopDerinlikPlus_t + viopGS_t + TahvilYuzeysel_t+ TahvilPlus_t+ TahvilDerinlik_t + AnalizPro_t;

            var aratoplam = pro_t + Cep_t + BorsaLisansToplam + yutdiditoplam + projetutar+projeBorsaTutar;

            var kdv = aratoplam * 0.20m;

            var geneltoplam = aratoplam + kdv;


     
            //KOD101
         
            gridDurumOzet.Rows[0].SetValues("Pro", pro.ToString(),"PayL1", PayYuzeysel.ToString(), "ViopL1", viopYuzeysel.ToString(), "Spot Paket", spotpaket, "CBOTM", CBOTM, "Pro Toplam", pro_t.ToString("0.00"));
            gridDurumOzet.Rows[1].SetValues("Cep", Cep.ToString(), "PayL1+", PayPlus.ToString(), "ViopL1+", viopPlus.ToString(), "DJI", DJI, "CMEM", CMEM, "Cep Toplam", Cep_t.ToString("0.00"));
            gridDurumOzet.Rows[2].SetValues("Robot", robot.ToString(), "PayL2", PayDerinlik.ToString(), "ViopL2", viopDerinlik.ToString(), "SPI", SPI, "EUREX", EUREX, "Proje Toplam",projetutar.ToString("0.00"));
            gridDurumOzet.Rows[3].SetValues("Projeler", projesayisi, "PayX", PayEndeks.ToString(), "ViopGS", viopGS.ToString(), "XETRA", XETRA,"","", "Yurtdışı Toplam",yutdiditoplam.ToString("0.00"));
            gridDurumOzet.Rows[4].SetValues("AnalizPro", AnalizPro.ToString(), "PayGS", PayGs.ToString(), "BD1", TahvilYuzeysel, "CBOT", CBOT, "","","Borsa Lisans",BorsaLisansToplam.ToString("0.00"));
            gridDurumOzet.Rows[5].SetValues("", "",  "PVA", analitik, "BD1P", TahvilPlus, "CME", CME, "", "","ProjeBorsaLisans",projeBorsaTutar.ToString("0.00"));
            gridDurumOzet.Rows[6].SetValues("", "",  "", "", "BD2", TahvilDerinlik, "", "", "", "", "Aratoplam", string.Format("{0:C}", aratoplam));
            gridDurumOzet.Rows[7].SetValues("", "",  "", "", "", "", "", "", "","", "KDV", string.Format("{0:C}", kdv));
            gridDurumOzet.Rows[8].SetValues("", "", "", "", "", "", "", "", "", "", "Genel Toplam", string.Format("{0:C}", geneltoplam));



            gridDurumOzet.Rows[8].Cells[11].Selected = true; 



            var renk = Color.Firebrick;
            var renkback = Color.WhiteSmoke;
            for (int i = 0; i < 9; i++)
            {
                gridDurumOzet.Rows[i].Cells[0].Style.ForeColor = gridDurumOzet.Rows[i].Cells[2].Style.ForeColor
                    = gridDurumOzet.Rows[i].Cells[4].Style.ForeColor = gridDurumOzet.Rows[i].Cells[6].Style.ForeColor
                    = gridDurumOzet.Rows[i].Cells[8].Style.ForeColor = gridDurumOzet.Rows[i].Cells[10].Style.ForeColor = renk;
                gridDurumOzet.Rows[i].Cells[0].Style.BackColor = gridDurumOzet.Rows[i].Cells[2].Style.BackColor 
                    = gridDurumOzet.Rows[i].Cells[4].Style.BackColor = gridDurumOzet.Rows[i].Cells[6].Style.BackColor
                    = gridDurumOzet.Rows[i].Cells[8].Style.BackColor = gridDurumOzet.Rows[i].Cells[10].Style.BackColor = renkback;
            }



        }

        private void tbSozlesmeDetay_SelectedIndexChanged(object sender, EventArgs e)
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            if (tbSozlesmeDetay.SelectedIndex == 0)
            {
                var ekranlar = crm.Users.Where(x => x.SozlesmeID == activeitem && x.LisansDurum.YayinDurumu == true && x.Status.StatusAdi!="SRV");
                dgEkran.DataSource = ekranlar.Select(x => new { Ad_Soyad = x.Name + " " + x.Surname, x.Sozlesmeler.SozlesmeNo, x.ProductType, x.UserName, x.UserID });

                var ekransayisi = ekranlar.ToList().Count;
                lblSozlesmeSayisi.Text = ekransayisi.ToString();
            }
           else if (tbSozlesmeDetay.SelectedIndex == 1)            {
                var projeler = crm.Projelers.Where(x => x.SozlesmeID == activeitem && x.yayinDurum == true);
                dgProje.DataSource = projeler.Select(x => new { x.MusteriNo, x.Sozlesmeler.SozlesmeNo, x.KullaniciAdi, x.ProjeAdi ,x.id  });

                var projesayisi = projeler.ToList().Count;
                lblSozlesmeSayisi.Text = projesayisi.ToString();

            }
            


        }

        private void dgEkran_DoubleClick(object sender, EventArgs e)
        {

            if (dgEkran.SelectedRows.Count < 1)
                return;

            var id = dgEkran.CurrentRow.Cells[4].Value;     
            formUserDetay frm = new formUserDetay();
            frm.Tag = id;
            frm.Show();

        }

        private void dgProje_DoubleClick(object sender, EventArgs e)
        {

            if (dgProje.SelectedRows.Count < 1)
                return;

            var id = dgProje.CurrentRow.Cells[4].Value;
            formProjeler frm = new formProjeler();
            frm.Tag = id;
            frm.ShowDialog();
        }
    }
}
