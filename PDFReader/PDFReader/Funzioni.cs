using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFReader
{
    public class Funzioni
    {

        public static void crealista()
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            StreamReader file = new System.IO.StreamReader(@"c:\Doc\albo.txt");
            List<string> ingegneri = new List<string>();
            string nuova = string.Empty;

            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\ListaIngegneri.txt"))
            {

                while ((line = file.ReadLine()) != null)
                {
                    string riga = line.Trim();
                    nuova += " " + riga;
                    if (counter > 0)
                    {
                        if (string.IsNullOrEmpty(riga))
                        {
                            ingegneri.Add(nuova);
                            outputFile.WriteLine(nuova);
                            nuova = string.Empty;
                        }
                    }

                    System.Console.WriteLine(line);
                    counter++;
                }
            }
            file.Close();
        }



        public static string creaInsert(string path)
        {
            StreamReader file = new StreamReader(path);
            string insert = "INSERT INTO Ingegneri (cognome, nome,luogoDiNascita,dataDiNascita,codiceFiscale,numero,email) VALUES ";
            string line = string.Empty;

            while ((line = file.ReadLine()) != null)
            {
                String[] splitstrings = line.Split(';');
                string value = "\n(";
                foreach(string valore in splitstrings)
                {
                    value += "'" + valore.Trim() + "',";
                }
                value = value.TrimEnd(',');
                value += "),";
                insert += value;
            }

            insert = insert.TrimEnd(',');

            return insert;
        }



        public static void ReadPDF(string path)
        {
            PdfReader reader = new PdfReader(path);
            int intPageNum = reader.NumberOfPages;
            string[] words;
            string line;

            for (int i = 2; i <= intPageNum; i++)
            {
                var text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());


                words = text.Split('\n');
                for (int j = 8, len = words.Length; j < len; j++)
                {
                    line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));
                }
            }

            Console.WriteLine(Funzioni.creaInsert(path));
            Console.ReadLine();
        }

        public static void EstraiNomi(string path)
        {
            StreamReader file = new System.IO.StreamReader(path);
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string line = string.Empty;

            using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\ListaIngegneriNomi.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                   var riga = line.Trim().Split();
                   int l = riga.Length-1;

                   outputFile.WriteLine(string.Format("{0}",riga[2]));



                }
            }
            file.Close();
        }
        

    }
}
