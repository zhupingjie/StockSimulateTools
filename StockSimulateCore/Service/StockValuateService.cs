﻿using StockSimulateCore.Entity;
using StockSimulateCore.Model;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Service
{
    public class StockValuateService
    {
        public static ValuateResultInfo[] Valuate()
        {
            var result = new List<ValuateResultInfo>();
            var stocks = SQLiteDBUtil.Instance.QueryAll<StockEntity>($"Type=0");
            foreach(var stock in stocks)
            {
                var info = Valuate(stock.Code);
                if(info != null)
                {
                    result.Add(info);
                }
            }
            return result.ToArray();
        }

        public static ValuateResultInfo Valuate(string stockCode, decimal growth = 0, decimal pe = 0)
        {
            var stock = SQLiteDBUtil.Instance.QueryFirst<StockEntity>($"Type=0 and Code='{stockCode}'");
            if (stock == null) return null;

            var newMainTargets = SQLiteDBUtil.Instance.QueryAll<MainTargetEntity>($"StockCode='{stockCode}' and Rtype=2", "Date desc", 8);
            if (newMainTargets.Length == 0) return null;

            var lastMainTarget = newMainTargets.FirstOrDefault();
            var nowYear = lastMainTarget.Date.Substring(0, 4);
            var yetYear = (int.Parse(nowYear) - 1).ToString();
            var yetDates = newMainTargets.Where(c => c.Date.StartsWith(nowYear)).Select(c => c.Date.Replace(nowYear, yetYear)).ToArray();
            var yetDate = $"{yetYear}-12-31";
            var mainTarget = SQLiteDBUtil.Instance.QueryFirst<MainTargetEntity>($"StockCode='{stockCode}' and Date='{yetDate}' and Rtype=1");
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

            //净利润差值
            var lostNetProfit = wantNetProfit - yetLostNetProfit;

            //预测每股收益
            var wantEPS = Math.Round(wantNetProfit / capital, 3);

            //预测市值
            var wantAmount = Math.Round(wantPE * wantNetProfit, 2);

            //预测股价
            var wantPrice = Math.Round(wantPE * wantEPS, 2);

            //推荐
            var advise = "观望";
            if (wantPrice >= price * 1.4m)
            {
                advise = "重仓";
            }
            else if(wantPrice >= price * 1.2m)
            {
                advise = "买入";
            }
            else if(wantPrice >= price * 0.8m)
            {
                advise = "观望";
            }
            else if(wantPrice >= price * 0.6m)
            {
                advise = "卖出";
            }
            else
            {
                advise = "清仓";
            }

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
                 Advise = advise
            };
        }
    }
}
