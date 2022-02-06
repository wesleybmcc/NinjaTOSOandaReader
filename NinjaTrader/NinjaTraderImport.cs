using ConsoleApp1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.NinjaTrader
{
    public class NinjaTraderImport
    {
        public static string baseFolder = @"I:\workspace\data\ninjatrader";

        public static PriceData LoadHistoricalData(string instrument, string periodName, int periodValue)
        {
            var ninjaTraderImport = new NinjaTraderImport();
            var priceData = new PriceData(instrument, periodName, periodValue);
            var fileName = String.Format(@"{0}\{1}.Last.txt", baseFolder, instrument);

            using(var reader = new System.IO.StreamReader(fileName))
            {
                priceData.AddOHLC(ninjaTraderImport.ParseOHLC(reader.ReadToEnd()));
            }

            return priceData;
        }

        private DateTime ParseDateTime(string date, string time)
        {
            int year = Convert.ToInt32(date.Substring(0, 4));
            int month = Convert.ToInt32(date.Substring(4, 2));
            int day = Convert.ToInt32(date.Substring(6, 2));
            int hour = Convert.ToInt32(time.Substring(0, 2));
            int minute = Convert.ToInt32(time.Substring(2, 2));
            int second = Convert.ToInt32(time.Substring(4, 2));
            return new DateTime(year, month, day, hour, minute, second);
        }

        // harded for 15 minutes
        private List<OHLC> OHLCGranularityGroup(IList<OHLC> ohlc, int minutes = 15)
        {
            var ohlcList = new List<OHLC>();

            var groupBy = ohlc.GroupBy(y => (int)(y.StartDateTime.Ticks / TimeSpan.TicksPerMinute / minutes));
            groupBy.ToList().ForEach(gb => {
                var high = gb.Max(g => g.High);
                var low = gb.Min(g => g.Low);

                ohlcList.Add(new OHLC(gb.First().StartDateTime, gb.Last().StartDateTime, 
                    gb.First().Open, high, low, gb.Last().Close));
            });

            return ohlcList;
        }
        private List<OHLC> ParseOHLC(string data)
        {
            var fileLineData = data.Split('\r');
            var ohlcList = new List<OHLC>();

            foreach (var fld in fileLineData)
            {
                var fieldData = fld.Replace("\n", "");
                if (fieldData.Length > 0)
                {
                    var fieldDataSplit = fieldData.Replace(";", " ").Split(' ');
                    if (fieldDataSplit.Length == 7)
                    {
                        DateTime dateTime = ParseDateTime(fieldDataSplit[0], fieldDataSplit[1]);                        
                        ohlcList.Add(new OHLC(dateTime, dateTime, 
                            Convert.ToDouble(fieldDataSplit[2]),
                            Convert.ToDouble(fieldDataSplit[3]),
                            Convert.ToDouble(fieldDataSplit[4]),
                            Convert.ToDouble(fieldDataSplit[5])));
                    }
                }
            }

            return OHLCGranularityGroup(ohlcList); ;
        }

        private List<OHLC> ParseTickOHLC(string data)
        {
            var fileLineData = data.Split('\r');
            var ohlcList = new List<OHLC>();
            var bidAsks = new List<BidAsk>();

            foreach (var fld in fileLineData)
            {
                var fieldData = fld.Replace("\n", "");
                if (fieldData.Length > 0)
                {
                    var fieldDataSplit = fieldData.Split(' ');
                    if(fieldDataSplit.Length == 3)
                    {
                        int year = Convert.ToInt32(fieldDataSplit[0].Substring(0, 4));
                        int month = Convert.ToInt32(fieldDataSplit[0].Substring(4, 2));
                        int day = Convert.ToInt32(fieldDataSplit[0].Substring(6,2));
                        int hour = Convert.ToInt32(fieldDataSplit[1].Substring(0, 2));
                        int minute = Convert.ToInt32(fieldDataSplit[1].Substring(2, 2));
                        int second = Convert.ToInt32(fieldDataSplit[1].Substring(4, 2));
                        DateTime dateTime = new DateTime(year, month, day, hour, minute, second);

                        var priceLine = fieldDataSplit[2].Split(';');
                        if(priceLine.Length >= 4)
                        {
                            long volume = Convert.ToInt64(priceLine[0]);
                            double bid = Convert.ToDouble(priceLine[1]);
                            double ask = Convert.ToDouble(priceLine[2]);

                            bidAsks.Add(new BidAsk(dateTime, volume, bid, ask));
                        }
                    }
                }
            }
            return new List<OHLC>();
        }
    }
}