using FraudPrevention.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudPrevention.Utilities
{
    public class Control
    {
        public static bool ControlData(Data input1,Data input2)
        {
            return (input1.Deal_id == input2.Deal_id && 
                input2.address.Equals(input2.address) && 
                !input1.credit_Card.Equals(input2.credit_Card));              
        }

        public static string FraudData(Data[]data)
        {
            StringBuilder output = new StringBuilder();

            for(int i=0;i<data.Length-1;i++)
            {
                for (int j = i+1; j < data.Length; j++)
                {
                    if (ControlData(data[i], data[j]))
                        output.AppendFormat("{0},{1}{2}", data[i].Order_id, data[i + 1].Order_id, "\n");
                }
            }
            return output.ToString();
        }


    }
}
