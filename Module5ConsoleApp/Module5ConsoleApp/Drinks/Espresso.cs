using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module5ConsoleApp.Drinks
{
    public sealed class Espresso : Coffee
    {
        public Espresso() : 
            base("Royal", 200, false, "Arabica", "Bras1l" )
        {
        }
    }
}
