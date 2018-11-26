using Module5ConsoleApp.Drinks;
using Module5ConsoleApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module5ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Espresso esp = new Espresso();

            Console.WriteLine("Contain Numbers? " + esp.Bean.ContainsNumbers());

            Console.WriteLine("Mul2: " + esp.ServingTemp.Double());

            Console.WriteLine(esp);

            Console.ReadKey();
        }
    }
}
