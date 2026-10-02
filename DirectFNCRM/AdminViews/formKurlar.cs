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
    public partial class formKurlar : Form
    {
        public formKurlar()
        {
            InitializeComponent();
        }

       

        private void formKurlar_Load(object sender, EventArgs e)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                var usd = crm.ParaBirimis.FirstOrDefault(x=>x.id==2);
                var euro = crm.ParaBirimis.FirstOrDefault(x => x.id == 3);

                txtUSD.Text = ((float)usd.Kur).ToString("0.0000");
                txtEuro.Text = ((float)euro.Kur).ToString("0.0000");

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


                var usd = crm.ParaBirimis.FirstOrDefault(x => x.id == 2);
                var euro = crm.ParaBirimis.FirstOrDefault(x => x.id == 3);

                usd.Kur = decimal.Parse(txtUSD.Text);
                euro.Kur = decimal.Parse(txtEuro.Text);

                crm.SubmitChanges();


                MessageBox.Show("Döviz Kurları Kaydedildi");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
