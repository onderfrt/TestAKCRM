using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectFNCRM
{
    class clsEvent
    {

        public delegate void ExcelAktarimMessageGeldi(string Mesaj);

        public static event ExcelAktarimMessageGeldi ExcelMesajGeldi;

        public static void onExcelMesajGeldi(string Mesaj)
        {
            if (ExcelMesajGeldi != null)
                ExcelMesajGeldi(Mesaj);
        }

    }
}
