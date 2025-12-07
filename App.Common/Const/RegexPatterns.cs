using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Const
{
    public static class RegexPatterns
    {
        public const string MobileNumberItaly = @"^\+?(?:39)?\s?(?:3\d{2}|\d{2})\s?\d{6,7}$";
        public const string MobileNumberEgypt = "^01[0,1,2,5]{1}[0-9]{8}$";
      
    }
}
