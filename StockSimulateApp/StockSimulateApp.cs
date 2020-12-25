using StockSimulateNetCore.Service;
using System.ServiceProcess;

namespace StockSimulateService.Service
{
    partial class StockSimulateApp : ServiceBase
    {
        public StockSimulateApp()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StockMarketService.Instance.Start();
        }

        protected override void OnStop()
        {
            StockMarketService.Instance.Stop();
        }
    }
}
