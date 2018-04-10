﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comuni
{
    public static class Constants
    {
        public static readonly Dictionary<string, string> CheckMount = new Dictionary<string, string> {
            { "gennaio","A"},
            { "febbraio","B"},
            { "marzo","C"},
            { "aprile","D"},
            { "maggio","E"},
            { "giugno","H"},
            { "luglio","L"},
            { "agosto","M"},
            { "settembre","P"},
            { "ottobre","R"},
            { "novembre","S"},
            { "dicembre","T"}
        };

        //CARATTERI ALFANUMERICI DISPARI
        public static readonly Dictionary<char, int> CheckOddCharacters = new Dictionary<char, int> {
            { '0',1},
            { '1',0},
            { '2',5},
            { '3',7},
            { '4',9},
            { '5',13},
            { '6',15},
            { '7',17},
            { '8',19},
            { '9',21},
            { 'A',1},
            { 'B',0},
            { 'C',5},
            { 'D',7},
            { 'E',9},
            { 'F',13},
            { 'G',15},
            { 'H',17},
            { 'I',19},
            { 'J',21},
            { 'K',2},
            { 'L',4},
            { 'M',18},
            { 'N',20},
            { 'O',11},
            { 'P',3},
            { 'Q',6},
            { 'R',8},
            { 'S',12},
            { 'T',14},
            { 'U',16},
            { 'V',10},
            { 'W',22},
            { 'X',25},
            { 'Y',24},
            { 'Z',23}
        };

        //CARATTERI ALFANUMERICI PARI
        public static readonly Dictionary<char, int> CheckEvenCharacters = new Dictionary<char, int> {
            { '0',0},
            { '1',1},
            { '2',2},
            { '3',3},
            { '4',4},
            { '5',5},
            { '6',6},
            { '7',7},
            { '8',8},
            { '9',9},
            { 'A',0},
            { 'B',1},
            { 'C',2},
            { 'D',3},
            { 'E',4},
            { 'F',5},
            { 'G',6},
            { 'H',7},
            { 'I',8},
            { 'J',9},
            { 'K',10},
            { 'L',11},
            { 'M',12},
            { 'N',13},
            { 'O',14},
            { 'P',15},
            { 'Q',16},
            { 'R',17},
            { 'S',18},
            { 'T',19},
            { 'U',20},
            { 'V',21},
            { 'W',22},
            { 'X',23},
            { 'Y',24},
            { 'Z',25}
        };

        public static readonly Dictionary<int, string> CheckRest = new Dictionary<int, string> {
            { 0,"A"},
            { 1,"B"},
            { 2,"C"},
            { 3,"D"},
            { 4,"E"},
            { 5,"F"},
            { 6,"G"},
            { 7,"H"},
            { 8,"I"},
            { 9,"J"},
            { 10,"K"},
            { 11,"L"},
            { 12,"M"},
            { 13,"N"},
            { 14,"O"},
            { 15,"P"},
            { 16,"Q"},
            { 17,"R"},
            { 18,"S"},
            { 19,"T"},
            { 20,"U"},
            { 21,"V"},
            { 22,"W"},
            { 23,"X"},
            { 24,"Y"},
            { 25,"Z"}           
        };

        public static readonly Dictionary<int, string> CheckOmocodia = new Dictionary<int, string> {
            { 0,"L"},
            { 1,"M"},
            { 2,"N"},
            { 3,"P"},
            { 4,"Q"},
            { 5,"R"},
            { 6,"S"},
            { 7,"T"},
            { 8,"U"},
            { 9,"V"},
            
        };

        public static string PatternConsonants = "[BCDFGHJKLMNPQRSTWVXZ]";

        public static string PatternVocals = "[AEIOUY]";

    }
}
