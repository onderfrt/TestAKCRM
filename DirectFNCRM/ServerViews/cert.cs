using nsoftware.IPWorks;
using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace DirectFNCRM.ServerViews
{
    public class cert : Form
    {
        internal System.Windows.Forms.ListBox lcerts;
        internal System.Windows.Forms.TextBox tInfo;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Button bOK;
        public Certmgr certmgr1;
        private System.ComponentModel.IContainer components;
        public cert()
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lcerts = new System.Windows.Forms.ListBox();
            this.tInfo = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.bOK = new System.Windows.Forms.Button();
            this.certmgr1 = new nsoftware.IPWorks.Certmgr(this.components);
            this.SuspendLayout();
            // 
            // lcerts
            // 
            this.lcerts.Location = new System.Drawing.Point(5, 68);
            this.lcerts.Name = "lcerts";
            this.lcerts.Size = new System.Drawing.Size(513, 95);
            this.lcerts.TabIndex = 11;
            this.lcerts.SelectedIndexChanged += new System.EventHandler(this.lcerts_SelectedIndexChanged);
            // 
            // tInfo
            // 
            this.tInfo.Location = new System.Drawing.Point(6, 199);
            this.tInfo.Multiline = true;
            this.tInfo.Name = "tInfo";
            this.tInfo.Size = new System.Drawing.Size(512, 154);
            this.tInfo.TabIndex = 9;
            // 
            // Label3
            // 
            this.Label3.Location = new System.Drawing.Point(9, 173);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(100, 23);
            this.Label3.TabIndex = 8;
            this.Label3.Text = "Certificate Info";
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point(7, 7);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(116, 23);
            this.Label2.TabIndex = 7;
            this.Label2.Text = "Certificate Store:  MY";
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(7, 44);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(112, 23);
            this.Label1.TabIndex = 6;
            this.Label1.Text = "Available Certificates";
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(452, 45);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(64, 19);
            this.bOK.TabIndex = 12;
            this.bOK.Text = "OK";
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // certmgr1
            // 
            this.certmgr1.About = "IP*Works! 2016 [Build 7126]";
            this.certmgr1.OnCertList += new nsoftware.IPWorks.Certmgr.OnCertListHandler(this.certmgr1_OnCertList);
            // 
            // cert
            // 
            this.ClientSize = new System.Drawing.Size(522, 358);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.lcerts);
            this.Controls.Add(this.tInfo);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Name = "cert";
            this.Text = "Certificate Manager";
            this.Load += new System.EventHandler(this.cert_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void cert_Load(object sender, EventArgs e)
        {
            FillStores();
        }
        private string currentStorePrefix = "";
        
        private void FillStores()
        {
            lcerts.Items.Clear();
            tInfo.Text = "";
            
            // Önce User store'dan sertifikaları listele
            WriteLog("User Store'dan sertifikalar listeleniyor...");
            currentStorePrefix = "[USER] ";
            certmgr1.CertStore = "MY";
            certmgr1.ListStoreCertificates();
            
            // Sonra LocalMachine store'dan sertifikaları listele
            WriteLog("LocalMachine Store'dan sertifikalar listeleniyor...");
            try
            {
                currentStorePrefix = "[MACHINE] ";
                // Farklı store adlarını dene
                string[] storeNames = { "LocalMachine\\MY", "MY", "Machine\\MY", "LocalMachine" };
                
                foreach (string storeName in storeNames)
                {
                    try
                    {
                        WriteLog($"Store adı deneniyor: {storeName}");
                        certmgr1.CertStore = storeName;
                        certmgr1.ListStoreCertificates();
                        WriteLog($"✅ Store '{storeName}' başarılı, {lcerts.Items.Count} sertifika bulundu");
                        break; // Başarılı olursa döngüden çık
                    }
                    catch (Exception storeEx)
                    {
                        WriteLog($"❌ Store '{storeName}' başarısız: {storeEx.Message}");
                    }
                }
                
                // Eğer hiçbir store adı çalışmazsa PowerShell ile dene
                if (lcerts.Items.Count == 0 || !lcerts.Items.Cast<string>().Any(x => x.StartsWith("[MACHINE]")))
                {
                    WriteLog("⚠️ IPWorks ile LocalMachine store'a erişilemedi, PowerShell fallback devreye giriyor...");
                    LoadLocalMachineCertsWithPowerShell();
                }
                else
                {
                    WriteLog($"✅ IPWorks ile LocalMachine store'a erişim başarılı");
                }
            }
            catch (Exception ex)
            {
                WriteLog($"LocalMachine store erişim hatası: {ex.Message}");
            }

            // ListBox'ta öğe varsa ilk öğeyi seç
            if (lcerts.Items.Count > 0)
            {
                lcerts.SelectedIndex = 0;
            }
            
            WriteLog($"Toplam {lcerts.Items.Count} sertifika listelendi");
        }
        
        private void LoadLocalMachineCertsWithPowerShell()
        {
            try
            {
                WriteLog("PowerShell ile LocalMachine sertifikaları yükleniyor...");
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName = "powershell";
                psi.Arguments = "-Command \"Get-ChildItem -Path Cert:\\LocalMachine\\My | Select-Object -ExpandProperty Subject\"";
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                
                using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    
                    if (!string.IsNullOrEmpty(output))
                    {
                        string[] certs = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string cert in certs)
                        {
                            if (!string.IsNullOrWhiteSpace(cert))
                            {
                                string certSubject = cert.Trim();
                                
                                // PowerShell çıktısında da encoding sorunları olabilir
                                if (certSubject.Contains("Ä°") || certSubject.Contains("ÅŸ") || certSubject.Contains("Åž"))
                                {
                                    WriteLog($"PowerShell çıktısında bozuk karakterler tespit edildi: {certSubject}");
                                    
                                    // Bozuk karakterleri düzelt
                                    certSubject = certSubject.Replace("Ä°", "İ")
                                                           .Replace("ÅŸ", "ş")
                                                           .Replace("Åž", "Ş")
                                                           .Replace("Ã¼", "ü")
                                                           .Replace("Ã‡", "Ç")
                                                           .Replace("Ã¶", "ö")
                                                           .Replace("Ã§", "ç")
                                                           .Replace("Ä±", "ı")
                                                           .Replace("ÄŸ", "ğ");
                                    
                                    WriteLog($"PowerShell çıktısı düzeltildi: {certSubject}");
                                }
                                
                                lcerts.Items.Add("[MACHINE] " + certSubject);
                                WriteLog($"PowerShell ile sertifika eklendi: {certSubject}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog($"PowerShell ile sertifika yükleme hatası: {ex.Message}");
            }
        }
        
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
        
        // Türkçe karakterleri düzeltme fonksiyonu
        private string FixTurkishCharacters(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
                
            // Bozuk karakterleri düzelt
            return text.Replace("Ä°", "İ")
                      .Replace("ÅŸ", "ş")
                      .Replace("Åž", "Ş")
                      .Replace("Ã¼", "ü")
                      .Replace("Ã‡", "Ç")
                      .Replace("Ã¶", "ö")
                      .Replace("Ã§", "ç")
                      .Replace("Ä±", "ı")
                      .Replace("ÄŸ", "ğ");
        }

        private void lcerts_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Seçili öğe kontrolü
            if (lcerts.SelectedIndex >= 0 && lcerts.SelectedIndex < lcerts.Items.Count)
            {
                try
                {
                    string selectedItem = lcerts.Text;
                    string fullCertName = selectedItem;
                    CertStoreTypes storeType = CertStoreTypes.cstUser;
                    
                    // Store tipini belirle
                    if (selectedItem.StartsWith("[USER]"))
                    {
                        fullCertName = selectedItem.Substring(7); // "[USER] " kısmını çıkar
                        storeType = CertStoreTypes.cstUser;
                        WriteLog($"User Store sertifikası seçildi: {fullCertName}");
                    }
                    else if (selectedItem.StartsWith("[MACHINE]"))
                    {
                        fullCertName = selectedItem.Substring(10); // "[MACHINE] " kısmını çıkar
                        storeType = (CertStoreTypes)1; // LocalMachine
                        WriteLog($"LocalMachine Store sertifikası seçildi: {fullCertName}");
                    }
                    
                    // CN kısmını extract et
                    string certName = ExtractCN(fullCertName);
                    WriteLog($"Tam sertifika adı: {fullCertName}");
                    WriteLog($"CN kısmı: {certName}");
                    
                    // Store tipine göre farklı store adları dene
                    string storeName = "MY";
                    bool certFound = false;
                    
                    if (storeType == (CertStoreTypes)1) // LocalMachine
                    {
                        string[] machineStoreNames = { "LocalMachine\\MY", "MY", "Machine\\MY" };
                        foreach (string store in machineStoreNames)
                        {
                            try
                            {
                                WriteLog($"LocalMachine sertifika için store deneniyor: {store}");
                                certmgr1.Cert = new Certificate(storeType, store, "", certName);
                                storeName = store;
                                certFound = true;
                                WriteLog($"✅ LocalMachine sertifika bulundu: {store}");
                                break;
                            }
                            catch (Exception storeEx)
                            {
                                WriteLog($"❌ Store '{store}' başarısız: {storeEx.Message}");
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            certmgr1.Cert = new Certificate(storeType, storeName, "", certName);
                            certFound = true;
                            WriteLog($"✅ User sertifika bulundu");
                        }
                        catch (Exception userEx)
                        {
                            WriteLog($"❌ User sertifika bulunamadı: {userEx.Message}");
                        }
                    }
                    
                    if (certFound)
                    {
                        // Sertifika bilgilerini encoding düzeltmesi ile göster
                        string issuer = FixTurkishCharacters(certmgr1.Cert.Issuer);
                        string subject = FixTurkishCharacters(certmgr1.Cert.Subject);
                        
                        tInfo.Text = "Store: " + (storeType == CertStoreTypes.cstUser ? "User" : "LocalMachine") + "\r\n";
                        tInfo.Text = tInfo.Text + "Store Name: " + storeName + "\r\n";
                        tInfo.Text = tInfo.Text + "Issuer: " + issuer + "\r\n";
                        tInfo.Text = tInfo.Text + "Subject: " + subject + "\r\n";
                        tInfo.Text = tInfo.Text + "Version: " + certmgr1.Cert.Version + "\r\n";
                        tInfo.Text = tInfo.Text + "Serial Number: " + certmgr1.Cert.SerialNumber + "\r\n";
                        tInfo.Text = tInfo.Text + "Signature Algorithm: " + certmgr1.Cert.SignatureAlgorithm + "\r\n";
                        tInfo.Text = tInfo.Text + "Effective Date: " + certmgr1.Cert.EffectiveDate + "\r\n";
                        tInfo.Text = tInfo.Text + "Expiration Date: " + certmgr1.Cert.ExpirationDate + "\r\n";
                        tInfo.Text = tInfo.Text + "Public Key Algorithm: " + certmgr1.Cert.PublicKeyAlgorithm + "\r\n";
                        tInfo.Text = tInfo.Text + "Public Key Length: " + certmgr1.Cert.PublicKeyLength + "\r\n";
                        tInfo.Text = tInfo.Text + "Public Key: " + certmgr1.Cert.PublicKey + "\r\n";
                    }
                    else
                    {
                        tInfo.Text = "❌ Sertifika bilgileri alınamadı!\r\n";
                        tInfo.Text = tInfo.Text + "Store: " + (storeType == CertStoreTypes.cstUser ? "User" : "LocalMachine") + "\r\n";
                        tInfo.Text = tInfo.Text + "Sertifika Adı: " + certName + "\r\n";
                        tInfo.Text = tInfo.Text + "Bu sertifika seçilebilir ama detayları görüntülenemiyor.\r\n";
                        tInfo.Text = tInfo.Text + "Yine de SSL için kullanılabilir.";
                    }
                }
                catch (Exception ex)
                {
                    WriteLog($"Sertifika bilgileri alınırken hata: {ex.Message}");
                    tInfo.Text = "Sertifika bilgileri alınırken hata oluştu: " + ex.Message;
                }
            }
        }

        private void certmgr1_OnCertList(object sender, CertmgrCertListEventArgs e)
        {
            // Türkçe karakterler için encoding düzeltmesi
            string certSubject = e.CertSubject;
            
            // Eğer sertifika adında bozuk karakterler varsa düzelt
            if (certSubject.Contains("Ä°") || certSubject.Contains("ÅŸ") || certSubject.Contains("Åž"))
            {
                WriteLog($"Bozuk karakterler tespit edildi: {certSubject}");
                
                // Bozuk karakterleri düzelt
                certSubject = certSubject.Replace("Ä°", "İ")
                                       .Replace("ÅŸ", "ş")
                                       .Replace("Åž", "Ş")
                                       .Replace("Ã¼", "ü")
                                       .Replace("Ã‡", "Ç")
                                       .Replace("Ã¶", "ö")
                                       .Replace("Ã§", "ç")
                                       .Replace("Ä±", "ı")
                                       .Replace("ÄŸ", "ğ");
                
                WriteLog($"Düzeltilmiş sertifika adı: {certSubject}");
            }
            
            lcerts.Items.Add(currentStorePrefix + certSubject);
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
