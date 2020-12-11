using StockPriceTools.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceTools.Service
{
    public class StockExchangeService : IStockExchangeService
    {
        public StockExchangeService(double amount)
        {
            StockAccount = new AccountEntity()
            {
                Amount = amount
            };
            ExchangeOrders = new List<ExchangeOrderEntity>();
            Stragegys = new List<StrategyEntity>();
            Stragegys.Add(new StrategyEntity()
            {
                Name = "默认策略",
                IncreasePer = -5,
                IncreaseAmount = 5000,
                MaxPositionPer = 25,
                IncreaseMorePer = -20,
                IncreaseMoreAmountPer = 30,
                IncreaseMaxPer = -30,
                IncreaseMaxAmountPer = 50,
            });
        }

        private AccountEntity StockAccount { get; set; }

        protected List<ExchangeOrderEntity> ExchangeOrders { get; set; }

        protected List<StrategyEntity> Stragegys { get; set; }

        public void Buy(ExchangeOrderEntity order)
        {
            var orders = this.ExchangeOrders.Where(c => c.StockCode == order.StockCode).ToArray();
            if (orders.Length == 0)
            {
                order.HoldQty = order.Qty;
                order.HoldAmount = order.Amount;
                order.Profit = 0;
                this.ExchangeOrders.Add(order);
                return;
            }
            var lastOrder = orders.LastOrDefault();
            var stragegy = Stragegys.FirstOrDefault(c => c.Name == order.Strategy);
            if (stragegy == null) throw new Exception($"未设置买入策略");
        }

        public void Sale(ExchangeOrderEntity order)
        {
            this.ExchangeOrders.Add(order);
        }
    }
}
