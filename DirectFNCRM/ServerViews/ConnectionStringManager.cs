using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace DirectFNCRM.ServerViews
{
  
    public static class ConnectionStringManager
    {
        private const string Provider = "DataProtectionConfigurationProvider";

        public static void UpdateAndProtect(string name, string newConnectionString)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var section = config.GetSection("connectionStrings");
            if (section == null)
                throw new ConfigurationErrorsException("connectionStrings section not found.");

            // Şifreliyse aç
            if (section.SectionInformation.IsProtected)
                section.SectionInformation.UnprotectSection();

            var csSection = (ConnectionStringsSection)section;

            if (csSection.ConnectionStrings[name] == null)
                throw new ConfigurationErrorsException($"Connection string not found: {name}");

            csSection.ConnectionStrings[name].ConnectionString = newConnectionString;

            // Tekrar şifrele
            section.SectionInformation.ProtectSection(Provider);
            section.SectionInformation.ForceSave = true;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        public static string Get(string name)
            => ConfigurationManager.ConnectionStrings[name]?.ConnectionString;
    }

}
