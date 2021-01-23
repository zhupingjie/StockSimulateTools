using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockSimulateCore.Utils
{
    public class MacdUtil
    {
        public static PriceModel[] CalcEMA(int n, PriceModel[] prices)
        {
            var ema = new List<PriceModel>();
            if (prices.Length == 0) return ema.ToArray();
            ema.Add(prices.FirstOrDefault());

            var a = 2 / (decimal)(n + 1);
            for (var i = 1; i < prices.Length; i++)
            {
                ema.Add(new PriceModel()
                {
                    Date = prices[i].Date,
                    Price = a * prices[i].Price + (1 - a) * ema[i - 1].Price
                });
            }
            return ema.ToArray();
        }

        /*
         * 计算DIF快线，用于MACD
         * @param {number} short 快速EMA时间窗口
         * @param {number} long 慢速EMA时间窗口
         * @param {array} data 输入数据
         * @param {string} field 计算字段配置
         */
        public static PriceModel[] CalcDIF(int sht, int lng, PriceModel[] prices)
        {
            var dif =new List<PriceModel>();
            var emaShort = CalcEMA(sht, prices);
            var emaLong = CalcEMA(lng, prices);
            for (var i = 0; i < prices.Length; i++)
            {
                dif.Add(new PriceModel()
                {
                    Date = prices[i].Date,
                    Price = emaShort[i].Price - emaLong[i].Price
                });
            }
            return dif.ToArray();
        }

        /*
         * 计算DEA慢线，用于MACD
         * @param {number} mid 对dif的时间窗口
         * @param {array} dif 输入数据
         */
        public static PriceModel[] CalcDEA (int mid, PriceModel[] dif)
        {
            return CalcEMA(mid, dif);
        }

        /*
         * 计算MACD
         * @param {number} short 快速EMA时间窗口
         * @param {number} long 慢速EMA时间窗口
         * @param {number} mid dea时间窗口
         * @param {array} data 输入数据
         * @param {string} field 计算字段配置
         */
        public static MacdModel[] CalcMACD(int sht, int lng, int mid, PriceModel[] prices)
        {
            var result = new List<MacdModel>();
            var macd = new List<PriceModel>();
            var dif = CalcDIF(sht, lng, prices);
            var dea = CalcDEA(mid, dif);
            for (var i = 0; i < prices.Length; i++)
            {
                macd.Add(new PriceModel()
                {
                    Date = prices[i].Date,
                    Price = (dif[i].Price - dea[i].Price) * 2
                });
            }
            for (var i=0; i<dif.Length; i++)
            {
                result.Add(new MacdModel()
                {
                    Date = dif[i].Date,
                    DIF = dif[i].Price,
                    DEA = dea[i].Price,
                    MACD = macd[i].Price
                });
            }
            return result.ToArray();
        }
    }

    public class PriceModel
    { 
        public string Date { get; set; }

        public decimal Price { get; set; }
    }

    public class MacdModel
    {
        public string Date { get; set; }
        public decimal MACD { get; set; }
        public decimal DIF { get; set; }
        public decimal DEA { get; set; }
    }
}
