using StockSimulateNetCore;
using StockSimulateService.Service;
using System;
using System.ServiceProcess;

namespace StockSimulateService
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
        StockMarketService.Instance.Start();
         Console.ReadKey();
#else
            ServiceBase[] services = new ServiceBase[] { new StockSimulateService() };
            ServiceBase.Run(services);
#endif
        }
    }
}
