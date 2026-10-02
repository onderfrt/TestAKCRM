using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectFNCRM.ServerViews
{
    public partial class formSaatKotrol : Form
    {
        public formSaatKotrol()
        {
            InitializeComponent();
        }

        private void formSaatKotrol_Load(object sender, EventArgs e)
        {
            textBox1.Text = Server.referance.kotrolsaati;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Server.referance.kotrolsaati = textBox1.Text;

            this.Close();
        }
    }
}
