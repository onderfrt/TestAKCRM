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
    public partial class formPushNotification : Form
    {
        public formPushNotification()
        {
            InitializeComponent();
        }

        int calisanid =0;

        int activeduyuruid = 0;

        private void btnMultiMsj_Click(object sender, EventArgs e)
        {
            try
            {
               

                if (rbMultiMesaj.Text.Trim() == "")
                {
                    MessageBox.Show("Gönderilecek Mesaj Boş"); return;
                }

                var crm = new crmDFNDataContext();
                if (rbtnTumKullanici.Checked)
                {
                    if (MessageBox.Show("Mesajınız Tüm kullanıcılara Gönderilecektir Emin misiniz !", "Toplu Bilditim Gönderim !", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                      

                        var mesaj = new Duyurular();

                        mesaj.CalisanId = calisanid;
                        mesaj.Mesaj = rbMultiMesaj.Text.Trim();
                        mesaj.Sembol = txtSembol.Text.Trim();
                        mesaj.tarih = DateTime.Now;

                        crm.Duyurulars.InsertOnSubmit(mesaj);

                        crm.SubmitChanges();
                        formAdminAra.referance.IPport.DataToSend = "PUSH|1|" + mesaj.id.ToString() + (char)3;
                    }
                }
                else if (rbtnTekKullanici.Checked)
                {

                    var user = crm.Users.FirstOrDefault(x => x.UserName == txtHesapno.Text);

                    if (user.FireBaseToken == "")
                    {
                        MessageBox.Show("Bu hesap No için PushNotification gönderilemez"); return;
                    }
                       
                    else
                    {

                        if (rbMultiMesaj.Text.Trim() == "")
                        {
                            MessageBox.Show("Gönderilecek Mesaj Boş"); return;
                        }

                        if (MessageBox.Show("Mesajınız " + user.UserName + "'a Gönderilecektir Emin misiniz !", "Tek Kullanıcı Mesaj Gönderim !", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            // tek mesaj

                            formAdminAra.referance.IPport.DataToSend = "TekMesaj|" + user.UserName + ";" + rbMultiMesaj.Text.Trim() + (char)3;
                            MessageBox.Show("Mesajınız Gönderildi"); return;


                        }
                        else
                        { return; }


                    }


                }






                Ara();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void formPushNotification_Load(object sender, EventArgs e)
        {
            try
            {
                calisanid = (int)this.Tag;



                Ara();
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Ara()
        {
            try
            {

                var crm = new crmDFNDataContext();
                var sorgu = crm.Duyurulars.Where(x=>x.id>0);

                if (txtSearch.Text != "")
                {
                    sorgu = sorgu.Where(x => x.Mesaj.Contains(txtSearch.Text.Trim()));

                }


                dataGridView1.DataSource = sorgu.Select(x => new { x.id, x.tarih, Çalışan = x.Calisan.Ad + " " + x.Calisan.Soyad, x.Mesaj });


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {

                if (activeduyuruid == 0)
                {
                    MessageBox.Show("Düzelmek için bir mesaj secin.");
                    return;
                }
                if (rbMultiMesaj.Text.Trim() == "")
                {
                    MessageBox.Show("Gönderilecek Mesaj Boş"); return;
                }

                var crm = new crmDFNDataContext();
                if (crm.Duyurulars.Where(x => x.id == activeduyuruid).Any())
                {
                    var mesaj = crm.Duyurulars.FirstOrDefault(x => x.id == activeduyuruid);

                    mesaj.Mesaj = rbMultiMesaj.Text.Trim();
                    mesaj.Sembol = txtSembol.Text.Trim();

                    crm.SubmitChanges();

                    formAdminAra.referance.IPport.DataToSend = "PUSH|2|" + mesaj.id.ToString() + (char)3;

                }
                else
                {
                    MessageBox.Show("Mesaj Bulunamadı");
                }


                Ara();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                rbMultiMesaj.Text = "";
                txtSembol.Text = "";
                activeduyuruid = 0;



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Ara();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            try
            {
                if (dataGridView1.SelectedRows.Count < 1) return;

                crmDFNDataContext crm = new crmDFNDataContext();
                var id = (int)dataGridView1.CurrentRow.Cells[0].Value;
                var mesaj = crm.Duyurulars.FirstOrDefault(x => x.id == id);
                activeduyuruid = mesaj.id;
                txtSembol.Text = mesaj.Sembol;
                rbMultiMesaj.Text = mesaj.Mesaj;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
        private void rbtnTumKullanici_Click(object sender, EventArgs e)
        {
            if (rbtnTumKullanici.Checked)
            {
                txtHesapno.Visible = false;
                label2.Visible = false;
            }
            else
            {
                txtHesapno.Visible = true;
                label2.Visible = true;
            }
        }
    }
}
