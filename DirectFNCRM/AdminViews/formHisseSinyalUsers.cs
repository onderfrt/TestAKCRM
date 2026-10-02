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
    public partial class formHisseSinyalUsers : Form
    {
        public formHisseSinyalUsers()
        {
            InitializeComponent();

            dataGridView1._DoubleBuffer(true);
        }

        private void formHisseSinyalUsers_Load(object sender, EventArgs e)
        {
            try
            {
                Ara();
            }
            catch { }

        }
        public void Ara()
        {
            try
            {
                var hs = new HisseSinyalDataContext();
                var datasource = hs.T_Members.OrderByDescending(x=>x.ID).Select(x => new { x.ID, x.isActive,  x.Member_Fullname, x.Member_Email, x.Member_Phone ,x.Member_Licences,x.Member_LastLogin,x.kvkkAccepted}).ToList();

                if (txtFiltre1.Text.Trim() != "")
                {
                    var filtre = txtFiltre1.Text.Trim();
                    datasource = datasource.Where(x=> x.Member_Fullname.Contains(filtre) ==true 
                    || x.Member_Email.Contains(filtre)
                       || x.Member_Licences.Contains(filtre)
                    ).ToList();
                }
                if (txtFiltre2.Text.Trim() != "")
                {
                    var filtre = txtFiltre2.Text.Trim();
                    datasource = datasource.Where(x => x.Member_Fullname.Contains(filtre) == true
                    || x.Member_Email.Contains(filtre)
                       || x.Member_Licences.Contains(filtre)
                    ).ToList();


                }
                if (txtFiltre3.Text.Trim() != "")
                {
                    var filtre = txtFiltre3.Text.Trim();
                    datasource = datasource.Where(x => x.Member_Fullname.Contains(filtre) == true
                    || x.Member_Email.Contains(filtre)
                       || x.Member_Licences.Contains(filtre)
                    ).ToList();


                }

                if (rbAcik.Checked) datasource = datasource.Where(x=>x.isActive).ToList();
                else if (rbKapali.Checked) datasource = datasource.Where(x => x.isActive==false).ToList();
                lblToplamUser.Text = datasource.Count().ToString();
                lblAcikKullanici.Text = datasource.Where(x=>x.isActive).Count().ToString();
                lblKRMD1.Text = datasource.Where(x => x.isActive && x.Member_Licences=="Karma 1").Count().ToString();
                dataGridView1.DataSource = datasource;
            }
            catch { }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            Ara();
        }

        private void txtFiltre1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    var textbox = (TextBox)sender;
                    if (textbox.Text.Trim() != "") Ara();
                }
            }
            catch { }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0) return;

                var id = dataGridView1.SelectedRows[0].Cells[0].Value;

                var frm = new formHisseSinyalUserDetay();
                frm.Tag = id;
                frm.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1._CopyToExcel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rbTum_CheckedChanged(object sender, EventArgs e)
        {
            Ara();
        }
    }
}
