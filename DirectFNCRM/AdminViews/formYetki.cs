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
    public partial class formYetki : Form
    {
        public formYetki()
        {
            InitializeComponent();
        }
        public int activeitem = 0;
        private void formYetki_Load(object sender, EventArgs e)
        {

            gridGuncelle();
        }
        public void gridGuncelle()
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            var sonuc = crm.Yetkis.Select(x => new { x.yetkiID, x.YetkiAdi });
            if (sonuc != null)
                dataGridView1.DataSource = sonuc;

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows == null)
                return;

            var row = dataGridView1.CurrentRow;
            activeitem = Int32.Parse(row.Cells[0].Value.ToString());
            txtAdi.Text = row.Cells[1].Value.ToString();

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            activeitem = 0;
            txtAdi.Text = string.Empty;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            crmDFNDataContext crm = new crmDFNDataContext();
            if (activeitem == 0)
            {
                if (txtAdi.Text == string.Empty)
                { MessageBox.Show("Yetki Adı Boş Bırakılamaz"); return; }


                Yetki cal = new Yetki();

                cal.YetkiAdi = txtAdi.Text;





                crm.Yetkis.InsertOnSubmit(cal);
                crm.SubmitChanges();

                if (cal.yetkiID > 0)
                {
                    MessageBox.Show(cal.YetkiAdi + "   Kaydedildi");
                }


            }
            else
            {


                var cal = crm.Yetkis.FirstOrDefault(x => x.yetkiID == activeitem);
                if (cal != null)
                {
                    if (MessageBox.Show(cal.YetkiAdi + " Değiştirilecek", "Değiştirme İşlemi", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {

                        if (txtAdi.Text == string.Empty)
                        { MessageBox.Show("Yetki Adı Boş Bırakılamaz"); return; }


                        cal.YetkiAdi = txtAdi.Text;
                        crm.SubmitChanges();

                        if (cal.yetkiID > 0)
                        {
                            MessageBox.Show(cal.YetkiAdi + "   Değiştirildi");
                        }

                    }

                }
                else MessageBox.Show("Yetki  Id si Bulunamadı");

            }

            gridGuncelle();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (activeitem == 0)
            {
                { MessageBox.Show("Silmek İçin Bir Yetki Seçiniz"); return; }


            }
            else
            {

                crmDFNDataContext crm = new crmDFNDataContext();
                var cal = crm.Yetkis.FirstOrDefault(x => x.yetkiID == activeitem);


                if (cal != null)
                {
                    if (MessageBox.Show(cal.YetkiAdi + " Silinecek", "Silme İşlemi", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        crm.Yetkis.DeleteOnSubmit(cal);
                        crm.SubmitChanges();
                        gridGuncelle();
                    }

                }
            }
        }
    }
}
