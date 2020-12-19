using StockSimulateCore.Config;
using StockSimulateCore.Entity;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Service
{
    public class StockRemindService
    {
        public static void Create(string accountName, string strUPPer)
        {
            var account = SQLiteDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{accountName}'");
            if (account == null) return;

            SQLiteDBUtil.Instance.Delete<RemindEntity>($"StrategyName='批量设置' and RType=0");

            var stocks = SQLiteDBUtil.Instance.QueryAll<StockEntity>($"");
            foreach(var stock in stocks)
            {
                Create(account, stock.Code, strUPPer, null, null, true);
            }
        }

        public static void Create(string accountName, string stockCode, string strUPPer, string strUPPrice, string strDOWNPrice, bool batchCreate = false)
        {
            var account = SQLiteDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{accountName}'");
            if (account == null) return;

            Create(account, stockCode, strUPPer, strUPPrice, strDOWNPrice, batchCreate);
        }

        public static void Create(AccountEntity account, string stockCode, string strUPPer, string strUPPrice, string strDOWNPrice, bool batchCreate = false)
        {
            var reminds = new List<RemindEntity>();
            if (!string.IsNullOrEmpty(strUPPer))
            {
                if (!batchCreate)
                {
                    SQLiteDBUtil.Instance.Delete<RemindEntity>($"StockCode='{stockCode}' and StrategyName='主动设置' and RType=0");
                }
                var udPers = ObjectUtil.GetSplitArray(strUPPer, ",");
                foreach (var udPer in udPers)
                {
                    var target = ObjectUtil.ToValue<decimal>(udPer, 0);
                    if (target == 0) continue;

                    var remind = new RemindEntity()
                    {
                        StockCode = stockCode,
                        Target = target,
                        Email = account.Email,
                        QQ = account.QQ,
                        RType = 0,
                        StrategyName = batchCreate ? "批量设置" : "主动设置",
                        StrategyTarget = $"波动{target}%"
                    };
                    reminds.Add(remind);
                }
            }
            if (!string.IsNullOrEmpty(strUPPrice))
            {
                SQLiteDBUtil.Instance.Delete<RemindEntity>($"StockCode='{stockCode}' and StrategyName='主动设置' and RType=1");

                var upPrices = ObjectUtil.GetSplitArray(strUPPrice, ",");
                foreach (var upPrice in upPrices)
                {
                    var target = ObjectUtil.ToValue<decimal>(upPrice, 0);
                    if (target == 0) continue;

                    var remind = new RemindEntity()
                    {
                        StockCode = stockCode,
                        Target = target,
                        Email = account.Email,
                        QQ = account.QQ,
                        RType = 1,
                        StrategyName = "主动设置",
                        StrategyTarget = $"上涨至{target}"
                    };
                    remind.MaxPrice = Math.Round(remind.Target * (1 + RunningConfig.Instance.RemindStockPriceFloatPer / 100m), 2);
                    remind.MinPrice = Math.Round(remind.Target * (1 - RunningConfig.Instance.RemindStockPriceFloatPer / 100m), 2);

                    reminds.Add(remind);
                }
            }
            if (!string.IsNullOrEmpty(strDOWNPrice))
            {
                SQLiteDBUtil.Instance.Delete<RemindEntity>($"StockCode='{stockCode}' and StrategyName='主动设置' and RType=2");

                var downPrices = ObjectUtil.GetSplitArray(strDOWNPrice, ",");
                foreach (var downPrice in downPrices)
                {
                    var target = ObjectUtil.ToValue<decimal>(downPrice, 0);
                    if (target == 0) continue;

                    var remind = new RemindEntity()
                    {
                        StockCode = stockCode,
                        Target = target,
                        Email = account.Email,
                        QQ = account.QQ,
                        RType = 2,
                        StrategyName = "主动设置",
                        StrategyTarget = $"下跌至{target}"
                    };
                    remind.MaxPrice = Math.Round(remind.Target * (1 + RunningConfig.Instance.RemindStockPriceFloatPer / 100m), 2);
                    remind.MinPrice = Math.Round(remind.Target * (1 - RunningConfig.Instance.RemindStockPriceFloatPer / 100m), 2);
                    reminds.Add(remind);
                }
            }

            SQLiteDBUtil.Instance.Insert<RemindEntity>(reminds.ToArray());
        }
        public static void AutoCreate(string accountName)
        {
            var account = SQLiteDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{accountName}'");
            if (account == null) return;

            var reminds = new List<RemindEntity>();
            var stocks = SQLiteDBUtil.Instance.QueryAll<StockEntity>($"Type=0 and Safety > 0");
            foreach (var stock in stocks)
            {
                var remind = new RemindEntity()
                {
                    StockCode = stock.Code,
                    Target = stock.Safety,
                    Email = account.Email,
                    QQ = account.QQ,
                    RType = 2,
                    StrategyName = "自动设置",
                    StrategyTarget = $"下跌至{stock.Safety}"
                };
                remind.MaxPrice = Math.Round(remind.Target * (1 + RunningConfig.Instance.RemindStockPriceFloatPer / 100m), 2);
                remind.MinPrice = Math.Round(remind.Target * (1 - RunningConfig.Instance.RemindStockPriceFloatPer / 100m), 2);
                reminds.Add(remind);
            }
            SQLiteDBUtil.Instance.Delete<RemindEntity>($"StrategyName='自动设置' and RType=2");
            SQLiteDBUtil.Instance.Insert<RemindEntity>(reminds.ToArray());
        }

        public static void Handle(string stockCode, int rType, decimal target)
        {
            var remind = SQLiteDBUtil.Instance.QueryFirst<RemindEntity>($"StockCode='{stockCode}' and RType={rType} and Target={target} and Handled='False'");
            if (remind != null)
            {
                remind.Handled = true;
                SQLiteDBUtil.Instance.Update<RemindEntity>(remind);
            }
        }

        public static void Cancel(string stockCode, int rType, decimal target)
        {
            var remind = SQLiteDBUtil.Instance.QueryFirst<RemindEntity>($"StockCode='{stockCode}' and RType={rType} and Target={target}");
            if (remind != null)
            {
                remind.Handled = true;
                SQLiteDBUtil.Instance.Delete<RemindEntity>(remind);
            }
        }
    }
}
