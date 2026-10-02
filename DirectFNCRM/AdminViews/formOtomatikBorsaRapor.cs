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
    public partial class formOtomatikBorsaRapor : Form
    {
        private string _iniPath => System.Windows.Forms.Application.StartupPath + @"\ClientAyarlar.ini";
        private const string IniSection = "OtomatikBorsaRapor";
        private const string IniKeyPeriyot = "Periyot"; // GUNLUK | AYSONU | NONE
        private const string IniKeyMailListesi = "MailListesi"; // ";" ile ayrılmış

        public formOtomatikBorsaRapor()
        {
            InitializeComponent();
            this.Load += formOtomatikBorsaRapor_Load;
          //  this.btnKaydet.Click += btnKaydet_Click;
            this.cbGunluk.CheckedChanged += PeriodCheckBox_CheckedChanged;
            this.cbAysonu.CheckedChanged += PeriodCheckBox_CheckedChanged;
            this.cbNone.CheckedChanged += PeriodCheckBox_CheckedChanged;
           // this.btnTest.Click += btnTest_Click;
        }

        private void formOtomatikBorsaRapor_Load(object sender, EventArgs e)
        {
            try
            {
                var periyot = MyTools.AyarOku(IniSection, IniKeyPeriyot, 64, _iniPath);
                var mailList = MyTools.AyarOku(IniSection, IniKeyMailListesi, 4096, _iniPath);

                if (string.IsNullOrWhiteSpace(periyot)) periyot = "NONE";

                if (periyot.Equals("GUNLUK", StringComparison.OrdinalIgnoreCase))
                {
                    cbGunluk.Checked = true;
                    cbAysonu.Checked = false;
                    cbNone.Checked = false;
                }
                else if (periyot.Equals("AYSONU", StringComparison.OrdinalIgnoreCase))
                {
                    cbGunluk.Checked = false;
                    cbAysonu.Checked = true;
                    cbNone.Checked = false;
                }
                else
                {
                    cbGunluk.Checked = false;
                    cbAysonu.Checked = false;
                    cbNone.Checked = true;
                }

                // Mail giriş ipucu: adresleri ";" ile ayırın
                try
                {
                    var toolTip = new ToolTip();
                    toolTip.SetToolTip(richTextBox1, "Mail adreslerini ; (noktalı virgül) ile ayırın");
                }
                catch { }

                if (!string.IsNullOrWhiteSpace(mailList))
                {
                    richTextBox1.Text = mailList;
                }
                else
                {
                    // Placeholder niteliğinde ipucu metni (kayıt edilmez)
                    richTextBox1.Text = "ornek1@domain.com; ornek2@domain.com";
                    richTextBox1.ForeColor = Color.Gray;
                    richTextBox1.GotFocus += (s, ea) =>
                    {
                        if (richTextBox1.ForeColor == Color.Gray)
                        {
                            richTextBox1.Text = string.Empty;
                            richTextBox1.ForeColor = SystemColors.WindowText;
                        }
                    };
                }
            }
            catch { }
        }

        private void PeriodCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // En fazla 1 tanesi seçili olacak şekilde zorunlu kıl
            var cb = sender as CheckBox;
            if (cb == null || !cb.Checked) return;

            if (cb == cbGunluk)
            {
                cbAysonu.Checked = false;
                cbNone.Checked = false;
            }
            else if (cb == cbAysonu)
            {
                cbGunluk.Checked = false;
                cbNone.Checked = false;
            }
            else if (cb == cbNone)
            {
                cbGunluk.Checked = false;
                cbAysonu.Checked = false;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                var periyot = cbGunluk.Checked ? "GUNLUK" : (cbAysonu.Checked ? "AYSONU" : "NONE");
               // var mailList = (richTextBox1.Text ?? string.Empty).Trim();

                var mailList = richTextBox1.ForeColor == Color.Gray
             ? string.Empty
            : (richTextBox1.Text ?? string.Empty).Trim();

                MyTools.AyarYaz(IniSection, IniKeyPeriyot, periyot, _iniPath);
                MyTools.AyarYaz(IniSection, IniKeyMailListesi, mailList, _iniPath);

                MessageBox.Show("Ayarlar kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (formAdminAra.referance != null)
                {
                    formAdminAra.referance.TriggerOtomatikBorsaRaporNowForTest();
                }
                else
                {
                    MessageBox.Show("Ana ekran bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGonderimTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (formAdminAra.referance != null)
                {
                    formAdminAra.referance.TestMailGonder();
                }
                else
                {
                    MessageBox.Show("Ana ekran bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
