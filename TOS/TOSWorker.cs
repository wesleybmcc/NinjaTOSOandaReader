using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkOrSwim;

namespace ConsoleApp1
{
    public class TOSWorker : IDisposable
    {
        public bool KeepGoing { get; set; }
        private Client client = new Client();

        public TOSWorker()
        {
            KeepGoing = true;
            client.Add("AUD/JPY", QuoteType.Bid);
            client.Add("AUD/JPY", QuoteType.Ask);
            client.Add("EUR/JPY", QuoteType.Bid);
            client.Add("EUR/JPY", QuoteType.Ask);
            client.Add("/ES[H22]", QuoteType.Ask);
        }

        public void DoWork()
        {
            while (KeepGoing)
            {
                foreach (var quote in client.Quotes())
                {
                    Console.WriteLine("{0} {1} {2}", quote.Symbol,
                        quote.Type.ToString(), quote.Value);
                }
            }
        }

        public void Dispose()
        {
            KeepGoing = false;
        }
    }
}
