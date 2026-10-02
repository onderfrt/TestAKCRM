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
    public partial class formAdminAyarlar : Form
    {
        public formAdminAyarlar()
        {
            InitializeComponent();
        }

       
        private void formAdminAyarlar_Load(object sender, EventArgs e)
        {

            btnSave.Select();

            var ip = Properties.Settings.Default.DBConnectionString;
            lblDBip.Text = ip.Split(';')[0];

        }

        private void formAdminAyarlar_FormClosed(object sender, FormClosedEventArgs e)
        {
            MyTools.WritePrivateProfileString("Connectiion", "ServerIP", txtServerIP.Text, Application.StartupPath + "\\ClientAyarlar.ini");
            MyTools.WritePrivateProfileString("Connectiion", "ServerPort", txtServerPort.Text, Application.StartupPath + "\\ClientAyarlar.ini");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
