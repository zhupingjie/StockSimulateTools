using StockSimulateCore.Data;
using StockSimulateDomain.Entity;
using StockSimulateDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockSimulateService.Service
{
    public class StockCacheService
    {
        public static void LoadCacheInfo(List<StockCacheInfo> stockCaches, List<StockPriceCacheInfo> stockPriceCaches)
        {
            if(stockCaches.Count > 0)
            {
                var date = DateTime.Now.Date.ToString("yyyy-MM-dd");
                if (stockCaches.Any(c => c.CacheDate == date)) return;

                stockCaches.Clear();
                stockPriceCaches.Clear();
            }

            var stocks = LoadStockCacheInfo();
            stockCaches.AddRange(stocks);

            foreach (var stock in stockCaches)
            {
                var stockPrices = LoadStockPriceCacheInfo(stock.StockCode);
                stockPriceCaches.AddRange(stockPrices);
            }
        }

        static StockCacheInfo[] LoadStockCacheInfo()
        {
            var stocks = Repository.Instance.QueryAll<StockEntity>(withNoLock: true);
            return stocks.Select(c => new StockCacheInfo()
            {
                StockID = c.ID,
                StockCode = c.Code,
                StockName = c.Name,
                ReportDate = c.ReportDate,
                PriceDate = c.PriceDate,
                Type = c.Type,
                Focus = c.Foucs
            }).ToArray();
        }

        static StockPriceCacheInfo[] LoadStockPriceCacheInfo(string stockCode)
        {
            var stockPrices = Repository.Instance.QueryAll<StockPriceEntity>($"StockCode='{stockCode}'", columns: new string[] { "StockCode", "DealDate", "Price" }, withNoLock: true);
            return stockPrices.Select(c => new StockPriceCacheInfo()
            {
                StockCode = c.StockCode,
                DealDate = c.DealDate,
                DealQty = c.DealQty,
                Price = c.Price
            }).ToArray();
        }
    }
}
