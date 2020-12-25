using StockSimulateCore.Config;
using StockSimulateDomain.Entity;
using StockSimulateDomain.Model;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateNetCore.Utils;
using StockSimulateDomain.Utils;

namespace StockSimulateCore.Service
{
    public class StockGatherService
    {
        public static void GatherHisPriceData()
        {
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>();
            foreach (var stock in stocks)
            {
                var stockPrices = EastMoneyUtil.GetStockHisPrice(stock.Code);
                if (stockPrices == null) continue;

                var lastPrice = MySQLDBUtil.Instance.QueryAll<StockPriceEntity>($"StockCode='{stock.Code}'", "DealDate asc", 1).FirstOrDefault();
                if (lastPrice == null)
                {
                    var newPrices = stockPrices.OrderByDescending(c => c.DealDate).ToArray();
                    MySQLDBUtil.Instance.Insert<StockPriceEntity>(newPrices);
                }
                else
                {
                    var lastDate = lastPrice.DealDate;
                    var newPrices = stockPrices.Where(c => c.DealDate.CompareTo(lastDate) < 0).OrderByDescending(c => c.DealDate).ToArray();
                    MySQLDBUtil.Instance.Insert<StockPriceEntity>(newPrices);
                }
            }
        }
    }
}
