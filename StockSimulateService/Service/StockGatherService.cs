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
            var stocks = Repository.Instance.QueryAll<StockEntity>($"", "ID asc");
            foreach (var stock in stocks)
            {
                bool newStock = stock.Price == 0;
                var stockInfo = EastMoneyUtil.GetStockPrice(stock.Code);
                if (stockInfo == null) continue;
                if (stockInfo.DayPrice.Price == 0) continue;

                //更新当前股票信息
                StockService.Update(stock, stockInfo);

                if (ObjectUtil.EffectStockDealTime(1))
                {
                    //更新当前股价
                    StockPriceService.Update(stock, stockInfo);

                    //当天日期=股价日期（开市）
                    if (stockInfo.Stock.PriceDate == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        //检测自动交易策略 
                        StockStrategyService.CheckRun(stock.Code, stockInfo.Stock.Price, DateTime.Now);
                    }
                }

                //采集历史价格数据
                if (newStock)
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
        public static void GatherFundStockData(Action<string> actionLog)
        {
            var stocks = Repository.Instance.QueryAll<StockEntity>($"Type=1");
            foreach (var stock in stocks)
            {
                if (!ObjectUtil.ColudGatherFundStock(stock.ReportDate)) continue;

                var fundStocks = EastMoneyUtil.GetFundStock(stock.Code);
                if (fundStocks.Length > 0)
                {
                    var date = fundStocks.Max(c => c.ReportDate);
                    Repository.Instance.Delete<FundStockEntity>($"StockCode='{stock.Code}' and ReportDate='{date}'");
                    Repository.Instance.Insert<FundStockEntity>(fundStocks);

                    //同步更新报告期
                    stock.ReportDate = fundStocks.Max(c => c.ReportDate);
                    Repository.Instance.Update<StockEntity>(stock, new string[] { "ReportDate" });
                }
            }
        }

        /// <summary>
        /// 采集自选股财务数据
        /// </summary>
        /// <param name="actionLog"></param>
        public static void GatherFinanceData(Action<string> actionLog)
        {
            var stocks = Repository.Instance.QueryAll<StockEntity>($"Type=0");
            foreach (var stock in stocks)
            {
                if (!ObjectUtil.ColudGatherFinanceReport(stock.ReportDate)) continue;

                #region 主要指标
                var mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 0);
                if (mainTargetInfos.Length > 0)
                {
                    var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=0 and Date in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                    var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<MainTargetEntity>(newMts);
                    }
                }
                mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 1);
                if (mainTargetInfos.Length > 0)
                {
                    var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=1 and Date in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                    var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<MainTargetEntity>(newMts);
                    }
                }
                mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 2);
                if (mainTargetInfos.Length > 0)
                {
                    var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=2 and Date in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                    var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<MainTargetEntity>(newMts);

                        actionLog($"已采集[{stock.Name}]主要指标数据...[{newMts.Length}份]");
                    }
                }
                #endregion

                #region 资产负债表
                var balanceTargetInfos = EastMoneyUtil.GetBalanceTargets(stock.Code, 0, 1);
                if (balanceTargetInfos.Length > 0)
                {
                    var dates = balanceTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<BalanceTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = balanceTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<BalanceTargetEntity>(newMts);
                    }
                }
                balanceTargetInfos = EastMoneyUtil.GetBalanceTargets(stock.Code, 1, 1);
                if (balanceTargetInfos.Length > 0)
                {
                    var dates = balanceTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<BalanceTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=1 and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = balanceTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<BalanceTargetEntity>(newMts);

                        actionLog($"已采集[{stock.Name}]资产负债表数据...[{newMts.Length}份]");
                    }
                }
                #endregion

                #region 利润表
                var profitTargetInfos = EastMoneyUtil.GetProfitTargets(stock.Code, 0, 1);
                if (profitTargetInfos.Length > 0)
                {
                    var dates = profitTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<ProfitTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = profitTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<ProfitTargetEntity>(newMts);
                    }
                }
                profitTargetInfos = EastMoneyUtil.GetProfitTargets(stock.Code, 1, 1);
                if (profitTargetInfos.Length > 0)
                {
                    var dates = profitTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<ProfitTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=1 and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = profitTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<ProfitTargetEntity>(newMts);
                    }
                }
                profitTargetInfos = EastMoneyUtil.GetProfitTargets(stock.Code, 0, 2);
                if (profitTargetInfos.Length > 0)
                {
                    var dates = profitTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<ProfitTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0 and REPORTTYPE=2 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = profitTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<ProfitTargetEntity>(newMts);

                        actionLog($"已采集[{stock.Name}]利润表数据...[{newMts.Length}份]");
                    }
                }
                #endregion

                #region 现金流量表
                var cashTargetInfos = EastMoneyUtil.GetCashTargets(stock.Code, 0, 1);
                if (cashTargetInfos.Length > 0)
                {
                    var dates = cashTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<CashTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0  and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = cashTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<CashTargetEntity>(newMts);
                    }
                }
                cashTargetInfos = EastMoneyUtil.GetCashTargets(stock.Code, 1, 1);
                if (cashTargetInfos.Length > 0)
                {
                    var dates = cashTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<CashTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=1 and REPORTTYPE=1 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = cashTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<CashTargetEntity>(newMts);
                    }
                }
                cashTargetInfos = EastMoneyUtil.GetCashTargets(stock.Code, 0, 2);
                if (cashTargetInfos.Length > 0)
                {
                    var dates = cashTargetInfos.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var mts = Repository.Instance.QueryAll<CashTargetEntity>($"SECURITYCODE='{stock.Code}' and REPORTDATETYPE=0 and REPORTTYPE=2 and REPORTDATE in ('{string.Join("','", dates)}')");
                    var mtDates = mts.Select(c => c.REPORTDATE).Distinct().ToArray();
                    var newMts = cashTargetInfos.Where(c => !mtDates.Contains(c.REPORTDATE)).ToArray();
                    if (newMts.Length > 0)
                    {
                        Repository.Instance.Insert<CashTargetEntity>(newMts);

                        actionLog($"已采集[{stock.Name}]现金流量表数据...[{newMts.Length}份]");
                    }
                }
                #endregion

                //同步更新报告期
                stock.ReportDate = mainTargetInfos.Max(c => c.Date);
                Repository.Instance.Update<StockEntity>(stock, new string[] { "ReportDate" });
            }

            //if (stocks.Length > 0) actionLog($">------------------------------------------------>");
        }

        /// <summary>
        /// 采集机构延保数据
        /// </summary>
        /// <param name="actionLog"></param>
        public static void GatherReportData(Action<string> actionLog)
        {
            var stocks = Repository.Instance.QueryAll<StockEntity>($"Type=0");
            foreach (var stock in stocks)
            {
                var reports = EastMoneyUtil.GetStockReports(stock.Code);
                var codes = reports.Select(c => c.PdfCode).ToArray();

                var hasReports = Repository.Instance.QueryAll<ReportEntity>($"StockCode='{stock.Code}'  and PdfCode in ('{string.Join("','", codes)}')");
                var hasCodes = hasReports.Select(c => c.PdfCode).Distinct().ToArray();

                var newReports = reports.Where(c => !hasCodes.Contains(c.PdfCode)).ToArray();
                if (newReports.Length > 0)
                {
                    Repository.Instance.Insert<ReportEntity>(newReports);

                    actionLog($"已采集[{stock.Name}]机构研报数据...[{newReports.Length}份]");
                }
            }
            if (true)
            {
                var endDate = DateTime.Now.Date;
                var reports = EastMoneyUtil.GetIndustryReports(DateTime.Now.Date.AddDays(-1 * RunningConfig.Instance.GatherIndustryReportPreDays), DateTime.Now.Date);
                var codes = reports.Select(c => c.PdfCode).ToArray();

                var hasReports = Repository.Instance.QueryAll<ReportEntity>($"PdfCode in ('{string.Join("','", codes)}')");
                var hasCodes = hasReports.Select(c => c.PdfCode).Distinct().ToArray();

                var newReports = reports.Where(c => !hasCodes.Contains(c.PdfCode)).ToArray();
                if (newReports.Length > 0)
                {
                    Repository.Instance.Insert<ReportEntity>(newReports);

                    actionLog($"已采集行业研报数据...[{newReports.Length}份]");
                }
            }
        }

        /// <summary>
        /// 采集机构预测数据
        /// </summary>
        /// <param name="actionLog"></param>
        public static void GatherForecastData(Action<string> actionLog)
        {
            var stocks = Repository.Instance.QueryAll<StockEntity>($"Type=0");
            foreach (var stock in stocks)
            {
                var forecastInfo = EastMoneyUtil.GetStockForecast(stock.Code);
                if (forecastInfo.mgsy == null || forecastInfo.jzcsyl == null || forecastInfo.gsjlr == null || forecastInfo.jgyc == null || forecastInfo.jgyc.data == null) continue;

                var updateStocks = new List<StockEntity>();
                var newForecasts = new List<StockForecastEntity>();
                var years = forecastInfo.mgsy.Select(c => c.Year).ToArray();

                var actualYear = $"{forecastInfo.jgyc.baseYear}A";
                var hasForecasts = Repository.Instance.QueryAll<StockForecastEntity>($"StockCode='{stock.Code}' and BaseYear='{actualYear}'");
                if (!hasForecasts.Any(c => c.BaseYear == actualYear && string.IsNullOrEmpty(c.TargetYear)))
                {                    
                    var newForecast = new StockForecastEntity()
                    {
                        StockCode = stock.Code,
                        BaseYear = actualYear,
                        TargetYear = ""
                    };
                    var mgsy = forecastInfo.mgsy.FirstOrDefault(c => c.Year == actualYear);
                    if(mgsy != null)
                    {
                        newForecast.Mgsy = mgsy.Value;
                        newForecast.MgsyRaito = mgsy.Ratio == "-" ? 0 : ObjectUtil.ToValue<decimal>(mgsy.Ratio);
                    }
                    var jzcsyl = forecastInfo.jzcsyl.FirstOrDefault(c => c.Year == actualYear);
                    if (jzcsyl != null)
                    {
                        newForecast.ROE = jzcsyl.Value;
                    }
                    var gsjlr = forecastInfo.gsjlr.FirstOrDefault(c => c.Year == actualYear);
                    if (gsjlr != null)
                    {
                        newForecast.Gsjlr = Math.Round(gsjlr.Value / 100000000, 3);
                    }
                    var yysr = forecastInfo.yysr.FirstOrDefault(c => c.Year == actualYear);
                    if (yysr != null)
                    {
                        newForecast.Yysr = Math.Round(yysr.Value / 100000000, 3);
                    }
                    var jgyc = forecastInfo.jgyc.data.FirstOrDefault(c => c.Jgmc.EndsWith("平均"));
                    if(jgyc != null)
                    {
                        newForecast.PE = jgyc.Syl;
                    }
                    newForecasts.Add(newForecast);

                    //同步ROE
                    stock.ROE = newForecast.ROE;
                }
                for (var i = 1; i <= 3; i++)
                {
                    var ycYear = $"{forecastInfo.jgyc.baseYear + i}E";
                    if (!hasForecasts.Any(c => c.BaseYear == actualYear && c.TargetYear == ycYear))
                    {
                        var newForecast = new StockForecastEntity()
                        {
                            StockCode = stock.Code,
                            BaseYear = actualYear,
                            TargetYear = ycYear
                        };
                        var mgsy = forecastInfo.mgsy.FirstOrDefault(c => c.Year == ycYear);
                        if (mgsy != null)
                        {
                            newForecast.Mgsy = mgsy.Value;
                            newForecast.MgsyRaito = mgsy.Ratio == "-" ? 0 : ObjectUtil.ToValue<decimal>(mgsy.Ratio);
                        }
                        var jzcsyl = forecastInfo.jzcsyl.FirstOrDefault(c => c.Year == ycYear);
                        if (jzcsyl != null)
                        {
                            newForecast.ROE = jzcsyl.Value;
                        }
                        var gsjlr = forecastInfo.gsjlr.FirstOrDefault(c => c.Year == ycYear);
                        if (gsjlr != null)
                        {
                            newForecast.Gsjlr = Math.Round(gsjlr.Value / 100000000, 3);
                        }
                        var yysr = forecastInfo.yysr.FirstOrDefault(c => c.Year == ycYear);
                        if (yysr != null)
                        {
                            newForecast.Yysr = Math.Round(yysr.Value / 100000000, 3);
                        }
                        var jgyc = forecastInfo.jgyc.data.FirstOrDefault(c => c.Jgmc.EndsWith("平均"));
                        if (jgyc != null)
                        {
                            newForecast.PE = (i == 1 ? jgyc.Syl1 : i == 2 ? jgyc.Syl2 : jgyc.Syl3);
                        }
                        newForecasts.Add(newForecast);

                        if (i == 1)
                        {
                            //同步机构预测结果
                            stock.EPE = newForecast.PE;
                            stock.ENetProfit = newForecast.Gsjlr;
                            stock.EGrowth = newForecast.MgsyRaito;
                            stock.EEPS = newForecast.Mgsy;
                            stock.EPrice = stock.EEPS * stock.EPE;
                            updateStocks.Add(stock);
                        }
                    }
                }
                if (updateStocks.Count > 0)
                {
                    Repository.Instance.Update<StockEntity>(updateStocks.ToArray(), new string[] { "ROE", "EPE", "ENetProfit", "EGrowth", "EEPS", "EPrice" });
                }
                if (newForecasts.Count > 0)
                {
                    Repository.Instance.Insert<StockForecastEntity>(newForecasts.ToArray());

                    actionLog($"已采集[{stock.Name}]机构预测数据...[{newForecasts.Count}份]");
                }
            }
        }
    }
}