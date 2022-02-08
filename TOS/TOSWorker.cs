using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using ThinkOrSwim;

namespace NinjaTOSOandaReader
{
    public class TOSWorker : IDisposable
    {
        public bool KeepGoing { get; set; }
        private Client client = new Client();

        public TOSWorker()
        {
            KeepGoing = true;
            new string[] { "EUR/USD", "USD/JPY", "GBP/USD", "USD/CHF", "AUD/USD", "USD/CAD" }.ToList().ForEach(instrument => {
                client.Add(instrument, QuoteType.Bid);
                client.Add(instrument, QuoteType.Ask);
            });
            //client.Add("AUD/JPY", QuoteType.Bid);
            //client.Add("AUD/JPY", QuoteType.Ask);
            //client.Add("EUR/USD", QuoteType.Bid);
            //client.Add("EUR/USD", QuoteType.Ask);


            //client.Add("/ES:XCME", QuoteType.Ask);
            //client.Add("/ES:XCME", QuoteType.Bid);
            //client.Add("/NQ:XCME", QuoteType.Ask);
            //client.Add("/NQ:XCME", QuoteType.Bid);
            //client.Add("/RTY:XCME", QuoteType.Ask);
            //client.Add("/RTY:XCME", QuoteType.Bid);
            //client.Add("/YM:XCBT", QuoteType.Ask);
            //client.Add("/YM:XCBT", QuoteType.Bid);

            // not working
            //client.Add("/CL:XNYM", QuoteType.Ask);
            //client.Add("/CL:XNYM", QuoteType.Bid);
            //client.Add("/GC:XCEC", QuoteType.Ask);
            //client.Add("/GC:XCEC", QuoteType.Bid);
        }

        public void DoWork()
        {
            while (KeepGoing)
            {
                foreach (var quote in client.Quotes())
                {
                    string message = string.Format("{0} {1} {2}", quote.Symbol,
                        quote.Type.ToString(), quote.Value);
                    Console.WriteLine(message);

                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "hello",
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "hello",
                                             basicProperties: null,
                                             body: body);
                    }

                }
            }
        }

        public void Dispose()
        {
            KeepGoing = false;
        }
    }
}
