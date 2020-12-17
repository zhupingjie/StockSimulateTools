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
                if (stockInfo == null) return;
                if (stockInfo.DayPrice.Price == 0) return;

                stockInfo.Stock.ID = stock.ID;
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
                var dealTime = DateTime.Now.ToString("HH:mm");
                if (dealTime.CompareTo("15:00") >= 0) dealTime = "15:00";

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
                        SQLiteDBUtil.Instance.Update<StockStrategyEntity>(item);
                    }
                }
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
                    SQLiteDBUtil.Instance.Update<AccountStockEntity>(item);

                    actionLog($"已计算[{item.StockName}]持有股价盈亏...[{item.Profit}] [{item.UDPer}%]");
                }
            }
            if (stocks.Length > 0) actionLog($">------------------------------------------------>");
        }

        /// <summary>
        /// 采集自选股财务数据
        /// </summary>
        /// <param name="actionLog"></param>
        public static void GatherFinanceData(Action<string> actionLog)
        {
            var stocks = SQLiteDBUtil.Instance.QueryAll<StockEntity>();
            foreach (var stock in stocks)
            {
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
            }
            if (stocks.Length > 0) actionLog($">------------------------------------------------>");
        }
    }
}
