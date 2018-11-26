using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Module5ConsoleApp.Extensions

{
    public static class StringExtend
    {
        public static bool ContainsNumbers(this string s)
        {
            return Regex.IsMatch(s, @"\d");
        }
    }

    public static class IntExtend
    {
        public static int Double(this int n)
        {
            return n*2;
        }
    }
}
