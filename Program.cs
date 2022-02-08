using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NinjaTOSOandaReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var priceService = new PriceService();
            priceService.Start();
            Console.ReadKey();
        }
    }
}
