using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Implementations
{
    public static class Utility
    {
        public static char CompressChar = '-';

        public static string CompressLicensePlate(string licensePlate)
        {
            if (String.IsNullOrEmpty(licensePlate))
                return "";

            var compress = licensePlate.Trim();
            compress = compress.ToUpper();
            compress = Regex.Replace(compress, "[^0-9{a-zA-ZäöüÄÖÜ} ]", " ", RegexOptions.None); 
            compress = compress.Trim();
            char prevChar = new char();
            var newCompress = "";
            foreach(char c in compress)
            {
                if((c == ' ' && prevChar == ' ') || (c == '-' && prevChar == '-') || (c == ' ' && prevChar == '-') || (c == '-' && prevChar == ' ') )
                {

                }
                else
                {
                    
                    if(c == ' ')
                    {
                        newCompress += CompressChar;
                        prevChar = c;
                    }
                    else
                    {
                        newCompress += c;
                        prevChar = c;
                    }
                }
            }
            return newCompress;
        }
    }
}