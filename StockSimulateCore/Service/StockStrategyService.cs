using StockSimulateCore.Config;
using StockSimulateCore.Entity;
using StockSimulateCore.Model;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Service
{
    public class StockStrategyService
    {
        /// <summary>
        /// 左侧交易策略
        /// </summary>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static StockStrategyEntity[] MakeStockStrategys(StrategyInfo strategy)
        {
            var stockStrategys = new List<StockStrategyEntity>();
            if (strategy.GetType().Equals(typeof(LeftExchangeStrategyInfo)))
            {
                stockStrategys = MakeLeftExchangeStrategys(strategy as LeftExchangeStrategyInfo);
            }
            else if(strategy.GetType().Equals(typeof(TExchangeStrategyInfo)))
            {
                stockStrategys = MakeTExchangeStrategys(strategy as TExchangeStrategyInfo);
            }

            SQLiteDBUtil.Instance.Delete<StockStrategyEntity>($"StockCode='{strategy.StockCode}'");
            SQLiteDBUtil.Instance.Delete<RemindEntity>($"StockCode='{strategy.StockCode}' and (RType={8} or RType={9})");
            SQLiteDBUtil.Instance.Insert<StockStrategyEntity>(stockStrategys.ToArray());

            return stockStrategys.ToArray();
        }

        static List<StockStrategyEntity> MakeLeftExchangeStrategys(LeftExchangeStrategyInfo strategy)
        {
            decimal buyPrice = strategy.BuyPrice;
            decimal buyAmount = strategy.BuyAmount;
            decimal maxBuyAmount = strategy.TotalBuyAmount;
            int buyCount = GetExchangeCount(Math.Floor(buyAmount / buyPrice));
            decimal totalBuyAmount = Math.Round(buyCount * buyPrice, 2);
            int hasCount = buyCount;
            decimal totalDownPercent = 0;
            decimal downPer = 0;
            decimal cost = buyPrice;
            decimal salePrice = strategy.SalePrice;
            int saleCount = 0;
            decimal saleAmount = 0;
            decimal profit = 0;

            var stockStrategys = new List<StockStrategyEntity>();

            var account = SQLiteDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{strategy.AccountName}'");
            if (account == null) return stockStrategys;

            stockStrategys.Add(new StockStrategyEntity()
            {
                StrategyName = strategy.Name,
                AccountName = strategy.AccountName,
                StockCode = strategy.StockCode,
                ExecuteMode = strategy.ExecuteMode,
                Target = "建仓",
                Condition = 0,
                Price = buyPrice,
                BuyQty = buyCount,
                BuyAmount = totalBuyAmount,
                SaleQty = saleCount,
                SaleAmount = saleAmount,
                HoldQty = buyCount,
                HoldAmount = totalBuyAmount,
                TotalBuyAmount = totalBuyAmount,
                FloatAmount = 0,
                Cost = buyPrice,
                Profit = 0
            });

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

                stockStrategys.Add(new StockStrategyEntity()
                {
                    StrategyName = strategy.Name,
                    AccountName = strategy.AccountName,
                    StockCode = strategy.StockCode,
                    ExecuteMode = strategy.ExecuteMode,
                    Target = $"下跌{downPer}%",
                    Condition = 0,
                    Price = buyPrice,
                    BuyQty = buyCount,
                    BuyAmount = thsBuyAmount,
                    SaleQty = saleCount,
                    SaleAmount = saleAmount,
                    HoldQty = hasCount,
                    HoldAmount = buyPrice * hasCount,
                    TotalBuyAmount = totalBuyAmount,
                    FloatAmount = buyPrice * hasCount - totalBuyAmount,
                    Cost = cost,
                    Profit = 0
                });
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

                stockStrategys.Add(new StockStrategyEntity()
                {
                    StrategyName = strategy.Name,
                    AccountName = strategy.AccountName,
                    StockCode = strategy.StockCode,
                    ExecuteMode = strategy.ExecuteMode,
                    Target = $"上涨{lastPer}%",
                    Condition = (saleCount > 0 ? 1 : 2),
                    Price = buyPrice,
                    BuyQty = 0,
                    BuyAmount = 0,
                    SaleQty = saleCount,
                    SaleAmount = saleAmount,
                    HoldQty = hasCount,
                    HoldAmount = buyPrice * hasCount,
                    TotalBuyAmount = totalBuyAmount,
                    FloatAmount = buyPrice * hasCount - totalBuyAmount,
                    Cost = cost,
                    Profit = profit
                });
            }

            var stockReminds = stockStrategys.Where(c => c.ExecuteMode == 1 && c.BuyQty > 0).ToArray();
            foreach (var detail in stockReminds)
            {
                var remind = new RemindEntity()
                {
                    StockCode = strategy.StockCode,
                    Target = detail.Price,
                    RType = detail.BuyQty > 0 ? 8 : 9,
                    Email = account.Email,
                    QQ = account.QQ,
                    StrategyName = detail.StrategyName,
                    StrategyTarget = detail.Target
                };
                remind.MaxPrice = Math.Round(remind.Target * (1 + RunningConfig.Instance.RemindStockPriceFloatPer / 100), 2);
                remind.MinPrice = Math.Round(remind.Target * (1 - RunningConfig.Instance.RemindStockPriceFloatPer / 100), 2);
                SQLiteDBUtil.Instance.Insert<RemindEntity>(remind);
            }
            return stockStrategys;
        }
        static List<StockStrategyEntity> MakeTExchangeStrategys(TExchangeStrategyInfo strategy)
        {
            var stockStrategys = new List<StockStrategyEntity>();

            var account = SQLiteDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{strategy.AccountName}'");
            if (account == null) return stockStrategys;

            var buyPrice = Math.Round((1 + strategy.ReducePricePer / 100m) * strategy.BasePrice, 2);
            stockStrategys.Add(new StockStrategyEntity()
            {
                StrategyName = strategy.Name,
                AccountName = strategy.AccountName,
                StockCode = strategy.StockCode,
                ExecuteMode = strategy.ExecuteMode,
                Target = $"低吸{buyPrice}",
                Condition = 0,
                Price = buyPrice,
                BuyQty = strategy.BSQty,
                BuyAmount = buyPrice * strategy.BSQty,
                SaleQty = 0,
                SaleAmount = 0,
                HoldQty = strategy.HoldQty + strategy.BSQty,
                HoldAmount = strategy.BasePrice * strategy.HoldQty + buyPrice * strategy.BSQty,
                TotalBuyAmount = buyPrice * strategy.BSQty,
                FloatAmount = 0,
                Cost = buyPrice,
                Profit = 0
            });
            var salePrice = Math.Round((1 + strategy.IncreasePricePer / 100m) * strategy.BasePrice, 2);
            stockStrategys.Add(new StockStrategyEntity()
            {
                StrategyName = strategy.Name,
                AccountName = strategy.AccountName,
                StockCode = strategy.StockCode,
                ExecuteMode = strategy.ExecuteMode,
                Target = $"高抛{salePrice}",
                Condition = 0,
                Price = salePrice,
                BuyQty = 0,
                BuyAmount = 0,
                SaleQty = strategy.BSQty,
                SaleAmount = salePrice * strategy.BSQty,
                HoldQty = strategy.HoldQty - strategy.BSQty,
                HoldAmount = strategy.BasePrice * strategy.HoldQty - salePrice * strategy.BSQty,
                TotalBuyAmount = strategy.BasePrice * strategy.HoldQty - salePrice * strategy.BSQty,
                FloatAmount = 0,
                Cost = buyPrice,
                Profit = 0
            });
            return stockStrategys;
        }
        static int GetExchangeCount(decimal amount)
        {
            var yu = amount % 100;
            if (yu > 50)
            {
                return (int)Math.Floor(amount / 100) * 100 + 100;
            }
            else
            {
                return (int)Math.Floor(amount / 100) * 100;
            }
        }
    }
}
