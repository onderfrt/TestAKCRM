using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectFNCRM.AdminViews
{
    public partial class formSubeEkle : Form
    {
        public formSubeEkle()
        {
            InitializeComponent();
        }

        private void formSubeEkle_Load(object sender, EventArgs e)
        {
            
            SubeleriGetir();

        }

        void SubeleriGetir()
        {
            using (crmDFNDataContext crm = new crmDFNDataContext())
            {
                SubeGrid.DataSource = crm.KurumSubes.Select(x => new
                {
                    x.id,
                    x.SubeAdi
                });
            }

        }


        private void btnYeniSube_Click(object sender, EventArgs e)
        {
            if (YeniSubeTxt.Text.Length <1)
            {
                return;
            }
            try
            {
                DialogResult dialogResult = MessageBox.Show(YeniSubeTxt.Text.ToString() + " şubesi eklenecek emin misiniz?", "Şube ekleniyor ....", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    using (crmDFNDataContext crm = new crmDFNDataContext())
                    {
                        KurumSube kurumSube = new KurumSube();
                        kurumSube.SubeAdi = YeniSubeTxt.Text;
                        kurumSube.MusteriNo = "10158";
                        crm.KurumSubes.InsertOnSubmit(kurumSube);
                        crm.SubmitChanges();
                    }
                    MessageBox.Show("Başarılı bir şekilde" + YeniSubeTxt.Text + " şubesi eklendi");
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }

            }
            catch (Exception)
            {
                MessageBox.Show(YeniSubeTxt.Text + " şubesi  eklenemedi");

                throw;
            }
            finally
            {
                SubeleriGetir();

            }
        }
        private void btnSubeGuncelle_Click(object sender, EventArgs e)
        {
            if (SubeGuncelletxt.Text.Length < 1)
            {
                return;
            }
            try
            {

                DialogResult dialogResult = MessageBox.Show(SubeGrid.CurrentRow.Cells[1].Value.ToString() + " şubesi "+ SubeGuncelletxt.Text.ToString()+" olarak güncellenecek emin misiniz?", "Şube güncelleniyor ....", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var eskiSubeAd = SubeGrid.CurrentRow.Cells[1].Value;
                    int eskiSubeId = Convert.ToInt32(SubeGrid.CurrentRow.Cells[0].Value);

                    using (crmDFNDataContext crm = new crmDFNDataContext())
                    {
                        var guncellenenSube = crm.KurumSubes.FirstOrDefault(x => x.id == eskiSubeId);
                        guncellenenSube.SubeAdi = SubeGuncelletxt.Text;
                        crm.SubmitChanges();
                    }

                    MessageBox.Show("Başarılı bir şekilde " + SubeGrid.CurrentRow.Cells[1].Value + " şubesi " + SubeGuncelletxt.Text + "olarak güncellendi.");
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
               
            }
            catch (Exception)
            {
                MessageBox.Show(SubeGuncelletxt.Text + " şubesi  eklenemedi");

            }
            finally
            {
                SubeleriGetir();
            }
        }
        private void btnSubeDelete_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult dialogResult = MessageBox.Show(SubeGrid.CurrentRow.Cells[1].Value.ToString()+" şubesi silinecek emin misiniz?", "Şube siliniyor ....", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    int silinecekSubeId = Convert.ToInt32(SubeGrid.CurrentRow.Cells[0].Value);

                    using (crmDFNDataContext crm = new crmDFNDataContext())
                    {
                        var silinecekSube = crm.KurumSubes.FirstOrDefault(x => x.id == silinecekSubeId);
                        crm.KurumSubes.DeleteOnSubmit(silinecekSube);
                        crm.SubmitChanges();
                    }

                    MessageBox.Show("Başarılı bir şekilde " + SubeGrid.CurrentRow.Cells[1].Value + " şubesi silindi.");
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }

               
            }
            catch (Exception)
            {
                MessageBox.Show(SubeGuncelletxt.Text + " şubesi silinemedi.");

            }
            finally
            {
                SubeleriGetir();
            }
        }

        private void SubeGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SubeGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            silinecekSubeAd.Text = SubeGrid.CurrentRow.Cells[1].Value.ToString();
        }
    }
}
