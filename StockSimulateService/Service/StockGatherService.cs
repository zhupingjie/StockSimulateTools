using StockSimulateDomain.Entity;
using StockSimulateService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateService.Utils;
using StockSimulateService.Helper;
using StockSimulateService.Service;

namespace StockSimulateNetService.Serivce
{
    public class StockGatherService
    {
        /// <summary>
        /// 采集自选股信息
        /// </summary>
        /// <param name="actionLog"></param>
        public static void GatherPriceData(Action<string> actionLog)
        {
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>();
            foreach (var stock in stocks)
            {
                var stockInfo = EastMoneyUtil.GetStockPrice(stock.Code);
                if (stockInfo == null) continue; ;
                if (stockInfo.DayPrice.Price == 0) continue;

                //更新当前股票信息
                StockService.Update(stock, stockInfo);

                if (ObjectUtil.EffectStockDealTime())
                {
                    //更新当前股价
                    StockPriceService.Update(stock, stockInfo);

                    //检测自动交易策略 
                    StockStrategyService.CheckRun(stock.Code, stockInfo.Stock.Price, DateTime.Now);
                }

                actionLog($"已采集[{stock.Name}]今日股价数据...[{stockInfo.DayPrice.Price}] [{stockInfo.DayPrice.UDPer}%]");
            }
            if (stocks.Length > 0) actionLog($">------------------------------------------------>");
        }

        public static void GatherHisPriceData()
        {
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>();
            foreach (var stock in stocks)
            {
                var stockPrices = EastMoneyUtil.GetStockHisPrice(stock.Code);
                if (stockPrices == null) continue;

                var lastPrice = MySQLDBUtil.Instance.QueryAll<StockPriceEntity>($"StockCode='{stock.Code}'", "DealDate asc", 1).FirstOrDefault();
                if (lastPrice == null)
                {
                    var newPrices = stockPrices.OrderByDescending(c => c.DealDate).ToArray();
                    MySQLDBUtil.Instance.Insert<StockPriceEntity>(newPrices);
                }
                else
                {
                    var lastDate = lastPrice.DealDate;
                    var newPrices = stockPrices.Where(c => c.DealDate.CompareTo(lastDate) < 0).OrderByDescending(c => c.DealDate).ToArray();
                    MySQLDBUtil.Instance.Insert<StockPriceEntity>(newPrices);
                }
            }
        }

        /// <summary>
        /// 采集自选股财务数据
        /// </summary>
        /// <param name="actionLog"></param>
        public static void GatherFinanceData(Action<string> actionLog)
        {
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>($"Type=0");
            foreach (var stock in stocks)
            {
                if (!ObjectUtil.ColudGatherFinanceReport(stock.ReportDate)) continue;

                #region 主要指标
                var mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 0);
                if (mainTargetInfos.Length > 0)
                {
                    var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                    var mts = MySQLDBUtil.Instance.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=0 and Date in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                    var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                    if (newMts.Length > 0)
                    {
                        MySQLDBUtil.Instance.Insert<MainTargetEntity>(newMts);
                    }
                }
                mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 1);
                if (mainTargetInfos.Length > 0)
                {
                    var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                    var mts = MySQLDBUtil.Instance.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=1 and Date in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                    var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                    if (newMts.Length > 0)
                    {
                        MySQLDBUtil.Instance.Insert<MainTargetEntity>(newMts);
                    }
                }
                mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 2);
                if (mainTargetInfos.Length > 0)
                {
                    var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                    var mts = MySQLDBUtil.Instance.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=2 and Date in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                    var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                    if (newMts.Length > 0)
                    {
                        MySQLDBUtil.Instance.Insert<MainTargetEntity>(newMts);

                        actionLog($"已采集[{stock.Name}]主要指标数据...[{newMts.Length}份]");
                    }
                }
                #endregion

                #region 资产负债表
                var balanceTargetInfos = EastMoneyUtil.GetBalanceTargets(stock.Code, 0, 1);
                if (balanceTargetInfos.Length > 0)
                {
                    var dates = balanceTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = MySQLDBUtil.Instance.QueryAll<BalanceTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = balanceTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        MySQLDBUtil.Instance.Insert<BalanceTargetEntity>(newMts);
                    }
                }
                balanceTargetInfos = EastMoneyUtil.GetBalanceTargets(stock.Code, 1, 1);
                if (balanceTargetInfos.Length > 0)
                {
                    var dates = balanceTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = MySQLDBUtil.Instance.QueryAll<BalanceTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=1 and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = balanceTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        MySQLDBUtil.Instance.Insert<BalanceTargetEntity>(newMts);

                        actionLog($"已采集[{stock.Name}]资产负债表数据...[{newMts.Length}份]");
                    }
                }
                #endregion

                #region 利润表
                var profitTargetInfos = EastMoneyUtil.GetProfitTargets(stock.Code, 0, 1);
                if (profitTargetInfos.Length > 0)
                {
                    var dates = profitTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = MySQLDBUtil.Instance.QueryAll<ProfitTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = profitTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        MySQLDBUtil.Instance.Insert<ProfitTargetEntity>(newMts);
                    }
                }
                profitTargetInfos = EastMoneyUtil.GetProfitTargets(stock.Code, 1, 1);
                if (profitTargetInfos.Length > 0)
                {
                    var dates = profitTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = MySQLDBUtil.Instance.QueryAll<ProfitTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=1 and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = profitTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        MySQLDBUtil.Instance.Insert<ProfitTargetEntity>(newMts);
                    }
                }
                profitTargetInfos = EastMoneyUtil.GetProfitTargets(stock.Code, 0, 2);
                if (profitTargetInfos.Length > 0)
                {
                    var dates = profitTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = MySQLDBUtil.Instance.QueryAll<ProfitTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0 and REPORTTYPE=2 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = profitTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        MySQLDBUtil.Instance.Insert<ProfitTargetEntity>(newMts);

                        actionLog($"已采集[{stock.Name}]利润表数据...[{newMts.Length}份]");
                    }
                }
                #endregion

                #region 现金流量表
                var cashTargetInfos = EastMoneyUtil.GetCashTargets(stock.Code, 0, 1);
                if (cashTargetInfos.Length > 0)
                {
                    var dates = cashTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = MySQLDBUtil.Instance.QueryAll<CashTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = cashTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        MySQLDBUtil.Instance.Insert<CashTargetEntity>(newMts);
                    }
                }
                cashTargetInfos = EastMoneyUtil.GetCashTargets(stock.Code, 1, 1);
                if (cashTargetInfos.Length > 0)
                {
                    var dates = cashTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = MySQLDBUtil.Instance.QueryAll<CashTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=1 and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = cashTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        MySQLDBUtil.Instance.Insert<CashTargetEntity>(newMts);
                    }
                }
                cashTargetInfos = EastMoneyUtil.GetCashTargets(stock.Code, 0, 2);
                if (cashTargetInfos.Length > 0)
                {
                    var dates = cashTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = MySQLDBUtil.Instance.QueryAll<CashTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0 and REPORTTYPE=2 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = cashTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        MySQLDBUtil.Instance.Insert<CashTargetEntity>(newMts);

                        actionLog($"已采集[{stock.Name}]现金流量表数据...[{newMts.Length}份]");
                    }
                }
                #endregion

                //同步更新报告期
                stock.ReportDate = mainTargetInfos.Max(c => c.Date);
                MySQLDBUtil.Instance.Update<StockEntity>(stock);
            }

            //if (stocks.Length > 0) actionLog($">------------------------------------------------>");
        }

        /// <summary>
        /// 采集机构延保数据
        /// </summary>
        /// <param name="actionLog"></param>
        public static void GatherReportData(Action<string> actionLog)
        {
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>($"Type=0");
            foreach (var stock in stocks)
            {
                var reports = EastMoneyUtil.GetReports(stock.Code);
                var codes = reports.Select(c => c.PdfCode).ToArray();

                var hasReports = MySQLDBUtil.Instance.QueryAll<ReportEntity>($"StockCode='{stock.Code}'  and PdfCode in ('{string.Join("','", codes)}')");
                var hasCodes = hasReports.Select(c => c.PdfCode).Distinct().ToArray();

                var newReports = reports.Where(c => !hasCodes.Contains(c.PdfCode)).ToArray();
                if (newReports.Length > 0)
                {
                    MySQLDBUtil.Instance.Insert<ReportEntity>(newReports);

                    actionLog($"已采集[{stock.Name}]机构研报数据...[{newReports.Length}份]");
                }
            }
        }
    }
}