using StockSimulateCore.Entity;
using StockSimulateCore.Model;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Service
{
    public class StockService
    {
        public static void Foucs(string stockCode)
        {
            var stock = SQLiteDBUtil.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null) return;

            if (stock.Foucs == 0) stock.Foucs = 1;
            else stock.Foucs = 0;
            SQLiteDBUtil.Instance.Update<StockEntity>(stock);
        }

        public static void Update(StockEntity stock, StockInfo stockInfo)
        {
            var preps = typeof(StockEntity).GetProperties();
            foreach(var prep in preps)
            {
                if (prep.GetCustomAttributes(typeof(GatherColumnAttribute), true).Length == 0) continue;

                var prepValue = ObjectUtil.GetPropertyValue(stockInfo.Stock, prep.Name);

                ObjectUtil.SetPropertyValue(stock, prep.Name, prepValue);
            }
            SQLiteDBUtil.Instance.Update<StockEntity>(stock);
        }

        public static void Delete(string stockCode)
        {
            var stock = SQLiteDBUtil.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock != null)
            {
                SQLiteDBUtil.Instance.Delete<StockEntity>(stock);
            }
            //var stockStrategy = SQLiteDBUtil.Instance.QueryFirst<AccountStockEntity>($"StockCode='{stockCode}'");
            //if (stockStrategy != null)
            //{
            //    SQLiteDBUtil.Instance.Delete<AccountStockEntity>(stockStrategy);
            //}
            var stockStrategyDetails = SQLiteDBUtil.Instance.QueryAll<StockStrategyEntity>($"StockCode='{stockCode}'");
            if (stockStrategyDetails.Length > 0)
            {
                SQLiteDBUtil.Instance.Delete<StockStrategyEntity>($"StockCode='{stockCode}'");
            }
        }

        public static void SaveValuateResult(string stockCode, decimal target, decimal safety, decimal growth, decimal pe, string advise)
        {
            var stock = SQLiteDBUtil.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null) return;

            stock.EPE = pe;
            stock.Growth = growth;
            stock.Target = target;
            stock.Safety = safety;
            stock.Advise = advise;
            SQLiteDBUtil.Instance.Update<StockEntity>(stock);
        }

        public static void SaveValuateResult(ValuateResultInfo[] results)
        {
            var codes = results.Select(c => c.StockCode).ToArray();
            var stocks = SQLiteDBUtil.Instance.QueryAll<StockEntity>($"Code in ('{string.Join("','", codes)}')");
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
            SQLiteDBUtil.Instance.Update<StockEntity>(stocks);
        }
    }
}
