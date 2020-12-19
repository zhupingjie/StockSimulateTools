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
    public class StockGatherService
    {
        /// <summary>
        /// 采集自选股信息
        /// </summary>
        /// <param name="actionLog"></param>
        public static void GatherPriceData(Action<string> actionLog)
        {
            var stocks = SQLiteDBUtil.Instance.QueryAll<StockEntity>();
            var accountStocks = SQLiteDBUtil.Instance.QueryAll<AccountStockEntity>();
            var stockStrategys = SQLiteDBUtil.Instance.QueryAll<StockStrategyEntity>($"ExecuteMode=1");
            foreach (var stock in stocks)
            {
                var stockInfo = EastMoneyUtil.GetStockPrice(stock.Code);
                if (stockInfo == null) continue;;
                if (stockInfo.DayPrice.Price == 0) continue;

                stockInfo.Stock.ID = stock.ID;
                stockInfo.Stock.Type = stock.Type;
                stockInfo.Stock.Target = stock.Target;
                stockInfo.Stock.Safety = stock.Safety;
                stockInfo.Stock.Foucs = stock.Foucs;
                stockInfo.Stock.Growth = stock.Growth;
                stockInfo.Stock.EPE = stock.EPE;
                stockInfo.Stock.Advise = stock.Advise;
                stockInfo.Stock.LockDay = stock.LockDay;
                SQLiteDBUtil.Instance.Update<StockEntity>(stockInfo.Stock);

                #region 更新当前股价
                var dealDate = DateTime.Now.ToString("yyyy-MM-dd");
                var price = SQLiteDBUtil.Instance.QueryFirst<StockPriceEntity>($"StockCode='{stock.Code}' and DealDate='{dealDate}' and DateType=0");
                if (price == null)
                {
                    stockInfo.DayPrice.DealTime = "";
                    SQLiteDBUtil.Instance.Insert<StockPriceEntity>(stockInfo.DayPrice);
                }
                else
                {
                    stockInfo.DayPrice.ID = price.ID;
                    stockInfo.DayPrice.DealTime = "";
                    SQLiteDBUtil.Instance.Update<StockPriceEntity>(stockInfo.DayPrice);
                }
                if (stock.Foucs > 0)
                {
                    var dealTime = DateTime.Now.ToString("HH:mm");
                    if (dealTime.CompareTo("15:00") >= 0) dealTime = "15:00";
                    if (dealTime.CompareTo("09:25") <= 0) dealTime = "09:25";

                    var price2 = SQLiteDBUtil.Instance.QueryFirst<StockPriceEntity>($"StockCode='{stock.Code}' and DealDate='{dealDate}' and DealTime='{dealTime}' and DateType=1");
                    if (price2 == null)
                    {
                        stockInfo.DayPrice.DateType = 1;//分钟
                        stockInfo.DayPrice.DealTime = dealTime;
                        SQLiteDBUtil.Instance.Insert<StockPriceEntity>(stockInfo.DayPrice);
                    }
                    else
                    {
                        stockInfo.DayPrice.ID = price2.ID;
                        stockInfo.DayPrice.DateType = 1;//分钟
                        stockInfo.DayPrice.DealTime = dealTime;
                        SQLiteDBUtil.Instance.Update<StockPriceEntity>(stockInfo.DayPrice);
                    }
                }
                #endregion

                #region 检测自动交易策略
                var runStrategys = stockStrategys.Where(c => c.StockCode == stock.Code
                    && (c.Condition == 0 && c.Price >= stock.Price || c.Condition == 1 && c.Price <= stock.Price)
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
                        Price = stockInfo.DayPrice.Price,
                        Qty = item.BuyQty
                    };
                    ExchangeResultInfo result = null;
                    if (item.BuyQty > 0)
                    {
                        result = StockExchangeService.Buy(exchangeInfo);
                    }
                    if (item.SaleQty > 0)
                    {
                        result = StockExchangeService.Sale(exchangeInfo);
                    }
                    if (result != null)
                    {
                        item.ExecuteOK = result.Success ? 1 : 2;
                        item.Message = result.Message;

                        //检测是否需要生成Next策略
                        if (result.Success)
                        {
                            //差价交易
                            if(item.StrategyInfoType == typeof(TExchangeStrategyInfo).FullName && !string.IsNullOrEmpty(item.StrategySource))
                            {
                                var strategyInfo = ServiceStack.Text.JsonSerializer.DeserializeFromString<TExchangeStrategyInfo>(item.StrategySource) as TExchangeStrategyInfo;
                                if (strategyInfo != null)
                                {
                                    if (item.BuyQty > 0)
                                    {
                                        strategyInfo.ActualSingleBuy += 1;
                                        strategyInfo.ActualSingleSale = 0;

                                        if (strategyInfo.ActualSingleBuy <= strategyInfo.MaxSingleBS)
                                        {
                                            var nextStrategys = StockStrategyService.MakeTExchangeStrategys(strategyInfo);
                                            SQLiteDBUtil.Instance.Insert<StockStrategyEntity>(nextStrategys.ToArray());
                                        }
                                    }
                                    if (item.SaleQty > 0)
                                    {
                                        strategyInfo.ActualSingleSale += 1;
                                        strategyInfo.ActualSingleBuy = 0;

                                        if (strategyInfo.ActualSingleSale <= strategyInfo.MaxSingleBS)
                                        {
                                            var nextStrategys = StockStrategyService.MakeTExchangeStrategys(strategyInfo);
                                            SQLiteDBUtil.Instance.Insert<StockStrategyEntity>(nextStrategys.ToArray());
                                        }
                                    }

                                    //删除批策略号的对立数据
                                    SQLiteDBUtil.Instance.Delete<StockStrategyEntity>($"BatchNo='{item.BatchNo}'");
                                }
                            }
                        }
                    }
                }
                SQLiteDBUtil.Instance.Update<StockStrategyEntity>(runStrategys);
                #endregion

                actionLog($"已采集[{stock.Name}]今日股价数据...[{stockInfo.DayPrice.Price}] [{stockInfo.DayPrice.UDPer}%]");
            }
            if(stocks.Length > 0) actionLog($">------------------------------------------------>");
        }

        /// <summary>
        /// 计算账户盈亏
        /// </summary>
        /// <param name="actionLog"></param>
        public static void CalculateProfit(Action<string> actionLog)
        {
            var stocks = SQLiteDBUtil.Instance.QueryAll<StockEntity>();
            var accountStocks = SQLiteDBUtil.Instance.QueryAll<AccountStockEntity>();
            foreach (var stock in stocks)
            {
                var accStocks = accountStocks.Where(c => c.StockCode == stock.Code).ToArray();
                foreach (var item in accStocks)
                {
                    item.Price = stock.Price;
                    item.HoldAmount = item.Price * item.HoldQty;
                    item.Profit = item.HoldAmount - item.TotalBuyAmount;
                    if (item.TotalBuyAmount == 0) item.UDPer = 0;
                    else item.UDPer = Math.Round(item.Profit / item.TotalBuyAmount * 100, 2);

                    actionLog($"已计算[{item.StockName}]持有股价盈亏...[{item.Profit}] [{item.UDPer}%]");
                }
            }
            SQLiteDBUtil.Instance.Update<AccountStockEntity>(accountStocks);

            if (stocks.Length > 0) actionLog($">------------------------------------------------>");
        }

        /// <summary>
        /// 采集自选股财务数据
        /// </summary>
        /// <param name="actionLog"></param>
        public static void GatherFinanceData(Action<string> actionLog)
        {
            var stocks = SQLiteDBUtil.Instance.QueryAll<StockEntity>($"Type=0");
            foreach (var stock in stocks)
            {
                #region 主要指标
                var mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 0);
                if (mainTargetInfos.Length > 0)
                {
                    var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                    var mts = SQLiteDBUtil.Instance.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=0 and Date in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                    var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                    if (newMts.Length > 0)
                    {
                        SQLiteDBUtil.Instance.Insert<MainTargetEntity>(newMts);
                    }
                }
                mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 1);
                if (mainTargetInfos.Length > 0)
                {
                    var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                    var mts = SQLiteDBUtil.Instance.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=1 and Date in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                    var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                    if (newMts.Length > 0)
                    {
                        SQLiteDBUtil.Instance.Insert<MainTargetEntity>(newMts);
                    }
                }
                mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 2);
                if (mainTargetInfos.Length > 0)
                {
                    var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                    var mts = SQLiteDBUtil.Instance.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=2 and Date in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                    var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                    if (newMts.Length > 0)
                    {
                        SQLiteDBUtil.Instance.Insert<MainTargetEntity>(newMts);
                    }
                }
                actionLog($"已采集[{stock.Name}]主要指标数据...");
                #endregion

                #region 资产负债表
                var balanceTargetInfos = EastMoneyUtil.GetBalanceTargets(stock.Code, 0, 1);
                if (balanceTargetInfos.Length > 0)
                {
                    var dates = balanceTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = SQLiteDBUtil.Instance.QueryAll<BalanceTargetEntity>($"StockCode='{stock.Code}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = balanceTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        SQLiteDBUtil.Instance.Insert<BalanceTargetEntity>(newMts);
                    }
                }
                balanceTargetInfos = EastMoneyUtil.GetBalanceTargets(stock.Code, 1, 1);
                if (balanceTargetInfos.Length > 0)
                {
                    var dates = balanceTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = SQLiteDBUtil.Instance.QueryAll<BalanceTargetEntity>($"StockCode='{stock.Code}' and REPORTDATETYPE=1 and REPORTTYPE=1and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = balanceTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        SQLiteDBUtil.Instance.Insert<BalanceTargetEntity>(newMts);
                    }
                }
                actionLog($"已采集[{stock.Name}]资产负债表数据...");
                #endregion

                #region 利润表
                var profitTargetInfos = EastMoneyUtil.GetProfitTargets(stock.Code, 0, 1);
                if (profitTargetInfos.Length > 0)
                {
                    var dates = profitTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = SQLiteDBUtil.Instance.QueryAll<ProfitTargetEntity>($"StockCode='{stock.Code}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = profitTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        SQLiteDBUtil.Instance.Insert<ProfitTargetEntity>(newMts);
                    }
                }
                profitTargetInfos = EastMoneyUtil.GetProfitTargets(stock.Code, 1, 1);
                if (profitTargetInfos.Length > 0)
                {
                    var dates = profitTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = SQLiteDBUtil.Instance.QueryAll<ProfitTargetEntity>($"StockCode='{stock.Code}' and REPORTDATETYPE=1 and REPORTTYPE=1and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = profitTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        SQLiteDBUtil.Instance.Insert<ProfitTargetEntity>(newMts);
                    }
                }
                profitTargetInfos = EastMoneyUtil.GetProfitTargets(stock.Code, 0, 2);
                if (profitTargetInfos.Length > 0)
                {
                    var dates = profitTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = SQLiteDBUtil.Instance.QueryAll<ProfitTargetEntity>($"StockCode='{stock.Code}' and REPORTDATETYPE=0 and REPORTTYPE=2 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = profitTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        SQLiteDBUtil.Instance.Insert<ProfitTargetEntity>(newMts);
                    }
                }
                actionLog($"已采集[{stock.Name}]利润表数据...");
                #endregion

                #region 现金流量表
                var cashTargetInfos = EastMoneyUtil.GetCashTargets(stock.Code, 0, 1);
                if(cashTargetInfos.Length > 0)
                {
                    var dates = cashTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = SQLiteDBUtil.Instance.QueryAll<CashTargetEntity>($"StockCode='{stock.Code}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = cashTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        SQLiteDBUtil.Instance.Insert<CashTargetEntity>(newMts);
                    }
                }
                cashTargetInfos = EastMoneyUtil.GetCashTargets(stock.Code, 1, 1);
                if (cashTargetInfos.Length > 0)
                {
                    var dates = cashTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = SQLiteDBUtil.Instance.QueryAll<CashTargetEntity>($"StockCode='{stock.Code}' and REPORTDATETYPE=1 and REPORTTYPE=1and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = cashTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        SQLiteDBUtil.Instance.Insert<CashTargetEntity>(newMts);
                    }
                }
                cashTargetInfos = EastMoneyUtil.GetCashTargets(stock.Code, 0, 2);
                if (cashTargetInfos.Length > 0)
                {
                    var dates = cashTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = SQLiteDBUtil.Instance.QueryAll<CashTargetEntity>($"StockCode='{stock.Code}' and REPORTDATETYPE=0 and REPORTTYPE=2 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = cashTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        SQLiteDBUtil.Instance.Insert<CashTargetEntity>(newMts);
                    }
                }
                actionLog($"已采集[{stock.Name}]现金流量表数据...");
                #endregion
            }
            if (stocks.Length > 0) actionLog($">------------------------------------------------>");
        }
    }
}
