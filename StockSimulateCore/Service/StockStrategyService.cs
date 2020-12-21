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

        public static List<StockStrategyEntity> MakeLeftExchangeStrategys(LeftExchangeStrategyInfo strategy)
        {
            var stockStrategys = new List<StockStrategyEntity>();

            var account = SQLiteDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{strategy.AccountName}'");
            if (account == null) return stockStrategys;

            var stock = SQLiteDBUtil.Instance.QueryFirst<StockEntity>($"Code='{strategy.StockCode}'");
            if (stock == null) return stockStrategys;

            var decNum = stock.Type == 0 ? 2 : 3;

            decimal buyPrice = strategy.BuyPrice;
            decimal buyAmount = strategy.BuyAmount;
            decimal maxBuyAmount = strategy.TotalBuyAmount;
            int buyCount = GetExchangeCount(Math.Floor(buyAmount / buyPrice));
            decimal totalBuyAmount = Math.Round(buyCount * buyPrice, decNum);
            int hasCount = buyCount;
            decimal totalDownPercent = 0;
            decimal downPer = 0;
            decimal cost = buyPrice;
            decimal salePrice = strategy.SalePrice;
            int saleCount = 0;
            decimal saleAmount = 0;

            decimal profit = 0;
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
                decimal thsBuyPrice = Math.Round(buyPrice * (1 + strategy.IncreasePricePer / 100), decNum);
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
                thsBuyAmount = Math.Round(buyCount * thsBuyPrice, decNum);

                if (totalBuyAmount + thsBuyAmount >= maxBuyAmount) break;

                buyPrice = Math.Round(buyPrice * (1 + strategy.IncreasePricePer / 100), decNum);
                totalBuyAmount += thsBuyAmount;
                downPer += strategy.IncreasePricePer;
                hasCount += buyCount;
                cost = Math.Round(totalBuyAmount / hasCount, decNum);

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

                buyPrice = Math.Round(buyPrice * (1 + -1 * strategy.IncreasePricePer / 100), decNum);
                if (buyPrice > salePrice)
                {
                    saleCount = GetExchangeCount(hasCount * strategy.ReducePositionPer / 100);
                    if (saleCount == 0) break;
                    hasCount -= saleCount;
                    saleAmount = buyPrice * saleCount;
                    totalBuyAmount -= saleAmount;
                    profit += saleAmount;
                    if (hasCount == 0) cost = 0;
                    else cost = Math.Round(totalBuyAmount / hasCount, decNum);
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

            var stockReminds = stockStrategys.Where(c => c.ExecuteMode == 0 && c.BuyQty > 0).ToArray();
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
                remind.MaxPrice = Math.Round(remind.Target * (1 + RunningConfig.Instance.RemindStockPriceFloatPer / 100), decNum);
                remind.MinPrice = Math.Round(remind.Target * (1 - RunningConfig.Instance.RemindStockPriceFloatPer / 100), decNum);
                SQLiteDBUtil.Instance.Insert<RemindEntity>(remind);
            }
            return stockStrategys;
        }
        public static List<StockStrategyEntity> MakeTExchangeStrategys(TExchangeStrategyInfo strategy)
        {
            var stockStrategys = new List<StockStrategyEntity>();

            var account = SQLiteDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{strategy.AccountName}'");
            if (account == null) return stockStrategys;

            var stock = SQLiteDBUtil.Instance.QueryFirst<StockEntity>($"Code='{strategy.StockCode}'");
            if (stock == null) return stockStrategys;

            var batchNo = DateTime.Now.Ticks.ToString();
            var decNum = stock.Type == 0 ? 2 : 3;
            var lastPrice = strategy.ActualPrice == 0 ? strategy.BasePrice : strategy.ActualPrice;

            if (strategy.ActualSingleBuy <= strategy.MaxSingleBS)
            {
                var buyPrice = Math.Round((1 + strategy.ReducePricePer / 100m) * lastPrice, decNum);
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
                    BatchNo = batchNo,
                    StrategyInfoType = typeof(TExchangeStrategyInfo).FullName,
                    StrategySource = ServiceStack.Text.JsonSerializer.SerializeToString(strategy)
                });
            }
            if (strategy.ActualSingleSale < strategy.MaxSingleBS)
            {
                var salePrice = Math.Round((1 + strategy.IncreasePricePer / 100m) * lastPrice, decNum);
                stockStrategys.Add(new StockStrategyEntity()
                {
                    StrategyName = strategy.Name,
                    AccountName = strategy.AccountName,
                    StockCode = strategy.StockCode,
                    ExecuteMode = strategy.ExecuteMode,
                    Target = $"高抛{salePrice}",
                    Condition = 1,
                    Price = salePrice,
                    BuyQty = 0,
                    BuyAmount = 0,
                    SaleQty = strategy.BSQty,
                    SaleAmount = salePrice * strategy.BSQty,
                    BatchNo = batchNo,
                    StrategyInfoType = typeof(TExchangeStrategyInfo).FullName,
                    StrategySource = ServiceStack.Text.JsonSerializer.SerializeToString(strategy)
                });
            }
            var stockReminds = stockStrategys.Where(c => c.ExecuteMode == 0 && (c.BuyQty > 0 || c.SaleQty > 0)).ToArray();
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
                remind.MaxPrice = Math.Round(remind.Target * (1 + RunningConfig.Instance.RemindStockPriceFloatPer / 100), decNum);
                remind.MinPrice = Math.Round(remind.Target * (1 - RunningConfig.Instance.RemindStockPriceFloatPer / 100), decNum);
                SQLiteDBUtil.Instance.Insert<RemindEntity>(remind);
            }

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

        public static void Mark(string strategyName, string stockCode, string target, decimal price)
        {
            var stockStrategy = SQLiteDBUtil.Instance.QueryFirst<StockStrategyEntity>($"StrategyName='{strategyName}' and StockCode='{stockCode}' and Target='{target}' and Price={price} and ExecuteOK !=1");
            if (stockStrategy == null) return;

            stockStrategy.ExecuteOK = 1;
            SQLiteDBUtil.Instance.Update<StockStrategyEntity>(stockStrategy);
        }

        public static void Cancel(string strategyName, string stockCode, string target, decimal price)
        {
            SQLiteDBUtil.Instance.Delete<StockStrategyEntity>($"StrategyName='{strategyName}' and StockCode='{stockCode}' and Target='{target}' and Price={price}");
            SQLiteDBUtil.Instance.Delete<RemindEntity>($"StrategyName='{strategyName}' and StockCode='{stockCode}' and StrategyTarget='{target}' and Target={price}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="stockPrice"></param>
        /// <param name="dealTime"></param>
        /// <param name="accountName"></param>
        public static void CheckRun(string stockCode, decimal stockPrice, DateTime dealTime, string accountName = "")
        {
            var stockStrategys = SQLiteDBUtil.Instance.QueryAll<StockStrategyEntity>($"ExecuteMode=1");

            var runStrategys = stockStrategys.Where(c => (c.AccountName == accountName || string.IsNullOrEmpty(accountName)) 
                    && c.ExecuteOK == 0
                    && c.StockCode == stockCode
                    && (c.Condition == 0 && c.Price >= stockPrice || c.Condition == 1 && c.Price <= stockPrice)
                    && (c.BuyQty > 0 || c.SaleQty > 0))
                .ToArray();
            foreach (var item in runStrategys)
            {
                var exchangeInfo = new ExchangeInfo()
                {
                    AccountName = item.AccountName,
                    StockCode = item.StockCode,
                    StrategyName = item.StrategyName,
                    Target = item.Target,
                    Price = stockPrice,
                    ExchangeTime = dealTime
                };

                //差价交易
                ExchangeRun(item, exchangeInfo);

                ExchangeResultInfo result = null;
                if (item.BuyQty > 0)
                {
                    exchangeInfo.Qty = item.BuyQty;
                    result = StockExchangeService.Buy(exchangeInfo);
                }
                if (item.SaleQty > 0)
                {
                    exchangeInfo.Qty = item.SaleQty;
                    result = StockExchangeService.Sale(exchangeInfo);
                }
                if (result != null)
                {
                    item.ExecuteOK = result.Success ? 1 : 2;
                    item.Message = result.Message;
                }
            }
            SQLiteDBUtil.Instance.Update<StockStrategyEntity>(runStrategys);
        }

        public static void ExchangeRun(StockStrategyEntity stockStrategy, ExchangeInfo exchangeInfo)
        {
            if (stockStrategy.StrategyInfoType == typeof(TExchangeStrategyInfo).FullName && !string.IsNullOrEmpty(stockStrategy.StrategySource))
            {
                var strategyInfo = ServiceStack.Text.JsonSerializer.DeserializeFromString<TExchangeStrategyInfo>(stockStrategy.StrategySource) as TExchangeStrategyInfo;
                if (strategyInfo == null) return;
                if (stockStrategy.BuyQty > 0)
                {
                    strategyInfo.ActualSingleBuy += 1;
                    strategyInfo.ActualSingleSale = 0;
                    strategyInfo.ActualPrice = exchangeInfo.Price;

                    if (strategyInfo.ActualSingleBuy > strategyInfo.MaxSingleBS) return;

                    if (StockExchangeService.CouldBuy(exchangeInfo))
                    {
                        var nextStrategys = StockStrategyService.MakeTExchangeStrategys(strategyInfo);
                        SQLiteDBUtil.Instance.Insert<StockStrategyEntity>(nextStrategys.ToArray());

                        //删除批策略号的对立数据
                        SQLiteDBUtil.Instance.Delete<StockStrategyEntity>($"BatchNo='{stockStrategy.BatchNo}' and ID<>{stockStrategy.ID}");
                    }
                }
                if (stockStrategy.SaleQty > 0)
                {
                    strategyInfo.ActualSingleSale += 1;
                    strategyInfo.ActualSingleBuy = 0;
                    strategyInfo.ActualPrice = exchangeInfo.Price;

                    if (strategyInfo.ActualSingleSale > strategyInfo.MaxSingleBS) return;

                    if (StockExchangeService.CouldSale(exchangeInfo))
                    {
                        var nextStrategys = StockStrategyService.MakeTExchangeStrategys(strategyInfo);
                        SQLiteDBUtil.Instance.Insert<StockStrategyEntity>(nextStrategys.ToArray());

                        //删除批策略号的对立数据
                        SQLiteDBUtil.Instance.Delete<StockStrategyEntity>($"BatchNo='{stockStrategy.BatchNo}' and ID<>{stockStrategy.ID}");
                    }
                }
            }
        }
    }
}
