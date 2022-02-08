using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTOSOandaReader
{
    public class PriceService
    {
        //private static readonly NinjaTraderWorker ninjaTraderWorker = new NinjaTraderWorker();
        private static readonly TOSWorker tosWorker = new TOSWorker();

        public PriceService()
        {
        }

        public void Start()
        {
            new Task(tosWorker.DoWork).Start();
            //new Task(ninjaTraderWorker.DoWork).Start();
        }

        public void Stop()
        {
            //ninjaTraderWorker.Dispose();
            tosWorker.Dispose();
        }
    }
}
