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
    public partial class formCalisancs : Form
    {
        public formCalisancs()
        {
            InitializeComponent();
        }

        public int activeCasisanId = 0;
        private void formCalisancs_Load(object sender, EventArgs e)
        {
            try
            {
                gridGuncelle();

                crmDFNDataContext crm = new crmDFNDataContext();


                cmbCalisanSube.Items.Clear();
                var subes = crm.KurumSubes.Where(x => x.MusteriNo == "10158").OrderBy(x => x.SubeAdi).Select(x => x.SubeAdi).ToArray();
                cmbCalisanSube.Items.AddRange(subes.ToArray());


                comboDepartman.DataSource = crm.Departmans;
                comboDepartman.DisplayMember = "DepartmanAdi";
                comboDepartman.ValueMember = "DepartmanId";

                comboYetki.DataSource = crm.Yetkis;
                comboYetki.DisplayMember = "YetkiAdi";
                comboYetki.ValueMember = "yetkiID";

                comboType.DataSource = crm.CalisanTips;
                comboType.DisplayMember = "CalisanTipAdi";
                comboType.ValueMember = "id";

                comboDepartman.SelectedIndex = -1;
                comboYetki.SelectedIndex = -1;
                comboType.SelectedIndex = -1;
                comboDurum.SelectedIndex = -1;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void GridToForm(int id)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                var calisan = crm.Calisans.FirstOrDefault(x => x.calisanID == id);

                txtAdi.Text = (calisan.Ad == null) ? "" : calisan.Ad;
                txtSoyad.Text = (calisan.Soyad == null) ? "" : calisan.Soyad;
                txtUsername.Text = (calisan.UserName == null) ? "" : calisan.UserName;
                txtPassword.Text = (calisan.Password == null) ? "" : MyTools.Sifreleme.Decryp(calisan.Password);
                txtKurumNo.Text = (calisan.kurumMutserino == null) ? "" : calisan.kurumMutserino;
                cmbCalisanSube.Text = (calisan.KurumSube == null) ? "" : calisan.KurumSube; //txtCalisanSube.Text
                txtTemsilciKodu.Text = (calisan.TemsilciKodu == null) ? "" : calisan.TemsilciKodu;
                if (calisan.sifrehata > 3)
                    chkBloke.Checked = true;
                else
                    chkBloke.Checked = false;
                if (calisan.DepartmanId != null)
                    comboDepartman.SelectedValue = calisan.DepartmanId;
                else
                    comboDepartman.SelectedIndex = -1;
                if (calisan.YetiID != null)
                    comboYetki.SelectedValue = calisan.YetiID;
                else
                    comboYetki.SelectedIndex = -1;
                if (calisan.CalisanTipId != null)
                    comboType.SelectedValue = calisan.CalisanTipId;
                else
                    comboType.SelectedIndex = -1;

                if (calisan.CalisanDurum != null)
                {
                    if (calisan.CalisanDurum == true)
                        comboDurum.SelectedIndex = 0;
                    else
                        comboDurum.SelectedIndex = 1;

                }
                else comboDurum.SelectedIndex = -1;

                if (calisan.iletsimId != 0)
                {
                    txtMail.Text = calisan.Iletisim.email;
                }

                //if (txtUsername.Text == "Admin" || txtUsername.Text == "ADMIN" || txtUsername.Text == "ADMİN" || txtUsername.Text == "admin")
                //{
                txtUsername.Enabled = false;
                //}
                //else
                //    txtUsername.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
                return;

            var row = dataGridView1.CurrentRow;
            activeCasisanId = Int32.Parse(row.Cells[7].Value.ToString());
            GridToForm(activeCasisanId);
            var calisanYetkiAdi = MyTools.ActiveCalisan.Yetki.YetkiAdi;
            var calisanYetkiId = MyTools.ActiveCalisan.YetiID;
            if (calisanYetkiAdi=="Admin" && calisanYetkiId==1)
            {
                txtUsername.Enabled = true;
            }
        }
        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            if (activeCasisanId>0)
            {
                var currentUser = crm.Calisans.FirstOrDefault(x => x.calisanID == activeCasisanId);
                if (currentUser == null)
                {
                    MessageBox.Show("Çalışan bulunamadı.");
                    return;
                }
                Calisan cal = new Calisan();

                cal.Ad = txtAdi.Text;
                cal.Soyad = txtSoyad.Text;
                cal.UserName = txtUsername.Text;
                cal.TemsilciKodu = txtTemsilciKodu.Text;
                cal.kurumMutserino = txtKurumNo.Text;
                cal.KurumSube = cmbCalisanSube.Text; // txtCalisanSube.Text;
                cal.tel = txtTelefon.Text;
                if (chkBloke.Checked == false)
                    cal.sifrehata = 0;


                // iletişim ekle

                Iletisim il = new Iletisim();
                il.email = txtMail.Text;
                crm.Iletisims.InsertOnSubmit(il);
                crm.SubmitChanges();

                cal.iletsimId = il.IletisimId;

                cal.Password = MyTools.Sifreleme.Encryp(txtPassword.Text);

                if (comboDurum.SelectedIndex == 0)
                    cal.CalisanDurum = true;
                else
                    cal.CalisanDurum = false;

                if (comboDepartman.SelectedIndex < 0)
                { MessageBox.Show("Bir derpartman Seçiniz"); return; }
                if (comboYetki.SelectedIndex < 0)
                { MessageBox.Show("Çalışan için bir yetki seçiniz"); return; }
                cal.DepartmanId = (int)comboDepartman.SelectedValue;
                cal.YetiID = (int)comboYetki.SelectedValue;
                cal.CalisanTipId = (int)comboType.SelectedValue;

                if (crm.Calisans.Where(x => x.UserName == txtUsername.Text.Trim()).Any())
                {
                    MessageBox.Show("Bu Username Başka Kullanıcı Tarafından Kullanılmaktadır.\n Lütfen Başka Bir Kullanıcı ismi giriniz.");
                    return;
                }
                crm.Calisans.InsertOnSubmit(cal);
                crm.SubmitChanges();

                if (cal.calisanID > 0)
                {
                    MessageBox.Show(cal.Ad + " " + cal.Soyad + "   Kaydedildi");
                }
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            activeCasisanId = 0;
            txtAdi.Text = string.Empty;
            txtSoyad.Text = string.Empty;
            if (txtUsername.Enabled == false) txtUsername.Enabled = true;
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtMail.Text = string.Empty;
            txtTelefon.Text = string.Empty;
            txtKurumNo.Text = MyTools.KurumKod;
            txtKurumNo.Enabled = false;
            cmbCalisanSube.Text = string.Empty; //txtCalisanSube.Text
            txtTemsilciKodu.Text = string.Empty;
            comboDepartman.SelectedIndex = -1;
            comboYetki.SelectedIndex = -1;
            comboType.SelectedIndex = -1;
            comboDurum.SelectedIndex = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            crmDFNDataContext crm = new crmDFNDataContext();



            if (activeCasisanId == 0)
            {
                if (txtAdi.Text == string.Empty)
                { MessageBox.Show("Çalışanın Adı Boş Bırakılamaz"); return; }
                if (txtSoyad.Text == string.Empty)
                { MessageBox.Show("Çalışanın Soyadı Boş Bırakılamaz"); return; }
                if (txtUsername.Text == string.Empty)
                { MessageBox.Show("Çalışanın UserName bilgisi Boş Bırakılamaz"); return; }
                if (txtPassword.Text == string.Empty)
                { MessageBox.Show("Çalışanın Şifre bilgisi Boş Bırakılamaz"); return; }
                if (comboDurum.SelectedIndex == -1)
                { MessageBox.Show("Çalışanının Durumunu seçiniz"); return; }

                Calisan cal = new Calisan();

                cal.Ad = txtAdi.Text;
                cal.Soyad = txtSoyad.Text;
                cal.UserName = txtUsername.Text;
                cal.TemsilciKodu = txtTemsilciKodu.Text;
                cal.kurumMutserino = txtKurumNo.Text;
                cal.KurumSube = cmbCalisanSube.Text; // txtCalisanSube.Text;
                cal.tel = txtTelefon.Text;
                if (chkBloke.Checked == false)
                    cal.sifrehata = 0;


                // iletişim ekle

                Iletisim il = new Iletisim();
                il.email = txtMail.Text;
                crm.Iletisims.InsertOnSubmit(il);
                crm.SubmitChanges();

                cal.iletsimId = il.IletisimId;

                cal.Password = MyTools.Sifreleme.Encryp(txtPassword.Text);

                if (comboDurum.SelectedIndex == 0)
                    cal.CalisanDurum = true;
                else
                    cal.CalisanDurum = false;

                if (comboDepartman.SelectedIndex < 0)
                { MessageBox.Show("Bir derpartman Seçiniz"); return; }
                if (comboYetki.SelectedIndex < 0)
                { MessageBox.Show("Çalışan için bir yetki seçiniz"); return; }
                cal.DepartmanId = (int)comboDepartman.SelectedValue;
                cal.YetiID = (int)comboYetki.SelectedValue;
                cal.CalisanTipId = (int)comboType.SelectedValue;

                if (crm.Calisans.Where(x => x.UserName == txtUsername.Text.Trim()).Any())
                {
                    MessageBox.Show("Bu Username Başka Kullanıcı Tarafından Kullanılmaktadır.\n Lütfen Başka Bir Kullanıcı ismi giriniz.");
                    return;
                }
                crm.Calisans.InsertOnSubmit(cal);
                crm.SubmitChanges();

                if (cal.calisanID > 0)
                {
                    MessageBox.Show(cal.Ad + " " + cal.Soyad + "   Kaydedildi");
                }
            }
            else
            {
                var cal = crm.Calisans.FirstOrDefault(x => x.calisanID == activeCasisanId);
                if (cal != null)
                {
                    if (cal.UserName != txtUsername.Text)
                    {
                        var calisanUsernameIsExist = crm.Calisans.FirstOrDefault(x => x.UserName == txtUsername.Text);
                        if (calisanUsernameIsExist != null)
                        {
                            MessageBox.Show($"{txtUsername.Text} isimli Username kayıtlı. Başka bir Username belirleyiniz.");
                            return;
                        }
                    }
                   
                    if (MessageBox.Show(cal.Ad + " " + cal.Soyad + " Değiştirilecek", "Değiştirme İşlemi", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        if (txtAdi.Text == string.Empty)
                        { MessageBox.Show("Çalışanın Adı Boş Bırakılamaz"); return; }
                        if (txtSoyad.Text == string.Empty)
                        { MessageBox.Show("Çalışanın Soyadı Boş Bırakılamaz"); return; }
                        if (txtUsername.Text == string.Empty)
                        { MessageBox.Show("Çalışanın UserName bilgisi Boş Bırakılamaz"); return; }
                        if (txtPassword.Text == string.Empty)
                        { MessageBox.Show("Çalışanın Şifre bilgisi Boş Bırakılamaz"); return; }
                        //if (crm.Calisans.Where(x => x.UserName == txtUsername.Text.Trim()).Any())
                        //{
                        //    MessageBox.Show("Bu Username Başka Kullanıcı Tarafından Kullanılmaktadır.\n Lütfen Başka Bir Kullanıcı ismi giriniz.");
                        //    return;
                        //}

                        cal.Ad = txtAdi.Text;
                        cal.Soyad = txtSoyad.Text;
                        cal.UserName = txtUsername.Text;
                        cal.TemsilciKodu = txtTemsilciKodu.Text;
                        cal.kurumMutserino = txtKurumNo.Text;
                        cal.KurumSube = cmbCalisanSube.Text; //txtCalisanSube.Text;
                        if (chkBloke.Checked == false)
                            cal.sifrehata = 0;
                        cal.tel = txtTelefon.Text;

                        if (cal.iletsimId != null)
                        {
                            cal.Iletisim.email = txtMail.Text;
                        }
                        else
                        {
                            Iletisim ilet = new Iletisim();
                            ilet.email = txtMail.Text;

                            crm.Iletisims.InsertOnSubmit(ilet);
                            crm.SubmitChanges();

                            cal.iletsimId = ilet.IletisimId;
                        }
                        cal.Password = MyTools.Sifreleme.Encryp(txtPassword.Text);

                        if (comboDepartman.SelectedIndex < 0)
                        { MessageBox.Show("Bir derpartman Seçiniz"); return; }
                        if (comboYetki.SelectedIndex < 0)
                        { MessageBox.Show("Çalışan için bir yetki seçiniz"); return; }
                        cal.DepartmanId = (int)comboDepartman.SelectedValue;
                        cal.YetiID = (int)comboYetki.SelectedValue;
                        cal.CalisanTipId = (int)comboType.SelectedValue;

                        if (comboDurum.SelectedIndex == 0)
                            cal.CalisanDurum = true;
                        else
                            cal.CalisanDurum = false;

                        crm.SubmitChanges();

                        if (cal.calisanID > 0)
                        {
                            MessageBox.Show(cal.Ad + " " + cal.Soyad + "   Değiştirildi");
                        }
                    }
                }
                else MessageBox.Show("Kullanıcının Id si Bulunamadı");
            }
            gridGuncelle();
        }

        //private void btnSil_Click(object sender, EventArgs e)
        //{
        //    if (activeCasisanId == 0)
        //    {
        //        { MessageBox.Show("Silmek İçin Bir Çalışan Seçiniz"); return; }
        //    }
        //    else
        //    {
        //        //crmDFNDataContext crm = new crmDFNDataContext();
        //        //var cal = crm.Calisans.FirstOrDefault(x => x.calisanID == activeCasisanId);
        //        //if (cal != null)
        //        //{
        //        //    if (MessageBox.Show(cal.Ad + " " + cal.Soyad + " Silinecek", "Silme İşlemi", MessageBoxButtons.OKCancel)== DialogResult.OK)
        //        //    {
        //        //        crm.Calisans.DeleteOnSubmit(cal);
        //        //        crm.SubmitChanges();
        //        //        gridGuncelle();
        //        //    }
        //        //}
        //    }
        //}
        public void gridGuncelle()
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            var query = crm.Calisans.AsQueryable();
            string ara = txtAra.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(ara))
            {
                query = query.Where(x =>
                   x.Ad.ToLower().Contains(ara) ||
                   x.Soyad.ToLower().Contains(ara) ||
                   x.UserName.ToLower().Contains(ara) ||
                   x.Password.ToLower().Contains(ara) ||
                   x.Departman.DepartmanAdi.ToLower().Contains(ara) ||
                   x.Yetki.YetkiAdi.ToLower().Contains(ara) ||
                   x.Iletisim.email.ToLower().Contains(ara) ||
                   // x.Iletisim.Ceptel.ToLower().Contains(ara) ||
                   // SqlFunctions.StringConvert((double)x.calisanID).Trim().Contains(ara) ||
                   x.calisanID.ToString().Contains(ara) ||
                   x.tel.ToLower().Contains(ara) ||
                   x.kurumMutserino.ToLower().Contains(ara) ||
                   x.KurumSube.ToLower().Contains(ara) ||
                   x.TemsilciKodu.ToLower().Contains(ara)
               );
            }

            var sonuc = query.Select(x => new
            {
                x.Ad,
                x.Soyad,
                x.UserName,
                x.Password,
                x.Departman.DepartmanAdi,
                x.Yetki.YetkiAdi,
                x.Iletisim.email,
                x.calisanID,
                x.tel,
                x.kurumMutserino
            });

            dataGridView1.DataSource = sonuc.ToList();
        }

        private void txtAra_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    gridGuncelle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void formCalisancs_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    gridGuncelle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chbTumCalisanlar_CheckedChanged(object sender, EventArgs e)
        {
            if (chbTumCalisanlar.Checked==true)
            {
                chbTumCalisanlar.Enabled = false;
                chbAktif.Enabled = true;
                chbPasif.Enabled = true;

                chbAktif.Checked = false;
                chbPasif.Checked = false;

                gridGuncelle();
            }
            
        }

        private void chbAktif_CheckedChanged(object sender, EventArgs e)
        {
            if (chbAktif.Checked == true)
            {
                chbAktif.Enabled = false;
                chbPasif.Enabled = true;
                chbTumCalisanlar.Enabled = true;

                chbTumCalisanlar.Checked = false;
                chbPasif.Checked = false;

                crmDFNDataContext crm = new crmDFNDataContext();
                var sonuc = crm.Calisans.Where(x=>x.CalisanDurum==true).Select(x => new { x.Ad, x.Soyad, x.UserName, x.Password, x.Departman.DepartmanAdi, x.Yetki.YetkiAdi, x.Iletisim.email, x.calisanID, x.KurumSube, });
                if (txtAra.Text.Trim() != "")
                {

                    sonuc = crm.Calisans.Where(x => x.Ad.StartsWith(txtAra.Text.Trim())
                    || x.Ad.StartsWith(txtAra.Text.Trim())
                    || x.kurumMutserino.StartsWith(txtAra.Text.Trim())
                    || x.UserName.StartsWith(txtAra.Text.Trim())
                    || x.TemsilciKodu.StartsWith(txtAra.Text)//.Trim()
                    || x.KurumSube.StartsWith(txtAra.Text.Trim())).Select(x => new { x.Ad, x.Soyad, x.UserName, x.Password, x.Departman.DepartmanAdi, x.Yetki.YetkiAdi, x.Iletisim.email, x.calisanID, x.KurumSube, });

                }
                if (sonuc != null)
                    dataGridView1.DataSource = sonuc;
            }
        }

        private void chbPasif_CheckedChanged(object sender, EventArgs e)
        {
            if (chbPasif.Checked == true)
            {

                chbPasif.Enabled = false;
                chbAktif.Enabled = true;
                chbTumCalisanlar.Enabled = true;
                
                chbTumCalisanlar.Checked = false;
                chbAktif.Checked = false;

                //Pasif çalışanları grid'de göster
                crmDFNDataContext crm = new crmDFNDataContext();
                var sonuc = crm.Calisans.Where(x => x.CalisanDurum == false).Select(x => new { x.Ad, x.Soyad, x.UserName, x.Password, x.Departman.DepartmanAdi, x.Yetki.YetkiAdi, x.Iletisim.email, x.calisanID, x.KurumSube, });
                if (txtAra.Text.Trim() != "")
                {

                    sonuc = crm.Calisans.Where(x => x.Ad.StartsWith(txtAra.Text.Trim())
                    || x.Ad.StartsWith(txtAra.Text.Trim())
                    || x.kurumMutserino.StartsWith(txtAra.Text.Trim())
                    || x.UserName.StartsWith(txtAra.Text.Trim())
                    || x.TemsilciKodu.StartsWith(txtAra.Text)//.Trim()
                    || x.KurumSube.StartsWith(txtAra.Text.Trim())).Select(x => new { x.Ad, x.Soyad, x.UserName, x.Password, x.Departman.DepartmanAdi, x.Yetki.YetkiAdi, x.Iletisim.email, x.calisanID, x.KurumSube, });

                }
                if (sonuc != null)
                    dataGridView1.DataSource = sonuc;
            }
        }
    }
}
