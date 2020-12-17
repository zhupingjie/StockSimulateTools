using StockPriceTools.UI;
using StockSimulateCore.Config;
using StockSimulateCore.Entity;
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
            //采集价格数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                while (true)
                {
                    if (ObjectUtil.EffectStockDealTime())
                    {
                        StockGatherService.GatherPriceData((message) =>
                        {
                            ActionLog(message);
                        });
                        Action act = delegate ()
                        {
                            this.LoadStockList();
                        };
                        this.Invoke(act);
                    }
                    Thread.Sleep(RC.GatherStockPriceInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            //计算盈亏数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                while (true)
                {
                    if (ObjectUtil.EffectStockDealTime())
                    {
                        StockGatherService.CalculateProfit((message) =>
                        {
                            ActionLog(message);
                        });
                        Action act = delegate ()
                        {
                            this.LoadAccountStockList();
                        };
                        this.Invoke(act);
                    }
                    Thread.Sleep(RC.UpdateAccountStockProfitInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            //采集财务数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(10000);
                while (true)
                {
                    if (ObjectUtil.EffectStockDealTime())
                    {
                        StockGatherService.GatherFinanceData((message) =>
                        {
                            ActionLog(message);
                        });
                    }
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
                                remind.RemindPrice = stock.Price;
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
                                remind.RemindPrice = stock.Price;
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
                                remind.RemindPrice = stock.Price;
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
                                remind.RemindPrice = stock.Price;
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
                                remind.RemindPrice = stock.Price;
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
            var chartType = timeType == 0 ? SeriesChartType.Candlestick : SeriesChartType.Line;
            this.chartPrice.Series.Clear();
            var series = this.chartPrice.Series.Add("");
            series.ChartType = chartType;
            series.BackSecondaryColor = Color.Green;
            series.Color = Color.Red;
            series.BorderWidth = 2;
            series.IsVisibleInLegend = false;
            series.YValueType = ChartValueType.Double;
            series.XValueType = ChartValueType.String;
            series.ShadowColor = Color.Gray;
            series.ShadowOffset = 2;

            var dateType = new int[] { 0, 1, 2 }.Contains(timeType) ? 0 : 1;
            var startDate = DateTime.Now.Date;
            if (timeType == 0) startDate = startDate.AddMonths(-1);
            else if (timeType == 1) startDate = startDate.AddDays(-5 * 30);
            else if (timeType == 2) startDate = startDate.AddYears(-1);
            var stockPrices = Repository.QueryAll<StockPriceEntity>($"StockCode='{stockCode}' and DateType={dateType} and DealDate>='{startDate.ToString("yyyy-MM-dd")}'", "ID desc");

            this.BindChartSeriesPoints(series, stockPrices.OrderBy(c => c.ID).ToArray(), timeType, chartType == SeriesChartType.Candlestick ? 4 : 1);

            var chartArea = this.chartPrice.ChartAreas[0];
            //chartArea.AxisY.Maximum = max;
            //chartArea.AxisY.Minimum = min;
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

        #region 加载走势图
        void BindChartSeriesPoints(Series series, StockPriceEntity[] stockPrices, int timeType, int pointCount = 1)
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
                    //var maxPrice = items.Average(c => c.TodayMaxPrice);
                    //var minPrice = items.Average(c => c.TodayMinPrice);
                    //var startPrice = items.Average(c => c.TodayStartPrice);
                    var endPrice = items.Average(c => c.TodayEndPrice);
                    //series.Points.AddXY(start, maxPrice, minPrice, startPrice, endPrice);
                    series.Points.AddXY(start, endPrice);
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
                    //var maxPrice = items.Average(c => c.TodayMaxPrice);
                    //var minPrice = items.Average(c => c.TodayMinPrice);
                    //var startPrice = items.Average(c => c.TodayStartPrice);
                    var endPrice = items.Average(c => c.TodayEndPrice);
                    //series.Points.AddXY(start, maxPrice, minPrice, startPrice, endPrice);
                    series.Points.AddXY(start, endPrice);
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

                var items = stockPrices.Where(c => c.DealDate == DateTime.Now.ToString("yyyy-MM-dd") && c.DealTime.CompareTo(start) >= 0 && c.DealTime.CompareTo(end) <= 0).ToArray();
                if (items.Length > 0)
                {
                    //var maxPrice = items.Average(c => c.TodayMaxPrice);
                    //var minPrice = items.Average(c => c.TodayMinPrice);
                    //var startPrice = items.Average(c => c.TodayStartPrice);
                    var endPrice = items.Average(c => c.UDPer);
                    //series.Points.AddXY(end, maxPrice, minPrice, startPrice, endPrice);
                    series.Points.AddXY(end, endPrice);
                }
                startTime = endTime;
            }
        }

        #endregion

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

        void LoadStockStrategyList(string stockCode)
        {
            var strategyDetails = Repository.QueryAll<StockStrategyEntity>($"StockCode='{stockCode}'", "ID asc");
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
            for (var i = 0; i < this.gridStockStrategyList.Rows.Count; i++)
            {
                var row = this.gridStockStrategyList.Rows[i];
                var value = ObjectUtil.ToValue<int>(row.Cells["执行策略"].Value, -1);
                this.gridStockStrategyList.Rows[i].Cells["执行策略"].Value = (value == 1 ? "自动模拟交易" : value == 0 ? "买卖点提醒" : "");

                var mode = ObjectUtil.ToValue<int>(row.Cells["条件"].Value, -1);
                this.gridStockStrategyList.Rows[i].Cells["条件"].Value = (mode == 0 ? "低于" : mode == 1 ? "高于" : mode == 2 ? "等待" : "");

                var ok = ObjectUtil.ToValue<int>(row.Cells["执行结果"].Value, -1);
                this.gridStockStrategyList.Rows[i].Cells["执行结果"].Value = (ok == 1 ? "成功" : ok == 2 ? "失败" : ok == 0 ? "等待" : "");
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
        }
        
        #endregion

        #region GridList事件
        /// <summary>
        /// 自选股列表点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridStockList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            this.lstBaseInfo.Items.Clear();

            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            this.LoadStockBaseInfo(stockCode);
            this.LoadTabGridList(this.tabControlBottom.SelectedIndex, stockCode);
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


        private void gridStockStrategyList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.gridStockStrategyList.SelectedRows.Count == 0) return;
            this.lstStrategyInfo.Items.Clear();

            var selectRow = this.gridStockStrategyList.SelectedRows[0];
            foreach (DataGridViewCell cell in selectRow.Cells)
            {
                var listViewItem = new ListViewItem();
                listViewItem.Text = $"{this.gridStockStrategyList.Columns[cell.ColumnIndex].Name}";
                listViewItem.SubItems.Add($"{selectRow.Cells[cell.ColumnIndex].Value}");
                this.lstStrategyInfo.Items.Add(listViewItem);
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

            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            var frm = new RemindForm();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.StockCode = stockCode;
            if(frm.ShowDialog() == DialogResult.OK)
            {
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
                    this.LoadPriceChart(stockCode, 3);
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
