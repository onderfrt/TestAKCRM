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
    public partial class formExcelDurum : Form
    {
        public static formExcelDurum reference;
        public formExcelDurum()
        {
            InitializeComponent();

            reference = this;
        

        }

        public static string MesajText = "";
       
        private void formExcelDurum_Load(object sender, EventArgs e)
        {
            try
            {
                this.TopMost = true;

                clsEvent.ExcelMesajGeldi += parseMessage;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

      

        private void parseMessage(string Mesaj)
        {
            MesajText = Mesaj;
            if (lblDurum.InvokeRequired)
            {
                lblDurum.Invoke((MethodInvoker)delegate () { parseMessage(Mesaj); });
            }
            else
            {
                lblDurum.Text = Mesaj;
            }

        }

       
        private void formExcelDurum_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                reference = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

       
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if(MyTools.ExcelRutin.excelKuyruk.Count>0)
            //lblDurum.Text = MyTools.ExcelRutin.excelKuyruk.Dequeue();
        }
    }
}
