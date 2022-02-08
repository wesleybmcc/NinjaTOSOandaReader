using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ConsoleApp1.Model;

namespace NinjaTOSOandaReader
{
    public class OandaClient
    {
        private static string baseUrl = "https://api-fxpractice.oanda.com";

        public static PriceData LoadHistoricalData(string instrument, string periodName = "M", int periodValue = 15)
        {
            try
            {
                var granularity = string.Format("{0}{1}", periodName, periodValue);
                var url = string.Format("{0}/v3/instruments/{1}/candles?granularity={2}", baseUrl, instrument, granularity);
                var request = (HttpWebRequest)WebRequest.Create(url);

                string json = "";
                string credentialHeader = String.Format("Bearer 1aa2f86b2ce2b328b9a2c415e68c7ae3-cc0091d0eb4a28b72073d3e58fe99b3c");
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", credentialHeader);

                HttpWebResponse webresponse = (HttpWebResponse)request.GetResponse();

                var sw = new StreamReader(webresponse.GetResponseStream(), System.Text.Encoding.ASCII);
                json = sw.ReadToEnd();
                sw.Close();

                return ParseJson(instrument, periodName, periodValue, json);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw ex;
            }
        }

        private static PriceData ParseJson(string instrument, string periodName, int periodValue, string json)
        {
            var candlesIndex = json.IndexOf("[", json.IndexOf("candles"));
            var candles = json.Substring(candlesIndex);
            candles = candles.Remove(candles.Length - 1, 1);

            var priceData = new PriceData(instrument, periodName, periodValue);
            priceData.AddOHLC(ParseOHLC(candles));

            return priceData;
        }

        private static List<OHLC> ParseOHLC(string data)
        {
            var baseIndex = 0;
            var ohlcList = new List<OHLC>();

            while (true)
            {
                var firstIndex = data.IndexOf("{", baseIndex);
                var secondIndex = data.IndexOf("{", firstIndex + 1);
                var thirdIndex = data.IndexOf("}", secondIndex + 1);
                var fourthIndex = data.IndexOf("}", thirdIndex + 1);
                if (firstIndex < 0 || secondIndex < 0 || thirdIndex < 0 || fourthIndex < 0)
                {
                    break;
                }
                var rcd = data.Substring(firstIndex, (fourthIndex - firstIndex) + 1);
                var t1 = rcd.IndexOf(":", rcd.IndexOf("time"));
                var t2 = rcd.IndexOf(",", t1);
                var timeString = rcd.Substring(t1 + 1, t2 - t1 - 1).Replace("\"", "");
                var mid = rcd.Substring(rcd.IndexOf("{", t1)).Replace("\"", "").Replace("{", "").Replace("}", "").Split(',');

                var ohlc = new OHLC
                {
                    StartDateTime = DateTime.Parse(timeString),
                    EndDateTime = DateTime.Parse(timeString).AddMinutes(14),
                    Open = 0,
                    High = 0,
                    Low = 0,
                    Close = 0
                };
                ohlcList.Add(ohlc);

                foreach (var m in mid)
                {
                    var price = m.Split(':')[0];
                    var value = m.Split(':')[1];
                    if (price == "o")
                    {
                        ohlc.Open = Convert.ToDouble(value);
                    }
                    if (price == "h")
                    {
                        ohlc.High = Convert.ToDouble(value);
                    }
                    if (price == "l")
                    {
                        ohlc.Low = Convert.ToDouble(value);
                    }
                    if (price == "c")
                    {
                        ohlc.Close = Convert.ToDouble(value);
                    }
                }

                baseIndex = fourthIndex + 1;
            }

            return ohlcList;
        }
    }
}
