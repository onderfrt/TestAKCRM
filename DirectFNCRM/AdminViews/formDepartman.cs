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
    public partial class formDepartman : Form
    {
        public formDepartman()
        {
            InitializeComponent();
        }

        public int activeitem = 0;
        private void formDepartman_Load(object sender, EventArgs e)
        {


            gridGuncelle();
            
        }

        public void gridGuncelle()
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            var sonuc = crm.Departmans.Select(x => new {x.DepartmanId, x.DepartmanAdi });
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
                { MessageBox.Show("Departman Adı Boş Bırakılamaz"); return; }


                Departman cal = new Departman();

                cal.DepartmanAdi = txtAdi.Text;
             

              


                crm.Departmans.InsertOnSubmit(cal);
                crm.SubmitChanges();

                if (cal.DepartmanId > 0)
                {
                    MessageBox.Show(cal.DepartmanAdi + "   Kaydedildi");
                }


            }
            else
            {


                var cal = crm.Departmans.FirstOrDefault(x => x.DepartmanId == activeitem);
                if (cal != null)
                {
                    if (MessageBox.Show(cal.DepartmanAdi  + " Değiştirilecek", "Değiştirme İşlemi", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {

                        if (txtAdi.Text == string.Empty)
                        { MessageBox.Show("Departman Adı Boş Bırakılamaz"); return; }
                      

                        cal.DepartmanAdi = txtAdi.Text;
                        crm.SubmitChanges();

                        if (cal.DepartmanId > 0)
                        {
                            MessageBox.Show(cal.DepartmanAdi + "   Değiştirildi");
                        }

                    }

                }
                else MessageBox.Show("Departman  Id si Bulunamadı");

            }

            gridGuncelle();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (activeitem == 0)
            {
                { MessageBox.Show("Silmek İçin Bir Departman Seçiniz"); return; }


            }
            else
            {

                crmDFNDataContext crm = new crmDFNDataContext();
                var cal = crm.Departmans.FirstOrDefault(x => x.DepartmanId == activeitem);


                if (cal != null)
                {
                    if (MessageBox.Show(cal.DepartmanAdi+ " Silinecek", "Silme İşlemi", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        crm.Departmans.DeleteOnSubmit(cal);
                        crm.SubmitChanges();
                        gridGuncelle();
                    }

                }
            }
        }
    }
}
