using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP2DB
{

public class Start
    {

        public static void Decompress()
        {
            FileInfo fileToDecompress = new FileInfo(@"C:\DatiBT\dettaglio8xx_00228444_20160810.txt.gz");
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
        }






        public static void Main(string[] args)
        {
            /* FTP ftpClient = new FTP(@"ftp://ftp.italia.bt.com", "tnet", "Btftppw036$");
             ftpClient.download("tnet/dettaglio8xx_00228444_20160810.txt.gz", @"C:\DatiBT\dettaglio8xx_00228444_20160810.txt.gz");
             Decompress();*/

            //string path= @"C:\DatiBT\"+Supporto.DCHDataCorrente();
            //string path = @"C:\DatiBT\dettaglio8xx_00228444_20160823.txt";
            //string path2 = @"C:\DatiBT\T.NET SpA_DCH_06_02_2017.txt";

            //traffico_8xx_00228444_2016_09.txt
            // string path3 = @"C:\DatiBT\traffico_8xx_00228444_2016_09.txt";

            //T.NET SpA_DTC_09_2016
            // string path4 = @"C:\DatiBT\T.NET SpA_DTC_09_2016.txt";





            /* if (File.Exists(path2))
               OperazioniDB.loadFileDB1(path2);*/



            /* //dettaglio8xx_00228444_
             if (File.Exists(path))
             OperazioniDB.loadFileDB2(path);*/



            GeneraScript.GenerateScriptDetails();
            GeneraScript.GenerateScriptTnet();



        }
    }
}
