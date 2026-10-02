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
    public partial class formHisseSinyalUserDetay : Form
    {
        public formHisseSinyalUserDetay()
        {
            InitializeComponent();
        }

        public int activeid = 0;
        private void formHisseSinyalUserDetay_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Tag != null) activeid = (int)this.Tag;

                if (activeid != 0)
                {
                    var hs = new HisseSinyalDataContext();

                    var user = hs.T_Members.FirstOrDefault(x=>x.ID==activeid);

                    this.Text = user.Member_Email + "  -  Detay";

                    txtAdSoyad.Text = user.Member_Fullname;
                    txtMail.Text = user.Member_Email;
                    txtTel.Text = user.Member_Phone;
                    txtAdres.Text = user.Member_Address;
                    txtUlke.Text = user.Member_Country;
                    txtSehir.Text = user.Member_City;
                    lblSonlogindate.Text = user.Member_LastLogin.ToString();
                    lblbirddate.Text = user.Member_Birthday.ToString();
                    txtUlke.Text = user.Member_Country;
                    txtTckn.Text = user.Member_TCKN;
                    rtbNot.Text = user.Member_Notes;
                
                    lblKVKK.Text = user.kvkkAccepted?"Evet":"Hayır";
                    if (user.Member_Licences.Contains("Karma 1"))
                    {
                        chkKarmad1.Checked = true;
                        dtpKrmd1EndDate.Value = user.Member_LicenceEndDate;
                    }
                    else
                    {
                        dtpKrmd1EndDate.Visible = false;

                    }
                    if (user.isActive)
                        comboYayinDurumu.SelectedIndex = 0;
                    else
                        comboYayinDurumu.SelectedIndex = 1;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

       
        private void comboYayinDurumu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboYayinDurumu.SelectedIndex == 0)
                comboYayinDurumu.BackColor = Color.LimeGreen;
            else if (comboYayinDurumu.SelectedIndex == 1)
            {
                comboYayinDurumu.BackColor = Color.Red;
            }

            btnKaydet.Select();
        }
    }
}
