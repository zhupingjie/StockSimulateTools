using StockSimulateDomain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateCore.Utils;
using StockSimulateService.Helper;
using StockSimulateCore.Data;
using StockSimulateDomain.Model;
using StockSimulateCore.Config;

namespace StockSimulateService.Service
{
    public class StockPriceService 
    {
        public static void Update(StockEntity stock, StockInfo stockInfo)
        {
            var dealDate = stockInfo.DayPrice.DealDate;
            var price = Repository.Instance.QueryFirst<StockPriceEntity>($"StockCode='{stock.Code}' and DealDate='{dealDate}' and DateType=0");
            if (price == null)
            {
                var stockPrice = stockInfo.DayPrice;
                stockPrice.DealTime = "";
                Repository.Instance.Insert<StockPriceEntity>(stockPrice);
            }
            else
            {
                stockInfo.DayPrice.ID = price.ID;
                stockInfo.DayPrice.DealTime = "";
                Repository.Instance.Update<StockPriceEntity>(stockInfo.DayPrice);
            }
            if (false)
            {
                var dealTime = DateTime.Now.ToString("HH:mm");
                if (dealTime.CompareTo("15:01") >= 0) return;
                if (dealTime.CompareTo("09:14") <= 0) return;

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
                    Repository.Instance.Update<StockPriceEntity>(stockInfo.DayPrice, new string[] { "DealTime", "DateType", "Price", "UDPer" });
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

            //var lastAvgPrices = Repository.Instance.QueryAll<StockAverageEntity>($"StockCode='{RunningConfig.Instance.SHZSOfStockCode}'", "DealDate desc", RunningConfig.Instance.KeepStockAssistTargetDays);
            //if(lastAvgPrices.Length == RunningConfig.Instance.KeepStockAssistTargetDays)
            //{
            //    dealDate = lastAvgPrices.OrderBy(c => c.DealDate).FirstOrDefault().DealDate;

            //    Repository.Instance.Delete<StockAverageEntity>($"DealDate<'{dealDate}'");
            //    Repository.Instance.Delete<StockMacdEntity>($"DealDate<'{dealDate}'");
            //}
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
                    else item.UDPer = Math.Round(item.Profit / item.TotalAmount * 100, 2);

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


        /// <summary>
        /// 计算所有可计算的平均价格
        /// </summary>
        public static void CalculateAllAvgrage(int calculateDays = 1)
        {
            var newAvgs = new List<StockAverageEntity>();
            var updateAvgs = new List<StockAverageEntity>();

            var stocks = Repository.Instance.QueryAll<StockEntity>("", "Type asc, Seq asc");
            foreach (var stock in stocks)
            {
                var allStockPrices = Repository.Instance.QueryAll<StockPriceEntity>($"StockCode='{stock.Code}' and DateType=0", "ID asc", columns: new string[] { "StockCode", "DealDate", "Price" }, withNoLock: true);
                var stockPrices = allStockPrices.OrderByDescending(c => c.DealDate).ToArray();
                if(calculateDays > 0) stockPrices = stockPrices.Take(calculateDays).ToArray();
                var allStockAvgs = Repository.Instance.QueryAll<StockAverageEntity>($"StockCode='{stock.Code}'");
                foreach (var stockPrice in stockPrices)
                {
                    CalculateNowAvgrage(stock, stockPrices, stockPrice, allStockAvgs, newAvgs, updateAvgs);
                }
            }
            Repository.Instance.Insert<StockAverageEntity>(newAvgs.ToArray());
            Repository.Instance.Update<StockAverageEntity>(updateAvgs.ToArray());
        }

        public static void CalculateNowAvgrage(StockEntity stock, StockPriceEntity[] allStockPrices, StockPriceEntity stockPrice, StockAverageEntity[] stockAverages, List<StockAverageEntity> newAvgs, List<StockAverageEntity> updateAvgs)
        {
            var decNum = stock.Type == 0 ? 2 : 3;

            var stockPrices = allStockPrices.Where(c => c.DealDate.CompareTo(stockPrice.DealDate) <= 0).OrderByDescending(c => c.DealDate).ToArray();
            if (stockPrices.Length == 0) return;

            var lastStockPrice = stockPrices.FirstOrDefault();

            var stockAverage = stockAverages.FirstOrDefault(c => c.DealDate == stockPrice.DealDate);
            if (stockAverage == null)
            {
                stockAverage = new StockAverageEntity()
                {
                    StockCode = stock.Code,
                    Price = lastStockPrice.Price,
                    DealDate = stockPrice.DealDate,
                };
                newAvgs.Add(stockAverage);
            }
            else
            {
                updateAvgs.Add(stockAverage);
            }
            stockAverage.Price = lastStockPrice.Price;
            if (stockPrices.Length >= 250) stockAverage.AvgPrice250 = Math.Round(stockPrices.Take(250).Average(c => c.Price), decNum);
            if (stockPrices.Length >= 180) stockAverage.AvgPrice180 = Math.Round(stockPrices.Take(180).Average(c => c.Price), decNum);
            if (stockPrices.Length >= 120) stockAverage.AvgPrice120 = Math.Round(stockPrices.Take(120).Average(c => c.Price), decNum);
            if (stockPrices.Length >= 60) stockAverage.AvgPrice60 = Math.Round(stockPrices.Take(60).Average(c => c.Price), decNum);
            if (stockPrices.Length >= 30) stockAverage.AvgPrice30 = Math.Round(stockPrices.Take(30).Average(c => c.Price), decNum);
            if (stockPrices.Length >= 20) stockAverage.AvgPrice20 = Math.Round(stockPrices.Take(20).Average(c => c.Price), decNum);
            if (stockPrices.Length >= 10) stockAverage.AvgPrice10 = Math.Round(stockPrices.Take(10).Average(c => c.Price), decNum);
            if (stockPrices.Length > 0) stockAverage.AvgPrice5 = Math.Round(stockPrices.Take(5).Average(c => c.Price), decNum);
        }

        public static void CalculateAllMACD(int shortAvgDays = 9, int midAvgDays = 12, int longAvgDays = 26, string startDate = "")
        {
            var newMacds = new List<StockMacdEntity>();
            var updateMacds = new List<StockMacdEntity>();

            var stocks = Repository.Instance.QueryAll<StockEntity>();
            foreach(var stock in stocks)
            {
                if (string.IsNullOrEmpty(startDate)) startDate = stock.PriceDate;

                var stockPrices = Repository.Instance.QueryAll<StockPriceEntity>($"StockCode='{stock.Code}' and DateType=0 and DealDate>='{startDate}'").OrderBy(c => c.DealDate).ToArray();
                var extMacds = Repository.Instance.QueryAll<StockMacdEntity>($"StockCode='{stock.Code}'");

                CalculateNowMACD(stock, stockPrices, extMacds, newMacds, updateMacds, shortAvgDays, midAvgDays, longAvgDays, startDate);
            }
            Repository.Instance.Insert<StockMacdEntity>(newMacds.ToArray());
            Repository.Instance.Update<StockMacdEntity>(updateMacds.ToArray());
        }

        public static void CalculateNowMACD(StockEntity stock, StockPriceEntity[] stockPrices, StockMacdEntity[] extMacds, List<StockMacdEntity> newMacds, List<StockMacdEntity> updateMacds, int shortAvgDays = 9, int midAvgDays = 12, int longAvgDays = 26, string startDate = "")
        {
            var lastEMA12 = 0m;
            var lastEMA26 = 0m;
            var lastDEA = 0m;

            var lastExtMacd = extMacds.Where(c => c.DealDate.CompareTo(startDate) < 0).OrderByDescending(c => c.DealDate).FirstOrDefault();
            if(lastExtMacd != null)
            {
                lastEMA12 = lastExtMacd.EMA12;
                lastEMA26 = lastExtMacd.EMA26;
                lastDEA = lastExtMacd.DEA;
            }
            var nowDate = stockPrices.Max(c => c.DealDate);
            foreach (var price in stockPrices)
            {
                var ema12 = Math.Round(lastEMA12 * 11 / 13 + price.Price * 2 / 13, 3);
                var ema26 = Math.Round(lastEMA26 * 25 / 27 + price.Price * 2 / 27, 3);
                var dif = ema12 - ema26;
                var dea = Math.Round(lastDEA * 8 / 10 + dif * 2 / 10, 3);
                var macd = (dif - dea) * 2;
                var stockMacd = extMacds.FirstOrDefault(c => c.DealDate == price.DealDate);
                if (stockMacd == null)
                {
                    stockMacd = new StockMacdEntity()
                    {
                        StockCode = stock.Code,
                        DealDate = price.DealDate,
                        EMA12 = ema12,
                        EMA26 = ema26,
                        DIF = dif,
                        DEA = dea,
                        MACD = macd,
                    };
                    newMacds.Add(stockMacd);
                }
                else if(price.DealDate == nowDate)
                {
                    stockMacd.EMA12 = ema12;
                    stockMacd.EMA26 = ema26;
                    stockMacd.DEA = dea;
                    stockMacd.DIF = dif;
                    stockMacd.MACD = macd;
                    updateMacds.Add(stockMacd);
                }
                lastEMA12 = ema12;
                lastEMA26 = ema26;
                lastDEA = dea;
            }
        }
    }
}
