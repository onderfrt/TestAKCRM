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
    public partial class formUserNotscs : Form
    {
        public formUserNotscs()
        {
            InitializeComponent();
        }

        public int activeuserID=0;
        public int activeitem=0;
        
        private void formUserNotscs_Load(object sender, EventArgs e)
        {

            activeuserID = (int)this.Tag;
            componentGizleGoster(false);


            GridDoldur();
        }

        public void componentGizleGoster(bool status)
        {


          
            rboxUserNot.ReadOnly = !status;
            btnSave.Enabled = status;

        }
        public void GridDoldur()
        {

            try
            {
              
                crmDFNDataContext crm = new crmDFNDataContext();
                var sorgu = crm.UserNots.Where(x=>x.Userid== activeuserID);

                var say = sorgu.ToList().Count;
            
                if (say==0)
                {
                    componentGizleGoster(true);
                }
                gridUserNots.DataSource = sorgu.OrderByDescending(x=>x.Tarih).Select(x=> new { x.id , x.Tarih,GirisYapan=x.Calisan.Ad+" "+ x.Calisan.Soyad  });

             


            }
            catch (Exception ex)
            {


                MessageBox.Show(ex.Message);
            }


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();        
            


                if (activeitem == 0)
                {
                    #region YeniKayit


                    UserNot not = new UserNot();

                    not.CalisanId = formAdminAra.referance.ActiveCalisan.calisanID;
                    not.Tarih = DateTime.Now;
                    not.Userid = activeuserID;
                    not.Text = rboxUserNot.Text;

                    crm.UserNots.InsertOnSubmit(not);
                    crm.SubmitChanges();

                    MessageBox.Show("Notunuz Kaydedildi");

                    

                    #endregion

                }
                else
                {

                    #region Guncelle



                    var notu = crm.UserNots.FirstOrDefault(x=>x.id==activeitem);
                    if (notu == null)
                    {
                        MessageBox.Show("Not Bulunamadı");
                        return;
                    }

                    if (notu.CalisanId != formAdminAra.referance.ActiveCalisan.calisanID)
                    {
                        MessageBox.Show("Bu değiştirmeye hakkınız yoktur.");
                        return;
                    }
                   
                    notu.Text = "Bu Not " + DateTime.Now.ToString() + " tarihinde değiştirldi -------------------- \n"+rboxUserNot.Text;

                    crm.SubmitChanges();

                    #endregion

                }

                componentGizleGoster(false);
                GridDoldur();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            componentGizleGoster(true);
        }

        private void gridUserNots_SelectionChanged(object sender, EventArgs e)
        {

            try
            {
                if (gridUserNots.SelectedRows.Count < 1) return;

                crmDFNDataContext crm = new crmDFNDataContext();
                var id = (int)gridUserNots.CurrentRow.Cells[0].Value;
                var not = crm.UserNots.FirstOrDefault(x => x.id == id);
                activeitem = not.id;
                rboxUserNot.Text = not.Text;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            activeitem =0;
            componentGizleGoster(true);
            rboxUserNot.Text = "";
        }
    }
}
