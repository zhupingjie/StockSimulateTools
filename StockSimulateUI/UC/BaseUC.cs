using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockSimulateCore.Utils;
using StockSimulateCore.Model;

namespace StockSimulateUI.UC
{
    public partial class BaseUC : UserControl
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string StrategyName { get; set; }

        public BaseUC()
        {
            InitializeComponent();
        }

        public virtual void CalcuateStrategy()
        {

        }

        public virtual StrategyEntity GetStrategyInfo()
        {
            return null;
        }
    }
}
