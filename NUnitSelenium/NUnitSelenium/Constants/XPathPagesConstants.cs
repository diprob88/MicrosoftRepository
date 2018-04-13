using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitSelenium.Constants
{
    public class XPathPagesConstants
    {
        //Constants for Login Page
        public const string userID = "/html/body/div/div/div/div/div[2]/form/div[1]/div/input";
        public const string passID = "/html/body/div/div/div/div/div[2]/form/div[2]/div/input";
        public const string EnterButtonXPath = "//button[contains(text(),'Accedi')]";
        public const string EnterHomeElementXPath = "//*[@id='page-content-wrapper']/div/header/div/div/h3/center";
        public const string EnterHomeElementText = "CALCOLO ABBATTIMENTI";


    }
}
