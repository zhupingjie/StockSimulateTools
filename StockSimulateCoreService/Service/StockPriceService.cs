using StockSimulateDomain.Entity;
using StockSimulateCoreService.Config;
using StockSimulateCoreService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateService.Helper;

namespace StockSimulateCoreService.Serivce
{
    public class StockPriceService 
    {
        public static void Update(StockEntity stock, StockInfo stockInfo)
        {
            var dealDate = DateTime.Now.ToString("yyyy-MM-dd");
            var price = MySQLDBUtil.Instance.QueryFirst<StockPriceEntity>($"StockCode='{stock.Code}' and DealDate='{dealDate}' and DateType=0");
            if (price == null)
            {
                stockInfo.DayPrice.DealTime = "";
                MySQLDBUtil.Instance.Insert<StockPriceEntity>(stockInfo.DayPrice);
            }
            else
            {
                stockInfo.DayPrice.ID = price.ID;
                stockInfo.DayPrice.DealTime = "";
                MySQLDBUtil.Instance.Update<StockPriceEntity>(stockInfo.DayPrice);
            }
            if (stock.Foucs > 0)
            {
                var dealTime = DateTime.Now.ToString("HH:mm");
                if (dealTime.CompareTo("15:00") >= 0) dealTime = "15:00";
                if (dealTime.CompareTo("09:25") <= 0) dealTime = "09:25";

                var price2 = MySQLDBUtil.Instance.QueryFirst<StockPriceEntity>($"StockCode='{stock.Code}' and DealDate='{dealDate}' and DealTime='{dealTime}' and DateType=1");
                if (price2 == null)
                {
                    stockInfo.DayPrice.DateType = 1;//分钟
                    stockInfo.DayPrice.DealTime = dealTime;
                    MySQLDBUtil.Instance.Insert<StockPriceEntity>(stockInfo.DayPrice);
                }
                else
                {
                    stockInfo.DayPrice.ID = price2.ID;
                    stockInfo.DayPrice.DateType = 1;//分钟
                    stockInfo.DayPrice.DealTime = dealTime;
                    MySQLDBUtil.Instance.Update<StockPriceEntity>(stockInfo.DayPrice);
                }
            }
        }

        /// <summary>
        /// 计算账户盈亏
        /// </summary>
        /// <param name="actionLog"></param>
        public static void CalculateProfit(Action<string> actionLog)
        {
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>();
            var accountStocks = MySQLDBUtil.Instance.QueryAll<AccountStockEntity>();
            foreach (var stock in stocks)
            {
                var accStocks = accountStocks.Where(c => c.StockCode == stock.Code).ToArray();
                foreach (var item in accStocks)
                {
                    item.Price = stock.Price;
                    item.HoldAmount = item.Price * item.HoldQty;
                    item.Profit = item.HoldAmount - item.TotalAmount;

                    if (item.TotalAmount == 0 || item.HoldAmount == 0) item.UDPer = 0;
                    else item.UDPer = Math.Round(item.Profit / item.HoldAmount * 100, 2);

                    actionLog($"已计算[{item.StockName}]持有股价盈亏...[{item.Profit}] [{item.UDPer}%]");
                }
            }
            MySQLDBUtil.Instance.Update<AccountStockEntity>(accountStocks);

            if (stocks.Length > 0) actionLog($">------------------------------------------------>");
        }

        public static void CalculateProfit(string accountName, string stockCode, decimal stockPrice)
        {
            var account = MySQLDBUtil.Instance.QueryFirst<AccountStockEntity>($"AccountName='{accountName}' and StockCode='{stockCode}'");
            if (account == null) return;

            account.Price = stockPrice;
            account.HoldAmount = account.Price * account.HoldQty;
            account.Profit = account.HoldAmount - account.TotalAmount;
            if (account.TotalAmount == 0) account.UDPer = 0;
            else account.UDPer = Math.Round(account.Profit / account.HoldAmount * 100, 2);

            MySQLDBUtil.Instance.Update<AccountStockEntity>(account);
        }

        public static void CalculateAvgrage(Action<string> actionLog)
        {
            var dealDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            var lastPrice = MySQLDBUtil.Instance.QueryAll<StockPriceEntity>($"DateType=0", "DealDate desc", 1).FirstOrDefault();
            if (lastPrice != null && lastPrice.DealDate != DateTime.Now.Date.ToString("yyyy-MM-dd")) dealDate = lastPrice.DealDate;

            var newStockAverages = new List<StockAverageEntity>();
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>();
            var stockAverages = MySQLDBUtil.Instance.QueryAll<StockAverageEntity>($"DealDate='{dealDate}'");
            foreach (var stock in stocks)
            {
                var decNum = stock.Type == 0 ? 2 : 3;

                var stockPrices = MySQLDBUtil.Instance.QueryAll<StockPriceEntity>($"StockCode='{stock.Code}' and DateType=0", "DealDate desc", 60, new string[] { "StockCode", "DealDate", "Price"});
                if (stockPrices.Length == 0) continue;

                var lastStockPrice = stockPrices.FirstOrDefault();

                var stockAverage = stockAverages.FirstOrDefault(c => c.StockCode == stock.Code);
                if (stockAverage == null)
                {
                    stockAverage = new StockAverageEntity()
                    {
                        StockCode = stock.Code,
                        Price = lastStockPrice.Price,
                        DealDate = dealDate,
                    };
                    newStockAverages.Add(stockAverage);
                }
                //if (stockPrices.Length >= 360) stockAverage.AvgPrice360 = Math.Round(stockPrices.Average(c => c.Price), decNum);
                //if (stockPrices.Length >= 180) stockAverage.AvgPrice180 = Math.Round(stockPrices.Take(180).Average(c => c.Price), decNum);
                //if (stockPrices.Length >= 120) stockAverage.AvgPrice120 = Math.Round(stockPrices.Take(120).Average(c => c.Price), decNum);
                if (stockPrices.Length >= 60) stockAverage.AvgPrice60 = Math.Round(stockPrices.Take(60).Average(c => c.Price), decNum);
                if (stockPrices.Length >= 20) stockAverage.AvgPrice20 = Math.Round(stockPrices.Take(20).Average(c => c.Price), decNum);
                if (stockPrices.Length >= 10) stockAverage.AvgPrice10 = Math.Round(stockPrices.Take(10).Average(c => c.Price), decNum);
                if (stockPrices.Length > 0) stockAverage.AvgPrice5 = Math.Round(stockPrices.Take(5).Average(c => c.Price), decNum);

                stock.AvgPrice5 = stockAverage.AvgPrice5;
                stock.AvgPrice10 = stockAverage.AvgPrice10;
                stock.AvgPrice20 = stockAverage.AvgPrice20;
                stock.AvgPrice60 = stockAverage.AvgPrice60;
                stock.Trend = GetTrend(stock, stockAverage);
            }
            MySQLDBUtil.Instance.Update<StockEntity>(stocks);
            MySQLDBUtil.Instance.Update<StockAverageEntity>(stockAverages);
            MySQLDBUtil.Instance.Insert<StockAverageEntity>(newStockAverages.ToArray());
        }

        /// <summary>
        /// ↘↗
        /// </summary>
        /// <param name="stock"></param>
        /// <param name="stockAverage"></param>
        /// <returns></returns>
        static string GetTrend(StockEntity stock, StockAverageEntity stockAverage)
        {
            var lng = "";
            if (stockAverage.AvgPrice60 > 0)
            {
                if (stock.Price > stockAverage.AvgPrice60)
                {
                    lng = "";
                }
                else if (stock.Price < stockAverage.AvgPrice60)
                {
                    lng = "L.离场";
                }
            }
            var mid = "";
            if (stockAverage.AvgPrice20 > 0)
            {
                if (stockAverage.AvgPrice5 > stockAverage.AvgPrice20)
                {
                    mid = "M.突破";
                }
                else if (stockAverage.AvgPrice5 < stockAverage.AvgPrice20)
                {
                    mid = "M.破位";
                }
            }
            var sht = "";
            if(stockAverage.AvgPrice10 > 0)
            {
                if (stockAverage.AvgPrice5 > stockAverage.AvgPrice10)
                {
                    if(stock.Price < stockAverage.AvgPrice5)
                    {
                        mid = "S.下探";
                    }
                    else
                    {
                        mid = "S.上攻";
                    }
                }
                else if (stockAverage.AvgPrice5 < stockAverage.AvgPrice10)
                {
                    if (stock.Price < stockAverage.AvgPrice5)
                    {
                        mid = "S.杀跌";
                    }
                    else
                    {
                        mid = "S.反弹";
                    }
                }
            }
            var tdy = "";
            if (stock.Price < stockAverage.AvgPrice5)
            {
                tdy = "N.↘";
            }
            else if (stock.Price > stockAverage.AvgPrice5)
            {
                tdy = "N.↗";
            }
            else
            {
                tdy = "N.→";
            }
            if (!string.IsNullOrEmpty(lng))
                return lng;
            return $"{tdy} {sht} {mid}";
        }
    }
}
