using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Comuni
{
    public class FiscalCode
    {
        private Persona person;

        public FiscalCode(Persona person)
        {
            this.person = person;
            if (string.IsNullOrEmpty(person.country))
                person.country = "ITALIA";
        }

        #region Public Method
        //These are methods for test
        public string GetName()
        {
            return GetEditNameSurname(person.name);
        }
        public string GetSurname()
        {
            return GetEditNameSurname(person.surname);
        }

        public string GetCodeCatasto()
        {
            return GetCodeCatasto(person.cityOfBirth);
        }
   
        public string GetCodeData()
        {
            return GetCodeData(person.birthday);
        }
        public string GetCin()
        {
            return GetCIN();
        }

        public string GetFiscalCode()
        {
            string item = GetEditNameSurname(person.surname) + GetEditNameSurname(person.name) + GetCodeData(person.birthday) + GetCodeCatasto(person.cityOfBirth);
            int rest = GetRest(item);
            return item+ Constants.CheckRest[rest];
        }
        #endregion



        #region Functions Methods
        /*
        Cognome (tre lettere)
        Vengono prese le consonanti del cognome (o dei cognomi, se ve ne è più di uno) nel loro ordine 
        (primo cognome, di seguito il secondo e così via). Se le consonanti sono insufficienti, 
        si prelevano anche le vocali (se sono sufficienti le consonanti si prelevano la prima la seconda e la terza consonante), 
        sempre nel loro ordine e, comunque, le vocali vengono riportate dopo le consonanti (per esempio: Rosi → RSO). 
        Nel caso in cui un cognome abbia meno di tre lettere, la parte di codice viene completata aggiungendo la lettera X (per esempio: Fo → FOX). 
        Per le donne, viene preso in considerazione il solo cognome da nubile.
        Nome (tre lettere)
        Vengono prese le consonanti del nome (o dei nomi, se ve ne è più di uno) 
        nel loro ordine (primo nome, di seguito il secondo e così via) in questo modo: 
        se il nome contiene quattro o più consonanti, si scelgono la prima, la terza e la quarta (per esempio: Gianfranco → GFR), 
        altrimenti le prime tre in ordine (per esempio: Tiziana → TZN). 
        Se il nome non ha consonanti a sufficienza, si prendono anche le vocali; 
        in ogni caso le vocali vengono riportate dopo le consonanti (per esempio: Luca → LCU). 
        Nel caso in cui il nome abbia meno di tre lettere la parte di codice viene completata aggiungendo la lettera X.
        */
        private string GetEditNameSurname(string name)
        {
            if (name == null)
                name = string.Empty;

            if (name.Length < 3)
            {
                for (int i = name.Length; i < 3; i++)
                    name += "X";
                return name.ToUpper();
            }
            string regexConsonant = Regex.Replace(name.ToUpper(), Constants.PatternVocals, string.Empty);
            if (regexConsonant.Length == 3)
                return regexConsonant;
            else if (regexConsonant.Length > 3)
            {
                var characters = regexConsonant.ToCharArray();
                return ("" + characters[0] + characters[2] + characters[3]).ToUpper();
            }
            else
            {
                string regexVocal = Regex.Replace(name.ToUpper(), Constants.PatternConsonants, string.Empty);
                string output = regexConsonant + regexVocal;
                return output.Substring(0, 3).ToUpper();
            }

        }

        /*
        Data di nascita e sesso (cinque caratteri alfanumerici)
        Anno di nascita (due cifre): si prendono le ultime due cifre dell'anno di nascita;
        Mese di nascita (una lettera): a ogni mese dell'anno viene associata una lettera (Constants.CheckMonth) 
        Giorno di nascita e sesso (due cifre): si prendono le due cifre del giorno di nascita (se è compreso tra 1 e 9 si pone uno zero come prima cifra); 
        per i soggetti di sesso femminile, a tale cifra va sommato il numero 40. In questo modo il campo contiene la doppia informazione giorno di nascita e sesso.
        */
        private string GetCodeData(DateTime birthdate)
        {
            string output = birthdate.ToString("yy");
            switch(birthdate.Month)
            {
                case 1:
                    output += Constants.CheckMount["gennaio"];
                    break;
                case 2:
                    output += Constants.CheckMount["febbraio"];
                    break;
                case 3:
                    output += Constants.CheckMount["marzo"];
                    break;
                case 4:
                    output += Constants.CheckMount["aprile"];
                    break;
                case 5:
                    output += Constants.CheckMount["maggio"];
                    break;
                case 6:
                    output += Constants.CheckMount["giugno"];
                    break;
                case 7:
                    output += Constants.CheckMount["luglio"];
                    break;
                case 8:
                    output += Constants.CheckMount["agosto"];
                    break;
                case 9:
                    output += Constants.CheckMount["settembre"];
                    break;
                case 10:
                    output += Constants.CheckMount["ottobre"];
                    break;
                case 11:
                    output += Constants.CheckMount["novembre"];
                    break;
                case 12:
                    output += Constants.CheckMount["dicembre"];
                    break;
            }
            if (person.sex == 'F' || person.sex == 'f')
            {
                int day = 40 + birthdate.Day;
                output += day;
            }
            else if (birthdate.Day < 10)
            {
                output += "0" + birthdate.Day;
            }
            else
                output += birthdate.Day;

            return output;               
        }

        private string GetCodeCatasto(string name)
        {
            return Utility.SearchComune(name).codiceCatastale;
        }

        /*
        Carattere di controllo (una lettera)
        A partire dai quindici caratteri alfanumerici ricavati in precedenza, si determina il carattere di controllo 
        (indicato a volte come CIN, Control Internal Number) in base a un particolare algoritmo che opera in questo modo:
        - si mettono da una parte i caratteri alfanumerici che si trovano in posizione dispari e da un'altra quelli che si trovano in posizione pari;
        - fatto questo, i caratteri vengono convertiti in valori numerici secondo le seguenti tabelle:
          CARATTERI ALFANUMERICI DISPARI(Constants.CheckOddCharacters)
          CARATTERI ALFANUMERICI PARI(Constants.CheckEvenCharacters)
        - a questo punto, i valori che si ottengono dai caratteri alfanumerici pari e dispari vanno sommati tra di loro e il risultato va diviso per 26; 
          il resto della divisione fornirà il codice identificativo, ottenuto dalla tabella di conversione:
          RESTO(Constants.CheckRest)

        */
        private string GetCIN()
        {
            string item = GetEditNameSurname(person.surname) + GetEditNameSurname(person.name) + GetCodeData(person.birthday) + GetCodeCatasto(person.cityOfBirth);
            var tmp = item.ToCharArray();
            int sum = 0;
            for(int i=0;i<tmp.Length;i++)
            {
                if (i % 2 == 0)
                    sum += Constants.CheckOddCharacters[tmp[i]];
                else
                    sum += Constants.CheckEvenCharacters[tmp[i]];
            }
            sum = sum % 26;
            return Constants.CheckRest[sum];
        }

        private int GetRest(string partialcode)
        {
            var tmp = partialcode.ToCharArray();
            int sum = 0;
            for (int i = 0; i < tmp.Length; i++)
            {
                if (i % 2 == 0)
                    sum += Constants.CheckOddCharacters[tmp[i]];
                else
                    sum += Constants.CheckEvenCharacters[tmp[i]];
            }
            return sum % 26;
        }


            #endregion
        }
}
