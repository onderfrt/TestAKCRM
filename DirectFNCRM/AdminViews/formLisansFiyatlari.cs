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
    public partial class formLisansFiyatlari : Form
    {
        public formLisansFiyatlari()
        {
            InitializeComponent();
        }
        public int activeitem = 0;

        private void formLisansFiyatlari_Load(object sender, EventArgs e)
        {

            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                comboParaBirimi.DataSource = crm.ParaBirimis;
                comboParaBirimi.DisplayMember = "ParaBirimiKod";
                comboParaBirimi.ValueMember = "id";

                if (comboParaBirimi.Items.Count > 0)
                {
                    comboParaBirimi.SelectedIndex = 0;
                }


                gridGuncelle();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {

                crmDFNDataContext crm = new crmDFNDataContext();
                if (activeitem == 0)
                {
                    if (txtLisansKod.Text == string.Empty)
                    { MessageBox.Show("Lisans Kodu bölümü Bırakılamaz"); return; }


                    LisansFiyatlari lfiyat1 = new LisansFiyatlari();

                    lfiyat1.LisansKod = txtLisansKod.Text;
                    lfiyat1.Aciklama = txtAciklama.Text;
                    lfiyat1.Fiyat =decimal.Parse(txtFiyat.Text);
                    lfiyat1.YurtDisiFiyat = decimal.Parse(txtYurtDisiFiyat.Text);
                    lfiyat1.ParaBirimiId = (int)comboParaBirimi.SelectedValue;

                    crm.LisansFiyatlaris.InsertOnSubmit(lfiyat1);
                    crm.SubmitChanges();

                    if (lfiyat1.id > 0)
                    {
                        MessageBox.Show(lfiyat1.LisansKod + "   Kaydedildi");
                    }
                }
                else
                {
                    var lfiyati = crm.LisansFiyatlaris.FirstOrDefault(x => x.id == activeitem);
                    if (lfiyati != null)
                    {
                        if (MessageBox.Show(lfiyati.LisansKod + " Değiştirilecek", "Değiştirme İşlemi", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {

                            if (txtLisansKod.Text == string.Empty)
                            { MessageBox.Show("Lisans Kodu Boş Bırakılamaz"); return; }


                            lfiyati.LisansKod = txtLisansKod.Text;
                            lfiyati.LisansKod = txtLisansKod.Text;
                            lfiyati.Aciklama = txtAciklama.Text;
                            lfiyati.Fiyat = decimal.Parse(txtFiyat.Text);
                            lfiyati.YurtDisiFiyat = decimal.Parse(txtYurtDisiFiyat.Text);
                            if (lfiyati.ParaBirimiId!=null)
                            {
                                var pbirimi = crm.ParaBirimis.First(x => x.id == (int)comboParaBirimi.SelectedValue);
                                lfiyati.ParaBirimi = pbirimi;
                            }
                            else
                            {
                                lfiyati.ParaBirimiId = (int)comboParaBirimi.SelectedValue;
                            }
                            
                      
                            crm.SubmitChanges();

                            if (lfiyati.id > 0)
                            {
                                MessageBox.Show(lfiyati.LisansKod + "   Değiştirildi");
                            }

                        }

                    }
                    else MessageBox.Show(" Id si Bulunamadı");

                }

                #region LisansFiyatlariniSqldenOku


                var lisanlar = crm.LisansFiyatlaris.ToList();

                foreach (var lfiyat in lisanlar)
                {
                    switch (lfiyat.LisansKod)
                    {
                        case "PD1":
                            MyTools.lisansfiyatlari.Fpd1 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypd1 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "PD1P":
                            MyTools.lisansfiyatlari.Fpd1p = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypd1p = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "PD2":
                            MyTools.lisansfiyatlari.Fpd2 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypd2 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "PD2P":
                            MyTools.lisansfiyatlari.Fpd2p = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypd2p = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "END":
                            MyTools.lisansfiyatlari.Fend = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Yend = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "PIT":
                            MyTools.lisansfiyatlari.Fpit = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypit = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "PITE":
                            MyTools.lisansfiyatlari.Fpite = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypite = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "PVA":
                            MyTools.lisansfiyatlari.Fpva = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ypva = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "VD1":
                            MyTools.lisansfiyatlari.Fvl1 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Yvl1 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "VD1P":
                            MyTools.lisansfiyatlari.Fvl1p = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Yvl1p = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "VD2":
                            MyTools.lisansfiyatlari.Fvl2 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Yvl2 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "VD2P":
                            MyTools.lisansfiyatlari.Fvl2p = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Yvl2p = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "VIT":
                            MyTools.lisansfiyatlari.Fvit = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Yvit = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "KRMD1":
                            MyTools.lisansfiyatlari.Fkrmd1 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ykrmd1 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "BD1":
                            MyTools.lisansfiyatlari.Fbd1 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ybd1 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "BD1P":
                            MyTools.lisansfiyatlari.Fbd1p = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ybd1p = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "BD2":
                            MyTools.lisansfiyatlari.Fbd2 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ybd2 = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "Analiz Pro":
                            MyTools.lisansfiyatlari.FanPro = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.FanPro = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "SENTIL1":
                            MyTools.lisansfiyatlari.FSentiL1 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YSentiL1 = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "SENTIL2":
                            MyTools.lisansfiyatlari.FSentiL2 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YSentiL2 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "MKK":
                            MyTools.lisansfiyatlari.Fmkk = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ymkk = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "TARAMA":
                            MyTools.lisansfiyatlari.Ftarama = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ytarama = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "GKKUL":
                            MyTools.lisansfiyatlari.Fgkkul = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.Ygkkul = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "karma1k":
                            MyTools.lisansfiyatlari.karma1k = (decimal)lfiyat.Fiyat;
                            break;
                        case "karma5k":
                            MyTools.lisansfiyatlari.karma5k = (decimal)lfiyat.Fiyat;
                            break;
                        case "karma10k":
                            MyTools.lisansfiyatlari.karma10k = (decimal)lfiyat.Fiyat;
                            break;
                        case "karma20k":
                            MyTools.lisansfiyatlari.karma20k = (decimal)lfiyat.Fiyat;
                            break;
                        case "karma50k":
                            MyTools.lisansfiyatlari.karma50k = (decimal)lfiyat.Fiyat;
                            break;
                        case "karma100k":
                            MyTools.lisansfiyatlari.karma100k = (decimal)lfiyat.Fiyat;
                            break;
                        case "karmaSINIRSIZ":
                            MyTools.lisansfiyatlari.karmaSINIRSIZ = (decimal)lfiyat.Fiyat;
                            break;

                        case "GKULKYD":
                            MyTools.lisansfiyatlari.GKULKYD = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YGKULKYD = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "GKULEND":
                            MyTools.lisansfiyatlari.GKULEND = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YGKULEND = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "GKULD1P":
                            MyTools.lisansfiyatlari.GKULD1P = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YGKULD1P = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "GKULD2":
                            MyTools.lisansfiyatlari.GKULD2 = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YGKULD2 = (decimal)lfiyat.YurtDisiFiyat;
                            break;
                        case "GKULPITE":
                            MyTools.lisansfiyatlari.GKULPITE = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YGKULPITE = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "GKULPVA":
                            MyTools.lisansfiyatlari.GKULPVA = (decimal)lfiyat.Fiyat;
                            MyTools.lisansfiyatlari.YGKULPVA = (decimal)lfiyat.YurtDisiFiyat;
                            break;

                        case "GUYEKYD":
                            MyTools.lisansfiyatlari.GUYEKYD = (decimal)lfiyat.Fiyat;
                            break;

                        case "GUYEEND":
                            MyTools.lisansfiyatlari.GUYEEND = (decimal)lfiyat.Fiyat;
                            break;

                        case "GUYED1P":
                            MyTools.lisansfiyatlari.GUYED1P = (decimal)lfiyat.Fiyat;
                            break;

                        case "GUYED2":
                            MyTools.lisansfiyatlari.GUYED2 = (decimal)lfiyat.Fiyat;
                            break;

                        case "GUYEPVA":
                            MyTools.lisansfiyatlari.GUYEPVA = (decimal)lfiyat.Fiyat;
                            break;

                        case "GUYEPITE":
                            MyTools.lisansfiyatlari.GUYEPITE = (decimal)lfiyat.Fiyat;
                            break;

                        case "Spot Paket":
                            MyTools.lisansfiyatlari.SpotNonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.SpotPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "DJI":
                            MyTools.lisansfiyatlari.DJINonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.DJIPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "SPX":
                            MyTools.lisansfiyatlari.SPINonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.SPIPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "XETRA":
                            MyTools.lisansfiyatlari.XETRANonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.XETRAPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "CMEM":
                            MyTools.lisansfiyatlari.CMEMNonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.CMEMPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "CBOTM":
                            MyTools.lisansfiyatlari.CBOTMNonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.CBOTMPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "CBOT":
                            MyTools.lisansfiyatlari.CBOTNonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.CBOTPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "CME":
                            MyTools.lisansfiyatlari.CMENonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.CMEPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        case "EUREX":
                            MyTools.lisansfiyatlari.EUREXNonPro = (decimal)lfiyat.Fiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            MyTools.lisansfiyatlari.EUREXPro = (decimal)lfiyat.YurtDisiFiyat * (decimal)lfiyat.ParaBirimi.Kur;
                            break;
                        default:
                            break;

                    }

                }

                #endregion
                gridGuncelle();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void gridGuncelle()
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                var sonuc = crm.LisansFiyatlaris.Select(x=> new {x.id,x.LisansKod,x.Fiyat,x.YurtDisiFiyat,x.Aciklama});
                if (sonuc != null)
                    dataGridView1.DataSource = sonuc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            activeitem = 0;
            txtAciklama.Text = "";
            txtFiyat.Text = "";
            txtLisansKod.Text = "";
            txtYurtDisiFiyat.Text = "";         

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {

            crmDFNDataContext crm = new crmDFNDataContext();
            if (dataGridView1.SelectedRows == null)
                return;

            var row = dataGridView1.CurrentRow;
            activeitem = Int32.Parse(row.Cells[0].Value.ToString());

            var lbilgi = crm.LisansFiyatlaris.FirstOrDefault(x => x.id == activeitem);


            txtLisansKod.Text = lbilgi.LisansKod;
            txtAciklama.Text = lbilgi.Aciklama;
            txtFiyat.Text = ((decimal)lbilgi.Fiyat).ToString("0.00") ;
            txtYurtDisiFiyat.Text = ((decimal)lbilgi.YurtDisiFiyat).ToString("0.00");
            comboParaBirimi.SelectedValue = lbilgi.ParaBirimiId;

        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (var crm = new crmDFNDataContext())
                {

                    var rows = crm.LisansFiyatlaris.Select(x => new { x.id, x.LisansKod, x.Fiyat, x.YurtDisiFiyat, ParaBirimiKod = x.ParaBirimi.ParaBirimiKod, x.Aciklama, }).ToList();

                    if (rows.Count == 0)
                    {
                        MessageBox.Show("Aktarılacak kayıt bulunamadı.");
                        return;
                    }

                    using (var sfd = new SaveFileDialog
                    {
                        Title = "Lisans Fiyatları Dışa Aktar",
                        Filter = "CSV dosyası|*.csv",
                        FileName = $"LisansFiyatlari_{DateTime.Now:yyyyMMdd_HHmm}.csv"
                    })
                    {
                        if (sfd.ShowDialog(this) != DialogResult.OK) return;


                        using (var sw = new System.IO.StreamWriter(sfd.FileName, false, Encoding.UTF8))
                        {
                            string[] notes =
                            {
                                "# KULLANIM NOTLARI:",
                                "# 1) Başlıkları DEĞİŞTİRMEYİN. Veri başlığı: id;LisansKod;Fiyat;YurtDisiFiyat;ParaBirimiKod;Aciklama",
                                "# 2) Ondalık ayırıcı olarak NOKTA kullanın (örn: 12.34).",
                                "# 3) id DOLU ise GÜNCELLE, boş/0 ise ve LisansKod yeni ise EKLE yapılır.",
                                "# 4) ParaBirimiKod: sistemde kayıtlı kodlar (örn: TRY, USD, EUR).",
                                "# 5) Satır silmek DB'de silme işlemi yapmaz. (Bu içe aktarma sadece ekle/güncelle yapar.)",
                                "# 6) Bu satırlar (# ile başlayanlar) içe aktarma sırasında otomatik atlanır.",
                                "# ---"
                            };
                            foreach (var line in notes) sw.WriteLine(line);
                            sw.WriteLine("id;LisansKod;Fiyat;YurtDisiFiyat;ParaBirimiKod;Aciklama");
                            foreach (var r in rows)
                            {

                                var fiyatStr = Convert.ToString(r.Fiyat, System.Globalization.CultureInfo.InvariantCulture);
                                var ydfStr = Convert.ToString(r.YurtDisiFiyat, System.Globalization.CultureInfo.InvariantCulture);


                                string Safe(string s) => (s ?? "").Replace(";", ",").Replace("\r", " ").Replace("\n", " ");

                                sw.WriteLine($"{r.id};{r.LisansKod};{fiyatStr};{ydfStr};{r.ParaBirimiKod};{Safe(r.Aciklama)}");
                            }
                        }

                        MessageBox.Show("Dışa aktarma tamamlandı.");
                    }
                }
            }
            catch (Exception ex)
            {
                MyTools.logyaz($"formLisansFiyatlari BtnExportExcel_Click Hata: {ex.Message}");
                MyTools.logyaz(ex.StackTrace);
                //  MessageBox.Show("Dışa aktarma hatası: " + ex.Message);
            }
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog
                {
                    Title = "Lisans Fiyatları İçe Aktar",
                    Filter = "CSV dosyası|*.csv"
                })
                {
                    if (ofd.ShowDialog(this) != DialogResult.OK) return;

                    // Dosyayı oku (UTF-8)
                    var lines = System.IO.File.ReadAllLines(ofd.FileName, Encoding.UTF8)
                                              .Where(l => !string.IsNullOrWhiteSpace(l))
                                              .ToList();

                    if (lines.Count == 0)
                    {
                        MessageBox.Show("Dosyada satır bulunamadı.");
                        return;
                    }

                    // # ile başlayan satırları geçerek İLK GEÇERLİ BAŞLIK SATIRINI bul
                    int headerIndex = -1;
                    for (int i = 0; i < lines.Count; i++)
                    {
                        var t = lines[i].Trim();
                        if (t.StartsWith("#")) continue;  // yorum satırı
                        headerIndex = i;                  // başlık burada
                        break;
                    }

                    if (headerIndex == -1)
                    {
                        MessageBox.Show("Geçerli başlık satırı bulunamadı.");
                        return;
                    }

                    var header = lines[headerIndex].Trim();
                    var expected = "id;LisansKod;Fiyat;YurtDisiFiyat;ParaBirimiKod;Aciklama";
                    if (!header.Equals(expected, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Başlık beklenen formatta değil.\nBeklenen: " + expected + "\nBulunan: " + header);
                        return;
                    }

                    
                    var dataLines = lines.Skip(headerIndex + 1);

                    int updated = 0, inserted = 0, skipped = 0;

                    using (var crm = new crmDFNDataContext())
                    {
                        
                        var pbDict = crm.ParaBirimis
                                        .ToDictionary(p => p.ParaBirimiKod.Trim().ToUpper(), p => p);

                        //prformans için 
                        var all = crm.LisansFiyatlaris.ToList();

                        var idIndex = all.ToDictionary(x => x.id, x => x);
                        var kodIndex = all
                            .GroupBy(x => (x.LisansKod ?? "").Trim().ToUpper())
                            .ToDictionary(g => g.Key, g => g.First());

                        var inv = System.Globalization.CultureInfo.InvariantCulture;

                        foreach (var rawLine in dataLines)
                        {
                            var line = rawLine.Trim();
                            if (line.Length == 0 || line.StartsWith("#")) continue; // boş/yorum satırı

                            var parts = line.Split(';');
                            if (parts.Length < 6) { skipped++; continue; }

                            string sid = parts[0].Trim();
                            string kod = parts[1].Trim();
                            string sf = parts[2].Trim();
                            string syf = parts[3].Trim();
                            string pbKod = parts[4].Trim().ToUpper();
                            string ack = parts[5].Trim();

                            if (string.IsNullOrEmpty(kod)) { skipped++; continue; }

                            if (!decimal.TryParse(sf, System.Globalization.NumberStyles.Any, inv, out var fiyat))
                            { skipped++; continue; }

                            if (!decimal.TryParse(syf, System.Globalization.NumberStyles.Any, inv, out var yDisiFiyat))
                            { skipped++; continue; }

                            if (!pbDict.TryGetValue(pbKod, out var pb))
                            {
                                skipped++; continue;
                            }

                            int id = 0;
                            int.TryParse(sid, out id);

                            LisansFiyatlari entity = null;

                            if (id > 0 && idIndex.TryGetValue(id, out var byId))
                            {
                                entity = byId; 
                            }
                            else
                            {
                                var key = (kod ?? "").Trim().ToUpper();
                                if (kodIndex.TryGetValue(key, out var byKod))
                                    entity = byKod;
                            }

                            if (entity == null)
                            {
                                entity = new LisansFiyatlari
                                {
                                    LisansKod = kod,
                                    Aciklama = ack,
                                    Fiyat = fiyat,
                                    YurtDisiFiyat = yDisiFiyat,
                                    ParaBirimiId = pb.id
                                };
                                crm.LisansFiyatlaris.InsertOnSubmit(entity);
                                inserted++;

                                all.Add(entity);
                                var k = (kod ?? "").Trim().ToUpper();
                                if (!kodIndex.ContainsKey(k)) kodIndex.Add(k, entity);
                            }
                            else
                            {
                                
                                entity.LisansKod = kod;
                                entity.Aciklama = ack;
                                entity.Fiyat = fiyat;
                                entity.YurtDisiFiyat = yDisiFiyat;
                                
                                if (entity.ParaBirimiId != null)
                                    entity.ParaBirimi = pb;
                                else
                                    entity.ParaBirimiId = pb.id;

                                updated++;
                            }
                        }

                        crm.SubmitChanges();
                    }

                    gridGuncelle();

                    MessageBox.Show($"İçe aktarma tamamlandı.\nGüncellenen: {updated}\nEklenen: {inserted}\nAtlanan: {skipped}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("İçe aktarma hatası: " + ex.Message);
            }
        }
    }
}
