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
    public partial class formFaturaGiris : Form
    {
        public formFaturaGiris()
        {
            InitializeComponent();
        }
        public bool comboildoldurma = false;
        public User activeuser;
        private void formFaturaGiris_Load(object sender, EventArgs e)
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            comboDonem.DataSource = crm.faturaDonems;
            comboDonem.DisplayMember = "DonemName";
            comboDonem.ValueMember = "DonemID";


            comboUlke.DataSource = crm.Ulkes;
            comboUlke.DisplayMember = "UlkeAdi";
            comboUlke.ValueMember = "Id";

            comboUlke.SelectedItem = comboUlke.Items[212];
            var t = DateTime.Now;
            dateTimeStart.Value = new DateTime(t.Year, t.Month, 1);

            activeuser = (User)this.Tag;


            lblUserName.Text = activeuser.UserName;
           
         
      

            comboildoldurma = true;




        }

        private void chkAdresOnay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAdresOnay.Checked == true)
            {
                txtAdres.Text = activeuser.Iletisim.acikadres;
                txtMail.Text = activeuser.Iletisim.email;
                txtCeptel.Text = activeuser.Iletisim.Ceptel;
                txtTel1.Text = activeuser.Iletisim.Tel1;
                txtTel2.Text = activeuser.Iletisim.Tel2;
                comboUlke.SelectedValue = activeuser.Iletisim.UlkeId;
                if(activeuser.Iletisim.IlId!=null)
                    comboIL.SelectedValue = activeuser.Iletisim.IlId;
                if (activeuser.Iletisim.IlceId != null)
                    comboILCE.SelectedValue = activeuser.Iletisim.IlceId;


            }
        }

        private void comboIL_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboIL.SelectedIndex == -1)
                    return;
                var ilid = 0;
                var ulkeid = 0;
                crmDFNDataContext crm = new crmDFNDataContext();
                if (comboIL.SelectedValue is Il)
                {
                    Il deger = (Il)comboIL.SelectedValue;
                    ilid = deger.Id;
                    ulkeid = (int)deger.UlkeId;
                }
                else
                {
                    ilid = (int)comboIL.SelectedValue;
                    var il = crm.Ils.Where(x => x.Id == ilid).FirstOrDefault();
                    ulkeid = il.UlkeId.Value;

                }

                if (ulkeid == 213)
                {

                    if (crm.Ilces.Where(x => x.IlId == ilid).Any())
                    {
                        comboILCE.DataSource = crm.Ilces.Where(x => x.IlId == ilid);
                        comboILCE.DisplayMember = "IlceAdi";
                        comboILCE.ValueMember = "Id";
                    }

                }

                if (!comboildoldurma)
                {
                    comboILCE.SelectedIndex = -1;
                }

                if (comboUlke.SelectedIndex != 212)
                {
                    comboILCE.SelectedIndex = -1;
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void comboUlke_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboUlke.SelectedIndex == -1)
                    return;

                var ulkeid = 0;
                crmDFNDataContext crm = new crmDFNDataContext();
                if (comboUlke.SelectedValue is Ulke)
                {
                    Ulke deger = (Ulke)comboUlke.SelectedValue;
                    ulkeid = deger.Id;
                }
                else
                {
                    ulkeid = (int)comboUlke.SelectedValue;
                }

                if (crm.Ils.Where(x => x.UlkeId == ulkeid).Any())
                {
                    comboIL.DataSource = crm.Ils.Where(x => x.UlkeId == ulkeid);
                    comboIL.DisplayMember = "IlAdi";
                    comboIL.ValueMember = "Id";
                }


                if (!comboildoldurma)
                {
                    comboIL.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void comboDonem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dateTimeStart.Value.Day > 1)
            {
                var t = DateTime.Now;
                dateTimeStart.Value = new DateTime(t.Year, t.Month, 1);
            }

            dateTimeEnd.Value = DateTime.Now;
            switch (comboDonem.SelectedIndex)
            {
                case 0:
                    dateTimeEnd.Value = dateTimeStart.Value.AddMonths(1).AddDays(-1);

                    break;

                case 1:
                    dateTimeEnd.Value = dateTimeStart.Value.AddMonths(3).AddDays(-1);

                    break;
                case 2:
                    dateTimeEnd.Value = dateTimeStart.Value.AddMonths(6).AddDays(-1);

                    break;
                case 3:
                    dateTimeEnd.Value = dateTimeStart.Value.AddMonths(12).AddDays(-1);

                    break;

                case 4:
                    dateTimeEnd.Value = dateTimeStart.Value.AddMonths(24).AddDays(-1);

                    break;
                default:
                    break;
            }
        }
    }
}
