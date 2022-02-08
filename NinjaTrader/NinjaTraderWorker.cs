using System;
using System.Timers;
using NinjaTrader.Client;

namespace NinjaTOSOandaReader
{
    class NinjaTraderWorker : IDisposable
    {
        public bool KeepGoing { get; set; }
        private Timer pollingTimer = new Timer(500);
        private Client ninjaClient = new Client();
        private string futuresInstrument = "NQ 03-22";

        public NinjaTraderWorker()
        {
            KeepGoing = true;
            int connect = ninjaClient.Connected(1);
        }

        ~NinjaTraderWorker()
        {
            KeepGoing = false;
        }
        public void Dispose()
        {
            pollingTimer.Stop();
            ninjaClient.UnsubscribeMarketData(futuresInstrument);
        }

        public void DoWork()
        {
            ninjaClient.SubscribeMarketData(futuresInstrument);
            pollingTimer.Elapsed += PollingTimer_Elapsed;
            pollingTimer.AutoReset = true;
            pollingTimer.Start();
        }

        private void PollingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var ask = ninjaClient.MarketData(futuresInstrument, 2);
            var bid = ninjaClient.MarketData(futuresInstrument, 1);
            var last = ninjaClient.MarketData(futuresInstrument, 0);

            Console.WriteLine(string.Format("{0} bid {1} ask {2} last {3}", futuresInstrument, bid, ask, last));
        }
    }
}