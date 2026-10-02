using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DirectFNCRM.AdminViews
{
    public partial class formEkstraAra : Form
    {
        public formEkstraAra()
        {
            InitializeComponent();
        }

        public int searchstate = 0;
        public static List<User> DataList = new List<User>();

        private void formEkstraAra_Load(object sender, EventArgs e)
        {
            searchstate = (int)this.Tag;

            crmDFNDataContext crm = new crmDFNDataContext();

            var sorgu = crm.Calisans.Where(x => x.CalisanTipId == 1).OrderBy(x => x.Ad).Select(x => new { x.calisanID, adsoyad = x.Ad + " " + x.Soyad });
            comboCalisan.DataSource = sorgu;
            comboCalisan.DisplayMember = "adsoyad";
            comboCalisan.ValueMember = "calisanID";


            comboDepartman.DataSource = crm.Departmans;
            comboDepartman.DisplayMember = "DepartmanAdi";
            comboDepartman.ValueMember = "DepartmanId";

            comboCalisan.SelectedIndex = -1;
            comboDepartman.SelectedIndex = -1;

            if(searchstate==4)
            {

                panelSonKullanim.Location = new Point(320,6);
                panelSonKullanim.Visible = true;

                panelTopluExpiryDate.Location = new Point(18, 34);
                if(formAdminAra.referance.ActiveCalisan.calisanID<3 && formAdminAra.referance.ActiveCalisan.calisanID>0)
                panelTopluExpiryDate.Visible = true;

                chkStatusClose.Visible = true;

            }
            else if (searchstate == 1)
            {
                groupBox1.Visible = true;
                dateTimeStartDate.Visible = true;
                dateTimeEndDate1.Visible = true;
                txtAra.Visible = true;
            }
            else
            {
                chkStatusClose.Visible = false;
            }
            ara();
        }

        public void ara()
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                #region menu1
                if (searchstate == 1)
                {
                    var srg = crm.LoginHistories.Where(x => x.username != "" && x.LoginTime != null);
                    if (txtkriter1.Text != "")
                    {
                        srg = srg.Where(x => x.username.Contains(txtkriter1.Text)|| x.Name.Contains(txtkriter1.Text) || x.Surname.Contains(txtkriter1.Text) || x.PmtsNo.Contains(txtkriter1.Text));
                    }
                    if (txtkriter2.Text != "")
                    {
                        srg = srg.Where(x => x.username.Contains(txtkriter2.Text) || x.Name.Contains(txtkriter2.Text) || x.Surname.Contains(txtkriter2.Text) || x.PmtsNo.Contains(txtkriter2.Text));
                    }
                    if (txtkriter3.Text != "")
                    {
                        srg = srg.Where(x => x.username.Contains(txtkriter3.Text) || x.Name.Contains(txtkriter3.Text) || x.Surname.Contains(txtkriter3.Text) || x.PmtsNo.Contains(txtkriter3.Text));
                    }
                    if (dateTimeStartDate.Value.Date != dateTimeEndDate1.Value.Date)
                    {

                        srg = srg.Where(x => x.LoginTime.Date >= dateTimeStartDate.Value.Date && x.LoginTime.Date <= dateTimeEndDate1.Value.Date);
                    }
                    else
                    {
                        srg = srg.Where(x => x.LoginTime.Date == dateTimeStartDate.Value.Date);
                    }
                    var dt = srg.ToList();
                    lblSayac.Text = dt.Count.ToString();

                    dataGridView1.DataSource = dt.OrderByDescending(x => x.LoginTime).Select(x => new {MusteriNumarası=x.PmtsNo, AdSoayd=x.Name+" "+ x.Surname , UserName = x.username, x.LoginTime }).ToList();

                }

                #endregion
                #region menu4

                if (searchstate == 4)
                {
                   // lblAciklama.Text = "Vergi Bilgileri veya TC Kimlik No girilmemiş aktif durumdaki müşteriler .";
                    this.Text = "Son Kullanım Tarihi Kontrol Penceresi";
                 
                    btnClear.Visible = true;

                    var sorgu = crm.Users.Where(x => x.ExpiryDate.Value.Date == dateTimeEndDate.Value.Date && x.LisansDurum.YayinDurumu==true);
                    if(chkStatusClose.Checked) sorgu = crm.Users.Where(x => x.ExpiryDate.Value.Date == dateTimeEndDate.Value.Date && x.LisansDurum.YayinDurumu == false);

                    if (chxKurumsalWeb.CheckState == CheckState.Checked)
                    {
                        sorgu = sorgu.Where(x => (!x.UserName.StartsWith("meksaweb-")) && (!x.UserName.StartsWith("investazweb-")) && (!x.UserName.StartsWith("alanweb-")));
                    }


                    if (comboEkranType.SelectedIndex != -1)
                    {

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

                    }


                    if (txtkriter1.Text != "")
                    {
                        sorgu = sorgu.Where(x => x.UserName.Contains(txtkriter1.Text)
                        || x.Name.Contains(txtkriter1.Text)
                        || x.Surname.Contains(txtkriter1.Text)
                        || x.Aciklama.Contains(txtkriter1.Text)
                          || x.Iletisim.Il.IlAdi.Contains(txtkriter1.Text)
                        || x.PmtsNo.Contains(txtkriter1.Text));
                    }
                    if (txtkriter2.Text != "")
                    {

                        sorgu = sorgu.Where(x => x.UserName.Contains(txtkriter2.Text)
                        || x.Name.Contains(txtkriter2.Text)
                        || x.Surname.Contains(txtkriter2.Text)
                        || x.Aciklama.Contains(txtkriter2.Text)
                          || x.Iletisim.Il.IlAdi.Contains(txtkriter2.Text)
                        || x.PmtsNo.Contains(txtkriter2.Text));
                    }
                    if (txtkriter3.Text != "")
                    {

                        sorgu = sorgu.Where(x => x.UserName.Contains(txtkriter3.Text)
                        || x.Name.Contains(txtkriter3.Text)
                        || x.Surname.Contains(txtkriter3.Text)
                        || x.Aciklama.Contains(txtkriter3.Text)
                          || x.Iletisim.Il.IlAdi.Contains(txtkriter3.Text)
                        || x.PmtsNo.Contains(txtkriter3.Text));
                    }


                    DataList = sorgu.ToList();

                    lblSayac.Text = DataList.Count.ToString();


                    dataGridView1.DataSource = sorgu.OrderByDescending(x => x.UserID).Select(x => new
                    {
                        x.PmtsNo,
                        x.UserName,
                        Adı_Soyadı = x.Name + " " + x.Surname,
                        x.Aciklama,
                        ExpiryDate = x.ExpiryDate.Value.Date,
                        x.Iletisim.Tel1,
                        x.LisansDurum.YayinDurumu,
                        x.UserID
                    });
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            crmDFNDataContext crm = new crmDFNDataContext();

            if (searchstate == 4)
            {
                if (dataGridView1.SelectedRows.Count < 1)
                    return;

                var id = dataGridView1.CurrentRow.Cells[7].Value;

                var user = crm.Users.FirstOrDefault(x => x.UserID == (int)id);

                if (user == null)
                    return;

                formUserDetay frm = new formUserDetay();
                frm.Tag = user.UserID;
                frm.Show();
            }
            else if (searchstate == 1)
            {
                if (dataGridView1.SelectedRows.Count < 1)
                    return;

                var usrname = dataGridView1.CurrentRow.Cells[2].Value;

                var user = crm.Users.FirstOrDefault(x => x.UserName == usrname.ToString());


                if (user == null)
                    return;

                formUserDetay frm = new formUserDetay();
                frm.Tag = user.UserID;
                frm.Show();
            }
        }

        private void txtkriter1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtkriter1.Text != null)
                    ara();

            }
        }

        private void txtkriter2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtkriter2.Text != null)
                    ara();
            }
        }

        private void txtkriter3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtkriter3.Text != null)
                    ara();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtkriter1.Text = "";
            txtkriter2.Text = "";
            txtkriter3.Text = "";

            comboCalisan.SelectedIndex = -1;
            comboEkranType.SelectedIndex = -1;
            chxKurumsalWeb.Checked = false;
            ara();
        }

        private void comboCalisan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboCalisan.SelectedIndex != -1)
                ara();
        }

        private void btnTekTarih_Click(object sender, EventArgs e)
        {
            ara();
        }

        private void dateTimeEndDate_ValueChanged(object sender, EventArgs e)
        {
            ara();
        }

        private void chxKurumsalWeb_CheckedChanged(object sender, EventArgs e)
        {
            ara();
        }

        private void comboEkranType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboEkranType.SelectedIndex != -1)
            ara();
        }

        public delegate void controlguncelle(string text);

        public void labelSayacGuncelle(string text)
        {
            try
            {
                if (lblSayc2.InvokeRequired)
                {
                    var cg = new controlguncelle(labelSayacGuncelle);
                    this.Invoke(cg, new object[] { text});
                }
                else
                {
                    lblSayc2.Text = text; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void brnExpriydateAta_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Seçmiş olduğunuz kullanıcıların son kullanım tarihleri değiştirilecektir", "Toplu ExpriyDate Değiştirme İşlemi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {

                var rows = dataGridView1.SelectedRows;


                if(rows.Count>0)
                {


                    var newthread = new Thread(new ThreadStart(() =>
                    {
                        crmDFNDataContext crm = new crmDFNDataContext();
                        
                        var a = 1;
                        foreach (DataGridViewRow r in rows)
                        {
                            Thread.Sleep(300);
                            if (r.Cells[7].Value == null)
                                break;
                            var id = Int32.Parse(r.Cells[7].Value.ToString());
                            var user = crm.Users.FirstOrDefault(x=>x.UserID==id);

                            if (user.LisansDurum.YayinDurumu == false)
                            {
                                UserEvent ue = new UserEvent();
                                ue.UserId = user.UserID;
                                ue.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
                                ue.EventTarih = DateTime.Now;
                                ue.IP = formAdminAra.referance.IpAdress;
                                ue.HostName = formAdminAra.referance.HostName;
                                ue.SonLisandurumID = user.LisansDurumId;
                                ue.EventTypeId = 1;
                                user.LisansDurum.YayinDurumu = true;
                                crm.UserEvents.InsertOnSubmit(ue);
                                crm.SubmitChanges();
                            }
                            user.ExpiryDate=dtExpriyDate.Value;
                            crm.SubmitChanges();

                            formAdminAra.referance.IPport.DataToSend = "ChangeUser|" + user.UserID.ToString() + (char)3;
                            labelSayacGuncelle(a.ToString());
                            a++;
                        }
                        labelSayacGuncelle("Bitti");
                    }));
                    newthread.Start();
                }
            }
        }

        private void txtAra_Click(object sender, EventArgs e)
        {
            ara();
        }
    }
}
