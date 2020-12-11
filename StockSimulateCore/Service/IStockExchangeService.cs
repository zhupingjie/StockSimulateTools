using StockSimulateCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Service
{
    public interface IStockExchangeService
    {
        void Buy(ExchangeOrderEntity order);

        void Sale(ExchangeOrderEntity order);
    }
}
