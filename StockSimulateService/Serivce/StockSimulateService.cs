using StockSimulateNetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace StockSimulateService.Service
{
    partial class StockSimulateService : ServiceBase
    {
        public StockSimulateService()
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
