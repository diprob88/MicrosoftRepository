using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.ComTypes;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PDFReader
{
    public class Esegui
    {
        static void Main(string[] args)
        {

            string path = @"c:\Doc\lista.txt";
            Funzioni.EstraiNomi(path);


           
        }
    }
}
