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

            var buyAmount = exchangeInfo.Qty * exchangeInfo.Price;
            if(buyAmount > account.Cash)
            {
                result.Success = false;
                result.Message = $"交易账户[{exchangeInfo.AccountName}]余额不足[{buyAmount - account.Cash}]";
                return result;
            }

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
                ExchangeTime = DateTime.Now,
            };
            if (string.IsNullOrEmpty(exchange.Strategy)) exchange.Strategy = "临时";
            if (string.IsNullOrEmpty(exchange.Target)) exchange.Target = $"{(stock.UDPer > 0 ? "上涨" : "下跌")}{stock.UDPer}%";

            SQLiteDBUtil.Instance.Insert<ExchangeOrderEntity>(exchange);

            account.BuyAmount += exchange.Amount;
            account.Cash -= exchange.Amount;
            SQLiteDBUtil.Instance.Update<AccountEntity>(account);

            var accountStock = SQLiteDBUtil.Instance.QueryFirst<AccountStockEntity>($"AccountName='{exchangeInfo.AccountName}' and StockCode='{exchangeInfo.StockCode}'");
            if (accountStock == null)
            {
                accountStock = new AccountStockEntity()
                {
                    AccountName = account.Name,
                    StockCode = stock.Code,
                    StockName = stock.Name,
                    HoldQty = exchangeInfo.Qty,
                    TotalBuyAmount = exchangeInfo.Qty * exchangeInfo.Price,
                    Cost = exchangeInfo.Price
                };
                SQLiteDBUtil.Instance.Insert<AccountStockEntity>(accountStock);
            }
            else
            {
                accountStock.HoldQty += exchange.Qty;
                accountStock.TotalBuyAmount += exchange.Amount;
                accountStock.Cost = Math.Round(accountStock.TotalBuyAmount / accountStock.HoldQty, 2);
                SQLiteDBUtil.Instance.Update<AccountStockEntity>(accountStock);
            }
            var message = $"交易账户[{account.Name}]按策略[{exchange.Strategy}-{exchange.Target}]买入[{stock.Name}]{exchange.Qty}股({exchange.Price})合计{exchange.Amount}元,请注意!";
            MailUtil.SendMailAsync(new Config.SenderMailConfig(), message, message, account.Email);
            return result;
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

            var accountStock = SQLiteDBUtil.Instance.QueryFirst<AccountStockEntity>($"AccountName='{exchangeInfo.AccountName}' and StockCode='{exchangeInfo.StockCode}'");
            if (accountStock == null)
            {
                result.Success = false;
                result.Message = $"交易账户[{exchangeInfo.AccountName}]未持有股票代码[{exchangeInfo.StockCode}]";
                return result;
            }

            var couldQty = accountStock.HoldQty;
            if (accountStock.LockDate.HasValue && accountStock.LockDate == DateTime.Now.Date) couldQty -= accountStock.LockQty;
            if (exchangeInfo.Qty > couldQty)
            {
                result.Success = false;
                result.Message = $"交易账户[{exchangeInfo.AccountName}]持有股票不足[{exchangeInfo.Qty - couldQty}]";
                return result;
            }

            var saleAmount = exchangeInfo.Qty * exchangeInfo.Price;
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
                ExchangeTime = DateTime.Now,
                HoldQty = accountStock.HoldQty - exchangeInfo.Qty
            };
            if (string.IsNullOrEmpty(exchange.Strategy)) exchange.Strategy = "临时";
            if (string.IsNullOrEmpty(exchange.Target)) exchange.Target = $"{(stock.UDPer > 0 ? "上涨" : "下跌")}{stock.UDPer}%";

            SQLiteDBUtil.Instance.Insert<ExchangeOrderEntity>(exchange);

            account.BuyAmount -= exchange.Amount;
            account.Cash += exchange.Amount;
            SQLiteDBUtil.Instance.Update<AccountEntity>(account);

            accountStock.HoldQty -= exchange.Qty;
            accountStock.TotalBuyAmount -= exchange.Amount;
            if (accountStock.HoldQty == 0)
            {
                accountStock.Cost = 0;
            }
            else
            {
                accountStock.Cost = Math.Round(accountStock.TotalBuyAmount / accountStock.HoldQty, 2);
            }
            SQLiteDBUtil.Instance.Update<AccountStockEntity>(accountStock);

            var message = $"交易账户[{account.Name}]按策略[{exchange.Strategy}-{exchange.Target}]卖出[{stock.Name}][{exchange.Qty}]股({exchange.Price})合计{exchange.Amount}元,请注意!";
            MailUtil.SendMailAsync(new Config.SenderMailConfig(), message, message, account.Email);
            return result;
        }
    }
}
