using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DirectFNCRM.AdminViews
{
    public partial class formSedatKontrol : Form
    {
        public formSedatKontrol()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            crmDFNDataContext crm = new crmDFNDataContext();
            var crmKullaciListesi = crm.Users.Select(x => x.UserName).ToList();
            //var ssoKullaciListesi = new List<string>();

       var filename = "";
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                filename = op.FileName;
            }

            if (filename == "")
                return;

            var newthread = new Thread(new ThreadStart(() => {


                var ssoKullaciListesi = File.ReadAllLines(op.FileName, Encoding.GetEncoding("iso-8859-9"));

                for (int i = 1; i < ssoKullaciListesi.Length; i++)
                {

                    if (crmKullaciListesi.Contains(ssoKullaciListesi[i]))
                    {

                    }
                    else
                      crmolmayanlaryaz(ssoKullaciListesi[i]+"\n");

                    lblsayacyaz(ssoKullaciListesi.Length.ToString() + " / " + (i + 1).ToString());

                }

            
                lblsayacyaz("0");

                for (int i = 1; i < crmKullaciListesi.Count; i++)
                {

                    if (ssoKullaciListesi.Contains(crmKullaciListesi[i]))
                    {

                    }
                    else
                      ssoolmayanlaryaz(crmKullaciListesi[i] + "\n");

                    lblsayacyaz(crmKullaciListesi.Count.ToString() + " / " + (i + 1).ToString());

                }

                lblsayacyaz("Bitti");



            }));

            newthread.Start();
        }
        public delegate void lblsayacguncelle(string text);

        public void lblsayacyaz(string text)
        {
            try
            {
                if (lblsayac.InvokeRequired)
                {
                    lblsayacguncelle lbl = new lblsayacguncelle(lblsayacyaz);

                    this.Invoke(lbl, new object[] { text });

                }
                else
                {
                    lblsayac.Text = text;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void crmolmayanlaryaz(string text)
        {
            try
            {
                if (rtxtCRMolmayanlar.InvokeRequired)
                {
                    lblsayacguncelle lbl = new lblsayacguncelle(crmolmayanlaryaz);

                    this.Invoke(lbl, new object[] { text });

                }
                else
                {
                    rtxtCRMolmayanlar.Text += text;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void ssoolmayanlaryaz(string text)
        {
            try
            {
                if (rtxtCRMolmayanlar.InvokeRequired)
                {
                    lblsayacguncelle lbl = new lblsayacguncelle(ssoolmayanlaryaz);

                    this.Invoke(lbl, new object[] { text });

                }
                else
                {
                    rtxtSSOolmayanlar.Text += text;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnPasifMusteri_Click(object sender, EventArgs e)
        {
            //var newtrhread = new Thread(new ThreadStart(() => {

            //    crmDFNDataContext crm = new crmDFNDataContext();

            //    var musteriler = crm.Musterilers.Where(x => x.Status == false).ToList();


            //    for (int i = 0; i < musteriler.Count; i++)
            //    {
            //        var sz = crm.Sozlesmelers.Where(x => x.MusteriNo == musteriler[i].MusteriNo);

            //        if (sz != null)
            //        {
            //            foreach (var item in sz)
            //            {
            //                item.status = false;
            //                crm.SubmitChanges();
            //            }
            //        }

            //    lblsayacyaz(i.ToString() + " / " + musteriler.Count);

            //    }


            //}));

            //newtrhread.Start();
        }

        private void formSedatKontrol_Load(object sender, EventArgs e)
        {

            crmDFNDataContext crm = new crmDFNDataContext();


            comboEventType.DataSource = crm.EventTypes;
            comboEventType.DisplayMember = "EventName";
            comboEventType.ValueMember = "EventTypeId";
            
        }

        public void ara()
        {

          

            crmDFNDataContext crm = new crmDFNDataContext();
            var sorgu = crm.UserEvents.Where(x => x.EventId != 0);

            var type = 1;
            
            if (comboEventType.SelectedIndex != -1)
            {
                if (comboEventType.SelectedValue is EventType)
                {
                    sorgu = sorgu.Where(x => x.EventType == comboEventType.SelectedValue);
                    type = comboEventType.SelectedIndex + 1;
                }
                else
                {
                    sorgu = sorgu.Where(x => x.EventTypeId == (int)comboEventType.SelectedValue);
                    type = (int)comboEventType.SelectedValue;
                }
            }
            var userolaylar = from ue in sorgu group ue by ue.EventTarih.Value.Date into g select new { tarih = g.Key, sayi = g.Select(x => x.User).Distinct().Count() };

            var list = userolaylar.ToList();

            // MessageBox.Show(" sayı" + list.Count);
           
            var cartismi = crm.EventTypes.FirstOrDefault(x => x.EventTypeId == type).EventName;
            chart1.Series[0].Name = cartismi;

            chart1.DataSource = list;
            chart1.Series[0].XValueMember = "tarih";
            chart1.Series[0].YValueMembers = "sayi";

            chart1.ChartAreas[0].AxisX.IntervalOffsetType = DateTimeIntervalType.Number;
            //var cartismi = crm.EventTypes.FirstOrDefault(x => x.EventTypeId == 1).EventName;


            //chart1.Series[0].Name = cartismi;

            //foreach (var item in list)
            //{
            //    chart1.Series[0].Points.AddXY(item.tarih.Date, item.sayi);
            //}
        }

        private void comboEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboEventType.SelectedIndex!=-1)
            ara();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var mec = chart1.ChartAreas[0].AxisX.Interval;
            chart1.ChartAreas[0].AxisX.Interval = mec + 1;
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var mec = chart1.ChartAreas[0].AxisX.Interval;
            if (mec == 1)
                return;
            chart1.ChartAreas[0].AxisX.Interval = mec - 1;
         
        }
    }
}
