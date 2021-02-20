using StockSimulateDomain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateCore.Utils;
using StockSimulateCore.Config;
using StockSimulateDomain.Model;
using StockSimulateCore.Data;

namespace StockSimulateService.Service
{
    public class StockRemindService
    {
        public static void Create(RemindInfo remindInfo)
        {
            var account = Repository.Instance.QueryFirst<AccountEntity>($"Name='{remindInfo.AccountName}'");
            if (account == null) return;

            if (string.IsNullOrEmpty(remindInfo.StockCode))
            {
                var stocks = Repository.Instance.QueryAll<StockEntity>($"Foucs>0");
                foreach (var stock in stocks)
                {
                    remindInfo.StockCode = stock.Code;

                    Create(account, stock, remindInfo);
                }
            }
            else
            {
                var stock = Repository.Instance.QueryFirst<StockEntity>($"Code='{remindInfo.StockCode}'");
                if (stock == null) return;
                if (stock.Foucs == 0)
                {
                    StockService.Foucs(stock.Code);
                }
                Create(account, stock, remindInfo);
            }
        }

        public static void Create(AccountEntity account, StockEntity stock, RemindInfo remindInfo)
        {
            var decNum = stock.Type == 0 ? 2 : 3;

            var reminds = new List<RemindEntity>();
            if (!string.IsNullOrEmpty(remindInfo.UDPer))
            {
                Repository.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=0");

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
                        StrategyName = "主动设置",
                        StrategyTarget = $"波动{target}%"
                    };
                    reminds.Add(remind);
                }
            }
            if (!string.IsNullOrEmpty(remindInfo.UpPrice))
            {
                Repository.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=1");

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
                Repository.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=2");

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
                Repository.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=3");

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
                Repository.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=4");

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
            if (!string.IsNullOrEmpty(remindInfo.UpMinDayPricePer))
            {
                Repository.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=5");

                var averagePrices = ObjectUtil.GetSplitArray(remindInfo.UpMinDayPricePer, ",");
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
                        StrategyTarget = $"低点反弹{target}%"
                    };
                    reminds.Add(remind);
                }
            }
            if (!string.IsNullOrEmpty(remindInfo.DownMaxDayPricePer))
            {
                Repository.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=6");

                var averagePrices = ObjectUtil.GetSplitArray(remindInfo.DownMaxDayPricePer, ",");
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
                        StrategyTarget = $"高点回落{target}%"
                    };
                    reminds.Add(remind);
                }
            }

            if(remindInfo.GoldMacd)
            {
                Repository.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=10");

                var remind = new RemindEntity()
                {
                    StockCode = stock.Code,
                    Target = 0,
                    Email = account.Email,
                    QQ = account.QQ,
                    RType = 10,
                    StrategyName = "主动设置",
                    StrategyTarget = $"MACD金叉"
                };
                reminds.Add(remind);
            }
            if(remindInfo.DieMacd)
            {
                Repository.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=11");

                var remind = new RemindEntity()
                {
                    StockCode = stock.Code,
                    Target = 0,
                    Email = account.Email,
                    QQ = account.QQ,
                    RType = 11,
                    StrategyName = "主动设置",
                    StrategyTarget = $"MACD死叉"
                };
                reminds.Add(remind);
            }

            if (remindInfo.UpMacd)
            {
                Repository.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=12");

                var remind = new RemindEntity()
                {
                    StockCode = stock.Code,
                    Target = 0,
                    Email = account.Email,
                    QQ = account.QQ,
                    RType = 12,
                    StrategyName = "主动设置",
                    StrategyTarget = $"MACD看多"
                };
                reminds.Add(remind);
            }
            if (remindInfo.DownMacd)
            {
                Repository.Instance.Delete<RemindEntity>($"StockCode='{stock.Code}' and StrategyName='主动设置' and RType=13");

                var remind = new RemindEntity()
                {
                    StockCode = stock.Code,
                    Target = 0,
                    Email = account.Email,
                    QQ = account.QQ,
                    RType = 13,
                    StrategyName = "主动设置",
                    StrategyTarget = $"MACD看空"
                };
                reminds.Add(remind);
            }
            Repository.Instance.Insert<RemindEntity>(reminds.ToArray());
        }
        public static void AutoCreate(string accountName)
        {
            var account = Repository.Instance.QueryFirst<AccountEntity>($"Name='{accountName}'");
            if (account == null) return;

            var reminds = new List<RemindEntity>();
            var stocks = Repository.Instance.QueryAll<StockEntity>($"Type=0 and Safety > 0 and Foucs>0");
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
            Repository.Instance.Delete<RemindEntity>($"StrategyName='自动设置' and RType=2");
            Repository.Instance.Insert<RemindEntity>(reminds.ToArray());
        }

        public static void Mark(string stockCode, int[] ids)
        {
            var reminds = Repository.Instance.QueryAll<RemindEntity>($"StockCode='{stockCode}' and Handled=0 and ID in ({string.Join(",", ids)})");
            foreach (var remind in reminds)
            {
                remind.Handled = 1;
            }
            Repository.Instance.Delete<RemindEntity>(reminds);
        }

        public static void Cancel(string stockCode, int[] ids)
        {
            Repository.Instance.Delete<RemindEntity>($"StockCode='{stockCode}'  and ID in ({string.Join(",", ids)})");
        }

        public static void CheckAutoRun(Action<string> action)
        {
            var reminds = Repository.Instance.QueryAll<RemindEntity>($"Handled=0", withNoLock: true);
            if (reminds.Length == 0) return;

            var dealDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            var stocks = Repository.Instance.QueryAll<StockEntity>($"Price>0 and PriceDate='{dealDate}' and Foucs>0");
            if (stocks.Length == 0) return;

            var stockAvgPrices = Repository.Instance.QueryAll<StockAverageEntity>($"");
            var stockMacds = Repository.Instance.QueryAll<StockMacdEntity>($"");

            foreach (var stock in stocks)
            {
                var remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 0 && Math.Abs(stock.UDPer) > c.Target && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date));
                if (remind != null)
                {
                    remind.LastRemind = DateTime.Now;
                    remind.RemindPrice = stock.Price;
                    remind.PlanRemind = DateTime.Now.Date.AddDays(1);
                    Repository.Instance.Update<RemindEntity>(remind, new string[] { "LastRemind", "RemindPrice", "PlanRemind" });

                    var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已{(stock.UDPer > 0 ? "上涨" : "下跌")}超过幅度[{remind.Target}%],请注意!";

                    if (RunningConfig.Instance.RemindNoticeByEmail)
                    {
                        MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                    }
                    if (RunningConfig.Instance.RemindNoticeByMessage)
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
                    Repository.Instance.Update<RemindEntity>(remind);
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
                    Repository.Instance.Update<RemindEntity>(remind);
                }

                var rds = reminds.Where(c => c.StockCode == stock.Code && c.RType == 3 && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date)).ToArray();
                foreach (var rd in rds)
                {
                    var avgPrice = stockAvgPrices.FirstOrDefault(c => c.StockCode == stock.Code && c.DealDate == dealDate);
                    if (avgPrice == null) continue;

                    var yestPrice = stockAvgPrices.Where(c => c.StockCode == stock.Code && c.DealDate.CompareTo(dealDate) < 0).OrderByDescending(c => c.DealDate).FirstOrDefault();
                    if (yestPrice == null) continue;

                    if ((rd.Target == 5 && stock.Price > avgPrice.AvgPrice10 && avgPrice.AvgPrice10 > 0)
                        || (rd.Target == 20 && stock.Price > avgPrice.AvgPrice20 && avgPrice.AvgPrice20 > 0)
                        || (rd.Target == 30 && stock.Price > avgPrice.AvgPrice30 && avgPrice.AvgPrice30 > 0)
                        || (rd.Target == 60 && stock.Price > avgPrice.AvgPrice60 && avgPrice.AvgPrice60 > 0)
                        || (rd.Target == 120 && stock.Price > avgPrice.AvgPrice120 && avgPrice.AvgPrice120 > 0)
                        || (rd.Target == 180 && stock.Price > avgPrice.AvgPrice180 && avgPrice.AvgPrice180 > 0)
                        || (rd.Target == 250 && stock.Price > avgPrice.AvgPrice250 && avgPrice.AvgPrice250 > 0)
                        &&
                        (rd.Target == 5 && yestPrice.Price <= yestPrice.AvgPrice5 && yestPrice.AvgPrice5 > 0)
                        || (rd.Target == 10 && yestPrice.Price <= yestPrice.AvgPrice10 && yestPrice.AvgPrice10 > 0)
                        || (rd.Target == 20 && yestPrice.Price <= yestPrice.AvgPrice20 && yestPrice.AvgPrice20 > 0)
                        || (rd.Target == 30 && yestPrice.Price <= yestPrice.AvgPrice30 && yestPrice.AvgPrice30 > 0)
                        || (rd.Target == 60 && yestPrice.Price <= yestPrice.AvgPrice60 && yestPrice.AvgPrice60 > 0)
                        || (rd.Target == 120 && yestPrice.Price <= yestPrice.AvgPrice120 && yestPrice.AvgPrice120 > 0)
                        || (rd.Target == 180 && yestPrice.Price <= yestPrice.AvgPrice180 && yestPrice.AvgPrice180 > 0)
                        || (rd.Target == 250 && yestPrice.Price <= yestPrice.AvgPrice250 && yestPrice.AvgPrice250 > 0))
                    {
                        var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已突破[{rd.Target}]日均线,请注意!";

                        if (RunningConfig.Instance.RemindNoticeByEmail)
                        {
                            MailUtil.SendMailAsync(new SenderMailConfig(), message, message, rd.Email);
                        }
                        if (RunningConfig.Instance.RemindNoticeByMessage)
                        {
                            StockMessageService.SendMessage(stock, message);
                        }
                        action(message);

                        rd.Handled = 1;
                        rd.LastRemind = DateTime.Now;
                        rd.RemindPrice = stock.Price;
                        Repository.Instance.Update<RemindEntity>(rd);
                    }
                }

                rds = reminds.Where(c => c.StockCode == stock.Code && c.RType == 4 && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date)).ToArray();
                foreach (var rd in rds)
                {
                    var avgPrice = stockAvgPrices.FirstOrDefault(c => c.StockCode == stock.Code && c.DealDate == dealDate);
                    if (avgPrice == null) continue;

                    var yestPrice = stockAvgPrices.Where(c => c.StockCode == stock.Code && c.DealDate.CompareTo(dealDate) < 0).OrderByDescending(c => c.DealDate).FirstOrDefault();
                    if (yestPrice == null) continue;

                    if ((rd.Target == 5 && stock.Price < avgPrice.AvgPrice5 && avgPrice.AvgPrice5 > 0)
                        || (rd.Target == 10 && stock.Price < avgPrice.AvgPrice10 && avgPrice.AvgPrice10 > 0)
                        || (rd.Target == 20 && stock.Price < avgPrice.AvgPrice20 && avgPrice.AvgPrice20 > 0)
                        || (rd.Target == 30 && stock.Price < avgPrice.AvgPrice30 && avgPrice.AvgPrice30 > 0)
                        || (rd.Target == 60 && stock.Price < avgPrice.AvgPrice60 && avgPrice.AvgPrice60 > 0)
                        || (rd.Target == 120 && stock.Price < avgPrice.AvgPrice120 && avgPrice.AvgPrice120 > 0)
                        || (rd.Target == 180 && stock.Price < avgPrice.AvgPrice180 && avgPrice.AvgPrice180 > 0)
                        || (rd.Target == 250 && stock.Price < avgPrice.AvgPrice250 && avgPrice.AvgPrice250 > 0)
                        &&
                        (rd.Target == 5 && yestPrice.Price >= yestPrice.AvgPrice5 && yestPrice.AvgPrice5 > 0)
                        || (rd.Target == 10 && yestPrice.Price >= yestPrice.AvgPrice10 && yestPrice.AvgPrice10 > 0)
                        || (rd.Target == 20 && yestPrice.Price >= yestPrice.AvgPrice20 && yestPrice.AvgPrice20 > 0)
                        || (rd.Target == 30 && yestPrice.Price >= yestPrice.AvgPrice30 && yestPrice.AvgPrice30 > 0)
                        || (rd.Target == 60 && yestPrice.Price >= yestPrice.AvgPrice60 && yestPrice.AvgPrice60 > 0)
                        || (rd.Target == 120 && yestPrice.Price >= yestPrice.AvgPrice120 && yestPrice.AvgPrice120 > 0)
                        || (rd.Target == 180 && yestPrice.Price >= yestPrice.AvgPrice180 && yestPrice.AvgPrice180 > 0)
                        || (rd.Target == 250 && yestPrice.Price >= yestPrice.AvgPrice250 && yestPrice.AvgPrice250 > 0))
                    {
                        var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已跌破[{rd.Target}]日均线,请注意!";

                        if (RunningConfig.Instance.RemindNoticeByEmail)
                        {
                            MailUtil.SendMailAsync(new SenderMailConfig(), message, message, rd.Email);
                        }
                        if (RunningConfig.Instance.RemindNoticeByMessage)
                        {
                            StockMessageService.SendMessage(stock, message);
                        }
                        action(message);

                        rd.Handled = 1;
                        rd.LastRemind = DateTime.Now;
                        rd.RemindPrice = stock.Price;
                        Repository.Instance.Update<RemindEntity>(rd);
                    }
                }

                //当前股价从最低价上涨比例
                if (stock.MinPrice > 0)
                {
                    var upPer = (stock.Price - stock.MinPrice) / stock.MinPrice;
                    if (upPer > 0 && stock.StartPrice > stock.MinPrice)
                    {
                        remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 5 && c.Target < upPer && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date));
                        if (remind != null)
                        {
                            remind.LastRemind = DateTime.Now;
                            remind.RemindPrice = stock.Price;
                            remind.PlanRemind = DateTime.Now.Date.AddDays(1);
                            Repository.Instance.Update<RemindEntity>(remind, new string[] { "LastRemind", "RemindPrice", "PlanRemind" });

                            var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已从低点反弹超过幅度[{remind.Target}%],请注意!";

                            if (RunningConfig.Instance.RemindNoticeByEmail)
                            {
                                MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                            }
                            if (RunningConfig.Instance.RemindNoticeByMessage)
                            {
                                StockMessageService.SendMessage(stock, message);
                            }
                            action(message);
                        }
                    }
                }

                if (stock.MaxPrice > 0)
                {
                    //当前股价从最高价下跌比例
                    var downPer = (stock.Price - stock.MaxPrice) / stock.MaxPrice;
                    if (downPer < 0 && stock.StartPrice < stock.MaxPrice)
                    {
                        remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 6 && c.Target < Math.Abs(downPer) && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date));
                        if (remind != null)
                        {
                            remind.LastRemind = DateTime.Now;
                            remind.RemindPrice = stock.Price;
                            remind.PlanRemind = DateTime.Now.Date.AddDays(1);
                            Repository.Instance.Update<RemindEntity>(remind, new string[] { "LastRemind", "RemindPrice", "PlanRemind" });

                            var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已从高点下跌超过幅度[{remind.Target}%],请注意!";

                            if (RunningConfig.Instance.RemindNoticeByEmail)
                            {
                                MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                            }
                            if (RunningConfig.Instance.RemindNoticeByMessage)
                            {
                                StockMessageService.SendMessage(stock, message);
                            }
                            action(message);
                        }
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
                    Repository.Instance.Update<RemindEntity>(remind);
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
                    Repository.Instance.Update<RemindEntity>(remind);
                }

                remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 10 && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date));
                if (remind != null)
                {
                    if (!ObjectUtil.EffectStockMacdTime()) continue;

                    var nowMacd = stockMacds.FirstOrDefault(c => c.StockCode == stock.Code && c.DealDate == dealDate);
                    var yestMacd = stockMacds.Where(c => c.StockCode == stock.Code && c.DealDate.CompareTo(dealDate) < 0).OrderByDescending(c => c.DealDate).FirstOrDefault();
                    if (yestMacd == null || nowMacd == null) continue;
                    //if (nowMacd.MACD >= 0 || yestMacd.MACD > 0) continue;

                    if (yestMacd.DIF < yestMacd.DEA && nowMacd.DIF >= nowMacd.DEA)
                    {
                        remind.LastRemind = DateTime.Now;
                        remind.RemindPrice = stock.Price;
                        remind.PlanRemind = DateTime.Now.Date.AddDays(1);
                        Repository.Instance.Update<RemindEntity>(remind, new string[] { "LastRemind", "RemindPrice", "PlanRemind" });

                        var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]MACD零轴{(nowMacd.MACD>=0?"上": "下")}金叉,请注意!";

                        if (RunningConfig.Instance.RemindNoticeByEmail)
                        {
                            MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                        }
                        if (RunningConfig.Instance.RemindNoticeByMessage)
                        {
                            StockMessageService.SendMessage(stock, message);
                        }
                        action(message);
                    }
                }

                remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 11 && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date));
                if (remind != null)
                {
                    if (!ObjectUtil.EffectStockMacdTime()) continue;

                    var nowMacd = stockMacds.FirstOrDefault(c => c.StockCode == stock.Code && c.DealDate == dealDate);
                    var yestMacd = stockMacds.Where(c => c.StockCode == stock.Code && c.DealDate.CompareTo(dealDate) < 0).OrderByDescending(c => c.DealDate).FirstOrDefault();
                    if (yestMacd == null || nowMacd == null) continue;
                    //if (nowMacd.MACD <= 0 || yestMacd.MACD <= 0) continue;

                    if (yestMacd.DIF >= yestMacd.DEA && nowMacd.DIF < nowMacd.DEA)
                    {
                        remind.LastRemind = DateTime.Now;
                        remind.RemindPrice = stock.Price;
                        remind.PlanRemind = DateTime.Now.Date.AddDays(1);
                        Repository.Instance.Update<RemindEntity>(remind, new string[] { "LastRemind", "RemindPrice", "PlanRemind" });

                        var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]MACD零轴{(nowMacd.MACD >= 0 ? "上" : "下")}死叉,请注意!";

                        if (RunningConfig.Instance.RemindNoticeByEmail)
                        {
                            MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                        }
                        if (RunningConfig.Instance.RemindNoticeByMessage)
                        {
                            StockMessageService.SendMessage(stock, message);
                        }
                        action(message);
                    }
                }

                remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 12 && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date));
                if (remind != null)
                {
                    if (!ObjectUtil.EffectStockMacdTime()) continue;

                    var nowMacd = stockMacds.FirstOrDefault(c => c.StockCode == stock.Code && c.DealDate == dealDate);
                    var yestMacd = stockMacds.Where(c => c.StockCode == stock.Code && c.DealDate.CompareTo(dealDate) < 0).OrderByDescending(c => c.DealDate).FirstOrDefault();
                    if (yestMacd == null || nowMacd == null) continue;
                    //if (nowMacd.MACD >= 0 || yestMacd.MACD > 0) continue;

                    if (yestMacd.MACD < nowMacd.MACD)
                    {
                        remind.Handled = 1;
                        remind.LastRemind = DateTime.Now;
                        remind.RemindPrice = stock.Price;
                        Repository.Instance.Update<RemindEntity>(remind);

                        var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]MACD零轴{(nowMacd.MACD >= 0 ? "上" : "下")}{(nowMacd.MACD >= 0 && yestMacd.MACD < 0 ? "转势" : "")}看多,请注意!";

                        if (RunningConfig.Instance.RemindNoticeByEmail)
                        {
                            MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                        }
                        if (RunningConfig.Instance.RemindNoticeByMessage)
                        {
                            StockMessageService.SendMessage(stock, message);
                        }
                        action(message);
                    }
                }

                remind = reminds.FirstOrDefault(c => c.StockCode == stock.Code && c.RType == 13 && (!c.PlanRemind.HasValue || c.PlanRemind <= DateTime.Now.Date));
                if (remind != null)
                {
                    if (!ObjectUtil.EffectStockMacdTime()) continue;

                    var nowMacd = stockMacds.FirstOrDefault(c => c.StockCode == stock.Code && c.DealDate == dealDate);
                    var yestMacd = stockMacds.Where(c => c.StockCode == stock.Code && c.DealDate.CompareTo(dealDate) < 0).OrderByDescending(c => c.DealDate).FirstOrDefault();
                    if (yestMacd == null || nowMacd == null) continue;
                    //if (nowMacd.MACD >= 0 || yestMacd.MACD > 0) continue;

                    if (yestMacd.MACD > nowMacd.MACD)
                    {
                        remind.Handled = 1;
                        remind.LastRemind = DateTime.Now;
                        remind.RemindPrice = stock.Price;
                        Repository.Instance.Update<RemindEntity>(remind);

                        var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]MACD零轴{(nowMacd.MACD >= 0 ? "上" : "下")}{(nowMacd.MACD < 0 && yestMacd.MACD >= 0 ? "转势" : "")}看空,请注意!";

                        if (RunningConfig.Instance.RemindNoticeByEmail)
                        {
                            MailUtil.SendMailAsync(new SenderMailConfig(), message, message, remind.Email);
                        }
                        if (RunningConfig.Instance.RemindNoticeByMessage)
                        {
                            StockMessageService.SendMessage(stock, message);
                        }
                        action(message);
                    }
                }
            }
        }

        public static void Clear(string stockCode = "", bool clearAll = false)
        {
            if (clearAll)
            {
                if (string.IsNullOrEmpty(stockCode))
                {
                    Repository.Instance.Delete<RemindEntity>($"ID>0");
                }
                else
                {
                    Repository.Instance.Delete<RemindEntity>($"StockCode='{stockCode}'");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(stockCode))
                {
                    Repository.Instance.Delete<RemindEntity>($"Handled=1");
                }
                else
                {
                    Repository.Instance.Delete<RemindEntity>($"StockCode='{stockCode}' and Handled=1");
                }
            }
        }
    }
}
