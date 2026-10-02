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
    public partial class formTeknikAnalizRaporYeni : Form
    {
        public formTeknikAnalizRaporYeni()
        {
            InitializeComponent();
        }

        public int activeid = 0;

        // Mail logunda "talep eden çalışan" bilgisi için. Formu açan yer set etmeli.
        // TODO: Oturumdaki çalışan id'sinin kaynağı netleşince doldurulacak.
        public int calisanId = 0;

        private const string TarihFormat = "dd.MM.yyyy HH:mm";

        private void formTeknikAnalizRaporYeni_Load(object sender, EventArgs e)
        {
            try
            {
                activeid = (int)this.Tag;
                comboPiyasa.SelectedIndex = 0;

                KontrolleriHazirla();

                if (activeid > 0)
                {
                    crmDFNDataContext crm = new crmDFNDataContext();

                    var item = crm.TeknikAnalizRapors.FirstOrDefault(x => x.id == activeid);
                    if (item == null)
                    {
                        MessageBox.Show("Teknik Analiz Raporu Bulunamadı");
                        return;
                    }

                    txtBaslik.Text = item.Baslik;
                    txtLink.Text = item.link;
                    richboxicerik.Text = item.icerik;

                    if (item.Piyasa == "Hisse")
                        comboPiyasa.SelectedIndex = 1;
                    else if (item.Piyasa == "Viop")
                        comboPiyasa.SelectedIndex = 2;

                    // Eski kod Stocks NULL olduğunda Split'te hata veriyordu
                    if (!string.IsNullOrWhiteSpace(item.Stocks))
                    {
                        foreach (var sembol in item.Stocks.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                            lboxSembol.Items.Add(sembol.Trim());
                    }

                    TarihYukle(dtpBaslangic, item.BaslangicTarihi);
                    TarihYukle(dtpBitis, item.BitisTarihi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void KontrolleriHazirla()
        {
            foreach (var dtp in new[] { dtpBaslangic, dtpBitis })
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = TarihFormat;
                dtp.ShowCheckBox = true;   // İşaretsiz = tarih girilmedi (NULL)
                dtp.Value = DateTime.Now;
                dtp.Checked = false;       // Value'dan SONRA set edilmeli; Value set etmek Checked'ı true yapar
            }

            chkMailGonder.Checked = true;
            lblViopUyari.Visible = false;
        }

        private void TarihYukle(DateTimePicker dtp, DateTime? deger)
        {
            if (deger.HasValue)
            {
                dtp.Value = deger.Value;
                dtp.Checked = true;
            }
            else
            {
                dtp.Checked = false;
            }
        }

        private DateTime? TarihOku(DateTimePicker dtp)
        {
            if (!dtp.Checked) return null;

            var v = dtp.Value;
            return new DateTime(v.Year, v.Month, v.Day, v.Hour, v.Minute, 0); // saniye/milisaniye sıfırlanır
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBaslik.Text.Trim() == "")
                {
                    MessageBox.Show("Başlık Bilgisini Giriniz");
                    return;
                }

                if (comboPiyasa.SelectedIndex == 0)
                {
                    MessageBox.Show("Piyasa Bilgisini Seçiniz");
                    return;
                }

                var baslangic = TarihOku(dtpBaslangic);
                var bitis = TarihOku(dtpBitis);

                if (baslangic.HasValue && bitis.HasValue && bitis.Value < baslangic.Value)
                {
                    MessageBox.Show("Bitiş tarihi başlangıç tarihinden önce olamaz");
                    return;
                }

                if (bitis.HasValue && bitis.Value < DateTime.Now)
                {
                    if (MessageBox.Show("Bitiş tarihi geçmişte olduğu için rapor mobilde gösterilmeyecek. Devam edilsin mi?",
                        "Teknik Analiz Raporu", MessageBoxButtons.YesNo) == DialogResult.No) return;
                }

                if (chkMailGonder.Checked)
                {
                    if (MessageBox.Show("Rapor mail olarak da gönderilecektir. Onaylıyor musunuz?",
                        "Mail Gönderimi", MessageBoxButtons.YesNo) == DialogResult.No) return;
                }

                crmDFNDataContext crm = new crmDFNDataContext();

                TeknikAnalizRapor item;
                string komut;

                if (activeid > 0)
                {
                    item = crm.TeknikAnalizRapors.FirstOrDefault(x => x.id == activeid);
                    if (item == null)
                    {
                        MessageBox.Show("Teknik Analiz Raporu Bulunamadı"); return;
                    }
                    komut = "UpdateTeknikRapor";
                }
                else
                {
                    item = new TeknikAnalizRapor();
                    item.Tarih = DateTime.Now;
                    crm.TeknikAnalizRapors.InsertOnSubmit(item);
                    komut = "CreateTeknikRapor";
                }

                item.Baslik = txtBaslik.Text.Trim();
                item.icerik = richboxicerik.Text.Trim();
                item.Piyasa = comboPiyasa.Text.Trim();
                item.Stocks = string.Join(",", lboxSembol.Items.Cast<object>().Select(x => x.ToString()).ToArray());
                item.link = txtLink.Text.Trim();
                item.BaslangicTarihi = baslangic;
                item.BitisTarihi = bitis;
                crm.SubmitChanges();

                // Mevcut Create/Update mesajı DEĞİŞMEDEN gider.
                var gonderilecek = komut + "|" + item.id.ToString() + (char)3;

                if (chkMailGonder.Checked)
                {
                    var log = new TeknikAnalizRaporMailLog();
                    log.RaporId = item.id;
                    log.Baslik = item.Baslik;
                    log.TalepEdenCalisanId = calisanId > 0 ? (int?)calisanId : null;
                    log.TalepTarihi = DateTime.Now; // LINQ to SQL DB default'unu kullanmaz, açıkça set edilmeli
                    log.Durum = 0;                  // 0: Bekliyor
                    crm.TeknikAnalizRaporMailLogs.InsertOnSubmit(log);
                    crm.SubmitChanges();

                    // İki mesaj tek yazımda, her biri kendi (char)3 sonlandırıcısıyla
                    gonderilecek += "SendTeknikRaporMail|" + log.id.ToString() + (char)3;
                }

                formAdminAra.referance.IPport.DataToSend = gonderilecek;

                MessageBox.Show(activeid > 0 ? "Teknik Analiz Raporu Güncellendi" : "Teknik Analiz Raporu Kaydedildi");

                if (formTeknikAnalizRapor.reference != null)
                    formTeknikAnalizRapor.reference.Ara();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                // Stocks formatı: virgülle ayrılmış, boşluksuz, büyük harf İngilizce karakter
                var sembol = txtSembol.Text.Trim()._ToEngUp().Replace(" ", "").Replace(",", "");

                if (sembol == "")
                {
                    MessageBox.Show("Lütfen Sembol Giriniz");
                    return;
                }

                if (lboxSembol.Items.Cast<object>().Any(x => x.ToString() == sembol))
                {
                    MessageBox.Show(sembol + " zaten listede");
                    return;
                }

                lboxSembol.Items.Add(sembol);
                txtSembol.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCikar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lboxSembol.SelectedIndex < 0)
                {
                    MessageBox.Show("Çıkarmak için bir listeden sembol seçiniz");
                    return;
                }
                lboxSembol.Items.RemoveAt(lboxSembol.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboPiyasa_SelectedIndexChanged(object sender, EventArgs e)
        {
            // VİOP raporları mobilde gösterilmeyecek
            lblViopUyari.Visible = comboPiyasa.SelectedIndex == 2;
        }
    }
}