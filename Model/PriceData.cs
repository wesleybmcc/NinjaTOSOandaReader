using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Model
{
    public class PriceData
    {
        public string Instrument { get; set; }
        public string PeriodName { get; set; }
        public int PeriodValue { get; set; }
        public List<OHLC> OHLC { get; set; }

        public PriceData(string instrument, string periodName, int periodValue)
        {
            Instrument = instrument;
            PeriodName = periodName;
            PeriodValue = periodValue;
        }

        public void AddOHLC(IList<OHLC> ohlc)
        {
            OHLC.AddRange(ohlc);
        }

        public void ConvertBidAsk(IList<BidAsk> bidAsks)
        {
            int firstHour = bidAsks[0].DateTime.Hour;

            bidAsks.ToList().ForEach(bidAsk => { 

            });
        }
    }

    public class OHLC
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public bool IsTick { get; set; }

        public OHLC() { }

        public OHLC(DateTime startDateTime, DateTime endDateTime, double open, double high, double low, double close)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            IsTick = startDateTime.Ticks == endDateTime.Ticks;
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }
    }

    public class BidAsk
    {
        public DateTime DateTime { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public long Volume { get; set; }

        public BidAsk(DateTime dateTime, long volume, double bid, double ask)
        {
            DateTime = dateTime;
            Volume = volume;
            Bid = bid;
            Ask = ask;
        }
    }
}
