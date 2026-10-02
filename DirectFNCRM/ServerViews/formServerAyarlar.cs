using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectFNCRM.ServerViews
{
    public partial class formServerAyarlar : Form
    {
        public formServerAyarlar()
        {
            InitializeComponent();
        }

        public string ssoip = "";
        public string ssoport = "";
        public string servisport = "";
        public string clientport = "";

        private void formServerAyarlar_Load(object sender, EventArgs e)
        {
          
            ssoip = Server.referance.ssoip;
            ssoport = Server.referance.ssoport;
            servisport = Server.referance.IpDeamon.LocalPort.ToString();
            clientport = Server.referance.IpDeamonForClilents.LocalPort.ToString();


        }

        private void formServerAyarlar_FormClosed(object sender, FormClosedEventArgs e)
        {
            MyTools.WritePrivateProfileString("Connectiion", "SSO_IP", txtSSOIP.Text, Application.StartupPath + "\\ServerAyarlar.ini");
            MyTools.WritePrivateProfileString("Connectiion", "SSO_PORT", txtSSOPort.Text, Application.StartupPath + "\\ServerAyarlar.ini");
            MyTools.WritePrivateProfileString("Connectiion", "Servis_PORT", txtServisPort.Text, Application.StartupPath + "\\ServerAyarlar.ini");
            MyTools.WritePrivateProfileString("Connectiion", "Client_PORT", txtClientPort.Text, Application.StartupPath + "\\ServerAyarlar.ini");
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtSSOIP.Text != ssoip || txtSSOPort.Text != ssoport)
                {
                

                    Server.referance.ssoip = txtSSOIP.Text;
                    Server.referance.ssoport = txtSSOPort.Text;
                }


                if (txtServisPort.Text != servisport)
                {
                    Server.referance.IpDeamon.Listening = false;
                    Server.referance.IpDeamon.LocalPort = Int32.Parse(txtServisPort.Text);
                    Server.referance.IpDeamon.Listening = true;
                }

                if (txtClientPort.Text != clientport)
                {
                    Server.referance.IpDeamonForClilents.Listening = false;
                    Server.referance.IpDeamonForClilents.LocalPort = Int32.Parse(txtClientPort.Text);
                    Server.referance.IpDeamonForClilents.Listening = true;
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

       
                      

        }
    }
}
