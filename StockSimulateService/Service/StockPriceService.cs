using StockSimulateService.Service;
using StockSimulateDomain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateService.Utils;
using StockSimulateService.Helper;

namespace StockSimulateService.Service
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
            var accounts = MySQLDBUtil.Instance.QueryAll<AccountEntity>();

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

            foreach(var account in accounts)
            {
                var accStocks = accountStocks.Where(c => c.AccountName == account.Name).ToArray();
                account.HoldAmount = accStocks.Sum(c => c.HoldAmount);
                account.Profit = accStocks.Sum(c => c.Profit);
                account.TotalAmount = account.Amount + account.Profit;
            }
            MySQLDBUtil.Instance.Update<AccountEntity>(accounts);

            if (stocks.Length > 0) actionLog($">------------------------------------------------>");
        }

        public static void CalculateProfit(string accountName, string stockCode, decimal stockPrice)
        {
            var accountStock = MySQLDBUtil.Instance.QueryFirst<AccountStockEntity>($"AccountName='{accountName}' and StockCode='{stockCode}'");
            if (accountStock == null) return;

            accountStock.Price = stockPrice;
            accountStock.HoldAmount = accountStock.Price * accountStock.HoldQty;
            accountStock.Profit = accountStock.HoldAmount - accountStock.TotalAmount;
            if (accountStock.TotalAmount == 0) accountStock.UDPer = 0;
            else accountStock.UDPer = Math.Round(accountStock.Profit / accountStock.HoldAmount * 100, 2);

            MySQLDBUtil.Instance.Update<AccountStockEntity>(accountStock);
        }

        public static void CalculateNowAvgrage(Action<string> actionLog)
        {
            var dealDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            var lastPrice = MySQLDBUtil.Instance.QueryAll<StockPriceEntity>($"DateType=0", "DealDate desc", 1).FirstOrDefault();
            if (lastPrice != null && lastPrice.DealDate != DateTime.Now.Date.ToString("yyyy-MM-dd")) dealDate = lastPrice.DealDate;

            var newStockAverages = new List<StockAverageEntity>();
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>();
            var stockAverages = MySQLDBUtil.Instance.QueryAll<StockAverageEntity>($"DealDate='{dealDate}'");

            var yestDate = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd");
            var yestPrice = MySQLDBUtil.Instance.QueryAll<StockPriceEntity>($"DateType=0 and DealDate<'{dealDate}'", "DealDate desc", 1).FirstOrDefault();
            if (yestPrice != null) yestDate = yestPrice.DealDate;

            var nextAverages = MySQLDBUtil.Instance.QueryAll<StockAverageEntity>($"DealDate='{yestDate}'");
            foreach (var stock in stocks)
            {
                var decNum = stock.Type == 0 ? 2 : 3;

                var stockPrices = MySQLDBUtil.Instance.QueryAll<StockPriceEntity>($"StockCode='{stock.Code}' and DateType=0", "DealDate desc", 250, new string[] { "StockCode", "DealDate", "Price"});
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
                stockAverage.Price = lastStockPrice.Price;
                if (stockPrices.Length >= 250) stockAverage.AvgPrice250 = Math.Round(stockPrices.Take(250).Average(c => c.Price), decNum);
                if (stockPrices.Length >= 120) stockAverage.AvgPrice120 = Math.Round(stockPrices.Take(120).Average(c => c.Price), decNum);
                if (stockPrices.Length >= 60) stockAverage.AvgPrice60 = Math.Round(stockPrices.Take(60).Average(c => c.Price), decNum);
                if (stockPrices.Length >= 20) stockAverage.AvgPrice20 = Math.Round(stockPrices.Take(20).Average(c => c.Price), decNum);
                if (stockPrices.Length >= 10) stockAverage.AvgPrice10 = Math.Round(stockPrices.Take(10).Average(c => c.Price), decNum);
                if (stockPrices.Length > 0) stockAverage.AvgPrice5 = Math.Round(stockPrices.Take(5).Average(c => c.Price), decNum);

                stock.AvgPrice5 = stockAverage.AvgPrice5;
                stock.AvgPrice10 = stockAverage.AvgPrice10;
                stock.AvgPrice20 = stockAverage.AvgPrice20;
                stock.AvgPrice60 = stockAverage.AvgPrice60;
                stock.AvgPrice120 = stockAverage.AvgPrice120;
                stock.AvgPrice250 = stockAverage.AvgPrice250;

                var yestAvgPrice = nextAverages.FirstOrDefault(c => c.StockCode == stock.Code);
                if(yestAvgPrice != null)
                {
                    stock.LastAvgPrice5 = yestAvgPrice.AvgPrice5;
                    stock.LastAvgPrice10 = yestAvgPrice.AvgPrice10;
                    stock.LastAvgPrice20 = yestAvgPrice.AvgPrice20;
                    stock.LastAvgPrice60 = yestAvgPrice.AvgPrice60;
                    stock.LastAvgPrice120 = yestAvgPrice.AvgPrice120;
                    stock.LastAvgPrice250 = yestAvgPrice.AvgPrice250;
                }
                //计算当前趋势
                stock.Trend = GetTrend(stock, stockAverage, yestAvgPrice);
            }
            MySQLDBUtil.Instance.Update<StockEntity>(stocks, new string[] { "Trend", "AvgPrice5", "AvgPrice10", "AvgPrice20", "AvgPrice60", "AvgPrice120", "AvgPrice250", "LastAvgPrice5", "LastAvgPrice10", "LastAvgPrice20", "LastAvgPrice60", "LastAvgPrice120", "LastAvgPrice250" });
            MySQLDBUtil.Instance.Update<StockAverageEntity>(stockAverages);
            MySQLDBUtil.Instance.Insert<StockAverageEntity>(newStockAverages.ToArray());
        }

        /// <summary>
        /// ↘↗
        /// </summary>
        /// <param name="stock"></param>
        /// <param name="nowAverage"></param>
        /// <returns></returns>
        static string GetTrend(StockEntity stock, StockAverageEntity nowAverage, StockAverageEntity yestAvgPrices)
        {
            if (yestAvgPrices == null) return "";

            var lng = "";
            if (nowAverage.AvgPrice250 > 0)
            {
                if (nowAverage.AvgPrice250 > yestAvgPrices.AvgPrice250)
                {
                    lng = "L.上行";
                }
                else if (nowAverage.AvgPrice250 < yestAvgPrices.AvgPrice250)
                {
                    lng = "L.下行";
                }
            }
            var mid = "";
            if (nowAverage.AvgPrice120 > 0 && nowAverage.AvgPrice60 > 0)
            {
                if (nowAverage.AvgPrice120 > yestAvgPrices.AvgPrice120 && nowAverage.AvgPrice60 > yestAvgPrices.AvgPrice60)
                {
                    mid = "M.上行";
                }
                else if (nowAverage.AvgPrice120 < yestAvgPrices.AvgPrice120 && nowAverage.AvgPrice60 < yestAvgPrices.AvgPrice60)
                {
                    mid = "M.下行";
                }
                else
                {
                    mid = "M.震荡";
                }
            }
            var sht = "";
            if (nowAverage.AvgPrice20 > 0 && nowAverage.AvgPrice10 > 0 && nowAverage.AvgPrice5 > 0)
            {
                if (nowAverage.AvgPrice20 > yestAvgPrices.AvgPrice20 && nowAverage.AvgPrice10 > yestAvgPrices.AvgPrice10 && nowAverage.AvgPrice5 > yestAvgPrices.AvgPrice5)
                {
                    sht = "S.上行";
                }
                else if (nowAverage.AvgPrice20 < yestAvgPrices.AvgPrice20 && nowAverage.AvgPrice10 < yestAvgPrices.AvgPrice10 && nowAverage.AvgPrice5 < yestAvgPrices.AvgPrice5)
                {
                    sht = "S.下行";
                }
                else
                {
                    sht = "S.震荡";
                }
            }
            var tdy = "";
            if (stock.Price < nowAverage.AvgPrice5)
            {
                tdy = "N.↘";
            }
            else if (stock.Price > nowAverage.AvgPrice5)
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

        /// <summary>
        /// 计算所有可计算的平均价格
        /// </summary>
        public static void CalculateAllAvgrage()
        {
            var newStockAverages = new List<StockAverageEntity>();
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>();
            var stockAverages = MySQLDBUtil.Instance.QueryAll<StockAverageEntity>();
            foreach (var stock in stocks)
            {
                var decNum = stock.Type == 0 ? 2 : 3;

                var stockPrices = MySQLDBUtil.Instance.QueryAll<StockPriceEntity>($"StockCode='{stock.Code}' and DateType=0", "DealDate desc", 0, new string[] { "StockCode", "DealDate", "Price" });
                if (stockPrices.Length == 0) continue;

                foreach(var stockPrice in stockPrices)
                {
                    var calcPrices = stockPrices.Where(c => c.DealDate.CompareTo(stockPrice.DealDate) <= 0).OrderByDescending(c => c.DealDate).ToArray();

                    var stockAverage = stockAverages.FirstOrDefault(c => c.StockCode == stock.Code && c.DealDate == stockPrice.DealDate);
                    if (stockAverage == null)
                    {
                        stockAverage = new StockAverageEntity()
                        {
                            StockCode = stock.Code,
                            Price = stockPrice.Price,
                            DealDate = stockPrice.DealDate,
                        };
                        newStockAverages.Add(stockAverage);
                    }
                    stockAverage.Price = stockPrice.Price;
                    if (calcPrices.Length >= 250) stockAverage.AvgPrice250 = Math.Round(calcPrices.Take(250).Average(c => c.Price), decNum);
                    if (calcPrices.Length >= 120) stockAverage.AvgPrice120 = Math.Round(calcPrices.Take(120).Average(c => c.Price), decNum);
                    if (calcPrices.Length >= 60) stockAverage.AvgPrice60 = Math.Round(calcPrices.Take(60).Average(c => c.Price), decNum);
                    if (calcPrices.Length >= 20) stockAverage.AvgPrice20 = Math.Round(calcPrices.Take(20).Average(c => c.Price), decNum);
                    if (calcPrices.Length >= 10) stockAverage.AvgPrice10 = Math.Round(calcPrices.Take(10).Average(c => c.Price), decNum);
                    if (calcPrices.Length > 0) stockAverage.AvgPrice5 = Math.Round(calcPrices.Take(5).Average(c => c.Price), decNum);
                }
            }
            MySQLDBUtil.Instance.Update<StockAverageEntity>(stockAverages);
            MySQLDBUtil.Instance.Insert<StockAverageEntity>(newStockAverages.ToArray());
        }
    }
}
