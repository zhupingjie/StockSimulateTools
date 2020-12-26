﻿using StockPriceTools.UI;
using StockSimulateDomain.Entity;
using StockSimulateDomain.Model;
using StockSimulateService.Service;
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
using StockSimulateService.Utils;
using StockSimulateService.Config;

namespace StockPriceTools
{
    public partial class DesktopFrom : Form
    {
        private MySQLDBUtil Repository = MySQLDBUtil.Instance;
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
        }

        void LoadConfigData()
        {
            Task.Factory.StartNew(() =>
            {
                MySQLDBUtil.Instance.InitDataBase();

                while (true)
                {
                    StockConfigService.LoadGlobalConfig(RC);

                    Thread.Sleep(RC.LoadGlobalConfigInterval * 1000);
                }
            }, CancellationTokenSource.Token);
        }

        void LoadStockData()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2 * 1000);
                while (true)
                {
                    Action act = delegate ()
                    {
                        this.LoadStockList();
                    };
                    this.Invoke(act);

                    Thread.Sleep(RC.GatherStockPriceInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5 * 1000);
                while (true)
                {
                    Action act = delegate ()
                    {
                        this.LoadMessageInfo();
                    };
                    this.Invoke(act);

                    Thread.Sleep(RC.LoadMessageInterval * 1000);
                }
            }, CancellationTokenSource.Token);
        }

        #endregion

        #region 顶部工具栏按钮事件
        private void btnAddStock_Click(object sender, EventArgs e)
        {
            var frm = new NewStockForm();
            frm.StartPosition = FormStartPosition.CenterParent;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.LoadStockList();
            }
        }

        private void btnDeleteStock_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            var stockName = $"{selectRow.Cells["股票名称"].Value}";

            if (MessageBox.Show($"确认要删除自选股[{stockName}]?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

            StockService.Delete(stockCode);
           
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
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.LoadRemindList(stockCode);
            }
        }

        private void btnValuate_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            var frm = new ValuateForm();
            frm.StockCode = stockCode;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.Show();
        }


        private void btnDebug_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            var frm = new DebugForm();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.StockCode = stockCode;
            frm.Show();
        }
        #endregion

        #region 中间TabControl区域事件

        int CurrentStockListSelectedIndex = -1;
        void LoadStockList()
        {
            var where = "1>0";//"Price>0";
            if (!string.IsNullOrEmpty(this.txtSearch.Text.Trim())) where += $" and (Code like '%{this.txtSearch.Text.Trim()}%' or Name like '%{this.txtSearch.Text.Trim()}%')";
            if (this.txtFoucST.Checked) where += $" and (Foucs=1)";
            else if (this.txtSHSZ.Checked) where += $" and (Type=0)";
            else if (this.txtETF.Checked) where += $" and (Type=1)";
            else if (this.txtAllStock.Checked) where += "";

            var stocks = Repository.QueryAll<StockEntity>(where);
            var dt = ObjectUtil.ConvertTable(stocks);
            this.gridStockList.DataSource = null;
            this.gridStockList.DataSource = dt.DefaultView;

            var goodCellStyle = new DataGridViewCellStyle();
            //goodCellStyle.Font = new Font("宋体", 8.5f, FontStyle.Bold);
            goodCellStyle.BackColor = Color.LightYellow;
            for (var i=0; i<this.gridStockList.ColumnCount; i++)
            {
                this.gridStockList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                
                var columnName = this.gridStockList.Columns[i].Name;
                this.gridStockList.Columns[i].Width = ObjectUtil.GetGridColumnLength(columnName);
                if (new string[] { "股价", "安全股价", "预测股价", "浮动(%)" }.Contains(columnName))
                {
                    this.gridStockList.Columns[i].DefaultCellStyle = goodCellStyle;
                }
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
            this.lblStockTotal.Text = $"股票总数:[{stocks.Length}]";
        }

        void LoadAccountStockList()
        {
            var account = Repository.QueryFirst<AccountEntity>($"Name='{RC.CurrentAccountName}'");
            if (account == null)
            {
                this.lblAccountStockInfo.Text = $"当前交易账户未设置";
                return;
            }

            var where = $"AccountName='{account.Name}'";
            var search = this.txtAccountSearch.Text.Trim();
            if (!string.IsNullOrEmpty(search)) where += $" and (Code like '%{search}%' or Name like '%{search}%')";
            if (this.txtHoldQty.Checked) where += " and HoldQty>0";

            var accountStocks = Repository.QueryAll<AccountStockEntity>(where);
            var dt = ObjectUtil.ConvertTable(accountStocks);
            this.gridAccountStockList.DataSource = null;
            this.gridAccountStockList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridAccountStockList.ColumnCount; i++)
            {
                this.gridAccountStockList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                var columnName = this.gridAccountStockList.Columns[i].Name;
                this.gridAccountStockList.Columns[i].Width = ObjectUtil.GetGridColumnLength(columnName);
            }
            for (var i = 0; i < this.gridAccountStockList.Rows.Count; i++)
            {
                var row = this.gridAccountStockList.Rows[i];
                var value = ObjectUtil.ToValue<decimal>(row.Cells["盈亏(元)"].Value, 0);
                if (value > 0)
                {
                    this.gridAccountStockList.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    this.gridAccountStockList.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                }
            }

            this.lblAccountStockTotal.Text = $"总股票数:[{accountStocks.Length}]";
            this.lblAccountStockInfo.Text = $"账户市值【{account.TotalAmount}】,持有市值【{account.HoldAmount}】,盈亏【{account.Profit}】";
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.tabControlMain.SelectedIndex == 1)
            {
                this.LoadAccountStockList();
            }
        }


        private void gridStockList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            this.lstBaseInfo.Items.Clear();

            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            var stockName = $"{selectRow.Cells["股票名称"].Value}";
            if (selectRow.Index >= 0) this.CurrentStockListSelectedIndex = selectRow.Index;

            Action act = delegate ()
            {
                this.LoadStockBaseInfo(stockCode);
                this.LoadTabGridList(this.tabControlBottom.SelectedIndex, stockCode, stockName);
            };
            this.Invoke(act);
        }

        private void btnFoucsStock_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            StockService.Foucs(stockCode);

            Action act = delegate ()
            {
                this.LoadStockList();
            };
            this.Invoke(act);
        }

        private void txtFoucST_CheckedChanged(object sender, EventArgs e)
        {
            this.CurrentStockListSelectedIndex = -1;

            Action act = delegate ()
            {
                this.LoadStockList();
            };
            this.Invoke(act);
        }
        private void txtAllStock_CheckedChanged(object sender, EventArgs e)
        {
            this.CurrentStockListSelectedIndex = -1;

            Action act = delegate ()
            {
                this.LoadStockList();
            };
            this.Invoke(act);
        }

        private void txtSHSZ_CheckedChanged(object sender, EventArgs e)
        {
            this.CurrentStockListSelectedIndex = -1;

            Action act = delegate ()
            {
                this.LoadStockList();
            };
            this.Invoke(act);
        }

        private void txtETF_CheckedChanged(object sender, EventArgs e)
        {
            this.CurrentStockListSelectedIndex = -1;

            Action act = delegate ()
            {
                this.LoadStockList();
            };
            this.Invoke(act);
        }
        private void gridStockList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.gridStockList.ClearSelection();
            if (CurrentStockListSelectedIndex >= 0 && this.gridStockList.Rows.Count> CurrentStockListSelectedIndex) this.gridStockList.Rows[CurrentStockListSelectedIndex].Selected = true;
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Action act = delegate ()
                {
                    this.LoadStockList();
                };
                this.Invoke(act);
            }
        }
        private void txtAccountStock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Action act = delegate ()
                {
                    this.LoadAccountStockList();
                };
                this.Invoke(act);
            }
        }

        private void txtHoldQty_CheckedChanged(object sender, EventArgs e)
        {
            Action act = delegate ()
            {
                this.LoadAccountStockList();
            };
            this.Invoke(act);
        }

        private void txtRealType_CheckedChanged(object sender, EventArgs e)
        {
            Action act = delegate ()
            {
                this.LoadAccountStockList();
            };
            this.Invoke(act);
        }
        #endregion

        #region 右侧ListView区域事件
        void LoadStockBaseInfo(string stockCode)
        {
            this.lstBaseInfo.Items.Clear();

            var stock = Repository.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null) return;

            var preps = typeof(StockEntity).GetProperties();
            foreach(var prep in preps)
            {
                if (prep.Name == "ID") continue;
                //if (prep.GetCustomAttributes(typeof(GridColumnIgnoreAttribute), true).Length > 0) continue;

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

        #region 底部TabControl区域事件

        void LoadMessageInfo()
        {
            var messages = Repository.QueryAll<MessageEntity>($"Handled=0 and NoticeTime<='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' and ReadTime is null", "ID asc");
            foreach(var message in messages)
            {
                message.ReadTime = DateTime.Now;

                this.ActionLog(message.Title);
            }
            Repository.Update<MessageEntity>(messages);
        }

        /// <summary>
        ///  加载走势图
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="dateType">0:日,1:分钟</param>
        void LoadPriceChart(string stockCode, string stockName, int dateType = 0, bool chartWithZS = true)
        {
            var title = this.chartPrice.Titles.FirstOrDefault();
            if (title == null) title = this.chartPrice.Titles.Add("");
            var text = $"【{stockName}】分时图";
            var stock = Repository.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock != null)
            {
                text += "【";
                text += $"M5:{stock.AvgPrice5}{(stock.Price > stock.AvgPrice5 ? "↗" : stock.Price < stock.AvgPrice5 ? "↘" : "→")}";
                if (stock.AvgPrice10 > 0) text += $",M10:{stock.AvgPrice10}{(stock.Price > stock.AvgPrice10 ? "↗" : stock.Price < stock.AvgPrice10 ? "↘" : "→")}";
                if (stock.AvgPrice20 > 0) text += $",M20:{stock.AvgPrice20}{(stock.Price > stock.AvgPrice20 ? "↗" : stock.Price < stock.AvgPrice20 ? "↘" : "→")}";
                if (stock.AvgPrice60 > 0) text += $",M60:{stock.AvgPrice60}{(stock.Price > stock.AvgPrice60 ? "↗" : stock.Price < stock.AvgPrice60 ? "↘" : "→")}";
                if (stock.AvgPrice120 > 0) text += $",M120:{stock.AvgPrice120}{(stock.Price > stock.AvgPrice120 ? "↗" : stock.Price < stock.AvgPrice120 ? "↘" : "→")}";
                if (stock.AvgPrice250 > 0) text += $",M250:{stock.AvgPrice250}{(stock.Price > stock.AvgPrice250 ? "↗" : stock.Price < stock.AvgPrice250 ? "↘" : "→")}";
                text += "】";
            }
            title.Text = text;
            title.ForeColor = Color.Blue;

            var series = this.chartPrice.Series.FirstOrDefault(c => c.Name == "FOUCS");
            if (series == null) series = this.chartPrice.Series.Add("FOUCS");

            if (dateType == 0)
            {
                series.IsValueShownAsLabel = true;

                this.BindStockPriceChart(series, stockCode, dateType, Color.Red);

                var zsSeries = this.chartPrice.Series.FirstOrDefault(c => c.Name == "ZS");
                if (zsSeries != null) this.chartPrice.Series.Remove(zsSeries);
            }
            else
            {
                series.IsValueShownAsLabel = false;

                this.BindStockPriceChart(series, stockCode, dateType, Color.Red);

                if (chartWithZS)
                {
                    var zsSeries = this.chartPrice.Series.FirstOrDefault(c => c.Name == "ZS");
                    if (zsSeries == null) zsSeries = this.chartPrice.Series.Add("ZS");

                    this.BindStockPriceChart(zsSeries, "SH000001", dateType, Color.Blue);
                }
            }

            var chartArea = this.chartPrice.ChartAreas[0];
            chartArea.AxisY.IsStartedFromZero = false;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.LineColor = Color.Gray;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.TextOrientation = TextOrientation.Stacked;
        }

        void BindStockPriceChart(Series series, string stockCode, int dateType, Color lineColor)
        {
            series.ChartType = SeriesChartType.Line;
            series.BackSecondaryColor = Color.Green;
            series.Color = lineColor;
            series.BorderWidth = 2;
            series.IsVisibleInLegend = false;
            series.YValueType = ChartValueType.Double;
            series.XValueType = ChartValueType.String;
            series.ShadowColor = Color.Gray;
            series.ShadowOffset = 2;

            var startDate = DateTime.Now.Date;
            if (dateType == 0) startDate = startDate.AddMonths(-1);
            else
            {
                var lastDate = Repository.QueryAll<StockPriceEntity>($"StockCode='{stockCode}' and DateType={dateType}", "DealDate Desc", 1).FirstOrDefault();
                if (lastDate != null)
                {
                    startDate = DateTime.Parse(lastDate.DealDate);
                }
            }
            var stockPrices = Repository.QueryAll<StockPriceEntity>($"StockCode='{stockCode}' and DateType={dateType} and DealDate>='{startDate.ToString("yyyy-MM-dd")}'", "ID desc");
            stockPrices = stockPrices.OrderBy(c => c.DealDate).ToArray();

            this.BindChartSeriesPoints(series, stockPrices, dateType, startDate);
        }

        #region 加载走势图
        void BindChartSeriesPoints(Series series, StockPriceEntity[] stockPrices, int dateType, DateTime startDate)
        {
            series.Points.Clear();

            if (dateType == 0)
            {
                this.BindChartDayPoints(series, stockPrices, dateType);
            }
            else
            {
                this.BindChartMinutePoints(series, stockPrices, dateType, startDate);
            }
        }

        void BindChartDayPoints(Series series, StockPriceEntity[] stockPrices, int dateType)
        {
            foreach (var item in stockPrices)
            {
                var xvalue = ObjectUtil.ToValue<DateTime>(item.DealDate, DateTime.Now).ToString("MM-dd");
                //series.Points.AddXY(xvalue, item.TodayMaxPrice, item.TodayMinPrice, item.TodayStartPrice, item.TodayEndPrice);
                series.Points.AddXY(xvalue, item.Price);
            }
        }
        
        void BindChartMinutePoints(Series series, StockPriceEntity[] stockPrices, int minute, DateTime startDate)
        {
            var startTime = startDate.Date.AddHours(9);
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

                var items = stockPrices.Where(c => c.DealDate == startDate.ToString("yyyy-MM-dd") && c.DealTime.CompareTo(start) >= 0 && c.DealTime.CompareTo(end) <= 0).ToArray();
                if (items.Length > 0)
                {
                    //var maxPrice = items.Average(c => c.TodayMaxPrice);
                    //var minPrice = items.Average(c => c.TodayMinPrice);
                    //var startPrice = items.Average(c => c.TodayStartPrice);
                    var endPrice = items.Average(c => c.UDPer);
                    //series.Points.AddXY(end, maxPrice, minPrice, startPrice, endPrice);
                    series.Points.AddXY($"{endTime.ToString("yy/MM/dd HH:mm")}", endPrice);
                }
                startTime = endTime;
            }
        }

        #endregion

        void LoadPriceList(string stockCode)
        {
            var stockPrices = Repository.QueryAll<StockPriceEntity>($"StockCode='{stockCode}' and DateType=0", "DealDate desc", 60);
            var dt = ObjectUtil.ConvertTable(stockPrices);
            this.gridPriceList.DataSource = null;
            this.gridPriceList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridPriceList.ColumnCount; i++)
            {
                this.gridPriceList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                var columnName = this.gridPriceList.Columns[i].Name;
                if (new string[] { "股票代码", "时间类型", "结算时间点" }.Contains(columnName)) this.gridPriceList.Columns[i].Visible = false;
                else
                {
                    this.gridPriceList.Columns[i].Width = ObjectUtil.GetGridColumnLength(columnName);
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
            var dt = ObjectUtil.ConvertTable(strategyDetails, true);
            this.gridStockStrategyList.DataSource = null;
            this.gridStockStrategyList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridStockStrategyList.ColumnCount; i++)
            {
                this.gridStockStrategyList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                var columnName = this.gridStockStrategyList.Columns[i].Name;
                if (columnName == "股票代码") this.gridStockStrategyList.Columns[i].Visible = false;
                else
                {
                    this.gridStockStrategyList.Columns[i].Width = ObjectUtil.GetGridColumnLength(columnName);
                }
            }
            for (var i = 0; i < this.gridStockStrategyList.Rows.Count; i++)
            {
                var row = this.gridStockStrategyList.Rows[i];
                var value = ObjectUtil.ToValue<string>(row.Cells["执行结果"].Value, "");
                if (value == "成功")
                {
                    this.gridStockStrategyList.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                }
                else if(value == "失败")
                {
                    this.gridStockStrategyList.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    this.gridStockStrategyList.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                }
            }
        }

        void LoadRemindList(string stockCode)
        {
            var reminds = Repository.QueryAll<RemindEntity>($"StockCode='{stockCode}'", "ID desc", 60);
            var dt = ObjectUtil.ConvertTable(reminds, true);
            this.gridRemindList.DataSource = null;
            this.gridRemindList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridRemindList.ColumnCount; i++)
            {
                this.gridRemindList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                var columnName = this.gridRemindList.Columns[i].Name;
                if (columnName == "股票代码") this.gridRemindList.Columns[i].Visible = false;
                else
                {
                    this.gridRemindList.Columns[i].Width = ObjectUtil.GetGridColumnLength(columnName);
                }
            }
            for (var i = 0; i < this.gridRemindList.Rows.Count; i++)
            {
                var row = this.gridRemindList.Rows[i];
                var value = ObjectUtil.ToValue<string>(row.Cells["是否执行"].Value, "");
                if (value == "✔")
                {
                    this.gridRemindList.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                }
                else
                {
                    this.gridRemindList.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                }
            }
        }

        void LoadExchangeList(string stockCode)
        {
            var exchangeOrders = Repository.QueryAll<ExchangeOrderEntity>($"StockCode='{stockCode}'", "ID desc", 60);
            var dt = ObjectUtil.ConvertTable(exchangeOrders);
            this.gridExchangeList.DataSource = null;
            this.gridExchangeList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridExchangeList.ColumnCount; i++)
            {
                this.gridExchangeList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                var columnName = this.gridExchangeList.Columns[i].Name;
                if (columnName == "股票代码") this.gridExchangeList.Columns[i].Visible = false;
                else
                {
                    this.gridExchangeList.Columns[i].Width = ObjectUtil.GetGridColumnLength(columnName);
                }
            }
        }

        void LoadReportList(string stockCode)
        {
            var reports = Repository.QueryAll<ReportEntity>($"StockCode='{stockCode}'", "PublishDate desc", 60);
            var dt = ObjectUtil.ConvertTable(reports);
            this.gridReportList.DataSource = null;
            this.gridReportList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridReportList.ColumnCount; i++)
            {
                this.gridReportList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                var columnName = this.gridReportList.Columns[i].Name;
                if (columnName == "股票代码") this.gridReportList.Columns[i].Visible = false;
                else
                {
                    if (columnName == "研报标题") this.gridReportList.Columns[i].Width = 250;
                    else this.gridReportList.Columns[i].Width = ObjectUtil.GetGridColumnLength(columnName);
                }
            }
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

        /// <summary>
        /// 买卖策略列表点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void tabControlBottom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            var stockName = $"{selectRow.Cells["股票名称"].Value}";

            this.LoadTabGridList(this.tabControlBottom.SelectedIndex, stockCode, stockName);
        }

        void LoadTabGridList(int tabIndex, string stockCode, string stockName)
        {
            switch (tabIndex)
            {
                case 1:
                    this.LoadPriceChart(stockCode, stockName, 1);
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
                    this.LoadReportList(stockCode);
                    break;
            }
        }


        private void btnDayChart_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            var stockName = $"{selectRow.Cells["股票名称"].Value}";

            this.LoadPriceChart(stockCode, stockName, 0);
        }

        private void btnWebChart_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            ObjectUtil.OpenBrowserUrl($"http://quote.eastmoney.com/concept/{stockCode}.html?from=classic");
        }

        private void btnMinuteChart_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            var stockName = $"{selectRow.Cells["股票名称"].Value}";
            var chartWithZS = this.txtChartWithZS.Checked;
            this.LoadPriceChart(stockCode, stockName, 1, chartWithZS);
        }

        private void btnHandleRemind_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            if (this.gridRemindList.SelectedRows.Count == 0) return;
            
            if (MessageBox.Show($"确认要标记执行选中的提醒数据?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

            var ids = new List<int>();
            for (var i = 0; i < this.gridRemindList.SelectedRows.Count; i++)
            {
                var row = this.gridRemindList.SelectedRows[i];
                var id = ObjectUtil.ToValue<int>(row.Cells["序"].Value, 0);
                if (id > 0) ids.Add(id);
            }
            if (ids.Count > 0)
            {
                StockRemindService.Mark(stockCode, ids.ToArray());
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

            var ids = new List<int>();
            for (var i = 0; i < this.gridRemindList.SelectedRows.Count; i++)
            {
                var row = this.gridRemindList.SelectedRows[i];
                var id = ObjectUtil.ToValue<int>(row.Cells["序"].Value, 0);
                if (id > 0) ids.Add(id);
            }
            if (ids.Count > 0)
            {
                StockRemindService.Cancel(stockCode, ids.ToArray());
            }
            this.LoadRemindList(stockCode);
        }

        private void btnRunStrategy_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            if (this.gridStockStrategyList.SelectedRows.Count == 0) return;

            if (MessageBox.Show($"确认要标记选中的策略数据?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

            var ids = new List<int>();
            for (var i = 0; i < this.gridStockStrategyList.SelectedRows.Count; i++)
            {
                var row = this.gridStockStrategyList.SelectedRows[i];
                var id = ObjectUtil.ToValue<int>(row.Cells["序"].Value, 0);
                if (id > 0) ids.Add(id);

            }
            if (ids.Count > 0)
            {
                StockStrategyService.Mark(stockCode, ids.ToArray());
            }
            this.LoadStockStrategyList(stockCode);
        }

        private void btnCancelStrategy_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            if (this.gridStockStrategyList.SelectedRows.Count == 0) return;

            if (MessageBox.Show($"确认要取消选中的策略数据?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

            var ids = new List<int>();
            for (var i = 0; i < this.gridStockStrategyList.SelectedRows.Count; i++)
            {
                var row = this.gridStockStrategyList.SelectedRows[i];
                var id = ObjectUtil.ToValue<int>(row.Cells["序"].Value, 0);
                if (id > 0) ids.Add(id);

            }
            if (ids.Count > 0)
            {
                StockStrategyService.Cancel(stockCode, ids.ToArray());
            }
            this.LoadStockStrategyList(stockCode);
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

        private void btnOpenBrower_Click(object sender, EventArgs e)
        {
            if (this.gridReportList.SelectedRows.Count == 0) return;
            var selectRow = this.gridReportList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            var pdfCode = $"{selectRow.Cells["编号"].Value}";

            var report = Repository.QueryFirst<ReportEntity>($"StockCode='{stockCode}' and PdfCode='{pdfCode}'");
            if (report == null) return;

            ObjectUtil.OpenBrowserUrl(report.PdfUrl);
        }

        #endregion

        #region 任务栏托盘事件
        private void DesktopFrom_SizeChanged(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
        }

        private void tmpExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tmpDesktop_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void tmpActionLog_Click(object sender, EventArgs e)
        {
            var frm = new MessageForm();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
        }

        private void tmpConfig_Click(object sender, EventArgs e)
        {
            var frm = new ConfigForm();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
        }

        #endregion

        #region 操作日志事件

        public void ActionLog(string message)
        {
            Action act = delegate ()
            {
                this.ShowMessage(message);

                this.txtActionLog.AppendText($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}\n");
                this.txtActionLog.SelectionStart = this.txtActionLog.Text.Length;
                this.txtActionLog.ScrollToCaret();

                this.notifyIcon1.ShowBalloonTip(RC.RemindMessageShowTime, "消息通知", message, ToolTipIcon.Info);
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

        private void DesktopFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

    }
}
