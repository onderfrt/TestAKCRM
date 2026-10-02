using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.IO;
using nsoftware.IPWorks;

namespace DirectFNCRM.ServerViews
{
    public partial class formSslAyarlar : Form
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        // Log fonksiyonu
        private static void WriteLog(string message)
        {
            try
            {
                string logPath = Path.Combine(Application.StartupPath, "ssl_debug.log");
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\r\n";
                File.AppendAllText(logPath, logEntry);
            }
            catch { }
        }

        // CN (Common Name) extraction fonksiyonu
        private static string ExtractCN(string certSubject)
        {
            try
            {
                // CN= ile başlayan kısmı bul
                int cnIndex = certSubject.IndexOf("CN=", StringComparison.OrdinalIgnoreCase);
                if (cnIndex >= 0)
                {
                    string cnPart = certSubject.Substring(cnIndex + 3); // "CN=" kısmını atla
                    
                    // Virgül veya boşluk ile biten kısmı al
                    int endIndex = cnPart.IndexOf(',');
                    if (endIndex > 0)
                    {
                        cnPart = cnPart.Substring(0, endIndex);
                    }
                    
                    return cnPart.Trim();
                }
                return certSubject; // CN bulunamazsa tam subject'i döndür
            }
            catch
            {
                return certSubject; // Hata durumunda tam subject'i döndür
            }
        }
        public formSslAyarlar()
        {
            InitializeComponent();
        }
        public string secureServicePort = "";
        private void formSslAyarlar_Load(object sender, EventArgs e)
        {
            secureServicePort = Server.referance.HttpsService.LocalPort.ToString();
            txtSecureServicePort.Text = secureServicePort;
            setCheckBox(Server.referance.HttpsService.SSLEnabled);


        }

        private void chkSSL_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLog($"=== SSL Sertifika Seçimi Başladı ===");
                WriteLog($"SSL Durumu: {chkSSL.Checked}");

                if (chkSSL.Checked)
                {
                    WriteLog($"Sertifika seçim penceresi açılıyor...");
                    var certForm = new cert();
                    certForm.StartPosition = FormStartPosition.CenterScreen;
                    certForm.ShowDialog();

                    if (certForm.lcerts.SelectedItem != null)
                    {
                        string selectedCert = certForm.lcerts.SelectedItem.ToString();
                        WriteLog($"Seçilen sertifika: {selectedCert}");
                        
                        // Store tipini ve sertifika adını belirle
                        string certName = selectedCert;
                        CertStoreTypes storeType = CertStoreTypes.cstUser;
                        
                        if (selectedCert.StartsWith("[USER]"))
                        {
                            certName = selectedCert.Substring(7);
                            storeType = CertStoreTypes.cstUser;
                            WriteLog($"User Store sertifikası seçildi");
                        }
                        else if (selectedCert.StartsWith("[MACHINE]"))
                        {
                            certName = selectedCert.Substring(10);
                            storeType = (CertStoreTypes)1;
                            WriteLog($"LocalMachine Store sertifikası seçildi");
                        }
                        
                        Server.referance.HttpsService.SSLCert = certForm.certmgr1.Cert;
                        Server.referance.HttpsService.SSLEnabled = true;
                        
                        // CN kısmını extract et
                        string cnOnly = ExtractCN(certName);
                        WriteLog($"Tam sertifika adı: {certName}");
                        WriteLog($"CN kısmı: {cnOnly}");
                        
                        // Base64 encoding
                        string base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes("CN=" + cnOnly));
                        string storeTypeStr = (storeType == CertStoreTypes.cstUser ? "USER" : "MACHINE");
                        string fullCertInfo = $"{storeTypeStr}|{base64Encoded}";
                        
                        Settings.MySetting.Sertifika = fullCertInfo;
                        Settings.MySetting.SertifikaDisplay = certName;
                        
                        WriteLog($"SSL sertifikası başarıyla seçildi: {cnOnly} ({storeTypeStr} store)");
                    }
                    else
                    {
                        WriteLog($"Hiçbir sertifika seçilmedi");
                        chkSSL.Checked = false;
                        MessageBox.Show("Lütfen bir sertifika seçin.", "Uyarı");
                        return;
                    }
                }
                else
                {
                    WriteLog($"SSL devre dışı bırakılıyor");
                    Server.referance.HttpsService.SSLCert = null;
                    Server.referance.HttpsService.SSLEnabled = false;
                    Settings.MySetting.Sertifika = "";
                    Settings.MySetting.SertifikaDisplay = "";
                }

                WriteLog($"=== SSL Sertifika Seçimi Tamamlandı ===");
            }
            catch (Exception ex)
            {
                WriteLog($"SSL sertifika seçiminde hata: {ex.Message}");
                MessageBox.Show(ex.Message.ToString(), "Hata");
                chkSSL.Checked = false;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLog($"=== SSL Ayarları Kaydetme Başladı ===");
                WriteLog($"Port: {txtSecureServicePort.Text}, SSL: {chkSSL.Checked}");

                if (txtSecureServicePort.Text != secureServicePort)
                {
                    WriteLog($"Port değişikliği tespit edildi, HTTPS servisi yeniden başlatılıyor...");
                    Server.referance.HttpsService.Listening = false;
                    Server.referance.HttpsService.LocalPort = Int32.Parse(txtSecureServicePort.Text);
                    Server.referance.HttpsService.Listening = true;
                    
                    WriteLog($"Ayarlar INI dosyasına kaydediliyor...");
                    WritePrivateProfileString("Connection", "SecureService_PORT", txtSecureServicePort.Text, Application.StartupPath + @"\ServerAyarlar.ini");
                    WritePrivateProfileString("CertificateInfo", "Certificate", Settings.MySetting.Sertifika, Application.StartupPath + @"\ServerAyarlar.ini");
                    WritePrivateProfileString("CertificateInfo", "CertificateDisplay", Settings.MySetting.SertifikaDisplay, Application.StartupPath + @"\ServerAyarlar.ini");
                    Settings.MySetting.SecureServicePort = txtSecureServicePort.Text._ToInt();
                    
                    WriteLog($"Ayarlar başarıyla kaydedildi");
                    MessageBox.Show("SecureService_PORT bilgisi ServerAyarlar.ini dosyasına kayıt edildi.");
                    this.Close();
                }
                else
                {
                    WriteLog($"Port değişikliği yok, sertifika bilgisi kontrol ediliyor...");
                    
                    StringBuilder currentCert = new StringBuilder(1000);
                    GetPrivateProfileString("CertificateInfo", "Certificate", "", currentCert, 1000, Application.StartupPath + @"\ServerAyarlar.ini");
                    
                    if (currentCert.ToString() != Settings.MySetting.Sertifika)
                    {
                        WriteLog($"Sertifika bilgisi değişti, INI dosyasına kaydediliyor...");
                        WritePrivateProfileString("CertificateInfo", "Certificate", Settings.MySetting.Sertifika, Application.StartupPath + @"\ServerAyarlar.ini");
                        WritePrivateProfileString("CertificateInfo", "CertificateDisplay", Settings.MySetting.SertifikaDisplay, Application.StartupPath + @"\ServerAyarlar.ini");
                        WriteLog($"Sertifika bilgisi kaydedildi");
                    }
                    else
                    {
                        WriteLog($"Sertifika bilgisi değişmedi, INI güncellemesi gerekmiyor");
                    }
                    
                    MessageBox.Show("SecureService_PORT bilgisi ServerAyarlar.ini dosyasına kayıt edildi.");
                    this.Close();
                }
                
                WriteLog($"=== SSL Ayarları Kaydetme Tamamlandı ===");
            }
            catch (Exception ex)
            {
                WriteLog($"SSL ayarları kaydetme hatası: {ex.Message}");
                MessageBox.Show(ex.Message);
            }
        }
        void setCheckBox(bool checkBool)
        {
            if (chkSSL.InvokeRequired)
            {
                chkSSL.Invoke((MethodInvoker)delegate { setCheckBox(checkBool); });
            }
            else
            {
                chkSSL.Checked = checkBool;
            }

        }
    }
}
