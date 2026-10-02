using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectFNCRM.AdminViews
{
    public partial class formDuzeltmeler : Form
    {
        public formDuzeltmeler()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource(); 
            btnIptal.Enabled = true;
            button1.Enabled = false;

            Task.Factory.StartNew(() => {

                try
                {
                    crmDFNDataContext crm = new crmDFNDataContext();
                    var bokluklular = crm.Users.Where(x => x.UserName.StartsWith(" ") || x.UserName.EndsWith(" ")).ToList();
                    if (bokluklular.Count == 0) labelYaz("Kayıt  Bulunamadı");

                    for (int i = 0; i < bokluklular.Count; i++)
                    {
                        if (cts.Token.IsCancellationRequested) break;
                        string eskiUsername = bokluklular[i].UserName;
                        bokluklular[i].UserName = bokluklular[i].UserName.Trim();
                        bokluklular[i].PmtsNo = bokluklular[i].PmtsNo?.Trim();      
                        crm.SubmitChanges();
                        Thread.Sleep(50);
                        formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + bokluklular[i].UserID.ToString() + (char)3;
                        // formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + bokluklular[i].UserName.ToString() + (char)3;
                        MyTools.logyaz($"{DateTime.Now} | UserID: {bokluklular[i].UserID} | Eski: '{eskiUsername}' → Yeni: '{bokluklular[i].UserName}'");
                        Thread.Sleep(100);
                        
                        labelYaz(bokluklular.Count.ToString() + " / " + (i + 1).ToString());
                    }
                    labelYaz(bokluklular.Count.ToString() + " - Düzeltildi ");
                }
                catch (Exception ex)
                {
                    MyTools.logyaz("formDuzeltmeler UsernameDüzeltmesi hata" + ex.Message);
                    labelYaz("HATA: " + ex.Message);
                }
                finally
                {
                    
                    this.Invoke((MethodInvoker)(() => {
                        btnIptal.Enabled = false;
                        button1.Enabled = true;
                    }));
                }
            });
        }

        void labelYaz(string text) {

            try
            {
                if (lblSayac.InvokeRequired)
                {
                    lblSayac.Invoke((MethodInvoker)delegate () { labelYaz(text); });
                }
                else
                {
                    lblSayac.Text = text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnOnizle_Click(object sender, EventArgs e)
        {
            labelYaz("Kontrol ediliyor...");
            var count = await Task.Run(() => {
                crmDFNDataContext crm = new crmDFNDataContext();
                return crm.Users.Count(x => x.UserName.StartsWith(" ") || x.UserName.EndsWith(" "));
            });
            labelYaz($"Temizlenecek kayıt sayısı: {count}");
        }
        CancellationTokenSource cts;
        private void btnIptal_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
            labelYaz("İşlem iptal edildi.");
        }
    }
}
