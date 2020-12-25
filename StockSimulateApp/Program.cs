using StockSimulateDomain.Utils;
using StockSimulateNetCore.Service;
using StockSimulateNetCore.Utils;
using System;
using System.ServiceProcess;

namespace StockSimulateService
{
    class Program
    {
        static void Main(string[] args)
        {
            LogUtil.Debug($"程序开始启动...");
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
            LogUtil.Debug($"程序结束运行...");
            //ServiceBase[] services = new ServiceBase[] { new Service.StockSimulateService() };
            //ServiceBase.Run(services);
        }
    }
}
