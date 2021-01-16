using StockSimulateDomain.Entity;
using StockSimulateDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateDomain.Attributes;
using StockSimulateCore.Utils;
using StockSimulateService.Helper;
using StockSimulateCore.Data;

namespace StockSimulateService.Service
{
    public class StockService
    {
        public static void Foucs(string stockCode)
        {
            var stock = Repository.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null) return;
            stock.Foucs += 1;
            if (stock.Foucs > 2) stock.Foucs = 0;
            Repository.Instance.Update<StockEntity>(stock, new string[] { "Foucs" });
        }
        public static void Top(string stockCode)
        {
            var stock = Repository.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null) return;
            stock.Top += 1;
            if (stock.Top > 1) stock.Top = 0;
            Repository.Instance.Update<StockEntity>(stock, new string[] { "Top" });
        }

        public static void Update(StockCacheInfo stockCache, IList<StockPriceCacheInfo> stockPriceCaches, StockInfo stockInfo)
        {
            var stock = stockInfo.Stock;
            stock.ID = stockCache.StockID;

            var decNum = stockCache.Type == 0 ? 2 : 3;
            var columns = new List<string>();
            var preps = typeof(StockEntity).GetProperties();
            foreach(var prep in preps)
            {
                if (prep.GetCustomAttributes(typeof(GatherColumnAttribute), true).Length == 0) continue;

                var changeValue = ObjectUtil.GetPropertyValue(stockInfo.Stock, prep.Name);
                ObjectUtil.SetPropertyValue(stock, prep.Name, changeValue);

                columns.Add(prep.Name);
            }
            if (stockPriceCaches.Count > 0)
            {
                var avgPrices = stockPriceCaches
                    .Where(c => c.DealDate.CompareTo(stockCache.PriceDate) < 0)
                    .OrderByDescending(c => c.DealDate)
                    .Select(c => c.Price)
                    .ToList();
                avgPrices.Insert(0, stock.Price);

                if (avgPrices.Count >= 0) stock.AvgPrice5 = Math.Round(avgPrices.Take(5).Average(), decNum);
                if (avgPrices.Count >= 10) stock.AvgPrice10 = Math.Round(avgPrices.Take(10).Average(), decNum);
                if (avgPrices.Count >= 20) stock.AvgPrice20 = Math.Round(avgPrices.Take(20).Average(), decNum);
                if (avgPrices.Count >= 30) stock.AvgPrice30 = Math.Round(avgPrices.Take(30).Average(), decNum);
                if (avgPrices.Count >= 60) stock.AvgPrice60 = Math.Round(avgPrices.Take(60).Average(), decNum);
                if (avgPrices.Count >= 120) stock.AvgPrice120 = Math.Round(avgPrices.Take(120).Average(), decNum);
                if (avgPrices.Count >= 250) stock.AvgPrice250 = Math.Round(avgPrices.Take(250).Average(), decNum);

                Repository.Instance.Update<StockEntity>(stock, columns.Concat(new string[] { "AvgPrice5", "AvgPrice10", "AvgPrice20", "AvgPrice60", "AvgPrice120", "AvgPrice250" }).ToArray());
            }
            else
            {
                Repository.Instance.Update<StockEntity>(stock, columns.ToArray());
            }
        }

        public static void Delete(string stockCode)
        {
            var stock = Repository.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock != null)
            {
                Repository.Instance.Delete<StockEntity>(stock);
            }
            var stockStrategyDetails = Repository.Instance.QueryAll<StockStrategyEntity>($"StockCode='{stockCode}'");
            if (stockStrategyDetails.Length > 0)
            {
                Repository.Instance.Delete<StockStrategyEntity>($"StockCode='{stockCode}'");
            }
        }

        public static void SaveValuateResult(string stockCode, decimal target, decimal safety, decimal growth, decimal pe, string advise)
        {
            var stock = Repository.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null) return;

            stock.EPE = pe;
            stock.Growth = growth;
            stock.Target = target;
            stock.Safety = safety;
            stock.Advise = advise;
            Repository.Instance.Update<StockEntity>(stock, new string[] { "EPE", "Growth", "Target", "Safety", "Advise" });
        }

        public static void SaveValuateResult(ValuateResultInfo[] results)
        {
            var codes = results.Select(c => c.StockCode).ToArray();
            var stocks = Repository.Instance.QueryAll<StockEntity>($"Code in ('{string.Join("','", codes)}')");
            if (stocks.Length == 0) return;

            foreach (var stock in stocks)
            {
                var result = results.FirstOrDefault(c => c.StockCode == stock.Code);
                if (result == null) continue;

                stock.EPE = result.PE;
                stock.Growth = result.Growth;
                stock.Target = result.Price;
                stock.Safety = result.SafePrice;
                stock.Advise = result.Advise;
            }
            Repository.Instance.Update<StockEntity>(stocks, new string[] { "EPE", "Growth", "Target", "Safety", "Advise" });
        }

        public static StockResultInfo[] GetStockInfos()
        {
            var stocks = Repository.Instance.QueryAll<StockEntity>();
            return stocks.Select(c => new StockResultInfo()
            {
                Code = c.Code,
                Name = c.Name,
                Price = c.Price,
                UDPer = c.UDPer,
                TTM = c.TTM,
                Amount = c.Amount
            }).ToArray();
        }
    }
}
