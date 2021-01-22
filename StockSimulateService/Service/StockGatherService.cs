using StockSimulateDomain.Entity;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateService.Helper;
using StockSimulateService.Service;
using StockSimulateCore.Config;
using StockSimulateCore.Data;
using StockSimulateDomain.Model;

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
            var stocks = Repository.Instance.QueryAll<StockEntity>(null);
            foreach (var stock in stocks)
            {
                var stockInfo = EastMoneyUtil.GetStockPrice(stock.Code);
                if (stockInfo == null) continue; ;
                if (stockInfo.DayPrice.Price == 0) continue;

                //更新当前股票信息
                StockService.Update(stock, stockInfo);

                if (ObjectUtil.EffectStockDealTime(1))
                {
                    //更新当前股价
                    StockPriceService.Update(stock, stockInfo);

                    //检测自动交易策略 
                    StockStrategyService.CheckRun(stock.Code, stockInfo.Stock.Price, DateTime.Now);

                    ////减少日志输出,每5分钟输出一次
                    //if(DateTime.Now.Minute % 5 == 0)
                    //    actionLog($"已采集[{stock.StockName}]今日股价数据...[{stockInfo.DayPrice.Price}] [{stockInfo.DayPrice.UDPer}%]");
                }

                //采集历史价格数据
                if (RunningConfig.Instance.DebugMode)
                {
                    var gatherCount = GatherHisPriceData(stock.Code);
                    if (gatherCount > 0)
                    {
                        actionLog($"已采集[{stock.Name}]历史股价数据...[{gatherCount}天]");
                    }
                }
            }
        }

        /// <summary>
        /// 采集股票历史股价数据
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        public static int GatherHisPriceData(string stockCode)
        {
            var priceIds = Repository.Instance.QueryAll<StockPriceEntity>($"StockCode='{stockCode}'", "ID desc", 20, new string[] { "ID" });
            if (priceIds.Length >= 20) return 0;

            var stockPrices = EastMoneyUtil.GetStockHisPrice(stockCode);
            if (stockPrices == null) return 0;

            var lastPrice = Repository.Instance.QueryAll<StockPriceEntity>($"StockCode='{stockCode}'", "DealDate asc", 1).FirstOrDefault();
            if (lastPrice == null)
            {
                var newPrices = stockPrices.OrderByDescending(c => c.DealDate).ToArray();
                Repository.Instance.Insert<StockPriceEntity>(newPrices);
                return newPrices.Length;
            }
            else
            {
                var lastDate = lastPrice.DealDate;
                var newPrices = stockPrices.Where(c => c.DealDate.CompareTo(lastDate) < 0).OrderByDescending(c => c.DealDate).ToArray();
                Repository.Instance.Insert<StockPriceEntity>(newPrices);
                return newPrices.Length;
            }
        }

        /// <summary>
        /// 采集基金持仓比例数据
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        public static void GatherFundStockData(IList<StockCacheInfo> stockCaches, Action<string> actionLog)
        {
            var stocks = stockCaches.Where(c => c.Type == 1).ToArray();
            foreach (var stockCache in stocks)
            {
                if (!ObjectUtil.ColudGatherFundStock(stockCache.ReportDate)) continue;

                var fundStocks = EastMoneyUtil.GetFundStock(stockCache.StockCode);
                if (fundStocks.Length > 0)
                {
                    var date = fundStocks.Max(c => c.ReportDate);
                    Repository.Instance.Delete<FundStockEntity>($"StockCode='{stockCache.StockCode}' and ReportDate='{date}'");
                    Repository.Instance.Insert<FundStockEntity>(fundStocks);

                    //同步更新报告期
                    var stock = Repository.Instance.QueryFirst<StockEntity>($"Code='{stockCache}'");
                    if (stock != null)
                    {
                        stock.ReportDate = fundStocks.Max(c => c.ReportDate);
                        stockCache.ReportDate = stock.ReportDate;
                        Repository.Instance.Update<StockEntity>(stock, new string[] { "ReportDate" });
                    }
                }
            }
        }

        /// <summary>
        /// 采集自选股财务数据
        /// </summary>
        /// <param name="actionLog"></param>
        public static void GatherFinanceData(IList<StockCacheInfo> stockCaches, Action<string> actionLog)
        {
            var stocks = stockCaches.Where(c => c.Type == 0).ToArray();
            foreach (var stock in stocks)
            {
                if (!ObjectUtil.ColudGatherFinanceReport(stock.ReportDate)) continue;

                #region 主要指标
                var mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.StockCode, 0);
                if (mainTargetInfos.Length > 0)
                {
                    var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<MainTargetEntity>($"StockCode='{stock.StockCode}' and Rtype=0 and Date in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                    var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<MainTargetEntity>(newMts);
                    }
                }
                mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.StockCode, 1);
                if (mainTargetInfos.Length > 0)
                {
                    var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<MainTargetEntity>($"StockCode='{stock.StockCode}' and Rtype=1 and Date in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                    var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<MainTargetEntity>(newMts);
                    }
                }
                mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.StockCode, 2);
                if (mainTargetInfos.Length > 0)
                {
                    var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<MainTargetEntity>($"StockCode='{stock.StockCode}' and Rtype=2 and Date in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                    var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<MainTargetEntity>(newMts);

                        actionLog($"已采集[{stock.StockName}]主要指标数据...[{newMts.Length}份]");
                    }
                }
                #endregion

                #region 资产负债表
                var balanceTargetInfos = EastMoneyUtil.GetBalanceTargets(stock.StockCode, 0, 1);
                if (balanceTargetInfos.Length > 0)
                {
                    var dates = balanceTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<BalanceTargetEntity>($"SECURITYCODE='{stock.StockCode}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = balanceTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<BalanceTargetEntity>(newMts);
                    }
                }
                balanceTargetInfos = EastMoneyUtil.GetBalanceTargets(stock.StockCode, 1, 1);
                if (balanceTargetInfos.Length > 0)
                {
                    var dates = balanceTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<BalanceTargetEntity>($"SECURITYCODE='{stock.StockCode}' and REPORTDATETYPE=1 and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = balanceTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<BalanceTargetEntity>(newMts);

                        actionLog($"已采集[{stock.StockName}]资产负债表数据...[{newMts.Length}份]");
                    }
                }
                #endregion

                #region 利润表
                var profitTargetInfos = EastMoneyUtil.GetProfitTargets(stock.StockCode, 0, 1);
                if (profitTargetInfos.Length > 0)
                {
                    var dates = profitTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<ProfitTargetEntity>($"SECURITYCODE='{stock.StockCode}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = profitTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<ProfitTargetEntity>(newMts);
                    }
                }
                profitTargetInfos = EastMoneyUtil.GetProfitTargets(stock.StockCode, 1, 1);
                if (profitTargetInfos.Length > 0)
                {
                    var dates = profitTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<ProfitTargetEntity>($"SECURITYCODE='{stock.StockCode}' and REPORTDATETYPE=1 and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = profitTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<ProfitTargetEntity>(newMts);
                    }
                }
                profitTargetInfos = EastMoneyUtil.GetProfitTargets(stock.StockCode, 0, 2);
                if (profitTargetInfos.Length > 0)
                {
                    var dates = profitTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<ProfitTargetEntity>($"SECURITYCODE='{stock.StockCode}' and REPORTDATETYPE=0 and REPORTTYPE=2 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = profitTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<ProfitTargetEntity>(newMts);

                        actionLog($"已采集[{stock.StockName}]利润表数据...[{newMts.Length}份]");
                    }
                }
                #endregion

                #region 现金流量表
                var cashTargetInfos = EastMoneyUtil.GetCashTargets(stock.StockCode, 0, 1);
                if (cashTargetInfos.Length > 0)
                {
                    var dates = cashTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<CashTargetEntity>($"SECURITYCODE='{stock.StockCode}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = cashTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<CashTargetEntity>(newMts);
                    }
                }
                cashTargetInfos = EastMoneyUtil.GetCashTargets(stock.StockCode, 1, 1);
                if (cashTargetInfos.Length > 0)
                {
                    var dates = cashTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<CashTargetEntity>($"SECURITYCODE='{stock.StockCode}' and REPORTDATETYPE=1 and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = cashTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<CashTargetEntity>(newMts);
                    }
                }
                cashTargetInfos = EastMoneyUtil.GetCashTargets(stock.StockCode, 0, 2);
                if (cashTargetInfos.Length > 0)
                {
                    var dates = cashTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<CashTargetEntity>($"SECURITYCODE='{stock.StockCode}' and REPORTDATETYPE=0 and REPORTTYPE=2 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = cashTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<CashTargetEntity>(newMts);

                        actionLog($"已采集[{stock.StockName}]现金流量表数据...[{newMts.Length}份]");
                    }
                }
                #endregion

                var dbStock = Repository.Instance.QueryFirst<StockEntity>($"Code='{stock.StockCode}'");
                if (dbStock != null)
                {
                    //同步更新报告期
                    dbStock.ReportDate = mainTargetInfos.Max(c => c.Date);
                    stock.ReportDate = dbStock.ReportDate;
                    Repository.Instance.Update<StockEntity>(dbStock, new string[] { "ReportDate" });
                }
            }

            //if (stocks.Length > 0) actionLog($">------------------------------------------------>");
        }

        /// <summary>
        /// 采集机构延保数据
        /// </summary>
        /// <param name="actionLog"></param>
        public static void GatherReportData(IList<StockCacheInfo> stockCaches, Action<string> actionLog)
        {
            var stocks = stockCaches.Where(c => c.Type == 0).ToArray();
            foreach (var stock in stocks)
            {
                var reports = EastMoneyUtil.GetReports(stock.StockCode);
                var codes = reports.Select(c => c.PdfCode).ToArray();

                var hasReports = Repository.Instance.QueryAll<ReportEntity>($"StockCode='{stock.StockCode}'  and PdfCode in ('{string.Join("','", codes)}')");
                var hasCodes = hasReports.Select(c => c.PdfCode).Distinct().ToArray();

                var newReports = reports.Where(c => !hasCodes.Contains(c.PdfCode)).ToArray();
                if (newReports.Length > 0)
                {
                    Repository.Instance.Insert<ReportEntity>(newReports);

                    actionLog($"已采集[{stock.StockName}]机构研报数据...[{newReports.Length}份]");
                }
            }
        }
    }
}