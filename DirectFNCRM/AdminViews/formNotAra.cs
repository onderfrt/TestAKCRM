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
    public partial class formNotAra : Form
    {
        public formNotAra()
        {
            InitializeComponent();
        }

        public static IQueryable<User> ActiveSorgu;
        private void formNotAra_Load(object sender, EventArgs e)
        {
            crmDFNDataContext crm = new crmDFNDataContext();

            var sorgu = crm.Calisans.Where(x=>x.CalisanTipId==1).OrderBy(x => x.Ad).Select(x => new { x.calisanID, adsoyad = x.Ad + " " + x.Soyad });
            comboCalisan.DataSource = sorgu;
            comboCalisan.DisplayMember = "adsoyad";
            comboCalisan.ValueMember = "calisanID";


            comboDepartman.DataSource = crm.Departmans;
            comboDepartman.DisplayMember = "DepartmanAdi";
            comboDepartman.ValueMember = "DepartmanId";

            comboCalisan.SelectedIndex = -1;
            comboDepartman.SelectedIndex = -1;

            ara();
        }

        public void ara()
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                var sonuc = crm.UserNots.Where(x=>x.id>0);


                if (txtkriter1.Text != "")
                {
                    sonuc = sonuc.Where(x => x.User.UserName.Contains(txtkriter1.Text)
                    || x.Text.Contains(txtkriter1.Text)
                    || x.User.Name.Contains(txtkriter1.Text)
                    || x.User.Surname.Contains(txtkriter1.Text)
                    || x.User.Aciklama.Contains(txtkriter1.Text));
                }
                if (txtkriter2.Text != "")
                {
                    sonuc = sonuc.Where(x => x.User.UserName.Contains(txtkriter2.Text)
                    || x.Text.Contains(txtkriter2.Text)
                    || x.User.Name.Contains(txtkriter2.Text)
                    || x.User.Surname.Contains(txtkriter2.Text)
                    || x.User.Aciklama.Contains(txtkriter2.Text));
                }
                if (txtkriter3.Text != "")
                {
                    sonuc = sonuc.Where(x => x.User.UserName.Contains(txtkriter3.Text)
                    || x.Text.Contains(txtkriter3.Text)
                    || x.User.Name.Contains(txtkriter3.Text)
                    || x.User.Surname.Contains(txtkriter3.Text)
                    || x.User.Aciklama.Contains(txtkriter3.Text));
                }

                if (comboCalisan.SelectedIndex != -1)
                {
                    if (comboCalisan.SelectedValue is Calisan)
                    { }
                    else if ((comboCalisan.SelectedValue is Int32))
                       sonuc = sonuc.Where(x => x.CalisanId == (int)comboCalisan.SelectedValue);
                }



                if (comboDepartman.SelectedIndex != -1)
                {
                    if (comboDepartman.SelectedValue is Departman)
                        sonuc = sonuc.Where(x => x.Calisan.Departman == comboDepartman.SelectedValue);
                    else
                        sonuc = sonuc.Where(x => x.Calisan.DepartmanId == (int)comboDepartman.SelectedValue);
                }

                if (dateTimeStartDate.Value.Date != dateTimeEndDate.Value.Date)
                {

                    sonuc = sonuc.Where(x => x.Tarih.Value.Date >= dateTimeStartDate.Value.Date && x.Tarih.Value.Date <= dateTimeEndDate.Value.Date);
                }
                else
                {
                    sonuc = sonuc.Where(x => x.Tarih.Value.Date == dateTimeStartDate.Value.Date);
                }



                dataGridView1.DataSource = sonuc.OrderByDescending(x=>x.Tarih).Select(x=> new {x.Tarih,x.User.PmtsNo,x.User.UserName, Aid_Soyad =x.User.Name + " " + x.User.Surname , x.Text ,x.Userid});

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }

        private void txtkriter1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ara();
            }
           
        }

        private void txtkriter2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ara();
            }
        }

        private void txtkriter3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ara();
            }
        }

        private void comboCalisan_SelectedIndexChanged(object sender, EventArgs e)
        {
            ara();
        }

        private void comboDepartman_SelectedIndexChanged(object sender, EventArgs e)
        {
            ara();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtkriter1.Text = "";
            txtkriter2.Text = "";
            txtkriter3.Text = "";
            comboDepartman.SelectedIndex = -1;
            comboCalisan.SelectedIndex = -1;
            dateTimeStartDate.Value = DateTime.Now;
            dateTimeEndDate.Value = DateTime.Now;

            ara();
        }

        private void btn2TarihAra_Click(object sender, EventArgs e)
        {
            ara();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count < 1)
                return;

            var id = dataGridView1.CurrentRow.Cells[5].Value;


            formUserNotscs frm = new formUserNotscs();
            frm.Tag = id;
            frm.ShowDialog();

        }
    }
}
