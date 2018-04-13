using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudPrevention.Model
{
    public class Data
    {

        private string[] stringSeparators = new string[] { "," };
        public int Order_id { get; set; }
        public int Deal_id { get; set; }
        public string Email { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_Code { get; set; }
        public string credit_Card { get; set; }

        public Data(string input)
        {
            try
            {
                var inizialize = input.Split(stringSeparators, StringSplitOptions.None);
                Order_id = Convert.ToInt32(inizialize[0]);
                Deal_id = Convert.ToInt32(inizialize[1]);
                Email = inizialize[2];
                address = inizialize[3];
                city = inizialize[4];
                state = inizialize[5];
                zip_Code = inizialize[6];
                credit_Card = inizialize[7];
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }


        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("Order_id: {0}{1}", Order_id, "\n");
            output.AppendFormat("Deal_id: {0}{1}", Deal_id, "\n");
            output.AppendFormat("Email: {0}{1}", Email, "\n");
            output.AppendFormat("address: {0}{1}", address, "\n");
            output.AppendFormat("city: {0}{1}", city, "\n");
            output.AppendFormat("state: {0}{1}", state, "\n");
            output.AppendFormat("zip_Code: {0}{1}", zip_Code, "\n");
            output.AppendFormat("credit_Card: {0}{1}", credit_Card, "\n");
            return output.ToString();
        }


    }
}
