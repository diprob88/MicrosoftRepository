using FraudPrevention.Model;
using FraudPrevention.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudPrevention
{
    public class Test
    {
        public static void Main()
        {

            Console.WriteLine("Insert records number");         
            int records = System.Convert.ToInt32(Console.ReadLine());
            Data[] orders = new Data[records];
            try {
                for (int i = 0; i < orders.Length; i++)
                {
                    Console.WriteLine("Insert record {0}", i + 1);
                    orders[i] = new Data(Convert.ToString(Console.ReadLine()));
                }
                Console.WriteLine(Control.FraudData(orders));
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
            Console.Read();
        }
    }
}
