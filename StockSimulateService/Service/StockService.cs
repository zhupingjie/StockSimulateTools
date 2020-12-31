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

        public static void Update(StockEntity stock, StockInfo stockInfo)
        {
            var columns = new List<string>();
            var preps = typeof(StockEntity).GetProperties();
            foreach(var prep in preps)
            {
                if (prep.GetCustomAttributes(typeof(GatherColumnAttribute), true).Length == 0) continue;

                var changeValue = ObjectUtil.GetPropertyValue(stockInfo.Stock, prep.Name);
                ObjectUtil.SetPropertyValue(stock, prep.Name, changeValue);

                columns.Add(prep.Name);
            }
            Repository.Instance.Update<StockEntity>(stock, columns.ToArray());
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
