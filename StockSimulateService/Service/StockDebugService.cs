using StockSimulateDomain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateCore.Utils;
using StockSimulateCore.Data;
using System.Threading;

namespace StockSimulateService.Service
{
    public class StockDebugService
    {
        public static void MakeCheckStrategyRun(string accountName, string stockCode, decimal stockPrice, DateTime dealTime)
        {
            var stock = Repository.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null) return;

            var price = new StockPriceEntity()
            {
                StockCode = stockCode,
                Price = stockPrice,
                DealDate = dealTime.ToString("yyyy-MM-dd"),
                DealTime = dealTime.ToString("HH:mm"),
                DateType = 1,
                TodayEndPrice = stockPrice,
                TodayMaxPrice = stockPrice,
                TodayMinPrice = stockPrice,
                TodayStartPrice = stockPrice,
                Debug = 1
            };
            price.UDPer = Math.Round((stockPrice - stock.Price) / stock.Price * 100, stock.Type == 0 ? 2 : 3);
            Repository.Instance.Insert<StockPriceEntity>(price);

            StockStrategyService.CheckRun(stockCode, stockPrice, dealTime, accountName);
            StockPriceService.CalculateProfit(accountName, stockCode, stockPrice);
        }

        public static void ClearDeugData(string accountName, string stockCode)
        {
            Repository.Instance.Delete<StockPriceEntity>($"Debug=1");
            Repository.Instance.Delete<ExchangeOrderEntity>($"AccountName='{accountName}'");
            Repository.Instance.Delete<AccountStockEntity>($"AccountName='{accountName}'");
            Repository.Instance.Delete<StockStrategyEntity>($"AccountName='{accountName}' and StockCode='{stockCode}'");

            var account = Repository.Instance.QueryFirst<AccountEntity>($"Name='{accountName}'");
            if (account == null) return;

            account.BuyAmount = 0;
            account.Cash = account.Amount;
            account.TotalAmount = account.Amount;
            account.HoldAmount = 0;
            account.Profit = 0;
            Repository.Instance.Update<AccountEntity>(account);
        }

        public static void Test()
        {
            Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    try
                    {
                        var stocks = Repository.Instance.QueryAll<StockEntity>($"Type=0", withNoLock:true);
                        foreach(var stock in stocks)
                        {
                            stock.ROE = 0;
                            Repository.Instance.Update<StockEntity>(stock, new string[] { "ROE" });
                        }
                        Console.WriteLine($"Thd{Thread.CurrentThread.ManagedThreadId} {(i + 1).ToString().PadLeft(4, '0')} stock... {stocks.Max(x=>x.LastDate).ToString("yyyy-MM-dd HH:mm:ss")}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Thd{Thread.CurrentThread.ManagedThreadId} {(i + 1).ToString().PadLeft(4, '0')} stock... Error:{ex.Message}");
                    }
                }
            });

            Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    try
                    {
                        var reminds = Repository.Instance.QueryAll<RemindEntity>($"Handled=0");
                        foreach (var remind in reminds)
                        {
                            remind.Handled = 0;
                            Repository.Instance.Update<RemindEntity>(remind, new string[] { "Handled" });
                        }
                        Console.WriteLine($"Thd{Thread.CurrentThread.ManagedThreadId} {(i + 1).ToString().PadLeft(4, '0')} remind... {reminds.Max(x => x.LastDate).ToString("yyyy-MM-dd HH:mm:ss")}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Thd{Thread.CurrentThread.ManagedThreadId} {(i + 1).ToString().PadLeft(4, '0')} remind... Error:{ex.Message}");
                    }
                }
            });
        }
    }
}
