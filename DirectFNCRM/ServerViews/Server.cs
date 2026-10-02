using nsoftware.IPWorks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using static DirectFNCRM.ServerViews.Server;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DirectFNCRM.ServerViews
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
            referance = this;
        }

        public static Server referance;
        public Calisan ActiveCalisan;
        public string HostName = "";
        public string IpAdress = "";
        public bool kontrol = false;
        public bool kontrolToplam = true;
        public string kotrolsaati = "00:05:00";
        public bool LisansSurekontrol = false;
        public string AyarPath = Application.StartupPath + @"\ServerAyarlar.ini";

        public Dictionary<string, string> DictionaryCalisan = new Dictionary<string, string>();
        public ConcurrentQueue<string> AuroCreateQuene = new ConcurrentQueue<string>();

        public string ssoip = "";
        public string ssoport = "";

        public static int GunSonuInterval = 200;
        public static string lastLoginHistory = "";
        public static bool LoginHistoryEnabled = false;
        public static string StatusString = "";

        private static int _acYeniKayit = 0;
        private static int _acGuncelleme = 0;
        private static int _acBasarisiz = 0;
        private static DateTime _acResetDate = DateTime.Today;
        private static readonly Encoding Latin5 = Encoding.GetEncoding("iso-8859-9");
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

        public delegate void listboxislemlerGuncelle(string text);
        private void Server_Load(object sender, EventArgs e)
        {
            try
            {
                timerOnOffCheck.Start();

                ActiveCalisan = (Calisan)this.Tag;

                this.Text += " -  " + ActiveCalisan.Ad + " Ver " + MyTools.KurumVersiyon + " ( " + MyTools.KurumAd + " )";
                try
                {
                    #region IpAdresHostNameOgren

                    HostName = System.Net.Dns.GetHostName();
                    foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        var addr = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();
                        if (addr != null)
                        {
                            if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                            {
                                foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                                {
                                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                    {
                                        IpAdress = ip.Address.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }
                #endregion

                lblDbip.Text = Properties.Settings.Default.DBConnectionString.Split(';')[0];

                StringBuilder okunan = new StringBuilder(1000);
                GetPrivateProfileString("Connection", "SSO_IP", "", okunan, 100, Application.StartupPath + @"\ServerAyarlar.ini");
                ssoip = okunan.ToString().Trim();

                GetPrivateProfileString("Connection", "SSO_PORT", "", okunan, 100, Application.StartupPath + @"\ServerAyarlar.ini");
                ssoport = okunan.ToString().Trim();
                GetPrivateProfileString("Connection", "Servis_PORT", "", okunan, 100, Application.StartupPath + @"\ServerAyarlar.ini");
                if (okunan.ToString().Trim() == "") { } else IpDeamon.LocalPort = Int32.Parse(okunan.ToString().Trim());
                GetPrivateProfileString("Connection", "Client_PORT", "", okunan, 100, Application.StartupPath + @"\ServerAyarlar.ini");
                if (okunan.ToString().Trim() == "") { } else IpDeamonForClilents.LocalPort = Int32.Parse(okunan.ToString().Trim());
                GetPrivateProfileString("Connection", "CepServerServis_PORT", "", okunan, 100, Application.StartupPath + @"\ServerAyarlar.ini");
                if (okunan.ToString().Trim() == "") { } else IpdaemonforCepServers.LocalPort = Int32.Parse(okunan.ToString().Trim());
                GetPrivateProfileString("Connection", "SecureService_PORT", "", okunan, 100, Application.StartupPath + @"\ServerAyarlar.ini");
                if (okunan.ToString().Trim() == "") { } else HttpsService.LocalPort = Int32.Parse(okunan.ToString().Trim());
                GetPrivateProfileString("CertificateInfo", "Certificate", "", okunan, 1000, Application.StartupPath + @"\ServerAyarlar.ini");
                if (okunan.ToString().Trim() == "") { }
                else
                {
                    // CertificateDisplay'i oku (try bloğunun dışında)
                    StringBuilder certDisplay = new StringBuilder(1000);
                    GetPrivateProfileString("CertificateInfo", "CertificateDisplay", "", certDisplay, 1000, Application.StartupPath + @"\ServerAyarlar.ini");
                    
                    try
                    {
                        var sb = new StringBuilder();
                        sb.Append($"=== SSL Sertifika Yükleme Başladı ===\n");
                        sb.Append($"Ham Bilgi: {okunan.ToString()}\n");
                        sb.Append($"Görüntülenen Ad: {certDisplay.ToString()}\n");

                        WriteLog(sb.ToString());
                        string certSubject = "";
                        CertStoreTypes storeType = CertStoreTypes.cstUser;
                        string storeName = "MY";
                        
                        // Yeni format kontrolü (Store|Base64Encoded veya Store|CN=...)
                        if (okunan.ToString().Contains("|"))
                        {
                            string[] parts = okunan.ToString().Split('|');
                            string storeTypeStr = parts[0];
                            string certPart = parts[1];
                            
                            // Base64 decode denemesi
                            try
                            {
                                certSubject = Encoding.UTF8.GetString(Convert.FromBase64String(certPart));
                                if (!certSubject.StartsWith("CN=", StringComparison.OrdinalIgnoreCase))
                                    certSubject = "CN=" + certSubject;
                                WriteLog($"Base64 çözüldü: {certSubject}");
                            }
                            catch
                            {
                                // Base64 decode başarısız, eski format olarak kullan
                                certSubject = certPart;
                                if (!certSubject.StartsWith("CN=", StringComparison.OrdinalIgnoreCase))
                                    certSubject = "CN=" + certSubject;
                                WriteLog($"Düz metin formatı kullanılıyor: {certSubject}");
                            }
                            
                            if (storeTypeStr == "MACHINE")
                            {
                                storeType = (CertStoreTypes)1; // LocalMachine
                                WriteLog($"Depo: LocalMachine");
                            }
                            else
                            {
                                storeType = CertStoreTypes.cstUser;
                                WriteLog($"Depo: User");
                            }
                        }
                        else
                        {
                            // Eski format (sadece Base64)
                            certSubject = Encoding.UTF8.GetString(Convert.FromBase64String(okunan.ToString()));
                            if (!certSubject.StartsWith("CN=", StringComparison.OrdinalIgnoreCase))
                                certSubject = "CN=" + certSubject;
                            WriteLog($"Eski format tespit edildi, önce User depo deneniyor...");
                        }

                        // Belirtilen store'dan yükle
                        WriteLog($"{storeType} deposundan sertifika yükleniyor...");
                        try
                        {
                            HttpsService.SSLCert = new Certificate(storeType, storeName, "", certSubject);
                            HttpsService.SSLEnabled = true;
                            WriteLog($"BAŞARILI: SSL sertifikası {storeType} deposundan yüklendi");
                            WriteLog($"Sertifika: {certSubject}");
                            WriteLog($"Görüntülenen: {certDisplay.ToString()}");
                        }
                        catch (Exception storeEx)
                        {
                            WriteLog($"{storeType} deposundan yükleme başarısız: {storeEx.Message}");
                            
                            // Belirtilen store başarısız olursa diğerini dene
                            CertStoreTypes fallbackStore = (storeType == CertStoreTypes.cstUser) ? (CertStoreTypes)1 : CertStoreTypes.cstUser;
                            WriteLog($"Yedek depo deneniyor: {fallbackStore}");
                            
                            try
                            {
                                HttpsService.SSLCert = new Certificate(fallbackStore, storeName, "", certSubject);
                                HttpsService.SSLEnabled = true;
                                WriteLog($"BAŞARILI: SSL sertifikası yedek depodan ({fallbackStore}) yüklendi");
                                WriteLog($"Sertifika: {certSubject}");
                                WriteLog($"Görüntülenen: {certDisplay.ToString()}");
                            }
                            catch (Exception fallbackEx)
                            {
                                WriteLog($"Yedek depodan yükleme başarısız: {fallbackEx.Message}");
                                WriteLog($"=== SSL Sertifika Yükleme BAŞARISIZ ===");
                                throw fallbackEx;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog($"Base64 çözme başarısız: {ex.Message}");
                        WriteLog($"Eski format deneniyor...");
                        
                        try
                        {
                            // Base64 decode başarısız olursa, eski format olarak dene
                            WriteLog($"User deposundan eski format deneniyor...");
                            HttpsService.SSLCert = new Certificate(CertStoreTypes.cstUser, "MY", "", okunan.ToString());
                            HttpsService.SSLEnabled = true;
                            WriteLog($"BAŞARILI: SSL sertifikası eski format ile User deposundan yüklendi");
                            WriteLog($"Sertifika: {okunan.ToString()}");
                            WriteLog($"Görüntülenen: {certDisplay.ToString()}");
                        }
                        catch (Exception ex2)
                        {
                            WriteLog($"User deposundan eski format başarısız: {ex2.Message}");
                            WriteLog($"LocalMachine deposundan eski format deneniyor...");
                            
                            try
                            {
                                HttpsService.SSLCert = new Certificate((CertStoreTypes)1, "MY", "", okunan.ToString());
                                HttpsService.SSLEnabled = true;
                                WriteLog($"BAŞARILI: SSL sertifikası eski format ile LocalMachine deposundan yüklendi");
                                WriteLog($"Sertifika: {okunan.ToString()}");
                                WriteLog($"Görüntülenen: {certDisplay.ToString()}");
                            }
                            catch (Exception ex3)
                            {
                                WriteLog($"LocalMachine deposundan eski format başarısız: {ex3.Message}");
                                WriteLog($"=== SSL Sertifika Yükleme TAMAMEN BAŞARISIZ ===");
                                MessageBox.Show($"Sertifika yüklenirken hata oluştu:\n{ex3.Message}", "SSL Sertifika Hatası");
                            }
                        }
                    }
                }
                GetPrivateProfileString("Kotrol", "KotrolSaati", "", okunan, 100, Application.StartupPath + @"\ServerAyarlar.ini");
                chkKarmaAcilsin.Checked = MyTools.AyarOku("COMEX_KARMA", "OtomatikAC", 10, AyarPath)._ToBool();

                if (GunSonuInterval < 0) GunSonuInterval = 200;
                txtGunSonuInterval.Text = GunSonuInterval.ToString();

                var saat = okunan.ToString().Trim();
                GetPrivateProfileString("Kotrol", "LoginHistory", "0", okunan, 10, Application.StartupPath + @"\ServerAyarlar.ini");
                LoginHistoryEnabled = okunan.ToString().Trim() == "1";
                if (saat != "")
                {
                    if (saat.Length == 8)
                    {
                        kotrolsaati = saat;
                    }
                }
                MyTools.ServerKey = MyTools.AyarOku("PUSHNOTIFICATION", "ServerKey", 300, AyarPath);
                MyTools.SenderId = MyTools.AyarOku("PUSHNOTIFICATION", "SenderId", 100, AyarPath);

                IpDeamon.Config("OutBufferSize=100000000");
                IpDeamon.Config("InBufferSize=100000000");
                IpDeamon.Config("MaxLineLength=500000");
                IpDeamon.Config("MaxConnections=1000");
                IpDeamon.Config("Encoding=iso-8859-9");
                IpDeamon.Config("TcpNoDelay=true");
                IpDeamon.InvokeThrough = this;
                if (IpDeamon.LocalPort > 0)
                    IpDeamon.Listening = true;


                IpDeamonForClilents.Config("OutBufferSize=100000000");
                IpDeamonForClilents.Config("InBufferSize=100000000");
                IpDeamonForClilents.Config("MaxLineLength=50000");
                IpDeamonForClilents.Config("MaxConnections=1000");
                IpDeamonForClilents.Config("TcpNoDelay=true");
                IpDeamonForClilents.InvokeThrough = this;
                if (IpDeamonForClilents.LocalPort > 0)
                    IpDeamonForClilents.Listening = true;

                IpdaemonforCepServers.Config("OutBufferSize=100000000");
                IpdaemonforCepServers.Config("InBufferSize=100000000");
                IpdaemonforCepServers.Config("MaxLineLength=50000");
                IpdaemonforCepServers.Config("MaxConnections=1000");
                IpdaemonforCepServers.Config("TcpNoDelay=true");
                IpdaemonforCepServers.InvokeThrough = this;
                if (IpdaemonforCepServers.LocalPort > 0)
                    IpdaemonforCepServers.Listening = true;

                HttpsService.Config("OutBufferSize=100000000");
                HttpsService.Config("InBufferSize=100000000");
                HttpsService.Config("MaxLineLength=50000");
                HttpsService.Config("MaxConnections=1000");
                HttpsService.Config("TcpNoDelay=true");
                HttpsService.InvokeThrough = this;
                if (HttpsService.LocalPort > 0)
                    HttpsService.Listening = true;

                try
                {
                    if (ssoip != "")
                    {
                        IpportSso.Config("InBufferSize=10000000");
                        IpportSso.Config("OutBufferSize=10000000");
                        IpportSso.Config("MaxLineLength=50000");
                        IpportSso.InvokeThrough = this;
                        IpportSso.EOL = ((char)3).ToString();
                        IpportSso.Connect(ssoip, 4447);
                    }
                }
                catch (Exception ex)
                {

                    formListeTransaction("Hata:Load" + ex.Message);
                }

            }
            catch (Exception ex)
            {

                MyTools.logyaz("Server Load Hata : " + ex.Message);
          
            }

        }
        public void formListeTransaction(string text)
        {
            if (text != "H")
            {
            //    if (listBoxTransactions.InvokeRequired)
            //    {
            //        listboxislemlerGuncelle lg = new listboxislemlerGuncelle(formListeTransaction);
            //        this.Invoke(lg, new object[] { text });

            //    }
            //    else
            //    {
            //        if (listBoxTransactions.Items.Count > 100)
            //            listBoxTransactions.Items.RemoveAt(listBoxTransactions.Items.Count - 1);
            //        listBoxTransactions.Items.Insert(0, DateTime.Now.ToString() + " : " + text);
            //    }
                MyTools.ServerEkranlogyaz(DateTime.Now.ToString() + " : " + text);
            }

        }
        private void IpDeamon_OnConnected(object sender, nsoftware.IPWorks.IpdaemonConnectedEventArgs e)
        {
            try
            {
                //  listBoxClientList.Items.Insert(0, IpDeamon.Connections[e.ConnectionId].RemoteHost);              

            }
            catch
            {

            }
        }
        private void IpDeamon_OnDisconnected(object sender, nsoftware.IPWorks.IpdaemonDisconnectedEventArgs e)
        {
            listBoxClientList.Items.Remove(IpDeamon.Connections[e.ConnectionId].RemoteHost);
        }
        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {

            MyTools.WritePrivateProfileString("Kotrol", "KotrolSaati", kotrolsaati.ToString(), Application.StartupPath + "\\ServerAyarlar.ini");
            MyTools.AyarYaz("Connection", "SSO_IP", ssoip, AyarPath);
            MyTools.AyarYaz("Connection", "SSO_PORT", ssoport, AyarPath);
            MyTools.AyarYaz("Connection", "Servis_PORT", IpDeamon.LocalPort.ToString(), AyarPath);
            MyTools.AyarYaz("Connection", "Client_PORT", IpDeamonForClilents.LocalPort.ToString(), AyarPath);
            MyTools.AyarYaz("PUSHNOTIFICATION", "ServerKey", MyTools.ServerKey, AyarPath);
            MyTools.AyarYaz("PUSHNOTIFICATION", "SenderId", MyTools.SenderId, AyarPath);
            MyTools.AyarYaz("Connection", "CepServerServis_PORT", IpdaemonforCepServers.LocalPort.ToString(), AyarPath);
            MyTools.AyarYaz("Connection", "SecureService_PORT", HttpsService.LocalPort.ToString(), AyarPath);
            MyTools.AyarYaz("COMEX_KARMA", "OtomatikAC", chkKarmaAcilsin.Checked._ToString(), AyarPath);

            IpDeamon.Listening = false;
            IpDeamonForClilents.Listening = false;
            HttpsService.Listening = false;
            Application.Exit();

        }
        private void txtServisPort_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void IpDeamon_OnDataIn(object sender, nsoftware.IPWorks.IpdaemonDataInEventArgs e)
        {
            try
            {
                Task.Run(() =>
                {
                    crmDFNDataContext crm = new crmDFNDataContext();
                    UserEvent userevent = new UserEvent();

                    userevent.EventTarih = DateTime.Now;
                    userevent.IP = IpDeamon.Connections[e.ConnectionId].RemoteHost;
                    userevent.HostName = IpDeamon.Connections[e.ConnectionId].RemoteHost;
                    string responsestr = "";
                    try
                    {
                        string inputmsg = e.Text;
                        MyTools.logyaz(e.Text);

                        if (inputmsg.Length > 20000 || inputmsg.Length < 5)
                        {
                            if (inputmsg.Substring(0, 3) == "GET")
                            {
                                responsestr = "ERROR";
                                IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                IpDeamon.Connections[e.ConnectionId].Connected = false;
                                return;
                            }
                        }
                        #region Get
                        if (inputmsg.Substring(0, 3) == "GET")
                        {
                            int startpos = inputmsg.IndexOf("/");
                            int endpos = inputmsg.IndexOf(" HTTP");
                            if (endpos > startpos && startpos > 0 && endpos > 0)
                            {
                                string message = inputmsg.Substring(startpos + 1, endpos - startpos - 1);
                                message = message.Replace("%20", " ").Replace("%7C", "|");
                                message = message.TurkceHttpResponse();

                                #region AutoCreateWebCustomer
                                if (message.StartsWith("AutoCreateWebCustomer"))
                                {
                                    AuroCreateQuene.Enqueue(e.ConnectionId + "~" + message);
                                    return;
                                }
                                #endregion
                                #region CRMWEB
                                if (message.StartsWith("CRMWEB"))
                                {
                                    var fieldarray1 = message.Split('|');
                                    if (fieldarray1[0] == "CRMWEB")
                                    {
                                        var Method = fieldarray1[1];
                                        var id = Int32.Parse(fieldarray1[2]);
                                        var calisan = fieldarray1[3];

                                        if (Method.StartsWith("CreateUser"))
                                        {
                                            var usr = crm.Users.FirstOrDefault(x => x.UserID == id);
                                            var response = calisan + " " + " Yeni Kullanıcı açtı ;" + " Açılan kullanıcı = " + usr.UserName;
                                            formListeTransaction(response);
                                            SendToSSO(id);
                                            CalisanlaraKomutGonder("CreateUser|" + response);
                                        }

                                        else if (Method.StartsWith("ChangeUser"))
                                        {
                                            var usr = crm.Users.FirstOrDefault(x => x.UserID == id);
                                            var response = calisan + " Kullanıcı Bilgisi Değiştirdi; Kullanıcı= " + usr.UserName;
                                            formListeTransaction(response);
                                            SendToSSO(id);
                                            CalisanlaraKomutGonder("CreateUser|" + response);
                                        }
                                        else if (Method.StartsWith("SendUserInfo"))
                                        {
                                            var usr = crm.Users.FirstOrDefault(x => x.UserID == id);
                                            var response = calisan + " Kullanıcı Bilgisi Değiştirdi; Kullanıcı= " + usr.UserName;
                                            formListeTransaction(response);
                                            SendToSSO(id);
                                            CalisanlaraKomutGonder("CreateUser|" + response);
                                        }
                                        else if (Method.StartsWith("SendUserMusMail"))
                                        {
                                            var usr = crm.Users.FirstOrDefault(x => x.UserID == id);
                                            var response = calisan + " müşteriye giriş bilgilerini içeren mail gönderdi; Kullanıcı= " + usr.UserName;
                                            formListeTransaction(response);
                                            CalisanlaraKomutGonder("SendUserMusMail|" + response);
                                        }
                                        responsestr = "OK";
                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                        return;
                                    }
                                }
                                #endregion
                                #region WEBKURUMTALEP
                                if (message.StartsWith("WEBKURUMTALEP"))
                                {
                                    var fieldarray1 = message.Split('|');
                                    if (fieldarray1[0] == "WEBKURUMTALEP")
                                    {
                                        var Method = fieldarray1[1];
                                        var id = Int32.Parse(fieldarray1[2]);
                                        var calisan = fieldarray1[2];

                                        if (Method.StartsWith("YeniEkranTalebi"))
                                        {
                                            var usr = crm.KurumTalepleris.FirstOrDefault(x => x.id == id);
                                            var response = calisan + " " + " Yeni Kullanıcı Talebi Gönderdi ;" + " Talep No = " + usr.id;
                                            formListeTransaction(response);
                                            CalisanlaraKomutGonder("WEBKURUMTALEP|" + response);
                                        }
                                        else if (Method.StartsWith("EkranLisansTalebi"))
                                        {
                                            var usr = crm.KurumTalepleris.FirstOrDefault(x => x.id == id);
                                            var response = calisan + " " + " Lisans Değişiklik Talebi Gönderdi ;" + " Talep No = " + usr.id;
                                            formListeTransaction(response);
                                            CalisanlaraKomutGonder("WEBKURUMTALEP|" + response);
                                        }
                                        else if (Method.StartsWith("EkranIptalTalebi"))
                                        {
                                            var usr = crm.KurumTalepleris.FirstOrDefault(x => x.id == id);
                                            var response = calisan + " " + " Iptal Talebi Gönderdi ;" + " Talep No = " + usr.id;
                                            formListeTransaction(response);
                                            CalisanlaraKomutGonder("WEBKURUMTALEP|" + response);
                                        }
                                        else if (Method.StartsWith("KurumTalepOnay"))
                                        {
                                            var usr = crm.KurumTalepleris.FirstOrDefault(x => x.id == id);
                                            var response = calisan + " " + " Bir Kurum Talebi Onayladı ;" + " Talep No = " + usr.id;
                                            formListeTransaction(response);
                                            CalisanlaraKomutGonder("WEBKURUMTALEP|" + response);
                                        }
                                        responsestr = "OK";
                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                        return;
                                    }
                                }
                                #endregion
                                #region FireBaseToken
                                if (message.StartsWith("FireBaseToken"))
                                {
                                    var fieldarray1 = message.Split('|');

                                    if (fieldarray1[0] == "FireBaseToken")
                                    {

                                        var kullaniciadi = "";
                                        var remoteHost = "";

                                        var newuser = crm.Users.FirstOrDefault(x => x.UserName == fieldarray1[1].Trim());
                                        if (newuser != null)
                                        {
                                            #region Guncelle

                                            for (int i = 2; i < fieldarray1.Length; i++)
                                            {
                                                var splitarray = fieldarray1[i].Split(';');
                                                switch (splitarray[0])
                                                {
                                                    case "FireBaseToken":
                                                        newuser.FireBaseToken = splitarray[1].Trim();
                                                        break;
                                                    case "RemoteHost":
                                                        remoteHost = splitarray[1];
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }

                                            if (newuser.MusteriMenseiID == null)
                                                newuser.MusteriMenseiID = 1;

                                            kullaniciadi = newuser.UserName;

                                            crm.SubmitChanges();

                                            #endregion
                                        }


                                        var text = "FireBaseToken ;  user name  = " + kullaniciadi + "; RemoteHost = " + remoteHost;
                                        formListeTransaction(text);

                                        responsestr = "OK";
                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                        return;
                                    }
                                }
                                #endregion

                                #region Musteri_Talepleri
                                if (message.StartsWith("MUSTALEP"))
                                {
                                    var fieldarray1 = message.Split('|');
                                    string username = "";
                                    string hesapkurum = "";
                                    string hesapno = "";
                                    string adsoyad = "";
                                    string TelNo = "";
                                    string Email = "";
                                    string Text = "";
                                    string remoteHost = "";


                                    if (crm.Users.FirstOrDefault(x => x.UserName == fieldarray1[1].Trim()) != null)
                                    {
                                        #region Guncelle
                                        username = fieldarray1[1].Trim();
                                        for (int i = 2; i < fieldarray1.Length; i++)
                                        {
                                            var splitarray = fieldarray1[i].Split(';');
                                            switch (splitarray[0])
                                            {
                                                case "HesapKurum":
                                                    hesapkurum = splitarray[1].Trim();
                                                    break;
                                                case "HesapNo":
                                                    hesapno = splitarray[1].Trim();
                                                    break;
                                                case "AdSoyad":
                                                    adsoyad = splitarray[1].Trim();
                                                    break;
                                                case "TelNo":
                                                    TelNo = splitarray[1].Trim();
                                                    break;
                                                case "Email":
                                                    Email = splitarray[1].Trim();
                                                    break;
                                                case "Text":
                                                    Text = splitarray[1].Trim().Replace("%0A", "\n");
                                                    break;
                                                case "RemoteHost":
                                                    remoteHost = splitarray[1];
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                        MusteriTalepler mt = new MusteriTalepler();
                                        mt.talepdate = DateTime.Now;
                                        mt.username = username;
                                        mt.hesapkurum = hesapkurum;
                                        mt.hesapno = hesapno;
                                        mt.remoteHost = remoteHost;
                                        mt.telno = TelNo;
                                        mt.text = Text;
                                        mt.email = Email;
                                        mt.adsoyad = adsoyad;

                                        crm.MusteriTaleplers.InsertOnSubmit(mt);

                                        crm.SubmitChanges();

                                        #endregion
                                    }
                                    var text2 = "MUSTALEP ;  user name  = " + username + "; RemoteHost = " + remoteHost;
                                    formListeTransaction(text2);

                                    responsestr = "OK";
                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                    return;
                                }
                                #endregion

                                #region MUSTERIBILGILERI

                                if (message.StartsWith("MUSTERIBILGILERI"))
                                {
                                    var fieldarray1 = message.Split('|');
                                    string username = "";
                                    string hesapkurum = "";
                                    string hesapno = "";
                                    string sehir = "";
                                    string TelNo = "";
                                    string Email = "";
                                    string adres = "";
                                    string remoteHost = "";


                                    var newuser = crm.Users.FirstOrDefault(x => x.UserName == fieldarray1[1].Trim());
                                    if (newuser != null)
                                    {
                                        #region Guncelle

                                        username = fieldarray1[1].Trim();
                                        for (int i = 2; i < fieldarray1.Length; i++)
                                        {
                                            var splitarray = fieldarray1[i].Split(';');
                                            switch (splitarray[0])
                                            {
                                                case "HesapKurum":
                                                    hesapkurum = splitarray[1].Trim();
                                                    break;
                                                case "HesapNo":
                                                    hesapno = splitarray[1].Trim();
                                                    break;
                                                case "Sehir":
                                                    sehir = splitarray[1].Trim();
                                                    break;
                                                case "TelNo":
                                                    TelNo = splitarray[1].Trim();
                                                    break;
                                                case "Email":
                                                    Email = splitarray[1].Trim();
                                                    break;
                                                case "Adres":
                                                    adres = splitarray[1].Trim().Replace("%0A", "\n");
                                                    break;
                                                case "RemoteHost":
                                                    remoteHost = splitarray[1];
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                        #region iletisim


                                        if (newuser.iletisimId != null)
                                        {

                                            newuser.Iletisim.Tel1 = TelNo;

                                            newuser.Iletisim.Ceptel = TelNo;


                                            newuser.Iletisim.acikadres = adres;
                                            newuser.Iletisim.email = Email;

                                            var il = MyTools.SehirIdBul(sehir);
                                            if (il != null && il.Id > 0)
                                            {
                                                newuser.Iletisim.IlId = il.Id;
                                                newuser.Iletisim.UlkeId = il.UlkeId;
                                            }

                                        }
                                        else
                                        {
                                            Iletisim ileti = new Iletisim();
                                            ileti.Tel1 = TelNo;

                                            ileti.Ceptel = TelNo;


                                            ileti.acikadres = adres;
                                            ileti.email = Email;

                                            var il = MyTools.SehirIdBul(sehir);
                                            if (il != null && il.Id > 0)
                                            {
                                                ileti.IlId = il.Id;
                                                ileti.UlkeId = il.UlkeId;
                                            }
                                            crm.Iletisims.InsertOnSubmit(ileti);
                                            crm.SubmitChanges();

                                            newuser.iletisimId = ileti.IletisimId;

                                        }
                                        #endregion


                                        crm.SubmitChanges();

                                        #endregion
                                    }




                                    var text2 = "MUSTERIBILGILERI ;  user name  = " + username + "; RemoteHost = " + remoteHost;
                                    formListeTransaction(text2);

                                    responsestr = "OK";
                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                    return;

                                }

                                #endregion

                                #region PUSH

                                if (message.StartsWith("PUSH"))
                                {
                                    var fieldarray1 = message.Split('|');
                                    string komut = "";
                                    string text = "";
                                    string hesapno = "";
                                    string tip = "";
                                    int id = 0;
                                    string calisan = "";
                                    for (int i = 2; i < fieldarray1.Length; i++)
                                    {
                                        var key = fieldarray1[i].Trim().Split(';')[0];

                                        if (key == "Text") text = HttpUtility.UrlDecode(fieldarray1[i].Trim().Split(';')[1]);
                                        else if (key == "HesapNo") hesapno = fieldarray1[i].Trim().Split(';')[1];
                                        else if (key == "Tip") tip = fieldarray1[i].Trim().Split(';')[1];
                                        else if (key == "ID") id = (fieldarray1[i].Trim().Split(';')[1] == "") ? 0 : Int32.Parse(fieldarray1[i].Trim().Split(';')[1]);
                                        else if (key == "Calisan") calisan = fieldarray1[i].Trim().Split(';')[1];
                                    }

                                    komut = fieldarray1[1].Trim();

                                    if (komut == "TekMesaj")
                                    {
                                        var usr = crm.Users.FirstOrDefault(x => x.UserName == hesapno);
                                        MyTools.SendAlarmToAndroidAndIos(usr.FireBaseToken, text);
                                        formListeTransaction(calisan + " " + hesapno + " 'nolu müşteriye  mesaj gönderdi " + "Mesaj : " + text);

                                    }
                                    else if (komut == "TopluMesaj")
                                    {
                                        MyTools.SendAlarmMulti(text);
                                        formListeTransaction(DictionaryCalisan[e.ConnectionId] + " Tüm Kullanıcılara Mesaj Gönderdi " + "Mesaj : " + text);

                                    }
                                    else if (komut == "PUSH")
                                    {
                                        var mesaj = crm.Duyurulars.FirstOrDefault(x => x.id == id);
                                        if (tip == "1")
                                        {

                                            formListeTransaction(calisan + " Tüm Kullanıcılara Push Mesaj Gönderdi " + "Mesaj : " + mesaj.Mesaj);
                                            MyTools.SendAlarmMulti(mesaj.Mesaj);
                                        }
                                        else
                                        {

                                            formListeTransaction(calisan + " Push Mesaı için düzeltme gönderdi " + "Mesaj : " + mesaj.Mesaj);
                                        }
                                        SendToCepServerDuyuru(id, tip);

                                    }
                                    var text2 = komut + " gönderen " + calisan + ";  user name  = " + ((hesapno != "") ? hesapno : "Tüm");

                                    CalisanlaraKomutGonder(komut + "|" + text2);
                                    responsestr = "OK";
                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                    return;

                                }

                                #endregion
                                #region SENTIMENT

                                if (message.StartsWith("SENTIMENT"))
                                {
                                    var fieldarray1 = message.Split('|');
                                    string komut = "";
                                    string sAlgoId = "";
                                    int calisanId = 0;
                                    for (int i = 2; i < fieldarray1.Length; i++)
                                    {
                                        var key = fieldarray1[i].Trim().Split(';')[0];
                                        if (key == "ID") sAlgoId = fieldarray1[i].Trim().Split(';')[1];
                                        else if (key == "Calisan") calisanId = fieldarray1[i].Trim().Split(';')[1]._ToInt();

                                    }

                                    var cal = crm.Calisans.FirstOrDefault(x => x.calisanID == calisanId);
                                    var CalisanAdSoyad = cal.Ad + " " + cal.Soyad;

                                    komut = fieldarray1[1].Trim();

                                    if (komut == "UpdateSentimentAlgo")
                                    {
                                        SentimentAlgoSendToCepServer("Update", sAlgoId._ToInt());


                                    }
                                    else if (komut == "CreateSentimentAlgo")
                                    {
                                        SentimentAlgoSendToCepServer("Update", sAlgoId._ToInt());
                                    }
                                    formListeTransaction(CalisanAdSoyad + " Sentiment algo içeriği gönderdi");

                                    CalisanlaraKomutGonder(CalisanAdSoyad + " Sentiment algo içeriği gönderdi");
                                    responsestr = "OK";
                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                    return;

                                }

                                #endregion


                                if (message.StartsWith("HisseSinyal"))
                                {
                                    var fieldarray1 = message.Split('&');
                                    if (fieldarray1.Length < 2)
                                    {

                                        responsestr = "TANIMLANAMAYAN METOD";
                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                        return;
                                    }
                                    #region KullaniciSorgula
                                    if (fieldarray1[1].Trim() == "KullaniciSorgula")
                                    {

                                        var username = "";
                                        var sonuc = new Sonuc();
                                        var remoteHost = IpDeamon.Connections[e.ConnectionId].RemoteHost;

                                        for (int i = 2; i < fieldarray1.Length; i++)
                                        {
                                            var splitarray = fieldarray1[i].Split('=');
                                            switch (splitarray[0].Trim()._ToEngUp())
                                            {
                                                case "HESAPNO":
                                                    username = splitarray[1].Trim();
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        //var text = "GetCustomerByNumber ;  user name  = " + kullaniciadi + "; RemoteHost = " + remoteHost;
                                        //formListeTransaction(text);

                                        var user = crm.Users.FirstOrDefault(x => x.UserName == username);
                                        if (user != null)
                                        {
                                            if (user.LisansDurum.YayinDurumu == true)
                                            {
                                                var storeuser = new HisseSinyalClass();

                                                storeuser.HESAPNO = user.UserName;
                                                //  storeuser.PASSWORD = MyTools.Sifreleme.Decryp(user.Password);
                                                if (user.Name != "" && user.Name != null)
                                                {
                                                    storeuser.AD = user.Name;
                                                }
                                                else
                                                {
                                                    storeuser.AD = "0";
                                                }

                                                if (user.Surname != "" && user.Surname != null)
                                                {
                                                    storeuser.SOYAD = user.Surname;
                                                }
                                                else
                                                {
                                                    storeuser.SOYAD = "0";
                                                }

                                                if (user.ExpiryDate == null)
                                                {
                                                    storeuser.EXPIREDATE = "0";
                                                }
                                                else
                                                {
                                                    storeuser.EXPIREDATE = user.ExpiryDate.Value.Date.ToString("yyyyMMdd");
                                                }
                                                // storeuser.EXPIREDATE = user.ExpiryDate.Value.Date.ToString("yyyyMMdd");

                                                if (user.Iletisim != null)
                                                {
                                                    if (user.Iletisim.acikadres != null && user.Iletisim.acikadres != "")
                                                        storeuser.ADRES = user.Iletisim.acikadres;
                                                    else { storeuser.ADRES = "0"; }

                                                    if (user.Iletisim.Ceptel != null && user.Iletisim.acikadres != "")
                                                        storeuser.GSM = user.Iletisim.Ceptel;
                                                    else { storeuser.GSM = "0"; }

                                                    if (user.Iletisim.email != null && user.Iletisim.email != "")
                                                        storeuser.EMAIL = user.Iletisim.email;
                                                    else { storeuser.EMAIL = "0"; }

                                                    if (user.Iletisim.Ulke != null)
                                                        storeuser.ULKE = user.Iletisim.Ulke.UlkeAdi;
                                                    else { storeuser.ULKE = "0"; }

                                                    if (user.Iletisim.Il != null)
                                                        storeuser.SEHIR = user.Iletisim.Il.IlAdi;
                                                    else { storeuser.SEHIR = "0"; }
                                                }
                                                else
                                                {
                                                    storeuser.ADRES = "0";
                                                    storeuser.GSM = "0";
                                                    storeuser.EMAIL = "0";
                                                    storeuser.ULKE = "0";
                                                    storeuser.SEHIR = "0";
                                                    storeuser.EXPIREDATE = "20301231";
                                                }

                                                //LİSANSLAR

                                                #region Lisanslar
                                                StringBuilder sb = new StringBuilder();
                                                storeuser.ONOF = user.LisansDurum.YayinDurumu._ToIntStr();
                                                if (user.LisansDurum.PayLP == true)
                                                {
                                                    sb.Append("PayLP;");
                                                }
                                                if (user.LisansDurum.PayL2 == true)
                                                {
                                                    sb.Append("PayL2;");
                                                }
                                                if (user.LisansDurum.Pd2P == true)
                                                {
                                                    sb.Append("Pd2P;");
                                                }
                                                if (user.LisansDurum.PayX == true)
                                                {
                                                    sb.Append("END;");
                                                }
                                                if (user.LisansDurum.PayGS == true)
                                                {
                                                    sb.Append("PayGS;");
                                                }
                                                if (user.LisansDurum.AnPro == true)
                                                {
                                                    sb.Append("AnalizPro;");
                                                }
                                                if (user.LisansDurum.PITE == true)
                                                {
                                                    sb.Append("PITE;");
                                                }
                                                if (user.LisansDurum.COMEX == true)
                                                {
                                                    sb.Append("KRMD1;");
                                                }
                                                if (user.LisansDurum.ViopLP == true)
                                                {
                                                    sb.Append("ViopLP;");
                                                }
                                                if (user.LisansDurum.ViopL2 == true)
                                                {
                                                    sb.Append("ViopL2;");
                                                }
                                                if (user.LisansDurum.Vd2P == true)
                                                {
                                                    sb.Append("Vd2P;");
                                                }
                                                if (user.LisansDurum.ViopGS == true)
                                                {
                                                    sb.Append("ViopGS;");
                                                }
                                                if (user.LisansDurum.MKK == true)
                                                {
                                                    sb.Append("MKK;");
                                                }
                                                if (user.LisansDurum.GKKUL == true)
                                                {
                                                    sb.Append("GKKUL;");
                                                }
                                                if (user.LisansDurum.TARAMA == true)
                                                {
                                                    sb.Append("TARAMA;");
                                                }
                                                if (user.LisansDurum.CME == true)
                                                {
                                                    sb.Append("CME;");
                                                }
                                                #endregion

                                                storeuser.lisansDurum = sb.ToString();
                                                var jsnn = new JavaScriptSerializer().Serialize(storeuser);

                                                IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(jsnn._InsertHeaderHTTP());
                                                IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                return;
                                            }
                                            else if (user.LisansDurum.YayinDurumu == false)
                                            {
                                                var newevent = new UserEvent();


                                                var storeuser = new HisseSinyalClass();

                                                storeuser.HESAPNO = user.UserName;
                                                //  storeuser.PASSWORD = MyTools.Sifreleme.Decryp(user.Password);
                                                if (user.Name != "" && user.Name != null)
                                                {
                                                    storeuser.AD = user.Name;
                                                }
                                                else
                                                {
                                                    storeuser.AD = "0";
                                                }

                                                if (user.Surname != "" && user.Surname != null)
                                                {
                                                    storeuser.SOYAD = user.Surname;
                                                }
                                                else
                                                {
                                                    storeuser.SOYAD = "0";
                                                }

                                                if (user.ExpiryDate == null)
                                                {
                                                    storeuser.EXPIREDATE = "0";
                                                }
                                                else
                                                {
                                                    storeuser.EXPIREDATE = user.ExpiryDate.Value.Date.ToString("yyyyMMdd");
                                                }
                                                // storeuser.EXPIREDATE = user.ExpiryDate.Value.Date.ToString("yyyyMMdd");

                                                if (user.Iletisim != null)
                                                {
                                                    if (user.Iletisim.acikadres != null && user.Iletisim.acikadres != "")
                                                        storeuser.ADRES = user.Iletisim.acikadres;
                                                    else { storeuser.ADRES = "0"; }

                                                    if (user.Iletisim.Ceptel != null && user.Iletisim.acikadres != "")
                                                        storeuser.GSM = user.Iletisim.Ceptel;
                                                    else { storeuser.GSM = "0"; }

                                                    if (user.Iletisim.email != null && user.Iletisim.email != "")
                                                        storeuser.EMAIL = user.Iletisim.email;
                                                    else { storeuser.EMAIL = "0"; }

                                                    if (user.Iletisim.Ulke != null)
                                                        storeuser.ULKE = user.Iletisim.Ulke.UlkeAdi;
                                                    else { storeuser.ULKE = "0"; }

                                                    if (user.Iletisim.Il != null)
                                                        storeuser.SEHIR = user.Iletisim.Il.IlAdi;
                                                    else { storeuser.SEHIR = "0"; }
                                                }
                                                else
                                                {
                                                    storeuser.ADRES = "0";
                                                    storeuser.GSM = "0";
                                                    storeuser.EMAIL = "0";
                                                    storeuser.ULKE = "0";
                                                    storeuser.SEHIR = "0";
                                                    // storeuser.EXPIREDATE = "20301231";
                                                }
                                                var lisans = new LisansDurum();
                                                user.ExpiryDate = DateTime.Now._LastDayOfMonth();
                                                lisans.YayinDurumu = true;
                                                lisans.SPI = true;
                                                lisans.CepYetki = true;
                                                lisans.CepYetkiStart = DateTime.Now;
                                                lisans.CepYetkiEnd = DateTime.Now._LastDayOfMonth();
                                                lisans.COMEX = true;
                                                lisans.KRMD1Start = DateTime.Now;
                                                lisans.KRMD1End = DateTime.Now._LastDayOfMonth();
                                                lisans.Futgck = true;
                                                lisans.WINX = true;
                                                crm.LisansDurums.InsertOnSubmit(lisans);
                                                crm.SubmitChanges();
                                                newevent.SonLisandurumID = lisans.LisansDurumId;
                                                user.LisansDurum = lisans;
                                                crm.SubmitChanges();
                                                newevent.UserId = user.UserID;
                                                newevent.CalisanId = 2;
                                                newevent.EventTarih = DateTime.Now;
                                                newevent.EventTypeId = 1;
                                                crm.UserEvents.InsertOnSubmit(newevent);
                                                crm.SubmitChanges();

                                                #region Lisanslar

                                                StringBuilder sb = new StringBuilder();
                                                storeuser.ONOF = user.LisansDurum.YayinDurumu._ToIntStr();
                                                if (lisans.PayLP == true)
                                                {
                                                    sb.Append("PayLP;");
                                                }
                                                if (lisans.PayL2 == true)
                                                {
                                                    sb.Append("PayL2;");
                                                }
                                                if (lisans.Pd2P == true)
                                                {
                                                    sb.Append("Pd2P;");
                                                }
                                                if (lisans.PayX == true)
                                                {
                                                    sb.Append("END;");
                                                }
                                                if (lisans.PayGS == true)
                                                {
                                                    sb.Append("PayGS;");
                                                }
                                                if (lisans.AnPro == true)
                                                {
                                                    sb.Append("AnalizPro;");
                                                }

                                                if (lisans.PITE == true)
                                                {
                                                    sb.Append("PITE;");
                                                }
                                                if (lisans.COMEX == true)
                                                {
                                                    sb.Append("KRMD1;");
                                                }
                                                if (lisans.ViopLP == true)
                                                {
                                                    sb.Append("ViopLP;");
                                                }
                                                if (lisans.ViopL2 == true)
                                                {
                                                    sb.Append("ViopL2;");
                                                }
                                                if (lisans.Vd2P == true)
                                                {
                                                    sb.Append("Vd2P;");
                                                }
                                                if (lisans.ViopGS == true)
                                                {
                                                    sb.Append("ViopGS;");
                                                }
                                                if (lisans.MKK == true)
                                                {
                                                    sb.Append("MKK;");
                                                }
                                                if (lisans.GKKUL == true)
                                                {
                                                    sb.Append("GKKUL;");
                                                }
                                                if (lisans.TARAMA == true)
                                                {
                                                    sb.Append("TARAMA;");
                                                }
                                                if (lisans.CME == true)
                                                {
                                                    sb.Append("CME;");
                                                }
                                                #endregion

                                                storeuser.lisansDurum = sb.ToString();
                                                var jsnn = new JavaScriptSerializer().Serialize(storeuser);

                                                IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(jsnn._InsertHeaderHTTP());
                                                IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            for (int i = 2; i < fieldarray1.Length; i++)
                                            {
                                                var splitarray = fieldarray1[i].Split('=');
                                                switch (splitarray[0].Trim()._ToEngUp())
                                                {
                                                    case "HESAPNO": username = splitarray[1].Trim(); break;
                                                }
                                            }
                                            StringBuilder bosAlanlar = new StringBuilder();
                                            BosAlanlar bos = new BosAlanlar();
                                            userevent.CalisanId = 2;
                                            userevent.EventTypeId = 1;
                                            user = new User();

                                            if (user.Name == "" || user.Name == null)
                                            {
                                                bosAlanlar.Append("AD:0,");
                                                bos.Ad = "0";
                                            }
                                            else
                                            {
                                                bosAlanlar.Append("AD:1,");
                                                bos.Ad = "1";
                                            }
                                            if (user.Surname == "" || user.Name == null)
                                            {
                                                bosAlanlar.Append("SOYAD:0,");
                                                bos.Soyad = "0";
                                            }
                                            else
                                            {
                                                bosAlanlar.Append("SOYAD:1,");
                                                bos.Soyad = "1";
                                            }

                                            if (user.Iletisim == null)
                                            {
                                                bosAlanlar.Append("Adres:0,GSM:0,Email:0,Ulke:0,Sehir:0 ");
                                                bos.Il = "0";
                                                bos.Ulke = "0";
                                                bos.GSM = "0";
                                                bos.Email = "0";
                                                bos.Adres = "0";
                                            }
                                            else
                                            {
                                                bosAlanlar.Append("Adres:1,GSM:1,Email:1,Ulke:1,Sehir:1 ");

                                                bos.Il = "1";
                                                bos.Ulke = "1";
                                                bos.GSM = "1";
                                                bos.Email = "1";
                                                bos.Adres = "1";
                                            }
                                            user.UserName = username;
                                            user.Password = MyTools.Sifreleme.Encryp("102030");
                                            user.Aciklama = username;
                                            user.BaslangicTarihi = DateTime.Now;
                                            user.PmtsNo = "10158";

                                            var LisansDurum = new LisansDurum();
                                            var expriydate = DateTime.Now._LastDayOfMonth();

                                            LisansDurum.SPI = true;
                                            LisansDurum.Futgck = true;
                                            LisansDurum.WINX = true;
                                            LisansDurum.COMEX = true;
                                            LisansDurum.KRMD1Start = DateTime.Now.Date;
                                            LisansDurum.KRMD1End = expriydate;
                                            user.ProductType = "IDEAL";
                                            user.BaslangicTarihi = DateTime.Now;
                                            user.ExpiryDate = expriydate;
                                            LisansDurum.YayinDurumu = true;
                                            user.StatusId = 1;

                                            #region Gelen_Lisanlari_isle
                                            LisansDurum.CepYetki = true;
                                            LisansDurum.CepYetkiStart = DateTime.Now.Date;
                                            LisansDurum.CepYetkiEnd = expriydate;
                                            #endregion

                                            crm.LisansDurums.InsertOnSubmit(LisansDurum);
                                            crm.SubmitChanges();

                                            user.LisansDurumId = LisansDurum.LisansDurumId;
                                            userevent.SonLisandurumID = LisansDurum.LisansDurumId;

                                            crm.Users.InsertOnSubmit(user);
                                            crm.SubmitChanges();

                                            userevent.UserId = user.UserID;


                                            crm.UserEvents.InsertOnSubmit(userevent);
                                            crm.SubmitChanges();

                                            responsestr = "OK";

                                            var text = "HisseSinyal  CreateCustomer  ;açilan user name  = " + user.UserName + ";  Remeto Ip =" + userevent.IP;
                                            formListeTransaction(text);
                                            CalisanlaraKomutGonder("HisseSinyal_CreateUser|" + text);
                                            SendToSSO(user.UserID);

                                            sonuc.Kod = "Kullanıcı Oluşturuldu.";
                                            sonuc.Aciklama = bosAlanlar.ToString();
                                            sonuc.Status = "OK";

                                            var jsnn = new JavaScriptSerializer().Serialize(sonuc);

                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(jsnn._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                            //responsestr = "Kullanıcı Oluşturuldu.Kullanıcının " + bosAlanlar;
                                        }
                                    }
                                    #endregion

                                    #region KullaniciBilgiGuncelle
                                    else if (fieldarray1[1].Trim() == "KullaniciBilgiGuncelle")
                                    {
                                        var username = "";
                                        var adres = "";
                                        var ulke = "";
                                        var sehir = "";

                                        var ad = "";
                                        var soyad = "";
                                        var mail = "";
                                        var telefon = "";
                                        var temsilci = "";
                                        var sube = "";
                                        var remoteHost = IpDeamon.Connections[e.ConnectionId].RemoteHost;

                                        for (int i = 2; i < fieldarray1.Length; i++)
                                        {
                                            var splitarray = fieldarray1[i].Split('=');
                                            switch (splitarray[0].Trim()._ToEngUp())
                                            {
                                                case "HESAPNO":
                                                    username = splitarray[1].Trim();
                                                    break;
                                                case "ADRES":
                                                    adres = splitarray[1].Trim();
                                                    break;
                                                case "ULKE":
                                                    ulke = splitarray[1].Trim();
                                                    break;
                                                case "IL":
                                                    sehir = splitarray[1].Trim();
                                                    break;
                                                case "AD":
                                                    ad = splitarray[1].Trim();
                                                    break;
                                                case "SOYAD":
                                                    soyad = splitarray[1].Trim();
                                                    break;
                                                case "MAIL":
                                                    mail = splitarray[1].Trim();
                                                    break;
                                                case "GSM":
                                                    telefon = splitarray[1].Trim();
                                                    break;
                                                case "SUBE":
                                                    sube = splitarray[1].Trim();
                                                    break;
                                                case "TEMSILCI":
                                                    temsilci = splitarray[1].Trim();
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        var user = crm.Users.FirstOrDefault(x => x.UserName == username);
                                        if (user != null)
                                        {
                                            if (ad != "")
                                            {
                                                user.Name = ad;
                                            }
                                            if (soyad != "")
                                            {
                                                user.Surname = soyad;
                                            }
                                            else
                                            {
                                                user.Surname = "";
                                            }
                                            if (temsilci != "")
                                            {
                                                user.FXkurum = temsilci;
                                            }
                                            if (user.KurumsalBilgiler == null)
                                            {
                                                if (sube != "")
                                                {
                                                    var krm = new KurumsalBilgiler();
                                                    var subelist = crm.KurumSubes.Select(x => new { x.id, x.SubeAdi }).ToList();

                                                    sube = sube.ToLower();

                                                    var subeItem = subelist.FirstOrDefault(x => x.SubeAdi._ToEngUp() == sube._ToEngUp());
                                                    if (subeItem != null)
                                                    {
                                                        var id = subeItem.id;
                                                        krm.KurumSube = subeItem.SubeAdi;
                                                        krm.kurumhesapno = "10158";
                                                        krm.Not1 = "HisseSinyalCreate";
                                                        crm.KurumsalBilgilers.InsertOnSubmit(krm);
                                                        crm.SubmitChanges();
                                                        user.KurumsalBilgiler = krm;
                                                    }
                                                }
                                            }
                                            if (user.Iletisim == null)
                                            {
                                                Iletisim ileti = new Iletisim();
                                                if (adres != "")
                                                {
                                                    ileti.acikadres = adres.Trim();

                                                }
                                                if (sehir != "")
                                                {
                                                    switch (sehir)
                                                    {
                                                        case "Kahramanmaraş":
                                                            sehir = "K.Maraş";
                                                            break;
                                                        case "Şanlıurfa":
                                                            sehir = "Ş.Urfa";
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                    var il = MyTools.SehirIdBul(sehir);
                                                    if (il != null && il.Id > 0)
                                                        ileti.IlId = il.Id;
                                                }
                                                if (ulke != "")
                                                {
                                                    var ulkeobje = MyTools.UlkeIdBul(ulke);
                                                    if (ulkeobje != null && ulkeobje.Id > 0)
                                                    {
                                                        ileti.UlkeId = ulkeobje.Id;
                                                    }
                                                    if (ulkeobje.Id == 213)
                                                    {
                                                        user.MusteriMenseiID = 1;
                                                    }
                                                    else
                                                    {
                                                        user.MusteriMenseiID = 2;
                                                    }
                                                }
                                                if (mail != "")
                                                {
                                                    ileti.email = mail.Trim();
                                                }
                                                if (telefon != "")
                                                {
                                                    ileti.Ceptel = telefon.Trim();
                                                }
                                                crm.Iletisims.InsertOnSubmit(ileti);
                                                crm.SubmitChanges();
                                                var iletisim = crm.Iletisims.OrderByDescending(x => x.IletisimId).FirstOrDefault();
                                                var iletisimID = iletisim.IletisimId;
                                                user.Iletisim = ileti;
                                                user.Iletisim.IletisimId = iletisimID;
                                                crm.SubmitChanges();
                                            }
                                            else
                                            {
                                                if (adres != "")
                                                {
                                                    user.Iletisim.acikadres = adres;
                                                }
                                                if (telefon != "")
                                                {
                                                    user.Iletisim.Ceptel = telefon;
                                                }
                                                if (ulke != "")
                                                {
                                                    var ulkeobje = MyTools.UlkeIdBul(ulke);
                                                    if (ulkeobje != null && ulkeobje.Id > 0)
                                                        user.Iletisim.UlkeId = ulkeobje.Id;

                                                    if (ulkeobje.Id == 213)
                                                    {
                                                        user.MusteriMenseiID = 1;
                                                    }
                                                    else
                                                    {
                                                        user.MusteriMenseiID = 2;
                                                    }
                                                }
                                                if (sehir != "")
                                                {
                                                    switch (sehir)
                                                    {
                                                        case "Kahramanmaraş":
                                                            sehir = "K.Maraş";
                                                            break;
                                                        case "Şanlıurfa":
                                                            sehir = "Ş.Urfa";
                                                            break;
                                                        default:
                                                            break;
                                                    }

                                                    var il = MyTools.SehirIdBul(sehir);
                                                    if (il != null && il.Id > 0)
                                                        user.Iletisim.IlId = il.Id;
                                                }

                                                if (mail != "")
                                                {
                                                    user.Iletisim.email = mail.Trim();
                                                }

                                                crm.SubmitChanges();
                                                var iletisimID = user.Iletisim.IletisimId;
                                                user.Iletisim = user.Iletisim;
                                                user.Iletisim.IletisimId = iletisimID;
                                                crm.SubmitChanges();
                                            }
                                            var sonuc = new Sonuc();
                                            sonuc.Kod = "Kayıt güüncellenmiştir.";
                                            sonuc.Aciklama = "Kullanıcı bilgileri güncellenmiştir.";
                                            sonuc.Status = "OK";
                                            var jsnn = new JavaScriptSerializer().Serialize(sonuc);

                                            var text = "HisseSinyal  UpdateUser  ;guncellenen user name  = " + user.UserName + ";  Remeto Ip =" + userevent.IP;
                                            formListeTransaction(text);
                                            CalisanlaraKomutGonder("HisseSinyal_UpdateUser|" + text);
                                            SendToSSO(user.UserID);

                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(jsnn._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }
                                        else
                                        {
                                            var sonuc = new Sonuc();
                                            sonuc.Kod = "Hatalı Kullanıcı.";
                                            sonuc.Aciklama = "Kullanıcı Bilgisi bulunamadı.";
                                            sonuc.Status = "ERROR";

                                            var jsnn = new JavaScriptSerializer().Serialize(sonuc);

                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(jsnn._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }
                                    }
                                    #endregion
                                    #region KullaniciBaglantiBilgi
                                    else if (fieldarray1[1].Trim() == "KullaniciBaglantiBilgi")
                                    {
                                        var username = "";
                                        var remoteHost = IpDeamon.Connections[e.ConnectionId].RemoteHost;
                                        for (int i = 2; i < fieldarray1.Length; i++)
                                        {
                                            var splitarray = fieldarray1[i].Split('=');
                                            switch (splitarray[0].Trim()._ToEngUp())
                                            {
                                                case "HESAPNO":
                                                    username = splitarray[1].Trim();
                                                    break;
                                            }
                                        }
                                        var sonucx = SendtoSSOMultiUser(username);
                                        responsestr = sonucx;
                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                        return;
                                    }
                                    #endregion
                                    #region KullaniciListele
                                    if (fieldarray1[1].Trim() == "KullaniciListele")
                                    {
                                        var parametre = "";
                                        for (int i = 2; i < fieldarray1.Length; i++)
                                        {
                                            var splitarray = fieldarray1[i].Split('=');
                                            switch (splitarray[0].Trim()._ToEngUp())
                                            {
                                                case "PARAMETRE":
                                                    parametre = splitarray[1].Trim();
                                                    break;
                                            }
                                        }
                                        // string responsestr = "";
                                        if (parametre == "ALL")
                                        {
                                            var users = crm.Users.Where(x => x.LisansDurum.YayinDurumu == true).ToList();
                                            var listelenecekler = new List<KullaniciListele>();
                                            var sb = new StringBuilder();
                                            foreach (var item in users)
                                            {
                                                sb.Append("Username: " + item.UserName);
                                                //  sb.Append("; PRO:" + item.LisansDurum.ProYetki._ToIntStr());
                                                sb.Append("; CEP:" + item.LisansDurum.CepYetki._ToIntStr());
                                                // sb.Append("|PD1: " + item.LisansDurum.PayL1._ToIntStr());
                                                sb.Append("; PD1P:" + item.LisansDurum.PayLP._ToIntStr());
                                                sb.Append("; PD2:" + item.LisansDurum.PayL2._ToIntStr());
                                                sb.Append("; PD2P:" + item.LisansDurum.Pd2P._ToIntStr());
                                                sb.Append("; END:" + item.LisansDurum.PayX._ToIntStr());
                                                sb.Append("; PIT:" + item.LisansDurum.PayGS._ToIntStr());
                                                sb.Append("; PITE:" + item.LisansDurum.PITE._ToIntStr());
                                                sb.Append("; ANALİZPRO:" + item.LisansDurum.AnPro._ToIntStr());
                                                sb.Append("; MKK:" + item.LisansDurum.MKK._ToIntStr());
                                                sb.Append("; GKKUL:" + item.LisansDurum.GKKUL._ToIntStr());
                                                sb.Append("; TARAMA:" + item.LisansDurum.TARAMA._ToIntStr());
                                                sb.Append("; CME:" + item.LisansDurum.CME._ToIntStr());


                                                // sb.Append;"|VD1: " + item.LisansDurum.ViopL1._ToIntStr());
                                                sb.Append("; VD1P:" + item.LisansDurum.ViopLP._ToIntStr());
                                                sb.Append("; VD2:" + item.LisansDurum.ViopL2._ToIntStr());
                                                sb.Append("; VD2P:" + item.LisansDurum.Vd2P._ToIntStr());
                                                sb.Append("; VIT:" + item.LisansDurum.ViopGS._ToIntStr());
                                                sb.Append("; KRMD1:" + item.LisansDurum.COMEX._ToIntStr() + "|");

                                                #region StartDate
                                                //if (item.LisansDurum.PayL1Start != null)
                                                //    user.PD1SD = item.LisansDurum.PayL1Start.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.PayLPStart != null)
                                                //    user.PD1PSD = item.LisansDurum.PayLPStart.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.PayL2Start != null)
                                                //    user.PD2SD = item.LisansDurum.PayL2Start.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.Pd2PStart != null)
                                                //    user.PD2PSD = item.LisansDurum.Pd2PStart.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.PayXStart != null)
                                                //    user.ENDSD = item.LisansDurum.PayXStart.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.PayGSStart != null)
                                                //    user.PITSD = item.LisansDurum.PayGSStart.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.PayPiteStart != null)
                                                //    user.PITESD = item.LisansDurum.PayPiteStart.Value.ToString("yyyyMMdd");

                                                //if (item.LisansDurum.ViopL1Start != null)
                                                //    user.VD1SD = item.LisansDurum.ViopL1Start.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.ViopLPStart != null)
                                                //    user.VD1PSD = item.LisansDurum.ViopLPStart.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.ViopL2Start != null)
                                                //    user.VD2SD = item.LisansDurum.ViopL2Start.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.ViopGSStart != null)
                                                //    user.VITSD = item.LisansDurum.ViopGSStart.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.KRMD1Start != null)
                                                //    user.KRMD1SD = item.LisansDurum.KRMD1Start.Value.ToString("yyyyMMdd");
                                                #endregion
                                                #region EndDate
                                                //if (item.LisansDurum.PayL1End != null)
                                                //    user.PD1ED = item.LisansDurum.PayL1End.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.PayLPEnd != null)
                                                //    user.PD1PED = item.LisansDurum.PayLPEnd.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.PayL2End != null)
                                                //    user.PD2ED = item.LisansDurum.PayL2End.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.Pd2Pend != null)
                                                //    user.PD2PED = item.LisansDurum.Pd2Pend.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.PayXEnd != null)
                                                //    user.ENDED = item.LisansDurum.PayXEnd.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.PayGSEnd != null)
                                                //    user.PITED = item.LisansDurum.PayGSEnd.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.PayPiteEnd != null)
                                                //    user.PITEED = item.LisansDurum.PayPiteEnd.Value.ToString("yyyyMMdd");

                                                //if (item.LisansDurum.ViopL1End != null)
                                                //    user.VD1ED = item.LisansDurum.ViopL1End.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.ViopLPEnd != null)
                                                //    user.VD1PED = item.LisansDurum.ViopLPEnd.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.ViopL2End != null)
                                                //    user.VD2ED = item.LisansDurum.ViopL2End.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.ViopGSEnd != null)
                                                //    user.VITED = item.LisansDurum.ViopGSEnd.Value.ToString("yyyyMMdd");
                                                //if (item.LisansDurum.KRMD1End != null)
                                                //    user.KRMD1ED = item.LisansDurum.KRMD1End.Value.ToString("yyyyMMdd");


                                                #endregion

                                            }
                                            var sonuc = sb.ToString()._InsertHeaderHTTP();
                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(sonuc);
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }

                                        else if (parametre == "GI")
                                        {
                                            var sb = new StringBuilder();
                                            var gunIci = crm.UserEvents.Where(x => x.EventTarih.Value.Date == DateTime.Now.Date && x.EventTarih.Value.Year == DateTime.Now.Year && x.EventTarih.Value.Day == DateTime.Now.Day && x.EventTypeId == 6).ToList();
                                            foreach (var item in gunIci)
                                            {
                                                var user = crm.Users.Where(x => x.UserID == item.UserId).FirstOrDefault();
                                                sb.Append("Username: " + user.UserName);
                                                //  sb.Append("; PRO:" + item.LisansDurum.ProYetki._ToIntStr());
                                                sb.Append("; CEP:" + item.LisansDurum.CepYetki._ToIntStr());
                                                // sb.Append("|PD1: " + item.LisansDurum.PayL1._ToIntStr());
                                                sb.Append("; PD1P:" + item.LisansDurum.PayLP._ToIntStr());
                                                sb.Append("; PD2:" + item.LisansDurum.PayL2._ToIntStr());
                                                sb.Append("; PD2P:" + item.LisansDurum.Pd2P._ToIntStr());
                                                sb.Append("; END:" + item.LisansDurum.PayX._ToIntStr());
                                                sb.Append("; PIT:" + item.LisansDurum.PayGS._ToIntStr());
                                                sb.Append("; PITE:" + item.LisansDurum.PITE._ToIntStr());
                                                sb.Append("; ABALİZPRO:" + item.LisansDurum.AnPro._ToIntStr());
                                                sb.Append("; MKK:" + item.LisansDurum.MKK._ToIntStr());
                                                sb.Append("; GKKUL:" + item.LisansDurum.GKKUL._ToIntStr());
                                                sb.Append("; TARAMA:" + item.LisansDurum.TARAMA._ToIntStr());
                                                sb.Append("; CME:" + item.LisansDurum.CME._ToIntStr());

                                                // sb.Append;"|VD1: " + item.LisansDurum.ViopL1._ToIntStr());
                                                sb.Append("; VD1P:" + item.LisansDurum.ViopLP._ToIntStr());
                                                sb.Append("; VD2:" + item.LisansDurum.ViopL2._ToIntStr());
                                                sb.Append("; VD2P:" + item.LisansDurum.Vd2P._ToIntStr());
                                                sb.Append("; VIT:" + item.LisansDurum.ViopGS._ToIntStr());
                                                sb.Append("; KRMD1:" + item.LisansDurum.COMEX._ToIntStr() + "|");

                                            }
                                            var sonuc = sb.ToString()._InsertHeaderHTTP();
                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(sonuc);
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }
                                        else
                                        {
                                            var sonuc = new Sonuc();
                                            sonuc.Kod = "Hatalı Parametre.";
                                            sonuc.Aciklama = "Geçersiz parametre kullanımı.";
                                            sonuc.Status = "ERROR";

                                            var jsnn = new JavaScriptSerializer().Serialize(sonuc);

                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(jsnn._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }

                                    }
                                    #endregion
                                    #region LisansGuncelleme
                                    if (fieldarray1[1].Trim() == "LisansGuncelle")
                                    {
                                        var sonuc = new Sonuc();
                                        #region Degiskenler
                                        var username = "";
                                        //pay
                                        var pd1p = false;
                                        var pd2 = false;
                                        var pd2p = false;
                                        var end = false;
                                        var pit = false;
                                        var pite = false;
                                        //viop
                                        var vd1p = false;
                                        var vd2 = false;
                                        var vd2p = false;
                                        var vit = false;
                                        var krmd1 = false;
                                        var mkk = false;
                                        var gkkul = false;
                                        var tarama = false;
                                        var cme = false;

                                        var anpro = false;


                                        var pd1pSd = "";
                                        var pd2Sd = "";
                                        var pd2pSd = "";
                                        var endSd = "";
                                        var pitSd = "";
                                        var piteSd = "";

                                        var anproSd = "";
                                        var mkkSd = "";
                                        var gkkulSd = "";
                                        var taramaSd = "";
                                        var cmeSd = "";

                                        var pd1Ed = "";
                                        var pd1pEd = "";
                                        var pd2Ed = "";
                                        var pd2pEd = "";
                                        var endEd = "";
                                        var pitEd = "";
                                        var piteEd = "";

                                        var anproEd = "";
                                        var mkkEd = "";
                                        var gkkulEd = "";
                                        var taramaEd = "";
                                        var cmeEd = "";

                                        var vd1Sd = "";
                                        var vd1pSd = "";
                                        var vd2Sd = "";
                                        var vd2pSd = "";
                                        var vitSd = "";
                                        var krmd1Sd = "";


                                        var vd1Ed = "";
                                        var vd1pEd = "";
                                        var vd2Ed = "";
                                        var vd2pEd = "";
                                        var vitEd = "";
                                        var krmd1Ed = "";

                                        var karmaGeldi = false;
                                        var Pd1pGeldi = false;
                                        var Pd2Geldi = false;
                                        var Pd2PGeldi = false;
                                        var Vd1pGeldi = false;
                                        var Vd2Geldi = false;
                                        var Vd2PGeldi = false;
                                        var VitGeldi = false;
                                        var PayGSGeldi = false;
                                        var PiteGeldi = false;
                                        var endGeldi = false;
                                        var anproGeldi = false;
                                        var mkkGeldi = false;
                                        var gkkulGeldi = false;
                                        var taramaGeldi = false;
                                        var cmeGeldi = false;

                                        var oncekiLisansDurum = false;
                                        #endregion

                                        #region Parse
                                        for (int i = 2; i < fieldarray1.Length; i++)
                                        {
                                            var splitarray = fieldarray1[i].Split('=');
                                            switch (splitarray[0].Trim())
                                            {
                                                case "USERNAME": username = splitarray[1].Trim(); break;
                                                // case "CEP": cep = splitarray[1].Trim()._ToBool();  cepGeldi = true; break;
                                                // case "PD1": pd1 = splitarray[1].Trim()._ToBool(); Pd1Geldi = true; break;
                                                case "PD1P": pd1p = splitarray[1].Trim()._ToBool(); Pd1pGeldi = true; break;
                                                case "PD2": pd2 = splitarray[1].Trim()._ToBool(); Pd2Geldi = true; break;
                                                case "PD2P": pd2p = splitarray[1].Trim()._ToBool(); Pd2PGeldi = true; break;
                                                case "END": end = splitarray[1].Trim()._ToBool(); endGeldi = true; break;
                                                case "PIT": pit = splitarray[1].Trim()._ToBool(); PayGSGeldi = true; break;
                                                case "PITE": pite = splitarray[1].Trim()._ToBool(); PiteGeldi = true; break;
                                                // case "VD1": vd1 = splitarray[1].Trim()._ToBool(); Vd1Geldi = true; break;
                                                case "VD1P": vd1p = splitarray[1].Trim()._ToBool(); Vd1pGeldi = true; break;
                                                case "VD2": vd2 = splitarray[1].Trim()._ToBool(); Vd2Geldi = true; break;
                                                case "VD2P": vd2p = splitarray[1].Trim()._ToBool(); Vd2PGeldi = true; break;
                                                case "VIT": vit = splitarray[1].Trim()._ToBool(); VitGeldi = true; break;
                                                case "KRMD1": krmd1 = splitarray[1].Trim()._ToBool(); karmaGeldi = true; break;
                                                case "ANALİZPRO": anpro = splitarray[1].Trim()._ToBool(); anproGeldi = true; break;
                                                case "MKK": mkk = splitarray[1].Trim()._ToBool(); mkkGeldi = true; break;
                                                case "GKKUL": gkkul = splitarray[1].Trim()._ToBool(); gkkulGeldi = true; break;
                                                case "TARAMA": tarama = splitarray[1].Trim()._ToBool(); taramaGeldi = true; break;
                                                case "CME": cme = splitarray[1].Trim()._ToBool(); cmeGeldi = true; break;
                                                // case "PD1SD": pd1Sd = splitarray[1].Trim(); break;
                                                case "PD1PSD": pd1pSd = splitarray[1].Trim(); break;
                                                case "PD2SD": pd2Sd = splitarray[1].Trim(); break;
                                                case "PD2PSD": pd2pSd = splitarray[1].Trim(); break;
                                                case "ENDSD": endSd = splitarray[1].Trim(); break;
                                                case "PITSD": pitSd = splitarray[1].Trim(); break;
                                                case "PITESD": piteSd = splitarray[1].Trim(); break;
                                                case "VD1SD": vd1Sd = splitarray[1].Trim(); break;
                                                case "VD1PSD": vd1pSd = splitarray[1].Trim(); break;
                                                case "VD2SD": vd2Sd = splitarray[1].Trim(); break;
                                                case "VD2PSD": vd2pSd = splitarray[1].Trim(); break;
                                                case "VITSD": vitSd = splitarray[1].Trim(); break;
                                                case "KRMD1SD": krmd1Sd = splitarray[1].Trim(); break;
                                                case "ANPROSD": anproSd = splitarray[1].Trim(); break;
                                                case "MKKSD": mkkSd = splitarray[1].Trim(); break;
                                                case "GKKULSD": gkkulSd = splitarray[1].Trim(); break;
                                                case "TARAMASD": taramaSd = splitarray[1].Trim(); break;
                                                case "CMESD": cmeSd = splitarray[1].Trim(); break;
                                                case "PD1ED": pd1Ed = splitarray[1].Trim(); break;
                                                case "PD1PED": pd1pEd = splitarray[1].Trim(); break;
                                                case "PD2ED": pd2Ed = splitarray[1].Trim(); break;
                                                case "PD2PED": pd2pEd = splitarray[1].Trim(); break;
                                                case "ENDED": endEd = splitarray[1].Trim(); break;
                                                case "PITED": pitEd = splitarray[1].Trim(); break;
                                                case "PITEED": piteEd = splitarray[1].Trim(); break;
                                                case "VD1ED": vd1Ed = splitarray[1].Trim(); break;
                                                case "VD1PED": vd1pEd = splitarray[1].Trim(); break;
                                                case "VD2ED": vd2Ed = splitarray[1].Trim(); break;
                                                case "VD2PED": vd2pEd = splitarray[1].Trim(); break;
                                                case "VITED": vitEd = splitarray[1].Trim(); break;
                                                case "KRMD1ED": krmd1Ed = splitarray[1].Trim(); break;
                                                case "ANPROED": anproEd = splitarray[1].Trim(); break;
                                                case "MKKED": mkkEd = splitarray[1].Trim(); break;
                                                case "GKKULED": gkkulEd = splitarray[1].Trim(); break;
                                                case "TARAMAED": taramaEd = splitarray[1].Trim(); break;
                                                case "CMEED": cmeEd = splitarray[1].Trim(); break;

                                            }
                                        }

                                        #endregion
                                        var user = crm.Users.FirstOrDefault(x => x.UserName == username);
                                        if (user != null)
                                        {
                                            oncekiLisansDurum = user.LisansDurum.YayinDurumu;

                                            var lisansdurum = new LisansDurum();

                                            lisansdurum.CepYetki = true;
                                            lisansdurum.YayinDurumu = user.LisansDurum.YayinDurumu;
                                            var kaptianlar = new StringBuilder();
                                            var acilanlar = new StringBuilder();

                                            #region PD1P
                                            if (Pd1pGeldi == true)
                                            {
                                                if (pd1p == true)
                                                {
                                                    if (pd1pSd != "" && pd1pEd != "")
                                                    {
                                                        if (pd1p && BuAyGelecekAyKontrolu(pd1pSd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.PayLP = true;
                                                            lisansdurum.PayL1 = true;
                                                            if (user.LisansDurum.PayLP == false)
                                                                acilanlar.Append("PayLP");

                                                        }
                                                        lisansdurum.PayL1Start = pd1pSd.StrinToDateTime();
                                                        lisansdurum.PayL1End = pd1pEd.StrinToDateTime();
                                                        lisansdurum.PayLPStart = pd1pSd.StrinToDateTime();
                                                        lisansdurum.PayLPEnd = pd1pEd.StrinToDateTime();
                                                    }
                                                    else
                                                    {
                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "PD1P start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.PayLP = user.LisansDurum.PayLP;
                                                    lisansdurum.PayLPStart = user.LisansDurum.PayLPStart;
                                                    lisansdurum.PayLPEnd = DateTime.Now._LastDayOfMonth();
                                                    lisansdurum.PayL1 = user.LisansDurum.PayL1;
                                                    lisansdurum.PayL1Start = user.LisansDurum.PayL1Start;
                                                    lisansdurum.PayL1End = DateTime.Now._LastDayOfMonth();

                                                    if (user.LisansDurum.PayL2 == true)
                                                    {
                                                        lisansdurum.PayL2 = user.LisansDurum.PayL2;
                                                        lisansdurum.PayL2Start = user.LisansDurum.PayL2Start;
                                                        lisansdurum.PayL2End = DateTime.Now._LastDayOfMonth();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.PayLP = user.LisansDurum.PayLP;
                                                lisansdurum.PayLPStart = user.LisansDurum.PayLPStart;
                                                lisansdurum.PayLPEnd = user.LisansDurum.PayLPEnd;
                                                lisansdurum.PayL1 = user.LisansDurum.PayL1;
                                                lisansdurum.PayL1Start = user.LisansDurum.PayL1Start;
                                                lisansdurum.PayL1End = user.LisansDurum.PayL1End;
                                            }
                                            #endregion
                                            #region PD2
                                            if (Pd2Geldi == true)
                                            {
                                                if (pd2 == true)
                                                {
                                                    if (pd2Sd != "" && pd2Ed != "")
                                                    {
                                                        if (pd2 && BuAyGelecekAyKontrolu(pd2Sd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.PayL2 = true;
                                                            lisansdurum.PayL1 = true;
                                                            lisansdurum.PayLP = true;
                                                            if (user.LisansDurum.PayL2 == false)
                                                                acilanlar.Append("PayL2");
                                                        }
                                                        lisansdurum.PayL2Start = pd2Sd.StrinToDateTime();
                                                        lisansdurum.PayL2End = pd2Ed.StrinToDateTime();
                                                        lisansdurum.PayL1Start = pd2Sd.StrinToDateTime();
                                                        lisansdurum.PayL1End = pd2Ed.StrinToDateTime();
                                                        lisansdurum.PayLPStart = pd2Sd.StrinToDateTime();
                                                        lisansdurum.PayLPEnd = pd2Ed.StrinToDateTime();
                                                    }
                                                    else
                                                    {

                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "PD2 start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.PayL2 = user.LisansDurum.PayL2;
                                                    lisansdurum.PayL2Start = user.LisansDurum.PayL2Start;
                                                    lisansdurum.PayL2End = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.PayL2 = user.LisansDurum.PayL2;
                                                lisansdurum.PayL2Start = user.LisansDurum.PayL2Start;
                                                lisansdurum.PayL2End = user.LisansDurum.PayL2End;
                                            }
                                            #endregion
                                            #region PD2P
                                            if (Pd2PGeldi == true)
                                            {
                                                if (pd2p == true)
                                                {
                                                    if (pd2pSd != "" && pd2pEd != "")
                                                    {
                                                        if (pd2p && BuAyGelecekAyKontrolu(pd2pSd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.PayL2 = true;
                                                            lisansdurum.PayL1 = true;
                                                            lisansdurum.PayLP = true;
                                                            lisansdurum.Pd2P = true;
                                                            if (user.LisansDurum.Pd2P == false)
                                                                acilanlar.Append("Pd2P");
                                                        }
                                                        lisansdurum.PayL2Start = pd2Sd.StrinToDateTime();
                                                        lisansdurum.PayL2End = pd2Ed.StrinToDateTime();
                                                        lisansdurum.Pd2PStart = pd2pSd.StrinToDateTime();
                                                        lisansdurum.Pd2PEnd = pd2pEd.StrinToDateTime();
                                                        lisansdurum.PayL1Start = pd2Sd.StrinToDateTime();
                                                        lisansdurum.PayL1End = pd2Ed.StrinToDateTime();
                                                        lisansdurum.PayLPStart = pd2Sd.StrinToDateTime();
                                                        lisansdurum.PayLPEnd = pd2Ed.StrinToDateTime();
                                                    }
                                                    else
                                                    {

                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "PD2P start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.Pd2P = user.LisansDurum.Pd2P;
                                                    lisansdurum.Pd2PStart = user.LisansDurum.Pd2PStart;
                                                    lisansdurum.Pd2PEnd = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.Pd2P = user.LisansDurum.Pd2P;
                                                lisansdurum.Pd2PStart = user.LisansDurum.Pd2PStart;
                                                lisansdurum.Pd2PEnd = user.LisansDurum.Pd2PEnd;
                                            }
                                            #endregion

                                            #region END
                                            if (endGeldi == true)
                                            {
                                                if (end == true)
                                                {
                                                    if (endSd != "" && endEd != "")
                                                    {
                                                        if (end && BuAyGelecekAyKontrolu(endSd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.PayX = true;
                                                            if (user.LisansDurum.PayX == false)
                                                                acilanlar.Append("END");
                                                        }
                                                        lisansdurum.PayXStart = endSd.StrinToDateTime();
                                                        lisansdurum.PayXEnd = endEd.StrinToDateTime();
                                                    }
                                                    else
                                                    {
                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "END(PayEndeks) start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.PayX = user.LisansDurum.PayX;
                                                    lisansdurum.PayXStart = user.LisansDurum.PayXStart;
                                                    lisansdurum.PayXEnd = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.PayX = user.LisansDurum.PayX;
                                                lisansdurum.PayXStart = user.LisansDurum.PayXStart;
                                                lisansdurum.PayXEnd = user.LisansDurum.PayXEnd;

                                            }
                                            #endregion
                                            #region PIT
                                            if (PayGSGeldi == true)
                                            {
                                                if (pit == true)
                                                {
                                                    if (pitSd != "" && pitEd != "")
                                                    {
                                                        if (pit && BuAyGelecekAyKontrolu(pitSd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.PayGS = true;
                                                            if (user.LisansDurum.PayGS == false)
                                                                acilanlar.Append("PayGS");
                                                        }
                                                        lisansdurum.PayGSStart = pitSd.StrinToDateTime();
                                                        lisansdurum.PayGSEnd = pitEd.StrinToDateTime();
                                                    }
                                                    else
                                                    {

                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "PIT start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.PayGS = user.LisansDurum.PayGS;
                                                    lisansdurum.PayGSStart = user.LisansDurum.PayGSStart;
                                                    lisansdurum.PayGSEnd = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.PayGS = user.LisansDurum.PayGS;
                                                lisansdurum.PayGSStart = user.LisansDurum.PayGSStart;
                                                lisansdurum.PayGSEnd = user.LisansDurum.PayGSEnd;

                                            }
                                            #endregion
                                            #region PITE
                                            if (PiteGeldi == true)
                                            {
                                                if (pite == true)
                                                {
                                                    if (piteSd != "" && piteEd != "")
                                                    {
                                                        if (pite && BuAyGelecekAyKontrolu(piteSd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.PITE = true;
                                                            if (user.LisansDurum.PITE == false)
                                                                acilanlar.Append("PITE");
                                                        }
                                                        lisansdurum.PayPiteStart = piteSd.StrinToDateTime();
                                                        lisansdurum.PayPiteEnd = piteEd.StrinToDateTime();
                                                    }
                                                    else
                                                    {

                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "PITE start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.PITE = user.LisansDurum.PITE;
                                                    lisansdurum.PayPiteStart = user.LisansDurum.PayPiteStart;
                                                    lisansdurum.PayPiteEnd = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.PITE = user.LisansDurum.PITE;
                                                lisansdurum.PayPiteStart = user.LisansDurum.PayPiteStart;
                                                lisansdurum.PayPiteEnd = user.LisansDurum.PayPiteEnd;
                                            }
                                            #endregion

                                            #region VD1P
                                            if (Vd1pGeldi == true)
                                            {
                                                if (vd1p == true)
                                                {
                                                    if (vd1pSd != "" && vd1pEd != "")
                                                    {
                                                        if (vd1p && BuAyGelecekAyKontrolu(vd1pSd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.ViopLP = true;
                                                            lisansdurum.ViopL1 = true;
                                                            if (user.LisansDurum.ViopLP == false)
                                                                acilanlar.Append("ViopLP");
                                                        }
                                                        lisansdurum.ViopL1Start = vd1pSd.StrinToDateTime();
                                                        lisansdurum.ViopL1End = vd1pEd.StrinToDateTime();
                                                        lisansdurum.ViopLPStart = vd1pSd.StrinToDateTime();
                                                        lisansdurum.ViopLPEnd = vd1pEd.StrinToDateTime();
                                                    }
                                                    else
                                                    {
                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "VD1P start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.ViopLP = user.LisansDurum.ViopLP;
                                                    lisansdurum.ViopLPStart = user.LisansDurum.ViopLPStart;
                                                    lisansdurum.ViopLPEnd = DateTime.Now._LastDayOfMonth();
                                                    lisansdurum.ViopL1 = user.LisansDurum.ViopL1;
                                                    lisansdurum.ViopL1Start = user.LisansDurum.ViopL1Start;
                                                    lisansdurum.ViopL1End = DateTime.Now._LastDayOfMonth();
                                                    if (user.LisansDurum.ViopL2 == true)
                                                    {
                                                        lisansdurum.ViopL2 = user.LisansDurum.ViopL2;
                                                        lisansdurum.ViopL2Start = user.LisansDurum.ViopL2Start;
                                                        lisansdurum.ViopL2End = DateTime.Now._LastDayOfMonth();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.ViopLP = user.LisansDurum.ViopLP;
                                                lisansdurum.ViopLPStart = user.LisansDurum.ViopLPStart;
                                                lisansdurum.ViopLPEnd = user.LisansDurum.ViopLPEnd;
                                                lisansdurum.ViopL1 = user.LisansDurum.ViopL1;
                                                lisansdurum.ViopL1Start = user.LisansDurum.ViopL1Start;
                                                lisansdurum.ViopL1End = user.LisansDurum.ViopL1End;
                                            }
                                            #endregion
                                            #region VD2
                                            if (Vd2Geldi == true)
                                            {
                                                if (vd2 == true)
                                                {
                                                    if (vd2Sd != "" && vd2Ed != "")
                                                    {
                                                        if (vd2 && BuAyGelecekAyKontrolu(vd2Sd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.ViopL2 = true;
                                                            lisansdurum.ViopLP = true;
                                                            lisansdurum.ViopL1 = true;
                                                            if (user.LisansDurum.ViopL2 == false)
                                                                acilanlar.Append("ViopL2");
                                                        }
                                                        lisansdurum.ViopL2Start = vd2Sd.StrinToDateTime();
                                                        lisansdurum.ViopL2End = vd2Ed.StrinToDateTime();
                                                        lisansdurum.ViopL1Start = vd2Sd.StrinToDateTime();
                                                        lisansdurum.ViopL1End = vd2Ed.StrinToDateTime();
                                                        lisansdurum.ViopLPStart = vd2Sd.StrinToDateTime();
                                                        lisansdurum.ViopLPEnd = vd2Ed.StrinToDateTime();
                                                    }
                                                    else
                                                    {
                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "VD2 start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.ViopL2 = user.LisansDurum.ViopL2;
                                                    lisansdurum.ViopL2Start = user.LisansDurum.ViopL2Start;
                                                    lisansdurum.ViopL2End = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.ViopL2 = user.LisansDurum.ViopL2;
                                                lisansdurum.ViopL2Start = user.LisansDurum.ViopL2Start;
                                                lisansdurum.ViopL2End = user.LisansDurum.ViopL2End;
                                            }
                                            #endregion
                                            #region VD2P
                                            if (Vd2PGeldi == true)
                                            {
                                                if (vd2p == true)
                                                {
                                                    if (vd2pSd != "" && vd2pEd != "")
                                                    {
                                                        if (vd2p && BuAyGelecekAyKontrolu(vd2pSd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.ViopL2 = true;
                                                            lisansdurum.Vd2P = true;
                                                            lisansdurum.ViopLP = true;
                                                            lisansdurum.ViopL1 = true;
                                                            if (user.LisansDurum.Vd2P == false)
                                                                acilanlar.Append("Vd2P");
                                                        }
                                                        lisansdurum.Vd2PStart = vd2pSd.StrinToDateTime();
                                                        lisansdurum.Vd2PEnd = vd2pEd.StrinToDateTime();
                                                        lisansdurum.ViopL2Start = vd2Sd.StrinToDateTime();
                                                        lisansdurum.ViopL2End = vd2Ed.StrinToDateTime();
                                                        lisansdurum.ViopL1Start = vd2Sd.StrinToDateTime();
                                                        lisansdurum.ViopL1End = vd2Ed.StrinToDateTime();
                                                        lisansdurum.ViopLPStart = vd2Sd.StrinToDateTime();
                                                        lisansdurum.ViopLPEnd = vd2Ed.StrinToDateTime();
                                                    }
                                                    else
                                                    {
                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "VD2P start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.Vd2P = user.LisansDurum.Vd2P;
                                                    lisansdurum.Vd2PStart = user.LisansDurum.Vd2PStart;
                                                    lisansdurum.Vd2PEnd = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.Vd2P = user.LisansDurum.Vd2P;
                                                lisansdurum.Vd2PStart = user.LisansDurum.Vd2PStart;
                                                lisansdurum.Vd2PEnd = user.LisansDurum.Vd2PEnd;
                                            }
                                            #endregion
                                            #region VIT
                                            if (VitGeldi == true)
                                            {
                                                if (vit == true)
                                                {
                                                    if (vitSd != "" && vitEd != "")
                                                    {
                                                        if (vit && BuAyGelecekAyKontrolu(vitSd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.ViopGS = true;
                                                            if (user.LisansDurum.ViopGS == false)
                                                                acilanlar.Append("ViopGS");
                                                        }
                                                        lisansdurum.ViopGSStart = vitSd.StrinToDateTime();
                                                        lisansdurum.ViopGSEnd = vitEd.StrinToDateTime();
                                                    }
                                                    else
                                                    {
                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "VIT start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.ViopGS = user.LisansDurum.ViopGS;
                                                    lisansdurum.ViopGSStart = user.LisansDurum.ViopGSStart;
                                                    lisansdurum.ViopGSEnd = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.ViopGS = user.LisansDurum.ViopGS;
                                                lisansdurum.ViopGSStart = user.LisansDurum.ViopGSStart;
                                                lisansdurum.ViopGSEnd = user.LisansDurum.ViopGSEnd;
                                            }
                                            #endregion
                                            #region KRMD1
                                            if (karmaGeldi == true)
                                            {
                                                if (krmd1 == true)
                                                {
                                                    if (krmd1Sd != "" && krmd1Ed != "")
                                                    {
                                                        if (krmd1 && BuAyGelecekAyKontrolu(krmd1Sd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.COMEX = true;
                                                            if (user.LisansDurum.COMEX == false)
                                                                acilanlar.Append("KRMD1");
                                                        }
                                                        lisansdurum.KRMD1Start = krmd1Sd.StrinToDateTime();
                                                        lisansdurum.KRMD1End = krmd1Ed.StrinToDateTime();
                                                    }
                                                    else
                                                    {
                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "KRMD1 start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.COMEX = user.LisansDurum.COMEX;
                                                    lisansdurum.KRMD1Start = user.LisansDurum.KRMD1Start;
                                                    lisansdurum.KRMD1End = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.COMEX = user.LisansDurum.COMEX;
                                                lisansdurum.KRMD1Start = user.LisansDurum.KRMD1Start;
                                                lisansdurum.KRMD1End = user.LisansDurum.KRMD1End;
                                            }
                                            #endregion

                                            #region ANALİZPRO
                                            if (anproGeldi == true)
                                            {
                                                if (anpro == true)
                                                {
                                                    if (anproSd != "" && anproEd != "")
                                                    {
                                                        if (anpro && BuAyGelecekAyKontrolu(anproSd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.AnPro = true;
                                                            if (user.LisansDurum.AnPro == false)
                                                                acilanlar.Append("ANALİZPRO");
                                                        }
                                                        lisansdurum.AnProStart = anproSd.StrinToDateTime();
                                                        lisansdurum.AnProEnd = anproEd.StrinToDateTime();
                                                    }
                                                    else
                                                    {
                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "ANALİZ PRO start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.COMEX = user.LisansDurum.COMEX;
                                                    lisansdurum.KRMD1Start = user.LisansDurum.KRMD1Start;
                                                    lisansdurum.KRMD1End = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.AnPro = user.LisansDurum.AnPro;
                                                lisansdurum.AnProStart = user.LisansDurum.AnProStart;
                                                lisansdurum.AnProEnd = user.LisansDurum.AnProEnd;
                                            }
                                            #endregion

                                            #region MKK
                                            if (mkkGeldi == true)
                                            {
                                                if (mkk == true)
                                                {
                                                    if (mkkSd != "" && mkkEd != "")
                                                    {
                                                        if (mkk && BuAyGelecekAyKontrolu(mkkSd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.MKK = true;
                                                            if (user.LisansDurum.MKK == false)
                                                                acilanlar.Append("MKK");
                                                        }
                                                        lisansdurum.MKKStart = mkkSd.StrinToDateTime();
                                                        lisansdurum.MKKEnd = mkkEd.StrinToDateTime();
                                                    }
                                                    else
                                                    {

                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "MKK start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.MKK = user.LisansDurum.MKK;
                                                    lisansdurum.MKKStart = user.LisansDurum.MKKStart;
                                                    lisansdurum.MKKEnd = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.MKK = user.LisansDurum.MKK;
                                                lisansdurum.MKKStart = user.LisansDurum.MKKStart;
                                                lisansdurum.MKKEnd = user.LisansDurum.MKKEnd;
                                            }
                                            #endregion
                                            #region GKKUL
                                            if (gkkulGeldi == true)
                                            {
                                                if (gkkul == true)
                                                {
                                                    if (gkkulSd != "" && gkkulEd != "")
                                                    {
                                                        if (gkkul && BuAyGelecekAyKontrolu(gkkulSd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.GKKUL = true;
                                                            if (user.LisansDurum.GKKUL == false)
                                                                acilanlar.Append("GKKUL");
                                                        }
                                                        lisansdurum.GKKULStart = gkkulSd.StrinToDateTime();
                                                        lisansdurum.GKKULEnd = gkkulEd.StrinToDateTime();
                                                    }
                                                    else
                                                    {

                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "GKKUL start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.GKKUL = user.LisansDurum.GKKUL;
                                                    lisansdurum.GKKULStart = user.LisansDurum.GKKULStart;
                                                    lisansdurum.GKKULEnd = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.GKKUL = user.LisansDurum.GKKUL;
                                                lisansdurum.GKKULStart = user.LisansDurum.GKKULStart;
                                                lisansdurum.GKKULEnd = user.LisansDurum.GKKULEnd;
                                            }
                                            #endregion
                                            #region TARAMA
                                            if (taramaGeldi == true)
                                            {
                                                if (tarama == true)
                                                {
                                                    if (taramaSd != "" && taramaEd != "")
                                                    {
                                                        if (tarama && BuAyGelecekAyKontrolu(taramaSd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.TARAMA = true;
                                                            if (user.LisansDurum.TARAMA == false)
                                                                acilanlar.Append("TARAMA");
                                                        }
                                                        lisansdurum.TaramaStart = taramaSd.StrinToDateTime();
                                                        lisansdurum.TaramaEnd = taramaEd.StrinToDateTime();
                                                    }
                                                    else
                                                    {

                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "TARAMA start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.TARAMA = user.LisansDurum.TARAMA;
                                                    lisansdurum.TaramaStart = user.LisansDurum.TaramaStart;
                                                    lisansdurum.TaramaEnd = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.TARAMA = user.LisansDurum.TARAMA;
                                                lisansdurum.TaramaStart = user.LisansDurum.TaramaStart;
                                                lisansdurum.TaramaEnd = user.LisansDurum.TaramaEnd;
                                            }
                                            #endregion

                                            #region CME
                                            if (cmeGeldi == true)
                                            {
                                                if (cme == true)
                                                {
                                                    if (cmeSd != "" && cmeEd != "")
                                                    {
                                                        if (cme && BuAyGelecekAyKontrolu(cmeSd.StrinToDateTime()))
                                                        {
                                                            lisansdurum.CME = true;
                                                            if (user.LisansDurum.CME == false)
                                                                acilanlar.Append("CME");
                                                        }
                                                        lisansdurum.CMEStart = cmeSd.StrinToDateTime();
                                                        lisansdurum.CMEEnd = cmeEd.StrinToDateTime();
                                                    }
                                                    else
                                                    {

                                                        sonuc.Kod = "Hatalı/Eksik Parametre.";
                                                        sonuc.Aciklama = "CME start date/end date bilgisi eksik/hatalı.";
                                                        sonuc.Status = "ERROR";

                                                        var json = new JavaScriptSerializer().Serialize(sonuc);

                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    lisansdurum.CME = user.LisansDurum.CME;
                                                    lisansdurum.CMEStart = user.LisansDurum.CMEStart;
                                                    lisansdurum.CMEEnd = DateTime.Now._LastDayOfMonth();
                                                }
                                            }
                                            else
                                            {
                                                lisansdurum.CME = user.LisansDurum.CME;
                                                lisansdurum.CMEStart = user.LisansDurum.CMEStart;
                                                lisansdurum.CMEEnd = user.LisansDurum.CMEEnd;
                                            }
                                            #endregion


                                            if (acilanlar.ToString() != "")
                                                userevent.EventTypeId = 4;
                                            else
                                                userevent.EventTypeId = 1;

                                            userevent.AcilanLisans = acilanlar.ToString();
                                            crm.LisansDurums.InsertOnSubmit(lisansdurum);
                                            crm.SubmitChanges();
                                            user.LisansDurum = lisansdurum;
                                            crm.SubmitChanges();
                                            userevent.SonLisandurumID = lisansdurum.LisansDurumId;
                                            userevent.UserId = user.UserID;
                                            crm.UserEvents.InsertOnSubmit(userevent);

                                            sonuc.Kod = "Lisans Guncelleme";
                                            sonuc.Aciklama = "Lisans başarıyla güncellenmiştir.";
                                            sonuc.Status = "OK";
                                            var text = "ATAPI  Update Lisance  ;  user name  = " + user.UserName + " ; Remeto Ip =" + userevent.IP;
                                            formListeTransaction(text);
                                            CalisanlaraKomutGonder("Ata_UpdateLisance|" + text);

                                            SendToSSO(user.UserID);
                                            var jsnn = new JavaScriptSerializer().Serialize(sonuc);

                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(jsnn._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }
                                        else
                                        {
                                            for (int i = 2; i < fieldarray1.Length; i++)
                                            {
                                                var splitarray = fieldarray1[i].Split('=');
                                                switch (splitarray[0].Trim()._ToEngUp())
                                                {
                                                    case "HESAPNO": username = splitarray[1].Trim(); break;
                                                }
                                            }
                                            StringBuilder bosAlanlar = new StringBuilder();
                                            BosAlanlar bos = new BosAlanlar();
                                            userevent.CalisanId = 2;
                                            userevent.EventTypeId = 1;
                                            user = new User();

                                            if (user.Name == "" || user.Name == null)
                                            {
                                                bosAlanlar.Append("AD:0,");
                                                bos.Ad = "0";
                                            }
                                            else
                                            {
                                                bosAlanlar.Append("AD:1,");
                                                bos.Ad = "1";
                                            }
                                            if (user.Surname == "" || user.Name == null)
                                            {
                                                bosAlanlar.Append("SOYAD:0,");
                                                bos.Soyad = "0";
                                            }
                                            else
                                            {
                                                bosAlanlar.Append("SOYAD:1,");
                                                bos.Soyad = "1";
                                            }

                                            if (user.Iletisim == null)
                                            {
                                                bosAlanlar.Append("Adres:0,GSM:0,Email:0,Ulke:0,Sehir:0 ");
                                                bos.Il = "0";
                                                bos.Ulke = "0";
                                                bos.GSM = "0";
                                                bos.Email = "0";
                                                bos.Adres = "0";
                                            }
                                            else
                                            {
                                                bosAlanlar.Append("Adres:1,GSM:1,Email:1,Ulke:1,Sehir:1 ");

                                                bos.Il = "1";
                                                bos.Ulke = "1";
                                                bos.GSM = "1";
                                                bos.Email = "1";
                                                bos.Adres = "1";
                                            }
                                            user.UserName = username;
                                            user.Password = MyTools.Sifreleme.Encryp("102030");
                                            user.Aciklama = username;
                                            user.BaslangicTarihi = DateTime.Now;
                                            user.PmtsNo = "10158";

                                            var LisansDurum = new LisansDurum();
                                            var expriydate = new DateTime(2030, 12, 31);

                                            LisansDurum.Futgck = true;
                                            LisansDurum.WINX = true;
                                            //LisansDurum.KRMD1 = true;
                                            //LisansDurum.KRMD1Start = DateTime.Now.Date;
                                            //LisansDurum.KRMD1End = expriydate;
                                            user.ProductType = "IDEAL";
                                            user.BaslangicTarihi = DateTime.Now;
                                            user.ExpiryDate = expriydate;
                                            LisansDurum.YayinDurumu = true;
                                            user.StatusId = 1;

                                            #region Gelen_Lisanlari_isle
                                            LisansDurum.CepYetki = true;
                                            #endregion

                                            crm.LisansDurums.InsertOnSubmit(LisansDurum);
                                            crm.SubmitChanges();

                                            user.LisansDurumId = LisansDurum.LisansDurumId;
                                            userevent.SonLisandurumID = LisansDurum.LisansDurumId;

                                            crm.Users.InsertOnSubmit(user);
                                            crm.SubmitChanges();

                                            userevent.UserId = user.UserID;

                                            crm.UserEvents.InsertOnSubmit(userevent);
                                            crm.SubmitChanges();

                                            responsestr = "OK";

                                            var text = "ATAPI  CreateCustomer  ;açilan user name  = " + user.UserName + ";  Remeto Ip =" + userevent.IP;
                                            formListeTransaction(text);
                                            CalisanlaraKomutGonder("Ata_CreateUser|" + text);
                                            SendToSSO(user.UserID);

                                            sonuc.Kod = "Kullanıcı Oluşturuldu.";
                                            sonuc.Aciklama = bosAlanlar.ToString();
                                            sonuc.Status = "OK";

                                            var jsnn = new JavaScriptSerializer().Serialize(sonuc);

                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(jsnn._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                            //responsestr = "Kullanıcı Oluşturuldu.Kullanıcının " + bosAlanlar;
                                        }
                                    }
                                    #endregion
                                }
                                var fieldarray = message.Split('?');

                                if (fieldarray[0] != "DFN")
                                {
                                    responsestr = "BULUNAMADI";
                                    formListeTransaction(responsestr);
                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                    return;
                                }

                                if (fieldarray[1] == "PW8190")  // estore
                                {
                                    #region estore
                                    switch (fieldarray[2])
                                    {
                                        case "EstoreCreateClient":
                                            {
                                                userevent.CalisanId = 6;


                                                string username = fieldarray[3];
                                                if (username == null || username == "")
                                                    return;
                                                if (crm.Users.Where(x => x.UserName == username).Any())
                                                {
                                                    #region güncelle

                                                    var kaptianlar = new StringBuilder();
                                                    var acilanlar = new StringBuilder();
                                                    LisansDurum lisanslar = new LisansDurum();



                                                    var newcustomer = crm.Users.FirstOrDefault(x => x.UserName == username);

                                                    if (newcustomer.LisansDurumId != null)
                                                    {
                                                        if (newcustomer.LisansDurum.ROBOT == true)
                                                        {
                                                            lisanslar.ROBOT = true;
                                                        }

                                                    }
                                                    for (int i = 3; i < fieldarray.Length; i++)
                                                    {
                                                        var splitarray = fieldarray[i].Split('=');
                                                        switch (splitarray[0]._ToEngUp())
                                                        {
                                                            case "IDEALSIFRE": newcustomer.Password = MyTools.Sifreleme.Encryp(splitarray[1].Trim()); break;


                                                            case "EXPIREDATE":

                                                                if (splitarray[1].Length < 7)
                                                                    break;
                                                                var newDate = DateTime.ParseExact(splitarray[1], "yyyyMMdd", CultureInfo.InvariantCulture);

                                                                newcustomer.ExpiryDate = newDate; break;
                                                            #region lisanslar




                                                            case "PRO": lisanslar.ProYetki = ((splitarray[1]._ToInt()) == 1) ? true : false; break;
                                                            case "CEP": lisanslar.CepYetki = ((splitarray[1]._ToInt()) == 1) ? true : false; break;
                                                            case "IMKBL1": lisanslar.PayL1 = splitarray[1]._ToBool(); break;
                                                            case "IMKBL1P": lisanslar.PayLP = splitarray[1]._ToBool(); break;
                                                            case "IMKBL2": lisanslar.PayL2 = splitarray[1]._ToBool(); break;
                                                            case "IMKBL2P": lisanslar.Pd2P = splitarray[1]._ToBool(); break;
                                                            case "IMKBISL": lisanslar.PayGS = splitarray[1]._ToBool(); break;
                                                            case "IMKBX": lisanslar.PayX = splitarray[1]._ToBool(); break;
                                                            case "IMKBANL": lisanslar.VeriAnalitik = splitarray[1]._ToBool(); break;
                                                            case "VIPL1": lisanslar.ViopL1 = splitarray[1]._ToBool(); break;
                                                            case "VIPL1P": lisanslar.ViopLP = splitarray[1]._ToBool(); break;
                                                            case "VIPL2": lisanslar.ViopL2 = splitarray[1]._ToBool(); break;
                                                            case "VIPL2P": lisanslar.Vd2P = splitarray[1]._ToBool(); break;
                                                            case "VIPNET": lisanslar.ViopGS = splitarray[1]._ToBool(); break;
                                                            case "THVL1": lisanslar.TahvilL1 = splitarray[1]._ToBool(); break;
                                                            case "THVL1P": lisanslar.TahvilLP = splitarray[1]._ToBool(); break;
                                                            case "THVL2": lisanslar.TahvilL2 = splitarray[1]._ToBool(); break;
                                                            case "ANALİZPRO": lisanslar.AnPro = splitarray[1]._ToBool(); break;
                                                            case "DJI": lisanslar.DJI = splitarray[1]._ToBool(); break;
                                                            case "SPI": lisanslar.SPI = splitarray[1]._ToBool(); break;
                                                            case "XETRA": lisanslar.XETRA = splitarray[1]._ToBool(); break;
                                                            case "CBOTM": lisanslar.CBOTM = splitarray[1]._ToBool(); break;
                                                            case "CBOT": lisanslar.CBOT = splitarray[1]._ToBool(); break;
                                                            case "CMEM": lisanslar.CMEM = splitarray[1]._ToBool(); break;
                                                            case "CME": lisanslar.CME = splitarray[1]._ToBool(); break;
                                                            case "EUREX": lisanslar.EUREX = splitarray[1]._ToBool(); break;
                                                            case "NYMEX": lisanslar.NYMEX = splitarray[1]._ToBool(); break;
                                                            case "NYMEXM": lisanslar.NYMEXM = splitarray[1]._ToBool(); break;
                                                            case "COMEX": lisanslar.COMEX = splitarray[1]._ToBool(); break;

                                                                #endregion

                                                        }



                                                    }

                                                    lisanslar.Futgck = true;
                                                    lisanslar.WINX = true;
                                                    lisanslar.YayinDurumu = true;
                                                    crm.LisansDurums.InsertOnSubmit(lisanslar);
                                                    crm.SubmitChanges();
                                                    newcustomer.LisansDurum = lisanslar;
                                                    crm.SubmitChanges();
                                                    userevent.SonLisandurumID = newcustomer.LisansDurumId;


                                                    userevent.EventTypeId = 5;
                                                    userevent.SonLisandurumID = newcustomer.LisansDurumId;



                                                    userevent.UserId = newcustomer.UserID;


                                                    crm.UserEvents.InsertOnSubmit(userevent);
                                                    crm.SubmitChanges();


                                                    var text = "EStore Kullanıcı Değişme  ;  user name  = " + newcustomer.UserName + " ; Remeto Ip =" + userevent.IP;
                                                    formListeTransaction(text);
                                                    CalisanlaraKomutGonder("Estrore_ChangeUser|" + text);
                                                    SendToSSO(newcustomer.UserID);

                                                    responsestr = "OK";
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;


                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region YeniKayit

                                                    userevent.EventTypeId = 5;
                                                    var newcustomer = new User();
                                                    var lisansd = new LisansDurum();
                                                    var iletisim = new Iletisim();
                                                    for (int i = 3; i < fieldarray.Length; i++)
                                                    {
                                                        var splitarray = fieldarray[i].Split('=');
                                                        switch (splitarray[0]._ToEngUp())
                                                        {
                                                            case "IDEALSIFRE": newcustomer.Password = MyTools.Sifreleme.Encryp(splitarray[1].Trim()); break;
                                                            case "ACIKLAMA":
                                                                newcustomer.Aciklama = splitarray[1]; break;
                                                            case "ISIM":

                                                                var gelenisim = splitarray[1].Trim().Split(' ');
                                                                var ad = "";
                                                                var soyad = "";
                                                                if (gelenisim.Length == 1)
                                                                {
                                                                    ad = gelenisim[0];
                                                                    soyad = "yok";

                                                                }
                                                                else if (gelenisim.Length == 2)
                                                                {
                                                                    ad = gelenisim[0];
                                                                    soyad = gelenisim[1];

                                                                }
                                                                else if (gelenisim.Length == 3)
                                                                {
                                                                    ad = gelenisim[0] + " " + gelenisim[1];
                                                                    soyad = gelenisim[2];
                                                                }

                                                                newcustomer.Name = ad;
                                                                newcustomer.Surname = soyad;
                                                                break;
                                                            case "SEHIR":
                                                                var il = MyTools.SehirIdBul(splitarray[1]);
                                                                if (il != null && il.Id > 0)
                                                                {
                                                                    iletisim.IlId = il.Id;
                                                                    iletisim.UlkeId = il.UlkeId;
                                                                }
                                                                break;
                                                            //  case "NOT1": newcustomer.Not1 = splitarray[1]; break;

                                                            case "EXPIREDATE":

                                                                if (splitarray[1].Length < 7)
                                                                    break;
                                                                var newDate = DateTime.ParseExact(splitarray[1], "yyyyMMdd", CultureInfo.InvariantCulture);

                                                                newcustomer.ExpiryDate = newDate; break;
                                                            case "PRO":
                                                                lisansd.ProYetki = ((splitarray[1]._ToInt()) == 1) ? true : false; break;
                                                            case "CEP": lisansd.CepYetki = ((splitarray[1]._ToInt()) == 1) ? true : false; break;
                                                            case "IMKBL1": lisansd.PayL1 = splitarray[1]._ToBool(); break;
                                                            case "IMKBL1P": lisansd.PayLP = splitarray[1]._ToBool(); break;
                                                            case "IMKBL2": lisansd.PayL2 = splitarray[1]._ToBool(); break;
                                                            case "IMKBL2P": lisansd.Pd2P = splitarray[1]._ToBool(); break;
                                                            case "IMKBISL": lisansd.PayGS = splitarray[1]._ToBool(); break;
                                                            case "IMKBX": lisansd.PayX = splitarray[1]._ToBool(); break;
                                                            case "IMKBANL": lisansd.VeriAnalitik = splitarray[1]._ToBool(); break;
                                                            case "VIPL1": lisansd.ViopL1 = splitarray[1]._ToBool(); break;
                                                            case "VIPL1P": lisansd.ViopLP = splitarray[1]._ToBool(); break;
                                                            case "VIPL2": lisansd.ViopL2 = splitarray[1]._ToBool(); break;
                                                            case "Vd2P": lisansd.Vd2P = splitarray[1]._ToBool(); break;
                                                            case "VIPNET": lisansd.ViopGS = splitarray[1]._ToBool(); break;
                                                            case "THVL1P": lisansd.TahvilLP = splitarray[1]._ToBool(); break;
                                                            case "THVL2": lisansd.TahvilL2 = splitarray[1]._ToBool(); break;
                                                            case "ANALİZPRO": lisansd.AnPro = splitarray[1]._ToBool(); break;
                                                            case "DJI": lisansd.DJI = splitarray[1]._ToBool(); break;
                                                            case "SPI": lisansd.SPI = splitarray[1]._ToBool(); break;
                                                            case "XETRA": lisansd.XETRA = splitarray[1]._ToBool(); break;
                                                            case "CBOTM": lisansd.CBOTM = splitarray[1]._ToBool(); break;
                                                            case "CBOT": lisansd.CBOT = splitarray[1]._ToBool(); break;
                                                            case "CMEM": lisansd.CMEM = splitarray[1]._ToBool(); break;
                                                            case "CME": lisansd.CME = splitarray[1]._ToBool(); break;
                                                            case "EUREX": lisansd.EUREX = splitarray[1]._ToBool(); break;
                                                            case "NYMEX": lisansd.NYMEX = splitarray[1]._ToBool(); break;
                                                            case "NYMEXM": lisansd.NYMEXM = splitarray[1]._ToBool(); break;
                                                            case "COMEX": lisansd.COMEX = splitarray[1]._ToBool(); break;
                                                            case "MAIL": iletisim.email = splitarray[1]; break;
                                                            case "ADRES": iletisim.acikadres = splitarray[1]; break;
                                                            case "TELEFON": iletisim.Tel1 = splitarray[1]; break;
                                                            case "TCKIMLIK": newcustomer.tckno = splitarray[1]; break;
                                                        }
                                                    }
                                                    newcustomer.MusteriMenseiID = 1;
                                                    newcustomer.UserName = username;

                                                    lisansd.Futgck = true;
                                                    lisansd.WINX = true;
                                                    newcustomer.ProductType = "IDEAL";
                                                    newcustomer.BaslangicTarihi = DateTime.Now;
                                                    lisansd.YayinDurumu = true;
                                                    newcustomer.StatusId = 1;
                                                    newcustomer.PmtsNo = "44444";

                                                    crm.LisansDurums.InsertOnSubmit(lisansd);
                                                    crm.SubmitChanges();

                                                    newcustomer.LisansDurumId = lisansd.LisansDurumId;
                                                    userevent.SonLisandurumID = lisansd.LisansDurumId;

                                                    crm.Iletisims.InsertOnSubmit(iletisim);
                                                    crm.SubmitChanges();
                                                    newcustomer.iletisimId = iletisim.IletisimId;

                                                    crm.Users.InsertOnSubmit(newcustomer);
                                                    crm.SubmitChanges();

                                                    userevent.UserId = newcustomer.UserID;

                                                    crm.UserEvents.InsertOnSubmit(userevent);
                                                    crm.SubmitChanges();

                                                    responsestr = "OK";

                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;

                                                    var text = "EStore Yeni Kullanıcı Açma  ;açilan user name  = " + newcustomer.UserName + ";  Remeto Ip =" + userevent.IP;
                                                    formListeTransaction(text);
                                                    CalisanlaraKomutGonder("Estrore_CreateUser|" + text);
                                                    SendToSSO(newcustomer.UserID);

                                                    #endregion
                                                    return;
                                                }
                                            }
                                    }
                                    #endregion
                                }
                                else  // oyak
                                {
                                    #region osmanli
                                    string dummy = MyTools.Split(ref fieldarray[1], '=');
                                    string kurum = fieldarray[1]._ToEngUp();
                                    dummy = MyTools.Split(ref fieldarray[2], '=');
                                    string kurumpassword = fieldarray[2]._ToEngUp();
                                    dummy = MyTools.Split(ref fieldarray[3], '=');
                                    string komut = fieldarray[3]._ToEngUp();

                                    if (kurum == "OYAK" && kurumpassword == "LEVENT2014")
                                    {

                                        User newcustomer = new User();
                                        LisansDurum lisanslar = new LisansDurum();
                                        KurumsalBilgiler kurumsalbilgi = new KurumsalBilgiler();
                                        UserExtraInfo extrainfo = new UserExtraInfo();

                                        var iletisim = new Iletisim();

                                        // OYAKMUSTERITANIMLA
                                        if (komut == "OYAKMUSTERITANIMLA")
                                        {
                                            string hesapno = "";
                                            string idealsifre = "";
                                            string idealusername = "";
                                            string aciklama = "";
                                            string sube = "";
                                            string kullanicitip = "";
                                            string isim = "";
                                            string sehir = "";
                                            string kurummusterino = "";
                                            string not1 = "";
                                            string not2 = "";
                                            string not3 = "";
                                            string expiredate = "";
                                            string mail = "";
                                            string adres = "";
                                            string telefon = "";
                                            string PRO = "";
                                            string CEP = "";
                                            string IMKBX = "";
                                            string IMKBANL = "";
                                            string IMKBL1 = "";
                                            string IMKBL1P = "";
                                            string IMKBL2 = "";
                                            string IMKBL2P = "";
                                            string IMKBISL = "";
                                            string VIPL1 = "";
                                            string VIPL1P = "";
                                            string VIPL2 = "";
                                            string VIPL2P = "";
                                            string VIPNET = "";
                                            string THVL1 = "";
                                            string THVL1P = "";
                                            string THVL2 = "";
                                            string DJI = "";
                                            string SPI = "";
                                            string XETRA = "";
                                            string CBOTM = "";
                                            string CBOT = "";
                                            string CMEM = "";
                                            string CME = "";
                                            string EUREX = "";
                                            string NYMEX = "";
                                            string NYMEXM = "";
                                            string COMEX = "";
                                            for (int i = 4; i < fieldarray.Length; i++)
                                            {
                                                var splitarray = fieldarray[i].Split('=');
                                                switch (splitarray[0]._ToEngUp())
                                                {
                                                    case "HESAPNO": hesapno = splitarray[1]; break;
                                                    case "IDEALSIFRE": idealsifre = splitarray[1]; break;
                                                    case "ACIKLAMA": aciklama = splitarray[1]; break;
                                                    case "SUBE": sube = splitarray[1]; break;
                                                    case "KULLANICITIP": kullanicitip = splitarray[1]; break;
                                                    case "ISIM": isim = splitarray[1]; break;
                                                    case "SEHIR": sehir = splitarray[1]; break;
                                                    case "KURUMMUSTERINO": kurummusterino = splitarray[1]; break;
                                                    case "NOT1": not1 = splitarray[1]; break;
                                                    case "NOT2": not2 = splitarray[1]; break;
                                                    case "NOT3": not3 = splitarray[1]; break;
                                                    case "EXPIREDATE": expiredate = splitarray[1]; break;
                                                    case "PRO": PRO = splitarray[1]; break;
                                                    case "CEP": CEP = splitarray[1]; break;
                                                    case "IMKBX": IMKBX = splitarray[1]; break;
                                                    case "IMKBANL": IMKBANL = splitarray[1]; break;
                                                    case "IMKBL1": IMKBL1 = splitarray[1]; break;
                                                    case "IMKBL1P": IMKBL1P = splitarray[1]; break;
                                                    case "IMKBL2": IMKBL2 = splitarray[1]; break;
                                                    case "IMKBL2P": IMKBL2P = splitarray[1]; break;
                                                    case "IMKBISL": IMKBISL = splitarray[1]; break;
                                                    case "VIPL1": VIPL1 = splitarray[1]; break;
                                                    case "VIPL1P": VIPL1P = splitarray[1]; break;
                                                    case "VIPL2": VIPL2 = splitarray[1]; break;
                                                    case "VIPL2P": VIPL2P = splitarray[1]; break;
                                                    case "VIPNET": VIPNET = splitarray[1]; break;
                                                    case "THVL1": THVL1 = splitarray[1]; break;
                                                    case "THVL1P": THVL1P = splitarray[1]; break;
                                                    case "THVL2": THVL2 = splitarray[1]; break;
                                                    case "DJI": DJI = splitarray[1]; break;
                                                    case "SPI": SPI = splitarray[1]; break;
                                                    case "XETRA": XETRA = splitarray[1]; break;
                                                    case "CBOTM": CBOTM = splitarray[1]; break;
                                                    case "CBOT": CBOT = splitarray[1]; break;
                                                    case "CMEM": CMEM = splitarray[1]; break;
                                                    case "CME": CME = splitarray[1]; break;
                                                    case "EUREX": EUREX = splitarray[1]; break;
                                                    case "NYMEX": NYMEX = splitarray[1]; break;
                                                    case "NYMEXM": NYMEXM = splitarray[1]; break;
                                                    case "COMEX": COMEX = splitarray[1]; break;
                                                    case "MAIL": mail = splitarray[1]; break;
                                                    case "ADRES": adres = splitarray[1]; break;
                                                    case "TELEFON": telefon = splitarray[1]; break;
                                                }
                                            }
                                            if (hesapno == "")
                                            {
                                                responsestr = "HATA : Hesap No Boş olmamalıdır";
                                                IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                                IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                return;
                                            }
                                            idealusername = "oyak" + hesapno;
                                            var t = DateTime.Now;
                                            var expireDate = new DateTime(t.Year, t.Month, 1).AddMonths(1).AddDays(-1);



                                            if (crm.Users.Where(x => x.UserName == idealusername).Any())
                                            {
                                                #region Guncele


                                                var user = crm.Users.FirstOrDefault(x => x.UserName == idealusername);

                                                if (user.LisansDurum.YayinDurumu == false)
                                                {
                                                    #region userKapali




                                                    user.ExpiryDate = expireDate;

                                                    user.PmtsNo = "10226";
                                                    user.ProductType = "IDEAL";
                                                    if (user.MusteriMenseiID == null)
                                                        user.MusteriMenseiID = 1;


                                                    if (idealsifre != "") user.Password = MyTools.Sifreleme.Encryp(idealsifre);
                                                    if (aciklama != "") user.Aciklama = aciklama;
                                                    #region isimcozumle
                                                    if (isim != "")
                                                    {
                                                        var gelenisim = isim.Trim().Split(' ');
                                                        var ad = "";
                                                        var soyad = "";
                                                        if (gelenisim.Length == 1)
                                                        {
                                                            ad = gelenisim[0];
                                                            soyad = "yok";

                                                        }
                                                        else if (gelenisim.Length == 2)
                                                        {
                                                            ad = gelenisim[0];
                                                            soyad = gelenisim[1];

                                                        }
                                                        else if (gelenisim.Length == 3)
                                                        {
                                                            ad = gelenisim[0] + " " + gelenisim[1];
                                                            soyad = gelenisim[2];
                                                        }


                                                        user.Name = ad;
                                                        user.Surname = soyad;
                                                    }

                                                    #endregion

                                                    #region kurumsalbilgi 


                                                    if (user.KurumsalBilgilerId != null)
                                                    {

                                                        if (sube != "") user.KurumsalBilgiler.KurumSube = sube;
                                                        if (kullanicitip != "") user.KurumsalBilgiler.KurumKullaniciTip = kullanicitip;
                                                        if (kurummusterino != "") user.KurumsalBilgiler.Not1 = kurummusterino;
                                                        if (hesapno != "") user.KurumsalBilgiler.kurumhesapno = hesapno;


                                                    }
                                                    else
                                                    {
                                                        if (sube != "") kurumsalbilgi.KurumSube = sube;
                                                        if (kullanicitip != "") kurumsalbilgi.KurumKullaniciTip = kullanicitip;
                                                        if (kurummusterino != "") kurumsalbilgi.Not1 = kurummusterino;
                                                        if (hesapno != "") kurumsalbilgi.kurumhesapno = hesapno;

                                                        crm.KurumsalBilgilers.InsertOnSubmit(kurumsalbilgi);
                                                        crm.SubmitChanges();
                                                        user.KurumsalBilgilerId = kurumsalbilgi.Id;
                                                    }





                                                    #endregion

                                                    #region iletisimbilgileri 



                                                    if (user.iletisimId != null)
                                                    {
                                                        if (mail != "") user.Iletisim.email = mail;
                                                        if (adres != "") user.Iletisim.acikadres = adres;
                                                        if (telefon != "") user.Iletisim.Tel1 = telefon;

                                                        if (sehir != "")
                                                        {
                                                            var il = MyTools.SehirIdBul(sehir);
                                                            if (il != null && il.Id > 0)
                                                            {
                                                                user.Iletisim.IlId = il.Id;
                                                                user.Iletisim.UlkeId = il.UlkeId;
                                                            }

                                                        }

                                                    }
                                                    else
                                                    {
                                                        if (mail != "") iletisim.email = mail;
                                                        if (adres != "") iletisim.acikadres = adres;
                                                        if (telefon != "") iletisim.Tel1 = telefon;

                                                        if (sehir != "")
                                                        {
                                                            var il = MyTools.SehirIdBul(sehir);
                                                            if (il != null && il.Id > 0)
                                                            {
                                                                iletisim.IlId = il.Id;
                                                                iletisim.UlkeId = il.UlkeId;
                                                            }

                                                        }



                                                        crm.Iletisims.InsertOnSubmit(iletisim);
                                                        crm.SubmitChanges();
                                                        user.iletisimId = iletisim.IletisimId;

                                                    }

                                                    #endregion

                                                    #region ExtaInfo

                                                    if (user.UserExtraInfoId != null)
                                                    {
                                                        if (not1 != "") user.UserExtraInfo.Not1 = not1;
                                                        if (not2 != "") user.UserExtraInfo.Not2 = not2;
                                                        if (not3 != "") user.UserExtraInfo.Not3 = not3;

                                                    }
                                                    else
                                                    {
                                                        if (not1 != "") extrainfo.Not1 = not1;
                                                        if (not2 != "") extrainfo.Not2 = not2;
                                                        if (not3 != "") extrainfo.Not3 = not3;


                                                        crm.UserExtraInfos.InsertOnSubmit(extrainfo);
                                                        crm.SubmitChanges();

                                                        user.UserExtraInfoId = extrainfo.id;

                                                    }



                                                    #endregion


                                                    #region Lisanlar



                                                    lisanslar.YayinDurumu = true;
                                                    lisanslar.Futgck = true;
                                                    lisanslar.WINX = true;


                                                    if (PRO == "1")
                                                        lisanslar.ProYetki = true;
                                                    if (CEP == "1")
                                                        lisanslar.CepYetki = true;



                                                    #region BistPay

                                                    if (IMKBX == "1")
                                                        lisanslar.PayX = true;
                                                    if (IMKBANL == "1")
                                                        lisanslar.VeriAnalitik = true;

                                                    if (IMKBL1 == "1")
                                                        lisanslar.PayL1 = true;
                                                    if (IMKBL1P == "1")
                                                        lisanslar.PayLP = true;
                                                    if (IMKBL2 == "1")
                                                        lisanslar.PayL2 = true;
                                                    if (IMKBL2P == "1")
                                                        lisanslar.Pd2P = true;
                                                    if (IMKBISL == "1")
                                                        lisanslar.PayGS = true;




                                                    #endregion
                                                    #region VIP

                                                    if (VIPL1 == "1")
                                                        lisanslar.ViopL1 = true;
                                                    if (VIPL1P == "1")
                                                        lisanslar.ViopLP = true;
                                                    if (VIPL2 == "1")
                                                        lisanslar.ViopL2 = true;
                                                    if (VIPL2P == "1")
                                                        lisanslar.Vd2P = true;
                                                    if (VIPNET == "1")
                                                        lisanslar.ViopGS = true;

                                                    #endregion
                                                    #region Tahvil

                                                    if (THVL1 == "1")
                                                        lisanslar.TahvilL1 = true;
                                                    if (THVL1P == "1")
                                                        lisanslar.TahvilLP = true;
                                                    if (THVL2 == "1")
                                                        lisanslar.TahvilL2 = true;

                                                    #endregion
                                                    #region YurtDisi

                                                    if (DJI == "1")
                                                        lisanslar.DJI = true;
                                                    if (SPI == "1")
                                                        lisanslar.SPI = true;
                                                    if (XETRA == "1")
                                                        lisanslar.XETRA = true;
                                                    if (CBOTM == "1")
                                                        lisanslar.CBOTM = true;
                                                    if (CBOT == "1")
                                                        lisanslar.CBOT = true;
                                                    if (CMEM == "1")
                                                        lisanslar.CMEM = true;
                                                    if (CME == "1")
                                                        lisanslar.CME = true;
                                                    if (EUREX == "1")
                                                        lisanslar.EUREX = true;
                                                    if (NYMEX == "1")
                                                        lisanslar.NYMEX = true;
                                                    if (NYMEXM == "1")
                                                        lisanslar.NYMEXM = true;
                                                    if (COMEX == "1")
                                                        lisanslar.COMEX = true;


                                                    #endregion

                                                    crm.LisansDurums.InsertOnSubmit(lisanslar);
                                                    crm.SubmitChanges();
                                                    userevent.SonLisandurumID = lisanslar.LisansDurumId;
                                                    user.LisansDurum = lisanslar;
                                                    crm.SubmitChanges();
                                                    userevent.CalisanId = 7;
                                                    userevent.EventTypeId = 1;
                                                    userevent.UserId = user.UserID;
                                                    crm.UserEvents.InsertOnSubmit(userevent);
                                                    crm.SubmitChanges();

                                                    #endregion


                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region userAcik

                                                    var kaptianlar = new StringBuilder();
                                                    var acilanlar = new StringBuilder();

                                                    user.ExpiryDate = expireDate;

                                                    user.PmtsNo = "10226";
                                                    user.ProductType = "IDEAL";
                                                    if (user.MusteriMenseiID == null)
                                                        user.MusteriMenseiID = 1;


                                                    if (idealsifre != "") user.Password = MyTools.Sifreleme.Encryp(idealsifre);
                                                    if (aciklama != "") user.Aciklama = aciklama;
                                                    #region isimcozumle
                                                    if (isim != "")
                                                    {
                                                        var gelenisim = isim.Trim().Split(' ');
                                                        var ad = "";
                                                        var soyad = "";
                                                        if (gelenisim.Length == 1)
                                                        {
                                                            ad = gelenisim[0];
                                                            soyad = "yok";

                                                        }
                                                        else if (gelenisim.Length == 2)
                                                        {
                                                            ad = gelenisim[0];
                                                            soyad = gelenisim[1];

                                                        }
                                                        else if (gelenisim.Length == 3)
                                                        {
                                                            ad = gelenisim[0] + " " + gelenisim[1];
                                                            soyad = gelenisim[2];
                                                        }


                                                        user.Name = ad;
                                                        user.Surname = soyad;
                                                    }

                                                    #endregion

                                                    #region kurumsalbilgi 


                                                    if (user.KurumsalBilgilerId != null)
                                                    {

                                                        if (sube != "") user.KurumsalBilgiler.KurumSube = sube;
                                                        if (kullanicitip != "") user.KurumsalBilgiler.KurumKullaniciTip = kullanicitip;
                                                        if (kurummusterino != "") user.KurumsalBilgiler.Not1 = kurummusterino;
                                                        if (hesapno != "") user.KurumsalBilgiler.kurumhesapno = hesapno;


                                                    }
                                                    else
                                                    {
                                                        if (sube != "") kurumsalbilgi.KurumSube = sube;
                                                        if (kullanicitip != "") kurumsalbilgi.KurumKullaniciTip = kullanicitip;
                                                        if (kurummusterino != "") kurumsalbilgi.Not1 = kurummusterino;
                                                        if (hesapno != "") kurumsalbilgi.kurumhesapno = hesapno;

                                                        crm.KurumsalBilgilers.InsertOnSubmit(kurumsalbilgi);
                                                        crm.SubmitChanges();
                                                        user.KurumsalBilgilerId = kurumsalbilgi.Id;
                                                    }





                                                    #endregion

                                                    #region iletisimbilgileri 



                                                    if (user.iletisimId != null)
                                                    {
                                                        if (mail != "") user.Iletisim.email = mail;
                                                        if (adres != "") user.Iletisim.acikadres = adres;
                                                        if (telefon != "") user.Iletisim.Tel1 = telefon;

                                                        if (sehir != "")
                                                        {
                                                            var il = MyTools.SehirIdBul(sehir);
                                                            if (il != null && il.Id > 0)
                                                            {
                                                                user.Iletisim.IlId = il.Id;
                                                                user.Iletisim.UlkeId = il.UlkeId;
                                                            }

                                                        }

                                                    }
                                                    else
                                                    {
                                                        if (mail != "") iletisim.email = mail;
                                                        if (adres != "") iletisim.acikadres = adres;
                                                        if (telefon != "") iletisim.Tel1 = telefon;

                                                        if (sehir != "")
                                                        {
                                                            var il = MyTools.SehirIdBul(sehir);
                                                            if (il != null && il.Id > 0)
                                                            {
                                                                iletisim.IlId = il.Id;
                                                                iletisim.UlkeId = il.UlkeId;
                                                            }

                                                        }



                                                        crm.Iletisims.InsertOnSubmit(iletisim);
                                                        crm.SubmitChanges();
                                                        user.iletisimId = iletisim.IletisimId;

                                                    }

                                                    #endregion

                                                    #region ExtaInfo

                                                    if (user.UserExtraInfoId != null)
                                                    {
                                                        if (not1 != "") user.UserExtraInfo.Not1 = not1;
                                                        if (not2 != "") user.UserExtraInfo.Not2 = not2;
                                                        if (not3 != "") user.UserExtraInfo.Not3 = not3;

                                                    }
                                                    else
                                                    {
                                                        if (not1 != "") extrainfo.Not1 = not1;
                                                        if (not2 != "") extrainfo.Not2 = not2;
                                                        if (not3 != "") extrainfo.Not3 = not3;


                                                        crm.UserExtraInfos.InsertOnSubmit(extrainfo);
                                                        crm.SubmitChanges();

                                                        user.UserExtraInfoId = extrainfo.id;

                                                    }



                                                    #endregion


                                                    #region Lisanlar



                                                    lisanslar.YayinDurumu = true;
                                                    lisanslar.Futgck = true;
                                                    lisanslar.WINX = true;



                                                    if (PRO == "1")
                                                    {
                                                        if (user.LisansDurum.ProYetki == false)
                                                        {
                                                            lisanslar.ProYetki = true;
                                                            acilanlar.Append("ProYetki;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ProYetki = user.LisansDurum.ProYetki;



                                                    if (CEP == "1")
                                                    {
                                                        if (user.LisansDurum.CepYetki == false)
                                                        {
                                                            lisanslar.CepYetki = true;
                                                            acilanlar.Append("CepYetki;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.CepYetki = user.LisansDurum.CepYetki;


                                                    #region BistPay


                                                    if (IMKBX == "1")
                                                    {
                                                        if (user.LisansDurum.PayX == false)
                                                        {
                                                            lisanslar.PayX = true;
                                                            acilanlar.Append("PayX;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayX = user.LisansDurum.PayX;

                                                    if (IMKBANL == "1")
                                                    {
                                                        if (user.LisansDurum.VeriAnalitik == false)
                                                        {
                                                            lisanslar.VeriAnalitik = true;
                                                            acilanlar.Append("VeriAnalitik;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.VeriAnalitik = user.LisansDurum.VeriAnalitik;

                                                    if (IMKBL1 == "1")
                                                    {
                                                        if (user.LisansDurum.PayL1 == false)
                                                        {
                                                            lisanslar.PayL1 = true;
                                                            acilanlar.Append("PayL1;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayL1 = user.LisansDurum.PayL1;


                                                    if (IMKBL1P == "1")
                                                    {
                                                        if (user.LisansDurum.PayLP == false)
                                                        {
                                                            lisanslar.PayL1 = true;
                                                            lisanslar.PayLP = true;
                                                            acilanlar.Append("PayLP;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayLP = user.LisansDurum.PayLP;



                                                    if (IMKBL2 == "1")
                                                    {
                                                        if (user.LisansDurum.PayL2 == false)
                                                        {
                                                            lisanslar.PayL1 = true;
                                                            lisanslar.PayLP = true;
                                                            lisanslar.PayL2 = true;
                                                            acilanlar.Append("PayL2;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayL2 = user.LisansDurum.PayL2;

                                                    if (IMKBL2P == "1")
                                                    {
                                                        if (user.LisansDurum.Pd2P == false)
                                                        {
                                                            lisanslar.PayL1 = true;
                                                            lisanslar.PayLP = true;
                                                            lisanslar.PayL2 = true;
                                                            lisanslar.Pd2P = true;
                                                            acilanlar.Append("Pd2P;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.Pd2P = user.LisansDurum.Pd2P;

                                                    if (IMKBISL == "1")
                                                    {
                                                        if (user.LisansDurum.PayGS == false)
                                                        {
                                                            lisanslar.PayGS = true;
                                                            acilanlar.Append("PayGS;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayGS = user.LisansDurum.PayGS;





                                                    #endregion
                                                    #region VIP

                                                    if (VIPL1 == "1")
                                                    {
                                                        if (user.LisansDurum.ViopL1 == false)
                                                        {
                                                            lisanslar.ViopL1 = true;
                                                            acilanlar.Append("ViopL1;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ViopL1 = user.LisansDurum.ViopL1;


                                                    if (VIPL1P == "1")
                                                    {
                                                        if (user.LisansDurum.ViopLP == false)
                                                        {
                                                            lisanslar.ViopL1 = true;
                                                            lisanslar.ViopLP = true;
                                                            acilanlar.Append("ViopLP;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ViopLP = user.LisansDurum.ViopLP;


                                                    if (VIPL2 == "1")
                                                    {
                                                        if (user.LisansDurum.ViopL2 == false)
                                                        {
                                                            lisanslar.ViopL1 = true;
                                                            lisanslar.ViopLP = true;
                                                            lisanslar.ViopL2 = true;
                                                            acilanlar.Append("ViopL2;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ViopL2 = user.LisansDurum.ViopL2;

                                                    if (VIPL2P == "1")
                                                    {
                                                        if (user.LisansDurum.Vd2P == false)
                                                        {
                                                            lisanslar.ViopL1 = true;
                                                            lisanslar.ViopLP = true;
                                                            lisanslar.ViopL2 = true;
                                                            lisanslar.Vd2P = true;
                                                            acilanlar.Append("Vd2P;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.Vd2P = user.LisansDurum.Vd2P;

                                                    if (VIPNET == "1")
                                                    {
                                                        if (user.LisansDurum.ViopGS == false)
                                                        {
                                                            lisanslar.ViopGS = true;
                                                            acilanlar.Append("ViopGS;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ViopGS = user.LisansDurum.ViopGS;



                                                    #endregion
                                                    #region Tahvil


                                                    if (THVL1 == "1")
                                                    {
                                                        if (user.LisansDurum.TahvilL1 == false)
                                                        {
                                                            lisanslar.TahvilL1 = true;
                                                            acilanlar.Append("TahvilL1;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.TahvilL1 = user.LisansDurum.TahvilL1;


                                                    if (THVL1P == "1")
                                                    {
                                                        if (user.LisansDurum.TahvilLP == false)
                                                        {
                                                            lisanslar.TahvilL1 = true;
                                                            lisanslar.TahvilLP = true;
                                                            acilanlar.Append("TahvilLP;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.TahvilLP = user.LisansDurum.TahvilLP;


                                                    if (THVL2 == "1")
                                                    {
                                                        if (user.LisansDurum.TahvilL2 == false)
                                                        {
                                                            lisanslar.TahvilL1 = true;
                                                            lisanslar.TahvilLP = true;
                                                            lisanslar.TahvilL2 = true;
                                                            acilanlar.Append("TahvilL2;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.TahvilL2 = user.LisansDurum.TahvilL2;



                                                    #endregion
                                                    #region YurtDisi


                                                    if (DJI == "1")
                                                    {
                                                        if (user.LisansDurum.DJI == false)
                                                        {
                                                            lisanslar.DJI = true;
                                                            acilanlar.Append("DJI;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.DJI = user.LisansDurum.DJI;



                                                    if (SPI == "1")
                                                    {
                                                        if (user.LisansDurum.SPI == false)
                                                        {
                                                            lisanslar.SPI = true;
                                                            acilanlar.Append("SPI;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.SPI = user.LisansDurum.SPI;

                                                    if (XETRA == "1")
                                                    {
                                                        if (user.LisansDurum.XETRA == false)
                                                        {
                                                            lisanslar.XETRA = true;
                                                            acilanlar.Append("XETRA;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.XETRA = user.LisansDurum.XETRA;


                                                    if (CBOTM == "1")
                                                    {
                                                        if (user.LisansDurum.CBOTM == false)
                                                        {
                                                            lisanslar.CBOTM = true;
                                                            acilanlar.Append("CBOTM;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.CBOTM = user.LisansDurum.CBOTM;


                                                    if (CBOT == "1")
                                                    {
                                                        if (user.LisansDurum.CBOT == false)
                                                        {
                                                            lisanslar.CBOT = true;
                                                            acilanlar.Append("CBOT;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.CBOT = user.LisansDurum.CBOT;

                                                    if (CMEM == "1")
                                                    {
                                                        if (user.LisansDurum.CMEM == false)
                                                        {
                                                            lisanslar.CMEM = true;
                                                            acilanlar.Append("CMEM;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.CMEM = user.LisansDurum.CMEM;

                                                    if (CME == "1")
                                                    {
                                                        if (user.LisansDurum.CME == false)
                                                        {
                                                            lisanslar.CME = true;
                                                            acilanlar.Append("CME;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.CME = user.LisansDurum.CME;

                                                    if (EUREX == "1")
                                                    {
                                                        if (user.LisansDurum.EUREX == false)
                                                        {
                                                            lisanslar.EUREX = true;
                                                            acilanlar.Append("EUREX;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.EUREX = user.LisansDurum.EUREX;

                                                    if (NYMEX == "1")
                                                    {
                                                        if (user.LisansDurum.NYMEX == false)
                                                        {
                                                            lisanslar.NYMEX = true;
                                                            acilanlar.Append("NYMEX;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.NYMEX = user.LisansDurum.NYMEX;


                                                    if (NYMEXM == "1")
                                                    {
                                                        if (user.LisansDurum.NYMEXM == false)
                                                        {
                                                            lisanslar.NYMEXM = true;
                                                            acilanlar.Append("NYMEXM;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.NYMEXM = user.LisansDurum.NYMEXM;

                                                    if (COMEX == "1")
                                                    {
                                                        if (user.LisansDurum.COMEX == false)
                                                        {
                                                            lisanslar.COMEX = true;
                                                            acilanlar.Append("COMEX;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.COMEX = user.LisansDurum.COMEX;





                                                    #endregion



                                                    crm.LisansDurums.InsertOnSubmit(lisanslar);
                                                    crm.SubmitChanges();
                                                    userevent.SonLisandurumID = lisanslar.LisansDurumId;
                                                    user.LisansDurum = lisanslar;
                                                    crm.SubmitChanges();
                                                    userevent.CalisanId = 7;
                                                    userevent.EventTypeId = 4;
                                                    userevent.AcilanLisans = acilanlar.ToString();
                                                    userevent.UserId = user.UserID;
                                                    crm.UserEvents.InsertOnSubmit(userevent);
                                                    crm.SubmitChanges();

                                                    #endregion


                                                    #endregion


                                                }

                                                var text = "OYAK AUTO CHANGE  ;  user name  = " + user.UserName + " ; Remeto Ip =" + userevent.IP;
                                                formListeTransaction(text);
                                                CalisanlaraKomutGonder("OYAK_AUTO_CREATE|" + text);
                                                SendToSSO(user.UserID);
                                                responsestr = "KULLANICI BILGILERI DEGISTIRILDI";
                                                IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                                IpDeamon.Connections[e.ConnectionId].Connected = false;

                                                return;
                                                #endregion
                                            }
                                            else
                                            {
                                                #region YeniKayit



                                                newcustomer.PmtsNo = "10226";
                                                newcustomer.UserName = idealusername;
                                                newcustomer.ProductType = "IDEAL";
                                                newcustomer.MusteriMenseiID = 1;


                                                if (idealsifre != "") newcustomer.Password = MyTools.Sifreleme.Encryp(idealsifre);
                                                if (aciklama != "") newcustomer.Aciklama = aciklama;
                                                #region isimcozumle
                                                if (isim != "")
                                                {
                                                    var gelenisim = isim.Trim().Split(' ');
                                                    var ad = "";
                                                    var soyad = "";
                                                    if (gelenisim.Length == 1)
                                                    {
                                                        ad = gelenisim[0];
                                                        soyad = "yok";

                                                    }
                                                    else if (gelenisim.Length == 2)
                                                    {
                                                        ad = gelenisim[0];
                                                        soyad = gelenisim[1];

                                                    }
                                                    else if (gelenisim.Length == 3)
                                                    {
                                                        ad = gelenisim[0] + " " + gelenisim[1];
                                                        soyad = gelenisim[2];
                                                    }


                                                    newcustomer.Name = ad;
                                                    newcustomer.Surname = soyad;
                                                }


                                                #endregion
                                                newcustomer.ExpiryDate = expireDate;
                                                newcustomer.BaslangicTarihi = DateTime.Now;
                                                newcustomer.StatusId = 1;


                                                #region iletisimBilgileri

                                                iletisim.email = mail;
                                                iletisim.acikadres = adres;
                                                iletisim.Tel1 = telefon;
                                                if (sehir != "")
                                                {

                                                    var il = MyTools.SehirIdBul(sehir);
                                                    if (il != null && il.Id > 0)
                                                    {
                                                        iletisim.IlId = il.Id;
                                                        iletisim.UlkeId = il.UlkeId;
                                                    }

                                                }
                                                #endregion

                                                #region KurumsalBilgiler

                                                if (sube != "") kurumsalbilgi.KurumSube = sube;
                                                if (kullanicitip != "") kurumsalbilgi.KurumKullaniciTip = kullanicitip;
                                                if (kurummusterino != "") kurumsalbilgi.Not1 = kurummusterino;
                                                if (hesapno != "") kurumsalbilgi.kurumhesapno = hesapno;


                                                #endregion
                                                #region ExtraInfo


                                                if (not1 != "") extrainfo.Not1 = not1;
                                                if (not2 != "") extrainfo.Not2 = not2;
                                                if (not3 != "") extrainfo.Not3 = not3;

                                                #endregion
                                                #region Lisanslar

                                                lisanslar.YayinDurumu = true;
                                                lisanslar.Futgck = true;
                                                lisanslar.WINX = true;

                                                if (PRO == "1") lisanslar.ProYetki = true;
                                                if (CEP == "1") lisanslar.CepYetki = true;
                                                if (IMKBX == "1") lisanslar.PayX = true;
                                                if (IMKBANL == "1") lisanslar.VeriAnalitik = true;
                                                if (IMKBL1 == "1") lisanslar.PayL1 = true;
                                                if (IMKBL1P == "1") lisanslar.PayLP = true;
                                                if (IMKBL2 == "1") lisanslar.Pd2P = true;
                                                if (IMKBISL == "1") lisanslar.PayGS = true;
                                                if (VIPL1 == "1") lisanslar.ViopL1 = true;
                                                if (VIPL1P == "1") lisanslar.ViopLP = true;
                                                if (VIPL2 == "1") lisanslar.ViopL2 = true;
                                                if (VIPL2P == "1") lisanslar.Vd2P = true;
                                                if (VIPNET == "1") lisanslar.ViopGS = true;
                                                if (THVL1 == "1") lisanslar.TahvilL1 = true;
                                                if (THVL1P == "1") lisanslar.TahvilLP = true;
                                                if (THVL2 == "1") lisanslar.TahvilL2 = true;
                                                if (DJI == "1") lisanslar.DJI = true;
                                                if (SPI == "1") lisanslar.SPI = true;
                                                if (XETRA == "1") lisanslar.XETRA = true;
                                                if (CBOTM == "1") lisanslar.CBOTM = true;
                                                if (CBOT == "1") lisanslar.CBOT = true;
                                                if (CMEM == "1") lisanslar.CMEM = true;
                                                if (CME == "1") lisanslar.CME = true;
                                                if (EUREX == "1") lisanslar.EUREX = true;
                                                if (NYMEX == "1") lisanslar.NYMEX = true;
                                                if (NYMEXM == "1") lisanslar.NYMEXM = true;
                                                if (COMEX == "1") lisanslar.COMEX = true;
                                                #endregion


                                                #region ToSSO

                                                //// main server create client
                                                //var sb = new StringBuilder();
                                                //sb.Append(newcustomer.Username);
                                                //sb.Append("|" + "Password;" + newcustomer.Password);
                                                //sb.Append("|" + "Explanation;" + newcustomer.Explanation);
                                                //sb.Append("|" + "PmtsNo;" + newcustomer.PmtsNo);
                                                //sb.Append("|" + "BaslangicTarih;" + newcustomer.BaslangicTarih);
                                                //if (aciklama != "") sb.Append("|" + "Explanation;" + newcustomer.Explanation);
                                                //if (sube != "") sb.Append("|" + "Sube;" + newcustomer.Sube);
                                                //if (kullanicitip != "") sb.Append("|" + "KullaniciTip;" + newcustomer.KullaniciTip);
                                                //if (isim != "") sb.Append("|" + "Isim;" + newcustomer.Isim);
                                                //if (sehir != "") sb.Append("|" + "Sehir;" + newcustomer.Sehir);
                                                //if (kurummusterino != "") sb.Append("|" + "KurumMusteriNo;" + newcustomer.KurumMusteriNo);
                                                //if (not1 != "") sb.Append("|" + "Not1;" + newcustomer.Not1);
                                                //if (not2 != "") sb.Append("|" + "Not2;" + newcustomer.Not2);
                                                //if (not3 != "") sb.Append("|" + "Not3;" + newcustomer.Not3);
                                                //if (mail != "") sb.Append("|" + "Mail;" + newcustomer.Mail);
                                                //if (adres != "") sb.Append("|" + "Adres;" + newcustomer.Adres);
                                                //if (telefon != "") sb.Append("|" + "Telefon;" + newcustomer.Telefon);
                                                //if (PRO == "1") sb.Append("|" + "PRO;1");
                                                //if (CEP == "1") sb.Append("|" + "CEP;1");
                                                //if (IMKBX == "1") sb.Append("|" + "IMKBX;1");
                                                //if (IMKBL1 == "1") sb.Append("|" + "IMKBL1;1");
                                                //if (IMKBL1P == "1") sb.Append("|" + "IMKBL1P;1");
                                                //if (IMKBL2 == "1") sb.Append("|" + "IMKBL2;1");
                                                //if (IMKBISL == "1") sb.Append("|" + "IMKBISL;1");
                                                //if (VIPL1 == "1") sb.Append("|" + "VIPL1;1");
                                                //if (VIPL1P == "1") sb.Append("|" + "VIPL1P;1");
                                                //if (VIPL2 == "1") sb.Append("|" + "VIPL2;1");
                                                //if (VIPNET == "1") sb.Append("|" + "VIPNET;1");
                                                //if (THVL1 == "1") sb.Append("|" + "THVL1;1");
                                                //if (THVL1P == "1") sb.Append("|" + "THVL1P;1");
                                                //if (THVL2 == "1") sb.Append("|" + "THVL2;1");
                                                //if (DJI == "1") sb.Append("|" + "DJI;1");
                                                //if (SPI == "1") sb.Append("|" + "SPI;1");
                                                //if (XETRA == "1") sb.Append("|" + "XETRA;1");
                                                //if (CBOTM == "1") sb.Append("|" + "CBOTM;1");
                                                //if (CBOT == "1") sb.Append("|" + "CBOT;1");
                                                //if (CMEM == "1") sb.Append("|" + "CMEM;1");
                                                //if (CME == "1") sb.Append("|" + "CME;1");
                                                //if (EUREX == "1") sb.Append("|" + "EUREX;1");
                                                //if (NYMEX == "1") sb.Append("|" + "NYMEX;1");
                                                //if (NYMEXM == "1") sb.Append("|" + "NYMEXM;1");
                                                //if (COMEX == "1") sb.Append("|" + "COMEX;1");
                                                //if (expiredate != "") sb.Append("|" + "EXPIREDATE;" + expiredate); 
                                                #endregion


                                                crm.UserExtraInfos.InsertOnSubmit(extrainfo);
                                                crm.SubmitChanges();

                                                newcustomer.UserExtraInfoId = extrainfo.id;


                                                crm.LisansDurums.InsertOnSubmit(lisanslar);
                                                crm.SubmitChanges();




                                                newcustomer.LisansDurumId = lisanslar.LisansDurumId;
                                                userevent.SonLisandurumID = lisanslar.LisansDurumId;

                                                crm.Iletisims.InsertOnSubmit(iletisim);
                                                crm.SubmitChanges();
                                                newcustomer.iletisimId = iletisim.IletisimId;

                                                crm.KurumsalBilgilers.InsertOnSubmit(kurumsalbilgi);
                                                crm.SubmitChanges();
                                                newcustomer.KurumsalBilgilerId = kurumsalbilgi.Id;

                                                crm.Users.InsertOnSubmit(newcustomer);
                                                crm.SubmitChanges();

                                                userevent.CalisanId = 7;
                                                userevent.EventTypeId = 5;
                                                userevent.UserId = newcustomer.UserID;


                                                crm.UserEvents.InsertOnSubmit(userevent);
                                                crm.SubmitChanges();




                                                var text = "OYAK AUTO CREATE- Yeni Kullanıcı Açma  ; açilan user name  = " + newcustomer.UserName + " ;  Remeto Ip =" + userevent.IP;
                                                formListeTransaction(text);
                                                CalisanlaraKomutGonder("OYAK_AUTO_CREATE|" + text);
                                                SendToSSO(newcustomer.UserID);




                                                responsestr = komut + "|" + "OK" + "|" + DateTime.Now.ToString("HH:mm:ss");
                                                IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                                IpDeamon.Connections[e.ConnectionId].Connected = false;

                                                // formServerCustomers.SendClientToAnaServer(idealusername);
                                                return;


                                                #endregion

                                            }

                                        }

                                        // OYAKSIFRESORGU
                                        if (komut == "OYAKSIFRESORGU")
                                        {
                                            string username = fieldarray[4].Trim();

                                            if (crm.Users.Where(x => x.UserName == username).Any())
                                            {
                                                var user = crm.Users.FirstOrDefault(x => x.UserName == username);

                                                if (user.PmtsNo != "10226") return;

                                                responsestr = komut + "|" + "OK" + "|" + DateTime.Now.ToString("HH:mm:ss") + "|" + "SIFRE=" + MyTools.Sifreleme.Decryp(user.Password);
                                                IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                                IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                return;

                                            }
                                            else
                                            {
                                                responsestr = "HATA : Bu User Sistemde yok";
                                                IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                                IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                return;

                                            }

                                        }
                                    }
                                    else if (kurum == "OSMANLI" && kurumpassword == "OSMANLIIDEAL2018")
                                    {
                                        User newcustomer = new User();
                                        LisansDurum lisanslar = new LisansDurum();
                                        KurumsalBilgiler kurumsalbilgi = new KurumsalBilgiler();
                                        UserExtraInfo extrainfo = new UserExtraInfo();

                                        var iletisim = new Iletisim();


                                        string hesapno = "";
                                        string idealsifre = "2458";
                                        string idealusername = "";
                                        string aciklama = "";
                                        string sube = "";
                                        string kullanicitip = "";
                                        string isim = "";
                                        string sehir = "";
                                        string kurummusterino = "";
                                        string not1 = "";
                                        string not2 = "";
                                        string not3 = "";
                                        string expiredate = "";
                                        string mail = "";
                                        string adres = "";
                                        string telefon = "";
                                        string IMKBX = "";
                                        string IMKBL1 = "";
                                        string IMKBL1P = "";
                                        string IMKBL2 = "";
                                        string IMKBL2P = "";
                                        string IMKBISL = "";
                                        string VIPL1 = "";
                                        string VIPL1P = "";
                                        string VIPL2 = "";
                                        string VIPL2P = "";
                                        string VIPNET = "";
                                        string option = "";

                                        for (int i = 4; i < fieldarray.Length; i++)
                                        {
                                            var splitarray = fieldarray[i].Split('=');
                                            switch (splitarray[0]._ToEngUp())
                                            {
                                                case "HESAPNO": hesapno = splitarray[1]; break;
                                                case "IDEALSIFRE": idealsifre = splitarray[1]; break;
                                                case "ACIKLAMA": aciklama = splitarray[1]; break;
                                                case "SUBE": sube = splitarray[1]; break;
                                                case "KULLANICITIP": kullanicitip = splitarray[1]; break;
                                                case "ISIM": isim = splitarray[1]; break;
                                                case "SEHIR": sehir = splitarray[1]; break;
                                                case "KURUMMUSTERINO": kurummusterino = splitarray[1]; break;
                                                case "NOT1": not1 = splitarray[1]; break;
                                                case "NOT2": not2 = splitarray[1]; break;
                                                case "NOT3": not3 = splitarray[1]; break;
                                                case "EXPIREDATE": expiredate = splitarray[1]; break;
                                                case "IMKBX": IMKBX = splitarray[1]; break;
                                                case "IMKBL1": IMKBL1 = splitarray[1]; break;
                                                case "IMKBL1P": IMKBL1P = splitarray[1]; break;
                                                case "IMKBL2": IMKBL2 = splitarray[1]; break;
                                                case "IMKBL2P": IMKBL2P = splitarray[1]; break;
                                                case "IMKBISL": IMKBISL = splitarray[1]; break;
                                                case "VIPL1": VIPL1 = splitarray[1]; break;
                                                case "VIPL1P": VIPL1P = splitarray[1]; break;
                                                case "VIPL2": VIPL2 = splitarray[1]; break;
                                                case "VIPL2P": VIPL2P = splitarray[1]; break;
                                                case "VIPNET": VIPNET = splitarray[1]; break;
                                                case "MAIL": mail = splitarray[1]; break;
                                                case "ADRES": adres = splitarray[1]; break;
                                                case "TELEFON": telefon = splitarray[1]; break;
                                                case "OPTION": option = splitarray[1]; break;
                                            }
                                        }

                                        var expireDate = DateTime.ParseExact(expiredate, "yyyyMMdd", CultureInfo.InvariantCulture);
                                        var kaptianlar = new StringBuilder();
                                        var acilanlar = new StringBuilder();



                                        if (crm.Users.Where(x => x.UserName == hesapno).Any())
                                        {
                                            #region Guncele


                                            var user = crm.Users.FirstOrDefault(x => x.UserName == hesapno);

                                            if (user.LisansDurum.YayinDurumu == false)
                                            {

                                                if (komut == "1" && option == "1")
                                                {
                                                    #region userKapali

                                                    user.ExpiryDate = expireDate;

                                                    user.PmtsNo = hesapno;
                                                    user.ProductType = "IDEAL";
                                                    if (user.MusteriMenseiID == null)
                                                        user.MusteriMenseiID = 1;


                                                    if (idealsifre != "") user.Password = MyTools.Sifreleme.Encryp(idealsifre);
                                                    if (aciklama != "") user.Aciklama = aciklama;
                                                    #region isimcozumle
                                                    if (isim != "")
                                                    {
                                                        var gelenisim = isim.Trim().Split(' ');
                                                        var ad = "";
                                                        var soyad = "";
                                                        if (gelenisim.Length == 1)
                                                        {
                                                            ad = gelenisim[0];
                                                            soyad = "yok";

                                                        }
                                                        else if (gelenisim.Length == 2)
                                                        {
                                                            ad = gelenisim[0];
                                                            soyad = gelenisim[1];

                                                        }
                                                        else if (gelenisim.Length == 3)
                                                        {
                                                            ad = gelenisim[0] + " " + gelenisim[1];
                                                            soyad = gelenisim[2];
                                                        }


                                                        user.Name = ad;
                                                        user.Surname = soyad;
                                                    }

                                                    #endregion

                                                    #region kurumsalbilgi 


                                                    if (user.KurumsalBilgilerId != null)
                                                    {

                                                        if (sube != "") user.KurumsalBilgiler.KurumSube = sube;
                                                        if (kullanicitip != "") user.KurumsalBilgiler.KurumKullaniciTip = kullanicitip;
                                                        if (kurummusterino != "") user.KurumsalBilgiler.Not1 = kurummusterino;
                                                        if (hesapno != "") user.KurumsalBilgiler.kurumhesapno = hesapno;


                                                    }
                                                    else
                                                    {
                                                        if (sube != "") kurumsalbilgi.KurumSube = sube;
                                                        if (kullanicitip != "") kurumsalbilgi.KurumKullaniciTip = kullanicitip;
                                                        if (kurummusterino != "") kurumsalbilgi.Not1 = kurummusterino;
                                                        if (hesapno != "") kurumsalbilgi.kurumhesapno = hesapno;

                                                        crm.KurumsalBilgilers.InsertOnSubmit(kurumsalbilgi);
                                                        crm.SubmitChanges();
                                                        user.KurumsalBilgilerId = kurumsalbilgi.Id;
                                                    }





                                                    #endregion

                                                    #region iletisimbilgileri 



                                                    if (user.iletisimId != null)
                                                    {
                                                        if (mail != "") user.Iletisim.email = mail;
                                                        if (adres != "") user.Iletisim.acikadres = adres;
                                                        if (telefon != "") user.Iletisim.Tel1 = telefon;

                                                        if (sehir != "")
                                                        {
                                                            var il = MyTools.SehirIdBul(sehir);
                                                            if (il != null && il.Id > 0)
                                                            {
                                                                user.Iletisim.IlId = il.Id;
                                                                user.Iletisim.UlkeId = il.UlkeId;
                                                            }

                                                        }

                                                    }
                                                    else
                                                    {
                                                        if (mail != "") iletisim.email = mail;
                                                        if (adres != "") iletisim.acikadres = adres;
                                                        if (telefon != "") iletisim.Tel1 = telefon;

                                                        if (sehir != "")
                                                        {
                                                            var il = MyTools.SehirIdBul(sehir);
                                                            if (il != null && il.Id > 0)
                                                            {
                                                                iletisim.IlId = il.Id;
                                                                iletisim.UlkeId = il.UlkeId;
                                                            }

                                                        }



                                                        crm.Iletisims.InsertOnSubmit(iletisim);
                                                        crm.SubmitChanges();
                                                        user.iletisimId = iletisim.IletisimId;

                                                    }

                                                    #endregion

                                                    #region ExtaInfo

                                                    if (user.UserExtraInfoId != null)
                                                    {
                                                        if (not1 != "") user.UserExtraInfo.Not1 = not1;
                                                        if (not2 != "") user.UserExtraInfo.Not2 = not2;
                                                        if (not3 != "") user.UserExtraInfo.Not3 = not3;

                                                    }
                                                    else
                                                    {
                                                        if (not1 != "") extrainfo.Not1 = not1;
                                                        if (not2 != "") extrainfo.Not2 = not2;
                                                        if (not3 != "") extrainfo.Not3 = not3;


                                                        crm.UserExtraInfos.InsertOnSubmit(extrainfo);
                                                        crm.SubmitChanges();

                                                        user.UserExtraInfoId = extrainfo.id;

                                                    }



                                                    #endregion


                                                    #region Lisanlar



                                                    lisanslar.YayinDurumu = true;
                                                    lisanslar.Futgck = true;
                                                    lisanslar.WINX = true;
                                                    lisanslar.CepYetki = true;




                                                    #region BistPay


                                                    if (IMKBX == "1")
                                                    {
                                                        if (user.LisansDurum.PayX == false)
                                                        {
                                                            lisanslar.PayX = true;
                                                            acilanlar.Append("PayX;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayX = user.LisansDurum.PayX;



                                                    if (IMKBL1 == "1")
                                                    {
                                                        if (user.LisansDurum.PayL1 == false)
                                                        {
                                                            lisanslar.PayL1 = true;
                                                            acilanlar.Append("PayL1;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayL1 = user.LisansDurum.PayL1;


                                                    if (IMKBL1P == "1")
                                                    {
                                                        if (user.LisansDurum.PayLP == false)
                                                        {
                                                            lisanslar.PayL1 = true;
                                                            lisanslar.PayLP = true;
                                                            acilanlar.Append("PayLP;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayLP = user.LisansDurum.PayLP;



                                                    if (IMKBL2 == "1")
                                                    {
                                                        if (user.LisansDurum.PayL2 == false)
                                                        {
                                                            lisanslar.PayL1 = true;
                                                            lisanslar.PayLP = true;
                                                            lisanslar.PayL2 = true;
                                                            acilanlar.Append("PayL2;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayL2 = user.LisansDurum.PayL2;

                                                    if (IMKBL2P == "1")
                                                    {
                                                        if (user.LisansDurum.Pd2P == false)
                                                        {
                                                            lisanslar.PayL1 = true;
                                                            lisanslar.PayLP = true;
                                                            lisanslar.PayL2 = true;
                                                            lisanslar.Pd2P = true;
                                                            acilanlar.Append("Pd2P;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.Pd2P = user.LisansDurum.Pd2P;

                                                    if (IMKBISL == "1")
                                                    {
                                                        if (user.LisansDurum.PayGS == false)
                                                        {
                                                            lisanslar.PayGS = true;
                                                            acilanlar.Append("PayGS;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayGS = user.LisansDurum.PayGS;





                                                    #endregion
                                                    #region VIP

                                                    if (VIPL1 == "1")
                                                    {
                                                        if (user.LisansDurum.ViopL1 == false)
                                                        {
                                                            lisanslar.ViopL1 = true;
                                                            acilanlar.Append("ViopL1;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ViopL1 = user.LisansDurum.ViopL1;


                                                    if (VIPL1P == "1")
                                                    {
                                                        if (user.LisansDurum.ViopLP == false)
                                                        {
                                                            lisanslar.ViopL1 = true;
                                                            lisanslar.ViopLP = true;
                                                            acilanlar.Append("ViopLP;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ViopLP = user.LisansDurum.ViopLP;


                                                    if (VIPL2 == "1")
                                                    {
                                                        if (user.LisansDurum.ViopL2 == false)
                                                        {
                                                            lisanslar.ViopL1 = true;
                                                            lisanslar.ViopLP = true;
                                                            lisanslar.ViopL2 = true;
                                                            acilanlar.Append("ViopL2;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ViopL2 = user.LisansDurum.ViopL2;

                                                    if (VIPL2P == "1")
                                                    {
                                                        if (user.LisansDurum.Vd2P == false)
                                                        {
                                                            lisanslar.ViopL1 = true;
                                                            lisanslar.ViopLP = true;
                                                            lisanslar.ViopL2 = true;
                                                            lisanslar.Vd2P = true;
                                                            acilanlar.Append("Vd2P;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.Vd2P = user.LisansDurum.Vd2P;

                                                    if (VIPNET == "1")
                                                    {
                                                        if (user.LisansDurum.ViopGS == false)
                                                        {
                                                            lisanslar.ViopGS = true;
                                                            acilanlar.Append("ViopGS;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ViopGS = user.LisansDurum.ViopGS;



                                                    #endregion



                                                    crm.LisansDurums.InsertOnSubmit(lisanslar);
                                                    crm.SubmitChanges();
                                                    userevent.SonLisandurumID = lisanslar.LisansDurumId;
                                                    user.LisansDurum = lisanslar;
                                                    crm.SubmitChanges();
                                                    userevent.CalisanId = 7;
                                                    userevent.EventTypeId = 1;
                                                    userevent.UserId = user.UserID;
                                                    crm.UserEvents.InsertOnSubmit(userevent);
                                                    crm.SubmitChanges();

                                                    #endregion


                                                    #endregion 
                                                }
                                            }
                                            else
                                            {
                                                if (komut == "3")
                                                {
                                                    user.ExpiryDate = expireDate;
                                                }
                                                else if (komut == "1" && option == "1")
                                                {
                                                    #region userAcik
                                                    user.ExpiryDate = expireDate;

                                                    user.PmtsNo = hesapno;
                                                    user.ProductType = "IDEAL";
                                                    if (user.MusteriMenseiID == null)
                                                        user.MusteriMenseiID = 1;


                                                    if (idealsifre != "") user.Password = MyTools.Sifreleme.Encryp(idealsifre);
                                                    if (aciklama != "") user.Aciklama = aciklama;
                                                    #region isimcozumle
                                                    if (isim != "")
                                                    {
                                                        var gelenisim = isim.Trim().Split(' ');
                                                        var ad = "";
                                                        var soyad = "";
                                                        if (gelenisim.Length == 1)
                                                        {
                                                            ad = gelenisim[0];
                                                            soyad = "yok";

                                                        }
                                                        else if (gelenisim.Length == 2)
                                                        {
                                                            ad = gelenisim[0];
                                                            soyad = gelenisim[1];

                                                        }
                                                        else if (gelenisim.Length == 3)
                                                        {
                                                            ad = gelenisim[0] + " " + gelenisim[1];
                                                            soyad = gelenisim[2];
                                                        }


                                                        user.Name = ad;
                                                        user.Surname = soyad;
                                                    }

                                                    #endregion

                                                    #region kurumsalbilgi 


                                                    if (user.KurumsalBilgilerId != null)
                                                    {

                                                        if (sube != "") user.KurumsalBilgiler.KurumSube = sube;
                                                        if (kullanicitip != "") user.KurumsalBilgiler.KurumKullaniciTip = kullanicitip;
                                                        if (kurummusterino != "") user.KurumsalBilgiler.Not1 = kurummusterino;
                                                        if (hesapno != "") user.KurumsalBilgiler.kurumhesapno = hesapno;


                                                    }
                                                    else
                                                    {
                                                        if (sube != "") kurumsalbilgi.KurumSube = sube;
                                                        if (kullanicitip != "") kurumsalbilgi.KurumKullaniciTip = kullanicitip;
                                                        if (kurummusterino != "") kurumsalbilgi.Not1 = kurummusterino;
                                                        if (hesapno != "") kurumsalbilgi.kurumhesapno = hesapno;

                                                        crm.KurumsalBilgilers.InsertOnSubmit(kurumsalbilgi);
                                                        crm.SubmitChanges();
                                                        user.KurumsalBilgilerId = kurumsalbilgi.Id;
                                                    }





                                                    #endregion

                                                    #region iletisimbilgileri 



                                                    if (user.iletisimId != null)
                                                    {
                                                        if (mail != "") user.Iletisim.email = mail;
                                                        if (adres != "") user.Iletisim.acikadres = adres;
                                                        if (telefon != "") user.Iletisim.Tel1 = telefon;

                                                        if (sehir != "")
                                                        {
                                                            var il = MyTools.SehirIdBul(sehir);
                                                            if (il != null && il.Id > 0)
                                                            {
                                                                user.Iletisim.IlId = il.Id;
                                                                user.Iletisim.UlkeId = il.UlkeId;
                                                            }

                                                        }

                                                    }
                                                    else
                                                    {
                                                        if (mail != "") iletisim.email = mail;
                                                        if (adres != "") iletisim.acikadres = adres;
                                                        if (telefon != "") iletisim.Tel1 = telefon;

                                                        if (sehir != "")
                                                        {
                                                            var il = MyTools.SehirIdBul(sehir);
                                                            if (il != null && il.Id > 0)
                                                            {
                                                                iletisim.IlId = il.Id;
                                                                iletisim.UlkeId = il.UlkeId;
                                                            }

                                                        }



                                                        crm.Iletisims.InsertOnSubmit(iletisim);
                                                        crm.SubmitChanges();
                                                        user.iletisimId = iletisim.IletisimId;

                                                    }

                                                    #endregion

                                                    #region ExtaInfo

                                                    if (user.UserExtraInfoId != null)
                                                    {
                                                        if (not1 != "") user.UserExtraInfo.Not1 = not1;
                                                        if (not2 != "") user.UserExtraInfo.Not2 = not2;
                                                        if (not3 != "") user.UserExtraInfo.Not3 = not3;

                                                    }
                                                    else
                                                    {
                                                        if (not1 != "") extrainfo.Not1 = not1;
                                                        if (not2 != "") extrainfo.Not2 = not2;
                                                        if (not3 != "") extrainfo.Not3 = not3;


                                                        crm.UserExtraInfos.InsertOnSubmit(extrainfo);
                                                        crm.SubmitChanges();

                                                        user.UserExtraInfoId = extrainfo.id;

                                                    }



                                                    #endregion


                                                    #region Lisanlar



                                                    lisanslar.YayinDurumu = true;
                                                    lisanslar.Futgck = true;
                                                    lisanslar.WINX = true;
                                                    lisanslar.CepYetki = true;


                                                    #region BistPay


                                                    if (IMKBX == "1")
                                                    {
                                                        if (user.LisansDurum.PayX == false)
                                                        {
                                                            lisanslar.PayX = true;
                                                            acilanlar.Append("PayX;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayX = user.LisansDurum.PayX;



                                                    if (IMKBL1 == "1")
                                                    {
                                                        if (user.LisansDurum.PayL1 == false)
                                                        {
                                                            lisanslar.PayL1 = true;
                                                            acilanlar.Append("PayL1;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayL1 = user.LisansDurum.PayL1;


                                                    if (IMKBL1P == "1")
                                                    {
                                                        if (user.LisansDurum.PayLP == false)
                                                        {
                                                            lisanslar.PayL1 = true;
                                                            lisanslar.PayLP = true;
                                                            acilanlar.Append("PayLP;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayLP = user.LisansDurum.PayLP;



                                                    if (IMKBL2 == "1")
                                                    {
                                                        if (user.LisansDurum.PayL2 == false)
                                                        {
                                                            lisanslar.PayL1 = true;
                                                            lisanslar.PayLP = true;
                                                            lisanslar.PayL2 = true;
                                                            acilanlar.Append("PayL2;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayL2 = user.LisansDurum.PayL2;

                                                    if (IMKBL2P == "1")
                                                    {
                                                        if (user.LisansDurum.Pd2P == false)
                                                        {
                                                            lisanslar.PayL1 = true;
                                                            lisanslar.PayLP = true;
                                                            lisanslar.PayL2 = true;
                                                            lisanslar.Pd2P = true;
                                                            acilanlar.Append("Pd2P;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.Pd2P = user.LisansDurum.Pd2P;

                                                    if (IMKBISL == "1")
                                                    {
                                                        if (user.LisansDurum.PayGS == false)
                                                        {
                                                            lisanslar.PayGS = true;
                                                            acilanlar.Append("PayGS;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.PayGS = user.LisansDurum.PayGS;





                                                    #endregion
                                                    #region VIP

                                                    if (VIPL1 == "1")
                                                    {
                                                        if (user.LisansDurum.ViopL1 == false)
                                                        {
                                                            lisanslar.ViopL1 = true;
                                                            acilanlar.Append("ViopL1;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ViopL1 = user.LisansDurum.ViopL1;


                                                    if (VIPL1P == "1")
                                                    {
                                                        if (user.LisansDurum.ViopLP == false)
                                                        {
                                                            lisanslar.ViopL1 = true;
                                                            lisanslar.ViopLP = true;
                                                            acilanlar.Append("ViopLP;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ViopLP = user.LisansDurum.ViopLP;


                                                    if (VIPL2 == "1")
                                                    {
                                                        if (user.LisansDurum.ViopL2 == false)
                                                        {
                                                            lisanslar.ViopL1 = true;
                                                            lisanslar.ViopLP = true;
                                                            lisanslar.ViopL2 = true;
                                                            acilanlar.Append("ViopL2;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ViopL2 = user.LisansDurum.ViopL2;

                                                    if (VIPL2P == "1")
                                                    {
                                                        if (user.LisansDurum.Vd2P == false)
                                                        {
                                                            lisanslar.ViopL1 = true;
                                                            lisanslar.ViopLP = true;
                                                            lisanslar.ViopL2 = true;
                                                            lisanslar.Vd2P = true;
                                                            acilanlar.Append("Vd2P;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ViopL2 = user.LisansDurum.ViopL2;

                                                    if (VIPNET == "1")
                                                    {
                                                        if (user.LisansDurum.ViopGS == false)
                                                        {
                                                            lisanslar.ViopGS = true;
                                                            acilanlar.Append("ViopGS;");
                                                        }
                                                    }
                                                    else
                                                        lisanslar.ViopGS = user.LisansDurum.ViopGS;



                                                    #endregion



                                                    #endregion

                                                    #endregion
                                                }


                                            }

                                            var text = "USER DEĞİŞİM  ;  user name  = " + user.UserName + " ; Remeto Ip =" + userevent.IP;
                                            if (komut == "3")
                                                text = "USER KAPATMA  ;  user name  = " + user.UserName + " ; Remeto Ip =" + userevent.IP;
                                            formListeTransaction(text);
                                            CalisanlaraKomutGonder("OYAK_AUTO_CREATE|" + text);
                                            SendToSSO(user.UserID);
                                            responsestr = "KULLANICI BILGILERI DEGISTIRILDI";
                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;

                                            return;
                                            #endregion

                                        }
                                        else
                                        {
                                            #region YeniKayit



                                            newcustomer.PmtsNo = "10226";
                                            newcustomer.UserName = idealusername;
                                            newcustomer.ProductType = "IDEAL";
                                            newcustomer.MusteriMenseiID = 1;


                                            if (idealsifre != "") newcustomer.Password = MyTools.Sifreleme.Encryp(idealsifre);
                                            if (aciklama != "") newcustomer.Aciklama = aciklama;
                                            #region isimcozumle
                                            if (isim != "")
                                            {
                                                var gelenisim = isim.Trim().Split(' ');
                                                var ad = "";
                                                var soyad = "";
                                                if (gelenisim.Length == 1)
                                                {
                                                    ad = gelenisim[0];
                                                    soyad = "yok";

                                                }
                                                else if (gelenisim.Length == 2)
                                                {
                                                    ad = gelenisim[0];
                                                    soyad = gelenisim[1];

                                                }
                                                else if (gelenisim.Length == 3)
                                                {
                                                    ad = gelenisim[0] + " " + gelenisim[1];
                                                    soyad = gelenisim[2];
                                                }


                                                newcustomer.Name = ad;
                                                newcustomer.Surname = soyad;
                                            }


                                            #endregion
                                            newcustomer.ExpiryDate = expireDate;
                                            newcustomer.BaslangicTarihi = DateTime.Now;
                                            newcustomer.StatusId = 1;


                                            #region iletisimBilgileri

                                            iletisim.email = mail;
                                            iletisim.acikadres = adres;
                                            iletisim.Tel1 = telefon;
                                            if (sehir != "")
                                            {

                                                var il = MyTools.SehirIdBul(sehir);
                                                if (il != null && il.Id > 0)
                                                {
                                                    iletisim.IlId = il.Id;
                                                    iletisim.UlkeId = il.UlkeId;
                                                }

                                            }
                                            #endregion

                                            #region KurumsalBilgiler

                                            if (sube != "") kurumsalbilgi.KurumSube = sube;
                                            if (kullanicitip != "") kurumsalbilgi.KurumKullaniciTip = kullanicitip;
                                            if (kurummusterino != "") kurumsalbilgi.Not1 = kurummusterino;
                                            if (hesapno != "") kurumsalbilgi.kurumhesapno = hesapno;


                                            #endregion
                                            #region ExtraInfo


                                            if (not1 != "") extrainfo.Not1 = not1;
                                            if (not2 != "") extrainfo.Not2 = not2;
                                            if (not3 != "") extrainfo.Not3 = not3;

                                            #endregion
                                            #region Lisanslar

                                            lisanslar.YayinDurumu = true;
                                            lisanslar.Futgck = true;
                                            lisanslar.WINX = true;


                                            if (IMKBX == "1") lisanslar.PayX = true;

                                            if (IMKBL1 == "1") lisanslar.PayL1 = true;
                                            if (IMKBL1P == "1") lisanslar.PayLP = true;
                                            if (IMKBL2 == "1") lisanslar.PayL2 = true;
                                            if (IMKBL2P == "1") lisanslar.Pd2P = true;
                                            if (IMKBISL == "1") lisanslar.PayGS = true;
                                            if (VIPL1 == "1") lisanslar.ViopL1 = true;
                                            if (VIPL1P == "1") lisanslar.ViopLP = true;
                                            if (VIPL2 == "1") lisanslar.ViopL2 = true;
                                            if (VIPL2P == "1") lisanslar.Vd2P = true;
                                            if (VIPNET == "1") lisanslar.ViopGS = true;

                                            #endregion





                                            crm.UserExtraInfos.InsertOnSubmit(extrainfo);
                                            crm.SubmitChanges();

                                            newcustomer.UserExtraInfoId = extrainfo.id;


                                            crm.LisansDurums.InsertOnSubmit(lisanslar);
                                            crm.SubmitChanges();




                                            newcustomer.LisansDurumId = lisanslar.LisansDurumId;
                                            userevent.SonLisandurumID = lisanslar.LisansDurumId;

                                            crm.Iletisims.InsertOnSubmit(iletisim);
                                            crm.SubmitChanges();
                                            newcustomer.iletisimId = iletisim.IletisimId;

                                            crm.KurumsalBilgilers.InsertOnSubmit(kurumsalbilgi);
                                            crm.SubmitChanges();
                                            newcustomer.KurumsalBilgilerId = kurumsalbilgi.Id;

                                            crm.Users.InsertOnSubmit(newcustomer);
                                            crm.SubmitChanges();

                                            userevent.CalisanId = 7;
                                            userevent.EventTypeId = 5;
                                            userevent.UserId = newcustomer.UserID;


                                            crm.UserEvents.InsertOnSubmit(userevent);
                                            crm.SubmitChanges();




                                            var text = "OYAK AUTO CREATE- Yeni Kullanıcı Açma  ; açilan user name  = " + newcustomer.UserName + " ;  Remeto Ip =" + userevent.IP;
                                            formListeTransaction(text);
                                            CalisanlaraKomutGonder("OYAK_AUTO_CREATE|" + text);
                                            SendToSSO(newcustomer.UserID);




                                            responsestr = komut + "|" + "OK" + "|" + DateTime.Now.ToString("HH:mm:ss");
                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;

                                            // formServerCustomers.SendClientToAnaServer(idealusername);
                                            return;


                                            #endregion

                                        }



                                    }



                                    #endregion

                                }
                            }
                            responsestr = "FAILED";
                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                            return;
                        }
                        #endregion

                        #region POST

                        if (inputmsg.Substring(0, 4) == "POST")
                        {
                            var resp = Latin5.GetString(e.TextB).Trim();
                            // var test = GetPath(resp);
                            var message = resp.TurkceHttpResponse();
                            message = WebUtility.UrlDecode(message);
                            var parsearr = message.Trim().Split('\n');
                            var gecerlimesaj = "";

                            for (int i = 0; i < parsearr.Length; i++)
                            {
                                if (parsearr[i].StartsWith("CRMAPI"))
                                    gecerlimesaj = parsearr[i];
                                if (parsearr[i].StartsWith("INFOANALIZ"))
                                    gecerlimesaj = parsearr[i];
                            }

                            if (gecerlimesaj == "")
                            {
                                responsestr = "INVALID";
                                IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                IpDeamon.Connections[e.ConnectionId].Connected = false;
                                return;
                            }
                            var fieldarray = gecerlimesaj.Split('&');

                            #region CRMAPI
                            if (fieldarray[0] == "CRMAPI")
                            {

                                switch (fieldarray[1])
                                {
                                    #region CREATEUSER
                                    case "CREATEUSER":
                                        {
                                            #region Degiskenler
                                            var username = "";
                                            var password = "";
                                            var hesapNo = "";
                                            var hesapPassword = "";
                                            var expiredate = "";
                                            var ulke = "";
                                            var Adres = "";
                                            var GSM = "";
                                            var EMAIL = "";
                                            var sehir = "";
                                            var aciklama = "";
                                            var ad = "";
                                            var soyad = "";
                                            var tcno = "";
                                            var pro = false;
                                            var cep = false;

                                            //pay
                                            var pd1 = false;
                                            var pd1p = false;
                                            var pd2 = false;
                                            var pd2p = false;
                                            var end = false;
                                            var pit = false;
                                            var pite = false;
                                            var anpro = false;

                                            var pd1Sd = "";
                                            var pd1pSd = "";
                                            var pd2Sd = "";
                                            var pd2pSd = "";
                                            var endSd = "";
                                            var pitSd = "";
                                            var piteSd = "";
                                            var anproSd = "";

                                            var pd1Ed = "";
                                            var pd1pEd = "";
                                            var pd2Ed = "";
                                            var pd2pEd = "";
                                            var endEd = "";
                                            var pitEd = "";
                                            var piteEd = "";
                                            var anproEd = "";

                                            //viop
                                            var vd1 = false;
                                            var vd1p = false;
                                            var vd2 = false;
                                            var vd2p = false;
                                            var vit = false;

                                            var vd1Sd = "";
                                            var vd1pSd = "";
                                            var vd2Sd = "";
                                            var vd2pSd = "";
                                            var vitSd = "";

                                            var vd1Ed = "";
                                            var vd1pEd = "";
                                            var vd2Ed = "";
                                            var vd2pEd = "";
                                            var vitEd = "";

                                            var krmd1 = false;
                                            var krmd1Sd = "";
                                            var krmd1Ed = "";

                                            var mkk = false;
                                            var mkkSd = "";
                                            var mkkEd = "";

                                            var tarama = false;
                                            var taramaSd = "";
                                            var taramaEd = "";

                                            var gkkul = false;
                                            var gkkulSd = "";
                                            var gkkulEd = "";

                                            var cme = false;
                                            var cmeSd = "";
                                            var cmeEd = "";

                                            //tahvil
                                            var bd1 = false;
                                            var bd1p = false;
                                            var bd2 = false;


                                            var sentiL1 = false;
                                            var sentiL2 = false;

                                            var bd1Sd = "";
                                            var bd1pSd = "";
                                            var bd2Sd = "";


                                            var sentiL1Sd = "";
                                            var sentiL2Sd = "";
                                            var proSd = "";
                                            var cepSd = "";

                                            var bd1Ed = "";
                                            var bd1pEd = "";
                                            var bd2Ed = "";


                                            var sentiL1Ed = "";
                                            var sentiL2Ed = "";
                                            var proEd = "";
                                            var cepEd = "";
                                            var yds = false;
                                            #endregion
                                            #region Parse
                                            var karmaGeldi = false;
                                            var Pd1Geldi = false;
                                            var Pd1pGeldi = false;
                                            var Pd2Geldi = false;
                                            var Pd2PGeldi = false;
                                            var Vd1Geldi = false;
                                            var Vd1pGeldi = false;
                                            var Vd2Geldi = false;
                                            var Vd2PGeldi = false;
                                            var anproGeldi = false;
                                            var mkkGeldi = false;
                                            var taramaGeldi = false;
                                            var gkkulGeldi = false;
                                            var cmeGeldi = false;

                                            for (int i = 2; i < fieldarray.Length; i++)
                                            {
                                                var splitarray = fieldarray[i].Split('=');
                                                switch (splitarray[0].Trim())
                                                {
                                                    case "USERNAME": username = splitarray[1].Trim(); break;
                                                    case "PASSWORD": password = splitarray[1].Trim(); break;
                                                    case "HESAPNO": hesapNo = splitarray[1].Trim(); break;
                                                    case "HESAPPASSWORD": hesapPassword = splitarray[1].Trim(); break;
                                                    case "EXPIREDATE": expiredate = splitarray[1].Trim(); break;
                                                    case "PRO": pro = splitarray[1].Trim()._ToBool(); break;
                                                    case "CEP": cep = splitarray[1].Trim()._ToBool(); break;
                                                    case "ULKE": ulke = splitarray[1].Trim(); break;
                                                    case "ADRES": Adres = splitarray[1].Trim(); break;
                                                    case "GSM": GSM = splitarray[1].Trim(); break;
                                                    case "EMAIL": EMAIL = splitarray[1].Trim(); break;
                                                    case "SEHIR": sehir = splitarray[1].Trim(); break;
                                                    case "ACIKLAMA": aciklama = splitarray[1].Trim(); break;
                                                    case "AD": ad = splitarray[1].Trim(); break;
                                                    case "SOYAD": soyad = splitarray[1].Trim(); break;
                                                    case "TCNO": tcno = splitarray[1].Trim(); break;
                                                    case "PD1": pd1 = splitarray[1].Trim()._ToBool(); Pd1Geldi = true; break;
                                                    case "PD1P": pd1p = splitarray[1].Trim()._ToBool(); Pd1pGeldi = true; break;
                                                    case "PD2": pd2 = splitarray[1].Trim()._ToBool(); Pd2Geldi = true; break;
                                                    case "PD2P": pd2p = splitarray[1].Trim()._ToBool(); Pd2PGeldi = true; break;
                                                    case "END": end = splitarray[1].Trim()._ToBool(); break;
                                                    case "PIT": pit = splitarray[1].Trim()._ToBool(); break;
                                                    case "PITE": pite = splitarray[1].Trim()._ToBool(); break;
                                                    case "VD1": vd1 = splitarray[1].Trim()._ToBool(); Vd1Geldi = true; break;
                                                    case "VD1P": vd1p = splitarray[1].Trim()._ToBool(); Vd1pGeldi = true; break;
                                                    case "VD2": vd2 = splitarray[1].Trim()._ToBool(); Vd2Geldi = true; break;
                                                    case "VD2P": vd2p = splitarray[1].Trim()._ToBool(); Vd2PGeldi = true; break;
                                                    case "VIT": vit = splitarray[1].Trim()._ToBool(); break;
                                                    case "BD1": bd1 = splitarray[1].Trim()._ToBool(); break;
                                                    case "BD1P": bd1p = splitarray[1].Trim()._ToBool(); break;
                                                    case "BD2": bd2 = splitarray[1].Trim()._ToBool(); break;
                                                    case "ANALİZPRO": anpro = splitarray[1].Trim()._ToBool(); break;
                                                    case "SentiL1": sentiL1 = splitarray[1].Trim()._ToBool(); break;
                                                    case "SentiL2": sentiL2 = splitarray[1].Trim()._ToBool(); break;
                                                    case "YDS": yds = splitarray[1].Trim()._ToBool(); break;
                                                    case "KRMD1": krmd1 = splitarray[1].Trim()._ToBool(); karmaGeldi = true; break;
                                                    case "MKK": mkk = splitarray[1].Trim()._ToBool(); mkkGeldi = true; break;
                                                    case "TARAMA": tarama = splitarray[1].Trim()._ToBool(); taramaGeldi = true; break;
                                                    case "GKKUL": gkkul = splitarray[1].Trim()._ToBool(); gkkulGeldi = true; break;
                                                    case "CME": cme = splitarray[1].Trim()._ToBool(); cmeGeldi = true; break;
                                                    case "PD1SD": pd1Sd = splitarray[1].Trim(); break;
                                                    case "PD1PSD": pd1pSd = splitarray[1].Trim(); break;
                                                    case "PD2SD": pd2Sd = splitarray[1].Trim(); break;
                                                    case "PD2PSD": pd2pSd = splitarray[1].Trim(); break;
                                                    case "ENDSD": endSd = splitarray[1].Trim(); break;
                                                    case "PITSD": pitSd = splitarray[1].Trim(); break;
                                                    case "PITESD": piteSd = splitarray[1].Trim(); break;
                                                    case "VD1SD": vd1Sd = splitarray[1].Trim(); break;
                                                    case "VD1PSD": vd1pSd = splitarray[1].Trim(); break;
                                                    case "VD2SD": vd2Sd = splitarray[1].Trim(); break;
                                                    case "VD2PSD": vd2pSd = splitarray[1].Trim(); break;
                                                    case "VITSD": vitSd = splitarray[1].Trim(); break;
                                                    case "BD1SD": bd1Sd = splitarray[1].Trim(); break;
                                                    case "BD1PSD": bd1pSd = splitarray[1].Trim(); break;
                                                    case "BD2SD": bd2Sd = splitarray[1].Trim(); break;
                                                    case "SENTIL1SD": sentiL1Sd = splitarray[1].Trim(); break;
                                                    case "SENTIL2SD": sentiL2Sd = splitarray[1].Trim(); break;
                                                    case "PROSD": proSd = splitarray[1].Trim(); break;
                                                    case "CEPSD": cepSd = splitarray[1].Trim(); break;
                                                    case "KRMD1SD": krmd1Sd = splitarray[1].Trim(); break;
                                                    case "MKKSD": mkkSd = splitarray[1].Trim(); break;
                                                    case "TARAMASD": taramaSd = splitarray[1].Trim(); break;
                                                    case "GKKULSD": gkkulSd = splitarray[1].Trim(); break;
                                                    case "CMESD": cmeSd = splitarray[1].Trim(); break;
                                                    case "PD1ED": pd1Ed = splitarray[1].Trim(); break;
                                                    case "PD1PED": pd1pEd = splitarray[1].Trim(); break;
                                                    case "PD2ED": pd2Ed = splitarray[1].Trim(); break;
                                                    case "PD2PED": pd2pEd = splitarray[1].Trim(); break;
                                                    case "ENDED": endEd = splitarray[1].Trim(); break;
                                                    case "PITED": pitEd = splitarray[1].Trim(); break;
                                                    case "PITEED": piteEd = splitarray[1].Trim(); break;
                                                    case "VD1ED": vd1Ed = splitarray[1].Trim(); break;
                                                    case "VD1PED": vd1pEd = splitarray[1].Trim(); break;
                                                    case "VD2ED": vd2Ed = splitarray[1].Trim(); break;
                                                    case "VD2PED": vd2pEd = splitarray[1].Trim(); break;
                                                    case "VITED": vitEd = splitarray[1].Trim(); break;
                                                    case "BD1ED": bd1Ed = splitarray[1].Trim(); break;
                                                    case "BD1PED": bd1pEd = splitarray[1].Trim(); break;
                                                    case "BD2ED": bd2Ed = splitarray[1].Trim(); break;
                                                    case "KRMD1ED": krmd1Ed = splitarray[1].Trim(); break;
                                                    case "SENTIL1ED": sentiL1Ed = splitarray[1].Trim(); break;
                                                    case "SENTIL2ED": sentiL2Ed = splitarray[1].Trim(); break;
                                                    case "PROED": proEd = splitarray[1].Trim(); break;
                                                    case "CEPED": cepEd = splitarray[1].Trim(); break;
                                                    case "MKKED": mkkEd = splitarray[1].Trim(); break;
                                                    case "GKKULED": gkkulEd = splitarray[1].Trim(); break;
                                                    case "TARAMAED": taramaEd = splitarray[1].Trim(); break;
                                                    case "CMEED": cmeEd = splitarray[1].Trim(); break;
                                                }
                                            }
                                            #endregion
                                            var rndm = new Random();
                                            var sifre = "";
                                            var sonuc = new Sonuc();
                                            if (expiredate == "")
                                            {
                                                sonuc.Kod = "102";
                                                sonuc.Aciklama = "Expiredate boş olamaz.";
                                                sonuc.Status = "Eksik Bilgi";
                                                var json = new JavaScriptSerializer().Serialize(sonuc);
                                                IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                return;
                                            }
                                            var calPassword = MyTools.Sifreleme.Encryp(password);
                                            var cal = crm.Calisans.FirstOrDefault(x => x.UserName == username && x.Password == calPassword);

                                            if (crm.Calisans.Where(x => x.UserName == username && x.Password == calPassword).Any())
                                            {

                                                if (crm.Users.Where(x => x.UserName == hesapNo).Any() == false)
                                                {
                                                    if (ad.Length >= 1 && soyad.Length >= 1) sifre = ad.Substring(0, 1).ToUpper() + soyad.Substring(0, 1).ToLower();
                                                    else sifre = "Id";
                                                    for (int i = 0; i < 5; i++)
                                                    {
                                                        sifre += rndm.Next(0, 10).ToString();
                                                    }
                                                    if (username._ToEngUp().StartsWith("APIOPTIMUS") || username._ToEngUp().StartsWith("OPTIMUS") || username._ToEngUp().StartsWith("APIMEKSA"))
                                                    {
                                                        sifre = hesapPassword;
                                                    }
                                                    userevent.Calisan = cal;
                                                    userevent.EventTypeId = 5;
                                                    var user = new User();

                                                    user.Password = MyTools.Sifreleme.Encryp(sifre);
                                                    user.PmtsNo = cal.kurumMutserino;
                                                    user.UserName = hesapNo;
                                                    user.BaslangicTarihi = DateTime.Now;
                                                    user.ExpiryDate = expiredate.StrinToDateTime();
                                                    user.Name = ad.ToUpper();
                                                    user.Surname = soyad.ToUpper();
                                                    user.tckno = tcno;
                                                    var tmpAdres = Adres._ToEngUp();
                                                    var iletisim = new Iletisim();
                                                    iletisim.acikadres = Adres;
                                                    var il = MyTools.SehirIdBul(sehir);
                                                    if (il != null && il.Id > 0)
                                                        iletisim.IlId = il.Id;

                                                    var ulkeobje = MyTools.UlkeIdBul(ulke);
                                                    if (ulkeobje != null && ulkeobje.Id > 0)
                                                        iletisim.UlkeId = ulkeobje.Id;

                                                    iletisim.Ceptel = GSM;
                                                    iletisim.email = EMAIL;

                                                    crm.Iletisims.InsertOnSubmit(iletisim);
                                                    crm.SubmitChanges();
                                                    user.iletisimId = iletisim.IletisimId;
                                                    var LisansDurum = new LisansDurum();

                                                    if (iletisim.UlkeId == 213)
                                                        user.MusteriMenseiID = 1;
                                                    else
                                                        user.MusteriMenseiID = 2;

                                                    LisansDurum.Futgck = true;
                                                    LisansDurum.WINX = true;
                                                    user.ProductType = "IDEAL";
                                                    user.BaslangicTarihi = DateTime.Now;
                                                    if (BuAyGelecekAyKontrolu(cepSd.StrinToDateTime()) == true || BuAyGelecekAyKontrolu(proSd.StrinToDateTime()) == true)
                                                    {
                                                        LisansDurum.YayinDurumu = true;
                                                    }
                                                    user.StatusId = 1;

                                                    #region Gelen_Lisanlari_isle
                                                    if (MyTools.KurumKod == "14999") //öfk
                                                    {
                                                        tarama = true;
                                                        taramaSd = user.BaslangicTarihi.ToString();
                                                    }
                                                    if (cep == true || (cepSd != "" && cepEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(cepSd.StrinToDateTime()))
                                                            LisansDurum.CepYetki = true;
                                                        LisansDurum.CepYetkiStart = cepSd.StrinToDateTime();
                                                        LisansDurum.CepYetkiEnd = cepEd.StrinToDateTime();
                                                    }
                                                    else if (cep == true && (cepSd == "" || cepEd == ""))
                                                    {
                                                        sonuc.Kod = "102";
                                                        sonuc.Aciklama = "Cep lisansı StartDate/EndDate boş olamaz.";
                                                        sonuc.Status = "Eksik Bilgi";
                                                        var jsonn = new JavaScriptSerializer().Serialize(sonuc);
                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(jsonn._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }
                                                    if (pro == true || (proSd != "" && proEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(proSd.StrinToDateTime()))
                                                            LisansDurum.ProYetki = true;
                                                        LisansDurum.ProYetkiStart = proSd.StrinToDateTime();
                                                        LisansDurum.ProYetkiEnd = proEd.StrinToDateTime();
                                                    }
                                                    else if (pro == true && (proSd == "" || proEd == ""))
                                                    {
                                                        sonuc.Kod = "102";
                                                        sonuc.Aciklama = "Pro lisansı StartDate/EndDate boş olamaz.";
                                                        sonuc.Status = "Eksik Bilgi";
                                                        var json2 = new JavaScriptSerializer().Serialize(sonuc);
                                                        IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json2._InsertHeaderHTTP());
                                                        IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                        return;
                                                    }


                                                    if (pd1 || (pd1Sd != "" && pd1Ed != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(pd1Sd.StrinToDateTime()))
                                                            LisansDurum.PayL1 = true;
                                                        LisansDurum.PayL1Start = pd1Sd.StrinToDateTime();
                                                        LisansDurum.PayL1End = pd1Ed.StrinToDateTime();
                                                    }
                                                    if (pd1p || (pd1pSd != "" && pd1pEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(pd1pSd.StrinToDateTime()))
                                                        {
                                                            LisansDurum.PayLP = true;
                                                            LisansDurum.PayL1 = true;
                                                        }
                                                        LisansDurum.PayLPStart = pd1pSd.StrinToDateTime();
                                                        LisansDurum.PayLPEnd = pd1pEd.StrinToDateTime();
                                                        LisansDurum.PayL1Start = pd1pSd.StrinToDateTime();
                                                        LisansDurum.PayL1End = pd1pEd.StrinToDateTime();
                                                    }
                                                    if (pd2 || (pd2Sd != "" && pd2Ed != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(pd2Sd.StrinToDateTime()))
                                                        {
                                                            LisansDurum.PayL2 = true;
                                                            LisansDurum.PayLP = true;
                                                            LisansDurum.PayL1 = true;
                                                        }
                                                        LisansDurum.PayL2Start = pd2Sd.StrinToDateTime();
                                                        LisansDurum.PayL2End = pd2Ed.StrinToDateTime();
                                                        LisansDurum.PayLPStart = pd2Sd.StrinToDateTime();
                                                        LisansDurum.PayLPEnd = pd2Ed.StrinToDateTime();
                                                        LisansDurum.PayL1Start = pd2Sd.StrinToDateTime();
                                                        LisansDurum.PayL1End = pd2Ed.StrinToDateTime();
                                                    }

                                                    if (pd2p || (pd2pSd != "" && pd2pEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(pd2Sd.StrinToDateTime()))
                                                        {
                                                            LisansDurum.Pd2P = true;
                                                            LisansDurum.PayL2 = true;
                                                            LisansDurum.PayLP = true;
                                                            LisansDurum.PayL1 = true;
                                                        }
                                                        LisansDurum.Pd2PStart = pd2pSd.StrinToDateTime();
                                                        LisansDurum.Pd2PEnd = pd2pEd.StrinToDateTime();
                                                        LisansDurum.PayL2Start = pd2Sd.StrinToDateTime();
                                                        LisansDurum.PayL2End = pd2Ed.StrinToDateTime();
                                                        LisansDurum.PayLPStart = pd2Sd.StrinToDateTime();
                                                        LisansDurum.PayLPEnd = pd2Ed.StrinToDateTime();
                                                        LisansDurum.PayL1Start = pd2Sd.StrinToDateTime();
                                                        LisansDurum.PayL1End = pd2Ed.StrinToDateTime();
                                                    }
                                                    if (end || (endSd != "" && endEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(endSd.StrinToDateTime()))
                                                            LisansDurum.PayX = true;
                                                        LisansDurum.PayXStart = endSd.StrinToDateTime();
                                                        LisansDurum.PayXEnd = endEd.StrinToDateTime();
                                                    }
                                                    if (pit || (pitSd != "" && pitEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(pitSd.StrinToDateTime()))
                                                            LisansDurum.PayGS = true;
                                                        LisansDurum.PayGSStart = pitSd.StrinToDateTime();
                                                        LisansDurum.PayGSEnd = pitEd.StrinToDateTime();
                                                    }
                                                    if (pite || (piteSd != "" && piteEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(piteSd.StrinToDateTime()))
                                                            LisansDurum.PITE = true;
                                                        LisansDurum.PayPiteStart = piteSd.StrinToDateTime();
                                                        LisansDurum.PayPiteEnd = piteEd.StrinToDateTime();
                                                    }

                                                    if (vd1 || (vd1Sd != "" && vd1Ed != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(vd1Sd.StrinToDateTime()))
                                                            LisansDurum.ViopL1 = true;
                                                        LisansDurum.ViopL1Start = vd1Sd.StrinToDateTime();
                                                        LisansDurum.ViopL1End = vd1Ed.StrinToDateTime();
                                                    }
                                                    if (vd1p || (vd1pSd != "" && vd1pEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(vd1pSd.StrinToDateTime()))
                                                        {
                                                            LisansDurum.ViopL1 = true;
                                                            LisansDurum.ViopLP = true;
                                                        }
                                                        LisansDurum.ViopLPStart = vd1pSd.StrinToDateTime();
                                                        LisansDurum.ViopLPEnd = vd1pEd.StrinToDateTime();
                                                        LisansDurum.ViopL1Start = vd1pSd.StrinToDateTime();
                                                        LisansDurum.ViopL1End = vd1pEd.StrinToDateTime();
                                                    }
                                                    if (vd2 || (vd2Sd != "" && vd2Ed != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(vd2Sd.StrinToDateTime()))
                                                        {
                                                            LisansDurum.ViopL2 = true;
                                                            LisansDurum.ViopL1 = true;
                                                            LisansDurum.ViopLP = true;
                                                        }
                                                        LisansDurum.ViopL2Start = vd2Sd.StrinToDateTime();
                                                        LisansDurum.ViopL2End = vd2Ed.StrinToDateTime();
                                                        LisansDurum.ViopLPStart = vd2Sd.StrinToDateTime();
                                                        LisansDurum.ViopLPEnd = vd2Ed.StrinToDateTime();
                                                        LisansDurum.ViopL1Start = vd2Sd.StrinToDateTime();
                                                        LisansDurum.ViopL1End = vd2Ed.StrinToDateTime();
                                                    }

                                                    if (vd2p || (vd2pSd != "" && vd2pEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(vd2Sd.StrinToDateTime()))
                                                        {
                                                            LisansDurum.Vd2P = true;
                                                            LisansDurum.ViopL2 = true;
                                                            LisansDurum.ViopL1 = true;
                                                            LisansDurum.ViopLP = true;
                                                        }
                                                        LisansDurum.Vd2PStart = vd2pSd.StrinToDateTime();
                                                        LisansDurum.Vd2PEnd = vd2pEd.StrinToDateTime();
                                                        LisansDurum.ViopL2Start = vd2Sd.StrinToDateTime();
                                                        LisansDurum.ViopL2End = vd2Ed.StrinToDateTime();
                                                        LisansDurum.ViopLPStart = vd2Sd.StrinToDateTime();
                                                        LisansDurum.ViopLPEnd = vd2Ed.StrinToDateTime();
                                                        LisansDurum.ViopL1Start = vd2Sd.StrinToDateTime();
                                                        LisansDurum.ViopL1End = vd2Ed.StrinToDateTime();
                                                    }

                                                    if (vit || (vitSd != "" && vitEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(vitSd.StrinToDateTime()))
                                                            LisansDurum.ViopGS = true;
                                                        LisansDurum.ViopGSStart = vitSd.StrinToDateTime();
                                                        LisansDurum.ViopGSEnd = vitEd.StrinToDateTime();
                                                    }
                                                    if (krmd1 || (krmd1Sd != "" && krmd1Ed != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(krmd1Sd.StrinToDateTime()))
                                                            LisansDurum.COMEX = true;
                                                        LisansDurum.KRMD1Start = krmd1Sd.StrinToDateTime();
                                                        LisansDurum.KRMD1End = krmd1Ed.StrinToDateTime();
                                                    }

                                                    if (mkk || (mkkSd != "" && mkkEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(mkkSd.StrinToDateTime()))
                                                            LisansDurum.MKK = true;
                                                        LisansDurum.MKKStart = mkkSd.StrinToDateTime();
                                                        LisansDurum.MKKEnd = mkkEd.StrinToDateTime();
                                                    }
                                                    if (gkkul || (gkkulSd != "" && gkkulEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(gkkulSd.StrinToDateTime()))
                                                            LisansDurum.GKKUL = true;
                                                        LisansDurum.GKKULStart = gkkulSd.StrinToDateTime();
                                                        LisansDurum.GKKULEnd = gkkulSd.StrinToDateTime();
                                                    }
                                                    if (tarama || (taramaSd != "" && taramaEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(taramaSd.StrinToDateTime()))
                                                            LisansDurum.TARAMA = true;
                                                        LisansDurum.TaramaStart = taramaSd.StrinToDateTime();
                                                        LisansDurum.TaramaEnd = taramaEd.StrinToDateTime();
                                                    }

                                                    if (cme || (cmeSd != "" && cmeEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(cmeSd.StrinToDateTime()))
                                                            LisansDurum.CME = true;
                                                        LisansDurum.CMEStart = cmeSd.StrinToDateTime();
                                                        LisansDurum.CMEEnd = cmeEd.StrinToDateTime();
                                                    }

                                                    if (bd1 || (bd1Sd != "" && bd1Ed != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(bd1Sd.StrinToDateTime()))
                                                            LisansDurum.TahvilL1 = true;
                                                        LisansDurum.TahvilL1Start = bd1Sd.StrinToDateTime();
                                                        LisansDurum.TahvilL1End = bd1Ed.StrinToDateTime();
                                                    }
                                                    if (bd1p || (bd1pSd != "" && bd1pEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(bd1pSd.StrinToDateTime()))
                                                        {
                                                            LisansDurum.TahvilLP = true;
                                                            LisansDurum.TahvilL1 = true;
                                                        }
                                                        LisansDurum.TahvilLPStart = bd1pSd.StrinToDateTime();
                                                        LisansDurum.TahvilLPEnd = bd1pEd.StrinToDateTime();
                                                        LisansDurum.TahvilL1Start = bd1pSd.StrinToDateTime();
                                                        LisansDurum.TahvilL1End = bd1pEd.StrinToDateTime();
                                                    }
                                                    if (bd2 || (bd2Sd != "" && bd2Ed != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(bd2Sd.StrinToDateTime()))
                                                        {
                                                            LisansDurum.TahvilLP = true;
                                                            LisansDurum.TahvilL1 = true;
                                                            LisansDurum.TahvilL2 = true;
                                                        }
                                                        LisansDurum.TahvilL2Start = bd2Sd.StrinToDateTime();
                                                        LisansDurum.TahvilL2End = bd2Ed.StrinToDateTime();
                                                        LisansDurum.TahvilLPStart = bd2Sd.StrinToDateTime();
                                                        LisansDurum.TahvilLPEnd = bd2Ed.StrinToDateTime();
                                                        LisansDurum.TahvilL1Start = bd2Sd.StrinToDateTime();
                                                        LisansDurum.TahvilL1End = bd2Ed.StrinToDateTime();
                                                    }
                                                    if (anpro || (anproSd != "" && anproEd != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(anproSd.StrinToDateTime()))
                                                            LisansDurum.AnPro = true;
                                                        LisansDurum.AnProStart = anproSd.StrinToDateTime();
                                                        LisansDurum.AnProEnd = anproEd.StrinToDateTime();
                                                    }

                                                    if (sentiL1 || (sentiL1Sd != "" && sentiL1Ed != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(sentiL1Sd.StrinToDateTime()))
                                                            LisansDurum.SentiL1 = true;
                                                        LisansDurum.SentiL1Start = sentiL1Sd.StrinToDateTime();
                                                        LisansDurum.SentiL1End = sentiL1Ed.StrinToDateTime();
                                                    }
                                                    if (yds)
                                                    {
                                                        LisansDurum.SPI = true;
                                                    }
                                                    if (sentiL2 || (sentiL2Sd != "" && sentiL2Ed != ""))
                                                    {
                                                        if (BuAyGelecekAyKontrolu(sentiL2Sd.StrinToDateTime()))
                                                        {
                                                            LisansDurum.SentiL1 = true;
                                                            LisansDurum.SentiL2 = true;
                                                        }
                                                        LisansDurum.SentiL2Start = sentiL2Sd.StrinToDateTime();
                                                        LisansDurum.SentiL2End = sentiL2Ed.StrinToDateTime();
                                                        LisansDurum.SentiL1Start = sentiL1Sd.StrinToDateTime();
                                                        LisansDurum.SentiL1End = sentiL1Ed.StrinToDateTime();
                                                    }

                                                    //test
                                                    #endregion
                                                    #region lisansKontrol
                                                    if (LisansDurum.COMEX)
                                                    {
                                                        if (LisansDurum.Pd2P == false && LisansDurum.PayL2 == false && LisansDurum.PayLP == false)
                                                        {

                                                            if (LisansDurum.PayL1 == true)
                                                            {
                                                                LisansDurum.PayL1 = false;
                                                                LisansDurum.PayL1End = null;
                                                                LisansDurum.PayL1Start = null;

                                                            }
                                                        }

                                                        if (LisansDurum.ViopLP == false && LisansDurum.ViopL2 == false && LisansDurum.Vd2P == false)
                                                        {

                                                            if (LisansDurum.ViopL1 == true)
                                                            {
                                                                LisansDurum.ViopL1 = false;
                                                                LisansDurum.ViopL1End = null;
                                                                LisansDurum.ViopL1Start = null;

                                                            }
                                                        }

                                                    }
                                                    if (karmaGeldi)
                                                    {
                                                        if (Pd1Geldi == true && Pd1pGeldi == false && Pd2Geldi == false && Pd2PGeldi == false)
                                                        {
                                                            LisansDurum.PayL1 = false;
                                                            LisansDurum.PayL1End = null;
                                                            LisansDurum.PayL1Start = null;
                                                        }

                                                        if (Vd1Geldi == true && Vd1pGeldi == false && Vd2Geldi == false && Vd2PGeldi == false)
                                                        {

                                                            LisansDurum.ViopL1 = false;
                                                            LisansDurum.ViopL1End = null;
                                                            LisansDurum.ViopL1Start = null;
                                                        }

                                                    }
                                                    if (LisansDurum.PITE)
                                                    {

                                                        if (LisansDurum.PayPiteStart.Value.Date > new DateTime(2020, 12, 1))
                                                        {
                                                            LisansDurum.PayGS = false;
                                                            LisansDurum.PayGSStart = null;
                                                            LisansDurum.PayGSEnd = null;
                                                        }
                                                    }

                                                    if ((LisansDurum.ProYetki == false && LisansDurum.CepYetki == false) && ((LisansDurum.CepYetkiStart != null && LisansDurum.CepYetkiStart.Value.Date >= DateTime.Now.Date) || (LisansDurum.CepYetkiStart != null && LisansDurum.ProYetkiStart.Value.Date >= DateTime.Now.Date)))
                                                    {
                                                        LisansDurum.YayinDurumu = false;
                                                    }
                                                    #endregion
                                                    crm.LisansDurums.InsertOnSubmit(LisansDurum);
                                                    crm.SubmitChanges();

                                                    user.LisansDurum = LisansDurum;
                                                    userevent.LisansDurum = LisansDurum;

                                                    crm.Users.InsertOnSubmit(user);
                                                    crm.SubmitChanges();

                                                    userevent.UserId = user.UserID;

                                                    if (userevent.EventId == 0)
                                                        crm.UserEvents.InsertOnSubmit(userevent);
                                                    crm.SubmitChanges();


                                                    var extra = new UserExtraInfo();
                                                    crm.UserExtraInfos.InsertOnSubmit(extra);
                                                    crm.SubmitChanges();
                                                    user.UserExtraInfoId = extra.id;

                                                    sonuc.Kod = "100";
                                                    sonuc.Aciklama = "Kullanıcı Oluşturuldu.";
                                                    sonuc.Status = "Başarılı";
                                                    var json = new JavaScriptSerializer().Serialize(sonuc);

                                                    var text = "Yeni Kullanıcı Açma  ;açilan user name  = " + user.UserName + ";  Remeto Ip =" + userevent.IP;
                                                    formListeTransaction(text);
                                                    CalisanlaraKomutGonder("_CreateUser|" + text);
                                                    SendToSSO(user.UserID);

                                                }
                                                else
                                                {
                                                    sonuc.Kod = "101";
                                                    sonuc.Aciklama = "Kullanıcı Mevcut.";
                                                    sonuc.Status = "Başarısız";
                                                }

                                            }
                                            else
                                            {
                                                sonuc.Kod = "101";
                                                sonuc.Aciklama = "Çalışan Bulunamadı.";
                                                sonuc.Status = "Başarısız";
                                            }
                                            var jsn = new JavaScriptSerializer().Serialize(sonuc);
                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(jsn._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }
                                    #endregion
                                    #region UPDATEUSER
                                    case "UPDATEUSER":
                                        {

                                            #region Degiskenler
                                            var username = "";
                                            var password = "";
                                            var hesapNo = "";
                                            var hesapPassword = "";
                                            var expiredate = "";
                                            var ulke = "";
                                            var Adres = "";
                                            var GSM = "";
                                            var EMAIL = "";
                                            var sehir = "";
                                            var aciklama = "";
                                            var ad = "";
                                            var soyad = "";
                                            var tcno = "";
                                            var pro = false;
                                            var cep = false;

                                            //pay
                                            var pd1 = false;
                                            var pd1p = false;
                                            var pd2 = false;
                                            var pd2p = false;
                                            var end = false;
                                            var pit = false;
                                            var pite = false;

                                            var pd1Sd = "";
                                            var pd1pSd = "";
                                            var pd2Sd = "";
                                            var pd2pSd = "";
                                            var endSd = "";
                                            var pitSd = "";
                                            var piteSd = "";

                                            var pd1Ed = "";
                                            var pd1pEd = "";
                                            var pd2Ed = "";
                                            var pd2pEd = "";
                                            var endEd = "";
                                            var pitEd = "";
                                            var piteEd = "";

                                            //viop
                                            var vd1 = false;
                                            var vd1p = false;
                                            var vd2 = false;
                                            var vd2p = false;
                                            var vit = false;

                                            var vd1Sd = "";
                                            var vd1pSd = "";
                                            var vd2Sd = "";
                                            var vd2pSd = "";
                                            var vitSd = "";

                                            var vd1Ed = "";
                                            var vd1pEd = "";
                                            var vd2Ed = "";
                                            var vd2pEd = "";
                                            var vitEd = "";

                                            var krmd1 = false;
                                            var krmd1Sd = "";
                                            var krmd1Ed = "";

                                            var mkk = false;
                                            var mkkSd = "";
                                            var mkkEd = "";

                                            var tarama = false;
                                            var taramaSd = "";
                                            var taramaEd = "";

                                            var gkkul = false;
                                            var gkkulSd = "";
                                            var gkkulEd = "";

                                            var cme = false;
                                            var cmeSd = "";
                                            var cmeEd = "";

                                            //tahvil
                                            var bd1 = false;
                                            var bd1p = false;
                                            var bd2 = false;

                                            var anpro = false;


                                            var sentiL1 = false;
                                            var sentiL2 = false;

                                            var bd1Sd = "";
                                            var bd1pSd = "";
                                            var bd2Sd = "";

                                            var anproSd = "";

                                            var sentiL1Sd = "";
                                            var sentiL2Sd = "";
                                            var proSd = "";
                                            var cepSd = "";

                                            var bd1Ed = "";
                                            var bd1pEd = "";
                                            var bd2Ed = "";
                                            var anproEd = "";
                                            var sentiL1Ed = "";
                                            var sentiL2Ed = "";
                                            var proEd = "";
                                            var cepEd = "";
                                            var yds = false;
                                            #endregion

                                            #region Parse
                                            var karmaGeldi = false;
                                            var Pd1Geldi = false;
                                            var Pd1pGeldi = false;
                                            var Pd2Geldi = false;
                                            var Pd2pGeldi = false;

                                            var Vd1Geldi = false;
                                            var Vd1pGeldi = false;
                                            var Vd2Geldi = false;
                                            var Vd2pGeldi = false;
                                            var mkkGeldi = false;
                                            var gkkulGeldi = false;
                                            var taramaGeldi = false;
                                            var cmeGeldi = false;
                                            var oncekiLisansDurum = false;



                                            for (int i = 2; i < fieldarray.Length; i++)
                                            {
                                                var splitarray = fieldarray[i].Split('=');
                                                switch (splitarray[0].Trim())
                                                {
                                                    case "USERNAME": username = splitarray[1].Trim(); break;
                                                    case "PASSWORD": password = splitarray[1].Trim(); break;
                                                    case "HESAPNO": hesapNo = splitarray[1].Trim(); break;
                                                    case "HESAPPASSWORD": hesapPassword = splitarray[1].Trim(); break;
                                                    case "EXPIREDATE": expiredate = splitarray[1].Trim(); break;
                                                    case "PRO": pro = splitarray[1].Trim()._ToBool(); break;
                                                    case "CEP": cep = splitarray[1].Trim()._ToBool(); break;
                                                    case "ULKE": ulke = splitarray[1].Trim(); break;
                                                    case "ADRES": Adres = splitarray[1].Trim(); break;
                                                    case "GSM": GSM = splitarray[1].Trim(); break;
                                                    case "EMAIL": EMAIL = splitarray[1].Trim(); break;
                                                    case "SEHIR": sehir = splitarray[1].Trim(); break;
                                                    case "ACIKLAMA": aciklama = splitarray[1].Trim(); break;
                                                    case "AD": ad = splitarray[1].Trim(); break;
                                                    case "SOYAD": soyad = splitarray[1].Trim(); break;
                                                    case "TCNO": tcno = splitarray[1].Trim(); break;
                                                    case "PD1": pd1 = splitarray[1].Trim()._ToBool(); Pd1Geldi = true; break;
                                                    case "PD1P": pd1p = splitarray[1].Trim()._ToBool(); Pd1pGeldi = true; break;
                                                    case "PD2": pd2 = splitarray[1].Trim()._ToBool(); Pd2Geldi = true; break;
                                                    case "PD2P": pd2p = splitarray[1].Trim()._ToBool(); Pd2pGeldi = true; break;
                                                    case "END": end = splitarray[1].Trim()._ToBool(); break;
                                                    case "PIT": pit = splitarray[1].Trim()._ToBool(); break;
                                                    case "PITE": pite = splitarray[1].Trim()._ToBool(); break;
                                                    case "VD1": vd1 = splitarray[1].Trim()._ToBool(); Vd1Geldi = true; break;
                                                    case "VD1P": vd1p = splitarray[1].Trim()._ToBool(); Vd1pGeldi = true; break;
                                                    case "VD2": vd2 = splitarray[1].Trim()._ToBool(); Vd2Geldi = true; break;
                                                    case "VD2P": vd2p = splitarray[1].Trim()._ToBool(); Vd2pGeldi = true; break;
                                                    case "VIT": vit = splitarray[1].Trim()._ToBool(); break;
                                                    case "BD1": bd1 = splitarray[1].Trim()._ToBool(); break;
                                                    case "BD1P": bd1p = splitarray[1].Trim()._ToBool(); break;
                                                    case "BD2": bd2 = splitarray[1].Trim()._ToBool(); break;
                                                    case "ANALİZPRO": anpro = splitarray[1].Trim()._ToBool(); break;
                                                    case "SENTIL1": sentiL1 = splitarray[1].Trim()._ToBool(); break;
                                                    case "SENTIL2": sentiL2 = splitarray[1].Trim()._ToBool(); break;
                                                    case "KRMD1": krmd1 = splitarray[1].Trim()._ToBool(); karmaGeldi = true; break;
                                                    case "MKK": mkk = splitarray[1].Trim()._ToBool(); mkkGeldi = true; break;
                                                    case "GKKUL": gkkul = splitarray[1].Trim()._ToBool(); gkkulGeldi = true; break;
                                                    case "TARAMA": tarama = splitarray[1].Trim()._ToBool(); taramaGeldi = true; break;
                                                    case "CME": cme = splitarray[1].Trim()._ToBool(); cmeGeldi = true; break;
                                                    case "PD1SD": pd1Sd = splitarray[1].Trim(); break;
                                                    case "PD1PSD": pd1pSd = splitarray[1].Trim(); break;
                                                    case "PD2SD": pd2Sd = splitarray[1].Trim(); break;
                                                    case "PD2PSD": pd2pSd = splitarray[1].Trim(); break;
                                                    case "ENDSD": endSd = splitarray[1].Trim(); break;
                                                    case "PITSD": pitSd = splitarray[1].Trim(); break;
                                                    case "PITESD": piteSd = splitarray[1].Trim(); break;
                                                    case "VD1SD": vd1Sd = splitarray[1].Trim(); break;
                                                    case "VD1PSD": vd1pSd = splitarray[1].Trim(); break;
                                                    case "VD2SD": vd2Sd = splitarray[1].Trim(); break;
                                                    case "VD2PSD": vd2pSd = splitarray[1].Trim(); break;
                                                    case "VITSD": vitSd = splitarray[1].Trim(); break;
                                                    case "BD1SD": bd1Sd = splitarray[1].Trim(); break;
                                                    case "BD1PSD": bd1pSd = splitarray[1].Trim(); break;
                                                    case "BD2SD": bd2Sd = splitarray[1].Trim(); break;
                                                    case "ANPROSD": anproSd = splitarray[1].Trim(); break;
                                                    case "KRMD1SD": krmd1Sd = splitarray[1].Trim(); break;
                                                    case "MKKSD": mkkSd = splitarray[1].Trim(); break;
                                                    case "GKKULSD": gkkulSd = splitarray[1].Trim(); break;
                                                    case "TARAMASD": taramaSd = splitarray[1].Trim(); break;
                                                    case "CMESD": cmeSd = splitarray[1].Trim(); break;
                                                    case "SENTIL1SD": sentiL1Sd = splitarray[1].Trim(); break;
                                                    case "SENTIL2SD": sentiL2Sd = splitarray[1].Trim(); break;
                                                    case "PROSD": proSd = splitarray[1].Trim(); break;
                                                    case "CEPSD": cepSd = splitarray[1].Trim(); break;
                                                    case "PD1ED": pd1Ed = splitarray[1].Trim(); break;
                                                    case "PD1PED": pd1pEd = splitarray[1].Trim(); break;
                                                    case "PD2ED": pd2Ed = splitarray[1].Trim(); break;
                                                    case "PD2PED": pd2pEd = splitarray[1].Trim(); break;
                                                    case "ENDED": endEd = splitarray[1].Trim(); break;
                                                    case "PITED": pitEd = splitarray[1].Trim(); break;
                                                    case "PITEED": piteEd = splitarray[1].Trim(); break;
                                                    case "VD1ED": vd1Ed = splitarray[1].Trim(); break;
                                                    case "VD1PED": vd1pEd = splitarray[1].Trim(); break;
                                                    case "VD2ED": vd2Ed = splitarray[1].Trim(); break;
                                                    case "VD2PED": vd2pEd = splitarray[1].Trim(); break;
                                                    case "VITED": vitEd = splitarray[1].Trim(); break;
                                                    case "BD1ED": bd1Ed = splitarray[1].Trim(); break;
                                                    case "BD1PED": bd1pEd = splitarray[1].Trim(); break;
                                                    case "BD2ED": bd2Ed = splitarray[1].Trim(); break;
                                                    case "ANPROED": anproEd = splitarray[1].Trim(); break;
                                                    case "KRMD1ED": krmd1Ed = splitarray[1].Trim(); break;
                                                    case "SENTIL1ED": sentiL1Ed = splitarray[1].Trim(); break;
                                                    case "SENTIL2ED": sentiL2Ed = splitarray[1].Trim(); break;
                                                    case "PROED": proEd = splitarray[1].Trim(); break;
                                                    case "CEPED": cepEd = splitarray[1].Trim(); break;
                                                    case "YDS": yds = splitarray[1].Trim()._ToBool(); break;
                                                    case "MKKED": mkkEd = splitarray[1].Trim(); break;
                                                    case "GKKULED": gkkulEd = splitarray[1].Trim(); break;
                                                    case "TARAMAED": taramaEd = splitarray[1].Trim(); break;
                                                    case "CMEED": cmeEd = splitarray[1].Trim(); break;


                                                }
                                            }
                                            #endregion

                                            var sonuc = new Sonuc();
                                            var calPassword = MyTools.Sifreleme.Encryp(password);
                                            var cal = crm.Calisans.FirstOrDefault(x => x.UserName == username && x.Password == calPassword);
                                            if (crm.Calisans.Where(x => x.UserName == username && x.Password == calPassword).Any())
                                            {

                                                if (crm.Users.Where(x => x.UserName == hesapNo).Any())
                                                {
                                                    userevent.Calisan = cal;
                                                    userevent.EventTypeId = 6;
                                                    var acilan = new StringBuilder();
                                                    var kapanan = new StringBuilder();
                                                    var user = crm.Users.FirstOrDefault(x => x.UserName == hesapNo);
                                                    oncekiLisansDurum = user.LisansDurum.YayinDurumu;

                                                    //user.PmtsNo = hesapNo;
                                                    user.PmtsNo = cal.kurumMutserino;
                                                    if (hesapPassword != "")
                                                        user.Password = MyTools.Sifreleme.Encryp(hesapPassword);

                                                    #region Isle

                                                    var lisansdurum = new LisansDurum();

                                                    var durumbool = false;

                                                    var ilet = new Iletisim();
                                                    if (user.LisansDurum.ProYetki == true && user.LisansDurum.YayinDurumu)
                                                    {
                                                        lisansdurum.ProYetki = true;
                                                        lisansdurum.ProYetkiStart = user.LisansDurum.ProYetkiStart;
                                                        lisansdurum.ProYetkiEnd = user.LisansDurum.ProYetkiEnd;
                                                    }
                                                    if (user.LisansDurum.CepYetki == true && user.LisansDurum.YayinDurumu)
                                                    {
                                                        lisansdurum.CepYetki = true;
                                                        lisansdurum.CepYetkiStart = user.LisansDurum.CepYetkiStart;
                                                        lisansdurum.CepYetkiEnd = user.LisansDurum.CepYetkiEnd;
                                                    }

                                                    if (user.LisansDurum.SentiL1 == true && user.LisansDurum.YayinDurumu)
                                                    {
                                                        lisansdurum.SentiL1 = true;
                                                        lisansdurum.SentiL1Start = user.LisansDurum.SentiL1Start;
                                                        lisansdurum.SentiL1End = user.LisansDurum.SentiL1End;
                                                    }
                                                    if (user.LisansDurum.SentiL2 == true && user.LisansDurum.YayinDurumu)
                                                    {
                                                        lisansdurum.SentiL1 = true;
                                                        lisansdurum.SentiL1Start = user.LisansDurum.SentiL1Start;
                                                        lisansdurum.SentiL1End = user.LisansDurum.SentiL1End;
                                                        lisansdurum.SentiL2 = true;
                                                        lisansdurum.SentiL2Start = user.LisansDurum.SentiL2Start;
                                                        lisansdurum.SentiL2End = user.LisansDurum.SentiL2End;
                                                    }
                                                    if (user.LisansDurum.AnPro == true && user.LisansDurum.YayinDurumu)
                                                    {
                                                        lisansdurum.AnPro = true;
                                                        lisansdurum.AnProStart = user.LisansDurum.AnProStart;
                                                        lisansdurum.AnProEnd = user.LisansDurum.AnProEnd;

                                                    }
                                                    if (user.LisansDurum.SPI == true && user.LisansDurum.YayinDurumu)
                                                    {
                                                        lisansdurum.SPI = true;
                                                    }

                                                    lisansdurum.YayinDurumu = user.LisansDurum.YayinDurumu;
                                                    var startdatepd1 = false;
                                                    var startdatepd1p = false;
                                                    var startdatepd2 = false;
                                                    var startdatepd2p = false;
                                                    var startdatevd1 = false;
                                                    var startdatevd1p = false;
                                                    var startdatevd2 = false;
                                                    var startdatebd1 = false;
                                                    var startdatebd1p = false;
                                                    var startdatesentiL1 = false;


                                                    for (int j = 2; j < fieldarray.Length; j++)
                                                    {
                                                        var splitarray = fieldarray[j].Split('=');
                                                        switch (splitarray[0].Trim())
                                                        {
                                                            #region EXPIREDATE
                                                            case "EXPIREDATE":

                                                                durumbool = true;
                                                                if (pro && BuAyGelecekAyKontrolu(proSd.StrinToDateTime()) || cep && BuAyGelecekAyKontrolu(cepSd.StrinToDateTime()))
                                                                    lisansdurum.YayinDurumu = true;
                                                                user.ExpiryDate = expiredate.StrinToDateTime();
                                                                break;
                                                            #endregion
                                                            #region PRO
                                                            case "PRO":

                                                                if (pro && BuAyGelecekAyKontrolu(proSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.ProYetki = true;
                                                                    acilan.Append(";ProYetki");
                                                                }

                                                                break;

                                                            #endregion
                                                            #region CEP
                                                            case "CEP":
                                                                if (cep && BuAyGelecekAyKontrolu(cepSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.CepYetki = true;
                                                                    acilan.Append(";CepYetki");
                                                                }
                                                                break;

                                                            #endregion
                                                            #region iletisim
                                                            case "ULKE":

                                                                var ulkeobje = MyTools.UlkeIdBul(ulke);
                                                                if (ulkeobje != null && ulkeobje.Id > 0)
                                                                {
                                                                    if (user.Iletisim != null)
                                                                    {
                                                                        user.Iletisim.UlkeId = ulkeobje.Id;
                                                                    }
                                                                    else
                                                                    {

                                                                        ilet.UlkeId = ulkeobje.Id;

                                                                        crm.Iletisims.InsertOnSubmit(ilet);
                                                                        crm.SubmitChanges();

                                                                        user.iletisimId = ilet.IletisimId;
                                                                    }
                                                                }

                                                                if (ulkeobje.Id == 213)
                                                                    user.MusteriMenseiID = 1;
                                                                else
                                                                    user.MusteriMenseiID = 2;


                                                                break;
                                                            case "ADRES":
                                                                if (user.Iletisim != null)
                                                                {
                                                                    user.Iletisim.acikadres = Adres;
                                                                }
                                                                else
                                                                {
                                                                    ilet.acikadres = Adres;
                                                                    crm.Iletisims.InsertOnSubmit(ilet);
                                                                    crm.SubmitChanges();

                                                                    user.iletisimId = ilet.IletisimId;
                                                                }

                                                                break;
                                                            case "GSM":

                                                                if (user.Iletisim != null)
                                                                {
                                                                    user.Iletisim.Ceptel = GSM;
                                                                }
                                                                else
                                                                {
                                                                    ilet.Ceptel = GSM;
                                                                    crm.Iletisims.InsertOnSubmit(ilet);
                                                                    crm.SubmitChanges();

                                                                    user.iletisimId = ilet.IletisimId;
                                                                }

                                                                break;
                                                            case "EMAIL":

                                                                if (user.Iletisim != null)
                                                                {
                                                                    user.Iletisim.email = EMAIL;
                                                                }
                                                                else
                                                                {
                                                                    ilet.email = EMAIL;
                                                                    crm.Iletisims.InsertOnSubmit(ilet);
                                                                    crm.SubmitChanges();

                                                                    user.iletisimId = ilet.IletisimId;
                                                                }

                                                                break;
                                                            case "SEHIR":
                                                                if (sehir == "Kahramanmaraş")
                                                                {
                                                                    sehir = "K.Maraş";
                                                                    var il = MyTools.SehirIdBul(sehir);
                                                                    if (il != null && il.Id > 0)
                                                                    {
                                                                        if (user.Iletisim != null)
                                                                        {
                                                                            user.Iletisim.IlId = il.Id;
                                                                        }
                                                                        else
                                                                        {
                                                                            ilet.IlId = il.Id;

                                                                            crm.Iletisims.InsertOnSubmit(ilet);
                                                                            crm.SubmitChanges();

                                                                            user.iletisimId = ilet.IletisimId;
                                                                        }
                                                                    }
                                                                }
                                                                else if (sehir == "Şanlıurfa")
                                                                {
                                                                    sehir = "Ş.Urfa";
                                                                    var il = MyTools.SehirIdBul(sehir);
                                                                    if (il != null && il.Id > 0)
                                                                    {
                                                                        if (user.Iletisim != null)
                                                                        {
                                                                            user.Iletisim.IlId = il.Id;
                                                                        }
                                                                        else
                                                                        {
                                                                            ilet.IlId = il.Id;

                                                                            crm.Iletisims.InsertOnSubmit(ilet);
                                                                            crm.SubmitChanges();

                                                                            user.iletisimId = ilet.IletisimId;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    var il = MyTools.SehirIdBul(sehir);
                                                                    if (il != null && il.Id > 0)
                                                                    {
                                                                        if (user.Iletisim != null)
                                                                        {
                                                                            user.Iletisim.IlId = il.Id;
                                                                        }
                                                                        else
                                                                        {
                                                                            ilet.IlId = il.Id;

                                                                            crm.Iletisims.InsertOnSubmit(ilet);
                                                                            crm.SubmitChanges();

                                                                            user.iletisimId = ilet.IletisimId;
                                                                        }
                                                                    }
                                                                }
                                                                break;
                                                            #endregion
                                                            #region ACIKLAMA
                                                            case "ACIKLAMA":
                                                                user.Aciklama = aciklama;
                                                                break;
                                                            #endregion
                                                            #region AD
                                                            case "AD":
                                                                user.Name = ad.ToUpper();
                                                                break;
                                                            #endregion
                                                            #region SOYAD
                                                            case "SOYAD":
                                                                user.Surname = soyad.ToUpper();
                                                                break;
                                                            #endregion
                                                            #region TCNO
                                                            case "TCNO":
                                                                user.tckno = tcno;
                                                                break;
                                                            #endregion
                                                            #region Lisanslar
                                                            case "PD1":
                                                                if (pd1 && BuAyGelecekAyKontrolu(pd1Sd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.PayL1 = true;
                                                                    acilan.Append(";PayL1");
                                                                }
                                                                break;

                                                            case "PD1P":
                                                                if (pd1p && BuAyGelecekAyKontrolu(pd1pSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.PayLP = true;
                                                                    lisansdurum.PayL1 = true;
                                                                    acilan.Append(";PayL1P");

                                                                }
                                                                else if (pd1p)
                                                                {
                                                                    if (user.LisansDurum.PayL1 && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.PayL1 = user.LisansDurum.PayL1;
                                                                        lisansdurum.PayL1Start = user.LisansDurum.PayL1Start;
                                                                        startdatepd1 = true;
                                                                    }
                                                                }
                                                                break;
                                                            case "PD2":
                                                                if (pd2 && BuAyGelecekAyKontrolu(pd2Sd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.PayL2 = true;
                                                                    lisansdurum.PayL1 = true;
                                                                    lisansdurum.PayLP = true;
                                                                    acilan.Append(";PayL2");
                                                                }
                                                                else if (pd2)
                                                                {
                                                                    if (user.LisansDurum.PayL1 && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.PayL1 = user.LisansDurum.PayL1;
                                                                        lisansdurum.PayL1Start = user.LisansDurum.PayL1Start;
                                                                        startdatepd1 = true;
                                                                    }
                                                                    if (user.LisansDurum.PayLP && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.PayLP = user.LisansDurum.PayLP;
                                                                        lisansdurum.PayLPStart = user.LisansDurum.PayLPStart;
                                                                        startdatepd1p = true;
                                                                    }
                                                                }
                                                                break;

                                                            case "PD2P":
                                                                if (pd2p && BuAyGelecekAyKontrolu(pd2pSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.Pd2P = true;
                                                                    lisansdurum.PayL2 = true;
                                                                    lisansdurum.PayL1 = true;
                                                                    lisansdurum.PayLP = true;
                                                                    acilan.Append(";Pd2P");
                                                                }
                                                                else if (pd2p)
                                                                {
                                                                    if (user.LisansDurum.PayL1 && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.PayL1 = user.LisansDurum.PayL1;
                                                                        lisansdurum.PayL1Start = user.LisansDurum.PayL1Start;
                                                                        startdatepd1 = true;
                                                                    }
                                                                    if (user.LisansDurum.PayLP && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.PayLP = user.LisansDurum.PayLP;
                                                                        lisansdurum.PayLPStart = user.LisansDurum.PayLPStart;
                                                                        startdatepd1p = true;
                                                                    }
                                                                    if (user.LisansDurum.PayL2 && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.PayL2 = user.LisansDurum.PayL2;
                                                                        lisansdurum.PayL2Start = user.LisansDurum.PayL2Start;
                                                                        startdatepd2 = true;
                                                                    }
                                                                }
                                                                break;
                                                            case "END":
                                                                if (end && BuAyGelecekAyKontrolu(endSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.PayX = true;
                                                                    acilan.Append(";PayX");

                                                                }
                                                                break;
                                                            case "PIT":
                                                                if (pit && BuAyGelecekAyKontrolu(pitSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.PayGS = true;
                                                                    acilan.Append(";PayGS");

                                                                }
                                                                break;
                                                            case "PITE":
                                                                if (pite && BuAyGelecekAyKontrolu(piteSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.PITE = true;
                                                                    acilan.Append(";PITE");

                                                                }
                                                                break;
                                                            case "VD1":
                                                                if (vd1 && BuAyGelecekAyKontrolu(vd1Sd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.ViopL1 = true;
                                                                    acilan.Append(";ViopL1");

                                                                }
                                                                break;
                                                            case "VD1P":
                                                                if (vd1p && BuAyGelecekAyKontrolu(vd1pSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.ViopLP = true;
                                                                    lisansdurum.ViopL1 = true;
                                                                    acilan.Append(";ViopLP");
                                                                }
                                                                else if (vd1p)
                                                                {
                                                                    if (user.LisansDurum.ViopL1 && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.ViopL1 = user.LisansDurum.ViopL1;
                                                                        lisansdurum.ViopL1Start = user.LisansDurum.ViopL1Start;
                                                                        startdatevd1 = true;
                                                                    }
                                                                }
                                                                break;
                                                            case "VD2":
                                                                if (vd2 && BuAyGelecekAyKontrolu(vd2Sd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.ViopL2 = true;
                                                                    lisansdurum.ViopLP = true;
                                                                    lisansdurum.ViopL1 = true;
                                                                    acilan.Append(";ViopL2");
                                                                }
                                                                else if (vd2)
                                                                {
                                                                    if (user.LisansDurum.ViopL1 && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.ViopL1 = user.LisansDurum.ViopL1;
                                                                        lisansdurum.ViopL1Start = user.LisansDurum.ViopL1Start;
                                                                        startdatevd1 = true;
                                                                    }
                                                                    if (user.LisansDurum.ViopLP && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.ViopLP = user.LisansDurum.ViopLP;
                                                                        lisansdurum.ViopLPStart = user.LisansDurum.ViopLPStart;
                                                                        startdatevd1p = true;
                                                                    }
                                                                }
                                                                break;

                                                            case "VD2P":
                                                                if (vd2p && BuAyGelecekAyKontrolu(vd2pSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.Vd2P = true;
                                                                    lisansdurum.ViopL2 = true;
                                                                    lisansdurum.ViopLP = true;
                                                                    lisansdurum.ViopL1 = true;
                                                                    acilan.Append(";Vd2P");
                                                                }
                                                                else if (vd2p)
                                                                {
                                                                    if (user.LisansDurum.ViopL1 && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.ViopL1 = user.LisansDurum.ViopL1;
                                                                        lisansdurum.ViopL1Start = user.LisansDurum.ViopL1Start;
                                                                        startdatevd1 = true;
                                                                    }
                                                                    if (user.LisansDurum.ViopLP && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.ViopLP = user.LisansDurum.ViopLP;
                                                                        lisansdurum.ViopLPStart = user.LisansDurum.ViopLPStart;
                                                                        startdatevd1p = true;
                                                                    }
                                                                    if (user.LisansDurum.ViopL2 && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.ViopL2 = user.LisansDurum.ViopL2;
                                                                        lisansdurum.ViopL2Start = user.LisansDurum.ViopL2Start;
                                                                        startdatevd2 = true;
                                                                    }
                                                                }
                                                                break;
                                                            case "VIT":
                                                                if (vit && BuAyGelecekAyKontrolu(vitSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.ViopGS = true;
                                                                    acilan.Append(";ViopGS");
                                                                }
                                                                break;
                                                            case "BD1":
                                                                if (bd1 && BuAyGelecekAyKontrolu(bd1Sd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.TahvilL1 = true;
                                                                    acilan.Append(";TahvilL1");
                                                                }
                                                                break;
                                                            case "BD1P":
                                                                if (bd1p && BuAyGelecekAyKontrolu(bd1pSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.TahvilLP = true;
                                                                    lisansdurum.TahvilL1 = true;
                                                                    acilan.Append(";TahvilLP");
                                                                }
                                                                else if (bd1p)
                                                                {
                                                                    if (user.LisansDurum.TahvilL1 && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.TahvilL1 = user.LisansDurum.TahvilL1;
                                                                        lisansdurum.TahvilL1Start = user.LisansDurum.TahvilL1Start;
                                                                        startdatebd1 = true;
                                                                    }
                                                                }
                                                                break;
                                                            case "BD2":
                                                                if (bd2 && BuAyGelecekAyKontrolu(bd2Sd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.TahvilL2 = true;
                                                                    lisansdurum.TahvilLP = true;
                                                                    lisansdurum.TahvilL1 = true;
                                                                    acilan.Append(";TahvilL2");

                                                                }
                                                                else if (bd2)
                                                                {
                                                                    if (user.LisansDurum.TahvilL1 && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.TahvilL1 = user.LisansDurum.TahvilL1;
                                                                        lisansdurum.TahvilL1Start = user.LisansDurum.TahvilL1Start;
                                                                        startdatebd1 = true;
                                                                    }
                                                                    if (user.LisansDurum.TahvilLP && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.TahvilLP = user.LisansDurum.TahvilLP;
                                                                        lisansdurum.TahvilLPStart = user.LisansDurum.TahvilLPStart;
                                                                        startdatebd1p = true;
                                                                    }
                                                                }
                                                                break;
                                                            case "ANPRO":
                                                                if (anpro && BuAyGelecekAyKontrolu(anproSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.AnPro = true;
                                                                    acilan.Append(";AnalizPro");
                                                                }
                                                                break;
                                                            case "SENTIL1":
                                                                if (sentiL1 && BuAyGelecekAyKontrolu(sentiL1Sd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.SentiL1 = true;
                                                                }
                                                                break;
                                                            case "SENTIL2":
                                                                if (sentiL2 && BuAyGelecekAyKontrolu(sentiL2Sd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.SentiL2 = true;
                                                                }
                                                                else if (sentiL2)
                                                                {
                                                                    if (user.LisansDurum.SentiL1 && oncekiLisansDurum)
                                                                    {
                                                                        lisansdurum.SentiL1 = user.LisansDurum.SentiL1;
                                                                        lisansdurum.SentiL1Start = user.LisansDurum.SentiL1Start;
                                                                        startdatesentiL1 = true;

                                                                    }
                                                                }
                                                                break;
                                                            case "KRMD1":
                                                                if (krmd1 && BuAyGelecekAyKontrolu(krmd1Sd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.COMEX = true;
                                                                    acilan.Append(";KRMD1");
                                                                }
                                                                break;

                                                            case "MKK":
                                                                if (mkk && BuAyGelecekAyKontrolu(mkkSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.MKK = true;
                                                                    acilan.Append(";MKK");
                                                                }
                                                                break;
                                                            case "GKKUL":
                                                                if (gkkul && BuAyGelecekAyKontrolu(gkkulSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.GKKUL = true;
                                                                    acilan.Append(";GKKUL");
                                                                }
                                                                break;
                                                            case "TARAMA":
                                                                if (tarama && BuAyGelecekAyKontrolu(taramaSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.TARAMA = true;
                                                                    acilan.Append(";TARAMA");
                                                                }
                                                                break;
                                                            case "CME":
                                                                if (cme && BuAyGelecekAyKontrolu(cmeSd.StrinToDateTime()))
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.CME = true;
                                                                    acilan.Append(";CME");
                                                                }
                                                                break;
                                                            case "YDS":
                                                                if (yds)
                                                                {
                                                                    durumbool = true;
                                                                    lisansdurum.SPI = true;
                                                                    acilan.Append(";SPI");
                                                                }
                                                                else
                                                                {
                                                                    durumbool = false;
                                                                    lisansdurum.SPI = false;
                                                                    kapanan.Append(";SPI");
                                                                }
                                                                break;
                                                            case "PD1SD":
                                                                {
                                                                    if (startdatepd1 == false)
                                                                        lisansdurum.PayL1Start = pd1Sd.StrinToDateTime();
                                                                    durumbool = true;
                                                                }
                                                                break;
                                                            case "PD1PSD":
                                                                {
                                                                    if (startdatepd1p == false)
                                                                    {
                                                                        lisansdurum.PayL1Start = pd1Sd == "" ? pd1pSd.StrinToDateTime() : pd1Sd.StrinToDateTime();
                                                                        lisansdurum.PayLPStart = pd1pSd.StrinToDateTime();
                                                                    }
                                                                    durumbool = true;
                                                                }
                                                                break;
                                                            case "PD2SD":
                                                                {
                                                                    if (startdatepd2 == false)
                                                                    {
                                                                        lisansdurum.PayLPStart = pd1pSd == "" ? pd2Sd.StrinToDateTime() : pd1pSd.StrinToDateTime();
                                                                        lisansdurum.PayL1Start = pd1Sd == "" ? pd2Sd.StrinToDateTime() : pd1Sd.StrinToDateTime();
                                                                        lisansdurum.PayL2Start = pd2Sd.StrinToDateTime();
                                                                    }
                                                                    durumbool = true;
                                                                }
                                                                break;
                                                            case "PD2PSD":
                                                                {
                                                                    if (startdatepd2p == false)
                                                                    {
                                                                        lisansdurum.PayLPStart = pd2pSd.StrinToDateTime();
                                                                        lisansdurum.PayL1Start = pd2pSd.StrinToDateTime();
                                                                        lisansdurum.PayL2Start = pd2pSd.StrinToDateTime();
                                                                        lisansdurum.Pd2PStart = pd2pSd.StrinToDateTime();
                                                                    }
                                                                    durumbool = true;
                                                                }
                                                                break;
                                                            case "ENDSD": lisansdurum.PayXStart = endSd.StrinToDateTime(); durumbool = true; break;
                                                            case "PITSD": lisansdurum.PayGSStart = pitSd.StrinToDateTime(); durumbool = true; break;
                                                            case "PITESD": lisansdurum.PayPiteStart = piteSd.StrinToDateTime(); durumbool = true; break;
                                                            case "VD1SD":
                                                                {
                                                                    if (startdatevd1 == false)
                                                                        lisansdurum.ViopL1Start = vd1Sd.StrinToDateTime(); durumbool = true;
                                                                }
                                                                break;
                                                            case "VD1PSD":
                                                                {
                                                                    if (startdatevd1p == false)
                                                                    {
                                                                        lisansdurum.ViopL1Start = vd1Sd == "" ? vd1pSd.StrinToDateTime() : vd1Sd.StrinToDateTime();
                                                                        lisansdurum.ViopLPStart = vd1pSd.StrinToDateTime();
                                                                    }
                                                                    durumbool = true;
                                                                }
                                                                break;
                                                            case "VD2SD":
                                                                {
                                                                    if (startdatevd2 == false)
                                                                    {
                                                                        lisansdurum.ViopL1Start = vd1Sd == "" ? vd2Sd.StrinToDateTime() : vd1Sd.StrinToDateTime();
                                                                        lisansdurum.ViopLPStart = vd1pSd == "" ? vd2Sd.StrinToDateTime() : vd1pSd.StrinToDateTime();
                                                                        lisansdurum.ViopL2Start = vd2Sd.StrinToDateTime();
                                                                    }
                                                                    durumbool = true;
                                                                }
                                                                break;

                                                            case "VD2PSD":
                                                                {
                                                                    lisansdurum.ViopL1Start = vd1Sd == "" ? vd2pSd.StrinToDateTime() : vd1Sd.StrinToDateTime();
                                                                    lisansdurum.ViopLPStart = vd1pSd == "" ? vd2pSd.StrinToDateTime() : vd1pSd.StrinToDateTime();
                                                                    lisansdurum.Vd2PStart = vd2Sd == "" ? vd2pSd.StrinToDateTime() : vd2Sd.StrinToDateTime();
                                                                    lisansdurum.ViopL2Start = vd2pSd.StrinToDateTime();
                                                                    durumbool = true;
                                                                }
                                                                break;
                                                            case "VITSD": lisansdurum.ViopGSStart = vitSd.StrinToDateTime(); durumbool = true; break;
                                                            case "BD1SD":
                                                                {
                                                                    if (startdatebd1 == false)
                                                                    {
                                                                        lisansdurum.TahvilL1Start = bd1Sd.StrinToDateTime();
                                                                    }
                                                                    durumbool = true;
                                                                }
                                                                break;
                                                            case "BD1PSD":
                                                                {
                                                                    if (startdatebd1p == false)
                                                                    {
                                                                        lisansdurum.TahvilL1Start = bd1Sd == "" ? bd1pSd.StrinToDateTime() : bd1Sd.StrinToDateTime();
                                                                        lisansdurum.TahvilLPStart = bd1pSd.StrinToDateTime();
                                                                    }
                                                                    durumbool = true;
                                                                }
                                                                break;
                                                            case "BD2SD":
                                                                lisansdurum.TahvilL1Start = bd1Sd == "" ? bd2Sd.StrinToDateTime() : bd1Sd.StrinToDateTime();
                                                                lisansdurum.TahvilLPStart = bd1pSd == "" ? bd2Sd.StrinToDateTime() : bd1pSd.StrinToDateTime();
                                                                lisansdurum.TahvilL2Start = bd2Sd.StrinToDateTime();
                                                                durumbool = true;
                                                                break;
                                                            case "ANPROSD":
                                                                lisansdurum.AnProStart = anproSd.StrinToDateTime();
                                                                durumbool = true;
                                                                break;
                                                            case "SENTIL1SD":
                                                                if (startdatesentiL1 == false)
                                                                    lisansdurum.SentiL1Start = sentiL1Sd.StrinToDateTime(); durumbool = true;
                                                                break;
                                                            case "SENTIL2SD": lisansdurum.SentiL2Start = sentiL2Sd.StrinToDateTime(); durumbool = true; break;
                                                            case "KRMD1SD": lisansdurum.KRMD1Start = krmd1Sd.StrinToDateTime(); durumbool = true; break;
                                                            case "PROSD": lisansdurum.ProYetkiStart = proSd.StrinToDateTime(); durumbool = true; break;
                                                            case "CEPSD": lisansdurum.CepYetkiStart = cepSd.StrinToDateTime(); durumbool = true; break;
                                                            case "MKKSD": lisansdurum.MKKStart = mkkSd.StrinToDateTime(); durumbool = true; break;
                                                            case "TARAMASD": lisansdurum.TaramaStart = taramaSd.StrinToDateTime(); durumbool = true; break;
                                                            case "GKKULSD": lisansdurum.GKKULStart = gkkulSd.StrinToDateTime(); durumbool = true; break;
                                                            case "CMESD": lisansdurum.CMEStart = cmeSd.StrinToDateTime(); durumbool = true; break;
                                                            case "PD1ED":
                                                                lisansdurum.PayL1End = pd1Ed.StrinToDateTime();
                                                                durumbool = true;
                                                                break;
                                                            case "PD1PED":
                                                                lisansdurum.PayL1End = pd1pEd.StrinToDateTime();
                                                                lisansdurum.PayLPEnd = pd1pEd.StrinToDateTime();
                                                                durumbool = true;
                                                                break;
                                                            case "PD2ED":
                                                                lisansdurum.PayL2End = pd2Ed.StrinToDateTime();
                                                                if (pd2Ed.StrinToDateTime() > pd1pEd.StrinToDateTime()) lisansdurum.PayLPEnd = pd2Ed.StrinToDateTime();
                                                                else lisansdurum.PayLPEnd = pd1pEd.StrinToDateTime();
                                                                lisansdurum.PayL1End = pd2Ed.StrinToDateTime();
                                                                durumbool = true; break;
                                                            case "PD2PED":
                                                                lisansdurum.Pd2PEnd = pd2pEd.StrinToDateTime();

                                                                if (pd2pEd.StrinToDateTime() > pd2Ed.StrinToDateTime()) lisansdurum.PayL2End = pd2pEd.StrinToDateTime();
                                                                else lisansdurum.PayL2End = pd2Ed.StrinToDateTime();

                                                                if (pd2pEd.StrinToDateTime() > pd1pEd.StrinToDateTime()) lisansdurum.PayLPEnd = pd2pEd.StrinToDateTime();
                                                                else lisansdurum.PayLPEnd = pd1pEd.StrinToDateTime();

                                                                if (pd2pEd.StrinToDateTime() > pd1Ed.StrinToDateTime()) lisansdurum.PayL1End = pd2pEd.StrinToDateTime();
                                                                else lisansdurum.PayL1End = pd1Ed.StrinToDateTime();
                                                                durumbool = true; break;
                                                            case "ENDED": lisansdurum.PayXEnd = endEd.StrinToDateTime(); durumbool = true; break;
                                                            case "PITED": lisansdurum.PayGSEnd = pitEd.StrinToDateTime(); durumbool = true; break;
                                                            case "PITEED": lisansdurum.PayPiteEnd = piteEd.StrinToDateTime(); durumbool = true; break;
                                                            case "VD1ED":
                                                                lisansdurum.ViopL1End = vd1Ed.StrinToDateTime();
                                                                durumbool = true;
                                                                break;
                                                            case "VD1PED":
                                                                lisansdurum.ViopL1End = vd1pEd.StrinToDateTime();
                                                                lisansdurum.ViopLPEnd = vd1pEd.StrinToDateTime();
                                                                durumbool = true;
                                                                break;
                                                            case "VD2ED":
                                                                if (vd2Ed.StrinToDateTime() > vd1Ed.StrinToDateTime()) lisansdurum.ViopL1End = vd2Ed.StrinToDateTime();
                                                                else lisansdurum.ViopL1End = vd1Ed.StrinToDateTime();

                                                                if (vd2Ed.StrinToDateTime() > vd1pEd.StrinToDateTime()) lisansdurum.ViopLPEnd = vd2Ed.StrinToDateTime();
                                                                else lisansdurum.ViopLPEnd = vd1pEd.StrinToDateTime();

                                                                lisansdurum.ViopL2End = vd2Ed.StrinToDateTime();
                                                                durumbool = true;
                                                                break;
                                                            case "VD2PED":
                                                                if (vd2pEd.StrinToDateTime() > vd1Ed.StrinToDateTime()) lisansdurum.ViopL1End = vd2pEd.StrinToDateTime();
                                                                else lisansdurum.ViopL1End = vd1Ed.StrinToDateTime();

                                                                if (vd2pEd.StrinToDateTime() > vd1pEd.StrinToDateTime()) lisansdurum.ViopLPEnd = vd2pEd.StrinToDateTime();
                                                                else lisansdurum.ViopLPEnd = vd1pEd.StrinToDateTime();

                                                                if (vd2pEd.StrinToDateTime() > vd2Ed.StrinToDateTime()) lisansdurum.ViopL2End = vd2pEd.StrinToDateTime();
                                                                else lisansdurum.ViopL2End = vd2Ed.StrinToDateTime();

                                                                lisansdurum.Vd2PEnd = vd2pEd.StrinToDateTime();
                                                                durumbool = true;
                                                                break;
                                                            case "VITED": lisansdurum.ViopGSEnd = vitEd.StrinToDateTime(); durumbool = true; break;
                                                            case "BD1ED":
                                                                lisansdurum.TahvilL1End = bd1Ed.StrinToDateTime();
                                                                durumbool = true;
                                                                break;
                                                            case "BD1PED":
                                                                lisansdurum.TahvilL1End = bd1pEd.StrinToDateTime();
                                                                lisansdurum.TahvilLPEnd = bd1pEd.StrinToDateTime();
                                                                durumbool = true;
                                                                break;
                                                            case "BD2ED":
                                                                lisansdurum.TahvilL1End = bd2Ed.StrinToDateTime();
                                                                lisansdurum.TahvilLPEnd = bd2Ed.StrinToDateTime();
                                                                lisansdurum.TahvilL2End = bd2Ed.StrinToDateTime();
                                                                durumbool = true;
                                                                break;
                                                            case "ANPROED":
                                                                lisansdurum.AnProEnd = anproEd.StrinToDateTime();
                                                                durumbool = true;
                                                                break;
                                                            case "SENTIL1ED": lisansdurum.SentiL1End = sentiL1Ed.StrinToDateTime(); durumbool = true; break;
                                                            case "SENTIL2ED": lisansdurum.SentiL2End = sentiL2Ed.StrinToDateTime(); durumbool = true; break;
                                                            case "PROED": lisansdurum.ProYetkiEnd = proEd.StrinToDateTime(); durumbool = true; break;
                                                            case "CEPED": lisansdurum.CepYetkiEnd = cepEd.StrinToDateTime(); durumbool = true; break;
                                                            case "KRMD1ED": lisansdurum.KRMD1End = krmd1Ed.StrinToDateTime(); durumbool = true; break;
                                                            case "MKKED": lisansdurum.MKKEnd = mkkEd.StrinToDateTime(); durumbool = true; break;
                                                            case "GKKULED": lisansdurum.GKKULEnd = gkkulEd.StrinToDateTime(); durumbool = true; break;
                                                            case "TARAMAED": lisansdurum.TaramaEnd = taramaEd.StrinToDateTime(); durumbool = true; break;
                                                            case "CMEED": lisansdurum.CMEEnd = cmeEd.StrinToDateTime(); durumbool = true; break;
                                                                #endregion
                                                        }
                                                    }
                                                    #endregion


                                                    if (lisansdurum.COMEX)
                                                    {
                                                        if (lisansdurum.Pd2P == false && lisansdurum.PayL2 == false && lisansdurum.PayLP == false)
                                                        {


                                                            if (lisansdurum.PayL1 == true)
                                                            {
                                                                lisansdurum.PayL1 = false;
                                                                lisansdurum.PayL1End = null;
                                                                lisansdurum.PayL1Start = null;
                                                            }
                                                        }

                                                        if (lisansdurum.ViopLP == false && lisansdurum.ViopL2 == false && lisansdurum.Vd2P == false)
                                                        {

                                                            if (lisansdurum.ViopL1 == true)
                                                            {
                                                                lisansdurum.ViopL1 = false;
                                                                lisansdurum.ViopL1End = null;
                                                                lisansdurum.ViopL1Start = null;

                                                            }
                                                        }

                                                    }

                                                    if (karmaGeldi)
                                                    {
                                                        if (Pd1Geldi == true && Pd1pGeldi == false && Pd2Geldi == false && Pd2pGeldi == false)
                                                        {

                                                            lisansdurum.PayL1 = false;
                                                            lisansdurum.PayL1End = null;
                                                            lisansdurum.PayL1Start = null;
                                                        }

                                                        if (Vd1Geldi == true && Vd1pGeldi == false && Vd2Geldi == false && Vd2pGeldi == false)
                                                        {
                                                            lisansdurum.ViopL1 = false;
                                                            lisansdurum.ViopL1End = null;
                                                            lisansdurum.ViopL1Start = null;
                                                        }

                                                    }


                                                    if (lisansdurum.PITE)
                                                    {

                                                        if (lisansdurum.PayPiteStart.Value.Date > new DateTime(2020, 12, 1))
                                                        {

                                                            lisansdurum.PayGS = false;
                                                            lisansdurum.PayGSStart = null;
                                                            lisansdurum.PayGSEnd = null;
                                                        }
                                                        else if (user.LisansDurum.PayGS == false)
                                                        {

                                                            lisansdurum.PayGS = false;
                                                            lisansdurum.PayGSStart = null;
                                                            lisansdurum.PayGSEnd = null;
                                                        }
                                                    }


                                                    if (durumbool)
                                                    {
                                                        lisansdurum.Futgck = true;
                                                        lisansdurum.WINX = true;

                                                        if ((lisansdurum.ProYetki == false && lisansdurum.CepYetki == false) && ((lisansdurum.CepYetkiStart != null && lisansdurum.CepYetkiStart.Value.Date >= DateTime.Now.Date) || (lisansdurum.CepYetkiStart != null && lisansdurum.ProYetkiStart.Value.Date >= DateTime.Now.Date)))
                                                        {
                                                            lisansdurum.YayinDurumu = false;
                                                        }

                                                        crm.LisansDurums.InsertOnSubmit(lisansdurum);
                                                        crm.SubmitChanges();

                                                        user.LisansDurum = lisansdurum;
                                                        userevent.AcilanLisans = acilan.ToString();
                                                        userevent.KapatilanLisans = kapanan.ToString();
                                                        crm.SubmitChanges();
                                                        userevent.SonLisandurumID = user.LisansDurumId;

                                                        userevent.UserId = user.UserID;
                                                        if (userevent.UserId == 0)
                                                            crm.UserEvents.InsertOnSubmit(userevent);
                                                    }

                                                    crm.SubmitChanges();

                                                    sonuc.Kod = "100";
                                                    sonuc.Aciklama = "Lisanslar güncellenmiştir.";
                                                    sonuc.Status = "Başarılı";
                                                    var text = "Kullanıcı Değişme  ;  user name  = " + user.UserName + " ; Remeto Ip =" + userevent.IP;
                                                    formListeTransaction(text);
                                                    CalisanlaraKomutGonder("_ChangeUser|" + text);
                                                    SendToSSO(user.UserID);

                                                }
                                                else
                                                {
                                                    sonuc.Kod = "101";
                                                    sonuc.Aciklama = "Kullanıcı bulunamadı.";
                                                    sonuc.Status = "Başarısız";
                                                }
                                            }
                                            else
                                            {
                                                sonuc.Kod = "101";
                                                sonuc.Aciklama = "Çalışan bulunamadı.";
                                                sonuc.Status = "Başarısız";
                                            }
                                            var jsn = new JavaScriptSerializer().Serialize(sonuc);
                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(jsn._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }
                                    #endregion
                                    #region CHECKUSER
                                    case "CHECKUSER":
                                        {
                                            var sonuc = new Sonuc();
                                            var username = "";
                                            var hesapNo = "";
                                            var password = "";
                                            var mailadres = "";
                                            for (int i = 2; i < fieldarray.Length; i++)
                                            {
                                                var splitarray = fieldarray[i].Split('=');
                                                switch (splitarray[0].Trim())
                                                {
                                                    case "USERNAME": username = splitarray[1].Trim(); break;
                                                    case "HESAPNO": hesapNo = splitarray[1].Trim(); break;
                                                    case "PASSWORD": password = splitarray[1].Trim(); break;
                                                    case "MAIL": mailadres = splitarray[1].Trim(); break;
                                                }
                                            }
                                            var calPassword = MyTools.Sifreleme.Encryp(password);

                                            var cal = crm.Calisans.FirstOrDefault(x => x.UserName == username && x.Password == calPassword);
                                            if (crm.Calisans.Where(x => x.UserName == username && x.Password == calPassword).Any())
                                            {
                                                var sirket_adi = crm.Musterilers.Where(x => x.MusteriNo == cal.kurumMutserino).Select(x => x.MusteriAdi).FirstOrDefault();

                                                if (hesapNo == "" && mailadres == "")
                                                {
                                                    sonuc.Kod = "102";
                                                    sonuc.Aciklama = "HesapNo ve/veya Mail Boş Olamaz.";
                                                    sonuc.Status = "Eksik Bilgi";

                                                }
                                                var sorgu = crm.Users;

                                                if (mailadres != "")
                                                {
                                                    if (sorgu.Where(x => x.Iletisim.email == mailadres).Any())
                                                    {
                                                        var storeUserList = new List<StoreUserMailCass>();
                                                        var users = sorgu.Where(x => x.Iletisim.email == mailadres).ToList();
                                                        for (int i = 0; i < users.Count; i++)
                                                        {
                                                            var storeuser = new StoreUserMailCass();
                                                            storeuser.Username = users[i].UserName;
                                                            storeuser.Status = users[i].LisansDurum.YayinDurumu._ToIntStr();

                                                            storeUserList.Add(storeuser);
                                                        }
                                                        responsestr = new JavaScriptSerializer().Serialize(storeUserList.OrderBy(x => x.Username).ToList());
                                                    }
                                                    else
                                                    {
                                                        sonuc.Kod = "101";
                                                        sonuc.Aciklama = "Kullanıcı Bulunamadı.";
                                                        sonuc.Status = "Başarısız";
                                                    }

                                                }
                                                else
                                                {
                                                    if (sorgu.Where(x => x.UserName == hesapNo).Any())
                                                    {
                                                        var user = sorgu.FirstOrDefault(x => x.UserName == hesapNo);
                                                        var storeuser = new StoreUserCass();
                                                        storeuser.Username = user.UserName;
                                                        responsestr = new JavaScriptSerializer().Serialize(storeuser);
                                                        sonuc.Kod = "100";
                                                        sonuc.Aciklama = "Kullanıcı Mevcut.";
                                                        sonuc.Status = "Başarılı";
                                                    }
                                                    else
                                                    {
                                                        sonuc.Kod = "101";
                                                        sonuc.Aciklama = "Kullanıcı Bulunamadı.";
                                                        sonuc.Status = "Başarısız";
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                sonuc.Kod = "101";
                                                sonuc.Aciklama = "Çalışan Bulunamadı.";
                                                sonuc.Status = "Başarısız";
                                            }
                                            var jsn = new JavaScriptSerializer().Serialize(sonuc);
                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(jsn._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }
                                    #endregion
                                    #region INFOUSER
                                    case "INFOUSER":
                                        {
                                            var sonuc = new Sonuc();
                                            var username = "";
                                            var hesapNo = "";
                                            var password = "";
                                            for (int i = 2; i < fieldarray.Length; i++)
                                            {
                                                var splitarray = fieldarray[i].Split('=');
                                                switch (splitarray[0].Trim())
                                                {
                                                    case "USERNAME": username = splitarray[1].Trim(); break;
                                                    case "HESAPNO": hesapNo = splitarray[1].Trim(); break;
                                                    case "PASSWORD": password = splitarray[1].Trim(); break;
                                                }
                                            }
                                            var calPassword = MyTools.Sifreleme.Encryp(password);

                                            var cal = crm.Calisans.FirstOrDefault(x => x.UserName == username && x.Password == calPassword);
                                            if (crm.Calisans.Where(x => x.UserName == username && x.Password == calPassword).Any())
                                            {


                                                if (crm.Users.Where(x => x.UserName == hesapNo).Any())
                                                {
                                                    var user = crm.Users.FirstOrDefault(x => x.UserName == hesapNo);

                                                    var storeuser = new StoreResponseClass();

                                                    storeuser.USERNAME = user.UserName;
                                                    storeuser.PASSWORD = MyTools.Sifreleme.Decryp(user.Password);
                                                    storeuser.AD = user.Name;
                                                    storeuser.SOYAD = user.Surname;
                                                    storeuser.ACIKLAMA = user.Aciklama;
                                                    storeuser.TCNO = user.tckno;

                                                    storeuser.EXPIREDATE = user.ExpiryDate.Value.Date.ToString("yyyyMMdd");

                                                    if (user.Iletisim != null)
                                                    {
                                                        storeuser.ADRES = user.Iletisim.acikadres;
                                                        storeuser.GSM = user.Iletisim.Ceptel;
                                                        storeuser.EMAIL = user.Iletisim.email;
                                                        if (user.Iletisim.Ulke != null)
                                                            storeuser.ULKE = user.Iletisim.Ulke.UlkeAdi;
                                                        if (user.Iletisim.Il != null)
                                                            storeuser.SEHIR = user.Iletisim.Il.IlAdi;

                                                    }

                                                    //LİSANSLAR

                                                    #region Lisanslar
                                                    if ((user.LisansDurum.ProYetki == false && user.LisansDurum.CepYetki == false) &&
                                                    //(user.LisansDurum.ProYetkiStart != null && user.LisansDurum.CepYetkiStart != null) && 
                                                    ((user.LisansDurum.ProYetkiStart != null && user.LisansDurum.ProYetkiStart.Value.Date >= DateTime.Now.Date) ||
                                                    (user.LisansDurum.CepYetkiStart != null && user.LisansDurum.CepYetkiStart.Value.Date >= DateTime.Now.Date)))
                                                    {
                                                        storeuser.ONOF = true._ToIntStr();

                                                        #region PRO
                                                        if (user.LisansDurum.ProYetkiStart != null && user.LisansDurum.ProYetkiStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.PRO = user.LisansDurum.ProYetki._ToIntStr();
                                                            if (user.LisansDurum.ProYetkiStart != null)
                                                                storeuser.PROSD = user.LisansDurum.ProYetkiStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.ProYetkiEnd != null)
                                                                storeuser.PROED = user.LisansDurum.ProYetkiEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.PRO = false._ToIntStr();
                                                            storeuser.PROSD = null;
                                                            storeuser.PROED = null;
                                                        }
                                                        #endregion
                                                        #region CEP
                                                        if (user.LisansDurum.CepYetkiStart != null && user.LisansDurum.CepYetkiStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.CEP = user.LisansDurum.CepYetki._ToIntStr();
                                                            if (user.LisansDurum.CepYetkiStart != null)
                                                                storeuser.CEPSD = user.LisansDurum.CepYetkiStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.CepYetkiEnd != null)
                                                                storeuser.CEPED = user.LisansDurum.CepYetkiEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.CEP = false._ToIntStr();
                                                            storeuser.CEPSD = null;
                                                            storeuser.CEPED = null;
                                                        }
                                                        #endregion
                                                        #region KRMD1
                                                        if (user.LisansDurum.KRMD1Start != null && user.LisansDurum.KRMD1Start.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.KRMD1 = user.LisansDurum.COMEX._ToIntStr();
                                                            if (user.LisansDurum.KRMD1Start != null)
                                                                storeuser.KRMD1SD = user.LisansDurum.KRMD1Start.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.KRMD1End != null)
                                                                storeuser.KRMD1ED = user.LisansDurum.KRMD1End.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.KRMD1 = false._ToIntStr();
                                                            storeuser.KRMD1SD = null;
                                                            storeuser.KRMD1ED = null;
                                                        }
                                                        #endregion
                                                        #region PD1

                                                        #endregion
                                                        #region PD1P
                                                        if (user.LisansDurum.PayLPStart != null && user.LisansDurum.PayLPStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.PD1 = user.LisansDurum.PayL1._ToIntStr();
                                                            if (user.LisansDurum.PayL1Start != null)
                                                                storeuser.PD1SD = user.LisansDurum.PayL1Start.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.PayL1End != null)
                                                                storeuser.PD1ED = user.LisansDurum.PayL1End.Value.ToString("yyyyMMdd");

                                                            storeuser.PD1P = user.LisansDurum.PayLP._ToIntStr();
                                                            if (user.LisansDurum.PayLPStart != null)
                                                                storeuser.PD1PSD = user.LisansDurum.PayLPStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.PayLPEnd != null)
                                                                storeuser.PD1PED = user.LisansDurum.PayLPEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.PD1 = false._ToIntStr();
                                                            storeuser.PD1SD = null;
                                                            storeuser.PD1ED = null;
                                                            storeuser.PD1P = false._ToIntStr();
                                                            storeuser.PD1PSD = null;
                                                            storeuser.PD1PED = null;
                                                        }
                                                        #endregion
                                                        #region PD2
                                                        if (user.LisansDurum.PayL2Start != null && user.LisansDurum.PayL2Start.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.PD2 = user.LisansDurum.PayL2._ToIntStr();
                                                            if (user.LisansDurum.PayL2Start != null)
                                                                storeuser.PD2SD = user.LisansDurum.PayL2Start.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.PayL2End != null)
                                                                storeuser.PD2ED = user.LisansDurum.PayL2End.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.PD2 = false._ToIntStr();
                                                            storeuser.PD2SD = null;
                                                            storeuser.PD2ED = null;
                                                        }
                                                        #endregion
                                                        #region PD2P
                                                        if (user.LisansDurum.Pd2PStart != null && user.LisansDurum.Pd2PStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.PD2P = user.LisansDurum.Pd2P._ToIntStr();
                                                            if (user.LisansDurum.Pd2PStart != null)
                                                                storeuser.PD2PSD = user.LisansDurum.Pd2PStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.Pd2PEnd != null)
                                                                storeuser.PD2PED = user.LisansDurum.Pd2PEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.PD2P = false._ToIntStr();
                                                            storeuser.PD2PSD = null;
                                                            storeuser.PD2PED = null;
                                                        }
                                                        #endregion

                                                        #region END
                                                        if (user.LisansDurum.PayXStart != null && user.LisansDurum.PayXStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.END = user.LisansDurum.PayX._ToIntStr();
                                                            if (user.LisansDurum.PayXStart != null)
                                                                storeuser.ENDSD = user.LisansDurum.PayXStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.PayXEnd != null)
                                                                storeuser.ENDED = user.LisansDurum.PayXEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.END = false._ToIntStr();
                                                            storeuser.ENDSD = null;
                                                            storeuser.ENDED = null;
                                                        }
                                                        #endregion
                                                        #region PIT
                                                        if (user.LisansDurum.PayGSStart != null && user.LisansDurum.PayGSStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.PIT = user.LisansDurum.PayGS._ToIntStr();
                                                            if (user.LisansDurum.PayGSStart != null)
                                                                storeuser.PITSD = user.LisansDurum.PayGSStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.PayGSEnd != null)
                                                                storeuser.PITED = user.LisansDurum.PayGSEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.PIT = false._ToIntStr();
                                                            storeuser.PITSD = null;
                                                            storeuser.PITED = null;
                                                        }
                                                        #endregion
                                                        #region PITE
                                                        if (user.LisansDurum.PayPiteStart != null && user.LisansDurum.PayPiteStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.PITE = user.LisansDurum.PITE._ToIntStr();
                                                            if (user.LisansDurum.PayPiteStart != null)
                                                                storeuser.PITESD = user.LisansDurum.PayPiteStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.PayPiteEnd != null)
                                                                storeuser.PITEED = user.LisansDurum.PayPiteEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.PITE = false._ToIntStr();
                                                            storeuser.PITESD = null;
                                                            storeuser.PITEED = null;
                                                        }
                                                        #endregion

                                                        #region VD1

                                                        #endregion

                                                        #region VD1P
                                                        if (user.LisansDurum.ViopLPStart != null && user.LisansDurum.ViopLPStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.VD1 = user.LisansDurum.ViopL1._ToIntStr();
                                                            if (user.LisansDurum.ViopL1Start != null)
                                                                storeuser.VD1SD = user.LisansDurum.ViopL1Start.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.ViopL1End != null)
                                                                storeuser.VD1ED = user.LisansDurum.ViopL1End.Value.ToString("yyyyMMdd");

                                                            storeuser.VD1P = user.LisansDurum.ViopLP._ToIntStr();
                                                            if (user.LisansDurum.ViopLPStart != null)
                                                                storeuser.VD1PSD = user.LisansDurum.ViopLPStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.ViopLPEnd != null)
                                                                storeuser.VD1PED = user.LisansDurum.ViopLPEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.VD1 = false._ToIntStr();
                                                            storeuser.VD1SD = null;
                                                            storeuser.VD1ED = null;
                                                            storeuser.VD1P = false._ToIntStr();
                                                            storeuser.VD1PSD = null;
                                                            storeuser.VD1PED = null;
                                                        }
                                                        #endregion
                                                        #region VD2
                                                        if (user.LisansDurum.ViopL2Start != null && user.LisansDurum.ViopL2Start.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.VD2 = user.LisansDurum.ViopL2._ToIntStr();
                                                            if (user.LisansDurum.ViopL2Start != null)
                                                                storeuser.VD2SD = user.LisansDurum.ViopL2Start.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.ViopL2End != null)
                                                                storeuser.VD2ED = user.LisansDurum.ViopL2End.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.VD2 = false._ToIntStr();
                                                            storeuser.VD2SD = null;
                                                            storeuser.VD2ED = null;
                                                        }
                                                        #endregion
                                                        #region VD2P
                                                        if (user.LisansDurum.Vd2PStart != null && user.LisansDurum.Vd2PStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.VD2P = user.LisansDurum.Vd2P._ToIntStr();
                                                            if (user.LisansDurum.Vd2PStart != null)
                                                                storeuser.VD2PSD = user.LisansDurum.Vd2PStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.Vd2PEnd != null)
                                                                storeuser.VD2PED = user.LisansDurum.Vd2PEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.VD2P = false._ToIntStr();
                                                            storeuser.VD2PSD = null;
                                                            storeuser.VD2PED = null;
                                                        }
                                                        #endregion
                                                        #region VIT
                                                        if (user.LisansDurum.ViopGSStart != null && user.LisansDurum.ViopGSStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.VIT = user.LisansDurum.ViopGS._ToIntStr();
                                                            if (user.LisansDurum.ViopGSStart != null)
                                                                storeuser.VITSD = user.LisansDurum.ViopGSStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.ViopGSEnd != null)
                                                                storeuser.VITED = user.LisansDurum.ViopGSEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.VIT = false._ToIntStr();
                                                            storeuser.VITSD = null;
                                                            storeuser.VITED = null;
                                                        }
                                                        #endregion

                                                        #region BD1

                                                        #endregion

                                                        #region BD1P
                                                        if (user.LisansDurum.TahvilLPStart != null && user.LisansDurum.TahvilLPStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.BD1 = user.LisansDurum.TahvilL1._ToIntStr();
                                                            if (user.LisansDurum.TahvilL1Start != null)
                                                                storeuser.BD1SD = user.LisansDurum.TahvilL1Start.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.TahvilL1End != null)
                                                                storeuser.BD1ED = user.LisansDurum.TahvilL1End.Value.ToString("yyyyMMdd");

                                                            storeuser.BD1P = user.LisansDurum.TahvilLP._ToIntStr();
                                                            if (user.LisansDurum.TahvilLPStart != null)
                                                                storeuser.BD1PSD = user.LisansDurum.TahvilLPStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.TahvilLPEnd != null)
                                                                storeuser.BD1PED = user.LisansDurum.TahvilLPEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.BD1 = false._ToIntStr();
                                                            storeuser.BD1SD = null;
                                                            storeuser.BD1ED = null;
                                                            storeuser.BD1P = false._ToIntStr();
                                                            storeuser.BD1PSD = null;
                                                            storeuser.BD1PED = null;
                                                        }
                                                        #endregion
                                                        #region BD2
                                                        if (user.LisansDurum.TahvilL2Start != null && user.LisansDurum.TahvilL2Start.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.BD2 = user.LisansDurum.TahvilL2._ToIntStr();
                                                            if (user.LisansDurum.TahvilL2Start != null)
                                                                storeuser.BD2SD = user.LisansDurum.TahvilL2Start.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.TahvilL2End != null)
                                                                storeuser.BD2ED = user.LisansDurum.TahvilL2End.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.BD2 = false._ToIntStr();
                                                            storeuser.BD2SD = null;
                                                            storeuser.BD2ED = null;
                                                        }
                                                        #endregion
                                                        #region ANALİZPRO
                                                        if (user.LisansDurum.AnProStart != null && user.LisansDurum.AnProStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.ANPRO = user.LisansDurum.AnPro._ToIntStr();
                                                            if (user.LisansDurum.AnProStart != null)
                                                                storeuser.ANPROSD = user.LisansDurum.AnProStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.AnProEnd != null)
                                                                storeuser.ANPROED = user.LisansDurum.AnProEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.ANPRO = false._ToIntStr();
                                                            storeuser.ANPROSD = null;
                                                            storeuser.ANPROED = null;
                                                        }
                                                        #endregion

                                                        #region SENTIL1
                                                        if (user.LisansDurum.SentiL1Start != null && user.LisansDurum.SentiL1Start.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.SENTIL1 = user.LisansDurum.SentiL1._ToIntStr();
                                                            if (user.LisansDurum.SentiL1Start != null)
                                                                storeuser.SENTIL1SD = user.LisansDurum.SentiL1Start.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.SentiL1End != null)
                                                                storeuser.SENTIL1ED = user.LisansDurum.SentiL1End.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.SENTIL1 = false._ToIntStr();
                                                            storeuser.SENTIL1SD = null;
                                                            storeuser.SENTIL1ED = null;
                                                        }
                                                        #endregion
                                                        #region SENTIL2
                                                        if (user.LisansDurum.SentiL2Start != null && user.LisansDurum.SentiL2Start.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.SENTIL2 = user.LisansDurum.SentiL2._ToIntStr();
                                                            if (user.LisansDurum.SentiL2Start != null)
                                                                storeuser.SENTIL2SD = user.LisansDurum.SentiL2Start.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.SentiL2End != null)
                                                                storeuser.SENTIL2ED = user.LisansDurum.SentiL2End.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.SENTIL2 = false._ToIntStr();
                                                            storeuser.SENTIL2SD = null;
                                                            storeuser.SENTIL2ED = null;
                                                        }
                                                        #endregion

                                                        #region MKK
                                                        if (user.LisansDurum.MKKStart != null && user.LisansDurum.MKKStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.MKK = user.LisansDurum.MKK._ToIntStr();
                                                            if (user.LisansDurum.MKKStart != null)
                                                                storeuser.MKKSD = user.LisansDurum.MKKStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.MKKEnd != null)
                                                                storeuser.MKKED = user.LisansDurum.MKKEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.MKK = false._ToIntStr();
                                                            storeuser.MKKSD = null;
                                                            storeuser.MKKED = null;
                                                        }
                                                        #endregion

                                                        #region GKKUL
                                                        if (user.LisansDurum.GKKULStart != null && user.LisansDurum.GKKULStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.GKKUL = user.LisansDurum.GKKUL._ToIntStr();
                                                            if (user.LisansDurum.GKKULStart != null)
                                                                storeuser.GKKULSD = user.LisansDurum.GKKULStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.GKKULEnd != null)
                                                                storeuser.GKKULED = user.LisansDurum.GKKULEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.GKKUL = false._ToIntStr();
                                                            storeuser.GKKULSD = null;
                                                            storeuser.GKKULED = null;
                                                        }
                                                        #endregion

                                                        #region TARAMA
                                                        if (user.LisansDurum.TaramaStart != null && user.LisansDurum.TaramaStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.TARAMA = user.LisansDurum.TARAMA._ToIntStr();
                                                            if (user.LisansDurum.TaramaStart != null)
                                                                storeuser.TARAMASD = user.LisansDurum.TaramaStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.TaramaEnd != null)
                                                                storeuser.TARAMAED = user.LisansDurum.TaramaEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.TARAMA = false._ToIntStr();
                                                            storeuser.TARAMASD = null;
                                                            storeuser.TARAMAED = null;
                                                        }
                                                        #endregion

                                                        #region CME
                                                        if (user.LisansDurum.CMEStart != null && user.LisansDurum.CMEStart.Value.Date >= DateTime.Now.Date)
                                                        {
                                                            storeuser.CME = user.LisansDurum.CME._ToIntStr();
                                                            if (user.LisansDurum.CMEStart != null)
                                                                storeuser.CMESD = user.LisansDurum.CMEStart.Value.ToString("yyyyMMdd");
                                                            if (user.LisansDurum.CMEEnd != null)
                                                                storeuser.CMEED = user.LisansDurum.CMEEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                        else
                                                        {
                                                            storeuser.CME = false._ToIntStr();
                                                            storeuser.CMESD = null;
                                                            storeuser.CMEED = null;
                                                        }
                                                        #endregion
                                                        //    if ((user.LisansDurum.ProYetkiStart.Value.Date >= DateTime.Now.Date ||
                                                        //user.LisansDurum.CepYetkiStart.Value.Date >= DateTime.Now.Date) &&
                                                        //(user.LisansDurum.YayinDurumu == false))
                                                        //    {

                                                        //    }
                                                        //}

                                                    }
                                                    else if (user.LisansDurum.YayinDurumu == false)
                                                    {
                                                        storeuser.ONOF = false._ToIntStr();
                                                        storeuser.PRO = false._ToIntStr();
                                                        storeuser.CEP = false._ToIntStr();
                                                        storeuser.KRMD1 = false._ToIntStr();
                                                        storeuser.PD1 = false._ToIntStr();
                                                        storeuser.PD1P = false._ToIntStr();
                                                        storeuser.PD2 = false._ToIntStr();
                                                        storeuser.PD2P = false._ToIntStr();
                                                        storeuser.END = false._ToIntStr();
                                                        storeuser.PIT = false._ToIntStr();
                                                        storeuser.PITE = false._ToIntStr();
                                                        storeuser.VD1 = false._ToIntStr();
                                                        storeuser.VD1P = false._ToIntStr();
                                                        storeuser.VD2 = false._ToIntStr();
                                                        storeuser.VD2P = false._ToIntStr();
                                                        storeuser.VIT = false._ToIntStr();
                                                        storeuser.BD1 = false._ToIntStr();
                                                        storeuser.BD1P = false._ToIntStr();
                                                        storeuser.BD2 = false._ToIntStr();
                                                        storeuser.ANPRO = false._ToIntStr();
                                                        storeuser.MKK = false._ToIntStr();
                                                        storeuser.GKKUL = false._ToIntStr();
                                                        storeuser.TARAMA = false._ToIntStr();
                                                        storeuser.CME = false._ToIntStr();


                                                        storeuser.SENTIL1 = false._ToIntStr();
                                                        storeuser.SENTIL2 = false._ToIntStr();
                                                        storeuser.KRMD1 = false._ToIntStr();



                                                        storeuser.PD1SD = null;
                                                        storeuser.PD1PSD = null;
                                                        storeuser.PD2SD = null;
                                                        storeuser.PD2PSD = null;
                                                        storeuser.ENDSD = null;
                                                        storeuser.PITSD = null;
                                                        storeuser.PITESD = null;
                                                        storeuser.VD1SD = null;
                                                        storeuser.VD1PSD = null;
                                                        storeuser.VD2SD = null;
                                                        storeuser.VD2PSD = null;
                                                        storeuser.BD1SD = null;
                                                        storeuser.BD1PSD = null;
                                                        storeuser.BD2SD = null;
                                                        storeuser.ANPROSD = null;

                                                        storeuser.KRMD1SD = null;
                                                        storeuser.SENTIL1SD = null;
                                                        storeuser.SENTIL2SD = null;
                                                        storeuser.PROSD = null;
                                                        storeuser.CEPSD = null;
                                                        storeuser.MKKSD = null;
                                                        storeuser.GKKULSD = null;
                                                        storeuser.TARAMASD = null;
                                                        storeuser.CMESD = null;

                                                        storeuser.PD1ED = null;
                                                        storeuser.PD1PED = null;
                                                        storeuser.PD2ED = null;
                                                        storeuser.PD2PED = null;
                                                        storeuser.ENDED = null;
                                                        storeuser.PITED = null;
                                                        storeuser.PITEED = null;
                                                        storeuser.VD1ED = null;
                                                        storeuser.VD1PED = null;
                                                        storeuser.VD2ED = null;
                                                        storeuser.VD2PED = null;
                                                        storeuser.VITED = null;
                                                        storeuser.BD1ED = null;
                                                        storeuser.BD1PED = null;
                                                        storeuser.BD2ED = null;
                                                        storeuser.ANPROED = null;
                                                        storeuser.KRMD1ED = null;
                                                        storeuser.SENTIL1ED = null;
                                                        storeuser.SENTIL2ED = null;
                                                        storeuser.PROED = null;
                                                        storeuser.CEPED = null;
                                                        storeuser.MKKED = null;
                                                        storeuser.GKKULED = null;
                                                        storeuser.TARAMAED = null;
                                                        storeuser.CMEED = null;
                                                    }
                                                    else if (user.LisansDurum.YayinDurumu == true)
                                                    {
                                                        storeuser.ONOF = user.LisansDurum.YayinDurumu._ToIntStr();
                                                        storeuser.PRO = user.LisansDurum.ProYetki._ToIntStr();
                                                        storeuser.CEP = user.LisansDurum.CepYetki._ToIntStr();
                                                        storeuser.PD1 = user.LisansDurum.PayL1._ToIntStr();
                                                        storeuser.PD1P = user.LisansDurum.PayLP._ToIntStr();
                                                        storeuser.PD2 = user.LisansDurum.PayL2._ToIntStr();
                                                        storeuser.PD2P = user.LisansDurum.Pd2P._ToIntStr();
                                                        storeuser.END = user.LisansDurum.PayX._ToIntStr();
                                                        storeuser.PIT = user.LisansDurum.PayGS._ToIntStr();
                                                        storeuser.PITE = user.LisansDurum.PITE._ToIntStr();

                                                        storeuser.VD1 = user.LisansDurum.ViopL1._ToIntStr();
                                                        storeuser.VD1P = user.LisansDurum.ViopLP._ToIntStr();
                                                        storeuser.VD2 = user.LisansDurum.ViopL2._ToIntStr();
                                                        storeuser.VD2P = user.LisansDurum.Vd2P._ToIntStr();
                                                        storeuser.VIT = user.LisansDurum.ViopGS._ToIntStr();

                                                        storeuser.BD1 = user.LisansDurum.TahvilL1._ToIntStr();
                                                        storeuser.BD1P = user.LisansDurum.TahvilLP._ToIntStr();
                                                        storeuser.BD2 = user.LisansDurum.TahvilL2._ToIntStr();
                                                        storeuser.ANPRO = user.LisansDurum.AnPro._ToIntStr();

                                                        storeuser.SENTIL1 = user.LisansDurum.SentiL1._ToIntStr();
                                                        storeuser.SENTIL2 = user.LisansDurum.SentiL2._ToIntStr();
                                                        storeuser.KRMD1 = user.LisansDurum.COMEX._ToIntStr();
                                                        storeuser.YDS = user.LisansDurum.SPI._ToIntStr();
                                                        storeuser.MKK = user.LisansDurum.MKK._ToIntStr();
                                                        storeuser.GKKUL = user.LisansDurum.GKKUL._ToIntStr();
                                                        storeuser.TARAMA = user.LisansDurum.TARAMA._ToIntStr();
                                                        storeuser.CME = user.LisansDurum.CME._ToIntStr();

                                                        if (user.LisansDurum.PayL1Start != null)
                                                            storeuser.PD1SD = user.LisansDurum.PayL1Start.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.PayLPStart != null)
                                                            storeuser.PD1PSD = user.LisansDurum.PayLPStart.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.PayL2Start != null)
                                                            storeuser.PD2SD = user.LisansDurum.PayL2Start.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.Pd2PStart != null)
                                                            storeuser.PD2PSD = user.LisansDurum.Pd2PStart.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.PayXStart != null)
                                                            storeuser.ENDSD = user.LisansDurum.PayXStart.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.PayGSStart != null)
                                                            storeuser.PITSD = user.LisansDurum.PayGSStart.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.PayPiteStart != null)
                                                            storeuser.PITESD = user.LisansDurum.PayPiteStart.Value.ToString("yyyyMMdd");

                                                        if (user.LisansDurum.ViopL1Start != null)
                                                            storeuser.VD1SD = user.LisansDurum.ViopL1Start.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.ViopLPStart != null)
                                                            storeuser.VD1PSD = user.LisansDurum.ViopLPStart.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.ViopL2Start != null)
                                                            storeuser.VD2SD = user.LisansDurum.ViopL2Start.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.Vd2PStart != null)
                                                            storeuser.VD2PSD = user.LisansDurum.Vd2PStart.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.ViopGSStart != null)
                                                            storeuser.VITSD = user.LisansDurum.ViopGSStart.Value.ToString("yyyyMMdd");

                                                        if (user.LisansDurum.TahvilL1Start != null)
                                                            storeuser.BD1SD = user.LisansDurum.TahvilL1Start.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.TahvilLPStart != null)
                                                            storeuser.BD1PSD = user.LisansDurum.TahvilLPStart.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.TahvilL2Start != null)
                                                            storeuser.BD2SD = user.LisansDurum.TahvilL2Start.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.AnProStart != null)
                                                            storeuser.ANPROSD = user.LisansDurum.AnProStart.Value.ToString("yyyyMMdd");

                                                        if (user.LisansDurum.KRMD1Start != null)
                                                            storeuser.KRMD1SD = user.LisansDurum.KRMD1Start.Value.ToString("yyyyMMdd");

                                                        if (user.LisansDurum.SentiL1Start != null)
                                                            storeuser.SENTIL1SD = user.LisansDurum.SentiL1Start.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.SentiL2Start != null)
                                                            storeuser.SENTIL2SD = user.LisansDurum.SentiL2Start.Value.ToString("yyyyMMdd");

                                                        if (user.LisansDurum.ProYetkiStart != null)
                                                            storeuser.PROSD = user.LisansDurum.ProYetkiStart.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.CepYetkiStart != null)
                                                            storeuser.CEPSD = user.LisansDurum.CepYetkiStart.Value.ToString("yyyyMMdd");


                                                        if (user.LisansDurum.PayL1End != null)
                                                            storeuser.PD1ED = user.LisansDurum.PayL1End.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.PayLPEnd != null)
                                                            storeuser.PD1PED = user.LisansDurum.PayLPEnd.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.PayL2End != null)
                                                            storeuser.PD2ED = user.LisansDurum.PayL2End.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.Pd2PEnd != null)
                                                            storeuser.PD2PED = user.LisansDurum.Pd2PEnd.Value.ToString("yyyyMMdd");

                                                        if (user.LisansDurum.PayXEnd != null)
                                                            storeuser.ENDED = user.LisansDurum.PayXEnd.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.PayGSEnd != null)
                                                            storeuser.PITED = user.LisansDurum.PayGSEnd.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.PayPiteEnd != null)
                                                            storeuser.PITEED = user.LisansDurum.PayPiteEnd.Value.ToString("yyyyMMdd");

                                                        if (user.LisansDurum.ViopL1End != null)
                                                            storeuser.VD1ED = user.LisansDurum.ViopL1End.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.ViopLPEnd != null)
                                                            storeuser.VD1PED = user.LisansDurum.ViopLPEnd.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.ViopL2End != null)
                                                            storeuser.VD2ED = user.LisansDurum.ViopL2End.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.Vd2PEnd != null)
                                                            storeuser.VD2PED = user.LisansDurum.Vd2PEnd.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.ViopGSEnd != null)
                                                            storeuser.VITED = user.LisansDurum.ViopGSEnd.Value.ToString("yyyyMMdd");

                                                        if (user.LisansDurum.TahvilL1End != null)
                                                            storeuser.BD1ED = user.LisansDurum.TahvilL1End.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.TahvilLPEnd != null)
                                                            storeuser.BD1PED = user.LisansDurum.TahvilLPEnd.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.TahvilL2End != null)
                                                            storeuser.BD2ED = user.LisansDurum.TahvilL2End.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.AnProEnd != null)
                                                            storeuser.ANPROED = user.LisansDurum.AnProEnd.Value.ToString("yyyyMMdd");


                                                        if (user.LisansDurum.KRMD1End != null)
                                                            storeuser.KRMD1ED = user.LisansDurum.KRMD1End.Value.ToString("yyyyMMdd");

                                                        if (user.LisansDurum.SentiL1End != null)
                                                            storeuser.SENTIL1ED = user.LisansDurum.SentiL1End.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.SentiL2End != null)
                                                            storeuser.SENTIL2ED = user.LisansDurum.SentiL2End.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.ProYetkiEnd != null)
                                                            storeuser.PROED = user.LisansDurum.ProYetkiEnd.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.CepYetkiEnd != null)
                                                            storeuser.CEPED = user.LisansDurum.CepYetkiEnd.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.MKKEnd != null)
                                                            storeuser.MKKED = user.LisansDurum.MKKEnd.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.GKKULEnd != null)
                                                            storeuser.GKKULED = user.LisansDurum.GKKULEnd.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.TaramaEnd != null)
                                                            storeuser.TARAMAED = user.LisansDurum.TaramaEnd.Value.ToString("yyyyMMdd");
                                                        if (user.LisansDurum.CMEEnd != null)
                                                            storeuser.CMEED = user.LisansDurum.CMEEnd.Value.ToString("yyyyMMdd");
                                                    }

                                                    #endregion
                                                    responsestr = new JavaScriptSerializer().Serialize(storeuser);
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;
                                                }
                                                else
                                                {
                                                    sonuc.Kod = "101";
                                                    sonuc.Aciklama = "Kullanıcı Bulunamadı.";
                                                    sonuc.Status = "Başarısız";
                                                }


                                            }
                                            else
                                            {
                                                sonuc.Kod = "101";
                                                sonuc.Aciklama = "Çalışan Bulunamadı.";
                                                sonuc.Status = "Başarısız";
                                            }

                                            var jsn = new JavaScriptSerializer().Serialize(sonuc);
                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(jsn._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }
                                        #endregion

                                }

                            }
                            #endregion

                            #region INFOANALIZ
                            if (fieldarray[0] == "INFOANALIZ")
                            {

                                var username = "";
                                var password = "";
                                var startdate = "";
                                var enddate = "";
                                var currentDate = "";
                                switch (fieldarray[1])
                                {
                                    #region MODELPORTFOY
                                    case "MODELPORTFOY":

                                        for (int i = 2; i < fieldarray.Length; i++)
                                        {
                                            var splitarray = fieldarray[i].Split('=');
                                            switch (splitarray[0].Trim())
                                            {
                                                case "USERNAME": username = splitarray[1].Trim(); break;
                                                case "PASSWORD": password = splitarray[1].Trim(); break;
                                            }
                                        }
                                        var modelPortfoySonuc = new Sonuc();
                                        if (username == "" || password == "")
                                        {
                                            var aciklama =
                                            modelPortfoySonuc.Kod = "102";
                                            modelPortfoySonuc.Aciklama = "USERNAME ve PASSWORD boş olamaz.";
                                            modelPortfoySonuc.Status = "Eksik Bilgi";
                                            var json = new JavaScriptSerializer().Serialize(modelPortfoySonuc);
                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }
                                        var calPassword = MyTools.Sifreleme.Encryp(password);
                                        //var cal = crm.Calisans.FirstOrDefault(x => x.UserName == username && x.Password == calPassword);

                                        if (crm.Calisans.Where(x => x.UserName == username && x.Password == calPassword).Any())
                                        {
                                            List<ModelPortfoyDto> modelPortfoyDto = new List<ModelPortfoyDto>();
                                            var modelPortfoy = crm.ModelPortfoys.ToList();
                                            if (modelPortfoy == null || modelPortfoy.Count == 0)
                                            {
                                                modelPortfoySonuc.Kod = "101";
                                                modelPortfoySonuc.Aciklama = "Model Portföy Bulunamadı.";
                                                modelPortfoySonuc.Status = "Başarısız";
                                            }
                                            else
                                            {
                                                foreach (var item in modelPortfoy)
                                                {
                                                    var hedefFiyat = item.HedefFiyat.ToString();

                                                    var hedefFiyatFormat = hedefFiyat.Substring(0, hedefFiyat.Length - 2);
                                                    var fiyat = item.Fiyat.ToString();

                                                    var fiyatFormat = fiyat.Substring(0, fiyat.Length - 2);

                                                    modelPortfoyDto.Add(new ModelPortfoyDto
                                                    {
                                                        Id = item.id,
                                                        Sembol = item.SembolName,
                                                        Fiyat = fiyatFormat,
                                                        HedefFiyat = hedefFiyatFormat,
                                                        Tarih = item.Date.Value.Date.ToString("yyyyMMdd")
                                                    });

                                                }
                                                responsestr = new JavaScriptSerializer().Serialize(modelPortfoyDto);
                                                IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                                IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                return;
                                            }


                                        }
                                        else
                                        {
                                            modelPortfoySonuc.Kod = "101";
                                            modelPortfoySonuc.Aciklama = "Çalışan Bulunamadı.";
                                            modelPortfoySonuc.Status = "Başarısız";
                                        }


                                        break;
                                    #endregion

                                    #region TEKNIKANALIZ
                                    case "TEKNIKANALIZ":
                                        for (int i = 2; i < fieldarray.Length; i++)
                                        {
                                            var splitarray = fieldarray[i].Split('=');
                                            switch (splitarray[0].Trim())
                                            {
                                                case "USERNAME": username = splitarray[1].Trim(); break;
                                                case "PASSWORD": password = splitarray[1].Trim(); break;
                                                case "STARTDATE": startdate = splitarray[1].Trim(); break;
                                                case "ENDDATE": enddate = splitarray[1].Trim(); break;
                                                case "DATE": currentDate = splitarray[1].Trim(); break;
                                            }
                                        }
                                        var teknikAnalizSonuc = new Sonuc();
                                        if (username == "" || password == "")
                                        {

                                            teknikAnalizSonuc.Kod = "102";
                                            teknikAnalizSonuc.Aciklama = "USERNAME ve PASSWORD boş olamaz.";
                                            teknikAnalizSonuc.Status = "Eksik Bilgi";
                                            var json = new JavaScriptSerializer().Serialize(teknikAnalizSonuc);
                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }

                                        var calisanPassword = MyTools.Sifreleme.Encryp(password);
                                        if (crm.Calisans.Where(x => x.UserName == username && x.Password == calisanPassword).Any())
                                        {
                                            if (currentDate.Length > 1 && startdate.Length == 0 && enddate.Length == 0)
                                            {
                                                #region Verilen Tarihte ki Teknik Analiz Raporu

                                                int day;
                                                int month;
                                                int year;
                                                try
                                                {
                                                    var dateFormat = DateTime.ParseExact(currentDate, "yyyyMMdd", null);
                                                    day = dateFormat.Day;
                                                    month = dateFormat.Month;
                                                    year = dateFormat.Year;
                                                    Console.WriteLine(dateFormat.ToString());
                                                }
                                                catch (Exception)
                                                {
                                                    var teknikAnalizSonuc2 = new Sonuc();

                                                    teknikAnalizSonuc2.Kod = "102";
                                                    teknikAnalizSonuc2.Aciklama = "Tarih Formatı Hatalı";
                                                    teknikAnalizSonuc2.Status = "Eksik Bilgi";
                                                    var json = new JavaScriptSerializer().Serialize(teknikAnalizSonuc2);
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;
                                                }




                                                List<TeknikAnalizRapor> teknikAnalizRaporList = new List<TeknikAnalizRapor>();
                                                var teknikAnaliz = crm.TeknikAnalizRapors
                                                    .Where(x => x.Tarih.Value.Day == day && x.Tarih.Value.Month == month && x.Tarih.Value.Year == year)
                                                    .OrderByDescending(x => x.id).ToList();
                                                if (teknikAnaliz == null || teknikAnaliz.Count == 0)
                                                {
                                                    teknikAnalizSonuc.Kod = "101";
                                                    teknikAnalizSonuc.Aciklama = "Teknik Analiz Bulunamadı.";
                                                    teknikAnalizSonuc.Status = "Başarısız";
                                                    var json = new JavaScriptSerializer().Serialize(teknikAnalizSonuc);
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;
                                                }
                                                else
                                                {
                                                    List<Stock> stockList;
                                                    foreach (var item in teknikAnaliz)
                                                    {

                                                        if ((item.Stocks != "" || item != null) && item.Stocks.Contains(","))
                                                        {
                                                            List<Stock> itemStockList = new List<Stock>();
                                                            var stocks = item.Stocks.Split(',');
                                                            var stocksCount = stocks.Count();
                                                            for (int i = 0; i < stocksCount; i++)
                                                            {
                                                                itemStockList.Add(new Stock { StockName = stocks[i] });
                                                            }
                                                            stockList = itemStockList;

                                                        }
                                                        else if (item.Stocks.Length > 1)
                                                        {
                                                            List<Stock> itemStocklist = new List<Stock>();
                                                            var stock = new Stock { StockName = item.Stocks };
                                                            itemStocklist.Add(stock);
                                                            stockList = itemStocklist;
                                                        }
                                                        else
                                                        {
                                                            stockList = null;
                                                        }
                                                        teknikAnalizRaporList.Add(
                                                            new TeknikAnalizRapor
                                                            {
                                                                Id = item.id,
                                                                Tarih = item.Tarih.Value.Date.ToString("yyyyMMdd"),
                                                                Baslik = item.Baslik,
                                                                Piyasa = item.Piyasa,
                                                                Stocks = stockList,
                                                                Link = item.link,
                                                                Icerik = item.icerik
                                                            });



                                                    }
                                                    responsestr = new JavaScriptSerializer().Serialize(teknikAnalizRaporList);
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;
                                                }
                                                #endregion
                                            }
                                            else if (startdate.Length > 1 && enddate.Length > 1 && currentDate.Length == 0)
                                            {
                                                #region iki tarih arası Teknik Analiz Raporu

                                                int startDay;
                                                int startMonth;
                                                int startYear;

                                                int endDay;
                                                int endMonth;
                                                int endYear;
                                                DateTime myStartDate = DateTime.ParseExact(startdate, "yyyyMMdd", null);
                                                DateTime myEndDate = DateTime.ParseExact(enddate, "yyyyMMdd", null);
                                                try
                                                {
                                                    var startDateFormat = DateTime.ParseExact(startdate, "yyyyMMdd", null);
                                                    var endDateFormat = DateTime.ParseExact(enddate, "yyyyMMdd", null);
                                                    startDay = startDateFormat.Day;
                                                    startMonth = startDateFormat.Month;
                                                    startYear = startDateFormat.Year;

                                                    endDay = endDateFormat.Day;
                                                    endMonth = endDateFormat.Month;
                                                    endYear = endDateFormat.Year;
                                                }
                                                catch (Exception)
                                                {
                                                    //var teknikAnalizSonuc3 = new Sonuc();

                                                    teknikAnalizSonuc.Kod = "102";
                                                    teknikAnalizSonuc.Aciklama = "Tarih Formatı Hatalı";
                                                    teknikAnalizSonuc.Status = "Eksik Bilgi";
                                                    var json = new JavaScriptSerializer().Serialize(teknikAnalizSonuc);
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;
                                                }




                                                List<TeknikAnalizRapor> teknikAnalizRaporList = new List<TeknikAnalizRapor>();
                                                var teknikAnaliz = crm.TeknikAnalizRapors.Where(x => x.Tarih > myStartDate && x.Tarih < myEndDate).OrderByDescending(x => x.id).ToList();
                                                if (teknikAnaliz == null || teknikAnaliz.Count == 0)
                                                {
                                                    teknikAnalizSonuc.Kod = "101";
                                                    teknikAnalizSonuc.Aciklama = "Teknik Analiz Bulunamadı.";
                                                    teknikAnalizSonuc.Status = "Başarısız";
                                                    var json = new JavaScriptSerializer().Serialize(teknikAnalizSonuc);
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;
                                                }
                                                else
                                                {
                                                    List<Stock> stockList;
                                                    foreach (var item in teknikAnaliz)
                                                    {

                                                        if ((item.Stocks != "" || item != null) && item.Stocks.Contains(","))
                                                        {
                                                            List<Stock> itemStockList = new List<Stock>();
                                                            var stocks = item.Stocks.Split(',');
                                                            var stocksCount = stocks.Count();
                                                            for (int i = 0; i < stocksCount; i++)
                                                            {
                                                                itemStockList.Add(new Stock { StockName = stocks[i] });
                                                            }
                                                            stockList = itemStockList;

                                                        }
                                                        else if (item.Stocks.Length > 1)
                                                        {
                                                            List<Stock> itemStocklist = new List<Stock>();
                                                            var stock = new Stock { StockName = item.Stocks };
                                                            itemStocklist.Add(stock);
                                                            stockList = itemStocklist;
                                                        }
                                                        else
                                                        {
                                                            stockList = null;
                                                        }
                                                        teknikAnalizRaporList.Add(
                                                            new TeknikAnalizRapor
                                                            {
                                                                Id = item.id,
                                                                Tarih = item.Tarih.Value.Date.ToString("yyyyMMdd"),
                                                                Baslik = item.Baslik,
                                                                Piyasa = item.Piyasa,
                                                                Stocks = stockList,
                                                                Link = item.link,
                                                                Icerik = item.icerik
                                                            });



                                                    }
                                                    responsestr = new JavaScriptSerializer().Serialize(teknikAnalizRaporList);
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                teknikAnalizSonuc.Kod = "101";
                                                teknikAnalizSonuc.Aciklama = "Request Syntax Hatası.";
                                                teknikAnalizSonuc.Status = "Başarısız";
                                                var json = new JavaScriptSerializer().Serialize(teknikAnalizSonuc);
                                                IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            teknikAnalizSonuc.Kod = "101";
                                            teknikAnalizSonuc.Aciklama = "Çalışan Bulunamadı.";
                                            teknikAnalizSonuc.Status = "Başarısız";

                                            responsestr = new JavaScriptSerializer().Serialize(teknikAnalizSonuc);
                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }

                                        break;
                                    #endregion

                                    #region SENTIMENTALGO
                                    case "SENTIMENTALGO":
                                        for (int i = 2; i < fieldarray.Length; i++)
                                        {
                                            var splitarray = fieldarray[i].Split('=');
                                            switch (splitarray[0].Trim())
                                            {
                                                case "USERNAME": username = splitarray[1].Trim(); break;
                                                case "PASSWORD": password = splitarray[1].Trim(); break;
                                                case "STARTDATE": startdate = splitarray[1].Trim(); break;
                                                case "ENDDATE": enddate = splitarray[1].Trim(); break;
                                                case "DATE": currentDate = splitarray[1].Trim(); break;
                                            }
                                        }
                                        var sentimentAlgoSonuc = new Sonuc();
                                        if (username == "" || password == "")
                                        {

                                            sentimentAlgoSonuc.Kod = "102";
                                            sentimentAlgoSonuc.Aciklama = "USERNAME ve PASSWORD boş olamaz.";
                                            sentimentAlgoSonuc.Status = "Eksik Bilgi";
                                            var json = new JavaScriptSerializer().Serialize(sentimentAlgoSonuc);
                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }

                                        var calPass = MyTools.Sifreleme.Encryp(password);
                                        if (crm.Calisans.Where(x => x.UserName == username && x.Password == calPass).Any())
                                        {
                                            if (currentDate.Length > 1 && startdate.Length == 0 && enddate.Length == 0)
                                            {
                                                #region Verilen Tarihte ki SetilmentAlgo

                                                int day;
                                                int month;
                                                int year;
                                                try
                                                {
                                                    var dateFormat = DateTime.ParseExact(currentDate, "yyyyMMdd", null);
                                                    day = dateFormat.Day;
                                                    month = dateFormat.Month;
                                                    year = dateFormat.Year;
                                                    Console.WriteLine(dateFormat.ToString());
                                                }
                                                catch (Exception)
                                                {
                                                    var sentimentAlgoSonuc2 = new Sonuc();

                                                    sentimentAlgoSonuc2.Kod = "102";
                                                    sentimentAlgoSonuc2.Aciklama = "Tarih Formatı Hatalı";
                                                    sentimentAlgoSonuc2.Status = "Eksik Bilgi";
                                                    var json = new JavaScriptSerializer().Serialize(sentimentAlgoSonuc2);
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;
                                                }




                                                var sentimentAlgo = crm.SentimentAlgos
                                                    .Where(x => x.OlusturmaTar.Value.Day == day && x.OlusturmaTar.Value.Month == month && x.OlusturmaTar.Value.Year == year)
                                                    .OrderByDescending(x => x.id).ToList();
                                                if (sentimentAlgo == null || sentimentAlgo.Count == 0)
                                                {
                                                    sentimentAlgoSonuc.Kod = "101";
                                                    sentimentAlgoSonuc.Aciklama = "SentimentAlgo Bulunamadı.";
                                                    sentimentAlgoSonuc.Status = "Başarısız";
                                                    var json = new JavaScriptSerializer().Serialize(sentimentAlgoSonuc);
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;
                                                }
                                                else
                                                {





                                                    List<SentimentSembol> sentimentSembolList = new List<SentimentSembol>();
                                                    List<SentimentAlgo> sentimentAlgoList = new List<SentimentAlgo>();
                                                    var sentimentSembols = crm.SentimentSembols;
                                                    foreach (var item in sentimentAlgo)
                                                    {
                                                        var query = sentimentSembols.Where(x => x.SentimentAlgoId == item.id).ToList();
                                                        if (query.Count == 0)
                                                        {
                                                            sentimentAlgoList.Add(
                                                                new SentimentAlgo
                                                                {
                                                                    Id = item.id,
                                                                    CreatedDate = item.OlusturmaTar.Value.Date.ToString("yyyyMMdd"),
                                                                    Baslik = item.Baslik,
                                                                    Icerik = item.Icerik.Replace("\r\n\r\n", " "),
                                                                    Link = item.Link,
                                                                    Symbols = null,
                                                                    UpdatedDate = item.GuncellemeTar.Value.Date.ToString("yyyyMMdd"),
                                                                }
                                                                );

                                                        }

                                                        if (query.Count > 0)
                                                        {
                                                            for (int i = 0; i < query.Count; i++)
                                                            {
                                                                sentimentSembolList.Add(
                                                                new SentimentSembol
                                                                {
                                                                    Id = query[i].id,
                                                                    Sembol = query[i].Sembol,
                                                                    SonFiyat = query[i].SonFiyat,
                                                                    SentimentAlgoId = query[i].SentimentAlgoId
                                                                });
                                                            }


                                                            sentimentAlgoList.Add(
                                                                new SentimentAlgo
                                                                {
                                                                    Id = item.id,
                                                                    CreatedDate = item.OlusturmaTar.Value.Date.ToString("yyyyMMdd"),
                                                                    Baslik = item.Baslik,
                                                                    Icerik = item.Icerik.Replace("\r\n\r\n", " "),
                                                                    Link = item.Link,
                                                                    Symbols = sentimentSembolList.ToList(),
                                                                    UpdatedDate = item.GuncellemeTar.Value.Date.ToString("yyyyMMdd"),

                                                                });
                                                        }

                                                    }

                                                    responsestr = new JavaScriptSerializer().Serialize(sentimentAlgoList);
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;
                                                }
                                                #endregion
                                            }
                                            else if (currentDate.Length == 0 && startdate.Length > 1 && enddate.Length > 1)
                                            {
                                                #region iki tarih arası SentimetAlgo

                                                int startDay;
                                                int startMonth;
                                                int startYear;

                                                int endDay;
                                                int endMonth;
                                                int endYear;
                                                DateTime myStartDate = DateTime.ParseExact(startdate, "yyyyMMdd", null);
                                                DateTime myEndDate = DateTime.ParseExact(enddate, "yyyyMMdd", null);
                                                try
                                                {
                                                    var startDateFormat = DateTime.ParseExact(startdate, "yyyyMMdd", null);
                                                    var endDateFormat = DateTime.ParseExact(enddate, "yyyyMMdd", null);
                                                    startDay = startDateFormat.Day;
                                                    startMonth = startDateFormat.Month;
                                                    startYear = startDateFormat.Year;

                                                    endDay = endDateFormat.Day;
                                                    endMonth = endDateFormat.Month;
                                                    endYear = endDateFormat.Year;
                                                }
                                                catch (Exception)
                                                {
                                                    //var teknikAnalizSonuc3 = new Sonuc();

                                                    sentimentAlgoSonuc.Kod = "102";
                                                    sentimentAlgoSonuc.Aciklama = "Tarih Formatı Hatalı";
                                                    sentimentAlgoSonuc.Status = "Eksik Bilgi";
                                                    var json = new JavaScriptSerializer().Serialize(sentimentAlgoSonuc);
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;
                                                }




                                                var sentimentAlgo = crm.SentimentAlgos
                                                   .Where(x => x.OlusturmaTar.Value.Date >= myStartDate && x.OlusturmaTar.Value.Date <= myEndDate)
                                                   .OrderByDescending(x => x.id).ToList();
                                                if (sentimentAlgo == null || sentimentAlgo.Count == 0)
                                                {
                                                    sentimentAlgoSonuc.Kod = "101";
                                                    sentimentAlgoSonuc.Aciklama = "SentimentAlgo Bulunamadı.";
                                                    sentimentAlgoSonuc.Status = "Başarısız";
                                                    responsestr = new JavaScriptSerializer().Serialize(sentimentAlgoSonuc);
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;
                                                }
                                                else
                                                {

                                                    List<SentimentSembol> sentimentSembolList = new List<SentimentSembol>();
                                                    List<SentimentAlgo> sentimentAlgoList = new List<SentimentAlgo>();
                                                    var sentimentSembols = crm.SentimentSembols;
                                                    foreach (var item in sentimentAlgo)
                                                    {
                                                        var query = sentimentSembols.Where(x => x.SentimentAlgoId == item.id).ToList();
                                                        if (query.Count == 0)
                                                        {
                                                            sentimentAlgoList.Add(
                                                                new SentimentAlgo
                                                                {
                                                                    Id = item.id,
                                                                    CreatedDate = item.OlusturmaTar.Value.Date.ToString("yyyyMMdd"),
                                                                    Baslik = item.Baslik,
                                                                    Icerik = item.Icerik.Replace("\r\n\r\n", " "),
                                                                    Link = item.Link,
                                                                    Symbols = null,
                                                                    UpdatedDate = item.GuncellemeTar.Value.Date.ToString("yyyyMMdd"),
                                                                }
                                                                );

                                                        }

                                                        if (query.Count > 0)
                                                        {
                                                            for (int i = 0; i < query.Count; i++)
                                                            {
                                                                sentimentSembolList.Add(
                                                                new SentimentSembol
                                                                {
                                                                    Id = query[i].id,
                                                                    Sembol = query[i].Sembol,
                                                                    SonFiyat = query[i].SonFiyat,
                                                                    SentimentAlgoId = query[i].SentimentAlgoId
                                                                });
                                                            }


                                                            sentimentAlgoList.Add(
                                                                new SentimentAlgo
                                                                {
                                                                    Id = item.id,
                                                                    CreatedDate = item.OlusturmaTar.Value.Date.ToString("yyyyMMdd"),
                                                                    Baslik = item.Baslik,
                                                                    Icerik = item.Icerik.Replace("\r\n\r\n", " "),
                                                                    Link = item.Link,
                                                                    Symbols = sentimentSembolList.ToList(),
                                                                    UpdatedDate = item.GuncellemeTar.Value.Date.ToString("yyyyMMdd"),

                                                                });
                                                        }

                                                    }

                                                    responsestr = new JavaScriptSerializer().Serialize(sentimentAlgoList);
                                                    IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                                    IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                    return;

                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                sentimentAlgoSonuc.Kod = "101";
                                                sentimentAlgoSonuc.Aciklama = "Request Syntax Hatası.";
                                                sentimentAlgoSonuc.Status = "Başarısız";
                                                var json = new JavaScriptSerializer().Serialize(sentimentAlgoSonuc);
                                                IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(json._InsertHeaderHTTP());
                                                IpDeamon.Connections[e.ConnectionId].Connected = false;
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            sentimentAlgoSonuc.Kod = "101";
                                            sentimentAlgoSonuc.Aciklama = "Çalışan Bulunamadı.";
                                            sentimentAlgoSonuc.Status = "Başarısız";

                                            responsestr = new JavaScriptSerializer().Serialize(sentimentAlgoSonuc);
                                            IpDeamon.Connections[e.ConnectionId].DataToSendB = Latin5.GetBytes(responsestr._InsertHeaderHTTP());
                                            IpDeamon.Connections[e.ConnectionId].Connected = false;
                                            return;
                                        }

                                        break;

                                }
                                #endregion
                            }
                            #endregion
                        }

                    }
                    catch (Exception ex) { formListeTransaction("HATA;IpDeamon_OnDataIn" + ex.Message); }
                });
            }
            catch { }
        }
        public void CalisanlaraKomutGonder(string komut)
        {
            try
            {
                if (IpDeamonForClilents.Connections.Count > 0)
                {

                    foreach (Connection cal in IpDeamonForClilents.Connections.Values)
                    {


                        if (cal.Connected)
                            cal.DataToSend = komut + (char)3;
                    }


                }


            }
            catch (Exception ex)
            {
                MyTools.logyaz("HATA;CalisanlaraKomutGonder" + ex.Message);            
            }

        }

        #region MenuClicks

        private void baglantiAyarlariToolStripMenuItem_Click(object sender, EventArgs e)
        {

            formServerAyarlar frm = new formServerAyarlar();
            frm.txtClientPort.Text = IpDeamonForClilents.LocalPort.ToString();
            frm.txtServisPort.Text = IpDeamon.LocalPort.ToString();
            frm.ShowDialog();

        }
        #endregion

        private void kullaniciListesiOkuToolStripMenuItem_Click(object sender, EventArgs e)
        {

            formIdealListeOku frm = new formIdealListeOku();
            frm.Show();
        }

        private void IpDeamonForClilents_OnDataIn(object sender, nsoftware.IPWorks.IpdaemonDataInEventArgs e)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                var data = e.Text.Split((char)3);

                formListeTransaction(data[0]);
                foreach (var d in data)
                {
                    if (string.IsNullOrEmpty(d)) continue; // son (char)3'ten sonra kalan boş parça
                    try
                    {
                        if (d.StartsWith("Connect"))
                        {
                            var satir = d.Split('|');

                            DictionaryCalisan.Add(e.ConnectionId, satir[1]);
                            listBoxClientList.Items.Add(satir[1]);
                            formListeTransaction(satir[1] + " Servera Bağlandı");

                        }
                        else if (d.StartsWith("CreateUser"))
                        {
                            var satir = d.Split('|');
                            var id = Int32.Parse(satir[1]);
                            var usr = crm.Users.FirstOrDefault(x => x.UserID == id);
                            var response = DictionaryCalisan[e.ConnectionId] + " " + " Yeni Kullanıcı açtı ;" + " Açılan kullanıcı = " + usr.UserName;
                            formListeTransaction(response);
                            SendToSSO(Int32.Parse(satir[1]));
                            CalisanlaraKomutGonder("CreateUser|" + response);

                        }


                        else if (d.StartsWith("ChangeUser"))
                        {

                            var satir = d.Split('|');
                            var id = Int32.Parse(satir[1]);
                            var usr = crm.Users.FirstOrDefault(x => x.UserID == id);
                            var response = DictionaryCalisan[e.ConnectionId] + " Kullanıcı Bilgisi Değiştirdi; Kullanıcı= " + usr.UserName;
                            formListeTransaction(response);
                            SendToSSO(Int32.Parse(satir[1]));
                            CalisanlaraKomutGonder("CreateUser|" + response);

                        }

                        else if (d.StartsWith("SendUserInfo"))
                        {

                            var satir = d.Split('|');
                            var id = Int32.Parse(satir[1]);
                            var usr = crm.Users.FirstOrDefault(x => x.UserID == id);
                            var response = DictionaryCalisan[e.ConnectionId] + " Kullanıcı Bilgisi Değiştirdi; Kullanıcı= " + usr.UserName;
                            formListeTransaction(response);
                            SendToSSO(Int32.Parse(satir[1]));
                            CalisanlaraKomutGonder("CreateUser|" + response);


                        }
                        else if (d.StartsWith("TekMesaj"))
                        {

                            var t1 = Task.Factory.StartNew(() =>
                            {
                                var satir = d.Split('|');
                                var usr = crm.Users.FirstOrDefault(x => x.UserName == satir[1].Split(';')[0]);
                                var msj = satir[1].Split(';')[1];


                                MyTools.SendAlarmToAndroidAndIos(usr.FireBaseToken, msj);

                                formListeTransaction(DictionaryCalisan[e.ConnectionId] + " " + usr.UserName + "'a mesaj gönderdi " + "Mesaj : " + msj);
                            });
                        }
                        else if (d.StartsWith("TopluMesaj"))
                        {

                            var t1 = Task.Factory.StartNew(() =>
                            {
                                var satir = d.Split('|');
                                var msj = satir[1];
                                MyTools.SendAlarmMulti(msj);

                                formListeTransaction(DictionaryCalisan[e.ConnectionId] + " Tüm Kullanıcılara Mesaj Gönderdi " + "Mesaj : " + msj);

                            });
                        }
                        else if (d.StartsWith("PUSH"))
                        {
                            var t1 = Task.Factory.StartNew(() =>
                            {

                                var satir = d.Split('|');
                                var tip = satir[1];
                                var id = satir[2]._ToInt();

                                var mesaj = crm.Duyurulars.FirstOrDefault(x => x.id == id);
                                if (tip == "1")
                                {
                                    MyTools.SendAlarmMulti(mesaj.Mesaj);
                                    formListeTransaction(DictionaryCalisan[e.ConnectionId] + " Tüm Kullanıcılara Push Mesaj Gönderdi " + "Mesaj : " + mesaj.Mesaj);
                                }
                                else
                                {
                                    formListeTransaction(DictionaryCalisan[e.ConnectionId] + " Push Mesaı için düzeltme gönderdi " + "Mesaj : " + mesaj.Mesaj);
                                }
                                SendToCepServerDuyuru(id, tip);

                            });

                        }

                        #region OBSERVE
                        else if (d.StartsWith("OBSERVE"))
                        {

                            var t1 = Task.Factory.StartNew(() =>
                            {

                                var liste = IpdaemonforCepServers.Connections.Values.ToList();
                                foreach (Connection con in liste)
                                {
                                    var count = 1;
                                    var sb = new StringBuilder();
                                    if (con.Connected)
                                    {
                                        sb.Append("OBSERVE");
                                        sb.Append("|" + count.ToString());
                                        con.DataToSend = sb.ToString() + (char)3;
                                        MyTools.logyaz(sb.ToString());
                                        count++;

                                    }


                                }
                            });

                            formListeTransaction(DictionaryCalisan[e.ConnectionId] + " istatistikleri görüntüledi");
                        }
                        #endregion
                        #region TeknikAnalizRapor
                        else if (d.StartsWith("CreateTeknikRapor"))
                        {
                            var satir = d.Split('|');
                            var id = Int32.Parse(satir[1]);
                            //var rapor = crm.TeknikAnalizRapors.FirstOrDefault(x => x.id == id);
                            ////TeknikRaporSendToCepServer("Insert", rapor.id);//öfk
                            //TeknikRaporSendToSSO("Insert", rapor.id).GetAwaiter();
                            //var response = DictionaryCalisan[e.ConnectionId] + " " + " Yeni Teknik Rapor Gönderdi ;" + " ID = " + rapor.id;
                            Task.Run(() => TeknikRaporSsoSenkronEt(id, true));
                            var response = DictionaryCalisan[e.ConnectionId] + " " + " Yeni Teknik Rapor Gönderdi ;" + " ID = " + id;
                            formListeTransaction(response);
                            CalisanlaraKomutGonder("CreateTeknikRapor|" + response);

                        }
                        else if (d.StartsWith("UpdateTeknikRapor"))
                        {
                            var satir = d.Split('|');
                            var id = Int32.Parse(satir[1]);
                            //var rapor = crm.TeknikAnalizRapors.FirstOrDefault(x => x.id == id);
                            ////TeknikRaporSendToCepServer("Insert", rapor.id);
                            //TeknikRaporSendToSSO("Insert", rapor.id).GetAwaiter();
                            //var response = DictionaryCalisan[e.ConnectionId] + " " + " Teknik Rapor Güncelledi ;" + " ID = " + rapor.id;
                            Task.Run(() => TeknikRaporSsoSenkronEt(id, true));
                            var response = DictionaryCalisan[e.ConnectionId] + " " + " Yeni Teknik Rapor Gönderdi ;" + " ID = " + id;
                            formListeTransaction(response);
                            CalisanlaraKomutGonder("UpdateTeknikRapor|" + response);

                        }
                        else if (d.StartsWith("DeleteTeknikRapor"))
                        {
                            var satir = d.Split('|');
                            var id = Int32.Parse(satir[1]);

                            //TeknikRaporSendToCepServer("Delete", id);
                            TeknikRaporSendToSSO("Delete", id).GetAwaiter();
                            var response = DictionaryCalisan[e.ConnectionId] + " " + " Teknik Rapor Sildi ;" + " ID = " + id;
                            formListeTransaction(response);
                            CalisanlaraKomutGonder("DeleteTeknikRapor|" + response);
                        }
                        #endregion
                        #region SentimentAlgo
                        else if (d.StartsWith("CreateSentimentAlgo"))
                        {
                            var satir = d.Split('|');
                            var id = Int32.Parse(satir[1]);
                            var sAlgo = crm.SentimentAlgos.FirstOrDefault(x => x.id == id);
                            SentimentAlgoSendToCepServer("Insert", sAlgo.id);
                            var response = DictionaryCalisan[e.ConnectionId] + " " + " Yeni Sentiment Algo Gönderdi ;" + " ID = " + sAlgo.id;
                            formListeTransaction(response);
                            CalisanlaraKomutGonder("CreateSentimentAlgo|" + response);

                        }
                        else if (d.StartsWith("UpdateSentimentAlgo"))
                        {
                            var satir = d.Split('|');
                            var id = Int32.Parse(satir[1]);
                            var sAlgo = crm.SentimentAlgos.FirstOrDefault(x => x.id == id);
                            SentimentAlgoSendToCepServer("Insert", sAlgo.id);
                            var response = DictionaryCalisan[e.ConnectionId] + " " + " Sentiment Algo Güncelledi ;" + " ID = " + sAlgo.id;
                            formListeTransaction(response);
                            CalisanlaraKomutGonder("UpdateSentimentAlgo|" + response);

                        }
                        else if (d.StartsWith("DeleteSentimentAlgo"))
                        {
                            var satir = d.Split('|');
                            var id = Int32.Parse(satir[1]);
                            SentimentAlgoSendToCepServer("Delete", id);
                            var response = DictionaryCalisan[e.ConnectionId] + " " + " Sentiment Algo Sildi ;" + " ID = " + id;
                            formListeTransaction(response);
                            CalisanlaraKomutGonder("DeleteSentimentAlgo|" + response);
                        }
                        #endregion
                        #region ModelPortFoy

                        else if (d.StartsWith("UpdateModelPortfoy"))
                        {
                            var t1 = Task.Factory.StartNew(() =>
                            {

                                var satir = d.Split('|');
                                var idx = satir[1]._ToInt();

                                var mdp = crm.ModelPortfoys.FirstOrDefault(x => x.id == idx);
                                formListeTransaction(DictionaryCalisan[e.ConnectionId] + " Model Porföy Girişi Yaptı" + "Sembol : " + mdp.SembolName + " Fiyat : " + mdp.Fiyat + " Hedef Fiyat : " + mdp.HedefFiyat);

                                var sb = new StringBuilder();
                                sb.Append("UpdateModelPortfoy");
                                sb.Append("|" + mdp.id);
                                sb.Append("|" + mdp.SembolName);
                                sb.Append("|" + mdp.Date.Value.ToString("yyyyMMddHHmmss"));
                                sb.Append("|" + mdp.Fiyat);
                                sb.Append("|" + mdp.HedefFiyat);
                                SendToCepServerModelPortfoy(sb.ToString());

                            });
                        }
                        else if (d.StartsWith("DeleteModelPortfoy"))
                        {
                            var t1 = Task.Factory.StartNew(() =>
                            {

                                var satir = d.Split('|');
                                var idx = satir[1]._ToInt();
                                var sembolname = satir[2];
                                var fiyat = satir[3];
                                var hedeffiyat = satir[4];
                                formListeTransaction(DictionaryCalisan[e.ConnectionId] + " Model Porföy Silme Gönderdi" + "Sembol : " + sembolname + " Fiyat : " + fiyat + " Hedef Fiyat : " + hedeffiyat);
                                var sb = new StringBuilder();
                                sb.Append("DeleteModelPortfoy");
                                sb.Append("|" + idx);
                                SendToCepServerModelPortfoy(sb.ToString());

                            });
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        // Bir mesajdaki hata aynı paketteki diğer mesajları düşürmesin
                        MyTools.logyaz("HATA;IpDeamonForClilents_OnDataIn Mesaj=" + d + " " + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                MyTools.logyaz("HATA;IpDeamonForClilents_OnDataIn " + ex.Message);
              
            }

        }

        private void IpDeamonForClilents_OnDisconnected(object sender, nsoftware.IPWorks.IpdaemonDisconnectedEventArgs e)
        {
            if (DictionaryCalisan.ContainsKey(e.ConnectionId))
            {
                listBoxClientList.Items.Remove(DictionaryCalisan[e.ConnectionId]);
                formListeTransaction(DictionaryCalisan[e.ConnectionId] + " Serverdan Ayrıldı");
                DictionaryCalisan.Remove(e.ConnectionId);
            }
        }


        public async void SendToSSO(int id)
        {
            try
            {
                using (var crm = new crmDFNDataContext())
                {
                var user = crm.Users.FirstOrDefault(x => x.UserID == id);

                if (user == null)
                {
                    formListeTransaction(id.ToString() + "numaralı ID bulunamadı");
                    return;
                }


                var sb = new StringBuilder();


                #region AKYATIRIM

                //if (MyTools.KurumKod == "10011")
                //{
                //    sb.Append("http://" + ssoip + ":" + ssoport + "/DFN?CRM?CRM?" + user.UserName + "?");
                //    // sb.Append("http://" + ssoip + ":" + ssoport + "/DFN?CRM?CRM?" + user.tckno + "?");
                //    sb.Append("IDEALSIFRE=" + MyTools.Sifreleme.Decryp(user.Password));
                //    sb.Append("?ACIKLAMA=" + ((user.Aciklama == null) ? "" : user.Aciklama));
                //    sb.Append("?ISIM=" + user.Name + " " + user.Surname);
                //    sb.Append("?EXPIREDATE=" + user.ExpiryDate.Value.ToString("yyyyMMdd"));
                //    sb.Append("?BASLANGICTARIH=" + user.BaslangicTarihi.Value.ToString("yyyyMMdd"));
                //    sb.Append("?ONOFF=" + ((user.LisansDurum.YayinDurumu) ? "1" : "0"));
                //    sb.Append("?PRO=" + ((user.LisansDurum.ProYetki) ? "1" : "0"));
                //    sb.Append("?CEP=" + ((user.LisansDurum.CepYetki) ? "1" : "0"));
                //    sb.Append("?ROBOT=" + ((user.LisansDurum.ROBOT) ? "1" : "0"));
                //    sb.Append("?SCMUSABLE=" + ((user.LisansDurum.SCMUsable) ? "1" : "0"));
                //    sb.Append("?SCMREALTIME=" + ((user.LisansDurum.SCMRealTıme) ? "1" : "0"));
                //    sb.Append("?SCMDOWNLOAD=" + ((user.LisansDurum.SCMDownload) ? "1" : "0"));
                //    sb.Append("?IMKBL1=" + ((user.LisansDurum.PayL1) ? "1" : "0"));
                //    sb.Append("?IMKBL1P=" + ((user.LisansDurum.PayLP) ? "1" : "0"));
                //    sb.Append("?IMKBL2=" + ((user.LisansDurum.PayL2) ? "1" : "0"));
                //    sb.Append("?EUREX=" + ((user.LisansDurum.Pd2P) ? "1" : "0"));
                //    sb.Append("?IMKBX=" + ((user.LisansDurum.PayX) ? "1" : "0"));
                //    sb.Append("?IMKBANL=" + ((user.LisansDurum.VeriAnalitik) ? "1" : "0"));
                //    sb.Append("?IMKBISL=" + ((user.LisansDurum.PayGS) ? "1" : "0"));
                //     sb.Append("?VIPL1=" + ((user.LisansDurum.ViopL1) ? "1" : "0"));
                //    sb.Append("?VIPL1P=" + ((user.LisansDurum.ViopLP) ? "1" : "0"));
                //    sb.Append("?VIPL2=" + ((user.LisansDurum.ViopL2) ? "1" : "0"));
                //   // sb.Append("?VIPL2P=" + ((user.LisansDurum.viopL2) ? "1" : "0"));
                //    sb.Append("?VIPL2P=" + ((user.LisansDurum.ViopGS) ? "1" : "0"));
                //    sb.Append("?CHIX=" + ((user.LisansDurum.Vd2P) ? "1" : "0"));
                //    sb.Append("?VIPNET=" + ((user.LisansDurum.ViopGS) ? "1" : "0"));
                //    sb.Append("?THVL1=" + ((user.LisansDurum.TahvilL1) ? "1" : "0"));
                //    sb.Append("?THVL1P=" + ((user.LisansDurum.TahvilLP) ? "1" : "0"));
                //    sb.Append("?THVL2=" + ((user.LisansDurum.TahvilL2) ? "1" : "0"));
                //    //sb.Append("?DJI=" + ((user.LisansDurum.DJI) ? "1" : "0"));
                //    //sb.Append("?SPI=" + ((user.LisansDurum.SPI) ? "1" : "0"));
                //    //sb.Append("?XETRA=" + ((user.LisansDurum.XETRA) ? "1" : "0"));
                //    //sb.Append("?CBOTM=" + ((user.LisansDurum.CBOTM) ? "1" : "0"));
                //    //sb.Append("?CBOT=" + ((user.LisansDurum.CBOT) ? "1" : "0"));
                //    //sb.Append("?CMEM=" + ((user.LisansDurum.CMEM) ? "1" : "0"));
                //    sb.Append("?CME=" + ((user.LisansDurum.CME) ? "1" : "0"));
               
                //    //sb.Append("?EUREX=" + ((user.LisansDurum.EUREX) ? "1" : "0"));
                //    //sb.Append("?NYMEX=" + ((user.LisansDurum.NYMEX) ? "1" : "0"));
                //    //sb.Append("?NYMEXM=" + ((user.LisansDurum.NYMEXM) ? "1" : "0"));
                //    sb.Append("?COMEX=" + ((user.LisansDurum.COMEX) ? "1" : "0"));
                //    sb.Append("?FUTGCK=1");
                //    sb.Append("?WINX=1");
                //    sb.Append("?PRODUCTTYPE=" + user.ProductType);
                //    sb.Append("?STATUS=" + user.StatusId);
                //}
                 if (MyTools.KurumKod == "10011")
                {
                    sb.Append("http://" + ssoip + ":" + ssoport + "/DFN?CRM?CRM?" + user.UserName + "?");
                    sb.Append("IDEALSIFRE=" + MyTools.Sifreleme.Decryp(user.Password));
                    sb.Append("?ACIKLAMA=" + ((user.Aciklama == null) ? "" : user.Aciklama));
                    sb.Append("?ISIM=" + user.Name + " " + user.Surname);
                    sb.Append("?EXPIREDATE=" + user.ExpiryDate.Value.ToString("yyyyMMdd"));
                    if (user.BaslangicTarihi == null)
                        user.BaslangicTarihi = DateTime.Now;
                    sb.Append("?BASLANGICTARIH=" + user.BaslangicTarihi.Value.ToString("yyyyMMdd"));
                    sb.Append("?ONOFF=" + ((user.LisansDurum.YayinDurumu) ? "1" : "0"));
                    sb.Append("?PRO=" + ((user.LisansDurum.ProYetki) ? "1" : "0"));
                    sb.Append("?CEP=" + ((user.LisansDurum.CepYetki) ? "1" : "0"));
                    sb.Append("?ROBOT=" + ((user.LisansDurum.ROBOT) ? "1" : "0"));

                    sb.Append("?SCMUSABLE=" + ((user.LisansDurum.SCMUsable) ? "1" : "0"));
                    sb.Append("?SCMREALTIME=" + ((user.LisansDurum.SCMRealTıme) ? "1" : "0"));
                    sb.Append("?SCMDOWNLOAD=" + ((user.LisansDurum.SCMDownload) ? "1" : "0"));
                    sb.Append("?IMKBL1=" + ((user.LisansDurum.PayL1) ? "1" : "0"));
                    sb.Append("?IMKBL1P=" + ((user.LisansDurum.PayLP) ? "1" : "0"));
                    sb.Append("?IMKBL2=" + ((user.LisansDurum.PayL2) ? "1" : "0"));
                    sb.Append("?EUREX=" + ((user.LisansDurum.Pd2P) ? "1" : "0"));
                    sb.Append("?IMKBX=" + ((user.LisansDurum.PayX) ? "1" : "0"));
                    sb.Append("?IMKBANL=" + ((user.LisansDurum.VeriAnalitik) ? "1" : "0"));
                    sb.Append("?IMKBISL=" + ((user.LisansDurum.PayGS) ? "1" : "0"));
                    sb.Append("?PITE=" + ((user.LisansDurum.PITE) ? "1" : "0"));
                    
                    sb.Append("?VIPL1=" + ((user.LisansDurum.ViopL1) ? "1" : "0"));
                    sb.Append("?VIPL1=" + ((user.LisansDurum.ViopL1) ? "1" : "0"));
                    sb.Append("?VIPL1P=" + ((user.LisansDurum.ViopLP) ? "1" : "0"));
                    sb.Append("?VIPL2=" + ((user.LisansDurum.ViopL2) ? "1" : "0"));
                    sb.Append("?CHIX=" + ((user.LisansDurum.Vd2P) ? "1" : "0"));
                    sb.Append("?VIPNET=" + ((user.LisansDurum.ViopGS) ? "1" : "0"));
                    sb.Append("?THVL1=" + ((user.LisansDurum.TahvilL1) ? "1" : "0"));
                    sb.Append("?THVL1P=" + ((user.LisansDurum.TahvilLP) ? "1" : "0"));
                    sb.Append("?THVL2=" + ((user.LisansDurum.TahvilL2) ? "1" : "0"));
                    //sb.Append("?DJI=" + ((user.LisansDurum.DJI) ? "1" : "0"));
                    sb.Append("?SPI=" + ((user.LisansDurum.SPI) ? "1" : "0"));
                    //sb.Append("?XETRA=" + ((user.LisansDurum.XETRA) ? "1" : "0"));
                    //sb.Append("?CBOTM=" + ((user.LisansDurum.CBOTM) ? "1" : "0"));
                    //sb.Append("?CBOT=" + ((user.LisansDurum.CBOT) ? "1" : "0"));
                    //sb.Append("?CMEM=" + ((user.LisansDurum.CMEM) ? "1" : "0"));
                    sb.Append("?CME=" + ((user.LisansDurum.CME) ? "1" : "0"));
                    //sb.Append("?EUREX=" + ((user.LisansDurum.EUREX) ? "1" : "0"));
                    //sb.Append("?NYMEX=" + ((user.LisansDurum.NYMEX) ? "1" : "0"));
                    //sb.Append("?NYMEXM=" + ((user.LisansDurum.NYMEXM) ? "1" : "0"));
                    sb.Append("?COMEX=" + ((user.LisansDurum.COMEX) ? "1" : "0"));
                    sb.Append("?FUTGCK=1");
                    sb.Append("?WINX=1");
                    sb.Append("?PRODUCTTYPE=" + user.ProductType);
                    sb.Append("?STATUS=" + user.StatusId);

                    var multiLisans = "";
                    if (user.LisansDurum.MKK)
                    {
                        multiLisans += "MKK;";
                    }

                    if (user.LisansDurum.GKKUL)
                    {
                        multiLisans += "GKKUL;";
                    }

                    if (!user.LisansDurum.MKK && !user.LisansDurum.GKKUL)
                    {
                        multiLisans = "";
                    }

                    sb.Append("?ML=" + multiLisans);
                }

                #endregion
                #region Digerleri

                else
                {
                    sb.Append("http://" + ssoip + ":" + ssoport + "/DFN?CRM?CRM?" + user.UserName + "?");
                    sb.Append("IDEALSIFRE=" + MyTools.Sifreleme.Decryp(user.Password));
                    sb.Append("?ACIKLAMA=" + ((user.Aciklama == null) ? "" : user.Aciklama));
                    sb.Append("?ISIM=" + user.Name + " " + user.Surname);
                    sb.Append("?EXPIREDATE=" + user.ExpiryDate.Value.ToString("yyyyMMdd"));
                    sb.Append("?BASLANGICTARIH=" + user.BaslangicTarihi.Value.ToString("yyyyMMdd"));
                    sb.Append("?ONOFF=" + ((user.LisansDurum.YayinDurumu) ? "1" : "0"));
                    sb.Append("?PRO=" + ((user.LisansDurum.ProYetki) ? "1" : "0"));
                    sb.Append("?CEP=" + ((user.LisansDurum.CepYetki) ? "1" : "0"));
                    sb.Append("?ROBOT=" + ((user.LisansDurum.ROBOT) ? "1" : "0"));
                    sb.Append("?SCMUSABLE=" + ((user.LisansDurum.SCMUsable) ? "1" : "0"));
                    sb.Append("?SCMREALTIME=" + ((user.LisansDurum.SCMRealTıme) ? "1" : "0"));
                    sb.Append("?SCMDOWNLOAD=" + ((user.LisansDurum.SCMDownload) ? "1" : "0"));
                    sb.Append("?IMKBL1=" + ((user.LisansDurum.PayL1) ? "1" : "0"));
                    sb.Append("?IMKBL1P=" + ((user.LisansDurum.PayLP) ? "1" : "0"));
                    sb.Append("?IMKBL2=" + ((user.LisansDurum.PayL2) ? "1" : "0"));
                    sb.Append("?EUREX=" + ((user.LisansDurum.Pd2P) ? "1" : "0"));
                    sb.Append("?IMKBX=" + ((user.LisansDurum.PayX) ? "1" : "0"));
                    sb.Append("?IMKBANL=" + ((user.LisansDurum.VeriAnalitik) ? "1" : "0"));
                    sb.Append("?IMKBISL=" + ((user.LisansDurum.PayGS) ? "1" : "0"));
                    sb.Append("?VIPL1=" + ((user.LisansDurum.ViopL1) ? "1" : "0"));
                    sb.Append("?VIPL1=" + ((user.LisansDurum.ViopL1) ? "1" : "0"));
                    sb.Append("?VIPL1P=" + ((user.LisansDurum.ViopLP) ? "1" : "0"));
                    sb.Append("?VIPL2=" + ((user.LisansDurum.ViopL2) ? "1" : "0"));
                    sb.Append("?CHIX=" + ((user.LisansDurum.Vd2P) ? "1" : "0"));
                    sb.Append("?VIPNET=" + ((user.LisansDurum.ViopGS) ? "1" : "0"));
                    sb.Append("?THVL1=" + ((user.LisansDurum.TahvilL1) ? "1" : "0"));
                    sb.Append("?THVL1P=" + ((user.LisansDurum.TahvilLP) ? "1" : "0"));
                    sb.Append("?THVL2=" + ((user.LisansDurum.TahvilL2) ? "1" : "0"));
                    sb.Append("?DJI=" + ((user.LisansDurum.DJI) ? "1" : "0"));
                    sb.Append("?SPI=" + ((user.LisansDurum.SPI) ? "1" : "0"));
                    sb.Append("?XETRA=" + ((user.LisansDurum.XETRA) ? "1" : "0"));
                    sb.Append("?CBOTM=" + ((user.LisansDurum.CBOTM) ? "1" : "0"));
                    sb.Append("?CBOT=" + ((user.LisansDurum.CBOT) ? "1" : "0"));
                    sb.Append("?CMEM=" + ((user.LisansDurum.CMEM) ? "1" : "0"));
                    sb.Append("?CME=" + ((user.LisansDurum.CME) ? "1" : "0"));
                    sb.Append("?NYMEX=" + ((user.LisansDurum.NYMEX) ? "1" : "0"));
                    sb.Append("?NYMEXM=" + ((user.LisansDurum.NYMEXM) ? "1" : "0"));
                    sb.Append("?COMEX=" + ((user.LisansDurum.COMEX) ? "1" : "0"));
                    sb.Append("?PITE=" + ((user.LisansDurum.PITE) ? "1" : "0"));
                    sb.Append("?FUTGCK=1");
                    sb.Append("?WINX=1");
                    sb.Append("?PRODUCTTYPE=" + user.ProductType);
                    sb.Append("?STATUS=" + user.StatusId);
                }

                #endregion

                //
                // iletişim
                if (user.Iletisim != null)
                {
                    if (user.Iletisim.email != null)
                        sb.Append("?MAIL=" + user.Iletisim.email);
                    if (user.Iletisim.acikadres != null)
                        sb.Append("?ADRES=" + user.Iletisim.acikadres);
                    if (user.Iletisim.Tel1 != null)
                        sb.Append("?TELEFON=" + user.Iletisim.Tel1);
                    if (user.Iletisim.Il != null)
                        sb.Append("?SEHIR=" + user.Iletisim.Il.IlAdi);
                }

                if (user.KurumsalBilgiler != null)
                {
                    sb.Append("?NOT1=" + user.KurumsalBilgiler.Not1);
                    sb.Append("?KURUMMUSTERINO=" + user.KurumsalBilgiler.kurumhesapno);
                    sb.Append("?KURUMMUSTERITIP=" + user.KurumsalBilgiler.KurumKullaniciTip);
                    sb.Append("?SUBE=" + user.KurumsalBilgiler.KurumSube);
                }

                if (user.tckno != null)
                    sb.Append("?TCKIMLIK=" + user.tckno);

                sb.Append("?PMTSNO=" + user.PmtsNo);

                await HttpRequestAsync(sb.ToString());
                //MyTools.logyaz(sb.ToString());
                //formListeTransaction(user.UserName + "  bilgileri  SSO ya gönderildi");
                }
            }
            catch (Exception ex)
            {
                formListeTransaction(ex.Message);
            }

        }

        public async Task ParseAutoCreate(string conid, string message)
        {
            await Task.Factory.StartNew(() =>
            {
                string responsestr2 = "OK";
                var fieldarray1 = message.Split('|');
                var id = 0;
                using (var crm = new crmDFNDataContext())
                {

                if (fieldarray1[0] == "AutoCreateWebCustomer")
                {

                    var kullaniciadi = "";
                    string cepStr = "", imkbxStr = "", krmd1Str = "", expripy = "";

                    var newuser = crm.Users.FirstOrDefault(x => x.UserName == fieldarray1[1].Trim());
                    if (newuser != null)
                    {
                        kullaniciadi = fieldarray1[1].Trim();
                        #region Guncelle
                        // var newuser = crm.Users.FirstOrDefault(x => x.tckno == fieldarray1[1].Trim());

                        var newevent = new UserEvent();
                        newevent.CalisanId = 2;
                        newevent.EventTarih = DateTime.Now;
                        newevent.EventTypeId = 1;

                        //var sondurum = MyTools.lisansAcKapa(newuser.LisansDurum, true, 1);
                        //newuser.LisansDurum = sondurum;

                        #region Degiskenler
                        //var username = "";
                        //var listeSayisi = 0;
                        //var expripy = "";
                        var password = "";
                        //var talepno = 0;
                        //var adetgeldi = false;
                        //var talepUserName = "";
                        //var filtre = "";
                        //var ulke = "";
                        var Adres = "";
                        //var GSM = "";
                        //var EMAIL = "";

                        var aciklama = "";
                        var ad = "";
                        var soyad = "";
                        var adres = "";
                        var sehir = "";
                        //var pro = 0;
                        var cep = 0;
                        //var TalepTipId = 0;
                        //var Tel = "";
                        //var SubeAdi = "";
                        //var temsilciKod = "";
                        //bool yayinDurumu = false;
                        //var durumId = 0;
                        var krmd1 = 0;
                        var PmtsNo = "";
                        //var hesapNo = "";
                        //var COMEX = "";
                        var RemoteHost = "";

                        //DateTime BaslangicTarih = new DateTime();
                        //DateTime ExpiryDate = new DateTime();

                        #endregion

                        for (int i = 2; i < fieldarray1.Length; i++)
                        {
                            var splitarray = fieldarray1[i].Split(';');
                            switch (splitarray[0])
                            {
                                case "Password":
                                    password = MyTools.Sifreleme.Encryp(splitarray[1]);
                                    break;
                                case "Explanation":
                                    aciklama = splitarray[1];
                                    break;
                                case "PmtsNo":
                                    PmtsNo = splitarray[1];
                                    break;
                                case "CEP": cepStr = splitarray[1]; break;
                                case "IMKBX": imkbxStr = splitarray[1]; break;
                                case "COMEX": krmd1Str = splitarray[1]; break;
                                case "ExpiryDate": expripy = splitarray[1]; break;
                                case "RemoteHost":
                                    RemoteHost = splitarray[1].ToString();
                                    break;
                                case "Adres":
                                    adres = splitarray[1];
                                    break;
                                case "Sehir":
                                    sehir = splitarray[1];
                                    break;
                                case "Ad":
                                    ad = splitarray[1];
                                    break;
                                case "Soyad":
                                    soyad = splitarray[1];
                                    break;
                                default:
                                    break;
                            }
                        }

                        newuser.UserName = fieldarray1[1].Trim();
                        //newuser.tckno = fieldarray1[1].Trim();

                        #region Kontrol
                        // Yayın durumu kontrolü
                        if (newuser.LisansDurum.YayinDurumu)  
                        {
                            DateTime today = DateTime.Today;

                            DateTime ed = !string.IsNullOrWhiteSpace(expripy)
                                ? DateTime.ParseExact(expripy, "yyyyMMdd", CultureInfo.InvariantCulture)
                               : today._LastDayOfYear();
                                //: new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

                                bool isScreenActive = newuser.ExpiryDate.HasValue && newuser.ExpiryDate.Value.Date >= today;

                            bool isCepExpired =
                                !newuser.LisansDurum.CepYetki ||
                                !newuser.LisansDurum.CepYetkiEnd.HasValue ||
                                newuser.LisansDurum.CepYetkiEnd.Value.Date < today;

                            // Ekran aktif ve CEP süresi dolmuşsa CEP'i yenile
                            if (isScreenActive && isCepExpired)
                            {
                                newuser.LisansDurum.CepYetki = true;
                                newuser.LisansDurum.CepYetkiStart = today;
                                newuser.LisansDurum.CepYetkiEnd = ed;

                                newevent.EventTypeId = 4;
                                newevent.EventTarih = DateTime.Now;
                                newevent.UserId = newuser.UserID;
                                newevent.SonLisandurumID = newuser.LisansDurumId;

                                crm.UserEvents.InsertOnSubmit(newevent);
                                crm.SubmitChanges();

                                //var textKontrol = "AutoCreateWebCustomer(CEP lisansı yenilendi) ;  user name  = " + kullaniciadi;
                                //CalisanlaraKomutGonder("AutoCreateWebCustomer|" + textKontrol);
                                SendToSSOAsync(newuser.UserID);

                                
                                Interlocked.Increment(ref _acGuncelleme);
                                SendHttpOkAndClose(conid, "OK");
                                return;
                            }
                            else
                            {
                                // Sadece SSO'ya gönder
                                //var textKontrol = "AutoCreateWebCustomer(Yayını Açık Kullanıcı) ;  user name  = " + kullaniciadi;
                                //CalisanlaraKomutGonder("AutoCreateWebCustomer|" + textKontrol);
                                SendToSSOAsync(newuser.UserID);
                                Interlocked.Increment(ref _acGuncelleme);
                                SendHttpOkAndClose(conid, "OK");
                                return;
                            }
                        }
                        else  // YAYINI KAPALI İSE - AÇMA İŞLEMİ
                        {
                            LisansDurum sondurum = new LisansDurum();
                            sondurum.LisansDurumId = newuser.LisansDurum.LisansDurumId;
                            sondurum.YayinDurumu = true;

                            for (int i = 2; i < fieldarray1.Length; i++)
                            {
                                var splitarray = fieldarray1[i].Split(';');
                                switch (splitarray[0])
                                {
                                    case "Password":
                                        newuser.Password = password;
                                        break;
                                    case "Explanation":
                                        newuser.Aciklama = aciklama;
                                        break;
                                    case "PmtsNo":
                                        newuser.PmtsNo = PmtsNo;
                                        break;
                                    case "CEP":
                                        sondurum.CepYetki = splitarray[1]._ToBool();
                                        break;
                                    case "IMKBX":
                                        sondurum.PayX = splitarray[1]._ToBool();
                                        break;
                                    case "COMEX":
                                        sondurum.COMEX = splitarray[1]._ToBool();
                                        break;
                                    case "Ad":
                                        newuser.Name = splitarray[1];
                                        break;
                                    case "Soyad":
                                        newuser.Surname = splitarray[1];
                                        break;
                                    case "RemoteHost":
                                        newevent.HostName = splitarray[1];
                                        newevent.IP = splitarray[1];
                                        break;
                                    default:
                                        break;
                                }
                            }

                            var ed = new DateTime();
                            DateTime today = DateTime.Now.Date;
                           // ed = today._LastDayOfMonth();
                           ed = today._LastDayOfYear();


                                if (cepStr._ToBool() || sondurum.CepYetki)
                            {
                                sondurum.CepYetki = true;
                                sondurum.CepYetkiStart = today;
                                sondurum.CepYetkiEnd = ed;
                            }

                            if (imkbxStr._ToBool() || sondurum.PayX)
                            {
                                sondurum.PayX = true;
                                sondurum.PayXStart = today;
                                sondurum.PayXEnd = ed;
                            }

                            if (krmd1Str._ToBool() || sondurum.COMEX)
                            {
                                sondurum.COMEX = true;
                                sondurum.KRMD1Start = today;
                                sondurum.KRMD1End = ed;
                            }

                            if (chkKarmaAcilsin.Checked)
                            {
                                sondurum.COMEX = true; if (!sondurum.KRMD1Start.HasValue) { sondurum.KRMD1Start = today; sondurum.KRMD1End = ed; }
                                sondurum.CepYetki = true; if (!sondurum.CepYetkiStart.HasValue) { sondurum.CepYetkiStart = today; sondurum.CepYetkiEnd = ed; }
                                sondurum.PayX = true; if (!sondurum.PayXStart.HasValue) { sondurum.PayXStart = today; sondurum.PayXEnd = ed; }
                            }
                            else
                            {
                                sondurum.CepYetki = true; if (!sondurum.CepYetkiStart.HasValue) { sondurum.CepYetkiStart = today; sondurum.CepYetkiEnd = ed; }
                                sondurum.PayX = true; if (!sondurum.PayXStart.HasValue) { sondurum.PayXStart = today; sondurum.PayXEnd = ed; }
                            }


                                newuser.ExpiryDate = ed;
                            crm.LisansDurums.InsertOnSubmit(sondurum);
                            crm.SubmitChanges();
                            newuser.LisansDurum = sondurum;
                        }
                        #endregion

                        id = newuser.UserID;
                        if (newuser.MusteriMenseiID == null)
                            newuser.MusteriMenseiID = 1;
                        kullaniciadi = newuser.UserName;
                        // kullaniciadi = newuser.tckno;
                        newevent.UserId = newuser.UserID;
                        newevent.SonLisandurumID = newuser.LisansDurumId;
                        crm.UserEvents.InsertOnSubmit(newevent);
                        crm.SubmitChanges();

                        #endregion
                    }
                    else
                    {
                        #region YeniKayit
                        try
                        {
                            newuser = new User();
                            var newevent = new UserEvent();
                            var lisanslar = new LisansDurum();
                            DateTime today = DateTime.Now.Date;
                            for (int i = 1; i < fieldarray1.Length; i++)
                            {
                                var splitarray = fieldarray1[i].Split(';');
                                switch (splitarray[0])
                                {
                                    case "Password":
                                        newuser.Password = MyTools.Sifreleme.Encryp(splitarray[1]);
                                        break;
                                    case "Explanation":
                                        newuser.Aciklama = splitarray[1];
                                        break;
                                    case "PmtsNo":
                                        newuser.PmtsNo = splitarray[1];
                                        break;
                                    //case "ExpiryDate":
                                    //    newuser.ExpiryDate = DateTime.ParseExact(splitarray[1], "yyyyMMdd", CultureInfo.InvariantCulture);
                                    //    break;
                                    case "CEP": cepStr = splitarray[1]; break;
                                    case "IMKBX": imkbxStr = splitarray[1]; break;
                                    case "COMEX": krmd1Str = splitarray[1]; break;
                                    case "ExpiryDate": expripy = splitarray[1]; break;
                                    case "BaslangicTarih":
                                        // newuser.BaslangicTarihi = DateTime.ParseExact(splitarray[1], "yyyy.MM.dd hh:mm:ss", CultureInfo.InvariantCulture);
                                        newuser.BaslangicTarihi = DateTime.Now;
                                        break;
                                    case "Ad":
                                        newuser.Name = splitarray[1];
                                        break;
                                    case "Soyad":
                                        newuser.Surname = splitarray[1];
                                        break;
                                    case "RemoteHost":
                                        newevent.HostName = splitarray[1];
                                        newevent.IP = splitarray[1];

                                        break;
                                    default:
                                        break;
                                }
                            }
                            var ed = new DateTime();
                             today = DateTime.Now.Date;
                            // ed = today._LastDayOfMonth();
                            ed = today._LastDayOfYear();

                                newevent.CalisanId = 2;
                            newevent.EventTarih = DateTime.Now;
                            newevent.EventTypeId = 5;
                            lisanslar.YayinDurumu = true;
                            if (newuser.MusteriMenseiID == null)
                                newuser.MusteriMenseiID = 1;

                            //newuser.UserName = "Test";
                            newuser.UserName = fieldarray1[1].Trim();
                            newuser.tckno = fieldarray1[1].Trim();
                            newuser.Name = newuser.Name;
                            newuser.Surname = newuser.Surname;
                            kullaniciadi = newuser.UserName;
                            //kullaniciadi = newuser.tckno;
                            newuser.BaslangicTarihi = DateTime.Now;

                                lisanslar.CepYetki = true;
                                lisanslar.CepYetkiStart = today;
                                lisanslar.CepYetkiEnd = ed;

                                lisanslar.PayX = true;
                                lisanslar.PayXStart = today;
                                lisanslar.PayXEnd = ed;
                                lisanslar.COMEX = true;
                                lisanslar.KRMD1Start = today;
                                lisanslar.KRMD1End = ed;

                            //if (chkKarmaAcilsin.Checked)
                            //{
                            //    // lisanslar.COMEX = true;
                            //    lisanslar.COMEX = true; if (!lisanslar.KRMD1Start.HasValue) { lisanslar.KRMD1Start = today; lisanslar.KRMD1End = ed; }
                            //    lisanslar.CepYetki = true; if (!lisanslar.CepYetkiStart.HasValue) { lisanslar.CepYetkiStart = today; lisanslar.CepYetkiEnd = ed; }
                            //    lisanslar.PayX = true; if (!lisanslar.PayXStart.HasValue) { lisanslar.PayXStart = today; lisanslar.PayXEnd = ed; }
                            //}

                            newuser.ExpiryDate = ed;
                            lisanslar.Futgck = true;
                            lisanslar.WINX = true;                         
                            newuser.Aciklama = newuser.UserName;
                            // newuser.Aciklama = newuser.tckno;
                            newuser.ProductType = "IDEAL";
                            newuser.StatusId = 1;
                            kullaniciadi = newuser.UserName;
                            //kullaniciadi = newuser.tckno;
                            crm.LisansDurums.InsertOnSubmit(lisanslar);
                            crm.SubmitChanges();
                            newuser.LisansDurumId = lisanslar.LisansDurumId;
                            newevent.SonLisandurumID = lisanslar.LisansDurumId;

                            crm.Users.InsertOnSubmit(newuser);
                            crm.SubmitChanges();

                            newevent.UserId = newuser.UserID;
                            id = newuser.UserID;

                            crm.UserEvents.InsertOnSubmit(newevent);
                            crm.SubmitChanges();
                            Interlocked.Increment(ref _acYeniKayit);
                        }
                        catch (Exception ex)
                        {
                            MyTools.logyaz("AutoCreateWebCustomer Hata : " + ex.Message);
                            Interlocked.Increment(ref _acBasarisiz);
                        }
                        #endregion
                    }

                    if (id > 0)
                    {
                        SendToSSOAsync(id);
                        var text = "AutoCreateWebCustomer ;  user name  = " + kullaniciadi;
                        formListeTransaction(text);
                        // CalisanlaraKomutGonder("AutoCreateWebCustomer|" + text);
                          SendHttpOkAndClose(conid, "OK");
                    }
                    else
                    {
                        formListeTransaction(kullaniciadi + " SSO ya gönderilemedi");
                        Interlocked.Increment(ref _acBasarisiz);
                          SendHttpOkAndClose(conid, "HATA");

                    }
                   
                    return;
                }
                }
            });
        }
             

        private void SendHttpOkAndClose(string conid, string body)
        {
            try
            {
                if (IpDeamon.Connections.ContainsKey(conid))
                {
                    // _InsertHeaderHTTP() sizin projede HTTP/1.1 200 OK + Content-Length ekliyor
                    IpDeamon.Connections[conid].DataToSendB = Encoding.UTF8.GetBytes(body._InsertHeaderHTTP());
                    IpDeamon.Connections[conid].Connected = false;
                }
            }
            catch { }
        }

        public async Task SendToSSOAsync(int id)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                var user = crm.Users.FirstOrDefault(x => x.UserID == id);

                if (user == null)
                {
                    formListeTransaction(id.ToString() + "numaralı ID bulunamadı");
                    return;
                }
                var sb = new StringBuilder();

                #region AKYATIRIM

                if (MyTools.KurumKod == "10011")
                {
                    sb.Append("http://" + ssoip + ":" + ssoport + "/DFN?CRM?CRM?" + user.UserName + "?");
                    // sb.Append("http://" + ssoip + ":" + ssoport + "/DFN?CRM?CRM?" + user.tckno + "?");
                    sb.Append("IDEALSIFRE=" + MyTools.Sifreleme.Decryp(user.Password));
                    sb.Append("?ACIKLAMA=" + ((user.Aciklama == null) ? "" : user.Aciklama));
                    sb.Append("?ISIM=" + user.Name + " " + user.Surname);
                    sb.Append("?EXPIREDATE=" + user.ExpiryDate.Value.ToString("yyyyMMdd"));
                    if (user.BaslangicTarihi == null)
                        user.BaslangicTarihi = DateTime.Now;
                    sb.Append("?BASLANGICTARIH=" + user.BaslangicTarihi.Value.ToString("yyyyMMdd"));
                    sb.Append("?ONOFF=" + ((user.LisansDurum.YayinDurumu) ? "1" : "0"));
                    sb.Append("?PRO=" + ((user.LisansDurum.ProYetki) ? "1" : "0"));
                    sb.Append("?CEP=" + ((user.LisansDurum.CepYetki) ? "1" : "0"));
                    sb.Append("?ROBOT=" + ((user.LisansDurum.ROBOT) ? "1" : "0"));

                    sb.Append("?SCMUSABLE=" + ((user.LisansDurum.SCMUsable) ? "1" : "0"));
                    sb.Append("?SCMREALTIME=" + ((user.LisansDurum.SCMRealTıme) ? "1" : "0"));
                    sb.Append("?SCMDOWNLOAD=" + ((user.LisansDurum.SCMDownload) ? "1" : "0"));
                    sb.Append("?IMKBL1=" + ((user.LisansDurum.PayL1) ? "1" : "0"));
                    sb.Append("?IMKBL1P=" + ((user.LisansDurum.PayLP) ? "1" : "0"));
                    sb.Append("?IMKBL2=" + ((user.LisansDurum.PayL2) ? "1" : "0"));
                    sb.Append("?EUREX=" + ((user.LisansDurum.Pd2P) ? "1" : "0"));
                    sb.Append("?IMKBX=" + ((user.LisansDurum.PayX) ? "1" : "0"));
                     sb.Append("?IMKBANL=" + ((user.LisansDurum.VeriAnalitik) ? "1" : "0"));
                    sb.Append("?IMKBISL=" + ((user.LisansDurum.PayGS) ? "1" : "0"));
                    sb.Append("?PITE=" + ((user.LisansDurum.PITE) ? "1" : "0"));
                    
                    // sb.Append("?VIPL1=" + ((user.LisansDurum.ViopL1) ? "1" : "0"));
                    sb.Append("?VIPL1=" + ((user.LisansDurum.ViopL1) ? "1" : "0"));
                    sb.Append("?VIPL1P=" + ((user.LisansDurum.ViopLP) ? "1" : "0"));
                    sb.Append("?VIPL2=" + ((user.LisansDurum.ViopL2) ? "1" : "0"));
                    sb.Append("?CHIX=" + ((user.LisansDurum.Vd2P) ? "1" : "0"));
                    sb.Append("?VIPNET=" + ((user.LisansDurum.ViopGS) ? "1" : "0"));
                    sb.Append("?THVL1=" + ((user.LisansDurum.TahvilL1) ? "1" : "0"));
                    sb.Append("?THVL1P=" + ((user.LisansDurum.TahvilLP) ? "1" : "0"));
                    sb.Append("?THVL2=" + ((user.LisansDurum.TahvilL2) ? "1" : "0"));
                    //sb.Append("?DJI=" + ((user.LisansDurum.DJI) ? "1" : "0"));
                    sb.Append("?SPI=" + ((user.LisansDurum.SPI) ? "1" : "0"));
                    //sb.Append("?XETRA=" + ((user.LisansDurum.XETRA) ? "1" : "0"));
                    //sb.Append("?CBOTM=" + ((user.LisansDurum.CBOTM) ? "1" : "0"));
                    //sb.Append("?CBOT=" + ((user.LisansDurum.CBOT) ? "1" : "0"));
                    //sb.Append("?CMEM=" + ((user.LisansDurum.CMEM) ? "1" : "0"));
                    sb.Append("?CME=" + ((user.LisansDurum.CME) ? "1" : "0"));
                    //sb.Append("?EUREX=" + ((user.LisansDurum.EUREX) ? "1" : "0"));
                    //sb.Append("?NYMEX=" + ((user.LisansDurum.NYMEX) ? "1" : "0"));
                    //sb.Append("?NYMEXM=" + ((user.LisansDurum.NYMEXM) ? "1" : "0"));
                    sb.Append("?COMEX=" + ((user.LisansDurum.COMEX) ? "1" : "0"));
                    sb.Append("?FUTGCK=1");
                    sb.Append("?WINX=1");
                    sb.Append("?PRODUCTTYPE=" + user.ProductType);
                    sb.Append("?STATUS=" + user.StatusId);

                    var multiLisans = "";
                    if (user.LisansDurum.MKK)
                    {
                        multiLisans += "MKK;";
                    }

                    if (user.LisansDurum.GKKUL)
                    {
                        multiLisans += "GKKUL;";
                    }

                    if (!user.LisansDurum.MKK && !user.LisansDurum.GKKUL)
                    {
                        multiLisans = "";
                    }

                    sb.Append("?ML=" + multiLisans);
                }

                #endregion

                #region Digerleri

                else
                {
                    sb.Append("http://" + ssoip + ":" + ssoport + "/DFN?CRM?CRM?" + user.UserName + "?");
                    // sb.Append("http://" + ssoip + ":" + ssoport + "/DFN?CRM?CRM?" + user.tckno + "?");
                    sb.Append("IDEALSIFRE=" + MyTools.Sifreleme.Decryp(user.Password));
                    sb.Append("?ACIKLAMA=" + ((user.Aciklama == null) ? "" : user.Aciklama));
                    sb.Append("?ISIM=" + user.Name + " " + user.Surname);
                    sb.Append("?EXPIREDATE=" + user.ExpiryDate.Value.ToString("yyyyMMdd"));
                    sb.Append("?BASLANGICTARIH=" + user.BaslangicTarihi.Value.ToString("yyyyMMdd"));
                    sb.Append("?ONOFF=" + ((user.LisansDurum.YayinDurumu) ? "1" : "0"));
                    sb.Append("?PRO=" + ((user.LisansDurum.ProYetki) ? "1" : "0"));
                    sb.Append("?CEP=" + ((user.LisansDurum.CepYetki) ? "1" : "0"));
                    sb.Append("?ROBOT=" + ((user.LisansDurum.ROBOT) ? "1" : "0"));
                    
                    sb.Append("?SCMUSABLE=" + ((user.LisansDurum.SCMUsable) ? "1" : "0"));
                    sb.Append("?SCMREALTIME=" + ((user.LisansDurum.SCMRealTıme) ? "1" : "0"));
                    sb.Append("?SCMDOWNLOAD=" + ((user.LisansDurum.SCMDownload) ? "1" : "0"));
                    sb.Append("?IMKBL1=" + ((user.LisansDurum.PayL1) ? "1" : "0"));
                    sb.Append("?IMKBL1P=" + ((user.LisansDurum.PayLP) ? "1" : "0"));
                    sb.Append("?IMKBL2=" + ((user.LisansDurum.PayL2) ? "1" : "0"));
                    sb.Append("?IMKBL2P=" + ((user.LisansDurum.Pd2P) ? "1" : "0"));
                    sb.Append("?IMKBX=" + ((user.LisansDurum.PayX) ? "1" : "0"));
                    sb.Append("?IMKBANL=" + ((user.LisansDurum.VeriAnalitik) ? "1" : "0"));
                    sb.Append("?IMKBISL=" + ((user.LisansDurum.PayGS) ? "1" : "0"));
                    sb.Append("?VIPL1=" + ((user.LisansDurum.ViopL1) ? "1" : "0"));
                    sb.Append("?VIPL1=" + ((user.LisansDurum.ViopL1) ? "1" : "0"));
                    sb.Append("?VIPL1P=" + ((user.LisansDurum.ViopLP) ? "1" : "0"));
                    sb.Append("?VIPL2=" + ((user.LisansDurum.ViopL2) ? "1" : "0"));
                    sb.Append("?VIPL2P=" + ((user.LisansDurum.Vd2P) ? "1" : "0"));
                    sb.Append("?VIPNET=" + ((user.LisansDurum.ViopGS) ? "1" : "0"));
                    sb.Append("?THVL1=" + ((user.LisansDurum.TahvilL1) ? "1" : "0"));
                    sb.Append("?THVL1P=" + ((user.LisansDurum.TahvilLP) ? "1" : "0"));
                    sb.Append("?THVL2=" + ((user.LisansDurum.TahvilL2) ? "1" : "0"));
                    sb.Append("?DJI=" + ((user.LisansDurum.DJI) ? "1" : "0"));
                    sb.Append("?SPI=" + ((user.LisansDurum.SPI) ? "1" : "0"));
                    sb.Append("?XETRA=" + ((user.LisansDurum.XETRA) ? "1" : "0"));
                    sb.Append("?CBOTM=" + ((user.LisansDurum.CBOTM) ? "1" : "0"));
                    sb.Append("?CBOT=" + ((user.LisansDurum.CBOT) ? "1" : "0"));
                    sb.Append("?CMEM=" + ((user.LisansDurum.CMEM) ? "1" : "0"));
                    sb.Append("?CME=" + ((user.LisansDurum.CME) ? "1" : "0"));
                    sb.Append("?EUREX=" + ((user.LisansDurum.EUREX) ? "1" : "0"));
                    sb.Append("?NYMEX=" + ((user.LisansDurum.NYMEX) ? "1" : "0"));
                    sb.Append("?NYMEXM=" + ((user.LisansDurum.NYMEXM) ? "1" : "0"));
                    sb.Append("?COMEX=" + ((user.LisansDurum.COMEX) ? "1" : "0"));
                    sb.Append("?FUTGCK=1");
                    sb.Append("?WINX=1");
                    sb.Append("?PRODUCTTYPE=" + user.ProductType);
                    sb.Append("?STATUS=" + user.StatusId);
                }

                #endregion

                // iletişim

                if (user.Iletisim != null)
                {
                    if (user.Iletisim.email != null)
                        sb.Append("?MAIL=" + user.Iletisim.email);
                    if (user.Iletisim.acikadres != null)
                        sb.Append("?ADRES=" + (user.Iletisim.acikadres).Replace("\n", ""));
                    if (user.Iletisim.Tel1 != null)
                        sb.Append("?TELEFON=" + user.Iletisim.Tel1);
                    if (user.Iletisim.Il != null)
                        sb.Append("?SEHIR=" + user.Iletisim.Il.IlAdi);
                }

                if (user.KurumsalBilgiler != null)
                {
                    sb.Append("?NOT1=" + user.KurumsalBilgiler.Not1);
                    sb.Append("?KURUMMUSTERINO=" + user.KurumsalBilgiler.kurumhesapno);
                    sb.Append("?KURUMMUSTERITIP=" + user.KurumsalBilgiler.KurumKullaniciTip);
                    sb.Append("?SUBE=" + user.KurumsalBilgiler.KurumSube);
                }


                if (user.tckno != null)
                    sb.Append("?TCKIMLIK=" + user.tckno);

                sb.Append("?PMTSNO=" + user.PmtsNo);



                await HttpRequestAsync(sb.ToString());
                MyTools.logyaz(sb.ToString());
                formListeTransaction(user.UserName + "  bilgileri  SSO ya gönderildi");
                // formListeTransaction(user.tckno + "  bilgileri  SSO ya gönderildi");


            }
            catch (Exception ex)
            {
                formListeTransaction(ex.Message);
            }
        }
        async Task<string> HttpRequestAsync(string urlx)
        {
            var result = "";

            try
            {
                using (var httpclient = new HttpClient())
                {
                    var request = await httpclient.GetAsync(urlx);
                    result = await request.Content.ReadAsStringAsync();
                    using (Stream ms = await request.Content.ReadAsStreamAsync())
                    {
                        var streamreaderx = new StreamReader(ms, Latin5);
                        result = streamreaderx.ReadToEnd();
                    }
                    ;
                }
                return result;
            }
            catch { return result; }

        }
        public void TeknikRaporSendToCepServer(string tip, int id)
        {
            try
            {
                var sb = new StringBuilder();
                if (tip == "Delete")
                {
                    sb.Append("AkTeknikAnalizDelete|" + id.ToString() + "|");
                }
                else
                {
                    crmDFNDataContext crm = new crmDFNDataContext();
                    var rapor = crm.TeknikAnalizRapors.FirstOrDefault(x => x.id == id);

                    if (rapor == null)
                    {
                        formListeTransaction(id.ToString() + "ID numaralı Teknik Rapor bulunamadı");
                        return;
                    }
                    sb.Append("AkTeknikAnalizInsert|" + rapor.id.ToString() + "|");
                    sb.Append(rapor.Tarih.Value.ToString("yyyyMMdd HH:mm:ss") + "|");
                    sb.Append(rapor.Baslik + "|");
                    sb.Append(rapor.Stocks + "|");
                    sb.Append(rapor.Piyasa + "|");
                    sb.Append(rapor.icerik + "|");
                    sb.Append(rapor.link + "|");
                }

                foreach (Connection con in IpdaemonforCepServers.Connections.Values)
                {
                    if (con.Connected)
                    {
                        con.DataToSend = sb.ToString() + (char)3;
                    }
                }



                MyTools.logyaz(sb.ToString());


            }
            catch (Exception ex)
            {
                formListeTransaction(ex.Message);
            }

        }
        //public async Task TeknikRaporSendToSSO(string tip, int id)
        //{
        //    try
        //    {
        //        var result = "";
        //        var sb = new StringBuilder();
        //        if (tip == "Delete")
        //        {
        //            sb.Append("http://" + ssoip + ":" + ssoport + "/DFN?AkTeknikAnaliz?Delete|" + id.ToString() + "|");
        //        }
        //        else
        //        {
        //            crmDFNDataContext crm = new crmDFNDataContext();
        //            var rapor = crm.TeknikAnalizRapors.FirstOrDefault(x => x.id == id);

        //            if (rapor == null)
        //            {
        //                formListeTransaction(id.ToString() + "ID numaralı Teknik Rapor bulunamadı");
        //                return;
        //            }
        //            sb.Append("http://" + ssoip + ":" + ssoport + "/DFN?AkTeknikAnaliz?Insert|" + rapor.id.ToString() + "|");
        //            sb.Append(rapor.Tarih.Value.ToString("yyyyMMdd HH:mm:ss") + "|");
        //            sb.Append(rapor.Baslik + "|");
        //            sb.Append(rapor.Stocks + "|");
        //            sb.Append(rapor.Piyasa + "|");
        //            sb.Append(rapor.icerik + "|");
        //            sb.Append(rapor.link + "|");
        //        }
        //        try
        //        {
        //            using (var httpclient = new HttpClient())
        //            {

        //                var request = await httpclient.GetAsync(sb.ToString());
        //                result = await request.Content.ReadAsStringAsync();
        //                using (Stream ms = await request.Content.ReadAsStreamAsync())
        //                {
        //                    var streamreaderx = new StreamReader(ms, Latin5);
        //                    result = streamreaderx.ReadToEnd();
        //                }
        //                ;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MyTools.logyaz(" Hata:TeknikRaporSendToSSO " + ex.Message);

        //        }
        //        //httpSSO.Get(sb.ToString());
        //        MyTools.logyaz(sb.ToString());


        //    }
        //    catch (Exception ex)
        //    {
        //        formListeTransaction(ex.Message);
        //    }

        //}

        public async Task<bool> TeknikRaporSendToSSO(string tip, int id)
        {
            var url = "";
            try
            {
                var sb = new StringBuilder();

                if (tip == "Delete")
                {
                    sb.Append("http://" + ssoip + ":" + ssoport + "/DFN?AkTeknikAnaliz?Delete|" + id.ToString() + "|");
                }
                else
                {
                    crmDFNDataContext db = new crmDFNDataContext();
                    var rapor = db.TeknikAnalizRapors.FirstOrDefault(x => x.id == id);

                    if (rapor == null)
                    {
                        formListeTransaction(id.ToString() + " ID numaralı Teknik Rapor bulunamadı");
                        return false;
                    }

                    // Mobil notu yayın tarihiyle göstersin ve doğru sıralasın
                    var yayinTarihi = rapor.BaslangicTarihi ?? rapor.Tarih ?? DateTime.Now;

                    sb.Append("http://" + ssoip + ":" + ssoport + "/DFN?AkTeknikAnaliz?Insert|" + rapor.id.ToString() + "|");
                    sb.Append(yayinTarihi.ToString("yyyyMMdd HH:mm:ss") + "|");
                    sb.Append(SsoAlan(rapor.Baslik) + "|");
                    sb.Append(SsoAlan(rapor.Stocks) + "|");
                    sb.Append(SsoAlan(rapor.Piyasa) + "|");
                    sb.Append(SsoAlan(rapor.icerik) + "|");
                    sb.Append(SsoAlan(rapor.link) + "|");
                }

                url = sb.ToString();

                using (var httpclient = new HttpClient())
                {
                    var request = await httpclient.GetAsync(url);

                    string result;
                    using (Stream ms = await request.Content.ReadAsStreamAsync())
                    using (var streamreaderx = new StreamReader(ms, Latin5))
                    {
                        result = streamreaderx.ReadToEnd();
                    }

                    MyTools.logyaz(url + " -> " + (int)request.StatusCode + " " + result);
                    return request.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                MyTools.logyaz(" Hata:TeknikRaporSendToSSO " + ex.Message + " URL=" + url);
                formListeTransaction("Teknik Rapor SSO gönderim hatası ; ID = " + id + " " + ex.Message);
                return false;
            }
        }

        /// <summary>SSO pipe formatına güvenli alan: '|' ayraçla çakışmasın, #, %, &, + ve satır sonları kaybolmasın.</summary>
        private static string SsoAlan(string deger)
        {
            if (string.IsNullOrEmpty(deger)) return "";
            return Uri.EscapeDataString(deger.Replace("|", "/"));
        }

        private static bool TeknikRaporAktifMi(DateTime? baslangic, DateTime? bitis, DateTime simdi)
        {
            return (baslangic == null || baslangic <= simdi)
                && (bitis == null || bitis >= simdi);
        }

        /// <param name="icerikGuncellendi">
        /// true  (Create/Update): aktif rapor yayında olsa bile güncel içerik yeniden gönderilir.
        /// false (zamanlayıcı)  : sadece durum değişikliği varsa gönderilir.
        /// </param>
        public async Task TeknikRaporSsoSenkronEt(int id, bool icerikGuncellendi)
        {
            try
            {
                var db = new crmDFNDataContext();
                var rapor = db.TeknikAnalizRapors.FirstOrDefault(x => x.id == id);
                if (rapor == null)
                {
                    formListeTransaction(id + " ID numaralı Teknik Rapor bulunamadı (SSO senkron)");
                    return;
                }

                var aktif = TeknikRaporAktifMi(rapor.BaslangicTarihi, rapor.BitisTarihi, DateTime.Now);

                if (aktif)
                {
                    if (!icerikGuncellendi && rapor.SsoYayinda) return; // zaten yayında, değişiklik yok

                    var basarili = await TeknikRaporSendToSSO("Insert", id);

                    // Başarısızsa bayrak 0 olur: güncel içerik SSO'ya ulaşmadı,
                    // zamanlayıcı tekrar Insert dener (Insert = upsert).
                    if (rapor.SsoYayinda != basarili)
                    {
                        rapor.SsoYayinda = basarili;
                        db.SubmitChanges();
                    }

                    formListeTransaction("Teknik Rapor SSO Insert ; ID = " + id + (basarili ? " başarılı" : " BAŞARISIZ"));
                }
                else if (rapor.SsoYayinda)
                {
                    // Yayında ama artık olmamalı (başlangıç ileri alındı / süresi doldu)
                    var basarili = await TeknikRaporSendToSSO("Delete", id);

                    // Başarısızsa bayrak 1 kalır, zamanlayıcı tekrar Delete dener
                    if (basarili)
                    {
                        rapor.SsoYayinda = false;
                        db.SubmitChanges();
                    }

                    formListeTransaction("Teknik Rapor SSO Delete ; ID = " + id + (basarili ? " başarılı" : " BAŞARISIZ"));
                }
                else
                {
                    // Aktif değil ve yayında değil: ileri tarihli -> zamanlayıcı yayınlayacak
                    formListeTransaction("Teknik Rapor ileri tarihli/süresi dolmuş, SSO'ya gönderilmedi ; ID = " + id);
                }
            }
            catch (Exception ex)
            {
                MyTools.logyaz(" Hata:TeknikRaporSsoSenkronEt ID=" + id + " " + ex.Message);
            }
        }



        public void SentimentAlgoSendToCepServer(string tip, int id)
        {
            try
            {
                var sb = new StringBuilder();
                if (tip == "Delete")
                {
                    sb.Append("SentimentAlgoDelete|" + id.ToString() + "|");
                }
                else
                {
                    crmDFNDataContext crm = new crmDFNDataContext();
                    var sAlgo = crm.SentimentAlgos.FirstOrDefault(x => x.id == id);

                    if (sAlgo == null)
                    {
                        formListeTransaction(id.ToString() + "ID numaralı Sentiment Algo bulunamadı");
                        return;
                    }
                    sb.Append("SentimentAlgoInsert|" + sAlgo.id.ToString() + "|");
                    sb.Append(sAlgo.OlusturmaTar.Value.ToString("yyyyMMdd HH:mm:ss") + "|");
                    sb.Append(sAlgo.Baslik + "|");
                    sb.Append(sAlgo.Icerik + "|");
                    sb.Append(sAlgo.Link + "|");

                    if (sAlgo.SentimentSembols != null)
                    {
                        if (sAlgo.SentimentSembols.Count > 0)
                        {
                            foreach (var asembol in sAlgo.SentimentSembols)
                            {
                                sb.Append(asembol.Sembol + "=" + asembol.SonFiyat + ";");
                            }
                            sb.Append("|");
                        }
                    }

                }

                foreach (Connection con in IpdaemonforCepServers.Connections.Values)
                {
                    if (con.Connected)
                    {
                        con.DataToSend = sb.ToString() + (char)3;
                    }
                }



                MyTools.logyaz(sb.ToString());


            }
            catch (Exception ex)
            {
                formListeTransaction(ex.Message);
            }

        }
        public void SendToCepServerDuyuru(int id, string tip)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                var duyuru = crm.Duyurulars.FirstOrDefault(x => x.id == id);

                if (duyuru == null)
                {
                    formListeTransaction(id.ToString() + "numaralı ID bulunamadı");
                    return;
                }

                var sb = new StringBuilder();

                foreach (Connection con in IpdaemonforCepServers.Connections.Values)
                {
                    if (con.Connected)
                    {
                        sb.Append("PUSH");
                        sb.Append("|" + tip);
                        sb.Append("|" + duyuru.id);
                        sb.Append("|" + duyuru.Mesaj);
                        sb.Append("|" + duyuru.Sembol);
                        sb.Append("|" + duyuru.tarih.ToString("yyyyMMddHHmmss"));
                        con.DataToSend = sb.ToString() + (char)3;
                        MyTools.logyaz(sb.ToString());
                    }
                }

                formListeTransaction(duyuru.id + " nolu Duyuru Cerserverlara gönderildi");
            }
            catch (Exception ex)
            {
                formListeTransaction(ex.Message);
            }

        }
        public void SendToCepServerModelPortfoy(string sentext)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                foreach (Connection con in IpdaemonforCepServers.Connections.Values)
                {
                    if (con.Connected)
                    {
                        con.DataToSend = sentext + (char)3;
                        MyTools.logyaz(sentext);
                    }
                }

                formListeTransaction(sentext + " Modelportföy Cerserverlara gönderildi");
            }
            catch (Exception ex)
            {
                formListeTransaction(ex.Message);
            }

        }

        public delegate void lblsayacguncelle(string text);
        public void lblsayacyaz(string text)
        {
            try
            {
                if (lblSayac.InvokeRequired)
                {
                    lblsayacguncelle lbl = new lblsayacguncelle(lblsayacyaz);

                    this.Invoke(lbl, new object[] { text });

                }
                else
                {
                    lblSayac.Text = text;
                }
            }
            catch (Exception ex)
            {
                MyTools.logyaz("Hata:lblsayacyaz" + ex.Message);
          
            }
        }
        public void lblsayacyaz2(string text)
        {
            try
            {
                if (lblSayac2.InvokeRequired)
                {
                    lblsayacguncelle lbl2 = new lblsayacguncelle(lblsayacyaz2);

                    this.Invoke(lbl2, new object[] { text });

                }
                else
                {
                    lblSayac2.Text = text;
                }
            }
            catch (Exception ex)
            {

                formListeTransaction("Hata:lblsayacyaz2" + ex.Message);
            }
        }
        public void lblsayacyaz3(string text)
        {
            try
            {
                if (lblSayac3.InvokeRequired)
                {
                    lblsayacguncelle lbl3 = new lblsayacguncelle(lblsayacyaz3);

                    this.Invoke(lbl3, new object[] { text });

                }
                else
                {
                    lblSayac3.Text = text;
                }
            }
            catch (Exception ex)
            {

                formListeTransaction("Hata:lblsayacyaz3" + ex.Message);
            }
        }

        public void kapalilarkontrol()
        {
            kontrol = false;
            Task.Run(() =>
            {
                var sonuser = "";
                try
                {
                    using (var crm = new crmDFNDataContext())
                    {
                        var acikusers = crm.Users.Where(x => x.LisansDurum.YayinDurumu == true && x.ExpiryDate.Value.Date < DateTime.Now.Date).ToList();
                        var i = 0;
                        var count = acikusers.Count;
                        foreach (var user in acikusers)
                        {
                            sonuser = user.UserName;
                            UserEvent userevent = new UserEvent();
                            var lid = MyTools.lisansAcKapa(user.LisansDurum, false, 1);
                            user.LisansDurum = lid;
                            userevent.CalisanId = 2;
                            userevent.EventTypeId = 2;
                            userevent.HostName = HostName;
                            userevent.IP = IpAdress;
                            userevent.LisansDurum = lid;
                            userevent.UserId = user.UserID;
                            userevent.EventTarih = DateTime.Now;
                            crm.UserEvents.InsertOnSubmit(userevent);

                            i++;
                            StatusString = ("Kontrol İşlemi Süreci : " + count.ToString() + " / " + (i).ToString());
                        }
                        crm.SubmitChanges();
                    }
                    lblsayacyaz("");
                    formListeTransaction("Expiy Date Kontrol Etme işi Bitti");
                    acilacaklarKontrol();
                    OnluPaketLisansKapat(0);
                }
                catch (Exception ex)
                {
                    formListeTransaction("ExpiryDate Kontrol : " + sonuser + "Kullanıcısında Hata :" + ex.Message);
                    kapalilarkontrol();
                }

            });
            formListeTransaction("Expiy Date Kontrol Etme işi başladı");
        }
        public void LisansSureKontrol()
        {
            LisansSurekontrol = false;

            Task.Run(() =>
            {
                try
                {
                    using (var crm = new crmDFNDataContext())
                    {
                        formListeTransaction("Lisans Süre Date Kontrol Etme işi başladı");
                        var acikusers = crm.Users.Where(x => x.LisansDurum.YayinDurumu == true).ToList();
                        var count = acikusers.Count;
                        for (int i = 0; i < acikusers.Count; i++)
                        {
                            var user = acikusers[i];
                            if (user.UserName == "_sedat")
                            {

                            }
                            var now = DateTime.Now.Date;
                            var SenSSOBool = false;
                            var acilan = new StringBuilder();
                            var kapanan = new StringBuilder();
                            #region LisansCopy
                            LisansDurum sondurum = new LisansDurum();
                            sondurum.YayinDurumu = user.LisansDurum.YayinDurumu;

                            // Bist Pay
                            sondurum.PayL1 = user.LisansDurum.PayL1;
                            sondurum.PayLP = user.LisansDurum.PayLP;
                            sondurum.PayL2 = user.LisansDurum.PayL2;
                            sondurum.Pd2P = user.LisansDurum.Pd2P;
                            sondurum.PayX = user.LisansDurum.PayX;
                            sondurum.PayGS = user.LisansDurum.PayGS;
                            sondurum.PITE = user.LisansDurum.PITE;
                            sondurum.VeriAnalitik = user.LisansDurum.VeriAnalitik;

                            // Viop
                            sondurum.ViopL1 = user.LisansDurum.ViopL1;
                            sondurum.ViopLP = user.LisansDurum.ViopLP;
                            sondurum.ViopL2 = user.LisansDurum.ViopL2;
                            sondurum.Vd2P = user.LisansDurum.Vd2P;
                            sondurum.ViopGS = user.LisansDurum.ViopGS;

                            // Tahvil
                            sondurum.TahvilL1 = user.LisansDurum.TahvilL1;
                            sondurum.TahvilLP = user.LisansDurum.TahvilLP;
                            sondurum.TahvilL2 = user.LisansDurum.TahvilL2;

                            // Karma / Senti / AnPro / diğer
                            sondurum.COMEX = user.LisansDurum.COMEX;
                            sondurum.AnPro = user.LisansDurum.AnPro;
                            sondurum.SentiL1 = user.LisansDurum.SentiL1;
                            sondurum.SentiL2 = user.LisansDurum.SentiL2;
                            sondurum.CME = user.LisansDurum.CME;
                            sondurum.MKK = user.LisansDurum.MKK;
                            sondurum.GKKUL = user.LisansDurum.GKKUL;
                            sondurum.ROBOT = user.LisansDurum.ROBOT;

                            // Local
                            sondurum.CepYetki = user.LisansDurum.CepYetki;
                            sondurum.ProYetki = user.LisansDurum.ProYetki;
                            sondurum.SCMDownload = user.LisansDurum.SCMDownload;
                            sondurum.SCMRealTıme = user.LisansDurum.SCMRealTıme;
                            sondurum.SCMUsable = user.LisansDurum.SCMUsable;
                            sondurum.Futgck = user.LisansDurum.Futgck;
                            sondurum.WINX = user.LisansDurum.WINX;

                            // Sase
                            sondurum.SaseL1 = user.LisansDurum.SaseL1;
                            sondurum.SaseL2 = user.LisansDurum.SaseL2;

                            // Yurtdışı (tarihsiz, sadece bool)
                            sondurum.DJI = user.LisansDurum.DJI;
                            sondurum.XETRA = user.LisansDurum.XETRA;
                            sondurum.CBOT = user.LisansDurum.CBOT;
                            sondurum.CBOTM = user.LisansDurum.CBOTM;
                            sondurum.CMEM = user.LisansDurum.CMEM;
                            sondurum.EUREX = user.LisansDurum.EUREX;
                            sondurum.NYMEX = user.LisansDurum.NYMEX;
                            sondurum.NYMEXM = user.LisansDurum.NYMEXM;
                            sondurum.NYSE = user.LisansDurum.NYSE;
                            sondurum.NASDAQ = user.LisansDurum.NASDAQ;
                            sondurum.Amex = user.LisansDurum.Amex;
                            sondurum.CHIX = user.LisansDurum.CHIX;
                            sondurum.LSE = user.LisansDurum.LSE;

                            // SPI tüm kullanıcılarda her zaman açık
                            sondurum.SPI = true;

                            // START DATE
                            sondurum.PayL1Start = user.LisansDurum.PayL1Start;
                            sondurum.PayLPStart = user.LisansDurum.PayLPStart;
                            sondurum.PayL2Start = user.LisansDurum.PayL2Start;
                            sondurum.Pd2PStart = user.LisansDurum.Pd2PStart;
                            sondurum.PayXStart = user.LisansDurum.PayXStart;
                            sondurum.PayGSStart = user.LisansDurum.PayGSStart;
                            sondurum.PayPiteStart = user.LisansDurum.PayPiteStart;
                            sondurum.ViopL1Start = user.LisansDurum.ViopL1Start;
                            sondurum.ViopLPStart = user.LisansDurum.ViopLPStart;
                            sondurum.ViopL2Start = user.LisansDurum.ViopL2Start;
                            sondurum.Vd2PStart = user.LisansDurum.Vd2PStart;
                            sondurum.ViopGSStart = user.LisansDurum.ViopGSStart;
                            sondurum.KRMD1Start = user.LisansDurum.KRMD1Start;
                            sondurum.TahvilL1Start = user.LisansDurum.TahvilL1Start;
                            sondurum.TahvilLPStart = user.LisansDurum.TahvilLPStart;
                            sondurum.TahvilL2Start = user.LisansDurum.TahvilL2Start;
                            sondurum.AnProStart = user.LisansDurum.AnProStart;
                            sondurum.SentiL1Start = user.LisansDurum.SentiL1Start;
                            sondurum.SentiL2Start = user.LisansDurum.SentiL2Start;
                            sondurum.CMEStart = user.LisansDurum.CMEStart;
                            sondurum.CepYetkiStart = user.LisansDurum.CepYetkiStart;
                            sondurum.ProYetkiStart = user.LisansDurum.ProYetkiStart;
                            sondurum.MKKStart = user.LisansDurum.MKKStart;
                            sondurum.GKKULStart = user.LisansDurum.GKKULStart;

                            // END DATE
                            sondurum.PayL1End = user.LisansDurum.PayL1End;
                            sondurum.PayLPEnd = user.LisansDurum.PayLPEnd;
                            sondurum.PayL2End = user.LisansDurum.PayL2End;
                            sondurum.Pd2PEnd = user.LisansDurum.Pd2PEnd;
                            sondurum.PayXEnd = user.LisansDurum.PayXEnd;
                            sondurum.PayGSEnd = user.LisansDurum.PayGSEnd;
                            sondurum.PayPiteEnd = user.LisansDurum.PayPiteEnd;
                            sondurum.ViopL1End = user.LisansDurum.ViopL1End;
                            sondurum.ViopLPEnd = user.LisansDurum.ViopLPEnd;
                            sondurum.ViopL2End = user.LisansDurum.ViopL2End;
                            sondurum.Vd2PEnd = user.LisansDurum.Vd2PEnd;
                            sondurum.ViopGSEnd = user.LisansDurum.ViopGSEnd;
                            sondurum.KRMD1End = user.LisansDurum.KRMD1End;
                            sondurum.TahvilL1End = user.LisansDurum.TahvilL1End;
                            sondurum.TahvilLPEnd = user.LisansDurum.TahvilLPEnd;
                            sondurum.TahvilL2End = user.LisansDurum.TahvilL2End;
                            sondurum.AnProEnd = user.LisansDurum.AnProEnd;
                            sondurum.SentiL1End = user.LisansDurum.SentiL1End;
                            sondurum.SentiL2End = user.LisansDurum.SentiL2End;
                            sondurum.CMEEnd = user.LisansDurum.CMEEnd;
                            sondurum.CepYetkiEnd = user.LisansDurum.CepYetkiEnd;
                            sondurum.ProYetkiEnd = user.LisansDurum.ProYetkiEnd;
                            sondurum.MKKEnd = user.LisansDurum.MKKEnd;
                            sondurum.GKKULEnd = user.LisansDurum.GKKULEnd;
                            #endregion

                            #region StartdateKontrol
                            if (sondurum.PayL1Start != null)
                            if (sondurum.PayL1Start.Value.Date == now && sondurum.PayL1 == false) { sondurum.PayL1 = true; SenSSOBool = true; acilan.Append("PayL1;"); }
                        if (sondurum.PayLPStart != null)
                            if (sondurum.PayLPStart.Value.Date == now && sondurum.PayLP == false) { sondurum.PayLP = true; SenSSOBool = true; acilan.Append("PayLP;"); }
                        if (sondurum.PayL2Start != null)
                            if (sondurum.PayL2Start.Value.Date == now && sondurum.PayL2 == false) { sondurum.PayL2 = true; SenSSOBool = true; acilan.Append("PayL2;"); }
                        if (sondurum.Pd2PStart != null)
                            if (sondurum.Pd2PStart.Value.Date == now && sondurum.Pd2P == false) { sondurum.Pd2P = true; SenSSOBool = true; acilan.Append("Pd2P;"); }
                        if (sondurum.PayXStart != null)
                            if (sondurum.PayXStart.Value.Date == now && sondurum.PayX == false) { sondurum.PayX = true; SenSSOBool = true; acilan.Append("PayX;"); }
                        if (sondurum.PayGSStart != null)
                            if (sondurum.PayGSStart.Value.Date == now && sondurum.PayGS == false) { sondurum.PayGS = true; SenSSOBool = true; acilan.Append("PayGS;"); }
                        if (sondurum.PayPiteStart != null)
                            if (sondurum.PayPiteStart.Value.Date == now && sondurum.PITE == false) { sondurum.PITE = true; SenSSOBool = true; acilan.Append("PITE;"); }


                        if (sondurum.ViopL1Start != null)
                            if (sondurum.ViopL1Start.Value.Date == now && sondurum.ViopL1 == false) { sondurum.ViopL1 = true; SenSSOBool = true; acilan.Append("ViopL1;"); }
                        if (sondurum.ViopLPStart != null)
                            if (sondurum.ViopLPStart.Value.Date == now && sondurum.ViopLP == false) { sondurum.ViopLP = true; SenSSOBool = true; acilan.Append("ViopLP;"); }
                        if (sondurum.ViopL2Start != null)
                            if (sondurum.ViopL2Start.Value.Date == now && sondurum.ViopL2 == false) { sondurum.ViopL2 = true; SenSSOBool = true; acilan.Append("ViopL2;"); }
                        if (sondurum.Vd2PStart != null)
                            if (sondurum.Vd2PStart.Value.Date == now && sondurum.Vd2P == false) { sondurum.Vd2P = true; SenSSOBool = true; acilan.Append("Vd2P;"); }
                        if (sondurum.ViopGSStart != null)
                            if (sondurum.ViopGSStart.Value.Date == now && sondurum.ViopGS == false) { sondurum.ViopGS = true; SenSSOBool = true; acilan.Append("ViopGS;"); }

                        if (sondurum.KRMD1Start != null)
                            if (sondurum.KRMD1Start.Value.Date == now && sondurum.COMEX == false) { sondurum.COMEX = true; SenSSOBool = true; acilan.Append("KRMD1;"); }

                        if (sondurum.MKKStart != null)
                            if (sondurum.MKKStart.Value.Date == now && sondurum.MKK == false) { sondurum.MKK = true; SenSSOBool = true; acilan.Append("MKK;"); }

                        if (sondurum.GKKULStart != null)
                            if (sondurum.GKKULStart.Value.Date == now && sondurum.GKKUL == false) { sondurum.GKKUL = true; SenSSOBool = true; acilan.Append("GKKUL;"); }

                        //if (sondurum.TaramaStart != null)
                        //    if (sondurum.TaramaStart.Value.Date == now && sondurum.TARAMA == false) { sondurum.TARAMA = true; SenSSOBool = true; acilan.Append("TARAMA;"); }

                        if (sondurum.TahvilL1Start != null)
                           if (sondurum.TahvilL1Start.Value.Date == now && sondurum.TahvilL1 == false) { sondurum.TahvilL1 = true; SenSSOBool = true; acilan.Append("TahvilL1;"); }
                        if (sondurum.TahvilLPStart != null)
                            if (sondurum.TahvilLPStart.Value.Date == now && sondurum.TahvilLP == false) { sondurum.TahvilLP = true; SenSSOBool = true; acilan.Append("TahvilLP;"); }
                        if (sondurum.TahvilL2Start != null)
                            if (sondurum.TahvilL2Start.Value.Date == now && sondurum.TahvilL2 == false) { sondurum.TahvilL2 = true; SenSSOBool = true; acilan.Append("TahvilL2;"); }
                        if (sondurum.AnProStart != null)
                            if (sondurum.AnProStart.Value.Date == now && sondurum.AnPro == false) { sondurum.AnPro = true; SenSSOBool = true; acilan.Append("AnalizPro;"); }

                        if (sondurum.CMEStart != null)
                            if (sondurum.CMEStart.Value.Date == now && sondurum.CME == false) { sondurum.CME = true; SenSSOBool = true; acilan.Append("CME;"); }
                        //if (sondurum.CMEMStart != null)
                        //    if (sondurum.CMEMStart.Value.Date == now && sondurum.CMEM == false) { sondurum.CMEM = true; SenSSOBool = true; acilan.Append("CMEM;"); }
                        //if (sondurum.RobotStart != null)
                        //    if (sondurum.RobotStart.Value.Date == now && sondurum.ROBOT == false) { sondurum.ROBOT = true; SenSSOBool = true; acilan.Append("ROBOT;"); }
                        //if (sondurum.TemelAnalizStart != null)
                        //    if (sondurum.TemelAnalizStart.Value.Date == now && sondurum.TemelAnaliz == false) { sondurum.TemelAnaliz = true; SenSSOBool = true; acilan.Append("TemelAnaliz;"); }
                        //if (sondurum.BMKStart != null)
                        //    if (sondurum.BMKStart.Value.Date == now && sondurum.BMK == false) { sondurum.BMK = true; SenSSOBool = true; acilan.Append("BMK;"); }
                        //if (sondurum.BMCStart != null)
                        //    if (sondurum.BMCStart.Value.Date == now && sondurum.BMC == false) { sondurum.BMC = true; SenSSOBool = true; acilan.Append("BMCAPITAL;"); }
                        //if (sondurum.BarSistemStart != null)
                        //    if (sondurum.BarSistemStart.Value.Date == now && sondurum.BarSistem == false) { sondurum.BarSistem = true; SenSSOBool = true; acilan.Append("BarSistem;"); }
                        //if (sondurum.FSystemStart != null)
                        //    if (sondurum.FSystemStart.Value.Date == now && sondurum.FSystem == false) { sondurum.FSystem = true; SenSSOBool = true; acilan.Append("FSystem;"); }
                        //if (sondurum.PARAStart != null)
                        //    if (sondurum.PARAStart.Value.Date == now && sondurum.PARA == false) { sondurum.PARA = true; SenSSOBool = true; acilan.Append("PARA;"); }
                        //if (sondurum.HISSEAStart != null)
                        //    if (sondurum.HISSEAStart.Value.Date == now && sondurum.HISSEA == false) { sondurum.HISSEA = true; SenSSOBool = true; acilan.Append("HisseAnaliz;"); }
                        //if (sondurum.SentiL1Start != null)
                        //    if (sondurum.SentiL1Start.Value.Date == now && sondurum.SentiL1 == false) { sondurum.SentiL1 = true; SenSSOBool = true; acilan.Append("SentiL1;"); }
                        //if (sondurum.SentiL2Start != null)
                        //    if (sondurum.SentiL2Start.Value.Date == now && sondurum.SentiL2 == false) { sondurum.SentiL2 = true; SenSSOBool = true; acilan.Append("SentiL2;"); }
                        if (sondurum.ProYetkiStart != null)
                            if (sondurum.ProYetkiStart.Value.Date == now && sondurum.ProYetki == false) { sondurum.ProYetki = true; SenSSOBool = true; acilan.Append("ProYetki;"); }
                        if (sondurum.CepYetkiStart != null)
                            if (sondurum.CepYetkiStart.Value.Date == now && sondurum.CepYetki == false) { sondurum.CepYetki = true; SenSSOBool = true; acilan.Append("CepYetki;"); }

                        #endregion
                        #region EndDateKontrol
                        if (sondurum.PayL1End != null)
                            if (sondurum.PayL1End.Value.Date < now && sondurum.PayL1 == true) { sondurum.PayL1 = false; sondurum.PayL1Start = null; sondurum.PayL1End = null; SenSSOBool = true; kapanan.Append("PayL1;"); }
                        if (sondurum.PayLPEnd != null)
                            if (sondurum.PayLPEnd.Value.Date < now && sondurum.PayLP == true) { sondurum.PayLP = false; sondurum.PayLPStart = null; sondurum.PayLPEnd = null; SenSSOBool = true; kapanan.Append("PayLP;"); }
                        if (sondurum.PayL2End != null)
                            if (sondurum.PayL2End.Value.Date < now && sondurum.PayL2 == true) { sondurum.PayL2 = false; sondurum.PayL2Start = null; sondurum.PayL2End = null; SenSSOBool = true; kapanan.Append("PayL2;"); }
                        if (sondurum.Pd2PEnd != null)
                            if (sondurum.Pd2PEnd.Value.Date < now && sondurum.Pd2P == true) { sondurum.Pd2P = false; sondurum.Pd2PStart = null; sondurum.Pd2PEnd = null; SenSSOBool = true; kapanan.Append("Pd2P;"); }
                        if (sondurum.PayXEnd != null)
                            if (sondurum.PayXEnd.Value.Date < now && sondurum.PayX == true) { sondurum.PayX = false; sondurum.PayXStart = null; sondurum.PayXEnd = null; SenSSOBool = true; kapanan.Append("PayX;"); }
                        if (sondurum.PayGSEnd != null)
                            if (sondurum.PayGSEnd.Value.Date < now && sondurum.PayGS == true) { sondurum.PayGS = false; sondurum.PayGSStart = null; sondurum.PayGSEnd = null; SenSSOBool = true; kapanan.Append("PayGS;"); }
                        if (sondurum.PayPiteEnd != null)
                            if (sondurum.PayPiteEnd.Value.Date < now && sondurum.PITE == true) { sondurum.PITE = false; sondurum.PayPiteStart = null; sondurum.PayPiteEnd = null; SenSSOBool = true; kapanan.Append("PITE;"); }
                        if (sondurum.ViopL1End != null)
                            if (sondurum.ViopL1End.Value.Date < now && sondurum.ViopL1 == true) { sondurum.ViopL1 = false; sondurum.ViopL1Start = null; sondurum.ViopL1End = null; SenSSOBool = true; kapanan.Append("ViopL1;"); }
                        if (sondurum.ViopLPEnd != null)
                            if (sondurum.ViopLPEnd.Value.Date < now && sondurum.ViopLP == true) { sondurum.ViopLP = false; sondurum.ViopLPStart = null; sondurum.ViopLPEnd = null; SenSSOBool = true; kapanan.Append("ViopLP;"); }
                        if (sondurum.ViopL2End != null)
                            if (sondurum.ViopL2End.Value.Date < now && sondurum.ViopL2 == true) { sondurum.ViopL2 = false; sondurum.ViopL2Start = null; sondurum.ViopL2End = null; SenSSOBool = true; kapanan.Append("ViopL2;"); }
                        if (sondurum.Vd2PEnd != null)
                            if (sondurum.Vd2PEnd.Value.Date < now && sondurum.Vd2P == true) { sondurum.Vd2P = false; sondurum.Vd2PStart = null; sondurum.Vd2PEnd = null; SenSSOBool = true; kapanan.Append("Vd2P;"); }

                        if (sondurum.ViopGSEnd != null)
                            if (sondurum.ViopGSEnd.Value.Date < now && sondurum.ViopGS == true) { sondurum.ViopGS = false; sondurum.ViopGSStart = null; sondurum.ViopGSEnd = null; SenSSOBool = true; kapanan.Append("ViopGS;"); }

                        if (sondurum.KRMD1End != null)
                            if (sondurum.KRMD1End.Value.Date < now && sondurum.COMEX == true) { sondurum.COMEX = false; sondurum.KRMD1Start = null; sondurum.KRMD1End = null; SenSSOBool = true; kapanan.Append("KRMD1;"); }

                        if (sondurum.MKKEnd != null)
                            if (sondurum.MKKEnd.Value.Date < now && sondurum.MKK == true) { sondurum.MKK = false; sondurum.MKKStart = null; sondurum.MKKEnd = null; SenSSOBool = true; kapanan.Append("MKK;"); }
                        if (sondurum.GKKULEnd != null)
                            if (sondurum.GKKULEnd.Value.Date < now && sondurum.GKKUL == true) { sondurum.GKKUL = false; sondurum.GKKULStart = null; sondurum.GKKULEnd = null; SenSSOBool = true; kapanan.Append("GKKUL;"); }
                        //if (sondurum.TaramaEnd != null)
                        //    if (sondurum.TaramaEnd.Value.Date < now && sondurum.TARAMA == true) { sondurum.TARAMA = false; sondurum.TaramaStart = null; sondurum.TaramaEnd = null; SenSSOBool = true; kapanan.Append("TARAMA;"); }

                        if (sondurum.TahvilL1End != null)
                            if (sondurum.TahvilL1End.Value.Date < now && sondurum.TahvilL1 == true) { sondurum.TahvilL1 = false; sondurum.TahvilL1Start = null; sondurum.TahvilL1End = null; SenSSOBool = true; kapanan.Append("TahvilL1;"); }
                        if (sondurum.TahvilLPEnd != null)
                            if (sondurum.TahvilLPEnd.Value.Date < now && sondurum.TahvilLP == true) { sondurum.TahvilLP = false; sondurum.TahvilLPStart = null; sondurum.TahvilLPEnd = null; SenSSOBool = true; kapanan.Append("TahvilLP;"); }
                        if (sondurum.TahvilL2End != null)
                            if (sondurum.TahvilL2End.Value.Date < now && sondurum.TahvilL2 == true) { sondurum.TahvilL2 = false; sondurum.TahvilL2Start = null; sondurum.TahvilL2End = null; SenSSOBool = true; kapanan.Append("TahvilL2;"); }
                        //if (sondurum.AnProEnd != null)
                        //    if (sondurum.AnProEnd.Value.Date < now && sondurum.AnPro == true) { sondurum.AnPro = false; sondurum.AnProStart = null; sondurum.AnProEnd = null; SenSSOBool = true; kapanan.Append("AnalizPro;"); }

                        //if (sondurum.SentiL1End != null)
                        //    if (sondurum.SentiL1End.Value.Date < now && sondurum.SentiL1 == true) { sondurum.SentiL1 = false; sondurum.SentiL1Start = null; sondurum.SentiL1End = null; SenSSOBool = true; kapanan.Append("SentiL1;"); }
                        //if (sondurum.SentiL2End != null)
                        //    if (sondurum.SentiL2End.Value.Date < now && sondurum.SentiL2 == true) { sondurum.SentiL2 = false; sondurum.SentiL2Start = null; sondurum.SentiL2End = null; SenSSOBool = true; kapanan.Append("SentiL2;"); }
                        if (sondurum.CMEEnd != null)
                            if (sondurum.CMEEnd.Value.Date < now && sondurum.CME == true) { sondurum.CME = false; sondurum.CMEStart = null; sondurum.CMEEnd = null; SenSSOBool = true; kapanan.Append("CME;"); } //var sozHaricLisnStr = SozlesmeharicLisanstemizle(user.SozHaricLisans, "CME"); user.SozHaricLisans = sozHaricLisnStr; }
                        //if (sondurum.CMEMEnd != null)
                        //    if (sondurum.CMEMEnd.Value.Date < now && sondurum.CMEM == true) { sondurum.CMEM = false; sondurum.CMEMStart = null; sondurum.CMEMEnd = null; SenSSOBool = true; kapanan.Append("CMEM;"); var sozHaricLisnStr = SozlesmeharicLisanstemizle(user.SozHaricLisans, "CMEM"); user.SozHaricLisans = sozHaricLisnStr; }

                        //if (sondurum.RobotEnd != null)
                        //    if (sondurum.RobotEnd.Value.Date < now && sondurum.ROBOT == true) { sondurum.ROBOT = false; sondurum.RobotStart = null; sondurum.RobotEnd = null; SenSSOBool = true; kapanan.Append("ROBOT;"); var sozHaricLisnStr = SozlesmeharicLisanstemizle(user.SozHaricLisans, "ROBOT"); user.SozHaricLisans = sozHaricLisnStr; }
                        //if (sondurum.TemelAnalizEnd != null)
                        //    if (sondurum.TemelAnalizEnd.Value.Date < now && sondurum.TemelAnaliz == true) { sondurum.TemelAnaliz = false; sondurum.TemelAnalizStart = null; sondurum.TemelAnalizEnd = null; SenSSOBool = true; kapanan.Append("TemelAnaliz;"); var sozHaricLisnStr = SozlesmeharicLisanstemizle(user.SozHaricLisans, "TemelAnaliz"); user.SozHaricLisans = sozHaricLisnStr; }
                        //if (sondurum.BMKEnd != null)
                        //    if (sondurum.BMKEnd.Value.Date < now && sondurum.BMK == true) { sondurum.BMK = false; sondurum.BMKStart = null; sondurum.BMKEnd = null; SenSSOBool = true; kapanan.Append("BMK;"); var sozHaricLisnStr = SozlesmeharicLisanstemizle(user.SozHaricLisans, "BMK"); user.SozHaricLisans = sozHaricLisnStr; }
                        //if (sondurum.BMCEnd != null)
                        //    if (sondurum.BMCEnd.Value.Date < now && sondurum.BMC == true) { sondurum.BMC = false; sondurum.BMCStart = null; sondurum.BMCEnd = null; SenSSOBool = true; kapanan.Append("BMC;"); var sozHaricLisnStr = SozlesmeharicLisanstemizle(user.SozHaricLisans, "BMCAPITAL"); user.SozHaricLisans = sozHaricLisnStr; }
                        //if (sondurum.BarSistemEnd != null)
                        //    if (sondurum.BarSistemEnd.Value.Date < now && sondurum.BarSistem == true) { sondurum.BarSistem = false; sondurum.BarSistemStart = null; sondurum.BarSistemEnd = null; SenSSOBool = true; kapanan.Append("BarSistem;"); var sozHaricLisnStr = SozlesmeharicLisanstemizle(user.SozHaricLisans, "BarSistem"); user.SozHaricLisans = sozHaricLisnStr; }
                        //if (sondurum.FSystemEnd != null)
                        //    if (sondurum.FSystemEnd.Value.Date < now && sondurum.FSystem == true) { sondurum.FSystem = false; sondurum.FSystemStart = null; sondurum.FSystemEnd = null; SenSSOBool = true; kapanan.Append("FSystem;"); var sozHaricLisnStr = SozlesmeharicLisanstemizle(user.SozHaricLisans, "FSystem"); user.SozHaricLisans = sozHaricLisnStr; }
                        //if (sondurum.PARAEnd != null)
                        //    if (sondurum.PARAEnd.Value.Date < now && sondurum.PARA == true) { sondurum.PARA = false; sondurum.PARAStart = null; sondurum.PARAEnd = null; SenSSOBool = true; kapanan.Append("PARA;"); var sozHaricLisnStr = SozlesmeharicLisanstemizle(user.SozHaricLisans, "PARA"); user.SozHaricLisans = sozHaricLisnStr; }
                        //if (sondurum.HISSEAEnd != null)
                        //if (sondurum.HISSEAEnd.Value.Date < now && sondurum.HISSEA == true) { sondurum.HISSEA = false; sondurum.HISSEAStart = null; sondurum.HISSEAEnd = null; SenSSOBool = true; kapanan.Append("HisseAnaliz;"); var sozHaricLisnStr = SozlesmeharicLisanstemizle(user.SozHaricLisans, "HISSEA"); user.SozHaricLisans = sozHaricLisnStr; }
                        if (sondurum.ProYetkiEnd != null)
                            if (sondurum.ProYetkiEnd.Value.Date < now && sondurum.ProYetki == true) { sondurum.ProYetki = false; sondurum.ProYetkiStart = null; sondurum.ProYetkiEnd = null; SenSSOBool = true; kapanan.Append("ProYetki;"); }
                        if (sondurum.CepYetkiEnd != null)
                            if (sondurum.CepYetkiEnd.Value.Date < now && sondurum.CepYetki == true) { sondurum.CepYetki = false; sondurum.CepYetkiStart = null; sondurum.CepYetkiEnd = null; SenSSOBool = true; kapanan.Append("CepYetki;"); }

                        #endregion

                        if ((sondurum.ProYetkiEnd != null && sondurum.ProYetkiEnd.Value.Date < DateTime.Now.Date) && (sondurum.CepYetkiEnd != null && sondurum.CepYetkiEnd.Value.Date < DateTime.Now))
                        {
                            sondurum.YayinDurumu = false;
                        }
                        if ((sondurum.ProYetkiEnd != null && sondurum.ProYetkiEnd.Value.Date < DateTime.Now.Date) && (sondurum.CepYetki == false))
                        {
                            sondurum.YayinDurumu = false;
                        }
                        if ((sondurum.CepYetkiEnd != null && sondurum.CepYetkiEnd.Value.Date < DateTime.Now.Date) && (sondurum.ProYetki == false))
                        {
                            sondurum.YayinDurumu = false;
                        }

                        if (SenSSOBool)
                        {
                            crm.LisansDurums.InsertOnSubmit(sondurum);
                            crm.SubmitChanges();
                            UserEvent userevent = new UserEvent();
                            user.LisansDurum = sondurum;
                            crm.SubmitChanges();
                            userevent.CalisanId = 2;
                            userevent.EventTypeId = 6;
                            userevent.HostName = HostName;
                            userevent.IP = IpAdress;
                            userevent.LisansDurum = sondurum;
                            userevent.UserId = user.UserID;
                            userevent.EventTarih = DateTime.Now;
                            userevent.AcilanLisans = acilan.ToString();
                            userevent.KapatilanLisans = kapanan.ToString();
                            crm.UserEvents.InsertOnSubmit(userevent);
                            crm.SubmitChanges();


                            formListeTransaction("Server Lisans Değitirme ; " + user.UserName + " ;Açılanlar " + acilan.ToString() + "; Kapanan " + kapanan.ToString());
                            // formListeTransaction("Server Lisans Değitirme ; " + user.tckno + " ;Açılanlar " + acilan.ToString() + "; Kapanan " + kapanan.ToString());
                            SendToSSO(user.UserID);
                        }
                        lblsayacyaz2("Lisans Süre Kontrol İşlemi Süreci : " + count.ToString() + " / " + (i).ToString());
                    }
                    lblsayacyaz2("");
                    formListeTransaction("Lisans Süre Date Kontrol Etme işi Bitti");
                    }
                }
                catch (Exception ex)
                {
                    formListeTransaction(ex.Message);
                }
            });
            formListeTransaction("Lisans Süre Kontrol Etme işi başladı");
        }
        public void acilacaklarKontrol()
        {
            kontrol = false;
            Task.Run(() =>
            {
                try
                {
                    using (var crm = new crmDFNDataContext())
                    {
                        var kapaliusers = crm.Users.Where(x => x.LisansDurum.YayinDurumu == false && (x.LisansDurum.ProYetkiStart.Value.Date == DateTime.Now.Date || x.LisansDurum.CepYetkiStart.Value.Date == DateTime.Now.Date)).ToList();

                        var count = kapaliusers.Count;

                        for (int i = 0; i < count; i++)
                        {
                            var user = kapaliusers[i];
                            if (user.ExpiryDate.Value.Date > DateTime.Now.Date)
                            {
                                UserEvent userevent = new UserEvent();
                                var sondurum = MyTools.lisansAcKapa(user.LisansDurum, true, 1);
                                user.LisansDurum = sondurum;
                                crm.SubmitChanges();
                                userevent.CalisanId = 2;
                                userevent.EventTypeId = 1;
                                userevent.HostName = HostName;
                                userevent.IP = IpAdress;
                                userevent.LisansDurum = sondurum;
                                userevent.UserId = user.UserID;
                                userevent.EventTarih = DateTime.Now;
                                crm.UserEvents.InsertOnSubmit(userevent);
                                crm.SubmitChanges();
                            }
                            lblsayacyaz3("Kontrol İşlemi Süreci : " + count.ToString() + " / " + (i).ToString());
                        }

                        lblsayacyaz3("");
                        formListeTransaction("Açılacaklar Kontrol Etme işi Bitti");
                        LisansSureKontrol();
                    }
                }
                catch (Exception ex)
                {
                    formListeTransaction(ex.Message);
                }
            });

            formListeTransaction("Açılacaklar Kontrol Etme işi başladı");
        }
        public class Sonuc
        {
            public string Kod { get; set; }
            public string Aciklama { get; set; }
            public string Status { get; set; }
        }
        class HisseSinyalClass
        {
            public string HESAPNO = "";

            public string EXPIREDATE = "";

            public string ULKE = "";

            public string ADRES = "";

            public string GSM = "";

            public string EMAIL = "";

            public string SEHIR = "";

            public string AD = "";

            public string SOYAD = "";

            public string lisansDurum = "";

            public string ONOF = "";

            //pay
        }

        public class BosAlanlar
        {
            public string Adres { get; set; }
            public string Ulke { get; set; }
            public string Il { get; set; }
            public string GSM { get; set; }
            public string Ad { get; set; }
            public string Soyad { get; set; }
            public string Email { get; set; }
        }
        public class KullaniciListele
        {
            public string USERNAME = "";
            public string EXPIREDATE = "";
            public string PRO = "";
            public string CEP = "";

            public string ONOF = "";

            //pay
            public string PD1 = "";
            public string PD1P = "";
            public string PD2 = "";
            public string PD2P = "";
            public string END = "";
            public string PIT = "";
            public string PITE = "";


            public string PD1SD = "";
            public string PD1PSD = "";
            public string PD2SD = "";
            public string PD2PSD = "";
            public string ENDSD = "";
            public string PITSD = "";
            public string PITESD = "";

            public string PD1ED = "";
            public string PD1PED = "";
            public string PD2ED = "";
            public string PD2PED = "";
            public string ENDED = "";
            public string PITED = "";
            public string PITEED = "";


            //viop
            public string VD1 = "";
            public string VD1P = "";
            public string VD2 = "";
            public string VD2P = "";
            public string VIT = "";

            public string VD1SD = "";
            public string VD1PSD = "";
            public string VD2SD = "";
            public string VD2PSD = "";
            public string VITSD = "";

            public string VD1ED = "";
            public string VD1PED = "";
            public string VD2ED = "";
            public string VD2PED = "";
            public string VITED = "";

            public string KRMD1 = "";
            public string KRMD1SD = "";
            public string KRMD1ED = "";

            public string MKK = "";
            public string MKKSD = "";
            public string MKKED = "";

            public string TARAMA = "";
            public string TARAMASD = "";
            public string TARAMAED = "";

            public string GKKUL = "";
            public string GKKULSD = "";
            public string GKKULED = "";

            public string CME = "";
            public string CMESD = "";
            public string CMEED = "";

            //tahvil
            //public string BD1 = "";
            //public string BD1P = "";
            //public string BD2 = "";

            //public string BD1SD = "";
            //public string BD1PSD = "";
            //public string BD2SD = "";

            //public string BD1ED = "";
            //public string BD1PED = "";
            //public string BD2ED = "";

        }

        public void OnluPaketLisansKapat(short val)
        {
            Thread newthread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    var gun = DateTime.Now.Day;
                    if (gun > 1 && val == 0) { return; }
                    crmDFNDataContext crm = new crmDFNDataContext();
                    formListeTransaction("Onlu Paket Lisans Kapama işi başladı");
                    var acikusers = crm.Users.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.SPI == true && x.Iletisim.acikadres == "GerekKalmadı").ToList();
                    var i = 0;
                    var count = acikusers.Count;
                    foreach (var user in acikusers)
                    {
                        var now = DateTime.Now.Date;
                        var acilan = new StringBuilder();
                        var kapanan = new StringBuilder();
                        var sondurum = MyTools.lisansAcKapa(user.LisansDurum, false, 2);
                        sondurum.SPI = false;
                        kapanan.Append(";SPI");
                        UserEvent userevent = new UserEvent();
                        Thread.Sleep(60);
                        user.LisansDurum = sondurum;
                        crm.SubmitChanges();
                        userevent.CalisanId = 2;
                        userevent.EventTypeId = 6;
                        userevent.HostName = HostName;
                        userevent.IP = IpAdress;
                        userevent.LisansDurum = sondurum;
                        userevent.UserId = user.UserID;
                        userevent.EventTarih = DateTime.Now;
                        userevent.AcilanLisans = acilan.ToString();
                        userevent.KapatilanLisans = kapanan.ToString();
                        

                        crm.UserEvents.InsertOnSubmit(userevent);
                        crm.SubmitChanges();

                        formListeTransaction("Server Lisans Değitirme ; " + user.UserName + " ;Açılanlar " + acilan.ToString() + "; Kapanan " + kapanan.ToString());
                        SendToSSOAsync(user.UserID);



                        i++;
                        lblsayacyaz2("Lisans Süre Kontrol İşlemi Süreci : " + count.ToString() + " / " + (i).ToString());
                    }

                    lblsayacyaz2("");
                    formListeTransaction("Onlu Paket Lisans Kapama işi bitti");

                }
                catch (Exception ex) { formListeTransaction(ex.Message); }

            }));

            newthread.Start();
            formListeTransaction("Lisans Süre Kontrol Etme işi başladı");
        }
        private void timerOnOffCheck_Tick(object sender, EventArgs e)
        {
            try
            {
                lblSayac.Text = StatusString;

                if (DateTime.Today > _acResetDate)
                {
                    _acResetDate = DateTime.Today;
                    Interlocked.Exchange(ref _acYeniKayit, 0);
                    Interlocked.Exchange(ref _acGuncelleme, 0);
                    Interlocked.Exchange(ref _acBasarisiz, 0);
                }
                lblAutoCreateStatsYaz($"AutoCreate | Yeni: {_acYeniKayit}  Güncelleme: {_acGuncelleme}  Başarısız: {_acBasarisiz}");

                try
                {
                    if (LoginHistoryEnabled)
                        labelLoginHistoryYaz(lastLoginHistory);
                }
                catch { }
                var sat2 = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);

                if (sat2 == kotrolsaati.Substring(0, 8))
                { kontrol = true; LisansSurekontrol = true; }
               

                if (kontrol)
                {
                    kapalilarkontrol();
                }

                var t = new TimeSpan(23, 59, 50);
                if (DateTime.Now.ToString("HH:mm:ss") == t.ToString())
                {
                    if (kontrolToplam)
                    {
                        ToplamlariYaz();
                    }
                }
            }
            catch
            {
            }
        }
        private void kotrolSaatiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formSaatKotrol frm = new formSaatKotrol();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();

        }
        private void IpDeamonForClilents_OnReadyToSend(object sender, IpdaemonReadyToSendEventArgs e)
        {

        }
        private void IpDeamon_OnReadyToSend(object sender, IpdaemonReadyToSendEventArgs e)
        {

        }
        public void ToplamlariYaz()
        {

            kontrolToplam = false;

            Task.Run(() =>
            {
                try
                {
                    #region Toplamlar
                    using (var crm = new crmDFNDataContext())
                    {
                    var ActiveSorgu = crm.Users.Where(x => x.UserID > 0);

                    var TumUserSayisi = crm.Users.Where(x => x.LisansDurum.YayinDurumu).Count();
                    var IseUserSayisi = crm.Users.Where(x => x.LisansDurum.YayinDurumu && x.StatusId == 1).Count();
                    var TrkUserSayisi = crm.Users.Where(x => x.LisansDurum.YayinDurumu && x.StatusId == 1).Count();
                    var MuafUserSayisi = crm.Users.Where(x => x.LisansDurum.YayinDurumu && x.StatusId == 2).Count();

                    var Cep = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true && x.StatusId == 1).Count();
                    var pro = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ProYetki == true && x.StatusId == 1).Count();
                    var robot = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ROBOT == true).Count();
                    var ProCep = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true && x.LisansDurum.ProYetki == true && (x.StatusId == 1)).Count();
                    var PayYuzeysel = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false && x.LisansDurum.Pd2P == false).Count();
                    var PayPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                    var PayDerinlik = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                    var PayDerinlikPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == true).Count();
                    var PayEndeks = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayX == true).Count();
                    var PayGs = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayGS == true).Count();

                    var analitik = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.VeriAnalitik == true).Count();
                    var pite = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PITE == true).Count();

                    var ViopYuzeysel = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false && x.LisansDurum.Vd2P == false).Count();
                    var ViopPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                    var ViopDerinlik = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                    var ViopDerinlikPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == true).Count();
                    var ViopGS = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();

                    var TahvilYuzeysel = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false).Count();
                    var TahvilPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == true).Count();
                    var TahvilDerinlik = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == true && x.LisansDurum.TahvilLP == true).Count();
                    var AnalizPro = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.AnPro == true).Count();
                    var KapaliUserSayisi = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == false).Count();

                    var DJI = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.DJI).Count();
                    var SPI = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.SPI).Count();
                    var XETRA = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.XETRA).Count();
                    var CBOT = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CBOT).Count();
                    var CME = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CME).Count();
                    var CBOTM = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CBOTM).Count();
                    var CMEM = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CMEM).Count();
                    var EUREX = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.EUREX).Count();
                    var TARAMA = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TARAMA).Count();
                    var MKK = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.MKK).Count();
                    var GKKUL = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.GKKUL).Count();

                    var ProEkran = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && (x.StatusId == 1) && x.ProNonPro.Value).Count();
                    var NonPro = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && (x.StatusId == 1) && x.ProNonPro.Value == false).Count();





                    var g = new GunSonuToplamlar();
                    g.tarih = DateTime.Now;
                    g.TumUserSayisi = TumUserSayisi;
                    g.IseUserSayisi = IseUserSayisi;
                    g.TrkUserSayisi = TrkUserSayisi;
                    g.MuafUserSayisi = MuafUserSayisi;
                    g.KapaliUserSayisi = KapaliUserSayisi;

                    g.Desktop = pro;
                    g.Mobile = Cep;
                    g.DesktopMobile = ProCep;
                    g.Robot = robot;

                    g.PayYuzeysel = PayYuzeysel;
                    g.PayPlus = PayPlus;
                    g.PayDerinlik = PayDerinlik;
                    g.Pd2P = PayDerinlikPlus;
                    g.PayEndeks = PayEndeks;
                    g.PayGs = PayGs;

                    g.Analitik = analitik;
                    g.PITE = pite;

                    g.ViopYuzeysel = ViopYuzeysel;
                    g.ViopPlus = ViopPlus;
                    g.ViopDerinlik = ViopDerinlik;
                    g.Vd2P = ViopDerinlikPlus;
                    g.ViopGS = ViopGS;


                    g.TahvilYuzeysel = TahvilYuzeysel;
                    g.TahvilPlus = TahvilPlus;
                    g.TahvilDerinlik = TahvilDerinlik;

                    g.AnPro = AnalizPro;
                    g.AnPro = AnalizPro;
                    g.MKK = MKK;
                    g.TARAMA = TARAMA;
                    g.GKKUL = GKKUL;
                    g.CME = CME;

                    g.DJI = DJI;
                    g.SPI = SPI;
                    g.XETRA = XETRA;
                    g.CBOT = CBOT;
                    g.CME = CME;
                    g.CBOTM = CBOTM;
                    g.CMEM = CMEM;
                    g.EUREX = EUREX;


                    g.ProEkran = ProEkran;
                    g.NonProEkran = NonPro;

                    crm.GunSonuToplamlars.InsertOnSubmit(g);
                    crm.SubmitChanges();

                    formListeTransaction("Gün Sonu Toplamlar Yazıldı");

                    kontrolToplam = true;


                    #endregion
                    }
                }
                catch (Exception ex)
                {
                    formListeTransaction(ex.Message);
                }
            });
        }
        public string SendtoSSOMultiUser(string usernameX)
        {
            try
            {
                if (usernameX == "") return "ERROR";
                var sb = new StringBuilder();
                sb.Append("http://" + ssoip + ":" + ssoport + "/DFN?CRM?USERCHECK?" + usernameX + "?");
                var sonuc = sb.ToString()._RequestHttp();
                formListeTransaction(usernameX + "'nolu user için  SSO'ya çift bağlantı gönderildi");
                formListeTransaction("SSO Dönen Cevap : " + sonuc);
                return sonuc;
            }
            catch { return "ERROR"; }

        }
        public bool BuAyGelecekAyKontrolu(DateTime tarih)
        {
            try
            {
                var sonuc = false;

                var lastdate = DateTime.Now.Date;
                if (tarih.Date > lastdate) return false;
                // if (tarih.Date.Month == lastdate.Month && tarih.Date.Year == lastdate.Date.Year)
                if (tarih.Date <= lastdate)
                    sonuc = true;
                else
                    sonuc = false;

                return sonuc;

            }
            catch { return false; }

        }
        private void btnTest_Click(object sender, EventArgs e)
        {
            ToplamlariYaz();
        }
        private void AllSendPushNotMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new AdminViews.formPushNotification();
            frm.Show();
        }
        private void expiryDateKontrolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            kapalilarkontrol();
        }
        private void IpdaemonforCepServers_OnConnected(object sender, IpdaemonConnectedEventArgs e)
        {
            if (IpdaemonforCepServers.Connections.ContainsKey(e.ConnectionId))
            {
                var ip = IpdaemonforCepServers.Connections[e.ConnectionId].RemoteHost;
                if (listBoxCepServerList.Items.Contains(ip) == false)
                {
                    listBoxCepServerList.Items.Insert(0, ip);
                }

            }
        }
        private void IpdaemonforCepServers_OnDisconnected(object sender, IpdaemonDisconnectedEventArgs e)
        {

            var ip = IpdaemonforCepServers.Connections[e.ConnectionId].RemoteHost;
            if (listBoxCepServerList.Items.Contains(ip))
            {
                listBoxCepServerList.Items.Remove(ip);
            }


        }
        private void IpdaemonforCepServers_OnDataIn(object sender, IpdaemonDataInEventArgs e)
        {
            // CalisanlaraKomutGonder(e.Text);
        }
        private void lblDbip_Click(object sender, EventArgs e)
        {

        }
        void labelLoginHistoryYaz(string txt)
        {
            if (lblLoginHistory.InvokeRequired)
            {
                lblLoginHistory.Invoke((MethodInvoker)delegate { labelLoginHistoryYaz(txt); });
            }
            else
            {
                lblLoginHistory.Text = txt;
            }
        }
        void lblAutoCreateStatsYaz(string txt)
        {
            if (lblAutoCreateStats.InvokeRequired)
            {
                lblAutoCreateStats.Invoke((MethodInvoker)delegate { lblAutoCreateStatsYaz(txt); });
            }
            else
            {
                lblAutoCreateStats.Text = txt;
            }
        }
        private void IpportSso_OnDataIn(object sender, IpportDataInEventArgs e)
        {
            if (!LoginHistoryEnabled) return;
            try
            {
                var dataarr = e.Text.Split((char)2);
                foreach (var item in dataarr)
                {
                    if (item == "") continue;
                    if (item.StartsWith("SsoCheck"))
                    {
                        lastLoginHistory = item;
                        var username = item.Split('|')[1];
                        //var tckn = item.Split('|')[1];
                        var lh = new LoginHistory();

                        lh.username = username;
                        lh.LoginTime = DateTime.Now;

                        using (var crm = new crmDFNDataContext())
                        {
                            var userDetay = crm.Users.FirstOrDefault(x => x.UserName == username);
                            if (userDetay != null)
                            {
                                lh.Name = userDetay.Name;
                                lh.Surname = userDetay.Surname;
                                lh.PmtsNo = userDetay.PmtsNo;
                            }

                            var lhistory = crm.LoginHistories
                                .Where(x => x.username == username && x.LoginTime.Date == DateTime.Now.Date)
                                .OrderByDescending(x => x.LoginTime)
                                .FirstOrDefault();

                            if (lhistory != null)
                            {
                                if (lhistory.LoginTime.Date == DateTime.Now.Date)
                                {
                                    lhistory.LoginTime = DateTime.Now;
                                    lhistory.Name = lh.Name;
                                    lhistory.Surname = lh.Surname;
                                    lhistory.PmtsNo = lh.PmtsNo;
                                    crm.SubmitChanges();
                                }
                                else
                                {
                                    crm.LoginHistories.InsertOnSubmit(lh);
                                    crm.SubmitChanges();
                                }
                            }
                            else
                            {
                                crm.LoginHistories.InsertOnSubmit(lh);
                                crm.SubmitChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                formListeTransaction("Hata:Load" + ex.Message);
            }
        }
        private void timerKontrol_Tick(object sender, EventArgs e)
        {
            try
            {
                if (IpportSso.Connected == false)
                {
                    IpportSso.Interrupt();
                    IpportSso.Connect(ssoip, 4447);
                }
            }
            catch { }
        }
        void acilacaklarKontrolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            acilacaklarKontrol();
        }
        private void timerParseAutocreate_Tick(object sender, EventArgs e)
        {
            try
            {
                if (AuroCreateQuene.Count > 0)
                {
                    if (AuroCreateQuene.TryDequeue(out string msj))
                    {
                        var arr = msj.Split('~');
                        ParseAutoCreate(arr[0], arr[1]).GetAwaiter();
                    }

                }
            }
            catch { }
        }
        private void txtGunSonuInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {

                    var text = txtGunSonuInterval.Text.Trim();

                    if (int.TryParse(text, out int result))
                    {
                        GunSonuInterval = result;
                    }
                }


            }
            catch { }
        }

        private void sSLVePortBilgileriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formSslAyarlar frm = new formSslAyarlar();
            frm.ShowDialog();
        }

        private static DateTime? MaxDate(params DateTime?[] dates)
        {
            DateTime? max = null;
            foreach (var d in dates)
                if (d.HasValue && (max == null || d.Value > max.Value))
                    max = d;
            return max;
        }

        private static DateTime ToEndOfDay(DateTime dt) => dt.Date.AddDays(0).AddTicks(-1);
        // private static DateTime ToEndOfTime = new DateTime(dt.Year, dt.Month, 1).AddMonths(1).AddDays(-1);
        static DateTime? Max(DateTime? a, DateTime? b)
     => !a.HasValue ? b : (!b.HasValue ? a : (a > b ? a : b));

        static void EmirZinciri(DateTime? parentSD, DateTime? parentED,
                                ref bool childFlag, ref DateTime? childSD, ref DateTime? childED)
        {
            if (!parentSD.HasValue || !parentED.HasValue) return;

            if (!childFlag)
            {
                childFlag = true;
                if (!childSD.HasValue) childSD = parentSD;
                childED = Max(childED, parentED); // ED en az ebeveyn ED
            }
            else
            {
                //  ED küçükse ebeveyne hizala, büyükse dokunma
                if (!childED.HasValue || childED.Value < parentED.Value)
                    childED = parentED;
            }
        }
        private void SendResponse(string connectionId, string response, string requestId)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionId))
                {
                    MyTools.logyaz($"[{requestId}] HATA: ConnectionId null veya boş");
                    return;
                }

                var conns = HttpsService?.Connections;
                if (conns == null) { return; }
                lock (conns)
                {
                    if (conns != null && HttpsService.Connections.ContainsKey(connectionId))
                    {
                      // HttpsService.Connections[connectionId].DataToSendB = Encoding.UTF8.GetBytes(response._InsertHeaderHTTP());
                        var enc = Latin5;
                        HttpsService.Connections[connectionId].DataToSendB = enc.GetBytes(response._InsertHeaderHTTP());

                        HttpsService.Connections[connectionId].Connected = false;
                        MyTools.logyaz($"[{requestId}] Response gönderildi - ConnectionId: {connectionId}");
                    }
                    else
                    {
                        MyTools.logyaz($"[{requestId}] UYARI: Connection bulunamadı (muhtemelen timeout) - ConnectionId: {connectionId}");
                    }
                }
            }
            catch (Exception ex)
            {
                MyTools.logyaz($"[{requestId}] HATA: Response gönderilirken hata - ConnectionId: {connectionId}, Hata: {ex.Message}");
            }
        }

        /// <summary>
        /// MOBIL / KRMD1 / PAYX → hedefTarih'e kadar kapalıysa aç, açıksa uzat.
        /// YENI → monthEnd, LISANSEKLE → user.ExpiryDate geçirilir.
        /// GUNCELLE → çağrılmaz.
        /// </summary>
        private void OtoLisansAc(LisansDurum ld, DateTime hedefTarih, DateTime now, string requestId)
        {
            // MOBIL
            if (!ld.CepYetki || !ld.CepYetkiEnd.HasValue || ld.CepYetkiEnd.Value.Date < hedefTarih)
            {
                var eski = ld.CepYetki ? ld.CepYetkiEnd?.ToString("yyyyMMdd") : "KAPALI";
                ld.CepYetki = true;
                ld.CepYetkiStart = ld.CepYetkiStart ?? now;
                ld.CepYetkiEnd = hedefTarih;
                MyTools.logyaz($"[{requestId}] OtoLisans MOBIL: {eski} -> {hedefTarih:yyyyMMdd}");
            }

            // KRMD1
            if (!ld.COMEX || !ld.KRMD1End.HasValue || ld.KRMD1End.Value.Date < hedefTarih)
            {
                var eski = ld.COMEX ? ld.KRMD1End?.ToString("yyyyMMdd") : "KAPALI";
                ld.COMEX = true;
                ld.KRMD1Start = ld.KRMD1Start ?? now;
                ld.KRMD1End = hedefTarih;
                MyTools.logyaz($"[{requestId}] OtoLisans KRMD1: {eski} -> {hedefTarih:yyyyMMdd}");
            }

            // PAYX (END)
            if (!ld.PayX || !ld.PayXEnd.HasValue || ld.PayXEnd.Value.Date < hedefTarih)
            {
                var eski = ld.PayX ? ld.PayXEnd?.ToString("yyyyMMdd") : "KAPALI";
                ld.PayX = true;
                ld.PayXStart = ld.PayXStart ?? now;
                ld.PayXEnd = hedefTarih;
                MyTools.logyaz($"[{requestId}] OtoLisans PAYX: {eski} -> {hedefTarih:yyyyMMdd}");
            }
        }

        #region çalışan
        private void HttpsService_OnDataIn(object sender, IpdaemonDataInEventArgs e)
        {
            var requestId = Guid.NewGuid().ToString("N").Substring(0, 8);
            var startTime = DateTime.Now;
            string remoteHost = "Unknown";
            string inputText = null;
            byte[] inputBytes = null;
            string connectionId = null;
            try
            {

                if (e == null)
                {
                    MyTools.logyaz($"[{requestId}] HATA: Event args null");
                    return;
                }

                connectionId = e.ConnectionId;

                if (string.IsNullOrEmpty(connectionId))
                {
                    MyTools.logyaz($"[{requestId}] HATA: ConnectionId null veya boş");
                    return;
                }

                var conns = HttpsService?.Connections;
                if (conns == null)
                {
                    MyTools.logyaz($"[{requestId}] HATA: HttpsService.Connections null");
                    return;
                }
                lock (conns)
                {

                    if (!conns.ContainsKey(connectionId))
                    {
                        MyTools.logyaz($"[{requestId}] HATA: Connection bulunamadı - ConnectionId: {connectionId}");
                        return;
                    }

                    try
                    {
                        remoteHost = HttpsService.Connections[connectionId]?.RemoteHost ?? "Unknown";
                        inputText = e.Text;
                        inputBytes = e.TextB;
                    }
                    catch (Exception initEx)
                    {
                        MyTools.logyaz($"[{requestId}] HATA: İlk değer alma hatası - {initEx.Message}");
                        return;
                    }
                }

                ThreadPool.QueueUserWorkItem(state =>
                {
                    var threadStartTime = DateTime.Now;
                    var now = DateTime.Now.Date;  
                    //  MyTools.logyaz($"[{requestId}] Thread başladı - ThreadId: {Thread.CurrentThread.ManagedThreadId}");

                    crmDFNDataContext crm = null;

                    try
                    {

                        var dbConnStart = DateTime.Now;
                        crm = new crmDFNDataContext();
                        //crm.CommandTimeout = 30;
                        //crm.ObjectTrackingEnabled = false;
                        var dlo = new DataLoadOptions();
                        dlo.LoadWith<User>(u => u.LisansDurum);
                        crm.LoadOptions = dlo;
                        var dbConnDuration = (DateTime.Now - dbConnStart).TotalMilliseconds;
                        // MyTools.logyaz($"[{requestId}] DB bağlantısı oluşturuldu - Süre: {dbConnDuration}ms");

                        UserEvent userevent = new UserEvent();
                        userevent.EventTarih = DateTime.Now;
                        userevent.IP = remoteHost;
                        userevent.HostName = remoteHost;
                     //  userevent.CalisanId = 2;

                        string responsestr = "";
                        try
                        {
                            string inputmsg = inputText;

                            if (string.IsNullOrEmpty(inputmsg))
                            {
                                MyTools.logyaz($"[{requestId}] HATA: Input message null veya boş");
                                SendResponse(connectionId, "ERROR", requestId);
                                return;
                            }

                            // MyTools.logyaz($"[{requestId}] Input message uzunluğu: {inputmsg.Length}");

                            if (inputmsg.Length > 20000 || inputmsg.Length < 5)
                            {
                                MyTools.logyaz($"[{requestId}] HATA: Geçersiz mesaj uzunluğu: {inputmsg.Length}");

                                if (inputmsg.Length >= 3 && inputmsg.Substring(0, 3) == "GET")
                                {
                                    SendResponse(connectionId, "ERROR", requestId);
                                    return;
                                }
                            }

                            #region Get
                            if (inputmsg.Length >= 3 && inputmsg.Substring(0, 3) == "GET")
                            {
                                var parseStart = DateTime.Now;

                                int startpos = inputmsg.IndexOf("/");
                                int endpos = inputmsg.IndexOf(" HTTP");

                                if (endpos > startpos && startpos > 0 && endpos > 0)
                                {
                                    string message = inputmsg.Substring(startpos + 1, endpos - startpos - 1);
                                    message = message.Replace("%20", " ").Replace("%7C", "|");
                                    message = message.TurkceHttpResponse();

                                    if (message == "favicon.ico")
                                    {
                                        SendResponse(connectionId, " ", requestId);
                                        return;
                                    }

                                    #region AKYATIRIM
                                    if (message.StartsWith("AKYATIRIM"))
                                    {
                                        userevent.CalisanId = 2;
                                        var sonuc = "";
                                        var ekransonuc = "";
                                        var fieldArray = message.Split('?');
                                        var komut = "";
                                        var sifre = "Akyatirim1";
                                        var ExpriyDate = "";
                                        var ad = "";
                                        var soyad = "";
                                        var ulke = "";
                                        var sehir = "";
                                        var aciklama = "";
                                        var musteriNo = "";
                                        var userName = "";
                                        var mail = "";
                                        var adres = "";
                                        var tel = "";
                                        var sube = "";
                                        var personel = "";
                                        var mbb = "";
                                         var tckno = "";

                                        var PRO = false;
                                        var MOBIL = false;
                                        var KRMD1 = false;
                                        // var PD1 = false;
                                        var PD1P = false;
                                        var PD2 = false;
                                        var PD2P = false;
                                        var PIT = false;
                                        var END = false;
                                        var PITE = false;
                                        // var VD1 = false;
                                        var VD1P = false;
                                        var VD2 = false;
                                        var VD2P = false;
                                        var VIT = false;
                                        // var BD1 = false;
                                        var BD1P = false;
                                        var BD2 = false;
                                        var CME = false;
                                        var ROBOT = false;
                                        var MKK = false;
                                        var GKKUL = false;

                                        #region startdate
                                        var PROSD = "";
                                        var MOBILSD = "";
                                        var KRMD1SD = "";
                                        // var PD1SD = "";
                                        var PD1PSD = "";
                                        var PD2SD = "";
                                        var PD2PSD = "";
                                        var PITSD = "";
                                        var ENDSD = "";
                                        var PITESD = "";
                                        // var VD1SD = "";
                                        var VD1PSD = "";
                                        var VD2SD = "";
                                        var VD2PSD = "";
                                        var VITSD = "";
                                        // var BD1SD = "";
                                        var BD1PSD = "";
                                        var BD2SD = "";
                                        var CMESD = "";
                                        var MKKSD = "";
                                        var GKKULSD = "";
                                        #endregion
                                        #region enddate
                                        var PROED = "";
                                        var MOBILED = "";
                                        var KRMD1ED = "";
                                        // var PD1ED = "";
                                        var PD1PED = "";
                                        var PD2ED = "";
                                        var PD2PED = "";
                                        var PITED = "";
                                        var ENDED = "";
                                        var PITEED = "";
                                        // var VD1ED = "";
                                        var VD1PED = "";
                                        var VD2ED = "";
                                        var VD2PED = "";
                                        var VITED = "";
                                        // var BD1ED = "";
                                        var BD1PED = "";
                                        var BD2ED = "";
                                        var CMEED = "";
                                        var MKKED = "";
                                        var GKKULED = "";
                                        #endregion

                                        var karmaGeldi = false;
                                        // var Pd1Geldi = false;
                                        var Pd1pGeldi = false;
                                        var Pd2Geldi = false;
                                        var Pd2PGeldi = false;
                                        // var Vd1Geldi = false;
                                        var Vd1pGeldi = false;
                                        var Vd2Geldi = false;
                                        var Vd2PGeldi = false;

                                        #region PARSE
                                        var parseFieldStart = DateTime.Now;
                                        for (int i = 0; i < fieldArray.Length; i++)
                                        {
                                            var splitarray = fieldArray[i].Split('=');
                                            if (splitarray.Length != 2) continue;
                                            switch (splitarray[0]._ToEngUp())
                                            {
                                                #region KOMUT
                                                case "AKYATIRIM": komut = splitarray[1].Trim();break;
                                                #endregion
                                                #region IDEALSIFRE
                                                case "IDEALSIFRE": sifre = splitarray[1].Trim(); break;
                                                #endregion
                                                #region EXPIREDATE
                                                case "EXPIREDATE":
                                                    ExpriyDate = splitarray[1].Trim(); break;

                                                //if (splitarray[1].Length < 7)
                                                //    break;
                                                //var newDate = DateTime.ParseExact(splitarray[1], "yyyyMMdd", CultureInfo.InvariantCulture);

                                                //newcustomer.ExpiryDate = newDate; break; 
                                                #endregion
                                                #region AD-SOYAD
                                                case "AD": ad = splitarray[1].Trim(); break;
                                                case "SOYAD": soyad = splitarray[1].Trim(); break;
                                                #endregion
                                                #region ISIM
                                                case "ISIM":

                                                    var gelenisim = splitarray[1].Trim().Split(' ');

                                                    if (gelenisim.Length == 1)
                                                    {
                                                        ad = gelenisim[0];
                                                        soyad = "yok";

                                                    }
                                                    else if (gelenisim.Length == 2)
                                                    {
                                                        ad = gelenisim[0];
                                                        soyad = gelenisim[1];

                                                    }
                                                    else if (gelenisim.Length == 3)
                                                    {
                                                        ad = gelenisim[0] + " " + gelenisim[1];
                                                        soyad = gelenisim[2];
                                                    }
                                                    break;
                                                #endregion
                                                #region ULKE
                                                case "ULKE": ulke = splitarray[1].Trim(); break;
                                                #endregion
                                                #region SEHIR
                                                case "SEHIR": sehir = splitarray[1].Trim(); break;
                                                #endregion
                                                #region MUSNO
                                                case "MUSNO": musteriNo = splitarray[1].Trim(); break;
                                                case "TCKN": userName = splitarray[1].Trim();  break;
                                                case "USERNAME": userName = splitarray[1].Trim();  break;
                                                case "TCKNO": tckno = splitarray[1].Trim(); break;
                                                #endregion
                                                #region tckn
                                                //case "TCKN": tckno = splitarray[1].Trim(); break;
                                                //case "TCNO": tckno = splitarray[1].Trim(); break;

                                                #endregion
                                                #region MAIL
                                                case "MAIL": mail = splitarray[1].Trim(); break;
                                                #endregion
                                                #region ADRES
                                                case "ADRES": adres = splitarray[1].Trim(); break;
                                                #endregion
                                                #region TELEFON
                                                case "TEL": tel = splitarray[1].Trim(); break;
                                                #endregion
                                                #region SUBE
                                                case "SUBE": sube = splitarray[1].Trim(); break;
                                                #endregion
                                                #region MBB
                                                case "MBB":
                                                    mbb = splitarray[1].Trim(); break;
                                                // musteriNo = splitarray[1].Trim();
                                                #endregion
                                                #region PERSONEL
                                                case "PERSONEL": personel = splitarray[1].Trim(); break;
                                                #endregion
                                                #region ACIKLAMA
                                                case "ACIKLAMA": aciklama = splitarray[1].Trim(); break;
                                                #endregion
                                                #region lisanslar
                                                case "PRO": PRO = splitarray[1].Trim()._ToBool(); break;
                                                case "MOBIL": MOBIL = splitarray[1].Trim()._ToBool(); break;
                                                case "KRMD1": KRMD1 = splitarray[1].Trim()._ToBool(); karmaGeldi = true; break;
                                                // case "PD1": PD1 = splitarray[1].Trim()._ToBool(); Pd1Geldi = true; break;
                                                case "PD1P": PD1P = splitarray[1].Trim()._ToBool(); Pd1pGeldi = true; break;
                                                case "PD2": PD2 = splitarray[1].Trim()._ToBool(); Pd2Geldi = true; break;
                                                case "PD2P": PD2P = splitarray[1].Trim()._ToBool(); Pd2PGeldi = true; break;
                                                case "PIT": PIT = splitarray[1].Trim()._ToBool(); break;
                                                case "END": END = splitarray[1].Trim()._ToBool(); break;
                                                case "PITE": PITE = splitarray[1].Trim()._ToBool(); break;

                                                //   case "VD1": VD1 = splitarray[1].Trim()._ToBool(); Vd1Geldi = true; break;
                                                case "VD1P": VD1P = splitarray[1].Trim()._ToBool(); Vd1pGeldi = true; break;
                                                case "VD2": VD2 = splitarray[1].Trim()._ToBool(); Vd2Geldi = true; break;
                                                case "VD2P": VD2P = splitarray[1].Trim()._ToBool(); Vd2PGeldi = true; break;
                                                case "VIT": VIT = splitarray[1].Trim()._ToBool(); break;
                                                // case "BD1": BD1 = splitarray[1].Trim()._ToBool(); break;
                                                case "BD1P": BD1P = splitarray[1].Trim()._ToBool(); break;
                                                case "BD2": BD2 = splitarray[1].Trim()._ToBool(); break;
                                                // case "CME": CME = splitarray[1].Trim()._ToBool(); break;
                                                case "ROBOT": ROBOT = splitarray[1].Trim()._ToBool(); break;
                                                case "MKK": MKK = splitarray[1].Trim()._ToBool(); break;
                                                case "GKKUL": GKKUL = splitarray[1].Trim()._ToBool(); break;


                                                case "PROSD": PROSD = splitarray[1].Trim(); break;
                                                case "MOBILSD": MOBILSD = splitarray[1].Trim(); break;
                                                case "KRMD1SD": KRMD1SD = splitarray[1].Trim(); break;
                                                // case "PD1SD": PD1SD = splitarray[1].Trim(); break;
                                                case "PD1PSD": PD1PSD = splitarray[1].Trim(); break;
                                                case "PD2SD": PD2SD = splitarray[1].Trim(); break;
                                                case "PD2PSD": PD2PSD = splitarray[1].Trim(); break;
                                                case "PITSD": PITSD = splitarray[1].Trim(); break;
                                                case "ENDSD": ENDSD = splitarray[1].Trim(); break;
                                                case "PITESD": PITESD = splitarray[1].Trim(); break;
                                                // case "VD1SD": VD1SD = splitarray[1].Trim(); break;
                                                case "VD1PSD": VD1PSD = splitarray[1].Trim(); break;
                                                case "VD2SD": VD2SD = splitarray[1].Trim(); break;
                                                case "VD2PSD": VD2PSD = splitarray[1].Trim(); break;
                                                case "VITSD": VITSD = splitarray[1].Trim(); break;
                                                // case "BD1SD": BD1SD = splitarray[1].Trim(); break;
                                                case "BD1PSD": BD1PSD = splitarray[1].Trim(); break;
                                                case "BD2SD": BD2SD = splitarray[1].Trim(); break;
                                                // case "CMESD": CMESD = splitarray[1].Trim(); break;

                                                case "MKKSD": MKKSD = splitarray[1].Trim(); break;
                                                case "GKKULSD": GKKULSD = splitarray[1].Trim(); break;
                                                case "PROED": PROED = splitarray[1].Trim(); break;
                                                case "MOBILED": MOBILED = splitarray[1].Trim(); break;
                                                case "KRMD1ED": KRMD1ED = splitarray[1].Trim(); break;
                                                // case "PD1ED": PD1ED = splitarray[1].Trim(); break;
                                                case "PD1PED": PD1PED = splitarray[1].Trim(); break;
                                                case "PD2ED": PD2ED = splitarray[1].Trim(); break;
                                                case "PD2PED": PD2PED = splitarray[1].Trim(); break;
                                                case "PITED": PITED = splitarray[1].Trim(); break;
                                                case "ENDED": ENDED = splitarray[1].Trim(); break;
                                                case "PITEED": PITEED = splitarray[1].Trim(); break;
                                                // case "VD1ED": VD1ED = splitarray[1].Trim(); break;
                                                case "VD1PED": VD1PED = splitarray[1].Trim(); break;
                                                case "VD2ED": VD2ED = splitarray[1].Trim(); break;
                                                case "VD2PED": VD2PED = splitarray[1].Trim(); break;
                                                case "VITED": VITED = splitarray[1].Trim(); break;
                                                // case "BD1ED": BD1ED = splitarray[1].Trim(); break;
                                                case "BD1PED": BD1PED = splitarray[1].Trim(); break;
                                                case "BD2ED": BD2ED = splitarray[1].Trim(); break;
                                                // case "CMEED": CMEED = splitarray[1].Trim(); break;
                                                case "MKKED": MKKED = splitarray[1].Trim(); break;
                                                case "GKKULED": GKKULED = splitarray[1].Trim(); break;

                                                case "ALG": CME = splitarray[1].Trim()._ToBool(); break;
                                                case "ALGSD": CMESD = splitarray[1].Trim(); break;
                                                case "ALGED": CMEED = splitarray[1].Trim(); break;
                                                    #endregion

                                            }
                                        }
                                        var parseFieldDuration = (DateTime.Now - parseFieldStart).TotalMilliseconds;
                                       
                                        #endregion

                                        #region NEWUSER
                                        if (komut == "YENI")
                                        {
                                            
                                            MyTools.logyaz(inputText);
                                            var operationStart = DateTime.Now;
                                            var sonucYeni = new Sonuc();
                                            if (string.IsNullOrWhiteSpace(userName) )
                                            {
                                                var err = new { Kod = "102", Aciklama = "HATA?MESAJ=USERNAME zorunlu", Status = "Gecersiz USERNAME" };
                                                SendResponse(connectionId, new JavaScriptSerializer().Serialize(err), requestId);
                                                return;
                                            }

                                            if (string.IsNullOrWhiteSpace(ExpriyDate))
                                            {
                                                MyTools.logyaz($"[{requestId}] HATA: EXPIREDATE eksik");
                                                var err = new { Kod = "103", Aciklama = "HATA?MESAJ=EXPIREDATE zorunlu", Status = "Eksik Bilgi" };
                                                SendResponse(connectionId, new JavaScriptSerializer().Serialize(err), requestId);
                                                return;

                                            }

                                            var acilanlar = new StringBuilder();
                                            var dbQueryStart = DateTime.Now;
                                            bool userExists = crm.Users.Any(x => x.UserName == userName);
                                            var dbQueryDuration = (DateTime.Now - dbQueryStart).TotalMilliseconds;

                                            if (!userExists)
                                            {

                                                userevent.EventTypeId = 5;
                                                var newcustomer = new User();
                                                var lisansd = new LisansDurum();
                                                var iletisim = new Iletisim();
                                                var kurumbilgi = new KurumsalBilgiler();
                                                newcustomer.tckno = tckno;
                                                // newcustomer.tckno = tckno;
                                                newcustomer.UserName = userName;
                                                // newcustomer.UserName = tckno;
                                                // newcustomer.PmtsNo = "10011";
                                                if (!string.IsNullOrEmpty(musteriNo))
                                                    newcustomer.PmtsNo = musteriNo;
                                                // newcustomer.PmtsNo = tckno;

                                                newcustomer.Aciklama = aciklama;
                                                newcustomer.Name = ad;
                                                newcustomer.Surname = soyad;
                                                newcustomer.Password = MyTools.Sifreleme.Encryp(sifre);
                                                //newcustomer.ExpiryDate = !string.IsNullOrEmpty(ExpriyDate)
                                                //? ExpriyDate.StrinToDateTime()
                                                //: DateTime.Now.AddYears(20)._LastDayOfMonth();
                                                newcustomer.ExpiryDate = ExpriyDate.StrinToDateTime();
                                                newcustomer.MusteriMenseiID = 1;
                                                newcustomer.ProductType = "IDEAL";
                                                newcustomer.BaslangicTarihi = DateTime.Now;
                                                newcustomer.StatusId = 1;

                                                #region Lisanslar
                                                lisansd.YayinDurumu = true;
                                                lisansd.Futgck = true;
                                                lisansd.WINX = true;
                                                lisansd.ProYetki = PRO;
                                                lisansd.CepYetki = MOBIL;

                                                lisansd.ROBOT = ROBOT;

                                                var sonucNewUser = new Sonuc();
                                                if (PRO || (!string.IsNullOrEmpty(PROSD) && !string.IsNullOrEmpty(PROED)))
                                                {
                                                    if (string.IsNullOrEmpty(PROSD) || string.IsNullOrEmpty(PROED))
                                                    {
                                                        sonucNewUser.Kod = "103";
                                                        sonucNewUser.Aciklama = "HATA?MESAJ=PRO lisansı için PROSD/PROED zorunlu";
                                                        sonucNewUser.Status = "Eksik Bilgi";
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=PRO lisansı için PROSD/PROED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonR = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonR, requestId);
                                                        return;
                                                    }
                                                    lisansd.ProYetkiStart = PROSD.StrinToDateTime();
                                                    lisansd.ProYetkiEnd = PROED.StrinToDateTime();
                                                    if (BuAyGelecekAyKontrolu((DateTime)lisansd.ProYetkiStart)) lisansd.ProYetki = true;
                                                }

                                                if (MOBIL || (!string.IsNullOrEmpty(MOBILSD) && !string.IsNullOrEmpty(MOBILED)))
                                                {
                                                    if (string.IsNullOrEmpty(MOBILSD) || string.IsNullOrEmpty(MOBILED))
                                                    {
                                                        sonucNewUser.Kod = "103";
                                                        sonucNewUser.Aciklama = "HATA?MESAJ=MOBIL lisansı için MOBILSD/MOBILED zorunlu";
                                                        sonucNewUser.Status = "Eksik Bilgi";
                                                        var jsonR = new JavaScriptSerializer().Serialize(sonucNewUser);
                                                        SendResponse(connectionId, jsonR, requestId);
                                                        return;
                                                    }
                                                    lisansd.CepYetkiStart = MOBILSD.StrinToDateTime();
                                                    lisansd.CepYetkiEnd = MOBILED.StrinToDateTime();
                                                    if (BuAyGelecekAyKontrolu((DateTime)lisansd.CepYetkiStart)) lisansd.CepYetki = true;
                                                }

                                                bool proSdGecerli = !string.IsNullOrEmpty(PROSD)
                                                                     && BuAyGelecekAyKontrolu(PROSD.StrinToDateTime());
                                                bool mobilSdGecerli = !string.IsNullOrEmpty(MOBILSD)
                                                                     && BuAyGelecekAyKontrolu(MOBILSD.StrinToDateTime());

                                                if (proSdGecerli || mobilSdGecerli)
                                                    lisansd.YayinDurumu = true;
                                                
                                                #region PD1 KALDIRILDI
                                                //if (PD1 || (!string.IsNullOrEmpty(PD1SD) && !string.IsNullOrEmpty(PD1ED)))
                                                //{
                                                //    if (string.IsNullOrEmpty(PD1SD) || string.IsNullOrEmpty(PD1ED))
                                                //    {
                                                //        var err = new Sonuc
                                                //        {
                                                //            Kod = "103",
                                                //            Aciklama = "HATA?MESAJ=PD1 lisansı için PD1SD/PD1ED zorunlu",
                                                //            Status = "Eksik Bilgi"
                                                //        };
                                                //        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                //        HttpsService.Connections[e.ConnectionId].DataToSendB =
                                                //            Encoding.UTF8.GetBytes(jsonErr._InsertHeaderHTTP());
                                                //        HttpsService.Connections[e.ConnectionId].Connected = false;
                                                //        return;
                                                //    }
                                                //    lisansd.PayL1Start = PD1SD.StrinToDateTime();
                                                //    lisansd.PayL1End = PD1ED.StrinToDateTime();
                                                //    if (lisansd.PayL1Start.HasValue && BuAyGelecekAyKontrolu(lisansd.PayL1Start.Value)) lisansd.PayL1 = true;
                                                //}
                                                #endregion
                                                if (PD1P || (!string.IsNullOrEmpty(PD1PSD) && !string.IsNullOrEmpty(PD1PED)))
                                                {
                                                    if (string.IsNullOrEmpty(PD1PSD) || string.IsNullOrEmpty(PD1PED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=PD1P lisansı için PD1PSD/PD1PED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.PayLPStart = PD1PSD.StrinToDateTime();
                                                    lisansd.PayLPEnd = PD1PED.StrinToDateTime();
                                                    lisansd.PayL1Start = PD1PSD.StrinToDateTime();
                                                    lisansd.PayL1End = PD1PED.StrinToDateTime();
                                                    if (lisansd.PayLPStart.HasValue && BuAyGelecekAyKontrolu(lisansd.PayLPStart.Value))
                                                    {
                                                        lisansd.PayLP = true;
                                                        lisansd.PayL1 = true;
                                                    }
                                                }
                                                if (PD2 || (!string.IsNullOrEmpty(PD2SD) && !string.IsNullOrEmpty(PD2ED)))
                                                {
                                                    if (string.IsNullOrEmpty(PD2SD) || string.IsNullOrEmpty(PD2ED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=PD2 lisansı için PD2SD/PD2ED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }

                                                    lisansd.PayL2Start = PD2SD.StrinToDateTime();
                                                    lisansd.PayL2End = PD2ED.StrinToDateTime();
                                                    lisansd.PayLPStart = PD2SD.StrinToDateTime();
                                                    lisansd.PayLPEnd = PD2ED.StrinToDateTime();
                                                    lisansd.PayL1Start = PD2SD.StrinToDateTime();
                                                    lisansd.PayL1End = PD2ED.StrinToDateTime();
                                                    if (lisansd.PayL2Start.HasValue && BuAyGelecekAyKontrolu(lisansd.PayL2Start.Value))
                                                    {
                                                        lisansd.PayL2 = true; lisansd.PayLP = true;
                                                        lisansd.PayL1 = true;
                                                    }
                                                }
                                                if (PD2P || (!string.IsNullOrEmpty(PD2PSD) && !string.IsNullOrEmpty(PD2PED)))
                                                {
                                                    if (string.IsNullOrEmpty(PD2PSD) || string.IsNullOrEmpty(PD2PED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=PD2P lisansı için PD2PSD/PD2PED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }

                                                    lisansd.Pd2PStart = PD2PSD.StrinToDateTime();
                                                    lisansd.Pd2PEnd = PD2PED.StrinToDateTime();
                                                    lisansd.PayL2Start = PD2PSD.StrinToDateTime();
                                                    lisansd.PayL2End = PD2PED.StrinToDateTime();
                                                    lisansd.PayLPStart = PD2PSD.StrinToDateTime();
                                                    lisansd.PayLPEnd = PD2PED.StrinToDateTime();
                                                    lisansd.PayL1Start = PD2PSD.StrinToDateTime();
                                                    lisansd.PayL1End = PD2PED.StrinToDateTime();
                                                    if (lisansd.Pd2PStart.HasValue && BuAyGelecekAyKontrolu(lisansd.Pd2PStart.Value))
                                                    {
                                                        lisansd.Pd2P = true; lisansd.PayL2 = true; lisansd.PayLP = true;
                                                        lisansd.PayL1 = true;
                                                    }
                                                }

                                                if (PIT || (!string.IsNullOrEmpty(PITSD) && !string.IsNullOrEmpty(PITED)))
                                                {
                                                    if (string.IsNullOrEmpty(PITSD) || string.IsNullOrEmpty(PITED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=PIT lisansı için PITSD/PITED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.PayGSStart = PITSD.StrinToDateTime();
                                                    lisansd.PayGSEnd = PITED.StrinToDateTime();
                                                    if (lisansd.PayGSStart.HasValue && BuAyGelecekAyKontrolu(lisansd.PayGSStart.Value)) lisansd.PayGS = true;
                                                }


                                                if (END || (!string.IsNullOrEmpty(ENDSD) && !string.IsNullOrEmpty(ENDED)))
                                                {
                                                    if (string.IsNullOrEmpty(ENDSD) || string.IsNullOrEmpty(ENDED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=END lisansı için ENDSD/ENDED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.PayXStart = ENDSD.StrinToDateTime();
                                                    lisansd.PayXEnd = ENDED.StrinToDateTime();
                                                    if (lisansd.PayXStart.HasValue && BuAyGelecekAyKontrolu(lisansd.PayXStart.Value)) lisansd.PayX = true;
                                                }
                                                else
                                                {
                                                    // incelediğim kadarıyla SD/ED gelmediyse eski davranışa sadık kalmalı  öfk
                                                    // lisansd.PayX = true;
                                                }

                                                if (PITE || (!string.IsNullOrEmpty(PITESD) && !string.IsNullOrEmpty(PITEED)))
                                                {
                                                    if (string.IsNullOrEmpty(PITESD) || string.IsNullOrEmpty(PITEED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=PITE lisansı için PITESD/PITEED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.PayPiteStart = PITESD.StrinToDateTime();
                                                    lisansd.PayPiteEnd = PITEED.StrinToDateTime();
                                                    if (lisansd.PayPiteStart.HasValue && BuAyGelecekAyKontrolu(lisansd.PayPiteStart.Value))
                                                    {
                                                        lisansd.PITE = true;

                                                        if (!lisansd.PayGSStart.HasValue || !lisansd.PayGSEnd.HasValue)
                                                        {
                                                            // PIT hiç açık değilse, PITE ile aynı süreye aç
                                                            lisansd.PayGSStart = lisansd.PayPiteStart;
                                                            lisansd.PayGSEnd = lisansd.PayPiteEnd;
                                                        }
                                                        else if (lisansd.PayGSEnd.Value < lisansd.PayPiteEnd.Value)
                                                        {
                                                            // PIT açık ama daha erken bitiyorsa, PITE'nin bitiş tarihine uzat
                                                            lisansd.PayGSEnd = lisansd.PayPiteEnd;
                                                        }

                                                        if (BuAyGelecekAyKontrolu(lisansd.PayGSStart.Value))
                                                            lisansd.PayGS = true;
                                                    }
                                                }

                                                #region VD1 KALDIRILDI
                                                //if (VD1 || (!string.IsNullOrEmpty(VD1SD) && !string.IsNullOrEmpty(VD1ED)))
                                                //{
                                                //    if (string.IsNullOrEmpty(VD1SD) || string.IsNullOrEmpty(VD1ED))
                                                //    {
                                                //        var err = new Sonuc
                                                //        {
                                                //            Kod = "103",
                                                //            Aciklama = "HATA?MESAJ=VD1 lisansı için VD1SD/VD1ED zorunlu",
                                                //            Status = "Eksik Bilgi"
                                                //        };
                                                //        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                //        HttpsService.Connections[e.ConnectionId].DataToSendB =
                                                //            Encoding.UTF8.GetBytes(jsonErr._InsertHeaderHTTP());
                                                //        HttpsService.Connections[e.ConnectionId].Connected = false;
                                                //        return;
                                                //    }
                                                //    lisansd.ViopL1Start = VD1SD.StrinToDateTime();
                                                //    lisansd.ViopL1End = VD1ED.StrinToDateTime();
                                                //    if (lisansd.ViopL1Start.HasValue && BuAyGelecekAyKontrolu(lisansd.ViopL1Start.Value)) lisansd.ViopL1 = true;
                                                //}
                                                #endregion
                                                if (VD1P || (!string.IsNullOrEmpty(VD1PSD) && !string.IsNullOrEmpty(VD1PED)))
                                                {
                                                    if (string.IsNullOrEmpty(VD1PSD) || string.IsNullOrEmpty(VD1PED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=VD1P lisansı için VD1PSD/VD1PED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.ViopLPStart = VD1PSD.StrinToDateTime();
                                                    lisansd.ViopLPEnd = VD1PED.StrinToDateTime();
                                                    lisansd.ViopL1Start = VD1PSD.StrinToDateTime();
                                                    lisansd.ViopL1End = VD1PED.StrinToDateTime();
                                                    if (lisansd.ViopLPStart.HasValue && BuAyGelecekAyKontrolu(lisansd.ViopLPStart.Value))
                                                    {
                                                        lisansd.ViopLP = true;
                                                        lisansd.ViopL1 = true;
                                                    }
                                                }
                                                if (VD2 || (!string.IsNullOrEmpty(VD2SD) && !string.IsNullOrEmpty(VD2ED)))
                                                {
                                                    if (string.IsNullOrEmpty(VD2SD) || string.IsNullOrEmpty(VD2ED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=VD2 lisansı için VD2SD/VD2ED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.ViopL2Start = VD2SD.StrinToDateTime();
                                                    lisansd.ViopL2End = VD2ED.StrinToDateTime();
                                                    lisansd.ViopLPStart = VD2SD.StrinToDateTime();
                                                    lisansd.ViopLPEnd = VD2ED.StrinToDateTime();
                                                    lisansd.ViopL1Start = VD2SD.StrinToDateTime();
                                                    lisansd.ViopL1End = VD2ED.StrinToDateTime();
                                                    if (lisansd.ViopL2Start.HasValue && BuAyGelecekAyKontrolu(lisansd.ViopL2Start.Value))
                                                    {
                                                        lisansd.ViopL2 = true; lisansd.ViopLP = true;
                                                        lisansd.ViopL1 = true;
                                                    }
                                                }
                                                if (VD2P || (!string.IsNullOrEmpty(VD2PSD) && !string.IsNullOrEmpty(VD2PED)))
                                                {
                                                    if (string.IsNullOrEmpty(VD2PSD) || string.IsNullOrEmpty(VD2PED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=VD2P lisansı için VD2PSD/VD2PED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.Vd2PStart = VD2PSD.StrinToDateTime();
                                                    lisansd.Vd2PEnd = VD2PED.StrinToDateTime();
                                                    lisansd.ViopL2Start = VD2PSD.StrinToDateTime();
                                                    lisansd.ViopL2End = VD2PED.StrinToDateTime();
                                                    lisansd.ViopLPStart = VD2PSD.StrinToDateTime();
                                                    lisansd.ViopLPEnd = VD2PED.StrinToDateTime();
                                                    lisansd.ViopL1Start = VD2PSD.StrinToDateTime();
                                                    lisansd.ViopL1End = VD2PED.StrinToDateTime();
                                                    if (lisansd.Vd2PStart.HasValue && BuAyGelecekAyKontrolu(lisansd.Vd2PStart.Value))
                                                    {
                                                        lisansd.Vd2P = true; lisansd.ViopL2 = true; lisansd.ViopLP = true;
                                                        lisansd.ViopL1 = true;
                                                    }

                                                    //EmirZinciri(lisansd.Vd2PStart, lisansd.Vd2PEnd,
                                                    //ref lisansd.ViopL2, ref lisansd.ViopL2Start, ref lisansd.ViopL2End);

                                                    //EmirZinciri(lisansd.Vd2PStart, lisansd.Vd2PEnd,
                                                    //            ref lisansd.ViopLP, ref lisansd.ViopLPStart, ref lisansd.ViopLPEnd);
                                                }

                                                #region TAHVIL  1 KALDIRILDI
                                                //if (BD1 || (!string.IsNullOrEmpty(BD1SD) && !string.IsNullOrEmpty(BD1ED)))
                                                //{
                                                //    if (string.IsNullOrEmpty(BD1SD) || string.IsNullOrEmpty(BD1ED))
                                                //    {
                                                //        var err = new Sonuc
                                                //        {
                                                //            Kod = "103",
                                                //            Aciklama = "HATA?MESAJ=BD1 lisansı için BD1SD/BD1ED zorunlu",
                                                //            Status = "Eksik Bilgi"
                                                //        };
                                                //        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                //        HttpsService.Connections[e.ConnectionId].DataToSendB =
                                                //            Encoding.UTF8.GetBytes(jsonErr._InsertHeaderHTTP());
                                                //        HttpsService.Connections[e.ConnectionId].Connected = false;
                                                //        return;
                                                //    }

                                                //    lisansd.TahvilL1Start = BD1SD.StrinToDateTime();
                                                //    lisansd.TahvilL1End = BD1ED.StrinToDateTime();
                                                //    if (lisansd.TahvilL1Start.HasValue && BuAyGelecekAyKontrolu(lisansd.TahvilL1Start.Value))
                                                //        lisansd.TahvilL1 = true;
                                                //}
                                                #endregion

                                                // TAHVIL — BD1P (Düzey 1 Plus)  Plus açılırsa L1 de beraber açılır
                                                if (BD1P || (!string.IsNullOrEmpty(BD1PSD) && !string.IsNullOrEmpty(BD1PED)))
                                                {
                                                    if (string.IsNullOrEmpty(BD1PSD) || string.IsNullOrEmpty(BD1PED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=BD1P lisansı için BD1PSD/BD1PED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.TahvilLPStart = BD1PSD.StrinToDateTime();
                                                    lisansd.TahvilLPEnd = BD1PED.StrinToDateTime();

                                                    // L1 tarihlerini de eşitle
                                                    lisansd.TahvilL1Start = BD1PSD.StrinToDateTime();
                                                    lisansd.TahvilL1End = BD1PED.StrinToDateTime();

                                                    if (lisansd.TahvilLPStart.HasValue && BuAyGelecekAyKontrolu(lisansd.TahvilLPStart.Value))
                                                    {
                                                        lisansd.TahvilLP = true;
                                                        lisansd.TahvilL1 = true;
                                                    }
                                                }

                                                // TAHVIL — BD2 (Düzey 2) Açılırsa LP ve L1 de beraber açılır (zincir)
                                                if (BD2 || (!string.IsNullOrEmpty(BD2SD) && !string.IsNullOrEmpty(BD2ED)))
                                                {
                                                    if (string.IsNullOrEmpty(BD2SD) || string.IsNullOrEmpty(BD2ED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=BD2 lisansı için BD2SD/BD2ED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.TahvilL2Start = BD2SD.StrinToDateTime();
                                                    lisansd.TahvilL2End = BD2ED.StrinToDateTime();

                                                    lisansd.TahvilLPStart = BD2SD.StrinToDateTime();
                                                    lisansd.TahvilLPEnd = BD2ED.StrinToDateTime();
                                                    lisansd.TahvilL1Start = BD2SD.StrinToDateTime();
                                                    lisansd.TahvilL1End = BD2ED.StrinToDateTime();

                                                    if (lisansd.TahvilL2Start.HasValue && BuAyGelecekAyKontrolu(lisansd.TahvilL2Start.Value))
                                                    {
                                                        lisansd.TahvilL2 = true;
                                                        lisansd.TahvilLP = true;
                                                        lisansd.TahvilL1 = true;
                                                    }
                                                }


                                                if (VIT || (!string.IsNullOrEmpty(VITSD) && !string.IsNullOrEmpty(VITED)))
                                                {
                                                    if (string.IsNullOrEmpty(VITSD) || string.IsNullOrEmpty(VITED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=VIT lisansı için VITSD/VITED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.ViopGSStart = VITSD.StrinToDateTime();
                                                    lisansd.ViopGSEnd = VITED.StrinToDateTime();
                                                    if (lisansd.ViopGSStart.HasValue && BuAyGelecekAyKontrolu(lisansd.ViopGSStart.Value)) lisansd.ViopGS = true;
                                                }


                                                if (KRMD1 || (!string.IsNullOrEmpty(KRMD1SD) && !string.IsNullOrEmpty(KRMD1ED)))
                                                {
                                                    if (string.IsNullOrEmpty(KRMD1SD) || string.IsNullOrEmpty(KRMD1ED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=KRMD1 lisansı için KRMD1SD/KRMD1ED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.KRMD1Start = KRMD1SD.StrinToDateTime();
                                                    lisansd.KRMD1End = KRMD1ED.StrinToDateTime();
                                                    if (lisansd.KRMD1Start.HasValue && BuAyGelecekAyKontrolu(lisansd.KRMD1Start.Value)) lisansd.COMEX = true;
                                                }


                                                if (MKK || (!string.IsNullOrEmpty(MKKSD) && !string.IsNullOrEmpty(MKKED)))
                                                {
                                                    if (string.IsNullOrEmpty(MKKSD) || string.IsNullOrEmpty(MKKED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=MKK lisansı için MKKSD/MKKED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.MKKStart = MKKSD.StrinToDateTime();
                                                    lisansd.MKKEnd = MKKED.StrinToDateTime();
                                                    if (lisansd.MKKStart.HasValue && BuAyGelecekAyKontrolu(lisansd.MKKStart.Value)) lisansd.MKK = true;
                                                }

                                                if (GKKUL || (!string.IsNullOrEmpty(GKKULSD) && !string.IsNullOrEmpty(GKKULED)))
                                                {
                                                    if (string.IsNullOrEmpty(GKKULSD) || string.IsNullOrEmpty(GKKULED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=GKKUL lisansı için GKKULSD/GKKULED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.GKKULStart = GKKULSD.StrinToDateTime();
                                                    lisansd.GKKULEnd = GKKULED.StrinToDateTime();
                                                    if (lisansd.GKKULStart.HasValue && BuAyGelecekAyKontrolu(lisansd.GKKULStart.Value)) lisansd.GKKUL = true;
                                                }

                                                if (CME || (!string.IsNullOrEmpty(CMESD) && !string.IsNullOrEmpty(CMEED)))
                                                {
                                                    if (string.IsNullOrEmpty(CMESD) || string.IsNullOrEmpty(CMEED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=ALG lisansı için ALGSD/ALGED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }

                                                    lisansd.CMEStart = CMESD.StrinToDateTime();
                                                    lisansd.CMEEnd = CMEED.StrinToDateTime();

                                                    if (lisansd.CMEStart.HasValue && BuAyGelecekAyKontrolu(lisansd.CMEStart.Value))
                                                        lisansd.CME = true;
                                                }

                                                if (ROBOT) lisansd.ROBOT = true;

                                                if (!PRO && !MOBIL && string.IsNullOrEmpty(PROSD) && string.IsNullOrEmpty(MOBILSD))
                                                {
                                                    lisansd.CepYetki = true;
                                                }

                                                bool mobilGonderildimi = MOBIL || (!string.IsNullOrEmpty(MOBILSD) && !string.IsNullOrEmpty(MOBILED));
                                                if (!mobilGonderildimi)
                                                {
                                                    lisansd.CepYetki = true;
                                                    lisansd.CepYetkiStart = DateTime.Now.Date;
                                                    lisansd.CepYetkiEnd = newcustomer.ExpiryDate;
                                                  //  acilanlar.Append("MOBIL;");
                                                    lisansd.YayinDurumu = true;
                                                }

                                          


                                                var monthEnd = DateTime.Now.Date._LastDayOfMonth();


                                                // YENI servisinde gönderilen tarihler referans değil, ay sonu esas alınır
                                                if (lisansd.ProYetki && lisansd.ProYetkiEnd.HasValue) lisansd.ProYetkiEnd = monthEnd;
                                                if (lisansd.CepYetki && lisansd.CepYetkiEnd.HasValue) lisansd.CepYetkiEnd = monthEnd;
                                                if (lisansd.PayGS && lisansd.PayGSEnd.HasValue) lisansd.PayGSEnd = monthEnd;
                                                if (lisansd.PITE && lisansd.PayPiteEnd.HasValue) lisansd.PayPiteEnd = monthEnd;
                                                if (lisansd.PayX && lisansd.PayXEnd.HasValue) lisansd.PayXEnd = monthEnd;
                                                if (lisansd.PayL1 && lisansd.PayL1End.HasValue) lisansd.PayL1End = monthEnd;
                                                if (lisansd.PayLP && lisansd.PayLPEnd.HasValue) lisansd.PayLPEnd = monthEnd;
                                                if (lisansd.PayL2 && lisansd.PayL2End.HasValue) lisansd.PayL2End = monthEnd;
                                                if (lisansd.Pd2P && lisansd.Pd2PEnd.HasValue) lisansd.Pd2PEnd = monthEnd;
                                                if (lisansd.ViopL1 && lisansd.ViopL1End.HasValue) lisansd.ViopL1End = monthEnd;
                                                if (lisansd.ViopLP && lisansd.ViopLPEnd.HasValue) lisansd.ViopLPEnd = monthEnd;
                                                if (lisansd.ViopL2 && lisansd.ViopL2End.HasValue) lisansd.ViopL2End = monthEnd;
                                                if (lisansd.Vd2P && lisansd.Vd2PEnd.HasValue) lisansd.Vd2PEnd = monthEnd;
                                                if (lisansd.ViopGS && lisansd.ViopGSEnd.HasValue) lisansd.ViopGSEnd = monthEnd;
                                                if (lisansd.TahvilL1 && lisansd.TahvilL1End.HasValue) lisansd.TahvilL1End = monthEnd;
                                                if (lisansd.TahvilLP && lisansd.TahvilLPEnd.HasValue) lisansd.TahvilLPEnd = monthEnd;
                                                if (lisansd.TahvilL2 && lisansd.TahvilL2End.HasValue) lisansd.TahvilL2End = monthEnd;
                                                if (lisansd.COMEX && lisansd.KRMD1End.HasValue) lisansd.KRMD1End = monthEnd;
                                                if (lisansd.MKK && lisansd.MKKEnd.HasValue) lisansd.MKKEnd = monthEnd;
                                                if (lisansd.GKKUL && lisansd.GKKULEnd.HasValue) lisansd.GKKULEnd = monthEnd;
                                                if (lisansd.CME && lisansd.CMEEnd.HasValue) lisansd.CMEEnd = monthEnd;
                                               

                                                
                                                OtoLisansAc(lisansd, monthEnd, now, requestId);

                                                if (lisansd.ProYetki) acilanlar.Append("PRO;");
                                                if (lisansd.CepYetki) acilanlar.Append("MOBIL;");
                                                if (lisansd.PayL1) acilanlar.Append("PD1;");
                                                if (lisansd.PayLP) acilanlar.Append("PD1P;");
                                                if (lisansd.PayL2) acilanlar.Append("PD2;");
                                                if (lisansd.Pd2P) acilanlar.Append("PD2P;");
                                                if (lisansd.PayGS) acilanlar.Append("PIT;");
                                                if (lisansd.PayX) acilanlar.Append("END;");
                                                if (lisansd.PITE) acilanlar.Append("PITE;");
                                                if (lisansd.ViopL1) acilanlar.Append("VD1;");
                                                if (lisansd.ViopLP) acilanlar.Append("VD1P;");
                                                if (lisansd.ViopL2) acilanlar.Append("VD2;");
                                                if (lisansd.Vd2P) acilanlar.Append("VD2P;");
                                                if (lisansd.ViopGS) acilanlar.Append("VIT;");
                                                if (lisansd.TahvilL1) acilanlar.Append("BD1;");
                                                if (lisansd.TahvilLP) acilanlar.Append("BD1P;");
                                                if (lisansd.TahvilL2) acilanlar.Append("BD2;");
                                                if (lisansd.COMEX) acilanlar.Append("KRMD1;");
                                                if (lisansd.CME) acilanlar.Append("ALG;");
                                                if (lisansd.MKK) acilanlar.Append("MKK;");
                                                if (lisansd.GKKUL) acilanlar.Append("GKKUL;");
                                                if (lisansd.ROBOT) acilanlar.Append("ROBOT;");

                                                // ED'lere göre ExpiryDate'i ileri taşımak için
                                                var maxEd = MaxDate(
                                                    lisansd.ProYetkiEnd ?? null,
                                                    lisansd.CepYetkiEnd ?? null,
                                                    lisansd.PayL1End ?? null,     // PD1
                                                    lisansd.PayL2End ?? null,
                                                    lisansd.PayLPEnd ?? null,     // PD1P
                                                    lisansd.Pd2PEnd ?? null,      // PD2P
                                                    lisansd.PayGSEnd ?? null,      // PIT
                                                    lisansd.PayPiteEnd ?? null,    // PITE
                                                    lisansd.PayXEnd ?? null,       // END
                                                    lisansd.ViopL1End ?? null,     // VD1
                                                    lisansd.ViopL2End ?? null,     // VD2
                                                    lisansd.Vd2PEnd ?? null,       // VD2P
                                                    lisansd.ViopGSEnd ?? null,     // VIT
                                                    lisansd.TahvilL1End ?? null,   // BD1
                                                    lisansd.TahvilL2End ?? null,   // BD2
                                                    lisansd.TahvilLPEnd ?? null,   // BD1P 
                                                    lisansd.KRMD1End ?? null,      // KRMD1
                                                    lisansd.MKKEnd ?? null,        // MKK
                                                    lisansd.GKKULEnd ?? null,        // GKKUL
                                                    lisansd.CMEEnd ?? null        // ALG (DB: CME)

                                                );

                                                if (maxEd.HasValue)
                                                {
                                                    //var newExpiry = ToEndOfDay(maxEd.Value);
                                                    //if (!newcustomer.ExpiryDate.HasValue || newExpiry > newcustomer.ExpiryDate.Value)
                                                    //    newcustomer.ExpiryDate = newExpiry;
                                                    newcustomer.ExpiryDate = ToEndOfDay(maxEd.Value);
                                                }

                                                #endregion

                                                #region iletisim
                                                iletisim.acikadres = adres;
                                                iletisim.Tel1 = tel;
                                                var il = MyTools.SehirIdBul(sehir);
                                                if (il != null && il.Id > 0)
                                                {
                                                    iletisim.IlId = il.Id;
                                                    iletisim.UlkeId = il.UlkeId;
                                                }
                                                iletisim.email = mail;
                                                #endregion

                                                kurumbilgi.kurumhesapno = userName + "/" + mbb;
                                                //  kurumbilgi.kurumhesapno = tckno + "/" + mbb;
                                                kurumbilgi.KurumSube = sube;
                                                kurumbilgi.Not1 = personel;

                                                crm.KurumsalBilgilers.InsertOnSubmit(kurumbilgi);
                                                crm.SubmitChanges();
                                                newcustomer.KurumsalBilgilerId = kurumbilgi.Id;

                                                crm.LisansDurums.InsertOnSubmit(lisansd);
                                                crm.SubmitChanges();

                                                newcustomer.LisansDurumId = lisansd.LisansDurumId;
                                                userevent.SonLisandurumID = lisansd.LisansDurumId;

                                                crm.Iletisims.InsertOnSubmit(iletisim);
                                                crm.SubmitChanges();
                                                newcustomer.iletisimId = iletisim.IletisimId;

                                                crm.Users.InsertOnSubmit(newcustomer);
                                                crm.SubmitChanges();

                                                userevent.UserId = newcustomer.UserID;

                                                userevent.AcilanLisans = acilanlar.ToString();
                                                crm.UserEvents.InsertOnSubmit(userevent);
                                                crm.SubmitChanges();
                                                SendToSSOAsync(newcustomer.UserID);

                                                ekransonuc = "SERVİS YENİ KULLANICI KAYIT;  UserName  = " + userName + "; RemoteHost = " + userevent.IP;
                                                var r = new { Kod = "000", Aciklama = "OK", Status = "Basarili" };
                                                var json = new JavaScriptSerializer().Serialize(r);
                                                SendResponse(connectionId, json, requestId);
                                                return;
                                            }
                                            else
                                            {
                                                MyTools.logyaz($"[{requestId}] HATA: Kullanıcı zaten kayıtlı");
                                                var err = new { Kod = "104", Aciklama = "KAYITLI KULLANICI", Status = "Hata" };
                                                SendResponse(connectionId, new JavaScriptSerializer().Serialize(err), requestId);
                                                return;
                                            }
                                        }
                                        #endregion
                                        #region UPDATEUSER yeni
                                        else if (komut == "GUNCELLE")
                                        {
                                           
                                            MyTools.logyaz(inputText); 
                                            var sonucYeni = new Sonuc();
                                            if (string.IsNullOrWhiteSpace(userName) )
                                            {
                                                MyTools.logyaz($"[{requestId}] HATA: Geçersiz USERNAME - {userName}");
                                                var err = new { Kod = "102", Aciklama = "HATA?MESAJ=Gecersiz USERNAME", Status = "Yanlis USERNAME" };
                                                var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                SendResponse(connectionId, jsonErr, requestId);
                                                return;
                                            }
                                            var user = crm.Users.FirstOrDefault(u => u.UserName == userName);
                                            if (user == null)
                                            {
                                                var r = new { Kod = "105", Aciklama = "KULLANICI BULUNAMADI", Status = "Hata" };
                                                var jsonErr = new JavaScriptSerializer().Serialize(r);
                                                SendResponse(connectionId, jsonErr, requestId);
                                                return;
                                            }
                                            else
                                            {
                                                userevent.EventTypeId = 6;
                                                user.UserName = userName;
                                                if (!string.IsNullOrEmpty(musteriNo))
                                                    user.PmtsNo = musteriNo;
                                      
                                                var acilanlar = new StringBuilder();

                                                // Temel kullanıcı alanları (gelenler boş değilse güncelle)
                                                if (!string.IsNullOrEmpty(ad)) user.Name = ad;
                                                if (!string.IsNullOrEmpty(soyad)) user.Surname = soyad;
                                                if (!string.IsNullOrEmpty(aciklama)) user.Aciklama = aciklama;
                                                if (!string.IsNullOrEmpty(ExpriyDate))
                                                    user.ExpiryDate = ExpriyDate.StrinToDateTime();

                                                // İletişim güncelle
                                                if (user.iletisimId == null)
                                                {
                                                    var iletisim = new Iletisim();
                                                    if (!string.IsNullOrEmpty(adres)) iletisim.acikadres = adres;
                                                    if (!string.IsNullOrEmpty(tel)) iletisim.Tel1 = tel;
                                                    if (!string.IsNullOrEmpty(sehir))
                                                    {
                                                        var il = MyTools.SehirIdBul(sehir);
                                                        if (il != null && il.Id > 0)
                                                        {
                                                            iletisim.IlId = il.Id;
                                                            iletisim.UlkeId = il.UlkeId;
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(mail)) iletisim.email = mail;

                                                    crm.Iletisims.InsertOnSubmit(iletisim);
                                                    crm.SubmitChanges();
                                                    user.iletisimId = iletisim.IletisimId;
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(adres)) user.Iletisim.acikadres = adres;
                                                    if (!string.IsNullOrEmpty(tel)) user.Iletisim.Tel1 = tel;
                                                    if (!string.IsNullOrEmpty(sehir))
                                                    {
                                                        var il = MyTools.SehirIdBul(sehir);
                                                        if (il != null && il.Id > 0)
                                                        {
                                                            user.Iletisim.IlId = il.Id;
                                                            user.Iletisim.UlkeId = il.UlkeId;
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(mail)) user.Iletisim.email = mail;
                                                }

                                                // Kurumsal bilgi güncelle
                                                if (user.KurumsalBilgilerId == null)
                                                {
                                                    var kurumbilgi = new KurumsalBilgiler();
                                                    if (!string.IsNullOrEmpty(mbb)) kurumbilgi.kurumhesapno = userName + "/" + mbb;
                                                    //if (!string.IsNullOrEmpty(mbb)) kurumbilgi.kurumhesapno = tckno + "/" + mbb;
                                                    if (!string.IsNullOrEmpty(sube)) kurumbilgi.KurumSube = sube;
                                                    if (!string.IsNullOrEmpty(personel)) kurumbilgi.Not1 = personel;
                                                    crm.KurumsalBilgilers.InsertOnSubmit(kurumbilgi);
                                                    crm.SubmitChanges();
                                                    user.KurumsalBilgilerId = kurumbilgi.Id;
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(mbb)) user.KurumsalBilgiler.kurumhesapno = userName + "/" + mbb;
                                                    // if (!string.IsNullOrEmpty(mbb)) user.KurumsalBilgiler.kurumhesapno = tckno + "/" + mbb;
                                                    if (!string.IsNullOrEmpty(sube)) user.KurumsalBilgiler.KurumSube = sube;
                                                    if (!string.IsNullOrEmpty(personel)) user.KurumsalBilgiler.Not1 = personel;
                                                }


                                                // Mevcut aktif lisansları baz alıp yeni LisansDurum oluştur
                                                var lisansd = new LisansDurum();
                                                lisansd.YayinDurumu = user.LisansDurum.YayinDurumu;
                                                lisansd.Futgck = true; lisansd.WINX = true;

                                                // Aktif olan mevcut lisans ve tarihlerini taşı
                                                if (user.LisansDurum.YayinDurumu)
                                                {
                                                    if (user.LisansDurum.ProYetki) { lisansd.ProYetki = true; lisansd.ProYetkiStart = user.LisansDurum.ProYetkiStart; lisansd.ProYetkiEnd = user.LisansDurum.ProYetkiEnd; }
                                                    if (user.LisansDurum.CepYetki) { lisansd.CepYetki = true; lisansd.CepYetkiStart = user.LisansDurum.CepYetkiStart; lisansd.CepYetkiEnd = user.LisansDurum.CepYetkiEnd; }

                                                     if (user.LisansDurum.PayL1) { lisansd.PayL1 = true; lisansd.PayL1Start = user.LisansDurum.PayL1Start; lisansd.PayL1End = user.LisansDurum.PayL1End; }
                                                    if (user.LisansDurum.PayLP) { lisansd.PayLP = true; lisansd.PayLPStart = user.LisansDurum.PayLPStart; lisansd.PayLPEnd = user.LisansDurum.PayLPEnd; }
                                                    if (user.LisansDurum.PayL2) { lisansd.PayL2 = true; lisansd.PayL2Start = user.LisansDurum.PayL2Start; lisansd.PayL2End = user.LisansDurum.PayL2End; }
                                                    if (user.LisansDurum.Pd2P) { lisansd.Pd2P = true; lisansd.Pd2PStart = user.LisansDurum.Pd2PStart; lisansd.Pd2PEnd = user.LisansDurum.Pd2PEnd; }
                                                    if (user.LisansDurum.PayGS) { lisansd.PayGS = true; lisansd.PayGSStart = user.LisansDurum.PayGSStart; lisansd.PayGSEnd = user.LisansDurum.PayGSEnd; }
                                                    if (user.LisansDurum.PayX) { lisansd.PayX = true; lisansd.PayXStart = user.LisansDurum.PayXStart; lisansd.PayXEnd = user.LisansDurum.PayXEnd; }
                                                    if (user.LisansDurum.PITE) { lisansd.PITE = true; lisansd.PayPiteStart = user.LisansDurum.PayPiteStart; lisansd.PayPiteEnd = user.LisansDurum.PayPiteEnd; }
                                                    if (user.LisansDurum.ViopL1) { lisansd.ViopL1 = true; lisansd.ViopL1Start = user.LisansDurum.ViopL1Start; lisansd.ViopL1End = user.LisansDurum.ViopL1End; }
                                                    if (user.LisansDurum.ViopLP) { lisansd.ViopLP = true; lisansd.ViopLPStart = user.LisansDurum.ViopLPStart; lisansd.ViopLPEnd = user.LisansDurum.ViopLPEnd; }
                                                    if (user.LisansDurum.ViopL2) { lisansd.ViopL2 = true; lisansd.ViopL2Start = user.LisansDurum.ViopL2Start; lisansd.ViopL2End = user.LisansDurum.ViopL2End; }
                                                    if (user.LisansDurum.Vd2P) { lisansd.Vd2P = true; lisansd.Vd2PStart = user.LisansDurum.Vd2PStart; lisansd.Vd2PEnd = user.LisansDurum.Vd2PEnd; }
                                                    if (user.LisansDurum.ViopGS) { lisansd.ViopGS = true; lisansd.ViopGSStart = user.LisansDurum.ViopGSStart; lisansd.ViopGSEnd = user.LisansDurum.ViopGSEnd; }
                                                    if (user.LisansDurum.TahvilL1) { lisansd.TahvilL1 = true; lisansd.TahvilL1Start = user.LisansDurum.TahvilL1Start; lisansd.TahvilL1End = user.LisansDurum.TahvilL1End; }
                                                    if (user.LisansDurum.TahvilLP) { lisansd.TahvilLP = true; lisansd.TahvilLPStart = user.LisansDurum.TahvilLPStart; lisansd.TahvilLPEnd = user.LisansDurum.TahvilLPEnd; }
                                                    if (user.LisansDurum.TahvilL2) { lisansd.TahvilL2 = true; lisansd.TahvilL2Start = user.LisansDurum.TahvilL2Start; lisansd.TahvilL2End = user.LisansDurum.TahvilL2End; }
                                                    if (user.LisansDurum.COMEX) { lisansd.COMEX = true; lisansd.KRMD1Start = user.LisansDurum.KRMD1Start; lisansd.KRMD1End = user.LisansDurum.KRMD1End; }
                                                    if (user.LisansDurum.MKK) { lisansd.MKK = true; lisansd.MKKStart = user.LisansDurum.MKKStart; lisansd.MKKEnd = user.LisansDurum.MKKEnd; }
                                                    if (user.LisansDurum.GKKUL) { lisansd.GKKUL = true; lisansd.GKKULStart = user.LisansDurum.GKKULStart; lisansd.GKKULEnd = user.LisansDurum.GKKULEnd; }
                                                    if (user.LisansDurum.CME) { lisansd.CME = true; lisansd.CMEStart = user.LisansDurum.CMEStart; lisansd.CMEEnd = user.LisansDurum.CMEEnd; }
                                                    if (user.LisansDurum.ROBOT) { lisansd.ROBOT = true; }

                                                    if (user.LisansDurum.SPI) { lisansd.SPI = true; }
                                                }

                                                // PRO
                                                if (PRO || (!string.IsNullOrEmpty(PROSD) && !string.IsNullOrEmpty(PROED)))
                                                {
                                                    if (string.IsNullOrEmpty(PROSD) || string.IsNullOrEmpty(PROED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=PRO lisansı için PROSD/PROED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    if (!string.IsNullOrEmpty(PROSD) && !string.IsNullOrEmpty(PROED))
                                                    {
                                                        lisansd.ProYetkiStart = PROSD.StrinToDateTime();
                                                        lisansd.ProYetkiEnd = PROED.StrinToDateTime();
                                                    }
                                                    if (lisansd.ProYetkiStart.HasValue && BuAyGelecekAyKontrolu(lisansd.ProYetkiStart.Value))
                                                    { lisansd.ProYetki = true; acilanlar.Append("PRO;"); }
                                                }

                                                // MOBIL
                                                if (MOBIL || (!string.IsNullOrEmpty(MOBILSD) && !string.IsNullOrEmpty(MOBILED)))
                                                {
                                                    if (string.IsNullOrEmpty(MOBILSD) || string.IsNullOrEmpty(MOBILED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=MOBIL lisansı için MOBILSD/MOBILED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    if (!string.IsNullOrEmpty(MOBILSD) && !string.IsNullOrEmpty(MOBILED))
                                                    {
                                                        lisansd.CepYetkiStart = MOBILSD.StrinToDateTime();
                                                        lisansd.CepYetkiEnd = MOBILED.StrinToDateTime();
                                                    }
                                                    if (lisansd.CepYetkiStart.HasValue && BuAyGelecekAyKontrolu(lisansd.CepYetkiStart.Value))
                                                    { lisansd.CepYetki = true; acilanlar.Append("MOBIL;"); }
                                                }

                                                // PRO/MOBIL başlangıcı bu ay/gelecek ay ise yayın açık
                                                //if (BuAyGelecekAyKontrolu(PROSD.StrinToDateTime()) || BuAyGelecekAyKontrolu(MOBILSD.StrinToDateTime()))
                                                //    lisansd.YayinDurumu = true;
                                                if ((!string.IsNullOrEmpty(PROSD) && BuAyGelecekAyKontrolu(PROSD.StrinToDateTime())) ||
                                                   (!string.IsNullOrEmpty(MOBILSD) && BuAyGelecekAyKontrolu(MOBILSD.StrinToDateTime())))
                                                {
                                                    lisansd.YayinDurumu = true;
                                                }

                                                #region PD1 KALDIRILDI
                                                //if (PD1 || (!string.IsNullOrEmpty(PD1SD) && !string.IsNullOrEmpty(PD1ED)))
                                                //{
                                                //    if (string.IsNullOrEmpty(PD1SD) || string.IsNullOrEmpty(PD1ED))
                                                //    {
                                                //        var err = new Sonuc
                                                //        {
                                                //            Kod = "103",
                                                //            Aciklama = "HATA?MESAJ=PD1 lisansı için PD1SD/PD1ED zorunlu",
                                                //            Status = "Eksik Bilgi"
                                                //        };
                                                //        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                //        HttpsService.Connections[e.ConnectionId].DataToSendB =
                                                //            Encoding.UTF8.GetBytes(jsonErr._InsertHeaderHTTP());
                                                //        HttpsService.Connections[e.ConnectionId].Connected = false;
                                                //        return;
                                                //    }
                                                //    if (!string.IsNullOrEmpty(PD1SD) && !string.IsNullOrEmpty(PD1ED))
                                                //    {
                                                //        lisansd.PayL1Start = PD1SD.StrinToDateTime();
                                                //        lisansd.PayL1End = PD1ED.StrinToDateTime();
                                                //    }
                                                //    else if (PD1) // SD/ED yoksa ay sonuna kadar
                                                //    {
                                                //        var now = DateTime.Now;
                                                //        lisansd.PayL1Start = new DateTime(now.Year, now.Month, 1);
                                                //        lisansd.PayL1End = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
                                                //    }

                                                //    if (lisansd.PayL1Start.HasValue && BuAyGelecekAyKontrolu(lisansd.PayL1Start.Value))
                                                //    { lisansd.PayL1 = true; acilanlar.Append("PD1;"); }
                                                //}

                                                #endregion
                                                // PD1P (LP )
                                                if (PD1P || (!string.IsNullOrEmpty(PD1PSD) && !string.IsNullOrEmpty(PD1PED)))
                                                {
                                                    if (string.IsNullOrEmpty(PD1PSD) || string.IsNullOrEmpty(PD1PED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=PD1P lisansı için PD1PSD/PD1PED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }

                                                    if (!string.IsNullOrEmpty(PD1PSD) && !string.IsNullOrEmpty(PD1PED))
                                                    {
                                                        lisansd.PayLPStart = PD1PSD.StrinToDateTime();
                                                        lisansd.PayLPEnd = PD1PED.StrinToDateTime();
                                                        lisansd.PayL1Start = lisansd.PayLPStart; lisansd.PayL1End = lisansd.PayLPEnd;
                                                    }
                                                    if (lisansd.PayLPStart.HasValue && BuAyGelecekAyKontrolu(lisansd.PayLPStart.Value))
                                                    {
                                                        lisansd.PayLP = true;
                                                        lisansd.PayL1 = true;
                                                        acilanlar.Append("PD1P;");
                                                    }
                                                }

                                                // PD2 (L2 + LP )
                                                if (PD2 || (!string.IsNullOrEmpty(PD2SD) && !string.IsNullOrEmpty(PD2ED)))
                                                {
                                                    if (string.IsNullOrEmpty(PD2SD) || string.IsNullOrEmpty(PD2ED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=PD2 lisansı için PD2SD/PD2ED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    if (!string.IsNullOrEmpty(PD2SD) && !string.IsNullOrEmpty(PD2ED))
                                                    {
                                                        lisansd.PayL2Start = PD2SD.StrinToDateTime();
                                                        lisansd.PayL2End = PD2ED.StrinToDateTime();
                                                        lisansd.PayLPStart = lisansd.PayL2Start; lisansd.PayLPEnd = lisansd.PayL2End;
                                                        lisansd.PayL1Start = lisansd.PayL2Start; lisansd.PayL1End = lisansd.PayL2End;
                                                    }
                                                    if (lisansd.PayL2Start.HasValue && BuAyGelecekAyKontrolu(lisansd.PayL2Start.Value))
                                                    {
                                                        lisansd.PayL2 = true; lisansd.PayLP = true;
                                                        lisansd.PayL1 = true;
                                                        acilanlar.Append("PD2;");
                                                    }
                                                }

                                                // PD2P (Pd2P + L2 + LP ) – zincir tarihlerini PD2 SD/ED ile eşitle
                                                if (PD2P || (!string.IsNullOrEmpty(PD2PSD) && !string.IsNullOrEmpty(PD2PED)))
                                                {
                                                    if (string.IsNullOrEmpty(PD2PSD) || string.IsNullOrEmpty(PD2PED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=PD2P lisansı için PD2PSD/PD2PED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    //if (PD2P && (string.IsNullOrEmpty(PD2PSD) || string.IsNullOrEmpty(PD2PED)))
                                                    //{
                                                    //    sonuc = "HATA?MESAJ=PD2P için PD2PSD/PD2PED zorunlu";
                                                    //    responsestr = sonuc;
                                                    //    HttpsService.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(responsestr._InsertHeaderHTTP());
                                                    //    HttpsService.Connections[e.ConnectionId].Connected = false;
                                                    //    return;
                                                    //}
                                                    // lisansd.Pd2PStart = !string.IsNullOrEmpty(PD2PSD) ? PD2PSD.StrinToDateTime() : (DateTime?)null;
                                                    lisansd.Pd2PStart = PD2PSD.StrinToDateTime();
                                                    // lisansd.Pd2PEnd = !string.IsNullOrEmpty(PD2PED) ? PD2PED.StrinToDateTime() : (DateTime?)null;
                                                    lisansd.Pd2PEnd = PD2PED.StrinToDateTime();


                                                    // DateTime? chainStart = !string.IsNullOrEmpty(PD2SD) ? PD2SD.StrinToDateTime() : lisansd.Pd2PStart;
                                                    // DateTime? chainEnd = !string.IsNullOrEmpty(PD2ED) ? PD2ED.StrinToDateTime() : lisansd.Pd2PEnd;
                                                    var chainStart = lisansd.Pd2PStart;
                                                    var chainEnd = lisansd.Pd2PEnd;

                                                    lisansd.PayL2Start = chainStart; lisansd.PayL2End = chainEnd;
                                                    lisansd.PayLPStart = chainStart; lisansd.PayLPEnd = chainEnd;
                                                    lisansd.PayL1Start = chainStart; lisansd.PayL1End = chainEnd;


                                                    if (lisansd.Pd2PStart.HasValue && BuAyGelecekAyKontrolu(lisansd.Pd2PStart.Value))
                                                    {
                                                        lisansd.Pd2P = true; lisansd.PayL2 = true; lisansd.PayLP = true;
                                                        lisansd.PayL1 = true;
                                                        acilanlar.Append("PD2P;");
                                                    }
                                                }

                                                // PIT
                                                if (PIT || (!string.IsNullOrEmpty(PITSD) && !string.IsNullOrEmpty(PITED)))
                                                {
                                                    if (string.IsNullOrEmpty(PITSD) || string.IsNullOrEmpty(PITED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=PIT lisansı için PITSD/PITED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    if (!string.IsNullOrEmpty(PITSD) && !string.IsNullOrEmpty(PITED))
                                                    {
                                                        lisansd.PayGSStart = PITSD.StrinToDateTime();
                                                        lisansd.PayGSEnd = PITED.StrinToDateTime();
                                                    }
                                                    if (lisansd.PayGSStart.HasValue && BuAyGelecekAyKontrolu(lisansd.PayGSStart.Value))
                                                    { lisansd.PayGS = true; acilanlar.Append("PIT;"); }
                                                }

                                                // END (PayX)
                                                if (END || (!string.IsNullOrEmpty(ENDSD) && !string.IsNullOrEmpty(ENDED)))
                                                {
                                                    if (string.IsNullOrEmpty(ENDSD) || string.IsNullOrEmpty(ENDED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=END lisansı için ENDSD/ENDED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    if (!string.IsNullOrEmpty(ENDSD) && !string.IsNullOrEmpty(ENDED))
                                                    {
                                                        lisansd.PayXStart = ENDSD.StrinToDateTime();
                                                        lisansd.PayXEnd = ENDED.StrinToDateTime();
                                                    }
                                                    if (lisansd.PayXStart.HasValue && BuAyGelecekAyKontrolu(lisansd.PayXStart.Value))
                                                    { lisansd.PayX = true; acilanlar.Append("END;"); }
                                                }

                                                // PITE
                                                if (PITE || (!string.IsNullOrEmpty(PITESD) && !string.IsNullOrEmpty(PITEED)))
                                                {
                                                    if (string.IsNullOrEmpty(PITESD) || string.IsNullOrEmpty(PITEED))
                                                    {
                                                        var err = new
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=PITE lisansı için PITESD/PITEED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;



                                                    }
                                                    if (!string.IsNullOrEmpty(PITESD) && !string.IsNullOrEmpty(PITEED))
                                                    {
                                                        lisansd.PayPiteStart = PITESD.StrinToDateTime();
                                                        lisansd.PayPiteEnd = PITEED.StrinToDateTime();
                                                    }

                                                    if (lisansd.PayPiteStart.HasValue && BuAyGelecekAyKontrolu(lisansd.PayPiteStart.Value))
                                                    {
                                                        lisansd.PITE = true;
                                                        acilanlar.Append("PITE;");

                                                        // **YENİ: PITE açıkken PIT'i de aynı süreye ayarla veya uzat**
                                                        if (!lisansd.PayGSStart.HasValue || !lisansd.PayGSEnd.HasValue)
                                                        {
                                                            // PIT hiç açılmamışsa, PITE ile aynı süreye aç
                                                            lisansd.PayGSStart = lisansd.PayPiteStart;
                                                            lisansd.PayGSEnd = lisansd.PayPiteEnd;
                                                            lisansd.PayGS = true;

                                                            if (!acilanlar.ToString().Contains("PIT;"))
                                                                acilanlar.Append("PIT;");
                                                        }
                                                        else if (lisansd.PayGSEnd.Value < lisansd.PayPiteEnd.Value)
                                                        {
                                                            // PIT daha erken bitiyorsa, PITE'nin bitiş tarihine uzat
                                                            lisansd.PayGSEnd = lisansd.PayPiteEnd;
                                                            lisansd.PayGS = true;

                                                            if (!acilanlar.ToString().Contains("PIT;"))
                                                                acilanlar.Append("PIT;");
                                                        }
                                                    }
                                                }

                                                #region VD1 KALDIRILDI
                                                // VD1
                                                //if (VD1 || (!string.IsNullOrEmpty(VD1SD) && !string.IsNullOrEmpty(VD1ED)))
                                                //{
                                                //    if (string.IsNullOrEmpty(VD1SD) || string.IsNullOrEmpty(VD1ED))
                                                //    {
                                                //        var err = new Sonuc
                                                //        {
                                                //            Kod = "103",
                                                //            Aciklama = "HATA?MESAJ=VD1 lisansı için VD1SD/VD1ED zorunlu",
                                                //            Status = "Eksik Bilgi"
                                                //        };
                                                //        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                //        HttpsService.Connections[e.ConnectionId].DataToSendB =
                                                //            Encoding.UTF8.GetBytes(jsonErr._InsertHeaderHTTP());
                                                //        HttpsService.Connections[e.ConnectionId].Connected = false;
                                                //        return;
                                                //    }
                                                //    if (!string.IsNullOrEmpty(VD1SD) && !string.IsNullOrEmpty(VD1ED))
                                                //    {
                                                //        lisansd.ViopL1Start = VD1SD.StrinToDateTime();
                                                //        lisansd.ViopL1End = VD1ED.StrinToDateTime();
                                                //    }
                                                //    if (lisansd.ViopL1Start.HasValue && BuAyGelecekAyKontrolu(lisansd.ViopL1Start.Value))
                                                //    { lisansd.ViopL1 = true; acilanlar.Append("VD1;"); }
                                                //}
                                                #endregion
                                                // VD1P (LP + L1)
                                                if (VD1P || (!string.IsNullOrEmpty(VD1PSD) && !string.IsNullOrEmpty(VD1PED)))
                                                {
                                                    if (string.IsNullOrEmpty(VD1PSD) || string.IsNullOrEmpty(VD1PED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=VD1P lisansı için VD1PSD/VD1PED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    if (!string.IsNullOrEmpty(VD1PSD) && !string.IsNullOrEmpty(VD1PED))
                                                    {
                                                        lisansd.ViopLPStart = VD1PSD.StrinToDateTime();
                                                        lisansd.ViopLPEnd = VD1PED.StrinToDateTime();
                                                        lisansd.ViopL1Start = lisansd.ViopLPStart; lisansd.ViopL1End = lisansd.ViopLPEnd;
                                                    }
                                                    if (lisansd.ViopLPStart.HasValue && BuAyGelecekAyKontrolu(lisansd.ViopLPStart.Value))
                                                    {
                                                        lisansd.ViopLP = true;
                                                        lisansd.ViopL1 = true;
                                                        acilanlar.Append("VD1P;");
                                                    }
                                                }

                                                if (VD2 || (!string.IsNullOrEmpty(VD2SD) && !string.IsNullOrEmpty(VD2ED)))
                                                {
                                                    if (string.IsNullOrEmpty(VD2SD) || string.IsNullOrEmpty(VD2ED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=VD2 lisansı için VD2SD/VD2ED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    if (!string.IsNullOrEmpty(VD2SD) && !string.IsNullOrEmpty(VD2ED))
                                                    {
                                                        lisansd.ViopL2Start = VD2SD.StrinToDateTime();
                                                        lisansd.ViopL2End = VD2ED.StrinToDateTime();

                                                        lisansd.ViopLPStart = lisansd.ViopL2Start;
                                                        lisansd.ViopLPEnd = lisansd.ViopL2End;
                                                        lisansd.ViopL1Start = lisansd.ViopL2Start;
                                                        lisansd.ViopL1End = lisansd.ViopL2End;
                                                    }
                                                    else if (VD2)
                                                    {
                                                       
                                                        var s = new DateTime(now.Year, now.Month, 1);
                                                        var ex = s.AddMonths(1).AddDays(-1);
                                                        lisansd.ViopL2Start = s; lisansd.ViopL2End = ex;
                                                        lisansd.ViopLPStart = s; lisansd.ViopLPEnd = ex;
                                                        lisansd.ViopL1Start = s; lisansd.ViopL1End = ex;
                                                    }

                                                    if (lisansd.ViopL2Start.HasValue && BuAyGelecekAyKontrolu(lisansd.ViopL2Start.Value))
                                                    {
                                                        lisansd.ViopL2 = true;
                                                        lisansd.ViopLP = true;
                                                        lisansd.ViopL1 = true;
                                                        acilanlar.Append("VD2;");
                                                    }
                                                }

                                                // VİOP — VD2P (Düzey 2 Plus) -> Açılırsa L2, LP, L1 de beraber açılır
                                                if (VD2P || (!string.IsNullOrEmpty(VD2PSD) && !string.IsNullOrEmpty(VD2PED)))
                                                {
                                                    if (string.IsNullOrEmpty(VD2PSD) || string.IsNullOrEmpty(VD2PED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=VD2P lisansı için VD2PSD/VD2PED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }

                                                    // lisansd.Vd2PStart = !string.IsNullOrEmpty(VD2PSD) ? VD2PSD.StrinToDateTime() : (DateTime?)null;
                                                    lisansd.Vd2PStart = VD2PSD.StrinToDateTime();
                                                    lisansd.Vd2PEnd = VD2PED.StrinToDateTime();
                                                    // lisansd.Vd2PEnd = !string.IsNullOrEmpty(VD2PED) ? VD2PED.StrinToDateTime() : (DateTime?)null;

                                                    // Zincirleme tarihleri: Öncelik SD/ED, boşsa Plus tarihleri
                                                    //DateTime? chainStart = !string.IsNullOrEmpty(VD2SD) ? VD2SD.StrinToDateTime() : lisansd.Vd2PStart;
                                                    //DateTime? chainEnd = !string.IsNullOrEmpty(VD2ED) ? VD2ED.StrinToDateTime() : lisansd.Vd2PEnd;

                                                    var chainStart = lisansd.Vd2PStart;
                                                    var chainEnd = lisansd.Vd2PEnd;

                                                    lisansd.ViopL2Start = chainStart;
                                                    lisansd.ViopL2End = chainEnd;
                                                    lisansd.ViopLPStart = chainStart;
                                                    lisansd.ViopLPEnd = chainEnd;
                                                    lisansd.ViopL1Start = chainStart;
                                                    lisansd.ViopL1End = chainEnd;

                                                    if (lisansd.Vd2PStart.HasValue && BuAyGelecekAyKontrolu(lisansd.Vd2PStart.Value))
                                                    {
                                                        lisansd.Vd2P = true;
                                                        lisansd.ViopL2 = true;
                                                        lisansd.ViopLP = true;
                                                        lisansd.ViopL1 = true;
                                                        acilanlar.Append("VD2P;");
                                                    }
                                                }

                                                #region TAHVIL — BD1 (Düzey 1)
                                                //if (BD1 || (!string.IsNullOrEmpty(BD1SD) && !string.IsNullOrEmpty(BD1ED)))
                                                //{
                                                //    if (string.IsNullOrEmpty(BD1SD) || string.IsNullOrEmpty(BD1ED))
                                                //    {
                                                //        var err = new Sonuc
                                                //        {
                                                //            Kod = "103",
                                                //            Aciklama = "HATA?MESAJ=BD1 lisansı için BD1SD/BD1ED zorunlu",
                                                //            Status = "Eksik Bilgi"
                                                //        };
                                                //        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                //        HttpsService.Connections[e.ConnectionId].DataToSendB =
                                                //            Encoding.UTF8.GetBytes(jsonErr._InsertHeaderHTTP());
                                                //        HttpsService.Connections[e.ConnectionId].Connected = false;
                                                //        return;
                                                //    }
                                                //    if (!string.IsNullOrEmpty(BD1SD) && !string.IsNullOrEmpty(BD1ED))
                                                //    {
                                                //        lisansd.TahvilL1Start = BD1SD.StrinToDateTime();
                                                //        lisansd.TahvilL1End = BD1ED.StrinToDateTime();
                                                //    }
                                                //    else if (BD1)
                                                //    {
                                                //        var now = DateTime.Now;
                                                //        lisansd.TahvilL1Start = new DateTime(now.Year, now.Month, 1);
                                                //        lisansd.TahvilL1End = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
                                                //    }
                                                //    if (lisansd.TahvilL1Start.HasValue && BuAyGelecekAyKontrolu(lisansd.TahvilL1Start.Value))
                                                //    { lisansd.TahvilL1 = true; acilanlar.Append("BD1;"); }
                                                //}
                                                #endregion
                                                // TAHVIL — BD1P (Düzey 1 Plus) 
                                                if (BD1P || (!string.IsNullOrEmpty(BD1PSD) && !string.IsNullOrEmpty(BD1PED)))
                                                {
                                                    if (string.IsNullOrEmpty(BD1PSD) || string.IsNullOrEmpty(BD1PED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=BD1P lisansı için BD1PSD/BD1PED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    lisansd.TahvilLPStart = BD1PSD.StrinToDateTime();
                                                    lisansd.TahvilLPEnd = BD1PED.StrinToDateTime();

                                                    // L1'i de aynı döneme taşı
                                                    lisansd.TahvilL1Start = lisansd.TahvilLPStart;
                                                    lisansd.TahvilL1End = lisansd.TahvilLPEnd;

                                                    if (lisansd.TahvilLPStart.HasValue && BuAyGelecekAyKontrolu(lisansd.TahvilLPStart.Value))
                                                    {
                                                        lisansd.TahvilLP = true;
                                                        lisansd.TahvilL1 = true;
                                                        acilanlar.Append("BD1P;");
                                                    }
                                                }

                                                if (BD2 || (!string.IsNullOrEmpty(BD2SD) && !string.IsNullOrEmpty(BD2ED)))
                                                {
                                                    if (string.IsNullOrEmpty(BD2SD) || string.IsNullOrEmpty(BD2ED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=BD2 lisansı için BD2SD/BD2ED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    if (!string.IsNullOrEmpty(BD2SD) && !string.IsNullOrEmpty(BD2ED))
                                                    {
                                                        lisansd.TahvilL2Start = BD2SD.StrinToDateTime();
                                                        lisansd.TahvilL2End = BD2ED.StrinToDateTime();

                                                        lisansd.TahvilLPStart = lisansd.TahvilL2Start;
                                                        lisansd.TahvilLPEnd = lisansd.TahvilL2End;
                                                        lisansd.TahvilL1Start = lisansd.TahvilL2Start;
                                                        lisansd.TahvilL1End = lisansd.TahvilL2End;
                                                    }
                                                    else if (BD2)
                                                    {
                                                      
                                                        var s = new DateTime(now.Year, now.Month, 1);
                                                        var x = s.AddMonths(1).AddDays(-1);
                                                        lisansd.TahvilL2Start = s; lisansd.TahvilL2End = x;
                                                        lisansd.TahvilLPStart = s; lisansd.TahvilLPEnd = x;
                                                        lisansd.TahvilL1Start = s; lisansd.TahvilL1End = x;
                                                    }

                                                    if (lisansd.TahvilL2Start.HasValue && BuAyGelecekAyKontrolu(lisansd.TahvilL2Start.Value))
                                                    {
                                                        lisansd.TahvilL2 = true; lisansd.TahvilLP = true;
                                                        lisansd.TahvilL1 = true;
                                                        acilanlar.Append("BD2;");
                                                    }
                                                }
                                                // VIT
                                                if (VIT || (!string.IsNullOrEmpty(VITSD) && !string.IsNullOrEmpty(VITED)))
                                                {
                                                    if (string.IsNullOrEmpty(VITSD) || string.IsNullOrEmpty(VITED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=VIT lisansı için VITSD/VITED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    if (!string.IsNullOrEmpty(VITSD) && !string.IsNullOrEmpty(VITED))
                                                    {
                                                        lisansd.ViopGSStart = VITSD.StrinToDateTime();
                                                        lisansd.ViopGSEnd = VITED.StrinToDateTime();
                                                    }
                                                    if (lisansd.ViopGSStart.HasValue && BuAyGelecekAyKontrolu(lisansd.ViopGSStart.Value))
                                                    { lisansd.ViopGS = true; acilanlar.Append("VIT;"); }
                                                }

                                                // KRMD1 (COMEX)
                                                if (KRMD1 || (!string.IsNullOrEmpty(KRMD1SD) && !string.IsNullOrEmpty(KRMD1ED)))
                                                {
                                                    if (string.IsNullOrEmpty(KRMD1SD) || string.IsNullOrEmpty(KRMD1ED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=KRMD1 lisansı için KRMD1SD/KRMD1ED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    if (!string.IsNullOrEmpty(KRMD1SD) && !string.IsNullOrEmpty(KRMD1ED))
                                                    {
                                                        lisansd.KRMD1Start = KRMD1SD.StrinToDateTime();
                                                        lisansd.KRMD1End = KRMD1ED.StrinToDateTime();
                                                    }
                                                    if (lisansd.KRMD1Start.HasValue && BuAyGelecekAyKontrolu(lisansd.KRMD1Start.Value))
                                                    { lisansd.COMEX = true; acilanlar.Append("KRMD1;"); }
                                                }

                                                if (MKK || (!string.IsNullOrEmpty(MKKSD) && !string.IsNullOrEmpty(MKKED)))
                                                {
                                                    if (string.IsNullOrEmpty(MKKSD) || string.IsNullOrEmpty(MKKED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=MKK lisansı için MKKSD/MKKED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    if (!string.IsNullOrEmpty(MKKSD) && !string.IsNullOrEmpty(MKKED))
                                                    {
                                                        lisansd.MKKStart = MKKSD.StrinToDateTime();
                                                        lisansd.MKKEnd = MKKED.StrinToDateTime();
                                                    }
                                                    else if (MKK)
                                                    {
                                                       
                                                        lisansd.MKKStart = new DateTime(now.Year, now.Month, 1);
                                                        lisansd.MKKEnd = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
                                                    }

                                                    if (lisansd.MKKStart.HasValue && BuAyGelecekAyKontrolu(lisansd.MKKStart.Value))
                                                    { lisansd.MKK = true; acilanlar.Append("MKK;"); }
                                                }

                                                if (GKKUL || (!string.IsNullOrEmpty(GKKULSD) && !string.IsNullOrEmpty(GKKULED)))
                                                {
                                                    if (string.IsNullOrEmpty(GKKULSD) || string.IsNullOrEmpty(GKKULED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=GKKUL lisansı için GKKULSD/GKKULED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }
                                                    if (!string.IsNullOrEmpty(GKKULSD) && !string.IsNullOrEmpty(GKKULED))
                                                    {
                                                        lisansd.GKKULStart = GKKULSD.StrinToDateTime();
                                                        lisansd.GKKULEnd = GKKULED.StrinToDateTime();
                                                    }
                                                    else if (GKKUL)
                                                    {
                                                       
                                                        lisansd.GKKULStart = new DateTime(now.Year, now.Month, 1);
                                                        lisansd.GKKULEnd = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
                                                    }

                                                    if (lisansd.GKKULStart.HasValue && BuAyGelecekAyKontrolu(lisansd.GKKULStart.Value))
                                                    { lisansd.GKKUL = true; acilanlar.Append("GKKUL;"); }
                                                }

                                                //  CME 
                                                if (CME || (!string.IsNullOrEmpty(CMESD) && !string.IsNullOrEmpty(CMEED)))
                                                {
                                                    if (string.IsNullOrEmpty(CMESD) || string.IsNullOrEmpty(CMEED))
                                                    {
                                                        var err = new Sonuc
                                                        {
                                                            Kod = "103",
                                                            Aciklama = "HATA?MESAJ=ALG lisansı için ALGSD/ALGED zorunlu",
                                                            Status = "Eksik Bilgi"
                                                        };
                                                        var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                        SendResponse(connectionId, jsonErr, requestId);
                                                        return;
                                                    }

                                                    if (!string.IsNullOrEmpty(CMESD) && !string.IsNullOrEmpty(CMEED))
                                                    {
                                                        lisansd.CMEStart = CMESD.StrinToDateTime();
                                                        lisansd.CMEEnd = CMEED.StrinToDateTime();
                                                    }

                                                    if (lisansd.CMEStart.HasValue && BuAyGelecekAyKontrolu(lisansd.CMEStart.Value))
                                                    { lisansd.CME = true; acilanlar.Append("ALG;"); }
                                                }

                                                if (ROBOT)
                                                {
                                                    lisansd.ROBOT = true;
                                                }


                                                // COMEX varsa ve üst kademeler yoksa L1'leri düşür
                                                if (lisansd.COMEX)
                                                {
                                                    // if (!lisansd.Pd2P && !lisansd.PayL2 && !lisansd.PayLP && lisansd.PayL1)
                                                    // { lisansd.PayL1 = false; lisansd.PayL1Start = null; lisansd.PayL1End = null; }
                                                    // if (!lisansd.ViopLP && !lisansd.ViopL2 && !lisansd.Vd2P && lisansd.ViopL1)
                                                    // { lisansd.ViopL1 = false; lisansd.ViopL1Start = null; lisansd.ViopL1End = null; }
                                                }

                                                // PITE sonrası PayGS kapama kuralı
                                                //if (lisansd.PITE && lisansd.PayPiteStart.HasValue && lisansd.PayPiteStart.Value.Date > new DateTime(2020, 12, 1))
                                                //{
                                                //    lisansd.PayGS = false; lisansd.PayGSStart = null; lisansd.PayGSEnd = null;
                                                //}


                                                if (!lisansd.ProYetki && !lisansd.CepYetki)
                                                {
                                                    var proStart = lisansd.ProYetkiStart;
                                                    var cepStart = lisansd.CepYetkiStart;
                                                    if ((cepStart != null && cepStart.Value.Date >= DateTime.Now.Date) ||
                                                        (proStart != null && proStart.Value.Date >= DateTime.Now.Date))
                                                    {
                                                        lisansd.YayinDurumu = false;
                                                    }
                                                }

                                                //  ExpiryDate koruması 
                                                DateTime? paramExpiry = null;
                                                if (!string.IsNullOrEmpty(ExpriyDate))
                                                {
                                                    var parsed = ExpriyDate.StrinToDateTime();
                                                    if (parsed != DateTime.MinValue) paramExpiry = parsed;
                                                }

                                                var maxEd = MaxDate(
                                                    lisansd.ProYetkiEnd,
                                                    lisansd.CepYetkiEnd,
                                                     lisansd.PayL2End,
                                                    lisansd.PayLPEnd,     // PD1P
                                                    lisansd.Pd2PEnd,      // PD2P
                                                    lisansd.PayGSEnd,
                                                    lisansd.PayPiteEnd,
                                                    lisansd.PayXEnd,
                                                    lisansd.ViopLPEnd,     // VD1P
                                                    lisansd.ViopL2End,
                                                    lisansd.Vd2PEnd,
                                                    lisansd.ViopGSEnd,
                                                    lisansd.TahvilL2End,
                                                    lisansd.TahvilLPEnd,
                                                    lisansd.KRMD1End,
                                                    lisansd.MKKEnd,
                                                    lisansd.GKKULEnd,
                                                    lisansd.CMEEnd,
                                                    paramExpiry
                                                );

                                                if (maxEd.HasValue)
                                                {
                                                    var newExpiry = ToEndOfDay(maxEd.Value);
                                                    if (!user.ExpiryDate.HasValue || newExpiry > user.ExpiryDate.Value)
                                                        user.ExpiryDate = newExpiry;
                                                }


                                                crm.LisansDurums.InsertOnSubmit(lisansd);
                                                crm.SubmitChanges();

                                                user.LisansDurum = lisansd;
                                                userevent.UserId = user.UserID;
                                                userevent.SonLisandurumID = lisansd.LisansDurumId;
                                                userevent.AcilanLisans = acilanlar.ToString();
                                                userevent.KapatilanLisans = "";

                                                crm.UserEvents.InsertOnSubmit(userevent);
                                                crm.SubmitChanges();
                                                SendToSSOAsync(user.UserID);

                                         
                                                ekransonuc = "SERVİS KULLANICI GÜNCELLE;  UserName  = " + userName + "; RemoteHost = " + userevent.IP;
                                                var r = new { Kod = "000", Aciklama = "OK", Status = "Basarili" };
                                                var json = new JavaScriptSerializer().Serialize(r);
                                                SendResponse(connectionId, json, requestId);
                                                return;
                                            }
                                        }
                                        #endregion
                                        #region IPTAL
                                        else if (komut == "IPTAL")
                                        {
                                          
                                            MyTools.logyaz(inputText); 
                                            var sonucYeni = new Sonuc();
                                            // if (string.IsNullOrWhiteSpace(tckno) || tckno.Length != 11)
                                          //  if (string.IsNullOrWhiteSpace(userName) || userName.Length != 11)
                                            if (string.IsNullOrWhiteSpace(userName) )
                                            {
                                                sonucYeni.Kod = "102";
                                                sonucYeni.Aciklama = "HATA?MESAJ=Gecersiz USERNAME";
                                                sonucYeni.Status = "Yanlis USERNAME";
                                                var json = new JavaScriptSerializer().Serialize(sonucYeni);
                                                // HttpsService.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(json._InsertHeaderHTTP());
                                                SendResponse(connectionId, json, requestId);
                                                return;
                                            }

                                            if (crm.Users.Where(x => x.UserName == userName).Any())
                                            // else if (crm.Users.Any(x => x.tckno == tckno))
                                            {

                                                //var newcustomer = crm.Users.FirstOrDefault(x => x.UserName == musteriNo);
                                                var newcustomer = crm.Users.FirstOrDefault(x => x.UserName == userName);
                                                // var newcustomer = crm.Users.FirstOrDefault(x => x.tckno == tckno);
                                                userevent.EventTypeId = 7;
                                                newcustomer.ExpiryDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
                                                crm.SubmitChanges();

                                                userevent.UserId = newcustomer.UserID;
                                                crm.UserEvents.InsertOnSubmit(userevent);
                                                crm.SubmitChanges();
                                                SendToSSOAsync(newcustomer.UserID);
                                                // sonuc = "OK";
                                                // ekransonuc = "SERVİS KULLANICI İPTAL;  Müsteri No  = " + userName + "; RemoteHost = " + userevent.IP;
                                                ekransonuc = "SERVİS KULLANICI İPTAL;  UserName  = " + userName + "; RemoteHost = " + userevent.IP;
                                                //ekransonuc = "SERVİS KULLANICI İPTAL;  TCKN  = " + tckno + "; RemoteHost = " + userevent.IP;
                                                var r = new { Kod = "000", Aciklama = "OK", Status = "Basarili" };
                                                var json = new JavaScriptSerializer().Serialize(r);
                                                SendResponse(connectionId, json, requestId);
                                                return;
                                            }
                                            else
                                            {
                                                //sonuc = "KULLANICI BULUNAMADI";
                                                // ekransonuc = "SERVİS KULLANICI GÜNCELLEME;  Müsteri No  = " + musteriNo + "; RemoteHost = " + userevent.IP;
                                                var r = new { Kod = "105", Aciklama = "KULLANICI BULUNAMADI", Status = "Hata" };
                                                var json = new JavaScriptSerializer().Serialize(r);
                                                SendResponse(connectionId, json, requestId);
                                                return;
                                            }
                                        }
                                        #endregion
                                        #region KULBILGI
                                        else if (komut == "KULBILGI")
                                        {
                                           
                                            MyTools.logyaz(inputText); 
                                           
                                            var operationStart = DateTime.Now;

                                            try
                                            {
                                                // TCKN parametresini al
                                                string tcknFromQuery = userName;

                                                for (int i = 0; i < fieldArray.Length; i++)
                                                {
                                                    var sp = fieldArray[i].Split('=');
                                                    if (sp.Length != 2) continue;
                                                    var key = sp[0].Trim().ToUpper();
                                                    var val = sp[1].Trim();
                                                    if (key == "TCKN" || key == "USERNAME")
                                                    {
                                                        tcknFromQuery = val;
                                                    }
                                                }


                                                if (string.IsNullOrWhiteSpace(tcknFromQuery))
                                                {
                                                    MyTools.logyaz($"[{requestId}] HATA: USERNAME boş");
                                                    var errResponse = new
                                                    {
                                                        Kod = "105",
                                                        Aciklama = "KULLANICI BULUNAMADI",
                                                        Status = "Hata"
                                                    };
                                                    var errJson = new JavaScriptSerializer().Serialize(errResponse);
                                                    SendResponse(connectionId, errJson, requestId);
                                                    return;
                                                }

                                                // DB sorgu süre ölçümü
                                                var dbQueryStart = DateTime.Now;
                                                var u = crm.Users.FirstOrDefault(x => x.UserName == tcknFromQuery);
                                                var dbQueryDuration = (DateTime.Now - dbQueryStart).TotalMilliseconds;

                                                if (u == null)
                                                {
                                                    var errResponse = new
                                                    {
                                                        Kod = "105",
                                                        Aciklama = "KULLANICI BULUNAMADI",
                                                        Status = "Hata"
                                                    };
                                                    var errJson = new JavaScriptSerializer().Serialize(errResponse);
                                                    SendResponse(connectionId, errJson, requestId);
                                                    return;
                                                }
                                                
                                                var l = u.LisansDurum;

                                                if (l == null)
                                                {
                                                    l = new LisansDurum();
                                                }

                                                bool kullanicipasif = u.ExpiryDate.HasValue && u.ExpiryDate.Value.Date < DateTime.Now.Date;
                                                if (kullanicipasif)
                                                {
                                                    l = new LisansDurum();
                                                }

                                                
                                                var dataObj = new
                                                {
                                                    // TCKN = u.tckno ?? u.UserName,
                                                    USERNAME = !string.IsNullOrEmpty(u.tckno) ? u.tckno : u.UserName,

                                                    ISIM = $"{(u.Name ?? "")} {(u.Surname ?? "")}".Trim(),
                                                    EXPIREDATE = u.ExpiryDate.HasValue ? u.ExpiryDate.Value.ToString("yyyyMMdd") : null,

                                                    PRO = l.ProYetki ? 1 : 0,
                                                    PROSD = l.ProYetkiStart?.ToString("yyyyMMdd"),
                                                    PROED = l.ProYetkiEnd?.ToString("yyyyMMdd"),

                                                    MOBIL = l.CepYetki ? 1 : 0,
                                                    MOBILSD = l.CepYetkiStart?.ToString("yyyyMMdd"),
                                                    MOBILED = l.CepYetkiEnd?.ToString("yyyyMMdd"),

                                                    KRMD1 = l.COMEX ? 1 : 0,
                                                    KRMD1SD = l.KRMD1Start?.ToString("yyyyMMdd"),
                                                    KRMD1ED = l.KRMD1End?.ToString("yyyyMMdd"),

                                                    // PD1 kaldırıldığı için sadece PD1P
                                                    PD1P = l.PayLP ? 1 : 0,
                                                    PD1PSD = l.PayLPStart?.ToString("yyyyMMdd"),
                                                    PD1PED = l.PayLPEnd?.ToString("yyyyMMdd"),

                                                    PD2 = l.PayL2 ? 1 : 0,
                                                    PD2SD = l.PayL2Start?.ToString("yyyyMMdd"),
                                                    PD2ED = l.PayL2End?.ToString("yyyyMMdd"),

                                                    PD2P = l.Pd2P ? 1 : 0,
                                                    PD2PSD = l.Pd2PStart?.ToString("yyyyMMdd"),
                                                    PD2PED = l.Pd2PEnd?.ToString("yyyyMMdd"),

                                                    PIT = l.PayGS ? 1 : 0,
                                                    PITSD = l.PayGSStart?.ToString("yyyyMMdd"),
                                                    PITED = l.PayGSEnd?.ToString("yyyyMMdd"),

                                                    END = l.PayX ? 1 : 0,
                                                    ENDSD = l.PayXStart?.ToString("yyyyMMdd"),
                                                    ENDED = l.PayXEnd?.ToString("yyyyMMdd"),

                                                    PITE = l.PITE ? 1 : 0,
                                                    PITESD = l.PayPiteStart?.ToString("yyyyMMdd"),
                                                    PITEED = l.PayPiteEnd?.ToString("yyyyMMdd"),

                                                    // VD1 kaldırıldı, yalnızca VD1P
                                                    VD1P = l.ViopLP ? 1 : 0,
                                                    VD1PSD = l.ViopLPStart?.ToString("yyyyMMdd"),
                                                    VD1PED = l.ViopLPEnd?.ToString("yyyyMMdd"),

                                                    VD2 = l.ViopL2 ? 1 : 0,
                                                    VD2SD = l.ViopL2Start?.ToString("yyyyMMdd"),
                                                    VD2ED = l.ViopL2End?.ToString("yyyyMMdd"),

                                                    VD2P = l.Vd2P ? 1 : 0,
                                                    VD2PSD = l.Vd2PStart?.ToString("yyyyMMdd"),
                                                    VD2PED = l.Vd2PEnd?.ToString("yyyyMMdd"),

                                                    VIT = l.ViopGS ? 1 : 0,
                                                    VITSD = l.ViopGSStart?.ToString("yyyyMMdd"),
                                                    VITED = l.ViopGSEnd?.ToString("yyyyMMdd"),

                                                    // BD1 kaldırıldı, yalnızca BD1P
                                                    BD1P = l.TahvilLP ? 1 : 0,
                                                    BD1PSD = l.TahvilLPStart?.ToString("yyyyMMdd"),
                                                    BD1PED = l.TahvilLPEnd?.ToString("yyyyMMdd"),

                                                    BD2 = l.TahvilL2 ? 1 : 0,
                                                    BD2SD = l.TahvilL2Start?.ToString("yyyyMMdd"),
                                                    BD2ED = l.TahvilL2End?.ToString("yyyyMMdd"),

                                                    MKK = l.MKK ? 1 : 0,
                                                    MKKSD = l.MKKStart?.ToString("yyyyMMdd"),
                                                    MKKED = l.MKKEnd?.ToString("yyyyMMdd"),

                                                    GKKUL = l.GKKUL ? 1 : 0,
                                                    GKKULSD = l.GKKULStart?.ToString("yyyyMMdd"),
                                                    GKKULED = l.GKKULEnd?.ToString("yyyyMMdd"),

                                                    // DB alanı CME, response alanı ALG
                                                    ALG = l.CME ? 1 : 0,
                                                    ALGSD = l.CMEStart?.ToString("yyyyMMdd"),
                                                    ALGED = l.CMEEnd?.ToString("yyyyMMdd"),

                                                    // Robot artık sadece checkbox (SD/ED yok)
                                                    ROBOT = l.ROBOT ? 1 : 0
                                                };

                                                // Başarılı JSON response
                                                var responseObj = new
                                                {
                                                    Kod = "000",
                                                    Aciklama = "OK",
                                                    Status = "Basarili",
                                                    Data = dataObj
                                                };

                                                var totalOperationDuration = (DateTime.Now - operationStart).TotalMilliseconds;
                                                MyTools.logyaz($"[{requestId}] KULBILGI işlemi BAŞARILI - Süre: {totalOperationDuration}ms");

                                                var json = new JavaScriptSerializer().Serialize(responseObj);
                                                SendResponse(connectionId, json, requestId);

                                                ekransonuc = $"SERVİS KULLANICI BİLGİ SORGUSU; UserName = {tcknFromQuery}; IP = {remoteHost}";
                                                return;
                                            }
                                            catch (Exception kulbilgiEx)
                                            {
                                                MyTools.logyaz($"[{requestId}] KULBILGI HATASI: {kulbilgiEx.Message}\nStackTrace: {kulbilgiEx.StackTrace}");

                                                var errResponse = new
                                                {
                                                    Kod = "500",
                                                    Aciklama = "Sunucu hatası: " + kulbilgiEx.Message,
                                                    Status = "Hata"
                                                };
                                                var errJson = new JavaScriptSerializer().Serialize(errResponse);
                                                SendResponse(connectionId, errJson, requestId);
                                                return;
                                            }
                                        }
                                        #endregion
                                        #region LISANSEKLE (tek lisans, SD/ED yok: kapalıysa ay sonuna aç; açıksa +1 ay uzat)
                                        else if (komut == "LISANSEKLE")
                                        {
                                            //string inputmsg2 = e.Text;
                                            //MyTools.logyaz(e.Text); 
                                           // MyTools.logyaz($"[{userName}] LISANSEKLE işlemi başladı");
                                            try
                                            {
                                               // var now = DateTime.Now.Date;
                                                //format ör: https://localhost:5555/AKYATIRIM=LISANSEKLE?TCKN=12345678910?PITE=1
                                                string tcknNo = null;


                                                bool fPRO = false, fMOBIL = false, fROBOT = false, fPIT = false, fEND = false, fPITE = false;
                                                bool fPD1P = false, fPD2 = false, fPD2P = false;
                                                bool fVD1P = false, fVD2 = false, fVD2P = false, fVIT = false;
                                                bool fBD1P = false, fBD2 = false, fKRMD1 = false, fMKK = false, fGKKUL = false, fCME = false;
                                                //bool fPD1 = false,bool fVD1 = false, bool fBD1 = false,

                                                for (int i = 0; i < fieldArray.Length; i++)
                                                {
                                                    var sp = fieldArray[i].Split('=');
                                                    if (sp.Length != 2) continue;
                                                    var key = sp[0].Trim().ToUpper();
                                                    var val = sp[1].Trim();

                                                    if (key == "TCKN" || key == "USERNAME") { tcknNo = val; continue; }


                                                    if (val == "1" || val.ToUpper() == "TRUE")
                                                    {
                                                        switch (key)
                                                        {
                                                            case "PRO": fPRO = true; break;
                                                            case "MOBIL": fMOBIL = true; break;
                                                            case "ROBOT": fROBOT = true; break;
                                                            case "PIT": fPIT = true; break;
                                                            case "END": fEND = true; break;
                                                            case "PITE": fPITE = true; break;

                                                            //  case "PD1": fPD1 = true; break;
                                                            case "PD1P": fPD1P = true; break;
                                                            case "PD2": fPD2 = true; break;
                                                            case "PD2P": fPD2P = true; break;

                                                            // case "VD1": fVD1 = true; break;
                                                            case "VD1P": fVD1P = true; break;
                                                            case "VD2": fVD2 = true; break;
                                                            case "VD2P": fVD2P = true; break;
                                                            case "VIT": fVIT = true; break;

                                                            //  case "BD1": fBD1 = true; break;
                                                            case "BD1P": fBD1P = true; break;
                                                            case "BD2": fBD2 = true; break;

                                                            case "KRMD1": fKRMD1 = true; break;
                                                            case "MKK": fMKK = true; break;
                                                            case "GKKUL": fGKKUL = true; break;
                                                            case "ALG": fCME = true; break;
                                                        }
                                                    }
                                                }

                                                var sonucYeni = new Sonuc();
                                              //  if (string.IsNullOrWhiteSpace(tcknNo) || tcknNo.Length != 11)
                                                if (string.IsNullOrWhiteSpace(tcknNo))
                                                {
                                                    sonucYeni.Kod = "102";
                                                    sonucYeni.Aciklama = "HATA?MESAJ=Gecersiz USERNAME";
                                                    sonucYeni.Status = "Yanlis USERNAME";
                                                    var json2 = new JavaScriptSerializer().Serialize(sonucYeni);
                                                    // HttpsService.Connections[e.ConnectionId].DataToSendB = Encoding.UTF8.GetBytes(json._InsertHeaderHTTP());
                                                    SendResponse(connectionId, json2, requestId);
                                                    return;
                                                }

                                                int flagCount =
                                                    (fPRO ? 1 : 0) + (fMOBIL ? 1 : 0) + (fROBOT ? 1 : 0) + (fPIT ? 1 : 0) + (fEND ? 1 : 0) + (fPITE ? 1 : 0) +
                                                     (fPD1P ? 1 : 0) + (fPD2 ? 1 : 0) + (fPD2P ? 1 : 0) +
                                                     (fVD1P ? 1 : 0) + (fVD2 ? 1 : 0) + (fVD2P ? 1 : 0) + (fVIT ? 1 : 0) +
                                                    (fBD1P ? 1 : 0) + (fBD2 ? 1 : 0) + (fKRMD1 ? 1 : 0) + (fMKK ? 1 : 0) + (fGKKUL ? 1 : 0) + (fCME ? 1 : 0);
                                                //(fPD1 ? 1 : 0) + (fVD1 ? 1 : 0) + (fBD1 ? 1 : 0) 
                                                if (flagCount == 0)
                                                {
                                                    var err = new Sonuc
                                                    {
                                                        Kod = "102",
                                                        Aciklama = "HATA?MESAJ=Her seferinde tek bir lisans belirtin (Orn. PITE=1).",
                                                        Status = "Eksik bilgi"
                                                    };
                                                    //sonuc = "HATA?MESAJ=Her seferinde tek bir lisans belirtin (örn. PITE=1).";
                                                    var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                    SendResponse(connectionId, jsonErr, requestId);
                                                    return;
                                                }
                                                if (flagCount > 1)
                                                {
                                                    var err = new Sonuc
                                                    {
                                                        Kod = "102",
                                                        Aciklama = "HATA?MESAJ=Tek lisans modu: ayni istekte birden fazla lisans gonderilemez.",
                                                        Status = "Hatali Gonderim"
                                                    };

                                                    var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                    SendResponse(connectionId, jsonErr, requestId);
                                                    return;
                                                }

                                               // var user = crm.Users.FirstOrDefault(x => x.tckno == tcknNo);
                                                var user = crm.Users.FirstOrDefault(x => x.UserName == tcknNo);
                                                if (user == null)
                                                {
                                                    var err = new Sonuc
                                                    {
                                                        Kod = "102",
                                                        Aciklama = "KULLANICI BULUNAMADI",
                                                        Status = "Hatali Kullanici"
                                                    };

                                                    var jsonErr = new JavaScriptSerializer().Serialize(err);
                                                    SendResponse(connectionId, jsonErr, requestId);
                                                    return;
                                                }

                                                var ld = user.LisansDurum;
                                                if (ld == null)
                                                {
                                                    ld = new LisansDurum();
                                                    ld.Futgck = true; ld.WINX = true;
                                                    crm.LisansDurums.InsertOnSubmit(ld);
                                                    crm.SubmitChanges();
                                                    user.LisansDurum = ld;
                                                    crm.SubmitChanges();
                                                }
                                               
                                                var monthEnd = DateTime.Now.Date._LastDayOfMonth();

                                                string lisansKodu = "";
                                                string islem = ""; // AC | UZAT
                                                string sdOut = "", edOut = "", eskiEdOut = "";

                                                //KAPALI KULLANICI AÇMA (ParseAutoCreate davranışı)
                                                bool kullaniciKapaliAcildi = false;

                                                if (ld.YayinDurumu == false)
                                                {
                                                    kullaniciKapaliAcildi = true;

                                                    //yeni LisansDurum oluşturur, eski lisans state'i sıfırlanır
                                                    var yeniLd = new LisansDurum();
                                                    yeniLd.YayinDurumu = true;
                                                    yeniLd.Futgck = true;   
                                                    yeniLd.WINX = true;

                                                    yeniLd.CepYetki = true;
                                                    yeniLd.CepYetkiStart = now;
                                                    yeniLd.CepYetkiEnd = monthEnd;

                                                    yeniLd.PayX = true;
                                                    yeniLd.PayXStart = now;
                                                    yeniLd.PayXEnd = monthEnd;

                                                    yeniLd.COMEX = true;
                                                    yeniLd.KRMD1Start = now;
                                                    yeniLd.KRMD1End = monthEnd;

                                                    // ExpiryDate
                                                    user.ExpiryDate = monthEnd;

                                                    crm.LisansDurums.InsertOnSubmit(yeniLd);
                                                    crm.SubmitChanges();

                                                    user.LisansDurum = yeniLd;
                                                    user.LisansDurumId = yeniLd.LisansDurumId;
                                                    ld = yeniLd; 

                                                    crm.SubmitChanges();

                                                    MyTools.logyaz($"[{requestId}] LISANSEKLE: Kapali kullanici acildi -> {tcknNo} (Yeni LisansDurumId: {yeniLd.LisansDurumId})");
                                                }

                                                if (fPRO)
                                                {
                                                    lisansKodu = "PRO";
                                                    bool acik = ld.ProYetki && ld.ProYetkiEnd.HasValue && ld.ProYetkiEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.ProYetki = true;
                                                        ld.ProYetkiStart = now;
                                                        ld.ProYetkiEnd = monthEnd;
                                                        islem = "AC"; sdOut = ld.ProYetkiStart.Value.ToString("yyyyMMdd"); edOut = ld.ProYetkiEnd.Value.ToString("yyyyMMdd");
                                                        ld.YayinDurumu = true;
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.ProYetkiEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.ProYetkiEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1); // mevcut ayın sonundan +1 ay
                                                        ld.ProYetkiEnd = eNext;
                                                        islem = "UZAT"; edOut = ld.ProYetkiEnd.Value.ToString("yyyyMMdd");
                                                        ld.YayinDurumu = true;
                                                    }
                                                }
                                                else if (fMOBIL)
                                                {
                                                    lisansKodu = "MOBIL";
                                                    bool acik = ld.CepYetki && ld.CepYetkiEnd.HasValue && ld.CepYetkiEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.CepYetki = true;
                                                        ld.CepYetkiStart = now;
                                                        ld.CepYetkiEnd = monthEnd;
                                                        islem = "AC"; sdOut = ld.CepYetkiStart.Value.ToString("yyyyMMdd"); edOut = ld.CepYetkiEnd.Value.ToString("yyyyMMdd");
                                                        ld.YayinDurumu = true;
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.CepYetkiEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.CepYetkiEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.CepYetkiEnd = eNext;
                                                        islem = "UZAT"; edOut = ld.CepYetkiEnd.Value.ToString("yyyyMMdd");
                                                        ld.YayinDurumu = true;
                                                    }
                                                }

                                                else if (fPIT)
                                                {
                                                    lisansKodu = "PIT";
                                                    bool acik = ld.PayGS && ld.PayGSEnd.HasValue && ld.PayGSEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.PayGS = true; ld.PayGSStart = now; ld.PayGSEnd = monthEnd;
                                                        islem = "AC"; sdOut = ld.PayGSStart.Value.ToString("yyyyMMdd"); edOut = ld.PayGSEnd.Value.ToString("yyyyMMdd");
                                                        if (ld.PITE && ld.PayPiteEnd.HasValue && ld.PayPiteEnd.Value > monthEnd)
                                                        {
                                                            ld.PayGSEnd = ld.PayPiteEnd;
                                                            edOut = ld.PayGSEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.PayGSEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.PayGSEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.PayGSEnd = eNext;
                                                        islem = "UZAT"; edOut = ld.PayGSEnd.Value.ToString("yyyyMMdd");
                                                        if (ld.PITE && ld.PayPiteEnd.HasValue && ld.PayPiteEnd.Value > eNext)
                                                        {
                                                            ld.PayGSEnd = ld.PayPiteEnd;
                                                            edOut = ld.PayGSEnd.Value.ToString("yyyyMMdd");
                                                        }
                                                    }
                                                }
                                                else if (fEND)
                                                {
                                                    lisansKodu = "END";
                                                    bool acik = ld.PayX && ld.PayXEnd.HasValue && ld.PayXEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.PayX = true; ld.PayXStart = now; ld.PayXEnd = monthEnd;
                                                        islem = "AC"; sdOut = ld.PayXStart.Value.ToString("yyyyMMdd"); edOut = ld.PayXEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.PayXEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.PayXEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.PayXEnd = eNext;
                                                        islem = "UZAT"; edOut = ld.PayXEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                }
                                                else if (fPITE)
                                                {
                                                    lisansKodu = "PITE";
                                                    bool acik = ld.PITE && ld.PayPiteEnd.HasValue && ld.PayPiteEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.PITE = true;
                                                        ld.PayPiteStart = now;
                                                        ld.PayPiteEnd = monthEnd;

                                                        
                                                        ld.PayGS = true;
                                                        ld.PayGSStart = now;
                                                        ld.PayGSEnd = monthEnd;

                                                        islem = "AC";
                                                        sdOut = ld.PayPiteStart.Value.ToString("yyyyMMdd");
                                                        edOut = ld.PayPiteEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.PayPiteEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.PayPiteEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.PayPiteEnd = eNext;

                                                        
                                                        if (ld.PayGSEnd.HasValue && ld.PayGSEnd.Value < eNext)
                                                        {
                                                            ld.PayGSEnd = eNext;
                                                            ld.PayGS = true;
                                                        }

                                                        islem = "UZAT";
                                                        edOut = ld.PayPiteEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                }
                                                //else if (fPD1)
                                                //{
                                                //    lisansKodu = "PD1";
                                                //    bool acik = ld.PayL1 && ld.PayL1End.HasValue && ld.PayL1End.Value.Date >= now;
                                                //    if (!acik)
                                                //    {
                                                //        ld.PayL1 = true; ld.PayL1Start = now; ld.PayL1End = monthEnd;
                                                //        islem = "AC"; sdOut = ld.PayL1Start.Value.ToString("yyyyMMdd"); edOut = ld.PayL1End.Value.ToString("yyyyMMdd");
                                                //    }
                                                //    else
                                                //    {
                                                //        eskiEdOut = ld.PayL1End.Value.ToString("yyyyMMdd");
                                                //        var ex = ld.PayL1End.Value;
                                                //        var eNext = new DateTime( ex.Year,  ex.Month, 1).AddMonths(2).AddDays(-1);
                                                //        ld.PayL1End = eNext;
                                                //        islem = "UZAT"; edOut = ld.PayL1End.Value.ToString("yyyyMMdd");
                                                //    }
                                                //}
                                                else if (fPD1P)
                                                {
                                                    lisansKodu = "PD1P";
                                                    bool acik = ld.PayLP && ld.PayLPEnd.HasValue && ld.PayLPEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.PayLP = true; ld.PayLPStart = now; ld.PayLPEnd = monthEnd;

                                                         if (!ld.PayL1) { ld.PayL1 = true; ld.PayL1Start = now; ld.PayL1End = monthEnd; }
                                                        islem = "AC"; sdOut = ld.PayLPStart.Value.ToString("yyyyMMdd"); edOut = ld.PayLPEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.PayLPEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.PayLPEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.PayLPEnd = eNext;

                                                         if (ld.PayL1End.HasValue && ld.PayL1End.Value < ld.PayLPEnd.Value) ld.PayL1End = ld.PayLPEnd;
                                                        islem = "UZAT"; edOut = ld.PayLPEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                }
                                                else if (fPD2)
                                                {
                                                    lisansKodu = "PD2";
                                                    bool acik = ld.PayL2 && ld.PayL2End.HasValue && ld.PayL2End.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.PayL2 = true; ld.PayL2Start = now; ld.PayL2End = monthEnd;

                                                        if (!ld.PayLP) { ld.PayLP = true; ld.PayLPStart = now; ld.PayLPEnd = monthEnd; }
                                                        if (!ld.PayL1) { ld.PayL1 = true; ld.PayL1Start = now; ld.PayL1End = monthEnd; }
                                                        islem = "AC"; sdOut = ld.PayL2Start.Value.ToString("yyyyMMdd"); edOut = ld.PayL2End.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.PayL2End.Value.ToString("yyyyMMdd");
                                                        var ex = ld.PayL2End.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.PayL2End = eNext;

                                                        if (ld.PayLPEnd.HasValue && ld.PayLPEnd.Value < eNext) ld.PayLPEnd = eNext;
                                                         if (ld.PayL1End.HasValue && ld.PayL1End.Value < eNext) ld.PayL1End = eNext;
                                                        islem = "UZAT"; edOut = ld.PayL2End.Value.ToString("yyyyMMdd");
                                                    }
                                                }
                                                else if (fPD2P)
                                                {
                                                    lisansKodu = "PD2P";
                                                    bool acik = ld.Pd2P && ld.Pd2PEnd.HasValue && ld.Pd2PEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.Pd2P = true; ld.Pd2PStart = now; ld.Pd2PEnd = monthEnd;

                                                        if (!ld.PayL2) { ld.PayL2 = true; ld.PayL2Start = now; ld.PayL2End = monthEnd; }
                                                        if (!ld.PayLP) { ld.PayLP = true; ld.PayLPStart = now; ld.PayLPEnd = monthEnd; }
                                                         if (!ld.PayL1){ ld.PayL1=true; ld.PayL1Start=now; ld.PayL1End=monthEnd; }
                                                        islem = "AC"; sdOut = ld.Pd2PStart.Value.ToString("yyyyMMdd"); edOut = ld.Pd2PEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.Pd2PEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.Pd2PEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.Pd2PEnd = eNext;

                                                        if (ld.PayL2End.HasValue && ld.PayL2End.Value < eNext) ld.PayL2End = eNext;
                                                        if (ld.PayLPEnd.HasValue && ld.PayLPEnd.Value < eNext) ld.PayLPEnd = eNext;
                                                        if (ld.PayL1End.HasValue && ld.PayL1End.Value < eNext) ld.PayL1End = eNext;
                                                        islem = "UZAT"; edOut = ld.Pd2PEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                }
                                                //else if (fVD1)
                                                //{
                                                //    lisansKodu = "VD1";
                                                //    bool acik = ld.ViopL1 && ld.ViopL1End.HasValue && ld.ViopL1End.Value.Date >= now;
                                                //    if (!acik)
                                                //    {
                                                //        ld.ViopL1 = true; ld.ViopL1Start = now; ld.ViopL1End = monthEnd;
                                                //        islem = "AC"; sdOut = ld.ViopL1Start.Value.ToString("yyyyMMdd"); edOut = ld.ViopL1End.Value.ToString("yyyyMMdd");
                                                //    }
                                                //    else
                                                //    {
                                                //        eskiEdOut = ld.ViopL1End.Value.ToString("yyyyMMdd");
                                                //        var ex = ld.ViopL1End.Value;
                                                //        var eNext = new DateTime( ex.Year,  ex.Month, 1).AddMonths(2).AddDays(-1);
                                                //        ld.ViopL1End = eNext;
                                                //        islem = "UZAT"; edOut = ld.ViopL1End.Value.ToString("yyyyMMdd");
                                                //    }
                                                //}
                                                else if (fVD1P)
                                                {
                                                    lisansKodu = "VD1P";
                                                    bool acik = ld.ViopLP && ld.ViopLPEnd.HasValue && ld.ViopLPEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.ViopLP = true; ld.ViopLPStart = now; ld.ViopLPEnd = monthEnd;

                                                        if (!ld.ViopL1){ ld.ViopL1=true; ld.ViopL1Start=now; ld.ViopL1End=monthEnd; }
                                                        islem = "AC"; sdOut = ld.ViopLPStart.Value.ToString("yyyyMMdd"); edOut = ld.ViopLPEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.ViopLPEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.ViopLPEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.ViopLPEnd = eNext;
                                                         if (ld.ViopL1End.HasValue && ld.ViopL1End.Value < eNext) ld.ViopL1End = eNext;
                                                        islem = "UZAT"; edOut = ld.ViopLPEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                }
                                                else if (fVD2)
                                                {
                                                    lisansKodu = "VD2";
                                                    bool acik = ld.ViopL2 && ld.ViopL2End.HasValue && ld.ViopL2End.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.ViopL2 = true; ld.ViopL2Start = now; ld.ViopL2End = monthEnd;

                                                        if (!ld.ViopLP) { ld.ViopLP = true; ld.ViopLPStart = now; ld.ViopLPEnd = monthEnd; }
                                                         if (!ld.ViopL1){ ld.ViopL1=true; ld.ViopL1Start=now; ld.ViopL1End=monthEnd; }
                                                        islem = "AC"; sdOut = ld.ViopL2Start.Value.ToString("yyyyMMdd"); edOut = ld.ViopL2End.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.ViopL2End.Value.ToString("yyyyMMdd");
                                                        var ex = ld.ViopL2End.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.ViopL2End = eNext;
                                                        if (ld.ViopLPEnd.HasValue && ld.ViopLPEnd.Value < eNext) ld.ViopLPEnd = eNext;
                                                        if (ld.ViopL1End.HasValue && ld.ViopL1End.Value < eNext) ld.ViopL1End = eNext;
                                                        islem = "UZAT"; edOut = ld.ViopL2End.Value.ToString("yyyyMMdd");
                                                    }
                                                }
                                                else if (fVD2P)
                                                {
                                                    lisansKodu = "VD2P";
                                                    bool acik = ld.Vd2P && ld.Vd2PEnd.HasValue && ld.Vd2PEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.Vd2P = true; ld.Vd2PStart = now; ld.Vd2PEnd = monthEnd;

                                                        if (!ld.ViopL2) { ld.ViopL2 = true; ld.ViopL2Start = now; ld.ViopL2End = monthEnd; }
                                                        if (!ld.ViopLP) { ld.ViopLP = true; ld.ViopLPStart = now; ld.ViopLPEnd = monthEnd; }
                                                        if (!ld.ViopL1){ ld.ViopL1=true; ld.ViopL1Start=now; ld.ViopL1End=monthEnd; }
                                                        islem = "AC"; sdOut = ld.Vd2PStart.Value.ToString("yyyyMMdd"); edOut = ld.Vd2PEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.Vd2PEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.Vd2PEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.Vd2PEnd = eNext;

                                                        if (ld.ViopLPEnd.HasValue && ld.ViopLPEnd.Value < eNext) ld.ViopLPEnd = eNext;
                                                        if (ld.ViopL2End.HasValue && ld.ViopL2End.Value < eNext) ld.ViopL2End = eNext;
                                                        if (ld.ViopL1End.HasValue && ld.ViopL1End.Value < eNext) ld.ViopL1End = eNext;
                                                        islem = "UZAT"; edOut = ld.Vd2PEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                }
                                                else if (fVIT)
                                                {
                                                    lisansKodu = "VIT";
                                                    bool acik = ld.ViopGS && ld.ViopGSEnd.HasValue && ld.ViopGSEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.ViopGS = true; ld.ViopGSStart = now; ld.ViopGSEnd = monthEnd;
                                                        islem = "AC"; sdOut = ld.ViopGSStart.Value.ToString("yyyyMMdd"); edOut = ld.ViopGSEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.ViopGSEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.ViopGSEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.ViopGSEnd = eNext;
                                                        islem = "UZAT"; edOut = ld.ViopGSEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                }
                                                //else if (fBD1)
                                                //{
                                                //    lisansKodu = "BD1";
                                                //    bool acik = ld.TahvilL1 && ld.TahvilL1End.HasValue && ld.TahvilL1End.Value.Date >= now;
                                                //    if (!acik)
                                                //    {
                                                //        ld.TahvilL1 = true; ld.TahvilL1Start = now; ld.TahvilL1End = monthEnd;
                                                //        islem = "AC"; sdOut = ld.TahvilL1Start.Value.ToString("yyyyMMdd"); edOut = ld.TahvilL1End.Value.ToString("yyyyMMdd");
                                                //    }
                                                //    else
                                                //    {
                                                //        eskiEdOut = ld.TahvilL1End.Value.ToString("yyyyMMdd");
                                                //        var ex = ld.TahvilL1End.Value;
                                                //        var eNext = new DateTime( ex.Year,  ex.Month, 1).AddMonths(2).AddDays(-1);
                                                //        ld.TahvilL1End = eNext;
                                                //        islem = "UZAT"; edOut = ld.TahvilL1End.Value.ToString("yyyyMMdd");
                                                //    }
                                                //}
                                                else if (fBD1P)
                                                {
                                                    lisansKodu = "BD1P";
                                                    bool acik = ld.TahvilLP && ld.TahvilLPEnd.HasValue && ld.TahvilLPEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.TahvilLP = true; ld.TahvilLPStart = now; ld.TahvilLPEnd = monthEnd;
                                                         if (!ld.TahvilL1){ ld.TahvilL1=true; ld.TahvilL1Start=now; ld.TahvilL1End=monthEnd; }
                                                        islem = "AC"; sdOut = ld.TahvilLPStart.Value.ToString("yyyyMMdd"); edOut = ld.TahvilLPEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.TahvilLPEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.TahvilLPEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.TahvilLPEnd = eNext;
                                                        if (ld.TahvilL1End.HasValue && ld.TahvilL1End.Value < eNext) ld.TahvilL1End = eNext;
                                                        islem = "UZAT"; edOut = ld.TahvilLPEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                }
                                                else if (fBD2)
                                                {
                                                    lisansKodu = "BD2";
                                                    bool acik = ld.TahvilL2 && ld.TahvilL2End.HasValue && ld.TahvilL2End.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.TahvilL2 = true; ld.TahvilL2Start = now; ld.TahvilL2End = monthEnd;

                                                        if (!ld.TahvilLP) { ld.TahvilLP = true; ld.TahvilLPStart = now; ld.TahvilLPEnd = monthEnd; }
                                                        if (!ld.TahvilL1){ ld.TahvilL1=true; ld.TahvilL1Start=now; ld.TahvilL1End=monthEnd; }
                                                        islem = "AC"; sdOut = ld.TahvilL2Start.Value.ToString("yyyyMMdd"); edOut = ld.TahvilL2End.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.TahvilL2End.Value.ToString("yyyyMMdd");
                                                        var ex = ld.TahvilL2End.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.TahvilL2End = eNext;
                                                        if (ld.TahvilLPEnd.HasValue && ld.TahvilLPEnd.Value < eNext) ld.TahvilLPEnd = eNext;
                                                        if (ld.TahvilL1End.HasValue && ld.TahvilL1End.Value < eNext) ld.TahvilL1End = eNext;
                                                        islem = "UZAT"; edOut = ld.TahvilL2End.Value.ToString("yyyyMMdd");
                                                    }
                                                }
                                                else if (fKRMD1)
                                                {
                                                    lisansKodu = "KRMD1";
                                                    bool acik = ld.COMEX && ld.KRMD1End.HasValue && ld.KRMD1End.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.COMEX = true; ld.KRMD1Start = now; ld.KRMD1End = monthEnd;
                                                        islem = "AC"; sdOut = ld.KRMD1Start.Value.ToString("yyyyMMdd"); edOut = ld.KRMD1End.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.KRMD1End.Value.ToString("yyyyMMdd");
                                                        var ey = ld.KRMD1End.Value;
                                                        var eNext = new DateTime(ey.Year, ey.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.KRMD1End = eNext;
                                                        islem = "UZAT"; edOut = ld.KRMD1End.Value.ToString("yyyyMMdd");
                                                    }
                                                }
                                                else if (fMKK)
                                                {
                                                    lisansKodu = "MKK";
                                                    bool acik = ld.MKK && ld.MKKEnd.HasValue && ld.MKKEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.MKK = true; ld.MKKStart = now; ld.MKKEnd = monthEnd;
                                                        islem = "AC"; sdOut = ld.MKKStart.Value.ToString("yyyyMMdd"); edOut = ld.MKKEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.MKKEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.MKKEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.MKKEnd = eNext;
                                                        islem = "UZAT"; edOut = ld.MKKEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                }

                                                else if (fGKKUL)
                                                {
                                                    lisansKodu = "GKKUL";
                                                    bool acik = ld.GKKUL && ld.GKKULEnd.HasValue && ld.GKKULEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.GKKUL = true; ld.GKKULStart = now; ld.GKKULEnd = monthEnd;
                                                        islem = "AC"; sdOut = ld.GKKULStart.Value.ToString("yyyyMMdd"); edOut = ld.GKKULEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.GKKULEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.GKKULEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.GKKULEnd = eNext;
                                                        islem = "UZAT"; edOut = ld.GKKULEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                }
                                                else if (fCME)
                                                {
                                                    lisansKodu = "ALG";
                                                    bool acik = ld.CME && ld.CMEEnd.HasValue && ld.CMEEnd.Value.Date >= now;
                                                    if (!acik)
                                                    {
                                                        ld.CME = true; ld.CMEStart = now; ld.CMEEnd = monthEnd;
                                                        islem = "AC"; sdOut = ld.CMEStart.Value.ToString("yyyyMMdd"); edOut = ld.CMEEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                    else
                                                    {
                                                        eskiEdOut = ld.CMEEnd.Value.ToString("yyyyMMdd");
                                                        var ex = ld.CMEEnd.Value;
                                                        var eNext = new DateTime(ex.Year, ex.Month, 1).AddMonths(2).AddDays(-1);
                                                        ld.CMEEnd = eNext;
                                                        islem = "UZAT"; edOut = ld.CMEEnd.Value.ToString("yyyyMMdd");
                                                    }
                                                }

                                                else if (fROBOT)
                                                {
                                                    lisansKodu = "ROBOT";
                                                    bool acik = ld.ROBOT;
                                                    if (!acik)
                                                    {
                                                        ld.ROBOT = true;
                                                        islem = "AC";
                                                    }
                                                }


                                                
                                                var maxEd = MaxDate(
                                                    ld.ProYetkiEnd, ld.CepYetkiEnd, ld.PayGSEnd, ld.PayPiteEnd, ld.PayXEnd,
                                                    ld.ViopL2End, ld.Vd2PEnd, ld.ViopGSEnd, ld.TahvilL2End, ld.TahvilLPEnd,
                                                    ld.KRMD1End, ld.MKKEnd, ld.GKKULEnd, ld.CMEEnd, ld.PayLPEnd,ld.PayL2End,ld.Pd2PEnd, ld.ViopLPEnd);

                                                if (maxEd.HasValue)
                                                {
                                                    var newExpiry = ToEndOfDay(maxEd.Value);
                                                    if (!user.ExpiryDate.HasValue || newExpiry > user.ExpiryDate.Value)
                                                        user.ExpiryDate = newExpiry;
                                                }

                                                //MOBIL / KRMD1 / PAYX ExpiryDate ile aç veya eşitle
                                               
                                                if (maxEd.HasValue)
                                                {
                                                    var hedefTarih = maxEd.Value.Date;

                                                    // MOBIL
                                                    if (!ld.CepYetki || !ld.CepYetkiEnd.HasValue || ld.CepYetkiEnd.Value.Date < hedefTarih)
                                                    {
                                                        var eskiMobil = ld.CepYetki ? ld.CepYetkiEnd?.ToString("yyyyMMdd") : "KAPALI";
                                                        ld.CepYetki = true;
                                                        ld.CepYetkiStart = ld.CepYetkiStart ?? now;
                                                        ld.CepYetkiEnd = hedefTarih;
                                                        MyTools.logyaz($"[{requestId}] LISANSEKLE MOBIL guncellendi: {eskiMobil} -> {hedefTarih:yyyyMMdd}");
                                                    }

                                                    // KRMD1
                                                    if (!ld.COMEX || !ld.KRMD1End.HasValue || ld.KRMD1End.Value.Date < hedefTarih)
                                                    {
                                                        var eskiKrmd1 = ld.COMEX ? ld.KRMD1End?.ToString("yyyyMMdd") : "KAPALI";
                                                        ld.COMEX = true;
                                                        ld.KRMD1Start = ld.KRMD1Start ?? now;
                                                        ld.KRMD1End = hedefTarih;
                                                        MyTools.logyaz($"[{requestId}] LISANSEKLE KRMD1 guncellendi: {eskiKrmd1} -> {hedefTarih:yyyyMMdd}");
                                                    }

                                                    // PAYX (END)
                                                    if (!ld.PayX || !ld.PayXEnd.HasValue || ld.PayXEnd.Value.Date < hedefTarih)
                                                    {
                                                        var eskiPayX = ld.PayX ? ld.PayXEnd?.ToString("yyyyMMdd") : "KAPALI";
                                                        ld.PayX = true;
                                                        ld.PayXStart = ld.PayXStart ?? now;
                                                        ld.PayXEnd = hedefTarih;
                                                        MyTools.logyaz($"[{requestId}] LISANSEKLE PAYX guncellendi: {eskiPayX} -> {hedefTarih:yyyyMMdd}");
                                                    }
                                                }
                                                if (ld.YayinDurumu == false)
                                                {
                                                    ld.YayinDurumu = true;
                                                }
                                                crm.SubmitChanges();

                                                userevent.EventTypeId = 6;
                                                userevent.UserId = user.UserID;
                                                userevent.SonLisandurumID = ld.LisansDurumId;
                                                userevent.AcilanLisans = (islem == "AC" ? lisansKodu : "");
                                                if (kullaniciKapaliAcildi)
                                                {
                                                    userevent.AcilanLisans = ($"MOBIL;KRMD1;END;{lisansKodu+";"}");
                                                }
                                                userevent.KapatilanLisans = ""; 
                                                crm.UserEvents.InsertOnSubmit(userevent);

                                                crm.SubmitChanges();
                                                SendToSSOAsync(user.UserID);
                                                // Response
                                                // OK?LISANS=PITE?ISLEM=AC?SD=20250801?ED=20250831  veya
                                                // OK?LISANS=PITE?ISLEM=UZAT?ESKI_ED=20250831?YENI_ED=20250930
                                                // islem, lisansKodu, sdOut, edOut, eskiEdOut değişkenleri zaten set ediliyor

                                                object responseObj;
                                                if (islem == "AC")
                                                {
                                                    responseObj = new
                                                    {
                                                        Kod = "000",
                                                        Aciklama = "OK",
                                                        Status = "Basarili",
                                                        Lisans = lisansKodu,
                                                        Islem = "AC",
                                                        SD = string.IsNullOrEmpty(sdOut) ? null : sdOut,
                                                        ED = string.IsNullOrEmpty(edOut) ? null : edOut
                                                    };
                                                }
                                                else // "UZAT"
                                                {
                                                    responseObj = new
                                                    {
                                                        Kod = "000",
                                                        Aciklama = "OK",
                                                        Status = "Basarili",
                                                        Lisans = lisansKodu,
                                                        Islem = "UZAT",
                                                        EskiED = string.IsNullOrEmpty(eskiEdOut) ? null : eskiEdOut,
                                                        YeniED = string.IsNullOrEmpty(edOut) ? null : edOut
                                                    };
                                                }

                                                var json = new JavaScriptSerializer().Serialize(responseObj);
                                                SendResponse(connectionId, json, requestId);
                                                return;

                                            }
                                            catch (Exception ex)
                                            {
                                                var err = new { Kod = "500", Aciklama = "HATA?MESAJ=" + ex.Message, Status = "Hata" };
                                                var json = new JavaScriptSerializer().Serialize(err);
                                                SendResponse(connectionId, json, requestId);
                                                return;
                                            }
                                        }
                                        #endregion

                                        if (!string.IsNullOrEmpty(ekransonuc))
                                        {
                                            try
                                            {
                                                formListeTransaction(ekransonuc);
                                                CalisanlaraKomutGonder("Servis|" + ekransonuc);
                                            }
                                            catch (Exception formEx)
                                            {
                                                MyTools.logyaz($"[{requestId}] UYARI: Form güncelleme hatası - {formEx.Message}");
                                            }
                                        }
                                        return;
                                    }
                                    #endregion
                                }

                                var errJson2 = new JavaScriptSerializer().Serialize(
                                 new { Kod = "400", Aciklama = "Gecersiz istek", Status = "Hata" });
                                SendResponse(connectionId, errJson2, requestId);
                                return;
                            }
                            #endregion

                            #region POST

                            if (inputmsg.Substring(0, 4) == "POST")
                            {
                                // MyTools.logyaz($"[{requestId}] POST isteği alındı");
                                SendResponse(connectionId, "OK", requestId);
                                return;
                            }

                            #endregion
                        }

                        catch (Exception ex)
                        {
                            MyTools.logyaz($"[{requestId}] İÇ HATA: {ex.Message}\nStackTrace: {ex.StackTrace}");

                            var errorResponse = new { Kod = "500", Aciklama = "Sunucu hatası: " + ex.Message, Status = "Hata" };
                            SendResponse(connectionId, new JavaScriptSerializer().Serialize(errorResponse), requestId);
                        }
                    }
                    catch (Exception threadEx)
                    {
                        MyTools.logyaz($"[{requestId}] THREAD HATASI: {threadEx.Message}\nStackTrace: {threadEx.StackTrace}");

                        try
                        {
                            var errorResponse = new { Kod = "500", Aciklama = "Sunucu hatası", Status = "Hata" };
                            SendResponse(connectionId, new JavaScriptSerializer().Serialize(errorResponse), requestId);
                        }
                        catch (Exception sendEx)
                        {
                            MyTools.logyaz($"[{requestId}] Response gönderilemedi: {sendEx.Message}");
                        }
                    }
                    finally
                    {

                        if (crm != null)
                        {
                            try
                            {
                                crm.Dispose();
                                
                            }
                            catch (Exception disposeEx)
                            {
                                MyTools.logyaz($"[{requestId}] DB dispose hatası: {disposeEx.Message}");
                            }
                        }

                        var threadDuration = (DateTime.Now - threadStartTime).TotalMilliseconds;
                    }
                });

                var totalRequestDuration = (DateTime.Now - startTime).TotalMilliseconds;
            }
            catch (Exception ex)
            {
                MyTools.logyaz($"[{requestId}] DIŞ HATA: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
        #endregion

        private void chkKarmaAcilsin_CheckedChanged(object sender, EventArgs e)
        {
            MyTools.AyarYaz("COMEX_KARMA", "OtomatikAC", chkKarmaAcilsin.Checked._ToString(), AyarPath);
        }
        private void onluPaketKapamaKontrolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OnluPaketLisansKapat(1);
            }
            catch (Exception ex) { formListeTransaction("HATA" + ex.Message); }
        }
        private void dBBaglantıBilgileriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmDbSettings())
            {
                frm.ShowDialog();
            }
        }
        private void timerLog_Tick(object sender, EventArgs e)
        {
            try
            {
                MyTools.writeDataLog();
                MyTools.writeServerEkranlog();
            }
            catch 
            {
               
            }

        }
    }
    #region INFOANALIZ DTO
    class ModelPortfoyDto
    {
        public int Id { get; set; }
        public string Sembol { get; set; }
        public string Tarih { get; set; }
        public string Fiyat { get; set; }
        public string HedefFiyat { get; set; }
    }
    class TeknikAnalizRapor
    {
        public int Id { get; set; }
        public string Tarih { get; set; }
        public string Baslik { get; set; }
        public List<Stock> Stocks { get; set; }
        public string Piyasa { get; set; }
        public string Link { get; set; }
        public string Icerik { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
    }
    class Stock
    {
        public string StockName { get; set; }
    }
    class SentimentAlgo
    {
        public int Id { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public string Link { get; set; }
        public List<SentimentSembol> Symbols { get; set; }

    }
    class SentimentSembol
    {
        public int Id { get; set; }
        public int SentimentAlgoId { get; set; }
        public string Sembol { get; set; }
        public string SonFiyat { get; set; }

    }
    #endregion

    #region StoreUserCass
    class StoreUserCass
    {
        public string Username = "";
    }
    class StoreUserMailCass : StoreUserCass
    {
        public string Status = "";
    }
    class StoreDemoUsers
    {
        public string Username = "";
        public string ExpiryDate = "";
    }
    #endregion

    #region StoreResponseClass
    class StoreResponseClass
    {
        public string USERNAME = "";
        public string PASSWORD = "";
        public string EXPIREDATE = "";
        public string ULKE = "";
        public string ADRES = "";
        public string GSM = "";
        public string EMAIL = "";
        public string SEHIR = "";
        public string ACIKLAMA = "";
        public string AD = "";
        public string SOYAD = "";
        public string TCNO = "";
        public string PRO = "";
        public string CEP = "";

        public string ONOF = "";

        //pay
        public string PD1 = "";
        public string PD1P = "";
        public string PD2 = "";
        public string PD2P = "";
        public string END = "";
        public string PIT = "";
        public string PITE = "";


        public string PD1SD = "";
        public string PD1PSD = "";
        public string PD2SD = "";
        public string PD2PSD = "";
        public string ENDSD = "";
        public string PITSD = "";
        public string PITESD = "";

        public string PD1ED = "";
        public string PD1PED = "";
        public string PD2ED = "";
        public string PD2PED = "";
        public string ENDED = "";
        public string PITED = "";
        public string PITEED = "";


        //viop
        public string VD1 = "";
        public string VD1P = "";
        public string VD2 = "";
        public string VD2P = "";
        public string VIT = "";

        public string VD1SD = "";
        public string VD1PSD = "";
        public string VD2SD = "";
        public string VD2PSD = "";
        public string VITSD = "";

        public string VD1ED = "";
        public string VD1PED = "";
        public string VD2ED = "";
        public string VD2PED = "";
        public string VITED = "";

        public string KRMD1 = "";
        public string KRMD1SD = "";
        public string KRMD1ED = "";

        public string MKK = "";
        public string MKKSD = "";
        public string MKKED = "";

        public string TARAMA = "";
        public string TARAMASD = "";
        public string TARAMAED = "";

        public string GKKUL = "";
        public string GKKULSD = "";
        public string GKKULED = "";

        public string CME = "";
        public string CMESD = "";
        public string CMEED = "";

        //tahvil
        public string BD1 = "";
        public string BD1P = "";
        public string BD2 = "";

        public string ANPRO = "";
        public string SENTIL1 = "";
        public string SENTIL2 = "";

        public string BD1SD = "";
        public string BD1PSD = "";
        public string BD2SD = "";

        public string ANPROSD = "";

        public string SENTIL1SD = "";
        public string SENTIL2SD = "";
        public string PROSD = "";
        public string CEPSD = "";

        public string BD1ED = "";
        public string BD1PED = "";
        public string BD2ED = "";

        public string ANPROED = "";



        public string SENTIL1ED = "";
        public string SENTIL2ED = "";
        public string PROED = "";
        public string CEPED = "";

        public string YDS = "";
    }
    #endregion
}
#endregion