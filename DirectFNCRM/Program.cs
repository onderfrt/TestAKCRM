using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace DirectFNCRM
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
       {
            //    var runningProcessByName = Process.GetProcessesByName("DirectFNCRM");
            //    if (runningProcessByName.Length != 0)
            //    {

            //        MessageBox.Show(runningProcessByName[0].ProcessName+" Zaten Çalışır Durumda !!");
            //        return;
            //    }


            ConfigProtector.Protect();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MyTools.serverbag = true;

            Application.Run(new formLogin());
            //Application.Run(new ServerViews.Server());
        }
    }

    public static class ConfigProtector
    {
        public static void Protect()
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var section = config.GetSection("connectionStrings");

                if (section == null) return;

                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                section.SectionInformation.ForceSave = true;

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }
            catch (Exception ex)
            {
                MyTools.logyaz($"ConfigProtector.Protect fonk. HATA !!! {ex.Message}");
            }

        }
        public static string Unprotect(string protectedText)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(protectedText));
        }

    }

}
