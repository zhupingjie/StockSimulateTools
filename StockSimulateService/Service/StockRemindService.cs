using StockSimulateDomain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateService.Utils;
using StockSimulateService.Config;

namespace StockSimulateService.Service
{
    public class StockRemindService
    {
        public static void Create(string accountName, string strUPPer)
        {
            var account = MySQLDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{accountName}'");
            if (account == null) return;

            MySQLDBUtil.Instance.Delete<RemindEntity>($"StrategyName='批量设置' and RType=0");

            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>($"");
            foreach(var stock in stocks)
            {
                Create(account, stock, strUPPer, null, null, null, null, true);
            }
        }

        public static void Create(string accountName, string stockCode, string strUPPer, string strUPPrice, string strDOWNPrice, string strUPAveragePrice, string strDOWNAveragePrice, bool batchCreate = false)
        {
            var account = MySQLDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{accountName}'");
            if (account == null) return;

            var stock = MySQLDBUtil.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null) return;

            Create(account, stock, strUPPer, strUPPrice, strDOWNPrice, strUPAveragePrice, strDOWNAveragePrice, batchCreate);
        }

        public static void Create(AccountEntity account, StockEntity stock, string strUPPer, string strUPPrice, string strDOWNPrice, string strUPAveragePrice, string strDOWNAveragePrice, bool batchCreate = false)
        {
            var decNum = stock.Type == 0 ? 2 : 3;

            var reminds = new List<RemindEntity>();
            if (!string.IsNullOrEmpty(strUPPer))
            {
                if (!batchCreate)
                {
                    MySQLDBUtil.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=0");
                }
                var udPers = ObjectUtil.GetSplitArray(strUPPer, ",");
                foreach (var udPer in udPers)
                {
                    var target = ObjectUtil.ToValue<decimal>(udPer, 0);
                    if (target == 0) continue;

                    var remind = new RemindEntity()
                    {
                        StockCode = stock.Code,
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
                MySQLDBUtil.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=1");

                var upPrices = ObjectUtil.GetSplitArray(strUPPrice, ",");
                foreach (var upPrice in upPrices)
                {
                    var target = ObjectUtil.ToValue<decimal>(upPrice, 0);
                    if (target == 0) continue;

                    var remind = new RemindEntity()
                    {
                        StockCode = stock.Code,
                        Target = target,
                        Email = account.Email,
                        QQ = account.QQ,
                        RType = 1,
                        StrategyName = "主动设置",
                        StrategyTarget = $"上涨至{target}"
                    };
                    remind.MaxPrice = Math.Round(remind.Target * (1 + RunningConfig.Instance.RemindStockPriceFloatPer / 100m), decNum);
                    remind.MinPrice = Math.Round(remind.Target * (1 - RunningConfig.Instance.RemindStockPriceFloatPer / 100m), decNum);

                    reminds.Add(remind);
                }
            }
            if (!string.IsNullOrEmpty(strDOWNPrice))
            {
                MySQLDBUtil.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=2");

                var downPrices = ObjectUtil.GetSplitArray(strDOWNPrice, ",");
                foreach (var downPrice in downPrices)
                {
                    var target = ObjectUtil.ToValue<decimal>(downPrice, 0);
                    if (target == 0) continue;

                    var remind = new RemindEntity()
                    {
                        StockCode = stock.Code,
                        Target = target,
                        Email = account.Email,
                        QQ = account.QQ,
                        RType = 2,
                        StrategyName = "主动设置",
                        StrategyTarget = $"下跌至{target}"
                    };
                    remind.MaxPrice = Math.Round(remind.Target * (1 + RunningConfig.Instance.RemindStockPriceFloatPer / 100m), decNum);
                    remind.MinPrice = Math.Round(remind.Target * (1 - RunningConfig.Instance.RemindStockPriceFloatPer / 100m), decNum);
                    reminds.Add(remind);
                }
            }
            if (!string.IsNullOrEmpty(strUPAveragePrice))
            {
                var averagePrices = ObjectUtil.GetSplitArray(strUPAveragePrice, ",");
                foreach (var avg in averagePrices)
                {
                    var target = ObjectUtil.ToValue<decimal>(avg, 0);
                    if (target == 0) continue;

                    var remind = new RemindEntity()
                    {
                        StockCode = stock.Code,
                        Target = target,
                        Email = account.Email,
                        QQ = account.QQ,
                        RType = 3,
                        StrategyName = "主动设置",
                        StrategyTarget = $"突破{target}日均线"
                    };
                    reminds.Add(remind);
                }
            }
            if (!string.IsNullOrEmpty(strDOWNAveragePrice))
            {
                var averagePrices = ObjectUtil.GetSplitArray(strDOWNAveragePrice, ",");
                foreach (var avg in averagePrices)
                {
                    var target = ObjectUtil.ToValue<decimal>(avg, 0);
                    if (target == 0) continue;

                    var remind = new RemindEntity()
                    {
                        StockCode = stock.Code,
                        Target = target,
                        Email = account.Email,
                        QQ = account.QQ,
                        RType = 4,
                        StrategyName = "主动设置",
                        StrategyTarget = $"跌破{target}日均线"
                    };
                    reminds.Add(remind);
                }
            }
            MySQLDBUtil.Instance.Insert<RemindEntity>(reminds.ToArray());
        }
        public static void AutoCreate(string accountName)
        {
            var account = MySQLDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{accountName}'");
            if (account == null) return;

            var reminds = new List<RemindEntity>();
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>($"Type=0 and Safety > 0");
            foreach (var stock in stocks)
            {
                var decNum = stock.Type == 0 ? 2 : 3;

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
                remind.MaxPrice = Math.Round(remind.Target * (1 + RunningConfig.Instance.RemindStockPriceFloatPer / 100m), decNum);
                remind.MinPrice = Math.Round(remind.Target * (1 - RunningConfig.Instance.RemindStockPriceFloatPer / 100m), decNum);
                reminds.Add(remind);
            }
            MySQLDBUtil.Instance.Delete<RemindEntity>($"StrategyName='自动设置' and RType=2");
            MySQLDBUtil.Instance.Insert<RemindEntity>(reminds.ToArray());
        }

        public static void Mark(string stockCode, int rType, decimal target)
        {
            var remind = MySQLDBUtil.Instance.QueryFirst<RemindEntity>($"StockCode='{stockCode}' and RType={rType} and Target={target} and Handled='False'");
            if (remind != null)
            {
                remind.Handled = 1;
                MySQLDBUtil.Instance.Update<RemindEntity>(remind);
            }
        }

        public static void Cancel(string stockCode, int rType, decimal target)
        {
            var remind = MySQLDBUtil.Instance.QueryFirst<RemindEntity>($"StockCode='{stockCode}' and RType={rType} and Target={target}");
            if (remind != null)
            {
                remind.Handled = 1;
                MySQLDBUtil.Instance.Delete<RemindEntity>(remind);
            }
        }

        public static void CheckAutoRun(Action<string> action)
        {
            var reminds = MySQLDBUtil.Instance.QueryAll<RemindEntity>($"Handled=0");
            var stockCodes = reminds.Select(c => c.StockCode).Distinct().ToArray();
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>($"Code in ('{string.Join("','", stockCodes)}') and Price>0 and LastDate>='{DateTime.Now.Date.ToString("yyyy-MM-dd")}'");

            foreach (var stock in stocks)
            {
                var remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 0 && Math.Abs(stock.UDPer) > c.Target && c.Handled == 0 && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date));
                if (remind != null)
                {
                    remind.Handled = 1;
                    remind.LastRemind = DateTime.Now;
                    remind.RemindPrice = stock.Price;
                    MySQLDBUtil.Instance.Update<RemindEntity>(remind);

                    var nextRemind = new RemindEntity()
                    {
                        RType = 0,
                        StockCode = remind.StockCode,
                        Target = remind.Target,
                        Email = remind.Email,
                        QQ = remind.QQ,
                        StrategyName = remind.StrategyName,
                        StrategyTarget = remind.StrategyTarget,
                        PlanRemind = DateTime.Now.Date.AddDays(1)
                    };
                    MySQLDBUtil.Instance.Insert<RemindEntity>(nextRemind);

                    var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已{(stock.UDPer > 0 ? "上涨" : "下跌")}超过幅度[{remind.Target}%],请注意!";

                    MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                    action(message);
                }

                remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 1 && c.Target <= stock.Price && c.Handled == 0);
                if (remind != null)
                {
                    var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已上涨高于股价[{remind.Target}],请注意!";

                    MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                    action(message);

                    remind.Handled = 1;
                    remind.LastRemind = DateTime.Now;
                    remind.RemindPrice = stock.Price;
                    MySQLDBUtil.Instance.Update<RemindEntity>(remind);
                }

                remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 2 && c.Target >= stock.Price && c.Handled == 0);
                if (remind != null)
                {
                    var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已下跌低于股价[{remind.Target}],请注意!";

                    MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                    action(message);

                    remind.Handled = 1;
                    remind.LastRemind = DateTime.Now;
                    remind.RemindPrice = stock.Price;
                    MySQLDBUtil.Instance.Update<RemindEntity>(remind);
                }

                remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 8 && c.MaxPrice >= stock.Price && c.Handled == 0 && (!c.LastRemind.HasValue || c.LastRemind < DateTime.Now.Date));
                if (remind != null)
                {
                    var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已达成买卖点[{remind.StrategyTarget} ({remind.MinPrice}-{remind.MaxPrice})],请注意!";

                    MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                    action(message);

                    remind.LastRemind = DateTime.Now;
                    remind.RemindPrice = stock.Price;
                    MySQLDBUtil.Instance.Update<RemindEntity>(remind);
                }

                remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 9 && c.MinPrice <= stock.Price && c.Handled == 0 && (!c.LastRemind.HasValue || c.LastRemind < DateTime.Now.Date));
                if (remind != null)
                {
                    var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已达成买卖点[{remind.StrategyTarget} ({remind.MinPrice}-{remind.MaxPrice})],请注意!";

                    MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                    action(message);

                    remind.LastRemind = DateTime.Now;
                    remind.RemindPrice = stock.Price;
                    MySQLDBUtil.Instance.Update<RemindEntity>(remind);
                }
            }
        }
    }
}
