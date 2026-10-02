using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DirectFNCRM.AdminViews
{
    public partial class formMusteriler : Form
    {
        public formMusteriler()
        {
            InitializeComponent();
            gridDurumOzet.Rows.Add(9);
        }

        public bool kcomboildoldurma = false;
        private int activeitem = 0;
        public bool aramamodu = true;
        public bool loaded = false;
        public string kontrolMusteriNo = "";

        private void formMusteriler_Load(object sender, EventArgs e)
        {

            this.dataGridView1.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView1_RowPrePaint);

            if (this.Tag != null)
            {
                kontrolMusteriNo = (string)this.Tag;
            }

            crmDFNDataContext crm = new crmDFNDataContext();
            comboUlke.DataSource = crm.Ulkes;
            comboUlke.DisplayMember = "UlkeAdi";
            comboUlke.ValueMember = "Id";
            comboUlke.SelectedItem = comboUlke.Items[212];

            var sorgu = crm.Calisans.Where(x=>x.CalisanTipId==1).OrderBy(x=>x.Ad).Select(x => new { x.calisanID, adsoyad = x.Ad + " " + x.Soyad });
            comboCalisan.DataSource = sorgu;
            comboCalisan.DisplayMember = "adsoyad";
            comboCalisan.ValueMember = "calisanID";
            comboCalisan.SelectedIndex =- 1;


            kcomboildoldurma = true;
            gridGuncelle();
            btnSave.Select();
            // temizle();
            // companentGizleGoster(false);

            comboOnOff.SelectedIndex = 0;

            loaded = true;
        

           
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var secili = dataGridView1.Rows[e.RowIndex].Cells[5].Value;
            //dataGridView1.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex+1).ToString();
            if (secili != null)
            {
                if ((bool)secili)
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                else
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;

            }
        }

        public void companentGizleGoster(bool status)
        {
            try
            {
                if (aramamodu)
                {
                    txtAdres.Enabled = status;
                    txtBilgiIslem.Enabled = status;
                    txtMail.Enabled = status;
                    txtTckno.Enabled = status;
                    txtTel1.Enabled = status;
                    txtTel2.Enabled = status;
                    txtVerdiDairesi.Enabled = status;
                    txtVergiNo.Enabled = status;
                    txtYetkili.Enabled = status;
                   // comboCalisan.Enabled = status;
                   // comboIL.Enabled = status;
                    //comboILCE.Enabled = status;
                   // comboUlke.Enabled = status;
                    dtpKayitTarihi.Enabled = status;
                    btnSave.Enabled = status;




                }
            }
            catch (Exception ex)
            {
            
                MessageBox.Show(ex.Message);
            }

        }

        private void comboUlke_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                if (comboUlke.SelectedIndex == -1)
                    return;

                if (comboUlke.SelectedItem != comboUlke.Items[212])
                    comboILCE.SelectedIndex = -1;


                var ulkeid = 0;
                crmDFNDataContext crm = new crmDFNDataContext();
                if (comboUlke.SelectedValue is Ulke)
                {
                    Ulke deger = (Ulke)comboUlke.SelectedValue;
                    ulkeid = deger.Id;
                }
                else
                {
                    ulkeid = (int)comboUlke.SelectedValue;
                }

                if (crm.Ils.Where(x => x.UlkeId == ulkeid).Any())
                {
                    comboIL.DataSource = crm.Ils.Where(x => x.UlkeId == ulkeid);
                    comboIL.DisplayMember = "IlAdi";
                    comboIL.ValueMember = "Id";
                }

                if (!kcomboildoldurma)
                {
                    comboIL.SelectedIndex = -1;
                }
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void comboIL_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboIL.SelectedIndex == -1)
                    return;
                var ilid = 0;
                var ulkeid = 0;
                crmDFNDataContext crm = new crmDFNDataContext();
                if (comboIL.SelectedValue is Il)
                {
                    Il deger = (Il)comboIL.SelectedValue;
                    ilid = deger.Id;
                    ulkeid = (int)deger.UlkeId;
                }
                else
                {
                    ilid = (int)comboIL.SelectedValue;
                    var il = crm.Ils.Where(x => x.Id == ilid).FirstOrDefault();
                    ulkeid = il.UlkeId.Value;

                }

                if (ulkeid == 213)
                {

                    if (crm.Ilces.Where(x => x.IlId == ilid).Any())
                    {
                        comboILCE.DataSource = crm.Ilces.Where(x => x.IlId == ilid);
                        comboILCE.DisplayMember = "IlceAdi";
                        comboILCE.ValueMember = "Id";
                    }

                }

                if (!kcomboildoldurma)
                {
                    comboILCE.SelectedIndex = -1;
                }


                comboILCE.SelectedIndex = -1;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            crmDFNDataContext crm = new crmDFNDataContext();
            if (activeitem == 0)
            {
                if (txtAdi.Text == string.Empty)
                { MessageBox.Show("Kurum  Adı Boş Bırakılamaz"); return; }


                Musteriler musteri = new Musteriler();
                musteri.MusteriAdi = txtAdi.Text;          
                musteri.MusteriNo = txtPMTS.Text;
                musteri.VergiNo = txtVergiNo.Text;
                musteri.VergiDairesi = txtVerdiDairesi.Text;
                musteri.BilgiIslem = txtBilgiIslem.Text;
                musteri.Yetkili = txtYetkili.Text;
                musteri.Tckno = txtTckno.Text;
                musteri.KayitTarihi = dtpKayitTarihi.Value;

                if (comboDurum.SelectedIndex == 0)
                    musteri.Status = true;
                else
                    musteri.Status = false;


                if (comboCalisan.SelectedIndex != -1)
                {
                    musteri.TemsilciID =(int) comboCalisan.SelectedValue;

                }


                Iletisim ileti = new Iletisim();

                ileti.acikadres = txtAdres.Text;
                ileti.Tel1 = txtTel1.Text;
                ileti.Tel2 = txtTel2.Text;
                ileti.email = txtMail.Text;
                ileti.UlkeId = (int)comboUlke.SelectedValue;
                if (comboIL.SelectedIndex != -1)
                    ileti.IlId = (int)comboIL.SelectedValue;
                if (comboILCE.SelectedIndex != -1)
                    ileti.IlceId = (int)comboILCE.SelectedValue;


                crm.Iletisims.InsertOnSubmit(ileti);
                crm.SubmitChanges();

                musteri.IletisimId = ileti.IletisimId;
                crm.Musterilers.InsertOnSubmit(musteri);
                crm.SubmitChanges();

                if (musteri.id > 0)
                {
                    MessageBox.Show("Müşteri Kaydedildi");
                }

                gridGuncelle();
            }
            else
            {


                var mus = crm.Musterilers.FirstOrDefault(x => x.id == activeitem);
                if (mus != null)
                {
                    if (MessageBox.Show(mus.MusteriAdi + " Değiştirilecek", "Değiştirme İşlemi", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {

                        if (txtAdi.Text == string.Empty)
                        { MessageBox.Show("Müşteri Adı Boş Bırakılamaz"); return; }


                        mus.MusteriAdi = txtAdi.Text;                     
                        mus.MusteriNo = txtPMTS.Text;
                        mus.VergiNo = txtVergiNo.Text;
                        mus.VergiDairesi = txtVerdiDairesi.Text;
                        mus.BilgiIslem = txtBilgiIslem.Text;
                        mus.Yetkili = txtYetkili.Text;
                        mus.Tckno = txtTckno.Text;
                        mus.KayitTarihi = dtpKayitTarihi.Value;

                        var tmpdurum = mus.Status;

                        if (comboDurum.SelectedIndex == 0)
                            mus.Status = true;
                        else
                            mus.Status = false;


                        if (tmpdurum != mus.Status)
                        {
                            if (mus.Status == false)
                            {
                                var sozlesmeler = crm.Sozlesmelers.Where(x => x.MusteriNo == mus.MusteriNo);

                                if (sozlesmeler != null)
                                {
                                    foreach (var sz in sozlesmeler)
                                    {

                                      //  sz.status = false;

                                        crm.SubmitChanges();


                                    }

                                }

                            }


                        }


                        if (comboCalisan.SelectedIndex != -1)
                        {
                            mus.TemsilciID = (int)comboCalisan.SelectedValue;

                        }

                        if (mus.IletisimId == null)
                        {

                            Iletisim ileti = new Iletisim();

                            ileti.acikadres = txtAdres.Text;
                            ileti.Tel1 = txtTel1.Text;
                            ileti.Tel2 = txtTel2.Text;
                            ileti.email = txtMail.Text;
                            if (comboUlke.SelectedIndex != -1)
                                ileti.UlkeId = (int)comboUlke.SelectedValue;
                            if (comboIL.SelectedIndex != -1)
                                ileti.IlId = (int)comboIL.SelectedValue;
                            if (comboILCE.SelectedIndex != -1)
                                ileti.IlceId = (int)comboILCE.SelectedValue;


                            crm.Iletisims.InsertOnSubmit(ileti);
                            crm.SubmitChanges();

                            mus.IletisimId = ileti.IletisimId;

                        }
                        else
                        {

                            mus.Iletisim.acikadres = txtAdres.Text;
                            mus.Iletisim.Tel1 = txtTel1.Text;
                            mus.Iletisim.Tel2 = txtTel2.Text;
                            mus.Iletisim.email = txtMail.Text;
                            mus.Iletisim.UlkeId = (int)comboUlke.SelectedValue;
                            if (comboIL.SelectedIndex != -1)
                                mus.Iletisim.IlId = (int)comboIL.SelectedValue;
                            if (comboILCE.SelectedIndex != -1)
                                mus.Iletisim.IlceId = (int)comboILCE.SelectedValue;
                        }


                        crm.SubmitChanges();

                        if (mus.id > 0)
                        {
                            MessageBox.Show(mus.MusteriAdi + "   Değiştirildi");
                        }

                    }

                }
                else MessageBox.Show("Müşteri Id si Bulunamadı");

            }

         
        }

        public void gridGuncelle()
        {
            //crmDFNDataContext crm = new crmDFNDataContext();
            //var sonuc = crm.Musterilers.Select(x => new { x.id, x.MusteriNo, x.MusteriAdi, x.Yetkili, x.VergiDairesi, x.VergiNo, x.Tckno, x.KayitTarihi, Temsilci = x.Calisan.Ad + " " + x.Calisan });
            //if (sonuc != null)
            //    dataGridView1.DataSource = sonuc;

            Ara();

        }

        public void toplamshow(string musterino)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                var Cep = crm.Users.Where(x => (x.PmtsNo == musterino && x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true && x.SozlesmeID!=null));
                decimal ceptoplam = 0;
                var cepsayisi  = 0;
                foreach (var c in Cep)
                {
                    if (c.Sozlesmeler.ParaBirimiID == 1)
                    {
                        ceptoplam += (decimal)c.Sozlesmeler.CepFiyat;

                    }
                    else
                    {
                        ceptoplam += (decimal)(c.Sozlesmeler.CepFiyat * c.Sozlesmeler.ParaBirimi.Kur);
                    }
                    cepsayisi++;

                }

                var pro = crm.Users.Where(x => x.PmtsNo == musterino && x.LisansDurum.YayinDurumu == true && x.LisansDurum.ProYetki == true && x.SozlesmeID != null);
                decimal protoplam = 0;
                var prosayisi = 0;
                foreach (var p in pro)
                {

                    if (p.Sozlesmeler.ParaBirimiID == 1)
                    {
                        protoplam += (decimal)p.Sozlesmeler.ProFiyat;

                    }
                    else
                    {
                        protoplam += (decimal)(p.Sozlesmeler.ProFiyat * p.Sozlesmeler.ParaBirimi.Kur);
                    }
          
                    prosayisi++;

                }

                var projesayisi = 0;
                decimal projetoplamtutar = 0;
                if (crm.Projelers.Where(x => x.MusteriNo == musterino).Any())
                {
                    var projeleri = crm.Projelers.Where(x => x.MusteriNo == musterino);

                    foreach (var proje in projeleri)
                    {

                        if (proje.Sozlesmeler.ParaBirimiID == 1)
                        {
                        projetoplamtutar+= (decimal)proje.Fiyat;
                        }
                        else
                        {
                            projetoplamtutar += (decimal)(proje.Fiyat * proje.Sozlesmeler.ParaBirimi.Kur);
                        }


                        projesayisi++;
                    }

                }

          

                var renk = Color.LightSalmon;

                decimal kdvoran = 0.20m;
                gridDurumOzet.Rows[0].SetValues("Ürün", "Sayısı" ,"Toplam", "", "", "");
                gridDurumOzet.Rows[0].DefaultCellStyle.BackColor = renk;
                gridDurumOzet.Rows[1].SetValues("iDeal", prosayisi, protoplam.ToString("0.00"), "", "", "");
                gridDurumOzet.Rows[2].SetValues("CEP", cepsayisi, ceptoplam.ToString("0.00"), "", "", "");
                gridDurumOzet.Rows[3].SetValues("Projeler", projesayisi, projetoplamtutar.ToString("0.00"), "", "", "");
                gridDurumOzet.Rows[4].SetValues("Toplam", "", (ceptoplam+protoplam + projetoplamtutar).ToString("0.00"), "", "", "");

                decimal kdv = (protoplam + ceptoplam + projetoplamtutar) * kdvoran; 
                gridDurumOzet.Rows[5].SetValues("", "", "" + "", "", "", "");
                gridDurumOzet.Rows[6].SetValues("KDV", "", "" + string.Format("{0:C}",kdv), "", "", "");
                gridDurumOzet.Rows[7].SetValues("Genel Toplam", "", "" + string.Format("{0:C}", kdv+ ceptoplam + protoplam + projetoplamtutar), "", "", "");

                gridDurumOzet.Rows[8].Cells[1].Selected = true;




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }



        }

        public void temizle()
        {
            try
            {

                activeitem = 0;
                txtAdi.Text = "";
                txtAdres.Text = "";
                txtYetkili.Text = "";
                txtBilgiIslem.Text = "";
                txtMail.Text = "";
                txtTel1.Text = "";
                txtTel2.Text = "";
                txtPMTS.Text = "";
                txtTckno.Text = "";
                txtVerdiDairesi.Text = "";
                txtVergiNo.Text = "";
                comboCalisan.SelectedIndex = -1;
                comboIL.SelectedIndex = -1;
                comboILCE.SelectedIndex = -1;
                dtpKayitTarihi.Value = DateTime.Now;
                companentGizleGoster(true);
                comboDurum.SelectedIndex = 0;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            temizle();

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {

            //try
            //{
            //    crmDFNDataContext crm = new crmDFNDataContext();
            //    if (dataGridView1.SelectedRows == null)
            //        return;
            //    var row = dataGridView1.CurrentRow;
            //    activeitem = Int32.Parse(row.Cells[0].Value.ToString());
            //    var musteri = crm.Musterilers.FirstOrDefault(x => x.id == activeitem);

            //    if (musteri.VergiDairesi != null) txtVerdiDairesi.Text = musteri.VergiDairesi;
            //    else txtVerdiDairesi.Text = ""; ;
            //    if (musteri.VergiNo != null) txtVergiNo.Text = musteri.VergiNo;
            //    else txtVergiNo.Text = "";
            //    if (musteri.Unvan != null) txtUnvan.Text = musteri.Unvan;
            //    else txtUnvan.Text = "";
            //    if (musteri.MusteriNo != null)
            //        txtPMTS.Text = musteri.MusteriNo;
            //    else txtPMTS.Text = "";
            //    if (musteri.Tckno != null)
            //        txtTckno.Text = musteri.Tckno;
            //    else txtTckno.Text = "";


            //    txtAdi.Text = musteri.MusteriAdi;

            //    if (musteri.IletisimId != null)
            //    {
            //        txtAdres.Text = musteri.Iletisim.acikadres;                  
            //        txtMail.Text = musteri.Iletisim.email;
            //        txtTel1.Text = musteri.Iletisim.Tel1;
            //        txtTel2.Text = musteri.Iletisim.Tel2;
            //        if (musteri.Iletisim.UlkeId != null)
            //            comboUlke.SelectedValue = musteri.Iletisim.UlkeId;
            //        else comboUlke.SelectedIndex = -1;

            //        if (musteri.Iletisim.IlId != null)
            //            comboIL.SelectedValue = musteri.Iletisim.IlId;
            //        else comboIL.SelectedIndex = -1;


            //        if (musteri.Iletisim.IlceId != null)
            //            comboILCE.SelectedValue = musteri.Iletisim.IlceId;
            //        else comboILCE.SelectedIndex = -1;
            //    }
            //    else
            //    {
            //        txtAdres.Text = "";
            //        txtMail.Text = "";
            //        txtTel1.Text = "";
            //        txtTel2.Text = "";
            //        comboUlke.SelectedIndex = -1;
            //        comboIL.SelectedIndex = -1;
            //        comboILCE.SelectedIndex = -1;

            //    }

             
              

            //}
            //catch (Exception ex)
            //{


            //    MessageBox.Show(ex.Message);
            //}


        }



        public delegate void lblsayacguncelle(string text);

        public void lblsayacyaz(string text)
        {
            try
            {
                if (lblsayac.InvokeRequired)
                {
                    lblsayacguncelle lbl = new lblsayacguncelle(lblsayacyaz);

                    this.Invoke(lbl, new object[] { text });

                }
                else
                {
                    lblsayac.Text = text;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
       
        private void btnDosyadanKurumYukle_Click(object sender, EventArgs e)
        {
            var filename = "";
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                filename = op.FileName;
            }

            if (filename == "")
                return;

                var  newthread = new Thread(new ThreadStart(() => {

               crmDFNDataContext crm = new crmDFNDataContext();
             
                   var tum = File.ReadAllLines(op.FileName, Encoding.GetEncoding("iso-8859-9"));

                   for (int i = 1; i < tum.Length; i++)
                   {

                       Thread.Sleep(100);
                       var satir = tum[i].Split(';');

                        if (satir.Length > 6)
                        {
                            Iletisim iletisim = new Iletisim();
                            Sozlesmeler fbil = new Sozlesmeler();
                            Musteriler m = new Musteriler();
                            var pmtsno = satir[0].Trim();
                            var musteriadi = satir[1];
                            var mtip= 1;
                          

                            var gelentip = 8;

                            if (satir[2] != "")
                            {
                                gelentip = Int32.Parse(satir[2]);

                                if (gelentip < 5)
                                {
                                    mtip = 2;
                                }
                            }

                            if (satir[3] != "")
                            {
                                var ilbul = MyTools.SehirIdBul(satir[3].ToLower());

                                iletisim.IlId = ilbul.Id;
                                iletisim.UlkeId = ilbul.UlkeId;
                            }
                            if (satir[4] != "")
                            {
                                m.VergiDairesi = satir[4];
                            }
                            if (satir[5] != "")
                            {
                               m.VergiNo = satir[5];
                            }


                            if (satir[6] != "")
                            {
                                var tstr = satir[6].Substring(0, 10);

                                if (satir[6].Contains("-"))
                                {

                                    m.KayitTarihi = DateTime.ParseExact(tstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);

                                }
                                else
                                {
                                    m.KayitTarihi = DateTime.ParseExact(tstr, "dd.MM.yyyy", System.Globalization.CultureInfo.CurrentCulture);
                                }
                            }




                            if (crm.Musterilers.Where(x => x.MusteriNo == pmtsno).Any())
                            {


                            }
                            else
                            {

                                if (iletisim.IlId >0)
                                {
                                    crm.Iletisims.InsertOnSubmit(iletisim);
                                    crm.SubmitChanges();
                                    m.IletisimId = iletisim.IletisimId;
                                }
                              

                                m.MusteriAdi = musteriadi;
                                m.MusteriNo = pmtsno;
                                m.MusteriTypeId = mtip;


                                


                                crm.Musterilers.InsertOnSubmit(m);
                                crm.SubmitChanges();

                            }

                            lblsayacyaz((i + 1).ToString());


                        }
                   }


               

           }));

            newthread.Start();
           
            
             
        }

       public void GridToForm(int id)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                var musteri = crm.Musterilers.FirstOrDefault(x => x.id == activeitem);

                if (musteri.VergiDairesi != null) txtVerdiDairesi.Text = musteri.VergiDairesi;
                else txtVerdiDairesi.Text = ""; ;
                if (musteri.VergiNo != null) txtVergiNo.Text = musteri.VergiNo;
                else txtVergiNo.Text = "";
                if (musteri.Yetkili != null) txtYetkili.Text = musteri.Yetkili;
                else txtYetkili.Text = "";
                if (musteri.BilgiIslem != null) txtBilgiIslem.Text = musteri.BilgiIslem;
                else txtBilgiIslem.Text = "";
                if (musteri.MusteriNo != null)
                    txtPMTS.Text = musteri.MusteriNo;
                else txtPMTS.Text = "";
                if (musteri.Tckno != null)
                    txtTckno.Text = musteri.Tckno;
                else txtTckno.Text = "";

                if (musteri.KayitTarihi != null) dtpKayitTarihi.Value = musteri.KayitTarihi.Value;
                else dtpKayitTarihi.Value = DateTime.Now;

                if (musteri.TemsilciID != null)
                {
                    comboCalisan.SelectedValue = musteri.TemsilciID;

                }
                else
                {
                    comboCalisan.SelectedIndex = -1;
                }

                if (musteri.Status.Value == true)
                    comboDurum.SelectedIndex = 0;
                else
                    comboDurum.SelectedIndex = 1;


                txtAdi.Text = musteri.MusteriAdi;

                if (musteri.IletisimId != null)
                {
                    txtAdres.Text = musteri.Iletisim.acikadres;
                    txtMail.Text = musteri.Iletisim.email;
                    txtTel1.Text = musteri.Iletisim.Tel1;
                    txtTel2.Text = musteri.Iletisim.Tel2;
                    if (musteri.Iletisim.UlkeId != null)
                        comboUlke.SelectedValue = musteri.Iletisim.UlkeId;
                    else comboUlke.SelectedIndex = -1;

                    if (musteri.Iletisim.IlId != null)
                        comboIL.SelectedValue = musteri.Iletisim.IlId;
                    else comboIL.SelectedIndex = -1;
                    
                    if (musteri.Iletisim.IlceId != null)
                        comboILCE.SelectedValue = musteri.Iletisim.IlceId;
                    else comboILCE.SelectedIndex = -1;
                }
                else
                {
                    txtAdres.Text = "";
                    txtMail.Text = "";
                    txtTel1.Text = "";
                    txtTel2.Text = "";
                    comboUlke.SelectedIndex = -1;
                    comboIL.SelectedIndex = -1;
                    comboILCE.SelectedIndex = -1;

                }


                toplamshow(musteri.MusteriNo);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        public void Ara()
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                // var sonuc = crm.Musterilers.Select(x => new { x.id, x.MusteriNo, x.MusteriAdi, x.Yetkili, x.VergiDairesi, x.VergiNo, x.Tckno, x.KayitTarihi , Temsilci= x.Calisan.Ad +" " + x.Calisan});
                var sonuc = crm.Musterilers.OrderByDescending(x => x.id).Where(x => x.MusteriAdi!=null);

                if (kontrolMusteriNo != "")
                {
                    sonuc = sonuc.Where(x =>x.MusteriNo==kontrolMusteriNo);
                    dataGridView1.DataSource = sonuc.Select(x => new { x.MusteriNo, x.MusteriAdi, Temsilci = x.Calisan.Ad + " " + x.Calisan.Soyad, x.Iletisim.Tel1, x.id, x.Status.Value });
                }
                

                if (comboOnOff.SelectedIndex == 1)
                {
                    sonuc = sonuc.Where(x => x.Status == true);
                }
                else if (comboOnOff.SelectedIndex ==2 )
                {
                    sonuc = sonuc.Where(x => x.Status == false);
                }


                if (txtkriter1.Text != "")
                {
                    sonuc = sonuc.Where(x => x.MusteriAdi.Contains(txtkriter1.Text)
                    || x.MusteriNo.Contains(txtkriter1.Text)
                    || x.Iletisim.Il.IlAdi.Contains(txtkriter1.Text)
                    || x.Calisan.Ad.Contains(txtkriter1.Text)                  
                    || x.Calisan.Soyad.Contains(txtkriter1.Text));
                }

                if (txtkriter2.Text != "")
                {
                    sonuc = sonuc.Where(x => x.MusteriAdi.Contains(txtkriter2.Text)
                    || x.MusteriNo.Contains(txtkriter2.Text)
                    || x.Iletisim.Il.IlAdi.Contains(txtkriter2.Text)
                    || x.Calisan.Ad.Contains(txtkriter2.Text)                  
                    || x.Calisan.Soyad.Contains(txtkriter2.Text));
                }


                if (txtkriter3.Text != "")
                {
                    sonuc = sonuc.Where(x => x.MusteriAdi.Contains(txtkriter3.Text)
                    || x.MusteriNo.Contains(txtkriter3.Text)
                    || x.Iletisim.Il.IlAdi.Contains(txtkriter3.Text)
                    || x.Calisan.Ad.Contains(txtkriter3.Text)                
                    || x.Calisan.Soyad.Contains(txtkriter3.Text));
                }


                //if (comboCalisan.SelectedIndex != -1)
                //{
                //    if (comboCalisan.SelectedValue is Calisan)
                //    { }
                //    else if ((comboCalisan.SelectedValue is Int32))
                //        sonuc = sonuc.Where(x => x.TemsilciID == (int)comboCalisan.SelectedValue);
                //}



                dataGridView1.DataSource = sonuc.Select(x=> new { x.MusteriNo, x.MusteriAdi,Temsilci=x.Calisan.Ad +" "+x.Calisan.Soyad, x.Iletisim.Tel1 ,x.id,x.Status.Value });//.Select(x => new {x.id, x.MusteriNo, x.MusteriAdi, x.Yetkili, x.VergiDairesi, x.VergiNo, x.Tckno, x.KayitTarihi, Temsilci = x.Calisan.Ad + " " + x.Calisan }); ;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count < 1)
                    return;
            
                if (dataGridView1.SelectedRows == null)
                    return;
                var row = dataGridView1.CurrentRow;
                activeitem = Int32.Parse(row.Cells[4].Value.ToString());
               GridToForm (activeitem);

                


            }
            catch (Exception ex)
            {


                MessageBox.Show(ex.Message);
            }
        }

        private void txtPMTS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                Ara();               

            }
        }
        
        private void txtkriter1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Ara();
            }
        }

        private void txtkriter2_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                Ara();
            }

        }

        private void txtkriter3_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                Ara();
            }
        }

        private void comboDurum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboDurum.SelectedIndex == 0)
                comboDurum.BackColor = Color.LimeGreen;
            else
                comboDurum.BackColor = Color.Red;

            btnSave.Select();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            
                //if (dataGridView1.SelectedRows.Count < 1)
                //    return;
                //var id = dataGridView1.CurrentRow.Cells[0].Value;
                //formSozlesmeler frm = new formSozlesmeler();
                //frm.Tag = id;
                //frm.Show();
        }

        private void comboOnOff_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ara();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var filename = "";
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                filename = op.FileName;
            }

            if (filename == "")
                return;

            var newthread = new Thread(new ThreadStart(() => {

                crmDFNDataContext crm = new crmDFNDataContext();

                var tum = File.ReadAllLines(op.FileName, Encoding.GetEncoding("iso-8859-9"));

                for (int i = 1; i < tum.Length; i++)
                {

                    Thread.Sleep(100);
                    var satir = tum[i].Split(';');

                    if (satir.Length > 2)
                    {
                        var musno = satir[1].Trim();
                        var isim = satir[0].Trim().Split(' ')[0];

                        if (crm.Calisans.Where(x => x.Ad.StartsWith(isim)).Any())
                        {
                         var Cal= crm.Calisans.FirstOrDefault(x => x.Ad.StartsWith(isim));

                            var mus = crm.Musterilers.FirstOrDefault(x => x.MusteriNo == musno);

                            if (mus.TemsilciID == null)
                            {


                                mus.TemsilciID = Cal.calisanID;
                                mus.Status = true;
                                crm.SubmitChanges();
                            }
                        }
                        else
                        {
                            var Cal = crm.Calisans.FirstOrDefault(x => x.Ad.StartsWith("Alican"));

                            var mus = crm.Musterilers.FirstOrDefault(x => x.MusteriNo == musno);
                            if (mus.TemsilciID == null)
                            {
                                mus.TemsilciID = Cal.calisanID;
                                mus.Status = true;
                                crm.SubmitChanges();
                            }
                        }



                    }


                        

                        lblsayacyaz(tum.Length.ToString()+" / " + (i + 1).ToString());


                    
                }




            }));

            newthread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //var newthread = new Thread(new ThreadStart(() => {

            //    crmDFNDataContext crm = new crmDFNDataContext();

            //    var sozlesmeler = crm.Sozlesmelers;
            //   var tum = sozlesmeler.ToList().Count;
            //    var i = 0;
            //    foreach (var item in sozlesmeler)
            //    {

            //       var mus= crm.Musterilers.FirstOrDefault(x => x.MusteriNo == item.MusteriNo);

            //      if(item.iletisimID==null)
            //        {
            //            if (mus.IletisimId != null)
            //            item.iletisimID = mus.IletisimId;
            //        }

            //        crm.SubmitChanges();
            //        i++;
            //    }


            var ekleneler = new List<string>();


            var filename = "";
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                filename = op.FileName;
            }

            if (filename == "")
                return;

            var newthread = new Thread(new ThreadStart(() => {

                crmDFNDataContext crm = new crmDFNDataContext();

                var tum = File.ReadAllLines(op.FileName, Encoding.GetEncoding("iso-8859-9"));

                for (int i = 1; i < tum.Length; i++)
                {

                    Thread.Sleep(100);
                    var satir = tum[i].Split(';');

                    if (satir.Length > 2)
                    {
                        var musno = satir[1].Trim();
                        var vergidairesi = satir[2].Trim();
                        var vergino = satir[3].Trim();
                        var adres = satir[4].Trim();
                        var il = satir[5].Trim();

                        if (crm.Musterilers.Where(x => x.MusteriNo==musno).Any())
                        {
                            var mus = crm.Musterilers.FirstOrDefault(x => x.MusteriNo == musno);

                            if (mus.IletisimId != null)
                            {
                                if (ekleneler.Contains(mus.MusteriNo))
                                { }
                                else
                                {
                                    var ileti = crm.Iletisims.FirstOrDefault(x => x.IletisimId == mus.IletisimId);

                                    ileti.acikadres = adres;
                                    ileti.UlkeId = 213;
                                    var ilbul = MyTools.SehirIdBul(il.ToLower());
                                    if (ilbul.Id >0)
                                    {
                                        if(ileti.IlId==null)
                                        ileti.IlId = ilbul.Id;
                                    }
                                       


                                    crm.SubmitChanges();

                                    ekleneler.Add(mus.MusteriNo);
                                }


                            }
                            else
                            {
                                if (!ekleneler.Contains(mus.MusteriNo))
                                {

                                    Iletisim ileti = new Iletisim();
                                    ileti.acikadres = adres;
                                    var ilbul = MyTools.SehirIdBul(il.ToLower());
                                    if (ilbul.Id != 0)
                                        ileti.IlId = ilbul.Id;

                                    crm.Iletisims.InsertOnSubmit(ileti);

                                    crm.SubmitChanges();

                                    mus.IletisimId = ileti.IletisimId;

                                    crm.SubmitChanges();
                                ekleneler.Add(mus.MusteriNo);
                                }

                            }

                        }
                        else
                        {


                        }



                    }




                    lblsayacyaz(tum.Length.ToString() + " / " + (i + 1).ToString());



                }




            }));

            newthread.Start();
        }

        private void comboCalisan_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (comboCalisan.SelectedIndex != -1 && loaded == true)
            //{
            //    txtAdi.Text = "";
            //    txtAdres.Text = "";
            //    txtYetkili.Text = "";
            //    txtBilgiIslem.Text = "";
            //    txtMail.Text = "";
            //    txtTel1.Text = "";
            //    txtTel2.Text = "";
            //    txtPMTS.Text = "";
            //    txtTckno.Text = "";
            //    txtVerdiDairesi.Text = "";
            //    txtVergiNo.Text = "";         
            //    comboIL.SelectedIndex = -1;
            //    comboILCE.SelectedIndex = -1;
            //    dtpKayitTarihi.Value = DateTime.Now;
            //    companentGizleGoster(true);
            //    comboDurum.SelectedIndex = 0;
            //    Ara();
            //}



        }
    }
}
