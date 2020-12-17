using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockSimulateCore.Model;

namespace StockSimulateUI.UC
{
    public partial class TExchangeUC : BaseUC
    {
        public TExchangeUC()
        {
            InitializeComponent();
        }

        public override void CalcuateStrategy()
        {
            base.CalcuateStrategy();
        }

        public override StrategyEntity GetStrategyInfo()
        {
            return base.GetStrategyInfo();
        }
    }
}
