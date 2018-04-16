using ProgramSolution.LINQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            LinqClass test = new LinqClass();
            //test.PrintNamesStarOrdertWith("s");
            //test.PrintNumbersLess();
            //test.PrintNumbersOperationLess();
            test.SearchCustomersWithRegion("asia");
            Console.Write("Program finished, press Enter/Return to continue:");
            Console.ReadLine();
        }
    }
}
