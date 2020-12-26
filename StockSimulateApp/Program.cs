using StockSimulateService.Utils;
using StockSimulateService.Service;
using System;
using System.ServiceProcess;

namespace StockSimulateService
{
    class Program
    {
        static void Main(string[] args)
        {
            LogUtil.Debug($"程序开始启动...");
#if DEBUG
            StockMarketService.Instance.Start();
            while (true)
            {
                var input = Console.ReadLine();
                if (input.Equals("exit"))
                {
                    LogUtil.Debug($"程序准备结束运行...");
                    break;
                }
            }
#else
            ServiceBase[] services = new ServiceBase[] { new StockSimulateApp() };
            ServiceBase.Run(services);
#endif
            LogUtil.Debug($"程序结束运行...");
        }
    }
}
