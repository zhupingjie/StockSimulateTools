using ServiceStack;
using StockSimulateDomain.Entity;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateService.Helper
{
    public class EastMoneyUtil
    {
        #region 股价及基本信息
        public static StockInfo GetStockPrice(string code)
        {
            try
            {
                var secid = GetStockSecid(code);
                var api = $"http://push2.eastmoney.com/api/qt/stock/get?fltt=2&secid={secid}&fields=f127,f43,f57,f58,f169,f170,f46,f44,f51,f168,f47,f164,f163,f116,f60,f45,f52,f50,f48,f167,f117,f71,f161,f49,f530,f135,f136,f137,f138,f139,f141,f142,f144,f145,f147,f148,f140,f143,f146,f149,f55,f62,f162,f92,f173,f104,f105,f84,f85,f183,f184,f185,f186,f187,f188,f189,f190,f191,f192,f206,f207,f208,f209,f210,f211,f212,f213,f214,f215,f78,f86,f107,f111,f177,f110,f262,f263,f264,f267,f268,f250,f251,f252,f253,f254,f255,f256,f257,f258,f266,f269,f270,f271,f273,f274,f275,f292";
                var retStr = api.PostJsonToUrl(string.Empty, requestFilter =>
                {
                    requestFilter.Timeout = 5 * 60 * 1000;
                });
                var apiModel = ServiceStack.Text.JsonSerializer.DeserializeFromString<EastMoneyAPIModel>(retStr);
                if (apiModel == null || apiModel.data == null) return null;
                var model = apiModel.data;

                var stockPrice = new StockPriceEntity();
                stockPrice.StockCode = code;
                stockPrice.DealDate = ObjectUtil.StampToDateTime(GetLongValue(model, "f86")).ToString("yyyy-MM-dd"); //DateTime.Now.ToString("yyyy-MM-dd");
                stockPrice.DealTime = DateTime.Now.ToString("HH:mm");
                stockPrice.Price = GetNumberValue(model, "f43");
                stockPrice.UDPer = GetNumberValue(model, "f170");
                stockPrice.TodayStartPrice = GetNumberValue(model, "f46");
                stockPrice.TodayMaxPrice = GetNumberValue(model, "f44");
                stockPrice.TodayMinPrice = GetNumberValue(model, "f45");
                stockPrice.TodayEndPrice = GetNumberValue(model, "f43");
                stockPrice.YesterdayEndPrice = GetNumberValue(model, "f60");
                stockPrice.DealQty = GetTenThousandValue(model, "f47");
                stockPrice.DealAmount = GetMillionValue(model, "f48");
                stockPrice.Capital = GetMillionValue(model, "f84");
                stockPrice.Amount = GetMillionValue(model, "f116");
                stockPrice.TTM = GetNumberValue(model, "f163");
                stockPrice.PE = GetNumberValue(model, "f162");

                var stock = new StockEntity();
                stock.PriceDate = stockPrice.DealDate;
                stock.Code = code;
                stock.IndustryName = GetStringValue(model, "f127");
                stock.Name = GetStringValue(model, "f58");
                stock.UDPer = GetNumberValue(model, "f170");
                stock.EPS = GetNumberValue(model, "f55");
                stock.BVPS = GetNumberValue(model, "f92");
                stock.PE = GetNumberValue(model, "f162");
                stock.TTM = GetNumberValue(model, "f164");
                stock.PB = GetNumberValue(model, "f167");
                stock.PEG = 0;
                stock.ROE = 0;
                stock.ROIC = 0;
                stock.Capital = GetMillionValue(model, "f84");
                stock.Amount = GetMillionValue(model, "f116");
                stock.Price = GetNumberValue(model, "f43");
                stock.DebtRage = GetNumberValue(model, "f188");
                stock.GrossRate = GetNumberValue(model, "f186");
                stock.NetRate = GetNumberValue(model, "f187");
                stock.NetProfit = GetMillionValue(model, "f105");
                stock.TotalRevenue = GetMillionValue(model, "f183");
                stock.RevenueGrewPer = GetNumberValue(model, "f184");
                stock.ProfitGrewPer = GetNumberValue(model, "f185");
                return new StockInfo()
                {
                    Stock = stock,
                    DayPrice = stockPrice
                };
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static StockPriceEntity[] GetStockHisPrice(string code, string startDate = "00000000", string endDate = "20500000")
        {
            try
            {
                var secid = GetStockSecid(code);
                var api = $"http://push2his.eastmoney.com/api/qt/stock/kline/get?fields1=f1,f2,f3,f4,f5,f6&fields2=f51,f52,f53,f54,f55,f56,f57,f58,f59,f60,f61&klt=101&fqt=1&secid={secid}&beg={startDate}&end={endDate}";
                var retStr = api.PostJsonToUrl(string.Empty, requestFilter =>
                {
                    requestFilter.Timeout = 5 * 60 * 1000;
                });
                var apiModel = ServiceStack.Text.JsonSerializer.DeserializeFromString<EastMoneyHisPriceAPIModel>(retStr);
                if (apiModel == null || apiModel.data == null || apiModel.data.klines == null) return null;

                var stockPrices = new List<StockPriceEntity>();
                foreach (var line in apiModel.data.klines)
                {
                    var arr = ObjectUtil.GetSplitArray(line, ",");
                    if (arr.Length != 11) continue;

                    var stockPrice = new StockPriceEntity();
                    stockPrice.StockCode = code;
                    stockPrice.DealDate = arr[0];
                    stockPrice.DealTime = "";
                    stockPrice.DateType = 0;
                    stockPrice.TodayStartPrice = ObjectUtil.ToValue<decimal>(arr[1], 0);
                    stockPrice.TodayEndPrice = ObjectUtil.ToValue<decimal>(arr[2], 0);
                    stockPrice.TodayMaxPrice = ObjectUtil.ToValue<decimal>(arr[3], 0);
                    stockPrice.TodayMinPrice = ObjectUtil.ToValue<decimal>(arr[4], 0);
                    stockPrice.DealQty = Math.Round(ObjectUtil.ToValue<decimal>(arr[5], 0) / 10000, 2);
                    stockPrice.DealAmount = Math.Round(ObjectUtil.ToValue<decimal>(arr[6], 0)/100000000,3);
                    stockPrice.GoPer = ObjectUtil.ToValue<decimal>(arr[7], 0);
                    stockPrice.UDPer = ObjectUtil.ToValue<decimal>(arr[8], 0);
                    stockPrice.UDValue = ObjectUtil.ToValue<decimal>(arr[9], 0);
                    stockPrice.GoHandPer = ObjectUtil.ToValue<decimal>(arr[10], 0);
                    stockPrice.Price = stockPrice.TodayEndPrice;

                    if (stockPrice.Price == 0) continue;
                    stockPrices.Add(stockPrice);
                }
                return stockPrices.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 机构预测信息

        public static StockForecastInfo GetStockForecast(string code)
        {
            try
            {
                var api = $"http://f10.eastmoney.com/ProfitForecast/ProfitForecastAjax?code={code}";
                var retStr = api.PostJsonToUrl(string.Empty, requestFilter =>
                {
                    requestFilter.Timeout = 5 * 60 * 1000;
                });
                var apiModel = ServiceStack.Text.JsonSerializer.DeserializeFromString<StockForecastInfo>(retStr);
                if (apiModel == null) return null;

                return apiModel;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region 财务主要指标

        public static MainTargetEntity[] GetMainTargets(string stockCode, int rtype)
        {
            try
            {
                var josnParam = ServiceStack.Text.JsonSerializer.SerializeToString(new
                {
                    type = rtype,
                    code = stockCode,
                });
                var retStr = "http://f10.eastmoney.com/NewFinanceAnalysis/MainTargetAjax".PostJsonToUrl(josnParam, requestFilter =>
                {
                    requestFilter.Timeout = 5 * 60 * 1000;
                });
                retStr = retStr.Replace("亿", "");
                var result = ServiceStack.Text.JsonSerializer.DeserializeFromString<MainTargetEntity[]>(retStr);
                if (result != null)
                {
                    result.ToList().ForEach(x =>
                    {
                        x.StockCode = stockCode;
                        x.Rtype = rtype;
                    });
                }
                return result;
            }
            catch(Exception ex)
            {
                return new MainTargetEntity[] { };
            }
        }

        public static DataTable ConvertMainTargetData(MainTargetEntity[] data)
        {
            var dt = new DataTable();
            dt.Columns.Add("指标");
            var dates = data.Where(c=>!string.IsNullOrEmpty(c.Date)).GroupBy(c => c.Date).Select(c => c.Key).OrderByDescending(c => c).ToArray();
            foreach (var date in dates)
            {
                dt.Columns.Add(date);
            }

            var preps = typeof(MainTargetEntity).GetProperties();
            foreach (var prep in preps)
            {
                if (new string[] { "ID", "LastDate", "StockCode", "Rtype", "RtypeText", "Date" }.Contains(prep.Name)) continue;
                BuildTargetRow(dt, prep.Name, dates, data);
            }
            return dt;
        }

        static void BuildTargetRow(DataTable dt, string field, string[] dates, MainTargetEntity[] data)
        {
            //基本每股收益
            var dr = dt.NewRow();
            dr["指标"] = ObjectUtil.GetPropertyDesc(typeof(MainTargetEntity), field);//"基本每股收益(元)";

            foreach (var date in dates)
            {
                var item = data.FirstOrDefault(c => c.Date == date);

                var value = ObjectUtil.GetPropertyValue<decimal>(item, field);
                dr[date] = value;
            }
            dt.Rows.Add(dr);
        }

        #endregion

        #region 现金流量表
        public static CashTargetEntity[] GetCashTargets(string stockCode, int rtype, int type, string endDate = "")
        {
            try
            {
                var josnParam = ServiceStack.Text.JsonSerializer.SerializeToString(new
                {
                    reportDateType = rtype,
                    reportType = type,
                    code = stockCode,
                    companyType = 4,
                    endDate
                });
                var retStr = "http://f10.eastmoney.com/NewFinanceAnalysis/xjllbAjax".PostJsonToUrl(josnParam, requestFilter =>
                {
                    requestFilter.Timeout = 5 * 60 * 1000;
                });
                retStr = retStr.Substring(1);
                retStr = retStr.Substring(0, retStr.Length - 1);
                retStr = retStr.Replace("\\\"", "\"");
                var result = ServiceStack.Text.JsonSerializer.DeserializeFromString<CashTargetEntity[]>(retStr);
                if (result != null)
                {
                    result.ToList().ForEach(x =>
                    {
                        x.SECURITYCODE = stockCode;
                        x.REPORTDATETYPE = rtype;
                        x.REPORTDATE = Convert.ToDateTime(x.REPORTDATE).ToString("yyyy-MM-dd");
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                return new CashTargetEntity[] { };
            }
        }

        public static DataTable ConvertCashTargetData(CashTargetEntity[] data)
        {
            var dt = new DataTable();
            dt.Columns.Add("指标");
            var dates = data.Where(c => !string.IsNullOrEmpty(c.REPORTDATE)).GroupBy(c => c.REPORTDATE).Select(c => c.Key).OrderByDescending(c => c).ToArray();
            foreach (var date in dates)
            {
                dt.Columns.Add(date);
            }

            var preps = typeof(CashTargetEntity).GetProperties();
            foreach(var prep in preps)
            {
                if (new string[] { "ID", "LastDate", "SECURITYCODE", "REPORTTYPE", "REPORTDATETYPE", "TYPE", "CURRENCY", "REPORTDATE" }.Contains(prep.Name)) continue;
                BuildTargetRow(dt, prep.Name, dates, data);
            }
            return dt;
        }

        static void BuildTargetRow(DataTable dt, string field, string[] dates, CashTargetEntity[] data)
        {
            var dr = dt.NewRow();
            dr["指标"] = ObjectUtil.GetPropertyDesc(typeof(CashTargetEntity), field);
            foreach (var date in dates)
            {
                var item = data.FirstOrDefault(c => c.REPORTDATE == date);

                var value = ObjectUtil.GetPropertyValue<decimal>(item, field);
                dr[date] = ObjectUtil.FormatMoney(value);
            }
            dt.Rows.Add(dr);
        }

        #endregion

        #region 资产负债表

        public static BalanceTargetEntity[] GetBalanceTargets(string stockCode, int rtype, int type, string endDate = "")
        {
            try
            {
                var josnParam = ServiceStack.Text.JsonSerializer.SerializeToString(new
                {
                    reportDateType = rtype,
                    reportType = type,
                    code = stockCode,
                    companyType = 4,
                    endDate
                });
                var retStr = "http://f10.eastmoney.com/NewFinanceAnalysis/zcfzbAjax".PostJsonToUrl(josnParam, requestFilter =>
                {
                    requestFilter.Timeout = 5 * 60 * 1000;
                });
                retStr = retStr.Substring(1);
                retStr = retStr.Substring(0, retStr.Length - 1);
                retStr = retStr.Replace("\\\"", "\"");
                var result = ServiceStack.Text.JsonSerializer.DeserializeFromString<BalanceTargetEntity[]>(retStr);
                if (result != null)
                {
                    result.ToList().ForEach(x =>
                    {
                        x.SECURITYCODE = stockCode;
                        x.REPORTDATETYPE = rtype;
                        x.REPORTDATE = Convert.ToDateTime(x.REPORTDATE).ToString("yyyy-MM-dd");
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                return new BalanceTargetEntity[] { };
            }
        }

        public static DataTable ConvertBalanceTargetData(BalanceTargetEntity[] data)
        {
            var dt = new DataTable();
            dt.Columns.Add("指标");
            var dates = data.Where(c => !string.IsNullOrEmpty(c.REPORTDATE)).GroupBy(c => c.REPORTDATE).Select(c => c.Key).OrderByDescending(c => c).ToArray();
            foreach (var date in dates)
            {
                dt.Columns.Add(date);
            }

            var preps = typeof(BalanceTargetEntity).GetProperties();
            foreach (var prep in preps)
            {
                if (new string[] { "ID", "LastDate", "SECURITYCODE", "REPORTTYPE", "REPORTDATETYPE", "TYPE", "CURRENCY", "REPORTDATE" }.Contains(prep.Name)) continue;
                BuildTargetRow(dt, prep.Name, dates, data);
            }
            return dt;
        }

        static void BuildTargetRow(DataTable dt, string field, string[] dates, BalanceTargetEntity[] data)
        {
            var dr = dt.NewRow();
            dr["指标"] = ObjectUtil.GetPropertyDesc(typeof(BalanceTargetEntity), field);
            foreach (var date in dates)
            {
                var item = data.FirstOrDefault(c => c.REPORTDATE == date);

                var value = ObjectUtil.GetPropertyValue<decimal>(item, field);
                dr[date] = ObjectUtil.FormatMoney(value);
            }
            dt.Rows.Add(dr);
        }

        #endregion

        #region 利润表
        public static ProfitTargetEntity[] GetProfitTargets(string stockCode, int rtype, int type, string endDate = "")
        {
            try
            {
                var josnParam = ServiceStack.Text.JsonSerializer.SerializeToString(new
                {
                    reportDateType = rtype,
                    reportType = type,
                    code = stockCode,
                    companyType = 4,
                    endDate
                });
                var retStr = "http://f10.eastmoney.com/NewFinanceAnalysis/lrbAjax".PostJsonToUrl(josnParam, requestFilter =>
                {
                    requestFilter.Timeout = 5 * 60 * 1000;
                });
                retStr = retStr.Substring(1);
                retStr = retStr.Substring(0, retStr.Length - 1);
                retStr = retStr.Replace("\\\"", "\"");
                var result = ServiceStack.Text.JsonSerializer.DeserializeFromString<ProfitTargetEntity[]>(retStr);
                if (result != null)
                {
                    result.ToList().ForEach(x =>
                    {
                        x.SECURITYCODE = stockCode;
                        x.REPORTDATETYPE = rtype;
                        x.REPORTDATE = Convert.ToDateTime(x.REPORTDATE).ToString("yyyy-MM-dd");
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ProfitTargetEntity[] { };
            }
        }

        public static DataTable ConvertProfitTargetData(ProfitTargetEntity[] data)
        {
            var dt = new DataTable();
            dt.Columns.Add("指标");
            var dates = data.Where(c => !string.IsNullOrEmpty(c.REPORTDATE)).GroupBy(c => c.REPORTDATE).Select(c => c.Key).OrderByDescending(c => c).ToArray();
            foreach (var date in dates)
            {
                dt.Columns.Add(date);
            }

            var preps = typeof(ProfitTargetEntity).GetProperties();
            foreach (var prep in preps)
            {
                if (new string[] { "ID", "LastDate", "SECURITYCODE", "REPORTTYPE", "REPORTDATETYPE", "TYPE", "CURRENCY", "REPORTDATE" }.Contains(prep.Name)) continue;
                BuildTargetRow(dt, prep.Name, dates, data);
            }
            return dt;
        }

        static void BuildTargetRow(DataTable dt, string field, string[] dates, ProfitTargetEntity[] data)
        {
            var dr = dt.NewRow();
            dr["指标"] = ObjectUtil.GetPropertyDesc(typeof(ProfitTargetEntity), field);
            foreach (var date in dates)
            {
                var item = data.FirstOrDefault(c => c.REPORTDATE == date);

                var value = ObjectUtil.GetPropertyValue<decimal>(item, field);
                dr[date] = ObjectUtil.FormatMoney(value);
            }
            dt.Rows.Add(dr);
        }

        #endregion

        #region 研报信息

        public static ReportEntity[] GetStockReports(string stockCode)
        {
            try
            {
                var retStr = $"http://data.eastmoney.com/report/{stockCode.Substring(2, 6)}.html".GetStringFromUrl();
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(retStr);

                var jsonNode = doc.DocumentNode.ChildNodes.FirstOrDefault(c => c.Name == "script" && c.InnerText.Contains("var initdata ="));
                if(jsonNode != null)
                {
                    var initData = jsonNode.InnerHtml.Substring(jsonNode.InnerHtml.IndexOf("var initdata ="));
                    var jsonData = initData.Replace("var initdata = ", "");
                    jsonData = jsonData.Substring(0, jsonData.Length - 1);

                    var result = ServiceStack.Text.JsonSerializer.DeserializeFromString<EastMoneyReportModel>(jsonData);
                    if (result == null || result.data == null) return new ReportEntity[] { };

                    var gatherDate = DateTime.Now.Date.AddMonths(-3);
                    return result.data.Where(c => c.publishDate > gatherDate).Select(c => new ReportEntity()
                    {
                        ReportType = 0,
                        StockCode = stockCode,
                        IndustryName = c.industryName,
                        Title = c.title,
                        PublishDate = c.publishDate.ToString("yyyy-MM-dd"),
                        PdfCode = c.infoCode,
                        OrgNam = c.orgSName,
                        ThisYearPE = c.predictThisYearPe,
                        ThisYearEPS = c.predictThisYearEps,
                        NextYearPE = c.predictNextYearPe,
                        NextYearEPS = c.predictThisYearEps,
                        NextTwoYearPE = c.predictNextYearPe,
                        NextTwoYearEPS = c.predictNextYearEps,
                    }).ToArray();
                }
                return new ReportEntity[] { };
            }
            catch (Exception ex)
            {
                return new ReportEntity[] { };
            }
        }

        public static ReportEntity[] GetStockFinanceReports(string stockCode)
        {
            try
            {
                var apiModel = GetFinanceReports(stockCode, 1);
                if (apiModel.data == null || apiModel.data.list.Length == 0) return null;

                var reports = new List<EastMoneyFinanceReport>();
                reports.AddRange(apiModel.data.list);

                var totalPage = apiModel.data.total_hits * 1.0m / apiModel.data.page_size * 1.0m;
                if (totalPage > 1)
                {
                    for (var i = 2; i <= Math.Ceiling(totalPage); i++)
                    {
                        var model = GetFinanceReports(stockCode, i);
                        if (model == null) continue;

                        reports.AddRange(model.data.list);
                    }
                }
                return reports.Select(c => new ReportEntity()
                {
                    ReportType = 1,
                    StockCode = stockCode,
                    Title = c.title,
                    PublishDate = c.notice_date.Substring(0, 10),
                    PdfCode = c.art_code
                }).ToArray();
            }
            catch (Exception ex)
            {
                return new ReportEntity[] { };
            }
        }

        static EastMoneyFinanceReportModel GetFinanceReports(string stockCode, int pageNo)
        {
            try
            {
                var api = $"http://np-anotice-stock.eastmoney.com/api/security/ann?sr=-1&page_size=50&page_index={pageNo}&ann_type=A&stock_list={stockCode.Substring(2, 6)}&f_node=1&s_node=0";
                var retStr = api.GetJsonFromUrl(requestFilter =>
                {
                    requestFilter.Timeout = 5 * 60 * 1000;
                });
                var apiModel = ServiceStack.Text.JsonSerializer.DeserializeFromString<EastMoneyFinanceReportModel>(retStr);
                if (apiModel.data == null || apiModel.data.list.Length == 0) return null;

                return apiModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ReportEntity[] GetIndustryReports(DateTime startDate, DateTime endDate)
        {
            var apiModel = GetIndustryReports(startDate, endDate, 1);
            if (apiModel == null) return new ReportEntity[] { };

            var reports = new List<ReportModel>();
            reports.AddRange(apiModel.data);

            if(apiModel.TotalPage > 1)
            {
                for (var i = 2; i <= apiModel.TotalPage; i++)
                {
                    var model = GetIndustryReports(startDate, endDate, i);
                    if (model == null) continue;

                    reports.AddRange(model.data);
                }
            }
            return reports.Select(c => new ReportEntity()
            {
                ReportType = 9,
                StockCode = string.Empty,
                IndustryName = c.industryName,
                Title = c.title,
                PublishDate = c.publishDate.ToString("yyyy-MM-dd"),
                PdfCode = c.infoCode,
                OrgNam = c.orgSName,
                ThisYearPE = c.predictThisYearPe,
                ThisYearEPS = c.predictThisYearEps,
                NextYearPE = c.predictNextYearPe,
                NextYearEPS = c.predictThisYearEps,
                NextTwoYearPE = c.predictNextYearPe,
                NextTwoYearEPS = c.predictNextYearEps,
            }).ToArray();
        }

        static EastMoneyReportModel GetIndustryReports(DateTime startDate, DateTime endDate, int pageNo)
        {
            try
            {
                var api = $"http://reportapi.eastmoney.com/report/list?pageSize=100&beginTime={startDate.ToString("yyyy-MM-dd")}&endTime={endDate.ToString("yyyy-MM-dd")}&pageNo={pageNo}&qType=1";
                var retStr = api.GetJsonFromUrl(requestFilter =>
                {
                    requestFilter.Timeout = 5 * 60 * 1000;
                });
                var apiModel = ServiceStack.Text.JsonSerializer.DeserializeFromString<EastMoneyReportModel>(retStr);
                if (apiModel.hits == 0 || apiModel.data == null || apiModel.data.Length == 0) return null;

                return apiModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #endregion

        #region 基金持仓

        public static FundStockEntity[] GetFundStock(string stockCode)
        {
            try
            {
                //https://fundwebapi.eastmoney.com/FundMEApi/FundPositionList?deviceid=123&version=4.3.0&product=Eastmoney&plat=Web&FCODE=510050
                var api = $"https://fundf10.eastmoney.com/FundArchivesDatas.aspx?type=jjcc&code={stockCode.Substring(2,6)}&topline=10&year=&month=9";
                var retStr = api.PostJsonToUrl(string.Empty, requestFilter =>
                {
                    requestFilter.Timeout = 5 * 60 * 1000;
                });
                retStr = retStr.Replace("var apidata={ content:\"", "");
                if(retStr.Contains("</div>")) retStr = retStr.Substring(0, retStr.LastIndexOf("</div>") + 6);

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(retStr);

                var fundStocks = new List<FundStockEntity>();

                var gahterQuarter = ObjectUtil.GetGatherQuarterNum(DateTime.Now);
                var gahterQuarterStr = $"{DateTime.Now.Year}年{gahterQuarter}季度";
                var reportDate = ObjectUtil.GetGatherQuarterStr(DateTime.Now, gahterQuarter);
                var divContent = doc.DocumentNode.ChildNodes.FirstOrDefault(c => c.InnerText.Contains(gahterQuarterStr));
                if (divContent != null)
                {
                    var trs = divContent.SelectNodes("div/table/tbody/tr");
                    foreach (var tr in trs)
                    {
                        var tds = tr.SelectNodes("td");
                        if (tds.Count < 9) continue;

                        var seq = ObjectUtil.ToValue<int>(tds[0].InnerText, 0);
                        if (seq == 0) continue;

                        var code= tds[1].InnerText;
                        var holdCode = ObjectUtil.GetStockMarket(code) + code;
                        var holdName = tds[2].InnerText;
                        var holdPer = ObjectUtil.ToValue<decimal>(tds[6].InnerText.Replace("%", ""), 0);
                        var holdQty = ObjectUtil.ToValue<decimal>(tds[7].InnerText, 0);
                        var holdAmount = ObjectUtil.ToValue<decimal>(tds[8].InnerText, 0);

                        var fund = new FundStockEntity()
                        {
                            StockCode = stockCode,
                            ReportDate = reportDate,
                            Seq = seq,
                            HoldStockCode = holdCode,
                            HoldStockName = holdName,
                            PositionPer= holdPer,
                            HoldQty = holdQty,
                            HoldAmount= holdAmount,
                        };
                        fundStocks.Add(fund);
                    }
                }
                return fundStocks.ToArray();
            }
            catch (Exception ex)
            {
                return new FundStockEntity[] { };
            }
        }

        #endregion

        #region 资金流向

        public static FundFlowEntity GetStockFundFlows(string stockCode, string dealDate)
        {
            try
            {
                var secid = GetStockSecid(stockCode);
                var api = $"http://push2.eastmoney.com/api/qt/ulist.np/get?fltt=2&secids={secid}&fields=f62,f184,f66,f69,f72,f75,f78,f81,f84,f87,f64,f65,f70,f71,f76,f77,f82,f83,f164,f166,f168,f170,f172,f252,f253,f254,f255,f256,f124,f6,f278,f279,f280,f281,f282";
                var retStr = api.PostJsonToUrl(string.Empty, requestFilter =>
                {
                    requestFilter.Timeout = 5 * 60 * 1000;
                });
                var apiModel = ServiceStack.Text.JsonSerializer.DeserializeFromString<EastAmountAPIModel>(retStr);
                if (apiModel == null || apiModel.data == null || apiModel.data.diff == null || apiModel.data.diff.Length == 0) return null;
                var model = apiModel.data.diff.FirstOrDefault();

                var fundFlow = new FundFlowEntity()
                {
                    StockCode = stockCode,
                    DealDate = dealDate
                };
                fundFlow.Amount = Math.Round(model.f62 / 100000000, 3);
                //fundFlow.RetailAmount = Math.Round((model.f78 + model.f84) / 100000000, 3);
                //fundFlow.Amount = Math.Round((model.f66 + model.f72 + model.f78 + model.f84) / 100000000, 3);
                return fundFlow;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static FundFlowEntity[] GetIndustryFundFlows(string dealDate)
        {
            try
            {
                var api = $"http://push2.eastmoney.com/api/qt/clist/get?pn=1&pz=500&po=1&np=1&fields=f12,f13,f14,f62,f66,f72,f78,f84&fid=f62&fs=m:90+t:2";
                var retStr = api.PostJsonToUrl(string.Empty, requestFilter =>
                {
                    requestFilter.Timeout = 5 * 60 * 1000;
                });
                var apiModel = ServiceStack.Text.JsonSerializer.DeserializeFromString<EastAmountAPIModel>(retStr);
                if (apiModel == null || apiModel.data == null || apiModel.data.diff == null || apiModel.data.diff.Length == 0) return null;

                var fundFlows = new List<FundFlowEntity>();
                foreach (var model in apiModel.data.diff)
                {
                    var fundFlow = new FundFlowEntity()
                    {
                        StockCode = string.Empty,
                        DealDate = dealDate,
                        IndustryName = model.f14
                    };
                    fundFlow.Amount = Math.Round(model.f62 / 100000000, 3);
                    //fundFlow.RetailAmount = Math.Round((model.f78 + model.f84) / 100000000, 3);
                    //fundFlow.Amount = Math.Round((model.f66 + model.f72 + model.f78 + model.f84) / 100000000, 3);
                    fundFlows.Add(fundFlow);
                }
                return fundFlows.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region 辅助方法

        static string GetStockSecid(string code)
        {
            var secid = code.Substring(0, 2) == "SZ" ? "0" : "1";

            return $"{secid}.{code.Substring(2, 6)}";
        }

        static string GetStringValue(Dictionary<string, object> model, string field)
        {
            if (!model.ContainsKey(field)) throw new Exception($"API接口未返回键名:{field}");

            return ObjectUtil.ToValue<string>(model[field], "");
        }

        static decimal GetNumberValue(Dictionary<string, object> model, string field)
        {
            if (!model.ContainsKey(field)) throw new Exception($"API接口未返回键名:{field}");

            var val = ObjectUtil.ToValue<decimal>(model[field], 0);
            return Math.Round(val, 3);
        }

        static decimal GetTenThousandValue(Dictionary<string, object> model, string field)
        {
            if (!model.ContainsKey(field)) throw new Exception($"API接口未返回键名:{field}");

            var val = ObjectUtil.ToValue<decimal>(model[field], 0);
            return Math.Round(val / 10000, 3);
        }

        static decimal GetMillionValue(Dictionary<string, object> model, string field)
        {
            if (!model.ContainsKey(field)) throw new Exception($"API接口未返回键名:{field}");

            var val = ObjectUtil.ToValue<decimal>(model[field], 0);
            return Math.Round(val / 100000000, 3);
        }

        static long GetLongValue(Dictionary<string, object> model, string field)
        {
            if (!model.ContainsKey(field)) throw new Exception($"API接口未返回键名:{field}");

            return ObjectUtil.ToValue<long>(model[field], 0);
        }
        #endregion
    }

    public class EastMoneyAPIModel
    {
        public Dictionary<string, object> data { get; set; }
    }

    public class EastMoneyReportModel
    {
        public int TotalPage { get; set; }
        public int hits { get; set; }
        public int pageNo { get; set; }
        public int size { get; set; }
        public ReportModel[] data { get; set; }
    }

    public class EastMoneyFinanceReportModel
    {
        public EastMoneyFinanceReportData data { get; set; }
    }

    public class EastMoneyFinanceReportData
    {
        public EastMoneyFinanceReport[] list { get; set; }
        public int page_index { get; set; }
        public int page_size { get; set; }
        public int total_hits { get; set; }
    }

    public class EastMoneyFinanceReport
    { 
        public string art_code { get; set; }
        public string notice_date { get; set; }
        public string title { get; set; }
    }

    public class EastMoneyHisPriceAPIModel
    {
        public EastMoneyHisPriceModel data { get; set; }
    }
    public class EastMoneyHisPriceModel
    {
        public string code { get; set; }
        public string[] klines { get; set; }
    }
    public class EastMoneyFundStockAPIModel
    {
        public string content { get; set; }

        public string curyear { get; set; }
    }

    public class EastAmountAPIModel
    { 
        public EastAmountModel data { get; set; }
    }

    public class EastAmountModel
    {
        public int total { get; set; }
        public EastAmountDetailModel[] diff { get; set; }
    }

    public class EastAmountDetailModel
    {
        public string f14 { get; set; }
        public decimal f62 { get; set; }
        public decimal f66 { get; set; }
        public decimal f72 { get; set; }
        public decimal f78 { get; set; }
        public decimal f84 { get; set; }
    }

    public class ReportModel
    { 
        public string title { get; set; }
        public DateTime publishDate { get; set; }
        public decimal predictThisYearPe { get; set; }
        public decimal predictThisYearEps { get; set; }
        public decimal predictNextYearPe { get; set; }
        public decimal predictNextYearEps { get; set; }
        public decimal predictNextTwoYearPe { get; set; }
        public decimal predictNextTwoYearEps { get; set; }
        public string orgSName { get; set; }
        public string infoCode { get; set; }
        public string indvInduName { get; set; }
        public string industryName { get; set; }
    }

    public class StockForecastInfo
    { 
        public StockForecastModel[] mgsy { get; set; }

        public StockForecastModel[] jzcsyl { get; set; }

        public StockForecastModel[] gsjlr { get; set; }

        public StockForecastModel[] yysr { get; set; }

        public StockForecastJgycModel jgyc { get; set; }
    }

    public class StockForecastModel
    {
        public string Year { get; set; }
        public decimal Value { get; set; }
        public string Ratio { get; set; }
    }

    public class StockForecastJgycModel
    {
        public int baseYear { get; set; }
       
        public StockForecastJgycMxModel[] data { get; set; }
    }

    public class StockForecastJgycMxModel
    {
        public string Jgmc { get; set; }
        public decimal Sy { get; set; }
        public decimal Sy1 { get; set; }
        public decimal Sy2 { get; set; }
        public decimal Sy3 { get; set; }

        public decimal Syl { get; set; }
        public decimal Syl1 { get; set; }
        public decimal Syl2 { get; set; }
        public decimal Syl3 { get; set; }
    }

    public class StockInfo
    {
        public StockEntity Stock { get; set; }

        public StockPriceEntity DayPrice { get; set; }
    }
}
