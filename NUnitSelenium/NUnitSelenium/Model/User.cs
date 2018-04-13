using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitSelenium.Model
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Category { get; set; }

        public User()
        {
            Category = "*";
        }

        public User(string user, string pass)
        {
            UserName = user;
            Password = pass;
            Category = "*";
        }
        public override string ToString()
        {
            return string.Format("{0} ({1}) - cat: {2}", new object[] { UserName, Password, Category });
        }
    }
}
