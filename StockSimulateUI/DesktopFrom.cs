using StockPriceTools.UI;
using StockSimulateCore.Config;
using StockSimulateCore.Model;
using StockSimulateCore.Service;
using StockSimulateCore.Utils;
using StockSimulateUI.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace StockPriceTools
{
    public partial class DesktopFrom : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        private RunningConfig RC = RunningConfig.Instance;
        public DesktopFrom()
        {
            InitializeComponent();
        }

        #region 页面加载事件
        private void DesktopFrom_Load(object sender, EventArgs e)
        {
            this.LoadConfigData();

            this.LoadStockData();

            this.GatherData();

            this.RemindData();
        }

        void LoadConfigData()
        {
            Task.Factory.StartNew(() =>
            {   
                //同步数据库
                Repository.InitSQLiteDB();

                var configs = Repository.QueryAll<GlobalConfigEntity>();
                foreach(var config in configs)
                {
                    var prepInfo = ObjectUtil.GetPropertyInfo(RC, config.Name);
                    if (prepInfo == null) continue;

                    var value = ObjectUtil.ToValue(config.Value, prepInfo.PropertyType);
                    if (value != null)
                    {
                        ObjectUtil.SetPropertyValue(RC, config.Name, value);
                    }
                }
            });
        }

        void LoadStockData()
        {
            //页面加载后加载一次数据，其余通过采集价格数据后刷新列表
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2 * 1000);

                Action act = delegate ()
                {
                    this.LoadStockList();
                    this.LoadAccountStockList();
                };
                this.Invoke(act);
            });
        }

        void GatherData()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);

                while (true)
                {
                    if (ObjectUtil.EffectStockDealTime())
                    {
                        var stocks = Repository.QueryAll<StockEntity>();
                        foreach (var stock in stocks)
                        {
                            var stockInfo = EastMoneyUtil.GetStockPrice(stock.Code);
                            if (stockInfo == null) return;
                            if (stockInfo.DayPrice.Price == 0) return;

                            stockInfo.Stock.ID = stock.ID;
                            Repository.Update<StockEntity>(stockInfo.Stock);

                            var dealDate = DateTime.Now.ToString("yyyy-MM-dd");
                            var price = Repository.QueryFirst<StockPriceEntity>($"StockCode='{stock.Code}' and DealDate='{dealDate}' and DateType=0");
                            if (price == null)
                            {
                                stockInfo.DayPrice.DealTime = "";
                                Repository.Insert<StockPriceEntity>(stockInfo.DayPrice);
                            }
                            else
                            {
                                stockInfo.DayPrice.ID = price.ID;
                                stockInfo.DayPrice.DealTime = "";
                                Repository.Update<StockPriceEntity>(stockInfo.DayPrice);
                            }
                            var dealTime = DateTime.Now.ToString("HH:mm");
                            if (dealTime.CompareTo("15:00") >= 0) dealTime = "15:00";

                            var price2 = Repository.QueryFirst<StockPriceEntity>($"StockCode='{stock.Code}' and DealDate='{dealDate}' and DealTime='{dealTime}' and DateType=1");
                            if (price2 == null)
                            {
                                stockInfo.DayPrice.DateType = 1;//分钟
                                stockInfo.DayPrice.DealTime = dealTime;
                                Repository.Insert<StockPriceEntity>(stockInfo.DayPrice);
                            }
                            else
                            {
                                stockInfo.DayPrice.ID = price2.ID;
                                stockInfo.DayPrice.DateType = 1;//分钟
                                stockInfo.DayPrice.DealTime = dealTime;
                                Repository.Update<StockPriceEntity>(stockInfo.DayPrice);
                            }
                            this.ActionLog($"已采集[{stock.Name}]今日股价数据...[{stockInfo.DayPrice.Price.ToString("####.00").PadLeft(4, ' ')}] [{stockInfo.DayPrice.UDPer}%]");
                        }
                        this.ActionLog($">------------------------------------------------>");
                        Action act = delegate ()
                        {
                            this.LoadStockList();
                            this.LoadAccountStockList();
                        };
                        this.Invoke(act);
                    }
                    Thread.Sleep(RC.GatherStockPriceInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(15 * 1000);
                while (true)
                {
                    var stocks = Repository.QueryAll<StockEntity>();
                    foreach (var stock in stocks)
                    {
                        var mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 0);
                        if (mainTargetInfos.Length > 0)
                        {
                            var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                            var mts = Repository.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=0 and Date in ('{string.Join("','", dates)}')");
                            var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                            var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                            if (newMts.Length > 0)
                            {
                                Repository.Insert<MainTargetEntity>(newMts);
                            }
                        }
                        mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 1);
                        if (mainTargetInfos.Length > 0)
                        {
                            var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                            var mts = Repository.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=1 and Date in ('{string.Join("','", dates)}')");
                            var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                            var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                            if (newMts.Length > 0)
                            {
                                Repository.Insert<MainTargetEntity>(newMts);
                            }
                        }
                        mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 2);
                        if (mainTargetInfos.Length > 0)
                        {
                            var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                            var mts = Repository.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=2 and Date in ('{string.Join("','", dates)}')");
                            var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                            var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                            if (newMts.Length > 0)
                            {
                                Repository.Insert<MainTargetEntity>(newMts);
                            }
                        }
                        this.ActionLog($"已采集[{stock.Name}]主要指标数据...");
                    }

                    this.ActionLog($">------------------------------------------------>");

                    Thread.Sleep(RC.GatherStockMainTargetInterval * 1000);
                }
            }, CancellationTokenSource.Token);
        }

        void RemindData()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (ObjectUtil.EffectStockDealTime())
                    {
                        var reminds = Repository.QueryAll<RemindEntity>($"Handled='False'");
                        var stockCodes = reminds.Select(c => c.StockCode).Distinct().ToArray();
                        var stocks = Repository.QueryAll<StockEntity>($"Code in ('{string.Join("','", stockCodes)}') and Price>0");

                        foreach (var stock in stocks)
                        {
                            var remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 0 && Math.Abs(stock.UDPer) > c.Target && !c.Handled && (!c.PlanRemind.HasValue || c.PlanRemind < DateTime.Now));
                            if (remind != null)
                            {
                                remind.Handled = true;
                                remind.LastRemind = DateTime.Now;
                                Repository.Update<RemindEntity>(remind);

                                var nextRemind = new RemindEntity()
                                {
                                    RType = 0,
                                    StockCode = remind.StockCode,
                                    Target = remind.Target,
                                    Email = remind.Email,
                                    QQ = remind.QQ,
                                    PlanRemind = DateTime.Now.Date.AddDays(1)
                                };
                                Repository.Insert<RemindEntity>(nextRemind);

                                var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已{(stock.UDPer > 0 ? "上涨" : "下跌")}超过幅度[{remind.Target}%],请注意!";

                                MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                                this.ShowMessage(message);
                                this.ActionLog(message);
                            }

                            remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 1 && c.Target <= stock.Price && !c.Handled);
                            if (remind != null)
                            {
                                var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已上涨高于股价[{remind.Target}],请注意!";

                                MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                                this.ShowMessage(message);
                                this.ActionLog(message);

                                remind.Handled = true;
                                remind.LastRemind = DateTime.Now;
                                Repository.Update<RemindEntity>(remind);
                            }

                            remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 2 && c.Target >= stock.Price && !c.Handled);
                            if (remind != null)
                            {
                                var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已下跌低于股价[{remind.Target}],请注意!";

                                MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                                this.ShowMessage(message);
                                this.ActionLog(message);

                                remind.Handled = true;
                                remind.LastRemind = DateTime.Now;
                                Repository.Update<RemindEntity>(remind);
                            }

                            remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 8 && c.MaxPrice >= stock.Price && !c.Handled && (!c.LastRemind.HasValue || c.LastRemind < DateTime.Now.Date));
                            if (remind != null)
                            {
                                var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已达成买卖点[{remind.StrategyTarget} ({remind.MinPrice}-{remind.MaxPrice})],请注意!";

                                MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                                this.ShowMessage(message);
                                this.ActionLog(message);

                                remind.LastRemind = DateTime.Now;
                                Repository.Update<RemindEntity>(remind);
                            }

                            remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 9 && c.MinPrice <= stock.Price && !c.Handled && (!c.LastRemind.HasValue || c.LastRemind < DateTime.Now.Date));
                            if (remind != null)
                            {
                                var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已达成买卖点[{remind.StrategyTarget} ({remind.MinPrice}-{remind.MaxPrice})],请注意!";

                                MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                                this.ShowMessage(message);
                                this.ActionLog(message);

                                remind.LastRemind = DateTime.Now;
                                Repository.Update<RemindEntity>(remind);
                            }
                        }
                    }
                    Thread.Sleep(RC.RemindStockStrategyInterval * 1000);
                }
            }, CancellationTokenSource.Token);
        }
   

        #endregion

        #region 加载GridList数据

        void LoadStockList()
        {
            var stocks = Repository.QueryAll<StockEntity>();
            var dt = ObjectUtil.ConvertTable(stocks);
            this.gridStockList.DataSource = null;
            this.gridStockList.DataSource = dt.DefaultView;
            for(var i=0; i<this.gridStockList.ColumnCount; i++)
            {
                var length = this.gridStockList.Columns[i].Name.Length;
                this.gridStockList.Columns[i].Width = length < 6 ? 80 : length < 8 ? 120 : 140;
            }
            for(var i=0; i<this.gridStockList.Rows.Count; i++)
            {
                var row = this.gridStockList.Rows[i];
                var value = ObjectUtil.ToValue<decimal>(row.Cells["浮动(%)"].Value, 0);
                if(value > 0)
                {
                    this.gridStockList.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    this.gridStockList.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                }
            }
        }

        void LoadAccountStockList()
        {
            var accountStocks = Repository.QueryAll<AccountStockEntity>();
            var dt = ObjectUtil.ConvertTable(accountStocks);
            this.gridAccountStockList.DataSource = null;
            this.gridAccountStockList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridAccountStockList.ColumnCount; i++)
            {
                var length = this.gridAccountStockList.Columns[i].Name.Length;
                this.gridAccountStockList.Columns[i].Width = length < 6 ? 80 : length < 8 ? 120 : 140;
            }
        }

        /// <summary>
        /// 0:日,1:周,2:月,3:1分钟,5:5分钟,15:15分钟
        /// </summary>
        /// <param name="stockCode">0:蜡烛图,1:折线图</param>
        /// <param name="timeType"></param>
        void LoadPriceChart(string stockCode, int timeType = 0)
        {
            var series = this.chartPrice.Series.FirstOrDefault();
            if (series == null)
            {
                series = this.chartPrice.Series.Add("");
                series.ChartType = SeriesChartType.Candlestick;
                series.BackSecondaryColor = Color.Green;
                series.Color = Color.Red;
                series.BorderWidth = 2;
                series.IsVisibleInLegend = false;
                series.YValueType = ChartValueType.Double;
                series.XValueType = ChartValueType.String;
                series.ShadowColor = Color.Gray;
                series.ShadowOffset = 2;
            }

            var dateType = new int[] { 0, 1, 2 }.Contains(timeType) ? 0 : 1;
            var takeCount = timeType == 0 ? 30 : timeType == 1 ? 5 * 30 : timeType == 2 ? 12 * 30 : timeType == 3 ? 30 : timeType * 30;
            var stockPrices = Repository.QueryAll<StockPriceEntity>($"StockCode='{stockCode}' and DateType={dateType}", "DealDate desc", takeCount);

            this.BindChartSeriesPoints(series, stockPrices.OrderBy(c => c.ID).ToArray(), timeType);

            double max = 0, min = 0;
            if(stockPrices.Length > 0)
            {
                max = Math.Round((double)stockPrices.Max(c => c.TodayMaxPrice) * 1.02d, 2);
                min = Math.Round((double)stockPrices.Min(c => c.TodayMinPrice) * 0.98d, 2);
            }
            var chartArea = this.chartPrice.ChartAreas[0];
            chartArea.AxisY.Maximum = max;
            chartArea.AxisY.Minimum = min;
            chartArea.AxisY.IsStartedFromZero = false;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.LineColor = Color.Gray;
            chartArea.AxisX.MajorGrid.Enabled = false;
            //chartArea.AxisX.IsReversed = true;
            chartArea.AxisX.TextOrientation = TextOrientation.Stacked;
            //chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            //chartArea.AxisX.IntervalOffsetType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.IntervalOffset = 1;
            //chartArea.AxisX.IntervalType = DateTimeIntervalType.Minutes;
        }

        void BindChartSeriesPoints(Series series, StockPriceEntity[] stockPrices, int timeType)
        {
            series.Points.Clear();

            if (timeType == 0)
            {
                this.BindChartDayPoints(series, stockPrices);
            }
            else if (timeType == 1)
            {
                this.BindChartWeekPoints(series, stockPrices);
            }
            else if (timeType == 2)
            {
                this.BindChartMonthPoints(series, stockPrices);
            }
            else if (timeType == 3)
            {
                this.BindChartMinutePoints(series, stockPrices, 1);
            }
            else
            {
                this.BindChartMinutePoints(series, stockPrices, timeType);
            }            
        }

        void BindChartDayPoints(Series series, StockPriceEntity[] stockPrices)
        {
            foreach (var item in stockPrices)
            {
                var xvalue = ObjectUtil.ToValue<DateTime>(item.DealDate, DateTime.Now).ToString("MM-dd");
                series.Points.AddXY(xvalue, item.TodayMaxPrice, item.TodayMinPrice, item.TodayStartPrice, item.TodayEndPrice);
            }
        }

        void BindChartWeekPoints(Series series, StockPriceEntity[] stockPrices)
        {
            for (var i = 29; i >= 0; i--)
            {
                var preDay = DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)DateTime.Now.DayOfWeek - 1;
                preDay = preDay + i * 7;
                var startTime = DateTime.Now.Date.AddDays(-1 * preDay);
                var endTime = startTime.AddDays(5);

                var start = startTime.ToString("yyyy-MM-dd");
                var end = endTime.ToString("yyyy-MM-dd");

                var items = stockPrices.Where(c => c.DealDate.CompareTo(start) >= 0 && c.DealDate.CompareTo(end) <= 0).ToArray();
                if (items.Length > 0)
                {
                    var maxPrice = items.Average(c => c.TodayMaxPrice);
                    var minPrice = items.Average(c => c.TodayMinPrice);
                    var startPrice = items.Average(c => c.TodayStartPrice);
                    var endPrice = items.Average(c => c.TodayEndPrice);
                    series.Points.AddXY(start, maxPrice, minPrice, startPrice, endPrice);
                }
                startTime = endTime.AddDays(1);
            }
        }

        void BindChartMonthPoints(Series series, StockPriceEntity[] stockPrices)
        {
            var startTime = DateTime.Now.Date.AddYears(-1).AddDays(-1 * DateTime.Now.Day + 1);
            while (true)
            {
                var endTime = startTime.AddMonths(1).AddDays(-1);
                
                var start = startTime.ToString("yyyy-MM-dd");
                var end = endTime.ToString("yyyy-MM-dd");
                if (start.CompareTo($"{DateTime.Now.Date.AddMonths(1).ToString("yyyy-MM-01")}") >= 0) break;

                var items = stockPrices.Where(c => c.DealDate.CompareTo(start) >= 0 && c.DealDate.CompareTo(end) <= 0).ToArray();
                if (items.Length > 0)
                {
                    var maxPrice = items.Average(c => c.TodayMaxPrice);
                    var minPrice = items.Average(c => c.TodayMinPrice);
                    var startPrice = items.Average(c => c.TodayStartPrice);
                    var endPrice = items.Average(c => c.TodayEndPrice);
                    series.Points.AddXY(start, maxPrice, minPrice, startPrice, endPrice);
                }
                startTime = endTime.AddDays(1);
            }
        }

        void BindChartMinutePoints(Series series, StockPriceEntity[] stockPrices, int minute = 1)
        {
            var startTime = DateTime.Now.Date.AddHours(9);
            while (true)
            {
                var endTime = startTime.AddMinutes(minute);
                
                var start = startTime.ToString("HH:mm");
                var end = endTime.ToString("HH:mm");
                if (start.CompareTo("11:30") >= 0 && start.CompareTo("13:00") < 0)
                {
                    startTime = endTime;
                    continue;
                }
                if (start.CompareTo("15:00") >= 0) break;
                if (end.CompareTo("15:00") >= 0) end = "15:00";

                var items = stockPrices.Where(c => c.DealTime.CompareTo(start) >= 0 && c.DealTime.CompareTo(end) <= 0).ToArray();
                if (items.Length > 0)
                {
                    var maxPrice = items.Average(c => c.TodayMaxPrice);
                    var minPrice = items.Average(c => c.TodayMinPrice);
                    var startPrice = items.Average(c => c.TodayStartPrice);
                    var endPrice = items.Average(c => c.TodayEndPrice);
                    series.Points.AddXY(end, maxPrice, minPrice, startPrice, endPrice);
                }
                startTime = endTime;
            }
        }

        void LoadPriceList(string stockCode)
        {
            var stockPrices = Repository.QueryAll<StockPriceEntity>($"StockCode='{stockCode}' and DateType=0", "DealDate desc",  60);
            var dt = ObjectUtil.ConvertTable(stockPrices);
            this.gridPriceList.DataSource = null;
            this.gridPriceList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridPriceList.ColumnCount; i++)
            {
                var columnName = this.gridPriceList.Columns[i].Name;
                if (new string[] { "股票代码", "时间类型", "结算时间点" }.Contains(columnName)) this.gridPriceList.Columns[i].Visible = false;
                else
                {
                    var length = this.gridPriceList.Columns[i].Name.Length;
                    this.gridPriceList.Columns[i].Width = length < 5 ? 80 : length < 6 ? 90 : length < 8 ? 120 : 140;
                }
            }
            for (var i = 0; i < this.gridPriceList.Rows.Count; i++)
            {
                var row = this.gridPriceList.Rows[i];
                var value = ObjectUtil.ToValue<decimal>(row.Cells["浮动(%)"].Value, 0);
                if (value > 0)
                {
                    this.gridPriceList.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    this.gridPriceList.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                }
            }
        }

        void LoadRemindList(string stockCode)
        {
            var reminds = Repository.QueryAll<RemindEntity>($"StockCode='{stockCode}'", "ID desc", 60);
            var dt = ObjectUtil.ConvertTable(reminds);
            this.gridRemindList.DataSource = null;
            this.gridRemindList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridRemindList.ColumnCount; i++)
            {
                var columnName = this.gridRemindList.Columns[i].Name;
                if (columnName == "股票代码") this.gridRemindList.Columns[i].Visible = false;
                else
                {
                    var length = this.gridRemindList.Columns[i].Name.Length;
                    this.gridRemindList.Columns[i].Width = length < 6 ? 80 : length < 8 ? 120 : 140;
                }
            }
            for (var i = 0; i < this.gridRemindList.Rows.Count; i++)
            {
                var row = this.gridRemindList.Rows[i];
                var value = ObjectUtil.ToValue<bool>(row.Cells["是否执行"].Value, false);
                if (value)
                {
                    this.gridRemindList.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                }
                else
                {
                    this.gridRemindList.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                }
                var type = ObjectUtil.ToValue<int>(row.Cells["类型"].Value, -1);
                row.Cells["类型"].Value = (type == 0 ? "涨跌幅" : type == 1 ? "上涨" : type == 2 ? "下跌" : type == 8 ? "买点" : type == 9 ? "卖点" : "");
            }
        }

        void LoadExchangeList(string stockCode)
        {
            var exchangeOrders = Repository.QueryAll<ExchangeOrderEntity>($"StockCode='{stockCode}'", "ExchangeTime desc", 60);
            var dt = ObjectUtil.ConvertTable(exchangeOrders);
            this.gridExchangeList.DataSource = null;
            this.gridExchangeList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridExchangeList.ColumnCount; i++)
            {
                var columnName = this.gridExchangeList.Columns[i].Name;
                if (columnName == "股票代码") this.gridExchangeList.Columns[i].Visible = false;
                else
                {
                    var length = columnName.Length;
                    this.gridExchangeList.Columns[i].Width = length < 6 ? 80 : length < 8 ? 120 : 140;
                }
            }
        }


        void LoadMainTargetInfo(string stockCode, int rtype = 0)
        {
            var mainTargets = Repository.QueryAll<MainTargetEntity>($"StockCode='{stockCode}' and Rtype={rtype}", "Date desc", 60);
            //var dt = ObjectUtil.ConvertTable(mainTargets);
            var dt = EastMoneyUtil.ConvertMainTargetData(mainTargets);
            this.gridMaintargetList.DataSource = null;
            this.gridMaintargetList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridMaintargetList.ColumnCount; i++)
            {
                if (i == 0) this.gridMaintargetList.Columns[i].Width = 160;
                else
                {
                    this.gridMaintargetList.Columns[i].Width = 100;
                }
            }
        }

        void LoadStockStrategyList(string stockCode)
        {
            var strategyDetails = Repository.QueryAll<StockStrategyEntity>($"StockCode='{stockCode}'");
            var dt = ObjectUtil.ConvertTable(strategyDetails);
            this.gridStockStrategyList.DataSource = null;
            this.gridStockStrategyList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridStockStrategyList.ColumnCount; i++)
            {
                var columnName = this.gridStockStrategyList.Columns[i].Name;
                if (columnName == "股票代码") this.gridStockStrategyList.Columns[i].Visible = false;
                else
                {
                    var length = columnName.Length;
                    this.gridStockStrategyList.Columns[i].Width = length < 6 ? 80 : length < 8 ? 120 : 140;
                }
            }
        }

        #endregion

        #region 加载ListView数据
        void LoadStockBaseInfo(string stockCode)
        {
            this.lstBaseInfo.Items.Clear();

            var stock = Repository.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null) return;

            var preps = typeof(StockEntity).GetProperties();
            foreach(var prep in preps)
            {
                if (prep.Name == "ID") continue;

                var desc = prep.Name;
                var attr = prep.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
                if (attr != null) desc = attr.Description;

                var listViewItem = new ListViewItem();
                listViewItem.Text = $"{desc}";
                listViewItem.SubItems.Add($"{ObjectUtil.GetPropertyValue(stock, prep.Name)}");
                this.lstBaseInfo.Items.Add(listViewItem);
            }
            //foreach (DataGridViewCell cell in selectRow.Cells)
            //{
            //    var listViewItem = new ListViewItem();
            //    listViewItem.Text = $"{this.gridStockList.Columns[cell.ColumnIndex].Name}";
            //    listViewItem.SubItems.Add($"{selectRow.Cells[cell.ColumnIndex].Value}");
            //    this.lstBaseInfo.Items.Add(listViewItem);
            //}
        }

        void LoadStockStrategyInfo(string stockCode)
        {
            this.lstStrategyInfo.Items.Clear();

            var accountStock = Repository.QueryFirst<AccountStockEntity>($"StockCode='{stockCode}'");
            if (accountStock == null) return;

            var strategy = Repository.QueryFirst<StrategyEntity>($"Name='{accountStock.StrategyName}'");
            if (strategy == null) return;

            var preps = typeof(StrategyEntity).GetProperties();
            foreach (var prep in preps)
            {
                if (prep.Name == "ID") continue;

                var desc = prep.Name;
                var attr = prep.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
                if (attr != null) desc = attr.Description;

                var listViewItem = new ListViewItem();
                listViewItem.Text = $"{desc}";
                listViewItem.SubItems.Add($"{ObjectUtil.GetPropertyValue(strategy, prep.Name)}");
                this.lstStrategyInfo.Items.Add(listViewItem);
            }
        }

        #endregion

        #region GridList事件
        /// <summary>
        /// 自选股列表点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            this.lstBaseInfo.Items.Clear();

            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            this.LoadStockBaseInfo(stockCode);
            this.LoadStockStrategyInfo(stockCode);
            this.LoadTabGridList(this.tabControlBottom.SelectedIndex, stockCode);
            //this.LoadPriceList(stockCode);
            //this.LoadExchangeList(stockCode);
            //this.LoadMainTargetInfo(stockCode);
        }

        /// <summary>
        /// 历史股价列表点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridPriceList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.gridPriceList.SelectedRows.Count == 0) return;
            this.lstPriceInfo.Items.Clear();

            var selectRow = this.gridPriceList.SelectedRows[0];
            foreach (DataGridViewCell cell in selectRow.Cells)
            {
                var listViewItem = new ListViewItem();
                listViewItem.Text = $"{this.gridPriceList.Columns[cell.ColumnIndex].Name}";
                listViewItem.SubItems.Add($"{selectRow.Cells[cell.ColumnIndex].Value}");
                this.lstPriceInfo.Items.Add(listViewItem);
            }
        }

        /// <summary>
        /// 交易数据列表点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void gridExchangeList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.gridExchangeList.SelectedRows.Count == 0) return;
            this.lstExchangeInfo.Items.Clear();

            var selectRow = this.gridExchangeList.SelectedRows[0];
            foreach (DataGridViewCell cell in selectRow.Cells)
            {
                var listViewItem = new ListViewItem();
                listViewItem.Text = $"{this.gridExchangeList.Columns[cell.ColumnIndex].Name}";
                listViewItem.SubItems.Add($"{selectRow.Cells[cell.ColumnIndex].Value}");
                this.lstExchangeInfo.Items.Add(listViewItem);
            }

        }

        #endregion

        #region 工具栏按钮事件
        private void btnAddStock_Click(object sender, EventArgs e)
        {
            var stockCode = string.Empty;
            var dlg = new NewStockForm();
            if (dlg.ShowDialog() == DialogResult.OK) stockCode = dlg.StockCode;
            if (string.IsNullOrEmpty(stockCode)) return;

            var stock = Repository.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null)
            {
                stock = new StockEntity()
                {
                    Code = stockCode
                };
                Repository.Insert<StockEntity>(stock);
            }
            this.LoadStockList();
        }

        private void btnDeleteStock_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            var stockName = $"{selectRow.Cells["股票名称"].Value}";

            if (MessageBox.Show($"确认要删除自选股[{stockName}]?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

            var stock = Repository.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if(stock != null)
            {
                Repository.Delete<StockEntity>(stock);
            }
            var stockStrategy = Repository.QueryFirst<AccountStockEntity>($"StockCode='{stockCode}'");
            if(stockStrategy != null)
            {
                Repository.Delete<AccountStockEntity>(stockStrategy);
            }
            var stockStrategyDetails = Repository.QueryAll<StockStrategyEntity>($"StockCode='{stockCode}'");
            if(stockStrategyDetails.Length > 0)
            {
                Repository.Delete<StockStrategyEntity>($"StockCode='{stockCode}'");
            }
            this.LoadStockList();
        }

        private void btnAccountInfo_Click(object sender, EventArgs e)
        {
            var frm = new AccountForm();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }
        private void btnConfig_Click(object sender, EventArgs e)
        {
            var frm = new ConfigForm();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void btnSetStrategy_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            var stockName = $"{selectRow.Cells["股票名称"].Value}";

            var frm = new StrategyForm();
            frm.StockCode = stockCode;
            frm.StockName = stockName;
            frm.StartPosition = FormStartPosition.CenterParent;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.LoadStockStrategyList(stockCode);
            }
        }

        private void btnAddRmind_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;

            var account = Repository.QueryFirst<AccountEntity>($"RealType='True'"); 
            if (account == null) return;

            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            var frm = new RemindForm();
            frm.StartPosition = FormStartPosition.CenterParent;
            if(frm.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(frm.UDPer))
                {
                    var udPers = ObjectUtil.GetSplitArray(frm.UDPer, ",");
                    foreach (var udPer in udPers)
                    {
                        var target = ObjectUtil.ToValue<decimal>(udPer, 0);
                        if (target == 0) continue;

                        var remind = Repository.QueryFirst<RemindEntity>($"StockCode='{stockCode}' and Target={udPer} and RType=0");
                        if (remind == null)
                        {
                            remind = new RemindEntity()
                            {
                                StockCode = stockCode,
                                Target = target,
                                Email = account.Email,
                                QQ = account.QQ,
                                RType = 0
                            };
                            Repository.Insert<RemindEntity>(remind);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(frm.UpPrice))
                {
                    var upPrices = ObjectUtil.GetSplitArray(frm.UpPrice, ",");
                    foreach (var upPrice in upPrices)
                    {
                        var target = ObjectUtil.ToValue<decimal>(upPrice, 0);
                        if (target == 0) continue;

                        var remind = Repository.QueryFirst<RemindEntity>($"StockCode='{stockCode}' and Target={upPrice} and RType=1");
                        if (remind == null)
                        {
                            remind = new RemindEntity()
                            {
                                StockCode = stockCode,
                                Target = target,
                                Email = account.Email,
                                QQ = account.QQ,
                                RType = 0
                            };
                            remind.MaxPrice = Math.Round(remind.Target * (1 + RC.RemindStockPriceFloatPer / 100m), 2);
                            remind.MinPrice = Math.Round(remind.Target * (1 - RC.RemindStockPriceFloatPer / 100m), 2);
                            Repository.Insert<RemindEntity>(remind);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(frm.DownPrice))
                {
                    var downPrices = ObjectUtil.GetSplitArray(frm.DownPrice, ",");
                    foreach (var downPrice in downPrices)
                    {
                        var target = ObjectUtil.ToValue<decimal>(downPrice, 0);
                        if (target == 0) continue;

                        var remind = Repository.QueryFirst<RemindEntity>($"StockCode='{stockCode}' and Target={downPrice} and RType=1");
                        if (remind == null)
                        {
                            remind = new RemindEntity()
                            {
                                StockCode = stockCode,
                                Target = target,
                                Email = account.Email,
                                QQ = account.QQ,
                                RType = 2
                            };
                            remind.MaxPrice = Math.Round(remind.Target * (1 + RC.RemindStockPriceFloatPer / 100m), 2);
                            remind.MinPrice = Math.Round(remind.Target * (1 - RC.RemindStockPriceFloatPer / 100m), 2);
                            Repository.Insert<RemindEntity>(remind);
                        }
                    }
                }
                this.LoadRemindList(stockCode);
            }
        }

        #endregion

        #region 底部TabControl区域事件

        private void tabControlBottom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            this.LoadTabGridList(this.tabControlBottom.SelectedIndex, stockCode);
        }

        void LoadTabGridList(int tabIndex, string stockCode)
        {
            switch (tabIndex)
            {
                case 1:
                    this.LoadPriceChart(stockCode);
                    break;
                case 2:
                    this.LoadPriceList(stockCode);
                    break;
                case 3:
                    this.LoadStockStrategyList(stockCode);
                    break;
                case 4:
                    this.LoadRemindList(stockCode);
                    break;
                case 5:
                    this.LoadExchangeList(stockCode);
                    break;
                case 6:
                    this.LoadMainTargetInfo(stockCode);
                    break;
            }
        }

        private void txtByReport_CheckedChanged(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            if (this.txtByReport.Checked)
            {
                this.LoadMainTargetInfo(stockCode, 0);
            }
        }

        private void txtByYear_CheckedChanged(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            if (this.txtByYear.Checked)
            {
                this.LoadMainTargetInfo(stockCode, 1);
            }
        }

        private void txtByQuarter_CheckedChanged(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            if (this.txtByQuarter.Checked)
            {
                this.LoadMainTargetInfo(stockCode, 2);
            }
        }
        
        private void btnHandleRemind_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            if (this.gridRemindList.SelectedRows.Count == 0) return;
            
            if (MessageBox.Show($"确认要标记执行选中的提醒数据?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

            for (var i = 0; i < this.gridRemindList.SelectedRows.Count; i++)
            {
                var row = this.gridRemindList.SelectedRows[i];

                var type = $"{row.Cells["类型"].Value}";
                var rtype = (type == "涨跌幅" ? 0 : type == "上涨" ? 1 : type == "下跌" ? 2 : type == "买点" ? 8 : type == "卖点" ? 9 : -1);
                var target = ObjectUtil.ToValue<decimal>(row.Cells["目标"].Value, 0);

                var remind = Repository.QueryFirst<RemindEntity>($"StockCode='{stockCode}' and RType={rtype} and Target={target} and Handled='False'");
                if (remind != null)
                {
                    remind.Handled = true;
                    Repository.Update<RemindEntity>(remind);
                }
            }
            this.LoadRemindList(stockCode);
        }

        private void btnCancelRemind_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            if (this.gridRemindList.SelectedRows.Count == 0) return;

            if (MessageBox.Show($"确认要取消选中的提醒数据?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

            for (var i = 0; i < this.gridRemindList.SelectedRows.Count; i++)
            {
                var row = this.gridRemindList.SelectedRows[i];

                var type = $"{row.Cells["类型"].Value}";
                var rtype = (type == "涨跌幅" ? 0 : type == "上涨" ? 1 : type == "下跌" ? 2 : type == "买点" ? 8 : type == "卖点" ? 9 : -1);
                var target = ObjectUtil.ToValue<decimal>(row.Cells["目标"].Value, 0);

                var remind = Repository.QueryFirst<RemindEntity>($"StockCode='{stockCode}' and RType={rtype} and Target={target}");
                if (remind != null)
                {
                    Repository.Delete<RemindEntity>(remind);
                }
            }
            this.LoadRemindList(stockCode);
        }

        private void btnBuyExchange_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            var stockName = $"{selectRow.Cells["股票名称"].Value}";
            var stockPrice = ObjectUtil.ToValue<decimal>($"{selectRow.Cells["股价"].Value}", 0);

            var frm = new ExchangeForm();
            frm.DealType = 0;
            frm.StockCode = stockCode;
            frm.StockName = stockName;
            frm.DealPrice = stockPrice;
            frm.StartPosition = FormStartPosition.CenterParent;
            if(frm.ShowDialog() == DialogResult.OK)
            {
                this.LoadExchangeList(stockCode);
                this.LoadAccountStockList();
            }
        }

        private void btnSaleExchange_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            var stockPrice = ObjectUtil.ToValue<decimal>($"{selectRow.Cells["股价"].Value}", 0);

            var frm = new ExchangeForm();
            frm.DealType = 1;
            frm.StockCode = stockCode;
            frm.DealPrice = stockPrice;
            frm.StartPosition = FormStartPosition.CenterParent;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.LoadExchangeList(stockCode);
                this.LoadAccountStockList();
            }
        }


        private void btnDayChart_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            this.LoadPriceChart(stockCode, 0);
        }

        private void btnWeekChart_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            this.LoadPriceChart(stockCode, 1);
        }

        private void btnMonthChart_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            this.LoadPriceChart(stockCode, 2);
        }

        private void btnOneChart_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            this.LoadPriceChart(stockCode, 3);
        }

        private void btnFiveChart_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            this.LoadPriceChart(stockCode, 5);
        }

        private void btnFifteenChart_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            this.LoadPriceChart(stockCode, 15);
        }

        private void btnThirtyChart_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            this.LoadPriceChart(stockCode, 30);
        }

        private void btnSixtyChart_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            this.LoadPriceChart(stockCode, 60);
        }
        #endregion

        #region 操作日志事件

        public void ActionLog(string message)
        {
            Action act = delegate ()
            {
                this.txtActionLog.AppendText($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}\n");
                this.txtActionLog.SelectionStart = this.txtActionLog.Text.Length;
                this.txtActionLog.ScrollToCaret();
            };
            this.Invoke(act);
        }

        public void ShowMessage(string message)
        {
            Action act = delegate ()
            {
                this.lblMessage.Text = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}";
            };
            this.Invoke(act);
        }
        #endregion
    }
}
