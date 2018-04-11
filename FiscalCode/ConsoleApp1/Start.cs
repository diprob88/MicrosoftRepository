using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Comuni
{
    public class Start
    {
        public static void Main(string[] args)
        {

            List<Comune> listaComuni = Utility.ReadAllJson();
            //Utility.ScriptInsertGenerate(listaComuni);
            var test = Utility.SearchComune("Catania");
            Persona person = new Persona
            {
                name = "Roberto",           
                surname = "Di Perna",
                sex = 'M',
                country="italia",
                cityOfBirth = "Catania",
                birthday = new DateTime(1988, 1, 20)
            };
            FiscalCode cod = new FiscalCode(person);

            //Utility.ReadExcel();

            Console.WriteLine(cod.GetFiscalCode());


            Console.Read();

        }
    }
}
