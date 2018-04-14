
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FTP2DB
{
    public static class GeneraScript
    {

        private const string pathDetails = @"./../../BTData/details";
        private const string pathTnet = @"./../../BTData/tnet";
        private const string pathOutput = @"./../../BTData/output/";

        public static void GenerateScriptDetails()
        {
            string riga = string.Empty;
            string output = "(INSERT INTO TnetVoipBT_ConsumoNV(Servizio, DateTimeStart, Durata, Mittente, Tipo, Costo, Tradotto, TariffaNVID) VALUES\n";
            var files = Directory.GetFiles(pathDetails);
            foreach (var file in files)
            {
                output += GenerateInputDetails(file.ToString());
            }
            output = output.Remove(output.Length - 2);

            File.WriteAllText(pathOutput + "02_2018_Details.sql", output);
        }

        private static string GenerateInputDetails(string path)
        {
            string riga = string.Empty;
            string output = string.Empty;
            StreamReader file = null;
            try
            {
                file = new StreamReader(path);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Caricamento file errato! - " + e);
                //equivalente C# di System.exit(1);
                Environment.Exit(1);
            }
            while ((riga = file.ReadLine()) != null)
            {
                string[] dati = riga.Split(':');
                if (!dati[6].Contains(".") || dati[6].Contains("T.NET SpA")) continue;
                double x = double.Parse(dati[7], CultureInfo.InvariantCulture);
                double cal = OperazioniDB.CalcoloTnet2(Convert.ToInt32(x), dati[10].Trim(), dati[12].Trim());
                string calcolo = cal.ToString().Replace(",", ".");
                int tariffa = OperazioniDB.getTariffaNV(dati[12].Trim());
                //Inserimento in produzione
                output += String.Format("('{0}','{1}',{2},'{3}','{4}',{5},'{6}',{7}),\n", dati[3].Trim(), Supporto.FormattaDataDCH(dati[5], dati[6]), Convert.ToInt32(x), dati[9].Trim(), dati[10].Trim(), calcolo, dati[12].Trim(), tariffa);
                //Inserimento In test
            }
            return output;
        }

        public static void GenerateScriptTnet()
        {
            string output = "INSERT INTO TnetVoipBT_Consumo (DateTimeStart, Mittente, Destinatario, Destinazione, Durata, Costo, Tipo, TariffaID) VALUES";
            var files = Directory.GetFiles(pathTnet);
            foreach (var file in files)
            {
                if (!file.ToString().Contains("desktop.ini"))
                    output += GenerateInputTnet(file.ToString());
            }
            output = output.Remove(output.Length - 2);

            File.WriteAllText(pathOutput + "02_2018_Tnet.sql", output);
        }

        private static string GenerateInputTnet(string path)
        {
            string riga = string.Empty;
            string output = string.Empty;
            StreamReader file = null;
            try
            {
                file = new StreamReader(path);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Caricamento file errato! - " + e);
                //equivalente C# di System.exit(1);
                Environment.Exit(1);
            }
            while ((riga = file.ReadLine()) != null)
            {
                string[] dati = riga.Split(':');
                int sec = int.Parse(dati[7].Trim());
                string mittente = dati[12].Trim();

                double cal = OperazioniDB.CalcoloTnet(sec, dati[11].Trim(), dati[6].Trim(), dati[12].Trim());
                string calcolo = cal.ToString().Replace(",", ".");
                int tariffa = OperazioniDB.getTariffa(dati[12].Trim());
                string destinazione = dati[6].ToString().Trim();
                string tipo = dati[11].Trim();

                //eliminare questo controllo se mai avro un riscontro dall'amministrazione
                if (!tipo.Equals("F-M"))
                {
                    if ((tariffa > 9 && tariffa < 21) && (!mittente.Equals("095492181")))
                        calcolo = "0.0";
                }

                if (destinazione.Contains("'"))
                    destinazione = destinazione.Replace("'", "''");
                //Inserimento in Produzione
                //Inserimento in test
                output += String.Format("('{0}','{1}','{2}','{3}',{4},{5},'{6}',{7}),\n", Supporto.FormattaData(dati[4].Trim(), dati[5].Trim()), mittente, dati[3].Trim(), destinazione, sec, calcolo, dati[11].Trim(), tariffa);

            }
            return output;

        }




    }
}