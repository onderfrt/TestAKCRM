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
    public partial class formChart : Form
    {
        public formChart()
        {
            InitializeComponent();
        }

        private void formChart_Load(object sender, EventArgs e)
        {
            lblX_curr.Visible = false;
            ChartCiz();
        }

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            lblX.Visible = true;
            lblY.Visible = true;
            lblX_curr.Visible = true;
        }

        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            lblX.Visible = false;
            lblY.Visible = false;
            lblX_curr.Visible = false;
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {

            try
            {

                lblX.Location = new Point((e.X), -5);
                lblY.Location = new Point(-5, (e.Y));


                if (e.X <= 80 || e.Y >= 350 || e.Y <= 23 || e.X >= 700)
                {


                }
                else
                {



                }

                var yvalue = chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);

                var Xvalue = DateTime.FromOADate(chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X)).Date;

                lblX_curr.Text = yvalue.ToString("0") + "\n" + Xvalue;


            }
            catch
            {


            }

        }

        private void ChartCiz()
        {

            var tag = (string)this.Tag;

            crmDFNDataContext crm = new crmDFNDataContext();
            var sorgu = crm.GunSonuToplamlars.OrderByDescending(x => x.tarih).Where(x=>x.id>0).ToList();

            this.Text = tag + "  Grafik Analizi";
          

            var liste = new List<GunSonuToplamlar>();

            switch (tag)
            {
                case "id":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.id }).ToList();                 

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "TumUserSayisi":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.TumUserSayisi }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "IseUserSayisi":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.IseUserSayisi }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "TrkUserSayisi":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.TrkUserSayisi }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "MuafUserSayisi":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.MuafUserSayisi }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "Desktop":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.Desktop }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "Mobile":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.Mobile }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "Robot":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.Robot }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "DesktopMobile":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.DesktopMobile }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "PayYuzeysel":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.PayYuzeysel }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "PayPlus":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.PayPlus }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "PayDerinlik":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.PayDerinlik }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;

                case "PayDerinlikPlus":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.Pd2P }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;

                case "PayEndeks":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.PayEndeks }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "PayGs":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.PayGs }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "Analitik":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.Analitik }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "PITE":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.PITE }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "ViopYuzeysel":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.ViopYuzeysel }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "ViopPlus":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.ViopPlus }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "ViopDerinlik":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.ViopDerinlik }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "ViopDerinlikPlus":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.Vd2P }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "ViopGS":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.ViopGS }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "TahvilYuzeysel":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.TahvilYuzeysel }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "TahvilPlus":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.TahvilPlus }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "TahvilDerinlik":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.TahvilDerinlik }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "KapaliUserSayisi":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.KapaliUserSayisi }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "DJI":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.DJI }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "SPI":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.SPI }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "XETRA":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.XETRA }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "CBOT":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.CBOT }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "CME":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.CME }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "CBOTM":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.CBOTM }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "CMEM":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.CMEM }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "EUREX":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.EUREX }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "ProEkran":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.ProEkran }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "NonProEkran":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.NonProEkran }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "MKK":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.MKK }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "TARAMA":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.TARAMA }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                case "GKKUL":
                    {
                        var sorgu2 = sorgu.Select(x => new { x.tarih, Deger = x.GKKUL }).ToList();

                        chart1.Series[0].Points.Clear();
                        for (int i = 0; i < sorgu2.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(sorgu2[i].tarih.Value.Date, sorgu2[i].Deger);
                        }
                    }
                    break;
                default:
                    break;
            }




            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chart1.Series[0].Points[0].Color = Color.White;
        }


    }
}
