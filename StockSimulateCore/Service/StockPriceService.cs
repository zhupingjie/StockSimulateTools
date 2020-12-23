using StockSimulateCore.Config;
using StockSimulateCore.Entity;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Service
{
    public class StockPriceService 
    {
        public static void Update(StockEntity stock, StockInfo stockInfo)
        {
            var dealDate = DateTime.Now.ToString("yyyy-MM-dd");
            var price = SQLiteDBUtil.Instance.QueryFirst<StockPriceEntity>($"StockCode='{stock.Code}' and DealDate='{dealDate}' and DateType=0");
            if (price == null)
            {
                stockInfo.DayPrice.DealTime = "";
                SQLiteDBUtil.Instance.Insert<StockPriceEntity>(stockInfo.DayPrice);
            }
            else
            {
                stockInfo.DayPrice.ID = price.ID;
                stockInfo.DayPrice.DealTime = "";
                SQLiteDBUtil.Instance.Update<StockPriceEntity>(stockInfo.DayPrice);
            }
            if (stock.Foucs > 0)
            {
                var dealTime = DateTime.Now.ToString("HH:mm");
                if (dealTime.CompareTo("15:00") >= 0) dealTime = "15:00";
                if (dealTime.CompareTo("09:25") <= 0) dealTime = "09:25";

                var price2 = SQLiteDBUtil.Instance.QueryFirst<StockPriceEntity>($"StockCode='{stock.Code}' and DealDate='{dealDate}' and DealTime='{dealTime}' and DateType=1");
                if (price2 == null)
                {
                    stockInfo.DayPrice.DateType = 1;//分钟
                    stockInfo.DayPrice.DealTime = dealTime;
                    SQLiteDBUtil.Instance.Insert<StockPriceEntity>(stockInfo.DayPrice);
                }
                else
                {
                    stockInfo.DayPrice.ID = price2.ID;
                    stockInfo.DayPrice.DateType = 1;//分钟
                    stockInfo.DayPrice.DealTime = dealTime;
                    SQLiteDBUtil.Instance.Update<StockPriceEntity>(stockInfo.DayPrice);
                }
            }
        }

        /// <summary>
        /// 计算账户盈亏
        /// </summary>
        /// <param name="actionLog"></param>
        public static void CalculateProfit(Action<string> actionLog)
        {
            var stocks = SQLiteDBUtil.Instance.QueryAll<StockEntity>();
            var accountStocks = SQLiteDBUtil.Instance.QueryAll<AccountStockEntity>();
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
            SQLiteDBUtil.Instance.Update<AccountStockEntity>(accountStocks);

            if (stocks.Length > 0) actionLog($">------------------------------------------------>");
        }

        public static void CalculateProfit(string accountName, string stockCode, decimal stockPrice)
        {
            var account = SQLiteDBUtil.Instance.QueryFirst<AccountStockEntity>($"AccountName='{accountName}' and StockCode='{stockCode}'");
            if (account == null) return;

            account.Price = stockPrice;
            account.HoldAmount = account.Price * account.HoldQty;
            account.Profit = account.HoldAmount - account.TotalAmount;
            if (account.TotalAmount == 0) account.UDPer = 0;
            else account.UDPer = Math.Round(account.Profit / account.HoldAmount * 100, 2);

            SQLiteDBUtil.Instance.Update<AccountStockEntity>(account);
        }
    }
}
