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
    public partial class formMusteriFeedBackDetay : Form
    {
        public formMusteriFeedBackDetay()
        {
            InitializeComponent();
        }

        private void formMusteriFeedBackDetay_Load(object sender, EventArgs e)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                var id = (int)this.Tag;

                var item = crm.MusteriTaleplers.FirstOrDefault(x => x.id == id);

                if (item != null)
                {
                    lblAdsoyad.Text = item.adsoyad;
                    lblHesapNo.Text = item.hesapno;
                    lblMail.Text = item.email;
                    lblTarih.Text = item.talepdate.Value.ToString();
                    rboxMesaj.Text = item.text;
                    lblTelno.Text = item.telno;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
    }
}
