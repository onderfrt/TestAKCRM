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
    public partial class formKurumTalepleri : Form
    {
        public formKurumTalepleri()
        {
            InitializeComponent();
            dataGridView1._DoubleBuffer(true);
        }

        private void formKurumTalepleri_Load(object sender, EventArgs e)
        {
            try
            {
                var crm = new crmDFNDataContext();
                var talepdurum = crm.KurumTalepDurums;
                comboDurum.DataSource = talepdurum;
                comboDurum.DisplayMember = "DurumAd";
                comboDurum.ValueMember = "id";
                comboDurum.SelectedIndex = 0;

                var sorgu = crm.Calisans.Where(x => x.CalisanTipId == 6 || x.CalisanTipId == 7).OrderBy(x => x.Ad).Select(x => new { x.calisanID, adsoyad = x.Ad + " " + x.Soyad });
                comboTalebiGiren.DataSource = sorgu;
                comboTalebiGiren.DisplayMember = "adsoyad";
                comboTalebiGiren.ValueMember = "calisanID";
                comboTalebiGiren.SelectedIndex = -1;
                Ara();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void Ara()
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                var sayi = crm.KurumTalepleris.Where(x => x.id > 0).OrderByDescending(x => x.TalepTarihi).ToList();
                var sonuc = crm.KurumTalepleris.Where(x => x.id > 0).OrderByDescending(x => x.TalepTarihi).Take(sayi.Count);
                var kriterx = txtKriter.Text.Trim();
                if (kriterx != "")
                {
                    sonuc = sonuc.Where(x => x.HesapNo.Contains(kriterx) || x.MusteriAd.Contains(kriterx) || x.Usename.Contains(kriterx) || x.Iletisim.email.Contains(kriterx)
                    || x.MusteriSoyad.Contains(kriterx) || x.PmtsNo.Contains(kriterx)).OrderByDescending(x => x.TalepTarihi).Take(500);
                }

                if (comboDurum.SelectedIndex != -1)
                {
                    sonuc = sonuc.Where(x => x.TalepDurumId == (int)comboDurum.SelectedValue);

                }
                if (comboTalebiGiren.SelectedIndex != -1)
                {
                    sonuc = sonuc.Where(x => x.TalebiGiren == (int)comboTalebiGiren.SelectedValue);

                }

                var listelenecekler = new List<TalepRecorKurum>();

                foreach (var item in sonuc)
                {
                    var rec = new TalepRecorKurum();

                    rec.TalepNo = item.id;
                    rec.Durum = item.KurumTalepDurum.DurumAd;
                    rec.AdSoyad = item.MusteriAd + " " + item.MusteriSoyad;
                    rec.Sube = item.SubeAdi;
                    rec.HesapNo = item.Usename;
                    rec.TemsilciKod = item.TemsilciKod;
                    rec.TalepTip = item.KurumTalepTip.TalepTipAd;
                    rec.TalepTarihi = item.TalepTarihi.Value.ToString();
                    rec.Pmtsno = item.PmtsNo;
                    listelenecekler.Add(rec);
                }
                dataGridView1.DataSource = listelenecekler.OrderByDescending(x => x.TalepNo).Select(x => new
                {
                    x.TalepNo,
                    x.TalepTarihi,
                    x.Pmtsno,
                    x.Durum,
                    x.HesapNo,
                    x.AdSoyad,
                    x.TalepTip,
                    x.Sube,
                    x.TemsilciKod,
                }).ToList();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public class TalepRecorKurum
        {
            public int TalepNo = 0;
            public string AdSoyad = "";
            public string TemsilciKod = "";
            public string Sube = "";
            public string TalepTip = "";
            public string HesapNo = "";
            public string Durum = "";
            public string TalepTarihi = "";
            public string Pmtsno = "";
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
                return;

            var id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

            var frm = new formKurumTalepDetay();

            frm.acriveTalepId = id;

            frm.Show();
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            Ara();
        }

        private void comboDurum_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Ara();
        }

        private void comboTalebiGiren_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Ara();
        }

        private void btnSecimleriTemizle_Click(object sender, EventArgs e)
        {
            txtKriter.Text = "";
            comboDurum.SelectedIndex = -1;
            comboTalebiGiren.SelectedIndex = -1;
            Ara();
        }

        private void btnAra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                if (txtKriter.Text != null)
                    Ara();
                //ToplamShow();

            }
        }
    }
}
