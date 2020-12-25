using StockSimulateDomain.Entity;
using StockSimulateDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateService.Utils;

namespace StockSimulateService.Service
{
    public class StockValuateService
    {
        public static ValuateResultInfo[] Valuate(decimal safeRate)
        {
            var result = new List<ValuateResultInfo>();
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>($"Type=0");
            foreach(var stock in stocks)
            {
                var info = Valuate(stock.Code, safeRate: safeRate);
                if(info != null)
                {
                    result.Add(info);
                }
            }
            return result.ToArray();
        }

        public static ValuateResultInfo Valuate(string stockCode, decimal growth = 0, decimal pe = 0, decimal safeRate = 80, decimal netProfit = 0)
        {
            var stock = MySQLDBUtil.Instance.QueryFirst<StockEntity>($"Type=0 and Code='{stockCode}'");
            if (stock == null) return null;

            var newMainTargets = MySQLDBUtil.Instance.QueryAll<MainTargetEntity>($"StockCode='{stockCode}' and Rtype=2", "Date desc", 8);
            if (newMainTargets.Length == 0) return null;

            var lastMainTarget = newMainTargets.FirstOrDefault();
            var nowYear = lastMainTarget.Date.Substring(0, 4);
            var yetYear = (int.Parse(nowYear) - 1).ToString();
            var yetDates = newMainTargets.Where(c => c.Date.StartsWith(nowYear)).Select(c => c.Date.Replace(nowYear, yetYear)).ToArray();
            var yetDate = $"{yetYear}-12-31";
            var mainTarget = MySQLDBUtil.Instance.QueryFirst<MainTargetEntity>($"StockCode='{stockCode}' and Date='{yetDate}' and Rtype=1");
            if (mainTarget == null) return null;

            var hasJlrs = newMainTargets.Where(c => c.Date.StartsWith(yetYear) && !yetDates.Contains(c.Date)).ToArray();
            var yetLostNetProfit = hasJlrs.Sum(c => c.Gsjlr);

            var price = stock.Price;
            var capital = stock.Capital;
            var yetNetProfit = mainTarget.Gsjlr;
            var yetGrowth = mainTarget.Gsjlrtbzz;
            var yetEPS = mainTarget.Jbmgsy;

            //自动预测按去年增长率及TTM计算
            var wantGrowth = growth == 0 ? yetGrowth : growth;
            var wantPE = pe == 0 ? stock.TTM : pe;

            //预测净利润
            var wantNetProfit = yetNetProfit * (1 + wantGrowth / 100m);
            if (netProfit > 0) wantNetProfit = netProfit;

            //净利润差值
            var lostNetProfit = wantNetProfit - stock.NetProfit;

            //预测每股收益
            var wantEPS = capital > 0 ? Math.Round(wantNetProfit / capital, 3) : 0;

            //预测市值
            var wantAmount = Math.Round(wantPE * wantNetProfit, 2);

            //预测股价
            var wantPrice = Math.Round(wantPE * wantEPS, 2);
            if (wantPrice < 0) wantPrice = 0;

            //预测盈利
            var wantUPPer = price > 0 ? Math.Round((wantPrice - price) / price * 100m, 2) : 0;

            //预测安全买入价格
            var safePrice = Math.Round(wantPrice * safeRate / 100m, 2);

            //预测安全盈利
            var safeUPPer = safePrice > 0 ? Math.Round((wantPrice - safePrice) / safePrice * 100m, 2) : 0;

            //推荐
            var advise = "等待";
            if (price <= safePrice * 0.8m) advise = "重仓";
            else if (price <= safePrice) advise = "买入";
            else if (price > safePrice && price <= wantPrice) advise = "等待";
            else if (price > wantPrice && price <= wantPrice * 1.2m) advise = "减仓";
            else if (price > wantPrice * 1.2m) advise = "卖出";

            return new ValuateResultInfo()
            {
                StockCode = stockCode,
                Growth = wantGrowth,
                PE = wantPE,
                NetProfit = wantNetProfit,
                LostNetProfit = lostNetProfit,
                EPS = wantEPS,
                Amount = wantAmount,
                Price = wantPrice,
                UPPer = wantUPPer,
                SafePrice = safePrice,
                SafeUPPer = safeUPPer,
                Advise = advise
            };
        }
    }
}
