using StockSimulateCore.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Service
{
    public class StockStrategyService
    {
        public static DataTable MakeStrategyData(StrategyEntity strategy, decimal buyPrice, decimal buyAmount, decimal salePrice, decimal maxBuyAmount)
        {
            var dt = new DataTable("stockprice");
            dt.Columns.Add("操作");
            dt.Columns.Add("收盘价");
            dt.Columns.Add("买入数");
            dt.Columns.Add("买入市值");
            dt.Columns.Add("卖出数");
            dt.Columns.Add("卖出市值");
            dt.Columns.Add("持有数");
            dt.Columns.Add("持有市值");
            dt.Columns.Add("投入市值");
            dt.Columns.Add("浮动市值");
            dt.Columns.Add("成本");
            dt.Columns.Add("盈亏");
            dt.Rows.Clear();


            decimal buyCount = GetExchangeCount(Math.Floor(buyAmount / buyPrice));
            decimal totalBuyAmount = Math.Round(buyCount * buyPrice, 2);
            decimal hasCount = buyCount;
            decimal totalDownPercent = 0;
            decimal downPer = 0;
            decimal cost = buyPrice;
            decimal saleCount = 0;
            decimal saleAmount = 0;
            decimal profit = 0;

            var dr = dt.NewRow();
            dr["操作"] = $"建仓";
            dr["收盘价"] = $"{buyPrice}";
            dr["买入数"] = $"{buyCount}";
            dr["买入市值"] = $"{totalBuyAmount}";
            dr["卖出数"] = $"{saleCount}";
            dr["卖出市值"] = $"{saleAmount}";
            dr["持有数"] = $"{buyCount}";
            dr["持有市值"] = $"{totalBuyAmount}";
            dr["投入市值"] = $"{totalBuyAmount}";
            dr["浮动市值"] = "0";
            dr["成本"] = $"{buyPrice}";
            dr["盈亏"] = "0";

            dt.Rows.Add(dr);

            while (true)
            {
                decimal thsBuyPrice = Math.Round(buyPrice * (1 + strategy.IncreasePricePer / 100), 2);
                decimal thsBuyAmount = strategy.IncreaseAmount;

                totalDownPercent += Math.Abs(strategy.IncreasePricePer);
                if (totalDownPercent >= Math.Abs(strategy.IncreaseMorePer))
                {
                    thsBuyAmount = strategy.IncreaseAmount * (1 + strategy.IncreaseMoreAmountPer / 100);
                }
                if (totalDownPercent >= Math.Abs(strategy.IncreaseMaxPer))
                {
                    thsBuyAmount = strategy.IncreaseAmount * (1 + strategy.IncreaseMaxAmountPer / 100);
                }
                buyCount = GetExchangeCount(Math.Floor(thsBuyAmount / thsBuyPrice));
                thsBuyAmount = Math.Round(buyCount * thsBuyPrice, 2);

                if (totalBuyAmount + thsBuyAmount >= maxBuyAmount) break;

                buyPrice = Math.Round(buyPrice * (1 + strategy.IncreasePricePer / 100), 2);
                totalBuyAmount += thsBuyAmount;
                downPer += strategy.IncreasePricePer;
                hasCount += buyCount;
                cost = Math.Round(totalBuyAmount / hasCount, 2);

                dr = dt.NewRow();
                dr["操作"] = $"下跌{downPer}%";
                dr["收盘价"] = $"{buyPrice}";
                dr["买入数"] = $"{buyCount}";
                dr["买入市值"] = $"{thsBuyAmount}";
                dr["卖出数"] = $"{saleCount}";
                dr["卖出市值"] = $"{saleAmount}";
                dr["持有数"] = $"{hasCount}";
                dr["持有市值"] = $"{buyPrice * hasCount}";
                dr["投入市值"] = $"{totalBuyAmount}";
                dr["浮动市值"] = $"{buyPrice * hasCount - totalBuyAmount}";
                dr["成本"] = $"{cost}";
                dr["盈亏"] = "0";
                dt.Rows.Add(dr);
            }

            decimal lastPer = downPer;
            decimal upPer = 0;
            while (true)
            {
                //if (lastPer >= Math.Abs(downPer)) break;
                upPer += Math.Abs(strategy.IncreasePricePer);
                lastPer = downPer + upPer;

                buyPrice = Math.Round(buyPrice * (1 + -1 * strategy.IncreasePricePer / 100), 2);
                if (buyPrice > salePrice)
                {
                    saleCount = GetExchangeCount(hasCount * strategy.ReducePositionPer / 100);
                    if (saleCount == 0) break;
                    hasCount -= saleCount;
                    saleAmount = buyPrice * saleCount;
                    totalBuyAmount -= saleAmount;
                    profit += saleAmount;
                    if (hasCount == 0) cost = 0;
                    else cost = Math.Round(totalBuyAmount / hasCount, 2);
                }

                dr = dt.NewRow();
                dr["操作"] = $"上涨{lastPer}%";
                dr["收盘价"] = $"{buyPrice}";
                dr["买入数"] = $"{0}";
                dr["买入市值"] = $"{0}";
                dr["卖出数"] = $"{saleCount}";
                dr["卖出市值"] = $"{saleAmount}";
                dr["持有数"] = $"{hasCount}";
                dr["持有市值"] = $"{buyPrice * hasCount}";
                dr["投入市值"] = $"{totalBuyAmount}";
                dr["浮动市值"] = $"{buyPrice * hasCount - totalBuyAmount}";
                dr["成本"] = $"{cost}";
                dr["盈亏"] = $"{profit}";
                dt.Rows.Add(dr);
            }

            return dt;
        }

        static decimal GetExchangeCount(decimal amount)
        {
            var yu = amount % 100;
            if (yu > 50)
            {
                return Math.Floor(amount / 100) * 100 + 100;
            }
            else
            {
                return Math.Floor(amount / 100) * 100;
            }
        }
    }
}
