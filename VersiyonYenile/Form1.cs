
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace VersiyonYenile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string yol = Application.StartupPath + "\\DirectFNCRM.exe";
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public static string AyarOku(string Baslik, string Anahtar, int lenth, string path)
        {
            StringBuilder okunan = new StringBuilder(lenth);
            GetPrivateProfileString(Baslik, Anahtar, "", okunan, lenth, path);

            return okunan.ToString().Trim();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            using (WebClient wc = new WebClient())
            {
              
              //var kurumURL = AyarOku("Kurum", "KurumURL", 100, Application.StartupPath + @"\ayarlar.ini");
              
                var kurumURL = AyarOku("Kurum", "KurumURL", 100, Application.StartupPath + @"\ayarlar.ini") + "/DirectFNCRM.exe";
                //  var kurumURL = "http://www.directfn.com.tr/crmDirectfnVersiyon/DirectFNCRM.exe";
                wc.DownloadFileCompleted += new AsyncCompletedEventHandler(completed);
                wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                if (File.Exists(yol))
                {
                    File.Delete(yol);
                }
                wc.DownloadFileAsync(new Uri(kurumURL), Application.StartupPath + "\\DirectFNCRM.exe");
            
            }

        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void completed(object sender, AsyncCompletedEventArgs e)
        {
            Thread.Sleep(3000);
            Process.Start(yol);
            Application.Exit();

        }
        
    }
}
