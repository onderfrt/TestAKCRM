using DirectFNCRM.AdminViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectFNCRM
{

   
    public partial class formSentimentAlgoDetay : Form
    {

        int activeitemid = 0;
        public formSentimentAlgoDetay()
        {
            InitializeComponent();
        }

        void btnCikar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lviewSembol.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Çıkarmak istediğiniz Sembolü listeden seçiniz");
                    return;
                }
                var exItem = lviewSembol.SelectedItems[0];

                if (activeitemid > 0)
                {
                    var crm = new crmDFNDataContext();
                var id = Int32.Parse( exItem.SubItems[0].Text);

                    if (crm.SentimentSembols.Where(x => x.id == id).Any())
                    {
                        var sembol = crm.SentimentSembols.FirstOrDefault(x => x.id == id);

                        crm.SentimentSembols.DeleteOnSubmit(sembol);
                        crm.SubmitChanges();

                        lviewSembol.Items.Remove(exItem);


                    }

                }
                else
                {
                    lviewSembol.Items.Remove(exItem);
                }
              
            }
            catch (Exception)
            {

                throw;
            }
        }
        void btnSembolekle_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtHisse.Text.Trim() == "") { MessageBox.Show("Eklemek için bir sembol seçiniz");return; }
                if (txtSonFiyat.Text.Trim() == "") { MessageBox.Show("Son Fiyat bilgisi giriniz"); return; }

                var foundSembol = false;
                for (int i = 0; i < lviewSembol.Items.Count; i++)                
                    if (lviewSembol.Items[i].SubItems[1].Text == txtHisse.Text.Trim()) foundSembol = true;

                if(foundSembol) { MessageBox.Show("Bu Sembol zaten eklenmiş"); return; }


                if (activeitemid > 0)
                {
                    crmDFNDataContext crm = new crmDFNDataContext();

                    var sembol = new SentimentSembol();
                    sembol.Sembol = txtHisse.Text.Trim();
                    sembol.SonFiyat = txtSonFiyat.Text.Trim();
                    sembol.SentimentAlgoId = activeitemid;
                    crm.SentimentSembols.InsertOnSubmit(sembol);
                    crm.SubmitChanges();



                }

                var litem = new ListViewItem(new string[] {"0",txtHisse.Text.Trim(),txtSonFiyat.Text.Trim() });            

                lviewSembol.Items.Add(litem);
                SembolGirdiTemizle();

                

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }            
        }
        void formSentimentAlgoDetay_Load(object sender, EventArgs e)
        {
            try
            {

                if (this.Tag != null) activeitemid = (int)this.Tag;

                if (activeitemid > 0)
                {
                    FormDoldur();
                }
                else
                {

                }
                

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

      

        void lviewSembol_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (lviewSembol.SelectedItems.Count < 1) return;              
                var item = lviewSembol.SelectedItems[0];
                lblHisseId.Text = item.SubItems[0].Text.Trim();
                var hisse = item.SubItems[1].Text.Trim();
                var sonfiyat = item.SubItems[2].Text.Trim();
                txtHisse.Text = hisse; txtSonFiyat.Text = sonfiyat;


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void FormDoldur()
        {
            try
            {
                var crm = new crmDFNDataContext();

                if (crm.SentimentAlgos.Where(x => x.id == activeitemid).Any())
                {
                    var algo = crm.SentimentAlgos.FirstOrDefault(x => x.id == activeitemid);

                    lblOlusturan.Text = algo.Calisan.Ad + " " + algo.Calisan.Soyad;
                    lblOlusturmaTarihi.Text = algo.OlusturmaTar.Value.ToString();
                    txtBaslik.Text = algo.Baslik;
                    txticerik.Text = algo.Icerik;
                    txtLink.Text = algo.Link;
                    if (algo.SentimentSembols != null)
                    {
                        if (algo.SentimentSembols.Count > 0)
                        {
                            foreach (var sembol in algo.SentimentSembols)
                            {
                                var litem = new ListViewItem(new string[] {sembol.id.ToString(),sembol.Sembol,sembol.SonFiyat });
                                lviewSembol.Items.Add(litem);
                            }
                        }
                    }

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        void SembolGirdiTemizle()
        {
            try
            {
                txtHisse.Text = "";
                txtSonFiyat.Text = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var crm = new crmDFNDataContext();
                if (txtBaslik.Text.Trim() == "")
                {
                    MessageBox.Show("Başlık Yazınız."); return;

                }

                if (activeitemid > 0)
                {

                    if (crm.SentimentAlgos.Where(x => x.id == activeitemid).Any())
                    {

                        var sentiment = crm.SentimentAlgos.FirstOrDefault(x => x.id == activeitemid);

                        sentiment.Baslik = txtBaslik.Text.Trim();
                        sentiment.Icerik = txticerik.Text.Trim();
                        sentiment.Link = txtLink.Text.Trim();
                        sentiment.GuncellemeTar = DateTime.Now;

                        crm.SubmitChanges();
                        
                        formAdminAra.referance.IPport.DataToSend = "UpdateSentimentAlgo|" + sentiment.id.ToString() + (char)3;


                    }
                    else
                    {
                        MessageBox.Show("Bulunamadı"); return;
                    }
                }
                else
                {

                    var sentiment = new SentimentAlgo();
                    sentiment.Baslik = txtBaslik.Text.Trim();
                    sentiment.Icerik = txticerik.Text.Trim();
                    sentiment.Link = txtLink.Text.Trim();
                    sentiment.OlusturmaTar = DateTime.Now;
                    sentiment.GuncellemeTar = DateTime.Now;
                    sentiment.calisanId = MyTools.ActiveCalisan.calisanID;

                    crm.SentimentAlgos.InsertOnSubmit(sentiment);

                    crm.SubmitChanges();

                    activeitemid =sentiment.id;
                    if (lviewSembol.Items.Count > 0  && activeitemid>0)
                    {
                        for (int i = 0; i < lviewSembol.Items.Count; i++)
                        {
                            var sembol = new SentimentSembol();                      
                            sembol.Sembol = lviewSembol.Items[i].SubItems[1].Text.Trim();
                            sembol.SonFiyat = lviewSembol.Items[i].SubItems[2].Text.Trim();
                            sembol.SentimentAlgoId = activeitemid;
                            crm.SentimentSembols.InsertOnSubmit(sembol);
                            crm.SubmitChanges();
                        }
                    }

                    formAdminAra.referance.IPport.DataToSend = "CreateSentimentAlgo|" + sentiment.id.ToString() + (char)3;
                }

               
                formSentimentAlgo.reference.Guncelle();
                this.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
