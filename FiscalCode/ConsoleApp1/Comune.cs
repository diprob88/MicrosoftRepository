using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comuni
{
    public class Comune
    {
        public string nome { set; get; }
        public string codice { set; get; }
        public Zona zona { set; get; }
        public Regione regione { set; get; }
        public Metropoli cm { set; get; }
        public Provincia provincia { set; get; }
        public string sigla { set; get; }
        public string codiceCatastale { set; get; }
        public List<string>cap { set; get; }
    }



    public class Zona
    {
        public string nome { set; get; }
        public string codice { set; get; }
    }

    public class Regione
    {
        public string nome { set; get; }
        public string codice { set; get; }
    }

    public class Provincia
    {
        public string nome { set; get; }
        public string codice { set; get; }
    }

    public class Metropoli
    {
        public string nome { set; get; }
        public string codice { set; get; }
    }
}
