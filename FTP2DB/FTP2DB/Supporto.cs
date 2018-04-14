using System;
using System.Globalization;

namespace FTP2DB
{
    public static class Supporto
    {


        public static string AddSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"[\000\010\011\012\015\032\042\047\134\140]", "\\$0");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }

            return Result;
        }

        public static double ConvertiDecimale(string decimale)
        {
            string supporto = decimale.Replace(",", ".");
            double tariffasecondo = double.Parse(supporto, CultureInfo.InvariantCulture);
            return tariffasecondo;
        }


        public static string DataCorrente()
        {
            string data=string.Empty;
            data = DateTime.Today.ToString("d");
            string[] partidata = data.Split('/');
            data = string.Format("{0}{1}{2}",partidata[2],partidata[1],partidata[0]);
            return data;
        }


        public static string DCHDataCorrente()
        {
            string[] partidata = DateTime.Today.ToString("d").Split('/');
            string dhc = string.Empty;
            dhc = string.Format("T.NET SpA_DCH_{0}_{1}_{2}.txt", partidata[0], partidata[1], partidata[2]);
            return dhc;
        }


        public static string DataQueryIniziale()
        {
            string[] partidata = DateTime.Today.ToString("d").Split('/');
            string dhc = string.Empty;
            dhc = string.Format("{0}-{1}-01 00:00:00.000", partidata[2], partidata[1]);
            return dhc;
        }

        public static string DataQueryFinale()
        {
            string[] partidata = DateTime.Today.ToString("d").Split('/');
            string dhc = string.Empty;
            dhc = string.Format("{0}-{1}-{2} 23:59:59.999", partidata[2], partidata[1],partidata[0]);
            return dhc;
        }



        public static string FormattaData(string data,string ora)
        {

            string tornadata = string.Empty;
            string parte1 = String.Format("{0}-{1}-{2}",data.Substring(0, 4), data.Substring(4, 2), data.Substring(6, 2));
            string parte2= String.Format("{0}:{1}:{2}.000", ora.Substring(0, 2), ora.Substring(2, 2), ora.Substring(4, 2));
            tornadata = String.Format("{0} {1}", parte1, parte2);
            return tornadata;
        }


        public static string FormattaDataDCH(string data1, string ora1)
        {
            string data = data1.Trim();
            string ora = ora1.Trim();
            string tornadata = string.Empty;
            string parte1 = string.Format("{0}-{1}-{2}", data.Substring(6, 4), data.Substring(3, 2),data.Substring(0, 2));
            string parte2 = ora.Replace(".",":");
            tornadata = String.Format("{0} {1}", parte1, parte2);
            return tornadata;
        }


    }

}
