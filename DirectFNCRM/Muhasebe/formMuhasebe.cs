using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectFNCRM.Muhasebe
{
    public partial class formMuhasebe : Form
    {
        public formMuhasebe()
        {
            InitializeComponent();
        }

        private void btnLisansFiyat_Click(object sender, EventArgs e)
        {
            AdminViews.formLisansFiyatlari frm = new AdminViews.formLisansFiyatlari();
            frm.ShowDialog();
        }

        private void btnOzetListe_Click(object sender, EventArgs e)
        {
            MyTools.ExcelRutin.BorsaOzetlisteToExcel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyTools.ExcelRutin.BorsaDetaylisteToExcel();
        }
    }
}
