using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;

namespace DirectFNCRM.AdminViews
{
    //bu sınıf akbankCrm kodları
    public partial class formOlayGoruntule : Form
    {
        public formOlayGoruntule()
        {
            InitializeComponent();
            gridLisanDurumOzet.Rows.Add(9);
        }

        public bool hesaplama = false;
        public bool IptalSorgu = false;

        private void formOlayGoruntule_Load(object sender, EventArgs e)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();


                comboEventType.DataSource = crm.EventTypes;
                comboEventType.DisplayMember = "EventName";
                comboEventType.ValueMember = "EventTypeId";

                comboDepartman.DataSource = crm.Departmans;
                comboDepartman.DisplayMember = "DepartmanAdi";
                comboDepartman.ValueMember = "DepartmanId";

                var sorgu = crm.Calisans.OrderBy(x => x.Ad).Select(x => new { x.calisanID, adsoyad = x.Ad + " " + x.Soyad });
                comboCalisan.DataSource = sorgu;
                comboCalisan.DisplayMember = "adsoyad";
                comboCalisan.ValueMember = "calisanID";

                // griddoldur();

                comboEventType.SelectedIndex = -1;
                comboDepartman.SelectedIndex = -1;
                comboCalisan.SelectedIndex = -1;

                ara();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        public void griddoldur()
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                var sonuc = crm.UserEvents.OrderByDescending(x => x.EventTarih).Select(x => new { x.EventId, x.EventTarih, x.User.UserName, islemiYapan = x.Calisan.Ad + "" + x.Calisan.Soyad, x.EventType.EventName, x.AcilanLisans, x.KapatilanLisans, x.LisansDurum });
                if (sonuc != null)
                    dataGridView1.DataSource = sonuc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void ara()
        {
            try
            {

                crmDFNDataContext crm = new crmDFNDataContext();
                var sorgu = crm.UserEvents.Where(x => x.EventId != 0);

                if (IptalSorgu)
                    sorgu = crm.UserEvents.Where(x => (!x.User.UserName.StartsWith("meksaweb-")) && (!x.User.UserName.StartsWith("investazweb-")) && (!x.User.UserName.StartsWith("alanweb-")));

                if (txtUserName.Text != "")
                {
                    sorgu = sorgu.Where(x => x.User.UserName.Contains(txtUserName.Text));
                }

                if (txtMusteriNo.Text != "")
                {
                    sorgu = sorgu.Where(x => x.User.PmtsNo.Contains(txtMusteriNo.Text));
                }

                if (txtAdi.Text != "")
                {
                    sorgu = sorgu.Where(x => x.User.Name.Contains(txtAdi.Text));
                }
                if (txtSurname.Text != "")
                {
                    sorgu = sorgu.Where(x => x.User.Surname.Contains(txtSurname.Text));
                }
                if (txtAciklama.Text != "")
                {
                    sorgu = sorgu.Where(x => x.User.Aciklama.Contains(txtAciklama.Text));
                }

                if (comboEventType.SelectedIndex != -1)
                {
                    if (comboEventType.SelectedValue is EventType)
                        sorgu = sorgu.Where(x => x.EventType == comboEventType.SelectedValue);
                    else
                        sorgu = sorgu.Where(x => x.EventTypeId == (int)comboEventType.SelectedValue);
                }


                if (comboCalisan.SelectedIndex != -1)
                {
                    if (comboCalisan.SelectedValue is Calisan)
                    { }
                    else if ((comboCalisan.SelectedValue is Int32))
                        sorgu = sorgu.Where(x => x.CalisanId == (int)comboCalisan.SelectedValue);
                }





                if (comboEkranType.SelectedIndex == 0)
                    sorgu = sorgu.Where(x => x.User.LisansDurum.ProYetki == true && x.User.LisansDurum.CepYetki == false || x.User.LisansDurum.SCMUsable == true);
                else if (comboEkranType.SelectedIndex == 1)
                    sorgu = sorgu.Where(x => x.User.LisansDurum.CepYetki == true && x.User.LisansDurum.ProYetki == false);
                else if (comboEkranType.SelectedIndex == 2)
                    sorgu = sorgu.Where(x => x.User.LisansDurum.CepYetki == true && x.User.LisansDurum.ProYetki == true);
                else if (comboEkranType.SelectedIndex == 3)
                    sorgu = sorgu.Where(x => x.User.LisansDurum.ROBOT == true);
                else if (comboEkranType.SelectedIndex == 4)
                    sorgu = sorgu.Where(x => x.User.ProductType == "SCM" && (x.User.LisansDurum.SCMDownload == true || x.User.LisansDurum.SCMRealTıme == true || x.User.LisansDurum.SCMUsable == true));
                else if (comboEkranType.SelectedIndex == 5)
                    sorgu = sorgu.Where(x => x.User.ProductType == "SCM" && x.User.LisansDurum.SCMUsable == true);





                if (comboDepartman.SelectedIndex != -1)
                {
                    if (comboDepartman.SelectedValue is Departman)
                        sorgu = sorgu.Where(x => x.Calisan.Departman == comboDepartman.SelectedValue);
                    else
                        sorgu = sorgu.Where(x => x.Calisan.DepartmanId == (int)comboDepartman.SelectedValue);
                }

                if (dateTimeStartDate.Value.Date != dateTimeEndDate.Value.Date)
                {

                    sorgu = sorgu.Where(x => x.EventTarih.Value.Date >= dateTimeStartDate.Value.Date && x.EventTarih.Value.Date <= dateTimeEndDate.Value.Date);
                }
                else
                {
                    sorgu = sorgu.Where(x => x.EventTarih.Value.Date == dateTimeStartDate.Value.Date);
                }



                if (hesaplama)
                {

                    var Cep = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true).Count();
                    var pro = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ProYetki == true).Count();
                    var robot = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ROBOT == true).Count();
                    var ProCep = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CepYetki == true && x.LisansDurum.ProYetki == true).Count();
                    //var PayYuzeysel = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == false && x.LisansDurum.Pd2P == false).Count();
                    var PayPlus = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == false && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                    var PayDerinlik = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == false).Count();
                    var PayDerinlikPlus = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayL1 == true && x.LisansDurum.PayL2 == true && x.LisansDurum.PayLP == true && x.LisansDurum.Pd2P == true).Count();
                    var PayEndeks = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayX == true).Count();
                    var PayGs = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PayGS == true).Count();
                    var pite = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.PITE == true).Count();

                    //var analitik = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.VeriAnalitik == true).Count();

                    //var viopYuzeysel = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == false && x.LisansDurum.Vd2P == false).Count();
                    var viopPlus = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == false && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                    var viopDerinlik = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == false).Count();
                    var viopDerinlikPlus = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopL1 == true && x.LisansDurum.ViopL2 == true && x.LisansDurum.ViopLP == true && x.LisansDurum.Vd2P == true).Count();
                    var viopGS = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.ViopGS == true).Count();

                    //var TahvilYuzeysel = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == false).Count();
                    var TahvilPlus = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == false && x.LisansDurum.TahvilLP == true).Count();
                    var TahvilDerinlik = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.TahvilL1 == true && x.LisansDurum.TahvilL2 == true && x.LisansDurum.TahvilLP == true).Count();
                    var mkk = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.MKK == true).Count();
                    var gkkul = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.GKKUL == true).Count();
                    var idealgo = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.CME == true).Count();
                    var krmd1 = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.COMEX == true).Count();
                    // var analizPro = sorgu.Where(x => x.LisansDurum.YayinDurumu == true && x.LisansDurum.AnPro == true).Count();

                    var On = sorgu.Where(x => x.LisansDurum.YayinDurumu == true).Count();
                    var Off = sorgu.Where(x => x.LisansDurum.YayinDurumu == false).Count();

                    var renk = Color.LightSalmon;

                    //gridLisanDurumOzet.Rows[0].SetValues("Cep", Cep.ToString(), "", "", " ", "");
                    //gridLisanDurumOzet.Rows[1].SetValues("Pro", pro.ToString(), "PayL1+", PayPlus.ToString(), "ViopL1+", viopPlus.ToString());
                    //gridLisanDurumOzet.Rows[2].SetValues("Pro+Cep", ProCep.ToString(), "PayL2", PayDerinlik.ToString(), "ViopL2", viopDerinlik.ToString());
                    //gridLisanDurumOzet.Rows[3].SetValues("Robot", robot.ToString(), "Pd2P", PayDerinlikPlus.ToString(), "Vd2P", viopDerinlikPlus.ToString());
                    //gridLisanDurumOzet.Rows[4].SetValues("", "", "PayX", PayEndeks.ToString(), "ViopGS", viopGS.ToString());
                    //gridLisanDurumOzet.Rows[5].SetValues("", "", "PayGS", PayGs.ToString(), "", "");
                    //gridLisanDurumOzet.Rows[6].SetValues("Açık", On.ToString(), "", "", "THVL1+", TahvilPlus.ToString());
                    //gridLisanDurumOzet.Rows[7].SetValues("Kapalı", Off.ToString(), "", "", "THVL2", TahvilDerinlik.ToString());

                    gridLisanDurumOzet.Rows[0].SetValues("Pro", pro.ToString(), "", "", "", "");
                    gridLisanDurumOzet.Rows[1].SetValues("Cep", Cep.ToString(), "PD1P", PayPlus.ToString(), "VD1P", viopPlus.ToString());
                    gridLisanDurumOzet.Rows[2].SetValues("Pro+Cep", ProCep.ToString(), "PD2", PayDerinlik.ToString(), "VD2", viopDerinlik.ToString());
                    gridLisanDurumOzet.Rows[3].SetValues("", "", "PD2P", PayDerinlik.ToString(), "VD2P", viopDerinlik.ToString());
                    gridLisanDurumOzet.Rows[4].SetValues("Robot", robot.ToString(), "END", PayEndeks.ToString(), "VIT", viopGS.ToString());
                    gridLisanDurumOzet.Rows[5].SetValues("TradeAll Algo", idealgo.ToString(), "PIT", PayGs.ToString(), "", "");
                    gridLisanDurumOzet.Rows[6].SetValues("Açık", On.ToString(), "PITE", pite.ToString(), "BD1P", TahvilPlus.ToString());
                    gridLisanDurumOzet.Rows[7].SetValues("Kapalı", Off.ToString(), "", "", "BD2", TahvilDerinlik.ToString());
                    gridLisanDurumOzet.Rows[8].SetValues("MKK", mkk.ToString(), "GKKUL", gkkul.ToString(), "KRMD1", krmd1.ToString());

                    gridLisanDurumOzet.Rows[6].Cells[1].Selected = true;

                    for (int i = 0; i < 9; i++)
                    {
                        gridLisanDurumOzet.Rows[i].Cells[0].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[2].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[5].Style.BackColor = renk;
                    }

                    hesaplama = false;
                    return;
                }

                var activesorgu = sorgu.ToList();

                lblSayac.Text = activesorgu.Count.ToString();

                //dataGridView1.DataSource = sorgu.OrderByDescending(x => x.EventTarih).Select(x => new
                //{
                //    x.EventId,
                //    x.EventTarih.Value.Date,
                //    x.User.UserName,
                //    islemiYapan = x.Calisan.Ad + " " + x.Calisan.Soyad,
                //    x.HostName,
                //    x.EventType.EventName,
                //    x.AcilanLisans,
                //    x.KapatilanLisans,                   
                //    x.LisansDurum.YayinDurumu,
                //    x.LisansDurum.PayLP,
                //    x.LisansDurum.PayL2,
                //    x.LisansDurum.Pd2P,
                //    x.LisansDurum.PayX,
                //    x.LisansDurum.PayGS,
                //    x.LisansDurum.ViopLP,
                //    x.LisansDurum.ViopL2,
                //    x.LisansDurum.Vd2P,
                //    x.LisansDurum.ViopGS,
                //    x.LisansDurum.TahvilLP,
                //    x.LisansDurum.TahvilL2,
                //    x.LisansDurum.MKK,
                //    x.LisansDurum.CME,
                //    x.LisansDurum.ROBOT,
                //    x.LisansDurum.COMEX,

                //});

                dataGridView1.DataSource =
                sorgu
                .OrderByDescending(x => x.EventTarih)
                .Select(x => new
                {
                    x.EventId,
                    Tarih = x.EventTarih,
                    x.User.UserName,
                    islemiYapan = x.Calisan.Ad + " " + x.Calisan.Soyad,
                    x.HostName,
                    EventName = x.EventType.EventName,
                    x.AcilanLisans,
                    x.KapatilanLisans,

                    YayinDurumu = (bool?)x.LisansDurum.YayinDurumu ?? false,
                    PayLP = (bool?)x.LisansDurum.PayLP ?? false,
                    PayL2 = (bool?)x.LisansDurum.PayL2 ?? false,
                    Pd2P = (bool?)x.LisansDurum.Pd2P ?? false,
                    PayX = (bool?)x.LisansDurum.PayX ?? false,
                    PayGS = (bool?)x.LisansDurum.PayGS ?? false,
                    ViopLP = (bool?)x.LisansDurum.ViopLP ?? false,
                    ViopL2 = (bool?)x.LisansDurum.ViopL2 ?? false,
                    Vd2P = (bool?)x.LisansDurum.Vd2P ?? false,
                    ViopGS = (bool?)x.LisansDurum.ViopGS ?? false,
                    TahvilLP = (bool?)x.LisansDurum.TahvilLP ?? false,
                    TahvilL2 = (bool?)x.LisansDurum.TahvilL2 ?? false,
                    MKK = (bool?)x.LisansDurum.MKK ?? false,
                    GKKUL = (bool?)x.LisansDurum.GKKUL ?? false,
                    CME = (bool?)x.LisansDurum.CME ?? false,
                    ROBOT = (bool?)x.LisansDurum.ROBOT ?? false,
                    COMEX = (bool?)x.LisansDurum.COMEX ?? false,
                })
                .ToList();
                IptalSorgu = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOtarihte_Click(object sender, EventArgs e)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();

                var otarihdekiler = crm.UserEvents.Where(x => x.EventTarih.Value.Date <= dateTimeDurum.Value.Date && x.User.StatusId == 1).ToList();
                var sorgu = (from n in otarihdekiler group n by n.User.UserName into g select new { tarih = g.OrderByDescending(x => x.EventTarih).FirstOrDefault() }).Where(x => x.tarih.LisansDurum.YayinDurumu == true).ToList();
                dataGridView1.DataSource = sorgu.Where(x => x.tarih.UserId != null).Select(x => new
                {
                    x.tarih.EventId,
                    x.tarih.EventTarih.Value.Date,
                    x.tarih.User.UserName,
                    islemiYapan = x.tarih.Calisan.Ad + "" + x.tarih.Calisan.Soyad,
                    x.tarih.HostName,
                    x.tarih.EventType.EventName,
                    x.tarih.AcilanLisans,
                    x.tarih.KapatilanLisans,
                    x.tarih.LisansDurum.YayinDurumu,
                    // x.tarih.LisansDurum.PayL1,
                    x.tarih.LisansDurum.PayLP,
                    x.tarih.LisansDurum.PayL2,
                    x.tarih.LisansDurum.Pd2P,
                    x.tarih.LisansDurum.PayX,
                    x.tarih.LisansDurum.PayGS,
                    x.tarih.LisansDurum.VeriAnalitik,
                    // x.tarih.LisansDurum.ViopL1,
                    x.tarih.LisansDurum.ViopLP,
                    x.tarih.LisansDurum.ViopL2,
                    x.tarih.LisansDurum.Vd2P,
                    x.tarih.LisansDurum.ViopGS,
                    x.tarih.LisansDurum.TahvilL1,
                    x.tarih.LisansDurum.TahvilLP,
                    x.tarih.LisansDurum.TahvilL2,
                    x.tarih.LisansDurum.MKK,
                    x.tarih.LisansDurum.GKKUL,
                    x.tarih.LisansDurum.CME,
                    x.tarih.LisansDurum.ROBOT,
                    x.tarih.LisansDurum.COMEX


                }).ToList();

                lblSayac.Text = sorgu.Count.ToString();

                var Cep = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.CepYetki == true).Count();
                var pro = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.ProYetki == true).Count();
                var robot = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.ROBOT == true).Count();
                var mkk = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.MKK == true).Count();
                var gkkul = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.GKKUL == true).Count();
                var ProCep = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.CepYetki == true && x.tarih.LisansDurum.ProYetki == true).Count();
                // var PayYuzeysel = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.PayL1 == true && x.tarih.LisansDurum.PayL2 == false && x.tarih.LisansDurum.PayLP == false && x.tarih.LisansDurum.Pd2P == false).Count();
                var PayPlus = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.PayL1 == true && x.tarih.LisansDurum.PayL2 == false && x.tarih.LisansDurum.PayLP == true && x.tarih.LisansDurum.Pd2P == false).Count();
                var PayDerinlik = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.PayL1 == true && x.tarih.LisansDurum.PayL2 == true && x.tarih.LisansDurum.PayLP == true && x.tarih.LisansDurum.Pd2P == false).Count();
                var PayDerinlikPlus = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.PayL1 == true && x.tarih.LisansDurum.PayL2 == true && x.tarih.LisansDurum.PayLP == true && x.tarih.LisansDurum.Pd2P == true).Count();
                var PayEndeks = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.PayX == true).Count();
                var PayGs = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.PayGS == true).Count();

                // var analitik = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.VeriAnalitik == true).Count();

                // var viopYuzeysel = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.ViopL1 == true && x.tarih.LisansDurum.ViopL2 == false && x.tarih.LisansDurum.ViopLP == false && x.tarih.LisansDurum.Vd2P == false).Count();
                var viopPlus = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.ViopL1 == true && x.tarih.LisansDurum.ViopL2 == false && x.tarih.LisansDurum.ViopLP == true && x.tarih.LisansDurum.Vd2P == false).Count();
                var viopDerinlik = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.ViopL1 == true && x.tarih.LisansDurum.ViopL2 == true && x.tarih.LisansDurum.ViopLP == true && x.tarih.LisansDurum.Vd2P == false).Count();
                var viopDerinlikPlus = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.ViopL1 == true && x.tarih.LisansDurum.ViopL2 == true && x.tarih.LisansDurum.ViopLP == true && x.tarih.LisansDurum.Vd2P == true).Count();
                var viopGS = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.ViopGS == true).Count();

                // var TahvilYuzeysel = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.TahvilL1 == true && x.tarih.LisansDurum.TahvilL2 == false && x.tarih.LisansDurum.TahvilLP == false).Count();
                var TahvilPlus = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.TahvilL1 == true && x.tarih.LisansDurum.TahvilL2 == false && x.tarih.LisansDurum.TahvilLP == true).Count();
                var TahvilDerinlik = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.TahvilL1 == true && x.tarih.LisansDurum.TahvilL2 == true && x.tarih.LisansDurum.TahvilLP == true).Count();
                var cme = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.CME == true).Count();
                var comex = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true && x.tarih.LisansDurum.COMEX == true).Count();

                var On = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == true).Count();
                var Off = sorgu.Where(x => x.tarih.LisansDurum.YayinDurumu == false).Count();


                var renk = Color.LightSalmon;

                gridLisanDurumOzet.Rows[0].SetValues("Pro", pro.ToString(), "", "", "", "");
                gridLisanDurumOzet.Rows[1].SetValues("Cep", Cep.ToString(), "PayL1+", PayPlus.ToString(), "ViopL1+", viopPlus.ToString());
                gridLisanDurumOzet.Rows[2].SetValues("Pro+Cep", ProCep.ToString(), "PayL2", PayDerinlik.ToString(), "ViopL2", viopDerinlik.ToString());
                gridLisanDurumOzet.Rows[3].SetValues("", "", "PD2P", PayDerinlik.ToString(), "VD2P", viopDerinlik.ToString());
                gridLisanDurumOzet.Rows[4].SetValues("Robot", robot.ToString(), "PayX", PayEndeks.ToString(), "ViopGS", viopGS.ToString());
                gridLisanDurumOzet.Rows[5].SetValues("", "", "PayGS", PayGs.ToString(), "", "");
                gridLisanDurumOzet.Rows[6].SetValues("Açık", On.ToString(), "", "", "THVL1+", TahvilPlus.ToString());
                gridLisanDurumOzet.Rows[7].SetValues("Kapalı", Off.ToString(), "ALG", cme.ToString(), "THVL2", TahvilDerinlik.ToString());
                gridLisanDurumOzet.Rows[8].SetValues("MKK", mkk.ToString(), "GKKUL", gkkul.ToString(), "KRMD1", comex.ToString());

                gridLisanDurumOzet.Rows[6].Cells[1].Selected = true;

                for (int i = 0; i < 8; i++)
                {
                    gridLisanDurumOzet.Rows[i].Cells[0].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[2].Style.BackColor = gridLisanDurumOzet.Rows[i].Cells[5].Style.BackColor = renk;
                }


            }
            catch (Exception ex)
            {


                MessageBox.Show(ex.Message);
            }
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            if (txtUserName.Text != "")
                ara();
        }

        private void txtAdi_TextChanged(object sender, EventArgs e)
        {
            if (txtAdi.Text != "")
                ara();
        }

        private void txtSurname_TextChanged(object sender, EventArgs e)
        {
            if (txtSurname.Text != "")
                ara();
        }

        private void txtAciklama_TextChanged(object sender, EventArgs e)
        {
            if (txtAciklama.Text != "")
                ara();
        }

        private void comboDepartman_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboDepartman.SelectedIndex != -1)
                ara();
        }
        private void comboEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboEventType.SelectedIndex != -1)
                ara();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {


                txtAciklama.Text = string.Empty;
                txtAdi.Text = string.Empty;
                txtSurname.Text = string.Empty;
                txtUserName.Text = string.Empty;
                txtMusteriNo.Text = string.Empty;


                comboDepartman.SelectedIndex = -1;
                comboEventType.SelectedIndex = -1;
                comboCalisan.SelectedIndex = -1;
                comboEkranType.SelectedIndex = -1;
                ara();




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btn2TarihAra_Click(object sender, EventArgs e)
        {
            ara();
        }

        private void comboCalisan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboCalisan.SelectedIndex != -1)
                ara();
        }

        private void btnLisansHesapla_Click(object sender, EventArgs e)
        {


        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
                return;

            var username = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            crmDFNDataContext crm = new crmDFNDataContext();

            var user = crm.Users.FirstOrDefault(x => x.UserName == username);

            if (user == null)
                return;

            formUserDetay frm = new formUserDetay();
            frm.Tag = user.UserID;
            frm.ShowDialog();

        }

        private void txtMusteriNo_TextChanged(object sender, EventArgs e)
        {
            if (txtMusteriNo.Text != "")
                ara();
        }

        private void btnIptalEdilenler_Click(object sender, EventArgs e)
        {
            IptalSorgu = true;
            ara();

        }

        private void comboEkranType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboEkranType.SelectedIndex != -1)
                ara();

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void exceleAktarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1._CopyToExcel(Application.StartupPath + "\\Olaylar.xlsx");
        }
        private void btnTarihindekiExcelRapor_Click(object sender, EventArgs e)
        {
            MyTools.ExcelRutin.OlaylarTarihRapor(dateTimeDurum.Value.Date);
        }

        private void OlayDetayMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count < 1)
                    return;

                var id = dataGridView1.CurrentRow.Cells[0].Value.ToString()._ToInt();
                if (id == 0) return;


                crmDFNDataContext crm = new crmDFNDataContext();

                var userevent = crm.UserEvents.FirstOrDefault(x => x.EventId == id);
                if (userevent == null)
                    return;

                formOlayDetay frm = new formOlayDetay();
                frm.Tag = userevent;
                frm.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
