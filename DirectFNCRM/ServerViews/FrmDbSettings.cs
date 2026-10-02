using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectFNCRM.ServerViews
{
    public partial class FrmDbSettings : Form
    {
        // Config’teki isimler
        private const string CS_MAIN = "DirectFNCRM.Properties.Settings.DBConnectionString";
        private const string CS_SIGNAL = "DirectFNCRM.Properties.Settings.HisseSinyalConnectionString";

        public FrmDbSettings()
        {
            InitializeComponent();
            rbWindowsAuth.CheckedChanged += (s, e) => SyncAuthUI();
            rbSqlAuth.CheckedChanged += (s, e) => SyncAuthUI();
            SyncAuthUI();
            ConnectionDbInfo();
        }
        void SyncAuthUI()
        {
            bool win = rbWindowsAuth.Checked;
            txtUser.Enabled = !win;
            txtPassword.Enabled = !win;

            if (win)
            {
                txtUser.Text = "";
                txtPassword.Text = "";
                lblStatus.Text = "Windows Authentication seçildi. Kullanıcı adı/şifre gerekmez.";
            }
            else
            {
                lblStatus.Text = "SQL Authentication seçildi. Kullanıcı adı ve şifre girin.";
            }
        }
        private string BuildConnectionString()
        {
            var b = new SqlConnectionStringBuilder
            {
                DataSource = txtServer.Text.Trim(),
                InitialCatalog = txtDatabase.Text.Trim(),
                IntegratedSecurity = rbWindowsAuth.Checked,
                PersistSecurityInfo = false,
                // opsiyonel ama kurumsalda çok lazım olabilir:
                 TrustServerCertificate = true,
                 Encrypt = true,
                ConnectTimeout = 5
            };

            if (!b.IntegratedSecurity)
            {
                b.UserID = txtUser.Text.Trim();
                b.Password = txtPassword.Text; 
            }

            return b.ConnectionString;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "Bağlantı test ediliyor...";
                lblStatus.Refresh();

                string cs = BuildConnectionString();

                using (var conn = new SqlConnection(cs))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT 1", conn))
                    {
                        cmd.ExecuteScalar();
                    }
                }

                lblStatus.Text = " Bağlantı başarılı.";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                
                lblStatus.Text = " Bağlantı başarısız: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
                string cs = BuildConnectionString();
                TestConnectionOrThrow(cs);

                
                ConnectionStringManager.UpdateAndProtect(CS_MAIN, cs);
              //  ConnectionStringManager.UpdateAndProtect(CS_SIGNAL, cs);

                MessageBox.Show(
                    "Veritabanı ayarları kaydedildi.\nUygulama yeniden başlatılacak.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                RestartApplication();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Kaydetme başarısız: " + ex.Message,
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void TestConnectionOrThrow(string cs)
        {
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT 1", conn))
                    cmd.ExecuteScalar();
            }
        }

        private void RestartApplication()
        {
            
            var exe = Application.ExecutablePath;
            System.Diagnostics.Process.Start(exe);
            Application.Exit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void ConnectionDbInfo()
        {
            try
            {
                var cs = ConfigurationManager.ConnectionStrings[CS_MAIN]?.ConnectionString;
                if (string.IsNullOrWhiteSpace(cs))
                {
                    lblCurrentDbValue.Text = "(bulunamadı)";
                    return;
                }

                var b = new SqlConnectionStringBuilder(cs);

                lblCurrentDbValue.Text = $"{b.DataSource} / {b.InitialCatalog}";

                
                txtServer.Text = b.DataSource;
                txtDatabase.Text = b.InitialCatalog;

                if (b.IntegratedSecurity)
                {
                    rbWindowsAuth.Checked = true;
                }
                else
                {
                    rbWindowsAuth.Checked = false;
                    txtUser.Text = b.UserID;
                    
                    txtPassword.Text = "";
                }
            }
            catch
            {
                lblCurrentDbValue.Text = "(okunamadı)";
            }
        }
    }
}
