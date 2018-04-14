using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

namespace FTP2DB
{
    public static class OperazioniDB
    {
        private static string dbhost = "212.34.194.171";
        private static string dbuser = "rodopi";
        private static string dbpass = "k5uba2la";
        private static string dbname = "TnetBill";
        // private static string stringaConnessione = string.Format("user id={0};password={1};database={2};server={3};", dbuser, dbpass,dbname,dbhost);
        private static string stringaConnessione = string.Format("data source={0};Persist Security Info=false;database={1};user id={2};password={3};MultipleActiveResultSets=True", dbhost, dbname, dbuser, dbpass);



        public static string getStringaConnessione() { return stringaConnessione; }
        public static SqlConnection getConnessione()
        {
            return new SqlConnection(getStringaConnessione());
        }

        #region  Recupero Tariffe
        public static int getTariffaNV(String tradotto)
        {
            int tarid = 0;
            try
            {
                SqlConnection conn = getConnessione();
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;
                // " + tradotto.substring(0, tradotto.length() - 2) + "
                string query = String.Format("select distinct tariffanvid from objectvalues, objects, plans, tnetvoipbt_tariffenv where objectvalues.objectid = objects.objectid and objects.planid = plans.planid and plans.customerid = tnetvoipbt_tariffenv.customerid and value_lpstr like ('%{0}%')", tradotto.Substring(0, tradotto.Length - 2));
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                tarid = int.Parse(reader[0].ToString());
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return tarid;
        }

        public static int getTariffa(String mitt)
        {
            int tarid = 0;
            try
            {
                SqlConnection conn = getConnessione();
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;
                string query = String.Format("select distinct tariffaid from objectvalues, objects, plans, tnetvoipbt_tariffe where objectvalues.objectid = objects.objectid and objects.planid = plans.planid and plans.customerid = tnetvoipbt_tariffe.customerid and value_lpstr like '%{0}%'", mitt.Substring(0, mitt.Length - 2));
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                tarid = int.Parse(reader[0].ToString());
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }

            return tarid;
        }
        #endregion

        #region Calcoli

        public static double CalcoloTnet(int sec, String tipo, String Descrizione, String mitt)
        {

            string innovazione = string.Empty;
            if (mitt.Equals("0952503398"))
            { }


            double costo = 0.0;
            double soglia = 0.0;
            int tarid = 0;
            int custid = 0;
            try
            {
                SqlConnection conn = getConnessione();
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;
                string query = String.Format("select distinct tariffainfoid, tnetvoipbt_tariffe.customerid from objectvalues, objects, plans, tnetvoipbt_tariffe where objectvalues.objectid = objects.objectid and objects.planid = plans.planid and plans.customerid = tnetvoipbt_tariffe.customerid and value_lpstr like '%{0}%'", mitt.Substring(0, mitt.Length - 2));
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                tarid = int.Parse(reader[0].ToString());
                custid = int.Parse(reader[1].ToString());
                reader.Close();

                if (tarid == 8 && string.Equals(tipo, "F-M", StringComparison.OrdinalIgnoreCase))
                {
                    string querySql = String.Format("select objectvalues.value_decimal from objectvalues, objects, plans where objectvalues.objectid = objects.objectid and objects.planid = plans.planid and itemid = 1067 and customerid = {0}", custid);
                    cmd.CommandText = querySql;
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    //old code rs.getDouble("value_decimal");
                    string valore = reader[0].ToString();
                    valore=valore.Replace(",", ".");
                    soglia = double.Parse(valore, CultureInfo.InvariantCulture);
                    reader.Close();

                    querySql = String.Format("select sum(durata) as durataTot from tnetvoipbt_consumo, tnetvoipbt_tariffe where tnetvoipbt_consumo.tariffaid = tnetvoipbt_tariffe.tariffaid and tnetvoipbt_tariffe.customerid = {0} and destinazione like ('%MOBILE%') and datetimestart >= '{1}' and datetimestart <= '{2}'", custid, Supporto.DataQueryIniziale(), Supporto.DataQueryFinale());
                    cmd.CommandText = querySql;
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    double duratatot = double.Parse(reader[0].ToString(), CultureInfo.InvariantCulture);

                    // if ((double)(rs.getInt("durataTot") + sec) <= soglia * 60.0)
                    if (duratatot + sec <= soglia * 60.0)
                    {
                        return 0.0;
                    }
                    if (duratatot <= soglia * 60.0)
                    {
                        sec = (int)(duratatot + sec - soglia * 60);
                    }
                }
                reader.Close();
                string querySql2 = String.Format("SELECT * FROM TnetVoipBT_TariffaInfo where tariffainfoid =  {0}",tarid);
                cmd.CommandText = querySql2;
                reader = cmd.ExecuteReader();
                reader.Read();
               
                if (Descrizione.Contains("Free"))
                {
                    conn.Close();
                    return 0.0;
                }
                if (string.Equals(tipo, "LOC", StringComparison.OrdinalIgnoreCase))
                {
                    double val = double.Parse(reader[1].ToString());
                    conn.Close();
                    return sec * val / 60.0;
                }
                if (string.Equals(tipo, "NAZ", StringComparison.OrdinalIgnoreCase))
                //if (tipo.compareToIgnoreCase("NAZ") == 0)
                {
                    // return (double)sec * rs.getDouble("Urbane") / 60.0;
                    double val = double.Parse(reader[1].ToString());
                    conn.Close();
                    return sec * val / 60.0;
                }
                //if (tipo.compareToIgnoreCase("ITZ") == 0)
                if (string.Equals(tipo, "ITZ", StringComparison.OrdinalIgnoreCase))
                {
                    //return (double)sec * rs.getDouble("Interurbane") / 60.0;
                    double val = double.Parse(reader[2].ToString());
                    conn.Close();
                    return sec * val / 60.0;

                }
                if (Descrizione.Contains("WIND"))
                {
                    //return (double)sec * rs.getDouble("WIND") / 60.0;
                    double val = double.Parse(reader[5].ToString());
                    conn.Close();
                    return sec * val / 60.0;

                }
                if (Descrizione.Contains("TI-M"))
                {
                    //return (double)sec * rs.getDouble("TIM") / 60.0;
                    double val = double.Parse(reader[3].ToString());
                    conn.Close();
                    return sec * val / 60.0;

                }
                if (Descrizione.Contains("VODAFONE"))
                {
                    // return (double)sec * rs.getDouble("VODAFONE") / 60.0;
                    double val = double.Parse(reader[4].ToString());
                    conn.Close();
                    return sec * val / 60.0;

                }
                if (Descrizione.Contains("H3G"))
                {
                    //return (double)sec * rs.getDouble("H3G") / 60.0;
                    double val = double.Parse(reader[6].ToString());
                    conn.Close();
                    return sec * val / 60.0;

                }
                if (!Descrizione.Contains("OTHERMOBILE"))
                {
                    conn.Close();
                    return costo;
                }
                //return (double)sec * rs.getDouble("H3G") / 60.0;
                double val2 = double.Parse(reader[6].ToString());
                conn.Close();
                return sec * val2 / 60.0;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return costo;
        }

        public static double CalcoloTnet2(int sec, String tipo, String mitt)
        {
            double costo = 0.0;
            int tarid = 0;
            try
            {
                using (SqlConnection conn = getConnessione())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader;
                    string query = String.Format("select distinct tariffanvinfoid from objectvalues, objects, plans, tnetvoipbt_tariffenv where objectvalues.objectid = objects.objectid and objects.planid = plans.planid and plans.customerid = tnetvoipbt_tariffenv.customerid and value_lpstr like '%{0}%'", mitt.Substring(0, mitt.Length - 2));
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;

                    reader = cmd.ExecuteReader();
                    reader.Read();
                    tarid = int.Parse(reader[0].ToString());
                    reader.Close();
               
                    query = String.Format("SELECT * FROM TnetVoipBT_TariffaNVInfo where tariffanvinfoid = {0}", tarid);
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    string compara = mitt.Substring(0, 2);

                    // if (tipo.compareToIgnoreCase("MOB-FIS") == 0)
                    if (string.Equals(tipo, "MOB-FIS", StringComparison.OrdinalIgnoreCase))
                    {
                        //if (mitt.Substring(0, 2).compareToIgnoreCase("33") == 0 || mitt.Substring(0, 2).compareToIgnoreCase("36") == 0)
                        if (string.Equals(compara, "33", StringComparison.OrdinalIgnoreCase) || string.Equals(compara, "36", StringComparison.OrdinalIgnoreCase))
                        {
                            //costo = (double)sec * rs.getDouble("TIM") / 60.0;
                            costo = sec * Supporto.ConvertiDecimale(reader[3].ToString()) / 60.0;
                        }
                        // if (mitt.Substring(0, 2).compareToIgnoreCase("34") == 0)
                        if (string.Equals(compara, "34", StringComparison.OrdinalIgnoreCase))
                        {
                            // costo = (double)sec * rs.getDouble("VODAFONE") / 60.0;
                            costo = sec * Supporto.ConvertiDecimale(reader[4].ToString()) / 60.0;
                        }
                        //if (mitt.Substring(0, 2).compareToIgnoreCase("32") == 0 || mitt.Substring(0, 2).compareToIgnoreCase("38") == 0)
                        if (string.Equals(compara, "32", StringComparison.OrdinalIgnoreCase) || string.Equals(compara, "38", StringComparison.OrdinalIgnoreCase))
                        {
                            //costo = (double)sec * rs.getDouble("WIND") / 60.0;
                            costo = sec * Supporto.ConvertiDecimale(reader[5].ToString()) / 60.0;
                        }
                        double tariffasecondo = double.Parse(reader[6].ToString(),CultureInfo.InvariantCulture);
                        costo = string.Equals(compara, "39", StringComparison.OrdinalIgnoreCase) ? sec * Supporto.ConvertiDecimale(reader[6].ToString()) / 60.0 : sec * Supporto.ConvertiDecimale(reader[3].ToString()) / 60.0;
                    }
                    else
                    {
                        
                        costo = string.Equals(tipo, "NAZION.", StringComparison.OrdinalIgnoreCase) ? sec *Supporto.ConvertiDecimale(reader[2].ToString()) / 60.0 : sec * Supporto.ConvertiDecimale(reader[1].ToString())/ 60.0;
                        //(double)sec * rs.getDouble("Naz") / 60.0 : (double)sec * rs.getDouble("Loc") / 60.0;
                    }
                    reader.Close();
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return costo;
        }

        #endregion


        #region Insert DB

        public static void loadFileDB2(String pathfile)//, String connectionUrl)
        {
            StreamReader file = null;
            try
            {
                file = new StreamReader(pathfile);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Caricamento file errato! - " + e);
                //equivalente C# di System.exit(1);
                Environment.Exit(1);
            }


            try
            {
                using (SqlConnection conn = getConnessione())
                {
                    conn.Open();
                    string riga = string.Empty;
                    string query = string.Empty;
                    while ((riga = file.ReadLine()) != null)
                    {
                        string[] dati = riga.Split(':');
                        if (!dati[6].Contains(".") || dati[6].Contains("T.NET SpA")) continue;
                        double x = double.Parse(dati[7], CultureInfo.InvariantCulture);
                        double cal = CalcoloTnet2(Convert.ToInt32(x), dati[10].Trim(), dati[12].Trim());
                        string calcolo = cal.ToString().Replace(",", ".");
                        int tariffa = getTariffaNV(dati[12].Trim());
                        //Inserimento in produzione
                        //query = String.Format("INSERT INTO TnetVoipBT_ConsumoNV (Servizio, DateTimeStart, Durata, Mittente, Tipo, Costo, Tradotto, TariffaNVID) VALUES('{0}','{1}',{2},'{3}','{4}',{5},'{6}',{7})", dati[3].Trim(), Supporto.FormattaDataDCH(dati[5], dati[6]), Convert.ToInt32(x), dati[9].Trim(), dati[10].Trim(), calcolo, dati[12].Trim(), tariffa);
                        //Inserimento In test
                        query = String.Format("INSERT INTO TnetVoipBT_ConsumoNV_Test (Servizio, DateTimeStart, Durata, Mittente, Tipo, Costo, Tradotto, TariffaNVID) VALUES('{0}','{1}',{2},'{3}','{4}',{5},'{6}',{7})", dati[3].Trim(), Supporto.FormattaDataDCH(dati[5], dati[6]), Convert.ToInt32(x), dati[9].Trim(), dati[10].Trim(), calcolo, dati[12].Trim(), tariffa);
                        //Console.WriteLine(query);
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.ExecuteNonQuery();

                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static void loadFileDB1(String pathfile)//, String connectionUrl)
        {
            StreamReader file = null;
            try
            {
                file = new StreamReader(pathfile);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Caricamento file errato! - " + e);
                //equivalente C# di System.exit(1);
                Environment.Exit(1);
            }

            try
            {
                using (SqlConnection conn = getConnessione())
                {
                    conn.Open();

                    string riga = string.Empty;
                    string query = string.Empty;
                
                    while ((riga = file.ReadLine()) != null)
                    {
                        string[] dati = riga.Split(':');
                        int sec = int.Parse(dati[7].Trim());
                        string mittente = dati[12].Trim();
                       
                            double cal = CalcoloTnet(sec, dati[11].Trim(), dati[6].Trim(), dati[12].Trim());
                        string calcolo = cal.ToString().Replace(",", ".");
                        int tariffa = getTariffa(dati[12].Trim());
                        string destinazione = dati[6].ToString().Trim();
                        string tipo = dati[11].Trim();
                      
                        
                        //eliminare questo controllo se mai avro un riscontro dall'amministrazione
                        if (!tipo.Equals("F-M"))
                        {
                            if((tariffa>9 && tariffa<21)&&(!mittente.Equals("095492181")))
                            calcolo = "0.0";
                        }

                        if (destinazione.Contains("'"))
                            destinazione = destinazione.Replace("'", "''");
                        //Inserimento in Produzione
                        //query = String.Format("INSERT INTO TnetVoipBT_Consumo (DateTimeStart, Mittente, Destinatario, Destinazione, Durata, Costo, Tipo, TariffaID) VALUES('{0}','{1}','{2}','{3}',{4},{5},'{6}',{7})", Supporto.FormattaData(dati[4].Trim(), dati[5].Trim()), mittente, dati[3].Trim(), destinazione, sec, calcolo, dati[11].Trim(), tariffa);
                        //Inserimento in test
                        query = String.Format("INSERT INTO TnetVoipBT_Consumo_Test (DateTimeStart, Mittente, Destinatario, Destinazione, Durata, Costo, Tipo, TariffaID) VALUES('{0}','{1}','{2}','{3}',{4},{5},'{6}',{7})", Supporto.FormattaData(dati[4].Trim(), dati[5].Trim()), mittente, dati[3].Trim(), destinazione, sec, calcolo, dati[11].Trim(), tariffa);

                        //if (cont > 302)
                        //    Console.WriteLine(query);
                      
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.ExecuteNonQuery();
                        

                       /* SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.ExecuteNonQuery();*/

                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static void loadFileDB3(String pathfile)//, String connectionUrl)
        {
            StreamReader file = null;
            try
            {
                file = new StreamReader(pathfile);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Caricamento file errato! - " + e);
                //equivalente C# di System.exit(1);
                Environment.Exit(1);
            }


            try
            {
                using (SqlConnection conn = getConnessione())
                {
                    conn.Open();
                    string riga = string.Empty;
                    string query = string.Empty;
                    while ((riga = file.ReadLine()) != null)
                    {
                        string[] dati = riga.Split(':');
                        //if (!dati[6].Contains(".") || dati[6].Contains("T.NET SpA")) continue;
                        double x = double.Parse(dati[8], CultureInfo.InvariantCulture);
                        double cal = CalcoloTnet2(Convert.ToInt32(x), dati[4].Trim(), dati[5].Trim());
                        string calcolo = cal.ToString().Replace(",", ".");
                        int tariffa = getTariffaNV(dati[5].Trim());
                        //Inserimento in produzione
                        //query = String.Format("INSERT INTO TnetVoipBT_ConsumoNV (Servizio, DateTimeStart, Durata, Mittente, Tipo, Costo, Tradotto, TariffaNVID) VALUES('{0}','{1}',{2},'{3}','{4}',{5},'{6}',{7})", dati[3].Trim(), Supporto.FormattaDataDCH(dati[5], dati[6]), Convert.ToInt32(x), dati[9].Trim(), dati[10].Trim(), calcolo, dati[12].Trim(), tariffa);
                        //Inserimento In test
                        query = String.Format("INSERT INTO TnetVoipBT_ConsumoNV_Test (Servizio, DateTimeStart, Durata, Mittente, Tipo, Costo, Tradotto, TariffaNVID) VALUES('{0}','{1}',{2},'{3}','{4}',{5},'{6}',{7})", dati[1].Trim(), Supporto.FormattaDataDCH(dati[6], dati[7]), Convert.ToInt32(x), dati[3].Trim(), dati[4].Trim(), calcolo, dati[5].Trim(), tariffa);
                        //Console.WriteLine(query);
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.ExecuteNonQuery();

                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        #endregion

    }
}


