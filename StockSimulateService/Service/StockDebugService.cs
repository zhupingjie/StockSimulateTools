using StockSimulateDomain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateService.Utils;

namespace StockSimulateService.Service
{
    public class StockDebugService
    {
        public static void MakeCheckStrategyRun(string accountName, string stockCode, decimal stockPrice, DateTime dealTime)
        {
            var stock = MySQLDBUtil.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
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
            MySQLDBUtil.Instance.Insert<StockPriceEntity>(price);

            StockStrategyService.CheckRun(stockCode, stockPrice, dealTime, accountName);
            StockPriceService.CalculateProfit(accountName, stockCode, stockPrice);
        }

        public static void ClearDeugData(string accountName, string stockCode)
        {
            MySQLDBUtil.Instance.Delete<StockPriceEntity>($"Debug=1");
            MySQLDBUtil.Instance.Delete<ExchangeOrderEntity>($"AccountName='{accountName}'");
            MySQLDBUtil.Instance.Delete<AccountStockEntity>($"AccountName='{accountName}'");
            MySQLDBUtil.Instance.Delete<StockStrategyEntity>($"AccountName='{accountName}' and StockCode='{stockCode}'");

            var account = MySQLDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{accountName}'");
            if (account == null) return;

            account.BuyAmount = 0;
            account.Cash = account.Amount;
            account.TotalAmount = account.Amount;
            account.HoldAmount = 0;
            account.Profit = 0;
            MySQLDBUtil.Instance.Update<AccountEntity>(account);
        }
    }
}
