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
    public partial class formProjeler : Form
    {
        public formProjeler()
        {
            InitializeComponent();
        }

  
        public int activeitem = 0;    

        private void formProjeler_Load(object sender, EventArgs e)
        {

            try
            {
                




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

            var secili = dataGridView1.Rows[e.RowIndex].Cells[5].Value;
            //dataGridView1.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex+1).ToString();
            if (secili != null)
            {
                if ((bool)secili)
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                else
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;

            }
        }

        public void Ara()
        {

            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                var sonuc = crm.Projelers.Where(x => x.MusteriNo != "");

                if (this.Tag != null)
                {
                    var id = (int)this.Tag;
                    sonuc = sonuc.Where(x => x.id == id);
                }


                if (txtkriter1.Text != "")
                {
                    sonuc = sonuc.Where(x => x.KullaniciAdi.Contains(txtkriter1.Text)
                    || x.MusteriNo.Contains(txtkriter1.Text)
                     || x.ProjeAdi.Contains(txtkriter1.Text)
                       || x.Aciklama.Contains(txtkriter1.Text)
                    );               
                              
                }

                if (txtkriter2.Text != "")
                {
                    sonuc = sonuc.Where(x => x.KullaniciAdi.Contains(txtkriter1.Text)
                    || x.MusteriNo.Contains(txtkriter2.Text)
                     || x.ProjeAdi.Contains(txtkriter2.Text)
                       || x.Aciklama.Contains(txtkriter2.Text)
                    );

                }


                if (txtkriter3.Text != "")
                {
                    sonuc = sonuc.Where(x => x.KullaniciAdi.Contains(txtkriter1.Text)
                    || x.MusteriNo.Contains(txtkriter3.Text)
                     || x.ProjeAdi.Contains(txtkriter3.Text)
                       || x.Aciklama.Contains(txtkriter3.Text)
                    );

                }




                dataGridView1.DataSource = sonuc.Select(x => new { x.id, x.MusteriNo, x.KullaniciAdi, x.ProjeAdi, x.Aciklama,YayinDurum = x.yayinDurum.Value });

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

                var proje = crm.Projelers.FirstOrDefault(x => x.id == activeitem);

                if (proje.KullaniciAdi != null) txtKullaniciAd.Text = proje.KullaniciAdi;                        
                comboProjeUrun.Text = proje.ProjeAdi;
                txtAciklama.Text = proje.Aciklama;
                txtFiyat.Text = ((decimal)proje.Fiyat).ToString("0.00");
                txtPmtsno.Text = proje.MusteriNo;


       
                var sorgu = crm.Sozlesmelers.Where(x => x.MusteriNo == txtPmtsno.Text);
                comboSozlesme.DataSource = sorgu;
                comboSozlesme.DisplayMember = "SozlesmeNo";
                comboSozlesme.ValueMember = "SozlesmeilID";

                if (proje.SozlesmeID != null)
                    comboSozlesme.SelectedValue = (int)proje.SozlesmeID;
                else
                    comboSozlesme.SelectedIndex = -1;


                if (proje.yayinDurum.Value)                
                    comboYayinDurumu.SelectedIndex = 0;             
                else
                  comboYayinDurumu.SelectedIndex = 1;

                if (proje.BorsaLisansId != null)
                {
                    chkGKULD1P.Checked = proje.BorsaAltLisansar.GKULD1P.Value;
                    chkGKULD2.Checked = proje.BorsaAltLisansar.GKULD2.Value;
                    chkGKULEND.Checked = proje.BorsaAltLisansar.GKULEND.Value;
                    chkGKULPVA.Checked = proje.BorsaAltLisansar.GKULPVA.Value;

                    chkGUYED1P.Checked = proje.BorsaAltLisansar.GUYED1P.Value;
                    chkGUYED2.Checked = proje.BorsaAltLisansar.GUYED2.Value;
                    chkGUYEEND.Checked = proje.BorsaAltLisansar.GUYEEND.Value;
                    chkGUYEPVA.Checked = proje.BorsaAltLisansar.GUYEPVA.Value;



                }
                else
                {
                    chkGKULD1P.Checked = false;
                    chkGKULD2.Checked = false;
                    chkGKULEND.Checked = false;
                    chkGKULPVA.Checked = false;
                    chkGUYED1P.Checked = false;
                    chkGUYED2.Checked = false;
                    chkGUYEEND.Checked = false;
                    chkGUYEPVA.Checked = false;
                }
             
                
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }


        public bool lisanssecildimi()
        {
            try
            {
                var sonuc = false;
                if (chkGKULD1P.Checked)
                    sonuc = true;
                if (chkGKULD2.Checked)
                    sonuc = true;
                if (chkGKULEND.Checked)
                    sonuc = true;
                if (chkGKULPVA.Checked)
                    sonuc = true;

                if (chkGUYED1P.Checked)
                    sonuc = true;
                if (chkGUYED2.Checked)
                    sonuc = true;
                if (chkGUYEEND.Checked)
                    sonuc = true;
                if (chkGUYEPVA.Checked)
                    sonuc = true;


                return sonuc;

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
                return false;
            }


        }


        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                if (txtPmtsno.Text == "")
                {
                    MessageBox.Show("Müşteri Numarasını Boş Bırakamazsınız");
                    return;
                }

                if (comboProjeUrun.Text == "")
                {
                    MessageBox.Show("Müşteri adını Boş Bırakamazsınız");
                    return;
                }
                if (txtKullaniciAd.Text == "")
                {
                    MessageBox.Show("Kullanıcı adını Boş Bırakamazsınız");
                    return;
                }

                if (activeitem == 0)
                {
                    var proje = new Projeler();

                    var yayindurumu = false;
                    if (comboYayinDurumu.SelectedIndex == 0)
                        yayindurumu = true;
                    else if (comboYayinDurumu.SelectedIndex == 1)
                        yayindurumu = false;
                    else
                    {
                        MessageBox.Show("yayın Durumunu Seçiniz");
                        return;
                    }


                    proje.yayinDurum = yayindurumu;
                    proje.ProjeAdi = comboProjeUrun.Text;
                    proje.MusteriNo = txtPmtsno.Text;
                    proje.KullaniciAdi = txtKullaniciAd.Text;                  
                    proje.Aciklama = txtAciklama.Text;
                    proje.baslangicTarihi = dtpStartDate.Value;
                    proje.BitisTarihi = dtpEndDate.Value;
                    proje.Fiyat = decimal.Parse(txtFiyat.Text);

                    if(comboSozlesme.SelectedIndex!=-1)
                    proje.SozlesmeID = (int)comboSozlesme.SelectedValue;

                    if (lisanssecildimi())
                    {
                        var lisanslar = new  BorsaAltLisansar();

                        lisanslar.GKULD1P = chkGKULD1P.Checked;
                        lisanslar.GKULD2 = chkGKULD2.Checked;
                        lisanslar.GKULEND = chkGKULEND.Checked;
                        lisanslar.GKULPVA = chkGKULPVA.Checked;

                        lisanslar.GUYED1P = chkGUYED1P.Checked;
                        lisanslar.GUYED2 = chkGUYED2.Checked;
                        lisanslar.GUYEEND = chkGUYEEND.Checked;
                        lisanslar.GUYEPVA = chkGUYEPVA.Checked;

                        crm.BorsaAltLisansars.InsertOnSubmit(lisanslar);
                        crm.SubmitChanges();
                        proje.BorsaLisansId = lisanslar.id;
                    }

                 
                    crm.Projelers.InsertOnSubmit(proje);
                    crm.SubmitChanges();

                    MessageBox.Show("Yani Proje Kaydedildi");

                    Ara();

                }
                else
                {


                    var proje = crm.Projelers.FirstOrDefault(x=>x.id==activeitem);

                    var yayindurumu = false;
                    if (comboYayinDurumu.SelectedIndex == 0)
                        yayindurumu = true;
                    else if (comboYayinDurumu.SelectedIndex == 1)
                        yayindurumu = false;
                    else
                    {
                        MessageBox.Show("yayın Durumunu Seçiniz");
                        return;
                    }


                    proje.yayinDurum = yayindurumu;
                    proje.ProjeAdi = comboProjeUrun.Text;
                    proje.MusteriNo = txtPmtsno.Text;
                    proje.KullaniciAdi = txtKullaniciAd.Text;
                    proje.Aciklama = txtAciklama.Text;
                    proje.baslangicTarihi = dtpStartDate.Value;
                    proje.BitisTarihi = dtpEndDate.Value;
                    proje.Fiyat = decimal.Parse(txtFiyat.Text);

                    if (comboSozlesme.SelectedIndex != -1)
                        proje.SozlesmeID = (int)comboSozlesme.SelectedValue;




                    if (proje.BorsaLisansId != null)
                    {
                        proje.BorsaAltLisansar.GKULD1P = chkGKULD1P.Checked;
                        proje.BorsaAltLisansar.GKULD2 = chkGKULD2.Checked;
                        proje.BorsaAltLisansar.GKULEND = chkGKULEND.Checked;
                        proje.BorsaAltLisansar.GKULPVA = chkGKULPVA.Checked;

                        proje.BorsaAltLisansar.GUYED1P = chkGUYED1P.Checked;
                        proje.BorsaAltLisansar.GUYED2 = chkGUYED2.Checked;
                        proje.BorsaAltLisansar.GUYEEND = chkGUYEEND.Checked;
                        proje.BorsaAltLisansar.GUYEPVA = chkGUYEPVA.Checked;
                    }
                    else
                    {

                        if (lisanssecildimi())
                        {

                            var lisanslar = new BorsaAltLisansar();

                            lisanslar.GKULD1P = chkGKULD1P.Checked;
                            lisanslar.GKULD2 = chkGKULD2.Checked;
                            lisanslar.GKULEND = chkGKULEND.Checked;
                            lisanslar.GKULPVA = chkGKULPVA.Checked;

                            lisanslar.GUYED1P = chkGUYED1P.Checked;
                            lisanslar.GUYED2 = chkGUYED2.Checked;
                            lisanslar.GUYEEND = chkGUYEEND.Checked;
                            lisanslar.GUYEPVA = chkGUYEPVA.Checked;

                            crm.BorsaAltLisansars.InsertOnSubmit(lisanslar);
                            crm.SubmitChanges();
                            proje.BorsaLisansId = lisanslar.id;
                        }
                    }       
                    


                   

                    crm.SubmitChanges();

                    MessageBox.Show("Proje Güncellendi");



                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            try
            {
                if (dataGridView1.SelectedRows.Count < 1)
                    return;

                if (dataGridView1.SelectedRows == null)
                    return;
                var row = dataGridView1.CurrentRow;
                activeitem = Int32.Parse(row.Cells[0].Value.ToString());
                GridToForm(activeitem);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtkriter1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Ara();
            }
        }

        private void txtkriter2_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                Ara();
            }

        }

        private void txtkriter3_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                Ara();
            }
        }    

        private void btnNew_Click(object sender, EventArgs e)
        {
            activeitem = 0;
            txtAciklama.Text = "";
            txtFiyat.Text = "";
            txtKullaniciAd.Text = "";
            txtPmtsno.Text = "";
            comboProjeUrun.Text = "";
            dtpStartDate.Value = DateTime.Now;
            dtpEndDate.Value = DateTime.Now;

        
            comboYayinDurumu.SelectedIndex = 0;

            chkGKULD1P.Checked = false;
            chkGKULD2.Checked = false;
            chkGKULEND.Checked = false;
            chkGKULPVA.Checked = false;
            chkGUYED1P.Checked = false;
            chkGUYED2.Checked = false;
            chkGUYEEND.Checked = false;
            chkGUYEPVA.Checked = false;
       






        }

        private void comboYayinDurumu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboYayinDurumu.SelectedIndex == 0)
                comboYayinDurumu.BackColor = Color.LimeGreen;
            else if (comboYayinDurumu.SelectedIndex == 1)
            {
                comboYayinDurumu.BackColor = Color.Red;
            }

            btnSave.Select();
        }

        private void txtPmtsno_Leave(object sender, EventArgs e)
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            var sorgu = crm.Sozlesmelers.Where(x => x.MusteriNo==txtPmtsno.Text);
            comboSozlesme.DataSource = sorgu;
            comboSozlesme.DisplayMember = "SozlesmeNo";
            comboSozlesme.ValueMember = "SozlesmeilID";
        }
    }
}
