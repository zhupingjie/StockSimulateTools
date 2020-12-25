using StockSimulateDomain.Entity;
using StockSimulateDomain.Model;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateDomain.Attributes;

namespace StockSimulateCore.Service
{
    public class StockService
    {
        public static void Foucs(string stockCode)
        {
            var stock = MySQLDBUtil.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null) return;
            stock.Foucs += 1;
            if (stock.Foucs > 2) stock.Foucs = 0;
            MySQLDBUtil.Instance.Update<StockEntity>(stock);
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
            MySQLDBUtil.Instance.Update<StockEntity>(stock);
        }

        public static void Delete(string stockCode)
        {
            var stock = MySQLDBUtil.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock != null)
            {
                MySQLDBUtil.Instance.Delete<StockEntity>(stock);
            }
            var stockStrategyDetails = MySQLDBUtil.Instance.QueryAll<StockStrategyEntity>($"StockCode='{stockCode}'");
            if (stockStrategyDetails.Length > 0)
            {
                MySQLDBUtil.Instance.Delete<StockStrategyEntity>($"StockCode='{stockCode}'");
            }
        }

        public static void SaveValuateResult(string stockCode, decimal target, decimal safety, decimal growth, decimal pe, string advise)
        {
            var stock = MySQLDBUtil.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null) return;

            stock.EPE = pe;
            stock.Growth = growth;
            stock.Target = target;
            stock.Safety = safety;
            stock.Advise = advise;
            MySQLDBUtil.Instance.Update<StockEntity>(stock);
        }

        public static void SaveValuateResult(ValuateResultInfo[] results)
        {
            var codes = results.Select(c => c.StockCode).ToArray();
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>($"Code in ('{string.Join("','", codes)}')");
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
            MySQLDBUtil.Instance.Update<StockEntity>(stocks);
        }
    }
}
