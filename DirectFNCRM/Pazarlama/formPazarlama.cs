using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace DirectFNCRM.Pazarlama
{
    public partial class formPazarlama : Form
    {
        public formPazarlama()
        {
            InitializeComponent();
            gridLisanDurumOzet.Rows.Add(8);
            referance = this;
        }

        delegate void UpdateGridThreadHandler(IQueryable<User> Sorgu);
        public bool acilis = false;
        public Calisan ActiveCalisan;
        public static formPazarlama referance;
        public static List<User> DataList = new List<User>();
        public static IQueryable<User> ActiveSorgu;
        public bool toplamguncelle = false;
        public string HostName = "";
        public string IpAdress = "";
        public delegate void ListBoxTransactionGuncelle(string Text);
        public delegate void gridviewOzetGuncelle(string Text);

        private void formPazarlama_Load(object sender, EventArgs e)
        {
            try
            {


                this.dataGridView1.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView1_RowPrePaint);
                ActiveCalisan = (Calisan)this.Tag;
                this.Text += " - " + ActiveCalisan.Ad + " " + ActiveCalisan.Soyad;

                #region LocalNetworkInfo

                HostName = Dns.GetHostName();
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
                #endregion

                IPport.Config("InBufferSize=10000000");
                IPport.Config("OutBufferSize=10000000");
                IPport.Config("MaxLineLength=50000");


                StringBuilder okunan = new StringBuilder(100);
                MyTools.GetPrivateProfileString("Connectiion", "ServerIP", "", okunan, 100, System.Windows.Forms.Application.StartupPath + @"\ClientAyarlar.ini");
                if (okunan.ToString() == "")
                {
                    IPport.RemoteHost = "10.5.5.52";

                }
                else
                    IPport.RemoteHost = okunan.ToString().Trim();

                MyTools.GetPrivateProfileString("Connectiion", "ServerPort", "", okunan, 100, System.Windows.Forms.Application.StartupPath + @"\ClientAyarlar.ini");

                if (okunan.ToString() == "")
                {
                    IPport.RemotePort = 4450;

                }
                else
                    IPport.RemotePort = Int32.Parse(okunan.ToString().Trim());


                if(ActiveCalisan.DepartmanId==3)
                IPport.Connected = true;
                

                crmDFNDataContext crm = new crmDFNDataContext();
                comboKurum.DataSource = crm.Kurumlars;
                comboKurum.DisplayMember = "KurumAdi";
                comboKurum.ValueMember = "KurumID";
                comboKurum.SelectedIndex = -1;




                toplamguncelle = true;


                comboStatus.DataSource = crm.Status;
                comboStatus.DisplayMember = "StatusAdi";
                comboStatus.ValueMember = "StatusId";
                comboStatus.SelectedIndex = 8;

                ara();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var secili = dataGridView1.Rows[e.RowIndex].Cells[8].Value;
            //dataGridView1.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex+1).ToString();
            if (secili != null)
            {
                if ((bool)secili)
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                else
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;

            }
        }

        private void formPazarlama_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ActiveCalisan.DepartmanId == 3)
                System.Windows.Forms.Application.Exit();
        }

        public void gridGuncelle()
        {
            ara();

        }
        private void ara()
        {
            try
            {
                if (acilis)
                {
                    crmDFNDataContext crm = new crmDFNDataContext();
                    var sorgu = crm.Users.Where(x => x.UserName != null);

                    if (txtUserName.Text != "")
                    {


                        sorgu = sorgu.Where(x => x.UserName.Contains(txtUserName.Text)
                        || x.Name.Contains(txtUserName.Text)
                        || x.Surname.Contains(txtUserName.Text)
                        || x.Aciklama.Contains(txtUserName.Text)
                          || x.Iletisim.Il.IlAdi.Contains(txtUserName.Text)
                        || x.PmtsNo.Contains(txtUserName.Text));
                    }
                    if (txtAdi.Text != "")
                    {

                        sorgu = sorgu.Where(x => x.UserName.Contains(txtAdi.Text)
                        || x.Name.Contains(txtAdi.Text)
                        || x.Surname.Contains(txtAdi.Text)
                        || x.Aciklama.Contains(txtAdi.Text)
                          || x.Iletisim.Il.IlAdi.Contains(txtAdi.Text)
                        || x.PmtsNo.Contains(txtAdi.Text));
                    }
                    if (txtSurname.Text != "")
                    {

                        sorgu = sorgu.Where(x => x.UserName.Contains(txtSurname.Text)
                        || x.Name.Contains(txtSurname.Text)
                        || x.Surname.Contains(txtSurname.Text)
                        || x.Aciklama.Contains(txtSurname.Text)
                          || x.Iletisim.Il.IlAdi.Contains(txtSurname.Text)
                        || x.PmtsNo.Contains(txtSurname.Text));
                    }

                    //if (comboKurum.SelectedIndex != -1)
                    //{
                    //    if (comboKurum.SelectedValue is Kurumlar)
                    //        sorgu = sorgu.Where(x => x.Kurumlar == comboKurum.SelectedValue);
                    //    else
                    //        sorgu = sorgu.Where(x => x.KurumID == (int)comboKurum.SelectedValue);
                    //}

                    if (comboYayinDurum.SelectedIndex != -1)
                    {
                        if (comboYayinDurum.SelectedIndex == 0)
                            sorgu = sorgu.Where(x => x.LisansDurum.YayinDurumu == true);
                        else if (comboYayinDurum.SelectedIndex == 1)
                            sorgu = sorgu.Where(x => x.LisansDurum.YayinDurumu == false);
                    }


                    if (comboEkranType.SelectedIndex == 0)
                        sorgu = sorgu.Where(x => x.LisansDurum.ProYetki == true && x.LisansDurum.CepYetki == false || x.LisansDurum.SCMUsable == true);
                    else if (comboEkranType.SelectedIndex == 1)
                        sorgu = sorgu.Where(x => x.LisansDurum.CepYetki == true && x.LisansDurum.ProYetki == false);
                    else if (comboEkranType.SelectedIndex == 2)
                        sorgu = sorgu.Where(x => x.LisansDurum.CepYetki == true && x.LisansDurum.ProYetki == true);
                    else if (comboEkranType.SelectedIndex == 3)
                        sorgu = sorgu.Where(x => x.LisansDurum.ROBOT == true);
                    else if (comboEkranType.SelectedIndex == 4)
                        sorgu = sorgu.Where(x => x.ProductType == "SCM" && (x.LisansDurum.SCMDownload == true || x.LisansDurum.SCMRealTıme == true || x.LisansDurum.SCMUsable == true));
                    else if (comboEkranType.SelectedIndex == 5)
                        sorgu = sorgu.Where(x => x.ProductType == "SCM" && x.LisansDurum.SCMUsable == true);



                    if (comboStatus.SelectedIndex != -1)
                    {
                        var a = comboStatus.SelectedIndex;

                        if (a == 8)
                        {


                        }
                        else
                        {
                            sorgu = sorgu.Where(x => x.StatusId == a + 1);
                        }

                    }




                    DataList = sorgu.ToList();
                    ActiveSorgu = sorgu;


                    

                    dataGridView1.DataSource = sorgu.OrderByDescending(x => x.UserID).Select(x => new
                    {
                        x.PmtsNo,
                        x.UserName,
                        Adı_Soyadı = x.Name + " " + x.Surname,
                        x.Aciklama,
                        Start_Date = x.BaslangicTarihi.Value.Date,
                        x.ExpiryDate,
                        x.Iletisim.Tel1,
                        x.LisansDurum.YayinDurumu,
                        x.UserID
                    });
                }
                else acilis = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ToplamShow()
        {

            try
            {

                #region Toplamlar


                // if (toplamguncelle)
                //  {
                var Cep = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true).Count();
                var pro = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ProYetki == true).Count();
                var robot = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ROBOT == true).Count();
                var ProCep = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true && x.LisansDurum.ProYetki == true).Count();
                var PayYuzeysel = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false).Count();
                var PayPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == true).Count();
                var PayDerinlik = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P== false).Count();
                var PayDerinlikPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P== true).Count();
                var PayEndeks = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayX == true).Count();
                var PayGs = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayGS == true).Count();

                var analitik = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.VeriAnalitik == true).Count();

                var viopYuzeysel = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false).Count();
                var viopPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == true).Count();
                var viopDerinlik = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P== false).Count();
                var viopDerinlikPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P== true).Count();
                var viopGS = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();

                var TahvilYuzeysel = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false).Count();
                var TahvilPlus = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == true).Count();
                var TahvilDerinlik = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == true && x.LisansDurum.TahvilLP == true).Count();

                var AnalizPro = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.AnPro == true).Count();

                var On = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == true).Count();
                var Off = ActiveSorgu.Where(x => x.LisansDurum.YayinDurumu == false).Count();


                var renk = Color.LightSalmon;

                gridLisanDurumOzet.Rows[0].SetValues("Pro", pro.ToString(), "PD1", PayYuzeysel.ToString(), "VD1", viopYuzeysel.ToString());
                gridLisanDurumOzet.Rows[1].SetValues("Cep", Cep.ToString(), "PD1P", PayPlus.ToString(), "VD1P", viopPlus.ToString());
                gridLisanDurumOzet.Rows[2].SetValues("Pro+Cep", ProCep.ToString(), "PD2", PayDerinlik.ToString(), "VD2", viopDerinlik.ToString());
                gridLisanDurumOzet.Rows[3].SetValues("Robot", robot.ToString(), "PD2P", PayDerinlikPlus.ToString(), "VD2P", viopDerinlikPlus.ToString());
                gridLisanDurumOzet.Rows[4].SetValues("AnalizPro", AnalizPro.ToString(), "END", PayEndeks.ToString(), "VIT", viopGS.ToString());
                gridLisanDurumOzet.Rows[5].SetValues("", "", "PIT", PayGs.ToString(), "BD1", TahvilYuzeysel.ToString());
                gridLisanDurumOzet.Rows[6].SetValues("Açık", On.ToString(), "PVA", analitik, "BD1P", TahvilPlus.ToString());
                gridLisanDurumOzet.Rows[7].SetValues("Kapalı", Off.ToString(), "", "", "BD2", TahvilDerinlik.ToString());

                gridLisanDurumOzet.Rows[5].Cells[1].Selected = true;

                for (int i = 0; i < 8; i++)
                {
                    gridLisanDurumOzet.Rows[i].Cells[0].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[2].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[4].Style.BackColor = renk;
                }


                toplamguncelle = false;

                //  }


                #endregion


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void GridUpdateHesaplama(IQueryable<User> sorgu)
        {

            if (gridLisanDurumOzet.InvokeRequired)
            {

                UpdateGridThreadHandler upgrid = new UpdateGridThreadHandler(GridUpdateHesaplama);
                this.Invoke(upgrid, new object[] { sorgu });

            }
            else
            {

                if (toplamguncelle)
                {

                    var Cep = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true).Count();
                    var pro = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ProYetki == true).Count();
                    var robot = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ROBOT == true).Count();
                    var ProCep = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true && x.LisansDurum.ProYetki == true).Count();
                    var PayYuzeysel = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false).Count();
                    var PayPlus = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == true).Count();
                    var PayDerinlik = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P== false).Count();
                    var PayDerinlikPlus = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P== true).Count();
                    var PayEndeks = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayX == true).Count();
                    var PayGs = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayGS == true).Count();

                    var analitik = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.VeriAnalitik == true).Count();

                    var viopYuzeysel = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false).Count();
                    var viopPlus = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == true).Count();
                    var viopDerinlik = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P== false).Count();
                    var viopDerinlikPlus = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P== true ).Count();
                    var viopGS = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();

                    var TahvilYuzeysel = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false).Count();
                    var TahvilPlus = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == true).Count();
                    var TahvilDerinlik = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == true && x.LisansDurum.TahvilLP == true).Count();

                    var AnalizPro = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.AnPro == true).Count();

                    var On = sorgu.Where(x => x.LisansDurum.YayinDurumu == true).Count();
                    var Off = sorgu.Where(x => x.LisansDurum.YayinDurumu == false).Count();


                    var renk = Color.LightSalmon;

                    gridLisanDurumOzet.Rows[0].SetValues("Pro", pro.ToString(), "PayL1", PayYuzeysel.ToString(), "ViopL1", viopYuzeysel.ToString());
                    gridLisanDurumOzet.Rows[1].SetValues("Cep", Cep.ToString(), "PayL1+", PayPlus.ToString(), "ViopL1+", viopPlus.ToString());
                    gridLisanDurumOzet.Rows[2].SetValues("Pro+Cep", ProCep.ToString(), "PayL2", PayDerinlik.ToString(), "ViopL2", viopDerinlik.ToString());
                    gridLisanDurumOzet.Rows[3].SetValues("Robot", robot.ToString(), "PD2P", PayDerinlikPlus.ToString(), "Vd2P", viopDerinlikPlus.ToString());
                    gridLisanDurumOzet.Rows[4].SetValues("AnalizPro",AnalizPro.ToString(), "PayX", PayEndeks.ToString(), "ViopGS", viopGS.ToString());
                    gridLisanDurumOzet.Rows[5].SetValues("", "", "PayGS", PayGs.ToString(), "THVL1", TahvilYuzeysel.ToString());
                    gridLisanDurumOzet.Rows[6].SetValues("Açık", On.ToString(), "VeriAnalitik", analitik, "THVL1+", TahvilPlus.ToString());
                    gridLisanDurumOzet.Rows[7].SetValues("Kapalı", Off.ToString(), "", "", "THVL2", TahvilDerinlik.ToString());

                    gridLisanDurumOzet.Rows[5].Cells[1].Selected = true;

                    for (int i = 0; i < 8; i++)
                    {
                        gridLisanDurumOzet.Rows[i].Cells[0].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[2].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[4].Style.BackColor = renk;
                    }


                    toplamguncelle = false;

                }

            }

        }
        public void GridAnaUpdate(IQueryable<User> sorgu)
        {

            if (dataGridView1.InvokeRequired)
            {

                UpdateGridThreadHandler upgrid = new UpdateGridThreadHandler(GridAnaUpdate);
                this.Invoke(upgrid, new object[] { sorgu });

            }
            else
            {

                DataList = sorgu.ToList();


                GridUpdateHesaplama(sorgu);



                dataGridView1.DataSource = sorgu.OrderByDescending(x => x.UserID).Select(x => new
                {
                    x.UserID,
                    x.UserName,
                    Adı_Soyadı = x.Name + " " + x.Surname,
                    x.BaslangicTarihi.Value.Date,
                    x.ExpiryDate,
                    x.PmtsNo,                
                    x.MusteriMensei.MenseiAdi,
                    x.Iletisim.Il.IlAdi,
                    x.Aciklama,
                    x.LisansDurum.YayinDurumu
                });





            }

        }



        private void ServerGuncelemetoForm()
        {
            crmDFNDataContext crm = new crmDFNDataContext();
            var sorgu = crm.Users.Where(x => x.UserName != null);

            GridAnaUpdate(sorgu);
        }



        public void ListBoxTransactionEkle(string text)
        {
            try
            {

                if (listBoxTransaction.InvokeRequired)
                {
                    ListBoxTransactionGuncelle list = new ListBoxTransactionGuncelle(ListBoxTransactionEkle);
                    this.Invoke(list, new object[] { text });
                }
                else
                {
                    if (listBoxTransaction.Items.Count > 100)
                        listBoxTransaction.Items.RemoveAt(listBoxTransaction.Items.Count - 1);
                    listBoxTransaction.Items.Insert(0, DateTime.Now.ToString() + " : " + text);
                    MyTools.logyaz(Text);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void comboKurum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboKurum.SelectedIndex != -1)
                ara();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {

                txtUserName.Text = string.Empty;
                txtAdi.Text = string.Empty;
                txtSurname.Text = string.Empty;
                txtSurname.Text = string.Empty;
                comboKurum.SelectedIndex = -1;
                comboYayinDurum.SelectedIndex = -1;
                comboEkranType.SelectedIndex = -1;

                ara();
                ToplamShow();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboYayinDurum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboYayinDurum.SelectedIndex != -1)
                ara();
        }

        private void comboEkranType_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboEkranType.SelectedIndex != -1)
                ara();
        }

        private void IPport_OnConnected(object sender, nsoftware.IPWorks.IpportConnectedEventArgs e)
        {
            if (e.StatusCode == 0)
            {

                panelBaglanti.BackColor = Color.LimeGreen;
                if (ActiveCalisan.DepartmanId == 3)
                    IPport.DataToSend = "Connect|" + ActiveCalisan.Ad + " " + ActiveCalisan.Soyad + (char)3;


            }
            else
            {
                panelBaglanti.BackColor = Color.Red;


                AdminViews.formBaglantiKotrol frm = new AdminViews.formBaglantiKotrol();
                frm.lblMesaj.Text = "Server Makinaya Bağlantınız Kurulamadı\nlütfen Sistem Yöneticinize başvurun";
                frm.ShowIcon = false;
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.TopMost = true;
                frm.ShowDialog();


            }
        }

        public void baglantiyok(object s, EventArgs e)
        {

            AdminViews.formBaglantiKotrol frm = new AdminViews.formBaglantiKotrol();
            frm.lblMesaj.Text = "Server Makina ile Bağlantınız Koptu\n Lütfen sistem yöneticinize başvurunuz";
            frm.ShowDialog();

        }

        private void IPport_OnDisconnected(object sender, nsoftware.IPWorks.IpportDisconnectedEventArgs e)
        {
            try
            {
                this.panelBaglanti.BackColor = Color.Red;
                this.Invoke(new EventHandler(baglantiyok));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void IPport_OnDataIn(object sender, nsoftware.IPWorks.IpportDataInEventArgs e)
        {

            try
            {
                var data = e.Text.Split((char)3);

                foreach (var d in data)
                {
                    if (d.StartsWith("CreateUser"))
                    {
                        var satir = d.Split('|');
                        ListBoxTransactionEkle(satir[1]);
                        toplamguncelle = true;
                        ServerGuncelemetoForm();
                    }
                    else if (d.StartsWith("Estrore_ChangeUser"))
                    {
                        var satir = d.Split('|');
                        ListBoxTransactionEkle(satir[1]);
                        toplamguncelle = true;
                        ServerGuncelemetoForm();
                    }
                    else if (d.StartsWith("Estrore_CreateUser"))
                    {
                        var satir = d.Split('|');
                        ListBoxTransactionEkle(satir[1]);
                        toplamguncelle = true;
                        ServerGuncelemetoForm();
                    }
                    else if (d.StartsWith("OYAK_AUTO_CREATE"))
                    {
                        var satir = d.Split('|');
                        ListBoxTransactionEkle(satir[1]);
                        toplamguncelle = true;
                        ServerGuncelemetoForm();
                    }




                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void comboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ara();
            ToplamShow();
        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                if (txtUserName.Text != null)
                    ara();

            }
        }

        private void txtAdi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtAdi.Text != null)
                    ara();

            }
        }

        private void txtSurname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtSurname.Text != null)
                    ara();

            }
        }
    }
}
