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
    public partial class formClientPushNotification : Form
    {
        public formClientPushNotification()
        {
            InitializeComponent();
        }

        private void rbtnTumKullanici_Click(object sender, EventArgs e)
        {
            if (rbtnTumKullanici.Checked)
            {
                txtHesapno.Visible = false;
                label1.Visible = false;
            }
            else
            {
                txtHesapno.Visible = true;
                label1.Visible = true;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            crmDFNDataContext crm = new crmDFNDataContext();

            if (rbtnTekKullanici.Checked)
            {
                if (txtHesapno.Text == "")
                {
                    MessageBox.Show("Lütfen Mesajı Göndermek İstediğiniz Kullanıcının Hesap Numarasını Giriniz"); return;
                }

                if (crm.Users.Where(x => x.UserName == txtHesapno.Text).Any() == false)
                {
                    MessageBox.Show("Hesap No Kaydı Bulunamadı"); return;
                }
                else
                {
                    var user = crm.Users.FirstOrDefault(x => x.UserName == txtHesapno.Text);

                    if (user.FireBaseToken == "")
                    {
                        MessageBox.Show("Bu hesap No için PushNotification gönderilemez"); return;
                    }
                    else
                    {

                        if (rtboxMesaj.Text.Trim() == "")
                        {
                            MessageBox.Show("Gönderilecek Mesaj Boş"); return;
                        }

                        if (MessageBox.Show("Mesajınız "+user.UserName+"'a Gönderilecektir Emin misiniz !", "Tek Kullanıcı Mesaj Gönderim !", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            // tek mesaj

                           formAdminAra.referance.IPport.DataToSend = "TekMesaj|" + user.UserName +";"+rtboxMesaj.Text.Trim() + (char)3;
                            MessageBox.Show("Mesajınız Gönderildi"); return;


                        }
                        else
                        {   return; }
                            

                    }


                }

            }
            else
            {

                if (rtboxMesaj.Text.Trim() == "")
                {
                    MessageBox.Show("Gönderilecek Mesaj Boş"); return;
                }

                if (MessageBox.Show("Mesajınız Tüm kullanıcılara Gönderilecektir Emin misiniz !", "Toplu Bilditim Gönderim !", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    formAdminAra.referance.IPport.DataToSend = "TopluMesaj|" + rtboxMesaj.Text.Trim() + (char)3;
                    MessageBox.Show("Mesajınız Gönderildi"); return;
                }
                else
                { return; }

            }
        }
    }
}
