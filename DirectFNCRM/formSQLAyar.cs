using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectFNCRM
{
    public partial class formSQLAyar : Form
    {
        public formSQLAyar()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MyTools.DBName = txtDBName.Text.Trim();
            MyTools.DataSource = txtDataSource.Text.Trim();
            MyTools.UserName = txtUserName.Text.Trim();
            MyTools.Password = txtPasword.Text.Trim();
            MyTools.ConnectionStingAyarlariYaz();

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["DirectFNCRM.Properties.Settings.DfnCrmConnectionString"].ConnectionString = MyTools.GetConnectionString();
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");

            MessageBox.Show("Ayarlar Kaydedildi");
            this.Close();

        }

        private void formSQLAyar_Load(object sender, EventArgs e)
        {
            txtDataSource.Text = MyTools.DataSource;
            txtDBName.Text = MyTools.DBName;
          

        }
    }
}
