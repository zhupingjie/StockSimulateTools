using StockSimulateDomain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateService.Utils;
using StockSimulateService.Config;
using StockSimulateDomain.Model;

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
                Create(account, stock, new RemindInfo()
                {
                    UDPer = strUPPer
                }, true);
            }
        }

        public static void Create(RemindInfo remindInfo, bool batchCreate = false)
        {
            var account = MySQLDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{remindInfo.AccountName}'");
            if (account == null) return;

            var stock = MySQLDBUtil.Instance.QueryFirst<StockEntity>($"Code='{remindInfo.StockCode}'");
            if (stock == null) return;

            Create(account, stock, remindInfo, batchCreate);
        }

        public static void Create(AccountEntity account, StockEntity stock, RemindInfo remindInfo, bool batchCreate = false)
        {
            var decNum = stock.Type == 0 ? 2 : 3;

            var reminds = new List<RemindEntity>();
            if (!string.IsNullOrEmpty(remindInfo.UDPer))
            {
                if (!batchCreate)
                {
                    MySQLDBUtil.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=0");
                }
                var udPers = ObjectUtil.GetSplitArray(remindInfo.UDPer, ",");
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
            if (!string.IsNullOrEmpty(remindInfo.UpPrice))
            {
                MySQLDBUtil.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=1");

                var upPrices = ObjectUtil.GetSplitArray(remindInfo.UpPrice, ",");
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
            if (!string.IsNullOrEmpty(remindInfo.DownPrice))
            {
                MySQLDBUtil.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=2");

                var downPrices = ObjectUtil.GetSplitArray(remindInfo.DownPrice, ",");
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
            if (!string.IsNullOrEmpty(remindInfo.UpAveragePrice))
            {
                var averagePrices = ObjectUtil.GetSplitArray(remindInfo.UpAveragePrice, ",");
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
                        StrategyTarget = $"突破{target}均线"
                    };
                    reminds.Add(remind);
                }
            }

            if (!string.IsNullOrEmpty(remindInfo.DownAveragePrice))
            {
                var averagePrices = ObjectUtil.GetSplitArray(remindInfo.DownAveragePrice, ",");
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
                        StrategyTarget = $"跌破{target}均线"
                    };
                    reminds.Add(remind);
                }
            }
            if (!string.IsNullOrEmpty(remindInfo.UpAveragePriceReverse))
            {
                var averagePrices = ObjectUtil.GetSplitArray(remindInfo.UpAveragePriceReverse, ",");
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
                        RType = 5,
                        StrategyName = "主动设置",
                        StrategyTarget = $"反转{target}均线"
                    };
                    reminds.Add(remind);
                }
            }
            if (!string.IsNullOrEmpty(remindInfo.DownAveragePriceReverse))
            {
                var averagePrices = ObjectUtil.GetSplitArray(remindInfo.DownAveragePriceReverse, ",");
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
                        RType = 6,
                        StrategyName = "主动设置",
                        StrategyTarget = $"逆转{target}日均线"
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

        public static void Mark(string stockCode, int[] ids)
        {
            var reminds = MySQLDBUtil.Instance.QueryAll<RemindEntity>($"StockCode='{stockCode}' and Handled=0 and ID in ({string.Join(",", ids)})");
            foreach (var remind in reminds)
            {
                remind.Handled = 1;
            }
            MySQLDBUtil.Instance.Delete<RemindEntity>(reminds);
        }

        public static void Cancel(string stockCode, int[] ids)
        {
            MySQLDBUtil.Instance.Delete<RemindEntity>($"StockCode='{stockCode}'  and ID in ({string.Join(",", ids)})");
        }

        public static void CheckAutoRun(Action<string> action)
        {
            var reminds = MySQLDBUtil.Instance.QueryAll<RemindEntity>($"Handled=0");
            var stockCodes = reminds.Select(c => c.StockCode).Distinct().ToArray();
            var stocks = MySQLDBUtil.Instance.QueryAll<StockEntity>($"Code in ('{string.Join("','", stockCodes)}') and Price>0 and LastDate>='{DateTime.Now.Date.ToString("yyyy-MM-dd")}'");

            foreach (var stock in stocks)
            {
                var remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 0 && Math.Abs(stock.UDPer) > c.Target && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date));
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

                    if (RunningConfig.Instance.RemindNoticeByEmail)
                    {
                        MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                    }
                    if(RunningConfig.Instance.RemindNoticeByMessage)
                    {
                        StockMessageService.SendMessage(stock, message);
                    }
                    action(message);
                }

                remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 1 && c.Target <= stock.Price);
                if (remind != null)
                {
                    var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已上涨高于股价[{remind.Target}],请注意!";

                    if (RunningConfig.Instance.RemindNoticeByEmail)
                    {
                        MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                    }
                    if (RunningConfig.Instance.RemindNoticeByMessage)
                    {
                        StockMessageService.SendMessage(stock, message);
                    }
                    action(message);

                    remind.Handled = 1;
                    remind.LastRemind = DateTime.Now;
                    remind.RemindPrice = stock.Price;
                    MySQLDBUtil.Instance.Update<RemindEntity>(remind);
                }

                remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 2 && c.Target >= stock.Price);
                if (remind != null)
                {
                    var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已下跌低于股价[{remind.Target}],请注意!";

                    if (RunningConfig.Instance.RemindNoticeByEmail)
                    {
                        MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                    }
                    if (RunningConfig.Instance.RemindNoticeByMessage)
                    {
                        StockMessageService.SendMessage(stock, message);
                    }
                    action(message);

                    remind.Handled = 1;
                    remind.LastRemind = DateTime.Now;
                    remind.RemindPrice = stock.Price;
                    MySQLDBUtil.Instance.Update<RemindEntity>(remind);
                }

                var rds = reminds.Where(c => c.StockCode == stock.Code && c.RType == 3 && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date)).ToArray();
                foreach (var rd in rds)
                {
                    if ((rd.Target == 5 && stock.Price > stock.AvgPrice5 && stock.AvgPrice5 > 0)
                        || (rd.Target == 10 && stock.Price > stock.AvgPrice10 && stock.AvgPrice10 > 0)
                        || (rd.Target == 20 && stock.Price > stock.AvgPrice20 && stock.AvgPrice20 > 0)
                        || (rd.Target == 60 && stock.Price > stock.AvgPrice60 && stock.AvgPrice60 > 0)
                        || (rd.Target == 120 && stock.Price > stock.AvgPrice120 && stock.AvgPrice120 > 0)
                        || (rd.Target == 250 && stock.Price > stock.AvgPrice250 && stock.AvgPrice250 > 0))
                    {
                        var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已突破[{remind.Target}]日均线,请注意!";

                        if (RunningConfig.Instance.RemindNoticeByEmail)
                        {
                            MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                        }
                        if (RunningConfig.Instance.RemindNoticeByMessage)
                        {
                            StockMessageService.SendMessage(stock, message);
                        }
                        action(message);

                        remind.Handled = 1;
                        remind.LastRemind = DateTime.Now;
                        remind.RemindPrice = stock.Price;
                        MySQLDBUtil.Instance.Update<RemindEntity>(remind);
                    }
                }

                rds = reminds.Where(c => c.StockCode == stock.Code && c.RType == 4 && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date)).ToArray();
                foreach (var rd in rds)
                {
                    if ((rd.Target == 5 && stock.Price < stock.AvgPrice5 && stock.AvgPrice5 > 0)
                        || (rd.Target == 10 && stock.Price < stock.AvgPrice10 && stock.AvgPrice10 > 0)
                        || (rd.Target == 20 && stock.Price < stock.AvgPrice20 && stock.AvgPrice20 > 0)
                        || (rd.Target == 60 && stock.Price < stock.AvgPrice60 && stock.AvgPrice60 > 0)
                        || (rd.Target == 120 && stock.Price < stock.AvgPrice120 && stock.AvgPrice120 > 0)
                        || (rd.Target == 250 && stock.Price < stock.AvgPrice250 && stock.AvgPrice250 > 0))
                    {
                        var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已跌破[{remind.Target}]日均线,请注意!";

                        if (RunningConfig.Instance.RemindNoticeByEmail)
                        {
                            MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                        }
                        if (RunningConfig.Instance.RemindNoticeByMessage)
                        {
                            StockMessageService.SendMessage(stock, message);
                        }
                        action(message);

                        remind.Handled = 1;
                        remind.LastRemind = DateTime.Now;
                        remind.RemindPrice = stock.Price;
                        MySQLDBUtil.Instance.Update<RemindEntity>(remind);
                    }
                }

                rds = reminds.Where(c => c.StockCode == stock.Code && c.RType == 5 && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date)).ToArray();
                foreach (var rd in rds)
                {
                    if ((rd.Target == 5 && ObjectUtil.CheckTrendReverse(stock.Trend5, true))
                        || (rd.Target == 10 && ObjectUtil.CheckTrendReverse(stock.Trend10, true))
                        || (rd.Target == 20 && ObjectUtil.CheckTrendReverse(stock.Trend20, true))
                        || (rd.Target == 60 && ObjectUtil.CheckTrendReverse(stock.Trend60, true))
                        || (rd.Target == 120 && ObjectUtil.CheckTrendReverse(stock.Trend120, true))
                        || (rd.Target == 250 && ObjectUtil.CheckTrendReverse(stock.Trend250, true)))
                    {
                        var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已反转[{remind.Target}]日均线,请注意!";

                        if (RunningConfig.Instance.RemindNoticeByEmail)
                        {
                            MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                        }
                        if (RunningConfig.Instance.RemindNoticeByMessage)
                        {
                            StockMessageService.SendMessage(stock, message);
                        }
                        action(message);

                        remind.Handled = 1;
                        remind.LastRemind = DateTime.Now;
                        remind.RemindPrice = stock.Price;
                        MySQLDBUtil.Instance.Update<RemindEntity>(remind);
                    }
                }

                rds = reminds.Where(c => c.StockCode == stock.Code && c.RType == 6 && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date)).ToArray();
                foreach (var rd in rds)
                {
                    if ((rd.Target == 5 && ObjectUtil.CheckTrendReverse(stock.Trend5, false))
                        || (rd.Target == 10 && ObjectUtil.CheckTrendReverse(stock.Trend10, false))
                        || (rd.Target == 20 && ObjectUtil.CheckTrendReverse(stock.Trend20, false))
                        || (rd.Target == 60 && ObjectUtil.CheckTrendReverse(stock.Trend60, false))
                        || (rd.Target == 120 && ObjectUtil.CheckTrendReverse(stock.Trend120, false))
                        || (rd.Target == 250 && ObjectUtil.CheckTrendReverse(stock.Trend250, false)))
                    {
                        var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已逆转[{remind.Target}]日均线,请注意!";

                        if (RunningConfig.Instance.RemindNoticeByEmail)
                        {
                            MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                        }
                        if (RunningConfig.Instance.RemindNoticeByMessage)
                        {
                            StockMessageService.SendMessage(stock, message);
                        }
                        action(message);

                        remind.Handled = 1;
                        remind.LastRemind = DateTime.Now;
                        remind.RemindPrice = stock.Price;
                        MySQLDBUtil.Instance.Update<RemindEntity>(remind);
                    }
                }

                remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 8 && c.MaxPrice >= stock.Price && c.Handled == 0 && (!c.LastRemind.HasValue || c.LastRemind < DateTime.Now.Date));
                if (remind != null)
                {
                    var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已达成买卖点[{remind.StrategyTarget} ({remind.MinPrice}-{remind.MaxPrice})],请注意!";

                    if (RunningConfig.Instance.RemindNoticeByEmail)
                    {
                        MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                    }
                    if (RunningConfig.Instance.RemindNoticeByMessage)
                    {
                        StockMessageService.SendMessage(stock, message);
                    }
                    action(message);

                    remind.LastRemind = DateTime.Now;
                    remind.RemindPrice = stock.Price;
                    MySQLDBUtil.Instance.Update<RemindEntity>(remind);
                }

                remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 9 && c.MinPrice <= stock.Price && c.Handled == 0 && (!c.LastRemind.HasValue || c.LastRemind < DateTime.Now.Date));
                if (remind != null)
                {
                    var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已达成买卖点[{remind.StrategyTarget} ({remind.MinPrice}-{remind.MaxPrice})],请注意!";

                    if (RunningConfig.Instance.RemindNoticeByEmail)
                    {
                        MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                    }
                    if (RunningConfig.Instance.RemindNoticeByMessage)
                    {
                        StockMessageService.SendMessage(stock, message);
                    }
                    action(message);

                    remind.LastRemind = DateTime.Now;
                    remind.RemindPrice = stock.Price;
                    MySQLDBUtil.Instance.Update<RemindEntity>(remind);
                }
            }
        }
    }
}
