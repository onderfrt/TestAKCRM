using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DirectFNCRM.AdminViews
{
    public partial class formToplamAnaliz : Form
    {
        public formToplamAnaliz()
        {
            InitializeComponent();
            gridLisanDurumOzet.Rows.Add(9);
            gridLisanDurumOzet._DoubleBuffer(true);
            
            
        }
        public bool acilis = false;
        private void formToplamAnaliz_Load(object sender, EventArgs e)
        {

            crmDFNDataContext crm = new crmDFNDataContext();
            var sorgu = crm.GunSonuToplamlars.OrderByDescending(x => x.tarih).ToList();
            dataGridView1.DataSource = sorgu.Select(x => new { x.tarih.Value.Date, x.TumUserSayisi, x.MuafUserSayisi, x.Mobile, x.Desktop, x.DesktopMobile, x.PayPlus, x.PayDerinlik, x.Pd2P, x.PayEndeks, x.ViopDerinlik, x.Vd2P, x.ViopPlus, x.CME, x.MKK, x.GKKUL }).ToList();

            dataGridView1.Columns["CME"].HeaderText = "TradeAllAlgo";

            dtEndDate.Value = DateTime.Now.AddDays(-1);

            var now = DateTime.Now.AddMonths(-1);
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var DaysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            var lastDay = new DateTime(now.Year, now.Month, DaysInMonth);

            dtStartDate.Value = lastDay;

            ToplamShow();


            acilis = true;


        }



        public void ToplamShow()
        {

            try
            {

                #region Toplamlar

                if (dtStartDate.Value.Date < new DateTime(2017, 1, 26))
                    dtStartDate.Value = new DateTime(2017, 1, 26);

                if (dtEndDate.Value.Date >= DateTime.Now.Date)
                    dtEndDate.Value = DateTime.Now.Date.AddDays(-1);


                crmDFNDataContext crm = new crmDFNDataContext();
                var ActiveSorgu = crm.GunSonuToplamlars.ToList();

                // var t2 = ActiveSorgu.FirstOrDefault(x => x.tarih.Value.Date == dtStartDate.Value.Date);
                var targetDate = dtStartDate.Value.Date;

                var t2 = ActiveSorgu
                    .FirstOrDefault(x => x != null
                                      && x.tarih.HasValue
                                      && x.tarih.Value.Date == targetDate);

                var t1 = ActiveSorgu.FirstOrDefault(x => x.tarih.Value.Date == dtEndDate.Value.Date);





                //var Cep = t1.Mobile - t2.Mobile;
                var pro = t1.Desktop - t2.Desktop;
                var Cep = t1.Mobile - t2.Mobile;
                var robot = t1.Robot - t2.Robot;

                var ProCep = t1.DesktopMobile - t2.DesktopMobile;
               // var PayYuzeysel = t1.PayYuzeysel - t2.PayYuzeysel;
                var PayPlus = t1.PayPlus - t2.PayPlus;
                var PayDerinlik = t1.PayDerinlik - t2.PayDerinlik;
                var PayDerinlikPlus = t1.Pd2P - t2.Pd2P;
                var PayEndeks = t1.PayEndeks - t2.PayEndeks;
                var PayGs = t1.PayGs - t2.PayGs;
                var idealgo = t1.CME - t2.CME;

              //  var analitik = t1.Analitik - t2.Analitik;
                var pite = t1.PITE - t2.PITE;
                var mkk = t1.MKK - t2.MKK;
                var gkkul = t1.GKKUL - t2.GKKUL;

              //  var viopYuzeysel = t1.ViopYuzeysel - t2.ViopYuzeysel;
                var viopPlus = t1.ViopPlus - t2.ViopPlus;
                var viopDerinlik = t1.ViopDerinlik - t2.ViopDerinlik;
                var viopDerinlikPlus = t1.Vd2P - t2.Vd2P;
                var viopGS = t1.ViopGS - t2.ViopGS;

              //  var TahvilYuzeysel = t1.TahvilYuzeysel - t2.TahvilYuzeysel;
                var TahvilPlus = t1.TahvilPlus - t2.TahvilPlus;
                var TahvilDerinlik = t1.TahvilDerinlik - t2.TahvilDerinlik;

                var On = t1.TumUserSayisi - t2.TumUserSayisi;
                var Off = t1.KapaliUserSayisi - t2.KapaliUserSayisi;




                gridLisanDurumOzet.Rows[0].SetValues("Pro", pro.ToString(), "","", "", "");
                gridLisanDurumOzet.Rows[1].SetValues("Cep", Cep.ToString(), "PD1P", PayPlus.ToString(), "VD1P", viopPlus.ToString());
                gridLisanDurumOzet.Rows[2].SetValues("Pro+Cep", ProCep.ToString(), "PD2", PayDerinlik.ToString(), "VD2", viopDerinlik.ToString());
                gridLisanDurumOzet.Rows[3].SetValues("", "", "PD2P", PayDerinlik.ToString(), "VD2P", viopDerinlik.ToString());
                gridLisanDurumOzet.Rows[4].SetValues("Robot", robot.ToString(), "END", PayEndeks.ToString(), "VIT", viopGS.ToString());
                gridLisanDurumOzet.Rows[5].SetValues("TradeAll Algo", idealgo.ToString(), "PIT", PayGs.ToString(), "", "");
                gridLisanDurumOzet.Rows[6].SetValues("Açık", On.ToString(), "PITE", pite.ToString(), "BD1P", TahvilPlus.ToString());
                gridLisanDurumOzet.Rows[7].SetValues("Kapalı", Off.ToString(), "", "", "BD2", TahvilDerinlik.ToString());
                gridLisanDurumOzet.Rows[8].SetValues("MKK", mkk.ToString(), "GKKUL", gkkul.ToString(), "", "");


                gridLisanDurumOzet.Rows[6].Cells[1].Selected = true;

                var renk = Color.Firebrick;
                var renkback = Color.WhiteSmoke;
                for (int i = 0; i < 8; i++)
                {
                    gridLisanDurumOzet.Rows[i].Cells[0].Style.ForeColor = gridLisanDurumOzet.Rows[i].Cells[2].Style.ForeColor = gridLisanDurumOzet.Rows[i].Cells[4].Style.ForeColor = renk;
                    gridLisanDurumOzet.Rows[i].Cells[0].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[2].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[4].Style.BackColor = renkback;
                }

                for (int i = 0; i < 8; i++)
                {
                    gridLisanDurumOzet.Rows[i].Cells[0].Style.ForeColor = gridLisanDurumOzet.Rows[i].Cells[2].Style.ForeColor = gridLisanDurumOzet.Rows[i].Cells[4].Style.ForeColor = renk;
                }


                #endregion


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            if (acilis)
                ToplamShow();
        }

        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            if (acilis)
                ToplamShow();
        }




        private void grafikToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = dataGridView1.SelectedCells[0].ColumnIndex;

            var c = dataGridView1.Columns[a].HeaderText;
            if (c == "tarih")
                return;


            var frm = new formChart();
            frm.Tag = c;
            frm.Show();
        }

        private void exceleAktarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1._CopyToExcel();
        }
    }
}
