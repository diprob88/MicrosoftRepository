
using ProgramSolution.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProgramSolution.LINQ
{
    public class LinqClass
    {
        private string[] names;
        private int[] numbers;
        private List<Customer> customers;

        public LinqClass()
        {
            names = GetStringArray();
            numbers = GenerateLotsOfNumbers(12345678);
            customers = GetCustomersList();
        }


        public void PrintNamesStartWith(string charapter)
        {
            /* var queryResults = from n in names
                 where n.StartsWith(charapter.ToUpper())
                 select n;*/
            var queryResults = names.Where(n => n.StartsWith(charapter.ToUpper()));
            Console.WriteLine("Names beginning with " + charapter.ToUpper() + ":");
            foreach (var item in queryResults)
            {
                Console.WriteLine(item);
            }

        }

        public void PrintNamesStarOrdertWith(string charapter)
        {
            /* var queryResults = from n in names
                 where n.StartsWith(charapter.ToUpper())
                 orderby n
                 select n;*/
            var queryResults = names.OrderBy(n => n).Where(n => n.StartsWith(charapter.ToUpper()));
            Console.WriteLine("Names beginning with " + charapter.ToUpper() + ":");
            foreach (var item in queryResults)
            {
                Console.WriteLine(item);
            }

        }

        public void PrintNumbersLess()
        {
            var queryResults = from n in numbers
                               where n < 1000
                               select n;
            Console.WriteLine("Numbers less than 1000:");
            foreach (var item in queryResults)
            {
                Console.WriteLine(item);
            }

        }

        public void PrintNumbersOperationLess()
        {
            Console.WriteLine("Numeric Aggregates");
            var queryResults = from n in numbers
                               where n > 1000
                               select n;
            Console.WriteLine("Count of Numbers > 1000");
            Console.WriteLine(queryResults.Count());
            Console.WriteLine("Max of Numbers > 1000");
            Console.WriteLine(queryResults.Max());
            Console.WriteLine("Min of Numbers > 1000");
            Console.WriteLine(queryResults.Min());
            Console.WriteLine("Average of Numbers > 1000");
            Console.WriteLine(queryResults.Average());
            Console.WriteLine("Sum of Numbers > 1000");
            Console.WriteLine(queryResults.Sum(n => (long)n));
        }

        public void SearchCustomersWithRegion(string region)
        {
            var queryResults =  from c in customers
                                where c.Region.ToUpper() == region.ToUpper()
                                select c;
            Console.WriteLine("Customers in "+region+":");
            foreach (Customer c in queryResults)
            {
                Console.WriteLine(c);
            }
        }


        #region Private Methods
        private static int[] GenerateLotsOfNumbers(int count)
        {
            Random generator = new Random(0);
            int[] result = new int[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = generator.Next();
            }
            return result;
        }
        private static string[] GetStringArray()
        {
            string[] names = { "Alonso", "Zheng", "Smith", "Jones", "Smythe", "Small", "Ruiz", "Hsieh", "Jorgenson", "Ilyich", "Singh", "Samba", "Fatimah" };
            return names;
        }

        private static List<Customer> GetCustomersList()
        {
            List<Customer> customers = new List<Customer>
            {
                new Customer { ID="A", City="New York", Country="USA",Region="North America", Sales=9999 },
                new Customer { ID="B", City="Mumbai", Country="India",Region="Asia", Sales=8888 },
                new Customer { ID="C", City="Karachi", Country="Pakistan",Region="Asia", Sales=7777 },
                new Customer { ID="D", City="Delhi", Country="India",Region="Asia", Sales=6666 },
                new Customer { ID="E", City="São Paulo", Country="Brazil",Region="South America", Sales=5555 },
                new Customer { ID="F", City="Moscow", Country="Russia",Region="Europe", Sales=4444 },
                new Customer { ID="G", City="Seoul", Country="Korea", Region="Asia",Sales=3333 },
                new Customer { ID="H", City="Istanbul", Country="Turkey",Region="Asia", Sales=2222 },
                new Customer { ID="I", City="Shanghai", Country="China", Region="Asia",Sales=1111 },
                new Customer { ID="J", City="Lagos", Country="Nigeria",Region="Africa", Sales=1000 },
                new Customer { ID="K", City="Mexico City", Country="Mexico",Region="North America", Sales=2000 },
                new Customer { ID="L", City="Jakarta", Country="Indonesia",Region="Asia", Sales=3000 },
                new Customer { ID="M", City="Tokyo", Country="Japan",Region="Asia", Sales=4000 },
                new Customer { ID="N", City="Los Angeles", Country="USA",Region="North America", Sales=5000 },
                new Customer { ID="O", City="Cairo", Country="Egypt",Region="Africa", Sales=6000 },
                new Customer { ID="P", City="Tehran", Country="Iran",Region="Asia", Sales=7000 },
                new Customer { ID="Q", City="London", Country="UK",Region="Europe", Sales=8000 },
                new Customer { ID="R", City="Beijing", Country="China",Region="Asia", Sales=9000 },
                new Customer { ID="S", City="Bogotá", Country="Colombia",Region="South America", Sales=1001 },
                new Customer { ID="T", City="Lima", Country="Peru",Region="South America", Sales=2002 }
            };
            return customers;
        }

        #endregion


    }
}