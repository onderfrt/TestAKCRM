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
    public partial class formCepProEsanliKontrol : Form
    {
        crmDFNDataContext crm = new crmDFNDataContext();
        List<HataliKullanici> ProCepEsanliHataliKullanicilar = new List<HataliKullanici>();
        List<HataliKullanici> AltLisansAtamasiEksikKullanicilar = new List<HataliKullanici>();
        List<HataliKullanici> ExpiryDateGecmisKullanicilar = new List<HataliKullanici>();
        List<HataliKullanici> ExpiryDateAySonuOlmayanKullanicilar = new List<HataliKullanici>();
        List<HataliKullanici> TumHataliKullanicilar = new List<HataliKullanici>();


        public formCepProEsanliKontrol()
        {
            InitializeComponent();
            cbxHatalar.SelectedIndex = 0;
        }

        private void formCepProEsanliKontrol_Load(object sender, EventArgs e)
        {
            HataliKullaniciDoldur();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
                return;

            var id = dataGridView1.CurrentRow.Cells[0].Value;

            var user = crm.Users.FirstOrDefault(x => x.UserID == (int)id);

            if (user == null)
                return;

            formUserDetay frm = new formUserDetay();
            frm.Tag = user.UserID;
            frm.Show();
        }

        /// <summary>
        /// Ayın son günü olup olmadığını kontrol eder.
        /// </summary>
        private bool AyinSonGunuMu(DateTime tarih)
        {
            return tarih.Day == DateTime.DaysInMonth(tarih.Year, tarih.Month);
        }

        public void HataliKullaniciDoldur()
        {
            DateTime bugun = DateTime.Today;

            // 1) Pro ve Cep eşanlı kapalı
            ProCepEsanliHataliKullanicilar = crm.Users
                .Where(x => x.LisansDurum.YayinDurumu == true
                         && x.LisansDurum.ProYetki == false
                         && x.LisansDurum.CepYetki == false)
                .Select(x => new HataliKullanici
                {
                    UserID = x.UserID,
                    UserName = x.UserName,
                    ExpiryDate = x.ExpiryDate,
                    YayinDurumu = x.LisansDurum.YayinDurumu,
                    CepYetki = x.LisansDurum.CepYetki,
                    ProYetki = x.LisansDurum.ProYetki,
                    PayL1 = x.LisansDurum.PayL1,
                    PayLP = x.LisansDurum.PayLP,
                    PayL2 = x.LisansDurum.PayL2,
                    Pd2P = x.LisansDurum.Pd2P,
                    ViopL1 = x.LisansDurum.ViopL1,
                    ViopLP = x.LisansDurum.ViopLP,
                    ViopL2 = x.LisansDurum.ViopL2,
                    Vd2P = x.LisansDurum.Vd2P
                }).ToList();

            // 2) Alt lisans ataması eksik
            AltLisansAtamasiEksikKullanicilar = crm.Users
                .Where(x => x.LisansDurum.YayinDurumu == true
                    && ((x.LisansDurum.Pd2P == true && (x.LisansDurum.PayL2 == false || x.LisansDurum.PayLP == false || x.LisansDurum.PayL1 == false))
                     || (x.LisansDurum.PayL2 == true && (x.LisansDurum.PayLP == false || x.LisansDurum.PayL1 == false))
                     || (x.LisansDurum.PayLP == true && x.LisansDurum.PayL1 == false)
                     || (x.LisansDurum.Pd2P == true && x.LisansDurum.PayL1 == false)
                     || (x.LisansDurum.Vd2P == true && (x.LisansDurum.ViopL2 == false || x.LisansDurum.ViopLP == false || x.LisansDurum.ViopL1 == false))
                     || (x.LisansDurum.ViopL2 == true && (x.LisansDurum.ViopLP == false || x.LisansDurum.ViopL1 == false))
                     || (x.LisansDurum.ViopLP == true && x.LisansDurum.ViopL1 == false)
                     || (x.LisansDurum.Vd2P == true && x.LisansDurum.ViopL1 == false)))
                .Select(x => new HataliKullanici
                {
                    UserID = x.UserID,
                    UserName = x.UserName,
                    ExpiryDate = x.ExpiryDate,
                    YayinDurumu = x.LisansDurum.YayinDurumu,
                    CepYetki = x.LisansDurum.CepYetki,
                    ProYetki = x.LisansDurum.ProYetki,
                    PayL1 = x.LisansDurum.PayL1,
                    PayLP = x.LisansDurum.PayLP,
                    PayL2 = x.LisansDurum.PayL2,
                    Pd2P = x.LisansDurum.Pd2P,
                    ViopL1 = x.LisansDurum.ViopL1,
                    ViopLP = x.LisansDurum.ViopLP,
                    ViopL2 = x.LisansDurum.ViopL2,
                    Vd2P = x.LisansDurum.Vd2P
                }).ToList();

            // 3) YayinDurumu açık ama ExpiryDate geçmiş
            ExpiryDateGecmisKullanicilar = crm.Users
                .Where(x => x.LisansDurum.YayinDurumu == true
                         && x.ExpiryDate != null
                         && x.ExpiryDate < bugun)
                .Select(x => new HataliKullanici
                {
                    UserID = x.UserID,
                    UserName = x.UserName,
                    ExpiryDate = x.ExpiryDate,
                    YayinDurumu = x.LisansDurum.YayinDurumu,
                    CepYetki = x.LisansDurum.CepYetki,
                    ProYetki = x.LisansDurum.ProYetki,
                    PayL1 = x.LisansDurum.PayL1,
                    PayLP = x.LisansDurum.PayLP,
                    PayL2 = x.LisansDurum.PayL2,
                    Pd2P = x.LisansDurum.Pd2P,
                    ViopL1 = x.LisansDurum.ViopL1,
                    ViopLP = x.LisansDurum.ViopLP,
                    ViopL2 = x.LisansDurum.ViopL2,
                    Vd2P = x.LisansDurum.Vd2P
                }).ToList();

            // 4) ExpiryDate ay sonuna set edilmemiş (YayinDurumu açık olanlar arasında)
            // LINQ to SQL'de DaysInMonth kullanılamadığı için önce çekip bellekte filtreleriz
            if (chkAySonuGoster.Checked == true)
            {
            var aysonuAdaylari = crm.Users
                .Where(x => x.LisansDurum.YayinDurumu == true
                         && x.ExpiryDate != null && (x.StatusId == 1))
                .Select(x => new HataliKullanici
                {
                    UserID = x.UserID,
                    UserName = x.UserName,
                    ExpiryDate = x.ExpiryDate,
                    YayinDurumu = x.LisansDurum.YayinDurumu,
                    CepYetki = x.LisansDurum.CepYetki,
                    ProYetki = x.LisansDurum.ProYetki,
                    PayL1 = x.LisansDurum.PayL1,
                    PayLP = x.LisansDurum.PayLP,
                    PayL2 = x.LisansDurum.PayL2,
                    Pd2P = x.LisansDurum.Pd2P,
                    ViopL1 = x.LisansDurum.ViopL1,
                    ViopLP = x.LisansDurum.ViopLP,
                    ViopL2 = x.LisansDurum.ViopL2,
                    Vd2P = x.LisansDurum.Vd2P
                }).ToList();

            ExpiryDateAySonuOlmayanKullanicilar = aysonuAdaylari
                .Where(x => x.ExpiryDate.HasValue && !AyinSonGunuMu(x.ExpiryDate.Value))
                .ToList();
            }
            // Tüm hatalıları birleştir (tekrarları kaldırarak)
            TumHataliKullanicilar.Clear();
            TumHataliKullanicilar.AddRange(ProCepEsanliHataliKullanicilar);
            TumHataliKullanicilar.AddRange(AltLisansAtamasiEksikKullanicilar);
            TumHataliKullanicilar.AddRange(ExpiryDateGecmisKullanicilar);
            if (chkAySonuGoster.Checked == true)
                TumHataliKullanicilar.AddRange(ExpiryDateAySonuOlmayanKullanicilar);

            // Aynı UserID birden fazla listede olabilir, tekrarları kaldır
            TumHataliKullanicilar = TumHataliKullanicilar
                .GroupBy(x => x.UserID)
                .Select(g => g.First())
                .ToList();

            // DataGridView'e veri aktarma
            dataGridView1.DataSource = null;
            switch (cbxHatalar.SelectedIndex)
            {
                case 0:
                    dataGridView1.DataSource = TumHataliKullanicilar;
                    lblAciklama.Text = "Tüm hatalı kullanıcılar listelenmektedir.";
                    break;
                case 1:
                    dataGridView1.DataSource = ProCepEsanliHataliKullanicilar;
                    lblAciklama.Text = "Pro ve Cep Lisansları aynı anda kapalı olan kullanıcılar listelenmektedir.";
                    break;
                case 2:
                    dataGridView1.DataSource = AltLisansAtamasiEksikKullanicilar;
                    lblAciklama.Text = "Atanmış lisansın alt lisanslarından herhangi birinde sıkıntı olan kullanıcılar listelenmektedir.";
                    break;
                case 3:
                    dataGridView1.DataSource = ExpiryDateGecmisKullanicilar;
                    lblAciklama.Text = "YayınDurumu açık olmasına rağmen ExpiryDate tarihi geçmiş kullanıcılar listelenmektedir.";
                    break;
                   
                case 4:
                    dataGridView1.DataSource = ExpiryDateAySonuOlmayanKullanicilar;
                    lblAciklama.Text = "ExpiryDate tarihi ayın son gününe set edilmemiş kullanıcılar listelenmektedir.";
                    break;
                default:
                    break;
            }
        }

        private void cbxHatalar_SelectedIndexChanged(object sender, EventArgs e)
        {
            HataliKullaniciDoldur();
        }
    }

    public class HataliKullanici
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool YayinDurumu { get; set; }
        public bool CepYetki { get; set; }
        public bool ProYetki { get; set; }
        public bool PayL1 { get; set; }
        public bool PayLP { get; set; }
        public bool PayL2 { get; set; }
        public bool Pd2P { get; set; }
        public bool ViopL1 { get; set; }
        public bool ViopLP { get; set; }
        public bool ViopL2 { get; set; }
        public bool Vd2P { get; set; }
    }
}