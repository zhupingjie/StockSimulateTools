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

            if(stock.Foucs == 0)
            {
                StockRemindService.Clear(stockCode, true);
            }
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
            if (stock.MinPrice == 0 || stock.MinPrice > stock.Price || stock.PriceDate != stockInfo.Stock.PriceDate) stock.MinPrice = stockInfo.DayPrice.TodayMinPrice;
            if (stock.MaxPrice == 0 || stock.MaxPrice < stock.Price || stock.PriceDate != stockInfo.Stock.PriceDate) stock.MaxPrice = stockInfo.DayPrice.TodayMaxPrice;
            if (stock.StartPrice == 0 || stock.PriceDate != stockInfo.Stock.PriceDate) stock.StartPrice = stockInfo.DayPrice.TodayStartPrice;
            
            stock.PriceDate = stockInfo.Stock.PriceDate;

            var columns = new List<string>() { "PriceDate", "MaxPrice", "MinPrice", "StartPrice", "PEG", "Advise" };
            var preps = typeof(StockEntity).GetProperties();
            foreach(var prep in preps)
            {
                if (prep.GetCustomAttributes(typeof(GatherColumnAttribute), true).Length == 0) continue;
                if (columns.Contains(prep.Name)) continue;

                var changeValue = ObjectUtil.GetPropertyValue(stockInfo.Stock, prep.Name);
                ObjectUtil.SetPropertyValue(stock, prep.Name, changeValue);

                columns.Add(prep.Name);
            }
            if (stock.ProfitGrewPer > 0)
            {
                stock.PEG = Math.Round(stock.TTM / stock.ProfitGrewPer, 2);
            }
            if(stock.EPrice > 0)
            {
                if (stock.Price >= stock.EPrice * 0.95m && stock.Price <= stock.EPrice * 1.05m)
                {
                    stock.Advise = "等待";
                }
                else if (stock.Price > stock.EPrice * 0.8m && stock.Price < stock.EPrice * 0.95m)
                {
                    stock.Advise = "买入";
                }
                else if (stock.Price < stock.EPrice * 0.8m)
                {
                    stock.Advise = "重仓";
                }
                else if (stock.Price > stock.EPrice * 1.05m && stock.Price < stock.EPrice * 1.2m)
                {
                    stock.Advise = "减仓";
                }
                else if (stock.Price > stock.EPrice * 1.2m)
                {
                    stock.Advise = "卖出";
                }
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

            //stock.EPE = pe;
            //stock.Growth = growth;
            stock.Target = target;
            stock.Safety = safety;
            //stock.Advise = advise;
            Repository.Instance.Update<StockEntity>(stock, new string[] { "Target", "Safety" });
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

                //stock.EPE = result.PE;
                //stock.Growth = result.Growth;
                stock.Target = result.Price;
                stock.Safety = result.SafePrice;
                //stock.Advise = result.Advise;
            }
            Repository.Instance.Update<StockEntity>(stocks, new string[] { "Target", "Safety" });
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
