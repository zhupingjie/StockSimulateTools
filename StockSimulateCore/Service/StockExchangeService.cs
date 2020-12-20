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
    public class StockExchangeService
    {
        public static bool CouldBuy(ExchangeInfo exchangeInfo)
        {
            var account = SQLiteDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{exchangeInfo.AccountName}'");
            if (account == null) return false;

            var stock = SQLiteDBUtil.Instance.QueryFirst<StockEntity>($"Code='{exchangeInfo.StockCode}'");
            if (stock == null) return false;

            var buyAmount = exchangeInfo.Qty * exchangeInfo.Price;
            if (buyAmount > account.Cash) return false;

            return true;
        }

        public static ExchangeResultInfo Buy(ExchangeInfo exchangeInfo)
        {
            var result = new ExchangeResultInfo();

            var account = SQLiteDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{exchangeInfo.AccountName}'");
            if(account == null)
            {
                result.Success = false;
                result.Message = $"交易账户[{exchangeInfo.AccountName}]不存在";
                return result;
            }

            var stock = SQLiteDBUtil.Instance.QueryFirst<StockEntity>($"Code='{exchangeInfo.StockCode}'");
            if (stock == null)
            {
                result.Success = false;
                result.Message = $"股票代码[{exchangeInfo.StockCode}]不存在";
                return result;
            }
            var decNum = stock.Type == 0 ? 2 : 3;
            var buyAmount = exchangeInfo.Qty * exchangeInfo.Price;
            if(buyAmount > account.Cash)
            {
                result.Success = false;
                result.Message = $"交易账户[{exchangeInfo.AccountName}]余额不足[{buyAmount - account.Cash}]";
                return result;
            }

            var accountStock = SQLiteDBUtil.Instance.QueryFirst<AccountStockEntity>($"AccountName='{exchangeInfo.AccountName}' and StockCode='{exchangeInfo.StockCode}'");
            var holdQty = exchangeInfo.Qty;
            var totalAmount = buyAmount;
            if (accountStock != null)
            {
                holdQty += accountStock.HoldQty;
                totalAmount += accountStock.TotalAmount;
            }
            var cost = Math.Round(totalAmount / holdQty, decNum);
            var exchange = new ExchangeOrderEntity()
            {
                AccountName = exchangeInfo.AccountName,
                StockCode = exchangeInfo.StockCode,
                Strategy = exchangeInfo.StrategyName,
                Target = exchangeInfo.Target,
                Qty = exchangeInfo.Qty,
                Price = exchangeInfo.Price,
                ExchangeType = "买入",
                Amount = buyAmount,
                HoldQty = holdQty,
                Cost = cost,
                ExchangeTime = exchangeInfo.ExchangeTime,
            };
            if (string.IsNullOrEmpty(exchange.Strategy)) exchange.Strategy = "临时";
            if (string.IsNullOrEmpty(exchange.Target)) exchange.Target = $"{(stock.UDPer > 0 ? "上涨" : "下跌")}{stock.UDPer}%";

            SQLiteDBUtil.Instance.Insert<ExchangeOrderEntity>(exchange);

            account.BuyAmount += exchange.Amount;
            account.Cash -= exchange.Amount;
            SQLiteDBUtil.Instance.Update<AccountEntity>(account);

            if (accountStock == null)
            {
                accountStock = new AccountStockEntity()
                {
                    AccountName = account.Name,
                    StockCode = stock.Code,
                    StockName = stock.Name,
                    HoldQty = exchangeInfo.Qty,
                    TotalAmount = exchangeInfo.Qty * exchangeInfo.Price,
                    Cost = exchangeInfo.Price
                };
                if (stock.LockDay > 0)
                {
                    accountStock.LockQty = exchangeInfo.Qty;
                    accountStock.LockDate = exchangeInfo.ExchangeTime.Date;
                }
                SQLiteDBUtil.Instance.Insert<AccountStockEntity>(accountStock);
            }
            else
            {
                accountStock.HoldQty += exchange.Qty;
                accountStock.TotalAmount += exchange.Amount;
                accountStock.Cost = Math.Round(accountStock.TotalAmount / accountStock.HoldQty, decNum);
                if (stock.LockDay > 0)
                {
                    if (accountStock.LockDate.HasValue)
                    {
                        if(accountStock.LockDate < exchangeInfo.ExchangeTime.Date)
                        {
                            accountStock.LockQty  = exchangeInfo.Qty;
                            accountStock.LockDate = exchangeInfo.ExchangeTime.Date;
                        }
                        else 
                        {
                            accountStock.LockQty += exchangeInfo.Qty;
                        }
                    }
                }
                SQLiteDBUtil.Instance.Update<AccountStockEntity>(accountStock);
            }
            var message = $"交易账户[{account.Name}]按策略[{exchange.Strategy}-{exchange.Target}]买入[{stock.Name}]{exchange.Qty}股({exchange.Price})合计{exchange.Amount}元,请注意!";
            MailUtil.SendMailAsync(new Config.SenderMailConfig(), message, message, account.Email);
            return result;
        }

        public static bool CouldSale(ExchangeInfo exchangeInfo)
        {
            var account = SQLiteDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{exchangeInfo.AccountName}'");
            if (account == null) return false;

            var stock = SQLiteDBUtil.Instance.QueryFirst<StockEntity>($"Code='{exchangeInfo.StockCode}'");
            if (stock == null) return false;

            var accountStock = SQLiteDBUtil.Instance.QueryFirst<AccountStockEntity>($"AccountName='{exchangeInfo.AccountName}' and StockCode='{exchangeInfo.StockCode}'");
            if (accountStock == null) return false;

            var couldQty = accountStock.HoldQty;
            if (accountStock.LockDate.HasValue && accountStock.LockDate == exchangeInfo.ExchangeTime.Date) couldQty -= accountStock.LockQty;
            if (exchangeInfo.Qty > couldQty) return false;

            return true;
        }

        public static ExchangeResultInfo Sale(ExchangeInfo exchangeInfo)
        {
            var result = new ExchangeResultInfo();

            var account = SQLiteDBUtil.Instance.QueryFirst<AccountEntity>($"Name='{exchangeInfo.AccountName}'");
            if (account == null)
            {
                result.Success = false;
                result.Message = $"交易账户[{exchangeInfo.AccountName}]不存在";
                return result;
            }

            var stock = SQLiteDBUtil.Instance.QueryFirst<StockEntity>($"Code='{exchangeInfo.StockCode}'");
            if (stock == null)
            {
                result.Success = false;
                result.Message = $"股票代码[{exchangeInfo.StockCode}]不存在";
                return result;
            }

            var decNum = stock.Type == 0 ? 2 : 3;
            var accountStock = SQLiteDBUtil.Instance.QueryFirst<AccountStockEntity>($"AccountName='{exchangeInfo.AccountName}' and StockCode='{exchangeInfo.StockCode}'");
            if (accountStock == null)
            {
                result.Success = false;
                result.Message = $"交易账户[{exchangeInfo.AccountName}]未持有股票代码[{exchangeInfo.StockCode}]";
                return result;
            }

            var couldQty = accountStock.HoldQty;
            if (accountStock.LockDate.HasValue && accountStock.LockDate == exchangeInfo.ExchangeTime.Date) couldQty -= accountStock.LockQty;
            if (exchangeInfo.Qty > couldQty)
            {
                result.Success = false;
                result.Message = $"交易账户[{exchangeInfo.AccountName}]持有股票不足[{exchangeInfo.Qty - couldQty}]";
                return result;
            }

            var saleAmount = exchangeInfo.Qty * exchangeInfo.Price;
            var totalAmount = accountStock.TotalAmount - saleAmount;
            var cost = 0m;
            var holdQty = accountStock.HoldQty - exchangeInfo.Qty;
            if (holdQty > 0) cost = Math.Round(totalAmount / holdQty, decNum);

            var exchange = new ExchangeOrderEntity()
            {
                AccountName = account.Name,
                StockCode = exchangeInfo.StockCode,
                Strategy = exchangeInfo.StrategyName,
                Target = exchangeInfo.Target,
                Qty = exchangeInfo.Qty,
                Price = exchangeInfo.Price,
                ExchangeType = "卖出",
                Amount = saleAmount,
                HoldQty = holdQty,
                Cost = cost,
                ExchangeTime = exchangeInfo.ExchangeTime
            };
            if (string.IsNullOrEmpty(exchange.Strategy)) exchange.Strategy = "临时";
            if (string.IsNullOrEmpty(exchange.Target)) exchange.Target = $"{(stock.UDPer > 0 ? "上涨" : "下跌")}{stock.UDPer}%";

            SQLiteDBUtil.Instance.Insert<ExchangeOrderEntity>(exchange);

            account.BuyAmount -= exchange.Amount;
            account.Cash += exchange.Amount;
            SQLiteDBUtil.Instance.Update<AccountEntity>(account);

            accountStock.HoldQty  = holdQty;
            accountStock.TotalAmount = totalAmount;
            if (accountStock.HoldQty == 0)
            {
                accountStock.Cost = 0;
            }
            else
            {
                accountStock.Cost = Math.Round(accountStock.TotalAmount / accountStock.HoldQty, 2);
            }
            SQLiteDBUtil.Instance.Update<AccountStockEntity>(accountStock);

            var message = $"交易账户[{account.Name}]按策略[{exchange.Strategy}-{exchange.Target}]卖出[{stock.Name}][{exchange.Qty}]股({exchange.Price})合计{exchange.Amount}元,请注意!";
            MailUtil.SendMailAsync(new Config.SenderMailConfig(), message, message, account.Email);
            return result;
        }
    }
}
