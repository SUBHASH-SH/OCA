using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCA
{

    public class Model
    {
        public int strikePrice { get; set; }
        public string expiryDate { get; set; }
        public string underlying { get; set; }
        public string identifier { get; set; }
        public double openInterest { get; set; }
        public double changeinOpenInterest { get; set; }
        public double pchangeinOpenInterest { get; set; }
        public int totalTradedVolume { get; set; }
        public int impliedVolatility { get; set; }
        public double lastPrice { get; set; }
        public double change { get; set; }
        public double pChange { get; set; }
        public int totalBuyQuantity { get; set; }
        public int totalSellQuantity { get; set; }
        public int bidQty { get; set; }
        public double bidprice { get; set; }
        public int askQty { get; set; }
        public double askPrice { get; set; }
        public double underlyingValue { get; set; }
        public int totOI { get; set; }
        public int totVol { get; set; }
    }

    public class Datum
    {
        public int strikePrice { get; set; }
        public string expiryDate { get; set; }
        public PE PE { get; set; }
        public CE CE { get; set; }
    }


    public class Filtered
    {
        public int strikePrice { get; set; }
        public string expiryDate { get; set; }
        public CE CE { get; set; }
        public PE PE { get; set; }
    }

    public class PE
    {
        public int strikePrice { get; set; }
        public string expiryDate { get; set; }
        public string underlying { get; set; }
        public string identifier { get; set; }
        public double openInterest { get; set; }
        public double changeinOpenInterest { get; set; }
        public double pchangeinOpenInterest { get; set; }
        public int totalTradedVolume { get; set; }
        public double impliedVolatility { get; set; }
        public double lastPrice { get; set; }
        public double change { get; set; }
        public double pChange { get; set; }
        public int totalBuyQuantity { get; set; }
        public int totalSellQuantity { get; set; }
        public int bidQty { get; set; }
        public double bidprice { get; set; }
        public int askQty { get; set; }
        public double askPrice { get; set; }
        public double underlyingValue { get; set; }
        public int totOI { get; set; }
        public int totVol { get; set; }
    }

    public class CE
    {
        public int strikePrice { get; set; }
        public string expiryDate { get; set; }
        public string underlying { get; set; }
        public string identifier { get; set; }
        public double openInterest { get; set; }
        public double changeinOpenInterest { get; set; }
        public double pchangeinOpenInterest { get; set; }
        public int totalTradedVolume { get; set; }
        public double impliedVolatility { get; set; }
        public double lastPrice { get; set; }
        public double change { get; set; }
        public double pChange { get; set; }
        public int totalBuyQuantity { get; set; }
        public int totalSellQuantity { get; set; }
        public int bidQty { get; set; }
        public double bidprice { get; set; }
        public int askQty { get; set; }
        public double askPrice { get; set; }
        public double underlyingValue { get; set; }
        public int totOI { get; set; }
        public int totVol { get; set; }
    }

    public class Records
    {
        public List<string> expiryDates { get; set; }
        public List<Datum> data { get; set; }
        public string timestamp { get; set; }
        public double underlyingValue { get; set; }
        public List<int> strikePrices { get; set; }
    }

    public class TimeStamp 
    {
        public string timestamp { get; set; }
    }

    public class UnderLyingValue
    {
        public string timestamp { get; set; }
    }

    public class Root
    {
        public Records records { get; set; }
        public Filtered filtered { get; set; }
    }


}
