using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;
namespace DirectFNCRM
{
    public static class extentioncs
    {
        public static bool _ToBool(this string strX)
        {
            try
            {
                if (strX == "0")
                    return false;
                else
                    return true;
            }
            catch { return false; }
        }
        public static bool _ToBool(this int strX)
        {
            try
            {
                if (strX == 0)
                    return false;
                else
                    return true;
            }
            catch { return false; }
        }
        public static byte _ToByte(this string strX)
        {
            try
            {
                if (strX == "")
                    return 0;
                else
                {
                    byte OutInt = 0;
                    if (byte.TryParse(strX, out OutInt))
                        return OutInt;
                    else
                        return 0;
                }
            }
            catch { return 0; }
        }
        public static string _ToEng(this string strX)
        {
            var sbuilder = new StringBuilder(strX);
            try
            {
                for (int i = 0; i < sbuilder.Length; i++)
                {
                    char ch = sbuilder[i];
                    switch (ch)
                    {
                        case 'ç': sbuilder[i] = 'c'; break;
                        case 'Ç': sbuilder[i] = 'C'; break;
                        case 'ğ': sbuilder[i] = 'g'; break;
                        case 'Ğ': sbuilder[i] = 'G'; break;
                        case 'ı': sbuilder[i] = 'i'; break;
                        case 'İ': sbuilder[i] = 'I'; break;
                        case 'ö': sbuilder[i] = 'o'; break;
                        case 'Ö': sbuilder[i] = 'O'; break;
                        case 'ş': sbuilder[i] = 's'; break;
                        case 'Ş': sbuilder[i] = 'S'; break;
                        case 'ü': sbuilder[i] = 'u'; break;
                        case 'Ü': sbuilder[i] = 'U'; break;
                        default:
                            sbuilder[i] = ch;//char.ToUpperInvariant(ch);
                            break;

                    }
                }
                return sbuilder.ToString();
            }
            catch { return ""; }
        }
        public static string _ToEng(this string strX, bool convertX)
        {
            if (convertX == false) return strX;

            strX = strX.ToUpper();
            var sbuilder = new StringBuilder(strX);
            try
            {
                for (int i = 0; i < sbuilder.Length; i++)
                {
                    char ch = sbuilder[i];
                    switch (ch)
                    {
                        case 'ç':
                        case 'Ç': sbuilder[i] = 'C'; break;
                        case 'ğ':
                        case 'Ğ': sbuilder[i] = 'G'; break;
                        case 'ı':
                        case 'I':
                        case 'i':
                        case 'İ': sbuilder[i] = 'I'; break;
                        case 'ö':
                        case 'Ö': sbuilder[i] = 'O'; break;
                        case 'ş':
                        case 'Ş': sbuilder[i] = 'S'; break;
                        case 'ü':
                        case 'Ü': sbuilder[i] = 'U'; break;
                    }
                }
                return sbuilder.ToString().ToUpperInvariant();
            }
            catch { return ""; }
        }
        public static string _ToEngUp(this string strX)
        {
            //strX = strX.ToUpper();
            if (strX == null) return "";
            var sbuilder = new StringBuilder(strX);
            try
            {
                for (int i = 0; i < sbuilder.Length; i++)
                {
                    char ch = sbuilder[i];
                    switch (ch)
                    {
                        case 'ç':
                        case 'Ç': sbuilder[i] = 'C'; break;
                        case 'ğ':
                        case 'Ğ': sbuilder[i] = 'G'; break;
                        case 'ı':
                        case 'I':
                        case 'i':
                        case 'İ': sbuilder[i] = 'I'; break;
                        case 'ö':
                        case 'Ö': sbuilder[i] = 'O'; break;
                        case 'ş':
                        case 'Ş': sbuilder[i] = 'S'; break;
                        case 'ü':
                        case 'Ü': sbuilder[i] = 'U'; break;
                    }
                }
                return sbuilder.ToString().ToUpperInvariant();
            }
            catch { return ""; }
        }
        public static int _ToInt(this string strX)
        {
            try
            {
                int result = 0;
                if (int.TryParse(strX, out result))
                    return result;
                return 0;
            }
            catch { return 0; }
        }
        public static long _ToLong(this string strX)
        {
            try
            {
                long result = 0;
                if (long.TryParse(strX, out result))
                    return result;
                return 0;

                //if (strX == "") return 0;

                //return long.Parse(strX);
            }
            catch { return 0; }
        }
        public static int _Inverse(this int valX)
        {
            try
            {
                if (valX == 1)
                    return 0;
                else
                    return 1;
            }
            catch { return 0; }
        }
        public static bool _ToBool(this byte val)
        {
            try
            {
                if (val == 1)
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }
        public static byte _Inverse(this byte val)
        {
            try
            {
                if (val == 1)
                    return 0;
                else
                    return 1;
            }
            catch { return 0; }
        }
        public static byte _ToByte(this bool val)
        {
            try
            {
                if (val)
                    return 1;
                else
                    return 0;
            }
            catch { return 0; }
        }
        public static int _ToInt(this bool val)
        {
            try
            {
                if (val)
                    return 1;
                else
                    return 0;
            }
            catch { return 0; }
        }
        public static string _ToIntStr(this bool val)
        {
            try
            {
                var sonuc = 0;
                if (val)
                    sonuc = 1;
                else
                    sonuc = 0;
                return sonuc.ToString();
            }
            catch { return "0"; }
        }
        public static string _ToString(this bool val)
        {
            try
            {
                if (val)
                    return "1";
                else
                    return "0";
            }
            catch { return "0"; }
        }
        public static DateTime StrinToDateTime(this string datestr)
        {
            DateTime date = new DateTime();
            try
            {
                date = DateTime.ParseExact(datestr, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                return date;
            }
            catch { return date; }
        }
        public static string _ToMonthString(this int valX)
        {
            try
            {
                switch (valX)
                {
                    case 1: return "Ock";
                    case 2: return "Şub";
                    case 3: return "Mar";
                    case 4: return "Nis";
                    case 5: return "May";
                    case 6: return "Haz";
                    case 7: return "Tem";
                    case 8: return "Ağu";
                    case 9: return "Eyl";
                    case 10: return "Ekm";
                    case 11: return "Kas";
                    case 12: return "Ara";
                    default: return "";
                }
            }
            catch { return ""; }
        }
        public static DateTime _StartOfWeek(this DateTime dateX)
        {
            try
            {
                int diff = dateX.DayOfWeek - DayOfWeek.Monday;
                if (diff < 0)
                    diff += 7;

                return dateX.AddDays(-1 * diff).Date;
            }
            catch {  return dateX; }
        }
        public static string _ToDayMonthString(this DateTime datetimeX)
        {
            try
            {
                return datetimeX.Day.ToString() + " " + datetimeX.Month._ToMonthString();
            }
            catch { return ""; }
        }
        public static DateTime _PrevDayOfMonth(this DateTime dateX)
        {
            try
            {
                dateX = dateX.AddMonths(-1);
                return new DateTime(dateX.Year, dateX.Month, 1).AddMonths(1).AddDays(-1);
            }
            catch { return dateX; }
        }
        public static DateTime _LastDayOfMonth(this DateTime dateX)
        {
            try
            {
                return new DateTime(dateX.Year, dateX.Month, 1).AddMonths(1).AddDays(-1);
                       
            }
            catch { return dateX; }
        }
        public static DateTime _LastDayOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 12, 31);
        }
        public static DateTime _FirstDayOfMonth(this DateTime dateX)
        {
            try
            {
                return new DateTime(dateX.Year, dateX.Month, 1).AddMonths(1);
            }
            catch { return dateX; }
        }
        public static string Split(ref string strX, char charX)
        {
            if (strX == null)
                return "";
            int intPos = -1;
            string strOut = "";
            try
            {
                intPos = strX.IndexOf(charX);
                if (strX.IndexOf(charX) >= 0)
                {
                    strOut = strX.Substring(0, intPos);
                    strX = strX.Substring(intPos + 1);
                }
                return strOut;
            }
            catch { return ""; }
        }
        private static readonly Encoding _iso8859_9 = Encoding.GetEncoding("iso-8859-9");

        public static string _InsertHeaderHTTP(this string buffer)
        {
            try
            {
                int contentLength = _iso8859_9.GetByteCount(buffer);
                var sb = new StringBuilder(128 + buffer.Length);
                sb.Append("HTTP/1.1 200 \r\n");
                sb.Append("Access-Control-Allow-Origin: *\r\n");
                sb.Append("Content-Type: charset=iso-8859-9\r\n");
                sb.Append("Content-Length: ").Append(contentLength).Append("\r\n\r\n");
                sb.Append(buffer);
                return sb.ToString();
            }
            catch { return ""; }
        }
        public static User _GetOrInsert(this string usrname)
        {
            try
            {
                crmDFNDataContext crm = new crmDFNDataContext();
                if (crm.Users.Where(x => x.UserName == usrname).Any())
                {
                    return crm.Users.FirstOrDefault(x => x.UserName == usrname);

                }
                else
                {
                    return new User();
                }
            }
            catch { return new User(); }
        }
        public static string TurkceHttpResponse(this string text)
        {
            try
            {
                var converttext = text;
                converttext = converttext.Replace("%C4%B1", "ı");
                converttext = converttext.Replace("%C3%A7", "ç");
                converttext = converttext.Replace("%C5%9F", "ş");
                converttext = converttext.Replace("%C4%9F", "ğ");
                converttext = converttext.Replace("%C3%BC", "ü");
                converttext = converttext.Replace("%C3%B6", "ö");
                converttext = converttext.Replace("%C4%B0", "İ");
                converttext = converttext.Replace("%C5%9E", "Ş");
                converttext = converttext.Replace("%C3%9C", "Ü");
                converttext = converttext.Replace("%C3%87", "Ç");
                converttext = converttext.Replace("%C3%96", "Ö");
                converttext = converttext.Replace("%E2%80%93", "–");
                converttext = converttext.Replace("%C4%9E", "Ğ");



                return converttext;


            }
            catch { return text; }
        }
        public static string _GetText(this XmlDocument docX, string keyX)
        {
            try
            {
                XmlNode node = docX.SelectSingleNode(keyX);

                if (node == null)
                    return "";
                return node.InnerText;
            }
            catch { return ""; }
        }
        public static string _GetText(this XmlNode nodeX, string keyX)
        {
            try
            {
                XmlNode node = nodeX.SelectSingleNode(keyX);

                if (node == null)
                    return "";
                return node.InnerText;
            }
            catch { return ""; }
        }
        public static V _GetOrInsert<T, V>(this Dictionary<T, V> dictionary, T key) where V : new()
        {
            try
            {
                if (dictionary.ContainsKey(key)) return dictionary[key];
                V item = new V();
                dictionary[key] = item;
                return item;
            }
            catch { return new V(); }
        }
        public static void _CopyToExcel(this DataGridView gridX)
        {

            try
            {
                MyTools.ExcelRutin.RunExcel();

                var cellarray = new object[gridX.Rows.Count + 1, gridX.ColumnCount];

                if (gridX.ColumnHeadersVisible)
                {
                    for (int j = 0; j < gridX.ColumnCount; j++)
                        cellarray[0, j] = gridX.Columns[j].HeaderText;
                }
                for (int i = 0; i < gridX.Rows.Count; i++)
                {
                    for (int j = 0; j < gridX.ColumnCount; j++)
                    {
                        cellarray[i + 1, j] = "";
                        if (gridX[j, i].Value != null)
                            cellarray[i + 1, j] = gridX[j, i].FormattedValue.ToString();
                    }
                }
                var excelworkbook = MyTools.ExcelRutin.ExcellApp.Workbooks.Add(Type.Missing);
                var excelsheet = (Excel.Worksheet)excelworkbook.Sheets.get_Item(1);

                if (MyTools.ExcelRutin.ExcellApp.Visible)
                {
                    Excel.Range range = excelsheet.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                    range.Value2 = cellarray;
                }
            }
            catch (Exception error) { MessageBox.Show(error.Message); }
        }
        public static void _CopyToExcel(this DataGridView gridX, string filenameX)
        {

            try
            {
                MyTools.ExcelRutin.RunExcel();

                var cellarray = new string[gridX.Rows.Count + 1, gridX.ColumnCount];
                for (int j = 0; j < gridX.ColumnCount; j++)
                    cellarray[0, j] = gridX.Columns[j].HeaderText;

                for (int i = 0; i < gridX.Rows.Count; i++)
                {
                    for (int j = 0; j < gridX.ColumnCount; j++)
                    {
                        cellarray[i + 1, j] = "";
                        if (gridX[j, i].Value != null)
                            cellarray[i + 1, j] = gridX[j, i].FormattedValue.ToString();
                    }
                }

                var excelworkbook = MyTools.ExcelRutin.GetExcelWorkbook(filenameX);
                if (excelworkbook != null)
                {
                    excelworkbook.Save();
                    excelworkbook.Close(false);
                }
                excelworkbook = MyTools.ExcelRutin.OpenExcelWorkbook(filenameX);
                if (excelworkbook == null) return;

                var excelsheet = (Excel.Worksheet)excelworkbook.Sheets.get_Item(1);

                MyTools.ExcelRutin.ExcellApp.ScreenUpdating = false;
                if (MyTools.ExcelRutin.ExcellApp.Visible)
                {
                    Excel.Range range = excelsheet.Cells.get_Resize(cellarray.GetLength(0), cellarray.GetLength(1));
                    range.Value2 = cellarray;
                }
                MyTools.ExcelRutin.ExcellApp.ScreenUpdating = true;
            }
            catch (Exception error) { MessageBox.Show(error.Message); MyTools.ExcelRutin.ExcellApp.ScreenUpdating = true; }
        }
        public static void _DoubleBuffer(this DataGridView grid, bool setting)
        {
            try
            {
                Type type1 = grid.GetType();
                PropertyInfo info1 = type1.GetProperty("DoubleBuffered",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                info1.SetValue(grid, setting, null);
            }
            catch { }
        }
        public static string _RequestHttp(this string urlX)
        {
            try
            {
                string response = "";
                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateCertificate);
                CookieContainer cookie = new CookieContainer();
                HttpWebRequest http = (HttpWebRequest)WebRequest.Create(urlX);
                var newproxy = new System.Net.WebProxy();
                newproxy.Credentials = CredentialCache.DefaultCredentials;
                newproxy.BypassProxyOnLocal = true;
                http.Proxy = newproxy;
                http.AllowAutoRedirect = true;
                http.ContentType = "application/x-www-form-urlencoded";
                http.Method = "GET";
                http.Timeout = 5000;
                http.CookieContainer = cookie;


                using (var webresp = http.GetResponse())
                using (var streamx = webresp.GetResponseStream())
                using (var streamreaderx = new StreamReader(streamx, Encoding.GetEncoding("iso-8859-9")))
                {
                    response = streamreaderx.ReadToEnd();
                }

                return response;
            }
            catch { return ""; }
        }
        public static void RangeBackColor(this Range range, Color renk)
        {
            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(renk);

        }
        public static void RangeFontColor(this Range range, Color renk)
        {
            range.Font.Color = System.Drawing.ColorTranslator.ToOle(renk);

        }
        public static void BorderWeight(this Range range, int sec)
        {
            if (sec == 2)
                range.Borders.Weight = XlBorderWeight.xlThin;
            else if (sec == 4)
                range.Borders.Weight = XlBorderWeight.xlThick;
            else if (sec == 1)
                range.Borders.Weight = XlBorderWeight.xlHairline;
            else if (sec == 3)
                range.Borders.Weight = XlBorderWeight.xlMedium;


        }
        public static void RangeOrtala(this Range range, int sec)
        {
            if (sec == 1)
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            else if (sec == 2)
                range.VerticalAlignment = XlVAlign.xlVAlignCenter;
            else if (sec == 3)
            {
                range.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            }

        }
        static bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
