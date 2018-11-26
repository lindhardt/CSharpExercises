using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module5ConsoleApp.Drinks
{
    public abstract class Beverage
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public int ServingTemp { get; set; }

        public bool IsFairTrade { get; set; }

        protected Beverage(string name, int servingTemp, bool isFairTrade)
        {
            _name = name;
            ServingTemp = servingTemp;
            IsFairTrade = isFairTrade;
        }



        public override string ToString() =>
            base.ToString() + "\n" +
            string.Format("Name: {0}\nServing temp: {1}\nIs fair trade: {2}",
                Name, ServingTemp, IsFairTrade);
        //{
        //    return base.ToString() + "\n" + string.Format("Name: {0}\nServing temp: {1}\nIs fair trade: {2}",
        //        Name, ServingTemp, IsFairTrade );
        //}
    }
}
