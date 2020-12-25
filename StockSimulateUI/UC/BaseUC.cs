using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockSimulateNetService.Utils;
using StockSimulateDomain.Model;
using StockSimulateNetService.Service;
using StockSimulateCoreService.Utils;

namespace StockSimulateUI.UC
{
    public partial class BaseUC : UserControl
    {
        private MySQLDBUtil Repository = MySQLDBUtil.Instance;
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string StrategyName { get; set; }

        public BaseUC()
        {
            InitializeComponent();
        }

        public virtual void CalcuateStrategy(string accountName, string strategyName, string stockCode)
        {

        }

        public virtual StrategyInfo GetStrategyInfo()
        {
            return null;
        }
    }
}
