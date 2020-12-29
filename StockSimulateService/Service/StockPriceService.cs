using StockSimulateService.Service;
using StockSimulateDomain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateCore.Utils;
using StockSimulateService.Helper;
using StockSimulateCore.Data;

namespace StockSimulateService.Service
{
    public class StockPriceService 
    {
        public static void Update(StockEntity stock, StockInfo stockInfo)
        {
            var dealDate = DateTime.Now.ToString("yyyy-MM-dd");
            var price = Repository.Instance.QueryFirst<StockPriceEntity>($"StockCode='{stock.Code}' and DealDate='{dealDate}' and DateType=0");
            if (price == null)
            {
                stockInfo.DayPrice.DealTime = "";
                Repository.Instance.Insert<StockPriceEntity>(stockInfo.DayPrice);
            }
            else
            {
                stockInfo.DayPrice.ID = price.ID;
                stockInfo.DayPrice.DealTime = "";
                Repository.Instance.Update<StockPriceEntity>(stockInfo.DayPrice);
            }
            if (stock.Foucs > 0)
            {
                var dealTime = DateTime.Now.ToString("HH:mm");
                if (dealTime.CompareTo("15:00") >= 0) dealTime = "15:00";
                if (dealTime.CompareTo("09:25") <= 0) dealTime = "09:25";

                var price2 = Repository.Instance.QueryFirst<StockPriceEntity>($"StockCode='{stock.Code}' and DealDate='{dealDate}' and DealTime='{dealTime}' and DateType=1");
                if (price2 == null)
                {
                    stockInfo.DayPrice.DateType = 1;//分钟
                    stockInfo.DayPrice.DealTime = dealTime;
                    Repository.Instance.Insert<StockPriceEntity>(stockInfo.DayPrice);
                }
                else
                {
                    stockInfo.DayPrice.ID = price2.ID;
                    stockInfo.DayPrice.DateType = 1;//分钟
                    stockInfo.DayPrice.DealTime = dealTime;
                    Repository.Instance.Update<StockPriceEntity>(stockInfo.DayPrice);
                }
            }
        }

        /// <summary>
        /// 每天执行一次,删除上一天的分时数据
        /// </summary>
        public static void Clear()
        {
            var dealDate = DateTime.Now.ToString("yyyy-MM-dd");
            var lastDate = Repository.Instance.QueryFirst<StockPriceEntity>($"DateType=1", "ID Desc");
            if (lastDate != null)
            {
                dealDate = lastDate.DealDate;
            }
            Repository.Instance.Delete<StockPriceEntity>($"DateType=1 and DealDate<'{dealDate}'");
        }

        /// <summary>
        /// 计算账户盈亏
        /// </summary>
        /// <param name="actionLog"></param>
        public static void CalculateProfit(Action<string> actionLog)
        {
            var stocks = Repository.Instance.QueryAll<StockEntity>();
            var accountStocks = Repository.Instance.QueryAll<AccountStockEntity>();
            var accounts = Repository.Instance.QueryAll<AccountEntity>();

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
            Repository.Instance.Update<AccountStockEntity>(accountStocks);

            foreach(var account in accounts)
            {
                var accStocks = accountStocks.Where(c => c.AccountName == account.Name).ToArray();
                account.HoldAmount = accStocks.Sum(c => c.HoldAmount);
                account.Profit = accStocks.Sum(c => c.Profit);
                account.TotalAmount = account.Amount + account.Profit;
            }
            Repository.Instance.Update<AccountEntity>(accounts);

            if (stocks.Length > 0) actionLog($">------------------------------------------------>");
        }

        public static void CalculateProfit(string accountName, string stockCode, decimal stockPrice)
        {
            var accountStock = Repository.Instance.QueryFirst<AccountStockEntity>($"AccountName='{accountName}' and StockCode='{stockCode}'");
            if (accountStock == null) return;

            accountStock.Price = stockPrice;
            accountStock.HoldAmount = accountStock.Price * accountStock.HoldQty;
            accountStock.Profit = accountStock.HoldAmount - accountStock.TotalAmount;
            if (accountStock.TotalAmount == 0) accountStock.UDPer = 0;
            else accountStock.UDPer = Math.Round(accountStock.Profit / accountStock.HoldAmount * 100, 2);

            Repository.Instance.Update<AccountStockEntity>(accountStock);
        }

        public static void CalculateNowAvgrage(Action<string> actionLog)
        {
            var dealDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            var lastPrice = Repository.Instance.QueryAll<StockPriceEntity>($"DateType=0", "DealDate desc", 1).FirstOrDefault();
            if (lastPrice != null && lastPrice.DealDate != DateTime.Now.Date.ToString("yyyy-MM-dd")) dealDate = lastPrice.DealDate;

            var newStockAverages = new List<StockAverageEntity>();
            var stocks = Repository.Instance.QueryAll<StockEntity>();
            var stockAverages = Repository.Instance.QueryAll<StockAverageEntity>($"DealDate='{dealDate}'");

            var yestDate = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd");
            var yestPrice = Repository.Instance.QueryAll<StockPriceEntity>($"DateType=0 and DealDate<'{dealDate}'", "DealDate desc", 1).FirstOrDefault();
            if (yestPrice != null) yestDate = yestPrice.DealDate;

            var nextAverages = Repository.Instance.QueryAll<StockAverageEntity>($"DealDate='{yestDate}'");
            foreach (var stock in stocks)
            {
                var decNum = stock.Type == 0 ? 2 : 3;

                var stockPrices = Repository.Instance.QueryAll<StockPriceEntity>($"StockCode='{stock.Code}' and DateType=0", "DealDate desc", 250, new string[] { "StockCode", "DealDate", "Price"});
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
                    stock.Trend5 = GetSingleTrend(stock.Trend5, stockAverage.AvgPrice5, yestAvgPrice.AvgPrice5);
                    stock.Trend10 = GetSingleTrend(stock.Trend10, stockAverage.AvgPrice10, yestAvgPrice.AvgPrice10);
                    stock.Trend20 = GetSingleTrend(stock.Trend20, stockAverage.AvgPrice20, yestAvgPrice.AvgPrice20);
                    stock.Trend60 = GetSingleTrend(stock.Trend60, stockAverage.AvgPrice60, yestAvgPrice.AvgPrice60);
                    stock.Trend120 = GetSingleTrend(stock.Trend120, stockAverage.AvgPrice120, yestAvgPrice.AvgPrice120);
                    stock.Trend250 = GetSingleTrend(stock.Trend250, stockAverage.AvgPrice250, yestAvgPrice.AvgPrice250);
                }
                //计算当前趋势
                stock.Trend = GetTrend(stock, stockAverage, yestAvgPrice);
            }
            Repository.Instance.Update<StockEntity>(stocks, new string[] { "Trend", "AvgPrice5", "AvgPrice10", "AvgPrice20", "AvgPrice60", "AvgPrice120", "AvgPrice250", "LastAvgPrice5", "LastAvgPrice10", "LastAvgPrice20", "LastAvgPrice60", "LastAvgPrice120", "LastAvgPrice250" });
            Repository.Instance.Update<StockAverageEntity>(stockAverages);
            Repository.Instance.Insert<StockAverageEntity>(newStockAverages.ToArray());
        }

        static string GetSingleTrend(string trend, decimal nowAverage, decimal yestAverage)
        {
            if (nowAverage == 0 || yestAverage == 0) return trend;

            if (nowAverage > yestAverage) trend = $"上{trend}";
            else if (nowAverage < yestAverage) trend = $"下{trend}";
            else trend = $"平{trend}";

            if (trend.Length > 5) trend = trend.Substring(0, 5);
            return trend;
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
            var stocks = Repository.Instance.QueryAll<StockEntity>();
            var stockAverages = Repository.Instance.QueryAll<StockAverageEntity>();
            foreach (var stock in stocks)
            {
                var decNum = stock.Type == 0 ? 2 : 3;

                var stockPrices = Repository.Instance.QueryAll<StockPriceEntity>($"StockCode='{stock.Code}' and DateType=0", "DealDate desc", 0, new string[] { "StockCode", "DealDate", "Price" });
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

                var nowStockAverage = stockAverages.Where(c => c.StockCode == stock.Code).OrderByDescending(c => c.DealDate).FirstOrDefault();
                if (nowStockAverage != null)
                {
                    stock.AvgPrice5 = nowStockAverage.AvgPrice5;
                    stock.AvgPrice10 = nowStockAverage.AvgPrice10;
                    stock.AvgPrice20 = nowStockAverage.AvgPrice20;
                    stock.AvgPrice60 = nowStockAverage.AvgPrice60;
                    stock.AvgPrice120 = nowStockAverage.AvgPrice120;
                    stock.AvgPrice250 = nowStockAverage.AvgPrice250;
                }
                var allStockAverage = stockAverages.Where(c => c.StockCode == stock.Code).Concat(newStockAverages.Where(c => c.StockCode == stock.Code)).OrderByDescending(c => c.DealDate).Take(6).ToArray();
                if (allStockAverage.Length > 0)
                {
                    stock.Trend5 = GetSingleHidTrend(stock.Trend5, allStockAverage.Select(c => c.AvgPrice5).ToArray());
                    stock.Trend10 = GetSingleHidTrend(stock.Trend10, allStockAverage.Select(c => c.AvgPrice10).ToArray());
                    stock.Trend20 = GetSingleHidTrend(stock.Trend20, allStockAverage.Select(c => c.AvgPrice20).ToArray());
                    stock.Trend60 = GetSingleHidTrend(stock.Trend60, allStockAverage.Select(c => c.AvgPrice60).ToArray());
                    stock.Trend120 = GetSingleHidTrend(stock.Trend120, allStockAverage.Select(c => c.AvgPrice120).ToArray());
                    stock.Trend250 = GetSingleHidTrend(stock.Trend250, allStockAverage.Select(c => c.AvgPrice250).ToArray());
                }

                //计算当前趋势
                var yestStockAverage = stockAverages.Where(c => c.StockCode == stock.Code).OrderByDescending(c => c.DealDate).Skip(1).FirstOrDefault();
                stock.Trend = GetTrend(stock, nowStockAverage, yestStockAverage);
            }
            Repository.Instance.Update<StockEntity>(stocks, new string[] { "Trend", "AvgPrice5", "AvgPrice10", "AvgPrice20", "AvgPrice60", "AvgPrice120", "AvgPrice250", "Trend5", "Trend10", "Trend20", "Trend60", "Trend120", "Trend250" });
            Repository.Instance.Update<StockAverageEntity>(stockAverages);
            Repository.Instance.Insert<StockAverageEntity>(newStockAverages.ToArray());
        }

        static string GetSingleHidTrend(string trend, decimal[] averages)
        {
            var arr = averages.Reverse().ToArray();
            for (var i = 0; i < arr.Length; i++)
            {
                if (i + 1 < arr.Length)
                {
                    trend = GetSingleTrend(trend, arr[i + 1], arr[i]);
                }
            }
            return trend;
        }
    }
}
