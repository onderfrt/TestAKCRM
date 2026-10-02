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
    public partial class formTeknikAnalizRapor : Form
    {
        public static formTeknikAnalizRapor reference;

        private const string DurumAktif = "Aktif";
        private const string DurumBeklemede = "Beklemede";
        private const string DurumSuresiDoldu = "Süresi Doldu";
        private const string TarihFormat = "dd.MM.yyyy HH:mm";

        // Load sırasında combo'ların SelectedIndexChanged olayı Ara()'yı gereksiz yere tetiklemesin
        private bool yukleniyor = true;

        public formTeknikAnalizRapor()
        {
            InitializeComponent();
            reference = this;
        }

        private void formTeknikAnalizRapor_Load(object sender, EventArgs e)
        {
            try
            {
                comboPiyasa.SelectedIndex = 0;
                comboDurum.SelectedIndex = 0;
                yukleniyor = false;

                Ara();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void formTeknikAnalizRapor_FormClosed(object sender, FormClosedEventArgs e)
        {
            reference = null;
        }

        #region Arama / Listeleme

        // Yeni/Düzenle formu kayıttan sonra listeyi yenileyebilsin diye public
        public void Ara()
        {
            try
            {
                var oncekiSeciliId = SeciliId();

                crmDFNDataContext crm = new crmDFNDataContext();
                var sorgu = crm.TeknikAnalizRapors.Where(x => x.Baslik != null);

                // Serbest metin: başlık, içerik veya sembol
                var aranan = txtUserName.Text.Trim();
                if (aranan != "")
                {
                    var arananSembol = aranan._ToEngUp();
                    sorgu = sorgu.Where(x => x.Baslik.Contains(aranan)
                                          || x.icerik.Contains(aranan)
                                          || x.Stocks.Contains(arananSembol));
                }

                if (comboPiyasa.SelectedIndex == 1)
                    sorgu = sorgu.Where(x => x.Piyasa == "Hisse");
                else if (comboPiyasa.SelectedIndex == 2)
                    sorgu = sorgu.Where(x => x.Piyasa == "Viop");

                var simdi = DateTime.Now;

                switch (comboDurum.SelectedIndex)
                {
                    case 1: // Aktif
                        sorgu = sorgu.Where(x => (x.BaslangicTarihi == null || x.BaslangicTarihi <= simdi)
                                              && (x.BitisTarihi == null || x.BitisTarihi >= simdi));
                        break;
                    case 2: // Beklemede
                        sorgu = sorgu.Where(x => x.BaslangicTarihi != null && x.BaslangicTarihi > simdi);
                        break;
                    case 3: // Süresi Doldu
                        sorgu = sorgu.Where(x => x.BitisTarihi != null && x.BitisTarihi < simdi);
                        break;
                }

                var liste = sorgu.OrderByDescending(x => x.Tarih).Select(x => new
                {
                    x.id,
                    x.Tarih,
                    x.Piyasa,
                    x.Baslik,
                    x.Stocks,
                    Baslangic = x.BaslangicTarihi,
                    Bitis = x.BitisTarihi,
                    Durum = (x.BaslangicTarihi != null && x.BaslangicTarihi > simdi) ? DurumBeklemede
                          : (x.BitisTarihi != null && x.BitisTarihi < simdi) ? DurumSuresiDoldu
                          : DurumAktif,
                    x.link,
                }).ToList();

                dataGridView1.DataSource = liste;
                lblKayitSayisi.Text = liste.Count + " kayıt";

                SatirSec(oncekiSeciliId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                KolonlariDuzenle();
                SatirlariRenklendir();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void KolonlariDuzenle()
        {
            var basliklar = new Dictionary<string, string>
            {
                { "id", "Id" },
                { "Tarih", "Kayıt Tarihi" },
                { "Piyasa", "Piyasa" },
                { "Baslik", "Başlık" },
                { "Stocks", "Semboller" },
                { "Baslangic", "Başlangıç" },
                { "Bitis", "Bitiş" },
                { "Durum", "Durum" },
                { "link", "Link" },
            };

            foreach (DataGridViewColumn kolon in dataGridView1.Columns)
            {
                string baslik;
                if (basliklar.TryGetValue(kolon.Name, out baslik))
                    kolon.HeaderText = baslik;
            }

            foreach (var ad in new[] { "Tarih", "Baslangic", "Bitis" })
            {
                if (dataGridView1.Columns.Contains(ad))
                    dataGridView1.Columns[ad].DefaultCellStyle.Format = TarihFormat;
            }
        }

        private void SatirlariRenklendir()
        {
            if (!dataGridView1.Columns.Contains("Durum")) return;

            foreach (DataGridViewRow satir in dataGridView1.Rows)
            {
                var durum = Convert.ToString(satir.Cells["Durum"].Value);

                if (durum == DurumBeklemede)
                    satir.DefaultCellStyle.BackColor = Color.LightYellow;
                else if (durum == DurumSuresiDoldu)
                    satir.DefaultCellStyle.ForeColor = Color.Gray;
            }
        }

        #endregion

        #region Seçim yardımcıları

        private int? SeciliId()
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0) return null;
            if (!dataGridView1.Columns.Contains("id")) return null;

            var deger = dataGridView1.CurrentRow.Cells["id"].Value;
            return deger == null ? (int?)null : (int)deger;
        }

        // Liste yenilendikten sonra kullanıcının seçtiği satırda kalınsın
        private void SatirSec(int? id)
        {
            if (!id.HasValue || !dataGridView1.Columns.Contains("id")) return;

            foreach (DataGridViewRow satir in dataGridView1.Rows)
            {
                if (satir.Cells["id"].Value is int && (int)satir.Cells["id"].Value == id.Value)
                {
                    dataGridView1.CurrentCell = satir.Cells["id"];
                    satir.Selected = true;
                    return;
                }
            }
        }

        #endregion

        #region İşlemler

        private void Duzenle()
        {
            try
            {
                var id = SeciliId();
                if (!id.HasValue) return;

                // Aynı rapor zaten açıksa yenisini açma, öne getir
                var acik = Application.OpenForms.OfType<formTeknikAnalizRaporYeni>()
                                      .FirstOrDefault(f => f.activeid == id.Value);
                if (acik != null)
                {
                    acik.Activate();
                    return;
                }

                var frm = new formTeknikAnalizRaporYeni();
                frm.Tag = id.Value;
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Sil()
        {
            try
            {
                var id = SeciliId();
                if (!id.HasValue) return;

                if (MessageBox.Show(id + " id Numaralı Teknik Analiz Raporu Silinecektir Onaylıyormusunuz ?",
                    "Teknik Analiz Rapor Silme İşlemi", MessageBoxButtons.YesNo) == DialogResult.No) return;

                crmDFNDataContext crm = new crmDFNDataContext();
                var item = crm.TeknikAnalizRapors.FirstOrDefault(x => x.id == id.Value);
                if (item == null)
                {
                    MessageBox.Show("Teknik Analiz Raporu Bulunamadı");
                    Ara();
                    return;
                }

                crm.TeknikAnalizRapors.DeleteOnSubmit(item);
                crm.SubmitChanges();

                formAdminAra.referance.IPport.DataToSend = "DeleteTeknikRapor|" + id.Value.ToString() + (char)3;
                MessageBox.Show("Teknik Analiz Raporu Silinmiştir");

                Ara();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Olaylar

        private void button1_Click(object sender, EventArgs e)
        {
            Ara();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new formTeknikAnalizRaporYeni();
                frm.Tag = 0;
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Ara();
            }
        }

        private void comboFiltre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!yukleniyor) Ara();
        }

        private void formTeknikAnalizRapor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                e.SuppressKeyPress = true;
                Ara();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Başlık satırına çift tıklama düzenleme açmasın
            if (e.RowIndex >= 0) Duzenle();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Sağ tıklanan satır seçilsin; aksi halde "Sil" önceden seçili başka satırı silerdi
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dataGridView1.ClearSelection();
                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex >= 0 ? e.ColumnIndex : 0];
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Duzenle();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                e.SuppressKeyPress = true;
                Sil();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var seciliVar = SeciliId().HasValue;
            duzenleToolStripMenuItem.Enabled = seciliVar;
            silToolStripMenuItem.Enabled = seciliVar;
        }

        private void duzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Duzenle();
        }

        private void yenileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ara();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sil();
        }

        #endregion
    }
}