using Module5ConsoleApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module5ConsoleApp.Drinks
{
    public class Coffee : Beverage
    {
        public string Bean { get; set; }

        public string CountryOfOrigin { get; set; }

        public Coffee( string name, int temp, bool isFairTrade,
            string bean, string countryOfOrigin ) :
            base(name, temp, isFairTrade)
        {
            Bean = bean;
            CountryOfOrigin = countryOfOrigin;
            Console.WriteLine( "Contains numbers: "+ CountryOfOrigin.ContainsNumbers() );
        }

        public override string ToString()
        {
            string st = base.ToString();

            return st + "\n" + string.Format("Bean: {0}\nCountry of Origin: {1}",
                Bean, CountryOfOrigin);
        }
    }
}
