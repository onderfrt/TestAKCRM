using DirectFNCRM.AdminViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectFNCRM
{
    public partial class formSentimentAlgo : Form
    {
        public static formSentimentAlgo reference;
        public formSentimentAlgo()
        {
            InitializeComponent();

            reference = this;
        }


        
        void formSentimentAlgo_Load(object sender, EventArgs e)
        {
            try
            {

                Ara();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
         
        }

        void Ara()
        {
            try
            {
                var crm = new crmDFNDataContext();
                var source = crm.SentimentAlgos.Where(x => x.Baslik.Contains(txtFiltre.Text.Trim()) || x.Icerik.Contains(txtFiltre.Text.Trim())).OrderByDescending(x=>x.id).Take(50).ToList();
                dataGridView1.DataSource = source.Select(x => new { x.id, OlusturmaTarihi=x.OlusturmaTar.Value.Date, x.Baslik, Olusturan = x.Calisan.Ad+" " +x.Calisan.Soyad }).ToList();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void Guncelle()
        {
            try
            {
                Ara();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void btnAra_Click(object sender, EventArgs e)
        {
            try
            {
                Ara();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new formSentimentAlgoDetay();
                frm.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

         void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.SelectedRows.Count == 0) return;
                var id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                var frm = new formSentimentAlgoDetay();
                frm.Tag = id;
                frm.Show();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

         void formSentimentAlgo_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                reference = null;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Silme işlemi için bir kayıt seçin"); return;
                }

                var crm = new crmDFNDataContext();
                var idx = Int32.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

                if (crm.SentimentAlgos.Where(x => x.id == idx).Any())
                {
                    var sAlgo = crm.SentimentAlgos.FirstOrDefault(x => x.id == idx);
                    if (MessageBox.Show(sAlgo.id+" id numaralı kayıt silinecek emin misiniz ?", "Sentiment Algo Kayıt Silme", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        crm.SentimentAlgos.DeleteOnSubmit(sAlgo);
                        crm.SubmitChanges();
                        formAdminAra.referance.IPport.DataToSend = "DeleteSentimentAlgo|" + idx.ToString() + (char)3;
                    }
                }
                else
                {
                    MessageBox.Show("Kayıt Bulunamadı!");
                }


                Ara();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
