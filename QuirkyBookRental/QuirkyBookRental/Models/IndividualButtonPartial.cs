using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace QuirkyBookRental.Models
{
    public class IndividualButtonPartial
    {
        public string ButtonType { get; set; }
        public string Action { get; set; }
        public string Glyph { get; set; }
        public string Text { get; set; }
        public int? GenderId { get; set; }
        public int? BookId { get; set; }
        public int? CustomerId { get; set; }
        public int? MemmershipTypeId { get; set; }


        public string ActionParameter()
        {
            
                var param = new StringBuilder(@"/");

                if (BookId != null && BookId > 0)
                {
                    param.Append(String.Format("{0}", BookId));
                }
                if (GenderId != null && GenderId > 0)
                {
                    param.Append(String.Format("{0}", GenderId));
                }
                if (CustomerId != null && CustomerId > 0)
                {
                    param.Append(String.Format("{0}", CustomerId));
                }
                if (MemmershipTypeId != null && MemmershipTypeId > 0)
                {
                    param.Append(String.Format("{0}", MemmershipTypeId));
                }
                return param.ToString();
            
        }
    }
}