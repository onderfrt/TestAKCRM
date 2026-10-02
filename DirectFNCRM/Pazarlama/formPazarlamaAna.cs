using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectFNCRM.Pazarlama
{
    public partial class formPazarlamaAna : Form
    {
        public formPazarlamaAna()
        {
            InitializeComponent();
            refence = this;
        }


        private formPazarlamaAna refence;

        public Calisan activeCalisan;

        private void formPazarlamaAna_Load(object sender, EventArgs e)
        {
            crmDFNDataContext crm = new crmDFNDataContext();

            activeCalisan = (Calisan)this.Tag;
            this.Text += " - " + activeCalisan.Ad + " " + activeCalisan.Soyad;


            gridGuncelle();

        }


        public void gridGuncelle()
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                dataGridView1.DataSource = crm.Musterilers;

            }
            catch (Exception ex)
            {
                

                MessageBox.Show(ex.Message);
            }


        }


    }
}
