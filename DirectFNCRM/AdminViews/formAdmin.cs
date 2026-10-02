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
    public partial class formAdmin : Form
    {
        public formAdmin()
        {
            InitializeComponent();
        }

        public int activeitem = 0;
        public bool lisansdegistirme = false;
        private bool comboildoldurma = false;
        public Calisan ActiveCalisan;
        public int currectrowindex = 0;
        public bool saveoırupdate = false;



        private void calisanislemMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                AdminViews.formCalisancs cls = new formCalisancs();
                cls.Show();

            }
            catch (Exception ex)
            {


                MessageBox.Show(ex.Message);
            }
        }

        private void formAdmin_Load(object sender, EventArgs e)
        {
            try
            {
                this.dataGridView1.RowPrePaint
    += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(
        this.dataGridView1_RowPrePaint);

                

                crmDFNDataContext crm = new crmDFNDataContext();
                ActiveCalisan = (Calisan)this.Tag;

                this.Text += "  -  " + ActiveCalisan.Ad + " " + ActiveCalisan.Soyad;

                comboMusteriType.DataSource = crm.MusteriTypes;
                comboMusteriType.DisplayMember = "MusteriTypeAdi";
                comboMusteriType.ValueMember = "MusteriTypeId";


                comboMensei.DataSource = crm.MusteriMenseis;
                comboMensei.DisplayMember = "MenseiAdi";
                comboMensei.ValueMember = "id";


                comboUlke.DataSource = crm.Ulkes;
                comboUlke.DisplayMember = "UlkeAdi";
                comboUlke.ValueMember = "Id";

                comboKurum.DataSource = crm.Kurumlars;
                comboKurum.DisplayMember = "KurumAdi";
                comboKurum.ValueMember = "KurumID";

                comboStatus.DataSource = crm.Status;
                comboStatus.DisplayMember = "StatusAdi";
                comboStatus.ValueMember = "StatusId";



                comboUlke.SelectedItem = comboUlke.Items[212];

                comboildoldurma = true;

                gridguncelle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {


            var secili = dataGridView1.Rows[e.RowIndex].Cells[10].Value;
            if (secili != null)
            {
                if((bool)secili)
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                else
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;


            }
            //if (Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Text) < Convert.ToInt32(dataGridView1.Rows[e.RowIndex]..Cells[10].Text))
            //{
            //    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Beige;
            //}
        }

        private void departmanListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminViews.formDepartman cls = new formDepartman();
            cls.Show();

        }

        private void yetkiListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminViews.formYetki cls = new formYetki();
            cls.Show();

        }

        private void formAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {

            Application.Exit();
        }

        private void ıLListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (dataGridView1.Rows.Count > 0)
                currectrowindex = dataGridView1.CurrentRow.Index;

            crmDFNDataContext crm = new crmDFNDataContext();

            UserEvent ue = new UserEvent();
            ue.CalisanId = ActiveCalisan.calisanID;
            ue.EventTarih = DateTime.Now;




            if (activeitem == 0)
            {

                ue.EventTypeId = 4;

                #region Kontrol

                if (txtUserName.Text == "")
                { MessageBox.Show("UserName Kısmı Boş Bırakılamaz"); return; }

                if (crm.Users.Where(x => x.UserName == txtUserName.Text).Any())
                {
                    MessageBox.Show("Bu Username Başka Kullanıcı Tarafından Kullanılmaktadır.\n Lütfen Başka Bir Kullanıcı ismi Saçiniz.");
                    return;
                }

                #endregion
                #region yenikayit

                try
                {

                    User user = new User();

                    if (txtPassword.Text == "")
                    { MessageBox.Show("Password Kısmı Boş Bırakılamaz"); return; }

                    if (txtPassword.Text == "")
                    { MessageBox.Show("Password Kısmı Boş Bırakılamaz"); return; }

                    if (txtPMTSno.Text == "")
                    { MessageBox.Show("PMTS NO Kısmı Boş Bırakılamaz"); return; }






                    user.UserName = txtUserName.Text;
                    user.Password = MyTools.Sifreleme.Encryp(txtPassword.Text);
                    user.Name = txtAdi.Text;
                    user.Surname = txtSurname.Text;               
                    user.FXkurum = txtFxKurum.Text;
                    user.Aciklama = txtAciklama.Text;
                  //  user.SozlesmeNo = txtSozlesmeNo.Text;
                    //user.PmtsNo = txtPMTSno.Text;
                    user.PmtsNo = txtTckn.Text;

                    //if (comboMusteriType.SelectedIndex == 1)
                    //{
                    //    if (comboKurum.SelectedIndex == -1)
                    //    { MessageBox.Show("Bir Kurum Seçiniz."); return; }
                    //    user.KurumID = (int)comboKurum.SelectedValue;

                    //    var kur = crm.Kurumlars.FirstOrDefault(x => x.KurumID == user.KurumID);
                    //    user.PmtsNo = kur.PmtsNo;
                    //}


                    if (chkSCMDownload.Checked == true || chkSCMRealTime.Checked == true || chkSCMUsable.Checked == true)
                        user.ProductType = "SCM";
                    else
                        user.ProductType = "IDEAL";


               


                    user.tckno = txtTckn.Text;


                    user.StatusId = (int)comboStatus.SelectedValue;

                    #region Mensei
                    user.MusteriMenseiID = (int)comboMensei.SelectedValue;
                    #endregion
                    #region iletisim
                    Iletisim ileti = new Iletisim();
                    ileti.Tel1 = txtTel1.Text;
                    ileti.Tel2 = txtTel2.Text;
                    ileti.Ceptel = txtCeptel.Text;
                    ileti.UlkeId = (int)comboUlke.SelectedValue;
                    if (comboIL.SelectedValue != null)
                        ileti.IlId = (int)comboIL.SelectedValue;
                    if (comboILCE.SelectedValue != null)
                        ileti.IlceId = (int)comboILCE.SelectedValue;
                    ileti.acikadres = txtAdres.Text;
                    ileti.email = txtMail.Text;

                    crm.Iletisims.InsertOnSubmit(ileti);
                    crm.SubmitChanges();

                    user.iletisimId = ileti.IletisimId;
                    user.Aciklama = txtAciklama.Text;

                    user.BaslangicTarihi = dateTimeStartDate.Value;
                    user.ExpiryDate = dateTimeExpiry.Value;



                    #endregion
                    #region Lisanslar

                    LisansDurum lisans = new LisansDurum();
                    lisans.YayinDurumu = true;

                    if (comboYayinDurumu.SelectedIndex == 0)
                        lisans.YayinDurumu = true;
                    else if (comboYayinDurumu.SelectedIndex == 1)
                        lisans.YayinDurumu = false;
                    else
                    {
                        MessageBox.Show("Yayın Durumunu Belirtiniz !");
                        return;
                    }
                    // Bist Pay
                    lisans.PayL1 = chkPayL1.Checked;
                    lisans.PayLP = chkPayLP.Checked;
                    lisans.PayL2 = chkPayL2.Checked;
                    lisans.Pd2P = chkPd2P.Checked;
                    lisans.PayGS = chkPayGS.Checked;
                    lisans.PayX = chkPayX.Checked;
                    lisans.VeriAnalitik = chkVeriAnalitik.Checked;
                    //viop
                    lisans.ViopL1 = chkVL1.Checked;
                    lisans.ViopLP = chkVLP.Checked;
                    lisans.ViopL2 = chkVL2.Checked;
                    lisans.Vd2P = chkVd2P.Checked;
                    lisans.ViopGS = chkVGS.Checked;

                    // tahvil

                    lisans.TahvilL1 = chkTL1.Checked;
                    lisans.TahvilLP = chkTLP.Checked;
                    lisans.TahvilL2 = chkTL2.Checked;

                    // analiz pro
                    lisans.AnPro = chkAnPro.Checked;

                    // local
                    lisans.CepYetki = chkCep.Checked;
                    lisans.ProYetki = chkPro.Checked;
                    lisans.ROBOT = chkRobot.Checked;
                    lisans.Futgck = chkFutGck.Checked;
                    lisans.WINX = chkWINX.Checked;
                    lisans.SCMDownload = chkSCMDownload.Checked;
                    lisans.SCMRealTıme = chkSCMRealTime.Checked;
                    lisans.SCMUsable = chkSCMUsable.Checked;

                    // sase

                    lisans.SaseL1 = chksaseL1.Checked;
                    lisans.SaseL2 = chkSaseL2.Checked;

                    // yurtdışı


                    lisans.DJI = chkDJI.Checked;
                    lisans.XETRA = chkXetra.Checked;
                    lisans.SPI = chkSpI.Checked;
                    lisans.CBOT = chkCBOT.Checked;
                    lisans.CBOTM = chkCBOTM.Checked;
                    lisans.CME = chkCME.Checked;
                    lisans.CMEM = chkCMEM.Checked;
                    lisans.EUREX = chkEUREX.Checked;
                    lisans.COMEX = chkComex.Checked;
                    lisans.NYMEX = chkNymex.Checked;
                    lisans.NYMEXM = chkNymexM.Checked;
                    lisans.NYSE = chkNYSE.Checked;
                    lisans.NASDAQ = chkNASDAQ.Checked;
                    lisans.Amex = chkAMEX.Checked;
                    lisans.CHIX = chkCHIX.Checked;
                    lisans.LSE = chkLSE.Checked;



                    crm.LisansDurums.InsertOnSubmit(lisans);
                    crm.SubmitChanges();


                    ue.SonLisandurumID = lisans.LisansDurumId;

                    user.LisansDurumId = lisans.LisansDurumId;

                    crm.Users.InsertOnSubmit(user);
                    crm.SubmitChanges();

                    if (user.UserID > 0)
                    {
                        MessageBox.Show("Kayıt Başarılı");
                    }
                    ue.UserId = user.UserID;
                    crm.UserEvents.InsertOnSubmit(ue);
                    crm.SubmitChanges();

                    #endregion


                  


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Yeni Kayıt İşlemi Sırasında HATA\n" + ex.Message);
                }

                #endregion


               
            }
            else
            {

                #region Guncelle

                var useru = crm.Users.Where(x => x.UserID == activeitem).FirstOrDefault();
                var kaptianlar = new StringBuilder();
                var acilanlar = new StringBuilder();

                if (useru == null)
                {
                    MessageBox.Show("Böyle Bir kKayıt Bulunmuyor");
                    return;
                }

                ue.UserId = useru.UserID;



                if (chkSCMDownload.Checked == true || chkSCMRealTime.Checked == true || chkSCMUsable.Checked == true)
                    useru.ProductType = "SCM";
                else
                    useru.ProductType = "IDEAL";


                useru.UserName = txtUserName.Text;
                useru.Password = MyTools.Sifreleme.Encryp(txtPassword.Text);
                useru.Name = txtAdi.Text;
                useru.Surname = txtSurname.Text;
            
                useru.FXkurum = txtFxKurum.Text;
                useru.Aciklama = txtAciklama.Text;
             
             
              
               // useru.PmtsNo = txtPMTSno.Text;
                useru.PmtsNo = txtTckn.Text;
                useru.tckno = txtTckn.Text;
                useru.StatusId = (int) comboStatus.SelectedValue;




                //if (comboKurum.SelectedIndex != -1)
                //    if (useru.MusteriTypeID != 1)
                //    {
                //        useru.KurumID = (int)comboKurum.SelectedValue;

                //        var kurum = crm.Kurumlars.FirstOrDefault(x => x.KurumID == (int)comboKurum.SelectedValue);
                //        useru.PmtsNo = kurum.PmtsNo;
                //    }
                //    else
                //    {
                //        MessageBox.Show("Müşteri Tipini Değiştirn "); return; }

                //if (useru.MusteriTypeID == 2)
                //{
                //    if (comboKurum.SelectedIndex == -1)
                //    { MessageBox.Show("Bir Kurum Seçiniz"); return; }

                //}

                //if (comboMusteriType.SelectedIndex == 0)
                //    useru.Kurumlar = null;

                #region Mensei
                useru.MusteriMenseiID = (int)comboMensei.SelectedValue;
                #endregion

                #region iletisim

                var adresdegistimi = false;
                if (useru.Iletisim.Tel1 != txtTel1.Text)
                {
                    useru.Iletisim.Tel1 = txtTel1.Text;
                    adresdegistimi = true;
                }
                if (useru.Iletisim.Tel2 != txtTel2.Text)
                {
                    useru.Iletisim.Tel2 = txtTel2.Text;
                    adresdegistimi = true;
                }
                if (useru.Iletisim.Ceptel != txtCeptel.Text)
                {
                    useru.Iletisim.Ceptel = txtCeptel.Text;
                    adresdegistimi = true;
                }
                if( comboUlke.SelectedValue!=null)
                if (useru.Iletisim.UlkeId != (int)comboUlke.SelectedValue)
                {
                    useru.Iletisim.UlkeId = (int)comboUlke.SelectedValue;
                    adresdegistimi = true;
                }
                if (comboIL.SelectedValue != null)
                    if (useru.Iletisim.IlId != (int)comboIL.SelectedValue)
                {
                    useru.Iletisim.IlId = (int)comboIL.SelectedValue;
                    adresdegistimi = true;
                }

                if (comboILCE.SelectedValue != null)
                    if (useru.Iletisim.IlceId != (int)comboILCE.SelectedValue)
                    {

                        useru.Iletisim.IlceId = (int)comboILCE.SelectedValue;
                        adresdegistimi = true;

                    }
                if (useru.Iletisim.acikadres != txtAdres.Text)
                {
                    useru.Iletisim.acikadres = txtAdres.Text;
                    adresdegistimi = true;
                }
                if (useru.Iletisim.email != txtMail.Text)
                {
                    useru.Iletisim.email = txtMail.Text;
                    adresdegistimi = true;
                }
                #endregion

                #region Lisanslar

                int lastLisansId = (int)useru.LisansDurumId;

                LisansDurum lisanslar = new LisansDurum();

                var yayind = false;

                if (comboYayinDurumu.SelectedIndex == 0)
                    yayind = true;
                else yayind = false;




                if (useru.LisansDurum.YayinDurumu != yayind)
                {
                    if (yayind == true)
                        ue.EventTypeId = 1;
                    else
                        ue.EventTypeId = 2;

                    lisanslar.YayinDurumu = yayind;

                    crm.UserEvents.InsertOnSubmit(ue);
                    crm.SubmitChanges();

                }
                else
                    lisanslar.YayinDurumu = yayind;





                // Bist Pay
                if (useru.LisansDurum.PayL1 != chkPayL1.Checked)
                {
                    if (chkPayL1.Checked == true)
                    {
                        acilanlar.Append("PayL1;");
                    }
                    else
                    {
                        kaptianlar.Append("PayL1;");
                    }
                    lisanslar.PayL1 = chkPayL1.Checked;
                }
                else
                    lisanslar.PayL1 = useru.LisansDurum.PayL1;

                if (useru.LisansDurum.PayLP != chkPayLP.Checked)
                {
                    if (chkPayLP.Checked == true)
                    {
                        acilanlar.Append("PayLP;");
                    }
                    else
                    {
                        kaptianlar.Append("PayLP;");
                    }
                    lisanslar.PayLP = chkPayLP.Checked;
                }
                else
                    lisanslar.PayLP = useru.LisansDurum.PayLP;

                if (useru.LisansDurum.PayL2 != chkPayL2.Checked)
                {
                    if (chkPayL2.Checked == true) acilanlar.Append("PayL2;"); else kaptianlar.Append("PayL2;");
                    lisanslar.PayL2 = chkPayL2.Checked;
                }
                else
                    lisanslar.PayL2 = useru.LisansDurum.PayL2;

                if (useru.LisansDurum.Pd2P != chkPd2P.Checked)
                {
                    if (chkPd2P.Checked == true) acilanlar.Append("Pd2P;"); else kaptianlar.Append("Pd2P;");
                    lisanslar.Pd2P = chkPd2P.Checked;
                }
                else
                    lisanslar.Pd2P = useru.LisansDurum.Pd2P;

                if (useru.LisansDurum.PayGS != chkPayGS.Checked)
                {
                    if (chkPayGS.Checked == true) acilanlar.Append("PayGS;"); else kaptianlar.Append("PayGS;");
                    lisanslar.PayGS = chkPayGS.Checked;
                }
                else
                    lisanslar.PayGS = useru.LisansDurum.PayGS;

                if (useru.LisansDurum.PayX != chkPayX.Checked)
                {
                    if (chkPayX.Checked == true) acilanlar.Append("PayX;"); else kaptianlar.Append("PayX;");
                    lisanslar.PayX = chkPayX.Checked;
                }
                else
                    lisanslar.PayX = useru.LisansDurum.PayX;


                if (useru.LisansDurum.VeriAnalitik != chkVeriAnalitik.Checked)
                {
                    if (chkVeriAnalitik.Checked == true) acilanlar.Append("PayAnalitik;"); else kaptianlar.Append("PayAnalitik;");
                    lisanslar.VeriAnalitik = chkVeriAnalitik.Checked;
                }
                else
                    lisanslar.VeriAnalitik = useru.LisansDurum.VeriAnalitik;



                //viop

                if (useru.LisansDurum.ViopL1 != chkVL1.Checked)
                {
                    if (chkVL1.Checked == true) acilanlar.Append("ViopL1;"); else kaptianlar.Append("ViopL1;");
                    lisanslar.ViopL1 = chkVL1.Checked;
                }
                else
                    lisanslar.ViopL1 = useru.LisansDurum.ViopL1;


                if (useru.LisansDurum.ViopLP != chkVLP.Checked)
                {
                    if (chkVLP.Checked == true) acilanlar.Append("ViopLP;"); else kaptianlar.Append("ViopLP;");
                    lisanslar.ViopLP = chkVLP.Checked;
                }
                else
                    lisanslar.ViopLP = useru.LisansDurum.ViopLP;

                if (useru.LisansDurum.ViopL2 != chkVL2.Checked)
                {
                    if (chkVL2.Checked == true) acilanlar.Append("ViopL2;"); else kaptianlar.Append("ViopL2;");
                    lisanslar.ViopL2 = chkVL2.Checked;
                }
                else
                    lisanslar.ViopL2 = useru.LisansDurum.ViopL2;

                if (useru.LisansDurum.Vd2P != chkVd2P.Checked)
                {
                    if (chkVd2P.Checked == true) acilanlar.Append("Vd2P;"); else kaptianlar.Append("Vd2P;");
                    lisanslar.Vd2P = chkVd2P.Checked;
                }
                else
                    lisanslar.Vd2P = useru.LisansDurum.Vd2P;

                if (useru.LisansDurum.ViopGS != chkVGS.Checked)
                {
                    if (chkVGS.Checked == true) acilanlar.Append("ViopGS;"); else kaptianlar.Append("ViopGS;");
                    lisanslar.ViopGS = chkVGS.Checked;
                }
                else
                    lisanslar.ViopGS = useru.LisansDurum.ViopGS;


                // tahvil

                if (useru.LisansDurum.TahvilL1 != chkTL1.Checked)
                {
                    if (chkTL1.Checked == true) acilanlar.Append("TahvilL1;"); else kaptianlar.Append("TahvilL1;");
                    lisanslar.TahvilL1 = chkTL1.Checked;
                }
                else
                    lisanslar.TahvilL1 = useru.LisansDurum.TahvilL1;


                if (useru.LisansDurum.TahvilLP != chkTLP.Checked)
                {
                    if (chkTLP.Checked == true) acilanlar.Append("TahvilLP;"); else kaptianlar.Append("TahvilLP;");
                    lisanslar.TahvilLP = chkTLP.Checked;
                }
                else
                    lisanslar.TahvilLP = useru.LisansDurum.TahvilLP;


                if (useru.LisansDurum.TahvilL2 != chkTL2.Checked)
                {
                    if (chkTL2.Checked == true) acilanlar.Append("TahvilL2;"); else kaptianlar.Append("TahvilL2;");
                    lisanslar.TahvilL2 = chkTL2.Checked;
                }
                else
                    lisanslar.TahvilL2 = useru.LisansDurum.TahvilL2;



                // analiz Pro

                if (useru.LisansDurum.AnPro != chkAnPro.Checked)
                {
                    if (chkAnPro.Checked == true) acilanlar.Append("AnalizPro;"); else kaptianlar.Append("AnalizPro;");
                    lisanslar.AnPro = chkAnPro.Checked;
                }
                else
                    lisanslar.AnPro = useru.LisansDurum.AnPro;


                // local


                if (useru.LisansDurum.CepYetki != chkCep.Checked)
                {
                    if (chkCep.Checked == true) acilanlar.Append("CepYetki;"); else kaptianlar.Append("CepYetki;");
                    lisanslar.CepYetki = chkCep.Checked;
                }
                else
                    lisanslar.CepYetki = useru.LisansDurum.CepYetki;



                if (useru.LisansDurum.ProYetki != chkPro.Checked)
                {
                    if (chkPro.Checked == true) acilanlar.Append("ProYetki;"); else kaptianlar.Append("ProYetki;");
                    lisanslar.ProYetki = chkPro.Checked;
                }
                else
                    lisanslar.ProYetki = useru.LisansDurum.ProYetki;

                if (useru.LisansDurum.ROBOT != chkRobot.Checked)
                {
                    if (chkRobot.Checked == true) acilanlar.Append("ROBOT;"); else kaptianlar.Append("ROBOT;");
                    lisanslar.ROBOT = chkRobot.Checked;
                }
                else
                    lisanslar.ROBOT = useru.LisansDurum.ROBOT;


                lisanslar.Futgck = chkFutGck.Checked;
                lisanslar.WINX = chkWINX.Checked;
                lisanslar.SCMDownload = chkSCMDownload.Checked;
                lisanslar.SCMRealTıme = chkSCMRealTime.Checked;
                lisanslar.SCMUsable = chkSCMUsable.Checked;

                // sase

                lisanslar.SaseL1 = chksaseL1.Checked;
                lisanslar.SaseL2 = chkSaseL2.Checked;

                // yurtdışı



                if (useru.LisansDurum.DJI != chkDJI.Checked)
                {
                    if (chkDJI.Checked == true) acilanlar.Append("DJI;"); else kaptianlar.Append("DJI;");
                    lisanslar.DJI = chkDJI.Checked;
                }
                else
                    lisanslar.DJI = useru.LisansDurum.DJI;

                if (useru.LisansDurum.XETRA != chkXetra.Checked)
                {
                    if (chkXetra.Checked == true) acilanlar.Append("XETRA;"); else kaptianlar.Append("XETRA;");
                    lisanslar.XETRA = chkXetra.Checked;
                }
                else
                    lisanslar.XETRA = useru.LisansDurum.XETRA;

                if (useru.LisansDurum.SPI != chkSpI.Checked)
                {
                    if (chkSpI.Checked == true) acilanlar.Append("SPI;"); else kaptianlar.Append("SPI;");
                    lisanslar.SPI = chkSpI.Checked;
                }
                else
                    lisanslar.SPI = useru.LisansDurum.SPI;

                if (useru.LisansDurum.CBOT != chkCBOT.Checked)
                {
                    if (chkCBOT.Checked == true) acilanlar.Append("CBOT;"); else kaptianlar.Append("CBOT;");
                    lisanslar.CBOT = chkCBOT.Checked;
                }
                else
                    lisanslar.CBOT = useru.LisansDurum.CBOT;


                if (useru.LisansDurum.CBOTM != chkCBOTM.Checked)
                {
                    if (chkCBOTM.Checked == true) acilanlar.Append("CBOTM;"); else kaptianlar.Append("CBOTM;");
                    lisanslar.CBOTM = chkCBOTM.Checked;
                }
                else
                    lisanslar.CBOTM = useru.LisansDurum.CBOTM;


                if (useru.LisansDurum.CME != chkCME.Checked)
                {
                    if (chkCME.Checked == true) acilanlar.Append("CME;"); else kaptianlar.Append("CME;");
                    lisanslar.CME = chkCME.Checked;
                }
                else
                    lisanslar.CME = useru.LisansDurum.CME;

                if (useru.LisansDurum.CMEM != chkCMEM.Checked)
                {
                    if (chkCMEM.Checked == true) acilanlar.Append("CMEM;"); else kaptianlar.Append("CMEM;");
                    lisanslar.CMEM = chkCMEM.Checked;
                }
                else
                    lisanslar.CMEM = useru.LisansDurum.CMEM;


                if (useru.LisansDurum.EUREX != chkEUREX.Checked)
                {
                    if (chkEUREX.Checked == true) acilanlar.Append("EUREX;"); else kaptianlar.Append("EUREX;");
                    lisanslar.EUREX = chkEUREX.Checked;
                }
                else
                    lisanslar.EUREX = useru.LisansDurum.EUREX;



                lisanslar.COMEX = chkComex.Checked;
                lisanslar.NYMEX = chkNymex.Checked;
                lisanslar.NYMEXM = chkNymexM.Checked;
                lisanslar.NYSE = chkNYSE.Checked;
                lisanslar.NASDAQ = chkNASDAQ.Checked;
                lisanslar.Amex = chkAMEX.Checked;
                lisanslar.CHIX = chkCHIX.Checked;
                lisanslar.LSE = chkLSE.Checked;

                if (lisansdegistirme)
                {

                    if (acilanlar.ToString() != "" || kaptianlar.ToString() != "")
                    {
                        if (acilanlar.ToString() != "" && kaptianlar.ToString() != "")
                        {
                            ue.EventTypeId = 7;
                            ue.KapatilanLisans = kaptianlar.ToString();
                            ue.AcilanLisans = acilanlar.ToString();

                        }
                        else
                        {
                            if (acilanlar.ToString() != "")
                            {
                                ue.EventTypeId = 3;
                                ue.AcilanLisans = acilanlar.ToString();
                            }

                            if (kaptianlar.ToString() != "")
                            {
                                ue.EventTypeId = 6;
                                ue.KapatilanLisans = kaptianlar.ToString();
                            }
                        }

                     


                    }
                    else
                    {
                        ue.EventTypeId = 8;
                      
                    }

                }


                #endregion

                crm.LisansDurums.InsertOnSubmit(lisanslar);
                crm.SubmitChanges();
                useru.LisansDurum = lisanslar;
                crm.SubmitChanges();
                ue.SonLisandurumID = lastLisansId;
                if(ue.EventId<1)
                crm.UserEvents.InsertOnSubmit(ue);
                crm.SubmitChanges();
                MessageBox.Show(useru.UserName + " Kullanıcısının bilgileri güncellendi");
                if (adresdegistimi)
                {
                    MessageBox.Show(useru.UserName + " Adres bilgileride değişti");
                }




                activeitem = useru.UserID;



                #endregion


            }

            saveoırupdate = true;
            gridguncelle();

        }

        private void comboUlke_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboUlke.SelectedIndex == -1)
                    return;

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


                if (!comboildoldurma)
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

                if (!comboildoldurma)
                {
                    comboILCE.SelectedIndex = -1;
                }

                if (comboUlke.SelectedIndex != 212)
                {
                    comboILCE.SelectedIndex = -1;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {

        }


        // my metot
        private void GridToForm()
        {
            try
            {
               // if (saveoırupdate)
               
                if (saveoırupdate && dataGridView1.CurrentRow.Index == 0)
                    { saveoırupdate = false; return; }
                crmDFNDataContext crm = new crmDFNDataContext();
                if (dataGridView1.SelectedRows == null)
                    return;
                var row = dataGridView1.CurrentRow;

                activeitem = Int32.Parse(row.Cells[0].Value.ToString());
                lisansdegistirme = false;
                LisansEtkin(false);

                var user = crm.Users.Where(x => x.UserID == activeitem).FirstOrDefault();



                txtUserName.Text = user.UserName;
                txtPassword.Text = MyTools.Sifreleme.Decryp(user.Password);
                txtAdi.Text = user.Name;
                txtAciklama.Text = user.Aciklama;
                txtSurname.Text = user.Surname;              
           
                txtFxKurum.Text = user.FXkurum;         
          
                txtPMTSno.Text = user.PmtsNo;
                txtTckn.Text = user.tckno;

                dateTimeStartDate.Value = user.BaslangicTarihi.Value;
                dateTimeExpiry.Value = user.ExpiryDate.Value;

                if (user.StatusId!=null)
                comboStatus.SelectedValue = user.StatusId;


                //if (user.KurumID != null)
                //    comboKurum.SelectedValue = user.KurumID;
                //else
                //    comboKurum.SelectedIndex = -1;





                txtTel1.Text = user.Iletisim.Tel1;
                txtTel2.Text = user.Iletisim.Tel2;
                txtCeptel.Text = user.Iletisim.Ceptel;
                txtMail.Text = user.Iletisim.email;
                txtAdres.Text = user.Iletisim.acikadres;
                comboUlke.SelectedValue = user.Iletisim.UlkeId;
                if (user.Iletisim.IlId != null)
                { comboIL.SelectedValue = user.Iletisim.IlId; }
                else comboIL.SelectedIndex = -1;
                if (user.Iletisim.Ilce != null)
                { comboILCE.SelectedValue = user.Iletisim.IlceId; }
                else comboILCE.SelectedIndex = -1;

                dateTimeStartDate.Value = user.BaslangicTarihi.Value;

                #region Lisanlaroku

                var lisansdurum = user.LisansDurum;
                if (lisansdurum != null)
                {
                    if (lisansdurum.YayinDurumu)
                    {
                        comboYayinDurumu.SelectedIndex = 0;
                        comboYayinDurumu.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        comboYayinDurumu.SelectedIndex = 1;
                        comboYayinDurumu.BackColor = Color.Red;
                    }


                    chkCep.Checked = (bool)lisansdurum.CepYetki;
                    chkPro.Checked = (bool)lisansdurum.ProYetki;
                    chkPayL1.Checked = (bool)lisansdurum.PayL1;
                    chkPayLP.Checked = (bool)lisansdurum.PayLP;
                    chkPayL2.Checked = (bool)lisansdurum.PayL2;
                    chkPd2P.Checked = (bool)lisansdurum.Pd2P;
                    chkPayGS.Checked = (bool)lisansdurum.PayGS;
                    chkPayX.Checked = (bool)lisansdurum.PayX;
                    chkVeriAnalitik.Checked = (bool)lisansdurum.VeriAnalitik;

                    chkVL1.Checked = (bool)lisansdurum.ViopL1;
                    chkVLP.Checked = (bool)lisansdurum.ViopLP;
                    chkVL2.Checked = (bool)lisansdurum.ViopL2;
                    chkVd2P.Checked = (bool)lisansdurum.Vd2P;
                    chkVGS.Checked = (bool)lisansdurum.ViopGS;

                    chkTL1.Checked = (bool)lisansdurum.TahvilL1;
                    chkTL2.Checked = (bool)lisansdurum.TahvilL2;
                    chkTLP.Checked = (bool)lisansdurum.TahvilLP;

                    chkAnPro.Checked = (bool)lisansdurum.AnPro;

                    chksaseL1.Checked = (bool)lisansdurum.SaseL1;
                    chkSaseL2.Checked = (bool)lisansdurum.SaseL2;

                    chkFutGck.Checked = (bool)lisansdurum.Futgck;
                    chkWINX.Checked = (bool)lisansdurum.WINX;

                    chkDJI.Checked = (bool)lisansdurum.DJI;
                    chkXetra.Checked = (bool)lisansdurum.XETRA;
                    chkSpI.Checked = (bool)lisansdurum.SPI;
                    chkCBOT.Checked = (bool)lisansdurum.CBOT;
                    chkCBOTM.Checked = (bool)lisansdurum.CBOTM;
                    chkCME.Checked = (bool)lisansdurum.CME;
                    chkCMEM.Checked = (bool)lisansdurum.CMEM;
                    chkEUREX.Checked = (bool)lisansdurum.EUREX;
                    chkComex.Checked = (bool)lisansdurum.COMEX;
                    chkNymex.Checked = (bool)lisansdurum.NYMEX;
                    chkNymexM.Checked = (bool)lisansdurum.NYMEXM;
                    chkNYSE.Checked = (bool)lisansdurum.NYSE;
                    chkNASDAQ.Checked = (bool)lisansdurum.NASDAQ;
                    chkAMEX.Checked = (bool)lisansdurum.Amex;
                    chkCHIX.Checked = (bool)lisansdurum.CHIX;
                    chkLSE.Checked = (bool)lisansdurum.LSE;

                    chkRobot.Checked = (bool)lisansdurum.ROBOT;
                    chkSCMDownload.Checked = (bool)lisansdurum.SCMDownload;
                    chkSCMRealTime.Checked = (bool)lisansdurum.SCMRealTıme;
                    chkSCMUsable.Checked = (bool)lisansdurum.SCMUsable;


                }


                #endregion
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        private void GridToForm(User user)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                if (dataGridView1.SelectedRows == null)
                    return;
                var row = dataGridView1.CurrentRow;
                activeitem = user.UserID;// Int32.Parse(row.Cells[0].Value.ToString());



                txtUserName.Text = user.UserName;
                txtPassword.Text = MyTools.Sifreleme.Decryp(user.Password);
                txtAdi.Text = user.Name;
                txtSurname.Text = user.Surname;
               
           
               
                txtPMTSno.Text = user.PmtsNo;
                txtTckn.Text = user.tckno;
                comboStatus.SelectedValue = user.StatusId;


                //if (user.KurumID != null)
                //    comboKurum.SelectedValue = user.KurumID;
                //else
                //    comboKurum.SelectedIndex = -1;





                txtTel1.Text = user.Iletisim.Tel1;
                txtTel2.Text = user.Iletisim.Tel2;
                txtCeptel.Text = user.Iletisim.Ceptel;
                txtMail.Text = user.Iletisim.email;
                txtAdres.Text = user.Iletisim.acikadres;
                comboUlke.SelectedValue = user.Iletisim.UlkeId;
                if (user.Iletisim.IlId != null)
                { comboIL.SelectedValue = user.Iletisim.IlId; }
                else comboIL.SelectedIndex = -1;
                if (user.Iletisim.Ilce != null)
                { comboILCE.SelectedValue = user.Iletisim.IlceId; }
                else comboILCE.SelectedIndex = -1;


                dateTimeStartDate.Value = user.BaslangicTarihi.Value;

                #region Lisanlaroku

                var lisansdurum = user.LisansDurum;
                if (lisansdurum != null)
                {
                    if (lisansdurum.YayinDurumu)
                    {
                        comboYayinDurumu.SelectedIndex = 0;
                        comboYayinDurumu.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        comboYayinDurumu.SelectedIndex = 1;
                        comboYayinDurumu.BackColor = Color.Red;
                    }


                    chkCep.Checked = (bool)lisansdurum.CepYetki;
                    chkPro.Checked = (bool)lisansdurum.ProYetki;
                    chkPayL1.Checked = (bool)lisansdurum.PayL1;
                    chkPayLP.Checked = (bool)lisansdurum.PayLP;
                    chkPayL2.Checked = (bool)lisansdurum.PayL2;
                    chkPd2P.Checked = (bool)lisansdurum.Pd2P;
                    chkPayGS.Checked = (bool)lisansdurum.PayGS;
                    chkPayX.Checked = (bool)lisansdurum.PayX;
                    chkVeriAnalitik.Checked = (bool)lisansdurum.VeriAnalitik;

                    chkVL1.Checked = (bool)lisansdurum.ViopL1;
                    chkVLP.Checked = (bool)lisansdurum.ViopLP;
                    chkVL2.Checked = (bool)lisansdurum.ViopL2;
                    chkVd2P.Checked = (bool)lisansdurum.Vd2P;
                    chkVGS.Checked = (bool)lisansdurum.ViopGS;

                    chkTL1.Checked = (bool)lisansdurum.TahvilL1;
                    chkTL2.Checked = (bool)lisansdurum.TahvilL2;
                    chkTLP.Checked = (bool)lisansdurum.TahvilLP;

                    chkAnPro.Checked = (bool)lisansdurum.AnPro;

                    chksaseL1.Checked = (bool)lisansdurum.SaseL1;
                    chkSaseL2.Checked = (bool)lisansdurum.SaseL2;

                    chkFutGck.Checked = (bool)lisansdurum.Futgck;
                    chkWINX.Checked = (bool)lisansdurum.WINX;

                    chkDJI.Checked = (bool)lisansdurum.DJI;
                    chkXetra.Checked = (bool)lisansdurum.XETRA;
                    chkSpI.Checked = (bool)lisansdurum.SPI;
                    chkCBOT.Checked = (bool)lisansdurum.CBOT;
                    chkCBOTM.Checked = (bool)lisansdurum.CBOTM;
                    chkCME.Checked = (bool)lisansdurum.CME;
                    chkCMEM.Checked = (bool)lisansdurum.CMEM;
                    chkEUREX.Checked = (bool)lisansdurum.EUREX;
                    chkComex.Checked = (bool)lisansdurum.COMEX;
                    chkNymex.Checked = (bool)lisansdurum.NYMEX;
                    chkNymexM.Checked = (bool)lisansdurum.NYMEXM;
                    chkNYSE.Checked = (bool)lisansdurum.NYSE;
                    chkNASDAQ.Checked = (bool)lisansdurum.NASDAQ;
                    chkAMEX.Checked = (bool)lisansdurum.Amex;
                    chkCHIX.Checked = (bool)lisansdurum.CHIX;
                    chkLSE.Checked = (bool)lisansdurum.LSE;

                    chkRobot.Checked = (bool)lisansdurum.ROBOT;
                    chkSCMDownload.Checked = (bool)lisansdurum.SCMDownload;
                    chkSCMRealTime.Checked = (bool)lisansdurum.SCMRealTıme;
                    chkSCMUsable.Checked = (bool)lisansdurum.SCMUsable;


                }


                #endregion
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        public void LisansEtkin(bool durum)
        {

            lisansdegistirme = durum;
            chkPayL1.Enabled = durum;
            chkPayLP.Enabled = durum;
            chkPayL2.Enabled = durum;
            chkPd2P.Enabled = durum;
            chkPayLP.Enabled = durum;
            chkPayGS.Enabled = durum;
            chkPayX.Enabled = durum;
            chkVeriAnalitik.Enabled = durum;
            chkVL1.Enabled = durum;
            chkVLP.Enabled = durum;
            chkVL2.Enabled = durum;
            chkVd2P.Enabled = durum;
            chkVGS.Enabled = durum;
            chkTL1.Enabled = durum;
            chkTL2.Enabled = durum;
            chkTLP.Enabled = durum;
            chkAnPro.Enabled = durum;

            chkCep.Enabled = durum;
            chkPro.Enabled = durum;
            chkRobot.Enabled = durum;
            chkWINX.Enabled = durum;
            chkFutGck.Enabled = durum;
            chkSCMDownload.Enabled = durum;
            chkSCMRealTime.Enabled = durum;
            chkSCMUsable.Enabled = durum;
            chksaseL1.Enabled = durum;
            chkSaseL2.Enabled = durum;


            chkDJI.Enabled = durum;
            chkXetra.Enabled = durum;
            chkSpI.Enabled = durum;
            chkCBOTM.Enabled = durum;
            chkEUREX.Enabled = durum;
            chkCMEM.Enabled = durum;
            chkCBOT.Enabled = durum;
            chkCME.Enabled = durum;
            chkComex.Enabled = durum;
            chkNymex.Enabled = durum;
            chkNymexM.Enabled = durum;
            chkNASDAQ.Enabled = durum;
            chkNYSE.Enabled = durum;
            chkLSE.Enabled = durum;
            chkCHIX.Enabled = durum;
            chkAMEX.Enabled = durum;
        }
        public void formTemizle()
        {
            try
            {



                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtAdi.Text = string.Empty;
                txtSurname.Text = string.Empty;
                txtAciklama.Text = string.Empty;
             
                comboMusteriType.SelectedItem = comboMusteriType.Items[0];
          
                txtPMTSno.Text = string.Empty;
                txtSozlesmeNo.Text = string.Empty;
                txtTckn.Text = string.Empty;
                txtFxKurum.Text = string.Empty;
                comboStatus.SelectedValue = 1;


                txtTel1.Text = string.Empty;
                txtTel2.Text = string.Empty;
                txtCeptel.Text = string.Empty;
                txtMail.Text = string.Empty;
                txtAdres.Text = string.Empty;
                comboUlke.SelectedItem = comboUlke.Items[212];
                comboIL.SelectedIndex = -1;
                comboILCE.SelectedIndex = -1;

                dateTimeStartDate.Value = DateTime.Now;

   

                comboYayinDurumu.SelectedIndex = 0;


                chkPayL1.Checked = false;
                chkPayLP.Checked = false;
                chkPayL2.Checked = false;
                chkPd2P.Checked = false;
                chkPayGS.Checked = false;
                chkPayX.Checked = false;
                chkVL1.Checked = false;
                chkVLP.Checked = false;
                chkVL2.Checked = false;
                chkVd2P.Checked = false;
                chkVGS.Checked = false;
                chkTL1.Checked = false;
                chkTL2.Checked = false;
                chkTLP.Checked = false;
                chkAnPro.Checked = false;
                chksaseL1.Checked = false;
                chkSaseL2.Checked = false;
                chkFutGck.Checked = false;
                chkWINX.Checked = false;

                chkDJI.Checked = false;
                chkXetra.Checked = false;
                chkSpI.Checked = false;
                chkCBOT.Checked = false;
                chkCBOTM.Checked = false;
                chkCME.Checked = false;
                chkCMEM.Checked = false;
                chkEUREX.Checked = false;
                chkComex.Checked = false;
                chkNymex.Checked = false;
                chkNymexM.Checked = false;
                chkNYSE.Checked = false;
                chkNASDAQ.Checked = false;
                chkAMEX.Checked = false;
                chkCHIX.Checked = false;
                chkLSE.Checked = false;

                chkSCMDownload.Checked = false;
                chkSCMRealTime.Checked = false;
                chkSCMUsable.Checked = false;



            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public void gridguncelle()
        {
            try
            {
               
                using (crmDFNDataContext crm = new crmDFNDataContext())
                {
                    var sonuc = crm.Users.OrderByDescending(x=>x.BaslangicTarihi).Select(x => new { x.UserID, x.UserName, Adı_Soyadı = x.Name + " " + x.Surname, x.BaslangicTarihi, x.ExpiryDate, x.PmtsNo, x.MusteriMensei.MenseiAdi, x.Iletisim.Il.IlAdi, x.Aciklama,
                        x.LisansDurum.YayinDurumu
                    });
                    if (sonuc != null)
                        dataGridView1.DataSource = sonuc;
                }


              //  dataGridView1.Rows[currectrowindex].Selected = true;
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }

        public string lisanstoString(LisansDurum lisanlar)
        {
            try
            {
                var sb = new StringBuilder();
                if (lisanlar == null)
                    return "";


                if (lisanlar.YayinDurumu)
                    sb.Append("AÇIK;");
                else
                    sb.Append("KAPALI;");


                if (lisanlar.PayL1)
                    sb.Append("PayL1;");
                if (lisanlar.PayLP)
                    sb.Append("PayLP;");
                if (lisanlar.PayL2)
                    sb.Append("PayL2;");
                if (lisanlar.Pd2P)
                    sb.Append("Pd2P;");
                if (lisanlar.PayX)
                    sb.Append("PayX;");
                if (lisanlar.PayGS)
                    sb.Append("PayGS;");
                if (lisanlar.VeriAnalitik)
                    sb.Append("VeriAnalitik;");

                //viop
                if (lisanlar.ViopL1)
                    sb.Append("ViopL1;");
                if (lisanlar.ViopLP)
                    sb.Append("ViopLP;");
                if (lisanlar.ViopL2)
                    sb.Append("ViopL2;");
                if (lisanlar.Vd2P)
                    sb.Append("Vd2P;");
                if (lisanlar.ViopGS)
                    sb.Append("ViopGS;");
                //tahvil
                if (lisanlar.TahvilL1)
                    sb.Append("TahvilL1;");
                if (lisanlar.TahvilL2)
                    sb.Append("TahvilL2;");
                if (lisanlar.TahvilLP)
                    sb.Append("TahvilLP;");

                // analiz pro
                if (lisanlar.AnPro)
                    sb.Append("AnalizPro;");


                // local
                if (lisanlar.CepYetki)
                    sb.Append("CepYetki;");
                if (lisanlar.ProYetki)
                    sb.Append("ProYetki;");
                if (lisanlar.ROBOT)
                    sb.Append("ROBOT;");
                if (lisanlar.DJI)
                    sb.Append("DJI;");
                if (lisanlar.XETRA)
                    sb.Append("XETRA;");
                if (lisanlar.SPI)
                    sb.Append("SPI;");
                if (lisanlar.CBOTM)
                    sb.Append("CBOTM;");
                if (lisanlar.EUREX)
                    sb.Append("EUREX;");
                if (lisanlar.CMEM)
                    sb.Append("CMEM;");
                if (lisanlar.CBOT)
                    sb.Append("CBOT;");
                if (lisanlar.CME)
                    sb.Append("CME;");
                if (lisanlar.COMEX)
                    sb.Append("COMEX;");
                if (lisanlar.NYMEX)
                    sb.Append("NYMEX;");
                if (lisanlar.NYMEXM)
                    sb.Append("NYMEXM;");
                if (lisanlar.NYSE)
                    sb.Append("NYSE;");
                if (lisanlar.NASDAQ)
                    sb.Append("NASDAQ;");
                if (lisanlar.Amex)
                    sb.Append("Amex;");
                if (lisanlar.CHIX)
                    sb.Append("CHIX;");
                if (lisanlar.LSE)
                    sb.Append("LSE;");

                return sb.ToString();


            }
            catch 
            {

                return "";
            }


        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                
                //UserEvent ue = new UserEvent();
                //ue.CalisanId = ActiveCalisan.calisanID;
                //ue.EventTarih = DateTime.Now;

                //if (activeitem == 0)
                //{
                //    MessageBox.Show("Silmek için bir Kullanıcı seçiniz");
                //    return;
                //}

                //crmDFNDataContext crm = new crmDFNDataContext();
                //var user = crm.Users.Where(x => x.UserID == activeitem).FirstOrDefault();
                //var ilet = crm.Iletisims.Where(x => x.IletisimId == user.iletisimId).FirstOrDefault();

                //if (MessageBox.Show(user.UserName + " silinecek eminmisiniz", "Silme İşlemi !", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                //{
                //    ue.UserId = user.UserID;
                //    ue.EventTypeId = 5;
                //    ue.SonLisanslar = lisanstoString(user.LisansDurum);
                //    crm.Users.DeleteOnSubmit(user);
                //    crm.SubmitChanges();

                //    crm.UserEvents.InsertOnSubmit(ue);
                //    crm.SubmitChanges();


                //    crm.Iletisims.DeleteOnSubmit(ilet);
                //    crm.SubmitChanges();
                //    MessageBox.Show("Kayıt Silindi");

                //}


                //gridguncelle();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnYenikayit_Click(object sender, EventArgs e)
        {
            activeitem = 0;
            lisansdegistirme = false;
            LisansEtkin(true);
            comboYayinDurumu.SelectedIndex = 0;
            formTemizle();
        }


     

        private void contextLisans_Opening(object sender, CancelEventArgs e)
        {

            
        }

        private void lisanDegistirToolStripMenuItem_Click(object sender, EventArgs e)
        {



            if (comboYayinDurumu.SelectedIndex != 1)
            { LisansEtkin(true);  lisansdegistirme = true; }
            else
            { MessageBox.Show("Bu kullanıcının yayını kapalı lisans eklemeden önce yayınını açmalısınız"); return; }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
                return;

            GridToForm();
        }

        private void comboYayinDurumu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboYayinDurumu.SelectedIndex == 0)
                comboYayinDurumu.BackColor = Color.LimeGreen;
            else if (comboYayinDurumu.SelectedIndex == 1)
            {
                comboYayinDurumu.BackColor = Color.Red;
            }

            btnSave.Select();
        }

        private void userOlaylarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminViews.formOlayGoruntule frm = new formOlayGoruntule();

            frm.ShowDialog();
        }

        private void txtPMTSno_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void faturaBilgileriToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void kurumlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formMusteriler frm = new formMusteriler();
            frm.ShowDialog();
        }

        private void comboMusteriType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboMusteriType.SelectedIndex != -1)
                if (comboildoldurma)
            {

                var item = (int)comboMusteriType.SelectedValue;
                if (item == 1)
                {
                    comboKurum.SelectedIndex = -1;
                } 
            }
        }

        private void comboKurum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboKurum.SelectedIndex !=-1)
            if (comboildoldurma)
            {
                var item = (int)comboKurum.SelectedValue;
                if (comboKurum.SelectedIndex>- 1)
                {
                    comboMusteriType.SelectedValue = 2;
                } 
            }
        }

        private void chkPayLP_CheckedChanged(object sender, EventArgs e)
        {
            chkPayL1.Checked = true;
        }

        private void chkPayL2_CheckedChanged(object sender, EventArgs e)
        {
            chkPayL1.Checked = true;
            chkPayLP.Checked = true;

        }

        private void chkPd2P_CheckedChanged(object sender, EventArgs e)
        {
            chkPayL1.Checked = true;
            chkPayLP.Checked = true;
            chkPayL2.Checked = true;
        }
        private void chkVLP_CheckedChanged(object sender, EventArgs e)
        {
            chkVL1.Checked = true;
          
        }

        private void chkVL2_CheckedChanged(object sender, EventArgs e)
        {
            chkVL1.Checked = true;
            chkVLP.Checked = true;
        }

        private void chkVd2P_CheckedChanged(object sender, EventArgs e)
        {
            chkVL1.Checked = true;
            chkVLP.Checked = true;
            chkVL2.Checked = true;
        }
        private void chkTLP_CheckedChanged(object sender, EventArgs e)
        {
            
            chkTL1.Checked = true;
        }

        private void chkTL2_CheckedChanged(object sender, EventArgs e)
        {
           
            chkTL1.Checked = true;
            chkTLP.Checked = true;

        }


        private void chkAnPro_CheckedChanged(object sender, EventArgs e)
        {
            chkAnPro.Checked = true;
        }
        private void faturaBilgileriniDuzenle_Click(object sender, EventArgs e)
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            int id =(int)dataGridView1.CurrentRow.Cells[0].Value;

            var user = crm.Users.FirstOrDefault(x => x.UserID == id);

            if (user != null)
            {
                AdminViews.formFaturaGiris frm = new formFaturaGiris();
                frm.Tag = user;
                frm.ShowDialog();

            }


        }

        private void comboILCE_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
