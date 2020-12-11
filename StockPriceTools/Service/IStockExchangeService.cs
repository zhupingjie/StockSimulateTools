using StockPriceTools.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceTools.Service
{
    public interface IStockExchangeService
    {
        void Buy(ExchangeOrderEntity order);

        void Sale(ExchangeOrderEntity order);
    }
}
