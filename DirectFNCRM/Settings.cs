using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectFNCRM
{
    [Serializable]
    public class Settings
    {
        public string Sertifika = "";
        public string SertifikaDisplay = "";
        public int SecureServicePort = 0;

        public static Settings MySetting = new Settings();
        public static void Deserialize()
        {
            if (MySetting.Sertifika == null) MySetting.Sertifika = "";
            if (MySetting.SertifikaDisplay == null) MySetting.SertifikaDisplay = "";
        }

    }
}
