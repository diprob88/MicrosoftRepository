using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comuni
{
    public static class Utility
    {
        public static List<Comune> ReadAllJson()
        {
            var json = File.ReadAllText(@"..\..\..\ConsoleApp1\src\json\comuni.json");
            var objects = JArray.Parse(json); // parse as array  
            List<Comune> listaComuni = new List<Comune>();

            foreach (JObject root in objects)
            {

                Comune comune = new Comune
                {
                    nome = (string)root["nome"],
                    codice = (string)root["codice"],
                    sigla = (string)root["sigla"],
                    codiceCatastale = (string)root["codiceCatastale"],
                    zona = new Zona
                    {
                        nome = (string)root["zona"]["nome"],
                        codice = (string)root["zona"]["codice"],
                    },
                    provincia = new Provincia
                    {
                        nome = (string)root["provincia"]["nome"],
                        codice = (string)root["provincia"]["codice"],
                    },
                    cm = new Metropoli
                    {
                        nome = (string)root["cm"]["nome"],
                        codice = (string)root["cm"]["codice"],
                    },
                    regione = new Regione
                    {
                        nome = (string)root["regione"]["nome"],
                        codice = (string)root["regione"]["codice"],
                    },

                };
                var caps = new List<string>();
                foreach (var cap in root["cap"])
                    caps.Add((string)cap);
                comune.cap = caps;
                listaComuni.Add(comune);
            }
            return listaComuni;
        }

        public static Comune SearchComune(string name)
        {
            var json = File.ReadAllText(@"..\..\..\ConsoleApp1\src\json\comuni.json");
            var objects = JArray.Parse(json); // parse as array  
            var search=objects.Where(c => ((string)c["nome"]).ToUpper().Equals(name.ToUpper())).ToList().FirstOrDefault();          
            Comune comune = new Comune
            {
                nome = (string)search["nome"],
                codice = (string)search["codice"],
                sigla = (string)search["sigla"],
                codiceCatastale = (string)search["codiceCatastale"],
                zona = new Zona
                {
                    nome = (string)search["zona"]["nome"],
                    codice = (string)search["zona"]["codice"],
                },
                provincia = new Provincia
                {
                    nome = (string)search["provincia"]["nome"],
                    codice = (string)search["provincia"]["codice"],
                },
                cm = new Metropoli
                {
                    nome = (string)search["cm"]["nome"],
                    codice = (string)search["cm"]["codice"],
                },
                regione = new Regione
                {
                    nome = (string)search["regione"]["nome"],
                    codice = (string)search["regione"]["codice"],
                },

            };
            var caps = new List<string>();
            foreach (var cap in search["cap"])
                caps.Add((string)cap);
            comune.cap = caps;
            return comune;
        }

        public static void ScriptInsertGenerate(List<Comune> comuni)
        {
            string output = "INSERT INTO Comuni (CodiceCatastale,Nome,Regione,Provincia,Sigla,CAP) VALUES";
            foreach (var comune in comuni)
            {
                output += String.Format("('{0}','{1}','{2}','{3}','{4}','{5}'),\n",
                    comune.codiceCatastale,
                    CheckMark(comune.nome),
                     CheckMark(comune.regione.nome),
                    String.IsNullOrEmpty(comune.provincia.nome) ? CheckMark(comune.cm.nome) : CheckMark(comune.provincia.nome),
                    comune.sigla,
                    ListCap(comune.cap));
            }
            output = output.Remove(output.Length - 2);

            File.WriteAllText(@"C:\Users\Roberto\Desktop\WriteText.txt", output);
        }
        public static string ListCap(List<string> caps)
        {
            string output = string.Empty;
            foreach (var cap in caps)
            {
                output += cap + ";";
            }
            output = output.Remove(output.Length - 1);
            return output;
        }
        public static string CheckMark(string word)
        {
            if(word.Contains("'"))
            {
               word= word.Replace("'", "\\'");
            }
            return word;
        }
        

    }
}
