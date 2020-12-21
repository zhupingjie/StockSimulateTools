using StockSimulateCore.Config;
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
            foreach (var stock in stocks)
            {
                var stockInfo = EastMoneyUtil.GetStockPrice(stock.Code);
                if (stockInfo == null) continue;;
                if (stockInfo.DayPrice.Price == 0) continue;

                //更新当前股票信息
                StockService.Update(stock, stockInfo);

                if (!RunningConfig.Instance.DebugMode)
                {
                    //更新当前股价
                    StockPriceService.Update(stock, stockInfo);

                    //检测自动交易策略 
                    StockStrategyService.CheckAutoRun(stock.Code, stockInfo.Stock.Price, DateTime.Now);
                }

                actionLog($"已采集[{stock.Name}]今日股价数据...[{stockInfo.DayPrice.Price}] [{stockInfo.DayPrice.UDPer}%]");
            }
            if(stocks.Length > 0) actionLog($">------------------------------------------------>");
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
                if (!ObjectUtil.ColudGatherFinanceReport(stock.ReportDate)) continue;

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
                    var mts = SQLiteDBUtil.Instance.QueryAll<BalanceTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
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
                    var mts = SQLiteDBUtil.Instance.QueryAll<BalanceTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=1 and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
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
                    var mts = SQLiteDBUtil.Instance.QueryAll<ProfitTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
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
                    var mts = SQLiteDBUtil.Instance.QueryAll<ProfitTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=1 and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
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
                    var mts = SQLiteDBUtil.Instance.QueryAll<ProfitTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0 and REPORTTYPE=2 and REPORTDATE in ('{string.Join("','", dates)}')");
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
                    var mts = SQLiteDBUtil.Instance.QueryAll<CashTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
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
                    var mts = SQLiteDBUtil.Instance.QueryAll<CashTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=1 and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
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
                    var mts = SQLiteDBUtil.Instance.QueryAll<CashTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0 and REPORTTYPE=2 and REPORTDATE in ('{string.Join("','", dates)}')");
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
            //if (stocks.Length > 0) actionLog($">------------------------------------------------>");
        }
    }
}
