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
    public partial class formModelPortfoy : Form
    {
        public static formModelPortfoy reference;
        public formModelPortfoy()
        {
            InitializeComponent();
        }

        public int activeItemId = 0;

        private void formModelPortfoy_Load(object sender, EventArgs e)
        {
            try
            {

                reference = this;

                ara();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ara()
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                dataGridView1.DataSource = crm.ModelPortfoys;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void formModelPortfoy_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {

                reference = null;

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

                txtSembol.Text = txtSembol.Text._ToEngUp();

               var id = 0;

                if (crm.ModelPortfoys.Where(x => x.SembolName == txtSembol.Text.Trim()).Any())
                {
                    var mdp = crm.ModelPortfoys.FirstOrDefault(x => x.SembolName == txtSembol.Text.Trim());

                    mdp.Date = dtptarih.Value;
                    mdp.Fiyat = decimal.Parse(txtFiyat.Text.Trim());
                    mdp.HedefFiyat = decimal.Parse(txtHedefFiyat.Text.Trim());
                    crm.SubmitChanges();
                    id = mdp.id;

                }
                else
                {

                    ModelPortfoy mdp = new ModelPortfoy();
                    mdp.Date = dtptarih.Value;
                    mdp.SembolName = txtSembol.Text.Trim();
                    mdp.Fiyat = decimal.Parse(txtFiyat.Text.Trim());
                    mdp.HedefFiyat = decimal.Parse(txtHedefFiyat.Text.Trim());
                    crm.ModelPortfoys.InsertOnSubmit(mdp);
                    crm.SubmitChanges();
                    id = mdp.id;


                }

                if(id>0)
                formAdminAra.referance.IPport.DataToSend = "UpdateModelPortfoy|" + id.ToString() + (char)3;
                Temizle();

                ara();





            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (activeItemId == 0)
                {
                    MessageBox.Show("Silmek için bir sembol seçin"); return;
                }

                crmDFNDataContext crm = new crmDFNDataContext();

                if (crm.ModelPortfoys.Where(x => x.id == activeItemId).Any())
                {
                    var mdp = crm.ModelPortfoys.FirstOrDefault(x => x.id == activeItemId);

                    var sendtext = "DeleteModelPortfoy|" + mdp.id.ToString() + "|" + mdp.SembolName + "|" + mdp.Fiyat + "|" + mdp.HedefFiyat;

                    var silinen = mdp.SembolName;
                    crm.ModelPortfoys.DeleteOnSubmit(mdp);

                    crm.SubmitChanges();

                    formAdminAra.referance.IPport.DataToSend = sendtext + (char)3;

                    MessageBox.Show(silinen + " Silindi");
                }
                else
                {
                    MessageBox.Show("bulunamadı");

                }

                Temizle();
                ara();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

               var idx =  (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;


                crmDFNDataContext crm = new crmDFNDataContext();

                var mdp = crm.ModelPortfoys.FirstOrDefault(x => x.id == idx);
                dtptarih.Value = mdp.Date.Value;
                activeItemId = mdp.id;
                txtSembol.Text = mdp.SembolName;
                txtFiyat.Text = mdp.Fiyat.ToString();
                txtHedefFiyat.Text = mdp.HedefFiyat.ToString();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            try
            {
                Temizle();

                ara();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Temizle()
        {
            activeItemId = 0;
            txtSembol.Text = "";
            txtFiyat.Text = "";
            txtHedefFiyat.Text = "";
        }
    }
}
