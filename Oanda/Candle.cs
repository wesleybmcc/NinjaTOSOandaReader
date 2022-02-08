using System;
using System.Collections.Generic;

namespace NinjaTOSOandaReader
{
    //public class Candlex
    //{
    //    public string Instrument { get; set; }
    //    public string Granularity { get; set; }
    //    public IList<OHLCx> OHLCx { get; set; }

    //    public static Candlex Create(string json)
    //    {
    //        var candle = new Candlex();

    //        var instrumentIndex = json.IndexOf("instrument");
    //        var endInstrumentIndex = json.IndexOf(",", instrumentIndex);
    //        candle.Instrument = json.Substring(instrumentIndex, endInstrumentIndex - instrumentIndex).Replace("\"", "").Split(':')[1];

    //        var granularityIndex = json.IndexOf("granularity");
    //        var endGranularityIndex = json.IndexOf(",", granularityIndex);
    //        candle.Granularity = json.Substring(granularityIndex, endGranularityIndex - granularityIndex).Replace("\"", "").Split(':')[1];

    //        var candlesIndex = json.IndexOf("[", json.IndexOf("candles"));
    //        var candles = json.Substring(candlesIndex);
    //        candles = candles.Remove(candles.Length - 1, 1);

    //        candle.OHLCx = Parse(candles);

    //        return candle;
    //    }

    //    private static IList<OHLCx> Parse(string candles)
    //    {
    //        var baseIndex = 0;
    //        var ohlcList = new List<OHLCx>();

    //        while (true)
    //        {
    //            var firstIndex = candles.IndexOf("{", baseIndex);
    //            var secondIndex = candles.IndexOf("{", firstIndex + 1);
    //            var thirdIndex = candles.IndexOf("}", secondIndex + 1);
    //            var fourthIndex = candles.IndexOf("}", thirdIndex + 1);
    //            if (firstIndex < 0 || secondIndex < 0 || thirdIndex < 0 || fourthIndex < 0)
    //            {
    //                break;
    //            }
    //            var rcd = candles.Substring(firstIndex, (fourthIndex - firstIndex) + 1);
    //            var t1 = rcd.IndexOf(":", rcd.IndexOf("time"));
    //            var t2 = rcd.IndexOf(",", t1);
    //            var timeString = rcd.Substring(t1 + 1, t2 - t1 - 1).Replace("\"", "");
    //            var mid = rcd.Substring(rcd.IndexOf("{", t1)).Replace("\"", "").Replace("{", "").Replace("}", "").Split(',');

    //            OHLC ohlc = new OHLC();
    //            ohlc.Time = DateTime.Parse(timeString);
    //            ohlcList.Add(ohlc);

    //            foreach (var m in mid)
    //            {
    //                var price = m.Split(':')[0];
    //                var value = m.Split(':')[1];
    //                if (price == "o")
    //                {
    //                    ohlc.Open = Convert.ToDouble(value);
    //                }
    //                if (price == "h")
    //                {
    //                    ohlc.High = Convert.ToDouble(value);
    //                }
    //                if (price == "l")
    //                {
    //                    ohlc.Low = Convert.ToDouble(value);
    //                }
    //                if (price == "c")
    //                {
    //                    ohlc.Close = Convert.ToDouble(value);
    //                }
    //            }
    //            baseIndex = fourthIndex + 1;
    //        }

    //        return ohlcList;
    //    }
    //}

    //public class OHLCx
    //{
    //    public DateTime Time { get; set; }
    //    public double Open { get; set; }
    //    public double High { get; set; }
    //    public double Low { get; set; }
    //    public double Close { get; set; }
    //}
}
