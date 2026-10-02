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
    public partial class formMusteriFeedBack : Form
    {
        public formMusteriFeedBack()
        {
            InitializeComponent();
        }

        private void formMusteriFeedBack_Load(object sender, EventArgs e)
        {

            Ara();
        }

        public void Ara()
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            var sonuc = crm.MusteriTaleplers.OrderByDescending(x => x.talepdate).Where(x => x.id > 0);


            if (txtkriter1.Text != "")
            {
                sonuc = sonuc.Where(x => x.telno.Contains(txtkriter1.Text)
                || x.email.Contains(txtkriter1.Text)
                || x.text.Contains(txtkriter1.Text)
                || x.username.Contains(txtkriter1.Text)
                || x.hesapno.Contains(txtkriter1.Text));
            }

            if (txtkriter2.Text != "")
            {
                sonuc = sonuc.Where(x => x.telno.Contains(txtkriter2.Text)
                 || x.email.Contains(txtkriter2.Text)
                 || x.text.Contains(txtkriter2.Text)
                 || x.username.Contains(txtkriter2.Text)
                 || x.hesapno.Contains(txtkriter2.Text));
            }


            if (txtkriter3.Text != "")
            {
                sonuc = sonuc.Where(x => x.telno.Contains(txtkriter3.Text)
                || x.email.Contains(txtkriter3.Text)
                || x.text.Contains(txtkriter3.Text)
                || x.username.Contains(txtkriter3.Text)
                || x.hesapno.Contains(txtkriter3.Text));
            }


            dataGridView1.DataSource= sonuc;

        }

        private void txtkriter1_KeyPress(object sender, KeyPressEventArgs e)
        {
         
            if(e.KeyChar ==(char) Keys.Enter)
                Ara();

        }

        private void txtkriter2_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
                Ara();

        }

        private void txtkriter3_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
                Ara();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count < 1)
                return;
            var id = dataGridView1.CurrentRow.Cells[0].Value;
            var frm = new formMusteriFeedBackDetay();
            frm.Tag = id;
            frm.Show();
        }
    }
}
